// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.instr;

public final class VirtualAddressTestCase {
	public final int bitness;
	public final String hexBytes;
	public final int decoderOptions;
	public final int operand;
	public final int usedMemIndex;
	public final int elementIndex;
	public final long expectedValue;
	public final VARegisterValue[] registerValues;

	public VirtualAddressTestCase(int bitness, String hexBytes, int decoderOptions, int operand, int usedMemIndex, int elementIndex,
			long expectedValue, VARegisterValue[] registerValues) {
		this.bitness = bitness;
		this.hexBytes = hexBytes;
		this.decoderOptions = decoderOptions;
		this.operand = operand;
		this.usedMemIndex = usedMemIndex;
		this.elementIndex = elementIndex;
		this.expectedValue = expectedValue;
		this.registerValues = registerValues;
	}
}
