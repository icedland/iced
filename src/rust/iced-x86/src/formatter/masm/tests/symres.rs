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

use super::super::super::test_utils::from_str_conv::to_vec_u8;
use super::super::super::test_utils::{create_decoder, get_formatter_unit_tests_dir};
use super::super::super::tests::sym_res::symbol_resolver_test;
use super::super::super::*;
use super::fmt_factory::create_resolver;
use super::sym_opts::*;
use super::sym_opts_parser::*;
#[cfg(not(feature = "std"))]
use alloc::boxed::Box;
#[cfg(not(feature = "std"))]
use alloc::string::String;
#[cfg(not(feature = "std"))]
use alloc::vec::Vec;

#[test]
fn symres() {
	symbol_resolver_test("Masm", "SymbolResolverTests", |symbol_resolver| create_resolver(symbol_resolver));
}

struct SymbolResolverImpl {
	flags: u32,
}

impl SymbolResolver for SymbolResolverImpl {
	fn symbol(
		&mut self, _instruction: &Instruction, _operand: u32, instruction_operand: Option<u32>, address: u64, _address_size: u32,
	) -> Option<SymbolResult> {
		if instruction_operand == Some(1) && (self.flags & SymbolTestFlags::SYMBOL) != 0 {
			Some(SymbolResult::with_str_kind_flags(
				address,
				"symbol",
				FormatterTextKind::Data,
				if (self.flags & SymbolTestFlags::SIGNED) != 0 { SymbolFlags::SIGNED } else { SymbolFlags::NONE },
			))
		} else {
			None
		}
	}
}

#[test]
fn symbol_options() {
	let mut path = get_formatter_unit_tests_dir();
	path.push("Masm");
	path.push("SymbolOptions.txt");
	for tc in SymbolOptionsTestParser::new(&path) {
		let bytes = to_vec_u8(&tc.hex_bytes).unwrap();
		let mut decoder = create_decoder(tc.bitness, &bytes, DecoderOptions::NONE).0;
		let instruction = decoder.decode();

		let symbol_resolver = Box::new(SymbolResolverImpl { flags: tc.flags });
		let mut formatter = create_resolver(symbol_resolver);
		formatter.options_mut().set_masm_symbol_displ_in_brackets((tc.flags & SymbolTestFlags::SYMBOL_DISPL_IN_BRACKETS) != 0);
		formatter.options_mut().set_masm_displ_in_brackets((tc.flags & SymbolTestFlags::DISPL_IN_BRACKETS) != 0);
		formatter.options_mut().set_rip_relative_addresses((tc.flags & SymbolTestFlags::RIP) != 0);
		formatter.options_mut().set_show_zero_displacements((tc.flags & SymbolTestFlags::SHOW_ZERO_DISPLACEMENTS) != 0);
		formatter.options_mut().set_masm_add_ds_prefix32((tc.flags & SymbolTestFlags::NO_ADD_DS_PREFIX32) == 0);

		let mut output = String::new();
		formatter.format(&instruction, &mut output);
		assert_eq!(tc.formatted_string, output);
	}
}
