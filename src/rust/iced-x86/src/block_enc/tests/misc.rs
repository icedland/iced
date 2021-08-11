// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::block_enc::tests::*;

#[test]
fn encode_zero_blocks() {
	let mut result;

	result = BlockEncoder::encode_slice(16, &[], BlockEncoderOptions::NONE).unwrap();
	assert!(result.is_empty());

	result = BlockEncoder::encode_slice(32, &[], BlockEncoderOptions::NONE).unwrap();
	assert!(result.is_empty());

	result = BlockEncoder::encode_slice(64, &[], BlockEncoderOptions::NONE).unwrap();
	assert!(result.is_empty());
}

#[test]
fn encode_zero_instructions() {
	let mut result;

	result = BlockEncoder::encode(16, InstructionBlock::new(&[], 0), BlockEncoderOptions::NONE).unwrap();
	assert_eq!(result.rip, 0);
	assert!(result.code_buffer.is_empty());
	assert!(result.reloc_infos.is_empty());
	assert!(result.new_instruction_offsets.is_empty());
	assert!(result.constant_offsets.is_empty());

	result = BlockEncoder::encode(32, InstructionBlock::new(&[], 0), BlockEncoderOptions::NONE).unwrap();
	assert_eq!(result.rip, 0);
	assert!(result.code_buffer.is_empty());
	assert!(result.reloc_infos.is_empty());
	assert!(result.new_instruction_offsets.is_empty());
	assert!(result.constant_offsets.is_empty());

	result = BlockEncoder::encode(64, InstructionBlock::new(&[], 0), BlockEncoderOptions::NONE).unwrap();
	assert_eq!(result.rip, 0);
	assert!(result.code_buffer.is_empty());
	assert!(result.reloc_infos.is_empty());
	assert!(result.new_instruction_offsets.is_empty());
	assert!(result.constant_offsets.is_empty());
}

#[test]
fn default_args() {
	const BITNESS: u32 = 64;
	const ORIG_RIP: u64 = 0x1234_5678_9ABC_DE00;
	const NEW_RIP: u64 = 0x8000_0000_0000_0000;

	let original_data = vec![
		/*0000*/ 0xB0, 0x00, // mov al,0
		/*0002*/ 0xEB, 0x09, // jmp short 123456789ABCDE0Dh
		/*0004*/ 0xB0, 0x01, // mov al,1
		/*0006*/ 0xE9, 0x03, 0x00, 0x00, 0x00, // jmp near ptr 123456789ABCDE0Eh
		/*000B*/ 0xB0, 0x02, // mov al,2
	];
	let instructions = decode(BITNESS, ORIG_RIP, &original_data, DecoderOptions::NONE);
	let result = BlockEncoder::encode(BITNESS, InstructionBlock::new(&instructions, NEW_RIP), BlockEncoderOptions::NONE).unwrap();
	assert_eq!(result.rip, NEW_RIP);
	assert_eq!(result.code_buffer.len(), 0x28);
	assert!(result.reloc_infos.is_empty());
	assert!(result.new_instruction_offsets.is_empty());
	assert!(result.constant_offsets.is_empty());
}

#[test]
fn verify_result_vectors() {
	const BITNESS: u32 = 64;
	const ORIG_RIP1: u64 = 0x1234_5678_9ABC_DE00;
	const ORIG_RIP2: u64 = 0x2234_5678_9ABC_DE00;
	const NEW_RIP1: u64 = 0x8000_0000_0000_0000;
	const NEW_RIP2: u64 = 0x9000_0000_0000_0000;

	#[rustfmt::skip]
	let tests = [
		BlockEncoderOptions::RETURN_RELOC_INFOS,
		BlockEncoderOptions::RETURN_NEW_INSTRUCTION_OFFSETS,
		BlockEncoderOptions::RETURN_CONSTANT_OFFSETS,
	];
	for &options in &tests {
		{
			let instructions1 = decode(BITNESS, ORIG_RIP1, &[0xE9, 0x56, 0x78, 0xA5, 0x5A], DecoderOptions::NONE);
			let result = BlockEncoder::encode(BITNESS, InstructionBlock::new(&instructions1, NEW_RIP1), options).unwrap();
			assert_eq!(result.rip, NEW_RIP1);
			if (options & BlockEncoderOptions::RETURN_RELOC_INFOS) != 0 {
				assert_eq!(result.reloc_infos.len(), 1);
			} else {
				assert!(result.reloc_infos.is_empty());
			}
			if (options & BlockEncoderOptions::RETURN_NEW_INSTRUCTION_OFFSETS) != 0 {
				assert_eq!(result.new_instruction_offsets.len(), 1);
			} else {
				assert!(result.new_instruction_offsets.is_empty());
			}
			if (options & BlockEncoderOptions::RETURN_CONSTANT_OFFSETS) != 0 {
				assert_eq!(result.constant_offsets.len(), 1);
			} else {
				assert!(result.constant_offsets.is_empty());
			}
		}
		{
			let instructions1 = decode(BITNESS, ORIG_RIP1, &[0xE9, 0x56, 0x78, 0xA5, 0x5A], DecoderOptions::NONE);
			let instructions2 = decode(BITNESS, ORIG_RIP2, &[0x90, 0xE9, 0x56, 0x78, 0xA5, 0x5A], DecoderOptions::NONE);
			let block1 = InstructionBlock::new(&instructions1, NEW_RIP1);
			let block2 = InstructionBlock::new(&instructions2, NEW_RIP2);
			let result = BlockEncoder::encode_slice(BITNESS, &[block1, block2], options).unwrap();
			assert_eq!(result.len(), 2);
			assert_eq!(result[0].rip, NEW_RIP1);
			assert_eq!(result[1].rip, NEW_RIP2);
			if (options & BlockEncoderOptions::RETURN_RELOC_INFOS) != 0 {
				assert_eq!(result[0].reloc_infos.len(), 1);
				assert_eq!(result[1].reloc_infos.len(), 1);
			} else {
				assert!(result[0].reloc_infos.is_empty());
				assert!(result[1].reloc_infos.is_empty());
			}
			if (options & BlockEncoderOptions::RETURN_NEW_INSTRUCTION_OFFSETS) != 0 {
				assert_eq!(result[0].new_instruction_offsets.len(), 1);
				assert_eq!(result[1].new_instruction_offsets.len(), 2);
			} else {
				assert!(result[0].new_instruction_offsets.is_empty());
				assert!(result[1].new_instruction_offsets.is_empty());
			}
			if (options & BlockEncoderOptions::RETURN_CONSTANT_OFFSETS) != 0 {
				assert_eq!(result[0].constant_offsets.len(), 1);
				assert_eq!(result[1].constant_offsets.len(), 2);
			} else {
				assert!(result[0].constant_offsets.is_empty());
				assert!(result[1].constant_offsets.is_empty());
			}
		}
	}
}

#[test]
fn encode_declare_byte() {
	const BITNESS: u32 = 64;
	const NEW_RIP: u64 = 0x8000_0000_0000_0000;

	#[rustfmt::skip]
	let test_data = [
		(vec![0x5A], vec![0x90, 0x5A, 0x90]),
		(vec![0xF0, 0xD2, 0x7A, 0x18, 0xA0], vec![0x90, 0xF0, 0xD2, 0x7A, 0x18, 0xA0, 0x90]),
		(vec![0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08], vec![0x90, 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08, 0x90]),
	];

	for info in &test_data {
		#[rustfmt::skip]
		let instructions = [
			Instruction::with(Code::Nopd),
			Instruction::with_declare_byte(&info.0).unwrap(),
			Instruction::with(Code::Nopd),
		];

		let result = BlockEncoder::encode(BITNESS, InstructionBlock::new(&instructions, NEW_RIP), BlockEncoderOptions::NONE).unwrap();
		assert_eq!(result.code_buffer, info.1);
		assert_eq!(result.rip, NEW_RIP);
		assert!(result.reloc_infos.is_empty());
		assert!(result.new_instruction_offsets.is_empty());
		assert!(result.constant_offsets.is_empty());
	}
}

#[test]
fn encode_with_invalid_bitness_fails_0() {
	assert!(BlockEncoder::encode(0, InstructionBlock::new(&[Instruction::default()], 0), BlockEncoderOptions::NONE).is_err());
}

#[test]
fn encode_with_invalid_bitness_fails_128() {
	assert!(BlockEncoder::encode(128, InstructionBlock::new(&[Instruction::default()], 0), BlockEncoderOptions::NONE).is_err());
}

#[test]
fn encode_slice_with_invalid_bitness_fails_0() {
	assert!(BlockEncoder::encode_slice(0, &[InstructionBlock::new(&[Instruction::default()], 0)], BlockEncoderOptions::NONE).is_err());
}

#[test]
fn encode_slice_with_invalid_bitness_fails_128() {
	assert!(BlockEncoder::encode_slice(128, &[InstructionBlock::new(&[Instruction::default()], 0)], BlockEncoderOptions::NONE).is_err());
}

#[test]
fn encode_rip_rel_mem_op() {
	let instr = Instruction::with2(
		Code::Add_r32_rm32,
		Register::ECX,
		MemoryOperand::new(Register::RIP, Register::None, 1, 0x1234_5678_9ABC_DEF1, 8, false, Register::None),
	)
	.unwrap();
	let vec_result = BlockEncoder::encode_slice(64, &[InstructionBlock::new(&[instr], 0x1234_5678_ABCD_EF02)], BlockEncoderOptions::NONE).unwrap();
	assert_eq!(vec_result.len(), 1);
	let result = &vec_result[0];
	assert_eq!(result.code_buffer, vec![0x03, 0x0D, 0xE9, 0xEF, 0xEE, 0xEE]);
}
