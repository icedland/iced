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

#if ((!NO_DECODER32 || !NO_DECODER64) && !NO_DECODER) || !NO_ENCODER
namespace Iced.Intel {
	enum TupleType : byte {
		None,
		Full_128,
		Full_256,
		Full_512,
		Half_128,
		Half_256,
		Half_512,
		Full_Mem_128,
		Full_Mem_256,
		Full_Mem_512,
		Tuple1_Scalar,
		Tuple1_Scalar_1,
		Tuple1_Scalar_2,
		Tuple1_Scalar_4,
		Tuple1_Scalar_8,
		Tuple1_Fixed,
		Tuple1_Fixed_4,
		Tuple1_Fixed_8,
		Tuple2,
		Tuple4,
		Tuple8,
		Tuple1_4X,
		Half_Mem_128,
		Half_Mem_256,
		Half_Mem_512,
		Quarter_Mem_128,
		Quarter_Mem_256,
		Quarter_Mem_512,
		Eighth_Mem_128,
		Eighth_Mem_256,
		Eighth_Mem_512,
		Mem128,
		MOVDDUP_128,
		MOVDDUP_256,
		MOVDDUP_512,
	}
}
#endif
