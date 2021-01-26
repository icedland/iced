// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

use super::super::test_utils::get_formatter_unit_tests_dir;
use super::options_test_case_parser::*;
use super::opts_info::OptionsInstructionInfo;
use alloc::vec::Vec;
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
