// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::formatter::gas::GasFormatter;
use crate::formatter::{Formatter, FormatterOptionsProvider, SymbolResolver};
use alloc::boxed::Box;

fn create_fmt() -> Box<GasFormatter> {
	create_fmt2(None, None)
}

fn create_fmt2(symbol_resolver: Option<Box<dyn SymbolResolver>>, options_provider: Option<Box<dyn FormatterOptionsProvider>>) -> Box<GasFormatter> {
	let mut fmt = Box::new(GasFormatter::with_options(symbol_resolver, options_provider));
	fmt.options_mut().set_uppercase_hex(false);
	fmt
}

pub(super) fn create_nosuffix() -> Box<GasFormatter> {
	let mut fmt = create_fmt();
	fmt.options_mut().set_gas_show_mnemonic_size_suffix(false);
	fmt.options_mut().set_gas_naked_registers(false);
	fmt.options_mut().set_show_branch_size(false);
	fmt.options_mut().set_rip_relative_addresses(true);
	fmt.options_mut().set_signed_immediate_operands(false);
	fmt.options_mut().set_space_after_operand_separator(false);
	fmt.options_mut().set_gas_space_after_memory_operand_comma(true);
	fmt
}

pub(super) fn create_forcesuffix() -> Box<GasFormatter> {
	let mut fmt = create_fmt();
	fmt.options_mut().set_gas_show_mnemonic_size_suffix(true);
	fmt.options_mut().set_gas_naked_registers(true);
	fmt.options_mut().set_show_branch_size(true);
	fmt.options_mut().set_rip_relative_addresses(false);
	fmt.options_mut().set_signed_immediate_operands(true);
	fmt.options_mut().set_space_after_operand_separator(true);
	fmt.options_mut().set_gas_space_after_memory_operand_comma(false);
	fmt
}

pub(super) fn create() -> Box<GasFormatter> {
	let mut fmt = create_fmt();
	fmt.options_mut().set_gas_show_mnemonic_size_suffix(false);
	fmt.options_mut().set_gas_naked_registers(false);
	fmt.options_mut().set_show_branch_size(false);
	fmt.options_mut().set_rip_relative_addresses(true);
	fmt
}

pub(super) fn create_options() -> Box<GasFormatter> {
	let mut fmt = create_fmt();
	fmt.options_mut().set_gas_show_mnemonic_size_suffix(false);
	fmt.options_mut().set_gas_naked_registers(false);
	fmt.options_mut().set_show_branch_size(false);
	fmt.options_mut().set_rip_relative_addresses(true);
	fmt
}

pub(super) fn create_registers(naked_registers: bool) -> Box<GasFormatter> {
	let mut fmt = create_fmt();
	fmt.options_mut().set_gas_naked_registers(naked_registers);
	fmt
}

pub(super) fn create_numbers() -> Box<GasFormatter> {
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

pub(super) fn create_resolver(symbol_resolver: Box<dyn SymbolResolver>) -> Box<GasFormatter> {
	let mut fmt = create_fmt2(Some(symbol_resolver), None);
	fmt.options_mut().set_gas_show_mnemonic_size_suffix(false);
	fmt.options_mut().set_gas_naked_registers(false);
	fmt.options_mut().set_show_branch_size(false);
	fmt.options_mut().set_rip_relative_addresses(true);
	fmt
}
