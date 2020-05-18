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

namespace Generator.Enums.InstructionInfo {
	[Enum("RegisterFlags", Flags = true, NoInitialize = true)]
	[Flags]
	enum RegisterFlags {
		None				= 0,
		SegmentRegister		= 1,
		GPR					= 2,
		GPR8				= 4,
		GPR16				= 8,
		GPR32				= 0x10,
		GPR64				= 0x20,
		XMM					= 0x40,
		YMM					= 0x80,
		ZMM					= 0x100,
		VectorRegister		= 0x200,
		IP					= 0x400,
		K					= 0x800,
		BND					= 0x1000,
		CR					= 0x2000,
		DR					= 0x4000,
		TR					= 0x8000,
		ST					= 0x10000,
		MM					= 0x20000,
	}
}
