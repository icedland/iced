// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Enums.Encoder {
	[Enum("OpCodeTableKind", Documentation = "Opcode table", Public = true)]
	enum OpCodeTableKind {
		[Comment("Legacy/#(c:MAP0)# table")]
		Normal,

		[Comment("#(c:0F)#/#(c:MAP1)# table (legacy, VEX, EVEX, MVEX)")]
		T0F,

		[Comment("#(c:0F38)#/#(c:MAP2)# table (legacy, VEX, EVEX, MVEX)")]
		T0F38,

		[Comment("#(c:0F3A)#/#(c:MAP3)# table (legacy, VEX, EVEX, MVEX)")]
		T0F3A,

		[Comment("#(c:MAP5)# table (EVEX)")]
		MAP5,

		[Comment("#(c:MAP6)# table (EVEX)")]
		MAP6,

		[Comment("#(c:MAP8)# table (XOP)")]
		MAP8,

		[Comment("#(c:MAP9)# table (XOP)")]
		MAP9,

		[Comment("#(c:MAP10)# table (XOP)")]
		MAP10,
	}
}
