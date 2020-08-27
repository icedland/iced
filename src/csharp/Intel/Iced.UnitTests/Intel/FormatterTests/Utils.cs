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

#if GAS || INTEL || MASM || NASM || FAST_FMT
using System;
using System.Collections.Generic;
using Iced.Intel;

namespace Iced.UnitTests.Intel.FormatterTests {
	static class Utils {
		public static IEnumerable<Formatter> GetAllFormatters() {
#if GAS
			yield return new GasFormatter();
#endif
#if INTEL
			yield return new IntelFormatter();
#endif
#if MASM
			yield return new MasmFormatter();
#endif
#if NASM
			yield return new NasmFormatter();
#endif
		}

		public static string[] Filter(string[] strings, HashSet<int> removed) {
			if (removed.Count == 0)
				return strings;
			var res = new string[strings.Length - removed.Count];
			int w = 0;
			for (int i = 0; i < strings.Length; i++) {
				if (!removed.Contains(i))
					res[w++] = strings[i];
			}
			if (w != res.Length)
				throw new InvalidOperationException();
			return res;
		}
	}
}
#endif
