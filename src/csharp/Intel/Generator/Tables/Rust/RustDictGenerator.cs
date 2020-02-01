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
using System.IO;
using Generator.Enums;
using Generator.IO;

namespace Generator.Tables.Rust {
	[Generator(TargetLanguage.Rust, GeneratorNames.Dictionaries)]
	sealed class RustDictGenerator {
		readonly IdentifierConverter idConverter;
		readonly GeneratorOptions generatorOptions;

		public RustDictGenerator(GeneratorOptions generatorOptions) {
			idConverter = RustIdentifierConverter.Create();
			this.generatorOptions = generatorOptions;
		}

		public void Generate() {
			var infos = new (string filename, string id, Action<FileWriter> func)[] {
				(
					Path.Combine(generatorOptions.RustDir, "info", "tests", "test_parser.rs"),
					"OpAccessDict",
					writer => WriteDict(writer, InstrInfoDictConstants.OpAccessConstants, "to_access")
				),
				(
					Path.Combine(generatorOptions.RustDir, "info", "tests", "mem_size_test_parser.rs"),
					"FlagsDict",
					writer => WriteDict(writer, InstrInfoDictConstants.MemorySizeFlagsTable, "to_flags")
				),
				(
					Path.Combine(generatorOptions.RustDir, "info", "tests", "reg_test_parser.rs"),
					"FlagsDict",
					writer => WriteDict(writer, InstrInfoDictConstants.RegisterFlagsTable, "to_flags")
				),
				(
					Path.Combine(generatorOptions.RustDir, "encoder", "tests", "op_code_test_case_parser.rs"),
					"EncodingKindDict",
					writer => WriteDict(writer, EncoderConstants.EncodingKindTable, "to_encoding_kind")
				),
				(
					Path.Combine(generatorOptions.RustDir, "encoder", "tests", "op_code_test_case_parser.rs"),
					"MandatoryPrefixDict",
					writer => WriteDict(writer, EncoderConstants.MandatoryPrefixTable, "to_mandatory_prefix")
				),
				(
					Path.Combine(generatorOptions.RustDir, "encoder", "tests", "op_code_test_case_parser.rs"),
					"OpCodeTableKindDict",
					writer => WriteDict(writer, EncoderConstants.OpCodeTableKindTable, "to_op_code_table_kind")
				),
				(
					Path.Combine(generatorOptions.RustDir, "formatter", "masm", "tests", "sym_opts_parser.rs"),
					"SymbolTestFlagsDict",
					writer => WriteDict(writer, MasmSymbolOptionsConstants.SymbolTestFlagsTable, "to_flags")
				),
			};
			foreach (var info in infos) {
				new FileUpdater(TargetLanguage.Rust, info.id, info.filename).Generate(writer => info.func(writer));
			}
		}

		void WriteDict(FileWriter writer, (string name, EnumValue value)[] constants, string hashName) {
			var declType = constants[0].value.DeclaringType;
			var declTypeStr = declType.Name(idConverter);
			writer.WriteLine($"let mut {hashName}: HashMap<&'static str, {(declType.IsFlags ? "u32" : declTypeStr)}> = HashMap::new();");
			foreach (var constant in constants) {
				var name = declType.IsFlags ? idConverter.Constant(constant.value.RawName) : constant.value.Name(idConverter);
				writer.WriteLine($"let _ = {hashName}.insert(\"{constant.name}\", {declTypeStr}::{name});");
			}
		}
	}
}
