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

use super::super::super::decoder::tests::test_utils::{code32_only, code64_only, decoder_tests, not_decoded32_only, not_decoded64_only};
use super::super::super::iced_constants::IcedConstants;
use super::super::super::test_utils::from_str_conv::{code_names, is_ignored_code, to_vec_u8};
use super::super::super::*;
#[cfg(not(feature = "std"))]
use alloc::string::String;
#[cfg(not(feature = "std"))]
use alloc::vec::Vec;
use core::iter;
use core::mem;
#[cfg(not(feature = "std"))]
use hashbrown::HashSet;
#[cfg(feature = "std")]
use std::collections::HashSet;

#[test]
fn verify_invalid_and_valid_lock_prefix() {
	for info in decoder_tests(false, false) {
		if (info.decoder_options() & DecoderOptions::NO_INVALID_CHECK) != 0 {
			continue;
		}

		let has_lock;
		let can_use_lock;

		{
			let bytes = to_vec_u8(info.hex_bytes()).unwrap();
			let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
			let instruction = decoder.decode();
			assert_eq!(info.code(), instruction.code());
			has_lock = instruction.has_lock_prefix();
			let op_code = info.code().op_code();
			can_use_lock = op_code.can_use_lock_prefix() && has_modrm_memory_operand(&instruction);

			match info.code() {
				Code::Mov_r32_cr | Code::Mov_r64_cr | Code::Mov_cr_r32 | Code::Mov_cr_r64 => continue,
				_ => {}
			}
		}

		if can_use_lock {
			let bytes = to_vec_u8(&add_lock(info.hex_bytes(), has_lock)).unwrap();
			let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
			let instruction = decoder.decode();
			assert_eq!(info.code(), instruction.code());
			assert!(instruction.has_lock_prefix());
		} else {
			debug_assert!(!has_lock);
			{
				let bytes = to_vec_u8(&add_lock(info.hex_bytes(), has_lock)).unwrap();
				let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
				let instruction = decoder.decode();
				assert_eq!(Code::INVALID, instruction.code());
				assert!(!decoder.invalid_no_more_bytes());
				assert!(!instruction.has_lock_prefix());
			}
			{
				let bytes = to_vec_u8(&add_lock(info.hex_bytes(), has_lock)).unwrap();
				let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options() | DecoderOptions::NO_INVALID_CHECK);
				let instruction = decoder.decode();
				assert_eq!(info.code(), instruction.code());
				assert!(instruction.has_lock_prefix());
			}
		}
	}

	fn add_lock(hex_bytes: &str, has_lock: bool) -> String {
		if has_lock {
			String::from(hex_bytes)
		} else {
			format!("F0{}", hex_bytes)
		}
	}

	fn has_modrm_memory_operand(instruction: &Instruction) -> bool {
		let op_count = instruction.op_count();
		for i in 0..op_count {
			if instruction.op_kind(i) == OpKind::Memory {
				return true;
			}
		}
		false
	}
}

#[test]
fn verify_invalid_rex_mandatory_prefixes_vex_evex_xop() {
	let prefixes1632 = vec!["66", "F3", "F2"];
	let prefixes64 = vec!["66", "F3", "F2", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "4A", "4B", "4C", "4D", "4E", "4F"];
	for info in decoder_tests(false, false) {
		if (info.decoder_options() & DecoderOptions::NO_INVALID_CHECK) != 0 {
			continue;
		}

		match info.code().op_code().encoding() {
			EncodingKind::Legacy | EncodingKind::D3NOW => continue,
			EncodingKind::VEX | EncodingKind::EVEX | EncodingKind::XOP => {}
		}

		let prefixes: &[&str] = match info.bitness() {
			16 | 32 => &prefixes1632,
			64 => &prefixes64,
			_ => unreachable!(),
		};
		for &prefix in prefixes {
			let orig_instr;
			{
				let bytes = to_vec_u8(info.hex_bytes()).unwrap();
				let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
				orig_instr = decoder.decode();
				assert_eq!(info.code(), orig_instr.code());
				// Mandatory prefix must be right before the opcode. If it has a seg override, there's also
				// a test without a seg override so just skip this.
				if orig_instr.segment_prefix() != Register::None {
					continue;
				}
				let mem_reg_size = get_memory_register_size(&orig_instr);
				// 67h prefix
				if mem_reg_size != 0 && mem_reg_size != info.bitness() {
					continue;
				}
				let (non_prefix_index, _) = skip_prefixes(&bytes, info.bitness());
				if bytes[0..non_prefix_index].iter().any(|&b| b == 0x67) {
					continue;
				}
			}
			let hex_bytes = String::from(prefix) + info.hex_bytes();
			{
				let bytes = to_vec_u8(&hex_bytes).unwrap();
				let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options() | DecoderOptions::NO_INVALID_CHECK);
				let mut instruction = decoder.decode();
				assert_eq!(info.code(), instruction.code());

				let len = instruction.len();
				instruction.set_len(len - 1);
				let next_ip = instruction.next_ip();
				instruction.set_next_ip(next_ip - 1);
				if prefix == "F3" {
					assert!(instruction.has_rep_prefix());
					assert!(instruction.has_repe_prefix());
					instruction.set_has_rep_prefix(false);
				} else if prefix == "F2" {
					assert!(instruction.has_repne_prefix());
					instruction.set_has_repne_prefix(false);
				}
				assert!(instruction.eq_all_bits(&orig_instr));
			}
			{
				let bytes = to_vec_u8(&hex_bytes).unwrap();
				let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
				let instruction = decoder.decode();
				assert_eq!(Code::INVALID, instruction.code());
				assert!(!decoder.invalid_no_more_bytes());
			}
		}
	}
}

fn get_memory_register_size(instruction: &Instruction) -> u32 {
	for i in 0..instruction.op_count() {
		match instruction.op_kind(i) {
			OpKind::Register
			| OpKind::NearBranch16
			| OpKind::NearBranch32
			| OpKind::NearBranch64
			| OpKind::FarBranch16
			| OpKind::FarBranch32
			| OpKind::Immediate8
			| OpKind::Immediate8_2nd
			| OpKind::Immediate16
			| OpKind::Immediate32
			| OpKind::Immediate64
			| OpKind::Immediate8to16
			| OpKind::Immediate8to32
			| OpKind::Immediate8to64
			| OpKind::Immediate32to64 => {}
			OpKind::MemorySegSI | OpKind::MemorySegDI | OpKind::MemoryESDI => return 16,
			OpKind::MemorySegESI | OpKind::MemorySegEDI | OpKind::MemoryESEDI => return 32,
			OpKind::MemorySegRSI | OpKind::MemorySegRDI | OpKind::MemoryESRDI => return 64,
			OpKind::Memory => {
				let mut reg = instruction.memory_base();
				if reg == Register::None {
					reg = instruction.memory_index();
				}
				if reg != Register::None {
					return reg_size(reg) * 8;
				} else if instruction.memory_displ_size() == 4 {
					return 32;
				} else if instruction.memory_displ_size() == 8 {
					return 64;
				}
			}
			OpKind::Memory64 => return 64,
		}
	}
	0
}

#[cfg(feature = "instr_info")]
fn reg_size(reg: Register) -> u32 {
	reg.size() as u32
}

#[cfg(not(feature = "instr_info"))]
fn reg_size(reg: Register) -> u32 {
	if Register::AX <= reg && reg <= Register::R15W {
		2
	} else if Register::EAX <= reg && reg <= Register::R15D || reg == Register::EIP {
		4
	} else if Register::RAX <= reg && reg <= Register::R15 || reg == Register::RIP {
		8
	} else {
		panic!()
	}
}

#[cfg(feature = "instr_info")]
fn reg_number(reg: Register) -> u32 {
	reg.number() as u32
}

#[cfg(not(feature = "instr_info"))]
fn reg_number(reg: Register) -> u32 {
	if Register::AL <= reg && reg <= Register::R15L {
		reg as u32 - Register::AL as u32
	} else if Register::AX <= reg && reg <= Register::R15W {
		reg as u32 - Register::AX as u32
	} else if Register::EAX <= reg && reg <= Register::R15D {
		reg as u32 - Register::EAX as u32
	} else if Register::RAX <= reg && reg <= Register::R15 {
		reg as u32 - Register::RAX as u32
	} else if Register::XMM0 <= reg && reg <= Register::XMM31 {
		reg as u32 - Register::XMM0 as u32
	} else if Register::YMM0 <= reg && reg <= Register::YMM31 {
		reg as u32 - Register::YMM0 as u32
	} else if Register::ZMM0 <= reg && reg <= Register::ZMM31 {
		reg as u32 - Register::ZMM0 as u32
	} else if Register::K0 <= reg && reg <= Register::K7 {
		reg as u32 - Register::K0 as u32
	} else if Register::BND0 <= reg && reg <= Register::BND3 {
		reg as u32 - Register::BND0 as u32
	} else if Register::CR0 <= reg && reg <= Register::CR15 {
		reg as u32 - Register::CR0 as u32
	} else if Register::DR0 <= reg && reg <= Register::DR15 {
		reg as u32 - Register::DR0 as u32
	} else if Register::MM0 <= reg && reg <= Register::MM7 {
		reg as u32 - Register::MM0 as u32
	} else if Register::ST0 <= reg && reg <= Register::ST7 {
		reg as u32 - Register::ST0 as u32
	} else if Register::TR0 <= reg && reg <= Register::TR7 {
		reg as u32 - Register::TR0 as u32
	} else {
		panic!()
	}
}

fn skip_prefixes(bytes: &[u8], bitness: u32) -> (usize, u32) {
	let mut rex = 0;
	for (i, &b) in bytes.iter().enumerate() {
		match b {
			0x26 | 0x2E | 0x36 | 0x3E | 0x64 | 0x65 | 0x66 | 0x67 | 0xF0 | 0xF2 | 0xF3 => {
				rex = 0;
			}
			_ => {
				if bitness == 64 && (b & 0xF0) == 0x40 {
					rex = b as u32;
				} else {
					return (i, rex);
				}
			}
		}
	}
	(bytes.len(), rex)
}

#[test]
fn test_evex_reserved_bits() {
	for info in decoder_tests(false, false) {
		if info.code().op_code().encoding() != EncodingKind::EVEX {
			continue;
		}
		let mut bytes = to_vec_u8(info.hex_bytes()).unwrap();
		let evex_index = get_evex_index(&bytes);
		for i in 1..4 {
			bytes[evex_index + 1] = (bytes[evex_index + 1] & !0x0C) | (i << 2) as u8;
			{
				let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
				let instruction = decoder.decode();
				assert_eq!(Code::INVALID, instruction.code());
				assert!(!decoder.invalid_no_more_bytes());
			}
			{
				let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options() ^ DecoderOptions::NO_INVALID_CHECK);
				let instruction = decoder.decode();
				assert_eq!(Code::INVALID, instruction.code());
				assert!(!decoder.invalid_no_more_bytes());
			}
		}
	}
}

fn get_evex_index(bytes: &[u8]) -> usize {
	bytes.iter().position(|&b| b == 0x62).unwrap()
}

fn get_vex_xop_index(bytes: &[u8]) -> usize {
	bytes.iter().position(|&b| b == 0xC4 || b == 0xC5 || b == 0x8F).unwrap()
}

#[test]
fn test_wig_instructions_ignore_w() {
	for info in decoder_tests(false, false) {
		let op_code = info.code().op_code();
		let encoding = op_code.encoding();
		let is_wig = op_code.is_wig() || (op_code.is_wig32() && info.bitness() != 64);
		if encoding == EncodingKind::EVEX {
			let mut bytes = to_vec_u8(info.hex_bytes()).unwrap();
			let evex_index = get_evex_index(&bytes);

			if is_wig {
				let instruction1;
				let instruction2;
				{
					let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
					instruction1 = decoder.decode();
					assert_eq!(info.code(), instruction1.code());
				}
				{
					bytes[evex_index + 2] ^= 0x80;
					let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
					instruction2 = decoder.decode();
					assert_eq!(info.code(), instruction2.code());
				}
				assert!(instruction1.eq_all_bits(&instruction2));
			} else {
				{
					let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
					let instruction = decoder.decode();
					assert_eq!(info.code(), instruction.code());
				}
				{
					bytes[evex_index + 2] ^= 0x80;
					let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
					let instruction = decoder.decode();
					assert_ne!(info.code(), instruction.code());
				}
			}
		} else if encoding == EncodingKind::VEX || encoding == EncodingKind::XOP {
			let mut bytes = to_vec_u8(info.hex_bytes()).unwrap();
			let vex_index = get_vex_xop_index(&bytes);
			if bytes[vex_index] == 0xC5 {
				continue;
			}

			if is_wig {
				let instruction1;
				let instruction2;
				{
					let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
					instruction1 = decoder.decode();
					assert_eq!(info.code(), instruction1.code());
				}
				{
					bytes[vex_index + 2] ^= 0x80;
					let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
					instruction2 = decoder.decode();
					assert_eq!(info.code(), instruction2.code());
				}
				assert!(instruction1.eq_all_bits(&instruction2));
			} else {
				{
					let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
					let instruction = decoder.decode();
					assert_eq!(info.code(), instruction.code());
				}
				{
					bytes[vex_index + 2] ^= 0x80;
					let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
					let instruction = decoder.decode();
					assert_ne!(info.code(), instruction.code());
				}
			}
		} else if encoding == EncodingKind::Legacy || encoding == EncodingKind::D3NOW {
			continue;
		} else {
			panic!();
		}
	}
}

#[test]
fn test_lig_instructions_ignore_l() {
	for info in decoder_tests(false, false) {
		let op_code = info.code().op_code();
		let encoding = op_code.encoding();
		if encoding == EncodingKind::EVEX {
			let mut bytes = to_vec_u8(info.hex_bytes()).unwrap();
			let evex_index = get_evex_index(&bytes);

			let is_reg_only = (bytes[evex_index + 5] >> 6) == 3;
			let evex_b = (bytes[evex_index + 3] & 0x10) != 0;
			if op_code.can_use_rounding_control() && is_reg_only && evex_b {
				continue;
			}
			let is_sae = op_code.can_suppress_all_exceptions() && is_reg_only && evex_b;

			if op_code.is_lig() {
				let instruction1;
				let mut instruction2;
				{
					let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
					instruction1 = decoder.decode();
					assert_eq!(info.code(), instruction1.code());
				}
				let orig_byte = bytes[evex_index + 3];
				for i in 1..4 {
					bytes[evex_index + 3] = orig_byte ^ (i << 5);
					let ll = (bytes[evex_index + 3] >> 5) & 3;
					let invalid = (info.decoder_options() & DecoderOptions::NO_INVALID_CHECK) == 0
						&& ll == 3 && (bytes[evex_index + 5] < 0xC0 || (bytes[evex_index + 3] & 0x10) == 0);
					if invalid {
						let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
						instruction2 = decoder.decode();
						assert_eq!(Code::INVALID, instruction2.code());
						assert!(!decoder.invalid_no_more_bytes());

						decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options() | DecoderOptions::NO_INVALID_CHECK);
						instruction2 = decoder.decode();
						assert_eq!(info.code(), instruction2.code());
						assert!(instruction1.eq_all_bits(&instruction2));
					} else {
						let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
						instruction2 = decoder.decode();
						assert_eq!(info.code(), instruction2.code());
						assert!(instruction1.eq_all_bits(&instruction2));
					}
				}
			} else {
				let instruction1;
				{
					let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
					instruction1 = decoder.decode();
					assert_eq!(info.code(), instruction1.code());
				}
				let orig_byte = bytes[evex_index + 3];
				for i in 1..4 {
					bytes[evex_index + 3] = orig_byte ^ (i << 5);
					let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
					let instruction2 = decoder.decode();
					if is_sae {
						assert_eq!(info.code(), instruction2.code());
						assert!(instruction1.eq_all_bits(&instruction2));
					} else {
						assert_ne!(info.code(), instruction2.code());
					}
				}
			}
		} else if encoding == EncodingKind::VEX || encoding == EncodingKind::XOP {
			let mut bytes = to_vec_u8(info.hex_bytes()).unwrap();
			let vex_index = get_vex_xop_index(&bytes);
			let l_index = if bytes[vex_index] == 0xC5 { vex_index + 1 } else { vex_index + 2 };

			if op_code.is_lig() {
				let instruction1;
				let instruction2;
				{
					let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
					instruction1 = decoder.decode();
					assert_eq!(info.code(), instruction1.code());
				}
				{
					bytes[l_index] ^= 4;
					let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
					instruction2 = decoder.decode();
					assert_eq!(info.code(), instruction2.code());
				}
				assert!(instruction1.eq_all_bits(&instruction2));
			} else {
				{
					let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
					let instruction = decoder.decode();
					assert_eq!(info.code(), instruction.code());
				}
				{
					bytes[l_index] ^= 4;
					let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
					let instruction = decoder.decode();
					assert_ne!(info.code(), instruction.code());
				}
			}
		} else if encoding == EncodingKind::Legacy || encoding == EncodingKind::D3NOW {
			continue;
		} else {
			panic!();
		}
	}
}

fn has_is4_or_is5_operands(op_code: &OpCodeInfo) -> bool {
	for i in 0..op_code.op_count() {
		match op_code.op_kind(i) {
			OpCodeOperandKind::xmm_is4 | OpCodeOperandKind::xmm_is5 | OpCodeOperandKind::ymm_is4 | OpCodeOperandKind::ymm_is5 => return true,
			_ => {}
		}
	}
	false
}

#[test]
fn test_is4_is5_instructions_ignore_bit7_in_1632mode() {
	for info in decoder_tests(false, false) {
		if info.bitness() != 16 && info.bitness() != 32 {
			continue;
		}
		let op_code = info.code().op_code();
		if !has_is4_or_is5_operands(op_code) {
			continue;
		}
		let mut bytes = to_vec_u8(info.hex_bytes()).unwrap();
		let instruction1 = {
			let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
			decoder.decode()
		};
		*bytes.last_mut().unwrap() ^= 0x80;
		let instruction2 = {
			let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
			decoder.decode()
		};
		assert_eq!(info.code(), instruction1.code());
		assert!(instruction1.eq_all_bits(&instruction2));
	}
}

#[test]
fn test_evex_k1_z_bits() {
	let p2_values_k1z: Vec<(bool, u8)> = vec![(true, 0x00), (true, 0x01), (false, 0x80), (true, 0x86)];
	let p2_values_k1: Vec<(bool, u8)> = vec![(true, 0x00), (true, 0x01), (false, 0x80), (false, 0x86)];
	let p2_values_k1_fk: Vec<(bool, u8)> = vec![(false, 0x00), (true, 0x01), (false, 0x80), (false, 0x86)];
	let p2_values_nothing: Vec<(bool, u8)> = vec![(true, 0x00), (false, 0x01), (false, 0x80), (false, 0x86)];
	for info in decoder_tests(false, false) {
		if (info.decoder_options() & DecoderOptions::NO_INVALID_CHECK) != 0 {
			continue;
		}

		let op_code = info.code().op_code();
		if op_code.encoding() != EncodingKind::EVEX {
			continue;
		}
		let mut bytes = to_vec_u8(info.hex_bytes()).unwrap();
		let evex_index = get_evex_index(&bytes);
		let p2_values: &[(bool, u8)] = if op_code.can_use_zeroing_masking() {
			assert!(op_code.can_use_op_mask_register());
			let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options() | DecoderOptions::NO_INVALID_CHECK);
			let instruction = decoder.decode();
			debug_assert_ne!(Code::INVALID, instruction.code());
			if instruction.op0_kind() == OpKind::Memory {
				&p2_values_k1
			} else {
				&p2_values_k1z
			}
		} else if op_code.can_use_op_mask_register() {
			if op_code.require_non_zero_op_mask_register() {
				&p2_values_k1_fk
			} else {
				&p2_values_k1
			}
		} else {
			&p2_values_nothing
		};

		let b = bytes[evex_index + 3];
		for p2v in p2_values {
			for i in 0..2 {
				bytes[evex_index + 3] = (b & !0x87) | p2v.1;
				let options = if i == 0 { info.decoder_options() } else { info.decoder_options() | DecoderOptions::NO_INVALID_CHECK };
				let mut decoder = Decoder::new(info.bitness(), &bytes, options);
				let instruction = decoder.decode();
				if p2v.0 || (options & DecoderOptions::NO_INVALID_CHECK) != 0 {
					assert_eq!(info.code(), instruction.code());
					assert_eq!((p2v.1 & 0x80) != 0, instruction.zeroing_masking());
					if (p2v.1 & 7) != 0 {
						let expected_reg: Register = unsafe { mem::transmute((Register::K0 as u32 + (p2v.1 & 7) as u32) as u8) };
						assert_eq!(expected_reg, instruction.op_mask());
					} else {
						assert_eq!(Register::None, instruction.op_mask());
					}
				} else {
					assert_eq!(Code::INVALID, instruction.code());
					assert!(!decoder.invalid_no_more_bytes());
				}
			}
		}
	}
}

#[test]
#[cfg_attr(feature = "cargo-clippy", allow(clippy::unnecessary_unwrap))]
fn test_evex_b_bit() {
	for info in decoder_tests(false, false) {
		if (info.decoder_options() & DecoderOptions::NO_INVALID_CHECK) != 0 {
			continue;
		}

		let op_code = info.code().op_code();
		if op_code.encoding() != EncodingKind::EVEX {
			continue;
		}
		let mut bytes = to_vec_u8(info.hex_bytes()).unwrap();
		let evex_index = get_evex_index(&bytes);

		let is_reg_only = (bytes[evex_index + 5] >> 6) == 3;
		let is_sae_or_er = is_reg_only && (op_code.can_use_rounding_control() || op_code.can_suppress_all_exceptions());
		let new_code = get_sae_er_instruction(op_code);

		if op_code.can_broadcast() && !is_reg_only {
			{
				bytes[evex_index + 3] &= 0xEF;
				let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
				let instruction = decoder.decode();
				assert_eq!(info.code(), instruction.code());
				assert!(!instruction.is_broadcast());
			}
			{
				bytes[evex_index + 3] |= 0x10;
				let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
				let instruction = decoder.decode();
				assert_eq!(info.code(), instruction.code());
				assert!(instruction.is_broadcast());
			}
		} else {
			if !is_sae_or_er {
				bytes[evex_index + 3] &= 0xEF;
				let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
				let instruction = decoder.decode();
				assert_eq!(info.code(), instruction.code());
				assert!(!instruction.is_broadcast());
			}
			{
				bytes[evex_index + 3] |= 0x10;
				let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
				let instruction = decoder.decode();
				if is_sae_or_er {
					assert_eq!(info.code(), instruction.code());
				} else if new_code.is_some() && is_reg_only {
					assert_eq!(new_code.unwrap(), instruction.code());
				} else {
					assert_eq!(Code::INVALID, instruction.code());
					assert!(!decoder.invalid_no_more_bytes());
				}
				assert!(!instruction.is_broadcast());
			}
			{
				bytes[evex_index + 3] |= 0x10;
				let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options() | DecoderOptions::NO_INVALID_CHECK);
				let instruction = decoder.decode();
				if new_code.is_some() && is_reg_only {
					assert_eq!(new_code.unwrap(), instruction.code());
				} else {
					assert_eq!(info.code(), instruction.code());
				}
				assert!(!instruction.is_broadcast());
			}
		}
	}
}

fn get_sae_er_instruction(op_code: &OpCodeInfo) -> Option<Code> {
	if op_code.encoding() == EncodingKind::EVEX && !(op_code.can_suppress_all_exceptions() || op_code.can_use_rounding_control()) {
		let mnemonic = op_code.code().mnemonic();
		for (j, i) in (op_code.code() as usize + 1..IcedConstants::NUMBER_OF_CODE_VALUES).enumerate() {
			if j > 1 {
				break;
			}
			let next_code: Code = unsafe { mem::transmute(i as u16) };
			if next_code.mnemonic() != mnemonic {
				break;
			}
			let next_op_code = next_code.op_code();
			if next_op_code.encoding() != op_code.encoding() {
				break;
			}
			if next_op_code.can_suppress_all_exceptions() || next_op_code.can_use_rounding_control() {
				return Some(next_code);
			}
		}
	}
	None
}

#[test]
#[cfg_attr(feature = "cargo-clippy", allow(clippy::needless_range_loop))]
fn verify_only_full_ddd_and_half_ddd_support_bcst() {
	let code_names = code_names();
	for i in 0..IcedConstants::NUMBER_OF_CODE_VALUES {
		if is_ignored_code(code_names[i]) {
			continue;
		}
		let code: Code = unsafe { mem::transmute(i as u16) };
		let op_code = code.op_code();
		let expected_bcst = match op_code.tuple_type() {
			TupleType::Full_128 | TupleType::Full_256 | TupleType::Full_512 | TupleType::Half_128 | TupleType::Half_256 | TupleType::Half_512 => true,
			_ => false,
		};
		assert_eq!(expected_bcst, op_code.can_broadcast());
	}
}

#[test]
fn verify_invalid_vvvv() {
	for info in decoder_tests(false, false) {
		if (info.decoder_options() & DecoderOptions::NO_INVALID_CHECK) != 0 {
			continue;
		}

		let op_code = info.code().op_code();

		match op_code.encoding() {
			EncodingKind::Legacy | EncodingKind::D3NOW => continue,
			EncodingKind::VEX | EncodingKind::EVEX | EncodingKind::XOP => {}
		}

		let (uses_vvvv, is_vsib, vvvv_mask) = get_vvvvv_info(op_code);

		if op_code.encoding() == EncodingKind::VEX || op_code.encoding() == EncodingKind::XOP {
			let mut bytes = to_vec_u8(info.hex_bytes()).unwrap();
			let vex_index = get_vex_xop_index(&bytes);
			let mut b2i = vex_index + 1;
			if bytes[vex_index] != 0xC5 {
				b2i += 1;
			}
			let b2 = bytes[b2i];
			let orig_instr;
			{
				let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
				orig_instr = decoder.decode();
				assert_eq!(info.code(), orig_instr.code());
			}
			let is_vex2 = bytes[vex_index] == 0xC5;
			let b2_mask = if info.bitness() == 64 || !is_vex2 { 0x78 } else { 0x38 };
			if uses_vvvv {
				bytes[b2i] = (b2 & !b2_mask) | (b2_mask & !(vvvv_mask << 3));
				{
					let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
					let instruction = decoder.decode();
					assert_eq!(info.code(), instruction.code());
				}
				if info.bitness() != 64 && !is_vex2 {
					// vvvv[3] is ignored in 16/32-bit modes, clear it (it's inverted, so 'set' it)
					bytes[b2i] = b2 & !0x40;
					let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
					let instruction = decoder.decode();
					assert_eq!(info.code(), instruction.code());
					assert!(orig_instr.eq_all_bits(&instruction));
				}
				if info.bitness() == 64 && vvvv_mask != 0xF {
					bytes[b2i] = b2 & !b2_mask;
					{
						let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
						let instruction = decoder.decode();
						assert_eq!(Code::INVALID, instruction.code());
						assert!(!decoder.invalid_no_more_bytes());
					}
					{
						let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options() | DecoderOptions::NO_INVALID_CHECK);
						let instruction = decoder.decode();
						assert_eq!(info.code(), instruction.code());
					}
				}
			} else {
				bytes[b2i] = b2 & !b2_mask;
				{
					let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
					let instruction = decoder.decode();
					assert_eq!(Code::INVALID, instruction.code());
					assert!(!decoder.invalid_no_more_bytes());
				}
				{
					let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options() | DecoderOptions::NO_INVALID_CHECK);
					let instruction = decoder.decode();
					assert_eq!(info.code(), instruction.code());
					assert!(orig_instr.eq_all_bits(&instruction));
				}
				if info.bitness() != 64 && !is_vex2 {
					// vvvv[3] is ignored in 16/32-bit modes, clear it (it's inverted, so 'set' it)
					bytes[b2i] = b2 & !0x40;
					let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
					let instruction = decoder.decode();
					assert_eq!(info.code(), instruction.code());
					assert!(orig_instr.eq_all_bits(&instruction));
				}
			}
		} else if op_code.encoding() == EncodingKind::EVEX {
			debug_assert_eq!(0x1F, vvvv_mask);
			let mut bytes = to_vec_u8(info.hex_bytes()).unwrap();
			let evex_index = get_evex_index(&bytes);
			let b2 = bytes[evex_index + 2];
			let b3 = bytes[evex_index + 3];
			let orig_instr;
			{
				let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
				orig_instr = decoder.decode();
				assert_eq!(info.code(), orig_instr.code());
			}
			bytes[evex_index + 2] = b2 & 0x87;
			if !is_vsib {
				bytes[evex_index + 3] = b3 & 0xF7;
			}
			{
				let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
				let instruction = decoder.decode();
				if uses_vvvv {
					assert_eq!(info.code(), instruction.code());
				} else {
					assert_eq!(Code::INVALID, instruction.code());
					assert!(!decoder.invalid_no_more_bytes());
					decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options() | DecoderOptions::NO_INVALID_CHECK);
					let instruction = decoder.decode();
					assert_eq!(info.code(), instruction.code());
				}
			}
			if !uses_vvvv {
				let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options() | DecoderOptions::NO_INVALID_CHECK);
				let instruction = decoder.decode();
				assert_eq!(info.code(), instruction.code());
				assert!(orig_instr.eq_all_bits(&instruction));
			}
			// V'vvvv[4:3] is ignored in 16/32-bit modes (vvvv[3] if it's a vsib instruction)
			bytes[evex_index + 2] = b2 & !0x40;
			if !is_vsib {
				bytes[evex_index + 3] = b3 & 0xF7;
			}
			if info.bitness() != 64 {
				let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
				let instruction = decoder.decode();
				assert_eq!(info.code(), instruction.code());
				assert!(orig_instr.eq_all_bits(&instruction));
			}
		} else {
			panic!();
		}
	}
}

fn get_vvvvv_info(op_code: &OpCodeInfo) -> (bool, bool, u8) {
	let mut uses_vvvv = false;
	let mut is_vsib = false;
	let mut vvvv_mask = match op_code.encoding() {
		EncodingKind::EVEX => 0x1F,
		EncodingKind::VEX | EncodingKind::XOP => 0xF,
		EncodingKind::Legacy | EncodingKind::D3NOW => panic!(),
	};
	for i in 0..op_code.op_count() {
		match op_code.op_kind(i) {
			OpCodeOperandKind::mem_vsib32x
			| OpCodeOperandKind::mem_vsib64x
			| OpCodeOperandKind::mem_vsib32y
			| OpCodeOperandKind::mem_vsib64y
			| OpCodeOperandKind::mem_vsib32z
			| OpCodeOperandKind::mem_vsib64z => is_vsib = true,
			OpCodeOperandKind::k_vvvv => {
				uses_vvvv = true;
				vvvv_mask = 0x7;
			}
			OpCodeOperandKind::r32_vvvv
			| OpCodeOperandKind::r64_vvvv
			| OpCodeOperandKind::xmm_vvvv
			| OpCodeOperandKind::xmmp3_vvvv
			| OpCodeOperandKind::ymm_vvvv
			| OpCodeOperandKind::zmm_vvvv
			| OpCodeOperandKind::zmmp3_vvvv => uses_vvvv = true,
			_ => {}
		}
	}
	(uses_vvvv, is_vsib, vvvv_mask)
}

#[test]
fn verify_gpr_rrxb_bits() {
	for info in decoder_tests(false, false) {
		if (info.decoder_options() & DecoderOptions::NO_INVALID_CHECK) != 0 {
			continue;
		}

		let op_code = info.code().op_code();

		match op_code.encoding() {
			EncodingKind::Legacy | EncodingKind::D3NOW => continue,
			EncodingKind::VEX | EncodingKind::EVEX | EncodingKind::XOP => {}
		}

		let mut uses_rm = false;
		let mut uses_reg = false;
		let mut other_rm = false;
		let mut other_reg = false;
		for i in 0..op_code.op_count() {
			match op_code.op_kind(i) {
				OpCodeOperandKind::r32_or_mem
				| OpCodeOperandKind::r64_or_mem
				| OpCodeOperandKind::r32_or_mem_mpx
				| OpCodeOperandKind::r64_or_mem_mpx
				| OpCodeOperandKind::r32_rm
				| OpCodeOperandKind::r64_rm => uses_rm = true,
				OpCodeOperandKind::r32_reg | OpCodeOperandKind::r64_reg => uses_reg = true,
				OpCodeOperandKind::k_or_mem
				| OpCodeOperandKind::k_rm
				| OpCodeOperandKind::xmm_or_mem
				| OpCodeOperandKind::ymm_or_mem
				| OpCodeOperandKind::zmm_or_mem
				| OpCodeOperandKind::xmm_rm
				| OpCodeOperandKind::ymm_rm
				| OpCodeOperandKind::zmm_rm => other_rm = true,
				OpCodeOperandKind::k_reg
				| OpCodeOperandKind::kp1_reg
				| OpCodeOperandKind::xmm_reg
				| OpCodeOperandKind::ymm_reg
				| OpCodeOperandKind::zmm_reg => other_reg = true,
				_ => {}
			}
		}
		if !uses_rm && !uses_reg && op_code.op_count() > 0 {
			continue;
		}

		if op_code.encoding() == EncodingKind::VEX || op_code.encoding() == EncodingKind::XOP {
			let mut bytes = to_vec_u8(info.hex_bytes()).unwrap();
			let vex_index = get_vex_xop_index(&bytes);
			let is_vex2 = bytes[vex_index] == 0xC5;
			let mrmi = vex_index + 3 + (if is_vex2 { 0 } else { 1 });
			let is_reg_only = mrmi >= bytes.len() || (bytes[mrmi] >> 6) == 3;
			let b1 = bytes[vex_index + 1];

			let orig_instr;
			{
				let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
				orig_instr = decoder.decode();
				assert_eq!(info.code(), orig_instr.code());
			}
			if uses_rm && !is_vex2 {
				bytes[vex_index + 1] = b1 ^ 0x20;
				{
					let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
					let instruction = decoder.decode();
					assert_eq!(info.code(), instruction.code());
					if is_reg_only && info.bitness() != 64 {
						assert!(orig_instr.eq_all_bits(&instruction));
					}
				}
				bytes[vex_index + 1] = b1 ^ 0x40;
				if info.bitness() == 64 {
					let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
					let instruction = decoder.decode();
					assert_eq!(info.code(), instruction.code());
					if is_reg_only {
						assert!(orig_instr.eq_all_bits(&instruction));
					}
				}
			} else if !other_rm && !is_vex2 {
				bytes[vex_index + 1] = b1 ^ 0x60;
				if info.bitness() != 64 {
					bytes[vex_index + 1] |= 0x40;
				}
				let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
				let instruction = decoder.decode();
				assert_eq!(info.code(), instruction.code());
				assert!(orig_instr.eq_all_bits(&instruction));
			}
			if uses_reg {
				bytes[vex_index + 1] = b1 ^ 0x80;
				if info.bitness() == 64 {
					let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
					let instruction = decoder.decode();
					assert_eq!(info.code(), instruction.code());
					if is_reg_only {
						assert!(!orig_instr.eq_all_bits(&instruction));
					}
				}
			} else if !other_reg {
				bytes[vex_index + 1] = b1 ^ 0x80;
				if info.bitness() != 64 {
					bytes[vex_index + 1] |= 0x80;
				}
				let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
				let instruction = decoder.decode();
				assert_eq!(info.code(), instruction.code());
				assert!(orig_instr.eq_all_bits(&instruction));
			}
		} else if op_code.encoding() == EncodingKind::EVEX {
			let mut bytes = to_vec_u8(info.hex_bytes()).unwrap();
			let evex_index = get_evex_index(&bytes);
			let is_reg_only = (bytes[evex_index + 5] >> 6) == 3;
			let b1 = bytes[evex_index + 1];

			let orig_instr;
			{
				let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
				orig_instr = decoder.decode();
				assert_eq!(info.code(), orig_instr.code());
			}
			if uses_rm {
				bytes[evex_index + 1] = b1 ^ 0x20;
				{
					let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
					let instruction = decoder.decode();
					assert_eq!(info.code(), instruction.code());
					if is_reg_only && info.bitness() != 64 {
						assert!(orig_instr.eq_all_bits(&instruction));
					}
				}
				bytes[evex_index + 1] = b1 ^ 0x40;
				if info.bitness() == 64 {
					let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
					let instruction = decoder.decode();
					assert_eq!(info.code(), instruction.code());
					if is_reg_only {
						assert!(orig_instr.eq_all_bits(&instruction));
					}
				}
			} else if !other_rm {
				bytes[evex_index + 1] = b1 ^ 0x60;
				if info.bitness() != 64 {
					bytes[evex_index + 1] |= 0x40;
				}
				let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
				let instruction = decoder.decode();
				assert_eq!(info.code(), instruction.code());
				assert!(orig_instr.eq_all_bits(&instruction));
			}
			if uses_reg {
				if info.bitness() == 64 {
					bytes[evex_index + 1] = b1 ^ 0x10;
					{
						let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
						let instruction = decoder.decode();
						assert_eq!(Code::INVALID, instruction.code());
						assert!(!decoder.invalid_no_more_bytes());
					}
					{
						let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options() | DecoderOptions::NO_INVALID_CHECK);
						let instruction = decoder.decode();
						assert_eq!(info.code(), instruction.code());
						assert!(orig_instr.eq_all_bits(&instruction));
					}
					bytes[evex_index + 1] = b1 ^ 0x80;
					{
						let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
						let instruction = decoder.decode();
						assert_eq!(info.code(), instruction.code());
					}
				} else {
					bytes[evex_index + 1] = b1 ^ 0x10;
					{
						let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
						let instruction = decoder.decode();
						assert_eq!(info.code(), instruction.code());
						assert!(orig_instr.eq_all_bits(&instruction));
					}
				}
			}
		} else {
			panic!();
		}
	}
}

#[test]
fn verify_k_reg_rrxb_bits() {
	for info in decoder_tests(false, false) {
		if (info.decoder_options() & DecoderOptions::NO_INVALID_CHECK) != 0 {
			continue;
		}

		let op_code = info.code().op_code();

		match op_code.encoding() {
			EncodingKind::Legacy | EncodingKind::D3NOW => continue,
			EncodingKind::VEX | EncodingKind::EVEX | EncodingKind::XOP => {}
		}

		let mut uses_rm = false;
		let mut maybe_uses_rm = false;
		let mut uses_reg = false;
		let mut other_rm = false;
		let mut other_reg = false;
		for i in 0..op_code.op_count() {
			match op_code.op_kind(i) {
				OpCodeOperandKind::mem => maybe_uses_rm = true,
				OpCodeOperandKind::k_or_mem | OpCodeOperandKind::k_rm => uses_rm = true,
				OpCodeOperandKind::k_reg | OpCodeOperandKind::kp1_reg => uses_reg = true,
				OpCodeOperandKind::r32_or_mem
				| OpCodeOperandKind::r64_or_mem
				| OpCodeOperandKind::r32_or_mem_mpx
				| OpCodeOperandKind::r64_or_mem_mpx
				| OpCodeOperandKind::r32_rm
				| OpCodeOperandKind::r64_rm
				| OpCodeOperandKind::xmm_or_mem
				| OpCodeOperandKind::ymm_or_mem
				| OpCodeOperandKind::zmm_or_mem
				| OpCodeOperandKind::xmm_rm
				| OpCodeOperandKind::ymm_rm
				| OpCodeOperandKind::zmm_rm => other_rm = true,
				OpCodeOperandKind::xmm_reg
				| OpCodeOperandKind::ymm_reg
				| OpCodeOperandKind::zmm_reg
				| OpCodeOperandKind::r32_reg
				| OpCodeOperandKind::r64_reg => other_reg = true,
				_ => {}
			}
		}
		if uses_reg && maybe_uses_rm {
			uses_rm = true;
		}
		if !uses_rm && !uses_reg && op_code.op_count() > 0 {
			continue;
		}

		if op_code.encoding() == EncodingKind::VEX || op_code.encoding() == EncodingKind::XOP {
			let mut bytes = to_vec_u8(info.hex_bytes()).unwrap();
			let vex_index = get_vex_xop_index(&bytes);
			let is_vex2 = bytes[vex_index] == 0xC5;
			let mrmi = vex_index + 3 + (if is_vex2 { 0 } else { 1 });
			let is_reg_only = mrmi >= bytes.len() || (bytes[mrmi] >> 6) == 3;
			let b1 = bytes[vex_index + 1];

			let orig_instr;
			{
				let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
				orig_instr = decoder.decode();
				assert_eq!(info.code(), orig_instr.code());
			}
			if uses_rm && !is_vex2 {
				bytes[vex_index + 1] = b1 ^ 0x20;
				{
					let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
					let instruction = decoder.decode();
					assert_eq!(info.code(), instruction.code());
					if is_reg_only && info.bitness() != 64 {
						assert!(orig_instr.eq_all_bits(&instruction));
					}
				}
				bytes[vex_index + 1] = b1 ^ 0x40;
				if info.bitness() == 64 {
					let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
					let instruction = decoder.decode();
					assert_eq!(info.code(), instruction.code());
					if is_reg_only {
						assert!(orig_instr.eq_all_bits(&instruction));
					}
				}
			} else if !other_rm && !is_vex2 {
				bytes[vex_index + 1] = b1 ^ 0x60;
				if info.bitness() != 64 {
					bytes[vex_index + 1] |= 0x40;
				}
				let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
				let instruction = decoder.decode();
				assert_eq!(info.code(), instruction.code());
				assert!(orig_instr.eq_all_bits(&instruction));
			}
			if uses_reg {
				bytes[vex_index + 1] = b1 ^ 0x80;
				if info.bitness() == 64 {
					{
						let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
						let instruction = decoder.decode();
						assert_eq!(Code::INVALID, instruction.code());
						assert!(!decoder.invalid_no_more_bytes());
					}
					{
						let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options() | DecoderOptions::NO_INVALID_CHECK);
						let instruction = decoder.decode();
						assert_eq!(info.code(), instruction.code());
						if is_reg_only {
							assert!(orig_instr.eq_all_bits(&instruction));
						}
					}
				}
			} else if !other_reg {
				bytes[vex_index + 1] = b1 ^ 0x80;
				if info.bitness() != 64 {
					bytes[vex_index + 1] |= 0x80;
				}
				let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
				let instruction = decoder.decode();
				assert_eq!(info.code(), instruction.code());
				assert!(orig_instr.eq_all_bits(&instruction));
			}
		} else if op_code.encoding() == EncodingKind::EVEX {
			let mut bytes = to_vec_u8(info.hex_bytes()).unwrap();
			let evex_index = get_evex_index(&bytes);
			let is_reg_only = (bytes[evex_index + 5] >> 6) == 3;
			let b1 = bytes[evex_index + 1];

			let orig_instr;
			{
				let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
				orig_instr = decoder.decode();
				assert_eq!(info.code(), orig_instr.code());
			}
			if uses_rm {
				bytes[evex_index + 1] = b1 ^ 0x20;
				{
					let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
					let instruction = decoder.decode();
					assert_eq!(info.code(), instruction.code());
					if is_reg_only && info.bitness() != 64 {
						assert!(orig_instr.eq_all_bits(&instruction));
					}
				}
				bytes[evex_index + 1] = b1 ^ 0x40;
				if info.bitness() == 64 {
					let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
					let instruction = decoder.decode();
					assert_eq!(info.code(), instruction.code());
					if is_reg_only {
						assert!(orig_instr.eq_all_bits(&instruction));
					}
				}
			} else if !other_rm {
				bytes[evex_index + 1] = b1 ^ 0x60;
				if info.bitness() != 64 {
					bytes[evex_index + 1] |= 0x40;
				}
				let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
				let instruction = decoder.decode();
				assert_eq!(info.code(), instruction.code());
				assert!(orig_instr.eq_all_bits(&instruction));
			}
			if uses_reg {
				if info.bitness() == 64 {
					bytes[evex_index + 1] = b1 ^ 0x10;
					{
						let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
						let instruction = decoder.decode();
						assert_eq!(Code::INVALID, instruction.code());
						assert!(!decoder.invalid_no_more_bytes());
					}
					{
						let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options() | DecoderOptions::NO_INVALID_CHECK);
						let instruction = decoder.decode();
						assert_eq!(info.code(), instruction.code());
						assert!(orig_instr.eq_all_bits(&instruction));
					}
					bytes[evex_index + 1] = b1 ^ 0x80;
					{
						let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
						let instruction = decoder.decode();
						assert_eq!(Code::INVALID, instruction.code());
						assert!(!decoder.invalid_no_more_bytes());
					}
					{
						let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options() | DecoderOptions::NO_INVALID_CHECK);
						let instruction = decoder.decode();
						assert_eq!(info.code(), instruction.code());
						assert!(orig_instr.eq_all_bits(&instruction));
					}
				} else {
					bytes[evex_index + 1] = b1 ^ 0x10;
					{
						let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
						let instruction = decoder.decode();
						assert_eq!(info.code(), instruction.code());
						assert!(orig_instr.eq_all_bits(&instruction));
					}
				}
			}
		} else {
			panic!();
		}
	}
}

#[test]
#[allow(unused_mut)]
fn verify_vsib_with_invalid_index_register_evex() {
	let mut code_values: HashSet<Code> = HashSet::new();
	#[cfg(not(feature = "no_evex"))]
	{
		let _ = code_values.insert(Code::EVEX_Vpgatherdd_xmm_k1_vm32x);
		let _ = code_values.insert(Code::EVEX_Vpgatherdd_ymm_k1_vm32y);
		let _ = code_values.insert(Code::EVEX_Vpgatherdd_zmm_k1_vm32z);
		let _ = code_values.insert(Code::EVEX_Vpgatherdq_xmm_k1_vm32x);
		let _ = code_values.insert(Code::EVEX_Vpgatherdq_ymm_k1_vm32x);
		let _ = code_values.insert(Code::EVEX_Vpgatherdq_zmm_k1_vm32y);
		let _ = code_values.insert(Code::EVEX_Vpgatherqd_xmm_k1_vm64x);
		let _ = code_values.insert(Code::EVEX_Vpgatherqd_xmm_k1_vm64y);
		let _ = code_values.insert(Code::EVEX_Vpgatherqd_ymm_k1_vm64z);
		let _ = code_values.insert(Code::EVEX_Vpgatherqq_xmm_k1_vm64x);
		let _ = code_values.insert(Code::EVEX_Vpgatherqq_ymm_k1_vm64y);
		let _ = code_values.insert(Code::EVEX_Vpgatherqq_zmm_k1_vm64z);
		let _ = code_values.insert(Code::EVEX_Vgatherdps_xmm_k1_vm32x);
		let _ = code_values.insert(Code::EVEX_Vgatherdps_ymm_k1_vm32y);
		let _ = code_values.insert(Code::EVEX_Vgatherdps_zmm_k1_vm32z);
		let _ = code_values.insert(Code::EVEX_Vgatherdpd_xmm_k1_vm32x);
		let _ = code_values.insert(Code::EVEX_Vgatherdpd_ymm_k1_vm32x);
		let _ = code_values.insert(Code::EVEX_Vgatherdpd_zmm_k1_vm32y);
		let _ = code_values.insert(Code::EVEX_Vgatherqps_xmm_k1_vm64x);
		let _ = code_values.insert(Code::EVEX_Vgatherqps_xmm_k1_vm64y);
		let _ = code_values.insert(Code::EVEX_Vgatherqps_ymm_k1_vm64z);
		let _ = code_values.insert(Code::EVEX_Vgatherqpd_xmm_k1_vm64x);
		let _ = code_values.insert(Code::EVEX_Vgatherqpd_ymm_k1_vm64y);
		let _ = code_values.insert(Code::EVEX_Vgatherqpd_zmm_k1_vm64z);
	}
	for info in decoder_tests(false, false) {
		if (info.decoder_options() & DecoderOptions::NO_INVALID_CHECK) != 0 {
			continue;
		}
		let op_code = info.code().op_code();
		assert_eq!(can_have_invalid_index_register_evex(op_code), code_values.contains(&info.code()));
		if !can_have_invalid_index_register_evex(op_code) {
			continue;
		}

		if op_code.encoding() == EncodingKind::EVEX {
			let mut bytes = to_vec_u8(info.hex_bytes()).unwrap();
			let evex_index = get_evex_index(&bytes);
			let p0 = bytes[evex_index + 1];
			let p2 = bytes[evex_index + 3];
			let m = bytes[evex_index + 5];
			let s = bytes[evex_index + 6];
			for i in 0..32 {
				let reg_num = if info.bitness() == 64 { i } else { i & 7 } as u32;
				let t = i ^ 0x1F;
				// reg  = R' R modrm.reg
				// vidx = V' X sib.index
				bytes[evex_index + 1] = (p0 & !0xD0) | /*R'*/(t & 0x10) | /*R*/((t & 0x08) << 4) | /*X*/((t & 0x08) << 3);
				if info.bitness() != 64 {
					bytes[evex_index + 1] |= 0xC0;
				}
				bytes[evex_index + 3] = (p2 & !0x08) | /*V'*/((t & 0x10) >> 1);
				bytes[evex_index + 5] = (m & 0xC7) | /*modrm.reg*/((i & 7) << 3);
				bytes[evex_index + 6] = (s & 0xC7) | /*sib.index*/((i & 7) << 3);

				{
					let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
					let instruction = decoder.decode();
					assert_eq!(Code::INVALID, instruction.code());
					assert!(!decoder.invalid_no_more_bytes());
				}
				{
					let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options() | DecoderOptions::NO_INVALID_CHECK);
					let instruction = decoder.decode();
					assert_eq!(info.code(), instruction.code());
					assert_eq!(OpKind::Register, instruction.op0_kind());
					assert_eq!(OpKind::Memory, instruction.op1_kind());
					assert_ne!(Register::None, instruction.memory_index());
					assert_eq!(reg_num, reg_number(instruction.op0_register()));
					assert_eq!(reg_num, reg_number(instruction.memory_index()));
				}
			}
		} else {
			panic!();
		}
	}
}

// All Vk_VSIB instructions, eg. EVEX_Vpgatherdd_xmm_k1_vm32x
fn can_have_invalid_index_register_evex(op_code: &OpCodeInfo) -> bool {
	if op_code.encoding() != EncodingKind::EVEX {
		return false;
	}

	match op_code.op0_kind() {
		OpCodeOperandKind::xmm_reg | OpCodeOperandKind::ymm_reg | OpCodeOperandKind::zmm_reg => {}
		_ => return false,
	}

	for i in 1..op_code.op_count() {
		match op_code.op_kind(i) {
			OpCodeOperandKind::mem_vsib32x
			| OpCodeOperandKind::mem_vsib32y
			| OpCodeOperandKind::mem_vsib32z
			| OpCodeOperandKind::mem_vsib64x
			| OpCodeOperandKind::mem_vsib64y
			| OpCodeOperandKind::mem_vsib64z => {
				return true;
			}
			_ => {}
		}
	}
	false
}

#[test]
#[allow(unused_mut)]
fn verify_vsib_with_invalid_index_mask_dest_register_vex() {
	let mut code_values: HashSet<Code> = HashSet::new();
	#[cfg(not(feature = "no_vex"))]
	{
		let _ = code_values.insert(Code::VEX_Vpgatherdd_xmm_vm32x_xmm);
		let _ = code_values.insert(Code::VEX_Vpgatherdd_ymm_vm32y_ymm);
		let _ = code_values.insert(Code::VEX_Vpgatherdq_xmm_vm32x_xmm);
		let _ = code_values.insert(Code::VEX_Vpgatherdq_ymm_vm32x_ymm);
		let _ = code_values.insert(Code::VEX_Vpgatherqd_xmm_vm64x_xmm);
		let _ = code_values.insert(Code::VEX_Vpgatherqd_xmm_vm64y_xmm);
		let _ = code_values.insert(Code::VEX_Vpgatherqq_xmm_vm64x_xmm);
		let _ = code_values.insert(Code::VEX_Vpgatherqq_ymm_vm64y_ymm);
		let _ = code_values.insert(Code::VEX_Vgatherdps_xmm_vm32x_xmm);
		let _ = code_values.insert(Code::VEX_Vgatherdps_ymm_vm32y_ymm);
		let _ = code_values.insert(Code::VEX_Vgatherdpd_xmm_vm32x_xmm);
		let _ = code_values.insert(Code::VEX_Vgatherdpd_ymm_vm32x_ymm);
		let _ = code_values.insert(Code::VEX_Vgatherqps_xmm_vm64x_xmm);
		let _ = code_values.insert(Code::VEX_Vgatherqps_xmm_vm64y_xmm);
		let _ = code_values.insert(Code::VEX_Vgatherqpd_xmm_vm64x_xmm);
		let _ = code_values.insert(Code::VEX_Vgatherqpd_ymm_vm64y_ymm);
	}
	for info in decoder_tests(false, false) {
		if (info.decoder_options() & DecoderOptions::NO_INVALID_CHECK) != 0 {
			continue;
		}
		let op_code = info.code().op_code();
		assert_eq!(can_have_invalid_index_mask_dest_register_vex(op_code), code_values.contains(&info.code()));
		if !can_have_invalid_index_mask_dest_register_vex(op_code) {
			continue;
		}

		if op_code.encoding() == EncodingKind::VEX || op_code.encoding() == EncodingKind::XOP {
			let mut bytes = to_vec_u8(info.hex_bytes()).unwrap();
			let vex_index = get_vex_xop_index(&bytes);

			let is_vex2 = bytes[vex_index] == 0xC5;
			let r_index = vex_index + 1;
			let v_index = if is_vex2 { r_index } else { r_index + 1 };
			let m_index = v_index + 2;
			let s_index = v_index + 3;

			let r = bytes[r_index];
			let v = bytes[v_index];
			let m = bytes[m_index];
			let s = bytes[s_index];

			#[derive(Copy, Clone)]
			#[allow(non_camel_case_types)]
			enum TestKind {
				reg_eq_vvvv,
				reg_eq_vidx,
				vvvv_eq_vidx,
				all_eq_all,
			}
			for &test_kind in &[TestKind::reg_eq_vvvv, TestKind::reg_eq_vidx, TestKind::vvvv_eq_vidx, TestKind::all_eq_all] {
				for i in 0..16 {
					let reg_num: u32 = if info.bitness() == 64 { i } else { i & 7 };
					// Use a small number (0-7) in case it's vex2 and 'other' is vidx (uses VEX.X bit)
					let other = if reg_num == 0 { 1 } else { 0 };
					let new_reg;
					let new_vvvv;
					let new_vidx;

					match test_kind {
						TestKind::reg_eq_vvvv => {
							new_vvvv = reg_num;
							new_reg = reg_num;
							new_vidx = other;
						}
						TestKind::reg_eq_vidx => {
							new_vidx = reg_num;
							new_reg = reg_num;
							new_vvvv = other;
						}
						TestKind::vvvv_eq_vidx => {
							new_vidx = reg_num;
							new_vvvv = reg_num;
							new_reg = other;
						}
						TestKind::all_eq_all => {
							new_vidx = reg_num;
							new_vvvv = reg_num;
							new_reg = reg_num;
						}
					}

					// reg  = R modrm.reg
					// vidx = X sib.index
					if is_vex2 {
						if new_vidx >= 8 {
							continue;
						}
						bytes[r_index] = (r & 0x07) | /*R*/((((new_reg as u8) ^ 8) & 0x8) << 4) | /*vvvv*/(((new_vvvv as u8) ^ 0xF) << 3);
					} else {
						bytes[r_index] = (r & 0x3F) | /*R*/((((new_reg as u8) ^ 8) & 8) << 4) | /*X*/((((new_vidx as u8) ^ 8) & 8) << 3);
						bytes[v_index] = (v & 0x87) | /*vvvv*/(((new_vvvv as u8) ^ 0xF) << 3);
					}
					bytes[m_index] = (m & 0xC7) | /*modrm.reg*/(((new_reg as u8) & 7) << 3);
					bytes[s_index] = (s & 0xC7) | /*sib.index*/(((new_vidx as u8) & 7) << 3);

					{
						let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
						let instruction = decoder.decode();
						assert_eq!(Code::INVALID, instruction.code());
						assert!(!decoder.invalid_no_more_bytes());
					}
					{
						let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options() | DecoderOptions::NO_INVALID_CHECK);
						let instruction = decoder.decode();
						assert_eq!(info.code(), instruction.code());
						assert_eq!(OpKind::Register, instruction.op0_kind());
						assert_eq!(OpKind::Memory, instruction.op1_kind());
						assert_eq!(OpKind::Register, instruction.op2_kind());
						assert_ne!(Register::None, instruction.memory_index());
						assert_eq!(new_reg, reg_number(instruction.op0_register()));
						assert_eq!(new_vidx, reg_number(instruction.memory_index()));
						assert_eq!(new_vvvv, reg_number(instruction.op2_register()));
					}
				}
			}
		} else {
			panic!();
		}
	}
}

// All VX_VSIB_HX instructions, eg. VEX_Vpgatherdd_xmm_vm32x_xmm
fn can_have_invalid_index_mask_dest_register_vex(op_code: &OpCodeInfo) -> bool {
	if op_code.encoding() != EncodingKind::VEX && op_code.encoding() != EncodingKind::XOP {
		return false;
	}
	if op_code.op_count() != 3 {
		return false;
	}

	match op_code.op0_kind() {
		OpCodeOperandKind::xmm_reg | OpCodeOperandKind::ymm_reg | OpCodeOperandKind::zmm_reg => {}
		_ => return false,
	}

	match op_code.op2_kind() {
		OpCodeOperandKind::xmm_vvvv | OpCodeOperandKind::ymm_vvvv | OpCodeOperandKind::zmm_vvvv => {}
		_ => return false,
	}

	match op_code.op1_kind() {
		OpCodeOperandKind::mem_vsib32x
		| OpCodeOperandKind::mem_vsib32y
		| OpCodeOperandKind::mem_vsib32z
		| OpCodeOperandKind::mem_vsib64x
		| OpCodeOperandKind::mem_vsib64y
		| OpCodeOperandKind::mem_vsib64z => {}
		_ => return false,
	}

	true
}

fn is_vsib(op_code: &OpCodeInfo) -> bool {
	get_vsib(op_code).is_some()
}

fn get_vsib(op_code: &OpCodeInfo) -> Option<(bool, bool)> {
	for i in 0..op_code.op_count() {
		match op_code.op_kind(i) {
			OpCodeOperandKind::mem_vsib32x | OpCodeOperandKind::mem_vsib32y | OpCodeOperandKind::mem_vsib32z => return Some((true, false)),
			OpCodeOperandKind::mem_vsib64x | OpCodeOperandKind::mem_vsib64y | OpCodeOperandKind::mem_vsib64z => return Some((false, true)),
			_ => {}
		}
	}

	None
}

#[test]
fn test_vsib_props() {
	for info in decoder_tests(false, false) {
		let bytes = to_vec_u8(info.hex_bytes()).unwrap();
		let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
		let instruction = decoder.decode();
		assert_eq!(info.code(), instruction.code());

		let (is_vsib, is_vsib32, is_vsib64) =
			if let Some((is_vsib32, is_vsib64)) = get_vsib(info.code().op_code()) { (true, is_vsib32, is_vsib64) } else { (false, false, false) };
		assert_eq!(instruction.is_vsib(), is_vsib);
		assert_eq!(instruction.is_vsib32(), is_vsib32);
		assert_eq!(instruction.is_vsib64(), is_vsib64);
	}
}

#[derive(Clone, Default)]
struct TestedInfo {
	// VEX/XOP.L and EVEX.L'L values
	l_bits: u32, // bit 0 = L0/L128, bit 1 = L1/L256, etc
	vex2_l_bits: u32,

	// REX/VEX/XOP/EVEX/MVEX: W values
	w_bits: u32, // bit 0 = W0, bit 1 = W1

	// REX/VEX/XOP/EVEX/MVEX.R
	r_bits: u32,
	vex2_r_bits: u32,
	// REX/VEX/XOP/EVEX/MVEX.X
	x_bits: u32,
	// REX/VEX/XOP/EVEX/MVEX.B
	b_bits: u32,
	// EVEX/MVEX.R'
	r2_bits: u32,
	// EVEX/MVEX.V'
	v2_bits: u32,

	// mod=11
	reg_reg: bool,
	// mod=00,01,10
	reg_mem: bool,

	// EVEX/MVEX only
	mem_disp8: bool,

	// Tested vex2 prefix
	vex2: bool,
	// Tested vex3 prefix
	vex3: bool,

	// EVEX/MVEX: tested opmask
	op_mask: bool,
	// EVEX/MVEX: tested no opmask
	no_op_mask: bool,

	prefix_xacquire: bool,
	prefix_no_xacquire: bool,
	prefix_xrelease: bool,
	prefix_no_xrelease: bool,
	prefix_lock: bool,
	prefix_no_lock: bool,
	prefix_hnt: bool,
	prefix_no_hnt: bool,
	prefix_ht: bool,
	prefix_no_ht: bool,
	prefix_rep: bool,
	prefix_no_rep: bool,
	prefix_repne: bool,
	prefix_no_repne: bool,
	prefix_notrack: bool,
	prefix_no_notrack: bool,
	prefix_bnd: bool,
	prefix_no_bnd: bool,
}

#[test]
#[cfg_attr(feature = "cargo-clippy", allow(clippy::needless_range_loop))]
fn verify_that_test_cases_test_enough_bits() {
	let mut tested_infos_16: Vec<TestedInfo> = iter::repeat(TestedInfo::default()).take(IcedConstants::NUMBER_OF_CODE_VALUES).collect();
	let mut tested_infos_32: Vec<TestedInfo> = iter::repeat(TestedInfo::default()).take(IcedConstants::NUMBER_OF_CODE_VALUES).collect();
	let mut tested_infos_64: Vec<TestedInfo> = iter::repeat(TestedInfo::default()).take(IcedConstants::NUMBER_OF_CODE_VALUES).collect();

	let mut can_use_w = [false; IcedConstants::NUMBER_OF_CODE_VALUES];
	{
		let mut uses_w: HashSet<(OpCodeTableKind, u32)> = HashSet::new();
		for i in 0..IcedConstants::NUMBER_OF_CODE_VALUES {
			let code: Code = unsafe { mem::transmute(i as u16) };
			let op_code = code.op_code();
			if op_code.encoding() != EncodingKind::Legacy {
				continue;
			}
			if op_code.operand_size() != 0 {
				let _ = uses_w.insert((op_code.table(), op_code.op_code()));
			}
		}
		for i in 0..IcedConstants::NUMBER_OF_CODE_VALUES {
			let code: Code = unsafe { mem::transmute(i as u16) };
			let op_code = code.op_code();
			match op_code.encoding() {
				EncodingKind::Legacy | EncodingKind::D3NOW => can_use_w[i] = !uses_w.contains(&(op_code.table(), op_code.op_code())),
				EncodingKind::VEX | EncodingKind::EVEX | EncodingKind::XOP => {}
			}
		}
	}

	for info in decoder_tests(false, false) {
		if (info.decoder_options() & DecoderOptions::NO_INVALID_CHECK) != 0 {
			continue;
		}
		let tested_infos: &mut [TestedInfo] = match info.bitness() {
			16 => &mut tested_infos_16,
			32 => &mut tested_infos_32,
			64 => &mut tested_infos_64,
			_ => panic!(),
		};

		let op_code = info.code().op_code();
		let tested = &mut tested_infos[info.code() as usize];

		let bytes = to_vec_u8(info.hex_bytes()).unwrap();
		let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
		let instruction = decoder.decode();
		assert_eq!(info.code(), instruction.code());

		if op_code.encoding() == EncodingKind::EVEX {
			let evex_index = get_evex_index(&bytes);
			if instruction.rounding_control() == RoundingControl::None {
				tested.l_bits |= 1 << ((bytes[evex_index + 3] >> 5) & 3);
			}

			let ll = (bytes[evex_index + 3] >> 5) & 3;
			let invalid = (info.decoder_options() & DecoderOptions::NO_INVALID_CHECK) == 0
				&& ll == 3 && (bytes[evex_index + 5] < 0xC0 || (bytes[evex_index + 3] & 0x10) == 0);
			if !invalid {
				tested.l_bits |= 1 << 3;
			}

			tested.w_bits |= 1 << (bytes[evex_index + 2] >> 7);
			tested.r_bits |= 1 << ((bytes[evex_index + 1] >> 7) ^ 1);
			tested.x_bits |= 1 << (((bytes[evex_index + 1] >> 6) & 1) ^ 1);
			tested.b_bits |= 1 << (((bytes[evex_index + 1] >> 5) & 1) ^ 1);
			tested.r2_bits |= 1 << (((bytes[evex_index + 1] >> 4) & 1) ^ 1);
			tested.v2_bits |= 1 << (((bytes[evex_index + 3] >> 3) & 1) ^ 1);
			if (bytes[evex_index + 5] >> 6) != 3 {
				tested.reg_mem = true;
				if instruction.memory_displ_size() == 1 && instruction.memory_displacement() != 0 {
					tested.mem_disp8 = true;
				}
			} else {
				tested.reg_reg = true;
			}
			if instruction.op_mask() != Register::None {
				tested.op_mask = true;
			} else {
				tested.no_op_mask = true;
			}
		} else if op_code.encoding() == EncodingKind::VEX || op_code.encoding() == EncodingKind::XOP {
			let vex_index = get_vex_xop_index(&bytes);
			let mrmi;
			if bytes[vex_index] == 0xC5 {
				mrmi = vex_index + 3;
				tested.vex2 = true;
				tested.vex2_r_bits |= 1 << ((bytes[vex_index + 1] >> 7) ^ 1);
				tested.vex2_l_bits |= 1 << ((bytes[vex_index + 1] >> 2) & 1);
			} else {
				mrmi = vex_index + 4;
				if op_code.encoding() == EncodingKind::VEX {
					tested.vex3 = true;
				}
				tested.r_bits |= 1 << ((bytes[vex_index + 1] >> 7) ^ 1);
				tested.x_bits |= 1 << (((bytes[vex_index + 1] >> 6) & 1) ^ 1);
				tested.b_bits |= 1 << (((bytes[vex_index + 1] >> 5) & 1) ^ 1);
				tested.w_bits |= 1 << (bytes[vex_index + 2] >> 7);
				tested.l_bits |= 1 << ((bytes[vex_index + 2] >> 2) & 1);
			}
			if has_modrm(op_code) {
				if (bytes[mrmi] >> 6) != 3 {
					tested.reg_mem = true;
				} else {
					tested.reg_reg = true;
				}
			}
		} else if op_code.encoding() == EncodingKind::Legacy || op_code.encoding() == EncodingKind::D3NOW {
			let (mut i, rex) = skip_prefixes(&bytes, info.bitness());
			if info.bitness() == 64 {
				tested.w_bits |= 1 << ((rex >> 3) & 1);
				tested.r_bits |= 1 << ((rex >> 2) & 1);
				tested.x_bits |= 1 << ((rex >> 1) & 1);
				tested.b_bits |= 1 << (rex & 1);
				// Can't access regs dr8-dr15
				if info.code() == Code::Mov_r64_dr || info.code() == Code::Mov_dr_r64 {
					tested.r_bits |= 1 << 1;
				}
			} else {
				tested.w_bits |= 1;
				tested.r_bits |= 1;
				tested.x_bits |= 1;
				tested.b_bits |= 1;
			}
			if has_modrm(op_code) {
				match op_code.table() {
					OpCodeTableKind::Normal => {}
					OpCodeTableKind::T0F => {
						if bytes[i] != 0x0F {
							panic!();
						}
						i += 1;
					}
					OpCodeTableKind::T0F38 => {
						if bytes[i] != 0x0F {
							panic!();
						}
						i += 1;
						if bytes[i] != 0x38 {
							panic!();
						}
						i += 1;
					}
					OpCodeTableKind::T0F3A => {
						if bytes[i] != 0x0F {
							panic!();
						}
						i += 1;
						if bytes[i] != 0x3A {
							panic!();
						}
						i += 1;
					}
					_ => {}
				}
				i += 1;
				if (bytes[i] >> 6) != 3 {
					tested.reg_mem = true;
				} else {
					tested.reg_reg = true;
				}
			}
			if op_code.can_use_xacquire_prefix() {
				if instruction.has_xacquire_prefix() {
					tested.prefix_xacquire = true;
				} else {
					tested.prefix_no_xacquire = true;
				}
			}
			if op_code.can_use_xrelease_prefix() {
				if instruction.has_xrelease_prefix() {
					tested.prefix_xrelease = true;
				} else {
					tested.prefix_no_xrelease = true;
				}
			}
			if op_code.can_use_lock_prefix() {
				if instruction.has_lock_prefix() {
					tested.prefix_lock = true;
				} else {
					tested.prefix_no_lock = true;
				}
			}
			if op_code.can_use_hint_taken_prefix() {
				if instruction.segment_prefix() == Register::CS {
					tested.prefix_hnt = true;
				} else {
					tested.prefix_no_hnt = true;
				}
			}
			if op_code.can_use_hint_taken_prefix() {
				if instruction.segment_prefix() == Register::DS {
					tested.prefix_ht = true;
				} else {
					tested.prefix_no_ht = true;
				}
			}
			if op_code.can_use_rep_prefix() {
				if instruction.has_rep_prefix() {
					tested.prefix_rep = true;
				} else {
					tested.prefix_no_rep = true;
				}
			}
			if op_code.can_use_repne_prefix() {
				if instruction.has_repne_prefix() {
					tested.prefix_repne = true;
				} else {
					tested.prefix_no_repne = true;
				}
			}
			if op_code.can_use_notrack_prefix() {
				if instruction.segment_prefix() == Register::DS {
					tested.prefix_notrack = true;
				} else {
					tested.prefix_no_notrack = true;
				}
			}
			if op_code.can_use_bnd_prefix() {
				if instruction.has_repne_prefix() {
					tested.prefix_bnd = true;
				} else {
					tested.prefix_no_bnd = true;
				}
			}
		} else {
			panic!();
		}
	}

	let mut wig32_16: Vec<Code> = Vec::new();
	let mut wig32_32: Vec<Code> = Vec::new();

	let mut wig_16: Vec<Code> = Vec::new();
	let mut wig_32: Vec<Code> = Vec::new();
	let mut wig_64: Vec<Code> = Vec::new();

	let mut w_64: Vec<Code> = Vec::new();

	let mut lig_16: Vec<Code> = Vec::new();
	let mut lig_32: Vec<Code> = Vec::new();
	let mut lig_64: Vec<Code> = Vec::new();

	let mut vex2_lig_16: Vec<Code> = Vec::new();
	let mut vex2_lig_32: Vec<Code> = Vec::new();
	let mut vex2_lig_64: Vec<Code> = Vec::new();

	let mut rr_16: Vec<Code> = Vec::new();
	let mut rr_32: Vec<Code> = Vec::new();
	let mut rr_64: Vec<Code> = Vec::new();

	let mut rm_16: Vec<Code> = Vec::new();
	let mut rm_32: Vec<Code> = Vec::new();
	let mut rm_64: Vec<Code> = Vec::new();

	let mut disp8_16: Vec<Code> = Vec::new();
	let mut disp8_32: Vec<Code> = Vec::new();
	let mut disp8_64: Vec<Code> = Vec::new();

	let mut vex2_16: Vec<Code> = Vec::new();
	let mut vex2_32: Vec<Code> = Vec::new();
	let mut vex2_64: Vec<Code> = Vec::new();

	let mut vex3_16: Vec<Code> = Vec::new();
	let mut vex3_32: Vec<Code> = Vec::new();
	let mut vex3_64: Vec<Code> = Vec::new();

	let mut opmask_16: Vec<Code> = Vec::new();
	let mut opmask_32: Vec<Code> = Vec::new();
	let mut opmask_64: Vec<Code> = Vec::new();

	let mut noopmask_16: Vec<Code> = Vec::new();
	let mut noopmask_32: Vec<Code> = Vec::new();
	let mut noopmask_64: Vec<Code> = Vec::new();

	let mut b_16: Vec<Code> = Vec::new();
	let mut b_32: Vec<Code> = Vec::new();
	let mut b_64: Vec<Code> = Vec::new();

	let mut r2_16: Vec<Code> = Vec::new();
	let mut r2_32: Vec<Code> = Vec::new();
	let mut r2_64: Vec<Code> = Vec::new();

	let mut r_64: Vec<Code> = Vec::new();
	let mut vex2_r_64: Vec<Code> = Vec::new();
	let mut x_64: Vec<Code> = Vec::new();
	let mut v2_64: Vec<Code> = Vec::new();

	let mut pfx_xacquire_16: Vec<Code> = Vec::new();
	let mut pfx_xacquire_32: Vec<Code> = Vec::new();
	let mut pfx_xacquire_64: Vec<Code> = Vec::new();

	let mut pfx_xrelease_16: Vec<Code> = Vec::new();
	let mut pfx_xrelease_32: Vec<Code> = Vec::new();
	let mut pfx_xrelease_64: Vec<Code> = Vec::new();

	let mut pfx_lock_16: Vec<Code> = Vec::new();
	let mut pfx_lock_32: Vec<Code> = Vec::new();
	let mut pfx_lock_64: Vec<Code> = Vec::new();

	let mut pfx_hnt_16: Vec<Code> = Vec::new();
	let mut pfx_hnt_32: Vec<Code> = Vec::new();
	let mut pfx_hnt_64: Vec<Code> = Vec::new();

	let mut pfx_ht_16: Vec<Code> = Vec::new();
	let mut pfx_ht_32: Vec<Code> = Vec::new();
	let mut pfx_ht_64: Vec<Code> = Vec::new();

	let mut pfx_rep_16: Vec<Code> = Vec::new();
	let mut pfx_rep_32: Vec<Code> = Vec::new();
	let mut pfx_rep_64: Vec<Code> = Vec::new();

	let mut pfx_repne_16: Vec<Code> = Vec::new();
	let mut pfx_repne_32: Vec<Code> = Vec::new();
	let mut pfx_repne_64: Vec<Code> = Vec::new();

	let mut pfx_notrack_16: Vec<Code> = Vec::new();
	let mut pfx_notrack_32: Vec<Code> = Vec::new();
	let mut pfx_notrack_64: Vec<Code> = Vec::new();

	let mut pfx_bnd_16: Vec<Code> = Vec::new();
	let mut pfx_bnd_32: Vec<Code> = Vec::new();
	let mut pfx_bnd_64: Vec<Code> = Vec::new();

	let mut pfx_no_xacquire_16: Vec<Code> = Vec::new();
	let mut pfx_no_xacquire_32: Vec<Code> = Vec::new();
	let mut pfx_no_xacquire_64: Vec<Code> = Vec::new();

	let mut pfx_no_xrelease_16: Vec<Code> = Vec::new();
	let mut pfx_no_xrelease_32: Vec<Code> = Vec::new();
	let mut pfx_no_xrelease_64: Vec<Code> = Vec::new();

	let mut pfx_no_lock_16: Vec<Code> = Vec::new();
	let mut pfx_no_lock_32: Vec<Code> = Vec::new();
	let mut pfx_no_lock_64: Vec<Code> = Vec::new();

	let mut pfx_no_hnt_16: Vec<Code> = Vec::new();
	let mut pfx_no_hnt_32: Vec<Code> = Vec::new();
	let mut pfx_no_hnt_64: Vec<Code> = Vec::new();

	let mut pfx_no_ht_16: Vec<Code> = Vec::new();
	let mut pfx_no_ht_32: Vec<Code> = Vec::new();
	let mut pfx_no_ht_64: Vec<Code> = Vec::new();

	let mut pfx_no_rep_16: Vec<Code> = Vec::new();
	let mut pfx_no_rep_32: Vec<Code> = Vec::new();
	let mut pfx_no_rep_64: Vec<Code> = Vec::new();

	let mut pfx_no_repne_16: Vec<Code> = Vec::new();
	let mut pfx_no_repne_32: Vec<Code> = Vec::new();
	let mut pfx_no_repne_64: Vec<Code> = Vec::new();

	let mut pfx_no_notrack_16: Vec<Code> = Vec::new();
	let mut pfx_no_notrack_32: Vec<Code> = Vec::new();
	let mut pfx_no_notrack_64: Vec<Code> = Vec::new();

	let mut pfx_no_bnd_16: Vec<Code> = Vec::new();
	let mut pfx_no_bnd_32: Vec<Code> = Vec::new();
	let mut pfx_no_bnd_64: Vec<Code> = Vec::new();

	let code_names = code_names();
	for &bitness in &[16u32, 32, 64] {
		let tested_infos: &[TestedInfo] = match bitness {
			16 => &tested_infos_16,
			32 => &tested_infos_32,
			64 => &tested_infos_64,
			_ => panic!(),
		};

		for i in 0..IcedConstants::NUMBER_OF_CODE_VALUES {
			if is_ignored_code(code_names[i]) {
				continue;
			}
			let code: Code = unsafe { mem::transmute(i as u16) };
			let op_code = code.op_code();
			if !op_code.is_instruction() || op_code.code() == Code::Popw_CS {
				continue;
			}
			if op_code.fwait() {
				continue;
			}

			match bitness {
				16 => {
					if !op_code.mode16() {
						continue;
					}
				}
				32 => {
					if !op_code.mode32() {
						continue;
					}
				}
				64 => {
					if !op_code.mode64() {
						continue;
					}
				}
				_ => panic!(),
			}

			let tested = &tested_infos[i];

			if (bitness == 16 || bitness == 32) && op_code.is_wig32() {
				if tested.w_bits != 3 {
					get_vec2(bitness, &mut wig32_16, &mut wig32_32).push(code);
				}
			}
			if op_code.is_wig() {
				if tested.w_bits != 3 {
					get_vec(bitness, &mut wig_16, &mut wig_32, &mut wig_64).push(code);
				}
			}
			if bitness == 64 && op_code.mode64() && (op_code.encoding() == EncodingKind::Legacy || op_code.encoding() == EncodingKind::D3NOW) {
				debug_assert!(!op_code.is_wig());
				debug_assert!(!op_code.is_wig32());
				if can_use_w[code as usize] && tested.w_bits != 3 {
					w_64.push(code);
				}
			}
			if op_code.is_lig() {
				let all_l_bits;
				match op_code.encoding() {
					EncodingKind::VEX | EncodingKind::XOP => all_l_bits = 3, // 1 bit = 2 values
					EncodingKind::EVEX => all_l_bits = 0xF,                  // 2 bits = 4 values
					EncodingKind::Legacy | EncodingKind::D3NOW => panic!(),
				}
				if tested.l_bits != all_l_bits {
					get_vec(bitness, &mut lig_16, &mut lig_32, &mut lig_64).push(code);
				}
			}
			if op_code.is_lig() && op_code.encoding() == EncodingKind::VEX {
				if tested.vex2_l_bits != 3 && can_use_vex2(op_code) {
					get_vec(bitness, &mut vex2_lig_16, &mut vex2_lig_32, &mut vex2_lig_64).push(code);
				}
			}
			if can_use_modrm_rm_mem(op_code) {
				if !tested.reg_mem {
					get_vec(bitness, &mut rm_16, &mut rm_32, &mut rm_64).push(code);
				}
			}
			if can_use_modrm_rm_reg(op_code) {
				if !tested.reg_reg {
					get_vec(bitness, &mut rr_16, &mut rr_32, &mut rr_64).push(code);
				}
			}
			match op_code.encoding() {
				EncodingKind::Legacy | EncodingKind::VEX | EncodingKind::XOP | EncodingKind::D3NOW => {}
				EncodingKind::EVEX => {
					if !tested.mem_disp8 && can_use_modrm_rm_mem(op_code) {
						get_vec(bitness, &mut disp8_16, &mut disp8_32, &mut disp8_64).push(code);
					}
				}
			}
			if op_code.encoding() == EncodingKind::VEX {
				if !tested.vex3 {
					get_vec(bitness, &mut vex3_16, &mut vex3_32, &mut vex3_64).push(code);
				}
				if !tested.vex2 && can_use_vex2(op_code) {
					get_vec(bitness, &mut vex2_16, &mut vex2_32, &mut vex2_64).push(code);
				}
			}
			if op_code.can_use_op_mask_register() {
				if !tested.op_mask {
					get_vec(bitness, &mut opmask_16, &mut opmask_32, &mut opmask_64).push(code);
				}
				if !tested.no_op_mask && !op_code.require_non_zero_op_mask_register() {
					get_vec(bitness, &mut noopmask_16, &mut noopmask_32, &mut noopmask_64).push(code);
				}
			}
			if can_use_b(bitness, op_code) {
				if tested.b_bits != 3 {
					get_vec(bitness, &mut b_16, &mut b_32, &mut b_64).push(code);
				}
			} else {
				if (tested.b_bits & 1) == 0 {
					get_vec(bitness, &mut b_16, &mut b_32, &mut b_64).push(code);
				}
			}
			match op_code.encoding() {
				EncodingKind::EVEX => {
					if can_use_r2(op_code) {
						if tested.r2_bits != 3 {
							get_vec(bitness, &mut r2_16, &mut r2_32, &mut r2_64).push(code);
						}
					} else {
						if (tested.r2_bits & 1) == 0 {
							get_vec(bitness, &mut r2_16, &mut r2_32, &mut r2_64).push(code);
						}
					}
				}
				EncodingKind::Legacy | EncodingKind::VEX | EncodingKind::XOP | EncodingKind::D3NOW => {}
			}
			if bitness == 64 && op_code.mode64() {
				if tested.vex2_r_bits != 3 && op_code.encoding() == EncodingKind::VEX && can_use_vex2(op_code) && can_use_r(op_code) {
					vex2_r_64.push(code);
				}
				if can_use_r(op_code) {
					if tested.r_bits != 3 {
						r_64.push(code);
					}
				} else {
					if (tested.r_bits & 1) == 0 {
						r_64.push(code);
					}
				}
				if is_vsib(op_code) {
					// The memory tests test vsib memory operands
				} else if can_use_x(op_code) {
					if tested.x_bits != 3 {
						x_64.push(code);
					}
				} else {
					if (tested.x_bits & 1) == 0 {
						x_64.push(code);
					}
				}
				match op_code.encoding() {
					EncodingKind::EVEX => {
						if is_vsib(op_code) {
							// The memory tests test vsib memory operands
						} else if can_use_v2(op_code) {
							if tested.v2_bits != 3 {
								v2_64.push(code);
							}
						} else {
							if (tested.v2_bits & 1) == 0 {
								v2_64.push(code);
							}
						}
					}
					EncodingKind::Legacy | EncodingKind::VEX | EncodingKind::XOP | EncodingKind::D3NOW => {}
				}
			}
			if op_code.can_use_xacquire_prefix() {
				if !tested.prefix_xacquire {
					get_vec(bitness, &mut pfx_xacquire_16, &mut pfx_xacquire_32, &mut pfx_xacquire_64).push(code);
				}
				if !tested.prefix_no_xacquire {
					get_vec(bitness, &mut pfx_no_xacquire_16, &mut pfx_no_xacquire_32, &mut pfx_no_xacquire_64).push(code);
				}
			}
			if op_code.can_use_xrelease_prefix() {
				if !tested.prefix_xrelease {
					get_vec(bitness, &mut pfx_xrelease_16, &mut pfx_xrelease_32, &mut pfx_xrelease_64).push(code);
				}
				if !tested.prefix_no_xrelease {
					get_vec(bitness, &mut pfx_no_xrelease_16, &mut pfx_no_xrelease_32, &mut pfx_no_xrelease_64).push(code);
				}
			}
			if op_code.can_use_lock_prefix() {
				if !tested.prefix_lock {
					get_vec(bitness, &mut pfx_lock_16, &mut pfx_lock_32, &mut pfx_lock_64).push(code);
				}
				if !tested.prefix_no_lock {
					get_vec(bitness, &mut pfx_no_lock_16, &mut pfx_no_lock_32, &mut pfx_no_lock_64).push(code);
				}
			}
			if op_code.can_use_hint_taken_prefix() {
				if !tested.prefix_hnt {
					get_vec(bitness, &mut pfx_hnt_16, &mut pfx_hnt_32, &mut pfx_hnt_64).push(code);
				}
				if !tested.prefix_no_hnt {
					get_vec(bitness, &mut pfx_no_hnt_16, &mut pfx_no_hnt_32, &mut pfx_no_hnt_64).push(code);
				}
			}
			if op_code.can_use_hint_taken_prefix() {
				if !tested.prefix_ht {
					get_vec(bitness, &mut pfx_ht_16, &mut pfx_ht_32, &mut pfx_ht_64).push(code);
				}
				if !tested.prefix_no_ht {
					get_vec(bitness, &mut pfx_no_ht_16, &mut pfx_no_ht_32, &mut pfx_no_ht_64).push(code);
				}
			}
			if op_code.can_use_rep_prefix() {
				if !tested.prefix_rep {
					get_vec(bitness, &mut pfx_rep_16, &mut pfx_rep_32, &mut pfx_rep_64).push(code);
				}
				if !tested.prefix_no_rep {
					get_vec(bitness, &mut pfx_no_rep_16, &mut pfx_no_rep_32, &mut pfx_no_rep_64).push(code);
				}
			}
			if op_code.can_use_repne_prefix() {
				if !tested.prefix_repne {
					get_vec(bitness, &mut pfx_repne_16, &mut pfx_repne_32, &mut pfx_repne_64).push(code);
				}
				if !tested.prefix_no_repne {
					get_vec(bitness, &mut pfx_no_repne_16, &mut pfx_no_repne_32, &mut pfx_no_repne_64).push(code);
				}
			}
			if op_code.can_use_notrack_prefix() {
				if !tested.prefix_notrack {
					get_vec(bitness, &mut pfx_notrack_16, &mut pfx_notrack_32, &mut pfx_notrack_64).push(code);
				}
				if !tested.prefix_no_notrack {
					get_vec(bitness, &mut pfx_no_notrack_16, &mut pfx_no_notrack_32, &mut pfx_no_notrack_64).push(code);
				}
			}
			if op_code.can_use_bnd_prefix() {
				if !tested.prefix_bnd {
					get_vec(bitness, &mut pfx_bnd_16, &mut pfx_bnd_32, &mut pfx_bnd_64).push(code);
				}
				if !tested.prefix_no_bnd {
					get_vec(bitness, &mut pfx_no_bnd_16, &mut pfx_no_bnd_32, &mut pfx_no_bnd_64).push(code);
				}
			}
		}
	}

	assert_eq!("wig32_16:", format!("wig32_16:{}", wig32_16.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("wig32_32:", format!("wig32_32:{}", wig32_32.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("wig_16:", format!("wig_16:{}", wig_16.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("wig_32:", format!("wig_32:{}", wig_32.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("wig_64:", format!("wig_64:{}", wig_64.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("w_64:", format!("w_64:{}", w_64.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("lig_16:", format!("lig_16:{}", lig_16.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("lig_32:", format!("lig_32:{}", lig_32.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("lig_64:", format!("lig_64:{}", lig_64.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("vex2_lig_16:", format!("vex2_lig_16:{}", vex2_lig_16.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("vex2_lig_32:", format!("vex2_lig_32:{}", vex2_lig_32.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("vex2_lig_64:", format!("vex2_lig_64:{}", vex2_lig_64.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("rr_16:", format!("rr_16:{}", rr_16.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("rr_32:", format!("rr_32:{}", rr_32.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("rr_64:", format!("rr_64:{}", rr_64.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("rm_16:", format!("rm_16:{}", rm_16.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("rm_32:", format!("rm_32:{}", rm_32.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("rm_64:", format!("rm_64:{}", rm_64.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("disp8_16:", format!("disp8_16:{}", disp8_16.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("disp8_32:", format!("disp8_32:{}", disp8_32.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("disp8_64:", format!("disp8_64:{}", disp8_64.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("vex2_16:", format!("vex2_16:{}", vex2_16.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("vex2_32:", format!("vex2_32:{}", vex2_32.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("vex2_64:", format!("vex2_64:{}", vex2_64.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("vex3_16:", format!("vex3_16:{}", vex3_16.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("vex3_32:", format!("vex3_32:{}", vex3_32.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("vex3_64:", format!("vex3_64:{}", vex3_64.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("opmask_16:", format!("opmask_16:{}", opmask_16.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("opmask_32:", format!("opmask_32:{}", opmask_32.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("opmask_64:", format!("opmask_64:{}", opmask_64.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("noopmask_16:", format!("noopmask_16:{}", noopmask_16.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("noopmask_32:", format!("noopmask_32:{}", noopmask_32.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("noopmask_64:", format!("noopmask_64:{}", noopmask_64.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("b_16:", format!("b_16:{}", b_16.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("b_32:", format!("b_32:{}", b_32.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("b_64:", format!("b_64:{}", b_64.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("r2_16:", format!("r2_16:{}", r2_16.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("r2_32:", format!("r2_32:{}", r2_32.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("r2_64:", format!("r2_64:{}", r2_64.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("r_64:", format!("r_64:{}", r_64.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("vex2_r_64:", format!("vex2_r_64:{}", vex2_r_64.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("x_64:", format!("x_64:{}", x_64.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("v2_64:", format!("v2_64:{}", v2_64.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!(
		"pfx_xacquire_16:",
		format!("pfx_xacquire_16:{}", pfx_xacquire_16.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(","))
	);
	assert_eq!(
		"pfx_xacquire_32:",
		format!("pfx_xacquire_32:{}", pfx_xacquire_32.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(","))
	);
	assert_eq!(
		"pfx_xacquire_64:",
		format!("pfx_xacquire_64:{}", pfx_xacquire_64.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(","))
	);
	assert_eq!(
		"pfx_xrelease_16:",
		format!("pfx_xrelease_16:{}", pfx_xrelease_16.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(","))
	);
	assert_eq!(
		"pfx_xrelease_32:",
		format!("pfx_xrelease_32:{}", pfx_xrelease_32.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(","))
	);
	assert_eq!(
		"pfx_xrelease_64:",
		format!("pfx_xrelease_64:{}", pfx_xrelease_64.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(","))
	);
	assert_eq!("pfx_lock_16:", format!("pfx_lock_16:{}", pfx_lock_16.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("pfx_lock_32:", format!("pfx_lock_32:{}", pfx_lock_32.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("pfx_lock_64:", format!("pfx_lock_64:{}", pfx_lock_64.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("pfx_hnt_16:", format!("pfx_hnt_16:{}", pfx_hnt_16.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("pfx_hnt_32:", format!("pfx_hnt_32:{}", pfx_hnt_32.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("pfx_hnt_64:", format!("pfx_hnt_64:{}", pfx_hnt_64.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("pfx_ht_16:", format!("pfx_ht_16:{}", pfx_ht_16.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("pfx_ht_32:", format!("pfx_ht_32:{}", pfx_ht_32.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("pfx_ht_64:", format!("pfx_ht_64:{}", pfx_ht_64.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("pfx_rep_16:", format!("pfx_rep_16:{}", pfx_rep_16.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("pfx_rep_32:", format!("pfx_rep_32:{}", pfx_rep_32.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("pfx_rep_64:", format!("pfx_rep_64:{}", pfx_rep_64.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("pfx_repne_16:", format!("pfx_repne_16:{}", pfx_repne_16.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("pfx_repne_32:", format!("pfx_repne_32:{}", pfx_repne_32.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("pfx_repne_64:", format!("pfx_repne_64:{}", pfx_repne_64.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!(
		"pfx_notrack_16:",
		format!("pfx_notrack_16:{}", pfx_notrack_16.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(","))
	);
	assert_eq!(
		"pfx_notrack_32:",
		format!("pfx_notrack_32:{}", pfx_notrack_32.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(","))
	);
	assert_eq!(
		"pfx_notrack_64:",
		format!("pfx_notrack_64:{}", pfx_notrack_64.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(","))
	);
	assert_eq!("pfx_bnd_16:", format!("pfx_bnd_16:{}", pfx_bnd_16.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("pfx_bnd_32:", format!("pfx_bnd_32:{}", pfx_bnd_32.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("pfx_bnd_64:", format!("pfx_bnd_64:{}", pfx_bnd_64.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!(
		"pfx_no_xacquire_16:",
		format!("pfx_no_xacquire_16:{}", pfx_no_xacquire_16.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(","))
	);
	assert_eq!(
		"pfx_no_xacquire_32:",
		format!("pfx_no_xacquire_32:{}", pfx_no_xacquire_32.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(","))
	);
	assert_eq!(
		"pfx_no_xacquire_64:",
		format!("pfx_no_xacquire_64:{}", pfx_no_xacquire_64.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(","))
	);
	assert_eq!(
		"pfx_no_xrelease_16:",
		format!("pfx_no_xrelease_16:{}", pfx_no_xrelease_16.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(","))
	);
	assert_eq!(
		"pfx_no_xrelease_32:",
		format!("pfx_no_xrelease_32:{}", pfx_no_xrelease_32.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(","))
	);
	assert_eq!(
		"pfx_no_xrelease_64:",
		format!("pfx_no_xrelease_64:{}", pfx_no_xrelease_64.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(","))
	);
	assert_eq!(
		"pfx_no_lock_16:",
		format!("pfx_no_lock_16:{}", pfx_no_lock_16.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(","))
	);
	assert_eq!(
		"pfx_no_lock_32:",
		format!("pfx_no_lock_32:{}", pfx_no_lock_32.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(","))
	);
	assert_eq!(
		"pfx_no_lock_64:",
		format!("pfx_no_lock_64:{}", pfx_no_lock_64.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(","))
	);
	assert_eq!("pfx_no_hnt_16:", format!("pfx_no_hnt_16:{}", pfx_no_hnt_16.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("pfx_no_hnt_32:", format!("pfx_no_hnt_32:{}", pfx_no_hnt_32.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("pfx_no_hnt_64:", format!("pfx_no_hnt_64:{}", pfx_no_hnt_64.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("pfx_no_ht_16:", format!("pfx_no_ht_16:{}", pfx_no_ht_16.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("pfx_no_ht_32:", format!("pfx_no_ht_32:{}", pfx_no_ht_32.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("pfx_no_ht_64:", format!("pfx_no_ht_64:{}", pfx_no_ht_64.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("pfx_no_rep_16:", format!("pfx_no_rep_16:{}", pfx_no_rep_16.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("pfx_no_rep_32:", format!("pfx_no_rep_32:{}", pfx_no_rep_32.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("pfx_no_rep_64:", format!("pfx_no_rep_64:{}", pfx_no_rep_64.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!(
		"pfx_no_repne_16:",
		format!("pfx_no_repne_16:{}", pfx_no_repne_16.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(","))
	);
	assert_eq!(
		"pfx_no_repne_32:",
		format!("pfx_no_repne_32:{}", pfx_no_repne_32.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(","))
	);
	assert_eq!(
		"pfx_no_repne_64:",
		format!("pfx_no_repne_64:{}", pfx_no_repne_64.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(","))
	);
	assert_eq!(
		"pfx_no_notrack_16:",
		format!("pfx_no_notrack_16:{}", pfx_no_notrack_16.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(","))
	);
	assert_eq!(
		"pfx_no_notrack_32:",
		format!("pfx_no_notrack_32:{}", pfx_no_notrack_32.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(","))
	);
	assert_eq!(
		"pfx_no_notrack_64:",
		format!("pfx_no_notrack_64:{}", pfx_no_notrack_64.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(","))
	);
	assert_eq!("pfx_no_bnd_16:", format!("pfx_no_bnd_16:{}", pfx_no_bnd_16.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("pfx_no_bnd_32:", format!("pfx_no_bnd_32:{}", pfx_no_bnd_32.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));
	assert_eq!("pfx_no_bnd_64:", format!("pfx_no_bnd_64:{}", pfx_no_bnd_64.iter().map(|&a| format!("{:?}", a)).collect::<Vec<String>>().join(",")));

	fn can_use_modrm_rm_reg(op_code: &OpCodeInfo) -> bool {
		for i in 0..op_code.op_count() {
			match op_code.op_kind(i) {
				OpCodeOperandKind::r8_or_mem
				| OpCodeOperandKind::r16_or_mem
				| OpCodeOperandKind::r32_or_mem
				| OpCodeOperandKind::r32_or_mem_mpx
				| OpCodeOperandKind::r64_or_mem
				| OpCodeOperandKind::r64_or_mem_mpx
				| OpCodeOperandKind::mm_or_mem
				| OpCodeOperandKind::xmm_or_mem
				| OpCodeOperandKind::ymm_or_mem
				| OpCodeOperandKind::zmm_or_mem
				| OpCodeOperandKind::bnd_or_mem_mpx
				| OpCodeOperandKind::k_or_mem
				| OpCodeOperandKind::r16_rm
				| OpCodeOperandKind::r32_rm
				| OpCodeOperandKind::r64_rm
				| OpCodeOperandKind::k_rm
				| OpCodeOperandKind::mm_rm
				| OpCodeOperandKind::xmm_rm
				| OpCodeOperandKind::ymm_rm
				| OpCodeOperandKind::zmm_rm => return true,
				_ => {}
			}
		}
		false
	}

	fn can_use_modrm_rm_mem(op_code: &OpCodeInfo) -> bool {
		for i in 0..op_code.op_count() {
			match op_code.op_kind(i) {
				OpCodeOperandKind::mem
				| OpCodeOperandKind::mem_mpx
				| OpCodeOperandKind::mem_mib
				| OpCodeOperandKind::mem_vsib32x
				| OpCodeOperandKind::mem_vsib64x
				| OpCodeOperandKind::mem_vsib32y
				| OpCodeOperandKind::mem_vsib64y
				| OpCodeOperandKind::mem_vsib32z
				| OpCodeOperandKind::mem_vsib64z
				| OpCodeOperandKind::r8_or_mem
				| OpCodeOperandKind::r16_or_mem
				| OpCodeOperandKind::r32_or_mem
				| OpCodeOperandKind::r32_or_mem_mpx
				| OpCodeOperandKind::r64_or_mem
				| OpCodeOperandKind::r64_or_mem_mpx
				| OpCodeOperandKind::mm_or_mem
				| OpCodeOperandKind::xmm_or_mem
				| OpCodeOperandKind::ymm_or_mem
				| OpCodeOperandKind::zmm_or_mem
				| OpCodeOperandKind::bnd_or_mem_mpx
				| OpCodeOperandKind::k_or_mem => return true,
				_ => {}
			}
		}
		false
	}

	fn can_use_vex2(op_code: &OpCodeInfo) -> bool {
		op_code.table() == OpCodeTableKind::T0F && op_code.w() == 0
	}

	fn can_use_b(bitness: u32, op_code: &OpCodeInfo) -> bool {
		match op_code.code() {
			Code::Nopw | Code::Nopd | Code::Nopq | Code::Bndmov_bnd_bndm128 | Code::Bndmov_bndm128_bnd => return false,
			_ => {}
		}

		for i in 0..op_code.op_count() {
			match op_code.op_kind(i) {
				OpCodeOperandKind::k_rm
				| OpCodeOperandKind::mm_rm
				| OpCodeOperandKind::r16_rm
				| OpCodeOperandKind::r32_rm
				| OpCodeOperandKind::r64_rm
				| OpCodeOperandKind::xmm_rm
				| OpCodeOperandKind::ymm_rm
				| OpCodeOperandKind::zmm_rm
				| OpCodeOperandKind::bnd_or_mem_mpx
				| OpCodeOperandKind::k_or_mem
				| OpCodeOperandKind::mm_or_mem
				| OpCodeOperandKind::r16_or_mem
				| OpCodeOperandKind::r32_or_mem
				| OpCodeOperandKind::r32_or_mem_mpx
				| OpCodeOperandKind::r64_or_mem
				| OpCodeOperandKind::r64_or_mem_mpx
				| OpCodeOperandKind::r8_or_mem
				| OpCodeOperandKind::xmm_or_mem
				| OpCodeOperandKind::ymm_or_mem
				| OpCodeOperandKind::zmm_or_mem => {
					if op_code.encoding() == EncodingKind::Legacy || op_code.encoding() == EncodingKind::D3NOW {
						return bitness == 64;
					}
					return true;
				}

				OpCodeOperandKind::mem
				| OpCodeOperandKind::mem_mpx
				| OpCodeOperandKind::mem_mib
				| OpCodeOperandKind::mem_vsib32x
				| OpCodeOperandKind::mem_vsib32y
				| OpCodeOperandKind::mem_vsib32z
				| OpCodeOperandKind::mem_vsib64x
				| OpCodeOperandKind::mem_vsib64y
				| OpCodeOperandKind::mem_vsib64z => {
					// The memory test tests all combinations
					return false;
				}

				_ => {}
			}
		}
		if op_code.encoding() == EncodingKind::Legacy || op_code.encoding() == EncodingKind::D3NOW {
			bitness == 64
		} else {
			true
		}
	}

	fn can_use_x(op_code: &OpCodeInfo) -> bool {
		for i in 0..op_code.op_count() {
			match op_code.op_kind(i) {
				OpCodeOperandKind::k_rm
				| OpCodeOperandKind::mm_rm
				| OpCodeOperandKind::r16_rm
				| OpCodeOperandKind::r32_rm
				| OpCodeOperandKind::r64_rm
				| OpCodeOperandKind::xmm_rm
				| OpCodeOperandKind::ymm_rm
				| OpCodeOperandKind::zmm_rm
				| OpCodeOperandKind::bnd_or_mem_mpx
				| OpCodeOperandKind::k_or_mem
				| OpCodeOperandKind::mm_or_mem
				| OpCodeOperandKind::r16_or_mem
				| OpCodeOperandKind::r32_or_mem
				| OpCodeOperandKind::r32_or_mem_mpx
				| OpCodeOperandKind::r64_or_mem
				| OpCodeOperandKind::r64_or_mem_mpx
				| OpCodeOperandKind::r8_or_mem
				| OpCodeOperandKind::xmm_or_mem
				| OpCodeOperandKind::ymm_or_mem
				| OpCodeOperandKind::zmm_or_mem => return true,
				OpCodeOperandKind::mem
				| OpCodeOperandKind::mem_mpx
				| OpCodeOperandKind::mem_mib
				| OpCodeOperandKind::mem_vsib32x
				| OpCodeOperandKind::mem_vsib32y
				| OpCodeOperandKind::mem_vsib32z
				| OpCodeOperandKind::mem_vsib64x
				| OpCodeOperandKind::mem_vsib64y
				| OpCodeOperandKind::mem_vsib64z => {
					// The memory test tests all combinations
					return false;
				}
				_ => {}
			}
		}
		true
	}

	fn can_use_r(op_code: &OpCodeInfo) -> bool {
		for i in 0..op_code.op_count() {
			match op_code.op_kind(i) {
				OpCodeOperandKind::k_reg | OpCodeOperandKind::kp1_reg | OpCodeOperandKind::tr_reg | OpCodeOperandKind::bnd_reg => return false,
				OpCodeOperandKind::cr_reg
				| OpCodeOperandKind::dr_reg
				| OpCodeOperandKind::mm_reg
				| OpCodeOperandKind::r16_reg
				| OpCodeOperandKind::r32_reg
				| OpCodeOperandKind::r64_reg
				| OpCodeOperandKind::r8_reg
				| OpCodeOperandKind::seg_reg
				| OpCodeOperandKind::xmm_reg
				| OpCodeOperandKind::ymm_reg
				| OpCodeOperandKind::zmm_reg => return true,
				_ => {}
			}
		}
		true
	}

	fn can_use_r2(op_code: &OpCodeInfo) -> bool {
		for i in 0..op_code.op_count() {
			match op_code.op_kind(i) {
				OpCodeOperandKind::k_reg
				| OpCodeOperandKind::kp1_reg
				| OpCodeOperandKind::tr_reg
				| OpCodeOperandKind::bnd_reg
				| OpCodeOperandKind::cr_reg
				| OpCodeOperandKind::dr_reg
				| OpCodeOperandKind::mm_reg
				| OpCodeOperandKind::r16_reg
				| OpCodeOperandKind::r32_reg
				| OpCodeOperandKind::r64_reg
				| OpCodeOperandKind::r8_reg
				| OpCodeOperandKind::seg_reg => return false,
				OpCodeOperandKind::xmm_reg | OpCodeOperandKind::ymm_reg | OpCodeOperandKind::zmm_reg => return true,
				_ => {}
			}
		}
		true
	}

	fn can_use_v2(op_code: &OpCodeInfo) -> bool {
		for i in 0..op_code.op_count() {
			match op_code.op_kind(i) {
				OpCodeOperandKind::k_vvvv | OpCodeOperandKind::r32_vvvv | OpCodeOperandKind::r64_vvvv => return false,
				OpCodeOperandKind::xmm_vvvv
				| OpCodeOperandKind::xmmp3_vvvv
				| OpCodeOperandKind::ymm_vvvv
				| OpCodeOperandKind::zmm_vvvv
				| OpCodeOperandKind::zmmp3_vvvv => return true,
				OpCodeOperandKind::mem_vsib32x
				| OpCodeOperandKind::mem_vsib32y
				| OpCodeOperandKind::mem_vsib32z
				| OpCodeOperandKind::mem_vsib64x
				| OpCodeOperandKind::mem_vsib64y
				| OpCodeOperandKind::mem_vsib64z => {
					// The memory test tests all combinations
					return false;
				}
				_ => {}
			}
		}
		false
	}

	fn has_modrm(op_code: &OpCodeInfo) -> bool {
		for i in 0..op_code.op_count() {
			match op_code.op_kind(i) {
				OpCodeOperandKind::mem
				| OpCodeOperandKind::mem_mpx
				| OpCodeOperandKind::mem_mib
				| OpCodeOperandKind::mem_vsib32x
				| OpCodeOperandKind::mem_vsib64x
				| OpCodeOperandKind::mem_vsib32y
				| OpCodeOperandKind::mem_vsib64y
				| OpCodeOperandKind::mem_vsib32z
				| OpCodeOperandKind::mem_vsib64z
				| OpCodeOperandKind::r8_or_mem
				| OpCodeOperandKind::r16_or_mem
				| OpCodeOperandKind::r32_or_mem
				| OpCodeOperandKind::r32_or_mem_mpx
				| OpCodeOperandKind::r64_or_mem
				| OpCodeOperandKind::r64_or_mem_mpx
				| OpCodeOperandKind::mm_or_mem
				| OpCodeOperandKind::xmm_or_mem
				| OpCodeOperandKind::ymm_or_mem
				| OpCodeOperandKind::zmm_or_mem
				| OpCodeOperandKind::bnd_or_mem_mpx
				| OpCodeOperandKind::k_or_mem
				| OpCodeOperandKind::r8_reg
				| OpCodeOperandKind::r16_reg
				| OpCodeOperandKind::r16_rm
				| OpCodeOperandKind::r32_reg
				| OpCodeOperandKind::r32_rm
				| OpCodeOperandKind::r64_reg
				| OpCodeOperandKind::r64_rm
				| OpCodeOperandKind::seg_reg
				| OpCodeOperandKind::k_reg
				| OpCodeOperandKind::kp1_reg
				| OpCodeOperandKind::k_rm
				| OpCodeOperandKind::mm_reg
				| OpCodeOperandKind::mm_rm
				| OpCodeOperandKind::xmm_reg
				| OpCodeOperandKind::xmm_rm
				| OpCodeOperandKind::ymm_reg
				| OpCodeOperandKind::ymm_rm
				| OpCodeOperandKind::zmm_reg
				| OpCodeOperandKind::zmm_rm
				| OpCodeOperandKind::cr_reg
				| OpCodeOperandKind::dr_reg
				| OpCodeOperandKind::tr_reg
				| OpCodeOperandKind::bnd_reg => return true,
				_ => {}
			}
		}
		false
	}

	fn get_vec2<'a>(bitness: u32, l16: &'a mut Vec<Code>, l32: &'a mut Vec<Code>) -> &'a mut Vec<Code> {
		match bitness {
			16 => l16,
			32 => l32,
			_ => panic!(),
		}
	}

	fn get_vec<'a>(bitness: u32, l16: &'a mut Vec<Code>, l32: &'a mut Vec<Code>, l64: &'a mut Vec<Code>) -> &'a mut Vec<Code> {
		match bitness {
			16 => l16,
			32 => l32,
			64 => l64,
			_ => panic!(),
		}
	}
}

#[test]
fn test_invalid_zero_opmask_reg() {
	for info in decoder_tests(false, false) {
		if (info.decoder_options() & DecoderOptions::NO_INVALID_CHECK) != 0 {
			continue;
		}
		let op_code = info.code().op_code();
		if !op_code.require_non_zero_op_mask_register() {
			continue;
		}

		let mut bytes = to_vec_u8(info.hex_bytes()).unwrap();
		let mut orig_instr;
		{
			let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
			orig_instr = decoder.decode();
			assert_eq!(info.code(), orig_instr.code());
		}

		let evex_index = get_evex_index(&bytes);
		bytes[evex_index + 3] &= 0xF8;
		{
			let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
			let instruction = decoder.decode();
			assert_eq!(Code::INVALID, instruction.code());
			assert!(!decoder.invalid_no_more_bytes());
		}
		{
			let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options() | DecoderOptions::NO_INVALID_CHECK);
			let instruction = decoder.decode();
			assert_eq!(info.code(), instruction.code());
			assert_eq!(Register::None, instruction.op_mask());
			orig_instr.set_op_mask(Register::None);
			assert!(orig_instr.eq_all_bits(&instruction));
		}
	}
}

#[test]
#[cfg_attr(feature = "cargo-clippy", allow(clippy::needless_range_loop))]
fn verify_cpu_mode() {
	let hash1632: HashSet<Code> = code32_only().iter().chain(not_decoded32_only().iter()).cloned().collect();
	let hash64: HashSet<Code> = code64_only().iter().chain(not_decoded64_only().iter()).cloned().collect();
	let code_names = code_names();
	for i in 0..IcedConstants::NUMBER_OF_CODE_VALUES {
		if is_ignored_code(code_names[i]) {
			continue;
		}
		let code: Code = unsafe { mem::transmute(i as u16) };
		let op_code = code.op_code();
		if hash1632.contains(&code) {
			assert!(op_code.mode16());
			assert!(op_code.mode32());
			assert!(!op_code.mode64());
		} else if hash64.contains(&code) {
			assert!(!op_code.mode16());
			assert!(!op_code.mode32());
			assert!(op_code.mode64());
		} else {
			assert!(op_code.mode16());
			assert!(op_code.mode32());
			assert!(op_code.mode64());
		}
	}
}

#[test]
fn verify_can_only_decode_in_correct_mode() {
	let extra_bytes: String = iter::repeat('0').take((IcedConstants::MAX_INSTRUCTION_LENGTH - 1) * 2).collect();
	for info in decoder_tests(false, false) {
		let op_code = info.code().op_code();
		let new_hex_bytes = format!("{}{}", info.hex_bytes(), extra_bytes);
		if !op_code.mode16() {
			let bytes = to_vec_u8(&new_hex_bytes).unwrap();
			let mut decoder = Decoder::new(16, &bytes, info.decoder_options());
			let instruction = decoder.decode();
			assert_ne!(info.code(), instruction.code());
		}
		if !op_code.mode32() {
			let bytes = to_vec_u8(&new_hex_bytes).unwrap();
			let mut decoder = Decoder::new(32, &bytes, info.decoder_options());
			let instruction = decoder.decode();
			assert_ne!(info.code(), instruction.code());
		}
		if !op_code.mode64() {
			let bytes = to_vec_u8(&new_hex_bytes).unwrap();
			let mut decoder = Decoder::new(64, &bytes, info.decoder_options());
			let instruction = decoder.decode();
			assert_ne!(info.code(), instruction.code());
		}
	}
}

#[test]
fn verify_invalid_table_encoding() {
	for info in decoder_tests(false, false) {
		let op_code = info.code().op_code();
		if op_code.encoding() == EncodingKind::EVEX {
			let mut hex_bytes = to_vec_u8(info.hex_bytes()).unwrap();
			let evex_index = get_evex_index(&hex_bytes);
			hex_bytes[evex_index + 1] &= 0xFC;
			{
				let mut decoder = Decoder::new(info.bitness(), &hex_bytes, info.decoder_options());
				let instruction = decoder.decode();
				assert_eq!(Code::INVALID, instruction.code());
				assert!(!decoder.invalid_no_more_bytes());
			}
			{
				let mut decoder = Decoder::new(info.bitness(), &hex_bytes, info.decoder_options() ^ DecoderOptions::NO_INVALID_CHECK);
				let instruction = decoder.decode();
				assert_eq!(Code::INVALID, instruction.code());
				assert!(!decoder.invalid_no_more_bytes());
			}
		} else if op_code.encoding() == EncodingKind::VEX {
			let mut hex_bytes = to_vec_u8(info.hex_bytes()).unwrap();
			let vex_index = get_vex_xop_index(&hex_bytes);
			if hex_bytes[vex_index] == 0xC5 {
				continue;
			}
			for i in 0..32 {
				match i {
					1 | 2 | 3 => continue,
					_ => {}
				}
				hex_bytes[vex_index + 1] = (hex_bytes[vex_index + 1] & 0xE0) | i;
				{
					let mut decoder = Decoder::new(info.bitness(), &hex_bytes, info.decoder_options());
					let instruction = decoder.decode();
					assert_eq!(Code::INVALID, instruction.code());
					assert!(!decoder.invalid_no_more_bytes());
				}
				{
					let mut decoder = Decoder::new(info.bitness(), &hex_bytes, info.decoder_options() ^ DecoderOptions::NO_INVALID_CHECK);
					let instruction = decoder.decode();
					assert_eq!(Code::INVALID, instruction.code());
					assert!(!decoder.invalid_no_more_bytes());
				}
			}
		} else if op_code.encoding() == EncodingKind::XOP {
			let mut hex_bytes = to_vec_u8(info.hex_bytes()).unwrap();
			let vex_index = get_vex_xop_index(&hex_bytes);
			for i in 0..32 {
				match i {
					8 | 9 | 0xA => continue,
					_ => {}
				}
				hex_bytes[vex_index + 1] = (hex_bytes[vex_index + 1] & 0xE0) | i;
				{
					let mut decoder = Decoder::new(info.bitness(), &hex_bytes, info.decoder_options());
					let instruction = decoder.decode();
					if i < 8 {
						assert_ne!(info.code(), instruction.code());
					} else {
						assert_eq!(Code::INVALID, instruction.code());
						assert!(!decoder.invalid_no_more_bytes());
					}
				}
				{
					let mut decoder = Decoder::new(info.bitness(), &hex_bytes, info.decoder_options() ^ DecoderOptions::NO_INVALID_CHECK);
					let instruction = decoder.decode();
					if i < 8 {
						assert_ne!(info.code(), instruction.code());
					} else {
						assert_eq!(Code::INVALID, instruction.code());
						assert!(!decoder.invalid_no_more_bytes());
					}
				}
			}
		} else if op_code.encoding() == EncodingKind::Legacy || op_code.encoding() == EncodingKind::D3NOW {
		} else {
			panic!();
		}
	}
}

#[test]
fn verify_invalid_pp_field() {
	for info in decoder_tests(false, false) {
		let op_code = info.code().op_code();
		if op_code.encoding() == EncodingKind::EVEX {
			let mut hex_bytes = to_vec_u8(info.hex_bytes()).unwrap();
			let evex_index = get_evex_index(&hex_bytes);
			let b = hex_bytes[evex_index + 2];
			for i in 1..4 {
				hex_bytes[evex_index + 2] = b ^ i;
				{
					let mut decoder = Decoder::new(info.bitness(), &hex_bytes, info.decoder_options());
					let instruction = decoder.decode();
					assert_ne!(info.code(), instruction.code());
				}
				{
					let mut decoder = Decoder::new(info.bitness(), &hex_bytes, info.decoder_options() ^ DecoderOptions::NO_INVALID_CHECK);
					let instruction = decoder.decode();
					assert_ne!(info.code(), instruction.code());
				}
			}
		} else if op_code.encoding() == EncodingKind::VEX || op_code.encoding() == EncodingKind::XOP {
			let mut hex_bytes = to_vec_u8(info.hex_bytes()).unwrap();
			let vex_index = get_vex_xop_index(&hex_bytes);
			let pp_index = if hex_bytes[vex_index] == 0xC5 { vex_index + 1 } else { vex_index + 2 };
			let b = hex_bytes[pp_index];
			for i in 1..4 {
				hex_bytes[pp_index] = b ^ i;
				{
					let mut decoder = Decoder::new(info.bitness(), &hex_bytes, info.decoder_options());
					let instruction = decoder.decode();
					assert_ne!(info.code(), instruction.code());
				}
				{
					let mut decoder = Decoder::new(info.bitness(), &hex_bytes, info.decoder_options() ^ DecoderOptions::NO_INVALID_CHECK);
					let instruction = decoder.decode();
					assert_ne!(info.code(), instruction.code());
				}
			}
		} else if op_code.encoding() == EncodingKind::Legacy || op_code.encoding() == EncodingKind::D3NOW {
		} else {
			panic!();
		}
	}
}

#[test]
fn verify_regonly_or_regmemonly_mod_bits() {
	let extra_bytes: String = iter::repeat('0').take((IcedConstants::MAX_INSTRUCTION_LENGTH - 1) * 2).collect();
	for info in decoder_tests(false, false) {
		let op_code = info.code().op_code();
		if !is_reg_only_or_reg_mem_only_mod_rm(op_code) {
			continue;
		}
		// There are a few instructions that ignore the mod bits...
		match info.code() {
			Code::Mov_r32_cr
			| Code::Mov_r64_cr
			| Code::Mov_r32_dr
			| Code::Mov_r64_dr
			| Code::Mov_cr_r32
			| Code::Mov_cr_r64
			| Code::Mov_dr_r32
			| Code::Mov_dr_r64
			| Code::Mov_r32_tr
			| Code::Mov_tr_r32 => continue,
			_ => {}
		}

		let mut bytes = to_vec_u8(&format!("{}{}", info.hex_bytes(), extra_bytes)).unwrap();
		let m_index = if op_code.encoding() == EncodingKind::EVEX {
			get_evex_index(&bytes) + 5
		} else if op_code.encoding() == EncodingKind::VEX || op_code.encoding() == EncodingKind::XOP {
			let vex_index = get_vex_xop_index(&bytes);
			if bytes[vex_index] == 0xC5 {
				vex_index + 3
			} else {
				vex_index + 4
			}
		} else if op_code.encoding() == EncodingKind::Legacy || op_code.encoding() == EncodingKind::D3NOW {
			let (mut m_index, _) = skip_prefixes(&bytes, info.bitness());
			match op_code.table() {
				OpCodeTableKind::Normal => {}
				OpCodeTableKind::T0F => {
					if bytes[m_index] != 0x0F {
						panic!();
					}
					m_index += 1;
				}
				OpCodeTableKind::T0F38 => {
					if bytes[m_index] != 0x0F {
						panic!();
					}
					m_index += 1;
					if bytes[m_index] != 0x38 {
						panic!();
					}
					m_index += 1;
				}
				OpCodeTableKind::T0F3A => {
					if bytes[m_index] != 0x0F {
						panic!();
					}
					m_index += 1;
					if bytes[m_index] != 0x3A {
						panic!();
					}
					m_index += 1;
				}
				_ => panic!(),
			}
			m_index + 1
		} else {
			panic!();
		};

		if bytes[m_index] >= 0xC0 {
			bytes[m_index] &= 0x3F;
		} else {
			bytes[m_index] |= 0xC0;
		}
		{
			let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
			let instruction = decoder.decode();
			assert_ne!(info.code(), instruction.code());
		}
		{
			let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options() ^ DecoderOptions::NO_INVALID_CHECK);
			let instruction = decoder.decode();
			assert_ne!(info.code(), instruction.code());
		}
	}

	fn is_reg_only_or_reg_mem_only_mod_rm(op_code: &OpCodeInfo) -> bool {
		for i in 0..op_code.op_count() {
			match op_code.op_kind(i) {
				OpCodeOperandKind::mem
				| OpCodeOperandKind::mem_mpx
				| OpCodeOperandKind::mem_mib
				| OpCodeOperandKind::mem_vsib32x
				| OpCodeOperandKind::mem_vsib64x
				| OpCodeOperandKind::mem_vsib32y
				| OpCodeOperandKind::mem_vsib64y
				| OpCodeOperandKind::mem_vsib32z
				| OpCodeOperandKind::mem_vsib64z
				| OpCodeOperandKind::r16_rm
				| OpCodeOperandKind::r32_rm
				| OpCodeOperandKind::r64_rm
				| OpCodeOperandKind::k_rm
				| OpCodeOperandKind::mm_rm
				| OpCodeOperandKind::xmm_rm
				| OpCodeOperandKind::ymm_rm
				| OpCodeOperandKind::zmm_rm => return true,
				_ => {}
			}
		}
		false
	}
}

#[test]
fn disable_decoder_option_disables_instruction() {
	let extra_bytes: String = iter::repeat('0').take((IcedConstants::MAX_INSTRUCTION_LENGTH - 1) * 2).collect();
	for info in decoder_tests(false, false) {
		if info.decoder_options() == DecoderOptions::NONE {
			continue;
		}
		const NO_OPTIONS: u32 = DecoderOptions::NO_INVALID_CHECK
			| DecoderOptions::NO_PAUSE
			| DecoderOptions::NO_WBNOINVD
			| DecoderOptions::NO_LOCK_MOV_CR0
			| DecoderOptions::NO_MPFX_0FBC
			| DecoderOptions::NO_MPFX_0FBD
			| DecoderOptions::NO_LAHF_SAHF_64;
		if (info.decoder_options() & NO_OPTIONS) != 0 {
			continue;
		}
		if !is_power_of_two(info.decoder_options()) {
			continue;
		}

		// Some 'normal' instructions are tested with some decoder option enabled (eg. retnq + amd)
		// but 'amd' option is a nop when decoding retnq. If it's one of those instructions, just
		// ignore it (continue). We specifically check for these ignored instructions instead of
		// continuing by default (in '_' case) because if a new instruction uses an existing
		// flag (eg. AMD), we need to test it. This test will panic if that ever happens so the code
		// will get updated (a new case is added to the 'break' code path).
		match info.decoder_options() {
			DecoderOptions::FORCE_RESERVED_NOP => continue,

			DecoderOptions::AMD => match info.code() {
				Code::Jecxz_rel8_16
				| Code::Jrcxz_rel8_16
				| Code::Loop_rel8_16_ECX
				| Code::Loop_rel8_16_RCX
				| Code::Loope_rel8_16_ECX
				| Code::Loope_rel8_16_RCX
				| Code::Loopne_rel8_16_ECX
				| Code::Loopne_rel8_16_RCX
				| Code::Ud0
				| Code::Call_rel16
				| Code::Call_rm16
				| Code::Ja_rel16
				| Code::Ja_rel8_16
				| Code::Jae_rel16
				| Code::Jae_rel8_16
				| Code::Jb_rel16
				| Code::Jb_rel8_16
				| Code::Jbe_rel16
				| Code::Jbe_rel8_16
				| Code::Je_rel16
				| Code::Je_rel8_16
				| Code::Jg_rel16
				| Code::Jg_rel8_16
				| Code::Jge_rel16
				| Code::Jge_rel8_16
				| Code::Jl_rel16
				| Code::Jl_rel8_16
				| Code::Jle_rel16
				| Code::Jle_rel8_16
				| Code::Jmp_rel16
				| Code::Jmp_rel8_16
				| Code::Jmp_rm16
				| Code::Jne_rel16
				| Code::Jne_rel8_16
				| Code::Jno_rel16
				| Code::Jno_rel8_16
				| Code::Jnp_rel16
				| Code::Jnp_rel8_16
				| Code::Jns_rel16
				| Code::Jns_rel8_16
				| Code::Jo_rel16
				| Code::Jo_rel8_16
				| Code::Jp_rel16
				| Code::Jp_rel8_16
				| Code::Js_rel16
				| Code::Js_rel8_16
				| Code::Retnw
				| Code::Retnw_imm16
				| Code::Lss_r32_m1632
				| Code::Lfs_r32_m1632
				| Code::Lgs_r32_m1632
				| Code::Call_m1632
				| Code::Jmp_m1632 => break,
				Code::Call_rel32_64
				| Code::Call_rm64
				| Code::Ja_rel32_64
				| Code::Jae_rel32_64
				| Code::Jb_rel32_64
				| Code::Jbe_rel32_64
				| Code::Je_rel32_64
				| Code::Jecxz_rel8_64
				| Code::Jg_rel32_64
				| Code::Jge_rel32_64
				| Code::Jl_rel32_64
				| Code::Jle_rel32_64
				| Code::Jmp_rel32_64
				| Code::Jmp_rel8_64
				| Code::Jmp_rm64
				| Code::Jne_rel32_64
				| Code::Jno_rel32_64
				| Code::Jnp_rel32_64
				| Code::Jns_rel32_64
				| Code::Jo_rel32_64
				| Code::Jp_rel32_64
				| Code::Jrcxz_rel8_64
				| Code::Js_rel32_64
				| Code::Loop_rel8_64_ECX
				| Code::Loop_rel8_64_RCX
				| Code::Loope_rel8_64_ECX
				| Code::Loope_rel8_64_RCX
				| Code::Loopne_rel8_64_ECX
				| Code::Loopne_rel8_64_RCX
				| Code::Retnq
				| Code::Retnq_imm16 => continue,
				_ => unreachable!("Update this code: `=> continue` or `=> {}`"),
			},

			DecoderOptions::CL1INVMB => match info.code() {
				Code::Cl1invmb => {}
				_ => unreachable!("Update this code: `=> continue` or `=> {}`"),
			},

			DecoderOptions::CMPXCHG486A => match info.code() {
				Code::Cmpxchg486_rm16_r16 | Code::Cmpxchg486_rm32_r32 | Code::Cmpxchg486_rm8_r8 => {}
				Code::Montmul_32 | Code::Montmul_64 | Code::Xsha1_32 | Code::Xsha1_64 | Code::Xsha256_32 | Code::Xsha256_64 => continue,
				_ => unreachable!("Update this code: `=> continue` or `=> {}`"),
			},

			DecoderOptions::JMPE => match info.code() {
				Code::Jmpe_disp16 | Code::Jmpe_disp32 | Code::Jmpe_rm16 | Code::Jmpe_rm32 => {}
				Code::Popcnt_r16_rm16 | Code::Popcnt_r32_rm32 | Code::Popcnt_r64_rm64 => continue,
				_ => unreachable!("Update this code: `=> continue` or `=> {}`"),
			},

			DecoderOptions::LOADALL286 => match info.code() {
				Code::Loadall286 | Code::Loadallreset286 => {}
				Code::Syscall => continue,
				_ => unreachable!("Update this code: `=> continue` or `=> {}`"),
			},

			DecoderOptions::LOADALL386 => match info.code() {
				Code::Loadall386 => {}
				Code::Sysretd | Code::Sysretq => continue,
				_ => unreachable!("Update this code: `=> continue` or `=> {}`"),
			},

			DecoderOptions::MOV_TR => match info.code() {
				Code::Mov_r32_tr | Code::Mov_tr_r32 => {}
				_ => unreachable!("Update this code: `=> continue` or `=> {}`"),
			},

			DecoderOptions::MPX => match info.code() {
				Code::Bndcl_bnd_rm32
				| Code::Bndcl_bnd_rm64
				| Code::Bndcn_bnd_rm32
				| Code::Bndcn_bnd_rm64
				| Code::Bndcu_bnd_rm32
				| Code::Bndcu_bnd_rm64
				| Code::Bndldx_bnd_mib
				| Code::Bndmk_bnd_m32
				| Code::Bndmk_bnd_m64
				| Code::Bndmov_bnd_bndm128
				| Code::Bndmov_bnd_bndm64
				| Code::Bndmov_bndm128_bnd
				| Code::Bndmov_bndm64_bnd
				| Code::Bndstx_mib_bnd => {}
				_ => unreachable!("Update this code: `=> continue` or `=> {}`"),
			},

			DecoderOptions::OLD_FPU => match info.code() {
				Code::Frstpm | Code::Fstdw_AX | Code::Fstsg_AX => {}
				_ => unreachable!("Update this code: `=> continue` or `=> {}`"),
			},

			DecoderOptions::PCOMMIT => match info.code() {
				Code::Pcommit => {}
				_ => unreachable!("Update this code: `=> continue` or `=> {}`"),
			},

			DecoderOptions::UMOV => match info.code() {
				Code::Umov_r16_rm16 | Code::Umov_r32_rm32 | Code::Umov_r8_rm8 | Code::Umov_rm16_r16 | Code::Umov_rm32_r32 | Code::Umov_rm8_r8 => {}
				Code::Movups_xmm_xmmm128
				| Code::Movupd_xmm_xmmm128
				| Code::Movss_xmm_xmmm32
				| Code::Movsd_xmm_xmmm64
				| Code::Movups_xmmm128_xmm
				| Code::Movupd_xmmm128_xmm
				| Code::Movss_xmmm32_xmm
				| Code::Movsd_xmmm64_xmm
				| Code::Movhlps_xmm_xmm
				| Code::Movlps_xmm_m64
				| Code::Movlpd_xmm_m64
				| Code::Movsldup_xmm_xmmm128
				| Code::Movddup_xmm_xmmm64
				| Code::Movlps_m64_xmm
				| Code::Movlpd_m64_xmm => continue,
				_ => unreachable!("Update this code: `=> continue` or `=> {}`"),
			},

			DecoderOptions::XBTS => match info.code() {
				Code::Ibts_rm16_r16 | Code::Ibts_rm32_r32 | Code::Xbts_r16_rm16 | Code::Xbts_r32_rm32 => {}
				Code::Montmul_32 | Code::Montmul_64 | Code::Xsha1_32 | Code::Xsha1_64 | Code::Xsha256_32 | Code::Xsha256_64 => continue,
				_ => unreachable!("Update this code: `=> continue` or `=> {}`"),
			},

			_ => unreachable!("Update this code"),
		}

		{
			let bytes = to_vec_u8(info.hex_bytes()).unwrap();
			let mut decoder = Decoder::new(info.bitness(), &bytes, info.decoder_options());
			let instruction = decoder.decode();
			assert_eq!(info.code(), instruction.code());
		}
		{
			let bytes = to_vec_u8(&format!("{}{}", info.hex_bytes(), extra_bytes)).unwrap();
			let mut decoder = Decoder::new(info.bitness(), &bytes, DecoderOptions::NONE);
			let instruction = decoder.decode();
			assert_ne!(info.code(), instruction.code());
		}
	}

	fn is_power_of_two(v: u32) -> bool {
		v != 0 && (v & (v - 1)) == 0
	}
}
