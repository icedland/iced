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
	/// Handlers for 16/32-bit mode (VEX)
	/// </summary>
	static class OpCodeHandlers32Tables_VEX {
		internal static readonly OpCodeHandler[] ThreeByteHandlers_0F38XX;
		internal static readonly OpCodeHandler[] ThreeByteHandlers_0F3AXX;
		internal static readonly OpCodeHandler[] TwoByteHandlers_0FXX;

		static OpCodeHandlers32Tables_VEX() {
			// Store it in a local. Instead of 1400+ ldfld, we'll have 1400+ ldloc.0 (save 4 bytes per load)
			var invalid = OpCodeHandler_Invalid.Instance;

			var handlers_Grp_0F71 = new OpCodeHandler[8] {
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_HRIb(Register.XMM0, Code.VEX_Vpsrlw_HX_RX_Ib),
						new OpCodeHandler_VEX_HRIb(Register.YMM0, Code.VEX_Vpsrlw_HY_RY_Ib)
					),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_HRIb(Register.XMM0, Code.VEX_Vpsraw_HX_RX_Ib),
						new OpCodeHandler_VEX_HRIb(Register.YMM0, Code.VEX_Vpsraw_HY_RY_Ib)
					),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_HRIb(Register.XMM0, Code.VEX_Vpsllw_HX_RX_Ib),
						new OpCodeHandler_VEX_HRIb(Register.YMM0, Code.VEX_Vpsllw_HY_RY_Ib)
					),
					invalid,
					invalid
				),
				invalid,
			};

			var handlers_Grp_0F72 = new OpCodeHandler[8] {
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_HRIb(Register.XMM0, Code.VEX_Vpsrld_HX_RX_Ib),
						new OpCodeHandler_VEX_HRIb(Register.YMM0, Code.VEX_Vpsrld_HY_RY_Ib)
					),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_HRIb(Register.XMM0, Code.VEX_Vpsrad_HX_RX_Ib),
						new OpCodeHandler_VEX_HRIb(Register.YMM0, Code.VEX_Vpsrad_HY_RY_Ib)
					),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_HRIb(Register.XMM0, Code.VEX_Vpslld_HX_RX_Ib),
						new OpCodeHandler_VEX_HRIb(Register.YMM0, Code.VEX_Vpslld_HY_RY_Ib)
					),
					invalid,
					invalid
				),
				invalid,
			};

			var handlers_Grp_0F73 = new OpCodeHandler[8] {
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_HRIb(Register.XMM0, Code.VEX_Vpsrlq_HX_RX_Ib),
						new OpCodeHandler_VEX_HRIb(Register.YMM0, Code.VEX_Vpsrlq_HY_RY_Ib)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_HRIb(Register.XMM0, Code.VEX_Vpsrldq_HX_RX_Ib),
						new OpCodeHandler_VEX_HRIb(Register.YMM0, Code.VEX_Vpsrldq_HY_RY_Ib)
					),
					invalid,
					invalid
				),
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_HRIb(Register.XMM0, Code.VEX_Vpsllq_HX_RX_Ib),
						new OpCodeHandler_VEX_HRIb(Register.YMM0, Code.VEX_Vpsllq_HY_RY_Ib)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_HRIb(Register.XMM0, Code.VEX_Vpslldq_HX_RX_Ib),
						new OpCodeHandler_VEX_HRIb(Register.YMM0, Code.VEX_Vpslldq_HY_RY_Ib)
					),
					invalid,
					invalid
				),
			};

			var handlers_Grp_0FAE = new OpCodeHandler[8] {
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_M(Code.VEX_Vldmxcsr_Md, MemorySize.UInt32),
						invalid
					),
					invalid,
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_M(Code.VEX_Vstmxcsr_Md, MemorySize.UInt32),
						invalid
					),
					invalid,
					invalid,
					invalid
				),
				invalid,
				invalid,
				invalid,
				invalid,
			};

			var handlers_Grp_0F38F3 = new OpCodeHandler[8] {
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Gv_Ev(Code.VEX_Blsr_Hd_Ed, Code.VEX_Blsr_Hq_Eq),
						invalid
					),
					invalid,
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Gv_Ev(Code.VEX_Blsmsk_Hd_Ed, Code.VEX_Blsmsk_Hq_Eq),
						invalid
					),
					invalid,
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Gv_Ev(Code.VEX_Blsi_Hd_Ed, Code.VEX_Blsi_Hq_Eq),
						invalid
					),
					invalid,
					invalid,
					invalid
				),
				invalid,
				invalid,
				invalid,
				invalid,
			};

			ThreeByteHandlers_0F38XX = new OpCodeHandler[0x100] {
				// 00
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpshufb_VX_HX_WX, MemorySize.Packed128_UInt8),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpshufb_VY_HY_WY, MemorySize.Packed256_UInt8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vphaddw_VX_HX_WX, MemorySize.Packed128_Int16),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vphaddw_VY_HY_WY, MemorySize.Packed256_Int16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vphaddd_VX_HX_WX, MemorySize.Packed128_Int32),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vphaddd_VY_HY_WY, MemorySize.Packed256_Int32)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vphaddsw_VX_HX_WX, MemorySize.Packed128_Int16),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vphaddsw_VY_HY_WY, MemorySize.Packed256_Int16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpmaddubsw_VX_HX_WX, MemorySize.Packed128_Int8),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpmaddubsw_VY_HY_WY, MemorySize.Packed256_Int8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vphsubw_VX_HX_WX, MemorySize.Packed128_Int16),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vphsubw_VY_HY_WY, MemorySize.Packed256_Int16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vphsubd_VX_HX_WX, MemorySize.Packed128_Int32),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vphsubd_VY_HY_WY, MemorySize.Packed256_Int32)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vphsubsw_VX_HX_WX, MemorySize.Packed128_Int16),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vphsubsw_VY_HY_WY, MemorySize.Packed256_Int16)
					),
					invalid,
					invalid
				),

				// 08
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpsignb_VX_HX_WX, MemorySize.Packed128_Int8),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpsignb_VY_HY_WY, MemorySize.Packed256_Int8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpsignw_VX_HX_WX, MemorySize.Packed128_Int16),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpsignw_VY_HY_WY, MemorySize.Packed256_Int16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpsignd_VX_HX_WX, MemorySize.Packed128_Int32),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpsignd_VY_HY_WY, MemorySize.Packed256_Int32)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpmulhrsw_VX_HX_WX, MemorySize.Packed128_Int16),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpmulhrsw_VY_HY_WY, MemorySize.Packed256_Int16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpermilps_VX_HX_WX, MemorySize.Packed128_Float32),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpermilps_VY_HY_WY, MemorySize.Packed256_Float32)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpermilpd_VX_HX_WX, MemorySize.Packed128_Float64),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpermilpd_VY_HY_WY, MemorySize.Packed256_Float64)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vtestps_VX_WX, MemorySize.Packed128_Float32),
							new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vtestps_VY_WY, MemorySize.Packed256_Float32)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vtestpd_VX_WX, MemorySize.Packed128_Float64),
							new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vtestpd_VY_WY, MemorySize.Packed256_Float64)
						),
						invalid
					),
					invalid,
					invalid
				),

				// 10
				invalid,
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VW(Register.XMM0, Register.XMM0, Code.VEX_Vcvtph2ps_VX_WX, MemorySize.Packed64_Float16),
							new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vcvtph2ps_VY_WX, MemorySize.Packed128_Float16)
						),
						invalid
					),
					invalid,
					invalid
				),
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							invalid,
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpermps_VY_HY_WY, MemorySize.Packed256_Float32)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vptest_VX_WX, MemorySize.UInt128),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vptest_VY_WY, MemorySize.UInt256)
					),
					invalid,
					invalid
				),

				// 18
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VW(Register.XMM0, Register.XMM0, Code.VEX_Vbroadcastss_VX_WX, MemorySize.Float32),
							new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vbroadcastss_VY_WX, MemorySize.Float32)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							invalid,
							new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vbroadcastsd_VY_WX, MemorySize.Float64)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							invalid,
							new OpCodeHandler_VEX_VM(Register.YMM0, Code.VEX_Vbroadcastf128_VY_M, MemorySize.Float128)
						),
						invalid
					),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vpabsb_VX_WX, MemorySize.Packed128_Int8),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vpabsb_VY_WY, MemorySize.Packed256_Int8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vpabsw_VX_WX, MemorySize.Packed128_Int16),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vpabsw_VY_WY, MemorySize.Packed256_Int16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vpabsd_VX_WX, MemorySize.Packed128_Int32),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vpabsd_VY_WY, MemorySize.Packed256_Int32)
					),
					invalid,
					invalid
				),
				invalid,

				// 20
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Register.XMM0, Code.VEX_Vpmovsxbw_VX_WX, MemorySize.Packed64_Int8),
						new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vpmovsxbw_VY_WX, MemorySize.Packed128_Int8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Register.XMM0, Code.VEX_Vpmovsxbd_VX_WX, MemorySize.Packed32_Int8),
						new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vpmovsxbd_VY_WX, MemorySize.Packed64_Int8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Register.XMM0, Code.VEX_Vpmovsxbq_VX_WX, MemorySize.Packed16_Int8),
						new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vpmovsxbq_VY_WX, MemorySize.Packed32_Int8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Register.XMM0, Code.VEX_Vpmovsxwd_VX_WX, MemorySize.Packed64_Int16),
						new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vpmovsxwd_VY_WX, MemorySize.Packed128_Int16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Register.XMM0, Code.VEX_Vpmovsxwq_VX_WX, MemorySize.Packed32_Int16),
						new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vpmovsxwq_VY_WX, MemorySize.Packed64_Int16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Register.XMM0, Code.VEX_Vpmovsxdq_VX_WX, MemorySize.Packed64_Int32),
						new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vpmovsxdq_VY_WX, MemorySize.Packed128_Int32)
					),
					invalid,
					invalid
				),
				invalid,
				invalid,

				// 28
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpmuldq_VX_HX_WX, MemorySize.Packed128_Int32),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpmuldq_VY_HY_WY, MemorySize.Packed256_Int32)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpcmpeqq_VX_HX_WX, MemorySize.Packed128_Int64),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpcmpeqq_VY_HY_WY, MemorySize.Packed256_Int64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VM(Register.XMM0, Code.VEX_Vmovntdqa_VX_M, MemorySize.UInt128),
						new OpCodeHandler_VEX_VM(Register.YMM0, Code.VEX_Vmovntdqa_VY_M, MemorySize.UInt256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpackusdw_VX_HX_WX, MemorySize.Packed128_Int32),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpackusdw_VY_HY_WY, MemorySize.Packed256_Int32)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHM(Register.XMM0, Code.VEX_Vmaskmovps_VX_HX_M, MemorySize.Packed128_Float32),
							new OpCodeHandler_VEX_VHM(Register.YMM0, Code.VEX_Vmaskmovps_VY_HY_M, MemorySize.Packed256_Float32)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHM(Register.XMM0, Code.VEX_Vmaskmovpd_VX_HX_M, MemorySize.Packed128_Float64),
							new OpCodeHandler_VEX_VHM(Register.YMM0, Code.VEX_Vmaskmovpd_VY_HY_M, MemorySize.Packed256_Float64)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_MHV(Register.XMM0, Code.VEX_Vmaskmovps_M_HX_VX, MemorySize.Packed128_Float32),
							new OpCodeHandler_VEX_MHV(Register.YMM0, Code.VEX_Vmaskmovps_M_HY_VY, MemorySize.Packed256_Float32)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_MHV(Register.XMM0, Code.VEX_Vmaskmovpd_M_HX_VX, MemorySize.Packed128_Float64),
							new OpCodeHandler_VEX_MHV(Register.YMM0, Code.VEX_Vmaskmovpd_M_HY_VY, MemorySize.Packed256_Float64)
						),
						invalid
					),
					invalid,
					invalid
				),

				// 30
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Register.XMM0, Code.VEX_Vpmovzxbw_VX_WX, MemorySize.Packed64_UInt8),
						new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vpmovzxbw_VY_WX, MemorySize.Packed128_UInt8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Register.XMM0, Code.VEX_Vpmovzxbd_VX_WX, MemorySize.Packed32_UInt8),
						new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vpmovzxbd_VY_WX, MemorySize.Packed64_UInt8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Register.XMM0, Code.VEX_Vpmovzxbq_VX_WX, MemorySize.Packed16_UInt8),
						new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vpmovzxbq_VY_WX, MemorySize.Packed32_UInt8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Register.XMM0, Code.VEX_Vpmovzxwd_VX_WX, MemorySize.Packed64_UInt16),
						new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vpmovzxwd_VY_WX, MemorySize.Packed128_UInt16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Register.XMM0, Code.VEX_Vpmovzxwq_VX_WX, MemorySize.Packed32_UInt16),
						new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vpmovzxwq_VY_WX, MemorySize.Packed64_UInt16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Register.XMM0, Code.VEX_Vpmovzxdq_VX_WX, MemorySize.Packed64_UInt32),
						new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vpmovzxdq_VY_WX, MemorySize.Packed128_UInt32)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							invalid,
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpermd_VY_HY_WY, MemorySize.Packed256_Int32)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpcmpgtq_VX_HX_WX, MemorySize.Packed128_Int64),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpcmpgtq_VY_HY_WY, MemorySize.Packed256_Int64)
					),
					invalid,
					invalid
				),

				// 38
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpminsb_VX_HX_WX, MemorySize.Packed128_Int8),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpminsb_VY_HY_WY, MemorySize.Packed256_Int8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpminsd_VX_HX_WX, MemorySize.Packed128_Int32),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpminsd_VY_HY_WY, MemorySize.Packed256_Int32)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpminuw_VX_HX_WX, MemorySize.Packed128_UInt16),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpminuw_VY_HY_WY, MemorySize.Packed256_UInt16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpminud_VX_HX_WX, MemorySize.Packed128_UInt32),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpminud_VY_HY_WY, MemorySize.Packed256_UInt32)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpmaxsb_VX_HX_WX, MemorySize.Packed128_Int8),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpmaxsb_VY_HY_WY, MemorySize.Packed256_Int8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpmaxsd_VX_HX_WX, MemorySize.Packed128_Int32),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpmaxsd_VY_HY_WY, MemorySize.Packed256_Int32)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpmaxuw_VX_HX_WX, MemorySize.Packed128_UInt16),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpmaxuw_VY_HY_WY, MemorySize.Packed256_UInt16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpmaxud_VX_HX_WX, MemorySize.Packed128_UInt32),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpmaxud_VY_HY_WY, MemorySize.Packed256_UInt32)
					),
					invalid,
					invalid
				),

				// 40
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpmulld_VX_HX_WX, MemorySize.Packed128_Int32),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpmulld_VY_HY_WY, MemorySize.Packed256_Int32)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vphminposuw_VX_WX, MemorySize.Packed128_UInt16),
					invalid,
					invalid
				),
				invalid,
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpsrlvd_VX_HX_WX, MemorySize.Packed128_UInt32),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpsrlvd_VY_HY_WY, MemorySize.Packed256_UInt32)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpsrlvq_VX_HX_WX, MemorySize.Packed128_UInt64),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpsrlvq_VY_HY_WY, MemorySize.Packed256_UInt64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpsravd_VX_HX_WX, MemorySize.Packed128_UInt32),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpsravd_VY_HY_WY, MemorySize.Packed256_UInt32)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpsllvd_VX_HX_WX, MemorySize.Packed128_UInt32),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpsllvd_VY_HY_WY, MemorySize.Packed256_UInt32)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpsllvq_VX_HX_WX, MemorySize.Packed128_UInt64),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpsllvq_VY_HY_WY, MemorySize.Packed256_UInt64)
						)
					),
					invalid,
					invalid
				),

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
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VW(Register.XMM0, Register.XMM0, Code.VEX_Vpbroadcastd_VX_WX, MemorySize.Int32),
							new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vpbroadcastd_VY_WX, MemorySize.Int32)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VW(Register.XMM0, Register.XMM0, Code.VEX_Vpbroadcastq_VX_WX, MemorySize.Int64),
							new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vpbroadcastq_VY_WX, MemorySize.Int64)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							invalid,
							new OpCodeHandler_VEX_VM(Register.YMM0, Code.VEX_Vbroadcasti128_VY_M, MemorySize.Int128)
						),
						invalid
					),
					invalid,
					invalid
				),
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
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VW(Register.XMM0, Register.XMM0, Code.VEX_Vpbroadcastb_VX_WX, MemorySize.Int8),
							new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vpbroadcastb_VY_WX, MemorySize.Int8)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VW(Register.XMM0, Register.XMM0, Code.VEX_Vpbroadcastw_VX_WX, MemorySize.Int16),
							new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vpbroadcastw_VY_WX, MemorySize.Int16)
						),
						invalid
					),
					invalid,
					invalid
				),
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
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHM(Register.XMM0, Code.VEX_Vpmaskmovd_VX_HX_M, MemorySize.Packed128_Int32),
							new OpCodeHandler_VEX_VHM(Register.YMM0, Code.VEX_Vpmaskmovd_VY_HY_M, MemorySize.Packed256_Int32)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHM(Register.XMM0, Code.VEX_Vpmaskmovq_VX_HX_M, MemorySize.Packed128_Int64),
							new OpCodeHandler_VEX_VHM(Register.YMM0, Code.VEX_Vpmaskmovq_VY_HY_M, MemorySize.Packed256_Int64)
						)
					),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_MHV(Register.XMM0, Code.VEX_Vpmaskmovd_M_HX_VX, MemorySize.Packed128_Int32),
							new OpCodeHandler_VEX_MHV(Register.YMM0, Code.VEX_Vpmaskmovd_M_HY_VY, MemorySize.Packed256_Int32)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_MHV(Register.XMM0, Code.VEX_Vpmaskmovq_M_HX_VX, MemorySize.Packed128_Int64),
							new OpCodeHandler_VEX_MHV(Register.YMM0, Code.VEX_Vpmaskmovq_M_HY_VY, MemorySize.Packed256_Int64)
						)
					),
					invalid,
					invalid
				),
				invalid,

				// 90
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VX_VSIB_HX(Register.XMM0, Register.XMM0, Register.XMM0, Code.VEX_Vpgatherdd_VX_VM32X_HX, MemorySize.Int32),
							new OpCodeHandler_VEX_VX_VSIB_HX(Register.YMM0, Register.YMM0, Register.YMM0, Code.VEX_Vpgatherdd_VY_VM32Y_HY, MemorySize.Int32)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VX_VSIB_HX(Register.XMM0, Register.XMM0, Register.XMM0, Code.VEX_Vpgatherdq_VX_VM32X_HX, MemorySize.Int64),
							new OpCodeHandler_VEX_VX_VSIB_HX(Register.YMM0, Register.XMM0, Register.YMM0, Code.VEX_Vpgatherdq_VY_VM32X_HY, MemorySize.Int64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VX_VSIB_HX(Register.XMM0, Register.XMM0, Register.XMM0, Code.VEX_Vpgatherqd_VX_VM64X_HX, MemorySize.Int32),
							new OpCodeHandler_VEX_VX_VSIB_HX(Register.XMM0, Register.YMM0, Register.XMM0, Code.VEX_Vpgatherqd_VX_VM64Y_HX, MemorySize.Int32)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VX_VSIB_HX(Register.XMM0, Register.XMM0, Register.XMM0, Code.VEX_Vpgatherqq_VX_VM64X_HX, MemorySize.Int64),
							new OpCodeHandler_VEX_VX_VSIB_HX(Register.YMM0, Register.YMM0, Register.YMM0, Code.VEX_Vpgatherqq_VY_VM64Y_HY, MemorySize.Int64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VX_VSIB_HX(Register.XMM0, Register.XMM0, Register.XMM0, Code.VEX_Vgatherdps_VX_VM32X_HX, MemorySize.Float32),
							new OpCodeHandler_VEX_VX_VSIB_HX(Register.YMM0, Register.YMM0, Register.YMM0, Code.VEX_Vgatherdps_VY_VM32Y_HY, MemorySize.Float32)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VX_VSIB_HX(Register.XMM0, Register.XMM0, Register.XMM0, Code.VEX_Vgatherdpd_VX_VM32X_HX, MemorySize.Float64),
							new OpCodeHandler_VEX_VX_VSIB_HX(Register.YMM0, Register.XMM0, Register.YMM0, Code.VEX_Vgatherdpd_VY_VM32X_HY, MemorySize.Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VX_VSIB_HX(Register.XMM0, Register.XMM0, Register.XMM0, Code.VEX_Vgatherqps_VX_VM64X_HX, MemorySize.Float32),
							new OpCodeHandler_VEX_VX_VSIB_HX(Register.XMM0, Register.YMM0, Register.XMM0, Code.VEX_Vgatherqps_VX_VM64Y_HX, MemorySize.Float32)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VX_VSIB_HX(Register.XMM0, Register.XMM0, Register.XMM0, Code.VEX_Vgatherqpd_VX_VM64X_HX, MemorySize.Float64),
							new OpCodeHandler_VEX_VX_VSIB_HX(Register.YMM0, Register.YMM0, Register.YMM0, Code.VEX_Vgatherqpd_VY_VM64Y_HY, MemorySize.Float64)
						)
					),
					invalid,
					invalid
				),
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmaddsub132ps_VX_HX_WX, MemorySize.Packed128_Float32),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmaddsub132ps_VY_HY_WY, MemorySize.Packed256_Float32)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmaddsub132pd_VX_HX_WX, MemorySize.Packed128_Float64),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmaddsub132pd_VY_HY_WY, MemorySize.Packed256_Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmsubadd132ps_VX_HX_WX, MemorySize.Packed128_Float32),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmsubadd132ps_VY_HY_WY, MemorySize.Packed256_Float32)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmsubadd132pd_VX_HX_WX, MemorySize.Packed128_Float64),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmsubadd132pd_VY_HY_WY, MemorySize.Packed256_Float64)
						)
					),
					invalid,
					invalid
				),

				// 98
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmadd132ps_VX_HX_WX, MemorySize.Packed128_Float32),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmadd132ps_VY_HY_WY, MemorySize.Packed256_Float32)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmadd132pd_VX_HX_WX, MemorySize.Packed128_Float64),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmadd132pd_VY_HY_WY, MemorySize.Packed256_Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmadd132ss_VX_HX_WX, MemorySize.Float32),
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmadd132sd_VX_HX_WX, MemorySize.Float64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmsub132ps_VX_HX_WX, MemorySize.Packed128_Float32),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmsub132ps_VY_HY_WY, MemorySize.Packed256_Float32)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmsub132pd_VX_HX_WX, MemorySize.Packed128_Float64),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmsub132pd_VY_HY_WY, MemorySize.Packed256_Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmsub132ss_VX_HX_WX, MemorySize.Float32),
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmsub132sd_VX_HX_WX, MemorySize.Float64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmadd132ps_VX_HX_WX, MemorySize.Packed128_Float32),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfnmadd132ps_VY_HY_WY, MemorySize.Packed256_Float32)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmadd132pd_VX_HX_WX, MemorySize.Packed128_Float64),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfnmadd132pd_VY_HY_WY, MemorySize.Packed256_Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmadd132ss_VX_HX_WX, MemorySize.Float32),
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmadd132sd_VX_HX_WX, MemorySize.Float64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmsub132ps_VX_HX_WX, MemorySize.Packed128_Float32),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfnmsub132ps_VY_HY_WY, MemorySize.Packed256_Float32)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmsub132pd_VX_HX_WX, MemorySize.Packed128_Float64),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfnmsub132pd_VY_HY_WY, MemorySize.Packed256_Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmsub132ss_VX_HX_WX, MemorySize.Float32),
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmsub132sd_VX_HX_WX, MemorySize.Float64)
					),
					invalid,
					invalid
				),

				// A0
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmaddsub213ps_VX_HX_WX, MemorySize.Packed128_Float32),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmaddsub213ps_VY_HY_WY, MemorySize.Packed256_Float32)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmaddsub213pd_VX_HX_WX, MemorySize.Packed128_Float64),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmaddsub213pd_VY_HY_WY, MemorySize.Packed256_Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmsubadd213ps_VX_HX_WX, MemorySize.Packed128_Float32),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmsubadd213ps_VY_HY_WY, MemorySize.Packed256_Float32)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmsubadd213pd_VX_HX_WX, MemorySize.Packed128_Float64),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmsubadd213pd_VY_HY_WY, MemorySize.Packed256_Float64)
						)
					),
					invalid,
					invalid
				),

				// A8
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmadd213ps_VX_HX_WX, MemorySize.Packed128_Float32),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmadd213ps_VY_HY_WY, MemorySize.Packed256_Float32)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmadd213pd_VX_HX_WX, MemorySize.Packed128_Float64),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmadd213pd_VY_HY_WY, MemorySize.Packed256_Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmadd213ss_VX_HX_WX, MemorySize.Float32),
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmadd213sd_VX_HX_WX, MemorySize.Float64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmsub213ps_VX_HX_WX, MemorySize.Packed128_Float32),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmsub213ps_VY_HY_WY, MemorySize.Packed256_Float32)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmsub213pd_VX_HX_WX, MemorySize.Packed128_Float64),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmsub213pd_VY_HY_WY, MemorySize.Packed256_Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmsub213ss_VX_HX_WX, MemorySize.Float32),
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmsub213sd_VX_HX_WX, MemorySize.Float64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmadd213ps_VX_HX_WX, MemorySize.Packed128_Float32),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfnmadd213ps_VY_HY_WY, MemorySize.Packed256_Float32)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmadd213pd_VX_HX_WX, MemorySize.Packed128_Float64),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfnmadd213pd_VY_HY_WY, MemorySize.Packed256_Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmadd213ss_VX_HX_WX, MemorySize.Float32),
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmadd213sd_VX_HX_WX, MemorySize.Float64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmsub213ps_VX_HX_WX, MemorySize.Packed128_Float32),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfnmsub213ps_VY_HY_WY, MemorySize.Packed256_Float32)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmsub213pd_VX_HX_WX, MemorySize.Packed128_Float64),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfnmsub213pd_VY_HY_WY, MemorySize.Packed256_Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmsub213ss_VX_HX_WX, MemorySize.Float32),
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmsub213sd_VX_HX_WX, MemorySize.Float64)
					),
					invalid,
					invalid
				),

				// B0
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmaddsub231ps_VX_HX_WX, MemorySize.Packed128_Float32),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmaddsub231ps_VY_HY_WY, MemorySize.Packed256_Float32)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmaddsub231pd_VX_HX_WX, MemorySize.Packed128_Float64),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmaddsub231pd_VY_HY_WY, MemorySize.Packed256_Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmsubadd231ps_VX_HX_WX, MemorySize.Packed128_Float32),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmsubadd231ps_VY_HY_WY, MemorySize.Packed256_Float32)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmsubadd231pd_VX_HX_WX, MemorySize.Packed128_Float64),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmsubadd231pd_VY_HY_WY, MemorySize.Packed256_Float64)
						)
					),
					invalid,
					invalid
				),

				// B8
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmadd231ps_VX_HX_WX, MemorySize.Packed128_Float32),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmadd231ps_VY_HY_WY, MemorySize.Packed256_Float32)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmadd231pd_VX_HX_WX, MemorySize.Packed128_Float64),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmadd231pd_VY_HY_WY, MemorySize.Packed256_Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmadd231ss_VX_HX_WX, MemorySize.Float32),
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmadd231sd_VX_HX_WX, MemorySize.Float64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmsub231ps_VX_HX_WX, MemorySize.Packed128_Float32),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmsub231ps_VY_HY_WY, MemorySize.Packed256_Float32)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmsub231pd_VX_HX_WX, MemorySize.Packed128_Float64),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmsub231pd_VY_HY_WY, MemorySize.Packed256_Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmsub231ss_VX_HX_WX, MemorySize.Float32),
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmsub231sd_VX_HX_WX, MemorySize.Float64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmadd231ps_VX_HX_WX, MemorySize.Packed128_Float32),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfnmadd231ps_VY_HY_WY, MemorySize.Packed256_Float32)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmadd231pd_VX_HX_WX, MemorySize.Packed128_Float64),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfnmadd231pd_VY_HY_WY, MemorySize.Packed256_Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmadd231ss_VX_HX_WX, MemorySize.Float32),
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmadd231sd_VX_HX_WX, MemorySize.Float64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmsub231ps_VX_HX_WX, MemorySize.Packed128_Float32),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfnmsub231ps_VY_HY_WY, MemorySize.Packed256_Float32)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmsub231pd_VX_HX_WX, MemorySize.Packed128_Float64),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfnmsub231pd_VY_HY_WY, MemorySize.Packed256_Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmsub231ss_VX_HX_WX, MemorySize.Float32),
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmsub231sd_VX_HX_WX, MemorySize.Float64)
					),
					invalid,
					invalid
				),

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
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vaesimc_VX_WX, MemorySize.UInt128),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vaesenc_VX_HX_WX, MemorySize.UInt128),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vaesenclast_VX_HX_WX, MemorySize.UInt128),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vaesdec_VX_HX_WX, MemorySize.UInt128),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vaesdeclast_VX_HX_WX, MemorySize.UInt128),
						invalid
					),
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
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Gv_Gv_Ev(Code.VEX_Andn_Gd_Hd_Ed, Code.VEX_Andn_Gq_Hq_Eq),
						invalid
					),
					invalid,
					invalid,
					invalid
				),
				new OpCodeHandler_Group(handlers_Grp_0F38F3),
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Gv_Ev_Gv(Code.VEX_Bzhi_Gd_Ed_Hd, Code.VEX_Bzhi_Gq_Eq_Hq),
						invalid
					),
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Gv_Gv_Ev(Code.VEX_Pext_Gd_Hd_Ed, Code.VEX_Pext_Gq_Hq_Eq),
						invalid
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Gv_Gv_Ev(Code.VEX_Pdep_Gd_Hd_Ed, Code.VEX_Pdep_Gq_Hq_Eq),
						invalid
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					invalid,
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Gv_Gv_Ev(Code.VEX_Mulx_Gd_Hd_Ed, Code.VEX_Mulx_Gq_Hq_Eq),
						invalid
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Gv_Ev_Gv(Code.VEX_Bextr_Gd_Ed_Hd, Code.VEX_Bextr_Gq_Eq_Hq),
						invalid
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Gv_Ev_Gv(Code.VEX_Shlx_Gd_Ed_Hd, Code.VEX_Shlx_Gq_Eq_Hq),
						invalid
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Gv_Ev_Gv(Code.VEX_Sarx_Gd_Ed_Hd, Code.VEX_Sarx_Gq_Eq_Hq, MemorySize.Int32, MemorySize.Int64),
						invalid
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Gv_Ev_Gv(Code.VEX_Shrx_Gd_Ed_Hd, Code.VEX_Shrx_Gq_Eq_Hq),
						invalid
					)
				),

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

			ThreeByteHandlers_0F3AXX = new OpCodeHandler[0x100] {
				// 00
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_VEX(
							invalid,
							new OpCodeHandler_VEX_VWIb(Register.YMM0, Code.VEX_Vpermq_VY_WY_Ib, MemorySize.Packed256_Int64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_VEX(
							invalid,
							new OpCodeHandler_VEX_VWIb(Register.YMM0, Code.VEX_Vpermpd_VY_WY_Ib, MemorySize.Packed256_Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.VEX_Vpblendd_VX_HX_WX_Ib, MemorySize.Packed128_Int32),
							new OpCodeHandler_VEX_VHWIb(Register.YMM0, Code.VEX_Vpblendd_VY_HY_WY_Ib, MemorySize.Packed256_Int32)
						),
						invalid
					),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VWIb(Register.XMM0, Code.VEX_Vpermilps_VX_WX_Ib, MemorySize.Packed128_Float32),
							new OpCodeHandler_VEX_VWIb(Register.YMM0, Code.VEX_Vpermilps_VY_WY_Ib, MemorySize.Packed256_Float32)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VWIb(Register.XMM0, Code.VEX_Vpermilpd_VX_WX_Ib, MemorySize.Packed128_Float64),
							new OpCodeHandler_VEX_VWIb(Register.YMM0, Code.VEX_Vpermilpd_VY_WY_Ib, MemorySize.Packed256_Float64)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							invalid,
							new OpCodeHandler_VEX_VHWIb(Register.YMM0, Code.VEX_Vperm2f128_VY_HY_WY_Ib, MemorySize.Packed256_Float128)
						),
						invalid
					),
					invalid,
					invalid
				),
				invalid,

				// 08
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VWIb(Register.XMM0, Code.VEX_Vroundps_VX_WX_Ib, MemorySize.Packed128_Float32),
						new OpCodeHandler_VEX_VWIb(Register.YMM0, Code.VEX_Vroundps_VY_WY_Ib, MemorySize.Packed256_Float32)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VWIb(Register.XMM0, Code.VEX_Vroundpd_VX_WX_Ib, MemorySize.Packed128_Float64),
						new OpCodeHandler_VEX_VWIb(Register.YMM0, Code.VEX_Vroundpd_VY_WY_Ib, MemorySize.Packed256_Float64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.VEX_Vroundss_VX_HX_WX_Ib, MemorySize.Float32),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.VEX_Vroundsd_VX_HX_WX_Ib, MemorySize.Float64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.VEX_Vblendps_VX_HX_WX_Ib, MemorySize.Packed128_Float32),
						new OpCodeHandler_VEX_VHWIb(Register.YMM0, Code.VEX_Vblendps_VY_HY_WY_Ib, MemorySize.Packed256_Float32)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.VEX_Vblendpd_VX_HX_WX_Ib, MemorySize.Packed128_Float64),
						new OpCodeHandler_VEX_VHWIb(Register.YMM0, Code.VEX_Vblendpd_VY_HY_WY_Ib, MemorySize.Packed256_Float64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.VEX_Vpblendw_VX_HX_WX_Ib, MemorySize.Packed128_Int16),
						new OpCodeHandler_VEX_VHWIb(Register.YMM0, Code.VEX_Vpblendw_VY_HY_WY_Ib, MemorySize.Packed256_Int16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.VEX_Vpalignr_VX_HX_WX_Ib, MemorySize.Packed128_Int8),
						new OpCodeHandler_VEX_VHWIb(Register.YMM0, Code.VEX_Vpalignr_VY_HY_WY_Ib, MemorySize.Packed256_Int8)
					),
					invalid,
					invalid
				),

				// 10
				invalid,
				invalid,
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_GvM_VX_Ib(Register.XMM0, Code.VEX_Vpextrb_RdMb_VX_Ib, Code.VEX_Vpextrb_RqMb_VX_Ib, MemorySize.UInt8, MemorySize.UInt8),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_GvM_VX_Ib(Register.XMM0, Code.VEX_Vpextrw_RdMw_VX_Ib, Code.VEX_Vpextrw_RqMw_VX_Ib, MemorySize.UInt16, MemorySize.UInt16),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_GvM_VX_Ib(Register.XMM0, Code.VEX_Vpextrd_Ed_VX_Ib, Code.VEX_Vpextrq_Eq_VX_Ib, MemorySize.UInt32, MemorySize.UInt64),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Ed_V_Ib(Register.XMM0, Code.VEX_Vextractps_Ed_VX_Ib, Code.VEX_Vextractps_Eq_VX_Ib, MemorySize.Float32, MemorySize.Float32),
						invalid
					),
					invalid,
					invalid
				),

				// 18
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							invalid,
							new OpCodeHandler_VEX_VHWIb(Register.YMM0, Register.YMM0, Register.XMM0, Code.VEX_Vinsertf128_VY_HY_WX_Ib, MemorySize.Float128)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							invalid,
							new OpCodeHandler_VEX_WVIb(Register.XMM0, Register.YMM0, Code.VEX_Vextractf128_WX_VY_Ib, MemorySize.Float128)
						),
						invalid
					),
					invalid,
					invalid
				),
				invalid,
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_WVIb(Register.XMM0, Register.XMM0, Code.VEX_Vcvtps2ph_WX_VX_Ib, MemorySize.Packed64_Float16),
							new OpCodeHandler_VEX_WVIb(Register.XMM0, Register.YMM0, Code.VEX_Vcvtps2ph_WX_VY_Ib, MemorySize.Packed128_Float16)
						),
						invalid
					),
					invalid,
					invalid
				),
				invalid,
				invalid,

				// 20
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHEvIb(Register.XMM0, Code.VEX_Vpinsrb_VX_HX_RdMb_Ib, Code.VEX_Vpinsrb_VX_HX_RqMb_Ib, MemorySize.UInt8, MemorySize.UInt8),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.VEX_Vinsertps_VX_HX_WX_Ib, MemorySize.Float32),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHEvIb(Register.XMM0, Code.VEX_Vpinsrd_VX_HX_Ed_Ib, Code.VEX_Vpinsrq_VX_HX_Eq_Ib, MemorySize.UInt32, MemorySize.UInt64),
						invalid
					),
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
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_RK_Ib(Code.VEX_Kshiftrb_VK_RK_Ib),
							new OpCodeHandler_VEX_VK_RK_Ib(Code.VEX_Kshiftrw_VK_RK_Ib)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_RK_Ib(Code.VEX_Kshiftrd_VK_RK_Ib),
							new OpCodeHandler_VEX_VK_RK_Ib(Code.VEX_Kshiftrq_VK_RK_Ib)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_RK_Ib(Code.VEX_Kshiftlb_VK_RK_Ib),
							new OpCodeHandler_VEX_VK_RK_Ib(Code.VEX_Kshiftlw_VK_RK_Ib)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_RK_Ib(Code.VEX_Kshiftld_VK_RK_Ib),
							new OpCodeHandler_VEX_VK_RK_Ib(Code.VEX_Kshiftlq_VK_RK_Ib)
						),
						invalid
					),
					invalid,
					invalid
				),
				invalid,
				invalid,
				invalid,
				invalid,

				// 38
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							invalid,
							new OpCodeHandler_VEX_VHWIb(Register.YMM0, Register.YMM0, Register.XMM0, Code.VEX_Vinserti128_VY_HY_WX_Ib, MemorySize.Int128)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							invalid,
							new OpCodeHandler_VEX_WVIb(Register.XMM0, Register.YMM0, Code.VEX_Vextracti128_WX_VY_Ib, MemorySize.Int128)
						),
						invalid
					),
					invalid,
					invalid
				),
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// 40
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.VEX_Vdpps_VX_HX_WX_Ib, MemorySize.Packed128_Float32),
						new OpCodeHandler_VEX_VHWIb(Register.YMM0, Code.VEX_Vdpps_VY_HY_WY_Ib, MemorySize.Packed256_Float32)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.VEX_Vdppd_VX_HX_WX_Ib, MemorySize.Packed128_Float64),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.VEX_Vmpsadbw_VX_HX_WX_Ib, MemorySize.Packed128_UInt8),
						new OpCodeHandler_VEX_VHWIb(Register.YMM0, Code.VEX_Vmpsadbw_VY_HY_WY_Ib, MemorySize.Packed256_UInt8)
					),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.VEX_Vpclmulqdq_VX_HX_WX_Ib, MemorySize.Packed128_UInt64),
						invalid
					),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							invalid,
							new OpCodeHandler_VEX_VHWIb(Register.YMM0, Code.VEX_Vperm2i128_VY_HY_WY_Ib, MemorySize.Packed256_Int128)
						),
						invalid
					),
					invalid,
					invalid
				),
				invalid,

				// 48
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.VEX_Vblendvps_VX_HX_WX_Is4X, MemorySize.Packed128_Float32),
							new OpCodeHandler_VEX_VHWIs4(Register.YMM0, Code.VEX_Vblendvps_VY_HY_WY_Is4Y, MemorySize.Packed256_Float32)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.VEX_Vblendvpd_VX_HX_WX_Is4X, MemorySize.Packed128_Float64),
							new OpCodeHandler_VEX_VHWIs4(Register.YMM0, Code.VEX_Vblendvpd_VY_HY_WY_Is4Y, MemorySize.Packed256_Float64)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.VEX_Vpblendvb_VX_HX_WX_Is4X, MemorySize.Packed128_Int8),
							new OpCodeHandler_VEX_VHWIs4(Register.YMM0, Code.VEX_Vpblendvb_VY_HY_WY_Is4Y, MemorySize.Packed256_Int8)
						),
						invalid
					),
					invalid,
					invalid
				),
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
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VWIb(Register.XMM0, Code.VEX_Vpcmpestrm_VX_WX_Ib, MemorySize.Packed128_UInt8),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VWIb(Register.XMM0, Code.VEX_Vpcmpestri_VX_WX_Ib, MemorySize.Packed128_UInt8),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VWIb(Register.XMM0, Code.VEX_Vpcmpistrm_VX_WX_Ib, MemorySize.Packed128_UInt8),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VWIb(Register.XMM0, Code.VEX_Vpcmpistri_VX_WX_Ib, MemorySize.Packed128_UInt8),
						invalid
					),
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
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VWIb(Register.XMM0, Code.VEX_Vaeskeygenassist_VX_WX_Ib, MemorySize.UInt128),
						invalid
					),
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
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					invalid,
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Gv_Ev_Ib(Code.VEX_Rorx_Gd_Ed_Ib, Code.VEX_Rorx_Gq_Eq_Ib),
						invalid
					)
				),
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

			TwoByteHandlers_0FXX = new OpCodeHandler[0x100] {
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
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// 10
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vmovups_VX_WX, MemorySize.Packed128_Float32),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vmovups_VY_WY, MemorySize.Packed256_Float32)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vmovupd_VX_WX, MemorySize.Packed128_Float64),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vmovupd_VY_WY, MemorySize.Packed256_Float64)
					),
					new OpCodeHandler_RM(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vmovss_VX_HX_RX, MemorySize.Float32),
						new OpCodeHandler_VEX_VM(Register.XMM0, Code.VEX_Vmovss_VX_M, MemorySize.Float32)
					),
					new OpCodeHandler_RM(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vmovsd_VX_HX_RX, MemorySize.Float64),
						new OpCodeHandler_VEX_VM(Register.XMM0, Code.VEX_Vmovsd_VX_M, MemorySize.Float64)
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_WV(Register.XMM0, Code.VEX_Vmovups_WX_VX, MemorySize.Packed128_Float32),
						new OpCodeHandler_VEX_WV(Register.YMM0, Code.VEX_Vmovups_WY_VY, MemorySize.Packed256_Float32)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_WV(Register.XMM0, Code.VEX_Vmovupd_WX_VX, MemorySize.Packed128_Float64),
						new OpCodeHandler_VEX_WV(Register.YMM0, Code.VEX_Vmovupd_WY_VY, MemorySize.Packed256_Float64)
					),
					new OpCodeHandler_RM(
						new OpCodeHandler_VEX_WHV(Register.XMM0, Code.VEX_Vmovss_RX_HX_VX, MemorySize.Float32),
						new OpCodeHandler_VEX_MV(Register.XMM0, Code.VEX_Vmovss_M_VX, MemorySize.Float32)
					),
					new OpCodeHandler_RM(
						new OpCodeHandler_VEX_WHV(Register.XMM0, Code.VEX_Vmovsd_RX_HX_VX, MemorySize.Float64),
						new OpCodeHandler_VEX_MV(Register.XMM0, Code.VEX_Vmovsd_M_VX, MemorySize.Float64)
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vmovhlps_VX_HX_RX, Code.VEX_Vmovlps_VX_HX_M, MemorySize.Packed64_Float32),
						invalid
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHM(Register.XMM0, Code.VEX_Vmovlpd_VX_HX_M, MemorySize.Float64),
						invalid
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vmovsldup_VX_WX, MemorySize.Packed128_Float32),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vmovsldup_VY_WY, MemorySize.Packed256_Float32)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vmovddup_VX_WX, MemorySize.Float64),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vmovddup_VY_WY, MemorySize.Packed256_Float64)
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_MV(Register.XMM0, Code.VEX_Vmovlps_M_VX, MemorySize.Packed64_Float32),
						invalid
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_MV(Register.XMM0, Code.VEX_Vmovlpd_M_VX, MemorySize.Float64),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vunpcklps_VX_HX_WX, MemorySize.Packed128_Float32),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vunpcklps_VY_HY_WY, MemorySize.Packed256_Float32)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vunpcklpd_VX_HX_WX, MemorySize.Packed128_Float64),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vunpcklpd_VY_HY_WY, MemorySize.Packed256_Float64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vunpckhps_VX_HX_WX, MemorySize.Packed128_Float32),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vunpckhps_VY_HY_WY, MemorySize.Packed256_Float32)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vunpckhpd_VX_HX_WX, MemorySize.Packed128_Float64),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vunpckhpd_VY_HY_WY, MemorySize.Packed256_Float64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vmovlhps_VX_HX_RX, Code.VEX_Vmovhps_VX_HX_M, MemorySize.Packed64_Float32),
						invalid
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHM(Register.XMM0, Code.VEX_Vmovhpd_VX_HX_M, MemorySize.Float64),
						invalid
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vmovshdup_VX_WX, MemorySize.Packed128_Float32),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vmovshdup_VY_WY, MemorySize.Packed256_Float32)
					),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_MV(Register.XMM0, Code.VEX_Vmovhps_M_VX, MemorySize.Packed64_Float32),
						invalid
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_MV(Register.XMM0, Code.VEX_Vmovhpd_M_VX, MemorySize.Float64),
						invalid
					),
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
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

				// 28
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vmovaps_VX_WX, MemorySize.Packed128_Float32),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vmovaps_VY_WY, MemorySize.Packed256_Float32)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vmovapd_VX_WX, MemorySize.Packed128_Float64),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vmovapd_VY_WY, MemorySize.Packed256_Float64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_WV(Register.XMM0, Code.VEX_Vmovaps_WX_VX, MemorySize.Packed128_Float32),
						new OpCodeHandler_VEX_WV(Register.YMM0, Code.VEX_Vmovaps_WY_VY, MemorySize.Packed256_Float32)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_WV(Register.XMM0, Code.VEX_Vmovapd_WX_VX, MemorySize.Packed128_Float64),
						new OpCodeHandler_VEX_WV(Register.YMM0, Code.VEX_Vmovapd_WY_VY, MemorySize.Packed256_Float64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					invalid,
					new OpCodeHandler_VEX_VHEv(Register.XMM0, Code.VEX_Vcvtsi2ss_VX_HX_Ed, Code.VEX_Vcvtsi2ss_VX_HX_Eq),
					new OpCodeHandler_VEX_VHEv(Register.XMM0, Code.VEX_Vcvtsi2sd_VX_HX_Ed, Code.VEX_Vcvtsi2sd_VX_HX_Eq)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_MV(Register.XMM0, Code.VEX_Vmovntps_M_VX, MemorySize.Packed128_Float32),
						new OpCodeHandler_VEX_MV(Register.YMM0, Code.VEX_Vmovntps_M_VY, MemorySize.Packed256_Float32)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_MV(Register.XMM0, Code.VEX_Vmovntpd_M_VX, MemorySize.Packed128_Float64),
						new OpCodeHandler_VEX_MV(Register.YMM0, Code.VEX_Vmovntpd_M_VY, MemorySize.Packed256_Float64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					invalid,
					new OpCodeHandler_VEX_Gv_W(Register.XMM0, Code.VEX_Vcvttss2si_Gd_WX, Code.VEX_Vcvttss2si_Gq_WX, MemorySize.Float32),
					new OpCodeHandler_VEX_Gv_W(Register.XMM0, Code.VEX_Vcvttsd2si_Gd_WX, Code.VEX_Vcvttsd2si_Gq_WX, MemorySize.Float64)
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					invalid,
					new OpCodeHandler_VEX_Gv_W(Register.XMM0, Code.VEX_Vcvtss2si_Gd_WX, Code.VEX_Vcvtss2si_Gq_WX, MemorySize.Float32),
					new OpCodeHandler_VEX_Gv_W(Register.XMM0, Code.VEX_Vcvtsd2si_Gd_WX, Code.VEX_Vcvtsd2si_Gq_WX, MemorySize.Float64)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vucomiss_VX_WX, MemorySize.Float32),
					new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vucomisd_VX_WX, MemorySize.Float64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vcomiss_VX_WX, MemorySize.Float32),
					new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vcomisd_VX_WX, MemorySize.Float64),
					invalid,
					invalid
				),

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
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						invalid,
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kandw_VK_HK_RK),
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kandq_VK_HK_RK)
						)
					),
					new OpCodeHandler_VectorLength_VEX(
						invalid,
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kandb_VK_HK_RK),
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kandd_VK_HK_RK)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						invalid,
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kandnw_VK_HK_RK),
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kandnq_VK_HK_RK)
						)
					),
					new OpCodeHandler_VectorLength_VEX(
						invalid,
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kandnb_VK_HK_RK),
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kandnd_VK_HK_RK)
						)
					),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_RK(Code.VEX_Knotw_VK_RK),
							new OpCodeHandler_VEX_VK_RK(Code.VEX_Knotq_VK_RK)
						),
						invalid
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_RK(Code.VEX_Knotb_VK_RK),
							new OpCodeHandler_VEX_VK_RK(Code.VEX_Knotd_VK_RK)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						invalid,
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Korw_VK_HK_RK),
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Korq_VK_HK_RK)
						)
					),
					new OpCodeHandler_VectorLength_VEX(
						invalid,
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Korb_VK_HK_RK),
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kord_VK_HK_RK)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						invalid,
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kxnorw_VK_HK_RK),
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kxnorq_VK_HK_RK)
						)
					),
					new OpCodeHandler_VectorLength_VEX(
						invalid,
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kxnorb_VK_HK_RK),
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kxnord_VK_HK_RK)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						invalid,
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kxorw_VK_HK_RK),
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kxorq_VK_HK_RK)
						)
					),
					new OpCodeHandler_VectorLength_VEX(
						invalid,
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kxorb_VK_HK_RK),
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kxord_VK_HK_RK)
						)
					),
					invalid,
					invalid
				),

				// 48
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						invalid,
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kaddw_VK_HK_RK),
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kaddq_VK_HK_RK)
						)
					),
					new OpCodeHandler_VectorLength_VEX(
						invalid,
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kaddb_VK_HK_RK),
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kaddd_VK_HK_RK)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						invalid,
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kunpckwd_VK_HK_RK),
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kunpckdq_VK_HK_RK)
						)
					),
					new OpCodeHandler_VectorLength_VEX(
						invalid,
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kunpckbw_VK_HK_RK),
							invalid
						)
					),
					invalid,
					invalid
				),
				invalid,
				invalid,
				invalid,
				invalid,

				// 50
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Gv_RX(Register.XMM0, Code.VEX_Vmovmskps_Gd_RX, Code.VEX_Vmovmskps_Gq_RX),
						new OpCodeHandler_VEX_Gv_RX(Register.YMM0, Code.VEX_Vmovmskps_Gd_RY, Code.VEX_Vmovmskps_Gq_RY)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Gv_RX(Register.XMM0, Code.VEX_Vmovmskpd_Gd_RX, Code.VEX_Vmovmskpd_Gq_RX),
						new OpCodeHandler_VEX_Gv_RX(Register.YMM0, Code.VEX_Vmovmskpd_Gd_RY, Code.VEX_Vmovmskpd_Gq_RY)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vsqrtps_VX_WX, MemorySize.Packed128_Float32),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vsqrtps_VY_WY, MemorySize.Packed256_Float32)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vsqrtpd_VX_WX, MemorySize.Packed128_Float64),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vsqrtpd_VY_WY, MemorySize.Packed256_Float64)
					),
					new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vsqrtss_VX_HX_WX, MemorySize.Float32),
					new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vsqrtsd_VX_HX_WX, MemorySize.Float64)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vrsqrtps_VX_WX, MemorySize.Packed128_Float32),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vrsqrtps_VY_WY, MemorySize.Packed256_Float32)
					),
					invalid,
					new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vrsqrtss_VX_HX_WX, MemorySize.Float32),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vrcpps_VX_WX, MemorySize.Packed128_Float32),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vrcpps_VY_WY, MemorySize.Packed256_Float32)
					),
					invalid,
					new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vrcpss_VX_HX_WX, MemorySize.Float32),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vandps_VX_HX_WX, MemorySize.Packed128_Float32),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vandps_VY_HY_WY, MemorySize.Packed256_Float32)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vandpd_VX_HX_WX, MemorySize.Packed128_Float64),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vandpd_VY_HY_WY, MemorySize.Packed256_Float64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vandnps_VX_HX_WX, MemorySize.Packed128_Float32),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vandnps_VY_HY_WY, MemorySize.Packed256_Float32)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vandnpd_VX_HX_WX, MemorySize.Packed128_Float64),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vandnpd_VY_HY_WY, MemorySize.Packed256_Float64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vorps_VX_HX_WX, MemorySize.Packed128_Float32),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vorps_VY_HY_WY, MemorySize.Packed256_Float32)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vorpd_VX_HX_WX, MemorySize.Packed128_Float64),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vorpd_VY_HY_WY, MemorySize.Packed256_Float64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vxorps_VX_HX_WX, MemorySize.Packed128_Float32),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vxorps_VY_HY_WY, MemorySize.Packed256_Float32)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vxorpd_VX_HX_WX, MemorySize.Packed128_Float64),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vxorpd_VY_HY_WY, MemorySize.Packed256_Float64)
					),
					invalid,
					invalid
				),

				// 58
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vaddps_VX_HX_WX, MemorySize.Packed128_Float32),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vaddps_VY_HY_WY, MemorySize.Packed256_Float32)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vaddpd_VX_HX_WX, MemorySize.Packed128_Float64),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vaddpd_VY_HY_WY, MemorySize.Packed256_Float64)
					),
					new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vaddss_VX_HX_WX, MemorySize.Float32),
					new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vaddsd_VX_HX_WX, MemorySize.Float64)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vmulps_VX_HX_WX, MemorySize.Packed128_Float32),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vmulps_VY_HY_WY, MemorySize.Packed256_Float32)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vmulpd_VX_HX_WX, MemorySize.Packed128_Float64),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vmulpd_VY_HY_WY, MemorySize.Packed256_Float64)
					),
					new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vmulss_VX_HX_WX, MemorySize.Float32),
					new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vmulsd_VX_HX_WX, MemorySize.Float64)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vcvtps2pd_VX_WX, MemorySize.Packed64_Float32),
						new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vcvtps2pd_VY_WX, MemorySize.Packed128_Float32)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vcvtpd2ps_VX_WX, MemorySize.Packed128_Float64),
						new OpCodeHandler_VEX_VW(Register.XMM0, Register.YMM0, Code.VEX_Vcvtpd2ps_VX_WY, MemorySize.Packed256_Float64)
					),
					new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vcvtss2sd_VX_HX_WX, MemorySize.Float32),
					new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vcvtsd2ss_VX_HX_WX, MemorySize.Float64)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vcvtdq2ps_VX_WX, MemorySize.Packed128_Int32),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vcvtdq2ps_VY_WY, MemorySize.Packed256_Int32)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vcvtps2dq_VX_WX, MemorySize.Packed128_Float32),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vcvtps2dq_VY_WY, MemorySize.Packed256_Float32)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vcvttps2dq_VX_WX, MemorySize.Packed128_Float32),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vcvttps2dq_VY_WY, MemorySize.Packed256_Float32)
					),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vsubps_VX_HX_WX, MemorySize.Packed128_Float32),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vsubps_VY_HY_WY, MemorySize.Packed256_Float32)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vsubpd_VX_HX_WX, MemorySize.Packed128_Float64),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vsubpd_VY_HY_WY, MemorySize.Packed256_Float64)
					),
					new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vsubss_VX_HX_WX, MemorySize.Float32),
					new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vsubsd_VX_HX_WX, MemorySize.Float64)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vminps_VX_HX_WX, MemorySize.Packed128_Float32),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vminps_VY_HY_WY, MemorySize.Packed256_Float32)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vminpd_VX_HX_WX, MemorySize.Packed128_Float64),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vminpd_VY_HY_WY, MemorySize.Packed256_Float64)
					),
					new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vminss_VX_HX_WX, MemorySize.Float32),
					new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vminsd_VX_HX_WX, MemorySize.Float64)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vdivps_VX_HX_WX, MemorySize.Packed128_Float32),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vdivps_VY_HY_WY, MemorySize.Packed256_Float32)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vdivpd_VX_HX_WX, MemorySize.Packed128_Float64),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vdivpd_VY_HY_WY, MemorySize.Packed256_Float64)
					),
					new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vdivss_VX_HX_WX, MemorySize.Float32),
					new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vdivsd_VX_HX_WX, MemorySize.Float64)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vmaxps_VX_HX_WX, MemorySize.Packed128_Float32),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vmaxps_VY_HY_WY, MemorySize.Packed256_Float32)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vmaxpd_VX_HX_WX, MemorySize.Packed128_Float64),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vmaxpd_VY_HY_WY, MemorySize.Packed256_Float64)
					),
					new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vmaxss_VX_HX_WX, MemorySize.Float32),
					new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vmaxsd_VX_HX_WX, MemorySize.Float64)
				),

				// 60
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpunpcklbw_VX_HX_WX, MemorySize.Packed128_Int8),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpunpcklbw_VY_HY_WY, MemorySize.Packed256_Int8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpunpcklwd_VX_HX_WX, MemorySize.Packed128_Int16),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpunpcklwd_VY_HY_WY, MemorySize.Packed256_Int16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpunpckldq_VX_HX_WX, MemorySize.Packed128_Int32),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpunpckldq_VY_HY_WY, MemorySize.Packed256_Int32)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpacksswb_VX_HX_WX, MemorySize.Packed128_Int16),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpacksswb_VY_HY_WY, MemorySize.Packed256_Int16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpcmpgtb_VX_HX_WX, MemorySize.Packed128_Int8),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpcmpgtb_VY_HY_WY, MemorySize.Packed256_Int8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpcmpgtw_VX_HX_WX, MemorySize.Packed128_Int16),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpcmpgtw_VY_HY_WY, MemorySize.Packed256_Int16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpcmpgtd_VX_HX_WX, MemorySize.Packed128_Int32),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpcmpgtd_VY_HY_WY, MemorySize.Packed256_Int32)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpackuswb_VX_HX_WX, MemorySize.Packed128_Int16),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpackuswb_VY_HY_WY, MemorySize.Packed256_Int16)
					),
					invalid,
					invalid
				),

				// 68
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpunpckhbw_VX_HX_WX, MemorySize.Packed128_Int8),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpunpckhbw_VY_HY_WY, MemorySize.Packed256_Int8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpunpckhwd_VX_HX_WX, MemorySize.Packed128_Int16),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpunpckhwd_VY_HY_WY, MemorySize.Packed256_Int16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpunpckhdq_VX_HX_WX, MemorySize.Packed128_Int32),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpunpckhdq_VY_HY_WY, MemorySize.Packed256_Int32)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpackssdw_VX_HX_WX, MemorySize.Packed128_Int32),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpackssdw_VY_HY_WY, MemorySize.Packed256_Int32)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpunpcklqdq_VX_HX_WX, MemorySize.Packed128_Int64),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpunpcklqdq_VY_HY_WY, MemorySize.Packed256_Int64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpunpckhqdq_VX_HX_WX, MemorySize.Packed128_Int64),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpunpckhqdq_VY_HY_WY, MemorySize.Packed256_Int64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VX_Ev(Code.VEX_Vmovd_VX_Ed, Code.VEX_Vmovq_VX_Eq),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vmovdqa_VX_WX, MemorySize.Packed128_Int32),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vmovdqa_VY_WY, MemorySize.Packed256_Int32)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vmovdqu_VX_WX, MemorySize.Packed128_Int32),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vmovdqu_VY_WY, MemorySize.Packed256_Int32)
					),
					invalid
				),

				// 70
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VWIb(Register.XMM0, Code.VEX_Vpshufd_VX_WX_Ib, MemorySize.Packed128_Int32),
						new OpCodeHandler_VEX_VWIb(Register.YMM0, Code.VEX_Vpshufd_VY_WY_Ib, MemorySize.Packed256_Int32)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VWIb(Register.XMM0, Code.VEX_Vpshufhw_VX_WX_Ib, MemorySize.Packed128_Int16),
						new OpCodeHandler_VEX_VWIb(Register.YMM0, Code.VEX_Vpshufhw_VY_WY_Ib, MemorySize.Packed256_Int16)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VWIb(Register.XMM0, Code.VEX_Vpshuflw_VX_WX_Ib, MemorySize.Packed128_Int16),
						new OpCodeHandler_VEX_VWIb(Register.YMM0, Code.VEX_Vpshuflw_VY_WY_Ib, MemorySize.Packed256_Int16)
					)
				),
				new OpCodeHandler_Group(handlers_Grp_0F71),
				new OpCodeHandler_Group(handlers_Grp_0F72),
				new OpCodeHandler_Group(handlers_Grp_0F73),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpcmpeqb_VX_HX_WX, MemorySize.Packed128_Int8),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpcmpeqb_VY_HY_WY, MemorySize.Packed256_Int8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpcmpeqw_VX_HX_WX, MemorySize.Packed128_Int16),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpcmpeqw_VY_HY_WY, MemorySize.Packed256_Int16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpcmpeqd_VX_HX_WX, MemorySize.Packed128_Int32),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpcmpeqd_VY_HY_WY, MemorySize.Packed256_Int32)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2_NoModRM(
					new OpCodeHandler_VectorLength_NoModRM_VEX(
						new OpCodeHandler_Simple(Code.VEX_Vzeroupper),
						new OpCodeHandler_Simple(Code.VEX_Vzeroall)
					),
					OpCodeHandler_Invalid_NoModRM.Instance,
					OpCodeHandler_Invalid_NoModRM.Instance,
					OpCodeHandler_Invalid_NoModRM.Instance
				),

				// 78
				invalid,
				invalid,
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vhaddpd_VX_HX_WX, MemorySize.Packed128_Float64),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vhaddpd_VY_HY_WY, MemorySize.Packed256_Float64)
					),
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vhaddps_VX_HX_WX, MemorySize.Packed128_Float32),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vhaddps_VY_HY_WY, MemorySize.Packed256_Float32)
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vhsubpd_VX_HX_WX, MemorySize.Packed128_Float64),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vhsubpd_VY_HY_WY, MemorySize.Packed256_Float64)
					),
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vhsubps_VX_HX_WX, MemorySize.Packed128_Float32),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vhsubps_VY_HY_WY, MemorySize.Packed256_Float32)
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Ev_VX(Code.VEX_Vmovd_Ed_VX, Code.VEX_Vmovq_Eq_VX),
						invalid
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vmovq_VX_WX, MemorySize.UInt64),
						invalid
					),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_WV(Register.XMM0, Code.VEX_Vmovdqa_WX_VX, MemorySize.Packed128_Int32),
						new OpCodeHandler_VEX_WV(Register.YMM0, Code.VEX_Vmovdqa_WY_VY, MemorySize.Packed256_Int32)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_WV(Register.XMM0, Code.VEX_Vmovdqu_WX_VX, MemorySize.Packed128_Int32),
						new OpCodeHandler_VEX_WV(Register.YMM0, Code.VEX_Vmovdqu_WY_VY, MemorySize.Packed256_Int32)
					),
					invalid
				),

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
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_WK(Code.VEX_Kmovw_VK_WK, MemorySize.UInt16),
							new OpCodeHandler_VEX_VK_WK(Code.VEX_Kmovq_VK_WK, MemorySize.UInt64)
						),
						invalid
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_WK(Code.VEX_Kmovb_VK_WK, MemorySize.UInt8),
							new OpCodeHandler_VEX_VK_WK(Code.VEX_Kmovd_VK_WK, MemorySize.UInt32)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_MK_VK(Code.VEX_Kmovw_MK_VK, MemorySize.UInt16),
							new OpCodeHandler_VEX_MK_VK(Code.VEX_Kmovq_MK_VK, MemorySize.UInt64)
						),
						invalid
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_MK_VK(Code.VEX_Kmovb_MK_VK, MemorySize.UInt8),
							new OpCodeHandler_VEX_MK_VK(Code.VEX_Kmovd_MK_VK, MemorySize.UInt32)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_R(Code.VEX_Kmovw_VK_Rd, Register.EAX),
							invalid
						),
						invalid
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_R(Code.VEX_Kmovb_VK_Rd, Register.EAX),
							invalid
						),
						invalid
					),
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_R(Code.VEX_Kmovd_VK_Rd, Register.EAX),
							invalid
						),
						invalid
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_G_VK(Code.VEX_Kmovw_Gd_RK, Register.EAX),
							invalid
						),
						invalid
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_G_VK(Code.VEX_Kmovb_Gd_RK, Register.EAX),
							invalid
						),
						invalid
					),
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_G_VK(Code.VEX_Kmovd_Gd_RK, Register.EAX),
							invalid
						),
						invalid
					)
				),
				invalid,
				invalid,
				invalid,
				invalid,

				// 98
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_RK(Code.VEX_Kortestw_VK_RK),
							new OpCodeHandler_VEX_VK_RK(Code.VEX_Kortestq_VK_RK)
						),
						invalid
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_RK(Code.VEX_Kortestb_VK_RK),
							new OpCodeHandler_VEX_VK_RK(Code.VEX_Kortestd_VK_RK)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_RK(Code.VEX_Ktestw_VK_RK),
							new OpCodeHandler_VEX_VK_RK(Code.VEX_Ktestq_VK_RK)
						),
						invalid
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_RK(Code.VEX_Ktestb_VK_RK),
							new OpCodeHandler_VEX_VK_RK(Code.VEX_Ktestd_VK_RK)
						),
						invalid
					),
					invalid,
					invalid
				),
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
				new OpCodeHandler_Group(handlers_Grp_0FAE),
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
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.VEX_Vcmpps_VX_HX_WX_Ib, MemorySize.Packed128_Float32),
						new OpCodeHandler_VEX_VHWIb(Register.YMM0, Code.VEX_Vcmpps_VY_HY_WY_Ib, MemorySize.Packed256_Float32)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.VEX_Vcmppd_VX_HX_WX_Ib, MemorySize.Packed128_Float64),
						new OpCodeHandler_VEX_VHWIb(Register.YMM0, Code.VEX_Vcmppd_VY_HY_WY_Ib, MemorySize.Packed256_Float64)
					),
					new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.VEX_Vcmpss_VX_HX_WX_Ib, MemorySize.Float32),
					new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.VEX_Vcmpsd_VX_HX_WX_Ib, MemorySize.Float64)
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VEX_VHEvIb(Register.XMM0, Code.VEX_Vpinsrw_VX_HX_RdMw_Ib, Code.VEX_Vpinsrw_VX_HX_RqMw_Ib, MemorySize.UInt16, MemorySize.UInt16),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VEX_Gv_GPR_Ib(Register.XMM0, Code.VEX_Vpextrw_Gd_RX_Ib, Code.VEX_Vpextrw_Gq_RX_Ib),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.VEX_Vshufps_VX_HX_WX_Ib, MemorySize.Packed128_Float32),
						new OpCodeHandler_VEX_VHWIb(Register.YMM0, Code.VEX_Vshufps_VY_HY_WY_Ib, MemorySize.Packed256_Float32)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.VEX_Vshufpd_VX_HX_WX_Ib, MemorySize.Packed128_Float64),
						new OpCodeHandler_VEX_VHWIb(Register.YMM0, Code.VEX_Vshufpd_VY_HY_WY_Ib, MemorySize.Packed256_Float64)
					),
					invalid,
					invalid
				),
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
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vaddsubpd_VX_HX_WX, MemorySize.Packed128_Float64),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vaddsubpd_VY_HY_WY, MemorySize.Packed256_Float64)
					),
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vaddsubps_VX_HX_WX, MemorySize.Packed128_Float32),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vaddsubps_VY_HY_WY, MemorySize.Packed256_Float32)
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Register.XMM0, Register.XMM0, Code.VEX_Vpsrlw_VX_HX_WX, MemorySize.Packed128_UInt64),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Register.YMM0, Register.XMM0, Code.VEX_Vpsrlw_VY_HY_WX, MemorySize.Packed128_UInt64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Register.XMM0, Register.XMM0, Code.VEX_Vpsrld_VX_HX_WX, MemorySize.Packed128_UInt64),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Register.YMM0, Register.XMM0, Code.VEX_Vpsrld_VY_HY_WX, MemorySize.Packed128_UInt64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Register.XMM0, Register.XMM0, Code.VEX_Vpsrlq_VX_HX_WX, MemorySize.Packed128_UInt64),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Register.YMM0, Register.XMM0, Code.VEX_Vpsrlq_VY_HY_WX, MemorySize.Packed128_UInt64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpaddq_VX_HX_WX, MemorySize.Packed128_UInt64),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpaddq_VY_HY_WY, MemorySize.Packed256_UInt64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpmullw_VX_HX_WX, MemorySize.Packed128_Int16),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpmullw_VY_HY_WY, MemorySize.Packed256_Int16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VEX_WV(Register.XMM0, Code.VEX_Vmovq_WX_VX, MemorySize.UInt64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Gv_RX(Register.XMM0, Code.VEX_Vpmovmskb_Gd_RX, Code.VEX_Vpmovmskb_Gq_RX),
						new OpCodeHandler_VEX_Gv_RX(Register.YMM0, Code.VEX_Vpmovmskb_Gd_RY, Code.VEX_Vpmovmskb_Gq_RY)
					),
					invalid,
					invalid
				),

				// D8
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpsubusb_VX_HX_WX, MemorySize.Packed128_UInt8),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpsubusb_VY_HY_WY, MemorySize.Packed256_UInt8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpsubusw_VX_HX_WX, MemorySize.Packed128_UInt16),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpsubusw_VY_HY_WY, MemorySize.Packed256_UInt16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpminub_VX_HX_WX, MemorySize.Packed128_UInt8),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpminub_VY_HY_WY, MemorySize.Packed256_UInt8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpand_VX_HX_WX, MemorySize.Packed128_UInt64),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpand_VY_HY_WY, MemorySize.Packed256_UInt64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpaddusb_VX_HX_WX, MemorySize.Packed128_UInt8),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpaddusb_VY_HY_WY, MemorySize.Packed256_UInt8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpaddusw_VX_HX_WX, MemorySize.Packed128_UInt16),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpaddusw_VY_HY_WY, MemorySize.Packed256_UInt16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpmaxub_VX_HX_WX, MemorySize.Packed128_UInt8),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpmaxub_VY_HY_WY, MemorySize.Packed256_UInt8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpandn_VX_HX_WX, MemorySize.Packed128_UInt64),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpandn_VY_HY_WY, MemorySize.Packed256_UInt64)
					),
					invalid,
					invalid
				),

				// E0
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpavgb_VX_HX_WX, MemorySize.Packed128_UInt8),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpavgb_VY_HY_WY, MemorySize.Packed256_UInt8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Register.XMM0, Register.XMM0, Code.VEX_Vpsraw_VX_HX_WX, MemorySize.Packed128_UInt64),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Register.YMM0, Register.XMM0, Code.VEX_Vpsraw_VY_HY_WX, MemorySize.Packed128_UInt64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Register.XMM0, Register.XMM0, Code.VEX_Vpsrad_VX_HX_WX, MemorySize.Packed128_UInt64),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Register.YMM0, Register.XMM0, Code.VEX_Vpsrad_VY_HY_WX, MemorySize.Packed128_UInt64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpavgw_VX_HX_WX, MemorySize.Packed128_UInt16),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpavgw_VY_HY_WY, MemorySize.Packed256_UInt16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpmulhuw_VX_HX_WX, MemorySize.Packed128_UInt16),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpmulhuw_VY_HY_WY, MemorySize.Packed256_UInt16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpmulhw_VX_HX_WX, MemorySize.Packed128_Int16),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpmulhw_VY_HY_WY, MemorySize.Packed256_Int16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vcvttpd2dq_VX_WX, MemorySize.Packed128_Float64),
						new OpCodeHandler_VEX_VW(Register.XMM0, Register.YMM0, Code.VEX_Vcvttpd2dq_VX_WY, MemorySize.Packed256_Float64)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vcvtdq2pd_VX_WX, MemorySize.Packed64_Int32),
						new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vcvtdq2pd_VY_WX, MemorySize.Packed128_Int32)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vcvtpd2dq_VX_WX, MemorySize.Packed128_Float64),
						new OpCodeHandler_VEX_VW(Register.XMM0, Register.YMM0, Code.VEX_Vcvtpd2dq_VX_WY, MemorySize.Packed256_Float64)
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_MV(Register.XMM0, Code.VEX_Vmovntdq_M_VX, MemorySize.Packed128_Int32),
						new OpCodeHandler_VEX_MV(Register.YMM0, Code.VEX_Vmovntdq_M_VY, MemorySize.Packed256_Int32)
					),
					invalid,
					invalid
				),

				// E8
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpsubsb_VX_HX_WX, MemorySize.Packed128_Int8),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpsubsb_VY_HY_WY, MemorySize.Packed256_Int8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpsubsw_VX_HX_WX, MemorySize.Packed128_Int16),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpsubsw_VY_HY_WY, MemorySize.Packed256_Int16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpminsw_VX_HX_WX, MemorySize.Packed128_Int16),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpminsw_VY_HY_WY, MemorySize.Packed256_Int16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpor_VX_HX_WX, MemorySize.Packed128_UInt64),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpor_VY_HY_WY, MemorySize.Packed256_UInt64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpaddsb_VX_HX_WX, MemorySize.Packed128_Int8),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpaddsb_VY_HY_WY, MemorySize.Packed256_Int8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpaddsw_VX_HX_WX, MemorySize.Packed128_Int16),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpaddsw_VY_HY_WY, MemorySize.Packed256_Int16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpmaxsw_VX_HX_WX, MemorySize.Packed128_Int16),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpmaxsw_VY_HY_WY, MemorySize.Packed256_Int16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpxor_VX_HX_WX, MemorySize.Packed128_UInt64),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpxor_VY_HY_WY, MemorySize.Packed256_UInt64)
					),
					invalid,
					invalid
				),

				// F0
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					invalid,
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VM(Register.XMM0, Code.VEX_Vlddqu_VX_M, MemorySize.UInt128),
						new OpCodeHandler_VEX_VM(Register.YMM0, Code.VEX_Vlddqu_VY_M, MemorySize.UInt256)
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Register.XMM0, Register.XMM0, Code.VEX_Vpsllw_VX_HX_WX, MemorySize.Packed128_UInt64),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Register.YMM0, Register.XMM0, Code.VEX_Vpsllw_VY_HY_WX, MemorySize.Packed128_UInt64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Register.XMM0, Register.XMM0, Code.VEX_Vpslld_VX_HX_WX, MemorySize.Packed128_UInt64),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Register.YMM0, Register.XMM0, Code.VEX_Vpslld_VY_HY_WX, MemorySize.Packed128_UInt64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Register.XMM0, Register.XMM0, Code.VEX_Vpsllq_VX_HX_WX, MemorySize.Packed128_UInt64),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Register.YMM0, Register.XMM0, Code.VEX_Vpsllq_VY_HY_WX, MemorySize.Packed128_UInt64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpmuludq_VX_HX_WX, MemorySize.Packed128_UInt64),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpmuludq_VY_HY_WY, MemorySize.Packed256_UInt64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpmaddwd_VX_HX_WX, MemorySize.Packed128_Int16),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpmaddwd_VY_HY_WY, MemorySize.Packed256_Int16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpsadbw_VX_HX_WX, MemorySize.Packed128_UInt8),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpsadbw_VY_HY_WY, MemorySize.Packed256_UInt8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_rDI_VX_RX(Register.XMM0, Code.VEX_Vmaskmovdqu_rDI_VX_RX, MemorySize.UInt128),
						invalid
					),
					invalid,
					invalid
				),

				// F8
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpsubb_VX_HX_WX, MemorySize.Packed128_Int8),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpsubb_VY_HY_WY, MemorySize.Packed256_Int8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpsubw_VX_HX_WX, MemorySize.Packed128_Int16),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpsubw_VY_HY_WY, MemorySize.Packed256_Int16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpsubd_VX_HX_WX, MemorySize.Packed128_Int32),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpsubd_VY_HY_WY, MemorySize.Packed256_Int32)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpsubq_VX_HX_WX, MemorySize.Packed128_Int64),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpsubq_VY_HY_WY, MemorySize.Packed256_Int64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpaddb_VX_HX_WX, MemorySize.Packed128_Int8),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpaddb_VY_HY_WY, MemorySize.Packed256_Int8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpaddw_VX_HX_WX, MemorySize.Packed128_Int16),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpaddw_VY_HY_WY, MemorySize.Packed256_Int16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpaddd_VX_HX_WX, MemorySize.Packed128_Int32),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpaddd_VY_HY_WY, MemorySize.Packed256_Int32)
					),
					invalid,
					invalid
				),
				invalid,
			};
		}
	}
}
#endif
