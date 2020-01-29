/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

use super::super::super::*;
use super::super::test_utils::get_formatter_unit_tests_dir;
use super::options_test_case_parser::*;
use super::opts_info::*;
use super::opts_infos;
use std::fs::File;
use std::io::prelude::*;
use std::io::BufReader;
use std::path::PathBuf;

fn read_lines(filename: PathBuf) -> Vec<String> {
	let display_filename = filename.display().to_string();
	let file = File::open(filename).unwrap_or_else(|_| panic!("Couldn't open file {}", display_filename));
	BufReader::new(file).lines().map(|r| r.unwrap()).filter(|line| !line.is_empty() && !line.starts_with('#')).collect()
}

pub(crate) fn test_format_file(dir: &str, file_part: &str, options_file: &str, fmt_factory: fn() -> Box<Formatter>) {
	let tmp_infos: Vec<OptionsInstructionInfo>;
	let all_infos = if options_file.is_empty() {
		&*opts_infos::ALL_INFOS
	} else {
		let mut opts_filename = get_formatter_unit_tests_dir();
		opts_filename.push(dir);
		opts_filename.push(format!("{}.txt", options_file));
		tmp_infos = OptionsTestParser::new(opts_filename.as_path()).into_iter().collect();
		&tmp_infos
	};
	let mut filename = get_formatter_unit_tests_dir();
	filename.push(dir);
	filename.push(format!("{}.txt", file_part));
	let display_filename = filename.display().to_string();
	let lines = read_lines(filename);
	if lines.len() != all_infos.len() {
		panic!("lines.len() ({}) != all_infos.len() ({}), file: {}", lines.len(), all_infos.len(), display_filename);
	}
	let infos: Vec<_> = all_infos.iter().zip(lines.into_iter()).map(|a| (a.0, a.1)).collect();
	test_format(infos, fmt_factory);
}

fn test_format(infos: Vec<(&OptionsInstructionInfo, String)>, fmt_factory: fn() -> Box<Formatter>) {
	for &(ref tc, ref formatted_string) in infos.iter() {
		let mut formatter = fmt_factory();
		tc.initialize_options(formatter.options_mut());
		super::simple_format_test(tc.bitness, &tc.hex_bytes, tc.code, 0, formatted_string.as_str(), formatter, |decoder| {
			tc.initialize_decoder(decoder)
		});
	}
}
