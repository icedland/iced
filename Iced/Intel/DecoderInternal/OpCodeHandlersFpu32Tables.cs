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

#if !NO_DECODER32 && !NO_DECODER
namespace Iced.Intel.DecoderInternal.OpCodeHandlers32 {
	/// <summary>
	/// x87 opcode handlers
	/// </summary>
	static class OpCodeHandlersFpu32Tables {
		public static readonly OpCodeHandler[] handlers_FPU_D8_low;
		public static readonly OpCodeHandler[] handlers_FPU_D8_high;
		public static readonly OpCodeHandler[] handlers_FPU_D9_low;
		public static readonly OpCodeHandler[] handlers_FPU_D9_high;
		public static readonly OpCodeHandler[] handlers_FPU_DA_low;
		public static readonly OpCodeHandler[] handlers_FPU_DA_high;
		public static readonly OpCodeHandler[] handlers_FPU_DB_low;
		public static readonly OpCodeHandler[] handlers_FPU_DB_high;
		public static readonly OpCodeHandler[] handlers_FPU_DC_low;
		public static readonly OpCodeHandler[] handlers_FPU_DC_high;
		public static readonly OpCodeHandler[] handlers_FPU_DD_low;
		public static readonly OpCodeHandler[] handlers_FPU_DD_high;
		public static readonly OpCodeHandler[] handlers_FPU_DE_low;
		public static readonly OpCodeHandler[] handlers_FPU_DE_high;
		public static readonly OpCodeHandler[] handlers_FPU_DF_low;
		public static readonly OpCodeHandler[] handlers_FPU_DF_high;

		static OpCodeHandlersFpu32Tables() {
			var invalid = OpCodeHandler_Invalid.Instance;

			handlers_FPU_D8_low = new OpCodeHandler[8] {
				new OpCodeHandler_Mf32(Code.Fadd_Mf32),
				new OpCodeHandler_Mf32(Code.Fmul_Mf32),
				new OpCodeHandler_Mf32(Code.Fcom_Mf32),
				new OpCodeHandler_Mf32(Code.Fcomp_Mf32),
				new OpCodeHandler_Mf32(Code.Fsub_Mf32),
				new OpCodeHandler_Mf32(Code.Fsubr_Mf32),
				new OpCodeHandler_Mf32(Code.Fdiv_Mf32),
				new OpCodeHandler_Mf32(Code.Fdivr_Mf32),
			};

			handlers_FPU_D8_high = new OpCodeHandler[8] {
				new OpCodeHandler_ST_STi(Code.Fadd_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fmul_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcom_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcomp_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fsub_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fsubr_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fdiv_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fdivr_ST_STi),
			};

			handlers_FPU_D9_low = new OpCodeHandler[8] {
				new OpCodeHandler_Mf32(Code.Fld_Mf32),
				invalid,
				new OpCodeHandler_Mf32(Code.Fst_Mf32),
				new OpCodeHandler_Mf32(Code.Fstp_Mf32),
				new OpCodeHandler_Mf(Code.Fldenv_M14, Code.Fldenv_M28, MemorySize.FpuEnv14, MemorySize.FpuEnv28),
				new OpCodeHandler_Mf2(Code.Fldcw_Mw, MemorySize.UInt16),
				new OpCodeHandler_Mf(Code.Fnstenv_M14, Code.Fnstenv_M28, MemorySize.FpuEnv14, MemorySize.FpuEnv28),
				new OpCodeHandler_Mf2(Code.Fnstcw_Mw, MemorySize.UInt16),
			};

			handlers_FPU_D9_high = new OpCodeHandler[0x40] {
				// C0
				new OpCodeHandler_ST_STi(Code.Fld_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fld_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fld_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fld_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fld_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fld_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fld_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fld_ST_STi),

				// C8
				new OpCodeHandler_ST_STi(Code.Fxch_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fxch_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fxch_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fxch_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fxch_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fxch_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fxch_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fxch_ST_STi),

				// D0
				new OpCodeHandler_Simple(Code.Fnop),
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
				invalid,

				// E0
				new OpCodeHandler_Simple(Code.Fchs),
				new OpCodeHandler_Simple(Code.Fabs),
				invalid,
				invalid,
				new OpCodeHandler_Simple(Code.Ftst),
				new OpCodeHandler_Simple(Code.Fxam),
				invalid,
				invalid,

				// E8
				new OpCodeHandler_Simple(Code.Fld1),
				new OpCodeHandler_Simple(Code.Fldl2t),
				new OpCodeHandler_Simple(Code.Fldl2e),
				new OpCodeHandler_Simple(Code.Fldpi),
				new OpCodeHandler_Simple(Code.Fldlg2),
				new OpCodeHandler_Simple(Code.Fldln2),
				new OpCodeHandler_Simple(Code.Fldz),
				invalid,

				// F0
				new OpCodeHandler_Simple(Code.F2xm1),
				new OpCodeHandler_Simple(Code.Fyl2x),
				new OpCodeHandler_Simple(Code.Fptan),
				new OpCodeHandler_Simple(Code.Fpatan),
				new OpCodeHandler_Simple(Code.Fxtract),
				new OpCodeHandler_Simple(Code.Fprem1),
				new OpCodeHandler_Simple(Code.Fdecstp),
				new OpCodeHandler_Simple(Code.Fincstp),

				// F8
				new OpCodeHandler_Simple(Code.Fprem),
				new OpCodeHandler_Simple(Code.Fyl2xp1),
				new OpCodeHandler_Simple(Code.Fsqrt),
				new OpCodeHandler_Simple(Code.Fsincos),
				new OpCodeHandler_Simple(Code.Frndint),
				new OpCodeHandler_Simple(Code.Fscale),
				new OpCodeHandler_Simple(Code.Fsin),
				new OpCodeHandler_Simple(Code.Fcos),
			};

			handlers_FPU_DA_low = new OpCodeHandler[8] {
				new OpCodeHandler_Mfi32(Code.Fiadd_Mfi32),
				new OpCodeHandler_Mfi32(Code.Fimul_Mfi32),
				new OpCodeHandler_Mfi32(Code.Ficom_Mfi32),
				new OpCodeHandler_Mfi32(Code.Ficomp_Mfi32),
				new OpCodeHandler_Mfi32(Code.Fisub_Mfi32),
				new OpCodeHandler_Mfi32(Code.Fisubr_Mfi32),
				new OpCodeHandler_Mfi32(Code.Fidiv_Mfi32),
				new OpCodeHandler_Mfi32(Code.Fidivr_Mfi32),
			};

			handlers_FPU_DA_high = new OpCodeHandler[0x40] {
				// C0
				new OpCodeHandler_ST_STi(Code.Fcmovb_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovb_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovb_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovb_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovb_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovb_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovb_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovb_ST_STi),

				// C8
				new OpCodeHandler_ST_STi(Code.Fcmove_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmove_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmove_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmove_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmove_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmove_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmove_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmove_ST_STi),

				// D0
				new OpCodeHandler_ST_STi(Code.Fcmovbe_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovbe_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovbe_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovbe_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovbe_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovbe_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovbe_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovbe_ST_STi),

				// D8
				new OpCodeHandler_ST_STi(Code.Fcmovu_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovu_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovu_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovu_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovu_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovu_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovu_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovu_ST_STi),

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
				new OpCodeHandler_Simple(Code.Fucompp),
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

			handlers_FPU_DB_low = new OpCodeHandler[8] {
				new OpCodeHandler_Mfi32(Code.Fild_Mfi32),
				new OpCodeHandler_Mfi32(Code.Fisttp_Mfi32),
				new OpCodeHandler_Mfi32(Code.Fist_Mfi32),
				new OpCodeHandler_Mfi32(Code.Fistp_Mfi32),
				invalid,
				new OpCodeHandler_Mf80(Code.Fld_Mf80),
				invalid,
				new OpCodeHandler_Mf80(Code.Fstp_Mf80),
			};

			handlers_FPU_DB_high = new OpCodeHandler[0x40] {
				// C0
				new OpCodeHandler_ST_STi(Code.Fcmovnb_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovnb_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovnb_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovnb_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovnb_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovnb_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovnb_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovnb_ST_STi),

				// C8
				new OpCodeHandler_ST_STi(Code.Fcmovne_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovne_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovne_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovne_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovne_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovne_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovne_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovne_ST_STi),

				// D0
				new OpCodeHandler_ST_STi(Code.Fcmovnbe_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovnbe_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovnbe_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovnbe_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovnbe_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovnbe_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovnbe_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovnbe_ST_STi),

				// D8
				new OpCodeHandler_ST_STi(Code.Fcmovnu_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovnu_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovnu_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovnu_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovnu_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovnu_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovnu_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcmovnu_ST_STi),

				// E0
				invalid,
				invalid,
				new OpCodeHandler_Simple(Code.Fnclex),
				new OpCodeHandler_Simple(Code.Fninit),
				invalid,
				invalid,
				invalid,
				invalid,

				// E8
				new OpCodeHandler_ST_STi(Code.Fucomi_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fucomi_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fucomi_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fucomi_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fucomi_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fucomi_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fucomi_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fucomi_ST_STi),

				// F0
				new OpCodeHandler_ST_STi(Code.Fcomi_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcomi_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcomi_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcomi_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcomi_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcomi_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcomi_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcomi_ST_STi),

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

			handlers_FPU_DC_low = new OpCodeHandler[8] {
				new OpCodeHandler_Mf64(Code.Fadd_Mf64),
				new OpCodeHandler_Mf64(Code.Fmul_Mf64),
				new OpCodeHandler_Mf64(Code.Fcom_Mf64),
				new OpCodeHandler_Mf64(Code.Fcomp_Mf64),
				new OpCodeHandler_Mf64(Code.Fsub_Mf64),
				new OpCodeHandler_Mf64(Code.Fsubr_Mf64),
				new OpCodeHandler_Mf64(Code.Fdiv_Mf64),
				new OpCodeHandler_Mf64(Code.Fdivr_Mf64),
			};

			handlers_FPU_DC_high = new OpCodeHandler[8] {
				new OpCodeHandler_STi_ST(Code.Fadd_STi_ST),
				new OpCodeHandler_STi_ST(Code.Fmul_STi_ST),
				invalid,
				invalid,
				new OpCodeHandler_STi_ST(Code.Fsubr_STi_ST),
				new OpCodeHandler_STi_ST(Code.Fsub_STi_ST),
				new OpCodeHandler_STi_ST(Code.Fdivr_STi_ST),
				new OpCodeHandler_STi_ST(Code.Fdiv_STi_ST),
			};

			handlers_FPU_DD_low = new OpCodeHandler[8] {
				new OpCodeHandler_Mf64(Code.Fld_Mf64),
				new OpCodeHandler_Mf64(Code.Fisttp_Mf64),
				new OpCodeHandler_Mf64(Code.Fst_Mf64),
				new OpCodeHandler_Mf64(Code.Fstp_Mf64),
				new OpCodeHandler_Mf(Code.Frstor_M98, Code.Frstor_M108, MemorySize.FpuState94, MemorySize.FpuState108),
				invalid,
				new OpCodeHandler_Mf(Code.Fnsave_M98, Code.Fnsave_M108, MemorySize.FpuState94, MemorySize.FpuState108),
				new OpCodeHandler_Mf2(Code.Fnstsw_Mw, MemorySize.UInt16),
			};

			handlers_FPU_DD_high = new OpCodeHandler[8] {
				new OpCodeHandler_STi(Code.Ffree_STi),
				invalid,
				new OpCodeHandler_STi(Code.Fst_STi),
				new OpCodeHandler_STi(Code.Fstp_STi),
				new OpCodeHandler_ST_STi(Code.Fucom_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fucomp_ST_STi),
				invalid,
				invalid,
			};

			handlers_FPU_DE_low = new OpCodeHandler[8] {
				new OpCodeHandler_Mfi16(Code.Fiadd_Mfi16),
				new OpCodeHandler_Mfi16(Code.Fimul_Mfi16),
				new OpCodeHandler_Mfi16(Code.Ficom_Mfi16),
				new OpCodeHandler_Mfi16(Code.Ficomp_Mfi16),
				new OpCodeHandler_Mfi16(Code.Fisub_Mfi16),
				new OpCodeHandler_Mfi16(Code.Fisubr_Mfi16),
				new OpCodeHandler_Mfi16(Code.Fidiv_Mfi16),
				new OpCodeHandler_Mfi16(Code.Fidivr_Mfi16),
			};

			handlers_FPU_DE_high = new OpCodeHandler[0x40] {
				// C0
				new OpCodeHandler_STi_ST(Code.Faddp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Faddp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Faddp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Faddp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Faddp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Faddp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Faddp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Faddp_STi_ST),

				// C8
				new OpCodeHandler_STi_ST(Code.Fmulp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Fmulp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Fmulp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Fmulp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Fmulp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Fmulp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Fmulp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Fmulp_STi_ST),

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
				new OpCodeHandler_Simple(Code.Fcompp),
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// E0
				new OpCodeHandler_STi_ST(Code.Fsubrp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Fsubrp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Fsubrp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Fsubrp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Fsubrp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Fsubrp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Fsubrp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Fsubrp_STi_ST),

				// E8
				new OpCodeHandler_STi_ST(Code.Fsubp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Fsubp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Fsubp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Fsubp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Fsubp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Fsubp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Fsubp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Fsubp_STi_ST),

				// F0
				new OpCodeHandler_STi_ST(Code.Fdivrp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Fdivrp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Fdivrp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Fdivrp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Fdivrp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Fdivrp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Fdivrp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Fdivrp_STi_ST),

				// F8
				new OpCodeHandler_STi_ST(Code.Fdivp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Fdivp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Fdivp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Fdivp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Fdivp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Fdivp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Fdivp_STi_ST),
				new OpCodeHandler_STi_ST(Code.Fdivp_STi_ST),
			};

			handlers_FPU_DF_low = new OpCodeHandler[8] {
				new OpCodeHandler_Mfi16(Code.Fild_Mfi16),
				new OpCodeHandler_Mfi16(Code.Fisttp_Mfi16),
				new OpCodeHandler_Mfi16(Code.Fist_Mfi16),
				new OpCodeHandler_Mfi16(Code.Fistp_Mfi16),
				new OpCodeHandler_Mfbcd(Code.Fbld_Mfbcd),
				new OpCodeHandler_Mfi64(Code.Fild_Mfi64),
				new OpCodeHandler_Mfbcd(Code.Fbstp_Mfbcd),
				new OpCodeHandler_Mfi64(Code.Fistp_Mfi64),
			};

			handlers_FPU_DF_high = new OpCodeHandler[0x40] {
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
				invalid,
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
				invalid,

				// E0
				new OpCodeHandler_Reg(Code.Fnstsw_AX, Register.AX),
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// E8
				new OpCodeHandler_ST_STi(Code.Fucomip_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fucomip_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fucomip_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fucomip_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fucomip_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fucomip_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fucomip_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fucomip_ST_STi),

				// F0
				new OpCodeHandler_ST_STi(Code.Fcomip_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcomip_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcomip_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcomip_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcomip_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcomip_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcomip_ST_STi),
				new OpCodeHandler_ST_STi(Code.Fcomip_ST_STi),

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
		}
	}
}
#endif
