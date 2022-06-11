// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.enc;

/**
 * Relocation info
 */
public final class RelocInfo {
	/**
	 * Address
	 */
	public final long address;

	/**
	 * Relocation kind (a {@link RelocKind} enum variant)
	 */
	public final int kind;

	/**
	 * Constructor
	 *
	 * @param kind    Relocation kind (a {@link RelocKind} enum variant)
	 * @param address Address
	 */
	public RelocInfo(int kind, long address) {
		this.kind = kind;
		this.address = address;
	}
}
