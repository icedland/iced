// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using Generator.IO;

namespace Generator.Formatters.Java {
	[Generator(TargetLanguage.Java)]
	sealed class JavaFormatterTableGenerator {
		readonly GenTypes genTypes;

		public JavaFormatterTableGenerator(GeneratorContext generatorContext) =>
			genTypes = generatorContext.Types;

		public void Generate() {
			var serializers = new List<(IFormatterTableSerializer serializer, Action<IFormatterTableSerializer, GenTypes, BinaryByteTableWriter, StringsTable> serialize)>();
			if (genTypes.Options.HasGasFormatter)
				serializers.Add((new JavaFormatterTableSerializer(genTypes.GetObject<Gas.CtorInfos>(TypeIds.GasCtorInfos).Infos, genTypes[TypeIds.GasCtorKind], JavaConstants.GasFormatterPackage), (obj, genTypes, writer, stringsTable) => ((JavaFormatterTableSerializer)obj).Serialize(genTypes, writer, stringsTable)));
			if (genTypes.Options.HasIntelFormatter)
				serializers.Add((new JavaFormatterTableSerializer(genTypes.GetObject<Intel.CtorInfos>(TypeIds.IntelCtorInfos).Infos, genTypes[TypeIds.IntelCtorKind], JavaConstants.IntelFormatterPackage), (obj, genTypes, writer, stringsTable) => ((JavaFormatterTableSerializer)obj).Serialize(genTypes, writer, stringsTable)));
			if (genTypes.Options.HasMasmFormatter)
				serializers.Add((new JavaFormatterTableSerializer(genTypes.GetObject<Masm.CtorInfos>(TypeIds.MasmCtorInfos).Infos, genTypes[TypeIds.MasmCtorKind], JavaConstants.MasmFormatterPackage), (obj, genTypes, writer, stringsTable) => ((JavaFormatterTableSerializer)obj).Serialize(genTypes, writer, stringsTable)));
			if (genTypes.Options.HasNasmFormatter)
				serializers.Add((new JavaFormatterTableSerializer(genTypes.GetObject<Nasm.CtorInfos>(TypeIds.NasmCtorInfos).Infos, genTypes[TypeIds.NasmCtorKind], JavaConstants.NasmFormatterPackage), (obj, genTypes, writer, stringsTable) => ((JavaFormatterTableSerializer)obj).Serialize(genTypes, writer, stringsTable)));
			if (genTypes.Options.HasFastFormatter)
				serializers.Add((new JavaFastFormatterTableSerializer(genTypes.GetObject<Fast.FmtTblInfos>(TypeIds.FastFmtTblInfos).Infos, JavaConstants.FastFormatterPackage), (obj, genTypes, writer, stringsTable) => ((JavaFastFormatterTableSerializer)obj).Serialize(genTypes, writer, stringsTable)));

			var stringsTable = new StringsTable();

			foreach (var info in serializers)
				info.serializer.Initialize(genTypes, stringsTable);

			stringsTable.Freeze();

			var serializer = new JavaStringsTableSerializer();
			serializer.Serialize(genTypes, stringsTable);

			foreach (var info in serializers) {
				using (var writer = new BinaryByteTableWriter(info.serializer.GetFilename(genTypes)))
					info.serialize(info.serializer, genTypes, writer, stringsTable);
			}
		}
	}
}
