// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.instr;

import com.github.icedland.iced.x86.VAGetRegisterValue;

public final class VAGetRegisterValueImpl implements VAGetRegisterValue {
	private final VARegisterValue[] results;

	public VAGetRegisterValueImpl(VARegisterValue[] results) {
		this.results = results;
	}

	public Long get(int register, int elementIndex, int elementSize) {
		VARegisterValue[] results = this.results;
		for (int i = 0; i < results.length; i++) {
			VARegisterValue info = results[i];
			if (info.register == register && info.elementIndex == elementIndex && info.elementSize == elementSize)
				return info.value;
		}
		return null;
	}
}
