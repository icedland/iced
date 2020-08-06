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

mod decoder_mem_test_case;
mod decoder_test_case;
pub(crate) mod enums;
mod mem_test_parser;
mod misc_tests;
mod test_cases;
mod test_parser;
pub(crate) mod test_utils;

use self::decoder_mem_test_case::*;
use self::decoder_test_case::*;
use self::test_utils::*;
use super::super::iced_constants::IcedConstants;
use super::super::test_utils::from_str_conv::{code_names, is_ignored_code, to_vec_u8};
use super::super::test_utils::*;
use super::super::*;
#[cfg(not(feature = "std"))]
use alloc::string::String;
use core::fmt::Write;

#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
pub(crate) static NON_DECODED_CODE_VALUES: [Code; 19] = [
	Code::DeclareByte,
	Code::DeclareDword,
	Code::DeclareQword,
	Code::DeclareWord,
	Code::Fclex,
	Code::Fdisi,
	Code::Feni,
	Code::Finit,
	Code::Fsave_m108byte,
	Code::Fsave_m94byte,
	Code::Fsetpm,
	Code::Fstcw_m2byte,
	Code::Fstenv_m14byte,
	Code::Fstenv_m28byte,
	Code::Fstsw_AX,
	Code::Fstsw_m2byte,
	Code::Popw_CS,
	Code::Fstdw_AX,
	Code::Fstsg_AX,
];

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

fn decode(bitness: u32) {
	for info in test_cases::get_test_cases(bitness) {
		decode_test(bitness, info);
	}
}

#[test]
fn decode_misc_16() {
	decode_misc(16);
}

#[test]
fn decode_misc_32() {
	decode_misc(32);
}

#[test]
fn decode_misc_64() {
	decode_misc(64);
}

fn decode_misc(bitness: u32) {
	for info in test_cases::get_misc_test_cases(bitness) {
		decode_test(bitness, info);
	}
}

fn decode_test(bitness: u32, tc: &DecoderTestCase) {
	let bytes = to_vec_u8(&tc.hex_bytes).unwrap();
	let (mut decoder, len, can_read) = create_decoder(bitness, &bytes, tc.decoder_options);
	assert_eq!(0, decoder.position());
	assert_eq!(bytes.len(), decoder.max_position());
	let rip = decoder.ip();
	let instr = decoder.decode();
	assert_eq!(tc.decoder_error, decoder.last_error());
	assert_eq!(len, decoder.position());
	assert_eq!(can_read, decoder.can_decode());
	assert_eq!(tc.code, instr.code());
	assert_eq!(tc.mnemonic, instr.mnemonic());
	assert_eq!(instr.mnemonic(), instr.code().mnemonic());
	assert_eq!(len, instr.len());
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
			assert!(!instr.is_vsib());
			assert!(!instr.is_vsib32());
			assert!(!instr.is_vsib64());
			assert_eq!(None, instr.vsib());
		}
		32 => {
			assert!(instr.is_vsib());
			assert!(instr.is_vsib32());
			assert!(!instr.is_vsib64());
			assert_eq!(Some(false), instr.vsib());
		}
		64 => {
			assert!(instr.is_vsib());
			assert!(!instr.is_vsib32());
			assert!(instr.is_vsib64());
			assert_eq!(Some(true), instr.vsib());
		}
		_ => panic!(),
	}
	assert_eq!(tc.op_mask, instr.op_mask());
	assert_eq!(tc.op_mask != Register::None, instr.has_op_mask());
	assert_eq!(tc.rounding_control, instr.rounding_control());
	assert_eq!(tc.segment_prefix, instr.segment_prefix());
	if instr.segment_prefix() == Register::None {
		assert!(!instr.has_segment_prefix());
	} else {
		assert!(instr.has_segment_prefix());
	}
	for i in 0..tc.op_count {
		let op_kind = tc.op_kind(i);
		assert_eq!(op_kind, instr.op_kind(i));
		match op_kind {
			OpKind::Register => assert_eq!(tc.op_register(i), instr.op_register(i)),
			OpKind::NearBranch16 => assert_eq!(tc.near_branch, instr.near_branch16() as u64),
			OpKind::NearBranch32 => assert_eq!(tc.near_branch, instr.near_branch32() as u64),
			OpKind::NearBranch64 => assert_eq!(tc.near_branch, instr.near_branch64()),
			OpKind::FarBranch16 => {
				assert_eq!(tc.far_branch, instr.far_branch16() as u32);
				assert_eq!(tc.far_branch_selector, instr.far_branch_selector());
			}

			OpKind::FarBranch32 => {
				assert_eq!(tc.far_branch, instr.far_branch32());
				assert_eq!(tc.far_branch_selector, instr.far_branch_selector());
			}

			OpKind::Immediate8 => assert_eq!(tc.immediate as u8, instr.immediate8()),
			OpKind::Immediate8_2nd => assert_eq!(tc.immediate_2nd, instr.immediate8_2nd()),
			OpKind::Immediate16 => assert_eq!(tc.immediate as u16, instr.immediate16()),
			OpKind::Immediate32 => assert_eq!(tc.immediate as u32, instr.immediate32()),
			OpKind::Immediate64 => assert_eq!(tc.immediate, instr.immediate64()),
			OpKind::Immediate8to16 => assert_eq!(tc.immediate as i16, instr.immediate8to16()),
			OpKind::Immediate8to32 => assert_eq!(tc.immediate as i32, instr.immediate8to32()),
			OpKind::Immediate8to64 => assert_eq!(tc.immediate as i64, instr.immediate8to64()),
			OpKind::Immediate32to64 => assert_eq!(tc.immediate as i64, instr.immediate32to64()),
			OpKind::MemorySegSI | OpKind::MemorySegESI | OpKind::MemorySegRSI | OpKind::MemorySegDI | OpKind::MemorySegEDI | OpKind::MemorySegRDI => {
				assert_eq!(tc.memory_segment, instr.memory_segment());
				assert_eq!(tc.memory_size, instr.memory_size());
			}

			OpKind::MemoryESDI | OpKind::MemoryESEDI | OpKind::MemoryESRDI => assert_eq!(tc.memory_size, instr.memory_size()),
			OpKind::Memory64 => {
				assert_eq!(tc.memory_segment, instr.memory_segment());
				assert_eq!(tc.memory_address64, instr.memory_address64());
				assert_eq!(tc.memory_size, instr.memory_size());
			}

			OpKind::Memory => {
				assert_eq!(tc.memory_segment, instr.memory_segment());
				assert_eq!(tc.memory_base, instr.memory_base());
				assert_eq!(tc.memory_index, instr.memory_index());
				assert_eq!(tc.memory_index_scale, instr.memory_index_scale());
				assert_eq!(tc.memory_displacement, instr.memory_displacement());
				assert_eq!(tc.memory_displacement as i32 as u64, instr.memory_displacement64());
				assert_eq!(tc.memory_displ_size, instr.memory_displ_size());
				assert_eq!(tc.memory_size, instr.memory_size());
			}
		}
	}
	if tc.op_count >= 1 {
		assert_eq!(tc.op0_kind, instr.op0_kind());
		if tc.op0_kind == OpKind::Register {
			assert_eq!(tc.op0_register, instr.op0_register());
		}
		if tc.op_count >= 2 {
			assert_eq!(tc.op1_kind, instr.op1_kind());
			if tc.op1_kind == OpKind::Register {
				assert_eq!(tc.op1_register, instr.op1_register());
			}
			if tc.op_count >= 3 {
				assert_eq!(tc.op2_kind, instr.op2_kind());
				if tc.op2_kind == OpKind::Register {
					assert_eq!(tc.op2_register, instr.op2_register());
				}
				if tc.op_count >= 4 {
					assert_eq!(tc.op3_kind, instr.op3_kind());
					if tc.op3_kind == OpKind::Register {
						assert_eq!(tc.op3_register, instr.op3_register());
					}
					if tc.op_count >= 5 {
						assert_eq!(tc.op4_kind, instr.op4_kind());
						if tc.op4_kind == OpKind::Register {
							assert_eq!(tc.op4_register, instr.op4_register());
						}
						assert_eq!(5, tc.op_count);
					}
				}
			}
		}
	}
	verify_constant_offsets(&tc.constant_offsets, &decoder.get_constant_offsets(&instr));
}

#[cfg_attr(feature = "cargo-clippy", allow(clippy::trivially_copy_pass_by_ref))]
fn verify_constant_offsets(expected: &ConstantOffsets, actual: &ConstantOffsets) {
	assert_eq!(expected.immediate_offset(), actual.immediate_offset());
	assert_eq!(expected.immediate_size(), actual.immediate_size());
	assert_eq!(expected.immediate_offset2(), actual.immediate_offset2());
	assert_eq!(expected.immediate_size2(), actual.immediate_size2());
	assert_eq!(expected.displacement_offset(), actual.displacement_offset());
	assert_eq!(expected.displacement_size(), actual.displacement_size());
}

#[test]
fn decode_mem_16() {
	decode_mem(16);
}

#[test]
fn decode_mem_32() {
	decode_mem(32);
}

#[test]
fn decode_mem_64() {
	decode_mem(64);
}

fn decode_mem(bitness: u32) {
	for info in test_cases::get_mem_test_cases(bitness) {
		decode_mem_test(bitness, info);
	}
}

fn decode_mem_test(bitness: u32, tc: &DecoderMemoryTestCase) {
	let bytes = to_vec_u8(&tc.hex_bytes).unwrap();
	let (mut decoder, len, can_read) = create_decoder(bitness, &bytes, tc.decoder_options);
	assert_eq!(0, decoder.position());
	assert_eq!(bytes.len(), decoder.max_position());
	let instr = decoder.decode();
	assert_eq!(DecoderError::None, decoder.last_error());
	assert_eq!(len, decoder.position());
	assert_eq!(can_read, decoder.can_decode());

	assert_eq!(tc.code, instr.code());
	assert_eq!(2, instr.op_count());
	assert_eq!(len, instr.len());
	assert!(!instr.has_rep_prefix());
	assert!(!instr.has_repe_prefix());
	assert!(!instr.has_repne_prefix());
	assert!(!instr.has_lock_prefix());
	assert_eq!(tc.prefix_segment, instr.segment_prefix());
	if instr.segment_prefix() == Register::None {
		assert!(!instr.has_segment_prefix());
	} else {
		assert!(instr.has_segment_prefix());
	}

	assert_eq!(OpKind::Memory, instr.op0_kind());
	assert_eq!(tc.segment, instr.memory_segment());
	assert_eq!(tc.base_register, instr.memory_base());
	assert_eq!(tc.index_register, instr.memory_index());
	assert_eq!(tc.displacement, instr.memory_displacement());
	assert_eq!(tc.displacement as i32 as u64, instr.memory_displacement64());
	assert_eq!(1 << tc.scale, instr.memory_index_scale());
	assert_eq!(tc.displ_size, instr.memory_displ_size());

	assert_eq!(OpKind::Register, instr.op1_kind());
	assert_eq!(tc.register, instr.op1_register());
	verify_constant_offsets(&tc.constant_offsets, &decoder.get_constant_offsets(&instr));
}

#[test]
fn make_sure_all_code_values_are_tested_in_16_32_64_bit_modes() {
	const T16: u8 = 0x01;
	const T32: u8 = 0x02;
	const T64: u8 = 0x04;
	let mut tested = [0u8; IcedConstants::NUMBER_OF_CODE_VALUES];
	tested[Code::INVALID as usize] = T16 | T32 | T64;

	for info in decoder_tests(false, false) {
		assert!(!not_decoded().contains(&info.code()));

		tested[info.code() as usize] |= match info.bitness() {
			16 => T16,
			32 => T32,
			64 => T64,
			_ => unreachable!(),
		}
	}

	if cfg!(feature = "encoder") {
		#[cfg(feature = "encoder")] // needed...
		for info in super::super::encoder::tests::non_decoded_tests::get_tests() {
			tested[info.2.code() as usize] |= match info.0 {
				16 => T16,
				32 => T32,
				64 => T64,
				_ => unreachable!(),
			}
		}
	} else {
		for &code in &NON_DECODED_CODE_VALUES {
			if code == Code::Popw_CS {
				tested[code as usize] |= T16 | T32;
			} else {
				tested[code as usize] |= T16 | T32 | T64;
			}
		}
	}

	for c in not_decoded() {
		assert!(!code32_only().contains(c));
		assert!(!code64_only().contains(c));
	}

	for &c in not_decoded32_only() {
		tested[c as usize] ^= T64;
	}
	for &c in not_decoded64_only() {
		tested[c as usize] ^= T16 | T32;
	}

	for c in code32_only() {
		assert!(!code64_only().contains(c));
		tested[*c as usize] ^= T64;
	}

	for c in code64_only() {
		assert!(!code32_only().contains(c));
		tested[*c as usize] ^= T16 | T32;
	}

	let mut sb16 = String::new();
	let mut sb32 = String::new();
	let mut sb64 = String::new();
	let mut missing16 = 0;
	let mut missing32 = 0;
	let mut missing64 = 0;
	let code_names = code_names();
	#[cfg_attr(feature = "cargo-clippy", allow(clippy::needless_range_loop))]
	for i in 0..tested.len() {
		if tested[i] != (T16 | T32 | T64) && !is_ignored_code(code_names[i]) {
			if (tested[i] & T16) == 0 {
				write!(sb16, "{} ", code_names[i]).unwrap();
				missing16 += 1;
			}
			if (tested[i] & T32) == 0 {
				write!(sb32, "{} ", code_names[i]).unwrap();
				missing32 += 1;
			}
			if (tested[i] & T64) == 0 {
				write!(sb64, "{} ", code_names[i]).unwrap();
				missing64 += 1;
			}
		}
	}
	assert_eq!("16: 0 ins ", format!("16: {} ins {}", missing16, sb16));
	assert_eq!("32: 0 ins ", format!("32: {} ins {}", missing32, sb32));
	assert_eq!("64: 0 ins ", format!("64: {} ins {}", missing64, sb64));
}
