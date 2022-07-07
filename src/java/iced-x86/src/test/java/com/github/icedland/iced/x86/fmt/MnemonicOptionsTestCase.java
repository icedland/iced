// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

public final class MnemonicOptionsTestCase {
	public final String hexBytes;
	public final int code;
	public final int bitness;
	public final long ip;
	public final String formattedString;
	public final int flags;

	public MnemonicOptionsTestCase(String hexBytes, int code, int bitness, long ip, String formattedString, int flags) {
		this.hexBytes = hexBytes;
		this.code = code;
		this.bitness = bitness;
		this.ip = ip;
		this.formattedString = formattedString;
		this.flags = flags;
	}
}
