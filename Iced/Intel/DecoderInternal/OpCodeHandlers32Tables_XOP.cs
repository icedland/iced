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
	/// Handlers for 16/32-bit mode (XOP)
	/// </summary>
	static class OpCodeHandlers32Tables_XOP {
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

		static OpCodeHandlers32Tables_XOP() {
			OpCodeHandler[] tbl;
			var invalid = OpCodeHandler_Invalid.Instance;

			tbl = new OpCodeHandler[0x100];
			for (int i = 0; i < tbl.Length; i++)
				tbl[i] = invalid;
			tbl[0x85] = W0L0(new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.XOP_Vpmacssww_VX_HX_WX_Is4X, MemorySize.Packed128_Int16));
			tbl[0x86] = W0L0(new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.XOP_Vpmacsswd_VX_HX_WX_Is4X, MemorySize.Packed128_Int16));
			tbl[0x87] = W0L0(new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.XOP_Vpmacssdql_VX_HX_WX_Is4X, MemorySize.Packed128_Int32));
			tbl[0x8E] = W0L0(new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.XOP_Vpmacssdd_VX_HX_WX_Is4X, MemorySize.Packed128_Int32));
			tbl[0x8F] = W0L0(new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.XOP_Vpmacssdqh_VX_HX_WX_Is4X, MemorySize.Packed128_Int32));
			tbl[0x95] = W0L0(new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.XOP_Vpmacsww_VX_HX_WX_Is4X, MemorySize.Packed128_Int16));
			tbl[0x96] = W0L0(new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.XOP_Vpmacswd_VX_HX_WX_Is4X, MemorySize.Packed128_Int16));
			tbl[0x97] = W0L0(new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.XOP_Vpmacsdql_VX_HX_WX_Is4X, MemorySize.Packed128_Int32));
			tbl[0x9E] = W0L0(new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.XOP_Vpmacsdd_VX_HX_WX_Is4X, MemorySize.Packed128_Int32));
			tbl[0x9F] = W0L0(new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.XOP_Vpmacsdqh_VX_HX_WX_Is4X, MemorySize.Packed128_Int32));
			tbl[0xA2] = W(
				new OpCodeHandler_VectorLength_VEX(
					new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.XOP_Vpcmov_VX_HX_WX_Is4X, MemorySize.UInt128),
					new OpCodeHandler_VEX_VHWIs4(Register.YMM0, Code.XOP_Vpcmov_VY_HY_WY_Is4Y, MemorySize.UInt256)
				),
				new OpCodeHandler_VectorLength_VEX(
					new OpCodeHandler_VEX_VHIs4W(Register.XMM0, Code.XOP_Vpcmov_VX_HX_Is4X_WX, MemorySize.UInt128),
					new OpCodeHandler_VEX_VHIs4W(Register.YMM0, Code.XOP_Vpcmov_VY_HY_Is4Y_WY, MemorySize.UInt256)
				)
			);
			tbl[0xA3] = W(
				new OpCodeHandler_VectorLength_VEX(
					new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.XOP_Vpperm_VX_HX_WX_Is4X, MemorySize.Packed128_UInt8),
					invalid
				),
				new OpCodeHandler_VectorLength_VEX(
					new OpCodeHandler_VEX_VHIs4W(Register.XMM0, Code.XOP_Vpperm_VX_HX_Is4X_WX, MemorySize.Packed128_UInt8),
					invalid
				)
			);
			tbl[0xA6] = W0L0(new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.XOP_Vpmadcsswd_VX_HX_WX_Is4X, MemorySize.Packed128_Int16));
			tbl[0xB6] = W0L0(new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.XOP_Vpmadcswd_VX_HX_WX_Is4X, MemorySize.Packed128_Int16));
			tbl[0xC0] = W0L0(new OpCodeHandler_VEX_VWIb(Register.XMM0, Code.XOP_Vprotb_VX_WX_Ib, MemorySize.Packed128_UInt8));
			tbl[0xC1] = W0L0(new OpCodeHandler_VEX_VWIb(Register.XMM0, Code.XOP_Vprotw_VX_WX_Ib, MemorySize.Packed128_UInt16));
			tbl[0xC2] = W0L0(new OpCodeHandler_VEX_VWIb(Register.XMM0, Code.XOP_Vprotd_VX_WX_Ib, MemorySize.Packed128_UInt32));
			tbl[0xC3] = W0L0(new OpCodeHandler_VEX_VWIb(Register.XMM0, Code.XOP_Vprotq_VX_WX_Ib, MemorySize.Packed128_UInt64));
			tbl[0xCC] = W0L0(new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.XOP_Vpcomb_VX_HX_WX_Ib, MemorySize.Packed128_Int8));
			tbl[0xCD] = W0L0(new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.XOP_Vpcomw_VX_HX_WX_Ib, MemorySize.Packed128_Int16));
			tbl[0xCE] = W0L0(new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.XOP_Vpcomd_VX_HX_WX_Ib, MemorySize.Packed128_Int32));
			tbl[0xCF] = W0L0(new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.XOP_Vpcomq_VX_HX_WX_Ib, MemorySize.Packed128_Int64));
			tbl[0xEC] = W0L0(new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.XOP_Vpcomub_VX_HX_WX_Ib, MemorySize.Packed128_UInt8));
			tbl[0xED] = W0L0(new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.XOP_Vpcomuw_VX_HX_WX_Ib, MemorySize.Packed128_UInt16));
			tbl[0xEE] = W0L0(new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.XOP_Vpcomud_VX_HX_WX_Ib, MemorySize.Packed128_UInt32));
			tbl[0xEF] = W0L0(new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.XOP_Vpcomuq_VX_HX_WX_Ib, MemorySize.Packed128_UInt64));
			XOP8 = tbl;

			tbl = new OpCodeHandler[0x100];
			for (int i = 0; i < tbl.Length; i++)
				tbl[i] = invalid;
			var grp_XOP9_01 = new OpCodeHandler[8] {
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Hv_Ev(Code.XOP_Blcfill_Hd_Ed, Code.XOP_Blcfill_Hq_Eq),
						invalid
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Hv_Ev(Code.XOP_Blsfill_Hd_Ed, Code.XOP_Blsfill_Hq_Eq),
						invalid
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Hv_Ev(Code.XOP_Blcs_Hd_Ed, Code.XOP_Blcs_Hq_Eq),
						invalid
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Hv_Ev(Code.XOP_Tzmsk_Hd_Ed, Code.XOP_Tzmsk_Hq_Eq),
						invalid
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Hv_Ev(Code.XOP_Blcic_Hd_Ed, Code.XOP_Blcic_Hq_Eq),
						invalid
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Hv_Ev(Code.XOP_Blsic_Hd_Ed, Code.XOP_Blsic_Hq_Eq),
						invalid
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Hv_Ev(Code.XOP_T1mskc_Hd_Ed, Code.XOP_T1mskc_Hq_Eq),
						invalid
					)
				),
			};
			tbl[0x01] = new OpCodeHandler_Group(grp_XOP9_01);
			var grp_XOP9_02 = new OpCodeHandler[8] {
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Hv_Ev(Code.XOP_Blcmsk_Hd_Ed, Code.XOP_Blcmsk_Hq_Eq),
						invalid
					)
				),
				invalid,
				invalid,
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Hv_Ev(Code.XOP_Blci_Hd_Ed, Code.XOP_Blci_Hq_Eq),
						invalid
					)
				),
				invalid,
			};
			tbl[0x02] = new OpCodeHandler_Group(grp_XOP9_02);
			var grp_XOP9_12 = new OpCodeHandler[8] {
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_RdRq(Code.XOP_Llwpcb_Rd, Code.XOP_Llwpcb_Rq),
						invalid
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_RdRq(Code.XOP_Slwpcb_Rd, Code.XOP_Slwpcb_Rq),
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
				new OpCodeHandler_VEX_VW(Register.XMM0, Code.XOP_Vfrczps_VX_WX, MemorySize.Packed128_Float32),
				new OpCodeHandler_VEX_VW(Register.YMM0, Code.XOP_Vfrczps_VY_WY, MemorySize.Packed256_Float32)
			);
			tbl[0x81] = W0L(
				new OpCodeHandler_VEX_VW(Register.XMM0, Code.XOP_Vfrczpd_VX_WX, MemorySize.Packed128_Float64),
				new OpCodeHandler_VEX_VW(Register.YMM0, Code.XOP_Vfrczpd_VY_WY, MemorySize.Packed256_Float64)
			);
			tbl[0x82] = W0L0(new OpCodeHandler_VEX_VW(Register.XMM0, Code.XOP_Vfrczss_VX_WX, MemorySize.Float32));
			tbl[0x83] = W0L0(new OpCodeHandler_VEX_VW(Register.XMM0, Code.XOP_Vfrczsd_VX_WX, MemorySize.Float64));
			tbl[0x90] = WL0(
				new OpCodeHandler_VEX_VWH(Register.XMM0, Code.XOP_Vprotb_VX_WX_HX, MemorySize.Packed128_UInt8),
				new OpCodeHandler_VEX_VHW(Register.XMM0, Code.XOP_Vprotb_VX_HX_WX, MemorySize.Packed128_UInt8)
			);
			tbl[0x91] = WL0(
				new OpCodeHandler_VEX_VWH(Register.XMM0, Code.XOP_Vprotw_VX_WX_HX, MemorySize.Packed128_UInt16),
				new OpCodeHandler_VEX_VHW(Register.XMM0, Code.XOP_Vprotw_VX_HX_WX, MemorySize.Packed128_UInt16)
			);
			tbl[0x92] = WL0(
				new OpCodeHandler_VEX_VWH(Register.XMM0, Code.XOP_Vprotd_VX_WX_HX, MemorySize.Packed128_UInt32),
				new OpCodeHandler_VEX_VHW(Register.XMM0, Code.XOP_Vprotd_VX_HX_WX, MemorySize.Packed128_UInt32)
			);
			tbl[0x93] = WL0(
				new OpCodeHandler_VEX_VWH(Register.XMM0, Code.XOP_Vprotq_VX_WX_HX, MemorySize.Packed128_UInt64),
				new OpCodeHandler_VEX_VHW(Register.XMM0, Code.XOP_Vprotq_VX_HX_WX, MemorySize.Packed128_UInt64)
			);
			tbl[0x94] = WL0(
				new OpCodeHandler_VEX_VWH(Register.XMM0, Code.XOP_Vpshlb_VX_WX_HX, MemorySize.Packed128_UInt8),
				new OpCodeHandler_VEX_VHW(Register.XMM0, Code.XOP_Vpshlb_VX_HX_WX, MemorySize.Packed128_UInt8)
			);
			tbl[0x95] = WL0(
				new OpCodeHandler_VEX_VWH(Register.XMM0, Code.XOP_Vpshlw_VX_WX_HX, MemorySize.Packed128_UInt16),
				new OpCodeHandler_VEX_VHW(Register.XMM0, Code.XOP_Vpshlw_VX_HX_WX, MemorySize.Packed128_UInt16)
			);
			tbl[0x96] = WL0(
				new OpCodeHandler_VEX_VWH(Register.XMM0, Code.XOP_Vpshld_VX_WX_HX, MemorySize.Packed128_UInt32),
				new OpCodeHandler_VEX_VHW(Register.XMM0, Code.XOP_Vpshld_VX_HX_WX, MemorySize.Packed128_UInt32)
			);
			tbl[0x97] = WL0(
				new OpCodeHandler_VEX_VWH(Register.XMM0, Code.XOP_Vpshlq_VX_WX_HX, MemorySize.Packed128_UInt64),
				new OpCodeHandler_VEX_VHW(Register.XMM0, Code.XOP_Vpshlq_VX_HX_WX, MemorySize.Packed128_UInt64)
			);
			tbl[0x98] = WL0(
				new OpCodeHandler_VEX_VWH(Register.XMM0, Code.XOP_Vpshab_VX_WX_HX, MemorySize.Packed128_Int8),
				new OpCodeHandler_VEX_VHW(Register.XMM0, Code.XOP_Vpshab_VX_HX_WX, MemorySize.Packed128_Int8)
			);
			tbl[0x99] = WL0(
				new OpCodeHandler_VEX_VWH(Register.XMM0, Code.XOP_Vpshaw_VX_WX_HX, MemorySize.Packed128_Int16),
				new OpCodeHandler_VEX_VHW(Register.XMM0, Code.XOP_Vpshaw_VX_HX_WX, MemorySize.Packed128_Int16)
			);
			tbl[0x9A] = WL0(
				new OpCodeHandler_VEX_VWH(Register.XMM0, Code.XOP_Vpshad_VX_WX_HX, MemorySize.Packed128_Int32),
				new OpCodeHandler_VEX_VHW(Register.XMM0, Code.XOP_Vpshad_VX_HX_WX, MemorySize.Packed128_Int32)
			);
			tbl[0x9B] = WL0(
				new OpCodeHandler_VEX_VWH(Register.XMM0, Code.XOP_Vpshaq_VX_WX_HX, MemorySize.Packed128_Int64),
				new OpCodeHandler_VEX_VHW(Register.XMM0, Code.XOP_Vpshaq_VX_HX_WX, MemorySize.Packed128_Int64)
			);
			tbl[0xC1] = W0L0(new OpCodeHandler_VEX_VW(Register.XMM0, Code.XOP_Vphaddbw_VX_WX, MemorySize.Packed128_Int8));
			tbl[0xC2] = W0L0(new OpCodeHandler_VEX_VW(Register.XMM0, Code.XOP_Vphaddbd_VX_WX, MemorySize.Packed128_Int8));
			tbl[0xC3] = W0L0(new OpCodeHandler_VEX_VW(Register.XMM0, Code.XOP_Vphaddbq_VX_WX, MemorySize.Packed128_Int8));
			tbl[0xC6] = W0L0(new OpCodeHandler_VEX_VW(Register.XMM0, Code.XOP_Vphaddwd_VX_WX, MemorySize.Packed128_Int16));
			tbl[0xC7] = W0L0(new OpCodeHandler_VEX_VW(Register.XMM0, Code.XOP_Vphaddwq_VX_WX, MemorySize.Packed128_Int16));
			tbl[0xCB] = W0L0(new OpCodeHandler_VEX_VW(Register.XMM0, Code.XOP_Vphadddq_VX_WX, MemorySize.Packed128_Int32));
			tbl[0xD1] = W0L0(new OpCodeHandler_VEX_VW(Register.XMM0, Code.XOP_Vphaddubw_VX_WX, MemorySize.Packed128_UInt8));
			tbl[0xD2] = W0L0(new OpCodeHandler_VEX_VW(Register.XMM0, Code.XOP_Vphaddubd_VX_WX, MemorySize.Packed128_UInt8));
			tbl[0xD3] = W0L0(new OpCodeHandler_VEX_VW(Register.XMM0, Code.XOP_Vphaddubq_VX_WX, MemorySize.Packed128_UInt8));
			tbl[0xD6] = W0L0(new OpCodeHandler_VEX_VW(Register.XMM0, Code.XOP_Vphadduwd_VX_WX, MemorySize.Packed128_UInt16));
			tbl[0xD7] = W0L0(new OpCodeHandler_VEX_VW(Register.XMM0, Code.XOP_Vphadduwq_VX_WX, MemorySize.Packed128_UInt16));
			tbl[0xDB] = W0L0(new OpCodeHandler_VEX_VW(Register.XMM0, Code.XOP_Vphaddudq_VX_WX, MemorySize.Packed128_UInt32));
			tbl[0xE1] = W0L0(new OpCodeHandler_VEX_VW(Register.XMM0, Code.XOP_Vphsubbw_VX_WX, MemorySize.Packed128_Int8));
			tbl[0xE2] = W0L0(new OpCodeHandler_VEX_VW(Register.XMM0, Code.XOP_Vphsubwd_VX_WX, MemorySize.Packed128_Int16));
			tbl[0xE3] = W0L0(new OpCodeHandler_VEX_VW(Register.XMM0, Code.XOP_Vphsubdq_VX_WX, MemorySize.Packed128_Int32));
			XOP9 = tbl;

			tbl = new OpCodeHandler[0x100];
			for (int i = 0; i < tbl.Length; i++)
				tbl[i] = invalid;
			tbl[0x10] = new OpCodeHandler_MandatoryPrefix2(
				new OpCodeHandler_VectorLength_VEX(
					new OpCodeHandler_VEX_Gv_Ev_Id(Code.XOP_Bextr_Gd_Ed_Id, Code.XOP_Bextr_Gq_Eq_Id),
					invalid
				)
			);
			var grp_XOPA_12 = new OpCodeHandler[8] {
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Hv_Ed_Id(Code.XOP_Lwpins_Hd_Ed_Id, Code.XOP_Lwpins_Hq_Ed_Id),
						invalid
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Hv_Ed_Id(Code.XOP_Lwpval_Hd_Ed_Id, Code.XOP_Lwpval_Hq_Ed_Id),
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
