// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System.Collections.Generic;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.DecoderTests {
	public sealed class MemoryTest16 : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_DecodeMemOps_as16_Data))]
		void Test16_DecodeMemOps(string hexBytes, Code code, DecoderMemoryTestCase tc) =>
			DecodeMemOpsBase(16, hexBytes, code, tc);
		public static IEnumerable<object[]> Test16_DecodeMemOps_as16_Data => GetMemOpsData(16);
	}
}
