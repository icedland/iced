# SPDX-License-Identifier: MIT
# Copyright (C) 2018-present iced project and contributors

import pytest
from iced_x86 import *

FMT_SYNTAXES = [
	FormatterSyntax.GAS,
	FormatterSyntax.INTEL,
	FormatterSyntax.MASM,
	FormatterSyntax.NASM,
]

def test_invalid_syntax():
	with pytest.raises(ValueError):
		Formatter(0x12345)

@pytest.mark.parametrize("syntax", FMT_SYNTAXES)
def test_invalid_op_access_arg(syntax):
	instr = Decoder(64, b"\x62\xF2\x4F\xDD\x72\x50\x01").decode()
	formatter = Formatter(syntax)
	with pytest.raises(ValueError):
		formatter.op_access(instr, 100)

@pytest.mark.parametrize("syntax", FMT_SYNTAXES)
def test_invalid_get_instruction_operand_arg(syntax):
	instr = Decoder(64, b"\x62\xF2\x4F\xDD\x72\x50\x01").decode()
	formatter = Formatter(syntax)
	with pytest.raises(ValueError):
		formatter.get_instruction_operand(instr, 100)

@pytest.mark.parametrize("syntax", FMT_SYNTAXES)
def test_invalid_get_formatter_operand_arg(syntax):
	instr = Decoder(64, b"\x62\xF2\x4F\xDD\x72\x50\x01").decode()
	formatter = Formatter(syntax)
	with pytest.raises(ValueError):
		formatter.get_formatter_operand(instr, 100)

@pytest.mark.parametrize("syntax", FMT_SYNTAXES)
def test_invalid_format_operand_arg(syntax):
	instr = Decoder(64, b"\x62\xF2\x4F\xDD\x72\x50\x01").decode()
	formatter = Formatter(syntax)
	with pytest.raises(ValueError):
		formatter.format_operand(instr, 100)

@pytest.mark.parametrize("syntax", FMT_SYNTAXES)
def test_number_base(syntax):
	for base in range(0, 20):
		formatter = Formatter(syntax)
		if base == 2 or base == 8 or base == 10 or base == 16:
			assert formatter.number_base == 16
			formatter.number_base = base
			assert formatter.number_base == base
		else:
			assert formatter.number_base == 16
			with pytest.raises(ValueError):
				formatter.number_base = base
			assert formatter.number_base == 16

@pytest.mark.parametrize("syntax", FMT_SYNTAXES)
def test_options_props(syntax):
	formatter = Formatter(syntax)

	formatter.uppercase_prefixes = True
	formatter.uppercase_mnemonics = True
	formatter.uppercase_registers = True
	formatter.uppercase_keywords = True
	formatter.uppercase_decorators = True
	formatter.uppercase_all = True
	formatter.first_operand_char_index = 10
	formatter.tab_size = 4
	formatter.space_after_operand_separator = True
	formatter.space_after_memory_bracket = True
	formatter.space_between_memory_add_operators = True
	formatter.space_between_memory_mul_operators = True
	formatter.scale_before_index = True
	formatter.always_show_scale = True
	formatter.always_show_segment_register = True
	formatter.show_zero_displacements = True
	formatter.hex_prefix = "0X"
	formatter.hex_suffix = "H"
	formatter.hex_digit_group_size = 5
	formatter.decimal_prefix = "0D"
	formatter.decimal_suffix = "D"
	formatter.decimal_digit_group_size = 6
	formatter.octal_prefix = "0O"
	formatter.octal_suffix = "O"
	formatter.octal_digit_group_size = 7
	formatter.binary_prefix = "0B"
	formatter.binary_suffix = "B"
	formatter.binary_digit_group_size = 8
	formatter.digit_separator = "`"
	formatter.leading_zeroes = True
	formatter.uppercase_hex = False
	formatter.small_hex_numbers_in_decimal = False
	formatter.add_leading_zero_to_hex_numbers = False
	formatter.number_base = 8
	formatter.branch_leading_zeroes = False
	formatter.signed_immediate_operands = True
	formatter.signed_memory_displacements = False
	formatter.displacement_leading_zeroes = True
	formatter.memory_size_options = MemorySizeOptions.NEVER
	formatter.rip_relative_addresses = True
	formatter.show_branch_size = False
	formatter.use_pseudo_ops = False
	formatter.show_symbol_address = True
	formatter.gas_naked_registers = True
	formatter.gas_show_mnemonic_size_suffix = True
	formatter.gas_space_after_memory_operand_comma = True
	formatter.masm_add_ds_prefix32 = False
	formatter.masm_symbol_displ_in_brackets = False
	formatter.masm_displ_in_brackets = False
	formatter.nasm_show_sign_extended_immediate_size = True
	formatter.prefer_st0 = True
	formatter.show_useless_prefixes = True
	formatter.cc_b = CC_b.C
	formatter.cc_ae = CC_ae.NB
	formatter.cc_e = CC_e.Z
	formatter.cc_ne = CC_ne.NZ
	formatter.cc_be = CC_be.NA
	formatter.cc_a = CC_a.NBE
	formatter.cc_p = CC_p.PE
	formatter.cc_np = CC_np.PO
	formatter.cc_l = CC_l.NGE
	formatter.cc_ge = CC_ge.NL
	formatter.cc_le = CC_le.NG
	formatter.cc_g = CC_g.NLE

	assert formatter.uppercase_prefixes
	assert formatter.uppercase_mnemonics
	assert formatter.uppercase_registers
	assert formatter.uppercase_keywords
	assert formatter.uppercase_decorators
	assert formatter.uppercase_all
	assert formatter.first_operand_char_index == 10
	assert formatter.tab_size == 4
	assert formatter.space_after_operand_separator
	assert formatter.space_after_memory_bracket
	assert formatter.space_between_memory_add_operators
	assert formatter.space_between_memory_mul_operators
	assert formatter.scale_before_index
	assert formatter.always_show_scale
	assert formatter.always_show_segment_register
	assert formatter.show_zero_displacements
	assert formatter.hex_prefix == "0X"
	assert formatter.hex_suffix == "H"
	assert formatter.hex_digit_group_size == 5
	assert formatter.decimal_prefix == "0D"
	assert formatter.decimal_suffix == "D"
	assert formatter.decimal_digit_group_size == 6
	assert formatter.octal_prefix == "0O"
	assert formatter.octal_suffix == "O"
	assert formatter.octal_digit_group_size == 7
	assert formatter.binary_prefix == "0B"
	assert formatter.binary_suffix == "B"
	assert formatter.binary_digit_group_size == 8
	assert formatter.digit_separator == "`"
	assert formatter.leading_zeroes
	assert not formatter.uppercase_hex
	assert not formatter.small_hex_numbers_in_decimal
	assert not formatter.add_leading_zero_to_hex_numbers
	assert formatter.number_base == 8
	assert not formatter.branch_leading_zeroes
	assert formatter.signed_immediate_operands
	assert not formatter.signed_memory_displacements
	assert formatter.displacement_leading_zeroes
	assert formatter.memory_size_options == MemorySizeOptions.NEVER
	assert formatter.rip_relative_addresses
	assert not formatter.show_branch_size
	assert not formatter.use_pseudo_ops
	assert formatter.show_symbol_address
	assert formatter.gas_naked_registers
	assert formatter.gas_show_mnemonic_size_suffix
	assert formatter.gas_space_after_memory_operand_comma
	assert not formatter.masm_add_ds_prefix32
	assert not formatter.masm_symbol_displ_in_brackets
	assert not formatter.masm_displ_in_brackets
	assert formatter.nasm_show_sign_extended_immediate_size
	assert formatter.prefer_st0
	assert formatter.show_useless_prefixes
	assert formatter.cc_b == CC_b.C
	assert formatter.cc_ae == CC_ae.NB
	assert formatter.cc_e == CC_e.Z
	assert formatter.cc_ne == CC_ne.NZ
	assert formatter.cc_be == CC_be.NA
	assert formatter.cc_a == CC_a.NBE
	assert formatter.cc_p == CC_p.PE
	assert formatter.cc_np == CC_np.PO
	assert formatter.cc_l == CC_l.NGE
	assert formatter.cc_ge == CC_ge.NL
	assert formatter.cc_le == CC_le.NG
	assert formatter.cc_g == CC_g.NLE

@pytest.mark.parametrize("syntax", FMT_SYNTAXES)
def test_invalid_memory_size_options_arg(syntax):
	formatter = Formatter(syntax)
	with pytest.raises(ValueError):
		formatter.memory_size_options = 123

@pytest.mark.parametrize("syntax", FMT_SYNTAXES)
def test_invalid_cc_b_arg(syntax):
	formatter = Formatter(syntax)
	with pytest.raises(ValueError):
		formatter.cc_b = 123

@pytest.mark.parametrize("syntax", FMT_SYNTAXES)
def test_invalid_cc_ae_arg(syntax):
	formatter = Formatter(syntax)
	with pytest.raises(ValueError):
		formatter.cc_ae = 123

@pytest.mark.parametrize("syntax", FMT_SYNTAXES)
def test_invalid_cc_e_arg(syntax):
	formatter = Formatter(syntax)
	with pytest.raises(ValueError):
		formatter.cc_e = 123

@pytest.mark.parametrize("syntax", FMT_SYNTAXES)
def test_invalid_cc_ne_arg(syntax):
	formatter = Formatter(syntax)
	with pytest.raises(ValueError):
		formatter.cc_ne = 123

@pytest.mark.parametrize("syntax", FMT_SYNTAXES)
def test_invalid_cc_be_arg(syntax):
	formatter = Formatter(syntax)
	with pytest.raises(ValueError):
		formatter.cc_be = 123

@pytest.mark.parametrize("syntax", FMT_SYNTAXES)
def test_invalid_cc_a_arg(syntax):
	formatter = Formatter(syntax)
	with pytest.raises(ValueError):
		formatter.cc_a = 123

@pytest.mark.parametrize("syntax", FMT_SYNTAXES)
def test_invalid_cc_p_arg(syntax):
	formatter = Formatter(syntax)
	with pytest.raises(ValueError):
		formatter.cc_p = 123

@pytest.mark.parametrize("syntax", FMT_SYNTAXES)
def test_invalid_cc_np_arg(syntax):
	formatter = Formatter(syntax)
	with pytest.raises(ValueError):
		formatter.cc_np = 123

@pytest.mark.parametrize("syntax", FMT_SYNTAXES)
def test_invalid_cc_l_arg(syntax):
	formatter = Formatter(syntax)
	with pytest.raises(ValueError):
		formatter.cc_l = 123

@pytest.mark.parametrize("syntax", FMT_SYNTAXES)
def test_invalid_cc_ge_arg(syntax):
	formatter = Formatter(syntax)
	with pytest.raises(ValueError):
		formatter.cc_ge = 123

@pytest.mark.parametrize("syntax", FMT_SYNTAXES)
def test_invalid_cc_le_arg(syntax):
	formatter = Formatter(syntax)
	with pytest.raises(ValueError):
		formatter.cc_le = 123

@pytest.mark.parametrize("syntax", FMT_SYNTAXES)
def test_invalid_cc_g_arg(syntax):
	formatter = Formatter(syntax)
	with pytest.raises(ValueError):
		formatter.cc_g = 123
