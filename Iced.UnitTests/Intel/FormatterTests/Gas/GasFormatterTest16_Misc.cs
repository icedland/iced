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

#if !NO_GAS_FORMATTER && !NO_FORMATTER
using System.Collections.Generic;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests.Gas {
	public sealed class GasFormatterTest16_Misc : FormatterTest {
		[Theory]
		[MemberData(nameof(Format_Data))]
		void Format(int index, InstructionInfo info, string formattedString) => FormatBase(index, info, formattedString, GasFormatterFactory.Create());
		public static IEnumerable<object[]> Format_Data => GetFormatData(infos, formattedStrings);

		static readonly InstructionInfo[] infos = InstructionInfos16_Misc.AllInfos;
		static readonly string[] formattedStrings = new string[InstructionInfos16_Misc.AllInfos_Length] {
			"jo,pn 0x7ff3",
			"jno,pn 0x7ff3",
			"jb,pn 0x7ff3",
			"jae,pn 0x7ff3",
			"je,pn 0x7ff3",
			"jne,pn 0x7ff3",
			"jbe,pn 0x7ff3",
			"ja,pn 0x7ff3",
			"js,pn 0x7ff3",
			"jns,pn 0x7ff3",
			"jp,pn 0x7ff3",
			"jnp,pn 0x7ff3",
			"jl,pn 0x7ff3",
			"jge,pn 0x7ff3",
			"jle,pn 0x7ff3",
			"jg,pn 0x7ff3",
			"jo,pt 0x7ff3",
			"jno,pt 0x7ff3",
			"jb,pt 0x7ff3",
			"jae,pt 0x7ff3",
			"je,pt 0x7ff3",
			"jne,pt 0x7ff3",
			"jbe,pt 0x7ff3",
			"ja,pt 0x7ff3",
			"js,pt 0x7ff3",
			"jns,pt 0x7ff3",
			"jp,pt 0x7ff3",
			"jnp,pt 0x7ff3",
			"jl,pt 0x7ff3",
			"jge,pt 0x7ff3",
			"jle,pt 0x7ff3",
			"jg,pt 0x7ff3",
			"jo,pn 0x7ff5",
			"jno,pn 0x7ff5",
			"jb,pn 0x7ff5",
			"jae,pn 0x7ff5",
			"je,pn 0x7ff5",
			"jne,pn 0x7ff5",
			"jbe,pn 0x7ff5",
			"ja,pn 0x7ff5",
			"js,pn 0x7ff5",
			"jns,pn 0x7ff5",
			"jp,pn 0x7ff5",
			"jnp,pn 0x7ff5",
			"jl,pn 0x7ff5",
			"jge,pn 0x7ff5",
			"jle,pn 0x7ff5",
			"jg,pn 0x7ff5",
			"jo,pt 0x7ff5",
			"jno,pt 0x7ff5",
			"jb,pt 0x7ff5",
			"jae,pt 0x7ff5",
			"je,pt 0x7ff5",
			"jne,pt 0x7ff5",
			"jbe,pt 0x7ff5",
			"ja,pt 0x7ff5",
			"js,pt 0x7ff5",
			"jns,pt 0x7ff5",
			"jp,pt 0x7ff5",
			"jnp,pt 0x7ff5",
			"jl,pt 0x7ff5",
			"jge,pt 0x7ff5",
			"jle,pt 0x7ff5",
			"jg,pt 0x7ff5",
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
			"bnd jmpw *(%bx,%si)",
			"bnd jmp *%ax",
			"bnd call 0x7ff4",
			"bnd callw *(%bx,%si)",
			"bnd call *%ax",
			"bnd ret $0",
			"bnd ret",
			"notrack callw *%ds:(%bx,%si)",
			"notrack callw *%ds:0x11(%bp)",
			"notrack call *%cx",
			"notrack jmpw *%ds:(%bx,%si)",
			"notrack jmpw *%ds:0x11(%bp)",
			"notrack jmp *%cx",
			"notrack bnd call *%cx",
			"notrack callw *%ds:(%bx,%si)",
			"callw *%fs:(%bx,%si)",
			"notrack bnd jmp *%cx",
			"notrack jmpw *%ds:(%bx,%si)",
			"jmpw *%fs:(%bx,%si)",
		};
	}
}
#endif
