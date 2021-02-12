// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

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
			var memSizeName = generatorContext.Types[TypeIds.MemorySize].Name(idConverter);
			foreach (var info in defs)
				writer.WriteLine($"MemorySizeInfo {{ size: {info.Size}, element_size: {info.ElementSize}, memory_size: {memSizeName}::{info.MemorySize.Name(idConverter)}, element_type: {memSizeName}::{info.ElementType.Name(idConverter)}, is_signed: {(info.IsSigned ? "true" : "false")}, is_broadcast: {(info.IsBroadcast ? "true" : "false")} }},");
		}
	}
}
