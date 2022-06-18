// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Security;
using System.Text;
using Generator.IO;

namespace Generator.Documentation.CSharp {
	sealed class CSharpDocCommentWriter : DocCommentWriter {
		readonly IdentifierConverter idConverter;
		readonly StringBuilder sb;

		static readonly Dictionary<string, (string type, bool isKeyword)> toTypeInfo = new(StringComparer.Ordinal) {
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
			while (sb.Length > 0 && char.IsWhiteSpace(sb[^1]))
				sb.Length--;
			var s = sb.ToString();
			sb.Clear();
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
		}

		public void EndWrite(FileWriter writer) =>
			RawWriteWithComment(writer, false);

		public void WriteSummary(FileWriter writer, string? documentation, string typeName) {
			if (string.IsNullOrEmpty(documentation))
				return;
			BeginWrite(writer);
			sb.Append("<summary>");
			WriteDoc(writer, documentation, typeName);
			sb.Append("</summary>");
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
				switch (info.kind) {
				case TokenKind.NewParagraph:
					if (!string.IsNullOrEmpty(info.value) && !string.IsNullOrEmpty(info.value2))
						throw new InvalidOperationException();
					sb.Append("<br/>");
					RawWriteWithComment(writer);
					sb.Append("<br/>");
					RawWriteWithComment(writer);
					break;
				case TokenKind.HorizontalLine:
					sb.Append("<hr/>");
					RawWriteWithComment(writer);
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
				case TokenKind.PrimitiveType:
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
				case TokenKind.Type:
					sb.Append("<see cref=\"");
					sb.Append(Escape(idConverter.Type(info.value)));
					sb.Append("\"/>");
					if (!string.IsNullOrEmpty(info.value2))
						throw new InvalidOperationException();
					break;
				case TokenKind.EnumFieldReference:
				case TokenKind.FieldReference:
					sb.Append("<see cref=\"");
					if (info.value != typeName) {
						sb.Append(Escape(idConverter.Type(info.value)));
						sb.Append('.');
					}
					sb.Append(Escape(idConverter.EnumField(info.value2)));
					sb.Append("\"/>");
					break;
				case TokenKind.Property:
					sb.Append("<see cref=\"");
					if (info.value != typeName) {
						sb.Append(Escape(idConverter.Type(info.value)));
						sb.Append('.');
					}
					sb.Append(Escape(idConverter.PropertyDoc(info.value2)));
					sb.Append("\"/>");
					break;
				case TokenKind.Method:
					sb.Append("<see cref=\"");
					if (info.value != typeName) {
						sb.Append(Escape(idConverter.Type(info.value)));
						sb.Append('.');
					}
					sb.Append(Escape(idConverter.MethodDoc(info.value2)));
					sb.Append("\"/>");
					break;
				default:
					throw new InvalidOperationException();
				}
			}
		}

		static string Escape(string value) => SecurityElement.Escape(value) ?? throw new InvalidOperationException();
	}
}
