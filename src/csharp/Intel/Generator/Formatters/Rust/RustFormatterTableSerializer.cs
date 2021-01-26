// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

using Generator.Enums;
using Generator.IO;

namespace Generator.Formatters.Rust {
	sealed class RustFormatterTableSerializer : FormatterTableSerializer {
		readonly string filename;

		public RustFormatterTableSerializer(string filename, FmtInstructionDef[] defs, EnumType ctorKindEnum)
			: base(defs, RustIdentifierConverter.Create(), ctorKindEnum["Previous"]) {
			this.filename = filename;
		}

		public override string GetFilename(GenTypes genTypes) => filename;

		public override void Serialize(GenTypes genTypes, FileWriter writer, StringsTable stringsTable) {
			writer.WriteFileHeader();
			writer.WriteLine(RustConstants.AttributeNoRustFmt);
			writer.WriteLine($"pub(super) static FORMATTER_TBL_DATA: &[u8] = &[");
			using (writer.Indent())
				SerializeTable(writer, stringsTable);
			writer.WriteLine("];");
		}
	}
}
