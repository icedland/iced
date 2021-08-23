// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::block_enc::tests::*;

const BITNESS: u32 = 16;
const ORIG_RIP: u64 = 0x8000;
const NEW_RIP: u64 = 0xF000;

#[test]
fn br8_fwd() {
	#[rustfmt::skip]
	let original_data = [
		/*0000*/ 0xB0, 0x00,// mov al,0
		/*0002*/ 0xE2, 0x22,// loop 8026h
		/*0004*/ 0xB0, 0x01,// mov al,1
		/*0006*/ 0x67, 0xE2, 0x1D,// loop 8026h
		/*0009*/ 0xB0, 0x02,// mov al,2
		/*000B*/ 0xE1, 0x19,// loope 8026h
		/*000D*/ 0xB0, 0x03,// mov al,3
		/*000F*/ 0x67, 0xE1, 0x14,// loope 8026h
		/*0012*/ 0xB0, 0x04,// mov al,4
		/*0014*/ 0xE0, 0x10,// loopne 8026h
		/*0016*/ 0xB0, 0x05,// mov al,5
		/*0018*/ 0x67, 0xE0, 0x0B,// loopne 8026h
		/*001B*/ 0xB0, 0x06,// mov al,6
		/*001D*/ 0x67, 0xE3, 0x06,// jecxz 8026h
		/*0020*/ 0xB0, 0x07,// mov al,7
		/*0022*/ 0xE3, 0x02,// jcxz 8026h
		/*0024*/ 0xB0, 0x08,// mov al,8
		/*0026*/ 0x90,// nop
	];
	#[rustfmt::skip]
	let new_data = [
		/*0000*/ 0xB0, 0x00,// mov al,0
		/*0002*/ 0xE2, 0x22,// loop 0F026h
		/*0004*/ 0xB0, 0x01,// mov al,1
		/*0006*/ 0x67, 0xE2, 0x1D,// loop 0F026h
		/*0009*/ 0xB0, 0x02,// mov al,2
		/*000B*/ 0xE1, 0x19,// loope 0F026h
		/*000D*/ 0xB0, 0x03,// mov al,3
		/*000F*/ 0x67, 0xE1, 0x14,// loope 0F026h
		/*0012*/ 0xB0, 0x04,// mov al,4
		/*0014*/ 0xE0, 0x10,// loopne 0F026h
		/*0016*/ 0xB0, 0x05,// mov al,5
		/*0018*/ 0x67, 0xE0, 0x0B,// loopne 0F026h
		/*001B*/ 0xB0, 0x06,// mov al,6
		/*001D*/ 0x67, 0xE3, 0x06,// jecxz 0F026h
		/*0020*/ 0xB0, 0x07,// mov al,7
		/*0022*/ 0xE3, 0x02,// jcxz 0F026h
		/*0024*/ 0xB0, 0x08,// mov al,8
		/*0026*/ 0x90,// nop
	];
	#[rustfmt::skip]
	let expected_instruction_offsets = [
		0x0000,
		0x0002,
		0x0004,
		0x0006,
		0x0009,
		0x000B,
		0x000D,
		0x000F,
		0x0012,
		0x0014,
		0x0016,
		0x0018,
		0x001B,
		0x001D,
		0x0020,
		0x0022,
		0x0024,
		0x0026,
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
fn br8_bwd() {
	#[rustfmt::skip]
	let original_data = [
		/*0000*/ 0x90,// nop
		/*0001*/ 0xB0, 0x00,// mov al,0
		/*0003*/ 0xE2, 0xFB,// loop 8000h
		/*0005*/ 0xB0, 0x01,// mov al,1
		/*0007*/ 0x67, 0xE2, 0xF6,// loop 8000h
		/*000A*/ 0xB0, 0x02,// mov al,2
		/*000C*/ 0xE1, 0xF2,// loope 8000h
		/*000E*/ 0xB0, 0x03,// mov al,3
		/*0010*/ 0x67, 0xE1, 0xED,// loope 8000h
		/*0013*/ 0xB0, 0x04,// mov al,4
		/*0015*/ 0xE0, 0xE9,// loopne 8000h
		/*0017*/ 0xB0, 0x05,// mov al,5
		/*0019*/ 0x67, 0xE0, 0xE4,// loopne 8000h
		/*001C*/ 0xB0, 0x06,// mov al,6
		/*001E*/ 0x67, 0xE3, 0xDF,// jecxz 8000h
		/*0021*/ 0xB0, 0x07,// mov al,7
		/*0023*/ 0xE3, 0xDB,// jcxz 8000h
		/*0025*/ 0xB0, 0x08,// mov al,8
	];
	#[rustfmt::skip]
	let new_data = [
		/*0000*/ 0x90,// nop
		/*0001*/ 0xB0, 0x00,// mov al,0
		/*0003*/ 0xE2, 0xFB,// loop 0F000h
		/*0005*/ 0xB0, 0x01,// mov al,1
		/*0007*/ 0x67, 0xE2, 0xF6,// loop 0F000h
		/*000A*/ 0xB0, 0x02,// mov al,2
		/*000C*/ 0xE1, 0xF2,// loope 0F000h
		/*000E*/ 0xB0, 0x03,// mov al,3
		/*0010*/ 0x67, 0xE1, 0xED,// loope 0F000h
		/*0013*/ 0xB0, 0x04,// mov al,4
		/*0015*/ 0xE0, 0xE9,// loopne 0F000h
		/*0017*/ 0xB0, 0x05,// mov al,5
		/*0019*/ 0x67, 0xE0, 0xE4,// loopne 0F000h
		/*001C*/ 0xB0, 0x06,// mov al,6
		/*001E*/ 0x67, 0xE3, 0xDF,// jecxz 0F000h
		/*0021*/ 0xB0, 0x07,// mov al,7
		/*0023*/ 0xE3, 0xDB,// jcxz 0F000h
		/*0025*/ 0xB0, 0x08,// mov al,8
	];
	#[rustfmt::skip]
	let expected_instruction_offsets = [
		0x0000,
		0x0001,
		0x0003,
		0x0005,
		0x0007,
		0x000A,
		0x000C,
		0x000E,
		0x0010,
		0x0013,
		0x0015,
		0x0017,
		0x0019,
		0x001C,
		0x001E,
		0x0021,
		0x0023,
		0x0025,
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
fn br8_fwd_os() {
	#[rustfmt::skip]
	let original_data = [
		/*0000*/ 0xB0, 0x00,// mov al,0
		/*0002*/ 0x66, 0xE2, 0x29,// loopd 0000802Eh
		/*0005*/ 0xB0, 0x01,// mov al,1
		/*0007*/ 0x66, 0x67, 0xE2, 0x23,// loopd 0000802Eh
		/*000B*/ 0xB0, 0x02,// mov al,2
		/*000D*/ 0x66, 0xE1, 0x1E,// looped 0000802Eh
		/*0010*/ 0xB0, 0x03,// mov al,3
		/*0012*/ 0x66, 0x67, 0xE1, 0x18,// looped 0000802Eh
		/*0016*/ 0xB0, 0x04,// mov al,4
		/*0018*/ 0x66, 0xE0, 0x13,// loopned 0000802Eh
		/*001B*/ 0xB0, 0x05,// mov al,5
		/*001D*/ 0x66, 0x67, 0xE0, 0x0D,// loopned 0000802Eh
		/*0021*/ 0xB0, 0x06,// mov al,6
		/*0023*/ 0x66, 0x67, 0xE3, 0x07,// jecxz 0000802Eh
		/*0027*/ 0xB0, 0x07,// mov al,7
		/*0029*/ 0x66, 0xE3, 0x02,// jcxz 0000802Eh
		/*002C*/ 0xB0, 0x08,// mov al,8
		/*002E*/ 0x90,// nop
	];
	#[rustfmt::skip]
	let new_data = [
		/*0000*/ 0xB0, 0x00,// mov al,0
		/*0002*/ 0x66, 0xE2, 0x29,// loopd 0000802Dh
		/*0005*/ 0xB0, 0x01,// mov al,1
		/*0007*/ 0x66, 0x67, 0xE2, 0x23,// loopd 0000802Dh
		/*000B*/ 0xB0, 0x02,// mov al,2
		/*000D*/ 0x66, 0xE1, 0x1E,// looped 0000802Dh
		/*0010*/ 0xB0, 0x03,// mov al,3
		/*0012*/ 0x66, 0x67, 0xE1, 0x18,// looped 0000802Dh
		/*0016*/ 0xB0, 0x04,// mov al,4
		/*0018*/ 0x66, 0xE0, 0x13,// loopned 0000802Dh
		/*001B*/ 0xB0, 0x05,// mov al,5
		/*001D*/ 0x66, 0x67, 0xE0, 0x0D,// loopned 0000802Dh
		/*0021*/ 0xB0, 0x06,// mov al,6
		/*0023*/ 0x66, 0x67, 0xE3, 0x07,// jecxz 0000802Dh
		/*0027*/ 0xB0, 0x07,// mov al,7
		/*0029*/ 0x66, 0xE3, 0x02,// jcxz 0000802Dh
		/*002C*/ 0xB0, 0x08,// mov al,8
		/*002E*/ 0x90,// nop
	];
	#[rustfmt::skip]
	let expected_instruction_offsets = [
		0x0000,
		0x0002,
		0x0005,
		0x0007,
		0x000B,
		0x000D,
		0x0010,
		0x0012,
		0x0016,
		0x0018,
		0x001B,
		0x001D,
		0x0021,
		0x0023,
		0x0027,
		0x0029,
		0x002C,
		0x002E,
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
fn br8_short_other_short() {
	#[rustfmt::skip]
	let original_data = [
		/*0000*/ 0xB0, 0x00,// mov al,0
		/*0002*/ 0xE2, 0x22,// loop 8026h
		/*0004*/ 0xB0, 0x01,// mov al,1
		/*0006*/ 0x67, 0xE2, 0x1D,// loop 8026h
		/*0009*/ 0xB0, 0x02,// mov al,2
		/*000B*/ 0xE1, 0x19,// loope 8026h
		/*000D*/ 0xB0, 0x03,// mov al,3
		/*000F*/ 0x67, 0xE1, 0x14,// loope 8026h
		/*0012*/ 0xB0, 0x04,// mov al,4
		/*0014*/ 0xE0, 0x10,// loopne 8026h
		/*0016*/ 0xB0, 0x05,// mov al,5
		/*0018*/ 0x67, 0xE0, 0x0B,// loopne 8026h
		/*001B*/ 0xB0, 0x06,// mov al,6
		/*001D*/ 0x67, 0xE3, 0x06,// jecxz 8026h
		/*0020*/ 0xB0, 0x07,// mov al,7
		/*0022*/ 0xE3, 0x02,// jcxz 8026h
		/*0024*/ 0xB0, 0x08,// mov al,8
	];
	#[rustfmt::skip]
	let new_data = [
		/*0000*/ 0xB0, 0x00,// mov al,0
		/*0002*/ 0xE2, 0x23,// loop 8026h
		/*0004*/ 0xB0, 0x01,// mov al,1
		/*0006*/ 0x67, 0xE2, 0x1E,// loop 8026h
		/*0009*/ 0xB0, 0x02,// mov al,2
		/*000B*/ 0xE1, 0x1A,// loope 8026h
		/*000D*/ 0xB0, 0x03,// mov al,3
		/*000F*/ 0x67, 0xE1, 0x15,// loope 8026h
		/*0012*/ 0xB0, 0x04,// mov al,4
		/*0014*/ 0xE0, 0x11,// loopne 8026h
		/*0016*/ 0xB0, 0x05,// mov al,5
		/*0018*/ 0x67, 0xE0, 0x0C,// loopne 8026h
		/*001B*/ 0xB0, 0x06,// mov al,6
		/*001D*/ 0x67, 0xE3, 0x07,// jecxz 8026h
		/*0020*/ 0xB0, 0x07,// mov al,7
		/*0022*/ 0xE3, 0x03,// jcxz 8026h
		/*0024*/ 0xB0, 0x08,// mov al,8
	];
	#[rustfmt::skip]
	let expected_instruction_offsets = [
		0x0000,
		0x0002,
		0x0004,
		0x0006,
		0x0009,
		0x000B,
		0x000D,
		0x000F,
		0x0012,
		0x0014,
		0x0016,
		0x0018,
		0x001B,
		0x001D,
		0x0020,
		0x0022,
		0x0024,
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
fn br8_short_other_near() {
	#[rustfmt::skip]
	let original_data = [
		/*0000*/ 0xB0, 0x00,// mov al,0
		/*0002*/ 0xE2, 0x22,// loop 8026h
		/*0004*/ 0xB0, 0x01,// mov al,1
		/*0006*/ 0x67, 0xE2, 0x1E,// loop 8027h
		/*0009*/ 0xB0, 0x02,// mov al,2
		/*000B*/ 0xE1, 0x1B,// loope 8028h
		/*000D*/ 0xB0, 0x03,// mov al,3
		/*000F*/ 0x67, 0xE1, 0x17,// loope 8029h
		/*0012*/ 0xB0, 0x04,// mov al,4
		/*0014*/ 0xE0, 0x14,// loopne 802Ah
		/*0016*/ 0xB0, 0x05,// mov al,5
		/*0018*/ 0x67, 0xE0, 0x10,// loopne 802Bh
		/*001B*/ 0xB0, 0x06,// mov al,6
		/*001D*/ 0x67, 0xE3, 0x0C,// jecxz 802Ch
		/*0020*/ 0xB0, 0x07,// mov al,7
		/*0022*/ 0xE3, 0x09,// jcxz 802Dh
		/*0024*/ 0xB0, 0x08,// mov al,8
	];
	#[rustfmt::skip]
	let new_data = [
		/*0000*/ 0xB0, 0x00,// mov al,0
		/*0002*/ 0xE2, 0x02,// loop 9006h
		/*0004*/ 0xEB, 0x03,// jmp short 9009h
		/*0006*/ 0xE9, 0x1D, 0xF0,// jmp near ptr 8026h
		/*0009*/ 0xB0, 0x01,// mov al,1
		/*000B*/ 0x67, 0xE2, 0x02,// loop 9010h
		/*000E*/ 0xEB, 0x03,// jmp short 9013h
		/*0010*/ 0xE9, 0x14, 0xF0,// jmp near ptr 8027h
		/*0013*/ 0xB0, 0x02,// mov al,2
		/*0015*/ 0xE1, 0x02,// loope 9019h
		/*0017*/ 0xEB, 0x03,// jmp short 901Ch
		/*0019*/ 0xE9, 0x0C, 0xF0,// jmp near ptr 8028h
		/*001C*/ 0xB0, 0x03,// mov al,3
		/*001E*/ 0x67, 0xE1, 0x02,// loope 9023h
		/*0021*/ 0xEB, 0x03,// jmp short 9026h
		/*0023*/ 0xE9, 0x03, 0xF0,// jmp near ptr 8029h
		/*0026*/ 0xB0, 0x04,// mov al,4
		/*0028*/ 0xE0, 0x02,// loopne 902Ch
		/*002A*/ 0xEB, 0x03,// jmp short 902Fh
		/*002C*/ 0xE9, 0xFB, 0xEF,// jmp near ptr 802Ah
		/*002F*/ 0xB0, 0x05,// mov al,5
		/*0031*/ 0x67, 0xE0, 0x02,// loopne 9036h
		/*0034*/ 0xEB, 0x03,// jmp short 9039h
		/*0036*/ 0xE9, 0xF2, 0xEF,// jmp near ptr 802Bh
		/*0039*/ 0xB0, 0x06,// mov al,6
		/*003B*/ 0x67, 0xE3, 0x02,// jecxz 9040h
		/*003E*/ 0xEB, 0x03,// jmp short 9043h
		/*0040*/ 0xE9, 0xE9, 0xEF,// jmp near ptr 802Ch
		/*0043*/ 0xB0, 0x07,// mov al,7
		/*0045*/ 0xE3, 0x02,// jcxz 9049h
		/*0047*/ 0xEB, 0x03,// jmp short 904Ch
		/*0049*/ 0xE9, 0xE1, 0xEF,// jmp near ptr 802Dh
		/*004C*/ 0xB0, 0x08,// mov al,8
	];
	#[rustfmt::skip]
	let expected_instruction_offsets = [
		0x0000,
		u32::MAX,
		0x0009,
		u32::MAX,
		0x0013,
		u32::MAX,
		0x001C,
		u32::MAX,
		0x0026,
		u32::MAX,
		0x002F,
		u32::MAX,
		0x0039,
		u32::MAX,
		0x0043,
		u32::MAX,
		0x004C,
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
fn br8_same_br() {
	#[rustfmt::skip]
	let original_data = [
		/*0000*/ 0xE2, 0xFE,// loop 8000h
		/*0002*/ 0xE2, 0xFC,// loop 8000h
		/*0004*/ 0xE2, 0xFA,// loop 8000h
	];
	#[rustfmt::skip]
	let new_data = [
		/*0000*/ 0xE2, 0xFE,// loop 8000h
		/*0002*/ 0xE2, 0xFC,// loop 8000h
		/*0004*/ 0xE2, 0xFA,// loop 8000h
	];
	#[rustfmt::skip]
	let expected_instruction_offsets = [
		0x0000,
		0x0002,
		0x0004,
	];
	let expected_reloc_infos = [];
	const OPTIONS: u32 = BlockEncoderOptions::NONE;
	encode_test(
		BITNESS,
		ORIG_RIP,
		&original_data,
		ORIG_RIP,
		&new_data,
		OPTIONS,
		DECODER_OPTIONS,
		&expected_instruction_offsets,
		&expected_reloc_infos,
	);
}
