// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using Generator.IO;

namespace Generator.Formatters.CSharp {
	[Generator(TargetLanguage.CSharp)]
	sealed class CSharpFormatterTableGenerator {
		readonly GenTypes genTypes;

		public CSharpFormatterTableGenerator(GeneratorContext generatorContext) =>
			genTypes = generatorContext.Types;

		public void Generate() {
			var serializers = new List<(IFormatterTableSerializer serializer, Action<IFormatterTableSerializer, GenTypes, FileWriter, StringsTable> serialize)>();
			if (genTypes.Options.HasGasFormatter)
				serializers.Add((new CSharpFormatterTableSerializer(genTypes.GetObject<Gas.CtorInfos>(TypeIds.GasCtorInfos).Infos, genTypes[TypeIds.GasCtorKind], CSharpConstants.GasFormatterDefine, CSharpConstants.GasFormatterNamespace), (obj, genTypes, writer, stringsTable) => ((CSharpFormatterTableSerializer)obj).Serialize(genTypes, writer, stringsTable)));
			if (genTypes.Options.HasIntelFormatter)
				serializers.Add((new CSharpFormatterTableSerializer(genTypes.GetObject<Intel.CtorInfos>(TypeIds.IntelCtorInfos).Infos, genTypes[TypeIds.IntelCtorKind], CSharpConstants.IntelFormatterDefine, CSharpConstants.IntelFormatterNamespace), (obj, genTypes, writer, stringsTable) => ((CSharpFormatterTableSerializer)obj).Serialize(genTypes, writer, stringsTable)));
			if (genTypes.Options.HasMasmFormatter)
				serializers.Add((new CSharpFormatterTableSerializer(genTypes.GetObject<Masm.CtorInfos>(TypeIds.MasmCtorInfos).Infos, genTypes[TypeIds.MasmCtorKind], CSharpConstants.MasmFormatterDefine, CSharpConstants.MasmFormatterNamespace), (obj, genTypes, writer, stringsTable) => ((CSharpFormatterTableSerializer)obj).Serialize(genTypes, writer, stringsTable)));
			if (genTypes.Options.HasNasmFormatter)
				serializers.Add((new CSharpFormatterTableSerializer(genTypes.GetObject<Nasm.CtorInfos>(TypeIds.NasmCtorInfos).Infos, genTypes[TypeIds.NasmCtorKind], CSharpConstants.NasmFormatterDefine, CSharpConstants.NasmFormatterNamespace), (obj, genTypes, writer, stringsTable) => ((CSharpFormatterTableSerializer)obj).Serialize(genTypes, writer, stringsTable)));
			if (genTypes.Options.HasFastFormatter)
				serializers.Add((new CSharpFastFormatterTableSerializer(genTypes.GetObject<Fast.FmtTblInfos>(TypeIds.FastFmtTblInfos).Infos, CSharpConstants.FastFormatterDefine, CSharpConstants.FastFormatterNamespace), (obj, genTypes, writer, stringsTable) => ((CSharpFastFormatterTableSerializer)obj).Serialize(genTypes, writer, stringsTable)));

			var stringsTable = new StringsTable();

			foreach (var info in serializers)
				info.serializer.Initialize(genTypes, stringsTable);

			stringsTable.Freeze();

			const string FormatterStringsTableName = "FormatterStringsTable";
			using (var writer = new FileWriter(TargetLanguage.CSharp, FileUtils.OpenWrite(CSharpConstants.GetFilename(genTypes, CSharpConstants.FormatterNamespace, FormatterStringsTableName + ".g.cs")))) {
				var serializer = new CSharpStringsTableSerializer(stringsTable, CSharpConstants.FormatterNamespace, FormatterStringsTableName, CSharpConstants.AnyFormatterDefine);
				serializer.Serialize(writer);
			}

			foreach (var info in serializers) {
				using (var writer = new FileWriter(TargetLanguage.CSharp, FileUtils.OpenWrite(info.serializer.GetFilename(genTypes))))
					info.serialize(info.serializer, genTypes, writer, stringsTable);
			}
		}
	}
}
