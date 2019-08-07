/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

#if (!NO_DECODER32 || !NO_DECODER64) && !NO_DECODER
using System;
using System.Collections.Generic;
using Iced.Intel.Internal;

namespace Iced.Intel.DecoderInternal {
	abstract class OpCodeHandlerReader {
		public abstract int ReadHandlers(ref TableDeserializer deserializer, OpCodeHandler?[] result, int resultIndex);
	}

	struct TableDeserializer {
		DataReader reader;
		readonly OpCodeHandlerReader handlerReader;
		readonly Dictionary<uint, OpCodeHandler> indexToHandler;
		readonly Dictionary<uint, OpCodeHandler?[]> indexToHandlers;
		readonly OpCodeHandler?[] handlerArray;

		public TableDeserializer(OpCodeHandlerReader handlerReader, byte[] data) {
			this.handlerReader = handlerReader;
			reader = new DataReader(data);
			indexToHandler = new Dictionary<uint, OpCodeHandler>();
			indexToHandlers = new Dictionary<uint, OpCodeHandler?[]>();
			handlerArray = new OpCodeHandler[1];
		}

		public void Deserialize() {
			for (uint currentIndex = 0; reader.CanRead; currentIndex++) {
				switch ((SerializedDataKind)reader.ReadByte()) {
				case SerializedDataKind.HandlerReference:
					indexToHandler.Add(currentIndex, ReadHandler());
					break;

				case SerializedDataKind.ArrayReference:
					indexToHandlers.Add(currentIndex, ReadHandlers((int)reader.ReadCompressedUInt32()));
					break;

				default:
					throw new InvalidOperationException();
				}
			}
			if (reader.CanRead)
				throw new InvalidOperationException();
		}

		public OpCodeHandlerKind ReadOpCodeHandlerKind() => (OpCodeHandlerKind)reader.ReadByte();
		public VexOpCodeHandlerKind ReadVexOpCodeHandlerKind() => (VexOpCodeHandlerKind)reader.ReadByte();
		public EvexOpCodeHandlerKind ReadEvexOpCodeHandlerKind() => (EvexOpCodeHandlerKind)reader.ReadByte();
		public Code ReadCode() => (Code)reader.ReadCompressedUInt32();
		public Register ReadRegister() => (Register)reader.ReadByte();
		public DecoderOptions ReadDecoderOptions() => (DecoderOptions)reader.ReadCompressedUInt32();
		public HandlerFlags ReadHandlerFlags() => (HandlerFlags)reader.ReadCompressedUInt32();
		public LegacyHandlerFlags ReadLegacyHandlerFlags() => (LegacyHandlerFlags)reader.ReadCompressedUInt32();
		public TupleType ReadTupleType() => (TupleType)reader.ReadByte();
		public bool ReadBoolean() => reader.ReadByte() != 0;
		public int ReadInt32() => (int)reader.ReadCompressedUInt32();

		public OpCodeHandler ReadHandler() {
			int count = handlerReader.ReadHandlers(ref this, handlerArray, 0);
			if (count != 1)
				throw new InvalidOperationException();
			return handlerArray[0] ?? throw new InvalidOperationException();
		}

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
			if (!indexToHandler.TryGetValue(index, out var handler))
				throw new InvalidOperationException();
			return handler;
		}

		public OpCodeHandler[] ReadArrayReference(uint kind) {
			if (reader.ReadByte() != kind)
				throw new InvalidOperationException();
			return GetTable(reader.ReadByte());
		}

		public OpCodeHandler[] GetTable(uint index) {
			if (!indexToHandlers.TryGetValue(index, out var handlers))
				throw new InvalidOperationException();
			return handlers!;
		}
	}
}
#endif
