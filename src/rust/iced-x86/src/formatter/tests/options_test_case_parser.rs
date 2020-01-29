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
use super::enums::OptionsProps;
use super::opts_info::*;
use core::iter::IntoIterator;
use core::u32;
use std::fs::File;
use std::io::prelude::*;
use std::io::{BufReader, Lines};
use std::path::Path;

pub(super) struct OptionsTestParser {
	filename: String,
	lines: Lines<BufReader<File>>,
}

impl OptionsTestParser {
	pub fn new(filename: &Path) -> Self {
		let display_filename = filename.display().to_string();
		let file = File::open(filename).unwrap_or_else(|_| panic!("Couldn't open file {}", display_filename));
		let lines = BufReader::new(file).lines();
		Self { filename: display_filename, lines }
	}
}

impl IntoIterator for OptionsTestParser {
	type Item = OptionsInstructionInfo;
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
							self.read_next_test_case(line, self.line_number)
						}
						Err(err) => Err(err.to_string()),
					};
					match result {
						Ok(tc) => return Some(tc),
						Err(err) => panic!("Error parsing options test case file '{}', line {}: {}", self.filename, self.line_number, err),
					}
				}
			}
		}
	}
}

impl IntoIter {
	fn read_next_test_case(&self, line: String, _line_number: u32) -> Result<OptionsInstructionInfo, String> {
		let elems: Vec<_> = line.split(',').collect();
		if elems.len() != 4 {
			return Err(format!("Invalid number of commas: {}", elems.len() - 1));
		}

		let bitness = to_u32(elems[0])?;
		let hex_bytes = elems[1];
		let _ = to_vec_u8(hex_bytes)?;
		let code = to_code(elems[2])?;
		let mut properties: Vec<(OptionsProps, OptionValue)> = Vec::new();

		for part in elems[3].split_whitespace() {
			let key = part.trim();
			if key.is_empty() {
				continue;
			}
			let kv_parts: Vec<_> = key.splitn(2, '=').collect();
			if kv_parts.len() != 2 {
				return Err(format!("Expected key=value: '{}'", part));
			}
			let value_str = kv_parts[1].trim();
			let prop = to_options_props(kv_parts[0])?;
			let value = match prop {
				OptionsProps::AddLeadingZeroToHexNumbers
				| OptionsProps::AlwaysShowScale
				| OptionsProps::AlwaysShowSegmentRegister
				| OptionsProps::BranchLeadingZeroes
				| OptionsProps::DisplacementLeadingZeroes
				| OptionsProps::GasNakedRegisters
				| OptionsProps::GasShowMnemonicSizeSuffix
				| OptionsProps::GasSpaceAfterMemoryOperandComma
				| OptionsProps::LeadingZeroes
				| OptionsProps::MasmAddDsPrefix32
				| OptionsProps::NasmShowSignExtendedImmediateSize
				| OptionsProps::PreferST0
				| OptionsProps::RipRelativeAddresses
				| OptionsProps::ScaleBeforeIndex
				| OptionsProps::ShowBranchSize
				| OptionsProps::ShowZeroDisplacements
				| OptionsProps::SignedImmediateOperands
				| OptionsProps::SignedMemoryDisplacements
				| OptionsProps::SmallHexNumbersInDecimal
				| OptionsProps::SpaceAfterMemoryBracket
				| OptionsProps::SpaceAfterOperandSeparator
				| OptionsProps::SpaceBetweenMemoryAddOperators
				| OptionsProps::SpaceBetweenMemoryMulOperators
				| OptionsProps::UpperCaseAll
				| OptionsProps::UpperCaseDecorators
				| OptionsProps::UpperCaseHex
				| OptionsProps::UpperCaseKeywords
				| OptionsProps::UpperCaseMnemonics
				| OptionsProps::UpperCasePrefixes
				| OptionsProps::UpperCaseRegisters
				| OptionsProps::UsePseudoOps => OptionValue::Boolean(to_boolean(value_str)?),

				OptionsProps::BinaryDigitGroupSize
				| OptionsProps::DecimalDigitGroupSize
				| OptionsProps::FirstOperandCharIndex
				| OptionsProps::HexDigitGroupSize
				| OptionsProps::OctalDigitGroupSize
				| OptionsProps::TabSize => OptionValue::Int32(to_i32(value_str)?),

				OptionsProps::IP => OptionValue::UInt64(to_u64(value_str)?),

				OptionsProps::BinaryPrefix
				| OptionsProps::BinarySuffix
				| OptionsProps::DecimalPrefix
				| OptionsProps::DecimalSuffix
				| OptionsProps::DigitSeparator
				| OptionsProps::HexPrefix
				| OptionsProps::HexSuffix
				| OptionsProps::OctalPrefix
				| OptionsProps::OctalSuffix => OptionValue::String(String::from(if value_str == "<null>" { "" } else { value_str })),

				OptionsProps::MemorySizeOptions => OptionValue::MemorySizeOptions(to_memory_size_options(value_str)?),
				OptionsProps::NumberBase => OptionValue::NumberBase(to_number_base(value_str)?),
			};
			properties.push((prop, value));
		}

		Ok(OptionsInstructionInfo { bitness, hex_bytes: String::from(hex_bytes), code, vec: properties })
	}
}
