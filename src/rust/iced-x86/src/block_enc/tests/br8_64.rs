// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::block_enc::tests::*;

const BITNESS: u32 = 64;
const ORIG_RIP: u64 = 0x8000;
const NEW_RIP: u64 = 0x8000_0000_0000_0000;

#[test]
fn br8_fwd() {
	#[rustfmt::skip]
	let original_data = [
		/*0000*/ 0xB0, 0x00,// mov al,0
		/*0002*/ 0xE2, 0x22,// loop 0000000000008026h
		/*0004*/ 0xB0, 0x01,// mov al,1
		/*0006*/ 0x67, 0xE2, 0x1D,// loopd 0000000000008026h
		/*0009*/ 0xB0, 0x02,// mov al,2
		/*000B*/ 0xE1, 0x19,// loope 0000000000008026h
		/*000D*/ 0xB0, 0x03,// mov al,3
		/*000F*/ 0x67, 0xE1, 0x14,// looped 0000000000008026h
		/*0012*/ 0xB0, 0x04,// mov al,4
		/*0014*/ 0xE0, 0x10,// loopne 0000000000008026h
		/*0016*/ 0xB0, 0x05,// mov al,5
		/*0018*/ 0x67, 0xE0, 0x0B,// loopned 0000000000008026h
		/*001B*/ 0xB0, 0x06,// mov al,6
		/*001D*/ 0x67, 0xE3, 0x06,// jecxz 0000000000008026h
		/*0020*/ 0xB0, 0x07,// mov al,7
		/*0022*/ 0xE3, 0x02,// jrcxz 0000000000008026h
		/*0024*/ 0xB0, 0x08,// mov al,8
		/*0026*/ 0x90,// nop
	];
	#[rustfmt::skip]
	let new_data = [
		/*0000*/ 0xB0, 0x00,// mov al,0
		/*0002*/ 0xE2, 0x22,// loop 8000000000000026h
		/*0004*/ 0xB0, 0x01,// mov al,1
		/*0006*/ 0x67, 0xE2, 0x1D,// loopd 8000000000000026h
		/*0009*/ 0xB0, 0x02,// mov al,2
		/*000B*/ 0xE1, 0x19,// loope 8000000000000026h
		/*000D*/ 0xB0, 0x03,// mov al,3
		/*000F*/ 0x67, 0xE1, 0x14,// looped 8000000000000026h
		/*0012*/ 0xB0, 0x04,// mov al,4
		/*0014*/ 0xE0, 0x10,// loopne 8000000000000026h
		/*0016*/ 0xB0, 0x05,// mov al,5
		/*0018*/ 0x67, 0xE0, 0x0B,// loopned 8000000000000026h
		/*001B*/ 0xB0, 0x06,// mov al,6
		/*001D*/ 0x67, 0xE3, 0x06,// jecxz 8000000000000026h
		/*0020*/ 0xB0, 0x07,// mov al,7
		/*0022*/ 0xE3, 0x02,// jrcxz 8000000000000026h
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
		/*0003*/ 0xE2, 0xFB,// loop 0000000000008000h
		/*0005*/ 0xB0, 0x01,// mov al,1
		/*0007*/ 0x67, 0xE2, 0xF6,// loopd 0000000000008000h
		/*000A*/ 0xB0, 0x02,// mov al,2
		/*000C*/ 0xE1, 0xF2,// loope 0000000000008000h
		/*000E*/ 0xB0, 0x03,// mov al,3
		/*0010*/ 0x67, 0xE1, 0xED,// looped 0000000000008000h
		/*0013*/ 0xB0, 0x04,// mov al,4
		/*0015*/ 0xE0, 0xE9,// loopne 0000000000008000h
		/*0017*/ 0xB0, 0x05,// mov al,5
		/*0019*/ 0x67, 0xE0, 0xE4,// loopned 0000000000008000h
		/*001C*/ 0xB0, 0x06,// mov al,6
		/*001E*/ 0x67, 0xE3, 0xDF,// jecxz 0000000000008000h
		/*0021*/ 0xB0, 0x07,// mov al,7
		/*0023*/ 0xE3, 0xDB,// jrcxz 0000000000008000h
		/*0025*/ 0xB0, 0x08,// mov al,8
	];
	#[rustfmt::skip]
	let new_data = [
		/*0000*/ 0x90,// nop
		/*0001*/ 0xB0, 0x00,// mov al,0
		/*0003*/ 0xE2, 0xFB,// loop 8000000000000000h
		/*0005*/ 0xB0, 0x01,// mov al,1
		/*0007*/ 0x67, 0xE2, 0xF6,// loopd 8000000000000000h
		/*000A*/ 0xB0, 0x02,// mov al,2
		/*000C*/ 0xE1, 0xF2,// loope 8000000000000000h
		/*000E*/ 0xB0, 0x03,// mov al,3
		/*0010*/ 0x67, 0xE1, 0xED,// looped 8000000000000000h
		/*0013*/ 0xB0, 0x04,// mov al,4
		/*0015*/ 0xE0, 0xE9,// loopne 8000000000000000h
		/*0017*/ 0xB0, 0x05,// mov al,5
		/*0019*/ 0x67, 0xE0, 0xE4,// loopned 8000000000000000h
		/*001C*/ 0xB0, 0x06,// mov al,6
		/*001E*/ 0x67, 0xE3, 0xDF,// jecxz 8000000000000000h
		/*0021*/ 0xB0, 0x07,// mov al,7
		/*0023*/ 0xE3, 0xDB,// jrcxz 8000000000000000h
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
		/*0002*/ 0x66, 0xE2, 0x29,// loopw 802Eh
		/*0005*/ 0xB0, 0x01,// mov al,1
		/*0007*/ 0x66, 0x67, 0xE2, 0x23,// loopw 802Eh
		/*000B*/ 0xB0, 0x02,// mov al,2
		/*000D*/ 0x66, 0xE1, 0x1E,// loopew 802Eh
		/*0010*/ 0xB0, 0x03,// mov al,3
		/*0012*/ 0x66, 0x67, 0xE1, 0x18,// loopew 802Eh
		/*0016*/ 0xB0, 0x04,// mov al,4
		/*0018*/ 0x66, 0xE0, 0x13,// loopnew 802Eh
		/*001B*/ 0xB0, 0x05,// mov al,5
		/*001D*/ 0x66, 0x67, 0xE0, 0x0D,// loopnew 802Eh
		/*0021*/ 0xB0, 0x06,// mov al,6
		/*0023*/ 0x66, 0x67, 0xE3, 0x07,// jcxz 802Eh
		/*0027*/ 0xB0, 0x07,// mov al,7
		/*0029*/ 0x66, 0xE3, 0x02,// jecxz 802Eh
		/*002C*/ 0xB0, 0x08,// mov al,8
		/*002E*/ 0x90,// nop
	];
	#[rustfmt::skip]
	let new_data = [
		/*0000*/ 0xB0, 0x00,// mov al,0
		/*0002*/ 0x66, 0xE2, 0x29,// loopw 802Dh
		/*0005*/ 0xB0, 0x01,// mov al,1
		/*0007*/ 0x66, 0x67, 0xE2, 0x23,// loopw 802Dh
		/*000B*/ 0xB0, 0x02,// mov al,2
		/*000D*/ 0x66, 0xE1, 0x1E,// loopew 802Dh
		/*0010*/ 0xB0, 0x03,// mov al,3
		/*0012*/ 0x66, 0x67, 0xE1, 0x18,// loopew 802Dh
		/*0016*/ 0xB0, 0x04,// mov al,4
		/*0018*/ 0x66, 0xE0, 0x13,// loopnew 802Dh
		/*001B*/ 0xB0, 0x05,// mov al,5
		/*001D*/ 0x66, 0x67, 0xE0, 0x0D,// loopnew 802Dh
		/*0021*/ 0xB0, 0x06,// mov al,6
		/*0023*/ 0x66, 0x67, 0xE3, 0x07,// jcxz 802Dh
		/*0027*/ 0xB0, 0x07,// mov al,7
		/*0029*/ 0x66, 0xE3, 0x02,// jecxz 802Dh
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
		DECODER_OPTIONS | DecoderOptions::AMD,
		&expected_instruction_offsets,
		&expected_reloc_infos,
	);
}

#[test]
fn br8_short_other_short() {
	#[rustfmt::skip]
	let original_data = [
		/*0000*/ 0xB0, 0x00,// mov al,0
		/*0002*/ 0xE2, 0x22,// loop 0000000000008026h
		/*0004*/ 0xB0, 0x01,// mov al,1
		/*0006*/ 0x67, 0xE2, 0x1D,// loopd 0000000000008026h
		/*0009*/ 0xB0, 0x02,// mov al,2
		/*000B*/ 0xE1, 0x19,// loope 0000000000008026h
		/*000D*/ 0xB0, 0x03,// mov al,3
		/*000F*/ 0x67, 0xE1, 0x14,// looped 0000000000008026h
		/*0012*/ 0xB0, 0x04,// mov al,4
		/*0014*/ 0xE0, 0x10,// loopne 0000000000008026h
		/*0016*/ 0xB0, 0x05,// mov al,5
		/*0018*/ 0x67, 0xE0, 0x0B,// loopned 0000000000008026h
		/*001B*/ 0xB0, 0x06,// mov al,6
		/*001D*/ 0x67, 0xE3, 0x06,// jecxz 0000000000008026h
		/*0020*/ 0xB0, 0x07,// mov al,7
		/*0022*/ 0xE3, 0x02,// jrcxz 0000000000008026h
		/*0024*/ 0xB0, 0x08,// mov al,8
	];
	#[rustfmt::skip]
	let new_data = [
		/*0000*/ 0xB0, 0x00,// mov al,0
		/*0002*/ 0xE2, 0x23,// loop 0000000000008026h
		/*0004*/ 0xB0, 0x01,// mov al,1
		/*0006*/ 0x67, 0xE2, 0x1E,// loopd 0000000000008026h
		/*0009*/ 0xB0, 0x02,// mov al,2
		/*000B*/ 0xE1, 0x1A,// loope 0000000000008026h
		/*000D*/ 0xB0, 0x03,// mov al,3
		/*000F*/ 0x67, 0xE1, 0x15,// looped 0000000000008026h
		/*0012*/ 0xB0, 0x04,// mov al,4
		/*0014*/ 0xE0, 0x11,// loopne 0000000000008026h
		/*0016*/ 0xB0, 0x05,// mov al,5
		/*0018*/ 0x67, 0xE0, 0x0C,// loopned 0000000000008026h
		/*001B*/ 0xB0, 0x06,// mov al,6
		/*001D*/ 0x67, 0xE3, 0x07,// jecxz 0000000000008026h
		/*0020*/ 0xB0, 0x07,// mov al,7
		/*0022*/ 0xE3, 0x03,// jrcxz 0000000000008026h
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
		/*0002*/ 0xE2, 0x22,// loop 0000000000008026h
		/*0004*/ 0xB0, 0x01,// mov al,1
		/*0006*/ 0x67, 0xE2, 0x1E,// loopd 0000000000008027h
		/*0009*/ 0xB0, 0x02,// mov al,2
		/*000B*/ 0xE1, 0x1B,// loope 0000000000008028h
		/*000D*/ 0xB0, 0x03,// mov al,3
		/*000F*/ 0x67, 0xE1, 0x17,// looped 0000000000008029h
		/*0012*/ 0xB0, 0x04,// mov al,4
		/*0014*/ 0xE0, 0x14,// loopne 000000000000802Ah
		/*0016*/ 0xB0, 0x05,// mov al,5
		/*0018*/ 0x67, 0xE0, 0x10,// loopned 000000000000802Bh
		/*001B*/ 0xB0, 0x06,// mov al,6
		/*001D*/ 0x67, 0xE3, 0x0C,// jecxz 000000000000802Ch
		/*0020*/ 0xB0, 0x07,// mov al,7
		/*0022*/ 0xE3, 0x09,// jrcxz 000000000000802Dh
		/*0024*/ 0xB0, 0x08,// mov al,8
	];
	#[rustfmt::skip]
	let new_data = [
		/*0000*/ 0xB0, 0x00,// mov al,0
		/*0002*/ 0xE2, 0x02,// loop 0000000000009006h
		/*0004*/ 0xEB, 0x05,// jmp short 000000000000900Bh
		/*0006*/ 0xE9, 0x1B, 0xF0, 0xFF, 0xFF,// jmp near ptr 0000000000008026h
		/*000B*/ 0xB0, 0x01,// mov al,1
		/*000D*/ 0x67, 0xE2, 0x02,// loopd 0000000000009012h
		/*0010*/ 0xEB, 0x05,// jmp short 0000000000009017h
		/*0012*/ 0xE9, 0x10, 0xF0, 0xFF, 0xFF,// jmp near ptr 0000000000008027h
		/*0017*/ 0xB0, 0x02,// mov al,2
		/*0019*/ 0xE1, 0x02,// loope 000000000000901Dh
		/*001B*/ 0xEB, 0x05,// jmp short 0000000000009022h
		/*001D*/ 0xE9, 0x06, 0xF0, 0xFF, 0xFF,// jmp near ptr 0000000000008028h
		/*0022*/ 0xB0, 0x03,// mov al,3
		/*0024*/ 0x67, 0xE1, 0x02,// looped 0000000000009029h
		/*0027*/ 0xEB, 0x05,// jmp short 000000000000902Eh
		/*0029*/ 0xE9, 0xFB, 0xEF, 0xFF, 0xFF,// jmp near ptr 0000000000008029h
		/*002E*/ 0xB0, 0x04,// mov al,4
		/*0030*/ 0xE0, 0x02,// loopne 0000000000009034h
		/*0032*/ 0xEB, 0x05,// jmp short 0000000000009039h
		/*0034*/ 0xE9, 0xF1, 0xEF, 0xFF, 0xFF,// jmp near ptr 000000000000802Ah
		/*0039*/ 0xB0, 0x05,// mov al,5
		/*003B*/ 0x67, 0xE0, 0x02,// loopned 0000000000009040h
		/*003E*/ 0xEB, 0x05,// jmp short 0000000000009045h
		/*0040*/ 0xE9, 0xE6, 0xEF, 0xFF, 0xFF,// jmp near ptr 000000000000802Bh
		/*0045*/ 0xB0, 0x06,// mov al,6
		/*0047*/ 0x67, 0xE3, 0x02,// jecxz 000000000000904Ch
		/*004A*/ 0xEB, 0x05,// jmp short 0000000000009051h
		/*004C*/ 0xE9, 0xDB, 0xEF, 0xFF, 0xFF,// jmp near ptr 000000000000802Ch
		/*0051*/ 0xB0, 0x07,// mov al,7
		/*0053*/ 0xE3, 0x02,// jrcxz 0000000000009057h
		/*0055*/ 0xEB, 0x05,// jmp short 000000000000905Ch
		/*0057*/ 0xE9, 0xD1, 0xEF, 0xFF, 0xFF,// jmp near ptr 000000000000802Dh
		/*005C*/ 0xB0, 0x08,// mov al,8
	];
	#[rustfmt::skip]
	let expected_instruction_offsets = [
		0x0000,
		u32::MAX,
		0x000B,
		u32::MAX,
		0x0017,
		u32::MAX,
		0x0022,
		u32::MAX,
		0x002E,
		u32::MAX,
		0x0039,
		u32::MAX,
		0x0045,
		u32::MAX,
		0x0051,
		u32::MAX,
		0x005C,
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
fn br8_short_other_long() {
	#[rustfmt::skip]
	let original_data = [
		/*0000*/ 0xB0, 0x00,// mov al,0
		/*0002*/ 0xE2, 0x22,// loop 123456789ABCDE26h
		/*0004*/ 0xB0, 0x01,// mov al,1
		/*0006*/ 0x67, 0xE2, 0x1E,// loopd 123456789ABCDE27h
		/*0009*/ 0xB0, 0x02,// mov al,2
		/*000B*/ 0xE1, 0x1B,// loope 123456789ABCDE28h
		/*000D*/ 0xB0, 0x03,// mov al,3
		/*000F*/ 0x67, 0xE1, 0x17,// looped 123456789ABCDE29h
		/*0012*/ 0xB0, 0x04,// mov al,4
		/*0014*/ 0xE0, 0x14,// loopne 123456789ABCDE2Ah
		/*0016*/ 0xB0, 0x05,// mov al,5
		/*0018*/ 0x67, 0xE0, 0x10,// loopned 123456789ABCDE2Bh
		/*001B*/ 0xB0, 0x06,// mov al,6
		/*001D*/ 0x67, 0xE3, 0x0C,// jecxz 123456789ABCDE2Ch
		/*0020*/ 0xB0, 0x07,// mov al,7
		/*0022*/ 0xE3, 0x09,// jrcxz 123456789ABCDE2Dh
		/*0024*/ 0xB0, 0x08,// mov al,8
	];
	#[rustfmt::skip]
	let new_data = [
		/*0000*/ 0xB0, 0x00,// mov al,0
		/*0002*/ 0xE2, 0x02,// loop 8000000000000006h
		/*0004*/ 0xEB, 0x06,// jmp short 800000000000000Ch
		/*0006*/ 0xFF, 0x25, 0x5C, 0x00, 0x00, 0x00,// jmp qword ptr [8000000000000068h]
		/*000C*/ 0xB0, 0x01,// mov al,1
		/*000E*/ 0x67, 0xE2, 0x02,// loopd 8000000000000013h
		/*0011*/ 0xEB, 0x06,// jmp short 8000000000000019h
		/*0013*/ 0xFF, 0x25, 0x57, 0x00, 0x00, 0x00,// jmp qword ptr [8000000000000070h]
		/*0019*/ 0xB0, 0x02,// mov al,2
		/*001B*/ 0xE1, 0x02,// loope 800000000000001Fh
		/*001D*/ 0xEB, 0x06,// jmp short 8000000000000025h
		/*001F*/ 0xFF, 0x25, 0x53, 0x00, 0x00, 0x00,// jmp qword ptr [8000000000000078h]
		/*0025*/ 0xB0, 0x03,// mov al,3
		/*0027*/ 0x67, 0xE1, 0x02,// looped 800000000000002Ch
		/*002A*/ 0xEB, 0x06,// jmp short 8000000000000032h
		/*002C*/ 0xFF, 0x25, 0x4E, 0x00, 0x00, 0x00,// jmp qword ptr [8000000000000080h]
		/*0032*/ 0xB0, 0x04,// mov al,4
		/*0034*/ 0xE0, 0x02,// loopne 8000000000000038h
		/*0036*/ 0xEB, 0x06,// jmp short 800000000000003Eh
		/*0038*/ 0xFF, 0x25, 0x4A, 0x00, 0x00, 0x00,// jmp qword ptr [8000000000000088h]
		/*003E*/ 0xB0, 0x05,// mov al,5
		/*0040*/ 0x67, 0xE0, 0x02,// loopned 8000000000000045h
		/*0043*/ 0xEB, 0x06,// jmp short 800000000000004Bh
		/*0045*/ 0xFF, 0x25, 0x45, 0x00, 0x00, 0x00,// jmp qword ptr [8000000000000090h]
		/*004B*/ 0xB0, 0x06,// mov al,6
		/*004D*/ 0x67, 0xE3, 0x02,// jecxz 8000000000000052h
		/*0050*/ 0xEB, 0x06,// jmp short 8000000000000058h
		/*0052*/ 0xFF, 0x25, 0x40, 0x00, 0x00, 0x00,// jmp qword ptr [8000000000000098h]
		/*0058*/ 0xB0, 0x07,// mov al,7
		/*005A*/ 0xE3, 0x02,// jrcxz 800000000000005Eh
		/*005C*/ 0xEB, 0x06,// jmp short 8000000000000064h
		/*005E*/ 0xFF, 0x25, 0x3C, 0x00, 0x00, 0x00,// jmp qword ptr [80000000000000A0h]
		/*0064*/ 0xB0, 0x08,// mov al,8
		/*0066*/ 0xCC, 0xCC,
		/*0068*/ 0x26, 0xDE, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12,
		/*0070*/ 0x27, 0xDE, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12,
		/*0078*/ 0x28, 0xDE, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12,
		/*0080*/ 0x29, 0xDE, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12,
		/*0088*/ 0x2A, 0xDE, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12,
		/*0090*/ 0x2B, 0xDE, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12,
		/*0098*/ 0x2C, 0xDE, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12,
		/*00A0*/ 0x2D, 0xDE, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12,
	];
	#[rustfmt::skip]
	let expected_instruction_offsets = [
		0x0000,
		u32::MAX,
		0x000C,
		u32::MAX,
		0x0019,
		u32::MAX,
		0x0025,
		u32::MAX,
		0x0032,
		u32::MAX,
		0x003E,
		u32::MAX,
		0x004B,
		u32::MAX,
		0x0058,
		u32::MAX,
		0x0064,
	];
	#[rustfmt::skip]
	let expected_reloc_infos = [
		RelocInfo::new(RelocKind::Offset64, 0x8000_0000_0000_0068),
		RelocInfo::new(RelocKind::Offset64, 0x8000_0000_0000_0070),
		RelocInfo::new(RelocKind::Offset64, 0x8000_0000_0000_0078),
		RelocInfo::new(RelocKind::Offset64, 0x8000_0000_0000_0080),
		RelocInfo::new(RelocKind::Offset64, 0x8000_0000_0000_0088),
		RelocInfo::new(RelocKind::Offset64, 0x8000_0000_0000_0090),
		RelocInfo::new(RelocKind::Offset64, 0x8000_0000_0000_0098),
		RelocInfo::new(RelocKind::Offset64, 0x8000_0000_0000_00A0),
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
