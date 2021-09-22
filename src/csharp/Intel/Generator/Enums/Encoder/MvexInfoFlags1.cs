// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Enums.Encoder {
	[Enum("MvexInfoFlags1", Flags = true, NoInitialize = true)]
	enum MvexInfoFlags1 {
		None					= 0,
		NDD						= 0x01,
		NDS						= 0x02,
		// Eviction hint {eh} is supported if it has a memory operand
		EvictionHint			= 0x04,
		// imm8 rounding control is supported
		ImmRoundingControl		= 0x08,
		// {er} is supported
		RoundingControl			= 0x10,
		// {sae} is supported
		SuppressAllExceptions	= 0x20,
		// {k1} is ignored (no #UD)
		IgnoresOpMaskRegister	= 0x40,
		// Opmask register is required or #UD
		RequireOpMaskRegister	= 0x80,
	}
}
