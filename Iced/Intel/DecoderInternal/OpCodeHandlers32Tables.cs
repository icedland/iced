/*
    Copyright (C) 2018-2019 de4dot@gmail.com

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

#if !NO_DECODER32 && !NO_DECODER
namespace Iced.Intel.DecoderInternal.OpCodeHandlers32 {
	/// <summary>
	/// Handlers for 16/32-bit mode
	/// </summary>
	static class OpCodeHandlers32Tables {
		internal static readonly OpCodeHandler[] OneByteHandlers;

		static OpCodeHandlers32Tables() {
			// Store it in a local. Instead of 1000+ ldfld, we'll have 1000+ ldloc.0 (save 4 bytes per load)
			var invalid = OpCodeHandler_Invalid.Instance;

			var handlers_Grp_80 = new OpCodeHandler[8] {
				new OpCodeHandler_Eb_Ib(Code.Add_rm8_imm8, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Eb_Ib(Code.Or_rm8_imm8, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Eb_Ib(Code.Adc_rm8_imm8, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Eb_Ib(Code.Sbb_rm8_imm8, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Eb_Ib(Code.And_rm8_imm8, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Eb_Ib(Code.Sub_rm8_imm8, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Eb_Ib(Code.Xor_rm8_imm8, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Eb_Ib(Code.Cmp_rm8_imm8),
			};

			var handlers_Grp_81 = new OpCodeHandler[8] {
				new OpCodeHandler_Ev_Iz(Code.Add_rm16_imm16, Code.Add_rm32_imm32, Code.Add_rm64_imm32, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Iz(Code.Or_rm16_imm16, Code.Or_rm32_imm32, Code.Or_rm64_imm32, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Iz(Code.Adc_rm16_imm16, Code.Adc_rm32_imm32, Code.Adc_rm64_imm32, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Iz(Code.Sbb_rm16_imm16, Code.Sbb_rm32_imm32, Code.Sbb_rm64_imm32, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Iz(Code.And_rm16_imm16, Code.And_rm32_imm32, Code.And_rm64_imm32, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Iz(Code.Sub_rm16_imm16, Code.Sub_rm32_imm32, Code.Sub_rm64_imm32, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Iz(Code.Xor_rm16_imm16, Code.Xor_rm32_imm32, Code.Xor_rm64_imm32, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Iz(Code.Cmp_rm16_imm16, Code.Cmp_rm32_imm32, Code.Cmp_rm64_imm32),
			};

			var handlers_Grp_82 = new OpCodeHandler[8] {
				new OpCodeHandler_Eb_Ib(Code.Add_rm8_imm8_82, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Eb_Ib(Code.Or_rm8_imm8_82, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Eb_Ib(Code.Adc_rm8_imm8_82, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Eb_Ib(Code.Sbb_rm8_imm8_82, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Eb_Ib(Code.And_rm8_imm8_82, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Eb_Ib(Code.Sub_rm8_imm8_82, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Eb_Ib(Code.Xor_rm8_imm8_82, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Eb_Ib(Code.Cmp_rm8_imm8_82),
			};

			var handlers_Grp_83 = new OpCodeHandler[8] {
				new OpCodeHandler_Ev_Ib(Code.Add_rm16_imm8, Code.Add_rm32_imm8, Code.Add_rm64_imm8, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Ib(Code.Or_rm16_imm8, Code.Or_rm32_imm8, Code.Or_rm64_imm8, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Ib(Code.Adc_rm16_imm8, Code.Adc_rm32_imm8, Code.Adc_rm64_imm8, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Ib(Code.Sbb_rm16_imm8, Code.Sbb_rm32_imm8, Code.Sbb_rm64_imm8, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Ib(Code.And_rm16_imm8, Code.And_rm32_imm8, Code.And_rm64_imm8, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Ib(Code.Sub_rm16_imm8, Code.Sub_rm32_imm8, Code.Sub_rm64_imm8, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Ib(Code.Xor_rm16_imm8, Code.Xor_rm32_imm8, Code.Xor_rm64_imm8, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Ib(Code.Cmp_rm16_imm8, Code.Cmp_rm32_imm8, Code.Cmp_rm64_imm8),
			};

			var handlers_Grp_8F = new OpCodeHandler[8] {
				new OpCodeHandler_Ev(Code.Pop_rm16, Code.Pop_rm32, Code.Pop_rm64),
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
			};

			var handlers_Grp_C0 = new OpCodeHandler[8] {
				new OpCodeHandler_Eb_Ib(Code.Rol_rm8_imm8),
				new OpCodeHandler_Eb_Ib(Code.Ror_rm8_imm8),
				new OpCodeHandler_Eb_Ib(Code.Rcl_rm8_imm8),
				new OpCodeHandler_Eb_Ib(Code.Rcr_rm8_imm8),
				new OpCodeHandler_Eb_Ib(Code.Shl_rm8_imm8),
				new OpCodeHandler_Eb_Ib(Code.Shr_rm8_imm8),
				new OpCodeHandler_Eb_Ib(Code.Sal_rm8_imm8),
				new OpCodeHandler_Eb_Ib(Code.Sar_rm8_imm8),
			};

			var handlers_Grp_C1 = new OpCodeHandler[8] {
				new OpCodeHandler_Ev_Ib2(Code.Rol_rm16_imm8, Code.Rol_rm32_imm8, Code.Rol_rm64_imm8),
				new OpCodeHandler_Ev_Ib2(Code.Ror_rm16_imm8, Code.Ror_rm32_imm8, Code.Ror_rm64_imm8),
				new OpCodeHandler_Ev_Ib2(Code.Rcl_rm16_imm8, Code.Rcl_rm32_imm8, Code.Rcl_rm64_imm8),
				new OpCodeHandler_Ev_Ib2(Code.Rcr_rm16_imm8, Code.Rcr_rm32_imm8, Code.Rcr_rm64_imm8),
				new OpCodeHandler_Ev_Ib2(Code.Shl_rm16_imm8, Code.Shl_rm32_imm8, Code.Shl_rm64_imm8),
				new OpCodeHandler_Ev_Ib2(Code.Shr_rm16_imm8, Code.Shr_rm32_imm8, Code.Shr_rm64_imm8),
				new OpCodeHandler_Ev_Ib2(Code.Sal_rm16_imm8, Code.Sal_rm32_imm8, Code.Sal_rm64_imm8),
				new OpCodeHandler_Ev_Ib2(Code.Sar_rm16_imm8, Code.Sar_rm32_imm8, Code.Sar_rm64_imm8),
			};

			var handlers_Grp_D0 = new OpCodeHandler[8] {
				new OpCodeHandler_Eb_1(Code.Rol_rm8_1),
				new OpCodeHandler_Eb_1(Code.Ror_rm8_1),
				new OpCodeHandler_Eb_1(Code.Rcl_rm8_1),
				new OpCodeHandler_Eb_1(Code.Rcr_rm8_1),
				new OpCodeHandler_Eb_1(Code.Shl_rm8_1),
				new OpCodeHandler_Eb_1(Code.Shr_rm8_1),
				new OpCodeHandler_Eb_1(Code.Sal_rm8_1),
				new OpCodeHandler_Eb_1(Code.Sar_rm8_1),
			};

			var handlers_Grp_D1 = new OpCodeHandler[8] {
				new OpCodeHandler_Ev_1(Code.Rol_rm16_1, Code.Rol_rm32_1, Code.Rol_rm64_1),
				new OpCodeHandler_Ev_1(Code.Ror_rm16_1, Code.Ror_rm32_1, Code.Ror_rm64_1),
				new OpCodeHandler_Ev_1(Code.Rcl_rm16_1, Code.Rcl_rm32_1, Code.Rcl_rm64_1),
				new OpCodeHandler_Ev_1(Code.Rcr_rm16_1, Code.Rcr_rm32_1, Code.Rcr_rm64_1),
				new OpCodeHandler_Ev_1(Code.Shl_rm16_1, Code.Shl_rm32_1, Code.Shl_rm64_1),
				new OpCodeHandler_Ev_1(Code.Shr_rm16_1, Code.Shr_rm32_1, Code.Shr_rm64_1),
				new OpCodeHandler_Ev_1(Code.Sal_rm16_1, Code.Sal_rm32_1, Code.Sal_rm64_1),
				new OpCodeHandler_Ev_1(Code.Sar_rm16_1, Code.Sar_rm32_1, Code.Sar_rm64_1),
			};

			var handlers_Grp_D2 = new OpCodeHandler[8] {
				new OpCodeHandler_Eb_CL(Code.Rol_rm8_CL),
				new OpCodeHandler_Eb_CL(Code.Ror_rm8_CL),
				new OpCodeHandler_Eb_CL(Code.Rcl_rm8_CL),
				new OpCodeHandler_Eb_CL(Code.Rcr_rm8_CL),
				new OpCodeHandler_Eb_CL(Code.Shl_rm8_CL),
				new OpCodeHandler_Eb_CL(Code.Shr_rm8_CL),
				new OpCodeHandler_Eb_CL(Code.Sal_rm8_CL),
				new OpCodeHandler_Eb_CL(Code.Sar_rm8_CL),
			};

			var handlers_Grp_D3 = new OpCodeHandler[8] {
				new OpCodeHandler_Ev_CL(Code.Rol_rm16_CL, Code.Rol_rm32_CL, Code.Rol_rm64_CL),
				new OpCodeHandler_Ev_CL(Code.Ror_rm16_CL, Code.Ror_rm32_CL, Code.Ror_rm64_CL),
				new OpCodeHandler_Ev_CL(Code.Rcl_rm16_CL, Code.Rcl_rm32_CL, Code.Rcl_rm64_CL),
				new OpCodeHandler_Ev_CL(Code.Rcr_rm16_CL, Code.Rcr_rm32_CL, Code.Rcr_rm64_CL),
				new OpCodeHandler_Ev_CL(Code.Shl_rm16_CL, Code.Shl_rm32_CL, Code.Shl_rm64_CL),
				new OpCodeHandler_Ev_CL(Code.Shr_rm16_CL, Code.Shr_rm32_CL, Code.Shr_rm64_CL),
				new OpCodeHandler_Ev_CL(Code.Sal_rm16_CL, Code.Sal_rm32_CL, Code.Sal_rm64_CL),
				new OpCodeHandler_Ev_CL(Code.Sar_rm16_CL, Code.Sar_rm32_CL, Code.Sar_rm64_CL),
			};

			var handlers_Grp_F6 = new OpCodeHandler[8] {
				new OpCodeHandler_Eb_Ib(Code.Test_rm8_imm8),
				new OpCodeHandler_Eb_Ib(Code.Test_rm8_imm8_F6r1),
				new OpCodeHandler_Eb(Code.Not_rm8, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Eb(Code.Neg_rm8, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Eb(Code.Mul_rm8),
				new OpCodeHandler_Eb(Code.Imul_rm8),
				new OpCodeHandler_Eb(Code.Div_rm8),
				new OpCodeHandler_Eb(Code.Idiv_rm8),
			};

			var handlers_Grp_F7 = new OpCodeHandler[8] {
				new OpCodeHandler_Ev_Iz(Code.Test_rm16_imm16, Code.Test_rm32_imm32, Code.Test_rm64_imm32),
				new OpCodeHandler_Ev_Iz(Code.Test_rm16_imm16_F7r1, Code.Test_rm32_imm32_F7r1, Code.Test_rm64_imm32_F7r1),
				new OpCodeHandler_Ev(Code.Not_rm16, Code.Not_rm32, Code.Not_rm64, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev(Code.Neg_rm16, Code.Neg_rm32, Code.Neg_rm64, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev(Code.Mul_rm16, Code.Mul_rm32, Code.Mul_rm64),
				new OpCodeHandler_Ev(Code.Imul_rm16, Code.Imul_rm32, Code.Imul_rm64),
				new OpCodeHandler_Ev(Code.Div_rm16, Code.Div_rm32, Code.Div_rm64),
				new OpCodeHandler_Ev(Code.Idiv_rm16, Code.Idiv_rm32, Code.Idiv_rm64),
			};

			var handlers_Grp_FE = new OpCodeHandler[8] {
				new OpCodeHandler_Eb(Code.Inc_rm8, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Eb(Code.Dec_rm8, HandlerFlags.XacquireRelease),
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
			};

			var handlers_Grp_FF = new OpCodeHandler[8] {
				new OpCodeHandler_Ev(Code.Inc_rm16, Code.Inc_rm32, Code.Inc_rm64, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev(Code.Dec_rm16, Code.Dec_rm32, Code.Dec_rm64, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Evj(Code.Call_rm16, Code.Call_rm32),
				new OpCodeHandler_Ep(Code.Call_m1616, Code.Call_m3216, Code.Call_m6416),
				new OpCodeHandler_Evj(Code.Jmp_rm16, Code.Jmp_rm32),
				new OpCodeHandler_Ep(Code.Jmp_m1616, Code.Jmp_m3216, Code.Jmp_m6416),
				new OpCodeHandler_Ev(Code.Push_rm16, Code.Push_rm32, Code.Push_rm64),
				invalid,
			};

			var handlers_Grp_0F00 = new OpCodeHandler[8] {
				new OpCodeHandler_Evw(Code.Sldt_rm16, Code.Sldt_r32m16, Code.Sldt_r64m16),
				new OpCodeHandler_Evw(Code.Str_rm16, Code.Str_r32m16, Code.Str_r64m16),
				new OpCodeHandler_Ew(Code.Lldt_rm16, Code.Lldt_r32m16, Code.Lldt_r64m16),
				new OpCodeHandler_Ew(Code.Ltr_rm16, Code.Ltr_r32m16, Code.Ltr_r64m16),
				new OpCodeHandler_Ew(Code.Verr_rm16, Code.Verr_r32m16, Code.Verr_r64m16),
				new OpCodeHandler_Ew(Code.Verw_rm16, Code.Verw_r32m16, Code.Verw_r64m16),
				new OpCodeHandler_Ev(Code.Jmpe_rm16, Code.Jmpe_rm32, Code.INVALID),
				invalid,
			};

			var handlers_Grp_0F01_lo = new OpCodeHandler[8] {
				new OpCodeHandler_Ms(Code.Sgdt_m40, Code.Sgdt_m48, Code.Sgdt_m80),
				new OpCodeHandler_Ms(Code.Sidt_m40, Code.Sidt_m48, Code.Sidt_m80),
				new OpCodeHandler_Ms(Code.Lgdt_m40, Code.Lgdt_m48, Code.Lgdt_m80),
				new OpCodeHandler_Ms(Code.Lidt_m40, Code.Lidt_m48, Code.Lidt_m80),
				new OpCodeHandler_Evw(Code.Smsw_rm16, Code.Smsw_r32m16, Code.Smsw_r64m16),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					invalid,
					new OpCodeHandler_M(Code.Rstorssp_m64),
					invalid
				),
				new OpCodeHandler_Evw(Code.Lmsw_rm16, Code.Lmsw_r32m16, Code.Lmsw_r64m16),
				new OpCodeHandler_M(Code.Invlpg_m),
			};

			var handlers_Grp_0F01_hi = new OpCodeHandler[0x40] {
				// C0
				new OpCodeHandler_Simple(Code.Enclv),
				new OpCodeHandler_Simple(Code.Vmcall),
				new OpCodeHandler_Simple(Code.Vmlaunch),
				new OpCodeHandler_Simple(Code.Vmresume),
				new OpCodeHandler_Simple(Code.Vmxoff),
				new OpCodeHandler_Simple(Code.Pconfig),
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
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					invalid,
					new OpCodeHandler_Simple_ModRM(Code.Setssbsy),
					invalid
				),
				null,
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					invalid,
					new OpCodeHandler_Simple_ModRM(Code.Saveprevssp),
					invalid
				),
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
				new OpCodeHandler_Simple5(Code.Clzerow, Code.Clzerod, Code.Clzeroq),
				null,
				null,
				null,
			};

			var handlers_Grp_0FBA = new OpCodeHandler[8] {
				invalid,
				invalid,
				invalid,
				invalid,
				new OpCodeHandler_Ev_Ib2(Code.Bt_rm16_imm8, Code.Bt_rm32_imm8, Code.Bt_rm64_imm8),
				new OpCodeHandler_Ev_Ib2(Code.Bts_rm16_imm8, Code.Bts_rm32_imm8, Code.Bts_rm64_imm8, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Ib2(Code.Btr_rm16_imm8, Code.Btr_rm32_imm8, Code.Btr_rm64_imm8, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Ib2(Code.Btc_rm16_imm8, Code.Btc_rm32_imm8, Code.Btc_rm64_imm8, HandlerFlags.XacquireRelease),
			};

			var handlers_Grp_0FC7 = new OpCodeHandler[8] {
				invalid,
				new OpCodeHandler_M_REXW(Code.Cmpxchg8b_m64, Code.Cmpxchg16b_m128, HandlerFlags.XacquireRelease, HandlerFlags.None),
				invalid,
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_M_REXW(Code.Xrstors_m, Code.Xrstors64_m),
					invalid,
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_M_REXW(Code.Xsavec_m, Code.Xsavec64_m),
					invalid,
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Ev_REXW(Code.Xsaves_m, Code.Xsaves64_m, allowReg: false, allowMem: true),
					invalid,
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix3(
					new OpCodeHandler_Rv(Code.Rdrand_r16, Code.Rdrand_r32, Code.Rdrand_r64),
					new OpCodeHandler_M(Code.Vmptrld_m64),
					new OpCodeHandler_Rv(Code.Rdrand_r16, Code.Rdrand_r32, Code.Rdrand_r64),
					new OpCodeHandler_M(Code.Vmclear_m64),
					invalid,
					new OpCodeHandler_M(Code.Vmxon_m64),
					invalid,
					invalid,
					LegacyHandlerFlags.HandlerReg | LegacyHandlerFlags.Handler66Reg
				),
				new OpCodeHandler_MandatoryPrefix3(
					new OpCodeHandler_Rv(Code.Rdseed_r16, Code.Rdseed_r32, Code.Rdseed_r64),
					new OpCodeHandler_M(Code.Vmptrst_m64),
					new OpCodeHandler_Rv(Code.Rdseed_r16, Code.Rdseed_r32, Code.Rdseed_r64),
					invalid,
					new OpCodeHandler_Rv_32_64(Code.Rdpid_r32, Code.Rdpid_r64),
					invalid,
					invalid,
					invalid,
					LegacyHandlerFlags.HandlerReg | LegacyHandlerFlags.Handler66Reg
				),
			};

			var handlers_Grp_C6_lo = new OpCodeHandler[8] {
				new OpCodeHandler_Eb_Ib(Code.Mov_rm8_imm8, HandlerFlags.Xrelease | HandlerFlags.XacquireReleaseNoLock),
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
				new OpCodeHandler_Ib3(Code.Xabort_imm8),
				null,
				null,
				null,
				null,
				null,
				null,
				null,
			};

			var handlers_Grp_C7_lo = new OpCodeHandler[8] {
				new OpCodeHandler_Ev_Iz(Code.Mov_rm16_imm16, Code.Mov_rm32_imm32, Code.Mov_rm64_imm32, HandlerFlags.Xrelease | HandlerFlags.XacquireReleaseNoLock),
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
				new OpCodeHandler_Jx(Code.Xbegin_rel16, Code.Xbegin_rel32, Code.Xbegin_rel32_REXW),
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
					new OpCodeHandler_NIb(Code.Psrlw_mm_imm8),
					new OpCodeHandler_RIb(Register.XMM0, Code.Psrlw_xmm_imm8),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_NIb(Code.Psraw_mm_imm8),
					new OpCodeHandler_RIb(Register.XMM0, Code.Psraw_xmm_imm8),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_NIb(Code.Psllw_mm_imm8),
					new OpCodeHandler_RIb(Register.XMM0, Code.Psllw_xmm_imm8),
					invalid,
					invalid
				),
				invalid,
			};

			var handlers_Grp_0F72 = new OpCodeHandler[8] {
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_NIb(Code.Psrld_mm_imm8),
					new OpCodeHandler_RIb(Register.XMM0, Code.Psrld_xmm_imm8),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_NIb(Code.Psrad_mm_imm8),
					new OpCodeHandler_RIb(Register.XMM0, Code.Psrad_xmm_imm8),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_NIb(Code.Pslld_mm_imm8),
					new OpCodeHandler_RIb(Register.XMM0, Code.Pslld_xmm_imm8),
					invalid,
					invalid
				),
				invalid,
			};

			var handlers_Grp_0F73 = new OpCodeHandler[8] {
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_NIb(Code.Psrlq_mm_imm8),
					new OpCodeHandler_RIb(Register.XMM0, Code.Psrlq_xmm_imm8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_RIb(Register.XMM0, Code.Psrldq_xmm_imm8),
					invalid,
					invalid
				),
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_NIb(Code.Psllq_mm_imm8),
					new OpCodeHandler_RIb(Register.XMM0, Code.Psllq_xmm_imm8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_RIb(Register.XMM0, Code.Pslldq_xmm_imm8),
					invalid,
					invalid
				),
			};

			var handlers_Grp_0FAE_lo = new OpCodeHandler[8] {
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Options_DontReadModRM(
						new OpCodeHandler_M(Code.Fxsave_m512byte, Code.Fxsave64_m512byte),
						new OpCodeHandler_M(Code.Zalloc_m256), DecoderOptions.Zalloc
					),
					invalid,
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_M(Code.Fxrstor_m512byte, Code.Fxrstor64_m512byte),
					invalid,
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_M(Code.Ldmxcsr_m32),
					invalid,
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_M(Code.Stmxcsr_m32),
					invalid,
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_M(Code.Xsave_m, Code.Xsave64_m),
					invalid,
					new OpCodeHandler_Ev_REXW(Code.Ptwrite_rm32, Code.Ptwrite_rm64, allowReg: true, allowMem: true),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_M(Code.Xrstor_m, Code.Xrstor64_m),
					invalid,
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_M(Code.Xsaveopt_m, Code.Xsaveopt64_m),
					new OpCodeHandler_M(Code.Clwb_m8),
					new OpCodeHandler_M(Code.Clrssbsy_m64),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_M(Code.Clflush_m8),
					new OpCodeHandler_M(Code.Clflushopt_m8),
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
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Simple_ModRM(Code.Lfence),
					invalid,
					new OpCodeHandler_Ev_REXW(Code.Incsspd_r32, Code.Incsspq_r64, allowReg: true, allowMem: false),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Simple_ModRM(Code.Lfence_E9),
					invalid,
					new OpCodeHandler_Ev_REXW(Code.Incsspd_r32, Code.Incsspq_r64, allowReg: true, allowMem: false),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Simple_ModRM(Code.Lfence_EA),
					invalid,
					new OpCodeHandler_Ev_REXW(Code.Incsspd_r32, Code.Incsspq_r64, allowReg: true, allowMem: false),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Simple_ModRM(Code.Lfence_EB),
					invalid,
					new OpCodeHandler_Ev_REXW(Code.Incsspd_r32, Code.Incsspq_r64, allowReg: true, allowMem: false),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Simple_ModRM(Code.Lfence_EC),
					invalid,
					new OpCodeHandler_Ev_REXW(Code.Incsspd_r32, Code.Incsspq_r64, allowReg: true, allowMem: false),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Simple_ModRM(Code.Lfence_ED),
					invalid,
					new OpCodeHandler_Ev_REXW(Code.Incsspd_r32, Code.Incsspq_r64, allowReg: true, allowMem: false),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Simple_ModRM(Code.Lfence_EE),
					invalid,
					new OpCodeHandler_Ev_REXW(Code.Incsspd_r32, Code.Incsspq_r64, allowReg: true, allowMem: false),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Simple_ModRM(Code.Lfence_EF),
					invalid,
					new OpCodeHandler_Ev_REXW(Code.Incsspd_r32, Code.Incsspq_r64, allowReg: true, allowMem: false),
					invalid
				),

				// F0
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Simple_ModRM(Code.Mfence),
					new OpCodeHandler_Ev_REXW(Code.Tpause_r32, Code.Tpause_r64, allowReg: true, allowMem: false),
					new OpCodeHandler_Simple5_ModRM_as(Code.Umonitor_r16, Code.Umonitor_r32, Code.Umonitor_r64),
					new OpCodeHandler_Ev_REXW(Code.Umwait_r32, Code.Umwait_r64, allowReg: true, allowMem: false)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Simple_ModRM(Code.Mfence_F1),
					new OpCodeHandler_Ev_REXW(Code.Tpause_r32, Code.Tpause_r64, allowReg: true, allowMem: false),
					new OpCodeHandler_Simple5_ModRM_as(Code.Umonitor_r16, Code.Umonitor_r32, Code.Umonitor_r64),
					new OpCodeHandler_Ev_REXW(Code.Umwait_r32, Code.Umwait_r64, allowReg: true, allowMem: false)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Simple_ModRM(Code.Mfence_F2),
					new OpCodeHandler_Ev_REXW(Code.Tpause_r32, Code.Tpause_r64, allowReg: true, allowMem: false),
					new OpCodeHandler_Simple5_ModRM_as(Code.Umonitor_r16, Code.Umonitor_r32, Code.Umonitor_r64),
					new OpCodeHandler_Ev_REXW(Code.Umwait_r32, Code.Umwait_r64, allowReg: true, allowMem: false)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Simple_ModRM(Code.Mfence_F3),
					new OpCodeHandler_Ev_REXW(Code.Tpause_r32, Code.Tpause_r64, allowReg: true, allowMem: false),
					new OpCodeHandler_Simple5_ModRM_as(Code.Umonitor_r16, Code.Umonitor_r32, Code.Umonitor_r64),
					new OpCodeHandler_Ev_REXW(Code.Umwait_r32, Code.Umwait_r64, allowReg: true, allowMem: false)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Simple_ModRM(Code.Mfence_F4),
					new OpCodeHandler_Ev_REXW(Code.Tpause_r32, Code.Tpause_r64, allowReg: true, allowMem: false),
					new OpCodeHandler_Simple5_ModRM_as(Code.Umonitor_r16, Code.Umonitor_r32, Code.Umonitor_r64),
					new OpCodeHandler_Ev_REXW(Code.Umwait_r32, Code.Umwait_r64, allowReg: true, allowMem: false)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Simple_ModRM(Code.Mfence_F5),
					new OpCodeHandler_Ev_REXW(Code.Tpause_r32, Code.Tpause_r64, allowReg: true, allowMem: false),
					new OpCodeHandler_Simple5_ModRM_as(Code.Umonitor_r16, Code.Umonitor_r32, Code.Umonitor_r64),
					new OpCodeHandler_Ev_REXW(Code.Umwait_r32, Code.Umwait_r64, allowReg: true, allowMem: false)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Simple_ModRM(Code.Mfence_F6),
					new OpCodeHandler_Ev_REXW(Code.Tpause_r32, Code.Tpause_r64, allowReg: true, allowMem: false),
					new OpCodeHandler_Simple5_ModRM_as(Code.Umonitor_r16, Code.Umonitor_r32, Code.Umonitor_r64),
					new OpCodeHandler_Ev_REXW(Code.Umwait_r32, Code.Umwait_r64, allowReg: true, allowMem: false)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Simple_ModRM(Code.Mfence_F7),
					new OpCodeHandler_Ev_REXW(Code.Tpause_r32, Code.Tpause_r64, allowReg: true, allowMem: false),
					new OpCodeHandler_Simple5_ModRM_as(Code.Umonitor_r16, Code.Umonitor_r32, Code.Umonitor_r64),
					new OpCodeHandler_Ev_REXW(Code.Umwait_r32, Code.Umwait_r64, allowReg: true, allowMem: false)
				),

				// F8
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Simple_ModRM(Code.Sfence),
					new OpCodeHandler_Options_DontReadModRM(
						invalid,
						new OpCodeHandler_Simple_ModRM(Code.Pcommit), DecoderOptions.Pcommit
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Simple_ModRM(Code.Sfence_F9),
					invalid,
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Simple_ModRM(Code.Sfence_FA),
					invalid,
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Simple_ModRM(Code.Sfence_FB),
					invalid,
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Simple_ModRM(Code.Sfence_FC),
					invalid,
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Simple_ModRM(Code.Sfence_FD),
					invalid,
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Simple_ModRM(Code.Sfence_FE),
					invalid,
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Simple_ModRM(Code.Sfence_FF),
					invalid,
					invalid,
					invalid
				),
			};

			var reservedNop_0F0D = new OpCodeHandler_Ev_Gv(Code.ReservedNop_rm16_r16_0F0D, Code.ReservedNop_rm32_r32_0F0D, Code.ReservedNop_rm64_r64_0F0D);
			var reservedNop_0F18 = new OpCodeHandler_Ev_Gv(Code.ReservedNop_rm16_r16_0F18, Code.ReservedNop_rm32_r32_0F18, Code.ReservedNop_rm64_r64_0F18);
			var reservedNop_0F19 = new OpCodeHandler_Ev_Gv(Code.ReservedNop_rm16_r16_0F19, Code.ReservedNop_rm32_r32_0F19, Code.ReservedNop_rm64_r64_0F19);
			var reservedNop_0F1A = new OpCodeHandler_Ev_Gv(Code.ReservedNop_rm16_r16_0F1A, Code.ReservedNop_rm32_r32_0F1A, Code.ReservedNop_rm64_r64_0F1A);
			var reservedNop_0F1B = new OpCodeHandler_Ev_Gv(Code.ReservedNop_rm16_r16_0F1B, Code.ReservedNop_rm32_r32_0F1B, Code.ReservedNop_rm64_r64_0F1B);
			var reservedNop_0F1C = new OpCodeHandler_Ev_Gv(Code.ReservedNop_rm16_r16_0F1C, Code.ReservedNop_rm32_r32_0F1C, Code.ReservedNop_rm64_r64_0F1C);
			var reservedNop_0F1D = new OpCodeHandler_Ev_Gv(Code.ReservedNop_rm16_r16_0F1D, Code.ReservedNop_rm32_r32_0F1D, Code.ReservedNop_rm64_r64_0F1D);
			var reservedNop_0F1E = new OpCodeHandler_Ev_Gv(Code.ReservedNop_rm16_r16_0F1E, Code.ReservedNop_rm32_r32_0F1E, Code.ReservedNop_rm64_r64_0F1E);
			var reservedNop_0F1F = new OpCodeHandler_Ev_Gv(Code.ReservedNop_rm16_r16_0F1F, Code.ReservedNop_rm32_r32_0F1F, Code.ReservedNop_rm64_r64_0F1F);

			var handlers_Grp_0F0D_mem = new OpCodeHandler[8] {
				new OpCodeHandler_M(Code.Prefetch_m8),
				new OpCodeHandler_M(Code.Prefetchw_m8),
				new OpCodeHandler_M(Code.Prefetchwt1_m8),
				new OpCodeHandler_M(Code.Prefetch_m8_r3),
				new OpCodeHandler_M(Code.Prefetch_m8_r4),
				new OpCodeHandler_M(Code.Prefetch_m8_r5),
				new OpCodeHandler_M(Code.Prefetch_m8_r6),
				new OpCodeHandler_M(Code.Prefetch_m8_r7),
			};
			var grp0F0D = new OpCodeHandler_RM(
				reservedNop_0F0D,
				new OpCodeHandler_RM(reservedNop_0F0D, new OpCodeHandler_Group(handlers_Grp_0F0D_mem))
			);

			var handlers_Grp_0F18_mem = new OpCodeHandler[8] {
				new OpCodeHandler_M(Code.Prefetchnta_m8),
				new OpCodeHandler_M(Code.Prefetcht0_m8),
				new OpCodeHandler_M(Code.Prefetcht1_m8),
				new OpCodeHandler_M(Code.Prefetcht2_m8),
				reservedNop_0F18,
				reservedNop_0F18,
				reservedNop_0F18,
				reservedNop_0F18,
			};
			var grp0F18 = new OpCodeHandler_ReservedNop(
				reservedNop_0F18,
				new OpCodeHandler_RM(
					reservedNop_0F18,
					new OpCodeHandler_RM(reservedNop_0F18, new OpCodeHandler_Group(handlers_Grp_0F18_mem))
				)
			);

			var handlers_Grp_0F1C_mem = new OpCodeHandler[8] {
				new OpCodeHandler_M(Code.Cldemote_m8),
				reservedNop_0F1C,
				reservedNop_0F1C,
				reservedNop_0F1C,
				reservedNop_0F1C,
				reservedNop_0F1C,
				reservedNop_0F1C,
				reservedNop_0F1C,
			};
			var grp0F1C = new OpCodeHandler_ReservedNop(
				reservedNop_0F1C,
				new OpCodeHandler_RM(
					reservedNop_0F1C,
					new OpCodeHandler_RM(reservedNop_0F1C, new OpCodeHandler_Group(handlers_Grp_0F1C_mem))
				)
			);

			var handlers_Grp_0F1E_mem = new OpCodeHandler[8] {
				reservedNop_0F1E,
				reservedNop_0F1E,
				reservedNop_0F1E,
				reservedNop_0F1E,
				reservedNop_0F1E,
				reservedNop_0F1E,
				reservedNop_0F1E,
				reservedNop_0F1E,
			};
			var handlers_Grp_0F1E_reg_lo = new OpCodeHandler[8] {
				reservedNop_0F1E,
				reservedNop_0F1E,
				reservedNop_0F1E,
				reservedNop_0F1E,
				reservedNop_0F1E,
				reservedNop_0F1E,
				reservedNop_0F1E,
				reservedNop_0F1E,
			};
			var grp0F1E_1 = new OpCodeHandler_MandatoryPrefix(
				reservedNop_0F1E,
				reservedNop_0F1E,
				new OpCodeHandler_RM(
					new OpCodeHandler_Ev_REXW(Code.Rdsspd_r32, Code.Rdsspq_r64, allowReg: true, allowMem: false),
					reservedNop_0F1E
				),
				reservedNop_0F1E
			);
			var handlers_Grp_0F1E_reg_hi = new OpCodeHandler[0x40] {
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
				grp0F1E_1,
				grp0F1E_1,
				grp0F1E_1,
				grp0F1E_1,
				grp0F1E_1,
				grp0F1E_1,
				grp0F1E_1,
				grp0F1E_1,

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
				null,
				null,
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					invalid,
					new OpCodeHandler_Simple_ModRM(Code.Endbr64),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					invalid,
					new OpCodeHandler_Simple_ModRM(Code.Endbr32),
					invalid
				),
				null,
				null,
				null,
				null,
			};
			var grp0F1E = new OpCodeHandler_ReservedNop(
				reservedNop_0F1E,
				new OpCodeHandler_RM(
					new OpCodeHandler_Group8x64(handlers_Grp_0F1E_reg_lo, handlers_Grp_0F1E_reg_hi),
					reservedNop_0F1E
				)
			);

			var handlers_Grp_0F1F = new OpCodeHandler[8] {
				new OpCodeHandler_Ev(Code.Nop_rm16, Code.Nop_rm32, Code.Nop_rm64),
				reservedNop_0F1F,
				reservedNop_0F1F,
				reservedNop_0F1F,
				reservedNop_0F1F,
				reservedNop_0F1F,
				reservedNop_0F1F,
				reservedNop_0F1F,
			};
			var grp0F1F = new OpCodeHandler_ReservedNop(
				reservedNop_0F1F,
				new OpCodeHandler_Group(handlers_Grp_0F1F)
			);

			var handlers_Grp_660F78 = new OpCodeHandler[8] {
				new OpCodeHandler_RIbIb(Register.XMM0, Code.Extrq_xmm_imm8_imm8),
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
					new OpCodeHandler_P_Q(Code.Pshufb_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Pshufb_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Phaddw_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Phaddw_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Phaddd_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Phaddd_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Phaddsw_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Phaddsw_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pmaddubsw_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Pmaddubsw_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Phsubw_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Phsubw_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Phsubd_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Phsubd_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Phsubsw_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Phsubsw_xmm_xmmm128),
					invalid,
					invalid
				),

				// 08
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Psignb_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Psignb_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Psignw_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Psignw_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Psignd_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Psignd_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pmulhrsw_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Pmulhrsw_xmm_xmmm128),
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
					new OpCodeHandler_VW(Register.XMM0, Code.Pblendvb_xmm_xmmm128),
					invalid,
					invalid
				),
				invalid,
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Blendvps_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Blendvpd_xmm_xmmm128),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Ptest_xmm_xmmm128),
					invalid,
					invalid
				),

				// 18
				invalid,
				invalid,
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pabsb_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Pabsb_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pabsw_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Pabsw_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pabsd_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Pabsd_xmm_xmmm128),
					invalid,
					invalid
				),
				invalid,

				// 20
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pmovsxbw_xmm_xmmm64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pmovsxbd_xmm_xmmm32),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pmovsxbq_xmm_xmmm16),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pmovsxwd_xmm_xmmm64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pmovsxwq_xmm_xmmm32),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pmovsxdq_xmm_xmmm64),
					invalid,
					invalid
				),
				invalid,
				invalid,

				// 28
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pmuldq_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pcmpeqq_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VM(Register.XMM0, Code.Movntdqa_xmm_m128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Packusdw_xmm_xmmm128),
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
					new OpCodeHandler_VW(Register.XMM0, Code.Pmovzxbw_xmm_xmmm64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pmovzxbd_xmm_xmmm32),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pmovzxbq_xmm_xmmm16),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pmovzxwd_xmm_xmmm64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pmovzxwq_xmm_xmmm32),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pmovzxdq_xmm_xmmm64),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pcmpgtq_xmm_xmmm128),
					invalid,
					invalid
				),

				// 38
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pminsb_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pminsd_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pminuw_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pminud_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pmaxsb_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pmaxsd_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pmaxuw_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pmaxud_xmm_xmmm128),
					invalid,
					invalid
				),

				// 40
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Pmulld_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Phminposuw_xmm_xmmm128),
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
					new OpCodeHandler_Gv_Ev_32_64(Code.Invept_r32_m128, Code.Invept_r64_m128, allowReg: false, allowMem: true),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_Gv_Ev_32_64(Code.Invvpid_r32_m128, Code.Invvpid_r64_m128, allowReg: false, allowMem: true),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_Gv_Ev_32_64(Code.Invpcid_r32_m128, Code.Invpcid_r64_m128, allowReg: false, allowMem: true),
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
					new OpCodeHandler_VW(Register.XMM0, Code.Sha1nexte_xmm_xmmm128),
					invalid,
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Sha1msg1_xmm_xmmm128),
					invalid,
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Sha1msg2_xmm_xmmm128),
					invalid,
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Sha256rnds2_xmm_xmmm128),
					invalid,
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Sha256msg1_xmm_xmmm128),
					invalid,
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Sha256msg2_xmm_xmmm128),
					invalid,
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Gf2p8mulb_xmm_xmmm128),
					invalid,
					invalid
				),

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
					new OpCodeHandler_VW(Register.XMM0, Code.Aesimc_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Aesenc_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Aesenclast_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Aesdec_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Aesdeclast_xmm_xmmm128),
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
					new OpCodeHandler_Gv_Mv(Code.Movbe_r16_m16, Code.Movbe_r32_m32, Code.Movbe_r64_m64),
					invalid,
					new OpCodeHandler_Gv_Eb_REX(Code.Crc32_r32_rm8, Code.Crc32_r64_rm8)
				),
				new OpCodeHandler_MandatoryPrefix_F3_F2(
					new OpCodeHandler_Mv_Gv(Code.Movbe_m16_r16, Code.Movbe_m32_r32, Code.Movbe_m64_r64),
					invalid,
					new OpCodeHandler_Gdq_Ev(Code.Crc32_r32_rm16, Code.Crc32_r32_rm32, Code.Crc32_r64_rm64)
				),
				invalid,
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_RM(
						invalid,
						new OpCodeHandler_Ev_Gv_REX(Code.Wrussd_m32_r32, Code.Wrussq_m64_r64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_RM(
						invalid,
						new OpCodeHandler_Ev_Gv_REX(Code.Wrssd_m32_r32, Code.Wrssq_m64_r64)
					),
					new OpCodeHandler_Gv_Ev_REX(Code.Adcx_r32_rm32, Code.Adcx_r64_rm64),
					new OpCodeHandler_Gv_Ev_REX(Code.Adox_r32_rm32, Code.Adox_r64_rm64),
					invalid
				),
				invalid,

				// F8
				new OpCodeHandler_Gv_M_as(Code.Movdir64b_r16_m512, Code.Movdir64b_r32_m512, Code.Movdir64b_r64_m512),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_RM(
						invalid,
						new OpCodeHandler_Ev_Gv_REX(Code.Movdiri_m32_r32, Code.Movdiri_m64_r64)
					),
					invalid,
					invalid,
					invalid
				),
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
					new OpCodeHandler_VWIb(Register.XMM0, Code.Roundps_xmm_xmmm128_imm8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VWIb(Register.XMM0, Code.Roundpd_xmm_xmmm128_imm8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VWIb(Register.XMM0, Code.Roundss_xmm_xmmm32_imm8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VWIb(Register.XMM0, Code.Roundsd_xmm_xmmm64_imm8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VWIb(Register.XMM0, Code.Blendps_xmm_xmmm128_imm8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VWIb(Register.XMM0, Code.Blendpd_xmm_xmmm128_imm8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VWIb(Register.XMM0, Code.Pblendw_xmm_xmmm128_imm8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q_Ib(Code.Palignr_mm_mmm64_imm8),
					new OpCodeHandler_VWIb(Register.XMM0, Code.Palignr_xmm_xmmm128_imm8),
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
					new OpCodeHandler_GvM_VX_Ib(Register.XMM0, Code.Pextrb_r32m8_xmm_imm8, Code.Pextrb_r64m8_xmm_imm8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_GvM_VX_Ib(Register.XMM0, Code.Pextrw_r32m16_xmm_imm8, Code.Pextrw_r64m16_xmm_imm8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_GvM_VX_Ib(Register.XMM0, Code.Pextrd_rm32_xmm_imm8, Code.Pextrq_rm64_xmm_imm8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_Ed_V_Ib(Register.XMM0, Code.Extractps_rm32_xmm_imm8, Code.Extractps_rm64_xmm_imm8),
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
					new OpCodeHandler_VX_E_Ib(Register.XMM0, Code.Pinsrb_xmm_r32m8_imm8, Code.Pinsrb_xmm_r64m8_imm8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VWIb(Register.XMM0, Code.Insertps_xmm_xmmm32_imm8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VX_E_Ib(Register.XMM0, Code.Pinsrd_xmm_rm32_imm8, Code.Pinsrq_xmm_rm64_imm8),
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
					new OpCodeHandler_VWIb(Register.XMM0, Code.Dpps_xmm_xmmm128_imm8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VWIb(Register.XMM0, Code.Dppd_xmm_xmmm128_imm8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VWIb(Register.XMM0, Code.Mpsadbw_xmm_xmmm128_imm8),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VWIb(Register.XMM0, Code.Pclmulqdq_xmm_xmmm128_imm8),
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
					new OpCodeHandler_VWIb(Register.XMM0, Code.Pcmpestrm_xmm_xmmm128_imm8, Code.Pcmpestrm64_xmm_xmmm128_imm8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VWIb(Register.XMM0, Code.Pcmpestri_xmm_xmmm128_imm8, Code.Pcmpestri64_xmm_xmmm128_imm8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VWIb(Register.XMM0, Code.Pcmpistrm_xmm_xmmm128_imm8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VWIb(Register.XMM0, Code.Pcmpistri_xmm_xmmm128_imm8),
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
					new OpCodeHandler_VWIb(Register.XMM0, Code.Sha1rnds4_xmm_xmmm128_imm8),
					invalid,
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VWIb(Register.XMM0, Code.Gf2p8affineqb_xmm_xmmm128_imm8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VWIb(Register.XMM0, Code.Gf2p8affineinvqb_xmm_xmmm128_imm8),
					invalid,
					invalid
				),

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
					new OpCodeHandler_VWIb(Register.XMM0, Code.Aeskeygenassist_xmm_xmmm128_imm8),
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
				new OpCodeHandler_Gv_Ev3(Code.Lar_r16_rm16, Code.Lar_r32_rm32, Code.Lar_r64_rm64),
				new OpCodeHandler_Gv_Ev3(Code.Lsl_r16_rm16, Code.Lsl_r32_rm32, Code.Lsl_r64_rm64),
				new OpCodeHandler_Options(
					invalid,
					new OpCodeHandler_Simple(Code.Loadallreset286), DecoderOptions.Loadall286
				),
				new OpCodeHandler_Options(
					new OpCodeHandler_Simple(Code.Syscall),
					new OpCodeHandler_Simple(Code.Loadall286), DecoderOptions.Loadall286
				),
				new OpCodeHandler_Simple(Code.Clts),
				new OpCodeHandler_Options(
					new OpCodeHandler_Simple2(Code.Sysretd, Code.Sysretd, Code.Sysretq),
					new OpCodeHandler_Simple(Code.Loadall386), DecoderOptions.Loadall386
				),

				// 08
				new OpCodeHandler_Simple(Code.Invd),
				new OpCodeHandler_MandatoryPrefix_NoModRM(
					new OpCodeHandler_Simple(Code.Wbinvd),
					new OpCodeHandler_Simple(Code.Wbinvd),
					new OpCodeHandler_Simple(Code.Wbnoinvd),
					new OpCodeHandler_Simple(Code.Wbinvd)
				),
				new OpCodeHandler_Options(
					invalid,
					new OpCodeHandler_Simple(Code.Cl1invmb), DecoderOptions.Cl1invmb,
					new OpCodeHandler_Simple(Code.Cflsh), DecoderOptions.Cflsh
				),
				new OpCodeHandler_Simple(Code.Ud2),
				invalid,
				new OpCodeHandler_ReservedNop(
					reservedNop_0F0D,
					grp0F0D
				),
				new OpCodeHandler_Simple(Code.Femms),
				new OpCodeHandler_D3NOW(),

				// 10
				new OpCodeHandler_Options(
					new OpCodeHandler_MandatoryPrefix(
						new OpCodeHandler_VW(Register.XMM0, Code.Movups_xmm_xmmm128),
						new OpCodeHandler_VW(Register.XMM0, Code.Movupd_xmm_xmmm128),
						new OpCodeHandler_VW(Register.XMM0, Code.Movss_xmm_xmmm32),
						new OpCodeHandler_VW(Register.XMM0, Code.Movsd_xmm_xmmm64)
					),
					new OpCodeHandler_Eb_Gb(Code.Umov_rm8_r8), DecoderOptions.Umov
				),
				new OpCodeHandler_Options(
					new OpCodeHandler_MandatoryPrefix(
						new OpCodeHandler_WV(Register.XMM0, Code.Movups_xmmm128_xmm),
						new OpCodeHandler_WV(Register.XMM0, Code.Movupd_xmmm128_xmm),
						new OpCodeHandler_WV(Register.XMM0, Code.Movss_xmmm32_xmm),
						new OpCodeHandler_WV(Register.XMM0, Code.Movsd_xmmm64_xmm)
					),
					new OpCodeHandler_Ev_Gv(Code.Umov_rm16_r16, Code.Umov_rm32_r32, Code.INVALID), DecoderOptions.Umov
				),
				new OpCodeHandler_Options(
					new OpCodeHandler_MandatoryPrefix(
						new OpCodeHandler_VW(Register.XMM0, Code.Movhlps_xmm_xmm, Code.Movlps_xmm_m64),
						new OpCodeHandler_VM(Register.XMM0, Code.Movlpd_xmm_m64),
						new OpCodeHandler_VW(Register.XMM0, Code.Movsldup_xmm_xmmm128),
						new OpCodeHandler_VW(Register.XMM0, Code.Movddup_xmm_xmmm64)
					),
					new OpCodeHandler_Gb_Eb(Code.Umov_r8_rm8), DecoderOptions.Umov
				),
				new OpCodeHandler_Options(
					new OpCodeHandler_MandatoryPrefix(
						new OpCodeHandler_MV(Register.XMM0, Code.Movlps_m64_xmm),
						new OpCodeHandler_MV(Register.XMM0, Code.Movlpd_m64_xmm),
						invalid,
						invalid
					),
					new OpCodeHandler_Gv_Ev(Code.Umov_r16_rm16, Code.Umov_r32_rm32, Code.INVALID), DecoderOptions.Umov
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Unpcklps_xmm_xmmm128),
					new OpCodeHandler_VW(Register.XMM0, Code.Unpcklpd_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Unpckhps_xmm_xmmm128),
					new OpCodeHandler_VW(Register.XMM0, Code.Unpckhpd_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Movlhps_xmm_xmm, Code.Movhps_xmm_m64),
					new OpCodeHandler_VM(Register.XMM0, Code.Movhpd_xmm_m64),
					new OpCodeHandler_VW(Register.XMM0, Code.Movshdup_xmm_xmmm128),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_MV(Register.XMM0, Code.Movhps_m64_xmm),
					new OpCodeHandler_MV(Register.XMM0, Code.Movhpd_m64_xmm),
					invalid,
					invalid
				),

				// 18
				grp0F18,
				reservedNop_0F19,
				new OpCodeHandler_ReservedNop(
					reservedNop_0F1A,
					new OpCodeHandler_MandatoryPrefix(
						new OpCodeHandler_RM(
							reservedNop_0F1A,
							new OpCodeHandler_B_MIB(Code.Bndldx_bnd_mib)
						),
						new OpCodeHandler_B_BM(Code.Bndmov_bnd_bndm64, Code.Bndmov_bnd_bndm128),
						new OpCodeHandler_B_Ev(Code.Bndcl_bnd_rm32, Code.Bndcl_bnd_rm64),
						new OpCodeHandler_B_Ev(Code.Bndcu_bnd_rm32, Code.Bndcu_bnd_rm64)
					)
				),
				new OpCodeHandler_ReservedNop(
					reservedNop_0F1B,
					new OpCodeHandler_MandatoryPrefix(
						new OpCodeHandler_RM(
							reservedNop_0F1B,
							new OpCodeHandler_MIB_B(Code.Bndstx_mib_bnd)
						),
						new OpCodeHandler_BM_B(Code.Bndmov_bndm64_bnd, Code.Bndmov_bndm128_bnd),
						new OpCodeHandler_RM(
							reservedNop_0F1B,
							new OpCodeHandler_B_Ev(Code.Bndmk_bnd_m32, Code.Bndmk_bnd_m64)
						),
						new OpCodeHandler_B_Ev(Code.Bndcn_bnd_rm32, Code.Bndcn_bnd_rm64)
					)
				),
				grp0F1C,
				reservedNop_0F1D,
				grp0F1E,
				grp0F1F,

				// 20
				new OpCodeHandler_Rd_Cd(Code.Mov_r32_cr, Register.CR0),
				new OpCodeHandler_Rd_Cd(Code.Mov_r32_dr, Register.DR0),
				new OpCodeHandler_Cd_Rd(Code.Mov_cr_r32, Register.CR0),
				new OpCodeHandler_Cd_Rd(Code.Mov_dr_r32, Register.DR0),
				new OpCodeHandler_Options(
					invalid,
					new OpCodeHandler_Rd_Cd(Code.Mov_r32_tr, Register.TR0), DecoderOptions.MovTr
				),
				invalid,
				new OpCodeHandler_Options(
					invalid,
					new OpCodeHandler_Cd_Rd(Code.Mov_tr_r32, Register.TR0), DecoderOptions.MovTr
				),
				invalid,

				// 28
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Movaps_xmm_xmmm128),
					new OpCodeHandler_VW(Register.XMM0, Code.Movapd_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_WV(Register.XMM0, Code.Movaps_xmmm128_xmm),
					new OpCodeHandler_WV(Register.XMM0, Code.Movapd_xmmm128_xmm),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VQ(Register.XMM0, Code.Cvtpi2ps_xmm_mmm64),
					new OpCodeHandler_VQ(Register.XMM0, Code.Cvtpi2pd_xmm_mmm64),
					new OpCodeHandler_V_Ev(Register.XMM0, Code.Cvtsi2ss_xmm_rm32, Code.Cvtsi2ss_xmm_rm64),
					new OpCodeHandler_V_Ev(Register.XMM0, Code.Cvtsi2sd_xmm_rm32, Code.Cvtsi2sd_xmm_rm64)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_MV(Register.XMM0, Code.Movntps_m128_xmm),
					new OpCodeHandler_MV(Register.XMM0, Code.Movntpd_m128_xmm),
					new OpCodeHandler_MV(Register.XMM0, Code.Movntss_m32_xmm),
					new OpCodeHandler_MV(Register.XMM0, Code.Movntsd_m64_xmm)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_W(Register.XMM0, Code.Cvttps2pi_mm_xmmm64),
					new OpCodeHandler_P_W(Register.XMM0, Code.Cvttpd2pi_mm_xmmm128),
					new OpCodeHandler_Gv_W(Register.XMM0, Code.Cvttss2si_r32_xmmm32, Code.Cvttss2si_r64_xmmm32),
					new OpCodeHandler_Gv_W(Register.XMM0, Code.Cvttsd2si_r32_xmmm64, Code.Cvttsd2si_r64_xmmm64)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_W(Register.XMM0, Code.Cvtps2pi_mm_xmmm64),
					new OpCodeHandler_P_W(Register.XMM0, Code.Cvtpd2pi_mm_xmmm128),
					new OpCodeHandler_Gv_W(Register.XMM0, Code.Cvtss2si_r32_xmmm32, Code.Cvtss2si_r64_xmmm32),
					new OpCodeHandler_Gv_W(Register.XMM0, Code.Cvtsd2si_r32_xmmm64, Code.Cvtsd2si_r64_xmmm64)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Ucomiss_xmm_xmmm32),
					new OpCodeHandler_VW(Register.XMM0, Code.Ucomisd_xmm_xmmm64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Comiss_xmm_xmmm32),
					new OpCodeHandler_VW(Register.XMM0, Code.Comisd_xmm_xmmm64),
					invalid,
					invalid
				),

				// 30
				new OpCodeHandler_Simple(Code.Wrmsr),
				new OpCodeHandler_Simple(Code.Rdtsc),
				new OpCodeHandler_Simple(Code.Rdmsr),
				new OpCodeHandler_Simple(Code.Rdpmc),
				new OpCodeHandler_Options(
					new OpCodeHandler_Simple(Code.Sysenter),
					new OpCodeHandler_Simple(Code.Wrecr), DecoderOptions.Ecr
				),
				new OpCodeHandler_Simple4(Code.Sysexitd, Code.Sysexitq),
				new OpCodeHandler_Options(
					invalid,
					new OpCodeHandler_Simple(Code.Rdecr), DecoderOptions.Ecr
				),
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
				new OpCodeHandler_Gv_Ev(Code.Cmovo_r16_rm16, Code.Cmovo_r32_rm32, Code.Cmovo_r64_rm64),
				new OpCodeHandler_Gv_Ev(Code.Cmovno_r16_rm16, Code.Cmovno_r32_rm32, Code.Cmovno_r64_rm64),
				new OpCodeHandler_Gv_Ev(Code.Cmovb_r16_rm16, Code.Cmovb_r32_rm32, Code.Cmovb_r64_rm64),
				new OpCodeHandler_Gv_Ev(Code.Cmovae_r16_rm16, Code.Cmovae_r32_rm32, Code.Cmovae_r64_rm64),
				new OpCodeHandler_Gv_Ev(Code.Cmove_r16_rm16, Code.Cmove_r32_rm32, Code.Cmove_r64_rm64),
				new OpCodeHandler_Gv_Ev(Code.Cmovne_r16_rm16, Code.Cmovne_r32_rm32, Code.Cmovne_r64_rm64),
				new OpCodeHandler_Gv_Ev(Code.Cmovbe_r16_rm16, Code.Cmovbe_r32_rm32, Code.Cmovbe_r64_rm64),
				new OpCodeHandler_Gv_Ev(Code.Cmova_r16_rm16, Code.Cmova_r32_rm32, Code.Cmova_r64_rm64),

				// 48
				new OpCodeHandler_Gv_Ev(Code.Cmovs_r16_rm16, Code.Cmovs_r32_rm32, Code.Cmovs_r64_rm64),
				new OpCodeHandler_Gv_Ev(Code.Cmovns_r16_rm16, Code.Cmovns_r32_rm32, Code.Cmovns_r64_rm64),
				new OpCodeHandler_Gv_Ev(Code.Cmovp_r16_rm16, Code.Cmovp_r32_rm32, Code.Cmovp_r64_rm64),
				new OpCodeHandler_Gv_Ev(Code.Cmovnp_r16_rm16, Code.Cmovnp_r32_rm32, Code.Cmovnp_r64_rm64),
				new OpCodeHandler_Gv_Ev(Code.Cmovl_r16_rm16, Code.Cmovl_r32_rm32, Code.Cmovl_r64_rm64),
				new OpCodeHandler_Gv_Ev(Code.Cmovge_r16_rm16, Code.Cmovge_r32_rm32, Code.Cmovge_r64_rm64),
				new OpCodeHandler_Gv_Ev(Code.Cmovle_r16_rm16, Code.Cmovle_r32_rm32, Code.Cmovle_r64_rm64),
				new OpCodeHandler_Gv_Ev(Code.Cmovg_r16_rm16, Code.Cmovg_r32_rm32, Code.Cmovg_r64_rm64),

				// 50
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Gv_RX(Register.XMM0, Code.Movmskps_r32_xmm, Code.Movmskps_r64_xmm),
					new OpCodeHandler_Gv_RX(Register.XMM0, Code.Movmskpd_r32_xmm, Code.Movmskpd_r64_xmm),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Sqrtps_xmm_xmmm128),
					new OpCodeHandler_VW(Register.XMM0, Code.Sqrtpd_xmm_xmmm128),
					new OpCodeHandler_VW(Register.XMM0, Code.Sqrtss_xmm_xmmm32),
					new OpCodeHandler_VW(Register.XMM0, Code.Sqrtsd_xmm_xmmm64)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Rsqrtps_xmm_xmmm128),
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Rsqrtss_xmm_xmmm32),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Rcpps_xmm_xmmm128),
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Rcpss_xmm_xmmm32),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Andps_xmm_xmmm128),
					new OpCodeHandler_VW(Register.XMM0, Code.Andpd_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Andnps_xmm_xmmm128),
					new OpCodeHandler_VW(Register.XMM0, Code.Andnpd_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Orps_xmm_xmmm128),
					new OpCodeHandler_VW(Register.XMM0, Code.Orpd_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Xorps_xmm_xmmm128),
					new OpCodeHandler_VW(Register.XMM0, Code.Xorpd_xmm_xmmm128),
					invalid,
					invalid
				),

				// 58
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Addps_xmm_xmmm128),
					new OpCodeHandler_VW(Register.XMM0, Code.Addpd_xmm_xmmm128),
					new OpCodeHandler_VW(Register.XMM0, Code.Addss_xmm_xmmm32),
					new OpCodeHandler_VW(Register.XMM0, Code.Addsd_xmm_xmmm64)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Mulps_xmm_xmmm128),
					new OpCodeHandler_VW(Register.XMM0, Code.Mulpd_xmm_xmmm128),
					new OpCodeHandler_VW(Register.XMM0, Code.Mulss_xmm_xmmm32),
					new OpCodeHandler_VW(Register.XMM0, Code.Mulsd_xmm_xmmm64)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Cvtps2pd_xmm_xmmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Cvtpd2ps_xmm_xmmm128),
					new OpCodeHandler_VW(Register.XMM0, Code.Cvtss2sd_xmm_xmmm32),
					new OpCodeHandler_VW(Register.XMM0, Code.Cvtsd2ss_xmm_xmmm64)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Cvtdq2ps_xmm_xmmm128),
					new OpCodeHandler_VW(Register.XMM0, Code.Cvtps2dq_xmm_xmmm128),
					new OpCodeHandler_VW(Register.XMM0, Code.Cvttps2dq_xmm_xmmm128),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Subps_xmm_xmmm128),
					new OpCodeHandler_VW(Register.XMM0, Code.Subpd_xmm_xmmm128),
					new OpCodeHandler_VW(Register.XMM0, Code.Subss_xmm_xmmm32),
					new OpCodeHandler_VW(Register.XMM0, Code.Subsd_xmm_xmmm64)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Minps_xmm_xmmm128),
					new OpCodeHandler_VW(Register.XMM0, Code.Minpd_xmm_xmmm128),
					new OpCodeHandler_VW(Register.XMM0, Code.Minss_xmm_xmmm32),
					new OpCodeHandler_VW(Register.XMM0, Code.Minsd_xmm_xmmm64)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Divps_xmm_xmmm128),
					new OpCodeHandler_VW(Register.XMM0, Code.Divpd_xmm_xmmm128),
					new OpCodeHandler_VW(Register.XMM0, Code.Divss_xmm_xmmm32),
					new OpCodeHandler_VW(Register.XMM0, Code.Divsd_xmm_xmmm64)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VW(Register.XMM0, Code.Maxps_xmm_xmmm128),
					new OpCodeHandler_VW(Register.XMM0, Code.Maxpd_xmm_xmmm128),
					new OpCodeHandler_VW(Register.XMM0, Code.Maxss_xmm_xmmm32),
					new OpCodeHandler_VW(Register.XMM0, Code.Maxsd_xmm_xmmm64)
				),

				// 60
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Punpcklbw_mm_mmm32),
					new OpCodeHandler_VW(Register.XMM0, Code.Punpcklbw_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Punpcklwd_mm_mmm32),
					new OpCodeHandler_VW(Register.XMM0, Code.Punpcklwd_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Punpckldq_mm_mmm32),
					new OpCodeHandler_VW(Register.XMM0, Code.Punpckldq_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Packsswb_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Packsswb_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pcmpgtb_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Pcmpgtb_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pcmpgtw_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Pcmpgtw_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pcmpgtd_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Pcmpgtd_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Packuswb_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Packuswb_xmm_xmmm128),
					invalid,
					invalid
				),

				// 68
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Punpckhbw_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Punpckhbw_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Punpckhwd_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Punpckhwd_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Punpckhdq_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Punpckhdq_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Packssdw_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Packssdw_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Punpcklqdq_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Punpckhqdq_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Ev(Code.Movd_mm_rm32, Code.Movq_mm_rm64),
					new OpCodeHandler_VX_Ev(Code.Movd_xmm_rm32, Code.Movq_xmm_rm64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Movq_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Movdqa_xmm_xmmm128),
					new OpCodeHandler_VW(Register.XMM0, Code.Movdqu_xmm_xmmm128),
					invalid
				),

				// 70
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q_Ib(Code.Pshufw_mm_mmm64_imm8),
					new OpCodeHandler_VWIb(Register.XMM0, Code.Pshufd_xmm_xmmm128_imm8),
					new OpCodeHandler_VWIb(Register.XMM0, Code.Pshufhw_xmm_xmmm128_imm8),
					new OpCodeHandler_VWIb(Register.XMM0, Code.Pshuflw_xmm_xmmm128_imm8)
				),
				new OpCodeHandler_Group(handlers_Grp_0F71),
				new OpCodeHandler_Group(handlers_Grp_0F72),
				new OpCodeHandler_Group(handlers_Grp_0F73),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pcmpeqb_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Pcmpeqb_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pcmpeqw_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Pcmpeqw_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pcmpeqd_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Pcmpeqd_xmm_xmmm128),
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
					new OpCodeHandler_Ev_Gv_32_64(Code.Vmread_rm32_r32, Code.Vmread_rm64_r64),
					new OpCodeHandler_Group(handlers_Grp_660F78),
					invalid,
					new OpCodeHandler_VRIbIb(Register.XMM0, Code.Insertq_xmm_xmm_imm8_imm8)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Gv_Ev_32_64(Code.Vmwrite_r32_rm32, Code.Vmwrite_r64_rm64, allowReg: true, allowMem: true),
					new OpCodeHandler_VW(Register.XMM0, Code.Extrq_xmm_xmm, Code.INVALID),
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Insertq_xmm_xmm, Code.INVALID)
				),
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Haddpd_xmm_xmmm128),
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Haddps_xmm_xmmm128)
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Hsubpd_xmm_xmmm128),
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Hsubps_xmm_xmmm128)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Ev_P(Code.Movd_rm32_mm, Code.Movq_rm64_mm),
					new OpCodeHandler_Ev_VX(Code.Movd_rm32_xmm, Code.Movq_rm64_xmm),
					new OpCodeHandler_VW(Register.XMM0, Code.Movq_xmm_xmmm64),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Q_P(Code.Movq_mmm64_mm),
					new OpCodeHandler_WV(Register.XMM0, Code.Movdqa_xmmm128_xmm),
					new OpCodeHandler_WV(Register.XMM0, Code.Movdqu_xmmm128_xmm),
					invalid
				),

				// 80
				new OpCodeHandler_Jz(Code.Jo_rel16, Code.Jo_rel32_32),
				new OpCodeHandler_Jz(Code.Jno_rel16, Code.Jno_rel32_32),
				new OpCodeHandler_Jz(Code.Jb_rel16, Code.Jb_rel32_32),
				new OpCodeHandler_Jz(Code.Jae_rel16, Code.Jae_rel32_32),
				new OpCodeHandler_Jz(Code.Je_rel16, Code.Je_rel32_32),
				new OpCodeHandler_Jz(Code.Jne_rel16, Code.Jne_rel32_32),
				new OpCodeHandler_Jz(Code.Jbe_rel16, Code.Jbe_rel32_32),
				new OpCodeHandler_Jz(Code.Ja_rel16, Code.Ja_rel32_32),

				// 88
				new OpCodeHandler_Jz(Code.Js_rel16, Code.Js_rel32_32),
				new OpCodeHandler_Jz(Code.Jns_rel16, Code.Jns_rel32_32),
				new OpCodeHandler_Jz(Code.Jp_rel16, Code.Jp_rel32_32),
				new OpCodeHandler_Jz(Code.Jnp_rel16, Code.Jnp_rel32_32),
				new OpCodeHandler_Jz(Code.Jl_rel16, Code.Jl_rel32_32),
				new OpCodeHandler_Jz(Code.Jge_rel16, Code.Jge_rel32_32),
				new OpCodeHandler_Jz(Code.Jle_rel16, Code.Jle_rel32_32),
				new OpCodeHandler_Jz(Code.Jg_rel16, Code.Jg_rel32_32),

				// 90
				new OpCodeHandler_Eb(Code.Seto_rm8),
				new OpCodeHandler_Eb(Code.Setno_rm8),
				new OpCodeHandler_Eb(Code.Setb_rm8),
				new OpCodeHandler_Eb(Code.Setae_rm8),
				new OpCodeHandler_Eb(Code.Sete_rm8),
				new OpCodeHandler_Eb(Code.Setne_rm8),
				new OpCodeHandler_Eb(Code.Setbe_rm8),
				new OpCodeHandler_Eb(Code.Seta_rm8),

				// 98
				new OpCodeHandler_Eb(Code.Sets_rm8),
				new OpCodeHandler_Eb(Code.Setns_rm8),
				new OpCodeHandler_Eb(Code.Setp_rm8),
				new OpCodeHandler_Eb(Code.Setnp_rm8),
				new OpCodeHandler_Eb(Code.Setl_rm8),
				new OpCodeHandler_Eb(Code.Setge_rm8),
				new OpCodeHandler_Eb(Code.Setle_rm8),
				new OpCodeHandler_Eb(Code.Setg_rm8),

				// A0
				new OpCodeHandler_OpSizeReg(Code.Pushw_FS, Code.Pushd_FS, Register.FS),
				new OpCodeHandler_OpSizeReg(Code.Popw_FS, Code.Popd_FS, Register.FS),
				new OpCodeHandler_Simple(Code.Cpuid),
				new OpCodeHandler_Ev_Gv(Code.Bt_rm16_r16, Code.Bt_rm32_r32, Code.Bt_rm64_r64),
				new OpCodeHandler_Ev_Gv_Ib(Code.Shld_rm16_r16_imm8, Code.Shld_rm32_r32_imm8, Code.Shld_rm64_r64_imm8),
				new OpCodeHandler_Ev_Gv_CL(Code.Shld_rm16_r16_CL, Code.Shld_rm32_r32_CL, Code.Shld_rm64_r64_CL),
				new OpCodeHandler_Options(
					invalid,
					new OpCodeHandler_Gv_Ev(Code.Xbts_r16_rm16, Code.Xbts_r32_rm32, Code.INVALID), DecoderOptions.Xbts,
					new OpCodeHandler_Eb_Gb(Code.Cmpxchg486_rm8_r8), DecoderOptions.Cmpxchg486A
				),
				new OpCodeHandler_Options(
					invalid,
					new OpCodeHandler_Ev_Gv(Code.Ibts_rm16_r16, Code.Ibts_rm32_r32, Code.INVALID), DecoderOptions.Xbts,
					new OpCodeHandler_Ev_Gv(Code.Cmpxchg486_rm16_r16, Code.Cmpxchg486_rm32_r32, Code.INVALID), DecoderOptions.Cmpxchg486A
				),

				// A8
				new OpCodeHandler_OpSizeReg(Code.Pushw_GS, Code.Pushd_GS, Register.GS),
				new OpCodeHandler_OpSizeReg(Code.Popw_GS, Code.Popd_GS, Register.GS),
				new OpCodeHandler_Simple(Code.Rsm),
				new OpCodeHandler_Ev_Gv(Code.Bts_rm16_r16, Code.Bts_rm32_r32, Code.Bts_rm64_r64, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Gv_Ib(Code.Shrd_rm16_r16_imm8, Code.Shrd_rm32_r32_imm8, Code.Shrd_rm64_r64_imm8),
				new OpCodeHandler_Ev_Gv_CL(Code.Shrd_rm16_r16_CL, Code.Shrd_rm32_r32_CL, Code.Shrd_rm64_r64_CL),
				new OpCodeHandler_Group8x64(handlers_Grp_0FAE_lo, handlers_Grp_0FAE_hi),
				new OpCodeHandler_Gv_Ev(Code.Imul_r16_rm16, Code.Imul_r32_rm32, Code.Imul_r64_rm64),

				// B0
				new OpCodeHandler_Eb_Gb(Code.Cmpxchg_rm8_r8, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Gv(Code.Cmpxchg_rm16_r16, Code.Cmpxchg_rm32_r32, Code.Cmpxchg_rm64_r64, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Gv_Mp(Code.Lss_r16_m32, Code.Lss_r32_m48, Code.Lss_r64_m80),
				new OpCodeHandler_Ev_Gv(Code.Btr_rm16_r16, Code.Btr_rm32_r32, Code.Btr_rm64_r64, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Gv_Mp(Code.Lfs_r16_m32, Code.Lfs_r32_m48, Code.Lfs_r64_m80),
				new OpCodeHandler_Gv_Mp(Code.Lgs_r16_m32, Code.Lgs_r32_m48, Code.Lgs_r64_m80),
				new OpCodeHandler_Gv_Eb(Code.Movzx_r16_rm8, Code.Movzx_r32_rm8, Code.Movzx_r64_rm8),
				new OpCodeHandler_Gv_Ew(Code.Movzx_r16_rm16, Code.Movzx_r32_rm16, Code.Movzx_r64_rm16),

				// B8
				new OpCodeHandler_MandatoryPrefix_F3_F2(
					new OpCodeHandler_Jdisp(Code.Jmpe_disp16, Code.Jmpe_disp32),
					new OpCodeHandler_Gv_Ev(Code.Popcnt_r16_rm16, Code.Popcnt_r32_rm32, Code.Popcnt_r64_rm64),
					invalid
				),
				new OpCodeHandler_Gv_Ev(Code.Ud1_r16_rm16, Code.Ud1_r32_rm32, Code.Ud1_r64_rm64),
				new OpCodeHandler_Group(handlers_Grp_0FBA),
				new OpCodeHandler_Ev_Gv(Code.Btc_rm16_r16, Code.Btc_rm32_r32, Code.Btc_rm64_r64, HandlerFlags.XacquireRelease),
				new OpCodeHandler_MandatoryPrefix_F3_F2(
					new OpCodeHandler_Gv_Ev(Code.Bsf_r16_rm16, Code.Bsf_r32_rm32, Code.Bsf_r64_rm64),
					new OpCodeHandler_Gv_Ev(Code.Tzcnt_r16_rm16, Code.Tzcnt_r32_rm32, Code.Tzcnt_r64_rm64),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix_F3_F2(
					new OpCodeHandler_Gv_Ev(Code.Bsr_r16_rm16, Code.Bsr_r32_rm32, Code.Bsr_r64_rm64),
					new OpCodeHandler_Gv_Ev(Code.Lzcnt_r16_rm16, Code.Lzcnt_r32_rm32, Code.Lzcnt_r64_rm64),
					invalid
				),
				new OpCodeHandler_Gv_Eb(Code.Movsx_r16_rm8, Code.Movsx_r32_rm8, Code.Movsx_r64_rm8),
				new OpCodeHandler_Gv_Ew(Code.Movsx_r16_rm16, Code.Movsx_r32_rm16, Code.Movsx_r64_rm16),

				// C0
				new OpCodeHandler_Eb_Gb(Code.Xadd_rm8_r8, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Gv(Code.Xadd_rm16_r16, Code.Xadd_rm32_r32, Code.Xadd_rm64_r64, HandlerFlags.XacquireRelease),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VWIb(Register.XMM0, Code.Cmpps_xmm_xmmm128_imm8),
					new OpCodeHandler_VWIb(Register.XMM0, Code.Cmppd_xmm_xmmm128_imm8),
					new OpCodeHandler_VWIb(Register.XMM0, Code.Cmpss_xmm_xmmm32_imm8),
					new OpCodeHandler_VWIb(Register.XMM0, Code.Cmpsd_xmm_xmmm64_imm8)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Mv_Gv_REXW(Code.Movnti_m32_r32, Code.Movnti_m64_r64),
					invalid,
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Ev_Ib(Code.Pinsrw_mm_r32m16_imm8, Code.Pinsrw_mm_r64m16_imm8),
					new OpCodeHandler_VX_E_Ib(Register.XMM0, Code.Pinsrw_xmm_r32m16_imm8, Code.Pinsrw_xmm_r64m16_imm8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Gv_N_Ib_REX(Code.Pextrw_r32_mm_imm8, Code.Pextrw_r64_mm_imm8),
					new OpCodeHandler_Gv_Ev_Ib_REX(Register.XMM0, Code.Pextrw_r32_xmm_imm8, Code.Pextrw_r64_xmm_imm8, allowMem: false),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_VWIb(Register.XMM0, Code.Shufps_xmm_xmmm128_imm8),
					new OpCodeHandler_VWIb(Register.XMM0, Code.Shufpd_xmm_xmmm128_imm8),
					invalid,
					invalid
				),
				new OpCodeHandler_Group(handlers_Grp_0FC7),

				// C8
				new OpCodeHandler_SimpleReg(Code.Bswap_r16, Code.Bswap_r32, Register.AX, Register.EAX),
				new OpCodeHandler_SimpleReg(Code.Bswap_r16, Code.Bswap_r32, Register.CX, Register.ECX),
				new OpCodeHandler_SimpleReg(Code.Bswap_r16, Code.Bswap_r32, Register.DX, Register.EDX),
				new OpCodeHandler_SimpleReg(Code.Bswap_r16, Code.Bswap_r32, Register.BX, Register.EBX),
				new OpCodeHandler_SimpleReg(Code.Bswap_r16, Code.Bswap_r32, Register.SP, Register.ESP),
				new OpCodeHandler_SimpleReg(Code.Bswap_r16, Code.Bswap_r32, Register.BP, Register.EBP),
				new OpCodeHandler_SimpleReg(Code.Bswap_r16, Code.Bswap_r32, Register.SI, Register.ESI),
				new OpCodeHandler_SimpleReg(Code.Bswap_r16, Code.Bswap_r32, Register.DI, Register.EDI),

				// D0
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Addsubpd_xmm_xmmm128),
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Addsubps_xmm_xmmm128)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Psrlw_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Psrlw_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Psrld_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Psrld_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Psrlq_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Psrlq_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Paddq_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Paddq_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pmullw_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Pmullw_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_WV(Register.XMM0, Code.Movq_xmmm64_xmm),
					new OpCodeHandler_VN(Register.XMM0, Code.Movq2dq_xmm_mm),
					new OpCodeHandler_P_R(Register.XMM0, Code.Movdq2q_mm_xmm)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_Gv_N(Code.Pmovmskb_r32_mm, Code.Pmovmskb_r64_mm),
					new OpCodeHandler_Gv_RX(Register.XMM0, Code.Pmovmskb_r32_xmm, Code.Pmovmskb_r64_xmm),
					invalid,
					invalid
				),

				// D8
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Psubusb_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Psubusb_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Psubusw_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Psubusw_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pminub_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Pminub_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pand_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Pand_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Paddusb_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Paddusb_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Paddusw_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Paddusw_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pmaxub_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Pmaxub_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pandn_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Pandn_xmm_xmmm128),
					invalid,
					invalid
				),

				// E0
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pavgb_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Pavgb_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Psraw_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Psraw_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Psrad_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Psrad_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pavgw_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Pavgw_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pmulhuw_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Pmulhuw_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pmulhw_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Pmulhw_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					new OpCodeHandler_VW(Register.XMM0, Code.Cvttpd2dq_xmm_xmmm128),
					new OpCodeHandler_VW(Register.XMM0, Code.Cvtdq2pd_xmm_xmmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Cvtpd2dq_xmm_xmmm128)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_MP(Code.Movntq_m64_mm),
					new OpCodeHandler_MV(Register.XMM0, Code.Movntdq_m128_xmm),
					invalid,
					invalid
				),

				// E8
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Psubsb_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Psubsb_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Psubsw_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Psubsw_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pminsw_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Pminsw_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Por_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Por_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Paddsb_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Paddsb_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Paddsw_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Paddsw_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pmaxsw_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Pmaxsw_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pxor_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Pxor_xmm_xmmm128),
					invalid,
					invalid
				),

				// F0
				new OpCodeHandler_MandatoryPrefix(
					invalid,
					invalid,
					invalid,
					new OpCodeHandler_VM(Register.XMM0, Code.Lddqu_xmm_m128)
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Psllw_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Psllw_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pslld_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Pslld_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Psllq_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Psllq_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pmuludq_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Pmuludq_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Pmaddwd_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Pmaddwd_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Psadbw_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Psadbw_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_rDI_P_N(Code.Maskmovq_rDI_mm_mm),
					new OpCodeHandler_rDI_VX_RX(Register.XMM0, Code.Maskmovdqu_rDI_xmm_xmm),
					invalid,
					invalid
				),

				// F8
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Psubb_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Psubb_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Psubw_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Psubw_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Psubd_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Psubd_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Psubq_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Psubq_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Paddb_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Paddb_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Paddw_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Paddw_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix(
					new OpCodeHandler_P_Q(Code.Paddd_mm_mmm64),
					new OpCodeHandler_VW(Register.XMM0, Code.Paddd_xmm_xmmm128),
					invalid,
					invalid
				),
				new OpCodeHandler_Gv_Ev(Code.Ud0_r16_rm16, Code.Ud0_r32_rm32, Code.Ud0_r64_rm64),
			};

			OneByteHandlers = new OpCodeHandler[0x100] {
				// 00
				new OpCodeHandler_Eb_Gb(Code.Add_rm8_r8, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Gv(Code.Add_rm16_r16, Code.Add_rm32_r32, Code.Add_rm64_r64, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Gb_Eb(Code.Add_r8_rm8),
				new OpCodeHandler_Gv_Ev(Code.Add_r16_rm16, Code.Add_r32_rm32, Code.Add_r64_rm64),
				new OpCodeHandler_RegIb(Code.Add_AL_imm8, Register.AL),
				new OpCodeHandler_Reg_Iz(Code.Add_AX_imm16, Code.Add_EAX_imm32, Register.AX, Register.EAX),
				new OpCodeHandler_OpSizeReg(Code.Pushw_ES, Code.Pushd_ES, Register.ES),
				new OpCodeHandler_OpSizeReg(Code.Popw_ES, Code.Popd_ES, Register.ES),

				// 08
				new OpCodeHandler_Eb_Gb(Code.Or_rm8_r8, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Gv(Code.Or_rm16_r16, Code.Or_rm32_r32, Code.Or_rm64_r64, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Gb_Eb(Code.Or_r8_rm8),
				new OpCodeHandler_Gv_Ev(Code.Or_r16_rm16, Code.Or_r32_rm32, Code.Or_r64_rm64),
				new OpCodeHandler_RegIb(Code.Or_AL_imm8, Register.AL),
				new OpCodeHandler_Reg_Iz(Code.Or_AX_imm16, Code.Or_EAX_imm32, Register.AX, Register.EAX),
				new OpCodeHandler_OpSizeReg(Code.Pushw_CS, Code.Pushd_CS, Register.CS),
				new OpCodeHandler_AnotherTable(TwoByteHandlers_0FXX),

				// 10
				new OpCodeHandler_Eb_Gb(Code.Adc_rm8_r8, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Gv(Code.Adc_rm16_r16, Code.Adc_rm32_r32, Code.Adc_rm64_r64, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Gb_Eb(Code.Adc_r8_rm8),
				new OpCodeHandler_Gv_Ev(Code.Adc_r16_rm16, Code.Adc_r32_rm32, Code.Adc_r64_rm64),
				new OpCodeHandler_RegIb(Code.Adc_AL_imm8, Register.AL),
				new OpCodeHandler_Reg_Iz(Code.Adc_AX_imm16, Code.Adc_EAX_imm32, Register.AX, Register.EAX),
				new OpCodeHandler_OpSizeReg(Code.Pushw_SS, Code.Pushd_SS, Register.SS),
				new OpCodeHandler_OpSizeReg(Code.Popw_SS, Code.Popd_SS, Register.SS),

				// 18
				new OpCodeHandler_Eb_Gb(Code.Sbb_rm8_r8, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Gv(Code.Sbb_rm16_r16, Code.Sbb_rm32_r32, Code.Sbb_rm64_r64, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Gb_Eb(Code.Sbb_r8_rm8),
				new OpCodeHandler_Gv_Ev(Code.Sbb_r16_rm16, Code.Sbb_r32_rm32, Code.Sbb_r64_rm64),
				new OpCodeHandler_RegIb(Code.Sbb_AL_imm8, Register.AL),
				new OpCodeHandler_Reg_Iz(Code.Sbb_AX_imm16, Code.Sbb_EAX_imm32, Register.AX, Register.EAX),
				new OpCodeHandler_OpSizeReg(Code.Pushw_DS, Code.Pushd_DS, Register.DS),
				new OpCodeHandler_OpSizeReg(Code.Popw_DS, Code.Popd_DS, Register.DS),

				// 20
				new OpCodeHandler_Eb_Gb(Code.And_rm8_r8, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Gv(Code.And_rm16_r16, Code.And_rm32_r32, Code.And_rm64_r64, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Gb_Eb(Code.And_r8_rm8),
				new OpCodeHandler_Gv_Ev(Code.And_r16_rm16, Code.And_r32_rm32, Code.And_r64_rm64),
				new OpCodeHandler_RegIb(Code.And_AL_imm8, Register.AL),
				new OpCodeHandler_Reg_Iz(Code.And_AX_imm16, Code.And_EAX_imm32, Register.AX, Register.EAX),
				invalid,// ES:
				new OpCodeHandler_Simple(Code.Daa),

				// 28
				new OpCodeHandler_Eb_Gb(Code.Sub_rm8_r8, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Gv(Code.Sub_rm16_r16, Code.Sub_rm32_r32, Code.Sub_rm64_r64, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Gb_Eb(Code.Sub_r8_rm8),
				new OpCodeHandler_Gv_Ev(Code.Sub_r16_rm16, Code.Sub_r32_rm32, Code.Sub_r64_rm64),
				new OpCodeHandler_RegIb(Code.Sub_AL_imm8, Register.AL),
				new OpCodeHandler_Reg_Iz(Code.Sub_AX_imm16, Code.Sub_EAX_imm32, Register.AX, Register.EAX),
				invalid,// CS:
				new OpCodeHandler_Simple(Code.Das),

				// 30
				new OpCodeHandler_Eb_Gb(Code.Xor_rm8_r8, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Ev_Gv(Code.Xor_rm16_r16, Code.Xor_rm32_r32, Code.Xor_rm64_r64, HandlerFlags.XacquireRelease),
				new OpCodeHandler_Gb_Eb(Code.Xor_r8_rm8),
				new OpCodeHandler_Gv_Ev(Code.Xor_r16_rm16, Code.Xor_r32_rm32, Code.Xor_r64_rm64),
				new OpCodeHandler_RegIb(Code.Xor_AL_imm8, Register.AL),
				new OpCodeHandler_Reg_Iz(Code.Xor_AX_imm16, Code.Xor_EAX_imm32, Register.AX, Register.EAX),
				invalid,// SS:
				new OpCodeHandler_Simple(Code.Aaa),

				// 38
				new OpCodeHandler_Eb_Gb(Code.Cmp_rm8_r8),
				new OpCodeHandler_Ev_Gv(Code.Cmp_rm16_r16, Code.Cmp_rm32_r32, Code.Cmp_rm64_r64),
				new OpCodeHandler_Gb_Eb(Code.Cmp_r8_rm8),
				new OpCodeHandler_Gv_Ev(Code.Cmp_r16_rm16, Code.Cmp_r32_rm32, Code.Cmp_r64_rm64),
				new OpCodeHandler_RegIb(Code.Cmp_AL_imm8, Register.AL),
				new OpCodeHandler_Reg_Iz(Code.Cmp_AX_imm16, Code.Cmp_EAX_imm32, Register.AX, Register.EAX),
				invalid,// DS:
				new OpCodeHandler_Simple(Code.Aas),

				// 40
				new OpCodeHandler_SimpleReg(Code.Inc_r16, Code.Inc_r32, Register.AX, Register.EAX),
				new OpCodeHandler_SimpleReg(Code.Inc_r16, Code.Inc_r32, Register.CX, Register.ECX),
				new OpCodeHandler_SimpleReg(Code.Inc_r16, Code.Inc_r32, Register.DX, Register.EDX),
				new OpCodeHandler_SimpleReg(Code.Inc_r16, Code.Inc_r32, Register.BX, Register.EBX),
				new OpCodeHandler_SimpleReg(Code.Inc_r16, Code.Inc_r32, Register.SP, Register.ESP),
				new OpCodeHandler_SimpleReg(Code.Inc_r16, Code.Inc_r32, Register.BP, Register.EBP),
				new OpCodeHandler_SimpleReg(Code.Inc_r16, Code.Inc_r32, Register.SI, Register.ESI),
				new OpCodeHandler_SimpleReg(Code.Inc_r16, Code.Inc_r32, Register.DI, Register.EDI),

				// 48
				new OpCodeHandler_SimpleReg(Code.Dec_r16, Code.Dec_r32, Register.AX, Register.EAX),
				new OpCodeHandler_SimpleReg(Code.Dec_r16, Code.Dec_r32, Register.CX, Register.ECX),
				new OpCodeHandler_SimpleReg(Code.Dec_r16, Code.Dec_r32, Register.DX, Register.EDX),
				new OpCodeHandler_SimpleReg(Code.Dec_r16, Code.Dec_r32, Register.BX, Register.EBX),
				new OpCodeHandler_SimpleReg(Code.Dec_r16, Code.Dec_r32, Register.SP, Register.ESP),
				new OpCodeHandler_SimpleReg(Code.Dec_r16, Code.Dec_r32, Register.BP, Register.EBP),
				new OpCodeHandler_SimpleReg(Code.Dec_r16, Code.Dec_r32, Register.SI, Register.ESI),
				new OpCodeHandler_SimpleReg(Code.Dec_r16, Code.Dec_r32, Register.DI, Register.EDI),

				// 50
				new OpCodeHandler_SimpleReg(Code.Push_r16, Code.Push_r32, Register.AX, Register.EAX),
				new OpCodeHandler_SimpleReg(Code.Push_r16, Code.Push_r32, Register.CX, Register.ECX),
				new OpCodeHandler_SimpleReg(Code.Push_r16, Code.Push_r32, Register.DX, Register.EDX),
				new OpCodeHandler_SimpleReg(Code.Push_r16, Code.Push_r32, Register.BX, Register.EBX),
				new OpCodeHandler_SimpleReg(Code.Push_r16, Code.Push_r32, Register.SP, Register.ESP),
				new OpCodeHandler_SimpleReg(Code.Push_r16, Code.Push_r32, Register.BP, Register.EBP),
				new OpCodeHandler_SimpleReg(Code.Push_r16, Code.Push_r32, Register.SI, Register.ESI),
				new OpCodeHandler_SimpleReg(Code.Push_r16, Code.Push_r32, Register.DI, Register.EDI),

				// 58
				new OpCodeHandler_SimpleReg(Code.Pop_r16, Code.Pop_r32, Register.AX, Register.EAX),
				new OpCodeHandler_SimpleReg(Code.Pop_r16, Code.Pop_r32, Register.CX, Register.ECX),
				new OpCodeHandler_SimpleReg(Code.Pop_r16, Code.Pop_r32, Register.DX, Register.EDX),
				new OpCodeHandler_SimpleReg(Code.Pop_r16, Code.Pop_r32, Register.BX, Register.EBX),
				new OpCodeHandler_SimpleReg(Code.Pop_r16, Code.Pop_r32, Register.SP, Register.ESP),
				new OpCodeHandler_SimpleReg(Code.Pop_r16, Code.Pop_r32, Register.BP, Register.EBP),
				new OpCodeHandler_SimpleReg(Code.Pop_r16, Code.Pop_r32, Register.SI, Register.ESI),
				new OpCodeHandler_SimpleReg(Code.Pop_r16, Code.Pop_r32, Register.DI, Register.EDI),

				// 60
				new OpCodeHandler_Simple2(Code.Pushaw, Code.Pushad, Code.Pushad),
				new OpCodeHandler_Simple2(Code.Popaw, Code.Popad, Code.Popad),
				new OpCodeHandler_EVEX(new OpCodeHandler_Gv_Ma(Code.Bound_r16_m1616, Code.Bound_r32_m3232)),
				new OpCodeHandler_RvMw_Gw(Code.Arpl_rm16_r16, Code.Arpl_r32m16_r32),
				invalid,// FS:
				invalid,// GS:
				invalid,// os
				invalid,// as

				// 68
				new OpCodeHandler_Iz(Code.Push_imm16, Code.Pushd_imm32),
				new OpCodeHandler_Gv_Ev_Iz(Code.Imul_r16_rm16_imm16, Code.Imul_r32_rm32_imm32, Code.Imul_r64_rm64_imm32),
				new OpCodeHandler_Ib2(Code.Pushw_imm8, Code.Pushd_imm8),
				new OpCodeHandler_Gv_Ev_Ib(Code.Imul_r16_rm16_imm8, Code.Imul_r32_rm32_imm8, Code.Imul_r64_rm64_imm8),
				new OpCodeHandler_Yb_Reg(Code.Insb_m8_DX, Register.DX),
				new OpCodeHandler_Yv_Reg(Code.Insw_m16_DX, Code.Insd_m32_DX, Register.DX, Register.DX),
				new OpCodeHandler_Reg_Xb(Code.Outsb_DX_m8, Register.DX),
				new OpCodeHandler_Reg_Xv(Code.Outsw_DX_m16, Code.Outsd_DX_m32, Register.DX, Register.DX),

				// 70
				new OpCodeHandler_Jb(Code.Jo_rel8_16, Code.Jo_rel8_32),
				new OpCodeHandler_Jb(Code.Jno_rel8_16, Code.Jno_rel8_32),
				new OpCodeHandler_Jb(Code.Jb_rel8_16, Code.Jb_rel8_32),
				new OpCodeHandler_Jb(Code.Jae_rel8_16, Code.Jae_rel8_32),
				new OpCodeHandler_Jb(Code.Je_rel8_16, Code.Je_rel8_32),
				new OpCodeHandler_Jb(Code.Jne_rel8_16, Code.Jne_rel8_32),
				new OpCodeHandler_Jb(Code.Jbe_rel8_16, Code.Jbe_rel8_32),
				new OpCodeHandler_Jb(Code.Ja_rel8_16, Code.Ja_rel8_32),

				// 78
				new OpCodeHandler_Jb(Code.Js_rel8_16, Code.Js_rel8_32),
				new OpCodeHandler_Jb(Code.Jns_rel8_16, Code.Jns_rel8_32),
				new OpCodeHandler_Jb(Code.Jp_rel8_16, Code.Jp_rel8_32),
				new OpCodeHandler_Jb(Code.Jnp_rel8_16, Code.Jnp_rel8_32),
				new OpCodeHandler_Jb(Code.Jl_rel8_16, Code.Jl_rel8_32),
				new OpCodeHandler_Jb(Code.Jge_rel8_16, Code.Jge_rel8_32),
				new OpCodeHandler_Jb(Code.Jle_rel8_16, Code.Jle_rel8_32),
				new OpCodeHandler_Jb(Code.Jg_rel8_16, Code.Jg_rel8_32),

				// 80
				new OpCodeHandler_Group(handlers_Grp_80),
				new OpCodeHandler_Group(handlers_Grp_81),
				new OpCodeHandler_Group(handlers_Grp_82),
				new OpCodeHandler_Group(handlers_Grp_83),
				new OpCodeHandler_Eb_Gb(Code.Test_rm8_r8),
				new OpCodeHandler_Ev_Gv(Code.Test_rm16_r16, Code.Test_rm32_r32, Code.Test_rm64_r64),
				new OpCodeHandler_Eb_Gb(Code.Xchg_rm8_r8, HandlerFlags.XacquireRelease | HandlerFlags.XacquireReleaseNoLock),
				new OpCodeHandler_Ev_Gv(Code.Xchg_rm16_r16, Code.Xchg_rm32_r32, Code.Xchg_rm64_r64, HandlerFlags.XacquireRelease | HandlerFlags.XacquireReleaseNoLock),

				// 88
				new OpCodeHandler_Eb_Gb(Code.Mov_rm8_r8, HandlerFlags.Xrelease | HandlerFlags.XacquireReleaseNoLock),
				new OpCodeHandler_Ev_Gv(Code.Mov_rm16_r16, Code.Mov_rm32_r32, Code.Mov_rm64_r64, HandlerFlags.Xrelease | HandlerFlags.XacquireReleaseNoLock),
				new OpCodeHandler_Gb_Eb(Code.Mov_r8_rm8),
				new OpCodeHandler_Gv_Ev(Code.Mov_r16_rm16, Code.Mov_r32_rm32, Code.Mov_r64_rm64),
				new OpCodeHandler_Ev_Sw(Code.Mov_rm16_Sreg, Code.Mov_rm32_Sreg, Code.Mov_rm64_Sreg),
				new OpCodeHandler_Gv_M(Code.Lea_r16_m, Code.Lea_r32_m, Code.Lea_r64_m),
				new OpCodeHandler_Sw_Ev(Code.Mov_Sreg_rm16, Code.Mov_Sreg_rm32, Code.Mov_Sreg_rm64),
				new OpCodeHandler_XOP(new OpCodeHandler_Group(handlers_Grp_8F)),

				// 90
				new OpCodeHandler_Xchg_Reg_eAX(0),
				new OpCodeHandler_Xchg_Reg_eAX(1),
				new OpCodeHandler_Xchg_Reg_eAX(2),
				new OpCodeHandler_Xchg_Reg_eAX(3),
				new OpCodeHandler_Xchg_Reg_eAX(4),
				new OpCodeHandler_Xchg_Reg_eAX(5),
				new OpCodeHandler_Xchg_Reg_eAX(6),
				new OpCodeHandler_Xchg_Reg_eAX(7),

				// 98
				new OpCodeHandler_Simple2(Code.Cbw, Code.Cwde, Code.Cdqe),
				new OpCodeHandler_Simple2(Code.Cwd, Code.Cdq, Code.Cqo),
				new OpCodeHandler_Ap(Code.Call_ptr1616, Code.Call_ptr3216),
				new OpCodeHandler_Simple(Code.Wait),
				new OpCodeHandler_Simple2(Code.Pushfw, Code.Pushfd, Code.Pushfq),
				new OpCodeHandler_Simple2(Code.Popfw, Code.Popfd, Code.Popfq),
				new OpCodeHandler_Simple(Code.Sahf),
				new OpCodeHandler_Simple(Code.Lahf),

				// A0
				new OpCodeHandler_Reg_Ob(Code.Mov_AL_moffs8, Register.AL),
				new OpCodeHandler_Reg_Ov(Code.Mov_AX_moffs16, Code.Mov_EAX_moffs32, Register.AX, Register.EAX),
				new OpCodeHandler_Ob_Reg(Code.Mov_moffs8_AL, Register.AL),
				new OpCodeHandler_Ov_Reg(Code.Mov_moffs16_AX, Code.Mov_moffs32_EAX, Register.AX, Register.EAX),
				new OpCodeHandler_Yb_Xb(Code.Movsb_m8_m8),
				new OpCodeHandler_Yv_Xv(Code.Movsw_m16_m16, Code.Movsd_m32_m32, Code.Movsq_m64_m64),
				new OpCodeHandler_Xb_Yb(Code.Cmpsb_m8_m8),
				new OpCodeHandler_Xv_Yv(Code.Cmpsw_m16_m16, Code.Cmpsd_m32_m32, Code.Cmpsq_m64_m64),

				// A8
				new OpCodeHandler_RegIb(Code.Test_AL_imm8, Register.AL),
				new OpCodeHandler_Reg_Iz(Code.Test_AX_imm16, Code.Test_EAX_imm32, Register.AX, Register.EAX),
				new OpCodeHandler_Yb_Reg(Code.Stosb_m8_AL, Register.AL),
				new OpCodeHandler_Yv_Reg(Code.Stosw_m16_AX, Code.Stosd_m32_EAX, Register.AX, Register.EAX),
				new OpCodeHandler_Reg_Xb(Code.Lodsb_AL_m8, Register.AL),
				new OpCodeHandler_Reg_Xv(Code.Lodsw_AX_m16, Code.Lodsd_EAX_m32, Register.AX, Register.EAX),
				new OpCodeHandler_Reg_Yb(Code.Scasb_AL_m8, Register.AL),
				new OpCodeHandler_Reg_Yv(Code.Scasw_AX_m16, Code.Scasd_EAX_m32, Register.AX, Register.EAX),

				// B0
				new OpCodeHandler_RegIb(Code.Mov_r8_imm8, Register.AL),
				new OpCodeHandler_RegIb(Code.Mov_r8_imm8, Register.CL),
				new OpCodeHandler_RegIb(Code.Mov_r8_imm8, Register.DL),
				new OpCodeHandler_RegIb(Code.Mov_r8_imm8, Register.BL),
				new OpCodeHandler_RegIb(Code.Mov_r8_imm8, Register.AH),
				new OpCodeHandler_RegIb(Code.Mov_r8_imm8, Register.CH),
				new OpCodeHandler_RegIb(Code.Mov_r8_imm8, Register.DH),
				new OpCodeHandler_RegIb(Code.Mov_r8_imm8, Register.BH),

				// B8
				new OpCodeHandler_Reg_Iz(Code.Mov_r16_imm16, Code.Mov_r32_imm32, Register.AX, Register.EAX),
				new OpCodeHandler_Reg_Iz(Code.Mov_r16_imm16, Code.Mov_r32_imm32, Register.CX, Register.ECX),
				new OpCodeHandler_Reg_Iz(Code.Mov_r16_imm16, Code.Mov_r32_imm32, Register.DX, Register.EDX),
				new OpCodeHandler_Reg_Iz(Code.Mov_r16_imm16, Code.Mov_r32_imm32, Register.BX, Register.EBX),
				new OpCodeHandler_Reg_Iz(Code.Mov_r16_imm16, Code.Mov_r32_imm32, Register.SP, Register.ESP),
				new OpCodeHandler_Reg_Iz(Code.Mov_r16_imm16, Code.Mov_r32_imm32, Register.BP, Register.EBP),
				new OpCodeHandler_Reg_Iz(Code.Mov_r16_imm16, Code.Mov_r32_imm32, Register.SI, Register.ESI),
				new OpCodeHandler_Reg_Iz(Code.Mov_r16_imm16, Code.Mov_r32_imm32, Register.DI, Register.EDI),

				// C0
				new OpCodeHandler_Group(handlers_Grp_C0),
				new OpCodeHandler_Group(handlers_Grp_C1),
				new OpCodeHandler_Iw(Code.Retnw_imm16, Code.Retnd_imm16, Code.Retnq_imm16),
				new OpCodeHandler_Simple2(Code.Retnw, Code.Retnd, Code.Retnq),
				new OpCodeHandler_VEX3(new OpCodeHandler_Gv_Mp(Code.Les_r16_m32, Code.Les_r32_m48)),
				new OpCodeHandler_VEX2(new OpCodeHandler_Gv_Mp(Code.Lds_r16_m32, Code.Lds_r32_m48)),
				new OpCodeHandler_Group8x64(handlers_Grp_C6_lo, handlers_Grp_C6_hi),
				new OpCodeHandler_Group8x64(handlers_Grp_C7_lo, handlers_Grp_C7_hi),

				// C8
				new OpCodeHandler_Iw_Ib(Code.Enterw_imm16_imm8, Code.Enterd_imm16_imm8, Code.Enterq_imm16_imm8),
				new OpCodeHandler_Simple2(Code.Leavew, Code.Leaved, Code.Leaveq),
				new OpCodeHandler_Iw(Code.Retfw_imm16, Code.Retfd_imm16, Code.Retfq_imm16),
				new OpCodeHandler_Simple2(Code.Retfw, Code.Retfd, Code.Retfq),
				new OpCodeHandler_Simple(Code.Int3),
				new OpCodeHandler_Ib(Code.Int_imm8),
				new OpCodeHandler_Simple(Code.Into),
				new OpCodeHandler_Simple2(Code.Iretw, Code.Iretd, Code.Iretq),

				// D0
				new OpCodeHandler_Group(handlers_Grp_D0),
				new OpCodeHandler_Group(handlers_Grp_D1),
				new OpCodeHandler_Group(handlers_Grp_D2),
				new OpCodeHandler_Group(handlers_Grp_D3),
				new OpCodeHandler_Ib(Code.Aam_imm8),
				new OpCodeHandler_Ib(Code.Aad_imm8),
				new OpCodeHandler_Simple(Code.Salc),
				new OpCodeHandler_MemBx(Code.Xlatb),

				// D8
				new OpCodeHandler_Group8x8(OpCodeHandlersFpu32Tables.handlers_FPU_D8_low, OpCodeHandlersFpu32Tables.handlers_FPU_D8_high),
				new OpCodeHandler_Group8x64(OpCodeHandlersFpu32Tables.handlers_FPU_D9_low, OpCodeHandlersFpu32Tables.handlers_FPU_D9_high),
				new OpCodeHandler_Group8x64(OpCodeHandlersFpu32Tables.handlers_FPU_DA_low, OpCodeHandlersFpu32Tables.handlers_FPU_DA_high),
				new OpCodeHandler_Group8x64(OpCodeHandlersFpu32Tables.handlers_FPU_DB_low, OpCodeHandlersFpu32Tables.handlers_FPU_DB_high),
				new OpCodeHandler_Group8x8(OpCodeHandlersFpu32Tables.handlers_FPU_DC_low, OpCodeHandlersFpu32Tables.handlers_FPU_DC_high),
				new OpCodeHandler_Group8x8(OpCodeHandlersFpu32Tables.handlers_FPU_DD_low, OpCodeHandlersFpu32Tables.handlers_FPU_DD_high),
				new OpCodeHandler_Group8x64(OpCodeHandlersFpu32Tables.handlers_FPU_DE_low, OpCodeHandlersFpu32Tables.handlers_FPU_DE_high),
				new OpCodeHandler_Group8x64(OpCodeHandlersFpu32Tables.handlers_FPU_DF_low, OpCodeHandlersFpu32Tables.handlers_FPU_DF_high),

				// E0
				new OpCodeHandler_Jb2(Code.Loopne_rel8_16_CX, Code.Loopne_rel8_16_ECX, Code.Loopne_rel8_32_CX, Code.Loopne_rel8_32_ECX),
				new OpCodeHandler_Jb2(Code.Loope_rel8_16_CX, Code.Loope_rel8_16_ECX, Code.Loope_rel8_32_CX, Code.Loope_rel8_32_ECX),
				new OpCodeHandler_Jb2(Code.Loop_rel8_16_CX, Code.Loop_rel8_16_ECX, Code.Loop_rel8_32_CX, Code.Loop_rel8_32_ECX),
				new OpCodeHandler_Jb2(Code.Jcxz_rel8_16, Code.Jecxz_rel8_16, Code.Jcxz_rel8_32, Code.Jecxz_rel8_32),
				new OpCodeHandler_RegIb(Code.In_AL_imm8, Register.AL),
				new OpCodeHandler_Reg_Ib2(Code.In_AX_imm8, Code.In_EAX_imm8, Register.AX, Register.EAX),
				new OpCodeHandler_IbReg(Code.Out_imm8_AL, Register.AL),
				new OpCodeHandler_IbReg2(Code.Out_imm8_AX, Code.Out_imm8_EAX, Register.AX, Register.EAX),

				// E8
				new OpCodeHandler_Jz(Code.Call_rel16, Code.Call_rel32_32),
				new OpCodeHandler_Jz(Code.Jmp_rel16, Code.Jmp_rel32_32),
				new OpCodeHandler_Ap(Code.Jmp_ptr1616, Code.Jmp_ptr3216),
				new OpCodeHandler_Jb(Code.Jmp_rel8_16, Code.Jmp_rel8_32),
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
