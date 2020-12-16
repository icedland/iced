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

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Generator.Misc.Python {
	static class ParseUtils {
		public static IEnumerable<(string name, string value)> GetArgsNameValues(string argsAttr) {
			if (!ParseUtils.TryGetArgsPayload(argsAttr, out var args))
				throw new InvalidOperationException($"Invalid #[args] attr: {argsAttr}");
			foreach (var part in args.Split(',', StringSplitOptions.RemoveEmptyEntries)) {
				int index = part.IndexOf('=', StringComparison.Ordinal);
				if (index < 0)
					throw new InvalidOperationException();
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

		public static bool TryGetSphinxTypeToTypeName(string sphinxType, [NotNullWhen(true)] out string? typeName) =>
			ParseUtils.TryRemovePrefixSuffix(sphinxType, ":class:`", "`", out typeName);

		public static bool TryGetArgsPayload(string argsAttr, [NotNullWhen(true)] out string? args) =>
			TryRemovePrefixSuffix(argsAttr, "#[args(", ")]", out args);

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
