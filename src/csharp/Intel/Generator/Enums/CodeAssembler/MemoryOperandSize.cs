// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Enums {
	[Enum("MemoryOperandSize", "CodeAsmMemoryOperandSize")]
	enum MemoryOperandSize {
		None,
		Byte,
		Word,
		Dword,
		Qword,
		Tbyte,
		Fword,
		Xword,
		Yword,
		Zword,
	}
}
