// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::decoder::tests::test_utils::decoder_tests;
use crate::iced_constants::IcedConstants;
use crate::test_utils::from_str_conv::to_vec_u8;
use crate::test_utils::*;
use crate::*;
use alloc::vec::Vec;
use std::collections::HashMap;

fn decoder_new_panics(bitness: u32) {
	let _ = Decoder::new(bitness, b"\x90", DecoderOptions::NONE);
}

fn decoder_try_new_fails(bitness: u32) {
	assert!(Decoder::try_new(bitness, b"\x90", DecoderOptions::NONE).is_err());
}

#[test]
#[should_panic]
fn decoder_new_panics_0() {
	decoder_new_panics(0);
}

#[test]
#[should_panic]
fn decoder_new_panics_128() {
	decoder_new_panics(128);
}

#[test]
fn decoder_try_new_fails_0() {
	decoder_try_new_fails(0);
}

#[test]
fn decoder_try_new_fails_128() {
	decoder_try_new_fails(128);
}

#[test]
fn decoder_try_new_succeeds_16() {
	let mut decoder = Decoder::try_new(16, b"\x90", DecoderOptions::NONE).unwrap();
	assert_eq!(decoder.decode().code(), Code::Nopw);
}

#[test]
fn decoder_try_new_succeeds_32() {
	let mut decoder = Decoder::try_new(32, b"\x90", DecoderOptions::NONE).unwrap();
	assert_eq!(decoder.decode().code(), Code::Nopd);
}

#[test]
fn decoder_try_new_succeeds_64() {
	let mut decoder = Decoder::try_new(64, b"\x90", DecoderOptions::NONE).unwrap();
	assert_eq!(decoder.decode().code(), Code::Nopd);
}

#[test]
fn decode_multiple_instrs_with_one_instance() {
	let tests = decoder_tests(true, true);

	let mut bytes_map16: HashMap<(u32, u32), Vec<u8>> = HashMap::new();
	let mut bytes_map32: HashMap<(u32, u32), Vec<u8>> = HashMap::new();
	let mut bytes_map64: HashMap<(u32, u32), Vec<u8>> = HashMap::new();

	let mut map16: HashMap<(u32, u32), Decoder<'_>> = HashMap::new();
	let mut map32: HashMap<(u32, u32), Decoder<'_>> = HashMap::new();
	let mut map64: HashMap<(u32, u32), Decoder<'_>> = HashMap::new();

	for tc in &tests {
		let bytes_map = match tc.bitness() {
			16 => &mut bytes_map16,
			32 => &mut bytes_map32,
			64 => &mut bytes_map64,
			_ => unreachable!(),
		};
		let key = (tc.bitness(), tc.decoder_options());
		let vec = bytes_map.entry(key).or_insert_with(Vec::<u8>::default);
		let bytes = to_vec_u8(tc.hex_bytes()).unwrap();
		vec.extend(bytes);
	}

	for tc in &tests {
		let (bytes_map, map) = match tc.bitness() {
			16 => (&bytes_map16, &mut map16),
			32 => (&bytes_map32, &mut map32),
			64 => (&bytes_map64, &mut map64),
			_ => unreachable!(),
		};
		let key = (tc.bitness(), tc.decoder_options());
		let vec = bytes_map.get(&key).unwrap();
		let _ = map.entry(key).or_insert_with(|| Decoder::new(tc.bitness(), vec, tc.decoder_options()));
	}

	let mut instr2 = Instruction::default();
	for tc in &tests {
		let bytes = to_vec_u8(tc.hex_bytes()).unwrap();
		let mut decoder = create_decoder(tc.bitness(), &bytes, tc.ip(), tc.decoder_options()).0;
		let key = (tc.bitness(), tc.decoder_options());
		let decoder_all = match tc.bitness() {
			16 => map16.get_mut(&key).unwrap(),
			32 => map32.get_mut(&key).unwrap(),
			64 => map64.get_mut(&key).unwrap(),
			_ => unreachable!(),
		};
		let ip = decoder.ip();
		decoder_all.set_ip(ip);

		let position = decoder_all.position();
		let instr1 = decoder.decode();
		decoder_all.decode_out(&mut instr2);
		let co1 = decoder.get_constant_offsets(&instr1);
		let co2 = decoder_all.get_constant_offsets(&instr2);
		if instr1.is_invalid() {
			// decoder_all has a bigger buffer and can decode more bytes.
			// It sometimes happens that instr1 is invalid but the full buffer is a valid instruction.
			// In that case, just ignore it.
			decoder_all.try_set_position(position + bytes.len()).unwrap();
			if !instr2.is_invalid() {
				continue;
			}
			let len = bytes.len().min(IcedConstants::MAX_INSTRUCTION_LENGTH);
			instr2.set_len(len);
			instr2.set_next_ip(ip.wrapping_add(len as u64));
		}
		assert_eq!(instr1.code(), instr2.code());
		assert!(instr1.eq_all_bits(&instr2));
		assert!(instr2.eq_all_bits(&instr1));
		super::verify_constant_offsets(&co1, &co2);
	}
}

#[test]
fn position() {
	const BITNESS: u32 = 64;
	let bytes = b"\x23\x18\x48\x89\xCE";
	let mut decoder = Decoder::with_ip(BITNESS, bytes, get_default_ip(BITNESS), DecoderOptions::NONE);

	assert!(decoder.can_decode());
	assert_eq!(decoder.position(), 0);
	assert_eq!(decoder.max_position(), bytes.len());

	let instr_a1 = decoder.decode();
	assert_eq!(instr_a1.code(), Code::And_r32_rm32);

	assert!(decoder.can_decode());
	assert_eq!(decoder.position(), 2);
	assert_eq!(decoder.max_position(), bytes.len());

	let instr_b1 = decoder.decode();
	assert_eq!(instr_b1.code(), Code::Mov_rm64_r64);

	assert!(!decoder.can_decode());
	assert_eq!(decoder.position(), 5);
	assert_eq!(decoder.max_position(), bytes.len());

	decoder.set_ip(get_default_ip(BITNESS) + 2);
	assert_eq!(decoder.position(), 5);
	decoder.try_set_position(2).unwrap();
	assert!(decoder.can_decode());
	assert_eq!(decoder.position(), 2);
	assert_eq!(decoder.max_position(), bytes.len());

	let instr_b2 = decoder.decode();
	assert_eq!(instr_b2.code(), Code::Mov_rm64_r64);

	decoder.set_ip(get_default_ip(BITNESS));
	assert_eq!(decoder.position(), 5);
	decoder.try_set_position(0).unwrap();
	assert!(decoder.can_decode());
	assert_eq!(decoder.position(), 0);
	assert_eq!(decoder.max_position(), bytes.len());

	let instr_a2 = decoder.decode();
	assert_eq!(instr_a2.code(), Code::And_r32_rm32);

	assert!(instr_a1.eq_all_bits(&instr_a2));
	assert!(instr_b1.eq_all_bits(&instr_b2));
}

#[test]
fn set_position_valid_position() {
	let bytes = b"\x23\x18\x48\x89\xCE";
	let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	for i in 0..bytes.len() + 1 {
		decoder.set_position(i).unwrap();
		assert_eq!(decoder.position(), i);
	}
	for i in (0..bytes.len() + 1).rev() {
		decoder.set_position(i).unwrap();
		assert_eq!(decoder.position(), i);
	}
	let mut decoder = Decoder::new(64, b"", DecoderOptions::NONE);
	decoder.set_position(0).unwrap();
	assert_eq!(decoder.position(), 0);
}

#[test]
fn try_set_position_valid_position() {
	let bytes = b"\x23\x18\x48\x89\xCE";
	let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	for i in 0..bytes.len() + 1 {
		decoder.try_set_position(i).unwrap();
		assert_eq!(decoder.position(), i);
	}
	for i in (0..bytes.len() + 1).rev() {
		decoder.try_set_position(i).unwrap();
		assert_eq!(decoder.position(), i);
	}
	let mut decoder = Decoder::new(64, b"", DecoderOptions::NONE);
	decoder.try_set_position(0).unwrap();
	assert_eq!(decoder.position(), 0);
}

#[test]
#[should_panic]
fn set_position_panics_if_invalid() {
	let bytes = b"\x23\x18\x48\x89\xCE";
	let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	decoder.set_position(bytes.len() + 1).unwrap();
}

#[test]
fn try_set_position_fails_if_invalid() {
	let bytes = b"\x23\x18\x48\x89\xCE";
	let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	assert!(decoder.try_set_position(bytes.len() + 1).is_err());
}

#[test]
fn decoder_for_loop_into_iter() {
	let bytes = b"\x23\x18\x48\x89\xCE";
	let decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	let mut instrs: Vec<Instruction> = Vec::new();
	for instr in decoder {
		instrs.push(instr);
	}
	assert_eq!(instrs.len(), 2);
	assert_eq!(instrs[0].code(), Code::And_r32_rm32);
	assert_eq!(instrs[1].code(), Code::Mov_rm64_r64);
}

#[test]
fn decoder_for_loop_ref_mut_decoder() {
	let bytes = b"\x23\x18\x48\x89\xCE";
	let mut decoder = Decoder::with_ip(64, bytes, 0x1234_5678_9ABC_DEF0, DecoderOptions::NONE);
	let mut instrs: Vec<Instruction> = Vec::new();
	for instr in &mut decoder {
		instrs.push(instr);
	}
	assert_eq!(decoder.ip(), 0x1234_5678_9ABC_DEF5);
	assert!(!decoder.can_decode());
	assert_eq!(decoder.position(), 5);
	assert_eq!(instrs.len(), 2);
	assert_eq!(instrs[0].code(), Code::And_r32_rm32);
	assert_eq!(instrs[1].code(), Code::Mov_rm64_r64);
}

#[test]
fn decoder_for_loop_decoder_iter() {
	let bytes = b"\x23\x18\x48\x89\xCE";
	let mut decoder = Decoder::with_ip(64, bytes, 0x1234_5678_9ABC_DEF0, DecoderOptions::NONE);
	let mut instrs: Vec<Instruction> = Vec::new();
	for instr in &mut decoder {
		instrs.push(instr);
	}
	assert_eq!(decoder.ip(), 0x1234_5678_9ABC_DEF5);
	assert!(!decoder.can_decode());
	assert_eq!(decoder.position(), 5);
	assert_eq!(instrs.len(), 2);
	assert_eq!(instrs[0].code(), Code::And_r32_rm32);
	assert_eq!(instrs[1].code(), Code::Mov_rm64_r64);
}

#[test]
fn decode_ip_xxxxxxxxffffffff() {
	let bytes = b"\x90";
	let mut decoder = Decoder::with_ip(64, bytes, 0x1234_5678_FFFF_FFFF, DecoderOptions::NONE);
	let _ = decoder.decode();
	assert_eq!(decoder.ip(), 0x1234_5679_0000_0000);
}

#[test]
fn decode_with_too_few_bytes_left() {
	for tc in decoder_tests(true, false) {
		let bytes = to_vec_u8(tc.hex_bytes()).unwrap();
		for i in 0..bytes.len() - 1 {
			let mut decoder = Decoder::with_ip(tc.bitness(), &bytes[0..i], 0x1000, tc.decoder_options());
			let instr = decoder.decode();
			assert_eq!(decoder.ip(), 0x1000 + i as u64);
			assert_eq!(instr.code(), Code::INVALID);
			assert_eq!(decoder.last_error(), DecoderError::NoMoreBytes);
		}
	}
}

#[test]
#[cfg(feature = "encoder")]
fn instruction_operator_eq_neq() {
	let instr1a = Instruction::with2(Code::Mov_r64_rm64, Register::RAX, Register::RCX).unwrap();
	let instr1b = instr1a;
	let instr2 = Instruction::with2(Code::Mov_r64_rm64, Register::RAX, Register::RDX).unwrap();
	assert_eq!(instr1a == instr1b, true);
	assert_eq!(instr1a == instr2, false);
	assert_eq!(instr1a != instr2, true);
	assert_eq!(instr1a != instr1b, false);
}
