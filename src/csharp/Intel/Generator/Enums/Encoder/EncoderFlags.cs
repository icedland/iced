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

namespace Generator.Enums.Encoder {
	[Enum("EncoderFlags", Flags = true, NoInitialize = true)]
	[Flags]
	enum EncoderFlags : uint {
		None				= 0,
		B					= 0x00000001,
		X					= 0x00000002,
		R					= 0x00000004,
		W					= 0x00000008,

		ModRM				= 0x00000010,
		Sib					= 0x00000020,
		REX					= 0x00000040,
		P66					= 0x00000080,
		P67					= 0x00000100,

		[Comment("#(c:EVEX.R')#")]
		R2					= 0x00000200,
		Broadcast			= 0x00000400,
		HighLegacy8BitRegs	= 0x00000800,
		Displ				= 0x00001000,
		PF0					= 0x00002000,
		RegIsMemory			= 0x00004000,
		MustUseSib			= 0x00008000,

		VvvvvShift			= 27,// 5 bits
		VvvvvMask			= 0x1F,
	}

	[TypeGen(TypeGenOrders.NoDeps)]
	sealed class EncoderFlagsEnum {
		EncoderFlagsEnum(GenTypes genTypes) {
			if ((uint)EncoderFlags.VvvvvShift + 5 > 32)
				throw new InvalidOperationException();
		}
	}
}
