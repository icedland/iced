// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::formatter::regs_tbl::MAX_STRING_LENGTH;
use crate::formatter::regs_tbl_ls::REGS_TBL;
use crate::formatter::FormatterString;
use crate::iced_constants::IcedConstants;
use alloc::boxed::Box;
use alloc::string::String;
use alloc::vec::Vec;
use core::convert::TryInto;
use lazy_static::lazy_static;

lazy_static! {
	pub(super) static ref ALL_REGISTERS: Box<[FormatterString; IcedConstants::REGISTER_ENUM_COUNT]> = {
		let regs_tbl: &[FormatterString; IcedConstants::REGISTER_ENUM_COUNT] = &REGS_TBL;
		let mut v = Vec::with_capacity(IcedConstants::REGISTER_ENUM_COUNT);
		let mut s = String::with_capacity(MAX_STRING_LENGTH + 1);
		for reg in regs_tbl {
			s.push('%');
			s.push_str(reg.get(false));
			v.push(FormatterString::new(s.clone()));
			s.clear();
		}
		#[allow(clippy::unwrap_used)]
		v.into_boxed_slice().try_into().ok().unwrap()
	};
}
