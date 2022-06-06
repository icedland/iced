// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using Generator.IO;

namespace Generator.Formatters.CSharp {
	sealed class CSharpFastFormatterTableSerializer : FastFormatterTableSerializer {
		readonly string define;
		readonly string @namespace;

		public CSharpFastFormatterTableSerializer(FastFmtInstructionDef[] defs, string define, string @namespace)
			: base(defs, CSharpIdentifierConverter.Create()) {
			this.define = define;
			this.@namespace = @namespace;
		}

		public override string GetFilename(GenTypes genTypes) =>
			CSharpConstants.GetFilename(genTypes, @namespace, "FmtData.g.cs");

		public void Serialize(GenTypes genTypes, FileWriter writer, StringsTable stringsTable) {
			writer.WriteFileHeader();
			writer.WriteLineNoIndent($"#if {define}");
			writer.WriteLine($"namespace {@namespace} {{");
			using (writer.Indent()) {
				writer.WriteLine("static partial class FmtData {");
				using (writer.Indent()) {
					writer.WriteLineNoIndent($"#if {CSharpConstants.HasSpanDefine}");
					writer.WriteLine("static System.ReadOnlySpan<byte> GetSerializedData() =>");
					writer.WriteLineNoIndent("#else");
					writer.WriteLine("static byte[] GetSerializedData() =>");
					writer.WriteLineNoIndent("#endif");
					using (writer.Indent()) {
						writer.WriteLine("new byte[] {");
						using (writer.Indent())
							SerializeTable(genTypes, new TextFileByteTableWriter(writer), stringsTable);
						writer.WriteLine("};");
					}
				}
				writer.WriteLine("}");
			}
			writer.WriteLine("}");
			writer.WriteLineNoIndent("#endif");
		}
	}
}
