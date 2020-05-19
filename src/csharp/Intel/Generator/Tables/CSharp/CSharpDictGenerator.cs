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

using System.Collections.Generic;
using System.IO;
using Generator.Enums;
using Generator.IO;

namespace Generator.Tables.CSharp {
	[Generator(TargetLanguage.CSharp, GeneratorNames.Dictionaries)]
	sealed class CSharpDictGenerator {
		readonly IdentifierConverter idConverter;
		readonly GeneratorContext generatorContext;

		public CSharpDictGenerator(GeneratorContext generatorContext) {
			idConverter = CSharpIdentifierConverter.Create();
			this.generatorContext = generatorContext;
		}

		public void Generate() {
			var genTypes = generatorContext.Types;
			new FileUpdater(TargetLanguage.CSharp, "Dicts", Path.Combine(generatorContext.CSharpTestsDir, "Intel", "InstructionInfoTests", "InstructionInfoConstants.cs")).Generate(writer => {
				WriteDict(writer, InstrInfoDictConstants.OpAccessConstants(genTypes), "ToAccess");
				WriteDict(writer, InstrInfoDictConstants.MemorySizeFlagsTable(genTypes), "MemorySizeFlagsTable");
				WriteDict(writer, InstrInfoDictConstants.RegisterFlagsTable(genTypes), "RegisterFlagsTable");
			});
			new FileUpdater(TargetLanguage.CSharp, "Dicts", Path.Combine(generatorContext.CSharpTestsDir, "Intel", "EncoderTests", "OpCodeInfoConstants.cs")).Generate(writer => {
				WriteDict(writer, EncoderConstants.EncodingKindTable(genTypes), "ToEncodingKind");
				WriteDict(writer, EncoderConstants.MandatoryPrefixTable(genTypes), "ToMandatoryPrefix");
				WriteDict(writer, EncoderConstants.OpCodeTableKindTable(genTypes), "ToOpCodeTableKind");
			});
			new FileUpdater(TargetLanguage.CSharp, "Dicts", Path.Combine(generatorContext.CSharpTestsDir, "Intel", "FormatterTests", "Masm", "SymbolOptionsTests.cs")).Generate(writer => {
				WriteDict(writer, MasmSymbolOptionsConstants.SymbolTestFlagsTable(genTypes), "ToSymbolTestFlags");
			});
			new FileUpdater(TargetLanguage.CSharp, "Dicts", Path.Combine(generatorContext.CSharpTestsDir, "Intel", "FormatterTests", "MnemonicOptionsTestsReader.cs")).Generate(writer => {
				WriteDict(writer, FormatMnemonicOptionsConstants.FormatMnemonicOptionsTable(genTypes), "ToFormatMnemonicOptions");
			});
			new FileUpdater(TargetLanguage.CSharp, "Dicts", Path.Combine(generatorContext.CSharpTestsDir, "Intel", "FormatterTests", "SymbolResolverTestsReader.cs")).Generate(writer => {
				WriteDict(writer, SymbolFlagsConstants.SymbolFlagsTable(genTypes), "ToSymbolFlags");
			});
			new FileUpdater(TargetLanguage.CSharp, "IgnoredCode", Path.Combine(generatorContext.CSharpTestsDir, "Intel", "CodeUtils.cs")).Generate(writer => {
				WriteHash(writer, genTypes.GetObject<HashSet<EnumValue>>(TypeIds.RemovedCodeValues), "ignored", false);
			});
		}

		void WriteDict(FileWriter writer, (string name, EnumValue value)[] constants, string fieldName, bool publicField = true) {
			var declTypeStr = constants[0].value.DeclaringType.Name(idConverter);
			writer.WriteLine($"{(publicField ? "internal " : string.Empty)}static readonly Dictionary<string, {declTypeStr}> {fieldName} = new Dictionary<string, {declTypeStr}>({constants.Length}, StringComparer.Ordinal) {{");
			using (writer.Indent()) {
				foreach (var constant in constants)
					writer.WriteLine($"{{ \"{constant.name}\", {declTypeStr}.{constant.value.Name(idConverter)} }},");
			}
			writer.WriteLine("};");
		}

		void WriteHash(FileWriter writer, HashSet<EnumValue> constants, string fieldName, bool publicField = true) {
			writer.WriteLine($"{(publicField ? "internal " : string.Empty)}static readonly HashSet<string> {fieldName} = new HashSet<string>({constants.Count}, StringComparer.Ordinal) {{");
			using (writer.Indent()) {
				foreach (var constant in constants)
					writer.WriteLine($"{{ \"{constant.RawName}\" }},");
			}
			writer.WriteLine("};");
		}
	}
}
