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
use super::super::super::test_utils::create_decoder;
use super::super::super::tests::misc;
use super::super::super::*;
use super::super::info::InstrOpInfo;
use super::super::regs::Registers;
use super::fmt_factory;

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
	const MAX_VALUE: u32 = IcedConstants::NUMBER_OF_REGISTERS as u32 - 1 + Registers::EXTRA_REGISTERS;
	const_assert!(MAX_VALUE < (1 << InstrOpInfo::TEST_REGISTER_BITS));
	const_assert!(MAX_VALUE >= (1 << (InstrOpInfo::TEST_REGISTER_BITS - 1)));
}

#[test]
fn verify_default_formatter_options() {
	let options = FormatterOptions::with_masm();
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
	assert_eq!("", options.hex_prefix());
	assert_eq!("h", options.hex_suffix());
	assert_eq!(4, options.hex_digit_group_size());
	assert_eq!("", options.decimal_prefix());
	assert_eq!("", options.decimal_suffix());
	assert_eq!(3, options.decimal_digit_group_size());
	assert_eq!("", options.octal_prefix());
	assert_eq!("o", options.octal_suffix());
	assert_eq!(4, options.octal_digit_group_size());
	assert_eq!("", options.binary_prefix());
	assert_eq!("b", options.binary_suffix());
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
	assert_eq!(&FormatterOptions::with_masm(), MasmFormatter::new().options());
}

#[test]
fn format_mnemonic_options() {
	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
	let data: [(&[u8], Code, u32, &'static str, u32); 25] = [
		(b"\x10\x08", Code::Adc_rm8_r8, 64, "adc", FormatMnemonicOptions::NONE),
		(b"\x10\x08", Code::Adc_rm8_r8, 64, "adc", FormatMnemonicOptions::NO_PREFIXES),
		(b"\x10\x08", Code::Adc_rm8_r8, 64, "", FormatMnemonicOptions::NO_MNEMONIC),
		(b"\x10\x08", Code::Adc_rm8_r8, 64, "", FormatMnemonicOptions::NO_PREFIXES | FormatMnemonicOptions::NO_MNEMONIC),

		(b"\xF0\x10\x08", Code::Adc_rm8_r8, 64, "lock adc", FormatMnemonicOptions::NONE),
		(b"\xF0\x10\x08", Code::Adc_rm8_r8, 64, "adc", FormatMnemonicOptions::NO_PREFIXES),
		(b"\xF0\x10\x08", Code::Adc_rm8_r8, 64, "lock", FormatMnemonicOptions::NO_MNEMONIC),

		(b"\xF3\x6C", Code::Insb_m8_DX, 64, "rep insb", FormatMnemonicOptions::NONE),
		(b"\xF3\x6C", Code::Insb_m8_DX, 64, "insb", FormatMnemonicOptions::NO_PREFIXES),
		(b"\xF3\x6C", Code::Insb_m8_DX, 64, "rep", FormatMnemonicOptions::NO_MNEMONIC),

		(b"\xF2\xA6", Code::Cmpsb_m8_m8, 64, "repne cmpsb", FormatMnemonicOptions::NONE),
		(b"\xF2\xA6", Code::Cmpsb_m8_m8, 64, "cmpsb", FormatMnemonicOptions::NO_PREFIXES),
		(b"\xF2\xA6", Code::Cmpsb_m8_m8, 64, "repne", FormatMnemonicOptions::NO_MNEMONIC),

		(b"\xF2\xF0\x10\x08", Code::Adc_rm8_r8, 64, "xacquire lock adc", FormatMnemonicOptions::NONE),
		(b"\xF2\xF0\x10\x08", Code::Adc_rm8_r8, 64, "adc", FormatMnemonicOptions::NO_PREFIXES),
		(b"\xF2\xF0\x10\x08", Code::Adc_rm8_r8, 64, "xacquire lock", FormatMnemonicOptions::NO_MNEMONIC),

		(b"\x2E\x70\x00", Code::Jo_rel8_64, 64, "hnt jo", FormatMnemonicOptions::NONE),
		(b"\x2E\x70\x00", Code::Jo_rel8_64, 64, "jo", FormatMnemonicOptions::NO_PREFIXES),
		(b"\x2E\x70\x00", Code::Jo_rel8_64, 64, "hnt", FormatMnemonicOptions::NO_MNEMONIC),

		(b"\xF2\x70\x00", Code::Jo_rel8_64, 64, "bnd jo", FormatMnemonicOptions::NONE),
		(b"\xF2\x70\x00", Code::Jo_rel8_64, 64, "jo", FormatMnemonicOptions::NO_PREFIXES),
		(b"\xF2\x70\x00", Code::Jo_rel8_64, 64, "bnd", FormatMnemonicOptions::NO_MNEMONIC),

		(b"\x3E\xFF\x10", Code::Call_rm64, 64, "notrack call", FormatMnemonicOptions::NONE),
		(b"\x3E\xFF\x10", Code::Call_rm64, 64, "call", FormatMnemonicOptions::NO_PREFIXES),
		(b"\x3E\xFF\x10", Code::Call_rm64, 64, "notrack", FormatMnemonicOptions::NO_MNEMONIC),
	];
	for &(hex_bytes, code, bitness, formatted_string, mnemonic_options) in data.iter() {
		let mut decoder = create_decoder(bitness, hex_bytes, DecoderOptions::NONE).0;
		let instruction = decoder.decode();
		assert_eq!(code, instruction.code());
		let mut formatter = fmt_factory::create();
		let mut output = String::new();
		formatter.format_mnemonic_options(&instruction, &mut output, mnemonic_options);
		assert_eq!(formatted_string, output);
	}
}
