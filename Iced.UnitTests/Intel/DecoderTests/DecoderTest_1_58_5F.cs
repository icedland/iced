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
	public sealed class DecoderTest_1_58_5F : DecoderTest {
		[Theory]
		[InlineData("58", 1, Code.Pop_r16, Register.AX)]
		[InlineData("59", 1, Code.Pop_r16, Register.CX)]
		[InlineData("5A", 1, Code.Pop_r16, Register.DX)]
		[InlineData("5B", 1, Code.Pop_r16, Register.BX)]
		[InlineData("5C", 1, Code.Pop_r16, Register.SP)]
		[InlineData("5D", 1, Code.Pop_r16, Register.BP)]
		[InlineData("5E", 1, Code.Pop_r16, Register.SI)]
		[InlineData("5F", 1, Code.Pop_r16, Register.DI)]
		void Test16_Pop_R16_1(string hexBytes, int byteLength, Code code, Register register) {
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
			Assert.Equal(register, instr.Op0Register);
		}

		[Theory]
		[InlineData("66 58", 2, Code.Pop_r16, Register.AX)]
		[InlineData("66 59", 2, Code.Pop_r16, Register.CX)]
		[InlineData("66 5A", 2, Code.Pop_r16, Register.DX)]
		[InlineData("66 5B", 2, Code.Pop_r16, Register.BX)]
		[InlineData("66 5C", 2, Code.Pop_r16, Register.SP)]
		[InlineData("66 5D", 2, Code.Pop_r16, Register.BP)]
		[InlineData("66 5E", 2, Code.Pop_r16, Register.SI)]
		[InlineData("66 5F", 2, Code.Pop_r16, Register.DI)]
		void Test32_Pop_R16_1(string hexBytes, int byteLength, Code code, Register register) {
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
			Assert.Equal(register, instr.Op0Register);
		}

		[Theory]
		[InlineData("66 58", 2, Code.Pop_r16, Register.AX)]
		[InlineData("66 59", 2, Code.Pop_r16, Register.CX)]
		[InlineData("66 5A", 2, Code.Pop_r16, Register.DX)]
		[InlineData("66 5B", 2, Code.Pop_r16, Register.BX)]
		[InlineData("66 5C", 2, Code.Pop_r16, Register.SP)]
		[InlineData("66 5D", 2, Code.Pop_r16, Register.BP)]
		[InlineData("66 5E", 2, Code.Pop_r16, Register.SI)]
		[InlineData("66 5F", 2, Code.Pop_r16, Register.DI)]
		[InlineData("66 41 58", 3, Code.Pop_r16, Register.R8W)]
		[InlineData("66 41 59", 3, Code.Pop_r16, Register.R9W)]
		[InlineData("66 41 5A", 3, Code.Pop_r16, Register.R10W)]
		[InlineData("66 41 5B", 3, Code.Pop_r16, Register.R11W)]
		[InlineData("66 41 5C", 3, Code.Pop_r16, Register.R12W)]
		[InlineData("66 41 5D", 3, Code.Pop_r16, Register.R13W)]
		[InlineData("66 41 5E", 3, Code.Pop_r16, Register.R14W)]
		[InlineData("66 41 5F", 3, Code.Pop_r16, Register.R15W)]
		[InlineData("66 46 58", 3, Code.Pop_r16, Register.AX)]
		[InlineData("66 46 59", 3, Code.Pop_r16, Register.CX)]
		[InlineData("66 46 5A", 3, Code.Pop_r16, Register.DX)]
		[InlineData("66 46 5B", 3, Code.Pop_r16, Register.BX)]
		[InlineData("66 46 5C", 3, Code.Pop_r16, Register.SP)]
		[InlineData("66 46 5D", 3, Code.Pop_r16, Register.BP)]
		[InlineData("66 46 5E", 3, Code.Pop_r16, Register.SI)]
		[InlineData("66 46 5F", 3, Code.Pop_r16, Register.DI)]
		[InlineData("66 47 58", 3, Code.Pop_r16, Register.R8W)]
		[InlineData("66 47 59", 3, Code.Pop_r16, Register.R9W)]
		[InlineData("66 47 5A", 3, Code.Pop_r16, Register.R10W)]
		[InlineData("66 47 5B", 3, Code.Pop_r16, Register.R11W)]
		[InlineData("66 47 5C", 3, Code.Pop_r16, Register.R12W)]
		[InlineData("66 47 5D", 3, Code.Pop_r16, Register.R13W)]
		[InlineData("66 47 5E", 3, Code.Pop_r16, Register.R14W)]
		[InlineData("66 47 5F", 3, Code.Pop_r16, Register.R15W)]
		void Test64_Pop_R16_1(string hexBytes, int byteLength, Code code, Register register) {
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
			Assert.Equal(register, instr.Op0Register);
		}

		[Theory]
		[InlineData("66 58", 2, Code.Pop_r32, Register.EAX)]
		[InlineData("66 59", 2, Code.Pop_r32, Register.ECX)]
		[InlineData("66 5A", 2, Code.Pop_r32, Register.EDX)]
		[InlineData("66 5B", 2, Code.Pop_r32, Register.EBX)]
		[InlineData("66 5C", 2, Code.Pop_r32, Register.ESP)]
		[InlineData("66 5D", 2, Code.Pop_r32, Register.EBP)]
		[InlineData("66 5E", 2, Code.Pop_r32, Register.ESI)]
		[InlineData("66 5F", 2, Code.Pop_r32, Register.EDI)]
		void Test16_Pop_R32_1(string hexBytes, int byteLength, Code code, Register register) {
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
			Assert.Equal(register, instr.Op0Register);
		}

		[Theory]
		[InlineData("58", 1, Code.Pop_r32, Register.EAX)]
		[InlineData("59", 1, Code.Pop_r32, Register.ECX)]
		[InlineData("5A", 1, Code.Pop_r32, Register.EDX)]
		[InlineData("5B", 1, Code.Pop_r32, Register.EBX)]
		[InlineData("5C", 1, Code.Pop_r32, Register.ESP)]
		[InlineData("5D", 1, Code.Pop_r32, Register.EBP)]
		[InlineData("5E", 1, Code.Pop_r32, Register.ESI)]
		[InlineData("5F", 1, Code.Pop_r32, Register.EDI)]
		void Test32_Pop_R32_1(string hexBytes, int byteLength, Code code, Register register) {
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
			Assert.Equal(register, instr.Op0Register);
		}

		[Theory]
		[InlineData("58", 1, Code.Pop_r64, Register.RAX)]
		[InlineData("59", 1, Code.Pop_r64, Register.RCX)]
		[InlineData("5A", 1, Code.Pop_r64, Register.RDX)]
		[InlineData("5B", 1, Code.Pop_r64, Register.RBX)]
		[InlineData("5C", 1, Code.Pop_r64, Register.RSP)]
		[InlineData("5D", 1, Code.Pop_r64, Register.RBP)]
		[InlineData("5E", 1, Code.Pop_r64, Register.RSI)]
		[InlineData("5F", 1, Code.Pop_r64, Register.RDI)]
		[InlineData("41 58", 2, Code.Pop_r64, Register.R8)]
		[InlineData("41 59", 2, Code.Pop_r64, Register.R9)]
		[InlineData("41 5A", 2, Code.Pop_r64, Register.R10)]
		[InlineData("41 5B", 2, Code.Pop_r64, Register.R11)]
		[InlineData("41 5C", 2, Code.Pop_r64, Register.R12)]
		[InlineData("41 5D", 2, Code.Pop_r64, Register.R13)]
		[InlineData("41 5E", 2, Code.Pop_r64, Register.R14)]
		[InlineData("41 5F", 2, Code.Pop_r64, Register.R15)]
		[InlineData("66 4E 58", 3, Code.Pop_r64, Register.RAX)]
		[InlineData("66 4E 59", 3, Code.Pop_r64, Register.RCX)]
		[InlineData("66 4E 5A", 3, Code.Pop_r64, Register.RDX)]
		[InlineData("66 4E 5B", 3, Code.Pop_r64, Register.RBX)]
		[InlineData("66 4E 5C", 3, Code.Pop_r64, Register.RSP)]
		[InlineData("66 4E 5D", 3, Code.Pop_r64, Register.RBP)]
		[InlineData("66 4E 5E", 3, Code.Pop_r64, Register.RSI)]
		[InlineData("66 4E 5F", 3, Code.Pop_r64, Register.RDI)]
		[InlineData("66 4F 58", 3, Code.Pop_r64, Register.R8)]
		[InlineData("66 4F 59", 3, Code.Pop_r64, Register.R9)]
		[InlineData("66 4F 5A", 3, Code.Pop_r64, Register.R10)]
		[InlineData("66 4F 5B", 3, Code.Pop_r64, Register.R11)]
		[InlineData("66 4F 5C", 3, Code.Pop_r64, Register.R12)]
		[InlineData("66 4F 5D", 3, Code.Pop_r64, Register.R13)]
		[InlineData("66 4F 5E", 3, Code.Pop_r64, Register.R14)]
		[InlineData("66 4F 5F", 3, Code.Pop_r64, Register.R15)]
		void Test64_Pop_R64_1(string hexBytes, int byteLength, Code code, Register register) {
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
			Assert.Equal(register, instr.Op0Register);
		}
	}
}
