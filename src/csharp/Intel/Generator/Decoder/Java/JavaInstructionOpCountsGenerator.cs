// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using Generator.IO;
using Generator.Tables;

namespace Generator.Decoder.Java {
	[Generator(TargetLanguage.Java)]
	sealed class JavaInstructionOpCountsGenerator {
		readonly IdentifierConverter idConverter;
		readonly GenTypes genTypes;

		public JavaInstructionOpCountsGenerator(GeneratorContext generatorContext) {
			idConverter = JavaIdentifierConverter.Create();
			genTypes = generatorContext.Types;
		}

		public void Generate() {
			var icedConstants = genTypes.GetConstantsType(TypeIds.IcedConstants);
			var defs = genTypes.GetObject<InstructionDefs>(TypeIds.InstructionDefs).Defs;
			const string ClassName = "InstructionOpCounts";
			using (var writer = new FileWriter(TargetLanguage.Java, FileUtils.OpenWrite(JavaConstants.GetFilename(genTypes, JavaConstants.IcedInternalPackage, ClassName + ".java")))) {
				writer.WriteFileHeader();
				writer.WriteLine($"package {JavaConstants.IcedInternalPackage};");
				writer.WriteLine();
				writer.WriteLine($"/** {JavaConstants.InternalDoc} */");
				writer.WriteLine($"public final class {ClassName} {{");
				using (writer.Indent()) {
					writer.WriteLine($"/** {JavaConstants.InternalDoc} */");
					writer.WriteLine("public static final byte[] opCount = new byte[] {");
					using (writer.Indent()) {
						foreach (var def in defs)
							writer.WriteLine($"{def.OpCount},// {def.Code.Name(idConverter)}");
					}
					writer.WriteLine("};");
				}
				writer.WriteLine("}");
			}
		}
	}
}
