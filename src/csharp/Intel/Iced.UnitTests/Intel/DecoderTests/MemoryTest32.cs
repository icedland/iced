// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

using System.Collections.Generic;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.DecoderTests {
	public sealed class MemoryTest32 : DecoderTest {
		[Theory]
		[MemberData(nameof(Test32_DecodeMemOps_as16_Data))]
		void Test32_DecodeMemOps(string hexBytes, Code code, Register register, Register prefixSeg, Register segReg, Register baseReg, Register indexReg, int scale, ulong displ, int displSize, ConstantOffsets constantOffsets, string encodedHexBytes, DecoderOptions options) =>
			DecodeMemOpsBase(32, hexBytes, code, register, prefixSeg, segReg, baseReg, indexReg, scale, displ, displSize, constantOffsets, encodedHexBytes, options);
		public static IEnumerable<object[]> Test32_DecodeMemOps_as16_Data => GetMemOpsData(32);
	}
}
