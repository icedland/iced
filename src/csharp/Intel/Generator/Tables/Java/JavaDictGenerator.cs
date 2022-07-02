// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Linq;
using Generator.Enums;
using Generator.IO;

namespace Generator.Tables.Java {
	[Generator(TargetLanguage.Java)]
	sealed class JavaDictGenerator {
		// Java method byte code limitations
		const int maxVariantsPerMethod = 500;

		readonly IdentifierConverter idConverter;
		readonly GeneratorContext generatorContext;

		public JavaDictGenerator(GeneratorContext generatorContext) {
			idConverter = JavaIdentifierConverter.Create();
			this.generatorContext = generatorContext;
		}

		public void Generate() {
			var genTypes = generatorContext.Types;
			var dirs = genTypes.Dirs;
			using (var writer = new FileWriter(TargetLanguage.Java, FileUtils.OpenWrite(JavaConstants.GetTestFilename(genTypes, JavaConstants.InstructionInfoPackage, "InstrInfoDicts.java")))) {
				writer.WriteFileHeader();
				writer.WriteLine($"package {JavaConstants.InstructionInfoPackage};");
				writer.WriteLine();
				writer.WriteLine("import java.util.HashMap;");
				writer.WriteLine();
				writer.WriteLine("final class InstrInfoDicts {");
				using (writer.Indent()) {
					WriteDict(writer, InstrInfoDictConstants.OpAccessConstants(genTypes));
					writer.WriteLine();
					WriteDict(writer, InstrInfoDictConstants.MemorySizeFlagsTable(genTypes));
					writer.WriteLine();
					WriteDict(writer, InstrInfoDictConstants.RegisterFlagsTable(genTypes));
				}
				writer.WriteLine("}");
			}
			using (var writer = new FileWriter(TargetLanguage.Java, FileUtils.OpenWrite(JavaConstants.GetTestFilename(genTypes, JavaConstants.InstructionInfoPackage, "OpCodeInfoDicts.java")))) {
				writer.WriteFileHeader();
				writer.WriteLine($"package {JavaConstants.InstructionInfoPackage};");
				writer.WriteLine();
				writer.WriteLine("import java.util.HashMap;");
				writer.WriteLine();
				writer.WriteLine($"import {JavaConstants.IcedPackage}.EncodingKind;");
				writer.WriteLine();
				writer.WriteLine("final class OpCodeInfoDicts {");
				using (writer.Indent()) {
					WriteDict(writer, EncoderConstants.EncodingKindTable(genTypes));
					writer.WriteLine();
					WriteDict(writer, EncoderConstants.MandatoryPrefixTable(genTypes));
					writer.WriteLine();
					WriteDict(writer, EncoderConstants.OpCodeTableKindTable(genTypes));
				}
				writer.WriteLine("}");
			}
			using (var writer = new FileWriter(TargetLanguage.Java, FileUtils.OpenWrite(JavaConstants.GetTestFilename(genTypes, JavaConstants.MasmFormatterPackage, "SymbolTestFlagsDict.java")))) {
				writer.WriteFileHeader();
				writer.WriteLine($"package {JavaConstants.MasmFormatterPackage};");
				writer.WriteLine();
				writer.WriteLine("import java.util.HashMap;");
				writer.WriteLine();
				writer.WriteLine("final class SymbolTestFlagsDict {");
				using (writer.Indent())
					WriteDict(writer, MasmSymbolOptionsConstants.SymbolTestFlagsTable(genTypes));
				writer.WriteLine("}");
			}
			using (var writer = new FileWriter(TargetLanguage.Java, FileUtils.OpenWrite(JavaConstants.GetTestFilename(genTypes, JavaConstants.FormatterPackage, "MnemonicOptionsDict.java")))) {
				writer.WriteFileHeader();
				writer.WriteLine($"package {JavaConstants.FormatterPackage};");
				writer.WriteLine();
				writer.WriteLine("import java.util.HashMap;");
				writer.WriteLine();
				writer.WriteLine("final class MnemonicOptionsDict {");
				using (writer.Indent())
					WriteDict(writer, FormatMnemonicOptionsConstants.FormatMnemonicOptionsTable(genTypes));
				writer.WriteLine("}");
			}
			using (var writer = new FileWriter(TargetLanguage.Java, FileUtils.OpenWrite(JavaConstants.GetTestFilename(genTypes, JavaConstants.FormatterPackage, "SymbolResolverDicts.java")))) {
				writer.WriteFileHeader();
				writer.WriteLine($"package {JavaConstants.FormatterPackage};");
				writer.WriteLine();
				writer.WriteLine("import java.util.HashMap;");
				writer.WriteLine();
				writer.WriteLine("final class SymbolResolverDicts {");
				using (writer.Indent())
					WriteDict(writer, SymbolFlagsConstants.SymbolFlagsTable(genTypes));
				writer.WriteLine("}");
			}
			new FileUpdater(TargetLanguage.Java, "IgnoredCode", JavaConstants.GetTestFilename(genTypes, JavaConstants.IcedPackage, "CodeUtils.java")).Generate(writer => {
				WriteHash(writer, genTypes.GetObject<HashSet<EnumValue>>(TypeIds.RemovedCodeValues), "ignored", "getIgnored", false);
			});
		}

		void WriteDict(FileWriter writer, (string name, EnumValue value)[] constants) {
			var fieldName = constants[0].value.DeclaringType.Name(idConverter);
			writer.WriteLine($"static final HashMap<String, Integer> to{fieldName} = create{fieldName}();");
			writer.WriteLine($"private static HashMap<String, Integer> create{fieldName}() {{");
			using (writer.Indent()) {
				writer.WriteLine($"HashMap<String, Integer> map = new HashMap<String, Integer>({constants.Length});");
				foreach (var constant in constants)
					writer.WriteLine($"map.put(\"{constant.name}\", {idConverter.ToDeclTypeAndValue(constant.value)});");
				writer.WriteLine("return map;");
			}
			writer.WriteLine("}");
		}

		static void WriteHash(FileWriter writer, HashSet<EnumValue> constants, string fieldName, string createFnName, bool publicField = true) {
			var consts = constants.OrderBy(a => a.Value).ToArray();
			int variantMethods = (consts.Length + maxVariantsPerMethod - 1) / maxVariantsPerMethod;
			writer.WriteLine($"private static HashSet<String> {createFnName}() {{");
			using (writer.Indent()) {
				writer.WriteLine($"HashSet<String> set = new HashSet<String>({consts.Length});");
				for (int i = 0; i < variantMethods; i++)
					writer.WriteLine($"{createFnName}{i}(set)");
				writer.WriteLine("return set;");
			}
			writer.WriteLine("}");
			for (int i = 0; i < variantMethods; i++) {
				writer.WriteLine($"private static {createFnName}{i}(HashSet<String> set) {{");
				using (writer.Indent()) {
					int start = i * maxVariantsPerMethod;
					int end = Math.Min(start + maxVariantsPerMethod, consts.Length);
					for (int j = start; j < end; j++) {
						var c = consts[j];
						writer.WriteLine($"set.add(\"{c.RawName}\")");
					}
				}
				writer.WriteLine("}");
			}
		}
	}
}
