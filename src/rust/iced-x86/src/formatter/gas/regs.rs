// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::formatter::regs_tbl::{MAX_STRING_LENGTH, REGS_TBL};
use crate::formatter::FormatterString;
use crate::Register;
use alloc::string::String;
use alloc::vec::Vec;
use lazy_static::lazy_static;

pub(super) struct Registers;
impl Registers {
	// Should be 1 past the last real register (not including DontUseF9-DontUseFF)
	pub(super) const REGISTER_ST: u32 = Register::DontUseF9 as u32;
	#[allow(dead_code)]
	pub(super) const EXTRA_REGISTERS: u32 = 0;
}

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
