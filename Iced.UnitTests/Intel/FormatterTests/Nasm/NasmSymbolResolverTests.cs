/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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
			"jo -symnext-0x123 (0x7ff2)",
			"jo short symbol-0x123",
			"jo symbol",
			"jo symbol+0x123",
			"jo -symnext+0x123 (0x7ffffff2)",
			"jo short symbol",
			"jo symbol+0x123",
			"jo symbol-0x123",
			"jo -symnext (0x7ffffffffffffff2)",
			"jo short symbol+0x123",
			"jo symbol-0x123",
			"call -selsym:symbol",
			"call selsymextra+0x123 (0xa55a):-symbolmore-0x123 (0xfedc)",
			"call 0xa55a:symbol-0x123",
			"call 0xa55a:symbol",
			"call -selsym-0x123:symbol+0x123",
			"call selsymextra-0x123 (0xa55a):-symbolmore+0x123 (0xfedcba98)",
			"call 0xa55a:symbol",
			"call 0xa55a:symbol+0x123",
			"mov cl,symbol-0x123",
			"mov cl,-symnext (0x00a5)",
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
			"mov ecx,-symnext (0xfedcba98)",
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
			"mov al,[qword -symnext (0xf0debc9a78563412)]",
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
