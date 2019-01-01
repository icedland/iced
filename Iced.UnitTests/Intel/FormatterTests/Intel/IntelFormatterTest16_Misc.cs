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

#if !NO_INTEL_FORMATTER && !NO_FORMATTER
using System.Collections.Generic;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests.Intel {
	public sealed class IntelFormatterTest16_Misc : FormatterTest {
		[Theory]
		[MemberData(nameof(Format_Data))]
		void Format(int index, InstructionInfo info, string formattedString) => FormatBase(index, info, formattedString, IntelFormatterFactory.Create());
		public static IEnumerable<object[]> Format_Data => GetFormatData(infos, formattedStrings);

		static readonly InstructionInfo[] infos = InstructionInfos16_Misc.AllInfos;
		static readonly string[] formattedStrings = new string[InstructionInfos16_Misc.AllInfos_Length] {
			"hint-not-taken jo short 0x7ff3",
			"hint-not-taken jno short 0x7ff3",
			"hint-not-taken jb short 0x7ff3",
			"hint-not-taken jae short 0x7ff3",
			"hint-not-taken je short 0x7ff3",
			"hint-not-taken jne short 0x7ff3",
			"hint-not-taken jbe short 0x7ff3",
			"hint-not-taken ja short 0x7ff3",
			"hint-not-taken js short 0x7ff3",
			"hint-not-taken jns short 0x7ff3",
			"hint-not-taken jp short 0x7ff3",
			"hint-not-taken jnp short 0x7ff3",
			"hint-not-taken jl short 0x7ff3",
			"hint-not-taken jge short 0x7ff3",
			"hint-not-taken jle short 0x7ff3",
			"hint-not-taken jg short 0x7ff3",
			"hint-taken jo short 0x7ff3",
			"hint-taken jno short 0x7ff3",
			"hint-taken jb short 0x7ff3",
			"hint-taken jae short 0x7ff3",
			"hint-taken je short 0x7ff3",
			"hint-taken jne short 0x7ff3",
			"hint-taken jbe short 0x7ff3",
			"hint-taken ja short 0x7ff3",
			"hint-taken js short 0x7ff3",
			"hint-taken jns short 0x7ff3",
			"hint-taken jp short 0x7ff3",
			"hint-taken jnp short 0x7ff3",
			"hint-taken jl short 0x7ff3",
			"hint-taken jge short 0x7ff3",
			"hint-taken jle short 0x7ff3",
			"hint-taken jg short 0x7ff3",
			"hint-not-taken jo 0x7ff5",
			"hint-not-taken jno 0x7ff5",
			"hint-not-taken jb 0x7ff5",
			"hint-not-taken jae 0x7ff5",
			"hint-not-taken je 0x7ff5",
			"hint-not-taken jne 0x7ff5",
			"hint-not-taken jbe 0x7ff5",
			"hint-not-taken ja 0x7ff5",
			"hint-not-taken js 0x7ff5",
			"hint-not-taken jns 0x7ff5",
			"hint-not-taken jp 0x7ff5",
			"hint-not-taken jnp 0x7ff5",
			"hint-not-taken jl 0x7ff5",
			"hint-not-taken jge 0x7ff5",
			"hint-not-taken jle 0x7ff5",
			"hint-not-taken jg 0x7ff5",
			"hint-taken jo 0x7ff5",
			"hint-taken jno 0x7ff5",
			"hint-taken jb 0x7ff5",
			"hint-taken jae 0x7ff5",
			"hint-taken je 0x7ff5",
			"hint-taken jne 0x7ff5",
			"hint-taken jbe 0x7ff5",
			"hint-taken ja 0x7ff5",
			"hint-taken js 0x7ff5",
			"hint-taken jns 0x7ff5",
			"hint-taken jp 0x7ff5",
			"hint-taken jnp 0x7ff5",
			"hint-taken jl 0x7ff5",
			"hint-taken jge 0x7ff5",
			"hint-taken jle 0x7ff5",
			"hint-taken jg 0x7ff5",
			"bnd jo short 0x7ff3",
			"bnd jno short 0x7ff3",
			"bnd jb short 0x7ff3",
			"bnd jae short 0x7ff3",
			"bnd je short 0x7ff3",
			"bnd jne short 0x7ff3",
			"bnd jbe short 0x7ff3",
			"bnd ja short 0x7ff3",
			"bnd js short 0x7ff3",
			"bnd jns short 0x7ff3",
			"bnd jp short 0x7ff3",
			"bnd jnp short 0x7ff3",
			"bnd jl short 0x7ff3",
			"bnd jge short 0x7ff3",
			"bnd jle short 0x7ff3",
			"bnd jg short 0x7ff3",
			"bnd jo 0x7ff5",
			"bnd jno 0x7ff5",
			"bnd jb 0x7ff5",
			"bnd jae 0x7ff5",
			"bnd je 0x7ff5",
			"bnd jne 0x7ff5",
			"bnd jbe 0x7ff5",
			"bnd ja 0x7ff5",
			"bnd js 0x7ff5",
			"bnd jns 0x7ff5",
			"bnd jp 0x7ff5",
			"bnd jnp 0x7ff5",
			"bnd jl 0x7ff5",
			"bnd jge 0x7ff5",
			"bnd jle 0x7ff5",
			"bnd jg 0x7ff5",
			"bnd jmp 0x7ff4",
			"bnd jmp word ptr [bx+si]",
			"bnd jmp ax",
			"bnd call 0x7ff4",
			"bnd call word ptr [bx+si]",
			"bnd call ax",
			"bnd ret 0",
			"bnd ret",
			"notrack call word ptr ds:[bx+si]",
			"notrack call word ptr ds:[bp+0x11]",
			"notrack call cx",
			"notrack jmp word ptr ds:[bx+si]",
			"notrack jmp word ptr ds:[bp+0x11]",
			"notrack jmp cx",
			"notrack bnd call cx",
			"notrack call word ptr ds:[bx+si]",
			"call word ptr fs:[bx+si]",
			"notrack bnd jmp cx",
			"notrack jmp word ptr ds:[bx+si]",
			"jmp word ptr fs:[bx+si]",
		};
	}
}
#endif
