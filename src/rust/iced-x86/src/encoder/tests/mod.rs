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

use super::super::decoder::tests::test_utils::*;
use super::super::iced_constants::IcedConstants;
use super::super::test_utils::from_str_conv::to_vec_u8;
use super::super::test_utils::*;
use super::super::*;

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
	assert!(orig_instr.len() <= IcedConstants::MAX_INSTRUCTION_LENGTH as usize);
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
		assert_eq!(
			slice_u8_to_string(expected_bytes.as_slice()),
			slice_u8_to_string(encoded_bytes.as_slice())
		);
		panic!();
	}

	let mut new_instr = create_decoder(info.bitness(), encoded_bytes.as_slice(), info.decoder_options())
		.0
		.decode();
	assert_eq!(info.code(), new_instr.code());
	assert_eq!(encoded_bytes.len(), new_instr.len());
	new_instr.set_len(orig_instr.len());
	new_instr.set_next_ip(orig_instr.next_ip());
	if orig_bytes.len() != expected_bytes.len() && (orig_instr.memory_base() == Register::EIP || orig_instr.memory_base() == Register::RIP) {
		let displ = new_instr
			.memory_displacement()
			.wrapping_add((expected_bytes.len().wrapping_sub(orig_bytes.len())) as u32);
		new_instr.set_memory_displacement(displ);
	}
	assert!(orig_instr.eq_all_bits(&new_instr));
	// Some tests use useless or extra prefixes, so we can't verify the exact length
	assert!(
		encoded_bytes.len() <= orig_bytes.len(),
		"Unexpected encoded prefixes: {}",
		slice_u8_to_string(encoded_bytes.as_slice())
	);
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
	use std::fmt::Write;
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
fn with_capacity_if_bitness_128() {
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
