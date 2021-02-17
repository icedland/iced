// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Enums.Formatter.Nasm {
	[Enum(nameof(SizeOverride), "NasmSizeOverride", NoInitialize = true)]
	enum SizeOverride {
		None,
		Size16,
		Size32,
		Size64,
	}
}
