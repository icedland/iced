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

#if FAST_FMT
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Iced.Intel {
	/// <summary>
	/// <see cref="FastFormatter"/> output
	/// </summary>
	public sealed class FastStringOutput {
		char[] buffer;
		int bufferLen;

		/// <summary>
		/// Gets the current length
		/// </summary>
		public int Length => bufferLen;

		/// <summary>
		/// Constructor
		/// </summary>
		public FastStringOutput() => buffer = new char[64];

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="capacity">Initial capacity</param>
		public FastStringOutput(int capacity) => buffer = new char[capacity];

		/// <summary>
		/// Append a char
		/// </summary>
		/// <param name="c">Character to append</param>
		public void Append(char c) {
			var buffer = this.buffer;
			var bufferLen = this.bufferLen;
			if ((uint)bufferLen >= (uint)buffer.Length) {
				Resize(1);
				buffer = this.buffer;
			}
			buffer[bufferLen] = c;
			this.bufferLen = bufferLen + 1;
		}

		/// <summary>
		/// Append a string
		/// </summary>
		/// <param name="value">String to append</param>
		public void Append(string? value) {
			if (value is object)
				AppendNotNull(value);
		}

		internal void AppendNotNull(string value) {
			var buffer = this.buffer;
			var bufferLen = this.bufferLen;
			if ((uint)bufferLen + value.Length > (uint)buffer.Length) {
				Resize(value.Length);
				buffer = this.buffer;
			}
			for (int i = 0; i < value.Length; i++) {
				buffer[bufferLen] = value[i];
				bufferLen++;
			}
			this.bufferLen = bufferLen;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		void Resize(int extraCount) {
			int capacity = buffer.Length;
			int newCount = checked(Math.Max(capacity * 2, capacity + extraCount));
			Array.Resize(ref buffer, newCount);
		}

#if HAS_SPAN
		/// <summary>
		/// Returns the current string as a span. The return value is valid until this instance gets mutated.
		/// </summary>
		/// <returns></returns>
		public ReadOnlySpan<char> AsSpan() =>
			new ReadOnlySpan<char>(buffer, 0, bufferLen);
#endif

		/// <summary>
		/// Copies all data to <paramref name="array"/>
		/// </summary>
		/// <param name="array">Destination array</param>
		/// <param name="arrayIndex">Destination array index</param>
		public void CopyTo(char[] array, int arrayIndex) =>
			Array.Copy(buffer, 0, array, arrayIndex, bufferLen);

		/// <summary>
		/// Resets the buffer to an empty string
		/// </summary>
		public void Clear() => bufferLen = 0;

		/// <summary>
		/// Gets the current string
		/// </summary>
		/// <returns></returns>
		public override string ToString() => new string(buffer, 0, bufferLen);
	}
}
#endif
