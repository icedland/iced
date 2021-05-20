// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::formatter::regs_tbl::{MAX_STRING_LENGTH, REGS_TBL};
use crate::formatter::FormatterString;
use alloc::string::String;
use alloc::vec::Vec;
use lazy_static::lazy_static;

lazy_static! {
	pub(super) static ref ALL_REGISTERS: Vec<FormatterString> = {
		let regs_tbl = &*REGS_TBL;
		let mut v = Vec::with_capacity(regs_tbl.len());
		let mut s = String::with_capacity(MAX_STRING_LENGTH + 1);
		for reg in regs_tbl {
			s.push('%');
			s.push_str(reg.get(false));
			v.push(FormatterString::new(s.clone()));
			s.clear();
		}
		v
	};
}
