// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::formatter::test_utils::get_formatter_unit_tests_dir;
use crate::formatter::tests::options_test_case_parser::OptionsTestParser;
use crate::formatter::tests::opts_info::*;
use crate::formatter::tests::{filter_removed_code_tests, opts_infos};
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
use crate::Formatter;
#[cfg(feature = "fast_fmt")]
use crate::{SpecializedFormatter, SpecializedFormatterTraitOptions};
use alloc::boxed::Box;
use alloc::string::String;
use alloc::vec::Vec;
use std::collections::HashSet;
use std::fs::File;
use std::io::prelude::*;
use std::io::BufReader;
use std::path::PathBuf;

fn read_lines(filename: PathBuf) -> Vec<String> {
	let display_filename = filename.display().to_string();
	let file = File::open(filename).unwrap_or_else(|_| panic!("Couldn't open file {}", display_filename));
	BufReader::new(file)
		.lines()
		.map(|r| r.unwrap_or_else(|e| panic!("{}", e.to_string())))
		.filter(|line| !line.is_empty() && !line.starts_with('#'))
		.collect()
}

fn read_infos<'a>(
	dir: &str, file_part: &str, options_file: &str, tmp_infos: &'a mut Vec<OptionsInstructionInfo>,
) -> Vec<(&'a OptionsInstructionInfo, String)> {
	let mut ignored: HashSet<u32>;
	let mut opts_filename = get_formatter_unit_tests_dir();
	opts_filename.push(dir);
	opts_filename.push(format!("{}.txt", options_file));
	ignored = HashSet::new();
	tmp_infos.extend(OptionsTestParser::new(opts_filename.as_path(), &mut ignored));
	filter_infos(dir, file_part, tmp_infos, &ignored)
}

fn filter_infos<'a>(
	dir: &str, file_part: &str, all_infos: &'a [OptionsInstructionInfo], ignored: &'_ HashSet<u32>,
) -> Vec<(&'a OptionsInstructionInfo, String)> {
	let mut filename = get_formatter_unit_tests_dir();
	filename.push(dir);
	filename.push(format!("{}.txt", file_part));
	let display_filename = filename.display().to_string();
	let lines = filter_removed_code_tests(read_lines(filename), ignored);
	if lines.len() != all_infos.len() {
		panic!("lines.len() ({}) != all_infos.len() ({}), file: {}", lines.len(), all_infos.len(), display_filename);
	}
	all_infos.iter().zip(lines).map(|a| (a.0, a.1)).collect()
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub(in super::super) fn test_format_file_common(dir: &str, file_part: &str, fmt_factory: fn() -> Box<dyn Formatter>) {
	let (all_infos, ignored): (&[OptionsInstructionInfo], &HashSet<u32>) = {
		let infos = &*opts_infos::COMMON_INFOS;
		(&infos.0, &infos.1)
	};
	let infos = filter_infos(dir, file_part, all_infos, ignored);
	test_format(infos, fmt_factory);
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub(in super::super) fn test_format_file_all(dir: &str, file_part: &str, fmt_factory: fn() -> Box<dyn Formatter>) {
	let (all_infos, ignored): (&[OptionsInstructionInfo], &HashSet<u32>) = {
		let infos = &*opts_infos::ALL_INFOS;
		(&infos.0, &infos.1)
	};
	let infos = filter_infos(dir, file_part, all_infos, ignored);
	test_format(infos, fmt_factory);
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub(in super::super) fn test_format_file(dir: &str, file_part: &str, options_file: &str, fmt_factory: fn() -> Box<dyn Formatter>) {
	let mut tmp_infos: Vec<OptionsInstructionInfo> = Vec::new();
	let infos = read_infos(dir, file_part, options_file, &mut tmp_infos);
	test_format(infos, fmt_factory);
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
fn test_format(infos: Vec<(&OptionsInstructionInfo, String)>, fmt_factory: fn() -> Box<dyn Formatter>) {
	for &(tc, ref formatted_string) in &infos {
		let mut formatter = fmt_factory();
		tc.initialize_options(formatter.options_mut());
		super::simple_format_test(
			tc.bitness,
			&tc.hex_bytes,
			tc.ip,
			tc.code,
			tc.decoder_options,
			tc.line_number,
			formatted_string.as_str(),
			formatter.as_mut(),
			|decoder| tc.initialize_decoder(decoder),
		);
	}
}

#[cfg(feature = "fast_fmt")]
pub(in super::super) fn test_format_file_common_fast<TraitOptions: SpecializedFormatterTraitOptions>(
	dir: &str, file_part: &str, fmt_factory: fn() -> Box<SpecializedFormatter<TraitOptions>>,
) {
	let (all_infos, ignored): (&[OptionsInstructionInfo], &HashSet<u32>) = {
		let infos = &*opts_infos::COMMON_INFOS;
		(&infos.0, &infos.1)
	};
	let infos = filter_infos(dir, file_part, all_infos, ignored);
	test_format_fast(infos, fmt_factory);
}

#[cfg(feature = "fast_fmt")]
pub(in super::super) fn test_format_file_fast<TraitOptions: SpecializedFormatterTraitOptions>(
	dir: &str, file_part: &str, options_file: &str, fmt_factory: fn() -> Box<SpecializedFormatter<TraitOptions>>,
) {
	let mut tmp_infos: Vec<OptionsInstructionInfo> = Vec::new();
	let infos = read_infos(dir, file_part, options_file, &mut tmp_infos);
	test_format_fast(infos, fmt_factory);
}

#[cfg(feature = "fast_fmt")]
fn test_format_fast<TraitOptions: SpecializedFormatterTraitOptions>(
	infos: Vec<(&OptionsInstructionInfo, String)>, fmt_factory: fn() -> Box<SpecializedFormatter<TraitOptions>>,
) {
	for &(tc, ref formatted_string) in &infos {
		let mut formatter = fmt_factory();
		tc.initialize_options_fast(formatter.options_mut());
		super::simple_format_test_fast(
			tc.bitness,
			&tc.hex_bytes,
			tc.ip,
			tc.code,
			tc.decoder_options,
			tc.line_number,
			formatted_string.as_str(),
			formatter.as_mut(),
			|decoder| tc.initialize_decoder(decoder),
		);
	}
}
