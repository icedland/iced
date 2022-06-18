// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86;

/**
 * A register passed to `Instruction.create()` methods.
 *
 * @see ICRegisters
 */
public final class ICRegister {
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
}
