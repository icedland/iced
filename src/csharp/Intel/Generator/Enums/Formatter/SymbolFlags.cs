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

namespace Generator.Enums.Formatter {
	[Enum("SymbolFlags", Documentation = "Symbol flags", Public = true, Flags = true, NoInitialize = true)]
	[Flags]
	enum SymbolFlags : uint {
		[Comment("No bit is set")]
		None				= 0,

		[Comment("It's a symbol relative to a register, eg. a struct offset #(c:[ebx+some_struct.field1])#. If this is cleared, it's the address of a symbol.")]
		Relative			= 0x00000001,

		[Comment("It's a signed symbol and it should be displayed as #(c:-symbol)# or #(c:reg-symbol)# instead of #(c:symbol)# or #(c:reg+symbol)#")]
		Signed				= 0x00000002,

		[Comment("Set if #(f:SymbolResult.SymbolSize)# is valid")]
		HasSymbolSize		= 0x00000004,
	}
}
