// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.internal.fmt;

/** DO NOT USE: INTERNAL API */
public final class StringUtils {
	private StringUtils() {
	}

	public static boolean isNullOrEmpty(String s) {
		return s == null || s.length() == 0;
	}
}
