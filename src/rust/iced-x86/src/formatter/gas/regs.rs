// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::formatter::regs_tbl::{MAX_STRING_LENGTH, REGS_TBL};
use crate::formatter::FormatterString;
use crate::iced_constants::IcedConstants;
use alloc::string::String;
use alloc::vec::Vec;
use lazy_static::lazy_static;

pub(super) struct Registers;
impl Registers {
	pub(super) const REGISTER_ST: u32 = IcedConstants::REGISTER_ENUM_COUNT as u32;
	#[allow(dead_code)]
	pub(super) const EXTRA_REGISTERS: u32 = 1;
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
