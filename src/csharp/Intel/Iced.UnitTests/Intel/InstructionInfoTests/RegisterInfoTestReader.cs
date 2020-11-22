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

#if INSTR_INFO
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Iced.Intel;

namespace Iced.UnitTests.Intel.InstructionInfoTests {
	static class RegisterInfoTestReader {
		static readonly char[] commaSeparator = new char[] { ',' };
		static readonly char[] spaceSeparator = new char[] { ' ' };

		public static RegisterInfoTestCase[] GetTestCases() {
			var result = new RegisterInfoTestCase[IcedConstants.RegisterEnumCount];

			var filename = PathUtils.GetTestTextFilename("RegisterInfo.txt", "InstructionInfo");
			Debug.Assert(File.Exists(filename));
			int lineNo = 0;
			foreach (var line in File.ReadLines(filename)) {
				lineNo++;
				if (line.Length == 0 || line[0] == '#')
					continue;

				RegisterInfoTestCase tc;
				try {
					tc = ParseLine(line, lineNo);
				}
				catch (Exception ex) {
					throw new Exception($"Invalid line {lineNo} ({filename}): {ex.Message}");
				}
				if (result[(int)tc.Register] is not null)
					throw new InvalidOperationException($"Duplicate test, {filename}, line {lineNo}");
				result[(int)tc.Register] = tc;
			}

			var sb = new StringBuilder();
			for (int i = 0; i < result.Length; i++) {
				var tc = result[i];
				if (tc is null) {
					if (sb.Length > 0)
						sb.Append(", ");
					sb.Append(((Register)i).ToString());
				}
			}
			if (sb.Length != 0)
				throw new InvalidOperationException($"Missing tests in {filename}: " + sb.ToString());
			return result!;
		}

		static RegisterInfoTestCase ParseLine(string line, int lineNo) {
			Static.Assert(MiscInstrInfoTestConstants.RegisterElemsPerLine == 7 ? 0 : -1);
			var elems = line.Split(commaSeparator, MiscInstrInfoTestConstants.RegisterElemsPerLine);
			if (elems.Length != MiscInstrInfoTestConstants.RegisterElemsPerLine)
				throw new Exception($"Expected {MiscInstrInfoTestConstants.RegisterElemsPerLine - 1} commas");

			var tc = new RegisterInfoTestCase();
			tc.LineNumber = lineNo;
			tc.Register = ToEnumConverter.GetRegister(elems[0].Trim());
			tc.Number = NumberConverter.ToInt32(elems[1].Trim());
			tc.BaseRegister = ToEnumConverter.GetRegister(elems[2].Trim());
			tc.FullRegister = ToEnumConverter.GetRegister(elems[3].Trim());
			tc.FullRegister32 = ToEnumConverter.GetRegister(elems[4].Trim());
			tc.Size = NumberConverter.ToInt32(elems[5].Trim());
			foreach (var value in elems[6].Split(spaceSeparator, StringSplitOptions.RemoveEmptyEntries)) {
				if (!InstructionInfoDicts.RegisterFlagsTable.TryGetValue(value, out var flags))
					throw new InvalidOperationException($"Invalid flags value: {value}");
				tc.Flags |= flags;
			}
			return tc;
		}
	}
}
#endif
