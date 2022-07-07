// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if FAST_FMT
using Iced.Intel;

namespace Iced.UnitTests.Intel.FormatterTests {
	public abstract class FastOptionsTests {
		protected void FormatBase(int index, OptionsTestCase tc, string formattedString, FastFormatter formatter) {
			tc.Initialize(formatter.Options);
			FormatterTestUtils.SimpleFormatTest(tc.Bitness, tc.HexBytes, tc.IP, tc.Code, tc.DecoderOptions, formattedString, formatter, decoder => tc.Initialize(decoder));
		}
	}
}
#endif
