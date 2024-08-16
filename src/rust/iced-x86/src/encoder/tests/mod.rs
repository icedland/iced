// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

mod create;
#[cfg(feature = "op_code_info")]
mod dec_enc;
pub(crate) mod non_decoded_tests;
#[cfg(feature = "op_code_info")]
mod op_code_test_case;
#[cfg(feature = "op_code_info")]
mod op_code_test_case_parser;

use crate::decoder::tests::test_utils::*;
use crate::encoder::op_code_handler::InvalidHandler;
#[cfg(feature = "op_code_info")]
use crate::encoder::tests::op_code_test_case::*;
#[cfg(feature = "op_code_info")]
use crate::encoder::tests::op_code_test_case_parser::OpCodeInfoTestParser;
use crate::iced_constants::IcedConstants;
use crate::test_utils::from_str_conv::to_vec_u8;
#[cfg(feature = "op_code_info")]
use crate::test_utils::from_str_conv::{code_names, is_ignored_code};
use crate::test_utils::*;
use crate::*;
use alloc::rc::Rc;
use alloc::string::String;
use alloc::vec::Vec;
use core::fmt::Write;
use std::panic;

#[test]
fn encode_16() {
	encode(16);
}

#[test]
fn encode_32() {
	encode(32);
}

#[test]
fn encode_64() {
	encode(64);
}

fn encode(bitness: u32) {
	for info in &encoder_tests(true, false) {
		if info.bitness() == bitness {
			encode_test(info);
		}
	}
}

fn encode_test(info: &DecoderTestInfo) {
	let orig_bytes = to_vec_u8(info.hex_bytes()).unwrap();
	let mut decoder = create_decoder(info.bitness(), orig_bytes.as_slice(), info.ip(), info.decoder_options()).0;
	let orig_rip = decoder.ip();
	let orig_instr = decoder.decode();
	let orig_co = decoder.get_constant_offsets(&orig_instr);
	assert_eq!(orig_instr.code(), info.code());
	assert_eq!(orig_instr.len(), orig_bytes.len());
	assert!(orig_instr.len() <= IcedConstants::MAX_INSTRUCTION_LENGTH);
	assert_eq!(orig_instr.ip16(), orig_rip as u16);
	assert_eq!(orig_instr.ip32(), orig_rip as u32);
	assert_eq!(orig_instr.ip(), orig_rip);
	let after_rip = decoder.ip();
	assert_eq!(orig_instr.next_ip16(), after_rip as u16);
	assert_eq!(orig_instr.next_ip32(), after_rip as u32);
	assert_eq!(orig_instr.next_ip(), after_rip);

	let mut encoder = Encoder::new(decoder.bitness());
	assert_eq!(encoder.bitness(), info.bitness());
	let orig_instr_copy = orig_instr;
	let encoded_instr_len;
	encoded_instr_len = encoder.encode(&orig_instr, orig_rip).unwrap();
	let mut encoded_co = encoder.get_constant_offsets();
	fix_constant_offsets(&mut encoded_co, orig_instr.len(), encoded_instr_len);
	verify_constant_offsets(&orig_co, &encoded_co);
	let encoded_bytes = encoder.take_buffer();
	assert_eq!(encoded_instr_len, encoded_bytes.len());
	assert!(orig_instr.eq_all_bits(&orig_instr_copy));

	let expected_bytes = to_vec_u8(info.encoded_hex_bytes()).unwrap();
	if expected_bytes != encoded_bytes {
		assert_eq!(slice_u8_to_string(encoded_bytes.as_slice()), slice_u8_to_string(expected_bytes.as_slice()));
		panic!();
	}

	let mut new_instr = create_decoder(info.bitness(), encoded_bytes.as_slice(), info.ip(), info.decoder_options()).0.decode();
	assert_eq!(new_instr.code(), info.code());
	assert_eq!(new_instr.len(), encoded_bytes.len());
	new_instr.set_len(orig_instr.len());
	new_instr.set_next_ip(orig_instr.next_ip());
	assert!(orig_instr.eq_all_bits(&new_instr));
}

fn fix_constant_offsets(co: &mut ConstantOffsets, orig_len: usize, new_len: usize) {
	let diff = orig_len.wrapping_sub(new_len) as u8;
	if co.has_displacement() {
		co.displacement_offset = co.displacement_offset.wrapping_add(diff);
	}
	if co.has_immediate() {
		co.immediate_offset = co.immediate_offset.wrapping_add(diff);
	}
	if co.has_immediate2() {
		co.immediate_offset2 = co.immediate_offset2.wrapping_add(diff);
	}
}

fn verify_constant_offsets(expected: &ConstantOffsets, actual: &ConstantOffsets) {
	assert_eq!(actual.immediate_offset(), expected.immediate_offset());
	assert_eq!(actual.immediate_size(), expected.immediate_size());
	assert_eq!(actual.immediate_offset2(), expected.immediate_offset2());
	assert_eq!(actual.immediate_size2(), expected.immediate_size2());
	assert_eq!(actual.displacement_offset(), expected.displacement_offset());
	assert_eq!(actual.displacement_size(), expected.displacement_size());
}

fn slice_u8_to_string(bytes: &[u8]) -> String {
	if bytes.is_empty() {
		return String::new();
	}
	let mut s = String::with_capacity(bytes.len() * 3 - 1);
	for b in bytes {
		if !s.is_empty() {
			s.push(' ');
		}
		write!(s, "{:02X}", b).unwrap();
	}
	s
}

#[test]
fn non_decode_encode_16() {
	non_decode_encode(16);
}

#[test]
fn non_decode_encode_32() {
	non_decode_encode(32);
}

#[test]
fn non_decode_encode_64() {
	non_decode_encode(64);
}

fn non_decode_encode(bitness: u32) {
	const RIP: u64 = 0;
	for tc in non_decoded_tests::get_tests() {
		if tc.0 != bitness {
			continue;
		}
		let expected_bytes = to_vec_u8(tc.1).unwrap();
		let mut encoder = Encoder::new(bitness);
		assert_eq!(encoder.bitness(), bitness);
		let encoded_instr_len = encoder.encode(&tc.2, RIP).unwrap();
		let encoded_bytes = encoder.take_buffer();
		assert_eq!(encoded_bytes, expected_bytes);
		assert_eq!(encoded_instr_len, encoded_bytes.len());
	}
}

fn get_invalid_test_cases() -> Vec<(u32, Rc<DecoderTestInfo>)> {
	let mut result: Vec<(u32, Rc<DecoderTestInfo>)> = Vec::new();
	for tc in encoder_tests(false, false) {
		let tc = Rc::new(tc);
		if code32_only().contains(&tc.code()) {
			result.push((64, tc.clone()));
		}
		if code64_only().contains(&tc.code()) {
			result.push((16, tc.clone()));
			result.push((32, tc.clone()));
		}
	}
	result
}

#[test]
fn encode_invalid() {
	for i in get_invalid_test_cases() {
		encode_invalid_test(i.0, i.1);
	}
}

fn encode_invalid_test(invalid_bitness: u32, tc: Rc<DecoderTestInfo>) {
	let orig_bytes = to_vec_u8(tc.hex_bytes()).unwrap();
	let mut decoder = create_decoder(tc.bitness(), orig_bytes.as_slice(), tc.ip(), tc.decoder_options()).0;
	let orig_rip = decoder.ip();
	let orig_instr = decoder.decode();
	assert_eq!(orig_instr.code(), tc.code());
	assert_eq!(orig_instr.len(), orig_bytes.len());
	assert!(orig_instr.len() <= IcedConstants::MAX_INSTRUCTION_LENGTH);
	assert_eq!(orig_instr.ip16(), orig_rip as u16);
	assert_eq!(orig_instr.ip32(), orig_rip as u32);
	assert_eq!(orig_instr.ip(), orig_rip);
	let after_rip = decoder.ip();
	assert_eq!(orig_instr.next_ip16(), after_rip as u16);
	assert_eq!(orig_instr.next_ip32(), after_rip as u32);
	assert_eq!(orig_instr.next_ip(), after_rip);

	let mut encoder = Encoder::new(invalid_bitness);
	match encoder.encode(&orig_instr, orig_rip) {
		Ok(_) => unreachable!(),
		Err(err) => {
			let expected_err = if invalid_bitness == 64 { Encoder::ERROR_ONLY_1632_BIT_MODE } else { Encoder::ERROR_ONLY_64_BIT_MODE };
			assert_eq!(format!("{}", err), expected_err);
		}
	}
}

#[test]
fn encode_with_error() {
	// xchg ah,[rdx+rsi+16h]
	let bytes = b"\x86\x64\x32\x16";
	let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	let mut instr = decoder.decode();

	let mut encoder = Encoder::new(decoder.bitness());
	assert!(encoder.encode(&instr, instr.ip()).is_ok());
	instr.set_op1_register(Register::CR0);
	assert!(encoder.encode(&instr, instr.ip()).is_err());
	instr.set_op1_register(Register::AL);
	assert!(encoder.encode(&instr, instr.ip()).is_ok());
}

#[test]
#[should_panic]
fn new_panics_if_bitness_0() {
	let _ = Encoder::new(0);
}

#[test]
#[should_panic]
fn new_panics_if_bitness_128() {
	let _ = Encoder::new(128);
}

#[test]
fn try_new_fails_if_bitness_0() {
	assert!(Encoder::try_new(0).is_err());
}

#[test]
fn try_new_fails_if_bitness_128() {
	assert!(Encoder::try_new(128).is_err());
}

#[test]
fn with_capacity_fails_if_bitness_0() {
	assert!(Encoder::try_with_capacity(0, 1).is_err());
}

#[test]
fn with_capacity_failss_if_bitness_128() {
	assert!(Encoder::try_with_capacity(128, 1).is_err());
}

#[test]
fn try_with_capacity_works() {
	let mut encoder = Encoder::try_with_capacity(64, 211).unwrap();
	let buffer = encoder.take_buffer();
	assert!(buffer.is_empty());
	assert_eq!(buffer.capacity(), 211);
}

#[test]
fn set_buffer_works() {
	let mut encoder = Encoder::new(64);
	encoder.set_buffer(vec![10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0]);
	assert_eq!(encoder.take_buffer(), vec![10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0]);
}

#[test]
fn encode_invalid_code_value_is_an_error() {
	let mut instr = Instruction::default();
	instr.set_code(Code::INVALID);
	let instr = instr;

	for &bitness in &[16, 32, 64] {
		let mut encoder = Encoder::new(bitness);
		match encoder.encode(&instr, 0) {
			Ok(_) => unreachable!(),
			Err(err) => assert_eq!(InvalidHandler::ERROR_MESSAGE, format!("{}", err)),
		}
	}
}

#[test]
#[allow(unexpected_cfgs)]
fn displsize_eq_1_uses_long_form_if_it_does_not_fit_in_1_byte() {
	const RIP: u64 = 0;

	let memory16 = MemoryOperand::with_base_displ_size(Register::SI, 0x1234, 1);
	let memory32 = MemoryOperand::with_base_displ_size(Register::ESI, 0x1234_5678, 1);
	let memory64 = MemoryOperand::with_base_displ_size(Register::R14, 0x1234_5678, 1);

	#[allow(unused_mut)]
	let mut tests: Vec<(u32, &'static str, u64, Instruction)> = vec![
		(16, "0F10 8C 3412", RIP, Instruction::with2(Code::Movups_xmm_xmmm128, Register::XMM1, memory16).unwrap()),
		(32, "0F10 8E 78563412", RIP, Instruction::with2(Code::Movups_xmm_xmmm128, Register::XMM1, memory32).unwrap()),
		(64, "41 0F10 8E 78563412", RIP, Instruction::with2(Code::Movups_xmm_xmmm128, Register::XMM1, memory64).unwrap()),
	];

	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
	#[cfg(not(feature = "no_vex"))]
	{
		tests.push((16, "C5F8 10 8C 3412", RIP, Instruction::with2(Code::VEX_Vmovups_xmm_xmmm128, Register::XMM1, memory16).unwrap()));
		tests.push((32, "C5F8 10 8E 78563412", RIP, Instruction::with2(Code::VEX_Vmovups_xmm_xmmm128, Register::XMM1, memory32).unwrap()));
		tests.push((64, "C4C178 10 8E 78563412", RIP, Instruction::with2(Code::VEX_Vmovups_xmm_xmmm128, Register::XMM1, memory64).unwrap()));
	}

	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
	#[cfg(not(feature = "no_evex"))]
	{
		tests.push((16, "62 F17C08 10 8C 3412", RIP, Instruction::with2(Code::EVEX_Vmovups_xmm_k1z_xmmm128, Register::XMM1, memory16).unwrap()));
		tests.push((32, "62 F17C08 10 8E 78563412", RIP, Instruction::with2(Code::EVEX_Vmovups_xmm_k1z_xmmm128, Register::XMM1, memory32).unwrap()));
		tests.push((64, "62 D17C08 10 8E 78563412", RIP, Instruction::with2(Code::EVEX_Vmovups_xmm_k1z_xmmm128, Register::XMM1, memory64).unwrap()));
	}

	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
	#[cfg(not(feature = "no_xop"))]
	{
		tests.push((16, "8F E878C0 8C 3412 A5", RIP, Instruction::with3(Code::XOP_Vprotb_xmm_xmmm128_imm8, Register::XMM1, memory16, 0xA5).unwrap()));
		tests.push((32, "8F E878C0 8E 78563412 A5", RIP, Instruction::with3(Code::XOP_Vprotb_xmm_xmmm128_imm8, Register::XMM1, memory32, 0xA5).unwrap()));
		tests.push((64, "8F C878C0 8E 78563412 A5", RIP, Instruction::with3(Code::XOP_Vprotb_xmm_xmmm128_imm8, Register::XMM1, memory64, 0xA5).unwrap()));
	}

	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
	#[cfg(not(feature = "no_d3now"))]
	{
		tests.push((16, "0F0F 8C 3412 0C", RIP, Instruction::with2(Code::D3NOW_Pi2fw_mm_mmm64, Register::MM1, memory16).unwrap()));
		tests.push((32, "0F0F 8E 78563412 0C", RIP, Instruction::with2(Code::D3NOW_Pi2fw_mm_mmm64, Register::MM1, memory32).unwrap()));
		tests.push((64, "0F0F 8E 78563412 0C", RIP, Instruction::with2(Code::D3NOW_Pi2fw_mm_mmm64, Register::MM1, memory64).unwrap()));
	}

	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
	#[cfg(feature = "mvex")]
	{
		tests.push((64, "62 D17808 28 8E 78563412", RIP, Instruction::with2(Code::MVEX_Vmovaps_zmm_k1_zmmmt, Register::ZMM1, memory64).unwrap()));
	}

	// If it fails, add more tests above (16-bit, 32-bit, and 64-bit test cases)
	const _: () = assert!(IcedConstants::ENCODING_KIND_ENUM_COUNT == 6);

	for &(bitness, hex_bytes, rip, instruction) in &tests {
		let expected_bytes = to_vec_u8(hex_bytes).unwrap();
		let mut encoder = Encoder::new(bitness);
		let encoded_length = encoder.encode(&instruction, rip).unwrap();
		assert_eq!(expected_bytes, encoder.take_buffer());
		assert_eq!(expected_bytes.len(), encoded_length);
	}
}

#[test]
fn encode_bp_with_no_displ() {
	let mut encoder = Encoder::new(16);
	let instr = Instruction::with2(Code::Mov_r16_rm16, Register::AX, MemoryOperand::with_base(Register::BP)).unwrap();
	let len = encoder.encode(&instr, 0).unwrap();
	let expected = vec![0x8B, 0x46, 0x00];
	let actual = encoder.take_buffer();
	assert_eq!(len, actual.len());
	assert_eq!(actual, expected);
}

#[test]
fn encode_ebp_with_no_displ() {
	let mut encoder = Encoder::new(32);
	let instr = Instruction::with2(Code::Mov_r32_rm32, Register::EAX, MemoryOperand::with_base(Register::EBP)).unwrap();
	let len = encoder.encode(&instr, 0).unwrap();
	let expected = vec![0x8B, 0x45, 0x00];
	let actual = encoder.take_buffer();
	assert_eq!(len, actual.len());
	assert_eq!(actual, expected);
}

#[test]
fn encode_ebp_edx_with_no_displ() {
	let mut encoder = Encoder::new(32);
	let instr = Instruction::with2(Code::Mov_r32_rm32, Register::EAX, MemoryOperand::with_base_index(Register::EBP, Register::EDX)).unwrap();
	let len = encoder.encode(&instr, 0).unwrap();
	let expected = vec![0x8B, 0x44, 0x15, 0x00];
	let actual = encoder.take_buffer();
	assert_eq!(len, actual.len());
	assert_eq!(actual, expected);
}

#[test]
fn encode_r13d_with_no_displ() {
	let mut encoder = Encoder::new(64);
	let instr = Instruction::with2(Code::Mov_r32_rm32, Register::EAX, MemoryOperand::with_base(Register::R13D)).unwrap();
	let len = encoder.encode(&instr, 0).unwrap();
	let expected = vec![0x67, 0x41, 0x8B, 0x45, 0x00];
	let actual = encoder.take_buffer();
	assert_eq!(len, actual.len());
	assert_eq!(actual, expected);
}

#[test]
fn encode_r13d_edx_with_no_displ() {
	let mut encoder = Encoder::new(64);
	let instr = Instruction::with2(Code::Mov_r32_rm32, Register::EAX, MemoryOperand::with_base_index(Register::R13D, Register::EDX)).unwrap();
	let len = encoder.encode(&instr, 0).unwrap();
	let expected = vec![0x67, 0x41, 0x8B, 0x44, 0x15, 0x00];
	let actual = encoder.take_buffer();
	assert_eq!(len, actual.len());
	assert_eq!(actual, expected);
}

#[test]
fn encode_rbp_with_no_displ() {
	let mut encoder = Encoder::new(64);
	let instr = Instruction::with2(Code::Mov_r64_rm64, Register::RAX, MemoryOperand::with_base(Register::RBP)).unwrap();
	let len = encoder.encode(&instr, 0).unwrap();
	let expected = vec![0x48, 0x8B, 0x45, 0x00];
	let actual = encoder.take_buffer();
	assert_eq!(len, actual.len());
	assert_eq!(actual, expected);
}

#[test]
fn encode_rbp_rdx_with_no_displ() {
	let mut encoder = Encoder::new(64);
	let instr = Instruction::with2(Code::Mov_r64_rm64, Register::RAX, MemoryOperand::with_base_index(Register::RBP, Register::RDX)).unwrap();
	let len = encoder.encode(&instr, 0).unwrap();
	let expected = vec![0x48, 0x8B, 0x44, 0x15, 0x00];
	let actual = encoder.take_buffer();
	assert_eq!(len, actual.len());
	assert_eq!(actual, expected);
}

#[test]
fn encode_r13_with_no_displ() {
	let mut encoder = Encoder::new(64);
	let instr = Instruction::with2(Code::Mov_r64_rm64, Register::RAX, MemoryOperand::with_base(Register::R13)).unwrap();
	let len = encoder.encode(&instr, 0).unwrap();
	let expected = vec![0x49, 0x8B, 0x45, 0x00];
	let actual = encoder.take_buffer();
	assert_eq!(len, actual.len());
	assert_eq!(actual, expected);
}

#[test]
fn encode_r13_rdx_with_no_displ() {
	let mut encoder = Encoder::new(64);
	let instr = Instruction::with2(Code::Mov_r64_rm64, Register::RAX, MemoryOperand::with_base_index(Register::R13, Register::RDX)).unwrap();
	let len = encoder.encode(&instr, 0).unwrap();
	let expected = vec![0x49, 0x8B, 0x44, 0x15, 0x00];
	let actual = encoder.take_buffer();
	assert_eq!(len, actual.len());
	assert_eq!(actual, expected);
}

#[test]
fn verify_encoder_options() {
	for &bitness in &[16, 32, 64] {
		let encoder = Encoder::new(bitness);
		assert!(!encoder.prevent_vex2());
		assert_eq!(encoder.vex_wig(), 0);
		assert_eq!(encoder.vex_lig(), 0);
		assert_eq!(encoder.evex_wig(), 0);
		assert_eq!(encoder.evex_lig(), 0);
		#[cfg(feature = "mvex")]
		assert_eq!(encoder.mvex_wig(), 0);
	}
}

#[test]
fn get_set_wig_lig_options() {
	for &bitness in &[16, 32, 64] {
		let mut encoder = Encoder::new(bitness);

		encoder.set_vex_lig(1);
		encoder.set_vex_wig(0);
		assert_eq!(encoder.vex_wig(), 0);
		assert_eq!(encoder.vex_lig(), 1);
		encoder.set_vex_wig(1);
		assert_eq!(encoder.vex_wig(), 1);
		assert_eq!(encoder.vex_lig(), 1);

		encoder.set_vex_wig(0xFFFF_FFFE);
		assert_eq!(encoder.vex_wig(), 0);
		assert_eq!(encoder.vex_lig(), 1);
		encoder.set_vex_wig(0xFFFF_FFFF);
		assert_eq!(encoder.vex_wig(), 1);
		assert_eq!(encoder.vex_lig(), 1);

		encoder.set_vex_wig(1);
		encoder.set_vex_lig(0);
		assert_eq!(encoder.vex_lig(), 0);
		assert_eq!(encoder.vex_wig(), 1);
		encoder.set_vex_lig(1);
		assert_eq!(encoder.vex_lig(), 1);
		assert_eq!(encoder.vex_wig(), 1);

		encoder.set_vex_lig(0xFFFF_FFFE);
		assert_eq!(encoder.vex_lig(), 0);
		assert_eq!(encoder.vex_wig(), 1);
		encoder.set_vex_lig(0xFFFF_FFFF);
		assert_eq!(encoder.vex_lig(), 1);
		assert_eq!(encoder.vex_wig(), 1);

		encoder.set_evex_lig(3);
		encoder.set_evex_wig(0);
		assert_eq!(encoder.evex_wig(), 0);
		assert_eq!(encoder.evex_lig(), 3);
		encoder.set_evex_wig(1);
		assert_eq!(encoder.evex_wig(), 1);
		assert_eq!(encoder.evex_lig(), 3);

		encoder.set_evex_wig(0xFFFF_FFFE);
		assert_eq!(encoder.evex_wig(), 0);
		assert_eq!(encoder.evex_lig(), 3);
		encoder.set_evex_wig(0xFFFF_FFFF);
		assert_eq!(encoder.evex_wig(), 1);
		assert_eq!(encoder.evex_lig(), 3);

		encoder.set_evex_wig(1);
		encoder.set_evex_lig(0);
		assert_eq!(encoder.evex_lig(), 0);
		assert_eq!(encoder.evex_wig(), 1);
		encoder.set_evex_lig(1);
		assert_eq!(encoder.evex_lig(), 1);
		assert_eq!(encoder.evex_wig(), 1);
		encoder.set_evex_lig(2);
		assert_eq!(encoder.evex_lig(), 2);
		assert_eq!(encoder.evex_wig(), 1);
		encoder.set_evex_lig(3);
		assert_eq!(encoder.evex_lig(), 3);
		assert_eq!(encoder.evex_wig(), 1);

		encoder.set_evex_lig(0xFFFF_FFFC);
		assert_eq!(encoder.evex_lig(), 0);
		assert_eq!(encoder.evex_wig(), 1);
		encoder.set_evex_lig(0xFFFF_FFFD);
		assert_eq!(encoder.evex_lig(), 1);
		assert_eq!(encoder.evex_wig(), 1);
		encoder.set_evex_lig(0xFFFF_FFFE);
		assert_eq!(encoder.evex_lig(), 2);
		assert_eq!(encoder.evex_wig(), 1);
		encoder.set_evex_lig(0xFFFF_FFFF);
		assert_eq!(encoder.evex_lig(), 3);
		assert_eq!(encoder.evex_wig(), 1);

		#[cfg(feature = "mvex")]
		{
			encoder.set_mvex_wig(0);
			assert_eq!(encoder.mvex_wig(), 0);
			encoder.set_mvex_wig(1);
			assert_eq!(encoder.mvex_wig(), 1);

			encoder.set_mvex_wig(0xFFFF_FFFE);
			assert_eq!(encoder.mvex_wig(), 0);
			encoder.set_mvex_wig(0xFFFF_FFFF);
			assert_eq!(encoder.mvex_wig(), 1);
		}
	}
}

#[test]
#[cfg(not(feature = "no_vex"))]
fn prevent_vex2_encoding() {
	#[rustfmt::skip]
	let tests = [
		("C5FC 10 10", "C4E17C 10 10", Code::VEX_Vmovups_ymm_ymmm256, true),
		("C5FC 10 10", "C5FC 10 10", Code::VEX_Vmovups_ymm_ymmm256, false),
	];
	for tc in &tests {
		let (hex_bytes, expected_bytes, code, prevent_vex2) = *tc;
		let hex_bytes = to_vec_u8(hex_bytes).unwrap();
		const BITNESS: u32 = 64;
		let mut decoder = create_decoder(BITNESS, &hex_bytes, get_default_ip(BITNESS), 0).0;
		let instr = decoder.decode();
		assert_eq!(instr.code(), code);
		let mut encoder = Encoder::new(decoder.bitness());
		encoder.set_prevent_vex2(prevent_vex2);
		let _ = encoder.encode(&instr, instr.ip()).unwrap();
		let encoded_bytes = encoder.take_buffer();
		let expected_bytes = to_vec_u8(expected_bytes).unwrap();
		assert_eq!(encoded_bytes, expected_bytes);
	}
}

#[test]
#[cfg(not(feature = "no_vex"))]
fn test_vex_wig_lig() {
	#[rustfmt::skip]
	let tests = [
		("C5CA 10 CD", "C5CA 10 CD", Code::VEX_Vmovss_xmm_xmm_xmm, 0, 0),
		("C5CA 10 CD", "C5CE 10 CD", Code::VEX_Vmovss_xmm_xmm_xmm, 0, 1),
		("C5CA 10 CD", "C5CA 10 CD", Code::VEX_Vmovss_xmm_xmm_xmm, 1, 0),
		("C5CA 10 CD", "C5CE 10 CD", Code::VEX_Vmovss_xmm_xmm_xmm, 1, 1),

		("C4414A 10 CD", "C4414A 10 CD", Code::VEX_Vmovss_xmm_xmm_xmm, 0, 0),
		("C4414A 10 CD", "C4414E 10 CD", Code::VEX_Vmovss_xmm_xmm_xmm, 0, 1),
		("C4414A 10 CD", "C441CA 10 CD", Code::VEX_Vmovss_xmm_xmm_xmm, 1, 0),
		("C4414A 10 CD", "C441CE 10 CD", Code::VEX_Vmovss_xmm_xmm_xmm, 1, 1),

		("C5F9 50 D3", "C5F9 50 D3", Code::VEX_Vmovmskpd_r32_xmm, 0, 0),
		("C5F9 50 D3", "C5F9 50 D3", Code::VEX_Vmovmskpd_r32_xmm, 0, 1),
		("C5F9 50 D3", "C5F9 50 D3", Code::VEX_Vmovmskpd_r32_xmm, 1, 0),
		("C5F9 50 D3", "C5F9 50 D3", Code::VEX_Vmovmskpd_r32_xmm, 1, 1),

		("C4C179 50 D3", "C4C179 50 D3", Code::VEX_Vmovmskpd_r32_xmm, 0, 0),
		("C4C179 50 D3", "C4C179 50 D3", Code::VEX_Vmovmskpd_r32_xmm, 0, 1),
		("C4C179 50 D3", "C4C179 50 D3", Code::VEX_Vmovmskpd_r32_xmm, 1, 0),
		("C4C179 50 D3", "C4C179 50 D3", Code::VEX_Vmovmskpd_r32_xmm, 1, 1),
	];
	for tc in &tests {
		let (hex_bytes, expected_bytes, code, wig, lig) = *tc;
		let hex_bytes = to_vec_u8(hex_bytes).unwrap();
		const BITNESS: u32 = 64;
		let mut decoder = create_decoder(BITNESS, &hex_bytes, get_default_ip(BITNESS), 0).0;
		let instr = decoder.decode();
		assert_eq!(instr.code(), code);
		let mut encoder = Encoder::new(decoder.bitness());
		encoder.set_vex_wig(wig);
		encoder.set_vex_lig(lig);
		let _ = encoder.encode(&instr, instr.ip()).unwrap();
		let encoded_bytes = encoder.take_buffer();
		let expected_bytes = to_vec_u8(expected_bytes).unwrap();
		assert_eq!(encoded_bytes, expected_bytes);
	}
}

#[test]
#[cfg(not(feature = "no_evex"))]
fn test_evex_wig_lig() {
	#[rustfmt::skip]
	let tests = [
		("62 F14E08 10 D3", "62 F14E08 10 D3", Code::EVEX_Vmovss_xmm_k1z_xmm_xmm, 0, 0),
		("62 F14E08 10 D3", "62 F14E28 10 D3", Code::EVEX_Vmovss_xmm_k1z_xmm_xmm, 0, 1),
		("62 F14E08 10 D3", "62 F14E48 10 D3", Code::EVEX_Vmovss_xmm_k1z_xmm_xmm, 0, 2),
		("62 F14E08 10 D3", "62 F14E68 10 D3", Code::EVEX_Vmovss_xmm_k1z_xmm_xmm, 0, 3),

		("62 F14E08 10 D3", "62 F14E08 10 D3", Code::EVEX_Vmovss_xmm_k1z_xmm_xmm, 1, 0),
		("62 F14E08 10 D3", "62 F14E28 10 D3", Code::EVEX_Vmovss_xmm_k1z_xmm_xmm, 1, 1),
		("62 F14E08 10 D3", "62 F14E48 10 D3", Code::EVEX_Vmovss_xmm_k1z_xmm_xmm, 1, 2),
		("62 F14E08 10 D3", "62 F14E68 10 D3", Code::EVEX_Vmovss_xmm_k1z_xmm_xmm, 1, 3),

		("62 F14D0B 60 50 01", "62 F14D0B 60 50 01", Code::EVEX_Vpunpcklbw_xmm_k1z_xmm_xmmm128, 0, 0),
		("62 F14D0B 60 50 01", "62 F14D0B 60 50 01", Code::EVEX_Vpunpcklbw_xmm_k1z_xmm_xmmm128, 0, 1),
		("62 F14D0B 60 50 01", "62 F14D0B 60 50 01", Code::EVEX_Vpunpcklbw_xmm_k1z_xmm_xmmm128, 0, 2),
		("62 F14D0B 60 50 01", "62 F14D0B 60 50 01", Code::EVEX_Vpunpcklbw_xmm_k1z_xmm_xmmm128, 0, 3),

		("62 F14D0B 60 50 01", "62 F1CD0B 60 50 01", Code::EVEX_Vpunpcklbw_xmm_k1z_xmm_xmmm128, 1, 0),
		("62 F14D0B 60 50 01", "62 F1CD0B 60 50 01", Code::EVEX_Vpunpcklbw_xmm_k1z_xmm_xmmm128, 1, 1),
		("62 F14D0B 60 50 01", "62 F1CD0B 60 50 01", Code::EVEX_Vpunpcklbw_xmm_k1z_xmm_xmmm128, 1, 2),
		("62 F14D0B 60 50 01", "62 F1CD0B 60 50 01", Code::EVEX_Vpunpcklbw_xmm_k1z_xmm_xmmm128, 1, 3),

		("62 F17C0B 51 50 01", "62 F17C0B 51 50 01", Code::EVEX_Vsqrtps_xmm_k1z_xmmm128b32, 0, 0),
		("62 F17C0B 51 50 01", "62 F17C0B 51 50 01", Code::EVEX_Vsqrtps_xmm_k1z_xmmm128b32, 0, 1),
		("62 F17C0B 51 50 01", "62 F17C0B 51 50 01", Code::EVEX_Vsqrtps_xmm_k1z_xmmm128b32, 0, 2),
		("62 F17C0B 51 50 01", "62 F17C0B 51 50 01", Code::EVEX_Vsqrtps_xmm_k1z_xmmm128b32, 0, 3),

		("62 F17C0B 51 50 01", "62 F17C0B 51 50 01", Code::EVEX_Vsqrtps_xmm_k1z_xmmm128b32, 1, 0),
		("62 F17C0B 51 50 01", "62 F17C0B 51 50 01", Code::EVEX_Vsqrtps_xmm_k1z_xmmm128b32, 1, 1),
		("62 F17C0B 51 50 01", "62 F17C0B 51 50 01", Code::EVEX_Vsqrtps_xmm_k1z_xmmm128b32, 1, 2),
		("62 F17C0B 51 50 01", "62 F17C0B 51 50 01", Code::EVEX_Vsqrtps_xmm_k1z_xmmm128b32, 1, 3),
	];
	for tc in &tests {
		let (hex_bytes, expected_bytes, code, wig, lig) = *tc;
		let hex_bytes = to_vec_u8(hex_bytes).unwrap();
		const BITNESS: u32 = 64;
		let mut decoder = create_decoder(BITNESS, &hex_bytes, get_default_ip(BITNESS), 0).0;
		let instr = decoder.decode();
		assert_eq!(instr.code(), code);
		let mut encoder = Encoder::new(decoder.bitness());
		encoder.set_evex_wig(wig);
		encoder.set_evex_lig(lig);
		let _ = encoder.encode(&instr, instr.ip()).unwrap();
		let encoded_bytes = encoder.take_buffer();
		let expected_bytes = to_vec_u8(expected_bytes).unwrap();
		assert_eq!(encoded_bytes, expected_bytes);
	}
}

#[test]
fn verify_memory_operand_ctors() {
	{
		let op = MemoryOperand::new(Register::RCX, Register::RSI, 4, -0x1234_5678_9ABC_DEF1, 8, true, Register::FS);
		assert_eq!(op.base, Register::RCX);
		assert_eq!(op.index, Register::RSI);
		assert_eq!(op.scale, 4);
		assert_eq!(op.displacement, -0x1234_5678_9ABC_DEF1);
		assert_eq!(op.displ_size, 8);
		assert!(op.is_broadcast);
		assert_eq!(op.segment_prefix, Register::FS);
	}
	{
		let op = MemoryOperand::with_base_index_scale_bcst_seg(Register::RCX, Register::RSI, 4, true, Register::FS);
		assert_eq!(op.base, Register::RCX);
		assert_eq!(op.index, Register::RSI);
		assert_eq!(op.scale, 4);
		assert_eq!(op.displacement, 0);
		assert_eq!(op.displ_size, 0);
		assert!(op.is_broadcast);
		assert_eq!(op.segment_prefix, Register::FS);
	}
	{
		let op = MemoryOperand::with_base_displ_size_bcst_seg(Register::RCX, -0x1234_5678_9ABC_DEF1, 8, true, Register::FS);
		assert_eq!(op.base, Register::RCX);
		assert_eq!(op.index, Register::None);
		assert_eq!(op.scale, 1);
		assert_eq!(op.displacement, -0x1234_5678_9ABC_DEF1);
		assert_eq!(op.displ_size, 8);
		assert!(op.is_broadcast);
		assert_eq!(op.segment_prefix, Register::FS);
	}
	{
		let op = MemoryOperand::with_index_scale_displ_size_bcst_seg(Register::RSI, 4, -0x1234_5678_9ABC_DEF1, 8, true, Register::FS);
		assert_eq!(op.base, Register::None);
		assert_eq!(op.index, Register::RSI);
		assert_eq!(op.scale, 4);
		assert_eq!(op.displacement, -0x1234_5678_9ABC_DEF1);
		assert_eq!(op.displ_size, 8);
		assert!(op.is_broadcast);
		assert_eq!(op.segment_prefix, Register::FS);
	}
	{
		let op = MemoryOperand::with_base_displ_bcst_seg(Register::RCX, -0x1234_5678_9ABC_DEF1, true, Register::FS);
		assert_eq!(op.base, Register::RCX);
		assert_eq!(op.index, Register::None);
		assert_eq!(op.scale, 1);
		assert_eq!(op.displacement, -0x1234_5678_9ABC_DEF1);
		assert_eq!(op.displ_size, 1);
		assert!(op.is_broadcast);
		assert_eq!(op.segment_prefix, Register::FS);
	}
	{
		let op = MemoryOperand::with_base_index_scale_displ_size(Register::RCX, Register::RSI, 4, -0x1234_5678_9ABC_DEF1, 8);
		assert_eq!(op.base, Register::RCX);
		assert_eq!(op.index, Register::RSI);
		assert_eq!(op.scale, 4);
		assert_eq!(op.displacement, -0x1234_5678_9ABC_DEF1);
		assert_eq!(op.displ_size, 8);
		assert!(!op.is_broadcast);
		assert_eq!(op.segment_prefix, Register::None);
	}
	{
		let op = MemoryOperand::with_base_index_scale(Register::RCX, Register::RSI, 4);
		assert_eq!(op.base, Register::RCX);
		assert_eq!(op.index, Register::RSI);
		assert_eq!(op.scale, 4);
		assert_eq!(op.displacement, 0);
		assert_eq!(op.displ_size, 0);
		assert!(!op.is_broadcast);
		assert_eq!(op.segment_prefix, Register::None);
	}
	{
		let op = MemoryOperand::with_base_index(Register::RCX, Register::RSI);
		assert_eq!(op.base, Register::RCX);
		assert_eq!(op.index, Register::RSI);
		assert_eq!(op.scale, 1);
		assert_eq!(op.displacement, 0);
		assert_eq!(op.displ_size, 0);
		assert!(!op.is_broadcast);
		assert_eq!(op.segment_prefix, Register::None);
	}
	{
		let op = MemoryOperand::with_base_displ_size(Register::RCX, -0x1234_5678_9ABC_DEF1, 8);
		assert_eq!(op.base, Register::RCX);
		assert_eq!(op.index, Register::None);
		assert_eq!(op.scale, 1);
		assert_eq!(op.displacement, -0x1234_5678_9ABC_DEF1);
		assert_eq!(op.displ_size, 8);
		assert!(!op.is_broadcast);
		assert_eq!(op.segment_prefix, Register::None);
	}
	{
		let op = MemoryOperand::with_index_scale_displ_size(Register::RSI, 4, -0x1234_5678_9ABC_DEF1, 8);
		assert_eq!(op.base, Register::None);
		assert_eq!(op.index, Register::RSI);
		assert_eq!(op.scale, 4);
		assert_eq!(op.displacement, -0x1234_5678_9ABC_DEF1);
		assert_eq!(op.displ_size, 8);
		assert!(!op.is_broadcast);
		assert_eq!(op.segment_prefix, Register::None);
	}
	{
		let op = MemoryOperand::with_base_displ(Register::RCX, -0x1234_5678_9ABC_DEF1);
		assert_eq!(op.base, Register::RCX);
		assert_eq!(op.index, Register::None);
		assert_eq!(op.scale, 1);
		assert_eq!(op.displacement, -0x1234_5678_9ABC_DEF1);
		assert_eq!(op.displ_size, 1);
		assert!(!op.is_broadcast);
		assert_eq!(op.segment_prefix, Register::None);
	}
	{
		let op = MemoryOperand::with_base(Register::RCX);
		assert_eq!(op.base, Register::RCX);
		assert_eq!(op.index, Register::None);
		assert_eq!(op.scale, 1);
		assert_eq!(op.displacement, 0);
		assert_eq!(op.displ_size, 0);
		assert!(!op.is_broadcast);
		assert_eq!(op.segment_prefix, Register::None);
	}
	{
		let op = MemoryOperand::with_displ(0x1234_5678_9ABC_DEF1, 8);
		assert_eq!(op.base, Register::None);
		assert_eq!(op.index, Register::None);
		assert_eq!(op.scale, 1);
		assert_eq!(op.displacement, 0x1234_5678_9ABC_DEF1);
		assert_eq!(op.displ_size, 8);
		assert!(!op.is_broadcast);
		assert_eq!(op.segment_prefix, Register::None);
	}
}

#[cfg(feature = "op_code_info")]
lazy_static::lazy_static! {
	static ref OP_CODE_INFO_TEST_CASES: Vec<OpCodeInfoTestCase> = {
		let mut filename = get_encoder_unit_tests_dir();
		filename.push("OpCodeInfos.txt");
		OpCodeInfoTestParser::new(filename.as_path()).into_iter().collect()
	};
}

#[cfg(feature = "op_code_info")]
#[test]
fn test_all_op_code_infos() {
	for tc in &*OP_CODE_INFO_TEST_CASES {
		test_op_code_info(tc);
	}
}

#[cfg(feature = "op_code_info")]
fn test_op_code_info(tc: &OpCodeInfoTestCase) {
	let info = tc.code.op_code();
	assert_eq!(info.code(), tc.code);
	assert_eq!(info.op_code_string(), tc.op_code_string);
	assert_eq!(info.instruction_string(), tc.instruction_string);
	{
		let mut display = String::with_capacity(tc.instruction_string.len());
		write!(display, "{}", info).unwrap();
		assert_eq!(display, tc.instruction_string);
	}
	assert_eq!(info.to_string(), tc.instruction_string);
	assert_eq!(info.mnemonic(), tc.mnemonic);
	assert_eq!(info.encoding(), tc.encoding);
	assert_eq!(info.is_instruction(), tc.is_instruction);
	assert_eq!(info.mode16(), tc.mode16);
	assert_eq!(info.is_available_in_mode(16), tc.mode16);
	assert_eq!(info.mode32(), tc.mode32);
	assert_eq!(info.is_available_in_mode(32), tc.mode32);
	assert_eq!(info.mode64(), tc.mode64);
	assert_eq!(info.is_available_in_mode(64), tc.mode64);
	assert_eq!(info.fwait(), tc.fwait);
	assert_eq!(info.operand_size(), tc.operand_size);
	assert_eq!(info.address_size(), tc.address_size);
	assert_eq!(info.l(), tc.l);
	assert_eq!(info.w(), tc.w);
	assert_eq!(info.is_lig(), tc.is_lig);
	assert_eq!(info.is_wig(), tc.is_wig);
	assert_eq!(info.is_wig32(), tc.is_wig32);
	assert_eq!(info.tuple_type(), tc.tuple_type);
	assert_eq!(info.memory_size(), tc.memory_size);
	assert_eq!(info.broadcast_memory_size(), tc.broadcast_memory_size);
	assert_eq!(info.decoder_option(), tc.decoder_option);
	assert_eq!(info.can_broadcast(), tc.can_broadcast);
	assert_eq!(info.can_use_rounding_control(), tc.can_use_rounding_control);
	assert_eq!(info.can_suppress_all_exceptions(), tc.can_suppress_all_exceptions);
	assert_eq!(info.can_use_op_mask_register(), tc.can_use_op_mask_register);
	assert_eq!(info.require_op_mask_register(), tc.require_op_mask_register);
	if tc.require_op_mask_register {
		assert!(info.can_use_op_mask_register());
		assert!(!info.can_use_zeroing_masking());
	}
	assert_eq!(info.can_use_zeroing_masking(), tc.can_use_zeroing_masking);
	assert_eq!(info.can_use_lock_prefix(), tc.can_use_lock_prefix);
	assert_eq!(info.can_use_xacquire_prefix(), tc.can_use_xacquire_prefix);
	assert_eq!(info.can_use_xrelease_prefix(), tc.can_use_xrelease_prefix);
	assert_eq!(info.can_use_rep_prefix(), tc.can_use_rep_prefix);
	assert_eq!(info.can_use_repne_prefix(), tc.can_use_repne_prefix);
	assert_eq!(info.can_use_bnd_prefix(), tc.can_use_bnd_prefix);
	assert_eq!(info.can_use_hint_taken_prefix(), tc.can_use_hint_taken_prefix);
	assert_eq!(info.can_use_notrack_prefix(), tc.can_use_notrack_prefix);
	assert_eq!(info.ignores_rounding_control(), tc.ignores_rounding_control);
	assert_eq!(info.amd_lock_reg_bit(), tc.amd_lock_reg_bit);
	assert_eq!(info.default_op_size64(), tc.default_op_size64);
	assert_eq!(info.force_op_size64(), tc.force_op_size64);
	assert_eq!(info.intel_force_op_size64(), tc.intel_force_op_size64);
	assert_eq!(info.must_be_cpl0(), tc.cpl0 && !tc.cpl1 && !tc.cpl2 && !tc.cpl3);
	assert_eq!(info.cpl0(), tc.cpl0);
	assert_eq!(info.cpl1(), tc.cpl1);
	assert_eq!(info.cpl2(), tc.cpl2);
	assert_eq!(info.cpl3(), tc.cpl3);
	assert_eq!(info.is_input_output(), tc.is_input_output);
	assert_eq!(info.is_nop(), tc.is_nop);
	assert_eq!(info.is_reserved_nop(), tc.is_reserved_nop);
	assert_eq!(info.is_serializing_intel(), tc.is_serializing_intel);
	assert_eq!(info.is_serializing_amd(), tc.is_serializing_amd);
	assert_eq!(info.may_require_cpl0(), tc.may_require_cpl0);
	assert_eq!(info.is_cet_tracked(), tc.is_cet_tracked);
	assert_eq!(info.is_non_temporal(), tc.is_non_temporal);
	assert_eq!(info.is_fpu_no_wait(), tc.is_fpu_no_wait);
	assert_eq!(info.ignores_mod_bits(), tc.ignores_mod_bits);
	assert_eq!(info.no66(), tc.no66);
	assert_eq!(info.nfx(), tc.nfx);
	assert_eq!(info.requires_unique_reg_nums(), tc.requires_unique_reg_nums);
	assert_eq!(info.requires_unique_dest_reg_num(), tc.requires_unique_dest_reg_num);
	assert_eq!(info.is_privileged(), tc.is_privileged);
	assert_eq!(info.is_save_restore(), tc.is_save_restore);
	assert_eq!(info.is_stack_instruction(), tc.is_stack_instruction);
	assert_eq!(info.ignores_segment(), tc.ignores_segment);
	assert_eq!(info.is_op_mask_read_write(), tc.is_op_mask_read_write);
	assert_eq!(info.real_mode(), tc.real_mode);
	assert_eq!(info.protected_mode(), tc.protected_mode);
	assert_eq!(info.virtual8086_mode(), tc.virtual8086_mode);
	assert_eq!(info.compatibility_mode(), tc.compatibility_mode);
	assert_eq!(info.long_mode(), tc.long_mode);
	assert_eq!(info.use_outside_smm(), tc.use_outside_smm);
	assert_eq!(info.use_in_smm(), tc.use_in_smm);
	assert_eq!(info.use_outside_enclave_sgx(), tc.use_outside_enclave_sgx);
	assert_eq!(info.use_in_enclave_sgx1(), tc.use_in_enclave_sgx1);
	assert_eq!(info.use_in_enclave_sgx2(), tc.use_in_enclave_sgx2);
	assert_eq!(info.use_outside_vmx_op(), tc.use_outside_vmx_op);
	assert_eq!(info.use_in_vmx_root_op(), tc.use_in_vmx_root_op);
	assert_eq!(info.use_in_vmx_non_root_op(), tc.use_in_vmx_non_root_op);
	assert_eq!(info.use_outside_seam(), tc.use_outside_seam);
	assert_eq!(info.use_in_seam(), tc.use_in_seam);
	assert_eq!(info.tdx_non_root_gen_ud(), tc.tdx_non_root_gen_ud);
	assert_eq!(info.tdx_non_root_gen_ve(), tc.tdx_non_root_gen_ve);
	assert_eq!(info.tdx_non_root_may_gen_ex(), tc.tdx_non_root_may_gen_ex);
	assert_eq!(info.intel_vm_exit(), tc.intel_vm_exit);
	assert_eq!(info.intel_may_vm_exit(), tc.intel_may_vm_exit);
	assert_eq!(info.intel_smm_vm_exit(), tc.intel_smm_vm_exit);
	assert_eq!(info.amd_vm_exit(), tc.amd_vm_exit);
	assert_eq!(info.amd_may_vm_exit(), tc.amd_may_vm_exit);
	assert_eq!(info.tsx_abort(), tc.tsx_abort);
	assert_eq!(info.tsx_impl_abort(), tc.tsx_impl_abort);
	assert_eq!(info.tsx_may_abort(), tc.tsx_may_abort);
	assert_eq!(info.intel_decoder16(), tc.intel_decoder16);
	assert_eq!(info.intel_decoder32(), tc.intel_decoder32);
	assert_eq!(info.intel_decoder64(), tc.intel_decoder64);
	assert_eq!(info.amd_decoder16(), tc.amd_decoder16);
	assert_eq!(info.amd_decoder32(), tc.amd_decoder32);
	assert_eq!(info.amd_decoder64(), tc.amd_decoder64);
	assert_eq!(info.table(), tc.table);
	assert_eq!(info.mandatory_prefix(), tc.mandatory_prefix);
	assert_eq!(info.op_code(), tc.op_code);
	assert_eq!(info.op_code_len(), tc.op_code_len);
	assert_eq!(info.is_group(), tc.is_group);
	assert_eq!(info.group_index(), tc.group_index);
	assert_eq!(info.is_rm_group(), tc.is_rm_group);
	assert_eq!(info.rm_group_index(), tc.rm_group_index);
	assert_eq!(info.op_count(), tc.op_count);
	assert_eq!(info.op0_kind(), tc.op_kinds[0]);
	assert_eq!(info.op1_kind(), tc.op_kinds[1]);
	assert_eq!(info.op2_kind(), tc.op_kinds[2]);
	assert_eq!(info.op3_kind(), tc.op_kinds[3]);
	assert_eq!(info.op4_kind(), tc.op_kinds[4]);
	assert_eq!(info.op_kind(0), tc.op_kinds[0]);
	assert_eq!(info.op_kind(1), tc.op_kinds[1]);
	assert_eq!(info.op_kind(2), tc.op_kinds[2]);
	assert_eq!(info.op_kind(3), tc.op_kinds[3]);
	assert_eq!(info.op_kind(4), tc.op_kinds[4]);
	assert_eq!(info.try_op_kind(0).unwrap(), tc.op_kinds[0]);
	assert_eq!(info.try_op_kind(1).unwrap(), tc.op_kinds[1]);
	assert_eq!(info.try_op_kind(2).unwrap(), tc.op_kinds[2]);
	assert_eq!(info.try_op_kind(3).unwrap(), tc.op_kinds[3]);
	assert_eq!(info.try_op_kind(4).unwrap(), tc.op_kinds[4]);
	let op_kinds = info.op_kinds();
	assert_eq!(op_kinds.len(), tc.op_count as usize);
	for (i, &op_kind) in op_kinds.iter().enumerate() {
		assert_eq!(op_kind, info.op_kind(i as u32));
		assert_eq!(op_kind, info.try_op_kind(i as u32).unwrap());
	}
	const _: () = assert!(IcedConstants::MAX_OP_COUNT == 5);
	for i in tc.op_count..IcedConstants::MAX_OP_COUNT as u32 {
		assert_eq!(info.op_kind(i), OpCodeOperandKind::None);
		assert_eq!(info.try_op_kind(i).unwrap(), OpCodeOperandKind::None);
	}
	#[cfg(feature = "mvex")]
	{
		assert_eq!(info.mvex_eh_bit(), tc.mvex.eh_bit);
		assert_eq!(info.mvex_can_use_eviction_hint(), tc.mvex.can_use_eviction_hint);
		assert_eq!(info.mvex_can_use_imm_rounding_control(), tc.mvex.can_use_imm_rounding_control);
		assert_eq!(info.mvex_ignores_op_mask_register(), tc.mvex.ignores_op_mask_register);
		assert_eq!(info.mvex_no_sae_rc(), tc.mvex.no_sae_rc);
		assert_eq!(info.mvex_tuple_type_lut_kind(), tc.mvex.tuple_type_lut_kind);
		assert_eq!(info.mvex_conversion_func(), tc.mvex.conversion_func);
		assert_eq!(info.mvex_valid_conversion_funcs_mask(), tc.mvex.valid_conversion_funcs_mask);
		assert_eq!(info.mvex_valid_swizzle_funcs_mask(), tc.mvex.valid_swizzle_funcs_mask);
	}
}

#[cfg(feature = "op_code_info")]
#[test]
fn op_kind_panics_if_invalid_input() {
	let op_code = Code::Aaa.op_code();
	if cfg!(debug_assertions) {
		assert!(panic::catch_unwind(|| { op_code.op_kind(IcedConstants::MAX_OP_COUNT as u32) }).is_err());
	} else {
		let _ = op_code.op_kind(IcedConstants::MAX_OP_COUNT as u32);
	}
}

#[cfg(feature = "op_code_info")]
#[test]
fn op_kind_fails_if_invalid_input() {
	let op_code = Code::Aaa.op_code();
	assert!(op_code.try_op_kind(IcedConstants::MAX_OP_COUNT as u32).is_err());
}

#[cfg(feature = "op_code_info")]
#[allow(trivial_casts)]
#[test]
fn verify_instruction_op_code_info() {
	for code in Code::values() {
		let mut instr = Instruction::default();
		instr.set_code(code);
		assert_eq!(code.op_code() as *const _ as usize, instr.op_code() as *const _ as usize);
	}
}

#[cfg(feature = "op_code_info")]
#[test]
fn make_sure_all_code_values_are_tested_exactly_once() {
	let mut tested = [false; IcedConstants::CODE_ENUM_COUNT];
	for tc in &*OP_CODE_INFO_TEST_CASES {
		assert!(!tested[tc.code as usize]);
		tested[tc.code as usize] = true;
	}
	let mut s = String::new();
	let code_names = code_names();
	for i in tested.iter().enumerate() {
		if !*i.1 && !is_ignored_code(code_names[i.0]) {
			if !s.is_empty() {
				s.push(',');
			}
			write!(s, "{}", code_names[i.0]).unwrap();
		}
	}
	assert_eq!(s, "");
}

#[cfg(feature = "op_code_info")]
#[test]
fn op_code_info_is_available_in_mode_fails_if_invalid_bitness_0() {
	assert!(!Code::Nopd.op_code().is_available_in_mode(0));
}

#[cfg(feature = "op_code_info")]
#[test]
fn op_code_info_is_available_in_mode_panics_if_invalid_bitness_128() {
	assert!(!Code::Nopd.op_code().is_available_in_mode(128));
}

#[test]
fn write_byte_works() {
	let mut encoder = Encoder::new(64);
	let instr = Instruction::with2(Code::Add_r64_rm64, Register::R8, Register::RBP).unwrap();
	encoder.write_u8(0x90);
	assert_eq!(encoder.encode(&instr, 0x5555_5555).unwrap(), 3);
	encoder.write_u8(0xCC);
	assert_eq!(encoder.take_buffer(), vec![0x90, 0x4C, 0x03, 0xC5, 0xCC]);
}

macro_rules! encode_ok {
	($bitness:expr, $instr:expr) => {
		let mut encoder = Encoder::new($bitness);
		let instr = $instr.unwrap();
		assert!(encoder.encode(&instr, 0x1234).is_ok());
	};
}
macro_rules! encode_err {
	($bitness:expr, $instr:expr) => {
		let mut encoder = Encoder::new($bitness);
		let instr = $instr.unwrap();
		assert!(encoder.encode(&instr, 0x1234).is_err());
	};
}

#[test]
#[rustfmt::skip]
fn invalid_displ_16() {
	const BITNESS: u32 = 16;

	encode_ok!(BITNESS, Instruction::with2(Code::Mov_EAX_moffs32, Register::EAX, MemoryOperand::with_displ(0x0_0000, 2)));
	encode_ok!(BITNESS, Instruction::with2(Code::Mov_EAX_moffs32, Register::EAX, MemoryOperand::with_displ(0x0_FFFF, 2)));
	encode_err!(BITNESS, Instruction::with2(Code::Mov_EAX_moffs32, Register::EAX, MemoryOperand::with_displ(0x1_0000, 2)));
	encode_err!(BITNESS, Instruction::with2(Code::Mov_EAX_moffs32, Register::EAX, MemoryOperand::with_displ(0xFFFF_FFFF_FFFF_FFFF, 2)));

	encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_displ(0x0_0000, 2)));
	encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_displ(0x0_FFFF, 2)));
	encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_displ(0x1_0000, 2)));
	encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_displ(0xFFFF_FFFF_FFFF_FFFF, 2)));

	for &displ_size in &[1, 2] {
		encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::BX, 0, displ_size)));
		encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::BX, -1, displ_size)));
		encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::BX, 1, displ_size)));
		encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::BX, -0x0_8000, displ_size)));
		encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::BX, 0x0_FFFF, displ_size)));
		encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::BX, -0x0_8001, displ_size)));
		encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::BX, 0x1_0000, displ_size)));
		encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::BX, -0x8000_0000_0000_0000, displ_size)));
		encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::BX, 0x7FFF_FFFF_FFFF_FFFF, displ_size)));
	}

	encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::BP, 0, 0)));
	encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::BP, 1, 0)));
	encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::BP, -1, 0)));

	encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::BX, 0, 0)));
	encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::BX, 1, 0)));
	encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::BX, -1, 0)));

	encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::EBP, 0, 0)));
	encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::EBP, 1, 0)));
	encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::EBP, -1, 0)));

	encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::EBX, 0, 0)));
	encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::EBX, 1, 0)));
	encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::EBX, -1, 0)));

	for &displ_size in &[1, 4] {
		encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::EBX, 0, displ_size)));
		encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::EBX, 1, displ_size)));
		encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::EBX, -1, displ_size)));
		encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::EBX, -0x8000_0000, displ_size)));
		encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::EBX, 0xFFFF_FFFF, displ_size)));
		encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::EBX, -0x8000_0001, displ_size)));
		encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::EBX, 0x1_0000_0000, displ_size)));
	}
}

#[test]
#[rustfmt::skip]
fn invalid_displ_32() {
	const BITNESS: u32 = 32;

	encode_ok!(BITNESS, Instruction::with2(Code::Mov_EAX_moffs32, Register::EAX, MemoryOperand::with_displ(0x0_0000, 2)));
	encode_ok!(BITNESS, Instruction::with2(Code::Mov_EAX_moffs32, Register::EAX, MemoryOperand::with_displ(0x0_FFFF, 2)));
	encode_err!(BITNESS, Instruction::with2(Code::Mov_EAX_moffs32, Register::EAX, MemoryOperand::with_displ(0x1_0000, 2)));
	encode_err!(BITNESS, Instruction::with2(Code::Mov_EAX_moffs32, Register::EAX, MemoryOperand::with_displ(0xFFFF_FFFF_FFFF_FFFF, 2)));

	encode_ok!(BITNESS, Instruction::with2(Code::Mov_EAX_moffs32, Register::EAX, MemoryOperand::with_displ(0x0_0000_0000, 4)));
	encode_ok!(BITNESS, Instruction::with2(Code::Mov_EAX_moffs32, Register::EAX, MemoryOperand::with_displ(0x0_FFFF_FFFF, 4)));
	encode_err!(BITNESS, Instruction::with2(Code::Mov_EAX_moffs32, Register::EAX, MemoryOperand::with_displ(0x1_0000_0000, 4)));
	encode_err!(BITNESS, Instruction::with2(Code::Mov_EAX_moffs32, Register::EAX, MemoryOperand::with_displ(0xFFFF_FFFF_FFFF_FFFF, 4)));

	for &displ_size in &[1, 4] {
		encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::None, 0, displ_size)));
		encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::None, 1, displ_size)));
		encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::None, -1, displ_size)));
		encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::None, -0x8000_0000, displ_size)));
		encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::None, 0xFFFF_FFFF, displ_size)));
		encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::None, -0x8000_0001, displ_size)));
		encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::None, 0x1_0000_0000, displ_size)));
	}

	encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::BP, 0, 0)));
	encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::BP, 1, 0)));
	encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::BP, -1, 0)));

	encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::BX, 0, 0)));
	encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::BX, 1, 0)));
	encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::BX, -1, 0)));

	encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::EBP, 0, 0)));
	encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::EBP, 1, 0)));
	encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::EBP, -1, 0)));

	encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::EBX, 0, 0)));
	encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::EBX, 1, 0)));
	encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::EBX, -1, 0)));

	for &displ_size in &[1, 4] {
		encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::EBX, 0, displ_size)));
		encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::EBX, 1, displ_size)));
		encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::EBX, -1, displ_size)));
		encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::EBX, -0x8000_0000, displ_size)));
		encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::EBX, 0xFFFF_FFFF, displ_size)));
		encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::EBX, -0x8000_0001, displ_size)));
		encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::EBX, 0x1_0000_0000, displ_size)));
	}
}

#[test]
#[rustfmt::skip]
fn invalid_displ_64() {
	const BITNESS: u32 = 64;

	encode_ok!(BITNESS, Instruction::with2(Code::Mov_EAX_moffs32, Register::EAX, MemoryOperand::with_displ(0x0_0000_0000, 4)));
	encode_ok!(BITNESS, Instruction::with2(Code::Mov_EAX_moffs32, Register::EAX, MemoryOperand::with_displ(0x0_FFFF_FFFF, 4)));
	encode_err!(BITNESS, Instruction::with2(Code::Mov_EAX_moffs32, Register::EAX, MemoryOperand::with_displ(0x1_0000_0000, 4)));
	encode_err!(BITNESS, Instruction::with2(Code::Mov_EAX_moffs32, Register::EAX, MemoryOperand::with_displ(0xFFFF_FFFF_FFFF_FFFF, 4)));

	encode_ok!(BITNESS, Instruction::with2(Code::Mov_EAX_moffs32, Register::EAX, MemoryOperand::with_displ(0x0000_0000_0000_0000, 8)));
	encode_ok!(BITNESS, Instruction::with2(Code::Mov_EAX_moffs32, Register::EAX, MemoryOperand::with_displ(0xFFFF_FFFF_FFFF_FFFF, 8)));

	for &displ_size in &[1, 8] {
		encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::None, 0, displ_size)));
		encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::None, 1, displ_size)));
		encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::None, -1, displ_size)));
		encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::None, -0x8000_0000, displ_size)));
		encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::None, 0x7FFF_FFFF, displ_size)));
		encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::None, -0x8000_0001, displ_size)));
		encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::None, 0x8000_0000, displ_size)));
	}

	encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::EBP, 0, 0)));
	encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::EBP, 1, 0)));
	encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::EBP, -1, 0)));

	encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::R13D, 0, 0)));
	encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::R13D, 1, 0)));
	encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::R13D, -1, 0)));

	encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::RBP, 0, 0)));
	encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::RBP, 1, 0)));
	encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::RBP, -1, 0)));

	encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::R13, 0, 0)));
	encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::R13, 1, 0)));
	encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::R13, -1, 0)));

	encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::EBX, 0, 0)));
	encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::EBX, 1, 0)));
	encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::EBX, -1, 0)));

	encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::RBX, 0, 0)));
	encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::RBX, 1, 0)));
	encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::RBX, -1, 0)));

	for &displ_size in &[1, 4] {
		encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::EBX, 0, displ_size)));
		encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::EBX, 1, displ_size)));
		encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::EBX, -1, displ_size)));
		encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::EBX, -0x8000_0000, displ_size)));
		encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::EBX, 0xFFFF_FFFF, displ_size)));
		encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::EBX, -0x8000_0001, displ_size)));
		encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::EBX, 0x1_0000_0000, displ_size)));
	}

	for &displ_size in &[1, 8] {
		encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::RBX, 0, displ_size)));
		encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::RBX, 1, displ_size)));
		encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::RBX, -1, displ_size)));
		encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::RBX, -0x8000_0000, displ_size)));
		encode_ok!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::RBX, 0x7FFF_FFFF, displ_size)));
		encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::RBX, -0x8000_0001, displ_size)));
		encode_err!(BITNESS, Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ_size(Register::RBX, 0x8000_0000, displ_size)));
	}
}

#[test]
fn test_unsupported_bitness() {
	{
		let mut encoder = Encoder::new(16);
		assert!(encoder.encode(&Instruction::with2(Code::Mov_r64_rm64, Register::RAX, Register::RCX).unwrap(), 0).is_err());
	}
	{
		let mut encoder = Encoder::new(32);
		assert!(encoder.encode(&Instruction::with2(Code::Mov_r64_rm64, Register::RAX, Register::RCX).unwrap(), 0).is_err());
	}
	{
		let mut encoder = Encoder::new(64);
		assert!(encoder.encode(&Instruction::with(Code::Pushad), 0).is_err());
	}
}

#[test]
fn test_too_long_instruction() {
	let mut encoder = Encoder::new(16);
	let mut instr = Instruction::with2(
		Code::Add_rm32_imm32,
		MemoryOperand::new(Register::ESP, Register::None, 1, 0x1234_5678, 4, false, Register::SS),
		0x1234_5678,
	)
	.unwrap();
	instr.set_has_xacquire_prefix(true);
	instr.set_has_lock_prefix(true);
	assert!(encoder.encode(&instr, 0).is_err());
}

#[test]
fn test_wrong_op_kind() {
	let mut encoder = Encoder::new(64);
	let mut instr = Instruction::with1(Code::Push_r64, Register::RAX).unwrap();
	instr.set_op0_kind(OpKind::Immediate16);
	assert!(encoder.encode(&instr, 0).is_err());
}

#[test]
fn test_wrong_implied_register() {
	let mut encoder = Encoder::new(64);
	let instr = Instruction::with2(Code::In_AL_DX, Register::RAX, Register::EDX).unwrap();
	assert!(encoder.encode(&instr, 0).is_err());
}

#[test]
fn test_wrong_register() {
	let mut encoder = Encoder::new(64);
	let instr = Instruction::with1(Code::Push_r64, Register::EAX).unwrap();
	assert!(encoder.encode(&instr, 0).is_err());
}

#[test]
fn test_invalid_maskmov() {
	let tests = [
		(16, Instruction::with_maskmovq(16, Register::MM0, Register::MM1, Register::None).unwrap(), OpKind::MemorySegRDI),
		(16, Instruction::with_maskmovdqu(16, Register::XMM0, Register::XMM1, Register::None).unwrap(), OpKind::MemorySegRDI),
		(16, Instruction::with_vmaskmovdqu(16, Register::XMM0, Register::XMM1, Register::None).unwrap(), OpKind::MemorySegRDI),
		(32, Instruction::with_maskmovq(32, Register::MM0, Register::MM1, Register::None).unwrap(), OpKind::MemorySegRDI),
		(32, Instruction::with_maskmovdqu(32, Register::XMM0, Register::XMM1, Register::None).unwrap(), OpKind::MemorySegRDI),
		(32, Instruction::with_vmaskmovdqu(32, Register::XMM0, Register::XMM1, Register::None).unwrap(), OpKind::MemorySegRDI),
		(64, Instruction::with_maskmovq(64, Register::MM0, Register::MM1, Register::None).unwrap(), OpKind::MemorySegDI),
		(64, Instruction::with_maskmovdqu(64, Register::XMM0, Register::XMM1, Register::None).unwrap(), OpKind::MemorySegDI),
		(64, Instruction::with_vmaskmovdqu(64, Register::XMM0, Register::XMM1, Register::None).unwrap(), OpKind::MemorySegDI),
	];
	for &(bitness, instr, bad_op_kind) in &tests {
		{
			let mut encoder = Encoder::new(bitness);
			let mut instr = instr;
			instr.set_op0_kind(OpKind::FarBranch16);
			assert!(encoder.encode(&instr, 0).is_err());
		}
		{
			let mut encoder = Encoder::new(bitness);
			let mut instr = instr;
			instr.set_op0_kind(bad_op_kind);
			assert!(encoder.encode(&instr, 0).is_err());
		}
	}
}

#[test]
fn test_invalid_outs() {
	let tests = [
		(16, Instruction::with_outsb(16, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegRSI),
		(16, Instruction::with_outsw(16, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegRSI),
		(16, Instruction::with_outsd(16, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegRSI),
		(32, Instruction::with_outsb(32, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegRSI),
		(32, Instruction::with_outsw(32, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegRSI),
		(32, Instruction::with_outsd(32, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegRSI),
		(64, Instruction::with_outsb(64, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegSI),
		(64, Instruction::with_outsw(64, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegSI),
		(64, Instruction::with_outsd(64, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegSI),
	];
	for &(bitness, instr, bad_op_kind) in &tests {
		{
			let mut encoder = Encoder::new(bitness);
			let mut instr = instr;
			instr.set_op1_kind(OpKind::FarBranch16);
			assert!(encoder.encode(&instr, 0).is_err());
		}
		{
			let mut encoder = Encoder::new(bitness);
			let mut instr = instr;
			instr.set_op1_kind(bad_op_kind);
			assert!(encoder.encode(&instr, 0).is_err());
		}
	}
}

#[test]
fn test_invalid_movs() {
	let tests = [
		(16, Instruction::with_movsb(16, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegRSI, OpKind::MemoryESRDI),
		(16, Instruction::with_movsw(16, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegRSI, OpKind::MemoryESRDI),
		(16, Instruction::with_movsd(16, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegRSI, OpKind::MemoryESRDI),
		(16, Instruction::with_movsq(16, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegRSI, OpKind::MemoryESRDI),
		(32, Instruction::with_movsb(32, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegRSI, OpKind::MemoryESRDI),
		(32, Instruction::with_movsw(32, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegRSI, OpKind::MemoryESRDI),
		(32, Instruction::with_movsd(32, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegRSI, OpKind::MemoryESRDI),
		(32, Instruction::with_movsq(32, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegRSI, OpKind::MemoryESRDI),
		(64, Instruction::with_movsb(64, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegSI, OpKind::MemoryESDI),
		(64, Instruction::with_movsw(64, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegSI, OpKind::MemoryESDI),
		(64, Instruction::with_movsd(64, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegSI, OpKind::MemoryESDI),
		(64, Instruction::with_movsq(64, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegSI, OpKind::MemoryESDI),
	];
	for &(bitness, instr, bad_op_kind1, bad_op_kind0) in &tests {
		{
			let mut encoder = Encoder::new(bitness);
			let mut instr = instr;
			instr.set_op0_kind(OpKind::FarBranch16);
			instr.set_op1_kind(OpKind::FarBranch16);
			assert!(encoder.encode(&instr, 0).is_err());
		}

		{
			let mut encoder = Encoder::new(bitness);
			let mut instr = instr;
			instr.set_op0_kind(bad_op_kind1);
			instr.set_op1_kind(bad_op_kind1);
			assert!(encoder.encode(&instr, 0).is_err());
		}
		{
			let mut encoder = Encoder::new(bitness);
			let mut instr = instr;
			instr.set_op1_kind(bad_op_kind1);
			assert!(encoder.encode(&instr, 0).is_err());
		}

		{
			let mut encoder = Encoder::new(bitness);
			let mut instr = instr;
			instr.set_op0_kind(bad_op_kind0);
			instr.set_op1_kind(bad_op_kind0);
			assert!(encoder.encode(&instr, 0).is_err());
		}
		{
			let mut encoder = Encoder::new(bitness);
			let mut instr = instr;
			instr.set_op0_kind(bad_op_kind0);
			assert!(encoder.encode(&instr, 0).is_err());
		}
	}
}

#[test]
fn test_invalid_cmps() {
	let tests = [
		(16, Instruction::with_cmpsb(16, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegRSI, OpKind::MemoryESRDI),
		(16, Instruction::with_cmpsw(16, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegRSI, OpKind::MemoryESRDI),
		(16, Instruction::with_cmpsd(16, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegRSI, OpKind::MemoryESRDI),
		(16, Instruction::with_cmpsq(16, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegRSI, OpKind::MemoryESRDI),
		(32, Instruction::with_cmpsb(32, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegRSI, OpKind::MemoryESRDI),
		(32, Instruction::with_cmpsw(32, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegRSI, OpKind::MemoryESRDI),
		(32, Instruction::with_cmpsd(32, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegRSI, OpKind::MemoryESRDI),
		(32, Instruction::with_cmpsq(32, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegRSI, OpKind::MemoryESRDI),
		(64, Instruction::with_cmpsb(64, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegSI, OpKind::MemoryESDI),
		(64, Instruction::with_cmpsw(64, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegSI, OpKind::MemoryESDI),
		(64, Instruction::with_cmpsd(64, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegSI, OpKind::MemoryESDI),
		(64, Instruction::with_cmpsq(64, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegSI, OpKind::MemoryESDI),
	];
	for &(bitness, instr, bad_op_kind1, bad_op_kind0) in &tests {
		{
			let mut encoder = Encoder::new(bitness);
			let mut instr = instr;
			instr.set_op0_kind(OpKind::FarBranch16);
			instr.set_op1_kind(OpKind::FarBranch16);
			assert!(encoder.encode(&instr, 0).is_err());
		}

		{
			let mut encoder = Encoder::new(bitness);
			let mut instr = instr;
			instr.set_op0_kind(bad_op_kind1);
			instr.set_op1_kind(bad_op_kind1);
			assert!(encoder.encode(&instr, 0).is_err());
		}
		{
			let mut encoder = Encoder::new(bitness);
			let mut instr = instr;
			instr.set_op1_kind(bad_op_kind1);
			assert!(encoder.encode(&instr, 0).is_err());
		}

		{
			let mut encoder = Encoder::new(bitness);
			let mut instr = instr;
			instr.set_op0_kind(bad_op_kind0);
			instr.set_op1_kind(bad_op_kind0);
			assert!(encoder.encode(&instr, 0).is_err());
		}
		{
			let mut encoder = Encoder::new(bitness);
			let mut instr = instr;
			instr.set_op0_kind(bad_op_kind0);
			assert!(encoder.encode(&instr, 0).is_err());
		}
	}
}

#[test]
fn test_invalid_lods() {
	let tests = [
		(16, Instruction::with_lodsb(16, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegRSI),
		(16, Instruction::with_lodsw(16, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegRSI),
		(16, Instruction::with_lodsd(16, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegRSI),
		(16, Instruction::with_lodsq(16, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegRSI),
		(32, Instruction::with_lodsb(32, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegRSI),
		(32, Instruction::with_lodsw(32, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegRSI),
		(32, Instruction::with_lodsd(32, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegRSI),
		(32, Instruction::with_lodsq(32, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegRSI),
		(64, Instruction::with_lodsb(64, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegSI),
		(64, Instruction::with_lodsw(64, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegSI),
		(64, Instruction::with_lodsd(64, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegSI),
		(64, Instruction::with_lodsq(64, Register::None, RepPrefixKind::None).unwrap(), OpKind::MemorySegSI),
	];
	for &(bitness, instr, bad_op_kind) in &tests {
		{
			let mut encoder = Encoder::new(bitness);
			let mut instr = instr;
			instr.set_op1_kind(OpKind::FarBranch16);
			assert!(encoder.encode(&instr, 0).is_err());
		}
		{
			let mut encoder = Encoder::new(bitness);
			let mut instr = instr;
			instr.set_op1_kind(bad_op_kind);
			assert!(encoder.encode(&instr, 0).is_err());
		}
	}
}

#[test]
fn test_invalid_ins() {
	let tests = [
		(16, Instruction::with_insb(16, RepPrefixKind::None).unwrap(), OpKind::MemoryESRDI),
		(16, Instruction::with_insw(16, RepPrefixKind::None).unwrap(), OpKind::MemoryESRDI),
		(16, Instruction::with_insd(16, RepPrefixKind::None).unwrap(), OpKind::MemoryESRDI),
		(32, Instruction::with_insb(32, RepPrefixKind::None).unwrap(), OpKind::MemoryESRDI),
		(32, Instruction::with_insw(32, RepPrefixKind::None).unwrap(), OpKind::MemoryESRDI),
		(32, Instruction::with_insd(32, RepPrefixKind::None).unwrap(), OpKind::MemoryESRDI),
		(64, Instruction::with_insb(64, RepPrefixKind::None).unwrap(), OpKind::MemoryESDI),
		(64, Instruction::with_insw(64, RepPrefixKind::None).unwrap(), OpKind::MemoryESDI),
		(64, Instruction::with_insd(64, RepPrefixKind::None).unwrap(), OpKind::MemoryESDI),
	];
	for &(bitness, instr, bad_op_kind) in &tests {
		{
			let mut encoder = Encoder::new(bitness);
			let mut instr = instr;
			instr.set_op0_kind(OpKind::FarBranch16);
			assert!(encoder.encode(&instr, 0).is_err());
		}
		{
			let mut encoder = Encoder::new(bitness);
			let mut instr = instr;
			instr.set_op0_kind(bad_op_kind);
			assert!(encoder.encode(&instr, 0).is_err());
		}
	}
}

#[test]
fn test_invalid_stos() {
	let tests = [
		(16, Instruction::with_stosb(16, RepPrefixKind::None).unwrap(), OpKind::MemoryESRDI),
		(16, Instruction::with_stosw(16, RepPrefixKind::None).unwrap(), OpKind::MemoryESRDI),
		(16, Instruction::with_stosd(16, RepPrefixKind::None).unwrap(), OpKind::MemoryESRDI),
		(16, Instruction::with_stosq(16, RepPrefixKind::None).unwrap(), OpKind::MemoryESRDI),
		(32, Instruction::with_stosb(32, RepPrefixKind::None).unwrap(), OpKind::MemoryESRDI),
		(32, Instruction::with_stosw(32, RepPrefixKind::None).unwrap(), OpKind::MemoryESRDI),
		(32, Instruction::with_stosd(32, RepPrefixKind::None).unwrap(), OpKind::MemoryESRDI),
		(32, Instruction::with_stosq(32, RepPrefixKind::None).unwrap(), OpKind::MemoryESRDI),
		(64, Instruction::with_stosb(64, RepPrefixKind::None).unwrap(), OpKind::MemoryESDI),
		(64, Instruction::with_stosw(64, RepPrefixKind::None).unwrap(), OpKind::MemoryESDI),
		(64, Instruction::with_stosd(64, RepPrefixKind::None).unwrap(), OpKind::MemoryESDI),
		(64, Instruction::with_stosq(64, RepPrefixKind::None).unwrap(), OpKind::MemoryESDI),
	];
	for &(bitness, instr, bad_op_kind) in &tests {
		{
			let mut encoder = Encoder::new(bitness);
			let mut instr = instr;
			instr.set_op0_kind(OpKind::FarBranch16);
			assert!(encoder.encode(&instr, 0).is_err());
		}
		{
			let mut encoder = Encoder::new(bitness);
			let mut instr = instr;
			instr.set_op0_kind(bad_op_kind);
			assert!(encoder.encode(&instr, 0).is_err());
		}
	}
}

#[test]
fn test_invalid_scas() {
	let tests = [
		(16, Instruction::with_scasb(16, RepPrefixKind::None).unwrap(), OpKind::MemoryESRDI),
		(16, Instruction::with_scasw(16, RepPrefixKind::None).unwrap(), OpKind::MemoryESRDI),
		(16, Instruction::with_scasd(16, RepPrefixKind::None).unwrap(), OpKind::MemoryESRDI),
		(16, Instruction::with_scasq(16, RepPrefixKind::None).unwrap(), OpKind::MemoryESRDI),
		(32, Instruction::with_scasb(32, RepPrefixKind::None).unwrap(), OpKind::MemoryESRDI),
		(32, Instruction::with_scasw(32, RepPrefixKind::None).unwrap(), OpKind::MemoryESRDI),
		(32, Instruction::with_scasd(32, RepPrefixKind::None).unwrap(), OpKind::MemoryESRDI),
		(32, Instruction::with_scasq(32, RepPrefixKind::None).unwrap(), OpKind::MemoryESRDI),
		(64, Instruction::with_scasb(64, RepPrefixKind::None).unwrap(), OpKind::MemoryESDI),
		(64, Instruction::with_scasw(64, RepPrefixKind::None).unwrap(), OpKind::MemoryESDI),
		(64, Instruction::with_scasd(64, RepPrefixKind::None).unwrap(), OpKind::MemoryESDI),
		(64, Instruction::with_scasq(64, RepPrefixKind::None).unwrap(), OpKind::MemoryESDI),
	];
	for &(bitness, instr, bad_op_kind) in &tests {
		{
			let mut encoder = Encoder::new(bitness);
			let mut instr = instr;
			instr.set_op0_kind(OpKind::FarBranch16);
			assert!(encoder.encode(&instr, 0).is_err());
		}
		{
			let mut encoder = Encoder::new(bitness);
			let mut instr = instr;
			instr.set_op0_kind(bad_op_kind);
			assert!(encoder.encode(&instr, 0).is_err());
		}
	}
}

#[test]
fn test_invalid_xlatb() {
	#[rustfmt::skip]
	let tests = [
		(16, Instruction::with1(Code::Xlat_m8, MemoryOperand::new(Register::BX, Register::AL, 1, 0, 0, false, Register::None)).unwrap(), Register::RBX),
		(32, Instruction::with1(Code::Xlat_m8, MemoryOperand::new(Register::EBX, Register::AL, 1, 0, 0, false, Register::None)).unwrap(), Register::RBX),
		(64, Instruction::with1(Code::Xlat_m8, MemoryOperand::new(Register::RBX, Register::AL, 1, 0, 0, false, Register::None)).unwrap(), Register::BX),
	];
	for &(bitness, instr, invalid_rbx) in &tests {
		{
			let mut encoder = Encoder::new(bitness);
			assert!(encoder.encode(&instr, 0).is_ok());
		}
		{
			let mut encoder = Encoder::new(bitness);
			let mut instr = instr;
			instr.set_memory_base(invalid_rbx);
			assert!(encoder.encode(&instr, 0).is_err());
		}
		{
			let mut encoder = Encoder::new(bitness);
			let mut instr = instr;
			instr.set_memory_base(Register::ESI);
			assert!(encoder.encode(&instr, 0).is_err());
		}
		{
			let mut encoder = Encoder::new(bitness);
			let mut instr = instr;
			instr.set_memory_index(Register::AX);
			assert!(encoder.encode(&instr, 0).is_err());
		}
		{
			let mut encoder = Encoder::new(bitness);
			let mut instr = instr;
			instr.set_memory_index(Register::None);
			assert!(encoder.encode(&instr, 0).is_err());
		}
		for &scale in &[2, 4, 8] {
			let mut encoder = Encoder::new(bitness);
			let mut instr = instr;
			instr.set_memory_index_scale(scale);
			assert!(encoder.encode(&instr, 0).is_err());
		}
		let invalid_displ_size = if bitness == 64 { 4 } else { 8 };
		for &(displ, displ_size) in &[(0, 1), (1, invalid_displ_size), (1, 1)] {
			let mut encoder = Encoder::new(bitness);
			let mut instr = instr;
			instr.set_memory_displacement64(displ);
			instr.set_memory_displ_size(displ_size);
			assert!(encoder.encode(&instr, 0).is_err());
		}
	}
}

#[test]
fn test_invalid_const_imm_op() {
	let mut encoder = Encoder::new(64);
	let instr = Instruction::with2(Code::Rol_rm8_1, Register::AL, 0).unwrap();
	assert!(encoder.encode(&instr, 0).is_err());
}

#[test]
#[cfg(not(feature = "no_vex"))]
fn test_invalid_is5_imm_op() {
	for imm in 0..0x100u32 {
		let mut encoder = Encoder::new(64);
		let instr =
			Instruction::with5(Code::VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm4, Register::XMM0, Register::XMM1, Register::XMM2, Register::XMM3, imm)
				.unwrap();
		if imm <= 0x0F {
			assert!(encoder.encode(&instr, 0).is_ok());
		} else {
			assert!(encoder.encode(&instr, 0).is_err());
		}
	}
}

#[test]
fn test_encode_invalid_instr() {
	let mut encoder = Encoder::new(64);
	let instr = Instruction::default();
	assert_eq!(instr.code(), Code::INVALID);
	assert!(encoder.encode(&instr, 0).is_err());
}

#[test]
fn test_high_r8_reg_with_rex_prefix() {
	for &reg in &[Register::AH, Register::CH, Register::DH, Register::BH] {
		let mut encoder = Encoder::new(64);
		let instr = Instruction::with2(Code::Movzx_r64_rm8, Register::RAX, reg).unwrap();
		assert!(encoder.encode(&instr, 0).is_err());
	}
}

#[test]
#[cfg(not(feature = "no_evex"))]
fn test_evex_invalid_k1() {
	let mut encoder = Encoder::new(64);
	let mut instr = Instruction::with2(Code::EVEX_Vucomiss_xmm_xmmm32_sae, Register::XMM0, Register::XMM1).unwrap();
	instr.set_op_mask(Register::K1);
	assert!(encoder.encode(&instr, 0).is_err());
}

#[test]
#[cfg(not(feature = "no_evex"))]
fn encode_without_required_op_mask_register() {
	let mut encoder = Encoder::new(64);
	let mut instr = Instruction::with2(
		Code::EVEX_Vpgatherdd_xmm_k1_vm32x,
		Register::XMM0,
		MemoryOperand::new(Register::RAX, Register::XMM1, 1, 0x10, 1, false, Register::None),
	)
	.unwrap();
	assert!(encoder.encode(&instr, 0).is_err());
	instr.set_op_mask(Register::K1);
	assert!(encoder.encode(&instr, 0).is_ok());
}

#[test]
#[cfg(not(feature = "no_evex"))]
fn encode_invalid_sae() {
	let mut encoder = Encoder::new(64);
	let mut instr = Instruction::with2(Code::EVEX_Vmovups_xmm_k1z_xmmm128, Register::XMM0, Register::XMM1).unwrap();
	assert!(encoder.encode(&instr, 0).is_ok());
	instr.set_suppress_all_exceptions(true);
	assert!(encoder.encode(&instr, 0).is_err());
}

#[test]
#[cfg(not(feature = "no_evex"))]
fn encode_invalid_er() {
	for rc in RoundingControl::values() {
		let mut encoder = Encoder::new(64);
		let mut instr = Instruction::with2(Code::EVEX_Vmovups_xmm_k1z_xmmm128, Register::XMM0, Register::XMM1).unwrap();
		instr.set_rounding_control(rc);
		if rc == RoundingControl::None {
			assert!(encoder.encode(&instr, 0).is_ok());
		} else {
			assert!(encoder.encode(&instr, 0).is_err());
		}
	}
}

#[test]
#[cfg(not(feature = "no_evex"))]
fn encode_invalid_bcst() {
	{
		let mut encoder = Encoder::new(64);
		let mut instr = Instruction::with2(Code::EVEX_Vmovups_xmm_k1z_xmmm128, Register::XMM0, MemoryOperand::with_base(Register::RAX)).unwrap();
		assert!(encoder.encode(&instr, 0).is_ok());
		instr.set_is_broadcast(true);
		assert!(encoder.encode(&instr, 0).is_err());
	}
	{
		let mut encoder = Encoder::new(64);
		let mut instr =
			Instruction::with3(Code::EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, Register::XMM0, Register::XMM1, MemoryOperand::with_base(Register::RAX))
				.unwrap();
		assert!(encoder.encode(&instr, 0).is_ok());
		instr.set_is_broadcast(true);
		assert!(encoder.encode(&instr, 0).is_ok());
	}
	{
		let mut encoder = Encoder::new(64);
		let mut instr = Instruction::with3(Code::EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, Register::XMM0, Register::XMM1, Register::XMM2).unwrap();
		assert!(encoder.encode(&instr, 0).is_ok());
		instr.set_is_broadcast(true);
		assert!(encoder.encode(&instr, 0).is_err());
	}
}

#[test]
#[cfg(not(feature = "no_evex"))]
fn encode_invalid_zmsk() {
	let mut encoder = Encoder::new(64);
	let mut instr = Instruction::with2(Code::EVEX_Vmovss_m32_k1_xmm, MemoryOperand::with_base(Register::RAX), Register::XMM1).unwrap();
	assert!(encoder.encode(&instr, 0).is_ok());
	instr.set_zeroing_masking(true);
	assert!(encoder.encode(&instr, 0).is_err());
	instr.set_op_mask(Register::K1);
	assert!(encoder.encode(&instr, 0).is_err());
}

#[test]
fn encode_invalid_abs_address() {
	#[rustfmt::skip]
	let tests = [
		(16, 0x1234, 2),
		(16, 0x1234_5678, 4),
		(32, 0x1234, 2),
		(32, 0x1234_5678, 4),
		(64, 0x1234_5678, 4),
		(64, 0x1234_5678_9ABC_DEF0, 8),
	];
	for &(bitness, address, displ_size) in &tests {
		let mem_reg = match displ_size {
			2 => Register::BX,
			4 => Register::EBX,
			8 => Register::RBX,
			_ => unreachable!(),
		};

		let instr = Instruction::with2(Code::Mov_EAX_moffs32, Register::EAX, MemoryOperand::with_displ(address, displ_size)).unwrap();
		{
			let mut encoder = Encoder::new(bitness);
			assert!(encoder.encode(&instr, 0).is_ok());
		}
		{
			let mut encoder = Encoder::new(bitness);
			let mut instr = instr;
			instr.set_memory_base(mem_reg);
			assert!(encoder.encode(&instr, 0).is_err());
		}
		{
			let mut encoder = Encoder::new(bitness);
			let mut instr = instr;
			instr.set_memory_index(mem_reg);
			assert!(encoder.encode(&instr, 0).is_err());
		}
		for &scale in &[2, 4, 8] {
			let mut encoder = Encoder::new(bitness);
			let mut instr = instr;
			instr.set_memory_index_scale(scale);
			assert!(encoder.encode(&instr, 0).is_err());
		}
		{
			let mut encoder = Encoder::new(bitness);
			let mut instr = instr;
			instr.set_op1_kind(OpKind::Immediate8);
			assert!(encoder.encode(&instr, 0).is_err());
		}
	}

	#[rustfmt::skip]
	let tests = [
		(16, 0x1234, 8),
		(32, 0x1234, 8),
		(64, 0x1234, 2),
	];
	for &(bitness, address, displ_size) in &tests {
		let mut encoder = Encoder::new(bitness);
		let instr = Instruction::with2(Code::Mov_EAX_moffs32, Register::EAX, MemoryOperand::with_displ(address, displ_size)).unwrap();
		assert!(encoder.encode(&instr, 0).is_err());
	}
}

#[test]
fn test_reg_op_not_allowed() {
	let mut encoder = Encoder::new(64);
	let instr = Instruction::with2(Code::Lea_r32_m, Register::EAX, MemoryOperand::with_base(Register::RAX)).unwrap();
	assert!(encoder.encode(&instr, 0).is_ok());
	let instr = Instruction::with2(Code::Lea_r32_m, Register::EAX, Register::ECX).unwrap();
	assert!(encoder.encode(&instr, 0).is_err());
}

#[test]
fn test_mem_op_not_allowed() {
	let mut encoder = Encoder::new(64);
	let instr = Instruction::with2(Code::Movhlps_xmm_xmm, Register::XMM0, Register::XMM1).unwrap();
	assert!(encoder.encode(&instr, 0).is_ok());
	let instr = Instruction::with2(Code::Movhlps_xmm_xmm, Register::XMM0, MemoryOperand::with_base(Register::RAX)).unwrap();
	assert!(encoder.encode(&instr, 0).is_err());
}

#[test]
fn test_regmem_op_is_wrong_size() {
	#[rustfmt::skip]
	let tests = [
		(16, Instruction::with2(Code::Enqcmd_r16_m512, Register::AX, MemoryOperand::with_base(Register::BX)).unwrap(), Register::EAX),
		(16, Instruction::with2(Code::Enqcmd_r32_m512, Register::EAX, MemoryOperand::with_base(Register::EAX)).unwrap(), Register::AX),
		(16, Instruction::with2(Code::Enqcmd_r32_m512, Register::EAX, MemoryOperand::with_base(Register::EAX)).unwrap(), Register::RAX),
		(32, Instruction::with2(Code::Enqcmd_r16_m512, Register::AX, MemoryOperand::with_base(Register::BX)).unwrap(), Register::EAX),
		(32, Instruction::with2(Code::Enqcmd_r32_m512, Register::EAX, MemoryOperand::with_base(Register::EAX)).unwrap(), Register::AX),
		(32, Instruction::with2(Code::Enqcmd_r32_m512, Register::EAX, MemoryOperand::with_base(Register::EAX)).unwrap(), Register::RAX),
		(64, Instruction::with2(Code::Enqcmd_r32_m512, Register::EAX, MemoryOperand::with_base(Register::EAX)).unwrap(), Register::RAX),
		(64, Instruction::with2(Code::Enqcmd_r64_m512, Register::RAX, MemoryOperand::with_base(Register::RAX)).unwrap(), Register::EAX),
		(64, Instruction::with2(Code::Enqcmd_r64_m512, Register::RAX, MemoryOperand::with_base(Register::RAX)).unwrap(), Register::AX),
	];
	for &(bitness, instr, invalid_reg) in &tests {
		{
			let mut encoder = Encoder::new(bitness);
			assert!(encoder.encode(&instr, 0).is_ok());
		}
		{
			let mut encoder = Encoder::new(bitness);
			let mut instr = instr;
			instr.set_op0_register(invalid_reg);
			assert!(encoder.encode(&instr, 0).is_err());
		}
	}
}

#[test]
#[cfg(not(feature = "no_evex"))]
fn test_vsib_16bit_addr() {
	for &bitness in &[16, 32, 64] {
		let mut encoder = Encoder::new(bitness);
		let mut instr = Instruction::with2(
			Code::EVEX_Vpgatherdd_xmm_k1_vm32x,
			Register::XMM0,
			MemoryOperand::new(Register::EAX, Register::XMM1, 1, 0x10, 1, false, Register::None),
		)
		.unwrap();
		instr.set_op_mask(Register::K1);
		assert!(encoder.encode(&instr, 0).is_ok());
		instr.set_memory_base(Register::BX);
		instr.set_memory_index(Register::SI);
		assert!(encoder.encode(&instr, 0).is_err());
	}
}

#[test]
fn test_expected_reg_or_mem_op_kind() {
	let mut encoder = Encoder::new(64);
	let mut instr = Instruction::with2(Code::Add_rm8_imm8, Register::AL, 123).unwrap();
	assert!(encoder.encode(&instr, 0).is_ok());
	instr.set_op0_kind(OpKind::Immediate8);
	assert!(encoder.encode(&instr, 0).is_err());
}

#[test]
fn test_16bit_addr_in_64bit_mode() {
	let mut encoder = Encoder::new(64);
	let instr = Instruction::with2(Code::Lea_r32_m, Register::EAX, MemoryOperand::with_base(Register::BX)).unwrap();
	assert!(encoder.encode(&instr, 0).is_err());
}

#[test]
fn test_64bit_addr_in_16_32bit_mode() {
	for &bitness in &[16, 32] {
		let mut encoder = Encoder::new(bitness);
		let instr = Instruction::with2(Code::Lea_r32_m, Register::EAX, MemoryOperand::with_base(Register::RAX)).unwrap();
		assert!(encoder.encode(&instr, 0).is_err());
	}
}

#[test]
fn test_invalid_16bit_mem_regs() {
	let tests = [
		(Register::AX, Register::None),
		(Register::R8W, Register::None),
		(Register::BL, Register::None),
		(Register::None, Register::CX),
		(Register::None, Register::R9W),
		(Register::None, Register::SIL),
		(Register::BX, Register::BP),
		(Register::BP, Register::BX),
	];
	for &(base, index) in &tests {
		let mut encoder = Encoder::new(16);
		let instr = Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_index(base, index)).unwrap();
		assert!(encoder.encode(&instr, 0).is_err());
	}
}

#[test]
fn test_invalid_16bit_displ_size() {
	let mut encoder = Encoder::new(16);
	let mut instr = Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ(Register::BX, 1)).unwrap();
	assert!(encoder.encode(&instr, 0).is_ok());
	instr.set_memory_displ_size(4);
	assert!(encoder.encode(&instr, 0).is_err());
	instr.set_memory_displ_size(8);
	assert!(encoder.encode(&instr, 0).is_err());
}

#[test]
fn test_invalid_32bit_displ_size() {
	let mut encoder = Encoder::new(32);
	let mut instr = Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ(Register::EAX, 1)).unwrap();
	assert!(encoder.encode(&instr, 0).is_ok());
	instr.set_memory_displ_size(2);
	assert!(encoder.encode(&instr, 0).is_err());
	instr.set_memory_displ_size(8);
	assert!(encoder.encode(&instr, 0).is_err());
}

#[test]
fn test_invalid_64bit_displ_size() {
	let mut encoder = Encoder::new(64);
	let mut instr = Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_displ(Register::RAX, 1)).unwrap();
	assert!(encoder.encode(&instr, 0).is_ok());
	instr.set_memory_displ_size(2);
	assert!(encoder.encode(&instr, 0).is_err());
	instr.set_memory_displ_size(4);
	assert!(encoder.encode(&instr, 0).is_err());
}

#[test]
fn test_invalid_ip_rel_memory() {
	for &(ip_reg, invalid_index) in &[(Register::EIP, Register::EDI), (Register::RIP, Register::RDI)] {
		{
			let mut encoder = Encoder::new(64);
			let instr = Instruction::with1(Code::Not_rm8, MemoryOperand::new(ip_reg, Register::None, 1, 0, 8, false, Register::None)).unwrap();
			assert!(encoder.encode(&instr, 0).is_ok());
		}
		for &displ_size in &[0, 1, 4, 8] {
			let mut encoder = Encoder::new(64);
			let instr =
				Instruction::with1(Code::Not_rm8, MemoryOperand::new(ip_reg, Register::None, 1, 0, displ_size, false, Register::None)).unwrap();
			assert!(encoder.encode(&instr, 0).is_ok());
		}
		{
			let mut encoder = Encoder::new(64);
			let instr = Instruction::with1(Code::Not_rm8, MemoryOperand::new(ip_reg, Register::None, 1, 0, 2, false, Register::None)).unwrap();
			assert!(encoder.encode(&instr, 0).is_err());
		}
		{
			let mut encoder = Encoder::new(64);
			let instr = Instruction::with1(Code::Not_rm8, MemoryOperand::new(ip_reg, invalid_index, 1, 0, 8, false, Register::None)).unwrap();
			assert!(encoder.encode(&instr, 0).is_err());
		}
		for &scale in &[2, 4, 8] {
			let mut encoder = Encoder::new(64);
			let instr = Instruction::with1(Code::Not_rm8, MemoryOperand::new(ip_reg, Register::None, scale, 0, 8, false, Register::None)).unwrap();
			assert!(encoder.encode(&instr, 0).is_err());
		}
	}
}

#[test]
fn test_invalid_ip_rel_memory_16_32() {
	for &bitness in &[16, 32] {
		for &(ip_reg, displ_size) in &[(Register::EIP, 4), (Register::RIP, 8)] {
			let mut encoder = Encoder::new(bitness);
			let instr =
				Instruction::with1(Code::Not_rm8, MemoryOperand::new(ip_reg, Register::None, 1, 0, displ_size, false, Register::None)).unwrap();
			assert!(encoder.encode(&instr, 0).is_err());
		}
	}
}

#[test]
#[cfg(not(feature = "no_vex"))]
fn test_invalid_ip_rel_memory_sib_required() {
	{
		let mut encoder = Encoder::new(64);
		let instr = Instruction::with2(
			Code::VEX_Tileloaddt1_tmm_sibmem,
			Register::TMM1,
			MemoryOperand::new(Register::RCX, Register::RDX, 1, 0x1234_5678, 8, false, Register::None),
		)
		.unwrap();
		assert!(encoder.encode(&instr, 0).is_ok());
	}
	{
		let mut encoder = Encoder::new(64);
		let instr = Instruction::with2(
			Code::VEX_Tileloaddt1_tmm_sibmem,
			Register::TMM1,
			MemoryOperand::new(Register::RIP, Register::None, 1, 0x1234_5678, 8, false, Register::None),
		)
		.unwrap();
		assert!(encoder.encode(&instr, 0).is_err());
	}
	{
		let mut encoder = Encoder::new(64);
		let instr = Instruction::with2(
			Code::VEX_Tileloaddt1_tmm_sibmem,
			Register::TMM1,
			MemoryOperand::new(Register::ECX, Register::EDX, 1, 0x1234_5678, 4, false, Register::None),
		)
		.unwrap();
		assert!(encoder.encode(&instr, 0).is_ok());
	}
	{
		let mut encoder = Encoder::new(64);
		let instr = Instruction::with2(
			Code::VEX_Tileloaddt1_tmm_sibmem,
			Register::TMM1,
			MemoryOperand::new(Register::EIP, Register::None, 1, 0x1234_5678, 4, false, Register::None),
		)
		.unwrap();
		assert!(encoder.encode(&instr, 0).is_err());
	}
}

#[test]
fn test_invalid_eip_rel_mem_target_addr() {
	for &target in &[0u64, 0x7FFF_FFFF, 0xFFFF_FFFF] {
		let mut encoder = Encoder::new(64);
		let instr =
			Instruction::with1(Code::Not_rm8, MemoryOperand::new(Register::EIP, Register::None, 1, target as i64, 4, false, Register::None)).unwrap();
		assert!(encoder.encode(&instr, 0).is_ok());
	}
	for &target in &[0x1_0000_0000u64, 0xFFFF_FFFF_FFFF_FFFF] {
		let mut encoder = Encoder::new(64);
		let instr =
			Instruction::with1(Code::Not_rm8, MemoryOperand::new(Register::EIP, Register::None, 1, target as i64, 4, false, Register::None)).unwrap();
		assert!(encoder.encode(&instr, 0).is_err());
	}
}

#[test]
#[cfg(not(feature = "no_evex"))]
fn test_vsib_with_offset_only_mem() {
	let mut encoder = Encoder::new(64);
	let mut instr = Instruction::with2(
		Code::EVEX_Vpgatherdd_xmm_k1_vm32x,
		Register::XMM0,
		MemoryOperand::new(Register::RAX, Register::XMM1, 1, 0x1234_5678, 8, false, Register::None),
	)
	.unwrap();
	instr.set_op_mask(Register::K1);
	assert!(encoder.encode(&instr, 0).is_ok());
	instr.set_memory_base(Register::None);
	instr.set_memory_index(Register::None);
	assert!(encoder.encode(&instr, 0).is_err());
}

#[test]
fn test_invalid_esp_rsp_index_regs() {
	for &sp_reg in &[Register::ESP, Register::RSP] {
		let mut encoder = Encoder::new(64);
		let instr = Instruction::with1(Code::Not_rm8, MemoryOperand::with_base_index_scale(Register::None, sp_reg, 2)).unwrap();
		assert!(encoder.encode(&instr, 0).is_err());
	}
}

#[test]
fn test_rip_rel_dist_too_far_away() {
	const INSTR_LEN: usize = 6;
	const INSTR_ADDR: u64 = 0x1234_5678_9ABC_DEF0;
	for &diff in &[i32::MIN as i64, i32::MAX as i64, -1, 0, 1, -0x1234_5678, 0x1234_5678] {
		let mut encoder = Encoder::new(64);
		let target = ((INSTR_ADDR + INSTR_LEN as u64) as i64).wrapping_add(diff);
		let instr =
			Instruction::with1(Code::Not_rm8, MemoryOperand::new(Register::RIP, Register::None, 1, target, 8, false, Register::None)).unwrap();
		let instr_len = match encoder.encode(&instr, INSTR_ADDR) {
			Ok(len) => len,
			Err(e) => panic!("{:?}", e),
		};
		assert_eq!(instr_len, INSTR_LEN);

		let bytes = encoder.take_buffer();
		let decoded = Decoder::with_ip(64, &bytes, INSTR_ADDR, DecoderOptions::NONE).decode();
		assert_eq!(decoded.code(), Code::Not_rm8);
		assert_eq!(decoded.memory_base(), Register::RIP);
		assert_eq!(decoded.memory_displacement64(), target as u64);
	}
	for &diff in &[i32::MIN as i64 - 1, i32::MAX as i64 + 1, -0x1234_5678_9ABC_DEF0, 0x1234_5678_9ABC_DEF0, i64::MIN, i64::MAX] {
		let mut encoder = Encoder::new(64);
		let target = ((INSTR_ADDR + INSTR_LEN as u64) as i64).wrapping_add(diff);
		let instr =
			Instruction::with1(Code::Not_rm8, MemoryOperand::new(Register::RIP, Register::None, 1, target, 8, false, Register::None)).unwrap();
		assert!(encoder.encode(&instr, INSTR_ADDR).is_err());
	}
}

#[test]
fn test_invalid_jcc_rel8_16() {
	let valid_diffs = &[i8::MIN as i64, i8::MAX as i64, -1, 0, 1, -0x12, 0x12];
	let invalid_diffs = &[i8::MIN as i64 - 1, i8::MAX as i64 + 1, -0x1234, 0x1234, i16::MIN as i64, i16::MAX as i64];
	test_invalid_jcc(16, Code::Je_rel8_16, 0x1234, 2, 0xFFFF, valid_diffs, invalid_diffs);
}

#[test]
fn test_invalid_jcc_rel8_32() {
	let valid_diffs = &[i8::MIN as i64, i8::MAX as i64, -1, 0, 1, -0x12, 0x12];
	let invalid_diffs = &[i8::MIN as i64 - 1, i8::MAX as i64 + 1, -0x1234_5678, 0x1234_5678, i32::MIN as i64, i32::MAX as i64];
	test_invalid_jcc(32, Code::Je_rel8_32, 0x1234_5678, 2, 0xFFFF_FFFF, valid_diffs, invalid_diffs);
}

#[test]
fn test_invalid_jcc_rel8_64() {
	let valid_diffs = &[i8::MIN as i64, i8::MAX as i64, -1, 0, 1, -0x12, 0x12];
	let invalid_diffs = &[i8::MIN as i64 - 1, i8::MAX as i64 + 1, -0x1234_5678_9ABC_DEF0, 0x1234_5678_9ABC_DEF0, i64::MIN, i64::MAX];
	test_invalid_jcc(64, Code::Je_rel8_64, 0x1234_5678_9ABC_DEF0, 2, 0xFFFF_FFFF_FFFF_FFFF, valid_diffs, invalid_diffs);
}

#[test]
fn test_invalid_jcc_rel16_16() {
	let valid_diffs = &[i16::MIN as i64, i16::MAX as i64, -1, 0, 1, -0x1234, 0x1234];
	let invalid_diffs = &[];
	test_invalid_jcc(16, Code::Je_rel16, 0x1234, 4, 0xFFFF, valid_diffs, invalid_diffs);
}

#[test]
fn test_invalid_jcc_rel32_32() {
	let valid_diffs = &[i32::MIN as i64, i32::MAX as i64, -1, 0, 1, -0x1234_5678, 0x1234_5678];
	let invalid_diffs = &[];
	test_invalid_jcc(32, Code::Je_rel32_32, 0x1234_5678, 6, 0xFFFF_FFFF, valid_diffs, invalid_diffs);
}

#[test]
fn test_invalid_jcc_rel32_64() {
	let valid_diffs = &[i32::MIN as i64, i32::MAX as i64, -1, 0, 1, -0x1234_5678, 0x1234_5678];
	let invalid_diffs = &[i32::MIN as i64 - 1, i32::MAX as i64 + 1, -0x1234_5678_9ABC_DEF0, 0x1234_5678_9ABC_DEF0, i64::MIN, i64::MAX];
	test_invalid_jcc(64, Code::Je_rel32_64, 0x1234_5678_9ABC_DEF0, 6, 0xFFFF_FFFF_FFFF_FFFF, valid_diffs, invalid_diffs);
}

fn test_invalid_jcc(bitness: u32, code: Code, instr_addr: u64, instr_len: usize, addr_mask: u64, valid_diffs: &[i64], invalid_diffs: &[i64]) {
	test_invalid_br(bitness, code, instr_addr, instr_len, addr_mask, valid_diffs, invalid_diffs, |code, _, target| {
		Instruction::with_branch(code, target).unwrap()
	})
}

#[test]
fn test_invalid_xbegin_rel16_16() {
	let valid_diffs = &[i16::MIN as i64, i16::MAX as i64, -1, 0, 1, -0x1234, 0x1234];
	let invalid_diffs = &[i16::MIN as i64 - 1, i16::MAX as i64 + 1, -0x1234_5678, 0x1234_5678, i32::MIN as i64, i32::MAX as i64];
	test_invalid_xbegin(16, Code::Xbegin_rel16, 0x1234, 4, 0xFFFF_FFFF, valid_diffs, invalid_diffs);
}

#[test]
fn test_invalid_xbegin_rel32_16() {
	let valid_diffs = &[i32::MIN as i64, i32::MAX as i64, -1, 0, 1, -0x1234_5678, 0x1234_5678];
	let invalid_diffs = &[];
	test_invalid_xbegin(16, Code::Xbegin_rel32, 0x1234, 7, 0xFFFF_FFFF, valid_diffs, invalid_diffs);
}

#[test]
fn test_invalid_xbegin_rel16_32() {
	let valid_diffs = &[i16::MIN as i64, i16::MAX as i64, -1, 0, 1, -0x1234, 0x1234];
	let invalid_diffs = &[i16::MIN as i64 - 1, i16::MAX as i64 + 1, -0x1234_5678, 0x1234_5678, i32::MIN as i64, i32::MAX as i64];
	test_invalid_xbegin(32, Code::Xbegin_rel16, 0x1234_5678, 5, 0xFFFF_FFFF, valid_diffs, invalid_diffs);
}

#[test]
fn test_invalid_xbegin_rel32_32() {
	let valid_diffs = &[i32::MIN as i64, i32::MAX as i64, -1, 0, 1, -0x1234_5678, 0x1234_5678];
	let invalid_diffs = &[];
	test_invalid_xbegin(32, Code::Xbegin_rel32, 0x1234_5678, 6, 0xFFFF_FFFF, valid_diffs, invalid_diffs);
}

#[test]
fn test_invalid_xbegin_rel16_64() {
	let valid_diffs = &[i16::MIN as i64, i16::MAX as i64, -1, 0, 1, -0x1234, 0x1234];
	let invalid_diffs = &[i16::MIN as i64 - 1, i16::MAX as i64 + 1, -0x1234_5678_9ABC_DEF0, 0x1234_5678_9ABC_DEF0, i64::MIN, i64::MAX];
	test_invalid_xbegin(64, Code::Xbegin_rel16, 0x1234_5678_9ABC_DEF0, 5, 0xFFFF_FFFF_FFFF_FFFF, valid_diffs, invalid_diffs);
}

#[test]
fn test_invalid_xbegin_rel32_64() {
	let valid_diffs = &[i32::MIN as i64, i32::MAX as i64, -1, 0, 1, -0x1234_5678, 0x1234_5678];
	let invalid_diffs = &[i32::MIN as i64 - 1, i32::MAX as i64 + 1, -0x1234_5678_9ABC_DEF0, 0x1234_5678_9ABC_DEF0, i64::MIN, i64::MAX];
	test_invalid_xbegin(64, Code::Xbegin_rel32, 0x1234_5678_9ABC_DEF0, 6, 0xFFFF_FFFF_FFFF_FFFF, valid_diffs, invalid_diffs);
}

fn test_invalid_xbegin(bitness: u32, code: Code, instr_addr: u64, instr_len: usize, addr_mask: u64, valid_diffs: &[i64], invalid_diffs: &[i64]) {
	test_invalid_br(bitness, code, instr_addr, instr_len, addr_mask, valid_diffs, invalid_diffs, |code, bitness, target| {
		let mut instr = Instruction::with_xbegin(bitness, target).unwrap();
		instr.set_code(code);
		instr
	})
}

fn test_invalid_br(
	bitness: u32, code: Code, instr_addr: u64, instr_len: usize, addr_mask: u64, valid_diffs: &[i64], invalid_diffs: &[i64],
	create_instr: fn(Code, u32, u64) -> Instruction,
) {
	for &diff in valid_diffs {
		let mut encoder = Encoder::new(bitness);
		let target = (instr_addr + instr_len as u64).wrapping_add(diff as u64) & addr_mask;
		let instr = create_instr(code, bitness, target);
		let decoded_len = match encoder.encode(&instr, instr_addr) {
			Ok(len) => len,
			Err(e) => panic!("{:?}", e),
		};
		assert_eq!(decoded_len, instr_len);

		let bytes = encoder.take_buffer();
		let decoded = Decoder::with_ip(bitness, &bytes, instr_addr, DecoderOptions::NONE).decode();
		assert_eq!(decoded.code(), code);
		assert_eq!(decoded.near_branch64(), target);
	}
	for &diff in invalid_diffs {
		let mut encoder = Encoder::new(bitness);
		let target = (instr_addr + instr_len as u64).wrapping_add(diff as u64) & addr_mask;
		let instr = create_instr(code, bitness, target);
		assert!(encoder.encode(&instr, instr_addr).is_err());
	}
}
