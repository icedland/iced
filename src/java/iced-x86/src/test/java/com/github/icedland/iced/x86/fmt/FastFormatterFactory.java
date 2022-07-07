// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

import com.github.icedland.iced.x86.fmt.fast.FastFormatter;

final class FastFormatterFactory {
	public static FastFormatter create_Default() {
		return new FastFormatter();
	}

	public static FastFormatter create_Inverted() {
		FastFormatter fast = new FastFormatter();
		fast.getOptions().setSpaceAfterOperandSeparator(!fast.getOptions().getSpaceAfterOperandSeparator());
		fast.getOptions().setRipRelativeAddresses(!fast.getOptions().getRipRelativeAddresses());
		fast.getOptions().setUsePseudoOps(!fast.getOptions().getUsePseudoOps());
		fast.getOptions().setShowSymbolAddress(!fast.getOptions().getShowSymbolAddress());
		fast.getOptions().setAlwaysShowSegmentRegister(!fast.getOptions().getAlwaysShowSegmentRegister());
		fast.getOptions().setAlwaysShowMemorySize(!fast.getOptions().getAlwaysShowMemorySize());
		fast.getOptions().setUppercaseHex(!fast.getOptions().getUppercaseHex());
		fast.getOptions().setUseHexPrefix(!fast.getOptions().getUseHexPrefix());
		return fast;
	}

	public static FastFormatter create_Options() {
		FastFormatter fast = new FastFormatter();
		fast.getOptions().setRipRelativeAddresses(true);
		return fast;
	}

	public static SymbolResolverTests.FastInfo create_Resolver(SymbolResolver symbolResolver) {
		FastFormatter fast = new FastFormatter(symbolResolver);
		fast.getOptions().setRipRelativeAddresses(true);
		return new SymbolResolverTests.FastInfo(fast, symbolResolver);
	}
}
