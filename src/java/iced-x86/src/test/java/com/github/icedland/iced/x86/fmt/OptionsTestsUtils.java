// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.HashSet;

import org.junit.jupiter.params.provider.Arguments;

final class OptionsTestsUtils {
	static Iterable<Arguments> getFormatData_Common(String formatterDir, String formattedStringsFile) {
		FormatterOptionsTests.Info info = FormatterOptionsTests.commonTests;
		return getFormatData(formatterDir, formattedStringsFile, info.tests, info.ignored);
	}

	static Iterable<Arguments> getFormatData_All(String formatterDir, String formattedStringsFile) {
		FormatterOptionsTests.Info info = FormatterOptionsTests.allTests;
		return getFormatData(formatterDir, formattedStringsFile, info.tests, info.ignored);
	}

	static Iterable<Arguments> getFormatData(String formatterDir, String formattedStringsFile, String optionsFile) {
		String testsFilename = FmtFileUtils.getFormatterFilename(Paths.get(formatterDir, optionsFile).toString());
		HashSet<Integer> ignored = new HashSet<Integer>();
		OptionsTestCase[] tests = OptionsTestsReader.readFile(testsFilename, ignored).toArray(new OptionsTestCase[0]);
		return getFormatData(formatterDir, formattedStringsFile, tests, ignored);
	}

	private static Iterable<Arguments> getFormatData(String formatterDir, String formattedStringsFile, OptionsTestCase[] tests, HashSet<Integer> ignored) {
		String[] formattedStrings = FmtFileUtils.readRawStrings(Paths.get(formatterDir, formattedStringsFile).toString()).toArray(new String[0]);
		formattedStrings = Utils.filter(formattedStrings, ignored);
		if (tests.length != formattedStrings.length)
			throw new UnsupportedOperationException();
		ArrayList<Arguments> res = new ArrayList<Arguments>(tests.length);
		for (int i = 0; i < tests.length; i++)
			res.add(Arguments.of(i, tests[i], formattedStrings[i]));
		return res;
	}
}
