// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

mod instr16;
mod instr32;
mod instr64;

use crate::code_asm::*;
use crate::{BlockEncoderOptions, Code, Decoder, DecoderOptions, MemoryOperand, Register};
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
	a.repz().cmpsb().unwrap();
	a.repne().cmpsb().unwrap();
	a.repnz().cmpsb().unwrap();
	a.bnd().call(rcx).unwrap();
	a.notrack().call(rcx).unwrap();
	let bytes = a.assemble(0x1234_5678_9ABC_DEF0).unwrap();
	assert_eq!(bytes, b"\xF0\xF2\x00\x11\xF0\xF3\x00\x11\xF0\x00\x11\xF3\xAA\xF3\xA6\xF3\xA6\xF2\xA6\xF2\xA6\xF2\xFF\xD1\x3E\xFF\xD1");
}

#[test]
fn test_prefixes_dup() {
	let mut a = CodeAssembler::new(64).unwrap();
	a.xacquire().xacquire().lock().lock().add(byte_ptr(rcx), dl).unwrap();
	a.xrelease().xrelease().lock().lock().add(byte_ptr(rcx), dl).unwrap();
	a.lock().lock().add(byte_ptr(rcx), dl).unwrap();
	a.rep().rep().stosb().unwrap();
	a.repe().repe().cmpsb().unwrap();
	a.repz().repz().cmpsb().unwrap();
	a.repne().repne().cmpsb().unwrap();
	a.repnz().repnz().cmpsb().unwrap();
	a.bnd().bnd().call(rcx).unwrap();
	a.notrack().notrack().call(rcx).unwrap();
	let bytes = a.assemble(0x1234_5678_9ABC_DEF0).unwrap();
	assert_eq!(bytes, b"\xF0\xF2\x00\x11\xF0\xF3\x00\x11\xF0\x00\x11\xF3\xAA\xF3\xA6\xF3\xA6\xF2\xA6\xF2\xA6\xF2\xFF\xD1\x3E\xFF\xD1");
}

#[test]
#[allow(clippy::redundant_closure_for_method_calls)]
fn prefixes_without_instr_fails_assemble() {
	#[rustfmt::skip]
	let set_prefixes: &[fn(&mut CodeAssembler) -> &mut CodeAssembler] = &[
		|a| a.xacquire(),
		|a| a.xrelease(),
		|a| a.lock(),
		|a| a.rep(),
		|a| a.repe(),
		|a| a.repz(),
		|a| a.repne(),
		|a| a.repnz(),
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

	a.set_label(&mut label1).unwrap();
	a.nop().unwrap();
	a.set_label(&mut label2).unwrap();
	a.int3().unwrap();
	a.je(label1).unwrap();
	a.jb(label2).unwrap();
	a.jo(label2).unwrap();
	a.jne(label3).unwrap();
	a.rdtsc().unwrap();
	a.set_label(&mut label3).unwrap();
	a.nop().unwrap();

	let bytes = a.assemble(0x1234_5678_9ABC_DEF0).unwrap();
	assert_eq!(bytes, b"\x90\xCC\x74\xFC\x72\xFB\x70\xF9\x75\x02\x0F\x31\x90");
}

#[test]
fn test_set_label_errors() {
	{
		let mut a = CodeAssembler::new(64).unwrap();
		let mut default_label = CodeLabel::default();
		assert!(a.set_label(&mut default_label).is_err());
	}
	{
		let mut a = CodeAssembler::new(64).unwrap();
		let mut label1 = a.create_label();
		a.set_label(&mut label1).unwrap();
		a.int3().unwrap();
		assert!(a.set_label(&mut label1).is_err());
	}
	{
		let mut a = CodeAssembler::new(64).unwrap();
		let mut label1 = a.create_label();
		let mut label2 = a.create_label();
		a.set_label(&mut label1).unwrap();
		assert!(a.set_label(&mut label2).is_err());
	}
}

#[test]
fn test_label_at_eof() {
	{
		let mut a = CodeAssembler::new(64).unwrap();
		let mut label1 = a.create_label();
		a.set_label(&mut label1).unwrap();
		assert!(a.assemble(0x1234_5678_9ABC_DEF0).is_err());
	}
	{
		let mut a = CodeAssembler::new(64).unwrap();
		a.int3().unwrap();
		let mut label1 = a.create_label();
		a.set_label(&mut label1).unwrap();
		assert!(a.assemble(0x1234_5678_9ABC_DEF0).is_err());
	}
	{
		let mut a = CodeAssembler::new(64).unwrap();
		a.anonymous_label().unwrap();
		assert!(a.assemble(0x1234_5678_9ABC_DEF0).is_err());
	}
	{
		let mut a = CodeAssembler::new(64).unwrap();
		let _ = a.fwd().unwrap();
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
	let anon = a.fwd().unwrap();
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
	let anon = a.fwd().unwrap();
	a.je(anon).unwrap();
	a.set_label(&mut label).unwrap();
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
	assert!(a.repz().dq(&[0]).is_err());

	let mut a = CodeAssembler::new(64).unwrap();
	assert!(a.repnz().dq_i(&[]).is_err());
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
	assert!(a.repz().nops_with_size(20).is_err());
	let mut a = CodeAssembler::new(64).unwrap();
	assert!(a.repnz().nops_with_size(30).is_err());
	let mut a = CodeAssembler::new(64).unwrap();
	assert!(a.bnd().nops_with_size(100).is_err());
	let mut a = CodeAssembler::new(64).unwrap();
	assert!(a.notrack().nops_with_size(2000).is_err());
}

#[test]
fn invalid_instr_fails() {
	let mut a = CodeAssembler::new(64).unwrap();
	a.aaa().unwrap();
	assert!(a.assemble(0x1234_5678_9ABC_DEF0).is_err());

	let mut a = CodeAssembler::new(64).unwrap();
	assert!(a.pop(cs).is_err());

	let mut a = CodeAssembler::new(64).unwrap();
	a.push(cs).unwrap();
	assert!(a.assemble(0x1234_5678_9ABC_DEF0).is_err());
}

#[test]
fn add_instruction() {
	let mut a = CodeAssembler::new(64).unwrap();
	a.nop().unwrap();
	let mut lbl = a.create_label();
	a.set_label(&mut lbl).unwrap();
	a.add_instruction(Instruction::with1(Code::Pop_r64, Register::RDX).unwrap()).unwrap();
	a.int3().unwrap();
	a.je(lbl).unwrap();
	let bytes = a.assemble(0x1234_5678_9ABC_DEF0).unwrap();
	assert_eq!(bytes, b"\x90\x5A\xCC\x74\xFC");
}

#[test]
fn test_mem_seg_overrides() {
	let mut a = CodeAssembler::new(64).unwrap();
	a.mov(ptr(rax), cl).unwrap();
	a.mov(ptr(rax).es(), cl).unwrap();
	a.mov(ptr(rax).cs(), cl).unwrap();
	a.mov(ptr(rax).ss(), cl).unwrap();
	a.mov(ptr(rax).ds(), cl).unwrap();
	a.mov(ptr(rax).fs(), cl).unwrap();
	a.mov(ptr(rax).gs(), cl).unwrap();
	a.mov(ptr(rax), cl).unwrap();
	let bytes = a.assemble(0x1234_5678_9ABC_DEF0).unwrap();
	assert_eq!(bytes, b"\x88\x08\x26\x88\x08\x2E\x88\x08\x36\x88\x08\x3E\x88\x08\x64\x88\x08\x65\x88\x08\x88\x08");
}

#[test]
fn test_mem_op_masks() {
	let mut a = CodeAssembler::new(64).unwrap();
	a.set_prefer_vex(false);
	a.vmovups(ptr(rax + 0x10), xmm2).unwrap();
	a.vmovups(ptr(rax + 0x10).k1(), xmm2).unwrap();
	a.vmovups(ptr(rax + 0x10).k2(), xmm2).unwrap();
	a.vmovups(ptr(rax + 0x10).k3(), xmm2).unwrap();
	a.vmovups(ptr(rax + 0x10).k4(), xmm2).unwrap();
	a.vmovups(ptr(rax + 0x10).k5(), xmm2).unwrap();
	a.vmovups(ptr(rax + 0x10).k6(), xmm2).unwrap();
	a.vmovups(ptr(rax + 0x10).k7(), xmm2).unwrap();
	a.vmovups(ptr(rax + 0x10), xmm2).unwrap();
	let bytes = a.assemble(0x1234_5678_9ABC_DEF0).unwrap();
	assert_eq!(
		bytes,
		b"\x62\xF1\x7C\x08\x11\x50\x01\
		\x62\xF1\x7C\x09\x11\x50\x01\
		\x62\xF1\x7C\x0A\x11\x50\x01\
		\x62\xF1\x7C\x0B\x11\x50\x01\
		\x62\xF1\x7C\x0C\x11\x50\x01\
		\x62\xF1\x7C\x0D\x11\x50\x01\
		\x62\xF1\x7C\x0E\x11\x50\x01\
		\x62\xF1\x7C\x0F\x11\x50\x01\
		\x62\xF1\x7C\x08\x11\x50\x01"
	);
}

#[test]
fn test_reg_op_masks() {
	let mut a = CodeAssembler::new(64).unwrap();
	a.set_prefer_vex(false);
	a.vmovups(xmm2, xmm3).unwrap();
	a.vmovups(xmm2.k1(), xmm3).unwrap();
	a.vmovups(xmm2.k2(), xmm3).unwrap();
	a.vmovups(xmm2.k3(), xmm3).unwrap();
	a.vmovups(xmm2.k4(), xmm3).unwrap();
	a.vmovups(xmm2.k5(), xmm3).unwrap();
	a.vmovups(xmm2.k6(), xmm3).unwrap();
	a.vmovups(xmm2.k7(), xmm3).unwrap();
	a.vmovups(xmm2.k7().z(), xmm3).unwrap();
	a.vmovups(xmm2, xmm3).unwrap();
	let bytes = a.assemble(0x1234_5678_9ABC_DEF0).unwrap();
	assert_eq!(
		bytes,
		b"\x62\xF1\x7C\x08\x10\xD3\
		\x62\xF1\x7C\x09\x10\xD3\
		\x62\xF1\x7C\x0A\x10\xD3\
		\x62\xF1\x7C\x0B\x10\xD3\
		\x62\xF1\x7C\x0C\x10\xD3\
		\x62\xF1\x7C\x0D\x10\xD3\
		\x62\xF1\x7C\x0E\x10\xD3\
		\x62\xF1\x7C\x0F\x10\xD3\
		\x62\xF1\x7C\x8F\x10\xD3\
		\x62\xF1\x7C\x08\x10\xD3"
	);
}

#[test]
fn test_reg_sae_er() {
	let mut a = CodeAssembler::new(64).unwrap();
	a.set_prefer_vex(false);
	a.vcvttss2si(edx, xmm11).unwrap();
	a.vcvttss2si(edx, xmm11.sae()).unwrap();
	a.vucomiss(xmm18, xmm3.sae()).unwrap();
	a.vcvtsi2ss(xmm2, xmm6, ebx).unwrap();
	a.vcvtsi2ss(xmm2, xmm6, ebx.rn_sae()).unwrap();
	a.vcvtsi2ss(xmm2, xmm6, ebx.rd_sae()).unwrap();
	a.vcvtsi2ss(xmm2, xmm6, ebx.ru_sae()).unwrap();
	a.vcvtsi2ss(xmm2, xmm6, ebx.rz_sae()).unwrap();
	let bytes = a.assemble(0x1234_5678_9ABC_DEF0).unwrap();
	assert_eq!(
		bytes,
		b"\x62\xD1\x7E\x08\x2C\xD3\
		\x62\xD1\x7E\x18\x2C\xD3\
		\x62\xE1\x7C\x18\x2E\xD3\
		\x62\xF1\x4E\x08\x2A\xD3\
		\x62\xF1\x4E\x18\x2A\xD3\
		\x62\xF1\x4E\x38\x2A\xD3\
		\x62\xF1\x4E\x58\x2A\xD3\
		\x62\xF1\x4E\x78\x2A\xD3"
	);
}

#[test]
fn test_mem_ops_16() {
	let mut a = CodeAssembler::new(16).unwrap();
	a.mov(ptr(0x1234), eax).unwrap();
	a.mov(ax, ptr(0xFEDC)).unwrap();
	a.mov(ptr(bx), cl).unwrap();
	a.mov(ptr(bp), cl).unwrap();
	a.mov(ptr(bx + 1), cl).unwrap();
	a.mov(ptr(bp - 1), cl).unwrap();
	a.mov(ptr(bx + 0x1234), cl).unwrap();
	a.mov(ptr(bp - 0x1234), cl).unwrap();
	a.mov(ptr(bx + si), cl).unwrap();
	a.mov(ptr(bx + si + 1), cl).unwrap();
	a.mov(ptr(bx + si - 1), cl).unwrap();
	a.mov(ptr(bp + di + 0x1234), cl).unwrap();
	a.mov(ptr(bp + di - 0x1234), cl).unwrap();
	a.mov(ptr((bp + di - 0x1234) - 10), cl).unwrap();
	a.mov(ptr((bp + di - 0x1234) + 10), cl).unwrap();
	let bytes = a.assemble(0x1234).unwrap();
	assert_eq!(
		bytes,
		b"\x66\xA3\x34\x12\
		\xA1\xDC\xFE\
		\x88\x0F\
		\x88\x4E\x00\
		\x88\x4F\x01\
		\x88\x4E\xFF\
		\x88\x8F\x34\x12\
		\x88\x8E\xCC\xED\
		\x88\x08\
		\x88\x48\x01\
		\x88\x48\xFF\
		\x88\x8B\x34\x12\
		\x88\x8B\xCC\xED\
		\x88\x8B\xC2\xED\
		\x88\x8B\xD6\xED"
	);
}

#[test]
#[allow(clippy::identity_op)]
fn test_mem_ops_32() {
	let mut a = CodeAssembler::new(32).unwrap();
	a.mov(ptr(0x1234_5678), eax).unwrap();
	a.mov(ax, ptr(0xFEDC_BA98u32)).unwrap();
	a.mov(ptr(ecx), cl).unwrap();
	a.mov(ptr(ecx + 123), cl).unwrap();
	a.mov(ptr(ecx - 123), cl).unwrap();
	a.mov(ptr(ecx * 1), cl).unwrap();
	a.mov(ptr(ecx * 2), cl).unwrap();
	a.mov(ptr(ecx * 2 + 123), cl).unwrap();
	a.mov(ptr(ecx * 2 - 123), cl).unwrap();
	a.mov(ptr(ecx + edx), cl).unwrap();
	a.mov(ptr(ecx + edx * 1), cl).unwrap();
	a.mov(ptr(ecx + edx * 2), cl).unwrap();
	a.mov(ptr(ecx + edx * 4 + 123), cl).unwrap();
	a.mov(ptr(ecx + edx * 8 - 123), cl).unwrap();
	a.mov(ptr((ecx + edx * 8 - 123) - 10), cl).unwrap();
	a.mov(ptr((ecx + edx * 8 - 123) + 10), cl).unwrap();
	a.mov(ptr(2 * ecx), cl).unwrap();
	a.vpgatherdd(xmm2.k1(), ptr(ecx + xmm4 * 4 + 1)).unwrap();
	a.vpgatherdd(xmm2.k2(), ptr(xmm4 * 4 + ecx - 1)).unwrap();
	a.vpgatherdd(xmm2.k3(), ptr(xmm4 + ecx + 1)).unwrap();
	a.vpgatherdd(xmm2.k4(), ptr(ecx + xmm4 - 1)).unwrap();
	let bytes = a.assemble(0x1234_5678).unwrap();
	assert_eq!(
		bytes,
		b"\xA3\x78\x56\x34\x12\
		\x66\xA1\x98\xBA\xDC\xFE\
		\x88\x09\
		\x88\x49\x7B\
		\x88\x49\x85\
		\x88\x0C\x0D\x00\x00\x00\x00\
		\x88\x0C\x4D\x00\x00\x00\x00\
		\x88\x0C\x4D\x7B\x00\x00\x00\
		\x88\x0C\x4D\x85\xFF\xFF\xFF\
		\x88\x0C\x11\
		\x88\x0C\x11\
		\x88\x0C\x51\
		\x88\x4C\x91\x7B\
		\x88\x4C\xD1\x85\
		\x88\x8C\xD1\x7B\xFF\xFF\xFF\
		\x88\x4C\xD1\x8F\
		\x88\x0C\x4D\x00\x00\x00\x00\
		\x62\xF2\x7D\x09\x90\x94\xA1\x01\x00\x00\x00\
		\x62\xF2\x7D\x0A\x90\x94\xA1\xFF\xFF\xFF\xFF\
		\x62\xF2\x7D\x0B\x90\x94\x21\x01\x00\x00\x00\
		\x62\xF2\x7D\x0C\x90\x94\x21\xFF\xFF\xFF\xFF"
	);
}

#[test]
#[allow(clippy::identity_op)]
fn test_mem_ops_64() {
	let mut a = CodeAssembler::new(64).unwrap();
	a.mov(ptr(0x1234_5678_9ABC_DEF0i64), rax).unwrap();
	a.mov(eax, ptr(0xF234_5678_9ABC_DEF1u64)).unwrap();
	a.mov(ptr(rcx), cl).unwrap();
	a.mov(ptr(rcx + 123), cl).unwrap();
	a.mov(ptr(rcx - 123), cl).unwrap();
	a.mov(ptr(rcx * 1), cl).unwrap();
	a.mov(ptr(rcx * 2), cl).unwrap();
	a.mov(ptr(rcx * 2 + 123), cl).unwrap();
	a.mov(ptr(rcx * 2 - 123), cl).unwrap();
	a.mov(ptr(rcx + rdx), cl).unwrap();
	a.mov(ptr(rcx + rdx * 1), cl).unwrap();
	a.mov(ptr(rcx + rdx * 2), cl).unwrap();
	a.mov(ptr(rcx + rdx * 4 + 123), cl).unwrap();
	a.mov(ptr(rcx + rdx * 8 - 123), cl).unwrap();
	a.mov(ptr((rcx + rdx * 8 - 123) - 10), cl).unwrap();
	a.mov(ptr((rcx + rdx * 8 - 123) + 10), cl).unwrap();
	a.mov(ptr(2 * rcx), cl).unwrap();
	a.vpgatherdd(xmm2.k1(), ptr(rcx + xmm4 * 4 + 1)).unwrap();
	a.vpgatherdd(xmm2.k2(), ptr(xmm4 * 4 + rcx - 1)).unwrap();
	a.vpgatherdd(xmm2.k3(), ptr(xmm4 + rcx + 1)).unwrap();
	a.vpgatherdd(xmm2.k4(), ptr(rcx + xmm4 - 1)).unwrap();
	let bytes = a.assemble(0x1234_5678_9ABC_DEF0).unwrap();
	assert_eq!(
		bytes,
		b"\x48\xA3\xF0\xDE\xBC\x9A\x78\x56\x34\x12\
		\xA1\xF1\xDE\xBC\x9A\x78\x56\x34\xF2\
		\x88\x09\
		\x88\x49\x7B\
		\x88\x49\x85\
		\x88\x0C\x0D\x00\x00\x00\x00\
		\x88\x0C\x4D\x00\x00\x00\x00\
		\x88\x0C\x4D\x7B\x00\x00\x00\
		\x88\x0C\x4D\x85\xFF\xFF\xFF\
		\x88\x0C\x11\
		\x88\x0C\x11\
		\x88\x0C\x51\
		\x88\x4C\x91\x7B\
		\x88\x4C\xD1\x85\
		\x88\x8C\xD1\x7B\xFF\xFF\xFF\
		\x88\x4C\xD1\x8F\
		\x88\x0C\x4D\x00\x00\x00\x00\
		\x62\xF2\x7D\x09\x90\x94\xA1\x01\x00\x00\x00\
		\x62\xF2\x7D\x0A\x90\x94\xA1\xFF\xFF\xFF\xFF\
		\x62\xF2\x7D\x0B\x90\x94\x21\x01\x00\x00\x00\
		\x62\xF2\x7D\x0C\x90\x94\x21\xFF\xFF\xFF\xFF"
	);
}

#[test]
fn test_label_mem_ops() {
	let mut a = CodeAssembler::new(64).unwrap();
	let mut lbl1 = a.create_label();
	let mut lbl2 = a.create_label();
	a.nop().unwrap();
	a.set_label(&mut lbl1).unwrap();
	a.int1().unwrap();
	a.lea(rax, ptr(lbl1)).unwrap();
	a.lea(rax, ptr(lbl2)).unwrap();
	a.int3().unwrap();
	a.set_label(&mut lbl2).unwrap();
	a.db(b"\x12\x34\x56\x78").unwrap();
	let bytes = a.assemble(0x1234_5678_9ABC_DEF0).unwrap();
	assert_eq!(bytes, b"\x90\xF1\x48\x8D\x05\xF8\xFF\xFF\xFF\x48\x8D\x05\x01\x00\x00\x00\xCC\x12\x34\x56\x78");
}

#[test]
fn test_call_far() {
	test_instr(
		16,
		|a| a.call_far(0x1234, 0x5678).unwrap(),
		Instruction::with_far_branch(Code::Call_ptr1616, 0x1234, 0x5678).unwrap(),
		TestInstrFlags::NONE,
		DecoderOptions::NONE,
	);
	test_instr(
		32,
		|a| a.call_far(0x1234, 0x5678_9ABC).unwrap(),
		Instruction::with_far_branch(Code::Call_ptr1632, 0x1234, 0x5678_9ABC).unwrap(),
		TestInstrFlags::NONE,
		DecoderOptions::NONE,
	);
}

#[test]
fn test_jmp_far() {
	test_instr(
		16,
		|a| a.jmp_far(0x1234, 0x5678).unwrap(),
		Instruction::with_far_branch(Code::Jmp_ptr1616, 0x1234, 0x5678).unwrap(),
		TestInstrFlags::NONE,
		DecoderOptions::NONE,
	);
	test_instr(
		32,
		|a| a.jmp_far(0x1234, 0x5678_9ABC).unwrap(),
		Instruction::with_far_branch(Code::Jmp_ptr1632, 0x1234, 0x5678_9ABC).unwrap(),
		TestInstrFlags::NONE,
		DecoderOptions::NONE,
	);
}

#[test]
fn test_xlatb() {
	test_instr(
		16,
		|a| a.xlatb().unwrap(),
		Instruction::with1(Code::Xlat_m8, MemoryOperand::with_base_index(Register::BX, Register::AL)).unwrap(),
		TestInstrFlags::NONE,
		DecoderOptions::NONE,
	);
	test_instr(
		32,
		|a| a.xlatb().unwrap(),
		Instruction::with1(Code::Xlat_m8, MemoryOperand::with_base_index(Register::EBX, Register::AL)).unwrap(),
		TestInstrFlags::NONE,
		DecoderOptions::NONE,
	);
	test_instr(
		64,
		|a| a.xlatb().unwrap(),
		Instruction::with1(Code::Xlat_m8, MemoryOperand::with_base_index(Register::RBX, Register::AL)).unwrap(),
		TestInstrFlags::NONE,
		DecoderOptions::NONE,
	);
}

#[test]
fn test_xbegin_label() {
	test_instr(
		16,
		|a| {
			let lbl = create_and_emit_label(a);
			a.xbegin(lbl).unwrap()
		},
		assign_label(Instruction::with_xbegin(16, FIRST_LABEL_ID).unwrap(), FIRST_LABEL_ID),
		TestInstrFlags::BRANCH,
		DecoderOptions::NONE,
	);
	test_instr(
		32,
		|a| {
			let lbl = create_and_emit_label(a);
			a.xbegin(lbl).unwrap()
		},
		assign_label(Instruction::with_xbegin(32, FIRST_LABEL_ID).unwrap(), FIRST_LABEL_ID),
		TestInstrFlags::BRANCH | TestInstrFlags::IGNORE_CODE,
		DecoderOptions::NONE,
	);
	test_instr(
		64,
		|a| {
			let lbl = create_and_emit_label(a);
			a.xbegin(lbl).unwrap()
		},
		assign_label(Instruction::with_xbegin(64, FIRST_LABEL_ID).unwrap(), FIRST_LABEL_ID),
		TestInstrFlags::BRANCH | TestInstrFlags::IGNORE_CODE,
		DecoderOptions::NONE,
	);
}

#[test]
fn test_xbegin_offset() {
	test_instr(16, |a| a.xbegin(12752).unwrap(), Instruction::with_xbegin(16, 12752).unwrap(), TestInstrFlags::BRANCH_U64, DecoderOptions::NONE);
	test_instr(
		32,
		|a| a.xbegin(12752).unwrap(),
		Instruction::with_xbegin(32, 12752).unwrap(),
		TestInstrFlags::BRANCH_U64 | TestInstrFlags::IGNORE_CODE,
		DecoderOptions::NONE,
	);
	test_instr(
		64,
		|a| a.xbegin(12752).unwrap(),
		Instruction::with_xbegin(64, 12752).unwrap(),
		TestInstrFlags::BRANCH_U64 | TestInstrFlags::IGNORE_CODE,
		DecoderOptions::NONE,
	);
}

// GENERATOR-BEGIN: TestInstrFlags
// ⚠️This was generated by GENERATOR!🦹‍♂️
pub(crate) struct TestInstrFlags;
#[allow(dead_code)]
impl TestInstrFlags {
	pub(crate) const NONE: u32 = 0x0000_0000;
	pub(crate) const FWAIT: u32 = 0x0000_0001;
	pub(crate) const PREFER_VEX: u32 = 0x0000_0002;
	pub(crate) const PREFER_EVEX: u32 = 0x0000_0004;
	pub(crate) const PREFER_SHORT_BRANCH: u32 = 0x0000_0008;
	pub(crate) const PREFER_NEAR_BRANCH: u32 = 0x0000_0010;
	pub(crate) const BRANCH: u32 = 0x0000_0020;
	pub(crate) const BROADCAST: u32 = 0x0000_0040;
	pub(crate) const BRANCH_U64: u32 = 0x0000_0080;
	pub(crate) const IGNORE_CODE: u32 = 0x0000_0100;
	pub(crate) const REMOVE_REP_REPNE_PREFIXES: u32 = 0x0000_0200;
}
// GENERATOR-END: TestInstrFlags

fn create_asm(bitness: u32, flags: u32) -> CodeAssembler {
	let mut a = CodeAssembler::new(bitness).unwrap();
	if (flags & TestInstrFlags::PREFER_VEX) != 0 {
		a.set_prefer_vex(true);
	} else if (flags & TestInstrFlags::PREFER_EVEX) != 0 {
		a.set_prefer_vex(false);
	}
	if (flags & TestInstrFlags::PREFER_SHORT_BRANCH) != 0 {
		a.set_prefer_short_branch(true);
	} else if (flags & TestInstrFlags::PREFER_NEAR_BRANCH) != 0 {
		a.set_prefer_short_branch(false);
	}
	a
}

fn test_instr(bitness: u32, create: fn(&mut CodeAssembler), mut expected: Instruction, flags: u32, decoder_options: u32) {
	let mut a = create_asm(bitness, flags);
	create(&mut a);
	assert_eq!(a.instructions.len(), 1);

	if (flags & TestInstrFlags::BROADCAST) != 0 {
		expected.set_is_broadcast(true);
	}
	let mut asm_instr = a.instructions[0];
	assert_eq!(asm_instr, expected);

	let rip = 0;
	let bytes = if (flags & TestInstrFlags::BRANCH_U64) != 0 {
		a.assemble(rip).unwrap()
	} else {
		a.assemble_options(rip, BlockEncoderOptions::DONT_FIX_BRANCHES).unwrap().inner.code_buffer
	};

	let mut decoder = Decoder::with_ip(bitness, &bytes, rip, decoder_options);
	let mut decoded_instr =
		if expected.code() == Code::Zero_bytes && bytes.is_empty() { Instruction::with(Code::Zero_bytes) } else { decoder.decode() };
	if (flags & TestInstrFlags::IGNORE_CODE) != 0 {
		decoded_instr.set_code(asm_instr.code());
	}
	if (flags & TestInstrFlags::REMOVE_REP_REPNE_PREFIXES) != 0 {
		decoded_instr.set_has_rep_prefix(false);
		decoded_instr.set_has_repne_prefix(false);
	}
	if (flags & TestInstrFlags::FWAIT) != 0 {
		assert_eq!(decoded_instr.code(), Code::Wait);
		decoded_instr = decoder.decode();
		let new_code = match decoded_instr.code() {
			Code::Fnstenv_m14byte => Code::Fstenv_m14byte,
			Code::Fnstenv_m28byte => Code::Fstenv_m28byte,
			Code::Fnstcw_m2byte => Code::Fstcw_m2byte,
			Code::Fneni => Code::Feni,
			Code::Fndisi => Code::Fdisi,
			Code::Fnclex => Code::Fclex,
			Code::Fninit => Code::Finit,
			Code::Fnsetpm => Code::Fsetpm,
			Code::Fnsave_m94byte => Code::Fsave_m94byte,
			Code::Fnsave_m108byte => Code::Fsave_m108byte,
			Code::Fnstsw_m2byte => Code::Fstsw_m2byte,
			Code::Fnstsw_AX => Code::Fstsw_AX,
			Code::Fnstdw_AX => Code::Fstdw_AX,
			Code::Fnstsg_AX => Code::Fstsg_AX,
			_ => unreachable!(),
		};
		decoded_instr.set_code(new_code);
	}
	if !matches!(asm_instr.code(), Code::Jmpe_disp16 | Code::Jmpe_disp32) && (flags & TestInstrFlags::BRANCH) != 0 {
		asm_instr.set_near_branch64(0);
	}

	// Short branches can be re-written if the target is too far away.
	// Eg. `loopne target` => `loopne jmpt; jmp short skip; jmpt: jmp near target; skip:`
	if (flags & TestInstrFlags::BRANCH_U64) != 0 {
		asm_instr.set_code(asm_instr.code().as_short_branch());
		decoded_instr.set_code(decoded_instr.code().as_short_branch());
		assert_eq!(asm_instr.code(), decoded_instr.code());

		if decoded_instr.near_branch64() == 4 {
			let next_instr = decoder.decode();
			let expected_code = match bitness {
				16 => Code::Jmp_rel8_16,
				32 => Code::Jmp_rel8_32,
				64 => Code::Jmp_rel8_64,
				_ => unreachable!(),
			};
			assert_eq!(next_instr.code(), expected_code);
		} else {
			assert_eq!(decoded_instr, asm_instr);
		}
	} else {
		assert_eq!(decoded_instr, asm_instr);
	}
}

fn test_invalid_instr(bitness: u32, create: fn(&mut CodeAssembler), flags: u32) {
	let mut a = create_asm(bitness, flags);
	create(&mut a);
	assert_eq!(a.instructions.len(), 0);
}

const FIRST_LABEL_ID: u64 = 1;

fn add_op_mask(mut instruction: Instruction, op_mask: Register) -> Instruction {
	instruction.set_op_mask(op_mask);
	instruction
}

fn create_and_emit_label(a: &mut CodeAssembler) -> CodeLabel {
	let mut lbl = a.create_label();
	a.set_label(&mut lbl).unwrap();
	lbl
}

fn assign_label(mut instruction: Instruction, label: u64) -> Instruction {
	instruction.set_ip(label);
	instruction
}

#[test]
fn test_vex_evex_prefixes() {
	let mut a = CodeAssembler::new(64).unwrap();

	a.set_prefer_vex(true);
	assert_eq!(a.prefer_vex(), true);
	a.vaddpd(xmm1, xmm2, xmm3).unwrap();
	a.vex().vaddpd(xmm1, xmm2, xmm3).unwrap();
	a.vaddpd(xmm1, xmm2, xmm3).unwrap();
	a.evex().vaddpd(xmm1, xmm2, xmm3).unwrap();
	a.vaddpd(xmm1, xmm2, xmm3).unwrap();
	assert_eq!(a.prefer_vex(), true);

	a.set_prefer_vex(false);
	assert_eq!(a.prefer_vex(), false);
	a.vaddpd(xmm1, xmm2, xmm3).unwrap();
	a.vex().vaddpd(xmm1, xmm2, xmm3).unwrap();
	a.vaddpd(xmm1, xmm2, xmm3).unwrap();
	a.evex().vaddpd(xmm1, xmm2, xmm3).unwrap();
	a.vaddpd(xmm1, xmm2, xmm3).unwrap();
	assert_eq!(a.prefer_vex(), false);

	let bytes = a.assemble(0x1234_5678_9ABC_DEF0).unwrap();
	assert_eq!(
		bytes,
		b"\
		\xC5\xE9\x58\xCB\
		\xC5\xE9\x58\xCB\
		\xC5\xE9\x58\xCB\
		\x62\xF1\xED\x08\x58\xCB\
		\xC5\xE9\x58\xCB\
		\x62\xF1\xED\x08\x58\xCB\
		\xC5\xE9\x58\xCB\
		\x62\xF1\xED\x08\x58\xCB\
		\x62\xF1\xED\x08\x58\xCB\
		\x62\xF1\xED\x08\x58\xCB"
	);
}

#[test]
fn test_zero_bytes() {
	let mut a = CodeAssembler::new(64).unwrap();

	let mut lblf = a.create_label();
	let mut lbll = a.create_label();
	let mut lbl1 = a.create_label();
	let mut lbl2 = a.create_label();

	a.set_label(&mut lblf).unwrap();
	a.zero_bytes().unwrap();

	a.je(lbl1).unwrap();
	a.je(lbl2).unwrap();
	a.set_label(&mut lbl1).unwrap();
	a.zero_bytes().unwrap();
	a.set_label(&mut lbl2).unwrap();
	a.nop().unwrap();
	a.lock().rep().zero_bytes().unwrap();

	a.set_label(&mut lbll).unwrap();
	a.zero_bytes().unwrap();

	let bytes = a.assemble(0x1234_5678_9ABC_DEF0).unwrap();
	assert_eq!(bytes, b"\x74\x02\x74\x00\x90");
}

#[test]
fn test_label_ip() {
	for &bitness in &[16, 32, 64] {
		let mut a = CodeAssembler::new(bitness).unwrap();
		let label0 = a.create_label();
		let mut label1 = a.create_label();
		a.nop().unwrap();
		a.nop().unwrap();
		a.nop().unwrap();
		a.set_label(&mut label1).unwrap();
		a.nop().unwrap();

		let result = a.assemble_options(0x1234_5678_9ABC_DEF0, BlockEncoderOptions::RETURN_NEW_INSTRUCTION_OFFSETS).unwrap();
		let label1_ip = result.label_ip(&label1).unwrap();
		assert_eq!(label1_ip, 0x1234_5678_9ABC_DEF3);
		assert!(result.label_ip(&label0).is_err());
	}
}
