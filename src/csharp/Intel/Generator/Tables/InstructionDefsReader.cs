// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using Generator.Constants.InstructionInfo;
using Generator.Enums;
using Generator.Enums.Encoder;
using Generator.Enums.Formatter;
using Generator.Enums.InstructionInfo;
using Generator.Formatters;
using Generator.InstructionInfo;
using Generator.IO;

namespace Generator.Tables {
	sealed class InstructionDefsReader {
		readonly StringBuilder sb;
		readonly GenTypes genTypes;
		readonly RegisterDef[] regDefs;
		readonly MemorySizeDefs memSizeTbl;
		readonly EnumValue tupleTypeN1;
		readonly string filename;
		readonly string[] lines;
		readonly List<string> errors;
		readonly ImpliedAccessParser impliedAccessParser;
		readonly Dictionary<string, int> usedCodeValues;
		readonly Dictionary<string, EnumValue> toCode;
		readonly Dictionary<string, EnumValue> toMnemonic;
		readonly Dictionary<string, EnumValue> toEncoding;
		readonly Dictionary<string, EnumValue> toCpuid;
		readonly Dictionary<string, EnumValue> toTupleType;
		readonly Dictionary<string, EnumValue> toMvexTupleTypeLutKind;
		readonly Dictionary<string, EnumValue> toConditionCode;
		readonly Dictionary<string, EnumValue> toPseudoOpsKind;
		readonly Dictionary<string, EnumValue> toDecOptionValue;
		readonly Dictionary<string, EnumValue> toMemorySize;
		readonly Dictionary<string, EnumValue> toRegisterIgnoreCase;
		readonly Dictionary<OpKindKey, OpCodeOperandKindDef> toOpCodeOperandKindDef;
		readonly EnumType fastFmtFlags;
		readonly EnumType gasCtorKind;
		readonly EnumType intelCtorKind;
		readonly EnumType masmCtorKind;
		readonly EnumType nasmCtorKind;
		readonly EnumType gasInstrOpInfoFlags;
		readonly EnumType intelInstrOpInfoFlags;
		readonly EnumType masmInstrOpInfoFlags;
		readonly EnumType nasmInstrOpInfoFlags;
		readonly EnumType registerType;
		readonly EnumType codeSizeType;
		readonly EnumType signExtendInfoType;
		readonly EnumType flowControlType;
		readonly EnumValue memorySizeUnknown;
		readonly EnumValue flowControlNext;
		readonly EnumValue decoderOptionNone;
		readonly Dictionary<EnumValue, string> toCpuidFeatureString;
		readonly List<OpInfo> opAccess;
		readonly List<OpCodeOperandKindDef> opKinds;
		readonly List<(string key, string value, int fmtLineIndex)> fmtKeyValues;
		const string DefBeginPrefix = "INSTRUCTION:";
		const string DefEnd = "END";

		static readonly HashSet<string> flagsMustHaveKeyValue = new(StringComparer.Ordinal) {
			"vmx",
			"fpu-push",
			"fpu-cond-push",
			"fpu-pop",
			"fpu-stack",
			"sp",
			"cc",
			"br",
			"cflow",
			"dec-opt",
			"pseudo",
			"asm",
		};

		readonly struct OpKindKey : IEquatable<OpKindKey> {
			readonly OpCodeOperandKindDefFlags flags;
			readonly OperandEncoding encoding;
			readonly Register register;
			readonly int size1;
			readonly int size2;

			public OpKindKey(OpCodeOperandKindDef def) {
				flags = def.Flags & ~OpCodeOperandKindDefFlags.Modrm;
				encoding = def.OperandEncoding;
				register = def.Register;
				size1 = def.Arg1;
				size2 = def.Arg2;
			}

			public OpKindKey(OperandEncoding encoding, ParsedInstructionOperandFlags opFlags, Register register, int size1, int size2, OpCodeOperandKindDefFlags flags) {
				if ((opFlags & ParsedInstructionOperandFlags.RegPlus1) != 0)
					flags |= OpCodeOperandKindDefFlags.RegPlus1;
				if ((opFlags & ParsedInstructionOperandFlags.RegPlus3) != 0)
					flags |= OpCodeOperandKindDefFlags.RegPlus3;
				if ((opFlags & ParsedInstructionOperandFlags.Memory) != 0)
					flags |= OpCodeOperandKindDefFlags.Memory;
				if ((opFlags & ParsedInstructionOperandFlags.MIB) != 0)
					flags |= OpCodeOperandKindDefFlags.MIB;
				if ((opFlags & ParsedInstructionOperandFlags.Sibmem) != 0)
					flags |= OpCodeOperandKindDefFlags.SibRequired;
				if ((opFlags & ParsedInstructionOperandFlags.Vsib) != 0) {
					flags |= size1 switch {
						32 => OpCodeOperandKindDefFlags.Vsib32,
						64 => OpCodeOperandKindDefFlags.Vsib64,
						_ => throw new InvalidOperationException(),
					};
					size1 = 0;
				}
				this.flags = flags;
				this.encoding = encoding;
				this.register = register;
				this.size1 = size1;
				this.size2 = size2;
			}

			public override bool Equals(object? obj) => obj is OpKindKey key && Equals(key);
			public bool Equals(OpKindKey other) => flags == other.flags && encoding == other.encoding && register == other.register && size1 == other.size1 && size2 == other.size2;
			public override int GetHashCode() => HashCode.Combine(flags, encoding, register, size1, size2);
		}

		public InstructionDefsReader(GenTypes genTypes, string filename) {
			sb = new StringBuilder();
			this.genTypes = genTypes;
			regDefs = genTypes.GetObject<RegisterDefs>(TypeIds.RegisterDefs).Defs;
			memSizeTbl = genTypes.GetObject<MemorySizeDefs>(TypeIds.MemorySizeDefs);
			this.filename = filename;
			lines = File.ReadAllLines(filename);
			errors = new List<string>();
			opAccess = new List<OpInfo>();
			opKinds = new List<OpCodeOperandKindDef>();
			fmtKeyValues = new List<(string key, string value, int fmtLineIndex)>();
			// Ignore case because two Code values with different casing is just too confusing
			usedCodeValues = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
			impliedAccessParser = new ImpliedAccessParser(genTypes);

			toCode = CreateEnumDict(genTypes[TypeIds.Code]);
			toMnemonic = CreateEnumDict(genTypes[TypeIds.Mnemonic]);
			toEncoding = CreateEnumDict(genTypes[TypeIds.EncodingKind]);
			toCpuid = CreateEnumDict(genTypes[TypeIds.CpuidFeature]);
			toTupleType = CreateEnumDict(genTypes[TypeIds.TupleType]);
			toMvexTupleTypeLutKind = CreateEnumDict(genTypes[TypeIds.MvexTupleTypeLutKind]);
			toConditionCode = CreateEnumDict(genTypes[TypeIds.ConditionCode]);
			toPseudoOpsKind = CreateEnumDict(genTypes[TypeIds.PseudoOpsKind]);
			toDecOptionValue = CreateEnumDict(genTypes[TypeIds.DecOptionValue]);
			toMemorySize = CreateEnumDict(genTypes[TypeIds.MemorySize]);
			toRegisterIgnoreCase = CreateEnumDict(genTypes[TypeIds.Register], ignoreCase: true);

			var opKindDefs = genTypes.GetObject<OpCodeOperandKindDefs>(TypeIds.OpCodeOperandKindDefs).Defs;
			toOpCodeOperandKindDef = opKindDefs.ToDictionary(a => new OpKindKey(a), a => a);

			fastFmtFlags = genTypes[TypeIds.FastFmtFlags];
			gasCtorKind = genTypes[TypeIds.GasCtorKind];
			intelCtorKind = genTypes[TypeIds.IntelCtorKind];
			masmCtorKind = genTypes[TypeIds.MasmCtorKind];
			nasmCtorKind = genTypes[TypeIds.NasmCtorKind];
			gasInstrOpInfoFlags = genTypes[TypeIds.GasInstrOpInfoFlags];
			intelInstrOpInfoFlags = genTypes[TypeIds.IntelInstrOpInfoFlags];
			masmInstrOpInfoFlags = genTypes[TypeIds.MasmInstrOpInfoFlags];
			nasmInstrOpInfoFlags = genTypes[TypeIds.NasmInstrOpInfoFlags];
			registerType = genTypes[TypeIds.Register];
			codeSizeType = genTypes[TypeIds.CodeSize];
			signExtendInfoType = genTypes[TypeIds.NasmSignExtendInfo];
			flowControlType = genTypes[TypeIds.FlowControl];

			toCpuidFeatureString = CreateCpuidFeatureStrings(genTypes);

			tupleTypeN1 = toTupleType[nameof(TupleType.N1)];
			memorySizeUnknown = toMemorySize[nameof(MemorySize.Unknown)];
			flowControlNext = flowControlType[nameof(FlowControl.Next)];
			decoderOptionNone = toDecOptionValue[nameof(DecOptionValue.None)];
		}

		static Dictionary<EnumValue, string> CreateCpuidFeatureStrings(GenTypes genTypes) {
			var cpuid = genTypes[TypeIds.CpuidFeature];
			var toCpuidName = cpuid.Values.ToDictionary(a => a, a => a.RawName);
			toCpuidName[cpuid[nameof(CpuidFeature.INTEL8086)]] = "8086+";
			toCpuidName[cpuid[nameof(CpuidFeature.INTEL8086_ONLY)]] = "8086";
			toCpuidName[cpuid[nameof(CpuidFeature.INTEL186)]] = "186+";
			toCpuidName[cpuid[nameof(CpuidFeature.INTEL286)]] = "286+";
			toCpuidName[cpuid[nameof(CpuidFeature.INTEL286_ONLY)]] = "286";
			toCpuidName[cpuid[nameof(CpuidFeature.INTEL386)]] = "386+";
			toCpuidName[cpuid[nameof(CpuidFeature.INTEL386_ONLY)]] = "386";
			toCpuidName[cpuid[nameof(CpuidFeature.INTEL386_A0_ONLY)]] = "386 A0";
			toCpuidName[cpuid[nameof(CpuidFeature.INTEL486)]] = "486+";
			toCpuidName[cpuid[nameof(CpuidFeature.INTEL486_A_ONLY)]] = "486 A";
			toCpuidName[cpuid[nameof(CpuidFeature.SMM)]] = "386+";
			toCpuidName[cpuid[nameof(CpuidFeature.UMOV)]] = "386/486";
			toCpuidName[cpuid[nameof(CpuidFeature.MOV_TR)]] = "386/486/Cyrix/Geode";
			toCpuidName[cpuid[nameof(CpuidFeature.IA64)]] = "IA-64";
			toCpuidName[cpuid[nameof(CpuidFeature.FPU)]] = "8087+";
			toCpuidName[cpuid[nameof(CpuidFeature.FPU287)]] = "287+";
			toCpuidName[cpuid[nameof(CpuidFeature.FPU287XL_ONLY)]] = "287 XL";
			toCpuidName[cpuid[nameof(CpuidFeature.FPU387)]] = "387+";
			toCpuidName[cpuid[nameof(CpuidFeature.FPU387SL_ONLY)]] = "387 SL";
			toCpuidName[cpuid[nameof(CpuidFeature.CYRIX_D3NOW)]] = "AMD Geode GX/LX";
			toCpuidName[cpuid[nameof(CpuidFeature.HLE_or_RTM)]] = "HLE or RTM";
			toCpuidName[cpuid[nameof(CpuidFeature.SEV_ES)]] = "SEV-ES";
			toCpuidName[cpuid[nameof(CpuidFeature.SEV_SNP)]] = "SEV-SNP";
			toCpuidName[cpuid[nameof(CpuidFeature.SKINIT_or_SVM)]] = "SKINIT or SVM";
			toCpuidName[cpuid[nameof(CpuidFeature.INVEPT)]] = "IA32_VMX_EPT_VPID_CAP[bit 20]";
			toCpuidName[cpuid[nameof(CpuidFeature.INVVPID)]] = "IA32_VMX_EPT_VPID_CAP[bit 32]";
			toCpuidName[cpuid[nameof(CpuidFeature.MULTIBYTENOP)]] = "CPUID.01H.EAX[Bits 11:8] = 0110B or 1111B";
			toCpuidName[cpuid[nameof(CpuidFeature.PAUSE)]] = "Pentium 4 or later";
			toCpuidName[cpuid[nameof(CpuidFeature.RDPMC)]] = "Pentium MMX or later, or Pentium Pro or later";
			toCpuidName[cpuid[nameof(CpuidFeature.D3NOW)]] = "3DNOW";
			toCpuidName[cpuid[nameof(CpuidFeature.D3NOWEXT)]] = "3DNOWEXT";
			toCpuidName[cpuid[nameof(CpuidFeature.SSE4_1)]] = "SSE4.1";
			toCpuidName[cpuid[nameof(CpuidFeature.SSE4_2)]] = "SSE4.2";
			toCpuidName[cpuid[nameof(CpuidFeature.AMX_BF16)]] = "AMX-BF16";
			toCpuidName[cpuid[nameof(CpuidFeature.AMX_TILE)]] = "AMX-TILE";
			toCpuidName[cpuid[nameof(CpuidFeature.AMX_INT8)]] = "AMX-INT8";
			toCpuidName[cpuid[nameof(CpuidFeature.CYRIX_FPU)]] = "Cyrix, AMD Geode GX/LX";
			toCpuidName[cpuid[nameof(CpuidFeature.CYRIX_SMM)]] = "Cyrix, AMD Geode GX/LX";
			toCpuidName[cpuid[nameof(CpuidFeature.CYRIX_SMINT)]] = "Cyrix 6x86MX+, AMD Geode GX/LX";
			toCpuidName[cpuid[nameof(CpuidFeature.CYRIX_SMINT_0F7E)]] = "Cyrix 6x86 or earlier";
			toCpuidName[cpuid[nameof(CpuidFeature.CYRIX_SHR)]] = "Cyrix 6x86MX, M II, III";
			toCpuidName[cpuid[nameof(CpuidFeature.CYRIX_DDI)]] = "Cyrix MediaGX, GXm, GXLV, GX1";
			toCpuidName[cpuid[nameof(CpuidFeature.CYRIX_DMI)]] = "AMD Geode GX/LX";
			toCpuidName[cpuid[nameof(CpuidFeature.CENTAUR_AIS)]] = "Centaur AIS";
			toCpuidName[cpuid[nameof(CpuidFeature.AVX_VNNI)]] = "AVX-VNNI";
			toCpuidName[cpuid[nameof(CpuidFeature.AVX512_FP16)]] = "AVX512-FP16";
			toCpuidName[cpuid[nameof(CpuidFeature.RAO_INT)]] = "RAO-INT";
			toCpuidName[cpuid[nameof(CpuidFeature.AMX_FP16)]] = "AMX-FP16";
			toCpuidName[cpuid[nameof(CpuidFeature.AVX_IFMA)]] = "AVX-IFMA";
			toCpuidName[cpuid[nameof(CpuidFeature.AVX_NE_CONVERT)]] = "AVX-NE-CONVERT";
			toCpuidName[cpuid[nameof(CpuidFeature.AVX_VNNI_INT8)]] = "AVX-VNNI-INT8";
			toCpuidName[cpuid[nameof(CpuidFeature.AMX_COMPLEX)]] = "AMX-COMPLEX";
			toCpuidName[cpuid[nameof(CpuidFeature.AVX_VNNI_INT16)]] = "AVX-VNNI-INT16";
			return toCpuidName;
		}

		static Dictionary<string, EnumValue> CreateEnumDict(EnumType enumType, bool ignoreCase = false) =>
			enumType.Values.ToDictionary(a => a.RawName, a => a, ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);

		void Error(int lineIndex, string message) => errors.Add($"Line {lineIndex + 1}: {message}");

		public (InstructionDef def, ImpliedAccesses? accesses)[] Read() {
			var defs = new List<(InstructionDef def, ImpliedAccesses? accesses, int lineIndex)>();

			var lines = this.lines;
			for (int lineIndex = 0; lineIndex < lines.Length;) {
				var line = lines[lineIndex].Trim();
				if (line.Length == 0 || line[0] == '#') {
					lineIndex++;
					continue;
				}

				int errorCount = errors.Count;
				int origLineIndex = lineIndex;
				if (!TryParse(ref lineIndex, out var def, out var accesses, out var defLineIndex)) {
					if (errorCount >= errors.Count)
						throw new InvalidOperationException();
					lineIndex = SkipLines(Math.Max(origLineIndex + 1, lineIndex));
				}
				else
					defs.Add((def, accesses, defLineIndex));
			}
			if (defs.Count == 0)
				Error(lines.Length, "No instruction definitions found");
			defs.Sort((a, b) => a.def.Code.Value.CompareTo(b.def.Code.Value));

			if (TryGetErrorString(out var errorMessage))
				throw new InvalidOperationException(errorMessage);

			UpdateCodeNameCommentsInFile(lines, defs);

			return defs.Select(a => (a.def, a.accesses)).ToArray();
		}

		void UpdateCodeNameCommentsInFile(string[] lines, List<(InstructionDef def, ImpliedAccesses? accesses, int lineIndex)> infos) {
			var newLines = lines.ToList();
			foreach (var (def, _, lineIndex) in infos.OrderByDescending(a => a.lineIndex)) {
				const string CodeCommentPrefix = "# Code: ";
				var newCommentLine = CodeCommentPrefix + def.Code.RawName;
				if (lineIndex > 0 && lines[lineIndex - 1].StartsWith(CodeCommentPrefix, StringComparison.Ordinal))
					newLines[lineIndex - 1] = newCommentLine;
				else
					newLines.Insert(lineIndex, newCommentLine);
			}
			if (!IsSame(newLines, lines))
				File.WriteAllLines(filename, newLines.ToArray(), FileUtils.FileEncoding);

			static bool IsSame(List<string> a, string[] b) {
				if (a.Count != b.Length)
					return false;
				for (int i = 0; i < b.Length; i++) {
					if (a[i] != b[i])
						return false;
				}
				return true;
			}
		}

		bool TryGetErrorString([NotNullWhen(true)] out string? errorMessages) {
			if (errors.Count == 0) {
				errorMessages = null;
				return false;
			}
			else {
				var sb = new StringBuilder();
				sb.AppendLine();
				sb.AppendLine($"Error parsing instruction definitions, file = {filename}");
				for (int i = 0; i < errors.Count; i++) {
					if (i >= 100) {
						sb.AppendLine("[Too many errors]");
						break;
					}
					sb.Append("Error: ");
					sb.AppendLine(errors[i]);
				}
				errorMessages = sb.ToString();
				return true;
			}
		}

		int SkipLines(int lineIndex) {
			var lines = this.lines;
			while (lineIndex < lines.Length) {
				var line = lines[lineIndex];
				if (line.StartsWith(DefBeginPrefix, StringComparison.Ordinal))
					break;
				lineIndex++;
				if (line == DefEnd)
					break;
			}
			return lineIndex;
		}

		bool TryParse(ref int lineIndex, [NotNullWhen(true)] out InstructionDef? def, out ImpliedAccesses? accesses, out int defLineIndex) {
			def = null;
			accesses = null;
			defLineIndex = -1;

			var line = lines[lineIndex].Trim();
			if (!line.StartsWith(DefBeginPrefix, StringComparison.Ordinal)) {
				Error(lineIndex, $"Expected an instruction definition: `{DefBeginPrefix}`");
				return false;
			}

			line = line[DefBeginPrefix.Length..].Trim();
			if (!TryParseDefLine(line, out var opCodeStr, out var instrStr, out var cpuid,
				out var tupleType, out var error)) {
				Error(lineIndex, error);
				return false;
			}

			var opCodeParser = new OpCodeStringParser(opCodeStr);
			if (!opCodeParser.TryParse(out var parsedOpCode, out error)) {
				Error(lineIndex, $"Opcode string: {error}");
				return false;
			}
			switch (parsedOpCode.OpCodeLength) {
			case 1:
				if (parsedOpCode.OpCode > 0xFF)
					throw new InvalidOperationException();
				break;
			case 2:
				if (parsedOpCode.OpCode > 0xFFFF)
					throw new InvalidOperationException();
				break;
			default:
				Error(lineIndex, $"Invalid op code length: {parsedOpCode.OpCodeLength}");
				return false;
			}

			var instrStrParser = new InstructionStringParser(toRegisterIgnoreCase, parsedOpCode.Encoding, instrStr);
			if (!instrStrParser.TryParse(out var parsedInstr, out error)) {
				Error(lineIndex, $"Instruction string: {error}");
				return false;
			}
			instrStr = parsedInstr.InstructionStr;

			var state = new InstructionDefState(lineIndex, opCodeStr, instrStr, cpuid,
				tupleType ?? tupleTypeN1, toEncoding[parsedOpCode.Encoding.ToString()]);
			lineIndex++;
			state.OpCode = parsedOpCode;

			bool foundEnd = false;
			bool hasRflags = false;
			bool hasFlags = false;
			const InstructionDefFlags2 Decoder16 = InstructionDefFlags2.IntelDecoder16 | InstructionDefFlags2.AmdDecoder16;
			const InstructionDefFlags2 Decoder32 = InstructionDefFlags2.IntelDecoder32 | InstructionDefFlags2.AmdDecoder32;
			const InstructionDefFlags2 Decoder64 = InstructionDefFlags2.IntelDecoder64 | InstructionDefFlags2.AmdDecoder64;
			const InstructionDefFlags2 AllDecoders = Decoder16 | Decoder32 | Decoder64;
			state.Flags2 |= AllDecoders;
			state.Flags2 |= InstructionDefFlags2.RealMode | InstructionDefFlags2.ProtectedMode | InstructionDefFlags2.Virtual8086Mode |
				InstructionDefFlags2.CompatibilityMode | InstructionDefFlags2.LongMode;
			state.Flags2 |= InstructionDefFlags2.UseOutsideSmm | InstructionDefFlags2.UseInSmm |
				InstructionDefFlags2.UseOutsideVmxOp | InstructionDefFlags2.UseInVmxRootOp | InstructionDefFlags2.UseInVmxNonRootOp |
				InstructionDefFlags2.UseOutsideSeam | InstructionDefFlags2.UseInSeam;
			const InstructionDefFlags2 AllEnclaves = InstructionDefFlags2.UseInEnclaveSgx1 | InstructionDefFlags2.UseInEnclaveSgx2;
			state.Flags2 |= InstructionDefFlags2.UseOutsideEnclaveSgx | AllEnclaves;
			fmtKeyValues.Clear();
			bool? privileged = null;
			int opsLineIndex = -1;
			for (; lineIndex < lines.Length; lineIndex++) {
				line = lines[lineIndex].Trim();
				if (line.Length == 0 || line[0] == '#')
					continue;

				if (line == DefEnd) {
					lineIndex++;
					foundEnd = true;
					break;
				}
				if (line.StartsWith(DefBeginPrefix, StringComparison.Ordinal))
					break;

				if (!TryGetDefLineKeyValue(line, out var lineKey, out var lineValue, out error)) {
					Error(lineIndex, error);
					return false;
				}

				if (lineValue == string.Empty) {
					Error(lineIndex, $"Missing {lineKey} value");
					return false;
				}

				switch (lineKey) {
				case "mnemonic":
					if (state.MnemonicStr is not null) {
						Error(lineIndex, $"Duplicate {lineKey}");
						return false;
					}
					state.MnemonicStr = lineValue;
					break;

				case "code-mnemonic":
					if (state.CodeMnemonic is not null) {
						Error(lineIndex, $"Duplicate {lineKey}");
						return false;
					}
					state.CodeMnemonic = lineValue;
					break;

				case "istring-option":
					if (state.InstrStrFmtOption != InstrStrFmtOption.None) {
						Error(lineIndex, $"Duplicate {lineKey}");
						return false;
					}
					switch (lineValue) {
					case "op-mask-is-k1":
					case "no-gpr-suffix":
						state.InstrStrFmtOption = InstrStrFmtOption.OpMaskIsK1_or_NoGprSuffix;
						break;
					case "inc-vec-index":
						state.InstrStrFmtOption = InstrStrFmtOption.IncVecIndex;
						break;
					case "no-vec-index":
						state.InstrStrFmtOption = InstrStrFmtOption.NoVecIndex;
						break;
					case "swap-vec-index12":
						state.InstrStrFmtOption = InstrStrFmtOption.SwapVecIndex12;
						break;
					case "fpu-skip-op0":
						state.InstrStrFmtOption = InstrStrFmtOption.SkipOp0;
						break;
					case "vec-index-same-as-op-index":
						state.InstrStrFmtOption = InstrStrFmtOption.VecIndexSameAsOpIndex;
						break;
					default:
						Error(lineIndex, $"Unknown value `{lineValue}`");
						return false;
					}
					break;

				case "code-suffix":
					if (state.CodeSuffix is not null) {
						Error(lineIndex, $"Duplicate {lineKey}");
						return false;
					}
					state.CodeSuffix = lineValue;
					break;

				case "code-memory-size":
					if (state.CodeMemorySize is not null) {
						Error(lineIndex, $"Duplicate {lineKey}");
						return false;
					}
					state.CodeMemorySize = lineValue;
					break;

				case "code-memory-size-suffix":
					if (state.CodeMemorySizeSuffix is not null) {
						Error(lineIndex, $"Duplicate {lineKey}");
						return false;
					}
					state.CodeMemorySizeSuffix = lineValue;
					break;

				case "implied":
					if (state.ImpliedAccesses is not null) {
						Error(lineIndex, $"Duplicate {lineKey}");
						return false;
					}
					if (!impliedAccessParser.ReadImpliedAccesses(lineValue, out state.ImpliedAccesses, out error)) {
						Error(lineIndex, error);
						return false;
					}
					break;

				case "rflags":
					if (hasRflags) {
						Error(lineIndex, $"Duplicate {lineKey}");
						return false;
					}
					hasRflags = true;
					foreach (var (key, value) in GetKeyValues(lineValue)) {
						if (value == string.Empty) {
							Error(lineIndex, "Missing value");
							return false;
						}
						if (!TryParseRflags(value, out var rflagsValue, out error)) {
							Error(lineIndex, error);
							return false;
						}
						switch (key) {
						case "r": state.RflagsRead |= rflagsValue; break;
						case "u": state.RflagsUndefined |= rflagsValue; break;
						case "w": state.RflagsWritten |= rflagsValue; break;
						case "0": state.RflagsCleared |= rflagsValue; break;
						case "1": state.RflagsSet |= rflagsValue; break;
						default:
							Error(lineIndex, $"Unknown rflags access: `{key}`");
							return false;
						}
					}
					break;

				case "flags":
					if (hasFlags) {
						Error(lineIndex, $"Duplicate {lineKey}");
						return false;
					}
					hasFlags = true;
					foreach (var value in lineValue.Split(' ', StringSplitOptions.RemoveEmptyEntries)) {
						var (newKey, newValue) = ParserUtils.GetKeyValue(value);
						if (newValue == string.Empty) {
							if (flagsMustHaveKeyValue.Contains(newKey)) {
								Error(lineIndex, $"Expected `key=value` but got `{value}`");
								return false;
							}
						}
						else {
							if (!flagsMustHaveKeyValue.Contains(newKey)) {
								Error(lineIndex, $"Expected `key` but got `key=value`: `{value}`");
								return false;
							}
						}
						switch (newKey) {
						case "16": state.Flags1 |= InstructionDefFlags1.Bit16; break;
						case "32": state.Flags1 |= InstructionDefFlags1.Bit32; break;
						case "64": state.Flags1 |= InstructionDefFlags1.Bit64; break;
						case "cpl0": state.Flags1 |= InstructionDefFlags1.Cpl0; break;
						case "cpl1": state.Flags1 |= InstructionDefFlags1.Cpl1; break;
						case "cpl2": state.Flags1 |= InstructionDefFlags1.Cpl2; break;
						case "cpl3": state.Flags1 |= InstructionDefFlags1.Cpl3; break;
						case "save-restore": state.Flags1 |= InstructionDefFlags1.SaveRestore; break;
						case "stack": state.Flags1 |= InstructionDefFlags1.StackInstruction; break;
						case "ignores-seg": state.Flags1 |= InstructionDefFlags1.IgnoresSegment; break;
						case "krw": state.Flags1 |= InstructionDefFlags1.OpMaskReadWrite; break;
						case "wig32":
							if (state.OpCode.WBit != OpCodeW.W0 && state.OpCode.WBit != OpCodeW.WIG) {
								Error(lineIndex, "Instruction isn't W0/WIG so can't be WIG32");
								return false;
							}
							state.Flags1 |= InstructionDefFlags1.WIG32;
							break;
						case "lock": state.Flags1 |= InstructionDefFlags1.Lock; break;
						case "xacquire": state.Flags1 |= InstructionDefFlags1.Xacquire; break;
						case "xrelease": state.Flags1 |= InstructionDefFlags1.Xrelease; break;
						case "rep": state.Flags1 |= InstructionDefFlags1.Rep; break;
						case "repe": state.Flags1 |= InstructionDefFlags1.Rep; break;
						case "repne": state.Flags1 |= InstructionDefFlags1.Repne; break;
						case "bnd": state.Flags1 |= InstructionDefFlags1.Bnd; break;
						case "ht": state.Flags1 |= InstructionDefFlags1.HintTaken; break;
						case "notrack": state.Flags1 |= InstructionDefFlags1.Notrack; break;
						case "no-instr": state.Flags1 |= InstructionDefFlags1.NoInstruction; break;
						case "knz": state.Flags1 |= InstructionDefFlags1.RequireOpMaskRegister; break;
						case "ignore-mod": state.Flags1 |= InstructionDefFlags1.IgnoresModBits; break;
						case "unique-reg-num": state.Flags1 |= InstructionDefFlags1.RequiresUniqueRegNums; break;
						case "no-66": state.Flags1 |= InstructionDefFlags1.No66; break;
						case "nfx": state.Flags1 |= InstructionDefFlags1.NFx; break;

						case "no-amd-dec": state.Flags2 &= ~(InstructionDefFlags2.AmdDecoder16 | InstructionDefFlags2.AmdDecoder32 | InstructionDefFlags2.AmdDecoder64); break;
						case "no-amd-dec64": state.Flags2 &= ~InstructionDefFlags2.AmdDecoder64; break;
						case "no-intel-dec": state.Flags2 &= ~(InstructionDefFlags2.IntelDecoder16 | InstructionDefFlags2.IntelDecoder32 | InstructionDefFlags2.IntelDecoder64); break;
						case "no-intel-dec64": state.Flags2 &= ~InstructionDefFlags2.IntelDecoder64; break;
						case "no-rm": state.Flags2 &= ~InstructionDefFlags2.RealMode; break;
						case "no-pm": state.Flags2 &= ~InstructionDefFlags2.ProtectedMode; break;
						case "no-v86": state.Flags2 &= ~InstructionDefFlags2.Virtual8086Mode; break;
						case "no-cm": state.Flags2 &= ~InstructionDefFlags2.CompatibilityMode; break;
						case "no-lm": state.Flags2 &= ~InstructionDefFlags2.LongMode; break;
						case "no-outside-smm": state.Flags2 &= ~InstructionDefFlags2.UseOutsideSmm; break;
						case "no-in-smm": state.Flags2 &= ~InstructionDefFlags2.UseInSmm; break;
						case "no-outside-vmx-op": state.Flags2 &= ~InstructionDefFlags2.UseOutsideVmxOp; break;
						case "no-in-vmx-root": state.Flags2 &= ~InstructionDefFlags2.UseInVmxRootOp; break;
						case "no-in-vmx-non-root": state.Flags2 &= ~InstructionDefFlags2.UseInVmxNonRootOp; break;
						case "no-outside-seam": state.Flags2 &= ~InstructionDefFlags2.UseOutsideSeam; break;
						case "no-in-seam": state.Flags2 &= ~InstructionDefFlags2.UseInSeam; break;
						case "no-outside-sgx": state.Flags2 &= ~InstructionDefFlags2.UseOutsideEnclaveSgx; break;
						case "no-in-sgx": state.Flags2 &= ~AllEnclaves; break;
						case "no-in-sgx1": state.Flags2 &= ~InstructionDefFlags2.UseInEnclaveSgx1; break;
						case "no-in-sgx2": state.Flags2 &= ~InstructionDefFlags2.UseInEnclaveSgx2; break;
						case "tdx-non-root-ud": state.Flags2 &= ~InstructionDefFlags2.TdxNonRootGenUd; break;
						case "tdx-non-root-ve": state.Flags2 &= ~InstructionDefFlags2.TdxNonRootGenVe; break;
						case "tdx-non-root-may-gen-ex": state.Flags2 &= ~InstructionDefFlags2.TdxNonRootMayGenEx; break;
						case "intel-vm-exit": state.Flags2 |= InstructionDefFlags2.IntelVmExit; break;
						case "intel-may-vm-exit": state.Flags2 |= InstructionDefFlags2.IntelMayVmExit; break;
						case "intel-smm-vm-exit": state.Flags2 |= InstructionDefFlags2.IntelSmmVmExit; break;
						case "amd-vm-exit": state.Flags2 |= InstructionDefFlags2.AmdVmExit; break;
						case "amd-may-vm-exit": state.Flags2 |= InstructionDefFlags2.AmdMayVmExit; break;
						case "tsx-abort": state.Flags2 |= InstructionDefFlags2.TsxAbort; break;
						case "tsx-impl-abort": state.Flags2 |= InstructionDefFlags2.TsxImplAbort; break;
						case "tsx-may-abort": state.Flags2 |= InstructionDefFlags2.TsxMayAbort; break;

						case "intel-fo64": state.Flags3 |= InstructionDefFlags3.IntelForceOpSize64; break;
						case "do64": state.Flags3 |= InstructionDefFlags3.DefaultOpSize64; break;
						case "fo64": state.Flags3 |= InstructionDefFlags3.ForceOpSize64; break;
						case "io": state.Flags3 |= InstructionDefFlags3.InputOutput; break;
						case "nop": state.Flags3 |= InstructionDefFlags3.Nop; break;
						case "res-nop": state.Flags3 |= InstructionDefFlags3.ReservedNop; break;
						case "ignore-er": state.Flags3 |= InstructionDefFlags3.IgnoresRoundingControl; break;
						case "serialize-intel": state.Flags3 |= InstructionDefFlags3.SerializingIntel; break;
						case "serialize-amd": state.Flags3 |= InstructionDefFlags3.SerializingAmd; break;
						case "may-require-cpl0": state.Flags3 |= InstructionDefFlags3.MayRequireCpl0; break;
						case "amd-lock-reg-bit": state.Flags3 |= InstructionDefFlags3.AmdLockRegBit; break;
						case "cet-tracked": state.Flags3 |= InstructionDefFlags3.CetTracked; break;
						case "non-temporal": state.Flags3 |= InstructionDefFlags3.NonTemporal; break;
						case "no-wait": state.Flags3 |= InstructionDefFlags3.FpuNoWait; break;
						case "implied-z": state.Flags3 |= InstructionDefFlags3.ImpliedZeroingMasking; break;
						case "k-elem-selector": state.Flags3 |= InstructionDefFlags3.OpMaskIsElementSelector; break;
						case "prefetch": state.Flags3 |= InstructionDefFlags3.Prefetch; break;
						case "ignores-index": state.Flags3 |= InstructionDefFlags3.IgnoresIndex; break;
						case "tile-stride-index": state.Flags3 |= InstructionDefFlags3.TileStrideIndex; break;
						case "unique-dest-reg-num": state.Flags3 |= InstructionDefFlags3.RequiresUniqueDestRegNum; break;
						case "is-string-op": state.Flags3 |= InstructionDefFlags3.IsStringOp; break;
						case "a32-req": state.Flags3 |= InstructionDefFlags3.RequiresAddressSize32; break;
						case "ig-modrm-low3": state.Flags3 |= InstructionDefFlags3.IgnoresModrmLow3Bits; break;
						case "atomic": state.Flags3 |= InstructionDefFlags3.Atomic; break;
						case "aligned-mem": state.Flags3 |= InstructionDefFlags3.AlignedMemory; break;

						case "vmx":
							if (state.VmxMode != VmxMode.None) {
								Error(lineIndex, $"Duplicate {newKey} value");
								return false;
							}
							state.VmxMode = newValue switch {
								"op" => VmxMode.VmxOp,
								"root" => VmxMode.VmxRootOp,
								"non-root" => VmxMode.VmxNonRootOp,
								_ => throw new InvalidOperationException(),
							};
							break;

						case "privileged":
						case "no-privileged":
							if (privileged is not null) {
								Error(lineIndex, $"Duplicate privileged/no-privileged value");
								return false;
							}
							privileged = value == "privileged";
							break;

						case "fpu-push":
						case "fpu-cond-push":
						case "fpu-pop":
						case "fpu-stack":
							if (state.FpuStackIncrement != 0) {
								Error(lineIndex, $"At most one of these can be used: `fpu-push`, `fpu-cond-push`, `fpu-pop`, `fpu-stack`");
								return false;
							}
							if (!ParserUtils.TryParseInt32(newValue, out int stackCount, out error))
								return false;
							state.Flags3 |= InstructionDefFlags3.WritesFpuTop;
							switch (newKey) {
							case "fpu-push":
							case "fpu-cond-push":
								if (stackCount < 1) {
									Error(lineIndex, $"Invalid FPU stack push count: {newValue}");
									return false;
								}
								state.FpuStackIncrement = -stackCount;
								if (newKey == "fpu-cond-push")
									state.Flags3 |= InstructionDefFlags3.IsFpuCondWriteTop;
								break;
							case "fpu-pop":
								if (stackCount < 1) {
									Error(lineIndex, $"Invalid FPU stack pop count: {newValue}");
									return false;
								}
								state.FpuStackIncrement = stackCount;
								break;
							case "fpu-stack":
								state.FpuStackIncrement = stackCount;
								break;
							default:
								throw new InvalidOperationException();
							}
							break;

						case "writes-fpu-top":
							state.Flags3 |= InstructionDefFlags3.WritesFpuTop;
							break;

						case "sp":
							if (state.StackInfo.Kind != StackInfoKind.None) {
								Error(lineIndex, $"Duplicate {newKey} value");
								return false;
							}
							state.Flags1 |= InstructionDefFlags1.StackInstruction;
							var spArgs = newValue.Split(';');
							if (spArgs.Length != 2) {
								Error(lineIndex, $"Expected exactly one semicolon");
								return false;
							}
							var spKey = spArgs[0];
							if (!ParserUtils.TryParseInt32(spArgs[1], out var spValue, out error))
								return false;
							switch (spKey) {
							case "push":
								state.StackInfo = new StackInfo(StackInfoKind.Increment, -spValue);
								break;
							case "pop":
								state.StackInfo = new StackInfo(StackInfoKind.Increment, spValue);
								break;
							case "enter":
								state.StackInfo = new StackInfo(StackInfoKind.Enter, spValue);
								break;
							case "iret":
								state.StackInfo = new StackInfo(StackInfoKind.Iret, spValue);
								break;
							case "pop_imm16":
								state.StackInfo = new StackInfo(StackInfoKind.PopImm16, spValue);
								break;
							default:
								Error(lineIndex, $"Unknown sp key: `{spKey}`");
								return false;
							}
							break;

						case "br":
							if (state.BranchKind != BranchKind.None) {
								Error(lineIndex, $"Duplicate {newKey}");
								return false;
							}
							switch (newValue) {
							case "jcc-short": state.BranchKind = BranchKind.JccShort; break;
							case "jcc-near": state.BranchKind = BranchKind.JccNear; break;
							case "jmp-short": state.BranchKind = BranchKind.JmpShort; break;
							case "jmp-near": state.BranchKind = BranchKind.JmpNear; break;
							case "jmp-far": state.BranchKind = BranchKind.JmpFar; break;
							case "jmp-near-indirect": state.BranchKind = BranchKind.JmpNearIndirect; break;
							case "jmp-far-indirect": state.BranchKind = BranchKind.JmpFarIndirect; break;
							case "call-near": state.BranchKind = BranchKind.CallNear; break;
							case "call-far": state.BranchKind = BranchKind.CallFar; break;
							case "call-near-indirect": state.BranchKind = BranchKind.CallNearIndirect; break;
							case "call-far-indirect": state.BranchKind = BranchKind.CallFarIndirect; break;
							case "jmpe-near": state.BranchKind = BranchKind.JmpeNear; break;
							case "jmpe-near-indirect": state.BranchKind = BranchKind.JmpeNearIndirect; break;
							case "loop": state.BranchKind = BranchKind.Loop; break;
							case "jrcxz": state.BranchKind = BranchKind.Jrcxz; break;
							case "xbegin": state.BranchKind = BranchKind.Xbegin; break;
							case "jkcc-short": state.BranchKind = BranchKind.JkccShort; break;
							case "jkcc-near": state.BranchKind = BranchKind.JkccNear; break;
							default:
								Error(lineIndex, $"Unknown branch kind `{newValue}`");
								return false;
							}
							break;

						case "cc":
							if (state.ConditionCode != ConditionCode.None) {
								Error(lineIndex, $"Duplicate {newKey}");
								return false;
							}
							var ccValues = newValue.Split(';');
							if (ccValues.Length != 3) {
								Error(lineIndex, $"Expected 2 semicolons: `{newValue}`");
								return false;
							}
							state.MnemonicCcPrefix = ccValues[0].ToLowerInvariant();
							state.MnemonicCcSuffix = ccValues[2].ToLowerInvariant();
							switch (ccValues[1]) {
							case "o": state.ConditionCode = ConditionCode.o; break;
							case "no": state.ConditionCode = ConditionCode.no; break;
							case "b": state.ConditionCode = ConditionCode.b; break;
							case "ae": state.ConditionCode = ConditionCode.ae; break;
							case "e": state.ConditionCode = ConditionCode.e; break;
							case "ne": state.ConditionCode = ConditionCode.ne; break;
							case "be": state.ConditionCode = ConditionCode.be; break;
							case "a": state.ConditionCode = ConditionCode.a; break;
							case "s": state.ConditionCode = ConditionCode.s; break;
							case "ns": state.ConditionCode = ConditionCode.ns; break;
							case "p": state.ConditionCode = ConditionCode.p; break;
							case "np": state.ConditionCode = ConditionCode.np; break;
							case "l": state.ConditionCode = ConditionCode.l; break;
							case "ge": state.ConditionCode = ConditionCode.ge; break;
							case "le": state.ConditionCode = ConditionCode.le; break;
							case "g": state.ConditionCode = ConditionCode.g; break;
							default:
								Error(lineIndex, $"Unknown condition code `{newValue}`");
								return false;
							}
							break;

						case "cflow":
							if (state.Cflow is not null) {
								Error(lineIndex, $"Duplicate {newKey}");
								return false;
							}
							switch (newValue) {
							case "br": state.Cflow = flowControlType[nameof(FlowControl.UnconditionalBranch)]; break;
							case "br-ind": state.Cflow = flowControlType[nameof(FlowControl.IndirectBranch)]; break;
							case "br-cond": state.Cflow = flowControlType[nameof(FlowControl.ConditionalBranch)]; break;
							case "ret": state.Cflow = flowControlType[nameof(FlowControl.Return)]; break;
							case "call": state.Cflow = flowControlType[nameof(FlowControl.Call)]; break;
							case "call-ind": state.Cflow = flowControlType[nameof(FlowControl.IndirectCall)]; break;
							case "int": state.Cflow = flowControlType[nameof(FlowControl.Interrupt)]; break;
							case "tsx": state.Cflow = flowControlType[nameof(FlowControl.XbeginXabortXend)]; break;
							case "ex": state.Cflow = flowControlType[nameof(FlowControl.Exception)]; break;
							default:
								Error(lineIndex, $"Unknown cflow value `{newValue}`");
								return false;
							}
							break;

						case "dec-opt":
							if (state.DecoderOption is not null) {
								Error(lineIndex, $"Duplicate {newKey}");
								return false;
							}
							if (!TryGetValue(toDecOptionValue, newValue, out state.DecoderOption, out _)) {
								Error(lineIndex, $"Add missing decoder option value to {nameof(DecOptionValue)}: {newValue}");
								return false;
							}
							break;

						case "pseudo":
							if (state.PseudoOpsKind is not null) {
								Error(lineIndex, $"Duplicate {newKey}");
								return false;
							}
							if (!TryGetValue(toPseudoOpsKind, newValue, out state.PseudoOpsKind, out error)) {
								Error(lineIndex, error);
								return false;
							}
							break;

						case "asm":
							if (state.AsmMnemonic is not null) {
								Error(lineIndex, $"Duplicate {newKey}");
								return false;
							}
							state.AsmMnemonic = newValue;
							break;

						case "asm-ig":
							state.Flags3 |= InstructionDefFlags3.AsmIgnore;
							break;

						case "asm-ig-mem":
							state.Flags3 |= InstructionDefFlags3.AsmIgnoreMemory;
							break;

						default:
							Error(lineIndex, $"Unknown flags value `{value}`");
							return false;
						}
					}
					break;

				case "ops":
					opsLineIndex = lineIndex;
					if (state.OpAccess.Length != 0 || state.OpKinds.Length != 0) {
						Error(lineIndex, $"Duplicate {lineKey}");
						return false;
					}
					var opsParts = lineValue.Split('|');
					if (opsParts.Length > 2) {
						Error(lineIndex, "Expected at most one `|`");
						return false;
					}
					opAccess.Clear();
					opKinds.Clear();
					int opIndex = -1;
					foreach (var (key, value) in GetKeyValues(opsParts[0].Trim())) {
						opIndex++;
						if (value == string.Empty) {
							Error(lineIndex, "Missing value");
							return false;
						}
						if (opIndex >= parsedInstr.Operands.Length) {
							Error(lineIndex, $"Too many operands. The instruction has exactly {parsedInstr.Operands.Length} operands.");
							return false;
						}

						var opFlags = OpCodeOperandKindDefFlags.None;
						var parsedOp = parsedInstr.Operands[opIndex];
						var parsedOpFlags = parsedOp.Flags;
						int size1 = parsedOp.SizeBits;
						int size2 = 0;
						var opReg = parsedOp.Register;

						OperandEncoding opEnc;
						var opKindParts = value.Split(';').Select(a => a.Trim()).ToArray();
						for (int i = 1; i < opKindParts.Length; i++) {
							var flag = opKindParts[i];
							switch (flag) {
							case "mpx":
								opFlags |= OpCodeOperandKindDefFlags.MPX;
								break;
							case "mem":
								opFlags |= OpCodeOperandKindDefFlags.Memory;
								break;
							case "p1":
								opFlags |= OpCodeOperandKindDefFlags.RegPlus1;
								break;
							case "p3":
								opFlags |= OpCodeOperandKindDefFlags.RegPlus3;
								break;
							case "lock":
								opFlags |= OpCodeOperandKindDefFlags.LockBit;
								break;
							default:
								if (size2 == 0 && int.TryParse(flag, out size2))
									break;
								else {
									Error(lineIndex, $"Unknown op flag `{flag}`");
									return false;
								}
							}
						}
						var opKindStr = opKindParts[0];
						switch (opKindStr) {
						case "br-far":
							if ((parsedOpFlags & ParsedInstructionOperandFlags.FarBranch) == 0) {
								Error(lineIndex, $"Op {opIndex}: Expected a far branch operand");
								return false;
							}
							opEnc = OperandEncoding.FarBranch;
							switch (parsedOpCode.OperandSize) {
							case CodeSize.Code16: size1 = 16; break;
							case CodeSize.Code32: size1 = 32; break;
							default:
								Error(lineIndex, $"Op {opIndex}: Expected a 16/32-bit far branch operand but got {parsedOpCode.OperandSize}");
								return false;
							}
							break;

						case "br":
						case "br64":
							if ((parsedOpFlags & ParsedInstructionOperandFlags.DispBranch) != 0)
								opEnc = OperandEncoding.AbsNearBranch;
							else if ((parsedOpFlags & ParsedInstructionOperandFlags.RelBranch) != 0) {
								opEnc = OperandEncoding.NearBranch;
								var opSize = parsedOpCode.OperandSize;
								if (opSize == CodeSize.Unknown) {
									switch (opKindStr) {
									case "br64": opSize = CodeSize.Code64; break;
									default:
										Error(lineIndex, "Unknown operand size");
										return false;
									}
								}
								switch (opSize) {
								case CodeSize.Code16: size2 = 16; break;
								case CodeSize.Code32: size2 = 32; break;
								case CodeSize.Code64: size2 = 64; break;
								default:
									Error(lineIndex, $"Op {opIndex}: Expected a 16/32/64-bit near branch operand but got {opSize}");
									return false;
								}
							}
							else {
								Error(lineIndex, $"Op {opIndex}: Expected a near branch operand");
								return false;
							}
							break;

						case "br-x":
							if ((parsedOpFlags & ParsedInstructionOperandFlags.RelBranch) == 0) {
								Error(lineIndex, $"Op {opIndex}: Expected a near branch operand");
								return false;
							}
							opEnc = OperandEncoding.Xbegin;
							break;

						case "moffs":
							if ((parsedOpFlags & ParsedInstructionOperandFlags.MemoryOffset) == 0) {
								Error(lineIndex, $"Op {opIndex}: Expected an moffs operand");
								return false;
							}
							opEnc = OperandEncoding.MemOffset;
							break;

						case "rm":
							switch (parsedOpFlags & (ParsedInstructionOperandFlags.Register | ParsedInstructionOperandFlags.Memory)) {
							case ParsedInstructionOperandFlags.Register:
								opEnc = OperandEncoding.RegModrmRm;
								break;

							case ParsedInstructionOperandFlags.Memory:
								opEnc = OperandEncoding.MemModrmRm;
								break;

							case ParsedInstructionOperandFlags.Register | ParsedInstructionOperandFlags.Memory:
								opEnc = OperandEncoding.RegMemModrmRm;
								break;

							default:
								Error(lineIndex, $"Op {opIndex}: Expected a register and/or a memory operand");
								return false;
							}
							break;

						case "reg":
							if ((parsedOpFlags & ParsedInstructionOperandFlags.Register) == 0 || opReg == Register.None) {
								Error(lineIndex, $"Op {opIndex}: Expected a register");
								return false;
							}
							opEnc = OperandEncoding.RegModrmReg;
							break;

						case "opcode":
							if ((parsedOpFlags & ParsedInstructionOperandFlags.Register) == 0 || opReg == Register.None) {
								Error(lineIndex, $"Op {opIndex}: Expected a register operand");
								return false;
							}
							opEnc = OperandEncoding.RegOpCode;
							break;

						case "vvvv":
							if ((parsedOpFlags & ParsedInstructionOperandFlags.Register) == 0 || opReg == Register.None) {
								Error(lineIndex, $"Op {opIndex}: Expected a register operand");
								return false;
							}
							opEnc = OperandEncoding.RegVvvvv;
							break;

						case "is":
							if ((parsedOpFlags & ParsedInstructionOperandFlags.Register) == 0 || opReg == Register.None) {
								Error(lineIndex, $"Op {opIndex}: Expected a register operand");
								return false;
							}
							if ((parsedOpCode.Flags & ParsedOpCodeFlags.Is5) != 0)
								opFlags |= OpCodeOperandKindDefFlags.Is5;
							opEnc = OperandEncoding.RegImm;
							break;

						case "imm":
							if ((parsedOpFlags & ParsedInstructionOperandFlags.Immediate) == 0) {
								Error(lineIndex, $"Op {opIndex}: Expected an immediate operand");
								return false;
							}
							if ((parsedOpCode.Flags & ParsedOpCodeFlags.Is5) != 0)
								opFlags |= OpCodeOperandKindDefFlags.M2Z;
							if (size2 == 0)
								size2 = size1;
							opEnc = OperandEncoding.Immediate;
							break;

						case "seg-rsi":
							if ((parsedOpFlags & ParsedInstructionOperandFlags.Memory) == 0) {
								Error(lineIndex, $"Op {opIndex}: Expected a memory operand");
								return false;
							}
							opFlags |= OpCodeOperandKindDefFlags.Memory;
							opEnc = OperandEncoding.SegRSI;
							break;

						case "es-rdi":
							if ((parsedOpFlags & ParsedInstructionOperandFlags.Memory) == 0) {
								Error(lineIndex, $"Op {opIndex}: Expected a memory operand");
								return false;
							}
							opFlags |= OpCodeOperandKindDefFlags.Memory;
							opEnc = OperandEncoding.ESRDI;
							break;

						case "seg-rdi":
							if ((parsedOpFlags & ParsedInstructionOperandFlags.Memory) == 0) {
								Error(lineIndex, $"Op {opIndex}: Expected a memory operand");
								return false;
							}
							opFlags |= OpCodeOperandKindDefFlags.Memory;
							opEnc = OperandEncoding.SegRDI;
							break;

						case "seg-rbx-al":
							if ((parsedOpFlags & ParsedInstructionOperandFlags.Memory) == 0) {
								Error(lineIndex, $"Op {opIndex}: Expected a memory operand");
								return false;
							}
							opFlags |= OpCodeOperandKindDefFlags.Memory;
							opEnc = OperandEncoding.SegRBX;
							break;

						default:
							int index = opKindStr.IndexOf(':', StringComparison.Ordinal);
							if (index < 0) {
								Error(lineIndex, $"Unknown op kind `{opKindStr}`");
								return false;
							}
							var opKindValue = opKindStr[(index + 1)..].Trim();
							opKindStr = opKindStr[0..index].Trim();
							switch (opKindStr) {
							case "r":
								if ((parsedOpFlags & ParsedInstructionOperandFlags.ImpliedRegister) == 0) {
									Error(lineIndex, $"Op {opIndex}: Expected an implied register operand");
									return false;
								}
								if (!TryGetValue(toRegisterIgnoreCase, opKindValue, out var regEnumValue, out error))
									return false;
								opReg = (Register)regEnumValue.Value;
								opEnc = OperandEncoding.ImpliedRegister;
								break;

							case "c":
								if ((parsedOpFlags & ParsedInstructionOperandFlags.ConstImmediate) == 0) {
									Error(lineIndex, $"Op {opIndex}: Expected an implied immediate operand");
									return false;
								}
								if (!ParserUtils.TryParseInt32(opKindValue, out size1, out error))
									return false;
								opEnc = OperandEncoding.ImpliedConst;
								break;

							default:
								Error(lineIndex, $"Unknown op kind `{opKindStr}`");
								return false;
							}
							break;
						}

						var opKindKey = new OpKindKey(opEnc, parsedOpFlags, opReg, size1, size2, opFlags);
						if (!toOpCodeOperandKindDef.TryGetValue(opKindKey, out var opKindDef)) {
							Error(lineIndex, $"Invalid enum value `{value}`");
							return false;
						}

						OpInfo opAccess;
						switch (key) {
						case "n": opAccess = OpInfo.None; break;
						case "cr": opAccess = OpInfo.CondRead; break;
						case "cw": opAccess = OpInfo.CondWrite; break;
						case "cw32_rw64": opAccess = OpInfo.CondWrite32_ReadWrite64; break;
						case "nma": opAccess = OpInfo.NoMemAccess; break;
						case "r":
							if ((opFlags & OpCodeOperandKindDefFlags.RegPlus3) != 0)
								opAccess = OpInfo.ReadP3;
							else
								opAccess = OpInfo.Read;
							break;
						case "rcw": opAccess = OpInfo.ReadCondWrite; break;
						case "rw": opAccess = OpInfo.ReadWrite; break;
						case "w": opAccess = OpInfo.Write; break;
						case "wvmm": opAccess = OpInfo.WriteVmm; break;
						case "rwvmm": opAccess = OpInfo.ReadWriteVmm; break;
						case "wf":
							if ((opFlags & OpCodeOperandKindDefFlags.RegPlus1) != 0)
								opAccess = OpInfo.WriteForceP1;
							else
								opAccess = OpInfo.WriteForce;
							break;
						case "wm_rwreg": opAccess = OpInfo.WriteMem_ReadWriteReg; break;
						default:
							Error(lineIndex, $"Unknown op access `{key}`");
							return false;
						}

						this.opAccess.Add(opAccess);
						opKinds.Add(opKindDef);
					}
					if (opAccess.Count == 0) {
						Error(lineIndex, "Missing op access and kind");
						return false;
					}
					if (opIndex + 1 != parsedInstr.Operands.Length) {
						Error(lineIndex, $"Too few operands. Instruction string has exactly {parsedInstr.Operands.Length} operands.");
						return false;
					}
					state.OpAccess = opAccess.ToArray();
					state.OpKinds = opKinds.ToArray();
					if (parsedOpCode.Encoding == EncodingKind.MVEX) {
						if (opsParts.Length < 2) {
							Error(lineIndex, "Expected MVEX memory/swizzle info");
							return false;
						}

						state.Mvex = new(toMvexTupleTypeLutKind[nameof(MvexTupleTypeLutKind.Int32)], state.OpCode.MvexEHBit, MvexConvFn.None, 0, 0);
						bool seenN = false;
						EnumValue? ttLutKind = null;
						int elemSize = 0;
						foreach (var (key, value) in GetKeyValues(opsParts[1].Trim())) {
							if (key is not "swizz" and not "mem" and not "N") {
								if (value != string.Empty) {
									Error(lineIndex, $"Unexpected value `{key}={value}`");
									return false;
								}
							}
							switch (key) {
							case "sae":
								state.Flags1 |= InstructionDefFlags1.SuppressAllExceptions;
								state.Mvex.Flags1 |= MvexInfoFlags1.SuppressAllExceptions;
								break;

							case "er":
								state.Flags1 |= InstructionDefFlags1.RoundingControl;
								state.Mvex.Flags1 |= MvexInfoFlags1.RoundingControl;
								break;

							case "er-imm":
								state.Flags1 |= InstructionDefFlags1.RoundingControl;
								state.Mvex.Flags1 |= MvexInfoFlags1.ImmRoundingControl;
								break;

							case "no-er-sae":
								state.Mvex.Flags2 |= MvexInfoFlags2.NoSaeRoundingControl;
								break;

							case "ignore-opmask":
								state.Mvex.Flags1 |= MvexInfoFlags1.IgnoresOpMaskRegister;
								break;

							case "ignore-eh":
								state.Mvex.Flags2 |= MvexInfoFlags2.IgnoresEvictionHint;
								break;

							case "swizz":
								if (value == string.Empty)
									state.Mvex.ValidSwizzleFns = 0xFF;
								else if (!ParseMvexValidFns(value, out state.Mvex.ValidSwizzleFns, out error)) {
									Error(lineIndex, $"Invalid swizzle-fn bits: {error}");
									return false;
								}
								break;

							case "mem":
								if (seenN) {
									Error(lineIndex, "mem=xxx must be before N=xxx");
									return false;
								}
								if (value == string.Empty) {
									Error(lineIndex, "Expected mem conv fn bits");
									return false;
								}
								if (!ParseMvexValidFns(value, out state.Mvex.ValidConvFns, out error)) {
									Error(lineIndex, $"Invalid mem-conv-fn bits: {error}");
									return false;
								}
								break;

							case "i32":
								ttLutKind = toMvexTupleTypeLutKind[nameof(MvexTupleTypeLutKind.Int32)];
								elemSize = 32;
								break;

							case "f32":
								ttLutKind = toMvexTupleTypeLutKind[nameof(MvexTupleTypeLutKind.Float32)];
								elemSize = 32;
								break;

							case "i32-half":
								ttLutKind = toMvexTupleTypeLutKind[nameof(MvexTupleTypeLutKind.Int32_Half)];
								elemSize = 32;
								break;

							case "f32-half":
								ttLutKind = toMvexTupleTypeLutKind[nameof(MvexTupleTypeLutKind.Float32_Half)];
								elemSize = 32;
								break;

							case "i32-bcst1":
							case "i32-elem":
								ttLutKind = toMvexTupleTypeLutKind[nameof(MvexTupleTypeLutKind.Int32_1to16_or_elem)];
								elemSize = 32;
								break;

							case "f32-bcst1":
							case "f32-elem":
								ttLutKind = toMvexTupleTypeLutKind[nameof(MvexTupleTypeLutKind.Float32_1to16_or_elem)];
								elemSize = 32;
								break;

							case "i64-bcst1":
							case "i64-elem":
								ttLutKind = toMvexTupleTypeLutKind[nameof(MvexTupleTypeLutKind.Int64_1to8_or_elem)];
								elemSize = 64;
								break;

							case "f64-bcst1":
							case "f64-elem":
								ttLutKind = toMvexTupleTypeLutKind[nameof(MvexTupleTypeLutKind.Float64_1to8_or_elem)];
								elemSize = 64;
								break;

							case "i32-bcst4":
								ttLutKind = toMvexTupleTypeLutKind[nameof(MvexTupleTypeLutKind.Int32_4to16)];
								elemSize = 32;
								break;

							case "f32-bcst4":
								ttLutKind = toMvexTupleTypeLutKind[nameof(MvexTupleTypeLutKind.Float32_4to16)];
								elemSize = 32;
								break;

							case "i64-bcst4":
								ttLutKind = toMvexTupleTypeLutKind[nameof(MvexTupleTypeLutKind.Int64_4to8)];
								elemSize = 64;
								break;

							case "f64-bcst4":
								ttLutKind = toMvexTupleTypeLutKind[nameof(MvexTupleTypeLutKind.Float64_4to8)];
								elemSize = 64;
								break;

							default:
								Error(lineIndex, $"Invalid key `{key}`");
								return false;
							}
						}
						foreach (var op in parsedInstr.Operands) {
							if (op.MvexConvFn != MvexConvFn.None) {
								state.Mvex.ConvFn = op.MvexConvFn;
								break;
							}
						}
						ttLutKind ??= state.Mvex.ConvFn switch {
							MvexConvFn.Si32 or MvexConvFn.Ui32 or MvexConvFn.Di32 => toMvexTupleTypeLutKind[nameof(MvexTupleTypeLutKind.Int32)],
							MvexConvFn.Sf32 or MvexConvFn.Uf32 or MvexConvFn.Df32 => toMvexTupleTypeLutKind[nameof(MvexTupleTypeLutKind.Float32)],
							MvexConvFn.Si64 or MvexConvFn.Ui64 or MvexConvFn.Di64 => toMvexTupleTypeLutKind[nameof(MvexTupleTypeLutKind.Int64)],
							MvexConvFn.Sf64 or MvexConvFn.Uf64 or MvexConvFn.Df64 => toMvexTupleTypeLutKind[nameof(MvexTupleTypeLutKind.Float64)],
							MvexConvFn.None => null,
							_ => throw new InvalidOperationException(),
						};
						state.Mvex.Flags2 |= state.Mvex.ConvFn switch {
							MvexConvFn.Si32 or MvexConvFn.Ui32 or MvexConvFn.Di32 or
							MvexConvFn.Sf32 or MvexConvFn.Uf32 or MvexConvFn.Df32 => MvexInfoFlags2.ConvFn32,
							MvexConvFn.Si64 or MvexConvFn.Ui64 or MvexConvFn.Di64 or
							MvexConvFn.Sf64 or MvexConvFn.Uf64 or MvexConvFn.Df64 => MvexInfoFlags2.None,
							MvexConvFn.None => elemSize == 32 ? MvexInfoFlags2.ConvFn32 : MvexInfoFlags2.None,
							_ => throw new InvalidOperationException(),
						};
						if (ttLutKind is null) {
							Error(lineIndex, $"Unknown {nameof(MvexTupleTypeLutKind)} value");
							return false;
						}
						state.Mvex.TupleTypeLutKind = ttLutKind;
					}
					else if (opsParts.Length > 1) {
						var memParts = opsParts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
						if (memParts.Length == 0 || memParts.Length > 2) {
							Error(lineIndex, $"Expected 1-2 {nameof(MemorySize)} values");
							return false;
						}
						if (!TryGetValue(toMemorySize, memParts[0], out state.MemorySize, out error)) {
							Error(lineIndex, error);
							return false;
						}
						if (memParts.Length > 1 && !TryGetValue(toMemorySize, memParts[1], out state.MemorySize_Broadcast, out error)) {
							Error(lineIndex, error);
							return false;
						}
						if (state.MemorySize_Broadcast is not null && state.OpCode.Encoding != EncodingKind.EVEX) {
							Error(lineIndex, "Only EVEX instructions support conditional broadcasting (EVEX.b bit)");
							return false;
						}
					}
					break;

				case "fast":
				case "gas":
				case "intel":
				case "masm":
				case "nasm":
					fmtKeyValues.Add((lineKey, lineValue, lineIndex));
					break;

				default:
					Error(lineIndex, $"Unknown key `{lineKey}`");
					return false;
				}
			}

			if (!foundEnd) {
				Error(lineIndex, $"Missing `{DefEnd}`");
				return false;
			}

			if ((state.Flags1 & InstructionDefFlags1.RequireOpMaskRegister) != 0)
				state.Mvex.Flags1 |= MvexInfoFlags1.RequireOpMaskRegister;

			if ((state.Flags3 & (InstructionDefFlags3.ImpliedZeroingMasking | InstructionDefFlags3.OpMaskIsElementSelector)) != 0) {
				if (state.OpAccess.Length == 0 || state.OpAccess[0] != OpInfo.Write) {
					Error(opsLineIndex, "`implied-z` and `k-elem-selector` require first operand to be `write` (w=xxx)");
					return false;
				}
				state.OpAccess[0] = OpInfo.WriteForce;
			}

			var isPriv = privileged ??
				(state.Flags1 & (InstructionDefFlags1.Cpl0 | InstructionDefFlags1.Cpl1 | InstructionDefFlags1.Cpl2 | InstructionDefFlags1.Cpl3)) == InstructionDefFlags1.Cpl0;
			if (isPriv)
				state.Flags3 |= InstructionDefFlags3.Privileged;

			// The formatters depend on some other lines so parse the formatter lines later
			foreach (var (key, value, fmtLineIndex) in fmtKeyValues) {
				switch (key) {
				case "fast":
					if (state.FastInfo is not null) {
						Error(fmtLineIndex, $"Duplicate {key}");
						return false;
					}
					if (!TryReadFastFmt(value, out state.FastInfo, out error)) {
						Error(fmtLineIndex, error);
						return false;
					}
					break;

				case "gas":
					if (state.Gas is not null) {
						Error(fmtLineIndex, $"Duplicate {key}");
						return false;
					}
					if (!TryReadGasFmt(value, state, out state.Gas, out error)) {
						Error(fmtLineIndex, error);
						return false;
					}
					break;

				case "intel":
					if (state.Intel is not null) {
						Error(fmtLineIndex, $"Duplicate {key}");
						return false;
					}
					if (!TryReadIntelFmt(value, state, out state.Intel, out error)) {
						Error(fmtLineIndex, error);
						return false;
					}
					break;

				case "masm":
					if (state.Masm is not null) {
						Error(fmtLineIndex, $"Duplicate {key}");
						return false;
					}
					if (!TryReadMasmFmt(value, state, out state.Masm, out error)) {
						Error(fmtLineIndex, error);
						return false;
					}
					break;

				case "nasm":
					if (state.Nasm is not null) {
						Error(fmtLineIndex, $"Duplicate {key}");
						return false;
					}
					if (!TryReadNasmFmt(value, state, out state.Nasm, out error)) {
						Error(fmtLineIndex, error);
						return false;
					}
					break;

				default:
					Error(fmtLineIndex, $"Unknown key `{key}`");
					return false;
				}
			}

			if (state.InstrStrFmtOption == InstrStrFmtOption.None) {
				int mm1Index = instrStr.IndexOf("mm1", StringComparison.Ordinal);
				int mm2Index = instrStr.IndexOf("mm2", StringComparison.Ordinal);
				if (instrStr.Contains("k2 {k1}", StringComparison.Ordinal))
					state.InstrStrFmtOption = InstrStrFmtOption.OpMaskIsK1_or_NoGprSuffix;
				else if (mm2Index >= 0 && mm1Index < 0 && !(state.OpKinds.Length > 2 &&
					state.OpKinds[0].HasRegister && state.OpKinds[0].Register == Register.K0)) {
					state.InstrStrFmtOption = InstrStrFmtOption.IncVecIndex;
				}
				else if ((instrStr.EndsWith("mm", StringComparison.Ordinal) || instrStr.Contains("mm,", StringComparison.Ordinal)) &&
					!instrStr.Contains("mm1", StringComparison.Ordinal) && !instrStr.Contains("mm2", StringComparison.Ordinal)) {
					state.InstrStrFmtOption = InstrStrFmtOption.NoVecIndex;
				}
				else if (mm1Index >= 0 && mm2Index >= 0 && mm2Index < mm1Index)
					state.InstrStrFmtOption = InstrStrFmtOption.SwapVecIndex12;
				else if (!instrStr.Contains(',', StringComparison.Ordinal) &&
					state.OpKinds.Length == 2 &&
					state.OpKinds[0].OperandEncoding == OperandEncoding.ImpliedRegister && state.OpKinds[0].Register == Register.ST0 &&
					state.OpKinds[1].OperandEncoding == OperandEncoding.RegOpCode && state.OpKinds[0].Register == Register.ST0) {
					state.InstrStrFmtOption = InstrStrFmtOption.SkipOp0;
				}
				else if (instrStr.Contains("r8, r8,", StringComparison.Ordinal) || instrStr.Contains("r16, r16,", StringComparison.Ordinal) ||
					instrStr.Contains("r32, r32,", StringComparison.Ordinal) || instrStr.Contains("r64, r64,", StringComparison.Ordinal) ||
					instrStr.EndsWith("r8, r8", StringComparison.Ordinal) || instrStr.EndsWith("r16, r16", StringComparison.Ordinal) ||
					instrStr.EndsWith("r32, r32", StringComparison.Ordinal) || instrStr.EndsWith("r64, r64", StringComparison.Ordinal)) {
					state.InstrStrFmtOption = InstrStrFmtOption.OpMaskIsK1_or_NoGprSuffix;
				}
				else if (instrStr.Contains("zmm1 {k1}, k2, ", StringComparison.Ordinal) && instrStr.Contains("zmm3", StringComparison.Ordinal))
					state.InstrStrFmtOption = InstrStrFmtOption.VecIndexSameAsOpIndex;
			}

			state.Cflow ??= flowControlNext;
			state.DecoderOption ??= decoderOptionNone;
			state.MemorySize ??= memorySizeUnknown;
			state.MemorySize_Broadcast ??= memorySizeUnknown;
			if (state.MemorySize_Broadcast != memorySizeUnknown && state.MemorySize == memorySizeUnknown) {
				Error(state.LineIndex, "Broadcast memory type with no normal memory type");
				return false;
			}

			bool canHaveTupleType = state.OpCode.Encoding == EncodingKind.EVEX;
			bool needTupleType = canHaveTupleType && state.MemorySize != memorySizeUnknown;
			if (tupleType is null) {
				if (needTupleType) {
					Error(state.LineIndex, "Missing tuple type");
					return false;
				}
			}
			else {
				if (!needTupleType) {
					Error(state.LineIndex, "Useless tuple type");
					return false;
				}
			}
			const InstructionDefFlags1 CpuModeBits = InstructionDefFlags1.Bit16 | InstructionDefFlags1.Bit32 | InstructionDefFlags1.Bit64;

			if (parsedOpCode.Encoding == EncodingKind.MVEX) {
				if (state.Cpuid.Length != 1 || (CpuidFeature)state.Cpuid[0].Value != CpuidFeature.KNC) {
					Error(state.LineIndex, "KNC CPUID feature must be used");
					return false;
				}
			}
			var isKnc = state.Cpuid.Any(x => (CpuidFeature)x.Value == CpuidFeature.KNC);
			if (isKnc) {
				if ((state.Flags1 & CpuModeBits) != 0) {
					Error(lineIndex, "KNC is 64-bit only. The parser hard codes the bitness");
					return false;
				}
				state.Flags1 |= InstructionDefFlags1.Bit64;
				state.Flags3 |= InstructionDefFlags3.AsmIgnore;
				if (state.DecoderOption != decoderOptionNone) {
					Error(lineIndex, "The parser adds KNC decoder option");
					return false;
				}
				state.DecoderOption = toDecOptionValue[nameof(DecOptionValue.KNC)];
			}
			switch (state.OpCode.MvexEHBit) {
			case MvexEHBit.None:
				break;
			case MvexEHBit.EH0:
			case MvexEHBit.EH1:
				if ((state.Mvex.Flags1 & MvexInfoFlags1.EvictionHint) != 0) {
					Error(state.LineIndex, "{eh} can't be used when the instruction requires EH0 or EH1");
					return false;
				}
				break;
			default:
				throw new InvalidOperationException();
			}

			if ((state.Flags1 & CpuModeBits) == 0)
				state.Flags1 |= CpuModeBits;
			const InstructionDefFlags1 CplBits = InstructionDefFlags1.Cpl0 | InstructionDefFlags1.Cpl1 |
												InstructionDefFlags1.Cpl2 | InstructionDefFlags1.Cpl3;
			if ((state.Flags1 & CplBits) == 0)
				state.Flags1 |= CplBits;
			if (state.MemorySize_Broadcast != memorySizeUnknown)
				state.Flags1 |= InstructionDefFlags1.Broadcast;
			if (((state.Flags1 & InstructionDefFlags1.Broadcast) != 0) != ((parsedInstr.Flags & ParsedInstructionFlags.Broadcast) != 0)) {
				Error(lineIndex, $"Mem size enum and instruction string's mem op aren't both broadcast or both not broadcast");
				return false;
			}
			if ((parsedOpCode.Flags & ParsedOpCodeFlags.Fwait) != 0)
				state.Flags1 |= InstructionDefFlags1.Fwait;
			if ((parsedOpCode.Flags & ParsedOpCodeFlags.ModRegRmString) != 0)
				state.InstrStrFlags |= InstructionStringFlags.ModRegRmString;
			if ((parsedInstr.Flags & ParsedInstructionFlags.RoundingControl) != 0)
				state.Flags1 |= InstructionDefFlags1.RoundingControl;
			if ((parsedInstr.Flags & ParsedInstructionFlags.SuppressAllExceptions) != 0)
				state.Flags1 |= InstructionDefFlags1.SuppressAllExceptions;
			if ((parsedInstr.Flags & ParsedInstructionFlags.OpMask) != 0)
				state.Flags1 |= InstructionDefFlags1.OpMaskRegister;
			if ((parsedInstr.Flags & ParsedInstructionFlags.ZeroingMasking) != 0)
				state.Flags1 |= InstructionDefFlags1.ZeroingMasking;
			if ((parsedInstr.Flags & ParsedInstructionFlags.EvictionHint) != 0)
				state.Mvex.Flags1 |= MvexInfoFlags1.EvictionHint;
			switch (state.VmxMode) {
			case VmxMode.None:
				break;
			case VmxMode.VmxOp:
				state.Flags2 &= ~InstructionDefFlags2.UseOutsideVmxOp;
				break;
			case VmxMode.VmxRootOp:
				state.Flags2 &= ~(InstructionDefFlags2.UseOutsideVmxOp | InstructionDefFlags2.UseInVmxNonRootOp);
				break;
			case VmxMode.VmxNonRootOp:
				state.Flags2 &= ~(InstructionDefFlags2.UseOutsideVmxOp | InstructionDefFlags2.UseInVmxRootOp);
				break;
			default:
				throw new InvalidOperationException();
			}

			static string UppercaseFirstLetter(string s) =>
				s[0..1].ToUpperInvariant() + s[1..].ToLowerInvariant();

			state.MnemonicStr ??= parsedInstr.Mnemonic.ToLowerInvariant();
			if (state.MnemonicStr.ToLowerInvariant() == state.MnemonicStr)
				state.MnemonicStr = UppercaseFirstLetter(state.MnemonicStr);
			state.CodeMnemonic ??= state.MnemonicStr;
			if (state.CodeMnemonic.ToLowerInvariant() == state.CodeMnemonic)
				state.CodeMnemonic = UppercaseFirstLetter(state.CodeMnemonic);

			switch (state.OpCode.Encoding) {
			case EncodingKind.Legacy:
			case EncodingKind.D3NOW:
				break;
			case EncodingKind.VEX:
			case EncodingKind.EVEX:
			case EncodingKind.XOP:
			case EncodingKind.MVEX:
				state.Flags2 &= ~(InstructionDefFlags2.RealMode | InstructionDefFlags2.Virtual8086Mode);
				break;
			default:
				throw new InvalidOperationException();
			}

			if ((state.Flags1 & InstructionDefFlags1.Bit16) == 0) {
				state.Flags2 &= ~Decoder16;
				// It's possible to use a 32-bit code segment in RM but we assume it's 16-bit only
				state.Flags2 &= ~(InstructionDefFlags2.RealMode | InstructionDefFlags2.Virtual8086Mode);
			}
			if ((state.Flags1 & InstructionDefFlags1.Bit32) == 0)
				state.Flags2 &= ~Decoder32;
			if ((state.Flags1 & InstructionDefFlags1.Bit64) == 0) {
				state.Flags2 &= ~Decoder64;
				state.Flags2 &= ~InstructionDefFlags2.LongMode;
			}
			if ((state.Flags1 & (InstructionDefFlags1.Bit16 | InstructionDefFlags1.Bit32)) == 0) {
				state.Flags2 &= ~(InstructionDefFlags2.RealMode | InstructionDefFlags2.ProtectedMode |
					InstructionDefFlags2.Virtual8086Mode | InstructionDefFlags2.CompatibilityMode);
			}
			// v86 mode and SGX enclaves will #GP(0) since CPL=3
			if ((state.Flags1 & InstructionDefFlags1.Cpl3) == 0)
				state.Flags2 &= ~(InstructionDefFlags2.Virtual8086Mode | AllEnclaves);

			if ((state.Flags2 & AllDecoders) == 0) {
				Error(state.LineIndex, "Instruction can't be decoded by any decoder");
				return false;
			}
			if (((state.Flags2 & InstructionDefFlags2.LongMode) != 0) != ((state.Flags1 & InstructionDefFlags1.Bit64) != 0)) {
				Error(state.LineIndex, "is-long-mode != is-64-bit");
				return false;
			}
			if ((state.Flags2 & InstructionDefFlags2.UseInSeam) != 0 && (state.Flags2 & (InstructionDefFlags2.UseInVmxRootOp | InstructionDefFlags2.UseInVmxNonRootOp)) == 0) {
				Error(state.LineIndex, "In-SEAM but not (in-VMX-root or in-VMX-non-root)");
				return false;
			}
			if ((state.Flags2 & (InstructionDefFlags2.IntelVmExit | InstructionDefFlags2.IntelMayVmExit)) == (InstructionDefFlags2.IntelVmExit | InstructionDefFlags2.IntelMayVmExit)) {
				Error(state.LineIndex, "intel-vm-exit and intel-may-vm-exit can't both be used. Remove one of them.");
				return false;
			}
			if ((state.Flags2 & (InstructionDefFlags2.AmdVmExit | InstructionDefFlags2.AmdMayVmExit)) == (InstructionDefFlags2.AmdVmExit | InstructionDefFlags2.AmdMayVmExit)) {
				Error(state.LineIndex, "amd-vm-exit and amd-may-vm-exit can't both be used. Remove one of them.");
				return false;
			}
			if ((state.Flags2 & (InstructionDefFlags2.UseOutsideSmm | InstructionDefFlags2.UseInSmm)) == 0) {
				Error(state.LineIndex, "Invalid SMM flags");
				return false;
			}
			if ((state.Flags2 & (InstructionDefFlags2.UseOutsideEnclaveSgx | AllEnclaves)) == 0) {
				Error(state.LineIndex, "Invalid SGX flags");
				return false;
			}
			if ((state.Flags2 & (InstructionDefFlags2.UseOutsideVmxOp | InstructionDefFlags2.UseInVmxRootOp | InstructionDefFlags2.UseInVmxNonRootOp)) == 0) {
				Error(state.LineIndex, "Invalid VMX flags");
				return false;
			}
			if ((state.Flags2 & (InstructionDefFlags2.UseOutsideSeam | InstructionDefFlags2.UseInSeam)) == 0) {
				Error(state.LineIndex, "Invalid SEAM flags");
				return false;
			}

			var codeFormatter = new CodeFormatter(sb, regDefs, memSizeTbl, state.CodeMnemonic, state.CodeSuffix, state.CodeMemorySize,
				state.CodeMemorySizeSuffix, state.MemorySize, state.MemorySize_Broadcast, state.Flags1, state.Mvex.Flags1, parsedOpCode.Encoding,
				state.OpKinds, isKnc);
			var codeValue = codeFormatter.Format();
			if (usedCodeValues.TryGetValue(codeValue, out var otherLineIndex)) {
				Error(state.LineIndex,
					$"Duplicate Code value with another definition on line {otherLineIndex + 1}. " +
					"Add one or more of: code-mnemonic, code-suffix, code-memory-size, code-memory-size-suffix, " +
					"eg. code-suffix set to the opcode (search the defs for examples)");
				return false;
			}
			usedCodeValues.Add(codeValue, state.LineIndex);
			if (!toCode.TryGetValue(codeValue, out state.Code)) {
				Error(state.LineIndex, $"Code value doesn't exist: {codeValue}");
				return false;
			}
			if (!toMnemonic.TryGetValue(state.MnemonicStr, out state.Mnemonic)) {
				Error(state.LineIndex, $"Mnemonic value doesn't exist: {state.MnemonicStr}");
				return false;
			}

			var fmtMnemonic = state.MnemonicStr;
			// Ignore JKccD, no-one cares about KNC instructions
			if (state.ConditionCode != ConditionCode.None && (CpuidFeature)state.Cpuid[0].Value != CpuidFeature.KNC)
				fmtMnemonic = state.MnemonicCcPrefix + GetConditionCodeStr(state.ConditionCode) + state.MnemonicCcSuffix;
			fmtMnemonic = fmtMnemonic.ToLowerInvariant();
			PseudoOpsKind? pseudoOp = state.PseudoOpsKind is null ? null : (PseudoOpsKind)state.PseudoOpsKind.Value;
			if (!TryCreateFastDef(state.Code, fmtMnemonic, pseudoOp, state.FastInfo ?? new(), out var fastDef, out error)) {
				Error(state.LineIndex, "(fast) " + error);
				return false;
			}
			if (!TryCreateGasDef(state, state.Code, fmtMnemonic, pseudoOp, state.Gas ?? new(), out var gasDef, out error)) {
				Error(state.LineIndex, "(gas) " + error);
				return false;
			}
			if (!TryCreateIntelDef(state, state.Code, fmtMnemonic, pseudoOp, state.Intel ?? new(), out var intelDef, out error)) {
				Error(state.LineIndex, "(intel) " + error);
				return false;
			}
			if (!TryCreateMasmDef(state, state.Code, fmtMnemonic, pseudoOp, state.Masm ?? new(), out var masmDef, out error)) {
				Error(state.LineIndex, "(masm) " + error);
				return false;
			}
			if (!TryCreateNasmDef(state, state.Code, fmtMnemonic, pseudoOp, state.Nasm ?? new(), out var nasmDef, out error)) {
				Error(state.LineIndex, "(nasm) " + error);
				return false;
			}
			if ((state.OpCode.OperandSize == CodeSize.Code64 || state.OpCode.AddressSize == CodeSize.Code64) &&
				(state.Flags1 & (InstructionDefFlags1.Bit16 | InstructionDefFlags1.Bit32)) != 0) {
				Error(state.LineIndex, "This looks like a 64-bit only instruction but it's missing `64`");
				return false;
			}

			if (state.OpAccess.Length != state.OpKinds.Length)
				throw new InvalidOperationException();
			if (state.OpKinds.Any(a => a.OperandEncoding == OperandEncoding.None)) {
				Error(state.LineIndex, $"An operand with a `{nameof(OperandEncoding.None)}` value");
				return false;
			}
			accesses = state.ImpliedAccesses;
			var cpuidFeatureStrings = state.Cpuid.Select(a => toCpuidFeatureString[a]).ToArray();
			def = new InstructionDef(state.Code, state.OpCodeStr, state.InstrStr, state.Mnemonic, state.MemorySize,
				state.MemorySize_Broadcast, state.DecoderOption, state.Flags1, state.Flags2, state.Flags3, state.InstrStrFmtOption,
				state.InstrStrFlags, parsedInstr.ImpliedOps, state.Mvex,
				state.OpCode.MandatoryPrefix, state.OpCode.Table, state.OpCode.LBit, state.OpCode.WBit, state.OpCode.NDKind,
				state.OpCode.OpCode,
				state.OpCode.OpCodeLength, state.OpCode.GroupIndex, state.OpCode.RmGroupIndex,
				state.OpCode.OperandSize, state.OpCode.AddressSize, (TupleType)state.TupleType.Value, state.OpKinds,
				pseudoOp, state.Encoding, state.Cflow, state.ConditionCode, state.MnemonicCcPrefix, state.MnemonicCcSuffix,
				state.BranchKind, state.StackInfo, state.FpuStackIncrement,
				state.RflagsRead, state.RflagsUndefined, state.RflagsWritten, state.RflagsCleared, state.RflagsSet,
				state.Cpuid, cpuidFeatureStrings, state.OpAccess,
				fastDef, gasDef, intelDef, masmDef, nasmDef,
				state.AsmMnemonic);
			defLineIndex = state.LineIndex;
			return true;
		}

		sealed class FmtLineParser {
			readonly string[] parts;
			int index;

			public FmtLineParser(string data) {
				parts = data.Split(' ', StringSplitOptions.RemoveEmptyEntries);
				index = 0;
			}

			public IEnumerable<(string key, string value)> GetKeyValues() {
				for (; index < parts.Length; index++) {
					var part = parts[index];
					var (key, value) = ParserUtils.GetKeyValue(part);
					if (value == string.Empty)
						break;
					yield return (key, value);
				}
			}

			public bool TryGetNext([NotNullWhen(true)] out string? next) {
				if (index >= parts.Length) {
					next = null;
					return false;
				}
				else {
					next = parts[index++];
					return true;
				}
			}

			public bool TryGet(int count, [NotNullWhen(true)] out string[]? strings, [NotNullWhen(false)] out string? error) {
				int remaining = parts.Length - index;
				if (remaining < count) {
					error = $"Expected {count} args but only {remaining} found";
					strings = null;
					return false;
				}
				var result = remaining == 0 ? Array.Empty<string>() : new string[count];
				for (int i = 0; i < result.Length; i++)
					result[i] = parts[index++];
				strings = result;
				error = null;
				return true;
			}

			public string[] GetRest() {
				int count = parts.Length - index;
				var result = count == 0 ? Array.Empty<string>() : new string[count];
				for (int i = 0; i < result.Length; i++)
					result[i] = parts[index++];
				return result;
			}
		}

		static IEnumValue CreateFlagsEnum(EnumType type, List<EnumValue> values) {
			if (values.Count == 0)
				return type.Values.First(a => a.Value == 0);
			if (values[0].DeclaringType != type)
				throw new InvalidOperationException();
			if (values.Count == 1)
				return values[0];
			return new OrEnumValue(type, values.ToArray());
		}

		static string GetConditionCodeStr(ConditionCode cc) =>
			cc switch {
				ConditionCode.None  => throw new InvalidOperationException(),
				ConditionCode.o => "o",
				ConditionCode.no => "no",
				ConditionCode.b => "b",
				ConditionCode.ae => "ae",
				ConditionCode.e => "e",
				ConditionCode.ne => "ne",
				ConditionCode.be => "be",
				ConditionCode.a => "a",
				ConditionCode.s => "s",
				ConditionCode.ns => "ns",
				ConditionCode.p => "p",
				ConditionCode.np => "np",
				ConditionCode.l => "l",
				ConditionCode.ge => "ge",
				ConditionCode.le => "le",
				ConditionCode.g => "g",
				_ => throw new InvalidOperationException(),
			};

		// The order of strings must be the same as the CC_xxx enums, eg. CC_b, etc, see CC.cs.
		// The index into the return array is the low 4 bits of the opcode.
		static string[][] CreateOtherCCMnemonics(string prefix, string suffix = "") =>
			new string[][] {
				Array.Empty<string>(), // o
				Array.Empty<string>(), // no
				new[] { prefix + "c" + suffix, prefix + "nae" + suffix },
				new[] { prefix + "nb" + suffix, prefix + "nc" + suffix },
				new[] { prefix + "z" + suffix },
				new[] { prefix + "nz" + suffix },
				new[] { prefix + "na" + suffix },
				new[] { prefix + "nbe" + suffix },
				Array.Empty<string>(), // s
				Array.Empty<string>(), // ns
				new[] { prefix + "pe" + suffix },
				new[] { prefix + "po" + suffix },
				new[] { prefix + "nge" + suffix },
				new[] { prefix + "nl" + suffix },
				new[] { prefix + "ng" + suffix },
				new[] { prefix + "nle" + suffix },
			};
		static readonly string[][] jccOtherMnemonics = CreateOtherCCMnemonics("j");
		static readonly string[][] cmovccOtherMnemonics = CreateOtherCCMnemonics("cmov");
		static readonly string[][] setccOtherMnemonics = CreateOtherCCMnemonics("set");
		static readonly string[][] cmpccxaddOtherMnemonics = CreateOtherCCMnemonics("cmp", "xadd");

		static bool TryGetCcMnemonics(InstructionDefState def, out int ccIndex, [NotNullWhen(true)] out string[]? extraMnemonics, [NotNullWhen(false)] out string? error) {
			ccIndex = (int)(def.OpCode.OpCode & 0x0F);
			if (def.MnemonicCcPrefix == "cmov" && def.MnemonicCcSuffix == "")
				extraMnemonics = cmovccOtherMnemonics[ccIndex];
			else if (def.MnemonicCcPrefix == "set" && def.MnemonicCcSuffix == "")
				extraMnemonics = setccOtherMnemonics[ccIndex];
			else if (def.MnemonicCcPrefix == "cmp" && def.MnemonicCcSuffix == "xadd")
				extraMnemonics = cmpccxaddOtherMnemonics[ccIndex];
			else {
				extraMnemonics = null;
				error = "Unsupported cc mnemonic";
				return false;
			}
			error = null;
			return true;
		}

		static bool TryGetLoopCcMnemonics(InstructionDefState def, out int ccIndex, [NotNullWhen(true)] out string? extraLoopMnemonic) {
			if (def.BranchKind == BranchKind.Loop && def.ConditionCode == ConditionCode.e) {
				ccIndex = 4;
				extraLoopMnemonic = "loopz";
				return true;
			}
			else if (def.BranchKind == BranchKind.Loop && def.ConditionCode == ConditionCode.ne) {
				ccIndex = 5;
				extraLoopMnemonic = "loopnz";
				return true;
			}
			else {
				ccIndex = -1;
				extraLoopMnemonic = null;
				return false;
			}
		}

		bool TryGetSignExtendInfo(InstructionDefState def, bool noSignExtend, [NotNullWhen(true)] out EnumValue? enumValue, [NotNullWhen(false)] out string? error) {
			if (def.OpKinds.Length == 0) {
				enumValue = null;
				error = "Instruction has no operands";
				return false;
			}

			EnumValue value;
			if (noSignExtend)
				value = signExtendInfoType[nameof(Enums.Formatter.Nasm.SignExtendInfo.None)];
			else {
				var lastDef = def.OpKinds[^1];
				var immTuple = lastDef.OperandEncoding == OperandEncoding.Immediate ? (lastDef.ImmediateSize, lastDef.ImmediateSignExtSize) : (-1, -1);
				switch (immTuple) {
				case (16, 16): value = signExtendInfoType[nameof(Enums.Formatter.Nasm.SignExtendInfo.Sex2)]; break;
				case (32, 32): value = signExtendInfoType[nameof(Enums.Formatter.Nasm.SignExtendInfo.Sex4)]; break;
				case (8, 16): value = signExtendInfoType[nameof(Enums.Formatter.Nasm.SignExtendInfo.Sex1to2)]; break;
				case (8, 32): value = signExtendInfoType[nameof(Enums.Formatter.Nasm.SignExtendInfo.Sex1to4)]; break;
				case (8, 64): value = signExtendInfoType[nameof(Enums.Formatter.Nasm.SignExtendInfo.Sex1to8)]; break;
				case (32, 64): value = signExtendInfoType[nameof(Enums.Formatter.Nasm.SignExtendInfo.Sex4to8)]; break;
				default:
					enumValue = null;
					error = "Instruction's last operand isn't a sign-extended immediate";
					return false;
				}
			}

			enumValue = value;
			error = null;
			return true;
		}

		static bool TryGetBooleanArg(FmtLineParser parser, string name, out bool result, [NotNullWhen(false)] out string? error) {
			if (parser.TryGetNext(out var next)) {
				if (next == name)
					result = true;
				else {
					result = false;
					error = $"Unknown value `{next}`";
					return false;
				}
			}
			else
				result = false;

			error = null;
			return true;
		}

		static bool TryGetIsPseudoArg(FmtLineParser parser, out bool isPseudo, [NotNullWhen(false)] out string? error) =>
			TryGetBooleanArg(parser, "pseudo", out isPseudo, out error);

		static bool TryGetIsLoadArg(FmtLineParser parser, out bool isPseudo, [NotNullWhen(false)] out string? error) =>
			TryGetBooleanArg(parser, "load", out isPseudo, out error);

		static bool VerifyNoSuffix(GasState state, [NotNullWhen(false)] out string? error) {
			if (state.Suffix is null) {
				error = null;
				return true;
			}
			else {
				error = "suffix=X isn't supported";
				return false;
			}
		}

		static bool VerifyNoFlags(FmtState state, [NotNullWhen(false)] out string? error) {
			if (state.Flags.Count == 0) {
				error = null;
				return true;
			}
			else {
				error = "flags=xxx isn't supported";
				return false;
			}
		}

		bool TryReadFastFmt(string data, [NotNullWhen(true)] out FastState? state, [NotNullWhen(false)] out string? error) {
			state = null;

			var parser = new FmtLineParser(data);
			var result = new FastState();
			foreach (var (key, value) in parser.GetKeyValues()) {
				switch (key) {
				case "mnemonic":
					if (value == string.Empty) {
						error = $"Missing {key} value";
						return false;
					}
					if (result.Mnemonic is not null) {
						error = $"Duplicate {key} value";
						return false;
					}
					result.Mnemonic = value;
					break;

				case "flags":
					if (value == string.Empty) {
						error = $"Missing {key} value";
						return false;
					}
					foreach (var fl in value.Split(';', StringSplitOptions.RemoveEmptyEntries)) {
						switch (fl) {
						case "force-size=always":
							result.Flags.Add(fastFmtFlags[nameof(Enums.Formatter.Fast.FastFmtFlags.ForceMemSize)]);
							break;

						default:
							error = $"Unknown value {fl}";
							return false;
						}
					}
					break;

				default:
					error = $"Unknown key `{key}`";
					return false;
				}
			}

			var garbage = parser.GetRest();
			if (garbage.Length > 0) {
				error = $"Extra args: `{string.Join(' ', garbage)}`";
				return false;
			}

			state = result;
			error = null;
			return true;
		}

		bool TryCreateFastDef(EnumValue code, string defaultMnemonic, PseudoOpsKind? pseudoOp, FastState state, [NotNullWhen(true)] out FastFmtInstructionDef? fastDef, [NotNullWhen(false)] out string? error) {
			var mnemonic = state.Mnemonic ?? defaultMnemonic;
			if (pseudoOp is PseudoOpsKind pseudoOp2) {
				var name = pseudoOp2.ToString();
				// We only store 8 bits per Code value and we don't have enough bits left for all pseudo ops
				if (pseudoOp2 > PseudoOpsKind.vpcmpd6) {
					// If it fails, we can remove the above if check and block. C#/Rust fast fmt code should be updated too.
					if (fastFmtFlags.Values.Any(x => x.RawName == name))
						throw new InvalidOperationException();
					name = nameof(PseudoOpsKind.vpcmpd6);
				}
				state.Flags.Add(fastFmtFlags[name]);
			}
			fastDef = new FastFmtInstructionDef(code, mnemonic, new OrEnumValue(fastFmtFlags, state.Flags.ToArray()));
			error = null;
			return true;
		}

		bool TryReadGasFmt(string data, InstructionDefState def, [NotNullWhen(true)] out GasState? state, [NotNullWhen(false)] out string? error) {
			state = null;

			var parser = new FmtLineParser(data);
			var result = new GasState();
			foreach (var (key, value) in parser.GetKeyValues()) {
				switch (key) {
				case "mnemonic":
					if (value == string.Empty) {
						error = $"Missing {key} value";
						return false;
					}
					if (result.Mnemonic is not null) {
						error = $"Duplicate {key} value";
						return false;
					}
					result.Mnemonic = value;
					break;

				case "flags":
					if (value == string.Empty) {
						error = $"Missing {key} value";
						return false;
					}
					foreach (var fl in value.Split(';', StringSplitOptions.RemoveEmptyEntries)) {
						switch (fl) {
						case "keep-op-order":
							result.Flags.Add(gasInstrOpInfoFlags[nameof(Enums.Formatter.Gas.InstrOpInfoFlags.KeepOperandOrder)]);
							break;
						case "force-mem-suffix":
							result.Flags.Add(gasInstrOpInfoFlags[nameof(Enums.Formatter.Gas.InstrOpInfoFlags.MnemonicSuffixIfMem)]);
							break;
						case "indirect":
							result.Flags.Add(gasInstrOpInfoFlags[nameof(Enums.Formatter.Gas.InstrOpInfoFlags.IndirectOperand)]);
							break;
						case "o64":
							result.Flags.Add(gasInstrOpInfoFlags[nameof(Enums.Formatter.Gas.InstrOpInfoFlags.OpSize64)]);
							break;
						case "force-suffix":
							result.ForceSuffix = true;
							break;
						case "ignore-index":
							result.Flags.Add(gasInstrOpInfoFlags[nameof(Enums.Formatter.Gas.InstrOpInfoFlags.IgnoreIndexReg)]);
							break;
						case "osz-is-byte-directive":
							result.Flags.Add(gasInstrOpInfoFlags[nameof(Enums.Formatter.Gas.InstrOpInfoFlags.OpSizeIsByteDirective)]);
							break;
						default:
							error = $"Unknown value {fl}";
							return false;
						}
					}
					break;

				case "suffix":
					if (value == string.Empty) {
						error = $"Missing {key} value";
						return false;
					}
					if (result.Suffix is not null) {
						error = $"Duplicate {key} value";
						return false;
					}
					if (value.Length != 1) {
						error = $"Expected one character suffix `{value}`";
						return false;
					}
					result.Suffix = value[0];
					break;

				default:
					error = $"Unknown key `{key}`";
					return false;
				}
			}

			if (parser.TryGetNext(out var ctorKindStr)) {
				int addressSize;
				int operandSize;
				int ccIndex;
				string[]? args;
				bool isPseudo;
				switch (ctorKindStr) {
				case "ignore-const10":
					result.CtorKind = gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.AamAad)];
					break;

				case "asz":
					result.CtorKind = gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.asz)];
					if (!def.TryGetAddressSize(out addressSize, out error))
						return false;
					result.Args.Add(addressSize);
					break;

				case "bnd":
					result.CtorKind = gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.bnd)];
					result.Args.Add(result.GetUsedSuffix());
					result.Args.Add(CreateFlagsEnum(gasInstrOpInfoFlags, result.GetUsedFlags()));
					break;

				case "cc":
					if (!TryGetCcMnemonics(def, out ccIndex, out var extraMnemonics, out error))
						return false;
					result.CtorKind = extraMnemonics.Length switch {
						0 => gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.CC_1)],
						1 => gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.CC_2)],
						2 => gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.CC_3)],
						_ => throw new InvalidOperationException(),
					};
					result.Args.AddRange(extraMnemonics);
					result.Args.Add(result.GetUsedSuffix());
					result.Args.Add(ccIndex);
					break;

				case "decl":
					result.CtorKind = gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.DeclareData)];
					break;

				case "far":
					result.CtorKind = gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.far)];
					if (!def.TryGetOperandSize(out operandSize, out error))
						return false;
					result.Args.Add(result.GetUsedSuffix());
					result.Args.Add(operandSize);
					break;

				case "imul":
					result.CtorKind = gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.imul)];
					result.Args.Add(result.GetUsedSuffix());
					break;

				case "maskmovq":
					result.CtorKind = gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.maskmovq)];
					break;

				case "movabs":
					result.CtorKind = gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.movabs)];
					result.Args.Add(result.GetUsedSuffix());
					result.Args.Add("movabs");
					break;

				case "nop":
					if (!def.TryGetOperandSize(out operandSize, out error))
						return false;
					switch (operandSize) {
					case 16:
						result.Args.Add(16);
						result.Args.Add(registerType[nameof(Register.AX)]);
						break;
					case 32:
						result.Args.Add(32 | 64);
						result.Args.Add(registerType[nameof(Register.EAX)]);
						break;
					case 64:
						result.Args.Add(0);
						result.Args.Add(registerType[nameof(Register.RAX)]);
						break;
					default:
						throw new InvalidOperationException();
					}
					result.CtorKind = gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.nop)];
					break;

				case "mem16":
					result.CtorKind = gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.mem16)];
					result.Args.Add(result.GetUsedSuffix());
					break;

				case "osz-suffix-1":
					result.CtorKind = gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.OpSize)];
					var codeSize = def.OpCode.OperandSize == CodeSize.Unknown ? CodeSize.Code64 : def.OpCode.OperandSize;
					result.Args.Add(codeSizeType[codeSize.ToString()]);
					break;

				case "osz-suffix-2":
					result.CtorKind = gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.OpSize2_bnd)];
					if (!parser.TryGet(3, out args, out error))
						return false;
					result.Args.AddRange(args);
					break;

				case "osz-suffix-3":
					result.CtorKind = gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.OpSize3)];
					args = parser.GetRest();
					if (args.Length == 0) {
						error = "Expected at least one argument";
						return false;
					}
					uint osz = 0;
					foreach (var arg in args) {
						if (!ParserUtils.TryParseUInt32(arg, out var value, out error))
							return false;
						osz |= value;
					}
					result.Args.Add(result.GetUsedSuffix());
					result.Args.Add((int)osz);
					break;

				case "osz":
					result.CtorKind = gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.os)];
					result.Args.Add(def.GetOperandSize(64));
					result.Args.Add((def.Flags1 & InstructionDefFlags1.Bnd) != 0);
					result.Args.Add(CreateFlagsEnum(gasInstrOpInfoFlags, result.GetUsedFlags()));
					break;

				case "loop":
					if (!def.TryGetAddressSize(out addressSize, out error))
						return false;
					if (TryGetLoopCcMnemonics(def, out ccIndex, out var extraLoopMnemonic)) {
						result.CtorKind = gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.os_loopcc)];
						result.Args.Add(extraLoopMnemonic);
						result.Args.Add(result.GetUsedSuffix());
						result.Args.Add(ccIndex);
					}
					else {
						result.CtorKind = gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.os_loop)];
						result.Args.Add(result.GetUsedSuffix());
					}
					result.Args.Add(def.GetOperandSize(64));
					result.Args.Add(addressSize);
					break;

				case "osz-mem-2":
					if (!def.TryGetOperandSize(out operandSize, out error))
						return false;
					result.CtorKind = gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.os_mem2)];
					result.Args.Add(result.GetUsedSuffix());
					result.Args.Add(operandSize == 16 ? 16 : 32 | 64);
					break;

				case "osz-suffix-4":
					result.Args.Add(result.GetUsedSuffix());
					result.Args.Add(def.GetOperandSize(64));
					result.Args.Add((def.Flags1 & InstructionDefFlags1.Bnd) != 0);
					if (result.Flags.Count > 0) {
						result.CtorKind = gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.os2_4)];
						result.Args.Add(CreateFlagsEnum(gasInstrOpInfoFlags, result.GetUsedFlags()));
					}
					else
						result.CtorKind = gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.os2_3)];
					break;

				case "xmm0":
					result.CtorKind = gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.pblendvb)];
					break;

				case "reg16":
					result.CtorKind = gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.Reg16)];
					break;

				case "reg32":
					result.CtorKind = gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.Reg32)];
					break;

				case "st1":
					result.CtorKind = gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.ST_STi)];
					break;

				case "st2":
					result.CtorKind = gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.STi_ST)];
					if (!TryGetIsPseudoArg(parser, out isPseudo, out error))
						return false;
					result.Args.Add(isPseudo);
					break;

				case "st1-ignore-st1":
					if (!TryGetIsPseudoArg(parser, out isPseudo, out error))
						return false;
					result.CtorKind = gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.STIG1)];
					result.Args.Add(isPseudo);
					break;

				default:
					error = $"Unknown value `{ctorKindStr}`";
					return false;
				}
			}

			var garbage = parser.GetRest();
			if (garbage.Length > 0) {
				error = $"Extra args: `{string.Join(' ', garbage)}`";
				return false;
			}

			state = result;
			error = null;
			return true;
		}

		bool TryCreateGasDef(InstructionDefState def, EnumValue code, string defaultMnemonic, PseudoOpsKind? pseudoOp, GasState state, [NotNullWhen(true)] out FmtInstructionDef? gasDef, [NotNullWhen(false)] out string? error) {
			gasDef = null;

			var mnemonic = state.Mnemonic ?? defaultMnemonic;
			var ctorKind = state.CtorKind;
			if (ctorKind is null) {
				if (state.Args.Count > 0)
					throw new InvalidOperationException();

				if (def.PseudoOpsKind is not null) {
					state.Args.Add(def.PseudoOpsKind);
					if (pseudoOp == PseudoOpsKind.pclmulqdq || pseudoOp == PseudoOpsKind.vpclmulqdq)
						ctorKind = gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.pclmulqdq)];
					else {
						ctorKind = gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.pops)];
						state.Args.Add((def.Flags1 & InstructionDefFlags1.SuppressAllExceptions) != 0);
					}
				}
				else if ((def.Flags1 & InstructionDefFlags1.RoundingControl) != 0) {
					if (def.OpKinds.Length == 0) {
						error = "Instruction has no operands";
						return false;
					}
					int erIndex = def.OpKinds[^1] is var lastDef && lastDef.OperandEncoding == OperandEncoding.RegMemModrmRm &&
						(lastDef.Register == Register.EAX || lastDef.Register == Register.RAX) ? 1 : 0;
					if (state.Flags.Count != 0 || state.Suffix != null) {
						ctorKind = gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.er_4)];
						state.Args.Add(state.GetUsedSuffix());
						state.Args.Add(erIndex);
						state.Args.Add(CreateFlagsEnum(gasInstrOpInfoFlags, state.GetUsedFlags()));
					}
					else {
						ctorKind = gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.er_2)];
						state.Args.Add(erIndex);
					}
				}
				else if ((def.Flags1 & InstructionDefFlags1.SuppressAllExceptions) != 0) {
					if (def.OpKinds.Length == 0) {
						error = "Instruction has no operands";
						return false;
					}
					ctorKind = gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.sae)];
					var saeIndex = def.OpKinds[^1].OperandEncoding == OperandEncoding.Immediate ? 1 : 0;
					state.Args.Add(saeIndex);
				}
				else if (def.BranchKind == BranchKind.JccShort || def.BranchKind == BranchKind.JccNear) {
					int ccIndex = (int)(def.OpCode.OpCode & 0x0F);
					var extraMnemonics = jccOtherMnemonics[ccIndex];
					ctorKind = extraMnemonics.Length switch {
						0 => gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.os_jcc_1)],
						1 => gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.os_jcc_2)],
						2 => gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.os_jcc_3)],
						_ => throw new InvalidOperationException(),
					};
					state.Args.AddRange(extraMnemonics);
					state.Args.Add(ccIndex);
					state.Args.Add(def.GetOperandSize(64));
				}
				else {
					if (state.ForceSuffix) {
						ctorKind = gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.Normal_2c)];
						state.Args.Add(state.GetUsedSuffix());
					}
					else if (state.Suffix is not null) {
						state.Args.Add(state.GetUsedSuffix());
						if (state.Flags.Count > 0) {
							ctorKind = gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.Normal_3)];
							state.Args.Add(CreateFlagsEnum(gasInstrOpInfoFlags, state.GetUsedFlags()));
						}
						else
							ctorKind = gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.Normal_2a)];
					}
					else {
						if (state.Flags.Count > 0) {
							ctorKind = gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.Normal_2b)];
							state.Args.Add(CreateFlagsEnum(gasInstrOpInfoFlags, state.GetUsedFlags()));
						}
						else
							ctorKind = gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.Normal_1)];
					}
				}
			}

			if (!state.UsedSuffix && !VerifyNoSuffix(state, out error))
				return false;
			if (!state.UsedFlags && !VerifyNoFlags(state, out error))
				return false;

			gasDef = new FmtInstructionDef(code, mnemonic, ctorKind, state.Args.ToArray());
			error = null;
			return true;
		}

		bool TryReadIntelFmt(string data, InstructionDefState def, [NotNullWhen(true)] out IntelState? state, [NotNullWhen(false)] out string? error) {
			state = null;

			var parser = new FmtLineParser(data);
			var result = new IntelState();
			foreach (var (key, value) in parser.GetKeyValues()) {
				switch (key) {
				case "mnemonic":
					if (value == string.Empty) {
						error = $"Missing {key} value";
						return false;
					}
					if (result.Mnemonic is not null) {
						error = $"Duplicate {key} value";
						return false;
					}
					result.Mnemonic = value;
					break;

				case "flags":
					if (value == string.Empty) {
						error = $"Missing {key} value";
						return false;
					}
					foreach (var fl in value.Split(';', StringSplitOptions.RemoveEmptyEntries)) {
						switch (fl) {
						case "force-size=always":
							result.Flags.Add(intelInstrOpInfoFlags[nameof(Enums.Formatter.Intel.InstrOpInfoFlags.ShowNoMemSize_ForceSize)]);
							result.Flags.Add(intelInstrOpInfoFlags[nameof(Enums.Formatter.Intel.InstrOpInfoFlags.ShowMinMemSize_ForceSize)]);
							break;
						case "force-size=default":
							result.Flags.Add(intelInstrOpInfoFlags[nameof(Enums.Formatter.Intel.InstrOpInfoFlags.ShowNoMemSize_ForceSize)]);
							break;
						case "mem-size=ignore":
							result.Flags.Add(intelInstrOpInfoFlags[nameof(Enums.Formatter.Intel.InstrOpInfoFlags.MemSize_Nothing)]);
							break;
						case "short":
							result.Flags.Add(intelInstrOpInfoFlags[nameof(Enums.Formatter.Intel.InstrOpInfoFlags.BranchSizeInfo_Short)]);
							break;
						case "far":
							result.Flags.Add(intelInstrOpInfoFlags[nameof(Enums.Formatter.Intel.InstrOpInfoFlags.FarMnemonic)]);
							break;
						case "o64":
							result.Flags.Add(intelInstrOpInfoFlags[nameof(Enums.Formatter.Intel.InstrOpInfoFlags.OpSize64)]);
							break;
						case "ignore-index":
							result.Flags.Add(intelInstrOpInfoFlags[nameof(Enums.Formatter.Intel.InstrOpInfoFlags.IgnoreIndexReg)]);
							break;
						default:
							error = $"Unknown value {fl}";
							return false;
						}
					}
					break;

				default:
					error = $"Unknown key `{key}`";
					return false;
				}
			}

			if (parser.TryGetNext(out var ctorKindStr)) {
				int addressSize;
				int operandSize;
				int ccIndex;
				bool isPseudo;
				switch (ctorKindStr) {
				case "asz":
					result.CtorKind = intelCtorKind[nameof(Enums.Formatter.Intel.CtorKind.asz)];
					if (!def.TryGetAddressSize(out addressSize, out error))
						return false;
					result.Args.Add(addressSize);
					break;

				case "bcst":
					result.CtorKind = intelCtorKind[nameof(Enums.Formatter.Intel.CtorKind.bcst)];
					result.Args.Add(CreateFlagsEnum(intelInstrOpInfoFlags, result.GetUsedFlags()));
					break;

				case "bnd":
					result.CtorKind = intelCtorKind[nameof(Enums.Formatter.Intel.CtorKind.bnd)];
					result.Args.Add(CreateFlagsEnum(intelInstrOpInfoFlags, result.GetUsedFlags()));
					break;

				case "cc":
					if (!TryGetCcMnemonics(def, out ccIndex, out var extraMnemonics, out error))
						return false;
					result.CtorKind = extraMnemonics.Length switch {
						0 => intelCtorKind[nameof(Enums.Formatter.Intel.CtorKind.CC_1)],
						1 => intelCtorKind[nameof(Enums.Formatter.Intel.CtorKind.CC_2)],
						2 => intelCtorKind[nameof(Enums.Formatter.Intel.CtorKind.CC_3)],
						_ => throw new InvalidOperationException(),
					};
					result.Args.AddRange(extraMnemonics);
					result.Args.Add(ccIndex);
					break;

				case "decl":
					result.CtorKind = intelCtorKind[nameof(Enums.Formatter.Intel.CtorKind.DeclareData)];
					break;

				case "invlpga":
					result.CtorKind = intelCtorKind[nameof(Enums.Formatter.Intel.CtorKind.invlpga)];
					if (!def.TryGetAddressSize(out addressSize, out error))
						return false;
					result.Args.Add(addressSize);
					break;

				case "imul":
					result.CtorKind = intelCtorKind[nameof(Enums.Formatter.Intel.CtorKind.imul)];
					break;

				case "maskmovq":
					result.CtorKind = intelCtorKind[nameof(Enums.Formatter.Intel.CtorKind.maskmovq)];
					break;

				case "osz-mem-2":
					result.CtorKind = intelCtorKind[nameof(Enums.Formatter.Intel.CtorKind.memsize)];
					int osz;
					if (parser.TryGetNext(out var next)) {
						if (next == "16")
							osz = 16;
						else {
							error = $"Unknown arg `{next}`";
							return false;
						}
					}
					else
						osz = 32 | 64;
					result.Args.Add(osz);
					break;

				case "movabs":
					result.CtorKind = intelCtorKind[nameof(Enums.Formatter.Intel.CtorKind.movabs)];
					break;

				case "nop":
					if (!def.TryGetOperandSize(out operandSize, out error))
						return false;
					switch (operandSize) {
					case 16:
						result.Args.Add(16);
						result.Args.Add(registerType[nameof(Register.AX)]);
						break;
					case 32:
						result.Args.Add(32 | 64);
						result.Args.Add(registerType[nameof(Register.EAX)]);
						break;
					case 64:
						result.Args.Add(0);
						result.Args.Add(registerType[nameof(Register.RAX)]);
						break;
					default:
						throw new InvalidOperationException();
					}
					result.CtorKind = intelCtorKind[nameof(Enums.Formatter.Intel.CtorKind.nop)];
					break;

				case "kmask-op":
					result.CtorKind = intelCtorKind[nameof(Enums.Formatter.Intel.CtorKind.opmask_op)];
					break;

				case "osz-bnd":
					result.CtorKind = intelCtorKind[nameof(Enums.Formatter.Intel.CtorKind.os_bnd)];
					result.Args.Add(def.GetOperandSize(64));
					break;

				case "loop":
					if (!def.TryGetAddressSize(out addressSize, out error))
						return false;
					var rcxReg = addressSize switch {
						16 => registerType[nameof(Register.CX)],
						32 => registerType[nameof(Register.ECX)],
						64 => registerType[nameof(Register.RCX)],
						_ => throw new InvalidOperationException(),
					};
					if (TryGetLoopCcMnemonics(def, out ccIndex, out var extraLoopMnemonic)) {
						result.CtorKind = intelCtorKind[nameof(Enums.Formatter.Intel.CtorKind.os_loopcc)];
						result.Args.Add(extraLoopMnemonic);
						result.Args.Add(ccIndex);
					}
					else
						result.CtorKind = intelCtorKind[nameof(Enums.Formatter.Intel.CtorKind.os_loop)];
					result.Args.Add(def.GetOperandSize(64));
					result.Args.Add(rcxReg);
					break;

				case "osz":
					result.Args.Add(def.GetOperandSize(64));
					if (result.Flags.Count > 0) {
						result.Args.Add(CreateFlagsEnum(intelInstrOpInfoFlags, result.GetUsedFlags()));
						result.CtorKind = intelCtorKind[nameof(Enums.Formatter.Intel.CtorKind.os3)];
					}
					else
						result.CtorKind = intelCtorKind[nameof(Enums.Formatter.Intel.CtorKind.os2)];
					break;

				case "reg":
					result.CtorKind = intelCtorKind[nameof(Enums.Formatter.Intel.CtorKind.reg)];
					if (!parser.TryGet(1, out var strings, out error))
						return false;
					if (!TryGetValue(toRegisterIgnoreCase, strings[0], out var enumValue, out error))
						return false;
					result.Args.Add(enumValue);
					break;

				case "reg16":
					result.CtorKind = intelCtorKind[nameof(Enums.Formatter.Intel.CtorKind.Reg16)];
					break;

				case "reg32":
					result.CtorKind = intelCtorKind[nameof(Enums.Formatter.Intel.CtorKind.Reg32)];
					break;

				case "st1":
					if (!TryGetIsPseudoArg(parser, out isPseudo, out error))
						return false;
					result.CtorKind = intelCtorKind[nameof(Enums.Formatter.Intel.CtorKind.ST_STi)];
					result.Args.Add(isPseudo);
					break;

				case "st2":
					if (!TryGetIsPseudoArg(parser, out isPseudo, out error))
						return false;
					result.CtorKind = intelCtorKind[nameof(Enums.Formatter.Intel.CtorKind.STi_ST)];
					result.Args.Add(isPseudo);
					break;

				case "add-st1":
					if (!TryGetIsLoadArg(parser, out var isLoad, out error))
						return false;
					result.Args.Add(CreateFlagsEnum(intelInstrOpInfoFlags, result.GetUsedFlags()));
					if (isLoad) {
						result.CtorKind = intelCtorKind[nameof(Enums.Formatter.Intel.CtorKind.ST1_3)];
						result.Args.Add(isLoad);
					}
					else
						result.CtorKind = intelCtorKind[nameof(Enums.Formatter.Intel.CtorKind.ST1_2)];
					break;

				case "add-st2":
					result.CtorKind = intelCtorKind[nameof(Enums.Formatter.Intel.CtorKind.ST2)];
					result.Args.Add(CreateFlagsEnum(intelInstrOpInfoFlags, result.GetUsedFlags()));
					break;

				case "ignore-first":
					result.CtorKind = intelCtorKind[nameof(Enums.Formatter.Intel.CtorKind.StringIg0)];
					break;

				case "ignore-last":
					result.CtorKind = intelCtorKind[nameof(Enums.Formatter.Intel.CtorKind.StringIg1)];
					break;

				default:
					error = $"Unknown value `{ctorKindStr}`";
					return false;
				}
			}

			var garbage = parser.GetRest();
			if (garbage.Length > 0) {
				error = $"Extra args: `{string.Join(' ', garbage)}`";
				return false;
			}

			state = result;
			error = null;
			return true;
		}

		bool TryCreateIntelDef(InstructionDefState def, EnumValue code, string defaultMnemonic, PseudoOpsKind? pseudoOp, IntelState state, [NotNullWhen(true)] out FmtInstructionDef? intelDef, [NotNullWhen(false)] out string? error) {
			intelDef = null;

			var mnemonic = state.Mnemonic ?? defaultMnemonic;
			var ctorKind = state.CtorKind;
			if (ctorKind is null) {
				if (state.Args.Count > 0)
					throw new InvalidOperationException();

				if (def.PseudoOpsKind is not null) {
					state.Args.Add(def.PseudoOpsKind);
					if (pseudoOp == PseudoOpsKind.pclmulqdq || pseudoOp == PseudoOpsKind.vpclmulqdq)
						ctorKind = intelCtorKind[nameof(Enums.Formatter.Intel.CtorKind.pclmulqdq)];
					else
						ctorKind = intelCtorKind[nameof(Enums.Formatter.Intel.CtorKind.pops)];
				}
				else if (def.BranchKind == BranchKind.JccShort || def.BranchKind == BranchKind.JccNear) {
					int ccIndex = (int)(def.OpCode.OpCode & 0x0F);
					var extraMnemonics = jccOtherMnemonics[ccIndex];
					if (def.BranchKind == BranchKind.JccShort)
						state.Flags.Add(intelInstrOpInfoFlags[nameof(Enums.Formatter.Intel.InstrOpInfoFlags.BranchSizeInfo_Short)]);
					if (state.Flags.Count > 0) {
						ctorKind = extraMnemonics.Length switch {
							0 => intelCtorKind[nameof(Enums.Formatter.Intel.CtorKind.os_jcc_b_1)],
							1 => intelCtorKind[nameof(Enums.Formatter.Intel.CtorKind.os_jcc_b_2)],
							2 => intelCtorKind[nameof(Enums.Formatter.Intel.CtorKind.os_jcc_b_3)],
							_ => throw new InvalidOperationException(),
						};
					}
					else {
						ctorKind = extraMnemonics.Length switch {
							0 => intelCtorKind[nameof(Enums.Formatter.Intel.CtorKind.os_jcc_a_1)],
							1 => intelCtorKind[nameof(Enums.Formatter.Intel.CtorKind.os_jcc_a_2)],
							2 => intelCtorKind[nameof(Enums.Formatter.Intel.CtorKind.os_jcc_a_3)],
							_ => throw new InvalidOperationException(),
						};
					}
					state.Args.AddRange(extraMnemonics);
					state.Args.Add(ccIndex);
					state.Args.Add(def.GetOperandSize(64));
					if (state.Flags.Count > 0)
						state.Args.Add(CreateFlagsEnum(intelInstrOpInfoFlags, state.GetUsedFlags()));
				}
				else {
					if (state.Flags.Count > 0) {
						ctorKind = intelCtorKind[nameof(Enums.Formatter.Intel.CtorKind.Normal_2)];
						state.Args.Add(CreateFlagsEnum(intelInstrOpInfoFlags, state.GetUsedFlags()));
					}
					else
						ctorKind = intelCtorKind[nameof(Enums.Formatter.Intel.CtorKind.Normal_1)];
				}
			}

			if (!state.UsedFlags && !VerifyNoFlags(state, out error))
				return false;

			intelDef = new FmtInstructionDef(code, mnemonic, ctorKind, state.Args.ToArray());
			error = null;
			return true;
		}

		static bool TryGetCharArg(FmtLineParser parser, out char c, [NotNullWhen(false)] out string? error) {
			if (!parser.TryGetNext(out var next)) {
				c = '\0';
				error = "Missing char arg";
				return false;
			}
			if (next.Length != 1) {
				c = '\0';
				error = $"Expected a char arg: `{next}`";
				return false;
			}

			c = next[0];
			error = null;
			return true;
		}

		bool TryReadMasmFmt(string data, InstructionDefState def, [NotNullWhen(true)] out MasmState? state, [NotNullWhen(false)] out string? error) {
			state = null;

			var parser = new FmtLineParser(data);
			var result = new MasmState();
			foreach (var (key, value) in parser.GetKeyValues()) {
				switch (key) {
				case "mnemonic":
					if (value == string.Empty) {
						error = $"Missing {key} value";
						return false;
					}
					if (result.Mnemonic is not null) {
						error = $"Duplicate {key} value";
						return false;
					}
					result.Mnemonic = value;
					break;

				case "flags":
					if (value == string.Empty) {
						error = $"Missing {key} value";
						return false;
					}
					foreach (var fl in value.Split(';', StringSplitOptions.RemoveEmptyEntries)) {
						switch (fl) {
						case "force-size=always":
							result.Flags.Add(masmInstrOpInfoFlags[nameof(Enums.Formatter.Masm.InstrOpInfoFlags.ShowNoMemSize_ForceSize)]);
							result.Flags.Add(masmInstrOpInfoFlags[nameof(Enums.Formatter.Masm.InstrOpInfoFlags.ShowMinMemSize_ForceSize)]);
							break;
						case "force-size=default":
							result.Flags.Add(masmInstrOpInfoFlags[nameof(Enums.Formatter.Masm.InstrOpInfoFlags.ShowNoMemSize_ForceSize)]);
							break;
						case "mem-size=mmx":
							result.Flags.Add(masmInstrOpInfoFlags[nameof(Enums.Formatter.Masm.InstrOpInfoFlags.MemSize_Mmx)]);
							break;
						case "mem-size=dorq":
							result.Flags.Add(masmInstrOpInfoFlags[nameof(Enums.Formatter.Masm.InstrOpInfoFlags.MemSize_DwordOrQword)]);
							break;
						case "mem-size=normal":
							result.Flags.Add(masmInstrOpInfoFlags[nameof(Enums.Formatter.Masm.InstrOpInfoFlags.MemSize_Normal)]);
							break;
						default:
							error = $"Unknown value {fl}";
							return false;
						}
					}
					break;

				default:
					error = $"Unknown key `{key}`";
					return false;
				}
			}

			if (parser.TryGetNext(out var ctorKindStr)) {
				char c;
				int addressSize;
				int operandSize;
				int ccIndex;
				string? next;
				string[]? args;
				bool isPseudo;
				CodeSize codeSize;
				switch (ctorKindStr) {
				case "ignore-const10":
					result.CtorKind = masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.AamAad)];
					break;

				case "asz-string-ax":
				case "asz-string-ay":
				case "asz-string-dx":
				case "asz-string-xy":
				case "asz-string-ya":
				case "asz-string-yd":
				case "asz-string-yx":
					result.CtorKind = ctorKindStr switch {
						"asz-string-ax" => masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.AX)],
						"asz-string-ay" => masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.AY)],
						"asz-string-dx" => masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.DX)],
						"asz-string-xy" => masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.XY)],
						"asz-string-ya" => masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.YA)],
						"asz-string-yd" => masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.YD)],
						"asz-string-yx" => masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.YX)],
						_ => throw new InvalidOperationException(),
					};
					if (!TryGetCharArg(parser, out c, out error))
						return false;
					result.Args.Add(c);
					break;

				case "bnd":
					result.CtorKind = masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.bnd)];
					result.Args.Add(CreateFlagsEnum(masmInstrOpInfoFlags, result.GetUsedFlags()));
					break;

				case "cc":
					if (!TryGetCcMnemonics(def, out ccIndex, out var extraMnemonics, out error))
						return false;
					if (result.Flags.Count > 0) {
						result.CtorKind = extraMnemonics.Length switch {
							0 => masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.CCb_1)],
							1 => masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.CCb_2)],
							2 => masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.CCb_3)],
							_ => throw new InvalidOperationException(),
						};
					}
					else {
						result.CtorKind = extraMnemonics.Length switch {
							0 => masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.CCa_1)],
							1 => masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.CCa_2)],
							2 => masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.CCa_3)],
							_ => throw new InvalidOperationException(),
						};
					}
					result.Args.AddRange(extraMnemonics);
					result.Args.Add(ccIndex);
					if (result.Flags.Count > 0)
						result.Args.Add(CreateFlagsEnum(masmInstrOpInfoFlags, result.GetUsedFlags()));
					break;

				case "decl":
					result.CtorKind = masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.DeclareData)];
					break;

				case "gidt":
					result.CtorKind = masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.fword)];
					switch (def.GetOperandSize(64)) {
					case 16:
						result.Args.Add('w');
						result.Args.Add(codeSizeType[nameof(CodeSize.Code16)]);
						break;
					case 32:
						result.Args.Add('d');
						result.Args.Add(codeSizeType[nameof(CodeSize.Code32)]);
						break;
					case 64:
						result.Args.Add('q');
						result.Args.Add(codeSizeType[nameof(CodeSize.Code64)]);
						break;
					default:
						throw new InvalidOperationException();
					}
					result.Args.Add(CreateFlagsEnum(masmInstrOpInfoFlags, result.GetUsedFlags()));
					break;

				case "imul":
					result.CtorKind = masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.imul)];
					break;

				case "int3":
					result.CtorKind = masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.Int3)];
					break;

				case "invlpga":
					result.CtorKind = masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.invlpga)];
					if (!def.TryGetAddressSize(out addressSize, out error))
						return false;
					result.Args.Add(addressSize);
					break;

				case "loop1":
					if (!TryGetCharArg(parser, out c, out _))
						c = '\0';
					if (TryGetLoopCcMnemonics(def, out ccIndex, out next)) {
						if (c != '\0')
							next += c;
						result.CtorKind = masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.Loopcc1)];
						result.Args.Add(next);
						result.Args.Add(ccIndex);
					}
					else
						throw new InvalidOperationException();
					break;

				case "loop2":
					if (!def.TryGetAddressSize(out addressSize, out error))
						return false;
					if (TryGetLoopCcMnemonics(def, out ccIndex, out next)) {
						result.CtorKind = masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.Loopcc2)];
						result.Args.Add(next);
						switch (addressSize) {
						case 16:
							result.Args.Add('w');
							result.Args.Add(ccIndex);
							result.Args.Add(codeSizeType[nameof(CodeSize.Code16)]);
							break;
						case 32:
							result.Args.Add('d');
							result.Args.Add(ccIndex);
							result.Args.Add(codeSizeType[nameof(CodeSize.Code32)]);
							break;
						case 64:
							result.Args.Add('q');
							result.Args.Add(ccIndex);
							result.Args.Add(codeSizeType[nameof(CodeSize.Code64)]);
							break;
						default:
							throw new InvalidOperationException();
						}
					}
					else
						throw new InvalidOperationException();
					break;

				case "maskmovq":
					result.CtorKind = masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.maskmovq)];
					result.Args.Add(CreateFlagsEnum(masmInstrOpInfoFlags, result.GetUsedFlags()));
					break;

				case "osz-mem-2":
					result.CtorKind = masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.memsize)];
					int osz;
					if (parser.TryGetNext(out next)) {
						if (next == "16")
							osz = 16;
						else {
							error = $"Unknown arg `{next}`";
							return false;
						}
					}
					else
						osz = 32 | 64;
					result.Args.Add(osz);
					break;

				case "monitor":
					result.CtorKind = masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.monitor)];
					if (!def.TryGetAddressSize(out addressSize, out error))
						return false;
					switch (addressSize) {
					case 16:
						result.Args.Add(registerType[nameof(Register.AX)]);
						result.Args.Add(registerType[nameof(Register.ECX)]);
						result.Args.Add(registerType[nameof(Register.EDX)]);
						break;
					case 32:
						result.Args.Add(registerType[nameof(Register.EAX)]);
						result.Args.Add(registerType[nameof(Register.ECX)]);
						result.Args.Add(registerType[nameof(Register.EDX)]);
						break;
					case 64:
						result.Args.Add(registerType[nameof(Register.RAX)]);
						result.Args.Add(registerType[nameof(Register.RCX)]);
						result.Args.Add(registerType[nameof(Register.RDX)]);
						break;
					default:
						throw new InvalidOperationException();
					}
					break;

				case "mwait":
					result.CtorKind = masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.mwait)];
					break;

				case "mwaitx":
					result.CtorKind = masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.mwaitx)];
					break;

				case "nop":
					if (!def.TryGetOperandSize(out operandSize, out error))
						return false;
					switch (operandSize) {
					case 16:
						result.Args.Add(16);
						result.Args.Add(registerType[nameof(Register.AX)]);
						break;
					case 32:
						result.Args.Add(32 | 64);
						result.Args.Add(registerType[nameof(Register.EAX)]);
						break;
					case 64:
						result.Args.Add(0);
						result.Args.Add(registerType[nameof(Register.RAX)]);
						break;
					default:
						throw new InvalidOperationException();
					}
					result.CtorKind = masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.nop)];
					break;

				case "osz-suffix-1":
					result.CtorKind = masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.OpSize_1)];
					codeSize = def.OpCode.OperandSize == CodeSize.Unknown ? CodeSize.Code64 : def.OpCode.OperandSize;
					result.Args.Add(codeSizeType[codeSize.ToString()]);
					break;

				case "osz-suffix-1-loop":
					result.CtorKind = masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.OpSize_2)];
					codeSize = def.OpCode.AddressSize == CodeSize.Unknown ? CodeSize.Code64 : def.OpCode.AddressSize;
					switch (codeSize) {
					case CodeSize.Code16: result.Args.Add('w'); break;
					case CodeSize.Code32: result.Args.Add('d'); break;
					case CodeSize.Code64: result.Args.Add('q'); break;
					default: throw new InvalidOperationException();
					}
					result.Args.Add(codeSizeType[codeSize.ToString()]);
					break;

				case "osz-suffix-2":
					result.CtorKind = masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.OpSize2)];
					if (!parser.TryGet(3, out args, out error))
						return false;
					result.Args.AddRange(args);
					result.Args.Add((def.Flags1 & InstructionDefFlags1.Bnd) != 0);
					break;

				case "xmm0":
					result.CtorKind = masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.pblendvb)];
					break;

				case "reg":
					result.CtorKind = masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.reg)];
					if (!parser.TryGet(1, out var strings, out error))
						return false;
					if (!TryGetValue(toRegisterIgnoreCase, strings[0], out var enumValue, out error))
						return false;
					result.Args.Add(enumValue);
					break;

				case "reg16":
					result.CtorKind = masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.Reg16)];
					result.Args.Add(CreateFlagsEnum(masmInstrOpInfoFlags, result.GetUsedFlags()));
					break;

				case "reg32":
					result.CtorKind = masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.Reg32)];
					result.Args.Add(CreateFlagsEnum(masmInstrOpInfoFlags, result.GetUsedFlags()));
					break;

				case "reverse":
					result.CtorKind = masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.reverse)];
					break;

				case "st1":
					result.CtorKind = masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.ST_STi)];
					break;

				case "st2":
					if (!TryGetIsPseudoArg(parser, out isPseudo, out error))
						return false;
					result.CtorKind = masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.STi_ST)];
					result.Args.Add(isPseudo);
					break;

				case "st1-ignore-st1":
					if (!TryGetIsPseudoArg(parser, out isPseudo, out error))
						return false;
					result.CtorKind = masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.STIG1)];
					result.Args.Add(isPseudo);
					break;

				case "xlat":
					result.CtorKind = masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.XLAT)];
					break;

				default:
					error = $"Unknown value `{ctorKindStr}`";
					return false;
				}
			}

			var garbage = parser.GetRest();
			if (garbage.Length > 0) {
				error = $"Extra args: `{string.Join(' ', garbage)}`";
				return false;
			}

			state = result;
			error = null;
			return true;
		}

		bool TryCreateMasmDef(InstructionDefState def, EnumValue code, string defaultMnemonic, PseudoOpsKind? pseudoOp, MasmState state, [NotNullWhen(true)] out FmtInstructionDef? masmDef, [NotNullWhen(false)] out string? error) {
			masmDef = null;

			var mnemonic = state.Mnemonic ?? defaultMnemonic;
			var ctorKind = state.CtorKind;
			if (ctorKind is null) {
				if (state.Args.Count > 0)
					throw new InvalidOperationException();

				if (def.PseudoOpsKind is not null) {
					state.Args.Add(def.PseudoOpsKind);
					if (pseudoOp == PseudoOpsKind.pclmulqdq || pseudoOp == PseudoOpsKind.vpclmulqdq)
						ctorKind = masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.pclmulqdq)];
					else if (state.Flags.Count > 0) {
						ctorKind = masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.pops_3)];
						state.Args.Add(CreateFlagsEnum(masmInstrOpInfoFlags, state.GetUsedFlags()));
					}
					else
						ctorKind = masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.pops_2)];
				}
				else if (def.BranchKind == BranchKind.JccShort || def.BranchKind == BranchKind.JccNear) {
					int ccIndex = (int)(def.OpCode.OpCode & 0x0F);
					var extraMnemonics = jccOtherMnemonics[ccIndex];
					ctorKind = extraMnemonics.Length switch {
						0 => masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.jcc_1)],
						1 => masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.jcc_2)],
						2 => masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.jcc_3)],
						_ => throw new InvalidOperationException(),
					};
					state.Args.AddRange(extraMnemonics);
					state.Args.Add(ccIndex);
				}
				else {
					if (state.Flags.Count > 0) {
						ctorKind = masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.Normal_2)];
						state.Args.Add(CreateFlagsEnum(masmInstrOpInfoFlags, state.GetUsedFlags()));
					}
					else
						ctorKind = masmCtorKind[nameof(Enums.Formatter.Masm.CtorKind.Normal_1)];
				}
			}

			if (!state.UsedFlags && !VerifyNoFlags(state, out error))
				return false;

			masmDef = new FmtInstructionDef(code, mnemonic, ctorKind, state.Args.ToArray());
			error = null;
			return true;
		}

		bool TryReadNasmFmt(string data, InstructionDefState def, [NotNullWhen(true)] out NasmState? state, [NotNullWhen(false)] out string? error) {
			state = null;

			var parser = new FmtLineParser(data);
			var result = new NasmState();
			foreach (var (key, value) in parser.GetKeyValues()) {
				switch (key) {
				case "mnemonic":
					if (value == string.Empty) {
						error = $"Missing {key} value";
						return false;
					}
					if (result.Mnemonic is not null) {
						error = $"Duplicate {key} value";
						return false;
					}
					result.Mnemonic = value;
					break;

				case "flags":
					if (value == string.Empty) {
						error = $"Missing {key} value";
						return false;
					}
					foreach (var fl in value.Split(';', StringSplitOptions.RemoveEmptyEntries)) {
						switch (fl) {
						case "force-size=always":
							result.Flags.Add(nasmInstrOpInfoFlags[nameof(Enums.Formatter.Nasm.InstrOpInfoFlags.ShowNoMemSize_ForceSize)]);
							result.Flags.Add(nasmInstrOpInfoFlags[nameof(Enums.Formatter.Nasm.InstrOpInfoFlags.ShowMinMemSize_ForceSize)]);
							break;
						case "force-size=default":
							result.Flags.Add(nasmInstrOpInfoFlags[nameof(Enums.Formatter.Nasm.InstrOpInfoFlags.ShowNoMemSize_ForceSize)]);
							break;
						case "mem-size=ignore":
							result.Flags.Add(nasmInstrOpInfoFlags[nameof(Enums.Formatter.Nasm.InstrOpInfoFlags.MemSize_Nothing)]);
							break;
						case "mem-size=unknown":
							result.UnknownMemSize = true;
							break;
						case "no-sx":
							result.NoSignExtend = true;
							break;
						case "short":
							result.Flags.Add(nasmInstrOpInfoFlags[nameof(Enums.Formatter.Nasm.InstrOpInfoFlags.BranchSizeInfo_Short)]);
							break;
						case "o64":
							result.Flags.Add(nasmInstrOpInfoFlags[nameof(Enums.Formatter.Nasm.InstrOpInfoFlags.OpSize64)]);
							break;
						default:
							error = $"Unknown value {fl}";
							return false;
						}
					}
					break;

				default:
					error = $"Unknown key `{key}`";
					return false;
				}
			}

			if (parser.TryGetNext(out var ctorKindStr)) {
				int addressSize;
				int operandSize;
				int ccIndex;
				string? next;
				string[]? args;
				bool isPseudo;
				EnumValue? enumValue;
				switch (ctorKindStr) {
				case "ignore-const10":
					result.CtorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.AamAad)];
					break;

				case "asz":
					result.CtorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.asz)];
					if (!def.TryGetAddressSize(out addressSize, out error))
						return false;
					result.Args.Add(addressSize);
					break;

				case "bcst":
					result.CtorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.bcst)];
					result.Args.Add(CreateFlagsEnum(nasmInstrOpInfoFlags, result.GetUsedFlags()));
					break;

				case "bnd":
					result.CtorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.bnd)];
					result.Args.Add(CreateFlagsEnum(nasmInstrOpInfoFlags, result.GetUsedFlags()));
					break;

				case "cc":
					if (!TryGetCcMnemonics(def, out ccIndex, out var extraMnemonics, out error))
						return false;
					result.CtorKind = extraMnemonics.Length switch {
						0 => nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.CC_1)],
						1 => nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.CC_2)],
						2 => nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.CC_3)],
						_ => throw new InvalidOperationException(),
					};
					result.Args.AddRange(extraMnemonics);
					result.Args.Add(ccIndex);
					break;

				case "decl":
					result.CtorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.DeclareData)];
					break;

				case "far":
					if (!def.TryGetOperandSize(out operandSize, out error))
						return false;
					result.CtorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.far)];
					result.Args.Add(operandSize);
					break;

				case "far-mem":
					if (!def.TryGetOperandSize(out operandSize, out error))
						return false;
					result.CtorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.far_mem)];
					result.Args.Add(operandSize);
					break;

				case "imul":
					result.CtorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.imul)];
					if (!TryGetSignExtendInfo(def, result.NoSignExtend, out enumValue, out error))
						return false;
					result.Args.Add(enumValue);
					break;

				case "invlpga":
					result.CtorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.invlpga)];
					if (!def.TryGetAddressSize(out addressSize, out error))
						return false;
					result.Args.Add(addressSize);
					break;

				case "maskmovq":
					result.CtorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.maskmovq)];
					break;

				case "movabs":
					result.CtorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.movabs)];
					break;

				case "nop":
					if (!def.TryGetOperandSize(out operandSize, out error))
						return false;
					switch (operandSize) {
					case 16:
						result.Args.Add(16);
						result.Args.Add(registerType[nameof(Register.AX)]);
						break;
					case 32:
						result.Args.Add(32 | 64);
						result.Args.Add(registerType[nameof(Register.EAX)]);
						break;
					case 64:
						result.Args.Add(0);
						result.Args.Add(registerType[nameof(Register.RAX)]);
						break;
					default:
						throw new InvalidOperationException();
					}
					result.CtorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.nop)];
					break;

				case "osz-suffix-1":
					result.CtorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.OpSize)];
					var codeSize = def.OpCode.OperandSize == CodeSize.Unknown ? CodeSize.Code64 : def.OpCode.OperandSize;
					result.Args.Add(codeSizeType[codeSize.ToString()]);
					break;

				case "osz-suffix-2":
					result.CtorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.OpSize2_bnd)];
					if (!parser.TryGet(3, out args, out error))
						return false;
					result.Args.AddRange(args);
					break;

				case "osz-suffix-3":
					result.CtorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.OpSize3)];
					switch (def.GetOperandSize(64)) {
					case 16:
						result.Args.Add('w');
						result.Args.Add(16);
						break;
					case 32:
						result.Args.Add('d');
						result.Args.Add(32 | 64);
						break;
					case 64:
						result.Args.Add('q');
						result.Args.Add(0);
						break;
					default:
						throw new InvalidOperationException();
					}
					break;

				case "osz":
					result.Args.Add(def.GetOperandSize(64));
					if (result.Flags.Count > 0) {
						result.Args.Add(CreateFlagsEnum(nasmInstrOpInfoFlags, result.GetUsedFlags()));
						result.CtorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.os_3)];
					}
					else
						result.CtorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.os_2)];
					break;

				case "osz-call":
					result.CtorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.os_call)];
					result.Args.Add(def.GetOperandSize(64));
					result.Args.Add((def.Flags1 & InstructionDefFlags1.Bnd) != 0);
					break;

				case "loop":
					if (!def.TryGetAddressSize(out addressSize, out error))
						return false;
					var rcxReg = addressSize switch {
						16 => registerType[nameof(Register.CX)],
						32 => registerType[nameof(Register.ECX)],
						64 => registerType[nameof(Register.RCX)],
						_ => throw new InvalidOperationException(),
					};
					if (TryGetLoopCcMnemonics(def, out ccIndex, out var extraLoopMnemonic)) {
						result.CtorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.os_loopcc)];
						result.Args.Add(extraLoopMnemonic);
						result.Args.Add(ccIndex);
					}
					else
						result.CtorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.os_loop)];
					result.Args.Add(def.GetOperandSize(64));
					result.Args.Add(rcxReg);
					break;

				case "osz-mem-1":
					result.CtorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.os_mem)];
					result.Args.Add(def.GetOperandSize(64));
					break;

				case "osz-mem-2":
					if (!def.TryGetOperandSize(out operandSize, out error))
						return false;
					result.CtorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.os_mem2)];
					result.Args.Add(operandSize == 16 ? 16 : 32 | 64);
					result.Args.Add(CreateFlagsEnum(nasmInstrOpInfoFlags, result.GetUsedFlags()));
					break;

				case "osz-reg16":
					result.CtorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.os_mem_reg16)];
					result.Args.Add(def.GetOperandSize(64));
					break;

				case "xmm0":
					result.CtorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.pblendvb)];
					if (result.UnknownMemSize)
						result.Args.Add(memorySizeUnknown);
					else {
						if (def.MemorySize is null) {
							error = "Missing memory size";
							return false;
						}
						result.Args.Add(def.MemorySize);
					}
					break;

				case "sx-push-imm8":
					result.CtorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.push_imm8)];
					if (!TryGetSignExtendInfo(def, result.NoSignExtend, out enumValue, out error))
						return false;
					result.Args.Add(def.GetOperandSize(64));
					result.Args.Add(enumValue);
					break;

				case "sx-push-imm":
					result.CtorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.push_imm)];
					if (!TryGetSignExtendInfo(def, result.NoSignExtend, out enumValue, out error))
						return false;
					result.Args.Add(def.GetOperandSize(64));
					result.Args.Add(enumValue);
					break;

				case "reg16":
					result.CtorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.Reg16)];
					break;

				case "reg32":
					result.CtorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.Reg32)];
					break;

				case "reverse":
					result.CtorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.reverse)];
					break;

				case "st1":
					if (!TryGetIsPseudoArg(parser, out isPseudo, out error))
						return false;
					result.CtorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.STIG1)];
					result.Args.Add(isPseudo);
					break;

				case "st2":
					if (!parser.TryGetNext(out next)) {
						error = "Missing arg";
						return false;
					}
					switch (next) {
					case "pseudo":
						result.CtorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.STIG2_2a)];
						result.Args.Add(true);
						break;

					case "to":
						result.CtorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.STIG2_2b)];
						result.Flags.Add(nasmInstrOpInfoFlags[nameof(Enums.Formatter.Nasm.InstrOpInfoFlags.RegisterTo)]);
						result.Args.Add(CreateFlagsEnum(nasmInstrOpInfoFlags, result.GetUsedFlags()));
						break;

					default:
						error = $"Unknown arg `{next}`";
						return false;
					}
					break;

				case "sx":
					if (!TryGetSignExtendInfo(def, result.NoSignExtend, out enumValue, out error))
						return false;
					result.CtorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.SignExt_3)];
					result.Args.Add(enumValue);
					result.Args.Add(CreateFlagsEnum(nasmInstrOpInfoFlags, result.GetUsedFlags()));
					break;

				case "sx2":
					if (!TryGetSignExtendInfo(def, result.NoSignExtend, out enumValue, out error))
						return false;
					result.CtorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.SignExt_4)];
					result.Args.Add(signExtendInfoType[nameof(Enums.Formatter.Nasm.SignExtendInfo.Sex4)]);
					result.Args.Add(enumValue);
					result.Args.Add(CreateFlagsEnum(nasmInstrOpInfoFlags, result.GetUsedFlags()));
					break;

				case "asz-string":
					result.CtorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.String)];
					break;

				case "xlat":
					result.CtorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.XLAT)];
					break;

				default:
					error = $"Unknown value `{ctorKindStr}`";
					return false;
				}
			}

			var garbage = parser.GetRest();
			if (garbage.Length > 0) {
				error = $"Extra args: `{string.Join(' ', garbage)}`";
				return false;
			}

			state = result;
			error = null;
			return true;
		}

		bool TryCreateNasmDef(InstructionDefState def, EnumValue code, string defaultMnemonic, PseudoOpsKind? pseudoOp, NasmState state, [NotNullWhen(true)] out FmtInstructionDef? nasmDef, [NotNullWhen(false)] out string? error) {
			nasmDef = null;

			var mnemonic = state.Mnemonic ?? defaultMnemonic;
			var ctorKind = state.CtorKind;
			if (ctorKind is null) {
				if (state.Args.Count > 0)
					throw new InvalidOperationException();

				if (def.PseudoOpsKind is not null) {
					state.Args.Add(def.PseudoOpsKind);
					if (pseudoOp == PseudoOpsKind.pclmulqdq || pseudoOp == PseudoOpsKind.vpclmulqdq)
						ctorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.pclmulqdq)];
					else
						ctorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.pops)];
				}
				else if ((def.Flags1 & InstructionDefFlags1.RoundingControl) != 0) {
					if (def.OpKinds.Length == 0) {
						error = "Instruction has no operands";
						return false;
					}
					int erIndex = def.OpKinds[^1] is var lastDef && lastDef.OperandEncoding == OperandEncoding.RegMemModrmRm &&
						(lastDef.Register == Register.EAX || lastDef.Register == Register.RAX) ? def.OpKinds.Length - 1 : def.OpKinds.Length;
					state.Args.Add(erIndex);
					if (state.Flags.Count != 0) {
						ctorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.er_3)];
						state.Args.Add(CreateFlagsEnum(nasmInstrOpInfoFlags, state.GetUsedFlags()));
					}
					else
						ctorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.er_2)];
				}
				else if ((def.Flags1 & InstructionDefFlags1.SuppressAllExceptions) != 0) {
					if (def.OpKinds.Length == 0) {
						error = "Instruction has no operands";
						return false;
					}
					ctorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.sae)];
					var saeIndex = def.OpKinds[^1].OperandEncoding == OperandEncoding.Immediate ? def.OpKinds.Length - 1 : def.OpKinds.Length;
					state.Args.Add(saeIndex);
				}
				else if (def.BranchKind == BranchKind.JccShort || def.BranchKind == BranchKind.JccNear) {
					int ccIndex = (int)(def.OpCode.OpCode & 0x0F);
					var extraMnemonics = jccOtherMnemonics[ccIndex];
					if (def.BranchKind == BranchKind.JccShort)
						state.Flags.Add(nasmInstrOpInfoFlags[nameof(Enums.Formatter.Nasm.InstrOpInfoFlags.BranchSizeInfo_Short)]);
					if (state.Flags.Count > 0) {
						ctorKind = extraMnemonics.Length switch {
							0 => nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.os_jcc_b_1)],
							1 => nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.os_jcc_b_2)],
							2 => nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.os_jcc_b_3)],
							_ => throw new InvalidOperationException(),
						};
					}
					else {
						ctorKind = extraMnemonics.Length switch {
							0 => nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.os_jcc_a_1)],
							1 => nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.os_jcc_a_2)],
							2 => nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.os_jcc_a_3)],
							_ => throw new InvalidOperationException(),
						};
					}
					state.Args.AddRange(extraMnemonics);
					state.Args.Add(ccIndex);
					state.Args.Add(def.GetOperandSize(64));
					if (state.Flags.Count > 0)
						state.Args.Add(CreateFlagsEnum(nasmInstrOpInfoFlags, state.GetUsedFlags()));
				}
				else {
					if (state.Flags.Count > 0) {
						ctorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.Normal_2)];
						state.Args.Add(CreateFlagsEnum(nasmInstrOpInfoFlags, state.GetUsedFlags()));
					}
					else
						ctorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.Normal_1)];
				}
			}

			if (!state.UsedFlags && !VerifyNoFlags(state, out error))
				return false;

			nasmDef = new FmtInstructionDef(code, mnemonic, ctorKind, state.Args.ToArray());
			error = null;
			return true;
		}

		static bool TryParseRflags(string rflagsStr, out RflagsBits rflags, [NotNullWhen(false)] out string? error) {
			rflags = RflagsBits.None;

			foreach (var c in rflagsStr) {
				switch (c) {
				case RflagsBitsConstants.OF: rflags |= RflagsBits.OF; break;
				case RflagsBitsConstants.SF: rflags |= RflagsBits.SF; break;
				case RflagsBitsConstants.ZF: rflags |= RflagsBits.ZF; break;
				case RflagsBitsConstants.AF: rflags |= RflagsBits.AF; break;
				case RflagsBitsConstants.CF: rflags |= RflagsBits.CF; break;
				case RflagsBitsConstants.PF: rflags |= RflagsBits.PF; break;
				case RflagsBitsConstants.DF: rflags |= RflagsBits.DF; break;
				case RflagsBitsConstants.IF: rflags |= RflagsBits.IF; break;
				case RflagsBitsConstants.AC: rflags |= RflagsBits.AC; break;
				case RflagsBitsConstants.C0: rflags |= RflagsBits.C0; break;
				case RflagsBitsConstants.C1: rflags |= RflagsBits.C1; break;
				case RflagsBitsConstants.C2: rflags |= RflagsBits.C2; break;
				case RflagsBitsConstants.C3: rflags |= RflagsBits.C3; break;
				case RflagsBitsConstants.UIF: rflags |= RflagsBits.UIF; break;
				default: error = $"Unknown rflags char `{c}`"; return false;
				}
			}

			error = null;
			return true;
		}

		static IEnumerable<(string key, string value)> GetKeyValues(string line) {
			foreach (var s in line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
				yield return ParserUtils.GetKeyValue(s);
		}

		static bool TryGetDefLineKeyValue(string line, [NotNullWhen(true)] out string? key, [NotNullWhen(true)] out string? value, [NotNullWhen(false)] out string? errMsg) {
			int index = line.IndexOf(':', StringComparison.Ordinal);
			if (index < 0) {
				errMsg = "Missing `:`";
				key = null;
				value = null;
				return false;
			}
			else {
				errMsg = null;
				key = line[0..index].Trim();
				value = line[(index + 1)..].Trim();
				return true;
			}
		}

		bool TryParseDefLine(string line, [NotNullWhen(true)] out string? opCodeStr, [NotNullWhen(true)] out string? instrStr, [NotNullWhen(true)] out EnumValue[]? cpuid, out EnumValue? tupleType, [NotNullWhen(false)] out string? error) {
			var parts = line.ToString().Split('|');
			if (parts.Length != 3 && parts.Length != 4) {
				opCodeStr = null;
				instrStr = null;
				cpuid = null;
				tupleType = null;
				error = "Too many/few `|` characters";
				return false;
			}

			opCodeStr = parts[0].Trim();
			instrStr = parts[1].Trim();

			var cpuidStrs = parts[2].Split(' ', StringSplitOptions.RemoveEmptyEntries);
			cpuid = new EnumValue[cpuidStrs.Length];
			for (int i = 0; i < cpuid.Length; i++) {
				if (!TryGetValue(toCpuid, cpuidStrs[i], out var cpuidValue, out var errMsg)) {
					error = errMsg;
					tupleType = null;
					return false;
				}
				cpuid[i] = cpuidValue;
			}

			if (parts.Length == 3)
				tupleType = null;
			else {
				if (parts.Length != 4)
					throw new InvalidOperationException();
				if (!TryGetValue(toTupleType, parts[3], out var ttValue, out var errMsg)) {
					error = errMsg;
					tupleType = null;
					return false;
				}
				tupleType = ttValue;
			}

			error = null;
			return true;
		}

		static bool TryGetValue(Dictionary<string, EnumValue> dict, string name, [NotNullWhen(true)] out EnumValue? enumValue, [NotNullWhen(false)] out string? error) {
			name = name.Trim();
			if (dict.TryGetValue(name, out var ev)) {
				enumValue = ev;
				error = null;
				return true;
			}
			else {
				enumValue = null;
				error = $"Invalid enum value `{name}`";
				return false;
			}
		}

		static bool ParseMvexValidFns(string value, out byte validBits, [NotNullWhen(false)] out string? error) {
			validBits = 0;
			int bit = 0;
			foreach (var c in value) {
				if (c == '_')
					continue;
				switch (c) {
				case '_':
					break;
				case '0':
					bit++;
					break;
				case '1':
					validBits |= (byte)(1 << bit);
					bit++;
					break;
				default:
					error = $"Invalid char `{c}`, expected `0`, `1` or `_`";
					return false;
				}
			}
			if (bit != 8) {
				error = $"Expected 8 bits but got {bit} bits";
				return false;
			}

			error = null;
			return true;
		}
	}

	sealed class FastState {
		public string? Mnemonic;
		public List<EnumValue> Flags = new();
	}
	abstract class FmtState {
		public string? Mnemonic;
		public List<EnumValue> Flags = new();
		public EnumValue? CtorKind;
		public List<object> Args = new();

		public bool UsedFlags;
		public List<EnumValue> GetUsedFlags() {
			UsedFlags = true;
			return Flags;
		}
	}
	sealed class GasState : FmtState {
		public char? Suffix;
		public bool ForceSuffix;

		public bool UsedSuffix;

		public char GetUsedSuffix() {
			UsedSuffix = true;
			return Suffix.GetValueOrDefault();
		}
	}
	sealed class IntelState : FmtState {
	}
	sealed class MasmState : FmtState {
	}
	sealed class NasmState : FmtState {
		public bool UnknownMemSize;
		public bool NoSignExtend;
	}

	sealed class InstructionDefState {
		public readonly int LineIndex;
		public readonly string OpCodeStr;
		public readonly string InstrStr;
		public readonly EnumValue[] Cpuid;
		public EnumValue TupleType;
		public readonly EnumValue Encoding;
		public OpInfo[] OpAccess;
		public OpCodeOperandKindDef[] OpKinds;
		public BranchKind BranchKind;
		public string[] ImpliedInstructionStringArgs;
		public string? MnemonicStr;
		public string? CodeMnemonic;
		public string? CodeSuffix;
		public string? CodeMemorySize;
		public string? CodeMemorySizeSuffix;
		public EnumValue? Code;
		public EnumValue? Mnemonic;
		public EnumValue? Cflow;
		public ImpliedAccesses? ImpliedAccesses;
		public ConditionCode ConditionCode;
		public string? MnemonicCcPrefix;
		public string? MnemonicCcSuffix;
		public EnumValue? DecoderOption;
		public InstructionDefFlags1 Flags1;
		public InstructionDefFlags2 Flags2;
		public InstructionDefFlags3 Flags3;
		public VmxMode VmxMode;
		public RflagsBits RflagsRead;
		public RflagsBits RflagsUndefined;
		public RflagsBits RflagsWritten;
		public RflagsBits RflagsCleared;
		public RflagsBits RflagsSet;
		public StackInfo StackInfo;
		public int FpuStackIncrement;
		public InstrStrFmtOption InstrStrFmtOption;
		public InstructionStringFlags InstrStrFlags;
		public EnumValue? PseudoOpsKind;
		public EnumValue? MemorySize;
		public EnumValue? MemorySize_Broadcast;
		public MvexInstructionInfo Mvex;
		public OpCodeDef OpCode;
		public FastState? FastInfo;
		public GasState? Gas;
		public IntelState? Intel;
		public MasmState? Masm;
		public NasmState? Nasm;
		public string? AsmMnemonic;

		public InstructionDefState(int lineIndex, string opCodeStr, string instrStr, EnumValue[] cpuid, EnumValue tupleType, EnumValue encoding) {
			LineIndex = lineIndex;
			OpCodeStr = opCodeStr;
			InstrStr = instrStr;
			Cpuid = cpuid;
			TupleType = tupleType;
			OpAccess = Array.Empty<OpInfo>();
			OpKinds = Array.Empty<OpCodeOperandKindDef>();
			Encoding = encoding;
			ImpliedInstructionStringArgs = Array.Empty<string>();
		}

		public bool TryGetAddressSize(out int addressSize, [NotNullWhen(false)] out string? error) {
			error = null;
			switch (OpCode.AddressSize) {
			case CodeSize.Unknown: addressSize = 0; error = "Missing address size"; return false;
			case CodeSize.Code16: addressSize = 16; return true;
			case CodeSize.Code32: addressSize = 32; return true;
			case CodeSize.Code64: addressSize = 64; return true;
			default: throw new InvalidOperationException();
			}
		}

		public bool TryGetOperandSize(out int operandSize, [NotNullWhen(false)] out string? error) {
			error = null;
			switch (OpCode.OperandSize) {
			case CodeSize.Unknown: operandSize = 0; error = "Missing operand size"; return false;
			case CodeSize.Code16: operandSize = 16; return true;
			case CodeSize.Code32: operandSize = 32; return true;
			case CodeSize.Code64: operandSize = 64; return true;
			default: throw new InvalidOperationException();
			}
		}

		public int GetOperandSize(int defaultValue) {
			if (!TryGetOperandSize(out var operandSize, out _))
				return defaultValue;
			return operandSize;
		}
	}
}
