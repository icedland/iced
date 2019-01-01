/*
    Copyright (C) 2018-2019 de4dot@gmail.com

    This file is part of Iced.

    Iced is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Iced is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Iced.  If not, see <https://www.gnu.org/licenses/>.
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
		};
	}
}
#endif
