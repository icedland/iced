// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER
namespace Iced.Intel {
	/// <summary>
	/// Used by an <see cref="Encoder"/> to write encoded instructions
	/// </summary>
	public abstract class CodeWriter {
		/// <summary>
		/// Writes the next byte
		/// </summary>
		/// <param name="value">Value</param>
		public abstract void WriteByte(byte value);
	}
}
#endif
