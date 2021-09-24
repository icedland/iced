// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Enums.InstructionInfo {
	[Enum("FlowControl", Documentation = "Control flow", Public = true)]
	enum FlowControl {
		[Comment("The next instruction that will be executed is the next instruction in the instruction stream")]
		Next,
		[Comment("It's an unconditional branch instruction: #(c:JMP NEAR)#, #(c:JMP FAR)#")]
		UnconditionalBranch,
		[Comment("It's an unconditional indirect branch: #(c:JMP NEAR reg)#, #(c:JMP NEAR [mem])#, #(c:JMP FAR [mem])#")]
		IndirectBranch,
		[Comment("It's a conditional branch instruction: #(c:Jcc SHORT)#, #(c:Jcc NEAR)#, #(c:LOOP)#, #(c:LOOPcc)#, #(c:JRCXZ)#, #(c:JKccD SHORT)#, #(c:JKccD NEAR)#")]
		ConditionalBranch,
		[Comment("It's a return instruction: #(c:RET NEAR)#, #(c:RET FAR)#, #(c:IRET)#, #(c:SYSRET)#, #(c:SYSEXIT)#, #(c:RSM)#, #(c:SKINIT)#, #(c:RDM)#, #(c:UIRET)#")]
		Return,
		[Comment("It's a call instruction: #(c:CALL NEAR)#, #(c:CALL FAR)#, #(c:SYSCALL)#, #(c:SYSENTER)#, #(c:VMLAUNCH)#, #(c:VMRESUME)#, #(c:VMCALL)#, #(c:VMMCALL)#, #(c:VMGEXIT)#, #(c:VMRUN)#, #(c:TDCALL)#, #(c:SEAMCALL)#, #(c:SEAMRET)#")]
		Call,
		[Comment("It's an indirect call instruction: #(c:CALL NEAR reg)#, #(c:CALL NEAR [mem])#, #(c:CALL FAR [mem])#")]
		IndirectCall,
		[Comment("It's an interrupt instruction: #(c:INT n)#, #(c:INT3)#, #(c:INT1)#, #(c:INTO)#, #(c:SMINT)#, #(c:DMINT)#")]
		Interrupt,
		[Comment("It's #(c:XBEGIN)#")]
		XbeginXabortXend,
		[Comment("It's an invalid instruction, eg. #(e:Code.INVALID)#, #(c:UD0)#, #(c:UD1)#, #(c:UD2)#")]
		Exception,
	}
}
