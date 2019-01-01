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
using Iced.UnitTests.Intel.DecoderTests;
using Xunit;

namespace Iced.UnitTests.Intel.EncoderTests {
	public sealed class EncoderTest_Invalid : EncoderTest {
		[Theory]
		[MemberData(nameof(EncodeInvalid_Data))]
		void EncodeInvalid(int codeSize, Code code, string hexBytes, DecoderOptions options, int invalidCodeSize) => EncodeInvalidBase(codeSize, code, hexBytes, options, invalidCodeSize);
		public static IEnumerable<object[]> EncodeInvalid_Data {
			get {
				foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false)) {
					if (DecoderTestUtils.Code32Only.Contains(info.Code))
						yield return new object[] { info.Bitness, info.Code, info.HexBytes, info.Options, 64 };
					if (DecoderTestUtils.Code64Only.Contains(info.Code)) {
						yield return new object[] { info.Bitness, info.Code, info.HexBytes, info.Options, 16 };
						yield return new object[] { info.Bitness, info.Code, info.HexBytes, info.Options, 32 };
					}
				}
			}
		}
	}
}
#endif
