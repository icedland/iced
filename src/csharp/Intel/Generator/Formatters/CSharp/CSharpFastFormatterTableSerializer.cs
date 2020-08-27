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

using System.IO;
using Generator.IO;

namespace Generator.Formatters.CSharp {
	sealed class CSharpFastFormatterTableSerializer : FastFormatterTableSerializer {
		readonly string define;
		readonly string @namespace;

		public CSharpFastFormatterTableSerializer(object[][] infos, string define, string @namespace)
			: base(infos, CSharpIdentifierConverter.Create()) {
			this.define = define;
			this.@namespace = @namespace;
		}

		public override string GetFilename(GeneratorContext generatorContext) =>
			Path.Combine(CSharpConstants.GetDirectory(generatorContext, @namespace), "FmtData.g.cs");

		public override void Serialize(GenTypes genTypes, FileWriter writer, StringsTable stringsTable) {
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
							SerializeTable(genTypes, writer, stringsTable);
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
