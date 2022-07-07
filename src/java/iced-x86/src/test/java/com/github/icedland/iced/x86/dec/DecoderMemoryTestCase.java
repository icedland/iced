// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.dec;

import com.github.icedland.iced.x86.ConstantOffsets;

public final class DecoderMemoryTestCase {
	public int lineNumber;
	public int decoderOptions;
	public int bitness;
	public String hexBytes;
	public String encodedHexBytes;
	public long ip;
	public int code;
	public int register;
	public int segmentPrefix;
	public int segmentRegister;
	public int baseRegister;
	public int indexRegister;
	public int scale;
	public long displacement;
	public int displacementSize;
	public int testOptions;
	public ConstantOffsets constantOffsets = new ConstantOffsets();
}
