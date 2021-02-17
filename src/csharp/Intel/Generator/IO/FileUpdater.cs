// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.IO;

namespace Generator.IO {
	sealed class FileUpdater {
		readonly TargetLanguage targetLanguage;
		readonly string id;
		readonly string filename;
		readonly string prefix;

		public FileUpdater(TargetLanguage targetLanguage, string id, string filename, string prefix = "// ") {
			this.targetLanguage = targetLanguage;
			this.id = id;
			this.filename = filename;
			this.prefix = prefix;
			if (!File.Exists(filename))
				throw new InvalidOperationException($"File {filename} does not exist");
		}

		public void Generate(Action<FileWriter> write) {
			var lines = File.ReadAllLines(filename);
			var (start, end, indent) = FindRegion(lines);
			using (var writer = new FileWriter(targetLanguage, FileUtils.OpenWrite(filename))) {
				for (int i = 0; i <= start; i++)
					writer.WriteLine(lines[i]);

				using (writer.Indent(indent)) {
					writer.WritePartialGeneratedComment();
					write(writer);
				}

				for (int i = end; i < lines.Length; i++)
					writer.WriteLine(lines[i]);
			}
		}

		(int start, int end, int indent) FindRegion(string[] lines) {
			var beginStr = prefix + "GENERATOR-BEGIN: " + id;
			var endStr = prefix + "GENERATOR-END: " + id;
			int start = FindString(lines, 0, beginStr);
			int end = FindString(lines, start + 1, endStr);
			int startIndex = lines[start].IndexOf(beginStr, StringComparison.Ordinal);
			int endIndex = lines[end].IndexOf(endStr, StringComparison.Ordinal);
			if (startIndex < 0 || startIndex != endIndex)
				throw new InvalidOperationException($"The lines should have the same indentation");
			int indent = GetIndent(startIndex, lines[start]);
			return (start, end, indent);
		}

		static int GetIndent(int index, string s) {
			if ((uint)index > s.Length)
				throw new InvalidOperationException();
			if (index == 0)
				return 0;
			int indentCount;
			int i = 0;
			if (s[i] == '\t') {
				while (i < s.Length && s[i] == '\t')
					i++;
				indentCount = i;
			}
			else if (i < s.Length && s[i] == ' ') {
				while (s[i] == ' ')
					i++;
				const int tabSize = 4;
				if ((i % tabSize) != 0)
					throw new InvalidOperationException();
				indentCount = i / 4;
			}
			else
				throw new InvalidOperationException();
			if (i != index)
				throw new InvalidOperationException("Use only spaces or only tabs");
			return indentCount;
		}

		int FindString(string[] lines, int index, string s) {
			while (index < lines.Length) {
				if (lines[index].Trim() == s)
					return index;
				index++;
			}
			throw new InvalidOperationException($"Couldn't find '{s}' in {filename}");
		}
	}
}
