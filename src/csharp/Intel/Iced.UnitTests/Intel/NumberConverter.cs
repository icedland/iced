// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using SG = System.Globalization;

namespace Iced.UnitTests.Intel {
	static class NumberConverter {
		public static ulong ToUInt64(string value) {
			if (value.StartsWith("0x", StringComparison.Ordinal)) {
				value = value.Substring(2);
				if (ulong.TryParse(value, SG.NumberStyles.HexNumber, null, out var number))
					return number;
			}
			else if (ulong.TryParse(value, out var number))
				return number;
			throw new InvalidOperationException($"Invalid number: '{value}'");
		}

		public static long ToInt64(string value) {
			var unsigned_value = value;
			long mult;
			if (unsigned_value.StartsWith("-", StringComparison.Ordinal)) {
				mult = -1;
				unsigned_value = unsigned_value.Substring(1);
			}
			else
				mult = 1;
			if (unsigned_value.StartsWith("0x", StringComparison.Ordinal)) {
				unsigned_value = unsigned_value.Substring(2);
				if (long.TryParse(unsigned_value, SG.NumberStyles.HexNumber, null, out var number))
					return number * mult;
			}
			else if (long.TryParse(unsigned_value, out var number))
				return number * mult;
			throw new InvalidOperationException($"Invalid number: '{value}'");
		}

		public static uint ToUInt32(string value) {
			ulong v = ToUInt64(value);
			if (v <= uint.MaxValue)
				return (uint)v;
			throw new InvalidOperationException($"Invalid number: '{value}'");
		}

		public static int ToInt32(string value) {
			long v = ToInt64(value);
			if (int.MinValue <= v && v <= int.MaxValue)
				return (int)v;
			throw new InvalidOperationException($"Invalid number: '{value}'");
		}

		public static ushort ToUInt16(string value) {
			ulong v = ToUInt64(value);
			if (v <= ushort.MaxValue)
				return (ushort)v;
			throw new InvalidOperationException($"Invalid number: '{value}'");
		}

		public static short ToInt16(string value) {
			long v = ToInt64(value);
			if (short.MinValue <= v && v <= short.MaxValue)
				return (short)v;
			throw new InvalidOperationException($"Invalid number: '{value}'");
		}

		public static byte ToUInt8(string value) {
			ulong v = ToUInt64(value);
			if (v <= byte.MaxValue)
				return (byte)v;
			throw new InvalidOperationException($"Invalid number: '{value}'");
		}

		public static sbyte ToInt8(string value) {
			long v = ToInt64(value);
			if (sbyte.MinValue <= v && v <= sbyte.MaxValue)
				return (sbyte)v;
			throw new InvalidOperationException($"Invalid number: '{value}'");
		}

		public static bool ToBoolean(string value) =>
			value switch {
				"true" => true,
				"false" => false,
				_ => throw new InvalidOperationException($"Invalid boolean: '{value}'"),
			};
	}
}
