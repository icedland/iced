// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::*;
use alloc::string::String;

#[allow(dead_code)]
pub(crate) struct DecoderMemoryTestCase {
	pub(crate) bitness: u32,
	pub(crate) hex_bytes: String,
	pub(crate) ip: u64,
	pub(crate) code: Code,
	pub(crate) register: Register,
	pub(crate) prefix_segment: Register,
	pub(crate) segment: Register,
	pub(crate) base_register: Register,
	pub(crate) index_register: Register,
	pub(crate) scale: u32,
	pub(crate) displacement: u64,
	pub(crate) displ_size: u32,
	pub(crate) constant_offsets: ConstantOffsets,
	pub(crate) encoded_hex_bytes: String,
	pub(crate) decoder_options: u32,
	pub(crate) line_number: u32,
	pub(crate) test_options: u32,
}
