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
using System.Collections.Generic;
using System.Linq;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests {
	public abstract class NumberTests {
		protected static IEnumerable<object[]> GetFormatData() {
			var numbers = NumberFileReader.ReadNumberFile(FileUtils.GetFormatterFilename("Number")).ToArray();
			var formattedNumbers = FileUtils.ReadRawStrings("NumberTests").Select(a => {
				var strings = a.Split(',');
				if (strings.Length != numberBases.Length)
					throw new InvalidOperationException($"Invalid line: {a}");
				for (int i = 0; i < strings.Length; i++)
					strings[i] = strings[i].Trim();
				return strings;
			}).ToArray();
			if (numbers.Length != formattedNumbers.Length)
				throw new InvalidOperationException($"Files don't have the same amount of lines: {numbers.Length} != {formattedNumbers.Length}");
			var res = new object[formattedNumbers.Length][];
			for (int i = 0; i < res.Length; i++)
				res[i] = new object[3] { i, numbers[i], formattedNumbers[i] };
			return res;
		}

		static readonly NumberBase[] numberBases = new NumberBase[] {
			NumberBase.Hexadecimal,
			NumberBase.Decimal,
			NumberBase.Octal,
			NumberBase.Binary,
		};
		protected void FormatBase(int index, object number, string[] formattedStrings, Formatter formatter) {
			if (numberBases.Length != ToEnumConverter.NumberBaseCount)
				throw new InvalidOperationException();
			if (formattedStrings.Length != numberBases.Length)
				throw new InvalidOperationException();
			for (int i = 0; i < formattedStrings.Length; i++) {
				var numberBase = numberBases[i];
				formatter.Options.NumberBase = numberBase;

				string actualFormattedString1;
				string actualFormattedString2;
				var numberOptions = NumberFormattingOptions.CreateImmediate(formatter.Options);
				switch (number) {
				case sbyte value:
					actualFormattedString1 = formatter.FormatInt8(value);
					actualFormattedString2 = formatter.FormatInt8(value, numberOptions);
					break;

				case short value:
					actualFormattedString1 = formatter.FormatInt16(value);
					actualFormattedString2 = formatter.FormatInt16(value, numberOptions);
					break;

				case int value:
					actualFormattedString1 = formatter.FormatInt32(value);
					actualFormattedString2 = formatter.FormatInt32(value, numberOptions);
					break;

				case long value:
					actualFormattedString1 = formatter.FormatInt64(value);
					actualFormattedString2 = formatter.FormatInt64(value, numberOptions);
					break;

				case byte value:
					actualFormattedString1 = formatter.FormatUInt8(value);
					actualFormattedString2 = formatter.FormatUInt8(value, numberOptions);
					break;

				case ushort value:
					actualFormattedString1 = formatter.FormatUInt16(value);
					actualFormattedString2 = formatter.FormatUInt16(value, numberOptions);
					break;

				case uint value:
					actualFormattedString1 = formatter.FormatUInt32(value);
					actualFormattedString2 = formatter.FormatUInt32(value, numberOptions);
					break;

				case ulong value:
					actualFormattedString1 = formatter.FormatUInt64(value);
					actualFormattedString2 = formatter.FormatUInt64(value, numberOptions);
					break;

				default:
					throw new InvalidOperationException();
				}
#pragma warning disable xUnit2006 // Do not use invalid string equality check
				// Show the full string without ellipses by using Equal<string>() instead of Equal()
				Assert.Equal<string>(formattedStrings[i], actualFormattedString1);
				Assert.Equal<string>(formattedStrings[i], actualFormattedString2);
#pragma warning restore xUnit2006 // Do not use invalid string equality check
			}
		}
	}
}
#endif
