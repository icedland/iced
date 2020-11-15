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
using Generator.Enums;
using Generator.Enums.Encoder;
using Generator.Enums.InstructionInfo;
using Generator.IO;

namespace Generator.Documentation.Rust {
	sealed class RustDocCommentWriter : DocCommentWriter {
		readonly IdentifierConverter idConverter;
		readonly string typeSeparator;
		readonly StringBuilder sb;
		readonly StringBuilder sb2;
		readonly List<(string @ref, string url)> refUrls;
		readonly HashSet<string> usedRefs;

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

		public RustDocCommentWriter(IdentifierConverter idConverter, string typeSeparator = "::") {
			this.idConverter = idConverter;
			this.typeSeparator = typeSeparator;
			sb = new StringBuilder();
			sb2 = new StringBuilder();
			refUrls = new List<(string @ref, string url)>();
			usedRefs = new HashSet<string>(StringComparer.Ordinal);
		}

		string GetStringAndReset() {
			while (sb.Length > 0 && char.IsWhiteSpace(sb[^1]))
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

		void RawWriteWithComment(FileWriter writer, bool writeEmpty = true) {
			var s = GetStringAndReset();
			if (s.Length == 0 && !writeEmpty)
				return;
			writer.WriteLine(s.Length == 0 ? "///" : "/// " + s);
		}

		public void BeginWrite(FileWriter writer) {
			if (sb.Length != 0)
				throw new InvalidOperationException();
			refUrls.Clear();
		}

		public void EndWrite(FileWriter writer) {
			RawWriteWithComment(writer, false);
			if (refUrls.Count > 0) {
				RawWriteWithComment(writer);
				usedRefs.Clear();
				foreach (var info in refUrls) {
					if (!usedRefs.Add(info.@ref))
						continue;
					sb.Append($"{info.@ref}: {info.url}");
					RawWriteWithComment(writer);
				}
			}
		}

		public void WriteSummary(FileWriter writer, string? documentation, string typeName) {
			if (string.IsNullOrEmpty(documentation))
				return;
			BeginWrite(writer);
			WriteDoc(writer, documentation, typeName);
			EndWrite(writer);
		}

		public void Write(string text) =>
			sb.Append(text);

		public void WriteLine(FileWriter writer, string text) {
			Write(text);
			RawWriteWithComment(writer);
		}

		public void WriteDocLine(FileWriter writer, string text, string typeName) {
			WriteDoc(writer, text, typeName);
			RawWriteWithComment(writer);
		}

		public void WriteDoc(FileWriter writer, string documentation, string typeName) {
			foreach (var info in GetTokens(typeName, documentation)) {
				sb2.Clear();
				string t, m;
				switch (info.kind) {
				case TokenKind.NewParagraph:
					if (!string.IsNullOrEmpty(info.value) && !string.IsNullOrEmpty(info.value2))
						throw new InvalidOperationException();
					RawWriteWithComment(writer);
					RawWriteWithComment(writer);
					break;
				case TokenKind.String:
					sb.Append(Escape(info.value));
					if (!string.IsNullOrEmpty(info.value2))
						throw new InvalidOperationException();
					break;
				case TokenKind.Code:
					sb.Append('`');
					sb.Append(info.value);
					sb.Append('`');
					if (!string.IsNullOrEmpty(info.value2))
						throw new InvalidOperationException();
					break;
				case TokenKind.PrimitiveType:
					if (!toTypeInfo.TryGetValue(info.value, out var typeInfo))
						throw new InvalidOperationException($"Unknown type '{info.value}, comment: {documentation}");
					sb.Append('`');
					sb.Append(idConverter.Type(typeInfo.type));
					sb.Append('`');
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
				case TokenKind.FieldReference:
					sb2.Append("[`");
					t = idConverter.Type(info.value);
					if (info.value != typeName) {
						sb2.Append(t);
						sb2.Append(typeSeparator);
					}
					m = info.kind == TokenKind.EnumFieldReference ? idConverter.EnumField(info.value2) : idConverter.Field(info.value2);
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
						sb2.Append(typeSeparator);
					}
					m = idConverter.PropertyDoc(info.value2);
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
						sb2.Append(typeSeparator);
					}
					m = idConverter.MethodDoc(TranslateMethodName(info.value2));
					sb2.Append(m);
					sb2.Append("`]");
					sb.Append(sb2);
					refUrls.Add((sb2.ToString(), $"{GetTypeKind(t)}.{t}.html#method.{GetMethodNameOnly(m)}"));
					break;
				default:
					throw new InvalidOperationException();
				}
			}
		}

		static string GetTypeKind(string name) =>
			name switch {
				nameof(Code) or nameof(CpuidFeature) or nameof(OpKind) or nameof(Register) or nameof(RepPrefixKind) => "enum",
				"BlockEncoder" or "ConstantOffsets" or "Instruction" or "RelocInfo" or "SymbolResult" => "struct",
				_ => throw new InvalidOperationException(),
			};

		static string GetMethodNameOnly(string name) {
			int index = name.IndexOf('(', StringComparison.Ordinal);
			if (index < 0)
				return name;
			return name[0..index];
		}

		static string TranslateMethodName(string name) {
			const string GetPattern = "Get";
			if (name.StartsWith(GetPattern, StringComparison.Ordinal))
				return name[GetPattern.Length..];
			return name;
		}
	}
}
