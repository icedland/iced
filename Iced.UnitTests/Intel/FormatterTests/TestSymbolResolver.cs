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

#if (!NO_GAS_FORMATTER || !NO_INTEL_FORMATTER || !NO_MASM_FORMATTER || !NO_NASM_FORMATTER) && !NO_FORMATTER
using Iced.Intel;

namespace Iced.UnitTests.Intel.FormatterTests {
	sealed class TestSymbolResolver : ISymbolResolver {
		public delegate bool TryGetSymbolDelegate(int operand, ref Instruction instruction, ulong address, int addressSize, out SymbolResult symbol);
		public TryGetSymbolDelegate tryGetSymbol;
		public bool TryGetSymbol(int operand, ref Instruction instruction, ulong address, int addressSize, out SymbolResult symbol) {
			if (tryGetSymbol != null)
				return tryGetSymbol(operand, ref instruction, address, addressSize, out symbol);
			symbol = default;
			return false;
		}
	}
}
#endif
