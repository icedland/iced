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
using System.Security;
using System.Text;
using Generator.IO;

namespace Generator.Documentation.CSharp {
	sealed class CSharpDocCommentWriter : DocCommentWriter {
		readonly IdentifierConverter idConverter;
		readonly StringBuilder sb;

		static readonly Dictionary<string, (string type, bool isKeyword)> toTypeInfo = new Dictionary<string, (string type, bool isKeyword)>(StringComparer.Ordinal) {
			{ "bcd", ("bcd", false) },
			{ "bf16", ("bfloat16", false) },
			{ "f16", ("float16", false) },
			{ "f32", ("float", true) },
			{ "f64", ("double", true) },
			{ "f80", ("float80", false) },
			{ "f128", ("float128", false) },
			{ "i8", ("sbyte", true) },
			{ "i16", ("short", true) },
			{ "i32", ("int", true) },
			{ "i64", ("long", true) },
			{ "i128", ("int128", false) },
			{ "i256", ("int256", false) },
			{ "i512", ("int512", false) },
			{ "u8", ("byte", true) },
			{ "u16", ("ushort", true) },
			{ "u32", ("uint", true) },
			{ "u52", ("uint52", false) },
			{ "u64", ("ulong", true) },
			{ "u128", ("uint128", false) },
			{ "u256", ("uint256", false) },
			{ "u512", ("uint512", false) },
		};

		public CSharpDocCommentWriter(IdentifierConverter idConverter) {
			this.idConverter = idConverter;
			sb = new StringBuilder();
		}

		string GetStringAndReset() {
			while (sb.Length > 0 && char.IsWhiteSpace(sb[sb.Length - 1]))
				sb.Length--;
			var s = sb.ToString();
			sb.Clear();
			return s;
		}

		public void Write(FileWriter writer, string? documentation, string enumName) {
			if (string.IsNullOrEmpty(documentation))
				return;
			if (sb.Length != 0)
				throw new InvalidOperationException();
			sb.Append("/// <summary>");
			foreach (var info in GetTokens(enumName, documentation)) {
				switch (info.kind) {
				case TokenKind.NewParagraph:
					if (!string.IsNullOrEmpty(info.value) && !string.IsNullOrEmpty(info.value2))
						throw new InvalidOperationException();
					sb.Append("<br/>");
					writer.WriteLine(GetStringAndReset());
					sb.Append("/// <br/>");
					writer.WriteLine(GetStringAndReset());
					sb.Append("/// ");
					break;
				case TokenKind.String:
					sb.Append(Escape(info.value));
					if (!string.IsNullOrEmpty(info.value2))
						throw new InvalidOperationException();
					break;
				case TokenKind.Code:
					sb.Append("<c>");
					sb.Append(Escape(info.value));
					sb.Append("</c>");
					if (!string.IsNullOrEmpty(info.value2))
						throw new InvalidOperationException();
					break;
				case TokenKind.Type:
					if (!toTypeInfo.TryGetValue(info.value, out var typeInfo))
						throw new InvalidOperationException($"Unknown type '{info.value}, comment: {documentation}");
					if (typeInfo.isKeyword) {
						sb.Append("<see cref=\"");
						sb.Append(Escape(idConverter.Type(typeInfo.type)));
						sb.Append("\"/>");
					}
					else {
						sb.Append("<c>");
						sb.Append(Escape(idConverter.Type(typeInfo.type)));
						sb.Append("</c>");
					}
					if (!string.IsNullOrEmpty(info.value2))
						throw new InvalidOperationException();
					break;
				case TokenKind.EnumFieldReference:
					sb.Append("<see cref=\"");
					if (info.value != enumName) {
						sb.Append(Escape(idConverter.Type(info.value)));
						sb.Append('.');
					}
					sb.Append(Escape(idConverter.EnumField(info.value2)));
					sb.Append("\"/>");
					break;
				default:
					throw new InvalidOperationException();
				}
			}
			sb.Append("</summary>");
			writer.WriteLine(GetStringAndReset());
		}

		static string Escape(string value) => SecurityElement.Escape(value) ?? throw new InvalidOperationException();
	}
}
