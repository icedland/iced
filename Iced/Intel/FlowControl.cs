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

#if !NO_INSTR_INFO
namespace Iced.Intel {
	/// <summary>
	/// Flow control
	/// </summary>
	public enum FlowControl {
		/// <summary>
		/// The next instruction that will be executed is the next instruction in the instruction stream
		/// </summary>
		Next,

		/// <summary>
		/// It's an unconditional branch instruction: jmp near, jmp far
		/// </summary>
		UnconditionalBranch,

		/// <summary>
		/// It's an unconditional indirect branch: jmp near reg, jmp near [mem], jmp far [mem]
		/// </summary>
		IndirectBranch,

		/// <summary>
		/// It's a conditional branch instruction: jcc short, jcc near, loop, loopcc, jrcxz
		/// </summary>
		ConditionalBranch,

		/// <summary>
		/// It's a return instruction: ret near, ret far, iret, sysret, sysexit, rsm, vmlaunch, vmresume, vmrun, skinit
		/// </summary>
		Return,

		/// <summary>
		/// It's a call instruction: call near, call far, syscall, sysenter, vmcall, vmmcall
		/// </summary>
		Call,

		/// <summary>
		/// It's an indirect call instruction: call near reg, call near [mem], call far [mem]
		/// </summary>
		IndirectCall,

		/// <summary>
		/// It's an interrupt instruction: int n, int3, int1, into
		/// </summary>
		Interrupt,

		/// <summary>
		/// It's xbegin, xabort or xend
		/// </summary>
		XbeginXabortXend,

		/// <summary>
		/// It's an invalid instruction, eg. <see cref="Code.INVALID"/>, ud0, ud1, ud2
		/// </summary>
		Exception,

		// If more values are added, update InfoFlags2.FlowControlMask/InfoFlags2.FlowControlShift if needed
	}
}
#endif
