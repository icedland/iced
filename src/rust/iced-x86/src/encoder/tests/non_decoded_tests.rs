// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::encoder::*;
use alloc::vec::Vec;
use lazy_static::lazy_static;

pub(crate) fn get_tests() -> Vec<(u32, &'static str, Instruction)> {
	let mut v = Vec::with_capacity(INFOS16.len() + INFOS32.len() + INFOS64.len());
	for &(hex_bytes, instr) in &*INFOS16 {
		v.push((16, hex_bytes, instr));
	}
	for &(hex_bytes, instr) in &*INFOS32 {
		v.push((32, hex_bytes, instr));
	}
	for &(hex_bytes, instr) in &*INFOS64 {
		v.push((64, hex_bytes, instr));
	}
	v
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub(crate) fn get_infos(bitness: u32) -> &'static Vec<(&'static str, Instruction)> {
	match bitness {
		16 => &INFOS16,
		32 => &INFOS32,
		64 => &INFOS64,
		_ => unreachable!(),
	}
}

fn c16(mut instruction: Instruction) -> Instruction {
	instruction.set_code_size(CodeSize::Code16);
	instruction
}

fn c32(mut instruction: Instruction) -> Instruction {
	instruction.set_code_size(CodeSize::Code32);
	instruction
}

fn c64(mut instruction: Instruction) -> Instruction {
	instruction.set_code_size(CodeSize::Code64);
	instruction
}

lazy_static! {
	static ref INFOS16: Vec<(&'static str, Instruction)> = {
		#[rustfmt::skip]
		let array = [
			("0F", c16(Instruction::with1(Code::Popw_CS, Register::CS).unwrap())),
			("9B D9 30", c16(Instruction::with1(Code::Fstenv_m14byte, MemoryOperand::new(Register::BX, Register::SI, 1, 0, 0, false, Register::None)).unwrap())),
			("9B 64 D9 30", c16(Instruction::with1(Code::Fstenv_m14byte, MemoryOperand::new(Register::BX, Register::SI, 1, 0, 0, false, Register::FS)).unwrap())),
			("9B 66 D9 30", c16(Instruction::with1(Code::Fstenv_m28byte, MemoryOperand::new(Register::BX, Register::SI, 1, 0, 0, false, Register::None)).unwrap())),
			("9B 64 66 D9 30", c16(Instruction::with1(Code::Fstenv_m28byte, MemoryOperand::new(Register::BX, Register::SI, 1, 0, 0, false, Register::FS)).unwrap())),
			("9B D9 38", c16(Instruction::with1(Code::Fstcw_m2byte, MemoryOperand::new(Register::BX, Register::SI, 1, 0, 0, false, Register::None)).unwrap())),
			("9B 64 D9 38", c16(Instruction::with1(Code::Fstcw_m2byte, MemoryOperand::new(Register::BX, Register::SI, 1, 0, 0, false, Register::FS)).unwrap())),
			("9B DB E0", c16(Instruction::with(Code::Feni))),
			("9B DB E1", c16(Instruction::with(Code::Fdisi))),
			("9B DB E2", c16(Instruction::with(Code::Fclex))),
			("9B DB E3", c16(Instruction::with(Code::Finit))),
			("9B DB E4", c16(Instruction::with(Code::Fsetpm))),
			("9B DD 30", c16(Instruction::with1(Code::Fsave_m94byte, MemoryOperand::new(Register::BX, Register::SI, 1, 0, 0, false, Register::None)).unwrap())),
			("9B 64 DD 30", c16(Instruction::with1(Code::Fsave_m94byte, MemoryOperand::new(Register::BX, Register::SI, 1, 0, 0, false, Register::FS)).unwrap())),
			("9B 66 DD 30", c16(Instruction::with1(Code::Fsave_m108byte, MemoryOperand::new(Register::BX, Register::SI, 1, 0, 0, false, Register::None)).unwrap())),
			("9B 64 66 DD 30", c16(Instruction::with1(Code::Fsave_m108byte, MemoryOperand::new(Register::BX, Register::SI, 1, 0, 0, false, Register::FS)).unwrap())),
			("9B DD 38", c16(Instruction::with1(Code::Fstsw_m2byte, MemoryOperand::new(Register::BX, Register::SI, 1, 0, 0, false, Register::None)).unwrap())),
			("9B 64 DD 38", c16(Instruction::with1(Code::Fstsw_m2byte, MemoryOperand::new(Register::BX, Register::SI, 1, 0, 0, false, Register::FS)).unwrap())),
			("9B DF E0", c16(Instruction::with1(Code::Fstsw_AX, Register::AX).unwrap())),
			("9B DF E1", c16(Instruction::with1(Code::Fstdw_AX, Register::AX).unwrap())),
			("9B DF E2", c16(Instruction::with1(Code::Fstsg_AX, Register::AX).unwrap())),
			("", c16(Instruction::with(Code::Zero_bytes))),
		];
		#[rustfmt::skip]
		let array_db = [
			("77", c16(Instruction::try_with_declare_byte_1(0x77).unwrap())),
			("77 A9", c16(Instruction::try_with_declare_byte_2(0x77, 0xA9).unwrap())),
			("77 A9 CE", c16(Instruction::try_with_declare_byte_3(0x77, 0xA9, 0xCE).unwrap())),
			("77 A9 CE 9D", c16(Instruction::try_with_declare_byte_4(0x77, 0xA9, 0xCE, 0x9D).unwrap())),
			("77 A9 CE 9D 55", c16(Instruction::try_with_declare_byte_5(0x77, 0xA9, 0xCE, 0x9D, 0x55).unwrap())),
			("77 A9 CE 9D 55 05", c16(Instruction::try_with_declare_byte_6(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05).unwrap())),
			("77 A9 CE 9D 55 05 42", c16(Instruction::try_with_declare_byte_7(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42).unwrap())),
			("77 A9 CE 9D 55 05 42 6C", c16(Instruction::try_with_declare_byte_8(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C).unwrap())),
			("77 A9 CE 9D 55 05 42 6C 86", c16(Instruction::try_with_declare_byte_9(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86).unwrap())),
			("77 A9 CE 9D 55 05 42 6C 86 32", c16(Instruction::try_with_declare_byte_10(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32).unwrap())),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE", c16(Instruction::try_with_declare_byte_11(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE).unwrap())),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F", c16(Instruction::try_with_declare_byte_12(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F).unwrap())),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34", c16(Instruction::try_with_declare_byte_13(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34).unwrap())),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34 27", c16(Instruction::try_with_declare_byte_14(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27).unwrap())),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34 27 AA", c16(Instruction::try_with_declare_byte_15(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA).unwrap())),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34 27 AA 08", c16(Instruction::try_with_declare_byte_16(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08).unwrap())),
			("A977", c16(Instruction::try_with_declare_word_1(0x77A9).unwrap())),
			("A977 9DCE", c16(Instruction::try_with_declare_word_2(0x77A9, 0xCE9D).unwrap())),
			("A977 9DCE 0555", c16(Instruction::try_with_declare_word_3(0x77A9, 0xCE9D, 0x5505).unwrap())),
			("A977 9DCE 0555 6C42", c16(Instruction::try_with_declare_word_4(0x77A9, 0xCE9D, 0x5505, 0x426C).unwrap())),
			("A977 9DCE 0555 6C42 3286", c16(Instruction::try_with_declare_word_5(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632).unwrap())),
			("A977 9DCE 0555 6C42 3286 4FFE", c16(Instruction::try_with_declare_word_6(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F).unwrap())),
			("A977 9DCE 0555 6C42 3286 4FFE 2734", c16(Instruction::try_with_declare_word_7(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427).unwrap())),
			("A977 9DCE 0555 6C42 3286 4FFE 2734 08AA", c16(Instruction::try_with_declare_word_8(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08).unwrap())),
			("9DCEA977", c16(Instruction::try_with_declare_dword_1(0x77A9_CE9D).unwrap())),
			("9DCEA977 6C420555", c16(Instruction::try_with_declare_dword_2(0x77A9_CE9D, 0x5505_426C).unwrap())),
			("9DCEA977 6C420555 4FFE3286", c16(Instruction::try_with_declare_dword_3(0x77A9_CE9D, 0x5505_426C, 0x8632_FE4F).unwrap())),
			("9DCEA977 6C420555 4FFE3286 08AA2734", c16(Instruction::try_with_declare_dword_4(0x77A9_CE9D, 0x5505_426C, 0x8632_FE4F, 0x3427_AA08).unwrap())),
			("6C4205559DCEA977", c16(Instruction::try_with_declare_qword_1(0x77A9_CE9D_5505_426C).unwrap())),
			("6C4205559DCEA977 08AA27344FFE3286", c16(Instruction::try_with_declare_qword_2(0x77A9_CE9D_5505_426C, 0x8632_FE4F_3427_AA08).unwrap())),
		];
		array.iter().copied().chain(array_db.iter().copied()).collect()
	};
}

lazy_static! {
	static ref INFOS32: Vec<(&'static str, Instruction)> = {
		#[rustfmt::skip]
		let array = [
			("66 0F", c32(Instruction::with1(Code::Popw_CS, Register::CS).unwrap())),
			("9B 66 D9 30", c32(Instruction::with1(Code::Fstenv_m14byte, MemoryOperand::new(Register::EAX, Register::None, 1, 0, 0, false, Register::None)).unwrap())),
			("9B 64 66 D9 30", c32(Instruction::with1(Code::Fstenv_m14byte, MemoryOperand::new(Register::EAX, Register::None, 1, 0, 0, false, Register::FS)).unwrap())),
			("9B D9 30", c32(Instruction::with1(Code::Fstenv_m28byte, MemoryOperand::new(Register::EAX, Register::None, 1, 0, 0, false, Register::None)).unwrap())),
			("9B 64 D9 30", c32(Instruction::with1(Code::Fstenv_m28byte, MemoryOperand::new(Register::EAX, Register::None, 1, 0, 0, false, Register::FS)).unwrap())),
			("9B D9 38", c32(Instruction::with1(Code::Fstcw_m2byte, MemoryOperand::new(Register::EAX, Register::None, 1, 0, 0, false, Register::None)).unwrap())),
			("9B 64 D9 38", c32(Instruction::with1(Code::Fstcw_m2byte, MemoryOperand::new(Register::EAX, Register::None, 1, 0, 0, false, Register::FS)).unwrap())),
			("9B DB E0", c32(Instruction::with(Code::Feni))),
			("9B DB E1", c32(Instruction::with(Code::Fdisi))),
			("9B DB E2", c32(Instruction::with(Code::Fclex))),
			("9B DB E3", c32(Instruction::with(Code::Finit))),
			("9B DB E4", c32(Instruction::with(Code::Fsetpm))),
			("9B 66 DD 30", c32(Instruction::with1(Code::Fsave_m94byte, MemoryOperand::new(Register::EAX, Register::None, 1, 0, 0, false, Register::None)).unwrap())),
			("9B 64 66 DD 30", c32(Instruction::with1(Code::Fsave_m94byte, MemoryOperand::new(Register::EAX, Register::None, 1, 0, 0, false, Register::FS)).unwrap())),
			("9B DD 30", c32(Instruction::with1(Code::Fsave_m108byte, MemoryOperand::new(Register::EAX, Register::None, 1, 0, 0, false, Register::None)).unwrap())),
			("9B 64 DD 30", c32(Instruction::with1(Code::Fsave_m108byte, MemoryOperand::new(Register::EAX, Register::None, 1, 0, 0, false, Register::FS)).unwrap())),
			("9B DD 38", c32(Instruction::with1(Code::Fstsw_m2byte, MemoryOperand::new(Register::EAX, Register::None, 1, 0, 0, false, Register::None)).unwrap())),
			("9B 64 DD 38", c32(Instruction::with1(Code::Fstsw_m2byte, MemoryOperand::new(Register::EAX, Register::None, 1, 0, 0, false, Register::FS)).unwrap())),
			("9B DF E0", c32(Instruction::with1(Code::Fstsw_AX, Register::AX).unwrap())),
			("9B DF E1", c32(Instruction::with1(Code::Fstdw_AX, Register::AX).unwrap())),
			("9B DF E2", c32(Instruction::with1(Code::Fstsg_AX, Register::AX).unwrap())),
			("", c32(Instruction::with(Code::Zero_bytes))),
		];
		#[rustfmt::skip]
		let array_db = [
			("77", c32(Instruction::try_with_declare_byte_1(0x77).unwrap())),
			("77 A9", c32(Instruction::try_with_declare_byte_2(0x77, 0xA9).unwrap())),
			("77 A9 CE", c32(Instruction::try_with_declare_byte_3(0x77, 0xA9, 0xCE).unwrap())),
			("77 A9 CE 9D", c32(Instruction::try_with_declare_byte_4(0x77, 0xA9, 0xCE, 0x9D).unwrap())),
			("77 A9 CE 9D 55", c32(Instruction::try_with_declare_byte_5(0x77, 0xA9, 0xCE, 0x9D, 0x55).unwrap())),
			("77 A9 CE 9D 55 05", c32(Instruction::try_with_declare_byte_6(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05).unwrap())),
			("77 A9 CE 9D 55 05 42", c32(Instruction::try_with_declare_byte_7(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42).unwrap())),
			("77 A9 CE 9D 55 05 42 6C", c32(Instruction::try_with_declare_byte_8(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C).unwrap())),
			("77 A9 CE 9D 55 05 42 6C 86", c32(Instruction::try_with_declare_byte_9(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86).unwrap())),
			("77 A9 CE 9D 55 05 42 6C 86 32", c32(Instruction::try_with_declare_byte_10(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32).unwrap())),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE", c32(Instruction::try_with_declare_byte_11(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE).unwrap())),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F", c32(Instruction::try_with_declare_byte_12(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F).unwrap())),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34", c32(Instruction::try_with_declare_byte_13(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34).unwrap())),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34 27", c32(Instruction::try_with_declare_byte_14(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27).unwrap())),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34 27 AA", c32(Instruction::try_with_declare_byte_15(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA).unwrap())),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34 27 AA 08", c32(Instruction::try_with_declare_byte_16(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08).unwrap())),
			("A977", c32(Instruction::try_with_declare_word_1(0x77A9).unwrap())),
			("A977 9DCE", c32(Instruction::try_with_declare_word_2(0x77A9, 0xCE9D).unwrap())),
			("A977 9DCE 0555", c32(Instruction::try_with_declare_word_3(0x77A9, 0xCE9D, 0x5505).unwrap())),
			("A977 9DCE 0555 6C42", c32(Instruction::try_with_declare_word_4(0x77A9, 0xCE9D, 0x5505, 0x426C).unwrap())),
			("A977 9DCE 0555 6C42 3286", c32(Instruction::try_with_declare_word_5(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632).unwrap())),
			("A977 9DCE 0555 6C42 3286 4FFE", c32(Instruction::try_with_declare_word_6(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F).unwrap())),
			("A977 9DCE 0555 6C42 3286 4FFE 2734", c32(Instruction::try_with_declare_word_7(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427).unwrap())),
			("A977 9DCE 0555 6C42 3286 4FFE 2734 08AA", c32(Instruction::try_with_declare_word_8(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08).unwrap())),
			("9DCEA977", c32(Instruction::try_with_declare_dword_1(0x77A9_CE9D).unwrap())),
			("9DCEA977 6C420555", c32(Instruction::try_with_declare_dword_2(0x77A9_CE9D, 0x5505_426C).unwrap())),
			("9DCEA977 6C420555 4FFE3286", c32(Instruction::try_with_declare_dword_3(0x77A9_CE9D, 0x5505_426C, 0x8632_FE4F).unwrap())),
			("9DCEA977 6C420555 4FFE3286 08AA2734", c32(Instruction::try_with_declare_dword_4(0x77A9_CE9D, 0x5505_426C, 0x8632_FE4F, 0x3427_AA08).unwrap())),
			("6C4205559DCEA977", c32(Instruction::try_with_declare_qword_1(0x77A9_CE9D_5505_426C).unwrap())),
			("6C4205559DCEA977 08AA27344FFE3286", c32(Instruction::try_with_declare_qword_2(0x77A9_CE9D_5505_426C, 0x8632_FE4F_3427_AA08).unwrap())),
		];
		array.iter().copied().chain(array_db.iter().copied()).collect()
	};
}

lazy_static! {
	static ref INFOS64: Vec<(&'static str, Instruction)> = {
		#[rustfmt::skip]
		let array = [
			("9B 66 D9 30", c64(Instruction::with1(Code::Fstenv_m14byte, MemoryOperand::new(Register::RAX, Register::None, 1, 0, 0, false, Register::None)).unwrap())),
			("9B 64 66 D9 30", c64(Instruction::with1(Code::Fstenv_m14byte, MemoryOperand::new(Register::RAX, Register::None, 1, 0, 0, false, Register::FS)).unwrap())),
			("9B D9 30", c64(Instruction::with1(Code::Fstenv_m28byte, MemoryOperand::new(Register::RAX, Register::None, 1, 0, 0, false, Register::None)).unwrap())),
			("9B 64 D9 30", c64(Instruction::with1(Code::Fstenv_m28byte, MemoryOperand::new(Register::RAX, Register::None, 1, 0, 0, false, Register::FS)).unwrap())),
			("9B D9 38", c64(Instruction::with1(Code::Fstcw_m2byte, MemoryOperand::new(Register::RAX, Register::None, 1, 0, 0, false, Register::None)).unwrap())),
			("9B 64 D9 38", c64(Instruction::with1(Code::Fstcw_m2byte, MemoryOperand::new(Register::RAX, Register::None, 1, 0, 0, false, Register::FS)).unwrap())),
			("9B DB E0", c64(Instruction::with(Code::Feni))),
			("9B DB E1", c64(Instruction::with(Code::Fdisi))),
			("9B DB E2", c64(Instruction::with(Code::Fclex))),
			("9B DB E3", c64(Instruction::with(Code::Finit))),
			("9B DB E4", c64(Instruction::with(Code::Fsetpm))),
			("9B 66 DD 30", c64(Instruction::with1(Code::Fsave_m94byte, MemoryOperand::new(Register::RAX, Register::None, 1, 0, 0, false, Register::None)).unwrap())),
			("9B 64 66 DD 30", c64(Instruction::with1(Code::Fsave_m94byte, MemoryOperand::new(Register::RAX, Register::None, 1, 0, 0, false, Register::FS)).unwrap())),
			("9B DD 30", c64(Instruction::with1(Code::Fsave_m108byte, MemoryOperand::new(Register::RAX, Register::None, 1, 0, 0, false, Register::None)).unwrap())),
			("9B 64 DD 30", c64(Instruction::with1(Code::Fsave_m108byte, MemoryOperand::new(Register::RAX, Register::None, 1, 0, 0, false, Register::FS)).unwrap())),
			("9B DD 38", c64(Instruction::with1(Code::Fstsw_m2byte, MemoryOperand::new(Register::RAX, Register::None, 1, 0, 0, false, Register::None)).unwrap())),
			("9B 64 DD 38", c64(Instruction::with1(Code::Fstsw_m2byte, MemoryOperand::new(Register::RAX, Register::None, 1, 0, 0, false, Register::FS)).unwrap())),
			("9B DF E0", c64(Instruction::with1(Code::Fstsw_AX, Register::AX).unwrap())),
			("", c64(Instruction::with(Code::Zero_bytes))),
		];
		#[rustfmt::skip]
		let array_db = [
			("77", c64(Instruction::try_with_declare_byte_1(0x77).unwrap())),
			("77 A9", c64(Instruction::try_with_declare_byte_2(0x77, 0xA9).unwrap())),
			("77 A9 CE", c64(Instruction::try_with_declare_byte_3(0x77, 0xA9, 0xCE).unwrap())),
			("77 A9 CE 9D", c64(Instruction::try_with_declare_byte_4(0x77, 0xA9, 0xCE, 0x9D).unwrap())),
			("77 A9 CE 9D 55", c64(Instruction::try_with_declare_byte_5(0x77, 0xA9, 0xCE, 0x9D, 0x55).unwrap())),
			("77 A9 CE 9D 55 05", c64(Instruction::try_with_declare_byte_6(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05).unwrap())),
			("77 A9 CE 9D 55 05 42", c64(Instruction::try_with_declare_byte_7(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42).unwrap())),
			("77 A9 CE 9D 55 05 42 6C", c64(Instruction::try_with_declare_byte_8(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C).unwrap())),
			("77 A9 CE 9D 55 05 42 6C 86", c64(Instruction::try_with_declare_byte_9(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86).unwrap())),
			("77 A9 CE 9D 55 05 42 6C 86 32", c64(Instruction::try_with_declare_byte_10(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32).unwrap())),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE", c64(Instruction::try_with_declare_byte_11(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE).unwrap())),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F", c64(Instruction::try_with_declare_byte_12(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F).unwrap())),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34", c64(Instruction::try_with_declare_byte_13(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34).unwrap())),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34 27", c64(Instruction::try_with_declare_byte_14(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27).unwrap())),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34 27 AA", c64(Instruction::try_with_declare_byte_15(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA).unwrap())),
			("77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34 27 AA 08", c64(Instruction::try_with_declare_byte_16(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08).unwrap())),
			("A977", c64(Instruction::try_with_declare_word_1(0x77A9).unwrap())),
			("A977 9DCE", c64(Instruction::try_with_declare_word_2(0x77A9, 0xCE9D).unwrap())),
			("A977 9DCE 0555", c64(Instruction::try_with_declare_word_3(0x77A9, 0xCE9D, 0x5505).unwrap())),
			("A977 9DCE 0555 6C42", c64(Instruction::try_with_declare_word_4(0x77A9, 0xCE9D, 0x5505, 0x426C).unwrap())),
			("A977 9DCE 0555 6C42 3286", c64(Instruction::try_with_declare_word_5(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632).unwrap())),
			("A977 9DCE 0555 6C42 3286 4FFE", c64(Instruction::try_with_declare_word_6(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F).unwrap())),
			("A977 9DCE 0555 6C42 3286 4FFE 2734", c64(Instruction::try_with_declare_word_7(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427).unwrap())),
			("A977 9DCE 0555 6C42 3286 4FFE 2734 08AA", c64(Instruction::try_with_declare_word_8(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08).unwrap())),
			("9DCEA977", c64(Instruction::try_with_declare_dword_1(0x77A9_CE9D).unwrap())),
			("9DCEA977 6C420555", c64(Instruction::try_with_declare_dword_2(0x77A9_CE9D, 0x5505_426C).unwrap())),
			("9DCEA977 6C420555 4FFE3286", c64(Instruction::try_with_declare_dword_3(0x77A9_CE9D, 0x5505_426C, 0x8632_FE4F).unwrap())),
			("9DCEA977 6C420555 4FFE3286 08AA2734", c64(Instruction::try_with_declare_dword_4(0x77A9_CE9D, 0x5505_426C, 0x8632_FE4F, 0x3427_AA08).unwrap())),
			("6C4205559DCEA977", c64(Instruction::try_with_declare_qword_1(0x77A9_CE9D_5505_426C).unwrap())),
			("6C4205559DCEA977 08AA27344FFE3286", c64(Instruction::try_with_declare_qword_2(0x77A9_CE9D_5505_426C, 0x8632_FE4F_3427_AA08).unwrap())),
		];
		array.iter().copied().chain(array_db.iter().copied()).collect()
	};
}
