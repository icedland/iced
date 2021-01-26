// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

namespace Generator {
	enum TargetLanguage {
		// Code that generates files (eg. text files) that is used by the other langs
		Other,
		CSharp,
		Rust,
		// Rust JavaScript bindings code
		RustJS,
		Python,
	}
}
