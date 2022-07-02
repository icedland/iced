// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

public final class SymbolResolverTestCase {
	final int bitness;
	final String hexBytes;
	final long ip;
	final int code;
	final OptionsTestCase.Info[] options;
	final SymbolResultTestCase[] symbolResults;

	SymbolResolverTestCase(int bitness, String hexBytes, long ip, int code, OptionsTestCase.Info[] options, SymbolResultTestCase[] symbolResults) {
		this.bitness = bitness;
		this.hexBytes = hexBytes;
		this.ip = ip;
		this.code = code;
		this.options = options;
		this.symbolResults = symbolResults;
	}
}
