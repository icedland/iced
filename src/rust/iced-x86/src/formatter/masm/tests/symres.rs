// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::formatter::masm::tests::fmt_factory::create_resolver;
use crate::formatter::masm::tests::sym_opts::*;
use crate::formatter::masm::tests::sym_opts_parser::*;
use crate::formatter::test_utils::from_str_conv::to_vec_u8;
use crate::formatter::test_utils::{create_decoder, get_formatter_unit_tests_dir};
use crate::formatter::tests::sym_res::symbol_resolver_test;
use crate::formatter::*;
use alloc::boxed::Box;
use alloc::string::String;

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
	) -> Option<SymbolResult<'_>> {
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
		let mut decoder = create_decoder(tc.bitness, &bytes, tc.ip, DecoderOptions::NONE).0;
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
		assert_eq!(output, tc.formatted_string);
	}
}
