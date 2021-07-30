// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::formatter::tests::enums::OptionsProps;
use crate::formatter::tests::opt_value::OptionValue;
use crate::test_utils::from_str_conv::*;
use alloc::string::String;
use alloc::vec::Vec;

pub(super) fn parse_option(key_value: &str) -> Result<(OptionsProps, OptionValue), String> {
	let kv_parts: Vec<_> = key_value.trim().splitn(2, '=').collect();
	if kv_parts.len() != 2 {
		return Err(format!("Expected key=value: '{}'", key_value));
	}
	let value_str = kv_parts[1].trim();
	let prop = to_options_props(kv_parts[0])?;
	let value = match prop {
		OptionsProps::AddLeadingZeroToHexNumbers
		| OptionsProps::AlwaysShowScale
		| OptionsProps::AlwaysShowSegmentRegister
		| OptionsProps::BranchLeadingZeros
		| OptionsProps::DisplacementLeadingZeros
		| OptionsProps::GasNakedRegisters
		| OptionsProps::GasShowMnemonicSizeSuffix
		| OptionsProps::GasSpaceAfterMemoryOperandComma
		| OptionsProps::LeadingZeros
		| OptionsProps::MasmAddDsPrefix32
		| OptionsProps::NasmShowSignExtendedImmediateSize
		| OptionsProps::PreferST0
		| OptionsProps::RipRelativeAddresses
		| OptionsProps::ScaleBeforeIndex
		| OptionsProps::ShowBranchSize
		| OptionsProps::ShowSymbolAddress
		| OptionsProps::ShowZeroDisplacements
		| OptionsProps::SignedImmediateOperands
		| OptionsProps::SignedMemoryDisplacements
		| OptionsProps::SmallHexNumbersInDecimal
		| OptionsProps::SpaceAfterMemoryBracket
		| OptionsProps::SpaceAfterOperandSeparator
		| OptionsProps::SpaceBetweenMemoryAddOperators
		| OptionsProps::SpaceBetweenMemoryMulOperators
		| OptionsProps::UppercaseAll
		| OptionsProps::UppercaseDecorators
		| OptionsProps::UppercaseHex
		| OptionsProps::UppercaseKeywords
		| OptionsProps::UppercaseMnemonics
		| OptionsProps::UppercasePrefixes
		| OptionsProps::UppercaseRegisters
		| OptionsProps::UsePseudoOps
		| OptionsProps::ShowUselessPrefixes => OptionValue::Boolean(to_boolean(value_str)?),

		OptionsProps::BinaryDigitGroupSize
		| OptionsProps::DecimalDigitGroupSize
		| OptionsProps::FirstOperandCharIndex
		| OptionsProps::HexDigitGroupSize
		| OptionsProps::OctalDigitGroupSize
		| OptionsProps::TabSize => OptionValue::Int32(to_i32(value_str)?),

		OptionsProps::IP => OptionValue::UInt64(to_u64(value_str)?),

		OptionsProps::BinaryPrefix
		| OptionsProps::BinarySuffix
		| OptionsProps::DecimalPrefix
		| OptionsProps::DecimalSuffix
		| OptionsProps::DigitSeparator
		| OptionsProps::HexPrefix
		| OptionsProps::HexSuffix
		| OptionsProps::OctalPrefix
		| OptionsProps::OctalSuffix => OptionValue::String(String::from(if value_str == "<null>" { "" } else { value_str })),

		OptionsProps::MemorySizeOptions => OptionValue::MemorySizeOptions(to_memory_size_options(value_str)?),
		OptionsProps::DecoderOptions => OptionValue::DecoderOptions(to_decoder_options(value_str)?),

		#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
		OptionsProps::NumberBase => OptionValue::NumberBase(to_number_base(value_str)?),
		#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
		OptionsProps::CC_b => OptionValue::CC_b(to_cc_b(value_str)?),
		#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
		OptionsProps::CC_ae => OptionValue::CC_ae(to_cc_ae(value_str)?),
		#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
		OptionsProps::CC_e => OptionValue::CC_e(to_cc_e(value_str)?),
		#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
		OptionsProps::CC_ne => OptionValue::CC_ne(to_cc_ne(value_str)?),
		#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
		OptionsProps::CC_be => OptionValue::CC_be(to_cc_be(value_str)?),
		#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
		OptionsProps::CC_a => OptionValue::CC_a(to_cc_a(value_str)?),
		#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
		OptionsProps::CC_p => OptionValue::CC_p(to_cc_p(value_str)?),
		#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
		OptionsProps::CC_np => OptionValue::CC_np(to_cc_np(value_str)?),
		#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
		OptionsProps::CC_l => OptionValue::CC_l(to_cc_l(value_str)?),
		#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
		OptionsProps::CC_ge => OptionValue::CC_ge(to_cc_ge(value_str)?),
		#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
		OptionsProps::CC_le => OptionValue::CC_le(to_cc_le(value_str)?),
		#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
		OptionsProps::CC_g => OptionValue::CC_g(to_cc_g(value_str)?),

		#[cfg(not(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm")))]
		OptionsProps::NumberBase => OptionValue::Ignore,
		#[cfg(not(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm")))]
		OptionsProps::CC_b => OptionValue::Ignore,
		#[cfg(not(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm")))]
		OptionsProps::CC_ae => OptionValue::Ignore,
		#[cfg(not(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm")))]
		OptionsProps::CC_e => OptionValue::Ignore,
		#[cfg(not(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm")))]
		OptionsProps::CC_ne => OptionValue::Ignore,
		#[cfg(not(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm")))]
		OptionsProps::CC_be => OptionValue::Ignore,
		#[cfg(not(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm")))]
		OptionsProps::CC_a => OptionValue::Ignore,
		#[cfg(not(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm")))]
		OptionsProps::CC_p => OptionValue::Ignore,
		#[cfg(not(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm")))]
		OptionsProps::CC_np => OptionValue::Ignore,
		#[cfg(not(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm")))]
		OptionsProps::CC_l => OptionValue::Ignore,
		#[cfg(not(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm")))]
		OptionsProps::CC_ge => OptionValue::Ignore,
		#[cfg(not(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm")))]
		OptionsProps::CC_le => OptionValue::Ignore,
		#[cfg(not(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm")))]
		OptionsProps::CC_g => OptionValue::Ignore,
	};
	Ok((prop, value))
}
