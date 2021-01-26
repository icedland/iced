// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

use super::data_reader::DataReader;
use super::strings_data::*;
use alloc::string::String;
use alloc::vec::Vec;

// The returned array isn't cached since only one formatter is normally used
pub(super) fn get_strings_table() -> Vec<String> {
	let mut reader = DataReader::new(&STRINGS_TBL_DATA);
	let mut strings = Vec::with_capacity(STRINGS_COUNT);
	for _ in 0..STRINGS_COUNT {
		strings.push(reader.read_ascii_string());
	}
	debug_assert!(!reader.can_read());

	strings
}
