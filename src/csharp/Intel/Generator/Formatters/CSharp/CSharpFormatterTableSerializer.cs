// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using Generator.Enums;
using Generator.IO;

namespace Generator.Formatters.CSharp {
	sealed class CSharpFormatterTableSerializer : FormatterTableSerializer {
		readonly string define;
		readonly string @namespace;

		public CSharpFormatterTableSerializer(FmtInstructionDef[] defs, EnumType ctorKindEnum, string define, string @namespace)
			: base(defs, CSharpIdentifierConverter.Create(), ctorKindEnum["Previous"]) {
			this.define = define;
			this.@namespace = @namespace;
		}

		public override string GetFilename(GenTypes genTypes) =>
			CSharpConstants.GetFilename(genTypes, @namespace, "InstrInfos.g.cs");

		public void Serialize(GenTypes genTypes, FileWriter writer, StringsTable stringsTable) {
			writer.WriteFileHeader();
			writer.WriteLineNoIndent($"#if {define}");
			writer.WriteLine($"namespace {@namespace} {{");
			using (writer.Indent()) {
				writer.WriteLine("static partial class InstrInfos {");
				using (writer.Indent()) {
					writer.WriteLineNoIndent($"#if {CSharpConstants.HasSpanDefine}");
					writer.WriteLine("static System.ReadOnlySpan<byte> GetSerializedInstrInfos() =>");
					writer.WriteLineNoIndent("#else");
					writer.WriteLine("static byte[] GetSerializedInstrInfos() =>");
					writer.WriteLineNoIndent("#endif");
					using (writer.Indent()) {
						writer.WriteLine("new byte[] {");
						using (writer.Indent())
							SerializeTable(new TextFileByteTableWriter(writer), stringsTable);
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
