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
	public sealed class DecoderTest_1_A0_A7 : DecoderTest {
		[Theory]
		[InlineData("A0 1234", 3, 0x3412, 2, Register.DS, Register.None)]
		[InlineData("26 A0 1234", 4, 0x3412, 2, Register.ES, Register.ES)]
		[InlineData("2E A0 1234", 4, 0x3412, 2, Register.CS, Register.CS)]
		[InlineData("36 A0 1234", 4, 0x3412, 2, Register.SS, Register.SS)]
		[InlineData("3E A0 1234", 4, 0x3412, 2, Register.DS, Register.DS)]
		[InlineData("64 A0 1234", 4, 0x3412, 2, Register.FS, Register.FS)]
		[InlineData("65 A0 1234", 4, 0x3412, 2, Register.GS, Register.GS)]
		[InlineData("67 A0 12345678", 6, 0x78563412, 4, Register.DS, Register.None)]
		[InlineData("26 67 A0 12345678", 7, 0x78563412, 4, Register.ES, Register.ES)]
		[InlineData("2E 67 A0 12345678", 7, 0x78563412, 4, Register.CS, Register.CS)]
		[InlineData("36 67 A0 12345678", 7, 0x78563412, 4, Register.SS, Register.SS)]
		[InlineData("3E 67 A0 12345678", 7, 0x78563412, 4, Register.DS, Register.DS)]
		[InlineData("64 67 A0 12345678", 7, 0x78563412, 4, Register.FS, Register.FS)]
		[InlineData("65 67 A0 12345678", 7, 0x78563412, 4, Register.GS, Register.GS)]
		void Test16_Mov_AL_Ob_1(string hexBytes, int byteLength, uint memoryDisplacement, int memoryDisplSize, Register seg, Register segPrefix) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_AL_Ob, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AL, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(seg, instr.MemorySegment);
			Assert.Equal(Register.None, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(memoryDisplacement, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(memoryDisplSize, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("67 A0 1234", 4, 0x3412, 2, Register.DS, Register.None)]
		[InlineData("26 67 A0 1234", 5, 0x3412, 2, Register.ES, Register.ES)]
		[InlineData("2E 67 A0 1234", 5, 0x3412, 2, Register.CS, Register.CS)]
		[InlineData("36 67 A0 1234", 5, 0x3412, 2, Register.SS, Register.SS)]
		[InlineData("3E 67 A0 1234", 5, 0x3412, 2, Register.DS, Register.DS)]
		[InlineData("64 67 A0 1234", 5, 0x3412, 2, Register.FS, Register.FS)]
		[InlineData("65 67 A0 1234", 5, 0x3412, 2, Register.GS, Register.GS)]
		[InlineData("A0 12345678", 5, 0x78563412, 4, Register.DS, Register.None)]
		[InlineData("26 A0 12345678", 6, 0x78563412, 4, Register.ES, Register.ES)]
		[InlineData("2E A0 12345678", 6, 0x78563412, 4, Register.CS, Register.CS)]
		[InlineData("36 A0 12345678", 6, 0x78563412, 4, Register.SS, Register.SS)]
		[InlineData("3E A0 12345678", 6, 0x78563412, 4, Register.DS, Register.DS)]
		[InlineData("64 A0 12345678", 6, 0x78563412, 4, Register.FS, Register.FS)]
		[InlineData("65 A0 12345678", 6, 0x78563412, 4, Register.GS, Register.GS)]
		void Test32_Mov_AL_Ob_1(string hexBytes, int byteLength, uint memoryDisplacement, int memoryDisplSize, Register seg, Register segPrefix) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_AL_Ob, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AL, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(seg, instr.MemorySegment);
			Assert.Equal(Register.None, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(memoryDisplacement, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(memoryDisplSize, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("67 A0 12345678", 6, 0x78563412, 4, Register.DS, Register.None)]
		[InlineData("26 67 A0 12345678", 7, 0x78563412, 4, Register.ES, Register.ES)]
		[InlineData("2E 67 A0 12345678", 7, 0x78563412, 4, Register.CS, Register.CS)]
		[InlineData("36 67 A0 12345678", 7, 0x78563412, 4, Register.SS, Register.SS)]
		[InlineData("3E 67 A0 12345678", 7, 0x78563412, 4, Register.DS, Register.DS)]
		[InlineData("64 67 A0 12345678", 7, 0x78563412, 4, Register.FS, Register.FS)]
		[InlineData("65 67 A0 12345678", 7, 0x78563412, 4, Register.GS, Register.GS)]
		[InlineData("66 67 4F A0 12345678", 8, 0x78563412, 4, Register.DS, Register.None)]
		void Test64_Mov_AL_Ob_1(string hexBytes, int byteLength, uint memoryDisplacement, int memoryDisplSize, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_AL_Ob, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AL, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(seg, instr.MemorySegment);
			Assert.Equal(Register.None, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(memoryDisplacement, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(memoryDisplSize, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("A0 123456789ABCDEF0", 9, 0xF0DEBC9A78563412, Register.DS, Register.None)]
		[InlineData("26 A0 123456789ABCDEF0", 10, 0xF0DEBC9A78563412, Register.ES, Register.ES)]
		[InlineData("2E A0 123456789ABCDEF0", 10, 0xF0DEBC9A78563412, Register.CS, Register.CS)]
		[InlineData("36 A0 123456789ABCDEF0", 10, 0xF0DEBC9A78563412, Register.SS, Register.SS)]
		[InlineData("3E A0 123456789ABCDEF0", 10, 0xF0DEBC9A78563412, Register.DS, Register.DS)]
		[InlineData("64 A0 123456789ABCDEF0", 10, 0xF0DEBC9A78563412, Register.FS, Register.FS)]
		[InlineData("65 A0 123456789ABCDEF0", 10, 0xF0DEBC9A78563412, Register.GS, Register.GS)]
		[InlineData("66 4F A0 123456789ABCDEF0", 11, 0xF0DEBC9A78563412, Register.DS, Register.None)]
		void Test64_Mov_AL_Ob_2(string hexBytes, int byteLength, ulong memoryAddress64, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_AL_Ob, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AL, instr.Op0Register);

			Assert.Equal(OpKind.Memory64, instr.Op1Kind);
			Assert.Equal(seg, instr.MemorySegment);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(memoryAddress64, instr.MemoryAddress64);
			Assert.Equal(8, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("A1 1234", 3, 0x3412, 2, Register.DS, Register.None)]
		[InlineData("26 A1 1234", 4, 0x3412, 2, Register.ES, Register.ES)]
		[InlineData("2E A1 1234", 4, 0x3412, 2, Register.CS, Register.CS)]
		[InlineData("36 A1 1234", 4, 0x3412, 2, Register.SS, Register.SS)]
		[InlineData("3E A1 1234", 4, 0x3412, 2, Register.DS, Register.DS)]
		[InlineData("64 A1 1234", 4, 0x3412, 2, Register.FS, Register.FS)]
		[InlineData("65 A1 1234", 4, 0x3412, 2, Register.GS, Register.GS)]
		[InlineData("67 A1 12345678", 6, 0x78563412, 4, Register.DS, Register.None)]
		[InlineData("26 67 A1 12345678", 7, 0x78563412, 4, Register.ES, Register.ES)]
		[InlineData("2E 67 A1 12345678", 7, 0x78563412, 4, Register.CS, Register.CS)]
		[InlineData("36 67 A1 12345678", 7, 0x78563412, 4, Register.SS, Register.SS)]
		[InlineData("3E 67 A1 12345678", 7, 0x78563412, 4, Register.DS, Register.DS)]
		[InlineData("64 67 A1 12345678", 7, 0x78563412, 4, Register.FS, Register.FS)]
		[InlineData("65 67 A1 12345678", 7, 0x78563412, 4, Register.GS, Register.GS)]
		void Test16_Mov_AX_Ow_1(string hexBytes, int byteLength, uint memoryDisplacement, int memoryDisplSize, Register seg, Register segPrefix) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_AX_Ow, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AX, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(seg, instr.MemorySegment);
			Assert.Equal(Register.None, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(memoryDisplacement, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(memoryDisplSize, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("66 67 A1 1234", 5, 0x3412, 2, Register.DS, Register.None)]
		[InlineData("26 66 67 A1 1234", 6, 0x3412, 2, Register.ES, Register.ES)]
		[InlineData("2E 66 67 A1 1234", 6, 0x3412, 2, Register.CS, Register.CS)]
		[InlineData("36 66 67 A1 1234", 6, 0x3412, 2, Register.SS, Register.SS)]
		[InlineData("3E 66 67 A1 1234", 6, 0x3412, 2, Register.DS, Register.DS)]
		[InlineData("64 66 67 A1 1234", 6, 0x3412, 2, Register.FS, Register.FS)]
		[InlineData("65 66 67 A1 1234", 6, 0x3412, 2, Register.GS, Register.GS)]
		[InlineData("66 A1 12345678", 6, 0x78563412, 4, Register.DS, Register.None)]
		[InlineData("26 66 A1 12345678", 7, 0x78563412, 4, Register.ES, Register.ES)]
		[InlineData("2E 66 A1 12345678", 7, 0x78563412, 4, Register.CS, Register.CS)]
		[InlineData("36 66 A1 12345678", 7, 0x78563412, 4, Register.SS, Register.SS)]
		[InlineData("3E 66 A1 12345678", 7, 0x78563412, 4, Register.DS, Register.DS)]
		[InlineData("64 66 A1 12345678", 7, 0x78563412, 4, Register.FS, Register.FS)]
		[InlineData("65 66 A1 12345678", 7, 0x78563412, 4, Register.GS, Register.GS)]
		void Test32_Mov_AX_Ow_1(string hexBytes, int byteLength, uint memoryDisplacement, int memoryDisplSize, Register seg, Register segPrefix) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_AX_Ow, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AX, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(seg, instr.MemorySegment);
			Assert.Equal(Register.None, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(memoryDisplacement, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(memoryDisplSize, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("66 67 A1 12345678", 7, 0x78563412, 4, Register.DS, Register.None)]
		[InlineData("26 66 67 A1 12345678", 8, 0x78563412, 4, Register.ES, Register.ES)]
		[InlineData("2E 66 67 A1 12345678", 8, 0x78563412, 4, Register.CS, Register.CS)]
		[InlineData("36 66 67 A1 12345678", 8, 0x78563412, 4, Register.SS, Register.SS)]
		[InlineData("3E 66 67 A1 12345678", 8, 0x78563412, 4, Register.DS, Register.DS)]
		[InlineData("64 66 67 A1 12345678", 8, 0x78563412, 4, Register.FS, Register.FS)]
		[InlineData("65 66 67 A1 12345678", 8, 0x78563412, 4, Register.GS, Register.GS)]
		[InlineData("67 66 47 A1 12345678", 8, 0x78563412, 4, Register.DS, Register.None)]
		void Test64_Mov_AX_Ow_1(string hexBytes, int byteLength, uint memoryDisplacement, int memoryDisplSize, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_AX_Ow, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AX, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(seg, instr.MemorySegment);
			Assert.Equal(Register.None, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(memoryDisplacement, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(memoryDisplSize, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("66 A1 123456789ABCDEF0", 10, 0xF0DEBC9A78563412, Register.DS, Register.None)]
		[InlineData("26 66 A1 123456789ABCDEF0", 11, 0xF0DEBC9A78563412, Register.ES, Register.ES)]
		[InlineData("2E 66 A1 123456789ABCDEF0", 11, 0xF0DEBC9A78563412, Register.CS, Register.CS)]
		[InlineData("36 66 A1 123456789ABCDEF0", 11, 0xF0DEBC9A78563412, Register.SS, Register.SS)]
		[InlineData("3E 66 A1 123456789ABCDEF0", 11, 0xF0DEBC9A78563412, Register.DS, Register.DS)]
		[InlineData("64 66 A1 123456789ABCDEF0", 11, 0xF0DEBC9A78563412, Register.FS, Register.FS)]
		[InlineData("65 66 A1 123456789ABCDEF0", 11, 0xF0DEBC9A78563412, Register.GS, Register.GS)]
		[InlineData("66 47 A1 123456789ABCDEF0", 11, 0xF0DEBC9A78563412, Register.DS, Register.None)]
		void Test64_Mov_AX_Ow_2(string hexBytes, int byteLength, ulong memoryAddress64, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_AX_Ow, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.AX, instr.Op0Register);

			Assert.Equal(OpKind.Memory64, instr.Op1Kind);
			Assert.Equal(seg, instr.MemorySegment);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(memoryAddress64, instr.MemoryAddress64);
			Assert.Equal(8, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("66 A1 1234", 4, 0x3412, 2, Register.DS, Register.None)]
		[InlineData("26 66 A1 1234", 5, 0x3412, 2, Register.ES, Register.ES)]
		[InlineData("2E 66 A1 1234", 5, 0x3412, 2, Register.CS, Register.CS)]
		[InlineData("36 66 A1 1234", 5, 0x3412, 2, Register.SS, Register.SS)]
		[InlineData("3E 66 A1 1234", 5, 0x3412, 2, Register.DS, Register.DS)]
		[InlineData("64 66 A1 1234", 5, 0x3412, 2, Register.FS, Register.FS)]
		[InlineData("65 66 A1 1234", 5, 0x3412, 2, Register.GS, Register.GS)]
		[InlineData("67 66 A1 12345678", 7, 0x78563412, 4, Register.DS, Register.None)]
		[InlineData("26 66 67 A1 12345678", 8, 0x78563412, 4, Register.ES, Register.ES)]
		[InlineData("2E 66 67 A1 12345678", 8, 0x78563412, 4, Register.CS, Register.CS)]
		[InlineData("36 66 67 A1 12345678", 8, 0x78563412, 4, Register.SS, Register.SS)]
		[InlineData("3E 66 67 A1 12345678", 8, 0x78563412, 4, Register.DS, Register.DS)]
		[InlineData("64 66 67 A1 12345678", 8, 0x78563412, 4, Register.FS, Register.FS)]
		[InlineData("65 66 67 A1 12345678", 8, 0x78563412, 4, Register.GS, Register.GS)]
		void Test16_Mov_EAX_Od_1(string hexBytes, int byteLength, uint memoryDisplacement, int memoryDisplSize, Register seg, Register segPrefix) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_EAX_Od, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EAX, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(seg, instr.MemorySegment);
			Assert.Equal(Register.None, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(memoryDisplacement, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(memoryDisplSize, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("67 A1 1234", 4, 0x3412, 2, Register.DS, Register.None)]
		[InlineData("26 67 A1 1234", 5, 0x3412, 2, Register.ES, Register.ES)]
		[InlineData("2E 67 A1 1234", 5, 0x3412, 2, Register.CS, Register.CS)]
		[InlineData("36 67 A1 1234", 5, 0x3412, 2, Register.SS, Register.SS)]
		[InlineData("3E 67 A1 1234", 5, 0x3412, 2, Register.DS, Register.DS)]
		[InlineData("64 67 A1 1234", 5, 0x3412, 2, Register.FS, Register.FS)]
		[InlineData("65 67 A1 1234", 5, 0x3412, 2, Register.GS, Register.GS)]
		[InlineData("A1 12345678", 5, 0x78563412, 4, Register.DS, Register.None)]
		[InlineData("26 A1 12345678", 6, 0x78563412, 4, Register.ES, Register.ES)]
		[InlineData("2E A1 12345678", 6, 0x78563412, 4, Register.CS, Register.CS)]
		[InlineData("36 A1 12345678", 6, 0x78563412, 4, Register.SS, Register.SS)]
		[InlineData("3E A1 12345678", 6, 0x78563412, 4, Register.DS, Register.DS)]
		[InlineData("64 A1 12345678", 6, 0x78563412, 4, Register.FS, Register.FS)]
		[InlineData("65 A1 12345678", 6, 0x78563412, 4, Register.GS, Register.GS)]
		void Test32_Mov_EAX_Od_1(string hexBytes, int byteLength, uint memoryDisplacement, int memoryDisplSize, Register seg, Register segPrefix) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_EAX_Od, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EAX, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(seg, instr.MemorySegment);
			Assert.Equal(Register.None, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(memoryDisplacement, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(memoryDisplSize, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("67 A1 12345678", 6, 0x78563412, 4, Register.DS, Register.None)]
		[InlineData("26 67 A1 12345678", 7, 0x78563412, 4, Register.ES, Register.ES)]
		[InlineData("2E 67 A1 12345678", 7, 0x78563412, 4, Register.CS, Register.CS)]
		[InlineData("36 67 A1 12345678", 7, 0x78563412, 4, Register.SS, Register.SS)]
		[InlineData("3E 67 A1 12345678", 7, 0x78563412, 4, Register.DS, Register.DS)]
		[InlineData("64 67 A1 12345678", 7, 0x78563412, 4, Register.FS, Register.FS)]
		[InlineData("65 67 A1 12345678", 7, 0x78563412, 4, Register.GS, Register.GS)]
		[InlineData("67 47 A1 12345678", 7, 0x78563412, 4, Register.DS, Register.None)]
		void Test64_Mov_EAX_Od_1(string hexBytes, int byteLength, uint memoryDisplacement, int memoryDisplSize, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_EAX_Od, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EAX, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(seg, instr.MemorySegment);
			Assert.Equal(Register.None, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(memoryDisplacement, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(memoryDisplSize, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("A1 123456789ABCDEF0", 9, 0xF0DEBC9A78563412, Register.DS, Register.None)]
		[InlineData("26 A1 123456789ABCDEF0", 10, 0xF0DEBC9A78563412, Register.ES, Register.ES)]
		[InlineData("2E A1 123456789ABCDEF0", 10, 0xF0DEBC9A78563412, Register.CS, Register.CS)]
		[InlineData("36 A1 123456789ABCDEF0", 10, 0xF0DEBC9A78563412, Register.SS, Register.SS)]
		[InlineData("3E A1 123456789ABCDEF0", 10, 0xF0DEBC9A78563412, Register.DS, Register.DS)]
		[InlineData("64 A1 123456789ABCDEF0", 10, 0xF0DEBC9A78563412, Register.FS, Register.FS)]
		[InlineData("65 A1 123456789ABCDEF0", 10, 0xF0DEBC9A78563412, Register.GS, Register.GS)]
		[InlineData("47 A1 123456789ABCDEF0", 10, 0xF0DEBC9A78563412, Register.DS, Register.None)]
		void Test64_Mov_EAX_Od_2(string hexBytes, int byteLength, ulong memoryAddress64, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_EAX_Od, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EAX, instr.Op0Register);

			Assert.Equal(OpKind.Memory64, instr.Op1Kind);
			Assert.Equal(seg, instr.MemorySegment);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(memoryAddress64, instr.MemoryAddress64);
			Assert.Equal(8, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("67 48 A1 12345678", 7, 0x78563412, 4, Register.DS, Register.None)]
		[InlineData("26 67 48 A1 12345678", 8, 0x78563412, 4, Register.ES, Register.ES)]
		[InlineData("2E 67 48 A1 12345678", 8, 0x78563412, 4, Register.CS, Register.CS)]
		[InlineData("36 67 48 A1 12345678", 8, 0x78563412, 4, Register.SS, Register.SS)]
		[InlineData("3E 67 48 A1 12345678", 8, 0x78563412, 4, Register.DS, Register.DS)]
		[InlineData("64 67 48 A1 12345678", 8, 0x78563412, 4, Register.FS, Register.FS)]
		[InlineData("65 67 48 A1 12345678", 8, 0x78563412, 4, Register.GS, Register.GS)]
		[InlineData("66 67 4F A1 12345678", 8, 0x78563412, 4, Register.DS, Register.None)]
		void Test64_Mov_RAX_Oq_1(string hexBytes, int byteLength, uint memoryDisplacement, int memoryDisplSize, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_RAX_Oq, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.RAX, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(seg, instr.MemorySegment);
			Assert.Equal(Register.None, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(memoryDisplacement, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt64, instr.MemorySize);
			Assert.Equal(memoryDisplSize, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("48 A1 123456789ABCDEF0", 10, 0xF0DEBC9A78563412, Register.DS, Register.None)]
		[InlineData("26 48 A1 123456789ABCDEF0", 11, 0xF0DEBC9A78563412, Register.ES, Register.ES)]
		[InlineData("2E 48 A1 123456789ABCDEF0", 11, 0xF0DEBC9A78563412, Register.CS, Register.CS)]
		[InlineData("36 48 A1 123456789ABCDEF0", 11, 0xF0DEBC9A78563412, Register.SS, Register.SS)]
		[InlineData("3E 48 A1 123456789ABCDEF0", 11, 0xF0DEBC9A78563412, Register.DS, Register.DS)]
		[InlineData("64 48 A1 123456789ABCDEF0", 11, 0xF0DEBC9A78563412, Register.FS, Register.FS)]
		[InlineData("65 48 A1 123456789ABCDEF0", 11, 0xF0DEBC9A78563412, Register.GS, Register.GS)]
		[InlineData("66 4F A1 123456789ABCDEF0", 11, 0xF0DEBC9A78563412, Register.DS, Register.None)]
		void Test64_Mov_RAX_Oq_2(string hexBytes, int byteLength, ulong memoryAddress64, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_RAX_Oq, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.RAX, instr.Op0Register);

			Assert.Equal(OpKind.Memory64, instr.Op1Kind);
			Assert.Equal(seg, instr.MemorySegment);
			Assert.Equal(MemorySize.UInt64, instr.MemorySize);
			Assert.Equal(memoryAddress64, instr.MemoryAddress64);
			Assert.Equal(8, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("A2 1234", 3, 0x3412, 2, Register.DS, Register.None)]
		[InlineData("26 A2 1234", 4, 0x3412, 2, Register.ES, Register.ES)]
		[InlineData("2E A2 1234", 4, 0x3412, 2, Register.CS, Register.CS)]
		[InlineData("36 A2 1234", 4, 0x3412, 2, Register.SS, Register.SS)]
		[InlineData("3E A2 1234", 4, 0x3412, 2, Register.DS, Register.DS)]
		[InlineData("64 A2 1234", 4, 0x3412, 2, Register.FS, Register.FS)]
		[InlineData("65 A2 1234", 4, 0x3412, 2, Register.GS, Register.GS)]
		[InlineData("67 A2 12345678", 6, 0x78563412, 4, Register.DS, Register.None)]
		[InlineData("26 67 A2 12345678", 7, 0x78563412, 4, Register.ES, Register.ES)]
		[InlineData("2E 67 A2 12345678", 7, 0x78563412, 4, Register.CS, Register.CS)]
		[InlineData("36 67 A2 12345678", 7, 0x78563412, 4, Register.SS, Register.SS)]
		[InlineData("3E 67 A2 12345678", 7, 0x78563412, 4, Register.DS, Register.DS)]
		[InlineData("64 67 A2 12345678", 7, 0x78563412, 4, Register.FS, Register.FS)]
		[InlineData("65 67 A2 12345678", 7, 0x78563412, 4, Register.GS, Register.GS)]
		void Test16_Mov_Ob_AL_1(string hexBytes, int byteLength, uint memoryDisplacement, int memoryDisplSize, Register seg, Register segPrefix) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_Ob_AL, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(seg, instr.MemorySegment);
			Assert.Equal(Register.None, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(memoryDisplacement, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(memoryDisplSize, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.AL, instr.Op1Register);
		}

		[Theory]
		[InlineData("67 A2 1234", 4, 0x3412, 2, Register.DS, Register.None)]
		[InlineData("26 67 A2 1234", 5, 0x3412, 2, Register.ES, Register.ES)]
		[InlineData("2E 67 A2 1234", 5, 0x3412, 2, Register.CS, Register.CS)]
		[InlineData("36 67 A2 1234", 5, 0x3412, 2, Register.SS, Register.SS)]
		[InlineData("3E 67 A2 1234", 5, 0x3412, 2, Register.DS, Register.DS)]
		[InlineData("64 67 A2 1234", 5, 0x3412, 2, Register.FS, Register.FS)]
		[InlineData("65 67 A2 1234", 5, 0x3412, 2, Register.GS, Register.GS)]
		[InlineData("A2 12345678", 5, 0x78563412, 4, Register.DS, Register.None)]
		[InlineData("26 A2 12345678", 6, 0x78563412, 4, Register.ES, Register.ES)]
		[InlineData("2E A2 12345678", 6, 0x78563412, 4, Register.CS, Register.CS)]
		[InlineData("36 A2 12345678", 6, 0x78563412, 4, Register.SS, Register.SS)]
		[InlineData("3E A2 12345678", 6, 0x78563412, 4, Register.DS, Register.DS)]
		[InlineData("64 A2 12345678", 6, 0x78563412, 4, Register.FS, Register.FS)]
		[InlineData("65 A2 12345678", 6, 0x78563412, 4, Register.GS, Register.GS)]
		void Test32_Mov_Ob_AL_1(string hexBytes, int byteLength, uint memoryDisplacement, int memoryDisplSize, Register seg, Register segPrefix) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_Ob_AL, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(seg, instr.MemorySegment);
			Assert.Equal(Register.None, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(memoryDisplacement, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(memoryDisplSize, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.AL, instr.Op1Register);
		}

		[Theory]
		[InlineData("67 A2 12345678", 6, 0x78563412, 4, Register.DS, Register.None)]
		[InlineData("26 67 A2 12345678", 7, 0x78563412, 4, Register.ES, Register.ES)]
		[InlineData("2E 67 A2 12345678", 7, 0x78563412, 4, Register.CS, Register.CS)]
		[InlineData("36 67 A2 12345678", 7, 0x78563412, 4, Register.SS, Register.SS)]
		[InlineData("3E 67 A2 12345678", 7, 0x78563412, 4, Register.DS, Register.DS)]
		[InlineData("64 67 A2 12345678", 7, 0x78563412, 4, Register.FS, Register.FS)]
		[InlineData("65 67 A2 12345678", 7, 0x78563412, 4, Register.GS, Register.GS)]
		[InlineData("66 67 4F A2 12345678", 8, 0x78563412, 4, Register.DS, Register.None)]
		void Test64_Mov_Ob_AL_1(string hexBytes, int byteLength, uint memoryDisplacement, int memoryDisplSize, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_Ob_AL, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(seg, instr.MemorySegment);
			Assert.Equal(Register.None, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(memoryDisplacement, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(memoryDisplSize, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.AL, instr.Op1Register);
		}

		[Theory]
		[InlineData("A2 123456789ABCDEF0", 9, 0xF0DEBC9A78563412, Register.DS, Register.None)]
		[InlineData("26 A2 123456789ABCDEF0", 10, 0xF0DEBC9A78563412, Register.ES, Register.ES)]
		[InlineData("2E A2 123456789ABCDEF0", 10, 0xF0DEBC9A78563412, Register.CS, Register.CS)]
		[InlineData("36 A2 123456789ABCDEF0", 10, 0xF0DEBC9A78563412, Register.SS, Register.SS)]
		[InlineData("3E A2 123456789ABCDEF0", 10, 0xF0DEBC9A78563412, Register.DS, Register.DS)]
		[InlineData("64 A2 123456789ABCDEF0", 10, 0xF0DEBC9A78563412, Register.FS, Register.FS)]
		[InlineData("65 A2 123456789ABCDEF0", 10, 0xF0DEBC9A78563412, Register.GS, Register.GS)]
		[InlineData("66 4F A2 123456789ABCDEF0", 11, 0xF0DEBC9A78563412, Register.DS, Register.None)]
		void Test64_Mov_Ob_AL_2(string hexBytes, int byteLength, ulong memoryAddress64, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_Ob_AL, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory64, instr.Op0Kind);
			Assert.Equal(seg, instr.MemorySegment);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(memoryAddress64, instr.MemoryAddress64);
			Assert.Equal(8, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.AL, instr.Op1Register);
		}

		[Theory]
		[InlineData("A3 1234", 3, 0x3412, 2, Register.DS, Register.None)]
		[InlineData("26 A3 1234", 4, 0x3412, 2, Register.ES, Register.ES)]
		[InlineData("2E A3 1234", 4, 0x3412, 2, Register.CS, Register.CS)]
		[InlineData("36 A3 1234", 4, 0x3412, 2, Register.SS, Register.SS)]
		[InlineData("3E A3 1234", 4, 0x3412, 2, Register.DS, Register.DS)]
		[InlineData("64 A3 1234", 4, 0x3412, 2, Register.FS, Register.FS)]
		[InlineData("65 A3 1234", 4, 0x3412, 2, Register.GS, Register.GS)]
		[InlineData("67 A3 12345678", 6, 0x78563412, 4, Register.DS, Register.None)]
		[InlineData("26 67 A3 12345678", 7, 0x78563412, 4, Register.ES, Register.ES)]
		[InlineData("2E 67 A3 12345678", 7, 0x78563412, 4, Register.CS, Register.CS)]
		[InlineData("36 67 A3 12345678", 7, 0x78563412, 4, Register.SS, Register.SS)]
		[InlineData("3E 67 A3 12345678", 7, 0x78563412, 4, Register.DS, Register.DS)]
		[InlineData("64 67 A3 12345678", 7, 0x78563412, 4, Register.FS, Register.FS)]
		[InlineData("65 67 A3 12345678", 7, 0x78563412, 4, Register.GS, Register.GS)]
		void Test16_Mov_Ow_AX_1(string hexBytes, int byteLength, uint memoryDisplacement, int memoryDisplSize, Register seg, Register segPrefix) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_Ow_AX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(seg, instr.MemorySegment);
			Assert.Equal(Register.None, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(memoryDisplacement, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(memoryDisplSize, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.AX, instr.Op1Register);
		}

		[Theory]
		[InlineData("66 67 A3 1234", 5, 0x3412, 2, Register.DS, Register.None)]
		[InlineData("26 66 67 A3 1234", 6, 0x3412, 2, Register.ES, Register.ES)]
		[InlineData("2E 66 67 A3 1234", 6, 0x3412, 2, Register.CS, Register.CS)]
		[InlineData("36 66 67 A3 1234", 6, 0x3412, 2, Register.SS, Register.SS)]
		[InlineData("3E 66 67 A3 1234", 6, 0x3412, 2, Register.DS, Register.DS)]
		[InlineData("64 66 67 A3 1234", 6, 0x3412, 2, Register.FS, Register.FS)]
		[InlineData("65 66 67 A3 1234", 6, 0x3412, 2, Register.GS, Register.GS)]
		[InlineData("66 A3 12345678", 6, 0x78563412, 4, Register.DS, Register.None)]
		[InlineData("26 66 A3 12345678", 7, 0x78563412, 4, Register.ES, Register.ES)]
		[InlineData("2E 66 A3 12345678", 7, 0x78563412, 4, Register.CS, Register.CS)]
		[InlineData("36 66 A3 12345678", 7, 0x78563412, 4, Register.SS, Register.SS)]
		[InlineData("3E 66 A3 12345678", 7, 0x78563412, 4, Register.DS, Register.DS)]
		[InlineData("64 66 A3 12345678", 7, 0x78563412, 4, Register.FS, Register.FS)]
		[InlineData("65 66 A3 12345678", 7, 0x78563412, 4, Register.GS, Register.GS)]
		void Test32_Mov_Ow_AX_1(string hexBytes, int byteLength, uint memoryDisplacement, int memoryDisplSize, Register seg, Register segPrefix) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_Ow_AX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(seg, instr.MemorySegment);
			Assert.Equal(Register.None, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(memoryDisplacement, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(memoryDisplSize, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.AX, instr.Op1Register);
		}

		[Theory]
		[InlineData("66 67 A3 12345678", 7, 0x78563412, 4, Register.DS, Register.None)]
		[InlineData("26 66 67 A3 12345678", 8, 0x78563412, 4, Register.ES, Register.ES)]
		[InlineData("2E 66 67 A3 12345678", 8, 0x78563412, 4, Register.CS, Register.CS)]
		[InlineData("36 66 67 A3 12345678", 8, 0x78563412, 4, Register.SS, Register.SS)]
		[InlineData("3E 66 67 A3 12345678", 8, 0x78563412, 4, Register.DS, Register.DS)]
		[InlineData("64 66 67 A3 12345678", 8, 0x78563412, 4, Register.FS, Register.FS)]
		[InlineData("65 66 67 A3 12345678", 8, 0x78563412, 4, Register.GS, Register.GS)]
		[InlineData("67 66 47 A3 12345678", 8, 0x78563412, 4, Register.DS, Register.None)]
		void Test64_Mov_Ow_AX_1(string hexBytes, int byteLength, uint memoryDisplacement, int memoryDisplSize, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_Ow_AX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(seg, instr.MemorySegment);
			Assert.Equal(Register.None, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(memoryDisplacement, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(memoryDisplSize, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.AX, instr.Op1Register);
		}

		[Theory]
		[InlineData("66 A3 123456789ABCDEF0", 10, 0xF0DEBC9A78563412, Register.DS, Register.None)]
		[InlineData("26 66 A3 123456789ABCDEF0", 11, 0xF0DEBC9A78563412, Register.ES, Register.ES)]
		[InlineData("2E 66 A3 123456789ABCDEF0", 11, 0xF0DEBC9A78563412, Register.CS, Register.CS)]
		[InlineData("36 66 A3 123456789ABCDEF0", 11, 0xF0DEBC9A78563412, Register.SS, Register.SS)]
		[InlineData("3E 66 A3 123456789ABCDEF0", 11, 0xF0DEBC9A78563412, Register.DS, Register.DS)]
		[InlineData("64 66 A3 123456789ABCDEF0", 11, 0xF0DEBC9A78563412, Register.FS, Register.FS)]
		[InlineData("65 66 A3 123456789ABCDEF0", 11, 0xF0DEBC9A78563412, Register.GS, Register.GS)]
		[InlineData("66 47 A3 123456789ABCDEF0", 11, 0xF0DEBC9A78563412, Register.DS, Register.None)]
		void Test64_Mov_Ow_AX_2(string hexBytes, int byteLength, ulong memoryAddress64, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_Ow_AX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory64, instr.Op0Kind);
			Assert.Equal(seg, instr.MemorySegment);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(memoryAddress64, instr.MemoryAddress64);
			Assert.Equal(8, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.AX, instr.Op1Register);
		}

		[Theory]
		[InlineData("66 A3 1234", 4, 0x3412, 2, Register.DS, Register.None)]
		[InlineData("26 66 A3 1234", 5, 0x3412, 2, Register.ES, Register.ES)]
		[InlineData("2E 66 A3 1234", 5, 0x3412, 2, Register.CS, Register.CS)]
		[InlineData("36 66 A3 1234", 5, 0x3412, 2, Register.SS, Register.SS)]
		[InlineData("3E 66 A3 1234", 5, 0x3412, 2, Register.DS, Register.DS)]
		[InlineData("64 66 A3 1234", 5, 0x3412, 2, Register.FS, Register.FS)]
		[InlineData("65 66 A3 1234", 5, 0x3412, 2, Register.GS, Register.GS)]
		[InlineData("67 66 A3 12345678", 7, 0x78563412, 4, Register.DS, Register.None)]
		[InlineData("26 66 67 A3 12345678", 8, 0x78563412, 4, Register.ES, Register.ES)]
		[InlineData("2E 66 67 A3 12345678", 8, 0x78563412, 4, Register.CS, Register.CS)]
		[InlineData("36 66 67 A3 12345678", 8, 0x78563412, 4, Register.SS, Register.SS)]
		[InlineData("3E 66 67 A3 12345678", 8, 0x78563412, 4, Register.DS, Register.DS)]
		[InlineData("64 66 67 A3 12345678", 8, 0x78563412, 4, Register.FS, Register.FS)]
		[InlineData("65 66 67 A3 12345678", 8, 0x78563412, 4, Register.GS, Register.GS)]
		void Test16_Mov_Od_EAX_1(string hexBytes, int byteLength, uint memoryDisplacement, int memoryDisplSize, Register seg, Register segPrefix) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_Od_EAX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(seg, instr.MemorySegment);
			Assert.Equal(Register.None, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(memoryDisplacement, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(memoryDisplSize, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EAX, instr.Op1Register);
		}

		[Theory]
		[InlineData("67 A3 1234", 4, 0x3412, 2, Register.DS, Register.None)]
		[InlineData("26 67 A3 1234", 5, 0x3412, 2, Register.ES, Register.ES)]
		[InlineData("2E 67 A3 1234", 5, 0x3412, 2, Register.CS, Register.CS)]
		[InlineData("36 67 A3 1234", 5, 0x3412, 2, Register.SS, Register.SS)]
		[InlineData("3E 67 A3 1234", 5, 0x3412, 2, Register.DS, Register.DS)]
		[InlineData("64 67 A3 1234", 5, 0x3412, 2, Register.FS, Register.FS)]
		[InlineData("65 67 A3 1234", 5, 0x3412, 2, Register.GS, Register.GS)]
		[InlineData("A3 12345678", 5, 0x78563412, 4, Register.DS, Register.None)]
		[InlineData("26 A3 12345678", 6, 0x78563412, 4, Register.ES, Register.ES)]
		[InlineData("2E A3 12345678", 6, 0x78563412, 4, Register.CS, Register.CS)]
		[InlineData("36 A3 12345678", 6, 0x78563412, 4, Register.SS, Register.SS)]
		[InlineData("3E A3 12345678", 6, 0x78563412, 4, Register.DS, Register.DS)]
		[InlineData("64 A3 12345678", 6, 0x78563412, 4, Register.FS, Register.FS)]
		[InlineData("65 A3 12345678", 6, 0x78563412, 4, Register.GS, Register.GS)]
		void Test32_Mov_Od_EAX_1(string hexBytes, int byteLength, uint memoryDisplacement, int memoryDisplSize, Register seg, Register segPrefix) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_Od_EAX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(seg, instr.MemorySegment);
			Assert.Equal(Register.None, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(memoryDisplacement, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(memoryDisplSize, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EAX, instr.Op1Register);
		}

		[Theory]
		[InlineData("67 A3 12345678", 6, 0x78563412, 4, Register.DS, Register.None)]
		[InlineData("26 67 A3 12345678", 7, 0x78563412, 4, Register.ES, Register.ES)]
		[InlineData("2E 67 A3 12345678", 7, 0x78563412, 4, Register.CS, Register.CS)]
		[InlineData("36 67 A3 12345678", 7, 0x78563412, 4, Register.SS, Register.SS)]
		[InlineData("3E 67 A3 12345678", 7, 0x78563412, 4, Register.DS, Register.DS)]
		[InlineData("64 67 A3 12345678", 7, 0x78563412, 4, Register.FS, Register.FS)]
		[InlineData("65 67 A3 12345678", 7, 0x78563412, 4, Register.GS, Register.GS)]
		[InlineData("67 47 A3 12345678", 7, 0x78563412, 4, Register.DS, Register.None)]
		void Test64_Mov_Od_EAX_1(string hexBytes, int byteLength, uint memoryDisplacement, int memoryDisplSize, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_Od_EAX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(seg, instr.MemorySegment);
			Assert.Equal(Register.None, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(memoryDisplacement, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(memoryDisplSize, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EAX, instr.Op1Register);
		}

		[Theory]
		[InlineData("A3 123456789ABCDEF0", 9, 0xF0DEBC9A78563412, Register.DS, Register.None)]
		[InlineData("26 A3 123456789ABCDEF0", 10, 0xF0DEBC9A78563412, Register.ES, Register.ES)]
		[InlineData("2E A3 123456789ABCDEF0", 10, 0xF0DEBC9A78563412, Register.CS, Register.CS)]
		[InlineData("36 A3 123456789ABCDEF0", 10, 0xF0DEBC9A78563412, Register.SS, Register.SS)]
		[InlineData("3E A3 123456789ABCDEF0", 10, 0xF0DEBC9A78563412, Register.DS, Register.DS)]
		[InlineData("64 A3 123456789ABCDEF0", 10, 0xF0DEBC9A78563412, Register.FS, Register.FS)]
		[InlineData("65 A3 123456789ABCDEF0", 10, 0xF0DEBC9A78563412, Register.GS, Register.GS)]
		[InlineData("47 A3 123456789ABCDEF0", 10, 0xF0DEBC9A78563412, Register.DS, Register.None)]
		void Test64_Mov_Od_EAX_2(string hexBytes, int byteLength, ulong memoryAddress64, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_Od_EAX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory64, instr.Op0Kind);
			Assert.Equal(seg, instr.MemorySegment);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(memoryAddress64, instr.MemoryAddress64);
			Assert.Equal(8, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EAX, instr.Op1Register);
		}

		[Theory]
		[InlineData("67 48 A3 12345678", 7, 0x78563412, 4, Register.DS, Register.None)]
		[InlineData("26 67 48 A3 12345678", 8, 0x78563412, 4, Register.ES, Register.ES)]
		[InlineData("2E 67 48 A3 12345678", 8, 0x78563412, 4, Register.CS, Register.CS)]
		[InlineData("36 67 48 A3 12345678", 8, 0x78563412, 4, Register.SS, Register.SS)]
		[InlineData("3E 67 48 A3 12345678", 8, 0x78563412, 4, Register.DS, Register.DS)]
		[InlineData("64 67 48 A3 12345678", 8, 0x78563412, 4, Register.FS, Register.FS)]
		[InlineData("65 67 48 A3 12345678", 8, 0x78563412, 4, Register.GS, Register.GS)]
		[InlineData("66 67 4F A3 12345678", 8, 0x78563412, 4, Register.DS, Register.None)]
		void Test64_Mov_Oq_RAX_1(string hexBytes, int byteLength, uint memoryDisplacement, int memoryDisplSize, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_Oq_RAX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(seg, instr.MemorySegment);
			Assert.Equal(Register.None, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(memoryDisplacement, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt64, instr.MemorySize);
			Assert.Equal(memoryDisplSize, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.RAX, instr.Op1Register);
		}

		[Theory]
		[InlineData("48 A3 123456789ABCDEF0", 10, 0xF0DEBC9A78563412, Register.DS, Register.None)]
		[InlineData("26 48 A3 123456789ABCDEF0", 11, 0xF0DEBC9A78563412, Register.ES, Register.ES)]
		[InlineData("2E 48 A3 123456789ABCDEF0", 11, 0xF0DEBC9A78563412, Register.CS, Register.CS)]
		[InlineData("36 48 A3 123456789ABCDEF0", 11, 0xF0DEBC9A78563412, Register.SS, Register.SS)]
		[InlineData("3E 48 A3 123456789ABCDEF0", 11, 0xF0DEBC9A78563412, Register.DS, Register.DS)]
		[InlineData("64 48 A3 123456789ABCDEF0", 11, 0xF0DEBC9A78563412, Register.FS, Register.FS)]
		[InlineData("65 48 A3 123456789ABCDEF0", 11, 0xF0DEBC9A78563412, Register.GS, Register.GS)]
		[InlineData("66 4F A3 123456789ABCDEF0", 11, 0xF0DEBC9A78563412, Register.DS, Register.None)]
		void Test64_Mov_Oq_RAX_2(string hexBytes, int byteLength, ulong memoryAddress64, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Mov_Oq_RAX, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory64, instr.Op0Kind);
			Assert.Equal(seg, instr.MemorySegment);
			Assert.Equal(MemorySize.UInt64, instr.MemorySize);
			Assert.Equal(memoryAddress64, instr.MemoryAddress64);
			Assert.Equal(8, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.RAX, instr.Op1Register);
		}

		[Theory]
		[InlineData("A4", 1, Register.DS, Register.None)]
		[InlineData("26 A4", 2, Register.ES, Register.ES)]
		[InlineData("2E A4", 2, Register.CS, Register.CS)]
		[InlineData("36 A4", 2, Register.SS, Register.SS)]
		[InlineData("3E A4", 2, Register.DS, Register.DS)]
		[InlineData("64 A4", 2, Register.FS, Register.FS)]
		[InlineData("65 A4", 2, Register.GS, Register.GS)]
		void Test16_Movsb_Yb_Xb_1(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Movsb_Yb_Xb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESDI, instr.Op0Kind);

			Assert.Equal(OpKind.MemorySegSI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("67 A4", 2, Register.DS, Register.None)]
		[InlineData("26 67 A4", 3, Register.ES, Register.ES)]
		[InlineData("2E 67 A4", 3, Register.CS, Register.CS)]
		[InlineData("36 67 A4", 3, Register.SS, Register.SS)]
		[InlineData("3E 67 A4", 3, Register.DS, Register.DS)]
		[InlineData("64 67 A4", 3, Register.FS, Register.FS)]
		[InlineData("65 67 A4", 3, Register.GS, Register.GS)]
		void Test16_Movsb_Yb_Xb_2(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Movsb_Yb_Xb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op0Kind);

			Assert.Equal(OpKind.MemorySegESI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("67 A4", 2, Register.DS, Register.None)]
		[InlineData("26 67 A4", 3, Register.ES, Register.ES)]
		[InlineData("2E 67 A4", 3, Register.CS, Register.CS)]
		[InlineData("36 67 A4", 3, Register.SS, Register.SS)]
		[InlineData("3E 67 A4", 3, Register.DS, Register.DS)]
		[InlineData("64 67 A4", 3, Register.FS, Register.FS)]
		[InlineData("65 67 A4", 3, Register.GS, Register.GS)]
		void Test32_Movsb_Yb_Xb_1(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Movsb_Yb_Xb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESDI, instr.Op0Kind);

			Assert.Equal(OpKind.MemorySegSI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("A4", 1, Register.DS, Register.None)]
		[InlineData("26 A4", 2, Register.ES, Register.ES)]
		[InlineData("2E A4", 2, Register.CS, Register.CS)]
		[InlineData("36 A4", 2, Register.SS, Register.SS)]
		[InlineData("3E A4", 2, Register.DS, Register.DS)]
		[InlineData("64 A4", 2, Register.FS, Register.FS)]
		[InlineData("65 A4", 2, Register.GS, Register.GS)]
		void Test32_Movsb_Yb_Xb_2(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Movsb_Yb_Xb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op0Kind);

			Assert.Equal(OpKind.MemorySegESI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("67 A4", 2, Register.DS, Register.None)]
		[InlineData("26 67 A4", 3, Register.ES, Register.ES)]
		[InlineData("2E 67 A4", 3, Register.CS, Register.CS)]
		[InlineData("36 67 A4", 3, Register.SS, Register.SS)]
		[InlineData("3E 67 A4", 3, Register.DS, Register.DS)]
		[InlineData("64 67 A4", 3, Register.FS, Register.FS)]
		[InlineData("65 67 A4", 3, Register.GS, Register.GS)]
		[InlineData("66 67 4F A4", 4, Register.DS, Register.None)]
		void Test64_Movsb_Yb_Xb_1(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Movsb_Yb_Xb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op0Kind);

			Assert.Equal(OpKind.MemorySegESI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("A4", 1, Register.DS, Register.None)]
		[InlineData("26 A4", 2, Register.ES, Register.ES)]
		[InlineData("2E A4", 2, Register.CS, Register.CS)]
		[InlineData("36 A4", 2, Register.SS, Register.SS)]
		[InlineData("3E A4", 2, Register.DS, Register.DS)]
		[InlineData("64 A4", 2, Register.FS, Register.FS)]
		[InlineData("65 A4", 2, Register.GS, Register.GS)]
		[InlineData("66 4F A4", 3, Register.DS, Register.None)]
		void Test64_Movsb_Yb_Xb_2(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Movsb_Yb_Xb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESRDI, instr.Op0Kind);

			Assert.Equal(OpKind.MemorySegRSI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("A5", 1, Register.DS, Register.None)]
		[InlineData("26 A5", 2, Register.ES, Register.ES)]
		[InlineData("2E A5", 2, Register.CS, Register.CS)]
		[InlineData("36 A5", 2, Register.SS, Register.SS)]
		[InlineData("3E A5", 2, Register.DS, Register.DS)]
		[InlineData("64 A5", 2, Register.FS, Register.FS)]
		[InlineData("65 A5", 2, Register.GS, Register.GS)]
		void Test16_Movsw_Yw_Xw_1(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Movsw_Yw_Xw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESDI, instr.Op0Kind);

			Assert.Equal(OpKind.MemorySegSI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("67 A5", 2, Register.DS, Register.None)]
		[InlineData("26 67 A5", 3, Register.ES, Register.ES)]
		[InlineData("2E 67 A5", 3, Register.CS, Register.CS)]
		[InlineData("36 67 A5", 3, Register.SS, Register.SS)]
		[InlineData("3E 67 A5", 3, Register.DS, Register.DS)]
		[InlineData("64 67 A5", 3, Register.FS, Register.FS)]
		[InlineData("65 67 A5", 3, Register.GS, Register.GS)]
		void Test16_Movsw_Yw_Xw_2(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Movsw_Yw_Xw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op0Kind);

			Assert.Equal(OpKind.MemorySegESI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("66 67 A5", 3, Register.DS, Register.None)]
		[InlineData("26 66 67 A5", 4, Register.ES, Register.ES)]
		[InlineData("2E 66 67 A5", 4, Register.CS, Register.CS)]
		[InlineData("36 66 67 A5", 4, Register.SS, Register.SS)]
		[InlineData("3E 66 67 A5", 4, Register.DS, Register.DS)]
		[InlineData("64 66 67 A5", 4, Register.FS, Register.FS)]
		[InlineData("65 66 67 A5", 4, Register.GS, Register.GS)]
		void Test32_Movsw_Yw_Xw_1(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Movsw_Yw_Xw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESDI, instr.Op0Kind);

			Assert.Equal(OpKind.MemorySegSI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("66 A5", 2, Register.DS, Register.None)]
		[InlineData("26 66 A5", 3, Register.ES, Register.ES)]
		[InlineData("2E 66 A5", 3, Register.CS, Register.CS)]
		[InlineData("36 66 A5", 3, Register.SS, Register.SS)]
		[InlineData("3E 66 A5", 3, Register.DS, Register.DS)]
		[InlineData("64 66 A5", 3, Register.FS, Register.FS)]
		[InlineData("65 66 A5", 3, Register.GS, Register.GS)]
		void Test32_Movsw_Yw_Xw_2(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Movsw_Yw_Xw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op0Kind);

			Assert.Equal(OpKind.MemorySegESI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("66 67 A5", 3, Register.DS, Register.None)]
		[InlineData("26 66 67 A5", 4, Register.ES, Register.ES)]
		[InlineData("2E 66 67 A5", 4, Register.CS, Register.CS)]
		[InlineData("36 66 67 A5", 4, Register.SS, Register.SS)]
		[InlineData("3E 66 67 A5", 4, Register.DS, Register.DS)]
		[InlineData("64 66 67 A5", 4, Register.FS, Register.FS)]
		[InlineData("65 66 67 A5", 4, Register.GS, Register.GS)]
		[InlineData("66 67 47 A5", 4, Register.DS, Register.None)]
		void Test64_Movsw_Yw_Xw_1(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Movsw_Yw_Xw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op0Kind);

			Assert.Equal(OpKind.MemorySegESI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("66 A5", 2, Register.DS, Register.None)]
		[InlineData("26 66 A5", 3, Register.ES, Register.ES)]
		[InlineData("2E 66 A5", 3, Register.CS, Register.CS)]
		[InlineData("36 66 A5", 3, Register.SS, Register.SS)]
		[InlineData("3E 66 A5", 3, Register.DS, Register.DS)]
		[InlineData("64 66 A5", 3, Register.FS, Register.FS)]
		[InlineData("65 66 A5", 3, Register.GS, Register.GS)]
		[InlineData("66 47 A5", 3, Register.DS, Register.None)]
		void Test64_Movsw_Yw_Xw_2(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Movsw_Yw_Xw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESRDI, instr.Op0Kind);

			Assert.Equal(OpKind.MemorySegRSI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("66 A5", 2, Register.DS, Register.None)]
		[InlineData("26 66 A5", 3, Register.ES, Register.ES)]
		[InlineData("2E 66 A5", 3, Register.CS, Register.CS)]
		[InlineData("36 66 A5", 3, Register.SS, Register.SS)]
		[InlineData("3E 66 A5", 3, Register.DS, Register.DS)]
		[InlineData("64 66 A5", 3, Register.FS, Register.FS)]
		[InlineData("65 66 A5", 3, Register.GS, Register.GS)]
		void Test16_Movsd_Yd_Xd_1(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Movsd_Yd_Xd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESDI, instr.Op0Kind);

			Assert.Equal(OpKind.MemorySegSI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("66 67 A5", 3, Register.DS, Register.None)]
		[InlineData("26 66 67 A5", 4, Register.ES, Register.ES)]
		[InlineData("2E 66 67 A5", 4, Register.CS, Register.CS)]
		[InlineData("36 66 67 A5", 4, Register.SS, Register.SS)]
		[InlineData("3E 66 67 A5", 4, Register.DS, Register.DS)]
		[InlineData("64 66 67 A5", 4, Register.FS, Register.FS)]
		[InlineData("65 66 67 A5", 4, Register.GS, Register.GS)]
		void Test16_Movsd_Yd_Xd_2(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Movsd_Yd_Xd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op0Kind);

			Assert.Equal(OpKind.MemorySegESI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("67 A5", 2, Register.DS, Register.None)]
		[InlineData("26 67 A5", 3, Register.ES, Register.ES)]
		[InlineData("2E 67 A5", 3, Register.CS, Register.CS)]
		[InlineData("36 67 A5", 3, Register.SS, Register.SS)]
		[InlineData("3E 67 A5", 3, Register.DS, Register.DS)]
		[InlineData("64 67 A5", 3, Register.FS, Register.FS)]
		[InlineData("65 67 A5", 3, Register.GS, Register.GS)]
		void Test32_Movsd_Yd_Xd_1(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Movsd_Yd_Xd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESDI, instr.Op0Kind);

			Assert.Equal(OpKind.MemorySegSI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("A5", 1, Register.DS, Register.None)]
		[InlineData("26 A5", 2, Register.ES, Register.ES)]
		[InlineData("2E A5", 2, Register.CS, Register.CS)]
		[InlineData("36 A5", 2, Register.SS, Register.SS)]
		[InlineData("3E A5", 2, Register.DS, Register.DS)]
		[InlineData("64 A5", 2, Register.FS, Register.FS)]
		[InlineData("65 A5", 2, Register.GS, Register.GS)]
		void Test32_Movsd_Yd_Xd_2(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Movsd_Yd_Xd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op0Kind);

			Assert.Equal(OpKind.MemorySegESI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("67 A5", 2, Register.DS, Register.None)]
		[InlineData("26 67 A5", 3, Register.ES, Register.ES)]
		[InlineData("2E 67 A5", 3, Register.CS, Register.CS)]
		[InlineData("36 67 A5", 3, Register.SS, Register.SS)]
		[InlineData("3E 67 A5", 3, Register.DS, Register.DS)]
		[InlineData("64 67 A5", 3, Register.FS, Register.FS)]
		[InlineData("65 67 A5", 3, Register.GS, Register.GS)]
		[InlineData("67 47 A5", 3, Register.DS, Register.None)]
		void Test64_Movsd_Yd_Xd_1(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Movsd_Yd_Xd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op0Kind);

			Assert.Equal(OpKind.MemorySegESI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("A5", 1, Register.DS, Register.None)]
		[InlineData("26 A5", 2, Register.ES, Register.ES)]
		[InlineData("2E A5", 2, Register.CS, Register.CS)]
		[InlineData("36 A5", 2, Register.SS, Register.SS)]
		[InlineData("3E A5", 2, Register.DS, Register.DS)]
		[InlineData("64 A5", 2, Register.FS, Register.FS)]
		[InlineData("65 A5", 2, Register.GS, Register.GS)]
		[InlineData("47 A5", 2, Register.DS, Register.None)]
		void Test64_Movsd_Yd_Xd_2(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Movsd_Yd_Xd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESRDI, instr.Op0Kind);

			Assert.Equal(OpKind.MemorySegRSI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("67 48 A5", 3, Register.DS, Register.None)]
		[InlineData("26 67 48 A5", 4, Register.ES, Register.ES)]
		[InlineData("2E 67 48 A5", 4, Register.CS, Register.CS)]
		[InlineData("36 67 48 A5", 4, Register.SS, Register.SS)]
		[InlineData("3E 67 48 A5", 4, Register.DS, Register.DS)]
		[InlineData("64 67 48 A5", 4, Register.FS, Register.FS)]
		[InlineData("65 67 48 A5", 4, Register.GS, Register.GS)]
		[InlineData("67 4F A5", 3, Register.DS, Register.None)]
		void Test64_Movsq_Yq_Xq_1(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Movsq_Yq_Xq, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op0Kind);

			Assert.Equal(OpKind.MemorySegESI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt64, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("48 A5", 2, Register.DS, Register.None)]
		[InlineData("26 48 A5", 3, Register.ES, Register.ES)]
		[InlineData("2E 48 A5", 3, Register.CS, Register.CS)]
		[InlineData("36 48 A5", 3, Register.SS, Register.SS)]
		[InlineData("3E 48 A5", 3, Register.DS, Register.DS)]
		[InlineData("64 48 A5", 3, Register.FS, Register.FS)]
		[InlineData("65 48 A5", 3, Register.GS, Register.GS)]
		[InlineData("4F A5", 2, Register.DS, Register.None)]
		void Test64_Movsq_Yq_Xq_2(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Movsq_Yq_Xq, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemoryESRDI, instr.Op0Kind);

			Assert.Equal(OpKind.MemorySegRSI, instr.Op1Kind);
			Assert.Equal(MemorySize.UInt64, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);
		}

		[Theory]
		[InlineData("A6", 1, Register.DS, Register.None)]
		[InlineData("26 A6", 2, Register.ES, Register.ES)]
		[InlineData("2E A6", 2, Register.CS, Register.CS)]
		[InlineData("36 A6", 2, Register.SS, Register.SS)]
		[InlineData("3E A6", 2, Register.DS, Register.DS)]
		[InlineData("64 A6", 2, Register.FS, Register.FS)]
		[InlineData("65 A6", 2, Register.GS, Register.GS)]
		void Test16_Cmpsb_Xb_Yb_1(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmpsb_Xb_Yb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemorySegSI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);

			Assert.Equal(OpKind.MemoryESDI, instr.Op1Kind);
		}

		[Theory]
		[InlineData("67 A6", 2, Register.DS, Register.None)]
		[InlineData("26 67 A6", 3, Register.ES, Register.ES)]
		[InlineData("2E 67 A6", 3, Register.CS, Register.CS)]
		[InlineData("36 67 A6", 3, Register.SS, Register.SS)]
		[InlineData("3E 67 A6", 3, Register.DS, Register.DS)]
		[InlineData("64 67 A6", 3, Register.FS, Register.FS)]
		[InlineData("65 67 A6", 3, Register.GS, Register.GS)]
		void Test16_Cmpsb_Xb_Yb_2(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmpsb_Xb_Yb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemorySegESI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op1Kind);
		}

		[Theory]
		[InlineData("67 A6", 2, Register.DS, Register.None)]
		[InlineData("26 67 A6", 3, Register.ES, Register.ES)]
		[InlineData("2E 67 A6", 3, Register.CS, Register.CS)]
		[InlineData("36 67 A6", 3, Register.SS, Register.SS)]
		[InlineData("3E 67 A6", 3, Register.DS, Register.DS)]
		[InlineData("64 67 A6", 3, Register.FS, Register.FS)]
		[InlineData("65 67 A6", 3, Register.GS, Register.GS)]
		void Test32_Cmpsb_Xb_Yb_1(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmpsb_Xb_Yb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemorySegSI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);

			Assert.Equal(OpKind.MemoryESDI, instr.Op1Kind);
		}

		[Theory]
		[InlineData("A6", 1, Register.DS, Register.None)]
		[InlineData("26 A6", 2, Register.ES, Register.ES)]
		[InlineData("2E A6", 2, Register.CS, Register.CS)]
		[InlineData("36 A6", 2, Register.SS, Register.SS)]
		[InlineData("3E A6", 2, Register.DS, Register.DS)]
		[InlineData("64 A6", 2, Register.FS, Register.FS)]
		[InlineData("65 A6", 2, Register.GS, Register.GS)]
		void Test32_Cmpsb_Xb_Yb_2(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmpsb_Xb_Yb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemorySegESI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op1Kind);
		}

		[Theory]
		[InlineData("67 A6", 2, Register.DS, Register.None)]
		[InlineData("26 67 A6", 3, Register.ES, Register.ES)]
		[InlineData("2E 67 A6", 3, Register.CS, Register.CS)]
		[InlineData("36 67 A6", 3, Register.SS, Register.SS)]
		[InlineData("3E 67 A6", 3, Register.DS, Register.DS)]
		[InlineData("64 67 A6", 3, Register.FS, Register.FS)]
		[InlineData("65 67 A6", 3, Register.GS, Register.GS)]
		[InlineData("66 67 4F A6", 4, Register.DS, Register.None)]
		void Test64_Cmpsb_Xb_Yb_1(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmpsb_Xb_Yb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemorySegESI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op1Kind);
		}

		[Theory]
		[InlineData("A6", 1, Register.DS, Register.None)]
		[InlineData("26 A6", 2, Register.ES, Register.ES)]
		[InlineData("2E A6", 2, Register.CS, Register.CS)]
		[InlineData("36 A6", 2, Register.SS, Register.SS)]
		[InlineData("3E A6", 2, Register.DS, Register.DS)]
		[InlineData("64 A6", 2, Register.FS, Register.FS)]
		[InlineData("65 A6", 2, Register.GS, Register.GS)]
		[InlineData("66 4F A6", 3, Register.DS, Register.None)]
		void Test64_Cmpsb_Xb_Yb_2(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmpsb_Xb_Yb, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemorySegRSI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);

			Assert.Equal(OpKind.MemoryESRDI, instr.Op1Kind);
		}

		[Theory]
		[InlineData("A7", 1, Register.DS, Register.None)]
		[InlineData("26 A7", 2, Register.ES, Register.ES)]
		[InlineData("2E A7", 2, Register.CS, Register.CS)]
		[InlineData("36 A7", 2, Register.SS, Register.SS)]
		[InlineData("3E A7", 2, Register.DS, Register.DS)]
		[InlineData("64 A7", 2, Register.FS, Register.FS)]
		[InlineData("65 A7", 2, Register.GS, Register.GS)]
		void Test16_Cmpsw_Xw_Yw_1(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmpsw_Xw_Yw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemorySegSI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);

			Assert.Equal(OpKind.MemoryESDI, instr.Op1Kind);
		}

		[Theory]
		[InlineData("67 A7", 2, Register.DS, Register.None)]
		[InlineData("26 67 A7", 3, Register.ES, Register.ES)]
		[InlineData("2E 67 A7", 3, Register.CS, Register.CS)]
		[InlineData("36 67 A7", 3, Register.SS, Register.SS)]
		[InlineData("3E 67 A7", 3, Register.DS, Register.DS)]
		[InlineData("64 67 A7", 3, Register.FS, Register.FS)]
		[InlineData("65 67 A7", 3, Register.GS, Register.GS)]
		void Test16_Cmpsw_Xw_Yw_2(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmpsw_Xw_Yw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemorySegESI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op1Kind);
		}

		[Theory]
		[InlineData("66 67 A7", 3, Register.DS, Register.None)]
		[InlineData("26 66 67 A7", 4, Register.ES, Register.ES)]
		[InlineData("2E 66 67 A7", 4, Register.CS, Register.CS)]
		[InlineData("36 66 67 A7", 4, Register.SS, Register.SS)]
		[InlineData("3E 66 67 A7", 4, Register.DS, Register.DS)]
		[InlineData("64 66 67 A7", 4, Register.FS, Register.FS)]
		[InlineData("65 66 67 A7", 4, Register.GS, Register.GS)]
		void Test32_Cmpsw_Xw_Yw_1(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmpsw_Xw_Yw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemorySegSI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);

			Assert.Equal(OpKind.MemoryESDI, instr.Op1Kind);
		}

		[Theory]
		[InlineData("66 A7", 2, Register.DS, Register.None)]
		[InlineData("26 66 A7", 3, Register.ES, Register.ES)]
		[InlineData("2E 66 A7", 3, Register.CS, Register.CS)]
		[InlineData("36 66 A7", 3, Register.SS, Register.SS)]
		[InlineData("3E 66 A7", 3, Register.DS, Register.DS)]
		[InlineData("64 66 A7", 3, Register.FS, Register.FS)]
		[InlineData("65 66 A7", 3, Register.GS, Register.GS)]
		void Test32_Cmpsw_Xw_Yw_2(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmpsw_Xw_Yw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemorySegESI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op1Kind);
		}

		[Theory]
		[InlineData("66 67 A7", 3, Register.DS, Register.None)]
		[InlineData("26 66 67 A7", 4, Register.ES, Register.ES)]
		[InlineData("2E 66 67 A7", 4, Register.CS, Register.CS)]
		[InlineData("36 66 67 A7", 4, Register.SS, Register.SS)]
		[InlineData("3E 66 67 A7", 4, Register.DS, Register.DS)]
		[InlineData("64 66 67 A7", 4, Register.FS, Register.FS)]
		[InlineData("65 66 67 A7", 4, Register.GS, Register.GS)]
		[InlineData("66 67 47 A7", 4, Register.DS, Register.None)]
		void Test64_Cmpsw_Xw_Yw_1(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmpsw_Xw_Yw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemorySegESI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op1Kind);
		}

		[Theory]
		[InlineData("66 A7", 2, Register.DS, Register.None)]
		[InlineData("26 66 A7", 3, Register.ES, Register.ES)]
		[InlineData("2E 66 A7", 3, Register.CS, Register.CS)]
		[InlineData("36 66 A7", 3, Register.SS, Register.SS)]
		[InlineData("3E 66 A7", 3, Register.DS, Register.DS)]
		[InlineData("64 66 A7", 3, Register.FS, Register.FS)]
		[InlineData("65 66 A7", 3, Register.GS, Register.GS)]
		[InlineData("66 47 A7", 3, Register.DS, Register.None)]
		void Test64_Cmpsw_Xw_Yw_2(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmpsw_Xw_Yw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemorySegRSI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);

			Assert.Equal(OpKind.MemoryESRDI, instr.Op1Kind);
		}

		[Theory]
		[InlineData("66 A7", 2, Register.DS, Register.None)]
		[InlineData("26 66 A7", 3, Register.ES, Register.ES)]
		[InlineData("2E 66 A7", 3, Register.CS, Register.CS)]
		[InlineData("36 66 A7", 3, Register.SS, Register.SS)]
		[InlineData("3E 66 A7", 3, Register.DS, Register.DS)]
		[InlineData("64 66 A7", 3, Register.FS, Register.FS)]
		[InlineData("65 66 A7", 3, Register.GS, Register.GS)]
		void Test16_Cmpsd_Xd_Yd_1(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmpsd_Xd_Yd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemorySegSI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);

			Assert.Equal(OpKind.MemoryESDI, instr.Op1Kind);
		}

		[Theory]
		[InlineData("66 67 A7", 3, Register.DS, Register.None)]
		[InlineData("26 66 67 A7", 4, Register.ES, Register.ES)]
		[InlineData("2E 66 67 A7", 4, Register.CS, Register.CS)]
		[InlineData("36 66 67 A7", 4, Register.SS, Register.SS)]
		[InlineData("3E 66 67 A7", 4, Register.DS, Register.DS)]
		[InlineData("64 66 67 A7", 4, Register.FS, Register.FS)]
		[InlineData("65 66 67 A7", 4, Register.GS, Register.GS)]
		void Test16_Cmpsd_Xd_Yd_2(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmpsd_Xd_Yd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemorySegESI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op1Kind);
		}

		[Theory]
		[InlineData("67 A7", 2, Register.DS, Register.None)]
		[InlineData("26 67 A7", 3, Register.ES, Register.ES)]
		[InlineData("2E 67 A7", 3, Register.CS, Register.CS)]
		[InlineData("36 67 A7", 3, Register.SS, Register.SS)]
		[InlineData("3E 67 A7", 3, Register.DS, Register.DS)]
		[InlineData("64 67 A7", 3, Register.FS, Register.FS)]
		[InlineData("65 67 A7", 3, Register.GS, Register.GS)]
		void Test32_Cmpsd_Xd_Yd_1(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmpsd_Xd_Yd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemorySegSI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);

			Assert.Equal(OpKind.MemoryESDI, instr.Op1Kind);
		}

		[Theory]
		[InlineData("A7", 1, Register.DS, Register.None)]
		[InlineData("26 A7", 2, Register.ES, Register.ES)]
		[InlineData("2E A7", 2, Register.CS, Register.CS)]
		[InlineData("36 A7", 2, Register.SS, Register.SS)]
		[InlineData("3E A7", 2, Register.DS, Register.DS)]
		[InlineData("64 A7", 2, Register.FS, Register.FS)]
		[InlineData("65 A7", 2, Register.GS, Register.GS)]
		void Test32_Cmpsd_Xd_Yd_2(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmpsd_Xd_Yd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemorySegESI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op1Kind);
		}

		[Theory]
		[InlineData("67 A7", 2, Register.DS, Register.None)]
		[InlineData("26 67 A7", 3, Register.ES, Register.ES)]
		[InlineData("2E 67 A7", 3, Register.CS, Register.CS)]
		[InlineData("36 67 A7", 3, Register.SS, Register.SS)]
		[InlineData("3E 67 A7", 3, Register.DS, Register.DS)]
		[InlineData("64 67 A7", 3, Register.FS, Register.FS)]
		[InlineData("65 67 A7", 3, Register.GS, Register.GS)]
		[InlineData("67 47 A7", 3, Register.DS, Register.None)]
		void Test64_Cmpsd_Xd_Yd_1(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmpsd_Xd_Yd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemorySegESI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op1Kind);
		}

		[Theory]
		[InlineData("A7", 1, Register.DS, Register.None)]
		[InlineData("26 A7", 2, Register.ES, Register.ES)]
		[InlineData("2E A7", 2, Register.CS, Register.CS)]
		[InlineData("36 A7", 2, Register.SS, Register.SS)]
		[InlineData("3E A7", 2, Register.DS, Register.DS)]
		[InlineData("64 A7", 2, Register.FS, Register.FS)]
		[InlineData("65 A7", 2, Register.GS, Register.GS)]
		[InlineData("47 A7", 2, Register.DS, Register.None)]
		void Test64_Cmpsd_Xd_Yd_2(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmpsd_Xd_Yd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemorySegRSI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);

			Assert.Equal(OpKind.MemoryESRDI, instr.Op1Kind);
		}

		[Theory]
		[InlineData("67 48 A7", 3, Register.DS, Register.None)]
		[InlineData("26 67 48 A7", 4, Register.ES, Register.ES)]
		[InlineData("2E 67 48 A7", 4, Register.CS, Register.CS)]
		[InlineData("36 67 48 A7", 4, Register.SS, Register.SS)]
		[InlineData("3E 67 48 A7", 4, Register.DS, Register.DS)]
		[InlineData("64 67 48 A7", 4, Register.FS, Register.FS)]
		[InlineData("65 67 48 A7", 4, Register.GS, Register.GS)]
		[InlineData("67 4F A7", 3, Register.DS, Register.None)]
		void Test64_Cmpsq_Xq_Yq_1(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmpsq_Xq_Yq, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemorySegESI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt64, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);

			Assert.Equal(OpKind.MemoryESEDI, instr.Op1Kind);
		}

		[Theory]
		[InlineData("48 A7", 2, Register.DS, Register.None)]
		[InlineData("26 48 A7", 3, Register.ES, Register.ES)]
		[InlineData("2E 48 A7", 3, Register.CS, Register.CS)]
		[InlineData("36 48 A7", 3, Register.SS, Register.SS)]
		[InlineData("3E 48 A7", 3, Register.DS, Register.DS)]
		[InlineData("64 48 A7", 3, Register.FS, Register.FS)]
		[InlineData("65 48 A7", 3, Register.GS, Register.GS)]
		[InlineData("4F A7", 2, Register.DS, Register.None)]
		void Test64_Cmpsq_Xq_Yq_2(string hexBytes, int byteLength, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Cmpsq_Xq_Yq, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.MemorySegRSI, instr.Op0Kind);
			Assert.Equal(MemorySize.UInt64, instr.MemorySize);
			Assert.Equal(seg, instr.MemorySegment);

			Assert.Equal(OpKind.MemoryESRDI, instr.Op1Kind);
		}
	}
}
