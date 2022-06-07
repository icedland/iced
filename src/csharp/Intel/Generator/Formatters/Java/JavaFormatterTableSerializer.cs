// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using Generator.Enums;
using Generator.IO;

namespace Generator.Formatters.Java {
	sealed class JavaFormatterTableSerializer : FormatterTableSerializer {
		readonly string package;

		public JavaFormatterTableSerializer(FmtInstructionDef[] defs, EnumType ctorKindEnum, string package)
			: base(defs, JavaIdentifierConverter.Create(), ctorKindEnum["Previous"]) => this.package = package;

		public override string GetFilename(GenTypes genTypes) =>
			JavaConstants.GetResourceFilename(genTypes, package, "InstrInfos.bin");

		public void Serialize(GenTypes genTypes, BinaryByteTableWriter writer, StringsTable stringsTable) =>
			SerializeTable(writer, stringsTable);
	}
}
