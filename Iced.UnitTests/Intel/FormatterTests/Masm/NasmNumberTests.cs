/*
    Copyright (C) 2018 de4dot@gmail.com

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

#if !NO_MASM_FORMATTER && !NO_FORMATTER
using System.Collections.Generic;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests.Masm {
	public sealed class MasmNumberTests : NumberTests {
		[Theory]
		[MemberData(nameof(Format_Data))]
		void Format(int index, object number, string formattedString) => FormatBase(index, number, formattedString, MasmFormatterFactory.Create_Numbers());
		public static IEnumerable<object[]> Format_Data => GetFormatData(allNumbers, formattedStrings);

		static readonly string[] formattedStrings = new string[allNumbersCount] {
			"-80h",
			"7Fh",
			"-8000h",
			"7FFFh",
			"-80000000h",
			"7FFFFFFFh",
			"-8000000000000000h",
			"7FFFFFFFFFFFFFFFh",
			"0FFh",
			"0FFFFh",
			"0FFFFFFFFh",
			"0FFFFFFFFFFFFFFFFh",
		};
	}
}
#endif
