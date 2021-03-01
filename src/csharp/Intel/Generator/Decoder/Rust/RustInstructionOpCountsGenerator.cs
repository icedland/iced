// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using Generator.Constants;
using Generator.IO;
using Generator.Tables;

namespace Generator.Decoder.Rust {
	[Generator(TargetLanguage.Rust)]
	sealed class RustInstructionOpCountsGenerator {
		readonly IdentifierConverter idConverter;
		readonly GeneratorContext generatorContext;

		public RustInstructionOpCountsGenerator(GeneratorContext generatorContext) {
			idConverter = RustIdentifierConverter.Create();
			this.generatorContext = generatorContext;
		}

		public void Generate() {
			var genTypes = generatorContext.Types;
			var icedConstants = genTypes.GetConstantsType(TypeIds.IcedConstants);
			var defs = genTypes.GetObject<InstructionDefs>(TypeIds.InstructionDefs).Defs;
			using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(generatorContext.Types.Dirs.GetRustFilename("instruction_op_counts.rs")))) {
				writer.WriteFileHeader();
				writer.WriteLine($"use crate::iced_constants::{icedConstants.Name(idConverter)};");
				writer.WriteLine();
				writer.WriteLine(RustConstants.AttributeNoRustFmt);
				writer.WriteLine($"pub(super) static OP_COUNT: [u8; {icedConstants.Name(idConverter)}::{icedConstants[IcedConstants.GetEnumCountName(TypeIds.Code)].Name(idConverter)}] = [");
				using (writer.Indent()) {
					foreach (var def in defs)
						writer.WriteLine($"{def.OpCount},// {def.Code.Name(idConverter)}");
				}
				writer.WriteLine("];");
			}
		}
	}
}
