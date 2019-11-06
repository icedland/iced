/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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
