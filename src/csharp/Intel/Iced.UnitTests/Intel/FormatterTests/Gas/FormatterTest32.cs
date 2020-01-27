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

#if !NO_GAS
using System.Collections.Generic;
using Iced.Intel;
using Iced.UnitTests.Intel.DecoderTests;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests.Gas {
	public sealed class FormatterTest32 : FormatterTest {
		[Theory]
		[MemberData(nameof(Format_Data_ForceSuffix))]
		void Format_ForceSuffix(int index, InstructionInfo info, string formattedString) => FormatBase(index, info, formattedString, FormatterFactory.Create_ForceSuffix());
		public static IEnumerable<object[]> Format_Data_ForceSuffix => GetFormatData(32, "Gas", "ForceSuffix");

		[Theory]
		[MemberData(nameof(Format_Data_NoSuffix))]
		void Format_NoSuffix(int index, InstructionInfo info, string formattedString) => FormatBase(index, info, formattedString, FormatterFactory.Create_NoSuffix());
		public static IEnumerable<object[]> Format_Data_NoSuffix => GetFormatData(32, "Gas", "NoSuffix");

#if !NO_ENCODER
		[Theory]
		[MemberData(nameof(Format_Data_NonDec_ForceSuffix))]
		void Format_NonDec_ForceSuffix(int index, Instruction info, string formattedString) => FormatBase(index, info, formattedString, FormatterFactory.Create_ForceSuffix());
		public static IEnumerable<object[]> Format_Data_NonDec_ForceSuffix => GetFormatData(32, NonDecodedInstructions.Infos32, "Gas", "NonDec_ForceSuffix");

		[Theory]
		[MemberData(nameof(Format_Data_NonDec_NoSuffix))]
		void Format_NonDec_NoSuffix(int index, Instruction info, string formattedString) => FormatBase(index, info, formattedString, FormatterFactory.Create_NoSuffix());
		public static IEnumerable<object[]> Format_Data_NonDec_NoSuffix => GetFormatData(32, NonDecodedInstructions.Infos32, "Gas", "NonDec_NoSuffix");
#endif

		[Theory]
		[MemberData(nameof(Format_Data_Misc))]
		void Format_Misc(int index, InstructionInfo info, string formattedString) => FormatBase(index, info, formattedString, FormatterFactory.Create());
		public static IEnumerable<object[]> Format_Data_Misc => GetFormatData(32, "Gas", "Misc", isMisc: true);
	}
}
#endif
