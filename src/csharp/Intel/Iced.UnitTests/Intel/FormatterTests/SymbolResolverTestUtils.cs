// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if GAS || INTEL || MASM || NASM || FAST_FMT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Iced.UnitTests.Intel.FormatterTests {
	static class SymbolResolverTestUtils {
		public static IEnumerable<object[]> GetFormatData(string formatterDir, string formattedStringsFile) {
			var (tests, ignored) = SymbolResolverTestCases.AllTests;
			var formattedStrings = FileUtils.ReadRawStrings(Path.Combine(formatterDir, formattedStringsFile)).ToArray();
			formattedStrings = Utils.Filter(formattedStrings, ignored);
			if (tests.Length != formattedStrings.Length)
				throw new ArgumentException($"(tests.Length) {tests.Length} != (formattedStrings.Length) {formattedStrings.Length} . tests[0].HexBytes = {(tests.Length == 0 ? "<EMPTY>" : tests[0].HexBytes)} & formattedStrings[0] = {(formattedStrings.Length == 0 ? "<EMPTY>" : formattedStrings[0])}");
			var res = new object[tests.Length][];
			for (int i = 0; i < tests.Length; i++)
				res[i] = new object[3] { i, tests[i], formattedStrings[i] };
			return res;
		}
	}
}
#endif
