/*
    Copyright (C) 2018-2019 de4dot@gmail.com

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

#if !NO_ENCODER
using System.Collections.Generic;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.EncoderTests {
	public sealed class EncoderTest32 : EncoderTest {
		[Theory]
		[MemberData(nameof(Encode_Data))]
		void Encode(int codeSize, Code code, string hexBytes, DecoderOptions options) => EncodeBase(codeSize, code, hexBytes, options);
		public static IEnumerable<object[]> Encode_Data => GetEncodeData(32);

		[Theory]
		[MemberData(nameof(NonDecodeEncode_Data))]
		void NonDecodeEncode(int codeSize, Instruction instr, string hexBytes, ulong rip) => NonDecodeEncodeBase(codeSize, ref instr, hexBytes, rip);
		public static IEnumerable<object[]> NonDecodeEncode_Data => GetNonDecodedEncodeData(32);
	}
}
#endif
