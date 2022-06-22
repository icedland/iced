// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86;

import java.util.ArrayList;

public final class BitnessUtils {
	public static Iterable<Integer> getInvalidBitnessValues() {
		ArrayList<Integer> result = new ArrayList<Integer>();
		result.add(-0x8000_0000);
		result.add(0x7FFF_FFFF);
		for (int bitness = -1; bitness <= 128; bitness++) {
			if (bitness == 16 || bitness == 32 || bitness == 64)
				continue;
			result.add(bitness);
		}
		return result;
	}
}
