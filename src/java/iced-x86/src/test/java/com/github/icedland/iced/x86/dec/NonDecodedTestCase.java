// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.dec;

import com.github.icedland.iced.x86.Instruction;

public final class NonDecodedTestCase {
	public final int bitness;
	public final String hexBytes;
	public final Instruction instruction;

	NonDecodedTestCase(int bitness, String hexBytes, Instruction instruction) {
		this.bitness = bitness;
		this.hexBytes = hexBytes;
		this.instruction = instruction;
	}
}
