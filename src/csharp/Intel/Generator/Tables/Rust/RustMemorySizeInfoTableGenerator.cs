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
