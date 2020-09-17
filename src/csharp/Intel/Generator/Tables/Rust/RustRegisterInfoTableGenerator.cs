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
			var infos = generatorContext.Types.GetObject<RegisterInfoTable>(TypeIds.RegisterInfoTable).Data;
			var filename = Path.Combine(generatorContext.RustDir, "register.rs");
			var updater = new FileUpdater(TargetLanguage.Rust, "RegisterInfoTable", filename);
			updater.Generate(writer => WriteTable(writer, infos));
		}

		void WriteTable(FileWriter writer, RegisterInfo[] infos) {
			var genTypes = generatorContext.Types;
			var regName = genTypes[TypeIds.Register].Name(idConverter);
			if (genTypes[TypeIds.Register].Values.Length > 0x100)
				throw new InvalidOperationException();
			foreach (var info in infos)
				writer.WriteLine($"RegisterInfo {{ register: {regName}::{info.Register.Name(idConverter)}, base: {regName}::{info.Base.Name(idConverter)}, full_register: {regName}::{info.FullRegister.Name(idConverter)}, size: {info.Size} }},");
		}
	}
}
