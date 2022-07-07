// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.info;

import com.github.icedland.iced.x86.internal.ResourceReader;

final class OpCodeInfoData {
	static final int[] opcFlags1 = ResourceReader.readIntArray(OpCodeInfoData.class.getClassLoader(),
			"com/github/icedland/iced/x86/info/OpCodeInfoData.opcFlags1.bin");
	static final int[] opcFlags2 = ResourceReader.readIntArray(OpCodeInfoData.class.getClassLoader(),
			"com/github/icedland/iced/x86/info/OpCodeInfoData.opcFlags2.bin");
}
