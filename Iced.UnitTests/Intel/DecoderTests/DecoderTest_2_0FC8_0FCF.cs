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
	public sealed class DecoderTest_2_0FC8_0FCF : DecoderTest {
		[Theory]
		[InlineData("0FC8", 2, Code.Bswap_r16, Register.AX)]
		[InlineData("0FC9", 2, Code.Bswap_r16, Register.CX)]
		[InlineData("0FCA", 2, Code.Bswap_r16, Register.DX)]
		[InlineData("0FCB", 2, Code.Bswap_r16, Register.BX)]
		[InlineData("0FCC", 2, Code.Bswap_r16, Register.SP)]
		[InlineData("0FCD", 2, Code.Bswap_r16, Register.BP)]
		[InlineData("0FCE", 2, Code.Bswap_r16, Register.SI)]
		[InlineData("0FCF", 2, Code.Bswap_r16, Register.DI)]
		void Test16_Bswap_Reg16_1(string hexBytes, int byteLength, Code code, Register register) {
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
		[InlineData("66 0FC8", 3, Code.Bswap_r16, Register.AX)]
		[InlineData("66 0FC9", 3, Code.Bswap_r16, Register.CX)]
		[InlineData("66 0FCA", 3, Code.Bswap_r16, Register.DX)]
		[InlineData("66 0FCB", 3, Code.Bswap_r16, Register.BX)]
		[InlineData("66 0FCC", 3, Code.Bswap_r16, Register.SP)]
		[InlineData("66 0FCD", 3, Code.Bswap_r16, Register.BP)]
		[InlineData("66 0FCE", 3, Code.Bswap_r16, Register.SI)]
		[InlineData("66 0FCF", 3, Code.Bswap_r16, Register.DI)]
		void Test32_Bswap_Reg16_1(string hexBytes, int byteLength, Code code, Register register) {
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
		[InlineData("66 0FC8", 3, Code.Bswap_r16, Register.AX)]
		[InlineData("66 0FC9", 3, Code.Bswap_r16, Register.CX)]
		[InlineData("66 0FCA", 3, Code.Bswap_r16, Register.DX)]
		[InlineData("66 0FCB", 3, Code.Bswap_r16, Register.BX)]
		[InlineData("66 0FCC", 3, Code.Bswap_r16, Register.SP)]
		[InlineData("66 0FCD", 3, Code.Bswap_r16, Register.BP)]
		[InlineData("66 0FCE", 3, Code.Bswap_r16, Register.SI)]
		[InlineData("66 0FCF", 3, Code.Bswap_r16, Register.DI)]
		[InlineData("66 41 0FC8", 4, Code.Bswap_r16, Register.R8W)]
		[InlineData("66 41 0FC9", 4, Code.Bswap_r16, Register.R9W)]
		[InlineData("66 41 0FCA", 4, Code.Bswap_r16, Register.R10W)]
		[InlineData("66 41 0FCB", 4, Code.Bswap_r16, Register.R11W)]
		[InlineData("66 41 0FCC", 4, Code.Bswap_r16, Register.R12W)]
		[InlineData("66 41 0FCD", 4, Code.Bswap_r16, Register.R13W)]
		[InlineData("66 41 0FCE", 4, Code.Bswap_r16, Register.R14W)]
		[InlineData("66 41 0FCF", 4, Code.Bswap_r16, Register.R15W)]

		[InlineData("66 46 0FC8", 4, Code.Bswap_r16, Register.AX)]
		[InlineData("66 46 0FC9", 4, Code.Bswap_r16, Register.CX)]
		[InlineData("66 46 0FCA", 4, Code.Bswap_r16, Register.DX)]
		[InlineData("66 46 0FCB", 4, Code.Bswap_r16, Register.BX)]
		[InlineData("66 46 0FCC", 4, Code.Bswap_r16, Register.SP)]
		[InlineData("66 46 0FCD", 4, Code.Bswap_r16, Register.BP)]
		[InlineData("66 46 0FCE", 4, Code.Bswap_r16, Register.SI)]
		[InlineData("66 46 0FCF", 4, Code.Bswap_r16, Register.DI)]
		[InlineData("66 47 0FC8", 4, Code.Bswap_r16, Register.R8W)]
		[InlineData("66 47 0FC9", 4, Code.Bswap_r16, Register.R9W)]
		[InlineData("66 47 0FCA", 4, Code.Bswap_r16, Register.R10W)]
		[InlineData("66 47 0FCB", 4, Code.Bswap_r16, Register.R11W)]
		[InlineData("66 47 0FCC", 4, Code.Bswap_r16, Register.R12W)]
		[InlineData("66 47 0FCD", 4, Code.Bswap_r16, Register.R13W)]
		[InlineData("66 47 0FCE", 4, Code.Bswap_r16, Register.R14W)]
		[InlineData("66 47 0FCF", 4, Code.Bswap_r16, Register.R15W)]
		void Test64_Bswap_Reg16_1(string hexBytes, int byteLength, Code code, Register register) {
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
		[InlineData("66 0FC8", 3, Code.Bswap_r32, Register.EAX)]
		[InlineData("66 0FC9", 3, Code.Bswap_r32, Register.ECX)]
		[InlineData("66 0FCA", 3, Code.Bswap_r32, Register.EDX)]
		[InlineData("66 0FCB", 3, Code.Bswap_r32, Register.EBX)]
		[InlineData("66 0FCC", 3, Code.Bswap_r32, Register.ESP)]
		[InlineData("66 0FCD", 3, Code.Bswap_r32, Register.EBP)]
		[InlineData("66 0FCE", 3, Code.Bswap_r32, Register.ESI)]
		[InlineData("66 0FCF", 3, Code.Bswap_r32, Register.EDI)]
		void Test16_Bswap_Reg32_1(string hexBytes, int byteLength, Code code, Register register) {
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
		[InlineData("0FC8", 2, Code.Bswap_r32, Register.EAX)]
		[InlineData("0FC9", 2, Code.Bswap_r32, Register.ECX)]
		[InlineData("0FCA", 2, Code.Bswap_r32, Register.EDX)]
		[InlineData("0FCB", 2, Code.Bswap_r32, Register.EBX)]
		[InlineData("0FCC", 2, Code.Bswap_r32, Register.ESP)]
		[InlineData("0FCD", 2, Code.Bswap_r32, Register.EBP)]
		[InlineData("0FCE", 2, Code.Bswap_r32, Register.ESI)]
		[InlineData("0FCF", 2, Code.Bswap_r32, Register.EDI)]
		void Test32_Bswap_Reg32_1(string hexBytes, int byteLength, Code code, Register register) {
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
		[InlineData("0FC8", 2, Code.Bswap_r32, Register.EAX)]
		[InlineData("0FC9", 2, Code.Bswap_r32, Register.ECX)]
		[InlineData("0FCA", 2, Code.Bswap_r32, Register.EDX)]
		[InlineData("0FCB", 2, Code.Bswap_r32, Register.EBX)]
		[InlineData("0FCC", 2, Code.Bswap_r32, Register.ESP)]
		[InlineData("0FCD", 2, Code.Bswap_r32, Register.EBP)]
		[InlineData("0FCE", 2, Code.Bswap_r32, Register.ESI)]
		[InlineData("0FCF", 2, Code.Bswap_r32, Register.EDI)]
		[InlineData("41 0FC8", 3, Code.Bswap_r32, Register.R8D)]
		[InlineData("41 0FC9", 3, Code.Bswap_r32, Register.R9D)]
		[InlineData("41 0FCA", 3, Code.Bswap_r32, Register.R10D)]
		[InlineData("41 0FCB", 3, Code.Bswap_r32, Register.R11D)]
		[InlineData("41 0FCC", 3, Code.Bswap_r32, Register.R12D)]
		[InlineData("41 0FCD", 3, Code.Bswap_r32, Register.R13D)]
		[InlineData("41 0FCE", 3, Code.Bswap_r32, Register.R14D)]
		[InlineData("41 0FCF", 3, Code.Bswap_r32, Register.R15D)]

		[InlineData("46 0FC8", 3, Code.Bswap_r32, Register.EAX)]
		[InlineData("46 0FC9", 3, Code.Bswap_r32, Register.ECX)]
		[InlineData("46 0FCA", 3, Code.Bswap_r32, Register.EDX)]
		[InlineData("46 0FCB", 3, Code.Bswap_r32, Register.EBX)]
		[InlineData("46 0FCC", 3, Code.Bswap_r32, Register.ESP)]
		[InlineData("46 0FCD", 3, Code.Bswap_r32, Register.EBP)]
		[InlineData("46 0FCE", 3, Code.Bswap_r32, Register.ESI)]
		[InlineData("46 0FCF", 3, Code.Bswap_r32, Register.EDI)]
		[InlineData("47 0FC8", 3, Code.Bswap_r32, Register.R8D)]
		[InlineData("47 0FC9", 3, Code.Bswap_r32, Register.R9D)]
		[InlineData("47 0FCA", 3, Code.Bswap_r32, Register.R10D)]
		[InlineData("47 0FCB", 3, Code.Bswap_r32, Register.R11D)]
		[InlineData("47 0FCC", 3, Code.Bswap_r32, Register.R12D)]
		[InlineData("47 0FCD", 3, Code.Bswap_r32, Register.R13D)]
		[InlineData("47 0FCE", 3, Code.Bswap_r32, Register.R14D)]
		[InlineData("47 0FCF", 3, Code.Bswap_r32, Register.R15D)]
		void Test64_Bswap_Reg32_1(string hexBytes, int byteLength, Code code, Register register) {
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
		[InlineData("48 0FC8", 3, Code.Bswap_r64, Register.RAX)]
		[InlineData("48 0FC9", 3, Code.Bswap_r64, Register.RCX)]
		[InlineData("48 0FCA", 3, Code.Bswap_r64, Register.RDX)]
		[InlineData("48 0FCB", 3, Code.Bswap_r64, Register.RBX)]
		[InlineData("48 0FCC", 3, Code.Bswap_r64, Register.RSP)]
		[InlineData("48 0FCD", 3, Code.Bswap_r64, Register.RBP)]
		[InlineData("48 0FCE", 3, Code.Bswap_r64, Register.RSI)]
		[InlineData("48 0FCF", 3, Code.Bswap_r64, Register.RDI)]
		[InlineData("49 0FC8", 3, Code.Bswap_r64, Register.R8)]
		[InlineData("49 0FC9", 3, Code.Bswap_r64, Register.R9)]
		[InlineData("49 0FCA", 3, Code.Bswap_r64, Register.R10)]
		[InlineData("49 0FCB", 3, Code.Bswap_r64, Register.R11)]
		[InlineData("49 0FCC", 3, Code.Bswap_r64, Register.R12)]
		[InlineData("49 0FCD", 3, Code.Bswap_r64, Register.R13)]
		[InlineData("49 0FCE", 3, Code.Bswap_r64, Register.R14)]
		[InlineData("49 0FCF", 3, Code.Bswap_r64, Register.R15)]

		[InlineData("4E 0FC8", 3, Code.Bswap_r64, Register.RAX)]
		[InlineData("4E 0FC9", 3, Code.Bswap_r64, Register.RCX)]
		[InlineData("4E 0FCA", 3, Code.Bswap_r64, Register.RDX)]
		[InlineData("4E 0FCB", 3, Code.Bswap_r64, Register.RBX)]
		[InlineData("4E 0FCC", 3, Code.Bswap_r64, Register.RSP)]
		[InlineData("4E 0FCD", 3, Code.Bswap_r64, Register.RBP)]
		[InlineData("4E 0FCE", 3, Code.Bswap_r64, Register.RSI)]
		[InlineData("4E 0FCF", 3, Code.Bswap_r64, Register.RDI)]
		[InlineData("4F 0FC8", 3, Code.Bswap_r64, Register.R8)]
		[InlineData("4F 0FC9", 3, Code.Bswap_r64, Register.R9)]
		[InlineData("4F 0FCA", 3, Code.Bswap_r64, Register.R10)]
		[InlineData("4F 0FCB", 3, Code.Bswap_r64, Register.R11)]
		[InlineData("4F 0FCC", 3, Code.Bswap_r64, Register.R12)]
		[InlineData("4F 0FCD", 3, Code.Bswap_r64, Register.R13)]
		[InlineData("4F 0FCE", 3, Code.Bswap_r64, Register.R14)]
		[InlineData("4F 0FCF", 3, Code.Bswap_r64, Register.R15)]
		void Test64_Bswap_Reg64_1(string hexBytes, int byteLength, Code code, Register register) {
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
