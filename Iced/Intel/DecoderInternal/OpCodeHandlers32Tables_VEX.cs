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
						new OpCodeHandler_VEX_HRIb(Register.XMM0, Code.VEX_Vpsrlw_xmm_xmm_imm8),
						new OpCodeHandler_VEX_HRIb(Register.YMM0, Code.VEX_Vpsrlw_ymm_ymm_imm8)
					),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_HRIb(Register.XMM0, Code.VEX_Vpsraw_xmm_xmm_imm8),
						new OpCodeHandler_VEX_HRIb(Register.YMM0, Code.VEX_Vpsraw_ymm_ymm_imm8)
					),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_HRIb(Register.XMM0, Code.VEX_Vpsllw_xmm_xmm_imm8),
						new OpCodeHandler_VEX_HRIb(Register.YMM0, Code.VEX_Vpsllw_ymm_ymm_imm8)
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
						new OpCodeHandler_VEX_HRIb(Register.XMM0, Code.VEX_Vpsrld_xmm_xmm_imm8),
						new OpCodeHandler_VEX_HRIb(Register.YMM0, Code.VEX_Vpsrld_ymm_ymm_imm8)
					),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_HRIb(Register.XMM0, Code.VEX_Vpsrad_xmm_xmm_imm8),
						new OpCodeHandler_VEX_HRIb(Register.YMM0, Code.VEX_Vpsrad_ymm_ymm_imm8)
					),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_HRIb(Register.XMM0, Code.VEX_Vpslld_xmm_xmm_imm8),
						new OpCodeHandler_VEX_HRIb(Register.YMM0, Code.VEX_Vpslld_ymm_ymm_imm8)
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
						new OpCodeHandler_VEX_HRIb(Register.XMM0, Code.VEX_Vpsrlq_xmm_xmm_imm8),
						new OpCodeHandler_VEX_HRIb(Register.YMM0, Code.VEX_Vpsrlq_ymm_ymm_imm8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_HRIb(Register.XMM0, Code.VEX_Vpsrldq_xmm_xmm_imm8),
						new OpCodeHandler_VEX_HRIb(Register.YMM0, Code.VEX_Vpsrldq_ymm_ymm_imm8)
					),
					invalid,
					invalid
				),
				invalid,
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_HRIb(Register.XMM0, Code.VEX_Vpsllq_xmm_xmm_imm8),
						new OpCodeHandler_VEX_HRIb(Register.YMM0, Code.VEX_Vpsllq_ymm_ymm_imm8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_HRIb(Register.XMM0, Code.VEX_Vpslldq_xmm_xmm_imm8),
						new OpCodeHandler_VEX_HRIb(Register.YMM0, Code.VEX_Vpslldq_ymm_ymm_imm8)
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
						new OpCodeHandler_VEX_M(Code.VEX_Vldmxcsr_m32),
						invalid
					),
					invalid,
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_M(Code.VEX_Vstmxcsr_m32),
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
						new OpCodeHandler_VEX_Hv_Ev(Code.VEX_Blsr_r32_rm32, Code.VEX_Blsr_r64_rm64),
						invalid
					),
					invalid,
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Hv_Ev(Code.VEX_Blsmsk_r32_rm32, Code.VEX_Blsmsk_r64_rm64),
						invalid
					),
					invalid,
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Hv_Ev(Code.VEX_Blsi_r32_rm32, Code.VEX_Blsi_r64_rm64),
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
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpshufb_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpshufb_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vphaddw_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vphaddw_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vphaddd_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vphaddd_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vphaddsw_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vphaddsw_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpmaddubsw_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpmaddubsw_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vphsubw_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vphsubw_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vphsubd_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vphsubd_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vphsubsw_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vphsubsw_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),

				// 08
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpsignb_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpsignb_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpsignw_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpsignw_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpsignd_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpsignd_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpmulhrsw_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpmulhrsw_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpermilps_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpermilps_ymm_ymm_ymmm256)
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
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpermilpd_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpermilpd_ymm_ymm_ymmm256)
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
							new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vtestps_xmm_xmmm128),
							new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vtestps_ymm_ymmm256)
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
							new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vtestpd_xmm_xmmm128),
							new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vtestpd_ymm_ymmm256)
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
							new OpCodeHandler_VEX_VW(Register.XMM0, Register.XMM0, Code.VEX_Vcvtph2ps_xmm_xmmm64),
							new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vcvtph2ps_ymm_xmmm128)
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
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpermps_ymm_ymm_ymmm256)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vptest_xmm_xmmm128),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vptest_ymm_ymmm256)
					),
					invalid,
					invalid
				),

				// 18
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VW(Register.XMM0, Register.XMM0, Code.VEX_Vbroadcastss_xmm_xmmm32),
							new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vbroadcastss_ymm_xmmm32)
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
							new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vbroadcastsd_ymm_xmmm64)
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
							new OpCodeHandler_VEX_VM(Register.YMM0, Code.VEX_Vbroadcastf128_ymm_m128)
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
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vpabsb_xmm_xmmm128),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vpabsb_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vpabsw_xmm_xmmm128),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vpabsw_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vpabsd_xmm_xmmm128),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vpabsd_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				invalid,

				// 20
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Register.XMM0, Code.VEX_Vpmovsxbw_xmm_xmmm64),
						new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vpmovsxbw_ymm_xmmm128)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Register.XMM0, Code.VEX_Vpmovsxbd_xmm_xmmm32),
						new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vpmovsxbd_ymm_xmmm64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Register.XMM0, Code.VEX_Vpmovsxbq_xmm_xmmm16),
						new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vpmovsxbq_ymm_xmmm32)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Register.XMM0, Code.VEX_Vpmovsxwd_xmm_xmmm64),
						new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vpmovsxwd_ymm_xmmm128)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Register.XMM0, Code.VEX_Vpmovsxwq_xmm_xmmm32),
						new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vpmovsxwq_ymm_xmmm64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Register.XMM0, Code.VEX_Vpmovsxdq_xmm_xmmm64),
						new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vpmovsxdq_ymm_xmmm128)
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
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpmuldq_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpmuldq_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpcmpeqq_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpcmpeqq_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VM(Register.XMM0, Code.VEX_Vmovntdqa_xmm_m128),
						new OpCodeHandler_VEX_VM(Register.YMM0, Code.VEX_Vmovntdqa_ymm_m256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpackusdw_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpackusdw_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHM(Register.XMM0, Code.VEX_Vmaskmovps_xmm_xmm_m128),
							new OpCodeHandler_VEX_VHM(Register.YMM0, Code.VEX_Vmaskmovps_ymm_ymm_m256)
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
							new OpCodeHandler_VEX_VHM(Register.XMM0, Code.VEX_Vmaskmovpd_xmm_xmm_m128),
							new OpCodeHandler_VEX_VHM(Register.YMM0, Code.VEX_Vmaskmovpd_ymm_ymm_m256)
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
							new OpCodeHandler_VEX_MHV(Register.XMM0, Code.VEX_Vmaskmovps_m128_xmm_xmm),
							new OpCodeHandler_VEX_MHV(Register.YMM0, Code.VEX_Vmaskmovps_m256_ymm_ymm)
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
							new OpCodeHandler_VEX_MHV(Register.XMM0, Code.VEX_Vmaskmovpd_m128_xmm_xmm),
							new OpCodeHandler_VEX_MHV(Register.YMM0, Code.VEX_Vmaskmovpd_m256_ymm_ymm)
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
						new OpCodeHandler_VEX_VW(Register.XMM0, Register.XMM0, Code.VEX_Vpmovzxbw_xmm_xmmm64),
						new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vpmovzxbw_ymm_xmmm128)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Register.XMM0, Code.VEX_Vpmovzxbd_xmm_xmmm32),
						new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vpmovzxbd_ymm_xmmm64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Register.XMM0, Code.VEX_Vpmovzxbq_xmm_xmmm16),
						new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vpmovzxbq_ymm_xmmm32)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Register.XMM0, Code.VEX_Vpmovzxwd_xmm_xmmm64),
						new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vpmovzxwd_ymm_xmmm128)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Register.XMM0, Code.VEX_Vpmovzxwq_xmm_xmmm32),
						new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vpmovzxwq_ymm_xmmm64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Register.XMM0, Code.VEX_Vpmovzxdq_xmm_xmmm64),
						new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vpmovzxdq_ymm_xmmm128)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							invalid,
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpermd_ymm_ymm_ymmm256)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpcmpgtq_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpcmpgtq_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),

				// 38
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpminsb_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpminsb_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpminsd_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpminsd_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpminuw_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpminuw_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpminud_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpminud_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpmaxsb_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpmaxsb_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpmaxsd_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpmaxsd_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpmaxuw_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpmaxuw_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpmaxud_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpmaxud_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),

				// 40
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpmulld_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpmulld_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vphminposuw_xmm_xmmm128),
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
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpsrlvd_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpsrlvd_ymm_ymm_ymmm256)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpsrlvq_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpsrlvq_ymm_ymm_ymmm256)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpsravd_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpsravd_ymm_ymm_ymmm256)
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
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpsllvd_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpsllvd_ymm_ymm_ymmm256)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpsllvq_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpsllvq_ymm_ymm_ymmm256)
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
							new OpCodeHandler_VEX_VW(Register.XMM0, Register.XMM0, Code.VEX_Vpbroadcastd_xmm_xmmm32),
							new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vpbroadcastd_ymm_xmmm32)
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
							new OpCodeHandler_VEX_VW(Register.XMM0, Register.XMM0, Code.VEX_Vpbroadcastq_xmm_xmmm64),
							new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vpbroadcastq_ymm_xmmm64)
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
							new OpCodeHandler_VEX_VM(Register.YMM0, Code.VEX_Vbroadcasti128_ymm_m128)
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
							new OpCodeHandler_VEX_VW(Register.XMM0, Register.XMM0, Code.VEX_Vpbroadcastb_xmm_xmmm8),
							new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vpbroadcastb_ymm_xmmm8)
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
							new OpCodeHandler_VEX_VW(Register.XMM0, Register.XMM0, Code.VEX_Vpbroadcastw_xmm_xmmm16),
							new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vpbroadcastw_ymm_xmmm16)
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
							new OpCodeHandler_VEX_VHM(Register.XMM0, Code.VEX_Vpmaskmovd_xmm_xmm_m128),
							new OpCodeHandler_VEX_VHM(Register.YMM0, Code.VEX_Vpmaskmovd_ymm_ymm_m256)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHM(Register.XMM0, Code.VEX_Vpmaskmovq_xmm_xmm_m128),
							new OpCodeHandler_VEX_VHM(Register.YMM0, Code.VEX_Vpmaskmovq_ymm_ymm_m256)
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
							new OpCodeHandler_VEX_MHV(Register.XMM0, Code.VEX_Vpmaskmovd_m128_xmm_xmm),
							new OpCodeHandler_VEX_MHV(Register.YMM0, Code.VEX_Vpmaskmovd_m256_ymm_ymm)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_MHV(Register.XMM0, Code.VEX_Vpmaskmovq_m128_xmm_xmm),
							new OpCodeHandler_VEX_MHV(Register.YMM0, Code.VEX_Vpmaskmovq_m256_ymm_ymm)
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
							new OpCodeHandler_VEX_VX_VSIB_HX(Register.XMM0, Register.XMM0, Register.XMM0, Code.VEX_Vpgatherdd_xmm_vm32x_xmm),
							new OpCodeHandler_VEX_VX_VSIB_HX(Register.YMM0, Register.YMM0, Register.YMM0, Code.VEX_Vpgatherdd_ymm_vm32y_ymm)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VX_VSIB_HX(Register.XMM0, Register.XMM0, Register.XMM0, Code.VEX_Vpgatherdq_xmm_vm32x_xmm),
							new OpCodeHandler_VEX_VX_VSIB_HX(Register.YMM0, Register.XMM0, Register.YMM0, Code.VEX_Vpgatherdq_ymm_vm32x_ymm)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VX_VSIB_HX(Register.XMM0, Register.XMM0, Register.XMM0, Code.VEX_Vpgatherqd_xmm_vm64x_xmm),
							new OpCodeHandler_VEX_VX_VSIB_HX(Register.XMM0, Register.YMM0, Register.XMM0, Code.VEX_Vpgatherqd_xmm_vm64y_xmm)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VX_VSIB_HX(Register.XMM0, Register.XMM0, Register.XMM0, Code.VEX_Vpgatherqq_xmm_vm64x_xmm),
							new OpCodeHandler_VEX_VX_VSIB_HX(Register.YMM0, Register.YMM0, Register.YMM0, Code.VEX_Vpgatherqq_ymm_vm64y_ymm)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VX_VSIB_HX(Register.XMM0, Register.XMM0, Register.XMM0, Code.VEX_Vgatherdps_xmm_vm32x_xmm),
							new OpCodeHandler_VEX_VX_VSIB_HX(Register.YMM0, Register.YMM0, Register.YMM0, Code.VEX_Vgatherdps_ymm_vm32y_ymm)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VX_VSIB_HX(Register.XMM0, Register.XMM0, Register.XMM0, Code.VEX_Vgatherdpd_xmm_vm32x_xmm),
							new OpCodeHandler_VEX_VX_VSIB_HX(Register.YMM0, Register.XMM0, Register.YMM0, Code.VEX_Vgatherdpd_ymm_vm32x_ymm)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VX_VSIB_HX(Register.XMM0, Register.XMM0, Register.XMM0, Code.VEX_Vgatherqps_xmm_vm64x_xmm),
							new OpCodeHandler_VEX_VX_VSIB_HX(Register.XMM0, Register.YMM0, Register.XMM0, Code.VEX_Vgatherqps_xmm_vm64y_xmm)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VX_VSIB_HX(Register.XMM0, Register.XMM0, Register.XMM0, Code.VEX_Vgatherqpd_xmm_vm64x_xmm),
							new OpCodeHandler_VEX_VX_VSIB_HX(Register.YMM0, Register.YMM0, Register.YMM0, Code.VEX_Vgatherqpd_ymm_vm64y_ymm)
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
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmaddsub132ps_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmaddsub132ps_ymm_ymm_ymmm256)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmaddsub132pd_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmaddsub132pd_ymm_ymm_ymmm256)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmsubadd132ps_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmsubadd132ps_ymm_ymm_ymmm256)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmsubadd132pd_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmsubadd132pd_ymm_ymm_ymmm256)
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
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmadd132ps_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmadd132ps_ymm_ymm_ymmm256)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmadd132pd_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmadd132pd_ymm_ymm_ymmm256)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmadd132ss_xmm_xmm_xmmm32),
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmadd132sd_xmm_xmm_xmmm64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmsub132ps_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmsub132ps_ymm_ymm_ymmm256)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmsub132pd_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmsub132pd_ymm_ymm_ymmm256)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmsub132ss_xmm_xmm_xmmm32),
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmsub132sd_xmm_xmm_xmmm64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmadd132ps_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfnmadd132ps_ymm_ymm_ymmm256)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmadd132pd_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfnmadd132pd_ymm_ymm_ymmm256)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmadd132ss_xmm_xmm_xmmm32),
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmadd132sd_xmm_xmm_xmmm64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmsub132ps_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfnmsub132ps_ymm_ymm_ymmm256)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmsub132pd_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfnmsub132pd_ymm_ymm_ymmm256)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmsub132ss_xmm_xmm_xmmm32),
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmsub132sd_xmm_xmm_xmmm64)
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
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmaddsub213ps_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmaddsub213ps_ymm_ymm_ymmm256)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmaddsub213pd_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmaddsub213pd_ymm_ymm_ymmm256)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmsubadd213ps_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmsubadd213ps_ymm_ymm_ymmm256)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmsubadd213pd_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmsubadd213pd_ymm_ymm_ymmm256)
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
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmadd213ps_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmadd213ps_ymm_ymm_ymmm256)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmadd213pd_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmadd213pd_ymm_ymm_ymmm256)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmadd213ss_xmm_xmm_xmmm32),
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmadd213sd_xmm_xmm_xmmm64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmsub213ps_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmsub213ps_ymm_ymm_ymmm256)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmsub213pd_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmsub213pd_ymm_ymm_ymmm256)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmsub213ss_xmm_xmm_xmmm32),
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmsub213sd_xmm_xmm_xmmm64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmadd213ps_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfnmadd213ps_ymm_ymm_ymmm256)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmadd213pd_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfnmadd213pd_ymm_ymm_ymmm256)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmadd213ss_xmm_xmm_xmmm32),
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmadd213sd_xmm_xmm_xmmm64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmsub213ps_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfnmsub213ps_ymm_ymm_ymmm256)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmsub213pd_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfnmsub213pd_ymm_ymm_ymmm256)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmsub213ss_xmm_xmm_xmmm32),
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmsub213sd_xmm_xmm_xmmm64)
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
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmaddsub231ps_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmaddsub231ps_ymm_ymm_ymmm256)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmaddsub231pd_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmaddsub231pd_ymm_ymm_ymmm256)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmsubadd231ps_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmsubadd231ps_ymm_ymm_ymmm256)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmsubadd231pd_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmsubadd231pd_ymm_ymm_ymmm256)
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
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmadd231ps_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmadd231ps_ymm_ymm_ymmm256)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmadd231pd_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmadd231pd_ymm_ymm_ymmm256)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmadd231ss_xmm_xmm_xmmm32),
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmadd231sd_xmm_xmm_xmmm64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmsub231ps_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmsub231ps_ymm_ymm_ymmm256)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmsub231pd_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfmsub231pd_ymm_ymm_ymmm256)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmsub231ss_xmm_xmm_xmmm32),
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfmsub231sd_xmm_xmm_xmmm64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmadd231ps_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfnmadd231ps_ymm_ymm_ymmm256)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmadd231pd_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfnmadd231pd_ymm_ymm_ymmm256)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmadd231ss_xmm_xmm_xmmm32),
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmadd231sd_xmm_xmm_xmmm64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmsub231ps_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfnmsub231ps_ymm_ymm_ymmm256)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmsub231pd_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vfnmsub231pd_ymm_ymm_ymmm256)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmsub231ss_xmm_xmm_xmmm32),
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vfnmsub231sd_xmm_xmm_xmmm64)
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
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vgf2p8mulb_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vgf2p8mulb_ymm_ymm_ymmm256)
						),
						invalid
					),
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
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vaesimc_xmm_xmmm128),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vaesenc_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vaesenc_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vaesenclast_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vaesenclast_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vaesdec_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vaesdec_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vaesdeclast_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vaesdeclast_ymm_ymm_ymmm256)
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
						new OpCodeHandler_VEX_Gv_Gv_Ev(Code.VEX_Andn_r32_r32_rm32, Code.VEX_Andn_r64_r64_rm64),
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
						new OpCodeHandler_VEX_Gv_Ev_Gv(Code.VEX_Bzhi_r32_rm32_r32, Code.VEX_Bzhi_r64_rm64_r64),
						invalid
					),
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Gv_Gv_Ev(Code.VEX_Pext_r32_r32_rm32, Code.VEX_Pext_r64_r64_rm64),
						invalid
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Gv_Gv_Ev(Code.VEX_Pdep_r32_r32_rm32, Code.VEX_Pdep_r64_r64_rm64),
						invalid
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					invalid,
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Gv_Gv_Ev(Code.VEX_Mulx_r32_r32_rm32, Code.VEX_Mulx_r64_r64_rm64),
						invalid
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Gv_Ev_Gv(Code.VEX_Bextr_r32_rm32_r32, Code.VEX_Bextr_r64_rm64_r64),
						invalid
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Gv_Ev_Gv(Code.VEX_Shlx_r32_rm32_r32, Code.VEX_Shlx_r64_rm64_r64),
						invalid
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Gv_Ev_Gv(Code.VEX_Sarx_r32_rm32_r32, Code.VEX_Sarx_r64_rm64_r64),
						invalid
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Gv_Ev_Gv(Code.VEX_Shrx_r32_rm32_r32, Code.VEX_Shrx_r64_rm64_r64),
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
							new OpCodeHandler_VEX_VWIb(Register.YMM0, Code.VEX_Vpermq_ymm_ymmm256_imm8)
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
							new OpCodeHandler_VEX_VWIb(Register.YMM0, Code.VEX_Vpermpd_ymm_ymmm256_imm8)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.VEX_Vpblendd_xmm_xmm_xmmm128_imm8),
							new OpCodeHandler_VEX_VHWIb(Register.YMM0, Code.VEX_Vpblendd_ymm_ymm_ymmm256_imm8)
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
							new OpCodeHandler_VEX_VWIb(Register.XMM0, Code.VEX_Vpermilps_xmm_xmmm128_imm8),
							new OpCodeHandler_VEX_VWIb(Register.YMM0, Code.VEX_Vpermilps_ymm_ymmm256_imm8)
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
							new OpCodeHandler_VEX_VWIb(Register.XMM0, Code.VEX_Vpermilpd_xmm_xmmm128_imm8),
							new OpCodeHandler_VEX_VWIb(Register.YMM0, Code.VEX_Vpermilpd_ymm_ymmm256_imm8)
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
							new OpCodeHandler_VEX_VHWIb(Register.YMM0, Code.VEX_Vperm2f128_ymm_ymm_ymmm256_imm8)
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
						new OpCodeHandler_VEX_VWIb(Register.XMM0, Code.VEX_Vroundps_xmm_xmmm128_imm8),
						new OpCodeHandler_VEX_VWIb(Register.YMM0, Code.VEX_Vroundps_ymm_ymmm256_imm8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VWIb(Register.XMM0, Code.VEX_Vroundpd_xmm_xmmm128_imm8),
						new OpCodeHandler_VEX_VWIb(Register.YMM0, Code.VEX_Vroundpd_ymm_ymmm256_imm8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.VEX_Vroundss_xmm_xmm_xmmm32_imm8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.VEX_Vroundsd_xmm_xmm_xmmm64_imm8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.VEX_Vblendps_xmm_xmm_xmmm128_imm8),
						new OpCodeHandler_VEX_VHWIb(Register.YMM0, Code.VEX_Vblendps_ymm_ymm_ymmm256_imm8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.VEX_Vblendpd_xmm_xmm_xmmm128_imm8),
						new OpCodeHandler_VEX_VHWIb(Register.YMM0, Code.VEX_Vblendpd_ymm_ymm_ymmm256_imm8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.VEX_Vpblendw_xmm_xmm_xmmm128_imm8),
						new OpCodeHandler_VEX_VHWIb(Register.YMM0, Code.VEX_Vpblendw_ymm_ymm_ymmm256_imm8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.VEX_Vpalignr_xmm_xmm_xmmm128_imm8),
						new OpCodeHandler_VEX_VHWIb(Register.YMM0, Code.VEX_Vpalignr_ymm_ymm_ymmm256_imm8)
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
						new OpCodeHandler_VEX_GvM_VX_Ib(Register.XMM0, Code.VEX_Vpextrb_r32m8_xmm_imm8, Code.VEX_Vpextrb_r64m8_xmm_imm8),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_GvM_VX_Ib(Register.XMM0, Code.VEX_Vpextrw_r32m16_xmm_imm8, Code.VEX_Vpextrw_r64m16_xmm_imm8),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_GvM_VX_Ib(Register.XMM0, Code.VEX_Vpextrd_rm32_xmm_imm8, Code.VEX_Vpextrq_rm64_xmm_imm8),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Ed_V_Ib(Register.XMM0, Code.VEX_Vextractps_rm32_xmm_imm8, Code.VEX_Vextractps_rm64_xmm_imm8),
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
							new OpCodeHandler_VEX_VHWIb(Register.YMM0, Register.YMM0, Register.XMM0, Code.VEX_Vinsertf128_ymm_ymm_xmmm128_imm8)
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
							new OpCodeHandler_VEX_WVIb(Register.XMM0, Register.YMM0, Code.VEX_Vextractf128_xmmm128_ymm_imm8)
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
							new OpCodeHandler_VEX_WVIb(Register.XMM0, Register.XMM0, Code.VEX_Vcvtps2ph_xmmm64_xmm_imm8),
							new OpCodeHandler_VEX_WVIb(Register.XMM0, Register.YMM0, Code.VEX_Vcvtps2ph_xmmm128_ymm_imm8)
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
						new OpCodeHandler_VEX_VHEvIb(Register.XMM0, Code.VEX_Vpinsrb_xmm_xmm_r32m8_imm8, Code.VEX_Vpinsrb_xmm_xmm_r64m8_imm8),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.VEX_Vinsertps_xmm_xmm_xmmm32_imm8),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHEvIb(Register.XMM0, Code.VEX_Vpinsrd_xmm_xmm_rm32_imm8, Code.VEX_Vpinsrq_xmm_xmm_rm64_imm8),
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
							new OpCodeHandler_VEX_VK_RK_Ib(Code.VEX_Kshiftrb_k_k_imm8),
							new OpCodeHandler_VEX_VK_RK_Ib(Code.VEX_Kshiftrw_k_k_imm8)
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
							new OpCodeHandler_VEX_VK_RK_Ib(Code.VEX_Kshiftrd_k_k_imm8),
							new OpCodeHandler_VEX_VK_RK_Ib(Code.VEX_Kshiftrq_k_k_imm8)
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
							new OpCodeHandler_VEX_VK_RK_Ib(Code.VEX_Kshiftlb_k_k_imm8),
							new OpCodeHandler_VEX_VK_RK_Ib(Code.VEX_Kshiftlw_k_k_imm8)
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
							new OpCodeHandler_VEX_VK_RK_Ib(Code.VEX_Kshiftld_k_k_imm8),
							new OpCodeHandler_VEX_VK_RK_Ib(Code.VEX_Kshiftlq_k_k_imm8)
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
							new OpCodeHandler_VEX_VHWIb(Register.YMM0, Register.YMM0, Register.XMM0, Code.VEX_Vinserti128_ymm_ymm_xmmm128_imm8)
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
							new OpCodeHandler_VEX_WVIb(Register.XMM0, Register.YMM0, Code.VEX_Vextracti128_xmmm128_ymm_imm8)
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
						new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.VEX_Vdpps_xmm_xmm_xmmm128_imm8),
						new OpCodeHandler_VEX_VHWIb(Register.YMM0, Code.VEX_Vdpps_ymm_ymm_ymmm256_imm8)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.VEX_Vdppd_xmm_xmm_xmmm128_imm8),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.VEX_Vmpsadbw_xmm_xmm_xmmm128_imm8),
						new OpCodeHandler_VEX_VHWIb(Register.YMM0, Code.VEX_Vmpsadbw_ymm_ymm_ymmm256_imm8)
					),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.VEX_Vpclmulqdq_xmm_xmm_xmmm128_imm8),
						new OpCodeHandler_VEX_VHWIb(Register.YMM0, Code.VEX_Vpclmulqdq_ymm_ymm_ymmm256_imm8)
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
							new OpCodeHandler_VEX_VHWIb(Register.YMM0, Code.VEX_Vperm2i128_ymm_ymm_ymmm256_imm8)
						),
						invalid
					),
					invalid,
					invalid
				),
				invalid,

				// 48
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHWIs5(Register.XMM0, Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm8),
							new OpCodeHandler_VEX_VHWIs5(Register.YMM0, Code.VEX_Vpermil2ps_ymm_ymm_ymmm256_ymm_imm8)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHIs5W(Register.XMM0, Code.VEX_Vpermil2ps_xmm_xmm_xmm_xmmm128_imm8),
							new OpCodeHandler_VEX_VHIs5W(Register.YMM0, Code.VEX_Vpermil2ps_ymm_ymm_ymm_ymmm256_imm8)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHWIs5(Register.XMM0, Code.VEX_Vpermil2pd_xmm_xmm_xmmm128_xmm_imm8),
							new OpCodeHandler_VEX_VHWIs5(Register.YMM0, Code.VEX_Vpermil2pd_ymm_ymm_ymmm256_ymm_imm8)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHIs5W(Register.XMM0, Code.VEX_Vpermil2pd_xmm_xmm_xmm_xmmm128_imm8),
							new OpCodeHandler_VEX_VHIs5W(Register.YMM0, Code.VEX_Vpermil2pd_ymm_ymm_ymm_ymmm256_imm8)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.VEX_Vblendvps_xmm_xmm_xmmm128_xmm),
							new OpCodeHandler_VEX_VHWIs4(Register.YMM0, Code.VEX_Vblendvps_ymm_ymm_ymmm256_ymm)
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
							new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.VEX_Vblendvpd_xmm_xmm_xmmm128_xmm),
							new OpCodeHandler_VEX_VHWIs4(Register.YMM0, Code.VEX_Vblendvpd_ymm_ymm_ymmm256_ymm)
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
							new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.VEX_Vpblendvb_xmm_xmm_xmmm128_xmm),
							new OpCodeHandler_VEX_VHWIs4(Register.YMM0, Code.VEX_Vpblendvb_ymm_ymm_ymmm256_ymm)
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
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.VEX_Vfmaddsubps_xmm_xmm_xmmm128_xmm),
							new OpCodeHandler_VEX_VHWIs4(Register.YMM0, Code.VEX_Vfmaddsubps_ymm_ymm_ymmm256_ymm)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHIs4W(Register.XMM0, Code.VEX_Vfmaddsubps_xmm_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHIs4W(Register.YMM0, Code.VEX_Vfmaddsubps_ymm_ymm_ymm_ymmm256)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.VEX_Vfmaddsubpd_xmm_xmm_xmmm128_xmm),
							new OpCodeHandler_VEX_VHWIs4(Register.YMM0, Code.VEX_Vfmaddsubpd_ymm_ymm_ymmm256_ymm)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHIs4W(Register.XMM0, Code.VEX_Vfmaddsubpd_xmm_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHIs4W(Register.YMM0, Code.VEX_Vfmaddsubpd_ymm_ymm_ymm_ymmm256)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.VEX_Vfmsubaddps_xmm_xmm_xmmm128_xmm),
							new OpCodeHandler_VEX_VHWIs4(Register.YMM0, Code.VEX_Vfmsubaddps_ymm_ymm_ymmm256_ymm)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHIs4W(Register.XMM0, Code.VEX_Vfmsubaddps_xmm_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHIs4W(Register.YMM0, Code.VEX_Vfmsubaddps_ymm_ymm_ymm_ymmm256)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.VEX_Vfmsubaddpd_xmm_xmm_xmmm128_xmm),
							new OpCodeHandler_VEX_VHWIs4(Register.YMM0, Code.VEX_Vfmsubaddpd_ymm_ymm_ymmm256_ymm)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHIs4W(Register.XMM0, Code.VEX_Vfmsubaddpd_xmm_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHIs4W(Register.YMM0, Code.VEX_Vfmsubaddpd_ymm_ymm_ymm_ymmm256)
						)
					),
					invalid,
					invalid
				),

				// 60
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VWIb(Register.XMM0, Code.VEX_Vpcmpestrm_xmm_xmmm128_imm8, Code.VEX_Vpcmpestrm64_xmm_xmmm128_imm8),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VWIb(Register.XMM0, Code.VEX_Vpcmpestri_xmm_xmmm128_imm8, Code.VEX_Vpcmpestri64_xmm_xmmm128_imm8),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VWIb(Register.XMM0, Code.VEX_Vpcmpistrm_xmm_xmmm128_imm8),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VWIb(Register.XMM0, Code.VEX_Vpcmpistri_xmm_xmmm128_imm8),
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
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.VEX_Vfmaddps_xmm_xmm_xmmm128_xmm),
							new OpCodeHandler_VEX_VHWIs4(Register.YMM0, Code.VEX_Vfmaddps_ymm_ymm_ymmm256_ymm)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHIs4W(Register.XMM0, Code.VEX_Vfmaddps_xmm_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHIs4W(Register.YMM0, Code.VEX_Vfmaddps_ymm_ymm_ymm_ymmm256)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.VEX_Vfmaddpd_xmm_xmm_xmmm128_xmm),
							new OpCodeHandler_VEX_VHWIs4(Register.YMM0, Code.VEX_Vfmaddpd_ymm_ymm_ymmm256_ymm)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHIs4W(Register.XMM0, Code.VEX_Vfmaddpd_xmm_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHIs4W(Register.YMM0, Code.VEX_Vfmaddpd_ymm_ymm_ymm_ymmm256)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.VEX_Vfmaddss_xmm_xmm_xmmm32_xmm),
						new OpCodeHandler_VEX_VHIs4W(Register.XMM0, Code.VEX_Vfmaddss_xmm_xmm_xmm_xmmm32)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.VEX_Vfmaddsd_xmm_xmm_xmmm64_xmm),
						new OpCodeHandler_VEX_VHIs4W(Register.XMM0, Code.VEX_Vfmaddsd_xmm_xmm_xmm_xmmm64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.VEX_Vfmsubps_xmm_xmm_xmmm128_xmm),
							new OpCodeHandler_VEX_VHWIs4(Register.YMM0, Code.VEX_Vfmsubps_ymm_ymm_ymmm256_ymm)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHIs4W(Register.XMM0, Code.VEX_Vfmsubps_xmm_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHIs4W(Register.YMM0, Code.VEX_Vfmsubps_ymm_ymm_ymm_ymmm256)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.VEX_Vfmsubpd_xmm_xmm_xmmm128_xmm),
							new OpCodeHandler_VEX_VHWIs4(Register.YMM0, Code.VEX_Vfmsubpd_ymm_ymm_ymmm256_ymm)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHIs4W(Register.XMM0, Code.VEX_Vfmsubpd_xmm_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHIs4W(Register.YMM0, Code.VEX_Vfmsubpd_ymm_ymm_ymm_ymmm256)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.VEX_Vfmsubss_xmm_xmm_xmmm32_xmm),
						new OpCodeHandler_VEX_VHIs4W(Register.XMM0, Code.VEX_Vfmsubss_xmm_xmm_xmm_xmmm32)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.VEX_Vfmsubsd_xmm_xmm_xmmm64_xmm),
						new OpCodeHandler_VEX_VHIs4W(Register.XMM0, Code.VEX_Vfmsubsd_xmm_xmm_xmm_xmmm64)
					),
					invalid,
					invalid
				),

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
							new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.VEX_Vfnmaddps_xmm_xmm_xmmm128_xmm),
							new OpCodeHandler_VEX_VHWIs4(Register.YMM0, Code.VEX_Vfnmaddps_ymm_ymm_ymmm256_ymm)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHIs4W(Register.XMM0, Code.VEX_Vfnmaddps_xmm_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHIs4W(Register.YMM0, Code.VEX_Vfnmaddps_ymm_ymm_ymm_ymmm256)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.VEX_Vfnmaddpd_xmm_xmm_xmmm128_xmm),
							new OpCodeHandler_VEX_VHWIs4(Register.YMM0, Code.VEX_Vfnmaddpd_ymm_ymm_ymmm256_ymm)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHIs4W(Register.XMM0, Code.VEX_Vfnmaddpd_xmm_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHIs4W(Register.YMM0, Code.VEX_Vfnmaddpd_ymm_ymm_ymm_ymmm256)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.VEX_Vfnmaddss_xmm_xmm_xmmm32_xmm),
						new OpCodeHandler_VEX_VHIs4W(Register.XMM0, Code.VEX_Vfnmaddss_xmm_xmm_xmm_xmmm32)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.VEX_Vfnmaddsd_xmm_xmm_xmmm64_xmm),
						new OpCodeHandler_VEX_VHIs4W(Register.XMM0, Code.VEX_Vfnmaddsd_xmm_xmm_xmm_xmmm64)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.VEX_Vfnmsubps_xmm_xmm_xmmm128_xmm),
							new OpCodeHandler_VEX_VHWIs4(Register.YMM0, Code.VEX_Vfnmsubps_ymm_ymm_ymmm256_ymm)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHIs4W(Register.XMM0, Code.VEX_Vfnmsubps_xmm_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHIs4W(Register.YMM0, Code.VEX_Vfnmsubps_ymm_ymm_ymm_ymmm256)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.VEX_Vfnmsubpd_xmm_xmm_xmmm128_xmm),
							new OpCodeHandler_VEX_VHWIs4(Register.YMM0, Code.VEX_Vfnmsubpd_ymm_ymm_ymmm256_ymm)
						),
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHIs4W(Register.XMM0, Code.VEX_Vfnmsubpd_xmm_xmm_xmm_xmmm128),
							new OpCodeHandler_VEX_VHIs4W(Register.YMM0, Code.VEX_Vfnmsubpd_ymm_ymm_ymm_ymmm256)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.VEX_Vfnmsubss_xmm_xmm_xmmm32_xmm),
						new OpCodeHandler_VEX_VHIs4W(Register.XMM0, Code.VEX_Vfnmsubss_xmm_xmm_xmm_xmmm32)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						new OpCodeHandler_VEX_VHWIs4(Register.XMM0, Code.VEX_Vfnmsubsd_xmm_xmm_xmmm64_xmm),
						new OpCodeHandler_VEX_VHIs4W(Register.XMM0, Code.VEX_Vfnmsubsd_xmm_xmm_xmm_xmmm64)
					),
					invalid,
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
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_W(
						invalid,
						new OpCodeHandler_VectorLength_VEX(
							new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.VEX_Vgf2p8affineqb_xmm_xmm_xmmm128_imm8),
							new OpCodeHandler_VEX_VHWIb(Register.YMM0, Code.VEX_Vgf2p8affineqb_ymm_ymm_ymmm256_imm8)
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
							new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.VEX_Vgf2p8affineinvqb_xmm_xmm_xmmm128_imm8),
							new OpCodeHandler_VEX_VHWIb(Register.YMM0, Code.VEX_Vgf2p8affineinvqb_ymm_ymm_ymmm256_imm8)
						)
					),
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
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VWIb(Register.XMM0, Code.VEX_Vaeskeygenassist_xmm_xmmm128_imm8),
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
						new OpCodeHandler_VEX_Gv_Ev_Ib(Code.VEX_Rorx_r32_rm32_imm8, Code.VEX_Rorx_r64_rm64_imm8),
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
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vmovups_xmm_xmmm128),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vmovups_ymm_ymmm256)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vmovupd_xmm_xmmm128),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vmovupd_ymm_ymmm256)
					),
					new OpCodeHandler_RM(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vmovss_xmm_xmm_xmm),
						new OpCodeHandler_VEX_VM(Register.XMM0, Code.VEX_Vmovss_xmm_m32)
					),
					new OpCodeHandler_RM(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vmovsd_xmm_xmm_xmm),
						new OpCodeHandler_VEX_VM(Register.XMM0, Code.VEX_Vmovsd_xmm_m64)
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_WV(Register.XMM0, Code.VEX_Vmovups_xmmm128_xmm),
						new OpCodeHandler_VEX_WV(Register.YMM0, Code.VEX_Vmovups_ymmm256_ymm)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_WV(Register.XMM0, Code.VEX_Vmovupd_xmmm128_xmm),
						new OpCodeHandler_VEX_WV(Register.YMM0, Code.VEX_Vmovupd_ymmm256_ymm)
					),
					new OpCodeHandler_RM(
						new OpCodeHandler_VEX_WHV(Register.XMM0, Code.VEX_Vmovss_xmm_xmm_xmm_0F11),
						new OpCodeHandler_VEX_MV(Register.XMM0, Code.VEX_Vmovss_m32_xmm)
					),
					new OpCodeHandler_RM(
						new OpCodeHandler_VEX_WHV(Register.XMM0, Code.VEX_Vmovsd_xmm_xmm_xmm_0F11),
						new OpCodeHandler_VEX_MV(Register.XMM0, Code.VEX_Vmovsd_m64_xmm)
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vmovhlps_xmm_xmm_xmm, Code.VEX_Vmovlps_xmm_xmm_m64),
						invalid
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHM(Register.XMM0, Code.VEX_Vmovlpd_xmm_xmm_m64),
						invalid
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vmovsldup_xmm_xmmm128),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vmovsldup_ymm_ymmm256)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vmovddup_xmm_xmmm64),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vmovddup_ymm_ymmm256)
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_MV(Register.XMM0, Code.VEX_Vmovlps_m64_xmm),
						invalid
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_MV(Register.XMM0, Code.VEX_Vmovlpd_m64_xmm),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vunpcklps_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vunpcklps_ymm_ymm_ymmm256)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vunpcklpd_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vunpcklpd_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vunpckhps_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vunpckhps_ymm_ymm_ymmm256)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vunpckhpd_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vunpckhpd_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vmovlhps_xmm_xmm_xmm, Code.VEX_Vmovhps_xmm_xmm_m64),
						invalid
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHM(Register.XMM0, Code.VEX_Vmovhpd_xmm_xmm_m64),
						invalid
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vmovshdup_xmm_xmmm128),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vmovshdup_ymm_ymmm256)
					),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_MV(Register.XMM0, Code.VEX_Vmovhps_m64_xmm),
						invalid
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_MV(Register.XMM0, Code.VEX_Vmovhpd_m64_xmm),
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
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vmovaps_xmm_xmmm128),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vmovaps_ymm_ymmm256)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vmovapd_xmm_xmmm128),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vmovapd_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_WV(Register.XMM0, Code.VEX_Vmovaps_xmmm128_xmm),
						new OpCodeHandler_VEX_WV(Register.YMM0, Code.VEX_Vmovaps_ymmm256_ymm)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_WV(Register.XMM0, Code.VEX_Vmovapd_xmmm128_xmm),
						new OpCodeHandler_VEX_WV(Register.YMM0, Code.VEX_Vmovapd_ymmm256_ymm)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					invalid,
					new OpCodeHandler_VEX_VHEv(Register.XMM0, Code.VEX_Vcvtsi2ss_xmm_xmm_rm32, Code.VEX_Vcvtsi2ss_xmm_xmm_rm64),
					new OpCodeHandler_VEX_VHEv(Register.XMM0, Code.VEX_Vcvtsi2sd_xmm_xmm_rm32, Code.VEX_Vcvtsi2sd_xmm_xmm_rm64)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_MV(Register.XMM0, Code.VEX_Vmovntps_m128_xmm),
						new OpCodeHandler_VEX_MV(Register.YMM0, Code.VEX_Vmovntps_m256_ymm)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_MV(Register.XMM0, Code.VEX_Vmovntpd_m128_xmm),
						new OpCodeHandler_VEX_MV(Register.YMM0, Code.VEX_Vmovntpd_m256_ymm)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					invalid,
					new OpCodeHandler_VEX_Gv_W(Register.XMM0, Code.VEX_Vcvttss2si_r32_xmmm32, Code.VEX_Vcvttss2si_r64_xmmm32),
					new OpCodeHandler_VEX_Gv_W(Register.XMM0, Code.VEX_Vcvttsd2si_r32_xmmm64, Code.VEX_Vcvttsd2si_r64_xmmm64)
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					invalid,
					new OpCodeHandler_VEX_Gv_W(Register.XMM0, Code.VEX_Vcvtss2si_r32_xmmm32, Code.VEX_Vcvtss2si_r64_xmmm32),
					new OpCodeHandler_VEX_Gv_W(Register.XMM0, Code.VEX_Vcvtsd2si_r32_xmmm64, Code.VEX_Vcvtsd2si_r64_xmmm64)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vucomiss_xmm_xmmm32),
					new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vucomisd_xmm_xmmm64),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vcomiss_xmm_xmmm32),
					new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vcomisd_xmm_xmmm64),
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
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kandw_k_k_k),
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kandq_k_k_k)
						)
					),
					new OpCodeHandler_VectorLength_VEX(
						invalid,
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kandb_k_k_k),
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kandd_k_k_k)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						invalid,
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kandnw_k_k_k),
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kandnq_k_k_k)
						)
					),
					new OpCodeHandler_VectorLength_VEX(
						invalid,
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kandnb_k_k_k),
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kandnd_k_k_k)
						)
					),
					invalid,
					invalid
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_RK(Code.VEX_Knotw_k_k),
							new OpCodeHandler_VEX_VK_RK(Code.VEX_Knotq_k_k)
						),
						invalid
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_RK(Code.VEX_Knotb_k_k),
							new OpCodeHandler_VEX_VK_RK(Code.VEX_Knotd_k_k)
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
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Korw_k_k_k),
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Korq_k_k_k)
						)
					),
					new OpCodeHandler_VectorLength_VEX(
						invalid,
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Korb_k_k_k),
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kord_k_k_k)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						invalid,
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kxnorw_k_k_k),
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kxnorq_k_k_k)
						)
					),
					new OpCodeHandler_VectorLength_VEX(
						invalid,
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kxnorb_k_k_k),
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kxnord_k_k_k)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						invalid,
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kxorw_k_k_k),
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kxorq_k_k_k)
						)
					),
					new OpCodeHandler_VectorLength_VEX(
						invalid,
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kxorb_k_k_k),
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kxord_k_k_k)
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
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kaddw_k_k_k),
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kaddq_k_k_k)
						)
					),
					new OpCodeHandler_VectorLength_VEX(
						invalid,
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kaddb_k_k_k),
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kaddd_k_k_k)
						)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						invalid,
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kunpckwd_k_k_k),
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kunpckdq_k_k_k)
						)
					),
					new OpCodeHandler_VectorLength_VEX(
						invalid,
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_HK_RK(Code.VEX_Kunpckbw_k_k_k),
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
						new OpCodeHandler_VEX_Gv_RX(Register.XMM0, Code.VEX_Vmovmskps_r32_xmm, Code.VEX_Vmovmskps_r64_xmm),
						new OpCodeHandler_VEX_Gv_RX(Register.YMM0, Code.VEX_Vmovmskps_r32_ymm, Code.VEX_Vmovmskps_r64_ymm)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Gv_RX(Register.XMM0, Code.VEX_Vmovmskpd_r32_xmm, Code.VEX_Vmovmskpd_r64_xmm),
						new OpCodeHandler_VEX_Gv_RX(Register.YMM0, Code.VEX_Vmovmskpd_r32_ymm, Code.VEX_Vmovmskpd_r64_ymm)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vsqrtps_xmm_xmmm128),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vsqrtps_ymm_ymmm256)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vsqrtpd_xmm_xmmm128),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vsqrtpd_ymm_ymmm256)
					),
					new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vsqrtss_xmm_xmm_xmmm32),
					new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vsqrtsd_xmm_xmm_xmmm64)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vrsqrtps_xmm_xmmm128),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vrsqrtps_ymm_ymmm256)
					),
					invalid,
					new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vrsqrtss_xmm_xmm_xmmm32),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vrcpps_xmm_xmmm128),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vrcpps_ymm_ymmm256)
					),
					invalid,
					new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vrcpss_xmm_xmm_xmmm32),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vandps_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vandps_ymm_ymm_ymmm256)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vandpd_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vandpd_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vandnps_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vandnps_ymm_ymm_ymmm256)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vandnpd_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vandnpd_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vorps_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vorps_ymm_ymm_ymmm256)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vorpd_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vorpd_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vxorps_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vxorps_ymm_ymm_ymmm256)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vxorpd_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vxorpd_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),

				// 58
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vaddps_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vaddps_ymm_ymm_ymmm256)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vaddpd_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vaddpd_ymm_ymm_ymmm256)
					),
					new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vaddss_xmm_xmm_xmmm32),
					new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vaddsd_xmm_xmm_xmmm64)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vmulps_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vmulps_ymm_ymm_ymmm256)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vmulpd_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vmulpd_ymm_ymm_ymmm256)
					),
					new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vmulss_xmm_xmm_xmmm32),
					new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vmulsd_xmm_xmm_xmmm64)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vcvtps2pd_xmm_xmmm64),
						new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vcvtps2pd_ymm_xmmm128)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vcvtpd2ps_xmm_xmmm128),
						new OpCodeHandler_VEX_VW(Register.XMM0, Register.YMM0, Code.VEX_Vcvtpd2ps_xmm_ymmm256)
					),
					new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vcvtss2sd_xmm_xmm_xmmm32),
					new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vcvtsd2ss_xmm_xmm_xmmm64)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vcvtdq2ps_xmm_xmmm128),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vcvtdq2ps_ymm_ymmm256)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vcvtps2dq_xmm_xmmm128),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vcvtps2dq_ymm_ymmm256)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vcvttps2dq_xmm_xmmm128),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vcvttps2dq_ymm_ymmm256)
					),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vsubps_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vsubps_ymm_ymm_ymmm256)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vsubpd_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vsubpd_ymm_ymm_ymmm256)
					),
					new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vsubss_xmm_xmm_xmmm32),
					new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vsubsd_xmm_xmm_xmmm64)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vminps_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vminps_ymm_ymm_ymmm256)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vminpd_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vminpd_ymm_ymm_ymmm256)
					),
					new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vminss_xmm_xmm_xmmm32),
					new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vminsd_xmm_xmm_xmmm64)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vdivps_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vdivps_ymm_ymm_ymmm256)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vdivpd_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vdivpd_ymm_ymm_ymmm256)
					),
					new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vdivss_xmm_xmm_xmmm32),
					new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vdivsd_xmm_xmm_xmmm64)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vmaxps_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vmaxps_ymm_ymm_ymmm256)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vmaxpd_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vmaxpd_ymm_ymm_ymmm256)
					),
					new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vmaxss_xmm_xmm_xmmm32),
					new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vmaxsd_xmm_xmm_xmmm64)
				),

				// 60
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpunpcklbw_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpunpcklbw_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpunpcklwd_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpunpcklwd_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpunpckldq_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpunpckldq_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpacksswb_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpacksswb_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpcmpgtb_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpcmpgtb_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpcmpgtw_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpcmpgtw_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpcmpgtd_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpcmpgtd_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpackuswb_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpackuswb_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),

				// 68
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpunpckhbw_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpunpckhbw_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpunpckhwd_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpunpckhwd_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpunpckhdq_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpunpckhdq_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpackssdw_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpackssdw_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpunpcklqdq_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpunpcklqdq_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpunpckhqdq_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpunpckhqdq_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VX_Ev(Code.VEX_Vmovd_xmm_rm32, Code.VEX_Vmovq_xmm_rm64),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vmovdqa_xmm_xmmm128),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vmovdqa_ymm_ymmm256)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vmovdqu_xmm_xmmm128),
						new OpCodeHandler_VEX_VW(Register.YMM0, Code.VEX_Vmovdqu_ymm_ymmm256)
					),
					invalid
				),

				// 70
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VWIb(Register.XMM0, Code.VEX_Vpshufd_xmm_xmmm128_imm8),
						new OpCodeHandler_VEX_VWIb(Register.YMM0, Code.VEX_Vpshufd_ymm_ymmm256_imm8)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VWIb(Register.XMM0, Code.VEX_Vpshufhw_xmm_xmmm128_imm8),
						new OpCodeHandler_VEX_VWIb(Register.YMM0, Code.VEX_Vpshufhw_ymm_ymmm256_imm8)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VWIb(Register.XMM0, Code.VEX_Vpshuflw_xmm_xmmm128_imm8),
						new OpCodeHandler_VEX_VWIb(Register.YMM0, Code.VEX_Vpshuflw_ymm_ymmm256_imm8)
					)
				),
				new OpCodeHandler_Group(handlers_Grp_0F71),
				new OpCodeHandler_Group(handlers_Grp_0F72),
				new OpCodeHandler_Group(handlers_Grp_0F73),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpcmpeqb_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpcmpeqb_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpcmpeqw_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpcmpeqw_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpcmpeqd_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpcmpeqd_ymm_ymm_ymmm256)
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
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vhaddpd_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vhaddpd_ymm_ymm_ymmm256)
					),
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vhaddps_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vhaddps_ymm_ymm_ymmm256)
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vhsubpd_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vhsubpd_ymm_ymm_ymmm256)
					),
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vhsubps_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vhsubps_ymm_ymm_ymmm256)
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Ev_VX(Code.VEX_Vmovd_rm32_xmm, Code.VEX_Vmovq_rm64_xmm),
						invalid
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vmovq_xmm_xmmm64),
						invalid
					),
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_WV(Register.XMM0, Code.VEX_Vmovdqa_xmmm128_xmm),
						new OpCodeHandler_VEX_WV(Register.YMM0, Code.VEX_Vmovdqa_ymmm256_ymm)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_WV(Register.XMM0, Code.VEX_Vmovdqu_xmmm128_xmm),
						new OpCodeHandler_VEX_WV(Register.YMM0, Code.VEX_Vmovdqu_ymmm256_ymm)
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
							new OpCodeHandler_VEX_VK_WK(Code.VEX_Kmovw_k_km16),
							new OpCodeHandler_VEX_VK_WK(Code.VEX_Kmovq_k_km64)
						),
						invalid
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_WK(Code.VEX_Kmovb_k_km8),
							new OpCodeHandler_VEX_VK_WK(Code.VEX_Kmovd_k_km32)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_MK_VK(Code.VEX_Kmovw_m16_k),
							new OpCodeHandler_VEX_MK_VK(Code.VEX_Kmovq_m64_k)
						),
						invalid
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_MK_VK(Code.VEX_Kmovb_m8_k),
							new OpCodeHandler_VEX_MK_VK(Code.VEX_Kmovd_m32_k)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_R(Code.VEX_Kmovw_k_r32, Register.EAX),
							invalid
						),
						invalid
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_R(Code.VEX_Kmovb_k_r32, Register.EAX),
							invalid
						),
						invalid
					),
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_R(Code.VEX_Kmovd_k_r32, Register.EAX),
							invalid
						),
						invalid
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_G_VK(Code.VEX_Kmovw_r32_k, Register.EAX),
							invalid
						),
						invalid
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_G_VK(Code.VEX_Kmovb_r32_k, Register.EAX),
							invalid
						),
						invalid
					),
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_G_VK(Code.VEX_Kmovd_r32_k, Register.EAX),
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
							new OpCodeHandler_VEX_VK_RK(Code.VEX_Kortestw_k_k),
							new OpCodeHandler_VEX_VK_RK(Code.VEX_Kortestq_k_k)
						),
						invalid
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_RK(Code.VEX_Kortestb_k_k),
							new OpCodeHandler_VEX_VK_RK(Code.VEX_Kortestd_k_k)
						),
						invalid
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_RK(Code.VEX_Ktestw_k_k),
							new OpCodeHandler_VEX_VK_RK(Code.VEX_Ktestq_k_k)
						),
						invalid
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_W(
							new OpCodeHandler_VEX_VK_RK(Code.VEX_Ktestb_k_k),
							new OpCodeHandler_VEX_VK_RK(Code.VEX_Ktestd_k_k)
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
						new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.VEX_Vcmpps_xmm_xmm_xmmm128_imm8),
						new OpCodeHandler_VEX_VHWIb(Register.YMM0, Code.VEX_Vcmpps_ymm_ymm_ymmm256_imm8)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.VEX_Vcmppd_xmm_xmm_xmmm128_imm8),
						new OpCodeHandler_VEX_VHWIb(Register.YMM0, Code.VEX_Vcmppd_ymm_ymm_ymmm256_imm8)
					),
					new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.VEX_Vcmpss_xmm_xmm_xmmm32_imm8),
					new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.VEX_Vcmpsd_xmm_xmm_xmmm64_imm8)
				),
				invalid,
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VEX_VHEvIb(Register.XMM0, Code.VEX_Vpinsrw_xmm_xmm_r32m16_imm8, Code.VEX_Vpinsrw_xmm_xmm_r64m16_imm8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VEX_Gv_GPR_Ib(Register.XMM0, Code.VEX_Vpextrw_r32_xmm_imm8, Code.VEX_Vpextrw_r64_xmm_imm8),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.VEX_Vshufps_xmm_xmm_xmmm128_imm8),
						new OpCodeHandler_VEX_VHWIb(Register.YMM0, Code.VEX_Vshufps_ymm_ymm_ymmm256_imm8)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHWIb(Register.XMM0, Code.VEX_Vshufpd_xmm_xmm_xmmm128_imm8),
						new OpCodeHandler_VEX_VHWIb(Register.YMM0, Code.VEX_Vshufpd_ymm_ymm_ymmm256_imm8)
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
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vaddsubpd_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vaddsubpd_ymm_ymm_ymmm256)
					),
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vaddsubps_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vaddsubps_ymm_ymm_ymmm256)
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Register.XMM0, Register.XMM0, Code.VEX_Vpsrlw_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Register.YMM0, Register.XMM0, Code.VEX_Vpsrlw_ymm_ymm_xmmm128)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Register.XMM0, Register.XMM0, Code.VEX_Vpsrld_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Register.YMM0, Register.XMM0, Code.VEX_Vpsrld_ymm_ymm_xmmm128)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Register.XMM0, Register.XMM0, Code.VEX_Vpsrlq_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Register.YMM0, Register.XMM0, Code.VEX_Vpsrlq_ymm_ymm_xmmm128)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpaddq_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpaddq_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpmullw_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpmullw_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VEX_WV(Register.XMM0, Code.VEX_Vmovq_xmmm64_xmm),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_Gv_RX(Register.XMM0, Code.VEX_Vpmovmskb_r32_xmm, Code.VEX_Vpmovmskb_r64_xmm),
						new OpCodeHandler_VEX_Gv_RX(Register.YMM0, Code.VEX_Vpmovmskb_r32_ymm, Code.VEX_Vpmovmskb_r64_ymm)
					),
					invalid,
					invalid
				),

				// D8
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpsubusb_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpsubusb_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpsubusw_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpsubusw_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpminub_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpminub_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpand_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpand_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpaddusb_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpaddusb_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpaddusw_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpaddusw_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpmaxub_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpmaxub_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpandn_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpandn_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),

				// E0
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpavgb_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpavgb_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Register.XMM0, Register.XMM0, Code.VEX_Vpsraw_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Register.YMM0, Register.XMM0, Code.VEX_Vpsraw_ymm_ymm_xmmm128)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Register.XMM0, Register.XMM0, Code.VEX_Vpsrad_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Register.YMM0, Register.XMM0, Code.VEX_Vpsrad_ymm_ymm_xmmm128)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpavgw_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpavgw_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpmulhuw_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpmulhuw_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpmulhw_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpmulhw_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vcvttpd2dq_xmm_xmmm128),
						new OpCodeHandler_VEX_VW(Register.XMM0, Register.YMM0, Code.VEX_Vcvttpd2dq_xmm_ymmm256)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vcvtdq2pd_xmm_xmmm64),
						new OpCodeHandler_VEX_VW(Register.YMM0, Register.XMM0, Code.VEX_Vcvtdq2pd_ymm_xmmm128)
					),
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VW(Register.XMM0, Code.VEX_Vcvtpd2dq_xmm_xmmm128),
						new OpCodeHandler_VEX_VW(Register.XMM0, Register.YMM0, Code.VEX_Vcvtpd2dq_xmm_ymmm256)
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_MV(Register.XMM0, Code.VEX_Vmovntdq_m128_xmm),
						new OpCodeHandler_VEX_MV(Register.YMM0, Code.VEX_Vmovntdq_m256_ymm)
					),
					invalid,
					invalid
				),

				// E8
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpsubsb_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpsubsb_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpsubsw_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpsubsw_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpminsw_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpminsw_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpor_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpor_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpaddsb_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpaddsb_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpaddsw_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpaddsw_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpmaxsw_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpmaxsw_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpxor_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpxor_ymm_ymm_ymmm256)
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
						new OpCodeHandler_VEX_VM(Register.XMM0, Code.VEX_Vlddqu_xmm_m128),
						new OpCodeHandler_VEX_VM(Register.YMM0, Code.VEX_Vlddqu_ymm_m256)
					)
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Register.XMM0, Register.XMM0, Code.VEX_Vpsllw_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Register.YMM0, Register.XMM0, Code.VEX_Vpsllw_ymm_ymm_xmmm128)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Register.XMM0, Register.XMM0, Code.VEX_Vpslld_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Register.YMM0, Register.XMM0, Code.VEX_Vpslld_ymm_ymm_xmmm128)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Register.XMM0, Register.XMM0, Code.VEX_Vpsllq_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Register.YMM0, Register.XMM0, Code.VEX_Vpsllq_ymm_ymm_xmmm128)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpmuludq_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpmuludq_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpmaddwd_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpmaddwd_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpsadbw_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpsadbw_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_rDI_VX_RX(Register.XMM0, Code.VEX_Vmaskmovdqu_rDI_xmm_xmm),
						invalid
					),
					invalid,
					invalid
				),

				// F8
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpsubb_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpsubb_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpsubw_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpsubw_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpsubd_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpsubd_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpsubq_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpsubq_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpaddb_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpaddb_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpaddw_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpaddw_ymm_ymm_ymmm256)
					),
					invalid,
					invalid
				),
				new OpCodeHandler_MandatoryPrefix2(
					invalid,
					new OpCodeHandler_VectorLength_VEX(
						new OpCodeHandler_VEX_VHW(Register.XMM0, Code.VEX_Vpaddd_xmm_xmm_xmmm128),
						new OpCodeHandler_VEX_VHW(Register.YMM0, Code.VEX_Vpaddd_ymm_ymm_ymmm256)
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
