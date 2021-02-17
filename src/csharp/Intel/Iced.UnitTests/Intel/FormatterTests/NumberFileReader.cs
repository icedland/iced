// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

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
