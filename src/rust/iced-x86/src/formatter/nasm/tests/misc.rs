// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::formatter::nasm::tests::fmt_factory;
use crate::formatter::test_utils::from_str_conv::to_vec_u8;
use crate::formatter::test_utils::{create_decoder, get_formatter_unit_tests_dir};
use crate::formatter::tests::misc;
use crate::formatter::tests::mnemonic_opts_parser::MnemonicOptionsTestParser;
use crate::formatter::*;
use alloc::string::String;

#[test]
fn methods_panic_if_invalid_operand_or_instruction_operand() {
	misc::methods_panic_if_invalid_operand_or_instruction_operand(|| fmt_factory::create());
}

#[test]
fn test_op_index() {
	misc::test_op_index(|| fmt_factory::create());
}

#[test]
fn verify_default_formatter_options() {
	let options = FormatterOptions::with_nasm();
	assert!(!options.uppercase_prefixes());
	assert!(!options.uppercase_mnemonics());
	assert!(!options.uppercase_registers());
	assert!(!options.uppercase_keywords());
	assert!(!options.uppercase_decorators());
	assert!(!options.uppercase_all());
	assert_eq!(options.first_operand_char_index(), 0);
	assert_eq!(options.tab_size(), 0);
	assert!(!options.space_after_operand_separator());
	assert!(!options.space_after_memory_bracket());
	assert!(!options.space_between_memory_add_operators());
	assert!(!options.space_between_memory_mul_operators());
	assert!(!options.scale_before_index());
	assert!(!options.always_show_scale());
	assert!(!options.always_show_segment_register());
	assert!(!options.show_zero_displacements());
	assert_eq!(options.hex_prefix(), "");
	assert_eq!(options.hex_suffix(), "h");
	assert_eq!(options.hex_digit_group_size(), 4);
	assert_eq!(options.decimal_prefix(), "");
	assert_eq!(options.decimal_suffix(), "");
	assert_eq!(options.decimal_digit_group_size(), 3);
	assert_eq!(options.octal_prefix(), "");
	assert_eq!(options.octal_suffix(), "o");
	assert_eq!(options.octal_digit_group_size(), 4);
	assert_eq!(options.binary_prefix(), "");
	assert_eq!(options.binary_suffix(), "b");
	assert_eq!(options.binary_digit_group_size(), 4);
	assert_eq!(options.digit_separator(), "");
	assert!(!options.leading_zeros());
	assert!(!options.leading_zeroes());
	assert!(options.uppercase_hex());
	assert!(options.small_hex_numbers_in_decimal());
	assert!(options.add_leading_zero_to_hex_numbers());
	assert_eq!(options.number_base(), NumberBase::Hexadecimal);
	assert!(options.branch_leading_zeros());
	assert!(options.branch_leading_zeroes());
	assert!(!options.signed_immediate_operands());
	assert!(options.signed_memory_displacements());
	assert!(!options.displacement_leading_zeros());
	assert!(!options.displacement_leading_zeroes());
	assert_eq!(options.memory_size_options(), MemorySizeOptions::Default);
	assert!(!options.rip_relative_addresses());
	assert!(options.show_branch_size());
	assert!(options.use_pseudo_ops());
	assert!(!options.show_symbol_address());
	assert!(!options.prefer_st0());
	assert_eq!(options.cc_b(), CC_b::b);
	assert_eq!(options.cc_ae(), CC_ae::ae);
	assert_eq!(options.cc_e(), CC_e::e);
	assert_eq!(options.cc_ne(), CC_ne::ne);
	assert_eq!(options.cc_be(), CC_be::be);
	assert_eq!(options.cc_a(), CC_a::a);
	assert_eq!(options.cc_p(), CC_p::p);
	assert_eq!(options.cc_np(), CC_np::np);
	assert_eq!(options.cc_l(), CC_l::l);
	assert_eq!(options.cc_ge(), CC_ge::ge);
	assert_eq!(options.cc_le(), CC_le::le);
	assert_eq!(options.cc_g(), CC_g::g);
	assert!(!options.show_useless_prefixes());
	assert!(!options.gas_naked_registers());
	assert!(!options.gas_show_mnemonic_size_suffix());
	assert!(!options.gas_space_after_memory_operand_comma());
	assert!(options.masm_add_ds_prefix32());
	assert!(options.masm_symbol_displ_in_brackets());
	assert!(options.masm_displ_in_brackets());
	assert!(!options.nasm_show_sign_extended_immediate_size());
}

#[test]
fn verify_formatter_options() {
	assert_eq!(NasmFormatter::new().options(), &FormatterOptions::with_nasm());
}

#[test]
fn format_mnemonic_options() {
	let mut path = get_formatter_unit_tests_dir();
	path.push("Nasm");
	path.push("MnemonicOptions.txt");
	for tc in MnemonicOptionsTestParser::new(&path) {
		let hex_bytes = to_vec_u8(&tc.hex_bytes).unwrap();
		let mut decoder = create_decoder(tc.bitness, &hex_bytes, tc.ip, DecoderOptions::NONE).0;
		let instruction = decoder.decode();
		assert_eq!(instruction.code(), tc.code);
		let mut formatter = fmt_factory::create();
		let mut output = String::new();
		formatter.format_mnemonic_options(&instruction, &mut output, tc.flags);
		assert_eq!(output, tc.formatted_string);
	}
}
