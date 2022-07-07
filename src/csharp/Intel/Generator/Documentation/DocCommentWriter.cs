// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using Generator.Enums.Encoder;
using Generator.Enums.InstructionInfo;
using Generator.Enums;

namespace Generator.Documentation {
	abstract class DocCommentWriter {
		protected enum TokenKind {
			NewParagraph,
			HorizontalLine,
			String,
			Code,
			PrimitiveType,
			Type,
			EnumFieldReference,
			FieldReference,
			Property,
			Method,
		}

		protected static IEnumerable<(TokenKind kind, string value, string value2)> GetTokens(string defaultTypeName, string comment) {
			int index = 0;
			while (index < comment.Length) {
				const string pattern = "#(";
				const string patternEnd = ")#";
				const char newParagraph = 'p';
				const char codeChar = 'c';
				const char primitiveTypeChar = 't';
				const char typeChar = 'r';
				const char enumFieldReferenceChar = 'e';
				const char fieldReferenceChar = 'f';
				const char propertyChar = 'P';
				const char methodChar = 'M';
				const char horizontalLineChar = 'h';
				// char (eg. 'c') + ':'
				const int kindLen = 1 + 1;

				int nextIndex = comment.IndexOf(pattern, index);
				if (nextIndex < 0)
					nextIndex = comment.Length;
				if (nextIndex != index) {
					yield return (TokenKind.String, comment[index..nextIndex], string.Empty);
					index = nextIndex;
				}
				if (index == comment.Length)
					break;
				index += pattern.Length;
				if (index + kindLen > comment.Length)
					throw new InvalidOperationException($"Invalid comment: {comment}");
				var type = comment[index];
				if (comment[index + 1] != ':')
					throw new InvalidOperationException($"Invalid comment: {comment}");
				nextIndex = comment.IndexOf(patternEnd, index + kindLen);
				if (nextIndex < 0)
					throw new InvalidOperationException($"Invalid comment: {comment}");

				string typeName, memberName;
				var data = comment[(index + kindLen)..nextIndex];
				switch (type) {
				case newParagraph:
					if (!string.IsNullOrEmpty(data))
						throw new InvalidOperationException($"Invalid comment: {comment}");
					yield return (TokenKind.NewParagraph, data, string.Empty);
					break;

				case codeChar:
					yield return (TokenKind.Code, data, string.Empty);
					break;

				case primitiveTypeChar:
					yield return (TokenKind.PrimitiveType, data, string.Empty);
					break;

				case typeChar:
					yield return (TokenKind.Type, data, string.Empty);
					break;

				case enumFieldReferenceChar:
					(typeName, memberName) = SplitMember(data, defaultTypeName);
					yield return (TokenKind.EnumFieldReference, typeName, memberName);
					break;

				case fieldReferenceChar:
					(typeName, memberName) = SplitMember(data, defaultTypeName);
					yield return (TokenKind.FieldReference, typeName, memberName);
					break;

				case propertyChar:
					(typeName, memberName) = SplitMember(data, defaultTypeName);
					yield return (TokenKind.Property, typeName, memberName);
					break;

				case methodChar:
					(typeName, memberName) = SplitMember(data, defaultTypeName);
					yield return (TokenKind.Method, typeName, memberName);
					break;

				case horizontalLineChar:
					(typeName, memberName) = SplitMember(data, defaultTypeName);
					yield return (TokenKind.HorizontalLine, typeName, memberName);
					break;

				default:
					throw new InvalidOperationException($"Invalid char '{type}', comment: {comment}");
				}

				index = nextIndex + patternEnd.Length;
			}
		}

		static (string type, string name) SplitMember(string s, string defaultTypeName) {
			string typeName, memberName;
			int typeIndex = s.IndexOf('.', StringComparison.Ordinal);
			if (typeIndex < 0) {
				typeName = defaultTypeName;
				memberName = s;
			}
			else {
				typeName = s[0..typeIndex];
				memberName = s[(typeIndex + 1)..];
			}
			return (typeName, memberName);
		}

		protected static string RemoveNamespace(string type) {
			int i = type.LastIndexOf('.');
			if (i < 0)
				return type;
			return type[(i + 1)..];
		}

		protected static string GetTypeKind(string name) =>
			name switch {
				nameof(Code) or nameof(CpuidFeature) or nameof(OpKind) or nameof(Register) or nameof(RepPrefixKind) => "enum",
				"BlockEncoder" or "ConstantOffsets" or "Instruction" or "RelocInfo" or "SymbolResult" => "struct",
				_ => throw new InvalidOperationException(),
			};

		protected static string GetMethodNameOnly(string name) {
			int index = name.IndexOf('(', StringComparison.Ordinal);
			if (index < 0)
				return name;
			return name[0..index];
		}

		protected static string TranslateMethodName(string name) {
			const string GetPattern = "Get";
			if (name.StartsWith(GetPattern, StringComparison.Ordinal))
				return name[GetPattern.Length..];
			return name;
		}
	}
}
