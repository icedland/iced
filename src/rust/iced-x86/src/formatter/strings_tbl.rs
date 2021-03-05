// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::formatter::data_reader::DataReader;
use crate::formatter::strings_data::*;
use alloc::vec::Vec;

// The returned array isn't cached since only one formatter is normally used
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub(super) fn get_strings_table_ref() -> Vec<&'static str> {
	let mut reader = DataReader::new(&STRINGS_TBL_DATA);
	let mut strings = Vec::with_capacity(STRINGS_COUNT);
	for _ in 0..STRINGS_COUNT {
		strings.push(reader.read_ascii_str());
	}
	debug_assert_eq!(reader.len_left(), PADDING_SIZE);

	strings
}
