// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Diagnostics;
using System.IO;

namespace Iced.UnitTests.Intel {
	static class SectionFileReader {
		public static void Read(string filename, (string sectionName, Action<string, string> handler)[] sectionInfos) {
			(string sectionName, Action<string, string> handler) currentSectionInfo = default;
			Debug.Assert(File.Exists(filename));
			int lineNo = 0;
			foreach (var line in File.ReadLines(filename)) {
				lineNo++;
				if (line.Length == 0 || line[0] == '#')
					continue;

				try {
					if (line.StartsWith("[", StringComparison.Ordinal)) {
						if (!line.EndsWith("]", StringComparison.Ordinal))
							throw new Exception("Missing ']'");
						var sectionName = line.Substring(1, line.Length - 1 - 1).Trim();
						if (!TryGetSection(sectionInfos, sectionName, out currentSectionInfo))
							throw new Exception($"Unknown section name: {sectionName}");
					}
					else {
						var handler = currentSectionInfo.handler ?? throw new Exception("Missing section");
						handler(currentSectionInfo.sectionName, line);
					}
				}
				catch (Exception ex) {
					throw new Exception($"Invalid line {lineNo} ({filename}): {ex.Message}");
				}
			}
		}

		static bool TryGetSection((string sectionName, Action<string, string> handler)[] sectionInfos, string sectionName, out (string sectionName, Action<string, string> handler) result) {
			foreach (var info in sectionInfos) {
				if (info.sectionName == sectionName) {
					result = info;
					return true;
				}
			}
			result = default;
			return false;
		}
	}
}
