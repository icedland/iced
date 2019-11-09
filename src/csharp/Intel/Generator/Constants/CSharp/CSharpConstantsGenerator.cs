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
using System.Text;
using Generator.Documentation.CSharp;
using Generator.Enums;
using Generator.IO;

namespace Generator.Constants.CSharp {
	sealed class CSharpConstantsGenerator : IConstantsGenerator {
		readonly Dictionary<ConstantsTypeKind, FullEnumFileInfo> toFullFileInfo;
		readonly CSharpDocCommentWriter docWriter;

		sealed class FullEnumFileInfo {
			public readonly string Filename;
			public readonly string Namespace;
			public readonly string? Define;

			public FullEnumFileInfo(string filename, string @namespace, string? define = null) {
				Filename = filename;
				Namespace = @namespace;
				Define = define;
			}
		}

		public CSharpConstantsGenerator(ProjectDirs projectDirs) {
			docWriter = new CSharpDocCommentWriter();

			var baseDir = CSharpConstants.GetDirectory(projectDirs, CSharpConstants.IcedNamespace);
			toFullFileInfo = new Dictionary<ConstantsTypeKind, FullEnumFileInfo>();
			toFullFileInfo.Add(ConstantsTypeKind.IcedConstants, new FullEnumFileInfo(Path.Combine(baseDir, nameof(ConstantsTypeKind.IcedConstants) + ".g.cs"), CSharpConstants.IcedNamespace));
		}

		public void Generate(ConstantsType constantsType) {
			if (toFullFileInfo.TryGetValue(constantsType.Kind, out var fullFileInfo))
				WriteFile(fullFileInfo, constantsType);
			else
				throw new InvalidOperationException();
		}

		void WriteFile(FullEnumFileInfo info, ConstantsType constantsType) {
			var sb = new StringBuilder();
			using (var writer = new FileWriter(TargetLanguage.CSharp, FileUtils.OpenWrite(info.Filename))) {
				writer.WriteFileHeader();
				if (!(info.Define is null))
					writer.WriteLine($"#if {info.Define}");

				writer.WriteLine($"namespace {info.Namespace} {{");

				if (constantsType.IsPublic && constantsType.IsMissingDocs)
					writer.WriteLine("#pragma warning disable 1591 // Missing XML comment for publicly visible type or member");
				writer.Indent();
				docWriter.Write(writer, constantsType.Documentation, constantsType.Name);
				var pub = constantsType.IsPublic ? "public " : string.Empty;
				writer.WriteLine($"{pub}static class {constantsType.Name} {{");

				writer.Indent();
				foreach (var constant in constantsType.Constants) {
					docWriter.Write(writer, constant.Documentation, constantsType.Name);
					sb.Clear();
					sb.Append(constant.IsPublic ? "public " : "internal ");
					sb.Append("const ");
					sb.Append(GetType(constant.Kind));
					sb.Append(' ');
					sb.Append(constant.Name);
					sb.Append(" = ");
					sb.Append(GetValue(constant));
					sb.Append(';');
					writer.WriteLine(sb.ToString());
				}
				writer.Unindent();

				writer.WriteLine("}");
				writer.Unindent();
				writer.WriteLine("}");

				if (!(info.Define is null))
					writer.WriteLine("#endif");
			}
		}

		static EnumType GetEnumType(ConstantKind kind) {
			switch (kind) {
			case ConstantKind.Register:
				return RegisterEnum.Instance;
			case ConstantKind.MemorySize:
				return MemorySizeEnum.Instance;
			default:
				throw new InvalidOperationException();
			}
		}

		static string GetType(ConstantKind kind) {
			switch (kind) {
			case ConstantKind.Int32:
				return "int";
			case ConstantKind.Register:
			case ConstantKind.MemorySize:
				return GetEnumType(kind).Name;
			default:
				throw new InvalidOperationException();
			}
		}

		static string GetValue(Constant constant) {
			switch (constant.Kind) {
			case ConstantKind.Int32:
				return ((int)constant.Value).ToString();

			case ConstantKind.Register:
			case ConstantKind.MemorySize:
				return GetValueString(constant);

			default:
				throw new InvalidOperationException();
			}
		}

		static string GetValueString(Constant constant) {
			var enumType = GetEnumType(constant.Kind);
			var enumValue = enumType.Values.First(a => a.Value == constant.Value);
			return $"{enumType.Name}.{enumValue.Name}";
		}
	}
}
