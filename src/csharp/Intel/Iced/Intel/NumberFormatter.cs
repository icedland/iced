// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if GAS || INTEL || MASM || NASM
using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace Iced.Intel {
	[Flags]
	enum NumberFormatterFlags {
		None						= 0,
		AddMinusSign				= 0x00000001,
		LeadingZeros				= 0x00000002,
		SmallHexNumbersInDecimal	= 0x00000004,
	}

	readonly struct NumberFormatter {
		const ulong SmallPositiveNumber = 9;

		readonly StringBuilder sb;

		public NumberFormatter(bool dummy) {
			const int CAP =
				2 +// "0b"
				64 +// 64 bin digits
				(16 - 1);// # digit separators if group size == 4 and digit sep is one char
			sb = new StringBuilder(CAP);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static void ToHexadecimal(StringBuilder sb, ulong value, int digitGroupSize, string? digitSeparator, int digits, bool upper, bool leadingZero) {
			if (digits == 0) {
				digits = 1;
				for (ulong tmp = value; ;) {
					tmp >>= 4;
					if (tmp == 0)
						break;
					digits++;
				}
			}

			int hexHigh = upper ? 'A' - 10 : 'a' - 10;
			if (leadingZero && digits < 17 && (int)((value >> ((digits - 1) << 2)) & 0xF) > 9)
				digits++;// Another 0
			bool useDigitSep = digitGroupSize > 0 && !string.IsNullOrEmpty(digitSeparator);
			for (int i = 0; i < digits; i++) {
				int index = digits - i - 1;
				int digit = index >= 16 ? 0 : (int)((value >> (index << 2)) & 0xF);
				if (digit > 9)
					sb.Append((char)(digit + hexHigh));
				else
					sb.Append((char)(digit + '0'));
				if (useDigitSep && index > 0 && (index % digitGroupSize) == 0)
					sb.Append(digitSeparator!);
			}
		}

		static readonly ulong[] divs = new ulong[] {
			1,
			10,
			100,
			1000,
			10000,
			100000,
			1000000,
			10000000,
			100000000,
			1000000000,
			10000000000,
			100000000000,
			1000000000000,
			10000000000000,
			100000000000000,
			1000000000000000,
			10000000000000000,
			100000000000000000,
			1000000000000000000,
			10000000000000000000,
		};
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static void ToDecimal(StringBuilder sb, ulong value, int digitGroupSize, string? digitSeparator, int digits) {
			if (digits == 0) {
				digits = 1;
				for (ulong tmp = value; ;) {
					tmp /= 10;
					if (tmp == 0)
						break;
					digits++;
				}
			}

			bool useDigitSep = digitGroupSize > 0 && !string2.IsNullOrEmpty(digitSeparator);
			var divs = NumberFormatter.divs;
			for (int i = 0; i < digits; i++) {
				int index = digits - i - 1;
				if ((uint)index < (uint)divs.Length) {
					int digit = (int)(value / divs[index] % 10);
					sb.Append((char)(digit + '0'));
				}
				else
					sb.Append('0');
				if (useDigitSep && index > 0 && (index % digitGroupSize) == 0)
					sb.Append(digitSeparator!);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static void ToOctal(StringBuilder sb, ulong value, int digitGroupSize, string? digitSeparator, int digits, string? prefix) {
			if (digits == 0) {
				digits = 1;
				for (ulong tmp = value; ;) {
					tmp >>= 3;
					if (tmp == 0)
						break;
					digits++;
				}
			}

			if (!string.IsNullOrEmpty(prefix)) {
				// The prefix is part of the number so that a digit separator can be placed
				// between the "prefix" and the rest of the number, eg. "0" + "1234" with
				// digit separator "`" and group size = 2 is "0`12`34" and not "012`34".
				// Other prefixes, eg. "0o" prefix: 0o12`34 and never 0o`12`34.
				if (prefix == "0") {
					if (digits < 23 && (int)((value >> (digits - 1) * 3) & 7) != 0)
						digits++;// Another 0
				}
				else
					sb.Append(prefix);
			}

			bool useDigitSep = digitGroupSize > 0 && !string2.IsNullOrEmpty(digitSeparator);
			for (int i = 0; i < digits; i++) {
				int index = digits - i - 1;
				int digit = index >= 22 ? 0 : (int)((value >> index * 3) & 7);
				sb.Append((char)(digit + '0'));
				if (useDigitSep && index > 0 && (index % digitGroupSize) == 0)
					sb.Append(digitSeparator!);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static void ToBinary(StringBuilder sb, ulong value, int digitGroupSize, string? digitSeparator, int digits) {
			if (digits == 0) {
				digits = 1;
				for (ulong tmp = value; ;) {
					tmp >>= 1;
					if (tmp == 0)
						break;
					digits++;
				}
			}

			bool useDigitSep = digitGroupSize > 0 && !string2.IsNullOrEmpty(digitSeparator);
			for (int i = 0; i < digits; i++) {
				int index = digits - i - 1;
				int digit = index >= 64 ? 0 : (int)((value >> index) & 1);
				sb.Append((char)(digit + '0'));
				if (useDigitSep && index > 0 && (index % digitGroupSize) == 0)
					sb.Append(digitSeparator!);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static NumberFormatterFlags GetFlags(bool leadingZeros, bool smallHexNumbersInDecimal) {
			var flags = NumberFormatterFlags.None;
			if (leadingZeros)
				flags |= NumberFormatterFlags.LeadingZeros;
			if (smallHexNumbersInDecimal)
				flags |= NumberFormatterFlags.SmallHexNumbersInDecimal;
			return flags;
		}

		public string FormatInt8(FormatterOptions formatterOptions, in NumberFormattingOptions options, sbyte value) {
			var flags = GetFlags(options.LeadingZeros, options.SmallHexNumbersInDecimal);
			if (value < 0) {
				flags |= NumberFormatterFlags.AddMinusSign;
				value = (sbyte)-value;
			}
			return FormatUnsignedInteger(formatterOptions, options, (byte)value, 8, flags);
		}

		public string FormatInt16(FormatterOptions formatterOptions, in NumberFormattingOptions options, short value) {
			var flags = GetFlags(options.LeadingZeros, options.SmallHexNumbersInDecimal);
			if (value < 0) {
				flags |= NumberFormatterFlags.AddMinusSign;
				value = (short)-value;
			}
			return FormatUnsignedInteger(formatterOptions, options, (ushort)value, 16, flags);
		}

		public string FormatInt32(FormatterOptions formatterOptions, in NumberFormattingOptions options, int value) {
			var flags = GetFlags(options.LeadingZeros, options.SmallHexNumbersInDecimal);
			if (value < 0) {
				flags |= NumberFormatterFlags.AddMinusSign;
				value = -value;
			}
			return FormatUnsignedInteger(formatterOptions, options, (uint)value, 32, flags);
		}

		public string FormatInt64(FormatterOptions formatterOptions, in NumberFormattingOptions options, long value) {
			var flags = GetFlags(options.LeadingZeros, options.SmallHexNumbersInDecimal);
			if (value < 0) {
				flags |= NumberFormatterFlags.AddMinusSign;
				value = -value;
			}
			return FormatUnsignedInteger(formatterOptions, options, (ulong)value, 64, flags);
		}

		public string FormatUInt8(FormatterOptions formatterOptions, in NumberFormattingOptions options, byte value) => FormatUnsignedInteger(formatterOptions, options, value, 8, GetFlags(options.LeadingZeros, options.SmallHexNumbersInDecimal));
		public string FormatUInt16(FormatterOptions formatterOptions, in NumberFormattingOptions options, ushort value) => FormatUnsignedInteger(formatterOptions, options, value, 16, GetFlags(options.LeadingZeros, options.SmallHexNumbersInDecimal));
		public string FormatUInt32(FormatterOptions formatterOptions, in NumberFormattingOptions options, uint value) => FormatUnsignedInteger(formatterOptions, options, value, 32, GetFlags(options.LeadingZeros, options.SmallHexNumbersInDecimal));
		public string FormatUInt64(FormatterOptions formatterOptions, in NumberFormattingOptions options, ulong value) => FormatUnsignedInteger(formatterOptions, options, value, 64, GetFlags(options.LeadingZeros, options.SmallHexNumbersInDecimal));

		public string FormatDisplUInt8(FormatterOptions formatterOptions, in NumberFormattingOptions options, byte value) => FormatUnsignedInteger(formatterOptions, options, value, 8, GetFlags(options.DisplacementLeadingZeros, options.SmallHexNumbersInDecimal));
		public string FormatDisplUInt16(FormatterOptions formatterOptions, in NumberFormattingOptions options, ushort value) => FormatUnsignedInteger(formatterOptions, options, value, 16, GetFlags(options.DisplacementLeadingZeros, options.SmallHexNumbersInDecimal));
		public string FormatDisplUInt32(FormatterOptions formatterOptions, in NumberFormattingOptions options, uint value) => FormatUnsignedInteger(formatterOptions, options, value, 32, GetFlags(options.DisplacementLeadingZeros, options.SmallHexNumbersInDecimal));
		public string FormatDisplUInt64(FormatterOptions formatterOptions, in NumberFormattingOptions options, ulong value) => FormatUnsignedInteger(formatterOptions, options, value, 64, GetFlags(options.DisplacementLeadingZeros, options.SmallHexNumbersInDecimal));

		public string FormatUInt16(FormatterOptions formatterOptions, in NumberFormattingOptions options, ushort value, bool leadingZeros) => FormatUnsignedInteger(formatterOptions, options, value, 16, GetFlags(leadingZeros, options.SmallHexNumbersInDecimal));
		public string FormatUInt32(FormatterOptions formatterOptions, in NumberFormattingOptions options, uint value, bool leadingZeros) => FormatUnsignedInteger(formatterOptions, options, value, 32, GetFlags(leadingZeros, options.SmallHexNumbersInDecimal));
		public string FormatUInt64(FormatterOptions formatterOptions, in NumberFormattingOptions options, ulong value, bool leadingZeros) => FormatUnsignedInteger(formatterOptions, options, value, 64, GetFlags(leadingZeros, options.SmallHexNumbersInDecimal));

		static readonly string[] smallDecimalValues = new string[(int)SmallPositiveNumber + 1] {
			"0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
		};

		string FormatUnsignedInteger(FormatterOptions formatterOptions, in NumberFormattingOptions options, ulong value, int valueSize, NumberFormatterFlags flags) {
			sb.Clear();
			if ((flags & NumberFormatterFlags.AddMinusSign) != 0)
				sb.Append('-');
			string? suffix;
			switch (options.NumberBase) {
			case NumberBase.Hexadecimal:
				if ((flags & NumberFormatterFlags.SmallHexNumbersInDecimal) != 0 && value <= SmallPositiveNumber) {
					if (string.IsNullOrEmpty(formatterOptions.DecimalPrefix) && string.IsNullOrEmpty(formatterOptions.DecimalSuffix))
						return smallDecimalValues[(int)value];
					if (!string.IsNullOrEmpty(formatterOptions.DecimalPrefix))
						sb.Append(formatterOptions.DecimalPrefix);
					sb.Append(smallDecimalValues[(int)value]);
					suffix = formatterOptions.DecimalSuffix;
				}
				else {
					if (!string.IsNullOrEmpty(options.Prefix))
						sb.Append(options.Prefix);
					ToHexadecimal(sb, value, options.DigitGroupSize, options.DigitSeparator, (flags & NumberFormatterFlags.LeadingZeros) != 0 ? (valueSize + 3) >> 2 : 0, options.UppercaseHex, options.AddLeadingZeroToHexNumbers && string.IsNullOrEmpty(options.Prefix));
					suffix = options.Suffix;
				}
				break;

			case NumberBase.Decimal:
				if (!string.IsNullOrEmpty(options.Prefix))
					sb.Append(options.Prefix);
				ToDecimal(sb, value, options.DigitGroupSize, options.DigitSeparator, 0);
				suffix = options.Suffix;
				break;

			case NumberBase.Octal:
				ToOctal(sb, value, options.DigitGroupSize, options.DigitSeparator, (flags & NumberFormatterFlags.LeadingZeros) != 0 ? (valueSize + 2) / 3 : 0, options.Prefix);
				suffix = options.Suffix;
				break;

			case NumberBase.Binary:
				if (!string.IsNullOrEmpty(options.Prefix))
					sb.Append(options.Prefix);
				ToBinary(sb, value, options.DigitGroupSize, options.DigitSeparator, (flags & NumberFormatterFlags.LeadingZeros) != 0 ? valueSize : 0);
				suffix = options.Suffix;
				break;

			default:
				throw new InvalidOperationException();
			}

			if (!string.IsNullOrEmpty(suffix))
				sb.Append(suffix);

			return sb.ToString();
		}
	}
}
#endif
