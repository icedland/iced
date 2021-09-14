// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Enums.Encoder {
	[Enum("NonDestructiveOpKind")]
	enum NonDestructiveOpKind {
		None,
		// non-destructive dst
		NDD,
		// non-destructive src
		NDS,
	}
}
