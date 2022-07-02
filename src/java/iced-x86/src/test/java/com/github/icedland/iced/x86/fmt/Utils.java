// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

import java.util.ArrayList;
import java.util.HashSet;

import com.github.icedland.iced.x86.fmt.gas.GasFormatter;
import com.github.icedland.iced.x86.fmt.intel.IntelFormatter;
import com.github.icedland.iced.x86.fmt.masm.MasmFormatter;
import com.github.icedland.iced.x86.fmt.nasm.NasmFormatter;

final class Utils {
	public static ArrayList<Formatter> getAllFormatters() {
		ArrayList<Formatter> result = new ArrayList<Formatter>(4);
		result.add(new GasFormatter());
		result.add(new IntelFormatter());
		result.add(new MasmFormatter());
		result.add(new NasmFormatter());
		return result;
	}

	public static String[] filter(String[] strings, HashSet<Integer> removed) {
		if (removed.size() == 0)
			return strings;
		String[] res = new String[strings.length - removed.size()];
		int w = 0;
		for (int i = 0; i < strings.length; i++) {
			if (!removed.contains(i))
				res[w++] = strings[i];
		}
		if (w != res.length)
			throw new UnsupportedOperationException();
		return res;
	}
}
