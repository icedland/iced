// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::block_enc::tests::*;

const BITNESS: u32 = 64;
const ORIG_RIP: u64 = 0x8000;
const NEW_RIP: u64 = 0x8000_0000_0000_0000;

#[test]
fn call_near_fwd() {
	#[rustfmt::skip]
	let original_data = [
		/*0000*/ 0xE8, 0x07, 0x00, 0x00, 0x00,// call 000000000000800Ch
		/*0005*/ 0xB0, 0x00,// mov al,0
		/*0007*/ 0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
		/*000C*/ 0x90,// nop
	];
	#[rustfmt::skip]
	let new_data = [
		/*0000*/ 0xE8, 0x07, 0x00, 0x00, 0x00,// call 800000000000000Ch
		/*0005*/ 0xB0, 0x00,// mov al,0
		/*0007*/ 0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
		/*000C*/ 0x90,// nop
	];
	#[rustfmt::skip]
	let expected_instruction_offsets = [
		0x0000,
		0x0005,
		0x0007,
		0x000C,
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
fn call_near_bwd() {
	#[rustfmt::skip]
	let original_data = [
		/*0000*/ 0x90,// nop
		/*0001*/ 0xE8, 0xFA, 0xFF, 0xFF, 0xFF,// call 0000000000008000h
		/*0006*/ 0xB0, 0x00,// mov al,0
		/*0008*/ 0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
	];
	#[rustfmt::skip]
	let new_data = [
		/*0000*/ 0x90,// nop
		/*0001*/ 0xE8, 0xFA, 0xFF, 0xFF, 0xFF,// call 8000000000000000h
		/*0006*/ 0xB0, 0x00,// mov al,0
		/*0008*/ 0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
	];
	#[rustfmt::skip]
	let expected_instruction_offsets = [
		0x0000,
		0x0001,
		0x0006,
		0x0008,
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
fn call_near_other_near() {
	#[rustfmt::skip]
	let original_data = [
		/*0000*/ 0xE8, 0x07, 0x00, 0x00, 0x00,// call 000000000000800Ch
		/*0005*/ 0xB0, 0x00,// mov al,0
		/*0007*/ 0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
	];
	#[rustfmt::skip]
	let new_data = [
		/*0000*/ 0xE8, 0x08, 0x00, 0x00, 0x00,// call 000000000000800Ch
		/*0005*/ 0xB0, 0x00,// mov al,0
		/*0007*/ 0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
	];
	#[rustfmt::skip]
	let expected_instruction_offsets = [
		0x0000,
		0x0005,
		0x0007,
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
fn call_near_other_near_os() {
	#[rustfmt::skip]
	let original_data = [
		/*0000*/ 0x66, 0xE8, 0x07, 0x00,// call 800Bh
		/*0004*/ 0xB0, 0x00,// mov al,0
		/*0006*/ 0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
	];
	#[rustfmt::skip]
	let new_data = [
		/*0000*/ 0x66, 0xE8, 0x08, 0x00,// call 800Bh
		/*0004*/ 0xB0, 0x00,// mov al,0
		/*0006*/ 0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
	];
	#[rustfmt::skip]
	let expected_instruction_offsets = [
		0x0000,
		0x0004,
		0x0006,
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
fn call_near_other_long() {
	#[rustfmt::skip]
	let original_data = [
		/*0000*/ 0xE8, 0x07, 0x00, 0x00, 0x00,// call 123456789ABCDE0Ch
		/*0005*/ 0xB0, 0x00,// mov al,0
		/*0007*/ 0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
	];
	#[rustfmt::skip]
	let new_data = [
		/*0000*/ 0xFF, 0x15, 0x0A, 0x00, 0x00, 0x00,// call qword ptr [8000000000000010h]
		/*0006*/ 0xB0, 0x00,// mov al,0
		/*0008*/ 0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
		/*000D*/ 0xCC, 0xCC, 0xCC,
		/*0010*/ 0x0C, 0xDE, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12,
	];
	#[rustfmt::skip]
	let expected_instruction_offsets = [
		u32::MAX,
		0x0006,
		0x0008,
	];
	#[rustfmt::skip]
	let expected_reloc_infos = [
		RelocInfo::new(RelocKind::Offset64, 0x8000_0000_0000_0010),
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
