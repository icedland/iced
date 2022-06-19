// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if DECODER
using System;
using System.Collections.Generic;
using Iced.Intel.Internal;

namespace Iced.Intel.DecoderInternal {
	abstract class OpCodeHandlerReader {
		public abstract int ReadHandlers(ref TableDeserializer deserializer, OpCodeHandler?[] result, int resultIndex);
	}

	readonly struct HandlerInfo {
		public readonly OpCodeHandler? handler;
		public readonly OpCodeHandler?[]? handlers;

		public HandlerInfo(OpCodeHandler handler) {
			this.handler = handler;
			handlers = null;
		}

		public HandlerInfo(OpCodeHandler?[] handlers) {
			handler = null;
			this.handlers = handlers;
		}
	}

#if HAS_SPAN
	ref struct TableDeserializer {
#else
	struct TableDeserializer {
#endif
		DataReader reader;
		readonly OpCodeHandlerReader handlerReader;
		readonly List<HandlerInfo> idToHandler;
		readonly OpCodeHandler?[] handlerArray;

#if HAS_SPAN
		public TableDeserializer(OpCodeHandlerReader handlerReader, int maxIds, ReadOnlySpan<byte> data) {
#else
		public TableDeserializer(OpCodeHandlerReader handlerReader, int maxIds, byte[] data) {
#endif
			this.handlerReader = handlerReader;
			reader = new DataReader(data);
			idToHandler = new List<HandlerInfo>(maxIds);
			handlerArray = new OpCodeHandler[1];
		}

		public void Deserialize() {
			for (; reader.CanRead;) {
				switch ((SerializedDataKind)reader.ReadByte()) {
				case SerializedDataKind.HandlerReference:
					idToHandler.Add(new HandlerInfo(ReadHandler()));
					break;

				case SerializedDataKind.ArrayReference:
					idToHandler.Add(new HandlerInfo(ReadHandlers((int)reader.ReadCompressedUInt32())));
					break;

				default:
					throw new InvalidOperationException();
				}
			}
			if (reader.CanRead)
				throw new InvalidOperationException();
		}

		public LegacyOpCodeHandlerKind ReadLegacyOpCodeHandlerKind() => (LegacyOpCodeHandlerKind)reader.ReadByte();
#if !NO_VEX || !NO_XOP
		public VexOpCodeHandlerKind ReadVexOpCodeHandlerKind() => (VexOpCodeHandlerKind)reader.ReadByte();
#endif
#if !NO_EVEX
		public EvexOpCodeHandlerKind ReadEvexOpCodeHandlerKind() => (EvexOpCodeHandlerKind)reader.ReadByte();
#endif
#if MVEX
		public MvexOpCodeHandlerKind ReadMvexOpCodeHandlerKind() => (MvexOpCodeHandlerKind)reader.ReadByte();
#endif
		public Code ReadCode() => (Code)reader.ReadCompressedUInt32();
		public Register ReadRegister() => (Register)reader.ReadByte();
		public DecoderOptions ReadDecoderOptions() => (DecoderOptions)reader.ReadCompressedUInt32();
		public HandlerFlags ReadHandlerFlags() => (HandlerFlags)reader.ReadCompressedUInt32();
		public LegacyHandlerFlags ReadLegacyHandlerFlags() => (LegacyHandlerFlags)reader.ReadCompressedUInt32();
#if !NO_EVEX
		public TupleType ReadTupleType() => (TupleType)reader.ReadByte();
#endif
		public bool ReadBoolean() => reader.ReadByte() != 0;
		public int ReadInt32() => (int)reader.ReadCompressedUInt32();

		public OpCodeHandler ReadHandler() => ReadHandlerOrNull() ?? throw new InvalidOperationException();

		public OpCodeHandler? ReadHandlerOrNull() {
			int count = handlerReader.ReadHandlers(ref this, handlerArray, 0);
			if (count != 1)
				throw new InvalidOperationException();
			return handlerArray[0];
		}

		public OpCodeHandler?[] ReadHandlers(int count) {
			var handlers = new OpCodeHandler?[count];
			for (int i = 0; i < handlers.Length;) {
				int num = handlerReader.ReadHandlers(ref this, handlers, i);
				if (num <= 0 || (uint)i + (uint)num > (uint)handlers.Length)
					throw new InvalidOperationException();
				i += num;
			}
			return handlers;
		}

		public OpCodeHandler ReadHandlerReference() {
			uint index = reader.ReadByte();
			return idToHandler[(int)index].handler ?? throw new InvalidOperationException();
		}

		public OpCodeHandler[] ReadArrayReference(uint kind) {
			if (reader.ReadByte() != kind)
				throw new InvalidOperationException();
			return GetTable(reader.ReadByte());
		}

		public OpCodeHandler[] GetTable(uint index) =>
			(idToHandler[(int)index].handlers ?? throw new InvalidOperationException())!;
	}
}
#endif
