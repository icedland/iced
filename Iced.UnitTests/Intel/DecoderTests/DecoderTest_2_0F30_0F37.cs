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
	public sealed class DecoderTest_2_0F30_0F37 : DecoderTest {
		[Theory]
		[InlineData("0F30", 2, Code.Wrmsr, DecoderOptions.None)]
		[InlineData("0F31", 2, Code.Rdtsc, DecoderOptions.None)]
		[InlineData("0F32", 2, Code.Rdmsr, DecoderOptions.None)]
		[InlineData("0F33", 2, Code.Rdpmc, DecoderOptions.None)]
		[InlineData("0F34", 2, Code.Sysenter, DecoderOptions.None)]
		[InlineData("0F34", 2, Code.Wrecr, DecoderOptions.Ecr)]
		[InlineData("0F35", 2, Code.Sysexitd, DecoderOptions.None)]
		[InlineData("0F36", 2, Code.Rdecr, DecoderOptions.Ecr)]
		[InlineData("0F37", 2, Code.Getsec, DecoderOptions.None)]
		void Test16_Simple_1(string hexBytes, int byteLength, Code code, DecoderOptions options) {
			var decoder = CreateDecoder16(hexBytes, options);
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
		[InlineData("0F30", 2, Code.Wrmsr, DecoderOptions.None)]
		[InlineData("0F31", 2, Code.Rdtsc, DecoderOptions.None)]
		[InlineData("0F32", 2, Code.Rdmsr, DecoderOptions.None)]
		[InlineData("0F33", 2, Code.Rdpmc, DecoderOptions.None)]
		[InlineData("0F34", 2, Code.Sysenter, DecoderOptions.None)]
		[InlineData("0F34", 2, Code.Wrecr, DecoderOptions.Ecr)]
		[InlineData("0F35", 2, Code.Sysexitd, DecoderOptions.None)]
		[InlineData("0F36", 2, Code.Rdecr, DecoderOptions.Ecr)]
		[InlineData("0F37", 2, Code.Getsec, DecoderOptions.None)]
		void Test32_Simple_1(string hexBytes, int byteLength, Code code, DecoderOptions options) {
			var decoder = CreateDecoder32(hexBytes, options);
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
		[InlineData("0F30", 2, Code.Wrmsr, DecoderOptions.None)]
		[InlineData("0F31", 2, Code.Rdtsc, DecoderOptions.None)]
		[InlineData("0F32", 2, Code.Rdmsr, DecoderOptions.None)]
		[InlineData("0F33", 2, Code.Rdpmc, DecoderOptions.None)]
		[InlineData("0F34", 2, Code.Sysenter, DecoderOptions.None)]
		[InlineData("0F35", 2, Code.Sysexitd, DecoderOptions.None)]
		[InlineData("48 0F35", 3, Code.Sysexitq, DecoderOptions.None)]
		[InlineData("0F37", 2, Code.Getsec, DecoderOptions.None)]
		void Test64_Simple_1(string hexBytes, int byteLength, Code code, DecoderOptions options) {
			var decoder = CreateDecoder64(hexBytes, options);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}
	}
}
