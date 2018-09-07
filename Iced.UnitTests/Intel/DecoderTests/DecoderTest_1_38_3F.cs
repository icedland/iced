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
	public sealed class DecoderTest_1_38_3F : DecoderTest {
		[Fact]
		void Test16_Cmp_Eb_Gb_1() {
			var decoder = CreateDecoder16("38 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Eb_Gb, instr.Code);
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
		void Test16_Cmp_Eb_Gb_2() {
			var decoder = CreateDecoder16("38 38");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Eb_Gb, instr.Code);
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
		void Test32_Cmp_Eb_Gb_1() {
			var decoder = CreateDecoder32("38 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Eb_Gb, instr.Code);
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
		void Test32_Cmp_Eb_Gb_2() {
			var decoder = CreateDecoder32("38 38");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Eb_Gb, instr.Code);
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
		[InlineData("38 CE", 2, Register.DH, Register.CL)]
		[InlineData("40 38 D5", 3, Register.BPL, Register.DL)]
		[InlineData("40 38 FA", 3, Register.DL, Register.DIL)]
		[InlineData("45 38 F0", 3, Register.R8L, Register.R14L)]
		[InlineData("41 38 D9", 3, Register.R9L, Register.BL)]
		[InlineData("44 38 EC", 3, Register.SPL, Register.R13L)]
		[InlineData("66 67 4E 38 EC", 5, Register.SPL, Register.R13L)]
		void Test64_Cmp_Eb_Gb_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Eb_Gb, instr.Code);
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
		void Test64_Cmp_Eb_Gb_2() {
			var decoder = CreateDecoder64("38 38");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Eb_Gb, instr.Code);
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
		void Test16_Cmp_Ew_Gw_1() {
			var decoder = CreateDecoder16("39 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Ew_Gw, instr.Code);
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
		void Test16_Cmp_Ew_Gw_2() {
			var decoder = CreateDecoder16("39 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Ew_Gw, instr.Code);
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
		void Test32_Cmp_Ew_Gw_1() {
			var decoder = CreateDecoder32("66 39 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Ew_Gw, instr.Code);
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
		void Test32_Cmp_Ew_Gw_2() {
			var decoder = CreateDecoder32("66 39 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Ew_Gw, instr.Code);
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
		[InlineData("66 39 CE", 3, Register.SI, Register.CX)]
		[InlineData("66 44 39 C5", 4, Register.BP, Register.R8W)]
		[InlineData("66 41 39 D6", 4, Register.R14W, Register.DX)]
		[InlineData("66 45 39 D0", 4, Register.R8W, Register.R10W)]
		[InlineData("66 41 39 D9", 4, Register.R9W, Register.BX)]
		[InlineData("66 44 39 EC", 4, Register.SP, Register.R13W)]
		void Test64_Cmp_Ew_Gw_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Ew_Gw, instr.Code);
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
		void Test64_Cmp_Ew_Gw_2() {
			var decoder = CreateDecoder64("66 39 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Ew_Gw, instr.Code);
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
		void Test16_Cmp_Ed_Gd_1() {
			var decoder = CreateDecoder16("66 39 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Ed_Gd, instr.Code);
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
		void Test16_Cmp_Ed_Gd_2() {
			var decoder = CreateDecoder16("66 39 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Ed_Gd, instr.Code);
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
		void Test32_Cmp_Ed_Gd_1() {
			var decoder = CreateDecoder32("39 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Ed_Gd, instr.Code);
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
		void Test32_Cmp_Ed_Gd_2() {
			var decoder = CreateDecoder32("39 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Ed_Gd, instr.Code);
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
		[InlineData("39 CE", 2, Register.ESI, Register.ECX)]
		[InlineData("44 39 C5", 3, Register.EBP, Register.R8D)]
		[InlineData("41 39 D6", 3, Register.R14D, Register.EDX)]
		[InlineData("45 39 D0", 3, Register.R8D, Register.R10D)]
		[InlineData("41 39 D9", 3, Register.R9D, Register.EBX)]
		[InlineData("44 39 EC", 3, Register.ESP, Register.R13D)]
		void Test64_Cmp_Ed_Gd_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Ed_Gd, instr.Code);
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
		void Test64_Cmp_Ed_Gd_2() {
			var decoder = CreateDecoder64("39 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Ed_Gd, instr.Code);
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
		[InlineData("48 39 CE", 3, Register.RSI, Register.RCX)]
		[InlineData("4C 39 C5", 3, Register.RBP, Register.R8)]
		[InlineData("49 39 D6", 3, Register.R14, Register.RDX)]
		[InlineData("4D 39 D0", 3, Register.R8, Register.R10)]
		[InlineData("49 39 D9", 3, Register.R9, Register.RBX)]
		[InlineData("4C 39 EC", 3, Register.RSP, Register.R13)]
		void Test64_Cmp_Eq_Gq_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Eq_Gq, instr.Code);
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
		void Test64_Cmp_Eq_Gq_2() {
			var decoder = CreateDecoder64("48 39 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Eq_Gq, instr.Code);
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
		void Test16_Cmp_Gb_Eb_1() {
			var decoder = CreateDecoder16("3A CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Gb_Eb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.CL, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.DH, instr.Op1Register);
		}

		[Fact]
		void Test16_Cmp_Gb_Eb_2() {
			var decoder = CreateDecoder16("3A 38");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Gb_Eb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.BH, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Fact]
		void Test32_Cmp_Gb_Eb_1() {
			var decoder = CreateDecoder32("3A CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Gb_Eb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.CL, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.DH, instr.Op1Register);
		}

		[Fact]
		void Test32_Cmp_Gb_Eb_2() {
			var decoder = CreateDecoder32("3A 38");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Gb_Eb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.BH, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("3A CE", 2, Register.CL, Register.DH)]
		[InlineData("40 3A D5", 3, Register.DL, Register.BPL)]
		[InlineData("40 3A FA", 3, Register.DIL, Register.DL)]
		[InlineData("45 3A F0", 3, Register.R14L, Register.R8L)]
		[InlineData("41 3A D9", 3, Register.BL, Register.R9L)]
		[InlineData("44 3A EC", 3, Register.R13L, Register.SPL)]
		[InlineData("66 67 4E 3A EC", 5, Register.R13L, Register.SPL)]
		void Test64_Cmp_Gb_Eb_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Gb_Eb, instr.Code);
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
		void Test64_Cmp_Gb_Eb_2() {
			var decoder = CreateDecoder64("3A 38");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Gb_Eb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.BH, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Fact]
		void Test16_Cmp_Gw_Ew_1() {
			var decoder = CreateDecoder16("3B CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Gw_Ew, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.CX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.SI, instr.Op1Register);
		}

		[Fact]
		void Test16_Cmp_Gw_Ew_2() {
			var decoder = CreateDecoder16("3B 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Gw_Ew, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.BX, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Fact]
		void Test32_Cmp_Gw_Ew_1() {
			var decoder = CreateDecoder32("66 3B CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Gw_Ew, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.CX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.SI, instr.Op1Register);
		}

		[Fact]
		void Test32_Cmp_Gw_Ew_2() {
			var decoder = CreateDecoder32("66 3B 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Gw_Ew, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.BX, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("66 3B CE", 3, Register.CX, Register.SI)]
		[InlineData("66 44 3B C5", 4, Register.R8W, Register.BP)]
		[InlineData("66 41 3B D6", 4, Register.DX, Register.R14W)]
		[InlineData("66 45 3B D0", 4, Register.R10W, Register.R8W)]
		[InlineData("66 41 3B D9", 4, Register.BX, Register.R9W)]
		[InlineData("66 44 3B EC", 4, Register.R13W, Register.SP)]
		void Test64_Cmp_Gw_Ew_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Gw_Ew, instr.Code);
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
		void Test64_Cmp_Gw_Ew_2() {
			var decoder = CreateDecoder64("66 3B 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Gw_Ew, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.BX, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Fact]
		void Test16_Cmp_Gd_Ed_1() {
			var decoder = CreateDecoder16("66 3B CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Gd_Ed, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ECX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ESI, instr.Op1Register);
		}

		[Fact]
		void Test16_Cmp_Gd_Ed_2() {
			var decoder = CreateDecoder16("66 3B 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Gd_Ed, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EBX, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Fact]
		void Test32_Cmp_Gd_Ed_1() {
			var decoder = CreateDecoder32("3B CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Gd_Ed, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ECX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ESI, instr.Op1Register);
		}

		[Fact]
		void Test32_Cmp_Gd_Ed_2() {
			var decoder = CreateDecoder32("3B 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Gd_Ed, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EBX, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("3B CE", 2, Register.ECX, Register.ESI)]
		[InlineData("44 3B C5", 3, Register.R8D, Register.EBP)]
		[InlineData("41 3B D6", 3, Register.EDX, Register.R14D)]
		[InlineData("45 3B D0", 3, Register.R10D, Register.R8D)]
		[InlineData("41 3B D9", 3, Register.EBX, Register.R9D)]
		[InlineData("44 3B EC", 3, Register.R13D, Register.ESP)]
		void Test64_Cmp_Gd_Ed_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Gd_Ed, instr.Code);
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
		void Test64_Cmp_Gd_Ed_2() {
			var decoder = CreateDecoder64("3B 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Gd_Ed, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EBX, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("48 3B CE", 3, Register.RCX, Register.RSI)]
		[InlineData("4C 3B C5", 3, Register.R8, Register.RBP)]
		[InlineData("49 3B D6", 3, Register.RDX, Register.R14)]
		[InlineData("4D 3B D0", 3, Register.R10, Register.R8)]
		[InlineData("49 3B D9", 3, Register.RBX, Register.R9)]
		[InlineData("4C 3B EC", 3, Register.R13, Register.RSP)]
		void Test64_Cmp_Gq_Eq_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Gq_Eq, instr.Code);
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
		void Test64_Cmp_Gq_Eq_2() {
			var decoder = CreateDecoder64("48 3B 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_Gq_Eq, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.RBX, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt64, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Fact]
		void Test16_Cmp_AL_Ib_1() {
			var decoder = CreateDecoder16("3C 5A");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_AL_Ib, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AL, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(0x5A, instr.Immediate8);
		}

		[Fact]
		void Test32_Cmp_AL_Ib_1() {
			var decoder = CreateDecoder32("3C A5");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_AL_Ib, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AL, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(0xA5, instr.Immediate8);
		}

		[Theory]
		[InlineData("3C A5", 2)]
		[InlineData("4F 3C A5", 3)]
		void Test64_Cmp_AL_Ib_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_AL_Ib, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AL, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(0xA5, instr.Immediate8);
		}

		[Fact]
		void Test16_Cmp_AX_Iw_1() {
			var decoder = CreateDecoder16("3D 5AA5");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_AX_Iw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AX, instr.Op0Register);

			Assert.Equal(OpKind.Immediate16, instr.Op1Kind);
			Assert.Equal(0xA55A, instr.Immediate16);
		}

		[Fact]
		void Test32_Cmp_AX_Iw_1() {
			var decoder = CreateDecoder32("66 3D A55A");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_AX_Iw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AX, instr.Op0Register);

			Assert.Equal(OpKind.Immediate16, instr.Op1Kind);
			Assert.Equal(0x5AA5, instr.Immediate16);
		}

		[Theory]
		[InlineData("66 3D A55A", 4)]
		[InlineData("66 47 3D A55A", 5)]
		void Test64_Cmp_AX_Iw_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_AX_Iw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AX, instr.Op0Register);

			Assert.Equal(OpKind.Immediate16, instr.Op1Kind);
			Assert.Equal(0x5AA5, instr.Immediate16);
		}

		[Fact]
		void Test16_Cmp_EAX_Id_1() {
			var decoder = CreateDecoder16("66 3D 5AA51234");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_EAX_Id, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(6, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EAX, instr.Op0Register);

			Assert.Equal(OpKind.Immediate32, instr.Op1Kind);
			Assert.Equal(0x3412A55AU, instr.Immediate32);
		}

		[Fact]
		void Test32_Cmp_EAX_Id_1() {
			var decoder = CreateDecoder32("3D A55A3412");
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_EAX_Id, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EAX, instr.Op0Register);

			Assert.Equal(OpKind.Immediate32, instr.Op1Kind);
			Assert.Equal(0x12345AA5U, instr.Immediate32);
		}

		[Theory]
		[InlineData("3D A55A3412", 5)]
		[InlineData("47 3D A55A3412", 6)]
		void Test64_Cmp_EAX_Id_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_EAX_Id, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EAX, instr.Op0Register);

			Assert.Equal(OpKind.Immediate32, instr.Op1Kind);
			Assert.Equal(0x12345AA5U, instr.Immediate32);
		}

		[Theory]
		[InlineData("48 3D A55A34A2", 6)]
		[InlineData("4F 3D A55A34A2", 6)]
		void Test64_Cmp_RAX_Id64_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmp_RAX_Id64, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.RAX, instr.Op0Register);

			Assert.Equal(OpKind.Immediate32to64, instr.Op1Kind);
			Assert.Equal(0xFFFFFFFFA2345AA5UL, (ulong)instr.Immediate32to64);
		}

		[Fact]
		void Test16_Aas_1() {
			var decoder = CreateDecoder16("3F");
			var instr = decoder.Decode();

			Assert.Equal(Code.Aas, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(1, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Fact]
		void Test32_Aas_1() {
			var decoder = CreateDecoder32("3F");
			var instr = decoder.Decode();

			Assert.Equal(Code.Aas, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(1, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}
	}
}
