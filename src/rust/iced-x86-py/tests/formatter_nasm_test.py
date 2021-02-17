# SPDX-License-Identifier: MIT
# Copyright (C) 2018-present iced project and contributors

from iced_x86 import *

FORMATTER_SYNTAX = FormatterSyntax.NASM

def test_default_options():
	formatter = Formatter(FORMATTER_SYNTAX)

	assert not formatter.uppercase_prefixes
	assert not formatter.uppercase_mnemonics
	assert not formatter.uppercase_registers
	assert not formatter.uppercase_keywords
	assert not formatter.uppercase_decorators
	assert not formatter.uppercase_all
	assert formatter.first_operand_char_index == 0
	assert formatter.tab_size == 0
	assert not formatter.space_after_operand_separator
	assert not formatter.space_after_memory_bracket
	assert not formatter.space_between_memory_add_operators
	assert not formatter.space_between_memory_mul_operators
	assert not formatter.scale_before_index
	assert not formatter.always_show_scale
	assert not formatter.always_show_segment_register
	assert not formatter.show_zero_displacements
	assert formatter.hex_prefix == ""
	assert formatter.hex_suffix == "h"
	assert formatter.hex_digit_group_size == 4
	assert formatter.decimal_prefix == ""
	assert formatter.decimal_suffix == ""
	assert formatter.decimal_digit_group_size == 3
	assert formatter.octal_prefix == ""
	assert formatter.octal_suffix == "o"
	assert formatter.octal_digit_group_size == 4
	assert formatter.binary_prefix == ""
	assert formatter.binary_suffix == "b"
	assert formatter.binary_digit_group_size == 4
	assert formatter.digit_separator == ""
	assert not formatter.leading_zeroes
	assert formatter.uppercase_hex
	assert formatter.small_hex_numbers_in_decimal
	assert formatter.add_leading_zero_to_hex_numbers
	assert formatter.number_base == 16
	assert formatter.branch_leading_zeroes
	assert not formatter.signed_immediate_operands
	assert formatter.signed_memory_displacements
	assert not formatter.displacement_leading_zeroes
	assert formatter.memory_size_options == MemorySizeOptions.DEFAULT
	assert not formatter.rip_relative_addresses
	assert formatter.show_branch_size
	assert formatter.use_pseudo_ops
	assert not formatter.show_symbol_address
	assert not formatter.gas_naked_registers
	assert not formatter.gas_show_mnemonic_size_suffix
	assert not formatter.gas_space_after_memory_operand_comma
	assert formatter.masm_add_ds_prefix32
	assert formatter.masm_symbol_displ_in_brackets
	assert formatter.masm_displ_in_brackets
	assert not formatter.nasm_show_sign_extended_immediate_size
	assert not formatter.prefer_st0
	assert not formatter.show_useless_prefixes
	assert formatter.cc_b == CC_b.B
	assert formatter.cc_ae == CC_ae.AE
	assert formatter.cc_e == CC_e.E
	assert formatter.cc_ne == CC_ne.NE
	assert formatter.cc_be == CC_be.BE
	assert formatter.cc_a == CC_a.A
	assert formatter.cc_p == CC_p.P
	assert formatter.cc_np == CC_np.NP
	assert formatter.cc_l == CC_l.L
	assert formatter.cc_ge == CC_ge.GE
	assert formatter.cc_le == CC_le.LE
	assert formatter.cc_g == CC_g.G

def test_format():
	instr, instr2 = [instr for instr in Decoder(64, b"\x62\xF2\x4F\xDD\x72\x50\x01\xF0\x00\x18")]
	formatter = Formatter(FORMATTER_SYNTAX)

	assert formatter.format(instr) == "vcvtne2ps2bf16 zmm2{k5}{z},zmm6,[rax+4]{1to16}"

	assert formatter.format_mnemonic(instr) == "vcvtne2ps2bf16"
	assert formatter.format_mnemonic(instr, FormatMnemonicOptions.NONE) == "vcvtne2ps2bf16"
	assert formatter.format_mnemonic(instr, FormatMnemonicOptions.NO_MNEMONIC) == ""
	assert formatter.format_mnemonic(instr, FormatMnemonicOptions.NO_PREFIXES) == "vcvtne2ps2bf16"
	assert formatter.format_mnemonic(instr, FormatMnemonicOptions.NO_MNEMONIC | FormatMnemonicOptions.NO_PREFIXES) == ""

	assert formatter.format_mnemonic(instr2) == "lock add"
	assert formatter.format_mnemonic(instr2, FormatMnemonicOptions.NONE) == "lock add"
	assert formatter.format_mnemonic(instr2, FormatMnemonicOptions.NO_MNEMONIC) == "lock"
	assert formatter.format_mnemonic(instr2, FormatMnemonicOptions.NO_PREFIXES) == "add"
	assert formatter.format_mnemonic(instr2, FormatMnemonicOptions.NO_MNEMONIC | FormatMnemonicOptions.NO_PREFIXES) == ""

	assert formatter.operand_count(instr) == 3
	assert formatter.op_access(instr, 0) is None
	assert formatter.get_instruction_operand(instr, 0) == 0
	assert formatter.get_instruction_operand(instr, 1) == 1
	assert formatter.get_instruction_operand(instr, 2) == 2
	assert formatter.get_formatter_operand(instr, 0) == 0
	assert formatter.get_formatter_operand(instr, 1) == 1
	assert formatter.get_formatter_operand(instr, 2) == 2
	assert formatter.format_operand(instr, 0) == "zmm2{k5}{z}"
	assert formatter.format_operand(instr, 1) == "zmm6"
	assert formatter.format_operand(instr, 2) == "[rax+4]{1to16}"
	assert formatter.format_operand_separator(instr) == ","
	assert formatter.format_all_operands(instr) == "zmm2{k5}{z},zmm6,[rax+4]{1to16}"

	assert formatter.format_register(Register.RCX) == "rcx"

	assert formatter.format_i8(-0x12) == "-12h"
	assert formatter.format_i16(-0x1234) == "-1234h"
	assert formatter.format_i32(-0x12345678) == "-12345678h"
	assert formatter.format_i64(-0x123456789ABCDEF0) == "-123456789ABCDEF0h"
	assert formatter.format_u8(0x89) == "89h"
	assert formatter.format_u16(0x89AB) == "89ABh"
	assert formatter.format_u32(0x89ABCDEF) == "89ABCDEFh"
	assert formatter.format_u64(0xFEDCBA9876543210) == "0FEDCBA9876543210h"
