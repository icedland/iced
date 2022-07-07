// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt.masm;

final class SymbolOptionsTestCase {
	public final String hexBytes;
	public final int bitness;
	public final long ip;
	public final String formattedString;
	public final int flags;

	public SymbolOptionsTestCase(String hexBytes, int bitness, long ip, String formattedString, int flags) {
		this.hexBytes = hexBytes;
		this.bitness = bitness;
		this.ip = ip;
		this.formattedString = formattedString;
		this.flags = flags;
	}
}
