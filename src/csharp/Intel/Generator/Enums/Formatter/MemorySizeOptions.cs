// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

namespace Generator.Enums.Formatter {
	[Enum("MemorySizeOptions", Documentation = "Memory size options used by the formatters", Public = true)]
	enum MemorySizeOptions {
		[Comment("Show memory size if the assembler requires it, else don't show anything")]
		Default,

		[Comment("Always show the memory size, even if the assembler doesn't need it")]
		Always,

		[Comment("Show memory size if a human can't figure out the size of the operand")]
		Minimum,

		[Comment("Never show memory size")]
		Never,
	}
}
