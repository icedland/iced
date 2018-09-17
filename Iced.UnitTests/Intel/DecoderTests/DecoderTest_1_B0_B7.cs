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

using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.DecoderTests {
	public sealed class DecoderTest_1_B0_B7 : DecoderTest {
		[Theory]
		[InlineData("B0 5A", 2, Code.Mov_AL_imm8, Register.AL, 0x5A)]
		[InlineData("B1 A5", 2, Code.Mov_CL_imm8, Register.CL, 0xA5)]
		[InlineData("B2 5A", 2, Code.Mov_DL_imm8, Register.DL, 0x5A)]
		[InlineData("B3 A5", 2, Code.Mov_BL_imm8, Register.BL, 0xA5)]
		[InlineData("B4 5A", 2, Code.Mov_AH_imm8, Register.AH, 0x5A)]
		[InlineData("B5 A5", 2, Code.Mov_CH_imm8, Register.CH, 0xA5)]
		[InlineData("B6 5A", 2, Code.Mov_DH_imm8, Register.DH, 0x5A)]
		[InlineData("B7 A5", 2, Code.Mov_BH_imm8, Register.BH, 0xA5)]
		void Test16_Movb_Reg8Ib_1(string hexBytes, int byteLength, Code code, Register register, byte immediate) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(register, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate, instr.Immediate8);
		}

		[Theory]
		[InlineData("B0 5A", 2, Code.Mov_AL_imm8, Register.AL, 0x5A)]
		[InlineData("B1 A5", 2, Code.Mov_CL_imm8, Register.CL, 0xA5)]
		[InlineData("B2 5A", 2, Code.Mov_DL_imm8, Register.DL, 0x5A)]
		[InlineData("B3 A5", 2, Code.Mov_BL_imm8, Register.BL, 0xA5)]
		[InlineData("B4 5A", 2, Code.Mov_AH_imm8, Register.AH, 0x5A)]
		[InlineData("B5 A5", 2, Code.Mov_CH_imm8, Register.CH, 0xA5)]
		[InlineData("B6 5A", 2, Code.Mov_DH_imm8, Register.DH, 0x5A)]
		[InlineData("B7 A5", 2, Code.Mov_BH_imm8, Register.BH, 0xA5)]
		void Test32_Movb_Reg8Ib_1(string hexBytes, int byteLength, Code code, Register register, byte immediate) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(register, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate, instr.Immediate8);
		}

		[Theory]
		[InlineData("B0 5A", 2, Code.Mov_AL_imm8, Register.AL, 0x5A)]
		[InlineData("B1 A5", 2, Code.Mov_CL_imm8, Register.CL, 0xA5)]
		[InlineData("B2 5A", 2, Code.Mov_DL_imm8, Register.DL, 0x5A)]
		[InlineData("B3 A5", 2, Code.Mov_BL_imm8, Register.BL, 0xA5)]
		[InlineData("B4 5A", 2, Code.Mov_AH_imm8, Register.AH, 0x5A)]
		[InlineData("B5 A5", 2, Code.Mov_CH_imm8, Register.CH, 0xA5)]
		[InlineData("B6 5A", 2, Code.Mov_DH_imm8, Register.DH, 0x5A)]
		[InlineData("B7 A5", 2, Code.Mov_BH_imm8, Register.BH, 0xA5)]

		[InlineData("40 B0 5A", 3, Code.Mov_AL_imm8, Register.AL, 0x5A)]
		[InlineData("40 B1 A5", 3, Code.Mov_CL_imm8, Register.CL, 0xA5)]
		[InlineData("40 B2 5A", 3, Code.Mov_DL_imm8, Register.DL, 0x5A)]
		[InlineData("40 B3 A5", 3, Code.Mov_BL_imm8, Register.BL, 0xA5)]
		[InlineData("40 B4 5A", 3, Code.Mov_SPL_imm8, Register.SPL, 0x5A)]
		[InlineData("40 B5 A5", 3, Code.Mov_BPL_imm8, Register.BPL, 0xA5)]
		[InlineData("40 B6 5A", 3, Code.Mov_SIL_imm8, Register.SIL, 0x5A)]
		[InlineData("40 B7 A5", 3, Code.Mov_DIL_imm8, Register.DIL, 0xA5)]

		[InlineData("41 B0 5A", 3, Code.Mov_R8L_imm8, Register.R8L, 0x5A)]
		[InlineData("41 B1 A5", 3, Code.Mov_R9L_imm8, Register.R9L, 0xA5)]
		[InlineData("41 B2 5A", 3, Code.Mov_R10L_imm8, Register.R10L, 0x5A)]
		[InlineData("41 B3 A5", 3, Code.Mov_R11L_imm8, Register.R11L, 0xA5)]
		[InlineData("41 B4 5A", 3, Code.Mov_R12L_imm8, Register.R12L, 0x5A)]
		[InlineData("41 B5 A5", 3, Code.Mov_R13L_imm8, Register.R13L, 0xA5)]
		[InlineData("41 B6 5A", 3, Code.Mov_R14L_imm8, Register.R14L, 0x5A)]
		[InlineData("41 B7 A5", 3, Code.Mov_R15L_imm8, Register.R15L, 0xA5)]

		[InlineData("4E B0 5A", 3, Code.Mov_AL_imm8, Register.AL, 0x5A)]
		[InlineData("4E B1 A5", 3, Code.Mov_CL_imm8, Register.CL, 0xA5)]
		[InlineData("4E B2 5A", 3, Code.Mov_DL_imm8, Register.DL, 0x5A)]
		[InlineData("4E B3 A5", 3, Code.Mov_BL_imm8, Register.BL, 0xA5)]
		[InlineData("4E B4 5A", 3, Code.Mov_SPL_imm8, Register.SPL, 0x5A)]
		[InlineData("4E B5 A5", 3, Code.Mov_BPL_imm8, Register.BPL, 0xA5)]
		[InlineData("4E B6 5A", 3, Code.Mov_SIL_imm8, Register.SIL, 0x5A)]
		[InlineData("4E B7 A5", 3, Code.Mov_DIL_imm8, Register.DIL, 0xA5)]

		[InlineData("4F B0 5A", 3, Code.Mov_R8L_imm8, Register.R8L, 0x5A)]
		[InlineData("4F B1 A5", 3, Code.Mov_R9L_imm8, Register.R9L, 0xA5)]
		[InlineData("4F B2 5A", 3, Code.Mov_R10L_imm8, Register.R10L, 0x5A)]
		[InlineData("4F B3 A5", 3, Code.Mov_R11L_imm8, Register.R11L, 0xA5)]
		[InlineData("4F B4 5A", 3, Code.Mov_R12L_imm8, Register.R12L, 0x5A)]
		[InlineData("4F B5 A5", 3, Code.Mov_R13L_imm8, Register.R13L, 0xA5)]
		[InlineData("4F B6 5A", 3, Code.Mov_R14L_imm8, Register.R14L, 0x5A)]
		[InlineData("4F B7 A5", 3, Code.Mov_R15L_imm8, Register.R15L, 0xA5)]
		void Test64_Movb_Reg8Ib_1(string hexBytes, int byteLength, Code code, Register register, byte immediate) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(register, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate, instr.Immediate8);
		}
	}
}
