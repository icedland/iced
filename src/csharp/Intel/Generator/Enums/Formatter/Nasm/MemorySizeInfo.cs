// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Enums.Formatter.Nasm {
	[Enum(nameof(MemorySizeInfo), "NasmMemorySizeInfo")]
	enum MemorySizeInfo {
		None,
		Word,
		Dword,
		Qword,
	}
}
