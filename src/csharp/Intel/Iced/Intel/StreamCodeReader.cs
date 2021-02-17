// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if DECODER
using System.IO;

namespace Iced.Intel {
	/// <summary>
	/// Code reader from a <see cref="System.IO.Stream"/>. 
	/// </summary>
	public sealed class StreamCodeReader : CodeReader {
		/// <summary>
		/// Creates a new instance of <see cref="StreamCodeReader"/>. 
		/// </summary>
		/// <param name="stream">The input stream</param>
		public StreamCodeReader(Stream stream) => Stream = stream;

		/// <summary>
		/// The stream this instance is writing to
		/// </summary>
		public readonly Stream Stream;

		/// <summary>
		/// Reads the next byte or returns less than 0 if there are no more bytes
		/// </summary>
		/// <returns></returns>
		public override int ReadByte() => Stream.ReadByte();
	}
}
#endif
