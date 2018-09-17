/*
    Copyright (C) 2018 de4dot@gmail.com

    This file is part of Iced.

    Iced is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Iced is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Iced.  If not, see <https://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.DecoderTests {
	public readonly struct DecoderTestInfo {
		public readonly int Bitness;
		public readonly Code Code;
		public readonly string HexBytes;

		public DecoderTestInfo(int bitness, Code code, string hexBytes) {
			Bitness = bitness;
			Code = code;
			HexBytes = hexBytes;
		}
	}

	public static class DecoderTestUtils {
		static readonly HashSet<Code> code32Only = new HashSet<Code>(GetCode32());
		static readonly HashSet<Code> code64Only = new HashSet<Code>(GetCode64());

		public static HashSet<Code> Code32Only => code32Only;
		public static HashSet<Code> Code64Only => code64Only;

		static IEnumerable<Code> GetCode32() {
			yield return Code.Pushw_ES;
			yield return Code.Pushd_ES;
			yield return Code.Popw_ES;
			yield return Code.Popd_ES;
			yield return Code.Pushw_CS;
			yield return Code.Pushd_CS;
			yield return Code.Pushw_SS;
			yield return Code.Pushd_SS;
			yield return Code.Popw_SS;
			yield return Code.Popd_SS;
			yield return Code.Pushw_DS;
			yield return Code.Pushd_DS;
			yield return Code.Popw_DS;
			yield return Code.Popd_DS;
			yield return Code.Daa;
			yield return Code.Das;
			yield return Code.Aaa;
			yield return Code.Aas;
			yield return Code.Inc_AX;
			yield return Code.Inc_EAX;
			yield return Code.Inc_CX;
			yield return Code.Inc_ECX;
			yield return Code.Inc_DX;
			yield return Code.Inc_EDX;
			yield return Code.Inc_BX;
			yield return Code.Inc_EBX;
			yield return Code.Inc_SP;
			yield return Code.Inc_ESP;
			yield return Code.Inc_BP;
			yield return Code.Inc_EBP;
			yield return Code.Inc_SI;
			yield return Code.Inc_ESI;
			yield return Code.Inc_DI;
			yield return Code.Inc_EDI;
			yield return Code.Dec_AX;
			yield return Code.Dec_EAX;
			yield return Code.Dec_CX;
			yield return Code.Dec_ECX;
			yield return Code.Dec_DX;
			yield return Code.Dec_EDX;
			yield return Code.Dec_BX;
			yield return Code.Dec_EBX;
			yield return Code.Dec_SP;
			yield return Code.Dec_ESP;
			yield return Code.Dec_BP;
			yield return Code.Dec_EBP;
			yield return Code.Dec_SI;
			yield return Code.Dec_ESI;
			yield return Code.Dec_DI;
			yield return Code.Dec_EDI;
			yield return Code.Push_EAX;
			yield return Code.Push_ECX;
			yield return Code.Push_EDX;
			yield return Code.Push_EBX;
			yield return Code.Push_ESP;
			yield return Code.Push_EBP;
			yield return Code.Push_ESI;
			yield return Code.Push_EDI;
			yield return Code.Pop_EAX;
			yield return Code.Pop_ECX;
			yield return Code.Pop_EDX;
			yield return Code.Pop_EBX;
			yield return Code.Pop_ESP;
			yield return Code.Pop_EBP;
			yield return Code.Pop_ESI;
			yield return Code.Pop_EDI;
			yield return Code.Pushaw;
			yield return Code.Pushad;
			yield return Code.Popaw;
			yield return Code.Popad;
			yield return Code.Bound_r16_m1616;
			yield return Code.Bound_r32_m3232;
			yield return Code.Arpl_rm16_r16;
			yield return Code.Arpl_r32m16_r32;
			yield return Code.Pushd_imm32;
			yield return Code.Pushd_imm8;
			yield return Code.Jo_rel8_16;
			yield return Code.Jo_rel8_32;
			yield return Code.Jno_rel8_16;
			yield return Code.Jno_rel8_32;
			yield return Code.Jb_rel8_16;
			yield return Code.Jb_rel8_32;
			yield return Code.Jae_rel8_16;
			yield return Code.Jae_rel8_32;
			yield return Code.Je_rel8_16;
			yield return Code.Je_rel8_32;
			yield return Code.Jne_rel8_16;
			yield return Code.Jne_rel8_32;
			yield return Code.Jbe_rel8_16;
			yield return Code.Jbe_rel8_32;
			yield return Code.Ja_rel8_16;
			yield return Code.Ja_rel8_32;
			yield return Code.Js_rel8_16;
			yield return Code.Js_rel8_32;
			yield return Code.Jns_rel8_16;
			yield return Code.Jns_rel8_32;
			yield return Code.Jp_rel8_16;
			yield return Code.Jp_rel8_32;
			yield return Code.Jnp_rel8_16;
			yield return Code.Jnp_rel8_32;
			yield return Code.Jl_rel8_16;
			yield return Code.Jl_rel8_32;
			yield return Code.Jge_rel8_16;
			yield return Code.Jge_rel8_32;
			yield return Code.Jle_rel8_16;
			yield return Code.Jle_rel8_32;
			yield return Code.Jg_rel8_16;
			yield return Code.Jg_rel8_32;
			yield return Code.Pop_rm32;
			yield return Code.Call_ptr1616;
			yield return Code.Call_ptr3216;
			yield return Code.Pushfd;
			yield return Code.Popfd;
			yield return Code.Retnw_imm16;
			yield return Code.Retnd_imm16;
			yield return Code.Retnw;
			yield return Code.Retnd;
			yield return Code.Les_r16_m32;
			yield return Code.Les_r32_m48;
			yield return Code.Lds_r16_m32;
			yield return Code.Lds_r32_m48;
			yield return Code.Enterd_imm16_imm8;
			yield return Code.Leaved;
			yield return Code.Into;
			yield return Code.Aam_imm8;
			yield return Code.Aad_imm8;
			yield return Code.Salc;
			yield return Code.Loopne_rel8_16_CX;
			yield return Code.Loopne_rel8_32_CX;
			yield return Code.Loopne_rel8_16_ECX;
			yield return Code.Loopne_rel8_32_ECX;
			yield return Code.Loope_rel8_16_CX;
			yield return Code.Loope_rel8_32_CX;
			yield return Code.Loope_rel8_16_ECX;
			yield return Code.Loope_rel8_32_ECX;
			yield return Code.Loop_rel8_16_CX;
			yield return Code.Loop_rel8_32_CX;
			yield return Code.Loop_rel8_16_ECX;
			yield return Code.Loop_rel8_32_ECX;
			yield return Code.Jcxz_rel8_16;
			yield return Code.Jcxz_rel8_32;
			yield return Code.Jecxz_rel8_16;
			yield return Code.Jecxz_rel8_32;
			yield return Code.Call_rel16;
			yield return Code.Call_rel32_32;
			yield return Code.Jmp_rel16;
			yield return Code.Jmp_rel32_32;
			yield return Code.Jmp_ptr1616;
			yield return Code.Jmp_ptr3216;
			yield return Code.Jmp_rel8_16;
			yield return Code.Jmp_rel8_32;
			yield return Code.Jo_rel16;
			yield return Code.Jo_rel32_32;
			yield return Code.Jno_rel16;
			yield return Code.Jno_rel32_32;
			yield return Code.Jb_rel16;
			yield return Code.Jb_rel32_32;
			yield return Code.Jae_rel16;
			yield return Code.Jae_rel32_32;
			yield return Code.Je_rel16;
			yield return Code.Je_rel32_32;
			yield return Code.Jne_rel16;
			yield return Code.Jne_rel32_32;
			yield return Code.Jbe_rel16;
			yield return Code.Jbe_rel32_32;
			yield return Code.Ja_rel16;
			yield return Code.Ja_rel32_32;
			yield return Code.Js_rel16;
			yield return Code.Js_rel32_32;
			yield return Code.Jns_rel16;
			yield return Code.Jns_rel32_32;
			yield return Code.Jp_rel16;
			yield return Code.Jp_rel32_32;
			yield return Code.Jnp_rel16;
			yield return Code.Jnp_rel32_32;
			yield return Code.Jl_rel16;
			yield return Code.Jl_rel32_32;
			yield return Code.Jge_rel16;
			yield return Code.Jge_rel32_32;
			yield return Code.Jle_rel16;
			yield return Code.Jle_rel32_32;
			yield return Code.Jg_rel16;
			yield return Code.Jg_rel32_32;
			yield return Code.Pushd_FS;
			yield return Code.Popd_FS;
			yield return Code.Pushd_GS;
			yield return Code.Popd_GS;
			yield return Code.Call_rm16;
			yield return Code.Call_rm32;
			yield return Code.Jmp_rm16;
			yield return Code.Jmp_rm32;
			yield return Code.Push_rm32;
			yield return Code.Mov_r32_cr;
			yield return Code.Mov_r32_dr;
			yield return Code.Mov_cr_r32;
			yield return Code.Mov_dr_r32;
			yield return Code.Bndmov_bnd_bndm64;
			yield return Code.Bndcl_bnd_rm32;
			yield return Code.Bndcu_bnd_rm32;
			yield return Code.Bndmov_bndm64_bnd;
			yield return Code.Bndmk_bnd_m32;
			yield return Code.Bndcn_bnd_rm32;
			yield return Code.Rdpid_r32;
			yield return Code.Vmread_rm32_r32;
			yield return Code.Vmwrite_r32_rm32;
			yield return Code.Invept_r32_m128;
			yield return Code.Invvpid_r32_m128;
			yield return Code.Invpcid_r32_m128;
			yield return Code.Monitorw;
			yield return Code.Sgdt_m40;
			yield return Code.Sgdt_m48;
			yield return Code.Sidt_m40;
			yield return Code.Sidt_m48;
			yield return Code.Lgdt_m40;
			yield return Code.Lgdt_m48;
			yield return Code.Lidt_m40;
			yield return Code.Lidt_m48;
			yield return Code.Vmrunw;
			yield return Code.Vmloadw;
			yield return Code.Vmsavew;
			yield return Code.Invlpgaw;
			yield return Code.Monitorxw;
		}

		static IEnumerable<Code> GetCode64() {
			yield return Code.Add_rm64_r64;
			yield return Code.Add_r64_rm64;
			yield return Code.Add_RAX_imm32;
			yield return Code.Or_rm64_r64;
			yield return Code.Or_r64_rm64;
			yield return Code.Or_RAX_imm32;
			yield return Code.Adc_rm64_r64;
			yield return Code.Adc_r64_rm64;
			yield return Code.Adc_RAX_imm32;
			yield return Code.Sbb_rm64_r64;
			yield return Code.Sbb_r64_rm64;
			yield return Code.Sbb_RAX_imm32;
			yield return Code.And_rm64_r64;
			yield return Code.And_r64_rm64;
			yield return Code.And_RAX_imm32;
			yield return Code.Sub_rm64_r64;
			yield return Code.Sub_r64_rm64;
			yield return Code.Sub_RAX_imm32;
			yield return Code.Xor_rm64_r64;
			yield return Code.Xor_r64_rm64;
			yield return Code.Xor_RAX_imm32;
			yield return Code.Cmp_rm64_r64;
			yield return Code.Cmp_r64_rm64;
			yield return Code.Cmp_RAX_imm32;
			yield return Code.Push_RAX;
			yield return Code.Push_R8;
			yield return Code.Push_RCX;
			yield return Code.Push_R9;
			yield return Code.Push_RDX;
			yield return Code.Push_R10;
			yield return Code.Push_RBX;
			yield return Code.Push_R11;
			yield return Code.Push_RSP;
			yield return Code.Push_R12;
			yield return Code.Push_RBP;
			yield return Code.Push_R13;
			yield return Code.Push_RSI;
			yield return Code.Push_R14;
			yield return Code.Push_RDI;
			yield return Code.Push_R15;
			yield return Code.Pop_RAX;
			yield return Code.Pop_R8;
			yield return Code.Pop_RCX;
			yield return Code.Pop_R9;
			yield return Code.Pop_RDX;
			yield return Code.Pop_R10;
			yield return Code.Pop_RBX;
			yield return Code.Pop_R11;
			yield return Code.Pop_RSP;
			yield return Code.Pop_R12;
			yield return Code.Pop_RBP;
			yield return Code.Pop_R13;
			yield return Code.Pop_RSI;
			yield return Code.Pop_R14;
			yield return Code.Pop_RDI;
			yield return Code.Pop_R15;
			yield return Code.Pushq_imm32;
			yield return Code.Imul_r64_rm64_imm32;
			yield return Code.Pushq_imm8;
			yield return Code.Imul_r64_rm64_imm8;
			yield return Code.Jo_rel8_64;
			yield return Code.Jno_rel8_64;
			yield return Code.Jb_rel8_64;
			yield return Code.Jae_rel8_64;
			yield return Code.Je_rel8_64;
			yield return Code.Jne_rel8_64;
			yield return Code.Jbe_rel8_64;
			yield return Code.Ja_rel8_64;
			yield return Code.Js_rel8_64;
			yield return Code.Jns_rel8_64;
			yield return Code.Jp_rel8_64;
			yield return Code.Jnp_rel8_64;
			yield return Code.Jl_rel8_64;
			yield return Code.Jge_rel8_64;
			yield return Code.Jle_rel8_64;
			yield return Code.Jg_rel8_64;
			yield return Code.Add_rm64_imm32;
			yield return Code.Or_rm64_imm32;
			yield return Code.Adc_rm64_imm32;
			yield return Code.Sbb_rm64_imm32;
			yield return Code.And_rm64_imm32;
			yield return Code.Sub_rm64_imm32;
			yield return Code.Xor_rm64_imm32;
			yield return Code.Cmp_rm64_imm32;
			yield return Code.Add_rm64_imm8;
			yield return Code.Or_rm64_imm8;
			yield return Code.Adc_rm64_imm8;
			yield return Code.Sbb_rm64_imm8;
			yield return Code.And_rm64_imm8;
			yield return Code.Sub_rm64_imm8;
			yield return Code.Xor_rm64_imm8;
			yield return Code.Cmp_rm64_imm8;
			yield return Code.Test_rm64_r64;
			yield return Code.Xchg_rm64_r64;
			yield return Code.Mov_rm64_r64;
			yield return Code.Mov_r64_rm64;
			yield return Code.Mov_rm64_Sreg;
			yield return Code.Lea_r64_m;
			yield return Code.Mov_Sreg_rm64;
			yield return Code.Pop_rm64;
			yield return Code.Nopq;
			yield return Code.Xchg_R8_RAX;
			yield return Code.Xchg_RCX_RAX;
			yield return Code.Xchg_R9_RAX;
			yield return Code.Xchg_RDX_RAX;
			yield return Code.Xchg_R10_RAX;
			yield return Code.Xchg_RBX_RAX;
			yield return Code.Xchg_R11_RAX;
			yield return Code.Xchg_RSP_RAX;
			yield return Code.Xchg_R12_RAX;
			yield return Code.Xchg_RBP_RAX;
			yield return Code.Xchg_R13_RAX;
			yield return Code.Xchg_RSI_RAX;
			yield return Code.Xchg_R14_RAX;
			yield return Code.Xchg_RDI_RAX;
			yield return Code.Xchg_R15_RAX;
			yield return Code.Cdqe;
			yield return Code.Cqo;
			yield return Code.Pushfq;
			yield return Code.Popfq;
			yield return Code.Mov_RAX_moffs64;
			yield return Code.Mov_moffs64_RAX;
			yield return Code.Movsq_m64_m64;
			yield return Code.Cmpsq_m64_m64;
			yield return Code.Test_RAX_imm32;
			yield return Code.Stosq_m64_RAX;
			yield return Code.Lodsq_RAX_m64;
			yield return Code.Scasq_RAX_m64;
			yield return Code.Mov_RAX_imm64;
			yield return Code.Mov_R8_imm64;
			yield return Code.Mov_RCX_imm64;
			yield return Code.Mov_R9_imm64;
			yield return Code.Mov_RDX_imm64;
			yield return Code.Mov_R10_imm64;
			yield return Code.Mov_RBX_imm64;
			yield return Code.Mov_R11_imm64;
			yield return Code.Mov_RSP_imm64;
			yield return Code.Mov_R12_imm64;
			yield return Code.Mov_RBP_imm64;
			yield return Code.Mov_R13_imm64;
			yield return Code.Mov_RSI_imm64;
			yield return Code.Mov_R14_imm64;
			yield return Code.Mov_RDI_imm64;
			yield return Code.Mov_R15_imm64;
			yield return Code.Rol_rm64_imm8;
			yield return Code.Ror_rm64_imm8;
			yield return Code.Rcl_rm64_imm8;
			yield return Code.Rcr_rm64_imm8;
			yield return Code.Shl_rm64_imm8;
			yield return Code.Shr_rm64_imm8;
			yield return Code.Sar_rm64_imm8;
			yield return Code.Retnq_imm16;
			yield return Code.Retnq;
			yield return Code.Mov_rm64_imm32;
			yield return Code.Enterq_imm16_imm8;
			yield return Code.Leaveq;
			yield return Code.Retfq_imm16;
			yield return Code.Retfq;
			yield return Code.Iretq;
			yield return Code.Rol_rm64_1;
			yield return Code.Ror_rm64_1;
			yield return Code.Rcl_rm64_1;
			yield return Code.Rcr_rm64_1;
			yield return Code.Shl_rm64_1;
			yield return Code.Shr_rm64_1;
			yield return Code.Sar_rm64_1;
			yield return Code.Rol_rm64_CL;
			yield return Code.Ror_rm64_CL;
			yield return Code.Rcl_rm64_CL;
			yield return Code.Rcr_rm64_CL;
			yield return Code.Shl_rm64_CL;
			yield return Code.Shr_rm64_CL;
			yield return Code.Sar_rm64_CL;
			yield return Code.Loopne_rel8_64_ECX;
			yield return Code.Loopne_rel8_64_RCX;
			yield return Code.Loope_rel8_64_ECX;
			yield return Code.Loope_rel8_64_RCX;
			yield return Code.Loop_rel8_64_ECX;
			yield return Code.Loop_rel8_64_RCX;
			yield return Code.Jecxz_rel8_64;
			yield return Code.Jrcxz_rel8_64;
			yield return Code.Call_rel32_64;
			yield return Code.Jmp_rel32_64;
			yield return Code.Jmp_rel8_64;
			yield return Code.Test_rm64_imm32;
			yield return Code.Not_rm64;
			yield return Code.Neg_rm64;
			yield return Code.Mul_rm64;
			yield return Code.Imul_rm64;
			yield return Code.Div_rm64;
			yield return Code.Idiv_rm64;
			yield return Code.Inc_rm64;
			yield return Code.Dec_rm64;
			yield return Code.Call_rm64;
			yield return Code.Call_m6416;
			yield return Code.Jmp_rm64;
			yield return Code.Jmp_m6416;
			yield return Code.Push_rm64;
			yield return Code.Sldt_r64m16;
			yield return Code.Str_r64m16;
			yield return Code.Lldt_r64m16;
			yield return Code.Ltr_r64m16;
			yield return Code.Verr_r64m16;
			yield return Code.Verw_r64m16;
			yield return Code.Sgdt_m80;
			yield return Code.Sidt_m80;
			yield return Code.Lgdt_m80;
			yield return Code.Lidt_m80;
			yield return Code.Smsw_r64m16;
			yield return Code.Lmsw_r64m16;
			yield return Code.Lar_r64_rm64;
			yield return Code.Lsl_r64_rm64;
			yield return Code.Mov_r64_cr;
			yield return Code.Mov_r64_dr;
			yield return Code.Mov_cr_r64;
			yield return Code.Mov_dr_r64;
			yield return Code.Jo_rel32_64;
			yield return Code.Jno_rel32_64;
			yield return Code.Jb_rel32_64;
			yield return Code.Jae_rel32_64;
			yield return Code.Je_rel32_64;
			yield return Code.Jne_rel32_64;
			yield return Code.Jbe_rel32_64;
			yield return Code.Ja_rel32_64;
			yield return Code.Js_rel32_64;
			yield return Code.Jns_rel32_64;
			yield return Code.Jp_rel32_64;
			yield return Code.Jnp_rel32_64;
			yield return Code.Jl_rel32_64;
			yield return Code.Jge_rel32_64;
			yield return Code.Jle_rel32_64;
			yield return Code.Jg_rel32_64;
			yield return Code.Pushq_FS;
			yield return Code.Popq_FS;
			yield return Code.Bt_rm64_r64;
			yield return Code.Shld_rm64_r64_imm8;
			yield return Code.Shld_rm64_r64_CL;
			yield return Code.Pushq_GS;
			yield return Code.Popq_GS;
			yield return Code.Bts_rm64_r64;
			yield return Code.Shrd_rm64_r64_imm8;
			yield return Code.Shrd_rm64_r64_CL;
			yield return Code.Imul_r64_rm64;
			yield return Code.Cmpxchg_rm64_r64;
			yield return Code.Lss_r64_m80;
			yield return Code.Btr_rm64_r64;
			yield return Code.Lfs_r64_m80;
			yield return Code.Lgs_r64_m80;
			yield return Code.Movzx_r64_rm8;
			yield return Code.Movzx_r64_rm16;
			yield return Code.Ud1_r64_rm64;
			yield return Code.Bt_rm64_imm8;
			yield return Code.Bts_rm64_imm8;
			yield return Code.Btr_rm64_imm8;
			yield return Code.Btc_rm64_imm8;
			yield return Code.Btc_rm64_r64;
			yield return Code.Bsf_r64_rm64;
			yield return Code.Bsr_r64_rm64;
			yield return Code.Movsx_r64_rm8;
			yield return Code.Movsx_r64_rm16;
			yield return Code.Xadd_rm64_r64;
			yield return Code.Bswap_RAX;
			yield return Code.Bswap_R8;
			yield return Code.Bswap_RCX;
			yield return Code.Bswap_R9;
			yield return Code.Bswap_RDX;
			yield return Code.Bswap_R10;
			yield return Code.Bswap_RBX;
			yield return Code.Bswap_R11;
			yield return Code.Bswap_RSP;
			yield return Code.Bswap_R12;
			yield return Code.Bswap_RBP;
			yield return Code.Bswap_R13;
			yield return Code.Bswap_RSI;
			yield return Code.Bswap_R14;
			yield return Code.Bswap_RDI;
			yield return Code.Bswap_R15;
			yield return Code.Push_R8W;
			yield return Code.Push_R9W;
			yield return Code.Push_R10W;
			yield return Code.Push_R11W;
			yield return Code.Push_R12W;
			yield return Code.Push_R13W;
			yield return Code.Push_R14W;
			yield return Code.Push_R15W;
			yield return Code.Pop_R8W;
			yield return Code.Pop_R9W;
			yield return Code.Pop_R10W;
			yield return Code.Pop_R11W;
			yield return Code.Pop_R12W;
			yield return Code.Pop_R13W;
			yield return Code.Pop_R14W;
			yield return Code.Pop_R15W;
			yield return Code.Xchg_R8W_AX;
			yield return Code.Xchg_R8D_EAX;
			yield return Code.Xchg_R9W_AX;
			yield return Code.Xchg_R9D_EAX;
			yield return Code.Xchg_R10W_AX;
			yield return Code.Xchg_R10D_EAX;
			yield return Code.Xchg_R11W_AX;
			yield return Code.Xchg_R11D_EAX;
			yield return Code.Xchg_R12W_AX;
			yield return Code.Xchg_R12D_EAX;
			yield return Code.Xchg_R13W_AX;
			yield return Code.Xchg_R13D_EAX;
			yield return Code.Xchg_R14W_AX;
			yield return Code.Xchg_R14D_EAX;
			yield return Code.Xchg_R15W_AX;
			yield return Code.Xchg_R15D_EAX;
			yield return Code.Mov_R8L_imm8;
			yield return Code.Mov_R9L_imm8;
			yield return Code.Mov_R10L_imm8;
			yield return Code.Mov_R11L_imm8;
			yield return Code.Mov_SPL_imm8;
			yield return Code.Mov_R12L_imm8;
			yield return Code.Mov_BPL_imm8;
			yield return Code.Mov_R13L_imm8;
			yield return Code.Mov_SIL_imm8;
			yield return Code.Mov_R14L_imm8;
			yield return Code.Mov_DIL_imm8;
			yield return Code.Mov_R15L_imm8;
			yield return Code.Mov_R8W_imm16;
			yield return Code.Mov_R8D_imm32;
			yield return Code.Mov_R9W_imm16;
			yield return Code.Mov_R9D_imm32;
			yield return Code.Mov_R10W_imm16;
			yield return Code.Mov_R10D_imm32;
			yield return Code.Mov_R11W_imm16;
			yield return Code.Mov_R11D_imm32;
			yield return Code.Mov_R12W_imm16;
			yield return Code.Mov_R12D_imm32;
			yield return Code.Mov_R13W_imm16;
			yield return Code.Mov_R13D_imm32;
			yield return Code.Mov_R14W_imm16;
			yield return Code.Mov_R14D_imm32;
			yield return Code.Mov_R15W_imm16;
			yield return Code.Mov_R15D_imm32;
			yield return Code.Bswap_R8W;
			yield return Code.Bswap_R8D;
			yield return Code.Bswap_R9W;
			yield return Code.Bswap_R9D;
			yield return Code.Bswap_R10W;
			yield return Code.Bswap_R10D;
			yield return Code.Bswap_R11W;
			yield return Code.Bswap_R11D;
			yield return Code.Bswap_R12W;
			yield return Code.Bswap_R12D;
			yield return Code.Bswap_R13W;
			yield return Code.Bswap_R13D;
			yield return Code.Bswap_R14W;
			yield return Code.Bswap_R14D;
			yield return Code.Bswap_R15W;
			yield return Code.Bswap_R15D;
			yield return Code.Movsxd_r16_rm16;
			yield return Code.Movsxd_r32_rm32;
			yield return Code.Movsxd_r64_rm32;
			yield return Code.Syscall;
			yield return Code.Sysretd;
			yield return Code.Sysretq;
			yield return Code.Cvtsi2ss_xmm_rm64;
			yield return Code.Cvtsi2sd_xmm_rm64;
			yield return Code.Cvttss2si_r64_xmmm32;
			yield return Code.Cvttsd2si_r64_xmmm64;
			yield return Code.Cvtss2si_r64_xmmm32;
			yield return Code.Cvtsd2si_r64_xmmm64;
			yield return Code.VEX_Vcvtsi2ss_xmm_xmm_rm64;
			yield return Code.EVEX_Vcvtsi2ss_xmm_xmm_rm64_er;
			yield return Code.VEX_Vcvtsi2sd_xmm_xmm_rm64;
			yield return Code.EVEX_Vcvtsi2sd_xmm_xmm_rm64_er;
			yield return Code.VEX_Vcvttss2si_r64_xmmm32;
			yield return Code.EVEX_Vcvttss2si_r64_xmmm32_sae;
			yield return Code.VEX_Vcvttsd2si_r64_xmmm64;
			yield return Code.EVEX_Vcvttsd2si_r64_xmmm64_sae;
			yield return Code.VEX_Vcvtss2si_r64_xmmm32;
			yield return Code.EVEX_Vcvtss2si_r64_xmmm32_er;
			yield return Code.VEX_Vcvtsd2si_r64_xmmm64;
			yield return Code.EVEX_Vcvtsd2si_r64_xmmm64_er;
			yield return Code.VEX_Kmovq_k_r64;
			yield return Code.VEX_Kmovq_r64_k;
			yield return Code.Movq_mm_rm64;
			yield return Code.Movq_xmm_rm64;
			yield return Code.VEX_Vmovq_xmm_rm64;
			yield return Code.EVEX_Vmovq_xmm_rm64;
			yield return Code.Movq_rm64_mm;
			yield return Code.Movq_rm64_xmm;
			yield return Code.VEX_Vmovq_rm64_xmm;
			yield return Code.EVEX_Vmovq_rm64_xmm;
			yield return Code.Xbegin_rel32_REXW;
			yield return Code.Swapgs;
			yield return Code.Bndmov_bnd_bndm128;
			yield return Code.Bndcl_bnd_rm64;
			yield return Code.Bndcu_bnd_rm64;
			yield return Code.Bndmov_bndm128_bnd;
			yield return Code.Bndmk_bnd_m64;
			yield return Code.Bndcn_bnd_rm64;
			yield return Code.Nop_rm64;
			yield return Code.Sysexitq;
			yield return Code.Cmovo_r64_rm64;
			yield return Code.Cmovno_r64_rm64;
			yield return Code.Cmovb_r64_rm64;
			yield return Code.Cmovae_r64_rm64;
			yield return Code.Cmove_r64_rm64;
			yield return Code.Cmovne_r64_rm64;
			yield return Code.Cmovbe_r64_rm64;
			yield return Code.Cmova_r64_rm64;
			yield return Code.Cmovs_r64_rm64;
			yield return Code.Cmovns_r64_rm64;
			yield return Code.Cmovp_r64_rm64;
			yield return Code.Cmovnp_r64_rm64;
			yield return Code.Cmovl_r64_rm64;
			yield return Code.Cmovge_r64_rm64;
			yield return Code.Cmovle_r64_rm64;
			yield return Code.Cmovg_r64_rm64;
			yield return Code.Movmskps_r64_xmm;
			yield return Code.VEX_Vmovmskps_r64_xmm;
			yield return Code.VEX_Vmovmskps_r64_ymm;
			yield return Code.Movmskpd_r64_xmm;
			yield return Code.VEX_Vmovmskpd_r64_xmm;
			yield return Code.VEX_Vmovmskpd_r64_ymm;
			yield return Code.EVEX_Vcvttss2usi_r64_xmmm32_sae;
			yield return Code.EVEX_Vcvttsd2usi_r64_xmmm64_sae;
			yield return Code.EVEX_Vcvtss2usi_r64_xmmm32_er;
			yield return Code.EVEX_Vcvtsd2usi_r64_xmmm64_er;
			yield return Code.EVEX_Vcvtusi2ss_xmm_xmm_rm64_er;
			yield return Code.EVEX_Vcvtusi2sd_xmm_xmm_rm64_er;
			yield return Code.Fxsave64_m4096;
			yield return Code.Fxrstor64_m4096;
			yield return Code.Xsave64_m0;
			yield return Code.Xrstor64_m0;
			yield return Code.Ptwrite_rm64;
			yield return Code.Xsaveopt64_m0;
			yield return Code.Rdfsbase_r32;
			yield return Code.Rdfsbase_r64;
			yield return Code.Rdgsbase_r32;
			yield return Code.Rdgsbase_r64;
			yield return Code.Wrfsbase_r32;
			yield return Code.Wrfsbase_r64;
			yield return Code.Wrgsbase_r32;
			yield return Code.Wrgsbase_r64;
			yield return Code.Popcnt_r64_rm64;
			yield return Code.Tzcnt_r64_rm64;
			yield return Code.Lzcnt_r64_rm64;
			yield return Code.Movnti_m64_r64;
			yield return Code.Pinsrw_mm_r64m16_imm8;
			yield return Code.Pinsrw_xmm_r64m16_imm8;
			yield return Code.VEX_Vpinsrw_xmm_xmm_r64m16_imm8;
			yield return Code.EVEX_Vpinsrw_xmm_xmm_r64m16_imm8;
			yield return Code.Pextrw_r64_mm_imm8;
			yield return Code.Pextrw_r64_xmm_imm8;
			yield return Code.VEX_Vpextrw_r64_xmm_imm8;
			yield return Code.EVEX_Vpextrw_r64_xmm_imm8;
			yield return Code.Cmpxchg16b_m128;
			yield return Code.Xrstors64_m0;
			yield return Code.Xsavec64_m0;
			yield return Code.Xsaves64_m0;
			yield return Code.Rdrand_r64;
			yield return Code.Rdseed_r64;
			yield return Code.Rdpid_r64;
			yield return Code.Pmovmskb_r64_mm;
			yield return Code.Pmovmskb_r64_xmm;
			yield return Code.VEX_Vpmovmskb_r64_xmm;
			yield return Code.VEX_Vpmovmskb_r64_ymm;
			yield return Code.Ud0_r64_rm64;
			yield return Code.Vmread_rm64_r64;
			yield return Code.Vmwrite_r64_rm64;
			yield return Code.Invept_r64_m128;
			yield return Code.Invvpid_r64_m128;
			yield return Code.Invpcid_r64_m128;
			yield return Code.EVEX_Vpbroadcastq_xmm_k1z_r64;
			yield return Code.EVEX_Vpbroadcastq_ymm_k1z_r64;
			yield return Code.EVEX_Vpbroadcastq_zmm_k1z_r64;
			yield return Code.Movbe_r64_m64;
			yield return Code.Movbe_m64_r64;
			yield return Code.Crc32_r64_rm8;
			yield return Code.Crc32_r64_rm64;
			yield return Code.VEX_Andn_r64_r64_rm64;
			yield return Code.VEX_Blsr_r64_rm64;
			yield return Code.VEX_Blsmsk_r64_rm64;
			yield return Code.VEX_Blsi_r64_rm64;
			yield return Code.VEX_Bzhi_r64_rm64_r64;
			yield return Code.VEX_Pext_r64_r64_rm64;
			yield return Code.VEX_Pdep_r64_r64_rm64;
			yield return Code.VEX_Mulx_r64_r64_rm64;
			yield return Code.Adcx_r64_rm64;
			yield return Code.Adox_r64_rm64;
			yield return Code.VEX_Bextr_r64_rm64_r64;
			yield return Code.VEX_Shlx_r64_rm64_r64;
			yield return Code.VEX_Sarx_r64_rm64_r64;
			yield return Code.VEX_Shrx_r64_rm64_r64;
			yield return Code.Pextrb_r64m8_xmm_imm8;
			yield return Code.VEX_Vpextrb_r64m8_xmm_imm8;
			yield return Code.EVEX_Vpextrb_r64m8_xmm_imm8;
			yield return Code.Pextrw_r64m16_xmm_imm8;
			yield return Code.VEX_Vpextrw_r64m16_xmm_imm8;
			yield return Code.EVEX_Vpextrw_r64m16_xmm_imm8;
			yield return Code.Pextrq_rm64_xmm_imm8;
			yield return Code.VEX_Vpextrq_rm64_xmm_imm8;
			yield return Code.EVEX_Vpextrq_rm64_xmm_imm8;
			yield return Code.Extractps_rm64_xmm_imm8;
			yield return Code.VEX_Vextractps_rm64_xmm_imm8;
			yield return Code.EVEX_Vextractps_rm64_xmm_imm8;
			yield return Code.Pinsrb_xmm_r64m8_imm8;
			yield return Code.VEX_Vpinsrb_xmm_xmm_r64m8_imm8;
			yield return Code.EVEX_Vpinsrb_xmm_xmm_r64m8_imm8;
			yield return Code.Pinsrq_xmm_rm64_imm8;
			yield return Code.VEX_Vpinsrq_xmm_xmm_rm64_imm8;
			yield return Code.EVEX_Vpinsrq_xmm_xmm_rm64_imm8;
			yield return Code.VEX_Rorx_r64_rm64_imm8;
			yield return Code.Monitorq;
			yield return Code.Pcmpestrm64_xmm_xmmm128_imm8;
			yield return Code.VEX_Vpcmpestrm64_xmm_xmmm128_imm8;
			yield return Code.Pcmpestri64_xmm_xmmm128_imm8;
			yield return Code.VEX_Vpcmpestri64_xmm_xmmm128_imm8;
			yield return Code.Vmrunq;
			yield return Code.Vmloadq;
			yield return Code.Vmsaveq;
			yield return Code.Invlpgaq;
			yield return Code.Monitorxq;
			yield return Code.Clzeroq;
			yield return Code.XOP_Blcfill_r64_rm64;
			yield return Code.XOP_Blsfill_r64_rm64;
			yield return Code.XOP_Blcs_r64_rm64;
			yield return Code.XOP_Tzmsk_r64_rm64;
			yield return Code.XOP_Blcic_r64_rm64;
			yield return Code.XOP_Blsic_r64_rm64;
			yield return Code.XOP_T1mskc_r64_rm64;
			yield return Code.XOP_Blcmsk_r64_rm64;
			yield return Code.XOP_Blci_r64_rm64;
			yield return Code.XOP_Llwpcb_r64;
			yield return Code.XOP_Slwpcb_r64;
			yield return Code.XOP_Bextr_r64_rm64_imm32;
			yield return Code.XOP_Lwpins_r64_rm32_imm32;
			yield return Code.XOP_Lwpval_r64_rm32_imm32;
		}

		public static IEnumerable<DecoderTestInfo> GetDecoderTests(bool needHexBytes, bool includeOtherTests) {
#if DEBUG
			needHexBytes = true;
#endif
			var codeNames = Enum.GetNames(typeof(Code));
			var nameToIndex = new Dictionary<string, int>(codeNames.Length, StringComparer.Ordinal);
			for (int i = 0; i < codeNames.Length; i++) {
				if ((Code)i == Code.INVALID)
					continue;
				nameToIndex.Add(codeNames[i], i);
			}

			var otherTests = new HashSet<string>(StringComparer.Ordinal);
			otherTests.Add(nameof(PrefixTests));

			var thisType = typeof(DecoderTestUtils);
			foreach (var type in thisType.Assembly.GetTypes()) {
				if (!type.IsPublic || type.Namespace != "Iced.UnitTests.Intel.DecoderTests")
					continue;
				bool isDecoderTest;
				// Only instruction tests have this prefix
				if (type.Name.StartsWith("DecoderTest_"))
					isDecoderTest = true;
				else if (type.Name.StartsWith("MemoryTest"))
					isDecoderTest = false;
				else if (otherTests.Contains(type.Name))
					isDecoderTest = false;
				else
					continue;
				if (!isDecoderTest && !includeOtherTests)
					continue;

				foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)) {
					var name = method.Name;
					if (!name.StartsWith("Test", StringComparison.Ordinal))
						continue;
					int bitness;
					if (name.StartsWith("Test16_"))
						bitness = 16;
					else if (name.StartsWith("Test32_"))
						bitness = 32;
					else if (name.StartsWith("Test64_"))
						bitness = 64;
					else
						continue;
					Assert.True(method.IsDefined(typeof(FactAttribute), inherit: false));

					bool hasCodeInMethodName;
					int codeValueIndex;
					if (isDecoderTest) {
						// Verify that name matches pattern Test{16,32,64}_{OptionalCodeValue}_{number}
						int index = name.LastIndexOf('_');
						Assert.False(index < 0);
						for (int i = index + 1; i < name.Length; i++)
							Assert.True(char.IsNumber(name[i]));
						var codeName = name.Substring("Test16_".Length, index - "Test16_".Length);
						hasCodeInMethodName = nameToIndex.TryGetValue(codeName, out codeValueIndex);
						Assert.True(!hasCodeInMethodName || (Code)codeValueIndex != Code.INVALID);
					}
					else {
						hasCodeInMethodName = false;
						codeValueIndex = 0;
					}

					bool hadInlineCodeValue = false;
					foreach (var ca in method.GetCustomAttributesData()) {
						if (ca.AttributeType == typeof(InlineDataAttribute)) {
							Assert.Equal(1, ca.ConstructorArguments.Count);
							var values = (IList<CustomAttributeTypedArgument>)ca.ConstructorArguments[0].Value;
							Assert.True(values.Count >= 2);
							Assert.True(values[0].ArgumentType == typeof(string));
							Assert.True(values[1].ArgumentType == typeof(int));
							Code code;
							if (values.Count >= 3 && values[2].ArgumentType == typeof(Code)) {
								code = (Code)(int)values[2].Value;
								Assert.True(!hasCodeInMethodName || (Code)codeValueIndex == code);
							}
							else {
								Assert.True(hasCodeInMethodName);
								code = (Code)codeValueIndex;
							}
							hadInlineCodeValue = true;
							yield return new DecoderTestInfo(bitness, code, (string)values[0].Value);
						}
						else if (ca.AttributeType == typeof(MemberDataAttribute)) {
							Assert.Equal(2, ca.ConstructorArguments.Count);
							var propertyName = (string)ca.ConstructorArguments[0].Value;
							var testCaseValues = (IEnumerable<object[]>)type.GetProperty(propertyName).GetGetMethod().Invoke(null, Array.Empty<object>());
							foreach (var tc in testCaseValues) {
								Assert.True(tc.Length >= 2);
								if (!isDecoderTest && tc[1] is Code) {
									Assert.True(tc.Length >= 2);
									Assert.True(tc[0] is string);
									Assert.True(tc[1] is Code);
									yield return new DecoderTestInfo(bitness, (Code)tc[1], (string)tc[0]);
								}
								else {
									Assert.True(tc.Length >= 3);
									Assert.True(tc[0] is string);
									Assert.True(tc[1] is int);
									Assert.True(tc[2] is Code);
									yield return new DecoderTestInfo(bitness, (Code)tc[2], (string)tc[0]);
								}
								hadInlineCodeValue = true;
							}
						}
					}

					if (!hadInlineCodeValue) {
						Assert.True(hasCodeInMethodName);
						string hexBytes;
						if (!needHexBytes)
							hexBytes = null;
						else {
							hexBytes = GetHexBytes(method);
							Assert.NotNull(hexBytes);
						}
						yield return new DecoderTestInfo(bitness, (Code)codeValueIndex, hexBytes);
					}
				}
			}
		}

		static string GetHexBytes(MethodBase method) {
			var ilCode = method.GetMethodBody().GetILAsByteArray();
			var stream = new MemoryStream(ilCode);
			var reader = new BinaryReader(stream);
			const int Ldstr = 0x0072;
			string prevStr = null;
			for (;;) {
				int opc = reader.ReadByte();
				if (opc == 0xFE)
					opc = (opc << 8) + reader.ReadByte();
				if (!toOpCode.TryGetValue(opc, out var opCode))
					throw new InvalidOperationException();

				var operand = ReadOperand(reader, opCode);

				string currStr = null;
				if (opCode.Value == Ldstr) {
					currStr = method.Module.ResolveString((int)operand);
					Assert.NotNull(currStr);
					if (prevStr != null) {
						if (currStr == method.Name)
							return prevStr;
					}
				}
				prevStr = currStr;
			}
		}

		static ulong ReadOperand(BinaryReader reader, OpCode opCode) {
			switch (opCode.OperandType) {
			case OperandType.InlineBrTarget:
			case OperandType.InlineField:
			case OperandType.InlineI:
			case OperandType.InlineMethod:
			case OperandType.InlineSig:
			case OperandType.InlineString:
			case OperandType.InlineTok:
			case OperandType.InlineType:
			case OperandType.ShortInlineR:
				return reader.ReadUInt32();

			case OperandType.InlineI8:
			case OperandType.InlineR:
				return reader.ReadUInt64();

			case OperandType.InlineNone:
				return 0;

			case OperandType.InlineSwitch:
				uint count = reader.ReadUInt32();
				reader.BaseStream.Position += (long)count * 4;
				return count;

			case OperandType.InlineVar:
				return reader.ReadUInt16();

			case OperandType.ShortInlineBrTarget:
			case OperandType.ShortInlineI:
			case OperandType.ShortInlineVar:
				return reader.ReadByte();

			default:
				return 0;
			}
		}

		static readonly Dictionary<int, OpCode> toOpCode = CreateToOpCodes();

		static Dictionary<int, OpCode> CreateToOpCodes() {
			var dict = new Dictionary<int, OpCode>();
			foreach (var info in typeof(OpCodes).GetFields(BindingFlags.Public | BindingFlags.Static)) {
				var sreOpCode = (OpCode)info.GetValue(null);
				dict[sreOpCode.Value] = sreOpCode;
			}
			return dict;
		}
	}
}
