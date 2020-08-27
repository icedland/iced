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

#if GAS || INTEL || MASM || NASM || FAST_FMT
using System.Linq;
using Iced.Intel;

namespace Iced.UnitTests.Intel.FormatterTests {
	sealed class TestSymbolResolver : ISymbolResolver {
		readonly SymbolResolverTestCase info;

		public TestSymbolResolver(in SymbolResolverTestCase info) => this.info = info;

		public bool TryGetSymbol(in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) {
			foreach (var tc in info.SymbolResults) {
				if (tc.Address != address || tc.AddressSize != addressSize)
					continue;
				var text = new TextInfo(tc.SymbolParts.Select(a => new TextPart(a, FormatterTextKind.Text)).ToArray());
				if (tc.MemorySize != null)
					symbol = new SymbolResult(tc.SymbolAddress, text, tc.Flags, tc.MemorySize.Value);
				else
					symbol = new SymbolResult(tc.SymbolAddress, text, tc.Flags);
				return true;
			}
			symbol = default;
			return false;
		}
	}
}
#endif
