// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Linq;
using Generator.Enums;
using Generator.IO;

namespace Generator.Tables.Rust {
	[Generator(TargetLanguage.Rust)]
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
					generatorContext.Types.Dirs.GetRustFilename("info", "tests", "test_parser.rs"),
					"OpAccessDict",
					writer => WriteDict(writer, InstrInfoDictConstants.OpAccessConstants(genTypes), "to_access")
				),
				(
					generatorContext.Types.Dirs.GetRustFilename("info", "tests", "mem_size_test_parser.rs"),
					"FlagsDict",
					writer => WriteDict(writer, InstrInfoDictConstants.MemorySizeFlagsTable(genTypes), "to_flags")
				),
				(
					generatorContext.Types.Dirs.GetRustFilename("info", "tests", "reg_test_parser.rs"),
					"FlagsDict",
					writer => WriteDict(writer, InstrInfoDictConstants.RegisterFlagsTable(genTypes), "to_flags")
				),
				(
					generatorContext.Types.Dirs.GetRustFilename("encoder", "tests", "op_code_test_case_parser.rs"),
					"EncodingKindDict",
					writer => WriteDict(writer, EncoderConstants.EncodingKindTable(genTypes), "to_encoding_kind")
				),
				(
					generatorContext.Types.Dirs.GetRustFilename("encoder", "tests", "op_code_test_case_parser.rs"),
					"MandatoryPrefixDict",
					writer => WriteDict(writer, EncoderConstants.MandatoryPrefixTable(genTypes), "to_mandatory_prefix")
				),
				(
					generatorContext.Types.Dirs.GetRustFilename("encoder", "tests", "op_code_test_case_parser.rs"),
					"OpCodeTableKindDict",
					writer => WriteDict(writer, EncoderConstants.OpCodeTableKindTable(genTypes), "to_op_code_table_kind")
				),
				(
					generatorContext.Types.Dirs.GetRustFilename("formatter", "masm", "tests", "sym_opts_parser.rs"),
					"SymbolTestFlagsDict",
					writer => WriteDict(writer, MasmSymbolOptionsConstants.SymbolTestFlagsTable(genTypes), "to_flags")
				),
				(
					generatorContext.Types.Dirs.GetRustFilename("formatter", "tests", "mnemonic_opts_parser.rs"),
					"OptionsDict",
					writer => WriteDict(writer, FormatMnemonicOptionsConstants.FormatMnemonicOptionsTable(genTypes), "to_flags")
				),
				(
					generatorContext.Types.Dirs.GetRustFilename("formatter", "tests", "sym_res_test_parser.rs"),
					"OptionsDict",
					writer => WriteDict(writer, SymbolFlagsConstants.SymbolFlagsTable(genTypes), "to_flags")
				),
				(
					generatorContext.Types.Dirs.GetRustFilename("test_utils", "from_str_conv", "ignored_code_table.rs"),
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
				writer.WriteLine($"let _ = {hashName}.insert(\"{constant.name}\", {declTypeStr}::{name});");
			}
		}

		static void WriteHash(FileWriter writer, HashSet<EnumValue> constants, string hashName) {
			var consts = constants.OrderBy(a => a.Value).ToArray();
			if (consts.Length == 0)
				writer.WriteLine($"let {hashName}: HashSet<&'static str> = HashSet::new();");
			else
				writer.WriteLine($"let mut {hashName}: HashSet<&'static str> = HashSet::with_capacity({consts.Length});");
			foreach (var constant in consts)
				writer.WriteLine($"let _ = {hashName}.insert(\"{constant.RawName}\");");
		}
	}
}
