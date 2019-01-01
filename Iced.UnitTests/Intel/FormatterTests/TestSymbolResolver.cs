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

#if (!NO_GAS_FORMATTER || !NO_INTEL_FORMATTER || !NO_MASM_FORMATTER || !NO_NASM_FORMATTER) && !NO_FORMATTER
using Iced.Intel;

namespace Iced.UnitTests.Intel.FormatterTests {
	sealed class TestSymbolResolver : ISymbolResolver {
		public TestSymbolResolver Clone() => new TestSymbolResolver() { tryGetSymbol = tryGetSymbol };
		public delegate bool TryGetSymbolDelegate(int operand, int instructionOperand, ref Instruction instruction, ulong address, int addressSize, out SymbolResult symbol);
		public TryGetSymbolDelegate tryGetSymbol;
		public int resultDispl;
		public bool TryGetSymbol(int operand, int instructionOperand, ref Instruction instruction, ulong address, int addressSize, out SymbolResult symbol) {
			if (tryGetSymbol != null) {
				if (!tryGetSymbol(operand, instructionOperand, ref instruction, address, addressSize, out symbol))
					return false;
				symbol = new SymbolResult(address + (ulong)resultDispl, symbol.Text, symbol.Flags);
				return true;
			}
			symbol = default;
			return false;
		}
	}
}
#endif
