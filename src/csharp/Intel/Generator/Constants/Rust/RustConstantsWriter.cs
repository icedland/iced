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
using Generator.Documentation;
using Generator.Documentation.Rust;
using Generator.IO;

namespace Generator.Constants.Rust {
	readonly struct RustConstantsWriter {
		readonly GenTypes genTypes;
		readonly IdentifierConverter idConverter;
		readonly RustDocCommentWriter docWriter;
		readonly DeprecatedWriter deprecatedWriter;

		public RustConstantsWriter(GenTypes genTypes, IdentifierConverter idConverter, RustDocCommentWriter docWriter, DeprecatedWriter deprecatedWriter) {
			this.genTypes = genTypes;
			this.idConverter = idConverter;
			this.docWriter = docWriter;
			this.deprecatedWriter = deprecatedWriter;
		}

		public void Write(FileWriter writer, ConstantsType constantsType, string[] attributes) {
			docWriter.WriteSummary(writer, constantsType.Documentation, constantsType.RawName);
			foreach (var attr in attributes)
				writer.WriteLine(attr);
			var pub = constantsType.IsPublic ? "pub " : "pub(crate) ";
			writer.WriteLine($"{pub}struct {constantsType.Name(idConverter)};");
			if (constantsType.IsPublic && constantsType.IsMissingDocs)
				writer.WriteLine(RustConstants.AttributeAllowMissingDocs);
			foreach (var attr in attributes.Where(a => a.StartsWith(RustConstants.FeaturePrefix)))
				writer.WriteLine(attr);
			if (!constantsType.IsPublic)
				writer.WriteLine(RustConstants.AttributeAllowDeadCode);
			writer.WriteLine($"impl {constantsType.Name(idConverter)} {{");

			var sb = new StringBuilder();
			using (writer.Indent()) {
				foreach (var constant in constantsType.Constants) {
					if (ShouldIgnore(constant))
						continue;
					docWriter.WriteSummary(writer, constant.Documentation, constantsType.RawName);
					deprecatedWriter.WriteDeprecated(writer, constant);
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
			}

			writer.WriteLine("}");
		}

		static bool ShouldIgnore(Constant constant) {
			if (constant.DeclaringType.TypeId == TypeIds.SymbolFlags)
				return constant.ValueUInt64 == (ulong)Enums.Formatter.SymbolFlags.HasSymbolSize;

			return false;
		}

		string GetType(ConstantKind kind) {
			switch (kind) {
			case ConstantKind.Char:
				return "char";
			case ConstantKind.String:
				return "&'static str";
			case ConstantKind.Int32:
			case ConstantKind.UInt32:
				return "u32";
			case ConstantKind.UInt64:
				return "u64";
			case ConstantKind.Index:
				return "usize";
			case ConstantKind.Register:
			case ConstantKind.MemorySize:
				return EnumUtils.GetEnumType(genTypes, kind).Name(idConverter);
			default:
				throw new InvalidOperationException();
			}
		}

		string GetValue(Constant constant) {
			switch (constant.Kind) {
			case ConstantKind.Char:
				var c = (char)constant.ValueUInt64;
				return "'" + c.ToString() + "'";

			case ConstantKind.String:
				if (constant.RefValue is string s)
					return "\"" + EscapeStringValue(s) + "\"";
				throw new InvalidOperationException();

			case ConstantKind.Int32:
			case ConstantKind.UInt32:
			case ConstantKind.Index:
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
			var enumType = EnumUtils.GetEnumType(genTypes, constant.Kind);
			var enumValue = enumType.Values.First(a => a.Value == constant.ValueUInt64);
			return $"{enumType.Name(idConverter)}::{enumValue.Name(idConverter)}";
		}
	}
}
