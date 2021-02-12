// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

namespace Generator.Enums.Encoder {
	[Enum("OpCodeTableKind", Documentation = "Opcode table", Public = true)]
	enum OpCodeTableKind {
		[Comment("Legacy encoding table")]
		Normal,

		[Comment("#(c:0Fxx)# table (legacy, VEX, EVEX)")]
		T0F,

		[Comment("#(c:0F38xx)# table (legacy, VEX, EVEX)")]
		T0F38,

		[Comment("#(c:0F3Axx)# table (legacy, VEX, EVEX)")]
		T0F3A,

		[Comment("#(c:XOP8)# table (XOP)")]
		XOP8,

		[Comment("#(c:XOP9)# table (XOP)")]
		XOP9,

		[Comment("#(c:XOPA)# table (XOP)")]
		XOPA,
	}
}
