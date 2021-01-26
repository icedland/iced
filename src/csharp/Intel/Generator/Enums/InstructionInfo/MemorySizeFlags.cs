// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

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
