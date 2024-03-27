// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;

namespace Generator.Misc.Python {
	sealed class PyClassParser {
		const string selfArgName = "$self";
		readonly string filename;
		readonly Lines lines;
		readonly List<string> docComments;
		RustAttributes? attributes;
		readonly Dictionary<string, PyClass> pyClasses;

		public PyClassParser(string filename) {
			this.filename = filename;
			lines = new(File.ReadAllLines(filename));
			docComments = new();
			pyClasses = new(StringComparer.Ordinal);
		}

		PyClass[] GetClasses() => pyClasses.Values.ToArray();

		bool TryGetPyClass(string name, [NotNullWhen(true)] out PyClass? pyClass) =>
			pyClasses.TryGetValue(name, out pyClass);

		void AddPyClass(PyClass pyClass) {
			if (pyClasses.ContainsKey(pyClass.Name))
				throw GetException($"Duplicate struct {pyClass.Name}");
			pyClasses.Add(pyClass.Name, pyClass);
		}

		void ClearTempState() {
			docComments.Clear();
			attributes = null;
		}

		bool HasTempState => docComments.Count != 0 || attributes is not null;

		public Exception GetException(string message) =>
			new InvalidOperationException($"{message}, line: {lines.LineNo}, file: {filename}");

		enum LineKind {
			Eof,
			Other,
			DocComment,
			Attribute,
			Struct,
			Impl,
			Fn,
		}

		sealed class Lines {
			readonly string[] lines;
			int index;

			public int LineNo => index + 1;

			public Lines(string[] lines) => this.lines = lines;

			public string GetLine(int lineNo) => lines[lineNo - 1];

			public void Skip() {
				if (index < lines.Length)
					index++;
			}

			public (LineKind kind, string line) Next() {
				var token = Peek();
				Skip();
				return token;
			}

			public (LineKind kind, string line) Peek() {
				if (index >= lines.Length)
					return (LineKind.Eof, string.Empty);
				var line = lines[index];
				return (GetKind(line), line);
			}

			static LineKind GetKind(string line) {
				var trimmed = line.Trim();
				if (trimmed.StartsWith("///", StringComparison.Ordinal))
					return LineKind.DocComment;
				if (trimmed.StartsWith("//", StringComparison.Ordinal))
					return LineKind.Other;
				if (trimmed.StartsWith("#[", StringComparison.Ordinal))
					return LineKind.Attribute;
				if (trimmed.StartsWith("struct ", StringComparison.Ordinal) || trimmed.Contains(" struct ", StringComparison.Ordinal))
					return LineKind.Struct;
				if (trimmed.StartsWith("impl ", StringComparison.Ordinal))
					return LineKind.Impl;
				if (trimmed.StartsWith("fn ", StringComparison.Ordinal) ||
					trimmed.StartsWith("pub fn ", StringComparison.Ordinal) ||
					trimmed.StartsWith("pub(crate) fn ", StringComparison.Ordinal)) {
					return LineKind.Fn;
				}
				return LineKind.Other;
			}
		}

		public PyClass[] ParseFile() {
			while (true) {
				var token = lines.Next();
				if (token.kind == LineKind.Eof)
					break;

				switch (token.kind) {
				case LineKind.Other:
					ClearTempState();
					break;

				case LineKind.DocComment:
					AddDocCommentLine(token.line);
					break;

				case LineKind.Attribute:
					ReadAttribute(token.line);
					break;

				case LineKind.Struct:
					ReadStruct(token.line);
					SkipBlock(token.line);
					break;

				case LineKind.Impl:
					if (docComments.Count != 0)
						throw GetException("Unexpected doc comments");
					var implName = GetName(token.line, "impl");
					if (!TryGetPyClass(implName, out var pyClass) ||
						attributes is null ||
						attributes.Attributes.Count == 0) {
						SkipBlock(token.line);
					}
					else {
						if (!attributes.Any(AttributeKind.PyMethods))
							SkipBlock(token.line);
						else
							ReadStructImpl(token.line, pyClass);
					}
					break;

				case LineKind.Fn:
					SkipBlock(token.line);
					break;

				case LineKind.Eof:
				default:
					throw GetException($"Unexpected token {token.kind}");
				}
			}

			var classes = GetClasses();
			foreach (var pyClass in classes) {
				var pyClassAttr = pyClass.Attributes.Attributes.FirstOrDefault(a => a.Kind == AttributeKind.PyClass);
				const string expectedPyClassAttr = "#[pyclass(module = \"iced_x86._iced_x86_py\")]";
				if (pyClassAttr?.Text != expectedPyClassAttr)
					throw GetException($"Class {pyClass.Name}: Expected this #[pyclass] attribute: {expectedPyClassAttr}");

				var ctor = pyClass.Methods.FirstOrDefault(a => a.Attributes.Any(AttributeKind.New));
				int argsSectCount = pyClass.DocComments.Sections.OfType<ArgsDocCommentSection>().Count();
				int expectedArgsSectCount = (ctor?.Arguments.Count ?? 0) == 0 ? 0 : 1;
				if (argsSectCount != expectedArgsSectCount)
					throw GetException($"Class {pyClass.Name}: Expected exactly {expectedArgsSectCount} `Args:` sections but found {argsSectCount}");
				if (ctor is not null) {
					var sigAttr = pyClass.Attributes.Attributes.FirstOrDefault(a => a.Kind == AttributeKind.Signature);
					if (sigAttr is not null)
						throw GetException($"Class {pyClass.Name}: The ctor should have the #[pyo3(signature = (...))] attribute");

					if (!CheckArgsSectionAndMethodArgs(pyClass.DocComments, ctor.Arguments, out var error))
						throw GetException($"Class {pyClass.Name}: {error}");
				}
			}

			return classes;
		}

		void ReadStruct(string line) {
			if (attributes?.Any(AttributeKind.PyClass) == true) {
				line = RemovePub(line);
				var name = GetName(line, "struct");
				if (!TryCreateDocComments(docComments, out var docComments2, out var error))
					throw GetException(error);
				var pyClass = new PyClass(name, docComments2, attributes ?? new RustAttributes());
				AddPyClass(pyClass);
			}
			ClearTempState();
		}

		void ReadStructImpl(string implLine, PyClass pyClass) {
			ClearTempState();
			var endOfBlockStr = GetIndent(implLine) + "}";
			while (true) {
				var token = lines.Next();
				if (token.kind == LineKind.Eof)
					break;
				if (token.kind == LineKind.Other && token.line == endOfBlockStr)
					break;

				switch (token.kind) {
				case LineKind.Other:
					var trimmed = token.line.Trim();
					if (!(trimmed == string.Empty || trimmed.StartsWith("//", StringComparison.Ordinal)))
						throw GetException("Unexpected line");
					break;

				case LineKind.DocComment:
					AddDocCommentLine(token.line);
					break;

				case LineKind.Attribute:
					ReadAttribute(token.line);
					break;

				case LineKind.Fn:
					foreach (var method in ReadMethod(pyClass, token.line)) {
						if (!IgnoreMethod(method))
							pyClass.Methods.Add(method);
					}
					break;

				case LineKind.Struct:
				case LineKind.Impl:
				case LineKind.Eof:
				default:
					throw GetException($"Unexpected token {token.kind}");
				}
			}

			if (HasTempState)
				throw GetException("Unexpected docs/attrs");
		}

		static bool IgnoreMethod(PyMethod method) =>
			method.Name switch {
				"__traverse__" or "__clear__" or "__next__" => true,
				_ => false
			};

		string RemovePub(string line) {
			if (line.StartsWith("pub", StringComparison.Ordinal)) {
				line = line["pub".Length..];
				if (line.StartsWith("(", StringComparison.Ordinal)) {
					int index = line.IndexOf(')');
					if (index < 0)
						throw GetException("Expected ')'");
					line = line[(index + 1)..];
				}
			}
			return line.TrimStart();
		}

		static IEnumerable<string> GetArgs(string argsLine) {
			var prevValue = string.Empty;
			foreach (var arg in argsLine.Split(',', StringSplitOptions.RemoveEmptyEntries)) {
				if (prevValue == string.Empty) {
					// Check if it's a generic, eg. `PyRef<'_, Instruction>` in which case
					// arg == `PyRef<'_` and next value is ` Instruction>`
					if (arg.Contains('<', StringComparison.Ordinal) && !arg.EndsWith('>'))
						prevValue = arg;
					else
						yield return arg;
				}
				else {
					prevValue = prevValue + "," + arg;
					if (arg.EndsWith('>')) {
						yield return prevValue;
						prevValue = string.Empty;
					}
				}
			}
			if (prevValue != string.Empty)
				throw new InvalidOperationException($"Invalid arg line: `{argsLine}`, prevValue = `{prevValue}`");
		}

		string ParseMethodArgsAndRetType(string fullLine, string line, bool isInstanceMethod, bool isSpecial, out List<PyMethodArg> args, out string rustReturnType) {
			args = new List<PyMethodArg>();

			if (!line.StartsWith('('))
				throw GetException("Expected '('");
			line = line[1..];
			bool foundThis = false;
			int index;
			while (true) {
				line = line.Trim();
				string argsLine;
				index = line.IndexOf(')');
				if (index >= 0) {
					argsLine = line[..index].Trim();
					line = line[index..].Trim();
				}
				else {
					argsLine = line;
					line = string.Empty;
				}
				foreach (var tmp in GetArgs(argsLine)) {
					var argInfo = tmp.Trim();
					if (argInfo == string.Empty)
						continue;
					const string mutPat = "mut ";
					if (argInfo.StartsWith(mutPat, StringComparison.Ordinal))
						argInfo = argInfo[mutPat.Length..].Trim();
					PyMethodArg arg;
					switch (argInfo) {
					case "&mut self":
					case "&self":
						arg = new PyMethodArg(selfArgName, argInfo, isSelf: true);
						break;
					default:
						index = argInfo.IndexOf(':');
						if (index < 0)
							throw GetException("Expected `:`");
						var name = argInfo[..index].Trim();
						var rustType = argInfo[(index + 1)..].Trim();
						if (rustType.StartsWith("Python<", StringComparison.Ordinal)) {
							if (name != "py" && name != "_py")
								throw GetException("Expected name to be `py` or `_py`");
							continue;
						}
						if (name.StartsWith('_')) {
							if (!isSpecial)
								throw GetException($"Unused arg {name}");
							name = name[1..];
						}
						bool isSelf = false;
						if (rustType == "PyRef<'_, Self>" || rustType == "PyRefMut<'_, Self>") {
							name = selfArgName;
							isSelf = true;
						}
						if (name.Contains(' ', StringComparison.Ordinal))
							throw GetException($"Name has a space: `{name}`");
						arg = new PyMethodArg(name, rustType, isSelf);
						break;
					}
					if (arg.IsSelf && args.Count != 0)
						throw GetException("`self` must be the first arg");
					foundThis |= arg.IsSelf;
					args.Add(arg);
				}

				if (line.StartsWith(')'))
					break;
				if (line != string.Empty)
					throw GetException("Internal error");
				var token = lines.Next();
				if (token.kind == LineKind.Eof)
					throw GetException("Unexpected EOF");
				fullLine = token.line;
				line = fullLine.Trim();
			}
			if (foundThis != isInstanceMethod) {
				if (isInstanceMethod)
					throw GetException("Expected `self` as first arg");
				throw GetException("`self` found when not an instance method");
			}

			index = line.IndexOf("->");
			if (index >= 0) {
				line = line[(index + 2)..].Trim();
				index = line.IndexOf('{');
				if (index < 0)
					throw GetException("Expected `{`");
				rustReturnType = line[..index].Trim();
				line = line[index..];
			}
			else
				rustReturnType = string.Empty;
			return fullLine;
		}

		static string GetExpectedTextSignature(PyMethod method) {
			var sb = new StringBuilder();
			sb.Append("#[pyo3(text_signature = \"(");
			Dictionary<string, string> toDefaultValue;
			var sigAttr = method.Attributes.Attributes.FirstOrDefault(a => a.Kind == AttributeKind.Signature);
			if (sigAttr is null)
				toDefaultValue = new Dictionary<string, string>(StringComparer.Ordinal);
			else
				toDefaultValue = ParseUtils.GetArgsNameValues(sigAttr.Text).ToDictionary(a => a.name, a => a.value, StringComparer.Ordinal);
			for (int i = 0; i < method.Arguments.Count; i++) {
				if (i > 0)
					sb.Append(", ");
				var arg = method.Arguments[i];
				sb.Append(arg.Name);
				if (toDefaultValue.TryGetValue(arg.Name, out var defaultValue)) {
					sb.Append(" = ");
					sb.Append(defaultValue);
				}
			}
			sb.Append(")\")]");
			return sb.ToString();
		}

		static bool CheckSignatureAttribute(PyMethod method, string? argsAttr) {
			if (argsAttr is null)
				return true;

			bool isStaticMethod = method.Attributes.Any(AttributeKind.StaticMethod);
			bool isClassMethod = method.Attributes.Any(AttributeKind.ClassMethod);
			bool isCtor = method.Attributes.Any(AttributeKind.New);
			bool isInstanceMethod = !isStaticMethod && !isClassMethod && !isCtor;

			if (!ParseUtils.TryGetSignaturePayload(argsAttr, out var s))
				return false;
			var attrArgs = s.Split(',').ToList();
			if (isInstanceMethod && (attrArgs.Count == 0 || attrArgs[0] != "$self"))
				attrArgs.Insert(0, selfArgName);
			if (attrArgs.Count != method.Arguments.Count)
				return false;
			for (int i = 0; i < attrArgs.Count; i++) {
				var attrArg = attrArgs[i].Trim();
				int index = attrArg.IndexOf('=', StringComparison.Ordinal);
				var attrArgName = index < 0 ? attrArg : attrArg[..index].Trim();
				var argName = method.Arguments[i].Name;
				if (attrArgName != argName)
					return false;
			}

			return true;
		}

		static bool CheckArgsSectionAndMethodArgs(DocComments docComments, List<PyMethodArg> arguments, [NotNullWhen(false)] out string? error) {
			var argsSection = docComments.Sections.OfType<ArgsDocCommentSection>().FirstOrDefault();
			if (arguments.Count == 0) {
				if (argsSection is not null) {
					error = "Unexpected `Args:` section";
					return false;
				}
			}
			else {
				int argsSectionLength = argsSection?.Args.Length ?? 0;
				int hasThis = arguments.Count != 0 && arguments[0].IsSelf ? 1 : 0;
				int expectedMethodArgs = arguments.Count - hasThis;
				if (argsSectionLength != expectedMethodArgs) {
					error = $"Expected `Args:` section with {expectedMethodArgs} but found {argsSectionLength} documented args";
					return false;
				}
				for (int i = hasThis; i < arguments.Count; i++) {
					var methodArg = arguments[i].Name;
					var argsArg = argsSection!.Args[i - hasThis].Name;
					if (methodArg != argsArg) {
						error = $"`Args:` section not sorted or using the wrong name. Expected `{methodArg}` but found `{argsArg}`";
						return false;
					}
				}
			}

			error = null;
			return true;
		}

		IEnumerable<PyMethod> ReadMethod(PyClass pyClass, string fullLine) {
			var line = RemovePub(fullLine.Trim());
			const string fnPat = "fn ";
			if (!line.StartsWith(fnPat, StringComparison.Ordinal))
				throw GetException("Expected `fn`");
			line = line[fnPat.Length..].Trim();

			int index = line.IndexOf('(');
			if (index < 0)
				throw GetException("Expected `(`");
			var name = line[..index].Trim();
			line = line[index..];
			index = name.IndexOf('<', StringComparison.Ordinal);
			if (index >= 0)
				name = name[..index].Trim();

			var attributes = this.attributes ?? new RustAttributes();
			bool isStaticMethod = attributes.Any(AttributeKind.StaticMethod);
			bool isClassMethod = attributes.Any(AttributeKind.ClassMethod);
			bool isCtor = attributes.Any(AttributeKind.New);
			bool isInstanceMethod = !isStaticMethod && !isClassMethod && !isCtor;
			if (isStaticMethod && isClassMethod)
				throw GetException("Method can't be both classmethod and staticmethod");

			bool isSpecial = name.StartsWith("__", StringComparison.Ordinal) &&
				name.EndsWith("__", StringComparison.Ordinal) &&
				name != "__copy__" && name != "__deepcopy__" && name != "__getstate__";

			fullLine = ParseMethodArgsAndRetType(fullLine, line, isInstanceMethod, isSpecial || name == "__deepcopy__", out var args, out var rustReturnType);
			if (!TryCreateDocComments(docComments, out var docComments2, out var error))
				throw GetException(error);

			bool isSetter = attributes.Any(AttributeKind.Setter);
			bool isGetter = attributes.Any(AttributeKind.Getter);

			if (isSetter && name.StartsWith("set_"))
				name = name["set_".Length..];
			if (isGetter && name.StartsWith("get_"))
				throw GetException($"Getters shouldn't have a `get_` prefix: {name}");

			var method = new PyMethod(name, docComments2, attributes, args, rustReturnType);

			var sigAttr = method.Attributes.Attributes.FirstOrDefault(a => a.Kind == AttributeKind.Signature);
			if (isSpecial || isGetter || isSetter) {
				if (sigAttr is not null)
					throw GetException("Unexpected #[pyo3(signature = (...))] attribute found");
			}
			else {
				if (!CheckSignatureAttribute(method, sigAttr?.Text))
					throw GetException($"Invalid #[pyo3(signature = (...))] attribute: {sigAttr?.Text}");
			}

			if (!(isSpecial || isGetter || isSetter || isCtor)) {
				int count = method.Attributes.Attributes.Count(a => a.Kind == AttributeKind.TextSignature);
				if (count != 1)
					throw GetException("Expected exactly one #[pyo3(text_signature ...)] attribute");
				var expectedTextSig = GetExpectedTextSignature(method);
				var textSigAttr = method.Attributes.Attributes.First(a => a.Kind == AttributeKind.TextSignature);
				if (textSigAttr.Text != expectedTextSig)
					throw GetException($"#[pyo3(text_signature ...)] didn't match the expected value: {expectedTextSig}");
			}

			if (isSetter) {
				if (method.DocComments.Sections.Count > 0)
					throw GetException($"Setters should have no docs, only getters should have docs: {name}");
				const string setterArgName = "new_value";
				if (method.Arguments.Count != 2)
					throw GetException($"Invalid number of setter arguments, expected 2 but found {method.Arguments.Count}");
				if (method.Arguments[1].Name != setterArgName)
					throw GetException($"Setter argument name must be `{setterArgName}` not {method.Arguments[1].Name}");
			}
			else {
				if (method.DocComments.Sections.OfType<ArgsDocCommentSection>().Count() > 1)
					throw GetException("Too many `Args:` sections");
				if (method.DocComments.Sections.OfType<RaisesDocCommentSection>().Count() > 1)
					throw GetException("Too many `Raises:` sections");
				if (isGetter) {
					if (method.DocComments.Sections.OfType<ReturnsDocCommentSection>().Any())
						throw GetException("Setters should have no `Returns:` sections. The return type should be the first type on the first doc line, eg. `int: Some docs here`");
					if (method.DocComments.Sections.FirstOrDefault() is not TextDocCommentSection sect || sect.Lines.Length == 0)
						throw GetException("Expected first doc comments section to be text");
					if (!ParseUtils.TryParseTypeAndDocs(sect.Lines[0], out _, out _))
						throw GetException("First data on the first line must be the property type");
				}
				else if (isCtor) {
					if (method.DocComments.Sections.Count != 0)
						throw GetException("Ctors should have no docs, they should be part of the class' docs");
				}
				else {
					if (!isSpecial && method.DocComments.Sections.Count == 0)
						throw GetException($"Missing documentation: {name}");
					if (!isSpecial && method.HasReturnType) {
						if (method.DocComments.Sections.OfType<ReturnsDocCommentSection>().Count() != 1)
							throw GetException("Expected exactly one `Returns:` section");
					}
					else {
						if (method.DocComments.Sections.OfType<ReturnsDocCommentSection>().Any())
							throw GetException("Expected no `Returns:` sections");
					}
				}
			}

			if (!isSpecial && !isGetter && !isSetter && !isCtor) {
				if (!CheckArgsSectionAndMethodArgs(method.DocComments, method.Arguments, out error))
					throw GetException(error);
			}

			var (startLine, endLine) = SkipBlock(fullLine);
			ClearTempState();

			if (method.Name == "__richcmp__") {
				var seenCompareOps = new HashSet<CompareOp>();
				for (int lineNo = startLine; lineNo < endLine; lineNo++) {
					line = lines.GetLine(lineNo);
					foreach (var compareOp in GetCompareOps(line)) {
						if (!seenCompareOps.Add(compareOp))
							throw GetException("Duplicate CompareOp found in the method");
						var opName = compareOp switch {
							CompareOp.Lt => "__lt__",
							CompareOp.Le => "__le__",
							CompareOp.Eq => "__eq__",
							CompareOp.Ne => "__ne__",
							CompareOp.Gt => "__gt__",
							CompareOp.Ge => "__ge__",
							_ => throw new InvalidOperationException(),
						};
						var newArgs = new List<PyMethodArg> {
							new PyMethodArg(selfArgName, "&self", isSelf: true),
							new PyMethodArg("other", "&Bound<'_, PyAny>", isSelf: false),
						};
						yield return new PyMethod(opName, method.DocComments, method.Attributes, newArgs, "bool");
					}
				}
			}
			else
				yield return method;
		}

		enum CompareOp {
			Lt,
			Le,
			Eq,
			Ne,
			Gt,
			Ge,
		}

		static IEnumerable<CompareOp> GetCompareOps(string line) {
			if (line.Contains("CompareOp::Lt", StringComparison.Ordinal)) yield return CompareOp.Lt;
			if (line.Contains("CompareOp::Le", StringComparison.Ordinal)) yield return CompareOp.Le;
			if (line.Contains("CompareOp::Eq", StringComparison.Ordinal)) yield return CompareOp.Eq;
			if (line.Contains("CompareOp::Ne", StringComparison.Ordinal)) yield return CompareOp.Ne;
			if (line.Contains("CompareOp::Gt", StringComparison.Ordinal)) yield return CompareOp.Gt;
			if (line.Contains("CompareOp::Ge", StringComparison.Ordinal)) yield return CompareOp.Ge;
		}

		void AddDocCommentLine(string line) {
			line = line.TrimStart();
			const string DocCommentPrefix = "///";
			if (!line.StartsWith(DocCommentPrefix, StringComparison.Ordinal))
				throw GetException("Expected a doc comment");
			var docComment = line[DocCommentPrefix.Length..];
			if (docComment.StartsWith(" ", StringComparison.Ordinal))
				docComment = docComment[1..];
			docComments.Add(docComment);
		}

		void ReadAttribute(string line) {
			attributes ??= new RustAttributes();
			attributes.Attributes.Add(ParseAttribute(line));
		}

		RustAttribute ParseAttribute(string line) {
			var attrLine = line.Trim();
			var fullAttrLine = attrLine;
			const string pyo3AttrPrefix = "#[pyo3(";
			const string attrPrefix = "#[";
			if (attrLine.StartsWith(pyo3AttrPrefix, StringComparison.Ordinal))
				attrLine = attrLine[pyo3AttrPrefix.Length..];
			else if (attrLine.StartsWith(attrPrefix, StringComparison.Ordinal))
				attrLine = attrLine[attrPrefix.Length..];
			else
				throw GetException("Expected an attribute");
			int index = attrLine.IndexOfAny(new[] { '(', ' ', '=', ']' });
			if (index < 0)
				throw GetException("Invalid attribute");
			var attrName = attrLine.Substring(0, index);
			var attrKind = attrName switch {
				"pyclass" => AttributeKind.PyClass,
				"pymethods" => AttributeKind.PyMethods,
				"new" => AttributeKind.New,
				"getter" => AttributeKind.Getter,
				"setter" => AttributeKind.Setter,
				"staticmethod" => AttributeKind.StaticMethod,
				"classmethod" => AttributeKind.ClassMethod,
				"text_signature" => AttributeKind.TextSignature,
				"signature" => AttributeKind.Signature,
				"derive" or "allow" or "rustfmt::skip" or "macro_use" or
				"pymodule" or "inline" => AttributeKind.Ignored,
				// Don't ignore unknown attrs by default. We must know what the attribute does
				// so we don't ignore an important one that gets added in the future.
				_ => throw GetException($"Unknown attribute: `{attrName}`"),
			};
			return new RustAttribute(attrKind, fullAttrLine);
		}

		string GetName(string line, string keyword) {
			int index = line.IndexOf(keyword + " ", StringComparison.Ordinal);
			if (index < 0)
				throw GetException($"Expected `{keyword}`");
			var name = line[(index + keyword.Length + 1)..].Trim();
			index = name.IndexOf('{');
			if (index < 0)
				index = name.Length;
			name = name.Substring(0, index);
			const string forString = " for ";
			index = name.IndexOf(forString);
			if (index >= 0)
				name = name[(index + forString.Length)..];
			name = name.Trim();
			if (name.Contains(' ', StringComparison.Ordinal))
				throw GetException($"Found whitespace in `id` after keyword `{keyword}`");
			return name;
		}

		static string GetIndent(string line) {
			var trimmed = line.TrimStart();
			return line.Substring(0, line.IndexOf(trimmed, StringComparison.Ordinal));
		}

		(int startLine, int endLine) SkipBlock(string line) {
			if (line.EndsWith(';') || line.EndsWith('}')) {
				ClearTempState();
				return (0, 0);
			}

			var expectedIndent = GetIndent(line);
			var expected = expectedIndent + "}";
			bool seenOpeningBlock = line.EndsWith("{", StringComparison.Ordinal);
			var startLine = lines.LineNo;
			while (true) {
				var token = lines.Next();
				if (token.kind == LineKind.Eof)
					throw GetException("Unexpected EOF");
				if (token.line != string.Empty) {
					var indent = GetIndent(token.line);
					if (indent.Length < expectedIndent.Length)
						throw GetException("New indent < current indent");
					if (indent.Length == expectedIndent.Length) {
						if (!seenOpeningBlock &&
							token.line.EndsWith("{", StringComparison.Ordinal) &&
							expectedIndent == GetIndent(token.line)) {
							seenOpeningBlock = true;
						}
						else {
							if (expected != token.line)
								throw GetException("Expected end of block");
							if (!seenOpeningBlock)
								throw GetException("Missing `{` at the start of the block");
							break;
						}
					}
				}
			}
			ClearTempState();
			return (startLine, lines.LineNo - 1);
		}

		static DocCommentKind GetDocCommentKind(string line) =>
			line switch {
				"Args:" => DocCommentKind.Args,
				"Raises:" => DocCommentKind.Raises,
				"Returns:" => DocCommentKind.Returns,
				"Note:" => DocCommentKind.Note,
				"Warning:" => DocCommentKind.Warning,
				".. testcode::" => DocCommentKind.TestCode,
				".. testoutput::" => DocCommentKind.TestOutput,
				_ => DocCommentKind.Text,
			};

		static bool TryCreateDocComments(List<string> lines, [NotNullWhen(true)] out DocComments? docComments, [NotNullWhen(false)] out string? error) {
			docComments = null;

			var docs = new DocComments();
			var currentTextLines = new List<string>();
			for (int i = 0; i < lines.Count; i++) {
				var line = lines[i];
				if (line == string.Empty) {
					AddCurrentText(docs, currentTextLines);
					continue;
				}
				if (char.IsWhiteSpace(line[0]))
					throw new InvalidOperationException($"Doc line starts with whitespace: `{line}`");

				var kind = GetDocCommentKind(line);
				if (kind != DocCommentKind.Text && currentTextLines.Count != 0)
					AddCurrentText(docs, currentTextLines);
				switch (kind) {
				case DocCommentKind.Text:
					currentTextLines.Add(line);
					break;

				case DocCommentKind.Args:
					var args = new List<DocCommentArg>();
					if (!TryGetDescLines(lines, ref i, out var descLines, out error))
						return false;
					foreach (var argLine in descLines) {
						if (!TryParseDocCommentArg(argLine, out error, out var arg))
							return false;
						args.Add(arg);
					}
					if (args.Count == 0) {
						error = "Missing `Args` info lines";
						return false;
					}
					docs.Sections.Add(new ArgsDocCommentSection(args.ToArray()));
					break;

				case DocCommentKind.Raises:
					var raises = new List<TypeAndDocs>();
					if (!TryGetDescLines(lines, ref i, out descLines, out error))
						return false;
					foreach (var argLine in descLines) {
						if (!ParseUtils.TryParseTypeAndDocs(argLine, out error, out var raisesInfo))
							return false;
						raises.Add(raisesInfo);
					}
					if (raises.Count == 0) {
						error = "Missing `Raises` info lines";
						return false;
					}
					docs.Sections.Add(new RaisesDocCommentSection(raises.ToArray()));
					break;

				case DocCommentKind.Returns:
					TypeAndDocs? returns = null;
					if (!TryGetDescLines(lines, ref i, out descLines, out error))
						return false;
					foreach (var argLine in descLines) {
						if (returns is not null) {
							error = "Multiple `Returns` info lines";
							return false;
						}
						if (!ParseUtils.TryParseTypeAndDocs(argLine, out error, out var returnsTmp))
							return false;
						returns = returnsTmp;
					}
					if (returns is null) {
						error = "Missing `Returns` info lines";
						return false;
					}
					docs.Sections.Add(new ReturnsDocCommentSection(returns.Value));
					break;

				case DocCommentKind.Note:
					if (!TryGetDescLines(lines, ref i, out descLines, out error))
						return false;
					docs.Sections.Add(new NoteDocCommentSection(descLines.ToArray()));
					break;

				case DocCommentKind.Warning:
					if (!TryGetDescLines(lines, ref i, out descLines, out error))
						return false;
					docs.Sections.Add(new WarningDocCommentSection(descLines.ToArray()));
					break;

				case DocCommentKind.TestCode:
					if (!TryReadExampleLines(lines, ref i, out var exampleLines, out error))
						return false;
					docs.Sections.Add(new TestCodeDocCommentSection(exampleLines));
					break;

				case DocCommentKind.TestOutput:
					if (!TryReadExampleLines(lines, ref i, out var outputLines, out error))
						return false;
					docs.Sections.Add(new TestOutputDocCommentSection(outputLines));
					break;

				default:
					throw new InvalidOperationException();
				}
			}

			AddCurrentText(docs, currentTextLines);

			docComments = docs;
			error = null;
			return true;

			static void AddCurrentText(DocComments docs, List<string> textLines) {
				if (textLines.Count != 0) {
					docs.AddText(new TextDocCommentSection(textLines.ToArray()));
					textLines.Clear();
				}
			}
		}

		static bool TryReadExampleLines(List<string> lines, ref int i, [NotNullWhen(true)] out string[]? resultLines, [NotNullWhen(false)] out string? error) {
			resultLines = null;

			if (!lines[i].StartsWith(".. ", StringComparison.Ordinal)) {
				error = "Expected `.. `";
				return false;
			}
			i++;
			if (i >= lines.Count) {
				error = "Missing example/output lines";
				return false;
			}
			if (!string.IsNullOrEmpty(lines[i])) {
				error = "First example/output line must be an empty line";
				return false;
			}

			bool lastLineWasEmpty = false;
			var result = new List<string>();
			for (; i + 1 < lines.Count; i++) {
				var line = lines[i + 1];

				if (line == string.Empty)
					lastLineWasEmpty = true;
				else {
					const string indent = "    ";
					if (!line.StartsWith(indent, StringComparison.Ordinal)) {
						if (lastLineWasEmpty)
							break;
						error = $"Expected indented text: `/// {indent}text here`";
						return false;
					}
					line = line[indent.Length..];
					lastLineWasEmpty = false;
				}
				result.Add(line);
			}

			while (result.Count > 0 && result[^1] == string.Empty)
				result.RemoveAt(result.Count - 1);
			if (result.Count == 0) {
				error = "Missing example/output lines";
				return false;
			}

			resultLines = result.ToArray();
			error = null;
			return true;
		}

		static bool TryParseDocCommentArg(string argLine, [NotNullWhen(false)] out string? error, out DocCommentArg arg) {
			arg = default;
			int index = argLine.IndexOf(' ');
			if (index < 0) {
				error = "Expected ' '";
				return false;
			}
			var name = argLine[..index];
			if (name.StartsWith('`')) {
				if (!name.EndsWith('`')) {
					error = "Expected ' '";
					return false;
				}
				name = name[1..^1];
			}
			if (name.Contains(' ')) {
				error = $"Found a space in the name: `{name}`";
				return false;
			}

			argLine = argLine[index..];
			index = argLine.IndexOf('(', StringComparison.Ordinal);
			if (index < 0) {
				error = "Expected '('";
				return false;
			}
			const string pattern = "):";
			int endIndex = argLine.IndexOf(pattern, StringComparison.Ordinal);
			if (endIndex < 0) {
				error = $"Expected '{pattern}'";
				return false;
			}
			var sphinxType = argLine[(index + 1)..endIndex];
			var docs = argLine[(endIndex + pattern.Length)..].Trim();

			arg = new DocCommentArg(name, sphinxType, docs);
			error = null;
			return true;
		}

		static bool TryGetDescLines(List<string> lines, ref int index, [NotNullWhen(true)] out List<string>? result, [NotNullWhen(false)] out string? error) {
			result = null;

			var descLines = new List<string>();
			string? expectedIndent = null;
			while (index + 1 < lines.Count) {
				var line = lines[index + 1];
				if (!line.StartsWith(' '))
					break;
				if (expectedIndent is null)
					expectedIndent = GetIndent(line);
				var indent = GetIndent(line);
				if (indent != expectedIndent) {
					error = "Invalid indent";
					return false;
				}
				descLines.Add(line.TrimStart());
				index++;
			}
			result = descLines;
			error = null;
			return true;
		}
	}
}
