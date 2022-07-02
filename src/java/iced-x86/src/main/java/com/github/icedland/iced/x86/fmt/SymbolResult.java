// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

import com.github.icedland.iced.x86.MemorySize;

/**
 * Created by a {@link SymbolResolver}
 */
public final class SymbolResult {
	private static final int DEFAULT_KIND = FormatterTextKind.LABEL;

	/**
	 * The address of the symbol
	 */
	public final long address;

	/**
	 * Contains the symbol
	 */
	public final TextInfo text;

	/**
	 * Symbol flags (a {@link SymbolFlags} flags value)
	 */
	public final int flags;

	/**
	 * Checks whether {@link #symbolSize} is valid
	 */
	public boolean hasSymbolSize() {
		return (flags & SymbolFlags.HAS_SYMBOL_SIZE) != 0;
	}

	/**
	 * Symbol size (a {@link com.github.icedland.iced.x86.MemorySize} enum variant) if {@link SymbolFlags#HAS_SYMBOL_SIZE} is set.
	 */
	public final int symbolSize;

	/**
	 * Constructor
	 *
	 * @param address The address of the symbol
	 * @param text    Symbol
	 */
	public SymbolResult(long address, String text) {
		this.address = address;
		this.text = new TextInfo(text, DEFAULT_KIND);
		this.flags = SymbolFlags.NONE;
		this.symbolSize = MemorySize.UNKNOWN;
	}

	/**
	 * Constructor
	 *
	 * @param address The address of the symbol
	 * @param text    Symbol
	 * @param size    Symbol size (a {@link com.github.icedland.iced.x86.MemorySize} enum variant)
	 */
	public SymbolResult(long address, String text, int size) {
		this.address = address;
		this.text = new TextInfo(text, DEFAULT_KIND);
		this.flags = SymbolFlags.HAS_SYMBOL_SIZE;
		this.symbolSize = size;
	}

	/**
	 * Constructor
	 *
	 * @param address The address of the symbol
	 * @param text    Symbol
	 * @param color   Color
	 * @param flags   Symbol flags (a {@link SymbolFlags} flags value)
	 */
	public SymbolResult(long address, String text, int color, int flags) {
		this.address = address;
		this.text = new TextInfo(text, color);
		this.flags = flags & ~SymbolFlags.HAS_SYMBOL_SIZE;
		this.symbolSize = MemorySize.UNKNOWN;
	}

	/**
	 * Constructor
	 *
	 * @param address The address of the symbol
	 * @param text    Symbol
	 */
	public SymbolResult(long address, TextInfo text) {
		this.address = address;
		this.text = text;
		this.flags = SymbolFlags.NONE;
		this.symbolSize = MemorySize.UNKNOWN;
	}

	/**
	 * Constructor
	 *
	 * @param address The address of the symbol
	 * @param text    Symbol
	 * @param size    Symbol size (a {@link com.github.icedland.iced.x86.MemorySize} enum variant)
	 */
	public SymbolResult(long address, TextInfo text, int size) {
		this.address = address;
		this.text = text;
		this.flags = SymbolFlags.HAS_SYMBOL_SIZE;
		this.symbolSize = size;
	}

	/**
	 * Constructor
	 *
	 * @param address The address of the symbol
	 * @param text    Symbol
	 * @param flags   Symbol flags (a {@link SymbolFlags} flags value)
	 * @param ignored Ignored
	 */
	public SymbolResult(long address, TextInfo text, int flags, boolean ignored) {
		this.address = address;
		this.text = text;
		this.flags = flags & ~SymbolFlags.HAS_SYMBOL_SIZE;
		this.symbolSize = MemorySize.UNKNOWN;
	}

	/**
	 * Constructor
	 *
	 * @param address The address of the symbol
	 * @param text    Symbol
	 * @param flags   Symbol flags (a {@link SymbolFlags} flags value)
	 * @param size    Symbol size (a {@link com.github.icedland.iced.x86.MemorySize} enum variant)
	 */
	public SymbolResult(long address, TextInfo text, int flags, int size) {
		this.address = address;
		this.text = text;
		this.flags = flags | SymbolFlags.HAS_SYMBOL_SIZE;
		this.symbolSize = size;
	}
}
