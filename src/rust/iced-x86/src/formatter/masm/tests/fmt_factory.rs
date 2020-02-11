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

use super::super::super::enums::MemorySizeOptions;
use super::super::super::MasmFormatter;
use super::super::super::{Formatter, FormatterOptionsProvider, SymbolResolver};
#[cfg(not(feature = "std"))]
use alloc::boxed::Box;
#[cfg(not(feature = "std"))]
use alloc::string::String;

fn create_fmt() -> Box<MasmFormatter> {
	create_fmt2(None, None)
}

fn create_fmt2(symbol_resolver: Option<Box<SymbolResolver>>, options_provider: Option<Box<FormatterOptionsProvider>>) -> Box<MasmFormatter> {
	Box::new(MasmFormatter::with_options(symbol_resolver, options_provider))
}

pub(super) fn create_memdefault() -> Box<MasmFormatter> {
	let mut fmt = create_fmt();
	fmt.options_mut().set_memory_size_options(MemorySizeOptions::Default);
	fmt.options_mut().set_show_branch_size(false);
	fmt.options_mut().set_rip_relative_addresses(true);
	fmt.options_mut().set_signed_immediate_operands(false);
	fmt.options_mut().set_space_after_operand_separator(false);
	fmt.options_mut().set_masm_add_ds_prefix32(true);
	fmt
}

pub(super) fn create_memalways() -> Box<MasmFormatter> {
	let mut fmt = create_fmt();
	fmt.options_mut().set_memory_size_options(MemorySizeOptions::Always);
	fmt.options_mut().set_show_branch_size(true);
	fmt.options_mut().set_rip_relative_addresses(false);
	fmt.options_mut().set_signed_immediate_operands(true);
	fmt.options_mut().set_space_after_operand_separator(true);
	fmt.options_mut().set_masm_add_ds_prefix32(true);
	fmt
}

pub(super) fn create_memminimum() -> Box<MasmFormatter> {
	let mut fmt = create_fmt();
	fmt.options_mut().set_memory_size_options(MemorySizeOptions::Minimum);
	fmt.options_mut().set_show_branch_size(true);
	fmt.options_mut().set_rip_relative_addresses(false);
	fmt.options_mut().set_signed_immediate_operands(true);
	fmt.options_mut().set_space_after_operand_separator(true);
	fmt.options_mut().set_masm_add_ds_prefix32(false);
	fmt
}

pub(super) fn create() -> Box<MasmFormatter> {
	let mut fmt = create_fmt();
	fmt.options_mut().set_memory_size_options(MemorySizeOptions::Always);
	fmt.options_mut().set_show_branch_size(true);
	fmt.options_mut().set_rip_relative_addresses(false);
	fmt
}

pub(super) fn create_options() -> Box<MasmFormatter> {
	let mut fmt = create_fmt();
	fmt.options_mut().set_memory_size_options(MemorySizeOptions::Default);
	fmt.options_mut().set_show_branch_size(false);
	fmt.options_mut().set_rip_relative_addresses(true);
	fmt
}

pub(super) fn create_registers() -> Box<MasmFormatter> {
	create_fmt()
}

pub(super) fn create_numbers() -> Box<MasmFormatter> {
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

pub(super) fn create_resolver(symbol_resolver: Box<SymbolResolver>) -> Box<MasmFormatter> {
	let mut fmt = create_fmt2(Some(symbol_resolver), None);
	fmt.options_mut().set_memory_size_options(MemorySizeOptions::Default);
	fmt.options_mut().set_show_branch_size(false);
	fmt.options_mut().set_rip_relative_addresses(true);
	fmt
}
