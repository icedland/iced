// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if GAS || INTEL || MASM || NASM
using System;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests {
	public abstract class OptionsTests {
		protected void FormatBase(int index, OptionsTestCase tc, string formattedString, Formatter formatter) {
			tc.Initialize(formatter.Options);
			FormatterTestUtils.SimpleFormatTest(tc.Bitness, tc.HexBytes, tc.IP, tc.Code, tc.DecoderOptions, formattedString, formatter, decoder => tc.Initialize(decoder));
		}

		protected void TestOptionsBase(FormatterOptions options) {
			{
				int min = int.MaxValue, max = int.MinValue;
				foreach (var value in ToEnumConverter.GetNumberBaseValues()) {
					min = Math.Min(min, (int)value);
					max = Math.Max(max, (int)value);
					options.NumberBase = value;
				}
				Assert.Throws<ArgumentOutOfRangeException>(() => options.NumberBase = (NumberBase)(min - 1));
				Assert.Throws<ArgumentOutOfRangeException>(() => options.NumberBase = (NumberBase)(max + 1));
				Assert.Throws<ArgumentOutOfRangeException>(() => options.NumberBase = (NumberBase)int.MinValue);
				Assert.Throws<ArgumentOutOfRangeException>(() => options.NumberBase = (NumberBase)int.MaxValue);
			}

			{
				int min = int.MaxValue, max = int.MinValue;
				foreach (var value in ToEnumConverter.GetMemorySizeOptionsValues()) {
					min = Math.Min(min, (int)value);
					max = Math.Max(max, (int)value);
					options.MemorySizeOptions = value;
				}
				Assert.Throws<ArgumentOutOfRangeException>(() => options.MemorySizeOptions = (MemorySizeOptions)(min - 1));
				Assert.Throws<ArgumentOutOfRangeException>(() => options.MemorySizeOptions = (MemorySizeOptions)(max + 1));
				Assert.Throws<ArgumentOutOfRangeException>(() => options.MemorySizeOptions = (MemorySizeOptions)int.MinValue);
				Assert.Throws<ArgumentOutOfRangeException>(() => options.MemorySizeOptions = (MemorySizeOptions)int.MaxValue);
			}
		}
	}
}
#endif
