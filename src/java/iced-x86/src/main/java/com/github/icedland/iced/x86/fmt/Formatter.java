// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

import com.github.icedland.iced.x86.Instruction;

/**
 * Formats instructions
 */
public abstract class Formatter {
	/**
	 * Gets the formatter options
	 */
	public abstract FormatterOptions getOptions();

	/**
	 * Formats the mnemonic and any prefixes
	 *
	 * @param instruction Instruction
	 * @param output      Output
	 */
	public void formatMnemonic(Instruction instruction, FormatterOutput output) {
		formatMnemonic(instruction, output, FormatMnemonicOptions.NONE);
	}

	/**
	 * Formats the mnemonic and/or any prefixes
	 *
	 * @param instruction Instruction
	 * @param output      Output
	 * @param options     Options (a {@link FormatMnemonicOptions} flags value)
	 */
	public abstract void formatMnemonic(Instruction instruction, FormatterOutput output, int options);

	/**
	 * Gets the number of operands that will be formatted. A formatter can add and remove operands
	 *
	 * @param instruction Instruction
	 */
	public abstract int getOperandCount(Instruction instruction);

	/**
	 * Returns the operand access (an {@link com.github.icedland.iced.x86.info.OpAccess} enum variant) but only if it's an operand added by the
	 * formatter. If it's an operand that is part of {@link com.github.icedland.iced.x86.Instruction}, you should call eg.
	 * {@link com.github.icedland.iced.x86.info.InstructionInfoFactory#getInfo(Instruction)}.
	 *
	 * @param instruction Instruction
	 * @param operand     Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand. See
	 *                    {@link #getOperandCount(Instruction)}
	 * @return The operand access (an {@link com.github.icedland.iced.x86.info.OpAccess} enum variant) or {@code null}
	 */
	public abstract Integer tryGetOpAccess(Instruction instruction, int operand);

	/**
	 * Converts a formatter operand index to an instruction operand index. Returns -1 if it's an operand added by the formatter
	 *
	 * @param instruction Instruction
	 * @param operand     Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand. See
	 *                    {@link #getOperandCount(Instruction)}
	 */
	public abstract int getInstructionOperand(Instruction instruction, int operand);

	/**
	 * Converts an instruction operand index to a formatter operand index. Returns -1 if the instruction operand isn't used by the formatter
	 *
	 * @param instruction        Instruction
	 * @param instructionOperand Instruction operand
	 */
	public abstract int getFormatterOperand(Instruction instruction, int instructionOperand);

	/**
	 * Formats an operand. This is a formatter operand and not necessarily a real instruction operand.
	 * A formatter can add and remove operands.
	 *
	 * @param instruction Instruction
	 * @param output      Output
	 * @param operand     Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand. See
	 *                    {@link #getOperandCount(Instruction)}
	 */
	public abstract void formatOperand(Instruction instruction, FormatterOutput output, int operand);

	/**
	 * Formats an operand separator
	 *
	 * @param instruction Instruction
	 * @param output      Output
	 */
	public abstract void formatOperandSeparator(Instruction instruction, FormatterOutput output);

	/**
	 * Formats all operands
	 *
	 * @param instruction Instruction
	 * @param output      Output
	 */
	public abstract void formatAllOperands(Instruction instruction, FormatterOutput output);

	/**
	 * Formats the whole instruction: prefixes, mnemonic, operands
	 *
	 * @param instruction Instruction
	 * @param output      Output
	 */
	public abstract void format(Instruction instruction, FormatterOutput output);

	/**
	 * Formats a register
	 *
	 * @param register Register (a {@link com.github.icedland.iced.x86.Register} enum variant)
	 */
	public abstract String formatRegister(int register);

	/**
	 * Formats a signed 8-bit integer
	 *
	 * @param value Value
	 */
	public String formatInt8(byte value) {
		return formatInt8(value, NumberFormattingOptions.createImmediate(getOptions()));
	}

	/**
	 * Formats a signed 16-bit integer
	 *
	 * @param value Value
	 */
	public String formatInt16(short value) {
		return formatInt16(value, NumberFormattingOptions.createImmediate(getOptions()));
	}

	/**
	 * Formats a signed 32-bit integer
	 *
	 * @param value Value
	 */
	public String formatInt32(int value) {
		return formatInt32(value, NumberFormattingOptions.createImmediate(getOptions()));
	}

	/**
	 * Formats a signed 64-bit integer
	 *
	 * @param value Value
	 */
	public String formatInt64(long value) {
		return formatInt64(value, NumberFormattingOptions.createImmediate(getOptions()));
	}

	/**
	 * Formats an unsigned 8-bit integer
	 *
	 * @param value Value
	 */
	public String formatUInt8(byte value) {
		return formatUInt8(value, NumberFormattingOptions.createImmediate(getOptions()));
	}

	/**
	 * Formats an unsigned 16-bit integer
	 *
	 * @param value Value
	 */
	public String formatUInt16(short value) {
		return formatUInt16(value, NumberFormattingOptions.createImmediate(getOptions()));
	}

	/**
	 * Formats an unsigned 32-bit integer
	 *
	 * @param value Value
	 */
	public String formatUInt32(int value) {
		return formatUInt32(value, NumberFormattingOptions.createImmediate(getOptions()));
	}

	/**
	 * Formats an unsigned 64-bit integer
	 *
	 * @param value Value
	 */
	public String formatUInt64(long value) {
		return formatUInt64(value, NumberFormattingOptions.createImmediate(getOptions()));
	}

	/**
	 * Formats a signed 8-bit integer
	 *
	 * @param value         Value
	 * @param numberOptions Options
	 */
	public abstract String formatInt8(byte value, NumberFormattingOptions numberOptions);

	/**
	 * Formats a signed 16-bit integer
	 *
	 * @param value         Value
	 * @param numberOptions Options
	 */
	public abstract String formatInt16(short value, NumberFormattingOptions numberOptions);

	/**
	 * Formats a signed 32-bit integer
	 *
	 * @param value         Value
	 * @param numberOptions Options
	 */
	public abstract String formatInt32(int value, NumberFormattingOptions numberOptions);

	/**
	 * Formats a signed 64-bit integer
	 *
	 * @param value         Value
	 * @param numberOptions Options
	 */
	public abstract String formatInt64(long value, NumberFormattingOptions numberOptions);

	/**
	 * Formats an unsigned 8-bit integer
	 *
	 * @param value         Value
	 * @param numberOptions Options
	 */
	public abstract String formatUInt8(byte value, NumberFormattingOptions numberOptions);

	/**
	 * Formats an unsigned 16-bit integer
	 *
	 * @param value         Value
	 * @param numberOptions Options
	 */
	public abstract String formatUInt16(short value, NumberFormattingOptions numberOptions);

	/**
	 * Formats an unsigned 32-bit integer
	 *
	 * @param value         Value
	 * @param numberOptions Options
	 */
	public abstract String formatUInt32(int value, NumberFormattingOptions numberOptions);

	/**
	 * Formats an unsigned 64-bit integer
	 *
	 * @param value         Value
	 * @param numberOptions Options
	 */
	public abstract String formatUInt64(long value, NumberFormattingOptions numberOptions);
}
