// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::formatter::regs_tbl::{MAX_STRING_LENGTH, PADDING_SIZE, REGS_DATA};
use crate::formatter::FormatterString;
use crate::iced_constants::IcedConstants;
use alloc::boxed::Box;
use alloc::string::String;
use alloc::vec::Vec;
use core::convert::TryInto;
use lazy_static::lazy_static;

lazy_static! {
	pub(super) static ref REGS_TBL: Box<[FormatterString; IcedConstants::REGISTER_ENUM_COUNT]> = {
		let mut v = Vec::with_capacity(IcedConstants::REGISTER_ENUM_COUNT);
		let mut s = String::with_capacity(MAX_STRING_LENGTH);
		let mut data = &REGS_DATA[..];
		for _ in 0..IcedConstants::REGISTER_ENUM_COUNT {
			let len = data[0] as usize;
			data = &data[1..];
			for &c in &data[0..len] {
				s.push(c as char);
			}
			data = &data[len..];
			v.push(FormatterString::new(s.clone()));
			s.clear();
		}
		debug_assert!(data.len() == PADDING_SIZE);
		#[allow(clippy::unwrap_used)]
		v.into_boxed_slice().try_into().ok().unwrap()
	};
}
