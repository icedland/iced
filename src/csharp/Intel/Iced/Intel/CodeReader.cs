// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if DECODER
namespace Iced.Intel {
	/// <summary>
	/// Reads instruction bytes
	/// </summary>
	public abstract class CodeReader {
		/// <summary>
		/// Reads the next byte or returns less than 0 if there are no more bytes
		/// </summary>
		/// <returns></returns>
		public abstract int ReadByte();
	}
}
#endif
