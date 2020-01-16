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

use super::*;
use core::u32;

const BITNESS: u32 = 64;
const ORIG_RIP: u64 = 0x8000;
const NEW_RIP: u64 = 0x8000_0000_0000_0000;

#[test]
fn xbegin_fwd() {
	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
	let original_data = [
		/*0000*/ 0xB0, 0x00,// mov al,0
		/*0002*/ 0x66, 0xC7, 0xF8, 0x11, 0x00,// xbegin 0000000000008018h
		/*0007*/ 0xB0, 0x01,// mov al,1
		/*0009*/ 0xC7, 0xF8, 0x0B, 0x00, 0x00, 0x00,// xbegin 000000000000801Ah
		/*000F*/ 0xB0, 0x02,// mov al,2
		/*0011*/ 0x48, 0xC7, 0xF8, 0x04, 0x00, 0x00, 0x00,// xbegin 000000000000801Ch
		/*0018*/ 0xB0, 0x03,// mov al,3
		/*001A*/ 0xB0, 0x04,// mov al,4
		/*001C*/ 0xB0, 0x05,// mov al,5
	];
	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
	let new_data = [
		/*0000*/ 0xB0, 0x00,// mov al,0
		/*0002*/ 0xC7, 0xF8, 0x10, 0x00, 0x00, 0x00,// xbegin 8000000000000018h
		/*0008*/ 0xB0, 0x01,// mov al,1
		/*000A*/ 0xC7, 0xF8, 0x0A, 0x00, 0x00, 0x00,// xbegin 800000000000001Ah
		/*0010*/ 0xB0, 0x02,// mov al,2
		/*0012*/ 0xC7, 0xF8, 0x04, 0x00, 0x00, 0x00,// xbegin 800000000000001Ch
		/*0018*/ 0xB0, 0x03,// mov al,3
		/*001A*/ 0xB0, 0x04,// mov al,4
		/*001C*/ 0xB0, 0x05,// mov al,5
	];
	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
	let expected_instruction_offsets = [
		0x0000,
		0x0002,
		0x0008,
		0x000A,
		0x0010,
		0x0012,
		0x0018,
		0x001A,
		0x001C,
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
fn xbegin_bwd() {
	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
	let original_data = [
		/*0000*/ 0xB0, 0x03,// mov al,3
		/*0002*/ 0xB0, 0x04,// mov al,4
		/*0004*/ 0xB0, 0x05,// mov al,5
		/*0006*/ 0xB0, 0x00,// mov al,0
		/*0008*/ 0x66, 0xC7, 0xF8, 0xF3, 0xFF,// xbegin 0000000000008000h
		/*000D*/ 0xB0, 0x01,// mov al,1
		/*000F*/ 0xC7, 0xF8, 0xED, 0xFF, 0xFF, 0xFF,// xbegin 0000000000008002h
		/*0015*/ 0xB0, 0x02,// mov al,2
		/*0017*/ 0x48, 0xC7, 0xF8, 0xE6, 0xFF, 0xFF, 0xFF,// xbegin 0000000000008004h
	];
	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
	let new_data = [
		/*0000*/ 0xB0, 0x03,// mov al,3
		/*0002*/ 0xB0, 0x04,// mov al,4
		/*0004*/ 0xB0, 0x05,// mov al,5
		/*0006*/ 0xB0, 0x00,// mov al,0
		/*0008*/ 0xC7, 0xF8, 0xF2, 0xFF, 0xFF, 0xFF,// xbegin 8000000000000000h
		/*000E*/ 0xB0, 0x01,// mov al,1
		/*0010*/ 0xC7, 0xF8, 0xEC, 0xFF, 0xFF, 0xFF,// xbegin 8000000000000002h
		/*0016*/ 0xB0, 0x02,// mov al,2
		/*0018*/ 0xC7, 0xF8, 0xE6, 0xFF, 0xFF, 0xFF,// xbegin 8000000000000004h
	];
	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
	let expected_instruction_offsets = [
		0x0000,
		0x0002,
		0x0004,
		0x0006,
		0x0008,
		0x000E,
		0x0010,
		0x0016,
		0x0018,
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
