// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.internal.fmt;

import java.util.Locale;

/** DO NOT USE: INTERNAL API */
public final class FormatterString {
	private final String lower;
	private final String upper;

	public int getLength() {
		return lower.length();
	}

	public FormatterString(String lower) {
		assert lower.toLowerCase(Locale.ROOT) == lower : lower;
		this.lower = lower;
		upper = lower.toUpperCase(Locale.ROOT).intern();
	}

	public static FormatterString[] create(String[] strings) {
		FormatterString[] res = new FormatterString[strings.length];
		for (int i = 0; i < strings.length; i++)
			res[i] = new FormatterString(strings[i]);
		return res;
	}

	public String get(boolean upper) {
		return upper ? this.upper : lower;
	}

	public String getLower() {
		return lower;
	}
}
