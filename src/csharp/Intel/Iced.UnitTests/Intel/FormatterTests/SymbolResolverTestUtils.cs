// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

#if GAS || INTEL || MASM || NASM || FAST_FMT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Iced.UnitTests.Intel.FormatterTests {
	static class SymbolResolverTestUtils {
		public static IEnumerable<object[]> GetFormatData(string formatterDir, string formattedStringsFile) {
			var (infos, ignored) = SymbolResolverTestInfos.AllInfos;
			var formattedStrings = FileUtils.ReadRawStrings(Path.Combine(formatterDir, formattedStringsFile)).ToArray();
			formattedStrings = Utils.Filter(formattedStrings, ignored);
			if (infos.Length != formattedStrings.Length)
				throw new ArgumentException($"(infos.Length) {infos.Length} != (formattedStrings.Length) {formattedStrings.Length} . infos[0].HexBytes = {(infos.Length == 0 ? "<EMPTY>" : infos[0].HexBytes)} & formattedStrings[0] = {(formattedStrings.Length == 0 ? "<EMPTY>" : formattedStrings[0])}");
			var res = new object[infos.Length][];
			for (int i = 0; i < infos.Length; i++)
				res[i] = new object[3] { i, infos[i], formattedStrings[i] };
			return res;
		}
	}
}
#endif
