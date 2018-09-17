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
	public sealed class DecoderTest_2_0FC0_0FC7 : DecoderTest {
		[Fact]
		void Test16_Xadd_rm8_r8_1() {
			var decoder = CreateDecoder16("0FC0 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Xadd_rm8_r8, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
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
		void Test16_Xadd_rm8_r8_2() {
			var decoder = CreateDecoder16("0FC0 38");
			var instr = decoder.Decode();

			Assert.Equal(Code.Xadd_rm8_r8, instr.Code);
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
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.BH, instr.Op1Register);
		}

		[Fact]
		void Test32_Xadd_rm8_r8_1() {
			var decoder = CreateDecoder32("0FC0 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Xadd_rm8_r8, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
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
		void Test32_Xadd_rm8_r8_2() {
			var decoder = CreateDecoder32("0FC0 38");
			var instr = decoder.Decode();

			Assert.Equal(Code.Xadd_rm8_r8, instr.Code);
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
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.BH, instr.Op1Register);
		}

		[Theory]
		[InlineData("0FC0 CE", 3, Register.DH, Register.CL)]
		[InlineData("40 0FC0 D5", 4, Register.BPL, Register.DL)]
		[InlineData("40 0FC0 FA", 4, Register.DL, Register.DIL)]
		[InlineData("45 0FC0 F0", 4, Register.R8L, Register.R14L)]
		[InlineData("41 0FC0 D9", 4, Register.R9L, Register.BL)]
		[InlineData("44 0FC0 EC", 4, Register.SPL, Register.R13L)]
		[InlineData("66 67 4E 0FC0 EC", 6, Register.SPL, Register.R13L)]
		void Test64_Xadd_rm8_r8_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Xadd_rm8_r8, instr.Code);
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
		void Test64_Xadd_rm8_r8_2() {
			var decoder = CreateDecoder64("0FC0 38");
			var instr = decoder.Decode();

			Assert.Equal(Code.Xadd_rm8_r8, instr.Code);
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
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.BH, instr.Op1Register);
		}

		[Fact]
		void Test16_Xadd_rm16_r16_1() {
			var decoder = CreateDecoder16("0FC1 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Xadd_rm16_r16, instr.Code);
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
		void Test16_Xadd_rm16_r16_2() {
			var decoder = CreateDecoder16("0FC1 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Xadd_rm16_r16, instr.Code);
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
		void Test32_Xadd_rm16_r16_1() {
			var decoder = CreateDecoder32("66 0FC1 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Xadd_rm16_r16, instr.Code);
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
		void Test32_Xadd_rm16_r16_2() {
			var decoder = CreateDecoder32("66 0FC1 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Xadd_rm16_r16, instr.Code);
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
		[InlineData("66 0FC1 CE", 4, Register.SI, Register.CX)]
		[InlineData("66 44 0FC1 C5", 5, Register.BP, Register.R8W)]
		[InlineData("66 41 0FC1 D6", 5, Register.R14W, Register.DX)]
		[InlineData("66 45 0FC1 D0", 5, Register.R8W, Register.R10W)]
		[InlineData("66 41 0FC1 D9", 5, Register.R9W, Register.BX)]
		[InlineData("66 44 0FC1 EC", 5, Register.SP, Register.R13W)]
		void Test64_Xadd_rm16_r16_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Xadd_rm16_r16, instr.Code);
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
		void Test64_Xadd_rm16_r16_2() {
			var decoder = CreateDecoder64("66 0FC1 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Xadd_rm16_r16, instr.Code);
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
		void Test16_Xadd_rm32_r32_1() {
			var decoder = CreateDecoder16("66 0FC1 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Xadd_rm32_r32, instr.Code);
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
		void Test16_Xadd_rm32_r32_2() {
			var decoder = CreateDecoder16("66 0FC1 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Xadd_rm32_r32, instr.Code);
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
		void Test32_Xadd_rm32_r32_1() {
			var decoder = CreateDecoder32("0FC1 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Xadd_rm32_r32, instr.Code);
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
		void Test32_Xadd_rm32_r32_2() {
			var decoder = CreateDecoder32("0FC1 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Xadd_rm32_r32, instr.Code);
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
		[InlineData("0FC1 CE", 3, Register.ESI, Register.ECX)]
		[InlineData("44 0FC1 C5", 4, Register.EBP, Register.R8D)]
		[InlineData("41 0FC1 D6", 4, Register.R14D, Register.EDX)]
		[InlineData("45 0FC1 D0", 4, Register.R8D, Register.R10D)]
		[InlineData("41 0FC1 D9", 4, Register.R9D, Register.EBX)]
		[InlineData("44 0FC1 EC", 4, Register.ESP, Register.R13D)]
		void Test64_Xadd_rm32_r32_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Xadd_rm32_r32, instr.Code);
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
		void Test64_Xadd_rm32_r32_2() {
			var decoder = CreateDecoder64("0FC1 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Xadd_rm32_r32, instr.Code);
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
		[InlineData("48 0FC1 CE", 4, Register.RSI, Register.RCX)]
		[InlineData("4C 0FC1 C5", 4, Register.RBP, Register.R8)]
		[InlineData("49 0FC1 D6", 4, Register.R14, Register.RDX)]
		[InlineData("4D 0FC1 D0", 4, Register.R8, Register.R10)]
		[InlineData("49 0FC1 D9", 4, Register.R9, Register.RBX)]
		[InlineData("4C 0FC1 EC", 4, Register.RSP, Register.R13)]
		void Test64_Xadd_rm64_r64_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Xadd_rm64_r64, instr.Code);
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
		void Test64_Xadd_rm64_r64_2() {
			var decoder = CreateDecoder64("48 0FC1 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Xadd_rm64_r64, instr.Code);
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

		[Theory]
		[MemberData(nameof(Test16_CmpV_VX_WX_Ib_1_Data))]
		void Test16_CmpV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_CmpV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "0FC2 08 A5", 4, Code.Cmpps_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Float32, 0xA5 };
				yield return new object[] { "66 0FC2 08 A5", 5, Code.Cmppd_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Float64, 0xA5 };
				yield return new object[] { "F3 0FC2 08 A5", 5, Code.Cmpss_xmm_xmmm32_imm8, Register.XMM1, MemorySize.Float32, 0xA5 };
				yield return new object[] { "F2 0FC2 08 A5", 5, Code.Cmpsd_xmm_xmmm64_imm8, Register.XMM1, MemorySize.Float64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_CmpV_VX_WX_Ib_2_Data))]
		void Test16_CmpV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_CmpV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "0FC2 CD A5", 4, Code.Cmpps_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };

				yield return new object[] { "66 0FC2 CD A5", 5, Code.Cmppd_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };

				yield return new object[] { "F3 0FC2 CD A5", 5, Code.Cmpss_xmm_xmmm32_imm8, Register.XMM1, Register.XMM5, 0xA5 };

				yield return new object[] { "F2 0FC2 CD A5", 5, Code.Cmpsd_xmm_xmmm64_imm8, Register.XMM1, Register.XMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_CmpV_VX_WX_Ib_1_Data))]
		void Test32_CmpV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_CmpV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "0FC2 08 A5", 4, Code.Cmpps_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Float32, 0xA5 };
				yield return new object[] { "66 0FC2 08 A5", 5, Code.Cmppd_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Float64, 0xA5 };
				yield return new object[] { "F3 0FC2 08 A5", 5, Code.Cmpss_xmm_xmmm32_imm8, Register.XMM1, MemorySize.Float32, 0xA5 };
				yield return new object[] { "F2 0FC2 08 A5", 5, Code.Cmpsd_xmm_xmmm64_imm8, Register.XMM1, MemorySize.Float64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_CmpV_VX_WX_Ib_2_Data))]
		void Test32_CmpV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_CmpV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "0FC2 CD A5", 4, Code.Cmpps_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };

				yield return new object[] { "66 0FC2 CD A5", 5, Code.Cmppd_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };

				yield return new object[] { "F3 0FC2 CD A5", 5, Code.Cmpss_xmm_xmmm32_imm8, Register.XMM1, Register.XMM5, 0xA5 };

				yield return new object[] { "F2 0FC2 CD A5", 5, Code.Cmpsd_xmm_xmmm64_imm8, Register.XMM1, Register.XMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_CmpV_VX_WX_Ib_1_Data))]
		void Test64_CmpV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_CmpV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "0FC2 08 A5", 4, Code.Cmpps_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Float32, 0xA5 };
				yield return new object[] { "66 0FC2 08 A5", 5, Code.Cmppd_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Float64, 0xA5 };
				yield return new object[] { "F3 0FC2 08 A5", 5, Code.Cmpss_xmm_xmmm32_imm8, Register.XMM1, MemorySize.Float32, 0xA5 };
				yield return new object[] { "F2 0FC2 08 A5", 5, Code.Cmpsd_xmm_xmmm64_imm8, Register.XMM1, MemorySize.Float64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_CmpV_VX_WX_Ib_2_Data))]
		void Test64_CmpV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_CmpV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "0FC2 CD A5", 4, Code.Cmpps_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
				yield return new object[] { "44 0FC2 CD A5", 5, Code.Cmpps_xmm_xmmm128_imm8, Register.XMM9, Register.XMM5, 0xA5 };
				yield return new object[] { "41 0FC2 CD A5", 5, Code.Cmpps_xmm_xmmm128_imm8, Register.XMM1, Register.XMM13, 0xA5 };
				yield return new object[] { "45 0FC2 CD A5", 5, Code.Cmpps_xmm_xmmm128_imm8, Register.XMM9, Register.XMM13, 0xA5 };

				yield return new object[] { "66 0FC2 CD A5", 5, Code.Cmppd_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
				yield return new object[] { "66 44 0FC2 CD A5", 6, Code.Cmppd_xmm_xmmm128_imm8, Register.XMM9, Register.XMM5, 0xA5 };
				yield return new object[] { "66 41 0FC2 CD A5", 6, Code.Cmppd_xmm_xmmm128_imm8, Register.XMM1, Register.XMM13, 0xA5 };
				yield return new object[] { "66 45 0FC2 CD A5", 6, Code.Cmppd_xmm_xmmm128_imm8, Register.XMM9, Register.XMM13, 0xA5 };

				yield return new object[] { "F3 0FC2 CD A5", 5, Code.Cmpss_xmm_xmmm32_imm8, Register.XMM1, Register.XMM5, 0xA5 };
				yield return new object[] { "F3 44 0FC2 CD A5", 6, Code.Cmpss_xmm_xmmm32_imm8, Register.XMM9, Register.XMM5, 0xA5 };
				yield return new object[] { "F3 41 0FC2 CD A5", 6, Code.Cmpss_xmm_xmmm32_imm8, Register.XMM1, Register.XMM13, 0xA5 };
				yield return new object[] { "F3 45 0FC2 CD A5", 6, Code.Cmpss_xmm_xmmm32_imm8, Register.XMM9, Register.XMM13, 0xA5 };

				yield return new object[] { "F2 0FC2 CD A5", 5, Code.Cmpsd_xmm_xmmm64_imm8, Register.XMM1, Register.XMM5, 0xA5 };
				yield return new object[] { "F2 44 0FC2 CD A5", 6, Code.Cmpsd_xmm_xmmm64_imm8, Register.XMM9, Register.XMM5, 0xA5 };
				yield return new object[] { "F2 41 0FC2 CD A5", 6, Code.Cmpsd_xmm_xmmm64_imm8, Register.XMM1, Register.XMM13, 0xA5 };
				yield return new object[] { "F2 45 0FC2 CD A5", 6, Code.Cmpsd_xmm_xmmm64_imm8, Register.XMM9, Register.XMM13, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VcmpV_VX_HX_WX_Ib_1_Data))]
		void Test16_VcmpV_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_VcmpV_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C5C8 C2 10 A5", 5, Code.VEX_Vcmpps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, 0xA5 };
				yield return new object[] { "C5CC C2 10 A5", 5, Code.VEX_Vcmpps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, 0xA5 };
				yield return new object[] { "C4E1C8 C2 10 A5", 6, Code.VEX_Vcmpps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, 0xA5 };
				yield return new object[] { "C4E1CC C2 10 A5", 6, Code.VEX_Vcmpps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, 0xA5 };

				yield return new object[] { "C5C9 C2 10 A5", 5, Code.VEX_Vcmppd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, 0xA5 };
				yield return new object[] { "C5CD C2 10 A5", 5, Code.VEX_Vcmppd_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, 0xA5 };
				yield return new object[] { "C4E1C9 C2 10 A5", 6, Code.VEX_Vcmppd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, 0xA5 };
				yield return new object[] { "C4E1CD C2 10 A5", 6, Code.VEX_Vcmppd_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, 0xA5 };

				yield return new object[] { "C5CA C2 10 A5", 5, Code.VEX_Vcmpss_xmm_xmm_xmmm32_imm8, Register.XMM2, Register.XMM6, MemorySize.Float32, 0xA5 };
				yield return new object[] { "C4E1CA C2 10 A5", 6, Code.VEX_Vcmpss_xmm_xmm_xmmm32_imm8, Register.XMM2, Register.XMM6, MemorySize.Float32, 0xA5 };
				yield return new object[] { "C5CE C2 10 A5", 5, Code.VEX_Vcmpss_xmm_xmm_xmmm32_imm8, Register.XMM2, Register.XMM6, MemorySize.Float32, 0xA5 };
				yield return new object[] { "C4E1CE C2 10 A5", 6, Code.VEX_Vcmpss_xmm_xmm_xmmm32_imm8, Register.XMM2, Register.XMM6, MemorySize.Float32, 0xA5 };

				yield return new object[] { "C5CB C2 10 A5", 5, Code.VEX_Vcmpsd_xmm_xmm_xmmm64_imm8, Register.XMM2, Register.XMM6, MemorySize.Float64, 0xA5 };
				yield return new object[] { "C4E1CB C2 10 A5", 6, Code.VEX_Vcmpsd_xmm_xmm_xmmm64_imm8, Register.XMM2, Register.XMM6, MemorySize.Float64, 0xA5 };
				yield return new object[] { "C5CF C2 10 A5", 5, Code.VEX_Vcmpsd_xmm_xmm_xmmm64_imm8, Register.XMM2, Register.XMM6, MemorySize.Float64, 0xA5 };
				yield return new object[] { "C4E1CF C2 10 A5", 6, Code.VEX_Vcmpsd_xmm_xmm_xmmm64_imm8, Register.XMM2, Register.XMM6, MemorySize.Float64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VcmpV_VX_HX_WX_Ib_2_Data))]
		void Test16_VcmpV_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_VcmpV_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C5C8 C2 D3 A5", 5, Code.VEX_Vcmpps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C5CC C2 D3 A5", 5, Code.VEX_Vcmpps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5 };

				yield return new object[] { "C5C9 C2 D3 A5", 5, Code.VEX_Vcmppd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C5CD C2 D3 A5", 5, Code.VEX_Vcmppd_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5 };

				yield return new object[] { "C5CA C2 D3 A5", 5, Code.VEX_Vcmpss_xmm_xmm_xmmm32_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };

				yield return new object[] { "C5CB C2 D3 A5", 5, Code.VEX_Vcmpsd_xmm_xmm_xmmm64_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VcmpV_VX_HX_WX_Ib_1_Data))]
		void Test32_VcmpV_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_VcmpV_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C5C8 C2 10 A5", 5, Code.VEX_Vcmpps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, 0xA5 };
				yield return new object[] { "C5CC C2 10 A5", 5, Code.VEX_Vcmpps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, 0xA5 };
				yield return new object[] { "C4E1C8 C2 10 A5", 6, Code.VEX_Vcmpps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, 0xA5 };
				yield return new object[] { "C4E1CC C2 10 A5", 6, Code.VEX_Vcmpps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, 0xA5 };

				yield return new object[] { "C5C9 C2 10 A5", 5, Code.VEX_Vcmppd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, 0xA5 };
				yield return new object[] { "C5CD C2 10 A5", 5, Code.VEX_Vcmppd_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, 0xA5 };
				yield return new object[] { "C4E1C9 C2 10 A5", 6, Code.VEX_Vcmppd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, 0xA5 };
				yield return new object[] { "C4E1CD C2 10 A5", 6, Code.VEX_Vcmppd_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, 0xA5 };

				yield return new object[] { "C5CA C2 10 A5", 5, Code.VEX_Vcmpss_xmm_xmm_xmmm32_imm8, Register.XMM2, Register.XMM6, MemorySize.Float32, 0xA5 };
				yield return new object[] { "C4E1CA C2 10 A5", 6, Code.VEX_Vcmpss_xmm_xmm_xmmm32_imm8, Register.XMM2, Register.XMM6, MemorySize.Float32, 0xA5 };
				yield return new object[] { "C5CE C2 10 A5", 5, Code.VEX_Vcmpss_xmm_xmm_xmmm32_imm8, Register.XMM2, Register.XMM6, MemorySize.Float32, 0xA5 };
				yield return new object[] { "C4E1CE C2 10 A5", 6, Code.VEX_Vcmpss_xmm_xmm_xmmm32_imm8, Register.XMM2, Register.XMM6, MemorySize.Float32, 0xA5 };

				yield return new object[] { "C5CB C2 10 A5", 5, Code.VEX_Vcmpsd_xmm_xmm_xmmm64_imm8, Register.XMM2, Register.XMM6, MemorySize.Float64, 0xA5 };
				yield return new object[] { "C4E1CB C2 10 A5", 6, Code.VEX_Vcmpsd_xmm_xmm_xmmm64_imm8, Register.XMM2, Register.XMM6, MemorySize.Float64, 0xA5 };
				yield return new object[] { "C5CF C2 10 A5", 5, Code.VEX_Vcmpsd_xmm_xmm_xmmm64_imm8, Register.XMM2, Register.XMM6, MemorySize.Float64, 0xA5 };
				yield return new object[] { "C4E1CF C2 10 A5", 6, Code.VEX_Vcmpsd_xmm_xmm_xmmm64_imm8, Register.XMM2, Register.XMM6, MemorySize.Float64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VcmpV_VX_HX_WX_Ib_2_Data))]
		void Test32_VcmpV_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_VcmpV_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C5C8 C2 D3 A5", 5, Code.VEX_Vcmpps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C5CC C2 D3 A5", 5, Code.VEX_Vcmpps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5 };

				yield return new object[] { "C5C9 C2 D3 A5", 5, Code.VEX_Vcmppd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C5CD C2 D3 A5", 5, Code.VEX_Vcmppd_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5 };

				yield return new object[] { "C5CA C2 D3 A5", 5, Code.VEX_Vcmpss_xmm_xmm_xmmm32_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };

				yield return new object[] { "C5CB C2 D3 A5", 5, Code.VEX_Vcmpsd_xmm_xmm_xmmm64_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VcmpV_VX_HX_WX_Ib_1_Data))]
		void Test64_VcmpV_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_VcmpV_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C5C8 C2 10 A5", 5, Code.VEX_Vcmpps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, 0xA5 };
				yield return new object[] { "C5CC C2 10 A5", 5, Code.VEX_Vcmpps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, 0xA5 };
				yield return new object[] { "C4E1C8 C2 10 A5", 6, Code.VEX_Vcmpps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, 0xA5 };
				yield return new object[] { "C4E1CC C2 10 A5", 6, Code.VEX_Vcmpps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, 0xA5 };

				yield return new object[] { "C5C9 C2 10 A5", 5, Code.VEX_Vcmppd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, 0xA5 };
				yield return new object[] { "C5CD C2 10 A5", 5, Code.VEX_Vcmppd_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, 0xA5 };
				yield return new object[] { "C4E1C9 C2 10 A5", 6, Code.VEX_Vcmppd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, 0xA5 };
				yield return new object[] { "C4E1CD C2 10 A5", 6, Code.VEX_Vcmppd_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, 0xA5 };

				yield return new object[] { "C5CA C2 10 A5", 5, Code.VEX_Vcmpss_xmm_xmm_xmmm32_imm8, Register.XMM2, Register.XMM6, MemorySize.Float32, 0xA5 };
				yield return new object[] { "C4E1CA C2 10 A5", 6, Code.VEX_Vcmpss_xmm_xmm_xmmm32_imm8, Register.XMM2, Register.XMM6, MemorySize.Float32, 0xA5 };
				yield return new object[] { "C5CE C2 10 A5", 5, Code.VEX_Vcmpss_xmm_xmm_xmmm32_imm8, Register.XMM2, Register.XMM6, MemorySize.Float32, 0xA5 };
				yield return new object[] { "C4E1CE C2 10 A5", 6, Code.VEX_Vcmpss_xmm_xmm_xmmm32_imm8, Register.XMM2, Register.XMM6, MemorySize.Float32, 0xA5 };

				yield return new object[] { "C5CB C2 10 A5", 5, Code.VEX_Vcmpsd_xmm_xmm_xmmm64_imm8, Register.XMM2, Register.XMM6, MemorySize.Float64, 0xA5 };
				yield return new object[] { "C4E1CB C2 10 A5", 6, Code.VEX_Vcmpsd_xmm_xmm_xmmm64_imm8, Register.XMM2, Register.XMM6, MemorySize.Float64, 0xA5 };
				yield return new object[] { "C5CF C2 10 A5", 5, Code.VEX_Vcmpsd_xmm_xmm_xmmm64_imm8, Register.XMM2, Register.XMM6, MemorySize.Float64, 0xA5 };
				yield return new object[] { "C4E1CF C2 10 A5", 6, Code.VEX_Vcmpsd_xmm_xmm_xmmm64_imm8, Register.XMM2, Register.XMM6, MemorySize.Float64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VcmpV_VX_HX_WX_Ib_2_Data))]
		void Test64_VcmpV_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_VcmpV_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C5C8 C2 D3 A5", 5, Code.VEX_Vcmpps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C5CC C2 D3 A5", 5, Code.VEX_Vcmpps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5 };
				yield return new object[] { "C548 C2 D3 A5", 5, Code.VEX_Vcmpps_xmm_xmm_xmmm128_imm8, Register.XMM10, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C54C C2 D3 A5", 5, Code.VEX_Vcmpps_ymm_ymm_ymmm256_imm8, Register.YMM10, Register.YMM6, Register.YMM3, 0xA5 };
				yield return new object[] { "C588 C2 D3 A5", 5, Code.VEX_Vcmpps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM14, Register.XMM3, 0xA5 };
				yield return new object[] { "C58C C2 D3 A5", 5, Code.VEX_Vcmpps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM14, Register.YMM3, 0xA5 };
				yield return new object[] { "C4C148 C2 D3 A5", 6, Code.VEX_Vcmpps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM11, 0xA5 };
				yield return new object[] { "C4C14C C2 D3 A5", 6, Code.VEX_Vcmpps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM11, 0xA5 };

				yield return new object[] { "C5C9 C2 D3 A5", 5, Code.VEX_Vcmppd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C5CD C2 D3 A5", 5, Code.VEX_Vcmppd_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5 };
				yield return new object[] { "C549 C2 D3 A5", 5, Code.VEX_Vcmppd_xmm_xmm_xmmm128_imm8, Register.XMM10, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C54D C2 D3 A5", 5, Code.VEX_Vcmppd_ymm_ymm_ymmm256_imm8, Register.YMM10, Register.YMM6, Register.YMM3, 0xA5 };
				yield return new object[] { "C589 C2 D3 A5", 5, Code.VEX_Vcmppd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM14, Register.XMM3, 0xA5 };
				yield return new object[] { "C58D C2 D3 A5", 5, Code.VEX_Vcmppd_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM14, Register.YMM3, 0xA5 };
				yield return new object[] { "C4C149 C2 D3 A5", 6, Code.VEX_Vcmppd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM11, 0xA5 };
				yield return new object[] { "C4C14D C2 D3 A5", 6, Code.VEX_Vcmppd_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM11, 0xA5 };

				yield return new object[] { "C5CA C2 D3 A5", 5, Code.VEX_Vcmpss_xmm_xmm_xmmm32_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C54A C2 D3 A5", 5, Code.VEX_Vcmpss_xmm_xmm_xmmm32_imm8, Register.XMM10, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C58A C2 D3 A5", 5, Code.VEX_Vcmpss_xmm_xmm_xmmm32_imm8, Register.XMM2, Register.XMM14, Register.XMM3, 0xA5 };
				yield return new object[] { "C4C14A C2 D3 A5", 6, Code.VEX_Vcmpss_xmm_xmm_xmmm32_imm8, Register.XMM2, Register.XMM6, Register.XMM11, 0xA5 };

				yield return new object[] { "C5CB C2 D3 A5", 5, Code.VEX_Vcmpsd_xmm_xmm_xmmm64_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C54B C2 D3 A5", 5, Code.VEX_Vcmpsd_xmm_xmm_xmmm64_imm8, Register.XMM10, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C58B C2 D3 A5", 5, Code.VEX_Vcmpsd_xmm_xmm_xmmm64_imm8, Register.XMM2, Register.XMM14, Register.XMM3, 0xA5 };
				yield return new object[] { "C4C14B C2 D3 A5", 6, Code.VEX_Vcmpsd_xmm_xmm_xmmm64_imm8, Register.XMM2, Register.XMM6, Register.XMM11, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VcmpV_k1_k2_HX_WX_Ib_1_Data))]
		void Test16_VcmpV_k1_k2_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_VcmpV_k1_k2_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F14C0B C2 50 01 A5", 8, Code.EVEX_Vcmpps_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, 0xA5 };
				yield return new object[] { "62 F14C1D C2 50 01 A5", 8, Code.EVEX_Vcmpps_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, 0xA5 };
				yield return new object[] { "62 F14C08 C2 50 01 A5", 8, Code.EVEX_Vcmpps_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, 0xA5 };

				yield return new object[] { "62 F14C2B C2 50 01 A5", 8, Code.EVEX_Vcmpps_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, 0xA5 };
				yield return new object[] { "62 F14C3D C2 50 01 A5", 8, Code.EVEX_Vcmpps_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, 0xA5 };
				yield return new object[] { "62 F14C28 C2 50 01 A5", 8, Code.EVEX_Vcmpps_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, 0xA5 };

				yield return new object[] { "62 F14C4B C2 50 01 A5", 8, Code.EVEX_Vcmpps_k_k1_zmm_zmmm512b32_imm8_sae, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, 0xA5 };
				yield return new object[] { "62 F14C5D C2 50 01 A5", 8, Code.EVEX_Vcmpps_k_k1_zmm_zmmm512b32_imm8_sae, Register.K2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, 0xA5 };
				yield return new object[] { "62 F14C48 C2 50 01 A5", 8, Code.EVEX_Vcmpps_k_k1_zmm_zmmm512b32_imm8_sae, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, 0xA5 };

				yield return new object[] { "62 F1CD0B C2 50 01 A5", 8, Code.EVEX_Vcmppd_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, 0xA5 };
				yield return new object[] { "62 F1CD1D C2 50 01 A5", 8, Code.EVEX_Vcmppd_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, 0xA5 };
				yield return new object[] { "62 F1CD08 C2 50 01 A5", 8, Code.EVEX_Vcmppd_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, 0xA5 };

				yield return new object[] { "62 F1CD2B C2 50 01 A5", 8, Code.EVEX_Vcmppd_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, 0xA5 };
				yield return new object[] { "62 F1CD3D C2 50 01 A5", 8, Code.EVEX_Vcmppd_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, 0xA5 };
				yield return new object[] { "62 F1CD28 C2 50 01 A5", 8, Code.EVEX_Vcmppd_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, 0xA5 };

				yield return new object[] { "62 F1CD4B C2 50 01 A5", 8, Code.EVEX_Vcmppd_k_k1_zmm_zmmm512b64_imm8_sae, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, 0xA5 };
				yield return new object[] { "62 F1CD5D C2 50 01 A5", 8, Code.EVEX_Vcmppd_k_k1_zmm_zmmm512b64_imm8_sae, Register.K2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, 0xA5 };
				yield return new object[] { "62 F1CD48 C2 50 01 A5", 8, Code.EVEX_Vcmppd_k_k1_zmm_zmmm512b64_imm8_sae, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, 0xA5 };

				yield return new object[] { "62 F14E0B C2 50 01 A5", 8, Code.EVEX_Vcmpss_k_k1_xmm_xmmm32_imm8_sae, Register.K2, Register.XMM6, Register.K3, MemorySize.Float32, 4, 0xA5 };
				yield return new object[] { "62 F14E08 C2 50 01 A5", 8, Code.EVEX_Vcmpss_k_k1_xmm_xmmm32_imm8_sae, Register.K2, Register.XMM6, Register.None, MemorySize.Float32, 4, 0xA5 };
				yield return new object[] { "62 F14E28 C2 50 01 A5", 8, Code.EVEX_Vcmpss_k_k1_xmm_xmmm32_imm8_sae, Register.K2, Register.XMM6, Register.None, MemorySize.Float32, 4, 0xA5 };
				yield return new object[] { "62 F14E48 C2 50 01 A5", 8, Code.EVEX_Vcmpss_k_k1_xmm_xmmm32_imm8_sae, Register.K2, Register.XMM6, Register.None, MemorySize.Float32, 4, 0xA5 };
				yield return new object[] { "62 F14E68 C2 50 01 A5", 8, Code.EVEX_Vcmpss_k_k1_xmm_xmmm32_imm8_sae, Register.K2, Register.XMM6, Register.None, MemorySize.Float32, 4, 0xA5 };

				yield return new object[] { "62 F1CF0B C2 50 01 A5", 8, Code.EVEX_Vcmpsd_k_k1_xmm_xmmm64_imm8_sae, Register.K2, Register.XMM6, Register.K3, MemorySize.Float64, 8, 0xA5 };
				yield return new object[] { "62 F1CF08 C2 50 01 A5", 8, Code.EVEX_Vcmpsd_k_k1_xmm_xmmm64_imm8_sae, Register.K2, Register.XMM6, Register.None, MemorySize.Float64, 8, 0xA5 };
				yield return new object[] { "62 F1CF28 C2 50 01 A5", 8, Code.EVEX_Vcmpsd_k_k1_xmm_xmmm64_imm8_sae, Register.K2, Register.XMM6, Register.None, MemorySize.Float64, 8, 0xA5 };
				yield return new object[] { "62 F1CF48 C2 50 01 A5", 8, Code.EVEX_Vcmpsd_k_k1_xmm_xmmm64_imm8_sae, Register.K2, Register.XMM6, Register.None, MemorySize.Float64, 8, 0xA5 };
				yield return new object[] { "62 F1CF68 C2 50 01 A5", 8, Code.EVEX_Vcmpsd_k_k1_xmm_xmmm64_imm8_sae, Register.K2, Register.XMM6, Register.None, MemorySize.Float64, 8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VcmpV_k1_k2_HX_WX_Ib_2_Data))]
		void Test16_VcmpV_k1_k2_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool sae, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_VcmpV_k1_k2_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F14C0B C2 D3 A5", 7, Code.EVEX_Vcmpps_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F14C5B C2 D3 A5", 7, Code.EVEX_Vcmpps_k_k1_zmm_zmmm512b32_imm8_sae, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F14C2B C2 D3 A5", 7, Code.EVEX_Vcmpps_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F14C1B C2 D3 A5", 7, Code.EVEX_Vcmpps_k_k1_zmm_zmmm512b32_imm8_sae, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F14C4B C2 D3 A5", 7, Code.EVEX_Vcmpps_k_k1_zmm_zmmm512b32_imm8_sae, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F14C3B C2 D3 A5", 7, Code.EVEX_Vcmpps_k_k1_zmm_zmmm512b32_imm8_sae, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F1CD0B C2 D3 A5", 7, Code.EVEX_Vcmppd_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F1CD5B C2 D3 A5", 7, Code.EVEX_Vcmppd_k_k1_zmm_zmmm512b64_imm8_sae, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F1CD2B C2 D3 A5", 7, Code.EVEX_Vcmppd_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F1CD1B C2 D3 A5", 7, Code.EVEX_Vcmppd_k_k1_zmm_zmmm512b64_imm8_sae, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F1CD4B C2 D3 A5", 7, Code.EVEX_Vcmppd_k_k1_zmm_zmmm512b64_imm8_sae, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F1CD7B C2 D3 A5", 7, Code.EVEX_Vcmppd_k_k1_zmm_zmmm512b64_imm8_sae, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F14E0B C2 D3 A5", 7, Code.EVEX_Vcmpss_k_k1_xmm_xmmm32_imm8_sae, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F14E5B C2 D3 A5", 7, Code.EVEX_Vcmpss_k_k1_xmm_xmmm32_imm8_sae, Register.K2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F1CF0B C2 D3 A5", 7, Code.EVEX_Vcmpsd_k_k1_xmm_xmmm64_imm8_sae, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F1CF7B C2 D3 A5", 7, Code.EVEX_Vcmpsd_k_k1_xmm_xmmm64_imm8_sae, Register.K2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VcmpV_k1_k2_HX_WX_Ib_1_Data))]
		void Test32_VcmpV_k1_k2_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_VcmpV_k1_k2_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F14C0B C2 50 01 A5", 8, Code.EVEX_Vcmpps_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, 0xA5 };
				yield return new object[] { "62 F14C1D C2 50 01 A5", 8, Code.EVEX_Vcmpps_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, 0xA5 };
				yield return new object[] { "62 F14C08 C2 50 01 A5", 8, Code.EVEX_Vcmpps_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, 0xA5 };

				yield return new object[] { "62 F14C2B C2 50 01 A5", 8, Code.EVEX_Vcmpps_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, 0xA5 };
				yield return new object[] { "62 F14C3D C2 50 01 A5", 8, Code.EVEX_Vcmpps_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, 0xA5 };
				yield return new object[] { "62 F14C28 C2 50 01 A5", 8, Code.EVEX_Vcmpps_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, 0xA5 };

				yield return new object[] { "62 F14C4B C2 50 01 A5", 8, Code.EVEX_Vcmpps_k_k1_zmm_zmmm512b32_imm8_sae, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, 0xA5 };
				yield return new object[] { "62 F14C5D C2 50 01 A5", 8, Code.EVEX_Vcmpps_k_k1_zmm_zmmm512b32_imm8_sae, Register.K2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, 0xA5 };
				yield return new object[] { "62 F14C48 C2 50 01 A5", 8, Code.EVEX_Vcmpps_k_k1_zmm_zmmm512b32_imm8_sae, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, 0xA5 };

				yield return new object[] { "62 F1CD0B C2 50 01 A5", 8, Code.EVEX_Vcmppd_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, 0xA5 };
				yield return new object[] { "62 F1CD1D C2 50 01 A5", 8, Code.EVEX_Vcmppd_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, 0xA5 };
				yield return new object[] { "62 F1CD08 C2 50 01 A5", 8, Code.EVEX_Vcmppd_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, 0xA5 };

				yield return new object[] { "62 F1CD2B C2 50 01 A5", 8, Code.EVEX_Vcmppd_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, 0xA5 };
				yield return new object[] { "62 F1CD3D C2 50 01 A5", 8, Code.EVEX_Vcmppd_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, 0xA5 };
				yield return new object[] { "62 F1CD28 C2 50 01 A5", 8, Code.EVEX_Vcmppd_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, 0xA5 };

				yield return new object[] { "62 F1CD4B C2 50 01 A5", 8, Code.EVEX_Vcmppd_k_k1_zmm_zmmm512b64_imm8_sae, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, 0xA5 };
				yield return new object[] { "62 F1CD5D C2 50 01 A5", 8, Code.EVEX_Vcmppd_k_k1_zmm_zmmm512b64_imm8_sae, Register.K2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, 0xA5 };
				yield return new object[] { "62 F1CD48 C2 50 01 A5", 8, Code.EVEX_Vcmppd_k_k1_zmm_zmmm512b64_imm8_sae, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, 0xA5 };

				yield return new object[] { "62 F14E0B C2 50 01 A5", 8, Code.EVEX_Vcmpss_k_k1_xmm_xmmm32_imm8_sae, Register.K2, Register.XMM6, Register.K3, MemorySize.Float32, 4, 0xA5 };
				yield return new object[] { "62 F14E08 C2 50 01 A5", 8, Code.EVEX_Vcmpss_k_k1_xmm_xmmm32_imm8_sae, Register.K2, Register.XMM6, Register.None, MemorySize.Float32, 4, 0xA5 };
				yield return new object[] { "62 F14E28 C2 50 01 A5", 8, Code.EVEX_Vcmpss_k_k1_xmm_xmmm32_imm8_sae, Register.K2, Register.XMM6, Register.None, MemorySize.Float32, 4, 0xA5 };
				yield return new object[] { "62 F14E48 C2 50 01 A5", 8, Code.EVEX_Vcmpss_k_k1_xmm_xmmm32_imm8_sae, Register.K2, Register.XMM6, Register.None, MemorySize.Float32, 4, 0xA5 };
				yield return new object[] { "62 F14E68 C2 50 01 A5", 8, Code.EVEX_Vcmpss_k_k1_xmm_xmmm32_imm8_sae, Register.K2, Register.XMM6, Register.None, MemorySize.Float32, 4, 0xA5 };

				yield return new object[] { "62 F1CF0B C2 50 01 A5", 8, Code.EVEX_Vcmpsd_k_k1_xmm_xmmm64_imm8_sae, Register.K2, Register.XMM6, Register.K3, MemorySize.Float64, 8, 0xA5 };
				yield return new object[] { "62 F1CF08 C2 50 01 A5", 8, Code.EVEX_Vcmpsd_k_k1_xmm_xmmm64_imm8_sae, Register.K2, Register.XMM6, Register.None, MemorySize.Float64, 8, 0xA5 };
				yield return new object[] { "62 F1CF28 C2 50 01 A5", 8, Code.EVEX_Vcmpsd_k_k1_xmm_xmmm64_imm8_sae, Register.K2, Register.XMM6, Register.None, MemorySize.Float64, 8, 0xA5 };
				yield return new object[] { "62 F1CF48 C2 50 01 A5", 8, Code.EVEX_Vcmpsd_k_k1_xmm_xmmm64_imm8_sae, Register.K2, Register.XMM6, Register.None, MemorySize.Float64, 8, 0xA5 };
				yield return new object[] { "62 F1CF68 C2 50 01 A5", 8, Code.EVEX_Vcmpsd_k_k1_xmm_xmmm64_imm8_sae, Register.K2, Register.XMM6, Register.None, MemorySize.Float64, 8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VcmpV_k1_k2_HX_WX_Ib_2_Data))]
		void Test32_VcmpV_k1_k2_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool sae, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_VcmpV_k1_k2_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F14C0B C2 D3 A5", 7, Code.EVEX_Vcmpps_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F14C5B C2 D3 A5", 7, Code.EVEX_Vcmpps_k_k1_zmm_zmmm512b32_imm8_sae, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F14C2B C2 D3 A5", 7, Code.EVEX_Vcmpps_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F14C1B C2 D3 A5", 7, Code.EVEX_Vcmpps_k_k1_zmm_zmmm512b32_imm8_sae, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F14C4B C2 D3 A5", 7, Code.EVEX_Vcmpps_k_k1_zmm_zmmm512b32_imm8_sae, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F14C3B C2 D3 A5", 7, Code.EVEX_Vcmpps_k_k1_zmm_zmmm512b32_imm8_sae, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F1CD0B C2 D3 A5", 7, Code.EVEX_Vcmppd_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F1CD5B C2 D3 A5", 7, Code.EVEX_Vcmppd_k_k1_zmm_zmmm512b64_imm8_sae, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F1CD2B C2 D3 A5", 7, Code.EVEX_Vcmppd_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F1CD1B C2 D3 A5", 7, Code.EVEX_Vcmppd_k_k1_zmm_zmmm512b64_imm8_sae, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F1CD4B C2 D3 A5", 7, Code.EVEX_Vcmppd_k_k1_zmm_zmmm512b64_imm8_sae, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F1CD7B C2 D3 A5", 7, Code.EVEX_Vcmppd_k_k1_zmm_zmmm512b64_imm8_sae, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F14E0B C2 D3 A5", 7, Code.EVEX_Vcmpss_k_k1_xmm_xmmm32_imm8_sae, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F14E5B C2 D3 A5", 7, Code.EVEX_Vcmpss_k_k1_xmm_xmmm32_imm8_sae, Register.K2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F1CF0B C2 D3 A5", 7, Code.EVEX_Vcmpsd_k_k1_xmm_xmmm64_imm8_sae, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F1CF7B C2 D3 A5", 7, Code.EVEX_Vcmpsd_k_k1_xmm_xmmm64_imm8_sae, Register.K2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VcmpV_k1_k2_HX_WX_Ib_1_Data))]
		void Test64_VcmpV_k1_k2_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_VcmpV_k1_k2_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F14C0B C2 50 01 A5", 8, Code.EVEX_Vcmpps_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, 0xA5 };
				yield return new object[] { "62 F14C1D C2 50 01 A5", 8, Code.EVEX_Vcmpps_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, 0xA5 };
				yield return new object[] { "62 F14C08 C2 50 01 A5", 8, Code.EVEX_Vcmpps_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, 0xA5 };

				yield return new object[] { "62 F14C2B C2 50 01 A5", 8, Code.EVEX_Vcmpps_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, 0xA5 };
				yield return new object[] { "62 F14C3D C2 50 01 A5", 8, Code.EVEX_Vcmpps_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, 0xA5 };
				yield return new object[] { "62 F14C28 C2 50 01 A5", 8, Code.EVEX_Vcmpps_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, 0xA5 };

				yield return new object[] { "62 F14C4B C2 50 01 A5", 8, Code.EVEX_Vcmpps_k_k1_zmm_zmmm512b32_imm8_sae, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, 0xA5 };
				yield return new object[] { "62 F14C5D C2 50 01 A5", 8, Code.EVEX_Vcmpps_k_k1_zmm_zmmm512b32_imm8_sae, Register.K2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, 0xA5 };
				yield return new object[] { "62 F14C48 C2 50 01 A5", 8, Code.EVEX_Vcmpps_k_k1_zmm_zmmm512b32_imm8_sae, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, 0xA5 };

				yield return new object[] { "62 F1CD0B C2 50 01 A5", 8, Code.EVEX_Vcmppd_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, 0xA5 };
				yield return new object[] { "62 F1CD1D C2 50 01 A5", 8, Code.EVEX_Vcmppd_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, 0xA5 };
				yield return new object[] { "62 F1CD08 C2 50 01 A5", 8, Code.EVEX_Vcmppd_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, 0xA5 };

				yield return new object[] { "62 F1CD2B C2 50 01 A5", 8, Code.EVEX_Vcmppd_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, 0xA5 };
				yield return new object[] { "62 F1CD3D C2 50 01 A5", 8, Code.EVEX_Vcmppd_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, 0xA5 };
				yield return new object[] { "62 F1CD28 C2 50 01 A5", 8, Code.EVEX_Vcmppd_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, 0xA5 };

				yield return new object[] { "62 F1CD4B C2 50 01 A5", 8, Code.EVEX_Vcmppd_k_k1_zmm_zmmm512b64_imm8_sae, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, 0xA5 };
				yield return new object[] { "62 F1CD5D C2 50 01 A5", 8, Code.EVEX_Vcmppd_k_k1_zmm_zmmm512b64_imm8_sae, Register.K2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, 0xA5 };
				yield return new object[] { "62 F1CD48 C2 50 01 A5", 8, Code.EVEX_Vcmppd_k_k1_zmm_zmmm512b64_imm8_sae, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, 0xA5 };

				yield return new object[] { "62 F14E0B C2 50 01 A5", 8, Code.EVEX_Vcmpss_k_k1_xmm_xmmm32_imm8_sae, Register.K2, Register.XMM6, Register.K3, MemorySize.Float32, 4, 0xA5 };
				yield return new object[] { "62 F14E08 C2 50 01 A5", 8, Code.EVEX_Vcmpss_k_k1_xmm_xmmm32_imm8_sae, Register.K2, Register.XMM6, Register.None, MemorySize.Float32, 4, 0xA5 };
				yield return new object[] { "62 F14E28 C2 50 01 A5", 8, Code.EVEX_Vcmpss_k_k1_xmm_xmmm32_imm8_sae, Register.K2, Register.XMM6, Register.None, MemorySize.Float32, 4, 0xA5 };
				yield return new object[] { "62 F14E48 C2 50 01 A5", 8, Code.EVEX_Vcmpss_k_k1_xmm_xmmm32_imm8_sae, Register.K2, Register.XMM6, Register.None, MemorySize.Float32, 4, 0xA5 };
				yield return new object[] { "62 F14E68 C2 50 01 A5", 8, Code.EVEX_Vcmpss_k_k1_xmm_xmmm32_imm8_sae, Register.K2, Register.XMM6, Register.None, MemorySize.Float32, 4, 0xA5 };

				yield return new object[] { "62 F1CF0B C2 50 01 A5", 8, Code.EVEX_Vcmpsd_k_k1_xmm_xmmm64_imm8_sae, Register.K2, Register.XMM6, Register.K3, MemorySize.Float64, 8, 0xA5 };
				yield return new object[] { "62 F1CF08 C2 50 01 A5", 8, Code.EVEX_Vcmpsd_k_k1_xmm_xmmm64_imm8_sae, Register.K2, Register.XMM6, Register.None, MemorySize.Float64, 8, 0xA5 };
				yield return new object[] { "62 F1CF28 C2 50 01 A5", 8, Code.EVEX_Vcmpsd_k_k1_xmm_xmmm64_imm8_sae, Register.K2, Register.XMM6, Register.None, MemorySize.Float64, 8, 0xA5 };
				yield return new object[] { "62 F1CF48 C2 50 01 A5", 8, Code.EVEX_Vcmpsd_k_k1_xmm_xmmm64_imm8_sae, Register.K2, Register.XMM6, Register.None, MemorySize.Float64, 8, 0xA5 };
				yield return new object[] { "62 F1CF68 C2 50 01 A5", 8, Code.EVEX_Vcmpsd_k_k1_xmm_xmmm64_imm8_sae, Register.K2, Register.XMM6, Register.None, MemorySize.Float64, 8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VcmpV_k1_k2_HX_WX_Ib_2_Data))]
		void Test64_VcmpV_k1_k2_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool sae, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_VcmpV_k1_k2_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F14C0B C2 D3 A5", 7, Code.EVEX_Vcmpps_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F10C0B C2 D3 A5", 7, Code.EVEX_Vcmpps_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM14, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 914C03 C2 D3 A5", 7, Code.EVEX_Vcmpps_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM22, Register.XMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B14C0B C2 D3 A5", 7, Code.EVEX_Vcmpps_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM6, Register.XMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F14C5B C2 D3 A5", 7, Code.EVEX_Vcmpps_k_k1_zmm_zmmm512b32_imm8_sae, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F14C2B C2 D3 A5", 7, Code.EVEX_Vcmpps_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F10C2B C2 D3 A5", 7, Code.EVEX_Vcmpps_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM14, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 914C23 C2 D3 A5", 7, Code.EVEX_Vcmpps_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM22, Register.YMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B14C2B C2 D3 A5", 7, Code.EVEX_Vcmpps_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM6, Register.YMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F14C1B C2 D3 A5", 7, Code.EVEX_Vcmpps_k_k1_zmm_zmmm512b32_imm8_sae, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F14C4B C2 D3 A5", 7, Code.EVEX_Vcmpps_k_k1_zmm_zmmm512b32_imm8_sae, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F10C4B C2 D3 A5", 7, Code.EVEX_Vcmpps_k_k1_zmm_zmmm512b32_imm8_sae, Register.K2, Register.ZMM14, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 914C43 C2 D3 A5", 7, Code.EVEX_Vcmpps_k_k1_zmm_zmmm512b32_imm8_sae, Register.K2, Register.ZMM22, Register.ZMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B14C4B C2 D3 A5", 7, Code.EVEX_Vcmpps_k_k1_zmm_zmmm512b32_imm8_sae, Register.K2, Register.ZMM6, Register.ZMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F14C3B C2 D3 A5", 7, Code.EVEX_Vcmpps_k_k1_zmm_zmmm512b32_imm8_sae, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F1CD0B C2 D3 A5", 7, Code.EVEX_Vcmppd_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F18D0B C2 D3 A5", 7, Code.EVEX_Vcmppd_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM14, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 91CD03 C2 D3 A5", 7, Code.EVEX_Vcmppd_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM22, Register.XMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B1CD0B C2 D3 A5", 7, Code.EVEX_Vcmppd_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM6, Register.XMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F1CD5B C2 D3 A5", 7, Code.EVEX_Vcmppd_k_k1_zmm_zmmm512b64_imm8_sae, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F1CD2B C2 D3 A5", 7, Code.EVEX_Vcmppd_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F18D2B C2 D3 A5", 7, Code.EVEX_Vcmppd_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM14, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 91CD23 C2 D3 A5", 7, Code.EVEX_Vcmppd_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM22, Register.YMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B1CD2B C2 D3 A5", 7, Code.EVEX_Vcmppd_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM6, Register.YMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F1CD1B C2 D3 A5", 7, Code.EVEX_Vcmppd_k_k1_zmm_zmmm512b64_imm8_sae, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F1CD4B C2 D3 A5", 7, Code.EVEX_Vcmppd_k_k1_zmm_zmmm512b64_imm8_sae, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F18D4B C2 D3 A5", 7, Code.EVEX_Vcmppd_k_k1_zmm_zmmm512b64_imm8_sae, Register.K2, Register.ZMM14, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 91CD43 C2 D3 A5", 7, Code.EVEX_Vcmppd_k_k1_zmm_zmmm512b64_imm8_sae, Register.K2, Register.ZMM22, Register.ZMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B1CD4B C2 D3 A5", 7, Code.EVEX_Vcmppd_k_k1_zmm_zmmm512b64_imm8_sae, Register.K2, Register.ZMM6, Register.ZMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F1CD7B C2 D3 A5", 7, Code.EVEX_Vcmppd_k_k1_zmm_zmmm512b64_imm8_sae, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F14E0B C2 D3 A5", 7, Code.EVEX_Vcmpss_k_k1_xmm_xmmm32_imm8_sae, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F10E0B C2 D3 A5", 7, Code.EVEX_Vcmpss_k_k1_xmm_xmmm32_imm8_sae, Register.K2, Register.XMM14, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 914E03 C2 D3 A5", 7, Code.EVEX_Vcmpss_k_k1_xmm_xmmm32_imm8_sae, Register.K2, Register.XMM22, Register.XMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B14E0B C2 D3 A5", 7, Code.EVEX_Vcmpss_k_k1_xmm_xmmm32_imm8_sae, Register.K2, Register.XMM6, Register.XMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F14E5B C2 D3 A5", 7, Code.EVEX_Vcmpss_k_k1_xmm_xmmm32_imm8_sae, Register.K2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F1CF0B C2 D3 A5", 7, Code.EVEX_Vcmpsd_k_k1_xmm_xmmm64_imm8_sae, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F18F0B C2 D3 A5", 7, Code.EVEX_Vcmpsd_k_k1_xmm_xmmm64_imm8_sae, Register.K2, Register.XMM14, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 91CF03 C2 D3 A5", 7, Code.EVEX_Vcmpsd_k_k1_xmm_xmmm64_imm8_sae, Register.K2, Register.XMM22, Register.XMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B1CF0B C2 D3 A5", 7, Code.EVEX_Vcmpsd_k_k1_xmm_xmmm64_imm8_sae, Register.K2, Register.XMM6, Register.XMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F1CF7B C2 D3 A5", 7, Code.EVEX_Vcmpsd_k_k1_xmm_xmmm64_imm8_sae, Register.K2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };
			}
		}

		[Fact]
		void Test16_Movnti_m32_r32_1() {
			var decoder = CreateDecoder16("0FC3 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Movnti_m32_r32, instr.Code);
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
		void Test32_Movnti_m32_r32_1() {
			var decoder = CreateDecoder32("0FC3 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Movnti_m32_r32, instr.Code);
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

		[Fact]
		void Test64_Movnti_m32_r32_1() {
			var decoder = CreateDecoder64("0FC3 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Movnti_m32_r32, instr.Code);
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

		[Fact]
		void Test64_Movnti_m64_r64_1() {
			var decoder = CreateDecoder64("48 0FC3 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Movnti_m64_r64, instr.Code);
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

		[Theory]
		[MemberData(nameof(Test16_PinsrV_VX_GvMw_Ib_1_Data))]
		void Test16_PinsrV_VX_GvMw_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_PinsrV_VX_GvMw_Ib_1_Data {
			get {
				yield return new object[] { "0FC4 08 A5", 4, Code.Pinsrw_mm_r32m16_imm8, Register.MM1, MemorySize.UInt16, 0xA5 };

				yield return new object[] { "66 0FC4 08 A5", 5, Code.Pinsrw_xmm_r32m16_imm8, Register.XMM1, MemorySize.UInt16, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PinsrV_VX_GvMw_Ib_2_Data))]
		void Test16_PinsrV_VX_GvMw_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_PinsrV_VX_GvMw_Ib_2_Data {
			get {
				yield return new object[] { "0FC4 CD A5", 4, Code.Pinsrw_mm_r32m16_imm8, Register.MM1, Register.EBP, 0xA5 };

				yield return new object[] { "66 0FC4 CD A5", 5, Code.Pinsrw_xmm_r32m16_imm8, Register.XMM1, Register.EBP, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PinsrV_VX_GvMw_Ib_1_Data))]
		void Test32_PinsrV_VX_GvMw_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_PinsrV_VX_GvMw_Ib_1_Data {
			get {
				yield return new object[] { "0FC4 08 A5", 4, Code.Pinsrw_mm_r32m16_imm8, Register.MM1, MemorySize.UInt16, 0xA5 };

				yield return new object[] { "66 0FC4 08 A5", 5, Code.Pinsrw_xmm_r32m16_imm8, Register.XMM1, MemorySize.UInt16, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PinsrV_VX_GvMw_Ib_2_Data))]
		void Test32_PinsrV_VX_GvMw_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_PinsrV_VX_GvMw_Ib_2_Data {
			get {
				yield return new object[] { "0FC4 CD A5", 4, Code.Pinsrw_mm_r32m16_imm8, Register.MM1, Register.EBP, 0xA5 };

				yield return new object[] { "66 0FC4 CD A5", 5, Code.Pinsrw_xmm_r32m16_imm8, Register.XMM1, Register.EBP, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PinsrV_VX_GvMw_Ib_1_Data))]
		void Test64_PinsrV_VX_GvMw_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_PinsrV_VX_GvMw_Ib_1_Data {
			get {
				yield return new object[] { "0FC4 08 A5", 4, Code.Pinsrw_mm_r32m16_imm8, Register.MM1, MemorySize.UInt16, 0xA5 };

				yield return new object[] { "48 0FC4 08 A5", 5, Code.Pinsrw_mm_r64m16_imm8, Register.MM1, MemorySize.UInt16, 0xA5 };

				yield return new object[] { "66 0FC4 08 A5", 5, Code.Pinsrw_xmm_r32m16_imm8, Register.XMM1, MemorySize.UInt16, 0xA5 };

				yield return new object[] { "66 48 0FC4 08 A5", 6, Code.Pinsrw_xmm_r64m16_imm8, Register.XMM1, MemorySize.UInt16, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PinsrV_VX_GvMw_Ib_2_Data))]
		void Test64_PinsrV_VX_GvMw_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_PinsrV_VX_GvMw_Ib_2_Data {
			get {
				yield return new object[] { "0FC4 CD A5", 4, Code.Pinsrw_mm_r32m16_imm8, Register.MM1, Register.EBP, 0xA5 };
				yield return new object[] { "41 0FC4 CD A5", 5, Code.Pinsrw_mm_r32m16_imm8, Register.MM1, Register.R13D, 0xA5 };

				yield return new object[] { "48 0FC4 CD A5", 5, Code.Pinsrw_mm_r64m16_imm8, Register.MM1, Register.RBP, 0xA5 };
				yield return new object[] { "49 0FC4 CD A5", 5, Code.Pinsrw_mm_r64m16_imm8, Register.MM1, Register.R13, 0xA5 };

				yield return new object[] { "66 0FC4 CD A5", 5, Code.Pinsrw_xmm_r32m16_imm8, Register.XMM1, Register.EBP, 0xA5 };
				yield return new object[] { "66 44 0FC4 CD A5", 6, Code.Pinsrw_xmm_r32m16_imm8, Register.XMM9, Register.EBP, 0xA5 };
				yield return new object[] { "66 41 0FC4 CD A5", 6, Code.Pinsrw_xmm_r32m16_imm8, Register.XMM1, Register.R13D, 0xA5 };
				yield return new object[] { "66 45 0FC4 CD A5", 6, Code.Pinsrw_xmm_r32m16_imm8, Register.XMM9, Register.R13D, 0xA5 };

				yield return new object[] { "66 48 0FC4 CD A5", 6, Code.Pinsrw_xmm_r64m16_imm8, Register.XMM1, Register.RBP, 0xA5 };
				yield return new object[] { "66 4C 0FC4 CD A5", 6, Code.Pinsrw_xmm_r64m16_imm8, Register.XMM9, Register.RBP, 0xA5 };
				yield return new object[] { "66 49 0FC4 CD A5", 6, Code.Pinsrw_xmm_r64m16_imm8, Register.XMM1, Register.R13, 0xA5 };
				yield return new object[] { "66 4D 0FC4 CD A5", 6, Code.Pinsrw_xmm_r64m16_imm8, Register.XMM9, Register.R13, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PinsrV_VEX_VX_HX_GvMw_Ib_1_Data))]
		void Test16_PinsrV_VEX_VX_HX_GvMw_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_PinsrV_VEX_VX_HX_GvMw_Ib_1_Data {
			get {
				yield return new object[] { "C5C9 C4 10 A5", 5, Code.VEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM2, Register.XMM6, MemorySize.UInt16, 0xA5 };

				yield return new object[] { "C4E1C9 C4 10 A5", 6, Code.VEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM2, Register.XMM6, MemorySize.UInt16, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PinsrV_VEX_VX_HX_GvMw_Ib_2_Data))]
		void Test16_PinsrV_VEX_VX_HX_GvMw_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_PinsrV_VEX_VX_HX_GvMw_Ib_2_Data {
			get {
				yield return new object[] { "C5C9 C4 D3 A5", 5, Code.VEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM2, Register.XMM6, Register.EBX, 0xA5 };

				yield return new object[] { "C4E1C9 C4 D3 A5", 6, Code.VEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM2, Register.XMM6, Register.EBX, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PinsrV_VEX_VX_HX_GvMw_Ib_1_Data))]
		void Test32_PinsrV_VEX_VX_HX_GvMw_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_PinsrV_VEX_VX_HX_GvMw_Ib_1_Data {
			get {
				yield return new object[] { "C5C9 C4 10 A5", 5, Code.VEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM2, Register.XMM6, MemorySize.UInt16, 0xA5 };

				yield return new object[] { "C4E1C9 C4 10 A5", 6, Code.VEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM2, Register.XMM6, MemorySize.UInt16, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PinsrV_VEX_VX_HX_GvMw_Ib_2_Data))]
		void Test32_PinsrV_VEX_VX_HX_GvMw_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_PinsrV_VEX_VX_HX_GvMw_Ib_2_Data {
			get {
				yield return new object[] { "C5C9 C4 D3 A5", 5, Code.VEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM2, Register.XMM6, Register.EBX, 0xA5 };

				yield return new object[] { "C4E1C9 C4 D3 A5", 6, Code.VEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM2, Register.XMM6, Register.EBX, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PinsrV_VEX_VX_HX_GvMw_Ib_1_Data))]
		void Test64_PinsrV_VEX_VX_HX_GvMw_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_PinsrV_VEX_VX_HX_GvMw_Ib_1_Data {
			get {
				yield return new object[] { "C5C9 C4 10 A5", 5, Code.VEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM2, Register.XMM6, MemorySize.UInt16, 0xA5 };

				yield return new object[] { "C4E1C9 C4 10 A5", 6, Code.VEX_Vpinsrw_xmm_xmm_r64m16_imm8, Register.XMM2, Register.XMM6, MemorySize.UInt16, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PinsrV_VEX_VX_HX_GvMw_Ib_2_Data))]
		void Test64_PinsrV_VEX_VX_HX_GvMw_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_PinsrV_VEX_VX_HX_GvMw_Ib_2_Data {
			get {
				yield return new object[] { "C5C9 C4 D3 A5", 5, Code.VEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM2, Register.XMM6, Register.EBX, 0xA5 };
				yield return new object[] { "C509 C4 D3 A5", 5, Code.VEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM10, Register.XMM14, Register.EBX, 0xA5 };
				yield return new object[] { "C4C149 C4 D3 A5", 6, Code.VEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM2, Register.XMM6, Register.R11D, 0xA5 };

				yield return new object[] { "C4E1C9 C4 D3 A5", 6, Code.VEX_Vpinsrw_xmm_xmm_r64m16_imm8, Register.XMM2, Register.XMM6, Register.RBX, 0xA5 };
				yield return new object[] { "C46189 C4 D3 A5", 6, Code.VEX_Vpinsrw_xmm_xmm_r64m16_imm8, Register.XMM10, Register.XMM14, Register.RBX, 0xA5 };
				yield return new object[] { "C4C1C9 C4 D3 A5", 6, Code.VEX_Vpinsrw_xmm_xmm_r64m16_imm8, Register.XMM2, Register.XMM6, Register.R11, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpinsrwV_EVEX_VX_HX_GvMw_Ib_1_Data))]
		void Test16_VpinsrwV_EVEX_VX_HX_GvMw_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpinsrwV_EVEX_VX_HX_GvMw_Ib_1_Data {
			get {
				yield return new object[] { "62 F14D08 C4 50 01 A5", 8, Code.EVEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt16, 2, false, 0xA5 };

				yield return new object[] { "62 F1CD08 C4 50 01 A5", 8, Code.EVEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt16, 2, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpinsrwV_EVEX_VX_HX_GvMw_Ib_2_Data))]
		void Test16_VpinsrwV_EVEX_VX_HX_GvMw_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpinsrwV_EVEX_VX_HX_GvMw_Ib_2_Data {
			get {
				yield return new object[] { "62 F14D08 C4 D3 A5", 7, Code.EVEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F1CD08 C4 D3 A5", 7, Code.EVEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpinsrwV_EVEX_VX_HX_GvMw_Ib_1_Data))]
		void Test32_VpinsrwV_EVEX_VX_HX_GvMw_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpinsrwV_EVEX_VX_HX_GvMw_Ib_1_Data {
			get {
				yield return new object[] { "62 F14D08 C4 50 01 A5", 8, Code.EVEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt16, 2, false, 0xA5 };

				yield return new object[] { "62 F1CD08 C4 50 01 A5", 8, Code.EVEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt16, 2, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpinsrwV_EVEX_VX_HX_GvMw_Ib_2_Data))]
		void Test32_VpinsrwV_EVEX_VX_HX_GvMw_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpinsrwV_EVEX_VX_HX_GvMw_Ib_2_Data {
			get {
				yield return new object[] { "62 F14D08 C4 D3 A5", 7, Code.EVEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F1CD08 C4 D3 A5", 7, Code.EVEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpinsrwV_EVEX_VX_HX_GvMw_Ib_1_Data))]
		void Test64_VpinsrwV_EVEX_VX_HX_GvMw_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpinsrwV_EVEX_VX_HX_GvMw_Ib_1_Data {
			get {
				yield return new object[] { "62 F14D08 C4 50 01 A5", 8, Code.EVEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt16, 2, false, 0xA5 };

				yield return new object[] { "62 F1CD08 C4 50 01 A5", 8, Code.EVEX_Vpinsrw_xmm_xmm_r64m16_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt16, 2, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpinsrwV_EVEX_VX_HX_GvMw_Ib_2_Data))]
		void Test64_VpinsrwV_EVEX_VX_HX_GvMw_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpinsrwV_EVEX_VX_HX_GvMw_Ib_2_Data {
			get {
				yield return new object[] { "62 F14D08 C4 D3 A5", 7, Code.EVEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 E10D08 C4 D3 A5", 7, Code.EVEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM18, Register.XMM14, Register.EBX, Register.None, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 514D00 C4 D3 A5", 7, Code.EVEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM10, Register.XMM22, Register.R11D, Register.None, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 D14D08 C4 D3 A5", 7, Code.EVEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM2, Register.XMM6, Register.R11D, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F1CD08 C4 D3 A5", 7, Code.EVEX_Vpinsrw_xmm_xmm_r64m16_imm8, Register.XMM2, Register.XMM6, Register.RBX, Register.None, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 E18D08 C4 D3 A5", 7, Code.EVEX_Vpinsrw_xmm_xmm_r64m16_imm8, Register.XMM18, Register.XMM14, Register.RBX, Register.None, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 51CD00 C4 D3 A5", 7, Code.EVEX_Vpinsrw_xmm_xmm_r64m16_imm8, Register.XMM10, Register.XMM22, Register.R11, Register.None, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 D1CD08 C4 D3 A5", 7, Code.EVEX_Vpinsrw_xmm_xmm_r64m16_imm8, Register.XMM2, Register.XMM6, Register.R11, Register.None, RoundingControl.None, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PextrV_Gd_RX_Ib_2_Data))]
		void Test16_PextrV_Gd_RX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_PextrV_Gd_RX_Ib_2_Data {
			get {
				yield return new object[] { "0FC5 CD A5", 4, Code.Pextrw_r32_mm_imm8, Register.ECX, Register.MM5, 0xA5 };

				yield return new object[] { "66 0FC5 CD A5", 5, Code.Pextrw_r32_xmm_imm8, Register.ECX, Register.XMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PextrV_Gd_RX_Ib_2_Data))]
		void Test32_PextrV_Gd_RX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_PextrV_Gd_RX_Ib_2_Data {
			get {
				yield return new object[] { "0FC5 CD A5", 4, Code.Pextrw_r32_mm_imm8, Register.ECX, Register.MM5, 0xA5 };

				yield return new object[] { "66 0FC5 CD A5", 5, Code.Pextrw_r32_xmm_imm8, Register.ECX, Register.XMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PextrV_Gd_RX_Ib_2_Data))]
		void Test64_PextrV_Gd_RX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_PextrV_Gd_RX_Ib_2_Data {
			get {
				yield return new object[] { "0FC5 CD A5", 4, Code.Pextrw_r32_mm_imm8, Register.ECX, Register.MM5, 0xA5 };
				yield return new object[] { "44 0FC5 CD A5", 5, Code.Pextrw_r32_mm_imm8, Register.R9D, Register.MM5, 0xA5 };

				yield return new object[] { "48 0FC5 CD A5", 5, Code.Pextrw_r64_mm_imm8, Register.RCX, Register.MM5, 0xA5 };
				yield return new object[] { "4C 0FC5 CD A5", 5, Code.Pextrw_r64_mm_imm8, Register.R9, Register.MM5, 0xA5 };

				yield return new object[] { "66 0FC5 CD A5", 5, Code.Pextrw_r32_xmm_imm8, Register.ECX, Register.XMM5, 0xA5 };
				yield return new object[] { "66 44 0FC5 CD A5", 6, Code.Pextrw_r32_xmm_imm8, Register.R9D, Register.XMM5, 0xA5 };
				yield return new object[] { "66 41 0FC5 CD A5", 6, Code.Pextrw_r32_xmm_imm8, Register.ECX, Register.XMM13, 0xA5 };
				yield return new object[] { "66 45 0FC5 CD A5", 6, Code.Pextrw_r32_xmm_imm8, Register.R9D, Register.XMM13, 0xA5 };

				yield return new object[] { "66 48 0FC5 CD A5", 6, Code.Pextrw_r64_xmm_imm8, Register.RCX, Register.XMM5, 0xA5 };
				yield return new object[] { "66 4C 0FC5 CD A5", 6, Code.Pextrw_r64_xmm_imm8, Register.R9, Register.XMM5, 0xA5 };
				yield return new object[] { "66 49 0FC5 CD A5", 6, Code.Pextrw_r64_xmm_imm8, Register.RCX, Register.XMM13, 0xA5 };
				yield return new object[] { "66 4D 0FC5 CD A5", 6, Code.Pextrw_r64_xmm_imm8, Register.R9, Register.XMM13, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PextrV_VEX_Gd_RX_Ib_2_Data))]
		void Test16_PextrV_VEX_Gd_RX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_PextrV_VEX_Gd_RX_Ib_2_Data {
			get {
				yield return new object[] { "C5F9 C5 D3 A5", 5, Code.VEX_Vpextrw_r32_xmm_imm8, Register.EDX, Register.XMM3, 0xA5 };

				yield return new object[] { "C4E1F9 C5 D3 A5", 6, Code.VEX_Vpextrw_r32_xmm_imm8, Register.EDX, Register.XMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PextrV_VEX_Gd_RX_Ib_2_Data))]
		void Test32_PextrV_VEX_Gd_RX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_PextrV_VEX_Gd_RX_Ib_2_Data {
			get {
				yield return new object[] { "C5F9 C5 D3 A5", 5, Code.VEX_Vpextrw_r32_xmm_imm8, Register.EDX, Register.XMM3, 0xA5 };

				yield return new object[] { "C4E1F9 C5 D3 A5", 6, Code.VEX_Vpextrw_r32_xmm_imm8, Register.EDX, Register.XMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PextrV_VEX_Gd_RX_Ib_2_Data))]
		void Test64_PextrV_VEX_Gd_RX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_PextrV_VEX_Gd_RX_Ib_2_Data {
			get {
				yield return new object[] { "C5F9 C5 D3 A5", 5, Code.VEX_Vpextrw_r32_xmm_imm8, Register.EDX, Register.XMM3, 0xA5 };
				yield return new object[] { "C579 C5 D3 A5", 5, Code.VEX_Vpextrw_r32_xmm_imm8, Register.R10D, Register.XMM3, 0xA5 };
				yield return new object[] { "C4C179 C5 D3 A5", 6, Code.VEX_Vpextrw_r32_xmm_imm8, Register.EDX, Register.XMM11, 0xA5 };

				yield return new object[] { "C4E1F9 C5 D3 A5", 6, Code.VEX_Vpextrw_r64_xmm_imm8, Register.RDX, Register.XMM3, 0xA5 };
				yield return new object[] { "C461F9 C5 D3 A5", 6, Code.VEX_Vpextrw_r64_xmm_imm8, Register.R10, Register.XMM3, 0xA5 };
				yield return new object[] { "C4C1F9 C5 D3 A5", 6, Code.VEX_Vpextrw_r64_xmm_imm8, Register.RDX, Register.XMM11, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpextrwV_EVEX_Gd_RX_Ib_2_Data))]
		void Test16_VpextrwV_EVEX_Gd_RX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpextrwV_EVEX_Gd_RX_Ib_2_Data {
			get {
				yield return new object[] { "62 F17D08 C5 D3 A5", 7, Code.EVEX_Vpextrw_r32_xmm_imm8, Register.EDX, Register.XMM3, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F1FD08 C5 D3 A5", 7, Code.EVEX_Vpextrw_r32_xmm_imm8, Register.EDX, Register.XMM3, Register.None, RoundingControl.None, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpextrwV_EVEX_Gd_RX_Ib_2_Data))]
		void Test32_VpextrwV_EVEX_Gd_RX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpextrwV_EVEX_Gd_RX_Ib_2_Data {
			get {
				yield return new object[] { "62 F17D08 C5 D3 A5", 7, Code.EVEX_Vpextrw_r32_xmm_imm8, Register.EDX, Register.XMM3, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F1FD08 C5 D3 A5", 7, Code.EVEX_Vpextrw_r32_xmm_imm8, Register.EDX, Register.XMM3, Register.None, RoundingControl.None, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpextrwV_EVEX_Gd_RX_Ib_2_Data))]
		void Test64_VpextrwV_EVEX_Gd_RX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpextrwV_EVEX_Gd_RX_Ib_2_Data {
			get {
				yield return new object[] { "62 F17D08 C5 D3 A5", 7, Code.EVEX_Vpextrw_r32_xmm_imm8, Register.EDX, Register.XMM3, Register.None, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 117D08 C5 D3 A5", 7, Code.EVEX_Vpextrw_r32_xmm_imm8, Register.R10D, Register.XMM27, Register.None, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 B17D08 C5 D3 A5", 7, Code.EVEX_Vpextrw_r32_xmm_imm8, Register.EDX, Register.XMM19, Register.None, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 517D08 C5 D3 A5", 7, Code.EVEX_Vpextrw_r32_xmm_imm8, Register.R10D, Register.XMM11, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F1FD08 C5 D3 A5", 7, Code.EVEX_Vpextrw_r64_xmm_imm8, Register.RDX, Register.XMM3, Register.None, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 11FD08 C5 D3 A5", 7, Code.EVEX_Vpextrw_r64_xmm_imm8, Register.R10, Register.XMM27, Register.None, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 B1FD08 C5 D3 A5", 7, Code.EVEX_Vpextrw_r64_xmm_imm8, Register.RDX, Register.XMM19, Register.None, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 51FD08 C5 D3 A5", 7, Code.EVEX_Vpextrw_r64_xmm_imm8, Register.R10, Register.XMM11, Register.None, RoundingControl.None, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_ShufV_VX_WX_Ib_1_Data))]
		void Test16_ShufV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_ShufV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "0FC6 08 A5", 4, Code.Shufps_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Float32, 0xA5 };

				yield return new object[] { "66 0FC6 08 A5", 5, Code.Shufpd_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Float64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_ShufV_VX_WX_Ib_2_Data))]
		void Test16_ShufV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_ShufV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "0FC6 CD A5", 4, Code.Shufps_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };

				yield return new object[] { "66 0FC6 CD A5", 5, Code.Shufpd_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_ShufV_VX_WX_Ib_1_Data))]
		void Test32_ShufV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_ShufV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "0FC6 08 A5", 4, Code.Shufps_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Float32, 0xA5 };

				yield return new object[] { "66 0FC6 08 A5", 5, Code.Shufpd_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Float64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_ShufV_VX_WX_Ib_2_Data))]
		void Test32_ShufV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_ShufV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "0FC6 CD A5", 4, Code.Shufps_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };

				yield return new object[] { "66 0FC6 CD A5", 5, Code.Shufpd_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_ShufV_VX_WX_Ib_1_Data))]
		void Test64_ShufV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_ShufV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "0FC6 08 A5", 4, Code.Shufps_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Float32, 0xA5 };

				yield return new object[] { "66 0FC6 08 A5", 5, Code.Shufpd_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Float64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_ShufV_VX_WX_Ib_2_Data))]
		void Test64_ShufV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_ShufV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "0FC6 CD A5", 4, Code.Shufps_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
				yield return new object[] { "44 0FC6 CD A5", 5, Code.Shufps_xmm_xmmm128_imm8, Register.XMM9, Register.XMM5, 0xA5 };
				yield return new object[] { "41 0FC6 CD A5", 5, Code.Shufps_xmm_xmmm128_imm8, Register.XMM1, Register.XMM13, 0xA5 };
				yield return new object[] { "45 0FC6 CD A5", 5, Code.Shufps_xmm_xmmm128_imm8, Register.XMM9, Register.XMM13, 0xA5 };

				yield return new object[] { "66 0FC6 CD A5", 5, Code.Shufpd_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
				yield return new object[] { "66 44 0FC6 CD A5", 6, Code.Shufpd_xmm_xmmm128_imm8, Register.XMM9, Register.XMM5, 0xA5 };
				yield return new object[] { "66 41 0FC6 CD A5", 6, Code.Shufpd_xmm_xmmm128_imm8, Register.XMM1, Register.XMM13, 0xA5 };
				yield return new object[] { "66 45 0FC6 CD A5", 6, Code.Shufpd_xmm_xmmm128_imm8, Register.XMM9, Register.XMM13, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VshufV_VX_HX_WX_Ib_1_Data))]
		void Test16_VshufV_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_VshufV_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C5C8 C6 10 A5", 5, Code.VEX_Vshufps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, 0xA5 };
				yield return new object[] { "C5CC C6 10 A5", 5, Code.VEX_Vshufps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, 0xA5 };
				yield return new object[] { "C4E1C8 C6 10 A5", 6, Code.VEX_Vshufps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, 0xA5 };
				yield return new object[] { "C4E1CC C6 10 A5", 6, Code.VEX_Vshufps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, 0xA5 };

				yield return new object[] { "C5C9 C6 10 A5", 5, Code.VEX_Vshufpd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, 0xA5 };
				yield return new object[] { "C5CD C6 10 A5", 5, Code.VEX_Vshufpd_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, 0xA5 };
				yield return new object[] { "C4E1C9 C6 10 A5", 6, Code.VEX_Vshufpd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, 0xA5 };
				yield return new object[] { "C4E1CD C6 10 A5", 6, Code.VEX_Vshufpd_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VshufV_VX_HX_WX_Ib_2_Data))]
		void Test16_VshufV_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_VshufV_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C5C8 C6 D3 A5", 5, Code.VEX_Vshufps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C5CC C6 D3 A5", 5, Code.VEX_Vshufps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5 };

				yield return new object[] { "C5C9 C6 D3 A5", 5, Code.VEX_Vshufpd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C5CD C6 D3 A5", 5, Code.VEX_Vshufpd_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VshufV_VX_HX_WX_Ib_1_Data))]
		void Test32_VshufV_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_VshufV_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C5C8 C6 10 A5", 5, Code.VEX_Vshufps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, 0xA5 };
				yield return new object[] { "C5CC C6 10 A5", 5, Code.VEX_Vshufps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, 0xA5 };
				yield return new object[] { "C4E1C8 C6 10 A5", 6, Code.VEX_Vshufps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, 0xA5 };
				yield return new object[] { "C4E1CC C6 10 A5", 6, Code.VEX_Vshufps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, 0xA5 };

				yield return new object[] { "C5C9 C6 10 A5", 5, Code.VEX_Vshufpd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, 0xA5 };
				yield return new object[] { "C5CD C6 10 A5", 5, Code.VEX_Vshufpd_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, 0xA5 };
				yield return new object[] { "C4E1C9 C6 10 A5", 6, Code.VEX_Vshufpd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, 0xA5 };
				yield return new object[] { "C4E1CD C6 10 A5", 6, Code.VEX_Vshufpd_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VshufV_VX_HX_WX_Ib_2_Data))]
		void Test32_VshufV_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_VshufV_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C5C8 C6 D3 A5", 5, Code.VEX_Vshufps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C5CC C6 D3 A5", 5, Code.VEX_Vshufps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5 };

				yield return new object[] { "C5C9 C6 D3 A5", 5, Code.VEX_Vshufpd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C5CD C6 D3 A5", 5, Code.VEX_Vshufpd_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VshufV_VX_HX_WX_Ib_1_Data))]
		void Test64_VshufV_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_VshufV_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C5C8 C6 10 A5", 5, Code.VEX_Vshufps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, 0xA5 };
				yield return new object[] { "C5CC C6 10 A5", 5, Code.VEX_Vshufps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, 0xA5 };
				yield return new object[] { "C4E1C8 C6 10 A5", 6, Code.VEX_Vshufps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, 0xA5 };
				yield return new object[] { "C4E1CC C6 10 A5", 6, Code.VEX_Vshufps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, 0xA5 };

				yield return new object[] { "C5C9 C6 10 A5", 5, Code.VEX_Vshufpd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, 0xA5 };
				yield return new object[] { "C5CD C6 10 A5", 5, Code.VEX_Vshufpd_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, 0xA5 };
				yield return new object[] { "C4E1C9 C6 10 A5", 6, Code.VEX_Vshufpd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, 0xA5 };
				yield return new object[] { "C4E1CD C6 10 A5", 6, Code.VEX_Vshufpd_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VshufV_VX_HX_WX_Ib_2_Data))]
		void Test64_VshufV_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_VshufV_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C5C8 C6 D3 A5", 5, Code.VEX_Vshufps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C5CC C6 D3 A5", 5, Code.VEX_Vshufps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5 };
				yield return new object[] { "C548 C6 D3 A5", 5, Code.VEX_Vshufps_xmm_xmm_xmmm128_imm8, Register.XMM10, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C54C C6 D3 A5", 5, Code.VEX_Vshufps_ymm_ymm_ymmm256_imm8, Register.YMM10, Register.YMM6, Register.YMM3, 0xA5 };
				yield return new object[] { "C588 C6 D3 A5", 5, Code.VEX_Vshufps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM14, Register.XMM3, 0xA5 };
				yield return new object[] { "C58C C6 D3 A5", 5, Code.VEX_Vshufps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM14, Register.YMM3, 0xA5 };
				yield return new object[] { "C4C148 C6 D3 A5", 6, Code.VEX_Vshufps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM11, 0xA5 };
				yield return new object[] { "C4C14C C6 D3 A5", 6, Code.VEX_Vshufps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM11, 0xA5 };

				yield return new object[] { "C5C9 C6 D3 A5", 5, Code.VEX_Vshufpd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C5CD C6 D3 A5", 5, Code.VEX_Vshufpd_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5 };
				yield return new object[] { "C549 C6 D3 A5", 5, Code.VEX_Vshufpd_xmm_xmm_xmmm128_imm8, Register.XMM10, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C54D C6 D3 A5", 5, Code.VEX_Vshufpd_ymm_ymm_ymmm256_imm8, Register.YMM10, Register.YMM6, Register.YMM3, 0xA5 };
				yield return new object[] { "C589 C6 D3 A5", 5, Code.VEX_Vshufpd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM14, Register.XMM3, 0xA5 };
				yield return new object[] { "C58D C6 D3 A5", 5, Code.VEX_Vshufpd_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM14, Register.YMM3, 0xA5 };
				yield return new object[] { "C4C149 C6 D3 A5", 6, Code.VEX_Vshufpd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM11, 0xA5 };
				yield return new object[] { "C4C14D C6 D3 A5", 6, Code.VEX_Vshufpd_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM11, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VshufV_VX_k1_HX_WX_Ib_1_Data))]
		void Test16_VshufV_VX_k1_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VshufV_VX_k1_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F14C0B C6 50 01 A5", 8, Code.EVEX_Vshufps_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false, 0xA5 };
				yield return new object[] { "62 F14C9D C6 50 01 A5", 8, Code.EVEX_Vshufps_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F14C08 C6 50 01 A5", 8, Code.EVEX_Vshufps_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false, 0xA5 };

				yield return new object[] { "62 F14C2B C6 50 01 A5", 8, Code.EVEX_Vshufps_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false, 0xA5 };
				yield return new object[] { "62 F14CBD C6 50 01 A5", 8, Code.EVEX_Vshufps_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F14C28 C6 50 01 A5", 8, Code.EVEX_Vshufps_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false, 0xA5 };

				yield return new object[] { "62 F14C4B C6 50 01 A5", 8, Code.EVEX_Vshufps_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false, 0xA5 };
				yield return new object[] { "62 F14CDD C6 50 01 A5", 8, Code.EVEX_Vshufps_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F14C48 C6 50 01 A5", 8, Code.EVEX_Vshufps_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false, 0xA5 };

				yield return new object[] { "62 F1CD0B C6 50 01 A5", 8, Code.EVEX_Vshufpd_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false, 0xA5 };
				yield return new object[] { "62 F1CD9D C6 50 01 A5", 8, Code.EVEX_Vshufpd_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD08 C6 50 01 A5", 8, Code.EVEX_Vshufpd_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false, 0xA5 };

				yield return new object[] { "62 F1CD2B C6 50 01 A5", 8, Code.EVEX_Vshufpd_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false, 0xA5 };
				yield return new object[] { "62 F1CDBD C6 50 01 A5", 8, Code.EVEX_Vshufpd_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD28 C6 50 01 A5", 8, Code.EVEX_Vshufpd_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false, 0xA5 };

				yield return new object[] { "62 F1CD4B C6 50 01 A5", 8, Code.EVEX_Vshufpd_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false, 0xA5 };
				yield return new object[] { "62 F1CDDD C6 50 01 A5", 8, Code.EVEX_Vshufpd_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD48 C6 50 01 A5", 8, Code.EVEX_Vshufpd_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VshufV_VX_k1_HX_WX_Ib_2_Data))]
		void Test16_VshufV_VX_k1_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VshufV_VX_k1_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F14C0B C6 D3 A5", 7, Code.EVEX_Vshufps_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F14C8B C6 D3 A5", 7, Code.EVEX_Vshufps_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F14C2B C6 D3 A5", 7, Code.EVEX_Vshufps_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F14CAB C6 D3 A5", 7, Code.EVEX_Vshufps_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F14C4B C6 D3 A5", 7, Code.EVEX_Vshufps_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F14CCB C6 D3 A5", 7, Code.EVEX_Vshufps_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F1CD8B C6 D3 A5", 7, Code.EVEX_Vshufpd_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F1CD0B C6 D3 A5", 7, Code.EVEX_Vshufpd_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F1CDAB C6 D3 A5", 7, Code.EVEX_Vshufpd_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F1CD2B C6 D3 A5", 7, Code.EVEX_Vshufpd_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F1CDCB C6 D3 A5", 7, Code.EVEX_Vshufpd_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F1CD4B C6 D3 A5", 7, Code.EVEX_Vshufpd_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VshufV_VX_k1_HX_WX_Ib_1_Data))]
		void Test32_VshufV_VX_k1_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VshufV_VX_k1_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F14C0B C6 50 01 A5", 8, Code.EVEX_Vshufps_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false, 0xA5 };
				yield return new object[] { "62 F14C9D C6 50 01 A5", 8, Code.EVEX_Vshufps_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F14C08 C6 50 01 A5", 8, Code.EVEX_Vshufps_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false, 0xA5 };

				yield return new object[] { "62 F14C2B C6 50 01 A5", 8, Code.EVEX_Vshufps_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false, 0xA5 };
				yield return new object[] { "62 F14CBD C6 50 01 A5", 8, Code.EVEX_Vshufps_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F14C28 C6 50 01 A5", 8, Code.EVEX_Vshufps_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false, 0xA5 };

				yield return new object[] { "62 F14C4B C6 50 01 A5", 8, Code.EVEX_Vshufps_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false, 0xA5 };
				yield return new object[] { "62 F14CDD C6 50 01 A5", 8, Code.EVEX_Vshufps_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F14C48 C6 50 01 A5", 8, Code.EVEX_Vshufps_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false, 0xA5 };

				yield return new object[] { "62 F1CD0B C6 50 01 A5", 8, Code.EVEX_Vshufpd_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false, 0xA5 };
				yield return new object[] { "62 F1CD9D C6 50 01 A5", 8, Code.EVEX_Vshufpd_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD08 C6 50 01 A5", 8, Code.EVEX_Vshufpd_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false, 0xA5 };

				yield return new object[] { "62 F1CD2B C6 50 01 A5", 8, Code.EVEX_Vshufpd_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false, 0xA5 };
				yield return new object[] { "62 F1CDBD C6 50 01 A5", 8, Code.EVEX_Vshufpd_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD28 C6 50 01 A5", 8, Code.EVEX_Vshufpd_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false, 0xA5 };

				yield return new object[] { "62 F1CD4B C6 50 01 A5", 8, Code.EVEX_Vshufpd_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false, 0xA5 };
				yield return new object[] { "62 F1CDDD C6 50 01 A5", 8, Code.EVEX_Vshufpd_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD48 C6 50 01 A5", 8, Code.EVEX_Vshufpd_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VshufV_VX_k1_HX_WX_Ib_2_Data))]
		void Test32_VshufV_VX_k1_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VshufV_VX_k1_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F14C0B C6 D3 A5", 7, Code.EVEX_Vshufps_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F14C8B C6 D3 A5", 7, Code.EVEX_Vshufps_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F14C2B C6 D3 A5", 7, Code.EVEX_Vshufps_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F14CAB C6 D3 A5", 7, Code.EVEX_Vshufps_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F14C4B C6 D3 A5", 7, Code.EVEX_Vshufps_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F14CCB C6 D3 A5", 7, Code.EVEX_Vshufps_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F1CD8B C6 D3 A5", 7, Code.EVEX_Vshufpd_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F1CD0B C6 D3 A5", 7, Code.EVEX_Vshufpd_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F1CDAB C6 D3 A5", 7, Code.EVEX_Vshufpd_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F1CD2B C6 D3 A5", 7, Code.EVEX_Vshufpd_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F1CDCB C6 D3 A5", 7, Code.EVEX_Vshufpd_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F1CD4B C6 D3 A5", 7, Code.EVEX_Vshufpd_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VshufV_VX_k1_HX_WX_Ib_1_Data))]
		void Test64_VshufV_VX_k1_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VshufV_VX_k1_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F14C0B C6 50 01 A5", 8, Code.EVEX_Vshufps_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false, 0xA5 };
				yield return new object[] { "62 F14C9D C6 50 01 A5", 8, Code.EVEX_Vshufps_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F14C08 C6 50 01 A5", 8, Code.EVEX_Vshufps_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false, 0xA5 };

				yield return new object[] { "62 F14C2B C6 50 01 A5", 8, Code.EVEX_Vshufps_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false, 0xA5 };
				yield return new object[] { "62 F14CBD C6 50 01 A5", 8, Code.EVEX_Vshufps_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F14C28 C6 50 01 A5", 8, Code.EVEX_Vshufps_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false, 0xA5 };

				yield return new object[] { "62 F14C4B C6 50 01 A5", 8, Code.EVEX_Vshufps_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false, 0xA5 };
				yield return new object[] { "62 F14CDD C6 50 01 A5", 8, Code.EVEX_Vshufps_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F14C48 C6 50 01 A5", 8, Code.EVEX_Vshufps_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false, 0xA5 };

				yield return new object[] { "62 F1CD0B C6 50 01 A5", 8, Code.EVEX_Vshufpd_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false, 0xA5 };
				yield return new object[] { "62 F1CD9D C6 50 01 A5", 8, Code.EVEX_Vshufpd_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD08 C6 50 01 A5", 8, Code.EVEX_Vshufpd_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false, 0xA5 };

				yield return new object[] { "62 F1CD2B C6 50 01 A5", 8, Code.EVEX_Vshufpd_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false, 0xA5 };
				yield return new object[] { "62 F1CDBD C6 50 01 A5", 8, Code.EVEX_Vshufpd_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD28 C6 50 01 A5", 8, Code.EVEX_Vshufpd_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false, 0xA5 };

				yield return new object[] { "62 F1CD4B C6 50 01 A5", 8, Code.EVEX_Vshufpd_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false, 0xA5 };
				yield return new object[] { "62 F1CDDD C6 50 01 A5", 8, Code.EVEX_Vshufpd_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD48 C6 50 01 A5", 8, Code.EVEX_Vshufpd_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VshufV_VX_k1_HX_WX_Ib_2_Data))]
		void Test64_VshufV_VX_k1_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VshufV_VX_k1_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F14C0B C6 D3 A5", 7, Code.EVEX_Vshufps_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 E10C0B C6 D3 A5", 7, Code.EVEX_Vshufps_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 114C03 C6 D3 A5", 7, Code.EVEX_Vshufps_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B14C0B C6 D3 A5", 7, Code.EVEX_Vshufps_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F14C8B C6 D3 A5", 7, Code.EVEX_Vshufps_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F14C2B C6 D3 A5", 7, Code.EVEX_Vshufps_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 E10C2B C6 D3 A5", 7, Code.EVEX_Vshufps_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 114C23 C6 D3 A5", 7, Code.EVEX_Vshufps_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B14C2B C6 D3 A5", 7, Code.EVEX_Vshufps_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F14CAB C6 D3 A5", 7, Code.EVEX_Vshufps_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F14C4B C6 D3 A5", 7, Code.EVEX_Vshufps_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 E10C4B C6 D3 A5", 7, Code.EVEX_Vshufps_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 114C43 C6 D3 A5", 7, Code.EVEX_Vshufps_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B14C4B C6 D3 A5", 7, Code.EVEX_Vshufps_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F14CCB C6 D3 A5", 7, Code.EVEX_Vshufps_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F1CD8B C6 D3 A5", 7, Code.EVEX_Vshufpd_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 E18D0B C6 D3 A5", 7, Code.EVEX_Vshufpd_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 11CD03 C6 D3 A5", 7, Code.EVEX_Vshufpd_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B1CD0B C6 D3 A5", 7, Code.EVEX_Vshufpd_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F1CD0B C6 D3 A5", 7, Code.EVEX_Vshufpd_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F1CDAB C6 D3 A5", 7, Code.EVEX_Vshufpd_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 E18D2B C6 D3 A5", 7, Code.EVEX_Vshufpd_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 11CD23 C6 D3 A5", 7, Code.EVEX_Vshufpd_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B1CD2B C6 D3 A5", 7, Code.EVEX_Vshufpd_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F1CD2B C6 D3 A5", 7, Code.EVEX_Vshufpd_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F1CDCB C6 D3 A5", 7, Code.EVEX_Vshufpd_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 E18D4B C6 D3 A5", 7, Code.EVEX_Vshufpd_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 11CD43 C6 D3 A5", 7, Code.EVEX_Vshufpd_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B1CD4B C6 D3 A5", 7, Code.EVEX_Vshufpd_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F1CD4B C6 D3 A5", 7, Code.EVEX_Vshufpd_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Grp_RM_1_Data))]
		void Test16_Grp_RM_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
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
		}
		public static IEnumerable<object[]> Test16_Grp_RM_1_Data {
			get {
				yield return new object[] { "0FC7 08", 3, Code.Cmpxchg8b_m64, MemorySize.UInt64 };

				yield return new object[] { "0FC7 18", 3, Code.Xrstors_m0, MemorySize.Xsave };

				yield return new object[] { "0FC7 20", 3, Code.Xsavec_m0, MemorySize.Xsave };

				yield return new object[] { "0FC7 28", 3, Code.Xsaves_m0, MemorySize.Xsave };

				yield return new object[] { "0FC7 30", 3, Code.Vmptrld_m64, MemorySize.UInt64 };

				yield return new object[] { "66 0FC7 30", 4, Code.Vmclear_m64, MemorySize.UInt64 };

				yield return new object[] { "F3 0FC7 30", 4, Code.Vmxon_m64, MemorySize.UInt64 };

				yield return new object[] { "0FC7 38", 3, Code.Vmptrst_m64, MemorySize.UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Grp_RM_2_Data))]
		void Test16_Grp_RM_2(string hexBytes, int byteLength, Code code, Register reg) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);
		}
		public static IEnumerable<object[]> Test16_Grp_RM_2_Data {
			get {
				yield return new object[] { "0FC7 F2", 3, Code.Rdrand_r16, Register.DX };

				yield return new object[] { "66 0FC7 F2", 4, Code.Rdrand_r32, Register.EDX };

				yield return new object[] { "0FC7 FA", 3, Code.Rdseed_r16, Register.DX };

				yield return new object[] { "66 0FC7 FA", 4, Code.Rdseed_r32, Register.EDX };

				yield return new object[] { "F3 0FC7 FA", 4, Code.Rdpid_r32, Register.EDX };

				yield return new object[] { "66 F3 0FC7 FA", 5, Code.Rdpid_r32, Register.EDX };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Grp_RM_1_Data))]
		void Test32_Grp_RM_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
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
		}
		public static IEnumerable<object[]> Test32_Grp_RM_1_Data {
			get {
				yield return new object[] { "0FC7 08", 3, Code.Cmpxchg8b_m64, MemorySize.UInt64 };

				yield return new object[] { "0FC7 18", 3, Code.Xrstors_m0, MemorySize.Xsave };

				yield return new object[] { "0FC7 20", 3, Code.Xsavec_m0, MemorySize.Xsave };

				yield return new object[] { "0FC7 28", 3, Code.Xsaves_m0, MemorySize.Xsave };

				yield return new object[] { "0FC7 30", 3, Code.Vmptrld_m64, MemorySize.UInt64 };

				yield return new object[] { "66 0FC7 30", 4, Code.Vmclear_m64, MemorySize.UInt64 };

				yield return new object[] { "F3 0FC7 30", 4, Code.Vmxon_m64, MemorySize.UInt64 };

				yield return new object[] { "0FC7 38", 3, Code.Vmptrst_m64, MemorySize.UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Grp_RM_2_Data))]
		void Test32_Grp_RM_2(string hexBytes, int byteLength, Code code, Register reg) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);
		}
		public static IEnumerable<object[]> Test32_Grp_RM_2_Data {
			get {
				yield return new object[] { "66 0FC7 F2", 4, Code.Rdrand_r16, Register.DX };

				yield return new object[] { "0FC7 F2", 3, Code.Rdrand_r32, Register.EDX };

				yield return new object[] { "66 0FC7 FA", 4, Code.Rdseed_r16, Register.DX };

				yield return new object[] { "0FC7 FA", 3, Code.Rdseed_r32, Register.EDX };

				yield return new object[] { "66 F3 0FC7 FA", 5, Code.Rdpid_r32, Register.EDX };

				yield return new object[] { "F3 0FC7 FA", 4, Code.Rdpid_r32, Register.EDX };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Grp_RM_1_Data))]
		void Test64_Grp_RM_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
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
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_Grp_RM_1_Data {
			get {
				yield return new object[] { "0FC7 08", 3, Code.Cmpxchg8b_m64, MemorySize.UInt64 };
				yield return new object[] { "66 0FC7 08", 4, Code.Cmpxchg8b_m64, MemorySize.UInt64 };
				yield return new object[] { "48 0FC7 08", 4, Code.Cmpxchg16b_m128, MemorySize.UInt128 };
				yield return new object[] { "66 48 0FC7 08", 5, Code.Cmpxchg16b_m128, MemorySize.UInt128 };

				yield return new object[] { "0FC7 18", 3, Code.Xrstors_m0, MemorySize.Xsave };
				yield return new object[] { "48 0FC7 18", 4, Code.Xrstors64_m0, MemorySize.Xsave64 };

				yield return new object[] { "0FC7 20", 3, Code.Xsavec_m0, MemorySize.Xsave };
				yield return new object[] { "48 0FC7 20", 4, Code.Xsavec64_m0, MemorySize.Xsave64 };

				yield return new object[] { "0FC7 28", 3, Code.Xsaves_m0, MemorySize.Xsave };
				yield return new object[] { "48 0FC7 28", 4, Code.Xsaves64_m0, MemorySize.Xsave64 };

				yield return new object[] { "0FC7 30", 3, Code.Vmptrld_m64, MemorySize.UInt64 };
				yield return new object[] { "48 0FC7 30", 4, Code.Vmptrld_m64, MemorySize.UInt64 };

				yield return new object[] { "66 0FC7 30", 4, Code.Vmclear_m64, MemorySize.UInt64 };
				yield return new object[] { "66 48 0FC7 30", 5, Code.Vmclear_m64, MemorySize.UInt64 };

				yield return new object[] { "F3 0FC7 30", 4, Code.Vmxon_m64, MemorySize.UInt64 };
				yield return new object[] { "F3 48 0FC7 30", 5, Code.Vmxon_m64, MemorySize.UInt64 };

				yield return new object[] { "0FC7 38", 3, Code.Vmptrst_m64, MemorySize.UInt64 };
				yield return new object[] { "48 0FC7 38", 4, Code.Vmptrst_m64, MemorySize.UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Grp_RM_2_Data))]
		void Test64_Grp_RM_2(string hexBytes, int byteLength, Code code, Register reg) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);
		}
		public static IEnumerable<object[]> Test64_Grp_RM_2_Data {
			get {
				yield return new object[] { "66 0FC7 F2", 4, Code.Rdrand_r16, Register.DX };
				yield return new object[] { "66 41 0FC7 F2", 5, Code.Rdrand_r16, Register.R10W };

				yield return new object[] { "0FC7 F2", 3, Code.Rdrand_r32, Register.EDX };
				yield return new object[] { "41 0FC7 F2", 4, Code.Rdrand_r32, Register.R10D };

				yield return new object[] { "48 0FC7 F2", 4, Code.Rdrand_r64, Register.RDX };
				yield return new object[] { "49 0FC7 F2", 4, Code.Rdrand_r64, Register.R10 };

				yield return new object[] { "66 0FC7 FA", 4, Code.Rdseed_r16, Register.DX };
				yield return new object[] { "66 41 0FC7 FA", 5, Code.Rdseed_r16, Register.R10W };

				yield return new object[] { "0FC7 FA", 3, Code.Rdseed_r32, Register.EDX };
				yield return new object[] { "41 0FC7 FA", 4, Code.Rdseed_r32, Register.R10D };

				yield return new object[] { "48 0FC7 FA", 4, Code.Rdseed_r64, Register.RDX };
				yield return new object[] { "49 0FC7 FA", 4, Code.Rdseed_r64, Register.R10 };

				yield return new object[] { "66 F3 0FC7 FA", 5, Code.Rdpid_r64, Register.RDX };
				yield return new object[] { "66 F3 41 0FC7 FA", 6, Code.Rdpid_r64, Register.R10 };

				yield return new object[] { "F3 0FC7 FA", 4, Code.Rdpid_r64, Register.RDX };
				yield return new object[] { "F3 41 0FC7 FA", 5, Code.Rdpid_r64, Register.R10 };

				yield return new object[] { "F3 48 0FC7 FA", 5, Code.Rdpid_r64, Register.RDX };
				yield return new object[] { "F3 49 0FC7 FA", 5, Code.Rdpid_r64, Register.R10 };
			}
		}
	}
}
