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
			yield return Code.Bound_Gw_Mw2;
			yield return Code.Bound_Gd_Md2;
			yield return Code.Arpl_Ew_Gw;
			yield return Code.Push_Id;
			yield return Code.Push_Ib32;
			yield return Code.Jo_Jb16;
			yield return Code.Jo_Jb32;
			yield return Code.Jno_Jb16;
			yield return Code.Jno_Jb32;
			yield return Code.Jb_Jb16;
			yield return Code.Jb_Jb32;
			yield return Code.Jae_Jb16;
			yield return Code.Jae_Jb32;
			yield return Code.Je_Jb16;
			yield return Code.Je_Jb32;
			yield return Code.Jne_Jb16;
			yield return Code.Jne_Jb32;
			yield return Code.Jbe_Jb16;
			yield return Code.Jbe_Jb32;
			yield return Code.Ja_Jb16;
			yield return Code.Ja_Jb32;
			yield return Code.Js_Jb16;
			yield return Code.Js_Jb32;
			yield return Code.Jns_Jb16;
			yield return Code.Jns_Jb32;
			yield return Code.Jp_Jb16;
			yield return Code.Jp_Jb32;
			yield return Code.Jnp_Jb16;
			yield return Code.Jnp_Jb32;
			yield return Code.Jl_Jb16;
			yield return Code.Jl_Jb32;
			yield return Code.Jge_Jb16;
			yield return Code.Jge_Jb32;
			yield return Code.Jle_Jb16;
			yield return Code.Jle_Jb32;
			yield return Code.Jg_Jb16;
			yield return Code.Jg_Jb32;
			yield return Code.Pop_Ed;
			yield return Code.Call_Aww;
			yield return Code.Call_Adw;
			yield return Code.Pushfd;
			yield return Code.Popfd;
			yield return Code.Retnw_Iw;
			yield return Code.Retnd_Iw;
			yield return Code.Retnw;
			yield return Code.Retnd;
			yield return Code.Les_Gw_Mp;
			yield return Code.Les_Gd_Mp;
			yield return Code.Lds_Gw_Mp;
			yield return Code.Lds_Gd_Mp;
			yield return Code.Enterd_Iw_Ib;
			yield return Code.Leaved;
			yield return Code.Into;
			yield return Code.Aam_Ib;
			yield return Code.Aad_Ib;
			yield return Code.Salc;
			yield return Code.Loopne_Jb16_CX;
			yield return Code.Loopne_Jb32_CX;
			yield return Code.Loopne_Jb16_ECX;
			yield return Code.Loopne_Jb32_ECX;
			yield return Code.Loope_Jb16_CX;
			yield return Code.Loope_Jb32_CX;
			yield return Code.Loope_Jb16_ECX;
			yield return Code.Loope_Jb32_ECX;
			yield return Code.Loop_Jb16_CX;
			yield return Code.Loop_Jb32_CX;
			yield return Code.Loop_Jb16_ECX;
			yield return Code.Loop_Jb32_ECX;
			yield return Code.Jcxz_Jb16;
			yield return Code.Jcxz_Jb32;
			yield return Code.Jecxz_Jb16;
			yield return Code.Jecxz_Jb32;
			yield return Code.Call_Jw16;
			yield return Code.Call_Jd32;
			yield return Code.Jmp_Jw16;
			yield return Code.Jmp_Jd32;
			yield return Code.Jmp_Aww;
			yield return Code.Jmp_Adw;
			yield return Code.Jmp_Jb16;
			yield return Code.Jmp_Jb32;
			yield return Code.Jo_Jw16;
			yield return Code.Jo_Jd32;
			yield return Code.Jno_Jw16;
			yield return Code.Jno_Jd32;
			yield return Code.Jb_Jw16;
			yield return Code.Jb_Jd32;
			yield return Code.Jae_Jw16;
			yield return Code.Jae_Jd32;
			yield return Code.Je_Jw16;
			yield return Code.Je_Jd32;
			yield return Code.Jne_Jw16;
			yield return Code.Jne_Jd32;
			yield return Code.Jbe_Jw16;
			yield return Code.Jbe_Jd32;
			yield return Code.Ja_Jw16;
			yield return Code.Ja_Jd32;
			yield return Code.Js_Jw16;
			yield return Code.Js_Jd32;
			yield return Code.Jns_Jw16;
			yield return Code.Jns_Jd32;
			yield return Code.Jp_Jw16;
			yield return Code.Jp_Jd32;
			yield return Code.Jnp_Jw16;
			yield return Code.Jnp_Jd32;
			yield return Code.Jl_Jw16;
			yield return Code.Jl_Jd32;
			yield return Code.Jge_Jw16;
			yield return Code.Jge_Jd32;
			yield return Code.Jle_Jw16;
			yield return Code.Jle_Jd32;
			yield return Code.Jg_Jw16;
			yield return Code.Jg_Jd32;
			yield return Code.Pushd_FS;
			yield return Code.Popd_FS;
			yield return Code.Pushd_GS;
			yield return Code.Popd_GS;
			yield return Code.Call_Ew;
			yield return Code.Call_Ed;
			yield return Code.Jmp_Ew;
			yield return Code.Jmp_Ed;
			yield return Code.Push_Ed;
			yield return Code.Mov_Rd_Cd;
			yield return Code.Mov_Rd_Dd;
			yield return Code.Mov_Cd_Rd;
			yield return Code.Mov_Dd_Rd;
			yield return Code.Bndmov_B_BMq;
			yield return Code.Bndcl_B_Ed;
			yield return Code.Bndcu_B_Ed;
			yield return Code.Bndmov_BMq_B;
			yield return Code.Bndmk_B_Md;
			yield return Code.Bndcn_B_Ed;
			yield return Code.Rdpid_Rd;
			yield return Code.Vmread_Ed_Gd;
			yield return Code.Vmwrite_Gd_Ed;
			yield return Code.Invept_Gd_M;
			yield return Code.Invvpid_Gd_M;
			yield return Code.Invpcid_Gd_M;
			yield return Code.Monitorw;
			yield return Code.Sgdtw_Ms;
			yield return Code.Sgdtd_Ms;
			yield return Code.Sidtw_Ms;
			yield return Code.Sidtd_Ms;
			yield return Code.Lgdtw_Ms;
			yield return Code.Lgdtd_Ms;
			yield return Code.Lidtw_Ms;
			yield return Code.Lidtd_Ms;
		}

		static IEnumerable<Code> GetCode64() {
			yield return Code.Add_Eq_Gq;
			yield return Code.Add_Gq_Eq;
			yield return Code.Add_RAX_Id64;
			yield return Code.Or_Eq_Gq;
			yield return Code.Or_Gq_Eq;
			yield return Code.Or_RAX_Id64;
			yield return Code.Adc_Eq_Gq;
			yield return Code.Adc_Gq_Eq;
			yield return Code.Adc_RAX_Id64;
			yield return Code.Sbb_Eq_Gq;
			yield return Code.Sbb_Gq_Eq;
			yield return Code.Sbb_RAX_Id64;
			yield return Code.And_Eq_Gq;
			yield return Code.And_Gq_Eq;
			yield return Code.And_RAX_Id64;
			yield return Code.Sub_Eq_Gq;
			yield return Code.Sub_Gq_Eq;
			yield return Code.Sub_RAX_Id64;
			yield return Code.Xor_Eq_Gq;
			yield return Code.Xor_Gq_Eq;
			yield return Code.Xor_RAX_Id64;
			yield return Code.Cmp_Eq_Gq;
			yield return Code.Cmp_Gq_Eq;
			yield return Code.Cmp_RAX_Id64;
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
			yield return Code.Push_Id64;
			yield return Code.Imul_Gq_Eq_Id64;
			yield return Code.Push_Ib64;
			yield return Code.Imul_Gq_Eq_Ib64;
			yield return Code.Jo_Jb64;
			yield return Code.Jno_Jb64;
			yield return Code.Jb_Jb64;
			yield return Code.Jae_Jb64;
			yield return Code.Je_Jb64;
			yield return Code.Jne_Jb64;
			yield return Code.Jbe_Jb64;
			yield return Code.Ja_Jb64;
			yield return Code.Js_Jb64;
			yield return Code.Jns_Jb64;
			yield return Code.Jp_Jb64;
			yield return Code.Jnp_Jb64;
			yield return Code.Jl_Jb64;
			yield return Code.Jge_Jb64;
			yield return Code.Jle_Jb64;
			yield return Code.Jg_Jb64;
			yield return Code.Add_Eq_Id64;
			yield return Code.Or_Eq_Id64;
			yield return Code.Adc_Eq_Id64;
			yield return Code.Sbb_Eq_Id64;
			yield return Code.And_Eq_Id64;
			yield return Code.Sub_Eq_Id64;
			yield return Code.Xor_Eq_Id64;
			yield return Code.Cmp_Eq_Id64;
			yield return Code.Add_Eq_Ib64;
			yield return Code.Or_Eq_Ib64;
			yield return Code.Adc_Eq_Ib64;
			yield return Code.Sbb_Eq_Ib64;
			yield return Code.And_Eq_Ib64;
			yield return Code.Sub_Eq_Ib64;
			yield return Code.Xor_Eq_Ib64;
			yield return Code.Cmp_Eq_Ib64;
			yield return Code.Test_Eq_Gq;
			yield return Code.Xchg_Eq_Gq;
			yield return Code.Mov_Eq_Gq;
			yield return Code.Mov_Gq_Eq;
			yield return Code.Mov_Eq_Sw;
			yield return Code.Lea_Gq_M;
			yield return Code.Mov_Sw_Eq;
			yield return Code.Pop_Eq;
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
			yield return Code.Mov_RAX_Oq;
			yield return Code.Mov_Oq_RAX;
			yield return Code.Movsq_Yq_Xq;
			yield return Code.Cmpsq_Xq_Yq;
			yield return Code.Test_RAX_Id64;
			yield return Code.Stosq_Yq_RAX;
			yield return Code.Lodsq_RAX_Xq;
			yield return Code.Scasq_RAX_Yq;
			yield return Code.Mov_RAX_Iq;
			yield return Code.Mov_R8_Iq;
			yield return Code.Mov_RCX_Iq;
			yield return Code.Mov_R9_Iq;
			yield return Code.Mov_RDX_Iq;
			yield return Code.Mov_R10_Iq;
			yield return Code.Mov_RBX_Iq;
			yield return Code.Mov_R11_Iq;
			yield return Code.Mov_RSP_Iq;
			yield return Code.Mov_R12_Iq;
			yield return Code.Mov_RBP_Iq;
			yield return Code.Mov_R13_Iq;
			yield return Code.Mov_RSI_Iq;
			yield return Code.Mov_R14_Iq;
			yield return Code.Mov_RDI_Iq;
			yield return Code.Mov_R15_Iq;
			yield return Code.Rol_Eq_Ib;
			yield return Code.Ror_Eq_Ib;
			yield return Code.Rcl_Eq_Ib;
			yield return Code.Rcr_Eq_Ib;
			yield return Code.Shl_Eq_Ib;
			yield return Code.Shr_Eq_Ib;
			yield return Code.Sar_Eq_Ib;
			yield return Code.Retnq_Iw;
			yield return Code.Retnq;
			yield return Code.Mov_Eq_Id64;
			yield return Code.Enterq_Iw_Ib;
			yield return Code.Leaveq;
			yield return Code.Retfq_Iw;
			yield return Code.Retfq;
			yield return Code.Iretq;
			yield return Code.Rol_Eq_1;
			yield return Code.Ror_Eq_1;
			yield return Code.Rcl_Eq_1;
			yield return Code.Rcr_Eq_1;
			yield return Code.Shl_Eq_1;
			yield return Code.Shr_Eq_1;
			yield return Code.Sar_Eq_1;
			yield return Code.Rol_Eq_CL;
			yield return Code.Ror_Eq_CL;
			yield return Code.Rcl_Eq_CL;
			yield return Code.Rcr_Eq_CL;
			yield return Code.Shl_Eq_CL;
			yield return Code.Shr_Eq_CL;
			yield return Code.Sar_Eq_CL;
			yield return Code.Loopne_Jb64_ECX;
			yield return Code.Loopne_Jb64_RCX;
			yield return Code.Loope_Jb64_ECX;
			yield return Code.Loope_Jb64_RCX;
			yield return Code.Loop_Jb64_ECX;
			yield return Code.Loop_Jb64_RCX;
			yield return Code.Jecxz_Jb64;
			yield return Code.Jrcxz_Jb64;
			yield return Code.Call_Jd64;
			yield return Code.Jmp_Jd64;
			yield return Code.Jmp_Jb64;
			yield return Code.Test_Eq_Id64;
			yield return Code.Not_Eq;
			yield return Code.Neg_Eq;
			yield return Code.Mul_Eq;
			yield return Code.Imul_Eq;
			yield return Code.Div_Eq;
			yield return Code.Idiv_Eq;
			yield return Code.Inc_Eq;
			yield return Code.Dec_Eq;
			yield return Code.Call_Eq;
			yield return Code.Call_Eqw;
			yield return Code.Jmp_Eq;
			yield return Code.Jmp_Eqw;
			yield return Code.Push_Eq;
			yield return Code.Sldtq_Ew;
			yield return Code.Strq_Ew;
			yield return Code.Lldtq_Ew;
			yield return Code.Ltrq_Ew;
			yield return Code.Verrq_Ew;
			yield return Code.Verwq_Ew;
			yield return Code.Sgdtq_Ms;
			yield return Code.Sidtq_Ms;
			yield return Code.Lgdtq_Ms;
			yield return Code.Lidtq_Ms;
			yield return Code.Smswq_Ew;
			yield return Code.Lmswq_Ew;
			yield return Code.Lar_Gq_Eq;
			yield return Code.Lsl_Gq_Eq;
			yield return Code.Mov_Rq_Cq;
			yield return Code.Mov_Rq_Dq;
			yield return Code.Mov_Cq_Rq;
			yield return Code.Mov_Dq_Rq;
			yield return Code.Jo_Jd64;
			yield return Code.Jno_Jd64;
			yield return Code.Jb_Jd64;
			yield return Code.Jae_Jd64;
			yield return Code.Je_Jd64;
			yield return Code.Jne_Jd64;
			yield return Code.Jbe_Jd64;
			yield return Code.Ja_Jd64;
			yield return Code.Js_Jd64;
			yield return Code.Jns_Jd64;
			yield return Code.Jp_Jd64;
			yield return Code.Jnp_Jd64;
			yield return Code.Jl_Jd64;
			yield return Code.Jge_Jd64;
			yield return Code.Jle_Jd64;
			yield return Code.Jg_Jd64;
			yield return Code.Pushq_FS;
			yield return Code.Popq_FS;
			yield return Code.Bt_Eq_Gq;
			yield return Code.Shld_Eq_Gq_Ib;
			yield return Code.Shld_Eq_Gq_CL;
			yield return Code.Pushq_GS;
			yield return Code.Popq_GS;
			yield return Code.Bts_Eq_Gq;
			yield return Code.Shrd_Eq_Gq_Ib;
			yield return Code.Shrd_Eq_Gq_CL;
			yield return Code.Imul_Gq_Eq;
			yield return Code.Cmpxchg_Eq_Gq;
			yield return Code.Lss_Gq_Mp;
			yield return Code.Btr_Eq_Gq;
			yield return Code.Lfs_Gq_Mp;
			yield return Code.Lgs_Gq_Mp;
			yield return Code.Movzx_Gq_Eb;
			yield return Code.Movzx_Gq_Ew;
			yield return Code.Ud1_Gq_Eq;
			yield return Code.Bt_Eq_Ib;
			yield return Code.Bts_Eq_Ib;
			yield return Code.Btr_Eq_Ib;
			yield return Code.Btc_Eq_Ib;
			yield return Code.Btc_Eq_Gq;
			yield return Code.Bsf_Gq_Eq;
			yield return Code.Bsr_Gq_Eq;
			yield return Code.Movsx_Gq_Eb;
			yield return Code.Movsx_Gq_Ew;
			yield return Code.Xadd_Eq_Gq;
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
			yield return Code.Mov_R8L_Ib;
			yield return Code.Mov_R9L_Ib;
			yield return Code.Mov_R10L_Ib;
			yield return Code.Mov_R11L_Ib;
			yield return Code.Mov_SPL_Ib;
			yield return Code.Mov_R12L_Ib;
			yield return Code.Mov_BPL_Ib;
			yield return Code.Mov_R13L_Ib;
			yield return Code.Mov_SIL_Ib;
			yield return Code.Mov_R14L_Ib;
			yield return Code.Mov_DIL_Ib;
			yield return Code.Mov_R15L_Ib;
			yield return Code.Mov_R8W_Iw;
			yield return Code.Mov_R8D_Id;
			yield return Code.Mov_R9W_Iw;
			yield return Code.Mov_R9D_Id;
			yield return Code.Mov_R10W_Iw;
			yield return Code.Mov_R10D_Id;
			yield return Code.Mov_R11W_Iw;
			yield return Code.Mov_R11D_Id;
			yield return Code.Mov_R12W_Iw;
			yield return Code.Mov_R12D_Id;
			yield return Code.Mov_R13W_Iw;
			yield return Code.Mov_R13D_Id;
			yield return Code.Mov_R14W_Iw;
			yield return Code.Mov_R14D_Id;
			yield return Code.Mov_R15W_Iw;
			yield return Code.Mov_R15D_Id;
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
			yield return Code.Movsxd_Gw_Ew;
			yield return Code.Movsxd_Gd_Ed;
			yield return Code.Movsxd_Gq_Ed;
			yield return Code.Syscall;
			yield return Code.Sysretd;
			yield return Code.Sysretq;
			yield return Code.Cvtsi2ss_VX_Eq;
			yield return Code.Cvtsi2sd_VX_Eq;
			yield return Code.Cvttss2si_Gq_WX;
			yield return Code.Cvttsd2si_Gq_WX;
			yield return Code.Cvtss2si_Gq_WX;
			yield return Code.Cvtsd2si_Gq_WX;
			yield return Code.VEX_Vcvtsi2ss_VX_HX_Eq;
			yield return Code.EVEX_Vcvtsi2ss_VX_HX_Eq_er;
			yield return Code.VEX_Vcvtsi2sd_VX_HX_Eq;
			yield return Code.EVEX_Vcvtsi2sd_VX_HX_Eq_er;
			yield return Code.VEX_Vcvttss2si_Gq_WX;
			yield return Code.EVEX_Vcvttss2si_Gq_WX_sae;
			yield return Code.VEX_Vcvttsd2si_Gq_WX;
			yield return Code.EVEX_Vcvttsd2si_Gq_WX_sae;
			yield return Code.VEX_Vcvtss2si_Gq_WX;
			yield return Code.EVEX_Vcvtss2si_Gq_WX_er;
			yield return Code.VEX_Vcvtsd2si_Gq_WX;
			yield return Code.EVEX_Vcvtsd2si_Gq_WX_er;
			yield return Code.VEX_Kmovq_VK_Rq;
			yield return Code.VEX_Kmovq_Gq_RK;
			yield return Code.Movq_P_Eq;
			yield return Code.Movq_VX_Eq;
			yield return Code.VEX_Vmovq_VX_Eq;
			yield return Code.EVEX_Vmovq_VX_Eq;
			yield return Code.Movq_Eq_P;
			yield return Code.Movq_Eq_VX;
			yield return Code.VEX_Vmovq_Eq_VX;
			yield return Code.EVEX_Vmovq_Eq_VX;
			yield return Code.Xbegin_Jd64;
			yield return Code.Swapgs;
			yield return Code.Bndmov_B_BMo;
			yield return Code.Bndcl_B_Eq;
			yield return Code.Bndcu_B_Eq;
			yield return Code.Bndmov_BMo_B;
			yield return Code.Bndmk_B_Mq;
			yield return Code.Bndcn_B_Eq;
			yield return Code.Nop_Eq;
			yield return Code.Sysexitq;
			yield return Code.Cmovo_Gq_Eq;
			yield return Code.Cmovno_Gq_Eq;
			yield return Code.Cmovb_Gq_Eq;
			yield return Code.Cmovae_Gq_Eq;
			yield return Code.Cmove_Gq_Eq;
			yield return Code.Cmovne_Gq_Eq;
			yield return Code.Cmovbe_Gq_Eq;
			yield return Code.Cmova_Gq_Eq;
			yield return Code.Cmovs_Gq_Eq;
			yield return Code.Cmovns_Gq_Eq;
			yield return Code.Cmovp_Gq_Eq;
			yield return Code.Cmovnp_Gq_Eq;
			yield return Code.Cmovl_Gq_Eq;
			yield return Code.Cmovge_Gq_Eq;
			yield return Code.Cmovle_Gq_Eq;
			yield return Code.Cmovg_Gq_Eq;
			yield return Code.Movmskps_Gq_RX;
			yield return Code.VEX_Vmovmskps_Gq_RX;
			yield return Code.VEX_Vmovmskps_Gq_RY;
			yield return Code.Movmskpd_Gq_RX;
			yield return Code.VEX_Vmovmskpd_Gq_RX;
			yield return Code.VEX_Vmovmskpd_Gq_RY;
			yield return Code.EVEX_Vcvttss2usi_Gq_WX_sae;
			yield return Code.EVEX_Vcvttsd2usi_Gq_WX_sae;
			yield return Code.EVEX_Vcvtss2usi_Gq_WX_er;
			yield return Code.EVEX_Vcvtsd2usi_Gq_WX_er;
			yield return Code.EVEX_Vcvtusi2ss_VX_HX_Eq_er;
			yield return Code.EVEX_Vcvtusi2sd_VX_HX_Eq_er;
			yield return Code.Fxsave64_M;
			yield return Code.Fxrstor64_M;
			yield return Code.Xsave64_M;
			yield return Code.Xrstor64_M;
			yield return Code.Ptwrite_Eq;
			yield return Code.Xsaveopt64_M;
			yield return Code.Rdfsbase_Rd;
			yield return Code.Rdfsbase_Rq;
			yield return Code.Rdgsbase_Rd;
			yield return Code.Rdgsbase_Rq;
			yield return Code.Wrfsbase_Rd;
			yield return Code.Wrfsbase_Rq;
			yield return Code.Wrgsbase_Rd;
			yield return Code.Wrgsbase_Rq;
			yield return Code.Popcnt_Gq_Eq;
			yield return Code.Tzcnt_Gq_Eq;
			yield return Code.Lzcnt_Gq_Eq;
			yield return Code.Movnti_Mq_Gq;
			yield return Code.Pinsrw_P_RqMw_Ib;
			yield return Code.Pinsrw_VX_RqMw_Ib;
			yield return Code.VEX_Vpinsrw_VX_HX_RqMw_Ib;
			yield return Code.EVEX_Vpinsrw_VX_HX_RqMw_Ib;
			yield return Code.Pextrw_Gq_N_Ib;
			yield return Code.Pextrw_Gq_RX_Ib;
			yield return Code.VEX_Vpextrw_Gq_RX_Ib;
			yield return Code.EVEX_Vpextrw_Gq_RX_Ib;
			yield return Code.Cmpxchg16b_Mo;
			yield return Code.Xrstors64_M;
			yield return Code.Xsavec64_M;
			yield return Code.Xsaves64_M;
			yield return Code.Rdrand_Rq;
			yield return Code.Rdseed_Rq;
			yield return Code.Rdpid_Rq;
			yield return Code.Pmovmskb_Gq_N;
			yield return Code.Pmovmskb_Gq_RX;
			yield return Code.VEX_Vpmovmskb_Gq_RX;
			yield return Code.VEX_Vpmovmskb_Gq_RY;
			yield return Code.Ud0_Gq_Eq;
			yield return Code.Vmread_Eq_Gq;
			yield return Code.Vmwrite_Gq_Eq;
			yield return Code.Invept_Gq_M;
			yield return Code.Invvpid_Gq_M;
			yield return Code.Invpcid_Gq_M;
			yield return Code.EVEX_Vpbroadcastq_VX_k1z_Rq;
			yield return Code.EVEX_Vpbroadcastq_VY_k1z_Rq;
			yield return Code.EVEX_Vpbroadcastq_VZ_k1z_Rq;
			yield return Code.Movbe_Gq_Mq;
			yield return Code.Movbe_Mq_Gq;
			yield return Code.Crc32_Gq_Eb;
			yield return Code.Crc32_Gq_Eq;
			yield return Code.VEX_Andn_Gq_Hq_Eq;
			yield return Code.VEX_Blsr_Hq_Eq;
			yield return Code.VEX_Blsmsk_Hq_Eq;
			yield return Code.VEX_Blsi_Hq_Eq;
			yield return Code.VEX_Bzhi_Gq_Eq_Hq;
			yield return Code.VEX_Pext_Gq_Hq_Eq;
			yield return Code.VEX_Pdep_Gq_Hq_Eq;
			yield return Code.VEX_Mulx_Gq_Hq_Eq;
			yield return Code.Adcx_Gq_Eq;
			yield return Code.Adox_Gq_Eq;
			yield return Code.VEX_Bextr_Gq_Eq_Hq;
			yield return Code.VEX_Shlx_Gq_Eq_Hq;
			yield return Code.VEX_Sarx_Gq_Eq_Hq;
			yield return Code.VEX_Shrx_Gq_Eq_Hq;
			yield return Code.Pextrb_RqMb_VX_Ib;
			yield return Code.VEX_Vpextrb_RqMb_VX_Ib;
			yield return Code.EVEX_Vpextrb_RqMb_VX_Ib;
			yield return Code.Pextrw_RqMw_VX_Ib;
			yield return Code.VEX_Vpextrw_RqMw_VX_Ib;
			yield return Code.EVEX_Vpextrw_RqMw_VX_Ib;
			yield return Code.Pextrq_Eq_VX_Ib;
			yield return Code.VEX_Vpextrq_Eq_VX_Ib;
			yield return Code.EVEX_Vpextrq_Eq_VX_Ib;
			yield return Code.Extractps_Eq_VX_Ib;
			yield return Code.VEX_Vextractps_Eq_VX_Ib;
			yield return Code.EVEX_Vextractps_Eq_VX_Ib;
			yield return Code.Pinsrb_VX_RqMb_Ib;
			yield return Code.VEX_Vpinsrb_VX_HX_RqMb_Ib;
			yield return Code.EVEX_Vpinsrb_VX_HX_RqMb_Ib;
			yield return Code.Pinsrq_VX_Eq_Ib;
			yield return Code.VEX_Vpinsrq_VX_HX_Eq_Ib;
			yield return Code.EVEX_Vpinsrq_VX_HX_Eq_Ib;
			yield return Code.VEX_Rorx_Gq_Eq_Ib;
			yield return Code.Monitorq;
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
