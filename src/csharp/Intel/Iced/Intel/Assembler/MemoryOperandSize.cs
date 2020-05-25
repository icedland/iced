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

#if ENCODER && BLOCK_ENCODER && CODE_ASSEMBLER
namespace Iced.Intel {
	/// <summary>
	/// Memory operand size.
	/// </summary>
	enum MemoryOperandSize {
		/// <summary>
		/// An unspecified memory operand.
		/// </summary>
		None,
		/// <summary>
		/// A 8-bit / byte pointer.
		/// </summary>
		BytePtr,
		/// <summary>
		/// A 16-bit / word pointer.
		/// </summary>
		WordPtr,
		/// <summary>
		/// A 32-bit / double-word pointer.
		/// </summary>
		DwordPtr,
		/// <summary>
		/// A 64-bit / quad word pointer.
		/// </summary>
		QwordPtr,
		/// <summary>
		/// A 80-bit / tword pointer.
		/// </summary>
		TwordPtr,
		/// <summary>
		/// A 16-bit segment + 32-bit address.
		/// </summary>
		FwordPtr,
		/// <summary>
		/// A 128-bit / xmm pointer.
		/// </summary>
		OwordPtr,
		/// <summary>
		/// A 256-bit / ymm pointer.
		/// </summary>
		YwordPtr,
		/// <summary>
		/// A 512-bit / zmm pointer.
		/// </summary>
		ZwordPtr,
	}
}
#endif
