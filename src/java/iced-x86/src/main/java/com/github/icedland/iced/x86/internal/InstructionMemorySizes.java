// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.internal;

/** DO NOT USE: INTERNAL API */
public final class InstructionMemorySizes {
	/** DO NOT USE: INTERNAL API */
	public static final byte[] sizesNormal = ResourceReader.readByteArray(InstructionMemorySizes.class.getClassLoader(),
			"com/github/icedland/iced/x86/InstructionMemorySizes.sizesNormal.bin");

	/** DO NOT USE: INTERNAL API */
	public static final byte[] sizesBcst = ResourceReader.readByteArray(InstructionMemorySizes.class.getClassLoader(),
			"com/github/icedland/iced/x86/InstructionMemorySizes.sizesBcst.bin");
}
