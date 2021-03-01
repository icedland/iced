// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::formatter::nasm::tests::fmt_factory::create_numbers;
use crate::formatter::tests::number::number_tests;

#[test]
fn test_numbers() {
	number_tests(|| create_numbers());
}
