// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

using System;

namespace Generator.Enums.Instruction {
	/// <summary>
	/// [1:0]	= Scale
	/// [4:2]	= Size of displacement: 0, 1, 2, 4, 8
	/// [7:5]	= Segment register prefix: none, es, cs, ss, ds, fs, gs, reserved
	/// [14:8]	= Not used
	/// [15]	= Broadcasted memory
	/// </summary>
	[Enum(nameof(MemoryFlags), "Instruction_MemoryFlags", Flags = true, NoInitialize = true)]
	[Flags]
	enum MemoryFlags : ushort {
		ScaleMask				= 3,
		DisplSizeShift			= 2,
		DisplSizeMask			= 7,
		SegmentPrefixShift		= 5,
		SegmentPrefixMask		= 7,
		// Unused bits here
		Broadcast				= 0x8000,
	}
}
