// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.dec;

import com.github.icedland.iced.x86.internal.ResourceReader;

final class OpCodeHandlersTables_VEX {
	static final OpCodeHandler[] handlers_MAP0;
	static final OpCodeHandler[] handlers_0F;
	static final OpCodeHandler[] handlers_0F38;
	static final OpCodeHandler[] handlers_0F3A;

	static byte[] getSerializedTables() {
		return ResourceReader.readByteArray(OpCodeHandlersData_VEX.class.getClassLoader(),
				"com/github/icedland/iced/x86/dec/OpCodeHandlersData_VEX.bin");
	}

	static {
		VexOpCodeHandlerReader handlerReader = new VexOpCodeHandlerReader();
		TableDeserializer deserializer = new TableDeserializer(handlerReader, OpCodeHandlersData_VEX.MAX_ID_NAMES, getSerializedTables());
		deserializer.deserialize();
		handlers_MAP0 = deserializer.getTable(OpCodeHandlersData_VEX.HANDLERS_MAP0_INDEX);
		handlers_0F = deserializer.getTable(OpCodeHandlersData_VEX.HANDLERS_0F_INDEX);
		handlers_0F38 = deserializer.getTable(OpCodeHandlersData_VEX.HANDLERS_0F38_INDEX);
		handlers_0F3A = deserializer.getTable(OpCodeHandlersData_VEX.HANDLERS_0F3A_INDEX);
	}
}
