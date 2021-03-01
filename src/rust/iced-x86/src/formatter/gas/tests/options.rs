// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::formatter::gas::tests::fmt_factory;
use crate::formatter::tests::options::{test_format_file, test_format_file_all, test_format_file_common};

#[test]
fn test_options_common() {
	test_format_file_common("Gas", "OptionsResult.Common", || fmt_factory::create_options());
}

#[test]
fn test_options_all() {
	test_format_file_all("Gas", "OptionsResult", || fmt_factory::create_options());
}

#[test]
fn test_options2() {
	test_format_file("Gas", "OptionsResult2", "Options2", || fmt_factory::create_options());
}
