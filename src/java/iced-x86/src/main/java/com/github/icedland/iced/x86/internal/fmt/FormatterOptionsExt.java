// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.internal.fmt;

import com.github.icedland.iced.x86.fmt.FormatterOptions;

/**
 * DO NOT USE: INTERNAL API
 *
 * @deprecated Not part of the public API
 */
@Deprecated
public final class FormatterOptionsExt {
	private FormatterOptionsExt() {
	}

	public static FormatterOptions createGas() {
		FormatterOptions options = new FormatterOptions();
		options.setHexPrefix("0x");
		options.setOctalPrefix("0");
		options.setBinaryPrefix("0b");
		return options;
	}

	public static FormatterOptions createIntel() {
		FormatterOptions options = new FormatterOptions();
		options.setHexSuffix("h");
		options.setOctalSuffix("o");
		options.setBinarySuffix("b");
		return options;
	}

	public static FormatterOptions createMasm() {
		FormatterOptions options = new FormatterOptions();
		options.setHexSuffix("h");
		options.setOctalSuffix("o");
		options.setBinarySuffix("b");
		return options;
	}

	public static FormatterOptions createNasm() {
		FormatterOptions options = new FormatterOptions();
		options.setHexSuffix("h");
		options.setOctalSuffix("o");
		options.setBinarySuffix("b");
		return options;
	}
}
