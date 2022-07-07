// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.internal.fmt;

import com.github.icedland.iced.x86.fmt.FormatterOptions;
import com.github.icedland.iced.x86.fmt.NumberBase;
import com.github.icedland.iced.x86.fmt.NumberFormattingOptions;

/**
 * DO NOT USE: INTERNAL API
 *
 * @deprecated Not part of the public API
 */
@Deprecated
public final class NumberFormatter {
	private static final long SMALL_POSITIVE_NUMBER = 9;

	private static final class Flags {
		static final int NONE = 0;
		static final int ADD_MINUS_SIGN = 0x00000001;
		static final int LEADING_ZEROS = 0x00000002;
		static final int SMALL_HEX_NUMBERS_IN_DECIMAL = 0x00000004;
	}

	private final StringBuilder sb;

	public NumberFormatter(boolean dummy) {
		final int CAP = 2 + // "0b"
				64 + // 64 bin digits
				(16 - 1);// # digit separators if group size == 4 and digit sep is one char
		sb = new StringBuilder(CAP);
	}

	private static void toHexadecimal(StringBuilder sb, long value, int digitGroupSize, String digitSeparator, int digits, boolean upper,
			boolean leadingZero) {
		if (digits == 0) {
			digits = 1;
			for (long tmp = value;;) {
				tmp >>>= 4;
				if (tmp == 0)
					break;
				digits++;
			}
		}

		int hexHigh = upper ? 'A' - 10 : 'a' - 10;
		if (leadingZero && digits < 17 && (int)((value >>> ((digits - 1) << 2)) & 0xF) > 9)
			digits++;// Another 0
		boolean useDigitSep = digitGroupSize > 0 && !StringUtils.isNullOrEmpty(digitSeparator);
		for (int i = 0; i < digits; i++) {
			int index = digits - i - 1;
			int digit = index >= 16 ? 0 : (int)((value >>> (index << 2)) & 0xF);
			if (digit > 9)
				sb.append((char)(digit + hexHigh));
			else
				sb.append((char)(digit + '0'));
			if (useDigitSep && index > 0 && (index % digitGroupSize) == 0)
				sb.append(digitSeparator);
		}
	}

	private static final long[] divs = new long[] {
		1L,
		10L,
		100L,
		1000L,
		10000L,
		100000L,
		1000000L,
		10000000L,
		100000000L,
		1000000000L,
		10000000000L,
		100000000000L,
		1000000000000L,
		10000000000000L,
		100000000000000L,
		1000000000000000L,
		10000000000000000L,
		100000000000000000L,
		1000000000000000000L,
		0x8AC7230489E80000L, // 10000000000000000000L
	};

	private static void toDecimal(StringBuilder sb, long value, int digitGroupSize, String digitSeparator, int digits) {
		if (digits == 0) {
			digits = 1;
			for (long tmp = value;;) {
				tmp = Long.divideUnsigned(tmp, 10);
				if (tmp == 0)
					break;
				digits++;
			}
		}

		boolean useDigitSep = digitGroupSize > 0 && !StringUtils.isNullOrEmpty(digitSeparator);
		long[] divs = NumberFormatter.divs;
		for (int i = 0; i < digits; i++) {
			int index = digits - i - 1;
			if (Long.compareUnsigned(index, divs.length) < 0) {
				int digit = (int)(Long.remainderUnsigned(Long.divideUnsigned(value, divs[index]), 10));
				sb.append((char)(digit + '0'));
			}
			else
				sb.append('0');
			if (useDigitSep && index > 0 && (index % digitGroupSize) == 0)
				sb.append(digitSeparator);
		}
	}

	private static void toOctal(StringBuilder sb, long value, int digitGroupSize, String digitSeparator, int digits, String prefix) {
		if (digits == 0) {
			digits = 1;
			for (long tmp = value;;) {
				tmp >>>= 3;
				if (tmp == 0)
					break;
				digits++;
			}
		}

		if (!StringUtils.isNullOrEmpty(prefix)) {
			// The prefix is part of the number so that a digit separator can be placed
			// between the "prefix" and the rest of the number, eg. "0" + "1234" with
			// digit separator "`" and group size = 2 is "0`12`34" and not "012`34".
			// Other prefixes, eg. "0o" prefix: 0o12`34 and never 0o`12`34.
			if (prefix.equals("0")) {
				if (digits < 23 && (int)((value >>> (digits - 1) * 3) & 7) != 0)
					digits++;// Another 0
			}
			else
				sb.append(prefix);
		}

		boolean useDigitSep = digitGroupSize > 0 && !StringUtils.isNullOrEmpty(digitSeparator);
		for (int i = 0; i < digits; i++) {
			int index = digits - i - 1;
			int digit = index >= 22 ? 0 : (int)((value >>> index * 3) & 7);
			sb.append((char)(digit + '0'));
			if (useDigitSep && index > 0 && (index % digitGroupSize) == 0)
				sb.append(digitSeparator);
		}
	}

	private static void toBinary(StringBuilder sb, long value, int digitGroupSize, String digitSeparator, int digits) {
		if (digits == 0) {
			digits = 1;
			for (long tmp = value;;) {
				tmp >>>= 1;
				if (tmp == 0)
					break;
				digits++;
			}
		}

		boolean useDigitSep = digitGroupSize > 0 && !StringUtils.isNullOrEmpty(digitSeparator);
		for (int i = 0; i < digits; i++) {
			int index = digits - i - 1;
			int digit = index >= 64 ? 0 : (int)((value >>> index) & 1);
			sb.append((char)(digit + '0'));
			if (useDigitSep && index > 0 && (index % digitGroupSize) == 0)
				sb.append(digitSeparator);
		}
	}

	private static int getFlags(boolean leadingZeros, boolean smallHexNumbersInDecimal) {
		int flags = Flags.NONE;
		if (leadingZeros)
			flags |= Flags.LEADING_ZEROS;
		if (smallHexNumbersInDecimal)
			flags |= Flags.SMALL_HEX_NUMBERS_IN_DECIMAL;
		return flags;
	}

	public String formatInt8(FormatterOptions formatterOptions, NumberFormattingOptions options, byte value) {
		int flags = getFlags(options.leadingZeros, options.smallHexNumbersInDecimal);
		if (value < 0) {
			flags |= Flags.ADD_MINUS_SIGN;
			value = (byte)-value;
		}
		return formatUnsignedInteger(formatterOptions, options, value & 0xFF, 8, flags);
	}

	public String formatInt16(FormatterOptions formatterOptions, NumberFormattingOptions options, short value) {
		int flags = getFlags(options.leadingZeros, options.smallHexNumbersInDecimal);
		if (value < 0) {
			flags |= Flags.ADD_MINUS_SIGN;
			value = (short)-value;
		}
		return formatUnsignedInteger(formatterOptions, options, value & 0xFFFF, 16, flags);
	}

	public String formatInt32(FormatterOptions formatterOptions, NumberFormattingOptions options, int value) {
		int flags = getFlags(options.leadingZeros, options.smallHexNumbersInDecimal);
		if (value < 0) {
			flags |= Flags.ADD_MINUS_SIGN;
			value = -value;
		}
		return formatUnsignedInteger(formatterOptions, options, value & 0xFFFF_FFFFL, 32, flags);
	}

	public String formatInt64(FormatterOptions formatterOptions, NumberFormattingOptions options, long value) {
		int flags = getFlags(options.leadingZeros, options.smallHexNumbersInDecimal);
		if (value < 0) {
			flags |= Flags.ADD_MINUS_SIGN;
			value = -value;
		}
		return formatUnsignedInteger(formatterOptions, options, value, 64, flags);
	}

	public String formatUInt8(FormatterOptions formatterOptions, NumberFormattingOptions options, byte value) {
		return formatUnsignedInteger(formatterOptions, options, value & 0xFF, 8, getFlags(options.leadingZeros, options.smallHexNumbersInDecimal));
	}

	public String formatUInt16(FormatterOptions formatterOptions, NumberFormattingOptions options, short value) {
		return formatUnsignedInteger(formatterOptions, options, value & 0xFFFF, 16, getFlags(options.leadingZeros, options.smallHexNumbersInDecimal));
	}

	public String formatUInt32(FormatterOptions formatterOptions, NumberFormattingOptions options, int value) {
		return formatUnsignedInteger(formatterOptions, options, value & 0xFFFF_FFFFL, 32,
				getFlags(options.leadingZeros, options.smallHexNumbersInDecimal));
	}

	public String formatUInt64(FormatterOptions formatterOptions, NumberFormattingOptions options, long value) {
		return formatUnsignedInteger(formatterOptions, options, value, 64, getFlags(options.leadingZeros, options.smallHexNumbersInDecimal));
	}

	public String formatDisplUInt8(FormatterOptions formatterOptions, NumberFormattingOptions options, byte value) {
		return formatUnsignedInteger(formatterOptions, options, value & 0xFF, 8,
				getFlags(options.displacementLeadingZeros, options.smallHexNumbersInDecimal));
	}

	public String formatDisplUInt16(FormatterOptions formatterOptions, NumberFormattingOptions options, short value) {
		return formatUnsignedInteger(formatterOptions, options, value & 0xFFFF, 16,
				getFlags(options.displacementLeadingZeros, options.smallHexNumbersInDecimal));
	}

	public String formatDisplUInt32(FormatterOptions formatterOptions, NumberFormattingOptions options, int value) {
		return formatUnsignedInteger(formatterOptions, options, value & 0xFFFF_FFFFL, 32,
				getFlags(options.displacementLeadingZeros, options.smallHexNumbersInDecimal));
	}

	public String formatDisplUInt64(FormatterOptions formatterOptions, NumberFormattingOptions options, long value) {
		return formatUnsignedInteger(formatterOptions, options, value, 64,
				getFlags(options.displacementLeadingZeros, options.smallHexNumbersInDecimal));
	}

	public String formatUInt16(FormatterOptions formatterOptions, NumberFormattingOptions options, short value, boolean leadingZeros) {
		return formatUnsignedInteger(formatterOptions, options, value & 0xFFFF, 16, getFlags(leadingZeros, options.smallHexNumbersInDecimal));
	}

	public String formatUInt32(FormatterOptions formatterOptions, NumberFormattingOptions options, int value, boolean leadingZeros) {
		return formatUnsignedInteger(formatterOptions, options, value & 0xFFFF_FFFFL, 32, getFlags(leadingZeros, options.smallHexNumbersInDecimal));
	}

	public String formatUInt64(FormatterOptions formatterOptions, NumberFormattingOptions options, long value, boolean leadingZeros) {
		return formatUnsignedInteger(formatterOptions, options, value, 64, getFlags(leadingZeros, options.smallHexNumbersInDecimal));
	}

	private static final String[] smallDecimalValues = new String[] {
		"0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
	};

	private String formatUnsignedInteger(FormatterOptions formatterOptions, NumberFormattingOptions options, long value, int valueSize, int flags) {
		sb.setLength(0);
		if ((flags & Flags.ADD_MINUS_SIGN) != 0)
			sb.append('-');
		String suffix;
		switch (options.numberBase) {
		case NumberBase.HEXADECIMAL:
			if ((flags & Flags.SMALL_HEX_NUMBERS_IN_DECIMAL) != 0 && Long.compareUnsigned(value, SMALL_POSITIVE_NUMBER) <= 0) {
				if (StringUtils.isNullOrEmpty(formatterOptions.getDecimalPrefix()) && StringUtils.isNullOrEmpty(formatterOptions.getDecimalSuffix()))
					return smallDecimalValues[(int)value];
				if (!StringUtils.isNullOrEmpty(formatterOptions.getDecimalPrefix()))
					sb.append(formatterOptions.getDecimalPrefix());
				sb.append(smallDecimalValues[(int)value]);
				suffix = formatterOptions.getDecimalSuffix();
			}
			else {
				if (!StringUtils.isNullOrEmpty(options.prefix))
					sb.append(options.prefix);
				toHexadecimal(sb, value, options.digitGroupSize, options.digitSeparator,
						(flags & Flags.LEADING_ZEROS) != 0 ? (valueSize + 3) >>> 2 : 0, options.uppercaseHex,
						options.addLeadingZeroToHexNumbers && StringUtils.isNullOrEmpty(options.prefix));
				suffix = options.suffix;
			}
			break;

		case NumberBase.DECIMAL:
			if (!StringUtils.isNullOrEmpty(options.prefix))
				sb.append(options.prefix);
			toDecimal(sb, value, options.digitGroupSize, options.digitSeparator, 0);
			suffix = options.suffix;
			break;

		case NumberBase.OCTAL:
			toOctal(sb, value, options.digitGroupSize, options.digitSeparator, (flags & Flags.LEADING_ZEROS) != 0 ? (valueSize + 2) / 3 : 0,
					options.prefix);
			suffix = options.suffix;
			break;

		case NumberBase.BINARY:
			if (!StringUtils.isNullOrEmpty(options.prefix))
				sb.append(options.prefix);
			toBinary(sb, value, options.digitGroupSize, options.digitSeparator, (flags & Flags.LEADING_ZEROS) != 0 ? valueSize : 0);
			suffix = options.suffix;
			break;

		default:
			throw new UnsupportedOperationException();
		}

		if (!StringUtils.isNullOrEmpty(suffix))
			sb.append(suffix);

		return sb.toString();
	}
}
