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
	public sealed class DecoderTest_1_50_57 : DecoderTest {
		[Theory]
		[InlineData("50", 1, Code.Push_r16, Register.AX)]
		[InlineData("51", 1, Code.Push_r16, Register.CX)]
		[InlineData("52", 1, Code.Push_r16, Register.DX)]
		[InlineData("53", 1, Code.Push_r16, Register.BX)]
		[InlineData("54", 1, Code.Push_r16, Register.SP)]
		[InlineData("55", 1, Code.Push_r16, Register.BP)]
		[InlineData("56", 1, Code.Push_r16, Register.SI)]
		[InlineData("57", 1, Code.Push_r16, Register.DI)]
		void Test16_Push_R16_1(string hexBytes, int byteLength, Code code, Register register) {
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
		[InlineData("66 50", 2, Code.Push_r16, Register.AX)]
		[InlineData("66 51", 2, Code.Push_r16, Register.CX)]
		[InlineData("66 52", 2, Code.Push_r16, Register.DX)]
		[InlineData("66 53", 2, Code.Push_r16, Register.BX)]
		[InlineData("66 54", 2, Code.Push_r16, Register.SP)]
		[InlineData("66 55", 2, Code.Push_r16, Register.BP)]
		[InlineData("66 56", 2, Code.Push_r16, Register.SI)]
		[InlineData("66 57", 2, Code.Push_r16, Register.DI)]
		void Test32_Push_R16_1(string hexBytes, int byteLength, Code code, Register register) {
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
		[InlineData("66 50", 2, Code.Push_r16, Register.AX)]
		[InlineData("66 51", 2, Code.Push_r16, Register.CX)]
		[InlineData("66 52", 2, Code.Push_r16, Register.DX)]
		[InlineData("66 53", 2, Code.Push_r16, Register.BX)]
		[InlineData("66 54", 2, Code.Push_r16, Register.SP)]
		[InlineData("66 55", 2, Code.Push_r16, Register.BP)]
		[InlineData("66 56", 2, Code.Push_r16, Register.SI)]
		[InlineData("66 57", 2, Code.Push_r16, Register.DI)]
		[InlineData("66 41 50", 3, Code.Push_r16, Register.R8W)]
		[InlineData("66 41 51", 3, Code.Push_r16, Register.R9W)]
		[InlineData("66 41 52", 3, Code.Push_r16, Register.R10W)]
		[InlineData("66 41 53", 3, Code.Push_r16, Register.R11W)]
		[InlineData("66 41 54", 3, Code.Push_r16, Register.R12W)]
		[InlineData("66 41 55", 3, Code.Push_r16, Register.R13W)]
		[InlineData("66 41 56", 3, Code.Push_r16, Register.R14W)]
		[InlineData("66 41 57", 3, Code.Push_r16, Register.R15W)]
		[InlineData("66 46 50", 3, Code.Push_r16, Register.AX)]
		[InlineData("66 46 51", 3, Code.Push_r16, Register.CX)]
		[InlineData("66 46 52", 3, Code.Push_r16, Register.DX)]
		[InlineData("66 46 53", 3, Code.Push_r16, Register.BX)]
		[InlineData("66 46 54", 3, Code.Push_r16, Register.SP)]
		[InlineData("66 46 55", 3, Code.Push_r16, Register.BP)]
		[InlineData("66 46 56", 3, Code.Push_r16, Register.SI)]
		[InlineData("66 46 57", 3, Code.Push_r16, Register.DI)]
		[InlineData("66 47 50", 3, Code.Push_r16, Register.R8W)]
		[InlineData("66 47 51", 3, Code.Push_r16, Register.R9W)]
		[InlineData("66 47 52", 3, Code.Push_r16, Register.R10W)]
		[InlineData("66 47 53", 3, Code.Push_r16, Register.R11W)]
		[InlineData("66 47 54", 3, Code.Push_r16, Register.R12W)]
		[InlineData("66 47 55", 3, Code.Push_r16, Register.R13W)]
		[InlineData("66 47 56", 3, Code.Push_r16, Register.R14W)]
		[InlineData("66 47 57", 3, Code.Push_r16, Register.R15W)]
		void Test64_Push_R16_1(string hexBytes, int byteLength, Code code, Register register) {
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
		[InlineData("66 50", 2, Code.Push_r32, Register.EAX)]
		[InlineData("66 51", 2, Code.Push_r32, Register.ECX)]
		[InlineData("66 52", 2, Code.Push_r32, Register.EDX)]
		[InlineData("66 53", 2, Code.Push_r32, Register.EBX)]
		[InlineData("66 54", 2, Code.Push_r32, Register.ESP)]
		[InlineData("66 55", 2, Code.Push_r32, Register.EBP)]
		[InlineData("66 56", 2, Code.Push_r32, Register.ESI)]
		[InlineData("66 57", 2, Code.Push_r32, Register.EDI)]
		void Test16_Push_R32_1(string hexBytes, int byteLength, Code code, Register register) {
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
		[InlineData("50", 1, Code.Push_r32, Register.EAX)]
		[InlineData("51", 1, Code.Push_r32, Register.ECX)]
		[InlineData("52", 1, Code.Push_r32, Register.EDX)]
		[InlineData("53", 1, Code.Push_r32, Register.EBX)]
		[InlineData("54", 1, Code.Push_r32, Register.ESP)]
		[InlineData("55", 1, Code.Push_r32, Register.EBP)]
		[InlineData("56", 1, Code.Push_r32, Register.ESI)]
		[InlineData("57", 1, Code.Push_r32, Register.EDI)]
		void Test32_Push_R32_1(string hexBytes, int byteLength, Code code, Register register) {
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
		[InlineData("50", 1, Code.Push_r64, Register.RAX)]
		[InlineData("51", 1, Code.Push_r64, Register.RCX)]
		[InlineData("52", 1, Code.Push_r64, Register.RDX)]
		[InlineData("53", 1, Code.Push_r64, Register.RBX)]
		[InlineData("54", 1, Code.Push_r64, Register.RSP)]
		[InlineData("55", 1, Code.Push_r64, Register.RBP)]
		[InlineData("56", 1, Code.Push_r64, Register.RSI)]
		[InlineData("57", 1, Code.Push_r64, Register.RDI)]
		[InlineData("41 50", 2, Code.Push_r64, Register.R8)]
		[InlineData("41 51", 2, Code.Push_r64, Register.R9)]
		[InlineData("41 52", 2, Code.Push_r64, Register.R10)]
		[InlineData("41 53", 2, Code.Push_r64, Register.R11)]
		[InlineData("41 54", 2, Code.Push_r64, Register.R12)]
		[InlineData("41 55", 2, Code.Push_r64, Register.R13)]
		[InlineData("41 56", 2, Code.Push_r64, Register.R14)]
		[InlineData("41 57", 2, Code.Push_r64, Register.R15)]
		[InlineData("66 4E 50", 3, Code.Push_r64, Register.RAX)]
		[InlineData("66 4E 51", 3, Code.Push_r64, Register.RCX)]
		[InlineData("66 4E 52", 3, Code.Push_r64, Register.RDX)]
		[InlineData("66 4E 53", 3, Code.Push_r64, Register.RBX)]
		[InlineData("66 4E 54", 3, Code.Push_r64, Register.RSP)]
		[InlineData("66 4E 55", 3, Code.Push_r64, Register.RBP)]
		[InlineData("66 4E 56", 3, Code.Push_r64, Register.RSI)]
		[InlineData("66 4E 57", 3, Code.Push_r64, Register.RDI)]
		[InlineData("66 4F 50", 3, Code.Push_r64, Register.R8)]
		[InlineData("66 4F 51", 3, Code.Push_r64, Register.R9)]
		[InlineData("66 4F 52", 3, Code.Push_r64, Register.R10)]
		[InlineData("66 4F 53", 3, Code.Push_r64, Register.R11)]
		[InlineData("66 4F 54", 3, Code.Push_r64, Register.R12)]
		[InlineData("66 4F 55", 3, Code.Push_r64, Register.R13)]
		[InlineData("66 4F 56", 3, Code.Push_r64, Register.R14)]
		[InlineData("66 4F 57", 3, Code.Push_r64, Register.R15)]
		void Test64_Push_R64_1(string hexBytes, int byteLength, Code code, Register register) {
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
