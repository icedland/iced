// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86;

/**
 * A register passed to `Instruction.create()` methods.
 *
 * @see ICRegisters
 */
public final class ICRegister {
	/** No register */
	public static final ICRegister NONE = new ICRegister(Register.NONE);

	private final int register;

	/**
	 * Constructor. This ctor should normally not be called, use {@link ICRegisters} instead.
	 *
	 * @param register Register (a {@link Register} enum variant)
	 */
	public ICRegister(int register) {
		this.register = register;
	}

	/**
	 * Gets the register (a {@link Register} enum variant)
	 */
	public int get() {
		return register;
	}

	@Override
	public int hashCode() {
		final int prime = 31;
		int result = 1;
		result = prime * result + register;
		return result;
	}

	@Override
	public boolean equals(Object obj) {
		if (obj == null)
			return false;
		if (getClass() != obj.getClass())
			return false;
		ICRegister other = (ICRegister)obj;
		return register == other.register;
	}
}
