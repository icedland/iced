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

namespace Generator.Enums.Formatter.Masm {
	[Enum(nameof(SymbolTestFlags), "MasmSymbolTestFlags", Flags = true, NoInitialize = true)]
	[Flags]
	enum SymbolTestFlags {
		None					= 0,
		[Comment("Symbol is found")]
		Symbol					= 1,
		[Comment("Signed symbol")]
		Signed					= 2,
		[Comment("#(c:options.MasmSymbolDisplInBrackets)#")]
		SymbolDisplInBrackets	= 4,
		[Comment("#(c:options.MasmDisplInBrackets)#")]
		DisplInBrackets			= 8,
		[Comment("#(c:options.RipRelativeAddresses)#")]
		Rip						= 0x10,
		[Comment("#(c:options.ShowZeroDisplacements)#")]
		ShowZeroDisplacements	= 0x20,
		[Comment("#(c:!options.MasmAddDsPrefix32)#")]
		NoAddDsPrefix32			= 0x40,
	}
}
