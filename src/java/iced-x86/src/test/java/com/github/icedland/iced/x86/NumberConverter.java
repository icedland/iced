// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86;

public final class NumberConverter {
	public static long toUInt64(String value) {
		int radix = 10;
		if (value.startsWith("0x")) {
			value = value.substring(2);
			radix = 16;
		}
		try {
			return Long.parseUnsignedLong(value, radix);
		}
		catch (NumberFormatException ex) {
			throw new UnsupportedOperationException(String.format("Invalid number: '%s'", value), ex);
		}
	}

	public static long toInt64(String value) {
		String unsigned_value = value;
		long mult;
		if (unsigned_value.startsWith("-")) {
			mult = -1;
			unsigned_value = unsigned_value.substring(1);
		}
		else
			mult = 1;
		int radix = 10;
		if (unsigned_value.startsWith("0x")) {
			unsigned_value = unsigned_value.substring(2);
			radix = 16;
		}
		long parsedValue;
		try {
			parsedValue = Long.parseUnsignedLong(unsigned_value, radix);
		}
		catch (NumberFormatException ex) {
			throw new UnsupportedOperationException(String.format("Invalid number: '%s'", value), ex);
		}
		if (mult == -1) {
			if (Long.compareUnsigned(parsedValue, 0x8000_0000_0000_0000L) > 0)
				throw new UnsupportedOperationException(String.format("Invalid number: '%s'", value));
		}
		else if (Long.compareUnsigned(parsedValue, 0x7FFF_FFFF_FFFF_FFFFL) > 0)
			throw new UnsupportedOperationException(String.format("Invalid number: '%s'", value));
		return parsedValue * mult;
	}

	public static int toUInt32(String value) {
		long v = toUInt64(value);
		if (Long.compareUnsigned(v, 0xFFFF_FFFFL) <= 0)
			return (int)v;
		throw new UnsupportedOperationException(String.format("Invalid number: '%s'", value));
	}

	public static int toInt32(String value) {
		long v = toInt64(value);
		if (-0x8000_0000 <= v && v <= 0x7FFF_FFFF)
			return (int)v;
		throw new UnsupportedOperationException(String.format("Invalid number: '%s'", value));
	}

	public static short toUInt16(String value) {
		long v = toUInt64(value);
		if (Long.compareUnsigned(v, 0xFFFF) <= 0)
			return (short)v;
		throw new UnsupportedOperationException(String.format("Invalid number: '%s'", value));
	}

	public static short toInt16(String value) {
		long v = toInt64(value);
		if (-0x8000 <= v && v <= 0x7FFF)
			return (short)v;
		throw new UnsupportedOperationException(String.format("Invalid number: '%s'", value));
	}

	public static byte toUInt8(String value) {
		long v = toUInt64(value);
		if (Long.compareUnsigned(v, 0xFF) <= 0)
			return (byte)v;
		throw new UnsupportedOperationException(String.format("Invalid number: '%s'", value));
	}

	public static byte toInt8(String value) {
		long v = toInt64(value);
		if (-0x80 <= v && v <= 0x7F)
			return (byte)v;
		throw new UnsupportedOperationException(String.format("Invalid number: '%s'", value));
	}

	public static boolean toBoolean(String value) {
		switch (value) {
		case "true":
			return true;
		case "false":
			return false;
		default:
			throw new UnsupportedOperationException(String.format("Invalid boolean: '%s'", value));
		}
	}
}
