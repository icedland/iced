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
	public sealed class DecoderTest_1_68_6F : DecoderTest {
		[Fact]
		void Test16_Push_Iw_1() {
			var decoder = CreateDecoder16("68 5AA5");
			var instr = decoder.Decode();

			Assert.Equal(Code.Push_Iw, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate16, instr.Op0Kind);
			Assert.Equal(0xA55A, instr.Immediate16);
		}

		[Fact]
		void Test32_Push_Iw_1() {
			var decoder = CreateDecoder32("66 68 5AA5");
			var instr = decoder.Decode();

			Assert.Equal(Code.Push_Iw, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate16, instr.Op0Kind);
			Assert.Equal(0xA55A, instr.Immediate16);
		}

		[Theory]
		[InlineData("66 68 5AA5", 4, 0xA55A)]
		[InlineData("66 47 68 5AA5", 5, 0xA55A)]
		void Test64_Push_Iw_1(string hexBytes, int byteLength, ushort imm16) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Push_Iw, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate16, instr.Op0Kind);
			Assert.Equal(imm16, instr.Immediate16);
		}

		[Fact]
		void Test16_Push_Id_1() {
			var decoder = CreateDecoder16("66 68 5AA51234");
			var instr = decoder.Decode();

			Assert.Equal(Code.Push_Id, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(6, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate32, instr.Op0Kind);
			Assert.Equal(0x3412A55AU, instr.Immediate32);
		}

		[Fact]
		void Test32_Push_Id_1() {
			var decoder = CreateDecoder32("68 5AA51234");
			var instr = decoder.Decode();

			Assert.Equal(Code.Push_Id, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate32, instr.Op0Kind);
			Assert.Equal(0x3412A55AU, instr.Immediate32);
		}

		[Theory]
		[InlineData("68 5AA512A4", 5, 0xFFFFFFFFA412A55AUL)]
		[InlineData("47 68 5AA51234", 6, 0x3412A55AUL)]
		[InlineData("66 4F 68 5AA51234", 7, 0x3412A55AUL)]
		void Test64_Push_Id64_1(string hexBytes, int byteLength, ulong imm64) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Push_Id64, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate32to64, instr.Op0Kind);
			Assert.Equal(imm64, (ulong)instr.Immediate32to64);
		}

		[Fact]
		void Test16_Imul_Gw_Ew_Iw_1() {
			var decoder = CreateDecoder16("69 CE 5AA5");
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gw_Ew_Iw, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.CX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.SI, instr.Op1Register);

			Assert.Equal(OpKind.Immediate16, instr.Op2Kind);
			Assert.Equal(0xA55A, instr.Immediate16);
		}

		[Fact]
		void Test16_Imul_Gw_Ew_Iw_2() {
			var decoder = CreateDecoder16("69 18 5AA5");
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gw_Ew_Iw, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.Int16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate16, instr.Op2Kind);
			Assert.Equal(0xA55A, instr.Immediate16);
		}

		[Fact]
		void Test32_Imul_Gw_Ew_Iw_1() {
			var decoder = CreateDecoder32("66 69 CE 5AA5");
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gw_Ew_Iw, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.CX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.SI, instr.Op1Register);

			Assert.Equal(OpKind.Immediate16, instr.Op2Kind);
			Assert.Equal(0xA55A, instr.Immediate16);
		}

		[Fact]
		void Test32_Imul_Gw_Ew_Iw_2() {
			var decoder = CreateDecoder32("66 69 18 5AA5");
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gw_Ew_Iw, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.Int16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate16, instr.Op2Kind);
			Assert.Equal(0xA55A, instr.Immediate16);
		}

		[Theory]
		[InlineData("66 69 CE A55A", 5, Register.CX, Register.SI, 0x5AA5)]
		[InlineData("66 44 69 C5 5AA5", 6, Register.R8W, Register.BP, 0xA55A)]
		[InlineData("66 41 69 D6 A55A", 6, Register.DX, Register.R14W, 0x5AA5)]
		[InlineData("66 45 69 D0 5AA5", 6, Register.R10W, Register.R8W, 0xA55A)]
		[InlineData("66 41 69 D9 A55A", 6, Register.BX, Register.R9W, 0x5AA5)]
		[InlineData("66 44 69 EC 5AA5", 6, Register.R13W, Register.SP, 0xA55A)]
		void Test64_Imul_Gw_Ew_Iw_1(string hexBytes, int byteLength, Register reg1, Register reg2, ushort immediate16) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gw_Ew_Iw, instr.Code);
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

			Assert.Equal(OpKind.Immediate16, instr.Op2Kind);
			Assert.Equal(immediate16, instr.Immediate16);
		}

		[Fact]
		void Test64_Imul_Gw_Ew_Iw_2() {
			var decoder = CreateDecoder64("66 69 18 5AA5");
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gw_Ew_Iw, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.Int16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate16, instr.Op2Kind);
			Assert.Equal(0xA55A, instr.Immediate16);
		}

		[Fact]
		void Test16_Imul_Gd_Ed_Id_1() {
			var decoder = CreateDecoder16("66 69 CE 5AA51234");
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gd_Ed_Id, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(7, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ECX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ESI, instr.Op1Register);

			Assert.Equal(OpKind.Immediate32, instr.Op2Kind);
			Assert.Equal(0x3412A55AU, instr.Immediate32);
		}

		[Fact]
		void Test16_Imul_Gd_Ed_Id_2() {
			var decoder = CreateDecoder16("66 69 18 5AA51234");
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gd_Ed_Id, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(7, instr.ByteLength);
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.Int32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate32, instr.Op2Kind);
			Assert.Equal(0x3412A55AU, instr.Immediate32);
		}

		[Fact]
		void Test32_Imul_Gd_Ed_Id_1() {
			var decoder = CreateDecoder32("69 CE 5AA51234");
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gd_Ed_Id, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(6, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ECX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ESI, instr.Op1Register);

			Assert.Equal(OpKind.Immediate32, instr.Op2Kind);
			Assert.Equal(0x3412A55AU, instr.Immediate32);
		}

		[Fact]
		void Test32_Imul_Gd_Ed_Id_2() {
			var decoder = CreateDecoder32("69 18 5AA51234");
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gd_Ed_Id, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(6, instr.ByteLength);
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.Int32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate32, instr.Op2Kind);
			Assert.Equal(0x3412A55AU, instr.Immediate32);
		}

		[Theory]
		[InlineData("69 CE 5AA51234", 6, Register.ECX, Register.ESI, 0x3412A55AU)]
		[InlineData("44 69 C5 5AA51234", 7, Register.R8D, Register.EBP, 0x3412A55AU)]
		[InlineData("41 69 D6 5AA51234", 7, Register.EDX, Register.R14D, 0x3412A55AU)]
		[InlineData("45 69 D0 5AA51234", 7, Register.R10D, Register.R8D, 0x3412A55AU)]
		[InlineData("41 69 D9 5AA51234", 7, Register.EBX, Register.R9D, 0x3412A55AU)]
		[InlineData("44 69 EC 5AA51234", 7, Register.R13D, Register.ESP, 0x3412A55AU)]
		void Test64_Imul_Gd_Ed_Id_1(string hexBytes, int byteLength, Register reg1, Register reg2, uint immediate32) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gd_Ed_Id, instr.Code);
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

			Assert.Equal(OpKind.Immediate32, instr.Op2Kind);
			Assert.Equal(immediate32, instr.Immediate32);
		}

		[Fact]
		void Test64_Imul_Gd_Ed_Id_2() {
			var decoder = CreateDecoder64("69 18 5AA51234");
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gd_Ed_Id, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(6, instr.ByteLength);
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.Int32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate32, instr.Op2Kind);
			Assert.Equal(0x3412A55AU, instr.Immediate32);
		}

		[Theory]
		[InlineData("48 69 CE 5AA512A4", 7, Register.RCX, Register.RSI, 0xFFFFFFFFA412A55AUL)]
		[InlineData("4C 69 C5 5AA51234", 7, Register.R8, Register.RBP, 0x3412A55AUL)]
		[InlineData("49 69 D6 5AA512A4", 7, Register.RDX, Register.R14, 0xFFFFFFFFA412A55AUL)]
		[InlineData("4D 69 D0 5AA51234", 7, Register.R10, Register.R8, 0x3412A55AUL)]
		[InlineData("49 69 D9 5AA512A4", 7, Register.RBX, Register.R9, 0xFFFFFFFFA412A55AUL)]
		[InlineData("4C 69 EC 5AA51234", 7, Register.R13, Register.RSP, 0x3412A55AUL)]
		void Test64_Imul_Gq_Eq_Id64_1(string hexBytes, int byteLength, Register reg1, Register reg2, ulong imm64) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gq_Eq_Id64, instr.Code);
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

			Assert.Equal(OpKind.Immediate32to64, instr.Op2Kind);
			Assert.Equal(imm64, (ulong)instr.Immediate32to64);
		}

		[Fact]
		void Test64_Imul_Gq_Eq_Id64_2() {
			var decoder = CreateDecoder64("48 69 18 5AA512A4");
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gq_Eq_Id64, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(7, instr.ByteLength);
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.Int64, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate32to64, instr.Op2Kind);
			Assert.Equal(0xFFFFFFFFA412A55AUL, (ulong)instr.Immediate32to64);
		}

		[Theory]
		[InlineData("6A 5A", 2, 0x005A)]
		[InlineData("6A A5", 2, 0xFFA5)]
		void Test16_Push_Ib16_1(string hexBytes, int byteLength, ushort imm16) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Push_Ib16, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate8to16, instr.Op0Kind);
			Assert.Equal((short)imm16, instr.Immediate8to16);
		}

		[Theory]
		[InlineData("66 6A 5A", 3, 0x005A)]
		[InlineData("66 6A A5", 3, 0xFFA5)]
		void Test32_Push_Ib16_1(string hexBytes, int byteLength, ushort imm16) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Push_Ib16, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate8to16, instr.Op0Kind);
			Assert.Equal((short)imm16, instr.Immediate8to16);
		}

		[Theory]
		[InlineData("66 6A 5A", 3, 0x005A)]
		[InlineData("66 6A A5", 3, 0xFFA5)]
		void Test64_Push_Ib16_1(string hexBytes, int byteLength, ushort imm16) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Push_Ib16, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate8to16, instr.Op0Kind);
			Assert.Equal((short)imm16, instr.Immediate8to16);
		}

		[Theory]
		[InlineData("66 6A 5A", 3, 0x0000005A)]
		[InlineData("66 6A A5", 3, 0xFFFFFFA5)]
		void Test16_Push_Ib32_1(string hexBytes, int byteLength, uint imm32) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Push_Ib32, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate8to32, instr.Op0Kind);
			Assert.Equal((int)imm32, instr.Immediate8to32);
		}

		[Theory]
		[InlineData("6A 5A", 2, 0x0000005A)]
		[InlineData("6A A5", 2, 0xFFFFFFA5)]
		void Test32_Push_Ib32_1(string hexBytes, int byteLength, uint imm32) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Push_Ib32, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate8to32, instr.Op0Kind);
			Assert.Equal((int)imm32, instr.Immediate8to32);
		}

		[Theory]
		[InlineData("6A 5A", 2, 0x0000005AUL)]
		[InlineData("6A A5", 2, 0xFFFFFFFFFFFFFFA5UL)]
		[InlineData("4F 6A 5A", 3, 0x0000005AUL)]
		[InlineData("66 4F 6A A5", 4, 0xFFFFFFFFFFFFFFA5UL)]
		void Test64_Push_Ib64_1(string hexBytes, int byteLength, ulong imm64) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Push_Ib64, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate8to64, instr.Op0Kind);
			Assert.Equal((long)imm64, instr.Immediate8to64);
		}

		[Theory]
		[InlineData("6B CE 5A", 3, 0x005A)]
		[InlineData("6B CE A5", 3, 0xFFA5)]
		void Test16_Imul_Gw_Ew_Ib16_1(string hexBytes, int byteLength, ushort imm16) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gw_Ew_Ib16, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.CX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.SI, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8to16, instr.Op2Kind);
			Assert.Equal((short)imm16, instr.Immediate8to16);
		}

		[Theory]
		[InlineData("6B 18 5A", 3, 0x005A)]
		[InlineData("6B 18 A5", 3, 0xFFA5)]
		void Test16_Imul_Gw_Ew_Ib16_2(string hexBytes, int byteLength, ushort imm16) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gw_Ew_Ib16, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.Int16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8to16, instr.Op2Kind);
			Assert.Equal((short)imm16, instr.Immediate8to16);
		}

		[Theory]
		[InlineData("66 6B CE 5A", 4, 0x005A)]
		[InlineData("66 6B CE A5", 4, 0xFFA5)]
		void Test32_Imul_Gw_Ew_Ib16_1(string hexBytes, int byteLength, ushort imm16) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gw_Ew_Ib16, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.CX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.SI, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8to16, instr.Op2Kind);
			Assert.Equal((short)imm16, instr.Immediate8to16);
		}

		[Theory]
		[InlineData("66 6B 18 5A", 4, 0x005A)]
		[InlineData("66 6B 18 A5", 4, 0xFFA5)]
		void Test32_Imul_Gw_Ew_Ib16_2(string hexBytes, int byteLength, ushort imm16) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gw_Ew_Ib16, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.Int16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8to16, instr.Op2Kind);
			Assert.Equal((short)imm16, instr.Immediate8to16);
		}

		[Theory]
		[InlineData("66 6B CE 5A", 4, Register.CX, Register.SI, 0x005A)]
		[InlineData("66 44 6B C5 A5", 5, Register.R8W, Register.BP, 0xFFA5)]
		[InlineData("66 41 6B D6 5A", 5, Register.DX, Register.R14W, 0x005A)]
		[InlineData("66 45 6B D0 A5", 5, Register.R10W, Register.R8W, 0xFFA5)]
		[InlineData("66 41 6B D9 5A", 5, Register.BX, Register.R9W, 0x005A)]
		[InlineData("66 44 6B EC A5", 5, Register.R13W, Register.SP, 0xFFA5)]
		void Test64_Imul_Gw_Ew_Ib16_1(string hexBytes, int byteLength, Register reg1, Register reg2, ushort immediate16) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gw_Ew_Ib16, instr.Code);
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

			Assert.Equal(OpKind.Immediate8to16, instr.Op2Kind);
			Assert.Equal((short)immediate16, instr.Immediate8to16);
		}

		[Theory]
		[InlineData("66 6B 18 5A", 4, 0x005A)]
		[InlineData("66 6B 18 A5", 4, 0xFFA5)]
		void Test64_Imul_Gw_Ew_Ib16_2(string hexBytes, int byteLength, ushort imm16) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gw_Ew_Ib16, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.Int16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8to16, instr.Op2Kind);
			Assert.Equal((short)imm16, instr.Immediate8to16);
		}

		[Theory]
		[InlineData("66 6B CE 5A", 4, 0x0000005A)]
		[InlineData("66 6B CE A5", 4, 0xFFFFFFA5)]
		void Test16_Imul_Gd_Ed_Ib32_1(string hexBytes, int byteLength, uint imm32) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gd_Ed_Ib32, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ECX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ESI, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8to32, instr.Op2Kind);
			Assert.Equal((int)imm32, instr.Immediate8to32);
		}

		[Theory]
		[InlineData("66 6B 18 5A", 4, 0x0000005A)]
		[InlineData("66 6B 18 A5", 4, 0xFFFFFFA5)]
		void Test16_Imul_Gd_Ed_Ib32_2(string hexBytes, int byteLength, uint imm32) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gd_Ed_Ib32, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.Int32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8to32, instr.Op2Kind);
			Assert.Equal((int)imm32, instr.Immediate8to32);
		}

		[Theory]
		[InlineData("6B CE 5A", 3, 0x0000005A)]
		[InlineData("6B CE A5", 3, 0xFFFFFFA5)]
		void Test32_Imul_Gd_Ed_Ib32_1(string hexBytes, int byteLength, uint imm32) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gd_Ed_Ib32, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ECX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ESI, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8to32, instr.Op2Kind);
			Assert.Equal((int)imm32, instr.Immediate8to32);
		}

		[Theory]
		[InlineData("6B 18 5A", 3, 0x0000005A)]
		[InlineData("6B 18 A5", 3, 0xFFFFFFA5)]
		void Test32_Imul_Gd_Ed_Ib32_2(string hexBytes, int byteLength, uint imm32) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gd_Ed_Ib32, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.Int32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8to32, instr.Op2Kind);
			Assert.Equal((int)imm32, instr.Immediate8to32);
		}


		[Theory]
		[InlineData("6B CE 5A", 3, Register.ECX, Register.ESI, 0x0000005A)]
		[InlineData("44 6B C5 A5", 4, Register.R8D, Register.EBP, 0xFFFFFFA5)]
		[InlineData("41 6B D6 5A", 4, Register.EDX, Register.R14D, 0x0000005A)]
		[InlineData("45 6B D0 A5", 4, Register.R10D, Register.R8D, 0xFFFFFFA5)]
		[InlineData("41 6B D9 5A", 4, Register.EBX, Register.R9D, 0x0000005A)]
		[InlineData("44 6B EC A5", 4, Register.R13D, Register.ESP, 0xFFFFFFA5)]
		void Test64_Imul_Gd_Ed_Ib32_1(string hexBytes, int byteLength, Register reg1, Register reg2, uint immediate32) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gd_Ed_Ib32, instr.Code);
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

			Assert.Equal(OpKind.Immediate8to32, instr.Op2Kind);
			Assert.Equal((int)immediate32, instr.Immediate8to32);
		}

		[Theory]
		[InlineData("6B 18 5A", 3, 0x0000005A)]
		[InlineData("6B 18 A5", 3, 0xFFFFFFA5)]
		void Test64_Imul_Gd_Ed_Ib32_2(string hexBytes, int byteLength, uint imm32) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gd_Ed_Ib32, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.Int32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8to32, instr.Op2Kind);
			Assert.Equal((int)imm32, instr.Immediate8to32);
		}

		[Theory]
		[InlineData("48 6B CE 5A", 4, Register.RCX, Register.RSI, 0x0000005AUL)]
		[InlineData("4C 6B C5 A5", 4, Register.R8, Register.RBP, 0xFFFFFFFFFFFFFFA5UL)]
		[InlineData("49 6B D6 5A", 4, Register.RDX, Register.R14, 0x0000005AUL)]
		[InlineData("4D 6B D0 A5", 4, Register.R10, Register.R8, 0xFFFFFFFFFFFFFFA5UL)]
		[InlineData("49 6B D9 5A", 4, Register.RBX, Register.R9, 0x0000005AUL)]
		[InlineData("4C 6B EC A5", 4, Register.R13, Register.RSP, 0xFFFFFFFFFFFFFFA5UL)]
		void Test64_Imul_Gq_Eq_Ib64_1(string hexBytes, int byteLength, Register reg1, Register reg2, ulong immediate64) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gq_Eq_Ib64, instr.Code);
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

			Assert.Equal(OpKind.Immediate8to64, instr.Op2Kind);
			Assert.Equal((long)immediate64, instr.Immediate8to64);
		}

		[Theory]
		[InlineData("48 6B 18 5A", 4, 0x0000005AUL)]
		[InlineData("48 6B 18 A5", 4, 0xFFFFFFFFFFFFFFA5UL)]
		void Test64_Imul_Gq_Eq_Ib64_2(string hexBytes, int byteLength, ulong imm64) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gq_Eq_Ib64, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.Int64, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8to64, instr.Op2Kind);
			Assert.Equal((long)imm64, instr.Immediate8to64);
		}

		[Theory]
		[InlineData("6C", 1)]
		[InlineData("66 6C", 2)]
		void Test16_Insb_Yb_DX_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Insb_Yb_DX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESDI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.DX, instr.Op1Register);
		}

		[Theory]
		[InlineData("67 6C", 2)]
		[InlineData("66 67 6C", 3)]
		void Test16_Insb_Yb_DX_2(string hexBytes, int byteLength) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Insb_Yb_DX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.DX, instr.Op1Register);
		}

		[Theory]
		[InlineData("6C", 1)]
		[InlineData("66 6C", 2)]
		void Test32_Insb_Yb_DX_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Insb_Yb_DX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.DX, instr.Op1Register);
		}

		[Theory]
		[InlineData("67 6C", 2)]
		[InlineData("66 67 6C", 3)]
		void Test32_Insb_Yb_DX_2(string hexBytes, int byteLength) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Insb_Yb_DX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESDI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.DX, instr.Op1Register);
		}

		[Theory]
		[InlineData("6C", 1)]
		[InlineData("66 6C", 2)]
		[InlineData("4F 6C", 2)]
		void Test64_Insb_Yb_DX_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Insb_Yb_DX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESRDI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.DX, instr.Op1Register);
		}

		[Theory]
		[InlineData("67 6C", 2)]
		[InlineData("66 67 6C", 3)]
		[InlineData("67 4F 6C", 3)]
		void Test64_Insb_Yb_DX_2(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Insb_Yb_DX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.DX, instr.Op1Register);
		}

		[Theory]
		[InlineData("6D", 1)]
		void Test16_Insw_Yw_DX_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Insw_Yw_DX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESDI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.DX, instr.Op1Register);
		}

		[Theory]
		[InlineData("67 6D", 2)]
		void Test16_Insw_Yw_DX_2(string hexBytes, int byteLength) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Insw_Yw_DX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.DX, instr.Op1Register);
		}

		[Theory]
		[InlineData("66 6D", 2)]
		void Test32_Insw_Yw_DX_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Insw_Yw_DX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.DX, instr.Op1Register);
		}

		[Theory]
		[InlineData("66 67 6D", 3)]
		void Test32_Insw_Yw_DX_2(string hexBytes, int byteLength) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Insw_Yw_DX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESDI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.DX, instr.Op1Register);
		}

		[Theory]
		[InlineData("66 6D", 2)]
		[InlineData("66 47 6D", 3)]
		void Test64_Insw_Yw_DX_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Insw_Yw_DX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESRDI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.DX, instr.Op1Register);
		}

		[Theory]
		[InlineData("66 67 6D", 3)]
		[InlineData("66 67 47 6D", 4)]
		void Test64_Insw_Yw_DX_2(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Insw_Yw_DX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.DX, instr.Op1Register);
		}

		[Theory]
		[InlineData("66 6D", 2)]
		void Test16_Insd_Yd_DX_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Insd_Yd_DX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESDI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.DX, instr.Op1Register);
		}

		[Theory]
		[InlineData("66 67 6D", 3)]
		void Test16_Insd_Yd_DX_2(string hexBytes, int byteLength) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Insd_Yd_DX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.DX, instr.Op1Register);
		}

		[Theory]
		[InlineData("6D", 1)]
		void Test32_Insd_Yd_DX_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Insd_Yd_DX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.DX, instr.Op1Register);
		}

		[Theory]
		[InlineData("67 6D", 2)]
		void Test32_Insd_Yd_DX_2(string hexBytes, int byteLength) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Insd_Yd_DX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESDI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.DX, instr.Op1Register);
		}

		[Theory]
		[InlineData("6D", 1)]
		[InlineData("47 6D", 2)]
		[InlineData("4F 6D", 2)]
		[InlineData("66 4F 6D", 3)]
		void Test64_Insd_Yd_DX_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Insd_Yd_DX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESRDI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.DX, instr.Op1Register);
		}

		[Theory]
		[InlineData("67 6D", 2)]
		[InlineData("67 47 6D", 3)]
		[InlineData("67 4F 6D", 3)]
		[InlineData("66 67 4F 6D", 4)]
		void Test64_Insd_Yd_DX_2(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Insd_Yd_DX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.DX, instr.Op1Register);
		}

		[Theory]
		[InlineData("6E", 1)]
		[InlineData("66 6E", 2)]
		void Test16_Outsb_DX_Xb_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Outsb_DX_Xb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.DX, instr.Op0Register);

			Assert.Equal(OpKind.MemorySegSI, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
		}

		[Theory]
		[InlineData("67 6E", 2)]
		[InlineData("66 67 6E", 3)]
		void Test16_Outsb_DX_Xb_2(string hexBytes, int byteLength) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Outsb_DX_Xb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.DX, instr.Op0Register);

			Assert.Equal(OpKind.MemorySegESI, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
		}

		[Theory]
		[InlineData("6E", 1)]
		[InlineData("66 6E", 2)]
		void Test32_Outsb_DX_Xb_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Outsb_DX_Xb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.DX, instr.Op0Register);

			Assert.Equal(OpKind.MemorySegESI, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
		}

		[Theory]
		[InlineData("67 6E", 2)]
		[InlineData("66 67 6E", 3)]
		void Test32_Outsb_DX_Xb_2(string hexBytes, int byteLength) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Outsb_DX_Xb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.DX, instr.Op0Register);

			Assert.Equal(OpKind.MemorySegSI, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
		}

		[Theory]
		[InlineData("6E", 1)]
		[InlineData("66 6E", 2)]
		[InlineData("4F 6E", 2)]
		void Test64_Outsb_DX_Xb_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Outsb_DX_Xb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.DX, instr.Op0Register);

			Assert.Equal(OpKind.MemorySegRSI, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
		}

		[Theory]
		[InlineData("67 6E", 2)]
		[InlineData("66 67 6E", 3)]
		[InlineData("67 4F 6E", 3)]
		void Test64_Outsb_DX_Xb_2(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Outsb_DX_Xb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.DX, instr.Op0Register);

			Assert.Equal(OpKind.MemorySegESI, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
		}

		[Theory]
		[InlineData("6F", 1)]
		void Test16_Outsw_DX_Xw_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Outsw_DX_Xw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.DX, instr.Op0Register);

			Assert.Equal(OpKind.MemorySegSI, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
		}

		[Theory]
		[InlineData("67 6F", 2)]
		void Test16_Outsw_DX_Xw_2(string hexBytes, int byteLength) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Outsw_DX_Xw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.DX, instr.Op0Register);

			Assert.Equal(OpKind.MemorySegESI, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
		}

		[Theory]
		[InlineData("66 6F", 2)]
		void Test32_Outsw_DX_Xw_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Outsw_DX_Xw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.DX, instr.Op0Register);

			Assert.Equal(OpKind.MemorySegESI, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
		}

		[Theory]
		[InlineData("66 67 6F", 3)]
		void Test32_Outsw_DX_Xw_2(string hexBytes, int byteLength) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Outsw_DX_Xw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.DX, instr.Op0Register);

			Assert.Equal(OpKind.MemorySegSI, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
		}

		[Theory]
		[InlineData("66 6F", 2)]
		[InlineData("66 47 6F", 3)]
		void Test64_Outsw_DX_Xw_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Outsw_DX_Xw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.DX, instr.Op0Register);

			Assert.Equal(OpKind.MemorySegRSI, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
		}

		[Theory]
		[InlineData("66 67 6F", 3)]
		[InlineData("66 67 47 6F", 4)]
		void Test64_Outsw_DX_Xw_2(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Outsw_DX_Xw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.DX, instr.Op0Register);

			Assert.Equal(OpKind.MemorySegESI, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
		}

		[Theory]
		[InlineData("66 6F", 2)]
		void Test16_Outsd_DX_Xd_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Outsd_DX_Xd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.DX, instr.Op0Register);

			Assert.Equal(OpKind.MemorySegSI, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
		}

		[Theory]
		[InlineData("66 67 6F", 3)]
		void Test16_Outsd_DX_Xd_2(string hexBytes, int byteLength) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Outsd_DX_Xd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.DX, instr.Op0Register);

			Assert.Equal(OpKind.MemorySegESI, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
		}

		[Theory]
		[InlineData("6F", 1)]
		void Test32_Outsd_DX_Xd_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Outsd_DX_Xd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.DX, instr.Op0Register);

			Assert.Equal(OpKind.MemorySegESI, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
		}

		[Theory]
		[InlineData("67 6F", 2)]
		void Test32_Outsd_DX_Xd_2(string hexBytes, int byteLength) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Outsd_DX_Xd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.DX, instr.Op0Register);

			Assert.Equal(OpKind.MemorySegSI, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
		}

		[Theory]
		[InlineData("6F", 1)]
		[InlineData("47 6F", 2)]
		[InlineData("4F 6F", 2)]
		[InlineData("66 4F 6F", 3)]
		void Test64_Outsd_DX_Xd_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Outsd_DX_Xd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.DX, instr.Op0Register);

			Assert.Equal(OpKind.MemorySegRSI, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
		}

		[Theory]
		[InlineData("67 6F", 2)]
		[InlineData("67 47 6F", 3)]
		[InlineData("67 4F 6F", 3)]
		[InlineData("66 67 4F 6F", 4)]
		void Test64_Outsd_DX_Xd_2(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Outsd_DX_Xd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.DX, instr.Op0Register);

			Assert.Equal(OpKind.MemorySegESI, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
		}
	}
}
