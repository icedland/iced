// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

#if INTEL
using System.Collections.Generic;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests.Intel {
	public sealed class SymbolResolverTests : FormatterTests.SymbolResolverTests {
		[Theory]
		[MemberData(nameof(Format_Data))]
		void Format(int index, SymbolResolverTestCase info, string formattedString) => FormatBase(index, info, formattedString, FormatterFactory.Create_Resolver(new TestSymbolResolver(info)));
		public static IEnumerable<object[]> Format_Data => SymbolResolverTestUtils.GetFormatData("Intel", "SymbolResolverTests");
	}
}
#endif
