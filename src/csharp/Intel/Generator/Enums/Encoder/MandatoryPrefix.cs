// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Enums.Encoder {
	[Enum("MandatoryPrefix", Documentation = "Mandatory prefix", Public = true)]
	enum MandatoryPrefix {
		[Comment("No mandatory prefix (legacy and 3DNow! tables only)")]
		None,
		[Comment("Empty mandatory prefix (no #(c:66)#, #(c:F3)# or #(c:F2)# prefix)")]
		PNP,
		[Comment("#(c:66)# prefix")]
		P66,
		[Comment("#(c:F3)# prefix")]
		PF3,
		[Comment("#(c:F2)# prefix")]
		PF2,
	}
}
