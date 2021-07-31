// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER && BLOCK_ENCODER && CODE_ASSEMBLER
namespace Iced.Intel {
	/// <summary>
	/// Memory operand size.
	/// </summary>
	enum MemoryOperandSize {
		/// <summary>
		/// An unspecified memory operand.
		/// </summary>
		None,
		/// <summary>
		/// A 8-bit / byte pointer.
		/// </summary>
		Byte,
		/// <summary>
		/// A 16-bit / word pointer.
		/// </summary>
		Word,
		/// <summary>
		/// A 32-bit / double-word pointer.
		/// </summary>
		Dword,
		/// <summary>
		/// A 64-bit / quad word pointer.
		/// </summary>
		Qword,
		/// <summary>
		/// A 80-bit / tword pointer.
		/// </summary>
		Tword,
		/// <summary>
		/// A 16-bit segment + 32-bit address.
		/// </summary>
		Fword,
		/// <summary>
		/// A 128-bit / xmm pointer.
		/// </summary>
		Xword,
		/// <summary>
		/// A 256-bit / ymm pointer.
		/// </summary>
		Yword,
		/// <summary>
		/// A 512-bit / zmm pointer.
		/// </summary>
		Zword,
	}
}
#endif
