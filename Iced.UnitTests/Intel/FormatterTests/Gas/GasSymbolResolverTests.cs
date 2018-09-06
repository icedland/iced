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

#if !NO_GAS_FORMATTER && !NO_FORMATTER
using System.Collections.Generic;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests.Gas {
	public sealed class GasSymbolResolverTests : SymbolResolverTests {
		[Theory]
		[MemberData(nameof(Format_Data))]
		void Format(int index, SymbolInstructionInfo info, string formattedString) => FormatBase(index, info, formattedString, GasFormatterFactory.Create_Resolver(info.SymbolResolver));
		public static IEnumerable<object[]> Format_Data => GetFormatData(infos, formattedStrings);

		static readonly SymbolInstructionInfo[] infos = SymbolResolverTestInfos.AllInfos;
		static readonly string[] formattedStrings = new string[SymbolResolverTestInfos.AllInfos_Length] {
			"jo symbol",
			"jo -symnext",
			"jo symbol",
			"jo symbol",
			"jo symbol",
			"jo -symnext",
			"jo symbol",
			"jo symbol",
			"jo symbol",
			"jo -symnext",
			"jo symbol",
			"jo symbol",
			"lcall $-selsym,$symbol",
			"lcall $selsymextra,$-symbolmore",
			"lcall $0xa55a,$symbol",
			"lcall $0xa55a,$symbol",
			"lcall $-selsym,$symbol",
			"lcall $selsymextra,$-symbolmore",
			"lcall $0xa55a,$symbol",
			"lcall $0xa55a,$symbol",
			"mov $symbol,%cl",
			"mov $-symnext,%cl",
			"mov $symbol,%cl",
			"mov $-symnext,%cl",
			"enter $0xa55a,$symbol",
			"enter $0xa55a,$-symnext",
			"enter $0xa55a,$symbol",
			"enter $0xa55a,$-symnext",
			"mov $symbol,%cx",
			"mov $-symnext,%cx",
			"mov $symbol,%cx",
			"mov $-symnext,%cx",
			"mov $symbol,%ecx",
			"mov $-symnext,%ecx",
			"mov $symbol,%ecx",
			"mov $-symnext,%ecx",
			"movabs $symbol,%rcx",
			"movabs $-symnext,%rcx",
			"movabs $symbol,%rcx",
			"movabs $-symnext,%rcx",
			"int3",
			"int3",
			"int3",
			"int3",
			"or $symbol,%cx",
			"or $-symnext,%cx",
			"or $symbol,%cx",
			"or $-symnext,%cx",
			"or $symbol,%ecx",
			"or $-symnext,%ecx",
			"or $symbol,%ecx",
			"or $-symnext,%ecx",
			"or $symbol,%rcx",
			"or $-symnext,%rcx",
			"or $symbol,%rcx",
			"or $-symnext,%rcx",
			"movsb %fs:symbol(%si),%es:symbol(%di)",
			"movsb %fs:-symnext(%si),%es:-symnext(%di)",
			"movsb %fs:symbol(%si),%es:symbol(%di)",
			"movsb %fs:-symnext(%si),%es:-symnext(%di)",
			"movsb %fs:symbol(%esi),%es:symbol(%edi)",
			"movsb %fs:-symnext(%esi),%es:-symnext(%edi)",
			"movsb %fs:symbol(%esi),%es:symbol(%edi)",
			"movsb %fs:-symnext(%esi),%es:-symnext(%edi)",
			"movsb %fs:symbol(%rsi),%es:symbol(%rdi)",
			"movsb %fs:-symnext(%rsi),%es:-symnext(%rdi)",
			"movsb %fs:symbol(%rsi),%es:symbol(%rdi)",
			"movsb %fs:-symnext(%rsi),%es:-symnext(%rdi)",
			"movabs symbol,%al",
			"movabs -symnext,%al",
			"movabs symbol,%al",
			"movabs -symnext,%al",
			"mov symbol(%rip),%cl",
			"mov -symnext(%eip),%cl",
			"mov symbol,%cl",
			"mov -symnext,%cl",
			"mov symbol,%al",
			"mov -symnext,%al",
			"mov symbol,%al",
			"mov -symnext,%al",
			"mov symbol,%al",
			"mov -symnext,%al",
			"mov symbol,%al",
			"mov -symnext,%al",
			"mov symbol,%al",
			"mov -symnext,%al",
			"mov symbol,%al",
			"mov -symnext,%al",
			"mov symbol(%rax),%al",
			"mov -symnext(%rax),%al",
			"mov symbol(%rax),%al",
			"mov -symnext(%rax),%al",
			"mov symbol(%rax),%al",
			"mov -symnext(%rax),%al",
			"mov symbol(%rax),%al",
			"mov -symnext(%rax),%al",
			"mov symbol(%rax),%al",
			"mov -symnext(%rax),%al",
			"mov symbol(%rax),%al",
			"mov -symnext(%rax),%al",
			"mov symbol(%bx,%si),%al",
			"mov -symnext(%bx,%si),%al",
			"mov symbol(%bx,%si),%al",
			"mov -symnext(%bx,%si),%al",
			"mov symbol(%eax),%al",
			"mov -symnext(%eax),%al",
			"mov symbol(%eax),%al",
			"mov -symnext(%eax),%al",
			"mov symbol(%rax),%al",
			"mov -symnext(%rax),%al",
			"mov symbol(%rax),%al",
			"mov -symnext(%rax),%al",
			"mov -0x5b(%rax),%al",
		};
	}
}
#endif
