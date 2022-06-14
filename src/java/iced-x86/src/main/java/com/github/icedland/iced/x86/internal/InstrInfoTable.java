// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.internal;

/**
 * DO NOT USE: INTERNAL API
 */
public final class InstrInfoTable {
	private InstrInfoTable() {
	}

	/**
	 * DO NOT USE: INTERNAL API
	 */
	public static final int[] data = ResourceReader.readIntArray(InstrInfoTable.class.getClassLoader(),
			"com/github/icedland/iced/x86/info/InstrInfoTable.bin");
}
