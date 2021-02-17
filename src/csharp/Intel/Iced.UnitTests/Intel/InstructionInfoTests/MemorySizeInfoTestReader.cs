// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if INSTR_INFO
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Iced.Intel;

namespace Iced.UnitTests.Intel.InstructionInfoTests {
	static class MemorySizeInfoTestReader {
		static readonly char[] commaSeparator = new char[] { ',' };
		static readonly char[] spaceSeparator = new char[] { ' ' };

		public static MemorySizeInfoTestCase[] GetTestCases() {
			var result = new MemorySizeInfoTestCase[IcedConstants.MemorySizeEnumCount];

			var filename = PathUtils.GetTestTextFilename("MemorySizeInfo.txt", "InstructionInfo");
			Debug.Assert(File.Exists(filename));
			int lineNo = 0;
			foreach (var line in File.ReadLines(filename)) {
				lineNo++;
				if (line.Length == 0 || line[0] == '#')
					continue;

				MemorySizeInfoTestCase tc;
				try {
					tc = ParseLine(line, lineNo);
				}
				catch (Exception ex) {
					throw new Exception($"Invalid line {lineNo} ({filename}): {ex.Message}");
				}
				if (result[(int)tc.MemorySize] is not null)
					throw new InvalidOperationException($"Duplicate test, {filename}, line {lineNo}");
				result[(int)tc.MemorySize] = tc;
			}

			var sb = new StringBuilder();
			for (int i = 0; i < result.Length; i++) {
				var tc = result[i];
				if (tc is null) {
					if (sb.Length > 0)
						sb.Append(", ");
					sb.Append(((MemorySize)i).ToString());
				}
			}
			if (sb.Length != 0)
				throw new InvalidOperationException($"Missing tests in {filename}: " + sb.ToString());
			return result!;
		}

		static MemorySizeInfoTestCase ParseLine(string line, int lineNo) {
			Static.Assert(MiscInstrInfoTestConstants.MemorySizeElemsPerLine == 6 ? 0 : -1);
			var elems = line.Split(commaSeparator, MiscInstrInfoTestConstants.MemorySizeElemsPerLine);
			if (elems.Length != MiscInstrInfoTestConstants.MemorySizeElemsPerLine)
				throw new Exception($"Expected {MiscInstrInfoTestConstants.MemorySizeElemsPerLine - 1} commas");

			var tc = new MemorySizeInfoTestCase();
			tc.LineNumber = lineNo;
			tc.MemorySize = ToEnumConverter.GetMemorySize(elems[0].Trim());
			tc.Size = NumberConverter.ToInt32(elems[1].Trim());
			tc.ElementSize = NumberConverter.ToInt32(elems[2].Trim());
			tc.ElementType = ToEnumConverter.GetMemorySize(elems[3].Trim());
			tc.ElementCount = NumberConverter.ToInt32(elems[4].Trim());
			foreach (var value in elems[5].Split(spaceSeparator, StringSplitOptions.RemoveEmptyEntries)) {
				if (!InstructionInfoDicts.MemorySizeFlagsTable.TryGetValue(value, out var flags))
					throw new InvalidOperationException($"Invalid flags value: {value}");
				tc.Flags |= flags;
			}
			return tc;
		}
	}
}
#endif
