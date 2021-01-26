// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

use super::super::super::data_reader::DataReader;
use super::super::super::iced_constants::IcedConstants;
use super::super::strings_tbl::get_strings_table;
use super::enums::*;
use super::fmt_data::FORMATTER_TBL_DATA;
use super::FmtTableData;
use alloc::boxed::Box;
use alloc::string::String;
use alloc::vec::Vec;

lazy_static! {
	pub(super) static ref FMT_DATA: FmtTableData = read();
}

fn read() -> FmtTableData {
	let mut mnemonics: Vec<&'static str> = Vec::with_capacity(IcedConstants::CODE_ENUM_COUNT);
	let mut flags: Vec<u8> = Vec::with_capacity(IcedConstants::CODE_ENUM_COUNT);
	let mut reader = DataReader::new(FORMATTER_TBL_DATA);
	let strings = get_strings_table();
	let mut prev_index = -1isize;
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
		let mnemonic = if (f & (FastFmtFlags::HAS_VPREFIX as usize)) != 0 {
			let s = &strings[reader.read_compressed_u32() as usize];
			let mut res = String::with_capacity(s.len() + 1);
			res.push('v');
			res.push_str(s);
			res
		} else {
			strings[reader.read_compressed_u32() as usize].clone()
		};

		let mnemonic = Box::into_raw(Box::new(mnemonic));
		let mnemonic: &'static str = unsafe { (*mnemonic).as_str() };
		flags.push(f as u8);
		mnemonics.push(mnemonic);

		if current_index >= 0 {
			reader.set_index(current_index as usize);
		}
	}
	debug_assert!(!reader.can_read());

	FmtTableData { mnemonics, flags }
}
