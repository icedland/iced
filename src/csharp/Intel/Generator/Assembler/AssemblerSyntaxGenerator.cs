// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Generator.Enums;
using Generator.Enums.Decoder;
using Generator.Enums.Formatter;
using Generator.Enums.InstructionInfo;
using Generator.Formatters;
using Generator.Tables;

namespace Generator.Assembler {
	abstract class AssemblerSyntaxGenerator {
		protected readonly GenTypes genTypes;
		protected readonly InstructionDef[] defs;
		protected readonly RegisterDef[] regDefs;
		readonly MemorySizeDef[] memDefs;
		readonly Dictionary<GroupKey, OpCodeInfoGroup> groups;
		readonly Dictionary<GroupKey, OpCodeInfoGroup> groupsWithPseudo;
		readonly HashSet<EnumValue> discardOpCodes;
		readonly Dictionary<EnumValue, string> mapOpCodeToNewName;
		readonly Dictionary<EnumValue, Code> toOrigCodeValue;
		readonly HashSet<EnumValue> ambiguousBcst;
		protected readonly EnumType decoderOptions;
		protected readonly EnumType testInstrFlags;
		readonly (RegisterKind kind, RegisterDef[] regs)[] regGroups;
		readonly RegisterClassInfo[] regClasses;
		readonly MemorySizeFuncInfo[] memSizeFnInfos;
		readonly Dictionary<MemorySizeFnKind, MemorySizeFuncInfo> toFnInfo;
		int stackDepth;

		protected static readonly Dictionary<int, HashSet<string>> ignoredTestsPerBitness = new() {
			// generates  System.InvalidOperationException : Operand 0: Expected: NearBranch16, actual: NearBranch32 : 0x1 jecxz 000031D0h
			{ 16, new HashSet<string> { "jecxz_lu64" } },
			// generates  System.InvalidOperationException : Operand 0: Expected: NearBranch32, actual: NearBranch16 : 0x1 jcxz 31D0h
			{ 32, new HashSet<string> { "jcxz_lu64" } },
		};

		protected Code GetOrigCodeValue(EnumValue value) {
			if (value.DeclaringType.TypeId != TypeIds.Code)
				throw new InvalidOperationException();
			return toOrigCodeValue[value];
		}

		sealed class AmbiguousBcstInstr : IEquatable<AmbiguousBcstInstr> {
			public readonly EnumValue Code;
			readonly EnumValue mnemonic;
			readonly uint bcstMemSize;
			readonly OpCodeOperandKindDef[] operands;
			public AmbiguousBcstInstr(InstructionDef def, uint bcstMemSize) {
				Code = def.Code;
				mnemonic = def.Mnemonic;
				this.bcstMemSize = bcstMemSize;
				operands = def.OpKindDefs.Where(a => !a.Memory).ToArray();
			}

			public override bool Equals(object? obj) => obj is AmbiguousBcstInstr other && Equals(other);
			public bool Equals(AmbiguousBcstInstr? other) {
				if (other is null)
					return false;
				if (mnemonic != other.mnemonic || bcstMemSize != other.bcstMemSize)
					return false;
				if (operands.Length != other.operands.Length)
					return false;
				for (int i = 0; i < operands.Length; i++) {
					if (operands[i] != other.operands[i])
						return false;
				}
				return true;
			}
			public override int GetHashCode() {
				int hc = HashCode.Combine(mnemonic, bcstMemSize);
				foreach (var op in operands)
					hc = HashCode.Combine(hc, op);
				return hc;
			}
		}

		protected AssemblerSyntaxGenerator(GenTypes genTypes) {
			this.genTypes = genTypes;
			defs = genTypes.GetObject<InstructionDefs>(TypeIds.InstructionDefs).Defs;
			regDefs = genTypes.GetObject<RegisterDefs>(TypeIds.RegisterDefs).Defs;
			regGroups = GetRegisterGroups(genTypes);
			memDefs = genTypes.GetObject<MemorySizeDefs>(TypeIds.MemorySizeDefs).Defs;
			groups = new Dictionary<GroupKey, OpCodeInfoGroup>();
			groupsWithPseudo = new Dictionary<GroupKey, OpCodeInfoGroup>();
			var origCode = genTypes.GetObject<EnumValue[]>(TypeIds.OrigCodeValues);
			toOrigCodeValue = new Dictionary<EnumValue, Code>(origCode.Length);
			for (int i = 0; i < origCode.Length; i++)
				toOrigCodeValue.Add(origCode[i], (Code)i);

			discardOpCodes = defs.Where(a => (a.Flags3 & InstructionDefFlags3.AsmIgnore) != 0).Select(a => a.Code).ToHashSet();
			mapOpCodeToNewName = defs.Where(a => a.AsmMnemonic is not null).ToDictionary(a => a.Code, a => a.AsmMnemonic!);

			var ambigDict = new Dictionary<AmbiguousBcstInstr, List<AmbiguousBcstInstr>>();
			foreach (var def in defs) {
				if ((def.Flags1 & InstructionDefFlags1.Broadcast) == 0)
					continue;
				var key = new AmbiguousBcstInstr(def, memDefs[(int)def.MemoryBroadcast.Value].Size);
				if (!ambigDict.TryGetValue(key, out var list))
					ambigDict.Add(key, list = new List<AmbiguousBcstInstr>());
				list.Add(key);
			}
			ambiguousBcst = ambigDict.Where(a => a.Value.Count >= 2).SelectMany(a => a.Value).Select(a => a.Code).ToHashSet();
			decoderOptions = genTypes[TypeIds.DecoderOptions];
			testInstrFlags = genTypes[TypeIds.TestInstrFlags];
			regClasses = GetRegisterClassInfos();
			memSizeFnInfos = GetMemorySizeFunctions();
			toFnInfo = memSizeFnInfos.ToDictionary(x => x.Kind, x => x);
		}

		protected sealed class RegisterClassInfo {
			public readonly RegisterKind Kind;
			public bool IsGPR16_32_64 => Kind == RegisterKind.GPR16 || Kind == RegisterKind.GPR32 || Kind == RegisterKind.GPR64;
			public bool IsGPR32_64 => Kind == RegisterKind.GPR32 || Kind == RegisterKind.GPR64;
			public bool IsVector => Kind == RegisterKind.XMM || Kind == RegisterKind.YMM || Kind == RegisterKind.ZMM;
			public bool HasSaeOrEr => IsGPR32_64 || IsVector;
			public bool NeedsState => IsVector || HasSaeOrEr || Kind == RegisterKind.K;
			public RegisterClassInfo(RegisterKind kind) => Kind = kind;
		}

		protected enum MemorySizeFnKind {
			Ptr,
			BytePtr,
			WordPtr,
			DwordPtr,
			QwordPtr,
			MmwordPtr,
			TbytePtr,
			TwordPtr,
			FwordPtr,
			OwordPtr,
			XmmwordPtr,
			YmmwordPtr,
			ZmmwordPtr,

			Bcst,
			WordBcst,
			DwordBcst,
			QwordBcst,
		}
		protected sealed class MemorySizeFuncInfo {
			public readonly MemorySizeFnKind Kind;
			public readonly MemoryOperandSize Size;
			public readonly bool IsBroadcast;
			// eg. { "word", "ptr" }
			public readonly string[] NameParts;
			// eg. "word ptr"
			public readonly string Name;

			public MemorySizeFuncInfo(MemorySizeFnKind kind) {
				Kind = kind;

				var (name, size) = kind switch {
					MemorySizeFnKind.Ptr => ("ptr", MemoryOperandSize.None),
					MemorySizeFnKind.BytePtr => ("byte ptr", MemoryOperandSize.Byte),
					MemorySizeFnKind.WordPtr => ("word ptr", MemoryOperandSize.Word),
					MemorySizeFnKind.DwordPtr => ("dword ptr", MemoryOperandSize.Dword),
					MemorySizeFnKind.QwordPtr => ("qword ptr", MemoryOperandSize.Qword),
					MemorySizeFnKind.MmwordPtr => ("mmword ptr", MemoryOperandSize.Qword),
					MemorySizeFnKind.TbytePtr => ("tbyte ptr", MemoryOperandSize.Tbyte),
					MemorySizeFnKind.TwordPtr => ("tword ptr", MemoryOperandSize.Tbyte),
					MemorySizeFnKind.FwordPtr => ("fword ptr", MemoryOperandSize.Fword),
					MemorySizeFnKind.OwordPtr => ("oword ptr", MemoryOperandSize.Xword),
					MemorySizeFnKind.XmmwordPtr => ("xmmword ptr", MemoryOperandSize.Xword),
					MemorySizeFnKind.YmmwordPtr => ("ymmword ptr", MemoryOperandSize.Yword),
					MemorySizeFnKind.ZmmwordPtr => ("zmmword ptr", MemoryOperandSize.Zword),
					MemorySizeFnKind.Bcst => ("bcst", MemoryOperandSize.None),
					MemorySizeFnKind.WordBcst => ("word bcst", MemoryOperandSize.Word),
					MemorySizeFnKind.DwordBcst => ("dword bcst", MemoryOperandSize.Dword),
					MemorySizeFnKind.QwordBcst => ("qword bcst", MemoryOperandSize.Qword),
					_ => throw new InvalidOperationException(),
				};

				Size = size;
				Name = name;
				NameParts = name.Split(' ');
				IsBroadcast = NameParts[^1] == "bcst";
			}

			public string GetMethodDocs(string verb, Func<string, string> toCodeStr) {
				var desc = Kind switch {
					MemorySizeFnKind.Ptr or MemorySizeFnKind.Bcst => "no",
					_ => AOrAn(Kind, toCodeStr(Name.ToUpperInvariant())),
				};
				var bcstDesc = IsBroadcast ? "broadcast " : string.Empty;
				return $"{verb} a {bcstDesc}memory operand with {desc} size hint";
			}

			static string AOrAn(MemorySizeFnKind kind, string s) =>
				kind switch {
					MemorySizeFnKind.MmwordPtr or MemorySizeFnKind.FwordPtr or MemorySizeFnKind.OwordPtr or
					MemorySizeFnKind.XmmwordPtr => $"an {s}",
					MemorySizeFnKind.Ptr or MemorySizeFnKind.BytePtr or MemorySizeFnKind.WordPtr or MemorySizeFnKind.DwordPtr or
					MemorySizeFnKind.QwordPtr or MemorySizeFnKind.TbytePtr or MemorySizeFnKind.TwordPtr or MemorySizeFnKind.YmmwordPtr or
					MemorySizeFnKind.ZmmwordPtr or MemorySizeFnKind.Bcst or MemorySizeFnKind.WordBcst or MemorySizeFnKind.DwordBcst or
					MemorySizeFnKind.QwordBcst => $"a {s}",
					_ => throw new InvalidOperationException(),
				};
		}

		protected RegisterDef GetRegisterDef(Register register) => regDefs[(int)register];

		protected abstract void GenerateRegisters((RegisterKind kind, RegisterDef[] regs)[] regGroups);
		protected abstract void GenerateRegisterClasses(RegisterClassInfo[] infos);
		protected abstract void GenerateMemorySizeFunctions(MemorySizeFuncInfo[] infos);
		protected abstract void Generate(Dictionary<GroupKey, OpCodeInfoGroup> map, OpCodeInfoGroup[] opCodes);

		public void Generate() {
			GenerateRegisters(regGroups);
			GenerateRegisterClasses(regClasses);
			GenerateMemorySizeFunctions(memSizeFnInfos);
			GenerateOpCodes();
		}

		static RegisterClassInfo[] GetRegisterClassInfos() {
			var infos = new RegisterClassInfo[] {
				new(RegisterKind.GPR8),
				new(RegisterKind.GPR16),
				new(RegisterKind.GPR32),
				new(RegisterKind.GPR64),
				new(RegisterKind.Segment),
				new(RegisterKind.CR),
				new(RegisterKind.DR),
				new(RegisterKind.TR),
				new(RegisterKind.ST),
				new(RegisterKind.MM),
				new(RegisterKind.XMM),
				new(RegisterKind.YMM),
				new(RegisterKind.ZMM),
				new(RegisterKind.TMM),
				new(RegisterKind.K),
				new(RegisterKind.BND),
			};

			var allKinds = Enum.GetValues<RegisterKind>();
			// Ignored: None, IP
			if (allKinds.Length - 2 != infos.Length)
				throw new InvalidOperationException("New register kinds must be initialized");

			return infos;
		}

		static (RegisterKind kind, RegisterDef[] regs)[] GetRegisterGroups(GenTypes genTypes) {
			static bool IsValidReg(RegisterKind kind) =>
				kind switch {
					RegisterKind.None or RegisterKind.IP => false,
					_ => true,
				};

			var defs = genTypes.GetObject<RegisterDefs>(TypeIds.RegisterDefs);
			var regGroups = defs.GetRegisterGroups(IsValidReg);
			// Ignore: None, IP
			if (regGroups.Length != genTypes[TypeIds.RegisterKind].Values.Length - 2)
				throw new InvalidOperationException();

			return regGroups.ToArray();
		}

		static MemorySizeFuncInfo[] GetMemorySizeFunctions() {
			var infos = Enum.GetValues<MemorySizeFnKind>().Select(a => new MemorySizeFuncInfo(a)).ToArray();
			Array.Sort(infos, (a, b) => a.Kind.CompareTo(b.Kind));
			return infos.ToArray();
		}

		void GenerateOpCodes() {
			foreach (var def in defs) {
				var code = def.Code;
				if (discardOpCodes.Contains(code))
					continue;

				var mnemonicName = def.Mnemonic.RawName;
				var name = mapOpCodeToNewName.TryGetValue(code, out var nameOpt) ? nameOpt : mnemonicName.ToLowerInvariant();

				var signature = new Signature();
				var regOnlySignature = new Signature();

				var pseudoOpsKind = def.PseudoOp;
				var opCodeArgFlags = OpCodeArgFlags.None;

				if (def.Encoding == EncodingKind.VEX) opCodeArgFlags |= OpCodeArgFlags.HasVex;
				if (def.Encoding == EncodingKind.EVEX) opCodeArgFlags |= OpCodeArgFlags.HasEvex;
				if (def.Encoding == EncodingKind.MVEX) throw new InvalidOperationException();

				if ((def.Flags1 & InstructionDefFlags1.ZeroingMasking) != 0) opCodeArgFlags |= OpCodeArgFlags.HasZeroingMask;
				if ((def.Flags1 & InstructionDefFlags1.OpMaskRegister) != 0) opCodeArgFlags |= OpCodeArgFlags.HasKMask;
				if ((def.Flags1 & InstructionDefFlags1.Broadcast) != 0) opCodeArgFlags |= OpCodeArgFlags.HasBroadcast;
				if ((def.Flags1 & InstructionDefFlags1.SuppressAllExceptions) != 0) opCodeArgFlags |= OpCodeArgFlags.SuppressAllExceptions;
				if ((def.Flags1 & InstructionDefFlags1.RoundingControl) != 0) opCodeArgFlags |= OpCodeArgFlags.RoundingControl;

				var argSizes = new List<int>();

				// For certain instruction, we need to discard them
				int numberLeadingArgToDiscard = 0;
				if (GetSpecialArgEncodingInstruction(def) is int argsToDiscard) {
					numberLeadingArgToDiscard = argsToDiscard;
					opCodeArgFlags |= OpCodeArgFlags.HasSpecialInstructionEncoding;
				}

				for (int i = numberLeadingArgToDiscard; i < def.OpKindDefs.Length; i++) {
					var opKindDef = def.OpKindDefs[i];
					ArgKind argKind;
					int argSize = 0;
					switch (opKindDef.OperandEncoding) {
					case OperandEncoding.NearBranch:
					case OperandEncoding.Xbegin:
					case OperandEncoding.AbsNearBranch:
						switch (opKindDef.BranchOffsetSize) {
						case 8:
							argKind = ArgKind.Label;
							opCodeArgFlags |= OpCodeArgFlags.HasShortBranch;
							opCodeArgFlags |= OpCodeArgFlags.HasLabel;
							break;
						case 16:
						case 32:
							argKind = ArgKind.Label;
							opCodeArgFlags |= OpCodeArgFlags.HasNearBranch;
							opCodeArgFlags |= OpCodeArgFlags.HasLabel;
							break;
						default:
							throw new InvalidOperationException();
						}
						break;

					case OperandEncoding.Immediate:
						if (opKindDef.M2Z) {
							opCodeArgFlags |= OpCodeArgFlags.HasImmediateByteLessThanBits;
							argKind = ArgKind.Immediate;
							argSize = 1;
							break;
						}
						else {
							argKind = ArgKind.Immediate;
							argSize = opKindDef.ImmediateSize / 8;
							switch ((opKindDef.ImmediateSize, opKindDef.ImmediateSignExtSize)) {
							case (8, 8):
								opCodeArgFlags |= OpCodeArgFlags.HasImmediateByte;
								break;
							case (8, 16):
							case (8, 32):
								opCodeArgFlags |= OpCodeArgFlags.HasImmediateByteSignExtended;
								break;
							case (8, 64):
								opCodeArgFlags |= OpCodeArgFlags.HasImmediateByteSignExtended | OpCodeArgFlags.UnsignedUIntNotSupported;
								break;
							case (32, 64):
								opCodeArgFlags |= OpCodeArgFlags.UnsignedUIntNotSupported;
								break;
							case (16, 16):
							case (32, 32):
							case (64, 64):
								break;
							default:
								throw new InvalidOperationException();
							}
						}
						break;

					case OperandEncoding.ImpliedConst:
						if (opKindDef.ImpliedConst != 1)
							throw new InvalidOperationException();
						opCodeArgFlags |= OpCodeArgFlags.HasImmediateByteEqual1;
						argSize = 1;
						argKind = ArgKind.Immediate;
						break;

					case OperandEncoding.ImpliedRegister:
					case OperandEncoding.RegImm:
					case OperandEncoding.RegOpCode:
					case OperandEncoding.RegModrmReg:
					case OperandEncoding.RegModrmRm:
					case OperandEncoding.RegVvvvv:
						argKind = GetArgKind(opKindDef, isRegMem: false);
						break;

					case OperandEncoding.RegMemModrmRm:
						argKind = GetArgKind(opKindDef, isRegMem: true);
						break;

					case OperandEncoding.SegRSI:
					case OperandEncoding.ESRDI:
					case OperandEncoding.MemModrmRm:
					case OperandEncoding.MemOffset:
						argKind = ArgKind.Memory;
						break;

					case OperandEncoding.None:
					case OperandEncoding.FarBranch:
					case OperandEncoding.SegRBX:
					case OperandEncoding.SegRDI:
					default:
						throw new InvalidOperationException();
					}

					if (argKind == ArgKind.Unknown)
						throw new InvalidOperationException();
					argSizes.Add(argSize);
					signature.AddArgKind(GetArgKindForSignature(argKind, true));
					regOnlySignature.AddArgKind(GetArgKindForSignature(argKind, false));
				}

				if (!ShouldDiscardDuplicatedOpCode(signature, def)) {
					// discard r16m16
					bool hasR64M16 = IsR64M16(def);
					if (!hasR64M16) {
						AddOpCodeToGroup(name, mnemonicName, signature, def, opCodeArgFlags, pseudoOpsKind, numberLeadingArgToDiscard,
							argSizes, false);
					}
				}

				if (signature != regOnlySignature) {
					opCodeArgFlags &= ~OpCodeArgFlags.HasBroadcast;
					AddOpCodeToGroup(name, mnemonicName, regOnlySignature, def, opCodeArgFlags | OpCodeArgFlags.HasRegisterMemoryMappedToRegister,
						pseudoOpsKind, numberLeadingArgToDiscard, argSizes, false);
				}
			}

			CreatePseudoInstructions();

			var orderedGroups = groups.OrderBy(x => x.Key).Select(x => x.Value).ToArray();
			var signatures = new HashSet<Signature>();
			var opcodes = new List<InstructionDef>();
			foreach (var group in orderedGroups) {
				if (group.HasRegisterMemoryMappedToRegister) {
					var inputDefs = group.Defs;
					opcodes.Clear();
					signatures.Clear();
					// First-pass to select only register versions
					FilterOpCodesRegister(group, inputDefs, opcodes, signatures, false);

					// Second-pass to populate with RM versions
					FilterOpCodesRegister(group, inputDefs, opcodes, signatures, true);

					inputDefs.Clear();
					inputDefs.AddRange(opcodes);
				}

				// Update the selector graph for this group of opcodes
				if (group.HasSpecialInstructionEncoding)
					group.RootOpCodeNode = new OpCodeNode(group.Defs[0]);
				else
					group.RootOpCodeNode = BuildSelectorGraph(group);
			}

			Generate(groups, orderedGroups);
		}

		ArgKind GetArgKind(OpCodeOperandKindDef def, bool isRegMem) {
			var (reg, rm) = regDefs[(int)def.Register].GetRegisterKind() switch {
				RegisterKind.GPR8 => (ArgKind.Register8, ArgKind.Register8Memory),
				RegisterKind.GPR16 => (ArgKind.Register16, ArgKind.Register16Memory),
				RegisterKind.GPR32 => (ArgKind.Register32, ArgKind.Register32Memory),
				RegisterKind.GPR64 => (ArgKind.Register64, ArgKind.Register64Memory),
				RegisterKind.IP => (ArgKind.Unknown, ArgKind.Unknown),
				RegisterKind.Segment => (ArgKind.RegisterSegment, ArgKind.Unknown),
				RegisterKind.ST => (ArgKind.RegisterSt, ArgKind.Unknown),
				RegisterKind.CR => (ArgKind.RegisterCr, ArgKind.Unknown),
				RegisterKind.DR => (ArgKind.RegisterDr, ArgKind.Unknown),
				RegisterKind.TR => (ArgKind.RegisterTr, ArgKind.Unknown),
				RegisterKind.BND => (ArgKind.RegisterBnd, ArgKind.RegisterBndMemory),
				RegisterKind.K => (ArgKind.RegisterK, ArgKind.RegisterKMemory),
				RegisterKind.MM => (ArgKind.RegisterMm, ArgKind.RegisterMmMemory),
				RegisterKind.XMM => (ArgKind.RegisterXmm, ArgKind.RegisterXmmMemory),
				RegisterKind.YMM => (ArgKind.RegisterYmm, ArgKind.RegisterYmmMemory),
				RegisterKind.ZMM => (ArgKind.RegisterZmm, ArgKind.RegisterZmmMemory),
				RegisterKind.TMM => (ArgKind.RegisterTmm, ArgKind.Unknown),
				_ => throw new InvalidOperationException(),
			};
			var argKind = isRegMem ? rm : reg;
			if (argKind == ArgKind.Unknown)
				throw new InvalidOperationException();
			return argKind;
		}

		static ArgKind GetArgKindForSignature(ArgKind kind, bool memory) =>
			kind switch {
				ArgKind.Register8Memory => memory ? ArgKind.Memory : ArgKind.Register8,
				ArgKind.Register16Memory => memory ? ArgKind.Memory : ArgKind.Register16,
				ArgKind.Register32Memory => memory ? ArgKind.Memory : ArgKind.Register32,
				ArgKind.Register64Memory => memory ? ArgKind.Memory : ArgKind.Register64,
				ArgKind.RegisterKMemory => memory ? ArgKind.Memory : ArgKind.RegisterK,
				ArgKind.RegisterBndMemory => memory ? ArgKind.Memory : ArgKind.RegisterBnd,
				ArgKind.RegisterMmMemory => memory ? ArgKind.Memory : ArgKind.RegisterMm,
				ArgKind.RegisterXmmMemory => memory ? ArgKind.Memory : ArgKind.RegisterXmm,
				ArgKind.RegisterYmmMemory => memory ? ArgKind.Memory : ArgKind.RegisterYmm,
				ArgKind.RegisterZmmMemory => memory ? ArgKind.Memory : ArgKind.RegisterZmm,
				_ => kind,
			};

		int? GetSpecialArgEncodingInstruction(InstructionDef def) {
			if ((def.Flags3 & InstructionDefFlags3.IsStringOp) != 0)
				return def.OpCount;
			return GetOrigCodeValue(def.Code) switch {
				Code.Maskmovq_rDI_mm_mm or Code.Maskmovdqu_rDI_xmm_xmm or Code.VEX_Vmaskmovdqu_rDI_xmm_xmm => 1,
				Code.Xbegin_rel16 or Code.Xbegin_rel32 => 0,
				_ => null,
			};
		}

		OpCodeNode BuildSelectorGraph(OpCodeInfoGroup group) {
			var opcodes = group.Defs;
			// Sort opcodes by decreasing size
			opcodes.Sort(group.OrderOpCodesPerOpKindPriority);
			return BuildSelectorGraph(group, group.Signature, group.Flags, opcodes);
		}

		OpCodeNode BuildSelectorGraph(OpCodeInfoGroup group, Signature signature, OpCodeArgFlags argFlags, List<InstructionDef> opcodes) {
			if (opcodes.Count == 0)
				return default;

			// In case of one opcode, we don't need to perform any disambiguation
			if (opcodes.Count == 1)
				return new OpCodeNode(opcodes[0]);

			if (stackDepth++ >= 16)
				throw new InvalidOperationException("Potential StackOverflow");
			try {
				OrderedSelectorList selectors;

				if ((argFlags & OpCodeArgFlags.HasImmediateByteEqual1) != 0) {
					// handle imm8 == 1 
					var defsWithImmediateByteEqual1 = new List<InstructionDef>();
					var defsOthers = new List<InstructionDef>();
					var indices = CollectByOperandKindPredicate(opcodes, IsImmediateByteEqual1, defsWithImmediateByteEqual1, defsOthers);
					if (indices.Count != 1)
						throw new InvalidOperationException();
					var newFlags = argFlags & ~OpCodeArgFlags.HasImmediateByteEqual1;
					return new OpCodeSelector(indices[0], OpCodeSelectorKind.ImmediateByteEqual1) {
						IfTrue = BuildSelectorGraph(group, signature, newFlags, defsWithImmediateByteEqual1),
						IfFalse = BuildSelectorGraph(group, signature, newFlags, defsOthers)
					};
				}
				else if (group.IsBranch) {
					var branchShort = new List<InstructionDef>();
					var branchNear = new List<InstructionDef>();
					CollectByOperandKindPredicate(opcodes, IsShortBranch, branchShort, branchNear);
					if (branchShort.Count > 0 && branchNear.Count > 0) {
						var newFlags = argFlags & ~(OpCodeArgFlags.HasShortBranch | OpCodeArgFlags.HasNearBranch);
						return new OpCodeSelector(OpCodeSelectorKind.ShortBranch) {
							IfTrue = BuildSelectorGraph(group, signature, newFlags, branchShort),
							IfFalse = BuildSelectorGraph(group, signature, newFlags, branchNear)
						};
					}
				}

				// Handle case of moffs
				if (group.Name == "mov") {
					var opCodesRAXMOffs = new List<InstructionDef>();
					var newOpCodes = new List<InstructionDef>();

					var memOffs64Selector = OpCodeSelectorKind.Invalid;
					var memOffsSelector = OpCodeSelectorKind.Invalid;

					int argIndex = -1;
					for (var i = 0; i < opcodes.Count; i++) {
						var def = opcodes[i];
						bool handled = false;
						if (def.OpKindDefs.Length == 2) {
							switch ((def.OpKindDefs[0].OperandEncoding, def.OpKindDefs[1].OperandEncoding)) {
							case (OperandEncoding.MemOffset, OperandEncoding.ImpliedRegister):
								switch (def.OpKindDefs[1].Register) {
								case Register.AL:
									memOffs64Selector = OpCodeSelectorKind.MemOffs64_AL;
									memOffsSelector = OpCodeSelectorKind.MemOffs_AL;
									argIndex = 0;
									handled = true;
									break;
								case Register.AX:
									memOffs64Selector = OpCodeSelectorKind.MemOffs64_AX;
									memOffsSelector = OpCodeSelectorKind.MemOffs_AX;
									argIndex = 0;
									handled = true;
									break;
								case Register.EAX:
									memOffs64Selector = OpCodeSelectorKind.MemOffs64_EAX;
									memOffsSelector = OpCodeSelectorKind.MemOffs_EAX;
									argIndex = 0;
									handled = true;
									break;
								case Register.RAX:
									memOffs64Selector = OpCodeSelectorKind.MemOffs64_RAX;
									memOffsSelector = OpCodeSelectorKind.MemOffs_RAX;
									argIndex = 0;
									handled = true;
									break;
								}
								break;
							case (OperandEncoding.ImpliedRegister, OperandEncoding.MemOffset):
								switch (def.OpKindDefs[0].Register) {
								case Register.AL:
									memOffs64Selector = OpCodeSelectorKind.MemOffs64_AL;
									memOffsSelector = OpCodeSelectorKind.MemOffs_AL;
									argIndex = 1;
									handled = true;
									break;
								case Register.AX:
									memOffs64Selector = OpCodeSelectorKind.MemOffs64_AX;
									memOffsSelector = OpCodeSelectorKind.MemOffs_AX;
									argIndex = 1;
									handled = true;
									break;
								case Register.EAX:
									memOffs64Selector = OpCodeSelectorKind.MemOffs64_EAX;
									memOffsSelector = OpCodeSelectorKind.MemOffs_EAX;
									argIndex = 1;
									handled = true;
									break;
								case Register.RAX:
									memOffs64Selector = OpCodeSelectorKind.MemOffs64_RAX;
									memOffsSelector = OpCodeSelectorKind.MemOffs_RAX;
									argIndex = 1;
									handled = true;
									break;
								}
								break;
							}
						}
						if (handled)
							opCodesRAXMOffs.Add(def);
						else
							newOpCodes.Add(def);
					}

					if (opCodesRAXMOffs.Count > 0) {
						if (opCodesRAXMOffs.Count != 1)
							throw new InvalidOperationException();
						return new OpCodeSelector(argIndex, memOffs64Selector) {
							IfTrue = BuildSelectorGraph(group, signature, argFlags, opCodesRAXMOffs),
							IfFalse = new OpCodeSelector(argIndex, memOffsSelector) {
								IfTrue = BuildSelectorGraph(group, signature, argFlags, opCodesRAXMOffs),
								IfFalse = BuildSelectorGraph(group, signature, argFlags, newOpCodes)
							}
						};
					}
				}

				// Handle disambiguation for auto-broadcast select
				if ((argFlags & OpCodeArgFlags.HasBroadcast) != 0) {
					int memoryIndex = GetBroadcastMemory(argFlags, opcodes, signature, out var broadcastSelectorKind, out var evexBroadcastDef);
					if (memoryIndex >= 0) {
						return new OpCodeSelector(memoryIndex, broadcastSelectorKind) {
							IfTrue = evexBroadcastDef ?? throw new InvalidOperationException(),
							IfFalse = BuildSelectorGraph(group, signature, argFlags & ~OpCodeArgFlags.HasBroadcast, opcodes)
						};
					}
				}

				// For the following instructions, we prefer bitness other memory operand
				switch (group.Name) {
				case "lgdt":
				case "lidt":
				case "sgdt":
				case "sidt":
				case "bndmov":
				case "jmpe":
					selectors = new OrderedSelectorList();
					break;
				default:
					selectors = BuildSelectorsByRegisterOrMemory(signature, argFlags, opcodes, true);

					if (selectors.Count < opcodes.Count) {
						var memSelectors = BuildSelectorsByRegisterOrMemory(signature, argFlags, opcodes, false);
						if (memSelectors.Count > selectors.Count)
							selectors = memSelectors;
					}
					break;
				}

				// If we have zero or one kind of selectors for all opcodes based on register and/or memory,
				// it means that disambiguation is not done by register or memory but by either:
				// - evex/vex
				// - bitness
				if (selectors.Count <= 1) {
					// Special case for push imm, select first by bitness and after by imm
					bool isPushImm = group.Name == "push" && signature.ArgCount == 1 && IsArgKindImmediate(signature.GetArgKind(0));
					if (isPushImm && opcodes.Count > 2) {
						// bitness
						selectors = BuildSelectorsPerBitness(group, argFlags, opcodes);
						return BuildSelectorGraphFromSelectors(group, signature, argFlags, selectors);
					}

					if ((argFlags & OpCodeArgFlags.HasImmediateByteSignExtended) != 0) {
						// handle imm >= sbyte.MinValue && imm <= byte.MaxValue 
						var defsWithImmediateByteSigned = new List<InstructionDef>();
						var defsOthers = new List<InstructionDef>();
						var indices = CollectByOperandKindPredicate(opcodes, IsImmediateByteSigned, defsWithImmediateByteSigned, defsOthers);

						int opSize;
						if (isPushImm)
							opSize = GetImmediateSizeInBits(opcodes[0]);
						else
							opSize = GetMemorySizeInBits(memDefs, defs, opcodes[0]);
						var selectorKind = opSize switch {
							64 => OpCodeSelectorKind.ImmediateByteSigned8To64,
							32 => OpCodeSelectorKind.ImmediateByteSigned8To32,
							16 => OpCodeSelectorKind.ImmediateByteSigned8To16,
							_ => throw new InvalidOperationException(),
						};

						if (indices.Count != 1)
							throw new InvalidOperationException();
						var newFlags = argFlags & ~OpCodeArgFlags.HasImmediateByteSignExtended;
						return new OpCodeSelector(indices[0], selectorKind) {
							IfTrue = BuildSelectorGraph(group, signature, newFlags, defsWithImmediateByteSigned),
							IfFalse = BuildSelectorGraph(group, signature, newFlags, defsOthers)
						};
					}

					if ((argFlags & (OpCodeArgFlags.HasVex | OpCodeArgFlags.HasEvex)) == (OpCodeArgFlags.HasVex | OpCodeArgFlags.HasEvex)) {
						var vex = opcodes.Where(x => x.Encoding == EncodingKind.VEX).ToList();
						var evex = opcodes.Where(x => x.Encoding == EncodingKind.EVEX).ToList();
						var mvex = opcodes.Where(x => x.Encoding == EncodingKind.MVEX).ToList();
						if (mvex.Count != 0)
							throw new InvalidOperationException();

						return new OpCodeSelector(OpCodeSelectorKind.Vex) {
							IfTrue = BuildSelectorGraph(group, signature, argFlags & ~(OpCodeArgFlags.HasVex | OpCodeArgFlags.HasEvex), vex),
							IfFalse = BuildSelectorGraph(group, signature, argFlags & ~(OpCodeArgFlags.HasVex | OpCodeArgFlags.HasEvex), evex),
						};
					}

					// bitness
					selectors = BuildSelectorsPerBitness(group, argFlags, opcodes);
				}

				return BuildSelectorGraphFromSelectors(group, signature, argFlags, selectors);
			}
			finally {
				stackDepth--;
			}
		}

		OpCodeNode BuildSelectorGraphFromSelectors(OpCodeInfoGroup group, Signature signature, OpCodeArgFlags argFlags, OrderedSelectorList selectors) {
			OpCodeSelector? previousSelector = null;
			OpCodeNode rootNode = default;
			for (int selectorIndex = 0; selectorIndex < selectors.Count; selectorIndex++) {
				var (kind, list) = selectors[selectorIndex];
				OpCodeNode node;
				OpCodeSelector? newSelector = null;

				switch (kind) {
				case OpCodeSelectorKind.Bitness32:
				case OpCodeSelectorKind.Bitness16:
				case OpCodeSelectorKind.Register8:
				case OpCodeSelectorKind.Register16:
				case OpCodeSelectorKind.Register32:
				case OpCodeSelectorKind.Register64:
				case OpCodeSelectorKind.RegisterST:
				case OpCodeSelectorKind.RegisterBND:
				case OpCodeSelectorKind.RegisterK:
				case OpCodeSelectorKind.RegisterSegment:
				case OpCodeSelectorKind.RegisterCR:
				case OpCodeSelectorKind.RegisterDR:
				case OpCodeSelectorKind.RegisterTR:
				case OpCodeSelectorKind.RegisterMM:
				case OpCodeSelectorKind.RegisterXMM:
				case OpCodeSelectorKind.RegisterYMM:
				case OpCodeSelectorKind.RegisterZMM:
				case OpCodeSelectorKind.RegisterTMM:
					if (selectorIndex + 1 == selectors.Count)
						node = BuildSelectorGraph(group, signature, argFlags, list);
					else
						goto default;
					break;
				default:
					newSelector = selectors.ArgIndex >= 0 ? new OpCodeSelector(selectors.ArgIndex, kind) : new OpCodeSelector(kind);
					node = new OpCodeNode(newSelector);
					newSelector.IfTrue = list.Count == 1 ? new OpCodeNode(list[0]) : BuildSelectorGraph(group, signature, argFlags, list);
					break;
				}

				if (rootNode.IsEmpty)
					rootNode = node;

				if (previousSelector is not null)
					previousSelector.IfFalse = node;
				else if (selectorIndex != 0)
					throw new InvalidOperationException();

				previousSelector = newSelector;
			}
			if (rootNode.IsEmpty)
				throw new InvalidOperationException();
			return rootNode;
		}

		static OrderedSelectorList BuildSelectorsPerBitness(OpCodeInfoGroup group, OpCodeArgFlags argFlags, List<InstructionDef> opcodes) {
			var selectors = new OrderedSelectorList();
			foreach (var def in opcodes) {
				if (def.Encoding == EncodingKind.Legacy) {
					int bitness = GetBitness(def);
					var selectorKind = bitness switch {
						16 => OpCodeSelectorKind.Bitness16,
						32 => OpCodeSelectorKind.Bitness32,
						64 => OpCodeSelectorKind.Bitness64,
						_ => throw new InvalidOperationException(),
					};
					selectors.Add(selectorKind, def);
				}
				else
					throw new InvalidOperationException($"Unable to detect bitness for opcode {def.Code.RawName} for group {group.Name} / {argFlags}");
			}
			if (selectors.Count == opcodes.Count)
				return selectors;

			// Try to detect bitness differently (for dec_rm16/dec_r16)
			if (selectors.Count == 1) {
				selectors.Clear();
				var added = new HashSet<InstructionDef>();
				foreach (var bitnessMask in new[] { InstructionDefFlags1.Bit64, InstructionDefFlags1.Bit32, InstructionDefFlags1.Bit16 }) {
					foreach (var def in opcodes) {
						if ((def.Flags1 & bitnessMask) == 0)
							continue;
						if (added.Contains(def))
							continue;
						var selectorKind = bitnessMask switch {
							InstructionDefFlags1.Bit16 => OpCodeSelectorKind.Bitness16,
							InstructionDefFlags1.Bit32 => OpCodeSelectorKind.Bitness32,
							InstructionDefFlags1.Bit64 => OpCodeSelectorKind.Bitness64,
							_ => throw new InvalidOperationException(),
						};
						added.Add(def);
						selectors.Add(selectorKind, def);
					}
				}
			}

			return selectors;
		}

		static int GetBroadcastMemory(OpCodeArgFlags argFlags, List<InstructionDef> opcodes, Signature signature,
			out OpCodeSelectorKind selectorKind, out InstructionDef? broadcastDef) {
			broadcastDef = null;
			selectorKind = OpCodeSelectorKind.Invalid;
			int memoryIndex = -1;
			if ((argFlags & OpCodeArgFlags.HasBroadcast) != 0) {
				for (int i = 0; i < signature.ArgCount; i++) {
					if (signature.GetArgKind(i) == ArgKind.Memory) {
						memoryIndex = i;
						var evex = opcodes.First(x => x.Encoding == EncodingKind.EVEX);
						if (evex.OpKindDefs[i].OperandEncoding != OperandEncoding.RegMemModrmRm)
							throw new InvalidOperationException();
						broadcastDef = evex;
						selectorKind = evex.OpKindDefs[i].Register switch {
							Register.XMM0 => OpCodeSelectorKind.EvexBroadcastX,
							Register.YMM0 => OpCodeSelectorKind.EvexBroadcastY,
							Register.ZMM0 => OpCodeSelectorKind.EvexBroadcastZ,
							_ => throw new InvalidOperationException(),
						};
						break;
					}
				}
				if (memoryIndex < 0)
					throw new InvalidOperationException();
			}

			return memoryIndex;
		}

		static bool ShouldDiscardDuplicatedOpCode(Signature signature, InstructionDef def) {
			if ((def.Flags3 & InstructionDefFlags3.AsmIgnoreMemory) != 0) {
				for (int i = 0; i < signature.ArgCount; i++) {
					var kind = signature.GetArgKind(i);
					if (kind == ArgKind.Memory)
						return true;
				}
			}

			return false;
		}

		static int GetBitness(InstructionDef def) {
			var sizeFlags = def.Flags1 & (InstructionDefFlags1.Bit16 | InstructionDefFlags1.Bit32 | InstructionDefFlags1.Bit64);
			var operandSize = def.OperandSize == CodeSize.Unknown ? def.AddressSize : def.OperandSize;
			return sizeFlags switch {
				InstructionDefFlags1.Bit16 | InstructionDefFlags1.Bit32 => operandSize == CodeSize.Unknown || operandSize == CodeSize.Code16 ? 16 : 32,
				InstructionDefFlags1.Bit16 | InstructionDefFlags1.Bit32 | InstructionDefFlags1.Bit64 => operandSize == CodeSize.Code16 ? 16 : 32,
				InstructionDefFlags1.Bit64 => operandSize == CodeSize.Code16 ? 16 : operandSize == CodeSize.Code32 ? 32 : 64,
				_ => throw new InvalidOperationException(),
			};
		}

		static List<int> CollectByOperandKindPredicate(List<InstructionDef> defs, Func<OpCodeOperandKindDef, bool?> predicate,
			List<InstructionDef> opcodesMatchingPredicate, List<InstructionDef> opcodesNotMatchingPredicate) {
			var argIndices = new List<int>();
			foreach (var def in defs) {
				var selected = opcodesNotMatchingPredicate;
				for (int i = 0; i < def.OpKindDefs.Length; i++) {
					var argOpKind = def.OpKindDefs[i];
					if (predicate(argOpKind) is bool result) {
						if (result) {
							if (!argIndices.Contains(i))
								argIndices.Add(i);
							selected = opcodesMatchingPredicate;
						}
						break;
					}
				}
				selected.Add(def);
			}

			return argIndices;
		}

		static bool? IsShortBranch(OpCodeOperandKindDef def) =>
			def.OperandEncoding switch {
				OperandEncoding.NearBranch or OperandEncoding.Xbegin or OperandEncoding.AbsNearBranch => def.BranchOffsetSize == 8,
				_ => null,
			};

		static bool? IsImmediateByteSigned(OpCodeOperandKindDef def) {
			if (def.OperandEncoding == OperandEncoding.Immediate) {
				switch ((def.ImmediateSize, def.ImmediateSignExtSize)) {
				case (8, 16):
				case (8, 32):
				case (8, 64):
					return true;
				}
			}

			return null;
		}

		static bool? IsImmediateByteEqual1(OpCodeOperandKindDef def) {
			switch (def.OperandEncoding) {
			case OperandEncoding.ImpliedConst:
				if (def.ImpliedConst != 1)
					throw new InvalidOperationException();
				return true;
			}

			return null;
		}

		OrderedSelectorList BuildSelectorsByRegisterOrMemory(Signature signature, OpCodeArgFlags argFlags, List<InstructionDef> opcodes,
			bool isRegister) {
			List<OrderedSelectorList>? selectorsList = null;
			for (int argIndex = 0; argIndex < signature.ArgCount; argIndex++) {
				var argKind = signature.GetArgKind(argIndex);
				if ((isRegister && !IsRegister(argKind)) || (!isRegister && argKind != ArgKind.Memory))
					continue;

				var selectors = new OrderedSelectorList() { ArgIndex = argIndex };
				foreach (var def in opcodes) {
					var argOpKind = def.OpKindDefs[argIndex];
					var conditionKind = GetSelectorKindForRegisterOrMemory(def, argOpKind,
											(argFlags & OpCodeArgFlags.HasRegisterMemoryMappedToRegister) != 0);
					selectors.Add(conditionKind, def);
				}

				// If we have already found the best selector, we can return immediately
				if (selectors.Count == opcodes.Count)
					return selectors;

				selectorsList ??= new List<OrderedSelectorList>();
				selectorsList.Add(selectors);
			}

			if (selectorsList is null)
				return new OrderedSelectorList();

			// Select the largest selector
			return selectorsList.First(x => x.Count == selectorsList.Max(x => x.Count));
		}

		protected static bool IsRegister(ArgKind kind) =>
			kind switch {
				ArgKind.Register8 or ArgKind.Register16 or ArgKind.Register32 or ArgKind.Register64 or ArgKind.RegisterK or
				ArgKind.RegisterSt or ArgKind.RegisterSegment or ArgKind.RegisterBnd or ArgKind.RegisterMm or ArgKind.RegisterXmm or
				ArgKind.RegisterYmm or ArgKind.RegisterZmm or ArgKind.RegisterCr or ArgKind.RegisterDr or ArgKind.RegisterTr or
				ArgKind.RegisterTmm => true,
				_ => false,
			};

		static int GetImmediateSizeInBits(InstructionDef def) {
			var opKindDef = def.OpKindDefs[0];
			return opKindDef.OperandEncoding == OperandEncoding.Immediate ? opKindDef.ImmediateSignExtSize : 0;
		}

		static int GetMemorySizeInBits(MemorySizeDef[] memDefs, InstructionDef[] defs, InstructionDef def) {
			var memSize = (MemorySize)defs[def.Code.Value].Memory.Value;
			switch (memSize) {
			case MemorySize.Fword6:
				memSize = MemorySize.UInt32;
				break;
			case MemorySize.Fword10:
				memSize = MemorySize.UInt64;
				break;
			}
			var size = memDefs[(int)memSize].Size;
			return (int)size * 8;
		}

		[DebuggerDisplay("Count = {" + nameof(Count) + "}")]
		sealed class OrderedSelectorList : List<(OpCodeSelectorKind, List<InstructionDef>)> {
			public int ArgIndex { get; set; }

			public OrderedSelectorList() => ArgIndex = -1;

			public void Add(OpCodeSelectorKind kindToAdd, InstructionDef def) {
				foreach (var (kind, list) in this) {
					if (kind == kindToAdd) {
						if (!list.Contains(def))
							list.Add(def);
						return;
					}
				}
				Add((kindToAdd, new List<InstructionDef>(1) { def }));
			}
		}

		static int GetPriorityFromKind(OpCodeOperandKindDef def, int memSize) {
			switch (def.OperandEncoding) {
			case OperandEncoding.NearBranch:
			case OperandEncoding.Xbegin:
			case OperandEncoding.AbsNearBranch:
			case OperandEncoding.FarBranch:
			case OperandEncoding.SegRBX:
			case OperandEncoding.SegRSI:
			case OperandEncoding.SegRDI:
			case OperandEncoding.ESRDI:
				break;

			case OperandEncoding.Immediate:
				return (def.ImmediateSize, def.ImmediateSignExtSize) switch {
					(32, 64) => 10,
					(64, 64) => 10,
					(32, 32) => 20,
					(16, 16) => 30,
					(8, _) => 50,
					(4, 4) => 50,
					_ => throw new InvalidOperationException(),
				};

			case OperandEncoding.ImpliedConst:
				return 40;

			case OperandEncoding.ImpliedRegister:
				return def.Register switch {
					Register.RAX => 0,
					Register.ST0 => 0,
					Register.EAX => 15,
					Register.AX => 25,
					Register.DX => 25,
					Register.AL => 45,
					Register.CL => 45,
					_ => int.MaxValue,
				};

			case OperandEncoding.RegImm:
			case OperandEncoding.RegOpCode:
			case OperandEncoding.RegModrmReg:
			case OperandEncoding.RegModrmRm:
			case OperandEncoding.RegMemModrmRm:
			case OperandEncoding.RegVvvvv:
				return def.Register switch {
					Register.ZMM0 => -30,
					Register.YMM0 => -20,
					Register.XMM0 => -10,
					Register.RAX => 10,
					Register.ST0 => 20,
					Register.EAX => 20,
					Register.AX => 30,
					Register.AL => 50,
					Register.ES => 60,
					_ => int.MaxValue,
				};

			case OperandEncoding.MemModrmRm:
			case OperandEncoding.MemOffset:
				return memSize switch {
					80 => 5,
					64 => 10,
					48 => 15,
					32 => 20,
					16 => 30,
					8 => 50,
					_ => int.MaxValue,
				};

			case OperandEncoding.None:
			default:
				throw new InvalidOperationException();
			}

			return int.MaxValue;
		}

		[Flags]
		protected enum OpCodeArgFlags {
			None = 0,
			HasImmediateByteEqual1 = 1 << 0,
			HasImmediateByteLessThanBits = 1 << 1,
			HasImmediateByteSignExtended = 1 << 2,
			HasLabel = 1 << 3,
			HasShortBranch = 1 << 4,
			HasNearBranch = 1 << 5,
			HasVex = 1 << 6,
			HasEvex = 1 << 7,
			HasRegisterMemoryMappedToRegister = 1 << 8,
			HasSpecialInstructionEncoding = 1 << 9,
			HasZeroingMask = 1 << 10,
			HasKMask = 1 << 11,
			HasBroadcast = 1 << 12,
			SuppressAllExceptions = 1 << 13,
			RoundingControl = 1 << 14,
			IsBroadcastXYZ = 1 << 15,
			HasLabelUlong = 1 << 16,
			HasImmediateByte = 1 << 17,
			UnsignedUIntNotSupported = 1 << 18,
			HasImmediateUnsigned = 1 << 19,
			GenerateInvalidTest = 1 << 20,
			GeneratedCondCode = 1 << 21,
		}

		void FilterOpCodesRegister(OpCodeInfoGroup group, List<InstructionDef> inputDefs, List<InstructionDef> opcodes,
			HashSet<Signature> signatures, bool allowMemory) {
			var bitnessFlags = InstructionDefFlags1.None;
			var vexOrEvexFlags = OpCodeArgFlags.None;

			foreach (var def in inputDefs) {
				var registerSignature = new Signature();
				bool isValid = true;
				for (int i = 0; i < def.OpKindDefs.Length; i++) {
					var argKind = GetFilterRegisterKindFromOpKind(def.OpKindDefs[i], GetMemorySizeInBits(memDefs, defs, def), allowMemory);
					if (argKind == ArgKind.Unknown) {
						isValid = false;
						break;
					}

					registerSignature.AddArgKind(argKind);
				}

				var codeBitnessFlags = def.Flags1 & (InstructionDefFlags1.Bit64 | InstructionDefFlags1.Bit32 | InstructionDefFlags1.Bit16);
				var codeEvexFlags = def.Encoding switch {
					EncodingKind.VEX => OpCodeArgFlags.HasVex,
					EncodingKind.EVEX => OpCodeArgFlags.HasEvex,
					EncodingKind.MVEX => throw new InvalidOperationException(),
					_ => OpCodeArgFlags.None,
				};

				if (isValid &&
					(signatures.Add(registerSignature) ||
					((bitnessFlags & codeBitnessFlags) != codeBitnessFlags) ||
					(codeEvexFlags != OpCodeArgFlags.None && (vexOrEvexFlags & codeEvexFlags) == 0) ||
					(group.Flags & (OpCodeArgFlags.RoundingControl | OpCodeArgFlags.SuppressAllExceptions)) != 0)) {
					bitnessFlags |= codeBitnessFlags;
					vexOrEvexFlags |= codeEvexFlags;
					if (!opcodes.Contains(def))
						opcodes.Add(def);
				}
			}
		}

		static ArgKind GetFilterRegisterKindFromOpKind(OpCodeOperandKindDef def, int memSize, bool allowMemory) {
			switch (def.OperandEncoding) {
			case OperandEncoding.NearBranch:
			case OperandEncoding.Xbegin:
			case OperandEncoding.AbsNearBranch:
			case OperandEncoding.FarBranch:
			case OperandEncoding.SegRBX:
			case OperandEncoding.SegRSI:
			case OperandEncoding.SegRDI:
			case OperandEncoding.ESRDI:
				break;

			case OperandEncoding.Immediate:
				if (def.ImmediateSize == 2)
					return ArgKind.FilterImmediate2;
				if (def.ImmediateSize == 8)
					return ArgKind.FilterImmediate8;
				return ArgKind.Immediate;

			case OperandEncoding.ImpliedConst:
				return ArgKind.FilterImmediate1;

			case OperandEncoding.ImpliedRegister:
				return def.Register switch {
					Register.AL => ArgKind.FilterRegisterAL,
					Register.CL => ArgKind.FilterRegisterCL,
					Register.AX => ArgKind.FilterRegisterAX,
					Register.DX => ArgKind.FilterRegisterDX,
					Register.EAX => ArgKind.FilterRegisterEAX,
					Register.RAX => ArgKind.FilterRegisterRAX,
					Register.ES => ArgKind.FilterRegisterES,
					Register.CS => ArgKind.FilterRegisterCS,
					Register.SS => ArgKind.FilterRegisterSS,
					Register.DS => ArgKind.FilterRegisterDS,
					Register.FS => ArgKind.FilterRegisterFS,
					Register.GS => ArgKind.FilterRegisterGS,
					Register.ST0 => ArgKind.RegisterSt,
					_ => throw new InvalidOperationException(),
				};

			case OperandEncoding.RegImm:
			case OperandEncoding.RegOpCode:
			case OperandEncoding.RegModrmReg:
			case OperandEncoding.RegModrmRm:
			case OperandEncoding.RegVvvvv:
			case OperandEncoding.RegMemModrmRm:
				if (def.OperandEncoding == OperandEncoding.RegMemModrmRm && !allowMemory)
					break;
				return def.Register switch {
					Register.AL => ArgKind.Register8,
					Register.AX => ArgKind.Register16,
					Register.EAX => ArgKind.Register32,
					Register.RAX => ArgKind.Register64,
					Register.ST0 => ArgKind.RegisterSt,
					Register.ES => ArgKind.RegisterSegment,
					Register.BND0 => ArgKind.RegisterBnd,
					Register.CR0 => ArgKind.RegisterCr,
					Register.DR0 => ArgKind.RegisterTr,
					Register.TR0 => ArgKind.RegisterDr,
					Register.K0 => ArgKind.RegisterK,
					Register.MM0 => ArgKind.RegisterMm,
					Register.XMM0 => ArgKind.RegisterXmm,
					Register.YMM0 => ArgKind.RegisterYmm,
					Register.ZMM0 => ArgKind.RegisterZmm,
					Register.TMM0 => ArgKind.RegisterTmm,
					_ => throw new InvalidOperationException(),
				};

			case OperandEncoding.MemModrmRm:
			case OperandEncoding.MemOffset:
				if (allowMemory) {
					return memSize switch {
						64 => ArgKind.Register64,
						32 => ArgKind.Register32,
						16 => ArgKind.Register16,
						8 => ArgKind.Register8,
						_ => throw new InvalidOperationException(),
					};
				}
				break;

			case OperandEncoding.None:
			default:
				throw new InvalidOperationException();
			}

			return ArgKind.Unknown;
		}

		OpCodeSelectorKind GetSelectorKindForRegisterOrMemory(InstructionDef def, OpCodeOperandKindDef opKindDef,
			bool returnMemoryAsRegister) {
			switch (opKindDef.OperandEncoding) {
			case OperandEncoding.ImpliedRegister:
				return opKindDef.Register switch {
					Register.ES => OpCodeSelectorKind.RegisterES,
					Register.CS => OpCodeSelectorKind.RegisterCS,
					Register.SS => OpCodeSelectorKind.RegisterSS,
					Register.DS => OpCodeSelectorKind.RegisterDS,
					Register.FS => OpCodeSelectorKind.RegisterFS,
					Register.GS => OpCodeSelectorKind.RegisterGS,
					Register.AL => OpCodeSelectorKind.RegisterAL,
					Register.CL => OpCodeSelectorKind.RegisterCL,
					Register.AX => OpCodeSelectorKind.RegisterAX,
					Register.DX => OpCodeSelectorKind.RegisterDX,
					Register.EAX => OpCodeSelectorKind.RegisterEAX,
					Register.RAX => OpCodeSelectorKind.RegisterRAX,
					Register.ST0 => OpCodeSelectorKind.RegisterST0,
					_ => throw new InvalidOperationException(),
				};

			case OperandEncoding.RegImm:
			case OperandEncoding.RegOpCode:
			case OperandEncoding.RegModrmReg:
			case OperandEncoding.RegModrmRm:
			case OperandEncoding.RegVvvvv:
				return opKindDef.Register switch {
					Register.AL => OpCodeSelectorKind.Register8,
					Register.AX => OpCodeSelectorKind.Register16,
					Register.EAX => OpCodeSelectorKind.Register32,
					Register.RAX => OpCodeSelectorKind.Register64,
					Register.ES => OpCodeSelectorKind.RegisterSegment,
					Register.K0 => OpCodeSelectorKind.RegisterK,
					Register.MM0 => OpCodeSelectorKind.RegisterMM,
					Register.XMM0 => OpCodeSelectorKind.RegisterXMM,
					Register.YMM0 => OpCodeSelectorKind.RegisterYMM,
					Register.ZMM0 => OpCodeSelectorKind.RegisterZMM,
					Register.TMM0 => OpCodeSelectorKind.RegisterTMM,
					Register.CR0 => OpCodeSelectorKind.RegisterCR,
					Register.DR0 => OpCodeSelectorKind.RegisterDR,
					Register.TR0 => OpCodeSelectorKind.RegisterTR,
					Register.BND0 => OpCodeSelectorKind.RegisterBND,
					Register.ST0 => OpCodeSelectorKind.RegisterST,
					_ => throw new InvalidOperationException(),
				};

			case OperandEncoding.RegMemModrmRm:
				return opKindDef.Register switch {
					Register.AL => returnMemoryAsRegister ? OpCodeSelectorKind.Register8 : GetOpCodeSelectorKindForMemory(def, OpCodeSelectorKind.Memory8),
					Register.AX => returnMemoryAsRegister ? OpCodeSelectorKind.Register16 : GetOpCodeSelectorKindForMemory(def, OpCodeSelectorKind.Memory16),
					Register.EAX => returnMemoryAsRegister ? OpCodeSelectorKind.Register32 : GetOpCodeSelectorKindForMemory(def, OpCodeSelectorKind.Memory32),
					Register.RAX => returnMemoryAsRegister ? OpCodeSelectorKind.Register64 : GetOpCodeSelectorKindForMemory(def, OpCodeSelectorKind.Memory64),
					Register.MM0 => returnMemoryAsRegister ? OpCodeSelectorKind.RegisterMM : GetOpCodeSelectorKindForMemory(def, OpCodeSelectorKind.MemoryMM),
					Register.XMM0 => returnMemoryAsRegister ? OpCodeSelectorKind.RegisterXMM : GetOpCodeSelectorKindForMemory(def, OpCodeSelectorKind.MemoryXMM),
					Register.YMM0 => returnMemoryAsRegister ? OpCodeSelectorKind.RegisterYMM : GetOpCodeSelectorKindForMemory(def, OpCodeSelectorKind.MemoryYMM),
					Register.ZMM0 => returnMemoryAsRegister ? OpCodeSelectorKind.RegisterZMM : GetOpCodeSelectorKindForMemory(def, OpCodeSelectorKind.MemoryZMM),
					Register.BND0 => returnMemoryAsRegister ? OpCodeSelectorKind.RegisterBND : GetOpCodeSelectorKindForMemory(def, OpCodeSelectorKind.Memory64),
					Register.K0 => returnMemoryAsRegister ? OpCodeSelectorKind.RegisterK : GetOpCodeSelectorKindForMemory(def, OpCodeSelectorKind.Memory),
					_ => throw new InvalidOperationException(),
				};

			case OperandEncoding.MemModrmRm:
			case OperandEncoding.MemOffset:
				if (opKindDef.Vsib32) {
					return opKindDef.Register switch {
						Register.XMM0 => OpCodeSelectorKind.MemoryIndex32Xmm,
						Register.YMM0 => OpCodeSelectorKind.MemoryIndex32Ymm,
						Register.ZMM0 => OpCodeSelectorKind.MemoryIndex32Zmm,
						_ => throw new InvalidOperationException(),
					};
				}
				else if (opKindDef.Vsib64) {
					return opKindDef.Register switch {
						Register.XMM0 => OpCodeSelectorKind.MemoryIndex64Xmm,
						Register.YMM0 => OpCodeSelectorKind.MemoryIndex64Ymm,
						Register.ZMM0 => OpCodeSelectorKind.MemoryIndex64Zmm,
						_ => throw new InvalidOperationException(),
					};
				}
				return GetOpCodeSelectorKindForMemory(def, OpCodeSelectorKind.Memory);

			case OperandEncoding.None:
			case OperandEncoding.NearBranch:
			case OperandEncoding.Xbegin:
			case OperandEncoding.AbsNearBranch:
			case OperandEncoding.FarBranch:
			case OperandEncoding.Immediate:
			case OperandEncoding.ImpliedConst:
			case OperandEncoding.SegRBX:
			case OperandEncoding.SegRSI:
			case OperandEncoding.SegRDI:
			case OperandEncoding.ESRDI:
			default:
				throw new InvalidOperationException();
			}
		}

		OpCodeSelectorKind GetOpCodeSelectorKindForMemory(InstructionDef def, OpCodeSelectorKind defaultMemory) {
			var memSize = (MemorySize)defs[def.Code.Value].Memory.Value;
			switch (memSize) {
			case MemorySize.Fword6:
				memSize = MemorySize.UInt32;
				break;
			case MemorySize.Fword10:
				memSize = MemorySize.UInt64;
				break;
			}

			var sizeBits = 8 * memDefs[(int)memSize].Size;
			return sizeBits switch {
				512 => OpCodeSelectorKind.MemoryZMM,
				256 => OpCodeSelectorKind.MemoryYMM,
				128 => OpCodeSelectorKind.MemoryXMM,
				80 => OpCodeSelectorKind.Memory80,
				64 => OpCodeSelectorKind.Memory64,
				48 => OpCodeSelectorKind.Memory48,
				32 => OpCodeSelectorKind.Memory32,
				16 => OpCodeSelectorKind.Memory16,
				8 => OpCodeSelectorKind.Memory8,
				_ => defaultMemory,
			};
		}

		protected virtual bool SupportsUnsigned => true;
		OpCodeInfoGroup AddOpCodeToGroup(string name, string mnemonicName, Signature signature, InstructionDef def, OpCodeArgFlags opCodeArgFlags,
			PseudoOpsKind? pseudoOpsKind, int numberLeadingArgsToDiscard, List<int> argSizes, bool isOtherImmediate) {
			var key = new GroupKey(name, signature);
			if (!groups.TryGetValue(key, out var group)) {
				group = new OpCodeInfoGroup(memDefs, defs, name, mnemonicName, signature, numberLeadingArgsToDiscard, pseudoOpsKind, null, 0);
				groups.Add(key, group);
				if (pseudoOpsKind is not null)
					groupsWithPseudo.Add(key, group);
			}
			if (group.MnemonicName != mnemonicName)
				throw new InvalidOperationException();
			if (group.NumberOfLeadingArgsToDiscard != numberLeadingArgsToDiscard)
				throw new InvalidOperationException();
			if (group.RootPseudoOpsKind != pseudoOpsKind)
				throw new InvalidOperationException();

			if (!group.Defs.Contains(def))
				group.Defs.Add(def);
			group.Flags |= opCodeArgFlags;
			group.AllDefFlags |= def.Flags1;

			group.UpdateMaxArgSizes(argSizes);

			// Duplicate immediate signatures with opposite unsigned/signed version
			if (!isOtherImmediate && (opCodeArgFlags & OpCodeArgFlags.UnsignedUIntNotSupported) == 0 && SupportsUnsigned) {
				var signatureWithOtherImmediate = new Signature();
				for (int i = 0; i < signature.ArgCount; i++) {
					var argKind = signature.GetArgKind(i);
					switch (argKind) {
					case ArgKind.Immediate:
						argKind = ArgKind.ImmediateUnsigned;
						break;
					}

					signatureWithOtherImmediate.AddArgKind(argKind);
				}

				if (signature != signatureWithOtherImmediate) {
					AddOpCodeToGroup(name, mnemonicName, signatureWithOtherImmediate, def, opCodeArgFlags | OpCodeArgFlags.HasImmediateUnsigned,
						null, numberLeadingArgsToDiscard, argSizes, true);
				}
			}

			if ((opCodeArgFlags & OpCodeArgFlags.HasRegisterMemoryMappedToRegister) == 0 &&
				(opCodeArgFlags & OpCodeArgFlags.IsBroadcastXYZ) == 0) {
				var broadcastName = RenameAmbiguousBroadcasts(name, def);
				if (broadcastName != name) {
					AddOpCodeToGroup(broadcastName, mnemonicName, signature, def, opCodeArgFlags | OpCodeArgFlags.IsBroadcastXYZ,
						pseudoOpsKind, numberLeadingArgsToDiscard, argSizes, isOtherImmediate);
				}
			}

			// Handle label as ulong
			if ((opCodeArgFlags & OpCodeArgFlags.HasLabelUlong) == 0 && (opCodeArgFlags & OpCodeArgFlags.HasLabel) != 0) {
				var newLabelULongSignature = new Signature();
				for (int i = 0; i < signature.ArgCount; i++) {
					var argKind = signature.GetArgKind(i);
					switch (argKind) {
					case ArgKind.Label:
						argKind = ArgKind.LabelU64;
						break;
					}
					newLabelULongSignature.AddArgKind(argKind);
				}
				AddOpCodeToGroup(name, mnemonicName, newLabelULongSignature, def, opCodeArgFlags | OpCodeArgFlags.HasLabelUlong,
					pseudoOpsKind, numberLeadingArgsToDiscard, argSizes, isOtherImmediate);
			}

			if (def.ConditionCode != ConditionCode.None && (opCodeArgFlags & OpCodeArgFlags.GeneratedCondCode) == 0) {
				var newCcStrings = GetConditionCodeStrings(group.Defs[0].ConditionCode);
				if (!group.MnemonicName.StartsWith(def.MnemonicCcPrefix, StringComparison.OrdinalIgnoreCase))
					throw new InvalidOperationException();
				if (!group.MnemonicName.EndsWith(def.MnemonicCcSuffix, StringComparison.OrdinalIgnoreCase))
					throw new InvalidOperationException();
				if (!group.Name.StartsWith(def.MnemonicCcPrefix, StringComparison.Ordinal))
					throw new InvalidOperationException();
				if (!group.Name.EndsWith(def.MnemonicCcSuffix, StringComparison.Ordinal))
					throw new InvalidOperationException();
				foreach (var newCcString in newCcStrings) {
					var newName = def.MnemonicCcPrefix + newCcString + def.MnemonicCcSuffix;
					var newMnemonicName = newName[0..1].ToUpperInvariant() + newName[1..];
					AddOpCodeToGroup(newName, newMnemonicName, signature, def, opCodeArgFlags | OpCodeArgFlags.GeneratedCondCode,
						pseudoOpsKind, numberLeadingArgsToDiscard, argSizes, isOtherImmediate);
				}
			}

			return group;
		}

		static string[] GetConditionCodeStrings(ConditionCode cc) =>
			cc switch {
				ConditionCode.o => Array.Empty<string>(),
				ConditionCode.no => Array.Empty<string>(),
				ConditionCode.b => new[] { "c", "nae" },
				ConditionCode.ae => new[] { "nb", "nc" },
				ConditionCode.e => new[] { "z" },
				ConditionCode.ne => new[] { "nz" },
				ConditionCode.be => new[] { "na" },
				ConditionCode.a => new[] { "nbe" },
				ConditionCode.s => Array.Empty<string>(),
				ConditionCode.ns => Array.Empty<string>(),
				ConditionCode.p => new[] { "pe" },
				ConditionCode.np => new[] { "po" },
				ConditionCode.l => new[] { "nge" },
				ConditionCode.ge => new[] { "nl" },
				ConditionCode.le => new[] { "ng" },
				ConditionCode.g => new[] { "nle" },
				_ => throw new InvalidOperationException(),
			};

		protected static bool IsArgKindImmediate(ArgKind argKind) =>
			argKind switch {
				ArgKind.Immediate or ArgKind.ImmediateUnsigned => true,
				_ => false,
			};

		void CreatePseudoInstructions() {
			foreach (var group in groupsWithPseudo.Values) {
				if (group.Signature.ArgCount < 1)
					throw new InvalidOperationException();
				var pseudo = group.RootPseudoOpsKind ?? throw new InvalidOperationException("Root cannot be null");
				var pseudoInfo = FormatterConstants.GetPseudoOps(pseudo);

				// Create new signature without last imm argument
				if (group.Signature.GetArgKind(group.Signature.ArgCount - 1) != ArgKind.Immediate)
					throw new InvalidOperationException();
				var signature = new Signature();
				for (int i = 0; i < group.Signature.ArgCount - 1; i++)
					signature.AddArgKind(group.Signature.GetArgKind(i));

				var newMaxArgSizes = group.MaxArgSizes.Take(group.MaxArgSizes.Count - 1).ToList();
				for (int i = 0; i < pseudoInfo.Length; i++) {
					var (name, imm) = pseudoInfo[i];
					var key = new GroupKey(name, signature);
					// eg. EVEX_Vpcmpeqd_kr_k1_xmm_xmmm128b32 vs EVEX_Vpcmpd_kr_k1_xmm_xmmm128b32_imm8
					if (groups.ContainsKey(key))
						continue;

					var newGroup = new OpCodeInfoGroup(memDefs, defs, name, group.MnemonicName, signature, 0, null, group, imm) {
						AllDefFlags = group.AllDefFlags,
					};
					newGroup.UpdateMaxArgSizes(newMaxArgSizes);
					groups.Add(key, newGroup);
				}
			}
		}

		[DebuggerDisplay("{" + nameof(Name) + "}")]
		protected readonly struct GroupKey : IEquatable<GroupKey>, IComparable<GroupKey> {
			public GroupKey(string name, Signature signature) {
				Name = name;
				Signature = signature;
			}

			public readonly string Name;
			public readonly Signature Signature;

			public bool Equals(GroupKey other) => Name == other.Name && Signature == other.Signature;
			public override bool Equals(object? obj) => obj is GroupKey other && Equals(other);
			public override int GetHashCode() => HashCode.Combine(Name, Signature);
			public static bool operator ==(GroupKey left, GroupKey right) => left.Equals(right);
			public static bool operator !=(GroupKey left, GroupKey right) => !left.Equals(right);

			public int CompareTo(GroupKey other) {
				var nameComparison = string.Compare(Name, other.Name, StringComparison.Ordinal);
				if (nameComparison != 0) return nameComparison;
				return Signature.CompareTo(other.Signature);
			}
		}

		protected struct Signature : IEquatable<Signature>, IComparable<Signature> {
			public readonly int ArgCount => argCount;
			int argCount;
			ulong argKinds;

			public ArgKind GetArgKind(int argIndex) => (ArgKind)((argKinds >> (8 * argIndex)) & 0xFF);

			public void AddArgKind(ArgKind kind) {
				var shift = (8 * argCount);
				argKinds |= (ulong)kind << shift;
				argCount++;
			}

			public bool HasKind(ArgKind kind) {
				for (int i = 0; i < argCount; i++) {
					if (GetArgKind(i) == kind)
						return true;
				}
				return false;
			}

			public override string ToString() {
				var builder = new StringBuilder();
				builder.Append('(');
				for (int i = 0; i < argCount; i++) {
					if (i > 0)
						builder.Append(", ");
					builder.Append(GetArgKind(i));
				}
				builder.Append(')');
				return builder.ToString();
			}

			public bool Equals(Signature other) => argCount == other.argCount && argKinds == other.argKinds;
			public override bool Equals(object? obj) => obj is Signature other && Equals(other);
			public override int GetHashCode() => HashCode.Combine(argCount, argKinds);
			public static bool operator ==(Signature left, Signature right) => left.Equals(right);
			public static bool operator !=(Signature left, Signature right) => !left.Equals(right);

			public int CompareTo(Signature other) {
				var argCountComparison = argCount.CompareTo(other.argCount);
				if (argCountComparison != 0)
					return argCountComparison;
				return argKinds.CompareTo(other.argKinds);
			}
		}

		protected enum ArgKind : byte {
			Unknown,
			Register8,
			Register16,
			Register32,
			Register64,
			RegisterK,
			RegisterSt,
			RegisterSegment,
			RegisterBnd,
			RegisterMm,
			RegisterXmm,
			RegisterYmm,
			RegisterZmm,
			RegisterCr,
			RegisterDr,
			RegisterTr,
			RegisterTmm,

			Register8Memory,
			Register16Memory,
			Register32Memory,
			Register64Memory,
			RegisterKMemory,
			RegisterBndMemory,
			RegisterMmMemory,
			RegisterXmmMemory,
			RegisterYmmMemory,
			RegisterZmmMemory,

			Memory,
			Immediate,
			ImmediateUnsigned,
			Label,
			LabelU64,

			FilterRegisterDX,
			FilterRegisterCL,
			FilterRegisterAL,
			FilterRegisterAX,
			FilterRegisterEAX,
			FilterRegisterRAX,
			FilterRegisterES,
			FilterRegisterCS,
			FilterRegisterDS,
			FilterRegisterSS,
			FilterRegisterFS,
			FilterRegisterGS,

			FilterImmediate1,
			FilterImmediate2,
			FilterImmediate8,
		}

		protected sealed class OpCodeInfoGroup {
			readonly MemorySizeDef[] memDefs;
			readonly InstructionDef[] defs;

			public OpCodeInfoGroup(MemorySizeDef[] memDefs, InstructionDef[] defs, string name, string mnemonicName, Signature signature,
				int numberLeadingArgsToDiscard, PseudoOpsKind? rootPseudoOpsKind, OpCodeInfoGroup? parentPseudoOpsKind,
				int pseudoOpsKindImmediateValue) {
				this.memDefs = memDefs;
				this.defs = defs;
				Name = name;
				MnemonicName = mnemonicName;
				Signature = signature;
				Defs = new List<InstructionDef>();
				MaxArgSizes = new List<int>();
				NumberOfLeadingArgsToDiscard = numberLeadingArgsToDiscard;
				RootPseudoOpsKind = rootPseudoOpsKind;
				ParentPseudoOpsKind = parentPseudoOpsKind;
				PseudoOpsKindImmediateValue = pseudoOpsKindImmediateValue;
			}

			public string MnemonicName { get; }
			public string Name { get; }
			public InstructionDefFlags1 AllDefFlags { get; set; }
			public OpCodeArgFlags Flags { get; set; }
			public PseudoOpsKind? RootPseudoOpsKind { get; }
			public OpCodeInfoGroup? ParentPseudoOpsKind { get; }
			public int PseudoOpsKindImmediateValue { get; }
			public bool HasLabel => (Flags & OpCodeArgFlags.HasLabel) != 0;
			public bool HasSpecialInstructionEncoding => (Flags & OpCodeArgFlags.HasSpecialInstructionEncoding) != 0;
			public bool IsBranch => (Flags & (OpCodeArgFlags.HasShortBranch | OpCodeArgFlags.HasNearBranch)) != 0;
			public bool HasRegisterMemoryMappedToRegister => (Flags & OpCodeArgFlags.HasRegisterMemoryMappedToRegister) != 0;
			public bool HasVexAndEvex => (Flags & (OpCodeArgFlags.HasVex | OpCodeArgFlags.HasEvex)) == (OpCodeArgFlags.HasVex | OpCodeArgFlags.HasEvex);
			public bool HasImmediateUnsigned => (Flags & OpCodeArgFlags.HasImmediateUnsigned) != 0;
			public bool HasOpMaskOrZeroingMasking => (Flags & (OpCodeArgFlags.HasKMask | OpCodeArgFlags.HasZeroingMask)) != 0;
			public bool HasBroadcast => (Flags & OpCodeArgFlags.HasBroadcast) != 0;
			public bool HasSaeOrRc => (Flags & (OpCodeArgFlags.SuppressAllExceptions | OpCodeArgFlags.RoundingControl)) != 0;
			public Signature Signature { get; }
			public OpCodeNode RootOpCodeNode { get; set; }
			public List<InstructionDef> Defs { get; }
			public List<int> MaxArgSizes { get; }
			public int NumberOfLeadingArgsToDiscard { get; }
			public bool AddNameSuffix { get; set; }

			public IEnumerable<InstructionDef> GetDefsAndParentDefs() {
				foreach (var def in Defs)
					yield return def;
				if (ParentPseudoOpsKind is OpCodeInfoGroup parent) {
					foreach (var def in parent.GetDefsAndParentDefs())
						yield return def;
				}
			}

			public void UpdateMaxArgSizes(List<int> argSizes) {
				if (MaxArgSizes.Count == 0)
					MaxArgSizes.AddRange(argSizes);
				if (Signature.ArgCount != argSizes.Count)
					throw new InvalidOperationException();
				for (int i = 0; i < MaxArgSizes.Count; i++)
					MaxArgSizes[i] = Math.Max(MaxArgSizes[i], argSizes[i]);
			}

			public int OrderOpCodesPerOpKindPriority(InstructionDef x, InstructionDef y) {
				if (x.OpKindDefs.Length != y.OpKindDefs.Length)
					throw new InvalidOperationException();
				int result;
				for (int i = 0; i < x.OpKindDefs.Length; i++) {
					if (!IsRegister(Signature.GetArgKind(i)))
						continue;
					result = GetPriorityFromKind(x.OpKindDefs[i], GetMemorySizeInBits(memDefs, defs, x)).
						CompareTo(GetPriorityFromKind(y.OpKindDefs[i], GetMemorySizeInBits(memDefs, defs, y)));
					if (result != 0)
						return result;
				}

				for (int i = 0; i < x.OpKindDefs.Length; i++) {
					if (IsRegister(Signature.GetArgKind(i)))
						continue;
					result = GetPriorityFromKind(x.OpKindDefs[i], GetMemorySizeInBits(memDefs, defs, x)).
						CompareTo(GetPriorityFromKind(y.OpKindDefs[i], GetMemorySizeInBits(memDefs, defs, y)));
					if (result != 0)
						return result;
				}

				// Case for ordering by decreasing bitness
				var xmemSize = (MemorySize)defs[x.Code.Value].Memory.Value;
				var ymemSize = (MemorySize)defs[y.Code.Value].Memory.Value;
				result = xmemSize.CompareTo(ymemSize);
				if (result == 0) {
					if (x.Encoding == EncodingKind.Legacy && y.Encoding == EncodingKind.Legacy) {
						var xBitness = GetBitness(x);
						var yBitness = GetBitness(y);
						result = xBitness.CompareTo(yBitness);
					}
				}
				return -result;
			}
		}

		protected static bool IsSelectorSupportedByBitness(int bitness, OpCodeSelectorKind kind, out bool continueElse) {
			switch (kind) {
			case OpCodeSelectorKind.Bitness64:
				continueElse = bitness < 64;
				return bitness == 64;
			case OpCodeSelectorKind.MemOffs64_RAX:
			case OpCodeSelectorKind.MemOffs64_EAX:
			case OpCodeSelectorKind.MemOffs64_AX:
			case OpCodeSelectorKind.MemOffs64_AL:
				continueElse = true;
				return bitness == 64;
			case OpCodeSelectorKind.Bitness32:
				continueElse = bitness < 32;
				return bitness >= 32;
			case OpCodeSelectorKind.Bitness16:
				continueElse = false;
				return bitness >= 16;
			case OpCodeSelectorKind.MemOffs_RAX:
			case OpCodeSelectorKind.MemOffs_EAX:
			case OpCodeSelectorKind.MemOffs_AX:
			case OpCodeSelectorKind.MemOffs_AL:
				continueElse = true;
				return bitness < 64;
			default:
				continueElse = true;
				return true;
			}
		}

		protected static (OpCodeArgFlags, OpCodeArgFlags) GetIfElseContextFlags(OpCodeSelectorKind kind) =>
			kind switch {
				OpCodeSelectorKind.Vex => (OpCodeArgFlags.HasVex, OpCodeArgFlags.HasEvex),
				OpCodeSelectorKind.EvexBroadcastX or OpCodeSelectorKind.EvexBroadcastY or
				OpCodeSelectorKind.EvexBroadcastZ => (OpCodeArgFlags.HasEvex | OpCodeArgFlags.HasBroadcast, OpCodeArgFlags.None),
				OpCodeSelectorKind.ShortBranch => (OpCodeArgFlags.HasShortBranch, OpCodeArgFlags.HasNearBranch),
				_ => (OpCodeArgFlags.None, OpCodeArgFlags.None),
			};

		bool IsR64M16(InstructionDef def) {
			foreach (var opDef in def.OpKindDefs) {
				if (opDef.OperandEncoding != OperandEncoding.RegMemModrmRm)
					continue;
				if (opDef.Register != Register.RAX)
					continue;
				if (memDefs[(int)def.Memory.Value].Size != 2)
					continue;

				return true;
			}
			return false;
		}

		string RenameAmbiguousBroadcasts(string name, InstructionDef def) {
			if (ambiguousBcst.Contains(def.Code)) {
				for (int i = 0; i < def.OpKindDefs.Length; i++) {
					var opKindDef = def.OpKindDefs[i];
					if (opKindDef.OperandEncoding == OperandEncoding.RegMemModrmRm) {
						return opKindDef.Register switch {
							Register.XMM0 => $"{name}x",
							Register.YMM0 => $"{name}y",
							Register.ZMM0 => $"{name}z",
							_ => throw new InvalidOperationException(),
						};
					}
				}
			}

			return name;
		}

		// Gets indexes of the args (if any) with State/Flags (eg. {k1}, {z}, bcst)
		protected static IEnumerable<int> GetStateArgIndexes(OpCodeInfoGroup group) {
			if (group.HasOpMaskOrZeroingMasking)
				yield return 0;

			if (group.HasBroadcast || group.HasSaeOrRc) {
				for (int i = group.Signature.ArgCount - 1; i >= 0; i--) {
					var argKind = group.Signature.GetArgKind(i);
					if ((group.HasBroadcast && argKind == ArgKind.Memory) || (group.HasSaeOrRc && !IsArgKindImmediate(argKind))) {
						yield return i;
						break;
					}
				}
			}
		}

		protected List<EnumValue> GetDecoderOptions(int bitness, InstructionDef def) {
			var list = new List<EnumValue>();

			if (def.DecoderOption.Value != 0)
				list.Add(decoderOptions[def.DecoderOption.RawName]);
			if ((def.Flags3 & InstructionDefFlags3.RequiresAddressSize32) != 0 && def.AddressSize != CodeSize.Code32)
				list.Add(decoderOptions[nameof(DecoderOptions.NoInvalidCheck)]);
			switch (bitness) {
			case 16:
				if ((def.Flags2 & InstructionDefFlags2.IntelDecoder16) == 0 && (def.Flags2 & InstructionDefFlags2.AmdDecoder16) != 0)
					list.Add(decoderOptions[nameof(DecoderOptions.AMD)]);
				break;
			case 32:
				if ((def.Flags2 & InstructionDefFlags2.IntelDecoder32) == 0 && (def.Flags2 & InstructionDefFlags2.AmdDecoder32) != 0)
					list.Add(decoderOptions[nameof(DecoderOptions.AMD)]);
				break;
			case 64:
				if ((def.Flags2 & InstructionDefFlags2.IntelDecoder64) == 0 && (def.Flags2 & InstructionDefFlags2.AmdDecoder64) != 0)
					list.Add(decoderOptions[nameof(DecoderOptions.AMD)]);
				break;
			default:
				throw new InvalidOperationException();
			}
			if ((def.Flags3 & InstructionDefFlags3.ReservedNop) != 0)
				list.Add(decoderOptions[nameof(DecoderOptions.ForceReservedNop)]);

			return list;
		}

		protected List<EnumValue> GetInstrTestFlags(InstructionDef def, OpCodeInfoGroup group, OpCodeArgFlags flags) {
			var instrFlags = new List<EnumValue>();
			if ((flags & OpCodeArgFlags.HasVex) != 0)
				instrFlags.Add(testInstrFlags[nameof(TestInstrFlags.PreferVex)]);
			if ((flags & OpCodeArgFlags.HasEvex) != 0)
				instrFlags.Add(testInstrFlags[nameof(TestInstrFlags.PreferEvex)]);
			if ((flags & OpCodeArgFlags.HasBroadcast) != 0)
				instrFlags.Add(testInstrFlags[nameof(TestInstrFlags.Broadcast)]);
			if ((flags & OpCodeArgFlags.HasShortBranch) != 0)
				instrFlags.Add(testInstrFlags[nameof(TestInstrFlags.PreferShortBranch)]);
			if ((flags & OpCodeArgFlags.HasNearBranch) != 0)
				instrFlags.Add(testInstrFlags[nameof(TestInstrFlags.PreferNearBranch)]);
			if ((def.Flags1 & InstructionDefFlags1.Fwait) != 0)
				instrFlags.Add(testInstrFlags[nameof(TestInstrFlags.Fwait)]);
			if (group.HasLabel) {
				instrFlags.Add((group.Flags & OpCodeArgFlags.HasLabelUlong) == 0 ?
					testInstrFlags[nameof(TestInstrFlags.Branch)] : testInstrFlags[nameof(TestInstrFlags.BranchU64)]);
			}
			foreach (var cpuid in def.Cpuid) {
				if (cpuid.RawName.Contains("PADLOCK", StringComparison.Ordinal)) {
					// They're mandatory prefix instructions but the REP prefix isn't cleared since it's shown in disassembly
					instrFlags.Add(testInstrFlags[nameof(TestInstrFlags.RemoveRepRepnePrefixes)]);
					break;
				}
			}
			return instrFlags;
		}

		protected static string GetTestMethodArgName(ArgKind kind) =>
			kind switch {
				ArgKind.Register8 => "r8",
				ArgKind.Register16 => "r16",
				ArgKind.Register32 => "r32",
				ArgKind.Register64 => "r64",
				ArgKind.RegisterK => "kr",
				ArgKind.RegisterSt => "st",
				ArgKind.RegisterSegment => "seg",
				ArgKind.RegisterBnd => "bnd",
				ArgKind.RegisterMm => "mm",
				ArgKind.RegisterXmm => "xmm",
				ArgKind.RegisterYmm => "ymm",
				ArgKind.RegisterZmm => "zmm",
				ArgKind.RegisterCr => "cr",
				ArgKind.RegisterDr => "dr",
				ArgKind.RegisterTr => "tr",
				ArgKind.RegisterTmm => "tmm",
				ArgKind.Memory => "m",
				ArgKind.Immediate => "i",
				ArgKind.ImmediateUnsigned => "u",
				ArgKind.Label => "l",
				ArgKind.LabelU64 => "lu64",
				_ => throw new ArgumentOutOfRangeException(kind.ToString()),
			};

		protected static bool IsBitnessSupported(int bitness, InstructionDefFlags1 flags) {
			var bitnessFlags = bitness switch {
				64 => InstructionDefFlags1.Bit64,
				32 => InstructionDefFlags1.Bit32,
				16 => InstructionDefFlags1.Bit16,
				_ => throw new InvalidOperationException(),
			};
			return (flags & bitnessFlags) != 0;
		}

		protected abstract TestArgValueBitness MemToTestArgValue(MemorySizeFuncInfo size, int bitness, ulong address);
		protected abstract TestArgValueBitness MemToTestArgValue(MemorySizeFuncInfo size, Register @base, Register index, int scale, int displ);
		protected abstract TestArgValueBitness RegToTestArgValue(Register register);
		protected abstract TestArgValueBitness UnsignedImmToTestArgValue(ulong immediate, int encImmSizeBits, int immSizeBits, int argSizeBits);
		protected abstract TestArgValueBitness SignedImmToTestArgValue(long immediate, int encImmSizeBits, int immSizeBits, int argSizeBits);
		protected abstract TestArgValueBitness LabelToTestArgValue();

		protected TestArgValue? GetInvalidArgValue(OpCodeSelectorKind selectorKind, int argIndex) =>
			selectorKind switch {
				OpCodeSelectorKind.Memory8 or OpCodeSelectorKind.Memory16 or OpCodeSelectorKind.Memory32 or OpCodeSelectorKind.Memory48 or
				OpCodeSelectorKind.Memory80 or OpCodeSelectorKind.Memory64 =>
					new TestArgValue(
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.ZmmwordPtr], Register.DI, Register.None, 1, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.ZmmwordPtr], Register.EDX, Register.None, 1, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.ZmmwordPtr], Register.RDX, Register.None, 1, 0)
					),
				OpCodeSelectorKind.MemoryMM or OpCodeSelectorKind.MemoryXMM or OpCodeSelectorKind.MemoryYMM or OpCodeSelectorKind.MemoryZMM =>
					new TestArgValue(
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.BytePtr], Register.DI, Register.None, 1, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.BytePtr], Register.EDX, Register.None, 1, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.BytePtr], Register.RDX, Register.None, 1, 0)
					),
				OpCodeSelectorKind.MemoryIndex32Xmm or OpCodeSelectorKind.MemoryIndex64Xmm or OpCodeSelectorKind.MemoryIndex64Ymm or
				OpCodeSelectorKind.MemoryIndex32Ymm =>
					new TestArgValue(
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], Register.EDI, Register.ZMM0 + argIndex, 1, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], Register.EDX, Register.ZMM0 + argIndex, 1, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], Register.RDX, Register.ZMM0 + argIndex, 1, 0)
					),
				_ => null,
			};

		protected IEnumerable<TestArgValue?> GetArgValue(OpCodeSelectorKind selectorKind, bool isElseBranch, int argIndex, Signature signature,
			int argSizeBits) {
			switch (selectorKind) {
			case OpCodeSelectorKind.MemOffs64_RAX:
			case OpCodeSelectorKind.MemOffs64_EAX:
			case OpCodeSelectorKind.MemOffs64_AX:
			case OpCodeSelectorKind.MemOffs64_AL:
				if (isElseBranch) {
					if (argIndex == 0) {
						yield return new TestArgValue(
							MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], Register.DI, Register.None, 1, 0),
							MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], Register.EDI, Register.None, 1, 0),
							MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], Register.RDI, Register.None, 1, 0)
						);
					}
					else {
						yield return new TestArgValue(
							MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], Register.SI, Register.None, 1, 0),
							MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], Register.ESI, Register.None, 1, 0),
							MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], Register.RSI, Register.None, 1, 0)
						);
					}
				}
				else
					yield return new TestArgValue(
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], 16, 0x89AB),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], 32, 0x89ABCDEF),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], 64, 0x89ABCDEF01234567)
					);
				break;
			case OpCodeSelectorKind.MemOffs_RAX:
			case OpCodeSelectorKind.MemOffs_EAX:
			case OpCodeSelectorKind.MemOffs_AX:
			case OpCodeSelectorKind.MemOffs_AL:
				if (isElseBranch) {
					if (argIndex == 0) {
						yield return new TestArgValue(
							MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], Register.DI, Register.None, 1, 0),
							MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], Register.EDI, Register.None, 1, 0),
							MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], Register.RDI, Register.None, 1, 0)
						);
					}
					else {
						yield return new TestArgValue(
							MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], Register.SI, Register.None, 1, 0),
							MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], Register.ESI, Register.None, 1, 0),
							MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], Register.RSI, Register.None, 1, 0)
						);
					}
				}
				else {
					yield return new TestArgValue(
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], 16, 0x1234),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], 32, 0x12345678),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], 64, 0x123456789ABCDEF0)
					);
				}
				break;
			case OpCodeSelectorKind.Bitness64:
				yield return null;
				break;
			case OpCodeSelectorKind.Bitness32:
				yield return null;
				break;
			case OpCodeSelectorKind.Bitness16:
				yield return null;
				break;
			case OpCodeSelectorKind.ShortBranch:
				yield return null;
				break;
			case OpCodeSelectorKind.ImmediateByteEqual1:
				if (isElseBranch) {
					if (signature.GetArgKind(argIndex) == ArgKind.Immediate)
						yield return new TestArgValue(SignedImmToTestArgValue(2, 8, 8, argSizeBits));
					else
						yield return new TestArgValue(UnsignedImmToTestArgValue(2, 8, 8, argSizeBits));
				}
				else {
					if (signature.GetArgKind(argIndex) == ArgKind.Immediate)
						yield return new TestArgValue(SignedImmToTestArgValue(1, 8, 8, argSizeBits));
					else
						yield return new TestArgValue(UnsignedImmToTestArgValue(1, 8, 8, argSizeBits));
				}
				break;
			case OpCodeSelectorKind.ImmediateByteSigned8To16:
			case OpCodeSelectorKind.ImmediateByteSigned8To32:
			case OpCodeSelectorKind.ImmediateByteSigned8To64: {
				int immSize = selectorKind switch {
					OpCodeSelectorKind.ImmediateByteSigned8To16 => 16,
					OpCodeSelectorKind.ImmediateByteSigned8To32 => 32,
					OpCodeSelectorKind.ImmediateByteSigned8To64 => 64,
					_ => throw new InvalidOperationException(),
				};
				if (isElseBranch)
					yield return null;
				else {
					if (signature.GetArgKind(argIndex) == ArgKind.Immediate) {
						yield return new TestArgValue(SignedImmToTestArgValue(sbyte.MinValue, 8, immSize, argSizeBits));
						yield return new TestArgValue(SignedImmToTestArgValue(sbyte.MaxValue, 8, immSize, argSizeBits));
					}
					else {
						yield return new TestArgValue(UnsignedImmToTestArgValue(unchecked((ulong)sbyte.MinValue), 8, immSize, argSizeBits));
						yield return new TestArgValue(UnsignedImmToTestArgValue((ulong)sbyte.MaxValue, 8, immSize, argSizeBits));
					}
				}

				break;
			}
			case OpCodeSelectorKind.Vex:
				yield return null;
				break;
			case OpCodeSelectorKind.EvexBroadcastX:
			case OpCodeSelectorKind.EvexBroadcastY:
			case OpCodeSelectorKind.EvexBroadcastZ:
				if (isElseBranch)
					yield return null;
				else {
					yield return new TestArgValue(
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.DwordBcst], Register.DI, Register.None, 1, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.DwordBcst], Register.EDX, Register.None, 1, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.DwordBcst], Register.RDX, Register.None, 1, 0)
					);
				}
				break;
			case OpCodeSelectorKind.RegisterCL:
				if (isElseBranch)
					yield return null;
				else
					yield return new TestArgValue(RegToTestArgValue(Register.CL));
				break;
			case OpCodeSelectorKind.RegisterAL:
				if (isElseBranch)
					yield return null;
				else
					yield return new TestArgValue(RegToTestArgValue(Register.AL));
				break;
			case OpCodeSelectorKind.RegisterAX:
				if (isElseBranch)
					yield return null;
				else
					yield return new TestArgValue(RegToTestArgValue(Register.AX));
				break;
			case OpCodeSelectorKind.RegisterEAX:
				if (isElseBranch)
					yield return null;
				else
					yield return new TestArgValue(RegToTestArgValue(Register.EAX));
				break;
			case OpCodeSelectorKind.RegisterRAX:
				if (isElseBranch)
					yield return null;
				else
					yield return new TestArgValue(RegToTestArgValue(Register.RAX));
				break;
			case OpCodeSelectorKind.RegisterBND:
				if (isElseBranch)
					yield return null;
				else
					yield return new TestArgValue(RegToTestArgValue(Register.BND0));
				break;
			case OpCodeSelectorKind.RegisterES:
				if (isElseBranch)
					yield return null;
				else
					yield return new TestArgValue(RegToTestArgValue(Register.ES));
				break;
			case OpCodeSelectorKind.RegisterCS:
				if (isElseBranch)
					yield return null;
				else
					yield return new TestArgValue(RegToTestArgValue(Register.CS));
				break;
			case OpCodeSelectorKind.RegisterSS:
				if (isElseBranch)
					yield return null;
				else
					yield return new TestArgValue(RegToTestArgValue(Register.SS));
				break;
			case OpCodeSelectorKind.RegisterDS:
				if (isElseBranch)
					yield return null;
				else
					yield return new TestArgValue(RegToTestArgValue(Register.DS));
				break;
			case OpCodeSelectorKind.RegisterFS:
				if (isElseBranch)
					yield return null;
				else
					yield return new TestArgValue(RegToTestArgValue(Register.FS));
				break;
			case OpCodeSelectorKind.RegisterGS:
				if (isElseBranch)
					yield return null;
				else
					yield return new TestArgValue(RegToTestArgValue(Register.GS));
				break;
			case OpCodeSelectorKind.RegisterDX:
				if (isElseBranch)
					yield return null;
				else
					yield return new TestArgValue(RegToTestArgValue(Register.DX));
				break;
			case OpCodeSelectorKind.Register8:
				if (isElseBranch)
					yield return null;
				else
					yield return new TestArgValue(RegToTestArgValue(Register.BL));
				break;
			case OpCodeSelectorKind.Register16:
				if (isElseBranch)
					yield return null;
				else
					yield return new TestArgValue(RegToTestArgValue(Register.BX));
				break;
			case OpCodeSelectorKind.Register32:
				if (isElseBranch)
					yield return null;
				else
					yield return new TestArgValue(RegToTestArgValue(Register.EBX));
				break;
			case OpCodeSelectorKind.Register64:
				if (isElseBranch)
					yield return null;
				else
					yield return new TestArgValue(RegToTestArgValue(Register.RBX));
				break;
			case OpCodeSelectorKind.RegisterK:
				if (isElseBranch)
					yield return null;
				else
					yield return new TestArgValue(RegToTestArgValue(Register.K0 + argIndex + 2));
				break;
			case OpCodeSelectorKind.RegisterST0:
				if (isElseBranch)
					yield return null;
				else
					yield return new TestArgValue(RegToTestArgValue(Register.ST0));
				break;
			case OpCodeSelectorKind.RegisterST:
				if (isElseBranch)
					yield return null;
				else
					yield return new TestArgValue(RegToTestArgValue(Register.ST3));
				break;
			case OpCodeSelectorKind.RegisterSegment:
				if (isElseBranch)
					yield return null;
				else
					yield return new TestArgValue(RegToTestArgValue(Register.FS));
				break;
			case OpCodeSelectorKind.RegisterCR:
				if (isElseBranch)
					yield return null;
				else
					yield return new TestArgValue(RegToTestArgValue(Register.CR3));
				break;
			case OpCodeSelectorKind.RegisterDR:
				if (isElseBranch)
					yield return null;
				else
					yield return new TestArgValue(RegToTestArgValue(Register.DR5));
				break;
			case OpCodeSelectorKind.RegisterTR:
				if (isElseBranch)
					yield return null;
				else
					yield return new TestArgValue(RegToTestArgValue(Register.TR4));
				break;
			case OpCodeSelectorKind.RegisterMM:
				if (isElseBranch)
					yield return null;
				else
					yield return new TestArgValue(RegToTestArgValue(Register.MM0 + argIndex + 2));
				break;
			case OpCodeSelectorKind.RegisterXMM:
				if (isElseBranch)
					yield return null;
				else
					yield return new TestArgValue(RegToTestArgValue(Register.XMM0 + argIndex + 2));
				break;
			case OpCodeSelectorKind.RegisterYMM:
				if (isElseBranch)
					yield return null;
				else
					yield return new TestArgValue(RegToTestArgValue(Register.YMM0 + argIndex + 2));
				break;
			case OpCodeSelectorKind.RegisterZMM:
				if (isElseBranch)
					yield return null;
				else
					yield return new TestArgValue(RegToTestArgValue(Register.ZMM0 + argIndex + 2));
				break;
			case OpCodeSelectorKind.RegisterTMM:
				if (isElseBranch)
					yield return null;
				else
					yield return new TestArgValue(RegToTestArgValue(Register.TMM0 + argIndex + 2));
				break;
			case OpCodeSelectorKind.Memory8:
				if (isElseBranch)
					yield return null;
				else {
					yield return new TestArgValue(
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.BytePtr], Register.DI, Register.None, 1, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.BytePtr], Register.EDX, Register.None, 1, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.BytePtr], Register.RDX, Register.None, 1, 0)
					);
				}
				break;
			case OpCodeSelectorKind.Memory16:
				if (isElseBranch)
					yield return null;
				else {
					yield return new TestArgValue(
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.WordPtr], Register.DI, Register.None, 1, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.WordPtr], Register.EDX, Register.None, 1, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.WordPtr], Register.RDX, Register.None, 1, 0)
					);
				}
				break;
			case OpCodeSelectorKind.Memory32:
				if (isElseBranch)
					yield return null;
				else {
					yield return new TestArgValue(
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.DwordPtr], Register.DI, Register.None, 1, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.DwordPtr], Register.EDX, Register.None, 1, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.DwordPtr], Register.RDX, Register.None, 1, 0)
					);
				}
				break;
			case OpCodeSelectorKind.Memory80:
				if (isElseBranch)
					yield return null;
				else {
					yield return new TestArgValue(
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.TwordPtr], Register.DI, Register.None, 1, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.TwordPtr], Register.EDX, Register.None, 1, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.TwordPtr], Register.RDX, Register.None, 1, 0)
					);
				}
				break;
			case OpCodeSelectorKind.Memory48:
				if (isElseBranch)
					yield return null;
				else {
					yield return new TestArgValue(
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.FwordPtr], Register.DI, Register.None, 1, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.FwordPtr], Register.EDX, Register.None, 1, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.FwordPtr], Register.RDX, Register.None, 1, 0)
					);
				}
				break;
			case OpCodeSelectorKind.Memory64:
			case OpCodeSelectorKind.MemoryMM:
				if (isElseBranch)
					yield return null;
				else {
					yield return new TestArgValue(
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.QwordPtr], Register.DI, Register.None, 1, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.QwordPtr], Register.EDX, Register.None, 1, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.QwordPtr], Register.RDX, Register.None, 1, 0)
					);
				}
				break;
			case OpCodeSelectorKind.MemoryXMM:
				if (isElseBranch)
					yield return null;
				else {
					yield return new TestArgValue(
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.XmmwordPtr], Register.DI, Register.None, 1, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.XmmwordPtr], Register.EDX, Register.None, 1, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.XmmwordPtr], Register.RDX, Register.None, 1, 0)
					);
				}
				break;
			case OpCodeSelectorKind.MemoryYMM:
				if (isElseBranch)
					yield return null;
				else {
					yield return new TestArgValue(
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.YmmwordPtr], Register.DI, Register.None, 1, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.YmmwordPtr], Register.EDX, Register.None, 1, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.YmmwordPtr], Register.RDX, Register.None, 1, 0)
					);
				}
				break;
			case OpCodeSelectorKind.MemoryZMM:
				if (isElseBranch)
					yield return null;
				else {
					yield return new TestArgValue(
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.ZmmwordPtr], Register.DI, Register.None, 1, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.ZmmwordPtr], Register.EDX, Register.None, 1, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.ZmmwordPtr], Register.RDX, Register.None, 1, 0)
					);
				}
				break;
			case OpCodeSelectorKind.MemoryIndex32Xmm:
			case OpCodeSelectorKind.MemoryIndex64Xmm:
				if (isElseBranch)
					yield return null;
				else {
					yield return new TestArgValue(
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], Register.EDI, Register.XMM0 + argIndex + 2, 1, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], Register.EDX, Register.XMM0 + argIndex + 2, 1, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], Register.RDX, Register.XMM0 + argIndex + 2, 1, 0)
					);
				}
				break;
			case OpCodeSelectorKind.MemoryIndex32Ymm:
			case OpCodeSelectorKind.MemoryIndex64Ymm:
				if (isElseBranch)
					yield return null;
				else {
					yield return new TestArgValue(
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], Register.EDI, Register.YMM0 + argIndex + 2, 1, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], Register.EDX, Register.YMM0 + argIndex + 2, 1, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], Register.RDX, Register.YMM0 + argIndex + 2, 1, 0)
					);
				}
				break;
			case OpCodeSelectorKind.MemoryIndex32Zmm:
			case OpCodeSelectorKind.MemoryIndex64Zmm:
				if (isElseBranch)
					yield return null;
				else {
					yield return new TestArgValue(
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], Register.EDI, Register.ZMM0 + argIndex + 2, 1, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], Register.EDX, Register.ZMM0 + argIndex + 2, 1, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], Register.RDX, Register.ZMM0 + argIndex + 2, 1, 0)
					);
				}
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(selectorKind), selectorKind, null);
			}
		}

		protected TestArgValue GetDefaultArgument(OpCodeOperandKindDef def, int index, ArgKind argKind, int argSizeBits) {
			switch (def.OperandEncoding) {
			case OperandEncoding.NearBranch:
			case OperandEncoding.Xbegin:
			case OperandEncoding.AbsNearBranch:
				if (argKind == ArgKind.LabelU64) {
					if (SupportsUnsigned)
						return new TestArgValue(UnsignedImmToTestArgValue(12752, 64, 64, argSizeBits));
					return new TestArgValue(SignedImmToTestArgValue(12752, 64, 64, argSizeBits));
				}
				return new TestArgValue(LabelToTestArgValue());

			case OperandEncoding.Immediate:
				bool isSigned = argKind == ArgKind.Immediate;
				return (def.ImmediateSize, def.ImmediateSignExtSize) switch {
					(4, 4) => new TestArgValue(isSigned ?
						SignedImmToTestArgValue(3, def.ImmediateSize, def.ImmediateSignExtSize, argSizeBits) :
						UnsignedImmToTestArgValue(3, def.ImmediateSize, def.ImmediateSignExtSize, argSizeBits)
					),
					(8, 8) => new TestArgValue(isSigned ?
						SignedImmToTestArgValue(-5, def.ImmediateSize, def.ImmediateSignExtSize, argSizeBits) :
						UnsignedImmToTestArgValue(127, def.ImmediateSize, def.ImmediateSignExtSize, argSizeBits)
					),
					(8, 16) => new TestArgValue(isSigned ?
						SignedImmToTestArgValue(-5, def.ImmediateSize, def.ImmediateSignExtSize, argSizeBits) :
						UnsignedImmToTestArgValue(5, def.ImmediateSize, def.ImmediateSignExtSize, argSizeBits)
					),
					(8, 32) => new TestArgValue(isSigned ?
						SignedImmToTestArgValue(-9, def.ImmediateSize, def.ImmediateSignExtSize, argSizeBits) :
						UnsignedImmToTestArgValue(9, def.ImmediateSize, def.ImmediateSignExtSize, argSizeBits)
					),
					(8, 64) => new TestArgValue(isSigned ?
						SignedImmToTestArgValue(-10, def.ImmediateSize, def.ImmediateSignExtSize, argSizeBits) :
						UnsignedImmToTestArgValue(10, def.ImmediateSize, def.ImmediateSignExtSize, argSizeBits)
					),
					(16, 16) => new TestArgValue(isSigned ?
						SignedImmToTestArgValue(0x40B7, def.ImmediateSize, def.ImmediateSignExtSize, argSizeBits) :
						UnsignedImmToTestArgValue(0x40B7, def.ImmediateSize, def.ImmediateSignExtSize, argSizeBits)
					),
					(32, 32) => new TestArgValue(isSigned ?
						SignedImmToTestArgValue(int.MaxValue, def.ImmediateSize, def.ImmediateSignExtSize, argSizeBits) :
						UnsignedImmToTestArgValue(int.MaxValue, def.ImmediateSize, def.ImmediateSignExtSize, argSizeBits)
					),
					(32, 64) => new TestArgValue(isSigned ?
						SignedImmToTestArgValue(int.MinValue, def.ImmediateSize, def.ImmediateSignExtSize, argSizeBits) :
						UnsignedImmToTestArgValue(unchecked((ulong)int.MinValue), def.ImmediateSize, def.ImmediateSignExtSize, argSizeBits)
					),
					(64, 64) => new TestArgValue(isSigned ?
						SignedImmToTestArgValue(long.MinValue, def.ImmediateSize, def.ImmediateSignExtSize, argSizeBits) :
						UnsignedImmToTestArgValue(unchecked((ulong)long.MinValue), def.ImmediateSize, def.ImmediateSignExtSize, argSizeBits)
					),
					_ => throw new InvalidOperationException(),
				};

			case OperandEncoding.ImpliedRegister:
				return new TestArgValue(RegToTestArgValue(def.Register));

			case OperandEncoding.RegImm:
			case OperandEncoding.RegOpCode:
			case OperandEncoding.RegModrmReg:
			case OperandEncoding.RegModrmRm:
			case OperandEncoding.RegVvvvv:
				return new TestArgValue(RegToTestArgValue(GetRegMemSizeInfo(def, index).reg));

			case OperandEncoding.RegMemModrmRm:
				var (reg, memSizeFnKind) = GetRegMemSizeInfo(def, index);
				if (argKind == ArgKind.Memory && def.OperandEncoding == OperandEncoding.RegMemModrmRm) {
					return new TestArgValue(
						MemToTestArgValue(toFnInfo[memSizeFnKind], Register.SI, Register.None, 1, 0),
						MemToTestArgValue(toFnInfo[memSizeFnKind], Register.ECX, Register.None, 1, 0),
						MemToTestArgValue(toFnInfo[memSizeFnKind], Register.RCX, Register.None, 1, 0)
					);
				}
				else
					return new TestArgValue(RegToTestArgValue(reg));

			case OperandEncoding.MemModrmRm:
				if (def.SibRequired) {
					return new TestArgValue(
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], Register.SI, Register.None, 1, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], Register.ECX, Register.EDX, 2, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], Register.RCX, Register.RDX, 4, 0)
					);
				}
				else if (def.MIB) {
					return new TestArgValue(
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], Register.SI, Register.None, 1, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], Register.ECX, Register.None, 1, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], Register.RCX, Register.None, 1, 0)
					);
				}
				else if (def.Vsib) {
					return new TestArgValue(
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], Register.SI, Register.None, 1, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], Register.EDX, def.Register + index + 2, 1, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], Register.RDX, def.Register + index + 2, 1, 0)
					);
				}
				else {
					return new TestArgValue(
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], Register.SI, Register.None, 1, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], Register.ECX, Register.None, 1, 0),
						MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], Register.RCX, Register.None, 1, 0)
					);
				}

			case OperandEncoding.MemOffset:
				return new TestArgValue(
					MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], 16, 0x6789),
					MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], 32, 0x6789ABCD),
					MemToTestArgValue(toFnInfo[MemorySizeFnKind.Ptr], 64, 0x6789ABCDEF012345)
				);

			case OperandEncoding.None:
			case OperandEncoding.FarBranch:
			case OperandEncoding.SegRBX:
			case OperandEncoding.SegRSI:
			case OperandEncoding.SegRDI:
			case OperandEncoding.ESRDI:
			default:
				throw new InvalidOperationException();
			}
		}

		static readonly Register[] r8Values = new Register[] { Register.DL, Register.BL, Register.AH, Register.CH, Register.DH };
		static readonly Register[] r16Values = new Register[] { Register.DX, Register.BX, Register.SP, Register.BP, Register.SI };
		static readonly Register[] r32Values = new Register[] { Register.EDX, Register.EBX, Register.ESP, Register.EBP, Register.ESI };
		static readonly Register[] r64Values = new Register[] { Register.RDX, Register.RBX, Register.RSP, Register.RBP, Register.RSI };
		static (Register reg, MemorySizeFnKind kind) GetRegMemSizeInfo(OpCodeOperandKindDef def, int index) =>
			def.Register switch {
				Register.AL => (r8Values[index], MemorySizeFnKind.BytePtr),
				Register.AX => (r16Values[index], MemorySizeFnKind.WordPtr),
				Register.EAX => (r32Values[index], MemorySizeFnKind.DwordPtr),
				Register.RAX => (r64Values[index], MemorySizeFnKind.QwordPtr),
				Register.MM0 => (Register.MM0 + index + 2, MemorySizeFnKind.QwordPtr),
				Register.XMM0 => (Register.XMM0 + index + 2, MemorySizeFnKind.XmmwordPtr),
				Register.YMM0 => (Register.YMM0 + index + 2, MemorySizeFnKind.YmmwordPtr),
				Register.ZMM0 => (Register.ZMM0 + index + 2, MemorySizeFnKind.ZmmwordPtr),
				Register.TMM0 => (Register.TMM0 + index + 2, MemorySizeFnKind.Ptr),
				Register.BND0 => (Register.BND0 + index + 2, MemorySizeFnKind.Ptr),
				Register.K0 => (Register.K0 + index + 2, MemorySizeFnKind.Ptr),
				Register.ES => (Register.DS, MemorySizeFnKind.Ptr),
				Register.CR0 => (Register.CR2, MemorySizeFnKind.Ptr),
				Register.DR0 => (Register.DR1, MemorySizeFnKind.Ptr),
				Register.TR0 => (Register.TR1, MemorySizeFnKind.Ptr),
				Register.ST0 => (Register.ST1, MemorySizeFnKind.Ptr),
				_ => throw new InvalidOperationException(),
			};

		protected static int GetArgBitness(int bitness, InstructionDef def) {
			foreach (var kindDef in def.OpKindDefs) {
				if (bitness == 16 && (kindDef.MIB || kindDef.MPX || kindDef.Vsib || kindDef.SibRequired))
					return 32;
				if (kindDef.OperandEncoding == OperandEncoding.RegModrmReg && kindDef.Memory) {
					return kindDef.Register switch {
						Register.AX => 16,
						Register.EAX => 32,
						Register.RAX => 64,
						_ => throw new InvalidOperationException(),
					};
				}
			}

			return bitness;
		}

		protected static int GetInvalidTestBitness(int bitness, OpCodeInfoGroup group) {
			// Force fake bitness support to allow to generate a throw for the last selector
			if (bitness == 64 && (group.Name == "bndcn" ||
								  group.Name == "bndmk" ||
								  group.Name == "bndcu" ||
								  group.Name == "bndcl")) {
				return 32;
			}
			return bitness;
		}

		protected sealed class TestArgValues {
			public readonly List<TestArgValue?> Args;
			public TestArgValues(int argCount) {
				Args = new(argCount);
				for (int i = 0; i < argCount; i++)
					Args.Add(null);
			}

			public TestArgValueBitness? GetArgValue(int bitness, int index) {
				if (Args[index] is TestArgValue argValue)
					return argValue.Get(bitness);
				else
					return null;
			}

			public TestArgValue? Set(int index, TestArgValue? arg) {
				if (index < 0)
					return null;
				var old = Args[index];
				Args[index] = arg;
				return old;
			}

			public void Restore(int index, TestArgValue? old) {
				if (index >= 0)
					Args[index] = old;
			}
		}

		protected sealed class TestArgValue {
			public readonly TestArgValueBitness Bitness16;
			public readonly TestArgValueBitness Bitness32;
			public readonly TestArgValueBitness Bitness64;

			public TestArgValue(TestArgValueBitness b) : this(b, b, b) { }
			public TestArgValue(TestArgValueBitness b16, TestArgValueBitness b32, TestArgValueBitness b64) {
				Bitness16 = b16;
				Bitness32 = b32;
				Bitness64 = b64;
			}

			public TestArgValueBitness Get(int bitness) =>
				bitness switch {
					16 => Bitness16,
					32 => Bitness32,
					64 => Bitness64,
					_ => throw new InvalidOperationException(),
				};
		}

		protected sealed class TestArgValueBitness {
			// String used when calling CodeAssembler methods, eg. `eax`, `byte_ptr(rcx+rdx*4)`, etc.
			public readonly string AsmStr;
			// String used when calling Instruction::with*() methods, eg. `Register::EAX`, etc.
			public readonly string WithStr;
			public TestArgValueBitness(string asmStr) : this(asmStr, asmStr) { }
			public TestArgValueBitness(string asmStr, string withStr) {
				AsmStr = asmStr;
				WithStr = withStr;
			}
		}

		protected readonly struct OpCodeNode {
			readonly object value;

			public OpCodeNode(InstructionDef def) => value = def;
			public OpCodeNode(OpCodeSelector selector) => value = selector;

			public bool IsEmpty => value is null;
			public InstructionDef? Def => value as InstructionDef;
			public OpCodeSelector? Selector => value as OpCodeSelector;
			public static implicit operator OpCodeNode(InstructionDef def) => new(def);
			public static implicit operator OpCodeNode(OpCodeSelector selector) => new(selector);
		}

		protected sealed class OpCodeSelector {
			public OpCodeSelector(OpCodeSelectorKind kind) {
				ArgIndex = -1;
				Kind = kind;
			}

			public OpCodeSelector(int argIndex, OpCodeSelectorKind kind) {
				ArgIndex = argIndex;
				Kind = kind;
			}

			public readonly int ArgIndex;
			public readonly OpCodeSelectorKind Kind;
			public OpCodeNode IfTrue;
			public OpCodeNode IfFalse;
			public bool IsConditionInlineable => IfTrue.Def is not null && IfFalse.Def is not null;
		}

		protected enum OpCodeSelectorKind {
			Invalid,

			Bitness64,
			Bitness32,
			Bitness16,

			ShortBranch,

			ImmediateInt,
			ImmediateByte,
			ImmediateByteEqual1,
			ImmediateByteSigned8To16,
			ImmediateByteSigned8To32,
			ImmediateByteSigned8To64,
			ImmediateByteWith2Bits,

			Vex,
			EvexBroadcastX,
			EvexBroadcastY,
			EvexBroadcastZ,

			MemoryIndex32Xmm,
			MemoryIndex32Ymm,
			MemoryIndex32Zmm,

			MemoryIndex64Xmm,
			MemoryIndex64Ymm,
			MemoryIndex64Zmm,

			RegisterCL,
			RegisterAL,
			RegisterAX,
			RegisterEAX,
			RegisterRAX,

			RegisterBND,

			RegisterES,
			RegisterCS,
			RegisterSS,
			RegisterDS,
			RegisterFS,
			RegisterGS,
			RegisterDX,

			Register8,
			Register16,
			Register32,
			Register64,

			RegisterK,

			RegisterST0,
			RegisterST,

			RegisterSegment,

			RegisterCR,
			RegisterDR,
			RegisterTR,

			RegisterMM,
			RegisterXMM,
			RegisterYMM,
			RegisterZMM,

			RegisterTMM,

			Memory,

			Memory8,
			Memory16,
			Memory32,
			Memory48,
			Memory64,
			Memory80,

			MemoryK,

			MemoryMM,
			MemoryXMM,
			MemoryYMM,
			MemoryZMM,

			MemOffs64_RAX,
			MemOffs64_EAX,
			MemOffs64_AX,
			MemOffs64_AL,
			MemOffs_RAX,
			MemOffs_EAX,
			MemOffs_AX,
			MemOffs_AL,
		}
	}
}
