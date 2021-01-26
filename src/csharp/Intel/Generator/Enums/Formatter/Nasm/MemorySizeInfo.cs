// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

namespace Generator.Enums.Formatter.Nasm {
	[Enum(nameof(MemorySizeInfo), "NasmMemorySizeInfo")]
	enum MemorySizeInfo {
		None,
		Word,
		Dword,
		Qword,
	}
}
