// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if FAST_FMT
using Iced.Intel;

namespace Iced.UnitTests.Intel.FormatterTests {
	public abstract class FastSymbolResolverTests {
		protected void FormatBase(int index, in SymbolResolverTestCase tc, string formattedString, (FastFormatter formatter, ISymbolResolver symbolResolver) formatterInfo) {
			var tcCopy = tc;
			var formatter = formatterInfo.formatter;
			var decoderOptions = OptionsPropsUtils.GetDecoderOptions(tcCopy.Options);
			OptionsPropsUtils.Initialize(formatter.Options, tcCopy.Options);
			FormatterTestUtils.SimpleFormatTest(tcCopy.Bitness, tcCopy.HexBytes, tcCopy.IP, tcCopy.Code, decoderOptions, formattedString,
				formatter, decoder => OptionsPropsUtils.Initialize(decoder, tcCopy.Options));
		}
	}
}
#endif
