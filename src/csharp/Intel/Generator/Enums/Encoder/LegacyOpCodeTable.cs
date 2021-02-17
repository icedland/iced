// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Enums.Encoder {
	[Enum("LegacyOpCodeTable", NoInitialize = true)]
	enum LegacyOpCodeTable {
		Normal					= 0,
		Table0F					= 1,
		Table0F38				= 2,
		Table0F3A				= 3,
	}
}
