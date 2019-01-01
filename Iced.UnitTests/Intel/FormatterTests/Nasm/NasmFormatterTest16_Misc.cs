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
	public sealed class NasmFormatterTest16_Misc : FormatterTest {
		[Theory]
		[MemberData(nameof(Format_Data))]
		void Format(int index, InstructionInfo info, string formattedString) => FormatBase(index, info, formattedString, NasmFormatterFactory.Create());
		public static IEnumerable<object[]> Format_Data => GetFormatData(infos, formattedStrings);

		static readonly InstructionInfo[] infos = InstructionInfos16_Misc.AllInfos;
		static readonly string[] formattedStrings = new string[InstructionInfos16_Misc.AllInfos_Length] {
			"cs jo 0x7ff3",
			"cs jno 0x7ff3",
			"cs jb 0x7ff3",
			"cs jae 0x7ff3",
			"cs je 0x7ff3",
			"cs jne 0x7ff3",
			"cs jbe 0x7ff3",
			"cs ja 0x7ff3",
			"cs js 0x7ff3",
			"cs jns 0x7ff3",
			"cs jp 0x7ff3",
			"cs jnp 0x7ff3",
			"cs jl 0x7ff3",
			"cs jge 0x7ff3",
			"cs jle 0x7ff3",
			"cs jg 0x7ff3",
			"ds jo 0x7ff3",
			"ds jno 0x7ff3",
			"ds jb 0x7ff3",
			"ds jae 0x7ff3",
			"ds je 0x7ff3",
			"ds jne 0x7ff3",
			"ds jbe 0x7ff3",
			"ds ja 0x7ff3",
			"ds js 0x7ff3",
			"ds jns 0x7ff3",
			"ds jp 0x7ff3",
			"ds jnp 0x7ff3",
			"ds jl 0x7ff3",
			"ds jge 0x7ff3",
			"ds jle 0x7ff3",
			"ds jg 0x7ff3",
			"cs jo 0x7ff5",
			"cs jno 0x7ff5",
			"cs jb 0x7ff5",
			"cs jae 0x7ff5",
			"cs je 0x7ff5",
			"cs jne 0x7ff5",
			"cs jbe 0x7ff5",
			"cs ja 0x7ff5",
			"cs js 0x7ff5",
			"cs jns 0x7ff5",
			"cs jp 0x7ff5",
			"cs jnp 0x7ff5",
			"cs jl 0x7ff5",
			"cs jge 0x7ff5",
			"cs jle 0x7ff5",
			"cs jg 0x7ff5",
			"ds jo 0x7ff5",
			"ds jno 0x7ff5",
			"ds jb 0x7ff5",
			"ds jae 0x7ff5",
			"ds je 0x7ff5",
			"ds jne 0x7ff5",
			"ds jbe 0x7ff5",
			"ds ja 0x7ff5",
			"ds js 0x7ff5",
			"ds jns 0x7ff5",
			"ds jp 0x7ff5",
			"ds jnp 0x7ff5",
			"ds jl 0x7ff5",
			"ds jge 0x7ff5",
			"ds jle 0x7ff5",
			"ds jg 0x7ff5",
			"bnd jo 0x7ff3",
			"bnd jno 0x7ff3",
			"bnd jb 0x7ff3",
			"bnd jae 0x7ff3",
			"bnd je 0x7ff3",
			"bnd jne 0x7ff3",
			"bnd jbe 0x7ff3",
			"bnd ja 0x7ff3",
			"bnd js 0x7ff3",
			"bnd jns 0x7ff3",
			"bnd jp 0x7ff3",
			"bnd jnp 0x7ff3",
			"bnd jl 0x7ff3",
			"bnd jge 0x7ff3",
			"bnd jle 0x7ff3",
			"bnd jg 0x7ff3",
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
			"bnd jmp word [bx+si]",
			"bnd jmp ax",
			"bnd call 0x7ff4",
			"bnd call word [bx+si]",
			"bnd call ax",
			"bnd ret 0",
			"bnd ret",
			"notrack call word [ds:bx+si]",
			"notrack call word [ds:bp+0x11]",
			"notrack call cx",
			"notrack jmp word [ds:bx+si]",
			"notrack jmp word [ds:bp+0x11]",
			"notrack jmp cx",
			"notrack bnd call cx",
			"notrack call word [ds:bx+si]",
			"call word [fs:bx+si]",
			"notrack bnd jmp cx",
			"notrack jmp word [ds:bx+si]",
			"jmp word [fs:bx+si]",
		};
	}
}
#endif
