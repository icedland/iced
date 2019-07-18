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

#if (!NO_GAS_FORMATTER || !NO_INTEL_FORMATTER || !NO_MASM_FORMATTER || !NO_NASM_FORMATTER) && !NO_FORMATTER
using Iced.Intel;

namespace Iced.UnitTests.Intel.FormatterTests {
	sealed class TestSymbolResolver : ISymbolResolver {
		public TestSymbolResolver Clone() => new TestSymbolResolver() { tryGetSymbol = tryGetSymbol };
		public delegate bool TryGetSymbolDelegate(ref Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol);
		public TryGetSymbolDelegate tryGetSymbol;
		public int resultDispl;
		public bool TryGetSymbol(ref Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) {
			if (tryGetSymbol != null) {
				if (!tryGetSymbol(ref instruction, operand, instructionOperand, address, addressSize, out symbol))
					return false;
				if (symbol.HasSymbolSize)
					symbol = new SymbolResult(address + (ulong)resultDispl, symbol.Text, symbol.Flags, symbol.SymbolSize);
				else
					symbol = new SymbolResult(address + (ulong)resultDispl, symbol.Text, symbol.Flags);
				return true;
			}
			symbol = default;
			return false;
		}
	}
}
#endif
