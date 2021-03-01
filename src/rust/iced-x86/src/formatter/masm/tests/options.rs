// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::formatter::masm::tests::fmt_factory;
use crate::formatter::tests::options::{test_format_file, test_format_file_all, test_format_file_common};

#[test]
fn test_options_common() {
	test_format_file_common("Masm", "OptionsResult.Common", || fmt_factory::create_options());
}

#[test]
fn test_options_all() {
	test_format_file_all("Masm", "OptionsResult", || fmt_factory::create_options());
}

#[test]
fn test_options2() {
	test_format_file("Masm", "OptionsResult2", "Options2", || fmt_factory::create_options());
}
