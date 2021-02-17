// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.IO;
using Iced.Intel;
using SG = System.Globalization;

namespace Iced.UnitTests.Intel.DecoderTests {
	static class MemoryDecoderTestParser {
		public static IEnumerable<DecoderMemoryTestCase> ReadFile(int bitness, string filename) {
			int lineNumber = 0;
			foreach (var line in File.ReadLines(filename)) {
				lineNumber++;
				if (line.Length == 0 || line[0] == '#')
					continue;
				DecoderMemoryTestCase testCase;
				try {
					testCase = ReadTestCase(bitness, line, lineNumber);
				}
				catch (Exception ex) {
					throw new InvalidOperationException($"Error parsing decoder test case file '{filename}', line {lineNumber}: {ex.Message}");
				}
				if (testCase is not null)
					yield return testCase;
			}
		}

		static readonly char[] colSep = new char[] { ',' };
		static DecoderMemoryTestCase ReadTestCase(int bitness, string line, int lineNumber) {
			var parts = line.Split(colSep, StringSplitOptions.None);
			if (parts.Length != 11 && parts.Length != 12)
				throw new InvalidOperationException();
			var tc = new DecoderMemoryTestCase();
			tc.LineNumber = lineNumber;
			tc.Bitness = bitness;
			tc.IP = bitness switch {
				16 => DecoderConstants.DEFAULT_IP16,
				32 => DecoderConstants.DEFAULT_IP32,
				64 => DecoderConstants.DEFAULT_IP64,
				_ => throw new InvalidOperationException(),
			};
			tc.HexBytes = parts[0].Trim();
			var code = parts[1].Trim();
			if (CodeUtils.IsIgnored(code))
				return null;
			tc.Code = ToEnumConverter.GetCode(code);
			tc.Register = ToEnumConverter.GetRegister(parts[2].Trim());
			tc.SegmentPrefix = ToEnumConverter.GetRegister(parts[3].Trim());
			tc.SegmentRegister = ToEnumConverter.GetRegister(parts[4].Trim());
			tc.BaseRegister = ToEnumConverter.GetRegister(parts[5].Trim());
			tc.IndexRegister = ToEnumConverter.GetRegister(parts[6].Trim());
			tc.Scale = (int)NumberConverter.ToUInt32(parts[7].Trim());
			tc.Displacement = NumberConverter.ToUInt64(parts[8].Trim());
			tc.DisplacementSize = (int)NumberConverter.ToUInt32(parts[9].Trim());
			var coStr = parts[10].Trim();
			if (!DecoderTestParser.TryParseConstantOffsets(coStr, out tc.ConstantOffsets))
				throw new InvalidOperationException($"Invalid ConstantOffsets: '{coStr}'");
			tc.EncodedHexBytes = parts.Length > 11 ? parts[11].Trim() : tc.HexBytes;
			tc.DecoderOptions = DecoderOptions.None;
			tc.TestOptions = DecoderTestOptions.None;
			return tc;
		}
	}
}
