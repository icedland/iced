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

#[cfg(feature = "op_code_info")]
use self::op_code_test_case::*;
#[cfg(feature = "op_code_info")]
use self::op_code_test_case_parser::OpCodeInfoTestParser;
use super::super::decoder::tests::test_utils::*;
use super::super::iced_constants::IcedConstants;
use super::super::test_utils::from_str_conv::to_vec_u8;
#[cfg(feature = "op_code_info")]
use super::super::test_utils::from_str_conv::{code_names, is_ignored_code};
use super::super::test_utils::*;
use super::super::*;
use super::op_code_handler::InvalidHandler;
use alloc::rc::Rc;
use alloc::string::String;
use alloc::vec::Vec;
use core::fmt::Write;

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
			result.push((64, Rc::clone(&tc)));
		}
		if code64_only().contains(&tc.code()) {
			result.push((16, Rc::clone(&tc)));
			result.push((32, Rc::clone(&tc)));
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
#[should_panic]
#[allow(deprecated)]
fn with_capacity_panics_if_bitness_0() {
	let _ = Encoder::with_capacity(0, 1);
}

#[test]
fn with_capacity_fails_if_bitness_0() {
	assert!(Encoder::try_with_capacity(0, 1).is_err());
}

#[test]
#[should_panic]
#[allow(deprecated)]
fn with_capacity_panics_if_bitness_128() {
	let _ = Encoder::with_capacity(128, 1);
}

#[test]
fn with_capacity_failss_if_bitness_128() {
	assert!(Encoder::try_with_capacity(128, 1).is_err());
}

#[test]
#[allow(deprecated)]
fn with_capacity_works() {
	let mut encoder = Encoder::with_capacity(64, 211);
	let buffer = encoder.take_buffer();
	assert!(buffer.is_empty());
	assert_eq!(buffer.capacity(), 211);
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
fn displsize_eq_1_uses_long_form_if_it_does_not_fit_in_1_byte() {
	const RIP: u64 = 0;

	let memory16 = MemoryOperand::with_base_displ_size(Register::SI, 0x1234, 1);
	let memory32 = MemoryOperand::with_base_displ_size(Register::ESI, 0x1234_5678, 1);
	let memory64 = MemoryOperand::with_base_displ_size(Register::R14, 0x1234_5678, 1);

	let mut tests: Vec<(u32, &'static str, u64, Instruction)> = Vec::new();

	tests.push((16, "0F10 8C 3412", RIP, Instruction::with_reg_mem(Code::Movups_xmm_xmmm128, Register::XMM1, memory16)));
	tests.push((32, "0F10 8E 78563412", RIP, Instruction::with_reg_mem(Code::Movups_xmm_xmmm128, Register::XMM1, memory32)));
	tests.push((64, "41 0F10 8E 78563412", RIP, Instruction::with_reg_mem(Code::Movups_xmm_xmmm128, Register::XMM1, memory64)));

	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
	#[cfg(not(feature = "no_vex"))]
	{
		tests.push((16, "C5F8 10 8C 3412", RIP, Instruction::with_reg_mem(Code::VEX_Vmovups_xmm_xmmm128, Register::XMM1, memory16)));
		tests.push((32, "C5F8 10 8E 78563412", RIP, Instruction::with_reg_mem(Code::VEX_Vmovups_xmm_xmmm128, Register::XMM1, memory32)));
		tests.push((64, "C4C178 10 8E 78563412", RIP, Instruction::with_reg_mem(Code::VEX_Vmovups_xmm_xmmm128, Register::XMM1, memory64)));
	}

	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
	#[cfg(not(feature = "no_evex"))]
	{
		tests.push((16, "62 F17C08 10 8C 3412", RIP, Instruction::with_reg_mem(Code::EVEX_Vmovups_xmm_k1z_xmmm128, Register::XMM1, memory16)));
		tests.push((32, "62 F17C08 10 8E 78563412", RIP, Instruction::with_reg_mem(Code::EVEX_Vmovups_xmm_k1z_xmmm128, Register::XMM1, memory32)));
		tests.push((64, "62 D17C08 10 8E 78563412", RIP, Instruction::with_reg_mem(Code::EVEX_Vmovups_xmm_k1z_xmmm128, Register::XMM1, memory64)));
	}

	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
	#[cfg(not(feature = "no_xop"))]
	{
		tests.push((16, "8F E878C0 8C 3412 A5", RIP, Instruction::try_with_reg_mem_u32(Code::XOP_Vprotb_xmm_xmmm128_imm8, Register::XMM1, memory16, 0xA5).unwrap()));
		tests.push((32, "8F E878C0 8E 78563412 A5", RIP, Instruction::try_with_reg_mem_u32(Code::XOP_Vprotb_xmm_xmmm128_imm8, Register::XMM1, memory32, 0xA5).unwrap()));
		tests.push((64, "8F C878C0 8E 78563412 A5", RIP, Instruction::try_with_reg_mem_u32(Code::XOP_Vprotb_xmm_xmmm128_imm8, Register::XMM1, memory64, 0xA5).unwrap()));
	}

	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
	#[cfg(not(feature = "no_d3now"))]
	{
		tests.push((16, "0F0F 8C 3412 0C", RIP, Instruction::with_reg_mem(Code::D3NOW_Pi2fw_mm_mmm64, Register::MM1, memory16)));
		tests.push((32, "0F0F 8E 78563412 0C", RIP, Instruction::with_reg_mem(Code::D3NOW_Pi2fw_mm_mmm64, Register::MM1, memory32)));
		tests.push((64, "0F0F 8E 78563412 0C", RIP, Instruction::with_reg_mem(Code::D3NOW_Pi2fw_mm_mmm64, Register::MM1, memory64)));
	}

	// If it fails, add more tests above (16-bit, 32-bit, and 64-bit test cases)
	const_assert_eq!(IcedConstants::ENCODING_KIND_ENUM_COUNT, 5);

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
	let instr = Instruction::with_reg_mem(Code::Mov_r16_rm16, Register::AX, MemoryOperand::with_base(Register::BP));
	let len = encoder.encode(&instr, 0).unwrap();
	let expected = vec![0x8B, 0x46, 0x00];
	let actual = encoder.take_buffer();
	assert_eq!(len, actual.len());
	assert_eq!(actual, expected);
}

#[test]
fn encode_ebp_with_no_displ() {
	let mut encoder = Encoder::new(32);
	let instr = Instruction::with_reg_mem(Code::Mov_r32_rm32, Register::EAX, MemoryOperand::with_base(Register::EBP));
	let len = encoder.encode(&instr, 0).unwrap();
	let expected = vec![0x8B, 0x45, 0x00];
	let actual = encoder.take_buffer();
	assert_eq!(len, actual.len());
	assert_eq!(actual, expected);
}

#[test]
fn encode_ebp_edx_with_no_displ() {
	let mut encoder = Encoder::new(32);
	let instr = Instruction::with_reg_mem(Code::Mov_r32_rm32, Register::EAX, MemoryOperand::with_base_index(Register::EBP, Register::EDX));
	let len = encoder.encode(&instr, 0).unwrap();
	let expected = vec![0x8B, 0x44, 0x15, 0x00];
	let actual = encoder.take_buffer();
	assert_eq!(len, actual.len());
	assert_eq!(actual, expected);
}

#[test]
fn encode_r13d_with_no_displ() {
	let mut encoder = Encoder::new(64);
	let instr = Instruction::with_reg_mem(Code::Mov_r32_rm32, Register::EAX, MemoryOperand::with_base(Register::R13D));
	let len = encoder.encode(&instr, 0).unwrap();
	let expected = vec![0x67, 0x41, 0x8B, 0x45, 0x00];
	let actual = encoder.take_buffer();
	assert_eq!(len, actual.len());
	assert_eq!(actual, expected);
}

#[test]
fn encode_r13d_edx_with_no_displ() {
	let mut encoder = Encoder::new(64);
	let instr = Instruction::with_reg_mem(Code::Mov_r32_rm32, Register::EAX, MemoryOperand::with_base_index(Register::R13D, Register::EDX));
	let len = encoder.encode(&instr, 0).unwrap();
	let expected = vec![0x67, 0x41, 0x8B, 0x44, 0x15, 0x00];
	let actual = encoder.take_buffer();
	assert_eq!(len, actual.len());
	assert_eq!(actual, expected);
}

#[test]
fn encode_rbp_with_no_displ() {
	let mut encoder = Encoder::new(64);
	let instr = Instruction::with_reg_mem(Code::Mov_r64_rm64, Register::RAX, MemoryOperand::with_base(Register::RBP));
	let len = encoder.encode(&instr, 0).unwrap();
	let expected = vec![0x48, 0x8B, 0x45, 0x00];
	let actual = encoder.take_buffer();
	assert_eq!(len, actual.len());
	assert_eq!(actual, expected);
}

#[test]
fn encode_rbp_rdx_with_no_displ() {
	let mut encoder = Encoder::new(64);
	let instr = Instruction::with_reg_mem(Code::Mov_r64_rm64, Register::RAX, MemoryOperand::with_base_index(Register::RBP, Register::RDX));
	let len = encoder.encode(&instr, 0).unwrap();
	let expected = vec![0x48, 0x8B, 0x44, 0x15, 0x00];
	let actual = encoder.take_buffer();
	assert_eq!(len, actual.len());
	assert_eq!(actual, expected);
}

#[test]
fn encode_r13_with_no_displ() {
	let mut encoder = Encoder::new(64);
	let instr = Instruction::with_reg_mem(Code::Mov_r64_rm64, Register::RAX, MemoryOperand::with_base(Register::R13));
	let len = encoder.encode(&instr, 0).unwrap();
	let expected = vec![0x49, 0x8B, 0x45, 0x00];
	let actual = encoder.take_buffer();
	assert_eq!(len, actual.len());
	assert_eq!(actual, expected);
}

#[test]
fn encode_r13_rdx_with_no_displ() {
	let mut encoder = Encoder::new(64);
	let instr = Instruction::with_reg_mem(Code::Mov_r64_rm64, Register::RAX, MemoryOperand::with_base_index(Register::R13, Register::RDX));
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
}

#[cfg(feature = "op_code_info")]
lazy_static! {
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
	#[allow(deprecated)]
	{
		assert_eq!(info.op_kind(0), tc.op_kinds[0]);
		assert_eq!(info.op_kind(1), tc.op_kinds[1]);
		assert_eq!(info.op_kind(2), tc.op_kinds[2]);
		assert_eq!(info.op_kind(3), tc.op_kinds[3]);
		assert_eq!(info.op_kind(4), tc.op_kinds[4]);
	}
	assert_eq!(info.try_op_kind(0).unwrap(), tc.op_kinds[0]);
	assert_eq!(info.try_op_kind(1).unwrap(), tc.op_kinds[1]);
	assert_eq!(info.try_op_kind(2).unwrap(), tc.op_kinds[2]);
	assert_eq!(info.try_op_kind(3).unwrap(), tc.op_kinds[3]);
	assert_eq!(info.try_op_kind(4).unwrap(), tc.op_kinds[4]);
	let op_kinds = info.op_kinds();
	assert_eq!(op_kinds.len(), tc.op_count as usize);
	for (i, &op_kind) in op_kinds.iter().enumerate() {
		#[allow(deprecated)]
		{
			assert_eq!(op_kind, info.op_kind(i as u32));
		}
		assert_eq!(op_kind, info.try_op_kind(i as u32).unwrap());
	}
	const_assert_eq!(IcedConstants::MAX_OP_COUNT, 5);
	for i in tc.op_count..IcedConstants::MAX_OP_COUNT as u32 {
		#[allow(deprecated)]
		{
			assert_eq!(info.op_kind(i), OpCodeOperandKind::None);
		}
		assert_eq!(info.try_op_kind(i).unwrap(), OpCodeOperandKind::None);
	}
}

#[cfg(feature = "op_code_info")]
#[test]
#[should_panic]
fn op_kind_panics_if_invalid_input() {
	let op_code = Code::Aaa.op_code();
	#[allow(deprecated)]
	{
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
	let instr = Instruction::with_reg_reg(Code::Add_r64_rm64, Register::R8, Register::RBP);
	encoder.write_u8(0x90);
	assert_eq!(encoder.encode(&instr, 0x5555_5555).unwrap(), 3);
	encoder.write_u8(0xCC);
	assert_eq!(encoder.take_buffer(), vec![0x90, 0x4C, 0x03, 0xC5, 0xCC]);
}
