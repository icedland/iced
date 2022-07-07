// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

import java.util.HashSet;

import com.github.icedland.iced.x86.PathUtils;

final class SymbolResolverTestCases {
	public static class Info {
		public SymbolResolverTestCase[] tests;
		public HashSet<Integer> ignored;

		Info(SymbolResolverTestCase[] tests, HashSet<Integer> ignored) {
			this.tests = tests;
			this.ignored = ignored;
		}
	}

	public static final Info allTests = getTests();

	private static Info getTests() {
		String filename = PathUtils.getTestTextFilename("Formatter", "SymbolResolverTests.txt");
		HashSet<Integer> ignored = new HashSet<Integer>();
		return new Info(SymbolResolverTestsReader.readFile(filename, ignored).toArray(new SymbolResolverTestCase[0]), ignored);
	}
}
