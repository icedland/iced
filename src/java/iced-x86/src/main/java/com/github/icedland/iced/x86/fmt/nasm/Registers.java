// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt.nasm;

import com.github.icedland.iced.x86.Register;
import com.github.icedland.iced.x86.internal.fmt.FormatterString;
import com.github.icedland.iced.x86.internal.fmt.RegistersTable;

final class Registers {
	public static final FormatterString[] allRegisters = getRegisters();

	static FormatterString[] getRegisters() {
		FormatterString[] registers = RegistersTable.getRegisters();
		for (int i = 0; i < 8; i++)
			registers[Register.ST0 + i] = new FormatterString("st" + i);
		return registers;
	}
}
