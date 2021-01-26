// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

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
