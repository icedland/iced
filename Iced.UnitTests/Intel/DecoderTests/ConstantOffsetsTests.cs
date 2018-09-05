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

using System.Collections.Generic;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.DecoderTests {
	public sealed class ConstantOffsetsTests : DecoderTest {
		[Theory]
		[MemberData(nameof(Test32_ConstantOffsets_Data))]
		void Test32_ConstantOffsets(string hexBytes, Code code, uint immOffset, uint immSize, uint displOffset, uint displSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();
			Assert.Equal(code, instr.Code);
			var constantOffsets = decoder.GetConstantOffsets(ref instr);
			Assert.Equal(displSize != 0, constantOffsets.HasDisplacement);
			Assert.Equal(immSize != 0, constantOffsets.HasImmediate);
			Assert.Equal(displOffset, constantOffsets.DisplacementOffset);
			Assert.Equal(displSize, constantOffsets.DisplacementSize);
			Assert.Equal(immOffset, constantOffsets.ImmediateOffset);
			Assert.Equal(immSize, constantOffsets.ImmediateSize);
		}
		public static IEnumerable<object[]> Test32_ConstantOffsets_Data {
			get {
				yield return new object[] { "68 5AA51234", Code.Push_Id, 1, 4, 0, 0 };
				yield return new object[] { "6A A5", Code.Push_Ib32, 1, 1, 0, 0 };
				yield return new object[] { "66 C2 5AA5", Code.Retnw_Iw, 2, 2, 0, 0 };
				yield return new object[] { "C2 5AA5", Code.Retnd_Iw, 1, 2, 0, 0 };
				yield return new object[] { "C8 5AA5 A6", Code.Enterd_Iw_Ib, 1, 2, 0, 0 };
				yield return new object[] { "D4 0A", Code.Aam_Ib, 1, 1, 0, 0 };
				yield return new object[] { "D5 0A", Code.Aad_Ib, 1, 1, 0, 0 };

				yield return new object[] { "66 9A 1122 3344", Code.Call_Aww, 2, 2, 0, 0 };
				yield return new object[] { "9A 11223344 5566", Code.Call_Adw, 1, 4, 0, 0 };
				yield return new object[] { "66 EA 1122 3344", Code.Jmp_Aww, 2, 2, 0, 0 };
				yield return new object[] { "EA 11223344 5566", Code.Jmp_Adw, 1, 4, 0, 0 };

				yield return new object[] { "67 A0 1122", Code.Mov_AL_Ob, 0, 0, 2, 2 };
				yield return new object[] { "66 67 A1 1122", Code.Mov_AX_Ow, 0, 0, 3, 2 };
				yield return new object[] { "67 A1 1122", Code.Mov_EAX_Od, 0, 0, 2, 2 };
				yield return new object[] { "67 A2 1122", Code.Mov_Ob_AL, 0, 0, 2, 2 };
				yield return new object[] { "66 67 A3 1122", Code.Mov_Ow_AX, 0, 0, 3, 2 };
				yield return new object[] { "67 A3 1122", Code.Mov_Od_EAX, 0, 0, 2, 2 };

				yield return new object[] { "A0 11223344", Code.Mov_AL_Ob, 0, 0, 1, 4 };
				yield return new object[] { "66 A1 11223344", Code.Mov_AX_Ow, 0, 0, 2, 4 };
				yield return new object[] { "A1 11223344", Code.Mov_EAX_Od, 0, 0, 1, 4 };
				yield return new object[] { "A2 11223344", Code.Mov_Ob_AL, 0, 0, 1, 4 };
				yield return new object[] { "66 A3 11223344", Code.Mov_Ow_AX, 0, 0, 2, 4 };
				yield return new object[] { "A3 11223344", Code.Mov_Od_EAX, 0, 0, 1, 4 };

				yield return new object[] { "66 67 01 0F", Code.Add_Ew_Gw, 0, 0, 0, 0 };
				yield return new object[] { "66 67 01 4F 12", Code.Add_Ew_Gw, 0, 0, 4, 1 };
				yield return new object[] { "66 67 01 8F 3412", Code.Add_Ew_Gw, 0, 0, 4, 2 };

				yield return new object[] { "01 08", Code.Add_Ed_Gd, 0, 0, 0, 0 };
				yield return new object[] { "01 48 5A", Code.Add_Ed_Gd, 0, 0, 2, 1 };
				yield return new object[] { "01 88 44332211", Code.Add_Ed_Gd, 0, 0, 2, 4 };
				yield return new object[] { "01 0C 10", Code.Add_Ed_Gd, 0, 0, 0, 0 };
				yield return new object[] { "01 4C 10 5A", Code.Add_Ed_Gd, 0, 0, 3, 1 };
				yield return new object[] { "01 8C 10 44332211", Code.Add_Ed_Gd, 0, 0, 3, 4 };
				yield return new object[] { "01 0D 44332211", Code.Add_Ed_Gd, 0, 0, 2, 4 };

				yield return new object[] { "67 81 47 5A 11223344", Code.Add_Ed_Id, 4, 4, 3, 1 };
				yield return new object[] { "67 81 87 5AA5 22334455", Code.Add_Ed_Id, 5, 4, 3, 2 };

				yield return new object[] { "80 80 78563412 9A", Code.Add_Eb_Ib, 6, 1, 2, 4 };
				yield return new object[] { "66 81 40 12 5634", Code.Add_Ew_Iw, 4, 2, 3, 1 };
				yield return new object[] { "81 40 12 9A785634", Code.Add_Ed_Id, 3, 4, 2, 1 };

				yield return new object[] { "CC", Code.Int3, 0, 0, 0, 0 };
				yield return new object[] { "F1", Code.Int1, 0, 0, 0, 0 };
				yield return new object[] { "D0 C1", Code.Rol_Eb_1, 0, 0, 0, 0 };
				yield return new object[] { "D0 CA", Code.Ror_Eb_1, 0, 0, 0, 0 };
				yield return new object[] { "D0 D3", Code.Rcl_Eb_1, 0, 0, 0, 0 };
				yield return new object[] { "D0 DC", Code.Rcr_Eb_1, 0, 0, 0, 0 };
				yield return new object[] { "D0 E5", Code.Shl_Eb_1, 0, 0, 0, 0 };
				yield return new object[] { "D0 EE", Code.Shr_Eb_1, 0, 0, 0, 0 };
				yield return new object[] { "D0 F8", Code.Sar_Eb_1, 0, 0, 0, 0 };
				yield return new object[] { "66 D1 C1", Code.Rol_Ew_1, 0, 0, 0, 0 };
				yield return new object[] { "D1 C1", Code.Rol_Ed_1, 0, 0, 0, 0 };
				yield return new object[] { "66 D1 CA", Code.Ror_Ew_1, 0, 0, 0, 0 };
				yield return new object[] { "D1 CA", Code.Ror_Ed_1, 0, 0, 0, 0 };
				yield return new object[] { "66 D1 D3", Code.Rcl_Ew_1, 0, 0, 0, 0 };
				yield return new object[] { "D1 D3", Code.Rcl_Ed_1, 0, 0, 0, 0 };
				yield return new object[] { "66 D1 DC", Code.Rcr_Ew_1, 0, 0, 0, 0 };
				yield return new object[] { "D1 DC", Code.Rcr_Ed_1, 0, 0, 0, 0 };
				yield return new object[] { "66 D1 E5", Code.Shl_Ew_1, 0, 0, 0, 0 };
				yield return new object[] { "D1 E5", Code.Shl_Ed_1, 0, 0, 0, 0 };
				yield return new object[] { "66 D1 EE", Code.Shr_Ew_1, 0, 0, 0, 0 };
				yield return new object[] { "D1 EE", Code.Shr_Ed_1, 0, 0, 0, 0 };
				yield return new object[] { "66 D1 F8", Code.Sar_Ew_1, 0, 0, 0, 0 };
				yield return new object[] { "D1 F8", Code.Sar_Ed_1, 0, 0, 0, 0 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_ConstantOffsets_Data))]
		void Test64_ConstantOffsets(string hexBytes, Code code, uint immOffset, uint immSize, uint displOffset, uint displSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();
			Assert.Equal(code, instr.Code);
			var constantOffsets = decoder.GetConstantOffsets(ref instr);
			Assert.Equal(displSize != 0, constantOffsets.HasDisplacement);
			Assert.Equal(immSize != 0, constantOffsets.HasImmediate);
			Assert.Equal(displOffset, constantOffsets.DisplacementOffset);
			Assert.Equal(displSize, constantOffsets.DisplacementSize);
			Assert.Equal(immOffset, constantOffsets.ImmediateOffset);
			Assert.Equal(immSize, constantOffsets.ImmediateSize);
		}
		public static IEnumerable<object[]> Test64_ConstantOffsets_Data {
			get {
				yield return new object[] { "04 A5", Code.Add_AL_Ib, 1, 1, 0, 0 };
				yield return new object[] { "66 05 A55A", Code.Add_AX_Iw, 2, 2, 0, 0 };
				yield return new object[] { "05 A55A3412", Code.Add_EAX_Id, 1, 4, 0, 0 };
				yield return new object[] { "48 05 A55A34A2", Code.Add_RAX_Id64, 2, 4, 0, 0 };
				yield return new object[] { "0C A5", Code.Or_AL_Ib, 1, 1, 0, 0 };
				yield return new object[] { "66 0D A55A", Code.Or_AX_Iw, 2, 2, 0, 0 };
				yield return new object[] { "0D A55A3412", Code.Or_EAX_Id, 1, 4, 0, 0 };
				yield return new object[] { "48 0D A55A34A2", Code.Or_RAX_Id64, 2, 4, 0, 0 };
				yield return new object[] { "14 A5", Code.Adc_AL_Ib, 1, 1, 0, 0 };
				yield return new object[] { "66 15 A55A", Code.Adc_AX_Iw, 2, 2, 0, 0 };
				yield return new object[] { "15 A55A3412", Code.Adc_EAX_Id, 1, 4, 0, 0 };
				yield return new object[] { "48 15 A55A34A2", Code.Adc_RAX_Id64, 2, 4, 0, 0 };
				yield return new object[] { "1C A5", Code.Sbb_AL_Ib, 1, 1, 0, 0 };
				yield return new object[] { "66 1D A55A", Code.Sbb_AX_Iw, 2, 2, 0, 0 };
				yield return new object[] { "1D A55A3412", Code.Sbb_EAX_Id, 1, 4, 0, 0 };
				yield return new object[] { "48 1D A55A3482", Code.Sbb_RAX_Id64, 2, 4, 0, 0 };
				yield return new object[] { "24 A5", Code.And_AL_Ib, 1, 1, 0, 0 };
				yield return new object[] { "66 25 A55A", Code.And_AX_Iw, 2, 2, 0, 0 };
				yield return new object[] { "25 A55A3412", Code.And_EAX_Id, 1, 4, 0, 0 };
				yield return new object[] { "48 25 A55A3482", Code.And_RAX_Id64, 2, 4, 0, 0 };
				yield return new object[] { "2C A5", Code.Sub_AL_Ib, 1, 1, 0, 0 };
				yield return new object[] { "66 2D A55A", Code.Sub_AX_Iw, 2, 2, 0, 0 };
				yield return new object[] { "2D A55A3412", Code.Sub_EAX_Id, 1, 4, 0, 0 };
				yield return new object[] { "48 2D A55A3482", Code.Sub_RAX_Id64, 2, 4, 0, 0 };
				yield return new object[] { "34 A5", Code.Xor_AL_Ib, 1, 1, 0, 0 };
				yield return new object[] { "66 35 A55A", Code.Xor_AX_Iw, 2, 2, 0, 0 };
				yield return new object[] { "35 A55A3412", Code.Xor_EAX_Id, 1, 4, 0, 0 };
				yield return new object[] { "48 35 A55A3482", Code.Xor_RAX_Id64, 2, 4, 0, 0 };
				yield return new object[] { "3C A5", Code.Cmp_AL_Ib, 1, 1, 0, 0 };
				yield return new object[] { "66 3D A55A", Code.Cmp_AX_Iw, 2, 2, 0, 0 };
				yield return new object[] { "3D A55A3412", Code.Cmp_EAX_Id, 1, 4, 0, 0 };
				yield return new object[] { "48 3D A55A3482", Code.Cmp_RAX_Id64, 2, 4, 0, 0 };
				yield return new object[] { "66 68 5AA5", Code.Push_Iw, 2, 2, 0, 0 };
				yield return new object[] { "68 5AA51284", Code.Push_Id64, 1, 4, 0, 0 };
				yield return new object[] { "66 69 CE A55A", Code.Imul_Gw_Ew_Iw, 3, 2, 0, 0 };
				yield return new object[] { "69 CE 5AA51234", Code.Imul_Gd_Ed_Id, 2, 4, 0, 0 };
				yield return new object[] { "48 69 CE 5AA51284", Code.Imul_Gq_Eq_Id64, 3, 4, 0, 0 };
				yield return new object[] { "66 6A A5", Code.Push_Ib16, 2, 1, 0, 0 };
				yield return new object[] { "6A A5", Code.Push_Ib64, 1, 1, 0, 0 };
				yield return new object[] { "66 6B CE A5", Code.Imul_Gw_Ew_Ib16, 3, 1, 0, 0 };
				yield return new object[] { "6B CE A5", Code.Imul_Gd_Ed_Ib32, 2, 1, 0, 0 };
				yield return new object[] { "48 6B CE A5", Code.Imul_Gq_Eq_Ib64, 3, 1, 0, 0 };
				yield return new object[] { "80 C1 5A", Code.Add_Eb_Ib, 2, 1, 0, 0 };
				yield return new object[] { "80 CA A5", Code.Or_Eb_Ib, 2, 1, 0, 0 };
				yield return new object[] { "80 D3 5A", Code.Adc_Eb_Ib, 2, 1, 0, 0 };
				yield return new object[] { "80 DC A5", Code.Sbb_Eb_Ib, 2, 1, 0, 0 };
				yield return new object[] { "80 E5 5A", Code.And_Eb_Ib, 2, 1, 0, 0 };
				yield return new object[] { "80 EE A5", Code.Sub_Eb_Ib, 2, 1, 0, 0 };
				yield return new object[] { "80 F7 5A", Code.Xor_Eb_Ib, 2, 1, 0, 0 };
				yield return new object[] { "80 F8 A5", Code.Cmp_Eb_Ib, 2, 1, 0, 0 };
				yield return new object[] { "66 81 C1 5AA5", Code.Add_Ew_Iw, 3, 2, 0, 0 };
				yield return new object[] { "81 C1 5AA51234", Code.Add_Ed_Id, 2, 4, 0, 0 };
				yield return new object[] { "48 81 C1 5AA51284", Code.Add_Eq_Id64, 3, 4, 0, 0 };
				yield return new object[] { "66 81 CA A55A", Code.Or_Ew_Iw, 3, 2, 0, 0 };
				yield return new object[] { "81 CA A55A89AB", Code.Or_Ed_Id, 2, 4, 0, 0 };
				yield return new object[] { "48 81 CA 5AA51284", Code.Or_Eq_Id64, 3, 4, 0, 0 };
				yield return new object[] { "66 81 D3 5AA5", Code.Adc_Ew_Iw, 3, 2, 0, 0 };
				yield return new object[] { "81 D3 5AA51234", Code.Adc_Ed_Id, 2, 4, 0, 0 };
				yield return new object[] { "48 81 D3 5AA51284", Code.Adc_Eq_Id64, 3, 4, 0, 0 };
				yield return new object[] { "66 81 DC A55A", Code.Sbb_Ew_Iw, 3, 2, 0, 0 };
				yield return new object[] { "81 DC A55A89AB", Code.Sbb_Ed_Id, 2, 4, 0, 0 };
				yield return new object[] { "48 81 DC 5AA51284", Code.Sbb_Eq_Id64, 3, 4, 0, 0 };
				yield return new object[] { "66 81 E5 5AA5", Code.And_Ew_Iw, 3, 2, 0, 0 };
				yield return new object[] { "81 E5 5AA51234", Code.And_Ed_Id, 2, 4, 0, 0 };
				yield return new object[] { "48 81 E5 5AA51284", Code.And_Eq_Id64, 3, 4, 0, 0 };
				yield return new object[] { "66 81 EE A55A", Code.Sub_Ew_Iw, 3, 2, 0, 0 };
				yield return new object[] { "81 EE A55A89AB", Code.Sub_Ed_Id, 2, 4, 0, 0 };
				yield return new object[] { "48 81 EE 5AA51284", Code.Sub_Eq_Id64, 3, 4, 0, 0 };
				yield return new object[] { "66 81 F7 5AA5", Code.Xor_Ew_Iw, 3, 2, 0, 0 };
				yield return new object[] { "81 F7 5AA51234", Code.Xor_Ed_Id, 2, 4, 0, 0 };
				yield return new object[] { "48 81 F7 5AA51284", Code.Xor_Eq_Id64, 3, 4, 0, 0 };
				yield return new object[] { "66 81 F8 A55A", Code.Cmp_Ew_Iw, 3, 2, 0, 0 };
				yield return new object[] { "81 F8 A55A89AB", Code.Cmp_Ed_Id, 2, 4, 0, 0 };
				yield return new object[] { "49 81 F8 5AA51284", Code.Cmp_Eq_Id64, 3, 4, 0, 0 };
				yield return new object[] { "66 83 C1 A5", Code.Add_Ew_Ib16, 3, 1, 0, 0 };
				yield return new object[] { "83 C1 A5", Code.Add_Ed_Ib32, 2, 1, 0, 0 };
				yield return new object[] { "48 83 C1 A5", Code.Add_Eq_Ib64, 3, 1, 0, 0 };
				yield return new object[] { "66 83 CA A5", Code.Or_Ew_Ib16, 3, 1, 0, 0 };
				yield return new object[] { "83 CA A5", Code.Or_Ed_Ib32, 2, 1, 0, 0 };
				yield return new object[] { "48 83 CA A5", Code.Or_Eq_Ib64, 3, 1, 0, 0 };
				yield return new object[] { "66 83 D3 A5", Code.Adc_Ew_Ib16, 3, 1, 0, 0 };
				yield return new object[] { "83 D3 A5", Code.Adc_Ed_Ib32, 2, 1, 0, 0 };
				yield return new object[] { "48 83 D3 A5", Code.Adc_Eq_Ib64, 3, 1, 0, 0 };
				yield return new object[] { "66 83 DC A5", Code.Sbb_Ew_Ib16, 3, 1, 0, 0 };
				yield return new object[] { "83 DC A5", Code.Sbb_Ed_Ib32, 2, 1, 0, 0 };
				yield return new object[] { "48 83 DC A5", Code.Sbb_Eq_Ib64, 3, 1, 0, 0 };
				yield return new object[] { "66 83 E5 A5", Code.And_Ew_Ib16, 3, 1, 0, 0 };
				yield return new object[] { "83 E5 A5", Code.And_Ed_Ib32, 2, 1, 0, 0 };
				yield return new object[] { "48 83 E5 A5", Code.And_Eq_Ib64, 3, 1, 0, 0 };
				yield return new object[] { "66 83 EE A5", Code.Sub_Ew_Ib16, 3, 1, 0, 0 };
				yield return new object[] { "83 EE A5", Code.Sub_Ed_Ib32, 2, 1, 0, 0 };
				yield return new object[] { "48 83 EE A5", Code.Sub_Eq_Ib64, 3, 1, 0, 0 };
				yield return new object[] { "66 83 F7 A5", Code.Xor_Ew_Ib16, 3, 1, 0, 0 };
				yield return new object[] { "83 F7 A5", Code.Xor_Ed_Ib32, 2, 1, 0, 0 };
				yield return new object[] { "48 83 F7 A5", Code.Xor_Eq_Ib64, 3, 1, 0, 0 };
				yield return new object[] { "66 83 F8 A5", Code.Cmp_Ew_Ib16, 3, 1, 0, 0 };
				yield return new object[] { "83 F8 A5", Code.Cmp_Ed_Ib32, 2, 1, 0, 0 };
				yield return new object[] { "49 83 F8 A5", Code.Cmp_Eq_Ib64, 3, 1, 0, 0 };
				yield return new object[] { "A8 A5", Code.Test_AL_Ib, 1, 1, 0, 0 };
				yield return new object[] { "66 A9 A55A", Code.Test_AX_Iw, 2, 2, 0, 0 };
				yield return new object[] { "A9 A55A3412", Code.Test_EAX_Id, 1, 4, 0, 0 };
				yield return new object[] { "48 A9 A55A3482", Code.Test_RAX_Id64, 2, 4, 0, 0 };
				yield return new object[] { "B0 5A", Code.Mov_AL_Ib, 1, 1, 0, 0 };
				yield return new object[] { "41 B0 5A", Code.Mov_R8L_Ib, 2, 1, 0, 0 };
				yield return new object[] { "B1 A5", Code.Mov_CL_Ib, 1, 1, 0, 0 };
				yield return new object[] { "41 B1 A5", Code.Mov_R9L_Ib, 2, 1, 0, 0 };
				yield return new object[] { "B2 5A", Code.Mov_DL_Ib, 1, 1, 0, 0 };
				yield return new object[] { "41 B2 5A", Code.Mov_R10L_Ib, 2, 1, 0, 0 };
				yield return new object[] { "B3 A5", Code.Mov_BL_Ib, 1, 1, 0, 0 };
				yield return new object[] { "41 B3 A5", Code.Mov_R11L_Ib, 2, 1, 0, 0 };
				yield return new object[] { "B4 5A", Code.Mov_AH_Ib, 1, 1, 0, 0 };
				yield return new object[] { "40 B4 5A", Code.Mov_SPL_Ib, 2, 1, 0, 0 };
				yield return new object[] { "41 B4 5A", Code.Mov_R12L_Ib, 2, 1, 0, 0 };
				yield return new object[] { "B5 A5", Code.Mov_CH_Ib, 1, 1, 0, 0 };
				yield return new object[] { "40 B5 A5", Code.Mov_BPL_Ib, 2, 1, 0, 0 };
				yield return new object[] { "41 B5 A5", Code.Mov_R13L_Ib, 2, 1, 0, 0 };
				yield return new object[] { "B6 5A", Code.Mov_DH_Ib, 1, 1, 0, 0 };
				yield return new object[] { "40 B6 5A", Code.Mov_SIL_Ib, 2, 1, 0, 0 };
				yield return new object[] { "41 B6 5A", Code.Mov_R14L_Ib, 2, 1, 0, 0 };
				yield return new object[] { "B7 A5", Code.Mov_BH_Ib, 1, 1, 0, 0 };
				yield return new object[] { "40 B7 A5", Code.Mov_DIL_Ib, 2, 1, 0, 0 };
				yield return new object[] { "41 B7 A5", Code.Mov_R15L_Ib, 2, 1, 0, 0 };
				yield return new object[] { "66 B8 5AA5", Code.Mov_AX_Iw, 2, 2, 0, 0 };
				yield return new object[] { "66 41 B8 5AA5", Code.Mov_R8W_Iw, 3, 2, 0, 0 };
				yield return new object[] { "B8 5AA51234", Code.Mov_EAX_Id, 1, 4, 0, 0 };
				yield return new object[] { "41 B8 5AA51234", Code.Mov_R8D_Id, 2, 4, 0, 0 };
				yield return new object[] { "48 B8 041526375AA51234", Code.Mov_RAX_Iq, 2, 8, 0, 0 };
				yield return new object[] { "49 B8 041526375AA51234", Code.Mov_R8_Iq, 2, 8, 0, 0 };
				yield return new object[] { "66 B9 A55A", Code.Mov_CX_Iw, 2, 2, 0, 0 };
				yield return new object[] { "66 41 B9 A55A", Code.Mov_R9W_Iw, 3, 2, 0, 0 };
				yield return new object[] { "B9 A55A5678", Code.Mov_ECX_Id, 1, 4, 0, 0 };
				yield return new object[] { "41 B9 A55A5678", Code.Mov_R9D_Id, 2, 4, 0, 0 };
				yield return new object[] { "48 B9 04152637A55A5678", Code.Mov_RCX_Iq, 2, 8, 0, 0 };
				yield return new object[] { "49 B9 04152637A55A5678", Code.Mov_R9_Iq, 2, 8, 0, 0 };
				yield return new object[] { "66 BA 5AA5", Code.Mov_DX_Iw, 2, 2, 0, 0 };
				yield return new object[] { "66 41 BA 5AA5", Code.Mov_R10W_Iw, 3, 2, 0, 0 };
				yield return new object[] { "BA 5AA51234", Code.Mov_EDX_Id, 1, 4, 0, 0 };
				yield return new object[] { "41 BA 5AA51234", Code.Mov_R10D_Id, 2, 4, 0, 0 };
				yield return new object[] { "48 BA 041526375AA51234", Code.Mov_RDX_Iq, 2, 8, 0, 0 };
				yield return new object[] { "49 BA 041526375AA51234", Code.Mov_R10_Iq, 2, 8, 0, 0 };
				yield return new object[] { "66 BB A55A", Code.Mov_BX_Iw, 2, 2, 0, 0 };
				yield return new object[] { "66 41 BB A55A", Code.Mov_R11W_Iw, 3, 2, 0, 0 };
				yield return new object[] { "BB A55A5678", Code.Mov_EBX_Id, 1, 4, 0, 0 };
				yield return new object[] { "41 BB A55A5678", Code.Mov_R11D_Id, 2, 4, 0, 0 };
				yield return new object[] { "48 BB 04152637A55A5678", Code.Mov_RBX_Iq, 2, 8, 0, 0 };
				yield return new object[] { "49 BB 04152637A55A5678", Code.Mov_R11_Iq, 2, 8, 0, 0 };
				yield return new object[] { "66 BC 5AA5", Code.Mov_SP_Iw, 2, 2, 0, 0 };
				yield return new object[] { "66 41 BC 5AA5", Code.Mov_R12W_Iw, 3, 2, 0, 0 };
				yield return new object[] { "BC 5AA51234", Code.Mov_ESP_Id, 1, 4, 0, 0 };
				yield return new object[] { "41 BC 5AA51234", Code.Mov_R12D_Id, 2, 4, 0, 0 };
				yield return new object[] { "48 BC 041526375AA51234", Code.Mov_RSP_Iq, 2, 8, 0, 0 };
				yield return new object[] { "49 BC 041526375AA51234", Code.Mov_R12_Iq, 2, 8, 0, 0 };
				yield return new object[] { "66 BD A55A", Code.Mov_BP_Iw, 2, 2, 0, 0 };
				yield return new object[] { "66 41 BD A55A", Code.Mov_R13W_Iw, 3, 2, 0, 0 };
				yield return new object[] { "BD A55A5678", Code.Mov_EBP_Id, 1, 4, 0, 0 };
				yield return new object[] { "41 BD A55A5678", Code.Mov_R13D_Id, 2, 4, 0, 0 };
				yield return new object[] { "48 BD 04152637A55A5678", Code.Mov_RBP_Iq, 2, 8, 0, 0 };
				yield return new object[] { "49 BD 04152637A55A5678", Code.Mov_R13_Iq, 2, 8, 0, 0 };
				yield return new object[] { "66 BE 5AA5", Code.Mov_SI_Iw, 2, 2, 0, 0 };
				yield return new object[] { "66 41 BE 5AA5", Code.Mov_R14W_Iw, 3, 2, 0, 0 };
				yield return new object[] { "BE 5AA51234", Code.Mov_ESI_Id, 1, 4, 0, 0 };
				yield return new object[] { "41 BE 5AA51234", Code.Mov_R14D_Id, 2, 4, 0, 0 };
				yield return new object[] { "48 BE 041526375AA51234", Code.Mov_RSI_Iq, 2, 8, 0, 0 };
				yield return new object[] { "49 BE 041526375AA51234", Code.Mov_R14_Iq, 2, 8, 0, 0 };
				yield return new object[] { "66 BF A55A", Code.Mov_DI_Iw, 2, 2, 0, 0 };
				yield return new object[] { "66 41 BF A55A", Code.Mov_R15W_Iw, 3, 2, 0, 0 };
				yield return new object[] { "BF A55A5678", Code.Mov_EDI_Id, 1, 4, 0, 0 };
				yield return new object[] { "41 BF A55A5678", Code.Mov_R15D_Id, 2, 4, 0, 0 };
				yield return new object[] { "48 BF 04152637A55A5678", Code.Mov_RDI_Iq, 2, 8, 0, 0 };
				yield return new object[] { "49 BF 04152637A55A5678", Code.Mov_R15_Iq, 2, 8, 0, 0 };
				yield return new object[] { "C0 C1 5A", Code.Rol_Eb_Ib, 2, 1, 0, 0 };
				yield return new object[] { "C0 CA A5", Code.Ror_Eb_Ib, 2, 1, 0, 0 };
				yield return new object[] { "C0 D3 5A", Code.Rcl_Eb_Ib, 2, 1, 0, 0 };
				yield return new object[] { "C0 DC A5", Code.Rcr_Eb_Ib, 2, 1, 0, 0 };
				yield return new object[] { "C0 E5 5A", Code.Shl_Eb_Ib, 2, 1, 0, 0 };
				yield return new object[] { "C0 EE A5", Code.Shr_Eb_Ib, 2, 1, 0, 0 };
				yield return new object[] { "C0 F8 A5", Code.Sar_Eb_Ib, 2, 1, 0, 0 };
				yield return new object[] { "66 C1 C1 5A", Code.Rol_Ew_Ib, 3, 1, 0, 0 };
				yield return new object[] { "C1 C1 5A", Code.Rol_Ed_Ib, 2, 1, 0, 0 };
				yield return new object[] { "48 C1 C1 5A", Code.Rol_Eq_Ib, 3, 1, 0, 0 };
				yield return new object[] { "66 C1 CA A5", Code.Ror_Ew_Ib, 3, 1, 0, 0 };
				yield return new object[] { "C1 CA A5", Code.Ror_Ed_Ib, 2, 1, 0, 0 };
				yield return new object[] { "48 C1 CA A5", Code.Ror_Eq_Ib, 3, 1, 0, 0 };
				yield return new object[] { "66 C1 D3 5A", Code.Rcl_Ew_Ib, 3, 1, 0, 0 };
				yield return new object[] { "C1 D3 5A", Code.Rcl_Ed_Ib, 2, 1, 0, 0 };
				yield return new object[] { "48 C1 D3 5A", Code.Rcl_Eq_Ib, 3, 1, 0, 0 };
				yield return new object[] { "66 C1 DC A5", Code.Rcr_Ew_Ib, 3, 1, 0, 0 };
				yield return new object[] { "C1 DC A5", Code.Rcr_Ed_Ib, 2, 1, 0, 0 };
				yield return new object[] { "48 C1 DC A5", Code.Rcr_Eq_Ib, 3, 1, 0, 0 };
				yield return new object[] { "66 C1 E5 5A", Code.Shl_Ew_Ib, 3, 1, 0, 0 };
				yield return new object[] { "C1 E5 5A", Code.Shl_Ed_Ib, 2, 1, 0, 0 };
				yield return new object[] { "48 C1 E5 5A", Code.Shl_Eq_Ib, 3, 1, 0, 0 };
				yield return new object[] { "66 C1 EE A5", Code.Shr_Ew_Ib, 3, 1, 0, 0 };
				yield return new object[] { "C1 EE A5", Code.Shr_Ed_Ib, 2, 1, 0, 0 };
				yield return new object[] { "48 C1 EE A5", Code.Shr_Eq_Ib, 3, 1, 0, 0 };
				yield return new object[] { "66 C1 F8 A5", Code.Sar_Ew_Ib, 3, 1, 0, 0 };
				yield return new object[] { "C1 F8 A5", Code.Sar_Ed_Ib, 2, 1, 0, 0 };
				yield return new object[] { "49 C1 F8 A5", Code.Sar_Eq_Ib, 3, 1, 0, 0 };
				yield return new object[] { "C2 5AA5", Code.Retnq_Iw, 1, 2, 0, 0 };
				yield return new object[] { "C6 C1 5A", Code.Mov_Eb_Ib, 2, 1, 0, 0 };
				yield return new object[] { "C6 F8 5A", Code.Xabort_Ib, 2, 1, 0, 0 };
				yield return new object[] { "66 C7 C1 5AA5", Code.Mov_Ew_Iw, 3, 2, 0, 0 };
				yield return new object[] { "C7 C1 5AA51234", Code.Mov_Ed_Id, 2, 4, 0, 0 };
				yield return new object[] { "48 C7 C1 5AA51284", Code.Mov_Eq_Id64, 3, 4, 0, 0 };
				yield return new object[] { "66 C8 5AA5 A6", Code.Enterw_Iw_Ib, 2, 2, 0, 0 };
				yield return new object[] { "C8 5AA5 A6", Code.Enterq_Iw_Ib, 1, 2, 0, 0 };
				yield return new object[] { "66 CA 5AA5", Code.Retfw_Iw, 2, 2, 0, 0 };
				yield return new object[] { "CA 5AA5", Code.Retfd_Iw, 1, 2, 0, 0 };
				yield return new object[] { "48 CA 5AA5", Code.Retfq_Iw, 2, 2, 0, 0 };
				yield return new object[] { "CD 5A", Code.Int_Ib, 1, 1, 0, 0 };
				yield return new object[] { "E4 5A", Code.In_AL_Ib, 1, 1, 0, 0 };
				yield return new object[] { "66 E5 5A", Code.In_AX_Ib, 2, 1, 0, 0 };
				yield return new object[] { "E5 5A", Code.In_EAX_Ib, 1, 1, 0, 0 };
				yield return new object[] { "E6 5A", Code.Out_Ib_AL, 1, 1, 0, 0 };
				yield return new object[] { "66 E7 5A", Code.Out_Ib_AX, 2, 1, 0, 0 };
				yield return new object[] { "E7 5A", Code.Out_Ib_EAX, 1, 1, 0, 0 };
				yield return new object[] { "F6 C1 5A", Code.Test_Eb_Ib, 2, 1, 0, 0 };
				yield return new object[] { "66 F7 C1 5AA5", Code.Test_Ew_Iw, 3, 2, 0, 0 };
				yield return new object[] { "F7 C1 5AA51234", Code.Test_Ed_Id, 2, 4, 0, 0 };
				yield return new object[] { "48 F7 C1 5AA51284", Code.Test_Eq_Id64, 3, 4, 0, 0 };
				yield return new object[] { "0F70 CD A5", Code.Pshufw_P_Q_Ib, 3, 1, 0, 0 };
				yield return new object[] { "66 0F70 CD A5", Code.Pshufd_VX_WX_Ib, 4, 1, 0, 0 };
				yield return new object[] { "C5F9 70 D3 A5", Code.VEX_Vpshufd_VX_WX_Ib, 4, 1, 0, 0 };
				yield return new object[] { "C5FD 70 D3 A5", Code.VEX_Vpshufd_VY_WY_Ib, 4, 1, 0, 0 };
				yield return new object[] { "62 F17D8B 70 D3 A5", Code.EVEX_Vpshufd_VX_k1z_WX_Ib_b, 6, 1, 0, 0 };
				yield return new object[] { "62 F17DAB 70 D3 A5", Code.EVEX_Vpshufd_VY_k1z_WY_Ib_b, 6, 1, 0, 0 };
				yield return new object[] { "62 F17DCB 70 D3 A5", Code.EVEX_Vpshufd_VZ_k1z_WZ_Ib_b, 6, 1, 0, 0 };
				yield return new object[] { "66 0FA4 CE 5A", Code.Shld_Ew_Gw_Ib, 4, 1, 0, 0 };
				yield return new object[] { "0FA4 CE 5A", Code.Shld_Ed_Gd_Ib, 3, 1, 0, 0 };
				yield return new object[] { "48 0FA4 CE 5A", Code.Shld_Eq_Gq_Ib, 4, 1, 0, 0 };
				yield return new object[] { "66 0FAC CE 5A", Code.Shrd_Ew_Gw_Ib, 4, 1, 0, 0 };
				yield return new object[] { "0FAC CE 5A", Code.Shrd_Ed_Gd_Ib, 3, 1, 0, 0 };
				yield return new object[] { "48 0FAC CE 5A", Code.Shrd_Eq_Gq_Ib, 4, 1, 0, 0 };
				yield return new object[] { "66 0FBA E5 5A", Code.Bt_Ew_Ib, 4, 1, 0, 0 };
				yield return new object[] { "0FBA E5 5A", Code.Bt_Ed_Ib, 3, 1, 0, 0 };
				yield return new object[] { "48 0FBA E5 5A", Code.Bt_Eq_Ib, 4, 1, 0, 0 };
				yield return new object[] { "66 0FBA EE A5", Code.Bts_Ew_Ib, 4, 1, 0, 0 };
				yield return new object[] { "0FBA EE A5", Code.Bts_Ed_Ib, 3, 1, 0, 0 };
				yield return new object[] { "48 0FBA EE A5", Code.Bts_Eq_Ib, 4, 1, 0, 0 };
				yield return new object[] { "66 0FBA F7 5A", Code.Btr_Ew_Ib, 4, 1, 0, 0 };
				yield return new object[] { "0FBA F7 5A", Code.Btr_Ed_Ib, 3, 1, 0, 0 };
				yield return new object[] { "48 0FBA F7 5A", Code.Btr_Eq_Ib, 4, 1, 0, 0 };
				yield return new object[] { "66 0FBA F8 A5", Code.Btc_Ew_Ib, 4, 1, 0, 0 };
				yield return new object[] { "0FBA F8 A5", Code.Btc_Ed_Ib, 3, 1, 0, 0 };
				yield return new object[] { "49 0FBA F8 A5", Code.Btc_Eq_Ib, 4, 1, 0, 0 };
				yield return new object[] { "C4E37B F0 D3 A5", Code.VEX_Rorx_Gd_Ed_Ib, 5, 1, 0, 0 };
				yield return new object[] { "C4E3FB F0 D3 A5", Code.VEX_Rorx_Gq_Eq_Ib, 5, 1, 0, 0 };

				yield return new object[] { "67 A0 11223344", Code.Mov_AL_Ob, 0, 0, 2, 4 };
				yield return new object[] { "66 67 A1 11223344", Code.Mov_AX_Ow, 0, 0, 3, 4 };
				yield return new object[] { "67 A1 11223344", Code.Mov_EAX_Od, 0, 0, 2, 4 };
				yield return new object[] { "67 48 A1 11223344", Code.Mov_RAX_Oq, 0, 0, 3, 4 };
				yield return new object[] { "67 A2 11223344", Code.Mov_Ob_AL, 0, 0, 2, 4 };
				yield return new object[] { "66 67 A3 11223344", Code.Mov_Ow_AX, 0, 0, 3, 4 };
				yield return new object[] { "67 A3 11223344", Code.Mov_Od_EAX, 0, 0, 2, 4 };
				yield return new object[] { "67 48 A3 11223344", Code.Mov_Oq_RAX, 0, 0, 3, 4 };

				yield return new object[] { "A0 1122334455667788", Code.Mov_AL_Ob, 0, 0, 1, 8 };
				yield return new object[] { "66 A1 1122334455667788", Code.Mov_AX_Ow, 0, 0, 2, 8 };
				yield return new object[] { "A1 1122334455667788", Code.Mov_EAX_Od, 0, 0, 1, 8 };
				yield return new object[] { "48 A1 1122334455667788", Code.Mov_RAX_Oq, 0, 0, 2, 8 };
				yield return new object[] { "A2 1122334455667788", Code.Mov_Ob_AL, 0, 0, 1, 8 };
				yield return new object[] { "66 A3 1122334455667788", Code.Mov_Ow_AX, 0, 0, 2, 8 };
				yield return new object[] { "A3 1122334455667788", Code.Mov_Od_EAX, 0, 0, 1, 8 };
				yield return new object[] { "48 A3 1122334455667788", Code.Mov_Oq_RAX, 0, 0, 2, 8 };

				yield return new object[] { "67 01 08", Code.Add_Ed_Gd, 0, 0, 0, 0 };
				yield return new object[] { "67 01 48 5A", Code.Add_Ed_Gd, 0, 0, 3, 1 };
				yield return new object[] { "67 01 88 44332211", Code.Add_Ed_Gd, 0, 0, 3, 4 };
				yield return new object[] { "67 01 0C 10", Code.Add_Ed_Gd, 0, 0, 0, 0 };
				yield return new object[] { "67 01 4C 10 5A", Code.Add_Ed_Gd, 0, 0, 4, 1 };
				yield return new object[] { "67 01 8C 10 44332211", Code.Add_Ed_Gd, 0, 0, 4, 4 };
				yield return new object[] { "67 01 0D 44332211", Code.Add_Ed_Gd, 0, 0, 3, 4 };

				yield return new object[] { "48 01 08", Code.Add_Eq_Gq, 0, 0, 0, 0 };
				yield return new object[] { "48 01 48 5A", Code.Add_Eq_Gq, 0, 0, 3, 1 };
				yield return new object[] { "48 01 88 44332211", Code.Add_Eq_Gq, 0, 0, 3, 4 };
				yield return new object[] { "48 01 0C 10", Code.Add_Eq_Gq, 0, 0, 0, 0 };
				yield return new object[] { "48 01 4C 10 5A", Code.Add_Eq_Gq, 0, 0, 4, 1 };
				yield return new object[] { "48 01 8C 10 44332211", Code.Add_Eq_Gq, 0, 0, 4, 4 };
				yield return new object[] { "48 01 0D 44332211", Code.Add_Eq_Gq, 0, 0, 3, 4 };

				yield return new object[] { "80 80 78563412 9A", Code.Add_Eb_Ib, 6, 1, 2, 4 };
				yield return new object[] { "66 81 40 12 5634", Code.Add_Ew_Iw, 4, 2, 3, 1 };
				yield return new object[] { "81 40 12 9A785634", Code.Add_Ed_Id, 3, 4, 2, 1 };

				yield return new object[] { "62 F2CD2B 3B 50 01", Code.EVEX_Vpminuq_VY_k1z_HY_WY_b, 0, 0, 6, 1 };

				yield return new object[] { "CC", Code.Int3, 0, 0, 0, 0 };
				yield return new object[] { "F1", Code.Int1, 0, 0, 0, 0 };
				yield return new object[] { "D0 C1", Code.Rol_Eb_1, 0, 0, 0, 0 };
				yield return new object[] { "D0 CA", Code.Ror_Eb_1, 0, 0, 0, 0 };
				yield return new object[] { "D0 D3", Code.Rcl_Eb_1, 0, 0, 0, 0 };
				yield return new object[] { "D0 DC", Code.Rcr_Eb_1, 0, 0, 0, 0 };
				yield return new object[] { "D0 E5", Code.Shl_Eb_1, 0, 0, 0, 0 };
				yield return new object[] { "D0 EE", Code.Shr_Eb_1, 0, 0, 0, 0 };
				yield return new object[] { "D0 F8", Code.Sar_Eb_1, 0, 0, 0, 0 };
				yield return new object[] { "66 D1 C1", Code.Rol_Ew_1, 0, 0, 0, 0 };
				yield return new object[] { "D1 C1", Code.Rol_Ed_1, 0, 0, 0, 0 };
				yield return new object[] { "48 D1 C1", Code.Rol_Eq_1, 0, 0, 0, 0 };
				yield return new object[] { "66 D1 CA", Code.Ror_Ew_1, 0, 0, 0, 0 };
				yield return new object[] { "D1 CA", Code.Ror_Ed_1, 0, 0, 0, 0 };
				yield return new object[] { "48 D1 CA", Code.Ror_Eq_1, 0, 0, 0, 0 };
				yield return new object[] { "66 D1 D3", Code.Rcl_Ew_1, 0, 0, 0, 0 };
				yield return new object[] { "D1 D3", Code.Rcl_Ed_1, 0, 0, 0, 0 };
				yield return new object[] { "48 D1 D3", Code.Rcl_Eq_1, 0, 0, 0, 0 };
				yield return new object[] { "66 D1 DC", Code.Rcr_Ew_1, 0, 0, 0, 0 };
				yield return new object[] { "D1 DC", Code.Rcr_Ed_1, 0, 0, 0, 0 };
				yield return new object[] { "48 D1 DC", Code.Rcr_Eq_1, 0, 0, 0, 0 };
				yield return new object[] { "66 D1 E5", Code.Shl_Ew_1, 0, 0, 0, 0 };
				yield return new object[] { "D1 E5", Code.Shl_Ed_1, 0, 0, 0, 0 };
				yield return new object[] { "48 D1 E5", Code.Shl_Eq_1, 0, 0, 0, 0 };
				yield return new object[] { "66 D1 EE", Code.Shr_Ew_1, 0, 0, 0, 0 };
				yield return new object[] { "D1 EE", Code.Shr_Ed_1, 0, 0, 0, 0 };
				yield return new object[] { "48 D1 EE", Code.Shr_Eq_1, 0, 0, 0, 0 };
				yield return new object[] { "66 D1 F8", Code.Sar_Ew_1, 0, 0, 0, 0 };
				yield return new object[] { "D1 F8", Code.Sar_Ed_1, 0, 0, 0, 0 };
				yield return new object[] { "49 D1 F8", Code.Sar_Eq_1, 0, 0, 0, 0 };
			}
		}
	}
}
