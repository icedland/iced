// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use super::super::test_utils::get_instruction_unit_tests_dir;
use super::va_test_case::VirtualAddressTestCase;
use super::va_test_parser::*;
use lazy_static::lazy_static;

lazy_static! {
	pub(crate) static ref VA_TEST_CASES: Vec<VirtualAddressTestCase> = read_va_test_cases();
}

fn read_va_test_cases() -> Vec<VirtualAddressTestCase> {
	let mut path = get_instruction_unit_tests_dir();
	path.push("VirtualAddressTests.txt");
	VirtualAddressTestParser::new(&path).into_iter().collect()
}
