// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.dec;

import java.util.ArrayList;

import com.github.icedland.iced.x86.internal.dec.SerializedDataKind;

abstract class OpCodeHandlerReader {
	abstract int readHandlers(TableDeserializer deserializer, OpCodeHandler[] result, int resultIndex);
}

final class HandlerInfo {
	final OpCodeHandler handler;
	final OpCodeHandler[] handlers;

	HandlerInfo(OpCodeHandler handler) {
		this.handler = handler;
		handlers = null;
	}

	HandlerInfo(OpCodeHandler[] handlers) {
		handler = null;
		this.handlers = handlers;
	}
}

@SuppressWarnings("deprecation")
final class TableDeserializer {
	private final com.github.icedland.iced.x86.internal.DataReader reader;
	private final OpCodeHandlerReader handlerReader;
	private final ArrayList<HandlerInfo> idToHandler;
	private final OpCodeHandler[] handlerArray;

	TableDeserializer(OpCodeHandlerReader handlerReader, int maxIds, byte[] data) {
		this.handlerReader = handlerReader;
		reader = new com.github.icedland.iced.x86.internal.DataReader(data);
		idToHandler = new ArrayList<HandlerInfo>(maxIds);
		handlerArray = new OpCodeHandler[1];
	}

	void deserialize() {
		for (; reader.canRead();) {
			switch (reader.readByte()) {
			case SerializedDataKind.HANDLER_REFERENCE:
				idToHandler.add(new HandlerInfo(readHandler()));
				break;

			case SerializedDataKind.ARRAY_REFERENCE:
				idToHandler.add(new HandlerInfo(readHandlers(reader.readCompressedUInt32())));
				break;

			default:
				throw new UnsupportedOperationException();
			}
		}
		if (reader.canRead())
			throw new UnsupportedOperationException();
	}

	int readLegacyOpCodeHandlerKind() {
		return reader.readByte();
	}

	int readVexOpCodeHandlerKind() {
		return reader.readByte();
	}

	int readEvexOpCodeHandlerKind() {
		return reader.readByte();
	}

	int readMvexOpCodeHandlerKind() {
		return reader.readByte();
	}

	int readCode() {
		return reader.readCompressedUInt32();
	}

	int readRegister() {
		return reader.readByte();
	}

	int readDecoderOptions() {
		return reader.readCompressedUInt32();
	}

	int readHandlerFlags() {
		return reader.readCompressedUInt32();
	}

	int readLegacyHandlerFlags() {
		return reader.readCompressedUInt32();
	}

	int readTupleType() {
		return reader.readByte();
	}

	boolean readBoolean() {
		return reader.readByte() != 0;
	}

	int readInt32() {
		return reader.readCompressedUInt32();
	}

	OpCodeHandler readHandler() {
		OpCodeHandler handler = readHandlerOrNull();
		if (handler == null)
			throw new UnsupportedOperationException();
		return handler;
	}

	OpCodeHandler readHandlerOrNull() {
		int count = handlerReader.readHandlers(this, handlerArray, 0);
		if (count != 1)
			throw new UnsupportedOperationException();
		return handlerArray[0];
	}

	OpCodeHandler[] readHandlers(int count) {
		OpCodeHandler[] handlers = new OpCodeHandler[count];
		for (int i = 0; i < handlers.length;) {
			int num = handlerReader.readHandlers(this, handlers, i);
			if (num <= 0 || Integer.compareUnsigned(i + num, handlers.length) > 0)
				throw new UnsupportedOperationException();
			i += num;
		}
		return handlers;
	}

	OpCodeHandler readHandlerReference() {
		int index = reader.readByte();
		OpCodeHandler handler = idToHandler.get(index).handler;
		if (handler == null)
			throw new UnsupportedOperationException();
		return handler;
	}

	OpCodeHandler[] readArrayReference(int kind) {
		if (reader.readByte() != kind)
			throw new UnsupportedOperationException();
		return getTable(reader.readByte());
	}

	OpCodeHandler[] getTable(int index) {
		OpCodeHandler[] handlers = idToHandler.get(index).handlers;
		if (handlers == null)
			throw new UnsupportedOperationException();
		return handlers;
	}
}
