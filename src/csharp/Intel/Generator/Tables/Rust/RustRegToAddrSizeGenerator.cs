// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using Generator.Constants;
using Generator.Enums;
using Generator.IO;

namespace Generator.Tables.Rust {
	[Generator(TargetLanguage.Rust)]
	sealed class RustRegToAddrSizeGenerator {
		readonly GenTypes genTypes;
		readonly IdentifierConverter idConverter;

		public RustRegToAddrSizeGenerator(GeneratorContext generatorContext) {
			genTypes = generatorContext.Types;
			idConverter = RustIdentifierConverter.Create();
		}

		public void Generate() {
			var filename = genTypes.Dirs.GetRustFilename("instruction_internal.rs");
			new FileUpdater(TargetLanguage.Rust, "RegToAddrSize", filename).Generate(writer => {
				var registerType = genTypes[TypeIds.Register];

				var icedConstantsName = genTypes.GetConstantsType(TypeIds.IcedConstants).Name(idConverter);
				var regCountName = idConverter.Constant(IcedConstants.GetEnumCountName(TypeIds.Register));
				writer.WriteLine(RustConstants.AttributeNoRustFmt);
				writer.WriteLine($"static REG_TO_ADDR_SIZE: [u8; {icedConstantsName}::{regCountName}] = [");
				using (writer.Indent()) {
					foreach (var regEnum in registerType.Values) {
						var reg = (Register)regEnum.Value;
						var size = GetAddrSize(reg);
						if (size > byte.MaxValue)
							throw new InvalidOperationException();
						writer.WriteLine($"{size}, // {regEnum.Name(idConverter)}");
					}
				}
				writer.WriteLine("];");
			});
		}

		static uint GetAddrSize(Register reg) {
			if (reg >= Register.AX && reg <= Register.R15W)
				return 2;
			if (reg >= Register.EAX && reg <= Register.R15D)
				return 4;
			if (reg >= Register.RAX && reg <= Register.R15)
				return 8;
			if (reg == Register.EIP)
				return 4;
			if (reg == Register.RIP)
				return 8;
			return 0;
		}
	}
}
