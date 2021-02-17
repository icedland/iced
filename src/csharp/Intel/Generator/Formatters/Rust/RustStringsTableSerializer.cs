// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using Generator.IO;

namespace Generator.Formatters.Rust {
	struct RustStringsTableSerializer {
		readonly StringsTable stringsTable;

		public RustStringsTableSerializer(StringsTable stringsTable) =>
			this.stringsTable = stringsTable;

		public void Serialize(FileWriter writer) {
			if (!stringsTable.IsFrozen)
				throw new InvalidOperationException();

			var sortedInfos = stringsTable.Infos;
			int maxStringLength = 0;
			foreach (var info in sortedInfos)
				maxStringLength = Math.Max(maxStringLength, info.String.Length);

			writer.WriteFileHeader();
			writer.WriteLine($"pub(super) const STRINGS_COUNT: usize = {sortedInfos.Length};");
			writer.WriteLine();
			writer.WriteLine(RustConstants.AttributeNoRustFmt);
			writer.WriteLine($"pub(super) static STRINGS_TBL_DATA: [u8; {StringsTableSerializerUtils.GetByteCount(sortedInfos)}] = [");
			using (writer.Indent())
				StringsTableSerializerUtils.SerializeTable(writer, sortedInfos);
			writer.WriteLine("];");
		}
	}
}
