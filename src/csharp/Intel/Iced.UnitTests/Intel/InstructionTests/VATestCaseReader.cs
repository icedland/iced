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
using System.Diagnostics;
using System.IO;
using Iced.Intel;

namespace Iced.UnitTests.Intel.InstructionTests {
	static class VATestCaseReader {
		public static IEnumerable<VirtualAddressTestCase> ReadFile(string filename) {
			Debug.Assert(File.Exists(filename));
			int lineNo = 0;
			foreach (var line in File.ReadLines(filename)) {
				lineNo++;
				if (line.Length == 0 || line.StartsWith("#"))
					continue;

				VirtualAddressTestCase? tc;
				try {
					tc = ParseLine(line);
				}
				catch (Exception ex) {
					throw new Exception($"Invalid line {lineNo} ({filename}): {ex.Message}");
				}
				if (tc.HasValue)
					yield return tc.GetValueOrDefault();
			}

		}

		static readonly char[] commaSeparator = new char[] { ',' };
		static readonly char[] spaceSeparator = new char[] { ' ' };
		static readonly char[] equalSeparator = new char[] { '=' };
		static readonly char[] semicolonSeparator = new char[] { ';' };
		static VirtualAddressTestCase? ParseLine(string line) {
			var elems = line.Split(commaSeparator);
			if (elems.Length != 7)
				throw new Exception($"Invalid number of commas: {elems.Length - 1}");

			var bitness = NumberConverter.ToInt32(elems[0].Trim());
			if (CodeUtils.IsIgnored(elems[1].Trim()))
				return null;
			var hexBytes = elems[2].Trim();
			var operand = NumberConverter.ToInt32(elems[3].Trim());
			var elementIndex = NumberConverter.ToInt32(elems[4].Trim());
			var expectedValue = NumberConverter.ToUInt64(elems[5].Trim());

			var registerValues = new List<(Register register, int elementIndex, int elementSize, ulong value)>();
			foreach (var tmp in elems[6].Split(spaceSeparator, StringSplitOptions.RemoveEmptyEntries)) {
				var kv = tmp.Split(equalSeparator);
				if (kv.Length != 2)
					throw new Exception($"Expected key=value: {tmp}");
				var key = kv[0];
				var valueStr = kv[1];

				Register register;
				int expectedElementIndex;
				int expectedElementSize;
				if (key.IndexOf(';') >= 0) {
					var parts = key.Split(semicolonSeparator);
					if (parts.Length != 3)
						throw new Exception($"Invalid number of semicolons: {parts.Length - 1}");
					register = ToEnumConverter.GetRegister(parts[0]);
					expectedElementIndex = NumberConverter.ToInt32(parts[1]);
					expectedElementSize = NumberConverter.ToInt32(parts[2]);
				}
				else {
					register = ToEnumConverter.GetRegister(key);
					expectedElementIndex = 0;
					expectedElementSize = 0;
				}
				ulong value = NumberConverter.ToUInt64(valueStr);
				registerValues.Add((register, expectedElementIndex, expectedElementSize, value));
			}

			return new VirtualAddressTestCase(bitness, hexBytes, operand, elementIndex, expectedValue, registerValues.ToArray());
		}
	}
}
