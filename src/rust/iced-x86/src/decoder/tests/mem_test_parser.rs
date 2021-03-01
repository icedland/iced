// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::decoder::tests::decoder_mem_test_case::*;
use crate::decoder::tests::enums::DecoderTestOptions;
use crate::test_utils::from_str_conv::*;
use crate::test_utils::get_default_ip;
use crate::*;
use alloc::string::String;
use alloc::vec::Vec;
use core::iter::IntoIterator;
use std::fs::File;
use std::io::prelude::*;
use std::io::{BufReader, Lines};
use std::path::Path;

pub(super) struct DecoderMemoryTestParser {
	filename: String,
	lines: Lines<BufReader<File>>,
	bitness: u32,
}

impl DecoderMemoryTestParser {
	pub(super) fn new(bitness: u32, filename: &Path) -> Self {
		let display_filename = filename.display().to_string();
		let file = File::open(filename).unwrap_or_else(|_| panic!("Couldn't open file {}", display_filename));
		let lines = BufReader::new(file).lines();
		Self { filename: display_filename, lines, bitness }
	}
}

impl IntoIterator for DecoderMemoryTestParser {
	type Item = DecoderMemoryTestCase;
	type IntoIter = IntoIter;

	fn into_iter(self) -> Self::IntoIter {
		IntoIter { filename: self.filename, lines: self.lines, bitness: self.bitness, line_number: 0 }
	}
}

pub(super) struct IntoIter {
	filename: String,
	lines: Lines<BufReader<File>>,
	bitness: u32,
	line_number: u32,
}

impl Iterator for IntoIter {
	type Item = DecoderMemoryTestCase;

	fn next(&mut self) -> Option<Self::Item> {
		loop {
			let result = match self.lines.next()? {
				Ok(line) => {
					self.line_number += 1;
					if line.is_empty() || line.starts_with('#') {
						continue;
					}
					self.read_next_test_case(line, self.line_number)
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
				Err(err) => panic!("Error parsing decoder memory test case file '{}', line {}: {}", self.filename, self.line_number, err),
			}
		}
	}
}

impl IntoIter {
	fn read_next_test_case(&self, line: String, line_number: u32) -> Result<Option<DecoderMemoryTestCase>, String> {
		let parts: Vec<_> = line.split(',').collect();
		if parts.len() != 11 && parts.len() != 12 {
			return Err(format!("Invalid number of commas ({} commas)", parts.len() - 1));
		}

		let hex_bytes = parts[0].trim();
		let ip = get_default_ip(self.bitness);
		let _ = to_vec_u8(hex_bytes)?;
		if is_ignored_code(parts[1].trim()) {
			return Ok(None);
		}
		let code = to_code(parts[1].trim())?;
		let register = to_register(parts[2].trim())?;
		let prefix_segment = to_register(parts[3].trim())?;
		let segment = to_register(parts[4].trim())?;
		let base_register = to_register(parts[5].trim())?;
		let index_register = to_register(parts[6].trim())?;
		let scale = to_u32(parts[7].trim())?;
		let displacement = to_u64(parts[8].trim())?;
		let displ_size = to_u32(parts[9].trim())?;
		let constant_offsets = super::test_parser::parse_constant_offsets(parts[10].trim())?;
		let encoded_hex_bytes = if parts.len() == 11 { hex_bytes } else { parts[11].trim() };
		let _ = to_vec_u8(encoded_hex_bytes)?;
		let decoder_options = DecoderOptions::NONE;
		let test_options = DecoderTestOptions::NONE;

		Ok(Some(DecoderMemoryTestCase {
			bitness: self.bitness,
			hex_bytes: hex_bytes.to_string(),
			ip,
			code,
			register,
			prefix_segment,
			segment,
			base_register,
			index_register,
			scale,
			displacement,
			displ_size,
			constant_offsets,
			encoded_hex_bytes: encoded_hex_bytes.to_string(),
			decoder_options,
			line_number,
			test_options,
		}))
	}
}
