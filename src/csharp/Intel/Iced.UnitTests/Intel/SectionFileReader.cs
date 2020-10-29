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
