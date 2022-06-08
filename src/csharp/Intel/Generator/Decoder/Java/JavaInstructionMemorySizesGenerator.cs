// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System.Linq;
using Generator.Enums;
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
			var defs = genTypes.GetObject<InstructionDefs>(TypeIds.InstructionDefs).Defs;
			const string className = "InstructionMemorySizes";
			var data = new (string name, EnumValue[] values)[] {
				("sizesNormal", defs.Select(a => a.Memory).ToArray()),
				("sizesBcst", defs.Select(a => a.MemoryBroadcast).ToArray()),
			};
			foreach (var (name, values) in data) {
				var filename = JavaConstants.GetResourceFilename(genTypes, JavaConstants.IcedPackage, className + "." + name + ".bin");
				using (var writer = new BinaryByteTableWriter(filename)) {
					foreach (var value in values) {
						writer.WriteByte(checked((byte)value.Value));
					}
				}
			}
		}
	}
}
