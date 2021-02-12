// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Generator.Enums;
using Generator.Enums.Formatter;
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
		readonly EnumValue? codeInt3;
		readonly Dictionary<EnumValue, Code> toOrigCodeValue;
		int stackDepth;

		protected Code GetOrigCodeValue(EnumValue value) {
			if (value.DeclaringType.TypeId != TypeIds.Code)
				throw new InvalidOperationException();
			return toOrigCodeValue[value];
		}

		protected AssemblerSyntaxGenerator(GenTypes genTypes) {
			this.genTypes = genTypes;
			defs = genTypes.GetObject<InstructionDefs>(TypeIds.InstructionDefs).Defs;
			regDefs = genTypes.GetObject<RegisterDefs>(TypeIds.RegisterDefs).Defs;
			memDefs = genTypes.GetObject<MemorySizeDefs>(TypeIds.MemorySizeDefs).Defs;
			groups = new Dictionary<GroupKey, OpCodeInfoGroup>();
			groupsWithPseudo = new Dictionary<GroupKey, OpCodeInfoGroup>();
			codeInt3 = genTypes.GetKeptCodeValues(Code.Int3).FirstOrDefault();
			var origCode = genTypes.GetObject<EnumValue[]>(TypeIds.OrigCodeValues);
			toOrigCodeValue = new Dictionary<EnumValue, Code>(origCode.Length);
			for (int i = 0; i < origCode.Length; i++)
				toOrigCodeValue.Add(origCode[i], (Code)i);

			discardOpCodes = genTypes.GetKeptCodeValues(new[] {
				Code.INVALID,

				Code.Nopq,

				Code.Add_rm8_imm8_82,
				Code.Or_rm8_imm8_82,
				Code.Adc_rm8_imm8_82,
				Code.Sbb_rm8_imm8_82,
				Code.And_rm8_imm8_82,
				Code.Sub_rm8_imm8_82,
				Code.Xor_rm8_imm8_82,
				Code.Cmp_rm8_imm8_82,
				Code.Test_rm16_imm16_F7r1,
				Code.Test_rm32_imm32_F7r1,
				Code.Test_rm64_imm32_F7r1,
				Code.Test_rm8_imm8_F6r1,
				Code.Lfence_E9,
				Code.Lfence_EA,
				Code.Lfence_EB,
				Code.Lfence_EC,
				Code.Lfence_ED,
				Code.Lfence_EE,
				Code.Lfence_EF,
				Code.Mfence_F1,
				Code.Mfence_F2,
				Code.Mfence_F3,
				Code.Mfence_F4,
				Code.Mfence_F5,
				Code.Mfence_F6,
				Code.Mfence_F7,
				Code.Sfence_F9,
				Code.Sfence_FA,
				Code.Sfence_FB,
				Code.Sfence_FC,
				Code.Sfence_FD,
				Code.Sfence_FE,
				Code.Sfence_FF,

				Code.Cmpxchg486_rm8_r8,
				Code.Cmpxchg486_rm16_r16,
				Code.Cmpxchg486_rm32_r32,

				Code.Loadall286,
				Code.Loadallreset286,

				Code.Fstp_sti_DFD0,
				Code.Fstp_sti_DFD8,
				Code.Fxch_st0_sti_DDC8,
				Code.Fxch_st0_sti_DFC8,
				Code.Fcom_st0_sti_DCD0,
				Code.Fcomp_st0_sti_DED0,
				Code.Fcomp_st0_sti_DCD8,

				Code.VEX_Vmovss_xmm_xmm_xmm_0F11,
				Code.VEX_Vmovsd_xmm_xmm_xmm_0F11,
				Code.EVEX_Vmovss_xmm_k1z_xmm_xmm_0F11,
				Code.EVEX_Vmovsd_xmm_k1z_xmm_xmm_0F11,

				Code.DeclareByte,
				Code.DeclareWord,
				Code.DeclareDword,
				Code.DeclareQword,

				Code.Loopne_rel8_32_CX,
				Code.Loopne_rel8_16_ECX,
				Code.Loopne_rel8_64_ECX,
				Code.Loopne_rel8_16_RCX,
				Code.Loope_rel8_32_CX,
				Code.Loope_rel8_16_ECX,
				Code.Loope_rel8_64_ECX,
				Code.Loope_rel8_16_RCX,
				Code.Loop_rel8_32_CX,
				Code.Loop_rel8_16_ECX,
				Code.Loop_rel8_64_ECX,
				Code.Loop_rel8_16_RCX,
				Code.Jcxz_rel8_32,
				Code.Jecxz_rel8_16,
				Code.Jecxz_rel8_64,
				Code.Jrcxz_rel8_16,

				Code.Popw_CS,

				Code.Prefetchreserved3_m8,
				Code.Prefetchreserved4_m8,
				Code.Prefetchreserved5_m8,
				Code.Prefetchreserved6_m8,
				Code.Prefetchreserved7_m8,

				Code.Vmgexit_F2,

				// The following are implemented manually
				Code.Call_ptr1616,
				Code.Call_ptr1632,
				Code.Xlat_m8,
				Code.Jmp_ptr1616,
				Code.Jmp_ptr1632,

				// Cyrix FPU instructions
				Code.Cyrix_D9D7,
				Code.Cyrix_D9E2,
				Code.Ftstp,
				Code.Cyrix_D9E7,
				Code.Frint2,
				Code.Frichop,
				Code.Cyrix_DED8,
				Code.Cyrix_DEDA,
				Code.Cyrix_DEDC,
				Code.Cyrix_DEDD,
				Code.Cyrix_DEDE,
				Code.Frinear,
			}).ToHashSet();
			var removed = genTypes.GetObject<HashSet<EnumValue>>(TypeIds.RemovedCodeValues);
			mapOpCodeToNewName = new[] {
				(Code.Iretw, "iret"),
				(Code.Iretd, "iretd"),
				(Code.Iretq, "iretq"),
				(Code.Pushaw, "pusha"),
				(Code.Pushad, "pushad"),
				(Code.Popaw, "popa"),
				(Code.Popad, "popad"),
				(Code.Pushfw, "pushf"),
				(Code.Pushfd, "pushfd"),
				(Code.Pushfq, "pushfq"),
				(Code.Popfw, "popf"),
				(Code.Popfd, "popfd"),
				(Code.Popfq, "popfq"),
				(Code.Sysexitd, "sysexit"),
				(Code.Sysexitq, "sysexitq"),
				(Code.Sysretd, "sysret"),
				(Code.Sysretq, "sysretq"),
				(Code.Getsecq, "getsecq"),
				(Code.Reservednop_rm16_r16_0F0D, "reservednop_0f0d"),
				(Code.Reservednop_rm32_r32_0F0D, "reservednop_0f0d"),
				(Code.Reservednop_rm64_r64_0F0D, "reservednop_0f0d"),
				(Code.Reservednop_rm16_r16_0F18, "reservednop_0f18"),
				(Code.Reservednop_rm32_r32_0F18, "reservednop_0f18"),
				(Code.Reservednop_rm64_r64_0F18, "reservednop_0f18"),
				(Code.Reservednop_rm16_r16_0F19, "reservednop_0f19"),
				(Code.Reservednop_rm32_r32_0F19, "reservednop_0f19"),
				(Code.Reservednop_rm64_r64_0F19, "reservednop_0f19"),
				(Code.Reservednop_rm16_r16_0F1A, "reservednop_0f1a"),
				(Code.Reservednop_rm32_r32_0F1A, "reservednop_0f1a"),
				(Code.Reservednop_rm64_r64_0F1A, "reservednop_0f1a"),
				(Code.Reservednop_rm16_r16_0F1B, "reservednop_0f1b"),
				(Code.Reservednop_rm32_r32_0F1B, "reservednop_0f1b"),
				(Code.Reservednop_rm64_r64_0F1B, "reservednop_0f1b"),
				(Code.Reservednop_rm16_r16_0F1C, "reservednop_0f1c"),
				(Code.Reservednop_rm32_r32_0F1C, "reservednop_0f1c"),
				(Code.Reservednop_rm64_r64_0F1C, "reservednop_0f1c"),
				(Code.Reservednop_rm16_r16_0F1D, "reservednop_0f1d"),
				(Code.Reservednop_rm32_r32_0F1D, "reservednop_0f1d"),
				(Code.Reservednop_rm64_r64_0F1D, "reservednop_0f1d"),
				(Code.Reservednop_rm16_r16_0F1E, "reservednop_0f1e"),
				(Code.Reservednop_rm32_r32_0F1E, "reservednop_0f1e"),
				(Code.Reservednop_rm64_r64_0F1E, "reservednop_0f1e"),
				(Code.Reservednop_rm16_r16_0F1F, "reservednop_0f1f"),
				(Code.Reservednop_rm32_r32_0F1F, "reservednop_0f1f"),
				(Code.Reservednop_rm64_r64_0F1F, "reservednop_0f1f"),
				(Code.Smint_0F7E, "smint_0f7e"),
				(Code.Pmulhrw_mm_mmm64, "pmulhrw_cyrix"),
			}.Select(a => (code: origCode[(int)a.Item1], name: a.Item2)).Where(a => !removed.Contains(a.code)).ToDictionary(a => a.code, a => a.name);
		}

		protected const InstructionDefFlags1 BitnessMaskFlags = InstructionDefFlags1.Bit64 | InstructionDefFlags1.Bit32 | InstructionDefFlags1.Bit16;

		protected abstract void GenerateRegisters(EnumType registers);
		protected abstract void Generate(Dictionary<GroupKey, OpCodeInfoGroup> map, OpCodeInfoGroup[] opCodes);

		public void Generate() {
			GenerateRegisters(genTypes[TypeIds.Register]);
			GenerateOpCodes();
		}

		void GenerateOpCodes() {
			foreach (var def in defs) {
				var codeValue = def.Code;
				if (discardOpCodes.Contains(codeValue)) continue;

				string memoName = def.Mnemonic.RawName;
				var name = mapOpCodeToNewName.TryGetValue(codeValue, out var nameOpt) ? nameOpt : memoName.ToLowerInvariant();
				if (codeValue == codeInt3) name = "int3";

				var signature = new Signature();
				var regOnlySignature = new Signature();

				var pseudoOpsKind = def.PseudoOp;
				var opCodeArgFlags = OpCodeArgFlags.Default;

				if (def.Encoding == EncodingKind.VEX) opCodeArgFlags |= OpCodeArgFlags.HasVex;
				if (def.Encoding == EncodingKind.EVEX) opCodeArgFlags |= OpCodeArgFlags.HasEvex;

				if ((def.Flags1 & InstructionDefFlags1.ZeroingMasking) != 0) opCodeArgFlags |= OpCodeArgFlags.HasZeroingMask;
				if ((def.Flags1 & InstructionDefFlags1.OpMaskRegister) != 0) opCodeArgFlags |= OpCodeArgFlags.HasKMask;
				if ((def.Flags1 & InstructionDefFlags1.Broadcast) != 0) opCodeArgFlags |= OpCodeArgFlags.HasBroadcast;
				if ((def.Flags1 & InstructionDefFlags1.SuppressAllExceptions) != 0) opCodeArgFlags |= OpCodeArgFlags.SuppressAllExceptions;
				if ((def.Flags1 & InstructionDefFlags1.RoundingControl) != 0) opCodeArgFlags |= OpCodeArgFlags.RoundingControl;

				var argSizes = new List<int>();

				// For certain instruction, we need to discard them
				int numberLeadingArgToDiscard = 0;
				var numberLeadingArgToDiscardOpt = GetSpecialArgEncodingInstruction(def);
				if (numberLeadingArgToDiscardOpt.HasValue) {
					numberLeadingArgToDiscard = numberLeadingArgToDiscardOpt.Value;
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
							opCodeArgFlags |= OpCodeArgFlags.HasBranchShort;
							opCodeArgFlags |= OpCodeArgFlags.HasLabel;
							break;
						case 16:
						case 32:
							argKind = ArgKind.Label;
							opCodeArgFlags |= OpCodeArgFlags.HasBranchNear;
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
								opCodeArgFlags |= OpCodeArgFlags.HasImmediateByteSignedExtended;
								break;
							case (8, 64):
								opCodeArgFlags |= OpCodeArgFlags.HasImmediateByteSignedExtended | OpCodeArgFlags.UnsignedUIntNotSupported;
								break;
							case (32, 64):
								opCodeArgFlags |= OpCodeArgFlags.UnsignedUIntNotSupported;
								break;
							case (16, _):
							case (32, 32):
							case (64, _):
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
					if (!hasR64M16)
						AddOpCodeToGroup(name, memoName, signature, def, opCodeArgFlags, pseudoOpsKind, numberLeadingArgToDiscard, argSizes, false);
				}

				if (signature != regOnlySignature) {
					opCodeArgFlags &= ~OpCodeArgFlags.HasBroadcast;
					AddOpCodeToGroup(name, memoName, regOnlySignature, def, opCodeArgFlags | OpCodeArgFlags.HasRegisterMemoryMappedToRegister, pseudoOpsKind, numberLeadingArgToDiscard, argSizes, false);
				}
			}

			CreatePseudoInstructions();

			var orderedGroups = groups.OrderBy(x => x.Key).Select(x => x.Value).ToList();
			var signatures = new HashSet<Signature>();
			var opcodes = new List<InstructionDef>();
			for (var i = 0; i < orderedGroups.Count; i++) {
				var @group = orderedGroups[i];

				if (@group.HasRegisterMemoryMappedToRegister) {
					var inputOpCodes = @group.Items;
					opcodes.Clear();
					signatures.Clear();
					// First-pass to select only register versions
					FilterOpCodesRegister(@group, inputOpCodes, opcodes, signatures, false);

					// Second-pass to populate with RM versions
					FilterOpCodesRegister(@group, inputOpCodes, opcodes, signatures, true);

					inputOpCodes.Clear();
					inputOpCodes.AddRange(opcodes);
				}

				// Update the selector graph for this group of opcodes
				if (@group.HasSpecialInstructionEncoding)
					@group.RootOpCodeNode = new OpCodeNode(@group.Items[0]);
				else
					@group.RootOpCodeNode = BuildSelectorGraph(@group);
			}

			Generate(groups, orderedGroups.ToArray());
		}

		ArgKind GetArgKind(OpCodeOperandKindDef def, bool isRegMem) {
			var (reg, rm) = (RegisterKind)regDefs[(int)def.Register].RegisterKind.Value switch {
				RegisterKind.GPR8 => (ArgKind.Register8, ArgKind.Register8Memory),
				RegisterKind.GPR16 => (ArgKind.Register16, ArgKind.Register16Memory),
				RegisterKind.GPR32 => (ArgKind.Register32, ArgKind.Register32Memory),
				RegisterKind.GPR64 => (ArgKind.Register64, ArgKind.Register64Memory),
				RegisterKind.IP => (ArgKind.Unknown, ArgKind.Unknown),
				RegisterKind.Segment => (ArgKind.RegisterSegment, ArgKind.Unknown),
				RegisterKind.ST => (ArgKind.RegisterST, ArgKind.Unknown),
				RegisterKind.CR => (ArgKind.RegisterCR, ArgKind.Unknown),
				RegisterKind.DR => (ArgKind.RegisterDR, ArgKind.Unknown),
				RegisterKind.TR => (ArgKind.RegisterTR, ArgKind.Unknown),
				RegisterKind.BND => (ArgKind.RegisterBND, ArgKind.RegisterBNDMemory),
				RegisterKind.K => (ArgKind.RegisterK, ArgKind.RegisterKMemory),
				RegisterKind.MM => (ArgKind.RegisterMM, ArgKind.RegisterMMMemory),
				RegisterKind.XMM => (ArgKind.RegisterXMM, ArgKind.RegisterXMMMemory),
				RegisterKind.YMM => (ArgKind.RegisterYMM, ArgKind.RegisterYMMMemory),
				RegisterKind.ZMM => (ArgKind.RegisterZMM, ArgKind.RegisterZMMMemory),
				RegisterKind.TMM => (ArgKind.RegisterTMM, ArgKind.Unknown),
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
				ArgKind.RegisterBNDMemory => memory ? ArgKind.Memory : ArgKind.RegisterBND,
				ArgKind.RegisterMMMemory => memory ? ArgKind.Memory : ArgKind.RegisterMM,
				ArgKind.RegisterXMMMemory => memory ? ArgKind.Memory : ArgKind.RegisterXMM,
				ArgKind.RegisterYMMMemory => memory ? ArgKind.Memory : ArgKind.RegisterYMM,
				ArgKind.RegisterZMMMemory => memory ? ArgKind.Memory : ArgKind.RegisterZMM,
				_ => kind,
			};

		int? GetSpecialArgEncodingInstruction(InstructionDef def) =>
			GetOrigCodeValue(def.Code) switch {
				Code.Outsb_DX_m8 or Code.Outsw_DX_m16 or Code.Outsd_DX_m32 or Code.Lodsb_AL_m8 or Code.Lodsw_AX_m16 or Code.Lodsd_EAX_m32 or
				Code.Lodsq_RAX_m64 or Code.Scasb_AL_m8 or Code.Scasw_AX_m16 or Code.Scasd_EAX_m32 or Code.Scasq_RAX_m64 or Code.Insb_m8_DX or
				Code.Insw_m16_DX or Code.Insd_m32_DX or Code.Stosb_m8_AL or Code.Stosw_m16_AX or Code.Stosd_m32_EAX or Code.Stosq_m64_RAX or
				Code.Cmpsb_m8_m8 or Code.Cmpsw_m16_m16 or Code.Cmpsd_m32_m32 or Code.Cmpsq_m64_m64 or Code.Movsb_m8_m8 or Code.Movsw_m16_m16 or
				Code.Movsd_m32_m32 or Code.Movsq_m64_m64 => 2,
				Code.Maskmovq_rDI_mm_mm or Code.Maskmovdqu_rDI_xmm_xmm or Code.VEX_Vmaskmovdqu_rDI_xmm_xmm => 1,
				Code.Xbegin_rel16 or Code.Xbegin_rel32 => 0,
				_ => null,
			};

		OpCodeNode BuildSelectorGraph(OpCodeInfoGroup group) {
			// In case of one opcode, we don't need to perform any disambiguation
			var opcodes = group.Items;
			// Sort opcodes by decreasing size
			opcodes.Sort(group.OrderOpCodesPerOpKindPriority);
			return BuildSelectorGraph(group, group.Signature, group.Flags, opcodes);
		}

		OpCodeNode BuildSelectorGraph(OpCodeInfoGroup group, Signature signature, OpCodeArgFlags argFlags, List<InstructionDef> opcodes) {
			if (opcodes.Count == 0) return default;

			if (opcodes.Count == 1)
				return new OpCodeNode(opcodes[0]);

			Debug.Assert(stackDepth++ < 16, "Potential StackOverflow");
			try {
				OrderedSelectorList selectors;

				if ((argFlags & OpCodeArgFlags.HasImmediateByteEqual1) != 0) {
					// handle imm8 == 1 
					var opcodesWithImmediateByteEqual1 = new List<InstructionDef>();
					var opcodesOthers = new List<InstructionDef>();
					var indices = CollectByOperandKindPredicate(opcodes, IsImmediateByteEqual1, opcodesWithImmediateByteEqual1, opcodesOthers);
					Debug.Assert(indices.Count == 1);
					var newFlags = argFlags ^ OpCodeArgFlags.HasImmediateByteEqual1;
					return new OpCodeSelector(indices[0], OpCodeSelectorKind.ImmediateByteEqual1) {
						IfTrue = BuildSelectorGraph(group, group.Signature, newFlags, opcodesWithImmediateByteEqual1),
						IfFalse = BuildSelectorGraph(group, group.Signature, newFlags, opcodesOthers)
					};
				}
				else if (group.IsBranch) {
					var branchShort = new List<InstructionDef>();
					var branchFar = new List<InstructionDef>();
					CollectByOperandKindPredicate(opcodes, IsBranchShort, branchShort, branchFar);
					if (branchShort.Count > 0 && branchFar.Count > 0) {
						var newFlags = argFlags & ~(OpCodeArgFlags.HasBranchShort | OpCodeArgFlags.HasBranchNear);
						return new OpCodeSelector(OpCodeSelectorKind.BranchShort) {
							IfTrue = BuildSelectorGraph(group, group.Signature, newFlags, branchShort),
							IfFalse = BuildSelectorGraph(group, group.Signature, newFlags, branchFar)
						};
					}
				}

				// Handle case of moffs
				if (group.Name == "mov") {
					var opCodesRAXMOffs = new List<InstructionDef>();
					var newOpCodes = new List<InstructionDef>();

					var memOffs64Selector = OpCodeSelectorKind.Invalid;
					var memOffsSelector = OpCodeSelectorKind.Invalid;

					int argIndex = 0;
					for (var i = 0; i < opcodes.Count; i++) {
						var opCodeInfo = opcodes[i];
						// Special case, we want to disambiguate on the register and moffs
						switch (GetOrigCodeValue(opCodeInfo.Code)) {
						case Code.Mov_moffs64_RAX:
							memOffs64Selector = OpCodeSelectorKind.MemOffs64_RAX;
							memOffsSelector = OpCodeSelectorKind.MemOffs_RAX;
							argIndex = 0;
							opCodesRAXMOffs.Add(opCodeInfo);
							break;
						case Code.Mov_moffs32_EAX:
							memOffs64Selector = OpCodeSelectorKind.MemOffs64_EAX;
							memOffsSelector = OpCodeSelectorKind.MemOffs_EAX;
							argIndex = 0;
							opCodesRAXMOffs.Add(opCodeInfo);
							break;
						case Code.Mov_moffs16_AX:
							memOffs64Selector = OpCodeSelectorKind.MemOffs64_AX;
							memOffsSelector = OpCodeSelectorKind.MemOffs_AX;
							argIndex = 0;
							opCodesRAXMOffs.Add(opCodeInfo);
							break;
						case Code.Mov_moffs8_AL:
							memOffs64Selector = OpCodeSelectorKind.MemOffs64_AL;
							memOffsSelector = OpCodeSelectorKind.MemOffs_AL;
							argIndex = 0;
							opCodesRAXMOffs.Add(opCodeInfo);
							break;
						case Code.Mov_RAX_moffs64:
							memOffs64Selector = OpCodeSelectorKind.MemOffs64_RAX;
							memOffsSelector = OpCodeSelectorKind.MemOffs_RAX;
							argIndex = 1;
							opCodesRAXMOffs.Add(opCodeInfo);
							break;
						case Code.Mov_EAX_moffs32:
							memOffs64Selector = OpCodeSelectorKind.MemOffs64_EAX;
							memOffsSelector = OpCodeSelectorKind.MemOffs_EAX;
							argIndex = 1;
							opCodesRAXMOffs.Add(opCodeInfo);
							break;
						case Code.Mov_AX_moffs16:
							memOffs64Selector = OpCodeSelectorKind.MemOffs64_AX;
							memOffsSelector = OpCodeSelectorKind.MemOffs_AX;
							argIndex = 1;
							opCodesRAXMOffs.Add(opCodeInfo);
							break;
						case Code.Mov_AL_moffs8:
							memOffs64Selector = OpCodeSelectorKind.MemOffs64_AL;
							memOffsSelector = OpCodeSelectorKind.MemOffs_AL;
							argIndex = 1;
							opCodesRAXMOffs.Add(opCodeInfo);
							break;
						default:
							newOpCodes.Add(opCodeInfo);
							break;
						}
					}

					if (opCodesRAXMOffs.Count > 0) {
						return new OpCodeSelector(argIndex, memOffs64Selector) {
							IfTrue = BuildSelectorGraph(group, group.Signature, argFlags, opCodesRAXMOffs),
							IfFalse = new OpCodeSelector(argIndex, memOffsSelector) {
								IfTrue = BuildSelectorGraph(group, group.Signature, argFlags, opCodesRAXMOffs),
								IfFalse = BuildSelectorGraph(group, group.Signature, argFlags, newOpCodes)
							}
						};
					}
				}

				// Handle disambiguation for auto-broadcast select
				if ((argFlags & OpCodeArgFlags.HasBroadcast) != 0) {
					int memoryIndex = GetBroadcastMemory(argFlags, opcodes, signature, out var broadcastSelectorKind, out var evexBroadcastDef);
					if (memoryIndex >= 0) {
						Debug.Assert(evexBroadcastDef is not null);
						return new OpCodeSelector(memoryIndex, broadcastSelectorKind) {
							IfTrue = evexBroadcastDef,
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
					bool isPushImm = group.Name == "push" && group.Signature.ArgCount == 1 && IsArgKindImmediate(group.Signature.GetArgKind(0));
					if (isPushImm && opcodes.Count > 2) {
						// bitness
						selectors = BuildSelectorsPerBitness(@group, argFlags, opcodes);
						return BuildSelectorGraphFromSelectors(group, group.Signature, argFlags, selectors);
					}

					if ((argFlags & OpCodeArgFlags.HasImmediateByteSignedExtended) != 0) {
						// handle imm >= sbyte.MinValue && imm <= byte.MaxValue 
						var opcodesWithImmediateByteSigned = new List<InstructionDef>();
						var opcodesOthers = new List<InstructionDef>();
						var indices = CollectByOperandKindPredicate(opcodes, IsImmediateByteSigned, opcodesWithImmediateByteSigned, opcodesOthers);

						var selectorKind = OpCodeSelectorKind.ImmediateByteSigned8;
						int opSize;
						if (isPushImm)
							opSize = GetImmediateSizeInBits(opcodes[0]);
						else
							opSize = GetMemorySizeInBits(memDefs, defs, opcodes[0]);
						if (opSize > 1) {
							switch (opSize) {
							case 32: selectorKind = OpCodeSelectorKind.ImmediateByteSigned8To32; break;
							case 16: selectorKind = OpCodeSelectorKind.ImmediateByteSigned8To16; break;
							default: break;
							}
						}

						Debug.Assert(indices.Count == 1);
						var newFlags = argFlags ^ OpCodeArgFlags.HasImmediateByteSignedExtended;
						return new OpCodeSelector(indices[0], selectorKind) {
							IfTrue = BuildSelectorGraph(group, group.Signature, newFlags, opcodesWithImmediateByteSigned),
							IfFalse = BuildSelectorGraph(group, group.Signature, newFlags, opcodesOthers)
						};
					}

					if ((argFlags & (OpCodeArgFlags.HasVex | OpCodeArgFlags.HasEvex)) == (OpCodeArgFlags.HasVex | OpCodeArgFlags.HasEvex)) {
						var vex = opcodes.Where(x => x.Encoding == EncodingKind.VEX).ToList();
						var evex = opcodes.Where(x => x.Encoding == EncodingKind.EVEX).ToList();

						return new OpCodeSelector(OpCodeSelectorKind.Vex) {
							IfTrue = BuildSelectorGraph(group, signature, argFlags & ~(OpCodeArgFlags.HasVex | OpCodeArgFlags.HasEvex), vex),
							IfFalse = BuildSelectorGraph(group, signature, argFlags & ~(OpCodeArgFlags.HasVex | OpCodeArgFlags.HasEvex), evex),
						};
					}

					// bitness
					selectors = BuildSelectorsPerBitness(@group, argFlags, opcodes);
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
			int selectorIndex = 0;
			foreach (var (kind, list) in selectors) {
				OpCodeNode node;
				OpCodeSelector? newSelector = null;

				switch (kind) {
				case OpCodeSelectorKind.Bitness32:
				case OpCodeSelectorKind.Bitness16:
					// Bitness32/Bitness16 can be last without a condition
					if (list.Count == 1 && selectorIndex + 1 == selectors.Count)
						node = new OpCodeNode(list[0]);
					else
						goto default;
					break;
				case OpCodeSelectorKind.Register8:
				case OpCodeSelectorKind.Register16:
				case OpCodeSelectorKind.Register32:
				case OpCodeSelectorKind.Register64:
				case OpCodeSelectorKind.RegisterST:
					if (list.Count == 1 || selectorIndex + 1 == selectors.Count)
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

				previousSelector = newSelector;
				selectorIndex++;
			}
			return rootNode;
		}

		static OrderedSelectorList BuildSelectorsPerBitness(OpCodeInfoGroup @group, OpCodeArgFlags argFlags, List<InstructionDef> opcodes) {
			var selectors = new OrderedSelectorList();
			foreach (var def in opcodes) {
				if (def.Encoding == EncodingKind.Legacy) {
					int bitness = GetBitness(def);
					var selectorKind = bitness switch {
						16 => OpCodeSelectorKind.Bitness16,
						32 => OpCodeSelectorKind.Bitness32,
						_ => OpCodeSelectorKind.Bitness64,
					};
					selectors.Add(selectorKind, def);
				}
				else
					throw new InvalidOperationException($"Unable to detect bitness for opcode {def.Code.RawName} for group {@group.Name} / {argFlags}");
			}

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

		static int GetBroadcastMemory(OpCodeArgFlags argFlags, List<InstructionDef> opcodes, Signature signature, out OpCodeSelectorKind selectorKind, out InstructionDef? broadcastDef) {
			broadcastDef = null;
			selectorKind = OpCodeSelectorKind.Invalid;
			int memoryIndex = -1;
			if ((argFlags & OpCodeArgFlags.HasBroadcast) != 0) {
				for (int i = 0; i < signature.ArgCount; i++) {
					if (signature.GetArgKind(i) == ArgKind.Memory) {
						memoryIndex = i;
						var evex = @opcodes.First(x => x.Encoding == EncodingKind.EVEX);
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
				Debug.Assert(memoryIndex >= 0);
			}

			return memoryIndex;
		}

		bool ShouldDiscardDuplicatedOpCode(Signature signature, InstructionDef def) {
			bool testDiscard = false;
			for (int i = 0; i < signature.ArgCount; i++) {
				var kind = signature.GetArgKind(i);
				if (kind == ArgKind.Memory) {
					testDiscard = true;
					break;
				}
			}

			if (testDiscard) {
				switch (GetOrigCodeValue(def.Code)) {
				case Code.Pextrb_r64m8_xmm_imm8:             // => Pextrb_r32m8_xmm_imm8
				case Code.Extractps_r64m32_xmm_imm8:         // => Extractps_rm32_xmm_imm8	
				case Code.Pinsrb_xmm_r64m8_imm8:             // => Pinsrb_xmm_r32m8_imm8
				case Code.Movq_rm64_xmm:                     // => Movq_xmmm64_xmm
				case Code.Movq_rm64_mm:                      // => Movq_mmm64_mm
				case Code.Movq_xmm_rm64:                     // => Movq_xmm_rm64
				case Code.Movq_mm_rm64:                      // => Movq_mm_rm64					
				case Code.EVEX_Vextractps_r64m32_xmm_imm8:   // => EVEX_Vextractps_rm32_xmm_imm8
				case Code.EVEX_Vmovq_rm64_xmm:               // => EVEX_Vmovq_xmmm64_xmm
				case Code.EVEX_Vmovq_xmm_rm64:               // => EVEX_Vmovq_xmm_xmmm64
				case Code.EVEX_Vpextrb_r64m8_xmm_imm8:       // => EVEX_Vpextrb_r32m8_xmm_imm8
				case Code.EVEX_Vpextrw_r64m16_xmm_imm8:      // => EVEX_Vpextrw_r32m16_xmm_imm8
				case Code.EVEX_Vpinsrb_xmm_xmm_r64m8_imm8:   // => EVEX_Vpinsrb_xmm_xmm_r32m8_imm8
				case Code.EVEX_Vpinsrw_xmm_xmm_r64m16_imm8:  // => EVEX_Vpinsrw_xmm_xmm_r32m16_imm8
				case Code.VEX_Vextractps_r64m32_xmm_imm8:    // => VEX_Vextractps_rm32_xmm_imm8
				case Code.VEX_Vmovq_rm64_xmm:                // => VEX_Vmovq_xmmm64_xmm
				case Code.VEX_Vmovq_xmm_rm64:                // => VEX_Vmovq_xmm_xmmm64
				case Code.VEX_Vpextrb_r64m8_xmm_imm8:        // => VEX_Vpextrb_r32m8_xmm_imm8
				case Code.VEX_Vpextrw_r64m16_xmm_imm8:       // => VEX_Vpextrw_r32m16_xmm_imm8
				case Code.VEX_Vpinsrb_xmm_xmm_r64m8_imm8:    // => VEX_Vpinsrb_xmm_xmm_r32m8_imm8
				case Code.VEX_Vpinsrw_xmm_xmm_r64m16_imm8:   // => VEX_Vpinsrw_xmm_xmm_r32m16_imm8
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

		static List<int> CollectByOperandKindPredicate(List<InstructionDef> defs, Func<OpCodeOperandKindDef, bool?> predicate, List<InstructionDef> opcodesMatchingPredicate, List<InstructionDef> opcodesNotMatchingPredicate) {
			var argIndices = new List<int>();
			foreach (var def in defs) {
				var selected = opcodesNotMatchingPredicate;
				for (int i = 0; i < def.OpKindDefs.Length; i++) {
					var argOpKind = def.OpKindDefs[i];
					var result = predicate(argOpKind);
					if (result.HasValue) {
						if (result.Value) {
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

		static bool? IsBranchShort(OpCodeOperandKindDef def) =>
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

		OrderedSelectorList BuildSelectorsByRegisterOrMemory(Signature signature, OpCodeArgFlags argFlags, List<InstructionDef> opcodes, bool isRegister) {
			List<OrderedSelectorList>? selectorsList = null;
			for (int argIndex = 0; argIndex < signature.ArgCount; argIndex++) {
				var argKind = signature.GetArgKind(argIndex);
				if (isRegister && !IsRegister(argKind) || !isRegister && (argKind != ArgKind.Memory))
					continue;

				var selectors = new OrderedSelectorList() { ArgIndex = argIndex };
				foreach (var def in opcodes) {
					var argOpKind = def.OpKindDefs[argIndex];
					var conditionKind = GetSelectorKindForRegisterOrMemory(def, argOpKind, (argFlags & OpCodeArgFlags.HasRegisterMemoryMappedToRegister) != 0);
					selectors.Add(conditionKind, def);
				}

				// If we have already found the best selector, we can return immediately
				if (selectors.Count == opcodes.Count)
					return selectors;

				selectorsList ??= new List<OrderedSelectorList>();
				selectorsList.Add(selectors);
			}

			if (selectorsList is null) return new OrderedSelectorList();

			// Select the largest selector
			return selectorsList.First(x => x.Count == selectorsList.Max(x => x.Count));
		}

		static bool IsRegister(ArgKind kind) =>
			kind switch {
				ArgKind.Register8 or ArgKind.Register16 or ArgKind.Register32 or ArgKind.Register64 or ArgKind.RegisterK or
				ArgKind.RegisterST or ArgKind.RegisterSegment or ArgKind.RegisterBND or ArgKind.RegisterMM or ArgKind.RegisterXMM or
				ArgKind.RegisterYMM or ArgKind.RegisterZMM or ArgKind.RegisterCR or ArgKind.RegisterDR or ArgKind.RegisterTR or
				ArgKind.RegisterTMM => true,
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
					80 => 05,
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
			Default = 0,
			HasImmediateByteEqual1 = 1,
			HasImmediateByteLessThanBits = 1 << 1,
			HasImmediateByteSignedExtended = 1 << 2,
			HasLabel = 1 << 3,
			HasBranchShort = 1 << 4,
			HasBranchNear = 1 << 5,
			HasVex = 1 << 6,
			HasEvex = 1 << 7,
			HasRegisterMemoryMappedToRegister = 1 << 8,
			HasSpecialInstructionEncoding = 1 << 9,
			HasZeroingMask = 1 << 10,
			HasKMask = 1 << 11,
			HasBroadcast = 1 << 12,
			Pseudo = 1 << 13,
			SuppressAllExceptions = 1 << 14,
			RoundingControl = 1 << 15,
			HasAmbiguousBroadcast = 1 << 16,
			IsBroadcastXYZ = 1 << 17,
			HasLabelUlong = 1 << 18,
			HasImmediateByte = 1 << 19,
			UnsignedUIntNotSupported = 1 << 20,
			HasImmediateUnsigned = 1 << 21,
		}

		void FilterOpCodesRegister(OpCodeInfoGroup @group, List<InstructionDef> inputOpCodes, List<InstructionDef> opcodes, HashSet<Signature> signatures, bool allowMemory) {
			var bitnessFlags = InstructionDefFlags1.None;
			var vexOrEvexFlags = OpCodeArgFlags.Default;

			foreach (var code in inputOpCodes) {
				var registerSignature = new Signature();
				bool isValid = true;
				for (int i = 0; i < code.OpKindDefs.Length; i++) {
					var argKind = GetFilterRegisterKindFromOpKind(code.OpKindDefs[i], GetMemorySizeInBits(memDefs, defs, code), allowMemory);
					if (argKind == ArgKind.Unknown) {
						isValid = false;
						break;
					}

					registerSignature.AddArgKind(argKind);
				}

				var codeBitnessFlags = code.Flags1 & BitnessMaskFlags;
				var codeEvexFlags = code.Encoding == EncodingKind.VEX ? OpCodeArgFlags.HasVex : code.Encoding == EncodingKind.EVEX ? OpCodeArgFlags.HasEvex : OpCodeArgFlags.Default;

				if (isValid && 
					(signatures.Add(registerSignature) ||
					((bitnessFlags & codeBitnessFlags) != codeBitnessFlags) ||
					(codeEvexFlags != OpCodeArgFlags.Default && (vexOrEvexFlags & codeEvexFlags) == 0) ||
					(group.Flags & (OpCodeArgFlags.RoundingControl | OpCodeArgFlags.SuppressAllExceptions)) != 0)) {
					bitnessFlags |= codeBitnessFlags;
					vexOrEvexFlags |= codeEvexFlags;
					if (!opcodes.Contains(code))
						opcodes.Add(code);
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
				if (def.ImpliedConst != 1)
					throw new InvalidOperationException();
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
					Register.ST0 => ArgKind.RegisterST,
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
					Register.ST0 => ArgKind.RegisterST,
					Register.ES => ArgKind.RegisterSegment,
					Register.BND0 => ArgKind.RegisterBND,
					Register.CR0 => ArgKind.RegisterCR,
					Register.DR0 => ArgKind.RegisterTR,
					Register.TR0 => ArgKind.RegisterDR,
					Register.K0 => ArgKind.RegisterK,
					Register.MM0 => ArgKind.RegisterMM,
					Register.XMM0 => ArgKind.RegisterXMM,
					Register.YMM0 => ArgKind.RegisterYMM,
					Register.ZMM0 => ArgKind.RegisterZMM,
					Register.TMM0 => ArgKind.RegisterTMM,
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

		protected OpCodeSelectorKind GetSelectorKindForRegisterOrMemory(InstructionDef def, OpCodeOperandKindDef opKindDef, bool returnMemoryAsRegister) {
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

		OpCodeInfoGroup AddOpCodeToGroup(string name, string memoName, Signature signature, InstructionDef def, OpCodeArgFlags opCodeArgFlags, PseudoOpsKind? pseudoOpsKind, int numberLeadingArgToDiscard, List<int> argSizes, bool isOtherImmediate) {
			var key = new GroupKey(name, signature);
			if (!groups.TryGetValue(key, out var group)) {
				group = new OpCodeInfoGroup(memDefs, defs, name, signature);
				group.MemoName = memoName;
				groups.Add(key, group);
			}

			if (!group.Items.Contains(def))
				group.Items.Add(def);
			group.Flags |= opCodeArgFlags;
			group.AllOpCodeFlags |= def.Flags1;

			// Handle pseudo ops
			if (group.RootPseudoOpsKind is not null) {
				Debug.Assert(pseudoOpsKind is not null);
				Debug.Assert(group.RootPseudoOpsKind.Value == pseudoOpsKind.Value);
				Debug.Assert(groupsWithPseudo.ContainsKey(key));
			}
			else {
				group.RootPseudoOpsKind = pseudoOpsKind;
				if (pseudoOpsKind.HasValue) {
					if (!groupsWithPseudo.ContainsKey(key))
						groupsWithPseudo.Add(key, group);
				}
			}

			group.NumberOfLeadingArgToDiscard = numberLeadingArgToDiscard;
			group.UpdateMaxArgSizes(argSizes);

			// Duplicate immediate signatures with opposite unsigned/signed version
			if (!isOtherImmediate && (opCodeArgFlags & OpCodeArgFlags.UnsignedUIntNotSupported) == 0) {
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

				if (signature != signatureWithOtherImmediate)
					AddOpCodeToGroup(name, memoName, signatureWithOtherImmediate, def, opCodeArgFlags | OpCodeArgFlags.HasImmediateUnsigned, null, numberLeadingArgToDiscard, argSizes, true);
			}

			if (!pseudoOpsKind.HasValue && (opCodeArgFlags & OpCodeArgFlags.HasRegisterMemoryMappedToRegister) == 0) {
				var broadcastName = RenameAmbiguousBroadcasts(name, def);
				if ((opCodeArgFlags & OpCodeArgFlags.IsBroadcastXYZ) == 0 && broadcastName != name) {
					group.Flags |= OpCodeArgFlags.HasAmbiguousBroadcast;
					AddOpCodeToGroup(broadcastName, memoName, signature, def, opCodeArgFlags | OpCodeArgFlags.IsBroadcastXYZ, null, numberLeadingArgToDiscard, argSizes, isOtherImmediate);
				}
			}

			// Handle label as ulong
			if (!pseudoOpsKind.HasValue && (opCodeArgFlags & OpCodeArgFlags.HasLabelUlong) == 0 && (opCodeArgFlags & OpCodeArgFlags.HasLabel) != 0) {
				var newLabelULongSignature = new Signature();
				for (int i = 0; i < signature.ArgCount; i++) {
					var argKind = signature.GetArgKind(i);
					switch (argKind) {
					case ArgKind.Label:
						argKind = ArgKind.LabelUlong;
						break;
					}
					newLabelULongSignature.AddArgKind(argKind);
				}
				AddOpCodeToGroup(name, memoName, newLabelULongSignature, def, opCodeArgFlags | OpCodeArgFlags.HasLabelUlong, null, numberLeadingArgToDiscard, argSizes, isOtherImmediate);
			}

			return group;
		}

		protected static bool IsArgKindImmediate(ArgKind argKind) =>
			argKind switch {
				ArgKind.Immediate or ArgKind.ImmediateUnsigned => true,
				_ => false,
			};

		void CreatePseudoInstructions() {
			foreach (var group in groupsWithPseudo.Values) {
				var pseudo = group.RootPseudoOpsKind ?? throw new InvalidOperationException("Root cannot be null");
				var pseudoInfo = FormatterConstants.GetPseudoOps(pseudo);

				// Create new signature with last imm argument
				var signature = new Signature();
				for (int i = 0; i < group.Signature.ArgCount - 1; i++)
					signature.AddArgKind(group.Signature.GetArgKind(i));

				for (int i = 0; i < pseudoInfo.Length; i++) {
					var (name, imm) = pseudoInfo[i];
					var key = new GroupKey(name, signature);

					var newGroup = new OpCodeInfoGroup(memDefs, defs, name, signature) {
						Flags = OpCodeArgFlags.Pseudo,
						AllOpCodeFlags = group.AllOpCodeFlags,
						MemoName = group.MemoName,
						ParentPseudoOpsKind = @group,
						PseudoOpsKindImmediateValue = imm
					};
					newGroup.UpdateMaxArgSizes(group.MaxArgSizes);
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
			public int ArgCount;
			ulong argKinds;

			public ArgKind GetArgKind(int argIndex) => (ArgKind)((argKinds >> (8 * argIndex)) & 0xFF);

			public void AddArgKind(ArgKind kind) {
				var shift = (8 * ArgCount);
				argKinds = (argKinds & ~((ulong)0xFF << shift)) | ((ulong)kind << shift);
				ArgCount++;
			}

			public override string ToString() {
				var builder = new StringBuilder();
				builder.Append('(');
				for (int i = 0; i < ArgCount; i++) {
					if (i > 0) builder.Append(", ");
					builder.Append(GetArgKind(i));
				}
				builder.Append(')');
				return builder.ToString();
			}

			public bool Equals(Signature other) => ArgCount == other.ArgCount && argKinds == other.argKinds;
			public override bool Equals(object? obj) => obj is Signature other && Equals(other);
			public override int GetHashCode() => HashCode.Combine(ArgCount, argKinds);
			public static bool operator ==(Signature left, Signature right) => left.Equals(right);
			public static bool operator !=(Signature left, Signature right) => !left.Equals(right);

			public int CompareTo(Signature other) {
				var argCountComparison = ArgCount.CompareTo(other.ArgCount);
				if (argCountComparison != 0) return argCountComparison;
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
			RegisterST,
			RegisterSegment,
			RegisterBND,
			RegisterMM,
			RegisterXMM,
			RegisterYMM,
			RegisterZMM,
			RegisterCR,
			RegisterDR,
			RegisterTR,
			RegisterTMM,

			Register8Memory,
			Register16Memory,
			Register32Memory,
			Register64Memory,
			RegisterKMemory,
			RegisterBNDMemory,
			RegisterMMMemory,
			RegisterXMMMemory,
			RegisterYMMMemory,
			RegisterZMMMemory,

			Memory,
			Immediate,
			ImmediateUnsigned,
			Label,
			LabelUlong,

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

			public OpCodeInfoGroup(MemorySizeDef[] memDefs, InstructionDef[] defs, string name, Signature signature) {
				this.memDefs = memDefs;
				this.defs = defs;
				Name = name;
				MemoName = name;
				Signature = signature;
				Items = new List<InstructionDef>();
				MaxArgSizes = new List<int>();
			}

			public string MemoName { get; set; }
			public string Name { get; }
			public InstructionDefFlags1 AllOpCodeFlags { get; set; }
			public OpCodeArgFlags Flags { get; set; }
			public PseudoOpsKind? RootPseudoOpsKind { get; set; }
			public OpCodeInfoGroup? ParentPseudoOpsKind { get; set; }
			public int PseudoOpsKindImmediateValue { get; set; }
			public bool HasLabel => (Flags & OpCodeArgFlags.HasLabel) != 0;
			public bool HasSpecialInstructionEncoding => (Flags & OpCodeArgFlags.HasSpecialInstructionEncoding) != 0;
			public bool IsBranch => (Flags & (OpCodeArgFlags.HasBranchShort | OpCodeArgFlags.HasBranchNear)) != 0;
			public bool HasRegisterMemoryMappedToRegister => (Flags & OpCodeArgFlags.HasRegisterMemoryMappedToRegister) != 0;
			public bool HasVexAndEvex => (Flags & (OpCodeArgFlags.HasVex | OpCodeArgFlags.HasEvex)) == (OpCodeArgFlags.HasVex | OpCodeArgFlags.HasEvex);
			public bool HasImmediateUnsigned => (Flags & OpCodeArgFlags.HasImmediateUnsigned) != 0;
			public Signature Signature { get; }
			public OpCodeNode RootOpCodeNode { get; set; }
			public int MaxImmediateSize { get; set; }
			public List<InstructionDef> Items { get; }
			public List<int> MaxArgSizes { get; }
			public int NumberOfLeadingArgToDiscard { get; set; }

			public void UpdateMaxArgSizes(List<int> argSizes) {
				if (MaxArgSizes.Count == 0)
					MaxArgSizes.AddRange(argSizes);
				Debug.Assert(MaxArgSizes.Count == argSizes.Count);
				for (int i = 0; i < MaxArgSizes.Count; i++) {
					if (MaxArgSizes[i] < argSizes[i])
						MaxArgSizes[i] = argSizes[i];
				}
			}

			public int OrderOpCodesPerOpKindPriority(InstructionDef x, InstructionDef y) {
				Debug.Assert(x.OpKindDefs.Length == y.OpKindDefs.Length);
				int result;
				for (int i = 0; i < x.OpKindDefs.Length; i++) {
					if (!IsRegister(Signature.GetArgKind(i))) continue;
					result = GetPriorityFromKind(x.OpKindDefs[i], GetMemorySizeInBits(memDefs, defs, x)).CompareTo(GetPriorityFromKind(y.OpKindDefs[i], GetMemorySizeInBits(memDefs, defs, y)));
					if (result != 0) return result;
				}

				for (int i = 0; i < x.OpKindDefs.Length; i++) {
					if (IsRegister(Signature.GetArgKind(i))) continue;
					result = GetPriorityFromKind(x.OpKindDefs[i], GetMemorySizeInBits(memDefs, defs, x)).CompareTo(GetPriorityFromKind(y.OpKindDefs[i], GetMemorySizeInBits(memDefs, defs, y)));
					if (result != 0) return result;
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
				OpCodeSelectorKind.EvexBroadcastZ => (OpCodeArgFlags.HasEvex | OpCodeArgFlags.HasBroadcast, OpCodeArgFlags.Default),
				OpCodeSelectorKind.BranchShort => (OpCodeArgFlags.HasBranchShort, OpCodeArgFlags.HasBranchNear),
				_ => (OpCodeArgFlags.Default, OpCodeArgFlags.Default),
			};

		protected static string GetRegisterSuffix(RegisterDef def) =>
			(RegisterKind)def.RegisterKind.Value switch {
				RegisterKind.GPR8 => "8",
				RegisterKind.GPR16 => "16",
				RegisterKind.GPR32 => "32",
				RegisterKind.GPR64 => "64",
				RegisterKind.IP => "IP",
				RegisterKind.Segment => "Segment",
				RegisterKind.ST => "ST",
				RegisterKind.CR => "CR",
				RegisterKind.DR => "DR",
				RegisterKind.TR => "TR",
				RegisterKind.BND => "BND",
				RegisterKind.K => "K",
				RegisterKind.MM => "MM",
				RegisterKind.XMM => "XMM",
				RegisterKind.YMM => "YMM",
				RegisterKind.ZMM => "ZMM",
				RegisterKind.TMM => "TMM",
				_ => throw new InvalidOperationException(),
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

		bool IsAmbiguousBroadcast(InstructionDef def) {
			switch (GetOrigCodeValue(def.Code)) {
			case Code.EVEX_Vcvtpd2ps_xmm_k1z_xmmm128b64:
			case Code.EVEX_Vcvtpd2ps_xmm_k1z_ymmm256b64:
			case Code.EVEX_Vcvtqq2ps_xmm_k1z_xmmm128b64:
			case Code.EVEX_Vcvtqq2ps_xmm_k1z_ymmm256b64:
			case Code.EVEX_Vcvttpd2udq_xmm_k1z_xmmm128b64:
			case Code.EVEX_Vcvttpd2udq_xmm_k1z_ymmm256b64:
			case Code.EVEX_Vcvtpd2udq_xmm_k1z_xmmm128b64:
			case Code.EVEX_Vcvtpd2udq_xmm_k1z_ymmm256b64:
			case Code.EVEX_Vcvtuqq2ps_xmm_k1z_xmmm128b64:
			case Code.EVEX_Vcvtuqq2ps_xmm_k1z_ymmm256b64:
			case Code.EVEX_Vcvttpd2dq_xmm_k1z_xmmm128b64:
			case Code.EVEX_Vcvttpd2dq_xmm_k1z_ymmm256b64:
			case Code.EVEX_Vcvtpd2dq_xmm_k1z_xmmm128b64:
			case Code.EVEX_Vcvtpd2dq_xmm_k1z_ymmm256b64:
			case Code.EVEX_Vcvtneps2bf16_xmm_k1z_xmmm128b32:
			case Code.EVEX_Vcvtneps2bf16_xmm_k1z_ymmm256b32:
			case Code.EVEX_Vfpclassps_kr_k1_xmmm128b32_imm8:
			case Code.EVEX_Vfpclassps_kr_k1_ymmm256b32_imm8:
			case Code.EVEX_Vfpclassps_kr_k1_zmmm512b32_imm8:
			case Code.EVEX_Vfpclasspd_kr_k1_xmmm128b64_imm8:
			case Code.EVEX_Vfpclasspd_kr_k1_ymmm256b64_imm8:
			case Code.EVEX_Vfpclasspd_kr_k1_zmmm512b64_imm8:
				return true;
			}
			return false;
		}

		string RenameAmbiguousBroadcasts(string name, InstructionDef def) {
			if ((def.Flags1 & InstructionDefFlags1.Broadcast) == 0)
				return name;

			if (IsAmbiguousBroadcast(def)) {
				for (int i = 0; i < def.OpKindDefs.Length; i++) {
					var opKindDef = def.OpKindDefs[i];
					if (opKindDef.OperandEncoding == OperandEncoding.RegMemModrmRm) {
						switch (opKindDef.Register) {
						case Register.XMM0: return $"{name}x";
						case Register.YMM0: return $"{name}y";
						case Register.ZMM0: return $"{name}z";
						}
					}
				}
			}

			return name;
		}

		protected readonly struct OpCodeNode {
			readonly object value;

			public OpCodeNode(InstructionDef def) => value = def;
			public OpCodeNode(OpCodeSelector selector) => value = selector;

			public bool IsEmpty => value is null;
			public InstructionDef? Def => value as InstructionDef;
			public OpCodeSelector? Selector => value as OpCodeSelector;
			public static implicit operator OpCodeNode(InstructionDef def) => new OpCodeNode(def);
			public static implicit operator OpCodeNode(OpCodeSelector selector) => new OpCodeNode(selector);
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

			BranchShort,

			ImmediateInt,
			ImmediateByte,
			ImmediateByteEqual1,
			ImmediateByteSigned8,
			ImmediateByteSigned8To16,
			ImmediateByteSigned8To32,
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
