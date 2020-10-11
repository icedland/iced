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
	[Enum("RflagsBits", Documentation = "#(c:RFLAGS)# bits (and #(c:UIF)#) supported by the instruction info code", Flags = true, NoInitialize = true, Public = true)]
	[Flags]
	enum RflagsBits {
		None	= 0,
		[Comment("#(c:RFLAGS.OF)#")]
		OF		= 0x00000001,
		[Comment("#(c:RFLAGS.SF)#")]
		SF		= 0x00000002,
		[Comment("#(c:RFLAGS.ZF)#")]
		ZF		= 0x00000004,
		[Comment("#(c:RFLAGS.AF)#")]
		AF		= 0x00000008,
		[Comment("#(c:RFLAGS.CF)#")]
		CF		= 0x00000010,
		[Comment("#(c:RFLAGS.PF)#")]
		PF		= 0x00000020,
		[Comment("#(c:RFLAGS.DF)#")]
		DF		= 0x00000040,
		[Comment("#(c:RFLAGS.IF)#")]
		IF		= 0x00000080,
		[Comment("#(c:RFLAGS.AC)#")]
		AC		= 0x00000100,
		[Comment("#(c:UIF)#")]
		UIF		= 0x00000200,
	}
}
