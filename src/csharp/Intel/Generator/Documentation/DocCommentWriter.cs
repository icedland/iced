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

namespace Generator.Documentation {
	abstract class DocCommentWriter {
		protected enum TokenKind {
			NewParagraph,
			String,
			Code,
			Type,
			EnumFieldReference,
		}

		protected IEnumerable<(TokenKind kind, string value, string value2)> GetTokens(string defaultTypeName, string comment) {
			int index = 0;
			while (index < comment.Length) {
				const string pattern = "#(";
				const string patternEnd = ")#";
				const char newParagraph = 'p';
				const char codeChar = 'c';
				const char typeChar = 't';
				const char enumFieldReferenceChar = 'e';
				// char (eg. 'c') + ':'
				const int kindLen = 1 + 1;

				int nextIndex = comment.IndexOf(pattern, index);
				if (nextIndex < 0)
					nextIndex = comment.Length;
				if (nextIndex != index) {
					yield return (TokenKind.String, comment.Substring(index, nextIndex - index), string.Empty);
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

				var data = comment.Substring(index + kindLen, nextIndex - (index + kindLen));
				switch (type) {
				case newParagraph:
					if (!string.IsNullOrEmpty(data))
						throw new InvalidOperationException($"Invalid comment: {comment}");
					yield return (TokenKind.NewParagraph, data, string.Empty);
					break;

				case codeChar:
					yield return (TokenKind.Code, data, string.Empty);
					break;

				case typeChar:
					yield return (TokenKind.Type, data, string.Empty);
					break;

				case enumFieldReferenceChar:
					string typeName, memberName;
					int typeIndex = data.IndexOf('.');
					if (typeIndex < 0) {
						typeName = defaultTypeName;
						memberName = data;
					}
					else {
						typeName = data.Substring(0, typeIndex);
						memberName = data.Substring(typeIndex + 1);
					}
					yield return (TokenKind.EnumFieldReference, typeName, memberName);
					break;

				default:
					throw new InvalidOperationException($"Invalid char '{type}', comment: {comment}");
				}

				index = nextIndex + patternEnd.Length;
			}
		}
	}
}
