// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

import java.util.HashSet;

import com.github.icedland.iced.x86.PathUtils;

final class FormatterOptionsTests {
	public static class Info {
		public OptionsTestCase[] tests;
		public HashSet<Integer> ignored;
	}

	public static final Info commonTests;
	public static final Info allTests;

	static {
		commonTests = readAllTests("Formatter", "Options.Common.txt");
		allTests = readAllTests("Formatter", "Options.txt");
	}

	private static Info readAllTests(String dir, String filename) {
		String optionsFilename = PathUtils.getTestTextFilename(dir, filename);
		HashSet<Integer> ignored = new HashSet<Integer>();
		Info info = new Info();
		info.tests = OptionsTestsReader.readFile(optionsFilename, ignored).toArray(new OptionsTestCase[0]);
		info.ignored = ignored;
		return info;
	}
}
