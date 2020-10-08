/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using Generator.Enums;
using Generator.Enums.Encoder;
using Generator.Enums.Formatter;
using Generator.Enums.InstructionInfo;
using Generator.Formatters;
using Generator.InstructionInfo;

namespace Generator.Tables {
	sealed class InstructionDefsReader {
		readonly StringBuilder sb;
		readonly GenTypes genTypes;
		readonly MemorySizeInfoTable memSizeTbl;
		readonly EnumValue tupleTypeN1;
		readonly string filename;
		readonly string[] lines;
		readonly List<string> errors;
		readonly Dictionary<string, int> usedCodeValues;
		readonly Dictionary<string, EnumValue> toCode;
		readonly Dictionary<string, EnumValue> toMnemonic;
		readonly Dictionary<string, EnumValue> toEncoding;
		readonly Dictionary<string, EnumValue> toCpuid;
		readonly Dictionary<string, EnumValue> toTupleType;
		readonly Dictionary<string, EnumValue> toConditionCode;
		readonly Dictionary<string, EnumValue> toFlowControl;
		readonly Dictionary<string, EnumValue> toCodeInfo;
		readonly Dictionary<string, EnumValue> toPseudoOpsKind;
		readonly Dictionary<string, EnumValue> toDecOptionValue;
		readonly Dictionary<string, EnumValue> toMemorySize;
		readonly Dictionary<string, EnumValue> toOpCodeOperandKind;
		readonly Dictionary<string, EnumValue> toRegisterIgnoreCase;
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
		readonly EnumValue memorySizeUnknown;
		readonly EnumValue flowControlNext;
		readonly EnumValue decoderOptionNone;
		readonly EnumValue codeInfoNone;
		readonly List<OpInfo> opAccess;
		readonly List<OpCodeOperandKind> opKinds;
		readonly List<(string key, string value)> fmtKeyValues;
		const string DefBeginPrefix = "INSTRUCTION:";
		const string DefEnd = "END";

		public InstructionDefsReader(GenTypes genTypes, string filename) {
			sb = new StringBuilder();
			this.genTypes = genTypes;
			memSizeTbl = genTypes.GetObject<MemorySizeInfoTable>(TypeIds.MemorySizeInfoTable);
			this.filename = filename;
			lines = File.ReadAllLines(filename);
			errors = new List<string>();
			opAccess = new List<OpInfo>();
			opKinds = new List<OpCodeOperandKind>();
			fmtKeyValues = new List<(string key, string value)>();
			// Ignore case because two Code values with different casing is just too confusing
			usedCodeValues = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

			toCode = CreateEnumDict(genTypes[TypeIds.Code]);
			toMnemonic = CreateEnumDict(genTypes[TypeIds.Mnemonic]);
			toEncoding = CreateEnumDict(genTypes[TypeIds.EncodingKind]);
			toCpuid = CreateEnumDict(genTypes[TypeIds.CpuidFeature]);
			toTupleType = CreateEnumDict(genTypes[TypeIds.TupleType]);
			toConditionCode = CreateEnumDict(genTypes[TypeIds.ConditionCode]);
			toFlowControl = CreateEnumDict(genTypes[TypeIds.FlowControl]);
			toCodeInfo = CreateEnumDict(genTypes[TypeIds.CodeInfo]);
			toPseudoOpsKind = CreateEnumDict(genTypes[TypeIds.PseudoOpsKind]);
			toDecOptionValue = CreateEnumDict(genTypes[TypeIds.DecOptionValue]);
			toMemorySize = CreateEnumDict(genTypes[TypeIds.MemorySize]);
			toOpCodeOperandKind = CreateEnumDict(genTypes[TypeIds.OpCodeOperandKind]);
			toRegisterIgnoreCase = CreateEnumDict(genTypes[TypeIds.Register], ignoreCase: true);

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

			tupleTypeN1 = toTupleType[nameof(TupleType.N1)];
			memorySizeUnknown = toMemorySize[nameof(MemorySize.Unknown)];
			flowControlNext = toFlowControl[nameof(FlowControl.Next)];
			decoderOptionNone = toDecOptionValue[nameof(DecOptionValue.None)];
			codeInfoNone = toCodeInfo[nameof(CodeInfo.None)];
		}

		static Dictionary<string, EnumValue> CreateEnumDict(EnumType enumType, bool ignoreCase = false) =>
			enumType.Values.ToDictionary(a => a.RawName, a => a, ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);

		void Error(int lineIndex, string message) => errors.Add($"Line {lineIndex + 1}: {message}");

		public InstructionDef[] Read() {
			var defs = new List<(InstructionDef def, int lineIndex)>();

			var lines = this.lines;
			for (int lineIndex = 0; lineIndex < lines.Length;) {
				var line = lines[lineIndex].Trim();
				if (line.Length == 0 || line[0] == '#') {
					lineIndex++;
					continue;
				}

				int errorCount = errors.Count;
				int origLineIndex = lineIndex;
				if (!TryParse(ref lineIndex, out var def, out var defLineIndex)) {
					Debug.Assert(errorCount < errors.Count);
					lineIndex = SkipLines(Math.Max(origLineIndex + 1, lineIndex));
				}
				else
					defs.Add((def, defLineIndex));
			}
			if (defs.Count == 0)
				Error(lines.Length, "No instruction definitions found");

			if (TryGetErrorString(out var errorMessage))
				throw new InvalidOperationException(errorMessage);

			UpdateCodeNameCommentsInFile(lines, defs);

			return defs.Select(a => a.def).ToArray();
		}

		void UpdateCodeNameCommentsInFile(string[] lines, List<(InstructionDef def, int lineIndex)> infos) {
			var newLines = lines.ToList();
			foreach (var (def, lineIndex) in infos.OrderByDescending(a => a.lineIndex)) {
				const string CodeCommentPrefix = "# Code: ";
				var newCommentLine = CodeCommentPrefix + def.Code.RawName;
				if (lineIndex > 0 && lines[lineIndex - 1].StartsWith(CodeCommentPrefix, StringComparison.Ordinal))
					newLines[lineIndex - 1] = newCommentLine;
				else
					newLines.Insert(lineIndex, newCommentLine);
			}
			if (!IsSame(newLines, lines))
				File.WriteAllLines(filename, newLines.ToArray(), new UTF8Encoding(false, true));

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

		bool TryParse(ref int lineIndex, [NotNullWhen(true)] out InstructionDef? def, out int defLineIndex) {
			def = null;
			defLineIndex = -1;

			var line = lines[lineIndex].Trim();
			if (!line.StartsWith(DefBeginPrefix, StringComparison.Ordinal)) {
				Error(lineIndex, $"Expected an instruction definition: `{DefBeginPrefix}`");
				return false;
			}

			line = line.Substring(DefBeginPrefix.Length).Trim();
			if (!TryParseDefLine(line, out var opCodeStr, out var instrStr, out var cpuid,
				out var tupleType, out var error)) {
				Error(lineIndex, error);
				return false;
			}

			var opCodeParser = new OpCodeStringParser(opCodeStr);
			if (!opCodeParser.TryParse(out var parsedOpCode, out error)) {
				Error(lineIndex, error);
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
			var state = new InstructionDefState(lineIndex, opCodeStr, instrStr, cpuid,
				tupleType ?? tupleTypeN1, toEncoding[parsedOpCode.Encoding.ToString()]);
			lineIndex++;
			state.OpCode = parsedOpCode;

			if ((parsedOpCode.Flags & ParsedOpCodeFlags.Fwait) != 0)
				state.Flags1 |= InstructionDefFlags1.Fwait;
			if ((parsedOpCode.Flags & ParsedOpCodeFlags.ModRegRmString) != 0)
				state.InstrStrFlags |= InstructionStringFlags.ModRegRmString;

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
			state.Flags2 |= InstructionDefFlags2.UseOutsideEnclaveSgx | InstructionDefFlags2.UseInEnclaveSgx1 | InstructionDefFlags2.UseInEnclaveSgx2;
			fmtKeyValues.Clear();
			bool? privileged = null;
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
					if (state.MnemonicStr is object) {
						Error(lineIndex, $"Duplicate {lineKey}");
						return false;
					}
					state.MnemonicStr = lineValue;
					break;

				case "code-mnemonic":
					if (state.CodeMnemonic is object) {
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
					default:
						Error(lineIndex, $"Unknown value `{lineValue}`");
						return false;
					}
					break;

				case "code-suffix":
					if (state.CodeSuffix is object) {
						Error(lineIndex, $"Duplicate {lineKey}");
						return false;
					}
					state.CodeSuffix = lineValue;
					break;

				case "code-memory-size":
					if (state.CodeMemorySize is object) {
						Error(lineIndex, $"Duplicate {lineKey}");
						return false;
					}
					state.CodeMemorySize = lineValue;
					break;

				case "code-memory-size-suffix":
					if (state.CodeMemorySizeSuffix is object) {
						Error(lineIndex, $"Duplicate {lineKey}");
						return false;
					}
					state.CodeMemorySizeSuffix = lineValue;
					break;

				case "cflow":
					if (state.Cflow is object) {
						Error(lineIndex, $"Duplicate {lineKey}");
						return false;
					}
					if (!TryGetValue(toFlowControl, lineValue, out state.Cflow, out error)) {
						Error(lineIndex, error);
						return false;
					}
					break;

				case "iinfo":
					if (state.CodeInfo is object) {
						Error(lineIndex, $"Duplicate {lineKey}");
						return false;
					}
					if (!TryGetValue(toCodeInfo, lineValue, out state.CodeInfo, out error)) {
						Error(lineIndex, error);
						return false;
					}
					break;

				case "decoder-option":
					if (state.DecoderOption is object) {
						Error(lineIndex, $"Duplicate {lineKey}");
						return false;
					}
					if (!TryGetValue(toDecOptionValue, lineValue, out state.DecoderOption, out _)) {
						Error(lineIndex, $"Add missing decoder option value to {nameof(DecOptionValue)}: {lineValue}");
						return false;
					}
					break;

				case "pseudo":
					if (state.PseudoOpsKind is object) {
						Error(lineIndex, $"Duplicate {lineKey}");
						return false;
					}
					if (!TryGetValue(toPseudoOpsKind, lineValue, out state.PseudoOpsKind, out error)) {
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
						switch (value) {
						case "16": state.Flags1 |= InstructionDefFlags1.Bit16; break;
						case "32": state.Flags1 |= InstructionDefFlags1.Bit32; break;
						case "64": state.Flags1 |= InstructionDefFlags1.Bit64; break;
						case "cpl0": state.Flags1 |= InstructionDefFlags1.Cpl0; break;
						case "cpl1": state.Flags1 |= InstructionDefFlags1.Cpl1; break;
						case "cpl2": state.Flags1 |= InstructionDefFlags1.Cpl2; break;
						case "cpl3": state.Flags1 |= InstructionDefFlags1.Cpl3; break;
						case "save-restore": state.Flags1 |= InstructionDefFlags1.SaveRestore; break;
						case "stack": state.Flags1 |= InstructionDefFlags1.StackInstruction; break;
						case "ignore-seg": state.Flags1 |= InstructionDefFlags1.IgnoresSegment; break;
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

						case "vmx=op":
						case "vmx=root":
						case "vmx=non-root":
							if (state.VmxMode != VmxMode.None) {
								error = "Duplicate vmx value";
								return false;
							}
							state.VmxMode = value switch {
								"vmx=op" => VmxMode.VmxOp,
								"vmx=root" => VmxMode.VmxRootOp,
								"vmx=non-root" => VmxMode.VmxNonRootOp,
								_ => throw new InvalidOperationException(),
							};
							break;

						case "privileged":
						case "no-privileged":
							if (privileged is object) {
								error = "Duplicate privileged/no-privileged value";
								return false;
							}
							privileged = value == "privileged";
							break;

						default:
							Error(lineIndex, $"Unknown flags value `{value}`");
							return false;
						}
					}
					break;

				case "branch":
					if (state.BranchKind != BranchKind.None) {
						Error(lineIndex, $"Duplicate {lineKey}");
						return false;
					}
					switch (lineValue) {
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
					default:
						Error(lineIndex, $"Unknown branch kind `{lineValue}`");
						return false;
					}
					break;

				case "cc":
					if (state.ConditionCode != ConditionCode.None) {
						Error(lineIndex, $"Duplicate {lineKey}");
						return false;
					}
					switch (lineValue) {
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
						Error(lineIndex, $"Unknown condition code `{lineValue}`");
						return false;
					}
					break;

				case "ops":
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
					foreach (var (key, value) in GetKeyValues(opsParts[0].Trim())) {
						if (value == string.Empty) {
							Error(lineIndex, "Missing value");
							return false;
						}
						OpInfo opAccess;
						switch (key) {
						case "n": opAccess = OpInfo.None; break;
						case "cr": opAccess = OpInfo.CondRead; break;
						case "cw": opAccess = OpInfo.CondWrite; break;
						case "cw32_rw64": opAccess = OpInfo.CondWrite32_ReadWrite64; break;
						case "nma": opAccess = OpInfo.NoMemAccess; break;
						case "r": opAccess = OpInfo.Read; break;
						case "rcw": opAccess = OpInfo.ReadCondWrite; break;
						case "rp3": opAccess = OpInfo.ReadP3; break;
						case "rw": opAccess = OpInfo.ReadWrite; break;
						case "w": opAccess = OpInfo.Write; break;
						case "wvmm": opAccess = OpInfo.WriteVmm; break;
						case "rwvmm": opAccess = OpInfo.ReadWriteVmm; break;
						case "wf": opAccess = OpInfo.WriteForce; break;
						case "wm_rwreg": opAccess = OpInfo.WriteMem_ReadWriteReg; break;
						default:
							Error(lineIndex, $"Unknown op access `{key}`");
							return false;
						}
						if (!TryGetValue(toOpCodeOperandKind, value, out var opKind, out error)) {
							Error(lineIndex, error);
							return false;
						}
						this.opAccess.Add(opAccess);
						opKinds.Add((OpCodeOperandKind)opKind.Value);
					}
					if (opAccess.Count == 0) {
						Error(lineIndex, "Missing op access and kind");
						return false;
					}
					state.OpAccess = opAccess.ToArray();
					state.OpKinds = opKinds.ToArray();
					if (opsParts.Length > 1) {
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
					}
					break;

				case "fast":
				case "gas":
				case "intel":
				case "masm":
				case "nasm":
					fmtKeyValues.Add((lineKey, lineValue));
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

			var isPriv = privileged ??
				(state.Flags1 & (InstructionDefFlags1.Cpl0 | InstructionDefFlags1.Cpl1 | InstructionDefFlags1.Cpl2 | InstructionDefFlags1.Cpl3)) == InstructionDefFlags1.Cpl0;
			if (isPriv)
				state.Flags3 |= InstructionDefFlags3.Privileged;

			// The formatters depend on some other lines so parse the formatter lines later
			foreach (var (key, value) in fmtKeyValues) {
				switch (key) {
				case "fast":
					if (state.FastInfo is object) {
						Error(lineIndex, $"Duplicate {key}");
						return false;
					}
					if (!TryReadFastFmt(value, out state.FastInfo, out error)) {
						Error(lineIndex, error);
						return false;
					}
					break;

				case "gas":
					if (state.Gas is object) {
						Error(lineIndex, $"Duplicate {key}");
						return false;
					}
					if (!TryReadGasFmt(value, state, out state.Gas, out error)) {
						Error(lineIndex, error);
						return false;
					}
					break;

				case "intel":
					if (state.Intel is object) {
						Error(lineIndex, $"Duplicate {key}");
						return false;
					}
					if (!TryReadIntelFmt(value, state, out state.Intel, out error)) {
						Error(lineIndex, error);
						return false;
					}
					break;

				case "masm":
					if (state.Masm is object) {
						Error(lineIndex, $"Duplicate {key}");
						return false;
					}
					if (!TryReadMasmFmt(value, state, out state.Masm, out error)) {
						Error(lineIndex, error);
						return false;
					}
					break;

				case "nasm":
					if (state.Nasm is object) {
						Error(lineIndex, $"Duplicate {key}");
						return false;
					}
					if (!TryReadNasmFmt(value, state, out state.Nasm, out error)) {
						Error(lineIndex, error);
						return false;
					}
					break;

				default:
					Error(lineIndex, $"Unknown key `{key}`");
					return false;
				}
			}

			if (state.InstrStrFmtOption == InstrStrFmtOption.None) {
				int mm1Index = instrStr.IndexOf("mm1", StringComparison.Ordinal);
				int mm2Index = instrStr.IndexOf("mm2", StringComparison.Ordinal);
				if (instrStr.Contains("k2 {k1}", StringComparison.Ordinal))
					state.InstrStrFmtOption = InstrStrFmtOption.OpMaskIsK1_or_NoGprSuffix;
				else if (mm2Index >= 0 && mm1Index < 0 &&
					!(state.OpKinds.Length > 2 &&
					(state.OpKinds[0] == OpCodeOperandKind.k_reg ||
					state.OpKinds[0] == OpCodeOperandKind.kp1_reg))) {
					state.InstrStrFmtOption = InstrStrFmtOption.IncVecIndex;
				}
				else if ((instrStr.EndsWith("mm", StringComparison.Ordinal) || instrStr.Contains("mm,", StringComparison.Ordinal)) &&
					!instrStr.Contains("mm1", StringComparison.Ordinal) && !instrStr.Contains("mm2", StringComparison.Ordinal)) {
					state.InstrStrFmtOption = InstrStrFmtOption.NoVecIndex;
				}
				else if (mm1Index >= 0 && mm2Index >= 0 && mm2Index < mm1Index)
					state.InstrStrFmtOption = InstrStrFmtOption.SwapVecIndex12;
				else if (!instrStr.Contains(',') &&
					state.OpKinds.Length == 2 &&
					state.OpKinds[0] == OpCodeOperandKind.st0 &&
					state.OpKinds[1] == OpCodeOperandKind.sti_opcode) {
					state.InstrStrFmtOption = InstrStrFmtOption.SkipOp0;
				}
				else if (instrStr.Contains("r8, r8,", StringComparison.Ordinal) || instrStr.Contains("r16, r16,", StringComparison.Ordinal) ||
					instrStr.Contains("r32, r32,", StringComparison.Ordinal) || instrStr.Contains("r64, r64,", StringComparison.Ordinal) ||
					instrStr.EndsWith("r8, r8", StringComparison.Ordinal) || instrStr.EndsWith("r16, r16", StringComparison.Ordinal) ||
					instrStr.EndsWith("r32, r32", StringComparison.Ordinal) || instrStr.EndsWith("r64, r64", StringComparison.Ordinal)) {
					state.InstrStrFmtOption = InstrStrFmtOption.OpMaskIsK1_or_NoGprSuffix;
				}
			}

			state.Cflow ??= flowControlNext;
			state.DecoderOption ??= decoderOptionNone;
			state.CodeInfo ??= codeInfoNone;
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
			if ((state.Flags1 & CpuModeBits) == 0)
				state.Flags1 |= CpuModeBits;
			const InstructionDefFlags1 CplBits = InstructionDefFlags1.Cpl0 | InstructionDefFlags1.Cpl1 |
												InstructionDefFlags1.Cpl2 | InstructionDefFlags1.Cpl3;
			if ((state.Flags1 & CplBits) == 0)
				state.Flags1 |= CplBits;
			if (state.MemorySize_Broadcast != memorySizeUnknown)
				state.Flags1 |= InstructionDefFlags1.Broadcast;
			if (instrStr.Contains("{er}", StringComparison.Ordinal))
				state.Flags1 |= InstructionDefFlags1.RoundingControl;
			if (instrStr.Contains("{sae}", StringComparison.Ordinal))
				state.Flags1 |= InstructionDefFlags1.SuppressAllExceptions;
			if (instrStr.Contains("{k1}", StringComparison.Ordinal) || instrStr.Contains("{k2}", StringComparison.Ordinal))
				state.Flags1 |= InstructionDefFlags1.OpMaskRegister;
			if (instrStr.Contains("{z}", StringComparison.Ordinal))
				state.Flags1 |= InstructionDefFlags1.ZeroingMasking;
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
				s.Substring(0, 1).ToUpperInvariant() + s.Substring(1).ToLowerInvariant();

			if (state.MnemonicStr is null) {
				int index = instrStr.IndexOf(' ');
				if (index < 0)
					index = instrStr.Length;
				state.MnemonicStr = instrStr.Substring(0, index).ToLowerInvariant();
			}
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
			// v86 mode and SGX enclaves use CPL=3, so disable all CPL<3 instructions
			if ((state.Flags1 & InstructionDefFlags1.Cpl3) == 0)
				state.Flags2 &= ~(InstructionDefFlags2.Virtual8086Mode | InstructionDefFlags2.UseInEnclaveSgx1 | InstructionDefFlags2.UseInEnclaveSgx2);

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

			var codeFormatter = new CodeFormatter(sb, memSizeTbl, state.CodeMnemonic, state.CodeSuffix, state.CodeMemorySize,
				state.CodeMemorySizeSuffix, state.MemorySize, state.MemorySize_Broadcast, state.Flags1, parsedOpCode.Encoding, state.OpKinds);
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

			var fmtMnemonic = state.MnemonicStr.ToLowerInvariant();
			var pseudoOp = state.PseudoOpsKind is null ? (PseudoOpsKind?)null : (PseudoOpsKind)state.PseudoOpsKind.Value;
			if (!TryCreateFastDef(state.Code, fmtMnemonic, pseudoOp, state.FastInfo ?? new FastState(), out var fastDef, out error)) {
				Error(state.LineIndex, "(fast) " + error);
				return false;
			}
			if (!TryCreateGasDef(state, state.Code, fmtMnemonic, pseudoOp, state.Gas ?? new GasState(), out var gasDef, out error)) {
				Error(state.LineIndex, "(gas) " + error);
				return false;
			}
			if (!TryCreateIntelDef(state, state.Code, fmtMnemonic, pseudoOp, state.Intel ?? new IntelState(), out var intelDef, out error)) {
				Error(state.LineIndex, "(intel) " + error);
				return false;
			}
			if (!TryCreateMasmDef(state, state.Code, fmtMnemonic, pseudoOp, state.Masm ?? new MasmState(), out var masmDef, out error)) {
				Error(state.LineIndex, "(masm) " + error);
				return false;
			}
			if (!TryCreateNasmDef(state, state.Code, fmtMnemonic, pseudoOp, state.Nasm ?? new NasmState(), out var nasmDef, out error)) {
				Error(state.LineIndex, "(nasm) " + error);
				return false;
			}

			if (state.OpAccess.Length != state.OpKinds.Length)
				throw new InvalidOperationException();
			if (state.OpKinds.Any(a => a == OpCodeOperandKind.None)) {
				Error(state.LineIndex, $"An operand with a `{nameof(OpCodeOperandKind.None)}` value");
				return false;
			}
			def = new InstructionDef(state.Code, state.OpCodeStr, state.InstrStr, state.Mnemonic, state.MemorySize,
				state.MemorySize_Broadcast, state.DecoderOption, state.Flags1, state.Flags2, state.Flags3, state.InstrStrFmtOption,
				state.InstrStrFlags, state.OpCode.MandatoryPrefix, state.OpCode.Table, state.OpCode.LBit, state.OpCode.WBit, state.OpCode.OpCode,
				state.OpCode.OpCodeLength, state.OpCode.GroupIndex, state.OpCode.RmGroupIndex,
				state.OpCode.OperandSize, state.OpCode.AddressSize, (TupleType)state.TupleType.Value, state.OpKinds,
				pseudoOp, (CodeInfo)state.CodeInfo.Value, state.Encoding, state.Cflow, state.ConditionCode, state.BranchKind, state.RflagsRead,
				state.RflagsUndefined, state.RflagsWritten, state.RflagsCleared, state.RflagsSet, state.Cpuid, state.OpAccess,
				fastDef, gasDef, intelDef, masmDef, nasmDef);
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
					int eqIndex = part.IndexOf('=');
					if (eqIndex < 0)
						break;
					var key = part.Substring(0, eqIndex).Trim();
					var value = part.Substring(eqIndex + 1).Trim();
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

		// The order of strings must be the same as the CC_xxx enums, eg. CC_b, etc, see CC.cs.
		// The index into the return array is the low 4 bits of the opcode.
		static string[][] CreateOtherCCMnemonics(string prefix) =>
			new string[][] {
				Array.Empty<string>(),
				Array.Empty<string>(),
				new[] { prefix + "c", prefix + "nae" },
				new[] { prefix + "nb", prefix + "nc" },
				new[] { prefix + "z" },
				new[] { prefix + "nz" },
				new[] { prefix + "na" },
				new[] { prefix + "nbe" },
				Array.Empty<string>(),
				Array.Empty<string>(),
				new[] { prefix + "pe" },
				new[] { prefix + "po" },
				new[] { prefix + "nge" },
				new[] { prefix + "nl" },
				new[] { prefix + "ng" },
				new[] { prefix + "nle" },
			};
		static readonly string[][] jccOtherMnemonics = CreateOtherCCMnemonics("j");
		static readonly string[][] cmovccOtherMnemonics = CreateOtherCCMnemonics("cmov");
		static readonly string[][] setccOtherMnemonics = CreateOtherCCMnemonics("set");

		static bool TryGetCcMnemonics(InstructionDefState def, out int ccIndex, [NotNullWhen(true)] out string[]? extraMnemonics, [NotNullWhen(false)] out string? error) {
			ccIndex = (int)(def.OpCode.OpCode & 0x0F);
			if (def.InstrStr.StartsWith("CMOV", StringComparison.OrdinalIgnoreCase))
				extraMnemonics = cmovccOtherMnemonics[ccIndex];
			else if (def.InstrStr.StartsWith("SET", StringComparison.OrdinalIgnoreCase))
				extraMnemonics = setccOtherMnemonics[ccIndex];
			else {
				extraMnemonics = null;
				error = "Unsupported cc mnemonic";
				return false;
			}
			error = null;
			return true;
		}

		bool TryGetLoopCcMnemonics(InstructionDefState def, out int ccIndex, [NotNullWhen(true)] out string? extraLoopMnemonic) {
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
				switch (def.OpKinds[^1]) {
				case OpCodeOperandKind.imm16: value = signExtendInfoType[nameof(Enums.Formatter.Nasm.SignExtendInfo.Sex2)]; break;
				case OpCodeOperandKind.imm32: value = signExtendInfoType[nameof(Enums.Formatter.Nasm.SignExtendInfo.Sex4)]; break;
				case OpCodeOperandKind.imm8sex16: value = signExtendInfoType[nameof(Enums.Formatter.Nasm.SignExtendInfo.Sex1to2)]; break;
				case OpCodeOperandKind.imm8sex32: value = signExtendInfoType[nameof(Enums.Formatter.Nasm.SignExtendInfo.Sex1to4)]; break;
				case OpCodeOperandKind.imm8sex64: value = signExtendInfoType[nameof(Enums.Formatter.Nasm.SignExtendInfo.Sex1to8)]; break;
				case OpCodeOperandKind.imm32sex64: value = signExtendInfoType[nameof(Enums.Formatter.Nasm.SignExtendInfo.Sex4to8)]; break;
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

		static bool TryGetMoffsIndex(InstructionDefState def, out int addrIndex, [NotNullWhen(false)] out string? error) {
			if (def.OpKinds.Length > 0 && def.OpKinds[0] == OpCodeOperandKind.mem_offs)
				addrIndex = 0;
			else if (def.OpKinds.Length > 1 && def.OpKinds[1] == OpCodeOperandKind.mem_offs)
				addrIndex = 1;
			else {
				addrIndex = -1;
				error = "1st or 2nd operand must be `mem_offs`";
				return false;
			}
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
					if (result.Mnemonic is object) {
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

			state = result;
			error = null;
			return true;
		}

		bool TryCreateFastDef(EnumValue code, string defaultMnemonic, PseudoOpsKind? pseudoOp, FastState state, [NotNullWhen(true)] out FastFmtInstructionDef? fastDef, [NotNullWhen(false)] out string? error) {
			var mnemonic = state.Mnemonic ?? defaultMnemonic;
			if (pseudoOp is object)
				state.Flags.Add(fastFmtFlags[pseudoOp.GetValueOrDefault().ToString()]);
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
					if (result.Mnemonic is object) {
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
					if (result.Suffix is object) {
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
					if (!TryGetMoffsIndex(def, out var addrIndex, out error))
						return false;
					result.Args.Add(result.GetUsedSuffix());
					result.Args.Add(addrIndex);
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
						if (!uint.TryParse(arg, out uint value)) {
							error = $"Expected an integer: `{arg}`";
							return false;
						}
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

				if (def.PseudoOpsKind is object) {
					state.Args.Add(def.PseudoOpsKind);
					if (pseudoOp == PseudoOpsKind.pclmulqdq || pseudoOp == PseudoOpsKind.vpclmulqdq)
						ctorKind = gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.pclmulqdq)];
					else {
						ctorKind = gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.pops)];
						state.Args.Add((def.Flags1 & InstructionDefFlags1.SuppressAllExceptions) != 0);
					}
				}
				else if ((def.Flags1 & InstructionDefFlags1.SuppressAllExceptions) != 0) {
					if (def.OpKinds.Length == 0) {
						error = "Instruction has no operands";
						return false;
					}
					ctorKind = gasCtorKind[nameof(Enums.Formatter.Gas.CtorKind.sae)];
					var saeIndex = def.OpKinds[^1] == OpCodeOperandKind.imm8 ? 1 : 0;
					state.Args.Add(saeIndex);
				}
				else if ((def.Flags1 & InstructionDefFlags1.RoundingControl) != 0) {
					if (def.OpKinds.Length == 0) {
						error = "Instruction has no operands";
						return false;
					}
					int erIndex;
					switch (def.OpKinds[^1]) {
					case OpCodeOperandKind.r32_or_mem:
					case OpCodeOperandKind.r64_or_mem:
						erIndex = 1;
						break;
					default:
						erIndex = 0;
						break;
					}
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
					else if (state.Suffix is object) {
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
					if (result.Mnemonic is object) {
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
					if (!TryGetMoffsIndex(def, out var addrIndex, out error))
						return false;
					result.Args.Add(addrIndex);
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

				if (def.PseudoOpsKind is object) {
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
					if (result.Mnemonic is object) {
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

				if (def.PseudoOpsKind is object) {
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
					if (result.Mnemonic is object) {
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
					if (!TryGetMoffsIndex(def, out var addrIndex, out error))
						return false;
					result.Args.Add(addrIndex);
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

				if (def.PseudoOpsKind is object) {
					state.Args.Add(def.PseudoOpsKind);
					if (pseudoOp == PseudoOpsKind.pclmulqdq || pseudoOp == PseudoOpsKind.vpclmulqdq)
						ctorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.pclmulqdq)];
					else
						ctorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.pops)];
				}
				else if ((def.Flags1 & InstructionDefFlags1.SuppressAllExceptions) != 0) {
					if (def.OpKinds.Length == 0) {
						error = "Instruction has no operands";
						return false;
					}
					ctorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.sae)];
					var saeIndex = def.OpKinds[^1] == OpCodeOperandKind.imm8 ? def.OpKinds.Length - 1 : def.OpKinds.Length;
					state.Args.Add(saeIndex);
				}
				else if ((def.Flags1 & InstructionDefFlags1.RoundingControl) != 0) {
					if (def.OpKinds.Length == 0) {
						error = "Instruction has no operands";
						return false;
					}
					int erIndex;
					switch (def.OpKinds[^1]) {
					case OpCodeOperandKind.r32_or_mem:
					case OpCodeOperandKind.r64_or_mem:
						erIndex = def.OpKinds.Length - 1;
						break;
					default:
						erIndex = def.OpKinds.Length;
						break;
					}
					state.Args.Add(erIndex);
					if (state.Flags.Count != 0) {
						ctorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.er_3)];
						state.Args.Add(CreateFlagsEnum(nasmInstrOpInfoFlags, state.GetUsedFlags()));
					}
					else
						ctorKind = nasmCtorKind[nameof(Enums.Formatter.Nasm.CtorKind.er_2)];
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
				case 'o': rflags |= RflagsBits.OF; break;
				case 's': rflags |= RflagsBits.SF; break;
				case 'z': rflags |= RflagsBits.ZF; break;
				case 'a': rflags |= RflagsBits.AF; break;
				case 'c': rflags |= RflagsBits.CF; break;
				case 'p': rflags |= RflagsBits.PF; break;
				case 'd': rflags |= RflagsBits.DF; break;
				case 'i': rflags |= RflagsBits.IF; break;
				case 'A': rflags |= RflagsBits.AC; break;
				default: error = $"Unknown rflags char `{c}`"; return false;
				}
			}

			error = null;
			return true;
		}

		static IEnumerable<(string key, string value)> GetKeyValues(string line) {
			foreach (var s in line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
				yield return GetKeyValue(s);
		}

		static (string key, string value) GetKeyValue(string s) {
			int index = s.IndexOf('=');
			if (index < 0)
				return (s, string.Empty);
			else {
				var key = s.Substring(0, index).Trim();
				var value = s.Substring(index + 1).Trim();
				return (key, value);
			}
		}

		static bool TryGetDefLineKeyValue(string line, [NotNullWhen(true)] out string? key, [NotNullWhen(true)] out string? value, [NotNullWhen(false)] out string? errMsg) {
			int index = line.IndexOf(':');
			if (index < 0) {
				errMsg = "Missing `:`";
				key = null;
				value = null;
				return false;
			}
			else {
				errMsg = null;
				key = line.Substring(0, index).Trim();
				value = line.Substring(index + 1).Trim();
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
				Debug.Assert(parts.Length == 4);
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
	}

	sealed class FastState {
		public string? Mnemonic;
		public List<EnumValue> Flags = new List<EnumValue>();
	}
	abstract class FmtState {
		public string? Mnemonic;
		public List<EnumValue> Flags = new List<EnumValue>();
		public EnumValue? CtorKind;
		public List<object> Args = new List<object>();

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
		public readonly EnumValue TupleType;
		public readonly EnumValue Encoding;
		public OpInfo[] OpAccess;
		public OpCodeOperandKind[] OpKinds;
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
		public ConditionCode ConditionCode;
		public EnumValue? DecoderOption;
		public EnumValue? CodeInfo;
		public InstructionDefFlags1 Flags1;
		public InstructionDefFlags2 Flags2;
		public InstructionDefFlags3 Flags3;
		public VmxMode VmxMode;
		public RflagsBits RflagsRead;
		public RflagsBits RflagsUndefined;
		public RflagsBits RflagsWritten;
		public RflagsBits RflagsCleared;
		public RflagsBits RflagsSet;
		public InstrStrFmtOption InstrStrFmtOption;
		public InstructionStringFlags InstrStrFlags;
		public EnumValue? PseudoOpsKind;
		public EnumValue? MemorySize;
		public EnumValue? MemorySize_Broadcast;
		public OpCodeDef OpCode;
		public FastState? FastInfo;
		public GasState? Gas;
		public IntelState? Intel;
		public MasmState? Masm;
		public NasmState? Nasm;

		public InstructionDefState(int lineIndex, string opCodeStr, string instrStr, EnumValue[] cpuid, EnumValue tupleType, EnumValue encoding) {
			LineIndex = lineIndex;
			OpCodeStr = opCodeStr;
			InstrStr = instrStr;
			Cpuid = cpuid;
			TupleType = tupleType;
			OpAccess = Array.Empty<OpInfo>();
			OpKinds = Array.Empty<OpCodeOperandKind>();
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
