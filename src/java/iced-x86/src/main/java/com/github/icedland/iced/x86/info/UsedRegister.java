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
	 * Returns a copy of this instance
	 */
	public UsedRegister copy() {
		return new UsedRegister(getRegister(), getAccess());
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

	@Override
	public int hashCode() {
		final int prime = 31;
		int result = 1;
		result = prime * result + access;
		result = prime * result + register;
		return result;
	}

	@Override
	public boolean equals(Object obj) {
		if (obj == null)
			return false;
		if (getClass() != obj.getClass())
			return false;
		UsedRegister other = (UsedRegister)obj;
		return access == other.access && register == other.register;
	}
}
