// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;

namespace Generator.Enums.InstructionInfo {
	[Enum("MemorySizeFlags", Flags = true, NoInitialize = true)]
	[Flags]
	enum MemorySizeFlags {
		None				= 0,
		Signed				= 1,
		Broadcast			= 2,
		Packed				= 4,
	}
}
