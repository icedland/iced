// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if FAST_FMT
using System.Collections.Generic;
using Iced.Intel;
using Iced.UnitTests.Intel.DecoderTests;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests.Fast {
	public sealed class FormatterTest32 : FastFormatterTest {
		[Theory]
		[MemberData(nameof(Format_Data_Default))]
		void Format_Default(int index, FormatterTestCase tc, string formattedString) => FormatBase(index, tc, formattedString, FormatterFactory.Create_Default());
		public static IEnumerable<object[]> Format_Data_Default => FormatterTestCases.GetFormatData(32, "Fast", "Default");

		[Theory]
		[MemberData(nameof(Format_Data_Inverted))]
		void Format_Inverted(int index, FormatterTestCase tc, string formattedString) => FormatBase(index, tc, formattedString, FormatterFactory.Create_Inverted());
		public static IEnumerable<object[]> Format_Data_Inverted => FormatterTestCases.GetFormatData(32, "Fast", "Inverted");


#if ENCODER
		[Theory]
		[MemberData(nameof(Format_Data_NonDec_Default))]
		void Format_NonDec_Default(int index, Instruction instr, string formattedString) => FormatBase(index, instr, formattedString, FormatterFactory.Create_Default());
		public static IEnumerable<object[]> Format_Data_NonDec_Default => FormatterTestCases.GetFormatData(32, NonDecodedInstructions.Infos32, "Fast", "NonDec_Default");

		[Theory]
		[MemberData(nameof(Format_Data_NonDec_Inverted))]
		void Format_NonDec_Inverted(int index, Instruction instr, string formattedString) => FormatBase(index, instr, formattedString, FormatterFactory.Create_Inverted());
		public static IEnumerable<object[]> Format_Data_NonDec_Inverted => FormatterTestCases.GetFormatData(32, NonDecodedInstructions.Infos32, "Fast", "NonDec_Inverted");
#endif

		[Theory]
		[MemberData(nameof(Format_Data_Misc))]
		void Format_Misc(int index, FormatterTestCase tc, string formattedString) => FormatBase(index, tc, formattedString, FormatterFactory.Create_Default());
		public static IEnumerable<object[]> Format_Data_Misc => FormatterTestCases.GetFormatData(32, "Fast", "Misc", isMisc: true);
	}
}
#endif
