// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Enums.Encoder {
	[Enum("MvexInfoFlags2", Flags = true, NoInitialize = true)]
	enum MvexInfoFlags2 {
		None					= 0,
		// {er} and {sae} aren't allowed and SSS must be 0
		NoSaeRoundingControl	= 0x01,
		// Set if it's Si32/Sf32/etc
		ConvFn32				= 0x02,
		// Ignores {eh} bit, eg. VPREFETCH* and CLEVICT*
		IgnoresEvictionHint		= 0x04,
	}
}
