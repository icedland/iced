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
using System.Text;
using Generator.IO;

namespace Generator.Documentation.Rust {
	sealed class RustDocCommentWriter : DocCommentWriter {
		readonly IdentifierConverter idConverter;
		readonly StringBuilder sb;
		readonly StringBuilder sb2;
		readonly List<(string @ref, string url)> refUrls;

		static readonly Dictionary<string, (string type, bool isKeyword)> toTypeInfo = new Dictionary<string, (string type, bool isKeyword)>(StringComparer.Ordinal) {
			{ "bcd", ("bcd", false) },
			{ "bf16", ("bfloat16", false) },
			{ "f16", ("f16", false) },
			{ "f32", ("f32", true) },
			{ "f64", ("f64", true) },
			{ "f80", ("f80", false) },
			{ "f128", ("f128", false) },
			{ "i8", ("i8", true) },
			{ "i16", ("i16", true) },
			{ "i32", ("i32", true) },
			{ "i64", ("i64", true) },
			{ "i128", ("i128", true) },
			{ "i256", ("i256", false) },
			{ "i512", ("i512", false) },
			{ "u8", ("u8", true) },
			{ "u16", ("u16", true) },
			{ "u32", ("u32", true) },
			{ "u52", ("u52", false) },
			{ "u64", ("u64", true) },
			{ "u128", ("u128", true) },
			{ "u256", ("u256", false) },
			{ "u512", ("u512", false) },
		};

		public RustDocCommentWriter(IdentifierConverter idConverter) {
			this.idConverter = idConverter;
			sb = new StringBuilder();
			sb2 = new StringBuilder();
			refUrls = new List<(string @ref, string url)>();
		}

		string GetStringAndReset() {
			while (sb.Length > 0 && char.IsWhiteSpace(sb[sb.Length - 1]))
				sb.Length--;
			var s = sb.ToString();
			sb.Clear();
			return s;
		}

		static string Escape(string s) {
			s = s.Replace(@"\", @"\\");
			s = s.Replace("[", @"\[");
			s = s.Replace("]", @"\]");
			return s;
		}

		public void Write(FileWriter writer, string? documentation, string typeName) {
			if (string.IsNullOrEmpty(documentation))
				return;
			if (sb.Length != 0)
				throw new InvalidOperationException();
			const string docComment = "/// ";
			refUrls.Clear();
			sb.Append(docComment);
			foreach (var info in GetTokens(typeName, documentation)) {
				sb2.Clear();
				string t, m;
				switch (info.kind) {
				case TokenKind.NewParagraph:
					if (!string.IsNullOrEmpty(info.value) && !string.IsNullOrEmpty(info.value2))
						throw new InvalidOperationException();
					writer.WriteLine(GetStringAndReset());
					sb.Append(docComment);
					writer.WriteLine(GetStringAndReset());
					sb.Append(docComment);
					break;
				case TokenKind.String:
					sb.Append(Escape(info.value));
					if (!string.IsNullOrEmpty(info.value2))
						throw new InvalidOperationException();
					break;
				case TokenKind.Code:
					sb.Append("`");
					sb.Append(info.value);
					sb.Append("`");
					if (!string.IsNullOrEmpty(info.value2))
						throw new InvalidOperationException();
					break;
				case TokenKind.PrimitiveType:
					if (!toTypeInfo.TryGetValue(info.value, out var typeInfo))
						throw new InvalidOperationException($"Unknown type '{info.value}, comment: {documentation}");
					sb.Append("`");
					sb.Append(idConverter.Type(typeInfo.type));
					sb.Append("`");
					if (!string.IsNullOrEmpty(info.value2))
						throw new InvalidOperationException();
					break;
				case TokenKind.Type:
					sb2.Append("[`");
					t = RemoveNamespace(idConverter.Type(info.value));
					sb2.Append(t);
					sb2.Append("`]");
					sb.Append(sb2);
					refUrls.Add((sb2.ToString(), $"{GetTypeKind(t)}.{t}.html"));
					if (!string.IsNullOrEmpty(info.value2))
						throw new InvalidOperationException();
					break;
				case TokenKind.EnumFieldReference:
					sb2.Append("[`");
					t = idConverter.Type(info.value);
					if (info.value != typeName) {
						sb2.Append(t);
						sb2.Append("::");
					}
					m = idConverter.EnumField(info.value2);
					sb2.Append(m);
					sb2.Append("`]");
					sb.Append(sb2);
					refUrls.Add((sb2.ToString(), $"{GetTypeKind(t)}.{t}.html#variant.{m}"));
					break;
				case TokenKind.Property:
					sb2.Append("[`");
					t = idConverter.Type(info.value);
					if (info.value != typeName) {
						sb2.Append(t);
						sb2.Append("::");
					}
					m = idConverter.Property(info.value2);
					sb2.Append(m);
					sb2.Append("`]");
					sb.Append(sb2);
					refUrls.Add((sb2.ToString(), $"{GetTypeKind(t)}.{t}.html#method.{GetMethodNameOnly(m)}"));
					break;
				case TokenKind.Method:
					sb2.Append("[`");
					t = idConverter.Type(info.value);
					if (info.value != typeName) {
						sb2.Append(t);
						sb2.Append("::");
					}
					m = idConverter.Method(TranslateMethodName(info.value2));
					sb2.Append(m);
					sb2.Append("`]");
					sb.Append(sb2);
					refUrls.Add((sb2.ToString(), $"{GetTypeKind(t)}.{t}.html#method.{GetMethodNameOnly(m)}"));
					break;
				default:
					throw new InvalidOperationException();
				}
			}
			writer.WriteLine(GetStringAndReset());
			if (refUrls.Count > 0) {
				writer.WriteLine(docComment.Trim());
				foreach (var info in refUrls)
					writer.WriteLine($"{docComment}{info.@ref}: {info.url}");
			}
		}

		static string GetTypeKind(string name) {
			switch (name) {
			case "Code":
			case "Register":
			case "OpKind":
			case "CpuidFeature":
				return "enum";
			case "Instruction":
				return "struct";
			default:
				throw new InvalidOperationException();
			}
		}

		static string GetMethodNameOnly(string name) {
			int index = name.IndexOf('(');
			if (index < 0)
				return name;
			return name.Substring(0, index);
		}

		string TranslateMethodName(string name) {
			const string GetPattern = "Get";
			if (name.StartsWith(GetPattern))
				return name.Substring(GetPattern.Length);
			return name;
		}
	}
}
