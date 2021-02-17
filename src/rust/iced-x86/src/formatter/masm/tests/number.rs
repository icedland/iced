// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use super::super::super::tests::number::number_tests;
use super::fmt_factory::create_numbers;

#[test]
fn test_numbers() {
	number_tests(|| create_numbers());
}
