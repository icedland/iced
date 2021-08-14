// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::code_asm::*;
use crate::{Code, Decoder, DecoderOptions, Register};
use core::convert::TryInto;

#[test]
fn create() {
	for &bitness in &[16, 32, 64] {
		let mut a = CodeAssembler::new(bitness).unwrap();
		assert_eq!(a.bitness(), bitness);
		assert_eq!(a.instructions().len(), 0);
		assert!(a.prefer_vex());
		assert!(a.prefer_short_branch());
		a.set_prefer_vex(false);
		assert!(!a.prefer_vex());
		assert!(a.prefer_short_branch());
		a.set_prefer_short_branch(false);
		assert!(!a.prefer_vex());
		assert!(!a.prefer_short_branch());
	}
}

#[test]
fn create_invalid_bitness() {
	assert!(CodeAssembler::new(1).is_err());
}

#[test]
fn assemble_no_instrs() {
	let mut a = CodeAssembler::new(64).unwrap();
	let bytes = a.assemble(0x1234_5678_9ABC_DEF0).unwrap();
	assert!(bytes.is_empty());
}

#[test]
fn assemble_keeps_instrs() {
	let mut a = CodeAssembler::new(64).unwrap();
	a.int3().unwrap();
	a.nop().unwrap();
	let bytes = a.assemble(0x1234_5678_9ABC_DEF0).unwrap();
	assert_eq!(bytes, b"\xCC\x90");
	assert_eq!(a.instructions().len(), 2);
	a.rdtsc().unwrap();
	let bytes = a.assemble(0x1234_5678_9ABC_DEF0).unwrap();
	assert_eq!(bytes, b"\xCC\x90\x0F\x31");
}

#[test]
fn test_prefixes() {
	let mut a = CodeAssembler::new(64).unwrap();
	a.xacquire().lock().add(byte_ptr(rcx), dl).unwrap();
	a.xrelease().lock().add(byte_ptr(rcx), dl).unwrap();
	a.lock().add(byte_ptr(rcx), dl).unwrap();
	a.rep().stosb().unwrap();
	a.repe().cmpsb().unwrap();
	a.repne().cmpsb().unwrap();
	a.bnd().call(rcx).unwrap();
	a.notrack().call(rcx).unwrap();
	let bytes = a.assemble(0x1234_5678_9ABC_DEF0).unwrap();
	assert_eq!(bytes, b"\xF0\xF2\x00\x11\xF0\xF3\x00\x11\xF0\x00\x11\xF3\xAA\xF3\xA6\xF2\xA6\xF2\xFF\xD1\x3E\xFF\xD1");
}

#[test]
fn test_prefixes_dup() {
	let mut a = CodeAssembler::new(64).unwrap();
	a.xacquire().xacquire().lock().lock().add(byte_ptr(rcx), dl).unwrap();
	a.xrelease().xrelease().lock().lock().add(byte_ptr(rcx), dl).unwrap();
	a.lock().lock().add(byte_ptr(rcx), dl).unwrap();
	a.rep().rep().stosb().unwrap();
	a.repe().repe().cmpsb().unwrap();
	a.repne().repne().cmpsb().unwrap();
	a.bnd().bnd().call(rcx).unwrap();
	a.notrack().notrack().call(rcx).unwrap();
	let bytes = a.assemble(0x1234_5678_9ABC_DEF0).unwrap();
	assert_eq!(bytes, b"\xF0\xF2\x00\x11\xF0\xF3\x00\x11\xF0\x00\x11\xF3\xAA\xF3\xA6\xF2\xA6\xF2\xFF\xD1\x3E\xFF\xD1");
}

#[test]
fn prefixes_without_instr_fails_assemble() {
	#[rustfmt::skip]
	let set_prefixes: &[fn(&mut CodeAssembler) -> &mut CodeAssembler] = &[
		|a| a.xacquire(),
		|a| a.xrelease(),
		|a| a.lock(),
		|a| a.rep(),
		|a| a.repe(),
		|a| a.repne(),
		|a| a.bnd(),
		|a| a.notrack(),
	];
	for set_prefix in set_prefixes {
		let mut a = CodeAssembler::new(64).unwrap();
		let _ = set_prefix(&mut a);
		assert!(a.assemble(0x1234_5678_9ABC_DEF0).is_err());
	}
}

#[test]
fn test_instrs_method() {
	let mut a = CodeAssembler::new(64).unwrap();
	assert_eq!(a.instructions, &[]);
	a.rdtsc().unwrap();
	assert_eq!(a.instructions, &[Instruction::with(Code::Rdtsc)]);
	a.xor(ecx, 0x1234_5678).unwrap();
	assert_eq!(a.instructions, &[Instruction::with(Code::Rdtsc), Instruction::with2(Code::Xor_rm32_imm32, Register::ECX, 0x1234_5678).unwrap()]);
}

#[test]
fn test_take_instrs() {
	let mut a = CodeAssembler::new(64).unwrap();
	let instrs = a.take_instructions();
	assert_eq!(instrs, &[]);
	a.rdtsc().unwrap();
	a.xor(ecx, 0x1234_5678).unwrap();
	let instrs = a.take_instructions();
	assert_eq!(instrs, &[Instruction::with(Code::Rdtsc), Instruction::with2(Code::Xor_rm32_imm32, Register::ECX, 0x1234_5678).unwrap()]);
	assert!(a.instructions().is_empty());
}

#[test]
fn test_take_instrs_calls_reset() {
	let mut a = CodeAssembler::new(64).unwrap();
	let _: &mut CodeAssembler = a.lock();
	let instrs = a.take_instructions();
	assert!(instrs.is_empty());
	let bytes = a.assemble(0x1234_5678_9ABC_DEF0).unwrap();
	assert_eq!(bytes, b"");
}

#[test]
fn test_reset_clears_instrs() {
	let mut a = CodeAssembler::new(64).unwrap();
	a.reset();
	assert!(a.instructions().is_empty());
	a.nop().unwrap();
	a.int3().unwrap();
	assert_eq!(a.instructions().len(), 2);
	a.reset();
	assert!(a.instructions().is_empty());
}

#[test]
fn test_reset_clears_flags() {
	let mut a = CodeAssembler::new(64).unwrap();
	let _: &mut CodeAssembler = a.lock();
	a.reset();
	assert!(a.instructions().is_empty());
	let bytes = a.assemble(0x1234_5678_9ABC_DEF0).unwrap();
	assert_eq!(bytes, b"");
}

#[test]
fn test_labels() {
	let mut a = CodeAssembler::new(64).unwrap();
	let _unused_label1 = a.create_label();
	let mut label1 = a.create_label();
	let mut label2 = a.create_label();
	let mut label3 = a.create_label();
	let _unused_label2 = a.create_label();

	a.set_current_label(&mut label1).unwrap();
	a.nop().unwrap();
	a.set_current_label(&mut label2).unwrap();
	a.int3().unwrap();
	a.je(label1).unwrap();
	a.jb(label2).unwrap();
	a.jo(label2).unwrap();
	a.jne(label3).unwrap();
	a.rdtsc().unwrap();
	a.set_current_label(&mut label3).unwrap();
	a.nop().unwrap();

	let bytes = a.assemble(0x1234_5678_9ABC_DEF0).unwrap();
	assert_eq!(bytes, b"\x90\xCC\x74\xFC\x72\xFB\x70\xF9\x75\x02\x0F\x31\x90");
}

#[test]
fn test_set_current_label_errors() {
	{
		let mut a = CodeAssembler::new(64).unwrap();
		let mut default_label = CodeLabel::default();
		assert!(a.set_current_label(&mut default_label).is_err());
	}
	{
		let mut a = CodeAssembler::new(64).unwrap();
		let mut label1 = a.create_label();
		a.set_current_label(&mut label1).unwrap();
		a.int3().unwrap();
		assert!(a.set_current_label(&mut label1).is_err());
	}
	{
		let mut a = CodeAssembler::new(64).unwrap();
		let mut label1 = a.create_label();
		let mut label2 = a.create_label();
		a.set_current_label(&mut label1).unwrap();
		assert!(a.set_current_label(&mut label2).is_err());
	}
}

#[test]
fn test_label_at_eof() {
	{
		let mut a = CodeAssembler::new(64).unwrap();
		let mut label1 = a.create_label();
		a.set_current_label(&mut label1).unwrap();
		assert!(a.assemble(0x1234_5678_9ABC_DEF0).is_err());
	}
	{
		let mut a = CodeAssembler::new(64).unwrap();
		a.int3().unwrap();
		let mut label1 = a.create_label();
		a.set_current_label(&mut label1).unwrap();
		assert!(a.assemble(0x1234_5678_9ABC_DEF0).is_err());
	}
	{
		let mut a = CodeAssembler::new(64).unwrap();
		a.anonymous_label().unwrap();
		assert!(a.assemble(0x1234_5678_9ABC_DEF0).is_err());
	}
	{
		let mut a = CodeAssembler::new(64).unwrap();
		let _ = a.fwd();
		assert!(a.assemble(0x1234_5678_9ABC_DEF0).is_err());
	}
}

#[test]
fn test_anon_labels() {
	let mut a = CodeAssembler::new(64).unwrap();
	a.push(rcx).unwrap();
	a.anonymous_label().unwrap();
	a.xor(rcx, rdx).unwrap();
	let anon = a.bwd().unwrap();
	a.je(anon).unwrap();
	let anon = a.fwd();
	a.js(anon).unwrap();
	a.nop().unwrap();
	a.anonymous_label().unwrap();
	a.sub(eax, eax).unwrap();
	let bytes = a.assemble(0x1234_5678_9ABC_DEF0).unwrap();
	assert_eq!(bytes, b"\x51\x48\x31\xD1\x74\xFB\x78\x01\x90\x29\xC0");
}

#[test]
fn test_mult_anon_labels_same_instr_fails() {
	let mut a = CodeAssembler::new(64).unwrap();
	a.anonymous_label().unwrap();
	assert!(a.anonymous_label().is_err());
}

#[test]
fn test_bwd_fails_if_no_anon_label() {
	let mut a = CodeAssembler::new(64).unwrap();
	assert!(a.bwd().is_err());
}

#[test]
fn test_normal_and_anon_label_error() {
	let mut a = CodeAssembler::new(64).unwrap();
	let mut label = a.create_label();
	a.nop().unwrap();
	let anon = a.fwd();
	a.je(anon).unwrap();
	a.set_current_label(&mut label).unwrap();
	a.anonymous_label().unwrap();
	assert!(a.nop().is_err());
}

const DB_TEST_DATA: &[u8] = b"\x87\xEE\x07\x18\x52\xF8\x1D\x6A\xBE\x81\x17\x03\x5E\x2F\x71\x73\
							  \xBD\x70\xDB\x8C\x97\xAB\x23\x32\xB2\xC0\x27\xAE\xB2\x25\x31\x64";

#[test]
fn test_db() {
	for len in 0..=DB_TEST_DATA.len() {
		let mut a = CodeAssembler::new(64).unwrap();
		a.db(&DB_TEST_DATA[0..len]).unwrap();
		let bytes = a.assemble(0x1234_5678_9ABC_DEF0).unwrap();
		assert_eq!(bytes, &DB_TEST_DATA[0..len]);
	}
}

#[test]
fn test_db_i() {
	let mut data = [0; DB_TEST_DATA.len()];
	for (t, b) in data.iter_mut().zip(DB_TEST_DATA.iter()) {
		*t = *b as i8;
	}
	let mut cmp_result = [0; DB_TEST_DATA.len()];
	for len in 0..=data.len() {
		let mut a = CodeAssembler::new(64).unwrap();
		a.db_i(&data[0..len]).unwrap();
		let bytes = a.assemble(0x1234_5678_9ABC_DEF0).unwrap();
		for (t, d) in cmp_result.iter_mut().zip(data[0..len].iter()) {
			*t = *d as u8;
		}
		assert_eq!(bytes, &cmp_result[0..len]);
	}
}

#[test]
fn test_dw() {
	const ELEM_SIZE: usize = 2;
	let mut tmp = [0; DB_TEST_DATA.len() / ELEM_SIZE];
	for len in 0..=DB_TEST_DATA.len() / ELEM_SIZE {
		let mut a = CodeAssembler::new(64).unwrap();
		for (t, b) in tmp[0..len].iter_mut().zip(DB_TEST_DATA.chunks(ELEM_SIZE)) {
			*t = u16::from_le_bytes(b.try_into().unwrap());
		}
		a.dw(&tmp[0..len]).unwrap();
		let bytes = a.assemble(0x1234_5678_9ABC_DEF0).unwrap();
		assert_eq!(bytes, &DB_TEST_DATA[0..len * ELEM_SIZE]);
	}
}

#[test]
fn test_dw_i() {
	const ELEM_SIZE: usize = 2;
	let mut tmp = [0; DB_TEST_DATA.len() / ELEM_SIZE];
	for len in 0..=DB_TEST_DATA.len() / ELEM_SIZE {
		let mut a = CodeAssembler::new(64).unwrap();
		for (t, b) in tmp[0..len].iter_mut().zip(DB_TEST_DATA.chunks(ELEM_SIZE)) {
			*t = i16::from_le_bytes(b.try_into().unwrap());
		}
		a.dw_i(&tmp[0..len]).unwrap();
		let bytes = a.assemble(0x1234_5678_9ABC_DEF0).unwrap();
		assert_eq!(bytes, &DB_TEST_DATA[0..len * ELEM_SIZE]);
	}
}

#[test]
fn test_dd() {
	const ELEM_SIZE: usize = 4;
	let mut tmp = [0; DB_TEST_DATA.len() / ELEM_SIZE];
	for len in 0..=DB_TEST_DATA.len() / ELEM_SIZE {
		let mut a = CodeAssembler::new(64).unwrap();
		for (t, b) in tmp[0..len].iter_mut().zip(DB_TEST_DATA.chunks(ELEM_SIZE)) {
			*t = u32::from_le_bytes(b.try_into().unwrap());
		}
		a.dd(&tmp[0..len]).unwrap();
		let bytes = a.assemble(0x1234_5678_9ABC_DEF0).unwrap();
		assert_eq!(bytes, &DB_TEST_DATA[0..len * ELEM_SIZE]);
	}
}

#[test]
fn test_dd_i() {
	const ELEM_SIZE: usize = 4;
	let mut tmp = [0; DB_TEST_DATA.len() / ELEM_SIZE];
	for len in 0..=DB_TEST_DATA.len() / ELEM_SIZE {
		let mut a = CodeAssembler::new(64).unwrap();
		for (t, b) in tmp[0..len].iter_mut().zip(DB_TEST_DATA.chunks(ELEM_SIZE)) {
			*t = i32::from_le_bytes(b.try_into().unwrap());
		}
		a.dd_i(&tmp[0..len]).unwrap();
		let bytes = a.assemble(0x1234_5678_9ABC_DEF0).unwrap();
		assert_eq!(bytes, &DB_TEST_DATA[0..len * ELEM_SIZE]);
	}
}

#[test]
fn test_dd_f32() {
	const ELEM_SIZE: usize = 4;
	let mut tmp = [0.; DB_TEST_DATA.len() / ELEM_SIZE];
	for len in 0..=DB_TEST_DATA.len() / ELEM_SIZE {
		let mut a = CodeAssembler::new(64).unwrap();
		for (t, b) in tmp[0..len].iter_mut().zip(DB_TEST_DATA.chunks(ELEM_SIZE)) {
			*t = f32::from_bits(u32::from_le_bytes(b.try_into().unwrap()));
		}
		a.dd_f32(&tmp[0..len]).unwrap();
		let bytes = a.assemble(0x1234_5678_9ABC_DEF0).unwrap();
		assert_eq!(bytes, &DB_TEST_DATA[0..len * ELEM_SIZE]);
	}
}

#[test]
fn test_dq() {
	const ELEM_SIZE: usize = 8;
	let mut tmp = [0; DB_TEST_DATA.len() / ELEM_SIZE];
	for len in 0..=DB_TEST_DATA.len() / ELEM_SIZE {
		let mut a = CodeAssembler::new(64).unwrap();
		for (t, b) in tmp[0..len].iter_mut().zip(DB_TEST_DATA.chunks(ELEM_SIZE)) {
			*t = u64::from_le_bytes(b.try_into().unwrap());
		}
		a.dq(&tmp[0..len]).unwrap();
		let bytes = a.assemble(0x1234_5678_9ABC_DEF0).unwrap();
		assert_eq!(bytes, &DB_TEST_DATA[0..len * ELEM_SIZE]);
	}
}

#[test]
fn test_dq_i() {
	const ELEM_SIZE: usize = 8;
	let mut tmp = [0; DB_TEST_DATA.len() / ELEM_SIZE];
	for len in 0..=DB_TEST_DATA.len() / ELEM_SIZE {
		let mut a = CodeAssembler::new(64).unwrap();
		for (t, b) in tmp[0..len].iter_mut().zip(DB_TEST_DATA.chunks(ELEM_SIZE)) {
			*t = i64::from_le_bytes(b.try_into().unwrap());
		}
		a.dq_i(&tmp[0..len]).unwrap();
		let bytes = a.assemble(0x1234_5678_9ABC_DEF0).unwrap();
		assert_eq!(bytes, &DB_TEST_DATA[0..len * ELEM_SIZE]);
	}
}

#[test]
fn test_dq_f64() {
	const ELEM_SIZE: usize = 8;
	let mut tmp = [0.; DB_TEST_DATA.len() / ELEM_SIZE];
	for len in 0..=DB_TEST_DATA.len() / ELEM_SIZE {
		let mut a = CodeAssembler::new(64).unwrap();
		for (t, b) in tmp[0..len].iter_mut().zip(DB_TEST_DATA.chunks(ELEM_SIZE)) {
			*t = f64::from_bits(u64::from_le_bytes(b.try_into().unwrap()));
		}
		a.dq_f64(&tmp[0..len]).unwrap();
		let bytes = a.assemble(0x1234_5678_9ABC_DEF0).unwrap();
		assert_eq!(bytes, &DB_TEST_DATA[0..len * ELEM_SIZE]);
	}
}

#[test]
fn test_db_dw_dd_dq_errors() {
	let mut a = CodeAssembler::new(64).unwrap();
	assert!(a.xrelease().db(&[]).is_err());
	let mut a = CodeAssembler::new(64).unwrap();
	assert!(a.xacquire().db(&[0]).is_err());

	let mut a = CodeAssembler::new(64).unwrap();
	assert!(a.lock().db_i(&[]).is_err());
	let mut a = CodeAssembler::new(64).unwrap();
	assert!(a.rep().db_i(&[0]).is_err());

	let mut a = CodeAssembler::new(64).unwrap();
	assert!(a.repe().dw(&[]).is_err());
	let mut a = CodeAssembler::new(64).unwrap();
	assert!(a.repne().dw(&[0]).is_err());

	let mut a = CodeAssembler::new(64).unwrap();
	assert!(a.bnd().dw_i(&[]).is_err());
	let mut a = CodeAssembler::new(64).unwrap();
	assert!(a.notrack().dw_i(&[0]).is_err());

	let mut a = CodeAssembler::new(64).unwrap();
	assert!(a.xacquire().dd(&[]).is_err());
	let mut a = CodeAssembler::new(64).unwrap();
	assert!(a.lock().dd(&[0]).is_err());

	let mut a = CodeAssembler::new(64).unwrap();
	assert!(a.rep().dd_i(&[]).is_err());
	let mut a = CodeAssembler::new(64).unwrap();
	assert!(a.repe().dd_i(&[0]).is_err());

	let mut a = CodeAssembler::new(64).unwrap();
	assert!(a.repne().dd_f32(&[]).is_err());
	let mut a = CodeAssembler::new(64).unwrap();
	assert!(a.bnd().dd_f32(&[0.]).is_err());

	let mut a = CodeAssembler::new(64).unwrap();
	assert!(a.notrack().dq(&[]).is_err());
	let mut a = CodeAssembler::new(64).unwrap();
	assert!(a.xrelease().dq(&[0]).is_err());

	let mut a = CodeAssembler::new(64).unwrap();
	assert!(a.lock().dq_i(&[]).is_err());
	let mut a = CodeAssembler::new(64).unwrap();
	assert!(a.rep().dq_i(&[0]).is_err());

	let mut a = CodeAssembler::new(64).unwrap();
	assert!(a.repne().dq_f64(&[]).is_err());
	let mut a = CodeAssembler::new(64).unwrap();
	assert!(a.notrack().dq_f64(&[0.]).is_err());
}

#[test]
fn nops() {
	for &bitness in &[16, 32, 64] {
		for size in 0..=128 {
			let mut a = CodeAssembler::new(bitness).unwrap();
			a.nops_with_size(size).unwrap();
			let bytes = a.assemble(0x1234_5678_9ABC_DEF0).unwrap();
			assert_eq!(bytes.len(), size);
			for instr in &mut Decoder::new(bitness, &bytes, DecoderOptions::NONE) {
				match instr.code() {
					Code::Nopw | Code::Nopd | Code::Nopq | Code::Nop_rm16 | Code::Nop_rm32 | Code::Nop_rm64 => {}
					_ => panic!("Unexpected NOP instruction: {:?}", instr.code()),
				}
			}
		}
	}
}

#[test]
fn nops_errors() {
	let mut a = CodeAssembler::new(64).unwrap();
	assert!(a.xacquire().nops_with_size(0).is_err());
	let mut a = CodeAssembler::new(64).unwrap();
	assert!(a.xrelease().nops_with_size(1).is_err());
	let mut a = CodeAssembler::new(64).unwrap();
	assert!(a.lock().nops_with_size(2).is_err());
	let mut a = CodeAssembler::new(64).unwrap();
	assert!(a.rep().nops_with_size(10).is_err());
	let mut a = CodeAssembler::new(64).unwrap();
	assert!(a.repe().nops_with_size(20).is_err());
	let mut a = CodeAssembler::new(64).unwrap();
	assert!(a.repne().nops_with_size(30).is_err());
	let mut a = CodeAssembler::new(64).unwrap();
	assert!(a.bnd().nops_with_size(100).is_err());
	let mut a = CodeAssembler::new(64).unwrap();
	assert!(a.notrack().nops_with_size(2000).is_err());
}
