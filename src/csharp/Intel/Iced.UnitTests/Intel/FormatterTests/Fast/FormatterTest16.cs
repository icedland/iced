// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

#if FAST_FMT
using System.Collections.Generic;
using Iced.Intel;
using Iced.UnitTests.Intel.DecoderTests;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests.Fast {
	public sealed class FormatterTest16 : FastFormatterTest {
		[Theory]
		[MemberData(nameof(Format_Data_Default))]
		void Format_Default(int index, InstructionInfo info, string formattedString) => FormatBase(index, info, formattedString, FormatterFactory.Create_Default());
		public static IEnumerable<object[]> Format_Data_Default => FormatterTestCases.GetFormatData(16, "Fast", "Default");

		[Theory]
		[MemberData(nameof(Format_Data_Inverted))]
		void Format_Inverted(int index, InstructionInfo info, string formattedString) => FormatBase(index, info, formattedString, FormatterFactory.Create_Inverted());
		public static IEnumerable<object[]> Format_Data_Inverted => FormatterTestCases.GetFormatData(16, "Fast", "Inverted");

#if ENCODER
		[Theory]
		[MemberData(nameof(Format_Data_NonDec_Default))]
		void Format_NonDec_Default(int index, Instruction info, string formattedString) => FormatBase(index, info, formattedString, FormatterFactory.Create_Default());
		public static IEnumerable<object[]> Format_Data_NonDec_Default => FormatterTestCases.GetFormatData(16, NonDecodedInstructions.Infos16, "Fast", "NonDec_Default");

		[Theory]
		[MemberData(nameof(Format_Data_NonDec_Inverted))]
		void Format_NonDec_Inverted(int index, Instruction info, string formattedString) => FormatBase(index, info, formattedString, FormatterFactory.Create_Inverted());
		public static IEnumerable<object[]> Format_Data_NonDec_Inverted => FormatterTestCases.GetFormatData(16, NonDecodedInstructions.Infos16, "Fast", "NonDec_Inverted");
#endif

		[Theory]
		[MemberData(nameof(Format_Data_Misc))]
		void Format_Misc(int index, InstructionInfo info, string formattedString) => FormatBase(index, info, formattedString, FormatterFactory.Create_Default());
		public static IEnumerable<object[]> Format_Data_Misc => FormatterTestCases.GetFormatData(16, "Fast", "Misc", isMisc: true);
	}
}
#endif
