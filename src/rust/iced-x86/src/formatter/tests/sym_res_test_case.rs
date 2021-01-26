// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

use super::super::super::*;
use super::enums::OptionsProps;
use super::opt_value::OptionValue;
#[cfg(not(feature = "std"))]
use alloc::string::String;
#[cfg(not(feature = "std"))]
use alloc::vec::Vec;

pub(super) struct SymbolResolverTestCase {
	pub(super) bitness: u32,
	pub(super) hex_bytes: String,
	pub(super) decoder_options: u32,
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
