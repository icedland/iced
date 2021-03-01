// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::formatter::fast::tests::fmt_factory;
use crate::formatter::tests::options::{test_format_file_common_fast, test_format_file_fast};

#[test]
fn test_options_common() {
	test_format_file_common_fast("Fast", "OptionsResult.Common", fmt_factory::create_options);
}

#[test]
fn test_options2() {
	test_format_file_fast("Fast", "OptionsResult2", "Options2", fmt_factory::create_options);
}
