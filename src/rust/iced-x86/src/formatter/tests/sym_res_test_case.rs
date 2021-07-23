// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::formatter::tests::enums::OptionsProps;
use crate::formatter::tests::opt_value::OptionValue;
use crate::*;
use alloc::string::String;
use alloc::vec::Vec;

pub(super) struct SymbolResolverTestCase {
	pub(super) bitness: u32,
	pub(super) hex_bytes: String,
	pub(super) ip: u64,
	pub(super) decoder_options: u32,
	pub(super) line_number: u32,
	pub(super) code: Code,
	pub(super) options: Vec<(OptionsProps, OptionValue)>,
	pub(super) symbol_results: Vec<SymbolResultTestCase>,
}

pub(super) struct SymbolResultTestCase {
	pub(super) address: u64,
	pub(super) symbol_address: u64,
	pub(super) address_size: u32,
	pub(super) flags: u32, // SymbolFlags
	pub(super) memory_size: Option<MemorySize>,
	pub(super) symbol_parts: Vec<String>,
}
