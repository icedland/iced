// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.enc;

final class BlockData {
	long __dont_use_address;
	boolean __dont_use_address_initd;

	boolean isValid;

	BlockData(boolean isValid) {
		this.isValid = isValid;
	}

	long getAddress() {
		if (!isValid)
			throw new UnsupportedOperationException();
		if (!__dont_use_address_initd)
			throw new UnsupportedOperationException();
		return __dont_use_address;
	}

	long data;
}
