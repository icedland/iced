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
	/// Handlers for 16/32-bit mode (EVEX)
	/// </summary>
	static class OpCodeHandlers32Tables_EVEX {
		internal static readonly OpCodeHandler[] ThreeByteHandlers_0F38XX;
		internal static readonly OpCodeHandler[] ThreeByteHandlers_0F3AXX;
		internal static readonly OpCodeHandler[] TwoByteHandlers_0FXX;

		static OpCodeHandlers32Tables_EVEX() {
			// Store it in a local. Instead of 1500+ ldfld, we'll have 1500+ ldloc.0 (save 4 bytes per load)
			var invalid = OpCodeHandler_Invalid.Instance;

			var handlers_Grp_0F71 = new OpCodeHandler[8] {
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_HkWIb(Register.XMM0, Code.EVEX_Vpsrlw_HX_k1z_WX_Ib, TupleType.Full_Mem_128, MemorySize.Packed128_UInt16),
						new OpCodeHandler_EVEX_HkWIb(Register.YMM0, Code.EVEX_Vpsrlw_HY_k1z_WY_Ib, TupleType.Full_Mem_256, MemorySize.Packed256_UInt16),
						new OpCodeHandler_EVEX_HkWIb(Register.ZMM0, Code.EVEX_Vpsrlw_HZ_k1z_WZ_Ib, TupleType.Full_Mem_512, MemorySize.Packed512_UInt16)
					),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_HkWIb(Register.XMM0, Code.EVEX_Vpsraw_HX_k1z_WX_Ib, TupleType.Full_Mem_128, MemorySize.Packed128_Int16),
						new OpCodeHandler_EVEX_HkWIb(Register.YMM0, Code.EVEX_Vpsraw_HY_k1z_WY_Ib, TupleType.Full_Mem_256, MemorySize.Packed256_Int16),
						new OpCodeHandler_EVEX_HkWIb(Register.ZMM0, Code.EVEX_Vpsraw_HZ_k1z_WZ_Ib, TupleType.Full_Mem_512, MemorySize.Packed512_Int16)
					),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_HkWIb(Register.XMM0, Code.EVEX_Vpsllw_HX_k1z_WX_Ib, TupleType.Full_Mem_128, MemorySize.Packed128_UInt16),
						new OpCodeHandler_EVEX_HkWIb(Register.YMM0, Code.EVEX_Vpsllw_HY_k1z_WY_Ib, TupleType.Full_Mem_256, MemorySize.Packed256_UInt16),
						new OpCodeHandler_EVEX_HkWIb(Register.ZMM0, Code.EVEX_Vpsllw_HZ_k1z_WZ_Ib, TupleType.Full_Mem_512, MemorySize.Packed512_UInt16)
					),
					invalid,
					invalid
				),
				invalid,
			};

			var handlers_Grp_0F72 = new OpCodeHandler[8] {
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_HkWIb(Register.XMM0, Code.EVEX_Vprord_HX_k1z_WX_Ib_b, TupleType.Full_128, MemorySize.Packed128_UInt32, MemorySize.Broadcast128_UInt32),
							new OpCodeHandler_EVEX_HkWIb(Register.YMM0, Code.EVEX_Vprord_HY_k1z_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_UInt32, MemorySize.Broadcast256_UInt32),
							new OpCodeHandler_EVEX_HkWIb(Register.ZMM0, Code.EVEX_Vprord_HZ_k1z_WZ_Ib_b, TupleType.Full_512, MemorySize.Packed512_UInt32, MemorySize.Broadcast512_UInt32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_HkWIb(Register.XMM0, Code.EVEX_Vprorq_HX_k1z_WX_Ib_b, TupleType.Full_128, MemorySize.Packed128_UInt64, MemorySize.Broadcast128_UInt64),
							new OpCodeHandler_EVEX_HkWIb(Register.YMM0, Code.EVEX_Vprorq_HY_k1z_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_UInt64, MemorySize.Broadcast256_UInt64),
							new OpCodeHandler_EVEX_HkWIb(Register.ZMM0, Code.EVEX_Vprorq_HZ_k1z_WZ_Ib_b, TupleType.Full_512, MemorySize.Packed512_UInt64, MemorySize.Broadcast512_UInt64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_HkWIb(Register.XMM0, Code.EVEX_Vprold_HX_k1z_WX_Ib_b, TupleType.Full_128, MemorySize.Packed128_UInt32, MemorySize.Broadcast128_UInt32),
							new OpCodeHandler_EVEX_HkWIb(Register.YMM0, Code.EVEX_Vprold_HY_k1z_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_UInt32, MemorySize.Broadcast256_UInt32),
							new OpCodeHandler_EVEX_HkWIb(Register.ZMM0, Code.EVEX_Vprold_HZ_k1z_WZ_Ib_b, TupleType.Full_512, MemorySize.Packed512_UInt32, MemorySize.Broadcast512_UInt32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_HkWIb(Register.XMM0, Code.EVEX_Vprolq_HX_k1z_WX_Ib_b, TupleType.Full_128, MemorySize.Packed128_UInt64, MemorySize.Broadcast128_UInt64),
							new OpCodeHandler_EVEX_HkWIb(Register.YMM0, Code.EVEX_Vprolq_HY_k1z_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_UInt64, MemorySize.Broadcast256_UInt64),
							new OpCodeHandler_EVEX_HkWIb(Register.ZMM0, Code.EVEX_Vprolq_HZ_k1z_WZ_Ib_b, TupleType.Full_512, MemorySize.Packed512_UInt64, MemorySize.Broadcast512_UInt64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_HkWIb(Register.XMM0, Code.EVEX_Vpsrld_HX_k1z_WX_Ib_b, TupleType.Full_128, MemorySize.Packed128_UInt32, MemorySize.Broadcast128_UInt32),
							new OpCodeHandler_EVEX_HkWIb(Register.YMM0, Code.EVEX_Vpsrld_HY_k1z_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_UInt32, MemorySize.Broadcast256_UInt32),
							new OpCodeHandler_EVEX_HkWIb(Register.ZMM0, Code.EVEX_Vpsrld_HZ_k1z_WZ_Ib_b, TupleType.Full_512, MemorySize.Packed512_UInt32, MemorySize.Broadcast512_UInt32)
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
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_HkWIb(Register.XMM0, Code.EVEX_Vpsrad_HX_k1z_WX_Ib_b, TupleType.Full_128, MemorySize.Packed128_Int32, MemorySize.Broadcast128_Int32),
							new OpCodeHandler_EVEX_HkWIb(Register.YMM0, Code.EVEX_Vpsrad_HY_k1z_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_Int32, MemorySize.Broadcast256_Int32),
							new OpCodeHandler_EVEX_HkWIb(Register.ZMM0, Code.EVEX_Vpsrad_HZ_k1z_WZ_Ib_b, TupleType.Full_512, MemorySize.Packed512_Int32, MemorySize.Broadcast512_Int32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_HkWIb(Register.XMM0, Code.EVEX_Vpsraq_HX_k1z_WX_Ib_b, TupleType.Full_128, MemorySize.Packed128_Int64, MemorySize.Broadcast128_Int64),
							new OpCodeHandler_EVEX_HkWIb(Register.YMM0, Code.EVEX_Vpsraq_HY_k1z_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_Int64, MemorySize.Broadcast256_Int64),
							new OpCodeHandler_EVEX_HkWIb(Register.ZMM0, Code.EVEX_Vpsraq_HZ_k1z_WZ_Ib_b, TupleType.Full_512, MemorySize.Packed512_Int64, MemorySize.Broadcast512_Int64)
						)
					),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_HkWIb(Register.XMM0, Code.EVEX_Vpslld_HX_k1z_WX_Ib_b, TupleType.Full_128, MemorySize.Packed128_UInt32, MemorySize.Broadcast128_UInt32),
							new OpCodeHandler_EVEX_HkWIb(Register.YMM0, Code.EVEX_Vpslld_HY_k1z_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_UInt32, MemorySize.Broadcast256_UInt32),
							new OpCodeHandler_EVEX_HkWIb(Register.ZMM0, Code.EVEX_Vpslld_HZ_k1z_WZ_Ib_b, TupleType.Full_512, MemorySize.Packed512_UInt32, MemorySize.Broadcast512_UInt32)
						),
						invalid
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
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_HkWIb(Register.XMM0, Code.EVEX_Vpsrlq_HX_k1z_WX_Ib_b, TupleType.Full_128, MemorySize.Packed128_UInt64, MemorySize.Broadcast128_UInt64),
							new OpCodeHandler_EVEX_HkWIb(Register.YMM0, Code.EVEX_Vpsrlq_HY_k1z_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_UInt64, MemorySize.Broadcast256_UInt64),
							new OpCodeHandler_EVEX_HkWIb(Register.ZMM0, Code.EVEX_Vpsrlq_HZ_k1z_WZ_Ib_b, TupleType.Full_512, MemorySize.Packed512_UInt64, MemorySize.Broadcast512_UInt64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_HkWIb(Register.XMM0, Code.EVEX_Vpsrldq_HX_WX_Ib, TupleType.Full_Mem_128, MemorySize.UInt128),
						new OpCodeHandler_EVEX_HkWIb(Register.YMM0, Code.EVEX_Vpsrldq_HY_WY_Ib, TupleType.Full_Mem_256, MemorySize.Packed256_UInt128),
						new OpCodeHandler_EVEX_HkWIb(Register.ZMM0, Code.EVEX_Vpsrldq_HZ_WZ_Ib, TupleType.Full_Mem_512, MemorySize.Packed512_UInt128)
					),
					invalid,
					invalid
				),
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_HkWIb(Register.XMM0, Code.EVEX_Vpsllq_HX_k1z_WX_Ib_b, TupleType.Full_128, MemorySize.Packed128_UInt64, MemorySize.Broadcast128_UInt64),
							new OpCodeHandler_EVEX_HkWIb(Register.YMM0, Code.EVEX_Vpsllq_HY_k1z_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_UInt64, MemorySize.Broadcast256_UInt64),
							new OpCodeHandler_EVEX_HkWIb(Register.ZMM0, Code.EVEX_Vpsllq_HZ_k1z_WZ_Ib_b, TupleType.Full_512, MemorySize.Packed512_UInt64, MemorySize.Broadcast512_UInt64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_HkWIb(Register.XMM0, Code.EVEX_Vpslldq_HX_WX_Ib, TupleType.Full_Mem_128, MemorySize.UInt128),
						new OpCodeHandler_EVEX_HkWIb(Register.YMM0, Code.EVEX_Vpslldq_HY_WY_Ib, TupleType.Full_Mem_256, MemorySize.Packed256_UInt128),
						new OpCodeHandler_EVEX_HkWIb(Register.ZMM0, Code.EVEX_Vpslldq_HZ_WZ_Ib, TupleType.Full_Mem_512, MemorySize.Packed512_UInt128)
					),
					invalid,
					invalid
				),
			};

			var handlers_Grp_0F38C6 = new OpCodeHandler[8] {
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							invalid,
							new OpCodeHandler_EVEX_VSIB_k1(Register.ZMM0, Code.EVEX_Vgatherpf0dps_VM32Z_k1, TupleType.Tuple1_Scalar, MemorySize.Float32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							invalid,
							new OpCodeHandler_EVEX_VSIB_k1(Register.YMM0, Code.EVEX_Vgatherpf0dpd_VM32Y_k1, TupleType.Tuple1_Scalar, MemorySize.Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							invalid,
							new OpCodeHandler_EVEX_VSIB_k1(Register.ZMM0, Code.EVEX_Vgatherpf1dps_VM32Z_k1, TupleType.Tuple1_Scalar, MemorySize.Float32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							invalid,
							new OpCodeHandler_EVEX_VSIB_k1(Register.YMM0, Code.EVEX_Vgatherpf1dpd_VM32Y_k1, TupleType.Tuple1_Scalar, MemorySize.Float64)
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
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							invalid,
							new OpCodeHandler_EVEX_VSIB_k1(Register.ZMM0, Code.EVEX_Vscatterpf0dps_VM32Z_k1, TupleType.Tuple1_Scalar, MemorySize.Float32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							invalid,
							new OpCodeHandler_EVEX_VSIB_k1(Register.YMM0, Code.EVEX_Vscatterpf0dpd_VM32Y_k1, TupleType.Tuple1_Scalar, MemorySize.Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							invalid,
							new OpCodeHandler_EVEX_VSIB_k1(Register.ZMM0, Code.EVEX_Vscatterpf1dps_VM32Z_k1, TupleType.Tuple1_Scalar, MemorySize.Float32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							invalid,
							new OpCodeHandler_EVEX_VSIB_k1(Register.YMM0, Code.EVEX_Vscatterpf1dpd_VM32Y_k1, TupleType.Tuple1_Scalar, MemorySize.Float64)
						)
					),
					invalid,
					invalid
				),
				invalid,
			};

			var handlers_Grp_0F38C7 = new OpCodeHandler[8] {
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							invalid,
							new OpCodeHandler_EVEX_VSIB_k1(Register.ZMM0, Code.EVEX_Vgatherpf0qps_VM64Z_k1, TupleType.Tuple1_Scalar, MemorySize.Float32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							invalid,
							new OpCodeHandler_EVEX_VSIB_k1(Register.ZMM0, Code.EVEX_Vgatherpf0qpd_VM64Z_k1, TupleType.Tuple1_Scalar, MemorySize.Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							invalid,
							new OpCodeHandler_EVEX_VSIB_k1(Register.ZMM0, Code.EVEX_Vgatherpf1qps_VM64Z_k1, TupleType.Tuple1_Scalar, MemorySize.Float32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							invalid,
							new OpCodeHandler_EVEX_VSIB_k1(Register.ZMM0, Code.EVEX_Vgatherpf1qpd_VM64Z_k1, TupleType.Tuple1_Scalar, MemorySize.Float64)
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
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							invalid,
							new OpCodeHandler_EVEX_VSIB_k1(Register.ZMM0, Code.EVEX_Vscatterpf0qps_VM64Z_k1, TupleType.Tuple1_Scalar, MemorySize.Float32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							invalid,
							new OpCodeHandler_EVEX_VSIB_k1(Register.ZMM0, Code.EVEX_Vscatterpf0qpd_VM64Z_k1, TupleType.Tuple1_Scalar, MemorySize.Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							invalid,
							new OpCodeHandler_EVEX_VSIB_k1(Register.ZMM0, Code.EVEX_Vscatterpf1qps_VM64Z_k1, TupleType.Tuple1_Scalar, MemorySize.Float32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							invalid,
							new OpCodeHandler_EVEX_VSIB_k1(Register.ZMM0, Code.EVEX_Vscatterpf1qpd_VM64Z_k1, TupleType.Tuple1_Scalar, MemorySize.Float64)
						)
					),
					invalid,
					invalid
				),
				invalid,
			};

			ThreeByteHandlers_0F38XX = new OpCodeHandler[0x100] {
				// 00
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpshufb_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_UInt8),
						new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpshufb_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_UInt8),
						new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpshufb_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_UInt8)
					),
					invalid,
					invalid
				),
				invalid,
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpmaddubsw_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int8),
						new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpmaddubsw_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int8),
						new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpmaddubsw_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int8)
					),
					invalid,
					invalid
				),
				invalid,
				invalid,
				invalid,

				// 08
				invalid,
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpmulhrsw_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int16),
						new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpmulhrsw_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int16),
						new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpmulhrsw_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpermilps_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpermilps_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpermilps_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpermilpd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpermilpd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpermilpd_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64)
						)
					),
					invalid,
					invalid
				),
				invalid,
				invalid,

				// 10
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpsrlvw_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_UInt16),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpsrlvw_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_UInt16),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpsrlvw_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_UInt16)
						)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.XMM0, Code.EVEX_Vpmovuswb_WX_k1z_VX, TupleType.Half_Mem_128, MemorySize.Packed64_UInt8),
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.YMM0, Code.EVEX_Vpmovuswb_WX_k1z_VY, TupleType.Half_Mem_256, MemorySize.Packed128_UInt8),
							new OpCodeHandler_EVEX_WkV(Register.YMM0, Register.ZMM0, Code.EVEX_Vpmovuswb_WY_k1z_VZ, TupleType.Half_Mem_512, MemorySize.Packed256_UInt8)
						),
						invalid
					),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpsravw_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_UInt16),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpsravw_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_UInt16),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpsravw_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_UInt16)
						)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.XMM0, Code.EVEX_Vpmovusdb_WX_k1z_VX, TupleType.Quarter_Mem_128, MemorySize.Packed32_UInt8),
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.YMM0, Code.EVEX_Vpmovusdb_WX_k1z_VY, TupleType.Quarter_Mem_256, MemorySize.Packed64_UInt8),
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.ZMM0, Code.EVEX_Vpmovusdb_WX_k1z_VZ, TupleType.Quarter_Mem_512, MemorySize.Packed128_UInt8)
						),
						invalid
					),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpsllvw_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_UInt16),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpsllvw_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_UInt16),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpsllvw_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_UInt16)
						)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.XMM0, Code.EVEX_Vpmovusqb_WX_k1z_VX, TupleType.Eighth_Mem_128, MemorySize.Packed16_UInt8),
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.YMM0, Code.EVEX_Vpmovusqb_WX_k1z_VY, TupleType.Eighth_Mem_256, MemorySize.Packed32_UInt8),
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.ZMM0, Code.EVEX_Vpmovusqb_WX_k1z_VZ, TupleType.Eighth_Mem_512, MemorySize.Packed64_UInt8)
						),
						invalid
					),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkW_er(Register.XMM0, Register.XMM0, Code.EVEX_Vcvtph2ps_VX_k1z_WX, TupleType.Half_Mem_128, MemorySize.Packed64_Float16, onlySAE: true),
							new OpCodeHandler_EVEX_VkW_er(Register.YMM0, Register.XMM0, Code.EVEX_Vcvtph2ps_VY_k1z_WX, TupleType.Half_Mem_256, MemorySize.Packed128_Float16, onlySAE: true),
							new OpCodeHandler_EVEX_VkW_er(Register.ZMM0, Register.YMM0, Code.EVEX_Vcvtph2ps_VZ_k1z_WY_sae, TupleType.Half_Mem_512, MemorySize.Packed256_Float16, onlySAE: true)
						),
						invalid
					),
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.XMM0, Code.EVEX_Vpmovusdw_WX_k1z_VX, TupleType.Half_Mem_128, MemorySize.Packed64_UInt16),
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.YMM0, Code.EVEX_Vpmovusdw_WX_k1z_VY, TupleType.Half_Mem_256, MemorySize.Packed128_UInt16),
							new OpCodeHandler_EVEX_WkV(Register.YMM0, Register.ZMM0, Code.EVEX_Vpmovusdw_WY_k1z_VZ, TupleType.Half_Mem_512, MemorySize.Packed256_UInt16)
						),
						invalid
					),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vprorvd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_UInt32, MemorySize.Broadcast128_UInt32),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vprorvd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_UInt32, MemorySize.Broadcast256_UInt32),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vprorvd_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_UInt32, MemorySize.Broadcast512_UInt32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vprorvq_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_UInt64, MemorySize.Broadcast128_UInt64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vprorvq_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_UInt64, MemorySize.Broadcast256_UInt64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vprorvq_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_UInt64, MemorySize.Broadcast512_UInt64)
						)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.XMM0, Code.EVEX_Vpmovusqw_WX_k1z_VX, TupleType.Quarter_Mem_128, MemorySize.Packed32_UInt16),
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.YMM0, Code.EVEX_Vpmovusqw_WX_k1z_VY, TupleType.Quarter_Mem_256, MemorySize.Packed64_UInt16),
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.ZMM0, Code.EVEX_Vpmovusqw_WX_k1z_VZ, TupleType.Quarter_Mem_512, MemorySize.Packed128_UInt16)
						),
						invalid
					),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vprolvd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_UInt32, MemorySize.Broadcast128_UInt32),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vprolvd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_UInt32, MemorySize.Broadcast256_UInt32),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vprolvd_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_UInt32, MemorySize.Broadcast512_UInt32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vprolvq_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_UInt64, MemorySize.Broadcast128_UInt64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vprolvq_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_UInt64, MemorySize.Broadcast256_UInt64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vprolvq_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_UInt64, MemorySize.Broadcast512_UInt64)
						)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.XMM0, Code.EVEX_Vpmovusqd_WX_k1z_VX, TupleType.Half_Mem_128, MemorySize.Packed64_UInt32),
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.YMM0, Code.EVEX_Vpmovusqd_WX_k1z_VY, TupleType.Half_Mem_256, MemorySize.Packed128_UInt32),
							new OpCodeHandler_EVEX_WkV(Register.YMM0, Register.ZMM0, Code.EVEX_Vpmovusqd_WY_k1z_VZ, TupleType.Half_Mem_512, MemorySize.Packed256_UInt32)
						),
						invalid
					),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpermps_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpermps_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpermpd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpermpd_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64)
						)
					),
					invalid,
					invalid
				),
				invalid,

				// 18
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkW(Register.XMM0, Register.XMM0, Code.EVEX_Vbroadcastss_VX_k1z_WX, TupleType.Tuple1_Scalar, MemorySize.Float32),
							new OpCodeHandler_EVEX_VkW(Register.YMM0, Register.XMM0, Code.EVEX_Vbroadcastss_VY_k1z_WX, TupleType.Tuple1_Scalar, MemorySize.Float32),
							new OpCodeHandler_EVEX_VkW(Register.ZMM0, Register.XMM0, Code.EVEX_Vbroadcastss_VZ_k1z_WX, TupleType.Tuple1_Scalar, MemorySize.Float32)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							new OpCodeHandler_EVEX_VkW(Register.YMM0, Register.XMM0, Code.EVEX_Vbroadcastf32x2_VY_k1z_WX, TupleType.Tuple2, MemorySize.Packed64_Float32),
							new OpCodeHandler_EVEX_VkW(Register.ZMM0, Register.XMM0, Code.EVEX_Vbroadcastf32x2_VZ_k1z_WX, TupleType.Tuple2, MemorySize.Packed64_Float32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							new OpCodeHandler_EVEX_VkW(Register.YMM0, Register.XMM0, Code.EVEX_Vbroadcastsd_VY_k1z_WX, TupleType.Tuple1_Scalar, MemorySize.Float64),
							new OpCodeHandler_EVEX_VkW(Register.ZMM0, Register.XMM0, Code.EVEX_Vbroadcastsd_VZ_k1z_WX, TupleType.Tuple1_Scalar, MemorySize.Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							new OpCodeHandler_EVEX_VkM(Register.YMM0, Code.EVEX_Vbroadcastf32x4_VY_k1z_M, TupleType.Tuple4, MemorySize.Packed128_Float32),
							new OpCodeHandler_EVEX_VkM(Register.ZMM0, Code.EVEX_Vbroadcastf32x4_VZ_k1z_M, TupleType.Tuple4, MemorySize.Packed128_Float32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							new OpCodeHandler_EVEX_VkM(Register.YMM0, Code.EVEX_Vbroadcastf64x2_VY_k1z_M, TupleType.Tuple2, MemorySize.Packed128_Float64),
							new OpCodeHandler_EVEX_VkM(Register.ZMM0, Code.EVEX_Vbroadcastf64x2_VZ_k1z_M, TupleType.Tuple2, MemorySize.Packed128_Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							invalid,
							new OpCodeHandler_EVEX_VkM(Register.ZMM0, Code.EVEX_Vbroadcastf32x8_VZ_k1z_M, TupleType.Tuple8, MemorySize.Packed256_Float32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							invalid,
							new OpCodeHandler_EVEX_VkM(Register.ZMM0, Code.EVEX_Vbroadcastf64x4_VZ_k1z_M, TupleType.Tuple4, MemorySize.Packed256_Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkW(Register.XMM0, Code.EVEX_Vpabsb_VX_k1z_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int8),
						new OpCodeHandler_EVEX_VkW(Register.YMM0, Code.EVEX_Vpabsb_VY_k1z_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int8),
						new OpCodeHandler_EVEX_VkW(Register.ZMM0, Code.EVEX_Vpabsb_VZ_k1z_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkW(Register.XMM0, Code.EVEX_Vpabsw_VX_k1z_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int16),
						new OpCodeHandler_EVEX_VkW(Register.YMM0, Code.EVEX_Vpabsw_VY_k1z_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int16),
						new OpCodeHandler_EVEX_VkW(Register.ZMM0, Code.EVEX_Vpabsw_VZ_k1z_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkW(Register.XMM0, Code.EVEX_Vpabsd_VX_k1z_WX_b, TupleType.Full_128, MemorySize.Packed128_Int32, MemorySize.Broadcast128_Int32),
							new OpCodeHandler_EVEX_VkW(Register.YMM0, Code.EVEX_Vpabsd_VY_k1z_WY_b, TupleType.Full_256, MemorySize.Packed256_Int32, MemorySize.Broadcast256_Int32),
							new OpCodeHandler_EVEX_VkW(Register.ZMM0, Code.EVEX_Vpabsd_VZ_k1z_WZ_b, TupleType.Full_512, MemorySize.Packed512_Int32, MemorySize.Broadcast512_Int32)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkW(Register.XMM0, Code.EVEX_Vpabsq_VX_k1z_WX_b, TupleType.Full_128, MemorySize.Packed128_Int64, MemorySize.Broadcast128_Int64),
							new OpCodeHandler_EVEX_VkW(Register.YMM0, Code.EVEX_Vpabsq_VY_k1z_WY_b, TupleType.Full_256, MemorySize.Packed256_Int64, MemorySize.Broadcast256_Int64),
							new OpCodeHandler_EVEX_VkW(Register.ZMM0, Code.EVEX_Vpabsq_VZ_k1z_WZ_b, TupleType.Full_512, MemorySize.Packed512_Int64, MemorySize.Broadcast512_Int64)
						)
					),
					invalid,
					invalid
				),

				// 20
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkW(Register.XMM0, Register.XMM0, Code.EVEX_Vpmovsxbw_VX_k1z_WX, TupleType.Half_Mem_128, MemorySize.Packed64_Int8),
						new OpCodeHandler_EVEX_VkW(Register.YMM0, Register.XMM0, Code.EVEX_Vpmovsxbw_VY_k1z_WX, TupleType.Half_Mem_256, MemorySize.Packed128_Int8),
						new OpCodeHandler_EVEX_VkW(Register.ZMM0, Register.YMM0, Code.EVEX_Vpmovsxbw_VZ_k1z_WY, TupleType.Half_Mem_512, MemorySize.Packed256_Int8)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.XMM0, Code.EVEX_Vpmovswb_WX_k1z_VX, TupleType.Half_Mem_128, MemorySize.Packed64_Int8),
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.YMM0, Code.EVEX_Vpmovswb_WX_k1z_VY, TupleType.Half_Mem_256, MemorySize.Packed128_Int8),
							new OpCodeHandler_EVEX_WkV(Register.YMM0, Register.ZMM0, Code.EVEX_Vpmovswb_WY_k1z_VZ, TupleType.Half_Mem_512, MemorySize.Packed256_Int8)
						),
						invalid
					),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkW(Register.XMM0, Register.XMM0, Code.EVEX_Vpmovsxbd_VX_k1z_WX, TupleType.Quarter_Mem_128, MemorySize.Packed32_Int8),
						new OpCodeHandler_EVEX_VkW(Register.YMM0, Register.XMM0, Code.EVEX_Vpmovsxbd_VY_k1z_WX, TupleType.Quarter_Mem_256, MemorySize.Packed64_Int8),
						new OpCodeHandler_EVEX_VkW(Register.ZMM0, Register.XMM0, Code.EVEX_Vpmovsxbd_VZ_k1z_WX, TupleType.Quarter_Mem_512, MemorySize.Packed128_Int8)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.XMM0, Code.EVEX_Vpmovsdb_WX_k1z_VX, TupleType.Quarter_Mem_128, MemorySize.Packed32_Int8),
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.YMM0, Code.EVEX_Vpmovsdb_WX_k1z_VY, TupleType.Quarter_Mem_256, MemorySize.Packed64_Int8),
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.ZMM0, Code.EVEX_Vpmovsdb_WX_k1z_VZ, TupleType.Quarter_Mem_512, MemorySize.Packed128_Int8)
						),
						invalid
					),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkW(Register.XMM0, Register.XMM0, Code.EVEX_Vpmovsxbq_VX_k1z_WX, TupleType.Eighth_Mem_128, MemorySize.Packed16_Int8),
						new OpCodeHandler_EVEX_VkW(Register.YMM0, Register.XMM0, Code.EVEX_Vpmovsxbq_VY_k1z_WX, TupleType.Eighth_Mem_256, MemorySize.Packed32_Int8),
						new OpCodeHandler_EVEX_VkW(Register.ZMM0, Register.XMM0, Code.EVEX_Vpmovsxbq_VZ_k1z_WX, TupleType.Eighth_Mem_512, MemorySize.Packed64_Int8)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.XMM0, Code.EVEX_Vpmovsqb_WX_k1z_VX, TupleType.Eighth_Mem_128, MemorySize.Packed16_Int8),
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.YMM0, Code.EVEX_Vpmovsqb_WX_k1z_VY, TupleType.Eighth_Mem_256, MemorySize.Packed32_Int8),
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.ZMM0, Code.EVEX_Vpmovsqb_WX_k1z_VZ, TupleType.Eighth_Mem_512, MemorySize.Packed64_Int8)
						),
						invalid
					),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkW(Register.XMM0, Register.XMM0, Code.EVEX_Vpmovsxwd_VX_k1z_WX, TupleType.Half_Mem_128, MemorySize.Packed64_Int16),
						new OpCodeHandler_EVEX_VkW(Register.YMM0, Register.XMM0, Code.EVEX_Vpmovsxwd_VY_k1z_WX, TupleType.Half_Mem_256, MemorySize.Packed128_Int16),
						new OpCodeHandler_EVEX_VkW(Register.ZMM0, Register.YMM0, Code.EVEX_Vpmovsxwd_VZ_k1z_WY, TupleType.Half_Mem_512, MemorySize.Packed256_Int16)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.XMM0, Code.EVEX_Vpmovsdw_WX_k1z_VX, TupleType.Half_Mem_128, MemorySize.Packed64_Int16),
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.YMM0, Code.EVEX_Vpmovsdw_WX_k1z_VY, TupleType.Half_Mem_256, MemorySize.Packed128_Int16),
							new OpCodeHandler_EVEX_WkV(Register.YMM0, Register.ZMM0, Code.EVEX_Vpmovsdw_WY_k1z_VZ, TupleType.Half_Mem_512, MemorySize.Packed256_Int16)
						),
						invalid
					),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkW(Register.XMM0, Register.XMM0, Code.EVEX_Vpmovsxwq_VX_k1z_WX, TupleType.Quarter_Mem_128, MemorySize.Packed32_Int16),
						new OpCodeHandler_EVEX_VkW(Register.YMM0, Register.XMM0, Code.EVEX_Vpmovsxwq_VY_k1z_WX, TupleType.Quarter_Mem_256, MemorySize.Packed64_Int16),
						new OpCodeHandler_EVEX_VkW(Register.ZMM0, Register.XMM0, Code.EVEX_Vpmovsxwq_VZ_k1z_WX, TupleType.Quarter_Mem_512, MemorySize.Packed128_Int16)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.XMM0, Code.EVEX_Vpmovsqw_WX_k1z_VX, TupleType.Quarter_Mem_128, MemorySize.Packed32_Int16),
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.YMM0, Code.EVEX_Vpmovsqw_WX_k1z_VY, TupleType.Quarter_Mem_256, MemorySize.Packed64_Int16),
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.ZMM0, Code.EVEX_Vpmovsqw_WX_k1z_VZ, TupleType.Quarter_Mem_512, MemorySize.Packed128_Int16)
						),
						invalid
					),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkW(Register.XMM0, Register.XMM0, Code.EVEX_Vpmovsxdq_VX_k1z_WX, TupleType.Half_Mem_128, MemorySize.Packed64_Int32),
							new OpCodeHandler_EVEX_VkW(Register.YMM0, Register.XMM0, Code.EVEX_Vpmovsxdq_VY_k1z_WX, TupleType.Half_Mem_256, MemorySize.Packed128_Int32),
							new OpCodeHandler_EVEX_VkW(Register.ZMM0, Register.YMM0, Code.EVEX_Vpmovsxdq_VZ_k1z_WY, TupleType.Half_Mem_512, MemorySize.Packed256_Int32)
						),
						invalid
					),
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.XMM0, Code.EVEX_Vpmovsqd_WX_k1z_VX, TupleType.Half_Mem_128, MemorySize.Packed64_Int32),
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.YMM0, Code.EVEX_Vpmovsqd_WX_k1z_VY, TupleType.Half_Mem_256, MemorySize.Packed128_Int32),
							new OpCodeHandler_EVEX_WkV(Register.YMM0, Register.ZMM0, Code.EVEX_Vpmovsqd_WY_k1z_VZ, TupleType.Half_Mem_512, MemorySize.Packed256_Int32)
						),
						invalid
					),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_KkHW(Register.XMM0, Code.EVEX_Vptestmb_VK_k1_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_UInt8),
							new OpCodeHandler_EVEX_KkHW(Register.YMM0, Code.EVEX_Vptestmb_VK_k1_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_UInt8),
							new OpCodeHandler_EVEX_KkHW(Register.ZMM0, Code.EVEX_Vptestmb_VK_k1_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_UInt8)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_KkHW(Register.XMM0, Code.EVEX_Vptestmw_VK_k1_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_UInt16),
							new OpCodeHandler_EVEX_KkHW(Register.YMM0, Code.EVEX_Vptestmw_VK_k1_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_UInt16),
							new OpCodeHandler_EVEX_KkHW(Register.ZMM0, Code.EVEX_Vptestmw_VK_k1_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_UInt16)
						)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_KkHW(Register.XMM0, Code.EVEX_Vptestnmb_VK_k1_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_UInt8),
							new OpCodeHandler_EVEX_KkHW(Register.YMM0, Code.EVEX_Vptestnmb_VK_k1_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_UInt8),
							new OpCodeHandler_EVEX_KkHW(Register.ZMM0, Code.EVEX_Vptestnmb_VK_k1_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_UInt8)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_KkHW(Register.XMM0, Code.EVEX_Vptestnmw_VK_k1_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_UInt16),
							new OpCodeHandler_EVEX_KkHW(Register.YMM0, Code.EVEX_Vptestnmw_VK_k1_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_UInt16),
							new OpCodeHandler_EVEX_KkHW(Register.ZMM0, Code.EVEX_Vptestnmw_VK_k1_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_UInt16)
						)
					),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_KkHW(Register.XMM0, Code.EVEX_Vptestmd_VK_k1_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_UInt32, MemorySize.Broadcast128_UInt32),
							new OpCodeHandler_EVEX_KkHW(Register.YMM0, Code.EVEX_Vptestmd_VK_k1_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_UInt32, MemorySize.Broadcast256_UInt32),
							new OpCodeHandler_EVEX_KkHW(Register.ZMM0, Code.EVEX_Vptestmd_VK_k1_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_UInt32, MemorySize.Broadcast512_UInt32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_KkHW(Register.XMM0, Code.EVEX_Vptestmq_VK_k1_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_UInt64, MemorySize.Broadcast128_UInt64),
							new OpCodeHandler_EVEX_KkHW(Register.YMM0, Code.EVEX_Vptestmq_VK_k1_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_UInt64, MemorySize.Broadcast256_UInt64),
							new OpCodeHandler_EVEX_KkHW(Register.ZMM0, Code.EVEX_Vptestmq_VK_k1_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_UInt64, MemorySize.Broadcast512_UInt64)
						)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_KkHW(Register.XMM0, Code.EVEX_Vptestnmd_VK_k1_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_UInt32, MemorySize.Broadcast128_UInt32),
							new OpCodeHandler_EVEX_KkHW(Register.YMM0, Code.EVEX_Vptestnmd_VK_k1_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_UInt32, MemorySize.Broadcast256_UInt32),
							new OpCodeHandler_EVEX_KkHW(Register.ZMM0, Code.EVEX_Vptestnmd_VK_k1_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_UInt32, MemorySize.Broadcast512_UInt32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_KkHW(Register.XMM0, Code.EVEX_Vptestnmq_VK_k1_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_UInt64, MemorySize.Broadcast128_UInt64),
							new OpCodeHandler_EVEX_KkHW(Register.YMM0, Code.EVEX_Vptestnmq_VK_k1_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_UInt64, MemorySize.Broadcast256_UInt64),
							new OpCodeHandler_EVEX_KkHW(Register.ZMM0, Code.EVEX_Vptestnmq_VK_k1_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_UInt64, MemorySize.Broadcast512_UInt64)
						)
					),
					invalid
				),

				// 28
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpmuldq_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Int32, MemorySize.Broadcast128_Int64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpmuldq_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Int32, MemorySize.Broadcast256_Int64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpmuldq_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Int32, MemorySize.Broadcast512_Int64)
						)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VK(Register.XMM0, Code.EVEX_Vpmovm2b_VX_RK),
							new OpCodeHandler_EVEX_VK(Register.YMM0, Code.EVEX_Vpmovm2b_VY_RK),
							new OpCodeHandler_EVEX_VK(Register.ZMM0, Code.EVEX_Vpmovm2b_VZ_RK)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VK(Register.XMM0, Code.EVEX_Vpmovm2w_VX_RK),
							new OpCodeHandler_EVEX_VK(Register.YMM0, Code.EVEX_Vpmovm2w_VY_RK),
							new OpCodeHandler_EVEX_VK(Register.ZMM0, Code.EVEX_Vpmovm2w_VZ_RK)
						)
					),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_KkHW(Register.XMM0, Code.EVEX_Vpcmpeqq_VK_k1_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Int64, MemorySize.Broadcast128_Int64),
							new OpCodeHandler_EVEX_KkHW(Register.YMM0, Code.EVEX_Vpcmpeqq_VK_k1_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Int64, MemorySize.Broadcast256_Int64),
							new OpCodeHandler_EVEX_KkHW(Register.ZMM0, Code.EVEX_Vpcmpeqq_VK_k1_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Int64, MemorySize.Broadcast512_Int64)
						)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_KR(Register.XMM0, Code.EVEX_Vpmovb2m_VK_RX),
							new OpCodeHandler_EVEX_KR(Register.YMM0, Code.EVEX_Vpmovb2m_VK_RY),
							new OpCodeHandler_EVEX_KR(Register.ZMM0, Code.EVEX_Vpmovb2m_VK_RZ)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_KR(Register.XMM0, Code.EVEX_Vpmovw2m_VK_RX),
							new OpCodeHandler_EVEX_KR(Register.YMM0, Code.EVEX_Vpmovw2m_VK_RY),
							new OpCodeHandler_EVEX_KR(Register.ZMM0, Code.EVEX_Vpmovw2m_VK_RZ)
						)
					),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VM(Register.XMM0, Code.EVEX_Vmovntdqa_VX_M, TupleType.Full_Mem_128, MemorySize.UInt128),
							new OpCodeHandler_EVEX_VM(Register.YMM0, Code.EVEX_Vmovntdqa_VY_M, TupleType.Full_Mem_256, MemorySize.UInt256),
							new OpCodeHandler_EVEX_VM(Register.ZMM0, Code.EVEX_Vmovntdqa_VZ_M, TupleType.Full_Mem_512, MemorySize.UInt512)
						),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VK(Register.XMM0, Code.EVEX_Vpbroadcastmb2q_VX_RK),
							new OpCodeHandler_EVEX_VK(Register.YMM0, Code.EVEX_Vpbroadcastmb2q_VY_RK),
							new OpCodeHandler_EVEX_VK(Register.ZMM0, Code.EVEX_Vpbroadcastmb2q_VZ_RK)
						)
					),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpackusdw_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Int32, MemorySize.Broadcast128_Int32),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpackusdw_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Int32, MemorySize.Broadcast256_Int32),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpackusdw_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Int32, MemorySize.Broadcast512_Int32)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vscalefps_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vscalefps_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vscalefps_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32, onlySAE: false)
						),
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vscalefpd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vscalefpd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vscalefpd_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: false)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vscalefss_VX_k1z_HX_WX_er, TupleType.Tuple1_Scalar, MemorySize.Float32, onlySAE: false),
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vscalefsd_VX_k1z_HX_WX_er, TupleType.Tuple1_Scalar, MemorySize.Float64, onlySAE: false)
					),
					invalid,
					invalid
				),
				invalid,
				invalid,

				// 30
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkW(Register.XMM0, Register.XMM0, Code.EVEX_Vpmovzxbw_VX_k1z_WX, TupleType.Half_Mem_128, MemorySize.Packed64_UInt8),
						new OpCodeHandler_EVEX_VkW(Register.YMM0, Register.XMM0, Code.EVEX_Vpmovzxbw_VY_k1z_WX, TupleType.Half_Mem_256, MemorySize.Packed128_UInt8),
						new OpCodeHandler_EVEX_VkW(Register.ZMM0, Register.YMM0, Code.EVEX_Vpmovzxbw_VZ_k1z_WY, TupleType.Half_Mem_512, MemorySize.Packed256_UInt8)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.XMM0, Code.EVEX_Vpmovwb_WX_k1z_VX, TupleType.Half_Mem_128, MemorySize.Packed64_Int8),
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.YMM0, Code.EVEX_Vpmovwb_WX_k1z_VY, TupleType.Half_Mem_256, MemorySize.Packed128_Int8),
							new OpCodeHandler_EVEX_WkV(Register.YMM0, Register.ZMM0, Code.EVEX_Vpmovwb_WY_k1z_VZ, TupleType.Half_Mem_512, MemorySize.Packed256_Int8)
						),
						invalid
					),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkW(Register.XMM0, Register.XMM0, Code.EVEX_Vpmovzxbd_VX_k1z_WX, TupleType.Quarter_Mem_128, MemorySize.Packed32_UInt8),
						new OpCodeHandler_EVEX_VkW(Register.YMM0, Register.XMM0, Code.EVEX_Vpmovzxbd_VY_k1z_WX, TupleType.Quarter_Mem_256, MemorySize.Packed64_UInt8),
						new OpCodeHandler_EVEX_VkW(Register.ZMM0, Register.XMM0, Code.EVEX_Vpmovzxbd_VZ_k1z_WX, TupleType.Quarter_Mem_512, MemorySize.Packed128_UInt8)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.XMM0, Code.EVEX_Vpmovdb_WX_k1z_VX, TupleType.Quarter_Mem_128, MemorySize.Packed32_Int8),
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.YMM0, Code.EVEX_Vpmovdb_WX_k1z_VY, TupleType.Quarter_Mem_256, MemorySize.Packed64_Int8),
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.ZMM0, Code.EVEX_Vpmovdb_WX_k1z_VZ, TupleType.Quarter_Mem_512, MemorySize.Packed128_Int8)
						),
						invalid
					),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkW(Register.XMM0, Register.XMM0, Code.EVEX_Vpmovzxbq_VX_k1z_WX, TupleType.Eighth_Mem_128, MemorySize.Packed16_UInt8),
						new OpCodeHandler_EVEX_VkW(Register.YMM0, Register.XMM0, Code.EVEX_Vpmovzxbq_VY_k1z_WX, TupleType.Eighth_Mem_256, MemorySize.Packed32_UInt8),
						new OpCodeHandler_EVEX_VkW(Register.ZMM0, Register.XMM0, Code.EVEX_Vpmovzxbq_VZ_k1z_WX, TupleType.Eighth_Mem_512, MemorySize.Packed64_UInt8)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.XMM0, Code.EVEX_Vpmovqb_WX_k1z_VX, TupleType.Eighth_Mem_128, MemorySize.Packed16_Int8),
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.YMM0, Code.EVEX_Vpmovqb_WX_k1z_VY, TupleType.Eighth_Mem_256, MemorySize.Packed32_Int8),
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.ZMM0, Code.EVEX_Vpmovqb_WX_k1z_VZ, TupleType.Eighth_Mem_512, MemorySize.Packed64_Int8)
						),
						invalid
					),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkW(Register.XMM0, Register.XMM0, Code.EVEX_Vpmovzxwd_VX_k1z_WX, TupleType.Half_Mem_128, MemorySize.Packed64_UInt16),
						new OpCodeHandler_EVEX_VkW(Register.YMM0, Register.XMM0, Code.EVEX_Vpmovzxwd_VY_k1z_WX, TupleType.Half_Mem_256, MemorySize.Packed128_UInt16),
						new OpCodeHandler_EVEX_VkW(Register.ZMM0, Register.YMM0, Code.EVEX_Vpmovzxwd_VZ_k1z_WY, TupleType.Half_Mem_512, MemorySize.Packed256_UInt16)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.XMM0, Code.EVEX_Vpmovdw_WX_k1z_VX, TupleType.Half_Mem_128, MemorySize.Packed64_Int16),
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.YMM0, Code.EVEX_Vpmovdw_WX_k1z_VY, TupleType.Half_Mem_256, MemorySize.Packed128_Int16),
							new OpCodeHandler_EVEX_WkV(Register.YMM0, Register.ZMM0, Code.EVEX_Vpmovdw_WY_k1z_VZ, TupleType.Half_Mem_512, MemorySize.Packed256_Int16)
						),
						invalid
					),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkW(Register.XMM0, Register.XMM0, Code.EVEX_Vpmovzxwq_VX_k1z_WX, TupleType.Quarter_Mem_128, MemorySize.Packed32_UInt16),
						new OpCodeHandler_EVEX_VkW(Register.YMM0, Register.XMM0, Code.EVEX_Vpmovzxwq_VY_k1z_WX, TupleType.Quarter_Mem_256, MemorySize.Packed64_UInt16),
						new OpCodeHandler_EVEX_VkW(Register.ZMM0, Register.XMM0, Code.EVEX_Vpmovzxwq_VZ_k1z_WX, TupleType.Quarter_Mem_512, MemorySize.Packed128_UInt16)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.XMM0, Code.EVEX_Vpmovqw_WX_k1z_VX, TupleType.Quarter_Mem_128, MemorySize.Packed32_Int16),
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.YMM0, Code.EVEX_Vpmovqw_WX_k1z_VY, TupleType.Quarter_Mem_256, MemorySize.Packed64_Int16),
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.ZMM0, Code.EVEX_Vpmovqw_WX_k1z_VZ, TupleType.Quarter_Mem_512, MemorySize.Packed128_Int16)
						),
						invalid
					),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkW(Register.XMM0, Register.XMM0, Code.EVEX_Vpmovzxdq_VX_k1z_WX, TupleType.Half_Mem_128, MemorySize.Packed64_UInt32),
						new OpCodeHandler_EVEX_VkW(Register.YMM0, Register.XMM0, Code.EVEX_Vpmovzxdq_VY_k1z_WX, TupleType.Half_Mem_256, MemorySize.Packed128_UInt32),
						new OpCodeHandler_EVEX_VkW(Register.ZMM0, Register.YMM0, Code.EVEX_Vpmovzxdq_VZ_k1z_WY, TupleType.Half_Mem_512, MemorySize.Packed256_UInt32)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.XMM0, Code.EVEX_Vpmovqd_WX_k1z_VX, TupleType.Half_Mem_128, MemorySize.Packed64_Int32),
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Register.YMM0, Code.EVEX_Vpmovqd_WX_k1z_VY, TupleType.Half_Mem_256, MemorySize.Packed128_Int32),
							new OpCodeHandler_EVEX_WkV(Register.YMM0, Register.ZMM0, Code.EVEX_Vpmovqd_WY_k1z_VZ, TupleType.Half_Mem_512, MemorySize.Packed256_Int32)
						),
						invalid
					),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpermd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Int32, MemorySize.Broadcast256_Int32),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpermd_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Int32, MemorySize.Broadcast512_Int32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpermq_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Int64, MemorySize.Broadcast256_Int64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpermq_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Int64, MemorySize.Broadcast512_Int64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_KkHW(Register.XMM0, Code.EVEX_Vpcmpgtq_VK_k1_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Int64, MemorySize.Broadcast128_Int64),
							new OpCodeHandler_EVEX_KkHW(Register.YMM0, Code.EVEX_Vpcmpgtq_VK_k1_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Int64, MemorySize.Broadcast256_Int64),
							new OpCodeHandler_EVEX_KkHW(Register.ZMM0, Code.EVEX_Vpcmpgtq_VK_k1_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Int64, MemorySize.Broadcast512_Int64)
						)
					),
					invalid,
					invalid
				),

				// 38
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpminsb_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int8),
						new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpminsb_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int8),
						new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpminsb_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int8)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VK(Register.XMM0, Code.EVEX_Vpmovm2d_VX_RK),
							new OpCodeHandler_EVEX_VK(Register.YMM0, Code.EVEX_Vpmovm2d_VY_RK),
							new OpCodeHandler_EVEX_VK(Register.ZMM0, Code.EVEX_Vpmovm2d_VZ_RK)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VK(Register.XMM0, Code.EVEX_Vpmovm2q_VX_RK),
							new OpCodeHandler_EVEX_VK(Register.YMM0, Code.EVEX_Vpmovm2q_VY_RK),
							new OpCodeHandler_EVEX_VK(Register.ZMM0, Code.EVEX_Vpmovm2q_VZ_RK)
						)
					),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpminsd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Int32, MemorySize.Broadcast128_Int32),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpminsd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Int32, MemorySize.Broadcast256_Int32),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpminsd_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Int32, MemorySize.Broadcast512_Int32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpminsq_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Int64, MemorySize.Broadcast128_Int64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpminsq_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Int64, MemorySize.Broadcast256_Int64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpminsq_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Int64, MemorySize.Broadcast512_Int64)
						)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_KR(Register.XMM0, Code.EVEX_Vpmovd2m_VK_RX),
							new OpCodeHandler_EVEX_KR(Register.YMM0, Code.EVEX_Vpmovd2m_VK_RY),
							new OpCodeHandler_EVEX_KR(Register.ZMM0, Code.EVEX_Vpmovd2m_VK_RZ)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_KR(Register.XMM0, Code.EVEX_Vpmovq2m_VK_RX),
							new OpCodeHandler_EVEX_KR(Register.YMM0, Code.EVEX_Vpmovq2m_VK_RY),
							new OpCodeHandler_EVEX_KR(Register.ZMM0, Code.EVEX_Vpmovq2m_VK_RZ)
						)
					),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpminuw_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_UInt16),
						new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpminuw_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_UInt16),
						new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpminuw_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_UInt16)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VK(Register.XMM0, Code.EVEX_Vpbroadcastmw2d_VX_RK),
							new OpCodeHandler_EVEX_VK(Register.YMM0, Code.EVEX_Vpbroadcastmw2d_VY_RK),
							new OpCodeHandler_EVEX_VK(Register.ZMM0, Code.EVEX_Vpbroadcastmw2d_VZ_RK)
						),
						invalid
					),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpminud_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_UInt32, MemorySize.Broadcast128_UInt32),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpminud_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_UInt32, MemorySize.Broadcast256_UInt32),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpminud_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_UInt32, MemorySize.Broadcast512_UInt32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpminuq_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_UInt64, MemorySize.Broadcast128_UInt64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpminuq_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_UInt64, MemorySize.Broadcast256_UInt64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpminuq_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_UInt64, MemorySize.Broadcast512_UInt64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpmaxsb_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int8),
						new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpmaxsb_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int8),
						new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpmaxsb_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpmaxsd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Int32, MemorySize.Broadcast128_Int32),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpmaxsd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Int32, MemorySize.Broadcast256_Int32),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpmaxsd_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Int32, MemorySize.Broadcast512_Int32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpmaxsq_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Int64, MemorySize.Broadcast128_Int64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpmaxsq_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Int64, MemorySize.Broadcast256_Int64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpmaxsq_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Int64, MemorySize.Broadcast512_Int64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpmaxuw_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_UInt16),
						new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpmaxuw_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_UInt16),
						new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpmaxuw_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_UInt16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpmaxud_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_UInt32, MemorySize.Broadcast128_UInt32),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpmaxud_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_UInt32, MemorySize.Broadcast256_UInt32),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpmaxud_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_UInt32, MemorySize.Broadcast512_UInt32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpmaxuq_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_UInt64, MemorySize.Broadcast128_UInt64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpmaxuq_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_UInt64, MemorySize.Broadcast256_UInt64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpmaxuq_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_UInt64, MemorySize.Broadcast512_UInt64)
						)
					),
					invalid,
					invalid
				),

				// 40
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpmulld_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Int32, MemorySize.Broadcast128_Int32),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpmulld_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Int32, MemorySize.Broadcast256_Int32),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpmulld_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Int32, MemorySize.Broadcast512_Int32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpmullq_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Int64, MemorySize.Broadcast128_Int64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpmullq_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Int64, MemorySize.Broadcast256_Int64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpmullq_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Int64, MemorySize.Broadcast512_Int64)
						)
					),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkW_er(Register.XMM0, Code.EVEX_Vgetexpps_VX_k1z_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: true),
							new OpCodeHandler_EVEX_VkW_er(Register.YMM0, Code.EVEX_Vgetexpps_VY_k1z_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: true),
							new OpCodeHandler_EVEX_VkW_er(Register.ZMM0, Code.EVEX_Vgetexpps_VZ_k1z_WZ_sae_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32, onlySAE: true)
						),
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkW_er(Register.XMM0, Code.EVEX_Vgetexppd_VX_k1z_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: true),
							new OpCodeHandler_EVEX_VkW_er(Register.YMM0, Code.EVEX_Vgetexppd_VY_k1z_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: true),
							new OpCodeHandler_EVEX_VkW_er(Register.ZMM0, Code.EVEX_Vgetexppd_VZ_k1z_WZ_sae_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: true)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vgetexpss_VX_k1z_HX_WX_sae, TupleType.Tuple1_Scalar, MemorySize.Float32, onlySAE: true),
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vgetexpsd_VX_k1z_HX_WX_sae, TupleType.Tuple1_Scalar, MemorySize.Float64, onlySAE: true)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkW(Register.XMM0, Code.EVEX_Vplzcntd_VX_k1z_WX_b, TupleType.Full_128, MemorySize.Packed128_UInt32, MemorySize.Broadcast128_UInt32),
							new OpCodeHandler_EVEX_VkW(Register.YMM0, Code.EVEX_Vplzcntd_VY_k1z_WY_b, TupleType.Full_256, MemorySize.Packed256_UInt32, MemorySize.Broadcast256_UInt32),
							new OpCodeHandler_EVEX_VkW(Register.ZMM0, Code.EVEX_Vplzcntd_VZ_k1z_WZ_b, TupleType.Full_512, MemorySize.Packed512_UInt32, MemorySize.Broadcast512_UInt32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkW(Register.XMM0, Code.EVEX_Vplzcntq_VX_k1z_WX_b, TupleType.Full_128, MemorySize.Packed128_UInt64, MemorySize.Broadcast128_UInt64),
							new OpCodeHandler_EVEX_VkW(Register.YMM0, Code.EVEX_Vplzcntq_VY_k1z_WY_b, TupleType.Full_256, MemorySize.Packed256_UInt64, MemorySize.Broadcast256_UInt64),
							new OpCodeHandler_EVEX_VkW(Register.ZMM0, Code.EVEX_Vplzcntq_VZ_k1z_WZ_b, TupleType.Full_512, MemorySize.Packed512_UInt64, MemorySize.Broadcast512_UInt64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpsrlvd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_UInt32, MemorySize.Broadcast128_UInt32),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpsrlvd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_UInt32, MemorySize.Broadcast256_UInt32),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpsrlvd_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_UInt32, MemorySize.Broadcast512_UInt32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpsrlvq_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_UInt64, MemorySize.Broadcast128_UInt64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpsrlvq_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_UInt64, MemorySize.Broadcast256_UInt64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpsrlvq_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_UInt64, MemorySize.Broadcast512_UInt64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpsravd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_UInt32, MemorySize.Broadcast128_UInt32),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpsravd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_UInt32, MemorySize.Broadcast256_UInt32),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpsravd_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_UInt32, MemorySize.Broadcast512_UInt32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpsravq_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_UInt64, MemorySize.Broadcast128_UInt64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpsravq_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_UInt64, MemorySize.Broadcast256_UInt64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpsravq_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_UInt64, MemorySize.Broadcast512_UInt64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpsllvd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_UInt32, MemorySize.Broadcast128_UInt32),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpsllvd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_UInt32, MemorySize.Broadcast256_UInt32),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpsllvd_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_UInt32, MemorySize.Broadcast512_UInt32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpsllvq_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_UInt64, MemorySize.Broadcast128_UInt64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpsllvq_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_UInt64, MemorySize.Broadcast256_UInt64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpsllvq_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_UInt64, MemorySize.Broadcast512_UInt64)
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
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkW(Register.XMM0, Code.EVEX_Vrcp14ps_VX_k1z_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32),
							new OpCodeHandler_EVEX_VkW(Register.YMM0, Code.EVEX_Vrcp14ps_VY_k1z_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32),
							new OpCodeHandler_EVEX_VkW(Register.ZMM0, Code.EVEX_Vrcp14ps_VZ_k1z_WZ_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkW(Register.XMM0, Code.EVEX_Vrcp14pd_VX_k1z_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64),
							new OpCodeHandler_EVEX_VkW(Register.YMM0, Code.EVEX_Vrcp14pd_VY_k1z_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64),
							new OpCodeHandler_EVEX_VkW(Register.ZMM0, Code.EVEX_Vrcp14pd_VZ_k1z_WZ_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vrcp14ss_VX_k1z_HX_WX, TupleType.Tuple1_Scalar, MemorySize.Float32),
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vrcp14sd_VX_k1z_HX_WX, TupleType.Tuple1_Scalar, MemorySize.Float64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkW(Register.XMM0, Code.EVEX_Vrsqrt14ps_VX_k1z_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32),
							new OpCodeHandler_EVEX_VkW(Register.YMM0, Code.EVEX_Vrsqrt14ps_VY_k1z_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32),
							new OpCodeHandler_EVEX_VkW(Register.ZMM0, Code.EVEX_Vrsqrt14ps_VZ_k1z_WZ_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkW(Register.XMM0, Code.EVEX_Vrsqrt14pd_VX_k1z_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64),
							new OpCodeHandler_EVEX_VkW(Register.YMM0, Code.EVEX_Vrsqrt14pd_VY_k1z_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64),
							new OpCodeHandler_EVEX_VkW(Register.ZMM0, Code.EVEX_Vrsqrt14pd_VZ_k1z_WZ_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vrsqrt14ss_VX_k1z_HX_WX, TupleType.Tuple1_Scalar, MemorySize.Float32),
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vrsqrt14sd_VX_k1z_HX_WX, TupleType.Tuple1_Scalar, MemorySize.Float64)
					),
					invalid,
					invalid
				),

				// 50
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					invalid,
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_VkHM(Register.ZMM0, Code.EVEX_Vp4dpwssd_VZ_k1z_HZP3_M, TupleType.Tuple1_4X, MemorySize.Packed128_Int16),
						invalid
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					invalid,
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_VkHM(Register.ZMM0, Code.EVEX_Vp4dpwssds_VZ_k1z_HZP3_M, TupleType.Tuple1_4X, MemorySize.Packed128_Int16),
						invalid
					)
				),
				invalid,
				invalid,
				invalid,
				invalid,

				// 58
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkW(Register.XMM0, Register.XMM0, Code.EVEX_Vpbroadcastd_VX_k1z_WX, TupleType.Tuple1_Scalar, MemorySize.Int32),
							new OpCodeHandler_EVEX_VkW(Register.YMM0, Register.XMM0, Code.EVEX_Vpbroadcastd_VY_k1z_WX, TupleType.Tuple1_Scalar, MemorySize.Int32),
							new OpCodeHandler_EVEX_VkW(Register.ZMM0, Register.XMM0, Code.EVEX_Vpbroadcastd_VZ_k1z_WX, TupleType.Tuple1_Scalar, MemorySize.Int32)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkW(Register.XMM0, Register.XMM0, Code.EVEX_Vbroadcasti32x2_VX_k1z_WX, TupleType.Tuple2, MemorySize.Packed64_Int32),
							new OpCodeHandler_EVEX_VkW(Register.YMM0, Register.XMM0, Code.EVEX_Vbroadcasti32x2_VY_k1z_WX, TupleType.Tuple2, MemorySize.Packed64_Int32),
							new OpCodeHandler_EVEX_VkW(Register.ZMM0, Register.XMM0, Code.EVEX_Vbroadcasti32x2_VZ_k1z_WX, TupleType.Tuple2, MemorySize.Packed64_Int32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkW(Register.XMM0, Register.XMM0, Code.EVEX_Vpbroadcastq_VX_k1z_WX, TupleType.Tuple1_Scalar, MemorySize.Int64),
							new OpCodeHandler_EVEX_VkW(Register.YMM0, Register.XMM0, Code.EVEX_Vpbroadcastq_VY_k1z_WX, TupleType.Tuple1_Scalar, MemorySize.Int64),
							new OpCodeHandler_EVEX_VkW(Register.ZMM0, Register.XMM0, Code.EVEX_Vpbroadcastq_VZ_k1z_WX, TupleType.Tuple1_Scalar, MemorySize.Int64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							new OpCodeHandler_EVEX_VkM(Register.YMM0, Code.EVEX_Vbroadcasti32x4_VY_k1z_M, TupleType.Tuple4, MemorySize.Packed128_Int32),
							new OpCodeHandler_EVEX_VkM(Register.ZMM0, Code.EVEX_Vbroadcasti32x4_VZ_k1z_M, TupleType.Tuple4, MemorySize.Packed128_Int32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							new OpCodeHandler_EVEX_VkM(Register.YMM0, Code.EVEX_Vbroadcasti64x2_VY_k1z_M, TupleType.Tuple2, MemorySize.Packed128_Int64),
							new OpCodeHandler_EVEX_VkM(Register.ZMM0, Code.EVEX_Vbroadcasti64x2_VZ_k1z_M, TupleType.Tuple2, MemorySize.Packed128_Int64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							invalid,
							new OpCodeHandler_EVEX_VkM(Register.ZMM0, Code.EVEX_Vbroadcasti32x8_VZ_k1z_M, TupleType.Tuple8, MemorySize.Packed256_Int32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							invalid,
							new OpCodeHandler_EVEX_VkM(Register.ZMM0, Code.EVEX_Vbroadcasti64x4_VZ_k1z_M, TupleType.Tuple4, MemorySize.Packed256_Int64)
						)
					),
					invalid,
					invalid
				),
				invalid,
				invalid,
				invalid,
				invalid,

				// 60
				invalid,
				invalid,
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpblendmd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Int32, MemorySize.Broadcast128_Int32),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpblendmd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Int32, MemorySize.Broadcast256_Int32),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpblendmd_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Int32, MemorySize.Broadcast512_Int32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpblendmq_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Int64, MemorySize.Broadcast128_Int64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpblendmq_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Int64, MemorySize.Broadcast256_Int64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpblendmq_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Int64, MemorySize.Broadcast512_Int64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vblendmps_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vblendmps_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vblendmps_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vblendmpd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vblendmpd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vblendmpd_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpblendmb_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int8),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpblendmb_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int8),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpblendmb_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int8)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpblendmw_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int16),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpblendmw_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int16),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpblendmw_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int16)
						)
					),
					invalid,
					invalid
				),
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
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpermi2b_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int8),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpermi2b_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int8),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpermi2b_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int8)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpermi2w_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int16),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpermi2w_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int16),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpermi2w_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int16)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpermi2d_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Int32, MemorySize.Broadcast128_Int32),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpermi2d_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Int32, MemorySize.Broadcast256_Int32),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpermi2d_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Int32, MemorySize.Broadcast512_Int32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpermi2q_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Int64, MemorySize.Broadcast128_Int64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpermi2q_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Int64, MemorySize.Broadcast256_Int64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpermi2q_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Int64, MemorySize.Broadcast512_Int64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpermi2ps_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpermi2ps_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpermi2ps_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpermi2pd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpermi2pd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpermi2pd_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64)
						)
					),
					invalid,
					invalid
				),

				// 78
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkW(Register.XMM0, Register.XMM0, Code.EVEX_Vpbroadcastb_VX_k1z_WX, TupleType.Tuple1_Scalar_1, MemorySize.Int8),
							new OpCodeHandler_EVEX_VkW(Register.YMM0, Register.XMM0, Code.EVEX_Vpbroadcastb_VY_k1z_WX, TupleType.Tuple1_Scalar_1, MemorySize.Int8),
							new OpCodeHandler_EVEX_VkW(Register.ZMM0, Register.XMM0, Code.EVEX_Vpbroadcastb_VZ_k1z_WX, TupleType.Tuple1_Scalar_1, MemorySize.Int8)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkW(Register.XMM0, Register.XMM0, Code.EVEX_Vpbroadcastw_VX_k1z_WX, TupleType.Tuple1_Scalar_2, MemorySize.Int16),
							new OpCodeHandler_EVEX_VkW(Register.YMM0, Register.XMM0, Code.EVEX_Vpbroadcastw_VY_k1z_WX, TupleType.Tuple1_Scalar_2, MemorySize.Int16),
							new OpCodeHandler_EVEX_VkW(Register.ZMM0, Register.XMM0, Code.EVEX_Vpbroadcastw_VZ_k1z_WX, TupleType.Tuple1_Scalar_2, MemorySize.Int16)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkEv_REXW(Register.XMM0, Code.EVEX_Vpbroadcastb_VX_k1z_Rd),
						new OpCodeHandler_EVEX_VkEv_REXW(Register.YMM0, Code.EVEX_Vpbroadcastb_VY_k1z_Rd),
						new OpCodeHandler_EVEX_VkEv_REXW(Register.ZMM0, Code.EVEX_Vpbroadcastb_VZ_k1z_Rd)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkEv_REXW(Register.XMM0, Code.EVEX_Vpbroadcastw_VX_k1z_Rd),
						new OpCodeHandler_EVEX_VkEv_REXW(Register.YMM0, Code.EVEX_Vpbroadcastw_VY_k1z_Rd),
						new OpCodeHandler_EVEX_VkEv_REXW(Register.ZMM0, Code.EVEX_Vpbroadcastw_VZ_k1z_Rd)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkEv_REXW(Register.XMM0, Code.EVEX_Vpbroadcastd_VX_k1z_Rd, Code.EVEX_Vpbroadcastq_VX_k1z_Rq),
						new OpCodeHandler_EVEX_VkEv_REXW(Register.YMM0, Code.EVEX_Vpbroadcastd_VY_k1z_Rd, Code.EVEX_Vpbroadcastq_VY_k1z_Rq),
						new OpCodeHandler_EVEX_VkEv_REXW(Register.ZMM0, Code.EVEX_Vpbroadcastd_VZ_k1z_Rd, Code.EVEX_Vpbroadcastq_VZ_k1z_Rq)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpermt2b_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int8),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpermt2b_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int8),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpermt2b_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int8)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpermt2w_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int16),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpermt2w_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int16),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpermt2w_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int16)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpermt2d_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Int32, MemorySize.Broadcast128_Int32),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpermt2d_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Int32, MemorySize.Broadcast256_Int32),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpermt2d_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Int32, MemorySize.Broadcast512_Int32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpermt2q_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Int64, MemorySize.Broadcast128_Int64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpermt2q_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Int64, MemorySize.Broadcast256_Int64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpermt2q_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Int64, MemorySize.Broadcast512_Int64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpermt2ps_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpermt2ps_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpermt2ps_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpermt2pd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpermt2pd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpermt2pd_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64)
						)
					),
					invalid,
					invalid
				),

				// 80
				invalid,
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpmultishiftqb_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Int64, MemorySize.Broadcast128_Int64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpmultishiftqb_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Int64, MemorySize.Broadcast256_Int64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpmultishiftqb_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Int64, MemorySize.Broadcast512_Int64)
						)
					),
					invalid,
					invalid
				),
				invalid,
				invalid,
				invalid,
				invalid,

				// 88
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkW(Register.XMM0, Code.EVEX_Vexpandps_VX_k1z_WX, TupleType.Tuple1_Scalar, MemorySize.Packed128_Float32),
							new OpCodeHandler_EVEX_VkW(Register.YMM0, Code.EVEX_Vexpandps_VY_k1z_WY, TupleType.Tuple1_Scalar, MemorySize.Packed256_Float32),
							new OpCodeHandler_EVEX_VkW(Register.ZMM0, Code.EVEX_Vexpandps_VZ_k1z_WZ, TupleType.Tuple1_Scalar, MemorySize.Packed512_Float32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkW(Register.XMM0, Code.EVEX_Vexpandpd_VX_k1z_WX, TupleType.Tuple1_Scalar, MemorySize.Packed128_Float64),
							new OpCodeHandler_EVEX_VkW(Register.YMM0, Code.EVEX_Vexpandpd_VY_k1z_WY, TupleType.Tuple1_Scalar, MemorySize.Packed256_Float64),
							new OpCodeHandler_EVEX_VkW(Register.ZMM0, Code.EVEX_Vexpandpd_VZ_k1z_WZ, TupleType.Tuple1_Scalar, MemorySize.Packed512_Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkW(Register.XMM0, Code.EVEX_Vpexpandd_VX_k1z_WX, TupleType.Tuple1_Scalar, MemorySize.Packed128_Int32),
							new OpCodeHandler_EVEX_VkW(Register.YMM0, Code.EVEX_Vpexpandd_VY_k1z_WY, TupleType.Tuple1_Scalar, MemorySize.Packed256_Int32),
							new OpCodeHandler_EVEX_VkW(Register.ZMM0, Code.EVEX_Vpexpandd_VZ_k1z_WZ, TupleType.Tuple1_Scalar, MemorySize.Packed512_Int32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkW(Register.XMM0, Code.EVEX_Vpexpandq_VX_k1z_WX, TupleType.Tuple1_Scalar, MemorySize.Packed128_Int64),
							new OpCodeHandler_EVEX_VkW(Register.YMM0, Code.EVEX_Vpexpandq_VY_k1z_WY, TupleType.Tuple1_Scalar, MemorySize.Packed256_Int64),
							new OpCodeHandler_EVEX_VkW(Register.ZMM0, Code.EVEX_Vpexpandq_VZ_k1z_WZ, TupleType.Tuple1_Scalar, MemorySize.Packed512_Int64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Code.EVEX_Vcompressps_WX_k1z_VX, TupleType.Tuple1_Scalar, MemorySize.Packed128_Float32),
							new OpCodeHandler_EVEX_WkV(Register.YMM0, Code.EVEX_Vcompressps_WY_k1z_VY, TupleType.Tuple1_Scalar, MemorySize.Packed256_Float32),
							new OpCodeHandler_EVEX_WkV(Register.ZMM0, Code.EVEX_Vcompressps_WZ_k1z_VZ, TupleType.Tuple1_Scalar, MemorySize.Packed512_Float32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Code.EVEX_Vcompresspd_WX_k1z_VX, TupleType.Tuple1_Scalar, MemorySize.Packed128_Float64),
							new OpCodeHandler_EVEX_WkV(Register.YMM0, Code.EVEX_Vcompresspd_WY_k1z_VY, TupleType.Tuple1_Scalar, MemorySize.Packed256_Float64),
							new OpCodeHandler_EVEX_WkV(Register.ZMM0, Code.EVEX_Vcompresspd_WZ_k1z_VZ, TupleType.Tuple1_Scalar, MemorySize.Packed512_Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Code.EVEX_Vpcompressd_WX_k1z_VX, TupleType.Tuple1_Scalar, MemorySize.Packed128_Int32),
							new OpCodeHandler_EVEX_WkV(Register.YMM0, Code.EVEX_Vpcompressd_WY_k1z_VY, TupleType.Tuple1_Scalar, MemorySize.Packed256_Int32),
							new OpCodeHandler_EVEX_WkV(Register.ZMM0, Code.EVEX_Vpcompressd_WZ_k1z_VZ, TupleType.Tuple1_Scalar, MemorySize.Packed512_Int32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Code.EVEX_Vpcompressq_WX_k1z_VX, TupleType.Tuple1_Scalar, MemorySize.Packed128_Int64),
							new OpCodeHandler_EVEX_WkV(Register.YMM0, Code.EVEX_Vpcompressq_WY_k1z_VY, TupleType.Tuple1_Scalar, MemorySize.Packed256_Int64),
							new OpCodeHandler_EVEX_WkV(Register.ZMM0, Code.EVEX_Vpcompressq_WZ_k1z_VZ, TupleType.Tuple1_Scalar, MemorySize.Packed512_Int64)
						)
					),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpermb_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int8),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpermb_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int8),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpermb_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int8)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpermw_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int16),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpermw_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int16),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpermw_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int16)
						)
					),
					invalid,
					invalid
				),
				invalid,
				invalid,

				// 90
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_Vk_VSIB(Register.XMM0, Register.XMM0, Code.EVEX_Vpgatherdd_VX_k1_VM32X, TupleType.Tuple1_Scalar, MemorySize.Int32),
							new OpCodeHandler_EVEX_Vk_VSIB(Register.YMM0, Register.YMM0, Code.EVEX_Vpgatherdd_VY_k1_VM32Y, TupleType.Tuple1_Scalar, MemorySize.Int32),
							new OpCodeHandler_EVEX_Vk_VSIB(Register.ZMM0, Register.ZMM0, Code.EVEX_Vpgatherdd_VZ_k1_VM32Z, TupleType.Tuple1_Scalar, MemorySize.Int32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_Vk_VSIB(Register.XMM0, Register.XMM0, Code.EVEX_Vpgatherdq_VX_k1_VM32X, TupleType.Tuple1_Scalar, MemorySize.Int64),
							new OpCodeHandler_EVEX_Vk_VSIB(Register.YMM0, Register.XMM0, Code.EVEX_Vpgatherdq_VY_k1_VM32X, TupleType.Tuple1_Scalar, MemorySize.Int64),
							new OpCodeHandler_EVEX_Vk_VSIB(Register.ZMM0, Register.YMM0, Code.EVEX_Vpgatherdq_VZ_k1_VM32Y, TupleType.Tuple1_Scalar, MemorySize.Int64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_Vk_VSIB(Register.XMM0, Register.XMM0, Code.EVEX_Vpgatherqd_VX_k1_VM64X, TupleType.Tuple1_Scalar, MemorySize.Int32),
							new OpCodeHandler_EVEX_Vk_VSIB(Register.XMM0, Register.YMM0, Code.EVEX_Vpgatherqd_VX_k1_VM64Y, TupleType.Tuple1_Scalar, MemorySize.Int32),
							new OpCodeHandler_EVEX_Vk_VSIB(Register.YMM0, Register.ZMM0, Code.EVEX_Vpgatherqd_VY_k1_VM64Z, TupleType.Tuple1_Scalar, MemorySize.Int32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_Vk_VSIB(Register.XMM0, Register.XMM0, Code.EVEX_Vpgatherqq_VX_k1_VM64X, TupleType.Tuple1_Scalar, MemorySize.Int64),
							new OpCodeHandler_EVEX_Vk_VSIB(Register.YMM0, Register.YMM0, Code.EVEX_Vpgatherqq_VY_k1_VM64Y, TupleType.Tuple1_Scalar, MemorySize.Int64),
							new OpCodeHandler_EVEX_Vk_VSIB(Register.ZMM0, Register.ZMM0, Code.EVEX_Vpgatherqq_VZ_k1_VM64Z, TupleType.Tuple1_Scalar, MemorySize.Int64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_Vk_VSIB(Register.XMM0, Register.XMM0, Code.EVEX_Vgatherdps_VX_k1_VM32X, TupleType.Tuple1_Scalar, MemorySize.Float32),
							new OpCodeHandler_EVEX_Vk_VSIB(Register.YMM0, Register.YMM0, Code.EVEX_Vgatherdps_VY_k1_VM32Y, TupleType.Tuple1_Scalar, MemorySize.Float32),
							new OpCodeHandler_EVEX_Vk_VSIB(Register.ZMM0, Register.ZMM0, Code.EVEX_Vgatherdps_VZ_k1_VM32Z, TupleType.Tuple1_Scalar, MemorySize.Float32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_Vk_VSIB(Register.XMM0, Register.XMM0, Code.EVEX_Vgatherdpd_VX_k1_VM32X, TupleType.Tuple1_Scalar, MemorySize.Float64),
							new OpCodeHandler_EVEX_Vk_VSIB(Register.YMM0, Register.XMM0, Code.EVEX_Vgatherdpd_VY_k1_VM32X, TupleType.Tuple1_Scalar, MemorySize.Float64),
							new OpCodeHandler_EVEX_Vk_VSIB(Register.ZMM0, Register.YMM0, Code.EVEX_Vgatherdpd_VZ_k1_VM32Y, TupleType.Tuple1_Scalar, MemorySize.Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_Vk_VSIB(Register.XMM0, Register.XMM0, Code.EVEX_Vgatherqps_VX_k1_VM64X, TupleType.Tuple1_Scalar, MemorySize.Float32),
							new OpCodeHandler_EVEX_Vk_VSIB(Register.XMM0, Register.YMM0, Code.EVEX_Vgatherqps_VX_k1_VM64Y, TupleType.Tuple1_Scalar, MemorySize.Float32),
							new OpCodeHandler_EVEX_Vk_VSIB(Register.YMM0, Register.ZMM0, Code.EVEX_Vgatherqps_VY_k1_VM64Z, TupleType.Tuple1_Scalar, MemorySize.Float32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_Vk_VSIB(Register.XMM0, Register.XMM0, Code.EVEX_Vgatherqpd_VX_k1_VM64X, TupleType.Tuple1_Scalar, MemorySize.Float64),
							new OpCodeHandler_EVEX_Vk_VSIB(Register.YMM0, Register.YMM0, Code.EVEX_Vgatherqpd_VY_k1_VM64Y, TupleType.Tuple1_Scalar, MemorySize.Float64),
							new OpCodeHandler_EVEX_Vk_VSIB(Register.ZMM0, Register.ZMM0, Code.EVEX_Vgatherqpd_VZ_k1_VM64Z, TupleType.Tuple1_Scalar, MemorySize.Float64)
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
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfmaddsub132ps_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vfmaddsub132ps_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vfmaddsub132ps_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32, onlySAE: false)
						),
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfmaddsub132pd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vfmaddsub132pd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vfmaddsub132pd_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: false)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfmsubadd132ps_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vfmsubadd132ps_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vfmsubadd132ps_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32, onlySAE: false)
						),
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfmsubadd132pd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vfmsubadd132pd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vfmsubadd132pd_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: false)
						)
					),
					invalid,
					invalid
				),

				// 98
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfmadd132ps_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vfmadd132ps_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vfmadd132ps_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32, onlySAE: false)
						),
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfmadd132pd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vfmadd132pd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vfmadd132pd_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: false)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfmadd132ss_VX_k1z_HX_WX_er, TupleType.Tuple1_Scalar, MemorySize.Float32, onlySAE: false),
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfmadd132sd_VX_k1z_HX_WX_er, TupleType.Tuple1_Scalar, MemorySize.Float64, onlySAE: false)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfmsub132ps_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vfmsub132ps_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vfmsub132ps_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32, onlySAE: false)
						),
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfmsub132pd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vfmsub132pd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vfmsub132pd_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: false)
						)
					),
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_VkHM(Register.ZMM0, Code.EVEX_V4fmaddps_VZ_k1z_HZP3_M, TupleType.Tuple1_4X, MemorySize.Packed128_Float32),
						invalid
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfmsub132ss_VX_k1z_HX_WX_er, TupleType.Tuple1_Scalar, MemorySize.Float32, onlySAE: false),
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfmsub132sd_VX_k1z_HX_WX_er, TupleType.Tuple1_Scalar, MemorySize.Float64, onlySAE: false)
					),
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_VkHM(Register.XMM0, Code.EVEX_V4fmaddss_VX_k1z_HXP3_M, TupleType.Tuple1_4X, MemorySize.Packed128_Float32),
						invalid
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfnmadd132ps_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vfnmadd132ps_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vfnmadd132ps_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32, onlySAE: false)
						),
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfnmadd132pd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vfnmadd132pd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vfnmadd132pd_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: false)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfnmadd132ss_VX_k1z_HX_WX_er, TupleType.Tuple1_Scalar, MemorySize.Float32, onlySAE: false),
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfnmadd132sd_VX_k1z_HX_WX_er, TupleType.Tuple1_Scalar, MemorySize.Float64, onlySAE: false)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfnmsub132ps_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vfnmsub132ps_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vfnmsub132ps_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32, onlySAE: false)
						),
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfnmsub132pd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vfnmsub132pd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vfnmsub132pd_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: false)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfnmsub132ss_VX_k1z_HX_WX_er, TupleType.Tuple1_Scalar, MemorySize.Float32, onlySAE: false),
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfnmsub132sd_VX_k1z_HX_WX_er, TupleType.Tuple1_Scalar, MemorySize.Float64, onlySAE: false)
					),
					invalid,
					invalid
				),

				// A0
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VSIB_k1_VX(Register.XMM0, Register.XMM0, Code.EVEX_Vpscatterdd_VM32X_k1_VX, TupleType.Tuple1_Scalar, MemorySize.Int32),
							new OpCodeHandler_EVEX_VSIB_k1_VX(Register.YMM0, Register.YMM0, Code.EVEX_Vpscatterdd_VM32Y_k1_VY, TupleType.Tuple1_Scalar, MemorySize.Int32),
							new OpCodeHandler_EVEX_VSIB_k1_VX(Register.ZMM0, Register.ZMM0, Code.EVEX_Vpscatterdd_VM32Z_k1_VZ, TupleType.Tuple1_Scalar, MemorySize.Int32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VSIB_k1_VX(Register.XMM0, Register.XMM0, Code.EVEX_Vpscatterdq_VM32X_k1_VX, TupleType.Tuple1_Scalar, MemorySize.Int64),
							new OpCodeHandler_EVEX_VSIB_k1_VX(Register.XMM0, Register.YMM0, Code.EVEX_Vpscatterdq_VM32X_k1_VY, TupleType.Tuple1_Scalar, MemorySize.Int64),
							new OpCodeHandler_EVEX_VSIB_k1_VX(Register.YMM0, Register.ZMM0, Code.EVEX_Vpscatterdq_VM32Y_k1_VZ, TupleType.Tuple1_Scalar, MemorySize.Int64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VSIB_k1_VX(Register.XMM0, Register.XMM0, Code.EVEX_Vpscatterqd_VM64X_k1_VX, TupleType.Tuple1_Scalar, MemorySize.Int32),
							new OpCodeHandler_EVEX_VSIB_k1_VX(Register.YMM0, Register.XMM0, Code.EVEX_Vpscatterqd_VM64Y_k1_VX, TupleType.Tuple1_Scalar, MemorySize.Int32),
							new OpCodeHandler_EVEX_VSIB_k1_VX(Register.ZMM0, Register.YMM0, Code.EVEX_Vpscatterqd_VM64Z_k1_VY, TupleType.Tuple1_Scalar, MemorySize.Int32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VSIB_k1_VX(Register.XMM0, Register.XMM0, Code.EVEX_Vpscatterqq_VM64X_k1_VX, TupleType.Tuple1_Scalar, MemorySize.Int64),
							new OpCodeHandler_EVEX_VSIB_k1_VX(Register.YMM0, Register.YMM0, Code.EVEX_Vpscatterqq_VM64Y_k1_VY, TupleType.Tuple1_Scalar, MemorySize.Int64),
							new OpCodeHandler_EVEX_VSIB_k1_VX(Register.ZMM0, Register.ZMM0, Code.EVEX_Vpscatterqq_VM64Z_k1_VZ, TupleType.Tuple1_Scalar, MemorySize.Int64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VSIB_k1_VX(Register.XMM0, Register.XMM0, Code.EVEX_Vscatterdps_VM32X_k1_VX, TupleType.Tuple1_Scalar, MemorySize.Float32),
							new OpCodeHandler_EVEX_VSIB_k1_VX(Register.YMM0, Register.YMM0, Code.EVEX_Vscatterdps_VM32Y_k1_VY, TupleType.Tuple1_Scalar, MemorySize.Float32),
							new OpCodeHandler_EVEX_VSIB_k1_VX(Register.ZMM0, Register.ZMM0, Code.EVEX_Vscatterdps_VM32Z_k1_VZ, TupleType.Tuple1_Scalar, MemorySize.Float32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VSIB_k1_VX(Register.XMM0, Register.XMM0, Code.EVEX_Vscatterdpd_VM32X_k1_VX, TupleType.Tuple1_Scalar, MemorySize.Float64),
							new OpCodeHandler_EVEX_VSIB_k1_VX(Register.XMM0, Register.YMM0, Code.EVEX_Vscatterdpd_VM32X_k1_VY, TupleType.Tuple1_Scalar, MemorySize.Float64),
							new OpCodeHandler_EVEX_VSIB_k1_VX(Register.YMM0, Register.ZMM0, Code.EVEX_Vscatterdpd_VM32Y_k1_VZ, TupleType.Tuple1_Scalar, MemorySize.Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VSIB_k1_VX(Register.XMM0, Register.XMM0, Code.EVEX_Vscatterqps_VM64X_k1_VX, TupleType.Tuple1_Scalar, MemorySize.Float32),
							new OpCodeHandler_EVEX_VSIB_k1_VX(Register.YMM0, Register.XMM0, Code.EVEX_Vscatterqps_VM64Y_k1_VX, TupleType.Tuple1_Scalar, MemorySize.Float32),
							new OpCodeHandler_EVEX_VSIB_k1_VX(Register.ZMM0, Register.YMM0, Code.EVEX_Vscatterqps_VM64Z_k1_VY, TupleType.Tuple1_Scalar, MemorySize.Float32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VSIB_k1_VX(Register.XMM0, Register.XMM0, Code.EVEX_Vscatterqpd_VM64X_k1_VX, TupleType.Tuple1_Scalar, MemorySize.Float64),
							new OpCodeHandler_EVEX_VSIB_k1_VX(Register.YMM0, Register.YMM0, Code.EVEX_Vscatterqpd_VM64Y_k1_VY, TupleType.Tuple1_Scalar, MemorySize.Float64),
							new OpCodeHandler_EVEX_VSIB_k1_VX(Register.ZMM0, Register.ZMM0, Code.EVEX_Vscatterqpd_VM64Z_k1_VZ, TupleType.Tuple1_Scalar, MemorySize.Float64)
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
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfmaddsub213ps_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vfmaddsub213ps_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vfmaddsub213ps_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32, onlySAE: false)
						),
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfmaddsub213pd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vfmaddsub213pd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vfmaddsub213pd_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: false)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfmsubadd213ps_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vfmsubadd213ps_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vfmsubadd213ps_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32, onlySAE: false)
						),
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfmsubadd213pd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vfmsubadd213pd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vfmsubadd213pd_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: false)
						)
					),
					invalid,
					invalid
				),

				// A8
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfmadd213ps_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vfmadd213ps_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vfmadd213ps_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32, onlySAE: false)
						),
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfmadd213pd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vfmadd213pd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vfmadd213pd_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: false)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfmadd213ss_VX_k1z_HX_WX_er, TupleType.Tuple1_Scalar, MemorySize.Float32, onlySAE: false),
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfmadd213sd_VX_k1z_HX_WX_er, TupleType.Tuple1_Scalar, MemorySize.Float64, onlySAE: false)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfmsub213ps_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vfmsub213ps_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vfmsub213ps_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32, onlySAE: false)
						),
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfmsub213pd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vfmsub213pd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vfmsub213pd_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: false)
						)
					),
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_VkHM(Register.ZMM0, Code.EVEX_V4fnmaddps_VZ_k1z_HZP3_M, TupleType.Tuple1_4X, MemorySize.Packed128_Float32),
						invalid
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfmsub213ss_VX_k1z_HX_WX_er, TupleType.Tuple1_Scalar, MemorySize.Float32, onlySAE: false),
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfmsub213sd_VX_k1z_HX_WX_er, TupleType.Tuple1_Scalar, MemorySize.Float64, onlySAE: false)
					),
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_VkHM(Register.XMM0, Code.EVEX_V4fnmaddss_VX_k1z_HXP3_M, TupleType.Tuple1_4X, MemorySize.Packed128_Float32),
						invalid
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfnmadd213ps_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vfnmadd213ps_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vfnmadd213ps_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32, onlySAE: false)
						),
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfnmadd213pd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vfnmadd213pd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vfnmadd213pd_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: false)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfnmadd213ss_VX_k1z_HX_WX_er, TupleType.Tuple1_Scalar, MemorySize.Float32, onlySAE: false),
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfnmadd213sd_VX_k1z_HX_WX_er, TupleType.Tuple1_Scalar, MemorySize.Float64, onlySAE: false)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfnmsub213ps_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vfnmsub213ps_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vfnmsub213ps_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32, onlySAE: false)
						),
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfnmsub213pd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vfnmsub213pd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vfnmsub213pd_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: false)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfnmsub213ss_VX_k1z_HX_WX_er, TupleType.Tuple1_Scalar, MemorySize.Float32, onlySAE: false),
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfnmsub213sd_VX_k1z_HX_WX_er, TupleType.Tuple1_Scalar, MemorySize.Float64, onlySAE: false)
					),
					invalid,
					invalid
				),

				// B0
				invalid,
				invalid,
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpmadd52luq_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_UInt52, MemorySize.Broadcast128_UInt52),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpmadd52luq_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_UInt52, MemorySize.Broadcast256_UInt52),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpmadd52luq_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_UInt52, MemorySize.Broadcast512_UInt52)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpmadd52huq_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_UInt52, MemorySize.Broadcast128_UInt52),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpmadd52huq_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_UInt52, MemorySize.Broadcast256_UInt52),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpmadd52huq_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_UInt52, MemorySize.Broadcast512_UInt52)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfmaddsub231ps_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vfmaddsub231ps_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vfmaddsub231ps_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32, onlySAE: false)
						),
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfmaddsub231pd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vfmaddsub231pd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vfmaddsub231pd_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: false)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfmsubadd231ps_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vfmsubadd231ps_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vfmsubadd231ps_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32, onlySAE: false)
						),
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfmsubadd231pd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vfmsubadd231pd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vfmsubadd231pd_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: false)
						)
					),
					invalid,
					invalid
				),

				// B8
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfmadd231ps_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vfmadd231ps_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vfmadd231ps_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32, onlySAE: false)
						),
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfmadd231pd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vfmadd231pd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vfmadd231pd_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: false)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfmadd231ss_VX_k1z_HX_WX_er, TupleType.Tuple1_Scalar, MemorySize.Float32, onlySAE: false),
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfmadd231sd_VX_k1z_HX_WX_er, TupleType.Tuple1_Scalar, MemorySize.Float64, onlySAE: false)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfmsub231ps_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vfmsub231ps_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vfmsub231ps_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32, onlySAE: false)
						),
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfmsub231pd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vfmsub231pd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vfmsub231pd_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: false)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfmsub231ss_VX_k1z_HX_WX_er, TupleType.Tuple1_Scalar, MemorySize.Float32, onlySAE: false),
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfmsub231sd_VX_k1z_HX_WX_er, TupleType.Tuple1_Scalar, MemorySize.Float64, onlySAE: false)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfnmadd231ps_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vfnmadd231ps_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vfnmadd231ps_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32, onlySAE: false)
						),
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfnmadd231pd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vfnmadd231pd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vfnmadd231pd_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: false)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfnmadd231ss_VX_k1z_HX_WX_er, TupleType.Tuple1_Scalar, MemorySize.Float32, onlySAE: false),
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfnmadd231sd_VX_k1z_HX_WX_er, TupleType.Tuple1_Scalar, MemorySize.Float64, onlySAE: false)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfnmsub231ps_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vfnmsub231ps_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vfnmsub231ps_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32, onlySAE: false)
						),
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfnmsub231pd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vfnmsub231pd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vfnmsub231pd_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: false)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfnmsub231ss_VX_k1z_HX_WX_er, TupleType.Tuple1_Scalar, MemorySize.Float32, onlySAE: false),
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vfnmsub231sd_VX_k1z_HX_WX_er, TupleType.Tuple1_Scalar, MemorySize.Float64, onlySAE: false)
					),
					invalid,
					invalid
				),

				// C0
				invalid,
				invalid,
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkW(Register.XMM0, Code.EVEX_Vpconflictd_VX_k1z_WX_b, TupleType.Full_128, MemorySize.Packed128_Int32, MemorySize.Broadcast128_Int32),
							new OpCodeHandler_EVEX_VkW(Register.YMM0, Code.EVEX_Vpconflictd_VY_k1z_WY_b, TupleType.Full_256, MemorySize.Packed256_Int32, MemorySize.Broadcast256_Int32),
							new OpCodeHandler_EVEX_VkW(Register.ZMM0, Code.EVEX_Vpconflictd_VZ_k1z_WZ_b, TupleType.Full_512, MemorySize.Packed512_Int32, MemorySize.Broadcast512_Int32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkW(Register.XMM0, Code.EVEX_Vpconflictq_VX_k1z_WX_b, TupleType.Full_128, MemorySize.Packed128_Int64, MemorySize.Broadcast128_Int64),
							new OpCodeHandler_EVEX_VkW(Register.YMM0, Code.EVEX_Vpconflictq_VY_k1z_WY_b, TupleType.Full_256, MemorySize.Packed256_Int64, MemorySize.Broadcast256_Int64),
							new OpCodeHandler_EVEX_VkW(Register.ZMM0, Code.EVEX_Vpconflictq_VZ_k1z_WZ_b, TupleType.Full_512, MemorySize.Packed512_Int64, MemorySize.Broadcast512_Int64)
						)
					),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_Group(handlers_Grp_0F38C6),
				new OpCodeHandler_Group(handlers_Grp_0F38C7),

				// C8
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							invalid,
							invalid,
							new OpCodeHandler_EVEX_VkW_er(Register.ZMM0, Code.EVEX_Vexp2ps_VZ_k1z_WZ_sae_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32, onlySAE: true)
						),
						new OpCodeHandler_VectorLength_EVEX_er(
							invalid,
							invalid,
							new OpCodeHandler_EVEX_VkW_er(Register.ZMM0, Code.EVEX_Vexp2pd_VZ_k1z_WZ_sae_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: true)
						)
					),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							invalid,
							invalid,
							new OpCodeHandler_EVEX_VkW_er(Register.ZMM0, Code.EVEX_Vrcp28ps_VZ_k1z_WZ_sae_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32, onlySAE: true)
						),
						new OpCodeHandler_VectorLength_EVEX_er(
							invalid,
							invalid,
							new OpCodeHandler_EVEX_VkW_er(Register.ZMM0, Code.EVEX_Vrcp28pd_VZ_k1z_WZ_sae_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: true)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vrcp28ss_VX_k1z_HX_WX_sae, TupleType.Tuple1_Scalar, MemorySize.Float32, onlySAE: true),
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vrcp28sd_VX_k1z_HX_WX_sae, TupleType.Tuple1_Scalar, MemorySize.Float64, onlySAE: true)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							invalid,
							invalid,
							new OpCodeHandler_EVEX_VkW_er(Register.ZMM0, Code.EVEX_Vrsqrt28ps_VZ_k1z_WZ_sae_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32, onlySAE: true)
						),
						new OpCodeHandler_VectorLength_EVEX_er(
							invalid,
							invalid,
							new OpCodeHandler_EVEX_VkW_er(Register.ZMM0, Code.EVEX_Vrsqrt28pd_VZ_k1z_WZ_sae_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: true)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vrsqrt28ss_VX_k1z_HX_WX_sae, TupleType.Tuple1_Scalar, MemorySize.Float32, onlySAE: true),
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vrsqrt28sd_VX_k1z_HX_WX_sae, TupleType.Tuple1_Scalar, MemorySize.Float64, onlySAE: true)
					),
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
				invalid,
				invalid,
				invalid,
				invalid,
				invalid,

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

			ThreeByteHandlers_0F3AXX = new OpCodeHandler[0x100] {
				// 00
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							new OpCodeHandler_EVEX_VkWIb(Register.YMM0, Code.EVEX_Vpermq_VY_k1z_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_Int64, MemorySize.Broadcast256_Int64),
							new OpCodeHandler_EVEX_VkWIb(Register.ZMM0, Code.EVEX_Vpermq_VZ_k1z_WZ_Ib_b, TupleType.Full_512, MemorySize.Packed512_Int64, MemorySize.Broadcast512_Int64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							new OpCodeHandler_EVEX_VkWIb(Register.YMM0, Code.EVEX_Vpermpd_VY_k1z_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64),
							new OpCodeHandler_EVEX_VkWIb(Register.ZMM0, Code.EVEX_Vpermpd_VZ_k1z_WZ_Ib_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64)
						)
					),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHWIb(Register.XMM0, Code.EVEX_Valignd_VX_k1z_HX_WX_Ib_b, TupleType.Full_128, MemorySize.Packed128_Int32, MemorySize.Broadcast128_Int32),
							new OpCodeHandler_EVEX_VkHWIb(Register.YMM0, Code.EVEX_Valignd_VY_k1z_HY_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_Int32, MemorySize.Broadcast256_Int32),
							new OpCodeHandler_EVEX_VkHWIb(Register.ZMM0, Code.EVEX_Valignd_VZ_k1z_HZ_WZ_Ib_b, TupleType.Full_512, MemorySize.Packed512_Int32, MemorySize.Broadcast512_Int32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHWIb(Register.XMM0, Code.EVEX_Valignq_VX_k1z_HX_WX_Ib_b, TupleType.Full_128, MemorySize.Packed128_Int64, MemorySize.Broadcast128_Int64),
							new OpCodeHandler_EVEX_VkHWIb(Register.YMM0, Code.EVEX_Valignq_VY_k1z_HY_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_Int64, MemorySize.Broadcast256_Int64),
							new OpCodeHandler_EVEX_VkHWIb(Register.ZMM0, Code.EVEX_Valignq_VZ_k1z_HZ_WZ_Ib_b, TupleType.Full_512, MemorySize.Packed512_Int64, MemorySize.Broadcast512_Int64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkWIb(Register.XMM0, Code.EVEX_Vpermilps_VX_k1z_WX_Ib_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32),
							new OpCodeHandler_EVEX_VkWIb(Register.YMM0, Code.EVEX_Vpermilps_VY_k1z_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32),
							new OpCodeHandler_EVEX_VkWIb(Register.ZMM0, Code.EVEX_Vpermilps_VZ_k1z_WZ_Ib_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkWIb(Register.XMM0, Code.EVEX_Vpermilpd_VX_k1z_WX_Ib_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64),
							new OpCodeHandler_EVEX_VkWIb(Register.YMM0, Code.EVEX_Vpermilpd_VY_k1z_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64),
							new OpCodeHandler_EVEX_VkWIb(Register.ZMM0, Code.EVEX_Vpermilpd_VZ_k1z_WZ_Ib_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64)
						)
					),
					invalid,
					invalid
				),
				invalid,
				invalid,

				// 08
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkWIb_er(Register.XMM0, Code.EVEX_Vrndscaleps_VX_k1z_WX_Ib_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: true),
							new OpCodeHandler_EVEX_VkWIb_er(Register.YMM0, Code.EVEX_Vrndscaleps_VY_k1z_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: true),
							new OpCodeHandler_EVEX_VkWIb_er(Register.ZMM0, Code.EVEX_Vrndscaleps_VZ_k1z_WZ_Ib_sae_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32, onlySAE: true)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkWIb_er(Register.XMM0, Code.EVEX_Vrndscalepd_VX_k1z_WX_Ib_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: true),
							new OpCodeHandler_EVEX_VkWIb_er(Register.YMM0, Code.EVEX_Vrndscalepd_VY_k1z_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: true),
							new OpCodeHandler_EVEX_VkWIb_er(Register.ZMM0, Code.EVEX_Vrndscalepd_VZ_k1z_WZ_Ib_sae_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: true)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_VkHWIb_er(Register.XMM0, Code.EVEX_Vrndscaless_VX_k1z_HX_WX_Ib_sae, TupleType.Tuple1_Scalar, MemorySize.Float32, onlySAE: true),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_EVEX_VkHWIb_er(Register.XMM0, Code.EVEX_Vrndscalesd_VX_k1z_HX_WX_Ib_sae, TupleType.Tuple1_Scalar, MemorySize.Float64, onlySAE: true)
					),
					invalid,
					invalid
				),
				invalid,
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkHWIb(Register.XMM0, Code.EVEX_Vpalignr_VX_k1z_HX_WX_Ib, TupleType.Full_Mem_128, MemorySize.Packed128_Int8),
						new OpCodeHandler_EVEX_VkHWIb(Register.YMM0, Code.EVEX_Vpalignr_VY_k1z_HY_WY_Ib, TupleType.Full_Mem_256, MemorySize.Packed256_Int8),
						new OpCodeHandler_EVEX_VkHWIb(Register.ZMM0, Code.EVEX_Vpalignr_VZ_k1z_HZ_WZ_Ib, TupleType.Full_Mem_512, MemorySize.Packed512_Int8)
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
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_GvM_VX_Ib(Register.XMM0, Code.EVEX_Vpextrb_RdMb_VX_Ib, Code.EVEX_Vpextrb_RqMb_VX_Ib, TupleType.Tuple1_Scalar_1, TupleType.Tuple1_Scalar_1, MemorySize.UInt8, MemorySize.UInt8),
						invalid,
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_GvM_VX_Ib(Register.XMM0, Code.EVEX_Vpextrw_RdMw_VX_Ib, Code.EVEX_Vpextrw_RqMw_VX_Ib, TupleType.Tuple1_Scalar_2, TupleType.Tuple1_Scalar_2, MemorySize.UInt16, MemorySize.UInt16),
						invalid,
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_GvM_VX_Ib(Register.XMM0, Code.EVEX_Vpextrd_Ed_VX_Ib, Code.EVEX_Vpextrq_Eq_VX_Ib, TupleType.Tuple1_Scalar_4, TupleType.Tuple1_Scalar_8, MemorySize.UInt32, MemorySize.UInt64),
						invalid,
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_Ed_V_Ib(Register.XMM0, Code.EVEX_Vextractps_Ed_VX_Ib, Code.EVEX_Vextractps_Eq_VX_Ib, TupleType.Tuple1_Scalar_4, TupleType.Tuple1_Scalar_4, MemorySize.Float32, MemorySize.Float32),
						invalid,
						invalid
					),
					invalid,
					invalid
				),

				// 18
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							new OpCodeHandler_EVEX_VkHWIb(Register.YMM0, Register.YMM0, Register.XMM0, Code.EVEX_Vinsertf32x4_VY_k1z_HY_WX_Ib, TupleType.Tuple4, MemorySize.Packed128_Float32),
							new OpCodeHandler_EVEX_VkHWIb(Register.ZMM0, Register.ZMM0, Register.XMM0, Code.EVEX_Vinsertf32x4_VZ_k1z_HZ_WX_Ib, TupleType.Tuple4, MemorySize.Packed128_Float32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							new OpCodeHandler_EVEX_VkHWIb(Register.YMM0, Register.YMM0, Register.XMM0, Code.EVEX_Vinsertf64x2_VY_k1z_HY_WX_Ib, TupleType.Tuple2, MemorySize.Packed128_Float64),
							new OpCodeHandler_EVEX_VkHWIb(Register.ZMM0, Register.ZMM0, Register.XMM0, Code.EVEX_Vinsertf64x2_VZ_k1z_HZ_WX_Ib, TupleType.Tuple2, MemorySize.Packed128_Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							new OpCodeHandler_EVEX_WkVIb(Register.XMM0, Register.YMM0, Code.EVEX_Vextractf32x4_WX_k1z_VY_Ib, TupleType.Tuple4, MemorySize.Packed128_Float32),
							new OpCodeHandler_EVEX_WkVIb(Register.XMM0, Register.ZMM0, Code.EVEX_Vextractf32x4_WX_k1z_VZ_Ib, TupleType.Tuple4, MemorySize.Packed128_Float32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							new OpCodeHandler_EVEX_WkVIb(Register.XMM0, Register.YMM0, Code.EVEX_Vextractf64x2_WX_k1z_VY_Ib, TupleType.Tuple2, MemorySize.Packed128_Float64),
							new OpCodeHandler_EVEX_WkVIb(Register.XMM0, Register.ZMM0, Code.EVEX_Vextractf64x2_WX_k1z_VZ_Ib, TupleType.Tuple2, MemorySize.Packed128_Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							invalid,
							new OpCodeHandler_EVEX_VkHWIb(Register.ZMM0, Register.ZMM0, Register.YMM0, Code.EVEX_Vinsertf32x8_VZ_k1z_HZ_WY_Ib, TupleType.Tuple8, MemorySize.Packed256_Float32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							invalid,
							new OpCodeHandler_EVEX_VkHWIb(Register.ZMM0, Register.ZMM0, Register.YMM0, Code.EVEX_Vinsertf64x4_VZ_k1z_HZ_WY_Ib, TupleType.Tuple4, MemorySize.Packed256_Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							invalid,
							new OpCodeHandler_EVEX_WkVIb(Register.YMM0, Register.ZMM0, Code.EVEX_Vextractf32x8_WY_k1z_VZ_Ib, TupleType.Tuple8, MemorySize.Packed256_Float32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							invalid,
							new OpCodeHandler_EVEX_WkVIb(Register.YMM0, Register.ZMM0, Code.EVEX_Vextractf64x4_WY_k1z_VZ_Ib, TupleType.Tuple4, MemorySize.Packed256_Float64)
						)
					),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_WkVIb_er(Register.XMM0, Register.XMM0, Code.EVEX_Vcvtps2ph_WX_k1z_VX_Ib, TupleType.Half_Mem_128, MemorySize.Packed64_Float16, onlySAE: true),
							new OpCodeHandler_EVEX_WkVIb_er(Register.XMM0, Register.YMM0, Code.EVEX_Vcvtps2ph_WX_k1z_VY_Ib, TupleType.Half_Mem_256, MemorySize.Packed128_Float16, onlySAE: true),
							new OpCodeHandler_EVEX_WkVIb_er(Register.YMM0, Register.ZMM0, Code.EVEX_Vcvtps2ph_WY_k1z_VZ_Ib_sae, TupleType.Half_Mem_512, MemorySize.Packed256_Float16, onlySAE: true)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_KkHWIb(Register.XMM0, Code.EVEX_Vpcmpud_VK_k1_HX_WX_Ib_b, TupleType.Full_128, MemorySize.Packed128_UInt32, MemorySize.Broadcast128_UInt32),
							new OpCodeHandler_EVEX_KkHWIb(Register.YMM0, Code.EVEX_Vpcmpud_VK_k1_HY_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_UInt32, MemorySize.Broadcast256_UInt32),
							new OpCodeHandler_EVEX_KkHWIb(Register.ZMM0, Code.EVEX_Vpcmpud_VK_k1_HZ_WZ_Ib_b, TupleType.Full_512, MemorySize.Packed512_UInt32, MemorySize.Broadcast512_UInt32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_KkHWIb(Register.XMM0, Code.EVEX_Vpcmpuq_VK_k1_HX_WX_Ib_b, TupleType.Full_128, MemorySize.Packed128_UInt64, MemorySize.Broadcast128_UInt64),
							new OpCodeHandler_EVEX_KkHWIb(Register.YMM0, Code.EVEX_Vpcmpuq_VK_k1_HY_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_UInt64, MemorySize.Broadcast256_UInt64),
							new OpCodeHandler_EVEX_KkHWIb(Register.ZMM0, Code.EVEX_Vpcmpuq_VK_k1_HZ_WZ_Ib_b, TupleType.Full_512, MemorySize.Packed512_UInt64, MemorySize.Broadcast512_UInt64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_KkHWIb(Register.XMM0, Code.EVEX_Vpcmpd_VK_k1_HX_WX_Ib_b, TupleType.Full_128, MemorySize.Packed128_Int32, MemorySize.Broadcast128_Int32),
							new OpCodeHandler_EVEX_KkHWIb(Register.YMM0, Code.EVEX_Vpcmpd_VK_k1_HY_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_Int32, MemorySize.Broadcast256_Int32),
							new OpCodeHandler_EVEX_KkHWIb(Register.ZMM0, Code.EVEX_Vpcmpd_VK_k1_HZ_WZ_Ib_b, TupleType.Full_512, MemorySize.Packed512_Int32, MemorySize.Broadcast512_Int32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_KkHWIb(Register.XMM0, Code.EVEX_Vpcmpq_VK_k1_HX_WX_Ib_b, TupleType.Full_128, MemorySize.Packed128_Int64, MemorySize.Broadcast128_Int64),
							new OpCodeHandler_EVEX_KkHWIb(Register.YMM0, Code.EVEX_Vpcmpq_VK_k1_HY_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_Int64, MemorySize.Broadcast256_Int64),
							new OpCodeHandler_EVEX_KkHWIb(Register.ZMM0, Code.EVEX_Vpcmpq_VK_k1_HZ_WZ_Ib_b, TupleType.Full_512, MemorySize.Packed512_Int64, MemorySize.Broadcast512_Int64)
						)
					),
					invalid,
					invalid
				),

				// 20
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_V_H_Ev_Ib(Register.XMM0, Code.EVEX_Vpinsrb_VX_HX_RdMb_Ib, Code.EVEX_Vpinsrb_VX_HX_RqMb_Ib, TupleType.Tuple1_Scalar_1, TupleType.Tuple1_Scalar_1, MemorySize.UInt8, MemorySize.UInt8),
						invalid,
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VHWIb(Register.XMM0, Code.EVEX_Vinsertps_VX_HX_WX_Ib, TupleType.Tuple1_Scalar, MemorySize.Float32),
							invalid,
							invalid
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_V_H_Ev_Ib(Register.XMM0, Code.EVEX_Vpinsrd_VX_HX_Ed_Ib, Code.EVEX_Vpinsrq_VX_HX_Eq_Ib, TupleType.Tuple1_Scalar_4, TupleType.Tuple1_Scalar_8, MemorySize.UInt32, MemorySize.UInt64),
						invalid,
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							new OpCodeHandler_EVEX_VkHWIb(Register.YMM0, Code.EVEX_Vshuff32x4_VY_k1z_HY_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32),
							new OpCodeHandler_EVEX_VkHWIb(Register.ZMM0, Code.EVEX_Vshuff32x4_VZ_k1z_HZ_WZ_Ib_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							new OpCodeHandler_EVEX_VkHWIb(Register.YMM0, Code.EVEX_Vshuff64x2_VY_k1z_HY_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64),
							new OpCodeHandler_EVEX_VkHWIb(Register.ZMM0, Code.EVEX_Vshuff64x2_VZ_k1z_HZ_WZ_Ib_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64)
						)
					),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHWIb(Register.XMM0, Code.EVEX_Vpternlogd_VX_k1z_HX_WX_Ib_b, TupleType.Full_128, MemorySize.Packed128_Int32, MemorySize.Broadcast128_Int32),
							new OpCodeHandler_EVEX_VkHWIb(Register.YMM0, Code.EVEX_Vpternlogd_VY_k1z_HY_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_Int32, MemorySize.Broadcast256_Int32),
							new OpCodeHandler_EVEX_VkHWIb(Register.ZMM0, Code.EVEX_Vpternlogd_VZ_k1z_HZ_WZ_Ib_b, TupleType.Full_512, MemorySize.Packed512_Int32, MemorySize.Broadcast512_Int32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHWIb(Register.XMM0, Code.EVEX_Vpternlogq_VX_k1z_HX_WX_Ib_b, TupleType.Full_128, MemorySize.Packed128_Int64, MemorySize.Broadcast128_Int64),
							new OpCodeHandler_EVEX_VkHWIb(Register.YMM0, Code.EVEX_Vpternlogq_VY_k1z_HY_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_Int64, MemorySize.Broadcast256_Int64),
							new OpCodeHandler_EVEX_VkHWIb(Register.ZMM0, Code.EVEX_Vpternlogq_VZ_k1z_HZ_WZ_Ib_b, TupleType.Full_512, MemorySize.Packed512_Int64, MemorySize.Broadcast512_Int64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkWIb_er(Register.XMM0, Code.EVEX_Vgetmantps_VX_k1z_WX_Ib_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: true),
							new OpCodeHandler_EVEX_VkWIb_er(Register.YMM0, Code.EVEX_Vgetmantps_VY_k1z_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: true),
							new OpCodeHandler_EVEX_VkWIb_er(Register.ZMM0, Code.EVEX_Vgetmantps_VZ_k1z_WZ_Ib_sae_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32, onlySAE: true)
						),
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkWIb_er(Register.XMM0, Code.EVEX_Vgetmantpd_VX_k1z_WX_Ib_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: true),
							new OpCodeHandler_EVEX_VkWIb_er(Register.YMM0, Code.EVEX_Vgetmantpd_VY_k1z_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: true),
							new OpCodeHandler_EVEX_VkWIb_er(Register.ZMM0, Code.EVEX_Vgetmantpd_VZ_k1z_WZ_Ib_sae_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: true)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_VkHWIb_er(Register.XMM0, Code.EVEX_Vgetmantss_VX_k1z_HX_WX_Ib_sae, TupleType.Tuple1_Scalar, MemorySize.Float32, onlySAE: true),
						new OpCodeHandler_EVEX_VkHWIb_er(Register.XMM0, Code.EVEX_Vgetmantsd_VX_k1z_HX_WX_Ib_sae, TupleType.Tuple1_Scalar, MemorySize.Float64, onlySAE: true)
					),
					invalid,
					invalid
				),

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
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							new OpCodeHandler_EVEX_VkHWIb(Register.YMM0, Register.YMM0, Register.XMM0, Code.EVEX_Vinserti32x4_VY_k1z_HY_WX_Ib, TupleType.Tuple4, MemorySize.Packed128_Int32),
							new OpCodeHandler_EVEX_VkHWIb(Register.ZMM0, Register.ZMM0, Register.XMM0, Code.EVEX_Vinserti32x4_VZ_k1z_HZ_WX_Ib, TupleType.Tuple4, MemorySize.Packed128_Int32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							new OpCodeHandler_EVEX_VkHWIb(Register.YMM0, Register.YMM0, Register.XMM0, Code.EVEX_Vinserti64x2_VY_k1z_HY_WX_Ib, TupleType.Tuple2, MemorySize.Packed128_Int64),
							new OpCodeHandler_EVEX_VkHWIb(Register.ZMM0, Register.ZMM0, Register.XMM0, Code.EVEX_Vinserti64x2_VZ_k1z_HZ_WX_Ib, TupleType.Tuple2, MemorySize.Packed128_Int64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							new OpCodeHandler_EVEX_WkVIb(Register.XMM0, Register.YMM0, Code.EVEX_Vextracti32x4_WX_k1z_VY_Ib, TupleType.Tuple4, MemorySize.Packed128_Int32),
							new OpCodeHandler_EVEX_WkVIb(Register.XMM0, Register.ZMM0, Code.EVEX_Vextracti32x4_WX_k1z_VZ_Ib, TupleType.Tuple4, MemorySize.Packed128_Int32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							new OpCodeHandler_EVEX_WkVIb(Register.XMM0, Register.YMM0, Code.EVEX_Vextracti64x2_WX_k1z_VY_Ib, TupleType.Tuple2, MemorySize.Packed128_Int64),
							new OpCodeHandler_EVEX_WkVIb(Register.XMM0, Register.ZMM0, Code.EVEX_Vextracti64x2_WX_k1z_VZ_Ib, TupleType.Tuple2, MemorySize.Packed128_Int64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							invalid,
							new OpCodeHandler_EVEX_VkHWIb(Register.ZMM0, Register.ZMM0, Register.YMM0, Code.EVEX_Vinserti32x8_VZ_k1z_HZ_WY_Ib, TupleType.Tuple8, MemorySize.Packed256_Int32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							invalid,
							new OpCodeHandler_EVEX_VkHWIb(Register.ZMM0, Register.ZMM0, Register.YMM0, Code.EVEX_Vinserti64x4_VZ_k1z_HZ_WY_Ib, TupleType.Tuple4, MemorySize.Packed256_Int64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							invalid,
							new OpCodeHandler_EVEX_WkVIb(Register.YMM0, Register.ZMM0, Code.EVEX_Vextracti32x8_WY_k1z_VZ_Ib, TupleType.Tuple8, MemorySize.Packed256_Int32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							invalid,
							new OpCodeHandler_EVEX_WkVIb(Register.YMM0, Register.ZMM0, Code.EVEX_Vextracti64x4_WY_k1z_VZ_Ib, TupleType.Tuple4, MemorySize.Packed256_Int64)
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
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_KkHWIb(Register.XMM0, Code.EVEX_Vpcmpub_VK_k1_HX_WX_Ib, TupleType.Full_Mem_128, MemorySize.Packed128_UInt8),
							new OpCodeHandler_EVEX_KkHWIb(Register.YMM0, Code.EVEX_Vpcmpub_VK_k1_HY_WY_Ib, TupleType.Full_Mem_256, MemorySize.Packed256_UInt8),
							new OpCodeHandler_EVEX_KkHWIb(Register.ZMM0, Code.EVEX_Vpcmpub_VK_k1_HZ_WZ_Ib, TupleType.Full_Mem_512, MemorySize.Packed512_UInt8)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_KkHWIb(Register.XMM0, Code.EVEX_Vpcmpuw_VK_k1_HX_WX_Ib, TupleType.Full_Mem_128, MemorySize.Packed128_UInt16),
							new OpCodeHandler_EVEX_KkHWIb(Register.YMM0, Code.EVEX_Vpcmpuw_VK_k1_HY_WY_Ib, TupleType.Full_Mem_256, MemorySize.Packed256_UInt16),
							new OpCodeHandler_EVEX_KkHWIb(Register.ZMM0, Code.EVEX_Vpcmpuw_VK_k1_HZ_WZ_Ib, TupleType.Full_Mem_512, MemorySize.Packed512_UInt16)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_KkHWIb(Register.XMM0, Code.EVEX_Vpcmpb_VK_k1_HX_WX_Ib, TupleType.Full_Mem_128, MemorySize.Packed128_Int8),
							new OpCodeHandler_EVEX_KkHWIb(Register.YMM0, Code.EVEX_Vpcmpb_VK_k1_HY_WY_Ib, TupleType.Full_Mem_256, MemorySize.Packed256_Int8),
							new OpCodeHandler_EVEX_KkHWIb(Register.ZMM0, Code.EVEX_Vpcmpb_VK_k1_HZ_WZ_Ib, TupleType.Full_Mem_512, MemorySize.Packed512_Int8)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_KkHWIb(Register.XMM0, Code.EVEX_Vpcmpw_VK_k1_HX_WX_Ib, TupleType.Full_Mem_128, MemorySize.Packed128_Int16),
							new OpCodeHandler_EVEX_KkHWIb(Register.YMM0, Code.EVEX_Vpcmpw_VK_k1_HY_WY_Ib, TupleType.Full_Mem_256, MemorySize.Packed256_Int16),
							new OpCodeHandler_EVEX_KkHWIb(Register.ZMM0, Code.EVEX_Vpcmpw_VK_k1_HZ_WZ_Ib, TupleType.Full_Mem_512, MemorySize.Packed512_Int16)
						)
					),
					invalid,
					invalid
				),

				// 40
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHWIb(Register.XMM0, Code.EVEX_Vdbpsadbw_VX_k1z_HX_WX_Ib, TupleType.Full_Mem_128, MemorySize.Packed128_UInt8),
							new OpCodeHandler_EVEX_VkHWIb(Register.YMM0, Code.EVEX_Vdbpsadbw_VY_k1z_HY_WY_Ib, TupleType.Full_Mem_256, MemorySize.Packed256_UInt8),
							new OpCodeHandler_EVEX_VkHWIb(Register.ZMM0, Code.EVEX_Vdbpsadbw_VZ_k1z_HZ_WZ_Ib, TupleType.Full_Mem_512, MemorySize.Packed512_UInt8)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							new OpCodeHandler_EVEX_VkHWIb(Register.YMM0, Code.EVEX_Vshufi32x4_VY_k1z_HY_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_Int32, MemorySize.Broadcast256_Int32),
							new OpCodeHandler_EVEX_VkHWIb(Register.ZMM0, Code.EVEX_Vshufi32x4_VZ_k1z_HZ_WZ_Ib_b, TupleType.Full_512, MemorySize.Packed512_Int32, MemorySize.Broadcast512_Int32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							invalid,
							new OpCodeHandler_EVEX_VkHWIb(Register.YMM0, Code.EVEX_Vshufi64x2_VY_k1z_HY_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_Int64, MemorySize.Broadcast256_Int64),
							new OpCodeHandler_EVEX_VkHWIb(Register.ZMM0, Code.EVEX_Vshufi64x2_VZ_k1z_HZ_WZ_Ib_b, TupleType.Full_512, MemorySize.Packed512_Int64, MemorySize.Broadcast512_Int64)
						)
					),
					invalid,
					invalid
				),
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
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHWIb_er(Register.XMM0, Code.EVEX_Vrangeps_VX_k1z_HX_WX_Ib_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: true),
							new OpCodeHandler_EVEX_VkHWIb_er(Register.YMM0, Code.EVEX_Vrangeps_VY_k1z_HY_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: true),
							new OpCodeHandler_EVEX_VkHWIb_er(Register.ZMM0, Code.EVEX_Vrangeps_VZ_k1z_HZ_WZ_Ib_sae_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32, onlySAE: true)
						),
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHWIb_er(Register.XMM0, Code.EVEX_Vrangepd_VX_k1z_HX_WX_Ib_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: true),
							new OpCodeHandler_EVEX_VkHWIb_er(Register.YMM0, Code.EVEX_Vrangepd_VY_k1z_HY_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: true),
							new OpCodeHandler_EVEX_VkHWIb_er(Register.ZMM0, Code.EVEX_Vrangepd_VZ_k1z_HZ_WZ_Ib_sae_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: true)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_VkHWIb_er(Register.XMM0, Code.EVEX_Vrangess_VX_k1z_HX_WX_Ib_sae, TupleType.Tuple1_Scalar, MemorySize.Float32, onlySAE: true),
						new OpCodeHandler_EVEX_VkHWIb_er(Register.XMM0, Code.EVEX_Vrangesd_VX_k1z_HX_WX_Ib_sae, TupleType.Tuple1_Scalar, MemorySize.Float64, onlySAE: true)
					),
					invalid,
					invalid
				),
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHWIb_er(Register.XMM0, Code.EVEX_Vfixupimmps_VX_k1z_HX_WX_Ib_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: true),
							new OpCodeHandler_EVEX_VkHWIb_er(Register.YMM0, Code.EVEX_Vfixupimmps_VY_k1z_HY_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: true),
							new OpCodeHandler_EVEX_VkHWIb_er(Register.ZMM0, Code.EVEX_Vfixupimmps_VZ_k1z_HZ_WZ_Ib_sae_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32, onlySAE: true)
						),
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHWIb_er(Register.XMM0, Code.EVEX_Vfixupimmpd_VX_k1z_HX_WX_Ib_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: true),
							new OpCodeHandler_EVEX_VkHWIb_er(Register.YMM0, Code.EVEX_Vfixupimmpd_VY_k1z_HY_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: true),
							new OpCodeHandler_EVEX_VkHWIb_er(Register.ZMM0, Code.EVEX_Vfixupimmpd_VZ_k1z_HZ_WZ_Ib_sae_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: true)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_VkHWIb_er(Register.XMM0, Code.EVEX_Vfixupimmss_VX_k1z_HX_WX_Ib_sae, TupleType.Tuple1_Scalar, MemorySize.Float32, onlySAE: true),
						new OpCodeHandler_EVEX_VkHWIb_er(Register.XMM0, Code.EVEX_Vfixupimmsd_VX_k1z_HX_WX_Ib_sae, TupleType.Tuple1_Scalar, MemorySize.Float64, onlySAE: true)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkWIb_er(Register.XMM0, Code.EVEX_Vreduceps_VX_k1z_WX_Ib_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: true),
							new OpCodeHandler_EVEX_VkWIb_er(Register.YMM0, Code.EVEX_Vreduceps_VY_k1z_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: true),
							new OpCodeHandler_EVEX_VkWIb_er(Register.ZMM0, Code.EVEX_Vreduceps_VZ_k1z_WZ_Ib_sae_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32, onlySAE: true)
						),
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkWIb_er(Register.XMM0, Code.EVEX_Vreducepd_VX_k1z_WX_Ib_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: true),
							new OpCodeHandler_EVEX_VkWIb_er(Register.YMM0, Code.EVEX_Vreducepd_VY_k1z_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: true),
							new OpCodeHandler_EVEX_VkWIb_er(Register.ZMM0, Code.EVEX_Vreducepd_VZ_k1z_WZ_Ib_sae_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: true)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_VkHWIb_er(Register.XMM0, Code.EVEX_Vreducess_VX_k1z_HX_WX_Ib_sae, TupleType.Tuple1_Scalar, MemorySize.Float32, onlySAE: true),
						new OpCodeHandler_EVEX_VkHWIb_er(Register.XMM0, Code.EVEX_Vreducesd_VX_k1z_HX_WX_Ib_sae, TupleType.Tuple1_Scalar, MemorySize.Float64, onlySAE: true)
					),
					invalid,
					invalid
				),

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
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_KkWIb(Register.XMM0, Code.EVEX_Vfpclassps_VK_k1_WX_Ib_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32),
							new OpCodeHandler_EVEX_KkWIb(Register.YMM0, Code.EVEX_Vfpclassps_VK_k1_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32),
							new OpCodeHandler_EVEX_KkWIb(Register.ZMM0, Code.EVEX_Vfpclassps_VK_k1_WZ_Ib_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_KkWIb(Register.XMM0, Code.EVEX_Vfpclasspd_VK_k1_WX_Ib_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64),
							new OpCodeHandler_EVEX_KkWIb(Register.YMM0, Code.EVEX_Vfpclasspd_VK_k1_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64),
							new OpCodeHandler_EVEX_KkWIb(Register.ZMM0, Code.EVEX_Vfpclasspd_VK_k1_WZ_Ib_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_KkWIb(Register.XMM0, Code.EVEX_Vfpclassss_VK_k1_WX_Ib, TupleType.Tuple1_Scalar, MemorySize.Float32),
						new OpCodeHandler_EVEX_KkWIb(Register.XMM0, Code.EVEX_Vfpclasssd_VK_k1_WX_Ib, TupleType.Tuple1_Scalar, MemorySize.Float64)
					),
					invalid,
					invalid
				),

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
				invalid,

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
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkW(Register.XMM0, Code.EVEX_Vmovups_VX_k1z_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Float32),
							new OpCodeHandler_EVEX_VkW(Register.YMM0, Code.EVEX_Vmovups_VY_k1z_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Float32),
							new OpCodeHandler_EVEX_VkW(Register.ZMM0, Code.EVEX_Vmovups_VZ_k1z_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Float32)
						),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkW(Register.XMM0, Code.EVEX_Vmovupd_VX_k1z_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Float64),
							new OpCodeHandler_EVEX_VkW(Register.YMM0, Code.EVEX_Vmovupd_VY_k1z_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Float64),
							new OpCodeHandler_EVEX_VkW(Register.ZMM0, Code.EVEX_Vmovupd_VZ_k1z_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Float64)
						)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_RM(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vmovss_VX_k1z_HX_RX, TupleType.Tuple1_Scalar, MemorySize.Float32),
							new OpCodeHandler_EVEX_VkW(Register.XMM0, Code.EVEX_Vmovss_VX_k1z_M, TupleType.Tuple1_Scalar, MemorySize.Float32)
						),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_RM(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vmovsd_VX_k1z_HX_RX, TupleType.Tuple1_Scalar, MemorySize.Float64),
							new OpCodeHandler_EVEX_VkW(Register.XMM0, Code.EVEX_Vmovsd_VX_k1z_M, TupleType.Tuple1_Scalar, MemorySize.Float64)
						)
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Code.EVEX_Vmovups_WX_k1z_VX, TupleType.Full_Mem_128, MemorySize.Packed128_Float32),
							new OpCodeHandler_EVEX_WkV(Register.YMM0, Code.EVEX_Vmovups_WY_k1z_VY, TupleType.Full_Mem_256, MemorySize.Packed256_Float32),
							new OpCodeHandler_EVEX_WkV(Register.ZMM0, Code.EVEX_Vmovups_WZ_k1z_VZ, TupleType.Full_Mem_512, MemorySize.Packed512_Float32)
						),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Code.EVEX_Vmovupd_WX_k1z_VX, TupleType.Full_Mem_128, MemorySize.Packed128_Float64),
							new OpCodeHandler_EVEX_WkV(Register.YMM0, Code.EVEX_Vmovupd_WY_k1z_VY, TupleType.Full_Mem_256, MemorySize.Packed256_Float64),
							new OpCodeHandler_EVEX_WkV(Register.ZMM0, Code.EVEX_Vmovupd_WZ_k1z_VZ, TupleType.Full_Mem_512, MemorySize.Packed512_Float64)
						)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_RM(
							new OpCodeHandler_EVEX_WkHV(Register.XMM0, Code.EVEX_Vmovss_RX_k1z_HX_VX, TupleType.Tuple1_Scalar, MemorySize.Float32),
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Code.EVEX_Vmovss_M_k1_VX, TupleType.Tuple1_Scalar, MemorySize.Float32)
						),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_RM(
							new OpCodeHandler_EVEX_WkHV(Register.XMM0, Code.EVEX_Vmovsd_RX_k1z_HX_VX, TupleType.Tuple1_Scalar, MemorySize.Float64),
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Code.EVEX_Vmovsd_M_k1_VX, TupleType.Tuple1_Scalar, MemorySize.Float64)
						)
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VHW(Register.XMM0, Code.EVEX_Vmovhlps_VX_HX_RX, Code.EVEX_Vmovlps_VX_HX_M, TupleType.Tuple2, MemorySize.Packed64_Float32),
							invalid,
							invalid
						),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VHM(Register.XMM0, Code.EVEX_Vmovlpd_VX_HX_M, TupleType.Tuple1_Scalar, MemorySize.Float64),
							invalid,
							invalid
						)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkW(Register.XMM0, Code.EVEX_Vmovsldup_VX_k1z_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Float32),
							new OpCodeHandler_EVEX_VkW(Register.YMM0, Code.EVEX_Vmovsldup_VY_k1z_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Float32),
							new OpCodeHandler_EVEX_VkW(Register.ZMM0, Code.EVEX_Vmovsldup_VZ_k1z_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Float32)
						),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkW(Register.XMM0, Code.EVEX_Vmovddup_VX_k1z_WX, TupleType.MOVDDUP_128, MemorySize.Float64, MemorySize.Float64),
							new OpCodeHandler_EVEX_VkW(Register.YMM0, Code.EVEX_Vmovddup_VY_k1z_WY, TupleType.MOVDDUP_256, MemorySize.Packed256_Float64, MemorySize.Packed256_Float64),
							new OpCodeHandler_EVEX_VkW(Register.ZMM0, Code.EVEX_Vmovddup_VZ_k1z_WZ, TupleType.MOVDDUP_512, MemorySize.Packed512_Float64, MemorySize.Packed512_Float64)
						)
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_MV(Register.XMM0, Code.EVEX_Vmovlps_M_VX, TupleType.Tuple2, MemorySize.Packed64_Float32),
							invalid,
							invalid
						),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_MV(Register.XMM0, Code.EVEX_Vmovlpd_M_VX, TupleType.Tuple1_Scalar, MemorySize.Float64),
							invalid,
							invalid
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vunpcklps_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vunpcklps_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vunpcklps_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32)
						),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vunpcklpd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vunpcklpd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vunpcklpd_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vunpckhps_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vunpckhps_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vunpckhps_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32)
						),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vunpckhpd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vunpckhpd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vunpckhpd_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VHW(Register.XMM0, Code.EVEX_Vmovlhps_VX_HX_RX, Code.EVEX_Vmovhps_VX_HX_M, TupleType.Tuple2, MemorySize.Packed64_Float32),
							invalid,
							invalid
						),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VHM(Register.XMM0, Code.EVEX_Vmovhpd_VX_HX_M, TupleType.Tuple1_Scalar, MemorySize.Float64),
							invalid,
							invalid
						)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkW(Register.XMM0, Code.EVEX_Vmovshdup_VX_k1z_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Float32),
							new OpCodeHandler_EVEX_VkW(Register.YMM0, Code.EVEX_Vmovshdup_VY_k1z_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Float32),
							new OpCodeHandler_EVEX_VkW(Register.ZMM0, Code.EVEX_Vmovshdup_VZ_k1z_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Float32)
						),
						invalid
					),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_MV(Register.XMM0, Code.EVEX_Vmovhps_M_VX, TupleType.Tuple2, MemorySize.Packed64_Float32),
							invalid,
							invalid
						),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_MV(Register.XMM0, Code.EVEX_Vmovhpd_M_VX, TupleType.Tuple1_Scalar, MemorySize.Float64),
							invalid,
							invalid
						)
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
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkW(Register.XMM0, Code.EVEX_Vmovaps_VX_k1z_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Float32),
							new OpCodeHandler_EVEX_VkW(Register.YMM0, Code.EVEX_Vmovaps_VY_k1z_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Float32),
							new OpCodeHandler_EVEX_VkW(Register.ZMM0, Code.EVEX_Vmovaps_VZ_k1z_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Float32)
						),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkW(Register.XMM0, Code.EVEX_Vmovapd_VX_k1z_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Float64),
							new OpCodeHandler_EVEX_VkW(Register.YMM0, Code.EVEX_Vmovapd_VY_k1z_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Float64),
							new OpCodeHandler_EVEX_VkW(Register.ZMM0, Code.EVEX_Vmovapd_VZ_k1z_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Code.EVEX_Vmovaps_WX_k1z_VX, TupleType.Full_Mem_128, MemorySize.Packed128_Float32),
							new OpCodeHandler_EVEX_WkV(Register.YMM0, Code.EVEX_Vmovaps_WY_k1z_VY, TupleType.Full_Mem_256, MemorySize.Packed256_Float32),
							new OpCodeHandler_EVEX_WkV(Register.ZMM0, Code.EVEX_Vmovaps_WZ_k1z_VZ, TupleType.Full_Mem_512, MemorySize.Packed512_Float32)
						),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Code.EVEX_Vmovapd_WX_k1z_VX, TupleType.Full_Mem_128, MemorySize.Packed128_Float64),
							new OpCodeHandler_EVEX_WkV(Register.YMM0, Code.EVEX_Vmovapd_WY_k1z_VY, TupleType.Full_Mem_256, MemorySize.Packed256_Float64),
							new OpCodeHandler_EVEX_WkV(Register.ZMM0, Code.EVEX_Vmovapd_WZ_k1z_VZ, TupleType.Full_Mem_512, MemorySize.Packed512_Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					invalid,
					new OpCodeHandler_EVEX_V_H_Ev_er(Register.XMM0, Code.EVEX_Vcvtsi2ss_VX_HX_Ed_er, Code.EVEX_Vcvtsi2ss_VX_HX_Eq_er, TupleType.Tuple1_Scalar, MemorySize.Int32, MemorySize.Int64, onlySAE: false),
					new OpCodeHandler_EVEX_V_H_Ev_er(Register.XMM0, Code.EVEX_Vcvtsi2sd_VX_HX_Ed, Code.EVEX_Vcvtsi2sd_VX_HX_Eq_er, TupleType.Tuple1_Scalar, MemorySize.Int32, MemorySize.Int64, onlySAE: false, noERd: true)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_MV(Register.XMM0, Code.EVEX_Vmovntps_M_VX, TupleType.Full_Mem_128, MemorySize.Packed128_Float32),
							new OpCodeHandler_EVEX_MV(Register.YMM0, Code.EVEX_Vmovntps_M_VY, TupleType.Full_Mem_256, MemorySize.Packed256_Float32),
							new OpCodeHandler_EVEX_MV(Register.ZMM0, Code.EVEX_Vmovntps_M_VZ, TupleType.Full_Mem_512, MemorySize.Packed512_Float32)
						),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_MV(Register.XMM0, Code.EVEX_Vmovntpd_M_VX, TupleType.Full_Mem_128, MemorySize.Packed128_Float64),
							new OpCodeHandler_EVEX_MV(Register.YMM0, Code.EVEX_Vmovntpd_M_VY, TupleType.Full_Mem_256, MemorySize.Packed256_Float64),
							new OpCodeHandler_EVEX_MV(Register.ZMM0, Code.EVEX_Vmovntpd_M_VZ, TupleType.Full_Mem_512, MemorySize.Packed512_Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					invalid,
					new OpCodeHandler_EVEX_Gv_W_er(Register.XMM0, Code.EVEX_Vcvttss2si_Gd_WX_sae, Code.EVEX_Vcvttss2si_Gq_WX_sae, TupleType.Tuple1_Scalar_4, MemorySize.Float32, onlySAE: true),
					new OpCodeHandler_EVEX_Gv_W_er(Register.XMM0, Code.EVEX_Vcvttsd2si_Gd_WX_sae, Code.EVEX_Vcvttsd2si_Gq_WX_sae, TupleType.Tuple1_Scalar_8, MemorySize.Float64, onlySAE: true)
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					invalid,
					new OpCodeHandler_EVEX_Gv_W_er(Register.XMM0, Code.EVEX_Vcvtss2si_Gd_WX_er, Code.EVEX_Vcvtss2si_Gq_WX_er, TupleType.Tuple1_Scalar_4, MemorySize.Float32, onlySAE: false),
					new OpCodeHandler_EVEX_Gv_W_er(Register.XMM0, Code.EVEX_Vcvtsd2si_Gd_WX_er, Code.EVEX_Vcvtsd2si_Gq_WX_er, TupleType.Tuple1_Scalar_8, MemorySize.Float64, onlySAE: false)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_VW_er(Register.XMM0, Code.EVEX_Vucomiss_VX_WX_sae, TupleType.Tuple1_Scalar, MemorySize.Float32, MemorySize.Float32, onlySAE: true),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_EVEX_VW_er(Register.XMM0, Code.EVEX_Vucomisd_VX_WX_sae, TupleType.Tuple1_Scalar, MemorySize.Float64, MemorySize.Float64, onlySAE: true)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_VW_er(Register.XMM0, Code.EVEX_Vcomiss_VX_WX_sae, TupleType.Tuple1_Scalar, MemorySize.Float32, MemorySize.Float32, onlySAE: true),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_EVEX_VW_er(Register.XMM0, Code.EVEX_Vcomisd_VX_WX_sae, TupleType.Tuple1_Scalar, MemorySize.Float64, MemorySize.Float64, onlySAE: true)
					),
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
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkW_er(Register.XMM0, Code.EVEX_Vsqrtps_VX_k1z_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkW_er(Register.YMM0, Code.EVEX_Vsqrtps_VY_k1z_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkW_er(Register.ZMM0, Code.EVEX_Vsqrtps_VZ_k1z_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32, onlySAE: false)
						),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkW_er(Register.XMM0, Code.EVEX_Vsqrtpd_VX_k1z_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkW_er(Register.YMM0, Code.EVEX_Vsqrtpd_VY_k1z_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkW_er(Register.ZMM0, Code.EVEX_Vsqrtpd_VZ_k1z_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: false)
						)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vsqrtss_VX_k1z_HX_WX_er, TupleType.Tuple1_Scalar, MemorySize.Float32, onlySAE: false),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vsqrtsd_VX_k1z_HX_WX_er, TupleType.Tuple1_Scalar, MemorySize.Float64, onlySAE: false)
					)
				),
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vandps_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vandps_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vandps_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32)
						),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vandpd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vandpd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vandpd_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vandnps_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vandnps_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vandnps_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32)
						),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vandnpd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vandnpd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vandnpd_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vorps_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vorps_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vorps_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32)
						),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vorpd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vorpd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vorpd_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vxorps_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vxorps_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vxorps_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32)
						),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vxorpd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vxorpd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vxorpd_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64)
						)
					),
					invalid,
					invalid
				),

				// 58
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vaddps_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vaddps_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vaddps_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32, onlySAE: false)
						),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vaddpd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vaddpd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vaddpd_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: false)
						)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vaddss_VX_k1z_HX_WX_er, TupleType.Tuple1_Scalar, MemorySize.Float32, onlySAE: false),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vaddsd_VX_k1z_HX_WX_er, TupleType.Tuple1_Scalar, MemorySize.Float64, onlySAE: false)
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vmulps_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vmulps_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vmulps_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32, onlySAE: false)
						),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vmulpd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vmulpd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vmulpd_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: false)
						)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vmulss_VX_k1z_HX_WX_er, TupleType.Tuple1_Scalar, MemorySize.Float32, onlySAE: false),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vmulsd_VX_k1z_HX_WX_er, TupleType.Tuple1_Scalar, MemorySize.Float64, onlySAE: false)
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkW_er(Register.XMM0, Register.XMM0, Code.EVEX_Vcvtps2pd_VX_k1z_WX_b, TupleType.Half_128, MemorySize.Packed64_Float32, MemorySize.Broadcast64_Float32, onlySAE: true),
							new OpCodeHandler_EVEX_VkW_er(Register.YMM0, Register.XMM0, Code.EVEX_Vcvtps2pd_VY_k1z_WX_b, TupleType.Half_256, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: true),
							new OpCodeHandler_EVEX_VkW_er(Register.ZMM0, Register.YMM0, Code.EVEX_Vcvtps2pd_VZ_k1z_WY_sae_b, TupleType.Half_512, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: true)
						),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkW_er(Register.XMM0, Register.XMM0, Code.EVEX_Vcvtpd2ps_VX_k1z_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkW_er(Register.XMM0, Register.YMM0, Code.EVEX_Vcvtpd2ps_VX_k1z_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkW_er(Register.YMM0, Register.ZMM0, Code.EVEX_Vcvtpd2ps_VY_k1z_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: false)
						)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vcvtss2sd_VX_k1z_HX_WX_sae, TupleType.Tuple1_Scalar, MemorySize.Float32, onlySAE: true),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vcvtsd2ss_VX_k1z_HX_WX_er, TupleType.Tuple1_Scalar, MemorySize.Float64, onlySAE: false)
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkW_er(Register.XMM0, Code.EVEX_Vcvtdq2ps_VX_k1z_WX_b, TupleType.Full_128, MemorySize.Packed128_Int32, MemorySize.Broadcast128_Int32, onlySAE: false),
							new OpCodeHandler_EVEX_VkW_er(Register.YMM0, Code.EVEX_Vcvtdq2ps_VY_k1z_WY_b, TupleType.Full_256, MemorySize.Packed256_Int32, MemorySize.Broadcast256_Int32, onlySAE: false),
							new OpCodeHandler_EVEX_VkW_er(Register.ZMM0, Code.EVEX_Vcvtdq2ps_VZ_k1z_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Int32, MemorySize.Broadcast512_Int32, onlySAE: false)
						),
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkW_er(Register.XMM0, Register.XMM0, Code.EVEX_Vcvtqq2ps_VX_k1z_WX_b, TupleType.Full_128, MemorySize.Packed128_Int64, MemorySize.Broadcast128_Int64, onlySAE: false),
							new OpCodeHandler_EVEX_VkW_er(Register.XMM0, Register.YMM0, Code.EVEX_Vcvtqq2ps_VX_k1z_WY_b, TupleType.Full_256, MemorySize.Packed256_Int64, MemorySize.Broadcast256_Int64, onlySAE: false),
							new OpCodeHandler_EVEX_VkW_er(Register.YMM0, Register.ZMM0, Code.EVEX_Vcvtqq2ps_VY_k1z_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Int64, MemorySize.Broadcast512_Int64, onlySAE: false)
						)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkW_er(Register.XMM0, Code.EVEX_Vcvtps2dq_VX_k1z_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkW_er(Register.YMM0, Code.EVEX_Vcvtps2dq_VY_k1z_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkW_er(Register.ZMM0, Code.EVEX_Vcvtps2dq_VZ_k1z_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32, onlySAE: false)
						),
						invalid
					),
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkW_er(Register.XMM0, Code.EVEX_Vcvttps2dq_VX_k1z_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: true),
							new OpCodeHandler_EVEX_VkW_er(Register.YMM0, Code.EVEX_Vcvttps2dq_VY_k1z_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: true),
							new OpCodeHandler_EVEX_VkW_er(Register.ZMM0, Code.EVEX_Vcvttps2dq_VZ_k1z_WZ_sae_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32, onlySAE: true)
						),
						invalid
					),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vsubps_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vsubps_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vsubps_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32, onlySAE: false)
						),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vsubpd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vsubpd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vsubpd_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: false)
						)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vsubss_VX_k1z_HX_WX_er, TupleType.Tuple1_Scalar, MemorySize.Float32, onlySAE: false),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vsubsd_VX_k1z_HX_WX_er, TupleType.Tuple1_Scalar, MemorySize.Float64, onlySAE: false)
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vminps_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: true),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vminps_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: true),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vminps_VZ_k1z_HZ_WZ_sae_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32, onlySAE: true)
						),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vminpd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: true),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vminpd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: true),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vminpd_VZ_k1z_HZ_WZ_sae_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: true)
						)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vminss_VX_k1z_HX_WX_sae, TupleType.Tuple1_Scalar, MemorySize.Float32, onlySAE: true),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vminsd_VX_k1z_HX_WX_sae, TupleType.Tuple1_Scalar, MemorySize.Float64, onlySAE: true)
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vdivps_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vdivps_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vdivps_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32, onlySAE: false)
						),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vdivpd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vdivpd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vdivpd_VZ_k1z_HZ_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: false)
						)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vdivss_VX_k1z_HX_WX_er, TupleType.Tuple1_Scalar, MemorySize.Float32, onlySAE: false),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vdivsd_VX_k1z_HX_WX_er, TupleType.Tuple1_Scalar, MemorySize.Float64, onlySAE: false)
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vmaxps_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: true),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vmaxps_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: true),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vmaxps_VZ_k1z_HZ_WZ_sae_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32, onlySAE: true)
						),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vmaxpd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: true),
							new OpCodeHandler_EVEX_VkHW_er(Register.YMM0, Code.EVEX_Vmaxpd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: true),
							new OpCodeHandler_EVEX_VkHW_er(Register.ZMM0, Code.EVEX_Vmaxpd_VZ_k1z_HZ_WZ_sae_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: true)
						)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vmaxss_VX_k1z_HX_WX_sae, TupleType.Tuple1_Scalar, MemorySize.Float32, onlySAE: true),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_EVEX_VkHW_er(Register.XMM0, Code.EVEX_Vmaxsd_VX_k1z_HX_WX_sae, TupleType.Tuple1_Scalar, MemorySize.Float64, onlySAE: true)
					)
				),

				// 60
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpunpcklbw_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int8),
						new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpunpcklbw_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int8),
						new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpunpcklbw_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpunpcklwd_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int16),
						new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpunpcklwd_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int16),
						new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpunpcklwd_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpunpckldq_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Int32, MemorySize.Broadcast128_Int32),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpunpckldq_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Int32, MemorySize.Broadcast256_Int32),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpunpckldq_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Int32, MemorySize.Broadcast512_Int32)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpacksswb_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int16),
						new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpacksswb_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int16),
						new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpacksswb_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_KkHW(Register.XMM0, Code.EVEX_Vpcmpgtb_VK_k1_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int8),
						new OpCodeHandler_EVEX_KkHW(Register.YMM0, Code.EVEX_Vpcmpgtb_VK_k1_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int8),
						new OpCodeHandler_EVEX_KkHW(Register.ZMM0, Code.EVEX_Vpcmpgtb_VK_k1_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_KkHW(Register.XMM0, Code.EVEX_Vpcmpgtw_VK_k1_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int16),
						new OpCodeHandler_EVEX_KkHW(Register.YMM0, Code.EVEX_Vpcmpgtw_VK_k1_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int16),
						new OpCodeHandler_EVEX_KkHW(Register.ZMM0, Code.EVEX_Vpcmpgtw_VK_k1_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_KkHW(Register.XMM0, Code.EVEX_Vpcmpgtd_VK_k1_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Int32, MemorySize.Broadcast128_Int32),
							new OpCodeHandler_EVEX_KkHW(Register.YMM0, Code.EVEX_Vpcmpgtd_VK_k1_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Int32, MemorySize.Broadcast256_Int32),
							new OpCodeHandler_EVEX_KkHW(Register.ZMM0, Code.EVEX_Vpcmpgtd_VK_k1_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Int32, MemorySize.Broadcast512_Int32)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpackuswb_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int16),
						new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpackuswb_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int16),
						new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpackuswb_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int16)
					),
					invalid,
					invalid
				),

				// 68
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpunpckhbw_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int8),
						new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpunpckhbw_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int8),
						new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpunpckhbw_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpunpckhwd_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int16),
						new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpunpckhwd_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int16),
						new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpunpckhwd_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpunpckhdq_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Int32, MemorySize.Broadcast128_Int32),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpunpckhdq_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Int32, MemorySize.Broadcast256_Int32),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpunpckhdq_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Int32, MemorySize.Broadcast512_Int32)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpackssdw_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Int32, MemorySize.Broadcast128_Int32),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpackssdw_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Int32, MemorySize.Broadcast256_Int32),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpackssdw_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Int32, MemorySize.Broadcast512_Int32)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpunpcklqdq_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Int64, MemorySize.Broadcast128_Int64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpunpcklqdq_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Int64, MemorySize.Broadcast256_Int64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpunpcklqdq_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Int64, MemorySize.Broadcast512_Int64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpunpckhqdq_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Int64, MemorySize.Broadcast128_Int64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpunpckhqdq_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Int64, MemorySize.Broadcast256_Int64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpunpckhqdq_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Int64, MemorySize.Broadcast512_Int64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VX_Ev(Code.EVEX_Vmovd_VX_Ed, Code.EVEX_Vmovq_VX_Eq, TupleType.Tuple1_Scalar),
						invalid,
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkW(Register.XMM0, Code.EVEX_Vmovdqa32_VX_k1z_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int32),
							new OpCodeHandler_EVEX_VkW(Register.YMM0, Code.EVEX_Vmovdqa32_VY_k1z_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int32),
							new OpCodeHandler_EVEX_VkW(Register.ZMM0, Code.EVEX_Vmovdqa32_VZ_k1z_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkW(Register.XMM0, Code.EVEX_Vmovdqa64_VX_k1z_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int32),
							new OpCodeHandler_EVEX_VkW(Register.YMM0, Code.EVEX_Vmovdqa64_VY_k1z_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int32),
							new OpCodeHandler_EVEX_VkW(Register.ZMM0, Code.EVEX_Vmovdqa64_VZ_k1z_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int32)
						)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkW(Register.XMM0, Code.EVEX_Vmovdqu32_VX_k1z_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int32),
							new OpCodeHandler_EVEX_VkW(Register.YMM0, Code.EVEX_Vmovdqu32_VY_k1z_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int32),
							new OpCodeHandler_EVEX_VkW(Register.ZMM0, Code.EVEX_Vmovdqu32_VZ_k1z_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkW(Register.XMM0, Code.EVEX_Vmovdqu64_VX_k1z_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int64),
							new OpCodeHandler_EVEX_VkW(Register.YMM0, Code.EVEX_Vmovdqu64_VY_k1z_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int64),
							new OpCodeHandler_EVEX_VkW(Register.ZMM0, Code.EVEX_Vmovdqu64_VZ_k1z_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int64)
						)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkW(Register.XMM0, Code.EVEX_Vmovdqu8_VX_k1z_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int8),
							new OpCodeHandler_EVEX_VkW(Register.YMM0, Code.EVEX_Vmovdqu8_VY_k1z_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int8),
							new OpCodeHandler_EVEX_VkW(Register.ZMM0, Code.EVEX_Vmovdqu8_VZ_k1z_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int8)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkW(Register.XMM0, Code.EVEX_Vmovdqu16_VX_k1z_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int16),
							new OpCodeHandler_EVEX_VkW(Register.YMM0, Code.EVEX_Vmovdqu16_VY_k1z_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int16),
							new OpCodeHandler_EVEX_VkW(Register.ZMM0, Code.EVEX_Vmovdqu16_VZ_k1z_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int16)
						)
					)
				),

				// 70
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkWIb(Register.XMM0, Code.EVEX_Vpshufd_VX_k1z_WX_Ib_b, TupleType.Full_128, MemorySize.Packed128_Int32, MemorySize.Broadcast128_Int32),
							new OpCodeHandler_EVEX_VkWIb(Register.YMM0, Code.EVEX_Vpshufd_VY_k1z_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_Int32, MemorySize.Broadcast256_Int32),
							new OpCodeHandler_EVEX_VkWIb(Register.ZMM0, Code.EVEX_Vpshufd_VZ_k1z_WZ_Ib_b, TupleType.Full_512, MemorySize.Packed512_Int32, MemorySize.Broadcast512_Int32)
						),
						invalid
					),
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkWIb(Register.XMM0, Code.EVEX_Vpshufhw_VX_k1z_WX_Ib, TupleType.Full_Mem_128, MemorySize.Packed128_Int16),
						new OpCodeHandler_EVEX_VkWIb(Register.YMM0, Code.EVEX_Vpshufhw_VY_k1z_WY_Ib, TupleType.Full_Mem_256, MemorySize.Packed256_Int16),
						new OpCodeHandler_EVEX_VkWIb(Register.ZMM0, Code.EVEX_Vpshufhw_VZ_k1z_WZ_Ib, TupleType.Full_Mem_512, MemorySize.Packed512_Int16)
					),
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkWIb(Register.XMM0, Code.EVEX_Vpshuflw_VX_k1z_WX_Ib, TupleType.Full_Mem_128, MemorySize.Packed128_Int16),
						new OpCodeHandler_EVEX_VkWIb(Register.YMM0, Code.EVEX_Vpshuflw_VY_k1z_WY_Ib, TupleType.Full_Mem_256, MemorySize.Packed256_Int16),
						new OpCodeHandler_EVEX_VkWIb(Register.ZMM0, Code.EVEX_Vpshuflw_VZ_k1z_WZ_Ib, TupleType.Full_Mem_512, MemorySize.Packed512_Int16)
					)
				),
				new OpCodeHandler_Group(handlers_Grp_0F71),
				new OpCodeHandler_Group(handlers_Grp_0F72),
				new OpCodeHandler_Group(handlers_Grp_0F73),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_KkHW(Register.XMM0, Code.EVEX_Vpcmpeqb_VK_k1_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int8),
						new OpCodeHandler_EVEX_KkHW(Register.YMM0, Code.EVEX_Vpcmpeqb_VK_k1_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int8),
						new OpCodeHandler_EVEX_KkHW(Register.ZMM0, Code.EVEX_Vpcmpeqb_VK_k1_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_KkHW(Register.XMM0, Code.EVEX_Vpcmpeqw_VK_k1_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int16),
						new OpCodeHandler_EVEX_KkHW(Register.YMM0, Code.EVEX_Vpcmpeqw_VK_k1_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int16),
						new OpCodeHandler_EVEX_KkHW(Register.ZMM0, Code.EVEX_Vpcmpeqw_VK_k1_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_KkHW(Register.XMM0, Code.EVEX_Vpcmpeqd_VK_k1_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Int32, MemorySize.Broadcast128_Int32),
							new OpCodeHandler_EVEX_KkHW(Register.YMM0, Code.EVEX_Vpcmpeqd_VK_k1_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Int32, MemorySize.Broadcast256_Int32),
							new OpCodeHandler_EVEX_KkHW(Register.ZMM0, Code.EVEX_Vpcmpeqd_VK_k1_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Int32, MemorySize.Broadcast512_Int32)
						),
						invalid
					),
					invalid,
					invalid
				),
				invalid,

				// 78
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkW_er(Register.XMM0, Register.XMM0, Code.EVEX_Vcvttps2udq_VX_k1z_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: true),
							new OpCodeHandler_EVEX_VkW_er(Register.YMM0, Register.YMM0, Code.EVEX_Vcvttps2udq_VY_k1z_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: true),
							new OpCodeHandler_EVEX_VkW_er(Register.ZMM0, Register.ZMM0, Code.EVEX_Vcvttps2udq_VZ_k1z_WZ_sae_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32, onlySAE: true)
						),
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkW_er(Register.XMM0, Register.XMM0, Code.EVEX_Vcvttpd2udq_VX_k1z_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: true),
							new OpCodeHandler_EVEX_VkW_er(Register.XMM0, Register.YMM0, Code.EVEX_Vcvttpd2udq_VX_k1z_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: true),
							new OpCodeHandler_EVEX_VkW_er(Register.YMM0, Register.ZMM0, Code.EVEX_Vcvttpd2udq_VY_k1z_WZ_sae_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: true)
						)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkW_er(Register.XMM0, Register.XMM0, Code.EVEX_Vcvttps2uqq_VX_k1z_WX_b, TupleType.Half_128, MemorySize.Packed64_Float32, MemorySize.Broadcast64_Float32, onlySAE: true),
							new OpCodeHandler_EVEX_VkW_er(Register.YMM0, Register.XMM0, Code.EVEX_Vcvttps2uqq_VY_k1z_WX_b, TupleType.Half_256, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: true),
							new OpCodeHandler_EVEX_VkW_er(Register.ZMM0, Register.YMM0, Code.EVEX_Vcvttps2uqq_VZ_k1z_WY_sae_b, TupleType.Half_512, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: true)
						),
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkW_er(Register.XMM0, Register.XMM0, Code.EVEX_Vcvttpd2uqq_VX_k1z_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: true),
							new OpCodeHandler_EVEX_VkW_er(Register.YMM0, Register.YMM0, Code.EVEX_Vcvttpd2uqq_VY_k1z_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: true),
							new OpCodeHandler_EVEX_VkW_er(Register.ZMM0, Register.ZMM0, Code.EVEX_Vcvttpd2uqq_VZ_k1z_WZ_sae_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: true)
						)
					),
					new OpCodeHandler_EVEX_Gv_W_er(Register.XMM0, Code.EVEX_Vcvttss2usi_Gd_WX_sae, Code.EVEX_Vcvttss2usi_Gq_WX_sae, TupleType.Tuple1_Fixed_4, MemorySize.Float32, onlySAE: true),
					new OpCodeHandler_EVEX_Gv_W_er(Register.XMM0, Code.EVEX_Vcvttsd2usi_Gd_WX_sae, Code.EVEX_Vcvttsd2usi_Gq_WX_sae, TupleType.Tuple1_Fixed_8, MemorySize.Float64, onlySAE: true)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkW_er(Register.XMM0, Register.XMM0, Code.EVEX_Vcvtps2udq_VX_k1z_WX_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkW_er(Register.YMM0, Register.YMM0, Code.EVEX_Vcvtps2udq_VY_k1z_WY_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkW_er(Register.ZMM0, Register.ZMM0, Code.EVEX_Vcvtps2udq_VZ_k1z_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32, onlySAE: false)
						),
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkW_er(Register.XMM0, Register.XMM0, Code.EVEX_Vcvtpd2udq_VX_k1z_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkW_er(Register.XMM0, Register.YMM0, Code.EVEX_Vcvtpd2udq_VX_k1z_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkW_er(Register.YMM0, Register.ZMM0, Code.EVEX_Vcvtpd2udq_VY_k1z_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: false)
						)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkW_er(Register.XMM0, Register.XMM0, Code.EVEX_Vcvtps2uqq_VX_k1z_WX_b, TupleType.Half_128, MemorySize.Packed64_Float32, MemorySize.Broadcast64_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkW_er(Register.YMM0, Register.XMM0, Code.EVEX_Vcvtps2uqq_VY_k1z_WX_b, TupleType.Half_256, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkW_er(Register.ZMM0, Register.YMM0, Code.EVEX_Vcvtps2uqq_VZ_k1z_WY_er_b, TupleType.Half_512, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: false)
						),
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkW_er(Register.XMM0, Register.XMM0, Code.EVEX_Vcvtpd2uqq_VX_k1z_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkW_er(Register.YMM0, Register.YMM0, Code.EVEX_Vcvtpd2uqq_VY_k1z_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkW_er(Register.ZMM0, Register.ZMM0, Code.EVEX_Vcvtpd2uqq_VZ_k1z_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: false)
						)
					),
					new OpCodeHandler_EVEX_Gv_W_er(Register.XMM0, Code.EVEX_Vcvtss2usi_Gd_WX_er, Code.EVEX_Vcvtss2usi_Gq_WX_er, TupleType.Tuple1_Fixed_4, MemorySize.Float32, onlySAE: false),
					new OpCodeHandler_EVEX_Gv_W_er(Register.XMM0, Code.EVEX_Vcvtsd2usi_Gd_WX_er, Code.EVEX_Vcvtsd2usi_Gq_WX_er, TupleType.Tuple1_Fixed_8, MemorySize.Float64, onlySAE: false)
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkW_er(Register.XMM0, Register.XMM0, Code.EVEX_Vcvttps2qq_VX_k1z_WX_b, TupleType.Half_128, MemorySize.Packed64_Float32, MemorySize.Broadcast64_Float32, onlySAE: true),
							new OpCodeHandler_EVEX_VkW_er(Register.YMM0, Register.XMM0, Code.EVEX_Vcvttps2qq_VY_k1z_WX_b, TupleType.Half_256, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: true),
							new OpCodeHandler_EVEX_VkW_er(Register.ZMM0, Register.YMM0, Code.EVEX_Vcvttps2qq_VZ_k1z_WY_sae_b, TupleType.Half_512, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: true)
						),
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkW_er(Register.XMM0, Register.XMM0, Code.EVEX_Vcvttpd2qq_VX_k1z_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: true),
							new OpCodeHandler_EVEX_VkW_er(Register.YMM0, Register.YMM0, Code.EVEX_Vcvttpd2qq_VY_k1z_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: true),
							new OpCodeHandler_EVEX_VkW_er(Register.ZMM0, Register.ZMM0, Code.EVEX_Vcvttpd2qq_VZ_k1z_WZ_sae_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: true)
						)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkW(Register.XMM0, Register.XMM0, Code.EVEX_Vcvtudq2pd_VX_k1z_WX_b, TupleType.Half_128, MemorySize.Packed64_UInt32, MemorySize.Broadcast64_UInt32),
							new OpCodeHandler_EVEX_VkW(Register.YMM0, Register.XMM0, Code.EVEX_Vcvtudq2pd_VY_k1z_WX_b, TupleType.Half_256, MemorySize.Packed128_UInt32, MemorySize.Broadcast128_UInt32),
							new OpCodeHandler_EVEX_VkW(Register.ZMM0, Register.YMM0, Code.EVEX_Vcvtudq2pd_VZ_k1z_WY_b, TupleType.Half_512, MemorySize.Packed256_UInt32, MemorySize.Broadcast256_UInt32)
						),
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkW_er(Register.XMM0, Register.XMM0, Code.EVEX_Vcvtuqq2pd_VX_k1z_WX_b, TupleType.Full_128, MemorySize.Packed128_UInt64, MemorySize.Broadcast128_UInt64, onlySAE: false),
							new OpCodeHandler_EVEX_VkW_er(Register.YMM0, Register.YMM0, Code.EVEX_Vcvtuqq2pd_VY_k1z_WY_b, TupleType.Full_256, MemorySize.Packed256_UInt64, MemorySize.Broadcast256_UInt64, onlySAE: false),
							new OpCodeHandler_EVEX_VkW_er(Register.ZMM0, Register.ZMM0, Code.EVEX_Vcvtuqq2pd_VZ_k1z_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_UInt64, MemorySize.Broadcast512_UInt64, onlySAE: false)
						)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkW_er(Register.XMM0, Register.XMM0, Code.EVEX_Vcvtudq2ps_VX_k1z_WX_b, TupleType.Full_128, MemorySize.Packed128_UInt32, MemorySize.Broadcast128_UInt32, onlySAE: false),
							new OpCodeHandler_EVEX_VkW_er(Register.YMM0, Register.YMM0, Code.EVEX_Vcvtudq2ps_VY_k1z_WY_b, TupleType.Full_256, MemorySize.Packed256_UInt32, MemorySize.Broadcast256_UInt32, onlySAE: false),
							new OpCodeHandler_EVEX_VkW_er(Register.ZMM0, Register.ZMM0, Code.EVEX_Vcvtudq2ps_VZ_k1z_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_UInt32, MemorySize.Broadcast512_UInt32, onlySAE: false)
						),
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkW_er(Register.XMM0, Register.XMM0, Code.EVEX_Vcvtuqq2ps_VX_k1z_WX_b, TupleType.Full_128, MemorySize.Packed128_UInt64, MemorySize.Broadcast128_UInt64, onlySAE: false),
							new OpCodeHandler_EVEX_VkW_er(Register.XMM0, Register.YMM0, Code.EVEX_Vcvtuqq2ps_VX_k1z_WY_b, TupleType.Full_256, MemorySize.Packed256_UInt64, MemorySize.Broadcast256_UInt64, onlySAE: false),
							new OpCodeHandler_EVEX_VkW_er(Register.YMM0, Register.ZMM0, Code.EVEX_Vcvtuqq2ps_VY_k1z_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_UInt64, MemorySize.Broadcast512_UInt64, onlySAE: false)
						)
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkW_er(Register.XMM0, Register.XMM0, Code.EVEX_Vcvtps2qq_VX_k1z_WX_b, TupleType.Half_128, MemorySize.Packed64_Float32, MemorySize.Broadcast64_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkW_er(Register.YMM0, Register.XMM0, Code.EVEX_Vcvtps2qq_VY_k1z_WX_b, TupleType.Half_256, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32, onlySAE: false),
							new OpCodeHandler_EVEX_VkW_er(Register.ZMM0, Register.YMM0, Code.EVEX_Vcvtps2qq_VZ_k1z_WY_er_b, TupleType.Half_512, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32, onlySAE: false)
						),
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkW_er(Register.XMM0, Register.XMM0, Code.EVEX_Vcvtpd2qq_VX_k1z_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkW_er(Register.YMM0, Register.YMM0, Code.EVEX_Vcvtpd2qq_VY_k1z_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkW_er(Register.ZMM0, Register.ZMM0, Code.EVEX_Vcvtpd2qq_VZ_k1z_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: false)
						)
					),
					new OpCodeHandler_EVEX_V_H_Ev_er(Register.XMM0, Code.EVEX_Vcvtusi2ss_VX_HX_Ed_er, Code.EVEX_Vcvtusi2ss_VX_HX_Eq_er, TupleType.Tuple1_Fixed, MemorySize.UInt32, MemorySize.UInt64, onlySAE: false),
					new OpCodeHandler_EVEX_V_H_Ev_er(Register.XMM0, Code.EVEX_Vcvtusi2sd_VX_HX_Ed, Code.EVEX_Vcvtusi2sd_VX_HX_Eq_er, TupleType.Tuple1_Fixed, MemorySize.UInt32, MemorySize.UInt64, onlySAE: false, noERd: true)
				),
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_Ev_VX(Code.EVEX_Vmovd_Ed_VX, Code.EVEX_Vmovq_Eq_VX, TupleType.Tuple1_Scalar),
						invalid,
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VW(Register.XMM0, Code.EVEX_Vmovq_VX_WX, TupleType.Tuple1_Scalar, MemorySize.UInt64),
							invalid,
							invalid
						)
					),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Code.EVEX_Vmovdqa32_WX_k1z_VX, TupleType.Full_Mem_128, MemorySize.Packed128_Int32),
							new OpCodeHandler_EVEX_WkV(Register.YMM0, Code.EVEX_Vmovdqa32_WY_k1z_VY, TupleType.Full_Mem_256, MemorySize.Packed256_Int32),
							new OpCodeHandler_EVEX_WkV(Register.ZMM0, Code.EVEX_Vmovdqa32_WZ_k1z_VZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Code.EVEX_Vmovdqa64_WX_k1z_VX, TupleType.Full_Mem_128, MemorySize.Packed128_Int32),
							new OpCodeHandler_EVEX_WkV(Register.YMM0, Code.EVEX_Vmovdqa64_WY_k1z_VY, TupleType.Full_Mem_256, MemorySize.Packed256_Int32),
							new OpCodeHandler_EVEX_WkV(Register.ZMM0, Code.EVEX_Vmovdqa64_WZ_k1z_VZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int32)
						)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Code.EVEX_Vmovdqu32_WX_k1z_VX, TupleType.Full_Mem_128, MemorySize.Packed128_Int32),
							new OpCodeHandler_EVEX_WkV(Register.YMM0, Code.EVEX_Vmovdqu32_WY_k1z_VY, TupleType.Full_Mem_256, MemorySize.Packed256_Int32),
							new OpCodeHandler_EVEX_WkV(Register.ZMM0, Code.EVEX_Vmovdqu32_WZ_k1z_VZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Code.EVEX_Vmovdqu64_WX_k1z_VX, TupleType.Full_Mem_128, MemorySize.Packed128_Int64),
							new OpCodeHandler_EVEX_WkV(Register.YMM0, Code.EVEX_Vmovdqu64_WY_k1z_VY, TupleType.Full_Mem_256, MemorySize.Packed256_Int64),
							new OpCodeHandler_EVEX_WkV(Register.ZMM0, Code.EVEX_Vmovdqu64_WZ_k1z_VZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int64)
						)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Code.EVEX_Vmovdqu8_WX_k1z_VX, TupleType.Full_Mem_128, MemorySize.Packed128_Int8),
							new OpCodeHandler_EVEX_WkV(Register.YMM0, Code.EVEX_Vmovdqu8_WY_k1z_VY, TupleType.Full_Mem_256, MemorySize.Packed256_Int8),
							new OpCodeHandler_EVEX_WkV(Register.ZMM0, Code.EVEX_Vmovdqu8_WZ_k1z_VZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int8)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_WkV(Register.XMM0, Code.EVEX_Vmovdqu16_WX_k1z_VX, TupleType.Full_Mem_128, MemorySize.Packed128_Int16),
							new OpCodeHandler_EVEX_WkV(Register.YMM0, Code.EVEX_Vmovdqu16_WY_k1z_VY, TupleType.Full_Mem_256, MemorySize.Packed256_Int16),
							new OpCodeHandler_EVEX_WkV(Register.ZMM0, Code.EVEX_Vmovdqu16_WZ_k1z_VZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int16)
						)
					)
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
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_kkHWIb(Register.XMM0, Code.EVEX_Vcmpps_VK_k1_HX_WX_Ib_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32),
							new OpCodeHandler_EVEX_kkHWIb(Register.YMM0, Code.EVEX_Vcmpps_VK_k1_HY_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32),
							new OpCodeHandler_EVEX_kkHWIb(Register.ZMM0, Code.EVEX_Vcmpps_VK_k1_HZ_WZ_Ib_sae_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32)
						),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_kkHWIb(Register.XMM0, Code.EVEX_Vcmppd_VK_k1_HX_WX_Ib_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64),
							new OpCodeHandler_EVEX_kkHWIb(Register.YMM0, Code.EVEX_Vcmppd_VK_k1_HY_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64),
							new OpCodeHandler_EVEX_kkHWIb(Register.ZMM0, Code.EVEX_Vcmppd_VK_k1_HZ_WZ_Ib_sae_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64)
						)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_EVEX_kkHWIb(Register.XMM0, Code.EVEX_Vcmpss_VK_k1_HX_WX_Ib_sae, TupleType.Tuple1_Scalar, MemorySize.Float32, MemorySize.Float32),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_EVEX_kkHWIb(Register.XMM0, Code.EVEX_Vcmpsd_VK_k1_HX_WX_Ib_sae, TupleType.Tuple1_Scalar, MemorySize.Float64, MemorySize.Float64)
					)
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_V_H_Ev_Ib(Register.XMM0, Code.EVEX_Vpinsrw_VX_HX_RdMw_Ib, Code.EVEX_Vpinsrw_VX_HX_RqMw_Ib, TupleType.Tuple1_Scalar_2, TupleType.Tuple1_Scalar_2, MemorySize.UInt16, MemorySize.UInt16),
							invalid,
							invalid
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_V_H_Ev_Ib(Register.XMM0, Code.EVEX_Vpinsrw_VX_HX_RdMw_Ib, Code.EVEX_Vpinsrw_VX_HX_RqMw_Ib, TupleType.Tuple1_Scalar_2, TupleType.Tuple1_Scalar_2, MemorySize.UInt16, MemorySize.UInt16),
							invalid,
							invalid
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_Ev_VX_Ib(Register.XMM0, Code.EVEX_Vpextrw_Gd_RX_Ib, Code.EVEX_Vpextrw_Gq_RX_Ib),
							invalid,
							invalid
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_Ev_VX_Ib(Register.XMM0, Code.EVEX_Vpextrw_Gd_RX_Ib, Code.EVEX_Vpextrw_Gq_RX_Ib),
							invalid,
							invalid
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHWIb(Register.XMM0, Code.EVEX_Vshufps_VX_k1z_HX_WX_Ib_b, TupleType.Full_128, MemorySize.Packed128_Float32, MemorySize.Broadcast128_Float32),
							new OpCodeHandler_EVEX_VkHWIb(Register.YMM0, Code.EVEX_Vshufps_VY_k1z_HY_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_Float32, MemorySize.Broadcast256_Float32),
							new OpCodeHandler_EVEX_VkHWIb(Register.ZMM0, Code.EVEX_Vshufps_VZ_k1z_HZ_WZ_Ib_b, TupleType.Full_512, MemorySize.Packed512_Float32, MemorySize.Broadcast512_Float32)
						),
						invalid
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHWIb(Register.XMM0, Code.EVEX_Vshufpd_VX_k1z_HX_WX_Ib_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64),
							new OpCodeHandler_EVEX_VkHWIb(Register.YMM0, Code.EVEX_Vshufpd_VY_k1z_HY_WY_Ib_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64),
							new OpCodeHandler_EVEX_VkHWIb(Register.ZMM0, Code.EVEX_Vshufpd_VZ_k1z_HZ_WZ_Ib_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64)
						)
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
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Register.XMM0, Register.XMM0, Code.EVEX_Vpsrlw_VX_k1z_HX_WX, TupleType.Mem128, MemorySize.Packed128_UInt64),
						new OpCodeHandler_EVEX_VkHW(Register.YMM0, Register.YMM0, Register.XMM0, Code.EVEX_Vpsrlw_VY_k1z_HY_WX, TupleType.Mem128, MemorySize.Packed128_UInt64),
						new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Register.ZMM0, Register.XMM0, Code.EVEX_Vpsrlw_VZ_k1z_HZ_WX, TupleType.Mem128, MemorySize.Packed128_UInt64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Register.XMM0, Register.XMM0, Code.EVEX_Vpsrld_VX_k1z_HX_WX, TupleType.Mem128, MemorySize.Packed128_UInt64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Register.YMM0, Register.XMM0, Code.EVEX_Vpsrld_VY_k1z_HY_WX, TupleType.Mem128, MemorySize.Packed128_UInt64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Register.ZMM0, Register.XMM0, Code.EVEX_Vpsrld_VZ_k1z_HZ_WX, TupleType.Mem128, MemorySize.Packed128_UInt64)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Register.XMM0, Register.XMM0, Code.EVEX_Vpsrlq_VX_k1z_HX_WX, TupleType.Mem128, MemorySize.Packed128_UInt64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Register.YMM0, Register.XMM0, Code.EVEX_Vpsrlq_VY_k1z_HY_WX, TupleType.Mem128, MemorySize.Packed128_UInt64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Register.ZMM0, Register.XMM0, Code.EVEX_Vpsrlq_VZ_k1z_HZ_WX, TupleType.Mem128, MemorySize.Packed128_UInt64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpaddq_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_UInt64, MemorySize.Broadcast128_UInt64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpaddq_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_UInt64, MemorySize.Broadcast256_UInt64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpaddq_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_UInt64, MemorySize.Broadcast512_UInt64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpmullw_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int16),
						new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpmullw_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int16),
						new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpmullw_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_WV(Register.XMM0, Code.EVEX_Vmovq_WX_VX, TupleType.Tuple1_Scalar, MemorySize.UInt64),
							invalid,
							invalid
						)
					),
					invalid,
					invalid
				),
				invalid,

				// D8
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpsubusb_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_UInt8),
						new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpsubusb_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_UInt8),
						new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpsubusb_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_UInt8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpsubusw_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_UInt16),
						new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpsubusw_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_UInt16),
						new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpsubusw_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_UInt16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpminub_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_UInt8),
						new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpminub_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_UInt8),
						new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpminub_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_UInt8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpandd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_UInt32, MemorySize.Broadcast128_UInt32),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpandd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_UInt32, MemorySize.Broadcast256_UInt32),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpandd_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_UInt32, MemorySize.Broadcast512_UInt32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpandq_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_UInt64, MemorySize.Broadcast128_UInt64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpandq_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_UInt64, MemorySize.Broadcast256_UInt64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpandq_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_UInt64, MemorySize.Broadcast512_UInt64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpaddusb_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_UInt8),
						new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpaddusb_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_UInt8),
						new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpaddusb_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_UInt8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpaddusw_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_UInt16),
						new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpaddusw_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_UInt16),
						new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpaddusw_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_UInt16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpmaxub_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_UInt8),
						new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpmaxub_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_UInt8),
						new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpmaxub_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_UInt8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpandnd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_UInt32, MemorySize.Broadcast128_UInt32),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpandnd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_UInt32, MemorySize.Broadcast256_UInt32),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpandnd_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_UInt32, MemorySize.Broadcast512_UInt32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpandnq_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_UInt64, MemorySize.Broadcast128_UInt64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpandnq_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_UInt64, MemorySize.Broadcast256_UInt64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpandnq_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_UInt64, MemorySize.Broadcast512_UInt64)
						)
					),
					invalid,
					invalid
				),

				// E0
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpavgb_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_UInt8),
						new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpavgb_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_UInt8),
						new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpavgb_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_UInt8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Register.XMM0, Register.XMM0, Code.EVEX_Vpsraw_VX_k1z_HX_WX, TupleType.Mem128, MemorySize.Packed128_UInt64),
						new OpCodeHandler_EVEX_VkHW(Register.YMM0, Register.YMM0, Register.XMM0, Code.EVEX_Vpsraw_VY_k1z_HY_WX, TupleType.Mem128, MemorySize.Packed128_UInt64),
						new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Register.ZMM0, Register.XMM0, Code.EVEX_Vpsraw_VZ_k1z_HZ_WX, TupleType.Mem128, MemorySize.Packed128_UInt64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Register.XMM0, Register.XMM0, Code.EVEX_Vpsrad_VX_k1z_HX_WX, TupleType.Mem128, MemorySize.Packed128_UInt64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Register.YMM0, Register.XMM0, Code.EVEX_Vpsrad_VY_k1z_HY_WX, TupleType.Mem128, MemorySize.Packed128_UInt64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Register.ZMM0, Register.XMM0, Code.EVEX_Vpsrad_VZ_k1z_HZ_WX, TupleType.Mem128, MemorySize.Packed128_UInt64)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Register.XMM0, Register.XMM0, Code.EVEX_Vpsraq_VX_k1z_HX_WX, TupleType.Mem128, MemorySize.Packed128_UInt64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Register.YMM0, Register.XMM0, Code.EVEX_Vpsraq_VY_k1z_HY_WX, TupleType.Mem128, MemorySize.Packed128_UInt64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Register.ZMM0, Register.XMM0, Code.EVEX_Vpsraq_VZ_k1z_HZ_WX, TupleType.Mem128, MemorySize.Packed128_UInt64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpavgw_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_UInt16),
						new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpavgw_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_UInt16),
						new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpavgw_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_UInt16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpmulhuw_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_UInt16),
						new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpmulhuw_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_UInt16),
						new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpmulhuw_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_UInt16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpmulhw_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int16),
						new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpmulhw_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int16),
						new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpmulhw_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkW_er(Register.XMM0, Register.XMM0, Code.EVEX_Vcvttpd2dq_VX_k1z_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: true),
							new OpCodeHandler_EVEX_VkW_er(Register.XMM0, Register.YMM0, Code.EVEX_Vcvttpd2dq_VX_k1z_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: true),
							new OpCodeHandler_EVEX_VkW_er(Register.YMM0, Register.ZMM0, Code.EVEX_Vcvttpd2dq_VY_k1z_WZ_sae_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: true)
						)
					),
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkW(Register.XMM0, Register.XMM0, Code.EVEX_Vcvtdq2pd_VX_k1z_WX_b, TupleType.Half_128, MemorySize.Packed64_Int32, MemorySize.Broadcast64_Int32),
							new OpCodeHandler_EVEX_VkW(Register.YMM0, Register.XMM0, Code.EVEX_Vcvtdq2pd_VY_k1z_WX_b, TupleType.Half_256, MemorySize.Packed128_Int32, MemorySize.Broadcast128_Int32),
							new OpCodeHandler_EVEX_VkW(Register.ZMM0, Register.YMM0, Code.EVEX_Vcvtdq2pd_VZ_k1z_WY_b, TupleType.Half_512, MemorySize.Packed256_Int32, MemorySize.Broadcast256_Int32)
						),
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkW_er(Register.XMM0, Register.XMM0, Code.EVEX_Vcvtqq2pd_VX_k1z_WX_b, TupleType.Full_128, MemorySize.Packed128_Int64, MemorySize.Broadcast128_Int64, onlySAE: false),
							new OpCodeHandler_EVEX_VkW_er(Register.YMM0, Register.YMM0, Code.EVEX_Vcvtqq2pd_VY_k1z_WY_b, TupleType.Full_256, MemorySize.Packed256_Int64, MemorySize.Broadcast256_Int64, onlySAE: false),
							new OpCodeHandler_EVEX_VkW_er(Register.ZMM0, Register.ZMM0, Code.EVEX_Vcvtqq2pd_VZ_k1z_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Int64, MemorySize.Broadcast512_Int64, onlySAE: false)
						)
					),
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX_er(
							new OpCodeHandler_EVEX_VkW_er(Register.XMM0, Register.XMM0, Code.EVEX_Vcvtpd2dq_VX_k1z_WX_b, TupleType.Full_128, MemorySize.Packed128_Float64, MemorySize.Broadcast128_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkW_er(Register.XMM0, Register.YMM0, Code.EVEX_Vcvtpd2dq_VX_k1z_WY_b, TupleType.Full_256, MemorySize.Packed256_Float64, MemorySize.Broadcast256_Float64, onlySAE: false),
							new OpCodeHandler_EVEX_VkW_er(Register.YMM0, Register.ZMM0, Code.EVEX_Vcvtpd2dq_VY_k1z_WZ_er_b, TupleType.Full_512, MemorySize.Packed512_Float64, MemorySize.Broadcast512_Float64, onlySAE: false)
						)
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_MV(Register.XMM0, Code.EVEX_Vmovntdq_M_VX, TupleType.Full_Mem_128, MemorySize.Packed128_Int32),
							new OpCodeHandler_EVEX_MV(Register.YMM0, Code.EVEX_Vmovntdq_M_VY, TupleType.Full_Mem_256, MemorySize.Packed256_Int32),
							new OpCodeHandler_EVEX_MV(Register.ZMM0, Code.EVEX_Vmovntdq_M_VZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int32)
						),
						invalid
					),
					invalid,
					invalid
				),

				// E8
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpsubsb_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int8),
						new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpsubsb_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int8),
						new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpsubsb_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpsubsw_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int16),
						new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpsubsw_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int16),
						new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpsubsw_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpminsw_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int16),
						new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpminsw_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int16),
						new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpminsw_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpord_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_UInt32, MemorySize.Broadcast128_UInt32),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpord_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_UInt32, MemorySize.Broadcast256_UInt32),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpord_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_UInt32, MemorySize.Broadcast512_UInt32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vporq_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_UInt64, MemorySize.Broadcast128_UInt64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vporq_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_UInt64, MemorySize.Broadcast256_UInt64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vporq_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_UInt64, MemorySize.Broadcast512_UInt64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpaddsb_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int8),
						new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpaddsb_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int8),
						new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpaddsb_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpaddsw_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int16),
						new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpaddsw_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int16),
						new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpaddsw_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpmaxsw_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int16),
						new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpmaxsw_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int16),
						new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpmaxsw_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpxord_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_UInt32, MemorySize.Broadcast128_UInt32),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpxord_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_UInt32, MemorySize.Broadcast256_UInt32),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpxord_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_UInt32, MemorySize.Broadcast512_UInt32)
						),
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpxorq_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_UInt64, MemorySize.Broadcast128_UInt64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpxorq_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_UInt64, MemorySize.Broadcast256_UInt64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpxorq_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_UInt64, MemorySize.Broadcast512_UInt64)
						)
					),
					invalid,
					invalid
				),

				// F0
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Register.XMM0, Register.XMM0, Code.EVEX_Vpsllw_VX_k1z_HX_WX, TupleType.Mem128, MemorySize.Packed128_UInt64),
						new OpCodeHandler_EVEX_VkHW(Register.YMM0, Register.YMM0, Register.XMM0, Code.EVEX_Vpsllw_VY_k1z_HY_WX, TupleType.Mem128, MemorySize.Packed128_UInt64),
						new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Register.ZMM0, Register.XMM0, Code.EVEX_Vpsllw_VZ_k1z_HZ_WX, TupleType.Mem128, MemorySize.Packed128_UInt64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Register.XMM0, Register.XMM0, Code.EVEX_Vpslld_VX_k1z_HX_WX, TupleType.Mem128, MemorySize.Packed128_UInt64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Register.YMM0, Register.XMM0, Code.EVEX_Vpslld_VY_k1z_HY_WX, TupleType.Mem128, MemorySize.Packed128_UInt64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Register.ZMM0, Register.XMM0, Code.EVEX_Vpslld_VZ_k1z_HZ_WX, TupleType.Mem128, MemorySize.Packed128_UInt64)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Register.XMM0, Register.XMM0, Code.EVEX_Vpsllq_VX_k1z_HX_WX, TupleType.Mem128, MemorySize.Packed128_UInt64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Register.YMM0, Register.XMM0, Code.EVEX_Vpsllq_VY_k1z_HY_WX, TupleType.Mem128, MemorySize.Packed128_UInt64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Register.ZMM0, Register.XMM0, Code.EVEX_Vpsllq_VZ_k1z_HZ_WX, TupleType.Mem128, MemorySize.Packed128_UInt64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpmuludq_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_UInt64, MemorySize.Broadcast128_UInt64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpmuludq_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_UInt64, MemorySize.Broadcast256_UInt64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpmuludq_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_UInt64, MemorySize.Broadcast512_UInt64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpmaddwd_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int16),
						new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpmaddwd_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int16),
						new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpmaddwd_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VHW(Register.XMM0, Code.EVEX_Vpsadbw_VX_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_UInt8),
						new OpCodeHandler_EVEX_VHW(Register.YMM0, Code.EVEX_Vpsadbw_VY_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_UInt8),
						new OpCodeHandler_EVEX_VHW(Register.ZMM0, Code.EVEX_Vpsadbw_VZ_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_UInt8)
					),
					invalid,
					invalid
				),
				invalid,

				// F8
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpsubb_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int8),
						new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpsubb_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int8),
						new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpsubb_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpsubw_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int16),
						new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpsubw_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int16),
						new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpsubw_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpsubd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Int32, MemorySize.Broadcast128_Int32),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpsubd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Int32, MemorySize.Broadcast256_Int32),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpsubd_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Int32, MemorySize.Broadcast512_Int32)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpsubq_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Int64, MemorySize.Broadcast128_Int64),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpsubq_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Int64, MemorySize.Broadcast256_Int64),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpsubq_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Int64, MemorySize.Broadcast512_Int64)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpaddb_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int8),
						new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpaddb_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int8),
						new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpaddb_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_EVEX(
						new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpaddw_VX_k1z_HX_WX, TupleType.Full_Mem_128, MemorySize.Packed128_Int16),
						new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpaddw_VY_k1z_HY_WY, TupleType.Full_Mem_256, MemorySize.Packed256_Int16),
						new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpaddw_VZ_k1z_HZ_WZ, TupleType.Full_Mem_512, MemorySize.Packed512_Int16)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_EVEX(
							new OpCodeHandler_EVEX_VkHW(Register.XMM0, Code.EVEX_Vpaddd_VX_k1z_HX_WX_b, TupleType.Full_128, MemorySize.Packed128_Int32, MemorySize.Broadcast128_Int32),
							new OpCodeHandler_EVEX_VkHW(Register.YMM0, Code.EVEX_Vpaddd_VY_k1z_HY_WY_b, TupleType.Full_256, MemorySize.Packed256_Int32, MemorySize.Broadcast256_Int32),
							new OpCodeHandler_EVEX_VkHW(Register.ZMM0, Code.EVEX_Vpaddd_VZ_k1z_HZ_WZ_b, TupleType.Full_512, MemorySize.Packed512_Int32, MemorySize.Broadcast512_Int32)
						),
						invalid
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
