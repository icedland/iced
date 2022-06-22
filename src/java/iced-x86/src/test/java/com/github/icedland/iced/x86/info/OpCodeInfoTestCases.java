// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.info;

import com.github.icedland.iced.x86.PathUtils;

final class OpCodeInfoTestCases {
	static final OpCodeInfoTestCase[] opCodeInfoTests = createOpCodeInfos();

	private static OpCodeInfoTestCase[] createOpCodeInfos() {
		String filename = PathUtils.getTestTextFilename("Encoder", "OpCodeInfos.txt");
		return OpCodeInfoTestCasesReader.readFile(filename);
	}
}
