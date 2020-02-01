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

pub(crate) mod enums;
mod instr_infos;
pub(crate) mod misc;
pub(crate) mod number;
pub(crate) mod options;
mod options_test_case_parser;
pub(crate) mod opts_info;
mod opts_infos;
pub(crate) mod registers;

use self::instr_infos::*;
#[cfg(feature = "encoder")]
use super::super::encoder::tests::non_decoded_tests;
use super::super::test_utils::create_decoder;
use super::super::test_utils::from_str_conv::to_vec_u8;
use super::super::{Code, Decoder, Instruction};
use super::Formatter;
#[cfg(not(feature = "std"))]
use alloc::boxed::Box;
#[cfg(not(feature = "std"))]
use alloc::string::String;
#[cfg(not(feature = "std"))]
use alloc::vec::Vec;

pub(crate) fn formatter_test(bitness: u32, dir: &str, filename: &str, is_misc: bool, fmt_factory: fn() -> Box<Formatter>) {
	let infos = get_infos(bitness, is_misc);
	let lines = get_formatted_lines(bitness, dir, filename);
	if infos.len() != lines.len() {
		panic!("Infos len ({}) != fmt len ({}); dir={}, filename: {}, is_misc: {}", infos.len(), lines.len(), dir, filename, is_misc);
	}
	for i in infos.iter().zip(lines.iter().map(|x| x.as_str())) {
		format_test_info(i.0, i.1, fmt_factory());
	}
}

#[cfg(feature = "encoder")]
pub(crate) fn formatter_test_nondec(bitness: u32, dir: &str, filename: &str, fmt_factory: fn() -> Box<Formatter>) {
	let instrs = non_decoded_tests::get_infos(bitness);
	let lines = get_formatted_lines(bitness, dir, filename);
	if instrs.len() != instrs.len() {
		panic!("Instrs len ({}) != fmt len ({}); dir={}, filename: {}", instrs.len(), lines.len(), dir, filename);
	}
	for i in instrs.iter().zip(lines.iter().map(|x| x.as_str())) {
		format_test_instruction(&(i.0).1, i.1, fmt_factory());
	}
}

fn format_test_info(info: &InstructionInfo, formatted_string: &str, formatter: Box<Formatter>) {
	format_test(info.bitness, &info.hex_bytes, info.code, info.options, formatted_string, formatter);
}

#[cfg(feature = "encoder")]
fn format_test_instruction(instruction: &Instruction, formatted_string: &str, formatter: Box<Formatter>) {
	format_test_instruction_core(instruction, formatted_string, formatter);
}

fn format_test(bitness: u32, hex_bytes: &str, code: Code, options: u32, formatted_string: &str, formatter: Box<Formatter>) {
	let bytes = to_vec_u8(hex_bytes).unwrap();
	let mut decoder = create_decoder(bitness, &bytes, options).0;
	let mut ip = decoder.ip();
	let instr = decoder.decode();
	assert_eq!(code, instr.code());
	assert_eq!(ip as u16, instr.ip16());
	assert_eq!(ip as u32, instr.ip32());
	assert_eq!(ip, instr.ip());
	ip += instr.len() as u64;
	assert_eq!(ip, decoder.ip());
	assert_eq!(ip as u16, instr.next_ip16());
	assert_eq!(ip as u32, instr.next_ip32());
	assert_eq!(ip, instr.next_ip());
	format_test_instruction_core(&instr, formatted_string, formatter);
}

fn format_test_instruction_core(instruction: &Instruction, formatted_string: &str, mut formatter: Box<Formatter>) {
	let mut actual_formatted_string = String::new();
	formatter.format(instruction, &mut actual_formatted_string);
	assert_eq!(formatted_string, actual_formatted_string);

	let mut mnemonic = String::new();
	formatter.format_mnemonic(instruction, &mut mnemonic);
	let op_count = formatter.operand_count(instruction);
	let mut operands: Vec<String> = Vec::with_capacity(op_count as usize);
	for i in 0..op_count {
		let mut output = String::new();
		formatter.format_operand(instruction, &mut output, i);
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
	assert_eq!(formatted_string, output);

	let mut all_operands = String::new();
	formatter.format_all_operands(instruction, &mut all_operands);
	let actual_formatted_string = if all_operands.is_empty() { mnemonic } else { format!("{} {}", mnemonic, all_operands) };
	assert_eq!(formatted_string, actual_formatted_string);
}

fn simple_format_test<F: Fn(&mut Decoder)>(
	bitness: u32, hex_bytes: &str, code: Code, decoder_options: u32, formatted_string: &str, mut formatter: Box<Formatter>, init_decoder: F,
) {
	let bytes = to_vec_u8(hex_bytes).unwrap();
	let mut decoder = create_decoder(bitness, &bytes, decoder_options).0;
	(init_decoder)(&mut decoder);
	let mut next_rip = decoder.ip();
	let instruction = decoder.decode();
	assert_eq!(code, instruction.code());
	assert_eq!(next_rip as u16, instruction.ip16());
	assert_eq!(next_rip as u32, instruction.ip32());
	assert_eq!(next_rip, instruction.ip());
	next_rip = next_rip.wrapping_add(instruction.len() as u64);
	assert_eq!(next_rip, decoder.ip());
	assert_eq!(next_rip as u16, instruction.next_ip16());
	assert_eq!(next_rip as u32, instruction.next_ip32());
	assert_eq!(next_rip, instruction.next_ip());

	let mut output = String::new();
	formatter.format(&instruction, &mut output);
	assert_eq!(formatted_string, output);
}
