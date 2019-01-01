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

#if !NO_MASM_FORMATTER && !NO_FORMATTER
using System.Collections.Generic;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests.Masm {
	public sealed class MasmSymbolResolverTests : SymbolResolverTests {
		[Theory]
		[MemberData(nameof(Format_Data))]
		void Format(int index, int resultDispl, SymbolInstructionInfo info, string formattedString) => FormatBase(index, resultDispl, info, formattedString, MasmFormatterFactory.Create_Resolver(info.SymbolResolver.Clone()));
		public static IEnumerable<object[]> Format_Data => GetFormatData(infos, formattedStrings);

		static readonly SymbolInstructionInfo[] infos = SymbolResolverTestInfos.AllInfos;
		static readonly string[] formattedStrings = new string[SymbolResolverTestInfos.AllInfos_Length] {
			"jo symbol",
			"jo -symnext-123h (7FF2h)",
			"jo short symbol-123h",
			"jo symbol",
			"jo symbol+123h",
			"jo -symnext+123h (7FFFFFF2h)",
			"jo short symbol",
			"jo symbol+123h",
			"jo symbol-123h",
			"jo -symnext (7FFFFFFFFFFFFFF2h)",
			"jo short symbol+123h",
			"jo symbol-123h",
			"call -selsym:symbol",
			"call selsymextra+123h (0A55Ah):-symbolmore-123h (0FEDCh)",
			"call far ptr 0A55Ah:symbol-123h",
			"call 0A55Ah:symbol",
			"call -selsym-123h:symbol+123h",
			"call selsymextra-123h (0A55Ah):-symbolmore+123h (0FEDCBA98h)",
			"call far ptr 0A55Ah:symbol",
			"call 0A55Ah:symbol+123h",
			"mov cl,symbol-123h",
			"mov cl,-symnext (00A5h)",
			"mov cl,offset symbol+123h",
			"mov cl,offset -symnext+123h",
			"enter 0A55Ah,symbol",
			"enter 0A55Ah,-symnext-123h",
			"enter 0A55Ah,offset symbol-123h",
			"enter 0A55Ah,offset -symnext",
			"mov cx,symbol+123h",
			"mov cx,-symnext+123h",
			"mov cx,offset symbol",
			"mov cx,offset -symnext-123h",
			"mov ecx,symbol-123h",
			"mov ecx,-symnext (0FEDCBA98h)",
			"mov ecx,offset symbol+123h",
			"mov ecx,offset -symnext+123h",
			"mov rcx,symbol",
			"mov rcx,-symnext-123h",
			"mov rcx,offset symbol-123h",
			"mov rcx,offset -symnext",
			"int symbol+123h",
			"int -symnext+123h",
			"int offset symbol",
			"int offset -symnext-123h",
			"or cx,symbol-123h",
			"or cx,-symnext",
			"or cx,offset symbol+123h",
			"or cx,offset -symnext+123h",
			"or ecx,symbol",
			"or ecx,-symnext-123h",
			"or ecx,offset symbol-123h",
			"or ecx,offset -symnext",
			"or rcx,symbol+123h",
			"or rcx,-symnext+123h",
			"or rcx,offset symbol",
			"or rcx,offset -symnext-123h",
			"movs byte ptr es:[di+symbol-123h],byte ptr fs:[si+symbol-123h]",
			"movs byte ptr es:[di-symnext],byte ptr fs:[si-symnext]",
			"movs byte ptr es:[di+symbol+123h],byte ptr fs:[si+symbol+123h]",
			"movs byte ptr es:[di-symnext+123h],byte ptr fs:[si-symnext+123h]",
			"movs byte ptr es:[edi+symbol],byte ptr fs:[esi+symbol]",
			"movs byte ptr es:[edi-symnext-123h],byte ptr fs:[esi-symnext-123h]",
			"movs byte ptr es:[edi+symbol-123h],byte ptr fs:[esi+symbol-123h]",
			"movs byte ptr es:[edi-symnext],byte ptr fs:[esi-symnext]",
			"movs byte ptr es:[rdi+symbol+123h],byte ptr fs:[rsi+symbol+123h]",
			"movs byte ptr es:[rdi-symnext+123h],byte ptr fs:[rsi-symnext+123h]",
			"movs byte ptr es:[rdi+symbol],byte ptr fs:[rsi+symbol]",
			"movs byte ptr es:[rdi-symnext-123h],byte ptr fs:[rsi-symnext-123h]",
			"mov al,[symbol-123h]",
			"mov al,[-symnext (0F0DEBC9A78563412h)]",
			"mov al,[symbol+123h]",
			"mov al,[-symnext+123h]",
			"mov cl,[symbol]",
			"mov cl,[-symnext-123h]",
			"mov cl,[rip+symbol-123h]",
			"mov cl,[eip-symnext]",
			"mov al,[symbol+123h]",
			"mov al,[-symnext+123h]",
			"mov al,[symbol]",
			"mov al,[-symnext-123h]",
			"mov al,[symbol-123h]",
			"mov al,[-symnext]",
			"mov al,[symbol+123h]",
			"mov al,[-symnext+123h]",
			"mov al,[symbol]",
			"mov al,[-symnext-123h]",
			"mov al,[symbol-123h]",
			"mov al,[-symnext]",
			"mov al,[rax+symbol+123h]",
			"mov al,[rax-symnext+123h]",
			"mov al,[rax+symbol]",
			"mov al,[rax-symnext-123h]",
			"mov al,[rax+symbol-123h]",
			"mov al,[rax-symnext]",
			"mov al,[rax+symbol+123h]",
			"mov al,[rax-symnext+123h]",
			"mov al,[rax + symbol]",
			"mov al,[rax - symnext - 123h]",
			"mov al,[rax+symbol-123h]",
			"mov al,[rax-symnext]",
			"mov al,[bx+si+symbol+123h]",
			"mov al,[bx+si-symnext+123h]",
			"mov al,[bx+si+symbol]",
			"mov al,[bx+si-symnext-123h]",
			"mov al,[eax+symbol-123h]",
			"mov al,[eax-symnext]",
			"mov al,[eax+symbol+123h]",
			"mov al,[eax-symnext+123h]",
			"mov al,[rax+symbol]",
			"mov al,[rax-symnext-123h]",
			"mov al,[rax+symbol-123h]",
			"mov al,[rax-symnext]",
			"mov al,[rax-5Bh]",
		};
	}
}
#endif
