// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if GAS || INTEL || MASM || NASM || FAST_FMT
using System.Linq;
using Iced.Intel;

namespace Iced.UnitTests.Intel.FormatterTests {
	sealed class TestSymbolResolver : ISymbolResolver {
		readonly SymbolResolverTestCase tc;

		public TestSymbolResolver(in SymbolResolverTestCase tc) => this.tc = tc;

		public bool TryGetSymbol(in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) {
			foreach (var tc in tc.SymbolResults) {
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
