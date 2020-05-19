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
				if (testCase is object)
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
			tc.Scale = (int)ParseUInt32(parts[7].Trim());
			tc.Displacement = ParseUInt32(parts[8].Trim());
			tc.DisplacementSize = (int)ParseUInt32(parts[9].Trim());
			var coStr = parts[10].Trim();
			if (!DecoderTestParser.TryParseConstantOffsets(coStr, out tc.ConstantOffsets))
				throw new InvalidOperationException($"Invalid ConstantOffsets: '{coStr}'");
			tc.EncodedHexBytes = parts.Length > 11 ? parts[11].Trim() : tc.HexBytes;
			tc.DecoderOptions = DecoderOptions.None;
			tc.CanEncode = true;
			return tc;
		}

		static uint ParseUInt32(string s) {
			if (uint.TryParse(s, out uint value))
				return value;
			if (s.StartsWith("0x")) {
				s = s.Substring(2);
				if (uint.TryParse(s, SG.NumberStyles.HexNumber, null, out value))
					return value;
			}

			throw new InvalidOperationException();
		}
	}
}
