// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if GAS || INTEL || MASM || NASM
using System.Collections.Generic;
using System.Linq;

namespace Iced.UnitTests.Intel.FormatterTests {
	static class FormatterOptionsTests {
		public static readonly (OptionsInstructionInfo[] testCases, HashSet<int> ignored) CommonInfos = ReadAllInfos("Options.Common.txt", "Formatter");
		public static readonly (OptionsInstructionInfo[] testCases, HashSet<int> ignored) AllInfos = ReadAllInfos("Options.txt", "Formatter");

		static (OptionsInstructionInfo[] testCases, HashSet<int> ignored) ReadAllInfos(string filename, params string[] directories) {
			var optionsFilename = PathUtils.GetTestTextFilename(filename, directories);
			var ignored = new HashSet<int>();
			return (OptionsTestsReader.ReadFile(optionsFilename, ignored).ToArray(), ignored);
		}
	}
}
#endif
