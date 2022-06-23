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

	@Override
	public int hashCode() {
		final int prime = 31;
		int result = 1;
		result = prime * result + kind;
		result = prime * result + Long.hashCode(address);
		return result;
	}

	@Override
	public boolean equals(Object obj) {
		if (obj == null)
			return false;
		if (getClass() != obj.getClass())
			return false;
		RelocInfo other = (RelocInfo)obj;
		return 
			kind == other.kind &&
			address == other.address;
	}
}
