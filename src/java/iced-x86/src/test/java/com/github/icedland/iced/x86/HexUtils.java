// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86;

public final class HexUtils {
	public static byte[] toByteArray(String hexData) {
		if (hexData == null)
			throw new NullPointerException();
		if (hexData.length() == 0)
			return new byte[0];
		int len = 0;
		for (int i = 0; i < hexData.length(); i++) {
			if (hexData.charAt(i) != ' ')
				len++;
		}
		byte[] data = new byte[len / 2];
		int w = 0;
		for (int i = 0; ;) {
			while (i < hexData.length() && Character.isWhitespace(hexData.charAt(i)))
				i++;
			if (i >= hexData.length())
				break;
			int hi = tryParseHexChar(hexData.charAt(i++));
			if (hi < 0)
				throw new UnsupportedOperationException();

			while (i < hexData.length() && Character.isWhitespace(hexData.charAt(i)))
				i++;
			if (i >= hexData.length())
				throw new UnsupportedOperationException();
			int lo = tryParseHexChar(hexData.charAt(i++));
			if (lo < 0)
				throw new UnsupportedOperationException();
			data[w++] = (byte)((hi << 4) | lo);
		}
		if (w != data.length)
			throw new UnsupportedOperationException();
		return data;
	}

	private static int tryParseHexChar(char c) {
		if ('0' <= c && c <= '9')
			return c - '0';
		if ('A' <= c && c <= 'F')
			return c - 'A' + 10;
		if ('a' <= c && c <= 'f')
			return c - 'a' + 10;
		return -1;
	}
}
