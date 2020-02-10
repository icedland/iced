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

#if GAS || INTEL || MASM || NASM
using System;
using System.Collections.Generic;
using System.IO;

namespace Iced.UnitTests.Intel.FormatterTests {
	static class NumberFileReader {
		public static IEnumerable<object> ReadNumberFile(string filename) {
			int lineNo = 0;
			foreach (var line in File.ReadLines(FileUtils.GetFormatterFilename("Number"))) {
				lineNo++;
				if (line.Length == 0 || line[0] == '#')
					continue;
				object testCase;
				try {
					testCase = ReadTestCase(line, lineNo);
				}
				catch (Exception ex) {
					throw new InvalidOperationException($"Error parsing number test case file '{filename}', line {lineNo}: {ex.Message}");
				}
				yield return testCase;
			}
		}

		static readonly char[] seps = new char[] { ',' };
		static object ReadTestCase(string line, int lineNo) {
			var parts = line.Split(seps);
			if (parts.Length != 2)
				throw new InvalidOperationException($"Invalid number of commas ({parts.Length - 1} commas)");

			var valueStr = parts[1].Trim();
			switch (parts[0].Trim()) {
			case "i8": return NumberConverter.ToInt8(valueStr);
			case "u8": return NumberConverter.ToUInt8(valueStr);
			case "i16": return NumberConverter.ToInt16(valueStr);
			case "u16": return NumberConverter.ToUInt16(valueStr);
			case "i32": return NumberConverter.ToInt32(valueStr);
			case "u32": return NumberConverter.ToUInt32(valueStr);
			case "i64": return NumberConverter.ToInt64(valueStr);
			case "u64": return NumberConverter.ToUInt64(valueStr);
			default: throw new InvalidOperationException($"Invalid type: {parts[0]}");
			}
		}
	}
}
#endif
