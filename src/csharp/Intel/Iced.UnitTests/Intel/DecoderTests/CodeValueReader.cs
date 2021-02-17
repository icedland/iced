// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.IO;
using Iced.Intel;

namespace Iced.UnitTests.Intel.DecoderTests {
	static class CodeValueReader {
		public static HashSet<Code> Read(string name) {
			var filename = PathUtils.GetTestTextFilename(name, "Decoder");
			int lineNumber = 0;
			var hash = new HashSet<Code>();
			foreach (var line in File.ReadLines(filename)) {
				lineNumber++;
				if (line.Length == 0 || line[0] == '#')
					continue;
				var value = line.Trim();
				if (CodeUtils.IsIgnored(value))
					continue;
				if (!ToEnumConverter.TryCode(value, out var code))
					throw new InvalidOperationException($"Error parsing Code file '{filename}', line {lineNumber}: Invalid value: {value}");
				hash.Add(code);
			}
			return hash;
		}
	}
}
