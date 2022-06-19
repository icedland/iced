// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.dec;

import com.github.icedland.iced.x86.internal.ResourceReader;

final class OpCodeHandlersTables_EVEX {
	static final OpCodeHandler[] handlers_0F;
	static final OpCodeHandler[] handlers_0F38;
	static final OpCodeHandler[] handlers_0F3A;
	static final OpCodeHandler[] handlers_MAP5;
	static final OpCodeHandler[] handlers_MAP6;

	static byte[] getSerializedTables() {
		return ResourceReader.readByteArray(OpCodeHandlersData_EVEX.class.getClassLoader(),
				"com/github/icedland/iced/x86/dec/OpCodeHandlersData_EVEX.bin");
	}

	static {
		EvexOpCodeHandlerReader handlerReader = new EvexOpCodeHandlerReader();
		TableDeserializer deserializer = new TableDeserializer(handlerReader, OpCodeHandlersData_EVEX.MAX_ID_NAMES, getSerializedTables());
		deserializer.deserialize();
		handlers_0F = deserializer.getTable(OpCodeHandlersData_EVEX.HANDLERS_0F_INDEX);
		handlers_0F38 = deserializer.getTable(OpCodeHandlersData_EVEX.HANDLERS_0F38_INDEX);
		handlers_0F3A = deserializer.getTable(OpCodeHandlersData_EVEX.HANDLERS_0F3A_INDEX);
		handlers_MAP5 = deserializer.getTable(OpCodeHandlersData_EVEX.HANDLERS_MAP5_INDEX);
		handlers_MAP6 = deserializer.getTable(OpCodeHandlersData_EVEX.HANDLERS_MAP6_INDEX);
	}
}
