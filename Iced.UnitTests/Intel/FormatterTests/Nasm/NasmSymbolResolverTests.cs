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

#if !NO_NASM_FORMATTER && !NO_FORMATTER
using System.Collections.Generic;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests.Nasm {
	public sealed class NasmSymbolResolverTests : SymbolResolverTests {
		[Theory]
		[MemberData(nameof(Format_Data))]
		void Format(int index, SymbolInstructionInfo info, string formattedString) => FormatBase(index, info, formattedString, NasmFormatterFactory.Create_Resolver(info.SymbolResolver));
		public static IEnumerable<object[]> Format_Data => GetFormatData(infos, formattedStrings);

		static readonly SymbolInstructionInfo[] infos = SymbolResolverTestInfos.AllInfos;
		static readonly string[] formattedStrings = new string[SymbolResolverTestInfos.AllInfos_Length] {
			"jo symbol",
			"jo -symnext",
			"jo short symbol",
			"jo symbol",
			"jo symbol",
			"jo -symnext",
			"jo short symbol",
			"jo symbol",
			"jo symbol",
			"jo -symnext",
			"jo short symbol",
			"jo symbol",
			"call -selsym:symbol",
			"call selsymextra:-symbolmore",
			"call 0xa55a:symbol",
			"call 0xa55a:symbol",
			"call -selsym:symbol",
			"call selsymextra:-symbolmore",
			"call 0xa55a:symbol",
			"call 0xa55a:symbol",
			"mov cl,symbol",
			"mov cl,-symnext",
			"mov cl,symbol",
			"mov cl,-symnext",
			"enter 0xa55a,symbol",
			"enter 0xa55a,-symnext",
			"enter 0xa55a,symbol",
			"enter 0xa55a,-symnext",
			"mov cx,symbol",
			"mov cx,-symnext",
			"mov cx,symbol",
			"mov cx,-symnext",
			"mov ecx,symbol",
			"mov ecx,-symnext",
			"mov ecx,symbol",
			"mov ecx,-symnext",
			"mov rcx,symbol",
			"mov rcx,-symnext",
			"mov rcx,symbol",
			"mov rcx,-symnext",
			"int3",
			"int3",
			"int3",
			"int3",
			"or cx,symbol",
			"or cx,-symnext",
			"or cx,symbol",
			"or cx,-symnext",
			"or ecx,symbol",
			"or ecx,-symnext",
			"or ecx,symbol",
			"or ecx,-symnext",
			"or rcx,symbol",
			"or rcx,-symnext",
			"or rcx,symbol",
			"or rcx,-symnext",
			"fs movsb",
			"fs movsb",
			"fs movsb",
			"fs movsb",
			"fs movsb",
			"fs movsb",
			"fs movsb",
			"fs movsb",
			"fs movsb",
			"fs movsb",
			"fs movsb",
			"fs movsb",
			"mov al,[qword symbol]",
			"mov al,[qword -symnext]",
			"mov al,[qword symbol]",
			"mov al,[qword -symnext]",
			"mov cl,[rel symbol]",
			"mov cl,[dword rel -symnext]",
			"mov cl,[rip+symbol]",
			"mov cl,[eip-symnext]",
			"mov al,[symbol]",
			"mov al,[-symnext]",
			"mov al,[symbol]",
			"mov al,[-symnext]",
			"mov al,[symbol]",
			"mov al,[-symnext]",
			"mov al,[symbol]",
			"mov al,[-symnext]",
			"mov al,[symbol]",
			"mov al,[-symnext]",
			"mov al,[symbol]",
			"mov al,[-symnext]",
			"mov al,[rax+symbol]",
			"mov al,[rax-symnext]",
			"mov al,[rax+symbol]",
			"mov al,[rax-symnext]",
			"mov al,[rax+symbol]",
			"mov al,[rax-symnext]",
			"mov al,[rax+symbol]",
			"mov al,[rax-symnext]",
			"mov al,[rax + symbol]",
			"mov al,[rax - symnext]",
			"mov al,[rax+symbol]",
			"mov al,[rax-symnext]",
			"mov al,[bx+si+symbol]",
			"mov al,[bx+si-symnext]",
			"mov al,[bx+si+symbol]",
			"mov al,[bx+si-symnext]",
			"mov al,[eax+symbol]",
			"mov al,[eax-symnext]",
			"mov al,[eax+symbol]",
			"mov al,[eax-symnext]",
			"mov al,[rax+symbol]",
			"mov al,[rax-symnext]",
			"mov al,[rax+symbol]",
			"mov al,[rax-symnext]",
			"mov al,[rax-0x5b]",
		};
	}
}
#endif
