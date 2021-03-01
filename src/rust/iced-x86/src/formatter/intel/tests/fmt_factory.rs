// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::formatter::enums_shared::MemorySizeOptions;
use crate::formatter::IntelFormatter;
use crate::formatter::{Formatter, FormatterOptionsProvider, SymbolResolver};
use alloc::boxed::Box;

fn create_fmt() -> Box<IntelFormatter> {
	create_fmt2(None, None)
}

fn create_fmt2(symbol_resolver: Option<Box<dyn SymbolResolver>>, options_provider: Option<Box<dyn FormatterOptionsProvider>>) -> Box<IntelFormatter> {
	let mut fmt = Box::new(IntelFormatter::with_options(symbol_resolver, options_provider));
	fmt.options_mut().set_uppercase_hex(false);
	fmt.options_mut().set_hex_prefix("0x");
	fmt.options_mut().set_hex_suffix("");
	fmt.options_mut().set_octal_prefix("0o");
	fmt.options_mut().set_octal_suffix("");
	fmt.options_mut().set_binary_prefix("0b");
	fmt.options_mut().set_binary_suffix("");
	fmt.options_mut().set_space_after_operand_separator(true);
	fmt
}

pub(super) fn create_memdefault() -> Box<IntelFormatter> {
	let mut fmt = create_fmt();
	fmt.options_mut().set_memory_size_options(MemorySizeOptions::Default);
	fmt.options_mut().set_show_branch_size(false);
	fmt.options_mut().set_rip_relative_addresses(true);
	fmt.options_mut().set_signed_immediate_operands(false);
	fmt.options_mut().set_space_after_operand_separator(false);
	fmt
}

pub(super) fn create_memalways() -> Box<IntelFormatter> {
	let mut fmt = create_fmt();
	fmt.options_mut().set_memory_size_options(MemorySizeOptions::Always);
	fmt.options_mut().set_show_branch_size(true);
	fmt.options_mut().set_rip_relative_addresses(false);
	fmt.options_mut().set_signed_immediate_operands(true);
	fmt.options_mut().set_space_after_operand_separator(true);
	fmt
}

pub(super) fn create_memminimum() -> Box<IntelFormatter> {
	let mut fmt = create_fmt();
	fmt.options_mut().set_memory_size_options(MemorySizeOptions::Minimal);
	fmt.options_mut().set_show_branch_size(true);
	fmt.options_mut().set_rip_relative_addresses(false);
	fmt.options_mut().set_signed_immediate_operands(true);
	fmt.options_mut().set_space_after_operand_separator(true);
	fmt
}

pub(super) fn create() -> Box<IntelFormatter> {
	let mut fmt = create_fmt();
	fmt.options_mut().set_memory_size_options(MemorySizeOptions::Always);
	fmt.options_mut().set_show_branch_size(true);
	fmt.options_mut().set_rip_relative_addresses(false);
	fmt
}

pub(super) fn create_options() -> Box<IntelFormatter> {
	let mut fmt = create_fmt();
	fmt.options_mut().set_memory_size_options(MemorySizeOptions::Default);
	fmt.options_mut().set_show_branch_size(false);
	fmt.options_mut().set_rip_relative_addresses(true);
	fmt.options_mut().set_space_after_operand_separator(false);
	fmt
}

pub(super) fn create_registers() -> Box<IntelFormatter> {
	create_fmt()
}

pub(super) fn create_numbers() -> Box<IntelFormatter> {
	let mut fmt = create_fmt();
	fmt.options_mut().set_uppercase_hex(true);
	fmt.options_mut().set_hex_prefix("");
	fmt.options_mut().set_hex_suffix("");
	fmt.options_mut().set_decimal_prefix("");
	fmt.options_mut().set_decimal_suffix("");
	fmt.options_mut().set_octal_prefix("");
	fmt.options_mut().set_octal_suffix("");
	fmt.options_mut().set_binary_prefix("");
	fmt.options_mut().set_binary_suffix("");
	fmt
}

pub(super) fn create_resolver(symbol_resolver: Box<dyn SymbolResolver>) -> Box<IntelFormatter> {
	let mut fmt = create_fmt2(Some(symbol_resolver), None);
	fmt.options_mut().set_memory_size_options(MemorySizeOptions::Default);
	fmt.options_mut().set_show_branch_size(false);
	fmt.options_mut().set_rip_relative_addresses(true);
	fmt.options_mut().set_space_after_operand_separator(false);
	fmt
}
