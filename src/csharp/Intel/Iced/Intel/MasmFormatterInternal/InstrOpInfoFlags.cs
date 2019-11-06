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

#if !NO_MASM_FORMATTER && !NO_FORMATTER
using System;

namespace Iced.Intel.MasmFormatterInternal {
	[Flags]
	enum InstrOpInfoFlags : ushort {
		None						= 0,

		MemSize_Mask				= 7,
		// Use xmmword ptr etc
		MemSize_Sse					= 0,
		// Use mmword ptr etc
		MemSize_Mmx					= 1,
		// use qword ptr, oword ptr
		MemSize_Normal				= 2,
		// show no mem size
		MemSize_Nothing				= 3,
		MemSize_XmmwordPtr			= 4,
		MemSize_DwordOrQword		= 5,

		// AlwaysShowMemorySize is disabled: always show memory size
		ShowNoMemSize_ForceSize		= 8,
		ShowMinMemSize_ForceSize	= 0x0010,

		JccNotTaken					= 0x0020,
		JccTaken					= 0x0040,
		BndPrefix					= 0x0080,
		IgnoreIndexReg				= 0x0100,
		MnemonicIsDirective			= 0x0200,
	}
}
#endif
