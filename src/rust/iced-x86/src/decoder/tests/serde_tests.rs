// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

// Test ser/de with serde_json and bincode since they trigger different code paths in our generated ser/de impls

use crate::decoder::tests::decoder_mem_test_case::DecoderMemoryTestCase;
use crate::decoder::tests::decoder_test_case::DecoderTestCase;
use crate::decoder::tests::test_cases;
use crate::test_utils::create_decoder;
use crate::test_utils::from_str_conv::to_vec_u8;
use crate::*;

struct TestCase {
	bitness: u32,
	hex_bytes: &'static str,
	ip: u64,
	code: Code,
	decoder_options: u32,
}

impl From<&'static DecoderTestCase> for TestCase {
	fn from(tc: &'static DecoderTestCase) -> Self {
		Self { bitness: tc.bitness, hex_bytes: &tc.hex_bytes, ip: tc.ip, code: tc.code, decoder_options: tc.decoder_options }
	}
}

impl From<&'static DecoderMemoryTestCase> for TestCase {
	fn from(tc: &'static DecoderMemoryTestCase) -> Self {
		Self { bitness: tc.bitness, hex_bytes: &tc.hex_bytes, ip: tc.ip, code: tc.code, decoder_options: tc.decoder_options }
	}
}

#[test]
fn test_serde_json() {
	test_serde(|instruction| {
		let ser_str = serde_json::to_string(instruction).unwrap();
		serde_json::from_str(&ser_str).unwrap()
	});
}

#[test]
fn test_bincode() {
	test_serde(|instruction| {
		let ser_bytes = bincode::serialize(instruction).unwrap();
		#[cfg(not(feature = "__internal_flip"))]
		assert_eq!(ser_bytes.len(), INSTRUCTION_TOTAL_SIZE);
		bincode::deserialize(&ser_bytes).unwrap()
	});
}

fn test_serde<F: Fn(&Instruction) -> Instruction>(f: F) {
	for &bitness in &[16, 32, 64] {
		let tests = test_cases::get_test_cases(bitness)
			.iter()
			.map(TestCase::from)
			.chain(test_cases::get_misc_test_cases(bitness).iter().map(TestCase::from))
			.chain(test_cases::get_mem_test_cases(bitness).iter().map(TestCase::from));
		for tc in tests {
			let bytes = to_vec_u8(tc.hex_bytes).unwrap();
			let (mut decoder, _, _) = create_decoder(tc.bitness, &bytes, tc.ip, tc.decoder_options);
			let instr = decoder.decode();
			assert_eq!(instr.code(), tc.code);
			let de_instr = f(&instr);
			assert!(instr.eq_all_bits(&de_instr));
		}
	}
	#[cfg(feature = "encoder")]
	{
		let instr = Instruction::with_declare_byte(b"\x03\x73\x27\x80\xE7\x49\x11\xEB\xB2\xFA\x13\xC8\x87\x42\xF7\x4B").unwrap();
		let de_instr = f(&instr);
		assert!(instr.eq_all_bits(&de_instr));
	}
}
