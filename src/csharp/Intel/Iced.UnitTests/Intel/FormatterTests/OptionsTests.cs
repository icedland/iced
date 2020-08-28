/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

#if GAS || INTEL || MASM || NASM
using System;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests {
	public abstract class OptionsTests {
		protected void FormatBase(int index, OptionsInstructionInfo info, string formattedString, Formatter formatter) {
			info.Initialize(formatter.Options);
			FormatterTestUtils.SimpleFormatTest(info.Bitness, info.HexBytes, info.Code, info.DecoderOptions, formattedString, formatter, decoder => info.Initialize(decoder));
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
