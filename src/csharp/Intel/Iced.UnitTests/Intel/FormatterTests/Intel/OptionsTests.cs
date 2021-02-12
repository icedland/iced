// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

#if INTEL
using System.Collections.Generic;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests.Intel {
	public sealed class OptionsTests : FormatterTests.OptionsTests {
		[Theory]
		[MemberData(nameof(FormatCommon_Data))]
		void FormatCommon(int index, OptionsInstructionInfo info, string formattedString) => FormatBase(index, info, formattedString, FormatterFactory.Create_Options());
		public static IEnumerable<object[]> FormatCommon_Data => OptionsTestsUtils.GetFormatData_Common("Intel", "OptionsResult.Common");

		[Theory]
		[MemberData(nameof(FormatAll_Data))]
		void FormatAll(int index, OptionsInstructionInfo info, string formattedString) => FormatBase(index, info, formattedString, FormatterFactory.Create_Options());
		public static IEnumerable<object[]> FormatAll_Data => OptionsTestsUtils.GetFormatData_All("Intel", "OptionsResult");

		[Theory]
		[MemberData(nameof(Format2_Data))]
		void Format2(int index, OptionsInstructionInfo info, string formattedString) => FormatBase(index, info, formattedString, FormatterFactory.Create_Options());
		public static IEnumerable<object[]> Format2_Data => OptionsTestsUtils.GetFormatData("Intel", "OptionsResult2", "Options2");

		[Fact]
		public void TestOptions() {
			var options = FormatterOptions.CreateIntel();
			TestOptionsBase(options);
		}
	}
}
#endif
