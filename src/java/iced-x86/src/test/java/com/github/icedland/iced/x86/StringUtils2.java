// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86;

public final class StringUtils2 {
	public static boolean isNullOrWhiteSpace(String s) {
		return s == null || s.isBlank();
	}

	public static String[] split(String s, String regex, int expectedLength) {
		String[] result = s.split(regex);
		if (result.length >= expectedLength)
			return result;
		String[] padded = new String[expectedLength];
		for (int i = 0; i < expectedLength; i++) {
			if (i < result.length)
				padded[i] = result[i];
			else
				padded[i] = "";
		}
		return padded;
	}
}
