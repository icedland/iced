// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Enums.Encoder {
	[Enum("RelocKind", Documentation = "Relocation kind", Public = true)]
	enum RelocKind {
		[Comment("64-bit offset. Only used if it's 64-bit code.")]
		Offset64,
	}
}
