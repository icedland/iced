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

namespace Generator.Enums {
	[Enum("TupleType", Documentation = "Tuple type (EVEX) which can be used to get the disp8 scale factor #(c:N)#", Public = true)]
	enum TupleType {
		[Comment("#(c:N = 1)#")]
		N1,
		[Comment("#(c:N = 2)#")]
		N2,
		[Comment("#(c:N = 4)#")]
		N4,
		[Comment("#(c:N = 8)#")]
		N8,
		[Comment("#(c:N = 16)#")]
		N16,
		[Comment("#(c:N = 32)#")]
		N32,
		[Comment("#(c:N = 64)#")]
		N64,
		[Comment("#(c:N = b ? 4 : 8)#")]
		N8b4,
		[Comment("#(c:N = b ? 4 : 16)#")]
		N16b4,
		[Comment("#(c:N = b ? 4 : 32)#")]
		N32b4,
		[Comment("#(c:N = b ? 4 : 64)#")]
		N64b4,
		[Comment("#(c:N = b ? 8 : 16)#")]
		N16b8,
		[Comment("#(c:N = b ? 8 : 32)#")]
		N32b8,
		[Comment("#(c:N = b ? 8 : 64)#")]
		N64b8,
	}
}
