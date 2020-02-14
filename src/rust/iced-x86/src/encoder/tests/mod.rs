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

mod create;
mod dec_enc;
pub(crate) mod non_decoded_tests;
mod op_code_test_case;
mod op_code_test_case_parser;

use self::op_code_test_case::*;
use self::op_code_test_case_parser::OpCodeInfoTestParser;
use super::super::decoder::tests::test_utils::*;
use super::super::iced_constants::IcedConstants;
use super::super::test_utils::from_str_conv::to_vec_u8;
use super::super::test_utils::*;
use super::super::*;
use super::op_code_handler::InvalidHandler;
#[cfg(any(has_alloc, not(feature = "std")))]
use alloc::rc::Rc;
#[cfg(not(feature = "std"))]
use alloc::string::String;
#[cfg(not(feature = "std"))]
use alloc::vec::Vec;
use core::fmt::Write;
use core::mem;
#[cfg(all(not(has_alloc), feature = "std"))]
use std::rc::Rc;

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
	for info in encoder_tests(true, false).iter() {
		if info.bitness() == bitness {
			encode_test(info);
		}
	}
}

fn encode_test(info: &DecoderTestInfo) {
	let orig_bytes = to_vec_u8(info.hex_bytes()).unwrap();
	let mut decoder = create_decoder(info.bitness(), orig_bytes.as_slice(), info.decoder_options()).0;
	let orig_rip = decoder.ip();
	let orig_instr = decoder.decode();
	let orig_co = decoder.get_constant_offsets(&orig_instr);
	assert_eq!(info.code(), orig_instr.code());
	assert_eq!(orig_bytes.len(), orig_instr.len());
	assert!(orig_instr.len() <= IcedConstants::MAX_INSTRUCTION_LENGTH);
	assert_eq!(orig_rip as u16, orig_instr.ip16());
	assert_eq!(orig_rip as u32, orig_instr.ip32());
	assert_eq!(orig_rip, orig_instr.ip());
	let after_rip = decoder.ip();
	assert_eq!(after_rip as u16, orig_instr.next_ip16());
	assert_eq!(after_rip as u32, orig_instr.next_ip32());
	assert_eq!(after_rip, orig_instr.next_ip());

	let mut encoder = Encoder::new(decoder.bitness());
	assert_eq!(info.bitness(), encoder.bitness());
	let orig_instr_copy = orig_instr;
	let encoded_instr_len;
	match encoder.encode(&orig_instr, orig_rip) {
		Ok(len) => encoded_instr_len = len,
		Err(err) => panic!("Unexpected error message: {}", err),
	}
	let mut encoded_co = encoder.get_constant_offsets();
	fix_constant_offsets(&mut encoded_co, orig_instr.len(), encoded_instr_len);
	verify_constant_offsets(&orig_co, &encoded_co);
	let encoded_bytes = encoder.take_buffer();
	assert_eq!(encoded_bytes.len(), encoded_instr_len);
	assert!(orig_instr.eq_all_bits(&orig_instr_copy));

	let expected_bytes = to_vec_u8(info.encoded_hex_bytes()).unwrap();
	if expected_bytes != encoded_bytes {
		assert_eq!(slice_u8_to_string(expected_bytes.as_slice()), slice_u8_to_string(encoded_bytes.as_slice()));
		panic!();
	}

	let mut new_instr = create_decoder(info.bitness(), encoded_bytes.as_slice(), info.decoder_options()).0.decode();
	assert_eq!(info.code(), new_instr.code());
	assert_eq!(encoded_bytes.len(), new_instr.len());
	new_instr.set_len(orig_instr.len());
	new_instr.set_next_ip(orig_instr.next_ip());
	if orig_bytes.len() != expected_bytes.len() && (orig_instr.memory_base() == Register::EIP || orig_instr.memory_base() == Register::RIP) {
		let displ = new_instr.memory_displacement().wrapping_add((expected_bytes.len().wrapping_sub(orig_bytes.len())) as u32);
		new_instr.set_memory_displacement(displ);
	}
	assert!(orig_instr.eq_all_bits(&new_instr));
	// Some tests use useless or extra prefixes, so we can't verify the exact length
	assert!(encoded_bytes.len() <= orig_bytes.len(), "Unexpected encoded prefixes: {}", slice_u8_to_string(encoded_bytes.as_slice()));
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

#[cfg_attr(feature = "cargo-clippy", allow(clippy::trivially_copy_pass_by_ref))]
fn verify_constant_offsets(expected: &ConstantOffsets, actual: &ConstantOffsets) {
	assert_eq!(expected.immediate_offset(), actual.immediate_offset());
	assert_eq!(expected.immediate_size(), actual.immediate_size());
	assert_eq!(expected.immediate_offset2(), actual.immediate_offset2());
	assert_eq!(expected.immediate_size2(), actual.immediate_size2());
	assert_eq!(expected.displacement_offset(), actual.displacement_offset());
	assert_eq!(expected.displacement_size(), actual.displacement_size());
}

fn slice_u8_to_string(bytes: &[u8]) -> String {
	if bytes.is_empty() {
		return String::new();
	}
	let mut s = String::with_capacity(bytes.len() * 3 - 1);
	for b in bytes.iter() {
		if !s.is_empty() {
			s.push_str(" ");
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
		assert_eq!(bitness, encoder.bitness());
		let encoded_instr_len = encoder.encode(&tc.2, RIP).unwrap();
		let encoded_bytes = encoder.take_buffer();
		assert_eq!(expected_bytes, encoded_bytes);
		assert_eq!(encoded_bytes.len(), encoded_instr_len);
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
	let mut decoder = create_decoder(tc.bitness(), orig_bytes.as_slice(), tc.decoder_options()).0;
	let orig_rip = decoder.ip();
	let orig_instr = decoder.decode();
	assert_eq!(tc.code(), orig_instr.code());
	assert_eq!(orig_bytes.len(), orig_instr.len());
	assert!(orig_instr.len() <= IcedConstants::MAX_INSTRUCTION_LENGTH);
	assert_eq!(orig_rip as u16, orig_instr.ip16());
	assert_eq!(orig_rip as u32, orig_instr.ip32());
	assert_eq!(orig_rip, orig_instr.ip());
	let after_rip = decoder.ip();
	assert_eq!(after_rip as u16, orig_instr.next_ip16());
	assert_eq!(after_rip as u32, orig_instr.next_ip32());
	assert_eq!(after_rip, orig_instr.next_ip());

	let mut encoder = Encoder::new(invalid_bitness);
	match encoder.encode(&orig_instr, orig_rip) {
		Ok(_) => unreachable!(),
		Err(err) => {
			let expected_err = if invalid_bitness == 64 { Encoder::ERROR_ONLY_1632_BIT_MODE } else { Encoder::ERROR_ONLY_64_BIT_MODE };
			assert_eq!(expected_err, err);
		}
	}
}

#[test]
fn encode_with_error() {
	// xchg [rdx+rsi+16h],ah
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
#[should_panic]
fn with_capacity_panics_if_bitness_0() {
	let _ = Encoder::with_capacity(0, 1);
}

#[test]
#[should_panic]
fn with_capacity_panics_if_bitness_128() {
	let _ = Encoder::with_capacity(128, 1);
}

#[test]
fn with_capacity_works() {
	let mut encoder = Encoder::with_capacity(64, 211);
	let buffer = encoder.take_buffer();
	assert!(buffer.is_empty());
	assert_eq!(211, buffer.capacity());
}

#[test]
fn set_buffer_works() {
	let mut encoder = Encoder::new(64);
	encoder.set_buffer(vec![10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0]);
	assert_eq!(vec![10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0], encoder.take_buffer());
}

#[test]
fn encode_invalid_code_value_is_an_error() {
	let mut instr = Instruction::default();
	instr.set_code(Code::INVALID);
	let instr = instr;

	for &bitness in [16, 32, 64].iter() {
		let mut encoder = Encoder::new(bitness);
		match encoder.encode(&instr, 0) {
			Ok(_) => unreachable!(),
			Err(err) => assert_eq!(InvalidHandler::ERROR_MESSAGE, err),
		}
	}
}

#[test]
fn displsize_eq_1_uses_long_form_if_it_does_not_fit_in_1_byte() {
	const RIP: u64 = 0;

	let memory16 = MemoryOperand::with_base_displ_size(Register::SI, 0x1234, 1);
	let memory32 = MemoryOperand::with_base_displ_size(Register::ESI, 0x1234_5678, 1);
	let memory64 = MemoryOperand::with_base_displ_size(Register::R14, 0x1234_5678, 1);

	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
	let tests = [
		(16, "0F10 8C 3412", RIP, Instruction::with_reg_mem(Code::Movups_xmm_xmmm128, Register::XMM1, memory16)),
		(16, "C5F8 10 8C 3412", RIP, Instruction::with_reg_mem(Code::VEX_Vmovups_xmm_xmmm128, Register::XMM1, memory16)),
		(16, "62 F17C08 10 8C 3412", RIP, Instruction::with_reg_mem(Code::EVEX_Vmovups_xmm_k1z_xmmm128, Register::XMM1, memory16)),
		(16, "8F E878C0 8C 3412 A5", RIP, Instruction::with_reg_mem_u32(Code::XOP_Vprotb_xmm_xmmm128_imm8, Register::XMM1, memory16, 0xA5)),
		(16, "0F0F 8C 3412 0C", RIP, Instruction::with_reg_mem(Code::D3NOW_Pi2fw_mm_mmm64, Register::MM1, memory16)),

		(32, "0F10 8E 78563412", RIP, Instruction::with_reg_mem(Code::Movups_xmm_xmmm128, Register::XMM1, memory32)),
		(32, "C5F8 10 8E 78563412", RIP, Instruction::with_reg_mem(Code::VEX_Vmovups_xmm_xmmm128, Register::XMM1, memory32)),
		(32, "62 F17C08 10 8E 78563412", RIP, Instruction::with_reg_mem(Code::EVEX_Vmovups_xmm_k1z_xmmm128, Register::XMM1, memory32)),
		(32, "8F E878C0 8E 78563412 A5", RIP, Instruction::with_reg_mem_u32(Code::XOP_Vprotb_xmm_xmmm128_imm8, Register::XMM1, memory32, 0xA5)),
		(32, "0F0F 8E 78563412 0C", RIP, Instruction::with_reg_mem(Code::D3NOW_Pi2fw_mm_mmm64, Register::MM1, memory32)),

		(64, "41 0F10 8E 78563412", RIP, Instruction::with_reg_mem(Code::Movups_xmm_xmmm128, Register::XMM1, memory64)),
		(64, "C4C178 10 8E 78563412", RIP, Instruction::with_reg_mem(Code::VEX_Vmovups_xmm_xmmm128, Register::XMM1, memory64)),
		(64, "62 D17C08 10 8E 78563412", RIP, Instruction::with_reg_mem(Code::EVEX_Vmovups_xmm_k1z_xmmm128, Register::XMM1, memory64)),
		(64, "8F C878C0 8E 78563412 A5", RIP, Instruction::with_reg_mem_u32(Code::XOP_Vprotb_xmm_xmmm128_imm8, Register::XMM1, memory64, 0xA5)),
		(64, "0F0F 8E 78563412 0C", RIP, Instruction::with_reg_mem(Code::D3NOW_Pi2fw_mm_mmm64, Register::MM1, memory64)),
	];

	// If it fails, add more tests above (16-bit, 32-bit, and 64-bit test cases)
	const_assert_eq!(5, IcedConstants::NUMBER_OF_ENCODING_KINDS);

	for &(bitness, hex_bytes, rip, instruction) in tests.iter() {
		let expected_bytes = to_vec_u8(hex_bytes).unwrap();
		let mut encoder = Encoder::new(bitness);
		let encoded_length = encoder.encode(&instruction, rip).unwrap();
		assert_eq!(encoder.take_buffer(), expected_bytes);
		assert_eq!(encoded_length, expected_bytes.len());
	}
}

#[test]
fn encode_bp_with_no_displ() {
	let mut encoder = Encoder::new(16);
	let instr = Instruction::with_reg_mem(Code::Mov_r16_rm16, Register::AX, MemoryOperand::with_base(Register::BP));
	let len = encoder.encode(&instr, 0).unwrap();
	let expected = vec![0x8B, 0x46, 0x00];
	let actual = encoder.take_buffer();
	assert_eq!(actual.len(), len);
	assert_eq!(expected, actual);
}

#[test]
fn encode_ebp_with_no_displ() {
	let mut encoder = Encoder::new(32);
	let instr = Instruction::with_reg_mem(Code::Mov_r32_rm32, Register::EAX, MemoryOperand::with_base(Register::EBP));
	let len = encoder.encode(&instr, 0).unwrap();
	let expected = vec![0x8B, 0x45, 0x00];
	let actual = encoder.take_buffer();
	assert_eq!(actual.len(), len);
	assert_eq!(expected, actual);
}

#[test]
fn encode_r13d_with_no_displ() {
	let mut encoder = Encoder::new(64);
	let instr = Instruction::with_reg_mem(Code::Mov_r32_rm32, Register::EAX, MemoryOperand::with_base(Register::R13D));
	let len = encoder.encode(&instr, 0).unwrap();
	let expected = vec![0x67, 0x41, 0x8B, 0x45, 0x00];
	let actual = encoder.take_buffer();
	assert_eq!(actual.len(), len);
	assert_eq!(expected, actual);
}

#[test]
fn encode_rbp_with_no_displ() {
	let mut encoder = Encoder::new(64);
	let instr = Instruction::with_reg_mem(Code::Mov_r64_rm64, Register::RAX, MemoryOperand::with_base(Register::RBP));
	let len = encoder.encode(&instr, 0).unwrap();
	let expected = vec![0x48, 0x8B, 0x45, 0x00];
	let actual = encoder.take_buffer();
	assert_eq!(actual.len(), len);
	assert_eq!(expected, actual);
}

#[test]
fn encode_r13_with_no_displ() {
	let mut encoder = Encoder::new(64);
	let instr = Instruction::with_reg_mem(Code::Mov_r64_rm64, Register::RAX, MemoryOperand::with_base(Register::R13));
	let len = encoder.encode(&instr, 0).unwrap();
	let expected = vec![0x49, 0x8B, 0x45, 0x00];
	let actual = encoder.take_buffer();
	assert_eq!(actual.len(), len);
	assert_eq!(expected, actual);
}

#[test]
fn verify_encoder_options() {
	for &bitness in [16, 32, 64].iter() {
		let encoder = Encoder::new(bitness);
		assert!(!encoder.prevent_vex2());
		assert_eq!(0, encoder.vex_wig());
		assert_eq!(0, encoder.vex_lig());
		assert_eq!(0, encoder.evex_wig());
		assert_eq!(0, encoder.evex_lig());
	}
}

#[test]
fn get_set_wig_lig_options() {
	for &bitness in [16, 32, 64].iter() {
		let mut encoder = Encoder::new(bitness);

		encoder.set_vex_lig(1);
		encoder.set_vex_wig(0);
		assert_eq!(0, encoder.vex_wig());
		assert_eq!(1, encoder.vex_lig());
		encoder.set_vex_wig(1);
		assert_eq!(1, encoder.vex_wig());
		assert_eq!(1, encoder.vex_lig());

		encoder.set_vex_wig(0xFFFF_FFFE);
		assert_eq!(0, encoder.vex_wig());
		assert_eq!(1, encoder.vex_lig());
		encoder.set_vex_wig(0xFFFF_FFFF);
		assert_eq!(1, encoder.vex_wig());
		assert_eq!(1, encoder.vex_lig());

		encoder.set_vex_wig(1);
		encoder.set_vex_lig(0);
		assert_eq!(0, encoder.vex_lig());
		assert_eq!(1, encoder.vex_wig());
		encoder.set_vex_lig(1);
		assert_eq!(1, encoder.vex_lig());
		assert_eq!(1, encoder.vex_wig());

		encoder.set_vex_lig(0xFFFF_FFFE);
		assert_eq!(0, encoder.vex_lig());
		assert_eq!(1, encoder.vex_wig());
		encoder.set_vex_lig(0xFFFF_FFFF);
		assert_eq!(1, encoder.vex_lig());
		assert_eq!(1, encoder.vex_wig());

		encoder.set_evex_lig(3);
		encoder.set_evex_wig(0);
		assert_eq!(0, encoder.evex_wig());
		assert_eq!(3, encoder.evex_lig());
		encoder.set_evex_wig(1);
		assert_eq!(1, encoder.evex_wig());
		assert_eq!(3, encoder.evex_lig());

		encoder.set_evex_wig(0xFFFF_FFFE);
		assert_eq!(0, encoder.evex_wig());
		assert_eq!(3, encoder.evex_lig());
		encoder.set_evex_wig(0xFFFF_FFFF);
		assert_eq!(1, encoder.evex_wig());
		assert_eq!(3, encoder.evex_lig());

		encoder.set_evex_wig(1);
		encoder.set_evex_lig(0);
		assert_eq!(0, encoder.evex_lig());
		assert_eq!(1, encoder.evex_wig());
		encoder.set_evex_lig(1);
		assert_eq!(1, encoder.evex_lig());
		assert_eq!(1, encoder.evex_wig());
		encoder.set_evex_lig(2);
		assert_eq!(2, encoder.evex_lig());
		assert_eq!(1, encoder.evex_wig());
		encoder.set_evex_lig(3);
		assert_eq!(3, encoder.evex_lig());
		assert_eq!(1, encoder.evex_wig());

		encoder.set_evex_lig(0xFFFF_FFFC);
		assert_eq!(0, encoder.evex_lig());
		assert_eq!(1, encoder.evex_wig());
		encoder.set_evex_lig(0xFFFF_FFFD);
		assert_eq!(1, encoder.evex_lig());
		assert_eq!(1, encoder.evex_wig());
		encoder.set_evex_lig(0xFFFF_FFFE);
		assert_eq!(2, encoder.evex_lig());
		assert_eq!(1, encoder.evex_wig());
		encoder.set_evex_lig(0xFFFF_FFFF);
		assert_eq!(3, encoder.evex_lig());
		assert_eq!(1, encoder.evex_wig());
	}
}

#[test]
fn prevent_vex2_encoding() {
	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
	let tests = [
		("C5FC 10 10", "C4E17C 10 10", Code::VEX_Vmovups_ymm_ymmm256, true),
		("C5FC 10 10", "C5FC 10 10", Code::VEX_Vmovups_ymm_ymmm256, false),
	];
	for tc in tests.iter() {
		let (hex_bytes, expected_bytes, code, prevent_vex2) = *tc;
		let hex_bytes = to_vec_u8(hex_bytes).unwrap();
		let mut decoder = create_decoder(64, &hex_bytes, 0).0;
		let instr = decoder.decode();
		assert_eq!(code, instr.code());
		let mut encoder = Encoder::new(decoder.bitness());
		encoder.set_prevent_vex2(prevent_vex2);
		let _ = encoder.encode(&instr, instr.ip()).unwrap();
		let encoded_bytes = encoder.take_buffer();
		let expected_bytes = to_vec_u8(expected_bytes).unwrap();
		assert_eq!(expected_bytes, encoded_bytes);
	}
}

#[test]
fn test_vex_wig_lig() {
	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
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
	for tc in tests.iter() {
		let (hex_bytes, expected_bytes, code, wig, lig) = *tc;
		let hex_bytes = to_vec_u8(hex_bytes).unwrap();
		let mut decoder = create_decoder(64, &hex_bytes, 0).0;
		let instr = decoder.decode();
		assert_eq!(code, instr.code());
		let mut encoder = Encoder::new(decoder.bitness());
		encoder.set_vex_wig(wig);
		encoder.set_vex_lig(lig);
		let _ = encoder.encode(&instr, instr.ip()).unwrap();
		let encoded_bytes = encoder.take_buffer();
		let expected_bytes = to_vec_u8(expected_bytes).unwrap();
		assert_eq!(expected_bytes, encoded_bytes);
	}
}

#[test]
fn test_evex_wig_lig() {
	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
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
	for tc in tests.iter() {
		let (hex_bytes, expected_bytes, code, wig, lig) = *tc;
		let hex_bytes = to_vec_u8(hex_bytes).unwrap();
		let mut decoder = create_decoder(64, &hex_bytes, 0).0;
		let instr = decoder.decode();
		assert_eq!(code, instr.code());
		let mut encoder = Encoder::new(decoder.bitness());
		encoder.set_evex_wig(wig);
		encoder.set_evex_lig(lig);
		let _ = encoder.encode(&instr, instr.ip()).unwrap();
		let encoded_bytes = encoder.take_buffer();
		let expected_bytes = to_vec_u8(expected_bytes).unwrap();
		assert_eq!(expected_bytes, encoded_bytes);
	}
}

#[test]
fn verify_memory_operand_ctors() {
	{
		let op = MemoryOperand::new(Register::RCX, Register::RSI, 4, 0x1234_5678, 8, true, Register::FS);
		assert_eq!(Register::RCX, op.base);
		assert_eq!(Register::RSI, op.index);
		assert_eq!(4, op.scale);
		assert_eq!(0x1234_5678, op.displacement);
		assert_eq!(8, op.displ_size);
		assert!(op.is_broadcast);
		assert_eq!(Register::FS, op.segment_prefix);
	}
	{
		let op = MemoryOperand::with_base_index_scale_bcst_seg(Register::RCX, Register::RSI, 4, true, Register::FS);
		assert_eq!(Register::RCX, op.base);
		assert_eq!(Register::RSI, op.index);
		assert_eq!(4, op.scale);
		assert_eq!(0, op.displacement);
		assert_eq!(0, op.displ_size);
		assert!(op.is_broadcast);
		assert_eq!(Register::FS, op.segment_prefix);
	}
	{
		let op = MemoryOperand::with_base_displ_size_bcst_seg(Register::RCX, 0x1234_5678, 8, true, Register::FS);
		assert_eq!(Register::RCX, op.base);
		assert_eq!(Register::None, op.index);
		assert_eq!(1, op.scale);
		assert_eq!(0x1234_5678, op.displacement);
		assert_eq!(8, op.displ_size);
		assert!(op.is_broadcast);
		assert_eq!(Register::FS, op.segment_prefix);
	}
	{
		let op = MemoryOperand::with_index_scale_displ_size_bcst_seg(Register::RSI, 4, 0x1234_5678, 8, true, Register::FS);
		assert_eq!(Register::None, op.base);
		assert_eq!(Register::RSI, op.index);
		assert_eq!(4, op.scale);
		assert_eq!(0x1234_5678, op.displacement);
		assert_eq!(8, op.displ_size);
		assert!(op.is_broadcast);
		assert_eq!(Register::FS, op.segment_prefix);
	}
	{
		let op = MemoryOperand::with_base_displ_bcst_seg(Register::RCX, 0x1234_5678, true, Register::FS);
		assert_eq!(Register::RCX, op.base);
		assert_eq!(Register::None, op.index);
		assert_eq!(1, op.scale);
		assert_eq!(0x1234_5678, op.displacement);
		assert_eq!(1, op.displ_size);
		assert!(op.is_broadcast);
		assert_eq!(Register::FS, op.segment_prefix);
	}
	{
		let op = MemoryOperand::with_base_index_scale_displ_size(Register::RCX, Register::RSI, 4, 0x1234_5678, 8);
		assert_eq!(Register::RCX, op.base);
		assert_eq!(Register::RSI, op.index);
		assert_eq!(4, op.scale);
		assert_eq!(0x1234_5678, op.displacement);
		assert_eq!(8, op.displ_size);
		assert!(!op.is_broadcast);
		assert_eq!(Register::None, op.segment_prefix);
	}
	{
		let op = MemoryOperand::with_base_index_scale(Register::RCX, Register::RSI, 4);
		assert_eq!(Register::RCX, op.base);
		assert_eq!(Register::RSI, op.index);
		assert_eq!(4, op.scale);
		assert_eq!(0, op.displacement);
		assert_eq!(0, op.displ_size);
		assert!(!op.is_broadcast);
		assert_eq!(Register::None, op.segment_prefix);
	}
	{
		let op = MemoryOperand::with_base_index(Register::RCX, Register::RSI);
		assert_eq!(Register::RCX, op.base);
		assert_eq!(Register::RSI, op.index);
		assert_eq!(1, op.scale);
		assert_eq!(0, op.displacement);
		assert_eq!(0, op.displ_size);
		assert!(!op.is_broadcast);
		assert_eq!(Register::None, op.segment_prefix);
	}
	{
		let op = MemoryOperand::with_base_displ_size(Register::RCX, 0x1234_5678, 8);
		assert_eq!(Register::RCX, op.base);
		assert_eq!(Register::None, op.index);
		assert_eq!(1, op.scale);
		assert_eq!(0x1234_5678, op.displacement);
		assert_eq!(8, op.displ_size);
		assert!(!op.is_broadcast);
		assert_eq!(Register::None, op.segment_prefix);
	}
	{
		let op = MemoryOperand::with_index_scale_displ_size(Register::RSI, 4, 0x1234_5678, 8);
		assert_eq!(Register::None, op.base);
		assert_eq!(Register::RSI, op.index);
		assert_eq!(4, op.scale);
		assert_eq!(0x1234_5678, op.displacement);
		assert_eq!(8, op.displ_size);
		assert!(!op.is_broadcast);
		assert_eq!(Register::None, op.segment_prefix);
	}
	{
		let op = MemoryOperand::with_base_displ(Register::RCX, 0x1234_5678);
		assert_eq!(Register::RCX, op.base);
		assert_eq!(Register::None, op.index);
		assert_eq!(1, op.scale);
		assert_eq!(0x1234_5678, op.displacement);
		assert_eq!(1, op.displ_size);
		assert!(!op.is_broadcast);
		assert_eq!(Register::None, op.segment_prefix);
	}
	{
		let op = MemoryOperand::with_base(Register::RCX);
		assert_eq!(Register::RCX, op.base);
		assert_eq!(Register::None, op.index);
		assert_eq!(1, op.scale);
		assert_eq!(0, op.displacement);
		assert_eq!(0, op.displ_size);
		assert!(!op.is_broadcast);
		assert_eq!(Register::None, op.segment_prefix);
	}
}

lazy_static! {
	static ref OP_CODE_INFO_TEST_CASES: Vec<OpCodeInfoTestCase> = {
		let mut filename = get_encoder_unit_tests_dir();
		filename.push("OpCodeInfos.txt");
		OpCodeInfoTestParser::new(filename.as_path()).into_iter().collect()
	};
}

#[test]
fn test_all_op_code_infos() {
	for tc in &*OP_CODE_INFO_TEST_CASES {
		test_op_code_info(tc);
	}
}

fn test_op_code_info(tc: &OpCodeInfoTestCase) {
	let info = tc.code.op_code();
	assert_eq!(tc.code, info.code());
	assert_eq!(tc.op_code_string, info.op_code_string());
	assert_eq!(tc.instruction_string, info.instruction_string());
	{
		let mut display = String::with_capacity(tc.instruction_string.len());
		write!(display, "{}", info).unwrap();
		assert_eq!(tc.instruction_string, display);
	}
	assert_eq!(tc.instruction_string, info.to_string());
	assert_eq!(tc.encoding, info.encoding());
	assert_eq!(tc.is_instruction, info.is_instruction());
	assert_eq!(tc.mode16, info.mode16());
	assert_eq!(tc.mode16, info.is_available_in_mode(16));
	assert_eq!(tc.mode32, info.mode32());
	assert_eq!(tc.mode32, info.is_available_in_mode(32));
	assert_eq!(tc.mode64, info.mode64());
	assert_eq!(tc.mode64, info.is_available_in_mode(64));
	assert_eq!(tc.fwait, info.fwait());
	assert_eq!(tc.operand_size, info.operand_size());
	assert_eq!(tc.address_size, info.address_size());
	assert_eq!(tc.l, info.l());
	assert_eq!(tc.w, info.w());
	assert_eq!(tc.is_lig, info.is_lig());
	assert_eq!(tc.is_wig, info.is_wig());
	assert_eq!(tc.is_wig32, info.is_wig32());
	assert_eq!(tc.tuple_type, info.tuple_type());
	assert_eq!(tc.can_broadcast, info.can_broadcast());
	assert_eq!(tc.can_use_rounding_control, info.can_use_rounding_control());
	assert_eq!(tc.can_suppress_all_exceptions, info.can_suppress_all_exceptions());
	assert_eq!(tc.can_use_op_mask_register, info.can_use_op_mask_register());
	assert_eq!(tc.require_non_zero_op_mask_register, info.require_non_zero_op_mask_register());
	if tc.require_non_zero_op_mask_register {
		assert!(info.can_use_op_mask_register());
		assert!(!info.can_use_zeroing_masking());
	}
	assert_eq!(tc.can_use_zeroing_masking, info.can_use_zeroing_masking());
	assert_eq!(tc.can_use_lock_prefix, info.can_use_lock_prefix());
	assert_eq!(tc.can_use_xacquire_prefix, info.can_use_xacquire_prefix());
	assert_eq!(tc.can_use_xrelease_prefix, info.can_use_xrelease_prefix());
	assert_eq!(tc.can_use_rep_prefix, info.can_use_rep_prefix());
	assert_eq!(tc.can_use_repne_prefix, info.can_use_repne_prefix());
	assert_eq!(tc.can_use_bnd_prefix, info.can_use_bnd_prefix());
	assert_eq!(tc.can_use_hint_taken_prefix, info.can_use_hint_taken_prefix());
	assert_eq!(tc.can_use_notrack_prefix, info.can_use_notrack_prefix());
	assert_eq!(tc.table, info.table());
	assert_eq!(tc.mandatory_prefix, info.mandatory_prefix());
	assert_eq!(tc.op_code, info.op_code());
	assert_eq!(tc.is_group, info.is_group());
	assert_eq!(tc.group_index, info.group_index());
	assert_eq!(tc.op_count, info.op_count());
	assert_eq!(tc.op0_kind, info.op0_kind());
	assert_eq!(tc.op1_kind, info.op1_kind());
	assert_eq!(tc.op2_kind, info.op2_kind());
	assert_eq!(tc.op3_kind, info.op3_kind());
	assert_eq!(tc.op4_kind, info.op4_kind());
	assert_eq!(tc.op0_kind, info.op_kind(0));
	assert_eq!(tc.op1_kind, info.op_kind(1));
	assert_eq!(tc.op2_kind, info.op_kind(2));
	assert_eq!(tc.op3_kind, info.op_kind(3));
	assert_eq!(tc.op4_kind, info.op_kind(4));
	const_assert_eq!(5, IcedConstants::MAX_OP_COUNT);
	for i in tc.op_count..IcedConstants::MAX_OP_COUNT as u32 {
		assert_eq!(OpCodeOperandKind::None, info.op_kind(i));
	}
}

#[test]
#[should_panic]
fn op_kind_panics_if_invalid_input() {
	let op_code = Code::Aaa.op_code();
	let _ = op_code.op_kind(IcedConstants::MAX_OP_COUNT as u32);
}

#[allow(trivial_casts)]
#[test]
fn verify_instruction_op_code_info() {
	for i in 0..IcedConstants::NUMBER_OF_CODE_VALUES {
		let code: Code = unsafe { mem::transmute(i as u16) };
		let mut instr = Instruction::default();
		instr.set_code(code);
		assert_eq!(instr.op_code() as *const _ as usize, code.op_code() as *const _ as usize);
	}
}

#[test]
fn make_sure_all_code_values_are_tested_exactly_once() {
	let mut tested = [false; IcedConstants::NUMBER_OF_CODE_VALUES];
	for tc in &*OP_CODE_INFO_TEST_CASES {
		assert!(!tested[tc.code as usize]);
		tested[tc.code as usize] = true;
	}
	let mut s = String::new();
	for i in tested.iter().enumerate() {
		let code: Code = unsafe { mem::transmute(i.0 as u16) };
		if !*i.1 {
			if !s.is_empty() {
				s.push(',');
			}
			write!(s, "{:?}", code).unwrap();
		}
	}
	assert_eq!("", s);
}

#[test]
#[should_panic]
fn op_code_info_is_available_in_mode_panics_if_invalid_bitness_0() {
	let _ = Code::Nopd.op_code().is_available_in_mode(0);
}

#[test]
#[should_panic]
fn op_code_info_is_available_in_mode_panics_if_invalid_bitness_128() {
	let _ = Code::Nopd.op_code().is_available_in_mode(128);
}

#[test]
fn write_byte_works() {
	let mut encoder = Encoder::new(64);
	let instr = Instruction::with_reg_reg(Code::Add_r64_rm64, Register::R8, Register::RBP);
	encoder.write_u8(0x90);
	assert_eq!(3, encoder.encode(&instr, 0x5555_5555).unwrap());
	encoder.write_u8(0xCC);
	assert_eq!(vec![0x90, 0x4C, 0x03, 0xC5, 0xCC], encoder.take_buffer());
}
