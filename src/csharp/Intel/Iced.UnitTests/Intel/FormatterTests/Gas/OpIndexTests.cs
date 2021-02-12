// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

#if GAS
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests.Gas {
	public sealed class OpIndexTests : FormatterTests.OpIndexTests {
		[Fact]
		void Test() => TestBase(FormatterFactory.Create());
	}
}
#endif
