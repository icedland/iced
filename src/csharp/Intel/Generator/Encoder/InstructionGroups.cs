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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Generator.Constants;
using Generator.Enums;
using Generator.Enums.Encoder;
using Generator.Tables;

namespace Generator.Encoder {
	enum InstructionOperand {
		Register,
		Memory,
		Imm32,
		Imm64,
	}

	sealed class InstructionGroup {
		public List<OpCodeInfo> OpCodes { get; }
		public InstructionOperand[] Operands { get; }
		public InstructionGroup(InstructionOperand[] operands) {
			Operands = operands;
			OpCodes = new List<OpCodeInfo>();
		}
		public override string ToString() {
			var sb = new StringBuilder();
			sb.Append(OpCodes[0].Code.RawName);
			sb.Append(" - (");
			for (int i = 0; i < Operands.Length; i++) {
				if (i > 0)
					sb.Append(", ");
				sb.Append(Operands[i].ToString());
			}
			sb.Append(")");
			return sb.ToString();
		}
	}

	sealed class InstructionGroups {
		readonly GenTypes genTypes;
		readonly Dictionary<LegacyOpKind, OpCodeOperandKind> legacyToOpKind;
		readonly Dictionary<VexOpKind, OpCodeOperandKind> vexToOpKind;
		readonly Dictionary<XopOpKind, OpCodeOperandKind> xopToOpKind;
		readonly Dictionary<EvexOpKind, OpCodeOperandKind> evexToOpKind;
		readonly OpCodeOperandKind[] d3nowOps;
		readonly HashSet<EnumValue> ignoredCodes;

		public InstructionGroups(GenTypes genTypes) {
			this.genTypes = genTypes;
			var encoderTypes = genTypes.GetObject<EncoderTypes>(TypeIds.EncoderTypes);
			legacyToOpKind = encoderTypes.LegacyOpHandlers.ToDictionary(a => (LegacyOpKind)a.legacyOpKind.Value, a => (OpCodeOperandKind)a.opCodeOperandKind.Value);
			vexToOpKind = encoderTypes.VexOpHandlers.ToDictionary(a => (VexOpKind)a.vexOpKind.Value, a => (OpCodeOperandKind)a.opCodeOperandKind.Value);
			xopToOpKind = encoderTypes.XopOpHandlers.ToDictionary(a => (XopOpKind)a.xopOpKind.Value, a => (OpCodeOperandKind)a.opCodeOperandKind.Value);
			evexToOpKind = encoderTypes.EvexOpHandlers.ToDictionary(a => (EvexOpKind)a.evexOpKind.Value, a => (OpCodeOperandKind)a.opCodeOperandKind.Value);
			d3nowOps = new OpCodeOperandKind[] {
				OpCodeOperandKind.mm_reg,
				OpCodeOperandKind.mm_or_mem,
			};
			ignoredCodes = genTypes.GetKeptCodeValues(new[] {
				Code.INVALID,
				Code.DeclareByte,
				Code.DeclareWord,
				Code.DeclareDword,
				Code.DeclareQword,
				Code.Jo_rel8_16,
				Code.Jo_rel8_32,
				Code.Jo_rel8_64,
				Code.Jno_rel8_16,
				Code.Jno_rel8_32,
				Code.Jno_rel8_64,
				Code.Jb_rel8_16,
				Code.Jb_rel8_32,
				Code.Jb_rel8_64,
				Code.Jae_rel8_16,
				Code.Jae_rel8_32,
				Code.Jae_rel8_64,
				Code.Je_rel8_16,
				Code.Je_rel8_32,
				Code.Je_rel8_64,
				Code.Jne_rel8_16,
				Code.Jne_rel8_32,
				Code.Jne_rel8_64,
				Code.Jbe_rel8_16,
				Code.Jbe_rel8_32,
				Code.Jbe_rel8_64,
				Code.Ja_rel8_16,
				Code.Ja_rel8_32,
				Code.Ja_rel8_64,
				Code.Js_rel8_16,
				Code.Js_rel8_32,
				Code.Js_rel8_64,
				Code.Jns_rel8_16,
				Code.Jns_rel8_32,
				Code.Jns_rel8_64,
				Code.Jp_rel8_16,
				Code.Jp_rel8_32,
				Code.Jp_rel8_64,
				Code.Jnp_rel8_16,
				Code.Jnp_rel8_32,
				Code.Jnp_rel8_64,
				Code.Jl_rel8_16,
				Code.Jl_rel8_32,
				Code.Jl_rel8_64,
				Code.Jge_rel8_16,
				Code.Jge_rel8_32,
				Code.Jge_rel8_64,
				Code.Jle_rel8_16,
				Code.Jle_rel8_32,
				Code.Jle_rel8_64,
				Code.Jg_rel8_16,
				Code.Jg_rel8_32,
				Code.Jg_rel8_64,
				Code.Jo_rel16,
				Code.Jo_rel32_32,
				Code.Jo_rel32_64,
				Code.Jno_rel16,
				Code.Jno_rel32_32,
				Code.Jno_rel32_64,
				Code.Jb_rel16,
				Code.Jb_rel32_32,
				Code.Jb_rel32_64,
				Code.Jae_rel16,
				Code.Jae_rel32_32,
				Code.Jae_rel32_64,
				Code.Je_rel16,
				Code.Je_rel32_32,
				Code.Je_rel32_64,
				Code.Jne_rel16,
				Code.Jne_rel32_32,
				Code.Jne_rel32_64,
				Code.Jbe_rel16,
				Code.Jbe_rel32_32,
				Code.Jbe_rel32_64,
				Code.Ja_rel16,
				Code.Ja_rel32_32,
				Code.Ja_rel32_64,
				Code.Js_rel16,
				Code.Js_rel32_32,
				Code.Js_rel32_64,
				Code.Jns_rel16,
				Code.Jns_rel32_32,
				Code.Jns_rel32_64,
				Code.Jp_rel16,
				Code.Jp_rel32_32,
				Code.Jp_rel32_64,
				Code.Jnp_rel16,
				Code.Jnp_rel32_32,
				Code.Jnp_rel32_64,
				Code.Jl_rel16,
				Code.Jl_rel32_32,
				Code.Jl_rel32_64,
				Code.Jge_rel16,
				Code.Jge_rel32_32,
				Code.Jge_rel32_64,
				Code.Jle_rel16,
				Code.Jle_rel32_32,
				Code.Jle_rel32_64,
				Code.Jg_rel16,
				Code.Jg_rel32_32,
				Code.Jg_rel32_64,
				Code.Loopne_rel8_16_CX,
				Code.Loopne_rel8_32_CX,
				Code.Loopne_rel8_16_ECX,
				Code.Loopne_rel8_32_ECX,
				Code.Loopne_rel8_64_ECX,
				Code.Loopne_rel8_16_RCX,
				Code.Loopne_rel8_64_RCX,
				Code.Loope_rel8_16_CX,
				Code.Loope_rel8_32_CX,
				Code.Loope_rel8_16_ECX,
				Code.Loope_rel8_32_ECX,
				Code.Loope_rel8_64_ECX,
				Code.Loope_rel8_16_RCX,
				Code.Loope_rel8_64_RCX,
				Code.Loop_rel8_16_CX,
				Code.Loop_rel8_32_CX,
				Code.Loop_rel8_16_ECX,
				Code.Loop_rel8_32_ECX,
				Code.Loop_rel8_64_ECX,
				Code.Loop_rel8_16_RCX,
				Code.Loop_rel8_64_RCX,
				Code.Jcxz_rel8_16,
				Code.Jcxz_rel8_32,
				Code.Jecxz_rel8_16,
				Code.Jecxz_rel8_32,
				Code.Jecxz_rel8_64,
				Code.Jrcxz_rel8_16,
				Code.Jrcxz_rel8_64,
				Code.Call_rel16,
				Code.Call_rel32_32,
				Code.Call_rel32_64,
				Code.Jmp_rel16,
				Code.Jmp_rel32_32,
				Code.Jmp_rel32_64,
				Code.Jmp_rel8_16,
				Code.Jmp_rel8_32,
				Code.Jmp_rel8_64,
				Code.Jmpe_disp16,
				Code.Jmpe_disp32,
				Code.Call_ptr1616,
				Code.Call_ptr1632,
				Code.Jmp_ptr1616,
				Code.Jmp_ptr1632,
				Code.Xbegin_rel16,
				Code.Xbegin_rel32,
				Code.Insb_m8_DX,
				Code.Insw_m16_DX,
				Code.Insd_m32_DX,
				Code.Outsb_DX_m8,
				Code.Outsw_DX_m16,
				Code.Outsd_DX_m32,
				Code.Stosb_m8_AL,
				Code.Stosw_m16_AX,
				Code.Stosd_m32_EAX,
				Code.Stosq_m64_RAX,
				Code.Lodsb_AL_m8,
				Code.Lodsw_AX_m16,
				Code.Lodsd_EAX_m32,
				Code.Lodsq_RAX_m64,
				Code.Scasb_AL_m8,
				Code.Scasw_AX_m16,
				Code.Scasd_EAX_m32,
				Code.Scasq_RAX_m64,
				Code.Movsb_m8_m8,
				Code.Movsw_m16_m16,
				Code.Movsd_m32_m32,
				Code.Movsq_m64_m64,
				Code.Cmpsb_m8_m8,
				Code.Cmpsw_m16_m16,
				Code.Cmpsd_m32_m32,
				Code.Cmpsq_m64_m64,
				Code.Maskmovq_rDI_mm_mm,
				Code.Maskmovdqu_rDI_xmm_xmm,
				Code.VEX_Vmaskmovdqu_rDI_xmm_xmm,
			}).ToHashSet();
		}

		OpCodeOperandKind[] GetOperands(OpCodeInfo info) =>
			info.Encoding switch {
				EncodingKind.Legacy => ((LegacyOpCodeInfo)info).OpKinds.Select(a => legacyToOpKind[a]).ToArray(),
				EncodingKind.VEX => ((VexOpCodeInfo)info).OpKinds.Select(a => vexToOpKind[a]).ToArray(),
				EncodingKind.EVEX => ((EvexOpCodeInfo)info).OpKinds.Select(a => evexToOpKind[a]).ToArray(),
				EncodingKind.XOP => ((XopOpCodeInfo)info).OpKinds.Select(a => xopToOpKind[a]).ToArray(),
				EncodingKind.D3NOW => d3nowOps,
				_ => throw new InvalidOperationException(),
			};

		sealed class OpComparer : IEqualityComparer<InstructionOperand[]> {
			public bool Equals([AllowNull] InstructionOperand[] x, [AllowNull] InstructionOperand[] y) {
				if (x!.Length != y!.Length)
					return false;
				for (int i = 0; i < x.Length; i++) {
					if (x[i] != y[i])
						return false;
				}
				return true;
			}

			public int GetHashCode([DisallowNull] InstructionOperand[] obj) {
				int hc = 0;
				foreach (var o in obj)
					hc = HashCode.Combine(hc, o);
				return hc;
			}
		}

		public InstructionGroup[] GetGroups() {
			var groups = new Dictionary<InstructionOperand[], InstructionGroup>(new OpComparer());

			foreach (var def in genTypes.GetObject<InstructionDefs>(TypeIds.InstructionDefs).Defs) {
				if (ignoredCodes.Contains(def.OpCodeInfo.Code))
					continue;

				var opKinds = GetOperands(def.OpCodeInfo);
				foreach (var ops in GetOperands(opKinds)) {
					if (!groups.TryGetValue(ops, out var group))
						groups.Add(ops, group = new InstructionGroup(ops));
					group.OpCodes.Add(def.OpCodeInfo);
				}
			}

			var result = groups.Values.ToArray();
			Array.Sort(result, (a, b) => {
				int c = a.Operands.Length - b.Operands.Length;
				if (c != 0)
					return c;
				for (int i = 0; i < a.Operands.Length; i++) {
					c = GetOrder(a.Operands[i]) - GetOrder(b.Operands[i]);
					if (c != 0)
						return c;
				}
				return 0;
			});
			return result;

			static int GetOrder(InstructionOperand op) =>
				op switch {
					InstructionOperand.Register => 0,
					InstructionOperand.Imm32 => 1,
					InstructionOperand.Imm64 => 2,
					InstructionOperand.Memory => 3,
					_ => throw new InvalidOperationException(),
				};
		}

		static IEnumerable<InstructionOperand[]> GetOperands(OpCodeOperandKind[] opKinds) {
			if (opKinds.Length == 0) {
				yield return Array.Empty<InstructionOperand>();
				yield break;
			}
			if (IcedConstants.MaxOpCount != 5)
				throw new InvalidOperationException();
			var ops = new InstructionOperand[IcedConstants.MaxOpCount][];
			for (int i = 0; i < ops.Length; i++)
				ops[i] = Array.Empty<InstructionOperand>();
			for (int i = 0; i < opKinds.Length; i++)
				ops[i] = GetOperand(opKinds[i]);
			foreach (var o0 in ops[0]) {
				if (opKinds.Length == 1)
					yield return new[] { o0 };
				else {
					foreach (var o1 in ops[1]) {
						if (opKinds.Length == 2)
							yield return new[] { o0, o1 };
						else {
							foreach (var o2 in ops[2]) {
								if (opKinds.Length == 3)
									yield return new[] { o0, o1, o2 };
								else {
									foreach (var o3 in ops[3]) {
										if (opKinds.Length == 4)
											yield return new[] { o0, o1, o2, o3 };
										else {
											foreach (var o4 in ops[4]) {
												if (opKinds.Length == 5)
													yield return new[] { o0, o1, o2, o3, o4 };
												else
													throw new InvalidOperationException();
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}

		static InstructionOperand[] GetOperand(OpCodeOperandKind kind) {
			switch (kind) {
			case OpCodeOperandKind.mem_offs:
			case OpCodeOperandKind.mem:
			case OpCodeOperandKind.sibmem:
			case OpCodeOperandKind.mem_mpx:
			case OpCodeOperandKind.mem_mib:
			case OpCodeOperandKind.mem_vsib32x:
			case OpCodeOperandKind.mem_vsib64x:
			case OpCodeOperandKind.mem_vsib32y:
			case OpCodeOperandKind.mem_vsib64y:
			case OpCodeOperandKind.mem_vsib32z:
			case OpCodeOperandKind.mem_vsib64z:
			case OpCodeOperandKind.seg_rBX_al:
				return new[] { InstructionOperand.Memory };

			case OpCodeOperandKind.r8_or_mem:
			case OpCodeOperandKind.r16_or_mem:
			case OpCodeOperandKind.r32_or_mem:
			case OpCodeOperandKind.r32_or_mem_mpx:
			case OpCodeOperandKind.r64_or_mem:
			case OpCodeOperandKind.r64_or_mem_mpx:
			case OpCodeOperandKind.mm_or_mem:
			case OpCodeOperandKind.xmm_or_mem:
			case OpCodeOperandKind.ymm_or_mem:
			case OpCodeOperandKind.zmm_or_mem:
			case OpCodeOperandKind.bnd_or_mem_mpx:
			case OpCodeOperandKind.k_or_mem:
				return new[] { InstructionOperand.Register, InstructionOperand.Memory };

			case OpCodeOperandKind.r8_reg:
			case OpCodeOperandKind.r8_opcode:
			case OpCodeOperandKind.r16_reg:
			case OpCodeOperandKind.r16_reg_mem:
			case OpCodeOperandKind.r16_rm:
			case OpCodeOperandKind.r16_opcode:
			case OpCodeOperandKind.r32_reg:
			case OpCodeOperandKind.r32_reg_mem:
			case OpCodeOperandKind.r32_rm:
			case OpCodeOperandKind.r32_opcode:
			case OpCodeOperandKind.r32_vvvv:
			case OpCodeOperandKind.r64_reg:
			case OpCodeOperandKind.r64_reg_mem:
			case OpCodeOperandKind.r64_rm:
			case OpCodeOperandKind.r64_opcode:
			case OpCodeOperandKind.r64_vvvv:
			case OpCodeOperandKind.seg_reg:
			case OpCodeOperandKind.k_reg:
			case OpCodeOperandKind.kp1_reg:
			case OpCodeOperandKind.k_rm:
			case OpCodeOperandKind.k_vvvv:
			case OpCodeOperandKind.mm_reg:
			case OpCodeOperandKind.mm_rm:
			case OpCodeOperandKind.xmm_reg:
			case OpCodeOperandKind.xmm_rm:
			case OpCodeOperandKind.xmm_vvvv:
			case OpCodeOperandKind.xmmp3_vvvv:
			case OpCodeOperandKind.xmm_is4:
			case OpCodeOperandKind.xmm_is5:
			case OpCodeOperandKind.ymm_reg:
			case OpCodeOperandKind.ymm_rm:
			case OpCodeOperandKind.ymm_vvvv:
			case OpCodeOperandKind.ymm_is4:
			case OpCodeOperandKind.ymm_is5:
			case OpCodeOperandKind.zmm_reg:
			case OpCodeOperandKind.zmm_rm:
			case OpCodeOperandKind.zmm_vvvv:
			case OpCodeOperandKind.zmmp3_vvvv:
			case OpCodeOperandKind.cr_reg:
			case OpCodeOperandKind.dr_reg:
			case OpCodeOperandKind.tr_reg:
			case OpCodeOperandKind.bnd_reg:
			case OpCodeOperandKind.es:
			case OpCodeOperandKind.cs:
			case OpCodeOperandKind.ss:
			case OpCodeOperandKind.ds:
			case OpCodeOperandKind.fs:
			case OpCodeOperandKind.gs:
			case OpCodeOperandKind.al:
			case OpCodeOperandKind.cl:
			case OpCodeOperandKind.ax:
			case OpCodeOperandKind.dx:
			case OpCodeOperandKind.eax:
			case OpCodeOperandKind.rax:
			case OpCodeOperandKind.st0:
			case OpCodeOperandKind.sti_opcode:
			case OpCodeOperandKind.tmm_reg:
			case OpCodeOperandKind.tmm_rm:
			case OpCodeOperandKind.tmm_vvvv:
				return new[] { InstructionOperand.Register };

			case OpCodeOperandKind.imm2_m2z:
			case OpCodeOperandKind.imm8:
			case OpCodeOperandKind.imm8_const_1:
			case OpCodeOperandKind.imm8sex16:
			case OpCodeOperandKind.imm8sex32:
			case OpCodeOperandKind.imm16:
			case OpCodeOperandKind.imm32:
			case OpCodeOperandKind.imm8sex64:
			case OpCodeOperandKind.imm32sex64:
				return new[] { InstructionOperand.Imm32 };

			case OpCodeOperandKind.imm64:
				return new[] { InstructionOperand.Imm64 };

			case OpCodeOperandKind.None:
			case OpCodeOperandKind.farbr2_2:
			case OpCodeOperandKind.farbr4_2:
			case OpCodeOperandKind.seg_rSI:
			case OpCodeOperandKind.es_rDI:
			case OpCodeOperandKind.seg_rDI:
			case OpCodeOperandKind.br16_1:
			case OpCodeOperandKind.br32_1:
			case OpCodeOperandKind.br64_1:
			case OpCodeOperandKind.br16_2:
			case OpCodeOperandKind.br32_4:
			case OpCodeOperandKind.br64_4:
			case OpCodeOperandKind.xbegin_2:
			case OpCodeOperandKind.xbegin_4:
			case OpCodeOperandKind.brdisp_2:
			case OpCodeOperandKind.brdisp_4:
			default:
				throw new InvalidOperationException();
			}
		}
	}
}
