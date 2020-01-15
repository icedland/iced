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

use super::*;
use std::{cmp, u32};

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
	/// Use upper case hex digits
	pub upper_case_hex: bool,
	/// Small hex numbers (-9 .. 9) are shown in decimal
	pub small_hex_numbers_in_decimal: bool,
	/// Add a leading zero to hex numbers if there's no prefix and the number starts with hex digits `A-F`
	pub add_leading_zero_to_hex_numbers: bool,
	/// If `true`, add leading zeroes to numbers, eg. `1h` vs `00000001h`
	pub leading_zeroes: bool,
	/// If `true`, the number is signed, and if `false` it's an unsigned number
	pub signed_number: bool,
	/// Sign extend the number to the real size (16-bit, 32-bit, 64-bit), eg. `mov al,[eax+12h]` vs `mov al,[eax+00000012h]`
	pub sign_extend_immediate: bool,
}

impl<'a> NumberFormattingOptions<'a> {
	/// Creates options used when formatting immediate values
	///
	/// # Arguments
	///
	/// * `options`: Formatter options to use
	#[inline]
	#[cfg_attr(has_must_use, must_use)]
	pub fn with_immediate(options: &'a FormatterOptions) -> Self {
		NumberFormattingOptions::new(options, options.leading_zeroes(), options.signed_immediate_operands(), false)
	}

	/// Creates options used when formatting displacements
	///
	/// # Arguments
	///
	/// * `options`: Formatter options to use
	#[inline]
	#[cfg_attr(has_must_use, must_use)]
	pub fn with_displacement(options: &'a FormatterOptions) -> Self {
		NumberFormattingOptions::new(
			options,
			options.leading_zeroes(),
			options.signed_memory_displacements(),
			options.sign_extend_memory_displacements(),
		)
	}

	/// Creates options used when formatting branch operands
	///
	/// # Arguments
	///
	/// * `options`: Formatter options to use
	#[inline]
	#[cfg_attr(has_must_use, must_use)]
	pub fn with_branch(options: &'a FormatterOptions) -> Self {
		NumberFormattingOptions::new(options, options.branch_leading_zeroes(), false, false)
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// * `options`: Formatter options to use
	/// * `leading_zeroes`: Add leading zeroes to numbers, eg. `1h` vs `00000001h`
	/// * `signed_number`: Signed numbers if `true`, and unsigned numbers if `false`
	/// * `sign_extend_immediate`: Sign extend the number to the real size (16-bit, 32-bit, 64-bit), eg. `mov al,[eax+12h]` vs `mov al,[eax+00000012h]`
	#[inline]
	#[cfg_attr(has_must_use, must_use)]
	#[cfg_attr(feature = "cargo-clippy", allow(clippy::missing_inline_in_public_items))]
	pub fn new(options: &'a FormatterOptions, leading_zeroes: bool, signed_number: bool, sign_extend_immediate: bool) -> Self {
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
			upper_case_hex: options.upper_case_hex(),
			small_hex_numbers_in_decimal: options.small_hex_numbers_in_decimal(),
			add_leading_zero_to_hex_numbers: options.add_leading_zero_to_hex_numbers(),
			leading_zeroes,
			signed_number,
			sign_extend_immediate,
		}
	}
}
