// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86;

public final class StringUtils2 {
	public static boolean isNullOrWhiteSpace(String s) {
		if (s == null || s.length() == 0)
			return true;
		for (int i = 0; i < s.length(); i++) {
			char c = s.charAt(i);
			if (!Character.isWhitespace(c))
				return false;
		}
		return true;
	}

	public static String[] split(String s, String regex) {
		String charStr = regex;
		if (charStr.startsWith("\\"))
			charStr = charStr.substring(1);
		if (charStr.length() != 1)
			throw new UnsupportedOperationException();

		String[] result = s.split(regex);
		if (!s.endsWith(charStr))
			return result;
		String[] padded = new String[result.length + 1];
		for (int i = 0; i < result.length; i++)
			padded[i] = result[i];
		padded[padded.length - 1] = "";
		return padded;
	}
}
