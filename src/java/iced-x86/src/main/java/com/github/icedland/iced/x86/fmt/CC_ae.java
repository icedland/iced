// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

// ⚠️This file was generated by GENERATOR!🦹‍♂️

package com.github.icedland.iced.x86.fmt;

/**
 * Mnemonic condition code selector (eg.<!-- --> <code>JAE</code> / <code>JNB</code> / <code>JNC</code>)
 */
public final class CC_ae {
	private CC_ae() {
	}

	/**
	 * <code>JAE</code>, <code>CMOVAE</code>, <code>SETAE</code>
	 */
	public static final int AE = 0;
	/**
	 * <code>JNB</code>, <code>CMOVNB</code>, <code>SETNB</code>
	 */
	public static final int NB = 1;
	/**
	 * <code>JNC</code>, <code>CMOVNC</code>, <code>SETNC</code>
	 */
	public static final int NC = 2;
}