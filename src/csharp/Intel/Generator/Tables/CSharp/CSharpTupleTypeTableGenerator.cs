// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

using System;
using Generator.IO;

namespace Generator.Tables.CSharp {
	[Generator(TargetLanguage.CSharp)]
	sealed class CSharpTupleTypeTableGenerator {
		readonly IdentifierConverter idConverter;
		readonly GenTypes genTypes;

		public CSharpTupleTypeTableGenerator(GeneratorContext generatorContext) {
			idConverter = CSharpIdentifierConverter.Create();
			genTypes = generatorContext.Types;
		}

		public void Generate() {
			var infos = genTypes.GetObject<TupleTypeTable>(TypeIds.TupleTypeTable).Data;
			var filename = CSharpConstants.GetFilename(genTypes, CSharpConstants.IcedNamespace, "TupleTypeTable.cs");
			var updater = new FileUpdater(TargetLanguage.CSharp, "TupleTypeTable", filename);
			updater.Generate(writer => WriteTable(writer, infos));
		}

		void WriteTable(FileWriter writer, TupleTypeInfo[] infos) {
			var tupleTypeName = genTypes[TypeIds.TupleType].Name(idConverter);
			foreach (var info in infos) {
				writer.WriteCommentLine($"{tupleTypeName}.{info.Value.Name(idConverter)}");
				if (info.N > byte.MaxValue)
					throw new InvalidOperationException();
				if (info.Nbcst > byte.MaxValue)
					throw new InvalidOperationException();
				writer.Write($"0x{info.N:X2},");
				writer.WriteCommentLine("N");
				writer.Write($"0x{info.Nbcst:X2},");
				writer.WriteCommentLine("Nbcst");
			}
		}
	}
}
