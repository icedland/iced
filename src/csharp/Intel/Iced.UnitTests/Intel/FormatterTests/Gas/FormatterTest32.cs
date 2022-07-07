// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if GAS
using System.Collections.Generic;
using Iced.Intel;
using Iced.UnitTests.Intel.DecoderTests;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests.Gas {
	public sealed class FormatterTest32 : FormatterTest {
		[Theory]
		[MemberData(nameof(Format_Data_ForceSuffix))]
		void Format_ForceSuffix(int index, FormatterTestCase tc, string formattedString) => FormatBase(index, tc, formattedString, FormatterFactory.Create_ForceSuffix());
		public static IEnumerable<object[]> Format_Data_ForceSuffix => FormatterTestCases.GetFormatData(32, "Gas", "ForceSuffix");

		[Theory]
		[MemberData(nameof(Format_Data_NoSuffix))]
		void Format_NoSuffix(int index, FormatterTestCase tc, string formattedString) => FormatBase(index, tc, formattedString, FormatterFactory.Create_NoSuffix());
		public static IEnumerable<object[]> Format_Data_NoSuffix => FormatterTestCases.GetFormatData(32, "Gas", "NoSuffix");

#if ENCODER
		[Theory]
		[MemberData(nameof(Format_Data_NonDec_ForceSuffix))]
		void Format_NonDec_ForceSuffix(int index, Instruction instr, string formattedString) => FormatBase(index, instr, formattedString, FormatterFactory.Create_ForceSuffix());
		public static IEnumerable<object[]> Format_Data_NonDec_ForceSuffix => FormatterTestCases.GetFormatData(32, NonDecodedInstructions.Infos32, "Gas", "NonDec_ForceSuffix");

		[Theory]
		[MemberData(nameof(Format_Data_NonDec_NoSuffix))]
		void Format_NonDec_NoSuffix(int index, Instruction instr, string formattedString) => FormatBase(index, instr, formattedString, FormatterFactory.Create_NoSuffix());
		public static IEnumerable<object[]> Format_Data_NonDec_NoSuffix => FormatterTestCases.GetFormatData(32, NonDecodedInstructions.Infos32, "Gas", "NonDec_NoSuffix");
#endif

		[Theory]
		[MemberData(nameof(Format_Data_Misc))]
		void Format_Misc(int index, FormatterTestCase tc, string formattedString) => FormatBase(index, tc, formattedString, FormatterFactory.Create());
		public static IEnumerable<object[]> Format_Data_Misc => FormatterTestCases.GetFormatData(32, "Gas", "Misc", isMisc: true);
	}
}
#endif
