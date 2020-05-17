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
		None,
		[Comment("#(c:N = b ? (W ? 8 : 4) : 16)#")]
		Full_128,
		[Comment("#(c:N = b ? (W ? 8 : 4) : 32)#")]
		Full_256,
		[Comment("#(c:N = b ? (W ? 8 : 4) : 64)#")]
		Full_512,
		[Comment("#(c:N = b ? 4 : 8)#")]
		Half_128,
		[Comment("#(c:N = b ? 4 : 16)#")]
		Half_256,
		[Comment("#(c:N = b ? 4 : 32)#")]
		Half_512,
		[Comment("#(c:N = 16)#")]
		Full_Mem_128,
		[Comment("#(c:N = 32)#")]
		Full_Mem_256,
		[Comment("#(c:N = 64)#")]
		Full_Mem_512,
		[Comment("#(c:N = W ? 8 : 4)#")]
		Tuple1_Scalar,
		[Comment("#(c:N = 1)#")]
		Tuple1_Scalar_1,
		[Comment("#(c:N = 2)#")]
		Tuple1_Scalar_2,
		[Comment("#(c:N = 4)#")]
		Tuple1_Scalar_4,
		[Comment("#(c:N = 8)#")]
		Tuple1_Scalar_8,
		[Comment("#(c:N = 4)#")]
		Tuple1_Fixed_4,
		[Comment("#(c:N = 8)#")]
		Tuple1_Fixed_8,
		[Comment("#(c:N = W ? 16 : 8)#")]
		Tuple2,
		[Comment("#(c:N = W ? 32 : 16)#")]
		Tuple4,
		[Comment("#(c:N = W ? error : 32)#")]
		Tuple8,
		[Comment("#(c:N = 16)#")]
		Tuple1_4X,
		[Comment("#(c:N = 8)#")]
		Half_Mem_128,
		[Comment("#(c:N = 16)#")]
		Half_Mem_256,
		[Comment("#(c:N = 32)#")]
		Half_Mem_512,
		[Comment("#(c:N = 4)#")]
		Quarter_Mem_128,
		[Comment("#(c:N = 8)#")]
		Quarter_Mem_256,
		[Comment("#(c:N = 16)#")]
		Quarter_Mem_512,
		[Comment("#(c:N = 2)#")]
		Eighth_Mem_128,
		[Comment("#(c:N = 4)#")]
		Eighth_Mem_256,
		[Comment("#(c:N = 8)#")]
		Eighth_Mem_512,
		[Comment("#(c:N = 16)#")]
		Mem128,
		[Comment("#(c:N = 8)#")]
		MOVDDUP_128,
		[Comment("#(c:N = 32)#")]
		MOVDDUP_256,
		[Comment("#(c:N = 64)#")]
		MOVDDUP_512,
	}
}
