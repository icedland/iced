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
using System.Linq;
using System.Text;
using Generator.Documentation.Rust;
using Generator.IO;

namespace Generator.Constants.Rust {
	readonly struct RustConstantsWriter {
		readonly IdentifierConverter idConverter;
		readonly RustDocCommentWriter docWriter;

		public RustConstantsWriter(IdentifierConverter idConverter, RustDocCommentWriter docWriter) {
			this.idConverter = idConverter;
			this.docWriter = docWriter;
		}

		public void Write(FileWriter writer, ConstantsType constantsType, string[] attributes) {
			if (constantsType.IsPublic && constantsType.IsMissingDocs)
				writer.WriteLine("#[allow(missing_docs)]");
			docWriter.Write(writer, constantsType.Documentation, constantsType.RawName);
			foreach (var attr in attributes)
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
			case ConstantKind.String:
				return "&'static str";
			case ConstantKind.Int32:
			case ConstantKind.UInt32:
				return "u32";
			case ConstantKind.UInt64:
				return "u64";
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
			case ConstantKind.UInt32:
				if (constant.UseHex)
					return NumberFormatter.FormatHexUInt32WithSep((uint)constant.ValueUInt64);
				return ((uint)constant.ValueUInt64).ToString();

			case ConstantKind.UInt64:
				if (constant.UseHex)
					return NumberFormatter.FormatHexUInt64WithSep(constant.ValueUInt64);
				return constant.ValueUInt64.ToString();

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
			var enumValue = enumType.Values.First(a => a.Value == constant.ValueUInt64);
			return $"{enumType.Name(idConverter)}::{enumValue.Name(idConverter)}";
		}
	}
}
