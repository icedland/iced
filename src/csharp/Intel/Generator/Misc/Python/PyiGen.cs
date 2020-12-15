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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using Generator.Enums;
using Generator.IO;

namespace Generator.Misc.Python {
	[Generator(TargetLanguage.Python, double.MaxValue)]
	sealed class PyiGen {
		const string selfArgName = "$self";
		readonly GenTypes genTypes;
		readonly ExportedPythonTypes exportedPythonTypes;

		public PyiGen(GeneratorContext generatorContext) {
			genTypes = generatorContext.Types;
			exportedPythonTypes = genTypes.GetObject<ExportedPythonTypes>(TypeIds.ExportedPythonTypes);
		}

		enum AttributeKind {
			Ignored,
			PyClass,
			PyMethods,
			PyProto,
			Derive,
			New,
			Getter,
			Setter,
			StaticMethod,
			ClassMethod,
			TextSignature,
			Args,
		}

		sealed class RustAttributes {
			public readonly List<RustAttribute> Attributes = new List<RustAttribute>();

			public bool Any(params AttributeKind[] attributes) {
				foreach (var attr in Attributes) {
					foreach (var kind in attributes) {
						if (attr.Kind == kind)
							return true;
					}
				}
				return false;
			}
		}

		[DebuggerDisplay("{Text,nq}")]
		sealed class RustAttribute {
			public readonly AttributeKind Kind;
			public readonly string Text;

			public RustAttribute(AttributeKind kind, string text) {
				Kind = kind;
				Text = text;
			}
		}

		enum DocCommentKind {
			Text,
			Args,
			Raises,
			Returns,
			TestCode,
			TestOutput,
		}

		abstract class DocCommentSection {
		}

		sealed class TextDocCommentSection : DocCommentSection {
			public readonly string[] Lines;

			public TextDocCommentSection(string[] lines) {
				int i;
				for (i = lines.Length - 1; i >= 0; i--) {
					if (!string.IsNullOrEmpty(lines[i]))
						break;
				}
				if (i + 1 != lines.Length)
					lines = lines.Take(i).ToArray();
				Lines = lines;
			}
		}

		[DebuggerDisplay("{SphinxType,nq}: {Documentation,nq}")]
		readonly struct TypeAndDocs {
			public readonly string SphinxType;
			public readonly string Documentation;

			public TypeAndDocs(string sphinxType, string documentation) {
				SphinxType = sphinxType;
				Documentation = documentation;
			}
		}

		[DebuggerDisplay("{Name,nq}: {SphinxType}")]
		readonly struct DocCommentArg {
			public readonly string Name;
			public readonly string SphinxType;
			public readonly string Documentation;

			public DocCommentArg(string name, string sphinxType, string documentation) {
				Name = name;
				SphinxType = sphinxType;
				Documentation = documentation;
			}
		}

		sealed class ArgsDocCommentSection : DocCommentSection {
			public readonly DocCommentArg[] Args;
			public ArgsDocCommentSection(DocCommentArg[] args) => Args = args;
		}

		sealed class RaisesDocCommentSection : DocCommentSection {
			public readonly TypeAndDocs[] Raises;
			public RaisesDocCommentSection(TypeAndDocs[] raises) => Raises = raises;
		}

		sealed class ReturnsDocCommentSection : DocCommentSection {
			public readonly TypeAndDocs Returns;
			public ReturnsDocCommentSection(TypeAndDocs returns) => Returns = returns;
		}

		sealed class TestCodeDocCommentSection : DocCommentSection {
			public readonly string[] Lines;
			public TestCodeDocCommentSection(string[] lines) => Lines = lines;
		}

		sealed class TestOutputDocCommentSection : DocCommentSection {
			public readonly string[] Lines;
			public TestOutputDocCommentSection(string[] lines) => Lines = lines;
		}

		sealed class DocComments {
			public readonly List<DocCommentSection> Sections = new List<DocCommentSection>();

			public void AddText(TextDocCommentSection text) {
				if (text.Lines.Length == 0)
					return;
				if (text.Lines.Length == 1 && text.Lines[0] == string.Empty)
					return;
				Sections.Add(text);
			}
		}

		[DebuggerDisplay("{Name,nq}: {RustType,nq}")]
		readonly struct PyMethodArg {
			public readonly string Name;
			public readonly string RustType;
			public readonly bool IsSelf;

			public PyMethodArg(string name, string rustType, bool isSelf) {
				Name = name;
				RustType = rustType;
				IsSelf = isSelf;
			}
		}

		[DebuggerDisplay("{Name} Args={Arguments.Count}")]
		sealed class PyMethod {
			public readonly string Name;
			public readonly DocComments DocComments;
			public readonly RustAttributes Attributes;
			public readonly List<PyMethodArg> Arguments;
			public readonly string RustReturnType;
			public bool HasReturnType =>
				RustReturnType != string.Empty &&
				RustReturnType != "PyResult<()>";

			public PyMethod(string name, DocComments docComments, RustAttributes attributes, List<PyMethodArg> arguments, string rustReturnType) {
				Name = name;
				DocComments = docComments;
				Attributes = attributes;
				Arguments = arguments;
				RustReturnType = rustReturnType;
			}
		}

		[DebuggerDisplay("{Name,nq} Methods={Methods.Count}")]
		sealed class PyClass {
			public readonly string Name;
			public readonly DocComments DocComments;
			public readonly RustAttributes Attributes;
			public readonly List<PyMethod> Methods;

			public PyClass(string name, DocComments docComments, RustAttributes attributes) {
				Name = name;
				DocComments = docComments;
				Attributes = attributes;
				Methods = new List<PyMethod>();
			}
		}

		public void Generate() {
			var classes = new List<PyClass>();
			foreach (var filename in Directory.GetFiles(genTypes.Dirs.GetPythonRustDir(), "*.rs"))
				classes.AddRange(ParseFile(filename));
			if (classes.Count == 0)
				throw new InvalidOperationException();

			WritePyi(classes);
		}

		static IEnumerable<(string name, string value)> GetArgsNameValues(string argsAttr) {
			if (!TryGetArgsPayload(argsAttr, out var args))
				throw new InvalidOperationException($"Invalid #[args] attr: {argsAttr}");
			foreach (var part in args.Split(',', StringSplitOptions.RemoveEmptyEntries)) {
				int index = part.IndexOf('=', StringComparison.Ordinal);
				if (index < 0)
					throw new InvalidOperationException();
				var name = part[..index].Trim();
				var value = part[(index + 1)..].Trim();
				yield return (name, value);
			}
		}

		static bool TryRemovePrefixSuffix(string s, string prefix, string suffix, [NotNullWhen(true)] out string? extracted) {
			extracted = null;

			if (!s.StartsWith(prefix, StringComparison.Ordinal))
				return false;
			if (!s.EndsWith(suffix, StringComparison.Ordinal))
				return false;

			extracted = s[prefix.Length..^suffix.Length];
			return true;
		}

		static string[] SplitSphinxTypes(string sphinxType) =>
			sphinxType.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(a => a.Trim()).ToArray();

		static bool TryGetSphinxTypeToTypeName(string sphinxType, [NotNullWhen(true)] out string? typeName) =>
			TryRemovePrefixSuffix(sphinxType, ":class:`", "`", out typeName);

		static bool TryGetArgsPayload(string argsAttr, [NotNullWhen(true)] out string? args) =>
			TryRemovePrefixSuffix(argsAttr, "#[args(", ")]", out args);

		// Gets all required enum fields that must be part of the pyi file because they're
		// default values in some methods.
		Dictionary<EnumType, HashSet<EnumValue>> GetRequiredEnumFields(List<PyClass> classes) {
			var reqEnumFields = new Dictionary<EnumType, HashSet<EnumValue>>();
			var argToEnumType = new Dictionary<string, EnumType>(StringComparer.Ordinal);
			foreach (var pyClass in classes) {
				foreach (var method in pyClass.Methods) {
					DocComments docComments;
					if (method.Attributes.Any(AttributeKind.New))
						docComments = pyClass.DocComments;
					else
						docComments = method.DocComments;
					var docs = docComments.Sections.OfType<ArgsDocCommentSection>().FirstOrDefault();
					if (docs is null)
						continue;
					int hasThis = method.Arguments.Count != 0 && method.Arguments[0].IsSelf ? 1 : 0;
					if (docs.Args.Length != (method.Arguments.Count - hasThis))
						throw new InvalidOperationException();
					argToEnumType.Clear();
					for (int i = 0; i < docs.Args.Length; i++) {
						var docArg = docs.Args[i];
						if (docArg.Name != method.Arguments[hasThis + i].Name)
							throw new InvalidOperationException();
						if (!TryGetSphinxTypeToTypeName(docArg.SphinxType, out var typeName))
							continue;
						if (!exportedPythonTypes.TryFindByName(typeName, out var enumType))
							continue;
						argToEnumType.Add(docArg.Name, enumType);
					}

					var argsAttr = method.Attributes.Attributes.FirstOrDefault(a => a.Kind == AttributeKind.Args);
					if (argsAttr is null)
						continue;
					foreach (var (name, value) in GetArgsNameValues(argsAttr.Text)) {
						if (!argToEnumType.TryGetValue(name, out var enumType))
							continue;
						if (!uint.TryParse(value, out var rawValue))
							throw new InvalidOperationException($"Couldn't parse {value} as an integer");
						var enumValue = enumType.Values.FirstOrDefault(a => a.Value == rawValue);
						if (enumValue is null)
							throw new InvalidOperationException($"Couldn't find an enum value in {enumType.RawName} with a value equal to {value}");
						if (!reqEnumFields.TryGetValue(enumType, out var hash))
							reqEnumFields.Add(enumType, hash = new HashSet<EnumValue>());
						hash.Add(enumValue);
					}
				}
			}
			return reqEnumFields;
		}

		void WritePyi(List<PyClass> classes) {
			var reqEnumFields = GetRequiredEnumFields(classes);
			var classOrder = new[] {
				"OpCodeInfo",
				"ConstantOffsets",
				"FpuStackIncrementInfo",
				"MemoryOperand",
				"Instruction",
				"Decoder",
				"Encoder",
				"Formatter",
				"FastFormatter",
				"BlockEncoder",
				"UsedRegister",
				"UsedMemory",
				"InstructionInfo",
				"InstructionInfoFactory",
				"MemorySizeInfo",
				"MemorySizeExt",
				"RegisterInfo",
				"RegisterExt",
			};
			var toClass = classes.ToDictionary(a => a.Name, a => a);

			foreach (var pyClass in classes) {
				if (!classOrder.Contains(pyClass.Name))
					throw new InvalidOperationException($"Missing {pyClass.Name} in {nameof(classOrder)}");
			}
			if (classOrder.Length != classes.Count)
				throw new InvalidOperationException($"{nameof(classOrder)}.Length {classOrder.Length} != {nameof(classes)}.Count {classes.Count}");

			var filename = genTypes.Dirs.GetPythonPyFilename("_iced_x86_py.pyi");
			using (var writer = new FileWriter(TargetLanguage.Python, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				writer.WriteLine("from collections.abc import Iterator");
				writer.WriteLine("from enum import IntEnum, IntFlag");
				writer.WriteLine("from typing import Any, List, Optional, Union");
				writer.WriteLine();

				var idConverter = PythonIdentifierConverter.Create();
				var allEnumTypes = exportedPythonTypes.IntEnums.Concat(exportedPythonTypes.IntFlags).Select(a => (enumType: a, pythonName: a.Name(idConverter)));
				var toEnumType = allEnumTypes.ToDictionary(a => a.pythonName, a => a.enumType, StringComparer.Ordinal);
				foreach (var (enumType, pythonName) in allEnumTypes.OrderBy(a => a.pythonName, StringComparer.Ordinal)) {
					var baseClass = enumType.IsFlags ? "IntFlag" : "IntEnum";
					if (reqEnumFields.TryGetValue(enumType, out var fields)) {
						writer.WriteLine($"class {pythonName}({baseClass}):");
						using (writer.Indent()) {
							bool uppercaseRawName = PythonUtils.UppercaseEnum(enumType.TypeId.Id1);
							foreach (var value in enumType.Values) {
								if (fields.Contains(value)) {
									fields.Remove(value);
									var (valueName, numStr) = PythonUtils.GetEnumNameValue(idConverter, value, uppercaseRawName);
									writer.WriteLine($"{valueName} = {numStr}");
								}
								if (fields.Count == 0)
									break;
							}
							if (fields.Count != 0)
								throw new InvalidOperationException();
							writer.WriteLine("...");
						}
					}
					else
						writer.WriteLine($"class {pythonName}({baseClass}): ...");
				}

				foreach (var classStr in classOrder) {
					var pyClass = toClass[classStr];
					toClass.Remove(classStr);
					writer.WriteLine();
					writer.WriteLine($"class {idConverter.Type(pyClass.Name)}:");
					using (writer.Indent()) {
						int defCount = 0;
						foreach (var member in GetMembers(pyClass)) {
							switch (member) {
							case PyMethod method:
								var docComments = method.Attributes.Any(AttributeKind.New) == true ?
									pyClass.DocComments : method.DocComments;
								Write(writer, idConverter, pyClass, method, docComments, toEnumType);
								defCount++;
								break;
							case PyProperty property:
								Write(writer, idConverter, pyClass, property.Getter, property.Getter.DocComments, toEnumType);
								defCount++;
								if (property.Setter is not null) {
									Write(writer, idConverter, pyClass, property.Setter, property.Getter.DocComments, toEnumType);
									defCount++;
								}
								break;
							default:
								throw new InvalidOperationException();
							}
						}
						if (defCount == 0)
							throw new InvalidOperationException($"class {pyClass.Name}: No class members");
					}
				}
				if (toClass.Count != 0)
					throw new InvalidOperationException();
			}
		}

		static void Write(FileWriter writer, IdentifierConverter idConverter, PyClass pyClass, PyMethod method, DocComments docComments, Dictionary<string, EnumType> toEnumType) {
			if (method.Attributes.Any(AttributeKind.ClassMethod) == true)
				writer.WriteLine("@classmethod");
			if (method.Attributes.Any(AttributeKind.StaticMethod) == true)
				writer.WriteLine("@staticmethod");
			bool isGetter = method.Attributes.Any(AttributeKind.Getter) == true;
			bool isSetter = method.Attributes.Any(AttributeKind.Setter) == true;
			if (isGetter)
				writer.WriteLine("@property");
			if (isSetter)
				writer.WriteLine($"@{method.Name}.setter");

			string sphinxReturnType = string.Empty;
			if (isGetter || isSetter) {
				if (docComments.Sections.FirstOrDefault() is not TextDocCommentSection textDocs || textDocs.Lines.Length == 0)
					throw new InvalidOperationException();
				if (!TryParseTypeAndDocs(textDocs.Lines[0], out _, out var typeInfo))
					throw new InvalidOperationException();
				sphinxReturnType = typeInfo.SphinxType;
			}
			else {
				var returns = docComments.Sections.OfType<ReturnsDocCommentSection>().FirstOrDefault();
				if (returns is not null)
					sphinxReturnType = returns.Returns.SphinxType;
			}

			bool isCtor = method.Attributes.Any(AttributeKind.New) == true;
			writer.Write("def ");
			writer.Write(isCtor ? "__init__" : method.Name);
			writer.Write("(");
			int argCount = 0;
			if (isCtor) {
				writer.Write("self");
				argCount++;
			}
			var argsDocs = docComments.Sections.OfType<ArgsDocCommentSection>().FirstOrDefault();
			int hasThis = method.Arguments.Count != 0 && method.Arguments[0].IsSelf ? 1 : 0;

			Dictionary<string, string> toDefaultValue;
			var argsAttr = method.Attributes.Attributes.FirstOrDefault(a => a.Kind == AttributeKind.Args);
			if (argsAttr is null)
				toDefaultValue = new Dictionary<string, string>(StringComparer.Ordinal);
			else
				toDefaultValue = GetArgsNameValues(argsAttr.Text).ToDictionary(a => a.name, a => a.value);

			for (int i = 0; i < method.Arguments.Count; i++) {
				if (argsDocs is not null && argsDocs.Args.Length != method.Arguments.Count - hasThis)
					throw new InvalidOperationException();
				var methodArg = method.Arguments[i];
				if (argCount > 0)
					writer.Write(", ");
				argCount++;
				if (methodArg.IsSelf)
					writer.Write("self");
				else
					writer.Write(methodArg.Name);
				if (!methodArg.IsSelf) {
					string docsSphinxType;
					if (argsDocs is not null) {
						var docsArg = argsDocs.Args[i - hasThis];
						if (docsArg.Name != methodArg.Name)
							throw new InvalidOperationException();
						docsSphinxType = docsArg.SphinxType;
					}
					else
						docsSphinxType = string.Empty;
					if (i == 1 && isSetter)
						docsSphinxType = sphinxReturnType;

					writer.Write(": ");
					var type = GetType(pyClass, method.Name, methodArg.RustType, docsSphinxType);
					writer.Write(type);

					if (toDefaultValue.TryGetValue(methodArg.Name, out var defaultValueStr)) {
						writer.Write(" = ");
						if (!TryGetValueStr(idConverter, type, defaultValueStr, toEnumType, out var valueStr))
							throw new InvalidOperationException($"method {pyClass.Name}.{method.Name}(): Couldn't convert default value `{defaultValueStr}` to a Python value");
						writer.Write(valueStr);
					}
				}
			}
			writer.Write(") -> ");
			if (method.HasReturnType && !isCtor)
				writer.Write(GetType(pyClass, method.Name, method.RustReturnType, sphinxReturnType));
			else
				writer.Write("None");
			writer.WriteLine(": ...");
		}

		static bool TryGetValueStr(IdentifierConverter idConverter, string typeStr, string defaultValueStr, Dictionary<string, EnumType> toEnumType, [NotNullWhen(true)] out string? valueStr) {
			valueStr = null;
			if (toEnumType.TryGetValue(typeStr, out var enumType)) {
				if (!uint.TryParse(defaultValueStr, out var rawValue))
					return false;
				var enumValue = enumType.Values.FirstOrDefault(a => a.Value == rawValue);
				if (enumValue is null)
					return false;
				valueStr = enumValue.DeclaringType.Name(idConverter) + "." + enumValue.Name(idConverter);
				return true;
			}

			if (typeStr == "int") {
				if (ulong.TryParse(defaultValueStr, out _) || long.TryParse(defaultValueStr, out _)) {
					valueStr = defaultValueStr;
					return true;
				}
			}

			switch (defaultValueStr) {
			case "true":
				valueStr = "True";
				return true;
			case "false":
				valueStr = "False";
				return true;
			}

			valueStr = null;
			return false;
		}

		static string GetType(PyClass pyClass, string methodName, string rustType, string sphinxType) {
			var typeStr = GetTypeCore(pyClass, methodName, rustType, sphinxType);
			if (methodName == "__iter__") {
				string returnType = pyClass.Name switch {
					"Decoder" => "Instruction",
					_ => throw new InvalidOperationException($"Unexpected iterator class {pyClass.Name}"),
				};
				return $"Iterator[{returnType}]";
			}
			return typeStr;
		}

		static string GetTypeCore(PyClass pyClass, string methodName, string rustType, string sphinxType) {
			if (sphinxType != string.Empty) {
				var sphinxTypes = SplitSphinxTypes(sphinxType).ToList();
				var convertedTypes = new List<string>();
				foreach (var stype in sphinxTypes) {
					if (!TryGetSphinxTypeToTypeName(stype, out var typeName))
						typeName = stype;
					convertedTypes.Add(typeName);
				}
				int index = convertedTypes.Count == 1 ? -1 : convertedTypes.IndexOf("None");
				if (index >= 0)
					convertedTypes.RemoveAt(index);
				string typeStr;
				if (convertedTypes.Count > 1)
					typeStr = "Union[" + string.Join(", ", convertedTypes.ToArray()) + "]";
				else
					typeStr = convertedTypes[0];
				if (index >= 0)
					return "Optional[" + typeStr + "]";
				return typeStr;
			}

			if (TryRemovePrefixSuffix(rustType, "PyResult<", ">", out var extractedType))
				rustType = extractedType;
			switch (rustType) {
			case "i8" or "i16" or "i32" or "i64" or "isize" or
				"u8" or "u16" or "u32" or "u64" or "usize":
				return "int";
			case "bool":
				return "bool";
			case "&str" or "String":
				return "str";
			case "PyRef<Self>" or "PyRefMut<Self>" or "Self":
				return pyClass.Name;
			case "&PyAny":
				return "Any";
			default:
				if (TryRemovePrefixSuffix(rustType, "IterNextOutput<", ", ()>", out extractedType))
					return extractedType;
				break;
			}

			throw new InvalidOperationException($"Method {pyClass.Name}.{methodName}(): Couldn't convert Rust/sphinx type to Python type: Rust=`{rustType}`, sphinx=`{sphinxType}`");
		}

		sealed class PyProperty {
			public readonly PyMethod Getter;
			public readonly PyMethod? Setter;

			public PyProperty(PyMethod getter, PyMethod? setter) {
				Getter = getter;
				Setter = setter;
			}
		}

		IEnumerable<object> GetMembers(PyClass pyClass) {
			var setters = pyClass.Methods.Where(a => a.Attributes.Any(AttributeKind.Setter) == true).ToDictionary(a => a.Name, a => a);
			var ignored = pyClass.Methods.Where(a => a.Attributes.Any(AttributeKind.Setter) == true).ToHashSet();
			foreach (var method in pyClass.Methods) {
				if (ignored.Contains(method))
					continue;
				if (method.Attributes.Any(AttributeKind.Getter) == true) {
					setters.TryGetValue(method.Name, out var setterMethod);
					setters.Remove(method.Name);
					yield return new PyProperty(method, setterMethod);
				}
				else
					yield return method;
			}
			if (setters.Count != 0)
				throw new InvalidOperationException($"{pyClass.Name}: Setter without a getter: {setters.First().Value.Name}");
		}

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

		sealed class ParseState {
			public readonly string Filename;
			public readonly Lines Lines;
			public readonly List<string> DocComments;
			public RustAttributes? Attributes;
			readonly Dictionary<string, PyClass> pyClasses;

			public ParseState(string filename) {
				Filename = filename;
				Lines = new Lines(File.ReadAllLines(filename));
				DocComments = new List<string>();
				pyClasses = new Dictionary<string, PyClass>(StringComparer.Ordinal);
			}

			public PyClass[] GetClasses() => pyClasses.Values.ToArray();

			public bool TryGetPyClass(string name, [NotNullWhen(true)] out PyClass? pyClass) =>
				pyClasses.TryGetValue(name, out pyClass);

			public void AddPyClass(PyClass pyClass) {
				if (pyClasses.ContainsKey(pyClass.Name))
					throw GetException($"Duplicate struct {pyClass.Name}");
				pyClasses.Add(pyClass.Name, pyClass);
			}

			public void ClearTempState() {
				DocComments.Clear();
				Attributes = null;
			}

			public bool HasTempState => DocComments.Count != 0 || Attributes is not null;

			public Exception GetException(string message) =>
				new InvalidOperationException($"{message}, line: {Lines.LineNo}, file: {Filename}");
		}

		static PyClass[] ParseFile(string filename) {
			var state = new ParseState(filename);
			while (true) {
				var token = state.Lines.Next();
				if (token.kind == LineKind.Eof)
					break;

				switch (token.kind) {
				case LineKind.Other:
					state.ClearTempState();
					break;

				case LineKind.DocComment:
					AddDocCommentLine(state, token.line);
					break;

				case LineKind.Attribute:
					ReadAttribute(state, token.line);
					break;

				case LineKind.Struct:
					ReadStruct(state, token.line);
					SkipBlock(state, token.line);
					break;

				case LineKind.Impl:
					if (state.DocComments.Count != 0)
						throw state.GetException("Unexpected doc comments");
					var implName = GetName(state, token.line, "impl");
					if (!state.TryGetPyClass(implName, out var pyClass) ||
						state.Attributes is null ||
						state.Attributes.Attributes.Count == 0) {
						SkipBlock(state, token.line);
					}
					else {
						if (!state.Attributes.Any(AttributeKind.PyMethods, AttributeKind.PyProto))
							SkipBlock(state, token.line);
						else
							ReadStructImpl(state, token.line, pyClass);
					}
					break;

				case LineKind.Fn:
					SkipBlock(state, token.line);
					break;

				case LineKind.Eof:
				default:
					throw state.GetException($"Unexpected token {token.kind}");
				}
			}

			var classes = state.GetClasses();
			foreach (var pyClass in classes) {
				var pyClassAttr = pyClass.Attributes.Attributes.FirstOrDefault(a => a.Kind == AttributeKind.PyClass);
				const string expectedPyClassAttr = "#[pyclass(module = \"_iced_x86_py\")]";
				if (pyClassAttr?.Text != expectedPyClassAttr)
					throw state.GetException($"Class {pyClass.Name}: Expected this #[pyclass] attribute: {expectedPyClassAttr}");

				var ctor = pyClass.Methods.FirstOrDefault(a => a.Attributes.Any(AttributeKind.New));
				int argsSectCount = pyClass.DocComments.Sections.OfType<ArgsDocCommentSection>().Count();
				int expectedArgsSectCount = (ctor?.Arguments.Count ?? 0) == 0 ? 0 : 1;
				if (argsSectCount != expectedArgsSectCount)
					throw state.GetException($"Class {pyClass.Name}: Expected exactly {expectedArgsSectCount} `Args:` sections but found {argsSectCount}");
				if (ctor is not null) {
					var expectedTextSig = GetExpectedTextSignature(ctor);
					var textSigAttr = pyClass.Attributes.Attributes.First(a => a.Kind == AttributeKind.TextSignature);
					if (textSigAttr.Text != expectedTextSig)
						throw state.GetException($"Class {pyClass.Name}: #[text_signature] didn't match the expected value: {expectedTextSig}");

					var argsAttr = pyClass.Attributes.Attributes.FirstOrDefault(a => a.Kind == AttributeKind.Args);
					if (argsAttr is not null)
						throw state.GetException($"Class {pyClass.Name}: The ctor should have the #[args] attribute");

					if (!CheckArgsSectionAndMethodArgs(pyClass.DocComments, ctor.Arguments, out var error))
						throw state.GetException($"Class {pyClass.Name}: {error}");
				}
			}

			return classes;
		}

		static void ReadStruct(ParseState state, string line) {
			if (state.Attributes?.Any(AttributeKind.PyClass) == true) {
				line = RemovePub(state, line);
				var name = GetName(state, line, "struct");
				if (!TryCreateDocComments(state.DocComments, out var docComments, out var error))
					throw state.GetException(error);
				var pyClass = new PyClass(name, docComments, state.Attributes ?? new RustAttributes());
				state.AddPyClass(pyClass);
			}
			state.ClearTempState();
		}

		static void ReadStructImpl(ParseState state, string implLine, PyClass pyClass) {
			state.ClearTempState();
			var endOfBlockStr = GetIndent(implLine) + "}";
			while (true) {
				var token = state.Lines.Next();
				if (token.kind == LineKind.Eof)
					break;
				if (token.kind == LineKind.Other && token.line == endOfBlockStr)
					break;

				switch (token.kind) {
				case LineKind.Other:
					var trimmed = token.line.Trim();
					if (!(trimmed == string.Empty || trimmed.StartsWith("//", StringComparison.Ordinal)))
						throw state.GetException("Unexpected line");
					break;

				case LineKind.DocComment:
					AddDocCommentLine(state, token.line);
					break;

				case LineKind.Attribute:
					ReadAttribute(state, token.line);
					break;

				case LineKind.Fn:
					foreach (var method in ReadMethod(state, pyClass, token.line)) {
						if (!IgnoreMethod(method))
							pyClass.Methods.Add(method);
					}
					break;

				case LineKind.Struct:
				case LineKind.Impl:
				case LineKind.Eof:
				default:
					throw state.GetException($"Unexpected token {token.kind}");
				}
			}

			if (state.HasTempState)
				throw state.GetException("Unexpected docs/attrs");
		}

		static bool IgnoreMethod(PyMethod method) =>
			method.Name switch {
				"__traverse__" or "__clear__" or "__next__" => true,
				_ => false
			};

		static string RemovePub(ParseState state, string line) {
			if (line.StartsWith("pub", StringComparison.Ordinal)) {
				line = line["pub".Length..];
				if (line.StartsWith("(", StringComparison.Ordinal)) {
					int index = line.IndexOf(')');
					if (index < 0)
						throw state.GetException("Expected ')'");
					line = line[(index + 1)..];
				}
			}
			return line.TrimStart();
		}

		static string ParseMethodArgsAndRetType(ParseState state, string fullLine, string line, bool isInstanceMethod, bool isSpecial, out List<PyMethodArg> args, out string rustReturnType) {
			args = new List<PyMethodArg>();

			if (!line.StartsWith('('))
				throw state.GetException("Expected '('");
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
				foreach (var tmp in argsLine.Split(',', StringSplitOptions.RemoveEmptyEntries)) {
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
						if (args.Count != 0)
							throw state.GetException("`self` must be the first arg");
						foundThis = true;
						arg = new PyMethodArg(selfArgName, argInfo, isSelf: true);
						break;
					default:
						index = argInfo.IndexOf(':');
						if (index < 0)
							throw state.GetException("Expected `:`");
						var name = argInfo[..index].Trim();
						var rustType = argInfo[(index + 1)..].Trim();
						if (rustType.StartsWith("Python<", StringComparison.Ordinal)) {
							if (name != "py" && name != "_py")
								throw state.GetException("Expected name to be `py` or `_py`");
							continue;
						}
						if (name.StartsWith('_')) {
							if (!isSpecial)
								throw state.GetException($"Unused arg {name}");
							name = name[1..];
						}
						bool isSelf = false;
						if (rustType == "PyRef<Self>" || rustType == "PyRefMut<Self>") {
							if (args.Count != 0)
								throw state.GetException("`self` must be the first arg");
							foundThis = true;
							name = selfArgName;
							isSelf = true;
						}
						if (name.Contains(' ', StringComparison.Ordinal))
							throw state.GetException("Name has a space");
						arg = new PyMethodArg(name, rustType, isSelf);
						break;
					}
					args.Add(arg);
				}

				if (line.StartsWith(')'))
					break;
				if (line != string.Empty)
					throw state.GetException("Internal error");
				var token = state.Lines.Next();
				if (token.kind == LineKind.Eof)
					throw state.GetException("Unexpected EOF");
				fullLine = token.line;
				line = fullLine.Trim();
			}
			if (foundThis != isInstanceMethod) {
				if (isInstanceMethod)
					throw state.GetException("Expected `self` as first arg");
				throw state.GetException("`self` found when not an instance method");
			}

			index = line.IndexOf("->");
			if (index >= 0) {
				line = line[(index + 2)..].Trim();
				index = line.IndexOf('{');
				if (index < 0)
					throw state.GetException("Expected `{`");
				rustReturnType = line[..index].Trim();
				line = line[index..];
			}
			else
				rustReturnType = string.Empty;
			return fullLine;
		}

		static string GetExpectedTextSignature(PyMethod method) {
			var sb = new StringBuilder();
			sb.Append("#[text_signature = \"(");
			foreach (var arg in method.Arguments) {
				sb.Append(arg.Name);
				sb.Append(", ");
			}
			sb.Append("/)\"]");
			return sb.ToString();
		}

		static bool CheckArgsAttribute(PyMethod method, string? argsAttr) {
			if (argsAttr is null)
				return true;

			if (!TryGetArgsPayload(argsAttr, out var s))
				return false;
			var attrArgs = s.Split(',');
			if (attrArgs.Length > method.Arguments.Count)
				return false;
			for (int i = 0; i < attrArgs.Length; i++) {
				var attrArg = attrArgs[i];
				int index = attrArg.IndexOf('=', StringComparison.Ordinal);
				if (index < 0)
					return false;
				var attrArgName = attrArg[..index].Trim();
				var argName = method.Arguments[method.Arguments.Count - attrArgs.Length + i].Name;
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

		static IEnumerable<PyMethod> ReadMethod(ParseState state, PyClass pyClass, string fullLine) {
			var firstLine = fullLine;
			var line = RemovePub(state, fullLine.Trim());
			const string fnPat = "fn ";
			if (!line.StartsWith(fnPat, StringComparison.Ordinal))
				throw state.GetException("Expected `fn`");
			line = line[fnPat.Length..].Trim();

			int index = line.IndexOf('(');
			if (index < 0)
				throw state.GetException("Expected `(`");
			var name = line[..index].Trim();
			line = line[index..];
			index = name.IndexOf('<', StringComparison.Ordinal);
			if (index >= 0)
				name = name[..index].Trim();

			var attributes = state.Attributes ?? new RustAttributes();
			bool isStaticMethod = attributes.Any(AttributeKind.StaticMethod) == true;
			bool isClassMethod = attributes.Any(AttributeKind.ClassMethod) == true;
			bool isCtor = attributes.Any(AttributeKind.New) == true;
			bool isInstanceMethod = !isStaticMethod && !isClassMethod && !isCtor;
			if (isStaticMethod && isClassMethod)
				throw state.GetException("Method can't be both classmethod and staticmethod");

			bool isSpecial = name.StartsWith("__", StringComparison.Ordinal) &&
				name.EndsWith("__", StringComparison.Ordinal) &&
				name != "__copy__" && name != "__deepcopy__";

			fullLine = ParseMethodArgsAndRetType(state, fullLine, line, isInstanceMethod, isSpecial || name == "__deepcopy__", out var args, out var rustReturnType);
			if (!TryCreateDocComments(state.DocComments, out var docComments, out var error))
				throw state.GetException(error);

			bool isSetter = attributes.Any(AttributeKind.Setter) == true;
			bool isGetter = attributes.Any(AttributeKind.Getter) == true;

			if (isSetter && name.StartsWith("set_"))
				name = name["set_".Length..];
			if (isGetter && name.StartsWith("get_"))
				throw state.GetException($"Getters shouldn't have a `get_` prefix: {name}");

			var method = new PyMethod(name, docComments, attributes, args, rustReturnType);

			var argsAttr = method.Attributes.Attributes.FirstOrDefault(a => a.Kind == AttributeKind.Args);
			if (isSpecial || isGetter || isSetter) {
				if (argsAttr is not null)
					throw state.GetException("Unexpected #[args] attribute found");
			}
			else {
				if (!CheckArgsAttribute(method, argsAttr?.Text))
					throw state.GetException($"Invalid #[args] attribute: {argsAttr?.Text}");
			}

			if (!(isSpecial || isGetter || isSetter || isCtor)) {
				int count = method.Attributes.Attributes.Count(a => a.Kind == AttributeKind.TextSignature);
				if (count != 1)
					throw state.GetException("Expected exactly one #[text_signature] attribute");
				var expectedTextSig = GetExpectedTextSignature(method);
				var textSigAttr = method.Attributes.Attributes.First(a => a.Kind == AttributeKind.TextSignature);
				if (textSigAttr.Text != expectedTextSig)
					throw state.GetException($"#[text_signature] didn't match the expected value: {expectedTextSig}");
			}

			if (isSetter) {
				if (method.DocComments.Sections.Count > 0)
					throw state.GetException($"Setters should have no docs, only getters should have docs: {name}");
				const string setterArgName = "new_value";
				if (method.Arguments.Count != 2)
					throw state.GetException($"Invalid number of setter arguments, expected 2 but found {method.Arguments.Count}");
				if (method.Arguments[1].Name != setterArgName)
					throw state.GetException($"Setter argument must be `{setterArgName}` not {method.Arguments[1].Name}");
			}
			else {
				if (method.DocComments.Sections.OfType<ArgsDocCommentSection>().Count() > 1)
					throw state.GetException("Too many `Args:` sections");
				if (method.DocComments.Sections.OfType<RaisesDocCommentSection>().Count() > 1)
					throw state.GetException("Too many `Raises:` sections");
				if (isGetter) {
					if (method.DocComments.Sections.OfType<ReturnsDocCommentSection>().Any())
						throw state.GetException("Setters should have no `Returns:` sections. The return type should be the first type on the first doc line, eg. `int: Some docs here`");
					if (method.DocComments.Sections.FirstOrDefault() is not TextDocCommentSection sect || sect.Lines.Length == 0)
						throw state.GetException("Expected first doc comments section to be text");
					if (!TryParseTypeAndDocs(sect.Lines[0], out _, out _))
						throw state.GetException("First data on the first line must be the property type");
				}
				else if (isCtor) {
					if (method.DocComments.Sections.Count != 0)
						throw state.GetException("Ctors should have no docs, they should be part of the class' docs");
				}
				else {
					if (!isSpecial && method.DocComments.Sections.Count == 0)
						throw state.GetException($"Missing documentation: {name}");
					if (!isSpecial && method.HasReturnType) {
						if (method.DocComments.Sections.OfType<ReturnsDocCommentSection>().Count() != 1)
							throw state.GetException("Expected exactly one `Returns:` section");
					}
					else {
						if (method.DocComments.Sections.OfType<ReturnsDocCommentSection>().Any())
							throw state.GetException("Expected no `Returns:` sections");
					}
				}
			}

			if (!isSpecial && !isGetter && !isSetter && !isCtor) {
				if (!CheckArgsSectionAndMethodArgs(method.DocComments, method.Arguments, out error))
					throw state.GetException(error);
			}

			var (startLine, endLine) = SkipBlock(state, fullLine);
			state.ClearTempState();

			if (method.Name == "__richcmp__") {
				var seenCompareOps = new HashSet<CompareOp>();
				for (int lineNo = startLine; lineNo < endLine; lineNo++) {
					line = state.Lines.GetLine(lineNo);
					foreach (var compareOp in GetCompareOps(line)) {
						if (!seenCompareOps.Add(compareOp))
							throw state.GetException("Duplicate CompareOp found in the method");
						var opName = compareOp switch {
							CompareOp.Lt => "__lt__",
							CompareOp.Le => "__le__",
							CompareOp.Eq => "__eq__",
							CompareOp.Ne => "__ne__",
							CompareOp.Gt => "__ge__",
							CompareOp.Ge => "__gt__",
							_ => throw new InvalidOperationException(),
						};
						var newArgs = new List<PyMethodArg> {
							new PyMethodArg(selfArgName, "&self", isSelf: true),
							new PyMethodArg("other", "&PyAny", isSelf: false),
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

		static void AddDocCommentLine(ParseState state, string line) {
			line = line.TrimStart();
			const string DocCommentPrefix = "///";
			if (!line.StartsWith(DocCommentPrefix, StringComparison.Ordinal))
				throw state.GetException("Expected a doc comment");
			var docComment = line[DocCommentPrefix.Length..];
			if (docComment.StartsWith(" ", StringComparison.Ordinal))
				docComment = docComment[1..];
			state.DocComments.Add(docComment);
		}

		static void ReadAttribute(ParseState state, string line) {
			state.Attributes ??= new RustAttributes();
			state.Attributes.Attributes.Add(ParseAttribute(state, line));
		}

		static RustAttribute ParseAttribute(ParseState state, string line) {
			var attrLine = line.Trim();
			var fullAttrLine = attrLine;
			const string attrPrefix = "#[";
			if (!attrLine.StartsWith(attrPrefix, StringComparison.Ordinal))
				throw state.GetException("Expected an attribute");
			attrLine = attrLine[attrPrefix.Length..];
			int index = attrLine.IndexOfAny(new[] { '(', ' ', '=', ']' });
			if (index < 0)
				throw state.GetException("Invalid attribute");
			var attrName = attrLine.Substring(0, index);
			var attrKind = attrName switch {
				"pyclass" => AttributeKind.PyClass,
				"pymethods" => AttributeKind.PyMethods,
				"pyproto" => AttributeKind.PyProto,
				"new" => AttributeKind.New,
				"getter" => AttributeKind.Getter,
				"setter" => AttributeKind.Setter,
				"staticmethod" => AttributeKind.StaticMethod,
				"classmethod" => AttributeKind.ClassMethod,
				"text_signature" => AttributeKind.TextSignature,
				"args" => AttributeKind.Args,
				"derive" or "allow" or "rustfmt::skip" or "macro_use" or
				"pymodule" or "inline" => AttributeKind.Ignored,
				// Don't ignore unknown attrs by default. We must know what the attribute does
				// so we don't ignore an important one that gets added in the future.
				_ => throw state.GetException($"Unknown attribute: `{attrName}`"),
			};
			return new RustAttribute(attrKind, fullAttrLine);
		}

		static string GetName(ParseState state, string line, string keyword) {
			int index = line.IndexOf(keyword + " ", StringComparison.Ordinal);
			if (index < 0)
				throw state.GetException($"Expected `{keyword}`");
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
				throw state.GetException($"Found whitespace in `id` after keyword `{keyword}`");
			return name;
		}

		static string GetIndent(string line) {
			var trimmed = line.TrimStart();
			return line.Substring(0, line.IndexOf(trimmed, StringComparison.Ordinal));
		}

		static (int startLine, int endLine) SkipBlock(ParseState state, string line) {
			if (line.EndsWith(';') || line.EndsWith('}')) {
				state.ClearTempState();
				return (0, 0);
			}

			var expectedIndent = GetIndent(line);
			var expected = expectedIndent + "}";
			bool seenOpeningBlock = line.EndsWith("{", StringComparison.Ordinal);
			var startLine = state.Lines.LineNo;
			while (true) {
				var token = state.Lines.Next();
				if (token.kind == LineKind.Eof)
					throw state.GetException("Unexpected EOF");
				if (token.line != string.Empty) {
					var indent = GetIndent(token.line);
					if (indent.Length < expectedIndent.Length)
						throw state.GetException("New indent < current indent");
					if (indent.Length == expectedIndent.Length) {
						if (!seenOpeningBlock &&
							token.line.EndsWith("{", StringComparison.Ordinal) &&
							expectedIndent == GetIndent(token.line)) {
							seenOpeningBlock = true;
						}
						else {
							if (expected != token.line)
								throw state.GetException("Expected end of block");
							if (!seenOpeningBlock)
								throw state.GetException("Missing `{` at the start of the block");
							break;
						}
					}
				}
			}
			state.ClearTempState();
			return (startLine, state.Lines.LineNo - 1);
		}

		static DocCommentKind GetDocCommentKind(string line) =>
			line switch {
				"Args:" => DocCommentKind.Args,
				"Raises:" => DocCommentKind.Raises,
				"Returns:" => DocCommentKind.Returns,
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
						if (!TryParseTypeAndDocs(argLine, out error, out var raisesInfo))
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
						if (!TryParseTypeAndDocs(argLine, out error, out var returnsTmp))
							return false;
						returns = returnsTmp;
					}
					if (returns is null) {
						error = "Missing `Returns` info lines";
						return false;
					}
					docs.Sections.Add(new ReturnsDocCommentSection(returns.Value));
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

		static bool TryParseTypeAndDocs(string argLine, [NotNullWhen(false)] out string? error, out TypeAndDocs result) {
			result = default;

			const string pattern = ": ";
			int index = argLine.IndexOf(pattern, StringComparison.Ordinal);
			if (index < 0) {
				error = "Expected `: `";
				return false;
			}

			var sphinxType = argLine[..index].Trim();
			string documentation = argLine[(index + 1)..].Trim();
			result = new TypeAndDocs(sphinxType, documentation);
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
