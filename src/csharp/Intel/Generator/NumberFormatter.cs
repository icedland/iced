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

namespace Generator {
	static class NumberFormatter {
		static string AddNumberSeparator32(string prefix, string number) {
			if (number.Length != 8)
				throw new InvalidOperationException();
			return prefix + number.Substring(0, 4) + "_" + number.Substring(4);
		}

		static string AddNumberSeparator64(string prefix, string number) {
			if (number.Length != 16)
				throw new InvalidOperationException();
			return prefix + number.Substring(0, 4) + "_" + number.Substring(4, 4) + "_" + number.Substring(8, 4) + "_" + number.Substring(12, 4);
		}

		public static string FormatHexUInt32WithSep(uint value) => AddNumberSeparator32("0x", value.ToString("X8"));
		public static string FormatHexUInt64WithSep(ulong value) => AddNumberSeparator64("0x", value.ToString("X16"));
	}
}
