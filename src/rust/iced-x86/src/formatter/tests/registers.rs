// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::formatter::test_utils::get_formatter_unit_tests_dir;
use crate::formatter::*;
use crate::iced_constants::IcedConstants;
use alloc::boxed::Box;

pub(in super::super) fn register_tests(dir: &str, file_part: &str, fmt_factory: fn() -> Box<dyn Formatter>) {
	let mut filename = get_formatter_unit_tests_dir();
	filename.push(dir);
	filename.push(format!("{}.txt", file_part));
	let lines = super::get_lines_ignore_comments(filename.as_path());
	assert_eq!(lines.len(), IcedConstants::REGISTER_ENUM_COUNT);
	for (expected_register_string, register) in lines.into_iter().zip(Register::values()) {
		{
			let mut formatter = fmt_factory();
			let actual_register_string = formatter.format_register(register);
			assert_eq!(actual_register_string, expected_register_string);
		}
		{
			let mut formatter = fmt_factory();
			formatter.options_mut().set_uppercase_registers(false);
			let actual_register_string = formatter.format_register(register);
			assert_eq!(actual_register_string, expected_register_string.to_lowercase());
		}
		{
			let mut formatter = fmt_factory();
			formatter.options_mut().set_uppercase_registers(true);
			let actual_register_string = formatter.format_register(register);
			assert_eq!(actual_register_string, expected_register_string.to_uppercase());
		}
	}
}
