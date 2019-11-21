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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Generator.Documentation.CSharp;
using Generator.IO;

namespace Generator.Constants.CSharp {
	sealed class CSharpConstantsGenerator : IConstantsGenerator {
		readonly IdentifierConverter idConverter;
		readonly Dictionary<TypeId, FullConstantsFileInfo> toFullFileInfo;
		readonly Dictionary<TypeId, PartialConstantsFileInfo> toPartialFileInfo;
		readonly CSharpDocCommentWriter docWriter;

		sealed class FullConstantsFileInfo {
			public readonly string Filename;
			public readonly string Namespace;
			public readonly string? Define;

			public FullConstantsFileInfo(string filename, string @namespace, string? define = null) {
				Filename = filename;
				Namespace = @namespace;
				Define = define;
			}
		}

		sealed class PartialConstantsFileInfo {
			public readonly string Id;
			public readonly string Filename;

			public PartialConstantsFileInfo(string id, string filename) {
				Id = id;
				Filename = filename;
			}
		}

		public CSharpConstantsGenerator(GeneratorOptions generatorOptions) {
			idConverter = CSharpIdentifierConverter.Create();
			docWriter = new CSharpDocCommentWriter(idConverter);

			var baseDir = CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.IcedNamespace);
			toFullFileInfo = new Dictionary<TypeId, FullConstantsFileInfo>();
			toFullFileInfo.Add(TypeIds.IcedConstants, new FullConstantsFileInfo(Path.Combine(baseDir, nameof(TypeIds.IcedConstants) + ".g.cs"), CSharpConstants.IcedNamespace));

			toPartialFileInfo = new Dictionary<TypeId, PartialConstantsFileInfo>();
			toPartialFileInfo.Add(TypeIds.DecoderTestParserConstants, new PartialConstantsFileInfo("DecoderTestText", Path.Combine(generatorOptions.CSharpTestsDir, "Intel", "DecoderTests", "DecoderTestParser.cs")));
		}

		public void Generate(ConstantsType constantsType) {
			if (toFullFileInfo.TryGetValue(constantsType.TypeId, out var fullFileInfo))
				WriteFile(fullFileInfo, constantsType);
			else if (toPartialFileInfo.TryGetValue(constantsType.TypeId, out var partialInfo)) {
				if (!(partialInfo is null))
					new FileUpdater(TargetLanguage.CSharp, partialInfo.Id, partialInfo.Filename).Generate(writer => WriteConstants(writer, constantsType));
			}
			else
				throw new InvalidOperationException();
		}

		void WriteFile(FullConstantsFileInfo info, ConstantsType constantsType) {
			using (var writer = new FileWriter(TargetLanguage.CSharp, FileUtils.OpenWrite(info.Filename))) {
				writer.WriteFileHeader();
				if (!(info.Define is null))
					writer.WriteLine($"#if {info.Define}");

				writer.WriteLine($"namespace {info.Namespace} {{");

				if (constantsType.IsPublic && constantsType.IsMissingDocs)
					writer.WriteLine("#pragma warning disable 1591 // Missing XML comment for publicly visible type or member");
				writer.Indent();
				WriteConstants(writer, constantsType);
				writer.Unindent();
				writer.WriteLine("}");

				if (!(info.Define is null))
					writer.WriteLine("#endif");
			}
		}

		void WriteConstants(FileWriter writer, ConstantsType constantsType) {
			docWriter.Write(writer, constantsType.Documentation, constantsType.RawName);
			var pub = constantsType.IsPublic ? "public " : string.Empty;
			writer.WriteLine($"{pub}static class {constantsType.Name(idConverter)} {{");

			writer.Indent();
			foreach (var constant in constantsType.Constants) {
				docWriter.Write(writer, constant.Documentation, constantsType.RawName);
				writer.Write(constant.IsPublic ? "public " : "internal ");
				writer.Write("const ");
				writer.Write(GetType(constant.Kind));
				writer.Write(" ");
				writer.Write(constant.Name(idConverter));
				writer.Write(" = ");
				writer.Write(GetValue(constant));
				writer.WriteLine(";");
			}
			writer.Unindent();

			writer.WriteLine("}");
		}

		string GetType(ConstantKind kind) {
			switch (kind) {
			case ConstantKind.String:
				return "string";
			case ConstantKind.Int32:
				return "int";
			case ConstantKind.UInt32:
				return "uint";
			case ConstantKind.Register:
			case ConstantKind.MemorySize:
				return ConstantsUtils.GetEnumType(kind).Name(idConverter);
			default:
				throw new InvalidOperationException();
			}
		}

		string GetValue(Constant constant) {
			switch (constant.Kind) {
			case ConstantKind.String:
				if (constant.RefValue is string s)
					return "\"" + EscapeStringValue(s) + "\"";
				throw new InvalidOperationException();

			case ConstantKind.Int32:
				return ((int)constant.ValueUInt32).ToString();

			case ConstantKind.Register:
			case ConstantKind.MemorySize:
				return GetValueString(constant);

			default:
				throw new InvalidOperationException();
			}
		}

		static string EscapeStringValue(string s) => s;

		string GetValueString(Constant constant) {
			var enumType = ConstantsUtils.GetEnumType(constant.Kind);
			var enumValue = enumType.Values.First(a => a.Value == constant.ValueUInt32);
			return $"{enumType.Name(idConverter)}.{enumValue.Name(idConverter)}";
		}
	}
}
