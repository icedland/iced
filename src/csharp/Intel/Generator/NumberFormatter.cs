// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;

namespace Generator {
	static class NumberFormatter {
		static string AddNumberSeparator32(string prefix, string number) {
			if (number.Length != 8)
				throw new InvalidOperationException();
			return prefix + number[0..4] + "_" + number[4..];
		}

		static string AddNumberSeparator64(string prefix, string number) {
			if (number.Length != 16)
				throw new InvalidOperationException();
			return prefix + number[0..4] + "_" + number[4..8] + "_" + number[8..12] + "_" + number[12..16];
		}

		public static string FormatHexUInt32WithSep(uint value) => AddNumberSeparator32("0x", value.ToString("X8"));
		public static string FormatHexUInt64WithSep(ulong value) => AddNumberSeparator64("0x", value.ToString("X16"));
	}
}
