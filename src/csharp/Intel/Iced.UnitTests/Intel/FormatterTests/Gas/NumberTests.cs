// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if GAS
using System.Collections.Generic;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests.Gas {
	public sealed class NumberTests : FormatterTests.NumberTests {
		[Theory]
		[MemberData(nameof(Format_Data))]
		void Format(int index, object number, string[] formattedStrings) => FormatBase(index, number, formattedStrings, FormatterFactory.Create_Numbers());
		public static IEnumerable<object[]> Format_Data => GetFormatData();
	}
}
#endif
