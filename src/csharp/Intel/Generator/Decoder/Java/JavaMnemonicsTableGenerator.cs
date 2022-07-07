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
			var defs = genTypes.GetObject<InstructionDefs>(TypeIds.InstructionDefs).Defs;
			const string className = "MnemonicUtilsData";
			var filename = JavaConstants.GetResourceFilename(genTypes, JavaConstants.IcedPackage, className + ".bin");
			using (var writer = new BinaryByteTableWriter(filename)) {
				foreach (var def in defs) {
					if (def.Mnemonic.Value > ushort.MaxValue)
						throw new InvalidOperationException();
					writer.WriteUInt16((ushort)def.Mnemonic.Value);
				}
			}
		}
	}
}
