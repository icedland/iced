// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

import com.github.icedland.iced.x86.Instruction;

/**
 * Used by a formatter to resolve symbols
 */
@FunctionalInterface
public interface SymbolResolver {
	/**
	 * Tries to resolve a symbol. Returns {@code null} if there's no symbol.
	 *
	 * @param instruction        Instruction
	 * @param operand            Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand.
	 * @param instructionOperand Instruction operand number, 0-based, or -1 if it's an operand created by the formatter.
	 * @param address            Address
	 * @param addressSize        Size of {@code address} in bytes (eg. 1, 2, 4 or 8)
	 */
	SymbolResult getSymbol(Instruction instruction, int operand, int instructionOperand, long address, int addressSize);
}
