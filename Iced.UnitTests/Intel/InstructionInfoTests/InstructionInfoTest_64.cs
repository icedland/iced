/*
    Copyright (C) 2018 de4dot@gmail.com

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

#if !NO_INSTR_INFO
using System.Collections.Generic;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.InstructionInfoTests {
	public sealed class InstructionInfoTest_64 : InstructionInfoTest {
		[Theory]
		[MemberData(nameof(Test64_InstructionInfo_Data))]
		void Test64_InstructionInfo(string hexBytes, Code code, int lineNo, InstructionInfoTestCase testCase) =>
			TestInstructionInfo(64, hexBytes, code, lineNo, testCase);
		public static IEnumerable<object[]> Test64_InstructionInfo_Data => GetTestCases(64, nameof(InstructionInfoTest_64));
	}
}
#endif
