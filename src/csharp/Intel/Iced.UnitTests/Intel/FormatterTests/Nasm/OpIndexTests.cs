// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

#if NASM
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests.Nasm {
	public sealed class OpIndexTests : FormatterTests.OpIndexTests {
		[Fact]
		void Test() => TestBase(FormatterFactory.Create());
	}
}
#endif
