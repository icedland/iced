// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.internal;

/** DO NOT USE: INTERNAL API */
public final class InstructionOpCounts {
	/** DO NOT USE: INTERNAL API */
	public static final byte[] opCount = ResourceReader.readByteArray(InstructionOpCounts.class.getClassLoader(),
			"com/github/icedland/iced/x86/InstructionOpCounts.bin");
}
