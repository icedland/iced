// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Enums.Encoder {
	[Enum("DisplSize")]
	enum DisplSize : byte {
		None,
		Size1,
		Size2,
		Size4,
		Size8,
		RipRelSize4_Target32,
		RipRelSize4_Target64,
	}
}
