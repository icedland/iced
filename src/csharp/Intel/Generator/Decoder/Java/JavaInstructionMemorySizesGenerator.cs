// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using Generator.IO;
using Generator.Tables;

namespace Generator.Decoder.Java {
	[Generator(TargetLanguage.Java)]
	sealed class JavaInstructionMemorySizesGenerator {
		readonly IdentifierConverter idConverter;
		readonly GenTypes genTypes;

		public JavaInstructionMemorySizesGenerator(GeneratorContext generatorContext) {
			idConverter = JavaIdentifierConverter.Create();
			genTypes = generatorContext.Types;
		}

		public void Generate() {
			var icedConstants = genTypes.GetConstantsType(TypeIds.IcedConstants);
			var defs = genTypes.GetObject<InstructionDefs>(TypeIds.InstructionDefs).Defs;
			const string ClassName = "InstructionMemorySizes";
			using (var writer = new FileWriter(TargetLanguage.Java, FileUtils.OpenWrite(JavaConstants.GetFilename(genTypes, JavaConstants.IcedInternalPackage, ClassName + ".java")))) {
				writer.WriteFileHeader();
				writer.WriteLine($"package {JavaConstants.IcedInternalPackage};");
				writer.WriteLine();
				var memSizeEnum = genTypes[TypeIds.MemorySize];
				writer.WriteLine($"import com.github.icedland.iced.x86.{memSizeEnum.Name(idConverter)};");
				writer.WriteLine();
				writer.WriteLine($"public final class {ClassName} {{");
				using (writer.Indent()) {
					writer.WriteLine("public static final byte[] sizesNormal = new byte[] {");
					using (writer.Indent()) {
						foreach (var def in defs) {
							if (def.Memory.Value > byte.MaxValue)
								throw new InvalidOperationException();
							string value;
							if (def.Memory.Value == 0)
								value = "0";
							else
								value = $"(byte){idConverter.ToDeclTypeAndValue(def.Memory)}";
							writer.WriteLine($"{value},// {def.Code.Name(idConverter)}");
						}
					}
					writer.WriteLine("};");
					writer.WriteLine();
					writer.WriteLine($"public static final byte[] sizesBcst = new byte[] {{");
					using (writer.Indent()) {
						foreach (var def in defs) {
							if (def.MemoryBroadcast.Value > byte.MaxValue)
								throw new InvalidOperationException();
							string value;
							if (def.MemoryBroadcast.Value == 0)
								value = "0";
							else
								value = $"(byte){idConverter.ToDeclTypeAndValue(def.MemoryBroadcast)}";
							writer.WriteLine($"{value},// {def.Code.Name(idConverter)}");
						}
					}
					writer.WriteLine("};");
				}
				writer.WriteLine("}");
			}
		}
	}
}
