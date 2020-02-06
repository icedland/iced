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

use super::super::super::test_utils::from_str_conv::*;
use super::enums::OptionsProps;
use super::opt_value::OptionValue;
#[cfg(not(feature = "std"))]
use alloc::string::String;
#[cfg(not(feature = "std"))]
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
		| OptionsProps::BranchLeadingZeroes
		| OptionsProps::DisplacementLeadingZeroes
		| OptionsProps::GasNakedRegisters
		| OptionsProps::GasShowMnemonicSizeSuffix
		| OptionsProps::GasSpaceAfterMemoryOperandComma
		| OptionsProps::LeadingZeroes
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
		| OptionsProps::UsePseudoOps => OptionValue::Boolean(to_boolean(value_str)?),

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
		OptionsProps::NumberBase => OptionValue::NumberBase(to_number_base(value_str)?),
	};
	Ok((prop, value))
}
