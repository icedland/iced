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

using System.IO;
using Generator.Enums;
using Generator.IO;

namespace Generator.Tables.CSharp {
	[Generator(TargetLanguage.CSharp, GeneratorNames.Dictionaries)]
	sealed class CSharpDictGenerator {
		readonly IdentifierConverter idConverter;
		readonly GeneratorOptions generatorOptions;

		public CSharpDictGenerator(GeneratorOptions generatorOptions) {
			idConverter = CSharpIdentifierConverter.Create();
			this.generatorOptions = generatorOptions;
		}

		public void Generate() {
			new FileUpdater(TargetLanguage.CSharp, "Dicts", Path.Combine(generatorOptions.CSharpTestsDir, "Intel", "InstructionInfoTests", "InstructionInfoConstants.cs")).Generate(writer => {
				WriteDict(writer, InstrInfoDictConstants.OpAccessConstants, "ToAccess");
				WriteDict(writer, InstrInfoDictConstants.MemorySizeFlagsTable, "MemorySizeFlagsTable");
				WriteDict(writer, InstrInfoDictConstants.RegisterFlagsTable, "RegisterFlagsTable");
			});
			new FileUpdater(TargetLanguage.CSharp, "Dicts", Path.Combine(generatorOptions.CSharpTestsDir, "Intel", "EncoderTests", "OpCodeInfoConstants.cs")).Generate(writer => {
				WriteDict(writer, EncoderConstants.EncodingKindTable, "ToEncodingKind");
				WriteDict(writer, EncoderConstants.MandatoryPrefixTable, "ToMandatoryPrefix");
				WriteDict(writer, EncoderConstants.OpCodeTableKindTable, "ToOpCodeTableKind");
			});
			new FileUpdater(TargetLanguage.CSharp, "Dicts", Path.Combine(generatorOptions.CSharpTestsDir, "Intel", "FormatterTests", "Masm", "SymbolOptionsTests.cs")).Generate(writer => {
				WriteDict(writer, MasmSymbolOptionsConstants.SymbolTestFlagsTable, "ToSymbolTestFlags");
			});
			new FileUpdater(TargetLanguage.CSharp, "Dicts", Path.Combine(generatorOptions.CSharpTestsDir, "Intel", "FormatterTests", "MnemonicOptionsTestsReader.cs")).Generate(writer => {
				WriteDict(writer, FormatMnemonicOptionsConstants.FormatMnemonicOptionsTable, "ToFormatMnemonicOptions");
			});
		}

		void WriteDict(FileWriter writer, (string name, EnumValue value)[] constants, string fieldName) {
			var declTypeStr = constants[0].value.DeclaringType.Name(idConverter);
			writer.WriteLine($"internal static readonly Dictionary<string, {declTypeStr}> {fieldName} = new Dictionary<string, {declTypeStr}>(StringComparer.Ordinal) {{");
			using (writer.Indent()) {
				foreach (var constant in constants)
					writer.WriteLine($"{{ \"{constant.name}\", {declTypeStr}.{constant.value.Name(idConverter)} }},");
			}
			writer.WriteLine("};");
		}
	}
}
