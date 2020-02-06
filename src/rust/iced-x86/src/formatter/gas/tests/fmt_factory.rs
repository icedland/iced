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

use super::super::super::{Formatter, FormatterOptionsProvider, SymbolResolver};
use super::super::GasFormatter;
#[cfg(not(feature = "std"))]
use alloc::boxed::Box;
#[cfg(not(feature = "std"))]
use alloc::string::String;

fn create_fmt<'a>() -> Box<GasFormatter<'a>> {
	create_fmt2(None, None)
}

fn create_fmt2<'a>(
	symbol_resolver: Option<&'a mut SymbolResolver>, options_provider: Option<&'a mut FormatterOptionsProvider>,
) -> Box<GasFormatter<'a>> {
	let mut fmt = Box::new(GasFormatter::with_options(symbol_resolver, options_provider));
	fmt.options_mut().set_uppercase_hex(false);
	fmt
}

pub(super) fn create_nosuffix<'a>() -> Box<GasFormatter<'a>> {
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

pub(super) fn create_forcesuffix<'a>() -> Box<GasFormatter<'a>> {
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

pub(super) fn create<'a>() -> Box<GasFormatter<'a>> {
	let mut fmt = create_fmt();
	fmt.options_mut().set_gas_show_mnemonic_size_suffix(false);
	fmt.options_mut().set_gas_naked_registers(false);
	fmt.options_mut().set_show_branch_size(false);
	fmt.options_mut().set_rip_relative_addresses(true);
	fmt
}

pub(super) fn create_options<'a>() -> Box<GasFormatter<'a>> {
	let mut fmt = create_fmt();
	fmt.options_mut().set_gas_show_mnemonic_size_suffix(false);
	fmt.options_mut().set_gas_naked_registers(false);
	fmt.options_mut().set_show_branch_size(false);
	fmt.options_mut().set_rip_relative_addresses(true);
	fmt
}

pub(super) fn create_registers<'a>(naked_registers: bool) -> Box<GasFormatter<'a>> {
	let mut fmt = create_fmt();
	fmt.options_mut().set_gas_naked_registers(naked_registers);
	fmt
}

pub(super) fn create_numbers<'a>() -> Box<GasFormatter<'a>> {
	let mut fmt = create_fmt();
	fmt.options_mut().set_uppercase_hex(true);
	fmt.options_mut().set_hex_prefix(String::from(""));
	fmt.options_mut().set_hex_suffix(String::from(""));
	fmt.options_mut().set_decimal_prefix(String::from(""));
	fmt.options_mut().set_decimal_suffix(String::from(""));
	fmt.options_mut().set_octal_prefix(String::from(""));
	fmt.options_mut().set_octal_suffix(String::from(""));
	fmt.options_mut().set_binary_prefix(String::from(""));
	fmt.options_mut().set_binary_suffix(String::from(""));
	fmt
}

pub(super) fn create_resolver<'a>(symbol_resolver: &'a mut SymbolResolver) -> Box<GasFormatter<'a>> {
	let mut fmt = create_fmt2(Some(symbol_resolver), None);
	fmt.options_mut().set_gas_show_mnemonic_size_suffix(false);
	fmt.options_mut().set_gas_naked_registers(false);
	fmt.options_mut().set_show_branch_size(false);
	fmt.options_mut().set_rip_relative_addresses(true);
	fmt
}
