// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Enums.Decoder {
	[Enum("DecoderError", Documentation = "Decoder error", Public = true)]
	enum DecoderError {
		[Comment("No error. The last decoded instruction is a valid instruction")]
		None,
		[Comment("It's an invalid instruction or an invalid encoding of an existing instruction (eg. some reserved bit is set/cleared)")]
		InvalidInstruction,
		[Comment("There's not enough bytes left to decode the instruction")]
		NoMoreBytes,
	}
}
