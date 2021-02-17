// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using Generator.IO;

namespace Generator.Tables.Rust {
	[Generator(TargetLanguage.Rust)]
	sealed class RustRegisterInfoTableGenerator {
		readonly IdentifierConverter idConverter;
		readonly GeneratorContext generatorContext;

		public RustRegisterInfoTableGenerator(GeneratorContext generatorContext) {
			idConverter = RustIdentifierConverter.Create();
			this.generatorContext = generatorContext;
		}

		public void Generate() {
			var defs = generatorContext.Types.GetObject<RegisterDefs>(TypeIds.RegisterDefs).Defs;
			var filename = generatorContext.Types.Dirs.GetRustFilename("register.rs");
			var updater = new FileUpdater(TargetLanguage.Rust, "RegisterInfoTable", filename);
			updater.Generate(writer => WriteTable(writer, defs));
		}

		void WriteTable(FileWriter writer, RegisterDef[] defs) {
			var genTypes = generatorContext.Types;
			var regName = genTypes[TypeIds.Register].Name(idConverter);
			if (genTypes[TypeIds.Register].Values.Length > 0x100)
				throw new InvalidOperationException();
			foreach (var def in defs)
				writer.WriteLine($"RegisterInfo {{ register: {regName}::{def.Register.Name(idConverter)}, base: {regName}::{def.BaseRegister.Name(idConverter)}, full_register32: {regName}::{def.FullRegister32.Name(idConverter)}, full_register: {regName}::{def.FullRegister.Name(idConverter)}, size: {def.Size} }},");
		}
	}
}
