// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System.Collections.Generic;
using System.Linq;
using Generator.Enums;
using Generator.IO;

namespace Generator.Tables.CSharp {
	[Generator(TargetLanguage.CSharp)]
	sealed class CSharpDictGenerator {
		readonly IdentifierConverter idConverter;
		readonly GeneratorContext generatorContext;

		public CSharpDictGenerator(GeneratorContext generatorContext) {
			idConverter = CSharpIdentifierConverter.Create();
			this.generatorContext = generatorContext;
		}

		public void Generate() {
			var genTypes = generatorContext.Types;
			var dirs = genTypes.Dirs;
			new FileUpdater(TargetLanguage.CSharp, "Dicts", dirs.GetCSharpTestFilename("Intel", "InstructionInfoTests", "InstructionInfoConstants.cs")).Generate(writer => {
				WriteDict(writer, InstrInfoDictConstants.OpAccessConstants(genTypes), "ToAccess");
				WriteDict(writer, InstrInfoDictConstants.MemorySizeFlagsTable(genTypes), "MemorySizeFlagsTable");
				WriteDict(writer, InstrInfoDictConstants.RegisterFlagsTable(genTypes), "RegisterFlagsTable");
			});
			new FileUpdater(TargetLanguage.CSharp, "Dicts", dirs.GetCSharpTestFilename("Intel", "EncoderTests", "OpCodeInfoConstants.cs")).Generate(writer => {
				WriteDict(writer, EncoderConstants.EncodingKindTable(genTypes), "ToEncodingKind");
				WriteDict(writer, EncoderConstants.MandatoryPrefixTable(genTypes), "ToMandatoryPrefix");
				WriteDict(writer, EncoderConstants.OpCodeTableKindTable(genTypes), "ToOpCodeTableKind");
			});
			new FileUpdater(TargetLanguage.CSharp, "Dicts", dirs.GetCSharpTestFilename("Intel", "FormatterTests", "Masm", "SymbolOptionsTests.cs")).Generate(writer => {
				WriteDict(writer, MasmSymbolOptionsConstants.SymbolTestFlagsTable(genTypes), "ToSymbolTestFlags");
			});
			new FileUpdater(TargetLanguage.CSharp, "Dicts", dirs.GetCSharpTestFilename("Intel", "FormatterTests", "MnemonicOptionsTestsReader.cs")).Generate(writer => {
				WriteDict(writer, FormatMnemonicOptionsConstants.FormatMnemonicOptionsTable(genTypes), "ToFormatMnemonicOptions");
			});
			new FileUpdater(TargetLanguage.CSharp, "Dicts", dirs.GetCSharpTestFilename("Intel", "FormatterTests", "SymbolResolverTestsReader.cs")).Generate(writer => {
				WriteDict(writer, SymbolFlagsConstants.SymbolFlagsTable(genTypes), "ToSymbolFlags");
			});
			new FileUpdater(TargetLanguage.CSharp, "IgnoredCode", dirs.GetCSharpTestFilename("Intel", "CodeUtils.cs")).Generate(writer => {
				WriteHash(writer, genTypes.GetObject<HashSet<EnumValue>>(TypeIds.RemovedCodeValues), "ignored", false);
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

		static void WriteHash(FileWriter writer, HashSet<EnumValue> constants, string fieldName, bool publicField = true) {
			var consts = constants.OrderBy(a => a.Value).ToArray();
			writer.WriteLine($"{(publicField ? "internal " : string.Empty)}static readonly HashSet<string> {fieldName} = new HashSet<string>({consts.Length}, StringComparer.Ordinal) {{");
			using (writer.Indent()) {
				foreach (var constant in consts)
					writer.WriteLine($"{{ \"{constant.RawName}\" }},");
			}
			writer.WriteLine("};");
		}
	}
}
