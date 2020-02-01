/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

use super::super::super::test_utils::from_str_conv::*;
use super::super::super::*;
use super::decoder_mem_test_case::*;
#[cfg(not(feature = "std"))]
use alloc::string::String;
#[cfg(not(feature = "std"))]
use alloc::vec::Vec;
use core::iter::IntoIterator;
use std::fs::File;
use std::io::prelude::*;
use std::io::{BufReader, Lines};
use std::path::Path;

pub(crate) struct DecoderMemoryTestParser {
	filename: String,
	lines: Lines<BufReader<File>>,
	bitness: u32,
}

impl DecoderMemoryTestParser {
	pub fn new(bitness: u32, filename: &Path) -> Self {
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

pub(crate) struct IntoIter {
	filename: String,
	lines: Lines<BufReader<File>>,
	bitness: u32,
	line_number: u32,
}

impl Iterator for IntoIter {
	type Item = DecoderMemoryTestCase;

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
							self.read_next_test_case(line, self.line_number)
						}
						Err(err) => Err(err.to_string()),
					};
					match result {
						Ok(tc) => return Some(tc),
						Err(err) => panic!("Error parsing decoder memory test case file '{}', line {}: {}", self.filename, self.line_number, err),
					}
				}
			}
		}
	}
}

impl IntoIter {
	fn read_next_test_case(&self, line: String, line_number: u32) -> Result<DecoderMemoryTestCase, String> {
		let parts: Vec<_> = line.split(',').collect();
		if parts.len() != 11 && parts.len() != 12 {
			return Err(format!("Invalid number of commas ({} commas)", parts.len() - 1));
		}

		let hex_bytes = parts[0].trim();
		let _ = to_vec_u8(hex_bytes)?;
		let code = to_code(parts[1].trim())?;
		let register = to_register(parts[2].trim())?;
		let prefix_segment = to_register(parts[3].trim())?;
		let segment = to_register(parts[4].trim())?;
		let base_register = to_register(parts[5].trim())?;
		let index_register = to_register(parts[6].trim())?;
		let scale = to_u32(parts[7].trim())?;
		let displacement = to_u32(parts[8].trim())?;
		let displ_size = to_u32(parts[9].trim())?;
		let constant_offsets = super::test_parser::parse_constant_offsets(parts[10].trim())?;
		let encoded_hex_bytes = if parts.len() == 11 { hex_bytes } else { parts[11].trim() };
		let _ = to_vec_u8(encoded_hex_bytes)?;
		let decoder_options = DecoderOptions::NONE;
		let can_encode = true;

		Ok(DecoderMemoryTestCase {
			bitness: self.bitness,
			hex_bytes: hex_bytes.to_string(),
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
			can_encode,
		})
	}
}
