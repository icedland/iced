// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using Generator.IO;

namespace Generator.Tables.Rust {
	[Generator(TargetLanguage.Rust)]
	sealed class RustMemorySizeInfoTableGenerator {
		readonly IdentifierConverter idConverter;
		readonly GeneratorContext generatorContext;

		public RustMemorySizeInfoTableGenerator(GeneratorContext generatorContext) {
			idConverter = RustIdentifierConverter.Create();
			this.generatorContext = generatorContext;
		}

		public void Generate() {
			var defs = generatorContext.Types.GetObject<MemorySizeDefs>(TypeIds.MemorySizeDefs).Defs;
			var filename = generatorContext.Types.Dirs.GetRustFilename("memory_size.rs");
			var updater = new FileUpdater(TargetLanguage.Rust, "MemorySizeInfoTable", filename);
			updater.Generate(writer => WriteTable(writer, defs));
		}

		void WriteTable(FileWriter writer, MemorySizeDef[] defs) {
			foreach (var info in defs)
				writer.WriteLine($"MemorySizeInfo {{ size: {info.Size}, element_size: {info.ElementSize}, memory_size: {idConverter.ToDeclTypeAndValue(info.MemorySize)}, element_type: {idConverter.ToDeclTypeAndValue(info.ElementType)}, is_signed: {(info.IsSigned ? "true" : "false")}, is_broadcast: {(info.IsBroadcast ? "true" : "false")} }},");
		}
	}
}
