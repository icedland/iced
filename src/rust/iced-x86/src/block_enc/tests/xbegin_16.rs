// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::block_enc::tests::*;

const BITNESS: u32 = 16;
const ORIG_RIP: u64 = 0x8000;
const NEW_RIP: u64 = 0xF000;

#[test]
fn xbegin_fwd_rel16() {
	#[rustfmt::skip]
	let original_data = [
		/*0000*/ 0xB0, 0x00,// mov al,0
		/*0002*/ 0x66, 0xC7, 0xF8, 0x06, 0x00, 0x00, 0x00,// xbegin 0000800Fh
		/*0009*/ 0xB0, 0x01,// mov al,1
		/*000B*/ 0xC7, 0xF8, 0x02, 0x00,// xbegin 00008011h
		/*000F*/ 0xB0, 0x02,// mov al,2
		/*0011*/ 0xB0, 0x03,// mov al,3
	];
	#[rustfmt::skip]
	let new_data = [
		/*0000*/ 0xB0, 0x00,// mov al,0
		/*0002*/ 0xC7, 0xF8, 0x06, 0x00,// xbegin 0000F00Ch
		/*0006*/ 0xB0, 0x01,// mov al,1
		/*0008*/ 0xC7, 0xF8, 0x02, 0x00,// xbegin 0000F00Eh
		/*000C*/ 0xB0, 0x02,// mov al,2
		/*000E*/ 0xB0, 0x03,// mov al,3
	];
	#[rustfmt::skip]
	let expected_instruction_offsets = [
		0x0000,
		0x0002,
		0x0006,
		0x0008,
		0x000C,
		0x000E,
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
fn xbegin_bwd_rel16() {
	#[rustfmt::skip]
	let original_data = [
		/*0000*/ 0xB0, 0x02,// mov al,2
		/*0002*/ 0xB0, 0x03,// mov al,3
		/*0004*/ 0xB0, 0x00,// mov al,0
		/*0006*/ 0x66, 0xC7, 0xF8, 0xF3, 0xFF, 0xFF, 0xFF,// xbegin 00008000h
		/*000D*/ 0xB0, 0x01,// mov al,1
		/*000F*/ 0xC7, 0xF8, 0xEF, 0xFF,// xbegin 00008002h
	];
	#[rustfmt::skip]
	let new_data = [
		/*0000*/ 0xB0, 0x02,// mov al,2
		/*0002*/ 0xB0, 0x03,// mov al,3
		/*0004*/ 0xB0, 0x00,// mov al,0
		/*0006*/ 0xC7, 0xF8, 0xF6, 0xFF,// xbegin 0000F000h
		/*000A*/ 0xB0, 0x01,// mov al,1
		/*000C*/ 0xC7, 0xF8, 0xF2, 0xFF,// xbegin 0000F002h
	];
	#[rustfmt::skip]
	let expected_instruction_offsets = [
		0x0000,
		0x0002,
		0x0004,
		0x0006,
		0x000A,
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
