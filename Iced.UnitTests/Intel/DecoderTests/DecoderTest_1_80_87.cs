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
	public sealed class DecoderTest_1_80_87 : DecoderTest {
		[Theory]
		[InlineData("80 00 5A", 3, Code.Add_Eb_Ib, 0x5A)]
		[InlineData("80 08 A5", 3, Code.Or_Eb_Ib, 0xA5)]
		[InlineData("80 10 5A", 3, Code.Adc_Eb_Ib, 0x5A)]
		[InlineData("80 18 A5", 3, Code.Sbb_Eb_Ib, 0xA5)]
		[InlineData("80 20 5A", 3, Code.And_Eb_Ib, 0x5A)]
		[InlineData("80 28 A5", 3, Code.Sub_Eb_Ib, 0xA5)]
		[InlineData("80 30 5A", 3, Code.Xor_Eb_Ib, 0x5A)]
		[InlineData("80 38 A5", 3, Code.Cmp_Eb_Ib, 0xA5)]
		void Test16_Grp1_Eb_Ib_1(string hexBytes, int byteLength, Code code, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

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
		[InlineData("80 C1 5A", 3, Code.Add_Eb_Ib, Register.CL, 0x5A)]
		[InlineData("80 CA A5", 3, Code.Or_Eb_Ib, Register.DL, 0xA5)]
		[InlineData("80 D3 5A", 3, Code.Adc_Eb_Ib, Register.BL, 0x5A)]
		[InlineData("80 DC A5", 3, Code.Sbb_Eb_Ib, Register.AH, 0xA5)]
		[InlineData("80 E5 5A", 3, Code.And_Eb_Ib, Register.CH, 0x5A)]
		[InlineData("80 EE A5", 3, Code.Sub_Eb_Ib, Register.DH, 0xA5)]
		[InlineData("80 F7 5A", 3, Code.Xor_Eb_Ib, Register.BH, 0x5A)]
		[InlineData("80 F8 A5", 3, Code.Cmp_Eb_Ib, Register.AL, 0xA5)]
		void Test16_Grp1_Eb_Ib_2(string hexBytes, int byteLength, Code code, Register reg, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("80 00 5A", 3, Code.Add_Eb_Ib, 0x5A)]
		[InlineData("80 08 A5", 3, Code.Or_Eb_Ib, 0xA5)]
		[InlineData("80 10 5A", 3, Code.Adc_Eb_Ib, 0x5A)]
		[InlineData("80 18 A5", 3, Code.Sbb_Eb_Ib, 0xA5)]
		[InlineData("80 20 5A", 3, Code.And_Eb_Ib, 0x5A)]
		[InlineData("80 28 A5", 3, Code.Sub_Eb_Ib, 0xA5)]
		[InlineData("80 30 5A", 3, Code.Xor_Eb_Ib, 0x5A)]
		[InlineData("80 38 A5", 3, Code.Cmp_Eb_Ib, 0xA5)]
		void Test32_Grp1_Eb_Ib_1(string hexBytes, int byteLength, Code code, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

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
		[InlineData("80 C1 5A", 3, Code.Add_Eb_Ib, Register.CL, 0x5A)]
		[InlineData("80 CA A5", 3, Code.Or_Eb_Ib, Register.DL, 0xA5)]
		[InlineData("80 D3 5A", 3, Code.Adc_Eb_Ib, Register.BL, 0x5A)]
		[InlineData("80 DC A5", 3, Code.Sbb_Eb_Ib, Register.AH, 0xA5)]
		[InlineData("80 E5 5A", 3, Code.And_Eb_Ib, Register.CH, 0x5A)]
		[InlineData("80 EE A5", 3, Code.Sub_Eb_Ib, Register.DH, 0xA5)]
		[InlineData("80 F7 5A", 3, Code.Xor_Eb_Ib, Register.BH, 0x5A)]
		[InlineData("80 F8 A5", 3, Code.Cmp_Eb_Ib, Register.AL, 0xA5)]
		void Test32_Grp1_Eb_Ib_2(string hexBytes, int byteLength, Code code, Register reg, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("80 00 5A", 3, Code.Add_Eb_Ib, 0x5A)]
		[InlineData("80 08 A5", 3, Code.Or_Eb_Ib, 0xA5)]
		[InlineData("80 10 5A", 3, Code.Adc_Eb_Ib, 0x5A)]
		[InlineData("80 18 A5", 3, Code.Sbb_Eb_Ib, 0xA5)]
		[InlineData("80 20 5A", 3, Code.And_Eb_Ib, 0x5A)]
		[InlineData("80 28 A5", 3, Code.Sub_Eb_Ib, 0xA5)]
		[InlineData("80 30 5A", 3, Code.Xor_Eb_Ib, 0x5A)]
		[InlineData("80 38 A5", 3, Code.Cmp_Eb_Ib, 0xA5)]

		[InlineData("44 80 00 5A", 4, Code.Add_Eb_Ib, 0x5A)]
		[InlineData("44 80 08 A5", 4, Code.Or_Eb_Ib, 0xA5)]
		[InlineData("44 80 10 5A", 4, Code.Adc_Eb_Ib, 0x5A)]
		[InlineData("44 80 18 A5", 4, Code.Sbb_Eb_Ib, 0xA5)]
		[InlineData("44 80 20 5A", 4, Code.And_Eb_Ib, 0x5A)]
		[InlineData("44 80 28 A5", 4, Code.Sub_Eb_Ib, 0xA5)]
		[InlineData("44 80 30 5A", 4, Code.Xor_Eb_Ib, 0x5A)]
		[InlineData("44 80 38 A5", 4, Code.Cmp_Eb_Ib, 0xA5)]
		void Test64_Grp1_Eb_Ib_1(string hexBytes, int byteLength, Code code, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

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
		[InlineData("80 C1 5A", 3, Code.Add_Eb_Ib, Register.CL, 0x5A)]
		[InlineData("80 CA A5", 3, Code.Or_Eb_Ib, Register.DL, 0xA5)]
		[InlineData("80 D3 5A", 3, Code.Adc_Eb_Ib, Register.BL, 0x5A)]
		[InlineData("80 DC A5", 3, Code.Sbb_Eb_Ib, Register.AH, 0xA5)]
		[InlineData("80 E5 5A", 3, Code.And_Eb_Ib, Register.CH, 0x5A)]
		[InlineData("80 EE A5", 3, Code.Sub_Eb_Ib, Register.DH, 0xA5)]
		[InlineData("80 F7 5A", 3, Code.Xor_Eb_Ib, Register.BH, 0x5A)]
		[InlineData("80 F8 A5", 3, Code.Cmp_Eb_Ib, Register.AL, 0xA5)]

		[InlineData("40 80 C1 5A", 4, Code.Add_Eb_Ib, Register.CL, 0x5A)]
		[InlineData("40 80 CA A5", 4, Code.Or_Eb_Ib, Register.DL, 0xA5)]
		[InlineData("40 80 D3 5A", 4, Code.Adc_Eb_Ib, Register.BL, 0x5A)]
		[InlineData("40 80 DC A5", 4, Code.Sbb_Eb_Ib, Register.SPL, 0xA5)]
		[InlineData("40 80 E5 5A", 4, Code.And_Eb_Ib, Register.BPL, 0x5A)]
		[InlineData("40 80 EE A5", 4, Code.Sub_Eb_Ib, Register.SIL, 0xA5)]
		[InlineData("40 80 F7 5A", 4, Code.Xor_Eb_Ib, Register.DIL, 0x5A)]
		[InlineData("41 80 F8 A5", 4, Code.Cmp_Eb_Ib, Register.R8L, 0xA5)]
		[InlineData("41 80 C1 5A", 4, Code.Add_Eb_Ib, Register.R9L, 0x5A)]
		[InlineData("41 80 CA A5", 4, Code.Or_Eb_Ib, Register.R10L, 0xA5)]
		[InlineData("41 80 D3 5A", 4, Code.Adc_Eb_Ib, Register.R11L, 0x5A)]
		[InlineData("41 80 DC A5", 4, Code.Sbb_Eb_Ib, Register.R12L, 0xA5)]
		[InlineData("41 80 E5 5A", 4, Code.And_Eb_Ib, Register.R13L, 0x5A)]
		[InlineData("41 80 EE A5", 4, Code.Sub_Eb_Ib, Register.R14L, 0xA5)]
		[InlineData("41 80 F7 5A", 4, Code.Xor_Eb_Ib, Register.R15L, 0x5A)]
		[InlineData("40 80 F8 A5", 4, Code.Cmp_Eb_Ib, Register.AL, 0xA5)]
		void Test64_Grp1_Eb_Ib_2(string hexBytes, int byteLength, Code code, Register reg, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("81 00 5AA5", 4, Code.Add_Ew_Iw, 0xA55A)]
		[InlineData("81 08 A55A", 4, Code.Or_Ew_Iw, 0x5AA5)]
		[InlineData("81 10 5AA5", 4, Code.Adc_Ew_Iw, 0xA55A)]
		[InlineData("81 18 A55A", 4, Code.Sbb_Ew_Iw, 0x5AA5)]
		[InlineData("81 20 5AA5", 4, Code.And_Ew_Iw, 0xA55A)]
		[InlineData("81 28 A55A", 4, Code.Sub_Ew_Iw, 0x5AA5)]
		[InlineData("81 30 5AA5", 4, Code.Xor_Ew_Iw, 0xA55A)]
		[InlineData("81 38 A55A", 4, Code.Cmp_Ew_Iw, 0x5AA5)]
		void Test16_Grp1_Ew_Iw_1(string hexBytes, int byteLength, Code code, ushort immediate16) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

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
		[InlineData("81 C1 5AA5", 4, Code.Add_Ew_Iw, Register.CX, 0xA55A)]
		[InlineData("81 CA A55A", 4, Code.Or_Ew_Iw, Register.DX, 0x5AA5)]
		[InlineData("81 D3 5AA5", 4, Code.Adc_Ew_Iw, Register.BX, 0xA55A)]
		[InlineData("81 DC A55A", 4, Code.Sbb_Ew_Iw, Register.SP, 0x5AA5)]
		[InlineData("81 E5 5AA5", 4, Code.And_Ew_Iw, Register.BP, 0xA55A)]
		[InlineData("81 EE A55A", 4, Code.Sub_Ew_Iw, Register.SI, 0x5AA5)]
		[InlineData("81 F7 5AA5", 4, Code.Xor_Ew_Iw, Register.DI, 0xA55A)]
		[InlineData("81 F8 A55A", 4, Code.Cmp_Ew_Iw, Register.AX, 0x5AA5)]
		void Test16_Grp1_Ew_Iw_2(string hexBytes, int byteLength, Code code, Register reg, ushort immediate16) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Immediate16, instr.Op1Kind);
			Assert.Equal(immediate16, instr.Immediate16);
		}

		[Theory]
		[InlineData("66 81 00 5AA5", 5, Code.Add_Ew_Iw, 0xA55A)]
		[InlineData("66 81 08 A55A", 5, Code.Or_Ew_Iw, 0x5AA5)]
		[InlineData("66 81 10 5AA5", 5, Code.Adc_Ew_Iw, 0xA55A)]
		[InlineData("66 81 18 A55A", 5, Code.Sbb_Ew_Iw, 0x5AA5)]
		[InlineData("66 81 20 5AA5", 5, Code.And_Ew_Iw, 0xA55A)]
		[InlineData("66 81 28 A55A", 5, Code.Sub_Ew_Iw, 0x5AA5)]
		[InlineData("66 81 30 5AA5", 5, Code.Xor_Ew_Iw, 0xA55A)]
		[InlineData("66 81 38 A55A", 5, Code.Cmp_Ew_Iw, 0x5AA5)]
		void Test32_Grp1_Ew_Iw_1(string hexBytes, int byteLength, Code code, ushort immediate16) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

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
		[InlineData("66 81 C1 5AA5", 5, Code.Add_Ew_Iw, Register.CX, 0xA55A)]
		[InlineData("66 81 CA A55A", 5, Code.Or_Ew_Iw, Register.DX, 0x5AA5)]
		[InlineData("66 81 D3 5AA5", 5, Code.Adc_Ew_Iw, Register.BX, 0xA55A)]
		[InlineData("66 81 DC A55A", 5, Code.Sbb_Ew_Iw, Register.SP, 0x5AA5)]
		[InlineData("66 81 E5 5AA5", 5, Code.And_Ew_Iw, Register.BP, 0xA55A)]
		[InlineData("66 81 EE A55A", 5, Code.Sub_Ew_Iw, Register.SI, 0x5AA5)]
		[InlineData("66 81 F7 5AA5", 5, Code.Xor_Ew_Iw, Register.DI, 0xA55A)]
		[InlineData("66 81 F8 A55A", 5, Code.Cmp_Ew_Iw, Register.AX, 0x5AA5)]
		void Test32_Grp1_Ew_Iw_2(string hexBytes, int byteLength, Code code, Register reg, ushort immediate16) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Immediate16, instr.Op1Kind);
			Assert.Equal(immediate16, instr.Immediate16);
		}

		[Theory]
		[InlineData("66 81 00 5AA5", 5, Code.Add_Ew_Iw, 0xA55A)]
		[InlineData("66 81 08 A55A", 5, Code.Or_Ew_Iw, 0x5AA5)]
		[InlineData("66 81 10 5AA5", 5, Code.Adc_Ew_Iw, 0xA55A)]
		[InlineData("66 81 18 A55A", 5, Code.Sbb_Ew_Iw, 0x5AA5)]
		[InlineData("66 81 20 5AA5", 5, Code.And_Ew_Iw, 0xA55A)]
		[InlineData("66 81 28 A55A", 5, Code.Sub_Ew_Iw, 0x5AA5)]
		[InlineData("66 81 30 5AA5", 5, Code.Xor_Ew_Iw, 0xA55A)]
		[InlineData("66 81 38 A55A", 5, Code.Cmp_Ew_Iw, 0x5AA5)]
		void Test64_Grp1_Ew_Iw_1(string hexBytes, int byteLength, Code code, ushort immediate16) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

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
		[InlineData("66 81 C1 5AA5", 5, Code.Add_Ew_Iw, Register.CX, 0xA55A)]
		[InlineData("66 81 CA A55A", 5, Code.Or_Ew_Iw, Register.DX, 0x5AA5)]
		[InlineData("66 81 D3 5AA5", 5, Code.Adc_Ew_Iw, Register.BX, 0xA55A)]
		[InlineData("66 81 DC A55A", 5, Code.Sbb_Ew_Iw, Register.SP, 0x5AA5)]
		[InlineData("66 81 E5 5AA5", 5, Code.And_Ew_Iw, Register.BP, 0xA55A)]
		[InlineData("66 81 EE A55A", 5, Code.Sub_Ew_Iw, Register.SI, 0x5AA5)]
		[InlineData("66 81 F7 5AA5", 5, Code.Xor_Ew_Iw, Register.DI, 0xA55A)]
		[InlineData("66 41 81 F8 A55A", 6, Code.Cmp_Ew_Iw, Register.R8W, 0x5AA5)]

		[InlineData("66 41 81 C1 5AA5", 6, Code.Add_Ew_Iw, Register.R9W, 0xA55A)]
		[InlineData("66 41 81 CA A55A", 6, Code.Or_Ew_Iw, Register.R10W, 0x5AA5)]
		[InlineData("66 41 81 D3 5AA5", 6, Code.Adc_Ew_Iw, Register.R11W, 0xA55A)]
		[InlineData("66 41 81 DC A55A", 6, Code.Sbb_Ew_Iw, Register.R12W, 0x5AA5)]
		[InlineData("66 41 81 E5 5AA5", 6, Code.And_Ew_Iw, Register.R13W, 0xA55A)]
		[InlineData("66 41 81 EE A55A", 6, Code.Sub_Ew_Iw, Register.R14W, 0x5AA5)]
		[InlineData("66 41 81 F7 5AA5", 6, Code.Xor_Ew_Iw, Register.R15W, 0xA55A)]
		[InlineData("66 81 F8 A55A", 5, Code.Cmp_Ew_Iw, Register.AX, 0x5AA5)]
		void Test64_Grp1_Ew_Iw_2(string hexBytes, int byteLength, Code code, Register reg, ushort immediate16) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Immediate16, instr.Op1Kind);
			Assert.Equal(immediate16, instr.Immediate16);
		}

		[Theory]
		[InlineData("66 81 00 5AA51234", 7, Code.Add_Ed_Id, 0x3412A55A)]
		[InlineData("66 81 08 A55A89AB", 7, Code.Or_Ed_Id, 0xAB895AA5)]
		[InlineData("66 81 10 5AA51234", 7, Code.Adc_Ed_Id, 0x3412A55A)]
		[InlineData("66 81 18 A55A89AB", 7, Code.Sbb_Ed_Id, 0xAB895AA5)]
		[InlineData("66 81 20 5AA51234", 7, Code.And_Ed_Id, 0x3412A55A)]
		[InlineData("66 81 28 A55A89AB", 7, Code.Sub_Ed_Id, 0xAB895AA5)]
		[InlineData("66 81 30 5AA51234", 7, Code.Xor_Ed_Id, 0x3412A55A)]
		[InlineData("66 81 38 A55A89AB", 7, Code.Cmp_Ed_Id, 0xAB895AA5)]
		void Test16_Grp1_Ed_Id_1(string hexBytes, int byteLength, Code code, uint immediate32) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

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
		[InlineData("66 81 C1 5AA51234", 7, Code.Add_Ed_Id, Register.ECX, 0x3412A55A)]
		[InlineData("66 81 CA A55A89AB", 7, Code.Or_Ed_Id, Register.EDX, 0xAB895AA5)]
		[InlineData("66 81 D3 5AA51234", 7, Code.Adc_Ed_Id, Register.EBX, 0x3412A55A)]
		[InlineData("66 81 DC A55A89AB", 7, Code.Sbb_Ed_Id, Register.ESP, 0xAB895AA5)]
		[InlineData("66 81 E5 5AA51234", 7, Code.And_Ed_Id, Register.EBP, 0x3412A55A)]
		[InlineData("66 81 EE A55A89AB", 7, Code.Sub_Ed_Id, Register.ESI, 0xAB895AA5)]
		[InlineData("66 81 F7 5AA51234", 7, Code.Xor_Ed_Id, Register.EDI, 0x3412A55A)]
		[InlineData("66 81 F8 A55A89AB", 7, Code.Cmp_Ed_Id, Register.EAX, 0xAB895AA5)]
		void Test16_Grp1_Ed_Id_2(string hexBytes, int byteLength, Code code, Register reg, uint immediate32) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Immediate32, instr.Op1Kind);
			Assert.Equal(immediate32, instr.Immediate32);
		}

		[Theory]
		[InlineData("81 00 5AA51234", 6, Code.Add_Ed_Id, 0x3412A55A)]
		[InlineData("81 08 A55A89AB", 6, Code.Or_Ed_Id, 0xAB895AA5)]
		[InlineData("81 10 5AA51234", 6, Code.Adc_Ed_Id, 0x3412A55A)]
		[InlineData("81 18 A55A89AB", 6, Code.Sbb_Ed_Id, 0xAB895AA5)]
		[InlineData("81 20 5AA51234", 6, Code.And_Ed_Id, 0x3412A55A)]
		[InlineData("81 28 A55A89AB", 6, Code.Sub_Ed_Id, 0xAB895AA5)]
		[InlineData("81 30 5AA51234", 6, Code.Xor_Ed_Id, 0x3412A55A)]
		[InlineData("81 38 A55A89AB", 6, Code.Cmp_Ed_Id, 0xAB895AA5)]
		void Test32_Grp1_Ed_Id_1(string hexBytes, int byteLength, Code code, uint immediate32) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

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
		[InlineData("81 C1 5AA51234", 6, Code.Add_Ed_Id, Register.ECX, 0x3412A55A)]
		[InlineData("81 CA A55A89AB", 6, Code.Or_Ed_Id, Register.EDX, 0xAB895AA5)]
		[InlineData("81 D3 5AA51234", 6, Code.Adc_Ed_Id, Register.EBX, 0x3412A55A)]
		[InlineData("81 DC A55A89AB", 6, Code.Sbb_Ed_Id, Register.ESP, 0xAB895AA5)]
		[InlineData("81 E5 5AA51234", 6, Code.And_Ed_Id, Register.EBP, 0x3412A55A)]
		[InlineData("81 EE A55A89AB", 6, Code.Sub_Ed_Id, Register.ESI, 0xAB895AA5)]
		[InlineData("81 F7 5AA51234", 6, Code.Xor_Ed_Id, Register.EDI, 0x3412A55A)]
		[InlineData("81 F8 A55A89AB", 6, Code.Cmp_Ed_Id, Register.EAX, 0xAB895AA5)]
		void Test32_Grp1_Ed_Id_2(string hexBytes, int byteLength, Code code, Register reg, uint immediate32) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Immediate32, instr.Op1Kind);
			Assert.Equal(immediate32, instr.Immediate32);
		}

		[Theory]
		[InlineData("81 00 5AA51234", 6, Code.Add_Ed_Id, 0x3412A55A)]
		[InlineData("81 08 A55A89AB", 6, Code.Or_Ed_Id, 0xAB895AA5)]
		[InlineData("81 10 5AA51234", 6, Code.Adc_Ed_Id, 0x3412A55A)]
		[InlineData("81 18 A55A89AB", 6, Code.Sbb_Ed_Id, 0xAB895AA5)]
		[InlineData("81 20 5AA51234", 6, Code.And_Ed_Id, 0x3412A55A)]
		[InlineData("81 28 A55A89AB", 6, Code.Sub_Ed_Id, 0xAB895AA5)]
		[InlineData("81 30 5AA51234", 6, Code.Xor_Ed_Id, 0x3412A55A)]
		[InlineData("81 38 A55A89AB", 6, Code.Cmp_Ed_Id, 0xAB895AA5)]
		void Test64_Grp1_Ed_Id_1(string hexBytes, int byteLength, Code code, uint immediate32) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

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
		[InlineData("81 C1 5AA51234", 6, Code.Add_Ed_Id, Register.ECX, 0x3412A55A)]
		[InlineData("81 CA A55A89AB", 6, Code.Or_Ed_Id, Register.EDX, 0xAB895AA5)]
		[InlineData("81 D3 5AA51234", 6, Code.Adc_Ed_Id, Register.EBX, 0x3412A55A)]
		[InlineData("81 DC A55A89AB", 6, Code.Sbb_Ed_Id, Register.ESP, 0xAB895AA5)]
		[InlineData("81 E5 5AA51234", 6, Code.And_Ed_Id, Register.EBP, 0x3412A55A)]
		[InlineData("81 EE A55A89AB", 6, Code.Sub_Ed_Id, Register.ESI, 0xAB895AA5)]
		[InlineData("81 F7 5AA51234", 6, Code.Xor_Ed_Id, Register.EDI, 0x3412A55A)]
		[InlineData("41 81 F8 A55A89AB", 7, Code.Cmp_Ed_Id, Register.R8D, 0xAB895AA5)]

		[InlineData("41 81 C1 5AA51234", 7, Code.Add_Ed_Id, Register.R9D, 0x3412A55A)]
		[InlineData("41 81 CA A55A89AB", 7, Code.Or_Ed_Id, Register.R10D, 0xAB895AA5)]
		[InlineData("41 81 D3 5AA51234", 7, Code.Adc_Ed_Id, Register.R11D, 0x3412A55A)]
		[InlineData("41 81 DC A55A89AB", 7, Code.Sbb_Ed_Id, Register.R12D, 0xAB895AA5)]
		[InlineData("41 81 E5 5AA51234", 7, Code.And_Ed_Id, Register.R13D, 0x3412A55A)]
		[InlineData("41 81 EE A55A89AB", 7, Code.Sub_Ed_Id, Register.R14D, 0xAB895AA5)]
		[InlineData("41 81 F7 5AA51234", 7, Code.Xor_Ed_Id, Register.R15D, 0x3412A55A)]
		[InlineData("81 F8 A55A89AB", 6, Code.Cmp_Ed_Id, Register.EAX, 0xAB895AA5)]
		void Test64_Grp1_Ed_Id_2(string hexBytes, int byteLength, Code code, Register reg, uint immediate32) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Immediate32, instr.Op1Kind);
			Assert.Equal(immediate32, instr.Immediate32);
		}

		[Theory]
		[InlineData("48 81 00 5AA51234", 7, Code.Add_Eq_Id64, 0x3412A55AUL)]
		[InlineData("48 81 08 A55A89AB", 7, Code.Or_Eq_Id64, 0xFFFFFFFFAB895AA5UL)]
		[InlineData("48 81 10 5AA51234", 7, Code.Adc_Eq_Id64, 0x3412A55AUL)]
		[InlineData("48 81 18 A55A89AB", 7, Code.Sbb_Eq_Id64, 0xFFFFFFFFAB895AA5UL)]
		[InlineData("48 81 20 5AA51234", 7, Code.And_Eq_Id64, 0x3412A55AUL)]
		[InlineData("48 81 28 A55A89AB", 7, Code.Sub_Eq_Id64, 0xFFFFFFFFAB895AA5UL)]
		[InlineData("48 81 30 5AA51234", 7, Code.Xor_Eq_Id64, 0x3412A55AUL)]
		[InlineData("48 81 38 A55A89AB", 7, Code.Cmp_Eq_Id64, 0xFFFFFFFFAB895AA5UL)]
		void Test64_Grp1_EqId_1(string hexBytes, int byteLength, Code code, ulong immediate64) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

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
		[InlineData("48 81 C1 5AA51234", 7, Code.Add_Eq_Id64, Register.RCX, 0x3412A55AUL)]
		[InlineData("48 81 CA A55A89AB", 7, Code.Or_Eq_Id64, Register.RDX, 0xFFFFFFFFAB895AA5UL)]
		[InlineData("48 81 D3 5AA51234", 7, Code.Adc_Eq_Id64, Register.RBX, 0x3412A55AUL)]
		[InlineData("48 81 DC A55A89AB", 7, Code.Sbb_Eq_Id64, Register.RSP, 0xFFFFFFFFAB895AA5UL)]
		[InlineData("48 81 E5 5AA51234", 7, Code.And_Eq_Id64, Register.RBP, 0x3412A55AUL)]
		[InlineData("48 81 EE A55A89AB", 7, Code.Sub_Eq_Id64, Register.RSI, 0xFFFFFFFFAB895AA5UL)]
		[InlineData("48 81 F7 5AA51234", 7, Code.Xor_Eq_Id64, Register.RDI, 0x3412A55AUL)]
		[InlineData("49 81 F8 A55A89AB", 7, Code.Cmp_Eq_Id64, Register.R8, 0xFFFFFFFFAB895AA5UL)]

		[InlineData("49 81 C1 5AA51234", 7, Code.Add_Eq_Id64, Register.R9, 0x3412A55AUL)]
		[InlineData("49 81 CA A55A89AB", 7, Code.Or_Eq_Id64, Register.R10, 0xFFFFFFFFAB895AA5UL)]
		[InlineData("49 81 D3 5AA51234", 7, Code.Adc_Eq_Id64, Register.R11, 0x3412A55AUL)]
		[InlineData("49 81 DC A55A89AB", 7, Code.Sbb_Eq_Id64, Register.R12, 0xFFFFFFFFAB895AA5UL)]
		[InlineData("49 81 E5 5AA51234", 7, Code.And_Eq_Id64, Register.R13, 0x3412A55AUL)]
		[InlineData("49 81 EE A55A89AB", 7, Code.Sub_Eq_Id64, Register.R14, 0xFFFFFFFFAB895AA5UL)]
		[InlineData("49 81 F7 5AA51234", 7, Code.Xor_Eq_Id64, Register.R15, 0x3412A55AUL)]
		[InlineData("48 81 F8 A55A89AB", 7, Code.Cmp_Eq_Id64, Register.RAX, 0xFFFFFFFFAB895AA5UL)]
		void Test64_Grp1_EqId_2(string hexBytes, int byteLength, Code code, Register reg, ulong immediate64) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Immediate32to64, instr.Op1Kind);
			Assert.Equal((long)immediate64, instr.Immediate32to64);
		}

		[Theory]
		[InlineData("82 00 5A", 3, Code.Add_Eb_Ib, 0x5A)]
		[InlineData("82 08 A5", 3, Code.Or_Eb_Ib, 0xA5)]
		[InlineData("82 10 5A", 3, Code.Adc_Eb_Ib, 0x5A)]
		[InlineData("82 18 A5", 3, Code.Sbb_Eb_Ib, 0xA5)]
		[InlineData("82 20 5A", 3, Code.And_Eb_Ib, 0x5A)]
		[InlineData("82 28 A5", 3, Code.Sub_Eb_Ib, 0xA5)]
		[InlineData("82 30 5A", 3, Code.Xor_Eb_Ib, 0x5A)]
		[InlineData("82 38 A5", 3, Code.Cmp_Eb_Ib, 0xA5)]
		void Test16_Grp1_82_Eb_Ib_1(string hexBytes, int byteLength, Code code, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

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
		[InlineData("82 C1 5A", 3, Code.Add_Eb_Ib, Register.CL, 0x5A)]
		[InlineData("82 CA A5", 3, Code.Or_Eb_Ib, Register.DL, 0xA5)]
		[InlineData("82 D3 5A", 3, Code.Adc_Eb_Ib, Register.BL, 0x5A)]
		[InlineData("82 DC A5", 3, Code.Sbb_Eb_Ib, Register.AH, 0xA5)]
		[InlineData("82 E5 5A", 3, Code.And_Eb_Ib, Register.CH, 0x5A)]
		[InlineData("82 EE A5", 3, Code.Sub_Eb_Ib, Register.DH, 0xA5)]
		[InlineData("82 F7 5A", 3, Code.Xor_Eb_Ib, Register.BH, 0x5A)]
		[InlineData("82 F8 A5", 3, Code.Cmp_Eb_Ib, Register.AL, 0xA5)]
		void Test16_Grp1_82_Eb_Ib_2(string hexBytes, int byteLength, Code code, Register reg, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("82 00 5A", 3, Code.Add_Eb_Ib, 0x5A)]
		[InlineData("82 08 A5", 3, Code.Or_Eb_Ib, 0xA5)]
		[InlineData("82 10 5A", 3, Code.Adc_Eb_Ib, 0x5A)]
		[InlineData("82 18 A5", 3, Code.Sbb_Eb_Ib, 0xA5)]
		[InlineData("82 20 5A", 3, Code.And_Eb_Ib, 0x5A)]
		[InlineData("82 28 A5", 3, Code.Sub_Eb_Ib, 0xA5)]
		[InlineData("82 30 5A", 3, Code.Xor_Eb_Ib, 0x5A)]
		[InlineData("82 38 A5", 3, Code.Cmp_Eb_Ib, 0xA5)]
		void Test32_Grp1_82_Eb_Ib_1(string hexBytes, int byteLength, Code code, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

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
		[InlineData("82 C1 5A", 3, Code.Add_Eb_Ib, Register.CL, 0x5A)]
		[InlineData("82 CA A5", 3, Code.Or_Eb_Ib, Register.DL, 0xA5)]
		[InlineData("82 D3 5A", 3, Code.Adc_Eb_Ib, Register.BL, 0x5A)]
		[InlineData("82 DC A5", 3, Code.Sbb_Eb_Ib, Register.AH, 0xA5)]
		[InlineData("82 E5 5A", 3, Code.And_Eb_Ib, Register.CH, 0x5A)]
		[InlineData("82 EE A5", 3, Code.Sub_Eb_Ib, Register.DH, 0xA5)]
		[InlineData("82 F7 5A", 3, Code.Xor_Eb_Ib, Register.BH, 0x5A)]
		[InlineData("82 F8 A5", 3, Code.Cmp_Eb_Ib, Register.AL, 0xA5)]
		void Test32_Grp1_82_Eb_Ib_2(string hexBytes, int byteLength, Code code, Register reg, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("83 00 5A", 3, Code.Add_Ew_Ib16, 0x005A)]
		[InlineData("83 08 A5", 3, Code.Or_Ew_Ib16, 0xFFA5)]
		[InlineData("83 10 5A", 3, Code.Adc_Ew_Ib16, 0x005A)]
		[InlineData("83 18 A5", 3, Code.Sbb_Ew_Ib16, 0xFFA5)]
		[InlineData("83 20 5A", 3, Code.And_Ew_Ib16, 0x005A)]
		[InlineData("83 28 A5", 3, Code.Sub_Ew_Ib16, 0xFFA5)]
		[InlineData("83 30 5A", 3, Code.Xor_Ew_Ib16, 0x005A)]
		[InlineData("83 38 A5", 3, Code.Cmp_Ew_Ib16, 0xFFA5)]
		void Test16_Grp1_Ew_Ib_1(string hexBytes, int byteLength, Code code, ushort immediate16) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8to16, instr.Op1Kind);
			Assert.Equal((short)immediate16, instr.Immediate8to16);
		}

		[Theory]
		[InlineData("83 C1 5A", 3, Code.Add_Ew_Ib16, Register.CX, 0x005A)]
		[InlineData("83 CA A5", 3, Code.Or_Ew_Ib16, Register.DX, 0xFFA5)]
		[InlineData("83 D3 5A", 3, Code.Adc_Ew_Ib16, Register.BX, 0x005A)]
		[InlineData("83 DC A5", 3, Code.Sbb_Ew_Ib16, Register.SP, 0xFFA5)]
		[InlineData("83 E5 5A", 3, Code.And_Ew_Ib16, Register.BP, 0x005A)]
		[InlineData("83 EE A5", 3, Code.Sub_Ew_Ib16, Register.SI, 0xFFA5)]
		[InlineData("83 F7 5A", 3, Code.Xor_Ew_Ib16, Register.DI, 0x005A)]
		[InlineData("83 F8 A5", 3, Code.Cmp_Ew_Ib16, Register.AX, 0xFFA5)]
		void Test16_Grp1_Ew_Ib_2(string hexBytes, int byteLength, Code code, Register reg, ushort immediate16) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8to16, instr.Op1Kind);
			Assert.Equal((short)immediate16, instr.Immediate8to16);
		}

		[Theory]
		[InlineData("66 83 00 5A", 4, Code.Add_Ew_Ib16, 0x005A)]
		[InlineData("66 83 08 A5", 4, Code.Or_Ew_Ib16, 0xFFA5)]
		[InlineData("66 83 10 5A", 4, Code.Adc_Ew_Ib16, 0x005A)]
		[InlineData("66 83 18 A5", 4, Code.Sbb_Ew_Ib16, 0xFFA5)]
		[InlineData("66 83 20 5A", 4, Code.And_Ew_Ib16, 0x005A)]
		[InlineData("66 83 28 A5", 4, Code.Sub_Ew_Ib16, 0xFFA5)]
		[InlineData("66 83 30 5A", 4, Code.Xor_Ew_Ib16, 0x005A)]
		[InlineData("66 83 38 A5", 4, Code.Cmp_Ew_Ib16, 0xFFA5)]
		void Test32_Grp1_Ew_Ib_1(string hexBytes, int byteLength, Code code, ushort immediate16) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8to16, instr.Op1Kind);
			Assert.Equal((short)immediate16, instr.Immediate8to16);
		}

		[Theory]
		[InlineData("66 83 C1 5A", 4, Code.Add_Ew_Ib16, Register.CX, 0x005A)]
		[InlineData("66 83 CA A5", 4, Code.Or_Ew_Ib16, Register.DX, 0xFFA5)]
		[InlineData("66 83 D3 5A", 4, Code.Adc_Ew_Ib16, Register.BX, 0x005A)]
		[InlineData("66 83 DC A5", 4, Code.Sbb_Ew_Ib16, Register.SP, 0xFFA5)]
		[InlineData("66 83 E5 5A", 4, Code.And_Ew_Ib16, Register.BP, 0x005A)]
		[InlineData("66 83 EE A5", 4, Code.Sub_Ew_Ib16, Register.SI, 0xFFA5)]
		[InlineData("66 83 F7 5A", 4, Code.Xor_Ew_Ib16, Register.DI, 0x005A)]
		[InlineData("66 83 F8 A5", 4, Code.Cmp_Ew_Ib16, Register.AX, 0xFFA5)]
		void Test32_Grp1_Ew_Ib_2(string hexBytes, int byteLength, Code code, Register reg, ushort immediate16) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8to16, instr.Op1Kind);
			Assert.Equal((short)immediate16, instr.Immediate8to16);
		}

		[Theory]
		[InlineData("66 83 00 5A", 4, Code.Add_Ew_Ib16, 0x005A)]
		[InlineData("66 83 08 A5", 4, Code.Or_Ew_Ib16, 0xFFA5)]
		[InlineData("66 83 10 5A", 4, Code.Adc_Ew_Ib16, 0x005A)]
		[InlineData("66 83 18 A5", 4, Code.Sbb_Ew_Ib16, 0xFFA5)]
		[InlineData("66 83 20 5A", 4, Code.And_Ew_Ib16, 0x005A)]
		[InlineData("66 83 28 A5", 4, Code.Sub_Ew_Ib16, 0xFFA5)]
		[InlineData("66 83 30 5A", 4, Code.Xor_Ew_Ib16, 0x005A)]
		[InlineData("66 83 38 A5", 4, Code.Cmp_Ew_Ib16, 0xFFA5)]
		void Test64_Grp1_Ew_Ib_1(string hexBytes, int byteLength, Code code, ushort immediate16) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8to16, instr.Op1Kind);
			Assert.Equal((short)immediate16, instr.Immediate8to16);
		}

		[Theory]
		[InlineData("66 83 C1 5A", 4, Code.Add_Ew_Ib16, Register.CX, 0x005A)]
		[InlineData("66 83 CA A5", 4, Code.Or_Ew_Ib16, Register.DX, 0xFFA5)]
		[InlineData("66 83 D3 5A", 4, Code.Adc_Ew_Ib16, Register.BX, 0x005A)]
		[InlineData("66 83 DC A5", 4, Code.Sbb_Ew_Ib16, Register.SP, 0xFFA5)]
		[InlineData("66 83 E5 5A", 4, Code.And_Ew_Ib16, Register.BP, 0x005A)]
		[InlineData("66 83 EE A5", 4, Code.Sub_Ew_Ib16, Register.SI, 0xFFA5)]
		[InlineData("66 83 F7 5A", 4, Code.Xor_Ew_Ib16, Register.DI, 0x005A)]
		[InlineData("66 41 83 F8 A5", 5, Code.Cmp_Ew_Ib16, Register.R8W, 0xFFA5)]

		[InlineData("66 41 83 C1 5A", 5, Code.Add_Ew_Ib16, Register.R9W, 0x005A)]
		[InlineData("66 41 83 CA A5", 5, Code.Or_Ew_Ib16, Register.R10W, 0xFFA5)]
		[InlineData("66 41 83 D3 5A", 5, Code.Adc_Ew_Ib16, Register.R11W, 0x005A)]
		[InlineData("66 41 83 DC A5", 5, Code.Sbb_Ew_Ib16, Register.R12W, 0xFFA5)]
		[InlineData("66 41 83 E5 5A", 5, Code.And_Ew_Ib16, Register.R13W, 0x005A)]
		[InlineData("66 41 83 EE A5", 5, Code.Sub_Ew_Ib16, Register.R14W, 0xFFA5)]
		[InlineData("66 41 83 F7 5A", 5, Code.Xor_Ew_Ib16, Register.R15W, 0x005A)]
		[InlineData("66 83 F8 A5", 4, Code.Cmp_Ew_Ib16, Register.AX, 0xFFA5)]
		void Test64_Grp1_Ew_Ib_2(string hexBytes, int byteLength, Code code, Register reg, ushort immediate16) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8to16, instr.Op1Kind);
			Assert.Equal((short)immediate16, instr.Immediate8to16);
		}

		[Theory]
		[InlineData("66 83 00 5A", 4, Code.Add_Ed_Ib32, 0x0000005A)]
		[InlineData("66 83 08 A5", 4, Code.Or_Ed_Ib32, 0xFFFFFFA5)]
		[InlineData("66 83 10 5A", 4, Code.Adc_Ed_Ib32, 0x0000005A)]
		[InlineData("66 83 18 A5", 4, Code.Sbb_Ed_Ib32, 0xFFFFFFA5)]
		[InlineData("66 83 20 5A", 4, Code.And_Ed_Ib32, 0x0000005A)]
		[InlineData("66 83 28 A5", 4, Code.Sub_Ed_Ib32, 0xFFFFFFA5)]
		[InlineData("66 83 30 5A", 4, Code.Xor_Ed_Ib32, 0x0000005A)]
		[InlineData("66 83 38 A5", 4, Code.Cmp_Ed_Ib32, 0xFFFFFFA5)]
		void Test16_Grp1_Ed_Ib_1(string hexBytes, int byteLength, Code code, uint immediate32) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8to32, instr.Op1Kind);
			Assert.Equal((int)immediate32, instr.Immediate8to32);
		}

		[Theory]
		[InlineData("66 83 C1 5A", 4, Code.Add_Ed_Ib32, Register.ECX, 0x0000005A)]
		[InlineData("66 83 CA A5", 4, Code.Or_Ed_Ib32, Register.EDX, 0xFFFFFFA5)]
		[InlineData("66 83 D3 5A", 4, Code.Adc_Ed_Ib32, Register.EBX, 0x0000005A)]
		[InlineData("66 83 DC A5", 4, Code.Sbb_Ed_Ib32, Register.ESP, 0xFFFFFFA5)]
		[InlineData("66 83 E5 5A", 4, Code.And_Ed_Ib32, Register.EBP, 0x0000005A)]
		[InlineData("66 83 EE A5", 4, Code.Sub_Ed_Ib32, Register.ESI, 0xFFFFFFA5)]
		[InlineData("66 83 F7 5A", 4, Code.Xor_Ed_Ib32, Register.EDI, 0x0000005A)]
		[InlineData("66 83 F8 A5", 4, Code.Cmp_Ed_Ib32, Register.EAX, 0xFFFFFFA5)]
		void Test16_Grp1_Ed_Ib_2(string hexBytes, int byteLength, Code code, Register reg, uint immediate32) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8to32, instr.Op1Kind);
			Assert.Equal((int)immediate32, instr.Immediate8to32);
		}

		[Theory]
		[InlineData("83 00 5A", 3, Code.Add_Ed_Ib32, 0x0000005A)]
		[InlineData("83 08 A5", 3, Code.Or_Ed_Ib32, 0xFFFFFFA5)]
		[InlineData("83 10 5A", 3, Code.Adc_Ed_Ib32, 0x0000005A)]
		[InlineData("83 18 A5", 3, Code.Sbb_Ed_Ib32, 0xFFFFFFA5)]
		[InlineData("83 20 5A", 3, Code.And_Ed_Ib32, 0x0000005A)]
		[InlineData("83 28 A5", 3, Code.Sub_Ed_Ib32, 0xFFFFFFA5)]
		[InlineData("83 30 5A", 3, Code.Xor_Ed_Ib32, 0x0000005A)]
		[InlineData("83 38 A5", 3, Code.Cmp_Ed_Ib32, 0xFFFFFFA5)]
		void Test32_Grp1_Ed_Ib_1(string hexBytes, int byteLength, Code code, uint immediate32) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8to32, instr.Op1Kind);
			Assert.Equal((int)immediate32, instr.Immediate8to32);
		}

		[Theory]
		[InlineData("83 C1 5A", 3, Code.Add_Ed_Ib32, Register.ECX, 0x0000005A)]
		[InlineData("83 CA A5", 3, Code.Or_Ed_Ib32, Register.EDX, 0xFFFFFFA5)]
		[InlineData("83 D3 5A", 3, Code.Adc_Ed_Ib32, Register.EBX, 0x0000005A)]
		[InlineData("83 DC A5", 3, Code.Sbb_Ed_Ib32, Register.ESP, 0xFFFFFFA5)]
		[InlineData("83 E5 5A", 3, Code.And_Ed_Ib32, Register.EBP, 0x0000005A)]
		[InlineData("83 EE A5", 3, Code.Sub_Ed_Ib32, Register.ESI, 0xFFFFFFA5)]
		[InlineData("83 F7 5A", 3, Code.Xor_Ed_Ib32, Register.EDI, 0x0000005A)]
		[InlineData("83 F8 A5", 3, Code.Cmp_Ed_Ib32, Register.EAX, 0xFFFFFFA5)]
		void Test32_Grp1_Ed_Ib_2(string hexBytes, int byteLength, Code code, Register reg, uint immediate32) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8to32, instr.Op1Kind);
			Assert.Equal((int)immediate32, instr.Immediate8to32);
		}

		[Theory]
		[InlineData("83 00 5A", 3, Code.Add_Ed_Ib32, 0x0000005A)]
		[InlineData("83 08 A5", 3, Code.Or_Ed_Ib32, 0xFFFFFFA5)]
		[InlineData("83 10 5A", 3, Code.Adc_Ed_Ib32, 0x0000005A)]
		[InlineData("83 18 A5", 3, Code.Sbb_Ed_Ib32, 0xFFFFFFA5)]
		[InlineData("83 20 5A", 3, Code.And_Ed_Ib32, 0x0000005A)]
		[InlineData("83 28 A5", 3, Code.Sub_Ed_Ib32, 0xFFFFFFA5)]
		[InlineData("83 30 5A", 3, Code.Xor_Ed_Ib32, 0x0000005A)]
		[InlineData("83 38 A5", 3, Code.Cmp_Ed_Ib32, 0xFFFFFFA5)]
		void Test64_Grp1_Ed_Ib_1(string hexBytes, int byteLength, Code code, uint immediate32) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8to32, instr.Op1Kind);
			Assert.Equal((int)immediate32, instr.Immediate8to32);
		}

		[Theory]
		[InlineData("83 C1 5A", 3, Code.Add_Ed_Ib32, Register.ECX, 0x0000005A)]
		[InlineData("83 CA A5", 3, Code.Or_Ed_Ib32, Register.EDX, 0xFFFFFFA5)]
		[InlineData("83 D3 5A", 3, Code.Adc_Ed_Ib32, Register.EBX, 0x0000005A)]
		[InlineData("83 DC A5", 3, Code.Sbb_Ed_Ib32, Register.ESP, 0xFFFFFFA5)]
		[InlineData("83 E5 5A", 3, Code.And_Ed_Ib32, Register.EBP, 0x0000005A)]
		[InlineData("83 EE A5", 3, Code.Sub_Ed_Ib32, Register.ESI, 0xFFFFFFA5)]
		[InlineData("83 F7 5A", 3, Code.Xor_Ed_Ib32, Register.EDI, 0x0000005A)]
		[InlineData("41 83 F8 A5", 4, Code.Cmp_Ed_Ib32, Register.R8D, 0xFFFFFFA5)]

		[InlineData("41 83 C1 5A", 4, Code.Add_Ed_Ib32, Register.R9D, 0x0000005A)]
		[InlineData("41 83 CA A5", 4, Code.Or_Ed_Ib32, Register.R10D, 0xFFFFFFA5)]
		[InlineData("41 83 D3 5A", 4, Code.Adc_Ed_Ib32, Register.R11D, 0x0000005A)]
		[InlineData("41 83 DC A5", 4, Code.Sbb_Ed_Ib32, Register.R12D, 0xFFFFFFA5)]
		[InlineData("41 83 E5 5A", 4, Code.And_Ed_Ib32, Register.R13D, 0x0000005A)]
		[InlineData("41 83 EE A5", 4, Code.Sub_Ed_Ib32, Register.R14D, 0xFFFFFFA5)]
		[InlineData("41 83 F7 5A", 4, Code.Xor_Ed_Ib32, Register.R15D, 0x0000005A)]
		[InlineData("83 F8 A5", 3, Code.Cmp_Ed_Ib32, Register.EAX, 0xFFFFFFA5)]
		void Test64_Grp1_Ed_Ib_2(string hexBytes, int byteLength, Code code, Register reg, uint immediate32) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8to32, instr.Op1Kind);
			Assert.Equal((int)immediate32, instr.Immediate8to32);
		}

		[Theory]
		[InlineData("48 83 00 5A", 4, Code.Add_Eq_Ib64, 0x0000005AUL)]
		[InlineData("48 83 08 A5", 4, Code.Or_Eq_Ib64, 0xFFFFFFFFFFFFFFA5UL)]
		[InlineData("48 83 10 5A", 4, Code.Adc_Eq_Ib64, 0x0000005AUL)]
		[InlineData("48 83 18 A5", 4, Code.Sbb_Eq_Ib64, 0xFFFFFFFFFFFFFFA5UL)]
		[InlineData("48 83 20 5A", 4, Code.And_Eq_Ib64, 0x0000005AUL)]
		[InlineData("48 83 28 A5", 4, Code.Sub_Eq_Ib64, 0xFFFFFFFFFFFFFFA5UL)]
		[InlineData("48 83 30 5A", 4, Code.Xor_Eq_Ib64, 0x0000005AUL)]
		[InlineData("48 83 38 A5", 4, Code.Cmp_Eq_Ib64, 0xFFFFFFFFFFFFFFA5UL)]
		void Test64_Grp1_Eq_Ib_1(string hexBytes, int byteLength, Code code, ulong immediate64) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt64, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8to64, instr.Op1Kind);
			Assert.Equal((long)immediate64, instr.Immediate8to64);
		}

		[Theory]
		[InlineData("48 83 C1 5A", 4, Code.Add_Eq_Ib64, Register.RCX, 0x0000005AUL)]
		[InlineData("48 83 CA A5", 4, Code.Or_Eq_Ib64, Register.RDX, 0xFFFFFFFFFFFFFFA5UL)]
		[InlineData("48 83 D3 5A", 4, Code.Adc_Eq_Ib64, Register.RBX, 0x0000005AUL)]
		[InlineData("48 83 DC A5", 4, Code.Sbb_Eq_Ib64, Register.RSP, 0xFFFFFFFFFFFFFFA5UL)]
		[InlineData("48 83 E5 5A", 4, Code.And_Eq_Ib64, Register.RBP, 0x0000005AUL)]
		[InlineData("48 83 EE A5", 4, Code.Sub_Eq_Ib64, Register.RSI, 0xFFFFFFFFFFFFFFA5UL)]
		[InlineData("48 83 F7 5A", 4, Code.Xor_Eq_Ib64, Register.RDI, 0x0000005AUL)]
		[InlineData("49 83 F8 A5", 4, Code.Cmp_Eq_Ib64, Register.R8, 0xFFFFFFFFFFFFFFA5UL)]

		[InlineData("49 83 C1 5A", 4, Code.Add_Eq_Ib64, Register.R9, 0x0000005AUL)]
		[InlineData("49 83 CA A5", 4, Code.Or_Eq_Ib64, Register.R10, 0xFFFFFFFFFFFFFFA5UL)]
		[InlineData("49 83 D3 5A", 4, Code.Adc_Eq_Ib64, Register.R11, 0x0000005AUL)]
		[InlineData("49 83 DC A5", 4, Code.Sbb_Eq_Ib64, Register.R12, 0xFFFFFFFFFFFFFFA5UL)]
		[InlineData("49 83 E5 5A", 4, Code.And_Eq_Ib64, Register.R13, 0x0000005AUL)]
		[InlineData("49 83 EE A5", 4, Code.Sub_Eq_Ib64, Register.R14, 0xFFFFFFFFFFFFFFA5UL)]
		[InlineData("49 83 F7 5A", 4, Code.Xor_Eq_Ib64, Register.R15, 0x0000005AUL)]
		[InlineData("48 83 F8 A5", 4, Code.Cmp_Eq_Ib64, Register.RAX, 0xFFFFFFFFFFFFFFA5UL)]
		void Test64_Grp1_Eq_Ib_2(string hexBytes, int byteLength, Code code, Register reg, ulong immediate64) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8to64, instr.Op1Kind);
			Assert.Equal((long)immediate64, instr.Immediate8to64);
		}

		[Fact]
		void Test16_Test_Eb_Gb_1() {
			var decoder = CreateDecoder16("84 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Test_Eb_Gb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.DH, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.CL, instr.Op1Register);
		}

		[Fact]
		void Test16_Test_Eb_Gb_2() {
			var decoder = CreateDecoder16("84 38");
			var instr = decoder.Decode();

			Assert.Equal(Code.Test_Eb_Gb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.BH, instr.Op1Register);
		}

		[Fact]
		void Test32_Test_Eb_Gb_1() {
			var decoder = CreateDecoder32("84 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Test_Eb_Gb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.DH, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.CL, instr.Op1Register);
		}

		[Fact]
		void Test32_Test_Eb_Gb_2() {
			var decoder = CreateDecoder32("84 38");
			var instr = decoder.Decode();

			Assert.Equal(Code.Test_Eb_Gb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.BH, instr.Op1Register);
		}

		[Theory]
		[InlineData("84 CE", 2, Register.DH, Register.CL)]
		[InlineData("40 84 D5", 3, Register.BPL, Register.DL)]
		[InlineData("40 84 FA", 3, Register.DL, Register.DIL)]
		[InlineData("45 84 F0", 3, Register.R8L, Register.R14L)]
		[InlineData("41 84 D9", 3, Register.R9L, Register.BL)]
		[InlineData("44 84 EC", 3, Register.SPL, Register.R13L)]
		[InlineData("66 67 4E 84 EC", 5, Register.SPL, Register.R13L)]
		void Test64_Test_Eb_Gb_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Test_Eb_Gb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}

		[Fact]
		void Test64_Test_Eb_Gb_2() {
			var decoder = CreateDecoder64("84 38");
			var instr = decoder.Decode();

			Assert.Equal(Code.Test_Eb_Gb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.BH, instr.Op1Register);
		}

		[Fact]
		void Test16_Test_Ew_Gw_1() {
			var decoder = CreateDecoder16("85 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Test_Ew_Gw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.SI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.CX, instr.Op1Register);
		}

		[Fact]
		void Test16_Test_Ew_Gw_2() {
			var decoder = CreateDecoder16("85 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Test_Ew_Gw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.BX, instr.Op1Register);
		}

		[Fact]
		void Test32_Test_Ew_Gw_1() {
			var decoder = CreateDecoder32("66 85 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Test_Ew_Gw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.SI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.CX, instr.Op1Register);
		}

		[Fact]
		void Test32_Test_Ew_Gw_2() {
			var decoder = CreateDecoder32("66 85 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Test_Ew_Gw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.BX, instr.Op1Register);
		}

		[Theory]
		[InlineData("66 85 CE", 3, Register.SI, Register.CX)]
		[InlineData("66 44 85 C5", 4, Register.BP, Register.R8W)]
		[InlineData("66 41 85 D6", 4, Register.R14W, Register.DX)]
		[InlineData("66 45 85 D0", 4, Register.R8W, Register.R10W)]
		[InlineData("66 41 85 D9", 4, Register.R9W, Register.BX)]
		[InlineData("66 44 85 EC", 4, Register.SP, Register.R13W)]
		void Test64_Test_Ew_Gw_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Test_Ew_Gw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}

		[Fact]
		void Test64_Test_Ew_Gw_2() {
			var decoder = CreateDecoder64("66 85 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Test_Ew_Gw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.BX, instr.Op1Register);
		}

		[Fact]
		void Test16_Test_Ed_Gd_1() {
			var decoder = CreateDecoder16("66 85 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Test_Ed_Gd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ESI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ECX, instr.Op1Register);
		}

		[Fact]
		void Test16_Test_Ed_Gd_2() {
			var decoder = CreateDecoder16("66 85 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Test_Ed_Gd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EBX, instr.Op1Register);
		}

		[Fact]
		void Test32_Test_Ed_Gd_1() {
			var decoder = CreateDecoder32("85 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Test_Ed_Gd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ESI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ECX, instr.Op1Register);
		}

		[Fact]
		void Test32_Test_Ed_Gd_2() {
			var decoder = CreateDecoder32("85 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Test_Ed_Gd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EBX, instr.Op1Register);
		}

		[Theory]
		[InlineData("85 CE", 2, Register.ESI, Register.ECX)]
		[InlineData("44 85 C5", 3, Register.EBP, Register.R8D)]
		[InlineData("41 85 D6", 3, Register.R14D, Register.EDX)]
		[InlineData("45 85 D0", 3, Register.R8D, Register.R10D)]
		[InlineData("41 85 D9", 3, Register.R9D, Register.EBX)]
		[InlineData("44 85 EC", 3, Register.ESP, Register.R13D)]
		void Test64_Test_Ed_Gd_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Test_Ed_Gd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}

		[Fact]
		void Test64_Test_Ed_Gd_2() {
			var decoder = CreateDecoder64("85 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Test_Ed_Gd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EBX, instr.Op1Register);
		}

		[Theory]
		[InlineData("48 85 CE", 3, Register.RSI, Register.RCX)]
		[InlineData("4C 85 C5", 3, Register.RBP, Register.R8)]
		[InlineData("49 85 D6", 3, Register.R14, Register.RDX)]
		[InlineData("4D 85 D0", 3, Register.R8, Register.R10)]
		[InlineData("49 85 D9", 3, Register.R9, Register.RBX)]
		[InlineData("4C 85 EC", 3, Register.RSP, Register.R13)]
		void Test64_Test_Eq_Gq_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Test_Eq_Gq, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}

		[Fact]
		void Test64_Test_Eq_Gq_2() {
			var decoder = CreateDecoder64("48 85 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Test_Eq_Gq, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt64, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.RBX, instr.Op1Register);
		}

		[Fact]
		void Test16_Xchg_Eb_Gb_1() {
			var decoder = CreateDecoder16("86 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Xchg_Eb_Gb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.DH, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.CL, instr.Op1Register);
		}

		[Fact]
		void Test16_Xchg_Eb_Gb_2() {
			var decoder = CreateDecoder16("86 38");
			var instr = decoder.Decode();

			Assert.Equal(Code.Xchg_Eb_Gb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.BH, instr.Op1Register);
		}

		[Fact]
		void Test32_Xchg_Eb_Gb_1() {
			var decoder = CreateDecoder32("86 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Xchg_Eb_Gb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.DH, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.CL, instr.Op1Register);
		}

		[Fact]
		void Test32_Xchg_Eb_Gb_2() {
			var decoder = CreateDecoder32("86 38");
			var instr = decoder.Decode();

			Assert.Equal(Code.Xchg_Eb_Gb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.BH, instr.Op1Register);
		}

		[Theory]
		[InlineData("86 CE", 2, Register.DH, Register.CL)]
		[InlineData("40 86 D5", 3, Register.BPL, Register.DL)]
		[InlineData("40 86 FA", 3, Register.DL, Register.DIL)]
		[InlineData("45 86 F0", 3, Register.R8L, Register.R14L)]
		[InlineData("41 86 D9", 3, Register.R9L, Register.BL)]
		[InlineData("44 86 EC", 3, Register.SPL, Register.R13L)]
		[InlineData("66 67 4E 86 EC", 5, Register.SPL, Register.R13L)]
		void Test64_Xchg_Eb_Gb_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Xchg_Eb_Gb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}

		[Fact]
		void Test64_Xchg_Eb_Gb_2() {
			var decoder = CreateDecoder64("86 38");
			var instr = decoder.Decode();

			Assert.Equal(Code.Xchg_Eb_Gb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.BH, instr.Op1Register);
		}

		[Fact]
		void Test16_Xchg_Ew_Gw_1() {
			var decoder = CreateDecoder16("87 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Xchg_Ew_Gw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.SI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.CX, instr.Op1Register);
		}

		[Fact]
		void Test16_Xchg_Ew_Gw_2() {
			var decoder = CreateDecoder16("87 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Xchg_Ew_Gw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.BX, instr.Op1Register);
		}

		[Fact]
		void Test32_Xchg_Ew_Gw_1() {
			var decoder = CreateDecoder32("66 87 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Xchg_Ew_Gw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.SI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.CX, instr.Op1Register);
		}

		[Fact]
		void Test32_Xchg_Ew_Gw_2() {
			var decoder = CreateDecoder32("66 87 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Xchg_Ew_Gw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.BX, instr.Op1Register);
		}

		[Theory]
		[InlineData("66 87 CE", 3, Register.SI, Register.CX)]
		[InlineData("66 44 87 C5", 4, Register.BP, Register.R8W)]
		[InlineData("66 41 87 D6", 4, Register.R14W, Register.DX)]
		[InlineData("66 45 87 D0", 4, Register.R8W, Register.R10W)]
		[InlineData("66 41 87 D9", 4, Register.R9W, Register.BX)]
		[InlineData("66 44 87 EC", 4, Register.SP, Register.R13W)]
		void Test64_Xchg_Ew_Gw_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Xchg_Ew_Gw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}

		[Fact]
		void Test64_Xchg_Ew_Gw_2() {
			var decoder = CreateDecoder64("66 87 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Xchg_Ew_Gw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.BX, instr.Op1Register);
		}

		[Fact]
		void Test16_Xchg_Ed_Gd_1() {
			var decoder = CreateDecoder16("66 87 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Xchg_Ed_Gd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ESI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ECX, instr.Op1Register);
		}

		[Fact]
		void Test16_Xchg_Ed_Gd_2() {
			var decoder = CreateDecoder16("66 87 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Xchg_Ed_Gd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EBX, instr.Op1Register);
		}

		[Fact]
		void Test32_Xchg_Ed_Gd_1() {
			var decoder = CreateDecoder32("87 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Xchg_Ed_Gd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ESI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ECX, instr.Op1Register);
		}

		[Fact]
		void Test32_Xchg_Ed_Gd_2() {
			var decoder = CreateDecoder32("87 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Xchg_Ed_Gd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EBX, instr.Op1Register);
		}

		[Theory]
		[InlineData("87 CE", 2, Register.ESI, Register.ECX)]
		[InlineData("44 87 C5", 3, Register.EBP, Register.R8D)]
		[InlineData("41 87 D6", 3, Register.R14D, Register.EDX)]
		[InlineData("45 87 D0", 3, Register.R8D, Register.R10D)]
		[InlineData("41 87 D9", 3, Register.R9D, Register.EBX)]
		[InlineData("44 87 EC", 3, Register.ESP, Register.R13D)]
		void Test64_Xchg_Ed_Gd_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Xchg_Ed_Gd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}

		[Fact]
		void Test64_Xchg_Ed_Gd_2() {
			var decoder = CreateDecoder64("87 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Xchg_Ed_Gd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EBX, instr.Op1Register);
		}

		[Theory]
		[InlineData("48 87 CE", 3, Register.RSI, Register.RCX)]
		[InlineData("4C 87 C5", 3, Register.RBP, Register.R8)]
		[InlineData("49 87 D6", 3, Register.R14, Register.RDX)]
		[InlineData("4D 87 D0", 3, Register.R8, Register.R10)]
		[InlineData("49 87 D9", 3, Register.R9, Register.RBX)]
		[InlineData("4C 87 EC", 3, Register.RSP, Register.R13)]
		void Test64_Xchg_Eq_Gq_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Xchg_Eq_Gq, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}

		[Fact]
		void Test64_Xchg_Eq_Gq_2() {
			var decoder = CreateDecoder64("48 87 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Xchg_Eq_Gq, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt64, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.RBX, instr.Op1Register);
		}
	}
}
