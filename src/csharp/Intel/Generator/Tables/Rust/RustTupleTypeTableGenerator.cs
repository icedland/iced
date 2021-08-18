// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using Generator.IO;

namespace Generator.Tables.Rust {
	[Generator(TargetLanguage.Rust)]
	sealed class RustTupleTypeTableGenerator {
		readonly IdentifierConverter idConverter;
		readonly GeneratorContext generatorContext;

		public RustTupleTypeTableGenerator(GeneratorContext generatorContext) {
			idConverter = RustIdentifierConverter.Create();
			this.generatorContext = generatorContext;
		}

		public void Generate() {
			var infos = generatorContext.Types.GetObject<TupleTypeTable>(TypeIds.TupleTypeTable).Data;
			var filename = generatorContext.Types.Dirs.GetRustFilename("tuple_type_tbl.rs");
			var updater = new FileUpdater(TargetLanguage.Rust, "TupleTypeTable", filename);
			updater.Generate(writer => WriteTable(writer, infos));
		}

		void WriteTable(FileWriter writer, TupleTypeInfo[] infos) {
			writer.WriteLine(RustConstants.AttributeNoRustFmt);
			writer.WriteLine($"static TUPLE_TYPE_TBL: [(u8, u8); {infos.Length}] = [");
			using (writer.Indent()) {
				foreach (var info in infos) {
					writer.WriteCommentLine(idConverter.ToDeclTypeAndValue(info.Value));
					if (info.N > byte.MaxValue)
						throw new InvalidOperationException();
					if (info.Nbcst > byte.MaxValue)
						throw new InvalidOperationException();
					writer.Write($"(0x{info.N:X2},");
					writer.WriteCommentLine("N");
					writer.Write($"0x{info.Nbcst:X2}),");
					writer.WriteCommentLine("Nbcst");
				}
			}
			writer.WriteLine("];");
		}
	}
}
