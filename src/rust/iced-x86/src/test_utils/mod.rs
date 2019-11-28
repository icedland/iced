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

use self::decoder_constants::*;
use std::env;
use std::path::PathBuf;

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

pub(crate) fn get_default_ip(bitness: u32) -> u64 {
	match bitness {
		16 => DecoderConstants::DEFAULT_IP16,
		32 => DecoderConstants::DEFAULT_IP32,
		64 => DecoderConstants::DEFAULT_IP64,
		_ => panic!(),
	}
}
