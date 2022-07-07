// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.instr;

public final class VARegisterValue {
	public int register;
	public int elementIndex;
	public int elementSize;
	public long value;

	public VARegisterValue(int register, int elementIndex, int elementSize, long value) {
		this.register = register;
		this.elementIndex = elementIndex;
		this.elementSize = elementSize;
		this.value = value;
	}
}
