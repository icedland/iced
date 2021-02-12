// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

#if MASM
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests.Masm {
	public sealed class OpIndexTests : FormatterTests.OpIndexTests {
		[Fact]
		void Test() => TestBase(FormatterFactory.Create());
	}
}
#endif
