// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System.Collections.Generic;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.DecoderTests {
	public sealed class MemoryTest32 : DecoderTest {
		[Theory]
		[MemberData(nameof(Test32_DecodeMemOps_as16_Data))]
		void Test32_DecodeMemOps(string hexBytes, Code code, DecoderMemoryTestCase tc) =>
			DecodeMemOpsBase(32, hexBytes, code, tc);
		public static IEnumerable<object[]> Test32_DecodeMemOps_as16_Data => GetMemOpsData(32);
	}
}
