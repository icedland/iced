// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

namespace Generator.Enums {
	[Enum("EncodingKind", Documentation = "Instruction encoding", Public = true)]
	enum EncodingKind {
		[Comment("Legacy encoding")]
		Legacy,
		[Comment("VEX encoding")]
		VEX,
		[Comment("EVEX encoding")]
		EVEX,
		[Comment("XOP encoding")]
		XOP,
		[Comment("3DNow! encoding")]
		D3NOW,
	}
}
