// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Generator.Enums;
using Generator.Enums.Python;
using Generator.IO;

namespace Generator.Misc.Python {
	[Generator(TargetLanguage.Python, double.MaxValue)]
	sealed class PyiGen {
		readonly GenTypes genTypes;
		readonly ExportedPythonTypes exportedPythonTypes;

		public PyiGen(GeneratorContext generatorContext) {
			genTypes = generatorContext.Types;
			exportedPythonTypes = genTypes.GetObject<ExportedPythonTypes>(TypeIds.ExportedPythonTypes);
		}

		public void Generate() {
			var classes = new List<PyClass>();
			foreach (var filename in Directory.GetFiles(genTypes.Dirs.GetPythonRustDir(), "*.rs")) {
				// mypy fix: we can't use Python enums (too slow). mypy complains because our enums are
				// ints, so we create dummy classes in lib.rs that the enum *.py files reference.
				// Ignore all of them.
				if (Path.GetFileName(filename) == "lib.rs")
					continue;
				var parser = new PyClassParser(filename);
				classes.AddRange(parser.ParseFile());
			}
			if (classes.Count == 0)
				throw new InvalidOperationException();

			WritePyi(classes);
		}

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
						if (!ParseUtils.TryConvertSphinxTypeToTypeName(docArg.SphinxType, out var typeName))
							continue;
						if (!exportedPythonTypes.TryFindByName(typeName, out var enumType))
							continue;
						argToEnumType.Add(docArg.Name, enumType);
					}

					var sigAttr = method.Attributes.Attributes.FirstOrDefault(a => a.Kind == AttributeKind.Signature);
					if (sigAttr is null)
						continue;
					foreach (var (name, value) in ParseUtils.GetArgsNameValues(sigAttr.Text)) {
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

			var filename = genTypes.Dirs.GetPythonPyFilename("_iced_x86_py.pyi");
			using (var writer = new FileWriter(TargetLanguage.Python, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				writer.WriteLine("# pylint: skip-file");
				writer.WriteLine();
				writer.WriteLine("from collections.abc import Iterator");
				writer.WriteLine("from enum import IntEnum, IntFlag");
				writer.WriteLine("from typing import Any, List, Optional, Union");
				writer.WriteLine();

				var idConverter = PythonIdentifierConverter.Create();
				var allEnumTypes = exportedPythonTypes.Enums.Select(a => (enumType: a, pythonName: a.Name(idConverter)));
				var toEnumType = allEnumTypes.ToDictionary(a => a.pythonName, a => a.enumType, StringComparer.Ordinal);
				foreach (var (enumType, pythonName) in allEnumTypes.OrderBy(a => a.pythonName, StringComparer.Ordinal)) {
					var baseClass = enumType.IsFlags ? "IntFlag" : "IntEnum";
					if (reqEnumFields.TryGetValue(enumType, out var fields)) {
						writer.WriteLine($"class {pythonName}({baseClass}):");
						using (writer.Indent()) {
							bool uppercaseRawName = Enums.EnumUtils.UppercaseTypeFields(enumType.TypeId.Id1);
							foreach (var value in enumType.Values) {
								if (fields.Contains(value)) {
									fields.Remove(value);
									var (valueName, numStr) = Enums.EnumUtils.GetEnumNameValue(idConverter, value, uppercaseRawName);
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

				var docGen = new PyiDocGen();
				foreach (var pyClass in classes.OrderBy(a => a.Name, StringComparer.Ordinal)) {
					writer.WriteLine();
					writer.WriteLine($"class {idConverter.Type(pyClass.Name)}:");
					using (writer.Indent()) {
						WriteDocs(writer, docGen.Convert(pyClass.DocComments));

						int defCount = 0;
						foreach (var member in GetMembers(pyClass)) {
							switch (member) {
							case PyMethod method:
								var docComments = method.Attributes.Any(AttributeKind.New) ?
									pyClass.DocComments : method.DocComments;
								Write(writer, docGen, idConverter, pyClass, method, docComments, toEnumType);
								defCount++;
								break;
							case PyProperty property:
								Write(writer, docGen, idConverter, pyClass, property.Getter, property.Getter.DocComments, toEnumType);
								defCount++;
								if (property.Setter is not null) {
									Write(writer, docGen, idConverter, pyClass, property.Setter, property.Getter.DocComments, toEnumType);
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
			}
		}

		static void WriteDocs(FileWriter writer, List<string> docs) {
			if (docs.Count == 0)
				throw new InvalidOperationException();

			const string docQuotes = "\"\"\"";
			if (docs.Count == 1)
				writer.WriteLine($"{docQuotes}{docs[0]}{docQuotes}");
			else {
				writer.WriteLine(docQuotes);
				foreach (var doc in docs) {
					if (doc == string.Empty)
						writer.WriteLineNoIndent(string.Empty);
					else
						writer.WriteLine(doc);
				}
				writer.WriteLine(docQuotes);
			}
		}

		static void Write(FileWriter writer, PyiDocGen docGen, IdentifierConverter idConverter, PyClass pyClass, PyMethod method, DocComments docComments, Dictionary<string, EnumType> toEnumType) {
			if (method.Attributes.Any(AttributeKind.ClassMethod))
				writer.WriteLine("@classmethod");
			if (method.Attributes.Any(AttributeKind.StaticMethod))
				writer.WriteLine("@staticmethod");
			bool isGetter = method.Attributes.Any(AttributeKind.Getter);
			bool isSetter = method.Attributes.Any(AttributeKind.Setter);
			if (isGetter)
				writer.WriteLine("@property");
			if (isSetter)
				writer.WriteLine($"@{method.Name}.setter");

			string sphinxReturnType = string.Empty;
			if (isGetter || isSetter) {
				if (docComments.Sections.FirstOrDefault() is not TextDocCommentSection textDocs || textDocs.Lines.Length == 0)
					throw new InvalidOperationException();
				if (!ParseUtils.TryParseTypeAndDocs(textDocs.Lines[0], out _, out var typeInfo))
					throw new InvalidOperationException();
				sphinxReturnType = typeInfo.SphinxType;
			}
			else {
				var returns = docComments.Sections.OfType<ReturnsDocCommentSection>().FirstOrDefault();
				if (returns is not null)
					sphinxReturnType = returns.Returns.SphinxType;
			}

			bool isCtor = method.Attributes.Any(AttributeKind.New);
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
			var sigAttr = method.Attributes.Attributes.FirstOrDefault(a => a.Kind == AttributeKind.Signature);
			if (sigAttr is null)
				toDefaultValue = new Dictionary<string, string>(StringComparer.Ordinal);
			else
				toDefaultValue = ParseUtils.GetArgsNameValues(sigAttr.Text).ToDictionary(a => a.name, a => a.value, StringComparer.Ordinal);

			for (int i = 0; i < method.Arguments.Count; i++) {
				if (argsDocs is not null && argsDocs.Args.Length != method.Arguments.Count - hasThis)
					throw new InvalidOperationException();
				var methodArg = method.Arguments[i];
				if (argCount > 0)
					writer.Write(", ");
				argCount++;
				if (methodArg.IsSelf)
					writer.Write("self");
				else {
					writer.Write(methodArg.Name);

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
				writer.Write(GetReturnType(pyClass, method.Name, method.RustReturnType, sphinxReturnType));
			else
				writer.Write("None");
			if (method.DocComments.Sections.Count == 0)
				writer.WriteLine(": ...");
			else {
				writer.WriteLine(":");
				using (writer.Indent()) {
					WriteDocs(writer, docGen.Convert(method.DocComments));
					writer.WriteLine("...");
				}
			}
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

		static string GetReturnType(PyClass pyClass, string methodName, string rustType, string sphinxType) {
			var typeStr = GetType(pyClass, methodName, rustType, sphinxType);
			if (methodName == "__iter__") {
				string returnType = pyClass.Name switch {
					"Decoder" => "Instruction",
					_ => throw new InvalidOperationException($"Unexpected iterator class {pyClass.Name}"),
				};
				return $"Iterator[{returnType}]";
			}
			return typeStr;
		}

		static string GetType(PyClass pyClass, string methodName, string rustType, string sphinxType) {
			// The type in the docs (sphinx type) is more accurate than the type in the source code
			// since `u32` is used in the source code if it's an enum value.
			if (sphinxType != string.Empty) {
				var sphinxTypes = ParseUtils.SplitSphinxTypes(sphinxType).ToList();
				var convertedTypes = new List<string>();
				foreach (var stype in sphinxTypes) {
					if (!ParseUtils.TryConvertSphinxTypeToTypeName(stype, out var typeName))
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

			if (ParseUtils.TryRemovePrefixSuffix(rustType, "PyResult<", ">", out var extractedType))
				rustType = extractedType;
			switch (rustType) {
			case "i8" or "i16" or "i32" or "i64" or "isize" or
				"u8" or "u16" or "u32" or "u64" or "usize":
				return "int";
			case "bool":
				return "bool";
			case "&str" or "String":
				return "str";
			case "PyRef<'_, Self>" or "PyRefMut<'_, Self>" or "Self":
				return pyClass.Name;
			case "&Bound<'_, PyAny>":
				return "Any";
			default:
				if (ParseUtils.TryRemovePrefixSuffix(rustType, "PyRef<'_, ", ">", out extractedType))
					return extractedType;
				if (ParseUtils.TryRemovePrefixSuffix(rustType, "PyRefMut<'_, ", ">", out extractedType))
					return extractedType;
				if (ParseUtils.TryRemovePrefixSuffix(rustType, "IterNextOutput<", ", ()>", out extractedType))
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

		static IEnumerable<object> GetMembers(PyClass pyClass) {
			var setters = pyClass.Methods.Where(a => a.Attributes.Any(AttributeKind.Setter)).ToDictionary(a => a.Name, a => a, StringComparer.Ordinal);
			foreach (var method in pyClass.Methods) {
				if (method.Attributes.Any(AttributeKind.Setter))
					continue;
				if (method.Attributes.Any(AttributeKind.Getter)) {
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
	}
}
