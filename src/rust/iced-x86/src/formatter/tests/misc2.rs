// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#[cfg(feature = "encoder")]
use crate::formatter::encoder::tests::non_decoded_tests;
use crate::formatter::test_utils::from_str_conv::*;
use crate::formatter::tests::instr_infos::*;
use crate::formatter::*;
use crate::iced_constants::IcedConstants;
use alloc::string::String;
use std::fmt::Write;

#[test]
fn make_sure_all_code_values_are_formatted() {
	let mut tested = [0u8; IcedConstants::CODE_ENUM_COUNT];

	#[rustfmt::skip]
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
		for &code in &crate::decoder::tests::NON_DECODED_CODE_VALUES1632 {
			tested[code as usize] = 1;
		}
		for &code in &crate::decoder::tests::NON_DECODED_CODE_VALUES {
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
	assert_eq!(format!("Fmt: {} ins {}", missing, sb), "Fmt: 0 ins ");
}

#[test]
#[allow(clippy::same_functions_in_if_condition)]
#[allow(clippy::if_same_then_else)]
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
	assert_eq!(actual, expected);
	let actual = instr.to_string();
	assert_eq!(actual, expected);
	let mut actual = String::new();
	write!(&mut actual, "{}", instr).unwrap();
	assert_eq!(actual, expected);
}
