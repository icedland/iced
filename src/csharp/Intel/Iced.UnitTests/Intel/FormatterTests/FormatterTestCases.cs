// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if GAS || INTEL || MASM || NASM || FAST_FMT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Iced.Intel;

namespace Iced.UnitTests.Intel.FormatterTests {
	public readonly struct FormatterTestCase {
		public readonly int Bitness;
		public readonly string HexBytes;
		public readonly ulong IP;
		public readonly Code Code;
		public readonly DecoderOptions Options;
		public FormatterTestCase(int bitness, string hexBytes, ulong ip, Code code, DecoderOptions options) {
			Bitness = bitness;
			HexBytes = hexBytes;
			IP = ip;
			Code = code;
			Options = options;
		}
	}

	static class FormatterTestCases {
		static (FormatterTestCase[] tests, HashSet<int> ignored) tests16;
		static (FormatterTestCase[] tests, HashSet<int> ignored) tests32;
		static (FormatterTestCase[] tests, HashSet<int> ignored) tests64;
		static (FormatterTestCase[] tests, HashSet<int> ignored) tests16_Misc;
		static (FormatterTestCase[] tests, HashSet<int> ignored) tests32_Misc;
		static (FormatterTestCase[] tests, HashSet<int> ignored) tests64_Misc;

		public static (FormatterTestCase[] tests, HashSet<int> ignored) GetTests(int bitness, bool isMisc) {
			if (isMisc) {
				return bitness switch {
					16 => GetTests(ref tests16_Misc, bitness, isMisc),
					32 => GetTests(ref tests32_Misc, bitness, isMisc),
					64 => GetTests(ref tests64_Misc, bitness, isMisc),
					_ => throw new ArgumentOutOfRangeException(nameof(bitness)),
				};
			}
			else {
				return bitness switch {
					16 => GetTests(ref tests16, bitness, isMisc),
					32 => GetTests(ref tests32, bitness, isMisc),
					64 => GetTests(ref tests64, bitness, isMisc),
					_ => throw new ArgumentOutOfRangeException(nameof(bitness)),
				};
			}
		}

		static (FormatterTestCase[] tests, HashSet<int> ignored) GetTests(ref (FormatterTestCase[] tests, HashSet<int> ignored) data, int bitness, bool isMisc) {
			if (data.tests is null) {
				var filename = "InstructionInfos" + bitness;
				if (isMisc)
					filename += "_Misc";
				data.ignored = new HashSet<int>();
				data.tests = GetTests(filename, bitness, data.ignored).ToArray();
			}
			return data;
		}

		static readonly char[] sep = new[] { ',' };
		static IEnumerable<FormatterTestCase> GetTests(string filename, int bitness, HashSet<int> ignored) {
			int lineNo = 0;
			int testCaseNo = 0;
			filename = FileUtils.GetFormatterFilename(filename);
			foreach (var line in File.ReadLines(filename)) {
				lineNo++;
				if (line.Length == 0 || line[0] == '#')
					continue;
				var parts = line.Split(sep);
				DecoderOptions options;
				if (parts.Length == 2)
					options = default;
				else if (parts.Length == 3) {
					if (!ToEnumConverter.TryDecoderOptions(parts[2].Trim(), out options))
						throw new InvalidOperationException($"Invalid line #{lineNo} in file {filename}");
				}
				else
					throw new InvalidOperationException($"Invalid line #{lineNo} in file {filename}");
				var hexBytes = parts[0].Trim();
				var codeStr = parts[1].Trim();
				var ip = bitness switch {
					16 => DecoderConstants.DEFAULT_IP16,
					32 => DecoderConstants.DEFAULT_IP32,
					64 => DecoderConstants.DEFAULT_IP64,
					_ => throw new InvalidOperationException(),
				};
				if (CodeUtils.IsIgnored(codeStr))
					ignored.Add(testCaseNo);
				else {
					if (!ToEnumConverter.TryCode(codeStr, out var code))
						throw new InvalidOperationException($"Invalid line #{lineNo} in file {filename}");
					yield return new FormatterTestCase(bitness, hexBytes, ip, code, options);
				}
				testCaseNo++;
			}
		}

		public static IEnumerable<object[]> GetFormatData(int bitness, string formatterDir, string formattedStringsFile, bool isMisc = false) {
			var data = GetTests(bitness, isMisc);
			var formattedStrings = FileUtils.ReadRawStrings(Path.Combine(formatterDir, $"Test{bitness}_{formattedStringsFile}")).ToArray();
			return GetFormatData(data.tests, data.ignored, formattedStrings);
		}

		static IEnumerable<object[]> GetFormatData(FormatterTestCase[] tests, HashSet<int> ignored, string[] formattedStrings) {
			formattedStrings = Utils.Filter(formattedStrings, ignored);
			if (tests.Length != formattedStrings.Length)
				throw new ArgumentException($"(tests.Length) {tests.Length} != (formattedStrings.Length) {formattedStrings.Length} . tests[0].HexBytes = {(tests.Length == 0 ? "<EMPTY>" : tests[0].HexBytes)} & formattedStrings[0] = {(formattedStrings.Length == 0 ? "<EMPTY>" : formattedStrings[0])}");
			var res = new object[tests.Length][];
			for (int i = 0; i < tests.Length; i++)
				res[i] = new object[3] { i, tests[i], formattedStrings[i] };
			return res;
		}

		public static IEnumerable<object[]> GetFormatData(int bitness, (string hexBytes, Instruction instruction)[] tests, string formatterDir, string formattedStringsFile) {
			var formattedStrings = FileUtils.ReadRawStrings(Path.Combine(formatterDir, $"Test{bitness}_{formattedStringsFile}")).ToArray();
			return GetFormatData(tests, formattedStrings);
		}

		static IEnumerable<object[]> GetFormatData((string hexBytes, Instruction instruction)[] tests, string[] formattedStrings) {
			if (tests.Length != formattedStrings.Length)
				throw new ArgumentException($"(tests.Length) {tests.Length} != (formattedStrings.Length) {formattedStrings.Length} . tests[0].hexBytes = {(tests.Length == 0 ? "<EMPTY>" : tests[0].hexBytes)} & formattedStrings[0] = {(formattedStrings.Length == 0 ? "<EMPTY>" : formattedStrings[0])}");
			var res = new object[tests.Length][];
			for (int i = 0; i < tests.Length; i++)
				res[i] = new object[3] { i, tests[i].instruction, formattedStrings[i] };
			return res;
		}
	}
}
#endif
