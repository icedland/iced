// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::Register;
use alloc::string::String;
use alloc::vec::Vec;

pub(crate) struct VARegisterValue {
	pub(crate) register: Register,
	pub(crate) element_index: usize,
	pub(crate) element_size: usize,
	pub(crate) value: u64,
}

pub(crate) struct VirtualAddressTestCase {
	pub(crate) bitness: u32,
	pub(crate) hex_bytes: String,
	pub(crate) decoder_options: u32,
	pub(crate) operand: i32,
	#[allow(dead_code)]
	pub(crate) used_mem_index: i32,
	pub(crate) element_index: usize,
	pub(crate) expected_value: u64,
	pub(crate) register_values: Vec<VARegisterValue>,
}
