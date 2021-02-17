// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

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
