// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

mod decoder_mem_test_case;
mod decoder_test_case;
pub(crate) mod enums;
mod mem_test_parser;
mod misc_tests;
#[cfg(feature = "serde")]
mod serde_tests;
mod test_cases;
mod test_parser;
pub(crate) mod test_utils;

use crate::decoder::tests::decoder_mem_test_case::*;
use crate::decoder::tests::decoder_test_case::*;
use crate::decoder::tests::test_utils::*;
use crate::iced_constants::IcedConstants;
use crate::test_utils::from_str_conv::{code_names, is_ignored_code, to_vec_u8};
use crate::test_utils::*;
use crate::*;
use alloc::string::String;
use core::fmt::Write;

#[rustfmt::skip]
pub(crate) static NON_DECODED_CODE_VALUES: [Code; 17] = [
	Code::DeclareByte,
	Code::DeclareDword,
	Code::DeclareQword,
	Code::DeclareWord,
	Code::Zero_bytes,
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
];
#[rustfmt::skip]
pub(crate) static NON_DECODED_CODE_VALUES1632: [Code; 3] = [
	Code::Popw_CS,
	Code::Fstdw_AX,
	Code::Fstsg_AX,
];

#[test]
fn decoder_new() {
	let decoder = Decoder::new(64, b"", DecoderOptions::NONE);
	assert_eq!(decoder.ip(), 0);
}

#[test]
fn decoder_try_new() {
	let decoder = Decoder::try_new(64, b"", DecoderOptions::NONE).unwrap();
	assert_eq!(decoder.ip(), 0);
}

#[test]
fn decoder_with_ip() {
	let decoder = Decoder::with_ip(64, b"", 0x1234_5678_9ABC_DEF1, DecoderOptions::NONE);
	assert_eq!(decoder.ip(), 0x1234_5678_9ABC_DEF1);
}

#[test]
fn decoder_try_with_ip() {
	let decoder = Decoder::try_with_ip(64, b"", 0x1234_5678_9ABC_DEF1, DecoderOptions::NONE).unwrap();
	assert_eq!(decoder.ip(), 0x1234_5678_9ABC_DEF1);
}

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
	let (mut decoder, len, can_read) = create_decoder(bitness, &bytes, tc.ip, tc.decoder_options);
	assert_eq!(decoder.position(), 0);
	assert_eq!(decoder.max_position(), bytes.len());
	let rip = decoder.ip();
	let instr = decoder.decode();
	assert_eq!(decoder.last_error(), tc.decoder_error);
	assert_eq!(decoder.position(), len);
	assert_eq!(decoder.can_decode(), can_read);
	assert_eq!(instr.code(), tc.code);
	assert_eq!(instr.is_invalid(), tc.code == Code::INVALID);
	assert_eq!(instr.mnemonic(), tc.mnemonic);
	assert_eq!(instr.code().mnemonic(), instr.mnemonic());
	assert_eq!(instr.len(), len);
	assert_eq!(instr.ip(), rip);
	assert_eq!(instr.next_ip(), decoder.ip());
	assert_eq!(instr.next_ip(), rip.wrapping_add(len as u64));
	match bitness {
		16 => assert_eq!(instr.code_size(), CodeSize::Code16),
		32 => assert_eq!(instr.code_size(), CodeSize::Code32),
		64 => assert_eq!(instr.code_size(), CodeSize::Code64),
		_ => unreachable!(),
	}
	assert_eq!(instr.op_count(), tc.op_count);
	assert_eq!(instr.zeroing_masking(), tc.zeroing_masking);
	assert_eq!(instr.merging_masking(), !tc.zeroing_masking);
	assert_eq!(instr.suppress_all_exceptions(), tc.suppress_all_exceptions);
	assert_eq!(instr.is_broadcast(), tc.is_broadcast);
	assert_eq!(instr.has_xacquire_prefix(), tc.has_xacquire_prefix);
	assert_eq!(instr.has_xrelease_prefix(), tc.has_xrelease_prefix);
	assert_eq!(instr.has_rep_prefix(), tc.has_repe_prefix);
	assert_eq!(instr.has_repe_prefix(), tc.has_repe_prefix);
	assert_eq!(instr.has_repne_prefix(), tc.has_repne_prefix);
	assert_eq!(instr.has_lock_prefix(), tc.has_lock_prefix);
	#[cfg(feature = "mvex")]
	{
		assert_eq!(instr.is_mvex_eviction_hint(), tc.mvex.eviction_hint);
		assert_eq!(instr.mvex_reg_mem_conv(), tc.mvex.reg_mem_conv);
	}
	match tc.vsib_bitness {
		0 => {
			assert!(!instr.is_vsib());
			assert!(!instr.is_vsib32());
			assert!(!instr.is_vsib64());
			assert_eq!(instr.vsib(), None);
		}
		32 => {
			assert!(instr.is_vsib());
			assert!(instr.is_vsib32());
			assert!(!instr.is_vsib64());
			assert_eq!(instr.vsib(), Some(false));
		}
		64 => {
			assert!(instr.is_vsib());
			assert!(!instr.is_vsib32());
			assert!(instr.is_vsib64());
			assert_eq!(instr.vsib(), Some(true));
		}
		_ => unreachable!(),
	}
	assert_eq!(instr.op_mask(), tc.op_mask);
	assert_eq!(instr.has_op_mask(), tc.op_mask != Register::None);
	assert_eq!(instr.rounding_control(), tc.rounding_control);
	assert_eq!(instr.segment_prefix(), tc.segment_prefix);
	if instr.segment_prefix() == Register::None {
		assert!(!instr.has_segment_prefix());
	} else {
		assert!(instr.has_segment_prefix());
	}
	assert_eq!(instr.op_kinds().len(), instr.op_count() as usize);
	let op_kinds: Vec<OpKind> = instr.op_kinds().collect();
	assert_eq!(op_kinds.len(), instr.op_count() as usize);
	for (i, op_kind) in op_kinds.into_iter().enumerate() {
		assert_eq!(instr.op_kind(i as u32), op_kind);
		assert_eq!(instr.try_op_kind(i as u32).unwrap(), op_kind);
	}
	for i in 0..tc.op_count {
		let op_kind = tc.op_kinds[i as usize];
		assert_eq!(instr.op_kind(i), op_kind);
		assert_eq!(instr.try_op_kind(i).unwrap(), op_kind);
		match op_kind {
			OpKind::Register => {
				assert_eq!(instr.op_register(i), tc.op_registers[i as usize]);
				assert_eq!(instr.try_op_register(i).unwrap(), tc.op_registers[i as usize]);
			}
			OpKind::NearBranch16 => {
				assert_eq!(tc.near_branch, instr.near_branch16() as u64);
				assert_eq!(tc.near_branch, instr.near_branch_target());
			}
			OpKind::NearBranch32 => {
				assert_eq!(tc.near_branch, instr.near_branch32() as u64);
				assert_eq!(tc.near_branch, instr.near_branch_target());
			}
			OpKind::NearBranch64 => {
				assert_eq!(tc.near_branch, instr.near_branch64());
				assert_eq!(tc.near_branch, instr.near_branch_target());
			}
			OpKind::FarBranch16 => {
				assert_eq!(instr.far_branch16() as u32, tc.far_branch);
				assert_eq!(instr.far_branch_selector(), tc.far_branch_selector);
			}

			OpKind::FarBranch32 => {
				assert_eq!(instr.far_branch32(), tc.far_branch);
				assert_eq!(instr.far_branch_selector(), tc.far_branch_selector);
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
				assert_eq!(instr.memory_segment(), tc.memory_segment);
				assert_eq!(instr.memory_size(), tc.memory_size);
			}

			OpKind::MemoryESDI | OpKind::MemoryESEDI | OpKind::MemoryESRDI => assert_eq!(tc.memory_size, instr.memory_size()),

			OpKind::Memory => {
				assert_eq!(instr.memory_segment(), tc.memory_segment);
				assert_eq!(instr.memory_base(), tc.memory_base);
				assert_eq!(instr.memory_index(), tc.memory_index);
				assert_eq!(instr.memory_index_scale(), tc.memory_index_scale);
				assert_eq!(instr.memory_displacement32(), tc.memory_displacement as u32);
				assert_eq!(instr.memory_displacement64(), tc.memory_displacement);
				assert_eq!(instr.memory_displ_size(), tc.memory_displ_size);
				assert_eq!(instr.memory_size(), tc.memory_size);
			}
		}
	}
	if tc.op_count >= 1 {
		assert_eq!(instr.op0_kind(), tc.op_kinds[0]);
		if tc.op_kinds[0] == OpKind::Register {
			assert_eq!(instr.op0_register(), tc.op_registers[0]);
		}
		if tc.op_count >= 2 {
			assert_eq!(instr.op1_kind(), tc.op_kinds[1]);
			if tc.op_kinds[1] == OpKind::Register {
				assert_eq!(instr.op1_register(), tc.op_registers[1]);
			}
			if tc.op_count >= 3 {
				assert_eq!(instr.op2_kind(), tc.op_kinds[2]);
				if tc.op_kinds[2] == OpKind::Register {
					assert_eq!(instr.op2_register(), tc.op_registers[2]);
				}
				if tc.op_count >= 4 {
					assert_eq!(instr.op3_kind(), tc.op_kinds[3]);
					if tc.op_kinds[3] == OpKind::Register {
						assert_eq!(instr.op3_register(), tc.op_registers[3]);
					}
					if tc.op_count >= 5 {
						assert_eq!(instr.op4_kind(), tc.op_kinds[4]);
						if tc.op_kinds[4] == OpKind::Register {
							assert_eq!(instr.op4_register(), tc.op_registers[4]);
						}
						const _: () = assert!(IcedConstants::MAX_OP_COUNT == 5);
						assert_eq!(tc.op_count, 5);
					}
				}
			}
		}
	}
	verify_constant_offsets(&tc.constant_offsets, &decoder.get_constant_offsets(&instr));
}

fn verify_constant_offsets(expected: &ConstantOffsets, actual: &ConstantOffsets) {
	assert_eq!(actual.immediate_offset(), expected.immediate_offset());
	assert_eq!(actual.immediate_size(), expected.immediate_size());
	assert_eq!(actual.immediate_offset2(), expected.immediate_offset2());
	assert_eq!(actual.immediate_size2(), expected.immediate_size2());
	assert_eq!(actual.displacement_offset(), expected.displacement_offset());
	assert_eq!(actual.displacement_size(), expected.displacement_size());
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
	let (mut decoder, len, can_read) = create_decoder(bitness, &bytes, tc.ip, tc.decoder_options);
	assert_eq!(decoder.position(), 0);
	assert_eq!(decoder.max_position(), bytes.len());
	let instr = decoder.decode();
	assert_eq!(decoder.last_error(), DecoderError::None);
	assert_eq!(decoder.position(), len);
	assert_eq!(decoder.can_decode(), can_read);

	assert_eq!(instr.code(), tc.code);
	assert_eq!(instr.is_invalid(), tc.code == Code::INVALID);
	assert_eq!(instr.op_count(), 2);
	assert_eq!(instr.len(), len);
	assert!(!instr.has_rep_prefix());
	assert!(!instr.has_repe_prefix());
	assert!(!instr.has_repne_prefix());
	assert!(!instr.has_lock_prefix());
	assert_eq!(instr.segment_prefix(), tc.prefix_segment);
	if instr.segment_prefix() == Register::None {
		assert!(!instr.has_segment_prefix());
	} else {
		assert!(instr.has_segment_prefix());
	}

	assert_eq!(instr.op0_kind(), OpKind::Memory);
	assert_eq!(instr.memory_segment(), tc.segment);
	assert_eq!(instr.memory_base(), tc.base_register);
	assert_eq!(instr.memory_index(), tc.index_register);
	assert_eq!(instr.memory_displacement32(), tc.displacement as u32);
	assert_eq!(instr.memory_displacement64(), tc.displacement);
	assert_eq!(instr.memory_index_scale(), 1 << tc.scale);
	assert_eq!(instr.memory_displ_size(), tc.displ_size);

	assert_eq!(instr.op1_kind(), OpKind::Register);
	assert_eq!(instr.op1_register(), tc.register);
	verify_constant_offsets(&tc.constant_offsets, &decoder.get_constant_offsets(&instr));
}

#[test]
fn make_sure_all_code_values_are_tested_in_16_32_64_bit_modes() {
	const T16: u8 = 0x01;
	const T32: u8 = 0x02;
	const T64: u8 = 0x04;
	let mut tested = [0u8; IcedConstants::CODE_ENUM_COUNT];
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
		for info in crate::encoder::tests::non_decoded_tests::get_tests() {
			tested[info.2.code() as usize] |= match info.0 {
				16 => T16,
				32 => T32,
				64 => T64,
				_ => unreachable!(),
			}
		}
	} else {
		for &code in &NON_DECODED_CODE_VALUES1632 {
			tested[code as usize] |= T16 | T32;
		}
		for &code in &NON_DECODED_CODE_VALUES {
			tested[code as usize] |= T16 | T32 | T64;
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
	#[allow(clippy::needless_range_loop)]
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
	assert_eq!(format!("16: {} ins {}", missing16, sb16), "16: 0 ins ");
	assert_eq!(format!("32: {} ins {}", missing32, sb32), "32: 0 ins ");
	assert_eq!(format!("64: {} ins {}", missing64, sb64), "64: 0 ins ");
}
