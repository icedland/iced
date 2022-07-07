// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86;

/**
 * Vsib flags (returned by {@link Instruction#getVsib()})
 */
public final class VsibFlags {
	private VsibFlags() {
	}

	/**
	 * It's not a vsib instruction (no bit is set)
	 */
	public static final int NONE = 0x00;
	/**
	 * Set if it's a 32-bit or a 64-bit vsib instruction
	 */
	public static final int VSIB = 0x01;
	/**
	 * Set if it's a 32-bit vsib instruction
	 */
	public static final int VSIB32 = 0x02;
	/**
	 * Set if it's a 64-bit vsib instruction
	 */
	public static final int VSIB64 = 0x04;
}
