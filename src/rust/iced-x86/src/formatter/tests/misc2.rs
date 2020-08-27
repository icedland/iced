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
#[cfg(feature = "encoder")]
use super::super::encoder::tests::non_decoded_tests;
use super::super::test_utils::from_str_conv::*;
use super::super::*;
use super::instr_infos::*;
#[cfg(not(feature = "std"))]
use alloc::boxed::Box;
#[cfg(not(feature = "std"))]
use alloc::string::String;
use std::fmt::Write;

#[test]
fn make_sure_all_code_values_are_formatted() {
	let mut tested = [0u8; IcedConstants::NUMBER_OF_CODE_VALUES];

	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
	let all_args: [(u32, bool); 6] = [
		(16, false),
		(32, false),
		(64, false),
		(16, true),
		(32, true),
		(64, true),
	];
	for &(bitness, is_misc) in &all_args {
		for info in &get_infos(bitness, is_misc).0 {
			tested[info.code as usize] = 1;
		}
	}
	if cfg!(feature = "encoder") {
		#[cfg(feature = "encoder")] // needed...
		for info in non_decoded_tests::get_tests() {
			tested[info.2.code() as usize] = 1;
		}
	} else {
		for &code in &super::super::super::decoder::tests::NON_DECODED_CODE_VALUES {
			tested[code as usize] = 1;
		}
	}

	let mut sb = String::new();
	let mut missing = 0;
	let code_names = code_names();
	for (i, &t) in tested.iter().enumerate() {
		if t != 1 && !is_ignored_code(code_names[i]) {
			sb.push_str(code_names[i]);
			sb.push(' ');
			missing += 1;
		}
	}
	assert_eq!("Fmt: 0 ins ", format!("Fmt: {} ins {}", missing, sb));
}

#[test]
#[cfg_attr(feature = "cargo-clippy", allow(clippy::same_functions_in_if_condition))]
#[cfg_attr(feature = "cargo-clippy", allow(clippy::if_same_then_else))]
fn display_trait() {
	let bytes = b"\x00\xCE";
	let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	let instr = decoder.decode();
	// The order of cfg checks must be the same as in instruction.rs (see `impl fmt::Display for Instruction`)
	let expected = if cfg!(feature = "masm") {
		"add dh,cl"
	} else if cfg!(feature = "nasm") {
		"add dh,cl"
	} else if cfg!(feature = "intel") {
		"add dh,cl"
	} else if cfg!(feature = "gas") {
		"add %cl,%dh"
	} else if cfg!(feature = "fast_fmt") {
		"add dh,cl"
	} else {
		unreachable!()
	};
	let actual = format!("{}", instr);
	assert_eq!(expected, actual);
	let actual = instr.to_string();
	assert_eq!(expected, actual);
	let mut actual = String::new();
	write!(&mut actual, "{}", instr).unwrap();
	assert_eq!(expected, actual);
}
