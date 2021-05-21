// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::formatter::fast::FastStringRegister;
use crate::formatter::regs_tbl::*;
use crate::iced_constants::IcedConstants;
use alloc::boxed::Box;
use alloc::vec::Vec;
use lazy_static::lazy_static;
use static_assertions::const_assert_eq;

lazy_static! {
	pub(super) static ref REGS_TBL: Box<[FastStringRegister; IcedConstants::REGISTER_ENUM_COUNT]> = create();
}

fn create() -> Box<[FastStringRegister; IcedConstants::REGISTER_ENUM_COUNT]> {
	// If this fails, the generator was updated and now FastStringRegister must be changed
	// to the correct type in fast.rs
	const_assert_eq!(FastStringRegister::SIZE, VALID_STRING_LENGTH);

	let mut result = Vec::with_capacity(IcedConstants::REGISTER_ENUM_COUNT);
	let mut data = &REGS_DATA[..];
	for _ in 0..IcedConstants::REGISTER_ENUM_COUNT {
		let len = data[0] as usize;

		// It's safe to read FastStringRegister::SIZE bytes from the last string since the
		// table includes extra padding. See const-assert above and the table.
		result.push(FastStringRegister::new(data.as_ptr()));

		data = &data[1 + len..];
	}
	debug_assert!(data.len() == PADDING_SIZE);

	let result = result.into_boxed_slice();
	debug_assert_eq!(result.len(), IcedConstants::REGISTER_ENUM_COUNT);
	// SAFETY: Size is verified above
	unsafe { Box::from_raw(Box::into_raw(result) as *mut [_; IcedConstants::REGISTER_ENUM_COUNT]) }
}
