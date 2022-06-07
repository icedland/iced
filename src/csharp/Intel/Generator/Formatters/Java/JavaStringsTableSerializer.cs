// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using Generator.IO;

namespace Generator.Formatters.Java {
	struct JavaStringsTableSerializer {
		public JavaStringsTableSerializer() {}

		public void Serialize(GenTypes genTypes, StringsTable stringsTable) {
			if (!stringsTable.IsFrozen)
				throw new InvalidOperationException();

			var sortedInfos = stringsTable.Infos;
			int maxStringLength = 0;
			foreach (var info in sortedInfos)
				maxStringLength = Math.Max(maxStringLength, info.String.Length);

			var srcPackage = JavaConstants.FormatterInternalPackage;
			var rsrcPackage = JavaConstants.FormatterPackage;
			const string className = "FormatterStringsTable";

			using (var writer = new FileWriter(TargetLanguage.Java, FileUtils.OpenWrite(JavaConstants.GetFilename(genTypes, srcPackage, className + ".java")))) {
				writer.WriteFileHeader();
				writer.WriteLine($"package {srcPackage};");
				writer.WriteLine();
				writer.WriteLine($"/** {JavaConstants.InternalDoc} */");
				writer.WriteLine($"public final class {className} {{");
				using (writer.Indent()) {
					writer.WriteLine($"/** {JavaConstants.InternalDoc} */");
					writer.WriteLine($"public static final int MAX_STRING_LENGTH = {maxStringLength};");
					writer.WriteLine($"/** {JavaConstants.InternalDoc} */");
					writer.WriteLine($"public static final int STRINGS_COUNT = {sortedInfos.Length};");
				}
				writer.WriteLine("}");
			}
			using (var writer = new BinaryByteTableWriter(JavaConstants.GetResourceFilename(genTypes, rsrcPackage, className + ".bin")))
				StringsTableSerializerUtils.SerializeTable(writer, sortedInfos);
		}
	}
}
