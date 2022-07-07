// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System.Linq;
using Generator.Enums;
using Generator.IO;

namespace Generator.Tables.Java {
	[Generator(TargetLanguage.Java)]
	sealed class JavaD3nowCodeValuesTableGenerator : D3nowCodeValuesTableGenerator {
		readonly IdentifierConverter idConverter;

		public JavaD3nowCodeValuesTableGenerator(GeneratorContext generatorContext)
			: base(generatorContext.Types) => idConverter = JavaIdentifierConverter.Create();

		protected override void Generate((int index, EnumValue enumValue)[] infos) {
			var filename = JavaConstants.GetFilename(genTypes, JavaConstants.DecoderPackage, "OpCodeHandlers_D3NOW.java");
			var updater = new FileUpdater(TargetLanguage.Java, "D3nowCodeValues", filename);
			updater.Generate(writer => WriteTable(writer, infos));
		}

		void WriteTable(FileWriter writer, (int index, EnumValue enumValue)[] infos) {
			foreach (var info in infos.OrderByDescending(a => a.index))
				writer.WriteLine($"result[0x{info.index:X2}] = {idConverter.ToDeclTypeAndValue(info.enumValue)};");
		}
	}
}
