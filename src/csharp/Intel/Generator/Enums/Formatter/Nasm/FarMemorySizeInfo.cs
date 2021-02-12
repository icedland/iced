// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

namespace Generator.Enums.Formatter.Nasm {
	[Enum(nameof(FarMemorySizeInfo), "NasmFarMemorySizeInfo")]
	enum FarMemorySizeInfo {
		None,
		Word,
		Dword,
	}
}
