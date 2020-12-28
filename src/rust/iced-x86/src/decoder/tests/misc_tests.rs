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

use super::super::super::test_utils::from_str_conv::to_vec_u8;
use super::super::super::test_utils::*;
use super::super::super::*;
use super::test_utils::decoder_tests;
#[cfg(not(feature = "std"))]
use alloc::vec::Vec;
#[cfg(not(feature = "std"))]
use hashbrown::HashMap;
#[cfg(feature = "std")]
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
	assert_eq!(Code::Nopw, decoder.decode().code());
}

#[test]
fn decoder_try_new_succeeds_32() {
	let mut decoder = Decoder::try_new(32, b"\x90", DecoderOptions::NONE).unwrap();
	assert_eq!(Code::Nopd, decoder.decode().code());
}

#[test]
fn decoder_try_new_succeeds_64() {
	let mut decoder = Decoder::try_new(64, b"\x90", DecoderOptions::NONE).unwrap();
	assert_eq!(Code::Nopd, decoder.decode().code());
}

#[test]
fn decode_multiple_instrs_with_one_instance() {
	let tests = decoder_tests(false, true);

	let mut bytes_map16: HashMap<(u32, u32), Vec<u8>> = HashMap::new();
	let mut bytes_map32: HashMap<(u32, u32), Vec<u8>> = HashMap::new();
	let mut bytes_map64: HashMap<(u32, u32), Vec<u8>> = HashMap::new();

	let mut map16: HashMap<(u32, u32), Decoder> = HashMap::new();
	let mut map32: HashMap<(u32, u32), Decoder> = HashMap::new();
	let mut map64: HashMap<(u32, u32), Decoder> = HashMap::new();

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
		let mut decoder = create_decoder(tc.bitness(), &bytes, tc.decoder_options()).0;
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
		assert_eq!(instr1.code(), instr2.code());
		if instr1.is_invalid() {
			// decoder_all has a bigger buffer and can decode more bytes
			decoder_all.try_set_position(position + bytes.len()).unwrap();
			instr2.set_len(bytes.len());
			instr2.set_next_ip(ip + bytes.len() as u64);
		}
		assert!(instr1.eq_all_bits(&instr2));
		assert!(instr2.eq_all_bits(&instr1));
		super::verify_constant_offsets(&co1, &co2);
	}
}

#[test]
fn position() {
	const BITNESS: u32 = 64;
	let bytes = b"\x23\x18\x48\x89\xCE";
	let mut decoder = Decoder::new(BITNESS, bytes, DecoderOptions::NONE);
	decoder.set_ip(get_default_ip(BITNESS));

	assert!(decoder.can_decode());
	assert_eq!(0, decoder.position());
	assert_eq!(bytes.len(), decoder.max_position());

	let instr_a1 = decoder.decode();
	assert_eq!(Code::And_r32_rm32, instr_a1.code());

	assert!(decoder.can_decode());
	assert_eq!(2, decoder.position());
	assert_eq!(bytes.len(), decoder.max_position());

	let instr_b1 = decoder.decode();
	assert_eq!(Code::Mov_rm64_r64, instr_b1.code());

	assert!(!decoder.can_decode());
	assert_eq!(5, decoder.position());
	assert_eq!(bytes.len(), decoder.max_position());

	decoder.set_ip(get_default_ip(BITNESS) + 2);
	assert_eq!(5, decoder.position());
	decoder.try_set_position(2).unwrap();
	assert!(decoder.can_decode());
	assert_eq!(2, decoder.position());
	assert_eq!(bytes.len(), decoder.max_position());

	let instr_b2 = decoder.decode();
	assert_eq!(Code::Mov_rm64_r64, instr_b2.code());

	decoder.set_ip(get_default_ip(BITNESS));
	assert_eq!(5, decoder.position());
	decoder.try_set_position(0).unwrap();
	assert!(decoder.can_decode());
	assert_eq!(0, decoder.position());
	assert_eq!(bytes.len(), decoder.max_position());

	let instr_a2 = decoder.decode();
	assert_eq!(Code::And_r32_rm32, instr_a2.code());

	assert!(instr_a1.eq_all_bits(&instr_a2));
	assert!(instr_b1.eq_all_bits(&instr_b2));
}

#[test]
fn set_position_valid_position() {
	let bytes = b"\x23\x18\x48\x89\xCE";
	let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	for i in 0..bytes.len() + 1 {
		#[allow(deprecated)]
		{
			decoder.set_position(i);
		}
		assert_eq!(i, decoder.position());
	}
	for i in (0..bytes.len() + 1).rev() {
		#[allow(deprecated)]
		{
			decoder.set_position(i);
		}
		assert_eq!(i, decoder.position());
	}
	let mut decoder = Decoder::new(64, b"", DecoderOptions::NONE);
	#[allow(deprecated)]
	{
		decoder.set_position(0);
	}
	assert_eq!(0, decoder.position());
}

#[test]
fn try_set_position_valid_position() {
	let bytes = b"\x23\x18\x48\x89\xCE";
	let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	for i in 0..bytes.len() + 1 {
		decoder.try_set_position(i).unwrap();
		assert_eq!(i, decoder.position());
	}
	for i in (0..bytes.len() + 1).rev() {
		decoder.try_set_position(i).unwrap();
		assert_eq!(i, decoder.position());
	}
	let mut decoder = Decoder::new(64, b"", DecoderOptions::NONE);
	decoder.try_set_position(0).unwrap();
	assert_eq!(0, decoder.position());
}

#[test]
#[should_panic]
fn set_position_panics_if_invalid() {
	let bytes = b"\x23\x18\x48\x89\xCE";
	let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	#[allow(deprecated)]
	{
		decoder.set_position(bytes.len() + 1);
	}
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
	assert_eq!(2, instrs.len());
	assert_eq!(Code::And_r32_rm32, instrs[0].code());
	assert_eq!(Code::Mov_rm64_r64, instrs[1].code());
}

#[test]
fn decoder_for_loop_ref_mut_decoder() {
	let bytes = b"\x23\x18\x48\x89\xCE";
	let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	decoder.set_ip(0x1234_5678_9ABC_DEF0);
	let mut instrs: Vec<Instruction> = Vec::new();
	for instr in &mut decoder {
		instrs.push(instr);
	}
	assert_eq!(0x1234_5678_9ABC_DEF5, decoder.ip());
	assert!(!decoder.can_decode());
	assert_eq!(5, decoder.position());
	assert_eq!(2, instrs.len());
	assert_eq!(Code::And_r32_rm32, instrs[0].code());
	assert_eq!(Code::Mov_rm64_r64, instrs[1].code());
}

#[test]
fn decoder_for_loop_decoder_iter() {
	let bytes = b"\x23\x18\x48\x89\xCE";
	let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	decoder.set_ip(0x1234_5678_9ABC_DEF0);
	let mut instrs: Vec<Instruction> = Vec::new();
	for instr in decoder.iter() {
		instrs.push(instr);
	}
	assert_eq!(0x1234_5678_9ABC_DEF5, decoder.ip());
	assert!(!decoder.can_decode());
	assert_eq!(5, decoder.position());
	assert_eq!(2, instrs.len());
	assert_eq!(Code::And_r32_rm32, instrs[0].code());
	assert_eq!(Code::Mov_rm64_r64, instrs[1].code());
}

#[test]
fn decode_ip_xxxxxxxxffffffff() {
	let bytes = b"\x90";
	let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	decoder.set_ip(0x1234_5678_FFFF_FFFF);
	let _ = decoder.decode();
	assert_eq!(0x1234_5679_0000_0000, decoder.ip());
}

#[test]
fn decode_with_too_few_bytes_left() {
	for tc in decoder_tests(true, false) {
		let bytes = to_vec_u8(tc.hex_bytes()).unwrap();
		for i in 0..bytes.len() - 1 {
			let mut decoder = Decoder::new(tc.bitness(), &bytes[0..i], tc.decoder_options());
			decoder.set_ip(0x1000);
			let instr = decoder.decode();
			assert_eq!(0x1000 + i as u64, decoder.ip());
			assert_eq!(Code::INVALID, instr.code());
			assert_eq!(DecoderError::NoMoreBytes, decoder.last_error());
		}
	}
}

#[test]
#[cfg(feature = "encoder")]
fn instruction_operator_eq_neq() {
	let instr1a = Instruction::with_reg_reg(Code::Mov_r64_rm64, Register::RAX, Register::RCX);
	let instr1b = instr1a;
	let instr2 = Instruction::with_reg_reg(Code::Mov_r64_rm64, Register::RAX, Register::RDX);
	assert_eq!(true, instr1a == instr1b);
	assert_eq!(false, instr1a == instr2);
	assert_eq!(true, instr1a != instr2);
	assert_eq!(false, instr1a != instr1b);
}
