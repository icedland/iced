// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System.Collections.Generic;
using Xunit;

namespace Iced.UnitTests.Intel.DecoderTests {
	public sealed class DecoderTest16 : DecoderTest {
		[Theory]
		[MemberData(nameof(Data))]
		void DecoderTest(int bitness, int lineNo, string hexBytes, DecoderTestCase tc) =>
			DecoderTestBase(bitness, lineNo, hexBytes, tc);
		public static IEnumerable<object[]> Data => GetDecoderTestData(16);
	}

	public sealed class DecoderTestMisc16 : DecoderTest {
		[Theory]
		[MemberData(nameof(Data))]
		void DecoderTestMisc(int bitness, int lineNo, string hexBytes, DecoderTestCase tc) =>
			DecoderTestBase(bitness, lineNo, hexBytes, tc);
		public static IEnumerable<object[]> Data => GetMiscDecoderTestData(16);
	}
}
