// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt.fast;

import com.github.icedland.iced.x86.internal.fmt.FormatterString;
import com.github.icedland.iced.x86.internal.fmt.RegistersTable;

final class Registers {
	static final FormatterString[] allRegisters = RegistersTable.getRegisters();
}
