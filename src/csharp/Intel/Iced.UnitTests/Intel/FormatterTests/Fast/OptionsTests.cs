// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if FAST_FMT
using System.Collections.Generic;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests.Fast {
	public sealed class OptionsTests : FastOptionsTests {
		[Theory]
		[MemberData(nameof(FormatCommon_Data))]
		void FormatCommon(int index, OptionsTestCase tc, string formattedString) => FormatBase(index, tc, formattedString, FormatterFactory.Create_Options());
		public static IEnumerable<object[]> FormatCommon_Data => OptionsTestsUtils.GetFormatData_Common("Fast", "OptionsResult.Common");

		[Theory]
		[MemberData(nameof(Format2_Data))]
		void Format2(int index, OptionsTestCase tc, string formattedString) => FormatBase(index, tc, formattedString, FormatterFactory.Create_Options());
		public static IEnumerable<object[]> Format2_Data => OptionsTestsUtils.GetFormatData("Fast", "OptionsResult2", "Options2");
	}
}
#endif
