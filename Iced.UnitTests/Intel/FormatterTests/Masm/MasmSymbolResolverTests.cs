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
	public sealed class MasmSymbolResolverTests : SymbolResolverTests {
		[Theory]
		[MemberData(nameof(Format_Data))]
		void Format(int index, SymbolInstructionInfo info, string formattedString) => FormatBase(index, info, formattedString, MasmFormatterFactory.Create_Resolver(info.SymbolResolver));
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
			"call far ptr 0A55Ah:symbol",
			"call 0A55Ah:symbol",
			"call -selsym:symbol",
			"call selsymextra:-symbolmore",
			"call far ptr 0A55Ah:symbol",
			"call 0A55Ah:symbol",
			"mov cl,symbol",
			"mov cl,-symnext",
			"mov cl,offset symbol",
			"mov cl,offset -symnext",
			"enter 0A55Ah,symbol",
			"enter 0A55Ah,-symnext",
			"enter 0A55Ah,offset symbol",
			"enter 0A55Ah,offset -symnext",
			"mov cx,symbol",
			"mov cx,-symnext",
			"mov cx,offset symbol",
			"mov cx,offset -symnext",
			"mov ecx,symbol",
			"mov ecx,-symnext",
			"mov ecx,offset symbol",
			"mov ecx,offset -symnext",
			"mov rcx,symbol",
			"mov rcx,-symnext",
			"mov rcx,offset symbol",
			"mov rcx,offset -symnext",
			"int symbol",
			"int -symnext",
			"int offset symbol",
			"int offset -symnext",
			"or cx,symbol",
			"or cx,-symnext",
			"or cx,offset symbol",
			"or cx,offset -symnext",
			"or ecx,symbol",
			"or ecx,-symnext",
			"or ecx,offset symbol",
			"or ecx,offset -symnext",
			"or rcx,symbol",
			"or rcx,-symnext",
			"or rcx,offset symbol",
			"or rcx,offset -symnext",
			"movs byte ptr es:[di+symbol],byte ptr fs:[si+symbol]",
			"movs byte ptr es:[di-symnext],byte ptr fs:[si-symnext]",
			"movs byte ptr es:[di+symbol],byte ptr fs:[si+symbol]",
			"movs byte ptr es:[di-symnext],byte ptr fs:[si-symnext]",
			"movs byte ptr es:[edi+symbol],byte ptr fs:[esi+symbol]",
			"movs byte ptr es:[edi-symnext],byte ptr fs:[esi-symnext]",
			"movs byte ptr es:[edi+symbol],byte ptr fs:[esi+symbol]",
			"movs byte ptr es:[edi-symnext],byte ptr fs:[esi-symnext]",
			"movs byte ptr es:[rdi+symbol],byte ptr fs:[rsi+symbol]",
			"movs byte ptr es:[rdi-symnext],byte ptr fs:[rsi-symnext]",
			"movs byte ptr es:[rdi+symbol],byte ptr fs:[rsi+symbol]",
			"movs byte ptr es:[rdi-symnext],byte ptr fs:[rsi-symnext]",
			"mov al,[symbol]",
			"mov al,[-symnext]",
			"mov al,[symbol]",
			"mov al,[-symnext]",
			"mov cl,[symbol]",
			"mov cl,[-symnext]",
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
			"mov al,[rax-5Bh]",
		};
	}
}
#endif
