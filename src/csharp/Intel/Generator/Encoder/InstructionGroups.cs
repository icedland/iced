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
using System.Linq;
using System.Text;
using Generator.Constants;
using Generator.Enums;
using Generator.Enums.Encoder;

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
		readonly Dictionary<LegacyOpKind, OpCodeOperandKind> legacyToOpKind;
		readonly Dictionary<VexOpKind, OpCodeOperandKind> vexToOpKind;
		readonly Dictionary<XopOpKind, OpCodeOperandKind> xopToOpKind;
		readonly Dictionary<EvexOpKind, OpCodeOperandKind> evexToOpKind;
		readonly OpCodeOperandKind[] d3nowOps;
		readonly HashSet<EnumValue> ignoredCodes;

		public InstructionGroups() {
			legacyToOpKind = EncoderTypes.LegacyOpHandlers.ToDictionary(a => (LegacyOpKind)a.legacyOpKind.Value, a => (OpCodeOperandKind)a.opCodeOperandKind.Value);
			vexToOpKind = EncoderTypes.VexOpHandlers.ToDictionary(a => (VexOpKind)a.vexOpKind.Value, a => (OpCodeOperandKind)a.opCodeOperandKind.Value);
			xopToOpKind = EncoderTypes.XopOpHandlers.ToDictionary(a => (XopOpKind)a.xopOpKind.Value, a => (OpCodeOperandKind)a.opCodeOperandKind.Value);
			evexToOpKind = EncoderTypes.EvexOpHandlers.ToDictionary(a => (EvexOpKind)a.evexOpKind.Value, a => (OpCodeOperandKind)a.opCodeOperandKind.Value);
			d3nowOps = new OpCodeOperandKind[] {
				OpCodeOperandKind.mm_reg,
				OpCodeOperandKind.mm_or_mem,
			};
			var code = CodeEnum.Instance;
			ignoredCodes = new HashSet<EnumValue> {
				code["INVALID"],
				code["DeclareByte"],
				code["DeclareWord"],
				code["DeclareDword"],
				code["DeclareQword"],
				code["Jo_rel8_16"],
				code["Jo_rel8_32"],
				code["Jo_rel8_64"],
				code["Jno_rel8_16"],
				code["Jno_rel8_32"],
				code["Jno_rel8_64"],
				code["Jb_rel8_16"],
				code["Jb_rel8_32"],
				code["Jb_rel8_64"],
				code["Jae_rel8_16"],
				code["Jae_rel8_32"],
				code["Jae_rel8_64"],
				code["Je_rel8_16"],
				code["Je_rel8_32"],
				code["Je_rel8_64"],
				code["Jne_rel8_16"],
				code["Jne_rel8_32"],
				code["Jne_rel8_64"],
				code["Jbe_rel8_16"],
				code["Jbe_rel8_32"],
				code["Jbe_rel8_64"],
				code["Ja_rel8_16"],
				code["Ja_rel8_32"],
				code["Ja_rel8_64"],
				code["Js_rel8_16"],
				code["Js_rel8_32"],
				code["Js_rel8_64"],
				code["Jns_rel8_16"],
				code["Jns_rel8_32"],
				code["Jns_rel8_64"],
				code["Jp_rel8_16"],
				code["Jp_rel8_32"],
				code["Jp_rel8_64"],
				code["Jnp_rel8_16"],
				code["Jnp_rel8_32"],
				code["Jnp_rel8_64"],
				code["Jl_rel8_16"],
				code["Jl_rel8_32"],
				code["Jl_rel8_64"],
				code["Jge_rel8_16"],
				code["Jge_rel8_32"],
				code["Jge_rel8_64"],
				code["Jle_rel8_16"],
				code["Jle_rel8_32"],
				code["Jle_rel8_64"],
				code["Jg_rel8_16"],
				code["Jg_rel8_32"],
				code["Jg_rel8_64"],
				code["Jo_rel16"],
				code["Jo_rel32_32"],
				code["Jo_rel32_64"],
				code["Jno_rel16"],
				code["Jno_rel32_32"],
				code["Jno_rel32_64"],
				code["Jb_rel16"],
				code["Jb_rel32_32"],
				code["Jb_rel32_64"],
				code["Jae_rel16"],
				code["Jae_rel32_32"],
				code["Jae_rel32_64"],
				code["Je_rel16"],
				code["Je_rel32_32"],
				code["Je_rel32_64"],
				code["Jne_rel16"],
				code["Jne_rel32_32"],
				code["Jne_rel32_64"],
				code["Jbe_rel16"],
				code["Jbe_rel32_32"],
				code["Jbe_rel32_64"],
				code["Ja_rel16"],
				code["Ja_rel32_32"],
				code["Ja_rel32_64"],
				code["Js_rel16"],
				code["Js_rel32_32"],
				code["Js_rel32_64"],
				code["Jns_rel16"],
				code["Jns_rel32_32"],
				code["Jns_rel32_64"],
				code["Jp_rel16"],
				code["Jp_rel32_32"],
				code["Jp_rel32_64"],
				code["Jnp_rel16"],
				code["Jnp_rel32_32"],
				code["Jnp_rel32_64"],
				code["Jl_rel16"],
				code["Jl_rel32_32"],
				code["Jl_rel32_64"],
				code["Jge_rel16"],
				code["Jge_rel32_32"],
				code["Jge_rel32_64"],
				code["Jle_rel16"],
				code["Jle_rel32_32"],
				code["Jle_rel32_64"],
				code["Jg_rel16"],
				code["Jg_rel32_32"],
				code["Jg_rel32_64"],
				code["Loopne_rel8_16_CX"],
				code["Loopne_rel8_32_CX"],
				code["Loopne_rel8_16_ECX"],
				code["Loopne_rel8_32_ECX"],
				code["Loopne_rel8_64_ECX"],
				code["Loopne_rel8_16_RCX"],
				code["Loopne_rel8_64_RCX"],
				code["Loope_rel8_16_CX"],
				code["Loope_rel8_32_CX"],
				code["Loope_rel8_16_ECX"],
				code["Loope_rel8_32_ECX"],
				code["Loope_rel8_64_ECX"],
				code["Loope_rel8_16_RCX"],
				code["Loope_rel8_64_RCX"],
				code["Loop_rel8_16_CX"],
				code["Loop_rel8_32_CX"],
				code["Loop_rel8_16_ECX"],
				code["Loop_rel8_32_ECX"],
				code["Loop_rel8_64_ECX"],
				code["Loop_rel8_16_RCX"],
				code["Loop_rel8_64_RCX"],
				code["Jcxz_rel8_16"],
				code["Jcxz_rel8_32"],
				code["Jecxz_rel8_16"],
				code["Jecxz_rel8_32"],
				code["Jecxz_rel8_64"],
				code["Jrcxz_rel8_16"],
				code["Jrcxz_rel8_64"],
				code["Call_rel16"],
				code["Call_rel32_32"],
				code["Call_rel32_64"],
				code["Jmp_rel16"],
				code["Jmp_rel32_32"],
				code["Jmp_rel32_64"],
				code["Jmp_rel8_16"],
				code["Jmp_rel8_32"],
				code["Jmp_rel8_64"],
				code["Jmpe_disp16"],
				code["Jmpe_disp32"],
				code["Call_ptr1616"],
				code["Call_ptr1632"],
				code["Jmp_ptr1616"],
				code["Jmp_ptr1632"],
				code["Xbegin_rel16"],
				code["Xbegin_rel32"],
				code["Insb_m8_DX"],
				code["Insw_m16_DX"],
				code["Insd_m32_DX"],
				code["Outsb_DX_m8"],
				code["Outsw_DX_m16"],
				code["Outsd_DX_m32"],
				code["Stosb_m8_AL"],
				code["Stosw_m16_AX"],
				code["Stosd_m32_EAX"],
				code["Stosq_m64_RAX"],
				code["Lodsb_AL_m8"],
				code["Lodsw_AX_m16"],
				code["Lodsd_EAX_m32"],
				code["Lodsq_RAX_m64"],
				code["Scasb_AL_m8"],
				code["Scasw_AX_m16"],
				code["Scasd_EAX_m32"],
				code["Scasq_RAX_m64"],
				code["Movsb_m8_m8"],
				code["Movsw_m16_m16"],
				code["Movsd_m32_m32"],
				code["Movsq_m64_m64"],
				code["Cmpsb_m8_m8"],
				code["Cmpsw_m16_m16"],
				code["Cmpsd_m32_m32"],
				code["Cmpsq_m64_m64"],
				code["Maskmovq_rDI_mm_mm"],
				code["Maskmovdqu_rDI_xmm_xmm"],
				code["VEX_Vmaskmovdqu_rDI_xmm_xmm"],
			};
		}

		OpCodeOperandKind[] GetOperands(OpCodeInfo info) {
			switch (info.Encoding) {
			case EncodingKind.Legacy:
				return ((LegacyOpCodeInfo)info).OpKinds.Select(a => legacyToOpKind[a]).ToArray();

			case EncodingKind.VEX:
				return ((VexOpCodeInfo)info).OpKinds.Select(a => vexToOpKind[a]).ToArray();

			case EncodingKind.EVEX:
				return ((EvexOpCodeInfo)info).OpKinds.Select(a => evexToOpKind[a]).ToArray();

			case EncodingKind.XOP:
				return ((XopOpCodeInfo)info).OpKinds.Select(a => xopToOpKind[a]).ToArray();

			case EncodingKind.D3NOW:
				return d3nowOps;

			default:
				throw new InvalidOperationException();
			}
		}

		sealed class OpComparer : IEqualityComparer<InstructionOperand[]> {
			public bool Equals(InstructionOperand[] x, InstructionOperand[] y) {
				if (x.Length != y.Length)
					return false;
				for (int i = 0; i < x.Length; i++) {
					if (x[i] != y[i])
						return false;
				}
				return true;
			}

			public int GetHashCode(InstructionOperand[] obj) {
				int hc = 0;
				foreach (var o in obj)
					hc = HashCode.Combine(hc, o);
				return hc;
			}
		}

		public InstructionGroup[] GetGroups() {
			var groups = new Dictionary<InstructionOperand[], InstructionGroup>(new OpComparer());

			foreach (var info in OpCodeInfoTable.Data) {
				if (ignoredCodes.Contains(info.Code))
					continue;

				var opKinds = GetOperands(info);
				foreach (var ops in GetOperands(opKinds)) {
					if (!groups.TryGetValue(ops, out var group))
						groups.Add(ops, group = new InstructionGroup(ops));
					group.OpCodes.Add(info);
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
			case OpCodeOperandKind.r16_rm:
			case OpCodeOperandKind.r16_opcode:
			case OpCodeOperandKind.r32_reg:
			case OpCodeOperandKind.r32_rm:
			case OpCodeOperandKind.r32_opcode:
			case OpCodeOperandKind.r32_vvvv:
			case OpCodeOperandKind.r64_reg:
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
