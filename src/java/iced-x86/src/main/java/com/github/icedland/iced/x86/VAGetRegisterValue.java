// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86;

/**
 * Called when calculating the virtual address of a memory operand
 */
@FunctionalInterface
public interface VAGetRegisterValue {
	/**
	 * Gets a register value. If {@code register} is a segment register, this method should return the segment's base address, not the segment's
	 * register value. If it's not possible to read the register value, this method should return {@code null}
	 *
	 * @param register     Register (GPR8, GPR16, GPR32, GPR64, XMM, YMM, ZMM, seg) (a {@link Register} enum variant)
	 * @param elementIndex Only used if it's a vsib memory operand. This is the element index of the vector index register.
	 * @param elementSize  Only used if it's a vsib memory operand. Size in bytes of elements in vector index register (4 or 8).
	 * @return {@code null} if it failed to read the register, else the value of the register
	 */
	Long get(int register, int elementIndex, int elementSize);
}
