// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Enums.Formatter.Nasm {
	[Enum(nameof(BranchSizeInfo), "NasmBranchSizeInfo", NoInitialize = true)]
	enum BranchSizeInfo {
		None,
		Near,
		NearWord,
		NearDword,
		Word,
		Dword,
		Short,
	}
}
