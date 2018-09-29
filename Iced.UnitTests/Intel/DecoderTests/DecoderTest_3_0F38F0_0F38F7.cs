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
	public sealed class DecoderTest_3_0F38F0_0F38F7 : DecoderTest {
		[Fact]
		void Test16_Movbe_r16_m16_2() {
			var decoder = CreateDecoder16("0F38F0 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Movbe_r16_m16, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		void Test32_Movbe_r16_m16_2() {
			var decoder = CreateDecoder32("66 0F38F0 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Movbe_r16_m16, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		[InlineData("66 0F38F0 18", 5, Register.BX)]
		[InlineData("66 44 0F38F0 18", 6, Register.R11W)]
		void Test64_Movbe_r16_m16_2(string hexBytes, int byteLength, Register reg) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Movbe_r16_m16, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);

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
		void Test16_Movbe_r32_m32_2() {
			var decoder = CreateDecoder16("66 0F38F0 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Movbe_r32_m32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		void Test32_Movbe_r32_m32_2() {
			var decoder = CreateDecoder32("0F38F0 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Movbe_r32_m32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		[InlineData("0F38F0 18", 4, Register.EBX)]
		[InlineData("44 0F38F0 18", 5, Register.R11D)]
		void Test64_Movbe_r32_m32_2(string hexBytes, int byteLength, Register reg) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Movbe_r32_m32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);

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
		[InlineData("48 0F38F0 18", 5, Register.RBX)]
		[InlineData("4C 0F38F0 18", 5, Register.R11)]
		void Test64_Movbe_r64_m64_2(string hexBytes, int byteLength, Register reg) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Movbe_r64_m64, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);

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
		void Test16_Movbe_m16_r16_2() {
			var decoder = CreateDecoder16("0F38F1 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Movbe_m16_r16, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.BX, instr.Op1Register);
		}

		[Fact]
		void Test32_Movbe_m16_r16_2() {
			var decoder = CreateDecoder32("66 0F38F1 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Movbe_m16_r16, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.BX, instr.Op1Register);
		}

		[Theory]
		[InlineData("66 0F38F1 18", 5, Register.BX)]
		[InlineData("66 44 0F38F1 18", 6, Register.R11W)]
		void Test64_Movbe_m16_r16_2(string hexBytes, int byteLength, Register reg) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Movbe_m16_r16, instr.Code);
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);
		}

		[Fact]
		void Test16_Movbe_m32_r32_2() {
			var decoder = CreateDecoder16("66 0F38F1 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Movbe_m32_r32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EBX, instr.Op1Register);
		}

		[Fact]
		void Test32_Movbe_m32_r32_2() {
			var decoder = CreateDecoder32("0F38F1 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Movbe_m32_r32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EBX, instr.Op1Register);
		}

		[Theory]
		[InlineData("0F38F1 18", 4, Register.EBX)]
		[InlineData("44 0F38F1 18", 5, Register.R11D)]
		void Test64_Movbe_m32_r32_2(string hexBytes, int byteLength, Register reg) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Movbe_m32_r32, instr.Code);
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);
		}

		[Theory]
		[InlineData("48 0F38F1 18", 5, Register.RBX)]
		[InlineData("4C 0F38F1 18", 5, Register.R11)]
		void Test64_Movbe_m64_r64_2(string hexBytes, int byteLength, Register reg) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Movbe_m64_r64, instr.Code);
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);
		}

		[Fact]
		void Test16_Crc32_r32_rm8_1() {
			var decoder = CreateDecoder16("F2 0F38F0 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Crc32_r32_rm8, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ECX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.DH, instr.Op1Register);
		}

		[Fact]
		void Test16_Crc32_r32_rm8_2() {
			var decoder = CreateDecoder16("F2 0F38F0 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Crc32_r32_rm8, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EBX, instr.Op0Register);

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
		void Test32_Crc32_r32_rm8_1() {
			var decoder = CreateDecoder32("F2 0F38F0 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Crc32_r32_rm8, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ECX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.DH, instr.Op1Register);
		}

		[Fact]
		void Test32_Crc32_r32_rm8_2() {
			var decoder = CreateDecoder32("F2 0F38F0 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Crc32_r32_rm8, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EBX, instr.Op0Register);

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
		[InlineData("F2 0F38F0 CE", 5, Register.ECX, Register.DH)]
		[InlineData("F2 0F38F0 C9", 5, Register.ECX, Register.CL)]
		[InlineData("F2 44 0F38F0 C2", 6, Register.R8D, Register.DL)]
		[InlineData("F2 44 0F38F0 C5", 6, Register.R8D, Register.BPL)]
		[InlineData("F2 41 0F38F0 D6", 6, Register.EDX, Register.R14L)]
		[InlineData("F2 45 0F38F0 D0", 6, Register.R10D, Register.R8L)]
		[InlineData("F2 41 0F38F0 D9", 6, Register.EBX, Register.R9L)]
		[InlineData("F2 44 0F38F0 EC", 6, Register.R13D, Register.SPL)]
		void Test64_Crc32_r32_rm8_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Crc32_r32_rm8, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}

		[Fact]
		void Test64_Crc32_r32_rm8_2() {
			var decoder = CreateDecoder64("F2 0F38F0 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Crc32_r32_rm8, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EBX, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("F2 48 0F38F0 CE", 6, Register.RCX, Register.SIL)]
		[InlineData("F2 4C 0F38F0 C5", 6, Register.R8, Register.BPL)]
		[InlineData("F2 49 0F38F0 D6", 6, Register.RDX, Register.R14L)]
		[InlineData("F2 4D 0F38F0 D0", 6, Register.R10, Register.R8L)]
		[InlineData("F2 49 0F38F0 D9", 6, Register.RBX, Register.R9L)]
		[InlineData("F2 4C 0F38F0 EC", 6, Register.R13, Register.SPL)]
		void Test64_Crc32_r64_rm8_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Crc32_r64_rm8, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}

		[Fact]
		void Test64_Crc32_r64_rm8_2() {
			var decoder = CreateDecoder64("F2 48 0F38F0 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Crc32_r64_rm8, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(6, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.RBX, instr.Op0Register);

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
		void Test16_Crc32_r32_rm16_1() {
			var decoder = CreateDecoder16("F2 0F38F1 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Crc32_r32_rm16, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ECX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.SI, instr.Op1Register);
		}

		[Fact]
		void Test16_Crc32_r32_rm16_2() {
			var decoder = CreateDecoder16("F2 0F38F1 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Crc32_r32_rm16, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EBX, instr.Op0Register);

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
		void Test32_Crc32_r32_rm16_1() {
			var decoder = CreateDecoder32("66 F2 0F38F1 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Crc32_r32_rm16, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(6, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ECX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.SI, instr.Op1Register);
		}

		[Fact]
		void Test32_Crc32_r32_rm16_2() {
			var decoder = CreateDecoder32("66 F2 0F38F1 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Crc32_r32_rm16, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(6, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EBX, instr.Op0Register);

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
		[InlineData("66 F2 0F38F1 CE", 6, Register.ECX, Register.SI)]
		[InlineData("66 F2 44 0F38F1 C5", 7, Register.R8D, Register.BP)]
		[InlineData("66 F2 41 0F38F1 D6", 7, Register.EDX, Register.R14W)]
		[InlineData("66 F2 45 0F38F1 D0", 7, Register.R10D, Register.R8W)]
		[InlineData("66 F2 41 0F38F1 D9", 7, Register.EBX, Register.R9W)]
		[InlineData("66 F2 44 0F38F1 EC", 7, Register.R13D, Register.SP)]
		void Test64_Crc32_r32_rm16_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Crc32_r32_rm16, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}

		[Fact]
		void Test64_Crc32_r32_rm16_2() {
			var decoder = CreateDecoder64("66 F2 0F38F1 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Crc32_r32_rm16, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(6, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EBX, instr.Op0Register);

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
		void Test16_Crc32_r32_rm32_1() {
			var decoder = CreateDecoder16("66 F2 0F38F1 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Crc32_r32_rm32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(6, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ECX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ESI, instr.Op1Register);
		}

		[Fact]
		void Test16_Crc32_r32_rm32_2() {
			var decoder = CreateDecoder16("66 F2 0F38F1 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Crc32_r32_rm32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(6, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		void Test32_Crc32_r32_rm32_1() {
			var decoder = CreateDecoder32("F2 0F38F1 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Crc32_r32_rm32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ECX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ESI, instr.Op1Register);
		}

		[Fact]
		void Test32_Crc32_r32_rm32_2() {
			var decoder = CreateDecoder32("F2 0F38F1 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Crc32_r32_rm32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		[InlineData("F2 0F38F1 CE", 5, Register.ECX, Register.ESI)]
		[InlineData("F2 44 0F38F1 C5", 6, Register.R8D, Register.EBP)]
		[InlineData("F2 41 0F38F1 D6", 6, Register.EDX, Register.R14D)]
		[InlineData("F2 45 0F38F1 D0", 6, Register.R10D, Register.R8D)]
		[InlineData("F2 41 0F38F1 D9", 6, Register.EBX, Register.R9D)]
		[InlineData("F2 44 0F38F1 EC", 6, Register.R13D, Register.ESP)]
		void Test64_Crc32_r32_rm32_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Crc32_r32_rm32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}

		[Fact]
		void Test64_Crc32_r32_rm32_2() {
			var decoder = CreateDecoder64("F2 0F38F1 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Crc32_r32_rm32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		[InlineData("F2 48 0F38F1 CE", 6, Register.RCX, Register.RSI)]
		[InlineData("F2 4C 0F38F1 C5", 6, Register.R8, Register.RBP)]
		[InlineData("F2 49 0F38F1 D6", 6, Register.RDX, Register.R14)]
		[InlineData("F2 4D 0F38F1 D0", 6, Register.R10, Register.R8)]
		[InlineData("F2 49 0F38F1 D9", 6, Register.RBX, Register.R9)]
		[InlineData("F2 4C 0F38F1 EC", 6, Register.R13, Register.RSP)]
		void Test64_Crc32_r64_rm64_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Crc32_r64_rm64, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}

		[Fact]
		void Test64_Crc32_r64_rm64_2() {
			var decoder = CreateDecoder64("F2 48 0F38F1 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Crc32_r64_rm64, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(6, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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

		[Theory]
		[MemberData(nameof(Test16_Andn_GdGd_Ed_1_Data))]
		void Test16_Andn_GdGd_Ed_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		}
		public static IEnumerable<object[]> Test16_Andn_GdGd_Ed_1_Data {
			get {
				yield return new object[] { "C4E248 F2 10", 5, Code.VEX_Andn_r32_r32_rm32, Register.EDX, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "C4E2C8 F2 10", 5, Code.VEX_Andn_r32_r32_rm32, Register.EDX, Register.ESI, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Andn_GdGd_Ed_2_Data))]
		void Test16_Andn_GdGd_Ed_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test16_Andn_GdGd_Ed_2_Data {
			get {
				yield return new object[] { "C4E248 F2 D3", 5, Code.VEX_Andn_r32_r32_rm32, Register.EDX, Register.ESI, Register.EBX };
				yield return new object[] { "C4E2C8 F2 D3", 5, Code.VEX_Andn_r32_r32_rm32, Register.EDX, Register.ESI, Register.EBX };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Andn_GdGd_Ed_1_Data))]
		void Test32_Andn_GdGd_Ed_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		}
		public static IEnumerable<object[]> Test32_Andn_GdGd_Ed_1_Data {
			get {
				yield return new object[] { "C4E248 F2 10", 5, Code.VEX_Andn_r32_r32_rm32, Register.EDX, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "C4E2C8 F2 10", 5, Code.VEX_Andn_r32_r32_rm32, Register.EDX, Register.ESI, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Andn_GdGd_Ed_2_Data))]
		void Test32_Andn_GdGd_Ed_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test32_Andn_GdGd_Ed_2_Data {
			get {
				yield return new object[] { "C4E248 F2 D3", 5, Code.VEX_Andn_r32_r32_rm32, Register.EDX, Register.ESI, Register.EBX };
				yield return new object[] { "C4E2C8 F2 D3", 5, Code.VEX_Andn_r32_r32_rm32, Register.EDX, Register.ESI, Register.EBX };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Andn_GdGd_Ed_1_Data))]
		void Test64_Andn_GdGd_Ed_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		}
		public static IEnumerable<object[]> Test64_Andn_GdGd_Ed_1_Data {
			get {
				yield return new object[] { "C4E248 F2 10", 5, Code.VEX_Andn_r32_r32_rm32, Register.EDX, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "C4E2C8 F2 10", 5, Code.VEX_Andn_r64_r64_rm64, Register.RDX, Register.RSI, MemorySize.UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Andn_GdGd_Ed_2_Data))]
		void Test64_Andn_GdGd_Ed_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test64_Andn_GdGd_Ed_2_Data {
			get {
				yield return new object[] { "C4E248 F2 D3", 5, Code.VEX_Andn_r32_r32_rm32, Register.EDX, Register.ESI, Register.EBX };
				yield return new object[] { "C46248 F2 D3", 5, Code.VEX_Andn_r32_r32_rm32, Register.R10D, Register.ESI, Register.EBX };
				yield return new object[] { "C4C208 F2 D3", 5, Code.VEX_Andn_r32_r32_rm32, Register.EDX, Register.R14D, Register.R11D };
				yield return new object[] { "C44248 F2 D3", 5, Code.VEX_Andn_r32_r32_rm32, Register.R10D, Register.ESI, Register.R11D };

				yield return new object[] { "C4E2C8 F2 D3", 5, Code.VEX_Andn_r64_r64_rm64, Register.RDX, Register.RSI, Register.RBX };
				yield return new object[] { "C462C8 F2 D3", 5, Code.VEX_Andn_r64_r64_rm64, Register.R10, Register.RSI, Register.RBX };
				yield return new object[] { "C4C288 F2 D3", 5, Code.VEX_Andn_r64_r64_rm64, Register.RDX, Register.R14, Register.R11 };
				yield return new object[] { "C442C8 F2 D3", 5, Code.VEX_Andn_r64_r64_rm64, Register.R10, Register.RSI, Register.R11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Blsr_Gd_Ed_1_Data))]
		void Test16_Blsr_Gd_Ed_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize) {
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
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_Blsr_Gd_Ed_1_Data {
			get {
				yield return new object[] { "C4E248 F3 08", 5, Code.VEX_Blsr_r32_rm32, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "C4E2C8 F3 08", 5, Code.VEX_Blsr_r32_rm32, Register.ESI, MemorySize.UInt32 };

				yield return new object[] { "C4E248 F3 10", 5, Code.VEX_Blsmsk_r32_rm32, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "C4E2C8 F3 10", 5, Code.VEX_Blsmsk_r32_rm32, Register.ESI, MemorySize.UInt32 };

				yield return new object[] { "C4E248 F3 18", 5, Code.VEX_Blsi_r32_rm32, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "C4E2C8 F3 18", 5, Code.VEX_Blsi_r32_rm32, Register.ESI, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Blsr_Gd_Ed_2_Data))]
		void Test16_Blsr_Gd_Ed_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test16_Blsr_Gd_Ed_2_Data {
			get {
				yield return new object[] { "C4E248 F3 CB", 5, Code.VEX_Blsr_r32_rm32, Register.ESI, Register.EBX };
				yield return new object[] { "C4E2C8 F3 CB", 5, Code.VEX_Blsr_r32_rm32, Register.ESI, Register.EBX };

				yield return new object[] { "C4E248 F3 D3", 5, Code.VEX_Blsmsk_r32_rm32, Register.ESI, Register.EBX };
				yield return new object[] { "C4E2C8 F3 D3", 5, Code.VEX_Blsmsk_r32_rm32, Register.ESI, Register.EBX };

				yield return new object[] { "C4E248 F3 DB", 5, Code.VEX_Blsi_r32_rm32, Register.ESI, Register.EBX };
				yield return new object[] { "C4E2C8 F3 DB", 5, Code.VEX_Blsi_r32_rm32, Register.ESI, Register.EBX };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Blsr_Gd_Ed_1_Data))]
		void Test32_Blsr_Gd_Ed_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize) {
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
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_Blsr_Gd_Ed_1_Data {
			get {
				yield return new object[] { "C4E248 F3 08", 5, Code.VEX_Blsr_r32_rm32, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "C4E2C8 F3 08", 5, Code.VEX_Blsr_r32_rm32, Register.ESI, MemorySize.UInt32 };

				yield return new object[] { "C4E248 F3 10", 5, Code.VEX_Blsmsk_r32_rm32, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "C4E2C8 F3 10", 5, Code.VEX_Blsmsk_r32_rm32, Register.ESI, MemorySize.UInt32 };

				yield return new object[] { "C4E248 F3 18", 5, Code.VEX_Blsi_r32_rm32, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "C4E2C8 F3 18", 5, Code.VEX_Blsi_r32_rm32, Register.ESI, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Blsr_Gd_Ed_2_Data))]
		void Test32_Blsr_Gd_Ed_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test32_Blsr_Gd_Ed_2_Data {
			get {
				yield return new object[] { "C4E248 F3 CB", 5, Code.VEX_Blsr_r32_rm32, Register.ESI, Register.EBX };
				yield return new object[] { "C4E2C8 F3 CB", 5, Code.VEX_Blsr_r32_rm32, Register.ESI, Register.EBX };

				yield return new object[] { "C4E248 F3 D3", 5, Code.VEX_Blsmsk_r32_rm32, Register.ESI, Register.EBX };
				yield return new object[] { "C4E2C8 F3 D3", 5, Code.VEX_Blsmsk_r32_rm32, Register.ESI, Register.EBX };

				yield return new object[] { "C4E248 F3 DB", 5, Code.VEX_Blsi_r32_rm32, Register.ESI, Register.EBX };
				yield return new object[] { "C4E2C8 F3 DB", 5, Code.VEX_Blsi_r32_rm32, Register.ESI, Register.EBX };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Blsr_Gd_Ed_1_Data))]
		void Test64_Blsr_Gd_Ed_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize) {
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
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_Blsr_Gd_Ed_1_Data {
			get {
				yield return new object[] { "C4E248 F3 08", 5, Code.VEX_Blsr_r32_rm32, Register.ESI, MemorySize.UInt32 };

				yield return new object[] { "C4E2C8 F3 08", 5, Code.VEX_Blsr_r64_rm64, Register.RSI, MemorySize.UInt64 };

				yield return new object[] { "C4E248 F3 10", 5, Code.VEX_Blsmsk_r32_rm32, Register.ESI, MemorySize.UInt32 };

				yield return new object[] { "C4E2C8 F3 10", 5, Code.VEX_Blsmsk_r64_rm64, Register.RSI, MemorySize.UInt64 };

				yield return new object[] { "C4E248 F3 18", 5, Code.VEX_Blsi_r32_rm32, Register.ESI, MemorySize.UInt32 };

				yield return new object[] { "C4E2C8 F3 18", 5, Code.VEX_Blsi_r64_rm64, Register.RSI, MemorySize.UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Blsr_Gd_Ed_2_Data))]
		void Test64_Blsr_Gd_Ed_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test64_Blsr_Gd_Ed_2_Data {
			get {
				yield return new object[] { "C4E248 F3 CB", 5, Code.VEX_Blsr_r32_rm32, Register.ESI, Register.EBX };
				yield return new object[] { "C4C208 F3 CB", 5, Code.VEX_Blsr_r32_rm32, Register.R14D, Register.R11D };
				yield return new object[] { "C44248 F3 CB", 5, Code.VEX_Blsr_r32_rm32, Register.ESI, Register.R11D };

				yield return new object[] { "C4E2C8 F3 CB", 5, Code.VEX_Blsr_r64_rm64, Register.RSI, Register.RBX };
				yield return new object[] { "C4C288 F3 CB", 5, Code.VEX_Blsr_r64_rm64, Register.R14, Register.R11 };
				yield return new object[] { "C442C8 F3 CB", 5, Code.VEX_Blsr_r64_rm64, Register.RSI, Register.R11 };

				yield return new object[] { "C4E248 F3 D3", 5, Code.VEX_Blsmsk_r32_rm32, Register.ESI, Register.EBX };
				yield return new object[] { "C4C208 F3 D3", 5, Code.VEX_Blsmsk_r32_rm32, Register.R14D, Register.R11D };
				yield return new object[] { "C44248 F3 D3", 5, Code.VEX_Blsmsk_r32_rm32, Register.ESI, Register.R11D };

				yield return new object[] { "C4E2C8 F3 D3", 5, Code.VEX_Blsmsk_r64_rm64, Register.RSI, Register.RBX };
				yield return new object[] { "C4C288 F3 D3", 5, Code.VEX_Blsmsk_r64_rm64, Register.R14, Register.R11 };
				yield return new object[] { "C442C8 F3 D3", 5, Code.VEX_Blsmsk_r64_rm64, Register.RSI, Register.R11 };

				yield return new object[] { "C4E248 F3 DB", 5, Code.VEX_Blsi_r32_rm32, Register.ESI, Register.EBX };
				yield return new object[] { "C4C208 F3 DB", 5, Code.VEX_Blsi_r32_rm32, Register.R14D, Register.R11D };
				yield return new object[] { "C44248 F3 DB", 5, Code.VEX_Blsi_r32_rm32, Register.ESI, Register.R11D };

				yield return new object[] { "C4E2C8 F3 DB", 5, Code.VEX_Blsi_r64_rm64, Register.RSI, Register.RBX };
				yield return new object[] { "C4C288 F3 DB", 5, Code.VEX_Blsi_r64_rm64, Register.R14, Register.R11 };
				yield return new object[] { "C442C8 F3 DB", 5, Code.VEX_Blsi_r64_rm64, Register.RSI, Register.R11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Mem_Reg_2_Data))]
		void Test16_Mem_Reg_2(string hexBytes, int byteLength, Code code, MemorySize memSize, Register reg, DecoderOptions options) {
			var decoder = CreateDecoder16(hexBytes, options);
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
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test16_Mem_Reg_2_Data {
			get {
				yield return new object[] { "66 0F38F5 18", 5, Code.Wrussd_m32_r32, MemorySize.UInt32, Register.EBX, DecoderOptions.None };

				yield return new object[] { "0F38F6 18", 4, Code.Wrssd_m32_r32, MemorySize.UInt32, Register.EBX, DecoderOptions.None };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Mem_Reg_2_Data))]
		void Test32_Mem_Reg_2(string hexBytes, int byteLength, Code code, MemorySize memSize, Register reg, DecoderOptions options) {
			var decoder = CreateDecoder32(hexBytes, options);
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
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test32_Mem_Reg_2_Data {
			get {
				yield return new object[] { "66 0F38F5 18", 5, Code.Wrussd_m32_r32, MemorySize.UInt32, Register.EBX, DecoderOptions.None };

				yield return new object[] { "0F38F6 18", 4, Code.Wrssd_m32_r32, MemorySize.UInt32, Register.EBX, DecoderOptions.None };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Mem_Reg_2_Data))]
		void Test64_Mem_Reg_2(string hexBytes, int byteLength, Code code, MemorySize memSize, Register reg, DecoderOptions options) {
			var decoder = CreateDecoder64(hexBytes, options);
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
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test64_Mem_Reg_2_Data {
			get {
				yield return new object[] { "66 0F38F5 18", 5, Code.Wrussd_m32_r32, MemorySize.UInt32, Register.EBX, DecoderOptions.None };
				yield return new object[] { "66 44 0F38F5 18", 6, Code.Wrussd_m32_r32, MemorySize.UInt32, Register.R11D, DecoderOptions.None };

				yield return new object[] { "66 48 0F38F5 18", 6, Code.Wrussq_m64_r64, MemorySize.UInt64, Register.RBX, DecoderOptions.None };
				yield return new object[] { "66 4C 0F38F5 18", 6, Code.Wrussq_m64_r64, MemorySize.UInt64, Register.R11, DecoderOptions.None };

				yield return new object[] { "0F38F6 18", 4, Code.Wrssd_m32_r32, MemorySize.UInt32, Register.EBX, DecoderOptions.None };
				yield return new object[] { "44 0F38F6 18", 5, Code.Wrssd_m32_r32, MemorySize.UInt32, Register.R11D, DecoderOptions.None };

				yield return new object[] { "48 0F38F6 18", 5, Code.Wrssq_m64_r64, MemorySize.UInt64, Register.RBX, DecoderOptions.None };
				yield return new object[] { "4C 0F38F6 18", 5, Code.Wrssq_m64_r64, MemorySize.UInt64, Register.R11, DecoderOptions.None };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Bzhi_Gd_Ed_Gd_1_Data))]
		void Test16_Bzhi_Gd_Ed_Gd_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg2, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test16_Bzhi_Gd_Ed_Gd_1_Data {
			get {
				yield return new object[] { "C4E248 F5 10", 5, Code.VEX_Bzhi_r32_rm32_r32, Register.EDX, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "C4E2C8 F5 10", 5, Code.VEX_Bzhi_r32_rm32_r32, Register.EDX, Register.ESI, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Bzhi_Gd_Ed_Gd_2_Data))]
		void Test16_Bzhi_Gd_Ed_Gd_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test16_Bzhi_Gd_Ed_Gd_2_Data {
			get {
				yield return new object[] { "C4E248 F5 D3", 5, Code.VEX_Bzhi_r32_rm32_r32, Register.EDX, Register.EBX, Register.ESI };
				yield return new object[] { "C4E2C8 F5 D3", 5, Code.VEX_Bzhi_r32_rm32_r32, Register.EDX, Register.EBX, Register.ESI };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Bzhi_Gd_Ed_Gd_1_Data))]
		void Test32_Bzhi_Gd_Ed_Gd_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg2, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test32_Bzhi_Gd_Ed_Gd_1_Data {
			get {
				yield return new object[] { "C4E248 F5 10", 5, Code.VEX_Bzhi_r32_rm32_r32, Register.EDX, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "C4E2C8 F5 10", 5, Code.VEX_Bzhi_r32_rm32_r32, Register.EDX, Register.ESI, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Bzhi_Gd_Ed_Gd_2_Data))]
		void Test32_Bzhi_Gd_Ed_Gd_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test32_Bzhi_Gd_Ed_Gd_2_Data {
			get {
				yield return new object[] { "C4E248 F5 D3", 5, Code.VEX_Bzhi_r32_rm32_r32, Register.EDX, Register.EBX, Register.ESI };
				yield return new object[] { "C4E2C8 F5 D3", 5, Code.VEX_Bzhi_r32_rm32_r32, Register.EDX, Register.EBX, Register.ESI };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Bzhi_Gd_Ed_Gd_1_Data))]
		void Test64_Bzhi_Gd_Ed_Gd_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg2, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test64_Bzhi_Gd_Ed_Gd_1_Data {
			get {
				yield return new object[] { "C4E248 F5 10", 5, Code.VEX_Bzhi_r32_rm32_r32, Register.EDX, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "C4E2C8 F5 10", 5, Code.VEX_Bzhi_r64_rm64_r64, Register.RDX, Register.RSI, MemorySize.UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Bzhi_Gd_Ed_Gd_2_Data))]
		void Test64_Bzhi_Gd_Ed_Gd_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test64_Bzhi_Gd_Ed_Gd_2_Data {
			get {
				yield return new object[] { "C4E248 F5 D3", 5, Code.VEX_Bzhi_r32_rm32_r32, Register.EDX, Register.EBX, Register.ESI };
				yield return new object[] { "C46248 F5 D3", 5, Code.VEX_Bzhi_r32_rm32_r32, Register.R10D, Register.EBX, Register.ESI };
				yield return new object[] { "C4C208 F5 D3", 5, Code.VEX_Bzhi_r32_rm32_r32, Register.EDX, Register.R11D, Register.R14D };
				yield return new object[] { "C44248 F5 D3", 5, Code.VEX_Bzhi_r32_rm32_r32, Register.R10D, Register.R11D, Register.ESI };

				yield return new object[] { "C4E2C8 F5 D3", 5, Code.VEX_Bzhi_r64_rm64_r64, Register.RDX, Register.RBX, Register.RSI };
				yield return new object[] { "C462C8 F5 D3", 5, Code.VEX_Bzhi_r64_rm64_r64, Register.R10, Register.RBX, Register.RSI };
				yield return new object[] { "C4C288 F5 D3", 5, Code.VEX_Bzhi_r64_rm64_r64, Register.RDX, Register.R11, Register.R14 };
				yield return new object[] { "C442C8 F5 D3", 5, Code.VEX_Bzhi_r64_rm64_r64, Register.R10, Register.R11, Register.RSI };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Pext_GdGd_Ed_1_Data))]
		void Test16_Pext_GdGd_Ed_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		}
		public static IEnumerable<object[]> Test16_Pext_GdGd_Ed_1_Data {
			get {
				yield return new object[] { "C4E24A F5 10", 5, Code.VEX_Pext_r32_r32_rm32, Register.EDX, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "C4E2CA F5 10", 5, Code.VEX_Pext_r32_r32_rm32, Register.EDX, Register.ESI, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Pext_GdGd_Ed_2_Data))]
		void Test16_Pext_GdGd_Ed_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test16_Pext_GdGd_Ed_2_Data {
			get {
				yield return new object[] { "C4E24A F5 D3", 5, Code.VEX_Pext_r32_r32_rm32, Register.EDX, Register.ESI, Register.EBX };
				yield return new object[] { "C4E2CA F5 D3", 5, Code.VEX_Pext_r32_r32_rm32, Register.EDX, Register.ESI, Register.EBX };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Pext_GdGd_Ed_1_Data))]
		void Test32_Pext_GdGd_Ed_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		}
		public static IEnumerable<object[]> Test32_Pext_GdGd_Ed_1_Data {
			get {
				yield return new object[] { "C4E24A F5 10", 5, Code.VEX_Pext_r32_r32_rm32, Register.EDX, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "C4E2CA F5 10", 5, Code.VEX_Pext_r32_r32_rm32, Register.EDX, Register.ESI, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Pext_GdGd_Ed_2_Data))]
		void Test32_Pext_GdGd_Ed_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test32_Pext_GdGd_Ed_2_Data {
			get {
				yield return new object[] { "C4E24A F5 D3", 5, Code.VEX_Pext_r32_r32_rm32, Register.EDX, Register.ESI, Register.EBX };
				yield return new object[] { "C4E2CA F5 D3", 5, Code.VEX_Pext_r32_r32_rm32, Register.EDX, Register.ESI, Register.EBX };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Pext_GdGd_Ed_1_Data))]
		void Test64_Pext_GdGd_Ed_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		}
		public static IEnumerable<object[]> Test64_Pext_GdGd_Ed_1_Data {
			get {
				yield return new object[] { "C4E24A F5 10", 5, Code.VEX_Pext_r32_r32_rm32, Register.EDX, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "C4E2CA F5 10", 5, Code.VEX_Pext_r64_r64_rm64, Register.RDX, Register.RSI, MemorySize.UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Pext_GdGd_Ed_2_Data))]
		void Test64_Pext_GdGd_Ed_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test64_Pext_GdGd_Ed_2_Data {
			get {
				yield return new object[] { "C4E24A F5 D3", 5, Code.VEX_Pext_r32_r32_rm32, Register.EDX, Register.ESI, Register.EBX };
				yield return new object[] { "C4624A F5 D3", 5, Code.VEX_Pext_r32_r32_rm32, Register.R10D, Register.ESI, Register.EBX };
				yield return new object[] { "C4C20A F5 D3", 5, Code.VEX_Pext_r32_r32_rm32, Register.EDX, Register.R14D, Register.R11D };
				yield return new object[] { "C4424A F5 D3", 5, Code.VEX_Pext_r32_r32_rm32, Register.R10D, Register.ESI, Register.R11D };

				yield return new object[] { "C4E2CA F5 D3", 5, Code.VEX_Pext_r64_r64_rm64, Register.RDX, Register.RSI, Register.RBX };
				yield return new object[] { "C462CA F5 D3", 5, Code.VEX_Pext_r64_r64_rm64, Register.R10, Register.RSI, Register.RBX };
				yield return new object[] { "C4C28A F5 D3", 5, Code.VEX_Pext_r64_r64_rm64, Register.RDX, Register.R14, Register.R11 };
				yield return new object[] { "C442CA F5 D3", 5, Code.VEX_Pext_r64_r64_rm64, Register.R10, Register.RSI, Register.R11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Pdep_GdGd_Ed_1_Data))]
		void Test16_Pdep_GdGd_Ed_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		}
		public static IEnumerable<object[]> Test16_Pdep_GdGd_Ed_1_Data {
			get {
				yield return new object[] { "C4E24B F5 10", 5, Code.VEX_Pdep_r32_r32_rm32, Register.EDX, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "C4E2CB F5 10", 5, Code.VEX_Pdep_r32_r32_rm32, Register.EDX, Register.ESI, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Pdep_GdGd_Ed_2_Data))]
		void Test16_Pdep_GdGd_Ed_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test16_Pdep_GdGd_Ed_2_Data {
			get {
				yield return new object[] { "C4E24B F5 D3", 5, Code.VEX_Pdep_r32_r32_rm32, Register.EDX, Register.ESI, Register.EBX };
				yield return new object[] { "C4E2CB F5 D3", 5, Code.VEX_Pdep_r32_r32_rm32, Register.EDX, Register.ESI, Register.EBX };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Pdep_GdGd_Ed_1_Data))]
		void Test32_Pdep_GdGd_Ed_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		}
		public static IEnumerable<object[]> Test32_Pdep_GdGd_Ed_1_Data {
			get {
				yield return new object[] { "C4E24B F5 10", 5, Code.VEX_Pdep_r32_r32_rm32, Register.EDX, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "C4E2CB F5 10", 5, Code.VEX_Pdep_r32_r32_rm32, Register.EDX, Register.ESI, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Pdep_GdGd_Ed_2_Data))]
		void Test32_Pdep_GdGd_Ed_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test32_Pdep_GdGd_Ed_2_Data {
			get {
				yield return new object[] { "C4E24B F5 D3", 5, Code.VEX_Pdep_r32_r32_rm32, Register.EDX, Register.ESI, Register.EBX };
				yield return new object[] { "C4E2CB F5 D3", 5, Code.VEX_Pdep_r32_r32_rm32, Register.EDX, Register.ESI, Register.EBX };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Pdep_GdGd_Ed_1_Data))]
		void Test64_Pdep_GdGd_Ed_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		}
		public static IEnumerable<object[]> Test64_Pdep_GdGd_Ed_1_Data {
			get {
				yield return new object[] { "C4E24B F5 10", 5, Code.VEX_Pdep_r32_r32_rm32, Register.EDX, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "C4E2CB F5 10", 5, Code.VEX_Pdep_r64_r64_rm64, Register.RDX, Register.RSI, MemorySize.UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Pdep_GdGd_Ed_2_Data))]
		void Test64_Pdep_GdGd_Ed_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test64_Pdep_GdGd_Ed_2_Data {
			get {
				yield return new object[] { "C4E24B F5 D3", 5, Code.VEX_Pdep_r32_r32_rm32, Register.EDX, Register.ESI, Register.EBX };
				yield return new object[] { "C4624B F5 D3", 5, Code.VEX_Pdep_r32_r32_rm32, Register.R10D, Register.ESI, Register.EBX };
				yield return new object[] { "C4C20B F5 D3", 5, Code.VEX_Pdep_r32_r32_rm32, Register.EDX, Register.R14D, Register.R11D };
				yield return new object[] { "C4424B F5 D3", 5, Code.VEX_Pdep_r32_r32_rm32, Register.R10D, Register.ESI, Register.R11D };

				yield return new object[] { "C4E2CB F5 D3", 5, Code.VEX_Pdep_r64_r64_rm64, Register.RDX, Register.RSI, Register.RBX };
				yield return new object[] { "C462CB F5 D3", 5, Code.VEX_Pdep_r64_r64_rm64, Register.R10, Register.RSI, Register.RBX };
				yield return new object[] { "C4C28B F5 D3", 5, Code.VEX_Pdep_r64_r64_rm64, Register.RDX, Register.R14, Register.R11 };
				yield return new object[] { "C442CB F5 D3", 5, Code.VEX_Pdep_r64_r64_rm64, Register.R10, Register.RSI, Register.R11 };
			}
		}

		[Fact]
		void Test16_Adcx_r32_rm32_1() {
			var decoder = CreateDecoder16("66 0F38F6 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Adcx_r32_rm32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ECX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ESI, instr.Op1Register);
		}

		[Fact]
		void Test16_Adcx_r32_rm32_2() {
			var decoder = CreateDecoder16("66 0F38F6 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Adcx_r32_rm32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		void Test32_Adcx_r32_rm32_1() {
			var decoder = CreateDecoder32("66 0F38F6 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Adcx_r32_rm32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ECX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ESI, instr.Op1Register);
		}

		[Fact]
		void Test32_Adcx_r32_rm32_2() {
			var decoder = CreateDecoder32("66 0F38F6 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Adcx_r32_rm32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		[InlineData("66 0F38F6 CE", 5, Register.ECX, Register.ESI)]
		[InlineData("66 44 0F38F6 C5", 6, Register.R8D, Register.EBP)]
		[InlineData("66 41 0F38F6 D6", 6, Register.EDX, Register.R14D)]
		[InlineData("66 45 0F38F6 D0", 6, Register.R10D, Register.R8D)]
		[InlineData("66 41 0F38F6 D9", 6, Register.EBX, Register.R9D)]
		[InlineData("66 44 0F38F6 EC", 6, Register.R13D, Register.ESP)]
		void Test64_Adcx_r32_rm32_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Adcx_r32_rm32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}

		[Fact]
		void Test64_Adcx_r32_rm32_2() {
			var decoder = CreateDecoder64("66 0F38F6 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Adcx_r32_rm32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		[InlineData("66 48 0F38F6 CE", 6, Register.RCX, Register.RSI)]
		[InlineData("66 4C 0F38F6 C5", 6, Register.R8, Register.RBP)]
		[InlineData("66 49 0F38F6 D6", 6, Register.RDX, Register.R14)]
		[InlineData("66 4D 0F38F6 D0", 6, Register.R10, Register.R8)]
		[InlineData("66 49 0F38F6 D9", 6, Register.RBX, Register.R9)]
		[InlineData("66 4C 0F38F6 EC", 6, Register.R13, Register.RSP)]
		void Test64_Adcx_r64_rm64_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Adcx_r64_rm64, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}

		[Fact]
		void Test64_Adcx_r64_rm64_2() {
			var decoder = CreateDecoder64("66 48 0F38F6 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Adcx_r64_rm64, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(6, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		void Test16_Adox_r32_rm32_1() {
			var decoder = CreateDecoder16("F3 0F38F6 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Adox_r32_rm32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ECX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ESI, instr.Op1Register);
		}

		[Fact]
		void Test16_Adox_r32_rm32_2() {
			var decoder = CreateDecoder16("F3 0F38F6 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Adox_r32_rm32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		void Test32_Adox_r32_rm32_1() {
			var decoder = CreateDecoder32("F3 0F38F6 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Adox_r32_rm32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ECX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ESI, instr.Op1Register);
		}

		[Fact]
		void Test32_Adox_r32_rm32_2() {
			var decoder = CreateDecoder32("F3 0F38F6 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Adox_r32_rm32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		[InlineData("F3 0F38F6 CE", 5, Register.ECX, Register.ESI)]
		[InlineData("F3 44 0F38F6 C5", 6, Register.R8D, Register.EBP)]
		[InlineData("F3 41 0F38F6 D6", 6, Register.EDX, Register.R14D)]
		[InlineData("F3 45 0F38F6 D0", 6, Register.R10D, Register.R8D)]
		[InlineData("F3 41 0F38F6 D9", 6, Register.EBX, Register.R9D)]
		[InlineData("F3 44 0F38F6 EC", 6, Register.R13D, Register.ESP)]
		void Test64_Adox_r32_rm32_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Adox_r32_rm32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}

		[Fact]
		void Test64_Adox_r32_rm32_2() {
			var decoder = CreateDecoder64("F3 0F38F6 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Adox_r32_rm32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		[InlineData("F3 48 0F38F6 CE", 6, Register.RCX, Register.RSI)]
		[InlineData("F3 4C 0F38F6 C5", 6, Register.R8, Register.RBP)]
		[InlineData("F3 49 0F38F6 D6", 6, Register.RDX, Register.R14)]
		[InlineData("F3 4D 0F38F6 D0", 6, Register.R10, Register.R8)]
		[InlineData("F3 49 0F38F6 D9", 6, Register.RBX, Register.R9)]
		[InlineData("F3 4C 0F38F6 EC", 6, Register.R13, Register.RSP)]
		void Test64_Adox_r64_rm64_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Adox_r64_rm64, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}

		[Fact]
		void Test64_Adox_r64_rm64_2() {
			var decoder = CreateDecoder64("F3 48 0F38F6 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Adox_r64_rm64, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(6, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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

		[Theory]
		[MemberData(nameof(Test16_Mulx_GdGd_Ed_1_Data))]
		void Test16_Mulx_GdGd_Ed_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		}
		public static IEnumerable<object[]> Test16_Mulx_GdGd_Ed_1_Data {
			get {
				yield return new object[] { "C4E24B F6 10", 5, Code.VEX_Mulx_r32_r32_rm32, Register.EDX, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "C4E2CB F6 10", 5, Code.VEX_Mulx_r32_r32_rm32, Register.EDX, Register.ESI, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Mulx_GdGd_Ed_2_Data))]
		void Test16_Mulx_GdGd_Ed_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test16_Mulx_GdGd_Ed_2_Data {
			get {
				yield return new object[] { "C4E24B F6 D3", 5, Code.VEX_Mulx_r32_r32_rm32, Register.EDX, Register.ESI, Register.EBX };
				yield return new object[] { "C4E2CB F6 D3", 5, Code.VEX_Mulx_r32_r32_rm32, Register.EDX, Register.ESI, Register.EBX };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Mulx_GdGd_Ed_1_Data))]
		void Test32_Mulx_GdGd_Ed_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		}
		public static IEnumerable<object[]> Test32_Mulx_GdGd_Ed_1_Data {
			get {
				yield return new object[] { "C4E24B F6 10", 5, Code.VEX_Mulx_r32_r32_rm32, Register.EDX, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "C4E2CB F6 10", 5, Code.VEX_Mulx_r32_r32_rm32, Register.EDX, Register.ESI, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Mulx_GdGd_Ed_2_Data))]
		void Test32_Mulx_GdGd_Ed_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test32_Mulx_GdGd_Ed_2_Data {
			get {
				yield return new object[] { "C4E24B F6 D3", 5, Code.VEX_Mulx_r32_r32_rm32, Register.EDX, Register.ESI, Register.EBX };
				yield return new object[] { "C4E2CB F6 D3", 5, Code.VEX_Mulx_r32_r32_rm32, Register.EDX, Register.ESI, Register.EBX };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Mulx_GdGd_Ed_1_Data))]
		void Test64_Mulx_GdGd_Ed_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		}
		public static IEnumerable<object[]> Test64_Mulx_GdGd_Ed_1_Data {
			get {
				yield return new object[] { "C4E24B F6 10", 5, Code.VEX_Mulx_r32_r32_rm32, Register.EDX, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "C4E2CB F6 10", 5, Code.VEX_Mulx_r64_r64_rm64, Register.RDX, Register.RSI, MemorySize.UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Mulx_GdGd_Ed_2_Data))]
		void Test64_Mulx_GdGd_Ed_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test64_Mulx_GdGd_Ed_2_Data {
			get {
				yield return new object[] { "C4E24B F6 D3", 5, Code.VEX_Mulx_r32_r32_rm32, Register.EDX, Register.ESI, Register.EBX };
				yield return new object[] { "C4624B F6 D3", 5, Code.VEX_Mulx_r32_r32_rm32, Register.R10D, Register.ESI, Register.EBX };
				yield return new object[] { "C4C20B F6 D3", 5, Code.VEX_Mulx_r32_r32_rm32, Register.EDX, Register.R14D, Register.R11D };
				yield return new object[] { "C4424B F6 D3", 5, Code.VEX_Mulx_r32_r32_rm32, Register.R10D, Register.ESI, Register.R11D };

				yield return new object[] { "C4E2CB F6 D3", 5, Code.VEX_Mulx_r64_r64_rm64, Register.RDX, Register.RSI, Register.RBX };
				yield return new object[] { "C462CB F6 D3", 5, Code.VEX_Mulx_r64_r64_rm64, Register.R10, Register.RSI, Register.RBX };
				yield return new object[] { "C4C28B F6 D3", 5, Code.VEX_Mulx_r64_r64_rm64, Register.RDX, Register.R14, Register.R11 };
				yield return new object[] { "C442CB F6 D3", 5, Code.VEX_Mulx_r64_r64_rm64, Register.R10, Register.RSI, Register.R11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Bextr_Gd_Ed_Gd_1_Data))]
		void Test16_Bextr_Gd_Ed_Gd_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg2, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test16_Bextr_Gd_Ed_Gd_1_Data {
			get {
				yield return new object[] { "C4E248 F7 10", 5, Code.VEX_Bextr_r32_rm32_r32, Register.EDX, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "C4E2C8 F7 10", 5, Code.VEX_Bextr_r32_rm32_r32, Register.EDX, Register.ESI, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Bextr_Gd_Ed_Gd_2_Data))]
		void Test16_Bextr_Gd_Ed_Gd_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test16_Bextr_Gd_Ed_Gd_2_Data {
			get {
				yield return new object[] { "C4E248 F7 D3", 5, Code.VEX_Bextr_r32_rm32_r32, Register.EDX, Register.EBX, Register.ESI };
				yield return new object[] { "C4E2C8 F7 D3", 5, Code.VEX_Bextr_r32_rm32_r32, Register.EDX, Register.EBX, Register.ESI };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Bextr_Gd_Ed_Gd_1_Data))]
		void Test32_Bextr_Gd_Ed_Gd_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg2, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test32_Bextr_Gd_Ed_Gd_1_Data {
			get {
				yield return new object[] { "C4E248 F7 10", 5, Code.VEX_Bextr_r32_rm32_r32, Register.EDX, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "C4E2C8 F7 10", 5, Code.VEX_Bextr_r32_rm32_r32, Register.EDX, Register.ESI, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Bextr_Gd_Ed_Gd_2_Data))]
		void Test32_Bextr_Gd_Ed_Gd_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test32_Bextr_Gd_Ed_Gd_2_Data {
			get {
				yield return new object[] { "C4E248 F7 D3", 5, Code.VEX_Bextr_r32_rm32_r32, Register.EDX, Register.EBX, Register.ESI };
				yield return new object[] { "C4E2C8 F7 D3", 5, Code.VEX_Bextr_r32_rm32_r32, Register.EDX, Register.EBX, Register.ESI };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Bextr_Gd_Ed_Gd_1_Data))]
		void Test64_Bextr_Gd_Ed_Gd_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg2, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test64_Bextr_Gd_Ed_Gd_1_Data {
			get {
				yield return new object[] { "C4E248 F7 10", 5, Code.VEX_Bextr_r32_rm32_r32, Register.EDX, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "C4E2C8 F7 10", 5, Code.VEX_Bextr_r64_rm64_r64, Register.RDX, Register.RSI, MemorySize.UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Bextr_Gd_Ed_Gd_2_Data))]
		void Test64_Bextr_Gd_Ed_Gd_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test64_Bextr_Gd_Ed_Gd_2_Data {
			get {
				yield return new object[] { "C4E248 F7 D3", 5, Code.VEX_Bextr_r32_rm32_r32, Register.EDX, Register.EBX, Register.ESI };
				yield return new object[] { "C46248 F7 D3", 5, Code.VEX_Bextr_r32_rm32_r32, Register.R10D, Register.EBX, Register.ESI };
				yield return new object[] { "C4C208 F7 D3", 5, Code.VEX_Bextr_r32_rm32_r32, Register.EDX, Register.R11D, Register.R14D };
				yield return new object[] { "C44248 F7 D3", 5, Code.VEX_Bextr_r32_rm32_r32, Register.R10D, Register.R11D, Register.ESI };

				yield return new object[] { "C4E2C8 F7 D3", 5, Code.VEX_Bextr_r64_rm64_r64, Register.RDX, Register.RBX, Register.RSI };
				yield return new object[] { "C462C8 F7 D3", 5, Code.VEX_Bextr_r64_rm64_r64, Register.R10, Register.RBX, Register.RSI };
				yield return new object[] { "C4C288 F7 D3", 5, Code.VEX_Bextr_r64_rm64_r64, Register.RDX, Register.R11, Register.R14 };
				yield return new object[] { "C442C8 F7 D3", 5, Code.VEX_Bextr_r64_rm64_r64, Register.R10, Register.R11, Register.RSI };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Shlx_Gd_Ed_Gd_1_Data))]
		void Test16_Shlx_Gd_Ed_Gd_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg2, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test16_Shlx_Gd_Ed_Gd_1_Data {
			get {
				yield return new object[] { "C4E249 F7 10", 5, Code.VEX_Shlx_r32_rm32_r32, Register.EDX, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "C4E2C9 F7 10", 5, Code.VEX_Shlx_r32_rm32_r32, Register.EDX, Register.ESI, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Shlx_Gd_Ed_Gd_2_Data))]
		void Test16_Shlx_Gd_Ed_Gd_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test16_Shlx_Gd_Ed_Gd_2_Data {
			get {
				yield return new object[] { "C4E249 F7 D3", 5, Code.VEX_Shlx_r32_rm32_r32, Register.EDX, Register.EBX, Register.ESI };
				yield return new object[] { "C4E2C9 F7 D3", 5, Code.VEX_Shlx_r32_rm32_r32, Register.EDX, Register.EBX, Register.ESI };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Shlx_Gd_Ed_Gd_1_Data))]
		void Test32_Shlx_Gd_Ed_Gd_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg2, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test32_Shlx_Gd_Ed_Gd_1_Data {
			get {
				yield return new object[] { "C4E249 F7 10", 5, Code.VEX_Shlx_r32_rm32_r32, Register.EDX, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "C4E2C9 F7 10", 5, Code.VEX_Shlx_r32_rm32_r32, Register.EDX, Register.ESI, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Shlx_Gd_Ed_Gd_2_Data))]
		void Test32_Shlx_Gd_Ed_Gd_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test32_Shlx_Gd_Ed_Gd_2_Data {
			get {
				yield return new object[] { "C4E249 F7 D3", 5, Code.VEX_Shlx_r32_rm32_r32, Register.EDX, Register.EBX, Register.ESI };
				yield return new object[] { "C4E2C9 F7 D3", 5, Code.VEX_Shlx_r32_rm32_r32, Register.EDX, Register.EBX, Register.ESI };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Shlx_Gd_Ed_Gd_1_Data))]
		void Test64_Shlx_Gd_Ed_Gd_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg2, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test64_Shlx_Gd_Ed_Gd_1_Data {
			get {
				yield return new object[] { "C4E249 F7 10", 5, Code.VEX_Shlx_r32_rm32_r32, Register.EDX, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "C4E2C9 F7 10", 5, Code.VEX_Shlx_r64_rm64_r64, Register.RDX, Register.RSI, MemorySize.UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Shlx_Gd_Ed_Gd_2_Data))]
		void Test64_Shlx_Gd_Ed_Gd_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test64_Shlx_Gd_Ed_Gd_2_Data {
			get {
				yield return new object[] { "C4E249 F7 D3", 5, Code.VEX_Shlx_r32_rm32_r32, Register.EDX, Register.EBX, Register.ESI };
				yield return new object[] { "C46249 F7 D3", 5, Code.VEX_Shlx_r32_rm32_r32, Register.R10D, Register.EBX, Register.ESI };
				yield return new object[] { "C4C209 F7 D3", 5, Code.VEX_Shlx_r32_rm32_r32, Register.EDX, Register.R11D, Register.R14D };
				yield return new object[] { "C44249 F7 D3", 5, Code.VEX_Shlx_r32_rm32_r32, Register.R10D, Register.R11D, Register.ESI };

				yield return new object[] { "C4E2C9 F7 D3", 5, Code.VEX_Shlx_r64_rm64_r64, Register.RDX, Register.RBX, Register.RSI };
				yield return new object[] { "C462C9 F7 D3", 5, Code.VEX_Shlx_r64_rm64_r64, Register.R10, Register.RBX, Register.RSI };
				yield return new object[] { "C4C289 F7 D3", 5, Code.VEX_Shlx_r64_rm64_r64, Register.RDX, Register.R11, Register.R14 };
				yield return new object[] { "C442C9 F7 D3", 5, Code.VEX_Shlx_r64_rm64_r64, Register.R10, Register.R11, Register.RSI };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Sarx_Gd_Ed_Gd_1_Data))]
		void Test16_Sarx_Gd_Ed_Gd_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg2, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test16_Sarx_Gd_Ed_Gd_1_Data {
			get {
				yield return new object[] { "C4E24A F7 10", 5, Code.VEX_Sarx_r32_rm32_r32, Register.EDX, Register.ESI, MemorySize.Int32 };
				yield return new object[] { "C4E2CA F7 10", 5, Code.VEX_Sarx_r32_rm32_r32, Register.EDX, Register.ESI, MemorySize.Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Sarx_Gd_Ed_Gd_2_Data))]
		void Test16_Sarx_Gd_Ed_Gd_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test16_Sarx_Gd_Ed_Gd_2_Data {
			get {
				yield return new object[] { "C4E24A F7 D3", 5, Code.VEX_Sarx_r32_rm32_r32, Register.EDX, Register.EBX, Register.ESI };
				yield return new object[] { "C4E2CA F7 D3", 5, Code.VEX_Sarx_r32_rm32_r32, Register.EDX, Register.EBX, Register.ESI };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Sarx_Gd_Ed_Gd_1_Data))]
		void Test32_Sarx_Gd_Ed_Gd_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg2, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test32_Sarx_Gd_Ed_Gd_1_Data {
			get {
				yield return new object[] { "C4E24A F7 10", 5, Code.VEX_Sarx_r32_rm32_r32, Register.EDX, Register.ESI, MemorySize.Int32 };
				yield return new object[] { "C4E2CA F7 10", 5, Code.VEX_Sarx_r32_rm32_r32, Register.EDX, Register.ESI, MemorySize.Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Sarx_Gd_Ed_Gd_2_Data))]
		void Test32_Sarx_Gd_Ed_Gd_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test32_Sarx_Gd_Ed_Gd_2_Data {
			get {
				yield return new object[] { "C4E24A F7 D3", 5, Code.VEX_Sarx_r32_rm32_r32, Register.EDX, Register.EBX, Register.ESI };
				yield return new object[] { "C4E2CA F7 D3", 5, Code.VEX_Sarx_r32_rm32_r32, Register.EDX, Register.EBX, Register.ESI };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Sarx_Gd_Ed_Gd_1_Data))]
		void Test64_Sarx_Gd_Ed_Gd_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg2, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test64_Sarx_Gd_Ed_Gd_1_Data {
			get {
				yield return new object[] { "C4E24A F7 10", 5, Code.VEX_Sarx_r32_rm32_r32, Register.EDX, Register.ESI, MemorySize.Int32 };
				yield return new object[] { "C4E2CA F7 10", 5, Code.VEX_Sarx_r64_rm64_r64, Register.RDX, Register.RSI, MemorySize.Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Sarx_Gd_Ed_Gd_2_Data))]
		void Test64_Sarx_Gd_Ed_Gd_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test64_Sarx_Gd_Ed_Gd_2_Data {
			get {
				yield return new object[] { "C4E24A F7 D3", 5, Code.VEX_Sarx_r32_rm32_r32, Register.EDX, Register.EBX, Register.ESI };
				yield return new object[] { "C4624A F7 D3", 5, Code.VEX_Sarx_r32_rm32_r32, Register.R10D, Register.EBX, Register.ESI };
				yield return new object[] { "C4C20A F7 D3", 5, Code.VEX_Sarx_r32_rm32_r32, Register.EDX, Register.R11D, Register.R14D };
				yield return new object[] { "C4424A F7 D3", 5, Code.VEX_Sarx_r32_rm32_r32, Register.R10D, Register.R11D, Register.ESI };

				yield return new object[] { "C4E2CA F7 D3", 5, Code.VEX_Sarx_r64_rm64_r64, Register.RDX, Register.RBX, Register.RSI };
				yield return new object[] { "C462CA F7 D3", 5, Code.VEX_Sarx_r64_rm64_r64, Register.R10, Register.RBX, Register.RSI };
				yield return new object[] { "C4C28A F7 D3", 5, Code.VEX_Sarx_r64_rm64_r64, Register.RDX, Register.R11, Register.R14 };
				yield return new object[] { "C442CA F7 D3", 5, Code.VEX_Sarx_r64_rm64_r64, Register.R10, Register.R11, Register.RSI };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Shrx_Gd_Ed_Gd_1_Data))]
		void Test16_Shrx_Gd_Ed_Gd_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg2, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test16_Shrx_Gd_Ed_Gd_1_Data {
			get {
				yield return new object[] { "C4E24B F7 10", 5, Code.VEX_Shrx_r32_rm32_r32, Register.EDX, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "C4E2CB F7 10", 5, Code.VEX_Shrx_r32_rm32_r32, Register.EDX, Register.ESI, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Shrx_Gd_Ed_Gd_2_Data))]
		void Test16_Shrx_Gd_Ed_Gd_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test16_Shrx_Gd_Ed_Gd_2_Data {
			get {
				yield return new object[] { "C4E24B F7 D3", 5, Code.VEX_Shrx_r32_rm32_r32, Register.EDX, Register.EBX, Register.ESI };
				yield return new object[] { "C4E2CB F7 D3", 5, Code.VEX_Shrx_r32_rm32_r32, Register.EDX, Register.EBX, Register.ESI };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Shrx_Gd_Ed_Gd_1_Data))]
		void Test32_Shrx_Gd_Ed_Gd_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg2, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test32_Shrx_Gd_Ed_Gd_1_Data {
			get {
				yield return new object[] { "C4E24B F7 10", 5, Code.VEX_Shrx_r32_rm32_r32, Register.EDX, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "C4E2CB F7 10", 5, Code.VEX_Shrx_r32_rm32_r32, Register.EDX, Register.ESI, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Shrx_Gd_Ed_Gd_2_Data))]
		void Test32_Shrx_Gd_Ed_Gd_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test32_Shrx_Gd_Ed_Gd_2_Data {
			get {
				yield return new object[] { "C4E24B F7 D3", 5, Code.VEX_Shrx_r32_rm32_r32, Register.EDX, Register.EBX, Register.ESI };
				yield return new object[] { "C4E2CB F7 D3", 5, Code.VEX_Shrx_r32_rm32_r32, Register.EDX, Register.EBX, Register.ESI };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Shrx_Gd_Ed_Gd_1_Data))]
		void Test64_Shrx_Gd_Ed_Gd_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg2, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test64_Shrx_Gd_Ed_Gd_1_Data {
			get {
				yield return new object[] { "C4E24B F7 10", 5, Code.VEX_Shrx_r32_rm32_r32, Register.EDX, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "C4E2CB F7 10", 5, Code.VEX_Shrx_r64_rm64_r64, Register.RDX, Register.RSI, MemorySize.UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Shrx_Gd_Ed_Gd_2_Data))]
		void Test64_Shrx_Gd_Ed_Gd_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test64_Shrx_Gd_Ed_Gd_2_Data {
			get {
				yield return new object[] { "C4E24B F7 D3", 5, Code.VEX_Shrx_r32_rm32_r32, Register.EDX, Register.EBX, Register.ESI };
				yield return new object[] { "C4624B F7 D3", 5, Code.VEX_Shrx_r32_rm32_r32, Register.R10D, Register.EBX, Register.ESI };
				yield return new object[] { "C4C20B F7 D3", 5, Code.VEX_Shrx_r32_rm32_r32, Register.EDX, Register.R11D, Register.R14D };
				yield return new object[] { "C4424B F7 D3", 5, Code.VEX_Shrx_r32_rm32_r32, Register.R10D, Register.R11D, Register.ESI };

				yield return new object[] { "C4E2CB F7 D3", 5, Code.VEX_Shrx_r64_rm64_r64, Register.RDX, Register.RBX, Register.RSI };
				yield return new object[] { "C462CB F7 D3", 5, Code.VEX_Shrx_r64_rm64_r64, Register.R10, Register.RBX, Register.RSI };
				yield return new object[] { "C4C28B F7 D3", 5, Code.VEX_Shrx_r64_rm64_r64, Register.RDX, Register.R11, Register.R14 };
				yield return new object[] { "C442CB F7 D3", 5, Code.VEX_Shrx_r64_rm64_r64, Register.R10, Register.R11, Register.RSI };
			}
		}
	}
}
