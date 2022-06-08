// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using Generator.IO;
using Generator.Tables;

namespace Generator.Decoder.Java {
	[Generator(TargetLanguage.Java)]
	sealed class JavaMnemonicsTableGenerator {
		readonly IdentifierConverter idConverter;
		readonly GenTypes genTypes;

		public JavaMnemonicsTableGenerator(GeneratorContext generatorContext) {
			idConverter = JavaIdentifierConverter.Create();
			genTypes = generatorContext.Types;
		}

		public void Generate() {
			var icedConstants = genTypes.GetConstantsType(TypeIds.IcedConstants);
			var defs = genTypes.GetObject<InstructionDefs>(TypeIds.InstructionDefs).Defs;
			const string ClassName = "MnemonicUtilsData";
			using (var writer = new FileWriter(TargetLanguage.Java, FileUtils.OpenWrite(JavaConstants.GetFilename(genTypes, JavaConstants.IcedInternalPackage, ClassName + ".java")))) {
				writer.WriteFileHeader();
				writer.WriteLine($"package {JavaConstants.IcedInternalPackage};");
				writer.WriteLine();
				var mnemonicEnum = genTypes[TypeIds.Mnemonic];
				writer.WriteLine($"import com.github.icedland.iced.x86.{mnemonicEnum.Name(idConverter)};");
				writer.WriteLine();
				writer.WriteLine($"/** {JavaConstants.InternalDoc} */");
				writer.WriteLine($"public final class {ClassName} {{");
				using (writer.Indent()) {
					writer.WriteLine($"/** {JavaConstants.InternalDoc} */");
					writer.WriteLine("public static final short[] toMnemonic = new short[] {");
					using (writer.Indent()) {
						foreach (var def in defs) {
							if (def.Mnemonic.Value > ushort.MaxValue)
								throw new InvalidOperationException();
							writer.WriteLine($"(short){idConverter.ToDeclTypeAndValue(def.Mnemonic)},// {def.Code.Name(idConverter)}");
						}
					}
					writer.WriteLine("};");
				}
				writer.WriteLine("}");
			}
		}
	}
}
