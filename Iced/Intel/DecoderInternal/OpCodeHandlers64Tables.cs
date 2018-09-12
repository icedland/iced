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

#if !NO_DECODER64 && !NO_DECODER
namespace Iced.Intel.DecoderInternal.OpCodeHandlers64 {
	/// <summary>
	/// Handlers for 64-bit mode
	/// </summary>
	static class OpCodeHandlers64Tables {
		internal static readonly OpCodeHandler[] OneByteHandlers;

		static OpCodeHandlers64Tables() {
			// Store it in a local. Instead of 1000+ ldfld, we'll have 1000+ ldloc.0 (save 4 bytes per load)
			var invalid = OpCodeHandler_Invalid.Instance;

			var handlers_Grp_80 = new OpCodeHandler[8] {
				new OpCodeHandler_Eb_Ib(Code.Add_Eb_Ib, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Eb_Ib(Code.Or_Eb_Ib, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Eb_Ib(Code.Adc_Eb_Ib, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Eb_Ib(Code.Sbb_Eb_Ib, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Eb_Ib(Code.And_Eb_Ib, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Eb_Ib(Code.Sub_Eb_Ib, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Eb_Ib(Code.Xor_Eb_Ib, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Eb_Ib(Code.Cmp_Eb_Ib),
			};

			var handlers_Grp_81 = new OpCodeHandler[8] {
				new OpCodeHandler_Ev_Iz(Code.Add_Ew_Iw, Code.Add_Ed_Id, Code.Add_Eq_Id64, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Iz(Code.Or_Ew_Iw, Code.Or_Ed_Id, Code.Or_Eq_Id64, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Iz(Code.Adc_Ew_Iw, Code.Adc_Ed_Id, Code.Adc_Eq_Id64, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Iz(Code.Sbb_Ew_Iw, Code.Sbb_Ed_Id, Code.Sbb_Eq_Id64, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Iz(Code.And_Ew_Iw, Code.And_Ed_Id, Code.And_Eq_Id64, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Iz(Code.Sub_Ew_Iw, Code.Sub_Ed_Id, Code.Sub_Eq_Id64, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Iz(Code.Xor_Ew_Iw, Code.Xor_Ed_Id, Code.Xor_Eq_Id64, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Iz(Code.Cmp_Ew_Iw, Code.Cmp_Ed_Id, Code.Cmp_Eq_Id64),
			};

			var handlers_Grp_83 = new OpCodeHandler[8] {
				new OpCodeHandler_Ev_Ib(Code.Add_Ew_Ib16, Code.Add_Ed_Ib32, Code.Add_Eq_Ib64, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Ib(Code.Or_Ew_Ib16, Code.Or_Ed_Ib32, Code.Or_Eq_Ib64, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Ib(Code.Adc_Ew_Ib16, Code.Adc_Ed_Ib32, Code.Adc_Eq_Ib64, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Ib(Code.Sbb_Ew_Ib16, Code.Sbb_Ed_Ib32, Code.Sbb_Eq_Ib64, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Ib(Code.And_Ew_Ib16, Code.And_Ed_Ib32, Code.And_Eq_Ib64, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Ib(Code.Sub_Ew_Ib16, Code.Sub_Ed_Ib32, Code.Sub_Eq_Ib64, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Ib(Code.Xor_Ew_Ib16, Code.Xor_Ed_Ib32, Code.Xor_Eq_Ib64, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Ib(Code.Cmp_Ew_Ib16, Code.Cmp_Ed_Ib32, Code.Cmp_Eq_Ib64),
			};

			var handlers_Grp_8F = new OpCodeHandler[8] {
				new OpCodeHandler_PushEv(Code.Pop_Ew, Code.Pop_Eq),
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
			};

			var handlers_Grp_C0 = new OpCodeHandler[8] {
				new OpCodeHandler_Eb_Ib(Code.Rol_Eb_Ib),
				new OpCodeHandler_Eb_Ib(Code.Ror_Eb_Ib),
				new OpCodeHandler_Eb_Ib(Code.Rcl_Eb_Ib),
				new OpCodeHandler_Eb_Ib(Code.Rcr_Eb_Ib),
				new OpCodeHandler_Eb_Ib(Code.Shl_Eb_Ib),
				new OpCodeHandler_Eb_Ib(Code.Shr_Eb_Ib),
				new OpCodeHandler_Eb_Ib(Code.Shl_Eb_Ib),
				new OpCodeHandler_Eb_Ib(Code.Sar_Eb_Ib, MemorySize.Int8),
			};

			var handlers_Grp_C1 = new OpCodeHandler[8] {
				new OpCodeHandler_Ev_Ib2(Code.Rol_Ew_Ib, Code.Rol_Ed_Ib, Code.Rol_Eq_Ib),
				new OpCodeHandler_Ev_Ib2(Code.Ror_Ew_Ib, Code.Ror_Ed_Ib, Code.Ror_Eq_Ib),
				new OpCodeHandler_Ev_Ib2(Code.Rcl_Ew_Ib, Code.Rcl_Ed_Ib, Code.Rcl_Eq_Ib),
				new OpCodeHandler_Ev_Ib2(Code.Rcr_Ew_Ib, Code.Rcr_Ed_Ib, Code.Rcr_Eq_Ib),
				new OpCodeHandler_Ev_Ib2(Code.Shl_Ew_Ib, Code.Shl_Ed_Ib, Code.Shl_Eq_Ib),
				new OpCodeHandler_Ev_Ib2(Code.Shr_Ew_Ib, Code.Shr_Ed_Ib, Code.Shr_Eq_Ib),
				new OpCodeHandler_Ev_Ib2(Code.Shl_Ew_Ib, Code.Shl_Ed_Ib, Code.Shl_Eq_Ib),
				new OpCodeHandler_Ev_Ib2(Code.Sar_Ew_Ib, Code.Sar_Ed_Ib, Code.Sar_Eq_Ib, true),
			};

			var handlers_Grp_D0 = new OpCodeHandler[8] {
				new OpCodeHandler_Eb_1(Code.Rol_Eb_1),
				new OpCodeHandler_Eb_1(Code.Ror_Eb_1),
				new OpCodeHandler_Eb_1(Code.Rcl_Eb_1),
				new OpCodeHandler_Eb_1(Code.Rcr_Eb_1),
				new OpCodeHandler_Eb_1(Code.Shl_Eb_1),
				new OpCodeHandler_Eb_1(Code.Shr_Eb_1),
				new OpCodeHandler_Eb_1(Code.Shl_Eb_1),
				new OpCodeHandler_Eb_1(Code.Sar_Eb_1, MemorySize.Int8),
			};

			var handlers_Grp_D1 = new OpCodeHandler[8] {
				new OpCodeHandler_Ev_1(Code.Rol_Ew_1, Code.Rol_Ed_1, Code.Rol_Eq_1),
				new OpCodeHandler_Ev_1(Code.Ror_Ew_1, Code.Ror_Ed_1, Code.Ror_Eq_1),
				new OpCodeHandler_Ev_1(Code.Rcl_Ew_1, Code.Rcl_Ed_1, Code.Rcl_Eq_1),
				new OpCodeHandler_Ev_1(Code.Rcr_Ew_1, Code.Rcr_Ed_1, Code.Rcr_Eq_1),
				new OpCodeHandler_Ev_1(Code.Shl_Ew_1, Code.Shl_Ed_1, Code.Shl_Eq_1),
				new OpCodeHandler_Ev_1(Code.Shr_Ew_1, Code.Shr_Ed_1, Code.Shr_Eq_1),
				new OpCodeHandler_Ev_1(Code.Shl_Ew_1, Code.Shl_Ed_1, Code.Shl_Eq_1),
				new OpCodeHandler_Ev_1(Code.Sar_Ew_1, Code.Sar_Ed_1, Code.Sar_Eq_1, true),
			};

			var handlers_Grp_D2 = new OpCodeHandler[8] {
				new OpCodeHandler_Eb_CL(Code.Rol_Eb_CL),
				new OpCodeHandler_Eb_CL(Code.Ror_Eb_CL),
				new OpCodeHandler_Eb_CL(Code.Rcl_Eb_CL),
				new OpCodeHandler_Eb_CL(Code.Rcr_Eb_CL),
				new OpCodeHandler_Eb_CL(Code.Shl_Eb_CL),
				new OpCodeHandler_Eb_CL(Code.Shr_Eb_CL),
				new OpCodeHandler_Eb_CL(Code.Shl_Eb_CL),
				new OpCodeHandler_Eb_CL(Code.Sar_Eb_CL, MemorySize.Int8),
			};

			var handlers_Grp_D3 = new OpCodeHandler[8] {
				new OpCodeHandler_Ev_CL(Code.Rol_Ew_CL, Code.Rol_Ed_CL, Code.Rol_Eq_CL),
				new OpCodeHandler_Ev_CL(Code.Ror_Ew_CL, Code.Ror_Ed_CL, Code.Ror_Eq_CL),
				new OpCodeHandler_Ev_CL(Code.Rcl_Ew_CL, Code.Rcl_Ed_CL, Code.Rcl_Eq_CL),
				new OpCodeHandler_Ev_CL(Code.Rcr_Ew_CL, Code.Rcr_Ed_CL, Code.Rcr_Eq_CL),
				new OpCodeHandler_Ev_CL(Code.Shl_Ew_CL, Code.Shl_Ed_CL, Code.Shl_Eq_CL),
				new OpCodeHandler_Ev_CL(Code.Shr_Ew_CL, Code.Shr_Ed_CL, Code.Shr_Eq_CL),
				new OpCodeHandler_Ev_CL(Code.Shl_Ew_CL, Code.Shl_Ed_CL, Code.Shl_Eq_CL),
				new OpCodeHandler_Ev_CL(Code.Sar_Ew_CL, Code.Sar_Ed_CL, Code.Sar_Eq_CL, true),
			};

			var handlers_Grp_F6 = new OpCodeHandler[8] {
				new OpCodeHandler_Eb_Ib(Code.Test_Eb_Ib),
				new OpCodeHandler_Eb_Ib(Code.Test_Eb_Ib),
				new OpCodeHandler_Eb(Code.Not_Eb, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Eb(Code.Neg_Eb, MemorySize.Int8, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Eb(Code.Mul_Eb),
				new OpCodeHandler_Eb(Code.Imul_Eb, MemorySize.Int8),
				new OpCodeHandler_Eb(Code.Div_Eb),
				new OpCodeHandler_Eb(Code.Idiv_Eb, MemorySize.Int8),
			};

			var handlers_Grp_F7 = new OpCodeHandler[8] {
				new OpCodeHandler_Ev_Iz(Code.Test_Ew_Iw, Code.Test_Ed_Id, Code.Test_Eq_Id64),
				new OpCodeHandler_Ev_Iz(Code.Test_Ew_Iw, Code.Test_Ed_Id, Code.Test_Eq_Id64),
				new OpCodeHandler_Ev(Code.Not_Ew, Code.Not_Ed, Code.Not_Eq, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev(Code.Neg_Ew, Code.Neg_Ed, Code.Neg_Eq, true, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev(Code.Mul_Ew, Code.Mul_Ed, Code.Mul_Eq),
				new OpCodeHandler_Ev(Code.Imul_Ew, Code.Imul_Ed, Code.Imul_Eq, true),
				new OpCodeHandler_Ev(Code.Div_Ew, Code.Div_Ed, Code.Div_Eq),
				new OpCodeHandler_Ev(Code.Idiv_Ew, Code.Idiv_Ed, Code.Idiv_Eq, true),
			};

			var handlers_Grp_FE = new OpCodeHandler[8] {
				new OpCodeHandler_Eb(Code.Inc_Eb, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Eb(Code.Dec_Eb, HandlerFlags.XacquireRelease),
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
			};

			var handlers_Grp_FF = new OpCodeHandler[8] {
				new OpCodeHandler_Ev(Code.Inc_Ew, Code.Inc_Ed, Code.Inc_Eq, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev(Code.Dec_Ew, Code.Dec_Ed, Code.Dec_Eq, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Evj(Code.Call_Eq),
				new OpCodeHandler_Ep(Code.Call_Eww, Code.Call_Edw, Code.Call_Eqw),
				new OpCodeHandler_Evj(Code.Jmp_Eq),
				new OpCodeHandler_Ep(Code.Jmp_Eww, Code.Jmp_Edw, Code.Jmp_Eqw),
				new OpCodeHandler_PushEv(Code.Push_Ew, Code.Push_Eq),
				invalid,
			};

			var handlers_Grp_0F00 = new OpCodeHandler[8] {
				new OpCodeHandler_Evw(Code.Sldt_Ew, Code.Sldt_RdMw, Code.Sldt_RqMw),
				new OpCodeHandler_Evw(Code.Str_Ew, Code.Str_RdMw, Code.Str_RqMw),
				new OpCodeHandler_Ew(Code.Lldt_Ew, Code.Lldt_RdMw, Code.Lldt_RqMw),
				new OpCodeHandler_Ew(Code.Ltr_Ew, Code.Ltr_RdMw, Code.Ltr_RqMw),
				new OpCodeHandler_Ew(Code.Verr_Ew, Code.Verr_RdMw, Code.Verr_RqMw),
				new OpCodeHandler_Ew(Code.Verw_Ew, Code.Verw_RdMw, Code.Verw_RqMw),
				invalid,
				invalid,
			};

			var handlers_Grp_0F01_lo = new OpCodeHandler[8] {
				new OpCodeHandler_Ms(Code.Sgdtw_Ms, Code.Sgdtd_Ms, Code.Sgdtq_Ms),
				new OpCodeHandler_Ms(Code.Sidtw_Ms, Code.Sidtd_Ms, Code.Sidtq_Ms),
				new OpCodeHandler_Ms(Code.Lgdtw_Ms, Code.Lgdtd_Ms, Code.Lgdtq_Ms),
				new OpCodeHandler_Ms(Code.Lidtw_Ms, Code.Lidtd_Ms, Code.Lidtq_Ms),
				new OpCodeHandler_Evw(Code.Smsw_Ew, Code.Smsw_RdMw, Code.Smsw_RqMw),
				invalid,
				new OpCodeHandler_Evw(Code.Lmsw_Ew, Code.Lmsw_RdMw, Code.Lmsw_RqMw),
				new OpCodeHandler_M(Code.Invlpg_M, MemorySize.Unknown),
			};

			var handlers_Grp_0F01_hi = new OpCodeHandler[0x40] {
				// C0
				new OpCodeHandler_Simple(Code.Enclv),
				new OpCodeHandler_Simple(Code.Vmcall),
				new OpCodeHandler_Simple(Code.Vmlaunch),
				new OpCodeHandler_Simple(Code.Vmresume),
				new OpCodeHandler_Simple(Code.Vmxoff),
				null,
				null,
				null,

				// C8
				new OpCodeHandler_Simple5(Code.Monitorw, Code.Monitord, Code.Monitorq),
				new OpCodeHandler_Simple(Code.Mwait),
				new OpCodeHandler_Simple(Code.Clac),
				new OpCodeHandler_Simple(Code.Stac),
				null,
				null,
				null,
				new OpCodeHandler_Simple(Code.Encls),

				// D0
				new OpCodeHandler_Simple(Code.Xgetbv),
				new OpCodeHandler_Simple(Code.Xsetbv),
				null,
				null,
				new OpCodeHandler_Simple(Code.Vmfunc),
				new OpCodeHandler_Simple(Code.Xend),
				new OpCodeHandler_Simple(Code.Xtest),
				new OpCodeHandler_Simple(Code.Enclu),

				// D8
				new OpCodeHandler_Simple5(Code.Vmrunw, Code.Vmrund, Code.Vmrunq),
				new OpCodeHandler_Simple(Code.Vmmcall),
				new OpCodeHandler_Simple5(Code.Vmloadw, Code.Vmloadd, Code.Vmloadq),
				new OpCodeHandler_Simple5(Code.Vmsavew, Code.Vmsaved, Code.Vmsaveq),
				new OpCodeHandler_Simple(Code.Stgi),
				new OpCodeHandler_Simple(Code.Clgi),
				new OpCodeHandler_Simple(Code.Skinit),
				new OpCodeHandler_Simple5(Code.Invlpgaw, Code.Invlpgad, Code.Invlpgaq),

				// E0
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,

				// E8
				null,
				null,
				null,
				null,
				null,
				null,
				new OpCodeHandler_Simple(Code.Rdpkru),
				new OpCodeHandler_Simple(Code.Wrpkru),

				// F0
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,

				// F8
				new OpCodeHandler_Simple(Code.Swapgs),
				new OpCodeHandler_Simple(Code.Rdtscp),
				new OpCodeHandler_Simple5(Code.Monitorxw, Code.Monitorxd, Code.Monitorxq),
				new OpCodeHandler_Simple(Code.Mwaitx),
				new OpCodeHandler_Simple2(Code.Clzerow, Code.Clzerod, Code.Clzeroq),
				null,
				null,
				null,
			};

			var handlers_Grp_0F1F = new OpCodeHandler[8] {
				new OpCodeHandler_Ev(Code.Nop_Ew, Code.Nop_Ed, Code.Nop_Eq),
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
			};

			var handlers_Grp_0FBA = new OpCodeHandler[8] {
				invalid,
				invalid,
				invalid,
				invalid,
				new OpCodeHandler_Ev_Ib2(Code.Bt_Ew_Ib, Code.Bt_Ed_Ib, Code.Bt_Eq_Ib),
				new OpCodeHandler_Ev_Ib2(Code.Bts_Ew_Ib, Code.Bts_Ed_Ib, Code.Bts_Eq_Ib, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Ib2(Code.Btr_Ew_Ib, Code.Btr_Ed_Ib, Code.Btr_Eq_Ib, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Ib2(Code.Btc_Ew_Ib, Code.Btc_Ed_Ib, Code.Btc_Eq_Ib, HandlerFlags.XacquireRelease),
			};

			var handlers_Grp_0FC7 = new OpCodeHandler[8] {
				invalid,
				new OpCodeHandler_M_REXW(Code.Cmpxchg8b_Mq, Code.Cmpxchg16b_Mo, MemorySize.UInt64, MemorySize.UInt128, HandlerFlags.XacquireRelease, HandlerFlags.None),
				invalid,
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_M_REXW(Code.Xrstors_M, Code.Xrstors64_M, MemorySize.Xsave, MemorySize.Xsave64),
					invalid,
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_M_REXW(Code.Xsavec_M, Code.Xsavec64_M, MemorySize.Xsave, MemorySize.Xsave64),
					invalid,
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Ev_REXW(Code.Xsaves_M, Code.Xsaves64_M, MemorySize.Xsave, MemorySize.Xsave64),
					invalid,
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix3(
					new OpCodeHandler_Rv(Code.Rdrand_Rw, Code.Rdrand_Rd, Code.Rdrand_Rq),
					new OpCodeHandler_M(Code.Vmptrld_M, MemorySize.UInt64),
					new OpCodeHandler_Rv(Code.Rdrand_Rw, Code.Rdrand_Rd, Code.Rdrand_Rq),
					new OpCodeHandler_M(Code.Vmclear_M, MemorySize.UInt64),
					invalid,
					new OpCodeHandler_M(Code.Vmxon_M, MemorySize.UInt64),
					invalid,
					invalid,
					LegacyHandlerFlags.HandlerReg | LegacyHandlerFlags.Handler66Reg
				),
				new OpCodeHandler_MandatoryPrefix3(
					new OpCodeHandler_Rv(Code.Rdseed_Rw, Code.Rdseed_Rd, Code.Rdseed_Rq),
					new OpCodeHandler_M(Code.Vmptrst_M, MemorySize.UInt64),
					new OpCodeHandler_Rv(Code.Rdseed_Rw, Code.Rdseed_Rd, Code.Rdseed_Rq),
					invalid,
					new OpCodeHandler_Rv_32_64(Code.Rdpid_Rd, Code.Rdpid_Rq),
					invalid,
					invalid,
					invalid,
					LegacyHandlerFlags.HandlerReg | LegacyHandlerFlags.Handler66Reg
				),
			};

			var handlers_Grp_C6_lo = new OpCodeHandler[8] {
				new OpCodeHandler_Eb_Ib(Code.Mov_Eb_Ib, HandlerFlags.Xrelease | HandlerFlags.XacquireReleaseNoLock),
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
			};

			var handlers_Grp_C6_hi = new OpCodeHandler[0x40] {
				// C0
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,

				// C8
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,

				// D0
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,

				// D8
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,

				// E0
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,

				// E8
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,

				// F0
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,

				// F8
				new OpCodeHandler_Ib3(Code.Xabort_Ib),
				null,
				null,
				null,
				null,
				null,
				null,
				null,
			};

			var handlers_Grp_C7_lo = new OpCodeHandler[8] {
				new OpCodeHandler_Ev_Iz(Code.Mov_Ew_Iw, Code.Mov_Ed_Id, Code.Mov_Eq_Id64, HandlerFlags.Xrelease | HandlerFlags.XacquireReleaseNoLock),
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
			};

			var handlers_Grp_C7_hi = new OpCodeHandler[0x40] {
				// C0
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,

				// C8
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,

				// D0
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,

				// D8
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,

				// E0
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,

				// E8
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,

				// F0
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,

				// F8
				new OpCodeHandler_Jx(Code.Xbegin_Jw16, Code.Xbegin_Jd32, Code.Xbegin_Jd64),
				null,
				null,
				null,
				null,
				null,
				null,
				null,
			};

			var handlers_Grp_0F71 = new OpCodeHandler[8] {
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_NIb(Code.Psrlw_N_Ib),
					new OpCodeHandler_RIb(Register.XMM0, Code.Psrlw_RX_Ib),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_NIb(Code.Psraw_N_Ib),
					new OpCodeHandler_RIb(Register.XMM0, Code.Psraw_RX_Ib),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_NIb(Code.Psllw_N_Ib),
					new OpCodeHandler_RIb(Register.XMM0, Code.Psllw_RX_Ib),
					invalid,
					invalid
				),
				invalid,
			};

			var handlers_Grp_0F72 = new OpCodeHandler[8] {
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_NIb(Code.Psrld_N_Ib),
					new OpCodeHandler_RIb(Register.XMM0, Code.Psrld_RX_Ib),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_NIb(Code.Psrad_N_Ib),
					new OpCodeHandler_RIb(Register.XMM0, Code.Psrad_RX_Ib),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_NIb(Code.Pslld_N_Ib),
					new OpCodeHandler_RIb(Register.XMM0, Code.Pslld_RX_Ib),
					invalid,
					invalid
				),
				invalid,
			};

			var handlers_Grp_0F73 = new OpCodeHandler[8] {
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_NIb(Code.Psrlq_N_Ib),
					new OpCodeHandler_RIb(Register.XMM0, Code.Psrlq_RX_Ib),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_RIb(Register.XMM0, Code.Psrldq_RX_Ib),
					invalid,
					invalid
				),
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_NIb(Code.Psllq_N_Ib),
					new OpCodeHandler_RIb(Register.XMM0, Code.Psllq_RX_Ib),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_RIb(Register.XMM0, Code.Pslldq_RX_Ib),
					invalid,
					invalid
				),
			};

			var handlers_Grp_0FAE_lo = new OpCodeHandler[8] {
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_M(Code.Fxsave_M, Code.Fxsave64_M, MemorySize.Fxsave_512Byte, MemorySize.Fxsave64_512Byte),
					invalid,
					new OpCodeHandler_Ev_REXW(Code.Rdfsbase_Rd, Code.Rdfsbase_Rq),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_M(Code.Fxrstor_M, Code.Fxrstor64_M, MemorySize.Fxsave_512Byte, MemorySize.Fxsave64_512Byte),
					invalid,
					new OpCodeHandler_Ev_REXW(Code.Rdgsbase_Rd, Code.Rdgsbase_Rq),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_M(Code.Ldmxcsr_Md, MemorySize.UInt32),
					invalid,
					new OpCodeHandler_Ev_REXW(Code.Wrfsbase_Rd, Code.Wrfsbase_Rq),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_M(Code.Stmxcsr_Md, MemorySize.UInt32),
					invalid,
					new OpCodeHandler_Ev_REXW(Code.Wrgsbase_Rd, Code.Wrgsbase_Rq),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_M(Code.Xsave_M, Code.Xsave64_M, MemorySize.Xsave, MemorySize.Xsave64),
					invalid,
					new OpCodeHandler_Ev_REXW(Code.Ptwrite_Ed, Code.Ptwrite_Eq, MemorySize.UInt32, MemorySize.UInt64),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_M(Code.Xrstor_M, Code.Xrstor64_M, MemorySize.Xsave, MemorySize.Xsave64),
					invalid,
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_M(Code.Xsaveopt_M, Code.Xsaveopt64_M, MemorySize.Xsave, MemorySize.Xsave64),
					new OpCodeHandler_M(Code.Clwb_Mb, MemorySize.UInt8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_M(Code.Clflush_Mb, MemorySize.UInt8),
					new OpCodeHandler_M(Code.Clflushopt_Mb, MemorySize.UInt8),
					invalid,
					invalid
				),
			};

			var handlers_Grp_0FAE_hi = new OpCodeHandler[0x40] {
				// C0
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,

				// C8
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,

				// D0
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,

				// D8
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,

				// E0
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,

				// E8
				new OpCodeHandler_Simple(Code.Lfence),
				new OpCodeHandler_Simple(Code.Lfence),
				new OpCodeHandler_Simple(Code.Lfence),
				new OpCodeHandler_Simple(Code.Lfence),
				new OpCodeHandler_Simple(Code.Lfence),
				new OpCodeHandler_Simple(Code.Lfence),
				new OpCodeHandler_Simple(Code.Lfence),
				new OpCodeHandler_Simple(Code.Lfence),

				// F0
				new OpCodeHandler_Simple(Code.Mfence),
				new OpCodeHandler_Simple(Code.Mfence),
				new OpCodeHandler_Simple(Code.Mfence),
				new OpCodeHandler_Simple(Code.Mfence),
				new OpCodeHandler_Simple(Code.Mfence),
				new OpCodeHandler_Simple(Code.Mfence),
				new OpCodeHandler_Simple(Code.Mfence),
				new OpCodeHandler_Simple(Code.Mfence),

				// F8
				new OpCodeHandler_Simple(Code.Sfence),
				new OpCodeHandler_Simple(Code.Sfence),
				new OpCodeHandler_Simple(Code.Sfence),
				new OpCodeHandler_Simple(Code.Sfence),
				new OpCodeHandler_Simple(Code.Sfence),
				new OpCodeHandler_Simple(Code.Sfence),
				new OpCodeHandler_Simple(Code.Sfence),
				new OpCodeHandler_Simple(Code.Sfence),
			};

			var handlers_Grp_0F18 = new OpCodeHandler[8] {
				new OpCodeHandler_M(Code.Prefetchnta_Mb, MemorySize.UInt8),
				new OpCodeHandler_M(Code.Prefetcht0_Mb, MemorySize.UInt8),
				new OpCodeHandler_M(Code.Prefetcht1_Mb, MemorySize.UInt8),
				new OpCodeHandler_M(Code.Prefetcht2_Mb, MemorySize.UInt8),
				invalid,
				invalid,
				invalid,
				invalid,
			};

			var handlers_Grp_0F0D = new OpCodeHandler[8] {
				new OpCodeHandler_M(Code.Prefetch_Mb, MemorySize.UInt8),
				new OpCodeHandler_M(Code.Prefetchw_Mb, MemorySize.UInt8),
				new OpCodeHandler_M(Code.Prefetchwt1_Mb, MemorySize.UInt8),
				new OpCodeHandler_M(Code.Prefetch_Mb_r3, MemorySize.UInt8),
				new OpCodeHandler_M(Code.Prefetch_Mb_r4, MemorySize.UInt8),
				new OpCodeHandler_M(Code.Prefetch_Mb_r5, MemorySize.UInt8),
				new OpCodeHandler_M(Code.Prefetch_Mb_r6, MemorySize.UInt8),
				new OpCodeHandler_M(Code.Prefetch_Mb_r7, MemorySize.UInt8),
			};

			var handlers_Grp_660F78 = new OpCodeHandler[8] {
				new OpCodeHandler_RIbIb(Register.XMM0, Code.Extrq_RX_Ib_Ib),
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
			};

			var ThreeByteHandlers_0F38XX = new OpCodeHandler[0x100] {
				// 00
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pshufb_P_Q, MemorySize.Packed64_UInt8),
					new OpCodeHandler_VW(Register.XMM0, Code.Pshufb_VX_WX, MemorySize.Packed128_UInt8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Phaddw_P_Q, MemorySize.Packed64_Int16),
					new OpCodeHandler_VW(Register.XMM0, Code.Phaddw_VX_WX, MemorySize.Packed128_Int16),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Phaddd_P_Q, MemorySize.Packed64_Int32),
					new OpCodeHandler_VW(Register.XMM0, Code.Phaddd_VX_WX, MemorySize.Packed128_Int32),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Phaddsw_P_Q, MemorySize.Packed64_Int16),
					new OpCodeHandler_VW(Register.XMM0, Code.Phaddsw_VX_WX, MemorySize.Packed128_Int16),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pmaddubsw_P_Q, MemorySize.Packed64_Int8),
					new OpCodeHandler_VW(Register.XMM0, Code.Pmaddubsw_VX_WX, MemorySize.Packed128_Int8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Phsubw_P_Q, MemorySize.Packed64_Int16),
					new OpCodeHandler_VW(Register.XMM0, Code.Phsubw_VX_WX, MemorySize.Packed128_Int16),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Phsubd_P_Q, MemorySize.Packed64_Int32),
					new OpCodeHandler_VW(Register.XMM0, Code.Phsubd_VX_WX, MemorySize.Packed128_Int32),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Phsubsw_P_Q, MemorySize.Packed64_Int16),
					new OpCodeHandler_VW(Register.XMM0, Code.Phsubsw_VX_WX, MemorySize.Packed128_Int16),
					invalid,
					invalid
				),

				// 08
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Psignb_P_Q, MemorySize.Packed64_Int8),
					new OpCodeHandler_VW(Register.XMM0, Code.Psignb_VX_WX, MemorySize.Packed128_Int8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Psignw_P_Q, MemorySize.Packed64_Int16),
					new OpCodeHandler_VW(Register.XMM0, Code.Psignw_VX_WX, MemorySize.Packed128_Int16),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Psignd_P_Q, MemorySize.Packed64_Int32),
					new OpCodeHandler_VW(Register.XMM0, Code.Psignd_VX_WX, MemorySize.Packed128_Int32),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pmulhrsw_P_Q, MemorySize.Packed64_Int16),
					new OpCodeHandler_VW(Register.XMM0, Code.Pmulhrsw_VX_WX, MemorySize.Packed128_Int16),
					invalid,
					invalid
				),
				invalid,
				invalid,
				invalid,
				invalid,

				// 10
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pblendvb_VX_WX, MemorySize.Packed128_Int8),
					invalid,
					invalid
				),
				invalid,
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Blendvps_VX_WX, MemorySize.Packed128_Float32),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Blendvpd_VX_WX, MemorySize.Packed128_Float64),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Ptest_VX_WX, MemorySize.UInt128),
					invalid,
					invalid
				),

				// 18
				invalid,
				invalid,
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pabsb_P_Q, MemorySize.Packed64_Int8),
					new OpCodeHandler_VW(Register.XMM0, Code.Pabsb_VX_WX, MemorySize.Packed128_Int8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pabsw_P_Q, MemorySize.Packed64_Int16),
					new OpCodeHandler_VW(Register.XMM0, Code.Pabsw_VX_WX, MemorySize.Packed128_Int16),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pabsd_P_Q, MemorySize.Packed64_Int32),
					new OpCodeHandler_VW(Register.XMM0, Code.Pabsd_VX_WX, MemorySize.Packed128_Int32),
					invalid,
					invalid
				),
				invalid,

				// 20
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pmovsxbw_VX_WX, MemorySize.Packed64_Int8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pmovsxbd_VX_WX, MemorySize.Packed32_Int8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pmovsxbq_VX_WX, MemorySize.Packed16_Int8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pmovsxwd_VX_WX, MemorySize.Packed64_Int16),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pmovsxwq_VX_WX, MemorySize.Packed32_Int16),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pmovsxdq_VX_WX, MemorySize.Packed64_Int32),
					invalid,
					invalid
				),
				invalid,
				invalid,

				// 28
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pmuldq_VX_WX, MemorySize.Packed128_Int32),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pcmpeqq_VX_WX, MemorySize.Packed128_Int64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VM(Register.XMM0, Code.Movntdqa_VX_M, MemorySize.UInt128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Packusdw_VX_WX, MemorySize.Packed128_Int32),
					invalid,
					invalid
				),
				invalid,
				invalid,
				invalid,
				invalid,

				// 30
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pmovzxbw_VX_WX, MemorySize.Packed64_UInt8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pmovzxbd_VX_WX, MemorySize.Packed32_UInt8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pmovzxbq_VX_WX, MemorySize.Packed16_UInt8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pmovzxwd_VX_WX, MemorySize.Packed64_UInt16),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pmovzxwq_VX_WX, MemorySize.Packed32_UInt16),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pmovzxdq_VX_WX, MemorySize.Packed64_UInt32),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pcmpgtq_VX_WX, MemorySize.Packed128_Int64),
					invalid,
					invalid
				),

				// 38
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pminsb_VX_WX, MemorySize.Packed128_Int8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pminsd_VX_WX, MemorySize.Packed128_Int32),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pminuw_VX_WX, MemorySize.Packed128_UInt16),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pminud_VX_WX, MemorySize.Packed128_UInt32),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pmaxsb_VX_WX, MemorySize.Packed128_Int8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pmaxsd_VX_WX, MemorySize.Packed128_Int32),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pmaxuw_VX_WX, MemorySize.Packed128_UInt16),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pmaxud_VX_WX, MemorySize.Packed128_UInt32),
					invalid,
					invalid
				),

				// 40
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pmulld_VX_WX, MemorySize.Packed128_Int32),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Phminposuw_VX_WX, MemorySize.Packed128_UInt16),
					invalid,
					invalid
				),
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// 48
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// 50
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// 58
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// 60
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// 68
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// 70
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// 78
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// 80
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_Gv_Ev_32_64(Code.Invept_Gd_M, Code.Invept_Gq_M, MemorySize.UInt128, MemorySize.UInt128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_Gv_Ev_32_64(Code.Invvpid_Gd_M, Code.Invvpid_Gq_M, MemorySize.UInt128, MemorySize.UInt128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_Gv_Ev_32_64(Code.Invpcid_Gd_M, Code.Invpcid_Gq_M, MemorySize.UInt128, MemorySize.UInt128),
					invalid,
					invalid
				),
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// 88
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// 90
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// 98
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// A0
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// A8
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// B0
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// B8
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// C0
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// C8
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Sha1nexte_VX_WX, MemorySize.Packed128_UInt32),
					invalid,
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Sha1msg1_VX_WX, MemorySize.Packed128_UInt32),
					invalid,
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Sha1msg2_VX_WX, MemorySize.Packed128_UInt32),
					invalid,
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Sha256rnds2_VX_WX, MemorySize.Packed128_UInt32),
					invalid,
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Sha256msg1_VX_WX, MemorySize.Packed128_UInt32),
					invalid,
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Sha256msg2_VX_WX, MemorySize.Packed128_UInt32),
					invalid,
					invalid,
					invalid
				),
				invalid,
				invalid,

				// D0
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// D8
				invalid,
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Aesimc_VX_WX, MemorySize.UInt128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Aesenc_VX_WX, MemorySize.UInt128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Aesenclast_VX_WX, MemorySize.UInt128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Aesdec_VX_WX, MemorySize.UInt128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Aesdeclast_VX_WX, MemorySize.UInt128),
					invalid,
					invalid
				),

				// E0
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// E8
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// F0
				new OpCodeHandler_MandatoryPrefix_F3_F2(
					new OpCodeHandler_Gv_Mv(Code.Movbe_Gw_Mw, Code.Movbe_Gd_Md, Code.Movbe_Gq_Mq),
					invalid,
					new OpCodeHandler_Gv_Eb_REX(Code.Crc32_Gd_Eb, Code.Crc32_Gq_Eb)
				),
				new OpCodeHandler_MandatoryPrefix_F3_F2(
					new OpCodeHandler_Mv_Gv(Code.Movbe_Mw_Gw, Code.Movbe_Md_Gd, Code.Movbe_Mq_Gq),
					invalid,
					new OpCodeHandler_Gdq_Ev(Code.Crc32_Gd_Ew, Code.Crc32_Gd_Ed, Code.Crc32_Gq_Eq)
				),
				invalid,
				invalid,
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_Gv_Ev_REX(Code.Adcx_Gd_Ed, Code.Adcx_Gq_Eq),
					new OpCodeHandler_Gv_Ev_REX(Code.Adox_Gd_Ed, Code.Adox_Gq_Eq),
					invalid
				),
				invalid,

				// F8
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
			};

			var ThreeByteHandlers_0F3AXX = new OpCodeHandler[0x100] {
				// 00
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// 08
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VWIb(Register.XMM0, Code.Roundps_VX_WX_Ib, MemorySize.Packed128_Float32),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VWIb(Register.XMM0, Code.Roundpd_VX_WX_Ib, MemorySize.Packed128_Float64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VWIb(Register.XMM0, Code.Roundss_VX_WX_Ib, MemorySize.Float32),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VWIb(Register.XMM0, Code.Roundsd_VX_WX_Ib, MemorySize.Float64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VWIb(Register.XMM0, Code.Blendps_VX_WX_Ib, MemorySize.Packed128_Float32),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VWIb(Register.XMM0, Code.Blendpd_VX_WX_Ib, MemorySize.Packed128_Float64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VWIb(Register.XMM0, Code.Pblendw_VX_WX_Ib, MemorySize.Packed128_Int16),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q_Ib(Code.Palignr_P_Q_Ib, MemorySize.Packed64_Int8),
					new OpCodeHandler_VWIb(Register.XMM0, Code.Palignr_VX_WX_Ib, MemorySize.Packed128_Int8),
					invalid,
					invalid
				),

				// 10
				invalid,
				invalid,
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_GvM_VX_Ib(Register.XMM0, Code.Pextrb_RdMb_VX_Ib, Code.Pextrb_RqMb_VX_Ib, MemorySize.UInt8, MemorySize.UInt8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_GvM_VX_Ib(Register.XMM0, Code.Pextrw_RdMw_VX_Ib, Code.Pextrw_RqMw_VX_Ib, MemorySize.UInt16, MemorySize.UInt16),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_GvM_VX_Ib(Register.XMM0, Code.Pextrd_Ed_VX_Ib, Code.Pextrq_Eq_VX_Ib, MemorySize.UInt32, MemorySize.UInt64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_Ed_V_Ib(Register.XMM0, Code.Extractps_Ed_VX_Ib, Code.Extractps_Eq_VX_Ib, MemorySize.Float32, MemorySize.Float32),
					invalid,
					invalid
				),

				// 18
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// 20
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VX_E_Ib(Register.XMM0, Code.Pinsrb_VX_RdMb_Ib, Code.Pinsrb_VX_RqMb_Ib, MemorySize.UInt8, MemorySize.UInt8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VWIb(Register.XMM0, Code.Insertps_VX_WX_Ib, MemorySize.Float32),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VX_E_Ib(Register.XMM0, Code.Pinsrd_VX_Ed_Ib, Code.Pinsrq_VX_Eq_Ib, MemorySize.UInt32, MemorySize.UInt64),
					invalid,
					invalid
				),
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// 28
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// 30
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// 38
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// 40
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VWIb(Register.XMM0, Code.Dpps_VX_WX_Ib, MemorySize.Packed128_Float32),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VWIb(Register.XMM0, Code.Dppd_VX_WX_Ib, MemorySize.Packed128_Float64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VWIb(Register.XMM0, Code.Mpsadbw_VX_WX_Ib, MemorySize.Packed128_UInt8),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VWIb(Register.XMM0, Code.Pclmulqdq_VX_WX_Ib, MemorySize.Packed128_UInt64),
					invalid,
					invalid
				),
				invalid,
				invalid,
				invalid,

				// 48
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// 50
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// 58
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// 60
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VWIb(Register.XMM0, Code.Pcmpestrm_VX_WX_Ib, Code.Pcmpestrm64_VX_WX_Ib, MemorySize.Packed128_UInt8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VWIb(Register.XMM0, Code.Pcmpestri_VX_WX_Ib, Code.Pcmpestri64_VX_WX_Ib, MemorySize.Packed128_UInt8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VWIb(Register.XMM0, Code.Pcmpistrm_VX_WX_Ib, MemorySize.Packed128_UInt8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VWIb(Register.XMM0, Code.Pcmpistri_VX_WX_Ib, MemorySize.Packed128_UInt8),
					invalid,
					invalid
				),
				invalid,
				invalid,
				invalid,
				invalid,

				// 68
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// 70
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// 78
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// 80
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// 88
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// 90
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// 98
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// A0
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// A8
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// B0
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// B8
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// C0
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// C8
				invalid,
				invalid,
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VWIb(Register.XMM0, Code.Sha1rnds4_VX_WX_Ib, MemorySize.Packed128_UInt32),
					invalid,
					invalid,
					invalid
				),
				invalid,
				invalid,
				invalid,

				// D0
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// D8
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VWIb(Register.XMM0, Code.Aeskeygenassist_VX_WX_Ib, MemorySize.UInt128),
					invalid,
					invalid
				),

				// E0
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// E8
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// F0
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// F8
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
			};

			var TwoByteHandlers_0FXX = new OpCodeHandler[0x100] {
				// 00
				new OpCodeHandler_Group(handlers_Grp_0F00),
				new OpCodeHandler_Group8x64(handlers_Grp_0F01_lo, handlers_Grp_0F01_hi),
				new OpCodeHandler_Gv_Ev3(Code.Lar_Gw_Ew, Code.Lar_Gd_Ed, Code.Lar_Gq_Eq),
				new OpCodeHandler_Gv_Ev3(Code.Lsl_Gw_Ew, Code.Lsl_Gd_Ed, Code.Lsl_Gq_Eq),
				invalid,
				new OpCodeHandler_Simple(Code.Syscall),
				new OpCodeHandler_Simple(Code.Clts),
				new OpCodeHandler_Simple2(Code.Sysretd, Code.Sysretd, Code.Sysretq),

				// 08
				new OpCodeHandler_Simple(Code.Invd),
				new OpCodeHandler_Simple(Code.Wbinvd),
				invalid,
				new OpCodeHandler_Simple(Code.Ud2),
				invalid,
				new OpCodeHandler_Group(handlers_Grp_0F0D),
				new OpCodeHandler_Simple(Code.Femms),
				new OpCodeHandler_D3NOW(),

				// 10
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Movups_VX_WX, MemorySize.Packed128_Float32),
					new OpCodeHandler_VW(Register.XMM0, Code.Movupd_VX_WX, MemorySize.Packed128_Float64),
					new OpCodeHandler_VW(Register.XMM0, Code.Movss_VX_WX, MemorySize.Float32),
					new OpCodeHandler_VW(Register.XMM0, Code.Movsd_VX_WX, MemorySize.Float64)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_WV(Register.XMM0, Code.Movups_WX_VX, MemorySize.Packed128_Float32),
					new OpCodeHandler_WV(Register.XMM0, Code.Movupd_WX_VX, MemorySize.Packed128_Float64),
					new OpCodeHandler_WV(Register.XMM0, Code.Movss_WX_VX, MemorySize.Float32),
					new OpCodeHandler_WV(Register.XMM0, Code.Movsd_WX_VX, MemorySize.Float64)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Movhlps_VX_RX, Code.Movlps_VX_M, MemorySize.Packed64_Float32),
					new OpCodeHandler_VM(Register.XMM0, Code.Movlpd_VX_M, MemorySize.Float64),
					new OpCodeHandler_VW(Register.XMM0, Code.Movsldup_VX_WX, MemorySize.Packed128_Float32),
					new OpCodeHandler_VW(Register.XMM0, Code.Movddup_VX_WX, MemorySize.Float64)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_MV(Register.XMM0, Code.Movlps_M_VX, MemorySize.Packed64_Float32),
					new OpCodeHandler_MV(Register.XMM0, Code.Movlpd_M_VX, MemorySize.Float64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Unpcklps_VX_WX, MemorySize.Packed128_Float32),
					new OpCodeHandler_VW(Register.XMM0, Code.Unpcklpd_VX_WX, MemorySize.Packed128_Float64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Unpckhps_VX_WX, MemorySize.Packed128_Float32),
					new OpCodeHandler_VW(Register.XMM0, Code.Unpckhpd_VX_WX, MemorySize.Packed128_Float64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Movlhps_VX_RX, Code.Movhps_VX_M, MemorySize.Packed64_Float32),
					new OpCodeHandler_VM(Register.XMM0, Code.Movhpd_VX_M, MemorySize.Float64),
					new OpCodeHandler_VW(Register.XMM0, Code.Movshdup_VX_WX, MemorySize.Packed128_Float32),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_MV(Register.XMM0, Code.Movhps_M_VX, MemorySize.Packed64_Float32),
					new OpCodeHandler_MV(Register.XMM0, Code.Movhpd_M_VX, MemorySize.Float64),
					invalid,
					invalid
				),

				// 18
				new OpCodeHandler_Group(handlers_Grp_0F18),
				invalid,
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_RM(
						invalid,
						new OpCodeHandler_B_MIB(Code.Bndldx_B_MIB)
					),
					new OpCodeHandler_B_BM(Code.Bndmov_B_BMq, Code.Bndmov_B_BMo, MemorySize.Bnd32, MemorySize.Bnd64),
					new OpCodeHandler_B_Ev(Code.Bndcl_B_Ed, Code.Bndcl_B_Eq),
					new OpCodeHandler_B_Ev(Code.Bndcu_B_Ed, Code.Bndcu_B_Eq)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_RM(
						invalid,
						new OpCodeHandler_MIB_B(Code.Bndstx_MIB_B)
					),
					new OpCodeHandler_BM_B(Code.Bndmov_BMq_B, Code.Bndmov_BMo_B, MemorySize.Bnd32, MemorySize.Bnd64),
					new OpCodeHandler_RM(
						invalid,
						new OpCodeHandler_B_Ev(Code.Bndmk_B_Md, Code.Bndmk_B_Mq)
					),
					new OpCodeHandler_B_Ev(Code.Bndcn_B_Ed, Code.Bndcn_B_Eq)
				),
				invalid,
				invalid,
				invalid,
				new OpCodeHandler_Group(handlers_Grp_0F1F),

				// 20
				new OpCodeHandler_Rq_Cq(Code.Mov_Rq_Cq, Register.CR0),
				new OpCodeHandler_Rq_Cq(Code.Mov_Rq_Dq, Register.DR0),
				new OpCodeHandler_Cq_Rq(Code.Mov_Cq_Rq, Register.CR0),
				new OpCodeHandler_Cq_Rq(Code.Mov_Dq_Rq, Register.DR0),
				invalid,
				invalid,
				invalid,
				invalid,

				// 28
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Movaps_VX_WX, MemorySize.Packed128_Float32),
					new OpCodeHandler_VW(Register.XMM0, Code.Movapd_VX_WX, MemorySize.Packed128_Float64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_WV(Register.XMM0, Code.Movaps_WX_VX, MemorySize.Packed128_Float32),
					new OpCodeHandler_WV(Register.XMM0, Code.Movapd_WX_VX, MemorySize.Packed128_Float64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VQ(Register.XMM0, Code.Cvtpi2ps_VX_Q, MemorySize.Packed64_Int32),
					new OpCodeHandler_VQ(Register.XMM0, Code.Cvtpi2pd_VX_Q, MemorySize.Packed64_Int32),
					new OpCodeHandler_V_Ev(Register.XMM0, Code.Cvtsi2ss_VX_Ed, Code.Cvtsi2ss_VX_Eq),
					new OpCodeHandler_V_Ev(Register.XMM0, Code.Cvtsi2sd_VX_Ed, Code.Cvtsi2sd_VX_Eq)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_MV(Register.XMM0, Code.Movntps_M_VX, MemorySize.Packed128_Float32),
					new OpCodeHandler_MV(Register.XMM0, Code.Movntpd_M_VX, MemorySize.Packed128_Float64),
					new OpCodeHandler_MV(Register.XMM0, Code.Movntss_M_VX, MemorySize.Float32),
					new OpCodeHandler_MV(Register.XMM0, Code.Movntsd_M_VX, MemorySize.Float64)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_W(Register.XMM0, Code.Cvttps2pi_P_WX, MemorySize.Packed64_Float32),
					new OpCodeHandler_P_W(Register.XMM0, Code.Cvttpd2pi_P_WX, MemorySize.Packed128_Float64),
					new OpCodeHandler_Gv_W(Register.XMM0, Code.Cvttss2si_Gd_WX, Code.Cvttss2si_Gq_WX, MemorySize.Float32),
					new OpCodeHandler_Gv_W(Register.XMM0, Code.Cvttsd2si_Gd_WX, Code.Cvttsd2si_Gq_WX, MemorySize.Float64)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_W(Register.XMM0, Code.Cvtps2pi_P_WX, MemorySize.Packed64_Float32),
					new OpCodeHandler_P_W(Register.XMM0, Code.Cvtpd2pi_P_WX, MemorySize.Packed128_Float64),
					new OpCodeHandler_Gv_W(Register.XMM0, Code.Cvtss2si_Gd_WX, Code.Cvtss2si_Gq_WX, MemorySize.Float32),
					new OpCodeHandler_Gv_W(Register.XMM0, Code.Cvtsd2si_Gd_WX, Code.Cvtsd2si_Gq_WX, MemorySize.Float64)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Ucomiss_VX_WX, MemorySize.Float32),
					new OpCodeHandler_VW(Register.XMM0, Code.Ucomisd_VX_WX, MemorySize.Float64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Comiss_VX_WX, MemorySize.Float32),
					new OpCodeHandler_VW(Register.XMM0, Code.Comisd_VX_WX, MemorySize.Float64),
					invalid,
					invalid
				),

				// 30
				new OpCodeHandler_Simple(Code.Wrmsr),
				new OpCodeHandler_Simple(Code.Rdtsc),
				new OpCodeHandler_Simple(Code.Rdmsr),
				new OpCodeHandler_Simple(Code.Rdpmc),
				new OpCodeHandler_Simple(Code.Sysenter),
				new OpCodeHandler_Simple4(Code.Sysexitd, Code.Sysexitq),
				invalid,
				new OpCodeHandler_Simple(Code.Getsec),

				// 38
				new OpCodeHandler_AnotherTable(ThreeByteHandlers_0F38XX),
				invalid,
				new OpCodeHandler_AnotherTable(ThreeByteHandlers_0F3AXX),
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// 40
				new OpCodeHandler_Gv_Ev(Code.Cmovo_Gw_Ew, Code.Cmovo_Gd_Ed, Code.Cmovo_Gq_Eq),
				new OpCodeHandler_Gv_Ev(Code.Cmovno_Gw_Ew, Code.Cmovno_Gd_Ed, Code.Cmovno_Gq_Eq),
				new OpCodeHandler_Gv_Ev(Code.Cmovb_Gw_Ew, Code.Cmovb_Gd_Ed, Code.Cmovb_Gq_Eq),
				new OpCodeHandler_Gv_Ev(Code.Cmovae_Gw_Ew, Code.Cmovae_Gd_Ed, Code.Cmovae_Gq_Eq),
				new OpCodeHandler_Gv_Ev(Code.Cmove_Gw_Ew, Code.Cmove_Gd_Ed, Code.Cmove_Gq_Eq),
				new OpCodeHandler_Gv_Ev(Code.Cmovne_Gw_Ew, Code.Cmovne_Gd_Ed, Code.Cmovne_Gq_Eq),
				new OpCodeHandler_Gv_Ev(Code.Cmovbe_Gw_Ew, Code.Cmovbe_Gd_Ed, Code.Cmovbe_Gq_Eq),
				new OpCodeHandler_Gv_Ev(Code.Cmova_Gw_Ew, Code.Cmova_Gd_Ed, Code.Cmova_Gq_Eq),

				// 48
				new OpCodeHandler_Gv_Ev(Code.Cmovs_Gw_Ew, Code.Cmovs_Gd_Ed, Code.Cmovs_Gq_Eq),
				new OpCodeHandler_Gv_Ev(Code.Cmovns_Gw_Ew, Code.Cmovns_Gd_Ed, Code.Cmovns_Gq_Eq),
				new OpCodeHandler_Gv_Ev(Code.Cmovp_Gw_Ew, Code.Cmovp_Gd_Ed, Code.Cmovp_Gq_Eq),
				new OpCodeHandler_Gv_Ev(Code.Cmovnp_Gw_Ew, Code.Cmovnp_Gd_Ed, Code.Cmovnp_Gq_Eq),
				new OpCodeHandler_Gv_Ev(Code.Cmovl_Gw_Ew, Code.Cmovl_Gd_Ed, Code.Cmovl_Gq_Eq),
				new OpCodeHandler_Gv_Ev(Code.Cmovge_Gw_Ew, Code.Cmovge_Gd_Ed, Code.Cmovge_Gq_Eq),
				new OpCodeHandler_Gv_Ev(Code.Cmovle_Gw_Ew, Code.Cmovle_Gd_Ed, Code.Cmovle_Gq_Eq),
				new OpCodeHandler_Gv_Ev(Code.Cmovg_Gw_Ew, Code.Cmovg_Gd_Ed, Code.Cmovg_Gq_Eq),

				// 50
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Gv_RX(Register.XMM0, Code.Movmskps_Gd_RX, Code.Movmskps_Gq_RX),
					new OpCodeHandler_Gv_RX(Register.XMM0, Code.Movmskpd_Gd_RX, Code.Movmskpd_Gq_RX),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Sqrtps_VX_WX, MemorySize.Packed128_Float32),
					new OpCodeHandler_VW(Register.XMM0, Code.Sqrtpd_VX_WX, MemorySize.Packed128_Float64),
					new OpCodeHandler_VW(Register.XMM0, Code.Sqrtss_VX_WX, MemorySize.Float32),
					new OpCodeHandler_VW(Register.XMM0, Code.Sqrtsd_VX_WX, MemorySize.Float64)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Rsqrtps_VX_WX, MemorySize.Packed128_Float32),
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Rsqrtss_VX_WX, MemorySize.Float32),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Rcpps_VX_WX, MemorySize.Packed128_Float32),
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Rcpss_VX_WX, MemorySize.Float32),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Andps_VX_WX, MemorySize.Packed128_Float32),
					new OpCodeHandler_VW(Register.XMM0, Code.Andpd_VX_WX, MemorySize.Packed128_Float64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Andnps_VX_WX, MemorySize.Packed128_Float32),
					new OpCodeHandler_VW(Register.XMM0, Code.Andnpd_VX_WX, MemorySize.Packed128_Float64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Orps_VX_WX, MemorySize.Packed128_Float32),
					new OpCodeHandler_VW(Register.XMM0, Code.Orpd_VX_WX, MemorySize.Packed128_Float64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Xorps_VX_WX, MemorySize.Packed128_Float32),
					new OpCodeHandler_VW(Register.XMM0, Code.Xorpd_VX_WX, MemorySize.Packed128_Float64),
					invalid,
					invalid
				),

				// 58
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Addps_VX_WX, MemorySize.Packed128_Float32),
					new OpCodeHandler_VW(Register.XMM0, Code.Addpd_VX_WX, MemorySize.Packed128_Float64),
					new OpCodeHandler_VW(Register.XMM0, Code.Addss_VX_WX, MemorySize.Float32),
					new OpCodeHandler_VW(Register.XMM0, Code.Addsd_VX_WX, MemorySize.Float64)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Mulps_VX_WX, MemorySize.Packed128_Float32),
					new OpCodeHandler_VW(Register.XMM0, Code.Mulpd_VX_WX, MemorySize.Packed128_Float64),
					new OpCodeHandler_VW(Register.XMM0, Code.Mulss_VX_WX, MemorySize.Float32),
					new OpCodeHandler_VW(Register.XMM0, Code.Mulsd_VX_WX, MemorySize.Float64)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Cvtps2pd_VX_WX, MemorySize.Packed64_Float32),
					new OpCodeHandler_VW(Register.XMM0, Code.Cvtpd2ps_VX_WX, MemorySize.Packed128_Float64),
					new OpCodeHandler_VW(Register.XMM0, Code.Cvtss2sd_VX_WX, MemorySize.Float32),
					new OpCodeHandler_VW(Register.XMM0, Code.Cvtsd2ss_VX_WX, MemorySize.Float64)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Cvtdq2ps_VX_WX, MemorySize.Packed128_Int32),
					new OpCodeHandler_VW(Register.XMM0, Code.Cvtps2dq_VX_WX, MemorySize.Packed128_Float32),
					new OpCodeHandler_VW(Register.XMM0, Code.Cvttps2dq_VX_WX, MemorySize.Packed128_Float32),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Subps_VX_WX, MemorySize.Packed128_Float32),
					new OpCodeHandler_VW(Register.XMM0, Code.Subpd_VX_WX, MemorySize.Packed128_Float64),
					new OpCodeHandler_VW(Register.XMM0, Code.Subss_VX_WX, MemorySize.Float32),
					new OpCodeHandler_VW(Register.XMM0, Code.Subsd_VX_WX, MemorySize.Float64)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Minps_VX_WX, MemorySize.Packed128_Float32),
					new OpCodeHandler_VW(Register.XMM0, Code.Minpd_VX_WX, MemorySize.Packed128_Float64),
					new OpCodeHandler_VW(Register.XMM0, Code.Minss_VX_WX, MemorySize.Float32),
					new OpCodeHandler_VW(Register.XMM0, Code.Minsd_VX_WX, MemorySize.Float64)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Divps_VX_WX, MemorySize.Packed128_Float32),
					new OpCodeHandler_VW(Register.XMM0, Code.Divpd_VX_WX, MemorySize.Packed128_Float64),
					new OpCodeHandler_VW(Register.XMM0, Code.Divss_VX_WX, MemorySize.Float32),
					new OpCodeHandler_VW(Register.XMM0, Code.Divsd_VX_WX, MemorySize.Float64)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Maxps_VX_WX, MemorySize.Packed128_Float32),
					new OpCodeHandler_VW(Register.XMM0, Code.Maxpd_VX_WX, MemorySize.Packed128_Float64),
					new OpCodeHandler_VW(Register.XMM0, Code.Maxss_VX_WX, MemorySize.Float32),
					new OpCodeHandler_VW(Register.XMM0, Code.Maxsd_VX_WX, MemorySize.Float64)
				),

				// 60
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Punpcklbw_P_Q, MemorySize.Packed32_Int8),
					new OpCodeHandler_VW(Register.XMM0, Code.Punpcklbw_VX_WX, MemorySize.Packed128_Int8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Punpcklwd_P_Q, MemorySize.Packed32_Int16),
					new OpCodeHandler_VW(Register.XMM0, Code.Punpcklwd_VX_WX, MemorySize.Packed128_Int16),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Punpckldq_P_Q, MemorySize.Int32),
					new OpCodeHandler_VW(Register.XMM0, Code.Punpckldq_VX_WX, MemorySize.Packed128_Int32),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Packsswb_P_Q, MemorySize.Packed64_Int16),
					new OpCodeHandler_VW(Register.XMM0, Code.Packsswb_VX_WX, MemorySize.Packed128_Int16),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pcmpgtb_P_Q, MemorySize.Packed64_Int8),
					new OpCodeHandler_VW(Register.XMM0, Code.Pcmpgtb_VX_WX, MemorySize.Packed128_Int8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pcmpgtw_P_Q, MemorySize.Packed64_Int16),
					new OpCodeHandler_VW(Register.XMM0, Code.Pcmpgtw_VX_WX, MemorySize.Packed128_Int16),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pcmpgtd_P_Q, MemorySize.Packed64_Int32),
					new OpCodeHandler_VW(Register.XMM0, Code.Pcmpgtd_VX_WX, MemorySize.Packed128_Int32),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Packuswb_P_Q, MemorySize.Packed64_Int16),
					new OpCodeHandler_VW(Register.XMM0, Code.Packuswb_VX_WX, MemorySize.Packed128_Int16),
					invalid,
					invalid
				),

				// 68
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Punpckhbw_P_Q, MemorySize.Packed64_Int8),
					new OpCodeHandler_VW(Register.XMM0, Code.Punpckhbw_VX_WX, MemorySize.Packed128_Int8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Punpckhwd_P_Q, MemorySize.Packed64_Int16),
					new OpCodeHandler_VW(Register.XMM0, Code.Punpckhwd_VX_WX, MemorySize.Packed128_Int16),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Punpckhdq_P_Q, MemorySize.Packed64_Int32),
					new OpCodeHandler_VW(Register.XMM0, Code.Punpckhdq_VX_WX, MemorySize.Packed128_Int32),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Packssdw_P_Q, MemorySize.Packed64_Int32),
					new OpCodeHandler_VW(Register.XMM0, Code.Packssdw_VX_WX, MemorySize.Packed128_Int32),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Punpcklqdq_VX_WX, MemorySize.Packed128_Int64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Punpckhqdq_VX_WX, MemorySize.Packed128_Int64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Ev(Code.Movd_P_Ed, Code.Movq_P_Eq),
					new OpCodeHandler_VX_Ev(Code.Movd_VX_Ed, Code.Movq_VX_Eq),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Movq_P_Q, MemorySize.Packed64_Int32),
					new OpCodeHandler_VW(Register.XMM0, Code.Movdqa_VX_WX, MemorySize.Packed128_Int32),
					new OpCodeHandler_VW(Register.XMM0, Code.Movdqu_VX_WX, MemorySize.Packed128_Int32),
					invalid
				),

				// 70
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q_Ib(Code.Pshufw_P_Q_Ib, MemorySize.Packed64_Int16),
					new OpCodeHandler_VWIb(Register.XMM0, Code.Pshufd_VX_WX_Ib, MemorySize.Packed128_Int32),
					new OpCodeHandler_VWIb(Register.XMM0, Code.Pshufhw_VX_WX_Ib, MemorySize.Packed128_Int16),
					new OpCodeHandler_VWIb(Register.XMM0, Code.Pshuflw_VX_WX_Ib, MemorySize.Packed128_Int16)
				),
				new OpCodeHandler_Group(handlers_Grp_0F71),
				new OpCodeHandler_Group(handlers_Grp_0F72),
				new OpCodeHandler_Group(handlers_Grp_0F73),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pcmpeqb_P_Q, MemorySize.Packed64_Int8),
					new OpCodeHandler_VW(Register.XMM0, Code.Pcmpeqb_VX_WX, MemorySize.Packed128_Int8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pcmpeqw_P_Q, MemorySize.Packed64_Int16),
					new OpCodeHandler_VW(Register.XMM0, Code.Pcmpeqw_VX_WX, MemorySize.Packed128_Int16),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pcmpeqd_P_Q, MemorySize.Packed64_Int32),
					new OpCodeHandler_VW(Register.XMM0, Code.Pcmpeqd_VX_WX, MemorySize.Packed128_Int32),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix_MaybeModRM(
					new OpCodeHandler_Simple(Code.Emms),
					invalid,
					invalid,
					invalid
				),

				// 78
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Ev_Gv_32_64(Code.Vmread_Ed_Gd, Code.Vmread_Eq_Gq),
					new OpCodeHandler_Group(handlers_Grp_660F78),
					invalid,
					new OpCodeHandler_VRIbIb(Register.XMM0, Code.Insertq_VX_RX_Ib_Ib)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Gv_Ev_32_64(Code.Vmwrite_Gd_Ed, Code.Vmwrite_Gq_Eq, MemorySize.UInt32, MemorySize.UInt64),
					new OpCodeHandler_VW(Register.XMM0, Code.Extrq_VX_RX, Code.INVALID, MemorySize.Unknown),
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Insertq_VX_RX, Code.INVALID, MemorySize.Unknown)
				),
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Haddpd_VX_WX, MemorySize.Packed128_Float64),
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Haddps_VX_WX, MemorySize.Packed128_Float32)
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Hsubpd_VX_WX, MemorySize.Packed128_Float64),
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Hsubps_VX_WX, MemorySize.Packed128_Float32)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Ev_P(Code.Movd_Ed_P, Code.Movq_Eq_P),
					new OpCodeHandler_Ev_VX(Code.Movd_Ed_VX, Code.Movq_Eq_VX),
					new OpCodeHandler_VW(Register.XMM0, Code.Movq_VX_WX, MemorySize.UInt64),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Q_P(Code.Movq_Q_P, MemorySize.Packed64_Int32),
					new OpCodeHandler_WV(Register.XMM0, Code.Movdqa_WX_VX, MemorySize.Packed128_Int32),
					new OpCodeHandler_WV(Register.XMM0, Code.Movdqu_WX_VX, MemorySize.Packed128_Int32),
					invalid
				),

				// 80
				new OpCodeHandler_Jz(Code.Jo_Jd64),
				new OpCodeHandler_Jz(Code.Jno_Jd64),
				new OpCodeHandler_Jz(Code.Jb_Jd64),
				new OpCodeHandler_Jz(Code.Jae_Jd64),
				new OpCodeHandler_Jz(Code.Je_Jd64),
				new OpCodeHandler_Jz(Code.Jne_Jd64),
				new OpCodeHandler_Jz(Code.Jbe_Jd64),
				new OpCodeHandler_Jz(Code.Ja_Jd64),

				// 88
				new OpCodeHandler_Jz(Code.Js_Jd64),
				new OpCodeHandler_Jz(Code.Jns_Jd64),
				new OpCodeHandler_Jz(Code.Jp_Jd64),
				new OpCodeHandler_Jz(Code.Jnp_Jd64),
				new OpCodeHandler_Jz(Code.Jl_Jd64),
				new OpCodeHandler_Jz(Code.Jge_Jd64),
				new OpCodeHandler_Jz(Code.Jle_Jd64),
				new OpCodeHandler_Jz(Code.Jg_Jd64),

				// 90
				new OpCodeHandler_Eb(Code.Seto_Eb),
				new OpCodeHandler_Eb(Code.Setno_Eb),
				new OpCodeHandler_Eb(Code.Setb_Eb),
				new OpCodeHandler_Eb(Code.Setae_Eb),
				new OpCodeHandler_Eb(Code.Sete_Eb),
				new OpCodeHandler_Eb(Code.Setne_Eb),
				new OpCodeHandler_Eb(Code.Setbe_Eb),
				new OpCodeHandler_Eb(Code.Seta_Eb),

				// 98
				new OpCodeHandler_Eb(Code.Sets_Eb),
				new OpCodeHandler_Eb(Code.Setns_Eb),
				new OpCodeHandler_Eb(Code.Setp_Eb),
				new OpCodeHandler_Eb(Code.Setnp_Eb),
				new OpCodeHandler_Eb(Code.Setl_Eb),
				new OpCodeHandler_Eb(Code.Setge_Eb),
				new OpCodeHandler_Eb(Code.Setle_Eb),
				new OpCodeHandler_Eb(Code.Setg_Eb),

				// A0
				new OpCodeHandler_PushOpSizeReg(Code.Pushw_FS, Code.Pushq_FS, Register.FS),
				new OpCodeHandler_PushOpSizeReg(Code.Popw_FS, Code.Popq_FS, Register.FS),
				new OpCodeHandler_Simple(Code.Cpuid),
				new OpCodeHandler_Ev_Gv(Code.Bt_Ew_Gw, Code.Bt_Ed_Gd, Code.Bt_Eq_Gq),
				new OpCodeHandler_Ev_Gv_Ib(Code.Shld_Ew_Gw_Ib, Code.Shld_Ed_Gd_Ib, Code.Shld_Eq_Gq_Ib),
				new OpCodeHandler_Ev_Gv_CL(Code.Shld_Ew_Gw_CL, Code.Shld_Ed_Gd_CL, Code.Shld_Eq_Gq_CL),
				invalid,
				invalid,

				// A8
				new OpCodeHandler_PushOpSizeReg(Code.Pushw_GS, Code.Pushq_GS, Register.GS),
				new OpCodeHandler_PushOpSizeReg(Code.Popw_GS, Code.Popq_GS, Register.GS),
				new OpCodeHandler_Simple(Code.Rsm),
				new OpCodeHandler_Ev_Gv(Code.Bts_Ew_Gw, Code.Bts_Ed_Gd, Code.Bts_Eq_Gq, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Gv_Ib(Code.Shrd_Ew_Gw_Ib, Code.Shrd_Ed_Gd_Ib, Code.Shrd_Eq_Gq_Ib),
				new OpCodeHandler_Ev_Gv_CL(Code.Shrd_Ew_Gw_CL, Code.Shrd_Ed_Gd_CL, Code.Shrd_Eq_Gq_CL),
				new OpCodeHandler_Group8x64(handlers_Grp_0FAE_lo, handlers_Grp_0FAE_hi),
				new OpCodeHandler_Gv_Ev(Code.Imul_Gw_Ew, Code.Imul_Gd_Ed, Code.Imul_Gq_Eq, true),

				// B0
				new OpCodeHandler_Eb_Gb(Code.Cmpxchg_Eb_Gb, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Gv(Code.Cmpxchg_Ew_Gw, Code.Cmpxchg_Ed_Gd, Code.Cmpxchg_Eq_Gq, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Gv_Mp(Code.Lss_Gw_Mp, Code.Lss_Gd_Mp, Code.Lss_Gq_Mp),
				new OpCodeHandler_Ev_Gv(Code.Btr_Ew_Gw, Code.Btr_Ed_Gd, Code.Btr_Eq_Gq, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Gv_Mp(Code.Lfs_Gw_Mp, Code.Lfs_Gd_Mp, Code.Lfs_Gq_Mp),
				new OpCodeHandler_Gv_Mp(Code.Lgs_Gw_Mp, Code.Lgs_Gd_Mp, Code.Lgs_Gq_Mp),
				new OpCodeHandler_Gv_Eb(Code.Movzx_Gw_Eb, Code.Movzx_Gd_Eb, Code.Movzx_Gq_Eb),
				new OpCodeHandler_Gv_Ew(Code.Movzx_Gw_Ew, Code.Movzx_Gd_Ew, Code.Movzx_Gq_Ew),

				// B8
				new OpCodeHandler_MandatoryPrefix_F3_F2(
					invalid,
					new OpCodeHandler_Gv_Ev(Code.Popcnt_Gw_Ew, Code.Popcnt_Gd_Ed, Code.Popcnt_Gq_Eq),
					invalid
				),
				new OpCodeHandler_Gv_Ev(Code.Ud1_Gw_Ew, Code.Ud1_Gd_Ed, Code.Ud1_Gq_Eq),
				new OpCodeHandler_Group(handlers_Grp_0FBA),
				new OpCodeHandler_Ev_Gv(Code.Btc_Ew_Gw, Code.Btc_Ed_Gd, Code.Btc_Eq_Gq, HandlerFlags.XacquireRelease),
				new OpCodeHandler_MandatoryPrefix_F3_F2(
					new OpCodeHandler_Gv_Ev(Code.Bsf_Gw_Ew, Code.Bsf_Gd_Ed, Code.Bsf_Gq_Eq),
					new OpCodeHandler_Gv_Ev(Code.Tzcnt_Gw_Ew, Code.Tzcnt_Gd_Ed, Code.Tzcnt_Gq_Eq),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix_F3_F2(
					new OpCodeHandler_Gv_Ev(Code.Bsr_Gw_Ew, Code.Bsr_Gd_Ed, Code.Bsr_Gq_Eq),
					new OpCodeHandler_Gv_Ev(Code.Lzcnt_Gw_Ew, Code.Lzcnt_Gd_Ed, Code.Lzcnt_Gq_Eq),
					invalid
				),
				new OpCodeHandler_Gv_Eb(Code.Movsx_Gw_Eb, Code.Movsx_Gd_Eb, Code.Movsx_Gq_Eb, MemorySize.Int8),
				new OpCodeHandler_Gv_Ew(Code.Movsx_Gw_Ew, Code.Movsx_Gd_Ew, Code.Movsx_Gq_Ew, MemorySize.Int16),

				// C0
				new OpCodeHandler_Eb_Gb(Code.Xadd_Eb_Gb, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Gv(Code.Xadd_Ew_Gw, Code.Xadd_Ed_Gd, Code.Xadd_Eq_Gq, HandlerFlags.XacquireRelease),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VWIb(Register.XMM0, Code.Cmpps_VX_WX_Ib, MemorySize.Packed128_Float32),
					new OpCodeHandler_VWIb(Register.XMM0, Code.Cmppd_VX_WX_Ib, MemorySize.Packed128_Float64),
					new OpCodeHandler_VWIb(Register.XMM0, Code.Cmpss_VX_WX_Ib, MemorySize.Float32),
					new OpCodeHandler_VWIb(Register.XMM0, Code.Cmpsd_VX_WX_Ib, MemorySize.Float64)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Mv_Gv_REXW(Code.Movnti_Md_Gd, Code.Movnti_Mq_Gq, MemorySize.UInt32, MemorySize.UInt64),
					invalid,
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Ev_Ib(Code.Pinsrw_P_RdMw_Ib, Code.Pinsrw_P_RqMw_Ib, MemorySize.UInt16, MemorySize.UInt16),
					new OpCodeHandler_VX_E_Ib(Register.XMM0, Code.Pinsrw_VX_RdMw_Ib, Code.Pinsrw_VX_RqMw_Ib, MemorySize.UInt16, MemorySize.UInt16),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Gv_N_Ib_REX(Code.Pextrw_Gd_N_Ib, Code.Pextrw_Gq_N_Ib),
					new OpCodeHandler_Gv_Ev_Ib_REX(Register.XMM0, Code.Pextrw_Gd_RX_Ib, Code.Pextrw_Gq_RX_Ib),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VWIb(Register.XMM0, Code.Shufps_VX_WX_Ib, MemorySize.Packed128_Float32),
					new OpCodeHandler_VWIb(Register.XMM0, Code.Shufpd_VX_WX_Ib, MemorySize.Packed128_Float64),
					invalid,
					invalid
				),
				new OpCodeHandler_Group(handlers_Grp_0FC7),

				// C8
				new OpCodeHandler_SimpleReg(0),
				new OpCodeHandler_SimpleReg(1),
				new OpCodeHandler_SimpleReg(2),
				new OpCodeHandler_SimpleReg(3),
				new OpCodeHandler_SimpleReg(4),
				new OpCodeHandler_SimpleReg(5),
				new OpCodeHandler_SimpleReg(6),
				new OpCodeHandler_SimpleReg(7),

				// D0
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Addsubpd_VX_WX, MemorySize.Packed128_Float64),
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Addsubps_VX_WX, MemorySize.Packed128_Float32)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Psrlw_P_Q, MemorySize.UInt64),
					new OpCodeHandler_VW(Register.XMM0, Code.Psrlw_VX_WX, MemorySize.Packed128_UInt64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Psrld_P_Q, MemorySize.UInt64),
					new OpCodeHandler_VW(Register.XMM0, Code.Psrld_VX_WX, MemorySize.Packed128_UInt64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Psrlq_P_Q, MemorySize.UInt64),
					new OpCodeHandler_VW(Register.XMM0, Code.Psrlq_VX_WX, MemorySize.Packed128_UInt64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Paddq_P_Q, MemorySize.UInt64),
					new OpCodeHandler_VW(Register.XMM0, Code.Paddq_VX_WX, MemorySize.Packed128_UInt64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pmullw_P_Q, MemorySize.Packed64_Int16),
					new OpCodeHandler_VW(Register.XMM0, Code.Pmullw_VX_WX, MemorySize.Packed128_Int16),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_WV(Register.XMM0, Code.Movq_WX_VX, MemorySize.UInt64),
					new OpCodeHandler_VN(Register.XMM0, Code.Movq2dq_VX_N),
					new OpCodeHandler_P_R(Register.XMM0, Code.Movdq2q_P_RX)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Gv_N(Code.Pmovmskb_Gd_N, Code.Pmovmskb_Gq_N),
					new OpCodeHandler_Gv_RX(Register.XMM0, Code.Pmovmskb_Gd_RX, Code.Pmovmskb_Gq_RX),
					invalid,
					invalid
				),

				// D8
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Psubusb_P_Q, MemorySize.Packed64_UInt8),
					new OpCodeHandler_VW(Register.XMM0, Code.Psubusb_VX_WX, MemorySize.Packed128_UInt8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Psubusw_P_Q, MemorySize.Packed64_UInt16),
					new OpCodeHandler_VW(Register.XMM0, Code.Psubusw_VX_WX, MemorySize.Packed128_UInt16),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pminub_P_Q, MemorySize.Packed64_UInt8),
					new OpCodeHandler_VW(Register.XMM0, Code.Pminub_VX_WX, MemorySize.Packed128_UInt8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pand_P_Q, MemorySize.UInt64),
					new OpCodeHandler_VW(Register.XMM0, Code.Pand_VX_WX, MemorySize.Packed128_UInt64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Paddusb_P_Q, MemorySize.Packed64_UInt8),
					new OpCodeHandler_VW(Register.XMM0, Code.Paddusb_VX_WX, MemorySize.Packed128_UInt8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Paddusw_P_Q, MemorySize.Packed64_UInt16),
					new OpCodeHandler_VW(Register.XMM0, Code.Paddusw_VX_WX, MemorySize.Packed128_UInt16),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pmaxub_P_Q, MemorySize.Packed64_UInt8),
					new OpCodeHandler_VW(Register.XMM0, Code.Pmaxub_VX_WX, MemorySize.Packed128_UInt8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pandn_P_Q, MemorySize.UInt64),
					new OpCodeHandler_VW(Register.XMM0, Code.Pandn_VX_WX, MemorySize.Packed128_UInt64),
					invalid,
					invalid
				),

				// E0
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pavgb_P_Q, MemorySize.Packed64_UInt8),
					new OpCodeHandler_VW(Register.XMM0, Code.Pavgb_VX_WX, MemorySize.Packed128_UInt8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Psraw_P_Q, MemorySize.UInt64),
					new OpCodeHandler_VW(Register.XMM0, Code.Psraw_VX_WX, MemorySize.Packed128_UInt64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Psrad_P_Q, MemorySize.UInt64),
					new OpCodeHandler_VW(Register.XMM0, Code.Psrad_VX_WX, MemorySize.Packed128_UInt64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pavgw_P_Q, MemorySize.Packed64_UInt16),
					new OpCodeHandler_VW(Register.XMM0, Code.Pavgw_VX_WX, MemorySize.Packed128_UInt16),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pmulhuw_P_Q, MemorySize.Packed64_UInt16),
					new OpCodeHandler_VW(Register.XMM0, Code.Pmulhuw_VX_WX, MemorySize.Packed128_UInt16),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pmulhw_P_Q, MemorySize.Packed64_Int16),
					new OpCodeHandler_VW(Register.XMM0, Code.Pmulhw_VX_WX, MemorySize.Packed128_Int16),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Cvttpd2dq_VX_WX, MemorySize.Packed128_Float64),
					new OpCodeHandler_VW(Register.XMM0, Code.Cvtdq2pd_VX_WX, MemorySize.Packed64_Int32),
					new OpCodeHandler_VW(Register.XMM0, Code.Cvtpd2dq_VX_WX, MemorySize.Packed128_Float64)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_MP(Code.Movntq_M_P, MemorySize.Packed64_Int32),
					new OpCodeHandler_MV(Register.XMM0, Code.Movntdq_M_VX, MemorySize.Packed128_Int32),
					invalid,
					invalid
				),

				// E8
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Psubsb_P_Q, MemorySize.Packed64_Int8),
					new OpCodeHandler_VW(Register.XMM0, Code.Psubsb_VX_WX, MemorySize.Packed128_Int8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Psubsw_P_Q, MemorySize.Packed64_Int16),
					new OpCodeHandler_VW(Register.XMM0, Code.Psubsw_VX_WX, MemorySize.Packed128_Int16),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pminsw_P_Q, MemorySize.Packed64_Int16),
					new OpCodeHandler_VW(Register.XMM0, Code.Pminsw_VX_WX, MemorySize.Packed128_Int16),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Por_P_Q, MemorySize.UInt64),
					new OpCodeHandler_VW(Register.XMM0, Code.Por_VX_WX, MemorySize.Packed128_UInt64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Paddsb_P_Q, MemorySize.Packed64_Int8),
					new OpCodeHandler_VW(Register.XMM0, Code.Paddsb_VX_WX, MemorySize.Packed128_Int8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Paddsw_P_Q, MemorySize.Packed64_Int16),
					new OpCodeHandler_VW(Register.XMM0, Code.Paddsw_VX_WX, MemorySize.Packed128_Int16),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pmaxsw_P_Q, MemorySize.Packed64_Int16),
					new OpCodeHandler_VW(Register.XMM0, Code.Pmaxsw_VX_WX, MemorySize.Packed128_Int16),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pxor_P_Q, MemorySize.UInt64),
					new OpCodeHandler_VW(Register.XMM0, Code.Pxor_VX_WX, MemorySize.Packed128_UInt64),
					invalid,
					invalid
				),

				// F0
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					invalid,
					invalid,
					new OpCodeHandler_VM(Register.XMM0, Code.Lddqu_VX_M, MemorySize.UInt128)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Psllw_P_Q, MemorySize.UInt64),
					new OpCodeHandler_VW(Register.XMM0, Code.Psllw_VX_WX, MemorySize.Packed128_UInt64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pslld_P_Q, MemorySize.UInt64),
					new OpCodeHandler_VW(Register.XMM0, Code.Pslld_VX_WX, MemorySize.Packed128_UInt64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Psllq_P_Q, MemorySize.UInt64),
					new OpCodeHandler_VW(Register.XMM0, Code.Psllq_VX_WX, MemorySize.Packed128_UInt64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pmuludq_P_Q, MemorySize.UInt64),
					new OpCodeHandler_VW(Register.XMM0, Code.Pmuludq_VX_WX, MemorySize.Packed128_UInt64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pmaddwd_P_Q, MemorySize.Packed64_Int16),
					new OpCodeHandler_VW(Register.XMM0, Code.Pmaddwd_VX_WX, MemorySize.Packed128_Int16),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Psadbw_P_Q, MemorySize.Packed64_UInt8),
					new OpCodeHandler_VW(Register.XMM0, Code.Psadbw_VX_WX, MemorySize.Packed128_UInt8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_rDI_P_N(Code.Maskmovq_rDI_P_N, MemorySize.UInt64),
					new OpCodeHandler_rDI_VX_RX(Register.XMM0, Code.Maskmovdqu_rDI_VX_RX, MemorySize.UInt128),
					invalid,
					invalid
				),

				// F8
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Psubb_P_Q, MemorySize.Packed64_Int8),
					new OpCodeHandler_VW(Register.XMM0, Code.Psubb_VX_WX, MemorySize.Packed128_Int8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Psubw_P_Q, MemorySize.Packed64_Int16),
					new OpCodeHandler_VW(Register.XMM0, Code.Psubw_VX_WX, MemorySize.Packed128_Int16),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Psubd_P_Q, MemorySize.Packed64_Int32),
					new OpCodeHandler_VW(Register.XMM0, Code.Psubd_VX_WX, MemorySize.Packed128_Int32),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Psubq_P_Q, MemorySize.Int64),
					new OpCodeHandler_VW(Register.XMM0, Code.Psubq_VX_WX, MemorySize.Packed128_Int64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Paddb_P_Q, MemorySize.Packed64_Int8),
					new OpCodeHandler_VW(Register.XMM0, Code.Paddb_VX_WX, MemorySize.Packed128_Int8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Paddw_P_Q, MemorySize.Packed64_Int16),
					new OpCodeHandler_VW(Register.XMM0, Code.Paddw_VX_WX, MemorySize.Packed128_Int16),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Paddd_P_Q, MemorySize.Packed64_Int32),
					new OpCodeHandler_VW(Register.XMM0, Code.Paddd_VX_WX, MemorySize.Packed128_Int32),
					invalid,
					invalid
				),
				new OpCodeHandler_Gv_Ev(Code.Ud0_Gw_Ew, Code.Ud0_Gd_Ed, Code.Ud0_Gq_Eq),
			};

			OneByteHandlers = new OpCodeHandler[0x100] {
				// 00
				new OpCodeHandler_Eb_Gb(Code.Add_Eb_Gb, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Gv(Code.Add_Ew_Gw, Code.Add_Ed_Gd, Code.Add_Eq_Gq, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Gb_Eb(Code.Add_Gb_Eb),
				new OpCodeHandler_Gv_Ev(Code.Add_Gw_Ew, Code.Add_Gd_Ed, Code.Add_Gq_Eq),
				new OpCodeHandler_RegIb(Code.Add_AL_Ib, Register.AL),
				new OpCodeHandler_Reg_Iz(Code.Add_AX_Iw, Code.Add_EAX_Id, Code.Add_RAX_Id64, Register.AX, Register.EAX, Register.RAX),
				invalid,
				invalid,

				// 08
				new OpCodeHandler_Eb_Gb(Code.Or_Eb_Gb, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Gv(Code.Or_Ew_Gw, Code.Or_Ed_Gd, Code.Or_Eq_Gq, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Gb_Eb(Code.Or_Gb_Eb),
				new OpCodeHandler_Gv_Ev(Code.Or_Gw_Ew, Code.Or_Gd_Ed, Code.Or_Gq_Eq),
				new OpCodeHandler_RegIb(Code.Or_AL_Ib, Register.AL),
				new OpCodeHandler_Reg_Iz(Code.Or_AX_Iw, Code.Or_EAX_Id, Code.Or_RAX_Id64, Register.AX, Register.EAX, Register.RAX),
				invalid,
				new OpCodeHandler_AnotherTable(TwoByteHandlers_0FXX),

				// 10
				new OpCodeHandler_Eb_Gb(Code.Adc_Eb_Gb, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Gv(Code.Adc_Ew_Gw, Code.Adc_Ed_Gd, Code.Adc_Eq_Gq, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Gb_Eb(Code.Adc_Gb_Eb),
				new OpCodeHandler_Gv_Ev(Code.Adc_Gw_Ew, Code.Adc_Gd_Ed, Code.Adc_Gq_Eq),
				new OpCodeHandler_RegIb(Code.Adc_AL_Ib, Register.AL),
				new OpCodeHandler_Reg_Iz(Code.Adc_AX_Iw, Code.Adc_EAX_Id, Code.Adc_RAX_Id64, Register.AX, Register.EAX, Register.RAX),
				invalid,
				invalid,

				// 18
				new OpCodeHandler_Eb_Gb(Code.Sbb_Eb_Gb, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Gv(Code.Sbb_Ew_Gw, Code.Sbb_Ed_Gd, Code.Sbb_Eq_Gq, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Gb_Eb(Code.Sbb_Gb_Eb),
				new OpCodeHandler_Gv_Ev(Code.Sbb_Gw_Ew, Code.Sbb_Gd_Ed, Code.Sbb_Gq_Eq),
				new OpCodeHandler_RegIb(Code.Sbb_AL_Ib, Register.AL),
				new OpCodeHandler_Reg_Iz(Code.Sbb_AX_Iw, Code.Sbb_EAX_Id, Code.Sbb_RAX_Id64, Register.AX, Register.EAX, Register.RAX),
				invalid,
				invalid,

				// 20
				new OpCodeHandler_Eb_Gb(Code.And_Eb_Gb, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Gv(Code.And_Ew_Gw, Code.And_Ed_Gd, Code.And_Eq_Gq, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Gb_Eb(Code.And_Gb_Eb),
				new OpCodeHandler_Gv_Ev(Code.And_Gw_Ew, Code.And_Gd_Ed, Code.And_Gq_Eq),
				new OpCodeHandler_RegIb(Code.And_AL_Ib, Register.AL),
				new OpCodeHandler_Reg_Iz(Code.And_AX_Iw, Code.And_EAX_Id, Code.And_RAX_Id64, Register.AX, Register.EAX, Register.RAX),
				invalid,// ES:
				invalid,

				// 28
				new OpCodeHandler_Eb_Gb(Code.Sub_Eb_Gb, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Gv(Code.Sub_Ew_Gw, Code.Sub_Ed_Gd, Code.Sub_Eq_Gq, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Gb_Eb(Code.Sub_Gb_Eb),
				new OpCodeHandler_Gv_Ev(Code.Sub_Gw_Ew, Code.Sub_Gd_Ed, Code.Sub_Gq_Eq),
				new OpCodeHandler_RegIb(Code.Sub_AL_Ib, Register.AL),
				new OpCodeHandler_Reg_Iz(Code.Sub_AX_Iw, Code.Sub_EAX_Id, Code.Sub_RAX_Id64, Register.AX, Register.EAX, Register.RAX),
				invalid,// CS:
				invalid,

				// 30
				new OpCodeHandler_Eb_Gb(Code.Xor_Eb_Gb, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Gv(Code.Xor_Ew_Gw, Code.Xor_Ed_Gd, Code.Xor_Eq_Gq, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Gb_Eb(Code.Xor_Gb_Eb),
				new OpCodeHandler_Gv_Ev(Code.Xor_Gw_Ew, Code.Xor_Gd_Ed, Code.Xor_Gq_Eq),
				new OpCodeHandler_RegIb(Code.Xor_AL_Ib, Register.AL),
				new OpCodeHandler_Reg_Iz(Code.Xor_AX_Iw, Code.Xor_EAX_Id, Code.Xor_RAX_Id64, Register.AX, Register.EAX, Register.RAX),
				invalid,// SS:
				invalid,

				// 38
				new OpCodeHandler_Eb_Gb(Code.Cmp_Eb_Gb),
				new OpCodeHandler_Ev_Gv(Code.Cmp_Ew_Gw, Code.Cmp_Ed_Gd, Code.Cmp_Eq_Gq),
				new OpCodeHandler_Gb_Eb(Code.Cmp_Gb_Eb),
				new OpCodeHandler_Gv_Ev(Code.Cmp_Gw_Ew, Code.Cmp_Gd_Ed, Code.Cmp_Gq_Eq),
				new OpCodeHandler_RegIb(Code.Cmp_AL_Ib, Register.AL),
				new OpCodeHandler_Reg_Iz(Code.Cmp_AX_Iw, Code.Cmp_EAX_Id, Code.Cmp_RAX_Id64, Register.AX, Register.EAX, Register.RAX),
				invalid,// DS:
				invalid,

				// 40
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// 48
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// 50
				new OpCodeHandler_PushSimpleReg(Code.Push_AX, Code.Push_R8W, Code.Push_RAX, Code.Push_R8, Register.AX, Register.R8W, Register.RAX, Register.R8),
				new OpCodeHandler_PushSimpleReg(Code.Push_CX, Code.Push_R9W, Code.Push_RCX, Code.Push_R9, Register.CX, Register.R9W, Register.RCX, Register.R9),
				new OpCodeHandler_PushSimpleReg(Code.Push_DX, Code.Push_R10W, Code.Push_RDX, Code.Push_R10, Register.DX, Register.R10W, Register.RDX, Register.R10),
				new OpCodeHandler_PushSimpleReg(Code.Push_BX, Code.Push_R11W, Code.Push_RBX, Code.Push_R11, Register.BX, Register.R11W, Register.RBX, Register.R11),
				new OpCodeHandler_PushSimpleReg(Code.Push_SP, Code.Push_R12W, Code.Push_RSP, Code.Push_R12, Register.SP, Register.R12W, Register.RSP, Register.R12),
				new OpCodeHandler_PushSimpleReg(Code.Push_BP, Code.Push_R13W, Code.Push_RBP, Code.Push_R13, Register.BP, Register.R13W, Register.RBP, Register.R13),
				new OpCodeHandler_PushSimpleReg(Code.Push_SI, Code.Push_R14W, Code.Push_RSI, Code.Push_R14, Register.SI, Register.R14W, Register.RSI, Register.R14),
				new OpCodeHandler_PushSimpleReg(Code.Push_DI, Code.Push_R15W, Code.Push_RDI, Code.Push_R15, Register.DI, Register.R15W, Register.RDI, Register.R15),

				// 58
				new OpCodeHandler_PushSimpleReg(Code.Pop_AX, Code.Pop_R8W, Code.Pop_RAX, Code.Pop_R8, Register.AX, Register.R8W, Register.RAX, Register.R8),
				new OpCodeHandler_PushSimpleReg(Code.Pop_CX, Code.Pop_R9W, Code.Pop_RCX, Code.Pop_R9, Register.CX, Register.R9W, Register.RCX, Register.R9),
				new OpCodeHandler_PushSimpleReg(Code.Pop_DX, Code.Pop_R10W, Code.Pop_RDX, Code.Pop_R10, Register.DX, Register.R10W, Register.RDX, Register.R10),
				new OpCodeHandler_PushSimpleReg(Code.Pop_BX, Code.Pop_R11W, Code.Pop_RBX, Code.Pop_R11, Register.BX, Register.R11W, Register.RBX, Register.R11),
				new OpCodeHandler_PushSimpleReg(Code.Pop_SP, Code.Pop_R12W, Code.Pop_RSP, Code.Pop_R12, Register.SP, Register.R12W, Register.RSP, Register.R12),
				new OpCodeHandler_PushSimpleReg(Code.Pop_BP, Code.Pop_R13W, Code.Pop_RBP, Code.Pop_R13, Register.BP, Register.R13W, Register.RBP, Register.R13),
				new OpCodeHandler_PushSimpleReg(Code.Pop_SI, Code.Pop_R14W, Code.Pop_RSI, Code.Pop_R14, Register.SI, Register.R14W, Register.RSI, Register.R14),
				new OpCodeHandler_PushSimpleReg(Code.Pop_DI, Code.Pop_R15W, Code.Pop_RDI, Code.Pop_R15, Register.DI, Register.R15W, Register.RDI, Register.R15),

				// 60
				invalid,
				invalid,
				new OpCodeHandler_EVEX(),
				new OpCodeHandler_Gv_Ev2(Code.Movsxd_Gw_Ew, Code.Movsxd_Gd_Ed, Code.Movsxd_Gq_Ed),
				invalid,// FS:
				invalid,// GS:
				invalid,// os
				invalid,// as

				// 68
				new OpCodeHandler_PushIz(Code.Push_Iw, Code.Push_Id64),
				new OpCodeHandler_Gv_Ev_Iz(Code.Imul_Gw_Ew_Iw, Code.Imul_Gd_Ed_Id, Code.Imul_Gq_Eq_Id64),
				new OpCodeHandler_PushIb2(Code.Push_Ib16, Code.Push_Ib64),
				new OpCodeHandler_Gv_Ev_Ib(Code.Imul_Gw_Ew_Ib16, Code.Imul_Gd_Ed_Ib32, Code.Imul_Gq_Eq_Ib64),
				new OpCodeHandler_Yb_Reg(Code.Insb_Yb_DX, Register.DX),
				new OpCodeHandler_Yv_Reg2(Code.Insw_Yw_DX, Code.Insd_Yd_DX, Register.DX, Register.DX),
				new OpCodeHandler_Reg_Xb(Code.Outsb_DX_Xb, Register.DX),
				new OpCodeHandler_Reg_Xv2(Code.Outsw_DX_Xw, Code.Outsd_DX_Xd, Code.Outsd_DX_Xd, Register.DX, Register.DX, Register.DX),

				// 70
				new OpCodeHandler_Jb(Code.Jo_Jb64),
				new OpCodeHandler_Jb(Code.Jno_Jb64),
				new OpCodeHandler_Jb(Code.Jb_Jb64),
				new OpCodeHandler_Jb(Code.Jae_Jb64),
				new OpCodeHandler_Jb(Code.Je_Jb64),
				new OpCodeHandler_Jb(Code.Jne_Jb64),
				new OpCodeHandler_Jb(Code.Jbe_Jb64),
				new OpCodeHandler_Jb(Code.Ja_Jb64),

				// 78
				new OpCodeHandler_Jb(Code.Js_Jb64),
				new OpCodeHandler_Jb(Code.Jns_Jb64),
				new OpCodeHandler_Jb(Code.Jp_Jb64),
				new OpCodeHandler_Jb(Code.Jnp_Jb64),
				new OpCodeHandler_Jb(Code.Jl_Jb64),
				new OpCodeHandler_Jb(Code.Jge_Jb64),
				new OpCodeHandler_Jb(Code.Jle_Jb64),
				new OpCodeHandler_Jb(Code.Jg_Jb64),

				// 80
				new OpCodeHandler_Group(handlers_Grp_80),
				new OpCodeHandler_Group(handlers_Grp_81),
				invalid,
				new OpCodeHandler_Group(handlers_Grp_83),
				new OpCodeHandler_Eb_Gb(Code.Test_Eb_Gb),
				new OpCodeHandler_Ev_Gv(Code.Test_Ew_Gw, Code.Test_Ed_Gd, Code.Test_Eq_Gq),
				new OpCodeHandler_Eb_Gb(Code.Xchg_Eb_Gb, HandlerFlags.XacquireRelease | HandlerFlags.XacquireReleaseNoLock),
				new OpCodeHandler_Ev_Gv(Code.Xchg_Ew_Gw, Code.Xchg_Ed_Gd, Code.Xchg_Eq_Gq, HandlerFlags.XacquireRelease | HandlerFlags.XacquireReleaseNoLock),

				// 88
				new OpCodeHandler_Eb_Gb(Code.Mov_Eb_Gb, HandlerFlags.Xrelease | HandlerFlags.XacquireReleaseNoLock),
				new OpCodeHandler_Ev_Gv(Code.Mov_Ew_Gw, Code.Mov_Ed_Gd, Code.Mov_Eq_Gq, HandlerFlags.Xrelease | HandlerFlags.XacquireReleaseNoLock),
				new OpCodeHandler_Gb_Eb(Code.Mov_Gb_Eb),
				new OpCodeHandler_Gv_Ev(Code.Mov_Gw_Ew, Code.Mov_Gd_Ed, Code.Mov_Gq_Eq),
				new OpCodeHandler_Ev_Sw(Code.Mov_Ew_Sw, Code.Mov_Ed_Sw, Code.Mov_Eq_Sw),
				new OpCodeHandler_Gv_M(Code.Lea_Gw_M, Code.Lea_Gd_M, Code.Lea_Gq_M),
				new OpCodeHandler_Sw_Ev(Code.Mov_Sw_Ew, Code.Mov_Sw_Ed, Code.Mov_Sw_Eq),
				new OpCodeHandler_XOP(new OpCodeHandler_Group(handlers_Grp_8F)),

				// 90
				new OpCodeHandler_Xchg_Reg_rAX(0),
				new OpCodeHandler_Xchg_Reg_rAX(1),
				new OpCodeHandler_Xchg_Reg_rAX(2),
				new OpCodeHandler_Xchg_Reg_rAX(3),
				new OpCodeHandler_Xchg_Reg_rAX(4),
				new OpCodeHandler_Xchg_Reg_rAX(5),
				new OpCodeHandler_Xchg_Reg_rAX(6),
				new OpCodeHandler_Xchg_Reg_rAX(7),

				// 98
				new OpCodeHandler_Simple2(Code.Cbw, Code.Cwde, Code.Cdqe),
				new OpCodeHandler_Simple2(Code.Cwd, Code.Cdq, Code.Cqo),
				invalid,
				new OpCodeHandler_Simple(Code.Wait),
				new OpCodeHandler_PushSimple2(Code.Pushfw, Code.Pushfq),
				new OpCodeHandler_PushSimple2(Code.Popfw, Code.Popfq),
				new OpCodeHandler_Simple(Code.Sahf),
				new OpCodeHandler_Simple(Code.Lahf),

				// A0
				new OpCodeHandler_Reg_Ob(Code.Mov_AL_Ob, Register.AL),
				new OpCodeHandler_Reg_Ov(Code.Mov_AX_Ow, Code.Mov_EAX_Od, Code.Mov_RAX_Oq, Register.AX, Register.EAX, Register.RAX),
				new OpCodeHandler_Ob_Reg(Code.Mov_Ob_AL, Register.AL),
				new OpCodeHandler_Ov_Reg(Code.Mov_Ow_AX, Code.Mov_Od_EAX, Code.Mov_Oq_RAX, Register.AX, Register.EAX, Register.RAX),
				new OpCodeHandler_Yb_Xb(Code.Movsb_Yb_Xb),
				new OpCodeHandler_Yv_Xv(Code.Movsw_Yw_Xw, Code.Movsd_Yd_Xd, Code.Movsq_Yq_Xq),
				new OpCodeHandler_Xb_Yb(Code.Cmpsb_Xb_Yb),
				new OpCodeHandler_Xv_Yv(Code.Cmpsw_Xw_Yw, Code.Cmpsd_Xd_Yd, Code.Cmpsq_Xq_Yq),

				// A8
				new OpCodeHandler_RegIb(Code.Test_AL_Ib, Register.AL),
				new OpCodeHandler_Reg_Iz(Code.Test_AX_Iw, Code.Test_EAX_Id, Code.Test_RAX_Id64, Register.AX, Register.EAX, Register.RAX),
				new OpCodeHandler_Yb_Reg(Code.Stosb_Yb_AL, Register.AL),
				new OpCodeHandler_Yv_Reg(Code.Stosw_Yw_AX, Code.Stosd_Yd_EAX, Code.Stosq_Yq_RAX, Register.AX, Register.EAX, Register.RAX),
				new OpCodeHandler_Reg_Xb(Code.Lodsb_AL_Xb, Register.AL),
				new OpCodeHandler_Reg_Xv(Code.Lodsw_AX_Xw, Code.Lodsd_EAX_Xd, Code.Lodsq_RAX_Xq, Register.AX, Register.EAX, Register.RAX),
				new OpCodeHandler_Reg_Yb(Code.Scasb_AL_Yb, Register.AL),
				new OpCodeHandler_Reg_Yv(Code.Scasw_AX_Yw, Code.Scasd_EAX_Yd, Code.Scasq_RAX_Yq, Register.AX, Register.EAX, Register.RAX),

				// B0
				new OpCodeHandler_RegIb3(0),
				new OpCodeHandler_RegIb3(1),
				new OpCodeHandler_RegIb3(2),
				new OpCodeHandler_RegIb3(3),
				new OpCodeHandler_RegIb3(4),
				new OpCodeHandler_RegIb3(5),
				new OpCodeHandler_RegIb3(6),
				new OpCodeHandler_RegIb3(7),

				// B8
				new OpCodeHandler_RegIz2(0),
				new OpCodeHandler_RegIz2(1),
				new OpCodeHandler_RegIz2(2),
				new OpCodeHandler_RegIz2(3),
				new OpCodeHandler_RegIz2(4),
				new OpCodeHandler_RegIz2(5),
				new OpCodeHandler_RegIz2(6),
				new OpCodeHandler_RegIz2(7),

				// C0
				new OpCodeHandler_Group(handlers_Grp_C0),
				new OpCodeHandler_Group(handlers_Grp_C1),
				new OpCodeHandler_BranchIw(Code.Retnq_Iw),
				new OpCodeHandler_BranchSimple(Code.Retnq),
				new OpCodeHandler_VEX3(),
				new OpCodeHandler_VEX2(),
				new OpCodeHandler_Group8x64(handlers_Grp_C6_lo, handlers_Grp_C6_hi),
				new OpCodeHandler_Group8x64(handlers_Grp_C7_lo, handlers_Grp_C7_hi),

				// C8
				new OpCodeHandler_Iw_Ib(Code.Enterw_Iw_Ib, Code.Enterq_Iw_Ib),
				new OpCodeHandler_Simple3(Code.Leavew, Code.Leaveq),
				new OpCodeHandler_Simple2Iw(Code.Retfw_Iw, Code.Retfd_Iw ,Code.Retfq_Iw),
				new OpCodeHandler_Simple2(Code.Retfw, Code.Retfd ,Code.Retfq),
				new OpCodeHandler_Simple(Code.Int3),
				new OpCodeHandler_Ib(Code.Int_Ib),
				invalid,
				new OpCodeHandler_Simple2(Code.Iretw, Code.Iretd, Code.Iretq),

				// D0
				new OpCodeHandler_Group(handlers_Grp_D0),
				new OpCodeHandler_Group(handlers_Grp_D1),
				new OpCodeHandler_Group(handlers_Grp_D2),
				new OpCodeHandler_Group(handlers_Grp_D3),
				invalid,
				invalid,
				invalid,
				new OpCodeHandler_MemBx(Code.Xlatb),

				// D8
				new OpCodeHandler_Group8x8(OpCodeHandlersFpu64Tables.handlers_FPU_D8_low, OpCodeHandlersFpu64Tables.handlers_FPU_D8_high),
				new OpCodeHandler_Group8x64(OpCodeHandlersFpu64Tables.handlers_FPU_D9_low, OpCodeHandlersFpu64Tables.handlers_FPU_D9_high),
				new OpCodeHandler_Group8x64(OpCodeHandlersFpu64Tables.handlers_FPU_DA_low, OpCodeHandlersFpu64Tables.handlers_FPU_DA_high),
				new OpCodeHandler_Group8x64(OpCodeHandlersFpu64Tables.handlers_FPU_DB_low, OpCodeHandlersFpu64Tables.handlers_FPU_DB_high),
				new OpCodeHandler_Group8x8(OpCodeHandlersFpu64Tables.handlers_FPU_DC_low, OpCodeHandlersFpu64Tables.handlers_FPU_DC_high),
				new OpCodeHandler_Group8x8(OpCodeHandlersFpu64Tables.handlers_FPU_DD_low, OpCodeHandlersFpu64Tables.handlers_FPU_DD_high),
				new OpCodeHandler_Group8x64(OpCodeHandlersFpu64Tables.handlers_FPU_DE_low, OpCodeHandlersFpu64Tables.handlers_FPU_DE_high),
				new OpCodeHandler_Group8x64(OpCodeHandlersFpu64Tables.handlers_FPU_DF_low, OpCodeHandlersFpu64Tables.handlers_FPU_DF_high),

				// E0
				new OpCodeHandler_Jb2(Code.Loopne_Jb64_ECX, Code.Loopne_Jb64_RCX),
				new OpCodeHandler_Jb2(Code.Loope_Jb64_ECX, Code.Loope_Jb64_RCX),
				new OpCodeHandler_Jb2(Code.Loop_Jb64_ECX, Code.Loop_Jb64_RCX),
				new OpCodeHandler_Jb2(Code.Jecxz_Jb64, Code.Jrcxz_Jb64),
				new OpCodeHandler_RegIb(Code.In_AL_Ib, Register.AL),
				new OpCodeHandler_Reg_Ib2(Code.In_AX_Ib, Code.In_EAX_Ib, Register.AX, Register.EAX),
				new OpCodeHandler_IbReg(Code.Out_Ib_AL, Register.AL),
				new OpCodeHandler_IbReg2(Code.Out_Ib_AX, Code.Out_Ib_EAX, Register.AX, Register.EAX),

				// E8
				new OpCodeHandler_Jz(Code.Call_Jd64),
				new OpCodeHandler_Jz(Code.Jmp_Jd64),
				invalid,
				new OpCodeHandler_Jb(Code.Jmp_Jb64),
				new OpCodeHandler_AL_DX(Code.In_AL_DX),
				new OpCodeHandler_eAX_DX(Code.In_AX_DX, Code.In_EAX_DX),
				new OpCodeHandler_DX_AL(Code.Out_DX_AL),
				new OpCodeHandler_DX_eAX(Code.Out_DX_AX, Code.Out_DX_EAX),

				// F0
				invalid,// lock
				new OpCodeHandler_Simple(Code.Int1),
				invalid,// repne
				invalid,// rep
				new OpCodeHandler_Simple(Code.Hlt),
				new OpCodeHandler_Simple(Code.Cmc),
				new OpCodeHandler_Group(handlers_Grp_F6),
				new OpCodeHandler_Group(handlers_Grp_F7),

				// F8
				new OpCodeHandler_Simple(Code.Clc),
				new OpCodeHandler_Simple(Code.Stc),
				new OpCodeHandler_Simple(Code.Cli),
				new OpCodeHandler_Simple(Code.Sti),
				new OpCodeHandler_Simple(Code.Cld),
				new OpCodeHandler_Simple(Code.Std),
				new OpCodeHandler_Group(handlers_Grp_FE),
				new OpCodeHandler_Group(handlers_Grp_FF),
			};
		}
	}
}
#endif
