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
	public sealed class DecoderTest_3_0F3A30_0F3A37 : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_Mask_VK_RK_1_Data))]
		void Test16_Mask_VK_RK_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_Mask_VK_RK_1_Data {
			get {
				yield return new object[] { "C4E3F9 30 D3 A5", 6, Code.VEX_Kshiftrw_k_k_imm8, Register.K2, Register.K3, 0xA5 };
				yield return new object[] { "C4E379 30 D3 A5", 6, Code.VEX_Kshiftrb_k_k_imm8, Register.K2, Register.K3, 0xA5 };
				yield return new object[] { "C4E3F9 31 D3 A5", 6, Code.VEX_Kshiftrq_k_k_imm8, Register.K2, Register.K3, 0xA5 };
				yield return new object[] { "C4E379 31 D3 A5", 6, Code.VEX_Kshiftrd_k_k_imm8, Register.K2, Register.K3, 0xA5 };

				yield return new object[] { "C4E3F9 32 D3 A5", 6, Code.VEX_Kshiftlw_k_k_imm8, Register.K2, Register.K3, 0xA5 };
				yield return new object[] { "C4E379 32 D3 A5", 6, Code.VEX_Kshiftlb_k_k_imm8, Register.K2, Register.K3, 0xA5 };
				yield return new object[] { "C4E3F9 33 D3 A5", 6, Code.VEX_Kshiftlq_k_k_imm8, Register.K2, Register.K3, 0xA5 };
				yield return new object[] { "C4E379 33 D3 A5", 6, Code.VEX_Kshiftld_k_k_imm8, Register.K2, Register.K3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Mask_VK_RK_1_Data))]
		void Test32_Mask_VK_RK_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_Mask_VK_RK_1_Data {
			get {
				yield return new object[] { "C4E3F9 30 D3 A5", 6, Code.VEX_Kshiftrw_k_k_imm8, Register.K2, Register.K3, 0xA5 };
				yield return new object[] { "C4E379 30 D3 A5", 6, Code.VEX_Kshiftrb_k_k_imm8, Register.K2, Register.K3, 0xA5 };
				yield return new object[] { "C4E3F9 31 D3 A5", 6, Code.VEX_Kshiftrq_k_k_imm8, Register.K2, Register.K3, 0xA5 };
				yield return new object[] { "C4E379 31 D3 A5", 6, Code.VEX_Kshiftrd_k_k_imm8, Register.K2, Register.K3, 0xA5 };

				yield return new object[] { "C4E3F9 32 D3 A5", 6, Code.VEX_Kshiftlw_k_k_imm8, Register.K2, Register.K3, 0xA5 };
				yield return new object[] { "C4E379 32 D3 A5", 6, Code.VEX_Kshiftlb_k_k_imm8, Register.K2, Register.K3, 0xA5 };
				yield return new object[] { "C4E3F9 33 D3 A5", 6, Code.VEX_Kshiftlq_k_k_imm8, Register.K2, Register.K3, 0xA5 };
				yield return new object[] { "C4E379 33 D3 A5", 6, Code.VEX_Kshiftld_k_k_imm8, Register.K2, Register.K3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Mask_VK_RK_1_Data))]
		void Test64_Mask_VK_RK_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_Mask_VK_RK_1_Data {
			get {
				yield return new object[] { "C4E3F9 30 D3 A5", 6, Code.VEX_Kshiftrw_k_k_imm8, Register.K2, Register.K3, 0xA5 };
				yield return new object[] { "C4E379 30 D3 A5", 6, Code.VEX_Kshiftrb_k_k_imm8, Register.K2, Register.K3, 0xA5 };
				yield return new object[] { "C4E3F9 31 D3 A5", 6, Code.VEX_Kshiftrq_k_k_imm8, Register.K2, Register.K3, 0xA5 };
				yield return new object[] { "C4E379 31 D3 A5", 6, Code.VEX_Kshiftrd_k_k_imm8, Register.K2, Register.K3, 0xA5 };

				yield return new object[] { "C4E3F9 32 D3 A5", 6, Code.VEX_Kshiftlw_k_k_imm8, Register.K2, Register.K3, 0xA5 };
				yield return new object[] { "C4E379 32 D3 A5", 6, Code.VEX_Kshiftlb_k_k_imm8, Register.K2, Register.K3, 0xA5 };
				yield return new object[] { "C4E3F9 33 D3 A5", 6, Code.VEX_Kshiftlq_k_k_imm8, Register.K2, Register.K3, 0xA5 };
				yield return new object[] { "C4E379 33 D3 A5", 6, Code.VEX_Kshiftld_k_k_imm8, Register.K2, Register.K3, 0xA5 };
			}
		}
	}
}
