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
		DecOptionValueMask		= 0xF,
		DecOptionValueShift		= 21,

		// FREE FREE FREE FREE
		// [31:25] = free
		// BITS BITS BITS BITS
	}
}
