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

use super::super::test_utils::*;
use super::super::*;
#[cfg(not(feature = "std"))]
use alloc::vec::Vec;
use core::cmp::Ordering;
use core::u32;

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
mod jmp_16;
mod jmp_32;
mod jmp_64;
mod misc;
mod xbegin_16;
mod xbegin_32;
mod xbegin_64;

const DECODER_OPTIONS: u32 = 0; // DecoderOptions

fn decode(bitness: u32, rip: u64, data: &[u8], options: u32) -> Vec<Instruction> {
	let mut decoder = create_decoder(bitness, data, options).0;
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

#[cfg_attr(feature = "cargo-clippy", allow(clippy::too_many_arguments))]
fn encode_test(
	bitness: u32, orig_rip: u64, original_data: &[u8], new_rip: u64, new_data: &[u8], mut options: u32, decoder_options: u32,
	expected_instruction_offsets: &[u32], expected_reloc_infos: &[RelocInfo],
) {
	let orig_instrs = decode(bitness, orig_rip, original_data, decoder_options);
	options |=
		BlockEncoderOptions::RETURN_RELOC_INFOS | BlockEncoderOptions::RETURN_NEW_INSTRUCTION_OFFSETS | BlockEncoderOptions::RETURN_CONSTANT_OFFSETS;
	let result = BlockEncoder::encode(bitness, InstructionBlock::new(&orig_instrs, new_rip), options).unwrap();
	let encoded_bytes = result.code_buffer;
	assert_eq!(new_data, &encoded_bytes[..]);
	assert_eq!(new_rip, result.rip);
	let reloc_infos = result.reloc_infos;
	let new_instruction_offsets = result.new_instruction_offsets;
	let constant_offsets = result.constant_offsets;
	assert_eq!(orig_instrs.len(), new_instruction_offsets.len());
	assert_eq!(orig_instrs.len(), constant_offsets.len());
	assert_eq!(sort(expected_reloc_infos.to_vec()), sort(reloc_infos));
	assert_eq!(expected_instruction_offsets, &new_instruction_offsets[..]);

	let mut expected_constant_offsets = Vec::with_capacity(constant_offsets.len());
	let mut decoder = create_decoder(bitness, &encoded_bytes, decoder_options).0;
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
	assert_eq!(expected_constant_offsets, constant_offsets);
}
