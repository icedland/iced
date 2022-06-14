// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.internal;

/** DO NOT USE: INTERNAL API */
public final class MnemonicUtilsData {
	/** DO NOT USE: INTERNAL API */
	public static final short[] toMnemonic = ResourceReader.readShortArray(MnemonicUtilsData.class.getClassLoader(),
			"com/github/icedland/iced/x86/MnemonicUtilsData.bin");
}
