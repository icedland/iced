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
using Generator.IO;

namespace Generator.Formatters.Rust {
	[Generator(TargetLanguage.Rust, GeneratorNames.Formatter_Table)]
	sealed class RustFormatterTableGenerator {
		readonly GeneratorContext generatorContext;

		public RustFormatterTableGenerator(GeneratorContext generatorContext) =>
			this.generatorContext = generatorContext;

		public void Generate() {
			var genTypes = generatorContext.Types;
			var serializers = new List<RustFormatterTableSerializer>();
			var basePath = Path.Combine(generatorContext.RustDir, "formatter");
			if (genTypes.Options.HasGasFormatter)
				serializers.Add(new RustFormatterTableSerializer(Path.Combine(basePath, "gas", "fmt_data.rs"), genTypes.GetObject<Gas.CtorInfos>(TypeIds.GasCtorInfos).Infos, genTypes[TypeIds.GasCtorKind]));
			if (genTypes.Options.HasIntelFormatter)
				serializers.Add(new RustFormatterTableSerializer(Path.Combine(basePath, "intel", "fmt_data.rs"), genTypes.GetObject<Intel.CtorInfos>(TypeIds.IntelCtorInfos).Infos, genTypes[TypeIds.IntelCtorKind]));
			if (genTypes.Options.HasMasmFormatter)
				serializers.Add(new RustFormatterTableSerializer(Path.Combine(basePath, "masm", "fmt_data.rs"), genTypes.GetObject<Masm.CtorInfos>(TypeIds.MasmCtorInfos).Infos, genTypes[TypeIds.MasmCtorKind]));
			if (genTypes.Options.HasNasmFormatter)
				serializers.Add(new RustFormatterTableSerializer(Path.Combine(basePath, "nasm", "fmt_data.rs"), genTypes.GetObject<Nasm.CtorInfos>(TypeIds.NasmCtorInfos).Infos, genTypes[TypeIds.NasmCtorKind]));

			var stringsTable = new StringsTable();

			foreach (var serializer in serializers)
				serializer.Initialize(genTypes, stringsTable);

			stringsTable.Freeze();

			using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(Path.Combine(basePath, "strings_data.rs")))) {
				var serializer = new RustStringsTableSerializer(stringsTable);
				serializer.Serialize(writer);
			}

			foreach (var serializer in serializers) {
				using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(serializer.Filename)))
					serializer.Serialize(writer, stringsTable);
			}
		}
	}
}
