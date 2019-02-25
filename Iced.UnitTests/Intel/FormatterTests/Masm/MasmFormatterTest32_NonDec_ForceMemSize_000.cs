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

#if !NO_MASM_FORMATTER && !NO_FORMATTER && !NO_ENCODER
using System.Collections.Generic;
using Iced.Intel;
using Iced.UnitTests.Intel.DecoderTests;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests.Masm {
	public sealed class MasmFormatterTest32_NonDec_ForceMemSize_000 : FormatterTest {
		[Theory]
		[MemberData(nameof(Format_Data))]
		void Format(int index, Instruction info, string formattedString) => FormatBase(index, info, formattedString, MasmFormatterFactory.Create_ForceMemSize());
		public static IEnumerable<object[]> Format_Data => GetFormatData(NonDecodedInstructions.Infos32, formattedStrings);

		static readonly string[] formattedStrings = new string[NonDecodedInstructions.Infos32_Count] {
			"popw cs",
			"fstenv [eax]",
			"fstenv fs:[eax]",
			"fstenv [eax]",
			"fstenv fs:[eax]",
			"fstcw word ptr [eax]",
			"fstcw word ptr fs:[eax]",
			"feni",
			"fdisi",
			"fclex",
			"finit",
			"fsetpm",
			"fsave [eax]",
			"fsave fs:[eax]",
			"fsave [eax]",
			"fsave fs:[eax]",
			"fstsw word ptr [eax]",
			"fstsw word ptr fs:[eax]",
			"fstsw ax",
			"db",
			"db 77h",
			"db 77h,-57h",
			"db 77h,-57h,-32h",
			"db 77h,-57h,-32h,-63h",
			"db 77h,-57h,-32h,-63h,55h",
			"db 77h,-57h,-32h,-63h,55h,5",
			"db 77h,-57h,-32h,-63h,55h,5,42h",
			"db 77h,-57h,-32h,-63h,55h,5,42h,6Ch",
			"db 77h,-57h,-32h,-63h,55h,5,42h,6Ch,-7Ah",
			"db 77h,-57h,-32h,-63h,55h,5,42h,6Ch,-7Ah,32h",
			"db 77h,-57h,-32h,-63h,55h,5,42h,6Ch,-7Ah,32h,-2",
			"db 77h,-57h,-32h,-63h,55h,5,42h,6Ch,-7Ah,32h,-2,4Fh",
			"db 77h,-57h,-32h,-63h,55h,5,42h,6Ch,-7Ah,32h,-2,4Fh,34h",
			"db 77h,-57h,-32h,-63h,55h,5,42h,6Ch,-7Ah,32h,-2,4Fh,34h,27h",
			"db 77h,-57h,-32h,-63h,55h,5,42h,6Ch,-7Ah,32h,-2,4Fh,34h,27h,-56h",
			"db 77h,-57h,-32h,-63h,55h,5,42h,6Ch,-7Ah,32h,-2,4Fh,34h,27h,-56h,8",
			"dw",
			"dw 77A9h",
			"dw 77A9h,-3163h",
			"dw 77A9h,-3163h,5505h",
			"dw 77A9h,-3163h,5505h,426Ch",
			"dw 77A9h,-3163h,5505h,426Ch,-79CEh",
			"dw 77A9h,-3163h,5505h,426Ch,-79CEh,-1B1h",
			"dw 77A9h,-3163h,5505h,426Ch,-79CEh,-1B1h,3427h",
			"dw 77A9h,-3163h,5505h,426Ch,-79CEh,-1B1h,3427h,-55F8h",
			"dd",
			"dd 77A9CE9Dh",
			"dd 77A9CE9Dh,5505426Ch",
			"dd 77A9CE9Dh,5505426Ch,-79CD01B1h",
			"dd 77A9CE9Dh,5505426Ch,-79CD01B1h,3427AA08h",
			"dq",
			"dq 77A9CE9D5505426Ch",
			"dq 77A9CE9D5505426Ch,-79CD01B0CBD855F8h",
		};
	}
}
#endif
