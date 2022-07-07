// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Formatters {
	interface IFormatterTableSerializer {
		string GetFilename(GenTypes genTypes);
		void Initialize(GenTypes genTypes, StringsTable stringsTable);
	}
}
