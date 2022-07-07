// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

final class SymbolResultTestCase {
	final long address;
	final long symbolAddress;
	final int addressSize;
	final int flags;
	final Integer memorySize;
	final String[] symbolParts;

	SymbolResultTestCase(long address, long symbolAddress, int addressSize, int flags, Integer memorySize, String[] symbolParts) {
		this.address = address;
		this.symbolAddress = symbolAddress;
		this.addressSize = addressSize;
		this.flags = flags;
		this.memorySize = memorySize;
		this.symbolParts = symbolParts;
	}
}
