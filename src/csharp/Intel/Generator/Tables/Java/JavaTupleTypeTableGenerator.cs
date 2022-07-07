// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using Generator.IO;

namespace Generator.Tables.Java {
	[Generator(TargetLanguage.Java)]
	sealed class JavaTupleTypeTableGenerator {
		readonly IdentifierConverter idConverter;
		readonly GenTypes genTypes;

		public JavaTupleTypeTableGenerator(GeneratorContext generatorContext) {
			idConverter = JavaIdentifierConverter.Create();
			genTypes = generatorContext.Types;
		}

		public void Generate() {
			var infos = genTypes.GetObject<TupleTypeTable>(TypeIds.TupleTypeTable).Data;
			var filename = JavaConstants.GetFilename(genTypes, JavaConstants.IcedInternalPackage, "TupleTypeTable.java");
			var updater = new FileUpdater(TargetLanguage.Java, "TupleTypeTable", filename);
			updater.Generate(writer => WriteTable(writer, infos));
		}

		void WriteTable(FileWriter writer, TupleTypeInfo[] infos) {
			foreach (var info in infos) {
				writer.WriteCommentLine(idConverter.ToDeclTypeAndValue(info.Value));
				if (info.N > byte.MaxValue)
					throw new InvalidOperationException();
				if (info.Nbcst > byte.MaxValue)
					throw new InvalidOperationException();
				writer.Write($"(byte)0x{info.N:X2},");
				writer.WriteCommentLine("N");
				writer.Write($"(byte)0x{info.Nbcst:X2},");
				writer.WriteCommentLine("Nbcst");
			}
		}
	}
}
