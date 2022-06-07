// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using Generator.IO;

namespace Generator.Formatters.CSharp {
	struct CSharpStringsTableSerializer {
		readonly StringsTable stringsTable;
		readonly string @namespace;
		readonly string className;
		readonly string preprocessorExpr;

		public CSharpStringsTableSerializer(StringsTable stringsTable, string @namespace, string className, string preprocessorExpr) {
			this.stringsTable = stringsTable;
			this.@namespace = @namespace;
			this.className = className;
			this.preprocessorExpr = preprocessorExpr;
		}

		public void Serialize(FileWriter writer) {
			if (!stringsTable.IsFrozen)
				throw new InvalidOperationException();

			var sortedInfos = stringsTable.Infos;
			int maxStringLength = 0;
			foreach (var info in sortedInfos)
				maxStringLength = Math.Max(maxStringLength, info.String.Length);

			writer.WriteFileHeader();
			if (preprocessorExpr is not null)
				writer.WriteLineNoIndent($"#if {preprocessorExpr}");
			writer.WriteLine($"namespace {@namespace} {{");
			using (writer.Indent()) {
				writer.WriteLine($"static partial class {className} {{");
				using (writer.Indent()) {
					writer.WriteLine($"const int MaxStringLength = {maxStringLength};");
					writer.WriteLine($"const int StringsCount = {sortedInfos.Length};");
					writer.WriteLineNoIndent($"#if {CSharpConstants.HasSpanDefine}");
					writer.WriteLine("static System.ReadOnlySpan<byte> GetSerializedStrings() =>");
					writer.WriteLineNoIndent("#else");
					writer.WriteLine("static byte[] GetSerializedStrings() =>");
					writer.WriteLineNoIndent("#endif");
					using (writer.Indent()) {
						writer.WriteLine("new byte[] {");
						using (writer.Indent())
							StringsTableSerializerUtils.SerializeTable(new TextFileByteTableWriter(writer), sortedInfos);
						writer.WriteLine("};");
					}
				}
				writer.WriteLine("}");
			}
			writer.WriteLine("}");
			if (preprocessorExpr is not null)
				writer.WriteLineNoIndent("#endif");
		}
	}
}
