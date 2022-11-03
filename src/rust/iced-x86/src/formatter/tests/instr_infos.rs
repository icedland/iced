// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::code::Code;
use crate::formatter::test_utils::from_str_conv::{is_ignored_code, to_code, to_decoder_options};
use crate::formatter::test_utils::{get_default_ip, get_formatter_unit_tests_dir};
use alloc::string::String;
use alloc::vec::Vec;
use lazy_static::lazy_static;
use std::collections::HashSet;
use std::fs::File;
use std::io::prelude::*;
use std::io::BufReader;

pub(super) struct InstructionInfo {
	pub(super) bitness: u32,
	pub(super) hex_bytes: String,
	pub(super) ip: u64,
	pub(super) code: Code,
	pub(super) options: u32,
}

lazy_static! {
	static ref INFOS_16: (Vec<InstructionInfo>, HashSet<u32>) = read_infos(16, false);
}
lazy_static! {
	static ref INFOS_32: (Vec<InstructionInfo>, HashSet<u32>) = read_infos(32, false);
}
lazy_static! {
	static ref INFOS_64: (Vec<InstructionInfo>, HashSet<u32>) = read_infos(64, false);
}

lazy_static! {
	static ref INFOS_MISC_16: (Vec<InstructionInfo>, HashSet<u32>) = read_infos(16, true);
}
lazy_static! {
	static ref INFOS_MISC_32: (Vec<InstructionInfo>, HashSet<u32>) = read_infos(32, true);
}
lazy_static! {
	static ref INFOS_MISC_64: (Vec<InstructionInfo>, HashSet<u32>) = read_infos(64, true);
}

pub(super) fn get_infos(bitness: u32, is_misc: bool) -> &'static (Vec<InstructionInfo>, HashSet<u32>) {
	if is_misc {
		match bitness {
			16 => &INFOS_MISC_16,
			32 => &INFOS_MISC_32,
			64 => &INFOS_MISC_64,
			_ => unreachable!(),
		}
	} else {
		match bitness {
			16 => &INFOS_16,
			32 => &INFOS_32,
			64 => &INFOS_64,
			_ => unreachable!(),
		}
	}
}

fn read_infos(bitness: u32, is_misc: bool) -> (Vec<InstructionInfo>, HashSet<u32>) {
	let mut filename = get_formatter_unit_tests_dir();
	if is_misc {
		filename.push(format!("InstructionInfos{}_Misc.txt", bitness));
	} else {
		filename.push(format!("InstructionInfos{}.txt", bitness));
	}

	let display_filename = filename.display().to_string();
	let file = File::open(filename).unwrap_or_else(|_| panic!("Couldn't open file {}", display_filename));
	let mut infos: Vec<InstructionInfo> = Vec::new();
	let mut line_number = 0;
	let mut ignored: HashSet<u32> = HashSet::new();
	let mut test_case_number = 0;
	for info in BufReader::new(file).lines() {
		let result = match info {
			Ok(line) => {
				line_number += 1;
				if line.is_empty() || line.starts_with('#') {
					continue;
				}
				test_case_number += 1;
				read_next_info(bitness, line)
			}
			Err(err) => Err(err.to_string()),
		};
		match result {
			Ok(tc) => {
				if let Some(tc) = tc {
					infos.push(tc)
				} else {
					let _ = ignored.insert(test_case_number - 1);
				}
			}
			Err(err) => panic!("Error parsing formatter test case file '{}', line {}: {}", display_filename, line_number, err),
		}
	}
	(infos, ignored)
}

fn read_next_info(bitness: u32, line: String) -> Result<Option<InstructionInfo>, String> {
	let parts: Vec<_> = line.split(',').collect();
	let options = match parts.len() {
		2 => 0,
		3 => to_decoder_options(parts[2])?,
		_ => return Err(String::from("Invalid number of commas")),
	};
	let hex_bytes = parts[0].trim();
	if is_ignored_code(parts[1].trim()) {
		return Ok(None);
	}
	let code = to_code(parts[1].trim())?;
	let ip = get_default_ip(bitness);
	Ok(Some(InstructionInfo { bitness, hex_bytes: String::from(hex_bytes), ip, code, options }))
}

pub(super) fn get_formatted_lines(bitness: u32, dir: &str, file_part: &str) -> Vec<String> {
	let mut filename = get_formatter_unit_tests_dir();
	filename.push(dir);
	filename.push(format!("Test{}_{}.txt", bitness, file_part));
	super::get_lines_ignore_comments(filename.as_path())
}
