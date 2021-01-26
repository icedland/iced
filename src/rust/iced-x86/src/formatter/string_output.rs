// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

use super::enums_shared::FormatterTextKind;
use super::FormatterOutput;
#[cfg(not(feature = "std"))]
use alloc::string::String;

impl FormatterOutput for String {
	#[inline]
	fn write(&mut self, text: &str, _kind: FormatterTextKind) {
		self.push_str(text);
	}
}
