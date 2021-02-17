// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System.Collections.Generic;
using Generator.IO;

namespace Generator.Formatters.Rust {
	[Generator(TargetLanguage.Rust)]
	sealed class RustFormatterTableGenerator {
		readonly GenTypes genTypes;

		public RustFormatterTableGenerator(GeneratorContext generatorContext) =>
			genTypes = generatorContext.Types;

		public void Generate() {
			var serializers = new List<IFormatterTableSerializer>();
			if (genTypes.Options.HasGasFormatter)
				serializers.Add(new RustFormatterTableSerializer(genTypes.Dirs.GetRustFilename("formatter", "gas", "fmt_data.rs"), genTypes.GetObject<Gas.CtorInfos>(TypeIds.GasCtorInfos).Infos, genTypes[TypeIds.GasCtorKind]));
			if (genTypes.Options.HasIntelFormatter)
				serializers.Add(new RustFormatterTableSerializer(genTypes.Dirs.GetRustFilename("formatter", "intel", "fmt_data.rs"), genTypes.GetObject<Intel.CtorInfos>(TypeIds.IntelCtorInfos).Infos, genTypes[TypeIds.IntelCtorKind]));
			if (genTypes.Options.HasMasmFormatter)
				serializers.Add(new RustFormatterTableSerializer(genTypes.Dirs.GetRustFilename("formatter", "masm", "fmt_data.rs"), genTypes.GetObject<Masm.CtorInfos>(TypeIds.MasmCtorInfos).Infos, genTypes[TypeIds.MasmCtorKind]));
			if (genTypes.Options.HasNasmFormatter)
				serializers.Add(new RustFormatterTableSerializer(genTypes.Dirs.GetRustFilename("formatter", "nasm", "fmt_data.rs"), genTypes.GetObject<Nasm.CtorInfos>(TypeIds.NasmCtorInfos).Infos, genTypes[TypeIds.NasmCtorKind]));
			if (genTypes.Options.HasFastFormatter)
				serializers.Add(new RustFastFormatterTableSerializer(genTypes.Dirs.GetRustFilename("formatter", "fast", "fmt_data.rs"), genTypes.GetObject<Fast.FmtTblInfos>(TypeIds.FastFmtTblInfos).Infos));

			var stringsTable = new StringsTable();

			foreach (var serializer in serializers)
				serializer.Initialize(genTypes, stringsTable);

			stringsTable.Freeze();

			using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(genTypes.Dirs.GetRustFilename("formatter", "strings_data.rs")))) {
				var serializer = new RustStringsTableSerializer(stringsTable);
				serializer.Serialize(writer);
			}

			foreach (var serializer in serializers) {
				using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(serializer.GetFilename(genTypes))))
					serializer.Serialize(genTypes, writer, stringsTable);
			}
		}
	}
}
