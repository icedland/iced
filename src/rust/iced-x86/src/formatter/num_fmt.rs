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

use super::enums::*;
use super::fmt_opts::*;
use super::num_fmt_opts::*;
#[cfg(not(feature = "std"))]
use alloc::string::String;

struct NumberFormatterFlags;
impl NumberFormatterFlags {
	const NONE: u32 = 0;
	const ADD_MINUS_SIGN: u32 = 0x0000_0001;
	const LEADING_ZEROES: u32 = 0x0000_0002;
	const SMALL_HEX_NUMBERS_IN_DECIMAL: u32 = 0x0000_0004;
}

#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
static SMALL_DECIMAL_VALUES: [&str; NumberFormatter::SMALL_POSITIVE_NUMBER as usize + 1] = [
	"0", "1", "2", "3", "4", "5", "6", "7", "8", "9"
];

#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
#[cfg_attr(feature = "cargo-clippy", allow(clippy::unreadable_literal))]
static DIVS: [u64; 20] = [
	1,
	10,
	100,
	1000,
	10000,
	100000,
	1000000,
	10000000,
	100000000,
	1000000000,
	10000000000,
	100000000000,
	1000000000000,
	10000000000000,
	100000000000000,
	1000000000000000,
	10000000000000000,
	100000000000000000,
	1000000000000000000,
	10000000000000000000,
];

pub(super) struct NumberFormatter {
	sb: String,
}

impl NumberFormatter {
	const SMALL_POSITIVE_NUMBER: u64 = 9;

	pub(super) fn new() -> Self {
		const CAP: usize = 2 +// "0b"
							64 +// 64 bin digits
							(16 - 1); // # digit separators if group size == 4 and digit sep is one char
		Self { sb: String::with_capacity(CAP) }
	}

	#[inline]
	fn write_hexadecimal(
		sb: &mut String, value: u64, digit_group_size: u32, digit_separator: &str, mut digits: u32, upper: bool, leading_zero: bool,
	) {
		if digits == 0 {
			digits = 1;
			let mut tmp = value;
			loop {
				tmp >>= 4;
				if tmp == 0 {
					break;
				}
				digits += 1;
			}
		}

		let hex_high = if upper { 'A' as u32 - 10 } else { 'a' as u32 - 10 };
		if leading_zero && digits < 17 && ((value >> ((digits - 1) << 2)) & 0xF) > 9 {
			digits += 1; // Another 0
		}
		let use_digit_sep = digit_group_size > 0 && !digit_separator.is_empty();
		for i in 0..digits {
			let index = digits - i - 1;
			let digit = if index >= 16 { 0 } else { ((value >> (index << 2)) & 0xF) as u32 };
			if digit > 9 {
				sb.push((digit + hex_high) as u8 as char);
			} else {
				sb.push((digit + '0' as u32) as u8 as char);
			}
			if use_digit_sep && index > 0 && (index % digit_group_size) == 0 {
				sb.push_str(digit_separator);
			}
		}
	}

	#[inline]
	fn write_decimal(sb: &mut String, value: u64, digit_group_size: u32, digit_separator: &str, mut digits: u32) {
		if digits == 0 {
			digits = 1;
			let mut tmp = value;
			loop {
				tmp /= 10;
				if tmp == 0 {
					break;
				}
				digits += 1;
			}
		}

		let use_digit_sep = digit_group_size > 0 && !digit_separator.is_empty();
		for i in 0..digits {
			let index = digits - i - 1;
			if (index as usize) < DIVS.len() {
				let digit = (value / DIVS[index as usize] % 10) as u32;
				sb.push((digit + '0' as u32) as u8 as char);
			} else {
				sb.push('0');
			}
			if use_digit_sep && index > 0 && (index % digit_group_size) == 0 {
				sb.push_str(digit_separator);
			}
		}
	}

	#[inline]
	fn write_octal(sb: &mut String, value: u64, digit_group_size: u32, digit_separator: &str, mut digits: u32, prefix: &str) {
		if digits == 0 {
			digits = 1;
			let mut tmp = value;
			loop {
				tmp >>= 3;
				if tmp == 0 {
					break;
				}
				digits += 1;
			}
		}

		if !prefix.is_empty() {
			// The prefix is part of the number so that a digit separator can be placed
			// between the "prefix" and the rest of the number, eg. "0" + "1234" with
			// digit separator "`" and group size = 2 is "0`12`34" and not "012`34".
			// Other prefixes, eg. "0o" prefix: 0o12`34 and never 0o`12`34.
			if prefix == "0" {
				if digits < 23 && ((value >> ((digits - 1) * 3)) & 7) != 0 {
					digits += 1; // Another 0
				}
			} else {
				sb.push_str(prefix);
			}
		}

		let use_digit_sep = digit_group_size > 0 && !digit_separator.is_empty();
		for i in 0..digits {
			let index = digits - i - 1;
			let digit = if index >= 22 { 0 } else { ((value >> (index * 3)) & 7) as u32 };
			sb.push((digit + '0' as u32) as u8 as char);
			if use_digit_sep && index > 0 && (index % digit_group_size) == 0 {
				sb.push_str(digit_separator);
			}
		}
	}

	#[inline]
	fn write_binary(sb: &mut String, value: u64, digit_group_size: u32, digit_separator: &str, mut digits: u32) {
		if digits == 0 {
			digits = 1;
			let mut tmp = value;
			loop {
				tmp >>= 1;
				if tmp == 0 {
					break;
				}
				digits += 1;
			}
		}

		let use_digit_sep = digit_group_size > 0 && !digit_separator.is_empty();
		for i in 0..digits {
			let index = digits - i - 1;
			let digit = if index >= 64 { 0 } else { ((value >> index) & 1) as u32 };
			sb.push((digit + '0' as u32) as u8 as char);
			if use_digit_sep && index > 0 && (index % digit_group_size) == 0 {
				sb.push_str(digit_separator);
			}
		}
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	fn get_flags(leading_zeroes: bool, small_hex_numbers_in_decimal: bool) -> u32 {
		let mut flags = NumberFormatterFlags::NONE;
		if leading_zeroes {
			flags |= NumberFormatterFlags::LEADING_ZEROES;
		}
		if small_hex_numbers_in_decimal {
			flags |= NumberFormatterFlags::SMALL_HEX_NUMBERS_IN_DECIMAL;
		}
		flags
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn format_i8(&mut self, formatter_options: &FormatterOptions, options: &NumberFormattingOptions, mut value: i8) -> &str {
		let mut flags = NumberFormatter::get_flags(options.leading_zeroes, options.small_hex_numbers_in_decimal);
		if value < 0 {
			flags |= NumberFormatterFlags::ADD_MINUS_SIGN;
			value = -value;
		}
		self.format_unsigned_integer(formatter_options, options, value as u8 as u64, 8, flags)
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn format_i16(&mut self, formatter_options: &FormatterOptions, options: &NumberFormattingOptions, mut value: i16) -> &str {
		let mut flags = NumberFormatter::get_flags(options.leading_zeroes, options.small_hex_numbers_in_decimal);
		if value < 0 {
			flags |= NumberFormatterFlags::ADD_MINUS_SIGN;
			value = -value;
		}
		self.format_unsigned_integer(formatter_options, options, value as u16 as u64, 16, flags)
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn format_i32(&mut self, formatter_options: &FormatterOptions, options: &NumberFormattingOptions, mut value: i32) -> &str {
		let mut flags = NumberFormatter::get_flags(options.leading_zeroes, options.small_hex_numbers_in_decimal);
		if value < 0 {
			flags |= NumberFormatterFlags::ADD_MINUS_SIGN;
			value = -value;
		}
		self.format_unsigned_integer(formatter_options, options, value as u32 as u64, 32, flags)
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn format_i64(&mut self, formatter_options: &FormatterOptions, options: &NumberFormattingOptions, mut value: i64) -> &str {
		let mut flags = NumberFormatter::get_flags(options.leading_zeroes, options.small_hex_numbers_in_decimal);
		if value < 0 {
			flags |= NumberFormatterFlags::ADD_MINUS_SIGN;
			value = -value;
		}
		self.format_unsigned_integer(formatter_options, options, value as u64, 64, flags)
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn format_u8(&mut self, formatter_options: &FormatterOptions, options: &NumberFormattingOptions, value: u8) -> &str {
		self.format_unsigned_integer(
			formatter_options,
			options,
			value as u64,
			8,
			NumberFormatter::get_flags(options.leading_zeroes, options.small_hex_numbers_in_decimal),
		)
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn format_u16(&mut self, formatter_options: &FormatterOptions, options: &NumberFormattingOptions, value: u16) -> &str {
		self.format_unsigned_integer(
			formatter_options,
			options,
			value as u64,
			16,
			NumberFormatter::get_flags(options.leading_zeroes, options.small_hex_numbers_in_decimal),
		)
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn format_u32(&mut self, formatter_options: &FormatterOptions, options: &NumberFormattingOptions, value: u32) -> &str {
		self.format_unsigned_integer(
			formatter_options,
			options,
			value as u64,
			32,
			NumberFormatter::get_flags(options.leading_zeroes, options.small_hex_numbers_in_decimal),
		)
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn format_u64(&mut self, formatter_options: &FormatterOptions, options: &NumberFormattingOptions, value: u64) -> &str {
		self.format_unsigned_integer(
			formatter_options,
			options,
			value,
			64,
			NumberFormatter::get_flags(options.leading_zeroes, options.small_hex_numbers_in_decimal),
		)
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn format_u16_zeroes(
		&mut self, formatter_options: &FormatterOptions, options: &NumberFormattingOptions, value: u16, leading_zeroes: bool,
	) -> &str {
		self.format_unsigned_integer(
			formatter_options,
			options,
			value as u64,
			16,
			NumberFormatter::get_flags(leading_zeroes, options.small_hex_numbers_in_decimal),
		)
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn format_u32_zeroes(
		&mut self, formatter_options: &FormatterOptions, options: &NumberFormattingOptions, value: u32, leading_zeroes: bool,
	) -> &str {
		self.format_unsigned_integer(
			formatter_options,
			options,
			value as u64,
			32,
			NumberFormatter::get_flags(leading_zeroes, options.small_hex_numbers_in_decimal),
		)
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn format_u64_zeroes(
		&mut self, formatter_options: &FormatterOptions, options: &NumberFormattingOptions, value: u64, leading_zeroes: bool,
	) -> &str {
		self.format_unsigned_integer(
			formatter_options,
			options,
			value,
			64,
			NumberFormatter::get_flags(leading_zeroes, options.small_hex_numbers_in_decimal),
		)
	}

	#[cfg_attr(has_must_use, must_use)]
	fn format_unsigned_integer(
		&mut self, formatter_options: &FormatterOptions, options: &NumberFormattingOptions, value: u64, value_size: u32, flags: u32,
	) -> &str {
		self.sb.clear();
		if (flags & NumberFormatterFlags::ADD_MINUS_SIGN) != 0 {
			self.sb.push('-');
		}
		let suffix;
		match options.number_base {
			NumberBase::Hexadecimal => {
				if (flags & NumberFormatterFlags::SMALL_HEX_NUMBERS_IN_DECIMAL) != 0 && (value as usize) < SMALL_DECIMAL_VALUES.len() {
					self.sb.push_str(formatter_options.decimal_prefix());
					self.sb.push_str(SMALL_DECIMAL_VALUES[value as usize]);
					suffix = formatter_options.decimal_suffix();
				} else {
					self.sb.push_str(options.prefix);
					NumberFormatter::write_hexadecimal(
						&mut self.sb,
						value,
						options.digit_group_size as u32,
						options.digit_separator,
						if (flags & NumberFormatterFlags::LEADING_ZEROES) != 0 { (value_size + 3) >> 2 } else { 0 },
						options.upper_case_hex,
						options.add_leading_zero_to_hex_numbers && options.prefix.is_empty(),
					);
					suffix = options.suffix;
				}
			}

			NumberBase::Decimal => {
				self.sb.push_str(options.prefix);
				NumberFormatter::write_decimal(&mut self.sb, value, options.digit_group_size as u32, options.digit_separator, 0);
				suffix = options.suffix;
			}

			NumberBase::Octal => {
				NumberFormatter::write_octal(
					&mut self.sb,
					value,
					options.digit_group_size as u32,
					options.digit_separator,
					if (flags & NumberFormatterFlags::LEADING_ZEROES) != 0 { (value_size + 2) / 3 } else { 0 },
					options.prefix,
				);
				suffix = options.suffix;
			}

			NumberBase::Binary => {
				self.sb.push_str(options.prefix);
				NumberFormatter::write_binary(
					&mut self.sb,
					value,
					options.digit_group_size as u32,
					options.digit_separator,
					if (flags & NumberFormatterFlags::LEADING_ZEROES) != 0 { value_size } else { 0 },
				);
				suffix = options.suffix;
			}
		}

		self.sb.push_str(suffix);
		&self.sb
	}
}
