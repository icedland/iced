// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::block_enc::tests::*;

const BITNESS: u32 = 16;
const ORIG_RIP: u64 = 0x8000;
const NEW_RIP: u64 = 0xF000;

#[test]
fn call_near_fwd() {
	#[rustfmt::skip]
	let original_data = [
		/*0000*/ 0xE8, 0x08, 0x00,// call 800Bh
		/*0003*/ 0xB0, 0x00,// mov al,0
		/*0005*/ 0x66, 0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
		/*000B*/ 0x90,// nop
	];
	#[rustfmt::skip]
	let new_data = [
		/*0000*/ 0xE8, 0x08, 0x00,// call 0F00Bh
		/*0003*/ 0xB0, 0x00,// mov al,0
		/*0005*/ 0x66, 0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
		/*000B*/ 0x90,// nop
	];
	#[rustfmt::skip]
	let expected_instruction_offsets = [
		0x0000,
		0x0003,
		0x0005,
		0x000B,
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
		/*0001*/ 0xE8, 0xFC, 0xFF,// call 8000h
		/*0004*/ 0xB0, 0x00,// mov al,0
		/*0006*/ 0x66, 0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
	];
	#[rustfmt::skip]
	let new_data = [
		/*0000*/ 0x90,// nop
		/*0001*/ 0xE8, 0xFC, 0xFF,// call 0F000h
		/*0004*/ 0xB0, 0x00,// mov al,0
		/*0006*/ 0x66, 0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
	];
	#[rustfmt::skip]
	let expected_instruction_offsets = [
		0x0000,
		0x0001,
		0x0004,
		0x0006,
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
		/*0000*/ 0xE8, 0x08, 0x00,// call 800Bh
		/*0003*/ 0xB0, 0x00,// mov al,0
		/*0005*/ 0x66, 0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
	];
	#[rustfmt::skip]
	let new_data = [
		/*0000*/ 0xE8, 0x09, 0x00,// call 800Bh
		/*0003*/ 0xB0, 0x00,// mov al,0
		/*0005*/ 0x66, 0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
	];
	#[rustfmt::skip]
	let expected_instruction_offsets = [
		0x0000,
		0x0003,
		0x0005,
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
		/*0000*/ 0x66, 0xE8, 0x08, 0x00, 0x00, 0x00,// call 0000800Eh
		/*0006*/ 0xB0, 0x00,// mov al,0
		/*0008*/ 0x66, 0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
	];
	#[rustfmt::skip]
	let new_data = [
		/*0000*/ 0x66, 0xE8, 0x08, 0x90, 0xFF, 0xFF,// call 0000800Eh
		/*0006*/ 0xB0, 0x00,// mov al,0
		/*0008*/ 0x66, 0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
	];
	#[rustfmt::skip]
	let expected_instruction_offsets = [
		0x0000,
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
