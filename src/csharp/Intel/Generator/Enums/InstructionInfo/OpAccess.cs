// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Enums.InstructionInfo {
	[Enum("OpAccess", Documentation = "Operand, register and memory access", Public = true)]
	enum OpAccess {
		[Comment("Nothing is read and nothing is written")]
		None,
		[Comment("The value is read")]
		Read,
		[Comment("The value is sometimes read and sometimes not")]
		CondRead,
		[Comment("The value is completely overwritten")]
		Write,
		[Comment("Conditional write, sometimes it's written and sometimes it's not modified")]
		CondWrite,
		[Comment("The value is read and written")]
		ReadWrite,
		[Comment("The value is read and sometimes written")]
		ReadCondWrite,
		[Comment("The memory operand doesn't refer to memory (eg. #(c:LEA)# instruction) or it's an instruction that doesn't read the data to a register or doesn't write to the memory location, it just prefetches/invalidates it, eg. #(c:INVLPG)#, #(c:PREFETCHNTA)#, #(c:VGATHERPF0DPS)#, etc. Some of those instructions still check if the code can access the memory location.")]
		NoMemAccess,
	}
}
