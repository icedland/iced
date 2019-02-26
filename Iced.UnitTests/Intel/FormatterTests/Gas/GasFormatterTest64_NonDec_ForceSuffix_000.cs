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

#if !NO_GAS_FORMATTER && !NO_FORMATTER && !NO_ENCODER
using System.Collections.Generic;
using Iced.Intel;
using Iced.UnitTests.Intel.DecoderTests;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests.Gas {
	public sealed class GasFormatterTest64_NonDec_ForceSuffix_000 : FormatterTest {
		[Theory]
		[MemberData(nameof(Format_Data))]
		void Format(int index, Instruction info, string formattedString) => FormatBase(index, info, formattedString, GasFormatterFactory.Create_ForceSuffix());
		public static IEnumerable<object[]> Format_Data => GetFormatData(NonDecodedInstructions.Infos64, formattedStrings);

		static readonly string[] formattedStrings = new string[NonDecodedInstructions.Infos64_Count] {
			"fstenv (rax)",
			"fstenv fs:(rax)",
			"fstenv (rax)",
			"fstenv fs:(rax)",
			"fstcw (rax)",
			"fstcw fs:(rax)",
			"feni",
			"fdisi",
			"fclex",
			"finit",
			"fsetpm",
			"fsave (rax)",
			"fsave fs:(rax)",
			"fsave (rax)",
			"fsave fs:(rax)",
			"fstsw (rax)",
			"fstsw fs:(rax)",
			"fstsw ax",
			".byte 0x77",
			".byte 0x77,-0x57",
			".byte 0x77,-0x57,-0x32",
			".byte 0x77,-0x57,-0x32,-0x63",
			".byte 0x77,-0x57,-0x32,-0x63,0x55",
			".byte 0x77,-0x57,-0x32,-0x63,0x55,5",
			".byte 0x77,-0x57,-0x32,-0x63,0x55,5,0x42",
			".byte 0x77,-0x57,-0x32,-0x63,0x55,5,0x42,0x6c",
			".byte 0x77,-0x57,-0x32,-0x63,0x55,5,0x42,0x6c,-0x7a",
			".byte 0x77,-0x57,-0x32,-0x63,0x55,5,0x42,0x6c,-0x7a,0x32",
			".byte 0x77,-0x57,-0x32,-0x63,0x55,5,0x42,0x6c,-0x7a,0x32,-2",
			".byte 0x77,-0x57,-0x32,-0x63,0x55,5,0x42,0x6c,-0x7a,0x32,-2,0x4f",
			".byte 0x77,-0x57,-0x32,-0x63,0x55,5,0x42,0x6c,-0x7a,0x32,-2,0x4f,0x34",
			".byte 0x77,-0x57,-0x32,-0x63,0x55,5,0x42,0x6c,-0x7a,0x32,-2,0x4f,0x34,0x27",
			".byte 0x77,-0x57,-0x32,-0x63,0x55,5,0x42,0x6c,-0x7a,0x32,-2,0x4f,0x34,0x27,-0x56",
			".byte 0x77,-0x57,-0x32,-0x63,0x55,5,0x42,0x6c,-0x7a,0x32,-2,0x4f,0x34,0x27,-0x56,8",
			".word 0x77a9",
			".word 0x77a9,-0x3163",
			".word 0x77a9,-0x3163,0x5505",
			".word 0x77a9,-0x3163,0x5505,0x426c",
			".word 0x77a9,-0x3163,0x5505,0x426c,-0x79ce",
			".word 0x77a9,-0x3163,0x5505,0x426c,-0x79ce,-0x1b1",
			".word 0x77a9,-0x3163,0x5505,0x426c,-0x79ce,-0x1b1,0x3427",
			".word 0x77a9,-0x3163,0x5505,0x426c,-0x79ce,-0x1b1,0x3427,-0x55f8",
			".int 0x77a9ce9d",
			".int 0x77a9ce9d,0x5505426c",
			".int 0x77a9ce9d,0x5505426c,-0x79cd01b1",
			".int 0x77a9ce9d,0x5505426c,-0x79cd01b1,0x3427aa08",
			".quad 0x77a9ce9d5505426c",
			".quad 0x77a9ce9d5505426c,-0x79cd01b0cbd855f8",
		};
	}
}
#endif
