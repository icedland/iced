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

#if (!NO_GAS_FORMATTER || !NO_INTEL_FORMATTER || !NO_MASM_FORMATTER || !NO_NASM_FORMATTER) && !NO_FORMATTER
using System;
using System.Diagnostics;
using System.Text;

namespace Iced.Intel {
	readonly struct NumberFormatter {
		const int SmallPositiveNumber = 9;

		readonly FormatterOptions formatterOptions;
		readonly StringBuilder sb;
		readonly char[] numberCharArray;

		public NumberFormatter(FormatterOptions formatterOptions) {
			this.formatterOptions = formatterOptions;
			sb = new StringBuilder();
			// We need 64 chars to format the longest number, which is an unsigned 64-bit value in binary
			numberCharArray = new char[64];
		}

		string AddDigitSeparators(string rawNumber, int digitGroupSize, string digitSeparator) {
			Debug.Assert(digitGroupSize > 0);
			Debug.Assert(!string.IsNullOrEmpty(digitSeparator));

			if (rawNumber.Length <= digitGroupSize)
				return rawNumber;

			var sb = this.sb;

			for (int i = 0; i < rawNumber.Length; i++) {
				int d = rawNumber.Length - i;
				if (i != 0 && (d % digitGroupSize) == 0 && rawNumber[i - 1] != '-')
					sb.Append(digitSeparator);
				sb.Append(rawNumber[i]);
			}

			var res = sb.ToString();
			sb.Length = 0;
			return res;
		}

		string ToHexadecimal(ulong value, int digits, bool upper, bool leadingZero) {
			var array = numberCharArray;

			if (digits == 0) {
				digits = 1;
				for (ulong tmp = value; ;) {
					tmp >>= 4;
					if (tmp == 0)
						break;
					digits++;
				}
			}
			Debug.Assert((uint)digits <= (uint)array.Length);

			char hexHigh = upper ? (char)('A' - 10) : (char)('a' - 10);
			int bi = 0;
			if (leadingZero && (int)((value >> ((digits - 1) << 2)) & 0xF) > 9)
				array[bi++] = '0';
			for (int i = 0; i < digits; i++) {
				int digit = (int)((value >> ((digits - i - 1) << 2)) & 0xF);
				if (digit > 9)
					array[bi++] = (char)(digit + hexHigh);
				else
					array[bi++] = (char)(digit + '0');
			}

			return new string(array, 0, bi);
		}

		string ToOctal(ulong value, int digits) {
			var array = numberCharArray;

			if (digits == 0) {
				digits = 1;
				for (ulong tmp = value; ;) {
					tmp >>= 3;
					if (tmp == 0)
						break;
					digits++;
				}
			}
			Debug.Assert((uint)digits <= (uint)array.Length);

			for (int i = 0; i < digits; i++) {
				int digit = (int)((value >> (digits - i - 1) * 3) & 7);
				array[i] = (char)(digit + '0');
			}

			return new string(array, 0, digits);
		}

		string ToBinary(ulong value, int digits) {
			var array = numberCharArray;

			if (digits == 0) {
				digits = 1;
				for (ulong tmp = value; ;) {
					tmp >>= 1;
					if (tmp == 0)
						break;
					digits++;
				}
			}
			Debug.Assert((uint)digits <= (uint)array.Length);

			for (int i = 0; i < digits; i++) {
				int digit = (int)((value >> (digits - i - 1)) & 1);
				array[i] = (char)(digit + '0');
			}

			return new string(array, 0, digits);
		}

		public string FormatUInt8(in NumberFormattingOptions options, byte value) => FormatUnsignedInteger(options, value, 8, options.LeadingZeroes, options.SmallHexNumbersInDecimal);
		public string FormatUInt16(in NumberFormattingOptions options, ushort value) => FormatUnsignedInteger(options, value, 16, options.LeadingZeroes, options.SmallHexNumbersInDecimal);
		public string FormatUInt32(in NumberFormattingOptions options, uint value) => FormatUnsignedInteger(options, value, 32, options.LeadingZeroes, options.SmallHexNumbersInDecimal);
		public string FormatUInt64(in NumberFormattingOptions options, ulong value) => FormatUnsignedInteger(options, value, 64, options.LeadingZeroes, options.SmallHexNumbersInDecimal);

		public string FormatUInt16(in NumberFormattingOptions options, ushort value, bool leadingZeroes) => FormatUnsignedInteger(options, value, 16, leadingZeroes, options.SmallHexNumbersInDecimal);
		public string FormatUInt32(in NumberFormattingOptions options, uint value, bool leadingZeroes) => FormatUnsignedInteger(options, value, 32, leadingZeroes, options.SmallHexNumbersInDecimal);
		public string FormatUInt64(in NumberFormattingOptions options, ulong value, bool leadingZeroes) => FormatUnsignedInteger(options, value, 64, leadingZeroes, options.SmallHexNumbersInDecimal);

		static readonly string[] smallDecimalValues = new string[SmallPositiveNumber + 1] {
			"0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
		};

		string FormatUnsignedInteger(in NumberFormattingOptions options, ulong value, int valueSize, bool leadingZeroes, bool smallHexNumbersInDecimal) {
			string rawNumber;
			string? prefix, suffix;
			int digitGroupSize;
			string? digitSeparator;
			switch (options.NumberBase) {
			case NumberBase.Hexadecimal:
				if (smallHexNumbersInDecimal && value <= SmallPositiveNumber) {
					digitGroupSize = formatterOptions.DecimalDigitGroupSize;
					digitSeparator = options.DigitSeparator;
					prefix = formatterOptions.DecimalPrefix;
					suffix = formatterOptions.DecimalSuffix;
					rawNumber = smallDecimalValues[(int)value];
				}
				else {
					digitGroupSize = options.DigitGroupSize;
					digitSeparator = options.DigitSeparator;
					prefix = options.Prefix;
					suffix = options.Suffix;
					rawNumber = ToHexadecimal(value, leadingZeroes ? (valueSize + 3) >> 2 : 0, options.UpperCaseHex, options.AddLeadingZeroToHexNumbers && string.IsNullOrEmpty(prefix));
				}
				break;

			case NumberBase.Decimal:
				digitGroupSize = options.DigitGroupSize;
				digitSeparator = options.DigitSeparator;
				prefix = options.Prefix;
				suffix = options.Suffix;
				rawNumber = value.ToString();
				break;

			case NumberBase.Octal:
				digitGroupSize = options.DigitGroupSize;
				digitSeparator = options.DigitSeparator;
				prefix = options.Prefix;
				suffix = options.Suffix;
				rawNumber = ToOctal(value, leadingZeroes ? (valueSize + 2) / 3 : 0);
				if (prefix == "0") {
					// The prefix is part of the number so that a digit separator can be placed
					// between the "prefix" and the rest of the number, eg. "0" + "1234" with
					// digit separator "`" and group size = 2 is "0`12`34" and not "012`34".
					// Other prefixes, eg. "0o" prefix: 0o12`34 and never 0o`12`34.
					if (rawNumber[0] != '0')
						rawNumber = prefix + rawNumber;
					prefix = null;
				}
				break;

			case NumberBase.Binary:
				digitGroupSize = options.DigitGroupSize;
				digitSeparator = options.DigitSeparator;
				prefix = options.Prefix;
				suffix = options.Suffix;
				rawNumber = ToBinary(value, leadingZeroes ? valueSize : 0);
				break;

			default:
				throw new InvalidOperationException();
			}

			if (digitGroupSize > 0 && !string2.IsNullOrEmpty(digitSeparator))
				rawNumber = AddDigitSeparators(rawNumber, digitGroupSize, digitSeparator);

			if (!string.IsNullOrEmpty(prefix)) {
				if (!string.IsNullOrEmpty(suffix))
					return prefix + rawNumber + suffix;
				return prefix + rawNumber;
			}
			else if (!string.IsNullOrEmpty(suffix))
				return rawNumber + suffix;
			else
				return rawNumber;
		}
	}
}
#endif
