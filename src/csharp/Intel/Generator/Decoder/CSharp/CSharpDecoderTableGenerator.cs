// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using Generator.IO;

namespace Generator.Decoder.CSharp {
	[Generator(TargetLanguage.CSharp)]
	sealed class CSharpDecoderTableGenerator {
		readonly GenTypes genTypes;

		public CSharpDecoderTableGenerator(GeneratorContext generatorContext) => genTypes = generatorContext.Types;

		public void Generate() {
			var serializers = new CSharpDecoderTableSerializer[] {
				new CSharpDecoderTableSerializer(genTypes, "OpCodeHandlersTables_Legacy", DecoderTableSerializerInfo.Legacy(genTypes)),
				new CSharpDecoderTableSerializer(genTypes, "OpCodeHandlersTables_VEX", DecoderTableSerializerInfo.Vex(genTypes)),
				new CSharpDecoderTableSerializer(genTypes, "OpCodeHandlersTables_EVEX", DecoderTableSerializerInfo.Evex(genTypes)),
				new CSharpDecoderTableSerializer(genTypes, "OpCodeHandlersTables_XOP", DecoderTableSerializerInfo.Xop(genTypes)),
				new CSharpDecoderTableSerializer(genTypes, "OpCodeHandlersTables_MVEX", DecoderTableSerializerInfo.Mvex(genTypes)),
			};

			foreach (var serializer in serializers) {
				var filename = CSharpConstants.GetFilename(genTypes, CSharpConstants.DecoderNamespace, serializer.ClassName + ".g.cs");
				using (var writer = new FileWriter(TargetLanguage.CSharp, FileUtils.OpenWrite(filename)))
					serializer.Serialize(writer);
			}
		}
	}
}
