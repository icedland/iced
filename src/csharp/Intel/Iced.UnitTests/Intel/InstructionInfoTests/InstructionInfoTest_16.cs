// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

#if INSTR_INFO
using System.Collections.Generic;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.InstructionInfoTests {
	public sealed class InstructionInfoTest_16 : InstructionInfoTest {
		[Theory]
		[MemberData(nameof(Test16_InstructionInfo_Data))]
		void Test16_InstructionInfo(string hexBytes, Code code, DecoderOptions options, int lineNo, InstructionInfoTestCase testCase) =>
			TestInstructionInfo(16, hexBytes, code, options, lineNo, testCase);
		public static IEnumerable<object[]> Test16_InstructionInfo_Data => GetTestCases(16);
	}
}
#endif
