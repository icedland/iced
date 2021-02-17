// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Generator.Tables {
	static class ParserUtils {
		public static (string key, string value) GetKeyValue(string s) {
			int index = s.IndexOf('=', StringComparison.Ordinal);
			if (index < 0)
				return (s, string.Empty);
			else {
				var key = s[0..index].Trim();
				var value = s[(index + 1)..].Trim();
				return (key, value);
			}
		}

		public static bool TryParseUInt32(string value, out uint result, [NotNullWhen(false)] out string? error) {
			var origValue = value;
			var numberStyles = NumberStyles.None;
			bool bad = false;
			if (value.StartsWith("0x", StringComparison.OrdinalIgnoreCase)) {
				numberStyles |= NumberStyles.HexNumber;
				value = value[2..];
				bad = value.TrimStart() != value;
			}
			if (bad || !uint.TryParse(value, numberStyles, null, out result)) {
				error = $"Invalid uint32: `{origValue}`";
				result = 0;
				return false;
			}

			error = null;
			return true;
		}

		public static bool TryParseInt32(string value, out int result, [NotNullWhen(false)] out string? error) {
			result = 0;
			var origValue = value;
			bool isSigned = value.StartsWith("-", StringComparison.Ordinal);
			if (isSigned)
				value = value[1..].Trim();

			var numberStyles = NumberStyles.None;
			bool bad = false;
			if (value.StartsWith("0x", StringComparison.OrdinalIgnoreCase)) {
				numberStyles |= NumberStyles.HexNumber;
				value = value[2..];
				bad = value.TrimStart() != value;
			}
			if (bad || !uint.TryParse(value, numberStyles, null, out var parsedValue)) {
				error = $"Invalid int32: `{origValue}`";
				return false;
			}

			if (isSigned) {
				if (parsedValue > 0x80000000) {
					error = $"Signed value is too small: {origValue}";
					return false;
				}
				result = -(int)parsedValue;
			}
			else {
				if (parsedValue > int.MaxValue) {
					error = $"Signed value is too big: {origValue}";
					return false;
				}
				result = (int)parsedValue;
			}

			error = null;
			return true;
		}
	}
}
