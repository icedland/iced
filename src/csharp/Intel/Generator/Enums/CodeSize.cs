// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

namespace Generator.Enums {
	[Enum("CodeSize", Documentation = "The code size (16/32/64) that was used when an instruction was decoded", Public = true)]
	enum CodeSize {
		[Comment("Unknown size")]
		Unknown,
		[Comment("16-bit code")]
		Code16,
		[Comment("32-bit code")]
		Code32,
		[Comment("64-bit code")]
		Code64,
	}
}
