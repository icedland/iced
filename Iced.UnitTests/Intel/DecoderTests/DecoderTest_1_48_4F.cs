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
	public sealed class DecoderTest_1_48_4F : DecoderTest {
		[Theory]
		[InlineData("48", 1, Code.Dec_AX, Register.AX)]
		[InlineData("49", 1, Code.Dec_CX, Register.CX)]
		[InlineData("4A", 1, Code.Dec_DX, Register.DX)]
		[InlineData("4B", 1, Code.Dec_BX, Register.BX)]
		[InlineData("4C", 1, Code.Dec_SP, Register.SP)]
		[InlineData("4D", 1, Code.Dec_BP, Register.BP)]
		[InlineData("4E", 1, Code.Dec_SI, Register.SI)]
		[InlineData("4F", 1, Code.Dec_DI, Register.DI)]
		void Test16_Dec_R16_1(string hexBytes, int byteLength, Code code, Register register) {
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
		[InlineData("66 48", 2, Code.Dec_AX, Register.AX)]
		[InlineData("66 49", 2, Code.Dec_CX, Register.CX)]
		[InlineData("66 4A", 2, Code.Dec_DX, Register.DX)]
		[InlineData("66 4B", 2, Code.Dec_BX, Register.BX)]
		[InlineData("66 4C", 2, Code.Dec_SP, Register.SP)]
		[InlineData("66 4D", 2, Code.Dec_BP, Register.BP)]
		[InlineData("66 4E", 2, Code.Dec_SI, Register.SI)]
		[InlineData("66 4F", 2, Code.Dec_DI, Register.DI)]
		void Test32_Dec_R16_1(string hexBytes, int byteLength, Code code, Register register) {
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
		[InlineData("66 48", 2, Code.Dec_EAX, Register.EAX)]
		[InlineData("66 49", 2, Code.Dec_ECX, Register.ECX)]
		[InlineData("66 4A", 2, Code.Dec_EDX, Register.EDX)]
		[InlineData("66 4B", 2, Code.Dec_EBX, Register.EBX)]
		[InlineData("66 4C", 2, Code.Dec_ESP, Register.ESP)]
		[InlineData("66 4D", 2, Code.Dec_EBP, Register.EBP)]
		[InlineData("66 4E", 2, Code.Dec_ESI, Register.ESI)]
		[InlineData("66 4F", 2, Code.Dec_EDI, Register.EDI)]
		void Test16_Dec_R32_1(string hexBytes, int byteLength, Code code, Register register) {
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
		[InlineData("48", 1, Code.Dec_EAX, Register.EAX)]
		[InlineData("49", 1, Code.Dec_ECX, Register.ECX)]
		[InlineData("4A", 1, Code.Dec_EDX, Register.EDX)]
		[InlineData("4B", 1, Code.Dec_EBX, Register.EBX)]
		[InlineData("4C", 1, Code.Dec_ESP, Register.ESP)]
		[InlineData("4D", 1, Code.Dec_EBP, Register.EBP)]
		[InlineData("4E", 1, Code.Dec_ESI, Register.ESI)]
		[InlineData("4F", 1, Code.Dec_EDI, Register.EDI)]
		void Test32_Dec_R32_1(string hexBytes, int byteLength, Code code, Register register) {
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
	}
}
