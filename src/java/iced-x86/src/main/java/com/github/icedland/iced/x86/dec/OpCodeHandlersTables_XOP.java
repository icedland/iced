// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.dec;

import com.github.icedland.iced.x86.internal.ResourceReader;

final class OpCodeHandlersTables_XOP {
	static final OpCodeHandler[] handlers_MAP8;
	static final OpCodeHandler[] handlers_MAP9;
	static final OpCodeHandler[] handlers_MAP10;

	static byte[] getSerializedTables() {
		return ResourceReader.readByteArray(OpCodeHandlersData_XOP.class.getClassLoader(),
				"com/github/icedland/iced/x86/dec/OpCodeHandlersData_XOP.bin");
	}

	static {
		VexOpCodeHandlerReader handlerReader = new VexOpCodeHandlerReader();
		TableDeserializer deserializer = new TableDeserializer(handlerReader, OpCodeHandlersData_XOP.MAX_ID_NAMES, getSerializedTables());
		deserializer.deserialize();
		handlers_MAP8 = deserializer.getTable(OpCodeHandlersData_XOP.HANDLERS_MAP8_INDEX);
		handlers_MAP9 = deserializer.getTable(OpCodeHandlersData_XOP.HANDLERS_MAP9_INDEX);
		handlers_MAP10 = deserializer.getTable(OpCodeHandlersData_XOP.HANDLERS_MAP10_INDEX);
	}
}
