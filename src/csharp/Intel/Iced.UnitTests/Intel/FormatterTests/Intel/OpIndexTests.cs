// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if INTEL
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests.Intel {
	public sealed class OpIndexTests : FormatterTests.OpIndexTests {
		[Fact]
		void Test() => TestBase(FormatterFactory.Create());
	}
}
#endif
