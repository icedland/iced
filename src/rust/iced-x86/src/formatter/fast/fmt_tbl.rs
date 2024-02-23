// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::data_reader::DataReader;
use crate::formatter::fast::enums::*;
use crate::formatter::fast::fmt_data::FORMATTER_TBL_DATA;
use crate::formatter::fast::{FastStringMnemonic, FmtTableData};
use crate::formatter::strings_data::*;
use crate::iced_constants::IcedConstants;
use alloc::vec::Vec;
use core::convert::TryInto;
use lazy_static::lazy_static;

// If this fails, change FastString20 to eg. FastString24 (multiple of 4 or 8 depending on what's best for PERF),
// see fast.rs where FastString20 is created.
const _: () = assert!(MAX_STRING_LEN <= FastStringMnemonic::SIZE);

lazy_static! {
	pub(super) static ref FMT_DATA: FmtTableData = read();
}

fn get_strings_table() -> Vec<FastStringMnemonic> {
	// If this fails, the generator was updated and now FastStringRegister must be changed
	// to the correct type in fast.rs
	const _: () = assert!(FastStringMnemonic::SIZE == VALID_STRING_LENGTH);

	let mut reader = DataReader::new(&STRINGS_TBL_DATA);
	let mut strings = Vec::with_capacity(STRINGS_COUNT);
	for _ in 0..STRINGS_COUNT {
		// It's safe to read FastStringMnemonic::SIZE bytes from the last string since the
		// table includes extra padding. See const-assert above and the table.
		let len_data = reader.read_len_data();
		strings.push(FastStringMnemonic::from_raw(len_data));
	}
	debug_assert_eq!(reader.len_left(), PADDING_SIZE);

	strings
}

fn read() -> FmtTableData {
	let mut mnemonics: Vec<FastStringMnemonic> = Vec::with_capacity(IcedConstants::CODE_ENUM_COUNT);
	let mut flags: Vec<u8> = Vec::with_capacity(IcedConstants::CODE_ENUM_COUNT);
	let mut reader = DataReader::new(FORMATTER_TBL_DATA);
	let strings = get_strings_table();
	let mut prev_index = -1isize;
	let mut prev_flags = FastFmtFlags::NONE as usize;
	for _ in 0..IcedConstants::CODE_ENUM_COUNT {
		let f = reader.read_u8();
		let current_index;
		if (f & (FastFmtFlags::SAME_AS_PREV as usize)) != 0 {
			current_index = reader.index() as isize;
			reader.set_index(prev_index as usize);
		} else {
			current_index = -1;
			prev_index = reader.index() as isize;
		}
		let mnemonic: FastStringMnemonic = if (prev_flags & FastFmtFlags::HAS_VPREFIX as usize) == (f & FastFmtFlags::HAS_VPREFIX as usize)
			&& (f & (FastFmtFlags::SAME_AS_PREV as usize)) != 0
		{
			mnemonics[mnemonics.len() - 1]
		} else if (f & (FastFmtFlags::HAS_VPREFIX as usize)) != 0 {
			let old_str = strings[reader.read_compressed_u32() as usize];
			let mut new_vec = Vec::with_capacity(old_str.len() + 1);
			let new_len = 1 + old_str.len();
			debug_assert!(new_len <= MAX_STRING_LEN);
			debug_assert!(new_len <= FastStringMnemonic::SIZE);
			new_vec.push(new_len as u8);
			new_vec.push(b'v');
			new_vec.extend(old_str.get_slice().iter().copied().chain(core::iter::repeat(b' ')).take(FastStringMnemonic::SIZE - 1));
			debug_assert_eq!(new_vec.len(), 1 + FastStringMnemonic::SIZE);
			let len_data = new_vec.leak();
			FastStringMnemonic::from_raw(len_data)
		} else {
			strings[reader.read_compressed_u32() as usize]
		};

		flags.push(f as u8);
		mnemonics.push(mnemonic);
		prev_flags = f;

		if current_index >= 0 {
			reader.set_index(current_index as usize);
		}
	}
	debug_assert!(!reader.can_read());

	#[allow(clippy::unwrap_used)]
	let mnemonics = mnemonics.into_boxed_slice().try_into().ok().unwrap();
	#[allow(clippy::unwrap_used)]
	let flags = flags.into_boxed_slice().try_into().ok().unwrap();
	FmtTableData { mnemonics, flags }
}
