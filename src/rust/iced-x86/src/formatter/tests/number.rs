// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::formatter::test_utils::from_str_conv::*;
use crate::formatter::test_utils::get_formatter_unit_tests_dir;
use crate::formatter::*;
use alloc::boxed::Box;
use alloc::string::String;
use alloc::vec::Vec;
use std::fs::File;
use std::io::prelude::*;
use std::io::BufReader;
use std::path::Path;

enum Number {
	Int8(i8),
	UInt8(u8),
	Int16(i16),
	UInt16(u16),
	Int32(i32),
	UInt32(u32),
	Int64(i64),
	UInt64(u64),
}

fn read_number_file(filename: &Path) -> Vec<Number> {
	let mut vec: Vec<Number> = Vec::new();
	let display_filename = filename.display().to_string();
	let file = File::open(filename).unwrap_or_else(|_| panic!("Couldn't open file {}", display_filename));
	let mut line_number = 0;
	for info in BufReader::new(file).lines() {
		let result = match info {
			Ok(line) => {
				line_number += 1;
				if line.is_empty() || line.starts_with('#') {
					continue;
				}
				read_number_test_case(line, line_number)
			}
			Err(err) => Err(err.to_string()),
		};
		match result {
			Ok(tc) => vec.push(tc),
			Err(err) => panic!("Error parsing number test case file '{}', line {}: {}", display_filename, line_number, err),
		}
	}
	vec
}

fn read_number_test_case(line: String, _line_number: u32) -> Result<Number, String> {
	let elems: Vec<_> = line.split(',').collect();
	if elems.len() != 2 {
		return Err(format!("Invalid number of commas: {}", elems.len() - 1));
	}

	match elems[0].trim() {
		"i8" => Ok(Number::Int8(to_i8(elems[1])?)),
		"u8" => Ok(Number::UInt8(to_u8(elems[1])?)),
		"i16" => Ok(Number::Int16(to_i16(elems[1])?)),
		"u16" => Ok(Number::UInt16(to_u16(elems[1])?)),
		"i32" => Ok(Number::Int32(to_i32(elems[1])?)),
		"u32" => Ok(Number::UInt32(to_u32(elems[1])?)),
		"i64" => Ok(Number::Int64(to_i64(elems[1])?)),
		"u64" => Ok(Number::UInt64(to_u64(elems[1])?)),
		_ => Err(format!("Invalid type: {}", elems[0])),
	}
}

fn read_number_strings_file(filename: &Path) -> Vec<Vec<String>> {
	let mut vec: Vec<Vec<String>> = Vec::new();
	let display_filename = filename.display().to_string();
	let file = File::open(filename).unwrap_or_else(|_| panic!("Couldn't open file {}", display_filename));
	let mut line_number = 0;
	for info in BufReader::new(file).lines() {
		let result = match info {
			Ok(line) => {
				line_number += 1;
				if line.is_empty() || line.starts_with('#') {
					continue;
				}
				read_number_strings(line, line_number)
			}
			Err(err) => Err(err.to_string()),
		};
		match result {
			Ok(tc) => vec.push(tc),
			Err(err) => panic!("Error parsing number strings test case file '{}', line {}: {}", display_filename, line_number, err),
		}
	}
	vec
}

fn read_number_strings(line: String, _line_number: u32) -> Result<Vec<String>, String> {
	let elems: Vec<_> = line.split(',').collect();
	if elems.len() != NUMBER_BASES.len() {
		return Err(format!("Invalid number of commas: {}", elems.len() - 1));
	}

	Ok(elems.into_iter().map(|s| String::from(s.trim())).collect())
}

#[rustfmt::skip]
static NUMBER_BASES: [NumberBase; 4] = [
	NumberBase::Hexadecimal,
	NumberBase::Decimal,
	NumberBase::Octal,
	NumberBase::Binary,
];

pub(in super::super) fn number_tests(fmt_factory: fn() -> Box<dyn Formatter>) {
	assert_eq!(NUMBER_BASES.len(), number_base_len());
	let mut number_filename = get_formatter_unit_tests_dir();
	number_filename.push("Number.txt");
	let numbers = read_number_file(number_filename.as_path());

	let mut strings_filename = get_formatter_unit_tests_dir();
	strings_filename.push("NumberTests.txt");
	let formatted_numbers = read_number_strings_file(strings_filename.as_path());

	if numbers.len() != formatted_numbers.len() {
		panic!("Files don't have the same amount of lines: {} != {}", numbers.len(), formatted_numbers.len());
	}

	for (number, formatted_strings) in numbers.into_iter().zip(formatted_numbers.into_iter()) {
		assert_eq!(formatted_strings.len(), NUMBER_BASES.len());
		for (&base, formatted_string) in NUMBER_BASES.iter().zip(formatted_strings.iter().map(String::as_str)) {
			let mut formatter = fmt_factory();
			formatter.options_mut().set_number_base(base);
			let cloned_options = formatter.options().clone();
			let number_options = NumberFormattingOptions::with_immediate(&cloned_options);
			#[rustfmt::skip]
			let (s1, s2) = match number {
				Number::Int8(value)   => (String::from(formatter.format_i8(value)),  String::from(formatter.format_i8_options(value, &number_options))),
				Number::UInt8(value)  => (String::from(formatter.format_u8(value)),  String::from(formatter.format_u8_options(value, &number_options))),
				Number::Int16(value)  => (String::from(formatter.format_i16(value)), String::from(formatter.format_i16_options(value, &number_options))),
				Number::UInt16(value) => (String::from(formatter.format_u16(value)), String::from(formatter.format_u16_options(value, &number_options))),
				Number::Int32(value)  => (String::from(formatter.format_i32(value)), String::from(formatter.format_i32_options(value, &number_options))),
				Number::UInt32(value) => (String::from(formatter.format_u32(value)), String::from(formatter.format_u32_options(value, &number_options))),
				Number::Int64(value)  => (String::from(formatter.format_i64(value)), String::from(formatter.format_i64_options(value, &number_options))),
				Number::UInt64(value) => (String::from(formatter.format_u64(value)), String::from(formatter.format_u64_options(value, &number_options))),
			};
			assert_eq!(s1, formatted_string);
			assert_eq!(s2, formatted_string);
		}
	}
}
