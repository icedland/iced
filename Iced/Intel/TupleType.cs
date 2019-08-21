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

#if !NO_DECODER || !NO_ENCODER
namespace Iced.Intel {
	/// <summary>
	/// Tuple type (EVEX) which can be used to get the disp8 scale factor N
	/// </summary>
	public enum TupleType {
		/// <summary>
		/// N = 1
		/// </summary>
		None,
		/// <summary>
		/// N = b ? (w ? 8 : 4) : 16
		/// </summary>
		Full_128,
		/// <summary>
		/// N = b ? (w ? 8 : 4) : 32
		/// </summary>
		Full_256,
		/// <summary>
		/// N = b ? (w ? 8 : 4) : 64
		/// </summary>
		Full_512,
		/// <summary>
		/// N = b ? 4 : 8
		/// </summary>
		Half_128,
		/// <summary>
		/// N = b ? 4 : 16
		/// </summary>
		Half_256,
		/// <summary>
		/// N = b ? 4 : 32
		/// </summary>
		Half_512,
		/// <summary>
		/// N = 16
		/// </summary>
		Full_Mem_128,
		/// <summary>
		/// N = 32
		/// </summary>
		Full_Mem_256,
		/// <summary>
		/// N = 64
		/// </summary>
		Full_Mem_512,
		/// <summary>
		/// N = w ? 8 : 4
		/// </summary>
		Tuple1_Scalar,
		/// <summary>
		/// N = 1
		/// </summary>
		Tuple1_Scalar_1,
		/// <summary>
		/// N = 2
		/// </summary>
		Tuple1_Scalar_2,
		/// <summary>
		/// N = 4
		/// </summary>
		Tuple1_Scalar_4,
		/// <summary>
		/// N = 8
		/// </summary>
		Tuple1_Scalar_8,
		/// <summary>
		/// N = w ? 8 : 4
		/// </summary>
		Tuple1_Fixed,
		/// <summary>
		/// N = 4
		/// </summary>
		Tuple1_Fixed_4,
		/// <summary>
		/// N = 8
		/// </summary>
		Tuple1_Fixed_8,
		/// <summary>
		/// N = w ? 16 : 8
		/// </summary>
		Tuple2,
		/// <summary>
		/// N = w ? 32 : 16
		/// </summary>
		Tuple4,
		/// <summary>
		/// N = w ? error : 32
		/// </summary>
		Tuple8,
		/// <summary>
		/// N = 16
		/// </summary>
		Tuple1_4X,
		/// <summary>
		/// N = 8
		/// </summary>
		Half_Mem_128,
		/// <summary>
		/// N = 16
		/// </summary>
		Half_Mem_256,
		/// <summary>
		/// N = 32
		/// </summary>
		Half_Mem_512,
		/// <summary>
		/// N = 4
		/// </summary>
		Quarter_Mem_128,
		/// <summary>
		/// N = 8
		/// </summary>
		Quarter_Mem_256,
		/// <summary>
		/// N = 16
		/// </summary>
		Quarter_Mem_512,
		/// <summary>
		/// N = 2
		/// </summary>
		Eighth_Mem_128,
		/// <summary>
		/// N = 4
		/// </summary>
		Eighth_Mem_256,
		/// <summary>
		/// N = 8
		/// </summary>
		Eighth_Mem_512,
		/// <summary>
		/// N = 16
		/// </summary>
		Mem128,
		/// <summary>
		/// N = 8
		/// </summary>
		MOVDDUP_128,
		/// <summary>
		/// N = 32
		/// </summary>
		MOVDDUP_256,
		/// <summary>
		/// N = 64
		/// </summary>
		MOVDDUP_512,
	}
}
#endif
