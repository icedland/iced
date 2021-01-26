// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

using Generator.IO;

namespace Generator.Formatters {
	interface IFormatterTableSerializer {
		string GetFilename(GenTypes genTypes);
		void Initialize(GenTypes genTypes, StringsTable stringsTable);
		void Serialize(GenTypes genTypes, FileWriter writer, StringsTable stringsTable);
	}
}
