// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt.gas;

import com.github.icedland.iced.x86.Register;
import com.github.icedland.iced.x86.internal.fmt.FormatterString;
import com.github.icedland.iced.x86.internal.fmt.RegistersTable;

final class Registers {
	@SuppressWarnings("deprecation")
	static final int REGISTER_ST = Register.DONTUSE0;
	static final FormatterString[] allRegistersNaked;
	static final FormatterString[] allRegisters;

	static {
		FormatterString[] registers = RegistersTable.getRegisters();
		allRegistersNaked = registers;

		FormatterString[] allRegs = new FormatterString[registers.length];
		for (int i = 0; i < registers.length; i++)
			allRegs[i] = new FormatterString("%" + registers[i].get(false));
		allRegisters = allRegs;
	}
}
