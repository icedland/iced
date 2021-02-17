// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if FAST_FMT
using Iced.Intel;

namespace Iced.UnitTests.Intel.FormatterTests {
	public abstract class FastOptionsTests {
		protected void FormatBase(int index, OptionsInstructionInfo info, string formattedString, FastFormatter formatter) {
			info.Initialize(formatter.Options);
			FormatterTestUtils.SimpleFormatTest(info.Bitness, info.HexBytes, info.IP, info.Code, info.DecoderOptions, formattedString, formatter, decoder => info.Initialize(decoder));
		}
	}
}
#endif
