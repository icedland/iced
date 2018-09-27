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
	public sealed class DecoderTest_2_0FA0_0FA7 : DecoderTest {
		[Fact]
		void Test16_Pushw_FS_1() {
			var decoder = CreateDecoder16("0FA0");
			var instr = decoder.Decode();

			Assert.Equal(Code.Pushw_FS, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.FS, instr.Op0Register);
		}

		[Fact]
		void Test32_Pushw_FS_1() {
			var decoder = CreateDecoder32("66 0FA0");
			var instr = decoder.Decode();

			Assert.Equal(Code.Pushw_FS, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.FS, instr.Op0Register);
		}

		[Fact]
		void Test64_Pushw_FS_1() {
			var decoder = CreateDecoder64("66 0FA0");
			var instr = decoder.Decode();

			Assert.Equal(Code.Pushw_FS, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.FS, instr.Op0Register);
		}

		[Fact]
		void Test16_Pushd_FS_1() {
			var decoder = CreateDecoder16("66 0FA0");
			var instr = decoder.Decode();

			Assert.Equal(Code.Pushd_FS, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.FS, instr.Op0Register);
		}

		[Fact]
		void Test32_Pushd_FS_1() {
			var decoder = CreateDecoder32("0FA0");
			var instr = decoder.Decode();

			Assert.Equal(Code.Pushd_FS, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.FS, instr.Op0Register);
		}

		[Fact]
		void Test64_Pushq_FS_1() {
			var decoder = CreateDecoder64("0FA0");
			var instr = decoder.Decode();

			Assert.Equal(Code.Pushq_FS, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.FS, instr.Op0Register);
		}

		[Fact]
		void Test16_Popw_FS_1() {
			var decoder = CreateDecoder16("0FA1");
			var instr = decoder.Decode();

			Assert.Equal(Code.Popw_FS, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.FS, instr.Op0Register);
		}

		[Fact]
		void Test32_Popw_FS_1() {
			var decoder = CreateDecoder32("66 0FA1");
			var instr = decoder.Decode();

			Assert.Equal(Code.Popw_FS, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.FS, instr.Op0Register);
		}

		[Fact]
		void Test64_Popw_FS_1() {
			var decoder = CreateDecoder64("66 0FA1");
			var instr = decoder.Decode();

			Assert.Equal(Code.Popw_FS, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.FS, instr.Op0Register);
		}

		[Fact]
		void Test16_Popd_FS_1() {
			var decoder = CreateDecoder16("66 0FA1");
			var instr = decoder.Decode();

			Assert.Equal(Code.Popd_FS, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.FS, instr.Op0Register);
		}

		[Fact]
		void Test32_Popd_FS_1() {
			var decoder = CreateDecoder32("0FA1");
			var instr = decoder.Decode();

			Assert.Equal(Code.Popd_FS, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.FS, instr.Op0Register);
		}

		[Fact]
		void Test64_Popq_FS_1() {
			var decoder = CreateDecoder64("0FA1");
			var instr = decoder.Decode();

			Assert.Equal(Code.Popq_FS, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.FS, instr.Op0Register);
		}

		[Theory]
		[InlineData("0FA2", 2, Code.Cpuid)]
		void Test16_Simple_1(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Theory]
		[InlineData("0FA2", 2, Code.Cpuid)]
		void Test32_Simple_1(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Theory]
		[InlineData("0FA2", 2, Code.Cpuid)]
		void Test64_Simple_1(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Fact]
		void Test16_Bt_rm16_r16_1() {
			var decoder = CreateDecoder16("0FA3 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Bt_rm16_r16, instr.Code);
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
		void Test16_Bt_rm16_r16_2() {
			var decoder = CreateDecoder16("0FA3 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Bt_rm16_r16, instr.Code);
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
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.BX, instr.Op1Register);
		}

		[Fact]
		void Test32_Bt_rm16_r16_1() {
			var decoder = CreateDecoder32("66 0FA3 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Bt_rm16_r16, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
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
		void Test32_Bt_rm16_r16_2() {
			var decoder = CreateDecoder32("66 0FA3 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Bt_rm16_r16, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
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
		[InlineData("66 0FA3 CE", 4, Register.SI, Register.CX)]
		[InlineData("66 44 0FA3 C5", 5, Register.BP, Register.R8W)]
		[InlineData("66 41 0FA3 D6", 5, Register.R14W, Register.DX)]
		[InlineData("66 45 0FA3 D0", 5, Register.R8W, Register.R10W)]
		[InlineData("66 41 0FA3 D9", 5, Register.R9W, Register.BX)]
		[InlineData("66 44 0FA3 EC", 5, Register.SP, Register.R13W)]
		void Test64_Bt_rm16_r16_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Bt_rm16_r16, instr.Code);
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
		void Test64_Bt_rm16_r16_2() {
			var decoder = CreateDecoder64("66 0FA3 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Bt_rm16_r16, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
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
		void Test16_Bt_rm32_r32_1() {
			var decoder = CreateDecoder16("66 0FA3 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Bt_rm32_r32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
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
		void Test16_Bt_rm32_r32_2() {
			var decoder = CreateDecoder16("66 0FA3 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Bt_rm32_r32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
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
		void Test32_Bt_rm32_r32_1() {
			var decoder = CreateDecoder32("0FA3 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Bt_rm32_r32, instr.Code);
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
		void Test32_Bt_rm32_r32_2() {
			var decoder = CreateDecoder32("0FA3 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Bt_rm32_r32, instr.Code);
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
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EBX, instr.Op1Register);
		}

		[Theory]
		[InlineData("0FA3 CE", 3, Register.ESI, Register.ECX)]
		[InlineData("44 0FA3 C5", 4, Register.EBP, Register.R8D)]
		[InlineData("41 0FA3 D6", 4, Register.R14D, Register.EDX)]
		[InlineData("45 0FA3 D0", 4, Register.R8D, Register.R10D)]
		[InlineData("41 0FA3 D9", 4, Register.R9D, Register.EBX)]
		[InlineData("44 0FA3 EC", 4, Register.ESP, Register.R13D)]
		void Test64_Bt_rm32_r32_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Bt_rm32_r32, instr.Code);
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
		void Test64_Bt_rm32_r32_2() {
			var decoder = CreateDecoder64("0FA3 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Bt_rm32_r32, instr.Code);
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
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EBX, instr.Op1Register);
		}

		[Theory]
		[InlineData("48 0FA3 CE", 4, Register.RSI, Register.RCX)]
		[InlineData("4C 0FA3 C5", 4, Register.RBP, Register.R8)]
		[InlineData("49 0FA3 D6", 4, Register.R14, Register.RDX)]
		[InlineData("4D 0FA3 D0", 4, Register.R8, Register.R10)]
		[InlineData("49 0FA3 D9", 4, Register.R9, Register.RBX)]
		[InlineData("4C 0FA3 EC", 4, Register.RSP, Register.R13)]
		void Test64_Bt_rm64_r64_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Bt_rm64_r64, instr.Code);
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
		void Test64_Bt_rm64_r64_2() {
			var decoder = CreateDecoder64("48 0FA3 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Bt_rm64_r64, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
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
		void Test16_Shld_rm16_r16_imm8_1() {
			var decoder = CreateDecoder16("0FA4 CE 5A");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shld_rm16_r16_imm8, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.SI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.CX, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(0x5A, instr.Immediate8);
		}

		[Fact]
		void Test16_Shld_rm16_r16_imm8_2() {
			var decoder = CreateDecoder16("0FA4 18 5A");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shld_rm16_r16_imm8, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
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

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(0x5A, instr.Immediate8);
		}

		[Fact]
		void Test32_Shld_rm16_r16_imm8_1() {
			var decoder = CreateDecoder32("66 0FA4 CE 5A");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shld_rm16_r16_imm8, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.SI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.CX, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(0x5A, instr.Immediate8);
		}

		[Fact]
		void Test32_Shld_rm16_r16_imm8_2() {
			var decoder = CreateDecoder32("66 0FA4 18 5A");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shld_rm16_r16_imm8, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
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

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(0x5A, instr.Immediate8);
		}

		[Theory]
		[InlineData("66 0FA4 CE 5A", 5, Register.SI, Register.CX, 0x5A)]
		[InlineData("66 44 0FA4 C5 5A", 6, Register.BP, Register.R8W, 0x5A)]
		[InlineData("66 41 0FA4 D6 5A", 6, Register.R14W, Register.DX, 0x5A)]
		[InlineData("66 45 0FA4 D0 5A", 6, Register.R8W, Register.R10W, 0x5A)]
		[InlineData("66 41 0FA4 D9 5A", 6, Register.R9W, Register.BX, 0x5A)]
		[InlineData("66 44 0FA4 EC 5A", 6, Register.SP, Register.R13W, 0x5A)]
		void Test64_Shld_rm16_r16_imm8_1(string hexBytes, int byteLength, Register reg1, Register reg2, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Shld_rm16_r16_imm8, instr.Code);
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

		[Fact]
		void Test64_Shld_rm16_r16_imm8_2() {
			var decoder = CreateDecoder64("66 0FA4 18 5A");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shld_rm16_r16_imm8, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
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

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(0x5A, instr.Immediate8);
		}

		[Fact]
		void Test16_Shld_rm32_r32_imm8_1() {
			var decoder = CreateDecoder16("66 0FA4 CE 5A");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shld_rm32_r32_imm8, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ESI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ECX, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(0x5A, instr.Immediate8);
		}

		[Fact]
		void Test16_Shld_rm32_r32_imm8_2() {
			var decoder = CreateDecoder16("66 0FA4 18 5A");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shld_rm32_r32_imm8, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
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

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(0x5A, instr.Immediate8);
		}

		[Fact]
		void Test32_Shld_rm32_r32_imm8_1() {
			var decoder = CreateDecoder32("0FA4 CE 5A");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shld_rm32_r32_imm8, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ESI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ECX, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(0x5A, instr.Immediate8);
		}

		[Fact]
		void Test32_Shld_rm32_r32_imm8_2() {
			var decoder = CreateDecoder32("0FA4 18 5A");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shld_rm32_r32_imm8, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
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

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(0x5A, instr.Immediate8);
		}

		[Theory]
		[InlineData("0FA4 CE 5A", 4, Register.ESI, Register.ECX, 0x5A)]
		[InlineData("44 0FA4 C5 5A", 5, Register.EBP, Register.R8D, 0x5A)]
		[InlineData("41 0FA4 D6 5A", 5, Register.R14D, Register.EDX, 0x5A)]
		[InlineData("45 0FA4 D0 5A", 5, Register.R8D, Register.R10D, 0x5A)]
		[InlineData("41 0FA4 D9 5A", 5, Register.R9D, Register.EBX, 0x5A)]
		[InlineData("44 0FA4 EC 5A", 5, Register.ESP, Register.R13D, 0x5A)]
		void Test64_Shld_rm32_r32_imm8_1(string hexBytes, int byteLength, Register reg1, Register reg2, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Shld_rm32_r32_imm8, instr.Code);
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

		[Fact]
		void Test64_Shld_rm32_r32_imm8_2() {
			var decoder = CreateDecoder64("0FA4 18 5A");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shld_rm32_r32_imm8, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
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

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(0x5A, instr.Immediate8);
		}

		[Theory]
		[InlineData("48 0FA4 CE 5A", 5, Register.RSI, Register.RCX, 0x5A)]
		[InlineData("4C 0FA4 C5 5A", 5, Register.RBP, Register.R8, 0x5A)]
		[InlineData("49 0FA4 D6 5A", 5, Register.R14, Register.RDX, 0x5A)]
		[InlineData("4D 0FA4 D0 5A", 5, Register.R8, Register.R10, 0x5A)]
		[InlineData("49 0FA4 D9 5A", 5, Register.R9, Register.RBX, 0x5A)]
		[InlineData("4C 0FA4 EC 5A", 5, Register.RSP, Register.R13, 0x5A)]
		void Test64_Shld_rm64_r64_imm8_1(string hexBytes, int byteLength, Register reg1, Register reg2, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Shld_rm64_r64_imm8, instr.Code);
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

		[Fact]
		void Test64_Shld_rm64_r64_imm8_2() {
			var decoder = CreateDecoder64("48 0FA4 18 5A");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shld_rm64_r64_imm8, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
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

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(0x5A, instr.Immediate8);
		}

		[Fact]
		void Test16_Shld_rm16_r16_CL_1() {
			var decoder = CreateDecoder16("0FA5 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shld_rm16_r16_CL, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.SI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.CX, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(Register.CL, instr.Op2Register);
		}

		[Fact]
		void Test16_Shld_rm16_r16_CL_2() {
			var decoder = CreateDecoder16("0FA5 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shld_rm16_r16_CL, instr.Code);
			Assert.Equal(3, instr.OpCount);
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
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.BX, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(Register.CL, instr.Op2Register);
		}

		[Fact]
		void Test32_Shld_rm16_r16_CL_1() {
			var decoder = CreateDecoder32("66 0FA5 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shld_rm16_r16_CL, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.SI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.CX, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(Register.CL, instr.Op2Register);
		}

		[Fact]
		void Test32_Shld_rm16_r16_CL_2() {
			var decoder = CreateDecoder32("66 0FA5 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shld_rm16_r16_CL, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(Register.CL, instr.Op2Register);
		}

		[Theory]
		[InlineData("66 0FA5 CE", 4, Register.SI, Register.CX)]
		[InlineData("66 44 0FA5 C5", 5, Register.BP, Register.R8W)]
		[InlineData("66 41 0FA5 D6", 5, Register.R14W, Register.DX)]
		[InlineData("66 45 0FA5 D0", 5, Register.R8W, Register.R10W)]
		[InlineData("66 41 0FA5 D9", 5, Register.R9W, Register.BX)]
		[InlineData("66 44 0FA5 EC", 5, Register.SP, Register.R13W)]
		void Test64_Shld_rm16_r16_CL_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Shld_rm16_r16_CL, instr.Code);
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(Register.CL, instr.Op2Register);
		}

		[Fact]
		void Test64_Shld_rm16_r16_CL_2() {
			var decoder = CreateDecoder64("66 0FA5 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shld_rm16_r16_CL, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(Register.CL, instr.Op2Register);
		}

		[Fact]
		void Test16_Shld_rm32_r32_CL_1() {
			var decoder = CreateDecoder16("66 0FA5 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shld_rm32_r32_CL, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ESI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ECX, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(Register.CL, instr.Op2Register);
		}

		[Fact]
		void Test16_Shld_rm32_r32_CL_2() {
			var decoder = CreateDecoder16("66 0FA5 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shld_rm32_r32_CL, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(Register.CL, instr.Op2Register);
		}

		[Fact]
		void Test32_Shld_rm32_r32_CL_1() {
			var decoder = CreateDecoder32("0FA5 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shld_rm32_r32_CL, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ESI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ECX, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(Register.CL, instr.Op2Register);
		}

		[Fact]
		void Test32_Shld_rm32_r32_CL_2() {
			var decoder = CreateDecoder32("0FA5 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shld_rm32_r32_CL, instr.Code);
			Assert.Equal(3, instr.OpCount);
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
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EBX, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(Register.CL, instr.Op2Register);
		}

		[Theory]
		[InlineData("0FA5 CE", 3, Register.ESI, Register.ECX)]
		[InlineData("44 0FA5 C5", 4, Register.EBP, Register.R8D)]
		[InlineData("41 0FA5 D6", 4, Register.R14D, Register.EDX)]
		[InlineData("45 0FA5 D0", 4, Register.R8D, Register.R10D)]
		[InlineData("41 0FA5 D9", 4, Register.R9D, Register.EBX)]
		[InlineData("44 0FA5 EC", 4, Register.ESP, Register.R13D)]
		void Test64_Shld_rm32_r32_CL_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Shld_rm32_r32_CL, instr.Code);
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(Register.CL, instr.Op2Register);
		}

		[Fact]
		void Test64_Shld_rm32_r32_CL_2() {
			var decoder = CreateDecoder64("0FA5 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shld_rm32_r32_CL, instr.Code);
			Assert.Equal(3, instr.OpCount);
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
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EBX, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(Register.CL, instr.Op2Register);
		}

		[Theory]
		[InlineData("48 0FA5 CE", 4, Register.RSI, Register.RCX)]
		[InlineData("4C 0FA5 C5", 4, Register.RBP, Register.R8)]
		[InlineData("49 0FA5 D6", 4, Register.R14, Register.RDX)]
		[InlineData("4D 0FA5 D0", 4, Register.R8, Register.R10)]
		[InlineData("49 0FA5 D9", 4, Register.R9, Register.RBX)]
		[InlineData("4C 0FA5 EC", 4, Register.RSP, Register.R13)]
		void Test64_Shld_rm64_r64_CL_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Shld_rm64_r64_CL, instr.Code);
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(Register.CL, instr.Op2Register);
		}

		[Fact]
		void Test64_Shld_rm64_r64_CL_2() {
			var decoder = CreateDecoder64("48 0FA5 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shld_rm64_r64_CL, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(Register.CL, instr.Op2Register);
		}

		[Theory]
		[MemberData(nameof(Test16_Xbts_G_E_1_Data))]
		void Test16_Xbts_G_E_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, DecoderOptions options) {
			var decoder = CreateDecoder16(hexBytes, options);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
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
		public static IEnumerable<object[]> Test16_Xbts_G_E_1_Data {
			get {
				yield return new object[] { "0FA6 CE", 3, Code.Xbts_r16_rm16, Register.CX, Register.SI, DecoderOptions.Xbts };
				yield return new object[] { "66 0FA6 CE", 4, Code.Xbts_r32_rm32, Register.ECX, Register.ESI, DecoderOptions.Xbts };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Xbts_G_E_2_Data))]
		void Test16_Xbts_G_E_2(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, DecoderOptions options) {
			var decoder = CreateDecoder16(hexBytes, options);
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

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_Xbts_G_E_2_Data {
			get {
				yield return new object[] { "0FA6 18", 3, Code.Xbts_r16_rm16, Register.BX, MemorySize.UInt16, DecoderOptions.Xbts };
				yield return new object[] { "66 0FA6 18", 4, Code.Xbts_r32_rm32, Register.EBX, MemorySize.UInt32, DecoderOptions.Xbts };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Xbts_G_E_1_Data))]
		void Test32_Xbts_G_E_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, DecoderOptions options) {
			var decoder = CreateDecoder32(hexBytes, options);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
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
		public static IEnumerable<object[]> Test32_Xbts_G_E_1_Data {
			get {
				yield return new object[] { "66 0FA6 CE", 4, Code.Xbts_r16_rm16, Register.CX, Register.SI, DecoderOptions.Xbts };
				yield return new object[] { "0FA6 CE", 3, Code.Xbts_r32_rm32, Register.ECX, Register.ESI, DecoderOptions.Xbts };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Xbts_G_E_2_Data))]
		void Test32_Xbts_G_E_2(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, DecoderOptions options) {
			var decoder = CreateDecoder32(hexBytes, options);
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

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_Xbts_G_E_2_Data {
			get {
				yield return new object[] { "66 0FA6 18", 4, Code.Xbts_r16_rm16, Register.BX, MemorySize.UInt16, DecoderOptions.Xbts };
				yield return new object[] { "0FA6 18", 3, Code.Xbts_r32_rm32, Register.EBX, MemorySize.UInt32, DecoderOptions.Xbts };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Ibts_E_G_1_Data))]
		void Test16_Ibts_E_G_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, DecoderOptions options) {
			var decoder = CreateDecoder16(hexBytes, options);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
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
		public static IEnumerable<object[]> Test16_Ibts_E_G_1_Data {
			get {
				yield return new object[] { "0FA7 CE", 3, Code.Ibts_rm16_r16, Register.SI, Register.CX, DecoderOptions.Xbts };
				yield return new object[] { "66 0FA7 CE", 4, Code.Ibts_rm32_r32, Register.ESI, Register.ECX, DecoderOptions.Xbts };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Ibts_E_G_2_Data))]
		void Test16_Ibts_E_G_2(string hexBytes, int byteLength, Code code, MemorySize memSize, Register reg, DecoderOptions options) {
			var decoder = CreateDecoder16(hexBytes, options);
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
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test16_Ibts_E_G_2_Data {
			get {
				yield return new object[] { "0FA7 18", 3, Code.Ibts_rm16_r16, MemorySize.UInt16, Register.BX, DecoderOptions.Xbts };
				yield return new object[] { "66 0FA7 18", 4, Code.Ibts_rm32_r32, MemorySize.UInt32, Register.EBX, DecoderOptions.Xbts };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Ibts_E_G_1_Data))]
		void Test32_Ibts_E_G_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, DecoderOptions options) {
			var decoder = CreateDecoder32(hexBytes, options);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
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
		public static IEnumerable<object[]> Test32_Ibts_E_G_1_Data {
			get {
				yield return new object[] { "66 0FA7 CE", 4, Code.Ibts_rm16_r16, Register.SI, Register.CX, DecoderOptions.Xbts };
				yield return new object[] { "0FA7 CE", 3, Code.Ibts_rm32_r32, Register.ESI, Register.ECX, DecoderOptions.Xbts };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Ibts_E_G_2_Data))]
		void Test32_Ibts_E_G_2(string hexBytes, int byteLength, Code code, MemorySize memSize, Register reg, DecoderOptions options) {
			var decoder = CreateDecoder32(hexBytes, options);
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
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test32_Ibts_E_G_2_Data {
			get {
				yield return new object[] { "66 0FA7 18", 4, Code.Ibts_rm16_r16, MemorySize.UInt16, Register.BX, DecoderOptions.Xbts };
				yield return new object[] { "0FA7 18", 3, Code.Ibts_rm32_r32, MemorySize.UInt32, Register.EBX, DecoderOptions.Xbts };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Cmpxchg486_E_G_1_Data))]
		void Test16_Cmpxchg486_E_G_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, DecoderOptions options) {
			var decoder = CreateDecoder16(hexBytes, options);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
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
		public static IEnumerable<object[]> Test16_Cmpxchg486_E_G_1_Data {
			get {
				yield return new object[] { "0FA6 CE", 3, Code.Cmpxchg486_rm8_r8, Register.DH, Register.CL, DecoderOptions.Cmpxchg486A };
				yield return new object[] { "0FA7 CE", 3, Code.Cmpxchg486_rm16_r16, Register.SI, Register.CX, DecoderOptions.Cmpxchg486A };
				yield return new object[] { "66 0FA7 CE", 4, Code.Cmpxchg486_rm32_r32, Register.ESI, Register.ECX, DecoderOptions.Cmpxchg486A };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Cmpxchg486_E_G_2_Data))]
		void Test16_Cmpxchg486_E_G_2(string hexBytes, int byteLength, Code code, MemorySize memSize, Register reg, DecoderOptions options) {
			var decoder = CreateDecoder16(hexBytes, options);
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
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test16_Cmpxchg486_E_G_2_Data {
			get {
				yield return new object[] { "0FA6 18", 3, Code.Cmpxchg486_rm8_r8, MemorySize.UInt8, Register.BL, DecoderOptions.Cmpxchg486A };
				yield return new object[] { "0FA7 18", 3, Code.Cmpxchg486_rm16_r16, MemorySize.UInt16, Register.BX, DecoderOptions.Cmpxchg486A };
				yield return new object[] { "66 0FA7 18", 4, Code.Cmpxchg486_rm32_r32, MemorySize.UInt32, Register.EBX, DecoderOptions.Cmpxchg486A };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Cmpxchg486_E_G_1_Data))]
		void Test32_Cmpxchg486_E_G_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, DecoderOptions options) {
			var decoder = CreateDecoder32(hexBytes, options);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
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
		public static IEnumerable<object[]> Test32_Cmpxchg486_E_G_1_Data {
			get {
				yield return new object[] { "0FA6 CE", 3, Code.Cmpxchg486_rm8_r8, Register.DH, Register.CL, DecoderOptions.Cmpxchg486A };
				yield return new object[] { "66 0FA7 CE", 4, Code.Cmpxchg486_rm16_r16, Register.SI, Register.CX, DecoderOptions.Cmpxchg486A };
				yield return new object[] { "0FA7 CE", 3, Code.Cmpxchg486_rm32_r32, Register.ESI, Register.ECX, DecoderOptions.Cmpxchg486A };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Cmpxchg486_E_G_2_Data))]
		void Test32_Cmpxchg486_E_G_2(string hexBytes, int byteLength, Code code, MemorySize memSize, Register reg, DecoderOptions options) {
			var decoder = CreateDecoder32(hexBytes, options);
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
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test32_Cmpxchg486_E_G_2_Data {
			get {
				yield return new object[] { "0FA6 18", 3, Code.Cmpxchg486_rm8_r8, MemorySize.UInt8, Register.BL, DecoderOptions.Cmpxchg486A };
				yield return new object[] { "66 0FA7 18", 4, Code.Cmpxchg486_rm16_r16, MemorySize.UInt16, Register.BX, DecoderOptions.Cmpxchg486A };
				yield return new object[] { "0FA7 18", 3, Code.Cmpxchg486_rm32_r32, MemorySize.UInt32, Register.EBX, DecoderOptions.Cmpxchg486A };
			}
		}
	}
}
