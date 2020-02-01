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
use super::super::super::Formatter;
use super::super::super::NasmFormatter;
#[cfg(not(feature = "std"))]
use alloc::boxed::Box;
#[cfg(not(feature = "std"))]
use alloc::string::String;

fn create_fmt<'a>() -> Box<NasmFormatter<'a>> {
	let mut fmt = Box::new(NasmFormatter::new());
	fmt.options_mut().set_uppercase_hex(false);
	fmt.options_mut().set_hex_prefix(String::from("0x"));
	fmt.options_mut().set_hex_suffix(String::from(""));
	fmt.options_mut().set_octal_prefix(String::from("0o"));
	fmt.options_mut().set_octal_suffix(String::from(""));
	fmt.options_mut().set_binary_prefix(String::from("0b"));
	fmt.options_mut().set_binary_suffix(String::from(""));
	fmt
}

pub(super) fn create_memdefault<'a>() -> Box<NasmFormatter<'a>> {
	let mut fmt = create_fmt();
	fmt.options_mut().set_memory_size_options(MemorySizeOptions::Default);
	fmt.options_mut().set_nasm_show_sign_extended_immediate_size(true);
	fmt.options_mut().set_show_branch_size(false);
	fmt.options_mut().set_rip_relative_addresses(true);
	fmt.options_mut().set_signed_immediate_operands(false);
	fmt.options_mut().set_space_after_operand_separator(false);
	fmt
}

pub(super) fn create_memalways<'a>() -> Box<NasmFormatter<'a>> {
	let mut fmt = create_fmt();
	fmt.options_mut().set_memory_size_options(MemorySizeOptions::Always);
	fmt.options_mut().set_nasm_show_sign_extended_immediate_size(true);
	fmt.options_mut().set_show_branch_size(true);
	fmt.options_mut().set_rip_relative_addresses(false);
	fmt.options_mut().set_signed_immediate_operands(true);
	fmt.options_mut().set_space_after_operand_separator(true);
	fmt
}

pub(super) fn create_memminimum<'a>() -> Box<NasmFormatter<'a>> {
	let mut fmt = create_fmt();
	fmt.options_mut().set_memory_size_options(MemorySizeOptions::Minimum);
	fmt.options_mut().set_nasm_show_sign_extended_immediate_size(true);
	fmt.options_mut().set_show_branch_size(true);
	fmt.options_mut().set_rip_relative_addresses(false);
	fmt.options_mut().set_signed_immediate_operands(true);
	fmt.options_mut().set_space_after_operand_separator(true);
	fmt
}

pub(super) fn create<'a>() -> Box<NasmFormatter<'a>> {
	let mut fmt = create_fmt();
	fmt.options_mut().set_memory_size_options(MemorySizeOptions::Default);
	fmt.options_mut().set_nasm_show_sign_extended_immediate_size(true);
	fmt.options_mut().set_show_branch_size(false);
	fmt.options_mut().set_rip_relative_addresses(true);
	fmt
}

pub(super) fn create_options<'a>() -> Box<NasmFormatter<'a>> {
	let mut fmt = create_fmt();
	fmt.options_mut().set_memory_size_options(MemorySizeOptions::Default);
	fmt.options_mut().set_nasm_show_sign_extended_immediate_size(false);
	fmt.options_mut().set_show_branch_size(false);
	fmt.options_mut().set_rip_relative_addresses(true);
	fmt
}

pub(super) fn create_registers<'a>() -> Box<NasmFormatter<'a>> {
	create_fmt()
}

pub(super) fn create_numbers<'a>() -> Box<NasmFormatter<'a>> {
	let mut formatter = create_fmt();
	formatter.options_mut().set_uppercase_hex(true);
	formatter.options_mut().set_hex_prefix(String::from(""));
	formatter.options_mut().set_hex_suffix(String::from(""));
	formatter.options_mut().set_decimal_prefix(String::from(""));
	formatter.options_mut().set_decimal_suffix(String::from(""));
	formatter.options_mut().set_octal_prefix(String::from(""));
	formatter.options_mut().set_octal_suffix(String::from(""));
	formatter.options_mut().set_binary_prefix(String::from(""));
	formatter.options_mut().set_binary_suffix(String::from(""));
	formatter
}

#[allow(dead_code)] //TODO: REMOVE
pub(super) fn create_resolver<'a>() -> Box<NasmFormatter<'a>> {
	panic!(); //TODO:
}
