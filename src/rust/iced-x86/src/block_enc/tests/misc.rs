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

use super::*;

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
	assert_eq!(0, result.rip);
	assert!(result.code_buffer.is_empty());
	assert!(result.reloc_infos.is_empty());
	assert!(result.new_instruction_offsets.is_empty());
	assert!(result.constant_offsets.is_empty());

	result = BlockEncoder::encode(32, InstructionBlock::new(&[], 0), BlockEncoderOptions::NONE).unwrap();
	assert_eq!(0, result.rip);
	assert!(result.code_buffer.is_empty());
	assert!(result.reloc_infos.is_empty());
	assert!(result.new_instruction_offsets.is_empty());
	assert!(result.constant_offsets.is_empty());

	result = BlockEncoder::encode(64, InstructionBlock::new(&[], 0), BlockEncoderOptions::NONE).unwrap();
	assert_eq!(0, result.rip);
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
	assert_eq!(NEW_RIP, result.rip);
	assert_eq!(0x28, result.code_buffer.len());
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

	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
	let tests = [
		BlockEncoderOptions::RETURN_RELOC_INFOS,
		BlockEncoderOptions::RETURN_NEW_INSTRUCTION_OFFSETS,
		BlockEncoderOptions::RETURN_CONSTANT_OFFSETS,
	];
	for &options in &tests {
		{
			let instructions1 = decode(BITNESS, ORIG_RIP1, &[0xE9, 0x56, 0x78, 0xA5, 0x5A], DecoderOptions::NONE);
			let result = BlockEncoder::encode(BITNESS, InstructionBlock::new(&instructions1, NEW_RIP1), options).unwrap();
			assert_eq!(NEW_RIP1, result.rip);
			if (options & BlockEncoderOptions::RETURN_RELOC_INFOS) != 0 {
				assert_eq!(1, result.reloc_infos.len());
			} else {
				assert!(result.reloc_infos.is_empty());
			}
			if (options & BlockEncoderOptions::RETURN_NEW_INSTRUCTION_OFFSETS) != 0 {
				assert_eq!(1, result.new_instruction_offsets.len());
			} else {
				assert!(result.new_instruction_offsets.is_empty());
			}
			if (options & BlockEncoderOptions::RETURN_CONSTANT_OFFSETS) != 0 {
				assert_eq!(1, result.constant_offsets.len());
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
			assert_eq!(2, result.len());
			assert_eq!(NEW_RIP1, result[0].rip);
			assert_eq!(NEW_RIP2, result[1].rip);
			if (options & BlockEncoderOptions::RETURN_RELOC_INFOS) != 0 {
				assert_eq!(1, result[0].reloc_infos.len());
				assert_eq!(1, result[1].reloc_infos.len());
			} else {
				assert!(result[0].reloc_infos.is_empty());
				assert!(result[1].reloc_infos.is_empty());
			}
			if (options & BlockEncoderOptions::RETURN_NEW_INSTRUCTION_OFFSETS) != 0 {
				assert_eq!(1, result[0].new_instruction_offsets.len());
				assert_eq!(2, result[1].new_instruction_offsets.len());
			} else {
				assert!(result[0].new_instruction_offsets.is_empty());
				assert!(result[1].new_instruction_offsets.is_empty());
			}
			if (options & BlockEncoderOptions::RETURN_CONSTANT_OFFSETS) != 0 {
				assert_eq!(1, result[0].constant_offsets.len());
				assert_eq!(2, result[1].constant_offsets.len());
			} else {
				assert!(result[0].constant_offsets.is_empty());
				assert!(result[1].constant_offsets.is_empty());
			}
		}
	}
}

#[test]
#[cfg(feature = "db")]
fn encode_declare_byte() {
	const BITNESS: u32 = 64;
	const NEW_RIP: u64 = 0x8000_0000_0000_0000;

	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
	let test_data = [
		(vec![0x5A], vec![0x90, 0x5A, 0x90]),
		(vec![0xF0, 0xD2, 0x7A, 0x18, 0xA0], vec![0x90, 0xF0, 0xD2, 0x7A, 0x18, 0xA0, 0x90]),
		(vec![0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08], vec![0x90, 0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08, 0x90]),
	];

	for info in &test_data {
		#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
		let instructions = [
			Instruction::with(Code::Nopd),
			Instruction::with_declare_byte(&info.0),
			Instruction::with(Code::Nopd),
		];

		let result = BlockEncoder::encode(BITNESS, InstructionBlock::new(&instructions, NEW_RIP), BlockEncoderOptions::NONE).unwrap();
		assert_eq!(info.1, result.code_buffer);
		assert_eq!(NEW_RIP, result.rip);
		assert!(result.reloc_infos.is_empty());
		assert!(result.new_instruction_offsets.is_empty());
		assert!(result.constant_offsets.is_empty());
	}
}

#[test]
#[should_panic]
fn encode_with_invalid_bitness_throws_0() {
	let _ = BlockEncoder::encode(0, InstructionBlock::new(&[Instruction::default()], 0), BlockEncoderOptions::NONE);
}

#[test]
#[should_panic]
fn encode_with_invalid_bitness_throws_128() {
	let _ = BlockEncoder::encode(128, InstructionBlock::new(&[Instruction::default()], 0), BlockEncoderOptions::NONE);
}

#[test]
#[should_panic]
fn encode_slice_with_invalid_bitness_throws_0() {
	let _ = BlockEncoder::encode_slice(0, &[InstructionBlock::new(&[Instruction::default()], 0)], BlockEncoderOptions::NONE);
}

#[test]
#[should_panic]
fn encode_slice_with_invalid_bitness_throws_128() {
	let _ = BlockEncoder::encode_slice(128, &[InstructionBlock::new(&[Instruction::default()], 0)], BlockEncoderOptions::NONE);
}
