// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.dec;

/**
 * Reads instruction bytes
 */
public abstract class CodeReader {
	/**
	 * Reads the next byte ({@code 0x00-0xFF}) or returns less than 0 if there are no more bytes
	 */
	public abstract int readByte();
}
