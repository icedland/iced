// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt.masm;

import com.github.icedland.iced.x86.Register;
import com.github.icedland.iced.x86.internal.fmt.FormatterString;
import com.github.icedland.iced.x86.internal.fmt.RegistersTable;

final class Registers {
	@SuppressWarnings("deprecation")
	public static final int REGISTER_ST = Register.DONTUSE0;
	public static final FormatterString[] allRegisters = RegistersTable.getRegisters();
}
