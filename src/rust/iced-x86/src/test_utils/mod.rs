// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

mod decoder_constants;
pub(crate) mod from_str_conv;
#[cfg(feature = "instr_info")]
pub(crate) mod section_file_reader;

use crate::iced_constants::IcedConstants;
use crate::test_utils::decoder_constants::*;
use crate::Decoder;
use core::cmp;
use std::path::PathBuf;

fn get_unit_tests_base_dir() -> PathBuf {
	PathBuf::from("../../UnitTests/Intel")
}

pub(crate) fn get_instruction_unit_tests_dir() -> PathBuf {
	let mut path = get_unit_tests_base_dir();
	path.push("Instruction");
	path
}

pub(crate) fn get_decoder_unit_tests_dir() -> PathBuf {
	let mut path = get_unit_tests_base_dir();
	path.push("Decoder");
	path
}

#[cfg(all(feature = "encoder", feature = "op_code_info"))]
pub(crate) fn get_encoder_unit_tests_dir() -> PathBuf {
	let mut path = get_unit_tests_base_dir();
	path.push("Encoder");
	path
}

#[cfg(feature = "instr_info")]
pub(crate) fn get_instr_info_unit_tests_dir() -> PathBuf {
	let mut path = get_unit_tests_base_dir();
	path.push("InstructionInfo");
	path
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm", feature = "fast_fmt"))]
pub(crate) fn get_formatter_unit_tests_dir() -> PathBuf {
	let mut path = get_unit_tests_base_dir();
	path.push("Formatter");
	path
}

pub(crate) fn get_default_ip(bitness: u32) -> u64 {
	match bitness {
		16 => DecoderConstants::DEFAULT_IP16,
		32 => DecoderConstants::DEFAULT_IP32,
		64 => DecoderConstants::DEFAULT_IP64,
		_ => unreachable!(),
	}
}

pub(crate) fn create_decoder(bitness: u32, bytes: &[u8], ip: u64, options: u32) -> (Decoder<'_>, usize, bool) {
	let decoder = Decoder::with_ip(bitness, bytes, ip, options);
	let len = cmp::min(IcedConstants::MAX_INSTRUCTION_LENGTH, bytes.len());
	(decoder, len, len < bytes.len())
}
