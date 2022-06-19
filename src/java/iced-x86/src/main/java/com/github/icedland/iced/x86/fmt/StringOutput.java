// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

/**
 * Formatter output that stores the formatted text in a {@link StringBuilder}
 */
public final class StringOutput extends FormatterOutput {
	private final StringBuilder sb;

	/**
	 * Constructor
	 */
	public StringOutput() {
		sb = new StringBuilder();
	}

	/**
	 * Constructor
	 *
	 * @param sb String builder
	 */
	public StringOutput(StringBuilder sb) {
		if (sb == null)
			throw new NullPointerException("sb");
		this.sb = sb;
	}

	/**
	 * Writes text and text kind
	 *
	 * @param text Text, can be an empty string
	 * @param kind Text kind (a {@link FormatterTextKind} enum variant)
	 */
	@Override
	public void write(String text, int kind) {
		sb.append(text);
	}

	/**
	 * Clears the {@link StringBuilder} instance so this class can be reused to format the next instruction
	 */
	public void reset() {
		sb.setLength(0);
	}

	/**
	 * Returns the current formatted text and clears the {@link StringBuilder} instance so this class can be reused to format the next instruction
	 */
	public String toStringAndReset() {
		String result = toString();
		reset();
		return result;
	}

	/**
	 * Gets the current output
	 */
	@Override
	public String toString() {
		return sb.toString();
	}
}
