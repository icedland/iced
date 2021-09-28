// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Enums.Encoder {
	[Enum("MvexEHBit", Documentation = "(MVEX) EH bit value", Public = true)]
	enum MvexEHBit {
		[Comment("Not hard coded to 0 or 1 so can be used for other purposes")]
		None,
		[Comment("EH bit must be 0")]
		EH0,
		[Comment("EH bit must be 1")]
		EH1,
	}
}
