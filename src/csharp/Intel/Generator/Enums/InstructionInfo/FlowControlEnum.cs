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
		[Comment("It's an unconditional branch instruction: #(c:jmp near)#, #(c:jmp far)#")]
		UnconditionalBranch,
		[Comment("It's an unconditional indirect branch: #(c:jmp near reg)#, #(c:jmp near [mem])#, #(c:jmp far [mem])#")]
		IndirectBranch,
		[Comment("It's a conditional branch instruction: #(c:jcc short)#, #(c:jcc near)#, #(c:loop)#, #(c:loopcc)#, #(c:jrcxz)#")]
		ConditionalBranch,
		[Comment("It's a return instruction: #(c:ret near)#, #(c:ret far)#, #(c:iret)#, #(c:sysret)#, #(c:sysexit)#, #(c:rsm)#, #(c:vmlaunch)#, #(c:vmresume)#, #(c:vmrun)#, #(c:skinit)#")]
		Return,
		[Comment("It's a call instruction: #(c:call near)#, #(c:call far)#, #(c:syscall)#, #(c:sysenter)#, #(c:vmcall)#, #(c:vmmcall)#")]
		Call,
		[Comment("It's an indirect call instruction: #(c:call near reg)#, #(c:call near [mem])#, #(c:call far [mem])#")]
		IndirectCall,
		[Comment("It's an interrupt instruction: #(c:int n)#, #(c:int3)#, #(c:int1)#, #(c:into)#")]
		Interrupt,
		[Comment("It's #(c:xbegin)#, #(c:xabort)# or #(c:xend)#")]
		XbeginXabortXend,
		[Comment("It's an invalid instruction, eg. #(e:Code.INVALID)#, #(c:ud0)#, #(c:ud1)#, #(c:ud2)#")]
		Exception,
	}

	static class FlowControlEnum {
		const string documentation = "Flow control";

		static EnumValue[] GetValues() =>
			typeof(FlowControl).GetFields().Where(a => a.IsLiteral).Select(a => new EnumValue((uint)(FlowControl)a.GetValue(null)!, a.Name, CommentAttribute.GetDocumentation(a))).ToArray();

		public static readonly EnumType Instance = new EnumType(TypeIds.FlowControl, documentation, GetValues(), EnumTypeFlags.Public);
	}
}
