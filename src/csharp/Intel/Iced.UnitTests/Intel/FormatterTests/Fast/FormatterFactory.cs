// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if FAST_FMT
using Iced.Intel;

namespace Iced.UnitTests.Intel.FormatterTests.Fast {
	static class FormatterFactory {
		public static FastFormatter Create_Default() => new FastFormatter();

		public static FastFormatter Create_Inverted() {
			var fast = new FastFormatter();
			fast.Options.SpaceAfterOperandSeparator ^= true;
			fast.Options.RipRelativeAddresses ^= true;
			fast.Options.UsePseudoOps ^= true;
			fast.Options.ShowSymbolAddress ^= true;
			fast.Options.AlwaysShowSegmentRegister ^= true;
			fast.Options.AlwaysShowMemorySize ^= true;
			fast.Options.UppercaseHex ^= true;
			fast.Options.UseHexPrefix ^= true;
			return fast;
		}

		public static FastFormatter Create_Options() {
			var fast = new FastFormatter();
			fast.Options.RipRelativeAddresses = true;
			return fast;
		}

		public static (FastFormatter formatter, ISymbolResolver symbolResolver) Create_Resolver(ISymbolResolver symbolResolver) {
			var fast = new FastFormatter(symbolResolver);
			fast.Options.RipRelativeAddresses = true;
			return (fast, symbolResolver);
		}
	}
}
#endif
