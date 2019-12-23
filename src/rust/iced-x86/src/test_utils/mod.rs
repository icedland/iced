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

mod decoder_constants;
pub(crate) mod from_str_conv;
pub(crate) mod section_file_reader;

use self::decoder_constants::*;
use super::iced_constants::IcedConstants;
use super::Decoder;
use std::path::PathBuf;
use std::{cmp, env};

fn get_unit_tests_base_dir() -> PathBuf {
	let mut path = env::current_exe().expect("Couldn't get the path of the current executable");
	for _ in 0..5 {
		let _ = path.pop();
	}
	path.extend(&["UnitTests", "Intel"]);
	path
}

pub(crate) fn get_decoder_unit_tests_dir() -> PathBuf {
	let mut path = get_unit_tests_base_dir();
	path.push("Decoder");
	path
}

pub(crate) fn get_encoder_unit_tests_dir() -> PathBuf {
	let mut path = get_unit_tests_base_dir();
	path.push("Encoder");
	path
}

pub(crate) fn get_instr_info_unit_tests_dir() -> PathBuf {
	let mut path = get_unit_tests_base_dir();
	path.push("InstructionInfo");
	path
}

pub(crate) fn get_default_ip(bitness: u32) -> u64 {
	match bitness {
		16 => DecoderConstants::DEFAULT_IP16,
		32 => DecoderConstants::DEFAULT_IP32,
		64 => DecoderConstants::DEFAULT_IP64,
		_ => panic!(),
	}
}

pub(crate) fn create_decoder<'a>(bitness: u32, bytes: &'a [u8], options: u32) -> (Decoder<'a>, usize, bool) {
	let mut decoder = Decoder::new(bitness, bytes, options);
	decoder.set_ip(get_default_ip(bitness));
	let len = cmp::min(IcedConstants::MAX_INSTRUCTION_LENGTH as usize, bytes.len());
	(decoder, len, len < bytes.len())
}
