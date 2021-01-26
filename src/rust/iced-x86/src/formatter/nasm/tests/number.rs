// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

use super::super::super::tests::number::number_tests;
use super::fmt_factory::create_numbers;

#[test]
fn test_numbers() {
	number_tests(|| create_numbers());
}
