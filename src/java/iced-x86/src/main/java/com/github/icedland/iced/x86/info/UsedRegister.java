// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.info;

/**
 * A register used by an instruction
 */
public final class UsedRegister {
	private final byte register;
	private final byte access;

	/**
	 * Register (a {@link com.github.icedland.iced.x86.Register} enum variant)
	 */
	@SuppressWarnings("deprecation")
	public int getRegister() {
		return register & com.github.icedland.iced.x86.internal.Constants.REG_MASK;
	}

	/**
	 * Register access (an {@link com.github.icedland.iced.x86.info.OpAccess} enum variant)
	 */
	public int getAccess() {
		return access;
	}

	/**
	 * Constructor
	 *
	 * @param register Register (a {@link com.github.icedland.iced.x86.Register} enum variant)
	 * @param access   Register access (an {@link com.github.icedland.iced.x86.info.OpAccess} enum variant)
	 */
	@SuppressWarnings("deprecation")
	UsedRegister(int register, int access) {
		assert Integer.compareUnsigned(register, com.github.icedland.iced.x86.internal.Constants.REG_MASK) <= 0 : register;
		this.register = (byte)register;
		this.access = (byte)access;
	}
}
