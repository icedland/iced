// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if FAST_FMT
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests.Fast {
	public sealed class MiscTests {
		[Fact]
		void Verify_default_formatter_options() {
			var options = new FastFormatter().Options;
			Assert.False(options.SpaceAfterOperandSeparator);
			Assert.False(options.AlwaysShowSegmentRegister);
			Assert.True(options.UppercaseHex);
			Assert.False(options.UseHexPrefix);
			Assert.False(options.AlwaysShowMemorySize);
			Assert.False(options.RipRelativeAddresses);
			Assert.True(options.UsePseudoOps);
			Assert.False(options.ShowSymbolAddress);
		}
	}
}
#endif
