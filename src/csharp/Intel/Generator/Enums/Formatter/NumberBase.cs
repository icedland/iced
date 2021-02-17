// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Enums.Formatter {
	[Enum("NumberBase", Documentation = "Number base", Public = true)]
	enum NumberBase {
		[Comment("Hex numbers (base 16)")]
		Hexadecimal,

		[Comment("Decimal numbers (base 10)")]
		Decimal,

		[Comment("Octal numbers (base 8)")]
		Octal,

		[Comment("Binary numbers (base 2)")]
		Binary,
	}
}
