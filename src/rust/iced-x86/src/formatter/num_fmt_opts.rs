// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::formatter::*;
use core::cmp;

/// Gets initialized with the default options and can be overridden by a [`FormatterOptionsProvider`]
///
/// [`FormatterOptionsProvider`]: trait.FormatterOptionsProvider.html
#[derive(Debug, Default, Copy, Clone)]
pub struct NumberFormattingOptions<'a> {
	/// Number prefix or an empty string
	pub prefix: &'a str,
	/// Number suffix or an empty string
	pub suffix: &'a str,
	/// Digit separator or an empty string to not use a digit separator
	pub digit_separator: &'a str,
	/// Size of a digit group or 0 to not use a digit separator
	pub digit_group_size: u8,
	/// Number base
	pub number_base: NumberBase,
	/// Use uppercase hex digits
	pub uppercase_hex: bool,
	/// Small hex numbers (-9 .. 9) are shown in decimal
	pub small_hex_numbers_in_decimal: bool,
	/// Add a leading zero to hex numbers if there's no prefix and the number starts with hex digits `A-F`
	pub add_leading_zero_to_hex_numbers: bool,
	/// If `true`, add leading zeros to numbers, eg. `1h` vs `00000001h`
	pub leading_zeros: bool,
	/// If `true`, the number is signed, and if `false` it's an unsigned number
	pub signed_number: bool,
	/// Add leading zeros to displacements
	pub displacement_leading_zeros: bool,
}

impl<'a> NumberFormattingOptions<'a> {
	/// Creates options used when formatting immediate values
	///
	/// # Arguments
	///
	/// * `options`: Formatter options to use
	#[inline]
	#[must_use]
	pub fn with_immediate(options: &'a FormatterOptions) -> Self {
		NumberFormattingOptions::new(options, options.leading_zeros(), options.signed_immediate_operands(), false)
	}

	/// Creates options used when formatting displacements
	///
	/// # Arguments
	///
	/// * `options`: Formatter options to use
	#[inline]
	#[must_use]
	pub fn with_displacement(options: &'a FormatterOptions) -> Self {
		NumberFormattingOptions::new(options, options.leading_zeros(), options.signed_memory_displacements(), options.displacement_leading_zeros())
	}

	/// Creates options used when formatting branch operands
	///
	/// # Arguments
	///
	/// * `options`: Formatter options to use
	#[inline]
	#[must_use]
	pub fn with_branch(options: &'a FormatterOptions) -> Self {
		NumberFormattingOptions::new(options, options.branch_leading_zeros(), false, false)
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// * `options`: Formatter options to use
	/// * `leading_zeros`: Add leading zeros to numbers, eg. `1h` vs `00000001h`
	/// * `signed_number`: Signed numbers if `true`, and unsigned numbers if `false`
	/// * `displacement_leading_zeros`: Add leading zeros to displacements
	#[inline]
	#[must_use]
	#[allow(clippy::missing_inline_in_public_items)]
	pub fn new(options: &'a FormatterOptions, leading_zeros: bool, signed_number: bool, displacement_leading_zeros: bool) -> Self {
		let (digit_group_size, prefix, suffix) = match options.number_base() {
			NumberBase::Hexadecimal => (options.hex_digit_group_size(), options.hex_prefix(), options.hex_suffix()),
			NumberBase::Decimal => (options.decimal_digit_group_size(), options.decimal_prefix(), options.decimal_suffix()),
			NumberBase::Octal => (options.octal_digit_group_size(), options.octal_prefix(), options.octal_suffix()),
			NumberBase::Binary => (options.binary_digit_group_size(), options.binary_prefix(), options.binary_suffix()),
		};
		Self {
			prefix,
			suffix,
			digit_separator: options.digit_separator(),
			digit_group_size: cmp::min(u8::MAX as u32, digit_group_size) as u8,
			number_base: options.number_base(),
			uppercase_hex: options.uppercase_hex(),
			small_hex_numbers_in_decimal: options.small_hex_numbers_in_decimal(),
			add_leading_zero_to_hex_numbers: options.add_leading_zero_to_hex_numbers(),
			leading_zeros,
			signed_number,
			displacement_leading_zeros,
		}
	}
}
