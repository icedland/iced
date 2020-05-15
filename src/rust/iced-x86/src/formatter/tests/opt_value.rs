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

use super::super::super::*;
use super::enums::OptionsProps;
#[cfg(not(feature = "std"))]
use alloc::string::String;
#[cfg(not(feature = "std"))]
use alloc::vec::Vec;

#[allow(non_camel_case_types)]
pub(in super::super) enum OptionValue {
	Boolean(bool),
	Int32(i32),
	UInt64(u64),
	String(String),
	MemorySizeOptions(MemorySizeOptions),
	NumberBase(NumberBase),
	CC_b(CC_b),
	CC_ae(CC_ae),
	CC_e(CC_e),
	CC_ne(CC_ne),
	CC_be(CC_be),
	CC_a(CC_a),
	CC_p(CC_p),
	CC_np(CC_np),
	CC_l(CC_l),
	CC_ge(CC_ge),
	CC_le(CC_le),
	CC_g(CC_g),
}

impl OptionValue {
	fn to_bool(&self) -> bool {
		if let &OptionValue::Boolean(value) = self {
			value
		} else {
			panic!()
		}
	}

	fn to_i32_as_u32(&self) -> u32 {
		if let &OptionValue::Int32(value) = self {
			if value <= 0 {
				0
			} else {
				value as u32
			}
		} else {
			panic!()
		}
	}

	pub(super) fn to_u64(&self) -> u64 {
		if let &OptionValue::UInt64(value) = self {
			value
		} else {
			panic!()
		}
	}

	fn to_str(&self) -> String {
		if let &OptionValue::String(ref value) = self {
			value.clone()
		} else {
			panic!()
		}
	}

	fn to_memory_size_options(&self) -> MemorySizeOptions {
		if let &OptionValue::MemorySizeOptions(value) = self {
			value
		} else {
			panic!()
		}
	}

	fn to_number_base(&self) -> NumberBase {
		if let &OptionValue::NumberBase(value) = self {
			value
		} else {
			panic!()
		}
	}

	fn to_cc_b(&self) -> CC_b {
		if let &OptionValue::CC_b(value) = self {
			value
		} else {
			panic!()
		}
	}

	fn to_cc_ae(&self) -> CC_ae {
		if let &OptionValue::CC_ae(value) = self {
			value
		} else {
			panic!()
		}
	}

	fn to_cc_e(&self) -> CC_e {
		if let &OptionValue::CC_e(value) = self {
			value
		} else {
			panic!()
		}
	}

	fn to_cc_ne(&self) -> CC_ne {
		if let &OptionValue::CC_ne(value) = self {
			value
		} else {
			panic!()
		}
	}

	fn to_cc_be(&self) -> CC_be {
		if let &OptionValue::CC_be(value) = self {
			value
		} else {
			panic!()
		}
	}

	fn to_cc_a(&self) -> CC_a {
		if let &OptionValue::CC_a(value) = self {
			value
		} else {
			panic!()
		}
	}

	fn to_cc_p(&self) -> CC_p {
		if let &OptionValue::CC_p(value) = self {
			value
		} else {
			panic!()
		}
	}

	fn to_cc_np(&self) -> CC_np {
		if let &OptionValue::CC_np(value) = self {
			value
		} else {
			panic!()
		}
	}

	fn to_cc_l(&self) -> CC_l {
		if let &OptionValue::CC_l(value) = self {
			value
		} else {
			panic!()
		}
	}

	fn to_cc_ge(&self) -> CC_ge {
		if let &OptionValue::CC_ge(value) = self {
			value
		} else {
			panic!()
		}
	}

	fn to_cc_le(&self) -> CC_le {
		if let &OptionValue::CC_le(value) = self {
			value
		} else {
			panic!()
		}
	}

	fn to_cc_g(&self) -> CC_g {
		if let &OptionValue::CC_g(value) = self {
			value
		} else {
			panic!()
		}
	}

	pub(super) fn initialize_options(&self, options: &mut FormatterOptions, property: OptionsProps) {
		match property {
			OptionsProps::AddLeadingZeroToHexNumbers => options.set_add_leading_zero_to_hex_numbers(self.to_bool()),
			OptionsProps::AlwaysShowScale => options.set_always_show_scale(self.to_bool()),
			OptionsProps::AlwaysShowSegmentRegister => options.set_always_show_segment_register(self.to_bool()),
			OptionsProps::BinaryDigitGroupSize => options.set_binary_digit_group_size(self.to_i32_as_u32()),
			OptionsProps::BinaryPrefix => options.set_binary_prefix_string(self.to_str()),
			OptionsProps::BinarySuffix => options.set_binary_suffix_string(self.to_str()),
			OptionsProps::BranchLeadingZeroes => options.set_branch_leading_zeroes(self.to_bool()),
			OptionsProps::DecimalDigitGroupSize => options.set_decimal_digit_group_size(self.to_i32_as_u32()),
			OptionsProps::DecimalPrefix => options.set_decimal_prefix_string(self.to_str()),
			OptionsProps::DecimalSuffix => options.set_decimal_suffix_string(self.to_str()),
			OptionsProps::DigitSeparator => options.set_digit_separator_string(self.to_str()),
			OptionsProps::DisplacementLeadingZeroes => options.set_displacement_leading_zeroes(self.to_bool()),
			OptionsProps::FirstOperandCharIndex => options.set_first_operand_char_index(self.to_i32_as_u32()),
			OptionsProps::GasNakedRegisters => options.set_gas_naked_registers(self.to_bool()),
			OptionsProps::GasShowMnemonicSizeSuffix => options.set_gas_show_mnemonic_size_suffix(self.to_bool()),
			OptionsProps::GasSpaceAfterMemoryOperandComma => options.set_gas_space_after_memory_operand_comma(self.to_bool()),
			OptionsProps::HexDigitGroupSize => options.set_hex_digit_group_size(self.to_i32_as_u32()),
			OptionsProps::HexPrefix => options.set_hex_prefix_string(self.to_str()),
			OptionsProps::HexSuffix => options.set_hex_suffix_string(self.to_str()),
			OptionsProps::LeadingZeroes => options.set_leading_zeroes(self.to_bool()),
			OptionsProps::MasmAddDsPrefix32 => options.set_masm_add_ds_prefix32(self.to_bool()),
			OptionsProps::MemorySizeOptions => options.set_memory_size_options(self.to_memory_size_options()),
			OptionsProps::NasmShowSignExtendedImmediateSize => options.set_nasm_show_sign_extended_immediate_size(self.to_bool()),
			OptionsProps::NumberBase => options.set_number_base(self.to_number_base()),
			OptionsProps::OctalDigitGroupSize => options.set_octal_digit_group_size(self.to_i32_as_u32()),
			OptionsProps::OctalPrefix => options.set_octal_prefix_string(self.to_str()),
			OptionsProps::OctalSuffix => options.set_octal_suffix_string(self.to_str()),
			OptionsProps::PreferST0 => options.set_prefer_st0(self.to_bool()),
			OptionsProps::RipRelativeAddresses => options.set_rip_relative_addresses(self.to_bool()),
			OptionsProps::ScaleBeforeIndex => options.set_scale_before_index(self.to_bool()),
			OptionsProps::ShowBranchSize => options.set_show_branch_size(self.to_bool()),
			OptionsProps::ShowSymbolAddress => options.set_show_symbol_address(self.to_bool()),
			OptionsProps::ShowZeroDisplacements => options.set_show_zero_displacements(self.to_bool()),
			OptionsProps::SignedImmediateOperands => options.set_signed_immediate_operands(self.to_bool()),
			OptionsProps::SignedMemoryDisplacements => options.set_signed_memory_displacements(self.to_bool()),
			OptionsProps::SmallHexNumbersInDecimal => options.set_small_hex_numbers_in_decimal(self.to_bool()),
			OptionsProps::SpaceAfterMemoryBracket => options.set_space_after_memory_bracket(self.to_bool()),
			OptionsProps::SpaceAfterOperandSeparator => options.set_space_after_operand_separator(self.to_bool()),
			OptionsProps::SpaceBetweenMemoryAddOperators => options.set_space_between_memory_add_operators(self.to_bool()),
			OptionsProps::SpaceBetweenMemoryMulOperators => options.set_space_between_memory_mul_operators(self.to_bool()),
			OptionsProps::TabSize => options.set_tab_size(self.to_i32_as_u32()),
			OptionsProps::UppercaseAll => options.set_uppercase_all(self.to_bool()),
			OptionsProps::UppercaseDecorators => options.set_uppercase_decorators(self.to_bool()),
			OptionsProps::UppercaseHex => options.set_uppercase_hex(self.to_bool()),
			OptionsProps::UppercaseKeywords => options.set_uppercase_keywords(self.to_bool()),
			OptionsProps::UppercaseMnemonics => options.set_uppercase_mnemonics(self.to_bool()),
			OptionsProps::UppercasePrefixes => options.set_uppercase_prefixes(self.to_bool()),
			OptionsProps::UppercaseRegisters => options.set_uppercase_registers(self.to_bool()),
			OptionsProps::UsePseudoOps => options.set_use_pseudo_ops(self.to_bool()),
			OptionsProps::CC_b => options.set_cc_b(self.to_cc_b()),
			OptionsProps::CC_ae => options.set_cc_ae(self.to_cc_ae()),
			OptionsProps::CC_e => options.set_cc_e(self.to_cc_e()),
			OptionsProps::CC_ne => options.set_cc_ne(self.to_cc_ne()),
			OptionsProps::CC_be => options.set_cc_be(self.to_cc_be()),
			OptionsProps::CC_a => options.set_cc_a(self.to_cc_a()),
			OptionsProps::CC_p => options.set_cc_p(self.to_cc_p()),
			OptionsProps::CC_np => options.set_cc_np(self.to_cc_np()),
			OptionsProps::CC_l => options.set_cc_l(self.to_cc_l()),
			OptionsProps::CC_ge => options.set_cc_ge(self.to_cc_ge()),
			OptionsProps::CC_le => options.set_cc_le(self.to_cc_le()),
			OptionsProps::CC_g => options.set_cc_g(self.to_cc_g()),
			OptionsProps::IP => {}
		}
	}

	pub(super) fn initialize_decoder(&self, decoder: &mut Decoder, property: OptionsProps) {
		if property == OptionsProps::IP {
			decoder.set_ip(self.to_u64());
		}
	}
}
