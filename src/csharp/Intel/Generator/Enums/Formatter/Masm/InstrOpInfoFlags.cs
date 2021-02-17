// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;

namespace Generator.Enums.Formatter.Masm {
	[Enum(nameof(InstrOpInfoFlags), "MasmInstrOpInfoFlags", Flags = true, NoInitialize = true)]
	[Flags]
	enum InstrOpInfoFlags : ushort {
		None						= 0,

		MemSize_Mask				= 7,
		// Use xmmword ptr etc
		MemSize_Sse					= 0,
		// Use mmword ptr etc
		MemSize_Mmx					= 1,
		// use qword ptr, oword ptr
		MemSize_Normal				= 2,
		// show no mem size
		MemSize_Nothing				= 3,
		MemSize_XmmwordPtr			= 4,
		MemSize_DwordOrQword		= 5,

		// AlwaysShowMemorySize is disabled: always show memory size
		ShowNoMemSize_ForceSize		= 8,
		ShowMinMemSize_ForceSize	= 0x0010,

		JccNotTaken					= 0x0020,
		JccTaken					= 0x0040,
		BndPrefix					= 0x0080,
		IgnoreIndexReg				= 0x0100,
		MnemonicIsDirective			= 0x0200,
	}
}
