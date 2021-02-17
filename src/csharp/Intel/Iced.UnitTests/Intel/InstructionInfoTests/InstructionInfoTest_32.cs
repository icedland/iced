// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if INSTR_INFO
using System.Collections.Generic;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.InstructionInfoTests {
	public sealed class InstructionInfoTest_32 : InstructionInfoTest {
		[Theory]
		[MemberData(nameof(Test32_InstructionInfo_Data))]
		void Test32_InstructionInfo(string hexBytes, Code code, DecoderOptions options, int lineNo, InstructionInfoTestCase testCase) =>
			TestInstructionInfo(32, hexBytes, code, options, lineNo, testCase);
		public static IEnumerable<object[]> Test32_InstructionInfo_Data => GetTestCases(32);
	}
}
#endif
