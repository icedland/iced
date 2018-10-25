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
		void Format(int index, int resultDispl, SymbolInstructionInfo info, string formattedString) => FormatBase(index, resultDispl, info, formattedString, NasmFormatterFactory.Create_Resolver(info.SymbolResolver.Clone()));
		public static IEnumerable<object[]> Format_Data => GetFormatData(infos, formattedStrings);

		static readonly SymbolInstructionInfo[] infos = SymbolResolverTestInfos.AllInfos;
		static readonly string[] formattedStrings = new string[SymbolResolverTestInfos.AllInfos_Length] {
			"jo symbol",
			"jo -symnext-0x123",
			"jo short symbol-0x123",
			"jo symbol",
			"jo symbol+0x123",
			"jo -symnext+0x123",
			"jo short symbol",
			"jo symbol+0x123",
			"jo symbol-0x123",
			"jo -symnext",
			"jo short symbol+0x123",
			"jo symbol-0x123",
			"call -selsym:symbol",
			"call selsymextra+0x123:-symbolmore-0x123",
			"call 0xa55a:symbol-0x123",
			"call 0xa55a:symbol",
			"call -selsym-0x123:symbol+0x123",
			"call selsymextra-0x123:-symbolmore+0x123",
			"call 0xa55a:symbol",
			"call 0xa55a:symbol+0x123",
			"mov cl,symbol-0x123",
			"mov cl,-symnext",
			"mov cl,symbol+0x123",
			"mov cl,-symnext+0x123",
			"enter 0xa55a,symbol",
			"enter 0xa55a,-symnext-0x123",
			"enter 0xa55a,symbol-0x123",
			"enter 0xa55a,-symnext",
			"mov cx,symbol+0x123",
			"mov cx,-symnext+0x123",
			"mov cx,symbol",
			"mov cx,-symnext-0x123",
			"mov ecx,symbol-0x123",
			"mov ecx,-symnext",
			"mov ecx,symbol+0x123",
			"mov ecx,-symnext+0x123",
			"mov rcx,symbol",
			"mov rcx,-symnext-0x123",
			"mov rcx,symbol-0x123",
			"mov rcx,-symnext",
			"int3",
			"int3",
			"int3",
			"int3",
			"or cx,symbol-0x123",
			"or cx,-symnext",
			"or cx,symbol+0x123",
			"or cx,-symnext+0x123",
			"or ecx,symbol",
			"or ecx,-symnext-0x123",
			"or ecx,symbol-0x123",
			"or ecx,-symnext",
			"or rcx,symbol+0x123",
			"or rcx,-symnext+0x123",
			"or rcx,symbol",
			"or rcx,-symnext-0x123",
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
			"mov al,[qword symbol-0x123]",
			"mov al,[qword -symnext]",
			"mov al,[qword symbol+0x123]",
			"mov al,[qword -symnext+0x123]",
			"mov cl,[rel symbol]",
			"mov cl,[dword rel -symnext-0x123]",
			"mov cl,[rip+symbol-0x123]",
			"mov cl,[eip-symnext]",
			"mov al,[symbol+0x123]",
			"mov al,[-symnext+0x123]",
			"mov al,[symbol]",
			"mov al,[-symnext-0x123]",
			"mov al,[symbol-0x123]",
			"mov al,[-symnext]",
			"mov al,[symbol+0x123]",
			"mov al,[-symnext+0x123]",
			"mov al,[symbol]",
			"mov al,[-symnext-0x123]",
			"mov al,[symbol-0x123]",
			"mov al,[-symnext]",
			"mov al,[rax+symbol+0x123]",
			"mov al,[rax-symnext+0x123]",
			"mov al,[rax+symbol]",
			"mov al,[rax-symnext-0x123]",
			"mov al,[rax+symbol-0x123]",
			"mov al,[rax-symnext]",
			"mov al,[rax+symbol+0x123]",
			"mov al,[rax-symnext+0x123]",
			"mov al,[rax + symbol]",
			"mov al,[rax - symnext - 0x123]",
			"mov al,[rax+symbol-0x123]",
			"mov al,[rax-symnext]",
			"mov al,[bx+si+symbol+0x123]",
			"mov al,[bx+si-symnext+0x123]",
			"mov al,[bx+si+symbol]",
			"mov al,[bx+si-symnext-0x123]",
			"mov al,[eax+symbol-0x123]",
			"mov al,[eax-symnext]",
			"mov al,[eax+symbol+0x123]",
			"mov al,[eax-symnext+0x123]",
			"mov al,[rax+symbol]",
			"mov al,[rax-symnext-0x123]",
			"mov al,[rax+symbol-0x123]",
			"mov al,[rax-symnext]",
			"mov al,[rax-0x5b]",
		};
	}
}
#endif
