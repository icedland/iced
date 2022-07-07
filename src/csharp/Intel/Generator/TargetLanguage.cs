// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator {
	enum TargetLanguage {
		// Code that generates files (eg. text files) that is used by the other langs
		Other,
		CSharp,
		Rust,
		// Rust JavaScript bindings code
		RustJS,
		Python,
		Lua,
		Java,
	}
}
