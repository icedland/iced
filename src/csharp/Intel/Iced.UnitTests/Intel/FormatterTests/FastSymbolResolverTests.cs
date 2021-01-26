// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

#if FAST_FMT
using Iced.Intel;

namespace Iced.UnitTests.Intel.FormatterTests {
	public abstract class FastSymbolResolverTests {
		protected void FormatBase(int index, in SymbolResolverTestCase info, string formattedString, (FastFormatter formatter, ISymbolResolver symbolResolver) formatterInfo) {
			var infoCopy = info;
			var formatter = formatterInfo.formatter;
			var decoderOptions = OptionsPropsUtils.GetDecoderOptions(infoCopy.Options);
			OptionsPropsUtils.Initialize(formatter.Options, infoCopy.Options);
			FormatterTestUtils.SimpleFormatTest(infoCopy.Bitness, infoCopy.HexBytes, infoCopy.Code, decoderOptions, formattedString,
				formatter, decoder => OptionsPropsUtils.Initialize(decoder, infoCopy.Options));
		}
	}
}
#endif
