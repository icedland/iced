// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Enums.Encoder {
	[Enum("RepPrefixKind", Documentation = "#(c:REP)#/#(c:REPE)#/#(c:REPNE)# prefix", Public = true)]
	enum RepPrefixKind {
		[Comment("No #(c:REP)#/#(c:REPE)#/#(c:REPNE)# prefix")]
		None,

		[Comment("#(c:REP)#/#(c:REPE)# prefix")]
		Repe,

		[Comment("#(c:REPNE)# prefix")]
		Repne,
	}
}
