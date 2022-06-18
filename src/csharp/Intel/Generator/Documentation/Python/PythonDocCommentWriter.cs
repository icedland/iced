// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Text;
using Generator.IO;

namespace Generator.Documentation.Python {
	sealed class PythonDocCommentWriter : DocCommentWriter {
		const string PYTHON_PACKAGE_NAME = "iced_x86";
		readonly IdentifierConverter idConverter;
		readonly bool isInRootModule;
		readonly string typeSeparator;
		readonly StringBuilder sb;
		readonly string linePrefix;
		readonly bool pythonDocComments;
		int summaryLineNumber;
		bool hasColonText;

		static readonly Dictionary<string, string> toTypeInfo = new(StringComparer.Ordinal) {
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

		public PythonDocCommentWriter(IdentifierConverter idConverter, TargetLanguage language, bool isInRootModule, string typeSeparator = ".") {
			this.idConverter = idConverter;
			this.isInRootModule = isInRootModule;
			this.typeSeparator = typeSeparator;
			sb = new StringBuilder();

			switch (language) {
			case TargetLanguage.Python:
				linePrefix = string.Empty;
				pythonDocComments = true;
				break;

			case TargetLanguage.Rust:
				linePrefix = "/// ";
				pythonDocComments = false;
				break;

			default:
				throw new InvalidOperationException();
			}
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
			if (summaryLineNumber == 0 && hasColonText) {
				// The first line has type info and it's everything before the colon
				s = ": " + s;
			}
			s = (linePrefix + s).TrimEnd();
			summaryLineNumber++;
			hasColonText = false;
			if (s.Length == 0)
				writer.WriteLineNoIndent(s);
			else
				writer.WriteLine(s);
		}

		public void BeginWrite(FileWriter writer) {
			if (sb.Length != 0)
				throw new InvalidOperationException();
			if (pythonDocComments)
				writer.WriteLine(@"""""""");
		}

		public void EndWrite(FileWriter writer) {
			RawWriteWithComment(writer, false);
			if (pythonDocComments)
				writer.WriteLine(@"""""""");
		}

		public void WriteSummary(FileWriter writer, string? documentation, string typeName) {
			if (string.IsNullOrEmpty(documentation))
				return;
			summaryLineNumber = 0;
			hasColonText = false;
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

		void WriteDoc(FileWriter writer, string documentation, string typeName) {
			foreach (var info in GetTokens(typeName, documentation)) {
				string t, m;
				switch (info.kind) {
				case TokenKind.NewParagraph:
					if (!string.IsNullOrEmpty(info.value) && !string.IsNullOrEmpty(info.value2))
						throw new InvalidOperationException();
					RawWriteWithComment(writer);
					RawWriteWithComment(writer);
					break;
				case TokenKind.HorizontalLine:
					break;
				case TokenKind.String:
					hasColonText |= info.value.Contains(':', StringComparison.Ordinal);
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
					sb.Append(":class:`");
					if (!isInRootModule && info.value != typeName)
						sb.Append(PYTHON_PACKAGE_NAME + ".");
					t = RemoveNamespace(idConverter.Type(info.value));
					sb.Append(t);
					sb.Append('`');
					if (!string.IsNullOrEmpty(info.value2))
						throw new InvalidOperationException();
					break;
				case TokenKind.EnumFieldReference:
				case TokenKind.FieldReference:
					sb.Append(":class:`");
					WriteTypeName(typeName, info.value);
					if (Enums.EnumUtils.UppercaseTypeFields(info.value))
						m = info.value2.ToUpperInvariant();
					else if (info.kind == TokenKind.EnumFieldReference)
						m = idConverter.EnumField(info.value2);
					else
						m = idConverter.Field(info.value2);
					sb.Append(m);
					sb.Append('`');
					break;
				case TokenKind.Property:
					sb.Append(":class:`");
					WriteTypeName(typeName, info.value);
					m = TranslatePropertyName(info.value, idConverter.PropertyDoc(info.value2));
					sb.Append(m);
					sb.Append('`');
					break;
				case TokenKind.Method:
					sb.Append(":class:`");
					WriteTypeName(typeName, info.value);
					m = idConverter.MethodDoc(TranslateMethodName(info.value2));
					sb.Append(m);
					sb.Append('`');
					break;
				default:
					throw new InvalidOperationException();
				}
			}
		}

		static string TranslatePropertyName(string typeName, string propertyName) {
			if (typeName == "Instruction" && propertyName == "memory_displacement64")
				propertyName = "memory_displacement";
			return propertyName;
		}

		void WriteTypeName(string thisTypeName, string currentTypeName) {
			if (!isInRootModule && currentTypeName != thisTypeName)
				sb.Append(PYTHON_PACKAGE_NAME + ".");
			if (currentTypeName != thisTypeName) {
				sb.Append(idConverter.Type(currentTypeName));
				sb.Append(typeSeparator);
			}
		}
	}
}
