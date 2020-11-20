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

namespace Generator.Documentation.Python {
	sealed class PythonDocCommentWriter : DocCommentWriter {
		readonly IdentifierConverter idConverter;
		readonly string typeSeparator;
		readonly StringBuilder sb;

		static readonly Dictionary<string, string> toTypeInfo = new Dictionary<string, string>(StringComparer.Ordinal) {
			{ "bcd", "bcd" },
			{ "bf16", "bfloat16" },
			{ "f16", "f16" },
			{ "f32", "f32" },
			{ "f64", "f64" },
			{ "f80", "f80" },
			{ "f128", "f128" },
			{ "i8", "i8" },
			{ "i16", "i16" },
			{ "i32", "i32" },
			{ "i64", "i64" },
			{ "i128", "i128" },
			{ "i256", "i256" },
			{ "i512", "i512" },
			{ "u8", "u8" },
			{ "u16", "u16" },
			{ "u32", "u32" },
			{ "u52", "u52" },
			{ "u64", "u64" },
			{ "u128", "u128" },
			{ "u256", "u256" },
			{ "u512", "u512" },
		};

		public PythonDocCommentWriter(IdentifierConverter idConverter, string typeSeparator = ".") {
			this.idConverter = idConverter;
			this.typeSeparator = typeSeparator;
			sb = new StringBuilder();
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
			s = s.Replace("`", @"\`");
			s = s.Replace("\"", @"\""");
			s = s.Replace("*", @"\*");
			return s;
		}

		void RawWriteWithComment(FileWriter writer, bool writeEmpty = true) {
			var s = GetStringAndReset();
			if (s.Length == 0 && !writeEmpty)
				return;
			if (s.Length == 0)
				writer.WriteLineNoIndent(s);
			else
				writer.WriteLine(s);
		}

		public void BeginWrite(FileWriter writer) {
			if (sb.Length != 0)
				throw new InvalidOperationException();
			writer.WriteLine(@"""""""");
		}

		public void EndWrite(FileWriter writer) {
			RawWriteWithComment(writer, false);
			writer.WriteLine(@"""""""");
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
					sb.Append("``");
					sb.Append(info.value);
					sb.Append("``");
					if (!string.IsNullOrEmpty(info.value2))
						throw new InvalidOperationException();
					break;
				case TokenKind.PrimitiveType:
					if (!toTypeInfo.TryGetValue(info.value, out var type))
						throw new InvalidOperationException($"Unknown type '{info.value}, comment: {documentation}");
					sb.Append("``");
					sb.Append(idConverter.Type(type));
					sb.Append("``");
					if (!string.IsNullOrEmpty(info.value2))
						throw new InvalidOperationException();
					break;
				case TokenKind.Type:
					sb.Append("``");
					t = RemoveNamespace(idConverter.Type(info.value));
					sb.Append(t);
					sb.Append("``");
					if (!string.IsNullOrEmpty(info.value2))
						throw new InvalidOperationException();
					break;
				case TokenKind.EnumFieldReference:
				case TokenKind.FieldReference:
					sb.Append("``");
					t = idConverter.Type(info.value);
					if (info.value != typeName) {
						sb.Append(t);
						sb.Append(typeSeparator);
					}
					m = info.kind == TokenKind.EnumFieldReference ? idConverter.EnumField(info.value2) : idConverter.Field(info.value2);
					sb.Append(m);
					sb.Append("``");
					break;
				case TokenKind.Property:
					sb.Append("``");
					t = idConverter.Type(info.value);
					if (info.value != typeName) {
						sb.Append(t);
						sb.Append(typeSeparator);
					}
					m = idConverter.PropertyDoc(info.value2);
					sb.Append(m);
					sb.Append("``");
					break;
				case TokenKind.Method:
					sb.Append("``");
					t = idConverter.Type(info.value);
					if (info.value != typeName) {
						sb.Append(t);
						sb.Append(typeSeparator);
					}
					m = idConverter.MethodDoc(TranslateMethodName(info.value2));
					sb.Append(m);
					sb.Append("``");
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
