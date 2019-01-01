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

using System.Collections.Generic;
using Xunit;

namespace Iced.UnitTests.Intel.DecoderTests {
	public sealed class DecoderTest16_000 : DecoderTest {
		[Theory]
		[MemberData(nameof(Data))]
		void DecoderTest(int bitness, int lineNo, string hexBytes, DecoderTestCase tc) =>
			DecoderTestBase(bitness, lineNo, hexBytes, tc);
		public static IEnumerable<object[]> Data => GetDecoderTestData(16, 0);
	}

	public sealed class DecoderTest16_001 : DecoderTest {
		[Theory]
		[MemberData(nameof(Data))]
		void DecoderTest(int bitness, int lineNo, string hexBytes, DecoderTestCase tc) =>
			DecoderTestBase(bitness, lineNo, hexBytes, tc);
		public static IEnumerable<object[]> Data => GetDecoderTestData(16, 1);
	}

	public sealed class DecoderTest16_002 : DecoderTest {
		[Theory]
		[MemberData(nameof(Data))]
		void DecoderTest(int bitness, int lineNo, string hexBytes, DecoderTestCase tc) =>
			DecoderTestBase(bitness, lineNo, hexBytes, tc);
		public static IEnumerable<object[]> Data => GetDecoderTestData(16, 2);
	}

	public sealed class DecoderTest16_003 : DecoderTest {
		[Theory]
		[MemberData(nameof(Data))]
		void DecoderTest(int bitness, int lineNo, string hexBytes, DecoderTestCase tc) =>
			DecoderTestBase(bitness, lineNo, hexBytes, tc);
		public static IEnumerable<object[]> Data => GetDecoderTestData(16, 3);
	}

	public sealed class DecoderTest16_004 : DecoderTest {
		[Theory]
		[MemberData(nameof(Data))]
		void DecoderTest(int bitness, int lineNo, string hexBytes, DecoderTestCase tc) =>
			DecoderTestBase(bitness, lineNo, hexBytes, tc);
		public static IEnumerable<object[]> Data => GetDecoderTestData(16, 4);
	}

	public sealed class DecoderTest16_005 : DecoderTest {
		[Theory]
		[MemberData(nameof(Data))]
		void DecoderTest(int bitness, int lineNo, string hexBytes, DecoderTestCase tc) =>
			DecoderTestBase(bitness, lineNo, hexBytes, tc);
		public static IEnumerable<object[]> Data => GetDecoderTestData(16, 5);
	}

	public sealed class DecoderTest16_006 : DecoderTest {
		[Theory]
		[MemberData(nameof(Data))]
		void DecoderTest(int bitness, int lineNo, string hexBytes, DecoderTestCase tc) =>
			DecoderTestBase(bitness, lineNo, hexBytes, tc);
		public static IEnumerable<object[]> Data => GetDecoderTestData(16, 6);
	}

	public sealed class DecoderTest16_007 : DecoderTest {
		[Theory]
		[MemberData(nameof(Data))]
		void DecoderTest(int bitness, int lineNo, string hexBytes, DecoderTestCase tc) =>
			DecoderTestBase(bitness, lineNo, hexBytes, tc);
		public static IEnumerable<object[]> Data => GetDecoderTestData(16, 7);
	}
}
