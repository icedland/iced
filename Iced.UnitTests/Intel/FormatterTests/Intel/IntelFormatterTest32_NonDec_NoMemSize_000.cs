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
	public sealed class IntelFormatterTest32_NonDec_NoMemSize_000 : FormatterTest {
		[Theory]
		[MemberData(nameof(Format_Data))]
		void Format(int index, Instruction info, string formattedString) => FormatBase(index, info, formattedString, IntelFormatterFactory.Create_NoMemSize());
		public static IEnumerable<object[]> Format_Data => GetFormatData(NonDecodedInstructions.Infos32, formattedStrings);

		static readonly string[] formattedStrings = new string[NonDecodedInstructions.Infos32_Count] {
			"pop cs",
			"fstenv [eax]",
			"fstenv fs:[eax]",
			"fstenv [eax]",
			"fstenv fs:[eax]",
			"fstcw [eax]",
			"fstcw fs:[eax]",
			"feni",
			"fdisi",
			"fclex",
			"finit",
			"fsetpm",
			"fsave [eax]",
			"fsave fs:[eax]",
			"fsave [eax]",
			"fsave fs:[eax]",
			"fstsw [eax]",
			"fstsw fs:[eax]",
			"fstsw ax",
		};
	}
}
#endif
