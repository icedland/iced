// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::decoder::tests::decoder_mem_test_case::*;
use crate::decoder::tests::decoder_test_case::*;
use crate::decoder::tests::enums::DecoderTestOptions;
use crate::decoder::tests::test_cases::*;
use crate::test_utils::from_str_conv::{is_ignored_code, to_code};
use crate::test_utils::*;
use crate::*;
use alloc::string::String;
use alloc::vec::Vec;
use lazy_static::lazy_static;
use std::collections::HashSet;
use std::fs::File;
use std::io::prelude::*;
use std::io::BufReader;

pub(crate) struct DecoderTestInfo {
	bitness: u32,
	code: Code,
	hex_bytes: String,
	ip: u64,
	#[allow(dead_code)]
	encoded_hex_bytes: String,
	decoder_options: u32,
	#[allow(dead_code)]
	decoder_test_options: u32,
}

impl DecoderTestInfo {
	pub(crate) fn bitness(&self) -> u32 {
		self.bitness
	}
	pub(crate) fn code(&self) -> Code {
		self.code
	}
	pub(crate) fn hex_bytes(&self) -> &str {
		&self.hex_bytes
	}
	pub(crate) fn ip(&self) -> u64 {
		self.ip
	}
	#[cfg(feature = "encoder")]
	pub(crate) fn encoded_hex_bytes(&self) -> &str {
		&self.encoded_hex_bytes
	}
	pub(crate) fn decoder_options(&self) -> u32 {
		self.decoder_options
	}
	#[cfg(feature = "op_code_info")]
	pub(crate) fn decoder_test_options(&self) -> u32 {
		self.decoder_test_options
	}
}

lazy_static! {
	static ref NOT_DECODED: HashSet<Code> = read_code_values("Code.NotDecoded.txt");
}
lazy_static! {
	static ref NOT_DECODED32_ONLY: HashSet<Code> = read_code_values("Code.NotDecoded32Only.txt");
}
lazy_static! {
	static ref NOT_DECODED64_ONLY: HashSet<Code> = read_code_values("Code.NotDecoded64Only.txt");
}
lazy_static! {
	static ref CODE32_ONLY: HashSet<Code> = read_code_values("Code.32Only.txt");
}
lazy_static! {
	static ref CODE64_ONLY: HashSet<Code> = read_code_values("Code.64Only.txt");
}

fn read_code_values(name: &str) -> HashSet<Code> {
	let mut filename = get_decoder_unit_tests_dir();
	filename.push(name);
	let display_filename = filename.display();
	let file = File::open(filename.as_path()).unwrap_or_else(|_| panic!("Couldn't open file {}", display_filename));
	let mut h = HashSet::new();
	for (info, line_number) in BufReader::new(file).lines().zip(1..) {
		let err = match info {
			Ok(line) => {
				if line.is_empty() || line.starts_with('#') || is_ignored_code(&line) {
					None
				} else {
					match to_code(&line) {
						Ok(code) => {
							let _ = h.insert(code);
							None
						}
						Err(err) => Some(err),
					}
				}
			}
			Err(err) => Some(err.to_string()),
		};
		if let Some(err) = err {
			panic!("Error parsing Code file '{}', line {}: {}", display_filename, line_number, err);
		}
	}
	h
}

pub(crate) fn not_decoded() -> &'static HashSet<Code> {
	&NOT_DECODED
}

pub(crate) fn not_decoded32_only() -> &'static HashSet<Code> {
	&NOT_DECODED32_ONLY
}

pub(crate) fn not_decoded64_only() -> &'static HashSet<Code> {
	&NOT_DECODED64_ONLY
}

pub(crate) fn code32_only() -> &'static HashSet<Code> {
	&CODE32_ONLY
}

pub(crate) fn code64_only() -> &'static HashSet<Code> {
	&CODE64_ONLY
}

#[cfg(feature = "encoder")]
pub(crate) fn encoder_tests(include_other_tests: bool, include_invalid: bool) -> Vec<DecoderTestInfo> {
	get_tests(include_other_tests, include_invalid, Some(true))
}

pub(crate) fn decoder_tests(include_other_tests: bool, include_invalid: bool) -> Vec<DecoderTestInfo> {
	get_tests(include_other_tests, include_invalid, None)
}

fn get_tests(include_other_tests: bool, include_invalid: bool, can_encode: Option<bool>) -> Vec<DecoderTestInfo> {
	let mut v: Vec<DecoderTestInfo> = Vec::new();
	let bitness_array = [16, 32, 64];
	for bitness in &bitness_array {
		add_tests(&mut v, get_test_cases(*bitness), include_invalid, can_encode);
	}
	if include_other_tests {
		for bitness in &bitness_array {
			add_tests(&mut v, get_misc_test_cases(*bitness), include_invalid, can_encode);
		}
		for bitness in &bitness_array {
			add_tests_mem(&mut v, get_mem_test_cases(*bitness), include_invalid, can_encode);
		}
	}
	v
}

fn add_tests(v: &mut Vec<DecoderTestInfo>, tests: &[DecoderTestCase], include_invalid: bool, can_encode: Option<bool>) {
	for tc in tests {
		if !include_invalid && tc.code == Code::INVALID {
			continue;
		}
		if let Some(can_encode) = can_encode {
			let tc_can_encode = (tc.test_options & DecoderTestOptions::NO_ENCODE) == 0;
			if tc_can_encode != can_encode {
				continue;
			}
		}
		v.push(DecoderTestInfo {
			bitness: tc.bitness,
			code: tc.code,
			hex_bytes: tc.hex_bytes.clone(),
			ip: tc.ip,
			encoded_hex_bytes: tc.encoded_hex_bytes.clone(),
			decoder_options: tc.decoder_options,
			decoder_test_options: tc.test_options,
		});
	}
}

fn add_tests_mem(v: &mut Vec<DecoderTestInfo>, tests: &[DecoderMemoryTestCase], include_invalid: bool, can_encode: Option<bool>) {
	for tc in tests {
		if !include_invalid && tc.code == Code::INVALID {
			continue;
		}
		if let Some(can_encode) = can_encode {
			let tc_can_encode = (tc.test_options & DecoderTestOptions::NO_ENCODE) == 0;
			if tc_can_encode != can_encode {
				continue;
			}
		}
		v.push(DecoderTestInfo {
			bitness: tc.bitness,
			code: tc.code,
			hex_bytes: tc.hex_bytes.clone(),
			ip: tc.ip,
			encoded_hex_bytes: tc.encoded_hex_bytes.clone(),
			decoder_options: tc.decoder_options,
			decoder_test_options: tc.test_options,
		});
	}
}
