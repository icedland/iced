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

mod decoder_test_case;
mod test_cases;
mod test_parser;

use self::decoder_test_case::*;
use super::super::test_utils::from_str_conv::to_vec_u8;
use super::super::test_utils::*;
use super::super::*;

#[test]
fn decode_16() {
	decode(16);
}

#[test]
fn decode_32() {
	decode(32);
}

#[test]
fn decode_64() {
	decode(64);
}

fn decode(bitness: i32) {
	for info in test_cases::get_test_cases(bitness).iter() {
		decode_test(bitness, &info);
	}
}

fn create_decoder<'a>(bitness: i32, bytes: &'a [u8], options: u32) -> Decoder<'a> {
	let mut decoder = Decoder::new(bitness, bytes, options);
	decoder.set_ip(get_default_ip(bitness));
	decoder
}

fn decode_test(bitness: i32, tc: &DecoderTestCase) {
	let bytes = to_vec_u8(&tc.hex_bytes).expect("Couldn't parse hex bytes");
	let mut decoder = create_decoder(bitness, &bytes, tc.decoder_options);
	assert_eq!(0, decoder.data_index());
	assert_eq!(bytes.len(), decoder.max_data_index());
	let rip = decoder.ip();
	let instr = decoder.decode();
	assert_eq!(bytes.len(), decoder.data_index());
	assert_eq!(false, decoder.can_decode());
	assert_eq!(tc.code as u32, instr.code() as u32);
	assert_eq!(tc.mnemonic as u32, instr.mnemonic() as u32);
	assert_eq!(instr.mnemonic() as u32, instr.code().to_mnemonic() as u32);
	assert_eq!(bytes.len(), instr.len() as usize);
	assert_eq!(rip, instr.ip());
	assert_eq!(decoder.ip(), instr.next_ip());
	assert_eq!(tc.op_count, instr.op_count());
	assert_eq!(tc.zeroing_masking, instr.zeroing_masking());
	assert_eq!(!tc.zeroing_masking, instr.merging_masking());
	assert_eq!(tc.suppress_all_exceptions, instr.suppress_all_exceptions());
	assert_eq!(tc.is_broadcast, instr.is_broadcast());
	assert_eq!(tc.has_xacquire_prefix, instr.has_xacquire_prefix());
	assert_eq!(tc.has_xrelease_prefix, instr.has_xrelease_prefix());
	assert_eq!(tc.has_repe_prefix, instr.has_rep_prefix());
	assert_eq!(tc.has_repe_prefix, instr.has_repe_prefix());
	assert_eq!(tc.has_repne_prefix, instr.has_repne_prefix());
	assert_eq!(tc.has_lock_prefix, instr.has_lock_prefix());
	match tc.vsib_bitness {
		0 => {
			assert_eq!(false, instr.is_vsib());
			assert_eq!(false, instr.is_vsib32());
			assert_eq!(false, instr.is_vsib64());
			assert_eq!(None, instr.vsib());
		}
		32 => {
			assert_eq!(true, instr.is_vsib());
			assert_eq!(true, instr.is_vsib32());
			assert_eq!(false, instr.is_vsib64());
			assert_eq!(Some(false), instr.vsib());
		}
		64 => {
			assert_eq!(true, instr.is_vsib());
			assert_eq!(false, instr.is_vsib32());
			assert_eq!(true, instr.is_vsib64());
			assert_eq!(Some(true), instr.vsib());
		}
		_ => panic!(),
	}
	assert!(tc.op_mask == instr.op_mask());
	assert_eq!(tc.op_mask != Register::None, instr.has_op_mask());
	assert!(tc.rounding_control == instr.rounding_control());
	assert!(tc.segment_prefix == instr.segment_prefix());
	if instr.segment_prefix() == Register::None {
		assert_eq!(false, instr.has_segment_prefix());
	} else {
		assert_eq!(true, instr.has_segment_prefix());
	}
	for i in 0..tc.op_count {
		let op_kind = tc.op_kind(i);
		assert!(op_kind == instr.op_kind(i));
		match op_kind {
			OpKind::Register => assert!(tc.op_register(i) == instr.op_register(i)),
			OpKind::NearBranch16 => assert!(tc.near_branch == instr.near_branch16() as u64),
			OpKind::NearBranch32 => assert!(tc.near_branch == instr.near_branch32() as u64),
			OpKind::NearBranch64 => assert!(tc.near_branch == instr.near_branch64()),
			OpKind::FarBranch16 => {
				assert!(tc.far_branch == instr.far_branch16() as u32);
				assert!(tc.far_branch_selector == instr.far_branch_selector());
			}

			OpKind::FarBranch32 => {
				assert!(tc.far_branch == instr.far_branch32());
				assert!(tc.far_branch_selector == instr.far_branch_selector());
			}

			OpKind::Immediate8 => assert!(tc.immediate as u8 == instr.immediate8()),
			OpKind::Immediate8_2nd => assert!(tc.immediate_2nd == instr.immediate8_2nd()),
			OpKind::Immediate16 => assert!(tc.immediate as u16 == instr.immediate16()),
			OpKind::Immediate32 => assert!(tc.immediate as u32 == instr.immediate32()),
			OpKind::Immediate64 => assert!(tc.immediate == instr.immediate64()),
			OpKind::Immediate8to16 => assert!(tc.immediate as i16 == instr.immediate8to16()),
			OpKind::Immediate8to32 => assert!(tc.immediate as i32 == instr.immediate8to32()),
			OpKind::Immediate8to64 => assert!(tc.immediate as i64 == instr.immediate8to64()),
			OpKind::Immediate32to64 => assert!(tc.immediate as i64 == instr.immediate32to64()),
			OpKind::MemorySegSI | OpKind::MemorySegESI | OpKind::MemorySegRSI | OpKind::MemorySegDI | OpKind::MemorySegEDI | OpKind::MemorySegRDI => {
				assert!(tc.memory_segment == instr.memory_segment());
				assert!(tc.memory_size == instr.memory_size());
			}

			OpKind::MemoryESDI | OpKind::MemoryESEDI | OpKind::MemoryESRDI => assert!(tc.memory_size == instr.memory_size()),
			OpKind::Memory64 => {
				assert!(tc.memory_segment == instr.memory_segment());
				assert!(tc.memory_address64 == instr.memory_address64());
				assert!(tc.memory_size == instr.memory_size());
			}

			OpKind::Memory => {
				assert!(tc.memory_segment == instr.memory_segment());
				assert!(tc.memory_base == instr.memory_base());
				assert!(tc.memory_index == instr.memory_index());
				assert!(tc.memory_index_scale == instr.memory_index_scale());
				assert!(tc.memory_displacement == instr.memory_displacement());
				assert!(tc.memory_displacement as i32 as u64 == instr.memory_displacement64());
				assert!(tc.memory_displ_size == instr.memory_displ_size());
				assert!(tc.memory_size == instr.memory_size());
			}
		}
	}
	if tc.op_count >= 1 {
		assert!(tc.op0_kind == instr.op0_kind());
		if tc.op0_kind == OpKind::Register {
			assert!(tc.op0_register == instr.op0_register());
		}
		if tc.op_count >= 2 {
			assert!(tc.op1_kind == instr.op1_kind());
			if tc.op1_kind == OpKind::Register {
				assert!(tc.op1_register == instr.op1_register());
			}
			if tc.op_count >= 3 {
				assert!(tc.op2_kind == instr.op2_kind());
				if tc.op2_kind == OpKind::Register {
					assert!(tc.op2_register == instr.op2_register());
				}
				if tc.op_count >= 4 {
					assert!(tc.op3_kind == instr.op3_kind());
					if tc.op3_kind == OpKind::Register {
						assert!(tc.op3_register == instr.op3_register());
					}
					if tc.op_count >= 5 {
						assert!(tc.op4_kind == instr.op4_kind());
						if tc.op4_kind == OpKind::Register {
							assert!(tc.op4_register == instr.op4_register());
						}
						assert!(5 == tc.op_count);
					}
				}
			}
		}
	}
	verify_constant_offsets(&tc.constant_offsets, &decoder.get_constant_offsets(&instr));
}

fn verify_constant_offsets(expected: &ConstantOffsets, actual: &ConstantOffsets) {
	assert_eq!(expected.immediate_offset(), actual.immediate_offset());
	assert_eq!(expected.immediate_size(), actual.immediate_size());
	assert_eq!(expected.immediate_offset2(), actual.immediate_offset2());
	assert_eq!(expected.immediate_size2(), actual.immediate_size2());
	assert_eq!(expected.displacement_offset(), actual.displacement_offset());
	assert_eq!(expected.displacement_size(), actual.displacement_size());
}
