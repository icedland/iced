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

#if !NO_DECODER64 && !NO_DECODER
namespace Iced.Intel.DecoderInternal.OpCodeHandlers64 {
	/// <summary>
	/// Handlers for 64-bit mode (XOP)
	/// </summary>
	static class OpCodeHandlers64Tables_XOP {
		internal static readonly OpCodeHandler[] XOP8;
		internal static readonly OpCodeHandler[] XOP9;
		internal static readonly OpCodeHandler[] XOPA;

		static OpCodeHandler W(OpCodeHandler handlerW0, OpCodeHandler handlerW1) =>
			new OpCodeHandler_MandatoryPrefix2(new OpCodeHandler_W(handlerW0, handlerW1));

		static OpCodeHandler W0L0(OpCodeHandler handler) => W0L(handler, OpCodeHandler_Invalid.Instance);

		static OpCodeHandler W0L(OpCodeHandler handler128, OpCodeHandler handler256) {
			var invalid = OpCodeHandler_Invalid.Instance;
			return new OpCodeHandler_MandatoryPrefix2(
				new OpCodeHandler_VectorLength_VEX(
					new OpCodeHandler_W(
						handler128,
						invalid
					),
					new OpCodeHandler_W(
						handler256,
						invalid
					)
				)
			);
		}

		static OpCodeHandler WL0(OpCodeHandler handlerW0, OpCodeHandler handlerW1) =>
			new OpCodeHandler_MandatoryPrefix2(
				new OpCodeHandler_VectorLength_VEX(
					new OpCodeHandler_W(
						handlerW0,
						handlerW1
					),
					OpCodeHandler_Invalid.Instance
				)
			);

		static OpCodeHandlers64Tables_XOP() {
			OpCodeHandler[] tbl;
			var invalid = OpCodeHandler_Invalid.Instance;

			tbl = new OpCodeHandler[0x100];
			for (int i = 0; i < tbl.Length; i++)
				tbl[i] = invalid;
			tbl[0x85] = W0L0(new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.XOP_Vpmacssww_xmm_xmm_xmmm128_xmm));
			tbl[0x86] = W0L0(new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.XOP_Vpmacsswd_xmm_xmm_xmmm128_xmm));
			tbl[0x87] = W0L0(new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.XOP_Vpmacssdql_xmm_xmm_xmmm128_xmm));
			tbl[0x8E] = W0L0(new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.XOP_Vpmacssdd_xmm_xmm_xmmm128_xmm));
			tbl[0x8F] = W0L0(new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.XOP_Vpmacssdqh_xmm_xmm_xmmm128_xmm));
			tbl[0x95] = W0L0(new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.XOP_Vpmacsww_xmm_xmm_xmmm128_xmm));
			tbl[0x96] = W0L0(new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.XOP_Vpmacswd_xmm_xmm_xmmm128_xmm));
			tbl[0x97] = W0L0(new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.XOP_Vpmacsdql_xmm_xmm_xmmm128_xmm));
			tbl[0x9E] = W0L0(new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.XOP_Vpmacsdd_xmm_xmm_xmmm128_xmm));
			tbl[0x9F] = W0L0(new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.XOP_Vpmacsdqh_xmm_xmm_xmmm128_xmm));
			tbl[0xA2] = W(
				new OpCodeHandler_VectorLength_VEX(
					new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.XOP_Vpcmov_xmm_xmm_xmmm128_xmm),
					new OpCodeHandler_VEX_VHWIs4(Register.YMM0, Code.XOP_Vpcmov_ymm_ymm_ymmm256_ymm)
				),
				new OpCodeHandler_VectorLength_VEX(
					new OpCodeHandler_VEX_VHIs4W(Register.XMM0, Code.XOP_Vpcmov_xmm_xmm_xmm_xmmm128),
					new OpCodeHandler_VEX_VHIs4W(Register.YMM0, Code.XOP_Vpcmov_ymm_ymm_ymm_ymmm256)
				)
			);
			tbl[0xA3] = W(
				new OpCodeHandler_VectorLength_VEX(
					new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.XOP_Vpperm_xmm_xmm_xmmm128_xmm),
					invalid
				),
				new OpCodeHandler_VectorLength_VEX(
					new OpCodeHandler_VEX_VHIs4W(Register.XMM0, Code.XOP_Vpperm_xmm_xmm_xmm_xmmm128),
					invalid
				)
			);
			tbl[0xA6] = W0L0(new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.XOP_Vpmadcsswd_xmm_xmm_xmmm128_xmm));
			tbl[0xB6] = W0L0(new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.XOP_Vpmadcswd_xmm_xmm_xmmm128_xmm));
			tbl[0xC0] = W0L0(new OpCodeHandler_VEX_VWIb(Register.XMM0, Code.XOP_Vprotb_xmm_xmmm128_imm8));
			tbl[0xC1] = W0L0(new OpCodeHandler_VEX_VWIb(Register.XMM0, Code.XOP_Vprotw_xmm_xmmm128_imm8));
			tbl[0xC2] = W0L0(new OpCodeHandler_VEX_VWIb(Register.XMM0, Code.XOP_Vprotd_xmm_xmmm128_imm8));
			tbl[0xC3] = W0L0(new OpCodeHandler_VEX_VWIb(Register.XMM0, Code.XOP_Vprotq_xmm_xmmm128_imm8));
			tbl[0xCC] = W0L0(new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.XOP_Vpcomb_xmm_xmm_xmmm128_imm8));
			tbl[0xCD] = W0L0(new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.XOP_Vpcomw_xmm_xmm_xmmm128_imm8));
			tbl[0xCE] = W0L0(new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.XOP_Vpcomd_xmm_xmm_xmmm128_imm8));
			tbl[0xCF] = W0L0(new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.XOP_Vpcomq_xmm_xmm_xmmm128_imm8));
			tbl[0xEC] = W0L0(new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.XOP_Vpcomub_xmm_xmm_xmmm128_imm8));
			tbl[0xED] = W0L0(new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.XOP_Vpcomuw_xmm_xmm_xmmm128_imm8));
			tbl[0xEE] = W0L0(new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.XOP_Vpcomud_xmm_xmm_xmmm128_imm8));
			tbl[0xEF] = W0L0(new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.XOP_Vpcomuq_xmm_xmm_xmmm128_imm8));
			XOP8 = tbl;

			tbl = new OpCodeHandler[0x100];
			for (int i = 0; i < tbl.Length; i++)
				tbl[i] = invalid;
			var grp_XOP9_01 = new OpCodeHandler[8] {
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Hv_Ev(Code.XOP_Blcfill_r32_rm32, Code.XOP_Blcfill_r64_rm64),
						invalid
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Hv_Ev(Code.XOP_Blsfill_r32_rm32, Code.XOP_Blsfill_r64_rm64),
						invalid
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Hv_Ev(Code.XOP_Blcs_r32_rm32, Code.XOP_Blcs_r64_rm64),
						invalid
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Hv_Ev(Code.XOP_Tzmsk_r32_rm32, Code.XOP_Tzmsk_r64_rm64),
						invalid
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Hv_Ev(Code.XOP_Blcic_r32_rm32, Code.XOP_Blcic_r64_rm64),
						invalid
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Hv_Ev(Code.XOP_Blsic_r32_rm32, Code.XOP_Blsic_r64_rm64),
						invalid
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Hv_Ev(Code.XOP_T1mskc_r32_rm32, Code.XOP_T1mskc_r64_rm64),
						invalid
					)
				),
			};
			tbl[0x01] = new OpCodeHandler_Group(grp_XOP9_01);
			var grp_XOP9_02 = new OpCodeHandler[8] {
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Hv_Ev(Code.XOP_Blcmsk_r32_rm32, Code.XOP_Blcmsk_r64_rm64),
						invalid
					)
				),
				invalid,
				invalid,
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Hv_Ev(Code.XOP_Blci_r32_rm32, Code.XOP_Blci_r64_rm64),
						invalid
					)
				),
				invalid,
			};
			tbl[0x02] = new OpCodeHandler_Group(grp_XOP9_02);
			var grp_XOP9_12 = new OpCodeHandler[8] {
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_RdRq(Code.XOP_Llwpcb_r32, Code.XOP_Llwpcb_r64),
						invalid
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_RdRq(Code.XOP_Slwpcb_r32, Code.XOP_Slwpcb_r64),
						invalid
					)
				),
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
			};
			tbl[0x12] = new OpCodeHandler_Group(grp_XOP9_12);
			tbl[0x80] = W0L(
				new OpCodeHandler_VEX_VW(Register.XMM0, Code.XOP_Vfrczps_xmm_xmmm128),
				new OpCodeHandler_VEX_VW(Register.YMM0, Code.XOP_Vfrczps_ymm_ymmm256)
			);
			tbl[0x81] = W0L(
				new OpCodeHandler_VEX_VW(Register.XMM0, Code.XOP_Vfrczpd_xmm_xmmm128),
				new OpCodeHandler_VEX_VW(Register.YMM0, Code.XOP_Vfrczpd_ymm_ymmm256)
			);
			tbl[0x82] = W0L0(new OpCodeHandler_VEX_VW(Register.XMM0, Code.XOP_Vfrczss_xmm_xmmm32));
			tbl[0x83] = W0L0(new OpCodeHandler_VEX_VW(Register.XMM0, Code.XOP_Vfrczsd_xmm_xmmm64));
			tbl[0x90] = WL0(
				new OpCodeHandler_VEX_VWH(Register.XMM0, Code.XOP_Vprotb_xmm_xmmm128_xmm),
				new OpCodeHandler_VEX_VHW(Register.XMM0, Code.XOP_Vprotb_xmm_xmm_xmmm128)
			);
			tbl[0x91] = WL0(
				new OpCodeHandler_VEX_VWH(Register.XMM0, Code.XOP_Vprotw_xmm_xmmm128_xmm),
				new OpCodeHandler_VEX_VHW(Register.XMM0, Code.XOP_Vprotw_xmm_xmm_xmmm128)
			);
			tbl[0x92] = WL0(
				new OpCodeHandler_VEX_VWH(Register.XMM0, Code.XOP_Vprotd_xmm_xmmm128_xmm),
				new OpCodeHandler_VEX_VHW(Register.XMM0, Code.XOP_Vprotd_xmm_xmm_xmmm128)
			);
			tbl[0x93] = WL0(
				new OpCodeHandler_VEX_VWH(Register.XMM0, Code.XOP_Vprotq_xmm_xmmm128_xmm),
				new OpCodeHandler_VEX_VHW(Register.XMM0, Code.XOP_Vprotq_xmm_xmm_xmmm128)
			);
			tbl[0x94] = WL0(
				new OpCodeHandler_VEX_VWH(Register.XMM0, Code.XOP_Vpshlb_xmm_xmmm128_xmm),
				new OpCodeHandler_VEX_VHW(Register.XMM0, Code.XOP_Vpshlb_xmm_xmm_xmmm128)
			);
			tbl[0x95] = WL0(
				new OpCodeHandler_VEX_VWH(Register.XMM0, Code.XOP_Vpshlw_xmm_xmmm128_xmm),
				new OpCodeHandler_VEX_VHW(Register.XMM0, Code.XOP_Vpshlw_xmm_xmm_xmmm128)
			);
			tbl[0x96] = WL0(
				new OpCodeHandler_VEX_VWH(Register.XMM0, Code.XOP_Vpshld_xmm_xmmm128_xmm),
				new OpCodeHandler_VEX_VHW(Register.XMM0, Code.XOP_Vpshld_xmm_xmm_xmmm128)
			);
			tbl[0x97] = WL0(
				new OpCodeHandler_VEX_VWH(Register.XMM0, Code.XOP_Vpshlq_xmm_xmmm128_xmm),
				new OpCodeHandler_VEX_VHW(Register.XMM0, Code.XOP_Vpshlq_xmm_xmm_xmmm128)
			);
			tbl[0x98] = WL0(
				new OpCodeHandler_VEX_VWH(Register.XMM0, Code.XOP_Vpshab_xmm_xmmm128_xmm),
				new OpCodeHandler_VEX_VHW(Register.XMM0, Code.XOP_Vpshab_xmm_xmm_xmmm128)
			);
			tbl[0x99] = WL0(
				new OpCodeHandler_VEX_VWH(Register.XMM0, Code.XOP_Vpshaw_xmm_xmmm128_xmm),
				new OpCodeHandler_VEX_VHW(Register.XMM0, Code.XOP_Vpshaw_xmm_xmm_xmmm128)
			);
			tbl[0x9A] = WL0(
				new OpCodeHandler_VEX_VWH(Register.XMM0, Code.XOP_Vpshad_xmm_xmmm128_xmm),
				new OpCodeHandler_VEX_VHW(Register.XMM0, Code.XOP_Vpshad_xmm_xmm_xmmm128)
			);
			tbl[0x9B] = WL0(
				new OpCodeHandler_VEX_VWH(Register.XMM0, Code.XOP_Vpshaq_xmm_xmmm128_xmm),
				new OpCodeHandler_VEX_VHW(Register.XMM0, Code.XOP_Vpshaq_xmm_xmm_xmmm128)
			);
			tbl[0xC1] = W0L0(new OpCodeHandler_VEX_VW(Register.XMM0, Code.XOP_Vphaddbw_xmm_xmmm128));
			tbl[0xC2] = W0L0(new OpCodeHandler_VEX_VW(Register.XMM0, Code.XOP_Vphaddbd_xmm_xmmm128));
			tbl[0xC3] = W0L0(new OpCodeHandler_VEX_VW(Register.XMM0, Code.XOP_Vphaddbq_xmm_xmmm128));
			tbl[0xC6] = W0L0(new OpCodeHandler_VEX_VW(Register.XMM0, Code.XOP_Vphaddwd_xmm_xmmm128));
			tbl[0xC7] = W0L0(new OpCodeHandler_VEX_VW(Register.XMM0, Code.XOP_Vphaddwq_xmm_xmmm128));
			tbl[0xCB] = W0L0(new OpCodeHandler_VEX_VW(Register.XMM0, Code.XOP_Vphadddq_xmm_xmmm128));
			tbl[0xD1] = W0L0(new OpCodeHandler_VEX_VW(Register.XMM0, Code.XOP_Vphaddubw_xmm_xmmm128));
			tbl[0xD2] = W0L0(new OpCodeHandler_VEX_VW(Register.XMM0, Code.XOP_Vphaddubd_xmm_xmmm128));
			tbl[0xD3] = W0L0(new OpCodeHandler_VEX_VW(Register.XMM0, Code.XOP_Vphaddubq_xmm_xmmm128));
			tbl[0xD6] = W0L0(new OpCodeHandler_VEX_VW(Register.XMM0, Code.XOP_Vphadduwd_xmm_xmmm128));
			tbl[0xD7] = W0L0(new OpCodeHandler_VEX_VW(Register.XMM0, Code.XOP_Vphadduwq_xmm_xmmm128));
			tbl[0xDB] = W0L0(new OpCodeHandler_VEX_VW(Register.XMM0, Code.XOP_Vphaddudq_xmm_xmmm128));
			tbl[0xE1] = W0L0(new OpCodeHandler_VEX_VW(Register.XMM0, Code.XOP_Vphsubbw_xmm_xmmm128));
			tbl[0xE2] = W0L0(new OpCodeHandler_VEX_VW(Register.XMM0, Code.XOP_Vphsubwd_xmm_xmmm128));
			tbl[0xE3] = W0L0(new OpCodeHandler_VEX_VW(Register.XMM0, Code.XOP_Vphsubdq_xmm_xmmm128));
			XOP9 = tbl;

			tbl = new OpCodeHandler[0x100];
			for (int i = 0; i < tbl.Length; i++)
				tbl[i] = invalid;
			tbl[0x10] = new OpCodeHandler_MandatoryPrefix2(
				new OpCodeHandler_VectorLength_VEX(
					new OpCodeHandler_VEX_Gv_Ev_Id(Code.XOP_Bextr_r32_rm32_imm32, Code.XOP_Bextr_r64_rm64_imm32),
					invalid
				)
			);
			var grp_XOPA_12 = new OpCodeHandler[8] {
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Hv_Ed_Id(Code.XOP_Lwpins_r32_rm32_imm32, Code.XOP_Lwpins_r64_rm32_imm32),
						invalid
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Hv_Ed_Id(Code.XOP_Lwpval_r32_rm32_imm32, Code.XOP_Lwpval_r64_rm32_imm32),
						invalid
					)
				),
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
			};
			tbl[0x12] = new OpCodeHandler_Group(grp_XOPA_12);
			XOPA = tbl;
		}
	}
}
#endif
