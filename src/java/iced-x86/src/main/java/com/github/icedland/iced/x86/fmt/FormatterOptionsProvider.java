// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

import com.github.icedland.iced.x86.Instruction;

/**
 * Can override options used by a {@link Formatter}
 */
@FunctionalInterface
public interface FormatterOptionsProvider {
	/**
	 * Called by the formatter. The method can override any options before the formatter uses them.
	 *
	 * @param instruction        Instruction
	 * @param operand            Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand.
	 * @param instructionOperand Instruction operand number, 0-based, or -1 if it's an operand created by the formatter.
	 * @param options            Options. Only those options that will be used by the formatter are initialized.
	 * @param numberOptions      Number formatting options
	 */
	void getOperandOptions(Instruction instruction, int operand, int instructionOperand, FormatterOperandOptions options,
			NumberFormattingOptions numberOptions);
}
