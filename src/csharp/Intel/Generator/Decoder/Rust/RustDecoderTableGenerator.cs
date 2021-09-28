// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using Generator.IO;

namespace Generator.Decoder.Rust {
	[Generator(TargetLanguage.Rust)]
	sealed class RustDecoderTableGenerator {
		readonly GeneratorContext generatorContext;

		public RustDecoderTableGenerator(GeneratorContext generatorContext) => this.generatorContext = generatorContext;

		public void Generate() {
			var genTypes = generatorContext.Types;
			var serializers = new RustDecoderTableSerializer[] {
				new RustDecoderTableSerializer(genTypes, "legacy", DecoderTableSerializerInfo.Legacy(genTypes)),
				new RustDecoderTableSerializer(genTypes, "vex", DecoderTableSerializerInfo.Vex(genTypes)),
				new RustDecoderTableSerializer(genTypes, "evex", DecoderTableSerializerInfo.Evex(genTypes)),
				new RustDecoderTableSerializer(genTypes, "xop", DecoderTableSerializerInfo.Xop(genTypes)),
				new RustDecoderTableSerializer(genTypes, "mvex", DecoderTableSerializerInfo.Mvex(genTypes)),
			};

			foreach (var serializer in serializers) {
				var filename = generatorContext.Types.Dirs.GetRustFilename("decoder", "table_de", $"data_{serializer.TableName}.rs");
				using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(filename)))
					serializer.Serialize(writer);
			}
		}
	}
}
