// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Generator.Misc.Python {
	static class ParseUtils {
		public static IEnumerable<(string name, string value)> GetArgsNameValues(string argsAttr) {
			if (!ParseUtils.TryGetSignaturePayload(argsAttr, out var args))
				throw new InvalidOperationException($"Invalid #[pyo3(signature = (...))] attr: {argsAttr}");
			foreach (var part in args.Split(',', StringSplitOptions.RemoveEmptyEntries)) {
				int index = part.IndexOf('=', StringComparison.Ordinal);
				if (index < 0)
					continue;
				var name = part[..index].Trim();
				var value = part[(index + 1)..].Trim();
				yield return (name, value);
			}
		}

		public static bool TryRemovePrefixSuffix(string s, string prefix, string suffix, [NotNullWhen(true)] out string? extracted) {
			extracted = null;

			if (!s.StartsWith(prefix, StringComparison.Ordinal))
				return false;
			if (!s.EndsWith(suffix, StringComparison.Ordinal))
				return false;

			extracted = s[prefix.Length..^suffix.Length];
			return true;
		}

		public static string[] SplitSphinxTypes(string sphinxType) =>
			sphinxType.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(a => a.Trim()).ToArray();

		public static bool TryConvertSphinxTypeToTypeName(string sphinxType, [NotNullWhen(true)] out string? typeName) {
			while (true) {
				const string prefix = ":class:`";
				int index = sphinxType.IndexOf(prefix, StringComparison.Ordinal);
				if (index < 0)
					break;
				const string suffix = "`";
				int index2 = sphinxType.IndexOf(suffix, index + prefix.Length, StringComparison.Ordinal);
				if (index2 < 0) {
					typeName = null;
					return false;
				}
				sphinxType = sphinxType[0..index] +
					sphinxType[(index + prefix.Length)..index2] +
					sphinxType[(index2 + suffix.Length)..];
			}
			typeName = sphinxType;
			return true;
		}

		public static bool TryGetSignaturePayload(string argsAttr, [NotNullWhen(true)] out string? args) =>
			TryRemovePrefixSuffix(argsAttr, "#[pyo3(signature = (", "))]", out args);

		public static bool TryParseTypeAndDocs(string argLine, [NotNullWhen(false)] out string? error, out TypeAndDocs result) {
			result = default;

			const string pattern = ": ";
			int index = argLine.IndexOf(pattern, StringComparison.Ordinal);
			if (index < 0) {
				error = "Expected `: `";
				return false;
			}

			var sphinxType = argLine[..index].Trim();
			string documentation = argLine[(index + 1)..].Trim();
			result = new TypeAndDocs(sphinxType, documentation);
			error = null;
			return true;
		}
	}
}
