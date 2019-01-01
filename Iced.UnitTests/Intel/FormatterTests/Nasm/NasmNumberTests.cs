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

#if !NO_NASM_FORMATTER && !NO_FORMATTER
using System.Collections.Generic;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests.Nasm {
	public sealed class NasmNumberTests : NumberTests {
		[Theory]
		[MemberData(nameof(Format_Data))]
		void Format(int index, object number, string formattedString) => FormatBase(index, number, formattedString, NasmFormatterFactory.Create_Numbers());
		public static IEnumerable<object[]> Format_Data => GetFormatData(allNumbers, formattedStrings);

		static readonly string[] formattedStrings = new string[allNumbersCount] {
			"-0x80",
			"0x7f",
			"-0x8000",
			"0x7fff",
			"-0x80000000",
			"0x7fffffff",
			"-0x8000000000000000",
			"0x7fffffffffffffff",
			"0xff",
			"0xffff",
			"0xffffffff",
			"0xffffffffffffffff",
		};
	}
}
#endif
