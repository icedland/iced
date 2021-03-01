// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::formatter::test_utils::get_formatter_unit_tests_dir;
use crate::formatter::tests::options_test_case_parser::*;
use crate::formatter::tests::opts_info::OptionsInstructionInfo;
use alloc::vec::Vec;
use lazy_static::lazy_static;
use std::collections::HashSet;

lazy_static! {
	pub(super) static ref COMMON_INFOS: (Vec<OptionsInstructionInfo>, HashSet<u32>) = {
		let mut filename = get_formatter_unit_tests_dir();
		filename.push("Options.Common.txt");
		let mut ignored: HashSet<u32> = HashSet::new();
		let v = OptionsTestParser::new(filename.as_path(), &mut ignored).into_iter().collect();
		(v, ignored)
	};
}

lazy_static! {
	pub(super) static ref ALL_INFOS: (Vec<OptionsInstructionInfo>, HashSet<u32>) = {
		let mut filename = get_formatter_unit_tests_dir();
		filename.push("Options.txt");
		let mut ignored: HashSet<u32> = HashSet::new();
		let v = OptionsTestParser::new(filename.as_path(), &mut ignored).into_iter().collect();
		(v, ignored)
	};
}
