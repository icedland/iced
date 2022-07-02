// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if GAS || INTEL || MASM || NASM || FAST_FMT
using System;
using System.Collections.Generic;
using System.IO;
using Iced.Intel;

namespace Iced.UnitTests.Intel.FormatterTests {
	static class OptionsTestsReader {
		public static IEnumerable<OptionsTestCase> ReadFile(string filename, HashSet<int> ignored) {
			int lineNo = 0;
			int testCaseNo = 0;
			foreach (var line in File.ReadLines(filename)) {
				lineNo++;
				if (line.Length == 0 || line[0] == '#')
					continue;
				OptionsTestCase? testCase;
				try {
					testCase = ReadTestCase(line, lineNo);
				}
				catch (Exception ex) {
					throw new InvalidOperationException($"Error parsing options test case file '{filename}', line {lineNo}: {ex.Message}");
				}
				if (testCase.HasValue)
					yield return testCase.GetValueOrDefault();
				else
					ignored.Add(testCaseNo);
				testCaseNo++;
			}
		}

		static readonly char[] seps = new char[] { ',' };
		static readonly char[] optsseps = new char[] { ' ' };
		static OptionsTestCase? ReadTestCase(string line, int lineNo) {
			var parts = line.Split(seps);
			if (parts.Length != 4)
				throw new InvalidOperationException($"Invalid number of commas ({parts.Length - 1} commas)");

			int bitness = NumberConverter.ToInt32(parts[0].Trim());
			var ip = bitness switch {
				16 => DecoderConstants.DEFAULT_IP16,
				32 => DecoderConstants.DEFAULT_IP32,
				64 => DecoderConstants.DEFAULT_IP64,
				_ => throw new InvalidOperationException(),
			};
			var hexBytes = parts[1].Trim();
			HexUtils.ToByteArray(hexBytes);
			var codeStr = parts[2].Trim();
			if (CodeUtils.IsIgnored(codeStr))
				return null;
			var code = ToCode(codeStr);

			var properties = new List<(OptionsProps property, object value)>();
			foreach (var part in parts[3].Split(optsseps, StringSplitOptions.RemoveEmptyEntries))
				properties.Add(OptionsParser.ParseOption(part));

			return new OptionsTestCase(bitness, hexBytes, ip, code, properties);
		}

		static Code ToCode(string value) {
			if (!ToEnumConverter.TryCode(value, out var code))
				throw new InvalidOperationException($"Invalid Code value: '{value}'");
			return code;
		}
	}
}
#endif
