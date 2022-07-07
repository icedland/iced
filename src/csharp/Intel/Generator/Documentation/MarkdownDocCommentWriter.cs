// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Text;
using Generator.IO;

namespace Generator.Documentation {
	abstract class MarkdownDocCommentWriter : DocCommentWriter {
		readonly IdentifierConverter idConverter;
		readonly string enumSeparator;
		readonly string fieldSeparator;
		readonly string propertySeparator;
		readonly string methodSeparator;
		readonly bool supportsRefs;
		readonly StringBuilder sb;
		readonly StringBuilder sb2;
		readonly List<(string @ref, string url)> refUrls;
		readonly HashSet<string> usedRefs;
		readonly Dictionary<string, (string type, bool isKeyword)> toTypeInfo;
		readonly string emptyLineComment;
		readonly string lineComment;

		protected MarkdownDocCommentWriter(IdentifierConverter idConverter, string enumSeparator, string fieldSeparator, string propertySeparator,
				string methodSeparator, string emptyLineComment, string lineComment, bool supportsRefs,
				Dictionary<string, (string type, bool isKeyword)> toTypeInfo) {
			this.idConverter = idConverter;
			this.enumSeparator = enumSeparator;
			this.fieldSeparator = fieldSeparator;
			this.propertySeparator = propertySeparator;
			this.methodSeparator = methodSeparator;
			this.emptyLineComment = emptyLineComment;
			this.lineComment = lineComment;
			this.supportsRefs = supportsRefs;
			this.toTypeInfo = toTypeInfo;
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
			writer.WriteLine(s.Length == 0 ? emptyLineComment : lineComment + s);
		}

		public void BeginWrite(FileWriter writer) {
			if (sb.Length != 0)
				throw new InvalidOperationException();
			refUrls.Clear();
		}

		void AddRefBracket(StringBuilder builder, char c) {
			if (supportsRefs)
				builder.Append(c);
		}

		public void EndWrite(FileWriter writer) {
			RawWriteWithComment(writer, false);
			if (refUrls.Count > 0 && supportsRefs) {
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
				case TokenKind.HorizontalLine:
					sb.Append(string.Empty);
					RawWriteWithComment(writer);
					sb.Append("---");
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
					AddRefBracket(sb2, '[');
					sb2.Append('`');
					t = RemoveNamespace(idConverter.Type(info.value));
					sb2.Append(t);
					sb2.Append('`');
					AddRefBracket(sb2, ']');
					sb.Append(sb2);
					refUrls.Add((sb2.ToString(), $"{GetTypeKind(t)}.{t}.html"));
					if (!string.IsNullOrEmpty(info.value2))
						throw new InvalidOperationException();
					break;
				case TokenKind.EnumFieldReference:
				case TokenKind.FieldReference:
					AddRefBracket(sb2, '[');
					sb2.Append('`');
					t = idConverter.Type(info.value);
					if (info.value != typeName) {
						sb2.Append(t);
						sb2.Append(info.kind == TokenKind.EnumFieldReference ? enumSeparator : fieldSeparator);
					}
					m = info.kind == TokenKind.EnumFieldReference ? idConverter.EnumField(info.value2) : idConverter.Field(info.value2);
					sb2.Append(m);
					sb2.Append('`');
					AddRefBracket(sb2, ']');
					sb.Append(sb2);
					refUrls.Add((sb2.ToString(), $"{GetTypeKind(t)}.{t}.html#variant.{m}"));
					break;
				case TokenKind.Property:
					AddRefBracket(sb2, '[');
					sb2.Append('`');
					t = idConverter.Type(info.value);
					if (info.value != typeName) {
						sb2.Append(t);
						sb2.Append(propertySeparator);
					}
					m = idConverter.PropertyDoc(info.value2);
					sb2.Append(m);
					sb2.Append('`');
					AddRefBracket(sb2, ']');
					sb.Append(sb2);
					refUrls.Add((sb2.ToString(), $"{GetTypeKind(t)}.{t}.html#method.{GetMethodNameOnly(m)}"));
					break;
				case TokenKind.Method:
					AddRefBracket(sb2, '[');
					sb2.Append('`');
					t = idConverter.Type(info.value);
					if (info.value != typeName) {
						sb2.Append(t);
						sb2.Append(methodSeparator);
					}
					m = idConverter.MethodDoc(TranslateMethodName(info.value2));
					sb2.Append(m);
					sb2.Append('`');
					AddRefBracket(sb2, ']');
					sb.Append(sb2);
					refUrls.Add((sb2.ToString(), $"{GetTypeKind(t)}.{t}.html#method.{GetMethodNameOnly(m)}"));
					break;
				default:
					throw new InvalidOperationException();
				}
			}
		}
	}
}
