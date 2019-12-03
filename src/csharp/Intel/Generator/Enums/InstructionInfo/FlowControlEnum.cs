/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System.Linq;

namespace Generator.Enums.InstructionInfo {
	enum FlowControl {
		[Comment("The next instruction that will be executed is the next instruction in the instruction stream")]
		Next,
		[Comment("It's an unconditional branch instruction: #(c:JMP NEAR)#, #(c:JMP FAR)#")]
		UnconditionalBranch,
		[Comment("It's an unconditional indirect branch: #(c:JMP NEAR reg)#, #(c:JMP NEAR [mem])#, #(c:JMP FAR [mem])#")]
		IndirectBranch,
		[Comment("It's a conditional branch instruction: #(c:Jcc SHORT)#, #(c:Jcc NEAR)#, #(c:LOOP)#, #(c:LOOPcc)#, #(c:JRCXZ)#")]
		ConditionalBranch,
		[Comment("It's a return instruction: #(c:RET NEAR)#, #(c:RET FAR)#, #(c:IRET)#, #(c:SYSRET)#, #(c:SYSEXIT)#, #(c:RSM)#, #(c:VMLAUNCH)#, #(c:VMRESUME)#, #(c:VMRUN)#, #(c:SKINIT)#")]
		Return,
		[Comment("It's a call instruction: #(c:CALL NEAR)#, #(c:CALL FAR)#, #(c:SYSCALL)#, #(c:SYSENTER)#, #(c:VMCALL)#, #(c:VMMCALL)#")]
		Call,
		[Comment("It's an indirect call instruction: #(c:CALL NEAR reg)#, #(c:CALL NEAR [mem])#, #(c:CALL FAR [mem])#")]
		IndirectCall,
		[Comment("It's an interrupt instruction: #(c:INT n)#, #(c:INT3)#, #(c:INT1)#, #(c:INTO)#")]
		Interrupt,
		[Comment("It's #(c:XBEGIN)#, #(c:XABORT)# or #(c:XEND)#")]
		XbeginXabortXend,
		[Comment("It's an invalid instruction, eg. #(e:Code.INVALID)#, #(c:UD0)#, #(c:UD1)#, #(c:UD2)#")]
		Exception,
	}

	static class FlowControlEnum {
		const string documentation = "Flow control";

		static EnumValue[] GetValues() =>
			typeof(FlowControl).GetFields().Where(a => a.IsLiteral).Select(a => new EnumValue((uint)(FlowControl)a.GetValue(null)!, a.Name, CommentAttribute.GetDocumentation(a))).ToArray();

		public static readonly EnumType Instance = new EnumType(TypeIds.FlowControl, documentation, GetValues(), EnumTypeFlags.Public);
	}
}
