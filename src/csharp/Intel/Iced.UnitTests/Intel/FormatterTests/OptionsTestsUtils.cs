// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if GAS || INTEL || MASM || NASM || FAST_FMT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Iced.UnitTests.Intel.FormatterTests {
	static class OptionsTestsUtils {
		public static IEnumerable<object[]> GetFormatData_Common(string formatterDir, string formattedStringsFile) {
			var (tests, ignored) = FormatterOptionsTests.CommonTests;
			return GetFormatData(formatterDir, formattedStringsFile, tests, ignored);
		}

		public static IEnumerable<object[]> GetFormatData_All(string formatterDir, string formattedStringsFile) {
			var (tests, ignored) = FormatterOptionsTests.AllTests;
			return GetFormatData(formatterDir, formattedStringsFile, tests, ignored);
		}

		public static IEnumerable<object[]> GetFormatData(string formatterDir, string formattedStringsFile, string optionsFile) {
			var testsFilename = FileUtils.GetFormatterFilename(Path.Combine(formatterDir, optionsFile));
			var ignored = new HashSet<int>();
			var tests = OptionsTestsReader.ReadFile(testsFilename, ignored).ToArray();
			return GetFormatData(formatterDir, formattedStringsFile, tests, ignored);
		}

		static IEnumerable<object[]> GetFormatData(string formatterDir, string formattedStringsFile, OptionsTestCase[] tests, HashSet<int> ignored) {
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
