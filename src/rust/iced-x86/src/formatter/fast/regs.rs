// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::formatter::fast::FastStringRegister;
use crate::formatter::regs_tbl::*;
use crate::iced_constants::IcedConstants;
use alloc::boxed::Box;
use alloc::vec::Vec;
use core::convert::TryInto;
use lazy_static::lazy_static;

lazy_static! {
	pub(super) static ref REGS_TBL: Box<[FastStringRegister; IcedConstants::REGISTER_ENUM_COUNT]> = create();
}

fn create() -> Box<[FastStringRegister; IcedConstants::REGISTER_ENUM_COUNT]> {
	// If this fails, the generator was updated and now FastStringRegister must be changed
	// to the correct type in fast.rs
	const _: () = assert!(FastStringRegister::SIZE == VALID_STRING_LENGTH);

	let mut result = Vec::with_capacity(IcedConstants::REGISTER_ENUM_COUNT);
	let mut data = &REGS_DATA[..];
	for _ in 0..IcedConstants::REGISTER_ENUM_COUNT {
		let len = data[0] as usize;

		// It's safe to read FastStringRegister::SIZE bytes from the last string since the
		// table includes extra padding. See const-assert above and the table.
		result.push(FastStringRegister::from_raw(data));

		data = &data[1 + len..];
	}
	debug_assert!(data.len() == PADDING_SIZE);

	#[allow(clippy::unwrap_used)]
	result.into_boxed_slice().try_into().ok().unwrap()
}
