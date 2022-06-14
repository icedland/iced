// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.internal.enc;

import com.github.icedland.iced.x86.internal.ResourceReader;

/**
 * DO NOT USE: INTERNAL API
 */
public final class EncoderData {
	/**
	 * DO NOT USE: INTERNAL API
	 */
	public static final int[] encFlags1 = ResourceReader.readIntArray(EncoderData.class.getClassLoader(),
			"com/github/icedland/iced/x86/enc/EncoderData.encFlags1.bin");
	/**
	 * DO NOT USE: INTERNAL API
	 */
	public static final int[] encFlags2 = ResourceReader.readIntArray(EncoderData.class.getClassLoader(),
			"com/github/icedland/iced/x86/enc/EncoderData.encFlags2.bin");
	/**
	 * DO NOT USE: INTERNAL API
	 */
	public static final int[] encFlags3 = ResourceReader.readIntArray(EncoderData.class.getClassLoader(),
			"com/github/icedland/iced/x86/enc/EncoderData.encFlags3.bin");
}
