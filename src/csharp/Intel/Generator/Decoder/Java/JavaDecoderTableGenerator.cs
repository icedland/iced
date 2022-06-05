// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using Generator.IO;

namespace Generator.Decoder.Java {
	[Generator(TargetLanguage.Java)]
	sealed class JavaDecoderTableGenerator {
		readonly GenTypes genTypes;

		public JavaDecoderTableGenerator(GeneratorContext generatorContext) => genTypes = generatorContext.Types;

		public void Generate() {
			var serializers = new JavaDecoderTableSerializer[] {
				new JavaDecoderTableSerializer(genTypes, "OpCodeHandlersData_Legacy", DecoderTableSerializerInfo.Legacy(genTypes)),
				new JavaDecoderTableSerializer(genTypes, "OpCodeHandlersData_VEX", DecoderTableSerializerInfo.Vex(genTypes)),
				new JavaDecoderTableSerializer(genTypes, "OpCodeHandlersData_EVEX", DecoderTableSerializerInfo.Evex(genTypes)),
				new JavaDecoderTableSerializer(genTypes, "OpCodeHandlersData_XOP", DecoderTableSerializerInfo.Xop(genTypes)),
				new JavaDecoderTableSerializer(genTypes, "OpCodeHandlersData_MVEX", DecoderTableSerializerInfo.Mvex(genTypes)),
			};

			foreach (var serializer in serializers) {
				var rsrcFilename = JavaConstants.GetResourceFilename(genTypes, JavaConstants.DecoderPackage, serializer.ClassName + ".bin");
				var srcFilename = JavaConstants.GetFilename(genTypes, JavaConstants.DecoderPackage, serializer.ClassName + ".java");
				using (var srcWriter = new FileWriter(TargetLanguage.Java, FileUtils.OpenWrite(srcFilename)))
				using (var binWriter = new BinaryByteTableWriter(rsrcFilename)) {
					srcWriter.WriteFileHeader();
					srcWriter.WriteLine($"package {JavaConstants.DecoderPackage};");
					srcWriter.WriteLine();
					srcWriter.WriteLine($"final class {serializer.ClassName} {{");
					using (srcWriter.Indent()) {
						srcWriter.WriteLine($"private {serializer.ClassName}() {{}}");
						srcWriter.WriteLine();
						serializer.Serialize(binWriter, srcWriter);
					}
					srcWriter.WriteLine("}");
				}
			}
		}
	}
}
