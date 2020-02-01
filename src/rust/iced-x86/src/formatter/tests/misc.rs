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
use super::super::decoder::tests::test_utils;
#[cfg(feature = "encoder")]
use super::super::encoder::tests::non_decoded_tests;
use super::super::test_utils::create_decoder;
use super::super::test_utils::from_str_conv::*;
use super::super::*;
use super::instr_infos::*;
#[cfg(not(feature = "std"))]
use alloc::boxed::Box;
#[cfg(not(feature = "std"))]
use alloc::string::String;
use std::panic;

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
	for &(bitness, is_misc) in all_args.iter() {
		for info in get_infos(bitness, is_misc).iter() {
			tested[info.code as usize] = 1;
		}
	}
	if cfg!(feature = "encoder") {
		#[cfg(feature = "encoder")] // needed...
		for info in non_decoded_tests::get_tests() {
			tested[info.2.code() as usize] = 1;
		}
	} else {
		for &code in super::super::super::decoder::tests::NON_DECODED_CODE_VALUES.iter() {
			tested[code as usize] = 1;
		}
	}

	let mut sb = String::new();
	let mut missing = 0;
	let code_names = code_names();
	for (i, &t) in tested.iter().enumerate() {
		if t != 1 {
			sb.push_str(code_names[i]);
			sb.push(' ');
			missing += 1;
		}
	}
	assert_eq!("Fmt: 0 ins ", format!("Fmt: {} ins {}", missing, sb));
}

#[test]
fn test_formatter_operand_options_methods() {
	let mut options = FormatterOperandOptions::default();

	options.set_memory_size_options(MemorySizeOptions::Always);
	assert_eq!(MemorySizeOptions::Always, options.memory_size_options());

	options.set_memory_size_options(MemorySizeOptions::Minimum);
	assert_eq!(MemorySizeOptions::Minimum, options.memory_size_options());

	options.set_memory_size_options(MemorySizeOptions::Never);
	assert_eq!(MemorySizeOptions::Never, options.memory_size_options());

	options.set_memory_size_options(MemorySizeOptions::Default);
	assert_eq!(MemorySizeOptions::Default, options.memory_size_options());

	options.set_branch_size(true);
	assert!(options.branch_size());
	options.set_branch_size(false);
	assert!(!options.branch_size());

	options.set_rip_relative_addresses(true);
	assert!(options.rip_relative_addresses());
	options.set_rip_relative_addresses(false);
	assert!(!options.rip_relative_addresses());
}

pub(crate) fn methods_panic_if_invalid_operand_or_instruction_operand(fmt_factory: fn() -> Box<Formatter>) {
	{
		let mut instruction = Instruction::default();
		instruction.set_code(Code::Mov_rm64_r64);
		instruction.set_op0_register(Register::RAX);
		instruction.set_op1_register(Register::RCX);
		let instruction = instruction;
		let num_ops = 2;
		let num_instr_ops = 2;
		assert_eq!(num_ops, fmt_factory().operand_count(&instruction));
		assert_eq!(num_instr_ops, instruction.op_count());
		#[cfg(feature = "instr_info")]
		{
			let instruction = instruction;
			assert!(panic::catch_unwind(move || fmt_factory().op_access(&instruction, num_ops)).is_err());
		}
		{
			let instruction = instruction;
			assert!(panic::catch_unwind(move || fmt_factory().get_instruction_operand(&instruction, num_ops)).is_err());
		}
		{
			let instruction = instruction;
			assert!(panic::catch_unwind(move || fmt_factory().get_formatter_operand(&instruction, num_instr_ops)).is_err());
		}
		{
			let instruction = instruction;
			assert!(panic::catch_unwind(move || fmt_factory().format_operand(&instruction, &mut String::new(), num_ops)).is_err());
		}
	}

	{
		let invalid = Instruction::default();
		#[cfg(feature = "instr_info")]
		{
			let invalid = invalid;
			assert!(panic::catch_unwind(move || fmt_factory().op_access(&invalid, 0)).is_err());
		}
		{
			let invalid = invalid;
			assert!(panic::catch_unwind(move || fmt_factory().get_instruction_operand(&invalid, 0)).is_err());
		}
		{
			let invalid = invalid;
			assert!(panic::catch_unwind(move || fmt_factory().get_formatter_operand(&invalid, 0)).is_err());
		}
		{
			let invalid = invalid;
			assert!(panic::catch_unwind(move || fmt_factory().format_operand(&invalid, &mut String::new(), 0)).is_err());
		}
	}

	#[cfg(feature = "encoder")]
	{
		let db = Instruction::with_declare_byte(&[0; 8]);
		assert_eq!(8, db.declare_data_len());
		for i in 0..db.declare_data_len() as u32 {
			#[cfg(feature = "instr_info")]
			let _ = fmt_factory().op_access(&db, i);
			let _ = fmt_factory().get_instruction_operand(&db, i);
			fmt_factory().format_operand(&db, &mut String::new(), i);
		}
		for i in db.declare_data_len() as u32..17 {
			#[cfg(feature = "instr_info")]
			{
				let db = db;
				assert!(panic::catch_unwind(move || fmt_factory().op_access(&db, i)).is_err());
			}
			{
				let db = db;
				assert!(panic::catch_unwind(move || fmt_factory().get_instruction_operand(&db, i)).is_err());
			}
			{
				let db = db;
				assert!(panic::catch_unwind(move || fmt_factory().format_operand(&db, &mut String::new(), i)).is_err());
			}
		}
		{
			let db = db;
			assert!(panic::catch_unwind(move || fmt_factory().get_formatter_operand(&db, 0)).is_err());
		}
	}

	#[cfg(feature = "encoder")]
	{
		let dw = Instruction::with_declare_word(&[0; 4]);
		assert_eq!(4, dw.declare_data_len());
		for i in 0..dw.declare_data_len() as u32 {
			#[cfg(feature = "instr_info")]
			let _ = fmt_factory().op_access(&dw, i);
			let _ = fmt_factory().get_instruction_operand(&dw, i);
			fmt_factory().format_operand(&dw, &mut String::new(), i);
		}
		for i in dw.declare_data_len() as u32..17 {
			#[cfg(feature = "instr_info")]
			{
				let dw = dw;
				assert!(panic::catch_unwind(move || fmt_factory().op_access(&dw, i)).is_err());
			}
			{
				let dw = dw;
				assert!(panic::catch_unwind(move || fmt_factory().get_instruction_operand(&dw, i)).is_err());
			}
			{
				let dw = dw;
				assert!(panic::catch_unwind(move || fmt_factory().format_operand(&dw, &mut String::new(), i)).is_err());
			}
		}
		{
			let dw = dw;
			assert!(panic::catch_unwind(move || fmt_factory().get_formatter_operand(&dw, 0)).is_err());
		}
	}

	#[cfg(feature = "encoder")]
	{
		let dd = Instruction::with_declare_dword(&[8; 2]);
		assert_eq!(2, dd.declare_data_len());
		for i in 0..dd.declare_data_len() as u32 {
			#[cfg(feature = "instr_info")]
			let _ = fmt_factory().op_access(&dd, i);
			let _ = fmt_factory().get_instruction_operand(&dd, i);
			fmt_factory().format_operand(&dd, &mut String::new(), i);
		}
		for i in dd.declare_data_len() as u32..17 {
			#[cfg(feature = "instr_info")]
			{
				let dd = dd;
				assert!(panic::catch_unwind(move || fmt_factory().op_access(&dd, i)).is_err());
			}
			{
				let dd = dd;
				assert!(panic::catch_unwind(move || fmt_factory().get_instruction_operand(&dd, i)).is_err());
			}
			{
				let dd = dd;
				assert!(panic::catch_unwind(move || fmt_factory().format_operand(&dd, &mut String::new(), i)).is_err());
			}
		}
		{
			let dd = dd;
			assert!(panic::catch_unwind(move || fmt_factory().get_formatter_operand(&dd, 0)).is_err());
		}
	}

	#[cfg(feature = "encoder")]
	{
		let dq = Instruction::with_declare_qword(&[0; 1]);
		assert_eq!(1, dq.declare_data_len());
		for i in 0..dq.declare_data_len() as u32 {
			#[cfg(feature = "instr_info")]
			let _ = fmt_factory().op_access(&dq, i);
			let _ = fmt_factory().get_instruction_operand(&dq, i);
			fmt_factory().format_operand(&dq, &mut String::new(), i);
		}
		for i in dq.declare_data_len() as u32..17 {
			#[cfg(feature = "instr_info")]
			{
				let dq = dq;
				assert!(panic::catch_unwind(move || fmt_factory().op_access(&dq, i)).is_err());
			}
			{
				let dq = dq;
				assert!(panic::catch_unwind(move || fmt_factory().get_instruction_operand(&dq, i)).is_err());
			}
			{
				let dq = dq;
				assert!(panic::catch_unwind(move || fmt_factory().format_operand(&dq, &mut String::new(), i)).is_err());
			}
		}
		{
			let dq = dq;
			assert!(panic::catch_unwind(move || fmt_factory().get_formatter_operand(&dq, 0)).is_err());
		}
	}
}

#[test]
fn verify_default_formatter_options() {
	let options = FormatterOptions::new();
	assert!(!options.uppercase_prefixes());
	assert!(!options.uppercase_mnemonics());
	assert!(!options.uppercase_registers());
	assert!(!options.uppercase_keywords());
	assert!(!options.uppercase_decorators());
	assert!(!options.uppercase_all());
	assert_eq!(0, options.first_operand_char_index());
	assert_eq!(0, options.tab_size());
	assert!(!options.space_after_operand_separator());
	assert!(!options.space_after_memory_bracket());
	assert!(!options.space_between_memory_add_operators());
	assert!(!options.space_between_memory_mul_operators());
	assert!(!options.scale_before_index());
	assert!(!options.always_show_scale());
	assert!(!options.always_show_segment_register());
	assert!(!options.show_zero_displacements());
	assert_eq!("", options.hex_prefix());
	assert_eq!("", options.hex_suffix());
	assert_eq!(4, options.hex_digit_group_size());
	assert_eq!("", options.decimal_prefix());
	assert_eq!("", options.decimal_suffix());
	assert_eq!(3, options.decimal_digit_group_size());
	assert_eq!("", options.octal_prefix());
	assert_eq!("", options.octal_suffix());
	assert_eq!(4, options.octal_digit_group_size());
	assert_eq!("", options.binary_prefix());
	assert_eq!("", options.binary_suffix());
	assert_eq!(4, options.binary_digit_group_size());
	assert_eq!("", options.digit_separator());
	assert!(!options.leading_zeroes());
	assert!(options.uppercase_hex());
	assert!(options.small_hex_numbers_in_decimal());
	assert!(options.add_leading_zero_to_hex_numbers());
	assert_eq!(NumberBase::Hexadecimal, options.number_base());
	assert!(options.branch_leading_zeroes());
	assert!(!options.signed_immediate_operands());
	assert!(options.signed_memory_displacements());
	assert!(!options.displacement_leading_zeroes());
	assert_eq!(MemorySizeOptions::Default, options.memory_size_options());
	assert!(!options.rip_relative_addresses());
	assert!(options.show_branch_size());
	assert!(options.use_pseudo_ops());
	assert!(!options.show_symbol_address());
	assert!(!options.prefer_st0());
	assert!(!options.gas_naked_registers());
	assert!(!options.gas_show_mnemonic_size_suffix());
	assert!(!options.gas_space_after_memory_operand_comma());
	assert!(options.masm_add_ds_prefix32());
	assert!(options.masm_symbol_displ_in_brackets());
	assert!(options.masm_displ_in_brackets());
	assert!(!options.nasm_show_sign_extended_immediate_size());
}

#[test]
fn verify_formatter_options_new_is_same_as_default() {
	assert_eq!(FormatterOptions::new(), FormatterOptions::default());
}

pub(crate) fn test_op_index(fmt_factory: fn() -> Box<Formatter>) {
	let mut formatter = fmt_factory();
	let mut instr_to_formatter: [Option<u32>; IcedConstants::MAX_OP_COUNT] = Default::default();
	for info in test_utils::decoder_tests(true, false) {
		let bytes = to_vec_u8(info.hex_bytes()).unwrap();
		let mut decoder = create_decoder(info.bitness(), &bytes, info.decoder_options()).0;
		let instruction = decoder.decode();
		assert_eq!(info.code(), instruction.code());

		for i in instr_to_formatter.iter_mut() {
			*i = None;
		}

		let formatter_op_count = formatter.operand_count(&instruction);
		let instruction_op_count = instruction.op_count();

		let mut instr_op_used: u32 = 0;
		assert!(instruction_op_count <= 32); // uint is 32 bits
		for formatter_op_index in 0..formatter_op_count {
			if let Some(instr_op_index) = formatter.get_instruction_operand(&instruction, formatter_op_index) {
				assert!(instr_op_index < instruction_op_count);
				instr_to_formatter[instr_op_index as usize] = Some(formatter_op_index);

				#[cfg(feature = "instr_info")]
				assert!(formatter.op_access(&instruction, formatter_op_index).is_none());

				let instr_op_bit: u32 = 1 << instr_op_index;
				assert!(0 == (instr_op_used & instr_op_bit), "More than one formatter operand index maps to the same instruction op index");
				instr_op_used |= instr_op_bit;

				assert_eq!(Some(formatter_op_index), formatter.get_formatter_operand(&instruction, instr_op_index));
			} else {
				#[cfg(feature = "instr_info")]
				assert!(formatter.op_access(&instruction, formatter_op_index).is_some());
			}
		}

		for instr_op_index in 0..instruction_op_count {
			let formatter_op_index = formatter.get_formatter_operand(&instruction, instr_op_index);
			assert_eq!(instr_to_formatter[instr_op_index as usize], formatter_op_index);
		}

		for instr_op_index in instruction_op_count..IcedConstants::MAX_OP_COUNT as u32 {
			let instruction = instruction;
			assert!(panic::catch_unwind(move || fmt_factory().get_formatter_operand(&instruction, instr_op_index)).is_err());
		}
	}
}
