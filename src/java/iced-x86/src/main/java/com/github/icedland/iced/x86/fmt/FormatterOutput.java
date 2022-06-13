// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

import com.github.icedland.iced.x86.Instruction;

/**
 * Used by a {@link Formatter} to write all text
 */
public abstract class FormatterOutput {
	/**
	 * Writes text and text kind
	 *
	 * @param text Text
	 * @param kind Text kind (a {@link FormatterTextKind} enum variant)
	 */
	public abstract void write(String text, int kind);

	/**
	 * Writes a prefix
	 *
	 * @param instruction Instruction
	 * @param text        Prefix text
	 * @param prefix      Prefix (a {@link PrefixKind} enum variant)
	 */
	public void writePrefix(Instruction instruction, String text, int prefix) {
		write(text, FormatterTextKind.PREFIX);
	}

	/**
	 * Writes a mnemonic (see {@link com.github.icedland.iced.x86.Instruction#getMnemonic()})
	 *
	 * @param instruction Instruction
	 * @param text        Mnemonic text
	 */
	public void writeMnemonic(Instruction instruction, String text) {
		write(text, FormatterTextKind.MNEMONIC);
	}

	/**
	 * Writes a number
	 *
	 * @param instruction        Instruction
	 * @param operand            Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand.
	 * @param instructionOperand Instruction operand number, 0-based, or -1 if it's an operand created by the formatter.
	 * @param text               Number text
	 * @param value              Value
	 * @param numberKind         Number kind (a {@link NumberKind} enum variant)
	 * @param kind               Text kind (a {@link FormatterTextKind} enum variant)
	 */
	public void writeNumber(Instruction instruction, int operand, int instructionOperand, String text, long value, int numberKind, int kind) {
		write(text, kind);
	}

	/**
	 * Writes a decorator
	 *
	 * @param instruction        Instruction
	 * @param operand            Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand.
	 * @param instructionOperand Instruction operand number, 0-based, or -1 if it's an operand created by the formatter.
	 * @param text               Decorator text
	 * @param decorator          Decorator (a {@link DecoratorKind} enum variant)
	 */
	public void writeDecorator(Instruction instruction, int operand, int instructionOperand, String text, int decorator) {
		write(text, FormatterTextKind.DECORATOR);
	}

	/**
	 * Writes a register
	 *
	 * @param instruction        Instruction
	 * @param operand            Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand.
	 * @param instructionOperand Instruction operand number, 0-based, or -1 if it's an operand created by the formatter.
	 * @param text               Register text
	 * @param register           Register (a {@link com.github.icedland.iced.x86.Register} enum variant)
	 */
	public void writeRegister(Instruction instruction, int operand, int instructionOperand, String text, int register) {
		write(text, FormatterTextKind.REGISTER);
	}

	/**
	 * Writes a symbol
	 *
	 * @param instruction        Instruction
	 * @param operand            Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand.
	 * @param instructionOperand Instruction operand number, 0-based, or -1 if it's an operand created by the formatter.
	 * @param address            Address
	 * @param symbol             Symbol
	 */
	public void writeSymbol(Instruction instruction, int operand, int instructionOperand, long address, SymbolResult symbol) {
		TextInfo text = symbol.text;
		TextPart[] array = text.textArray;
		if (array != null) {
			for (TextPart part : array)
				write(part.text, part.color);
		}
		else if (text.text != null)
			write(text.text.text, text.text.color);
	}
}
