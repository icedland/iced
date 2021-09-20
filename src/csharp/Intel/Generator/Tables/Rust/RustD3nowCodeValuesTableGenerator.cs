// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using Generator.Enums;
using Generator.IO;

namespace Generator.Tables.Rust {
	[Generator(TargetLanguage.Rust)]
	sealed class RustD3nowCodeValuesTableGenerator : D3nowCodeValuesTableGenerator {
		readonly GeneratorContext generatorContext;
		readonly IdentifierConverter idConverter;

		public RustD3nowCodeValuesTableGenerator(GeneratorContext generatorContext)
			: base(generatorContext.Types) {
			this.generatorContext = generatorContext;
			idConverter = RustIdentifierConverter.Create();
		}

		protected override void Generate((int index, EnumValue enumValue)[] infos) {
			var filename = generatorContext.Types.Dirs.GetRustFilename("decoder", "handlers", "d3now.rs");
			var updater = new FileUpdater(TargetLanguage.Rust, "D3nowCodeValues", filename);
			updater.Generate(writer => WriteTable(writer, infos));
		}

		void WriteTable(FileWriter writer, (int index, EnumValue enumValue)[] infos) {
			var values = new EnumValue?[0x100];
			foreach (var info in infos) {
				if (values[info.index] is not null)
					throw new InvalidOperationException();
				values[info.index] = info.enumValue;
			}
			var invalid = genTypes[TypeIds.Code][nameof(Code.INVALID)];
			foreach (var value in values) {
				var enumValue = value ?? invalid;
				writer.WriteLine($"{idConverter.ToDeclTypeAndValue(enumValue)},");
			}
		}
	}
}
