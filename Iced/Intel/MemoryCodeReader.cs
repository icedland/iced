/*
    Copyright (C) 2018 de4dot@gmail.com

    This file is part of Iced.

    Iced is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Iced is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Iced.  If not, see <https://www.gnu.org/licenses/>.
*/

#if NETCOREAPP || NETSTANDARD2_1
#if (!NO_DECODER32 || !NO_DECODER64) && !NO_DECODER
using System;

namespace Iced.Intel {
	/// <summary>
	/// A <see cref="CodeReader"/> that reads data from a <see cref="ReadOnlyMemory{T}"/> or a <see cref="Memory{T}"/>
	/// </summary>
	public sealed class MemoryCodeReader : CodeReader {
		readonly ReadOnlyMemory<byte> data;
		int currentPosition;

		/// <summary>
		/// Current position
		/// </summary>
		public int Position {
			get => currentPosition;
			set {
				if ((uint)value > (uint)Count)
					throw new ArgumentOutOfRangeException(nameof(value));
				currentPosition = value;
			}
		}

		/// <summary>
		/// Number of bytes that can be read
		/// </summary>
		public int Count => data.Length;

		/// <summary>
		/// Checks if it's possible to read another byte
		/// </summary>
		public bool CanReadByte => currentPosition < data.Length;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="data">Data</param>
		public MemoryCodeReader(ReadOnlyMemory<byte> data) {
			this.data = data;
			currentPosition = 0;
		}

		/// <summary>
		/// Reads the next byte or returns less than 0 if there are no more bytes
		/// </summary>
		/// <returns></returns>
		public override int ReadByte() {
			var span = data.Span;
			int pos = currentPosition;
			if ((uint)pos >= (uint)span.Length)
				return -1;
			var result = span[pos];
			currentPosition = pos + 1;
			return result;
		}
	}
}
#endif
#endif
