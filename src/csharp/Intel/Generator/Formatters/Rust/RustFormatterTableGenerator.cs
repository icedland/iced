// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using Generator.IO;

namespace Generator.Formatters.Rust {
	[Generator(TargetLanguage.Rust)]
	sealed class RustFormatterTableGenerator {
		readonly GenTypes genTypes;

		public RustFormatterTableGenerator(GeneratorContext generatorContext) =>
			genTypes = generatorContext.Types;

		public void Generate() {
			var serializers = new List<(IFormatterTableSerializer serializer, Action<IFormatterTableSerializer, GenTypes, FileWriter, StringsTable> serialize)>();
			if (genTypes.Options.HasGasFormatter)
				serializers.Add((new RustFormatterTableSerializer(genTypes.Dirs.GetRustFilename("formatter", "gas", "fmt_data.rs"), genTypes.GetObject<Gas.CtorInfos>(TypeIds.GasCtorInfos).Infos, genTypes[TypeIds.GasCtorKind]), (obj, genTypes, writer, stringsTable) => ((RustFormatterTableSerializer)obj).Serialize(genTypes, writer, stringsTable)));
			if (genTypes.Options.HasIntelFormatter)
				serializers.Add((new RustFormatterTableSerializer(genTypes.Dirs.GetRustFilename("formatter", "intel", "fmt_data.rs"), genTypes.GetObject<Intel.CtorInfos>(TypeIds.IntelCtorInfos).Infos, genTypes[TypeIds.IntelCtorKind]), (obj, genTypes, writer, stringsTable) => ((RustFormatterTableSerializer)obj).Serialize(genTypes, writer, stringsTable)));
			if (genTypes.Options.HasMasmFormatter)
				serializers.Add((new RustFormatterTableSerializer(genTypes.Dirs.GetRustFilename("formatter", "masm", "fmt_data.rs"), genTypes.GetObject<Masm.CtorInfos>(TypeIds.MasmCtorInfos).Infos, genTypes[TypeIds.MasmCtorKind]), (obj, genTypes, writer, stringsTable) => ((RustFormatterTableSerializer)obj).Serialize(genTypes, writer, stringsTable)));
			if (genTypes.Options.HasNasmFormatter)
				serializers.Add((new RustFormatterTableSerializer(genTypes.Dirs.GetRustFilename("formatter", "nasm", "fmt_data.rs"), genTypes.GetObject<Nasm.CtorInfos>(TypeIds.NasmCtorInfos).Infos, genTypes[TypeIds.NasmCtorKind]), (obj, genTypes, writer, stringsTable) => ((RustFormatterTableSerializer)obj).Serialize(genTypes, writer, stringsTable)));
			if (genTypes.Options.HasFastFormatter)
				serializers.Add((new RustFastFormatterTableSerializer(genTypes.Dirs.GetRustFilename("formatter", "fast", "fmt_data.rs"), genTypes.GetObject<Fast.FmtTblInfos>(TypeIds.FastFmtTblInfos).Infos), (obj, genTypes, writer, stringsTable) => ((RustFastFormatterTableSerializer)obj).Serialize(genTypes, writer, stringsTable)));

			var stringsTable = new StringsTable();

			foreach (var info in serializers)
				info.serializer.Initialize(genTypes, stringsTable);

			stringsTable.Freeze();

			using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(genTypes.Dirs.GetRustFilename("formatter", "strings_data.rs")))) {
				var serializer = new RustStringsTableSerializer(stringsTable);
				serializer.Serialize(writer);
			}

			foreach (var info in serializers) {
				using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(info.serializer.GetFilename(genTypes))))
					info.serialize(info.serializer, genTypes, writer, stringsTable);
			}
		}
	}
}
