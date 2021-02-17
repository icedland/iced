// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using Generator.IO;

namespace Generator.Formatters.Rust {
	sealed class RustFastFormatterTableSerializer : FastFormatterTableSerializer {
		readonly string filename;

		public RustFastFormatterTableSerializer(string filename, FastFmtInstructionDef[] defs)
			: base(defs, RustIdentifierConverter.Create()) {
			this.filename = filename;
		}

		public override string GetFilename(GenTypes genTypes) => filename;

		public override void Serialize(GenTypes genTypes, FileWriter writer, StringsTable stringsTable) {
			writer.WriteFileHeader();
			writer.WriteLine(RustConstants.AttributeNoRustFmt);
			writer.WriteLine($"pub(super) static FORMATTER_TBL_DATA: &[u8] = &[");
			using (writer.Indent())
				SerializeTable(genTypes, writer, stringsTable);
			writer.WriteLine("];");
		}
	}
}
