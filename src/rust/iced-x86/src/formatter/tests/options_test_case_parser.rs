// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

use super::super::super::test_utils::from_str_conv::*;
use super::enums::OptionsProps;
use super::opt_value::OptionValue;
use super::options_parser::parse_option;
use super::opts_info::*;
use alloc::string::String;
use alloc::vec::Vec;
use core::iter::IntoIterator;
use core::u32;
use std::collections::HashSet;
use std::fs::File;
use std::io::prelude::*;
use std::io::{BufReader, Lines};
use std::path::Path;

pub(super) struct OptionsTestParser<'a> {
	filename: String,
	lines: Lines<BufReader<File>>,
	ignored: &'a mut HashSet<u32>,
}

impl<'a> OptionsTestParser<'a> {
	pub(super) fn new(filename: &Path, ignored: &'a mut HashSet<u32>) -> Self {
		let display_filename = filename.display().to_string();
		let file = File::open(filename).unwrap_or_else(|_| panic!("Couldn't open file {}", display_filename));
		let lines = BufReader::new(file).lines();
		Self { filename: display_filename, lines, ignored }
	}
}

impl<'a> IntoIterator for OptionsTestParser<'a> {
	type Item = OptionsInstructionInfo;
	type IntoIter = IntoIter<'a>;

	fn into_iter(self) -> Self::IntoIter {
		IntoIter { filename: self.filename, lines: self.lines, ignored: self.ignored, test_case_number: 0, line_number: 0 }
	}
}

pub(super) struct IntoIter<'a> {
	filename: String,
	lines: Lines<BufReader<File>>,
	ignored: &'a mut HashSet<u32>,
	test_case_number: u32,
	line_number: u32,
}

impl<'a> Iterator for IntoIter<'a> {
	type Item = OptionsInstructionInfo;

	fn next(&mut self) -> Option<Self::Item> {
		loop {
			match self.lines.next() {
				None => return None,
				Some(info) => {
					let result = match info {
						Ok(line) => {
							self.line_number += 1;
							if line.is_empty() || line.starts_with('#') {
								continue;
							}
							self.test_case_number += 1;
							IntoIter::read_next_test_case(line, self.line_number)
						}
						Err(err) => Err(err.to_string()),
					};
					match result {
						Ok(tc) => {
							if let Some(tc) = tc {
								return Some(tc);
							} else {
								let _ = self.ignored.insert(self.test_case_number - 1);
								continue;
							}
						}
						Err(err) => panic!("Error parsing options test case file '{}', line {}: {}", self.filename, self.line_number, err),
					}
				}
			}
		}
	}
}

impl<'a> IntoIter<'a> {
	fn read_next_test_case(line: String, _line_number: u32) -> Result<Option<OptionsInstructionInfo>, String> {
		let elems: Vec<_> = line.split(',').collect();
		if elems.len() != 4 {
			return Err(format!("Invalid number of commas: {}", elems.len() - 1));
		}

		let bitness = to_u32(elems[0])?;
		let hex_bytes = elems[1];
		let _ = to_vec_u8(hex_bytes)?;
		if is_ignored_code(elems[2]) {
			return Ok(None);
		}
		let code = to_code(elems[2])?;
		let mut properties: Vec<(OptionsProps, OptionValue)> = Vec::new();

		for part in elems[3].split_whitespace() {
			if part.trim().is_empty() {
				continue;
			}
			let parsed_option = parse_option(part)?;
			properties.push(parsed_option);
		}

		let decoder_options = OptionValue::get_decoder_options(&properties);
		Ok(Some(OptionsInstructionInfo { bitness, hex_bytes: String::from(hex_bytes), decoder_options, code, vec: properties }))
	}
}
