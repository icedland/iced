// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if INSTR_INFO
using System.Collections.Generic;
using System.Text;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.InstructionInfoTests {
	public sealed class CodeValueTests {
		[Fact]
		void Make_sure_all_Code_values_are_tested() {
			var tested = new bool[IcedConstants.CodeEnumCount];

			foreach (var info in GetTests())
				tested[(int)(Code)info[1]] = true;

			var sb = new StringBuilder();
			int missing = 0;
			var codeNames = ToEnumConverter.GetCodeNames();
			Assert.Equal(tested.Length, codeNames.Length);
			for (int i = 0; i < tested.Length; i++) {
				if (!tested[i] && !CodeUtils.IsIgnored(codeNames[i])) {
					sb.Append(codeNames[i] + " ");
					missing++;
				}
			}
			Assert.Equal("0 ins ", $"{missing} ins " + sb.ToString());
		}

		static IEnumerable<object[]> GetTests() {
			foreach (var info in InstructionInfoTest_16.Test16_InstructionInfo_Data)
				yield return info;
			foreach (var info in InstructionInfoTest_32.Test32_InstructionInfo_Data)
				yield return info;
			foreach (var info in InstructionInfoTest_64.Test64_InstructionInfo_Data)
				yield return info;
		}
	}
}
#endif
