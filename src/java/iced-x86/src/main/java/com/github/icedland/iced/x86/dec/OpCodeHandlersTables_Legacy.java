// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.dec;

import com.github.icedland.iced.x86.internal.ResourceReader;

final class OpCodeHandlersTables_Legacy {
	static final OpCodeHandler[] handlers_MAP0;

	static byte[] getSerializedTables() {
		return ResourceReader.readByteArray(OpCodeHandlersData_Legacy.class.getClassLoader(),
				"com/github/icedland/iced/x86/dec/OpCodeHandlersData_Legacy.bin");
	}

	static {
		LegacyOpCodeHandlerReader handlerReader = new LegacyOpCodeHandlerReader();
		TableDeserializer deserializer = new TableDeserializer(handlerReader, OpCodeHandlersData_Legacy.MAX_ID_NAMES, getSerializedTables());
		deserializer.deserialize();
		handlers_MAP0 = deserializer.getTable(OpCodeHandlersData_Legacy.HANDLERS_MAP0_INDEX);
	}
}
