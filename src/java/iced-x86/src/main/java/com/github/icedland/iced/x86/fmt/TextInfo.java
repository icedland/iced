// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

/**
 * Contains one or more {@link TextPart}s (text and color)
 */
public final class TextInfo {
	/**
	 * {@code true} if this is the default instance
	 */
	public boolean isDefault() {
		return textArray == null && text == null;
	}

	/**
	 * The text and color unless {@link #textArray} is non-null
	 */
	public final TextPart text;

	/**
	 * Text and color or null if {@link #text} should be used
	 */
	public final TextPart[] textArray;

	/**
	 * Constructor
	 */
	public TextInfo() {
		text = null;
		textArray = null;
	}

	/**
	 * Constructor
	 *
	 * @param text  Text
	 * @param color Color
	 */
	public TextInfo(String text, int color) {
		this.text = new TextPart(text, color);
		textArray = null;
	}

	/**
	 * Constructor
	 *
	 * @param text Text
	 */
	public TextInfo(TextPart text) {
		this.text = text;
		textArray = null;
	}

	/**
	 * Constructor
	 *
	 * @param text All text parts
	 */
	public TextInfo(TextPart[] text) {
		this.text = null;
		textArray = text;
	}
}
