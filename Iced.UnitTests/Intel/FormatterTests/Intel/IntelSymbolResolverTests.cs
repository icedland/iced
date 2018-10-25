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

#if !NO_INTEL_FORMATTER && !NO_FORMATTER
using System.Collections.Generic;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests.Intel {
	public sealed class IntelSymbolResolverTests : SymbolResolverTests {
		[Theory]
		[MemberData(nameof(Format_Data))]
		void Format(int index, int resultDispl, SymbolInstructionInfo info, string formattedString) => FormatBase(index, resultDispl, info, formattedString, IntelFormatterFactory.Create_Resolver(info.SymbolResolver.Clone()));
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
			"call far symbol,-selsym",
			"call far -symbolmore-0x123,selsymextra+0x123",
			"call far symbol-0x123,0xa55a",
			"call far symbol,0xa55a",
			"call far symbol+0x123,-selsym-0x123",
			"call far -symbolmore+0x123,selsymextra-0x123",
			"call far symbol,0xa55a",
			"call far symbol+0x123,0xa55a",
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
			"movsb es:[di+symbol-0x123],fs:[si+symbol-0x123]",
			"movsb es:[di-symnext],fs:[si-symnext]",
			"movsb es:[di+symbol+0x123],fs:[si+symbol+0x123]",
			"movsb es:[di-symnext+0x123],fs:[si-symnext+0x123]",
			"movsb es:[edi+symbol],fs:[esi+symbol]",
			"movsb es:[edi-symnext-0x123],fs:[esi-symnext-0x123]",
			"movsb es:[edi+symbol-0x123],fs:[esi+symbol-0x123]",
			"movsb es:[edi-symnext],fs:[esi-symnext]",
			"movsb es:[rdi+symbol+0x123],fs:[rsi+symbol+0x123]",
			"movsb es:[rdi-symnext+0x123],fs:[rsi-symnext+0x123]",
			"movsb es:[rdi+symbol],fs:[rsi+symbol]",
			"movsb es:[rdi-symnext-0x123],fs:[rsi-symnext-0x123]",
			"mov al,[symbol-0x123]",
			"mov al,[-symnext]",
			"mov al,[symbol+0x123]",
			"mov al,[-symnext+0x123]",
			"mov cl,[symbol]",
			"mov cl,[-symnext-0x123]",
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
