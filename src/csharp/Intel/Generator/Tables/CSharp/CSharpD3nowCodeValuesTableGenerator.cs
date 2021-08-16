// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System.Linq;
using Generator.Enums;
using Generator.IO;

namespace Generator.Tables.CSharp {
	[Generator(TargetLanguage.CSharp)]
	sealed class CSharpD3nowCodeValuesTableGenerator : D3nowCodeValuesTableGenerator {
		readonly IdentifierConverter idConverter;

		public CSharpD3nowCodeValuesTableGenerator(GeneratorContext generatorContext)
			: base(generatorContext.Types) => idConverter = CSharpIdentifierConverter.Create();

		protected override void Generate((int index, EnumValue enumValue)[] infos) {
			var filename = CSharpConstants.GetFilename(genTypes, CSharpConstants.DecoderNamespace, "OpCodeHandlers_D3NOW.cs");
			var updater = new FileUpdater(TargetLanguage.CSharp, "D3nowCodeValues", filename);
			updater.Generate(writer => WriteTable(writer, infos));
		}

		void WriteTable(FileWriter writer, (int index, EnumValue enumValue)[] infos) {
			foreach (var info in infos.OrderByDescending(a => a.index))
				writer.WriteLine($"result[0x{info.index:X2}] = {idConverter.ToDeclTypeAndValue(info.enumValue)};");
		}
	}
}
