// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

// ⚠️This file was generated by GENERATOR!🦹‍♂️

package com.github.icedland.iced.x86.fmt;

/**
 * Mnemonic condition code selector (eg.<!-- --> <code>JNE</code> / <code>JNZ</code>)
 */
public final class CC_ne {
	private CC_ne() {
	}

	/**
	 * <code>JNE</code>, <code>CMOVNE</code>, <code>SETNE</code>, <code>LOOPNE</code>, <code>REPNE</code>
	 */
	public static final int NE = 0;
	/**
	 * <code>JNZ</code>, <code>CMOVNZ</code>, <code>SETNZ</code>, <code>LOOPNZ</code>, <code>REPNZ</code>
	 */
	public static final int NZ = 1;
}