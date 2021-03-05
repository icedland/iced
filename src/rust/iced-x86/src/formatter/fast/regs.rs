// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::formatter::fast::FastStringRegister;
use crate::formatter::regs_tbl::*;
use alloc::vec::Vec;
use lazy_static::lazy_static;
use static_assertions::const_assert_eq;

lazy_static! {
	pub(super) static ref REGS_TBL: Vec<FastStringRegister> = create();
}

#[allow(clippy::char_lit_as_u8)]
fn create() -> Vec<FastStringRegister> {
	// If this fails, the generator was updated and now FastStringRegister must be changed
	// to the correct type in fast.rs
	const_assert_eq!(FastStringRegister::SIZE, VALID_STRING_LENGTH);

	let mut result = Vec::with_capacity(STRINGS_COUNT);
	let mut data = &REGS_DATA[..];
	for _ in 0..STRINGS_COUNT {
		let len = data[0] as usize;

		// It's safe to read FastStringRegister::SIZE bytes from the last string since the
		// table includes extra padding. See const-assert above and the table.
		result.push(FastStringRegister::new(data.as_ptr()));

		data = &data[1 + len..];
	}
	debug_assert!(data.len() == PADDING_SIZE);
	result
}
