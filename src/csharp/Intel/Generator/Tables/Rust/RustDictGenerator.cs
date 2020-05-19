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
using System.IO;
using Generator.Enums;
using Generator.IO;

namespace Generator.Tables.Rust {
	[Generator(TargetLanguage.Rust, GeneratorNames.Dictionaries)]
	sealed class RustDictGenerator {
		readonly IdentifierConverter idConverter;
		readonly GeneratorContext generatorContext;

		public RustDictGenerator(GeneratorContext generatorContext) {
			idConverter = RustIdentifierConverter.Create();
			this.generatorContext = generatorContext;
		}

		public void Generate() {
			var genTypes = generatorContext.Types;
			var infos = new (string filename, string id, Action<FileWriter> func)[] {
				(
					Path.Combine(generatorContext.RustDir, "info", "tests", "test_parser.rs"),
					"OpAccessDict",
					writer => WriteDict(writer, InstrInfoDictConstants.OpAccessConstants(genTypes), "to_access")
				),
				(
					Path.Combine(generatorContext.RustDir, "info", "tests", "mem_size_test_parser.rs"),
					"FlagsDict",
					writer => WriteDict(writer, InstrInfoDictConstants.MemorySizeFlagsTable(genTypes), "to_flags")
				),
				(
					Path.Combine(generatorContext.RustDir, "info", "tests", "reg_test_parser.rs"),
					"FlagsDict",
					writer => WriteDict(writer, InstrInfoDictConstants.RegisterFlagsTable(genTypes), "to_flags")
				),
				(
					Path.Combine(generatorContext.RustDir, "encoder", "tests", "op_code_test_case_parser.rs"),
					"EncodingKindDict",
					writer => WriteDict(writer, EncoderConstants.EncodingKindTable(genTypes), "to_encoding_kind")
				),
				(
					Path.Combine(generatorContext.RustDir, "encoder", "tests", "op_code_test_case_parser.rs"),
					"MandatoryPrefixDict",
					writer => WriteDict(writer, EncoderConstants.MandatoryPrefixTable(genTypes), "to_mandatory_prefix")
				),
				(
					Path.Combine(generatorContext.RustDir, "encoder", "tests", "op_code_test_case_parser.rs"),
					"OpCodeTableKindDict",
					writer => WriteDict(writer, EncoderConstants.OpCodeTableKindTable(genTypes), "to_op_code_table_kind")
				),
				(
					Path.Combine(generatorContext.RustDir, "formatter", "masm", "tests", "sym_opts_parser.rs"),
					"SymbolTestFlagsDict",
					writer => WriteDict(writer, MasmSymbolOptionsConstants.SymbolTestFlagsTable(genTypes), "to_flags")
				),
				(
					Path.Combine(generatorContext.RustDir, "formatter", "tests", "mnemonic_opts_parser.rs"),
					"OptionsDict",
					writer => WriteDict(writer, FormatMnemonicOptionsConstants.FormatMnemonicOptionsTable(genTypes), "to_flags")
				),
				(
					Path.Combine(generatorContext.RustDir, "formatter", "tests", "sym_res_test_parser.rs"),
					"OptionsDict",
					writer => WriteDict(writer, SymbolFlagsConstants.SymbolFlagsTable(genTypes), "to_flags")
				),
				(
					Path.Combine(generatorContext.RustDir, "test_utils", "from_str_conv", "ignored_code_table.rs"),
					"CodeHash",
					writer => WriteHash(writer, genTypes.GetObject<HashSet<EnumValue>>(TypeIds.RemovedCodeValues), "h")
				),
			};
			foreach (var info in infos)
				new FileUpdater(TargetLanguage.Rust, info.id, info.filename).Generate(writer => info.func(writer));
		}

		void WriteDict(FileWriter writer, (string name, EnumValue value)[] constants, string hashName) {
			var declType = constants[0].value.DeclaringType;
			var declTypeStr = declType.Name(idConverter);
			if (constants.Length == 0)
				writer.WriteLine($"let {hashName}: HashMap<&'static str, {(declType.IsFlags ? "u32" : declTypeStr)}> = HashMap::new();");
			else
				writer.WriteLine($"let mut {hashName}: HashMap<&'static str, {(declType.IsFlags ? "u32" : declTypeStr)}> = HashMap::with_capacity({constants.Length});");
			foreach (var constant in constants) {
				var name = declType.IsFlags ? idConverter.Constant(constant.value.RawName) : constant.value.Name(idConverter);
				writer.WriteLine($"{hashName}.insert(\"{constant.name}\", {declTypeStr}::{name});");
			}
		}

		void WriteHash(FileWriter writer, HashSet<EnumValue> constants, string hashName) {
			if (constants.Count == 0)
				writer.WriteLine($"let {hashName}: HashSet<&'static str> = HashSet::new();");
			else
				writer.WriteLine($"let mut {hashName}: HashSet<&'static str> = HashSet::with_capacity({constants.Count});");
			foreach (var constant in constants)
				writer.WriteLine($"{hashName}.insert(\"{constant.RawName}\");");
		}
	}
}
