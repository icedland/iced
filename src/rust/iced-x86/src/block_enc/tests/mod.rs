// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::test_utils::*;
use crate::*;
use alloc::vec::Vec;
use core::cmp::Ordering;

mod br8_16;
mod br8_32;
mod br8_64;
mod call_16;
mod call_32;
mod call_64;
mod ip_rel_64;
mod jcc_16;
mod jcc_32;
mod jcc_64;
#[cfg(feature = "mvex")]
mod jkcc_64;
mod jmp_16;
mod jmp_32;
mod jmp_64;
mod misc;
mod xbegin_16;
mod xbegin_32;
mod xbegin_64;

const DECODER_OPTIONS: u32 = DecoderOptions::NONE;

fn decode(bitness: u32, rip: u64, data: &[u8], options: u32) -> Vec<Instruction> {
	let mut decoder = create_decoder(bitness, data, get_default_ip(bitness), options).0;
	decoder.set_ip(rip);
	decoder.into_iter().collect()
}

fn sort(mut vec: Vec<RelocInfo>) -> Vec<RelocInfo> {
	vec.sort_unstable_by(|a, b| {
		let c = a.address.cmp(&b.address);
		if c != Ordering::Equal {
			c
		} else {
			a.kind.cmp(&b.kind)
		}
	});
	vec
}

#[allow(clippy::too_many_arguments)]
fn encode_test(
	bitness: u32, orig_rip: u64, original_data: &[u8], new_rip: u64, new_data: &[u8], mut options: u32, decoder_options: u32,
	expected_instruction_offsets: &[u32], expected_reloc_infos: &[RelocInfo],
) {
	let orig_instrs = decode(bitness, orig_rip, original_data, decoder_options);
	options |=
		BlockEncoderOptions::RETURN_RELOC_INFOS | BlockEncoderOptions::RETURN_NEW_INSTRUCTION_OFFSETS | BlockEncoderOptions::RETURN_CONSTANT_OFFSETS;
	let result = BlockEncoder::encode(bitness, InstructionBlock::new(&orig_instrs, new_rip), options).unwrap();
	let encoded_bytes = result.code_buffer;
	assert_eq!(&encoded_bytes[..], new_data);
	assert_eq!(result.rip, new_rip);
	let reloc_infos = result.reloc_infos;
	let new_instruction_offsets = result.new_instruction_offsets;
	let constant_offsets = result.constant_offsets;
	assert_eq!(new_instruction_offsets.len(), orig_instrs.len());
	assert_eq!(constant_offsets.len(), orig_instrs.len());
	assert_eq!(sort(reloc_infos), sort(expected_reloc_infos.to_vec()));
	assert_eq!(&new_instruction_offsets[..], expected_instruction_offsets);

	let mut expected_constant_offsets = Vec::with_capacity(constant_offsets.len());
	let mut decoder = create_decoder(bitness, &encoded_bytes, get_default_ip(bitness), decoder_options).0;
	let mut instr = Instruction::default();
	for &offset in &new_instruction_offsets {
		if offset == u32::MAX {
			expected_constant_offsets.push(ConstantOffsets::default());
		} else {
			decoder.try_set_position(offset as usize).unwrap();
			decoder.set_ip(new_rip.wrapping_add(offset as u64));
			decoder.decode_out(&mut instr);
			expected_constant_offsets.push(decoder.get_constant_offsets(&instr));
		}
	}
	assert_eq!(constant_offsets, expected_constant_offsets);
}
