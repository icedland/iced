// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if FAST_FMT
using System.Collections.Generic;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests.Fast {
	public sealed class SymbolResolverTests : FastSymbolResolverTests {
		[Theory]
		[MemberData(nameof(Format_Data))]
		void Format(int index, SymbolResolverTestCase tc, string formattedString) => FormatBase(index, tc, formattedString, FormatterFactory.Create_Resolver(new TestSymbolResolver(tc)));
		public static IEnumerable<object[]> Format_Data => SymbolResolverTestUtils.GetFormatData("Fast", "SymbolResolverTests");
	}
}
#endif
