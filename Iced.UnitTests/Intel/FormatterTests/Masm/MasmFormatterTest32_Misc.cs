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
	public sealed class MasmFormatterTest32_Misc : FormatterTest {
		[Theory]
		[MemberData(nameof(Format_Data))]
		void Format(int index, InstructionInfo info, string formattedString) => FormatBase(index, info, formattedString, MasmFormatterFactory.Create());
		public static IEnumerable<object[]> Format_Data => GetFormatData(infos, formattedStrings);

		static readonly InstructionInfo[] infos = InstructionInfos32_Misc.AllInfos;
		static readonly string[] formattedStrings = new string[InstructionInfos32_Misc.AllInfos_Length] {
			"hnt jo short 7FFFFFF3h",
			"hnt jno short 7FFFFFF3h",
			"hnt jb short 7FFFFFF3h",
			"hnt jae short 7FFFFFF3h",
			"hnt je short 7FFFFFF3h",
			"hnt jne short 7FFFFFF3h",
			"hnt jbe short 7FFFFFF3h",
			"hnt ja short 7FFFFFF3h",
			"hnt js short 7FFFFFF3h",
			"hnt jns short 7FFFFFF3h",
			"hnt jp short 7FFFFFF3h",
			"hnt jnp short 7FFFFFF3h",
			"hnt jl short 7FFFFFF3h",
			"hnt jge short 7FFFFFF3h",
			"hnt jle short 7FFFFFF3h",
			"hnt jg short 7FFFFFF3h",
			"ht jo short 7FFFFFF3h",
			"ht jno short 7FFFFFF3h",
			"ht jb short 7FFFFFF3h",
			"ht jae short 7FFFFFF3h",
			"ht je short 7FFFFFF3h",
			"ht jne short 7FFFFFF3h",
			"ht jbe short 7FFFFFF3h",
			"ht ja short 7FFFFFF3h",
			"ht js short 7FFFFFF3h",
			"ht jns short 7FFFFFF3h",
			"ht jp short 7FFFFFF3h",
			"ht jnp short 7FFFFFF3h",
			"ht jl short 7FFFFFF3h",
			"ht jge short 7FFFFFF3h",
			"ht jle short 7FFFFFF3h",
			"ht jg short 7FFFFFF3h",
			"hnt jo near ptr 7FFFFFF7h",
			"hnt jno near ptr 7FFFFFF7h",
			"hnt jb near ptr 7FFFFFF7h",
			"hnt jae near ptr 7FFFFFF7h",
			"hnt je near ptr 7FFFFFF7h",
			"hnt jne near ptr 7FFFFFF7h",
			"hnt jbe near ptr 7FFFFFF7h",
			"hnt ja near ptr 7FFFFFF7h",
			"hnt js near ptr 7FFFFFF7h",
			"hnt jns near ptr 7FFFFFF7h",
			"hnt jp near ptr 7FFFFFF7h",
			"hnt jnp near ptr 7FFFFFF7h",
			"hnt jl near ptr 7FFFFFF7h",
			"hnt jge near ptr 7FFFFFF7h",
			"hnt jle near ptr 7FFFFFF7h",
			"hnt jg near ptr 7FFFFFF7h",
			"ht jo near ptr 7FFFFFF7h",
			"ht jno near ptr 7FFFFFF7h",
			"ht jb near ptr 7FFFFFF7h",
			"ht jae near ptr 7FFFFFF7h",
			"ht je near ptr 7FFFFFF7h",
			"ht jne near ptr 7FFFFFF7h",
			"ht jbe near ptr 7FFFFFF7h",
			"ht ja near ptr 7FFFFFF7h",
			"ht js near ptr 7FFFFFF7h",
			"ht jns near ptr 7FFFFFF7h",
			"ht jp near ptr 7FFFFFF7h",
			"ht jnp near ptr 7FFFFFF7h",
			"ht jl near ptr 7FFFFFF7h",
			"ht jge near ptr 7FFFFFF7h",
			"ht jle near ptr 7FFFFFF7h",
			"ht jg near ptr 7FFFFFF7h",
			"bnd jo short 7FFFFFF3h",
			"bnd jno short 7FFFFFF3h",
			"bnd jb short 7FFFFFF3h",
			"bnd jae short 7FFFFFF3h",
			"bnd je short 7FFFFFF3h",
			"bnd jne short 7FFFFFF3h",
			"bnd jbe short 7FFFFFF3h",
			"bnd ja short 7FFFFFF3h",
			"bnd js short 7FFFFFF3h",
			"bnd jns short 7FFFFFF3h",
			"bnd jp short 7FFFFFF3h",
			"bnd jnp short 7FFFFFF3h",
			"bnd jl short 7FFFFFF3h",
			"bnd jge short 7FFFFFF3h",
			"bnd jle short 7FFFFFF3h",
			"bnd jg short 7FFFFFF3h",
			"bnd jo near ptr 7FFFFFF7h",
			"bnd jno near ptr 7FFFFFF7h",
			"bnd jb near ptr 7FFFFFF7h",
			"bnd jae near ptr 7FFFFFF7h",
			"bnd je near ptr 7FFFFFF7h",
			"bnd jne near ptr 7FFFFFF7h",
			"bnd jbe near ptr 7FFFFFF7h",
			"bnd ja near ptr 7FFFFFF7h",
			"bnd js near ptr 7FFFFFF7h",
			"bnd jns near ptr 7FFFFFF7h",
			"bnd jp near ptr 7FFFFFF7h",
			"bnd jnp near ptr 7FFFFFF7h",
			"bnd jl near ptr 7FFFFFF7h",
			"bnd jge near ptr 7FFFFFF7h",
			"bnd jle near ptr 7FFFFFF7h",
			"bnd jg near ptr 7FFFFFF7h",
			"bnd jmp near ptr 7FFFFFF6h",
			"bnd jmp dword ptr [eax]",
			"bnd jmp eax",
			"bnd call 7FFFFFF6h",
			"bnd call dword ptr [eax]",
			"bnd call eax",
			"bnd ret 0",
			"bnd ret",
		};
	}
}
#endif
