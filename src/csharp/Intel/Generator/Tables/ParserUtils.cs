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

using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Generator.Tables {
	static class ParserUtils {
		public static (string key, string value) GetKeyValue(string s) {
			int index = s.IndexOf('=');
			if (index < 0)
				return (s, string.Empty);
			else {
				var key = s.Substring(0, index).Trim();
				var value = s.Substring(index + 1).Trim();
				return (key, value);
			}
		}

		public static bool TryParseUInt32(string value, out uint result, [NotNullWhen(false)] out string? error) {
			var origValue = value;
			var numberStyles = NumberStyles.None;
			bool bad = false;
			if (value.StartsWith("0x", System.StringComparison.OrdinalIgnoreCase)) {
				numberStyles |= NumberStyles.HexNumber;
				value = value.Substring(2);
				bad = value.TrimStart() != value;
			}
			if (bad || !uint.TryParse(value, numberStyles, null, out result)) {
				error = $"Invalid uint: `{origValue}`";
				result = 0;
				return false;
			}

			error = null;
			return true;
		}
	}
}
