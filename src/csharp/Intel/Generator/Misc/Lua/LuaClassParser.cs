// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Generator.Misc.Lua {
	sealed class LuaClassParser {
		const string ConstructorName = "new";
		const RegexOptions MyRegexOptions = RegexOptions.Singleline | RegexOptions.CultureInvariant;
		static readonly Regex exportNameRegex = new Regex(@"\b([A-Z][A-Z_]+)\b", MyRegexOptions);
		static readonly Regex structModMacroRegex = new Regex(@"lua_struct_module!\s*{\s*luaopen_(\w+)\s*:\s*(\w+)\s*}", MyRegexOptions);
		static readonly Regex pubMethodsRegex = new Regex(@"lua_pub_methods!\s*{\s*static\s+(\w+)\s*=>", MyRegexOptions);
		static readonly Regex moduleRegex = new Regex(@"fn luaopen_(\w+)\(lua\)", MyRegexOptions);
		readonly string filename;
		readonly Lines lines;
		readonly List<string> docComments;
		// eg. "DECODER_EXPORTS" -> "Decoder"
		readonly Dictionary<string, string> exportsNameToClassName;
		// eg. "DECODER_EXPORTS" -> list of all fns
		readonly Dictionary<string, List<LuaMethod>> exportsNameToFuncInfos;
		// eg. "Decoder" -> "iced_x86_Decoder"
		readonly Dictionary<string, string> classNameToLuaModuleName;
		// eg. "Decoder" -> docs
		readonly Dictionary<string, DocComments> classNameToDocs;

		public LuaClassParser(string filename) {
			this.filename = filename;
			lines = new(File.ReadAllLines(filename));
			docComments = new();
			exportsNameToClassName = new(StringComparer.Ordinal);
			exportsNameToFuncInfos = new(StringComparer.Ordinal);
			classNameToLuaModuleName = new(StringComparer.Ordinal);
			classNameToDocs = new(StringComparer.Ordinal);
		}

		void ClearTempState() =>
			docComments.Clear();

		bool HasTempState => docComments.Count != 0;

		public Exception GetException(string message) =>
			new InvalidOperationException($"{message}, line: {lines.LineNo}, file: {filename}");

		enum LineKind {
			Eof,
			Other,
			Attribute,
			DocComment,
			Struct,
			Impl,
			Fn,
			LuaStructModuleMacro,
			LuaPubMethodsMacro,
			LuaModule,
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
				if (trimmed.StartsWith("#[", StringComparison.Ordinal) || trimmed.StartsWith("#!["))
					return LineKind.Attribute;
				if (trimmed.StartsWith("pub(crate) struct ", StringComparison.Ordinal) || trimmed.StartsWith("struct ", StringComparison.Ordinal))
					return LineKind.Struct;
				if (trimmed.StartsWith("impl ", StringComparison.Ordinal))
					return LineKind.Impl;
				if (trimmed.StartsWith("fn ", StringComparison.Ordinal) ||
					trimmed.StartsWith("unsafe fn ", StringComparison.Ordinal)) {
					return LineKind.Fn;
				}
				if (trimmed.StartsWith("lua_struct_module!", StringComparison.Ordinal))
					return LineKind.LuaStructModuleMacro;
				if (trimmed.StartsWith("lua_pub_methods!", StringComparison.Ordinal))
					return LineKind.LuaPubMethodsMacro;
				if (trimmed.StartsWith("lua_module!", StringComparison.Ordinal))
					return LineKind.LuaModule;
				return LineKind.Other;
			}
		}

		public LuaClass[] ParseFile() {
			while (true) {
				var token = lines.Next();
				if (token.kind == LineKind.Eof)
					break;

				switch (token.kind) {
				case LineKind.Other:
					ClearTempState();
					break;

				case LineKind.Attribute:
					// Don't clear doc comments, we need them for the structs
					break;

				case LineKind.DocComment:
					AddDocCommentLine(token.line);
					break;

				case LineKind.Struct:
					ReadStruct(token.line);
					break;

				case LineKind.Impl:
					if (docComments.Count != 0)
						throw GetException("Unexpected doc comments");
					ReadImplExports(token.line);
					break;

				case LineKind.Fn:
					SkipBlock(token.line);
					break;

				case LineKind.LuaStructModuleMacro:
					if (docComments.Count != 0)
						throw GetException("Unexpected doc comments");
					ReadLuaStructModuleMacro(token.line);
					break;

				case LineKind.LuaPubMethodsMacro:
					if (docComments.Count != 0)
						throw GetException("Unexpected doc comments");
					ReadLuaPubMethodsMacro(token.line);
					break;

				case LineKind.LuaModule:
					if (docComments.Count != 0)
						throw GetException("Unexpected doc comments");
					ReadLuaModuleMacro(token.line);
					break;

				case LineKind.Eof:
				default:
					throw GetException($"Unexpected token {token.kind}");
				}
			}

			var classes = CreateClasses();

			foreach (var cls in classes) {
				var clsAnnotsSect = cls.DocComments.Sections.OfType<LuaAnnotationDocCommentSection>().FirstOrDefault();
				if (clsAnnotsSect is null && !cls.Name.EndsWith("Ext"))
					throw new InvalidOperationException($"Class {cls.Name} has no docs");
				if (clsAnnotsSect is not null) {
					if (clsAnnotsSect.Class is not LuaClassAnnot clsAnnot)
						throw new InvalidOperationException($"Class {cls.Name} has no @class in its docs");
					if (clsAnnot.Type.Types.Length != 1 || clsAnnot.Type.Types[0] != cls.Name)
						throw new InvalidOperationException($"Class {cls.Name} documentation has @class {clsAnnot.Type.Types[0]} but expected @class {cls.Name}");
				}

				foreach (var method in cls.Methods) {
					if (method.DocComments.Sections.Count == 0)
						throw new InvalidOperationException($"Method {method.Name} in class {cls.Name} has no docs!");

					var annotsSect = method.DocComments.Sections.OfType<LuaAnnotationDocCommentSection>().FirstOrDefault();
					var paramCount = annotsSect?.Params.Length ?? 0;
					if (method.Args.Length != paramCount)
						throw new InvalidOperationException($"Method {cls.Name}:{method.Name}: expected {method.Args.Length} args but found {paramCount} @param's");
					if (annotsSect is not null) {
						for (int i = 0; i < paramCount; i++) {
							if (method.Args[i].Name != annotsSect.Params[i].Name)
								throw new InvalidOperationException($"Method {cls.Name}:{method.Name}: expected arg name {method.Args[i].Name} but found {annotsSect.Params[i].Name} @param name");
						}
					}
					if (method.Name == ConstructorName) {
						var luaReturn = annotsSect?.Return;
						if (luaReturn is null || luaReturn.Types.Length != 1 || luaReturn.Types[0].Types.Length != 1 || luaReturn.Types[0].Types[0] != cls.Name)
							throw new InvalidOperationException($"Method {cls.Name}:{method.Name}: Need `@return {cls.Name}`");
						if (method.ReturnValueCount != 1)
							throw new InvalidOperationException($"Method {cls.Name}:{method.Name}: Expected 1 return value (`-> 1`)");
					}
				}
			}

			return classes;
		}

		LuaClass[] CreateClasses() {
			var classes = new List<LuaClass>();

			var modulePath = new List<string>();
			foreach (var kv in exportsNameToFuncInfos) {
				var exportsName = kv.Key;
				var methods = kv.Value;
				if (!exportsNameToClassName.TryGetValue(exportsName, out var className)) {
					if (methods.Count == 0)
						continue;
					throw new InvalidOperationException($"Don't know which class exports `{exportsName}`");
				}
				if (!classNameToLuaModuleName.TryGetValue(className, out var luaModuleName))
					throw new InvalidOperationException($"Class `{className}` has no lua module name");
				if (!classNameToDocs.TryGetValue(className, out var classDocs))
					throw new InvalidOperationException($"Class `{className}` has no documentation");

				FixSetterDocs(className, methods);

				const string icedModule = "iced_x86";
				if (!luaModuleName.StartsWith(icedModule + "_"))
					throw new InvalidOperationException($"Module name must start with {icedModule}");
				if (!luaModuleName.EndsWith("_" + className))
					throw new InvalidOperationException($"Module name must end with the class name ({className})");
				modulePath.Clear();
				modulePath.Add(icedModule);
				var remainingPath = luaModuleName[(icedModule.Length + 1)..];
				if (remainingPath == className)
					remainingPath = string.Empty;
				else
					remainingPath = remainingPath[..^(className.Length + 1)];
				modulePath.AddRange(remainingPath.Split('_', StringSplitOptions.RemoveEmptyEntries));
				modulePath.Add(className);

				var luaClass = new LuaClass(className, new LuaModulePath(modulePath.ToArray()), classDocs, methods.ToArray());
				classes.Add(luaClass);
			}

			return classes.ToArray();
		}

		static void FixSetterDocs(string className, List<LuaMethod> methods) {
			var nameToMethod = methods.ToDictionary(m => m.Name);
			for (int i = 0; i < methods.Count; i++) {
				var setter = methods[i];
				const string pat = "set_";
				if (!setter.Name.StartsWith(pat))
					continue;
				var getterName = setter.Name[pat.Length..];
				// A few setters don't have getters, eg. Instruction:set_immediate_i32()
				if (className == "Instruction") {
					if (setter.Name is "set_immediate_i32" or "set_immediate_u32" or "set_immediate_i64" or "set_immediate_u64")
						continue;
					// Getters exist but they have a get_ prefix
					if (setter.Name is "set_declare_byte_value" or "set_declare_word_value" or "set_declare_dword_value" or "set_declare_qword_value")
						continue;
				}
				if (!nameToMethod.TryGetValue(getterName, out var getter))
					throw new InvalidOperationException($"Couldn't find getter {getterName} in class {className}");
				if (setter.DocComments.Sections.Count != 0)
					throw new InvalidOperationException($"Setter {setter.Name} in class {className} should have no docs. They are copied from the getter.");

				var docComments = new DocComments();
				foreach (var doc in getter.DocComments.Sections) {
					DocCommentSection section;
					if (doc is LuaAnnotationDocCommentSection paramSect) {
						var args = new List<LuaParamAnnot>(paramSect.Params.Length + 1);
						args.AddRange(paramSect.Params);
						if (paramSect.Return is not LuaReturnAnnot luaReturn)
							throw new InvalidOperationException($"Getter {getter.Name} in class {className} has no @return");
						if (luaReturn.Types.Length != 1)
							throw new InvalidOperationException($"Getter {getter.Name} in class {className} doesn't have exactly one return type");
						if (setter.Args.Length != getter.Args.Length + 1)
							throw new InvalidOperationException($"Getter/setter arg count mismatch in class {className}");
						string? oldComment = null;
						if (luaReturn.Comment is string returnComment)
							oldComment = $" ({returnComment})";
						args.Add(new LuaParamAnnot(setter.Args[^1].Name, luaReturn.Types[0], $"New value{oldComment}", false));
						section = new LuaAnnotationDocCommentSection(null, null, Array.Empty<LuaOverloadAnnot>(), args.ToArray());
					}
					else
						section = doc;
					docComments.Sections.Add(section);
				}
				methods[i] = new LuaMethod(setter.Kind, setter.Name, docComments, setter.ReturnValueCount, setter.Args);
			}
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

		string GetName(string line, string keyword) {
			int index = line.IndexOf(keyword + " ", StringComparison.Ordinal);
			if (index < 0)
				throw GetException($"Expected `{keyword}`");
			var name = line[(index + keyword.Length + 1)..].Trim();
			index = name.IndexOf('{');
			if (index < 0)
				index = name.IndexOf(';');
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

		void ReadStruct(string structLine) {
			var structName = GetName(structLine, "struct");
			if (!TryCreateDocComments(docComments, out var docComments2, out var error))
				throw GetException(error);
			classNameToDocs.Add(structName, docComments2);
			SkipBlock(structLine);
		}

		void ReadImplExports(string implLine) {
			ClearTempState();
			var implName = GetName(implLine, "impl");
			var endOfBlockStr = GetIndent(implLine) + "}";
			while (true) {
				var token = lines.Next();
				if (token.kind == LineKind.Eof)
					break;
				if (token.kind == LineKind.Other && token.line == endOfBlockStr)
					break;

				switch (token.kind) {
				case LineKind.Other:
					break;

				case LineKind.Fn:
					ReadExportsFn(token.line, implName);
					break;

				case LineKind.Attribute:
					break;

				case LineKind.DocComment:
				case LineKind.Struct:
				case LineKind.Impl:
				case LineKind.LuaStructModuleMacro:
				case LineKind.LuaPubMethodsMacro:
				case LineKind.LuaModule:
				default:
					throw GetException($"Unexpected token {token.kind}");
				}
			}
		}

		void ReadExportsFn(string fullLine, string className) {
			var (startLine, endLine) = SkipBlock(fullLine);
			for (int lineNo = startLine; lineNo < endLine; lineNo++) {
				var line = lines.GetLine(lineNo);
				foreach (Match match in exportNameRegex.Matches(line))
					exportsNameToClassName.Add(match.Value, className);
			}
		}

		void ReadLuaStructModuleMacro(string line) {
			var match = structModMacroRegex.Match(line);
			if (!match.Success || match.Groups.Count != 3)
				throw GetException($"Couldn't parse Lua struct module macro, line: {line}");
			classNameToLuaModuleName.Add(match.Groups[2].Value, match.Groups[1].Value);
		}

		void ReadLuaPubMethodsMacro(string fullLine) {
			var match = pubMethodsRegex.Match(fullLine);
			if (!match.Success || match.Groups.Count != 2)
				throw GetException($"Couldn't parse Lua pub methods macro, line: `{fullLine}`");
			var exportsName = match.Groups[1].Value;
			if (!exportsNameToFuncInfos.TryGetValue(exportsName, out var funcs))
				exportsNameToFuncInfos.Add(exportsName, funcs = new());
			var endOfBlockStr = GetIndent(fullLine) + "}";
			while (true) {
				var token = lines.Next();
				if (token.kind == LineKind.Eof)
					break;
				if (token.kind == LineKind.Other && token.line == endOfBlockStr)
					break;

				switch (token.kind) {
				case LineKind.Other:
					break;

				case LineKind.Fn:
					var method = ReadMethod(token.line);
					funcs.Add(method);
					break;

				case LineKind.DocComment:
					AddDocCommentLine(token.line);
					break;

				case LineKind.Attribute:
					break;

				case LineKind.Struct:
				case LineKind.Impl:
				case LineKind.LuaStructModuleMacro:
				case LineKind.LuaPubMethodsMacro:
				case LineKind.LuaModule:
				default:
					throw GetException($"Unexpected token {token.kind}");
				}
			}
		}

		void ReadLuaModuleMacro(string fullLine) {
			if (!fullLine.EndsWith("lua_module! {"))
				throw GetException($"Couldn't parse Lua module macro, line: `{fullLine}`");
			var endOfBlockStr = GetIndent(fullLine) + "}";
			bool foundFn = false;
			while (true) {
				var token = lines.Next();
				if (token.kind == LineKind.Eof)
					break;
				if (token.kind == LineKind.Other && token.line == endOfBlockStr)
					break;

				switch (token.kind) {
				case LineKind.Other:
					break;

				case LineKind.Fn:
					if (foundFn)
						throw new InvalidOperationException();
					foundFn = true;
					var match = moduleRegex.Match(token.line);
					if (!match.Success || match.Groups.Count != 2)
						throw GetException($"Couldn't parse Lua struct module macro, line: {token.line}");
					var moduleName = match.Groups[1].Value;
					int index = moduleName.LastIndexOf('_');
					if (index < 0)
						throw GetException($"Invalid module name: {moduleName}");
					var structName = moduleName[(index + 1)..];
					classNameToLuaModuleName.Add(structName, moduleName);

					if (!TryCreateDocComments(docComments, out var docComments2, out var error))
						throw GetException(error);
					classNameToDocs.Add(structName, docComments2);

					ReadExportsFn(token.line, structName);
					break;

				case LineKind.DocComment:
					AddDocCommentLine(token.line);
					break;

				case LineKind.Attribute:
					break;

				case LineKind.Struct:
				case LineKind.Impl:
				case LineKind.LuaStructModuleMacro:
				case LineKind.LuaPubMethodsMacro:
				case LineKind.LuaModule:
				default:
					throw GetException($"Unexpected token {token.kind}");
				}
			}
			if (!foundFn)
				throw new InvalidOperationException();
		}

		static IEnumerable<string> GetArgs(string argsLine) {
			var prevValue = string.Empty;
			foreach (var arg in argsLine.Split(',', StringSplitOptions.RemoveEmptyEntries)) {
				if (prevValue == string.Empty) {
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

		string ParseMethodArgsAndRetType(string fullLine, string line, string methodName, out LuaMethodKind kind, out List<LuaMethodArg> args, out uint returnValueCount) {
			args = new List<LuaMethodArg>();
			kind = methodName == ConstructorName ? LuaMethodKind.Constructor : LuaMethodKind.Function;

			if (!line.StartsWith('('))
				throw GetException("Expected '('");
			line = line[1..];
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
				int argIndex = -1;
				int parsedArgs = 0;
				foreach (var tmp in GetArgs(argsLine)) {
					argIndex++;
					var argInfo = tmp.Trim();
					if (argInfo == string.Empty)
						continue;
					parsedArgs++;
					if (argIndex == 0 && argInfo == "lua")
						continue;
					index = argInfo.IndexOf(':');
					if (index < 0)
						throw GetException("Expected `:`");
					var name = argInfo[..index].Trim();
					var rustType = argInfo[(index + 1)..].Trim();
					if (name.StartsWith('_'))
						name = name[1..];
					if (name.Contains(' ', StringComparison.Ordinal))
						throw GetException($"Name has a space: `{name}`");
					if (kind != LuaMethodKind.Constructor && name == "this")
						kind = LuaMethodKind.Method;
					if (argIndex == 1 && kind == LuaMethodKind.Method)
						continue;
					bool isSelf = args.Count == 0 && kind == LuaMethodKind.Method;
					var arg = new LuaMethodArg(name, rustType, isSelf);
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

			index = line.IndexOf("->");
			if (index < 0)
				throw GetException("Expected a return value count");
			line = line[(index + 2)..].Trim();
			index = line.IndexOf('{');
			if (index < 0)
				throw GetException("Expected `{`");
			var numStr = line[..index].Trim();
			if (!uint.TryParse(numStr, out returnValueCount))
				throw GetException($"Not a uint: `{numStr}`");
			return fullLine;
		}

		LuaMethod ReadMethod(string fullLine) {
			var line = fullLine.Trim();
			const string fnPat = "unsafe fn ";
			if (!line.StartsWith(fnPat, StringComparison.Ordinal))
				throw GetException("Expected `unsafe fn`");
			line = line[fnPat.Length..].Trim();

			int index = line.IndexOf('(');
			if (index < 0)
				throw GetException("Expected `(`");
			var name = line[..index].Trim();
			line = line[index..];
			index = name.IndexOf('<', StringComparison.Ordinal);
			if (index >= 0)
				name = name[..index].Trim();

			fullLine = ParseMethodArgsAndRetType(fullLine, line, name, out var kind, out var args, out var returnValueCount);
			if (!TryCreateDocComments(docComments, out var docComments2, out var error))
				throw GetException(error);

			var method = new LuaMethod(kind, name, docComments2, returnValueCount, args.ToArray());

			SkipBlock(fullLine);
			ClearTempState();

			return method;
		}

		enum DocCommentKind {
			Text,
			ArgsReturn,
			TestCode,
		}

		static DocCommentKind GetDocCommentKind(string line) {
			if (line.StartsWith("@"))
				return DocCommentKind.ArgsReturn;
			if (line.StartsWith("```"))
				return DocCommentKind.TestCode;
			return DocCommentKind.Text;
		}

		static bool TryCreateDocComments(List<string> lines, [NotNullWhen(true)] out DocComments? docComments, [NotNullWhen(false)] out string? error) {
			docComments = null;

			var docs = new DocComments();
			var currentTextLines = new List<string>();
			int numArgsReturn = 0;
			for (int i = 0; i < lines.Count; i++) {
				var line = lines[i];
				if (line == string.Empty) {
					AddCurrentText(docs, currentTextLines);
					continue;
				}

				var kind = GetDocCommentKind(line);
				if (kind != DocCommentKind.Text && currentTextLines.Count != 0)
					AddCurrentText(docs, currentTextLines);
				switch (kind) {
				case DocCommentKind.Text:
					currentTextLines.Add(line);
					break;

				case DocCommentKind.ArgsReturn:
					numArgsReturn++;
					if (numArgsReturn != 1) {
						error = "All @param and @return lines must be next to each other";
						return false;
					}
					var args = new List<LuaParamAnnot>();
					var overloads = new List<LuaOverloadAnnot>();
					LuaReturnAnnot? luaReturn = null;
					LuaClassAnnot? luaClass = null;
					if (!TryGetLines(lines, ref i, line => line.StartsWith("@"), out var argLines, out error))
						return false;
					i--;
					foreach (var argLine in argLines) {
						if (argLine.StartsWith("@param ")) {
							if (luaReturn is not null) {
								error = "@return should be after @param";
								return false;
							}
							if (!TryParseParam(argLine, out var luaParam, out error))
								return false;
							args.Add(luaParam);
						}
						else if (argLine.StartsWith("@return ")) {
							if (luaReturn is not null) {
								error = "Multiple @return found";
								return false;
							}
							if (!TryParseReturn(argLine, out luaReturn, out error))
								return false;
						}
						else if (argLine.StartsWith("@class ")) {
							if (luaClass is not null) {
								error = "Multiple @class found";
								return false;
							}
							if (!TryParseClass(argLine, out luaClass, out error))
								return false;
						}
						else if (argLine.StartsWith("@overload ")) {
							if (!TryParseOverload(argLine, out var luaOverload, out error))
								return false;
							overloads.Add(luaOverload);
						}
						else {
							error = "Expected one of: @param, @return, @class";
							return false;
						}
					}
					docs.Sections.Add(new LuaAnnotationDocCommentSection(luaClass, luaReturn, overloads.ToArray(), args.ToArray()));
					break;

				case DocCommentKind.TestCode:
					i++;
					if (!TryGetLines(lines, ref i, line => !line.StartsWith("```"), out var testCodeLines, out error))
						return false;
					if (lines[i] != "```") {
						error = "End of code block must be \"```\"";
						return false;
					}
					docs.Sections.Add(new TestCodeDocCommentSection(testCodeLines.ToArray()));
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

		static bool TryParseParam(string line, [NotNullWhen(true)] out LuaParamAnnot? luaParam, [NotNullWhen(false)] out string? error) {
			luaParam = null;

			const string pattern = "@param ";
			if (!line.StartsWith(pattern)) {
				error = "Expected @return";
				return false;
			}
			line = line[pattern.Length..].Trim();

			line = GetComment(line, out var comment).Trim();
			if (string.IsNullOrEmpty(comment)) {
				error = "Missing @param comment";
				return false;
			}

			var parts = line.Split(' ', StringSplitOptions.None);
			if (parts.Length != 2) {
				error = "Expected @param <name> <type> #Comment";
				return false;
			}
			var name = parts[0];
			if (!TryCreateLuaType(parts[1], out var type, out error))
				return false;
			bool isOptional = false;
			if (name.EndsWith("?")) {
				isOptional = true;
				name = name[..^1];
			}

			luaParam = new LuaParamAnnot(name, type, comment, isOptional);
			error = null;
			return true;
		}

		static bool TryParseClass(string line, [NotNullWhen(true)] out LuaClassAnnot? luaClass, [NotNullWhen(false)] out string? error) {
			luaClass = null;

			const string pattern = "@class ";
			if (!line.StartsWith(pattern)) {
				error = "Expected @class";
				return false;
			}
			line = line[pattern.Length..].Trim();

			line = GetComment(line, out var comment);
			if (!string.IsNullOrEmpty(comment)) {
				error = "@class can't have a comment";
				return false;
			}
			if (!TryCreateLuaType(line, out var type, out error))
				return false;

			luaClass = new LuaClassAnnot(type);
			error = null;
			return true;
		}

		static bool TryParseOverload(string line, [NotNullWhen(true)] out LuaOverloadAnnot? luaOverload, [NotNullWhen(false)] out string? error) {
			luaOverload = null;

			const string pattern = "@overload ";
			if (!line.StartsWith(pattern)) {
				error = "Expected @overload";
				return false;
			}
			line = line[pattern.Length..].Trim();

			line = GetComment(line, out var comment);
			if (!string.IsNullOrEmpty(comment)) {
				error = "@overload can't have a comment";
				return false;
			}
			var function = line;

			luaOverload = new LuaOverloadAnnot(function);
			error = null;
			return true;
		}

		static bool TryParseReturn(string line, [NotNullWhen(true)] out LuaReturnAnnot? luaReturn, [NotNullWhen(false)] out string? error) {
			luaReturn = null;

			const string pattern = "@return ";
			if (!line.StartsWith(pattern)) {
				error = "Expected @return";
				return false;
			}
			line = line[pattern.Length..].Trim();

			var typesStr = GetComment(line, out var comment);
			var types = new List<LuaType>();
			foreach (var s in typesStr.Split(',')) {
				if (!TryCreateLuaType(s.Trim(), out var type, out error))
					return false;
				types.Add(type);
			}

			luaReturn = new LuaReturnAnnot(comment, types.ToArray());
			error = null;
			return true;
		}

		static string GetComment(string line, out string? comment) {
			var index = line.IndexOf('#', StringComparison.Ordinal);
			if (index >= 0) {
				comment = line[(index + 1)..].Trim();
				line = line[..index];
			}
			else
				comment = null;
			return line;
		}

		static bool TryCreateLuaType(string s, [NotNullWhen(true)] out LuaType? luaType, [NotNullWhen(false)] out string? error) {
			luaType = null;
			var types = new List<string>();
			foreach (var tmp in s.Split('|')) {
				var type = tmp.Trim();
				if (!IsValidType(type)) {
					error = $"Invalid type: `{type}`";
					return false;
				}
				types.Add(type);
			}
			luaType = new LuaType(types.ToArray());
			error = null;
			return true;
		}

		static bool IsValidType(string type) {
			// '?' should be part of the param name
			if (type.IndexOfAny(new[] { ' ', '?' }) >= 0)
				return false;
			while (type.EndsWith("[]"))
				type = type[..^2].Trim();
			// Check if it's one of our custom types, eg. "Decoder"
			if (type.Length > 0 && char.IsUpper(type[0]))
				return true;
			return type is "nil" or "any" or "boolean" or "string" or "number" or "integer" or
							"function" or "table" or "thread" or "userdata" or "lightuserdata";
		}

		static bool TryGetLines(List<string> lines, ref int index, Func<string, bool> isValidLine, [NotNullWhen(true)] out List<string>? result, [NotNullWhen(false)] out string? error) {
			result = null;

			var resultLines = new List<string>();
			string? expectedIndent = null;
			while (index < lines.Count) {
				var line = lines[index];
				if (!isValidLine(line))
					break;
				if (expectedIndent is null)
					expectedIndent = GetIndent(line);
				var indent = GetIndent(line);
				if (!indent.StartsWith(expectedIndent)) {
					error = "Invalid indent";
					return false;
				}
				if (!line.StartsWith(expectedIndent)) {
					error = $"Line doesn't start with expected indent: `{line}";
					return false;
				}
				line = line[expectedIndent.Length..];
				resultLines.Add(line);
				index++;
			}
			result = resultLines;
			error = null;
			return true;
		}
	}
}
