// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

use super::super::super::iced_constants::IcedConstants;
use super::super::regs_tbl::{MAX_STRING_LENGTH, REGS_TBL};
use super::super::FormatterString;
#[cfg(not(feature = "std"))]
use alloc::string::String;
#[cfg(not(feature = "std"))]
use alloc::vec::Vec;

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
