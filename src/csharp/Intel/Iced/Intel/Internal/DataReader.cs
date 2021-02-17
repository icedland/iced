// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if INSTR_INFO || DECODER || GAS || INTEL || MASM || NASM || FAST_FMT
using System;

namespace Iced.Intel.Internal {
#if HAS_SPAN
	ref struct DataReader {
		readonly ReadOnlySpan<byte> data;
#else
	struct DataReader {
		readonly byte[] data;
#endif
		readonly char[] stringData;
		int index;

		public int Index {
			readonly get => index;
			set => index = value;
		}

		public readonly bool CanRead => (uint)index < (uint)data.Length;

#if HAS_SPAN
		public DataReader(ReadOnlySpan<byte> data)
#else
		public DataReader(byte[] data)
#endif
			: this(data, 0) {
		}

#if HAS_SPAN
		public DataReader(ReadOnlySpan<byte> data, int maxStringLength) {
#else
		public DataReader(byte[] data, int maxStringLength) {
#endif
			this.data = data;
			stringData = maxStringLength == 0 ? Array2.Empty<char>() : new char[maxStringLength];
			index = 0;
		}

		public byte ReadByte() => data[index++];

		public uint ReadCompressedUInt32() {
			uint result = 0;
			for (int shift = 0; shift < 32; shift += 7) {
				uint b = ReadByte();
				if ((b & 0x80) == 0)
					return result | (b << shift);
				result |= (b & 0x7F) << shift;
			}
			throw new InvalidOperationException();
		}

		public string ReadAsciiString() {
			int length = ReadByte();
			var stringData = this.stringData;
			for (int i = 0; i < length; i++)
				stringData[i] = (char)ReadByte();
			return new string(stringData, 0, length);
		}
	}
}
#endif
