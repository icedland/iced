// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using Generator.IO;

namespace Generator.Formatters.Java {
	sealed class JavaFastFormatterTableSerializer : FastFormatterTableSerializer {
		readonly string package;

		public JavaFastFormatterTableSerializer(FastFmtInstructionDef[] defs, string package)
			: base(defs, JavaIdentifierConverter.Create()) => this.package = package;

		public override string GetFilename(GenTypes genTypes) =>
			JavaConstants.GetResourceFilename(genTypes, package, "FmtData.bin");

		public void Serialize(GenTypes genTypes, BinaryByteTableWriter writer, StringsTable stringsTable) =>
			SerializeTable(genTypes, writer, stringsTable);
	}
}
