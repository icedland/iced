// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if GAS
using System.Collections.Generic;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests.Gas {
	public sealed class RegisterTests : FormatterTests.RegisterTests {
		[Theory]
		[MemberData(nameof(Format_Data))]
		void Format(Register register, string formattedString) => FormatBase(register, formattedString, FormatterFactory.Create_Registers(nakedRegisters: false));
		public static IEnumerable<object[]> Format_Data => GetFormatData("Gas", "RegisterTests_1");

		[Theory]
		[MemberData(nameof(Format2_Data))]
		void Format2(Register register, string formattedString) => FormatBase(register, formattedString, FormatterFactory.Create_Registers(nakedRegisters: true));
		public static IEnumerable<object[]> Format2_Data => GetFormatData("Gas", "RegisterTests_2");
	}
}
#endif
