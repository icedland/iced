// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

#if GAS
using System.Collections.Generic;
using Iced.Intel;
using Iced.UnitTests.Intel.DecoderTests;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests.Gas {
	public sealed class FormatterTest16 : FormatterTest {
		[Theory]
		[MemberData(nameof(Format_Data_ForceSuffix))]
		void Format_ForceSuffix(int index, InstructionInfo info, string formattedString) => FormatBase(index, info, formattedString, FormatterFactory.Create_ForceSuffix());
		public static IEnumerable<object[]> Format_Data_ForceSuffix => FormatterTestCases.GetFormatData(16, "Gas", "ForceSuffix");

		[Theory]
		[MemberData(nameof(Format_Data_NoSuffix))]
		void Format_NoSuffix(int index, InstructionInfo info, string formattedString) => FormatBase(index, info, formattedString, FormatterFactory.Create_NoSuffix());
		public static IEnumerable<object[]> Format_Data_NoSuffix => FormatterTestCases.GetFormatData(16, "Gas", "NoSuffix");

#if ENCODER
		[Theory]
		[MemberData(nameof(Format_Data_NonDec_ForceSuffix))]
		void Format_NonDec_ForceSuffix(int index, Instruction info, string formattedString) => FormatBase(index, info, formattedString, FormatterFactory.Create_ForceSuffix());
		public static IEnumerable<object[]> Format_Data_NonDec_ForceSuffix => FormatterTestCases.GetFormatData(16, NonDecodedInstructions.Infos16, "Gas", "NonDec_ForceSuffix");

		[Theory]
		[MemberData(nameof(Format_Data_NonDec_NoSuffix))]
		void Format_NonDec_NoSuffix(int index, Instruction info, string formattedString) => FormatBase(index, info, formattedString, FormatterFactory.Create_NoSuffix());
		public static IEnumerable<object[]> Format_Data_NonDec_NoSuffix => FormatterTestCases.GetFormatData(16, NonDecodedInstructions.Infos16, "Gas", "NonDec_NoSuffix");
#endif

		[Theory]
		[MemberData(nameof(Format_Data_Misc))]
		void Format_Misc(int index, InstructionInfo info, string formattedString) => FormatBase(index, info, formattedString, FormatterFactory.Create());
		public static IEnumerable<object[]> Format_Data_Misc => FormatterTestCases.GetFormatData(16, "Gas", "Misc", isMisc: true);
	}
}
#endif
