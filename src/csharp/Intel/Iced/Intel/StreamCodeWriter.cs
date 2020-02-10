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

#if ENCODER
using System.IO;

namespace Iced.Intel {
	/// <summary>
	/// Code writer to a <see cref="System.IO.Stream"/>. 
	/// </summary>
	public sealed class StreamCodeWriter : CodeWriter {
		/// <summary>
		/// Creates a new instance of <see cref="StreamCodeWriter"/>. 
		/// </summary>
		/// <param name="stream">The output stream</param>
		public StreamCodeWriter(Stream stream) => Stream = stream;

		/// <summary>
		/// The stream this instance is writing to
		/// </summary>
		public readonly Stream Stream;

		/// <summary>
		/// Writes the next byte
		/// </summary>
		/// <param name="value">Value</param>
		public override void WriteByte(byte value) => Stream.WriteByte(value);
	}
}
#endif
