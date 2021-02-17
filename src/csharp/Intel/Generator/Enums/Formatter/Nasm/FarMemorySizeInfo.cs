// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Enums.Formatter.Nasm {
	[Enum(nameof(FarMemorySizeInfo), "NasmFarMemorySizeInfo")]
	enum FarMemorySizeInfo {
		None,
		Word,
		Dword,
	}
}
