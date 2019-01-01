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

#if !NO_NASM_FORMATTER && !NO_FORMATTER && !NO_ENCODER
using System.Collections.Generic;
using Iced.Intel;
using Iced.UnitTests.Intel.DecoderTests;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests.Nasm {
	public sealed class NasmFormatterTest16_NonDec_ForceMemSize_000 : FormatterTest {
		[Theory]
		[MemberData(nameof(Format_Data))]
		void Format(int index, Instruction info, string formattedString) => FormatBase(index, info, formattedString, NasmFormatterFactory.Create_ForceMemSize());
		public static IEnumerable<object[]> Format_Data => GetFormatData(NonDecodedInstructions.Infos16, formattedStrings);

		static readonly string[] formattedStrings = new string[NonDecodedInstructions.Infos16_Count] {
			"pop cs",
			"fstenv [bx+si]",
			"fstenv [fs:bx+si]",
			"fstenv [bx+si]",
			"fstenv [fs:bx+si]",
			"fstcw word [bx+si]",
			"fstcw word [fs:bx+si]",
			"feni",
			"fdisi",
			"fclex",
			"finit",
			"fsetpm",
			"fsave [bx+si]",
			"fsave [fs:bx+si]",
			"fsave [bx+si]",
			"fsave [fs:bx+si]",
			"fstsw word [bx+si]",
			"fstsw word [fs:bx+si]",
			"fstsw ax",
		};
	}
}
#endif
