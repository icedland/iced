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

#if !NO_INTEL_FORMATTER && !NO_FORMATTER
using System.Collections.Generic;
using Iced.Intel;
using Iced.UnitTests.Intel.DecoderTests;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests.Intel {
	public sealed class FormatterTest32 : FormatterTest {
		[Theory]
		[MemberData(nameof(Format_Data_MemAlways))]
		void Format_MemAlways(int index, InstructionInfo info, string formattedString) => FormatBase(index, info, formattedString, FormatterFactory.Create_MemAlways());
		public static IEnumerable<object[]> Format_Data_MemAlways => GetFormatData(32, "Intel", "MemAlways");

		[Theory]
		[MemberData(nameof(Format_Data_MemDefault))]
		void Format_MemDefault(int index, InstructionInfo info, string formattedString) => FormatBase(index, info, formattedString, FormatterFactory.Create_MemDefault());
		public static IEnumerable<object[]> Format_Data_MemDefault => GetFormatData(32, "Intel", "MemDefault");

		[Theory]
		[MemberData(nameof(Format_Data_MemMinimum))]
		void Format_MemMinimum(int index, InstructionInfo info, string formattedString) => FormatBase(index, info, formattedString, FormatterFactory.Create_MemMinimum());
		public static IEnumerable<object[]> Format_Data_MemMinimum => GetFormatData(32, "Intel", "MemMinimum");

#if !NO_ENCODER
		[Theory]
		[MemberData(nameof(Format_Data_NonDec_MemAlways))]
		void Format_NonDec_MemAlways(int index, Instruction info, string formattedString) => FormatBase(index, info, formattedString, FormatterFactory.Create_MemAlways());
		public static IEnumerable<object[]> Format_Data_NonDec_MemAlways => GetFormatData(32, NonDecodedInstructions.Infos32, "Intel", "NonDec_MemAlways");

		[Theory]
		[MemberData(nameof(Format_Data_NonDec_MemDefault))]
		void Format_NonDec_MemDefault(int index, Instruction info, string formattedString) => FormatBase(index, info, formattedString, FormatterFactory.Create_MemDefault());
		public static IEnumerable<object[]> Format_Data_NonDec_MemDefault => GetFormatData(32, NonDecodedInstructions.Infos32, "Intel", "NonDec_MemDefault");

		[Theory]
		[MemberData(nameof(Format_Data_NonDec_MemMinimum))]
		void Format_NonDec_MemMinimum(int index, Instruction info, string formattedString) => FormatBase(index, info, formattedString, FormatterFactory.Create_MemMinimum());
		public static IEnumerable<object[]> Format_Data_NonDec_MemMinimum => GetFormatData(32, NonDecodedInstructions.Infos32, "Intel", "NonDec_MemMinimum");
#endif

		[Theory]
		[MemberData(nameof(Format_Data_Misc))]
		void Format_Misc(int index, InstructionInfo info, string formattedString) => FormatBase(index, info, formattedString, FormatterFactory.Create());
		public static IEnumerable<object[]> Format_Data_Misc => GetFormatData(32, "Intel", "Misc", isMisc: true);
	}
}
#endif
