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
				yield return new object[] { "68 5AA51234", Code.Pushd_imm32, 1, 4, 0, 0 };
				yield return new object[] { "6A A5", Code.Pushd_imm8, 1, 1, 0, 0 };
				yield return new object[] { "66 C2 5AA5", Code.Retnw_imm16, 2, 2, 0, 0 };
				yield return new object[] { "C2 5AA5", Code.Retnd_imm16, 1, 2, 0, 0 };
				yield return new object[] { "C8 5AA5 A6", Code.Enterd_imm16_imm8, 1, 2, 0, 0 };
				yield return new object[] { "D4 0A", Code.Aam_imm8, 1, 1, 0, 0 };
				yield return new object[] { "D5 0A", Code.Aad_imm8, 1, 1, 0, 0 };

				yield return new object[] { "66 9A 1122 3344", Code.Call_ptr1616, 2, 2, 0, 0 };
				yield return new object[] { "9A 11223344 5566", Code.Call_ptr3216, 1, 4, 0, 0 };
				yield return new object[] { "66 EA 1122 3344", Code.Jmp_ptr1616, 2, 2, 0, 0 };
				yield return new object[] { "EA 11223344 5566", Code.Jmp_ptr3216, 1, 4, 0, 0 };

				yield return new object[] { "67 A0 1122", Code.Mov_AL_moffs8, 0, 0, 2, 2 };
				yield return new object[] { "66 67 A1 1122", Code.Mov_AX_moffs16, 0, 0, 3, 2 };
				yield return new object[] { "67 A1 1122", Code.Mov_EAX_moffs32, 0, 0, 2, 2 };
				yield return new object[] { "67 A2 1122", Code.Mov_moffs8_AL, 0, 0, 2, 2 };
				yield return new object[] { "66 67 A3 1122", Code.Mov_moffs16_AX, 0, 0, 3, 2 };
				yield return new object[] { "67 A3 1122", Code.Mov_moffs32_EAX, 0, 0, 2, 2 };

				yield return new object[] { "A0 11223344", Code.Mov_AL_moffs8, 0, 0, 1, 4 };
				yield return new object[] { "66 A1 11223344", Code.Mov_AX_moffs16, 0, 0, 2, 4 };
				yield return new object[] { "A1 11223344", Code.Mov_EAX_moffs32, 0, 0, 1, 4 };
				yield return new object[] { "A2 11223344", Code.Mov_moffs8_AL, 0, 0, 1, 4 };
				yield return new object[] { "66 A3 11223344", Code.Mov_moffs16_AX, 0, 0, 2, 4 };
				yield return new object[] { "A3 11223344", Code.Mov_moffs32_EAX, 0, 0, 1, 4 };

				yield return new object[] { "66 67 01 0F", Code.Add_rm16_r16, 0, 0, 0, 0 };
				yield return new object[] { "66 67 01 4F 12", Code.Add_rm16_r16, 0, 0, 4, 1 };
				yield return new object[] { "66 67 01 8F 3412", Code.Add_rm16_r16, 0, 0, 4, 2 };

				yield return new object[] { "01 08", Code.Add_rm32_r32, 0, 0, 0, 0 };
				yield return new object[] { "01 48 5A", Code.Add_rm32_r32, 0, 0, 2, 1 };
				yield return new object[] { "01 88 44332211", Code.Add_rm32_r32, 0, 0, 2, 4 };
				yield return new object[] { "01 0C 10", Code.Add_rm32_r32, 0, 0, 0, 0 };
				yield return new object[] { "01 4C 10 5A", Code.Add_rm32_r32, 0, 0, 3, 1 };
				yield return new object[] { "01 8C 10 44332211", Code.Add_rm32_r32, 0, 0, 3, 4 };
				yield return new object[] { "01 0D 44332211", Code.Add_rm32_r32, 0, 0, 2, 4 };

				yield return new object[] { "67 81 47 5A 11223344", Code.Add_rm32_imm32, 4, 4, 3, 1 };
				yield return new object[] { "67 81 87 5AA5 22334455", Code.Add_rm32_imm32, 5, 4, 3, 2 };

				yield return new object[] { "80 80 78563412 9A", Code.Add_rm8_imm8, 6, 1, 2, 4 };
				yield return new object[] { "66 81 40 12 5634", Code.Add_rm16_imm16, 4, 2, 3, 1 };
				yield return new object[] { "81 40 12 9A785634", Code.Add_rm32_imm32, 3, 4, 2, 1 };

				yield return new object[] { "CC", Code.Int3, 0, 0, 0, 0 };
				yield return new object[] { "F1", Code.Int1, 0, 0, 0, 0 };
				yield return new object[] { "D0 C1", Code.Rol_rm8_1, 0, 0, 0, 0 };
				yield return new object[] { "D0 CA", Code.Ror_rm8_1, 0, 0, 0, 0 };
				yield return new object[] { "D0 D3", Code.Rcl_rm8_1, 0, 0, 0, 0 };
				yield return new object[] { "D0 DC", Code.Rcr_rm8_1, 0, 0, 0, 0 };
				yield return new object[] { "D0 E5", Code.Shl_rm8_1, 0, 0, 0, 0 };
				yield return new object[] { "D0 EE", Code.Shr_rm8_1, 0, 0, 0, 0 };
				yield return new object[] { "D0 F8", Code.Sar_rm8_1, 0, 0, 0, 0 };
				yield return new object[] { "66 D1 C1", Code.Rol_rm16_1, 0, 0, 0, 0 };
				yield return new object[] { "D1 C1", Code.Rol_rm32_1, 0, 0, 0, 0 };
				yield return new object[] { "66 D1 CA", Code.Ror_rm16_1, 0, 0, 0, 0 };
				yield return new object[] { "D1 CA", Code.Ror_rm32_1, 0, 0, 0, 0 };
				yield return new object[] { "66 D1 D3", Code.Rcl_rm16_1, 0, 0, 0, 0 };
				yield return new object[] { "D1 D3", Code.Rcl_rm32_1, 0, 0, 0, 0 };
				yield return new object[] { "66 D1 DC", Code.Rcr_rm16_1, 0, 0, 0, 0 };
				yield return new object[] { "D1 DC", Code.Rcr_rm32_1, 0, 0, 0, 0 };
				yield return new object[] { "66 D1 E5", Code.Shl_rm16_1, 0, 0, 0, 0 };
				yield return new object[] { "D1 E5", Code.Shl_rm32_1, 0, 0, 0, 0 };
				yield return new object[] { "66 D1 EE", Code.Shr_rm16_1, 0, 0, 0, 0 };
				yield return new object[] { "D1 EE", Code.Shr_rm32_1, 0, 0, 0, 0 };
				yield return new object[] { "66 D1 F8", Code.Sar_rm16_1, 0, 0, 0, 0 };
				yield return new object[] { "D1 F8", Code.Sar_rm32_1, 0, 0, 0, 0 };
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
				yield return new object[] { "04 A5", Code.Add_AL_imm8, 1, 1, 0, 0 };
				yield return new object[] { "66 05 A55A", Code.Add_AX_imm16, 2, 2, 0, 0 };
				yield return new object[] { "05 A55A3412", Code.Add_EAX_imm32, 1, 4, 0, 0 };
				yield return new object[] { "48 05 A55A34A2", Code.Add_RAX_imm32, 2, 4, 0, 0 };
				yield return new object[] { "0C A5", Code.Or_AL_imm8, 1, 1, 0, 0 };
				yield return new object[] { "66 0D A55A", Code.Or_AX_imm16, 2, 2, 0, 0 };
				yield return new object[] { "0D A55A3412", Code.Or_EAX_imm32, 1, 4, 0, 0 };
				yield return new object[] { "48 0D A55A34A2", Code.Or_RAX_imm32, 2, 4, 0, 0 };
				yield return new object[] { "14 A5", Code.Adc_AL_imm8, 1, 1, 0, 0 };
				yield return new object[] { "66 15 A55A", Code.Adc_AX_imm16, 2, 2, 0, 0 };
				yield return new object[] { "15 A55A3412", Code.Adc_EAX_imm32, 1, 4, 0, 0 };
				yield return new object[] { "48 15 A55A34A2", Code.Adc_RAX_imm32, 2, 4, 0, 0 };
				yield return new object[] { "1C A5", Code.Sbb_AL_imm8, 1, 1, 0, 0 };
				yield return new object[] { "66 1D A55A", Code.Sbb_AX_imm16, 2, 2, 0, 0 };
				yield return new object[] { "1D A55A3412", Code.Sbb_EAX_imm32, 1, 4, 0, 0 };
				yield return new object[] { "48 1D A55A3482", Code.Sbb_RAX_imm32, 2, 4, 0, 0 };
				yield return new object[] { "24 A5", Code.And_AL_imm8, 1, 1, 0, 0 };
				yield return new object[] { "66 25 A55A", Code.And_AX_imm16, 2, 2, 0, 0 };
				yield return new object[] { "25 A55A3412", Code.And_EAX_imm32, 1, 4, 0, 0 };
				yield return new object[] { "48 25 A55A3482", Code.And_RAX_imm32, 2, 4, 0, 0 };
				yield return new object[] { "2C A5", Code.Sub_AL_imm8, 1, 1, 0, 0 };
				yield return new object[] { "66 2D A55A", Code.Sub_AX_imm16, 2, 2, 0, 0 };
				yield return new object[] { "2D A55A3412", Code.Sub_EAX_imm32, 1, 4, 0, 0 };
				yield return new object[] { "48 2D A55A3482", Code.Sub_RAX_imm32, 2, 4, 0, 0 };
				yield return new object[] { "34 A5", Code.Xor_AL_imm8, 1, 1, 0, 0 };
				yield return new object[] { "66 35 A55A", Code.Xor_AX_imm16, 2, 2, 0, 0 };
				yield return new object[] { "35 A55A3412", Code.Xor_EAX_imm32, 1, 4, 0, 0 };
				yield return new object[] { "48 35 A55A3482", Code.Xor_RAX_imm32, 2, 4, 0, 0 };
				yield return new object[] { "3C A5", Code.Cmp_AL_imm8, 1, 1, 0, 0 };
				yield return new object[] { "66 3D A55A", Code.Cmp_AX_imm16, 2, 2, 0, 0 };
				yield return new object[] { "3D A55A3412", Code.Cmp_EAX_imm32, 1, 4, 0, 0 };
				yield return new object[] { "48 3D A55A3482", Code.Cmp_RAX_imm32, 2, 4, 0, 0 };
				yield return new object[] { "66 68 5AA5", Code.Push_imm16, 2, 2, 0, 0 };
				yield return new object[] { "68 5AA51284", Code.Pushq_imm32, 1, 4, 0, 0 };
				yield return new object[] { "66 69 CE A55A", Code.Imul_r16_rm16_imm16, 3, 2, 0, 0 };
				yield return new object[] { "69 CE 5AA51234", Code.Imul_r32_rm32_imm32, 2, 4, 0, 0 };
				yield return new object[] { "48 69 CE 5AA51284", Code.Imul_r64_rm64_imm32, 3, 4, 0, 0 };
				yield return new object[] { "66 6A A5", Code.Pushw_imm8, 2, 1, 0, 0 };
				yield return new object[] { "6A A5", Code.Pushq_imm8, 1, 1, 0, 0 };
				yield return new object[] { "66 6B CE A5", Code.Imul_r16_rm16_imm8, 3, 1, 0, 0 };
				yield return new object[] { "6B CE A5", Code.Imul_r32_rm32_imm8, 2, 1, 0, 0 };
				yield return new object[] { "48 6B CE A5", Code.Imul_r64_rm64_imm8, 3, 1, 0, 0 };
				yield return new object[] { "80 C1 5A", Code.Add_rm8_imm8, 2, 1, 0, 0 };
				yield return new object[] { "80 CA A5", Code.Or_rm8_imm8, 2, 1, 0, 0 };
				yield return new object[] { "80 D3 5A", Code.Adc_rm8_imm8, 2, 1, 0, 0 };
				yield return new object[] { "80 DC A5", Code.Sbb_rm8_imm8, 2, 1, 0, 0 };
				yield return new object[] { "80 E5 5A", Code.And_rm8_imm8, 2, 1, 0, 0 };
				yield return new object[] { "80 EE A5", Code.Sub_rm8_imm8, 2, 1, 0, 0 };
				yield return new object[] { "80 F7 5A", Code.Xor_rm8_imm8, 2, 1, 0, 0 };
				yield return new object[] { "80 F8 A5", Code.Cmp_rm8_imm8, 2, 1, 0, 0 };
				yield return new object[] { "66 81 C1 5AA5", Code.Add_rm16_imm16, 3, 2, 0, 0 };
				yield return new object[] { "81 C1 5AA51234", Code.Add_rm32_imm32, 2, 4, 0, 0 };
				yield return new object[] { "48 81 C1 5AA51284", Code.Add_rm64_imm32, 3, 4, 0, 0 };
				yield return new object[] { "66 81 CA A55A", Code.Or_rm16_imm16, 3, 2, 0, 0 };
				yield return new object[] { "81 CA A55A89AB", Code.Or_rm32_imm32, 2, 4, 0, 0 };
				yield return new object[] { "48 81 CA 5AA51284", Code.Or_rm64_imm32, 3, 4, 0, 0 };
				yield return new object[] { "66 81 D3 5AA5", Code.Adc_rm16_imm16, 3, 2, 0, 0 };
				yield return new object[] { "81 D3 5AA51234", Code.Adc_rm32_imm32, 2, 4, 0, 0 };
				yield return new object[] { "48 81 D3 5AA51284", Code.Adc_rm64_imm32, 3, 4, 0, 0 };
				yield return new object[] { "66 81 DC A55A", Code.Sbb_rm16_imm16, 3, 2, 0, 0 };
				yield return new object[] { "81 DC A55A89AB", Code.Sbb_rm32_imm32, 2, 4, 0, 0 };
				yield return new object[] { "48 81 DC 5AA51284", Code.Sbb_rm64_imm32, 3, 4, 0, 0 };
				yield return new object[] { "66 81 E5 5AA5", Code.And_rm16_imm16, 3, 2, 0, 0 };
				yield return new object[] { "81 E5 5AA51234", Code.And_rm32_imm32, 2, 4, 0, 0 };
				yield return new object[] { "48 81 E5 5AA51284", Code.And_rm64_imm32, 3, 4, 0, 0 };
				yield return new object[] { "66 81 EE A55A", Code.Sub_rm16_imm16, 3, 2, 0, 0 };
				yield return new object[] { "81 EE A55A89AB", Code.Sub_rm32_imm32, 2, 4, 0, 0 };
				yield return new object[] { "48 81 EE 5AA51284", Code.Sub_rm64_imm32, 3, 4, 0, 0 };
				yield return new object[] { "66 81 F7 5AA5", Code.Xor_rm16_imm16, 3, 2, 0, 0 };
				yield return new object[] { "81 F7 5AA51234", Code.Xor_rm32_imm32, 2, 4, 0, 0 };
				yield return new object[] { "48 81 F7 5AA51284", Code.Xor_rm64_imm32, 3, 4, 0, 0 };
				yield return new object[] { "66 81 F8 A55A", Code.Cmp_rm16_imm16, 3, 2, 0, 0 };
				yield return new object[] { "81 F8 A55A89AB", Code.Cmp_rm32_imm32, 2, 4, 0, 0 };
				yield return new object[] { "49 81 F8 5AA51284", Code.Cmp_rm64_imm32, 3, 4, 0, 0 };
				yield return new object[] { "66 83 C1 A5", Code.Add_rm16_imm8, 3, 1, 0, 0 };
				yield return new object[] { "83 C1 A5", Code.Add_rm32_imm8, 2, 1, 0, 0 };
				yield return new object[] { "48 83 C1 A5", Code.Add_rm64_imm8, 3, 1, 0, 0 };
				yield return new object[] { "66 83 CA A5", Code.Or_rm16_imm8, 3, 1, 0, 0 };
				yield return new object[] { "83 CA A5", Code.Or_rm32_imm8, 2, 1, 0, 0 };
				yield return new object[] { "48 83 CA A5", Code.Or_rm64_imm8, 3, 1, 0, 0 };
				yield return new object[] { "66 83 D3 A5", Code.Adc_rm16_imm8, 3, 1, 0, 0 };
				yield return new object[] { "83 D3 A5", Code.Adc_rm32_imm8, 2, 1, 0, 0 };
				yield return new object[] { "48 83 D3 A5", Code.Adc_rm64_imm8, 3, 1, 0, 0 };
				yield return new object[] { "66 83 DC A5", Code.Sbb_rm16_imm8, 3, 1, 0, 0 };
				yield return new object[] { "83 DC A5", Code.Sbb_rm32_imm8, 2, 1, 0, 0 };
				yield return new object[] { "48 83 DC A5", Code.Sbb_rm64_imm8, 3, 1, 0, 0 };
				yield return new object[] { "66 83 E5 A5", Code.And_rm16_imm8, 3, 1, 0, 0 };
				yield return new object[] { "83 E5 A5", Code.And_rm32_imm8, 2, 1, 0, 0 };
				yield return new object[] { "48 83 E5 A5", Code.And_rm64_imm8, 3, 1, 0, 0 };
				yield return new object[] { "66 83 EE A5", Code.Sub_rm16_imm8, 3, 1, 0, 0 };
				yield return new object[] { "83 EE A5", Code.Sub_rm32_imm8, 2, 1, 0, 0 };
				yield return new object[] { "48 83 EE A5", Code.Sub_rm64_imm8, 3, 1, 0, 0 };
				yield return new object[] { "66 83 F7 A5", Code.Xor_rm16_imm8, 3, 1, 0, 0 };
				yield return new object[] { "83 F7 A5", Code.Xor_rm32_imm8, 2, 1, 0, 0 };
				yield return new object[] { "48 83 F7 A5", Code.Xor_rm64_imm8, 3, 1, 0, 0 };
				yield return new object[] { "66 83 F8 A5", Code.Cmp_rm16_imm8, 3, 1, 0, 0 };
				yield return new object[] { "83 F8 A5", Code.Cmp_rm32_imm8, 2, 1, 0, 0 };
				yield return new object[] { "49 83 F8 A5", Code.Cmp_rm64_imm8, 3, 1, 0, 0 };
				yield return new object[] { "A8 A5", Code.Test_AL_imm8, 1, 1, 0, 0 };
				yield return new object[] { "66 A9 A55A", Code.Test_AX_imm16, 2, 2, 0, 0 };
				yield return new object[] { "A9 A55A3412", Code.Test_EAX_imm32, 1, 4, 0, 0 };
				yield return new object[] { "48 A9 A55A3482", Code.Test_RAX_imm32, 2, 4, 0, 0 };
				yield return new object[] { "B0 5A", Code.Mov_r8_imm8, 1, 1, 0, 0 };
				yield return new object[] { "41 B0 5A", Code.Mov_r8_imm8, 2, 1, 0, 0 };
				yield return new object[] { "B1 A5", Code.Mov_r8_imm8, 1, 1, 0, 0 };
				yield return new object[] { "41 B1 A5", Code.Mov_r8_imm8, 2, 1, 0, 0 };
				yield return new object[] { "B2 5A", Code.Mov_r8_imm8, 1, 1, 0, 0 };
				yield return new object[] { "41 B2 5A", Code.Mov_r8_imm8, 2, 1, 0, 0 };
				yield return new object[] { "B3 A5", Code.Mov_r8_imm8, 1, 1, 0, 0 };
				yield return new object[] { "41 B3 A5", Code.Mov_r8_imm8, 2, 1, 0, 0 };
				yield return new object[] { "B4 5A", Code.Mov_r8_imm8, 1, 1, 0, 0 };
				yield return new object[] { "40 B4 5A", Code.Mov_r8_imm8, 2, 1, 0, 0 };
				yield return new object[] { "41 B4 5A", Code.Mov_r8_imm8, 2, 1, 0, 0 };
				yield return new object[] { "B5 A5", Code.Mov_r8_imm8, 1, 1, 0, 0 };
				yield return new object[] { "40 B5 A5", Code.Mov_r8_imm8, 2, 1, 0, 0 };
				yield return new object[] { "41 B5 A5", Code.Mov_r8_imm8, 2, 1, 0, 0 };
				yield return new object[] { "B6 5A", Code.Mov_r8_imm8, 1, 1, 0, 0 };
				yield return new object[] { "40 B6 5A", Code.Mov_r8_imm8, 2, 1, 0, 0 };
				yield return new object[] { "41 B6 5A", Code.Mov_r8_imm8, 2, 1, 0, 0 };
				yield return new object[] { "B7 A5", Code.Mov_r8_imm8, 1, 1, 0, 0 };
				yield return new object[] { "40 B7 A5", Code.Mov_r8_imm8, 2, 1, 0, 0 };
				yield return new object[] { "41 B7 A5", Code.Mov_r8_imm8, 2, 1, 0, 0 };
				yield return new object[] { "66 B8 5AA5", Code.Mov_r16_imm16, 2, 2, 0, 0 };
				yield return new object[] { "66 41 B8 5AA5", Code.Mov_r16_imm16, 3, 2, 0, 0 };
				yield return new object[] { "B8 5AA51234", Code.Mov_r32_imm32, 1, 4, 0, 0 };
				yield return new object[] { "41 B8 5AA51234", Code.Mov_r32_imm32, 2, 4, 0, 0 };
				yield return new object[] { "48 B8 041526375AA51234", Code.Mov_r64_imm64, 2, 8, 0, 0 };
				yield return new object[] { "49 B8 041526375AA51234", Code.Mov_r64_imm64, 2, 8, 0, 0 };
				yield return new object[] { "66 B9 A55A", Code.Mov_r16_imm16, 2, 2, 0, 0 };
				yield return new object[] { "66 41 B9 A55A", Code.Mov_r16_imm16, 3, 2, 0, 0 };
				yield return new object[] { "B9 A55A5678", Code.Mov_r32_imm32, 1, 4, 0, 0 };
				yield return new object[] { "41 B9 A55A5678", Code.Mov_r32_imm32, 2, 4, 0, 0 };
				yield return new object[] { "48 B9 04152637A55A5678", Code.Mov_r64_imm64, 2, 8, 0, 0 };
				yield return new object[] { "49 B9 04152637A55A5678", Code.Mov_r64_imm64, 2, 8, 0, 0 };
				yield return new object[] { "66 BA 5AA5", Code.Mov_r16_imm16, 2, 2, 0, 0 };
				yield return new object[] { "66 41 BA 5AA5", Code.Mov_r16_imm16, 3, 2, 0, 0 };
				yield return new object[] { "BA 5AA51234", Code.Mov_r32_imm32, 1, 4, 0, 0 };
				yield return new object[] { "41 BA 5AA51234", Code.Mov_r32_imm32, 2, 4, 0, 0 };
				yield return new object[] { "48 BA 041526375AA51234", Code.Mov_r64_imm64, 2, 8, 0, 0 };
				yield return new object[] { "49 BA 041526375AA51234", Code.Mov_r64_imm64, 2, 8, 0, 0 };
				yield return new object[] { "66 BB A55A", Code.Mov_r16_imm16, 2, 2, 0, 0 };
				yield return new object[] { "66 41 BB A55A", Code.Mov_r16_imm16, 3, 2, 0, 0 };
				yield return new object[] { "BB A55A5678", Code.Mov_r32_imm32, 1, 4, 0, 0 };
				yield return new object[] { "41 BB A55A5678", Code.Mov_r32_imm32, 2, 4, 0, 0 };
				yield return new object[] { "48 BB 04152637A55A5678", Code.Mov_r64_imm64, 2, 8, 0, 0 };
				yield return new object[] { "49 BB 04152637A55A5678", Code.Mov_r64_imm64, 2, 8, 0, 0 };
				yield return new object[] { "66 BC 5AA5", Code.Mov_r16_imm16, 2, 2, 0, 0 };
				yield return new object[] { "66 41 BC 5AA5", Code.Mov_r16_imm16, 3, 2, 0, 0 };
				yield return new object[] { "BC 5AA51234", Code.Mov_r32_imm32, 1, 4, 0, 0 };
				yield return new object[] { "41 BC 5AA51234", Code.Mov_r32_imm32, 2, 4, 0, 0 };
				yield return new object[] { "48 BC 041526375AA51234", Code.Mov_r64_imm64, 2, 8, 0, 0 };
				yield return new object[] { "49 BC 041526375AA51234", Code.Mov_r64_imm64, 2, 8, 0, 0 };
				yield return new object[] { "66 BD A55A", Code.Mov_r16_imm16, 2, 2, 0, 0 };
				yield return new object[] { "66 41 BD A55A", Code.Mov_r16_imm16, 3, 2, 0, 0 };
				yield return new object[] { "BD A55A5678", Code.Mov_r32_imm32, 1, 4, 0, 0 };
				yield return new object[] { "41 BD A55A5678", Code.Mov_r32_imm32, 2, 4, 0, 0 };
				yield return new object[] { "48 BD 04152637A55A5678", Code.Mov_r64_imm64, 2, 8, 0, 0 };
				yield return new object[] { "49 BD 04152637A55A5678", Code.Mov_r64_imm64, 2, 8, 0, 0 };
				yield return new object[] { "66 BE 5AA5", Code.Mov_r16_imm16, 2, 2, 0, 0 };
				yield return new object[] { "66 41 BE 5AA5", Code.Mov_r16_imm16, 3, 2, 0, 0 };
				yield return new object[] { "BE 5AA51234", Code.Mov_r32_imm32, 1, 4, 0, 0 };
				yield return new object[] { "41 BE 5AA51234", Code.Mov_r32_imm32, 2, 4, 0, 0 };
				yield return new object[] { "48 BE 041526375AA51234", Code.Mov_r64_imm64, 2, 8, 0, 0 };
				yield return new object[] { "49 BE 041526375AA51234", Code.Mov_r64_imm64, 2, 8, 0, 0 };
				yield return new object[] { "66 BF A55A", Code.Mov_r16_imm16, 2, 2, 0, 0 };
				yield return new object[] { "66 41 BF A55A", Code.Mov_r16_imm16, 3, 2, 0, 0 };
				yield return new object[] { "BF A55A5678", Code.Mov_r32_imm32, 1, 4, 0, 0 };
				yield return new object[] { "41 BF A55A5678", Code.Mov_r32_imm32, 2, 4, 0, 0 };
				yield return new object[] { "48 BF 04152637A55A5678", Code.Mov_r64_imm64, 2, 8, 0, 0 };
				yield return new object[] { "49 BF 04152637A55A5678", Code.Mov_r64_imm64, 2, 8, 0, 0 };
				yield return new object[] { "C0 C1 5A", Code.Rol_rm8_imm8, 2, 1, 0, 0 };
				yield return new object[] { "C0 CA A5", Code.Ror_rm8_imm8, 2, 1, 0, 0 };
				yield return new object[] { "C0 D3 5A", Code.Rcl_rm8_imm8, 2, 1, 0, 0 };
				yield return new object[] { "C0 DC A5", Code.Rcr_rm8_imm8, 2, 1, 0, 0 };
				yield return new object[] { "C0 E5 5A", Code.Shl_rm8_imm8, 2, 1, 0, 0 };
				yield return new object[] { "C0 EE A5", Code.Shr_rm8_imm8, 2, 1, 0, 0 };
				yield return new object[] { "C0 F8 A5", Code.Sar_rm8_imm8, 2, 1, 0, 0 };
				yield return new object[] { "66 C1 C1 5A", Code.Rol_rm16_imm8, 3, 1, 0, 0 };
				yield return new object[] { "C1 C1 5A", Code.Rol_rm32_imm8, 2, 1, 0, 0 };
				yield return new object[] { "48 C1 C1 5A", Code.Rol_rm64_imm8, 3, 1, 0, 0 };
				yield return new object[] { "66 C1 CA A5", Code.Ror_rm16_imm8, 3, 1, 0, 0 };
				yield return new object[] { "C1 CA A5", Code.Ror_rm32_imm8, 2, 1, 0, 0 };
				yield return new object[] { "48 C1 CA A5", Code.Ror_rm64_imm8, 3, 1, 0, 0 };
				yield return new object[] { "66 C1 D3 5A", Code.Rcl_rm16_imm8, 3, 1, 0, 0 };
				yield return new object[] { "C1 D3 5A", Code.Rcl_rm32_imm8, 2, 1, 0, 0 };
				yield return new object[] { "48 C1 D3 5A", Code.Rcl_rm64_imm8, 3, 1, 0, 0 };
				yield return new object[] { "66 C1 DC A5", Code.Rcr_rm16_imm8, 3, 1, 0, 0 };
				yield return new object[] { "C1 DC A5", Code.Rcr_rm32_imm8, 2, 1, 0, 0 };
				yield return new object[] { "48 C1 DC A5", Code.Rcr_rm64_imm8, 3, 1, 0, 0 };
				yield return new object[] { "66 C1 E5 5A", Code.Shl_rm16_imm8, 3, 1, 0, 0 };
				yield return new object[] { "C1 E5 5A", Code.Shl_rm32_imm8, 2, 1, 0, 0 };
				yield return new object[] { "48 C1 E5 5A", Code.Shl_rm64_imm8, 3, 1, 0, 0 };
				yield return new object[] { "66 C1 EE A5", Code.Shr_rm16_imm8, 3, 1, 0, 0 };
				yield return new object[] { "C1 EE A5", Code.Shr_rm32_imm8, 2, 1, 0, 0 };
				yield return new object[] { "48 C1 EE A5", Code.Shr_rm64_imm8, 3, 1, 0, 0 };
				yield return new object[] { "66 C1 F8 A5", Code.Sar_rm16_imm8, 3, 1, 0, 0 };
				yield return new object[] { "C1 F8 A5", Code.Sar_rm32_imm8, 2, 1, 0, 0 };
				yield return new object[] { "49 C1 F8 A5", Code.Sar_rm64_imm8, 3, 1, 0, 0 };
				yield return new object[] { "C2 5AA5", Code.Retnq_imm16, 1, 2, 0, 0 };
				yield return new object[] { "C6 C1 5A", Code.Mov_rm8_imm8, 2, 1, 0, 0 };
				yield return new object[] { "C6 F8 5A", Code.Xabort_imm8, 2, 1, 0, 0 };
				yield return new object[] { "66 C7 C1 5AA5", Code.Mov_rm16_imm16, 3, 2, 0, 0 };
				yield return new object[] { "C7 C1 5AA51234", Code.Mov_rm32_imm32, 2, 4, 0, 0 };
				yield return new object[] { "48 C7 C1 5AA51284", Code.Mov_rm64_imm32, 3, 4, 0, 0 };
				yield return new object[] { "66 C8 5AA5 A6", Code.Enterw_imm16_imm8, 2, 2, 0, 0 };
				yield return new object[] { "C8 5AA5 A6", Code.Enterq_imm16_imm8, 1, 2, 0, 0 };
				yield return new object[] { "66 CA 5AA5", Code.Retfw_imm16, 2, 2, 0, 0 };
				yield return new object[] { "CA 5AA5", Code.Retfd_imm16, 1, 2, 0, 0 };
				yield return new object[] { "48 CA 5AA5", Code.Retfq_imm16, 2, 2, 0, 0 };
				yield return new object[] { "CD 5A", Code.Int_imm8, 1, 1, 0, 0 };
				yield return new object[] { "E4 5A", Code.In_AL_imm8, 1, 1, 0, 0 };
				yield return new object[] { "66 E5 5A", Code.In_AX_imm8, 2, 1, 0, 0 };
				yield return new object[] { "E5 5A", Code.In_EAX_imm8, 1, 1, 0, 0 };
				yield return new object[] { "E6 5A", Code.Out_imm8_AL, 1, 1, 0, 0 };
				yield return new object[] { "66 E7 5A", Code.Out_imm8_AX, 2, 1, 0, 0 };
				yield return new object[] { "E7 5A", Code.Out_imm8_EAX, 1, 1, 0, 0 };
				yield return new object[] { "F6 C1 5A", Code.Test_rm8_imm8, 2, 1, 0, 0 };
				yield return new object[] { "66 F7 C1 5AA5", Code.Test_rm16_imm16, 3, 2, 0, 0 };
				yield return new object[] { "F7 C1 5AA51234", Code.Test_rm32_imm32, 2, 4, 0, 0 };
				yield return new object[] { "48 F7 C1 5AA51284", Code.Test_rm64_imm32, 3, 4, 0, 0 };
				yield return new object[] { "0F70 CD A5", Code.Pshufw_mm_mmm64_imm8, 3, 1, 0, 0 };
				yield return new object[] { "66 0F70 CD A5", Code.Pshufd_xmm_xmmm128_imm8, 4, 1, 0, 0 };
				yield return new object[] { "C5F9 70 D3 A5", Code.VEX_Vpshufd_xmm_xmmm128_imm8, 4, 1, 0, 0 };
				yield return new object[] { "C5FD 70 D3 A5", Code.VEX_Vpshufd_ymm_ymmm256_imm8, 4, 1, 0, 0 };
				yield return new object[] { "62 F17D8B 70 D3 A5", Code.EVEX_Vpshufd_xmm_k1z_xmmm128b32_imm8, 6, 1, 0, 0 };
				yield return new object[] { "62 F17DAB 70 D3 A5", Code.EVEX_Vpshufd_ymm_k1z_ymmm256b32_imm8, 6, 1, 0, 0 };
				yield return new object[] { "62 F17DCB 70 D3 A5", Code.EVEX_Vpshufd_zmm_k1z_zmmm512b32_imm8, 6, 1, 0, 0 };
				yield return new object[] { "66 0FA4 CE 5A", Code.Shld_rm16_r16_imm8, 4, 1, 0, 0 };
				yield return new object[] { "0FA4 CE 5A", Code.Shld_rm32_r32_imm8, 3, 1, 0, 0 };
				yield return new object[] { "48 0FA4 CE 5A", Code.Shld_rm64_r64_imm8, 4, 1, 0, 0 };
				yield return new object[] { "66 0FAC CE 5A", Code.Shrd_rm16_r16_imm8, 4, 1, 0, 0 };
				yield return new object[] { "0FAC CE 5A", Code.Shrd_rm32_r32_imm8, 3, 1, 0, 0 };
				yield return new object[] { "48 0FAC CE 5A", Code.Shrd_rm64_r64_imm8, 4, 1, 0, 0 };
				yield return new object[] { "66 0FBA E5 5A", Code.Bt_rm16_imm8, 4, 1, 0, 0 };
				yield return new object[] { "0FBA E5 5A", Code.Bt_rm32_imm8, 3, 1, 0, 0 };
				yield return new object[] { "48 0FBA E5 5A", Code.Bt_rm64_imm8, 4, 1, 0, 0 };
				yield return new object[] { "66 0FBA EE A5", Code.Bts_rm16_imm8, 4, 1, 0, 0 };
				yield return new object[] { "0FBA EE A5", Code.Bts_rm32_imm8, 3, 1, 0, 0 };
				yield return new object[] { "48 0FBA EE A5", Code.Bts_rm64_imm8, 4, 1, 0, 0 };
				yield return new object[] { "66 0FBA F7 5A", Code.Btr_rm16_imm8, 4, 1, 0, 0 };
				yield return new object[] { "0FBA F7 5A", Code.Btr_rm32_imm8, 3, 1, 0, 0 };
				yield return new object[] { "48 0FBA F7 5A", Code.Btr_rm64_imm8, 4, 1, 0, 0 };
				yield return new object[] { "66 0FBA F8 A5", Code.Btc_rm16_imm8, 4, 1, 0, 0 };
				yield return new object[] { "0FBA F8 A5", Code.Btc_rm32_imm8, 3, 1, 0, 0 };
				yield return new object[] { "49 0FBA F8 A5", Code.Btc_rm64_imm8, 4, 1, 0, 0 };
				yield return new object[] { "C4E37B F0 D3 A5", Code.VEX_Rorx_r32_rm32_imm8, 5, 1, 0, 0 };
				yield return new object[] { "C4E3FB F0 D3 A5", Code.VEX_Rorx_r64_rm64_imm8, 5, 1, 0, 0 };

				yield return new object[] { "67 A0 11223344", Code.Mov_AL_moffs8, 0, 0, 2, 4 };
				yield return new object[] { "66 67 A1 11223344", Code.Mov_AX_moffs16, 0, 0, 3, 4 };
				yield return new object[] { "67 A1 11223344", Code.Mov_EAX_moffs32, 0, 0, 2, 4 };
				yield return new object[] { "67 48 A1 11223344", Code.Mov_RAX_moffs64, 0, 0, 3, 4 };
				yield return new object[] { "67 A2 11223344", Code.Mov_moffs8_AL, 0, 0, 2, 4 };
				yield return new object[] { "66 67 A3 11223344", Code.Mov_moffs16_AX, 0, 0, 3, 4 };
				yield return new object[] { "67 A3 11223344", Code.Mov_moffs32_EAX, 0, 0, 2, 4 };
				yield return new object[] { "67 48 A3 11223344", Code.Mov_moffs64_RAX, 0, 0, 3, 4 };

				yield return new object[] { "A0 1122334455667788", Code.Mov_AL_moffs8, 0, 0, 1, 8 };
				yield return new object[] { "66 A1 1122334455667788", Code.Mov_AX_moffs16, 0, 0, 2, 8 };
				yield return new object[] { "A1 1122334455667788", Code.Mov_EAX_moffs32, 0, 0, 1, 8 };
				yield return new object[] { "48 A1 1122334455667788", Code.Mov_RAX_moffs64, 0, 0, 2, 8 };
				yield return new object[] { "A2 1122334455667788", Code.Mov_moffs8_AL, 0, 0, 1, 8 };
				yield return new object[] { "66 A3 1122334455667788", Code.Mov_moffs16_AX, 0, 0, 2, 8 };
				yield return new object[] { "A3 1122334455667788", Code.Mov_moffs32_EAX, 0, 0, 1, 8 };
				yield return new object[] { "48 A3 1122334455667788", Code.Mov_moffs64_RAX, 0, 0, 2, 8 };

				yield return new object[] { "67 01 08", Code.Add_rm32_r32, 0, 0, 0, 0 };
				yield return new object[] { "67 01 48 5A", Code.Add_rm32_r32, 0, 0, 3, 1 };
				yield return new object[] { "67 01 88 44332211", Code.Add_rm32_r32, 0, 0, 3, 4 };
				yield return new object[] { "67 01 0C 10", Code.Add_rm32_r32, 0, 0, 0, 0 };
				yield return new object[] { "67 01 4C 10 5A", Code.Add_rm32_r32, 0, 0, 4, 1 };
				yield return new object[] { "67 01 8C 10 44332211", Code.Add_rm32_r32, 0, 0, 4, 4 };
				yield return new object[] { "67 01 0D 44332211", Code.Add_rm32_r32, 0, 0, 3, 4 };

				yield return new object[] { "48 01 08", Code.Add_rm64_r64, 0, 0, 0, 0 };
				yield return new object[] { "48 01 48 5A", Code.Add_rm64_r64, 0, 0, 3, 1 };
				yield return new object[] { "48 01 88 44332211", Code.Add_rm64_r64, 0, 0, 3, 4 };
				yield return new object[] { "48 01 0C 10", Code.Add_rm64_r64, 0, 0, 0, 0 };
				yield return new object[] { "48 01 4C 10 5A", Code.Add_rm64_r64, 0, 0, 4, 1 };
				yield return new object[] { "48 01 8C 10 44332211", Code.Add_rm64_r64, 0, 0, 4, 4 };
				yield return new object[] { "48 01 0D 44332211", Code.Add_rm64_r64, 0, 0, 3, 4 };

				yield return new object[] { "80 80 78563412 9A", Code.Add_rm8_imm8, 6, 1, 2, 4 };
				yield return new object[] { "66 81 40 12 5634", Code.Add_rm16_imm16, 4, 2, 3, 1 };
				yield return new object[] { "81 40 12 9A785634", Code.Add_rm32_imm32, 3, 4, 2, 1 };

				yield return new object[] { "62 F2CD2B 3B 50 01", Code.EVEX_Vpminuq_ymm_k1z_ymm_ymmm256b64, 0, 0, 6, 1 };

				yield return new object[] { "CC", Code.Int3, 0, 0, 0, 0 };
				yield return new object[] { "F1", Code.Int1, 0, 0, 0, 0 };
				yield return new object[] { "D0 C1", Code.Rol_rm8_1, 0, 0, 0, 0 };
				yield return new object[] { "D0 CA", Code.Ror_rm8_1, 0, 0, 0, 0 };
				yield return new object[] { "D0 D3", Code.Rcl_rm8_1, 0, 0, 0, 0 };
				yield return new object[] { "D0 DC", Code.Rcr_rm8_1, 0, 0, 0, 0 };
				yield return new object[] { "D0 E5", Code.Shl_rm8_1, 0, 0, 0, 0 };
				yield return new object[] { "D0 EE", Code.Shr_rm8_1, 0, 0, 0, 0 };
				yield return new object[] { "D0 F8", Code.Sar_rm8_1, 0, 0, 0, 0 };
				yield return new object[] { "66 D1 C1", Code.Rol_rm16_1, 0, 0, 0, 0 };
				yield return new object[] { "D1 C1", Code.Rol_rm32_1, 0, 0, 0, 0 };
				yield return new object[] { "48 D1 C1", Code.Rol_rm64_1, 0, 0, 0, 0 };
				yield return new object[] { "66 D1 CA", Code.Ror_rm16_1, 0, 0, 0, 0 };
				yield return new object[] { "D1 CA", Code.Ror_rm32_1, 0, 0, 0, 0 };
				yield return new object[] { "48 D1 CA", Code.Ror_rm64_1, 0, 0, 0, 0 };
				yield return new object[] { "66 D1 D3", Code.Rcl_rm16_1, 0, 0, 0, 0 };
				yield return new object[] { "D1 D3", Code.Rcl_rm32_1, 0, 0, 0, 0 };
				yield return new object[] { "48 D1 D3", Code.Rcl_rm64_1, 0, 0, 0, 0 };
				yield return new object[] { "66 D1 DC", Code.Rcr_rm16_1, 0, 0, 0, 0 };
				yield return new object[] { "D1 DC", Code.Rcr_rm32_1, 0, 0, 0, 0 };
				yield return new object[] { "48 D1 DC", Code.Rcr_rm64_1, 0, 0, 0, 0 };
				yield return new object[] { "66 D1 E5", Code.Shl_rm16_1, 0, 0, 0, 0 };
				yield return new object[] { "D1 E5", Code.Shl_rm32_1, 0, 0, 0, 0 };
				yield return new object[] { "48 D1 E5", Code.Shl_rm64_1, 0, 0, 0, 0 };
				yield return new object[] { "66 D1 EE", Code.Shr_rm16_1, 0, 0, 0, 0 };
				yield return new object[] { "D1 EE", Code.Shr_rm32_1, 0, 0, 0, 0 };
				yield return new object[] { "48 D1 EE", Code.Shr_rm64_1, 0, 0, 0, 0 };
				yield return new object[] { "66 D1 F8", Code.Sar_rm16_1, 0, 0, 0, 0 };
				yield return new object[] { "D1 F8", Code.Sar_rm32_1, 0, 0, 0, 0 };
				yield return new object[] { "49 D1 F8", Code.Sar_rm64_1, 0, 0, 0, 0 };
			}
		}
	}
}
