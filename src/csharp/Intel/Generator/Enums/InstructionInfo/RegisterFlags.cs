// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;

namespace Generator.Enums.InstructionInfo {
	[Enum("RegisterFlags", Flags = true, NoInitialize = true)]
	[Flags]
	enum RegisterFlags {
		None				= 0,
		SegmentRegister		= 1,
		GPR					= 2,
		GPR8				= 4,
		GPR16				= 8,
		GPR32				= 0x10,
		GPR64				= 0x20,
		XMM					= 0x40,
		YMM					= 0x80,
		ZMM					= 0x100,
		VectorRegister		= 0x200,
		IP					= 0x400,
		K					= 0x800,
		BND					= 0x1000,
		CR					= 0x2000,
		DR					= 0x4000,
		TR					= 0x8000,
		ST					= 0x10000,
		MM					= 0x20000,
		TMM					= 0x40000,
	}
}
