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
			//TODO:
			// new FileUpdater(TargetLanguage.Java, "Dicts", JavaConstants.GetTestFilename(genTypes, JavaConstants.InstructionInfoPackage, "InstructionInfoConstants.java")).Generate(writer => {
			// 	WriteDict(writer, InstrInfoDictConstants.OpAccessConstants(genTypes), "ToAccess");
			// 	WriteDict(writer, InstrInfoDictConstants.MemorySizeFlagsTable(genTypes), "MemorySizeFlagsTable");
			// 	WriteDict(writer, InstrInfoDictConstants.RegisterFlagsTable(genTypes), "RegisterFlagsTable");
			// });
			//TODO:
			// new FileUpdater(TargetLanguage.Java, "Dicts", JavaConstants.GetTestFilename(genTypes, JavaConstants.EncoderPackage, "OpCodeInfoConstants.java")).Generate(writer => {
			// 	WriteDict(writer, EncoderConstants.EncodingKindTable(genTypes), "ToEncodingKind");
			// 	WriteDict(writer, EncoderConstants.MandatoryPrefixTable(genTypes), "ToMandatoryPrefix");
			// 	WriteDict(writer, EncoderConstants.OpCodeTableKindTable(genTypes), "ToOpCodeTableKind");
			// });
			//TODO:
			// new FileUpdater(TargetLanguage.Java, "Dicts", JavaConstants.GetTestFilename(genTypes, JavaConstants.MasmFormatterPackage, "SymbolOptionsTests.java")).Generate(writer => {
			// 	WriteDict(writer, MasmSymbolOptionsConstants.SymbolTestFlagsTable(genTypes), "ToSymbolTestFlags");
			// });
			//TODO:
			// new FileUpdater(TargetLanguage.Java, "Dicts", JavaConstants.GetTestFilename(genTypes, JavaConstants.FormatterPackage, "MnemonicOptionsTestsReader.java")).Generate(writer => {
			// 	WriteDict(writer, FormatMnemonicOptionsConstants.FormatMnemonicOptionsTable(genTypes), "ToFormatMnemonicOptions");
			// });
			//TODO:
			// new FileUpdater(TargetLanguage.Java, "Dicts", JavaConstants.GetTestFilename(genTypes, JavaConstants.FormatterPackage, "SymbolResolverTestsReader.java")).Generate(writer => {
			// 	WriteDict(writer, SymbolFlagsConstants.SymbolFlagsTable(genTypes), "ToSymbolFlags");
			// });
			new FileUpdater(TargetLanguage.Java, "IgnoredCode", JavaConstants.GetTestFilename(genTypes, JavaConstants.IcedPackage, "CodeUtils.java")).Generate(writer => {
				WriteHash(writer, genTypes.GetObject<HashSet<EnumValue>>(TypeIds.RemovedCodeValues), "ignored", "getIgnored", false);
			});
		}

		void WriteDict(FileWriter writer, (string name, EnumValue value)[] constants, string fieldName, bool publicField = true) {
			var declTypeStr = constants[0].value.DeclaringType.Name(idConverter);
			writer.WriteLine($"{(publicField ? "internal " : string.Empty)}static readonly Dictionary<string, {declTypeStr}> {fieldName} = new Dictionary<string, {declTypeStr}>({constants.Length}, StringComparer.Ordinal) {{");
			using (writer.Indent()) {
				foreach (var constant in constants)
					writer.WriteLine($"{{ \"{constant.name}\", {idConverter.ToDeclTypeAndValue(constant.value)} }},");
			}
			writer.WriteLine("};");
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
