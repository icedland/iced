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
using System.Linq;
using System.Text;
using Generator.Encoder;
using Generator.Enums;
using Generator.Enums.Encoder;
using Generator.Enums.Formatter;
using Generator.Formatters;
using Generator.Tables;

namespace Generator.Assembler {
	abstract class AssemblerSyntaxGenerator {
		protected readonly GenTypes genTypes;
		protected readonly EncoderTypes encoderTypes;
		protected readonly InstructionDef[] defs;
		readonly object[][] intelCtorInfos;
		readonly MemorySizeInfoTable memorySizeInfoTable;
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
			encoderTypes = genTypes.GetObject<EncoderTypes>(TypeIds.EncoderTypes);
			intelCtorInfos = genTypes.GetObject<Formatters.Intel.CtorInfos>(TypeIds.IntelCtorInfos).Infos;
			memorySizeInfoTable = genTypes.GetObject<MemorySizeInfoTable>(TypeIds.MemorySizeInfoTable);
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

				Code.PrefetchReserved3_m8,
				Code.PrefetchReserved4_m8,
				Code.PrefetchReserved5_m8,
				Code.PrefetchReserved6_m8,
				Code.PrefetchReserved7_m8,

				// The following are implemented manually
				Code.Call_ptr1616,
				Code.Call_ptr1632,
				Code.Xlat_m8,
				Code.Jmp_ptr1616,
				Code.Jmp_ptr1632,
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
				(Code.ReservedNop_rm16_r16_0F0D, "reserved_nop_0f0d"),
				(Code.ReservedNop_rm32_r32_0F0D, "reserved_nop_0f0d"),
				(Code.ReservedNop_rm64_r64_0F0D, "reserved_nop_0f0d"),
				(Code.ReservedNop_rm16_r16_0F18, "reserved_nop_0f18"),
				(Code.ReservedNop_rm32_r32_0F18, "reserved_nop_0f18"),
				(Code.ReservedNop_rm64_r64_0F18, "reserved_nop_0f18"),
				(Code.ReservedNop_rm16_r16_0F19, "reserved_nop_0f19"),
				(Code.ReservedNop_rm32_r32_0F19, "reserved_nop_0f19"),
				(Code.ReservedNop_rm64_r64_0F19, "reserved_nop_0f19"),
				(Code.ReservedNop_rm16_r16_0F1A, "reserved_nop_0f1a"),
				(Code.ReservedNop_rm32_r32_0F1A, "reserved_nop_0f1a"),
				(Code.ReservedNop_rm64_r64_0F1A, "reserved_nop_0f1a"),
				(Code.ReservedNop_rm16_r16_0F1B, "reserved_nop_0f1b"),
				(Code.ReservedNop_rm32_r32_0F1B, "reserved_nop_0f1b"),
				(Code.ReservedNop_rm64_r64_0F1B, "reserved_nop_0f1b"),
				(Code.ReservedNop_rm16_r16_0F1C, "reserved_nop_0f1c"),
				(Code.ReservedNop_rm32_r32_0F1C, "reserved_nop_0f1c"),
				(Code.ReservedNop_rm64_r64_0F1C, "reserved_nop_0f1c"),
				(Code.ReservedNop_rm16_r16_0F1D, "reserved_nop_0f1d"),
				(Code.ReservedNop_rm32_r32_0F1D, "reserved_nop_0f1d"),
				(Code.ReservedNop_rm64_r64_0F1D, "reserved_nop_0f1d"),
				(Code.ReservedNop_rm16_r16_0F1E, "reserved_nop_0f1e"),
				(Code.ReservedNop_rm32_r32_0F1E, "reserved_nop_0f1e"),
				(Code.ReservedNop_rm64_r64_0F1E, "reserved_nop_0f1e"),
				(Code.ReservedNop_rm16_r16_0F1F, "reserved_nop_0f1f"),
				(Code.ReservedNop_rm32_r32_0F1F, "reserved_nop_0f1f"),
				(Code.ReservedNop_rm64_r64_0F1F, "reserved_nop_0f1f"),
			}.Select(a => (code: origCode[(int)a.Item1], name: a.Item2)).Where(a => !removed.Contains(a.code)).ToDictionary(a => a.code, a => a.name);
		}

		protected const OpCodeFlags BitnessMaskFlags = OpCodeFlags.Mode64 | OpCodeFlags.Mode32 | OpCodeFlags.Mode16;

		protected abstract void GenerateRegisters(EnumType registers);

		protected abstract void Generate(Dictionary<GroupKey, OpCodeInfoGroup> map, OpCodeInfoGroup[] opCodes);

		public void Generate() {
			GenerateRegisters(genTypes[TypeIds.Register]);
			GenerateOpCodes();
		}

		void GenerateOpCodes() {
			foreach (var def in defs) {
				var opCodeInfo = def.OpCodeInfo;
				var codeValue = opCodeInfo.Code;
				if (discardOpCodes.Contains(codeValue)) continue;

				string memoName = def.Mnemonic.RawName;
				var name = mapOpCodeToNewName.TryGetValue(codeValue, out var nameOpt) ? nameOpt : memoName.ToLowerInvariant();
				if (codeValue == codeInt3) name = "int3";

				bool toAdd = true;
				var signature = new Signature();
				var regOnlySignature = new Signature();

				PseudoOpsKind? pseudoOpsKind = null;
				{
					var ctorInfos = intelCtorInfos[opCodeInfo.Code.Value];
					var enumValue = ctorInfos[ctorInfos.Length - 1] as EnumValue;
					if (enumValue is object && enumValue.DeclaringType.TypeId == TypeIds.PseudoOpsKind) {
						pseudoOpsKind = (PseudoOpsKind)enumValue.Value;
					}
				}

				var opCodeArgFlags = OpCodeArgFlags.Default;

				if (opCodeInfo is VexOpCodeInfo) { opCodeArgFlags |= OpCodeArgFlags.HasVex; }
				if (opCodeInfo is EvexOpCodeInfo) { opCodeArgFlags |= OpCodeArgFlags.HasEvex; }

				if ((opCodeInfo.Flags & OpCodeFlags.ZeroingMasking) != 0) opCodeArgFlags |= OpCodeArgFlags.HasZeroingMask;
				if ((opCodeInfo.Flags & OpCodeFlags.OpMaskRegister) != 0) opCodeArgFlags |= OpCodeArgFlags.HasKMask;
				if ((opCodeInfo.Flags & OpCodeFlags.Broadcast) != 0) opCodeArgFlags |= OpCodeArgFlags.HasBroadcast;
				if ((opCodeInfo.Flags & OpCodeFlags.SuppressAllExceptions) != 0) opCodeArgFlags |= OpCodeArgFlags.SuppressAllExceptions;
				if ((opCodeInfo.Flags & OpCodeFlags.RoundingControl) != 0) opCodeArgFlags |= OpCodeArgFlags.RoundingControl;

				var argSizes = new List<int>();
				bool discard = false;
				string? discardReason = null;

				// For certain instruction, we need to discard them
				int numberLeadingArgToDiscard = 0;
				var numberLeadingArgToDiscardOpt = GetSpecialArgEncodingInstruction(opCodeInfo);
				if (numberLeadingArgToDiscardOpt.HasValue) {
					numberLeadingArgToDiscard = numberLeadingArgToDiscardOpt.Value;
					opCodeArgFlags |= OpCodeArgFlags.HasSpecialInstructionEncoding;
				}

				for (int i = numberLeadingArgToDiscard; i < opCodeInfo.OpKindsLength; i++) {
					var opKind = GetOperandKind(encoderTypes, opCodeInfo, i);
					var argKind = ArgKind.Unknown;
					int argSize = 0;
					switch (opKind) {
					case OpCodeOperandKind.cl:
					case OpCodeOperandKind.al:
					case OpCodeOperandKind.r8_opcode:
					case OpCodeOperandKind.r8_reg:
						argKind = ArgKind.Register8;
						break;

					case OpCodeOperandKind.ax:
					case OpCodeOperandKind.dx:
					case OpCodeOperandKind.r16_opcode:
					case OpCodeOperandKind.r16_reg:
					case OpCodeOperandKind.r16_reg_mem:
					case OpCodeOperandKind.r16_rm:
						argKind = ArgKind.Register16;
						break;

					case OpCodeOperandKind.eax:
					case OpCodeOperandKind.r32_opcode:
					case OpCodeOperandKind.r32_reg:
					case OpCodeOperandKind.r32_vvvv:
					case OpCodeOperandKind.r32_rm:
					case OpCodeOperandKind.r32_reg_mem:
						argKind = ArgKind.Register32;
						break;

					case OpCodeOperandKind.rax:
					case OpCodeOperandKind.r64_reg:
					case OpCodeOperandKind.r64_opcode:
					case OpCodeOperandKind.r64_vvvv:
					case OpCodeOperandKind.r64_rm:
					case OpCodeOperandKind.r64_reg_mem:
						argKind = ArgKind.Register64;
						break;

					case OpCodeOperandKind.es:
					case OpCodeOperandKind.cs:
					case OpCodeOperandKind.ss:
					case OpCodeOperandKind.ds:
					case OpCodeOperandKind.fs:
					case OpCodeOperandKind.gs:
					case OpCodeOperandKind.seg_reg:
						argKind = ArgKind.RegisterSegment;
						break;

					case OpCodeOperandKind.mm_reg:
					case OpCodeOperandKind.mm_rm:
						argKind = ArgKind.RegisterMM;
						break;

					case OpCodeOperandKind.xmm_is4:
					case OpCodeOperandKind.xmm_is5:
					case OpCodeOperandKind.xmm_reg:
					case OpCodeOperandKind.xmm_vvvv:
					case OpCodeOperandKind.xmm_rm:
					case OpCodeOperandKind.xmmp3_vvvv:
						argKind = ArgKind.RegisterXMM;
						break;

					case OpCodeOperandKind.ymm_is4:
					case OpCodeOperandKind.ymm_is5:
					case OpCodeOperandKind.ymm_reg:
					case OpCodeOperandKind.ymm_vvvv:
					case OpCodeOperandKind.ymm_rm:
						argKind = ArgKind.RegisterYMM;
						break;

					case OpCodeOperandKind.zmm_reg:
					case OpCodeOperandKind.zmm_vvvv:
					case OpCodeOperandKind.zmm_rm:
					case OpCodeOperandKind.zmmp3_vvvv:
						argKind = ArgKind.RegisterZMM;
						break;

					case OpCodeOperandKind.kp1_reg:
					case OpCodeOperandKind.k_reg:
					case OpCodeOperandKind.k_vvvv:
					case OpCodeOperandKind.k_rm:
						argKind = ArgKind.RegisterK;
						break;

					case OpCodeOperandKind.cr_reg:
						argKind = ArgKind.RegisterCR;
						break;
					case OpCodeOperandKind.dr_reg:
						argKind = ArgKind.RegisterDR;
						break;
					case OpCodeOperandKind.tr_reg:
						argKind = ArgKind.RegisterTR;
						break;
					case OpCodeOperandKind.st0:
					case OpCodeOperandKind.sti_opcode:
						argKind = ArgKind.RegisterST;
						break;
					case OpCodeOperandKind.bnd_reg:
						argKind = ArgKind.RegisterBND;
						break;

					case OpCodeOperandKind.seg_rSI:
					case OpCodeOperandKind.es_rDI:
						argKind = ArgKind.Memory;
						break;

					case OpCodeOperandKind.k_or_mem:
						argKind = ArgKind.RegisterKMemory;
						break;
					case OpCodeOperandKind.mm_or_mem:
						argKind = ArgKind.RegisterMMMemory;
						break;
					case OpCodeOperandKind.xmm_or_mem:
						argKind = ArgKind.RegisterXMMMemory;
						break;
					case OpCodeOperandKind.ymm_or_mem:
						argKind = ArgKind.RegisterYMMMemory;
						break;
					case OpCodeOperandKind.zmm_or_mem:
						argKind = ArgKind.RegisterZMMMemory;
						break;
					case OpCodeOperandKind.r8_or_mem:
						argKind = ArgKind.Register8Memory;
						break;
					case OpCodeOperandKind.r16_or_mem:
						argKind = ArgKind.Register16Memory;
						break;
					case OpCodeOperandKind.r32_or_mem:
					case OpCodeOperandKind.r32_or_mem_mpx:
						argKind = ArgKind.Register32Memory;
						break;
					case OpCodeOperandKind.r64_or_mem:
					case OpCodeOperandKind.r64_or_mem_mpx:
						argKind = ArgKind.Register64Memory;
						break;
					case OpCodeOperandKind.bnd_or_mem_mpx:
						argKind = ArgKind.RegisterBNDMemory;
						break;

					case OpCodeOperandKind.mem:
					case OpCodeOperandKind.mem_offs:
					case OpCodeOperandKind.mem_mpx:
					case OpCodeOperandKind.mem_mib:
					case OpCodeOperandKind.mem_vsib32x:
					case OpCodeOperandKind.mem_vsib64x:
					case OpCodeOperandKind.mem_vsib32y:
					case OpCodeOperandKind.mem_vsib64y:
					case OpCodeOperandKind.mem_vsib32z:
					case OpCodeOperandKind.mem_vsib64z:
						argKind = ArgKind.Memory;
						break;

					case OpCodeOperandKind.xbegin_4:
					case OpCodeOperandKind.brdisp_4:
						argKind = ArgKind.Label;
						opCodeArgFlags |= OpCodeArgFlags.HasBranchNear;
						opCodeArgFlags |= OpCodeArgFlags.HasLabel;
						break;

					case OpCodeOperandKind.br32_4: // NEAR
					case OpCodeOperandKind.br64_4: // NEAR
					case OpCodeOperandKind.br16_2:
						argKind = ArgKind.Label;
						if (name != "call") {
							opCodeArgFlags |= OpCodeArgFlags.HasBranchNear;
						}
						opCodeArgFlags |= OpCodeArgFlags.HasLabel;
						break;

					case OpCodeOperandKind.xbegin_2:
					case OpCodeOperandKind.brdisp_2:
					case OpCodeOperandKind.br16_1:
					case OpCodeOperandKind.br32_1:
					case OpCodeOperandKind.br64_1:
						argKind = ArgKind.Label;
						opCodeArgFlags |= OpCodeArgFlags.HasBranchShort;
						opCodeArgFlags |= OpCodeArgFlags.HasLabel;

						if (opCodeInfo is LegacyOpCodeInfo legacy) {
							switch (name) {
							case "loopne":
							case "loope":
							case "loop":
							case "jcxz":
							case "jecxz":
							case "jrcxz":
								if (legacy.OperandSize != OperandSize.None || legacy.AddressSize != AddressSize.None) {
									if (legacy.OperandSize != (OperandSize)legacy.AddressSize) {
										argKind = ArgKind.Unknown;
										discardReason = $"Duplicated";
									}
								}
								break;
							}
						}
						break;

					case OpCodeOperandKind.imm8_const_1:
						opCodeArgFlags |= OpCodeArgFlags.HasImmediateByteEqual1;
						argSize = 1;
						argKind = ArgKind.Immediate;
						break;

					case OpCodeOperandKind.imm2_m2z:
						opCodeArgFlags |= OpCodeArgFlags.HasImmediateByteLessThanBits;
						argKind = ArgKind.Immediate;
						argSize = 1;
						break;

					case OpCodeOperandKind.imm8:
						opCodeArgFlags |= OpCodeArgFlags.HasImmediateByte;
						argKind = ArgKind.Immediate;
						argSize = 1;
						break;

					case OpCodeOperandKind.imm8sex16:
					case OpCodeOperandKind.imm8sex32:
						opCodeArgFlags |= OpCodeArgFlags.HasImmediateByteSignedExtended;
						argKind = ArgKind.Immediate;
						argSize = 1;
						break;

					case OpCodeOperandKind.imm8sex64:
						opCodeArgFlags |= OpCodeArgFlags.HasImmediateByteSignedExtended | OpCodeArgFlags.UnsignedUIntNotSupported;
						argKind = ArgKind.Immediate;
						argSize = 1;
						break;

					case OpCodeOperandKind.imm16:
						argKind = ArgKind.Immediate;
						argSize = 2;
						break;

					case OpCodeOperandKind.imm32:
						argKind = ArgKind.Immediate;
						argSize = 4;
						break;

					case OpCodeOperandKind.imm32sex64:
						opCodeArgFlags |= OpCodeArgFlags.UnsignedUIntNotSupported;
						argKind = ArgKind.Immediate;
						argSize = 4;
						break;

					case OpCodeOperandKind.imm64:
						argKind = ArgKind.Immediate;
						argSize = 8;
						break;
					}

					if (argKind == ArgKind.Unknown) {
						toAdd = false;
						break;
					}
					else {
						argSizes.Add(argSize);
						signature.AddArgKind(GetArgKindForSignature(argKind, true));
						regOnlySignature.AddArgKind(GetArgKindForSignature(argKind, false));
					}
				}

				if (toAdd) {
					if (!ShouldDiscardDuplicatedOpCode(signature, opCodeInfo)) {
						// discard r16m16
						bool hasR64M16 = IsR64M16(opCodeInfo);
						if (!hasR64M16) {
							AddOpCodeToGroup(name, memoName, signature, opCodeInfo, opCodeArgFlags, pseudoOpsKind, numberLeadingArgToDiscard, argSizes, false);
						}
					}

					if (signature != regOnlySignature) {
						opCodeArgFlags = opCodeArgFlags & ~OpCodeArgFlags.HasBroadcast;
						AddOpCodeToGroup(name, memoName, regOnlySignature, opCodeInfo, opCodeArgFlags | OpCodeArgFlags.HasRegisterMemoryMappedToRegister, pseudoOpsKind, numberLeadingArgToDiscard, argSizes, false);
					}
				}
				else {
					if (discard) {
						Console.WriteLine($"Discarding: {opCodeInfo.GetType().Name} {memoName.ToLowerInvariant()} => {opCodeInfo.Code.RawName}. Reason: {discardReason}");
					}
					else {
						Console.WriteLine($"TODO: {opCodeInfo.GetType().Name} {memoName.ToLowerInvariant()} => {opCodeInfo.Code.RawName} not supported yet");
					}
				}
			}

			CreatePseudoInstructions();

			var orderedGroups = groups.OrderBy(x => x.Key).Select(x => x.Value).ToList();
			var signatures = new HashSet<Signature>();
			var opcodes = new List<OpCodeInfo>();
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
				if (@group.HasSpecialInstructionEncoding) {
					@group.RootOpCodeNode = new OpCodeNode(@group.Items[0]);
				}
				else {
					@group.RootOpCodeNode = BuildSelectorGraph(@group);
				}
			}

			Generate(groups, orderedGroups.ToArray());
		}

		static ArgKind GetArgKindForSignature(ArgKind kind, bool memory) {
			switch (kind) {
			case ArgKind.Register8Memory:
				return memory ? ArgKind.Memory : ArgKind.Register8;
			case ArgKind.Register16Memory:
				return memory ? ArgKind.Memory : ArgKind.Register16;
			case ArgKind.Register32Memory:
				return memory ? ArgKind.Memory : ArgKind.Register32;
			case ArgKind.Register64Memory:
				return memory ? ArgKind.Memory : ArgKind.Register64;
			case ArgKind.RegisterKMemory:
				return memory ? ArgKind.Memory : ArgKind.RegisterK;
			case ArgKind.RegisterBNDMemory:
				return memory ? ArgKind.Memory : ArgKind.RegisterBND;
			case ArgKind.RegisterMMMemory:
				return memory ? ArgKind.Memory : ArgKind.RegisterMM;
			case ArgKind.RegisterXMMMemory:
				return memory ? ArgKind.Memory : ArgKind.RegisterXMM;
			case ArgKind.RegisterYMMMemory:
				return memory ? ArgKind.Memory : ArgKind.RegisterYMM;
			case ArgKind.RegisterZMMMemory:
				return memory ? ArgKind.Memory : ArgKind.RegisterZMM;
			}
			return kind;
		}

		int? GetSpecialArgEncodingInstruction(OpCodeInfo opCodeInfo) {
			switch (GetOrigCodeValue(opCodeInfo.Code)) {
			case Code.Outsb_DX_m8:
			case Code.Outsw_DX_m16:
			case Code.Outsd_DX_m32:
			case Code.Lodsb_AL_m8:
			case Code.Lodsw_AX_m16:
			case Code.Lodsd_EAX_m32:
			case Code.Lodsq_RAX_m64:
			case Code.Scasb_AL_m8:
			case Code.Scasw_AX_m16:
			case Code.Scasd_EAX_m32:
			case Code.Scasq_RAX_m64:
			case Code.Insb_m8_DX:
			case Code.Insw_m16_DX:
			case Code.Insd_m32_DX:
			case Code.Stosb_m8_AL:
			case Code.Stosw_m16_AX:
			case Code.Stosd_m32_EAX:
			case Code.Stosq_m64_RAX:
			case Code.Cmpsb_m8_m8:
			case Code.Cmpsw_m16_m16:
			case Code.Cmpsd_m32_m32:
			case Code.Cmpsq_m64_m64:
			case Code.Movsb_m8_m8:
			case Code.Movsw_m16_m16:
			case Code.Movsd_m32_m32:
			case Code.Movsq_m64_m64:
				return 2;
			case Code.Maskmovq_rDI_mm_mm:
			case Code.Maskmovdqu_rDI_xmm_xmm:
			case Code.VEX_Vmaskmovdqu_rDI_xmm_xmm:
				return 1;
			case Code.Xbegin_rel16:
			case Code.Xbegin_rel32:
				return 0;
			}

			return null;
		}

		OpCodeNode BuildSelectorGraph(OpCodeInfoGroup group) {
			// In case of one opcode, we don't need to perform any disambiguation
			var opcodes = group.Items;
			// Sort opcodes by decreasing size
			opcodes.Sort(group.OrderOpCodesPerOpKindPriority);
			return BuildSelectorGraph(group, group.Signature, group.Flags, opcodes);
		}

		OpCodeNode BuildSelectorGraph(OpCodeInfoGroup group, Signature signature, OpCodeArgFlags argFlags, List<OpCodeInfo> opcodes) {
			if (opcodes.Count == 0) return default;

			if (opcodes.Count == 1) {
				return new OpCodeNode(opcodes[0]);
			}

			Debug.Assert(stackDepth++ < 16, "Potential StackOverflow");
			try {
				OrderedSelectorList selectors;

				if ((argFlags & OpCodeArgFlags.HasImmediateByteEqual1) != 0) {
					// handle imm8 == 1 
					var opcodesWithImmediateByteEqual1 = new List<OpCodeInfo>();
					var opcodesOthers = new List<OpCodeInfo>();
					var indices = CollectByOperandKindPredicate(opcodes, IsImmediateByteEqual1, opcodesWithImmediateByteEqual1, opcodesOthers);
					Debug.Assert(indices.Count == 1);
					var newFlags = argFlags ^ OpCodeArgFlags.HasImmediateByteEqual1;
					return new OpCodeSelector(indices[0], OpCodeSelectorKind.ImmediateByteEqual1) { IfTrue = BuildSelectorGraph(group, group.Signature, newFlags, opcodesWithImmediateByteEqual1), IfFalse = BuildSelectorGraph(group, group.Signature, newFlags, opcodesOthers) };
				}
				else if (group.Name != "jmpe" && group.IsBranch) {
					var branchShort = new List<OpCodeInfo>();
					var branchFar = new List<OpCodeInfo>();
					CollectByOperandKindPredicate(opcodes, IsBranchShort, branchShort, branchFar);
					if (branchShort.Count > 0 && branchFar.Count > 0) {
						var newFlags = argFlags & ~(OpCodeArgFlags.HasBranchShort | OpCodeArgFlags.HasBranchNear);
						return new OpCodeSelector(OpCodeSelectorKind.BranchShort) { IfTrue = BuildSelectorGraph(group, group.Signature, newFlags, branchShort), IfFalse = BuildSelectorGraph(group, group.Signature, newFlags, branchFar) };
					}
				}

				// Handle case of moffs
				if (group.Name == "mov") {
					var opCodesRAXMOffs = new List<OpCodeInfo>();
					var newOpCodes = new List<OpCodeInfo>();

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
					int memoryIndex = GetBroadcastMemory(argFlags, opcodes, signature, out var broadcastSelectorKind, out var evexBroadcastOpCode);
					if (memoryIndex >= 0) {
						Debug.Assert(evexBroadcastOpCode is object);
						return new OpCodeSelector(memoryIndex, broadcastSelectorKind) {
							IfTrue = evexBroadcastOpCode,
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
						if (memSelectors.Count > selectors.Count) {
							selectors = memSelectors;
						}
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
						var opcodesWithImmediateByteSigned = new List<OpCodeInfo>();
						var opcodesOthers = new List<OpCodeInfo>();
						var indices = CollectByOperandKindPredicate(opcodes, IsImmediateByteSigned, opcodesWithImmediateByteSigned, opcodesOthers);

						var selectorKind = OpCodeSelectorKind.ImmediateByteSigned8;
						int opSize;
						if (isPushImm)
							opSize = GetImmediateSizeInBits(opcodes[0]);
						else
							opSize = GetMemoryAddressSizeInBits(memorySizeInfoTable, defs, opcodes[0]);
						if (opSize > 1) {
							switch (opSize) {
							case 32:
								selectorKind = OpCodeSelectorKind.ImmediateByteSigned8To32;
								break;
							case 16:
								selectorKind = OpCodeSelectorKind.ImmediateByteSigned8To16;
								break;
							default:
								break;
							}
						}

						Debug.Assert(indices.Count == 1);
						var newFlags = argFlags ^ OpCodeArgFlags.HasImmediateByteSignedExtended;
						return new OpCodeSelector(indices[0], selectorKind) { IfTrue = BuildSelectorGraph(group, group.Signature, newFlags, opcodesWithImmediateByteSigned), IfFalse = BuildSelectorGraph(group, group.Signature, newFlags, opcodesOthers) };
					}

					if ((argFlags & (OpCodeArgFlags.HasVex | OpCodeArgFlags.HasEvex)) == (OpCodeArgFlags.HasVex | OpCodeArgFlags.HasEvex)) {
						var vex = opcodes.Where(x => x is VexOpCodeInfo).ToList();
						var evex = opcodes.Where(x => x is EvexOpCodeInfo).ToList();

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
					if (list.Count == 1 && selectorIndex + 1 == selectors.Count) {
						node = new OpCodeNode(list[0]);
					}
					else {
						goto default;
					}
					break;
				case OpCodeSelectorKind.Register8:
				case OpCodeSelectorKind.Register16:
				case OpCodeSelectorKind.Register32:
				case OpCodeSelectorKind.Register64:
				case OpCodeSelectorKind.RegisterST:
					if (list.Count == 1 || selectorIndex + 1 == selectors.Count) {
						node = BuildSelectorGraph(group, signature, argFlags, list);
					}
					else {
						goto default;
					}
					break;
				default:
					newSelector = selectors.ArgIndex >= 0 ? new OpCodeSelector(selectors.ArgIndex, kind) : new OpCodeSelector(kind);
					node = new OpCodeNode(newSelector);
					newSelector.IfTrue = list.Count == 1 ? new OpCodeNode(list[0]) : BuildSelectorGraph(group, signature, argFlags, list);
					break;
				}

				if (rootNode.IsEmpty) {
					rootNode = node;
				}

				if (previousSelector is object) {
					previousSelector.IfFalse = node;
				}

				previousSelector = newSelector;

				selectorIndex++;
			}
			return rootNode;
		}

		static OrderedSelectorList BuildSelectorsPerBitness(OpCodeInfoGroup @group, OpCodeArgFlags argFlags, List<OpCodeInfo> opcodes) {
			var selectors = new OrderedSelectorList();
			foreach (var opCodeInfo in opcodes) {
				if (opCodeInfo is LegacyOpCodeInfo legacy) {
					int bitness = GetBitness(legacy);

					OpCodeSelectorKind selectorKind;
					switch (bitness) {
					case 16:
						selectorKind = OpCodeSelectorKind.Bitness16;
						break;
					case 32:
						selectorKind = OpCodeSelectorKind.Bitness32;
						break;
					default:
					case 64:
						selectorKind = OpCodeSelectorKind.Bitness64;
						break;
					}

					selectors.Add(selectorKind, opCodeInfo);
				}
				else {
					Console.WriteLine($"Unable to detect bitness for opcode {opCodeInfo.Code.RawName} for group {@group.Name} / {argFlags}");
					//selectors.Add(OpCodeSelectorKind.Invalid, opCodeInfo);
				}
			}

			// Try to detect bitness differently (for dec_rm16/dec_r16)
			if (selectors.Count == 1) {
				selectors.Clear();
				var added = new HashSet<OpCodeInfo>();
				foreach (var bitnessMask in new OpCodeFlags[] { OpCodeFlags.Mode64, OpCodeFlags.Mode32, OpCodeFlags.Mode16 }) {
					foreach (var opCodeInfo in opcodes) {
						if ((opCodeInfo.Flags & bitnessMask) == 0) continue;
						if (added.Contains(opCodeInfo)) continue;

						OpCodeSelectorKind selectorKind;
						switch (bitnessMask) {
						case OpCodeFlags.Mode16:
							selectorKind = OpCodeSelectorKind.Bitness16;
							break;
						case OpCodeFlags.Mode32:
							selectorKind = OpCodeSelectorKind.Bitness32;
							break;
						case OpCodeFlags.Mode64:
							selectorKind = OpCodeSelectorKind.Bitness64;
							break;
						default:
							throw new ArgumentException($"Invalid {bitnessMask}");
						}

						added.Add(opCodeInfo);
						selectors.Add(selectorKind, opCodeInfo);
					}
				}
			}

			return selectors;
		}

		int GetBroadcastMemory(OpCodeArgFlags argFlags, List<OpCodeInfo> opcodes, Signature signature, out OpCodeSelectorKind selctorKind, out OpCodeInfo? broadcastOpCodeInfo) {
			broadcastOpCodeInfo = null;
			selctorKind = OpCodeSelectorKind.Invalid;
			int memoryIndex = -1;
			if ((argFlags & OpCodeArgFlags.HasBroadcast) != 0) {
				for (int i = 0; i < signature.ArgCount; i++) {
					if (signature.GetArgKind(i) == ArgKind.Memory) {
						memoryIndex = i;
						var evex = @opcodes.First(x => x is EvexOpCodeInfo);
						var opKind = evex.OpKind(encoderTypes, i);
						broadcastOpCodeInfo = evex;
						switch (opKind) {
						case OpCodeOperandKind.xmm_or_mem:
							selctorKind = OpCodeSelectorKind.EvexBroadcastX;
							break;
						case OpCodeOperandKind.ymm_or_mem:
							selctorKind = OpCodeSelectorKind.EvexBroadcastY;
							break;
						case OpCodeOperandKind.zmm_or_mem:
							selctorKind = OpCodeSelectorKind.EvexBroadcastZ;
							break;
						default:
							throw new ArgumentException($"invalud {opKind}");
						}
						break;
					}
				}
				Debug.Assert(memoryIndex >= 0);
			}

			return memoryIndex;
		}

		bool ShouldDiscardDuplicatedOpCode(Signature signature, OpCodeInfo opCode) {
			bool testDiscard = false;
			for (int i = 0; i < signature.ArgCount; i++) {
				var kind = signature.GetArgKind(i);
				if (kind == ArgKind.Memory) {
					testDiscard = true;
					break;
				}
			}

			if (testDiscard) {
				switch (GetOrigCodeValue(opCode.Code)) {
				case Code.Pextrb_r64m8_xmm_imm8:             // => Code.Pextrb_r32m8_xmm_imm8
				case Code.Extractps_r64m32_xmm_imm8:         // => Code.Extractps_rm32_xmm_imm8	
				case Code.Pinsrb_xmm_r64m8_imm8:             // => Code.Pinsrb_xmm_r32m8_imm8
				case Code.Movq_rm64_xmm:                     // => Code.Movq_xmmm64_xmm
				case Code.Movq_rm64_mm:                      // => Code.Movq_mmm64_mm
				case Code.Movq_xmm_rm64:                     // => Code.Movq_xmm_rm64
				case Code.Movq_mm_rm64:                      // => Code.Movq_mm_rm64					
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

		static int GetBitness(LegacyOpCodeInfo legacy) {
			int bitness = 64;
			var sizeFlags = legacy.Flags & (OpCodeFlags.Mode16 | OpCodeFlags.Mode32 | OpCodeFlags.Mode64);
			var operandSize = legacy.OperandSize == OperandSize.None ? (OperandSize)legacy.AddressSize : legacy.OperandSize;
			switch (sizeFlags) {
			case OpCodeFlags.Mode16 | OpCodeFlags.Mode32:
				bitness = operandSize == OperandSize.None || operandSize == OperandSize.Size16 ? 16 : 32;
				break;
			case OpCodeFlags.Mode16 | OpCodeFlags.Mode32 | OpCodeFlags.Mode64:
				bitness = operandSize == OperandSize.Size16 ? 16 : 32;
				break;
			case OpCodeFlags.Mode64:
				bitness = operandSize == OperandSize.Size16 ? 16 : operandSize == OperandSize.Size32 ? 32 : 64;
				break;
			default:
				throw new InvalidOperationException();
			}
			return bitness;
		}

		List<int> CollectByOperandKindPredicate(List<OpCodeInfo> opcodes, Func<OpCodeOperandKind, bool?> predicate, List<OpCodeInfo> opcodesMatchingPredicate, List<OpCodeInfo> opcodesNotMatchingPredicate) {
			var argIndices = new List<int>();
			foreach (var opCodeInfo in opcodes) {
				var selected = opcodesNotMatchingPredicate;
				for (int i = 0; i < opCodeInfo.OpKindsLength; i++) {
					var argOpKind = GetOperandKind(encoderTypes, opCodeInfo, i);
					var result = predicate(argOpKind);
					if (result.HasValue) {
						if (result.Value) {
							if (!argIndices.Contains(i)) {
								argIndices.Add(i);
							}
							selected = opcodesMatchingPredicate;
						}
						break;
					}
				}
				selected.Add(opCodeInfo);
			}

			return argIndices;
		}

		static bool? IsBranchShort(OpCodeOperandKind kind) {
			switch (kind) {
			case OpCodeOperandKind.br32_4:
			case OpCodeOperandKind.br64_4:
			case OpCodeOperandKind.xbegin_4:
			case OpCodeOperandKind.brdisp_4:
			case OpCodeOperandKind.br16_2:
				return false;

			case OpCodeOperandKind.xbegin_2:
			case OpCodeOperandKind.brdisp_2:
			case OpCodeOperandKind.br16_1:
			case OpCodeOperandKind.br32_1:
			case OpCodeOperandKind.br64_1:
				return true;
			}

			return null;
		}

		static bool? IsImmediateByteSigned(OpCodeOperandKind kind) {
			switch (kind) {
			case OpCodeOperandKind.imm8:
			case OpCodeOperandKind.imm8sex16:
			case OpCodeOperandKind.imm8sex32:
			case OpCodeOperandKind.imm8sex64:
				return true;
			}

			return null;
		}

		static bool? IsImmediateByteEqual1(OpCodeOperandKind kind) {
			switch (kind) {
			case OpCodeOperandKind.imm8_const_1:
				return true;
			}

			return null;
		}

		OrderedSelectorList BuildSelectorsByRegisterOrMemory(Signature signature, OpCodeArgFlags argFlags, List<OpCodeInfo> opcodes, bool isRegister) {
			List<OrderedSelectorList>? selectorsList = null;
			for (int argIndex = 0; argIndex < signature.ArgCount; argIndex++) {
				var argKind = signature.GetArgKind(argIndex);
				if (isRegister && !IsRegister(argKind) || !isRegister && (argKind != ArgKind.Memory)) {
					continue;
				}

				var selectors = new OrderedSelectorList() { ArgIndex = argIndex };
				foreach (var opCodeInfo in opcodes) {
					var argOpKind = GetOperandKind(encoderTypes, opCodeInfo, argIndex);
					var conditionKind = GetSelectorKindForRegisterOrMemory(opCodeInfo, argOpKind, (argFlags & OpCodeArgFlags.HasRegisterMemoryMappedToRegister) != 0);
					selectors.Add(conditionKind, opCodeInfo);
				}

				// If we have already found the best selector, we can return immediately
				if (selectors.Count == opcodes.Count) {
					return selectors;
				}

				if (selectorsList is null) selectorsList = new List<OrderedSelectorList>();
				selectorsList.Add(selectors);
			}

			if (selectorsList is null) return new OrderedSelectorList();

			// Select the biggest selector
			return selectorsList.First(x => x.Count == selectorsList.Max(x => x.Count));
		}

		static OpCodeOperandKind GetOperandKind(EncoderTypes encoderTypes, OpCodeInfo opCodeInfo, int index) =>
			opCodeInfo.OpKind(encoderTypes, index);

		static bool IsRegister(ArgKind kind) {
			switch (kind) {
			case ArgKind.Register8:
			case ArgKind.Register16:
			case ArgKind.Register32:
			case ArgKind.Register64:
			case ArgKind.RegisterK:
			case ArgKind.RegisterST:
			case ArgKind.RegisterSegment:
			case ArgKind.RegisterBND:
			case ArgKind.RegisterMM:
			case ArgKind.RegisterXMM:
			case ArgKind.RegisterYMM:
			case ArgKind.RegisterZMM:
			case ArgKind.RegisterCR:
			case ArgKind.RegisterDR:
			case ArgKind.RegisterTR:
				return true;
			}
			return false;
		}

		protected bool IsMoffs(OpCodeInfo opCodeInfo) {
			// Special case for moffs
			switch (GetOrigCodeValue(opCodeInfo.Code)) {
			case Code.Mov_AL_moffs8:
			case Code.Mov_AX_moffs16:
			case Code.Mov_EAX_moffs32:
			case Code.Mov_RAX_moffs64:
			case Code.Mov_moffs8_AL:
			case Code.Mov_moffs16_AX:
			case Code.Mov_moffs32_EAX:
			case Code.Mov_moffs64_RAX:
				return true;
			}

			return false;
		}

		int GetImmediateSizeInBits(OpCodeInfo opCodeInfo) {
			switch (opCodeInfo.OpKind(encoderTypes, 0)) {
			case OpCodeOperandKind.imm2_m2z:
				return 2;
			case OpCodeOperandKind.imm8:
				return 8;
			case OpCodeOperandKind.imm8_const_1:
				return 0;
			case OpCodeOperandKind.imm8sex16:
			case OpCodeOperandKind.imm16:
				return 16;
			case OpCodeOperandKind.imm8sex32:
			case OpCodeOperandKind.imm32:
				return 32;
			case OpCodeOperandKind.imm32sex64:
			case OpCodeOperandKind.imm64:
				return 64;
			default:
				return 0;
			}
		}

		static int GetMemoryAddressSizeInBits(MemorySizeInfoTable memorySizeInfoTable, InstructionDef[] defs, OpCodeInfo opCodeInfo) {
			var memSize = (MemorySize)defs[opCodeInfo.Code.Value].Mem.Value;
			switch (memSize) {
			case MemorySize.Fword6:
				memSize = MemorySize.UInt32;
				break;
			case MemorySize.Fword10:
				memSize = MemorySize.UInt64;
				break;
			}
			var addressSize = memorySizeInfoTable.Data[(int)memSize].Size;
			return addressSize * 8;
		}

		[DebuggerDisplay("Count = {" + nameof(Count) + "}")]
		private class OrderedSelectorList : List<(OpCodeSelectorKind, List<OpCodeInfo>)> {
			public OrderedSelectorList() {
				ArgIndex = -1;
			}

			public int ArgIndex { get; set; }

			public void Add(OpCodeSelectorKind kindToAdd, OpCodeInfo opCodeInfo) {
				foreach (var (kind, list) in this) {
					if (kind == kindToAdd) {
						if (!list.Contains(opCodeInfo)) {
							list.Add(opCodeInfo);
						}
						return;
					}
				}
				Add((kindToAdd, new List<OpCodeInfo>(1) { opCodeInfo }));
			}
		}


		static int GetPriorityFromKind(OpCodeOperandKind kind, int addressSize) {
			switch (kind) {

			case OpCodeOperandKind.zmm_reg:
			case OpCodeOperandKind.zmm_vvvv:
			case OpCodeOperandKind.zmm_rm:
			case OpCodeOperandKind.zmm_or_mem:
			case OpCodeOperandKind.zmmp3_vvvv:
				return -30;

			case OpCodeOperandKind.ymm_reg:
			case OpCodeOperandKind.ymm_is4:
			case OpCodeOperandKind.ymm_is5:
			case OpCodeOperandKind.ymm_vvvv:
			case OpCodeOperandKind.ymm_rm:
			case OpCodeOperandKind.ymm_or_mem:
				return -20;

			case OpCodeOperandKind.xmm_is4:
			case OpCodeOperandKind.xmm_is5:
			case OpCodeOperandKind.xmm_reg:
			case OpCodeOperandKind.xmm_vvvv:
			case OpCodeOperandKind.xmmp3_vvvv:
			case OpCodeOperandKind.xmm_rm:
			case OpCodeOperandKind.xmm_or_mem:
				return -10;

			case OpCodeOperandKind.rax:
			case OpCodeOperandKind.st0:
				return 0;

			case OpCodeOperandKind.r64_opcode:
			case OpCodeOperandKind.r64_vvvv:
			case OpCodeOperandKind.r64_reg:
			case OpCodeOperandKind.r64_or_mem:
			case OpCodeOperandKind.r64_or_mem_mpx:
			case OpCodeOperandKind.imm32sex64:
			case OpCodeOperandKind.imm64:
			case OpCodeOperandKind.r64_rm:
			case OpCodeOperandKind.r64_reg_mem:
				return 10;

			case OpCodeOperandKind.eax:
				return 15;

			case OpCodeOperandKind.sti_opcode:
			case OpCodeOperandKind.r32_opcode:
			case OpCodeOperandKind.r32_vvvv:
			case OpCodeOperandKind.r32_reg:
			case OpCodeOperandKind.r32_or_mem:
			case OpCodeOperandKind.r32_or_mem_mpx:
			case OpCodeOperandKind.imm32:
			case OpCodeOperandKind.r32_rm:
			case OpCodeOperandKind.r32_reg_mem:
				return 20;

			case OpCodeOperandKind.ax:
			case OpCodeOperandKind.dx:
				return 25;

			case OpCodeOperandKind.r16_opcode:
			case OpCodeOperandKind.r16_reg:
			case OpCodeOperandKind.r16_or_mem:
			case OpCodeOperandKind.r16_reg_mem:
			case OpCodeOperandKind.r16_rm:
			case OpCodeOperandKind.imm16:
				return 30;

			case OpCodeOperandKind.imm8_const_1:
				return 40;

			case OpCodeOperandKind.al:
			case OpCodeOperandKind.cl:
				return 45;

			case OpCodeOperandKind.r8_opcode:
			case OpCodeOperandKind.r8_reg:
			case OpCodeOperandKind.r8_or_mem:
			case OpCodeOperandKind.imm8:
			case OpCodeOperandKind.imm8sex16:
			case OpCodeOperandKind.imm8sex32:
			case OpCodeOperandKind.imm8sex64:
				return 50;

			case OpCodeOperandKind.mem:
			case OpCodeOperandKind.mem_offs:
			case OpCodeOperandKind.mem_mpx:
			case OpCodeOperandKind.mem_mib:
			case OpCodeOperandKind.mem_vsib32z:
			case OpCodeOperandKind.mem_vsib64z:
			case OpCodeOperandKind.mem_vsib32y:
			case OpCodeOperandKind.mem_vsib64y:
			case OpCodeOperandKind.mem_vsib32x:
			case OpCodeOperandKind.mem_vsib64x:
				switch (addressSize) {
				case 80:
					return 05;
				case 64:
					return 10;
				case 48:
					return 15;
				case 32:
					return 20;
				case 16:
					return 30;
				case 8:
					return 50;
				}
				return int.MaxValue;

			case OpCodeOperandKind.seg_reg:
				return 60;

			default:


				return int.MaxValue;
			}
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
			IsBroadcastXYZ = 1 << 16,
			HasLabelUlong = 1 << 17,
			HasImmediateByte = 1 << 18,
			UnsignedUIntNotSupported = 1 << 19,
			HasImmediateUnsigned = 1 << 20,
		}

		void FilterOpCodesRegister(OpCodeInfoGroup @group, List<OpCodeInfo> inputOpCodes, List<OpCodeInfo> opcodes, HashSet<Signature> signatures, bool allowMemory) {

			var bitnessFlags = OpCodeFlags.None;
			var vexOrEvexFlags = OpCodeArgFlags.Default;

			foreach (var code in inputOpCodes) {
				var registerSignature = new Signature();
				bool isValid = true;
				for (int i = 0; i < code.OpKindsLength; i++) {
					var argKind = GetFilterRegisterKindFromOpKind(GetOperandKind(encoderTypes, code, i), GetMemoryAddressSizeInBits(memorySizeInfoTable, defs, code), allowMemory);
					if (argKind == ArgKind.Unknown) {
						isValid = false;
						break;
					}

					registerSignature.AddArgKind(argKind);
				}

				var codeBitnessFlags = code.Flags & BitnessMaskFlags;
				var codeEvexFlags = code is VexOpCodeInfo ? OpCodeArgFlags.HasVex : code is EvexOpCodeInfo ? OpCodeArgFlags.HasEvex : OpCodeArgFlags.Default;

				if (isValid && (signatures.Add(registerSignature) || ((bitnessFlags & codeBitnessFlags) != codeBitnessFlags) || (codeEvexFlags != OpCodeArgFlags.Default && (vexOrEvexFlags & codeEvexFlags) == 0) || (group.Flags & (OpCodeArgFlags.RoundingControl | OpCodeArgFlags.SuppressAllExceptions)) != 0)) {
					bitnessFlags |= codeBitnessFlags;
					vexOrEvexFlags |= codeEvexFlags;
					if (!opcodes.Contains(code)) {
						opcodes.Add(code);
					}
				}
			}
		}

		private ArgKind GetFilterRegisterKindFromOpKind(OpCodeOperandKind opKind, int addressSize, bool allowMemory) {
			switch (opKind) {
			case OpCodeOperandKind.st0:
			case OpCodeOperandKind.sti_opcode:
				return ArgKind.RegisterST;

			case OpCodeOperandKind.r8_opcode:
			case OpCodeOperandKind.r8_reg:
				return ArgKind.Register8;
			case OpCodeOperandKind.r16_opcode:
			case OpCodeOperandKind.r16_reg:
			case OpCodeOperandKind.r16_reg_mem:
			case OpCodeOperandKind.r16_rm:
				return ArgKind.Register16;
			case OpCodeOperandKind.r32_opcode:
			case OpCodeOperandKind.r32_vvvv:
			case OpCodeOperandKind.r32_reg:
			case OpCodeOperandKind.r32_rm:
			case OpCodeOperandKind.r32_reg_mem:
				return ArgKind.Register32;
			case OpCodeOperandKind.r64_opcode:
			case OpCodeOperandKind.r64_vvvv:
			case OpCodeOperandKind.r64_reg:
			case OpCodeOperandKind.r64_rm:
			case OpCodeOperandKind.r64_reg_mem:
				return ArgKind.Register64;

			case OpCodeOperandKind.imm8_const_1:
				return ArgKind.FilterImmediate1;

			case OpCodeOperandKind.imm2_m2z:
				return ArgKind.FilterImmediate2;

			case OpCodeOperandKind.imm8:
			case OpCodeOperandKind.imm8sex16:
			case OpCodeOperandKind.imm8sex32:
			case OpCodeOperandKind.imm8sex64:
				return ArgKind.FilterImmediate8;

			case OpCodeOperandKind.imm16:
			case OpCodeOperandKind.imm32:
			case OpCodeOperandKind.imm32sex64:
			case OpCodeOperandKind.imm64:
				return ArgKind.Immediate;

			case OpCodeOperandKind.seg_reg:
				return ArgKind.RegisterSegment;

			case OpCodeOperandKind.dx:
				return ArgKind.FilterRegisterDX;
			case OpCodeOperandKind.cl:
				return ArgKind.FilterRegisterCL;
			case OpCodeOperandKind.al:
				return ArgKind.FilterRegisterAL;
			case OpCodeOperandKind.ax:
				return ArgKind.FilterRegisterAX;
			case OpCodeOperandKind.eax:
				return ArgKind.FilterRegisterEAX;
			case OpCodeOperandKind.rax:
				return ArgKind.FilterRegisterRAX;

			case OpCodeOperandKind.es:
				return ArgKind.FilterRegisterES;
			case OpCodeOperandKind.cs:
				return ArgKind.FilterRegisterCS;
			case OpCodeOperandKind.ss:
				return ArgKind.FilterRegisterSS;
			case OpCodeOperandKind.ds:
				return ArgKind.FilterRegisterDS;
			case OpCodeOperandKind.fs:
				return ArgKind.FilterRegisterFS;
			case OpCodeOperandKind.gs:
				return ArgKind.FilterRegisterGS;

			case OpCodeOperandKind.bnd_reg:
				return ArgKind.RegisterBND;

			case OpCodeOperandKind.cr_reg:
				return ArgKind.RegisterCR;
			case OpCodeOperandKind.tr_reg:
				return ArgKind.RegisterTR;
			case OpCodeOperandKind.dr_reg:
				return ArgKind.RegisterDR;

			case OpCodeOperandKind.k_reg:
			case OpCodeOperandKind.kp1_reg:
			case OpCodeOperandKind.k_rm:
			case OpCodeOperandKind.k_vvvv:
				return ArgKind.RegisterK;
			case OpCodeOperandKind.xmm_is4:
			case OpCodeOperandKind.xmm_is5:
			case OpCodeOperandKind.xmm_reg:
			case OpCodeOperandKind.xmm_vvvv:
			case OpCodeOperandKind.xmmp3_vvvv:
			case OpCodeOperandKind.xmm_rm:
				return ArgKind.RegisterXMM;
			case OpCodeOperandKind.ymm_is4:
			case OpCodeOperandKind.ymm_is5:
			case OpCodeOperandKind.ymm_reg:
			case OpCodeOperandKind.ymm_vvvv:
			case OpCodeOperandKind.ymm_rm:
				return ArgKind.RegisterYMM;
			case OpCodeOperandKind.zmm_reg:
			case OpCodeOperandKind.zmm_vvvv:
			case OpCodeOperandKind.zmm_rm:
			case OpCodeOperandKind.zmmp3_vvvv:
				return ArgKind.RegisterZMM;
			case OpCodeOperandKind.mm_reg:
			case OpCodeOperandKind.mm_rm:
				return ArgKind.RegisterMM;
			}

			if (allowMemory) {
				switch (opKind) {
				case OpCodeOperandKind.bnd_or_mem_mpx:
					return ArgKind.RegisterBND;
				case OpCodeOperandKind.r8_or_mem:
					return ArgKind.Register8;
				case OpCodeOperandKind.r16_or_mem:
					return ArgKind.Register16;
				case OpCodeOperandKind.r32_or_mem:
				case OpCodeOperandKind.r32_or_mem_mpx:
					return ArgKind.Register32;
				case OpCodeOperandKind.r64_or_mem:
				case OpCodeOperandKind.r64_or_mem_mpx:
					return ArgKind.Register64;
				case OpCodeOperandKind.mm_or_mem:
					return ArgKind.RegisterMM;
				case OpCodeOperandKind.k_or_mem:
					return ArgKind.RegisterK;
				case OpCodeOperandKind.xmm_or_mem:
					return ArgKind.RegisterXMM;
				case OpCodeOperandKind.ymm_or_mem:
					return ArgKind.RegisterYMM;
				case OpCodeOperandKind.zmm_or_mem:
					return ArgKind.RegisterZMM;
				case OpCodeOperandKind.mem:
				case OpCodeOperandKind.mem_offs:
				case OpCodeOperandKind.mem_mpx:
				case OpCodeOperandKind.mem_mib:
					switch (addressSize) {
					case 64:
						return ArgKind.Register64;
					case 32:
						return ArgKind.Register32;
					case 16:
						return ArgKind.Register16;
					case 8:
						return ArgKind.Register8;
					}
					break;
				}
			}

			return ArgKind.Unknown;
		}

		protected static bool IsSegmentRegister(OpCodeOperandKind kind) {
			switch (kind) {
			case OpCodeOperandKind.es:
			case OpCodeOperandKind.cs:
			case OpCodeOperandKind.fs:
			case OpCodeOperandKind.gs:
			case OpCodeOperandKind.ss:
			case OpCodeOperandKind.ds:
				return true;
			}

			return false;
		}

		protected OpCodeSelectorKind GetSelectorKindForRegisterOrMemory(OpCodeInfo opCodeInfo, OpCodeOperandKind opKind, bool returnMemoryAsRegister) {

			switch (opKind) {

			case OpCodeOperandKind.mem:
			case OpCodeOperandKind.mem_offs:
			case OpCodeOperandKind.mem_mpx:
			case OpCodeOperandKind.mem_mib: {
				return GetOpCodeSelectorKindForMemory(opCodeInfo, OpCodeSelectorKind.Memory);
			}

			case OpCodeOperandKind.mem_vsib32x:
				return OpCodeSelectorKind.MemoryIndex32Xmm;

			case OpCodeOperandKind.mem_vsib32y:
				return OpCodeSelectorKind.MemoryIndex32Ymm;

			case OpCodeOperandKind.mem_vsib32z:
				return OpCodeSelectorKind.MemoryIndex32Zmm;

			case OpCodeOperandKind.mem_vsib64x:
				return OpCodeSelectorKind.MemoryIndex64Xmm;

			case OpCodeOperandKind.mem_vsib64y:
				return OpCodeSelectorKind.MemoryIndex64Ymm;

			case OpCodeOperandKind.mem_vsib64z:
				return OpCodeSelectorKind.MemoryIndex64Zmm;

			case OpCodeOperandKind.r8_or_mem:
				return returnMemoryAsRegister ? OpCodeSelectorKind.Register8 : GetOpCodeSelectorKindForMemory(opCodeInfo, OpCodeSelectorKind.Memory8);

			case OpCodeOperandKind.r16_or_mem:
				return returnMemoryAsRegister ? OpCodeSelectorKind.Register16 : GetOpCodeSelectorKindForMemory(opCodeInfo, OpCodeSelectorKind.Memory16);

			case OpCodeOperandKind.r32_or_mem:
			case OpCodeOperandKind.r32_or_mem_mpx:
				return returnMemoryAsRegister ? OpCodeSelectorKind.Register32 : GetOpCodeSelectorKindForMemory(opCodeInfo, OpCodeSelectorKind.Memory32);

			case OpCodeOperandKind.r64_or_mem:
			case OpCodeOperandKind.r64_or_mem_mpx:
				return returnMemoryAsRegister ? OpCodeSelectorKind.Register64 : GetOpCodeSelectorKindForMemory(opCodeInfo, OpCodeSelectorKind.Memory64);

			case OpCodeOperandKind.mm_or_mem:
				return returnMemoryAsRegister ? OpCodeSelectorKind.RegisterMM : GetOpCodeSelectorKindForMemory(opCodeInfo, OpCodeSelectorKind.MemoryMM);

			case OpCodeOperandKind.xmm_or_mem:
				return returnMemoryAsRegister ? OpCodeSelectorKind.RegisterXMM : GetOpCodeSelectorKindForMemory(opCodeInfo, OpCodeSelectorKind.MemoryXMM);

			case OpCodeOperandKind.ymm_or_mem:
				return returnMemoryAsRegister ? OpCodeSelectorKind.RegisterYMM : GetOpCodeSelectorKindForMemory(opCodeInfo, OpCodeSelectorKind.MemoryYMM);

			case OpCodeOperandKind.zmm_or_mem:
				return returnMemoryAsRegister ? OpCodeSelectorKind.RegisterZMM : GetOpCodeSelectorKindForMemory(opCodeInfo, OpCodeSelectorKind.MemoryZMM);

			case OpCodeOperandKind.bnd_or_mem_mpx:
				return returnMemoryAsRegister ? OpCodeSelectorKind.RegisterBND : GetOpCodeSelectorKindForMemory(opCodeInfo, OpCodeSelectorKind.Memory64);

			case OpCodeOperandKind.k_or_mem:
				return returnMemoryAsRegister ? OpCodeSelectorKind.RegisterK : GetOpCodeSelectorKindForMemory(opCodeInfo, OpCodeSelectorKind.Memory);

			case OpCodeOperandKind.r8_reg:
			case OpCodeOperandKind.r8_opcode:
				return OpCodeSelectorKind.Register8;

			case OpCodeOperandKind.r16_reg:
			case OpCodeOperandKind.r16_rm:
			case OpCodeOperandKind.r16_opcode:
			case OpCodeOperandKind.r16_reg_mem:
				return OpCodeSelectorKind.Register16;

			case OpCodeOperandKind.r32_reg:
			case OpCodeOperandKind.r32_rm:
			case OpCodeOperandKind.r32_opcode:
			case OpCodeOperandKind.r32_vvvv:
			case OpCodeOperandKind.r32_reg_mem:
				return OpCodeSelectorKind.Register32;

			case OpCodeOperandKind.r64_reg:
			case OpCodeOperandKind.r64_rm:
			case OpCodeOperandKind.r64_opcode:
			case OpCodeOperandKind.r64_vvvv:
			case OpCodeOperandKind.r64_reg_mem:
				return OpCodeSelectorKind.Register64;

			case OpCodeOperandKind.seg_reg:
				return OpCodeSelectorKind.RegisterSegment;

			case OpCodeOperandKind.k_reg:
			case OpCodeOperandKind.kp1_reg:
			case OpCodeOperandKind.k_rm:
			case OpCodeOperandKind.k_vvvv:
				return OpCodeSelectorKind.RegisterK;

			case OpCodeOperandKind.mm_reg:
			case OpCodeOperandKind.mm_rm:
				return OpCodeSelectorKind.RegisterMM;

			case OpCodeOperandKind.xmm_reg:
			case OpCodeOperandKind.xmm_rm:
			case OpCodeOperandKind.xmm_vvvv:
			case OpCodeOperandKind.xmmp3_vvvv:
			case OpCodeOperandKind.xmm_is4:
			case OpCodeOperandKind.xmm_is5:
				return OpCodeSelectorKind.RegisterXMM;

			case OpCodeOperandKind.ymm_reg:
			case OpCodeOperandKind.ymm_rm:
			case OpCodeOperandKind.ymm_vvvv:
			case OpCodeOperandKind.ymm_is4:
			case OpCodeOperandKind.ymm_is5:
				return OpCodeSelectorKind.RegisterYMM;

			case OpCodeOperandKind.zmm_reg:
			case OpCodeOperandKind.zmm_rm:
			case OpCodeOperandKind.zmm_vvvv:
			case OpCodeOperandKind.zmmp3_vvvv:
				return OpCodeSelectorKind.RegisterZMM;

			case OpCodeOperandKind.cr_reg:
				return OpCodeSelectorKind.RegisterCR;

			case OpCodeOperandKind.dr_reg:
				return OpCodeSelectorKind.RegisterDR;

			case OpCodeOperandKind.tr_reg:
				return OpCodeSelectorKind.RegisterTR;

			case OpCodeOperandKind.bnd_reg:
				return OpCodeSelectorKind.RegisterBND;

			case OpCodeOperandKind.es:
				return OpCodeSelectorKind.RegisterES;

			case OpCodeOperandKind.cs:
				return OpCodeSelectorKind.RegisterCS;

			case OpCodeOperandKind.ss:
				return OpCodeSelectorKind.RegisterSS;

			case OpCodeOperandKind.ds:
				return OpCodeSelectorKind.RegisterDS;

			case OpCodeOperandKind.fs:
				return OpCodeSelectorKind.RegisterFS;

			case OpCodeOperandKind.gs:
				return OpCodeSelectorKind.RegisterGS;

			case OpCodeOperandKind.al:
				return OpCodeSelectorKind.RegisterAL;

			case OpCodeOperandKind.cl:
				return OpCodeSelectorKind.RegisterCL;

			case OpCodeOperandKind.ax:
				return OpCodeSelectorKind.RegisterAX;

			case OpCodeOperandKind.dx:
				return OpCodeSelectorKind.RegisterDX;

			case OpCodeOperandKind.eax:
				return OpCodeSelectorKind.RegisterEAX;

			case OpCodeOperandKind.rax:
				return OpCodeSelectorKind.RegisterRAX;

			case OpCodeOperandKind.st0:
				return OpCodeSelectorKind.RegisterST0;
			case OpCodeOperandKind.sti_opcode:
				return OpCodeSelectorKind.RegisterST;

			case OpCodeOperandKind.seg_rSI:
			case OpCodeOperandKind.es_rDI:
			case OpCodeOperandKind.seg_rDI:
			case OpCodeOperandKind.seg_rBX_al:
				return OpCodeSelectorKind.Memory;

			default:
				throw new ArgumentOutOfRangeException(nameof(opKind), opKind, null);
			}
		}

		OpCodeSelectorKind GetOpCodeSelectorKindForMemory(OpCodeInfo opCodeInfo, OpCodeSelectorKind defaultMemory) {
			var memSize = (MemorySize)defs[opCodeInfo.Code.Value].Mem.Value;
			switch (memSize) {
			case MemorySize.Fword6:
				memSize = MemorySize.UInt32;
				break;
			case MemorySize.Fword10:
				memSize = MemorySize.UInt64;
				break;
			}

			var addressSize = 8 * memorySizeInfoTable.Data[(int)memSize].Size;
			switch (addressSize) {
			case 512:
				return OpCodeSelectorKind.MemoryZMM;
			case 256:
				return OpCodeSelectorKind.MemoryYMM;
			case 128:
				return OpCodeSelectorKind.MemoryXMM;
			case 80:
				return OpCodeSelectorKind.Memory80;
			case 64:
				return OpCodeSelectorKind.Memory64;
			case 48:
				return OpCodeSelectorKind.Memory48;
			case 32:
				return OpCodeSelectorKind.Memory32;
			case 16:
				return OpCodeSelectorKind.Memory16;
			case 8:
				return OpCodeSelectorKind.Memory8;
			}

			return defaultMemory;
		}

		OpCodeInfoGroup AddOpCodeToGroup(string name, string memoName, Signature signature, OpCodeInfo code, OpCodeArgFlags opCodeArgFlags, PseudoOpsKind? pseudoOpsKind, int numberLeadingArgToDiscard, List<int> argSizes, bool isOtherImmediate) {
			var key = new GroupKey(name, signature);
			if (!groups.TryGetValue(key, out var group)) {
				group = new OpCodeInfoGroup(encoderTypes, memorySizeInfoTable, defs, name, signature);
				group.MemoName = memoName;
				groups.Add(key, group);
			}

			if (!group.Items.Contains(code)) {
				group.Items.Add(code);
			}
			group.Flags |= opCodeArgFlags;
			group.AllOpCodeFlags |= code.Flags;

			// Handle pseudo ops
			if (group.RootPseudoOpsKind is object) {
				Debug.Assert(pseudoOpsKind is object);
				Debug.Assert(group.RootPseudoOpsKind.Value == pseudoOpsKind.Value);
				Debug.Assert(groupsWithPseudo.ContainsKey(key));
			}
			else {
				group.RootPseudoOpsKind = pseudoOpsKind;
				if (pseudoOpsKind.HasValue) {
					if (!groupsWithPseudo.ContainsKey(key)) {
						groupsWithPseudo.Add(key, group);
					}
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

				if (signature != signatureWithOtherImmediate) {
					AddOpCodeToGroup(name, memoName, signatureWithOtherImmediate, code, opCodeArgFlags | OpCodeArgFlags.HasImmediateUnsigned, null, numberLeadingArgToDiscard, argSizes, true);
				}
			}

			if (!pseudoOpsKind.HasValue && (opCodeArgFlags & OpCodeArgFlags.HasRegisterMemoryMappedToRegister) == 0) {
				var broadcastName = RenameAmbiguousBroadcasts(name, code);
				if ((opCodeArgFlags & OpCodeArgFlags.IsBroadcastXYZ) == 0 && broadcastName != name) {
					group.Flags |= OpCodeArgFlags.HasAmbiguousBroadcast;
					AddOpCodeToGroup(broadcastName, memoName, signature, code, opCodeArgFlags | OpCodeArgFlags.IsBroadcastXYZ, null, numberLeadingArgToDiscard, argSizes, isOtherImmediate);
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
				AddOpCodeToGroup(name, memoName, newLabelULongSignature, code, opCodeArgFlags | OpCodeArgFlags.HasLabelUlong, null, numberLeadingArgToDiscard, argSizes, isOtherImmediate);
			}

			return group;
		}

		protected static bool IsArgKindImmediate(ArgKind argKind) {
			switch (argKind) {
			case ArgKind.Immediate:
			case ArgKind.ImmediateUnsigned:
				return true;
			}

			return false;
		}

		void CreatePseudoInstructions() {
			foreach (var group in groupsWithPseudo.Values) {
				var pseudo = group.RootPseudoOpsKind ?? throw new InvalidOperationException("Root cannot be null");
				var pseudoNames = FormatterConstants.GetPseudoOps(pseudo);

				// Create new signature with last imm argument
				var signature = new Signature();
				for (int i = 0; i < group.Signature.ArgCount - 1; i++) {
					signature.AddArgKind(group.Signature.GetArgKind(i));
				}

				for (int i = 0; i < pseudoNames.Length; i++) {
					var name = pseudoNames[i];
					var key = new GroupKey(name, signature);

					var imm = i;
					switch (name) {
					case "pclmullqhqdq":
					case "vpclmullqhqdq":
						imm = 0x10;
						break;
					case "pclmulhqhqdq":
					case "vpclmulhqhqdq":
						imm = 0x11;
						break;
					}

					var newGroup = new OpCodeInfoGroup(encoderTypes, memorySizeInfoTable, defs, name, signature) {
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

		[DebuggerDisplay("{" + nameof(Name) + "} {" + nameof(Kind) + "}")]
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

		protected class OpCodeInfoGroup {
			readonly EncoderTypes encoderTypes;
			readonly MemorySizeInfoTable memorySizeInfoTable;
			readonly InstructionDef[] defs;

			public OpCodeInfoGroup(EncoderTypes encoderTypes, MemorySizeInfoTable memorySizeInfoTable, InstructionDef[] defs, string name, Signature signature) {
				this.encoderTypes = encoderTypes;
				this.memorySizeInfoTable = memorySizeInfoTable;
				this.defs = defs;
				Name = name;
				MemoName = name;
				Signature = signature;
				Items = new List<OpCodeInfo>();
				MaxArgSizes = new List<int>();
			}

			public string MemoName { get; set; }
			public string Name { get; }
			public OpCodeFlags AllOpCodeFlags { get; set; }
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
			public List<OpCodeInfo> Items { get; }
			public List<int> MaxArgSizes { get; }
			public int NumberOfLeadingArgToDiscard { get; set; }

			public void UpdateMaxArgSizes(List<int> argSizes) {
				if (MaxArgSizes.Count == 0) {
					MaxArgSizes.AddRange(argSizes);
				}
				Debug.Assert(MaxArgSizes.Count == argSizes.Count);
				for (int i = 0; i < MaxArgSizes.Count; i++) {
					if (MaxArgSizes[i] < argSizes[i]) {
						MaxArgSizes[i] = argSizes[i];
					}
				}
			}

			public int OrderOpCodesPerOpKindPriority(OpCodeInfo x, OpCodeInfo y) {
				Debug.Assert(x.OpKindsLength == y.OpKindsLength);
				int result;
				for (int i = 0; i < x.OpKindsLength; i++) {
					if (!IsRegister(Signature.GetArgKind(i))) continue;
					result = GetPriorityFromKind(GetOperandKind(encoderTypes, x, i), GetMemoryAddressSizeInBits(memorySizeInfoTable, defs, x)).CompareTo(GetPriorityFromKind(GetOperandKind(encoderTypes, y, i), GetMemoryAddressSizeInBits(memorySizeInfoTable, defs, y)));
					if (result != 0) return result;
				}

				for (int i = 0; i < x.OpKindsLength; i++) {
					if (IsRegister(Signature.GetArgKind(i))) continue;
					result = GetPriorityFromKind(GetOperandKind(encoderTypes, x, i), GetMemoryAddressSizeInBits(memorySizeInfoTable, defs, x)).CompareTo(GetPriorityFromKind(GetOperandKind(encoderTypes, y, i), GetMemoryAddressSizeInBits(memorySizeInfoTable, defs, y)));
					if (result != 0) return result;
				}

				// Case for ordering by decreasing bitness
				var xmemSize = (MemorySize)defs[x.Code.Value].Mem.Value;
				var ymemSize = (MemorySize)defs[y.Code.Value].Mem.Value;
				result = xmemSize.CompareTo(ymemSize);
				if (result == 0) {
					if (x is LegacyOpCodeInfo x1 && y is LegacyOpCodeInfo y1) {
						var xBitness = GetBitness(x1);
						var yBitness = GetBitness(y1);
						result = xBitness.CompareTo(yBitness);
					}
				}
				return -result;
			}
		}

		protected PseudoOpsKind? GetPseudoOpsKind(OpCodeInfoGroup group) {
			PseudoOpsKind? pseudoOpsKind = null;
			foreach (var opCodeInfo in group.Items) {
				var ctorInfos = intelCtorInfos[opCodeInfo.Code.Value];
				var enumValue = ctorInfos[ctorInfos.Length - 1] as EnumValue;
				if (enumValue is null || enumValue.DeclaringType.TypeId != TypeIds.PseudoOpsKind)
					break;

				pseudoOpsKind = (PseudoOpsKind)enumValue.Value;
			}

			return pseudoOpsKind;
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

		protected static bool IsMemOffs64Selector(OpCodeSelectorKind kind) {
			switch (kind) {
			case OpCodeSelectorKind.MemOffs64_RAX:
			case OpCodeSelectorKind.MemOffs64_EAX:
			case OpCodeSelectorKind.MemOffs64_AX:
			case OpCodeSelectorKind.MemOffs64_AL:
				return true;
			}

			return false;
		}

		protected static (OpCodeArgFlags, OpCodeArgFlags) GetIfElseContextFlags(OpCodeSelectorKind kind) {
			switch (kind) {
			case OpCodeSelectorKind.Vex:
				return (OpCodeArgFlags.HasVex, OpCodeArgFlags.HasEvex);
			case OpCodeSelectorKind.EvexBroadcastX:
			case OpCodeSelectorKind.EvexBroadcastY:
			case OpCodeSelectorKind.EvexBroadcastZ:
				return (OpCodeArgFlags.HasEvex | OpCodeArgFlags.HasBroadcast, OpCodeArgFlags.Default);
			case OpCodeSelectorKind.BranchShort:
				return (OpCodeArgFlags.HasBranchShort, OpCodeArgFlags.HasBranchNear);
			}

			return (OpCodeArgFlags.Default, OpCodeArgFlags.Default);
		}

		protected string GetRegisterPostfix(Register register) {
			switch (register) {
			case Register.AL:
			case Register.CL:
			case Register.DL:
			case Register.BL:
			case Register.AH:
			case Register.CH:
			case Register.DH:
			case Register.BH:
			case Register.SPL:
			case Register.BPL:
			case Register.SIL:
			case Register.DIL:
			case Register.R8L:
			case Register.R9L:
			case Register.R10L:
			case Register.R11L:
			case Register.R12L:
			case Register.R13L:
			case Register.R14L:
			case Register.R15L:
				return "8";
			case Register.AX:
			case Register.CX:
			case Register.DX:
			case Register.BX:
			case Register.SP:
			case Register.BP:
			case Register.SI:
			case Register.DI:
			case Register.R8W:
			case Register.R9W:
			case Register.R10W:
			case Register.R11W:
			case Register.R12W:
			case Register.R13W:
			case Register.R14W:
			case Register.R15W:
				return "16";
			case Register.EAX:
			case Register.ECX:
			case Register.EDX:
			case Register.EBX:
			case Register.ESP:
			case Register.EBP:
			case Register.ESI:
			case Register.EDI:
			case Register.R8D:
			case Register.R9D:
			case Register.R10D:
			case Register.R11D:
			case Register.R12D:
			case Register.R13D:
			case Register.R14D:
			case Register.R15D:
				return "32";
			case Register.RAX:
			case Register.RCX:
			case Register.RDX:
			case Register.RBX:
			case Register.RSP:
			case Register.RBP:
			case Register.RSI:
			case Register.RDI:
			case Register.R8:
			case Register.R9:
			case Register.R10:
			case Register.R11:
			case Register.R12:
			case Register.R13:
			case Register.R14:
			case Register.R15:
			case Register.EIP:
			case Register.RIP:
				return "64";
			case Register.ES:
			case Register.CS:
			case Register.SS:
			case Register.DS:
			case Register.FS:
			case Register.GS:
				return "Segment";
			case Register.XMM0:
			case Register.XMM1:
			case Register.XMM2:
			case Register.XMM3:
			case Register.XMM4:
			case Register.XMM5:
			case Register.XMM6:
			case Register.XMM7:
			case Register.XMM8:
			case Register.XMM9:
			case Register.XMM10:
			case Register.XMM11:
			case Register.XMM12:
			case Register.XMM13:
			case Register.XMM14:
			case Register.XMM15:
			case Register.XMM16:
			case Register.XMM17:
			case Register.XMM18:
			case Register.XMM19:
			case Register.XMM20:
			case Register.XMM21:
			case Register.XMM22:
			case Register.XMM23:
			case Register.XMM24:
			case Register.XMM25:
			case Register.XMM26:
			case Register.XMM27:
			case Register.XMM28:
			case Register.XMM29:
			case Register.XMM30:
			case Register.XMM31:
				return "XMM";
			case Register.YMM0:
			case Register.YMM1:
			case Register.YMM2:
			case Register.YMM3:
			case Register.YMM4:
			case Register.YMM5:
			case Register.YMM6:
			case Register.YMM7:
			case Register.YMM8:
			case Register.YMM9:
			case Register.YMM10:
			case Register.YMM11:
			case Register.YMM12:
			case Register.YMM13:
			case Register.YMM14:
			case Register.YMM15:
			case Register.YMM16:
			case Register.YMM17:
			case Register.YMM18:
			case Register.YMM19:
			case Register.YMM20:
			case Register.YMM21:
			case Register.YMM22:
			case Register.YMM23:
			case Register.YMM24:
			case Register.YMM25:
			case Register.YMM26:
			case Register.YMM27:
			case Register.YMM28:
			case Register.YMM29:
			case Register.YMM30:
			case Register.YMM31:
				return "YMM";
			case Register.ZMM0:
			case Register.ZMM1:
			case Register.ZMM2:
			case Register.ZMM3:
			case Register.ZMM4:
			case Register.ZMM5:
			case Register.ZMM6:
			case Register.ZMM7:
			case Register.ZMM8:
			case Register.ZMM9:
			case Register.ZMM10:
			case Register.ZMM11:
			case Register.ZMM12:
			case Register.ZMM13:
			case Register.ZMM14:
			case Register.ZMM15:
			case Register.ZMM16:
			case Register.ZMM17:
			case Register.ZMM18:
			case Register.ZMM19:
			case Register.ZMM20:
			case Register.ZMM21:
			case Register.ZMM22:
			case Register.ZMM23:
			case Register.ZMM24:
			case Register.ZMM25:
			case Register.ZMM26:
			case Register.ZMM27:
			case Register.ZMM28:
			case Register.ZMM29:
			case Register.ZMM30:
			case Register.ZMM31:
				return "ZMM";
			case Register.K0:
			case Register.K1:
			case Register.K2:
			case Register.K3:
			case Register.K4:
			case Register.K5:
			case Register.K6:
			case Register.K7:
				return "K";
			case Register.BND0:
			case Register.BND1:
			case Register.BND2:
			case Register.BND3:
				return "BND";
			case Register.CR0:
			case Register.CR1:
			case Register.CR2:
			case Register.CR3:
			case Register.CR4:
			case Register.CR5:
			case Register.CR6:
			case Register.CR7:
			case Register.CR8:
			case Register.CR9:
			case Register.CR10:
			case Register.CR11:
			case Register.CR12:
			case Register.CR13:
			case Register.CR14:
			case Register.CR15:
				return "CR";
			case Register.DR0:
			case Register.DR1:
			case Register.DR2:
			case Register.DR3:
			case Register.DR4:
			case Register.DR5:
			case Register.DR6:
			case Register.DR7:
			case Register.DR8:
			case Register.DR9:
			case Register.DR10:
			case Register.DR11:
			case Register.DR12:
			case Register.DR13:
			case Register.DR14:
			case Register.DR15:
				return "DR";
			case Register.ST0:
			case Register.ST1:
			case Register.ST2:
			case Register.ST3:
			case Register.ST4:
			case Register.ST5:
			case Register.ST6:
			case Register.ST7:
				return "ST";
			case Register.MM0:
			case Register.MM1:
			case Register.MM2:
			case Register.MM3:
			case Register.MM4:
			case Register.MM5:
			case Register.MM6:
			case Register.MM7:
				return "MM";
			case Register.TR0:
			case Register.TR1:
			case Register.TR2:
			case Register.TR3:
			case Register.TR4:
			case Register.TR5:
			case Register.TR6:
			case Register.TR7:
				return "TR";
			default:
				throw new ArgumentOutOfRangeException(nameof(register), register, null);
			}
		}

		private bool IsR64M16(OpCodeInfo opCodeInfo) {
			switch (GetOrigCodeValue(opCodeInfo.Code)) {
			case Code.Mov_r64m16_Sreg:
			case Code.Mov_Sreg_r64m16:
			case Code.Sldt_r64m16:
			case Code.Str_r64m16:
			case Code.Lldt_r64m16:
			case Code.Ltr_r64m16:
			case Code.Verr_r64m16:
			case Code.Verw_r64m16:
			case Code.Smsw_r64m16:
			case Code.Lmsw_r64m16:
			case Code.Lar_r64_r64m16:
			case Code.Lsl_r64_r64m16:
			case Code.Pinsrw_mm_r64m16_imm8:
			case Code.Pinsrw_xmm_r64m16_imm8:
			case Code.VEX_Vpinsrw_xmm_xmm_r64m16_imm8:
			case Code.EVEX_Vpinsrw_xmm_xmm_r64m16_imm8:
			case Code.Pextrw_r64m16_xmm_imm8:
			case Code.VEX_Vpextrw_r64m16_xmm_imm8:
			case Code.EVEX_Vpextrw_r64m16_xmm_imm8:
				return true;
			}

			return false;
		}

		protected bool IsAmbiguousBroadcast(OpCodeInfo opCodeInfo) {
			switch (GetOrigCodeValue(opCodeInfo.Code)) {
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

		string RenameAmbiguousBroadcasts(string name, OpCodeInfo opCodeInfo) {
			if ((opCodeInfo.Flags & OpCodeFlags.Broadcast) == 0) return name;

			if (IsAmbiguousBroadcast(opCodeInfo)) {
				for (int i = 0; i < opCodeInfo.OpKindsLength; i++) {
					var kind = GetOperandKind(encoderTypes, opCodeInfo, i);
					switch (kind) {
					case OpCodeOperandKind.xmm_or_mem:
						return $"{name}x";
					case OpCodeOperandKind.ymm_or_mem:
						return $"{name}y";
					case OpCodeOperandKind.zmm_or_mem:
						return $"{name}z";
					}
				}
			}

			return name;
		}

		protected readonly struct OpCodeNode {
			readonly object value;

			public OpCodeNode(OpCodeInfo opCodeInfo) {
				value = opCodeInfo;
			}

			public OpCodeNode(OpCodeSelector selector) {
				value = selector;
			}

			public bool IsEmpty => value is null;
			public OpCodeInfo? OpCodeInfo => value as OpCodeInfo;
			public OpCodeSelector? Selector => value as OpCodeSelector;
			public static implicit operator OpCodeNode(OpCodeInfo opCodeInfo) => new OpCodeNode(opCodeInfo);
			public static implicit operator OpCodeNode(OpCodeSelector selector) => new OpCodeNode(selector);
		}

		protected class OpCodeSelector {
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
			public bool IsConditionInlineable => IfTrue.OpCodeInfo is object && IfFalse.OpCodeInfo is object;
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
