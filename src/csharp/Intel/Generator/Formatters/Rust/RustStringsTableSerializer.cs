/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

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
