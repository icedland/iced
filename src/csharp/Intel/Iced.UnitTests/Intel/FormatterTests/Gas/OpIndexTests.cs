// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if GAS
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests.Gas {
	public sealed class OpIndexTests : FormatterTests.OpIndexTests {
		[Fact]
		void Test() => TestBase(FormatterFactory.Create());
	}
}
#endif
