// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if FAST_FMT
using Iced.Intel;

namespace Iced.UnitTests.Intel.FormatterTests.Fast {
	public abstract class FastFormatterTest {
		protected void FormatBase(int index, FormatterTestCase tc, string formattedString, FastFormatter formatter) =>
			FormatterTestUtils.FormatTest(tc.Bitness, tc.HexBytes, tc.IP, tc.Code, tc.Options, formattedString, formatter);

		protected void FormatBase(int index, Instruction instruction, string formattedString, FastFormatter formatter) =>
			FormatterTestUtils.FormatTest(instruction, formattedString, formatter);
	}
}
#endif
