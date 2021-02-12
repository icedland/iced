// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

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
