// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

namespace Generator.Enums.Formatter.Nasm {
	[Enum(nameof(SignExtendInfo), "NasmSignExtendInfo")]
	enum SignExtendInfo {
		None,
		Sex1to2,
		Sex1to4,
		Sex1to8,
		Sex4to8,
		Sex2,
		Sex4,
	}
}
