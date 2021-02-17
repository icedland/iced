// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System.Collections.Generic;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.DecoderTests {
	public sealed class MemoryTest64 : DecoderTest {
		[Theory]
		[MemberData(nameof(Test64_DecodeMemOps_as32_1_Data))]
		void Test64_DecodeMemOps(string hexBytes, Code code, DecoderMemoryTestCase tc) =>
			DecodeMemOpsBase(64, hexBytes, code, tc);
		public static IEnumerable<object[]> Test64_DecodeMemOps_as32_1_Data => GetMemOpsData(64);
	}
}
