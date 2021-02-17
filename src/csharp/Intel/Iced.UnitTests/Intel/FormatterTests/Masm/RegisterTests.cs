// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if MASM
using System.Collections.Generic;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests.Masm {
	public sealed class RegisterTests : FormatterTests.RegisterTests {
		[Theory]
		[MemberData(nameof(Format_Data))]
		void Format(Register register, string formattedString) => FormatBase(register, formattedString, FormatterFactory.Create_Registers());
		public static IEnumerable<object[]> Format_Data => GetFormatData("Masm", "RegisterTests");
	}
}
#endif
