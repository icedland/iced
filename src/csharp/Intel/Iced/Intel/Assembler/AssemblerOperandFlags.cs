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

namespace Iced.Intel
{
	/// <summary>
	/// Assembler operand flags.
	/// </summary>
	[Flags]
	public enum AssemblerOperandFlags {
		/// <summary>
		/// No flags.
		/// </summary>
		None = 0,
		/// <summary>
		/// Broadcast.
		/// </summary>
		Broadcast = 1,
		/// <summary>
		/// Zeroing mask.
		/// </summary>
		Zeroing = 1 << 1,
		/// <summary>
		/// Mask register K0
		/// </summary>
		K0 = 1 << 2,
		/// <summary>
		/// Mask register K1.
		/// </summary>
		K1 = 2 << 2,
		/// <summary>
		/// Mask register K2.
		/// </summary>
		K2 = 3 << 2,
		/// <summary>
		/// Mask register K3.
		/// </summary>
		K3 = 4 << 2,
		/// <summary>
		/// Mask register K4.
		/// </summary>
		K4 = 5 << 2,
		/// <summary>
		/// Mask register K5.
		/// </summary>
		K5 = 6 << 2,
		/// <summary>
		/// Mask register K6.
		/// </summary>
		K6 = 7 << 2,
		/// <summary>
		/// Mask register K7.
		/// </summary>
		K7 = 8 << 2,
		/// <summary>
		/// Mask for K registers.
		/// </summary>
		RegisterMask = 0xF << 2,
	}
}
