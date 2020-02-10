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

#if DECODER
using System;

namespace Iced.Intel {
	/// <summary>
	/// A <see cref="CodeReader"/> that reads data from a byte array
	/// </summary>
	public sealed class ByteArrayCodeReader : CodeReader {
		readonly byte[] data;
		int currentPosition;
		readonly int startPosition;
		readonly int endPosition;

		/// <summary>
		/// Current position
		/// </summary>
		public int Position {
			get => currentPosition - startPosition;
			set {
				if ((uint)value > (uint)Count)
					ThrowHelper.ThrowArgumentOutOfRangeException_value();
				currentPosition = startPosition + value;
			}
		}

		/// <summary>
		/// Number of bytes that can be read
		/// </summary>
		public int Count => endPosition - startPosition;

		/// <summary>
		/// Checks if it's possible to read another byte
		/// </summary>
		public bool CanReadByte => currentPosition < endPosition;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="hexData">Hex bytes encoded in a string</param>
		public ByteArrayCodeReader(string hexData)
			: this(HexUtils.ToByteArray(hexData)) {
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="data">Data</param>
		public ByteArrayCodeReader(byte[] data) {
			if (data is null)
				ThrowHelper.ThrowArgumentNullException_data();
			this.data = data;
			currentPosition = 0;
			startPosition = 0;
			endPosition = data.Length;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="data">Data</param>
		/// <param name="index">Start index</param>
		/// <param name="count">Number of bytes</param>
		public ByteArrayCodeReader(byte[] data, int index, int count) {
			if (data is null)
				ThrowHelper.ThrowArgumentNullException_data();
			this.data = data;
			if (index < 0)
				ThrowHelper.ThrowArgumentOutOfRangeException_index();
			if (count < 0)
				ThrowHelper.ThrowArgumentOutOfRangeException_count();
			if ((uint)index + (uint)count > (uint)data.Length)
				ThrowHelper.ThrowArgumentOutOfRangeException_count();
			currentPosition = index;
			startPosition = index;
			endPosition = index + count;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="data">Data</param>
		public ByteArrayCodeReader(ArraySegment<byte> data) {
			if (data.Array is null)
				ThrowHelper.ThrowArgumentException();
			this.data = data.Array;
			int offset = data.Offset;
			currentPosition = offset;
			startPosition = offset;
			endPosition = offset + data.Count;
		}

		/// <summary>
		/// Reads the next byte or returns less than 0 if there are no more bytes
		/// </summary>
		/// <returns></returns>
		public override int ReadByte() {
			if (currentPosition >= endPosition)
				return -1;
			return data[currentPosition++];
		}
	}
}
#endif
