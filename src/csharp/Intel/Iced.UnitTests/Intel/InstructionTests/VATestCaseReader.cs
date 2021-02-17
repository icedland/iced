// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

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
				if (line.Length == 0 || line[0] == '#')
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
			if (elems.Length != 9)
				throw new Exception($"Invalid number of commas: {elems.Length - 1}");

			var bitness = NumberConverter.ToInt32(elems[0].Trim());
			if (CodeUtils.IsIgnored(elems[1].Trim()))
				return null;
			var hexBytes = elems[2].Trim();
			var operand = NumberConverter.ToInt32(elems[3].Trim());
			var usedMemIndex = NumberConverter.ToInt32(elems[4].Trim());
			var elementIndex = NumberConverter.ToInt32(elems[5].Trim());
			var expectedValue = NumberConverter.ToUInt64(elems[6].Trim());
			var decOptStr = elems[7].Trim();
			var decoderOptions = decOptStr == string.Empty ? DecoderOptions.None : ToEnumConverter.GetDecoderOptions(decOptStr);

			var registerValues = new List<(Register register, int elementIndex, int elementSize, ulong value)>();
			foreach (var tmp in elems[8].Split(spaceSeparator, StringSplitOptions.RemoveEmptyEntries)) {
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

			return new VirtualAddressTestCase(bitness, hexBytes, decoderOptions, operand, usedMemIndex, elementIndex, expectedValue, registerValues.ToArray());
		}
	}
}
