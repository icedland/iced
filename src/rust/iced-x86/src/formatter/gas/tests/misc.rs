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

use super::super::super::super::iced_constants::IcedConstants;
use super::super::super::test_utils::from_str_conv::to_vec_u8;
use super::super::super::test_utils::{create_decoder, get_formatter_unit_tests_dir};
use super::super::super::tests::misc;
use super::super::super::tests::mnemonic_opts_parser::MnemonicOptionsTestParser;
use super::super::super::*;
use super::super::info::InstrOpInfo;
use super::super::regs::Registers;
use super::fmt_factory;
#[cfg(not(feature = "std"))]
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
fn register_is_not_too_big() {
	#[allow(dead_code)]
	const MAX_VALUE: u32 = IcedConstants::REGISTER_ENUM_COUNT as u32 - 1 + Registers::EXTRA_REGISTERS;
	const_assert!(MAX_VALUE < (1 << InstrOpInfo::TEST_REGISTER_BITS));
	const_assert!(MAX_VALUE >= (1 << (InstrOpInfo::TEST_REGISTER_BITS - 1)));
}

#[test]
fn verify_default_formatter_options() {
	let options = FormatterOptions::with_gas();
	assert!(!options.uppercase_prefixes());
	assert!(!options.uppercase_mnemonics());
	assert!(!options.uppercase_registers());
	assert!(!options.uppercase_keywords());
	assert!(!options.uppercase_decorators());
	assert!(!options.uppercase_all());
	assert_eq!(0, options.first_operand_char_index());
	assert_eq!(0, options.tab_size());
	assert!(!options.space_after_operand_separator());
	assert!(!options.space_after_memory_bracket());
	assert!(!options.space_between_memory_add_operators());
	assert!(!options.space_between_memory_mul_operators());
	assert!(!options.scale_before_index());
	assert!(!options.always_show_scale());
	assert!(!options.always_show_segment_register());
	assert!(!options.show_zero_displacements());
	assert_eq!("0x", options.hex_prefix());
	assert_eq!("", options.hex_suffix());
	assert_eq!(4, options.hex_digit_group_size());
	assert_eq!("", options.decimal_prefix());
	assert_eq!("", options.decimal_suffix());
	assert_eq!(3, options.decimal_digit_group_size());
	assert_eq!("0", options.octal_prefix());
	assert_eq!("", options.octal_suffix());
	assert_eq!(4, options.octal_digit_group_size());
	assert_eq!("0b", options.binary_prefix());
	assert_eq!("", options.binary_suffix());
	assert_eq!(4, options.binary_digit_group_size());
	assert_eq!("", options.digit_separator());
	assert!(!options.leading_zeroes());
	assert!(options.uppercase_hex());
	assert!(options.small_hex_numbers_in_decimal());
	assert!(options.add_leading_zero_to_hex_numbers());
	assert_eq!(NumberBase::Hexadecimal, options.number_base());
	assert!(options.branch_leading_zeroes());
	assert!(!options.signed_immediate_operands());
	assert!(options.signed_memory_displacements());
	assert!(!options.displacement_leading_zeroes());
	assert_eq!(MemorySizeOptions::Default, options.memory_size_options());
	assert!(!options.rip_relative_addresses());
	assert!(options.show_branch_size());
	assert!(options.use_pseudo_ops());
	assert!(!options.show_symbol_address());
	assert!(!options.prefer_st0());
	assert_eq!(CC_b::b, options.cc_b());
	assert_eq!(CC_ae::ae, options.cc_ae());
	assert_eq!(CC_e::e, options.cc_e());
	assert_eq!(CC_ne::ne, options.cc_ne());
	assert_eq!(CC_be::be, options.cc_be());
	assert_eq!(CC_a::a, options.cc_a());
	assert_eq!(CC_p::p, options.cc_p());
	assert_eq!(CC_np::np, options.cc_np());
	assert_eq!(CC_l::l, options.cc_l());
	assert_eq!(CC_ge::ge, options.cc_ge());
	assert_eq!(CC_le::le, options.cc_le());
	assert_eq!(CC_g::g, options.cc_g());
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
	assert_eq!(&FormatterOptions::with_gas(), GasFormatter::new().options());
}

#[test]
fn format_mnemonic_options() {
	let mut path = get_formatter_unit_tests_dir();
	path.push("Gas");
	path.push("MnemonicOptions.txt");
	for tc in MnemonicOptionsTestParser::new(&path) {
		let hex_bytes = to_vec_u8(&tc.hex_bytes).unwrap();
		let mut decoder = create_decoder(tc.bitness, &hex_bytes, DecoderOptions::NONE).0;
		let instruction = decoder.decode();
		assert_eq!(tc.code, instruction.code());
		let mut formatter = fmt_factory::create();
		let mut output = String::new();
		formatter.format_mnemonic_options(&instruction, &mut output, tc.flags);
		assert_eq!(tc.formatted_string, output);
	}
}
