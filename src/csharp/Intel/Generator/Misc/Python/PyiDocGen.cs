// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Generator.Misc.Python {
	// The docs in the *.rs files are in a format understood by sphinx but it's not
	// supported by most Python language servers. This class converts the docs to
	// something that is accepted by most Python language servers.
	//
	// Tested: VSCode(Jedi, Pylance, Microsoft)
	sealed class PyiDocGen {
		readonly List<string> converted = new();

		const string HeaderPrefix = "###";

		public List<string> Convert(DocComments docComments) {
			converted.Clear();

			bool addEmptyLine = false;
			var colDefs = new List<(int start, int length)>();
			var tableLines = new List<List<string>>();
			var columnLengths = new List<int>();
			var sb = new StringBuilder();
			foreach (var section in docComments.Sections) {
				if (addEmptyLine)
					converted.Add(string.Empty);
				addEmptyLine = true;

				switch (section) {
				case TextDocCommentSection text:
					for (int i = 0; i < text.Lines.Length; i++) {
						var line = text.Lines[i];

						// Convert tables to code blocks. They're not converted to
						// markdown tables because Jedi doesn't support tables in
						// tooltips. Instead, we create a text block that will be
						// rendered with a monospaced font.
						if (IsTableLine(line)) {
							GetColumnDefs(colDefs, line);
							if (colDefs.Count < 2)
								throw new InvalidOperationException($"Invalid table, expected at least 2 columns. First line: {line}");

							i++;
							if (!TryGetNextTableLine(text.Lines, ref i, out var tblLine) || IsTableLine(tblLine))
								throw new InvalidOperationException("Invalid table");
							tableLines.Add(GetColumns(colDefs, tblLine));
							if (!TryGetNextTableLine(text.Lines, ref i, out tblLine) || !IsTableLine(tblLine))
								throw new InvalidOperationException("Invalid table");
							while (TryGetNextTableLine(text.Lines, ref i, out tblLine) && !IsTableLine(tblLine))
								tableLines.Add(GetColumns(colDefs, tblLine));

							foreach (var tableCols in tableLines) {
								for (int j = 0; j < tableCols.Count; j++)
									tableCols[j] = FixTableColumn(tableCols[j], j == 0);
							}

							columnLengths.Clear();
							for (int j = 0; j < colDefs.Count; j++)
								columnLengths.Add(0);
							foreach (var tableCols in tableLines) {
								if (tableCols.Count != columnLengths.Count)
									throw new InvalidOperationException();
								for (int j = 0; j < columnLengths.Count; j++)
									columnLengths[j] = Math.Max(columnLengths[j], tableCols[j].Length);
							}
							const int colSepLen = 2;
							for (int j = 0; j < columnLengths.Count - 1; j++)
								columnLengths[j] += colSepLen;

							converted.Add("```text");
							for (int j = 0; j < tableLines.Count; j++) {
								var tableCols = tableLines[j];
								sb.Clear();
								for (int k = 0; k < tableCols.Count; k++) {
									var col = tableCols[k];
									sb.Append(FixCodeBlockLine(col));
									int colLen = columnLengths[k];
									if (col.Length > colLen)
										throw new InvalidOperationException();
									if (k + 1 != tableCols.Count)
										sb.Append(' ', colLen - col.Length);
								}
								converted.Add(sb.ToString());
								if (j == 0) {
									sb.Clear();
									sb.Append('-', columnLengths.Sum() - columnLengths[^1] + tableLines[0][^1].Length);
									converted.Add(sb.ToString());
								}
							}
							converted.Add("```");
						}
						else
							converted.Add(FixDocLine(line));
					}
					break;

				case ArgsDocCommentSection args:
					converted.Add($"{HeaderPrefix} Args:");
					converted.Add(string.Empty);
					foreach (var arg in args.Args)
						converted.Add(FixDocLine($"- `{arg.Name}` ({arg.SphinxType}): {arg.Documentation}"));
					break;

				case RaisesDocCommentSection raises:
					converted.Add($"{HeaderPrefix} Raises:");
					converted.Add(string.Empty);
					foreach (var raise in raises.Raises)
						converted.Add(FixDocLine($"- {raise.SphinxType}: {raise.Documentation}"));
					break;

				case ReturnsDocCommentSection returns:
					converted.Add($"{HeaderPrefix} Returns:");
					converted.Add(string.Empty);
					converted.Add(FixDocLine($"- {returns.Returns.SphinxType}: {returns.Returns.Documentation}"));
					break;

				case NoteDocCommentSection note:
					converted.Add($"{HeaderPrefix} Note:");
					converted.Add(string.Empty);
					foreach (var line in note.Lines)
						converted.Add(FixDocLine($"- {line}"));
					break;

				case WarningDocCommentSection warning:
					converted.Add($"{HeaderPrefix} Warning:");
					converted.Add(string.Empty);
					foreach (var line in warning.Lines)
						converted.Add(FixDocLine($"- {line}"));
					break;

				case TestCodeDocCommentSection testCode:
					converted.Add("```python");
					foreach (var line in testCode.Lines)
						converted.Add(FixCodeBlockLine(line));
					converted.Add("```");
					break;

				case TestOutputDocCommentSection testOutput:
					converted.Add("```text");
					foreach (var line in testOutput.Lines)
						converted.Add(FixCodeBlockLine(line));
					converted.Add("```");
					break;

				default:
					throw new InvalidOperationException();
				}
			}

			return converted;
		}

		static bool IsTableLine(string line) =>
			line.StartsWith("===", StringComparison.Ordinal) &&
			line.EndsWith("===", StringComparison.Ordinal);

		static bool TryGetNextTableLine(string[] strings, ref int i, [NotNullWhen(true)] out string? tblLine) {
			if (i >= strings.Length) {
				tblLine = null;
				return false;
			}
			else {
				tblLine = strings[i];
				i++;
				return true;
			}
		}

		static List<string> GetColumns(List<(int start, int length)> colDefs, string line) {
			var cols = new List<string>();
			for (int i = 0; i < colDefs.Count; i++) {
				bool isLastCol = i + 1 == colDefs.Count;
				var colDef = colDefs[i];
				if (colDef.start > 0 && line[colDef.start - 1] != ' ')
					throw new InvalidOperationException($"Invalid column #{i}, line: {line}");
				if (!isLastCol &&
					colDef.start + colDef.length < line.Length && line[colDef.start + colDef.length] != ' ') {
					throw new InvalidOperationException($"Invalid column #{i}, line: {line}");
				}
				string col;
				if (isLastCol)
					col = line[colDef.start..].Trim();
				else
					col = line.Substring(colDef.start, colDef.length).Trim();
				cols.Add(col);
			}
			return cols;
		}

		static void GetColumnDefs(List<(int start, int length)> colDefs, string line) {
			colDefs.Clear();
			for (int index = 0; ;) {
				while (index < line.Length && line[index] == ' ')
					index++;
				if (index >= line.Length)
					break;
				int startIndex = index;
				index = line.IndexOf(' ', index);
				if (index < 0)
					index = line.Length;
				colDefs.Add((startIndex, index - startIndex));
			}
		}

		static string FixDocLine(string line) {
			line = line.Replace(@"\", @"\\");
			line = line.Replace("\"\"\"", "\\\"\\\"\\\"");
			line = line.Replace(@"``", @"`");
			line = line.Replace(@":class:", string.Empty);
			if (line == "Examples:")
				line = HeaderPrefix + " " + line;
			return line;
		}

		static string FixCodeBlockLine(string line) {
			line = line.Replace(@"\", @"\\");
			line = line.Replace("\"\"\"", "\\\"\\\"\\\"");
			return line;
		}

		static string FixTableColumn(string line, bool isFirst) {
			if (isFirst && line == @"\")
				line = string.Empty;
			line = line.Replace(@"``", @"`");
			line = line.Replace(@":class:", string.Empty);
			return line;
		}
	}
}
