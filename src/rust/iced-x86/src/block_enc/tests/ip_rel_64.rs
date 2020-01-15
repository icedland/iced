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
fn ip_rel_fwd_bwd() {
	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
	let original_data = [
		/*0000*/ 0xB0, 0x01,// mov al,1
		/*0002*/ 0x48, 0x8B, 0x05, 0x1F, 0x00, 0x00, 0x00,// mov rax,[8028h]
		/*0009*/ 0xB0, 0x02,// mov al,2
		/*000B*/ 0x48, 0x8B, 0x05, 0xEE, 0xFF, 0xFF, 0xFF,// mov rax,[8000h]
		/*0012*/ 0xB0, 0x03,// mov al,3
		/*0014*/ 0x67, 0x48, 0x8B, 0x05, 0x0C, 0x00, 0x00, 0x00,// mov rax,[8028h]
		/*001C*/ 0xB0, 0x04,// mov al,4
		/*001E*/ 0x67, 0x48, 0x8B, 0x05, 0xDA, 0xFF, 0xFF, 0xFF,// mov rax,[8000h]
		/*0026*/ 0xB0, 0x05,// mov al,5
		/*0028*/ 0xB0, 0x06,// mov al,6
	];
	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
	let new_data = [
		/*0000*/ 0xB0, 0x01,// mov al,1
		/*0002*/ 0x48, 0x8B, 0x05, 0x1D, 0x00, 0x00, 0x00,// mov rax,[8000000000000026h]
		/*0009*/ 0xB0, 0x02,// mov al,2
		/*000B*/ 0x48, 0x8B, 0x05, 0xEE, 0xFF, 0xFF, 0xFF,// mov rax,[8000000000000000h]
		/*0012*/ 0xB0, 0x03,// mov al,3
		/*0014*/ 0x48, 0x8B, 0x05, 0x0B, 0x00, 0x00, 0x00,// mov rax,[8000000000000026h]
		/*001B*/ 0xB0, 0x04,// mov al,4
		/*001D*/ 0x48, 0x8B, 0x05, 0xDC, 0xFF, 0xFF, 0xFF,// mov rax,[8000000000000000h]
		/*0024*/ 0xB0, 0x05,// mov al,5
		/*0026*/ 0xB0, 0x06,// mov al,6
	];
	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
	let expected_instruction_offsets = [
		0x0000,
		0x0002,
		0x0009,
		0x000B,
		0x0012,
		0x0014,
		0x001B,
		0x001D,
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
fn ip_rel_fwd_bwd_other_near() {
	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
	let original_data = [
		/*0000*/ 0xB0, 0x01,// mov al,1
		/*0002*/ 0x48, 0x8B, 0x05, 0x1F, 0x00, 0x00, 0x00,// mov rax,[8028h]
		/*0009*/ 0xB0, 0x02,// mov al,2
		/*000B*/ 0x48, 0x8B, 0x05, 0xED, 0xFF, 0xFF, 0xFF,// mov rax,[7FFFh]
		/*0012*/ 0xB0, 0x03,// mov al,3
		/*0014*/ 0x67, 0x48, 0x8B, 0x05, 0x0C, 0x00, 0x00, 0x00,// mov rax,[8028h]
		/*001C*/ 0xB0, 0x04,// mov al,4
		/*001E*/ 0x67, 0x48, 0x8B, 0x05, 0xD9, 0xFF, 0xFF, 0xFF,// mov rax,[7FFFh]
		/*0026*/ 0xB0, 0x05,// mov al,5
	];
	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
	let new_data = [
		/*0000*/ 0xB0, 0x01,// mov al,1
		/*0002*/ 0x48, 0x8B, 0x05, 0x1F, 0xF0, 0xFF, 0xFF,// mov rax,[8028h]
		/*0009*/ 0xB0, 0x02,// mov al,2
		/*000B*/ 0x48, 0x8B, 0x05, 0xED, 0xEF, 0xFF, 0xFF,// mov rax,[7FFFh]
		/*0012*/ 0xB0, 0x03,// mov al,3
		/*0014*/ 0x48, 0x8B, 0x05, 0x0D, 0xF0, 0xFF, 0xFF,// mov rax,[8028h]
		/*001B*/ 0xB0, 0x04,// mov al,4
		/*001D*/ 0x48, 0x8B, 0x05, 0xDB, 0xEF, 0xFF, 0xFF,// mov rax,[7FFFh]
		/*0024*/ 0xB0, 0x05,// mov al,5
	];
	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
	let expected_instruction_offsets = [
		0x0000,
		0x0002,
		0x0009,
		0x000B,
		0x0012,
		0x0014,
		0x001B,
		0x001D,
		0x0024,
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
fn ip_rel_fwd_bwd_other_long_low4gb() {
	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
	let original_data = [
		/*0000*/ 0xB0, 0x01,// mov al,1
		/*0002*/ 0x48, 0x8B, 0x05, 0x1F, 0x00, 0x00, 0x00,// mov rax,[8028h]
		/*0009*/ 0xB0, 0x02,// mov al,2
		/*000B*/ 0x48, 0x8B, 0x05, 0xED, 0xFF, 0xFF, 0xFF,// mov rax,[7FFFh]
		/*0012*/ 0xB0, 0x03,// mov al,3
		/*0014*/ 0x67, 0x48, 0x8B, 0x05, 0x0C, 0x00, 0x00, 0x00,// mov rax,[8028h]
		/*001C*/ 0xB0, 0x04,// mov al,4
		/*001E*/ 0x67, 0x48, 0x8B, 0x05, 0xD9, 0xFF, 0xFF, 0xFF,// mov rax,[7FFFh]
		/*0026*/ 0xB0, 0x05,// mov al,5
	];
	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
	let new_data = [
		/*0000*/ 0xB0, 0x01,// mov al,1
		/*0002*/ 0x67, 0x48, 0x8B, 0x05, 0x1E, 0x80, 0x00, 0x00,// mov rax,[8028h]
		/*000A*/ 0xB0, 0x02,// mov al,2
		/*000C*/ 0x67, 0x48, 0x8B, 0x05, 0xEB, 0x7F, 0x00, 0x00,// mov rax,[7FFFh]
		/*0014*/ 0xB0, 0x03,// mov al,3
		/*0016*/ 0x67, 0x48, 0x8B, 0x05, 0x0A, 0x80, 0x00, 0x00,// mov rax,[8028h]
		/*001E*/ 0xB0, 0x04,// mov al,4
		/*0020*/ 0x67, 0x48, 0x8B, 0x05, 0xD7, 0x7F, 0x00, 0x00,// mov rax,[7FFFh]
		/*0028*/ 0xB0, 0x05,// mov al,5
	];
	#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
	let expected_instruction_offsets = [
		0x0000,
		0x0002,
		0x000A,
		0x000C,
		0x0014,
		0x0016,
		0x001E,
		0x0020,
		0x0028,
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
