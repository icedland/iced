// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if GAS || INTEL || MASM || NASM || FAST_FMT
using System.Collections.Generic;
using System.Linq;

namespace Iced.UnitTests.Intel.FormatterTests {
	static class FormatterOptionsTests {
		public static readonly (OptionsTestCase[] testCases, HashSet<int> ignored) CommonTests = ReadAllTests("Options.Common.txt", "Formatter");
		public static readonly (OptionsTestCase[] testCases, HashSet<int> ignored) AllTests = ReadAllTests("Options.txt", "Formatter");

		static (OptionsTestCase[] testCases, HashSet<int> ignored) ReadAllTests(string filename, params string[] directories) {
			var optionsFilename = PathUtils.GetTestTextFilename(filename, directories);
			var ignored = new HashSet<int>();
			return (OptionsTestsReader.ReadFile(optionsFilename, ignored).ToArray(), ignored);
		}
	}
}
#endif
