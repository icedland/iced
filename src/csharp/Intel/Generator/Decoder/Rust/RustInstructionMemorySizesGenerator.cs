// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using Generator.Constants;
using Generator.IO;
using Generator.Tables;

namespace Generator.Decoder.Rust {
	[Generator(TargetLanguage.Rust)]
	sealed class RustInstructionMemorySizesGenerator {
		readonly IdentifierConverter idConverter;
		readonly GeneratorContext generatorContext;

		public RustInstructionMemorySizesGenerator(GeneratorContext generatorContext) {
			idConverter = RustIdentifierConverter.Create();
			this.generatorContext = generatorContext;
		}

		public void Generate() {
			var genTypes = generatorContext.Types;
			var icedConstants = genTypes.GetConstantsType(TypeIds.IcedConstants);
			var defs = genTypes.GetObject<InstructionDefs>(TypeIds.InstructionDefs).Defs;
			var memSizeName = genTypes[TypeIds.MemorySize].Name(idConverter);
			using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(generatorContext.Types.Dirs.GetRustFilename("instruction_memory_sizes.rs")))) {
				writer.WriteFileHeader();
				writer.WriteLine($"use crate::iced_constants::{icedConstants.Name(idConverter)};");
				writer.WriteLine($"use crate::{genTypes[TypeIds.MemorySize].Name(idConverter)};");
				writer.WriteLine();
				writer.WriteLine("// 0 = memory size");
				writer.WriteLine("// 1 = broadcast memory size");
				writer.WriteLine(RustConstants.AttributeNoRustFmt);
				writer.WriteLine($"pub(super) static SIZES: [{memSizeName}; {icedConstants.Name(idConverter)}::{icedConstants[IcedConstants.GetEnumCountName(TypeIds.Code)].Name(idConverter)} * 2] = [");
				using (writer.Indent()) {
					foreach (var def in defs) {
						if (def.Memory.Value > byte.MaxValue)
							throw new InvalidOperationException();
						string value = $"{memSizeName}::{def.Memory.Name(idConverter)}";
						writer.WriteLine($"{value},// {def.Code.Name(idConverter)}");
					}
					foreach (var def in defs) {
						if (def.MemoryBroadcast.Value > byte.MaxValue)
							throw new InvalidOperationException();
						string value = $"{memSizeName}::{def.MemoryBroadcast.Name(idConverter)}";
						writer.WriteLine($"{value},// {def.Code.Name(idConverter)}");
					}
				}
				writer.WriteLine("];");
			}
		}
	}
}
