// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

#if GAS || INTEL || MASM || NASM || FAST_FMT
using System.Collections.Generic;
using System.Linq;

namespace Iced.UnitTests.Intel.FormatterTests {
	static class SymbolResolverTestInfos {
		public static readonly (SymbolResolverTestCase[] testCases, HashSet<int> ignored) AllInfos = GetTests();

		static (SymbolResolverTestCase[] testCases, HashSet<int> ignored) GetTests() {
			var filename = PathUtils.GetTestTextFilename("SymbolResolverTests.txt", "Formatter");
			var ignored = new HashSet<int>();
			return (SymbolResolverTestsReader.ReadFile(filename, ignored).ToArray(), ignored);
		}
	}
}
#endif
