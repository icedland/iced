// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.instr;

import com.github.icedland.iced.x86.PathUtils;

public final class VirtualAddressTestCases {
	public static final VirtualAddressTestCase[] tests = createTests();

	private static VirtualAddressTestCase[] createTests() {
		String filename = PathUtils.getTestTextFilename("Instruction", "VirtualAddressTests.txt");
		return VATestCaseReader.readFile(filename);
	}
}
