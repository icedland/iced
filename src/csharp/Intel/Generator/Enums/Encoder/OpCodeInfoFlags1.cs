// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;

namespace Generator.Enums.Encoder {
	[Enum("OpCodeInfoFlags1", Flags = true, NoInitialize = true)]
	enum OpCodeInfoFlags1 : uint {
		None					= 0,
		Cpl0Only				= 0x00000001,
		Cpl3Only				= 0x00000002,
		InputOutput				= 0x00000004,
		Nop						= 0x00000008,
		ReservedNop				= 0x00000010,
		SerializingIntel		= 0x00000020,
		SerializingAmd			= 0x00000040,
		MayRequireCpl0			= 0x00000080,
		CetTracked				= 0x00000100,
		NonTemporal				= 0x00000200,
		FpuNoWait				= 0x00000400,
		IgnoresModBits			= 0x00000800,
		No66					= 0x00001000,
		NFx						= 0x00002000,
		RequiresUniqueRegNums	= 0x00004000,
		Privileged				= 0x00008000,
		SaveRestore				= 0x00010000,
		StackInstruction		= 0x00020000,
		IgnoresSegment			= 0x00040000,
		OpMaskReadWrite			= 0x00080000,
		ModRegRmString			= 0x00100000,
		/// <summary><see cref="DecOptionValue"/></summary>
		DecOptionValueMask		= 0x1F,
		DecOptionValueShift		= 21,

		// FREE FREE FREE FREE
		// [29:24] = free
		// BITS BITS BITS BITS

		ForceOpSize64			= 0x40000000,
		RequiresUniqueDestRegNum= 0x80000000,
	}
}
