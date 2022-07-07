// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Security;
using System.Text;
using Generator.IO;

namespace Generator.Documentation.Java {
	sealed class JavaDocCommentWriter : DocCommentWriter {
		readonly IdentifierConverter idConverter;
		readonly StringBuilder sb;
		bool isSummary;

		static readonly Dictionary<string, (string type, bool isKeyword)> toTypeInfo = new(StringComparer.Ordinal) {
			{ "bcd", ("bcd", false) },
			{ "bf16", ("bfloat16", false) },
			{ "f16", ("float16", false) },
			{ "f32", ("float", true) },
			{ "f64", ("double", true) },
			{ "f80", ("float80", false) },
			{ "f128", ("float128", false) },
			{ "i8", ("byte", true) },
			{ "i16", ("short", true) },
			{ "i32", ("int", true) },
			{ "i64", ("long", true) },
			{ "i128", ("int128", false) },
			{ "i256", ("int256", false) },
			{ "i512", ("int512", false) },
			{ "u8", ("ubyte", false) },
			{ "u16", ("ushort", false) },
			{ "u32", ("uint", false) },
			{ "u52", ("uint52", false) },
			{ "u64", ("ulong", false) },
			{ "u128", ("uint128", false) },
			{ "u256", ("uint256", false) },
			{ "u512", ("uint512", false) },
		};

		public JavaDocCommentWriter(IdentifierConverter idConverter) {
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
			writer.WriteLine(s.Length == 0 ? " *" : " * " + s);
		}

		public void BeginWrite(FileWriter writer) {
			if (sb.Length != 0)
				throw new InvalidOperationException();
			writer.WriteLine("/**");
		}

		public void EndWrite(FileWriter writer) {
			RawWriteWithComment(writer, false);
			writer.WriteLine(" */");
		}

		public void WriteSummary(FileWriter writer, string? documentation, string typeName, string? deprecMsg) {
			if (string.IsNullOrEmpty(documentation))
				return;
			BeginWrite(writer);
			isSummary = true;
			WriteDoc(writer, documentation, typeName);
			RawWriteWithComment(writer);
			if (deprecMsg is not null) {
				WriteLine(writer, string.Empty);
				WriteLine(writer, $"@deprecated {deprecMsg}");
			}
			EndWrite(writer);
			isSummary = false;
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
				string javadocType;
				string javadocMember;
				switch (info.kind) {
				case TokenKind.NewParagraph:
					if (!string.IsNullOrEmpty(info.value) && !string.IsNullOrEmpty(info.value2))
						throw new InvalidOperationException();
					sb.Append("<p>");
					RawWriteWithComment(writer);
					break;
				case TokenKind.HorizontalLine:
					sb.Append("<hr>");
					RawWriteWithComment(writer);
					break;
				case TokenKind.String:
					var s = Escape(info.value);
					if (isSummary && s.Contains("."))
						s = s.Replace(".", ".<!-- -->");
					sb.Append(s);
					if (!string.IsNullOrEmpty(info.value2))
						throw new InvalidOperationException();
					break;
				case TokenKind.Code:
					sb.Append("{@code ");
					sb.Append(Escape(info.value));
					sb.Append("}");
					if (!string.IsNullOrEmpty(info.value2))
						throw new InvalidOperationException();
					break;
				case TokenKind.PrimitiveType:
					if (!toTypeInfo.TryGetValue(info.value, out var typeInfo))
						throw new InvalidOperationException($"Unknown type '{info.value}, comment: {documentation}");
					sb.Append("{@code ");
					sb.Append(Escape(idConverter.Type(typeInfo.type)));
					sb.Append("}");
					if (!string.IsNullOrEmpty(info.value2))
						throw new InvalidOperationException();
					break;
				case TokenKind.Type:
					javadocType = TypeToJavaDocName(info.value);
					sb.Append($"{{@link {javadocType}}}");
					if (!string.IsNullOrEmpty(info.value2))
						throw new InvalidOperationException();
					break;
				case TokenKind.EnumFieldReference:
				case TokenKind.FieldReference:
				case TokenKind.Property:
				case TokenKind.Method:
					javadocType = TypeToJavaDocName(info.value);
					javadocMember = MemberToJavaDocName(info.value, info.value2, info.kind);
					sb.Append($"{{@link {javadocType}#{javadocMember}}}");
					break;
				default:
					throw new InvalidOperationException();
				}
			}
		}

		static string Escape(string value) {
			var s = SecurityElement.Escape(value) ?? throw new InvalidOperationException();
			// JDK 8 will complain if we don't do this
			s = s.Replace("&apos;", "'");
			s = s.Replace(">", "&gt;");
			s = s.Replace("<", "&lt;");
			return s;
		}

		static string TypeToJavaDocName(string type) =>
			type switch {
				"Iced.Intel.Register" => JavaConstants.IcedPackage + ".Register",
				"BlockEncoder" => JavaConstants.BlockEncoderPackage + "." + type,
				"ConstantOffsets" or "CpuidFeature" or "Code" or "Instruction" or "Register" or "RepPrefixKind" => JavaConstants.IcedPackage + "." + type,
				"RelocInfo" => JavaConstants.BlockEncoderPackage + "." + type,
				"SymbolResult" => JavaConstants.FormatterPackage + "." + type,
				_ => throw new InvalidOperationException($"Unknown type: {type}"),
			};

		string MemberToJavaDocName(string type, string member, TokenKind kind) {
			switch (kind) {
			case TokenKind.EnumFieldReference:
				if (Enums.EnumUtils.UppercaseTypeFields(type))
					return member.ToUpperInvariant();
				return idConverter.EnumField(member);
			case TokenKind.FieldReference:
				return idConverter.Field(member);
			case TokenKind.Property:
				return idConverter.PropertyDoc(member);
			case TokenKind.Method:
				switch (member) {
				case "GetOpRegister": return "getOpRegister(int)";
				default: return idConverter.MethodDoc(member);
				}
			default:
				throw new InvalidOperationException();
			}
		}
	}
}
