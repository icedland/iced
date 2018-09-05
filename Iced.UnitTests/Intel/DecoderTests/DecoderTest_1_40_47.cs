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
	public sealed class DecoderTest_1_40_47 : DecoderTest {
		[Theory]
		[InlineData("40", 1, Code.Inc_AX, Register.AX)]
		[InlineData("41", 1, Code.Inc_CX, Register.CX)]
		[InlineData("42", 1, Code.Inc_DX, Register.DX)]
		[InlineData("43", 1, Code.Inc_BX, Register.BX)]
		[InlineData("44", 1, Code.Inc_SP, Register.SP)]
		[InlineData("45", 1, Code.Inc_BP, Register.BP)]
		[InlineData("46", 1, Code.Inc_SI, Register.SI)]
		[InlineData("47", 1, Code.Inc_DI, Register.DI)]
		void Test16_Inc_R16_1(string hexBytes, int byteLength, Code code, Register register) {
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
		[InlineData("66 40", 2, Code.Inc_AX, Register.AX)]
		[InlineData("66 41", 2, Code.Inc_CX, Register.CX)]
		[InlineData("66 42", 2, Code.Inc_DX, Register.DX)]
		[InlineData("66 43", 2, Code.Inc_BX, Register.BX)]
		[InlineData("66 44", 2, Code.Inc_SP, Register.SP)]
		[InlineData("66 45", 2, Code.Inc_BP, Register.BP)]
		[InlineData("66 46", 2, Code.Inc_SI, Register.SI)]
		[InlineData("66 47", 2, Code.Inc_DI, Register.DI)]
		void Test32_Inc_R16_1(string hexBytes, int byteLength, Code code, Register register) {
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
		[InlineData("66 40", 2, Code.Inc_EAX, Register.EAX)]
		[InlineData("66 41", 2, Code.Inc_ECX, Register.ECX)]
		[InlineData("66 42", 2, Code.Inc_EDX, Register.EDX)]
		[InlineData("66 43", 2, Code.Inc_EBX, Register.EBX)]
		[InlineData("66 44", 2, Code.Inc_ESP, Register.ESP)]
		[InlineData("66 45", 2, Code.Inc_EBP, Register.EBP)]
		[InlineData("66 46", 2, Code.Inc_ESI, Register.ESI)]
		[InlineData("66 47", 2, Code.Inc_EDI, Register.EDI)]
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
		[InlineData("40", 1, Code.Inc_EAX, Register.EAX)]
		[InlineData("41", 1, Code.Inc_ECX, Register.ECX)]
		[InlineData("42", 1, Code.Inc_EDX, Register.EDX)]
		[InlineData("43", 1, Code.Inc_EBX, Register.EBX)]
		[InlineData("44", 1, Code.Inc_ESP, Register.ESP)]
		[InlineData("45", 1, Code.Inc_EBP, Register.EBP)]
		[InlineData("46", 1, Code.Inc_ESI, Register.ESI)]
		[InlineData("47", 1, Code.Inc_EDI, Register.EDI)]
		void Test32_Inc_R32_1(string hexBytes, int byteLength, Code code, Register register) {
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
