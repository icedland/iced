// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::formatter::enums_shared::FormatterTextKind;
use crate::formatter::FormatterOutput;
use alloc::string::String;

impl FormatterOutput for String {
	#[inline]
	fn write(&mut self, text: &str, _kind: FormatterTextKind) {
		self.push_str(text);
	}
}
