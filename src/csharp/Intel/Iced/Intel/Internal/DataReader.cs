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
