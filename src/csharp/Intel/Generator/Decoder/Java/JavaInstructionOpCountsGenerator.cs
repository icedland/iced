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
			var defs = genTypes.GetObject<InstructionDefs>(TypeIds.InstructionDefs).Defs;
			const string className = "InstructionOpCounts";
			var filename = JavaConstants.GetResourceFilename(genTypes, JavaConstants.IcedPackage, className + ".bin");
			using (var writer = new BinaryByteTableWriter(filename)) {
				foreach (var def in defs)
					writer.WriteByte(checked((byte)def.OpCount));
			}
		}
	}
}
