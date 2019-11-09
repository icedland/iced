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
using Generator.Documentation.Rust;
using Generator.IO;

namespace Generator.Constants.Rust {
	sealed class RustConstantsGenerator : IConstantsGenerator {
		readonly IdentifierConverter idConverter;
		readonly Dictionary<ConstantsTypeKind, PartialConstantsFileInfo> toPartialFileInfo;
		readonly RustDocCommentWriter docWriter;

		sealed class PartialConstantsFileInfo {
			public readonly string Id;
			public readonly string Filename;
			public readonly string[] Attributes;

			public PartialConstantsFileInfo(string id, string filename, string? attribute = null) {
				Id = id;
				Filename = filename;
				Attributes = attribute is null ? Array.Empty<string>() : new string[] { attribute };
			}

			public PartialConstantsFileInfo(string id, string filename, string[] attributes) {
				Id = id;
				Filename = filename;
				Attributes = attributes;
			}
		}

		public RustConstantsGenerator(ProjectDirs projectDirs) {
			idConverter = RustIdentifierConverter.Create();
			docWriter = new RustDocCommentWriter(idConverter);

			toPartialFileInfo = new Dictionary<ConstantsTypeKind, PartialConstantsFileInfo>();
			toPartialFileInfo.Add(ConstantsTypeKind.IcedConstants, new PartialConstantsFileInfo("IcedConstants", Path.Combine(projectDirs.RustDir, "common", "icedconstants.rs")));
		}

		public void Generate(ConstantsType constantsType) {
			if (toPartialFileInfo.TryGetValue(constantsType.Kind, out var partialInfo)) {
				if (!(partialInfo is null))
					new FileUpdater(TargetLanguage.Rust, partialInfo.Id, partialInfo.Filename).Generate(writer => WriteConstants(writer, partialInfo, constantsType));
			}
			else
				throw new InvalidOperationException();
		}

		void WriteConstants(FileWriter writer, PartialConstantsFileInfo info, ConstantsType constantsType) {
			if (constantsType.IsPublic && constantsType.IsMissingDocs)
				writer.WriteLine("#[allow(missing_docs)]");
			docWriter.Write(writer, constantsType.Documentation, constantsType.RawName);
			foreach (var attr in info.Attributes)
				writer.WriteLine(attr);
			var pub = constantsType.IsPublic ? "pub " : "pub(crate) ";
			writer.WriteLine($"{pub}struct {constantsType.Name(idConverter)};");
			writer.WriteLine($"impl {constantsType.Name(idConverter)} {{");

			var sb = new StringBuilder();
			writer.Indent();
			foreach (var constant in constantsType.Constants) {
				docWriter.Write(writer, constant.Documentation, constantsType.RawName);
				sb.Clear();
				sb.Append(constant.IsPublic ? "pub " : "pub(crate) ");
				sb.Append("const ");
				sb.Append(constant.Name(idConverter));
				sb.Append(": ");
				sb.Append(GetType(constant.Kind));
				sb.Append(" = ");
				sb.Append(GetValue(constant));
				sb.Append(';');
				writer.WriteLine(sb.ToString());
			}
			writer.Unindent();

			writer.WriteLine("}");
		}

		string GetType(ConstantKind kind) {
			switch (kind) {
			case ConstantKind.Int32:
				return "i32";
			case ConstantKind.Register:
			case ConstantKind.MemorySize:
				return ConstantsUtils.GetEnumType(kind).Name(idConverter);
			default:
				throw new InvalidOperationException();
			}
		}

		string GetValue(Constant constant) {
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

		string GetValueString(Constant constant) {
			var enumType = ConstantsUtils.GetEnumType(constant.Kind);
			var enumValue = enumType.Values.First(a => a.Value == constant.Value);
			return $"{enumType.Name(idConverter)}::{enumValue.Name(idConverter)}";
		}
	}
}
