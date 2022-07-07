// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

/**
 * Contains text and colors
 */
public final class TextPart {
	/**
	 * Text
	 */
	public final String text;

	/**
	 * Color (a {@link FormatterTextKind} enum variant)
	 */
	public final int color;

	/**
	 * Constructor
	 *
	 * @param text  Text
	 * @param color Color (a {@link FormatterTextKind} enum variant)
	 */
	public TextPart(String text, int color) {
		this.text = text;
		this.color = color;
	}
}
