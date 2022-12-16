// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Enums.InstructionInfo {
	[Enum("ConditionCode", Documentation = "Instruction condition code (used by #(c:Jcc)#, #(c:SETcc)#, #(c:CMOVcc)#, #(c:CMPccXADD)#, #(c:LOOPcc)#)", Public = true)]
	enum ConditionCode {
		[Comment("The instruction doesn't have a condition code")]
		None,
		[Comment("Overflow (#(c:OF=1)#)")]
		o,
		[Comment("Not overflow (#(c:OF=0)#)")]
		no,
		[Comment("Below (unsigned) (#(c:CF=1)#)")]
		b,
		[Comment("Above or equal (unsigned) (#(c:CF=0)#)")]
		ae,
		[Comment("Equal / zero (#(c:ZF=1)#)")]
		e,
		[Comment("Not equal / zero (#(c:ZF=0)#)")]
		ne,
		[Comment("Below or equal (unsigned) (#(c:CF=1 or ZF=1)#)")]
		be,
		[Comment("Above (unsigned) (#(c:CF=0 and ZF=0)#)")]
		a,
		[Comment("Signed (#(c:SF=1)#)")]
		s,
		[Comment("Not signed (#(c:SF=0)#)")]
		ns,
		[Comment("Parity (#(c:PF=1)#)")]
		p,
		[Comment("Not parity (#(c:PF=0)#)")]
		np,
		[Comment("Less (signed) (#(c:SF!=OF)#)")]
		l,
		[Comment("Greater than or equal (signed) (#(c:SF=OF)#)")]
		ge,
		[Comment("Less than or equal (signed) (#(c:ZF=1 or SF!=OF)#)")]
		le,
		[Comment("Greater (signed) (#(c:ZF=0 and SF=OF)#)")]
		g,
	}
}
