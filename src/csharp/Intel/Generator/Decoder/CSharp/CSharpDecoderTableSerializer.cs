// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using Generator.IO;

namespace Generator.Decoder.CSharp {
	sealed class CSharpDecoderTableSerializer : DecoderTableSerializer {
		public string ClassName { get; }

		public CSharpDecoderTableSerializer(GenTypes genTypes, string className, DecoderTableSerializerInfo info)
			: base(genTypes, CSharpIdentifierConverter.Create(), info) => ClassName = className;

		public void Serialize(FileWriter writer) {
			writer.WriteFileHeader();
			writer.WriteLineNoIndent($"#if {info.Define}");
			writer.WriteLine($"namespace {CSharpConstants.DecoderNamespace} {{");
			using (writer.Indent()) {
				writer.WriteLine($"static partial class {ClassName} {{");
				using (writer.Indent()) {
					writer.WriteLineNoIndent($"#if {CSharpConstants.HasSpanDefine}");
					writer.WriteLine("static System.ReadOnlySpan<byte> GetSerializedTables() =>");
					writer.WriteLineNoIndent("#else");
					writer.WriteLine("static byte[] GetSerializedTables() =>");
					writer.WriteLineNoIndent("#endif");
					using (writer.Indent()) {
						writer.WriteLine("new byte[] {");
						using (writer.Indent())
							SerializeCore(new TextFileByteTableWriter(writer));
						writer.WriteLine("};");
					}

					writer.WriteLine($"const int MaxIdNames = {info.TablesToSerialize.Length};");
					foreach (var name in info.TableIndexNames) {
						var constName = idConverter.Constant($"{name}Index");
						writer.WriteLine($"const uint {constName} = {GetInfo(name).Index};");
					}

				}
				writer.WriteLine("}");
			}
			writer.WriteLine("}");
			writer.WriteLineNoIndent("#endif");
		}
	}
}
