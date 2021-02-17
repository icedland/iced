// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;

namespace Generator.Enums.InstructionInfo {
	[Enum("RflagsBits", Documentation = "#(c:RFLAGS)# bits, FPU condition code bits and misc bits (#(c:UIF)#) supported by the instruction info code", Flags = true, NoInitialize = true, Public = true)]
	[Flags]
	enum RflagsBits {
		[Comment("No bit is set")]
		None	= 0,
		[Comment("#(c:RFLAGS.OF)#")]
		OF		= 0x00000001,
		[Comment("#(c:RFLAGS.SF)#")]
		SF		= 0x00000002,
		[Comment("#(c:RFLAGS.ZF)#")]
		ZF		= 0x00000004,
		[Comment("#(c:RFLAGS.AF)#")]
		AF		= 0x00000008,
		[Comment("#(c:RFLAGS.CF)#")]
		CF		= 0x00000010,
		[Comment("#(c:RFLAGS.PF)#")]
		PF		= 0x00000020,
		[Comment("#(c:RFLAGS.DF)#")]
		DF		= 0x00000040,
		[Comment("#(c:RFLAGS.IF)#")]
		IF		= 0x00000080,
		[Comment("#(c:RFLAGS.AC)#")]
		AC		= 0x00000100,
		[Comment("#(c:UIF)#")]
		UIF		= 0x00000200,
		[Comment("FPU status word bit #(c:C0)#")]
		C0		= 0x00000400,
		[Comment("FPU status word bit #(c:C1)#")]
		C1		= 0x00000800,
		[Comment("FPU status word bit #(c:C2)#")]
		C2		= 0x00001000,
		[Comment("FPU status word bit #(c:C3)#")]
		C3		= 0x00002000,
	}
}
