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
	public sealed class DecoderTest_1_F0_F7 : DecoderTest {
		[Theory]
		[InlineData("F1", 1, Code.Int1)]
		[InlineData("F4", 1, Code.Hlt)]
		[InlineData("F5", 1, Code.Cmc)]
		void Test16_Simple_1(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Theory]
		[InlineData("F1", 1, Code.Int1)]
		[InlineData("F4", 1, Code.Hlt)]
		[InlineData("F5", 1, Code.Cmc)]
		void Test32_Simple_1(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Theory]
		[InlineData("F1", 1, Code.Int1)]
		[InlineData("F4", 1, Code.Hlt)]
		[InlineData("F5", 1, Code.Cmc)]
		void Test64_Simple_1(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Theory]
		[InlineData("F6 00 5A", 3, Code.Test_rm8_imm8, 0x5A)]
		[InlineData("F6 08 A5", 3, Code.Test_rm8_imm8_F6r1, 0xA5)]
		void Test16_Grp3_Test_rm8_imm8_1(string hexBytes, int byteLength, Code code, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("F6 C1 5A", 3, Code.Test_rm8_imm8, Register.CL, 0x5A)]
		[InlineData("F6 CA A5", 3, Code.Test_rm8_imm8_F6r1, Register.DL, 0xA5)]
		void Test16_Grp3_Test_rm8_imm8_2(string hexBytes, int byteLength, Code code, Register reg, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("F6 00 5A", 3, Code.Test_rm8_imm8, 0x5A)]
		[InlineData("F6 08 A5", 3, Code.Test_rm8_imm8_F6r1, 0xA5)]
		void Test32_Grp3_Test_rm8_imm8_1(string hexBytes, int byteLength, Code code, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("F6 C1 5A", 3, Code.Test_rm8_imm8, Register.CL, 0x5A)]
		[InlineData("F6 CA A5", 3, Code.Test_rm8_imm8_F6r1, Register.DL, 0xA5)]
		void Test32_Grp3_Test_rm8_imm8_2(string hexBytes, int byteLength, Code code, Register reg, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("F6 00 5A", 3, Code.Test_rm8_imm8, 0x5A)]
		[InlineData("F6 08 A5", 3, Code.Test_rm8_imm8_F6r1, 0xA5)]

		[InlineData("44 F6 00 5A", 4, Code.Test_rm8_imm8, 0x5A)]
		[InlineData("44 F6 08 A5", 4, Code.Test_rm8_imm8_F6r1, 0xA5)]
		void Test64_Grp3_Test_rm8_imm8_1(string hexBytes, int byteLength, Code code, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("F6 C1 5A", 3, Code.Test_rm8_imm8, Register.CL, 0x5A)]
		[InlineData("F6 CE A5", 3, Code.Test_rm8_imm8_F6r1, Register.DH, 0xA5)]

		[InlineData("40 F6 C1 5A", 4, Code.Test_rm8_imm8, Register.CL, 0x5A)]
		[InlineData("40 F6 CE A5", 4, Code.Test_rm8_imm8_F6r1, Register.SIL, 0xA5)]
		[InlineData("41 F6 C1 5A", 4, Code.Test_rm8_imm8, Register.R9L, 0x5A)]
		[InlineData("41 F6 CE A5", 4, Code.Test_rm8_imm8_F6r1, Register.R14L, 0xA5)]
		void Test64_Grp3_Test_rm8_imm8_2(string hexBytes, int byteLength, Code code, Register reg, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("F7 00 5AA5", 4, Code.Test_rm16_imm16, 0xA55A)]
		[InlineData("F7 08 A55A", 4, Code.Test_rm16_imm16_F7r1, 0x5AA5)]
		void Test16_Grp3_Test_rm16_imm16_1(string hexBytes, int byteLength, Code code, ushort immediate16) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate16, instr.Op1Kind);
			Assert.Equal(immediate16, instr.Immediate16);
		}

		[Theory]
		[InlineData("F7 C1 5AA5", 4, Code.Test_rm16_imm16, Register.CX, 0xA55A)]
		[InlineData("F7 CA A55A", 4, Code.Test_rm16_imm16_F7r1, Register.DX, 0x5AA5)]
		void Test16_Grp3_Test_rm16_imm16_2(string hexBytes, int byteLength, Code code, Register reg, ushort immediate16) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Immediate16, instr.Op1Kind);
			Assert.Equal(immediate16, instr.Immediate16);
		}

		[Theory]
		[InlineData("66 F7 00 5AA5", 5, Code.Test_rm16_imm16, 0xA55A)]
		[InlineData("66 F7 08 A55A", 5, Code.Test_rm16_imm16_F7r1, 0x5AA5)]
		void Test32_Grp3_Test_rm16_imm16_1(string hexBytes, int byteLength, Code code, ushort immediate16) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate16, instr.Op1Kind);
			Assert.Equal(immediate16, instr.Immediate16);
		}

		[Theory]
		[InlineData("66 F7 C1 5AA5", 5, Code.Test_rm16_imm16, Register.CX, 0xA55A)]
		[InlineData("66 F7 CA A55A", 5, Code.Test_rm16_imm16_F7r1, Register.DX, 0x5AA5)]
		void Test32_Grp3_Test_rm16_imm16_2(string hexBytes, int byteLength, Code code, Register reg, ushort immediate16) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Immediate16, instr.Op1Kind);
			Assert.Equal(immediate16, instr.Immediate16);
		}

		[Theory]
		[InlineData("66 F7 00 5AA5", 5, Code.Test_rm16_imm16, 0xA55A)]
		[InlineData("66 F7 08 A55A", 5, Code.Test_rm16_imm16_F7r1, 0x5AA5)]
		void Test64_Grp3_Test_rm16_imm16_1(string hexBytes, int byteLength, Code code, ushort immediate16) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate16, instr.Op1Kind);
			Assert.Equal(immediate16, instr.Immediate16);
		}

		[Theory]
		[InlineData("66 F7 C1 5AA5", 5, Code.Test_rm16_imm16, Register.CX, 0xA55A)]
		[InlineData("66 F7 CE A55A", 5, Code.Test_rm16_imm16_F7r1, Register.SI, 0x5AA5)]
		[InlineData("66 41 F7 C1 5AA5", 6, Code.Test_rm16_imm16, Register.R9W, 0xA55A)]
		[InlineData("66 41 F7 CE A55A", 6, Code.Test_rm16_imm16_F7r1, Register.R14W, 0x5AA5)]
		void Test64_Grp3_Test_rm16_imm16_2(string hexBytes, int byteLength, Code code, Register reg, ushort immediate16) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Immediate16, instr.Op1Kind);
			Assert.Equal(immediate16, instr.Immediate16);
		}

		[Theory]
		[InlineData("66 F7 00 5AA51234", 7, Code.Test_rm32_imm32, 0x3412A55A)]
		[InlineData("66 F7 08 A55A89AB", 7, Code.Test_rm32_imm32_F7r1, 0xAB895AA5)]
		void Test16_Grp3_Test_rm32_imm32_1(string hexBytes, int byteLength, Code code, uint immediate32) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate32, instr.Op1Kind);
			Assert.Equal(immediate32, instr.Immediate32);
		}

		[Theory]
		[InlineData("66 F7 C1 5AA51234", 7, Code.Test_rm32_imm32, Register.ECX, 0x3412A55A)]
		[InlineData("66 F7 CA A55A89AB", 7, Code.Test_rm32_imm32_F7r1, Register.EDX, 0xAB895AA5)]
		void Test16_Grp3_Test_rm32_imm32_2(string hexBytes, int byteLength, Code code, Register reg, uint immediate32) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Immediate32, instr.Op1Kind);
			Assert.Equal(immediate32, instr.Immediate32);
		}

		[Theory]
		[InlineData("F7 00 5AA51234", 6, Code.Test_rm32_imm32, 0x3412A55A)]
		[InlineData("F7 08 A55A89AB", 6, Code.Test_rm32_imm32_F7r1, 0xAB895AA5)]
		void Test32_Grp3_Test_rm32_imm32_1(string hexBytes, int byteLength, Code code, uint immediate32) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate32, instr.Op1Kind);
			Assert.Equal(immediate32, instr.Immediate32);
		}

		[Theory]
		[InlineData("F7 C1 5AA51234", 6, Code.Test_rm32_imm32, Register.ECX, 0x3412A55A)]
		[InlineData("F7 CA A55A89AB", 6, Code.Test_rm32_imm32_F7r1, Register.EDX, 0xAB895AA5)]
		void Test32_Grp3_Test_rm32_imm32_2(string hexBytes, int byteLength, Code code, Register reg, uint immediate32) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Immediate32, instr.Op1Kind);
			Assert.Equal(immediate32, instr.Immediate32);
		}

		[Theory]
		[InlineData("F7 00 5AA51234", 6, Code.Test_rm32_imm32, 0x3412A55A)]
		[InlineData("F7 08 A55A89AB", 6, Code.Test_rm32_imm32_F7r1, 0xAB895AA5)]
		void Test64_Grp3_Test_rm32_imm32_1(string hexBytes, int byteLength, Code code, uint immediate32) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate32, instr.Op1Kind);
			Assert.Equal(immediate32, instr.Immediate32);
		}

		[Theory]
		[InlineData("F7 C1 5AA51234", 6, Code.Test_rm32_imm32, Register.ECX, 0x3412A55A)]
		[InlineData("F7 CE A55A89AB", 6, Code.Test_rm32_imm32_F7r1, Register.ESI, 0xAB895AA5)]
		[InlineData("41 F7 C1 5AA51234", 7, Code.Test_rm32_imm32, Register.R9D, 0x3412A55A)]
		[InlineData("41 F7 CE A55A89AB", 7, Code.Test_rm32_imm32_F7r1, Register.R14D, 0xAB895AA5)]
		void Test64_Grp3_Test_rm32_imm32_2(string hexBytes, int byteLength, Code code, Register reg, uint immediate32) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Immediate32, instr.Op1Kind);
			Assert.Equal(immediate32, instr.Immediate32);
		}

		[Theory]
		[InlineData("48 F7 00 5AA51234", 7, Code.Test_rm64_imm32, 0x3412A55AUL)]
		[InlineData("48 F7 08 A55A89AB", 7, Code.Test_rm64_imm32_F7r1, 0xFFFFFFFFAB895AA5UL)]
		void Test64_Grp3_Test_EqId_1(string hexBytes, int byteLength, Code code, ulong immediate64) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt64, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate32to64, instr.Op1Kind);
			Assert.Equal((long)immediate64, instr.Immediate32to64);
		}

		[Theory]
		[InlineData("48 F7 C1 5AA51234", 7, Code.Test_rm64_imm32, Register.RCX, 0x3412A55AUL)]
		[InlineData("48 F7 CE A55A89AB", 7, Code.Test_rm64_imm32_F7r1, Register.RSI, 0xFFFFFFFFAB895AA5UL)]
		[InlineData("49 F7 C1 5AA51234", 7, Code.Test_rm64_imm32, Register.R9, 0x3412A55AUL)]
		[InlineData("49 F7 CE A55A89AB", 7, Code.Test_rm64_imm32_F7r1, Register.R14, 0xFFFFFFFFAB895AA5UL)]
		void Test64_Grp3_Test_EqId_2(string hexBytes, int byteLength, Code code, Register reg, ulong immediate64) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Immediate32to64, instr.Op1Kind);
			Assert.Equal((long)immediate64, instr.Immediate32to64);
		}

		[Theory]
		[InlineData("F6 10", 2, Code.Not_rm8, MemorySize.UInt8)]
		[InlineData("F6 18", 2, Code.Neg_rm8, MemorySize.Int8)]
		[InlineData("F6 20", 2, Code.Mul_rm8, MemorySize.UInt8)]
		[InlineData("F6 28", 2, Code.Imul_rm8, MemorySize.Int8)]
		[InlineData("F6 30", 2, Code.Div_rm8, MemorySize.UInt8)]
		[InlineData("F6 38", 2, Code.Idiv_rm8, MemorySize.Int8)]
		void Test16_Grp3_Eb_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("F6 D1", 2, Code.Not_rm8, Register.CL)]
		[InlineData("F6 DA", 2, Code.Neg_rm8, Register.DL)]
		[InlineData("F6 E5", 2, Code.Mul_rm8, Register.CH)]
		[InlineData("F6 EE", 2, Code.Imul_rm8, Register.DH)]
		[InlineData("F6 F7", 2, Code.Div_rm8, Register.BH)]
		[InlineData("F6 F8", 2, Code.Idiv_rm8, Register.AL)]
		void Test16_Grp3_Eb_2(string hexBytes, int byteLength, Code code, Register reg) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);
		}

		[Theory]
		[InlineData("F6 10", 2, Code.Not_rm8, MemorySize.UInt8)]
		[InlineData("F6 18", 2, Code.Neg_rm8, MemorySize.Int8)]
		[InlineData("F6 20", 2, Code.Mul_rm8, MemorySize.UInt8)]
		[InlineData("F6 28", 2, Code.Imul_rm8, MemorySize.Int8)]
		[InlineData("F6 30", 2, Code.Div_rm8, MemorySize.UInt8)]
		[InlineData("F6 38", 2, Code.Idiv_rm8, MemorySize.Int8)]
		void Test32_Grp3_Eb_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("F6 D1", 2, Code.Not_rm8, Register.CL)]
		[InlineData("F6 DA", 2, Code.Neg_rm8, Register.DL)]
		[InlineData("F6 E5", 2, Code.Mul_rm8, Register.CH)]
		[InlineData("F6 EE", 2, Code.Imul_rm8, Register.DH)]
		[InlineData("F6 F7", 2, Code.Div_rm8, Register.BH)]
		[InlineData("F6 F8", 2, Code.Idiv_rm8, Register.AL)]
		void Test32_Grp3_Eb_2(string hexBytes, int byteLength, Code code, Register reg) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);
		}

		[Theory]
		[InlineData("F6 10", 2, Code.Not_rm8, MemorySize.UInt8)]
		[InlineData("F6 18", 2, Code.Neg_rm8, MemorySize.Int8)]
		[InlineData("F6 20", 2, Code.Mul_rm8, MemorySize.UInt8)]
		[InlineData("F6 28", 2, Code.Imul_rm8, MemorySize.Int8)]
		[InlineData("F6 30", 2, Code.Div_rm8, MemorySize.UInt8)]
		[InlineData("F6 38", 2, Code.Idiv_rm8, MemorySize.Int8)]

		[InlineData("44 F6 10", 3, Code.Not_rm8, MemorySize.UInt8)]
		[InlineData("44 F6 18", 3, Code.Neg_rm8, MemorySize.Int8)]
		[InlineData("44 F6 20", 3, Code.Mul_rm8, MemorySize.UInt8)]
		[InlineData("44 F6 28", 3, Code.Imul_rm8, MemorySize.Int8)]
		[InlineData("44 F6 30", 3, Code.Div_rm8, MemorySize.UInt8)]
		[InlineData("44 F6 38", 3, Code.Idiv_rm8, MemorySize.Int8)]
		void Test64_Grp3_Eb_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("F6 D1", 2, Code.Not_rm8, Register.CL)]
		[InlineData("F6 DE", 2, Code.Neg_rm8, Register.DH)]
		[InlineData("F6 E5", 2, Code.Mul_rm8, Register.CH)]
		[InlineData("F6 EE", 2, Code.Imul_rm8, Register.DH)]
		[InlineData("F6 F7", 2, Code.Div_rm8, Register.BH)]
		[InlineData("F6 F8", 2, Code.Idiv_rm8, Register.AL)]

		[InlineData("40 F6 D1", 3, Code.Not_rm8, Register.CL)]
		[InlineData("40 F6 DE", 3, Code.Neg_rm8, Register.SIL)]
		[InlineData("41 F6 D1", 3, Code.Not_rm8, Register.R9L)]
		[InlineData("41 F6 DE", 3, Code.Neg_rm8, Register.R14L)]
		[InlineData("40 F6 E5", 3, Code.Mul_rm8, Register.BPL)]
		[InlineData("40 F6 EE", 3, Code.Imul_rm8, Register.SIL)]
		[InlineData("40 F6 F7", 3, Code.Div_rm8, Register.DIL)]
		[InlineData("41 F6 F8", 3, Code.Idiv_rm8, Register.R8L)]
		[InlineData("41 F6 E5", 3, Code.Mul_rm8, Register.R13L)]
		[InlineData("41 F6 EE", 3, Code.Imul_rm8, Register.R14L)]
		[InlineData("41 F6 F7", 3, Code.Div_rm8, Register.R15L)]
		[InlineData("40 F6 F8", 3, Code.Idiv_rm8, Register.AL)]
		void Test64_Grp3_Eb_2(string hexBytes, int byteLength, Code code, Register reg) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);
		}

		[Theory]
		[InlineData("F7 10", 2, Code.Not_rm16, MemorySize.UInt16)]
		[InlineData("F7 18", 2, Code.Neg_rm16, MemorySize.Int16)]
		[InlineData("F7 20", 2, Code.Mul_rm16, MemorySize.UInt16)]
		[InlineData("F7 28", 2, Code.Imul_rm16, MemorySize.Int16)]
		[InlineData("F7 30", 2, Code.Div_rm16, MemorySize.UInt16)]
		[InlineData("F7 38", 2, Code.Idiv_rm16, MemorySize.Int16)]
		void Test16_Grp3_Ew_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("F7 D1", 2, Code.Not_rm16, Register.CX)]
		[InlineData("F7 DA", 2, Code.Neg_rm16, Register.DX)]
		[InlineData("F7 E5", 2, Code.Mul_rm16, Register.BP)]
		[InlineData("F7 EE", 2, Code.Imul_rm16, Register.SI)]
		[InlineData("F7 F7", 2, Code.Div_rm16, Register.DI)]
		[InlineData("F7 F8", 2, Code.Idiv_rm16, Register.AX)]
		void Test16_Grp3_Ew_2(string hexBytes, int byteLength, Code code, Register reg) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);
		}

		[Theory]
		[InlineData("66 F7 10", 3, Code.Not_rm16, MemorySize.UInt16)]
		[InlineData("66 F7 18", 3, Code.Neg_rm16, MemorySize.Int16)]
		[InlineData("66 F7 20", 3, Code.Mul_rm16, MemorySize.UInt16)]
		[InlineData("66 F7 28", 3, Code.Imul_rm16, MemorySize.Int16)]
		[InlineData("66 F7 30", 3, Code.Div_rm16, MemorySize.UInt16)]
		[InlineData("66 F7 38", 3, Code.Idiv_rm16, MemorySize.Int16)]
		void Test32_Grp3_Ew_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("66 F7 D1", 3, Code.Not_rm16, Register.CX)]
		[InlineData("66 F7 DA", 3, Code.Neg_rm16, Register.DX)]
		[InlineData("66 F7 E5", 3, Code.Mul_rm16, Register.BP)]
		[InlineData("66 F7 EE", 3, Code.Imul_rm16, Register.SI)]
		[InlineData("66 F7 F7", 3, Code.Div_rm16, Register.DI)]
		[InlineData("66 F7 F8", 3, Code.Idiv_rm16, Register.AX)]
		void Test32_Grp3_Ew_2(string hexBytes, int byteLength, Code code, Register reg) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);
		}

		[Theory]
		[InlineData("66 F7 10", 3, Code.Not_rm16, MemorySize.UInt16)]
		[InlineData("66 F7 18", 3, Code.Neg_rm16, MemorySize.Int16)]
		[InlineData("66 F7 20", 3, Code.Mul_rm16, MemorySize.UInt16)]
		[InlineData("66 F7 28", 3, Code.Imul_rm16, MemorySize.Int16)]
		[InlineData("66 F7 30", 3, Code.Div_rm16, MemorySize.UInt16)]
		[InlineData("66 F7 38", 3, Code.Idiv_rm16, MemorySize.Int16)]
		void Test64_Grp3_Ew_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("66 F7 D1", 3, Code.Not_rm16, Register.CX)]
		[InlineData("66 F7 DE", 3, Code.Neg_rm16, Register.SI)]
		[InlineData("66 F7 E5", 3, Code.Mul_rm16, Register.BP)]
		[InlineData("66 F7 EE", 3, Code.Imul_rm16, Register.SI)]
		[InlineData("66 F7 F7", 3, Code.Div_rm16, Register.DI)]
		[InlineData("66 41 F7 F8", 4, Code.Idiv_rm16, Register.R8W)]

		[InlineData("66 41 F7 D1", 4, Code.Not_rm16, Register.R9W)]
		[InlineData("66 41 F7 DE", 4, Code.Neg_rm16, Register.R14W)]
		[InlineData("66 41 F7 E5", 4, Code.Mul_rm16, Register.R13W)]
		[InlineData("66 41 F7 EE", 4, Code.Imul_rm16, Register.R14W)]
		[InlineData("66 41 F7 F7", 4, Code.Div_rm16, Register.R15W)]
		[InlineData("66 F7 F8", 3, Code.Idiv_rm16, Register.AX)]
		void Test64_Grp3_Ew_2(string hexBytes, int byteLength, Code code, Register reg) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);
		}

		[Theory]
		[InlineData("66 F7 10", 3, Code.Not_rm32, MemorySize.UInt32)]
		[InlineData("66 F7 18", 3, Code.Neg_rm32, MemorySize.Int32)]
		[InlineData("66 F7 20", 3, Code.Mul_rm32, MemorySize.UInt32)]
		[InlineData("66 F7 28", 3, Code.Imul_rm32, MemorySize.Int32)]
		[InlineData("66 F7 30", 3, Code.Div_rm32, MemorySize.UInt32)]
		[InlineData("66 F7 38", 3, Code.Idiv_rm32, MemorySize.Int32)]
		void Test16_Grp3_Ed_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("66 F7 D1", 3, Code.Not_rm32, Register.ECX)]
		[InlineData("66 F7 DA", 3, Code.Neg_rm32, Register.EDX)]
		[InlineData("66 F7 E5", 3, Code.Mul_rm32, Register.EBP)]
		[InlineData("66 F7 EE", 3, Code.Imul_rm32, Register.ESI)]
		[InlineData("66 F7 F7", 3, Code.Div_rm32, Register.EDI)]
		[InlineData("66 F7 F8", 3, Code.Idiv_rm32, Register.EAX)]
		void Test16_Grp3_Ed_2(string hexBytes, int byteLength, Code code, Register reg) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);
		}

		[Theory]
		[InlineData("F7 10", 2, Code.Not_rm32, MemorySize.UInt32)]
		[InlineData("F7 18", 2, Code.Neg_rm32, MemorySize.Int32)]
		[InlineData("F7 20", 2, Code.Mul_rm32, MemorySize.UInt32)]
		[InlineData("F7 28", 2, Code.Imul_rm32, MemorySize.Int32)]
		[InlineData("F7 30", 2, Code.Div_rm32, MemorySize.UInt32)]
		[InlineData("F7 38", 2, Code.Idiv_rm32, MemorySize.Int32)]
		void Test32_Grp3_Ed_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("F7 D1", 2, Code.Not_rm32, Register.ECX)]
		[InlineData("F7 DA", 2, Code.Neg_rm32, Register.EDX)]
		[InlineData("F7 E5", 2, Code.Mul_rm32, Register.EBP)]
		[InlineData("F7 EE", 2, Code.Imul_rm32, Register.ESI)]
		[InlineData("F7 F7", 2, Code.Div_rm32, Register.EDI)]
		[InlineData("F7 F8", 2, Code.Idiv_rm32, Register.EAX)]
		void Test32_Grp3_Ed_2(string hexBytes, int byteLength, Code code, Register reg) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);
		}

		[Theory]
		[InlineData("F7 10", 2, Code.Not_rm32, MemorySize.UInt32)]
		[InlineData("F7 18", 2, Code.Neg_rm32, MemorySize.Int32)]
		[InlineData("F7 20", 2, Code.Mul_rm32, MemorySize.UInt32)]
		[InlineData("F7 28", 2, Code.Imul_rm32, MemorySize.Int32)]
		[InlineData("F7 30", 2, Code.Div_rm32, MemorySize.UInt32)]
		[InlineData("F7 38", 2, Code.Idiv_rm32, MemorySize.Int32)]
		void Test64_Grp3_Ed_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("F7 D1", 2, Code.Not_rm32, Register.ECX)]
		[InlineData("F7 DE", 2, Code.Neg_rm32, Register.ESI)]
		[InlineData("F7 E5", 2, Code.Mul_rm32, Register.EBP)]
		[InlineData("F7 EE", 2, Code.Imul_rm32, Register.ESI)]
		[InlineData("F7 F7", 2, Code.Div_rm32, Register.EDI)]
		[InlineData("41 F7 F8", 3, Code.Idiv_rm32, Register.R8D)]

		[InlineData("41 F7 D1", 3, Code.Not_rm32, Register.R9D)]
		[InlineData("41 F7 DE", 3, Code.Neg_rm32, Register.R14D)]
		[InlineData("41 F7 E5", 3, Code.Mul_rm32, Register.R13D)]
		[InlineData("41 F7 EE", 3, Code.Imul_rm32, Register.R14D)]
		[InlineData("41 F7 F7", 3, Code.Div_rm32, Register.R15D)]
		[InlineData("F7 F8", 2, Code.Idiv_rm32, Register.EAX)]
		void Test64_Grp3_Ed_2(string hexBytes, int byteLength, Code code, Register reg) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);
		}

		[Theory]
		[InlineData("48 F7 10", 3, Code.Not_rm64, MemorySize.UInt64)]
		[InlineData("48 F7 18", 3, Code.Neg_rm64, MemorySize.Int64)]
		[InlineData("48 F7 20", 3, Code.Mul_rm64, MemorySize.UInt64)]
		[InlineData("48 F7 28", 3, Code.Imul_rm64, MemorySize.Int64)]
		[InlineData("48 F7 30", 3, Code.Div_rm64, MemorySize.UInt64)]
		[InlineData("48 F7 38", 3, Code.Idiv_rm64, MemorySize.Int64)]
		void Test64_Grp3_Eq_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("48 F7 D1", 3, Code.Not_rm64, Register.RCX)]
		[InlineData("48 F7 DE", 3, Code.Neg_rm64, Register.RSI)]
		[InlineData("49 F7 D1", 3, Code.Not_rm64, Register.R9)]
		[InlineData("49 F7 DE", 3, Code.Neg_rm64, Register.R14)]
		[InlineData("48 F7 E5", 3, Code.Mul_rm64, Register.RBP)]
		[InlineData("48 F7 EE", 3, Code.Imul_rm64, Register.RSI)]
		[InlineData("48 F7 F7", 3, Code.Div_rm64, Register.RDI)]
		[InlineData("49 F7 F8", 3, Code.Idiv_rm64, Register.R8)]
		[InlineData("49 F7 E5", 3, Code.Mul_rm64, Register.R13)]
		[InlineData("49 F7 EE", 3, Code.Imul_rm64, Register.R14)]
		[InlineData("49 F7 F7", 3, Code.Div_rm64, Register.R15)]
		[InlineData("48 F7 F8", 3, Code.Idiv_rm64, Register.RAX)]
		void Test64_Grp3_Eq_2(string hexBytes, int byteLength, Code code, Register reg) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);
		}
	}
}
