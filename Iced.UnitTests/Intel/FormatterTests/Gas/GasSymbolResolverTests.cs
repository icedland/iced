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
		void Format(int index, int resultDispl, SymbolInstructionInfo info, string formattedString) => FormatBase(index, resultDispl, info, formattedString, GasFormatterFactory.Create_Resolver(info.SymbolResolver.Clone()));
		public static IEnumerable<object[]> Format_Data => GetFormatData(infos, formattedStrings);

		static readonly SymbolInstructionInfo[] infos = SymbolResolverTestInfos.AllInfos;
		static readonly string[] formattedStrings = new string[SymbolResolverTestInfos.AllInfos_Length] {
			"jo symbol",
			"jo -symnext-0x123",
			"jo symbol-0x123",
			"jo symbol",
			"jo symbol+0x123",
			"jo -symnext+0x123",
			"jo symbol",
			"jo symbol+0x123",
			"jo symbol-0x123",
			"jo -symnext",
			"jo symbol+0x123",
			"jo symbol-0x123",
			"lcall $-selsym,$symbol",
			"lcall $selsymextra+0x123,$-symbolmore-0x123",
			"lcall $0xa55a,$symbol-0x123",
			"lcall $0xa55a,$symbol",
			"lcall $-selsym-0x123,$symbol+0x123",
			"lcall $selsymextra-0x123,$-symbolmore+0x123",
			"lcall $0xa55a,$symbol",
			"lcall $0xa55a,$symbol+0x123",
			"mov $symbol-0x123,%cl",
			"mov $-symnext,%cl",
			"mov $symbol+0x123,%cl",
			"mov $-symnext+0x123,%cl",
			"enter $0xa55a,$symbol",
			"enter $0xa55a,$-symnext-0x123",
			"enter $0xa55a,$symbol-0x123",
			"enter $0xa55a,$-symnext",
			"mov $symbol+0x123,%cx",
			"mov $-symnext+0x123,%cx",
			"mov $symbol,%cx",
			"mov $-symnext-0x123,%cx",
			"mov $symbol-0x123,%ecx",
			"mov $-symnext,%ecx",
			"mov $symbol+0x123,%ecx",
			"mov $-symnext+0x123,%ecx",
			"movabs $symbol,%rcx",
			"movabs $-symnext-0x123,%rcx",
			"movabs $symbol-0x123,%rcx",
			"movabs $-symnext,%rcx",
			"int3",
			"int3",
			"int3",
			"int3",
			"or $symbol-0x123,%cx",
			"or $-symnext,%cx",
			"or $symbol+0x123,%cx",
			"or $-symnext+0x123,%cx",
			"or $symbol,%ecx",
			"or $-symnext-0x123,%ecx",
			"or $symbol-0x123,%ecx",
			"or $-symnext,%ecx",
			"or $symbol+0x123,%rcx",
			"or $-symnext+0x123,%rcx",
			"or $symbol,%rcx",
			"or $-symnext-0x123,%rcx",
			"movsb %fs:symbol-0x123(%si),%es:symbol-0x123(%di)",
			"movsb %fs:-symnext(%si),%es:-symnext(%di)",
			"movsb %fs:symbol+0x123(%si),%es:symbol+0x123(%di)",
			"movsb %fs:-symnext+0x123(%si),%es:-symnext+0x123(%di)",
			"movsb %fs:symbol(%esi),%es:symbol(%edi)",
			"movsb %fs:-symnext-0x123(%esi),%es:-symnext-0x123(%edi)",
			"movsb %fs:symbol-0x123(%esi),%es:symbol-0x123(%edi)",
			"movsb %fs:-symnext(%esi),%es:-symnext(%edi)",
			"movsb %fs:symbol+0x123(%rsi),%es:symbol+0x123(%rdi)",
			"movsb %fs:-symnext+0x123(%rsi),%es:-symnext+0x123(%rdi)",
			"movsb %fs:symbol(%rsi),%es:symbol(%rdi)",
			"movsb %fs:-symnext-0x123(%rsi),%es:-symnext-0x123(%rdi)",
			"movabs symbol-0x123,%al",
			"movabs -symnext,%al",
			"movabs symbol+0x123,%al",
			"movabs -symnext+0x123,%al",
			"mov symbol,%cl",
			"mov -symnext-0x123,%cl",
			"mov symbol-0x123(%rip),%cl",
			"mov -symnext(%eip),%cl",
			"mov symbol+0x123,%al",
			"mov -symnext+0x123,%al",
			"mov symbol,%al",
			"mov -symnext-0x123,%al",
			"mov symbol-0x123,%al",
			"mov -symnext,%al",
			"mov symbol+0x123,%al",
			"mov -symnext+0x123,%al",
			"mov symbol,%al",
			"mov -symnext-0x123,%al",
			"mov symbol-0x123,%al",
			"mov -symnext,%al",
			"mov symbol+0x123(%rax),%al",
			"mov -symnext+0x123(%rax),%al",
			"mov symbol(%rax),%al",
			"mov -symnext-0x123(%rax),%al",
			"mov symbol-0x123(%rax),%al",
			"mov -symnext(%rax),%al",
			"mov symbol+0x123(%rax),%al",
			"mov -symnext+0x123(%rax),%al",
			"mov symbol(%rax),%al",
			"mov -symnext-0x123(%rax),%al",
			"mov symbol-0x123(%rax),%al",
			"mov -symnext(%rax),%al",
			"mov symbol+0x123(%bx,%si),%al",
			"mov -symnext+0x123(%bx,%si),%al",
			"mov symbol(%bx,%si),%al",
			"mov -symnext-0x123(%bx,%si),%al",
			"mov symbol-0x123(%eax),%al",
			"mov -symnext(%eax),%al",
			"mov symbol+0x123(%eax),%al",
			"mov -symnext+0x123(%eax),%al",
			"mov symbol(%rax),%al",
			"mov -symnext-0x123(%rax),%al",
			"mov symbol-0x123(%rax),%al",
			"mov -symnext(%rax),%al",
			"mov -0x5b(%rax),%al",
		};
	}
}
#endif
