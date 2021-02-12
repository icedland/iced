// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

#if INTEL
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests.Intel {
	public sealed class OpIndexTests : FormatterTests.OpIndexTests {
		[Fact]
		void Test() => TestBase(FormatterFactory.Create());
	}
}
#endif
