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

use super::super::super::test_utils::from_str_conv::to_code;
use super::super::super::test_utils::*;
use super::super::super::*;
use std::collections::HashSet;
use std::fs::File;
use std::io::prelude::*;
use std::io::BufReader;
use std::u32;

pub(crate) struct DecoderTestInfo {
	id: u32,
	bitness: i32,
	code: Code,
	hex_bytes: String,
	encoded_hex_bytes: String,
	options: u32,
	can_encode: bool,
}

impl DecoderTestInfo {
	pub(crate) fn id(&self) -> u32 {
		self.id
	}
	pub(crate) fn bitness(&self) -> i32 {
		self.bitness
	}
	pub(crate) fn code(&self) -> Code {
		self.code
	}
	pub(crate) fn hex_bytes(&self) -> &String {
		&self.hex_bytes
	}
	pub(crate) fn encoded_hex_bytes(&self) -> &String {
		&self.encoded_hex_bytes
	}
	pub(crate) fn options(&self) -> u32 {
		self.options
	}
	pub(crate) fn can_encode(&self) -> bool {
		self.can_encode
	}
}

lazy_static! {
	static ref NOT_DECODED: HashSet<Code> = { read_code_values("Code.NotDecoded.txt") };
}
lazy_static! {
	static ref NOT_DECODED32_ONLY: HashSet<Code> = { read_code_values("Code.NotDecoded32Only.txt") };
}
lazy_static! {
	static ref NOT_DECODED64_ONLY: HashSet<Code> = { read_code_values("Code.NotDecoded64Only.txt") };
}
lazy_static! {
	static ref CODE32_ONLY: HashSet<Code> = { read_code_values("Code.32Only.txt") };
}
lazy_static! {
	static ref CODE64_ONLY: HashSet<Code> = { read_code_values("Code.64Only.txt") };
}

fn read_code_values(name: &str) -> HashSet<Code> {
	let mut filename = get_decoder_unit_tests_dir();
	filename.push(name);
	let display_filename = filename.display();
	let file = File::open(filename.as_path()).expect(format!("Couldn't open file {}", display_filename).as_str());
	let mut h = HashSet::new();
	let mut line_number = 0;
	for info in BufReader::new(file).lines() {
		line_number += 1;
		let err = match info {
			Ok(line) => {
				if line.is_empty() || line.starts_with("#") {
					None
				} else {
					match to_code(&line) {
						Ok(code) => {
							let _ = h.insert(code);
							None
						}
						Err(err) => Some(err),
					}
				}
			}
			Err(err) => Some(err.to_string()),
		};
		if let Some(err) = err {
			panic!("Error parsing Code file '{}', line {}: {}", display_filename, line_number, err);
		}
	}
	h
}

pub(crate) fn not_decoded() -> &'static HashSet<Code> {
	&*NOT_DECODED
}

pub(crate) fn not_decoded32_only() -> &'static HashSet<Code> {
	&*NOT_DECODED32_ONLY
}

pub(crate) fn not_decoded64_only() -> &'static HashSet<Code> {
	&*NOT_DECODED64_ONLY
}

pub(crate) fn code32_only() -> &'static HashSet<Code> {
	&*CODE32_ONLY
}

pub(crate) fn code64_only() -> &'static HashSet<Code> {
	&*CODE64_ONLY
}
