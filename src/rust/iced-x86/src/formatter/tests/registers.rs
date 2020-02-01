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

use super::super::super::iced_constants::IcedConstants;
use super::super::test_utils::get_formatter_unit_tests_dir;
use super::super::*;
#[cfg(not(feature = "std"))]
use alloc::boxed::Box;
#[cfg(not(feature = "std"))]
use alloc::vec::Vec;
use core::mem;
use std::fs::File;
use std::io::prelude::*;
use std::io::BufReader;

pub(crate) fn register_tests(dir: &str, file_part: &str, fmt_factory: fn() -> Box<Formatter>) {
	let mut filename = get_formatter_unit_tests_dir();
	filename.push(dir);
	filename.push(format!("{}.txt", file_part));
	let display_filename = filename.display().to_string();
	let file = File::open(filename).unwrap_or_else(|_| panic!("Couldn't open file {}", display_filename));
	let lines: Vec<_> = BufReader::new(file)
		.lines()
		.map(|r| r.unwrap_or_else(|e| panic!(e.to_string())))
		.filter(|line| !line.is_empty() && !line.starts_with('#'))
		.collect();
	assert_eq!(IcedConstants::NUMBER_OF_REGISTERS, lines.len());
	for (i, expected_register_string) in lines.into_iter().enumerate() {
		let register: Register = unsafe { mem::transmute(i as u8) };
		{
			let mut formatter = fmt_factory();
			let actual_register_string = formatter.format_register(register);
			assert_eq!(expected_register_string, actual_register_string);
		}
		{
			let mut formatter = fmt_factory();
			formatter.options_mut().set_uppercase_registers(false);
			let actual_register_string = formatter.format_register(register);
			assert_eq!(expected_register_string.to_lowercase(), actual_register_string);
		}
		{
			let mut formatter = fmt_factory();
			formatter.options_mut().set_uppercase_registers(true);
			let actual_register_string = formatter.format_register(register);
			assert_eq!(expected_register_string.to_uppercase(), actual_register_string);
		}
	}
}
