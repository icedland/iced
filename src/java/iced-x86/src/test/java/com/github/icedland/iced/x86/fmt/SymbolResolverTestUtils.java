// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

import java.nio.file.Paths;
import java.util.ArrayList;

import org.junit.jupiter.params.provider.Arguments;

final class SymbolResolverTestUtils {
	public static ArrayList<Arguments> getFormatData(String formatterDir, String formattedStringsFile) {
		SymbolResolverTestCases.Info info = SymbolResolverTestCases.allTests;
		String[] formattedStrings = FmtFileUtils.readRawStrings(Paths.get(formatterDir, formattedStringsFile).toString()).toArray(new String[0]);
		formattedStrings = Utils.filter(formattedStrings, info.ignored);
		if (info.tests.length != formattedStrings.length)
			throw new UnsupportedOperationException();
		ArrayList<Arguments> result = new ArrayList<Arguments>(info.tests.length);
		for (int i = 0; i < info.tests.length; i++)
			result.add(Arguments.of(i, info.tests[i], formattedStrings[i]));
		return result;
	}
}
