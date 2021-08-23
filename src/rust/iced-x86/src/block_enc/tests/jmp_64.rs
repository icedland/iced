// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::block_enc::tests::*;

const BITNESS: u32 = 64;
const ORIG_RIP: u64 = 0x8000;
const NEW_RIP: u64 = 0x8000_0000_0000_0000;

#[test]
fn jmp_fwd() {
	#[rustfmt::skip]
	let original_data = [
		/*0000*/ 0xB0, 0x00,// mov al,0
		/*0002*/ 0xEB, 0x09,// jmp short 000000000000800Dh
		/*0004*/ 0xB0, 0x01,// mov al,1
		/*0006*/ 0xE9, 0x02, 0x00, 0x00, 0x00,// jmp near ptr 000000000000800Dh
		/*000B*/ 0xB0, 0x02,// mov al,2
		/*000D*/ 0x90,// nop
	];
	#[rustfmt::skip]
	let new_data = [
		/*0000*/ 0xB0, 0x00,// mov al,0
		/*0002*/ 0xEB, 0x06,// jmp short 800000000000000Ah
		/*0004*/ 0xB0, 0x01,// mov al,1
		/*0006*/ 0xEB, 0x02,// jmp short 800000000000000Ah
		/*0008*/ 0xB0, 0x02,// mov al,2
		/*000A*/ 0x90,// nop
	];
	#[rustfmt::skip]
	let expected_instruction_offsets = [
		0x0000,
		0x0002,
		0x0004,
		0x0006,
		0x0008,
		0x000A,
	];
	let expected_reloc_infos = [];
	const OPTIONS: u32 = BlockEncoderOptions::NONE;
	encode_test(
		BITNESS,
		ORIG_RIP,
		&original_data,
		NEW_RIP,
		&new_data,
		OPTIONS,
		DECODER_OPTIONS,
		&expected_instruction_offsets,
		&expected_reloc_infos,
	);
}

#[test]
fn jmp_bwd() {
	#[rustfmt::skip]
	let original_data = [
		/*0000*/ 0x90,// nop
		/*0001*/ 0xB0, 0x00,// mov al,0
		/*0003*/ 0xEB, 0xFB,// jmp short 0000000000008000h
		/*0005*/ 0xB0, 0x01,// mov al,1
		/*0007*/ 0xE9, 0xF4, 0xFF, 0xFF, 0xFF,// jmp near ptr 0000000000008000h
		/*000C*/ 0xB0, 0x02,// mov al,2
	];
	#[rustfmt::skip]
	let new_data = [
		/*0000*/ 0x90,// nop
		/*0001*/ 0xB0, 0x00,// mov al,0
		/*0003*/ 0xEB, 0xFB,// jmp short 8000000000000000h
		/*0005*/ 0xB0, 0x01,// mov al,1
		/*0007*/ 0xEB, 0xF7,// jmp short 8000000000000000h
		/*0009*/ 0xB0, 0x02,// mov al,2
	];
	#[rustfmt::skip]
	let expected_instruction_offsets = [
		0x0000,
		0x0001,
		0x0003,
		0x0005,
		0x0007,
		0x0009,
	];
	let expected_reloc_infos = [];
	const OPTIONS: u32 = BlockEncoderOptions::NONE;
	encode_test(
		BITNESS,
		ORIG_RIP,
		&original_data,
		NEW_RIP,
		&new_data,
		OPTIONS,
		DECODER_OPTIONS,
		&expected_instruction_offsets,
		&expected_reloc_infos,
	);
}

#[test]
fn jmp_other_short_os() {
	#[rustfmt::skip]
	let original_data = [
		/*0000*/ 0xB0, 0x00,// mov al,0
		/*0002*/ 0x66, 0xEB, 0x08,// jmp short 800Dh
		/*0005*/ 0xB0, 0x01,// mov al,1
		/*0007*/ 0x66, 0xE9, 0x02, 0x00,// jmp near ptr 800Dh
		/*000B*/ 0xB0, 0x02,// mov al,2
	];
	#[rustfmt::skip]
	let new_data = [
		/*0000*/ 0xB0, 0x00,// mov al,0
		/*0002*/ 0x66, 0xEB, 0x09,// jmp short 800Dh
		/*0005*/ 0xB0, 0x01,// mov al,1
		/*0007*/ 0x66, 0xEB, 0x04,// jmp short 800Dh
		/*000A*/ 0xB0, 0x02,// mov al,2
	];
	#[rustfmt::skip]
	let expected_instruction_offsets = [
		0x0000,
		0x0002,
		0x0005,
		0x0007,
		0x000A,
	];
	let expected_reloc_infos = [];
	const OPTIONS: u32 = BlockEncoderOptions::NONE;
	encode_test(
		BITNESS,
		ORIG_RIP,
		&original_data,
		ORIG_RIP - 1,
		&new_data,
		OPTIONS,
		DECODER_OPTIONS | DecoderOptions::AMD,
		&expected_instruction_offsets,
		&expected_reloc_infos,
	);
}

#[test]
fn jmp_other_near_os() {
	#[rustfmt::skip]
	let original_data = [
		/*0000*/ 0xB0, 0x00,// mov al,0
		/*0002*/ 0x66, 0xEB, 0x08,// jmp short 800Dh
		/*0005*/ 0xB0, 0x01,// mov al,1
		/*0007*/ 0x66, 0xE9, 0x02, 0x00,// jmp near ptr 800Dh
		/*000B*/ 0xB0, 0x02,// mov al,2
	];
	#[rustfmt::skip]
	let new_data = [
		/*0000*/ 0xB0, 0x00,// mov al,0
		/*0002*/ 0x66, 0xE9, 0x07, 0xF0,// jmp near ptr 800Dh
		/*0006*/ 0xB0, 0x01,// mov al,1
		/*0008*/ 0x66, 0xE9, 0x01, 0xF0,// jmp near ptr 800Dh
		/*000C*/ 0xB0, 0x02,// mov al,2
	];
	#[rustfmt::skip]
	let expected_instruction_offsets = [
		0x0000,
		0x0002,
		0x0006,
		0x0008,
		0x000C,
	];
	let expected_reloc_infos = [];
	const OPTIONS: u32 = BlockEncoderOptions::NONE;
	encode_test(
		BITNESS,
		ORIG_RIP,
		&original_data,
		ORIG_RIP + 0x1000,
		&new_data,
		OPTIONS,
		DECODER_OPTIONS | DecoderOptions::AMD,
		&expected_instruction_offsets,
		&expected_reloc_infos,
	);
}

#[test]
fn jmp_other_short() {
	#[rustfmt::skip]
	let original_data = [
		/*0000*/ 0xB0, 0x00,// mov al,0
		/*0002*/ 0xEB, 0x09,// jmp short 000000000000800Dh
		/*0004*/ 0xB0, 0x01,// mov al,1
		/*0006*/ 0xE9, 0x02, 0x00, 0x00, 0x00,// jmp near ptr 000000000000800Dh
		/*000B*/ 0xB0, 0x02,// mov al,2
	];
	#[rustfmt::skip]
	let new_data = [
		/*0000*/ 0xB0, 0x00,// mov al,0
		/*0002*/ 0xEB, 0x0A,// jmp short 000000000000800Dh
		/*0004*/ 0xB0, 0x01,// mov al,1
		/*0006*/ 0xEB, 0x06,// jmp short 000000000000800Dh
		/*0008*/ 0xB0, 0x02,// mov al,2
	];
	#[rustfmt::skip]
	let expected_instruction_offsets = [
		0x0000,
		0x0002,
		0x0004,
		0x0006,
		0x0008,
	];
	let expected_reloc_infos = [];
	const OPTIONS: u32 = BlockEncoderOptions::NONE;
	encode_test(
		BITNESS,
		ORIG_RIP,
		&original_data,
		ORIG_RIP - 1,
		&new_data,
		OPTIONS,
		DECODER_OPTIONS,
		&expected_instruction_offsets,
		&expected_reloc_infos,
	);
}

#[test]
fn jmp_other_near() {
	#[rustfmt::skip]
	let original_data = [
		/*0000*/ 0xB0, 0x00,// mov al,0
		/*0002*/ 0xEB, 0x09,// jmp short 000000000000800Dh
		/*0004*/ 0xB0, 0x01,// mov al,1
		/*0006*/ 0xE9, 0x02, 0x00, 0x00, 0x00,// jmp near ptr 000000000000800Dh
		/*000B*/ 0xB0, 0x02,// mov al,2
	];
	#[rustfmt::skip]
	let new_data = [
		/*0000*/ 0xB0, 0x00,// mov al,0
		/*0002*/ 0xE9, 0x06, 0xF0, 0xFF, 0xFF,// jmp near ptr 000000000000800Dh
		/*0007*/ 0xB0, 0x01,// mov al,1
		/*0009*/ 0xE9, 0xFF, 0xEF, 0xFF, 0xFF,// jmp near ptr 000000000000800Dh
		/*000E*/ 0xB0, 0x02,// mov al,2
	];
	#[rustfmt::skip]
	let expected_instruction_offsets = [
		0x0000,
		0x0002,
		0x0007,
		0x0009,
		0x000E,
	];
	let expected_reloc_infos = [];
	const OPTIONS: u32 = BlockEncoderOptions::NONE;
	encode_test(
		BITNESS,
		ORIG_RIP,
		&original_data,
		ORIG_RIP + 0x1000,
		&new_data,
		OPTIONS,
		DECODER_OPTIONS,
		&expected_instruction_offsets,
		&expected_reloc_infos,
	);
}

#[test]
fn jmp_other_long() {
	#[rustfmt::skip]
	let original_data = [
		/*0000*/ 0xB0, 0x00,// mov al,0
		/*0002*/ 0xEB, 0x09,// jmp short 123456789ABCDE0Dh
		/*0004*/ 0xB0, 0x01,// mov al,1
		/*0006*/ 0xE9, 0x03, 0x00, 0x00, 0x00,// jmp near ptr 123456789ABCDE0Eh
		/*000B*/ 0xB0, 0x02,// mov al,2
	];
	#[rustfmt::skip]
	let new_data = [
		/*0000*/ 0xB0, 0x00,// mov al,0
		/*0002*/ 0xFF, 0x25, 0x10, 0x00, 0x00, 0x00,// jmp qword ptr [8000000000000018h]
		/*0008*/ 0xB0, 0x01,// mov al,1
		/*000A*/ 0xFF, 0x25, 0x10, 0x00, 0x00, 0x00,// jmp qword ptr [8000000000000020h]
		/*0010*/ 0xB0, 0x02,// mov al,2
		/*0012*/ 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
		/*0018*/ 0x0D, 0xDE, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12,
		/*0020*/ 0x0E, 0xDE, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12,
	];
	#[rustfmt::skip]
	let expected_instruction_offsets = [
		0x0000,
		u32::MAX,
		0x0008,
		u32::MAX,
		0x0010,
	];
	#[rustfmt::skip]
	let expected_reloc_infos = [
		RelocInfo::new(RelocKind::Offset64, 0x8000_0000_0000_0018),
		RelocInfo::new(RelocKind::Offset64, 0x8000_0000_0000_0020),
	];
	const OPTIONS: u32 = BlockEncoderOptions::NONE;
	const ORIG_RIP: u64 = 0x1234_5678_9ABC_DE00;
	encode_test(
		BITNESS,
		ORIG_RIP,
		&original_data,
		NEW_RIP,
		&new_data,
		OPTIONS,
		DECODER_OPTIONS,
		&expected_instruction_offsets,
		&expected_reloc_infos,
	);
}

#[test]
fn jmp_fwd_no_opt() {
	#[rustfmt::skip]
	let original_data = [
		/*0000*/ 0xB0, 0x00,// mov al,0
		/*0002*/ 0xEB, 0x09,// jmp short 000000000000800Dh
		/*0004*/ 0xB0, 0x01,// mov al,1
		/*0006*/ 0xE9, 0x02, 0x00, 0x00, 0x00,// jmp near ptr 000000000000800Dh
		/*000B*/ 0xB0, 0x02,// mov al,2
		/*000D*/ 0x90,// nop
	];
	#[rustfmt::skip]
	let new_data = [
		/*0000*/ 0xB0, 0x00,// mov al,0
		/*0002*/ 0xEB, 0x09,// jmp short 000000000000800Dh
		/*0004*/ 0xB0, 0x01,// mov al,1
		/*0006*/ 0xE9, 0x02, 0x00, 0x00, 0x00,// jmp near ptr 000000000000800Dh
		/*000B*/ 0xB0, 0x02,// mov al,2
		/*000D*/ 0x90,// nop
	];
	#[rustfmt::skip]
	let expected_instruction_offsets = [
		0x0000,
		0x0002,
		0x0004,
		0x0006,
		0x000B,
		0x000D,
	];
	let expected_reloc_infos = [];
	const OPTIONS: u32 = BlockEncoderOptions::DONT_FIX_BRANCHES;
	encode_test(
		BITNESS,
		ORIG_RIP,
		&original_data,
		NEW_RIP,
		&new_data,
		OPTIONS,
		DECODER_OPTIONS,
		&expected_instruction_offsets,
		&expected_reloc_infos,
	);
}
