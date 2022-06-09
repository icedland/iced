// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86;

/**
 * Used by an {@link com.github.icedland.iced.x86.enc.Encoder} to write encoded instructions
 */
public interface CodeWriter {
	/**
	 * Writes the next byte
	 *
	 * @param value Value
	 */
	public abstract void writeByte(byte value);
}
