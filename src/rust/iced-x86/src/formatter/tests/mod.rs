// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

pub(crate) mod enums;
mod instr_infos;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub(super) mod misc;
pub(super) mod misc2;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub(super) mod mnemonic_opts_parser;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub(super) mod number;
pub(super) mod opt_value;
pub(super) mod options;
mod options_parser;
pub(super) mod options_test_case_parser;
pub(super) mod opts_info;
mod opts_infos;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub(super) mod registers;
pub(super) mod sym_res;
pub(super) mod sym_res_test_case;
pub(super) mod sym_res_test_parser;

#[cfg(feature = "encoder")]
use crate::encoder::tests::non_decoded_tests;
use crate::formatter::tests::instr_infos::*;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
use crate::formatter::Formatter;
#[cfg(feature = "fast_fmt")]
use crate::formatter::{SpecializedFormatter, SpecializedFormatterTraitOptions};
use crate::iced_constants::IcedConstants;
use crate::test_utils::create_decoder;
use crate::test_utils::from_str_conv::to_vec_u8;
use crate::{Code, Decoder, Instruction};
use alloc::boxed::Box;
use alloc::string::String;
use alloc::vec::Vec;
use std::collections::HashSet;
use std::fs::File;
use std::io::prelude::*;
use std::io::BufReader;
use std::path::Path;

fn get_lines_ignore_comments(filename: &Path) -> Vec<String> {
	let display_filename = filename.display().to_string();
	let file = File::open(filename).unwrap_or_else(|_| panic!("Couldn't open file {}", display_filename));
	BufReader::new(file)
		.lines()
		.map(|r| r.unwrap_or_else(|e| panic!("{}", e.to_string())))
		.filter(|line| !line.is_empty() && !line.starts_with('#'))
		.collect()
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub(super) fn formatter_test(bitness: u32, dir: &str, filename: &str, is_misc: bool, fmt_factory: fn() -> Box<dyn Formatter>) {
	let (infos, ignored) = get_infos(bitness, is_misc);
	let lines = filter_removed_code_tests(get_formatted_lines(bitness, dir, filename), ignored);
	if infos.len() != lines.len() {
		panic!("Infos len ({}) != fmt len ({}); dir={}, filename: {}, is_misc: {}", infos.len(), lines.len(), dir, filename, is_misc);
	}
	for i in infos.iter().zip(lines.iter().map(String::as_str)) {
		format_test_info(i.0, i.1, fmt_factory());
	}
}

#[cfg(feature = "fast_fmt")]
pub(super) fn formatter_test_fast<TraitOptions: SpecializedFormatterTraitOptions>(
	bitness: u32, dir: &str, filename: &str, is_misc: bool, fmt_factory: fn() -> Box<SpecializedFormatter<TraitOptions>>,
) {
	let (infos, ignored) = get_infos(bitness, is_misc);
	let lines = filter_removed_code_tests(get_formatted_lines(bitness, dir, filename), ignored);
	if infos.len() != lines.len() {
		panic!("Infos len ({}) != fmt len ({}); dir={}, filename: {}, is_misc: {}", infos.len(), lines.len(), dir, filename, is_misc);
	}
	for i in infos.iter().zip(lines.iter().map(String::as_str)) {
		format_test_info_fast(i.0, i.1, fmt_factory());
	}
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
#[cfg(feature = "encoder")]
pub(super) fn formatter_test_nondec(bitness: u32, dir: &str, filename: &str, fmt_factory: fn() -> Box<dyn Formatter>) {
	let instrs = non_decoded_tests::get_infos(bitness);
	let lines = get_formatted_lines(bitness, dir, filename);
	if instrs.len() != instrs.len() {
		panic!("Instrs len ({}) != fmt len ({}); dir={}, filename: {}", instrs.len(), lines.len(), dir, filename);
	}
	for i in instrs.iter().zip(lines.iter().map(String::as_str)) {
		format_test_instruction(&(i.0).1, i.1, fmt_factory());
	}
}

#[cfg(feature = "fast_fmt")]
#[cfg(feature = "encoder")]
pub(super) fn formatter_test_nondec_fast<TraitOptions: SpecializedFormatterTraitOptions>(
	bitness: u32, dir: &str, filename: &str, fmt_factory: fn() -> Box<SpecializedFormatter<TraitOptions>>,
) {
	let instrs = non_decoded_tests::get_infos(bitness);
	let lines = get_formatted_lines(bitness, dir, filename);
	if instrs.len() != instrs.len() {
		panic!("Instrs len ({}) != fmt len ({}); dir={}, filename: {}", instrs.len(), lines.len(), dir, filename);
	}
	for i in instrs.iter().zip(lines.iter().map(String::as_str)) {
		format_test_instruction_fast(&(i.0).1, i.1, fmt_factory());
	}
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
fn format_test_info(info: &InstructionInfo, formatted_string: &str, formatter: Box<dyn Formatter>) {
	format_test(info.bitness, &info.hex_bytes, info.ip, info.code, info.options, formatted_string, formatter);
}

#[cfg(feature = "fast_fmt")]
fn format_test_info_fast<TraitOptions: SpecializedFormatterTraitOptions>(
	info: &InstructionInfo, formatted_string: &str, formatter: Box<SpecializedFormatter<TraitOptions>>,
) {
	format_test_fast(info.bitness, &info.hex_bytes, info.ip, info.code, info.options, formatted_string, formatter);
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
#[cfg(feature = "encoder")]
fn format_test_instruction(instruction: &Instruction, formatted_string: &str, formatter: Box<dyn Formatter>) {
	format_test_instruction_core(instruction, formatted_string, formatter);
}

#[cfg(feature = "fast_fmt")]
#[cfg(feature = "encoder")]
fn format_test_instruction_fast<TraitOptions: SpecializedFormatterTraitOptions>(
	instruction: &Instruction, formatted_string: &str, formatter: Box<SpecializedFormatter<TraitOptions>>,
) {
	format_test_instruction_fast_core(instruction, formatted_string, formatter);
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
fn format_test(bitness: u32, hex_bytes: &str, ip: u64, code: Code, options: u32, formatted_string: &str, formatter: Box<dyn Formatter>) {
	let bytes = to_vec_u8(hex_bytes).unwrap();
	let mut decoder = create_decoder(bitness, &bytes, ip, options).0;
	let mut ip = decoder.ip();
	let instr = decoder.decode();
	assert_eq!(instr.code(), code);
	assert_eq!(instr.ip16(), ip as u16);
	assert_eq!(instr.ip32(), ip as u32);
	assert_eq!(instr.ip(), ip);
	ip += instr.len() as u64;
	assert_eq!(decoder.ip(), ip);
	assert_eq!(instr.next_ip16(), ip as u16);
	assert_eq!(instr.next_ip32(), ip as u32);
	assert_eq!(instr.next_ip(), ip);
	format_test_instruction_core(&instr, formatted_string, formatter);
}

#[cfg(feature = "fast_fmt")]
fn format_test_fast<TraitOptions: SpecializedFormatterTraitOptions>(
	bitness: u32, hex_bytes: &str, ip: u64, code: Code, options: u32, formatted_string: &str, formatter: Box<SpecializedFormatter<TraitOptions>>,
) {
	let bytes = to_vec_u8(hex_bytes).unwrap();
	let mut decoder = create_decoder(bitness, &bytes, ip, options).0;
	let mut ip = decoder.ip();
	let instr = decoder.decode();
	assert_eq!(instr.code(), code);
	assert_eq!(instr.ip16(), ip as u16);
	assert_eq!(instr.ip32(), ip as u32);
	assert_eq!(instr.ip(), ip);
	ip += instr.len() as u64;
	assert_eq!(decoder.ip(), ip);
	assert_eq!(instr.next_ip16(), ip as u16);
	assert_eq!(instr.next_ip32(), ip as u32);
	assert_eq!(instr.next_ip(), ip);
	format_test_instruction_fast_core(&instr, formatted_string, formatter);
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
fn format_test_instruction_core(instruction: &Instruction, formatted_string: &str, mut formatter: Box<dyn Formatter>) {
	let mut actual_formatted_string = String::new();
	formatter.format(instruction, &mut actual_formatted_string);
	assert_eq!(actual_formatted_string, formatted_string);

	let mut mnemonic = String::new();
	formatter.format_mnemonic(instruction, &mut mnemonic);
	let op_count = formatter.operand_count(instruction);
	let mut operands: Vec<String> = Vec::with_capacity(op_count as usize);
	for i in 0..op_count {
		let mut output = String::new();
		assert!(formatter.format_operand(instruction, &mut output, i).is_ok());
		operands.push(output);
	}
	let mut output = String::new();
	output.push_str(&mnemonic);
	if !operands.is_empty() {
		output.push(' ');
		for (i, operand) in operands.iter().enumerate() {
			if i > 0 {
				formatter.format_operand_separator(instruction, &mut output);
			}
			output.push_str(operand);
		}
	}
	assert_eq!(output, formatted_string);

	let mut all_operands = String::new();
	formatter.format_all_operands(instruction, &mut all_operands);
	let actual_formatted_string = if all_operands.is_empty() { mnemonic } else { format!("{} {}", mnemonic, all_operands) };
	assert_eq!(actual_formatted_string, formatted_string);
}

#[cfg(feature = "fast_fmt")]
fn format_test_instruction_fast_core<TraitOptions: SpecializedFormatterTraitOptions>(
	instruction: &Instruction, formatted_string: &str, mut formatter: Box<SpecializedFormatter<TraitOptions>>,
) {
	let mut actual_formatted_string = String::new();
	formatter.as_mut().format(instruction, &mut actual_formatted_string);
	assert_eq!(actual_formatted_string, formatted_string);
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
fn simple_format_test<F: Fn(&mut Decoder<'_>)>(
	bitness: u32, hex_bytes: &str, ip: u64, code: Code, decoder_options: u32, line_number: u32, formatted_string: &str,
	formatter: &mut dyn Formatter, init_decoder: F,
) {
	let bytes = to_vec_u8(hex_bytes).unwrap();
	let mut decoder = create_decoder(bitness, &bytes, ip, decoder_options).0;
	(init_decoder)(&mut decoder);
	let mut next_rip = decoder.ip();
	let instruction = decoder.decode();
	assert_eq!(instruction.code(), code);
	assert_eq!(instruction.ip16(), next_rip as u16);
	assert_eq!(instruction.ip32(), next_rip as u32);
	assert_eq!(instruction.ip(), next_rip);
	next_rip = next_rip.wrapping_add(instruction.len() as u64);
	assert_eq!(decoder.ip(), next_rip);
	assert_eq!(instruction.next_ip16(), next_rip as u16);
	assert_eq!(instruction.next_ip32(), next_rip as u32);
	assert_eq!(instruction.next_ip(), next_rip);

	let mut output = String::new();
	formatter.format(&instruction, &mut output);
	assert_eq!(output, formatted_string, "line {}", line_number);
}

#[cfg(feature = "fast_fmt")]
fn simple_format_test_fast<TraitOptions: SpecializedFormatterTraitOptions, F: Fn(&mut Decoder<'_>)>(
	bitness: u32, hex_bytes: &str, ip: u64, code: Code, decoder_options: u32, line_number: u32, formatted_string: &str,
	formatter: &mut SpecializedFormatter<TraitOptions>, init_decoder: F,
) {
	let bytes = to_vec_u8(hex_bytes).unwrap();
	let mut decoder = create_decoder(bitness, &bytes, ip, decoder_options).0;
	(init_decoder)(&mut decoder);
	let mut next_rip = decoder.ip();
	let instruction = decoder.decode();
	assert_eq!(instruction.code(), code);
	assert_eq!(instruction.ip16(), next_rip as u16);
	assert_eq!(instruction.ip32(), next_rip as u32);
	assert_eq!(instruction.ip(), next_rip);
	next_rip = next_rip.wrapping_add(instruction.len() as u64);
	assert_eq!(decoder.ip(), next_rip);
	assert_eq!(instruction.next_ip16(), next_rip as u16);
	assert_eq!(instruction.next_ip32(), next_rip as u32);
	assert_eq!(instruction.next_ip(), next_rip);

	let mut output = String::new();
	formatter.format(&instruction, &mut output);
	assert_eq!(output, formatted_string, "line {}", line_number);
}

fn filter_removed_code_tests(strings: Vec<String>, ignored: &HashSet<u32>) -> Vec<String> {
	if ignored.is_empty() {
		strings
	} else {
		strings.into_iter().enumerate().filter(|a| !ignored.contains(&(a.0 as u32))).map(|a| a.1).collect()
	}
}

#[cfg(any(feature = "fast_fmt", feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
#[test]
fn verify_sae_er() {
	use crate::RoundingControl;

	#[cfg(feature = "fast_fmt")]
	let (mut fast, mut fast_output) = {
		let fast = crate::FastFormatter::new();
		let fast_output = String::new();
		(fast, fast_output)
	};

	#[cfg(feature = "gas")]
	let (mut gas, mut gas_output) = {
		let mut gas = crate::GasFormatter::new();
		let gas_output = String::new();
		gas.options_mut().set_show_useless_prefixes(true);
		(gas, gas_output)
	};

	#[cfg(feature = "intel")]
	let (mut intel, mut intel_output) = {
		let mut intel = crate::IntelFormatter::new();
		let intel_output = String::new();
		intel.options_mut().set_show_useless_prefixes(true);
		(intel, intel_output)
	};

	#[cfg(feature = "masm")]
	let (mut masm, mut masm_output) = {
		let mut masm = crate::MasmFormatter::new();
		let masm_output = String::new();
		masm.options_mut().set_show_useless_prefixes(true);
		(masm, masm_output)
	};

	#[cfg(feature = "nasm")]
	let (mut nasm, mut nasm_output) = {
		let mut nasm = crate::NasmFormatter::new();
		let nasm_output = String::new();
		nasm.options_mut().set_show_useless_prefixes(true);
		(nasm, nasm_output)
	};

	const FL_SAE: u16 = 0x01;
	const FL_RN: u16 = 0x02;
	const FL_RD: u16 = 0x04;
	const FL_RU: u16 = 0x08;
	const FL_RZ: u16 = 0x10;
	const FL_RN_SAE: u16 = 0x20;
	const FL_RD_SAE: u16 = 0x40;
	const FL_RU_SAE: u16 = 0x80;
	const FL_RZ_SAE: u16 = 0x100;

	let mut instr = Instruction::default();
	let mut flags_result = Vec::with_capacity(5);
	let mut all_output = Vec::new();
	let test_cases = crate::decoder::tests::test_utils::decoder_tests(true, false);
	for tc in test_cases {
		all_output.clear();
		flags_result.clear();
		let bytes = to_vec_u8(tc.hex_bytes()).unwrap();
		let mut decoder = create_decoder(tc.bitness(), &bytes, tc.ip(), tc.decoder_options()).0;
		decoder.decode_out(&mut instr);

		let expected_flags = if IcedConstants::is_mvex(tc.code()) && !instr.suppress_all_exceptions() {
			match instr.rounding_control() {
				RoundingControl::None => 0,
				RoundingControl::RoundToNearest => FL_RN,
				RoundingControl::RoundDown => FL_RD,
				RoundingControl::RoundUp => FL_RU,
				RoundingControl::RoundTowardZero => FL_RZ,
			}
		} else {
			match instr.rounding_control() {
				RoundingControl::None => {
					if instr.suppress_all_exceptions() {
						FL_SAE
					} else {
						0
					}
				}
				RoundingControl::RoundToNearest => FL_RN_SAE,
				RoundingControl::RoundDown => FL_RD_SAE,
				RoundingControl::RoundUp => FL_RU_SAE,
				RoundingControl::RoundTowardZero => FL_RZ_SAE,
			}
		};

		fn get_flags(disasm: &str, values: &[(&str, u16)]) -> u16 {
			let mut result = 0;
			for &(s, f) in values {
				if disasm.contains(s) {
					result |= f;
				}
			}
			result
		}

		#[cfg(feature = "fast_fmt")]
		{
			fast_output.clear();
			fast.format(&instr, &mut fast_output);
			let flags = get_flags(
				&fast_output,
				&[
					("{sae}", FL_SAE),
					("{rn}", FL_RN),
					("{rd}", FL_RD),
					("{ru}", FL_RU),
					("{rz}", FL_RZ),
					("{rn-sae}", FL_RN_SAE),
					("{rd-sae}", FL_RD_SAE),
					("{ru-sae}", FL_RU_SAE),
					("{rz-sae}", FL_RZ_SAE),
				],
			);
			flags_result.push(flags);
			all_output.push(format!(" fast: 0x{:X} {}", flags, fast_output));
		}

		#[cfg(feature = "gas")]
		{
			gas_output.clear();
			gas.format(&instr, &mut gas_output);
			let flags = get_flags(
				&gas_output,
				&[
					("{sae}", FL_SAE),
					("{rn}", FL_RN),
					("{rd}", FL_RD),
					("{ru}", FL_RU),
					("{rz}", FL_RZ),
					("{rn-sae}", FL_RN_SAE),
					("{rd-sae}", FL_RD_SAE),
					("{ru-sae}", FL_RU_SAE),
					("{rz-sae}", FL_RZ_SAE),
				],
			);
			flags_result.push(flags);
			all_output.push(format!("  gas: 0x{:X} {}", flags, gas_output));
		}

		#[cfg(feature = "intel")]
		{
			intel_output.clear();
			intel.format(&instr, &mut intel_output);
			let flags = get_flags(
				&intel_output,
				&[
					("{sae}", FL_SAE),
					("{rne}", FL_RN),
					("{rd}", FL_RD),
					("{ru}", FL_RU),
					("{rz}", FL_RZ),
					("{rne-sae}", FL_RN_SAE),
					("{rd-sae}", FL_RD_SAE),
					("{ru-sae}", FL_RU_SAE),
					("{rz-sae}", FL_RZ_SAE),
				],
			);
			flags_result.push(flags);
			all_output.push(format!("intel: 0x{:X} {}", flags, intel_output));
		}

		#[cfg(feature = "masm")]
		{
			masm_output.clear();
			masm.format(&instr, &mut masm_output);
			let flags = get_flags(
				&masm_output,
				&[
					("{sae}", FL_SAE),
					("{rn}", FL_RN),
					("{rd}", FL_RD),
					("{ru}", FL_RU),
					("{rz}", FL_RZ),
					("{rn-sae}", FL_RN_SAE),
					("{rd-sae}", FL_RD_SAE),
					("{ru-sae}", FL_RU_SAE),
					("{rz-sae}", FL_RZ_SAE),
				],
			);
			flags_result.push(flags);
			all_output.push(format!(" masm: 0x{:X} {}", flags, masm_output));
		}

		#[cfg(feature = "nasm")]
		{
			nasm_output.clear();
			nasm.format(&instr, &mut nasm_output);
			let flags = get_flags(
				&nasm_output,
				&[
					("{sae}", FL_SAE),
					("{rn}", FL_RN),
					("{rd}", FL_RD),
					("{ru}", FL_RU),
					("{rz}", FL_RZ),
					("{rn-sae}", FL_RN_SAE),
					("{rd-sae}", FL_RD_SAE),
					("{ru-sae}", FL_RU_SAE),
					("{rz-sae}", FL_RZ_SAE),
				],
			);
			flags_result.push(flags);
			all_output.push(format!(" nasm: 0x{:X} {}", flags, nasm_output));
		}

		if !flags_result.iter().all(|&f| f == expected_flags) {
			panic!(
				"\nMissing/extra {{sae}} and/or {{er}}\nexpected: 0x{:X}\n{}\n{}-bit, hex {} Code = {:?}\n",
				expected_flags,
				all_output.join("\n"),
				tc.bitness(),
				tc.hex_bytes(),
				tc.code(),
			);
		}
	}
}
