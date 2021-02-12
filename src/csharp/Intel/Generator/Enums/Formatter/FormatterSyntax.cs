// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

using System;

namespace Generator.Enums.Formatter {
	[Enum("FormatterSyntax", Documentation = "Formatter syntax (GNU Assembler, Intel XED, masm, nasm)", Public = true)]
	enum FormatterSyntax {
		[Comment("GNU Assembler (AT&T)")]
		Gas,
		[Comment("Intel XED")]
		Intel,
		[Comment("masm")]
		Masm,
		[Comment("nasm")]
		Nasm,
		// This enum only contains entries for formatters that implement the Formatter iface/trait
		// so it doesn't include fast fmt.
	}
}
