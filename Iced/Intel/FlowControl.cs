/*
    Copyright (C) 2018-2019 de4dot@gmail.com

    This file is part of Iced.

    Iced is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Iced is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Iced.  If not, see <https://www.gnu.org/licenses/>.
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
