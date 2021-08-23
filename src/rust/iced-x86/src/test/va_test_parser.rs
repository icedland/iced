// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::test::va_test_case::*;
use crate::test_utils::from_str_conv::*;
use crate::DecoderOptions;
use alloc::string::String;
use alloc::vec::Vec;
use core::iter::IntoIterator;
use std::fs::File;
use std::io::prelude::*;
use std::io::{BufReader, Lines};
use std::path::Path;

pub(super) struct VirtualAddressTestParser {
	filename: String,
	lines: Lines<BufReader<File>>,
}

impl VirtualAddressTestParser {
	pub(super) fn new(filename: &Path) -> Self {
		let display_filename = filename.display().to_string();
		let file = File::open(filename).unwrap_or_else(|_| panic!("Couldn't open file {}", display_filename));
		let lines = BufReader::new(file).lines();
		Self { filename: display_filename, lines }
	}
}

impl IntoIterator for VirtualAddressTestParser {
	type Item = VirtualAddressTestCase;
	type IntoIter = IntoIter;

	fn into_iter(self) -> Self::IntoIter {
		IntoIter { filename: self.filename, lines: self.lines, line_number: 0 }
	}
}

pub(super) struct IntoIter {
	filename: String,
	lines: Lines<BufReader<File>>,
	line_number: u32,
}

impl Iterator for IntoIter {
	type Item = VirtualAddressTestCase;

	fn next(&mut self) -> Option<Self::Item> {
		loop {
			let result = match self.lines.next()? {
				Ok(line) => {
					self.line_number += 1;
					if line.is_empty() || line.starts_with('#') {
						continue;
					}
					IntoIter::read_next_test_case(line)
				}
				Err(err) => Err(err.to_string()),
			};
			match result {
				Ok(tc) => {
					if let Some(tc) = tc {
						return Some(tc);
					} else {
						continue;
					}
				}
				Err(err) => panic!("Error parsing virtual address test case file '{}', line {}: {}", self.filename, self.line_number, err),
			}
		}
	}
}

impl IntoIter {
	fn read_next_test_case(line: String) -> Result<Option<VirtualAddressTestCase>, String> {
		let elems: Vec<_> = line.split(',').collect();
		if elems.len() != 9 {
			return Err(format!("Invalid number of commas: {}", elems.len() - 1));
		}

		let bitness = to_u32(elems[0])?;
		if is_ignored_code(elems[1].trim()) {
			return Ok(None);
		}
		let hex_bytes = String::from(elems[2].trim());
		let _ = to_vec_u8(&hex_bytes)?;
		let operand = to_i32(elems[3])?;
		let used_mem_index = to_i32(elems[4])?;
		let element_index = to_u32(elems[5])? as usize;
		let expected_value = to_u64(elems[6])?;
		let dec_opt_str = elems[7].trim();
		let decoder_options = if dec_opt_str.is_empty() { DecoderOptions::NONE } else { to_decoder_options(dec_opt_str)? };

		let mut register_values: Vec<VARegisterValue> = Vec::new();
		for elem in elems[8].split_whitespace() {
			if elem.is_empty() {
				continue;
			}
			let kv: Vec<_> = elem.split('=').collect();
			if kv.len() != 2 {
				return Err(format!("Expected key=value: {}", elem));
			}
			let key = kv[0];
			let value_str = kv[1];

			let (register, expected_element_index, expected_element_size) = if key.contains(';') {
				let parts: Vec<_> = key.split(';').collect();
				if parts.len() != 3 {
					return Err(format!("Invalid number of semicolons: {}", parts.len() - 1));
				}
				(to_register(parts[0])?, to_u32(parts[1])? as usize, to_u32(parts[2])? as usize)
			} else {
				(to_register(key)?, 0, 0)
			};
			let value = to_u64(value_str)?;
			register_values.push(VARegisterValue { register, element_index: expected_element_index, element_size: expected_element_size, value });
		}

		Ok(Some(VirtualAddressTestCase {
			bitness,
			hex_bytes,
			decoder_options,
			operand,
			used_mem_index,
			element_index,
			expected_value,
			register_values,
		}))
	}
}
