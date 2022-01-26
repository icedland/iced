// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::encoder::test_utils::from_str_conv::to_vec_u8;
use crate::encoder::test_utils::*;
use crate::encoder::*;
use alloc::vec::Vec;
use std::panic;

#[test]
fn encoder_ignores_prefixes_if_declare_data() {
	let instr = Instruction::with_declare_byte_16(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08);
	verify(&instr);

	let instr = Instruction::with_declare_word_8(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08);
	verify(&instr);

	let instr = Instruction::with_declare_dword_4(0x77A9_CE9D, 0x5505_426C, 0x8632_FE4F, 0x3427_AA08);
	verify(&instr);

	let instr = Instruction::with_declare_qword_2(0x77A9_CE9D_5505_426C, 0x8632_FE4F_3427_AA08);
	verify(&instr);

	let instr = Instruction::try_with_declare_byte_16(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08)
		.unwrap();
	verify(&instr);

	let instr = Instruction::try_with_declare_word_8(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08).unwrap();
	verify(&instr);

	let instr = Instruction::try_with_declare_dword_4(0x77A9_CE9D, 0x5505_426C, 0x8632_FE4F, 0x3427_AA08).unwrap();
	verify(&instr);

	let instr = Instruction::try_with_declare_qword_2(0x77A9_CE9D_5505_426C, 0x8632_FE4F_3427_AA08).unwrap();
	verify(&instr);

	fn verify(instr: &Instruction) {
		let mut instr = *instr;
		let orig_data = get_data(&instr);
		instr.set_has_lock_prefix(true);
		instr.set_has_repe_prefix(true);
		instr.set_has_repne_prefix(true);
		instr.set_segment_prefix(Register::GS);
		instr.set_has_xrelease_prefix(true);
		instr.set_has_xacquire_prefix(true);
		instr.set_suppress_all_exceptions(true);
		instr.set_zeroing_masking(true);
		for &bitness in &[16, 32, 64] {
			let mut encoder = Encoder::new(bitness);
			let _ = encoder.encode(&instr, 0).unwrap();
			assert_eq!(encoder.take_buffer(), orig_data);
		}
	}
}

fn get_data(instr: &Instruction) -> Vec<u8> {
	let length = instr.declare_data_len()
		* match instr.code() {
			Code::DeclareByte => 1,
			Code::DeclareWord => 2,
			Code::DeclareDword => 4,
			Code::DeclareQword => 8,
			_ => unreachable!(),
		};
	let mut v = Vec::with_capacity(length);
	for i in 0..length {
		v.push(instr.get_declare_byte_value(i));
	}
	v
}

#[test]
fn declare_data_byte_order_is_same() {
	let data = vec![0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08];
	let db = Instruction::with_declare_byte_16(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08);
	let dw = Instruction::with_declare_word_8(0xA977, 0x9DCE, 0x0555, 0x6C42, 0x3286, 0x4FFE, 0x2734, 0x08AA);
	let dd = Instruction::with_declare_dword_4(0x9DCE_A977, 0x6C42_0555, 0x4FFE_3286, 0x08AA_2734);
	let dq = Instruction::with_declare_qword_2(0x6C42_0555_9DCE_A977, 0x08AA_2734_4FFE_3286);
	let data1 = get_data(&db);
	let data2 = get_data(&dw);
	let data4 = get_data(&dd);
	let data8 = get_data(&dq);
	assert_eq!(data1, data);
	assert_eq!(data2, data);
	assert_eq!(data4, data);
	assert_eq!(data8, data);
}

#[test]
fn try_declare_data_byte_order_is_same() {
	let data = vec![0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08];
	let db = Instruction::try_with_declare_byte_16(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08)
		.unwrap();
	let dw = Instruction::try_with_declare_word_8(0xA977, 0x9DCE, 0x0555, 0x6C42, 0x3286, 0x4FFE, 0x2734, 0x08AA).unwrap();
	let dd = Instruction::try_with_declare_dword_4(0x9DCE_A977, 0x6C42_0555, 0x4FFE_3286, 0x08AA_2734).unwrap();
	let dq = Instruction::try_with_declare_qword_2(0x6C42_0555_9DCE_A977, 0x08AA_2734_4FFE_3286).unwrap();
	let data1 = get_data(&db);
	let data2 = get_data(&dw);
	let data4 = get_data(&dd);
	let data8 = get_data(&dq);
	assert_eq!(data1, data);
	assert_eq!(data2, data);
	assert_eq!(data4, data);
	assert_eq!(data8, data);
}

#[test]
fn declare_byte_can_get_set() {
	let mut db = Instruction::with_declare_byte_16(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08);
	db.set_declare_byte_value(0, 0xE2);
	db.set_declare_byte_value(1, 0xC5);
	db.set_declare_byte_value(2, 0xFA);
	db.set_declare_byte_value(3, 0xB4);
	db.set_declare_byte_value(4, 0xCB);
	db.set_declare_byte_value(5, 0xE3);
	db.set_declare_byte_value(6, 0x4D);
	db.set_declare_byte_value(7, 0xE4);
	db.set_declare_byte_value(8, 0x96);
	db.set_declare_byte_value(9, 0x98);
	db.set_declare_byte_value(10, 0xFD);
	db.set_declare_byte_value(11, 0x56);
	db.set_declare_byte_value(12, 0x82);
	db.set_declare_byte_value(13, 0x8D);
	db.set_declare_byte_value(14, 0x06);
	db.set_declare_byte_value(15, 0xC3);
	assert_eq!(db.get_declare_byte_value(0), 0xE2);
	assert_eq!(db.get_declare_byte_value(1), 0xC5);
	assert_eq!(db.get_declare_byte_value(2), 0xFA);
	assert_eq!(db.get_declare_byte_value(3), 0xB4);
	assert_eq!(db.get_declare_byte_value(4), 0xCB);
	assert_eq!(db.get_declare_byte_value(5), 0xE3);
	assert_eq!(db.get_declare_byte_value(6), 0x4D);
	assert_eq!(db.get_declare_byte_value(7), 0xE4);
	assert_eq!(db.get_declare_byte_value(8), 0x96);
	assert_eq!(db.get_declare_byte_value(9), 0x98);
	assert_eq!(db.get_declare_byte_value(10), 0xFD);
	assert_eq!(db.get_declare_byte_value(11), 0x56);
	assert_eq!(db.get_declare_byte_value(12), 0x82);
	assert_eq!(db.get_declare_byte_value(13), 0x8D);
	assert_eq!(db.get_declare_byte_value(14), 0x06);
	assert_eq!(db.get_declare_byte_value(15), 0xC3);
}

#[test]
fn declare_byte_can_get_set_rev() {
	let mut db = Instruction::with_declare_byte_16(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08);
	db.set_declare_byte_value(15, 0xC3);
	db.set_declare_byte_value(14, 0x06);
	db.set_declare_byte_value(13, 0x8D);
	db.set_declare_byte_value(12, 0x82);
	db.set_declare_byte_value(11, 0x56);
	db.set_declare_byte_value(10, 0xFD);
	db.set_declare_byte_value(9, 0x98);
	db.set_declare_byte_value(8, 0x96);
	db.set_declare_byte_value(7, 0xE4);
	db.set_declare_byte_value(6, 0x4D);
	db.set_declare_byte_value(5, 0xE3);
	db.set_declare_byte_value(4, 0xCB);
	db.set_declare_byte_value(3, 0xB4);
	db.set_declare_byte_value(2, 0xFA);
	db.set_declare_byte_value(1, 0xC5);
	db.set_declare_byte_value(0, 0xE2);
	assert_eq!(db.get_declare_byte_value(0), 0xE2);
	assert_eq!(db.get_declare_byte_value(1), 0xC5);
	assert_eq!(db.get_declare_byte_value(2), 0xFA);
	assert_eq!(db.get_declare_byte_value(3), 0xB4);
	assert_eq!(db.get_declare_byte_value(4), 0xCB);
	assert_eq!(db.get_declare_byte_value(5), 0xE3);
	assert_eq!(db.get_declare_byte_value(6), 0x4D);
	assert_eq!(db.get_declare_byte_value(7), 0xE4);
	assert_eq!(db.get_declare_byte_value(8), 0x96);
	assert_eq!(db.get_declare_byte_value(9), 0x98);
	assert_eq!(db.get_declare_byte_value(10), 0xFD);
	assert_eq!(db.get_declare_byte_value(11), 0x56);
	assert_eq!(db.get_declare_byte_value(12), 0x82);
	assert_eq!(db.get_declare_byte_value(13), 0x8D);
	assert_eq!(db.get_declare_byte_value(14), 0x06);
	assert_eq!(db.get_declare_byte_value(15), 0xC3);
}

#[test]
fn try_declare_byte_can_get_set() {
	let mut db =
		Instruction::try_with_declare_byte_16(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08)
			.unwrap();
	db.try_set_declare_byte_value(0, 0xE2).unwrap();
	db.try_set_declare_byte_value(1, 0xC5).unwrap();
	db.try_set_declare_byte_value(2, 0xFA).unwrap();
	db.try_set_declare_byte_value(3, 0xB4).unwrap();
	db.try_set_declare_byte_value(4, 0xCB).unwrap();
	db.try_set_declare_byte_value(5, 0xE3).unwrap();
	db.try_set_declare_byte_value(6, 0x4D).unwrap();
	db.try_set_declare_byte_value(7, 0xE4).unwrap();
	db.try_set_declare_byte_value(8, 0x96).unwrap();
	db.try_set_declare_byte_value(9, 0x98).unwrap();
	db.try_set_declare_byte_value(10, 0xFD).unwrap();
	db.try_set_declare_byte_value(11, 0x56).unwrap();
	db.try_set_declare_byte_value(12, 0x82).unwrap();
	db.try_set_declare_byte_value(13, 0x8D).unwrap();
	db.try_set_declare_byte_value(14, 0x06).unwrap();
	db.try_set_declare_byte_value(15, 0xC3).unwrap();
	assert_eq!(db.try_get_declare_byte_value(0).unwrap(), 0xE2);
	assert_eq!(db.try_get_declare_byte_value(1).unwrap(), 0xC5);
	assert_eq!(db.try_get_declare_byte_value(2).unwrap(), 0xFA);
	assert_eq!(db.try_get_declare_byte_value(3).unwrap(), 0xB4);
	assert_eq!(db.try_get_declare_byte_value(4).unwrap(), 0xCB);
	assert_eq!(db.try_get_declare_byte_value(5).unwrap(), 0xE3);
	assert_eq!(db.try_get_declare_byte_value(6).unwrap(), 0x4D);
	assert_eq!(db.try_get_declare_byte_value(7).unwrap(), 0xE4);
	assert_eq!(db.try_get_declare_byte_value(8).unwrap(), 0x96);
	assert_eq!(db.try_get_declare_byte_value(9).unwrap(), 0x98);
	assert_eq!(db.try_get_declare_byte_value(10).unwrap(), 0xFD);
	assert_eq!(db.try_get_declare_byte_value(11).unwrap(), 0x56);
	assert_eq!(db.try_get_declare_byte_value(12).unwrap(), 0x82);
	assert_eq!(db.try_get_declare_byte_value(13).unwrap(), 0x8D);
	assert_eq!(db.try_get_declare_byte_value(14).unwrap(), 0x06);
	assert_eq!(db.try_get_declare_byte_value(15).unwrap(), 0xC3);
}

#[test]
fn try_declare_byte_can_get_set_rev() {
	let mut db =
		Instruction::try_with_declare_byte_16(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08)
			.unwrap();
	db.try_set_declare_byte_value(15, 0xC3).unwrap();
	db.try_set_declare_byte_value(14, 0x06).unwrap();
	db.try_set_declare_byte_value(13, 0x8D).unwrap();
	db.try_set_declare_byte_value(12, 0x82).unwrap();
	db.try_set_declare_byte_value(11, 0x56).unwrap();
	db.try_set_declare_byte_value(10, 0xFD).unwrap();
	db.try_set_declare_byte_value(9, 0x98).unwrap();
	db.try_set_declare_byte_value(8, 0x96).unwrap();
	db.try_set_declare_byte_value(7, 0xE4).unwrap();
	db.try_set_declare_byte_value(6, 0x4D).unwrap();
	db.try_set_declare_byte_value(5, 0xE3).unwrap();
	db.try_set_declare_byte_value(4, 0xCB).unwrap();
	db.try_set_declare_byte_value(3, 0xB4).unwrap();
	db.try_set_declare_byte_value(2, 0xFA).unwrap();
	db.try_set_declare_byte_value(1, 0xC5).unwrap();
	db.try_set_declare_byte_value(0, 0xE2).unwrap();
	assert_eq!(db.try_get_declare_byte_value(0).unwrap(), 0xE2);
	assert_eq!(db.try_get_declare_byte_value(1).unwrap(), 0xC5);
	assert_eq!(db.try_get_declare_byte_value(2).unwrap(), 0xFA);
	assert_eq!(db.try_get_declare_byte_value(3).unwrap(), 0xB4);
	assert_eq!(db.try_get_declare_byte_value(4).unwrap(), 0xCB);
	assert_eq!(db.try_get_declare_byte_value(5).unwrap(), 0xE3);
	assert_eq!(db.try_get_declare_byte_value(6).unwrap(), 0x4D);
	assert_eq!(db.try_get_declare_byte_value(7).unwrap(), 0xE4);
	assert_eq!(db.try_get_declare_byte_value(8).unwrap(), 0x96);
	assert_eq!(db.try_get_declare_byte_value(9).unwrap(), 0x98);
	assert_eq!(db.try_get_declare_byte_value(10).unwrap(), 0xFD);
	assert_eq!(db.try_get_declare_byte_value(11).unwrap(), 0x56);
	assert_eq!(db.try_get_declare_byte_value(12).unwrap(), 0x82);
	assert_eq!(db.try_get_declare_byte_value(13).unwrap(), 0x8D);
	assert_eq!(db.try_get_declare_byte_value(14).unwrap(), 0x06);
	assert_eq!(db.try_get_declare_byte_value(15).unwrap(), 0xC3);
}

#[test]
fn declare_word_can_get_set() {
	let mut dw = Instruction::with_declare_word_8(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08);
	dw.set_declare_word_value(0, 0xE2C5);
	dw.set_declare_word_value(1, 0xFAB4);
	dw.set_declare_word_value(2, 0xCBE3);
	dw.set_declare_word_value(3, 0x4DE4);
	dw.set_declare_word_value(4, 0x9698);
	dw.set_declare_word_value(5, 0xFD56);
	dw.set_declare_word_value(6, 0x828D);
	dw.set_declare_word_value(7, 0x06C3);
	assert_eq!(dw.get_declare_word_value(0), 0xE2C5);
	assert_eq!(dw.get_declare_word_value(1), 0xFAB4);
	assert_eq!(dw.get_declare_word_value(2), 0xCBE3);
	assert_eq!(dw.get_declare_word_value(3), 0x4DE4);
	assert_eq!(dw.get_declare_word_value(4), 0x9698);
	assert_eq!(dw.get_declare_word_value(5), 0xFD56);
	assert_eq!(dw.get_declare_word_value(6), 0x828D);
	assert_eq!(dw.get_declare_word_value(7), 0x06C3);
}

#[test]
fn declare_word_can_get_set_rev() {
	let mut dw = Instruction::with_declare_word_8(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08);
	dw.set_declare_word_value(7, 0x06C3);
	dw.set_declare_word_value(6, 0x828D);
	dw.set_declare_word_value(5, 0xFD56);
	dw.set_declare_word_value(4, 0x9698);
	dw.set_declare_word_value(3, 0x4DE4);
	dw.set_declare_word_value(2, 0xCBE3);
	dw.set_declare_word_value(1, 0xFAB4);
	dw.set_declare_word_value(0, 0xE2C5);
	assert_eq!(dw.get_declare_word_value(0), 0xE2C5);
	assert_eq!(dw.get_declare_word_value(1), 0xFAB4);
	assert_eq!(dw.get_declare_word_value(2), 0xCBE3);
	assert_eq!(dw.get_declare_word_value(3), 0x4DE4);
	assert_eq!(dw.get_declare_word_value(4), 0x9698);
	assert_eq!(dw.get_declare_word_value(5), 0xFD56);
	assert_eq!(dw.get_declare_word_value(6), 0x828D);
	assert_eq!(dw.get_declare_word_value(7), 0x06C3);
}

#[test]
fn try_declare_word_can_get_set() {
	let mut dw = Instruction::try_with_declare_word_8(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08).unwrap();
	dw.try_set_declare_word_value(0, 0xE2C5).unwrap();
	dw.try_set_declare_word_value(1, 0xFAB4).unwrap();
	dw.try_set_declare_word_value(2, 0xCBE3).unwrap();
	dw.try_set_declare_word_value(3, 0x4DE4).unwrap();
	dw.try_set_declare_word_value(4, 0x9698).unwrap();
	dw.try_set_declare_word_value(5, 0xFD56).unwrap();
	dw.try_set_declare_word_value(6, 0x828D).unwrap();
	dw.try_set_declare_word_value(7, 0x06C3).unwrap();
	assert_eq!(dw.try_get_declare_word_value(0).unwrap(), 0xE2C5);
	assert_eq!(dw.try_get_declare_word_value(1).unwrap(), 0xFAB4);
	assert_eq!(dw.try_get_declare_word_value(2).unwrap(), 0xCBE3);
	assert_eq!(dw.try_get_declare_word_value(3).unwrap(), 0x4DE4);
	assert_eq!(dw.try_get_declare_word_value(4).unwrap(), 0x9698);
	assert_eq!(dw.try_get_declare_word_value(5).unwrap(), 0xFD56);
	assert_eq!(dw.try_get_declare_word_value(6).unwrap(), 0x828D);
	assert_eq!(dw.try_get_declare_word_value(7).unwrap(), 0x06C3);
}

#[test]
fn try_declare_word_can_get_set_rev() {
	let mut dw = Instruction::try_with_declare_word_8(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08).unwrap();
	dw.try_set_declare_word_value(7, 0x06C3).unwrap();
	dw.try_set_declare_word_value(6, 0x828D).unwrap();
	dw.try_set_declare_word_value(5, 0xFD56).unwrap();
	dw.try_set_declare_word_value(4, 0x9698).unwrap();
	dw.try_set_declare_word_value(3, 0x4DE4).unwrap();
	dw.try_set_declare_word_value(2, 0xCBE3).unwrap();
	dw.try_set_declare_word_value(1, 0xFAB4).unwrap();
	dw.try_set_declare_word_value(0, 0xE2C5).unwrap();
	assert_eq!(dw.try_get_declare_word_value(0).unwrap(), 0xE2C5);
	assert_eq!(dw.try_get_declare_word_value(1).unwrap(), 0xFAB4);
	assert_eq!(dw.try_get_declare_word_value(2).unwrap(), 0xCBE3);
	assert_eq!(dw.try_get_declare_word_value(3).unwrap(), 0x4DE4);
	assert_eq!(dw.try_get_declare_word_value(4).unwrap(), 0x9698);
	assert_eq!(dw.try_get_declare_word_value(5).unwrap(), 0xFD56);
	assert_eq!(dw.try_get_declare_word_value(6).unwrap(), 0x828D);
	assert_eq!(dw.try_get_declare_word_value(7).unwrap(), 0x06C3);
}

#[test]
fn declare_dword_can_get_set() {
	let mut dd = Instruction::with_declare_dword_4(0x77A9_CE9D, 0x5505_426C, 0x8632_FE4F, 0x3427_AA08);
	dd.set_declare_dword_value(0, 0xE2C5_FAB4);
	dd.set_declare_dword_value(1, 0xCBE3_4DE4);
	dd.set_declare_dword_value(2, 0x9698_FD56);
	dd.set_declare_dword_value(3, 0x828D_06C3);
	assert_eq!(dd.get_declare_dword_value(0), 0xE2C5_FAB4);
	assert_eq!(dd.get_declare_dword_value(1), 0xCBE3_4DE4);
	assert_eq!(dd.get_declare_dword_value(2), 0x9698_FD56);
	assert_eq!(dd.get_declare_dword_value(3), 0x828D_06C3);
}

#[test]
fn declare_dword_can_get_set_rev() {
	let mut dd = Instruction::with_declare_dword_4(0x77A9_CE9D, 0x5505_426C, 0x8632_FE4F, 0x3427_AA08);
	dd.set_declare_dword_value(3, 0x828D_06C3);
	dd.set_declare_dword_value(2, 0x9698_FD56);
	dd.set_declare_dword_value(1, 0xCBE3_4DE4);
	dd.set_declare_dword_value(0, 0xE2C5_FAB4);
	assert_eq!(dd.get_declare_dword_value(0), 0xE2C5_FAB4);
	assert_eq!(dd.get_declare_dword_value(1), 0xCBE3_4DE4);
	assert_eq!(dd.get_declare_dword_value(2), 0x9698_FD56);
	assert_eq!(dd.get_declare_dword_value(3), 0x828D_06C3);
}

#[test]
fn try_declare_dword_can_get_set() {
	let mut dd = Instruction::try_with_declare_dword_4(0x77A9_CE9D, 0x5505_426C, 0x8632_FE4F, 0x3427_AA08).unwrap();
	dd.try_set_declare_dword_value(0, 0xE2C5_FAB4).unwrap();
	dd.try_set_declare_dword_value(1, 0xCBE3_4DE4).unwrap();
	dd.try_set_declare_dword_value(2, 0x9698_FD56).unwrap();
	dd.try_set_declare_dword_value(3, 0x828D_06C3).unwrap();
	assert_eq!(dd.try_get_declare_dword_value(0).unwrap(), 0xE2C5_FAB4);
	assert_eq!(dd.try_get_declare_dword_value(1).unwrap(), 0xCBE3_4DE4);
	assert_eq!(dd.try_get_declare_dword_value(2).unwrap(), 0x9698_FD56);
	assert_eq!(dd.try_get_declare_dword_value(3).unwrap(), 0x828D_06C3);
}

#[test]
fn try_declare_dword_can_get_set_rev() {
	let mut dd = Instruction::try_with_declare_dword_4(0x77A9_CE9D, 0x5505_426C, 0x8632_FE4F, 0x3427_AA08).unwrap();
	dd.try_set_declare_dword_value(3, 0x828D_06C3).unwrap();
	dd.try_set_declare_dword_value(2, 0x9698_FD56).unwrap();
	dd.try_set_declare_dword_value(1, 0xCBE3_4DE4).unwrap();
	dd.try_set_declare_dword_value(0, 0xE2C5_FAB4).unwrap();
	assert_eq!(dd.try_get_declare_dword_value(0).unwrap(), 0xE2C5_FAB4);
	assert_eq!(dd.try_get_declare_dword_value(1).unwrap(), 0xCBE3_4DE4);
	assert_eq!(dd.try_get_declare_dword_value(2).unwrap(), 0x9698_FD56);
	assert_eq!(dd.try_get_declare_dword_value(3).unwrap(), 0x828D_06C3);
}

#[test]
fn declare_qword_can_get_set() {
	let mut dq = Instruction::with_declare_qword_2(0x77A9_CE9D_5505_426C, 0x8632_FE4F_3427_AA08);
	dq.set_declare_qword_value(0, 0xE2C5_FAB4_CBE3_4DE4);
	dq.set_declare_qword_value(1, 0x9698_FD56_828D_06C3);
	assert_eq!(dq.get_declare_qword_value(0), 0xE2C5_FAB4_CBE3_4DE4);
	assert_eq!(dq.get_declare_qword_value(1), 0x9698_FD56_828D_06C3);
}

#[test]
fn declare_qword_can_get_set_rev() {
	let mut dq = Instruction::with_declare_qword_2(0x77A9_CE9D_5505_426C, 0x8632_FE4F_3427_AA08);
	dq.set_declare_qword_value(1, 0x9698_FD56_828D_06C3);
	dq.set_declare_qword_value(0, 0xE2C5_FAB4_CBE3_4DE4);
	assert_eq!(dq.get_declare_qword_value(0), 0xE2C5_FAB4_CBE3_4DE4);
	assert_eq!(dq.get_declare_qword_value(1), 0x9698_FD56_828D_06C3);
}

#[test]
fn try_declare_qword_can_get_set() {
	let mut dq = Instruction::try_with_declare_qword_2(0x77A9_CE9D_5505_426C, 0x8632_FE4F_3427_AA08).unwrap();
	dq.try_set_declare_qword_value(0, 0xE2C5_FAB4_CBE3_4DE4).unwrap();
	dq.try_set_declare_qword_value(1, 0x9698_FD56_828D_06C3).unwrap();
	assert_eq!(dq.try_get_declare_qword_value(0).unwrap(), 0xE2C5_FAB4_CBE3_4DE4);
	assert_eq!(dq.try_get_declare_qword_value(1).unwrap(), 0x9698_FD56_828D_06C3);
}

#[test]
fn try_declare_qword_can_get_set_rev() {
	let mut dq = Instruction::try_with_declare_qword_2(0x77A9_CE9D_5505_426C, 0x8632_FE4F_3427_AA08).unwrap();
	dq.try_set_declare_qword_value(1, 0x9698_FD56_828D_06C3).unwrap();
	dq.try_set_declare_qword_value(0, 0xE2C5_FAB4_CBE3_4DE4).unwrap();
	assert_eq!(dq.try_get_declare_qword_value(0).unwrap(), 0xE2C5_FAB4_CBE3_4DE4);
	assert_eq!(dq.try_get_declare_qword_value(1).unwrap(), 0x9698_FD56_828D_06C3);
}

#[test]
fn declare_data_does_not_use_other_properties() {
	let data = [0xFFu8; 16];

	let instr = Instruction::with_declare_byte(&data[..]).unwrap();
	verify(&instr);

	let instr = Instruction::with_declare_word_slice_u8(&data[..]).unwrap();
	verify(&instr);

	let instr = Instruction::with_declare_dword_slice_u8(&data[..]).unwrap();
	verify(&instr);

	let instr = Instruction::with_declare_qword_slice_u8(&data[..]).unwrap();
	verify(&instr);

	fn verify(&instr: &Instruction) {
		assert_eq!(instr.segment_prefix(), Register::None);
		assert_eq!(instr.code_size(), CodeSize::Unknown);
		assert_eq!(instr.rounding_control(), RoundingControl::None);
		assert_eq!(instr.ip(), 0);
		assert!(!instr.is_broadcast());
		assert!(!instr.has_op_mask());
		assert!(!instr.suppress_all_exceptions());
		assert!(!instr.zeroing_masking());
		assert!(!instr.has_xacquire_prefix());
		assert!(!instr.has_xrelease_prefix());
		assert!(!instr.has_rep_prefix());
		assert!(!instr.has_repe_prefix());
		assert!(!instr.has_repne_prefix());
		assert!(!instr.has_lock_prefix());
	}
}

#[test]
fn with_declare_byte() {
	let instr = Instruction::with_declare_byte_1(0x77);
	assert_eq!(instr.code(), Code::DeclareByte);
	assert_eq!(instr.declare_data_len(), 1);
	assert_eq!(get_data(&instr), vec![0x77]);

	let instr = Instruction::with_declare_byte_2(0x77, 0xA9);
	assert_eq!(instr.code(), Code::DeclareByte);
	assert_eq!(instr.declare_data_len(), 2);
	assert_eq!(get_data(&instr), vec![0x77, 0xA9]);

	let instr = Instruction::with_declare_byte_3(0x77, 0xA9, 0xCE);
	assert_eq!(instr.code(), Code::DeclareByte);
	assert_eq!(instr.declare_data_len(), 3);
	assert_eq!(get_data(&instr), vec![0x77, 0xA9, 0xCE]);

	let instr = Instruction::with_declare_byte_4(0x77, 0xA9, 0xCE, 0x9D);
	assert_eq!(instr.code(), Code::DeclareByte);
	assert_eq!(instr.declare_data_len(), 4);
	assert_eq!(get_data(&instr), vec![0x77, 0xA9, 0xCE, 0x9D]);

	let instr = Instruction::with_declare_byte_5(0x77, 0xA9, 0xCE, 0x9D, 0x55);
	assert_eq!(instr.code(), Code::DeclareByte);
	assert_eq!(instr.declare_data_len(), 5);
	assert_eq!(get_data(&instr), vec![0x77, 0xA9, 0xCE, 0x9D, 0x55]);

	let instr = Instruction::with_declare_byte_6(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05);
	assert_eq!(instr.code(), Code::DeclareByte);
	assert_eq!(instr.declare_data_len(), 6);
	assert_eq!(get_data(&instr), vec![0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05]);

	let instr = Instruction::with_declare_byte_7(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42);
	assert_eq!(instr.code(), Code::DeclareByte);
	assert_eq!(instr.declare_data_len(), 7);
	assert_eq!(get_data(&instr), vec![0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42]);

	let instr = Instruction::with_declare_byte_8(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C);
	assert_eq!(instr.code(), Code::DeclareByte);
	assert_eq!(instr.declare_data_len(), 8);
	assert_eq!(get_data(&instr), vec![0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C]);

	let instr = Instruction::with_declare_byte_9(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86);
	assert_eq!(instr.code(), Code::DeclareByte);
	assert_eq!(instr.declare_data_len(), 9);
	assert_eq!(get_data(&instr), vec![0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86]);

	let instr = Instruction::with_declare_byte_10(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32);
	assert_eq!(instr.code(), Code::DeclareByte);
	assert_eq!(instr.declare_data_len(), 10);
	assert_eq!(get_data(&instr), vec![0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32]);

	let instr = Instruction::with_declare_byte_11(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE);
	assert_eq!(instr.code(), Code::DeclareByte);
	assert_eq!(instr.declare_data_len(), 11);
	assert_eq!(get_data(&instr), vec![0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE]);

	let instr = Instruction::with_declare_byte_12(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F);
	assert_eq!(instr.code(), Code::DeclareByte);
	assert_eq!(instr.declare_data_len(), 12);
	assert_eq!(get_data(&instr), vec![0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F]);

	let instr = Instruction::with_declare_byte_13(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34);
	assert_eq!(instr.code(), Code::DeclareByte);
	assert_eq!(instr.declare_data_len(), 13);
	assert_eq!(get_data(&instr), vec![0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34]);

	let instr = Instruction::with_declare_byte_14(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27);
	assert_eq!(instr.code(), Code::DeclareByte);
	assert_eq!(instr.declare_data_len(), 14);
	assert_eq!(get_data(&instr), vec![0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27]);

	let instr = Instruction::with_declare_byte_15(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA);
	assert_eq!(instr.code(), Code::DeclareByte);
	assert_eq!(instr.declare_data_len(), 15);
	assert_eq!(get_data(&instr), vec![0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA]);

	let instr = Instruction::with_declare_byte_16(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08);
	assert_eq!(instr.code(), Code::DeclareByte);
	assert_eq!(instr.declare_data_len(), 16);
	assert_eq!(get_data(&instr), vec![0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08]);
}

#[test]
fn try_with_declare_byte() {
	let instr = Instruction::try_with_declare_byte_1(0x77).unwrap();
	assert_eq!(instr.code(), Code::DeclareByte);
	assert_eq!(instr.declare_data_len(), 1);
	assert_eq!(get_data(&instr), vec![0x77]);

	let instr = Instruction::try_with_declare_byte_2(0x77, 0xA9).unwrap();
	assert_eq!(instr.code(), Code::DeclareByte);
	assert_eq!(instr.declare_data_len(), 2);
	assert_eq!(get_data(&instr), vec![0x77, 0xA9]);

	let instr = Instruction::try_with_declare_byte_3(0x77, 0xA9, 0xCE).unwrap();
	assert_eq!(instr.code(), Code::DeclareByte);
	assert_eq!(instr.declare_data_len(), 3);
	assert_eq!(get_data(&instr), vec![0x77, 0xA9, 0xCE]);

	let instr = Instruction::try_with_declare_byte_4(0x77, 0xA9, 0xCE, 0x9D).unwrap();
	assert_eq!(instr.code(), Code::DeclareByte);
	assert_eq!(instr.declare_data_len(), 4);
	assert_eq!(get_data(&instr), vec![0x77, 0xA9, 0xCE, 0x9D]);

	let instr = Instruction::try_with_declare_byte_5(0x77, 0xA9, 0xCE, 0x9D, 0x55).unwrap();
	assert_eq!(instr.code(), Code::DeclareByte);
	assert_eq!(instr.declare_data_len(), 5);
	assert_eq!(get_data(&instr), vec![0x77, 0xA9, 0xCE, 0x9D, 0x55]);

	let instr = Instruction::try_with_declare_byte_6(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05).unwrap();
	assert_eq!(instr.code(), Code::DeclareByte);
	assert_eq!(instr.declare_data_len(), 6);
	assert_eq!(get_data(&instr), vec![0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05]);

	let instr = Instruction::try_with_declare_byte_7(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42).unwrap();
	assert_eq!(instr.code(), Code::DeclareByte);
	assert_eq!(instr.declare_data_len(), 7);
	assert_eq!(get_data(&instr), vec![0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42]);

	let instr = Instruction::try_with_declare_byte_8(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C).unwrap();
	assert_eq!(instr.code(), Code::DeclareByte);
	assert_eq!(instr.declare_data_len(), 8);
	assert_eq!(get_data(&instr), vec![0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C]);

	let instr = Instruction::try_with_declare_byte_9(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86).unwrap();
	assert_eq!(instr.code(), Code::DeclareByte);
	assert_eq!(instr.declare_data_len(), 9);
	assert_eq!(get_data(&instr), vec![0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86]);

	let instr = Instruction::try_with_declare_byte_10(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32).unwrap();
	assert_eq!(instr.code(), Code::DeclareByte);
	assert_eq!(instr.declare_data_len(), 10);
	assert_eq!(get_data(&instr), vec![0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32]);

	let instr = Instruction::try_with_declare_byte_11(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE).unwrap();
	assert_eq!(instr.code(), Code::DeclareByte);
	assert_eq!(instr.declare_data_len(), 11);
	assert_eq!(get_data(&instr), vec![0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE]);

	let instr = Instruction::try_with_declare_byte_12(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F).unwrap();
	assert_eq!(instr.code(), Code::DeclareByte);
	assert_eq!(instr.declare_data_len(), 12);
	assert_eq!(get_data(&instr), vec![0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F]);

	let instr = Instruction::try_with_declare_byte_13(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34).unwrap();
	assert_eq!(instr.code(), Code::DeclareByte);
	assert_eq!(instr.declare_data_len(), 13);
	assert_eq!(get_data(&instr), vec![0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34]);

	let instr = Instruction::try_with_declare_byte_14(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27).unwrap();
	assert_eq!(instr.code(), Code::DeclareByte);
	assert_eq!(instr.declare_data_len(), 14);
	assert_eq!(get_data(&instr), vec![0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27]);

	let instr =
		Instruction::try_with_declare_byte_15(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA).unwrap();
	assert_eq!(instr.code(), Code::DeclareByte);
	assert_eq!(instr.declare_data_len(), 15);
	assert_eq!(get_data(&instr), vec![0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA]);

	let instr = Instruction::try_with_declare_byte_16(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08)
		.unwrap();
	assert_eq!(instr.code(), Code::DeclareByte);
	assert_eq!(instr.declare_data_len(), 16);
	assert_eq!(get_data(&instr), vec![0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08]);
}

#[test]
fn with_declare_word() {
	let instr = Instruction::with_declare_word_1(0x77A9);
	assert_eq!(instr.code(), Code::DeclareWord);
	assert_eq!(instr.declare_data_len(), 1);
	assert_eq!(get_data(&instr), vec![0xA9, 0x77]);

	let instr = Instruction::with_declare_word_2(0x77A9, 0xCE9D);
	assert_eq!(instr.code(), Code::DeclareWord);
	assert_eq!(instr.declare_data_len(), 2);
	assert_eq!(get_data(&instr), vec![0xA9, 0x77, 0x9D, 0xCE]);

	let instr = Instruction::with_declare_word_3(0x77A9, 0xCE9D, 0x5505);
	assert_eq!(instr.code(), Code::DeclareWord);
	assert_eq!(instr.declare_data_len(), 3);
	assert_eq!(get_data(&instr), vec![0xA9, 0x77, 0x9D, 0xCE, 0x05, 0x55]);

	let instr = Instruction::with_declare_word_4(0x77A9, 0xCE9D, 0x5505, 0x426C);
	assert_eq!(instr.code(), Code::DeclareWord);
	assert_eq!(instr.declare_data_len(), 4);
	assert_eq!(get_data(&instr), vec![0xA9, 0x77, 0x9D, 0xCE, 0x05, 0x55, 0x6C, 0x42]);

	let instr = Instruction::with_declare_word_5(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632);
	assert_eq!(instr.code(), Code::DeclareWord);
	assert_eq!(instr.declare_data_len(), 5);
	assert_eq!(get_data(&instr), vec![0xA9, 0x77, 0x9D, 0xCE, 0x05, 0x55, 0x6C, 0x42, 0x32, 0x86]);

	let instr = Instruction::with_declare_word_6(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F);
	assert_eq!(instr.code(), Code::DeclareWord);
	assert_eq!(instr.declare_data_len(), 6);
	assert_eq!(get_data(&instr), vec![0xA9, 0x77, 0x9D, 0xCE, 0x05, 0x55, 0x6C, 0x42, 0x32, 0x86, 0x4F, 0xFE]);

	let instr = Instruction::with_declare_word_7(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427);
	assert_eq!(instr.code(), Code::DeclareWord);
	assert_eq!(instr.declare_data_len(), 7);
	assert_eq!(get_data(&instr), vec![0xA9, 0x77, 0x9D, 0xCE, 0x05, 0x55, 0x6C, 0x42, 0x32, 0x86, 0x4F, 0xFE, 0x27, 0x34]);

	let instr = Instruction::with_declare_word_8(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08);
	assert_eq!(instr.code(), Code::DeclareWord);
	assert_eq!(instr.declare_data_len(), 8);
	assert_eq!(get_data(&instr), vec![0xA9, 0x77, 0x9D, 0xCE, 0x05, 0x55, 0x6C, 0x42, 0x32, 0x86, 0x4F, 0xFE, 0x27, 0x34, 0x08, 0xAA]);
}

#[test]
fn try_with_declare_word() {
	let instr = Instruction::try_with_declare_word_1(0x77A9).unwrap();
	assert_eq!(instr.code(), Code::DeclareWord);
	assert_eq!(instr.declare_data_len(), 1);
	assert_eq!(get_data(&instr), vec![0xA9, 0x77]);

	let instr = Instruction::try_with_declare_word_2(0x77A9, 0xCE9D).unwrap();
	assert_eq!(instr.code(), Code::DeclareWord);
	assert_eq!(instr.declare_data_len(), 2);
	assert_eq!(get_data(&instr), vec![0xA9, 0x77, 0x9D, 0xCE]);

	let instr = Instruction::try_with_declare_word_3(0x77A9, 0xCE9D, 0x5505).unwrap();
	assert_eq!(instr.code(), Code::DeclareWord);
	assert_eq!(instr.declare_data_len(), 3);
	assert_eq!(get_data(&instr), vec![0xA9, 0x77, 0x9D, 0xCE, 0x05, 0x55]);

	let instr = Instruction::try_with_declare_word_4(0x77A9, 0xCE9D, 0x5505, 0x426C).unwrap();
	assert_eq!(instr.code(), Code::DeclareWord);
	assert_eq!(instr.declare_data_len(), 4);
	assert_eq!(get_data(&instr), vec![0xA9, 0x77, 0x9D, 0xCE, 0x05, 0x55, 0x6C, 0x42]);

	let instr = Instruction::try_with_declare_word_5(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632).unwrap();
	assert_eq!(instr.code(), Code::DeclareWord);
	assert_eq!(instr.declare_data_len(), 5);
	assert_eq!(get_data(&instr), vec![0xA9, 0x77, 0x9D, 0xCE, 0x05, 0x55, 0x6C, 0x42, 0x32, 0x86]);

	let instr = Instruction::try_with_declare_word_6(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F).unwrap();
	assert_eq!(instr.code(), Code::DeclareWord);
	assert_eq!(instr.declare_data_len(), 6);
	assert_eq!(get_data(&instr), vec![0xA9, 0x77, 0x9D, 0xCE, 0x05, 0x55, 0x6C, 0x42, 0x32, 0x86, 0x4F, 0xFE]);

	let instr = Instruction::try_with_declare_word_7(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427).unwrap();
	assert_eq!(instr.code(), Code::DeclareWord);
	assert_eq!(instr.declare_data_len(), 7);
	assert_eq!(get_data(&instr), vec![0xA9, 0x77, 0x9D, 0xCE, 0x05, 0x55, 0x6C, 0x42, 0x32, 0x86, 0x4F, 0xFE, 0x27, 0x34]);

	let instr = Instruction::try_with_declare_word_8(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08).unwrap();
	assert_eq!(instr.code(), Code::DeclareWord);
	assert_eq!(instr.declare_data_len(), 8);
	assert_eq!(get_data(&instr), vec![0xA9, 0x77, 0x9D, 0xCE, 0x05, 0x55, 0x6C, 0x42, 0x32, 0x86, 0x4F, 0xFE, 0x27, 0x34, 0x08, 0xAA]);
}

#[test]
fn with_declare_dword() {
	let instr = Instruction::with_declare_dword_1(0x77A9_CE9D);
	assert_eq!(instr.code(), Code::DeclareDword);
	assert_eq!(instr.declare_data_len(), 1);
	assert_eq!(get_data(&instr), vec![0x9D, 0xCE, 0xA9, 0x77]);

	let instr = Instruction::with_declare_dword_2(0x77A9_CE9D, 0x5505_426C);
	assert_eq!(instr.code(), Code::DeclareDword);
	assert_eq!(instr.declare_data_len(), 2);
	assert_eq!(get_data(&instr), vec![0x9D, 0xCE, 0xA9, 0x77, 0x6C, 0x42, 0x05, 0x55]);

	let instr = Instruction::with_declare_dword_3(0x77A9_CE9D, 0x5505_426C, 0x8632_FE4F);
	assert_eq!(instr.code(), Code::DeclareDword);
	assert_eq!(instr.declare_data_len(), 3);
	assert_eq!(get_data(&instr), vec![0x9D, 0xCE, 0xA9, 0x77, 0x6C, 0x42, 0x05, 0x55, 0x4F, 0xFE, 0x32, 0x86]);

	let instr = Instruction::with_declare_dword_4(0x77A9_CE9D, 0x5505_426C, 0x8632_FE4F, 0x3427_AA08);
	assert_eq!(instr.code(), Code::DeclareDword);
	assert_eq!(instr.declare_data_len(), 4);
	assert_eq!(get_data(&instr), vec![0x9D, 0xCE, 0xA9, 0x77, 0x6C, 0x42, 0x05, 0x55, 0x4F, 0xFE, 0x32, 0x86, 0x08, 0xAA, 0x27, 0x34]);
}

#[test]
fn try_with_declare_dword() {
	let instr = Instruction::try_with_declare_dword_1(0x77A9_CE9D).unwrap();
	assert_eq!(instr.code(), Code::DeclareDword);
	assert_eq!(instr.declare_data_len(), 1);
	assert_eq!(get_data(&instr), vec![0x9D, 0xCE, 0xA9, 0x77]);

	let instr = Instruction::try_with_declare_dword_2(0x77A9_CE9D, 0x5505_426C).unwrap();
	assert_eq!(instr.code(), Code::DeclareDword);
	assert_eq!(instr.declare_data_len(), 2);
	assert_eq!(get_data(&instr), vec![0x9D, 0xCE, 0xA9, 0x77, 0x6C, 0x42, 0x05, 0x55]);

	let instr = Instruction::try_with_declare_dword_3(0x77A9_CE9D, 0x5505_426C, 0x8632_FE4F).unwrap();
	assert_eq!(instr.code(), Code::DeclareDword);
	assert_eq!(instr.declare_data_len(), 3);
	assert_eq!(get_data(&instr), vec![0x9D, 0xCE, 0xA9, 0x77, 0x6C, 0x42, 0x05, 0x55, 0x4F, 0xFE, 0x32, 0x86]);

	let instr = Instruction::try_with_declare_dword_4(0x77A9_CE9D, 0x5505_426C, 0x8632_FE4F, 0x3427_AA08).unwrap();
	assert_eq!(instr.code(), Code::DeclareDword);
	assert_eq!(instr.declare_data_len(), 4);
	assert_eq!(get_data(&instr), vec![0x9D, 0xCE, 0xA9, 0x77, 0x6C, 0x42, 0x05, 0x55, 0x4F, 0xFE, 0x32, 0x86, 0x08, 0xAA, 0x27, 0x34]);
}

#[test]
fn with_declare_qword() {
	let instr = Instruction::with_declare_qword_1(0x77A9_CE9D_5505_426C);
	assert_eq!(instr.code(), Code::DeclareQword);
	assert_eq!(instr.declare_data_len(), 1);
	assert_eq!(get_data(&instr), vec![0x6C, 0x42, 0x05, 0x55, 0x9D, 0xCE, 0xA9, 0x77]);

	let instr = Instruction::with_declare_qword_2(0x77A9_CE9D_5505_426C, 0x8632_FE4F_3427_AA08);
	assert_eq!(instr.code(), Code::DeclareQword);
	assert_eq!(instr.declare_data_len(), 2);
	assert_eq!(get_data(&instr), vec![0x6C, 0x42, 0x05, 0x55, 0x9D, 0xCE, 0xA9, 0x77, 0x08, 0xAA, 0x27, 0x34, 0x4F, 0xFE, 0x32, 0x86]);
}

#[test]
fn try_with_declare_qword() {
	let instr = Instruction::try_with_declare_qword_1(0x77A9_CE9D_5505_426C).unwrap();
	assert_eq!(instr.code(), Code::DeclareQword);
	assert_eq!(instr.declare_data_len(), 1);
	assert_eq!(get_data(&instr), vec![0x6C, 0x42, 0x05, 0x55, 0x9D, 0xCE, 0xA9, 0x77]);

	let instr = Instruction::try_with_declare_qword_2(0x77A9_CE9D_5505_426C, 0x8632_FE4F_3427_AA08).unwrap();
	assert_eq!(instr.code(), Code::DeclareQword);
	assert_eq!(instr.declare_data_len(), 2);
	assert_eq!(get_data(&instr), vec![0x6C, 0x42, 0x05, 0x55, 0x9D, 0xCE, 0xA9, 0x77, 0x08, 0xAA, 0x27, 0x34, 0x4F, 0xFE, 0x32, 0x86]);
}

#[test]
fn with_declare_byte_slice() {
	#[rustfmt::skip]
	let tests = vec![
		(Instruction::with_declare_byte_1(0x77), vec![0x77]),
		(Instruction::with_declare_byte_2(0x77, 0xA9), vec![0x77, 0xA9]),
		(Instruction::with_declare_byte_3(0x77, 0xA9, 0xCE), vec![0x77, 0xA9, 0xCE]),
		(Instruction::with_declare_byte_4(0x77, 0xA9, 0xCE, 0x9D), vec![0x77, 0xA9, 0xCE, 0x9D]),
		(Instruction::with_declare_byte_5(0x77, 0xA9, 0xCE, 0x9D, 0x55), vec![0x77, 0xA9, 0xCE, 0x9D, 0x55]),
		(Instruction::with_declare_byte_6(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05), vec![0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05]),
		(Instruction::with_declare_byte_7(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42), vec![0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42]),
		(Instruction::with_declare_byte_8(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C), vec![0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C]),
		(Instruction::with_declare_byte_9(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86), vec![0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86]),
		(Instruction::with_declare_byte_10(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32), vec![0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32]),
		(Instruction::with_declare_byte_11(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE), vec![0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE]),
		(Instruction::with_declare_byte_12(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F), vec![0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F]),
		(Instruction::with_declare_byte_13(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34), vec![0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34]),
		(Instruction::with_declare_byte_14(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27), vec![0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27]),
		(Instruction::with_declare_byte_15(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA), vec![0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA]),
		(Instruction::with_declare_byte_16(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08), vec![0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08]),
	];
	for (instr1, data) in tests {
		let instr2 = Instruction::with_declare_byte(&data).unwrap();
		assert!(instr1.eq_all_bits(&instr2));
	}
}

#[test]
fn with_declare_word_slice() {
	#[rustfmt::skip]
	let tests = vec![
		(Instruction::with_declare_word_1(0x77A9), vec![0xA9, 0x77]),
		(Instruction::with_declare_word_2(0x77A9, 0xCE9D), vec![0xA9, 0x77, 0x9D, 0xCE]),
		(Instruction::with_declare_word_3(0x77A9, 0xCE9D, 0x5505), vec![0xA9, 0x77, 0x9D, 0xCE, 0x05, 0x55]),
		(Instruction::with_declare_word_4(0x77A9, 0xCE9D, 0x5505, 0x426C), vec![0xA9, 0x77, 0x9D, 0xCE, 0x05, 0x55, 0x6C, 0x42]),
		(Instruction::with_declare_word_5(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632), vec![0xA9, 0x77, 0x9D, 0xCE, 0x05, 0x55, 0x6C, 0x42, 0x32, 0x86]),
		(Instruction::with_declare_word_6(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F), vec![0xA9, 0x77, 0x9D, 0xCE, 0x05, 0x55, 0x6C, 0x42, 0x32, 0x86, 0x4F, 0xFE]),
		(Instruction::with_declare_word_7(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427), vec![0xA9, 0x77, 0x9D, 0xCE, 0x05, 0x55, 0x6C, 0x42, 0x32, 0x86, 0x4F, 0xFE, 0x27, 0x34]),
		(Instruction::with_declare_word_8(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08), vec![0xA9, 0x77, 0x9D, 0xCE, 0x05, 0x55, 0x6C, 0x42, 0x32, 0x86, 0x4F, 0xFE, 0x27, 0x34, 0x08, 0xAA]),
	];
	for (instr1, data) in tests {
		let instr2 = Instruction::with_declare_word_slice_u8(&data).unwrap();
		assert!(instr1.eq_all_bits(&instr2));
	}
}

#[test]
fn with_declare_dword_slice() {
	#[rustfmt::skip]
	let tests = vec![
		(Instruction::with_declare_dword_1(0x77A9_CE9D), vec![0x9D, 0xCE, 0xA9, 0x77]),
		(Instruction::with_declare_dword_2(0x77A9_CE9D, 0x5505_426C), vec![0x9D, 0xCE, 0xA9, 0x77, 0x6C, 0x42, 0x05, 0x55]),
		(Instruction::with_declare_dword_3(0x77A9_CE9D, 0x5505_426C, 0x8632_FE4F), vec![0x9D, 0xCE, 0xA9, 0x77, 0x6C, 0x42, 0x05, 0x55, 0x4F, 0xFE, 0x32, 0x86]),
		(Instruction::with_declare_dword_4(0x77A9_CE9D, 0x5505_426C, 0x8632_FE4F, 0x3427_AA08), vec![0x9D, 0xCE, 0xA9, 0x77, 0x6C, 0x42, 0x05, 0x55, 0x4F, 0xFE, 0x32, 0x86, 0x08, 0xAA, 0x27, 0x34]),
	];
	for (instr1, data) in tests {
		let instr2 = Instruction::with_declare_dword_slice_u8(&data).unwrap();
		assert!(instr1.eq_all_bits(&instr2));
	}
}

#[test]
fn with_declare_qword_slice() {
	#[rustfmt::skip]
	let tests = vec![
		(Instruction::with_declare_qword_1(0x77A9_CE9D_5505_426C), vec![0x6C, 0x42, 0x05, 0x55, 0x9D, 0xCE, 0xA9, 0x77]),
		(Instruction::with_declare_qword_2(0x77A9_CE9D_5505_426C, 0x8632_FE4F_3427_AA08), vec![0x6C, 0x42, 0x05, 0x55, 0x9D, 0xCE, 0xA9, 0x77, 0x08, 0xAA, 0x27, 0x34, 0x4F, 0xFE, 0x32, 0x86]),
	];
	for (instr1, data) in tests {
		let instr2 = Instruction::with_declare_qword_slice_u8(&data).unwrap();
		assert!(instr1.eq_all_bits(&instr2));
	}
}

#[test]
fn with_declare_word_slice2() {
	#[rustfmt::skip]
	let tests = vec![
		(Instruction::with_declare_word_1(0x77A9), vec![0x77A9]),
		(Instruction::with_declare_word_2(0x77A9, 0xCE9D), vec![0x77A9, 0xCE9D]),
		(Instruction::with_declare_word_3(0x77A9, 0xCE9D, 0x5505), vec![0x77A9, 0xCE9D, 0x5505]),
		(Instruction::with_declare_word_4(0x77A9, 0xCE9D, 0x5505, 0x426C), vec![0x77A9, 0xCE9D, 0x5505, 0x426C]),
		(Instruction::with_declare_word_5(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632), vec![0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632]),
		(Instruction::with_declare_word_6(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F), vec![0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F]),
		(Instruction::with_declare_word_7(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427), vec![0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427]),
		(Instruction::with_declare_word_8(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08), vec![0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08]),
	];
	for (instr1, data) in tests {
		let instr2 = Instruction::with_declare_word(&data).unwrap();
		assert!(instr1.eq_all_bits(&instr2));
	}
}

#[test]
fn with_declare_dword_slice2() {
	#[rustfmt::skip]
	let tests = vec![
		(Instruction::with_declare_dword_1(0x77A9_CE9D), vec![0x77A9_CE9D]),
		(Instruction::with_declare_dword_2(0x77A9_CE9D, 0x5505_426C), vec![0x77A9_CE9D, 0x5505_426C]),
		(Instruction::with_declare_dword_3(0x77A9_CE9D, 0x5505_426C, 0x8632_FE4F), vec![0x77A9_CE9D, 0x5505_426C, 0x8632_FE4F]),
		(Instruction::with_declare_dword_4(0x77A9_CE9D, 0x5505_426C, 0x8632_FE4F, 0x3427_AA08), vec![0x77A9_CE9D, 0x5505_426C, 0x8632_FE4F, 0x3427_AA08]),
	];
	for (instr1, data) in tests {
		let instr2 = Instruction::with_declare_dword(&data).unwrap();
		assert!(instr1.eq_all_bits(&instr2));
	}
}

#[test]
fn with_declare_qword_slice2() {
	#[rustfmt::skip]
	let tests = vec![
		(Instruction::with_declare_qword_1(0x77A9_CE9D_5505_426C), vec![0x77A9_CE9D_5505_426C]),
		(Instruction::with_declare_qword_2(0x77A9_CE9D_5505_426C, 0x8632_FE4F_3427_AA08), vec![0x77A9_CE9D_5505_426C, 0x8632_FE4F_3427_AA08]),
	];
	for (instr1, data) in tests {
		let instr2 = Instruction::with_declare_qword(&data).unwrap();
		assert!(instr1.eq_all_bits(&instr2));
	}
}

#[test]
fn try_with_test() {
	#[rustfmt::skip]
	let tests: Vec<(u32, &str, u32, Instruction)> = vec![
		(64, "90", DecoderOptions::NONE, Instruction::with(Code::Nopd)),
		(64, "48B9FFFFFFFFFFFFFFFF", DecoderOptions::NONE, Instruction::with2(Code::Mov_r64_imm64, Register::RCX, -1).unwrap()),
		(64, "48B9FFFFFFFFFFFFFFFF", DecoderOptions::NONE, Instruction::with2(Code::Mov_r64_imm64, Register::RCX, -1).unwrap()),
		(64, "48B9123456789ABCDE31", DecoderOptions::NONE, Instruction::with2(Code::Mov_r64_imm64, Register::RCX, 0x31DE_BC9A_7856_3412u64).unwrap()),
		(64, "48B9FFFFFFFF00000000", DecoderOptions::NONE, Instruction::with2(Code::Mov_r64_imm64, Register::RCX, 0xFFFF_FFFFu32).unwrap()),
		(64, "8FC1", DecoderOptions::NONE, Instruction::with1(Code::Pop_rm64, Register::RCX).unwrap()),
		(64, "648F847501EFCDAB", DecoderOptions::NONE, Instruction::with1(Code::Pop_rm64, MemoryOperand::new(Register::RBP, Register::RSI, 2, -0x5432_10FF, 8, false, Register::FS)).unwrap()),
		(64, "C6F85A", DecoderOptions::NONE, Instruction::with1(Code::Xabort_imm8, 0x5A).unwrap()),
		(64, "66685AA5", DecoderOptions::NONE, Instruction::with1(Code::Push_imm16, 0xA55A).unwrap()),
		(32, "685AA51234", DecoderOptions::NONE, Instruction::with1(Code::Pushd_imm32, 0x3412_A55A).unwrap()),
		(64, "666A5A", DecoderOptions::NONE, Instruction::with1(Code::Pushw_imm8, 0x5A).unwrap()),
		(32, "6A5A", DecoderOptions::NONE, Instruction::with1(Code::Pushd_imm8, 0x5A).unwrap()),
		(64, "6A5A", DecoderOptions::NONE, Instruction::with1(Code::Pushq_imm8, 0x5A).unwrap()),
		(64, "685AA512A4", DecoderOptions::NONE, Instruction::with1(Code::Pushq_imm32, -0x5BED_5AA6).unwrap()),
		(32, "66705A", DecoderOptions::NONE, Instruction::with_branch(Code::Jo_rel8_16, 0x4D).unwrap()),
		(32, "705A", DecoderOptions::NONE, Instruction::with_branch(Code::Jo_rel8_32, 0x8000_004C).unwrap()),
		(64, "705A", DecoderOptions::NONE, Instruction::with_branch(Code::Jo_rel8_64, 0x8000_0000_0000_004C).unwrap()),
		(32, "669A12345678", DecoderOptions::NONE, Instruction::with_far_branch(Code::Call_ptr1616, 0x7856, 0x3412).unwrap()),
		(32, "9A123456789ABC", DecoderOptions::NONE, Instruction::with_far_branch(Code::Call_ptr1632, 0xBC9A, 0x7856_3412).unwrap()),
		(16, "C7F85AA5", DecoderOptions::NONE, Instruction::with_xbegin(16, 0x254E).unwrap()),
		(32, "C7F85AA51234", DecoderOptions::NONE, Instruction::with_xbegin(32, 0xB412_A550).unwrap()),
		(64, "C7F85AA51234", DecoderOptions::NONE, Instruction::with_xbegin(64, 0x8000_0000_3412_A550).unwrap()),
		(64, "00D1", DecoderOptions::NONE, Instruction::with2(Code::Add_rm8_r8, Register::CL, Register::DL).unwrap()),
		(64, "64028C7501EFCDAB", DecoderOptions::NONE, Instruction::with2(Code::Add_r8_rm8, Register::CL, MemoryOperand::new(Register::RBP, Register::RSI, 2, -0x5432_10FF, 8, false, Register::FS)).unwrap()),
		(64, "80C15A", DecoderOptions::NONE, Instruction::with2(Code::Add_rm8_imm8, Register::CL, 0x5A).unwrap()),
		(64, "6681C15AA5", DecoderOptions::NONE, Instruction::with2(Code::Add_rm16_imm16, Register::CX, 0xA55A).unwrap()),
		(64, "81C15AA51234", DecoderOptions::NONE, Instruction::with2(Code::Add_rm32_imm32, Register::ECX, 0x3412_A55A).unwrap()),
		(64, "48B904152637A55A5678", DecoderOptions::NONE, Instruction::with2(Code::Mov_r64_imm64, Register::RCX, 0x7856_5AA5_3726_1504u64).unwrap()),
		(64, "6683C15A", DecoderOptions::NONE, Instruction::with2(Code::Add_rm16_imm8, Register::CX, 0x5A).unwrap()),
		(64, "83C15A", DecoderOptions::NONE, Instruction::with2(Code::Add_rm32_imm8, Register::ECX, 0x5A).unwrap()),
		(64, "4883C15A", DecoderOptions::NONE, Instruction::with2(Code::Add_rm64_imm8, Register::RCX, 0x5A).unwrap()),
		(64, "4881C15AA51234", DecoderOptions::NONE, Instruction::with2(Code::Add_rm64_imm32, Register::RCX, 0x3412_A55A).unwrap()),
		(64, "64A0123456789ABCDEF0", DecoderOptions::NONE, Instruction::with2(Code::Mov_AL_moffs8, Register::AL, MemoryOperand::with_base_displ_size_bcst_seg(Register::None, 0xF0DE_BC9A_7856_3412u64 as i64, 8, false, Register::FS)).unwrap()),
		(64, "6400947501EFCDAB", DecoderOptions::NONE, Instruction::with2(Code::Add_rm8_r8, MemoryOperand::new(Register::RBP, Register::RSI, 2, -0x5432_10FF, 8, false, Register::FS), Register::DL).unwrap()),
		(64, "6480847501EFCDAB5A", DecoderOptions::NONE, Instruction::with2(Code::Add_rm8_imm8, MemoryOperand::new(Register::RBP, Register::RSI, 2, -0x5432_10FF, 8, false, Register::FS), 0x5A).unwrap()),
		(64, "646681847501EFCDAB5AA5", DecoderOptions::NONE, Instruction::with2(Code::Add_rm16_imm16, MemoryOperand::new(Register::RBP, Register::RSI, 2, -0x5432_10FF, 8, false, Register::FS), 0xA55A).unwrap()),
		(64, "6481847501EFCDAB5AA51234", DecoderOptions::NONE, Instruction::with2(Code::Add_rm32_imm32, MemoryOperand::new(Register::RBP, Register::RSI, 2, -0x5432_10FF, 8, false, Register::FS), 0x3412_A55A).unwrap()),
		(64, "646683847501EFCDAB5A", DecoderOptions::NONE, Instruction::with2(Code::Add_rm16_imm8, MemoryOperand::new(Register::RBP, Register::RSI, 2, -0x5432_10FF, 8, false, Register::FS), 0x5A).unwrap()),
		(64, "6483847501EFCDAB5A", DecoderOptions::NONE, Instruction::with2(Code::Add_rm32_imm8, MemoryOperand::new(Register::RBP, Register::RSI, 2, -0x5432_10FF, 8, false, Register::FS), 0x5A).unwrap()),
		(64, "644883847501EFCDAB5A", DecoderOptions::NONE, Instruction::with2(Code::Add_rm64_imm8, MemoryOperand::new(Register::RBP, Register::RSI, 2, -0x5432_10FF, 8, false, Register::FS), 0x5A).unwrap()),
		(64, "644881847501EFCDAB5AA51234", DecoderOptions::NONE, Instruction::with2(Code::Add_rm64_imm32, MemoryOperand::new(Register::RBP, Register::RSI, 2, -0x5432_10FF, 8, false, Register::FS), 0x3412_A55A).unwrap()),
		(64, "E65A", DecoderOptions::NONE, Instruction::with2(Code::Out_imm8_AL, 0x5A, Register::AL).unwrap()),
		(64, "E65A", DecoderOptions::NONE, Instruction::with2(Code::Out_imm8_AL, 0x5A, Register::AL).unwrap()),
		(64, "66C85AA5A6", DecoderOptions::NONE, Instruction::with2(Code::Enterw_imm16_imm8, 0xA55A, 0xA6).unwrap()),
		(64, "66C85AA5A6", DecoderOptions::NONE, Instruction::with2(Code::Enterw_imm16_imm8, 0xA55A, 0xA6).unwrap()),
		(64, "64A2123456789ABCDEF0", DecoderOptions::NONE, Instruction::with2(Code::Mov_moffs8_AL, MemoryOperand::with_base_displ_size_bcst_seg(Register::None, 0xF0DE_BC9A_7856_3412u64 as i64, 8, false, Register::FS), Register::AL).unwrap()),
		(64, "6669CAA55A", DecoderOptions::NONE, Instruction::with3(Code::Imul_r16_rm16_imm16, Register::CX, Register::DX, 0x5AA5).unwrap()),
		(64, "69CA5AA51234", DecoderOptions::NONE, Instruction::with3(Code::Imul_r32_rm32_imm32, Register::ECX, Register::EDX, 0x3412_A55A).unwrap()),
		(64, "666BCA5A", DecoderOptions::NONE, Instruction::with3(Code::Imul_r16_rm16_imm8, Register::CX, Register::DX, 0x5A).unwrap()),
		(64, "6BCA5A", DecoderOptions::NONE, Instruction::with3(Code::Imul_r32_rm32_imm8, Register::ECX, Register::EDX, 0x5A).unwrap()),
		(64, "486BCA5A", DecoderOptions::NONE, Instruction::with3(Code::Imul_r64_rm64_imm8, Register::RCX, Register::RDX, 0x5A).unwrap()),
		(64, "4869CA5AA512A4", DecoderOptions::NONE, Instruction::with3(Code::Imul_r64_rm64_imm32, Register::RCX, Register::RDX, -0x5BED_5AA6).unwrap()),
		(64, "6466698C7501EFCDAB5AA5", DecoderOptions::NONE, Instruction::with3(Code::Imul_r16_rm16_imm16, Register::CX, MemoryOperand::new(Register::RBP, Register::RSI, 2, -0x5432_10FF, 8, false, Register::FS), 0xA55A).unwrap()),
		(64, "64698C7501EFCDAB5AA51234", DecoderOptions::NONE, Instruction::with3(Code::Imul_r32_rm32_imm32, Register::ECX, MemoryOperand::new(Register::RBP, Register::RSI, 2, -0x5432_10FF, 8, false, Register::FS), 0x3412_A55A).unwrap()),
		(64, "64666B8C7501EFCDAB5A", DecoderOptions::NONE, Instruction::with3(Code::Imul_r16_rm16_imm8, Register::CX, MemoryOperand::new(Register::RBP, Register::RSI, 2, -0x5432_10FF, 8, false, Register::FS), 0x5A).unwrap()),
		(64, "646B8C7501EFCDAB5A", DecoderOptions::NONE, Instruction::with3(Code::Imul_r32_rm32_imm8, Register::ECX, MemoryOperand::new(Register::RBP, Register::RSI, 2, -0x5432_10FF, 8, false, Register::FS), 0x5A).unwrap()),
		(64, "64486B8C7501EFCDAB5A", DecoderOptions::NONE, Instruction::with3(Code::Imul_r64_rm64_imm8, Register::RCX, MemoryOperand::new(Register::RBP, Register::RSI, 2, -0x5432_10FF, 8, false, Register::FS), 0x5A).unwrap()),
		(64, "6448698C7501EFCDAB5AA512A4", DecoderOptions::NONE, Instruction::with3(Code::Imul_r64_rm64_imm32, Register::RCX, MemoryOperand::new(Register::RBP, Register::RSI, 2, -0x5432_10FF, 8, false, Register::FS), -0x5BED_5AA6).unwrap()),
		(64, "660F78C1A5FD", DecoderOptions::NONE, Instruction::with3(Code::Extrq_xmm_imm8_imm8, Register::XMM1, 0xA5, 0xFD).unwrap()),
		(64, "660F78C1A5FD", DecoderOptions::NONE, Instruction::with3(Code::Extrq_xmm_imm8_imm8, Register::XMM1, 0xA5, 0xFD).unwrap()),
		(64, "64660FA4947501EFCDAB5A", DecoderOptions::NONE, Instruction::with3(Code::Shld_rm16_r16_imm8, MemoryOperand::new(Register::RBP, Register::RSI, 2, -0x5432_10FF, 8, false, Register::FS), Register::DX, 0x5A).unwrap()),
		(64, "64660FA4947501EFCDAB5A", DecoderOptions::NONE, Instruction::with3(Code::Shld_rm16_r16_imm8, MemoryOperand::new(Register::RBP, Register::RSI, 2, -0x5432_10FF, 8, false, Register::FS), Register::DX, 0x5A).unwrap()),
		(64, "F20F78CAA5FD", DecoderOptions::NONE, Instruction::with4(Code::Insertq_xmm_xmm_imm8_imm8, Register::XMM1, Register::XMM2, 0xA5, 0xFD).unwrap()),
		(64, "F20F78CAA5FD", DecoderOptions::NONE, Instruction::with4(Code::Insertq_xmm_xmm_imm8_imm8, Register::XMM1, Register::XMM2, 0xA5, 0xFD).unwrap()),
		(16, "0FB855AA", DecoderOptions::JMPE, Instruction::with_branch(Code::Jmpe_disp16, 0xAA55).unwrap()),
		(32, "0FB8123455AA", DecoderOptions::JMPE, Instruction::with_branch(Code::Jmpe_disp32, 0xAA55_3412).unwrap()),
		(32, "64676E", DecoderOptions::NONE, Instruction::with_outsb(16, Register::FS, RepPrefixKind::None).unwrap()),
		(64, "64676E", DecoderOptions::NONE, Instruction::with_outsb(32, Register::FS, RepPrefixKind::None).unwrap()),
		(64, "646E", DecoderOptions::NONE, Instruction::with_outsb(64, Register::FS, RepPrefixKind::None).unwrap()),
		(32, "6466676F", DecoderOptions::NONE, Instruction::with_outsw(16, Register::FS, RepPrefixKind::None).unwrap()),
		(64, "6466676F", DecoderOptions::NONE, Instruction::with_outsw(32, Register::FS, RepPrefixKind::None).unwrap()),
		(64, "64666F", DecoderOptions::NONE, Instruction::with_outsw(64, Register::FS, RepPrefixKind::None).unwrap()),
		(32, "64676F", DecoderOptions::NONE, Instruction::with_outsd(16, Register::FS, RepPrefixKind::None).unwrap()),
		(64, "64676F", DecoderOptions::NONE, Instruction::with_outsd(32, Register::FS, RepPrefixKind::None).unwrap()),
		(64, "646F", DecoderOptions::NONE, Instruction::with_outsd(64, Register::FS, RepPrefixKind::None).unwrap()),
		(32, "67AE", DecoderOptions::NONE, Instruction::with_scasb(16, RepPrefixKind::None).unwrap()),
		(64, "67AE", DecoderOptions::NONE, Instruction::with_scasb(32, RepPrefixKind::None).unwrap()),
		(64, "AE", DecoderOptions::NONE, Instruction::with_scasb(64, RepPrefixKind::None).unwrap()),
		(32, "6667AF", DecoderOptions::NONE, Instruction::with_scasw(16, RepPrefixKind::None).unwrap()),
		(64, "6667AF", DecoderOptions::NONE, Instruction::with_scasw(32, RepPrefixKind::None).unwrap()),
		(64, "66AF", DecoderOptions::NONE, Instruction::with_scasw(64, RepPrefixKind::None).unwrap()),
		(32, "67AF", DecoderOptions::NONE, Instruction::with_scasd(16, RepPrefixKind::None).unwrap()),
		(64, "67AF", DecoderOptions::NONE, Instruction::with_scasd(32, RepPrefixKind::None).unwrap()),
		(64, "AF", DecoderOptions::NONE, Instruction::with_scasd(64, RepPrefixKind::None).unwrap()),
		(64, "6748AF", DecoderOptions::NONE, Instruction::with_scasq(32, RepPrefixKind::None).unwrap()),
		(64, "48AF", DecoderOptions::NONE, Instruction::with_scasq(64, RepPrefixKind::None).unwrap()),
		(32, "6467AC", DecoderOptions::NONE, Instruction::with_lodsb(16, Register::FS, RepPrefixKind::None).unwrap()),
		(64, "6467AC", DecoderOptions::NONE, Instruction::with_lodsb(32, Register::FS, RepPrefixKind::None).unwrap()),
		(64, "64AC", DecoderOptions::NONE, Instruction::with_lodsb(64, Register::FS, RepPrefixKind::None).unwrap()),
		(32, "646667AD", DecoderOptions::NONE, Instruction::with_lodsw(16, Register::FS, RepPrefixKind::None).unwrap()),
		(64, "646667AD", DecoderOptions::NONE, Instruction::with_lodsw(32, Register::FS, RepPrefixKind::None).unwrap()),
		(64, "6466AD", DecoderOptions::NONE, Instruction::with_lodsw(64, Register::FS, RepPrefixKind::None).unwrap()),
		(32, "6467AD", DecoderOptions::NONE, Instruction::with_lodsd(16, Register::FS, RepPrefixKind::None).unwrap()),
		(64, "6467AD", DecoderOptions::NONE, Instruction::with_lodsd(32, Register::FS, RepPrefixKind::None).unwrap()),
		(64, "64AD", DecoderOptions::NONE, Instruction::with_lodsd(64, Register::FS, RepPrefixKind::None).unwrap()),
		(64, "646748AD", DecoderOptions::NONE, Instruction::with_lodsq(32, Register::FS, RepPrefixKind::None).unwrap()),
		(64, "6448AD", DecoderOptions::NONE, Instruction::with_lodsq(64, Register::FS, RepPrefixKind::None).unwrap()),
		(32, "676C", DecoderOptions::NONE, Instruction::with_insb(16, RepPrefixKind::None).unwrap()),
		(64, "676C", DecoderOptions::NONE, Instruction::with_insb(32, RepPrefixKind::None).unwrap()),
		(64, "6C", DecoderOptions::NONE, Instruction::with_insb(64, RepPrefixKind::None).unwrap()),
		(32, "66676D", DecoderOptions::NONE, Instruction::with_insw(16, RepPrefixKind::None).unwrap()),
		(64, "66676D", DecoderOptions::NONE, Instruction::with_insw(32, RepPrefixKind::None).unwrap()),
		(64, "666D", DecoderOptions::NONE, Instruction::with_insw(64, RepPrefixKind::None).unwrap()),
		(32, "676D", DecoderOptions::NONE, Instruction::with_insd(16, RepPrefixKind::None).unwrap()),
		(64, "676D", DecoderOptions::NONE, Instruction::with_insd(32, RepPrefixKind::None).unwrap()),
		(64, "6D", DecoderOptions::NONE, Instruction::with_insd(64, RepPrefixKind::None).unwrap()),
		(32, "67AA", DecoderOptions::NONE, Instruction::with_stosb(16, RepPrefixKind::None).unwrap()),
		(64, "67AA", DecoderOptions::NONE, Instruction::with_stosb(32, RepPrefixKind::None).unwrap()),
		(64, "AA", DecoderOptions::NONE, Instruction::with_stosb(64, RepPrefixKind::None).unwrap()),
		(32, "6667AB", DecoderOptions::NONE, Instruction::with_stosw(16, RepPrefixKind::None).unwrap()),
		(64, "6667AB", DecoderOptions::NONE, Instruction::with_stosw(32, RepPrefixKind::None).unwrap()),
		(64, "66AB", DecoderOptions::NONE, Instruction::with_stosw(64, RepPrefixKind::None).unwrap()),
		(32, "67AB", DecoderOptions::NONE, Instruction::with_stosd(16, RepPrefixKind::None).unwrap()),
		(64, "67AB", DecoderOptions::NONE, Instruction::with_stosd(32, RepPrefixKind::None).unwrap()),
		(64, "AB", DecoderOptions::NONE, Instruction::with_stosd(64, RepPrefixKind::None).unwrap()),
		(64, "6748AB", DecoderOptions::NONE, Instruction::with_stosq(32, RepPrefixKind::None).unwrap()),
		(64, "48AB", DecoderOptions::NONE, Instruction::with_stosq(64, RepPrefixKind::None).unwrap()),
		(32, "6467A6", DecoderOptions::NONE, Instruction::with_cmpsb(16, Register::FS, RepPrefixKind::None).unwrap()),
		(64, "6467A6", DecoderOptions::NONE, Instruction::with_cmpsb(32, Register::FS, RepPrefixKind::None).unwrap()),
		(64, "64A6", DecoderOptions::NONE, Instruction::with_cmpsb(64, Register::FS, RepPrefixKind::None).unwrap()),
		(32, "646667A7", DecoderOptions::NONE, Instruction::with_cmpsw(16, Register::FS, RepPrefixKind::None).unwrap()),
		(64, "646667A7", DecoderOptions::NONE, Instruction::with_cmpsw(32, Register::FS, RepPrefixKind::None).unwrap()),
		(64, "6466A7", DecoderOptions::NONE, Instruction::with_cmpsw(64, Register::FS, RepPrefixKind::None).unwrap()),
		(32, "6467A7", DecoderOptions::NONE, Instruction::with_cmpsd(16, Register::FS, RepPrefixKind::None).unwrap()),
		(64, "6467A7", DecoderOptions::NONE, Instruction::with_cmpsd(32, Register::FS, RepPrefixKind::None).unwrap()),
		(64, "64A7", DecoderOptions::NONE, Instruction::with_cmpsd(64, Register::FS, RepPrefixKind::None).unwrap()),
		(64, "646748A7", DecoderOptions::NONE, Instruction::with_cmpsq(32, Register::FS, RepPrefixKind::None).unwrap()),
		(64, "6448A7", DecoderOptions::NONE, Instruction::with_cmpsq(64, Register::FS, RepPrefixKind::None).unwrap()),
		(32, "6467A4", DecoderOptions::NONE, Instruction::with_movsb(16, Register::FS, RepPrefixKind::None).unwrap()),
		(64, "6467A4", DecoderOptions::NONE, Instruction::with_movsb(32, Register::FS, RepPrefixKind::None).unwrap()),
		(64, "64A4", DecoderOptions::NONE, Instruction::with_movsb(64, Register::FS, RepPrefixKind::None).unwrap()),
		(32, "646667A5", DecoderOptions::NONE, Instruction::with_movsw(16, Register::FS, RepPrefixKind::None).unwrap()),
		(64, "646667A5", DecoderOptions::NONE, Instruction::with_movsw(32, Register::FS, RepPrefixKind::None).unwrap()),
		(64, "6466A5", DecoderOptions::NONE, Instruction::with_movsw(64, Register::FS, RepPrefixKind::None).unwrap()),
		(32, "6467A5", DecoderOptions::NONE, Instruction::with_movsd(16, Register::FS, RepPrefixKind::None).unwrap()),
		(64, "6467A5", DecoderOptions::NONE, Instruction::with_movsd(32, Register::FS, RepPrefixKind::None).unwrap()),
		(64, "64A5", DecoderOptions::NONE, Instruction::with_movsd(64, Register::FS, RepPrefixKind::None).unwrap()),
		(64, "646748A5", DecoderOptions::NONE, Instruction::with_movsq(32, Register::FS, RepPrefixKind::None).unwrap()),
		(64, "6448A5", DecoderOptions::NONE, Instruction::with_movsq(64, Register::FS, RepPrefixKind::None).unwrap()),
		(32, "64670FF7D3", DecoderOptions::NONE, Instruction::with_maskmovq(16, Register::MM2, Register::MM3, Register::FS).unwrap()),
		(64, "64670FF7D3", DecoderOptions::NONE, Instruction::with_maskmovq(32, Register::MM2, Register::MM3, Register::FS).unwrap()),
		(64, "640FF7D3", DecoderOptions::NONE, Instruction::with_maskmovq(64, Register::MM2, Register::MM3, Register::FS).unwrap()),
		(32, "6467660FF7D3", DecoderOptions::NONE, Instruction::with_maskmovdqu(16, Register::XMM2, Register::XMM3, Register::FS).unwrap()),
		(64, "6467660FF7D3", DecoderOptions::NONE, Instruction::with_maskmovdqu(32, Register::XMM2, Register::XMM3, Register::FS).unwrap()),
		(64, "64660FF7D3", DecoderOptions::NONE, Instruction::with_maskmovdqu(64, Register::XMM2, Register::XMM3, Register::FS).unwrap()),

		(32, "6467F36E", DecoderOptions::NONE, Instruction::with_outsb(16, Register::FS, RepPrefixKind::Repe).unwrap()),
		(64, "6467F36E", DecoderOptions::NONE, Instruction::with_outsb(32, Register::FS, RepPrefixKind::Repe).unwrap()),
		(64, "64F36E", DecoderOptions::NONE, Instruction::with_outsb(64, Register::FS, RepPrefixKind::Repe).unwrap()),
		(32, "646667F36F", DecoderOptions::NONE, Instruction::with_outsw(16, Register::FS, RepPrefixKind::Repe).unwrap()),
		(64, "646667F36F", DecoderOptions::NONE, Instruction::with_outsw(32, Register::FS, RepPrefixKind::Repe).unwrap()),
		(64, "6466F36F", DecoderOptions::NONE, Instruction::with_outsw(64, Register::FS, RepPrefixKind::Repe).unwrap()),
		(32, "6467F36F", DecoderOptions::NONE, Instruction::with_outsd(16, Register::FS, RepPrefixKind::Repe).unwrap()),
		(64, "6467F36F", DecoderOptions::NONE, Instruction::with_outsd(32, Register::FS, RepPrefixKind::Repe).unwrap()),
		(64, "64F36F", DecoderOptions::NONE, Instruction::with_outsd(64, Register::FS, RepPrefixKind::Repe).unwrap()),
		(32, "67F3AE", DecoderOptions::NONE, Instruction::with_scasb(16, RepPrefixKind::Repe).unwrap()),
		(64, "67F3AE", DecoderOptions::NONE, Instruction::with_scasb(32, RepPrefixKind::Repe).unwrap()),
		(64, "F3AE", DecoderOptions::NONE, Instruction::with_scasb(64, RepPrefixKind::Repe).unwrap()),
		(32, "6667F3AF", DecoderOptions::NONE, Instruction::with_scasw(16, RepPrefixKind::Repe).unwrap()),
		(64, "6667F3AF", DecoderOptions::NONE, Instruction::with_scasw(32, RepPrefixKind::Repe).unwrap()),
		(64, "66F3AF", DecoderOptions::NONE, Instruction::with_scasw(64, RepPrefixKind::Repe).unwrap()),
		(32, "67F3AF", DecoderOptions::NONE, Instruction::with_scasd(16, RepPrefixKind::Repe).unwrap()),
		(64, "67F3AF", DecoderOptions::NONE, Instruction::with_scasd(32, RepPrefixKind::Repe).unwrap()),
		(64, "F3AF", DecoderOptions::NONE, Instruction::with_scasd(64, RepPrefixKind::Repe).unwrap()),
		(64, "67F348AF", DecoderOptions::NONE, Instruction::with_scasq(32, RepPrefixKind::Repe).unwrap()),
		(64, "F348AF", DecoderOptions::NONE, Instruction::with_scasq(64, RepPrefixKind::Repe).unwrap()),
		(32, "6467F3AC", DecoderOptions::NONE, Instruction::with_lodsb(16, Register::FS, RepPrefixKind::Repe).unwrap()),
		(64, "6467F3AC", DecoderOptions::NONE, Instruction::with_lodsb(32, Register::FS, RepPrefixKind::Repe).unwrap()),
		(64, "64F3AC", DecoderOptions::NONE, Instruction::with_lodsb(64, Register::FS, RepPrefixKind::Repe).unwrap()),
		(32, "646667F3AD", DecoderOptions::NONE, Instruction::with_lodsw(16, Register::FS, RepPrefixKind::Repe).unwrap()),
		(64, "646667F3AD", DecoderOptions::NONE, Instruction::with_lodsw(32, Register::FS, RepPrefixKind::Repe).unwrap()),
		(64, "6466F3AD", DecoderOptions::NONE, Instruction::with_lodsw(64, Register::FS, RepPrefixKind::Repe).unwrap()),
		(32, "6467F3AD", DecoderOptions::NONE, Instruction::with_lodsd(16, Register::FS, RepPrefixKind::Repe).unwrap()),
		(64, "6467F3AD", DecoderOptions::NONE, Instruction::with_lodsd(32, Register::FS, RepPrefixKind::Repe).unwrap()),
		(64, "64F3AD", DecoderOptions::NONE, Instruction::with_lodsd(64, Register::FS, RepPrefixKind::Repe).unwrap()),
		(64, "6467F348AD", DecoderOptions::NONE, Instruction::with_lodsq(32, Register::FS, RepPrefixKind::Repe).unwrap()),
		(64, "64F348AD", DecoderOptions::NONE, Instruction::with_lodsq(64, Register::FS, RepPrefixKind::Repe).unwrap()),
		(32, "67F36C", DecoderOptions::NONE, Instruction::with_insb(16, RepPrefixKind::Repe).unwrap()),
		(64, "67F36C", DecoderOptions::NONE, Instruction::with_insb(32, RepPrefixKind::Repe).unwrap()),
		(64, "F36C", DecoderOptions::NONE, Instruction::with_insb(64, RepPrefixKind::Repe).unwrap()),
		(32, "6667F36D", DecoderOptions::NONE, Instruction::with_insw(16, RepPrefixKind::Repe).unwrap()),
		(64, "6667F36D", DecoderOptions::NONE, Instruction::with_insw(32, RepPrefixKind::Repe).unwrap()),
		(64, "66F36D", DecoderOptions::NONE, Instruction::with_insw(64, RepPrefixKind::Repe).unwrap()),
		(32, "67F36D", DecoderOptions::NONE, Instruction::with_insd(16, RepPrefixKind::Repe).unwrap()),
		(64, "67F36D", DecoderOptions::NONE, Instruction::with_insd(32, RepPrefixKind::Repe).unwrap()),
		(64, "F36D", DecoderOptions::NONE, Instruction::with_insd(64, RepPrefixKind::Repe).unwrap()),
		(32, "67F3AA", DecoderOptions::NONE, Instruction::with_stosb(16, RepPrefixKind::Repe).unwrap()),
		(64, "67F3AA", DecoderOptions::NONE, Instruction::with_stosb(32, RepPrefixKind::Repe).unwrap()),
		(64, "F3AA", DecoderOptions::NONE, Instruction::with_stosb(64, RepPrefixKind::Repe).unwrap()),
		(32, "6667F3AB", DecoderOptions::NONE, Instruction::with_stosw(16, RepPrefixKind::Repe).unwrap()),
		(64, "6667F3AB", DecoderOptions::NONE, Instruction::with_stosw(32, RepPrefixKind::Repe).unwrap()),
		(64, "66F3AB", DecoderOptions::NONE, Instruction::with_stosw(64, RepPrefixKind::Repe).unwrap()),
		(32, "67F3AB", DecoderOptions::NONE, Instruction::with_stosd(16, RepPrefixKind::Repe).unwrap()),
		(64, "67F3AB", DecoderOptions::NONE, Instruction::with_stosd(32, RepPrefixKind::Repe).unwrap()),
		(64, "F3AB", DecoderOptions::NONE, Instruction::with_stosd(64, RepPrefixKind::Repe).unwrap()),
		(64, "67F348AB", DecoderOptions::NONE, Instruction::with_stosq(32, RepPrefixKind::Repe).unwrap()),
		(64, "F348AB", DecoderOptions::NONE, Instruction::with_stosq(64, RepPrefixKind::Repe).unwrap()),
		(32, "6467F3A6", DecoderOptions::NONE, Instruction::with_cmpsb(16, Register::FS, RepPrefixKind::Repe).unwrap()),
		(64, "6467F3A6", DecoderOptions::NONE, Instruction::with_cmpsb(32, Register::FS, RepPrefixKind::Repe).unwrap()),
		(64, "64F3A6", DecoderOptions::NONE, Instruction::with_cmpsb(64, Register::FS, RepPrefixKind::Repe).unwrap()),
		(32, "646667F3A7", DecoderOptions::NONE, Instruction::with_cmpsw(16, Register::FS, RepPrefixKind::Repe).unwrap()),
		(64, "646667F3A7", DecoderOptions::NONE, Instruction::with_cmpsw(32, Register::FS, RepPrefixKind::Repe).unwrap()),
		(64, "6466F3A7", DecoderOptions::NONE, Instruction::with_cmpsw(64, Register::FS, RepPrefixKind::Repe).unwrap()),
		(32, "6467F3A7", DecoderOptions::NONE, Instruction::with_cmpsd(16, Register::FS, RepPrefixKind::Repe).unwrap()),
		(64, "6467F3A7", DecoderOptions::NONE, Instruction::with_cmpsd(32, Register::FS, RepPrefixKind::Repe).unwrap()),
		(64, "64F3A7", DecoderOptions::NONE, Instruction::with_cmpsd(64, Register::FS, RepPrefixKind::Repe).unwrap()),
		(64, "6467F348A7", DecoderOptions::NONE, Instruction::with_cmpsq(32, Register::FS, RepPrefixKind::Repe).unwrap()),
		(64, "64F348A7", DecoderOptions::NONE, Instruction::with_cmpsq(64, Register::FS, RepPrefixKind::Repe).unwrap()),
		(32, "6467F3A4", DecoderOptions::NONE, Instruction::with_movsb(16, Register::FS, RepPrefixKind::Repe).unwrap()),
		(64, "6467F3A4", DecoderOptions::NONE, Instruction::with_movsb(32, Register::FS, RepPrefixKind::Repe).unwrap()),
		(64, "64F3A4", DecoderOptions::NONE, Instruction::with_movsb(64, Register::FS, RepPrefixKind::Repe).unwrap()),
		(32, "646667F3A5", DecoderOptions::NONE, Instruction::with_movsw(16, Register::FS, RepPrefixKind::Repe).unwrap()),
		(64, "646667F3A5", DecoderOptions::NONE, Instruction::with_movsw(32, Register::FS, RepPrefixKind::Repe).unwrap()),
		(64, "6466F3A5", DecoderOptions::NONE, Instruction::with_movsw(64, Register::FS, RepPrefixKind::Repe).unwrap()),
		(32, "6467F3A5", DecoderOptions::NONE, Instruction::with_movsd(16, Register::FS, RepPrefixKind::Repe).unwrap()),
		(64, "6467F3A5", DecoderOptions::NONE, Instruction::with_movsd(32, Register::FS, RepPrefixKind::Repe).unwrap()),
		(64, "64F3A5", DecoderOptions::NONE, Instruction::with_movsd(64, Register::FS, RepPrefixKind::Repe).unwrap()),
		(64, "6467F348A5", DecoderOptions::NONE, Instruction::with_movsq(32, Register::FS, RepPrefixKind::Repe).unwrap()),
		(64, "64F348A5", DecoderOptions::NONE, Instruction::with_movsq(64, Register::FS, RepPrefixKind::Repe).unwrap()),

		(32, "6467F26E", DecoderOptions::NONE, Instruction::with_outsb(16, Register::FS, RepPrefixKind::Repne).unwrap()),
		(64, "6467F26E", DecoderOptions::NONE, Instruction::with_outsb(32, Register::FS, RepPrefixKind::Repne).unwrap()),
		(64, "64F26E", DecoderOptions::NONE, Instruction::with_outsb(64, Register::FS, RepPrefixKind::Repne).unwrap()),
		(32, "646667F26F", DecoderOptions::NONE, Instruction::with_outsw(16, Register::FS, RepPrefixKind::Repne).unwrap()),
		(64, "646667F26F", DecoderOptions::NONE, Instruction::with_outsw(32, Register::FS, RepPrefixKind::Repne).unwrap()),
		(64, "6466F26F", DecoderOptions::NONE, Instruction::with_outsw(64, Register::FS, RepPrefixKind::Repne).unwrap()),
		(32, "6467F26F", DecoderOptions::NONE, Instruction::with_outsd(16, Register::FS, RepPrefixKind::Repne).unwrap()),
		(64, "6467F26F", DecoderOptions::NONE, Instruction::with_outsd(32, Register::FS, RepPrefixKind::Repne).unwrap()),
		(64, "64F26F", DecoderOptions::NONE, Instruction::with_outsd(64, Register::FS, RepPrefixKind::Repne).unwrap()),
		(32, "67F2AE", DecoderOptions::NONE, Instruction::with_scasb(16, RepPrefixKind::Repne).unwrap()),
		(64, "67F2AE", DecoderOptions::NONE, Instruction::with_scasb(32, RepPrefixKind::Repne).unwrap()),
		(64, "F2AE", DecoderOptions::NONE, Instruction::with_scasb(64, RepPrefixKind::Repne).unwrap()),
		(32, "6667F2AF", DecoderOptions::NONE, Instruction::with_scasw(16, RepPrefixKind::Repne).unwrap()),
		(64, "6667F2AF", DecoderOptions::NONE, Instruction::with_scasw(32, RepPrefixKind::Repne).unwrap()),
		(64, "66F2AF", DecoderOptions::NONE, Instruction::with_scasw(64, RepPrefixKind::Repne).unwrap()),
		(32, "67F2AF", DecoderOptions::NONE, Instruction::with_scasd(16, RepPrefixKind::Repne).unwrap()),
		(64, "67F2AF", DecoderOptions::NONE, Instruction::with_scasd(32, RepPrefixKind::Repne).unwrap()),
		(64, "F2AF", DecoderOptions::NONE, Instruction::with_scasd(64, RepPrefixKind::Repne).unwrap()),
		(64, "67F248AF", DecoderOptions::NONE, Instruction::with_scasq(32, RepPrefixKind::Repne).unwrap()),
		(64, "F248AF", DecoderOptions::NONE, Instruction::with_scasq(64, RepPrefixKind::Repne).unwrap()),
		(32, "6467F2AC", DecoderOptions::NONE, Instruction::with_lodsb(16, Register::FS, RepPrefixKind::Repne).unwrap()),
		(64, "6467F2AC", DecoderOptions::NONE, Instruction::with_lodsb(32, Register::FS, RepPrefixKind::Repne).unwrap()),
		(64, "64F2AC", DecoderOptions::NONE, Instruction::with_lodsb(64, Register::FS, RepPrefixKind::Repne).unwrap()),
		(32, "646667F2AD", DecoderOptions::NONE, Instruction::with_lodsw(16, Register::FS, RepPrefixKind::Repne).unwrap()),
		(64, "646667F2AD", DecoderOptions::NONE, Instruction::with_lodsw(32, Register::FS, RepPrefixKind::Repne).unwrap()),
		(64, "6466F2AD", DecoderOptions::NONE, Instruction::with_lodsw(64, Register::FS, RepPrefixKind::Repne).unwrap()),
		(32, "6467F2AD", DecoderOptions::NONE, Instruction::with_lodsd(16, Register::FS, RepPrefixKind::Repne).unwrap()),
		(64, "6467F2AD", DecoderOptions::NONE, Instruction::with_lodsd(32, Register::FS, RepPrefixKind::Repne).unwrap()),
		(64, "64F2AD", DecoderOptions::NONE, Instruction::with_lodsd(64, Register::FS, RepPrefixKind::Repne).unwrap()),
		(64, "6467F248AD", DecoderOptions::NONE, Instruction::with_lodsq(32, Register::FS, RepPrefixKind::Repne).unwrap()),
		(64, "64F248AD", DecoderOptions::NONE, Instruction::with_lodsq(64, Register::FS, RepPrefixKind::Repne).unwrap()),
		(32, "67F26C", DecoderOptions::NONE, Instruction::with_insb(16, RepPrefixKind::Repne).unwrap()),
		(64, "67F26C", DecoderOptions::NONE, Instruction::with_insb(32, RepPrefixKind::Repne).unwrap()),
		(64, "F26C", DecoderOptions::NONE, Instruction::with_insb(64, RepPrefixKind::Repne).unwrap()),
		(32, "6667F26D", DecoderOptions::NONE, Instruction::with_insw(16, RepPrefixKind::Repne).unwrap()),
		(64, "6667F26D", DecoderOptions::NONE, Instruction::with_insw(32, RepPrefixKind::Repne).unwrap()),
		(64, "66F26D", DecoderOptions::NONE, Instruction::with_insw(64, RepPrefixKind::Repne).unwrap()),
		(32, "67F26D", DecoderOptions::NONE, Instruction::with_insd(16, RepPrefixKind::Repne).unwrap()),
		(64, "67F26D", DecoderOptions::NONE, Instruction::with_insd(32, RepPrefixKind::Repne).unwrap()),
		(64, "F26D", DecoderOptions::NONE, Instruction::with_insd(64, RepPrefixKind::Repne).unwrap()),
		(32, "67F2AA", DecoderOptions::NONE, Instruction::with_stosb(16, RepPrefixKind::Repne).unwrap()),
		(64, "67F2AA", DecoderOptions::NONE, Instruction::with_stosb(32, RepPrefixKind::Repne).unwrap()),
		(64, "F2AA", DecoderOptions::NONE, Instruction::with_stosb(64, RepPrefixKind::Repne).unwrap()),
		(32, "6667F2AB", DecoderOptions::NONE, Instruction::with_stosw(16, RepPrefixKind::Repne).unwrap()),
		(64, "6667F2AB", DecoderOptions::NONE, Instruction::with_stosw(32, RepPrefixKind::Repne).unwrap()),
		(64, "66F2AB", DecoderOptions::NONE, Instruction::with_stosw(64, RepPrefixKind::Repne).unwrap()),
		(32, "67F2AB", DecoderOptions::NONE, Instruction::with_stosd(16, RepPrefixKind::Repne).unwrap()),
		(64, "67F2AB", DecoderOptions::NONE, Instruction::with_stosd(32, RepPrefixKind::Repne).unwrap()),
		(64, "F2AB", DecoderOptions::NONE, Instruction::with_stosd(64, RepPrefixKind::Repne).unwrap()),
		(64, "67F248AB", DecoderOptions::NONE, Instruction::with_stosq(32, RepPrefixKind::Repne).unwrap()),
		(64, "F248AB", DecoderOptions::NONE, Instruction::with_stosq(64, RepPrefixKind::Repne).unwrap()),
		(32, "6467F2A6", DecoderOptions::NONE, Instruction::with_cmpsb(16, Register::FS, RepPrefixKind::Repne).unwrap()),
		(64, "6467F2A6", DecoderOptions::NONE, Instruction::with_cmpsb(32, Register::FS, RepPrefixKind::Repne).unwrap()),
		(64, "64F2A6", DecoderOptions::NONE, Instruction::with_cmpsb(64, Register::FS, RepPrefixKind::Repne).unwrap()),
		(32, "646667F2A7", DecoderOptions::NONE, Instruction::with_cmpsw(16, Register::FS, RepPrefixKind::Repne).unwrap()),
		(64, "646667F2A7", DecoderOptions::NONE, Instruction::with_cmpsw(32, Register::FS, RepPrefixKind::Repne).unwrap()),
		(64, "6466F2A7", DecoderOptions::NONE, Instruction::with_cmpsw(64, Register::FS, RepPrefixKind::Repne).unwrap()),
		(32, "6467F2A7", DecoderOptions::NONE, Instruction::with_cmpsd(16, Register::FS, RepPrefixKind::Repne).unwrap()),
		(64, "6467F2A7", DecoderOptions::NONE, Instruction::with_cmpsd(32, Register::FS, RepPrefixKind::Repne).unwrap()),
		(64, "64F2A7", DecoderOptions::NONE, Instruction::with_cmpsd(64, Register::FS, RepPrefixKind::Repne).unwrap()),
		(64, "6467F248A7", DecoderOptions::NONE, Instruction::with_cmpsq(32, Register::FS, RepPrefixKind::Repne).unwrap()),
		(64, "64F248A7", DecoderOptions::NONE, Instruction::with_cmpsq(64, Register::FS, RepPrefixKind::Repne).unwrap()),
		(32, "6467F2A4", DecoderOptions::NONE, Instruction::with_movsb(16, Register::FS, RepPrefixKind::Repne).unwrap()),
		(64, "6467F2A4", DecoderOptions::NONE, Instruction::with_movsb(32, Register::FS, RepPrefixKind::Repne).unwrap()),
		(64, "64F2A4", DecoderOptions::NONE, Instruction::with_movsb(64, Register::FS, RepPrefixKind::Repne).unwrap()),
		(32, "646667F2A5", DecoderOptions::NONE, Instruction::with_movsw(16, Register::FS, RepPrefixKind::Repne).unwrap()),
		(64, "646667F2A5", DecoderOptions::NONE, Instruction::with_movsw(32, Register::FS, RepPrefixKind::Repne).unwrap()),
		(64, "6466F2A5", DecoderOptions::NONE, Instruction::with_movsw(64, Register::FS, RepPrefixKind::Repne).unwrap()),
		(32, "6467F2A5", DecoderOptions::NONE, Instruction::with_movsd(16, Register::FS, RepPrefixKind::Repne).unwrap()),
		(64, "6467F2A5", DecoderOptions::NONE, Instruction::with_movsd(32, Register::FS, RepPrefixKind::Repne).unwrap()),
		(64, "64F2A5", DecoderOptions::NONE, Instruction::with_movsd(64, Register::FS, RepPrefixKind::Repne).unwrap()),
		(64, "6467F248A5", DecoderOptions::NONE, Instruction::with_movsq(32, Register::FS, RepPrefixKind::Repne).unwrap()),
		(64, "64F248A5", DecoderOptions::NONE, Instruction::with_movsq(64, Register::FS, RepPrefixKind::Repne).unwrap()),

		(32, "67F36E", DecoderOptions::NONE, Instruction::with_rep_outsb(16).unwrap()),
		(64, "67F36E", DecoderOptions::NONE, Instruction::with_rep_outsb(32).unwrap()),
		(64, "F36E", DecoderOptions::NONE, Instruction::with_rep_outsb(64).unwrap()),
		(32, "6667F36F", DecoderOptions::NONE, Instruction::with_rep_outsw(16).unwrap()),
		(64, "6667F36F", DecoderOptions::NONE, Instruction::with_rep_outsw(32).unwrap()),
		(64, "66F36F", DecoderOptions::NONE, Instruction::with_rep_outsw(64).unwrap()),
		(32, "67F36F", DecoderOptions::NONE, Instruction::with_rep_outsd(16).unwrap()),
		(64, "67F36F", DecoderOptions::NONE, Instruction::with_rep_outsd(32).unwrap()),
		(64, "F36F", DecoderOptions::NONE, Instruction::with_rep_outsd(64).unwrap()),
		(32, "67F3AE", DecoderOptions::NONE, Instruction::with_repe_scasb(16).unwrap()),
		(64, "67F3AE", DecoderOptions::NONE, Instruction::with_repe_scasb(32).unwrap()),
		(64, "F3AE", DecoderOptions::NONE, Instruction::with_repe_scasb(64).unwrap()),
		(32, "6667F3AF", DecoderOptions::NONE, Instruction::with_repe_scasw(16).unwrap()),
		(64, "6667F3AF", DecoderOptions::NONE, Instruction::with_repe_scasw(32).unwrap()),
		(64, "66F3AF", DecoderOptions::NONE, Instruction::with_repe_scasw(64).unwrap()),
		(32, "67F3AF", DecoderOptions::NONE, Instruction::with_repe_scasd(16).unwrap()),
		(64, "67F3AF", DecoderOptions::NONE, Instruction::with_repe_scasd(32).unwrap()),
		(64, "F3AF", DecoderOptions::NONE, Instruction::with_repe_scasd(64).unwrap()),
		(64, "67F348AF", DecoderOptions::NONE, Instruction::with_repe_scasq(32).unwrap()),
		(64, "F348AF", DecoderOptions::NONE, Instruction::with_repe_scasq(64).unwrap()),
		(32, "67F2AE", DecoderOptions::NONE, Instruction::with_repne_scasb(16).unwrap()),
		(64, "67F2AE", DecoderOptions::NONE, Instruction::with_repne_scasb(32).unwrap()),
		(64, "F2AE", DecoderOptions::NONE, Instruction::with_repne_scasb(64).unwrap()),
		(32, "6667F2AF", DecoderOptions::NONE, Instruction::with_repne_scasw(16).unwrap()),
		(64, "6667F2AF", DecoderOptions::NONE, Instruction::with_repne_scasw(32).unwrap()),
		(64, "66F2AF", DecoderOptions::NONE, Instruction::with_repne_scasw(64).unwrap()),
		(32, "67F2AF", DecoderOptions::NONE, Instruction::with_repne_scasd(16).unwrap()),
		(64, "67F2AF", DecoderOptions::NONE, Instruction::with_repne_scasd(32).unwrap()),
		(64, "F2AF", DecoderOptions::NONE, Instruction::with_repne_scasd(64).unwrap()),
		(64, "67F248AF", DecoderOptions::NONE, Instruction::with_repne_scasq(32).unwrap()),
		(64, "F248AF", DecoderOptions::NONE, Instruction::with_repne_scasq(64).unwrap()),
		(32, "67F3AC", DecoderOptions::NONE, Instruction::with_rep_lodsb(16).unwrap()),
		(64, "67F3AC", DecoderOptions::NONE, Instruction::with_rep_lodsb(32).unwrap()),
		(64, "F3AC", DecoderOptions::NONE, Instruction::with_rep_lodsb(64).unwrap()),
		(32, "6667F3AD", DecoderOptions::NONE, Instruction::with_rep_lodsw(16).unwrap()),
		(64, "6667F3AD", DecoderOptions::NONE, Instruction::with_rep_lodsw(32).unwrap()),
		(64, "66F3AD", DecoderOptions::NONE, Instruction::with_rep_lodsw(64).unwrap()),
		(32, "67F3AD", DecoderOptions::NONE, Instruction::with_rep_lodsd(16).unwrap()),
		(64, "67F3AD", DecoderOptions::NONE, Instruction::with_rep_lodsd(32).unwrap()),
		(64, "F3AD", DecoderOptions::NONE, Instruction::with_rep_lodsd(64).unwrap()),
		(64, "67F348AD", DecoderOptions::NONE, Instruction::with_rep_lodsq(32).unwrap()),
		(64, "F348AD", DecoderOptions::NONE, Instruction::with_rep_lodsq(64).unwrap()),
		(32, "67F36C", DecoderOptions::NONE, Instruction::with_rep_insb(16).unwrap()),
		(64, "67F36C", DecoderOptions::NONE, Instruction::with_rep_insb(32).unwrap()),
		(64, "F36C", DecoderOptions::NONE, Instruction::with_rep_insb(64).unwrap()),
		(32, "6667F36D", DecoderOptions::NONE, Instruction::with_rep_insw(16).unwrap()),
		(64, "6667F36D", DecoderOptions::NONE, Instruction::with_rep_insw(32).unwrap()),
		(64, "66F36D", DecoderOptions::NONE, Instruction::with_rep_insw(64).unwrap()),
		(32, "67F36D", DecoderOptions::NONE, Instruction::with_rep_insd(16).unwrap()),
		(64, "67F36D", DecoderOptions::NONE, Instruction::with_rep_insd(32).unwrap()),
		(64, "F36D", DecoderOptions::NONE, Instruction::with_rep_insd(64).unwrap()),
		(32, "67F3AA", DecoderOptions::NONE, Instruction::with_rep_stosb(16).unwrap()),
		(64, "67F3AA", DecoderOptions::NONE, Instruction::with_rep_stosb(32).unwrap()),
		(64, "F3AA", DecoderOptions::NONE, Instruction::with_rep_stosb(64).unwrap()),
		(32, "6667F3AB", DecoderOptions::NONE, Instruction::with_rep_stosw(16).unwrap()),
		(64, "6667F3AB", DecoderOptions::NONE, Instruction::with_rep_stosw(32).unwrap()),
		(64, "66F3AB", DecoderOptions::NONE, Instruction::with_rep_stosw(64).unwrap()),
		(32, "67F3AB", DecoderOptions::NONE, Instruction::with_rep_stosd(16).unwrap()),
		(64, "67F3AB", DecoderOptions::NONE, Instruction::with_rep_stosd(32).unwrap()),
		(64, "F3AB", DecoderOptions::NONE, Instruction::with_rep_stosd(64).unwrap()),
		(64, "67F348AB", DecoderOptions::NONE, Instruction::with_rep_stosq(32).unwrap()),
		(64, "F348AB", DecoderOptions::NONE, Instruction::with_rep_stosq(64).unwrap()),
		(32, "67F3A6", DecoderOptions::NONE, Instruction::with_repe_cmpsb(16).unwrap()),
		(64, "67F3A6", DecoderOptions::NONE, Instruction::with_repe_cmpsb(32).unwrap()),
		(64, "F3A6", DecoderOptions::NONE, Instruction::with_repe_cmpsb(64).unwrap()),
		(32, "6667F3A7", DecoderOptions::NONE, Instruction::with_repe_cmpsw(16).unwrap()),
		(64, "6667F3A7", DecoderOptions::NONE, Instruction::with_repe_cmpsw(32).unwrap()),
		(64, "66F3A7", DecoderOptions::NONE, Instruction::with_repe_cmpsw(64).unwrap()),
		(32, "67F3A7", DecoderOptions::NONE, Instruction::with_repe_cmpsd(16).unwrap()),
		(64, "67F3A7", DecoderOptions::NONE, Instruction::with_repe_cmpsd(32).unwrap()),
		(64, "F3A7", DecoderOptions::NONE, Instruction::with_repe_cmpsd(64).unwrap()),
		(64, "67F348A7", DecoderOptions::NONE, Instruction::with_repe_cmpsq(32).unwrap()),
		(64, "F348A7", DecoderOptions::NONE, Instruction::with_repe_cmpsq(64).unwrap()),
		(32, "67F2A6", DecoderOptions::NONE, Instruction::with_repne_cmpsb(16).unwrap()),
		(64, "67F2A6", DecoderOptions::NONE, Instruction::with_repne_cmpsb(32).unwrap()),
		(64, "F2A6", DecoderOptions::NONE, Instruction::with_repne_cmpsb(64).unwrap()),
		(32, "6667F2A7", DecoderOptions::NONE, Instruction::with_repne_cmpsw(16).unwrap()),
		(64, "6667F2A7", DecoderOptions::NONE, Instruction::with_repne_cmpsw(32).unwrap()),
		(64, "66F2A7", DecoderOptions::NONE, Instruction::with_repne_cmpsw(64).unwrap()),
		(32, "67F2A7", DecoderOptions::NONE, Instruction::with_repne_cmpsd(16).unwrap()),
		(64, "67F2A7", DecoderOptions::NONE, Instruction::with_repne_cmpsd(32).unwrap()),
		(64, "F2A7", DecoderOptions::NONE, Instruction::with_repne_cmpsd(64).unwrap()),
		(64, "67F248A7", DecoderOptions::NONE, Instruction::with_repne_cmpsq(32).unwrap()),
		(64, "F248A7", DecoderOptions::NONE, Instruction::with_repne_cmpsq(64).unwrap()),
		(32, "67F3A4", DecoderOptions::NONE, Instruction::with_rep_movsb(16).unwrap()),
		(64, "67F3A4", DecoderOptions::NONE, Instruction::with_rep_movsb(32).unwrap()),
		(64, "F3A4", DecoderOptions::NONE, Instruction::with_rep_movsb(64).unwrap()),
		(32, "6667F3A5", DecoderOptions::NONE, Instruction::with_rep_movsw(16).unwrap()),
		(64, "6667F3A5", DecoderOptions::NONE, Instruction::with_rep_movsw(32).unwrap()),
		(64, "66F3A5", DecoderOptions::NONE, Instruction::with_rep_movsw(64).unwrap()),
		(32, "67F3A5", DecoderOptions::NONE, Instruction::with_rep_movsd(16).unwrap()),
		(64, "67F3A5", DecoderOptions::NONE, Instruction::with_rep_movsd(32).unwrap()),
		(64, "F3A5", DecoderOptions::NONE, Instruction::with_rep_movsd(64).unwrap()),
		(64, "67F348A5", DecoderOptions::NONE, Instruction::with_rep_movsq(32).unwrap()),
		(64, "F348A5", DecoderOptions::NONE, Instruction::with_rep_movsq(64).unwrap()),
	];
	with_test_core(tests);
}

#[test]
#[cfg(not(feature = "no_vex"))]
fn try_with_test_vex() {
	#[rustfmt::skip]
	let tests: Vec<(u32, &str, u32, Instruction)> = vec![
		(64, "C5E814CB", DecoderOptions::NONE, Instruction::with3(Code::VEX_Vunpcklps_xmm_xmm_xmmm128, Register::XMM1, Register::XMM2, Register::XMM3).unwrap()),
		(64, "64C5E8148C7501EFCDAB", DecoderOptions::NONE, Instruction::with3(Code::VEX_Vunpcklps_xmm_xmm_xmmm128, Register::XMM1, Register::XMM2, MemoryOperand::new(Register::RBP, Register::RSI, 2, -0x5432_10FF, 8, false, Register::FS)).unwrap()),
		(64, "64C4E261908C7501EFCDAB", DecoderOptions::NONE, Instruction::with3(Code::VEX_Vpgatherdd_xmm_vm32x_xmm, Register::XMM1, MemoryOperand::new(Register::RBP, Register::XMM6, 2, -0x5432_10FF, 8, false, Register::FS), Register::XMM3).unwrap()),
		(64, "64C4E2692E9C7501EFCDAB", DecoderOptions::NONE, Instruction::with3(Code::VEX_Vmaskmovps_m128_xmm_xmm, MemoryOperand::new(Register::RBP, Register::RSI, 2, -0x5432_10FF, 8, false, Register::FS), Register::XMM2, Register::XMM3).unwrap()),
		(64, "C4E3694ACB40", DecoderOptions::NONE, Instruction::with4(Code::VEX_Vblendvps_xmm_xmm_xmmm128_xmm, Register::XMM1, Register::XMM2, Register::XMM3, Register::XMM4).unwrap()),
		(64, "64C4E3E95C8C7501EFCDAB30", DecoderOptions::NONE, Instruction::with4(Code::VEX_Vfmaddsubps_xmm_xmm_xmm_xmmm128, Register::XMM1, Register::XMM2, Register::XMM3, MemoryOperand::new(Register::RBP, Register::RSI, 2, -0x5432_10FF, 8, false, Register::FS)).unwrap()),
		(64, "64C4E3694A8C7501EFCDAB40", DecoderOptions::NONE, Instruction::with4(Code::VEX_Vblendvps_xmm_xmm_xmmm128_xmm, Register::XMM1, Register::XMM2, MemoryOperand::new(Register::RBP, Register::RSI, 2, -0x5432_10FF, 8, false, Register::FS), Register::XMM4).unwrap()),
		(64, "C4E36948CB40", DecoderOptions::NONE, Instruction::with5(Code::VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm4, Register::XMM1, Register::XMM2, Register::XMM3, Register::XMM4, 0x0).unwrap()),
		(64, "C4E36948CB40", DecoderOptions::NONE, Instruction::with5(Code::VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm4, Register::XMM1, Register::XMM2, Register::XMM3, Register::XMM4, 0x0).unwrap()),
		(64, "64C4E3E9488C7501EFCDAB31", DecoderOptions::NONE, Instruction::with5(Code::VEX_Vpermil2ps_xmm_xmm_xmm_xmmm128_imm4, Register::XMM1, Register::XMM2, Register::XMM3, MemoryOperand::new(Register::RBP, Register::RSI, 2, -0x5432_10FF, 8, false, Register::FS), 0x1).unwrap()),
		(64, "64C4E3E9488C7501EFCDAB31", DecoderOptions::NONE, Instruction::with5(Code::VEX_Vpermil2ps_xmm_xmm_xmm_xmmm128_imm4, Register::XMM1, Register::XMM2, Register::XMM3, MemoryOperand::new(Register::RBP, Register::RSI, 2, -0x5432_10FF, 8, false, Register::FS), 0x1).unwrap()),
		(64, "64C4E369488C7501EFCDAB41", DecoderOptions::NONE, Instruction::with5(Code::VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm4, Register::XMM1, Register::XMM2, MemoryOperand::new(Register::RBP, Register::RSI, 2, -0x5432_10FF, 8, false, Register::FS), Register::XMM4, 0x1).unwrap()),
		(64, "64C4E369488C7501EFCDAB41", DecoderOptions::NONE, Instruction::with5(Code::VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm4, Register::XMM1, Register::XMM2, MemoryOperand::new(Register::RBP, Register::RSI, 2, -0x5432_10FF, 8, false, Register::FS), Register::XMM4, 0x1).unwrap()),
		(32, "6467C5F9F7D3", DecoderOptions::NONE, Instruction::with_vmaskmovdqu(16, Register::XMM2, Register::XMM3, Register::FS).unwrap()),
		(64, "6467C5F9F7D3", DecoderOptions::NONE, Instruction::with_vmaskmovdqu(32, Register::XMM2, Register::XMM3, Register::FS).unwrap()),
		(64, "64C5F9F7D3", DecoderOptions::NONE, Instruction::with_vmaskmovdqu(64, Register::XMM2, Register::XMM3, Register::FS).unwrap()),
	];
	with_test_core(tests);
}

#[test]
#[cfg(not(feature = "no_evex"))]
fn try_with_test_evex() {
	#[rustfmt::skip]
	let tests: Vec<(u32, &str, u32, Instruction)> = vec![
		(64, "62F1F50873D2A5", DecoderOptions::NONE, Instruction::with3(Code::EVEX_Vpsrlq_xmm_k1z_xmmm128b64_imm8, Register::XMM1, Register::XMM2, 0xA5).unwrap()),
		(64, "6462F1F50873947501EFCDABA5", DecoderOptions::NONE, Instruction::with3(Code::EVEX_Vpsrlq_xmm_k1z_xmmm128b64_imm8, Register::XMM1, MemoryOperand::new(Register::RBP, Register::RSI, 2, -0x5432_10FF, 8, false, Register::FS), 0xA5).unwrap()),
		(64, "62F16D08C4CBA5", DecoderOptions::NONE, Instruction::with4(Code::EVEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register::XMM1, Register::XMM2, Register::EBX, 0xA5).unwrap()),
		(64, "62F16D08C4CBA5", DecoderOptions::NONE, Instruction::with4(Code::EVEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register::XMM1, Register::XMM2, Register::EBX, 0xA5).unwrap()),
		(64, "6462F16D08C48C7501EFCDABA5", DecoderOptions::NONE, Instruction::with4(Code::EVEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register::XMM1, Register::XMM2, MemoryOperand::new(Register::RBP, Register::RSI, 2, -0x5432_10FF, 8, false, Register::FS), 0xA5).unwrap()),
		(64, "6462F16D08C48C7501EFCDABA5", DecoderOptions::NONE, Instruction::with4(Code::EVEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register::XMM1, Register::XMM2, MemoryOperand::new(Register::RBP, Register::RSI, 2, -0x5432_10FF, 8, false, Register::FS), 0xA5).unwrap()),
	];
	with_test_core(tests);
}

fn with_test_core(tests: Vec<(u32, &str, u32, Instruction)>) {
	for (bitness, hex_bytes, options, created_instr) in tests {
		let bytes = to_vec_u8(hex_bytes).unwrap();
		let mut decoder = create_decoder(bitness, bytes.as_slice(), get_default_ip(bitness), options).0;
		let orig_rip = decoder.ip();
		let mut decoded_instr = decoder.decode();
		decoded_instr.set_code_size(CodeSize::default());
		decoded_instr.set_len(0);
		decoded_instr.set_next_ip(0);

		assert!(decoded_instr.eq_all_bits(&created_instr));

		let mut encoder = Encoder::new(decoder.bitness());
		let _ = encoder.encode(&created_instr, orig_rip).unwrap();
		assert_eq!(encoder.take_buffer(), bytes);
	}
}

#[test]
fn with_declare_xxx_fails_if_invalid_length() {
	#[rustfmt::skip]
	let tests: Vec<fn() -> Result<Instruction, IcedError>> = vec![
		|| Instruction::with_declare_byte(&[0; 0]),
		|| Instruction::with_declare_byte(&[0; 17]),

		|| Instruction::with_declare_word_slice_u8(&[0; 0]),
		|| Instruction::with_declare_word_slice_u8(&[0; 1]),
		|| Instruction::with_declare_word_slice_u8(&[0; 3]),
		|| Instruction::with_declare_word_slice_u8(&[0; 5]),
		|| Instruction::with_declare_word_slice_u8(&[0; 7]),
		|| Instruction::with_declare_word_slice_u8(&[0; 9]),
		|| Instruction::with_declare_word_slice_u8(&[0; 11]),
		|| Instruction::with_declare_word_slice_u8(&[0; 13]),
		|| Instruction::with_declare_word_slice_u8(&[0; 15]),
		|| Instruction::with_declare_word_slice_u8(&[0; 17]),
		|| Instruction::with_declare_word_slice_u8(&[0; 18]),
		|| Instruction::with_declare_word(&[0; 0]),
		|| Instruction::with_declare_word(&[0; 9]),

		|| Instruction::with_declare_dword_slice_u8(&[0; 0]),
		|| Instruction::with_declare_dword_slice_u8(&[0; 1]),
		|| Instruction::with_declare_dword_slice_u8(&[0; 2]),
		|| Instruction::with_declare_dword_slice_u8(&[0; 3]),
		|| Instruction::with_declare_dword_slice_u8(&[0; 5]),
		|| Instruction::with_declare_dword_slice_u8(&[0; 6]),
		|| Instruction::with_declare_dword_slice_u8(&[0; 7]),
		|| Instruction::with_declare_dword_slice_u8(&[0; 9]),
		|| Instruction::with_declare_dword_slice_u8(&[0; 10]),
		|| Instruction::with_declare_dword_slice_u8(&[0; 11]),
		|| Instruction::with_declare_dword_slice_u8(&[0; 13]),
		|| Instruction::with_declare_dword_slice_u8(&[0; 14]),
		|| Instruction::with_declare_dword_slice_u8(&[0; 15]),
		|| Instruction::with_declare_dword_slice_u8(&[0; 17]),
		|| Instruction::with_declare_dword_slice_u8(&[0; 20]),
		|| Instruction::with_declare_dword(&[0; 0]),
		|| Instruction::with_declare_dword(&[0; 5]),

		|| Instruction::with_declare_qword_slice_u8(&[0; 0]),
		|| Instruction::with_declare_qword_slice_u8(&[0; 1]),
		|| Instruction::with_declare_qword_slice_u8(&[0; 2]),
		|| Instruction::with_declare_qword_slice_u8(&[0; 3]),
		|| Instruction::with_declare_qword_slice_u8(&[0; 4]),
		|| Instruction::with_declare_qword_slice_u8(&[0; 5]),
		|| Instruction::with_declare_qword_slice_u8(&[0; 6]),
		|| Instruction::with_declare_qword_slice_u8(&[0; 7]),
		|| Instruction::with_declare_qword_slice_u8(&[0; 9]),
		|| Instruction::with_declare_qword_slice_u8(&[0; 10]),
		|| Instruction::with_declare_qword_slice_u8(&[0; 11]),
		|| Instruction::with_declare_qword_slice_u8(&[0; 12]),
		|| Instruction::with_declare_qword_slice_u8(&[0; 13]),
		|| Instruction::with_declare_qword_slice_u8(&[0; 14]),
		|| Instruction::with_declare_qword_slice_u8(&[0; 15]),
		|| Instruction::with_declare_qword_slice_u8(&[0; 17]),
		|| Instruction::with_declare_qword_slice_u8(&[0; 24]),
		|| Instruction::with_declare_qword(&[0; 0]),
		|| Instruction::with_declare_qword(&[0; 3]),
	];
	for f in tests {
		assert!(f().is_err());
	}
}

#[test]
fn get_set_op_kind_panics_if_invalid_input() {
	let mut instr = Instruction::with2(Code::Adc_EAX_imm32, Register::EAX, u32::MAX).unwrap();

	let _ = instr.op_kind(0);
	let _ = instr.op_kind(1);
	for i in 2..IcedConstants::MAX_OP_COUNT as u32 {
		let _ = instr.op_kind(i);
	}
	if cfg!(debug_assertions) {
		assert!(panic::catch_unwind(|| { instr.op_kind(IcedConstants::MAX_OP_COUNT as u32) }).is_err());
	} else {
		assert_eq!(instr.op_kind(IcedConstants::MAX_OP_COUNT as u32), OpKind::default());
	}

	instr.set_op_kind(0, OpKind::Register);
	instr.set_op_kind(1, OpKind::Immediate32);
	for i in 2..IcedConstants::MAX_OP_COUNT as u32 {
		instr.set_op_kind(i, OpKind::Immediate8);
	}
	if cfg!(debug_assertions) {
		assert!(panic::catch_unwind(move || { instr.set_op_kind(IcedConstants::MAX_OP_COUNT as u32, OpKind::Register) }).is_err());
	} else {
		instr.set_op_kind(IcedConstants::MAX_OP_COUNT as u32, OpKind::Register);
	}
}

#[test]
fn get_set_op_kind_fails_if_invalid_input() {
	let mut instr = Instruction::with2(Code::Adc_EAX_imm32, Register::EAX, u32::MAX).unwrap();

	let _ = instr.try_op_kind(0).unwrap();
	let _ = instr.try_op_kind(1).unwrap();
	for i in 2..IcedConstants::MAX_OP_COUNT as u32 {
		let _ = instr.try_op_kind(i).unwrap();
	}
	assert!(instr.try_op_kind(IcedConstants::MAX_OP_COUNT as u32).is_err());

	instr.try_set_op_kind(0, OpKind::Register).unwrap();
	instr.try_set_op_kind(1, OpKind::Immediate32).unwrap();
	for i in 2..IcedConstants::MAX_OP_COUNT as u32 {
		instr.try_set_op_kind(i, OpKind::Immediate8).unwrap();
	}
	assert!(instr.try_set_op_kind(IcedConstants::MAX_OP_COUNT as u32, OpKind::Register).is_err());
}

#[test]
fn try_immediate_fails_if_invalid_input() {
	let instr = Instruction::with2(Code::Adc_EAX_imm32, Register::EAX, u32::MAX).unwrap();

	assert!(instr.try_immediate(0).is_err());
	assert!(instr.try_immediate(1).is_ok());
	for i in 2..IcedConstants::MAX_OP_COUNT as u32 {
		if i == 4 && instr.op4_kind() == OpKind::Immediate8 {
			continue;
		}
		assert!(instr.try_immediate(i).is_err());
	}
}

#[test]
fn get_set_immediate_panics_if_invalid_input() {
	let mut instr = Instruction::with2(Code::Adc_EAX_imm32, Register::EAX, u32::MAX).unwrap();

	if cfg!(debug_assertions) {
		assert!(panic::catch_unwind(move || { instr.immediate(0) }).is_err());
	} else {
		let _ = instr.immediate(0);
	}
	let _ = instr.immediate(1);
	for i in 2..IcedConstants::MAX_OP_COUNT as u32 {
		if i == 4 && instr.op4_kind() == OpKind::Immediate8 {
			continue;
		}
		if cfg!(debug_assertions) {
			assert!(panic::catch_unwind(move || { instr.immediate(i) }).is_err());
		} else {
			let _ = instr.immediate(i);
		}
	}
	if cfg!(debug_assertions) {
		assert!(panic::catch_unwind(move || { instr.immediate(IcedConstants::MAX_OP_COUNT as u32) }).is_err());
	} else {
		let _ = instr.immediate(IcedConstants::MAX_OP_COUNT as u32);
	}

	{
		if cfg!(debug_assertions) {
			let mut instr = instr;
			assert!(panic::catch_unwind(move || { instr.set_immediate_i32(0, 0) }).is_err());
		} else {
			instr.set_immediate_i32(0, 0);
		}
	}
	{
		if cfg!(debug_assertions) {
			let mut instr = instr;
			assert!(panic::catch_unwind(move || { instr.set_immediate_i64(0, 0) }).is_err());
		} else {
			instr.set_immediate_i64(0, 0);
		}
	}
	{
		if cfg!(debug_assertions) {
			let mut instr = instr;
			assert!(panic::catch_unwind(move || { instr.set_immediate_u32(0, 0) }).is_err());
		} else {
			instr.set_immediate_u32(0, 0);
		}
	}
	{
		if cfg!(debug_assertions) {
			let mut instr = instr;
			assert!(panic::catch_unwind(move || { instr.set_immediate_u64(0, 0) }).is_err());
		} else {
			instr.set_immediate_u64(0, 0);
		}
	}

	instr.set_immediate_i32(1, 0);
	instr.set_immediate_i64(1, 0);
	instr.set_immediate_u32(1, 0);
	instr.set_immediate_u64(1, 0);

	for i in 2..IcedConstants::MAX_OP_COUNT as u32 {
		if i == 4 && instr.op4_kind() == OpKind::Immediate8 {
			continue;
		}
		{
			if cfg!(debug_assertions) {
				let mut instr = instr;
				assert!(panic::catch_unwind(move || { instr.set_immediate_i32(i, 0) }).is_err());
			} else {
				instr.set_immediate_i32(i, 0);
			}
		}
		{
			if cfg!(debug_assertions) {
				let mut instr = instr;
				assert!(panic::catch_unwind(move || { instr.set_immediate_i64(i, 0) }).is_err());
			} else {
				instr.set_immediate_i64(i, 0);
			}
		}
		{
			if cfg!(debug_assertions) {
				let mut instr = instr;
				assert!(panic::catch_unwind(move || { instr.set_immediate_u32(i, 0) }).is_err());
			} else {
				instr.set_immediate_u32(i, 0);
			}
		}
		{
			if cfg!(debug_assertions) {
				let mut instr = instr;
				assert!(panic::catch_unwind(move || { instr.set_immediate_u64(i, 0) }).is_err());
			} else {
				instr.set_immediate_u64(i, 0);
			}
		}
	}
	if cfg!(debug_assertions) {
		let mut instr = instr;
		assert!(panic::catch_unwind(move || { instr.set_immediate_i32(IcedConstants::MAX_OP_COUNT as u32, 0) }).is_err());
	} else {
		instr.set_immediate_i32(IcedConstants::MAX_OP_COUNT as u32, 0);
	}
	if cfg!(debug_assertions) {
		let mut instr = instr;
		assert!(panic::catch_unwind(move || { instr.set_immediate_i64(IcedConstants::MAX_OP_COUNT as u32, 0) }).is_err());
	} else {
		instr.set_immediate_i64(IcedConstants::MAX_OP_COUNT as u32, 0);
	}
	if cfg!(debug_assertions) {
		let mut instr = instr;
		assert!(panic::catch_unwind(move || { instr.set_immediate_u32(IcedConstants::MAX_OP_COUNT as u32, 0) }).is_err());
	} else {
		instr.set_immediate_u32(IcedConstants::MAX_OP_COUNT as u32, 0);
	}
	if cfg!(debug_assertions) {
		let mut instr = instr;
		assert!(panic::catch_unwind(move || { instr.set_immediate_u64(IcedConstants::MAX_OP_COUNT as u32, 0) }).is_err());
	} else {
		instr.set_immediate_u64(IcedConstants::MAX_OP_COUNT as u32, 0);
	}
}

#[test]
fn get_set_immediate_fails_if_invalid_input() {
	let mut instr = Instruction::with2(Code::Adc_EAX_imm32, Register::EAX, u32::MAX).unwrap();

	if cfg!(debug_assertions) {
		assert!(panic::catch_unwind(move || { instr.immediate(0) }).is_err());
	} else {
		let _ = instr.immediate(0);
	}
	let _ = instr.immediate(1);
	for i in 2..IcedConstants::MAX_OP_COUNT as u32 {
		if i == 4 && instr.op4_kind() == OpKind::Immediate8 {
			continue;
		}
		if cfg!(debug_assertions) {
			assert!(panic::catch_unwind(move || { instr.immediate(i) }).is_err());
		} else {
			let _ = instr.immediate(i);
		}
	}
	if cfg!(debug_assertions) {
		assert!(panic::catch_unwind(move || { instr.immediate(IcedConstants::MAX_OP_COUNT as u32) }).is_err());
	} else {
		let _ = instr.immediate(IcedConstants::MAX_OP_COUNT as u32);
	}

	assert!(instr.try_set_immediate_i32(0, 0).is_err());
	assert!(instr.try_set_immediate_i64(0, 0).is_err());
	assert!(instr.try_set_immediate_u32(0, 0).is_err());
	assert!(instr.try_set_immediate_u64(0, 0).is_err());

	instr.try_set_immediate_i32(1, 0).unwrap();
	instr.try_set_immediate_i64(1, 0).unwrap();
	instr.try_set_immediate_u32(1, 0).unwrap();
	instr.try_set_immediate_u64(1, 0).unwrap();

	for i in 2..IcedConstants::MAX_OP_COUNT as u32 {
		if i == 4 && instr.op4_kind() == OpKind::Immediate8 {
			continue;
		}
		assert!(instr.try_set_immediate_i32(i, 0).is_err());
		assert!(instr.try_set_immediate_i64(i, 0).is_err());
		assert!(instr.try_set_immediate_u32(i, 0).is_err());
		assert!(instr.try_set_immediate_u64(i, 0).is_err());
	}
	assert!(instr.try_set_immediate_i32(IcedConstants::MAX_OP_COUNT as u32, 0).is_err());
	assert!(instr.try_set_immediate_i64(IcedConstants::MAX_OP_COUNT as u32, 0).is_err());
	assert!(instr.try_set_immediate_u32(IcedConstants::MAX_OP_COUNT as u32, 0).is_err());
	assert!(instr.try_set_immediate_u64(IcedConstants::MAX_OP_COUNT as u32, 0).is_err());
}

#[test]
fn try_get_set_immediate_fails_if_invalid_input() {
	let mut instr = Instruction::with2(Code::Adc_EAX_imm32, Register::EAX, u32::MAX).unwrap();

	assert!(instr.try_immediate(0).is_err());
	let _ = instr.try_immediate(1).unwrap();
	for i in 2..IcedConstants::MAX_OP_COUNT as u32 {
		if i == 4 && instr.op4_kind() == OpKind::Immediate8 {
			continue;
		}
		assert!(instr.try_immediate(i).is_err());
	}
	assert!(instr.try_immediate(IcedConstants::MAX_OP_COUNT as u32).is_err());

	assert!(instr.try_set_immediate_i32(0, 0).is_err());
	assert!(instr.try_set_immediate_i64(0, 0).is_err());
	assert!(instr.try_set_immediate_u32(0, 0).is_err());
	assert!(instr.try_set_immediate_u64(0, 0).is_err());

	instr.try_set_immediate_i32(1, 0).unwrap();
	instr.try_set_immediate_i64(1, 0).unwrap();
	instr.try_set_immediate_u32(1, 0).unwrap();
	instr.try_set_immediate_u64(1, 0).unwrap();

	for i in 2..IcedConstants::MAX_OP_COUNT as u32 {
		if i == 4 && instr.op4_kind() == OpKind::Immediate8 {
			continue;
		}
		assert!(instr.try_set_immediate_i32(i, 0).is_err());
		assert!(instr.try_set_immediate_i64(i, 0).is_err());
		assert!(instr.try_set_immediate_u32(i, 0).is_err());
		assert!(instr.try_set_immediate_u64(i, 0).is_err());
	}
	assert!(instr.try_set_immediate_i32(IcedConstants::MAX_OP_COUNT as u32, 0).is_err());
	assert!(instr.try_set_immediate_i64(IcedConstants::MAX_OP_COUNT as u32, 0).is_err());
	assert!(instr.try_set_immediate_u32(IcedConstants::MAX_OP_COUNT as u32, 0).is_err());
	assert!(instr.try_set_immediate_u64(IcedConstants::MAX_OP_COUNT as u32, 0).is_err());
}

#[test]
fn get_set_register_panics_if_invalid_input() {
	let mut instr = Instruction::with2(Code::Adc_EAX_imm32, Register::EAX, u32::MAX).unwrap();

	for i in 0..IcedConstants::MAX_OP_COUNT as u32 {
		let _ = instr.op_register(i);
	}
	if cfg!(debug_assertions) {
		assert!(panic::catch_unwind(|| { instr.op_register(IcedConstants::MAX_OP_COUNT as u32) }).is_err());
	} else {
		let _ = instr.op_register(IcedConstants::MAX_OP_COUNT as u32);
	}

	for i in 0..IcedConstants::MAX_OP_COUNT as u32 {
		if i == 4 && instr.op4_kind() == OpKind::Immediate8 {
			continue;
		}
		instr.set_op_register(i, Register::EAX);
	}
	if cfg!(debug_assertions) {
		assert!(panic::catch_unwind(move || { instr.set_op_register(IcedConstants::MAX_OP_COUNT as u32, Register::EAX) }).is_err());
	} else {
		instr.set_op_register(IcedConstants::MAX_OP_COUNT as u32, Register::EAX);
	}
}

#[test]
fn get_set_register_fails_if_invalid_input() {
	let mut instr = Instruction::with2(Code::Adc_EAX_imm32, Register::EAX, u32::MAX).unwrap();

	for i in 0..IcedConstants::MAX_OP_COUNT as u32 {
		let _ = instr.try_op_register(i).unwrap();
	}
	assert!(instr.try_op_register(IcedConstants::MAX_OP_COUNT as u32).is_err());

	for i in 0..IcedConstants::MAX_OP_COUNT as u32 {
		if i == 4 && instr.op4_kind() == OpKind::Immediate8 {
			assert!(instr.try_set_op_register(i, Register::EAX).is_err());
		} else {
			instr.try_set_op_register(i, Register::EAX).unwrap();
		}
	}
	assert!(instr.try_set_op_register(IcedConstants::MAX_OP_COUNT as u32, Register::EAX).is_err());
}

#[test]
fn set_declare_xxx_value_panics_if_invalid_input() {
	{
		let mut instr = Instruction::with_declare_byte(&[0; 1]).unwrap();
		if cfg!(debug_assertions) {
			let mut instr = instr;
			assert!(panic::catch_unwind(move || { instr.set_declare_byte_value_i8(16, 0) }).is_err());
		} else {
			instr.set_declare_byte_value_i8(16, 0);
		}
		if cfg!(debug_assertions) {
			let mut instr = instr;
			assert!(panic::catch_unwind(move || { instr.set_declare_byte_value(16, 0) }).is_err());
		} else {
			instr.set_declare_byte_value(16, 0);
		}
		for i in 0..16 {
			instr.set_declare_byte_value_i8(i, 0);
			instr.set_declare_byte_value(i, 0);
		}
	}
	{
		let mut instr = Instruction::with_declare_word(&[0; 1]).unwrap();
		if cfg!(debug_assertions) {
			let mut instr = instr;
			assert!(panic::catch_unwind(move || { instr.set_declare_word_value_i16(8, 0) }).is_err());
		} else {
			instr.set_declare_word_value_i16(8, 0);
		}
		if cfg!(debug_assertions) {
			let mut instr = instr;
			assert!(panic::catch_unwind(move || { instr.set_declare_word_value(8, 0) }).is_err());
		} else {
			instr.set_declare_word_value(8, 0);
		}
		for i in 0..8 {
			instr.set_declare_word_value_i16(i, 0);
			instr.set_declare_word_value(i, 0);
		}
	}
	{
		let mut instr = Instruction::with_declare_dword(&[0; 1]).unwrap();
		if cfg!(debug_assertions) {
			let mut instr = instr;
			assert!(panic::catch_unwind(move || { instr.set_declare_dword_value_i32(4, 0) }).is_err());
		} else {
			instr.set_declare_dword_value_i32(4, 0);
		}
		if cfg!(debug_assertions) {
			let mut instr = instr;
			assert!(panic::catch_unwind(move || { instr.set_declare_dword_value(4, 0) }).is_err());
		} else {
			instr.set_declare_dword_value(4, 0);
		}
		for i in 0..4 {
			instr.set_declare_dword_value_i32(i, 0);
			instr.set_declare_dword_value(i, 0);
		}
	}
	{
		let mut instr = Instruction::with_declare_qword(&[0; 1]).unwrap();
		if cfg!(debug_assertions) {
			let mut instr = instr;
			assert!(panic::catch_unwind(move || { instr.set_declare_qword_value_i64(2, 0) }).is_err());
		} else {
			instr.set_declare_qword_value_i64(2, 0);
		}
		if cfg!(debug_assertions) {
			let mut instr = instr;
			assert!(panic::catch_unwind(move || { instr.set_declare_qword_value(2, 0) }).is_err());
		} else {
			instr.set_declare_qword_value(2, 0);
		}
		for i in 0..2 {
			instr.set_declare_qword_value_i64(i, 0);
			instr.set_declare_qword_value(i, 0);
		}
	}
}

#[test]
fn set_declare_xxx_value_fails_if_invalid_input() {
	{
		let mut instr = Instruction::with_declare_byte(&[0; 1]).unwrap();
		assert!(instr.try_set_declare_byte_value_i8(16, 0).is_err());
		assert!(instr.try_set_declare_byte_value(16, 0).is_err());
		for i in 0..16 {
			instr.try_set_declare_byte_value_i8(i, 0).unwrap();
			instr.try_set_declare_byte_value(i, 0).unwrap();
		}
	}
	{
		let mut instr = Instruction::with_declare_word(&[0; 1]).unwrap();
		assert!(instr.try_set_declare_word_value_i16(8, 0).is_err());
		assert!(instr.try_set_declare_word_value(8, 0).is_err());
		for i in 0..8 {
			instr.try_set_declare_word_value_i16(i, 0).unwrap();
			instr.try_set_declare_word_value(i, 0).unwrap();
		}
	}
	{
		let mut instr = Instruction::with_declare_dword(&[0; 1]).unwrap();
		assert!(instr.try_set_declare_dword_value_i32(4, 0).is_err());
		assert!(instr.try_set_declare_dword_value(4, 0).is_err());
		for i in 0..4 {
			instr.try_set_declare_dword_value_i32(i, 0).unwrap();
			instr.try_set_declare_dword_value(i, 0).unwrap();
		}
	}
	{
		let mut instr = Instruction::with_declare_qword(&[0; 1]).unwrap();
		assert!(instr.try_set_declare_qword_value_i64(2, 0).is_err());
		assert!(instr.try_set_declare_qword_value(2, 0).is_err());
		for i in 0..2 {
			instr.try_set_declare_qword_value_i64(i, 0).unwrap();
			instr.try_set_declare_qword_value(i, 0).unwrap();
		}
	}
}

#[test]
fn get_declare_xxx_value_panics_if_invalid_input() {
	{
		let instr = Instruction::with_declare_byte(&[0; 1]).unwrap();
		if cfg!(debug_assertions) {
			assert!(panic::catch_unwind(|| { instr.get_declare_byte_value(16) }).is_err());
		} else {
			let _ = instr.get_declare_byte_value(16);
		}
		for i in 0..16 {
			let _ = instr.get_declare_byte_value(i);
		}
	}
	{
		let instr = Instruction::with_declare_word(&[0; 1]).unwrap();
		if cfg!(debug_assertions) {
			assert!(panic::catch_unwind(|| { instr.get_declare_word_value(8) }).is_err());
		} else {
			let _ = instr.get_declare_word_value(8);
		}
		for i in 0..8 {
			let _ = instr.get_declare_word_value(i);
		}
	}
	{
		let instr = Instruction::with_declare_dword(&[0; 1]).unwrap();
		if cfg!(debug_assertions) {
			assert!(panic::catch_unwind(|| { instr.get_declare_dword_value(4) }).is_err());
		} else {
			let _ = instr.get_declare_dword_value(4);
		}
		for i in 0..4 {
			let _ = instr.get_declare_dword_value(i);
		}
	}
	{
		let instr = Instruction::with_declare_qword(&[0; 1]).unwrap();
		if cfg!(debug_assertions) {
			assert!(panic::catch_unwind(|| { instr.get_declare_qword_value(2) }).is_err());
		} else {
			let _ = instr.get_declare_qword_value(2);
		}
		for i in 0..2 {
			let _ = instr.get_declare_qword_value(i);
		}
	}
}

#[test]
fn get_declare_xxx_value_fails_if_invalid_input() {
	{
		let instr = Instruction::with_declare_byte(&[0; 1]).unwrap();
		assert!(instr.try_get_declare_byte_value(16).is_err());
		for i in 0..16 {
			let _ = instr.try_get_declare_byte_value(i).unwrap();
		}
	}
	{
		let instr = Instruction::with_declare_word(&[0; 1]).unwrap();
		assert!(instr.try_get_declare_word_value(8).is_err());
		for i in 0..8 {
			let _ = instr.try_get_declare_word_value(i).unwrap();
		}
	}
	{
		let instr = Instruction::with_declare_dword(&[0; 1]).unwrap();
		assert!(instr.try_get_declare_dword_value(4).is_err());
		for i in 0..4 {
			let _ = instr.try_get_declare_dword_value(i).unwrap();
		}
	}
	{
		let instr = Instruction::with_declare_qword(&[0; 1]).unwrap();
		assert!(instr.try_get_declare_qword_value(2).is_err());
		for i in 0..2 {
			let _ = instr.try_get_declare_qword_value(i).unwrap();
		}
	}
}

#[test]
fn encode_invalid_reg_op_size() {
	#[rustfmt::skip]
	let tests: Vec<(u32, Instruction)> = vec![
		(16, Instruction::with2(Code::Movdir64b_r16_m512, Register::CX, MemoryOperand::with_base(Register::EBX)).unwrap()),
		(32, Instruction::with2(Code::Movdir64b_r16_m512, Register::CX, MemoryOperand::with_base(Register::EBX)).unwrap()),

		(16, Instruction::with2(Code::Movdir64b_r32_m512, Register::ECX, MemoryOperand::with_base(Register::BX)).unwrap()),
		(32, Instruction::with2(Code::Movdir64b_r32_m512, Register::ECX, MemoryOperand::with_base(Register::BX)).unwrap()),
		(64, Instruction::with2(Code::Movdir64b_r32_m512, Register::ECX, MemoryOperand::with_base(Register::RBX)).unwrap()),

		(64, Instruction::with2(Code::Movdir64b_r64_m512, Register::RCX, MemoryOperand::with_base(Register::EBX)).unwrap()),

		(16, Instruction::with2(Code::Enqcmds_r16_m512, Register::CX, MemoryOperand::with_base(Register::EBX)).unwrap()),
		(32, Instruction::with2(Code::Enqcmds_r16_m512, Register::CX, MemoryOperand::with_base(Register::EBX)).unwrap()),

		(16, Instruction::with2(Code::Enqcmds_r32_m512, Register::ECX, MemoryOperand::with_base(Register::BX)).unwrap()),
		(32, Instruction::with2(Code::Enqcmds_r32_m512, Register::ECX, MemoryOperand::with_base(Register::BX)).unwrap()),
		(64, Instruction::with2(Code::Enqcmds_r32_m512, Register::ECX, MemoryOperand::with_base(Register::RBX)).unwrap()),

		(64, Instruction::with2(Code::Enqcmds_r64_m512, Register::RCX, MemoryOperand::with_base(Register::EBX)).unwrap()),

		(16, Instruction::with2(Code::Enqcmd_r16_m512, Register::CX, MemoryOperand::with_base(Register::EBX)).unwrap()),
		(32, Instruction::with2(Code::Enqcmd_r16_m512, Register::CX, MemoryOperand::with_base(Register::EBX)).unwrap()),

		(16, Instruction::with2(Code::Enqcmd_r32_m512, Register::ECX, MemoryOperand::with_base(Register::BX)).unwrap()),
		(32, Instruction::with2(Code::Enqcmd_r32_m512, Register::ECX, MemoryOperand::with_base(Register::BX)).unwrap()),
		(64, Instruction::with2(Code::Enqcmd_r32_m512, Register::ECX, MemoryOperand::with_base(Register::RBX)).unwrap()),

		(64, Instruction::with2(Code::Enqcmd_r64_m512, Register::RCX, MemoryOperand::with_base(Register::EBX)).unwrap()),
	];
	for (bitness, instr) in tests {
		let mut encoder = Encoder::new(bitness);
		let error_message = encoder.encode(&instr, 0).expect_err("It should fail to encode an invalid instruction");
		let error_message = format!("{}", error_message);
		assert!(error_message.contains("Register operand size must equal memory addressing mode (16/32/64)"));
	}
}

fn create_fails_if_invalid_bitness_core(tests: Vec<fn(u32) -> Result<Instruction, IcedError>>) {
	for f in tests {
		assert!(f(128).is_err());
	}
}

#[test]
fn create_fails_if_invalid_bitness() {
	#[rustfmt::skip]
	let tests: Vec<fn(u32) -> Result<Instruction, IcedError>> = vec![
		|bitness| Instruction::with_xbegin(bitness, 0x8000_0000_3412_A550),
		|bitness| Instruction::with_outsb(bitness, Register::FS, RepPrefixKind::None),
		|bitness| Instruction::with_outsw(bitness, Register::FS, RepPrefixKind::None),
		|bitness| Instruction::with_outsd(bitness, Register::FS, RepPrefixKind::None),
		|bitness| Instruction::with_scasb(bitness, RepPrefixKind::None),
		|bitness| Instruction::with_scasw(bitness, RepPrefixKind::None),
		|bitness| Instruction::with_scasd(bitness, RepPrefixKind::None),
		|bitness| Instruction::with_scasq(bitness, RepPrefixKind::None),
		|bitness| Instruction::with_lodsb(bitness, Register::FS, RepPrefixKind::None),
		|bitness| Instruction::with_lodsw(bitness, Register::FS, RepPrefixKind::None),
		|bitness| Instruction::with_lodsd(bitness, Register::FS, RepPrefixKind::None),
		|bitness| Instruction::with_lodsq(bitness, Register::FS, RepPrefixKind::None),
		|bitness| Instruction::with_insb(bitness, RepPrefixKind::None),
		|bitness| Instruction::with_insw(bitness, RepPrefixKind::None),
		|bitness| Instruction::with_insd(bitness, RepPrefixKind::None),
		|bitness| Instruction::with_stosb(bitness, RepPrefixKind::None),
		|bitness| Instruction::with_stosw(bitness, RepPrefixKind::None),
		|bitness| Instruction::with_stosd(bitness, RepPrefixKind::None),
		|bitness| Instruction::with_stosq(bitness, RepPrefixKind::None),
		|bitness| Instruction::with_cmpsb(bitness, Register::FS, RepPrefixKind::None),
		|bitness| Instruction::with_cmpsw(bitness, Register::FS, RepPrefixKind::None),
		|bitness| Instruction::with_cmpsd(bitness, Register::FS, RepPrefixKind::None),
		|bitness| Instruction::with_cmpsq(bitness, Register::FS, RepPrefixKind::None),
		|bitness| Instruction::with_movsb(bitness, Register::FS, RepPrefixKind::None),
		|bitness| Instruction::with_movsw(bitness, Register::FS, RepPrefixKind::None),
		|bitness| Instruction::with_movsd(bitness, Register::FS, RepPrefixKind::None),
		|bitness| Instruction::with_movsq(bitness, Register::FS, RepPrefixKind::None),
		|bitness| Instruction::with_maskmovq(bitness, Register::MM2, Register::MM3, Register::FS),
		|bitness| Instruction::with_maskmovdqu(bitness, Register::XMM2, Register::XMM3, Register::FS),
		|bitness| Instruction::with_rep_outsb(bitness),
		|bitness| Instruction::with_rep_outsw(bitness),
		|bitness| Instruction::with_rep_outsd(bitness),
		|bitness| Instruction::with_repe_scasb(bitness),
		|bitness| Instruction::with_repe_scasw(bitness),
		|bitness| Instruction::with_repe_scasd(bitness),
		|bitness| Instruction::with_repe_scasq(bitness),
		|bitness| Instruction::with_repne_scasb(bitness),
		|bitness| Instruction::with_repne_scasw(bitness),
		|bitness| Instruction::with_repne_scasd(bitness),
		|bitness| Instruction::with_repne_scasq(bitness),
		|bitness| Instruction::with_rep_lodsb(bitness),
		|bitness| Instruction::with_rep_lodsw(bitness),
		|bitness| Instruction::with_rep_lodsd(bitness),
		|bitness| Instruction::with_rep_lodsq(bitness),
		|bitness| Instruction::with_rep_insb(bitness),
		|bitness| Instruction::with_rep_insw(bitness),
		|bitness| Instruction::with_rep_insd(bitness),
		|bitness| Instruction::with_rep_stosb(bitness),
		|bitness| Instruction::with_rep_stosw(bitness),
		|bitness| Instruction::with_rep_stosd(bitness),
		|bitness| Instruction::with_rep_stosq(bitness),
		|bitness| Instruction::with_repe_cmpsb(bitness),
		|bitness| Instruction::with_repe_cmpsw(bitness),
		|bitness| Instruction::with_repe_cmpsd(bitness),
		|bitness| Instruction::with_repe_cmpsq(bitness),
		|bitness| Instruction::with_repne_cmpsb(bitness),
		|bitness| Instruction::with_repne_cmpsw(bitness),
		|bitness| Instruction::with_repne_cmpsd(bitness),
		|bitness| Instruction::with_repne_cmpsq(bitness),
		|bitness| Instruction::with_rep_movsb(bitness),
		|bitness| Instruction::with_rep_movsw(bitness),
		|bitness| Instruction::with_rep_movsd(bitness),
		|bitness| Instruction::with_rep_movsq(bitness),
	];
	create_fails_if_invalid_bitness_core(tests);
}

#[test]
#[cfg(not(feature = "no_vex"))]
fn create_fails_if_invalid_bitness_vex() {
	#[rustfmt::skip]
	let tests: Vec<fn(u32) -> Result<Instruction, IcedError>> = vec![
		|bitness| Instruction::with_vmaskmovdqu(bitness, Register::XMM2, Register::XMM3, Register::FS),
	];
	create_fails_if_invalid_bitness_core(tests);
}

#[test]
#[cfg(not(feature = "no_evex"))]
fn encoding_instruction_requiring_opmask_fails_if_no_opmask() {
	let instr = Instruction::with2(
		Code::EVEX_Vpgatherdd_xmm_k1_vm32x,
		Register::XMM1,
		MemoryOperand::with_base_index_scale(Register::RDX, Register::XMM3, 4),
	)
	.unwrap();
	assert!(!instr.has_op_mask());
	let mut encoder = Encoder::new(64);
	let error_message = encoder.encode(&instr, 0).expect_err("It should fail to encode an invalid instruction");
	let error_message = format!("{}", error_message);
	assert_eq!(error_message, "The instruction must use an opmask register");
}

#[test]
fn try_create_imm_works() {
	// OpKind::Immediate8
	for &imm in &[-0x80i32, 0xFF] {
		let instr = Instruction::with2(Code::Add_rm8_imm8, Register::CL, imm).unwrap();
		assert_eq!(instr.immediate8(), imm as u8);
	}
	for &imm in &[-0x81i32, 0x100] {
		assert!(Instruction::with2(Code::Add_rm8_imm8, Register::CL, imm).is_err());
	}
	for &imm in &[-0x80i64, 0xFF] {
		let instr = Instruction::with2(Code::Add_rm8_imm8, Register::CL, imm).unwrap();
		assert_eq!(instr.immediate8(), imm as u8);
	}
	for &imm in &[-0x81i64, 0x100] {
		assert!(Instruction::with2(Code::Add_rm8_imm8, Register::CL, imm).is_err());
	}
	for &imm in &[0u32, 0xFF] {
		let instr = Instruction::with2(Code::Add_rm8_imm8, Register::CL, imm).unwrap();
		assert_eq!(instr.immediate8() as u32, imm);
	}
	for &imm in &[0x100u32, 0xFFFF_FFFF] {
		assert!(Instruction::with2(Code::Add_rm8_imm8, Register::CL, imm).is_err());
	}
	for &imm in &[0u64, 0xFF] {
		let instr = Instruction::with2(Code::Add_rm8_imm8, Register::CL, imm).unwrap();
		assert_eq!(instr.immediate8() as u64, imm);
	}
	for &imm in &[0x100u64, 0xFFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF] {
		assert!(Instruction::with2(Code::Add_rm8_imm8, Register::CL, imm).is_err());
	}

	// OpKind::Immediate8_2nd
	for &imm in &[-0x80i32, 0xFF] {
		let instr = Instruction::with2(Code::Enterq_imm16_imm8, 0, imm).unwrap();
		assert_eq!(instr.immediate8_2nd(), imm as u8);
	}
	for &imm in &[-0x81i32, 0x100] {
		assert!(Instruction::with2(Code::Enterq_imm16_imm8, 0, imm).is_err());
	}
	for &imm in &[0u32, 0xFF] {
		let instr = Instruction::with2(Code::Enterq_imm16_imm8, 0, imm).unwrap();
		assert_eq!(instr.immediate8_2nd() as u32, imm);
	}
	for &imm in &[0x100u32, 0xFFFF_FFFF] {
		assert!(Instruction::with2(Code::Enterq_imm16_imm8, 0, imm).is_err());
	}

	// OpKind::Immediate8to16
	for &imm in &[-0x80i32, 0x7F] {
		let instr = Instruction::with2(Code::Add_rm16_imm8, Register::CX, imm).unwrap();
		assert_eq!(instr.immediate8to16() as i32, imm);
	}
	for &imm in &[-0x81i32, 0x80] {
		assert!(Instruction::with2(Code::Add_rm16_imm8, Register::CX, imm).is_err());
	}
	for &imm in &[-0x80i64, 0x7F] {
		let instr = Instruction::with2(Code::Add_rm16_imm8, Register::CX, imm).unwrap();
		assert_eq!(instr.immediate8to16() as i64, imm);
	}
	for &imm in &[-0x81i64, 0x80] {
		assert!(Instruction::with2(Code::Add_rm16_imm8, Register::CX, imm).is_err());
	}
	for &imm in &[0u32, 0x7F, 0xFF80, 0xFFFF] {
		let instr = Instruction::with2(Code::Add_rm16_imm8, Register::CX, imm).unwrap();
		assert_eq!(instr.immediate8to16() as u16 as u32, imm);
	}
	for &imm in &[0x80u32, 0xFF7F, 0x0001_0000, 0xFFFF_FFFF, 0x0001_FF80, 0x0001_FFFF] {
		assert!(Instruction::with2(Code::Add_rm16_imm8, Register::CX, imm).is_err());
	}
	for &imm in &[0u64, 0x7F, 0xFF80, 0xFFFF] {
		let instr = Instruction::with2(Code::Add_rm16_imm8, Register::CX, imm).unwrap();
		assert_eq!(instr.immediate8to16() as u16 as u64, imm);
	}
	for &imm in &[0x80u64, 0xFF7F, 0x0001_0000, 0xFFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0x0001_FF80, 0x0001_FFFF] {
		assert!(Instruction::with2(Code::Add_rm16_imm8, Register::CX, imm).is_err());
	}

	// OpKind::Immediate8to32
	for &imm in &[-0x80i32, 0x7F] {
		let instr = Instruction::with2(Code::Add_rm32_imm8, Register::ECX, imm).unwrap();
		assert_eq!(instr.immediate8to32(), imm);
	}
	for &imm in &[-0x81i32, 0x80] {
		assert!(Instruction::with2(Code::Add_rm32_imm8, Register::ECX, imm).is_err());
	}
	for &imm in &[-0x80i64, 0x7F] {
		let instr = Instruction::with2(Code::Add_rm32_imm8, Register::ECX, imm).unwrap();
		assert_eq!(instr.immediate8to32() as i64, imm);
	}
	for &imm in &[-0x81i64, 0x80] {
		assert!(Instruction::with2(Code::Add_rm32_imm8, Register::ECX, imm).is_err());
	}
	for &imm in &[0u32, 0x7F, 0xFFFF_FF80, 0xFFFF_FFFF] {
		let instr = Instruction::with2(Code::Add_rm32_imm8, Register::ECX, imm).unwrap();
		assert_eq!(instr.immediate8to32() as u32, imm);
	}
	for &imm in &[0x80u32, 0xFFFF_FF7F] {
		assert!(Instruction::with2(Code::Add_rm32_imm8, Register::ECX, imm).is_err());
	}
	for &imm in &[0u64, 0x7F, 0xFFFF_FF80, 0xFFFF_FFFF] {
		let instr = Instruction::with2(Code::Add_rm32_imm8, Register::ECX, imm).unwrap();
		assert_eq!(instr.immediate8to32() as u32 as u64, imm);
	}
	for &imm in &[0x80u64, 0xFFFF_FF7F, 0x0001_0000_0000, 0xFFFF_FFFF_FFFF_FFFF, 0x0001_FFFF_FF80, 0x0001_FFFF_FFFF] {
		assert!(Instruction::with2(Code::Add_rm32_imm8, Register::ECX, imm).is_err());
	}

	// OpKind::Immediate8to64
	for &imm in &[-0x80i32, 0x7F] {
		let instr = Instruction::with2(Code::Add_rm64_imm8, Register::RCX, imm).unwrap();
		assert_eq!(instr.immediate8to64(), imm as i64);
	}
	for &imm in &[-0x81i32, 0x80] {
		assert!(Instruction::with2(Code::Add_rm64_imm8, Register::RCX, imm).is_err());
	}
	for &imm in &[-0x80i64, 0x7F] {
		let instr = Instruction::with2(Code::Add_rm64_imm8, Register::RCX, imm).unwrap();
		assert_eq!(instr.immediate8to64(), imm);
	}
	for &imm in &[-0x81i64, 0x80] {
		assert!(Instruction::with2(Code::Add_rm64_imm8, Register::RCX, imm).is_err());
	}
	for &imm in &[0u32, 0x7F] {
		let instr = Instruction::with2(Code::Add_rm64_imm8, Register::RCX, imm).unwrap();
		assert_eq!(instr.immediate8to64(), imm as i64);
	}
	for &imm in &[0x80u32, 0xFFFF_FFFF] {
		assert!(Instruction::with2(Code::Add_rm64_imm8, Register::RCX, imm).is_err());
	}
	for &imm in &[0u64, 0x7F, 0xFFFF_FFFF_FFFF_FF80, 0xFFFF_FFFF_FFFF_FFFF] {
		let instr = Instruction::with2(Code::Add_rm64_imm8, Register::RCX, imm).unwrap();
		assert_eq!(instr.immediate8to64() as u64, imm);
	}
	for &imm in &[0x80u64, 0xFFFF_FFFF_FFFF_FF7F] {
		assert!(Instruction::with2(Code::Add_rm64_imm8, Register::RCX, imm).is_err());
	}

	// OpKind::Immediate32to64
	for &imm in &[-0x8000_0000i32, 0x7FFF_FFFF] {
		let instr = Instruction::with2(Code::Add_rm64_imm32, Register::RCX, imm).unwrap();
		assert_eq!(instr.immediate32to64(), imm as i64);
	}
	for &imm in &[-0x8000_0000i64, 0x7FFF_FFFF] {
		let instr = Instruction::with2(Code::Add_rm64_imm32, Register::RCX, imm).unwrap();
		assert_eq!(instr.immediate32to64(), imm);
	}
	for &imm in &[-0x8000_0001i64, 0x8000_0000] {
		assert!(Instruction::with2(Code::Add_rm64_imm32, Register::RCX, imm).is_err());
	}
	for &imm in &[0u32, 0x7FFF_FFFF] {
		let instr = Instruction::with2(Code::Add_rm64_imm32, Register::RCX, imm).unwrap();
		assert_eq!(instr.immediate32to64(), imm as i64);
	}
	for &imm in &[0x8000_0000u32, 0xFFFF_FFFF] {
		assert!(Instruction::with2(Code::Add_rm64_imm32, Register::RCX, imm).is_err());
	}
	for &imm in &[0u64, 0x7FFF_FFFF, 0xFFFF_FFFF_8000_0000, 0xFFFF_FFFF_FFFF_FFFF] {
		let instr = Instruction::with2(Code::Add_rm64_imm32, Register::RCX, imm).unwrap();
		assert_eq!(instr.immediate32to64() as u64, imm);
	}
	for &imm in &[0x8000_0000u64, 0x0001_0000_0000, 0xFFFF_FFFF_7FFF_FFFF] {
		assert!(Instruction::with2(Code::Add_rm64_imm32, Register::RCX, imm).is_err());
	}

	// OpKind::Immediate16
	for &imm in &[-0x8000i32, 0xFFFF] {
		let instr = Instruction::with2(Code::Add_rm16_imm16, Register::CX, imm).unwrap();
		assert_eq!(instr.immediate16(), imm as u16);
	}
	for &imm in &[-0x8001i32, 0x0001_0000] {
		assert!(Instruction::with2(Code::Add_rm16_imm16, Register::CX, imm).is_err());
	}
	for &imm in &[-0x8000i64, 0xFFFF] {
		let instr = Instruction::with2(Code::Add_rm16_imm16, Register::CX, imm).unwrap();
		assert_eq!(instr.immediate16(), imm as u16);
	}
	for &imm in &[-0x8001i64, 0x0001_0000] {
		assert!(Instruction::with2(Code::Add_rm16_imm16, Register::CX, imm).is_err());
	}
	for &imm in &[0u32, 0xFFFF] {
		let instr = Instruction::with2(Code::Add_rm16_imm16, Register::CX, imm).unwrap();
		assert_eq!(instr.immediate16() as u32, imm);
	}
	for &imm in &[0x0001_0000u32, 0xFFFF_FFFF] {
		assert!(Instruction::with2(Code::Add_rm16_imm16, Register::CX, imm).is_err());
	}
	for &imm in &[0u64, 0xFFFF] {
		let instr = Instruction::with2(Code::Add_rm16_imm16, Register::CX, imm).unwrap();
		assert_eq!(instr.immediate16() as u64, imm);
	}
	for &imm in &[0x0001_0000u64, 0xFFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF] {
		assert!(Instruction::with2(Code::Add_rm16_imm16, Register::CX, imm).is_err());
	}

	// OpKind::Immediate32
	for &imm in &[-0x8000_0000i32, 0x7FFF_FFFF] {
		let instr = Instruction::with2(Code::Add_rm32_imm32, Register::ECX, imm).unwrap();
		assert_eq!(instr.immediate32(), imm as u32);
	}
	for &imm in &[-0x8000_0000i64, 0xFFFF_FFFF] {
		let instr = Instruction::with2(Code::Add_rm32_imm32, Register::ECX, imm).unwrap();
		assert_eq!(instr.immediate32(), imm as u32);
	}
	for &imm in &[-0x8000_0001i64, 0x0001_0000_0000] {
		assert!(Instruction::with2(Code::Add_rm32_imm32, Register::ECX, imm).is_err());
	}
	for &imm in &[0u32, 0xFFFF_FFFF] {
		let instr = Instruction::with2(Code::Add_rm32_imm32, Register::ECX, imm).unwrap();
		assert_eq!(instr.immediate32(), imm);
	}
	for &imm in &[0u64, 0xFFFF_FFFF] {
		let instr = Instruction::with2(Code::Add_rm32_imm32, Register::ECX, imm).unwrap();
		assert_eq!(instr.immediate32() as u64, imm);
	}
	for &imm in &[0x0001_0000_0000u64, 0xFFFF_FFFF_FFFF_FFFF] {
		assert!(Instruction::with2(Code::Add_rm32_imm32, Register::ECX, imm).is_err());
	}

	// OpKind::Immediate64
	for &imm in &[i32::MIN, i32::MAX] {
		let instr = Instruction::with2(Code::Mov_r64_imm64, Register::RCX, imm).unwrap();
		assert_eq!(instr.immediate64(), imm as u64);
	}
	for &imm in &[i64::MIN, i64::MAX] {
		let instr = Instruction::with2(Code::Mov_r64_imm64, Register::RCX, imm).unwrap();
		assert_eq!(instr.immediate64(), imm as u64);
	}
	for &imm in &[u32::MIN, u32::MAX] {
		let instr = Instruction::with2(Code::Mov_r64_imm64, Register::RCX, imm).unwrap();
		assert_eq!(instr.immediate64(), imm as u64);
	}
	for &imm in &[u64::MIN, u64::MAX] {
		let instr = Instruction::with2(Code::Mov_r64_imm64, Register::RCX, imm).unwrap();
		assert_eq!(instr.immediate64(), imm);
	}
}

#[test]
fn encode_invalid_len_dw_dd_dq() {
	let mut encoder = Encoder::new(64);

	let mut dw = Instruction::try_with_declare_word_1(1).unwrap();
	dw.set_declare_data_len(8);
	assert_eq!(encoder.encode(&dw, 0).unwrap(), 16);
	dw.set_declare_data_len(8 + 1);
	assert!(encoder.encode(&dw, 0).is_err());

	let mut dd = Instruction::try_with_declare_dword_1(1).unwrap();
	dd.set_declare_data_len(4);
	assert_eq!(encoder.encode(&dd, 0).unwrap(), 16);
	dd.set_declare_data_len(4 + 1);
	assert!(encoder.encode(&dd, 0).is_err());

	let mut dq = Instruction::try_with_declare_qword_1(1).unwrap();
	dq.set_declare_data_len(2);
	assert_eq!(encoder.encode(&dq, 0).unwrap(), 16);
	dq.set_declare_data_len(2 + 1);
	assert!(encoder.encode(&dq, 0).is_err());
}
