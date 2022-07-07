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

			const int FastStringMnemonicSize = 20;
			if (maxStringLength > FastStringMnemonicSize) {
				// Requires updating fast.rs `FastStringMnemonic` to match the new aligned size
				// eg. FastString24/etc depending on perf
				throw new InvalidOperationException();
			}

			var last = sortedInfos[^1];
			int extraPadding = FastStringMnemonicSize - last.String.Length;
			if (extraPadding < 0)
				throw new InvalidOperationException();

			writer.WriteFileHeader();
			writer.WriteLine($"pub(super) const STRINGS_COUNT: usize = {sortedInfos.Length};");
			writer.WriteLine(RustConstants.AttributeAllowDeadCode);
			writer.WriteLine($"pub(super) const MAX_STRING_LEN: usize = {maxStringLength};");
			writer.WriteLine(RustConstants.AttributeAllowDeadCode);
			writer.WriteLine($"pub(super) const VALID_STRING_LENGTH: usize = {FastStringMnemonicSize};");
			writer.WriteLine($"pub(super) const PADDING_SIZE: usize = {extraPadding};");
			writer.WriteLine();
			writer.WriteLine(RustConstants.AttributeNoRustFmt);
			writer.WriteLine($"pub(super) static STRINGS_TBL_DATA: [u8; {StringsTableSerializerUtils.GetByteCount(sortedInfos) + extraPadding}] = [");
			using (writer.Indent())
				StringsTableSerializerUtils.SerializeTable(new TextFileByteTableWriter(writer), sortedInfos, extraPadding, "Padding so it's possible to read FastStringMnemonic::SIZE bytes from the last value");
			writer.WriteLine("];");
		}
	}
}
