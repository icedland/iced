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

#if !NO_INTEL_FORMATTER && !NO_FORMATTER && !NO_ENCODER
using System.Collections.Generic;
using Iced.Intel;
using Iced.UnitTests.Intel.DecoderTests;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests.Intel {
	public sealed class IntelFormatterTest16_NonDec_NoMemSize_000 : FormatterTest {
		[Theory]
		[MemberData(nameof(Format_Data))]
		void Format(int index, Instruction info, string formattedString) => FormatBase(index, info, formattedString, IntelFormatterFactory.Create_NoMemSize());
		public static IEnumerable<object[]> Format_Data => GetFormatData(NonDecodedInstructions.Infos16, formattedStrings);

		static readonly string[] formattedStrings = new string[NonDecodedInstructions.Infos16_Count] {
			"pop cs",
			"fstenv [bx+si]",
			"fstenv fs:[bx+si]",
			"fstenv [bx+si]",
			"fstenv fs:[bx+si]",
			"fstcw [bx+si]",
			"fstcw fs:[bx+si]",
			"feni",
			"fdisi",
			"fclex",
			"finit",
			"fsetpm",
			"fsave [bx+si]",
			"fsave fs:[bx+si]",
			"fsave [bx+si]",
			"fsave fs:[bx+si]",
			"fstsw [bx+si]",
			"fstsw fs:[bx+si]",
			"fstsw ax",
			"db",
			"db 0x77",
			"db 0x77, 0xa9",
			"db 0x77, 0xa9, 0xce",
			"db 0x77, 0xa9, 0xce, 0x9d",
			"db 0x77, 0xa9, 0xce, 0x9d, 0x55",
			"db 0x77, 0xa9, 0xce, 0x9d, 0x55, 5",
			"db 0x77, 0xa9, 0xce, 0x9d, 0x55, 5, 0x42",
			"db 0x77, 0xa9, 0xce, 0x9d, 0x55, 5, 0x42, 0x6c",
			"db 0x77, 0xa9, 0xce, 0x9d, 0x55, 5, 0x42, 0x6c, 0x86",
			"db 0x77, 0xa9, 0xce, 0x9d, 0x55, 5, 0x42, 0x6c, 0x86, 0x32",
			"db 0x77, 0xa9, 0xce, 0x9d, 0x55, 5, 0x42, 0x6c, 0x86, 0x32, 0xfe",
			"db 0x77, 0xa9, 0xce, 0x9d, 0x55, 5, 0x42, 0x6c, 0x86, 0x32, 0xfe, 0x4f",
			"db 0x77, 0xa9, 0xce, 0x9d, 0x55, 5, 0x42, 0x6c, 0x86, 0x32, 0xfe, 0x4f, 0x34",
			"db 0x77, 0xa9, 0xce, 0x9d, 0x55, 5, 0x42, 0x6c, 0x86, 0x32, 0xfe, 0x4f, 0x34, 0x27",
			"db 0x77, 0xa9, 0xce, 0x9d, 0x55, 5, 0x42, 0x6c, 0x86, 0x32, 0xfe, 0x4f, 0x34, 0x27, 0xaa",
			"db 0x77, 0xa9, 0xce, 0x9d, 0x55, 5, 0x42, 0x6c, 0x86, 0x32, 0xfe, 0x4f, 0x34, 0x27, 0xaa, 8",
			"dw",
			"dw 0x77a9",
			"dw 0x77a9, 0xce9d",
			"dw 0x77a9, 0xce9d, 0x5505",
			"dw 0x77a9, 0xce9d, 0x5505, 0x426c",
			"dw 0x77a9, 0xce9d, 0x5505, 0x426c, 0x8632",
			"dw 0x77a9, 0xce9d, 0x5505, 0x426c, 0x8632, 0xfe4f",
			"dw 0x77a9, 0xce9d, 0x5505, 0x426c, 0x8632, 0xfe4f, 0x3427",
			"dw 0x77a9, 0xce9d, 0x5505, 0x426c, 0x8632, 0xfe4f, 0x3427, 0xaa08",
			"dd",
			"dd 0x77a9ce9d",
			"dd 0x77a9ce9d, 0x5505426c",
			"dd 0x77a9ce9d, 0x5505426c, 0x8632fe4f",
			"dd 0x77a9ce9d, 0x5505426c, 0x8632fe4f, 0x3427aa08",
			"dq",
			"dq 0x77a9ce9d5505426c",
			"dq 0x77a9ce9d5505426c, 0x8632fe4f3427aa08",
		};
	}
}
#endif
