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

				writer.Indent(indent);
				writer.WritePartialGeneratedComment();
				write(writer);
				writer.Unindent(indent);

				for (int i = end; i < lines.Length; i++)
					writer.WriteLine(lines[i]);
			}
		}

		(int start, int end, int indent) FindRegion(string[] lines) {
			var beginStr = prefix + "GENERATOR-BEGIN: " + id;
			var endStr = prefix + "GENERATOR-END: " + id;
			int start = FindString(lines, 0, beginStr);
			int end = FindString(lines, start + 1, endStr);
			int startIndex = lines[start].IndexOf(beginStr);
			int endIndex = lines[end].IndexOf(endStr);
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
