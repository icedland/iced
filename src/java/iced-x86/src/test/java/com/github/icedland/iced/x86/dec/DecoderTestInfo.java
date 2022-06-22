// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.dec;

public final class DecoderTestInfo {
	public final int id;
	public final int bitness;
	public final long ip;
	public final int code;
	public final String hexBytes;
	public final String encodedHexBytes;
	public final int options;
	public final int testOptions;

	public DecoderTestInfo(int id, int bitness, long ip, int code, String hexBytes, String encodedHexBytes, int options, int testOptions) {
		this.id = id;
		this.bitness = bitness;
		this.ip = ip;
		this.code = code;
		this.hexBytes = hexBytes;
		this.encodedHexBytes = encodedHexBytes;
		this.options = options;
		this.testOptions = testOptions;
	}
}
