// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

public final class FormatterTestCase {
	public final int bitness;
	public final String hexBytes;
	public final long ip;
	public final int code;
	public final int options;

	public FormatterTestCase(int bitness, String hexBytes, long ip, int code, int options) {
		this.bitness = bitness;
		this.hexBytes = hexBytes;
		this.ip = ip;
		this.code = code;
		this.options = options;
	}
}
