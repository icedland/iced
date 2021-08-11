// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using Generator.Constants;
using Generator.IO;
using Generator.Tables;

namespace Generator.Decoder.Rust {
	[Generator(TargetLanguage.Rust)]
	sealed class RustMnemonicsTableGenerator {
		readonly IdentifierConverter idConverter;
		readonly GeneratorContext generatorContext;

		public RustMnemonicsTableGenerator(GeneratorContext generatorContext) {
			idConverter = RustIdentifierConverter.Create();
			this.generatorContext = generatorContext;
		}

		public void Generate() {
			var genTypes = generatorContext.Types;
			var icedConstants = genTypes.GetConstantsType(TypeIds.IcedConstants);
			var defs = genTypes.GetObject<InstructionDefs>(TypeIds.InstructionDefs).Defs;
			var mnemonicName = genTypes[TypeIds.Mnemonic].Name(idConverter);
			using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(generatorContext.Types.Dirs.GetRustFilename("mnemonics.rs")))) {
				writer.WriteFileHeader();

				writer.WriteLine($"use crate::iced_constants::{icedConstants.Name(idConverter)};");
				writer.WriteLine($"use crate::{genTypes[TypeIds.Mnemonic].Name(idConverter)};");
				writer.WriteLine();
				writer.WriteLine(RustConstants.AttributeNoRustFmt);
				writer.WriteLine($"pub(super) static TO_MNEMONIC: [{mnemonicName}; {icedConstants.Name(idConverter)}::{icedConstants[IcedConstants.GetEnumCountName(TypeIds.Code)].Name(idConverter)}] = [");
				using (writer.Indent()) {
					foreach (var def in defs) {
						if (def.Mnemonic.Value > ushort.MaxValue)
							throw new InvalidOperationException();
						writer.WriteLine($"{idConverter.ToDeclTypeAndValue(def.Mnemonic)},// {def.Code.Name(idConverter)}");
					}
				}
				writer.WriteLine("];");
			}
		}
	}
}
