// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

mod constants;
mod info_test_case;
mod mem_size_test_case;
mod mem_size_test_parser;
mod misc_test_data;
mod reg_info_test_case;
mod reg_test_parser;
mod test_parser;
mod va;

use crate::iced_constants::IcedConstants;
use crate::info::factory::*;
use crate::info::tests::constants::*;
use crate::info::tests::info_test_case::*;
use crate::info::tests::mem_size_test_parser::*;
use crate::info::tests::misc_test_data::*;
use crate::info::tests::reg_test_parser::*;
use crate::info::tests::test_parser::*;
use crate::test_utils::from_str_conv::*;
use crate::test_utils::*;
use crate::*;
use alloc::string::String;
use alloc::vec::Vec;
use core::cmp::Ordering;
use core::fmt::Write;
use core::mem;
use lazy_static::lazy_static;
use std::collections::{HashMap, HashSet};
use std::panic;

lazy_static! {
	static ref INSTR_INFO_16: Vec<InstrInfoTestCase> = read_instr_info_test_cases(16);
}
lazy_static! {
	static ref INSTR_INFO_32: Vec<InstrInfoTestCase> = read_instr_info_test_cases(32);
}
lazy_static! {
	static ref INSTR_INFO_64: Vec<InstrInfoTestCase> = read_instr_info_test_cases(64);
}

fn read_instr_info_test_cases(bitness: u32) -> Vec<InstrInfoTestCase> {
	let mut path = get_instr_info_unit_tests_dir();
	path.push(format!("InstructionInfoTest_{}.txt", bitness));
	InstrInfoTestParser::new(bitness, &path).into_iter().collect()
}

fn get_instr_info_test_cases(bitness: u32) -> &'static Vec<InstrInfoTestCase> {
	match bitness {
		16 => &INSTR_INFO_16,
		32 => &INSTR_INFO_32,
		64 => &INSTR_INFO_64,
		_ => unreachable!(),
	}
}

#[test]
fn info_16() {
	test_info(16);
}

#[test]
fn info_32() {
	test_info(32);
}

#[test]
fn info_64() {
	test_info(64);
}

fn test_info(bitness: u32) {
	let mut factory = InstructionInfoFactory::new();
	for tc in get_instr_info_test_cases(bitness) {
		test_info_core(tc, &mut factory);
	}
}

#[allow(unused_mut)]
fn test_info_core(tc: &InstrInfoTestCase, factory: &mut InstructionInfoFactory) {
	let code_bytes = to_vec_u8(&tc.hex_bytes).unwrap();
	let mut instr;
	if tc.is_special {
		if tc.bitness == 16 && tc.code == Code::Popw_CS && tc.hex_bytes == "0F" {
			instr = Instruction::default();
			instr.set_code(Code::Popw_CS);
			instr.set_op0_kind(OpKind::Register);
			instr.set_op0_register(Register::CS);
			instr.set_code_size(CodeSize::Code16);
			instr.set_len(1);
		} else if tc.code <= Code::DeclareQword {
			instr = Instruction::default();
			instr.set_code(tc.code);
			instr.set_declare_data_len(1);
			assert_eq!(tc.bitness, 64);
			instr.set_code_size(CodeSize::Code64);
			match tc.code {
				Code::DeclareByte => {
					assert_eq!(tc.hex_bytes, "66");
					instr.set_declare_byte_value(0, 0x66);
				}
				Code::DeclareWord => {
					assert_eq!(tc.hex_bytes, "6644");
					instr.set_declare_word_value(0, 0x4466);
				}
				Code::DeclareDword => {
					assert_eq!(tc.hex_bytes, "664422EE");
					instr.set_declare_dword_value(0, 0xEE22_4466);
				}
				Code::DeclareQword => {
					assert_eq!(tc.hex_bytes, "664422EE12345678");
					instr.set_declare_qword_value(0, 0x7856_3412_EE22_4466);
				}
				_ => unreachable!(),
			}
		} else if tc.code == Code::Zero_bytes {
			instr = Instruction::default();
			instr.set_code(tc.code);
			assert_eq!(tc.bitness, 64);
			instr.set_code_size(CodeSize::Code64);
			assert_eq!(tc.hex_bytes, "");
		} else {
			let mut decoder = create_decoder(tc.bitness, &code_bytes, tc.ip, tc.decoder_options).0;
			instr = decoder.decode();
			if code_bytes.len() > 1 && code_bytes[0] == 0x9B && instr.len() == 1 {
				instr = decoder.decode();
				match instr.code() {
					Code::Fnstenv_m14byte => instr.set_code(Code::Fstenv_m14byte),
					Code::Fnstenv_m28byte => instr.set_code(Code::Fstenv_m28byte),
					Code::Fnstcw_m2byte => instr.set_code(Code::Fstcw_m2byte),
					Code::Fneni => instr.set_code(Code::Feni),
					Code::Fndisi => instr.set_code(Code::Fdisi),
					Code::Fnclex => instr.set_code(Code::Fclex),
					Code::Fninit => instr.set_code(Code::Finit),
					Code::Fnsetpm => instr.set_code(Code::Fsetpm),
					Code::Fnsave_m94byte => instr.set_code(Code::Fsave_m94byte),
					Code::Fnsave_m108byte => instr.set_code(Code::Fsave_m108byte),
					Code::Fnstsw_m2byte => instr.set_code(Code::Fstsw_m2byte),
					Code::Fnstsw_AX => instr.set_code(Code::Fstsw_AX),
					Code::Fnstdw_AX => instr.set_code(Code::Fstdw_AX),
					Code::Fnstsg_AX => instr.set_code(Code::Fstsg_AX),
					_ => unreachable!(),
				}
			} else {
				unreachable!();
			}
		}
	} else {
		let mut decoder = create_decoder(tc.bitness, &code_bytes, tc.ip, tc.decoder_options).0;
		instr = decoder.decode();
	}
	let instr = instr;
	assert_eq!(instr.code(), tc.code);

	assert_eq!(instr.stack_pointer_increment(), tc.stack_pointer_increment);

	let mut factory1 = InstructionInfoFactory::new();
	let info = factory1.info(&instr);
	assert_eq!(info.op0_access(), tc.op0_access);
	assert_eq!(info.op1_access(), tc.op1_access);
	assert_eq!(info.op2_access(), tc.op2_access);
	assert_eq!(info.op3_access(), tc.op3_access);
	assert_eq!(info.op4_access(), tc.op4_access);
	let fpu_info = instr.fpu_stack_increment_info();
	assert_eq!(fpu_info.increment(), tc.fpu_top_increment);
	assert_eq!(fpu_info.conditional(), tc.fpu_conditional_top);
	assert_eq!(fpu_info.writes_top(), tc.fpu_writes_top);
	assert_eq!(info.used_memory().iter().collect::<HashSet<_>>(), tc.used_memory.iter().collect::<HashSet<_>>());
	assert_eq!(get_used_registers(info.used_registers().iter()), get_used_registers(tc.used_registers.iter()));

	const _: () = assert!(IcedConstants::MAX_OP_COUNT == 5);
	assert!(instr.op_count() <= IcedConstants::MAX_OP_COUNT as u32);
	for i in 0..instr.op_count() {
		match i {
			0 => assert_eq!(tc.op0_access, info.op_access(i)),
			1 => assert_eq!(tc.op1_access, info.op_access(i)),
			2 => assert_eq!(tc.op2_access, info.op_access(i)),
			3 => assert_eq!(tc.op3_access, info.op_access(i)),
			4 => assert_eq!(tc.op4_access, info.op_access(i)),
			_ => unreachable!(),
		}
		match i {
			0 => assert_eq!(tc.op0_access, info.try_op_access(i).unwrap()),
			1 => assert_eq!(tc.op1_access, info.try_op_access(i).unwrap()),
			2 => assert_eq!(tc.op2_access, info.try_op_access(i).unwrap()),
			3 => assert_eq!(tc.op3_access, info.try_op_access(i).unwrap()),
			4 => assert_eq!(tc.op4_access, info.try_op_access(i).unwrap()),
			_ => unreachable!(),
		}
	}
	for i in instr.op_count()..IcedConstants::MAX_OP_COUNT as u32 {
		assert_eq!(info.op_access(i), OpAccess::None);
		assert_eq!(info.try_op_access(i).unwrap(), OpAccess::None);
	}
	if cfg!(debug_assertions) {
		assert!(panic::catch_unwind(|| { info.op_access(IcedConstants::MAX_OP_COUNT as u32) }).is_err());
	} else {
		let _ = info.op_access(IcedConstants::MAX_OP_COUNT as u32);
	}
	assert!(info.try_op_access(IcedConstants::MAX_OP_COUNT as u32).is_err());

	let mut factory2 = InstructionInfoFactory::new();
	let info2 = factory2.info_options(&instr, InstructionInfoOptions::NONE);
	check_equal(info, info2, true, true);
	let mut factory2 = InstructionInfoFactory::new();
	let info2 = factory2.info_options(&instr, InstructionInfoOptions::NO_MEMORY_USAGE);
	check_equal(info, info2, true, false);
	let mut factory2 = InstructionInfoFactory::new();
	let info2 = factory2.info_options(&instr, InstructionInfoOptions::NO_REGISTER_USAGE);
	check_equal(info, info2, false, true);
	let mut factory2 = InstructionInfoFactory::new();
	let info2 = factory2.info_options(&instr, InstructionInfoOptions::NO_REGISTER_USAGE | InstructionInfoOptions::NO_MEMORY_USAGE);
	check_equal(info, info2, false, false);

	{
		let info2 = factory.info(&instr);
		check_equal(info, info2, true, true);
	}
	{
		let info2 = factory.info_options(&instr, InstructionInfoOptions::NONE);
		check_equal(info, info2, true, true);
	}
	{
		let info2 = factory.info_options(&instr, InstructionInfoOptions::NO_MEMORY_USAGE);
		check_equal(info, info2, true, false);
	}
	{
		let info2 = factory.info_options(&instr, InstructionInfoOptions::NO_REGISTER_USAGE);
		check_equal(info, info2, false, true);
	}
	{
		let info2 = factory.info_options(&instr, InstructionInfoOptions::NO_REGISTER_USAGE | InstructionInfoOptions::NO_MEMORY_USAGE);
		check_equal(info, info2, false, false);
	}

	assert_eq!(instr.code().encoding(), tc.encoding);
	#[cfg(all(feature = "encoder", feature = "op_code_info"))]
	{
		assert_eq!(tc.encoding, tc.code.op_code().encoding());
	}
	assert_eq!(instr.code().cpuid_features(), tc.cpuid_features);
	assert_eq!(instr.code().flow_control(), tc.flow_control);
	assert_eq!(instr.code().is_privileged(), tc.is_privileged);
	assert_eq!(instr.code().is_stack_instruction(), tc.is_stack_instruction);
	assert_eq!(instr.code().is_save_restore_instruction(), tc.is_save_restore_instruction);

	assert_eq!(instr.encoding(), tc.encoding);
	#[cfg(feature = "mvex")]
	assert_eq!(instr.encoding() == EncodingKind::MVEX, IcedConstants::is_mvex(instr.code()));
	assert_eq!(instr.cpuid_features(), tc.cpuid_features);
	assert_eq!(instr.flow_control(), tc.flow_control);
	assert_eq!(instr.is_privileged(), tc.is_privileged);
	assert_eq!(instr.is_stack_instruction(), tc.is_stack_instruction);
	assert_eq!(instr.is_save_restore_instruction(), tc.is_save_restore_instruction);
	assert_eq!(instr.rflags_read(), tc.rflags_read);
	assert_eq!(instr.rflags_written(), tc.rflags_written);
	assert_eq!(instr.rflags_cleared(), tc.rflags_cleared);
	assert_eq!(instr.rflags_set(), tc.rflags_set);
	assert_eq!(instr.rflags_undefined(), tc.rflags_undefined);
	assert_eq!(instr.rflags_modified(), tc.rflags_written | tc.rflags_cleared | tc.rflags_set | tc.rflags_undefined);

	assert_eq!(instr.rflags_written() & (instr.rflags_cleared() | instr.rflags_set() | instr.rflags_undefined()), RflagsBits::NONE);
	assert_eq!(instr.rflags_cleared() & (instr.rflags_written() | instr.rflags_set() | instr.rflags_undefined()), RflagsBits::NONE);
	assert_eq!(instr.rflags_set() & (instr.rflags_written() | instr.rflags_cleared() | instr.rflags_undefined()), RflagsBits::NONE);
	assert_eq!(instr.rflags_undefined() & (instr.rflags_written() | instr.rflags_cleared() | instr.rflags_set()), RflagsBits::NONE);
}

fn check_equal(info1: &InstructionInfo, info2: &InstructionInfo, has_regs2: bool, has_mem2: bool) {
	if has_regs2 {
		assert_eq!(info2.used_registers(), info1.used_registers());
	} else {
		assert!(info2.used_registers().is_empty());
	}
	if has_mem2 {
		assert_eq!(info2.used_memory(), info1.used_memory());
	} else {
		assert!(info2.used_memory().is_empty());
	}
	assert_eq!(info2.op0_access(), info1.op0_access());
	assert_eq!(info2.op1_access(), info1.op1_access());
	assert_eq!(info2.op2_access(), info1.op2_access());
	assert_eq!(info2.op3_access(), info1.op3_access());
	assert_eq!(info2.op4_access(), info1.op4_access());
}

#[must_use]
fn get_used_registers<'a, T: Iterator<Item = &'a UsedRegister>>(iter: T) -> Vec<UsedRegister> {
	let mut read: Vec<Register> = Vec::new();
	let mut write: Vec<Register> = Vec::new();
	let mut cond_read: Vec<Register> = Vec::new();
	let mut cond_write: Vec<Register> = Vec::new();

	for info in iter {
		match info.access() {
			OpAccess::Read => read.push(info.register()),
			OpAccess::CondRead => cond_read.push(info.register()),
			OpAccess::Write => write.push(info.register()),
			OpAccess::CondWrite => cond_write.push(info.register()),
			OpAccess::ReadWrite => {
				read.push(info.register());
				write.push(info.register());
			}
			OpAccess::ReadCondWrite => {
				read.push(info.register());
				cond_write.push(info.register());
			}
			OpAccess::None | OpAccess::NoMemAccess => unreachable!(),
		}
	}

	let mut h: HashSet<UsedRegister> = HashSet::new();
	h.extend(get_registers(read).into_iter().map(|reg| UsedRegister { register: reg, access: OpAccess::Read }));
	h.extend(get_registers(write).into_iter().map(|reg| UsedRegister { register: reg, access: OpAccess::Write }));
	h.extend(get_registers(cond_read).into_iter().map(|reg| UsedRegister { register: reg, access: OpAccess::CondRead }));
	h.extend(get_registers(cond_write).into_iter().map(|reg| UsedRegister { register: reg, access: OpAccess::CondWrite }));
	let mut vec: Vec<_> = h.into_iter().collect();
	vec.sort_by(|x, y| {
		let c = x.register.cmp(&y.register);
		if c != Ordering::Equal {
			c
		} else {
			x.access.cmp(&y.access)
		}
	});
	vec
}

#[must_use]
fn get_registers(mut regs: Vec<Register>) -> Vec<Register> {
	if regs.len() <= 1 {
		return regs;
	}

	regs.sort_by(|x, y| {
		let ord = get_register_group_order(*x).cmp(&get_register_group_order(*y));
		if ord != Ordering::Equal {
			ord
		} else {
			(*x as i32).cmp(&(*y as i32))
		}
	});

	let mut hash: HashSet<Register> = HashSet::new();
	let mut index;
	for reg in regs {
		if Register::EAX <= reg && reg <= Register::R15D {
			index = reg as u32 - Register::EAX as u32;
			if hash.contains(&(Register::RAX + index)) {
				continue;
			}
		} else if Register::AX <= reg && reg <= Register::R15W {
			index = reg as u32 - Register::AX as u32;
			if hash.contains(&(Register::RAX + index)) {
				continue;
			}
			if hash.contains(&(Register::EAX + index)) {
				continue;
			}
		} else if Register::AL <= reg && reg <= Register::R15L {
			index = reg as u32 - Register::AL as u32;
			if Register::AH <= reg && reg <= Register::BH {
				index -= 4;
			}
			if hash.contains(&(Register::RAX + index)) {
				continue;
			}
			if hash.contains(&(Register::EAX + index)) {
				continue;
			}
			if hash.contains(&(Register::AX + index)) {
				continue;
			}
		} else if Register::YMM0 <= reg && reg <= IcedConstants::YMM_LAST {
			index = reg as u32 - Register::YMM0 as u32;
			if hash.contains(&(Register::ZMM0 + index)) {
				continue;
			}
		} else if Register::XMM0 <= reg && reg <= IcedConstants::XMM_LAST {
			index = reg as u32 - Register::XMM0 as u32;
			if hash.contains(&(Register::ZMM0 + index)) {
				continue;
			}
			if hash.contains(&(Register::YMM0 + index)) {
				continue;
			}
		}
		let _ = hash.insert(reg);
	}

	for info in &LOW_REGS {
		if hash.contains(&info.0) && hash.contains(&info.1) {
			let _ = hash.remove(&info.0);
			let _ = hash.remove(&info.1);
			let _ = hash.insert(info.2);
		}
	}

	hash.into_iter().collect()
}

static LOW_REGS: [(Register, Register, Register); 4] = [
	(Register::AL, Register::AH, Register::AX),
	(Register::CL, Register::CH, Register::CX),
	(Register::DL, Register::DH, Register::DX),
	(Register::BL, Register::BH, Register::BX),
];

#[must_use]
fn get_register_group_order(reg: Register) -> i32 {
	if Register::RAX <= reg && reg <= Register::R15 {
		0
	} else if Register::EAX <= reg && reg <= Register::R15D {
		1
	} else if Register::AX <= reg && reg <= Register::R15W {
		2
	} else if Register::AL <= reg && reg <= Register::R15L {
		3
	} else if Register::ZMM0 <= reg && reg <= IcedConstants::ZMM_LAST {
		4
	} else if Register::YMM0 <= reg && reg <= IcedConstants::YMM_LAST {
		5
	} else if Register::XMM0 <= reg && reg <= IcedConstants::XMM_LAST {
		6
	} else {
		-1
	}
}

#[test]
fn memory_size_info() {
	let mut path = get_instr_info_unit_tests_dir();
	path.push("MemorySizeInfo.txt");
	let test_cases: Vec<_> = MemorySizeInfoTestParser::new(&path).into_iter().collect();
	let h: HashSet<MemorySize> = test_cases.iter().map(|a| a.memory_size).collect();
	// Make sure every value is tested
	assert_eq!(h.len(), IcedConstants::MEMORY_SIZE_ENUM_COUNT);
	// Make sure there are no dupes
	assert_eq!(test_cases.len(), IcedConstants::MEMORY_SIZE_ENUM_COUNT);
	for tc in &test_cases {
		let info = tc.memory_size.info();
		assert_eq!(info.memory_size(), tc.memory_size);
		assert_eq!(info.size(), tc.size);
		assert_eq!(info.element_size(), tc.element_size);
		assert_eq!(info.element_type(), tc.element_type);
		assert_eq!(info.is_signed(), (tc.flags & MemorySizeFlags::SIGNED) != 0);
		assert_eq!(info.is_broadcast(), (tc.flags & MemorySizeFlags::BROADCAST) != 0);
		assert_eq!(info.is_packed(), (tc.flags & MemorySizeFlags::PACKED) != 0);
		assert_eq!(info.element_count(), tc.element_count);

		assert_eq!(tc.memory_size.size(), tc.size);
		assert_eq!(tc.memory_size.element_size(), tc.element_size);
		assert_eq!(tc.memory_size.element_type(), tc.element_type);
		assert_eq!(tc.memory_size.element_type_info().memory_size(), tc.element_type);
		assert_eq!(tc.memory_size.is_signed(), (tc.flags & MemorySizeFlags::SIGNED) != 0);
		assert_eq!(tc.memory_size.is_packed(), (tc.flags & MemorySizeFlags::PACKED) != 0);
		assert_eq!(tc.memory_size.is_broadcast(), (tc.flags & MemorySizeFlags::BROADCAST) != 0);
		assert_eq!(tc.memory_size.element_count(), tc.element_count);
	}
}

#[test]
fn register_info() {
	let mut path = get_instr_info_unit_tests_dir();
	path.push("RegisterInfo.txt");
	let test_cases: Vec<_> = RegisterInfoTestParser::new(&path).into_iter().collect();
	let h: HashSet<Register> = test_cases.iter().map(|a| a.register).collect();
	// Make sure every value is tested
	assert_eq!(h.len(), IcedConstants::REGISTER_ENUM_COUNT);
	// Make sure there are no dupes
	assert_eq!(test_cases.len(), IcedConstants::REGISTER_ENUM_COUNT);
	for tc in &test_cases {
		let info = tc.register.info();
		assert_eq!(info.register(), tc.register);
		assert_eq!(info.base(), tc.base);
		assert_eq!(info.number(), tc.number);
		assert_eq!(info.full_register(), tc.full_register);
		assert_eq!(info.full_register32(), tc.full_register32);
		assert_eq!(info.size(), tc.size);

		assert_eq!(tc.register.base(), tc.base);
		assert_eq!(tc.register.number(), tc.number);
		assert_eq!(tc.register.full_register(), tc.full_register);
		assert_eq!(tc.register.full_register32(), tc.full_register32);
		assert_eq!(tc.register.size(), tc.size);

		const ALL_FLAGS: u32 = RegisterFlags::SEGMENT_REGISTER
			| RegisterFlags::GPR
			| RegisterFlags::GPR8
			| RegisterFlags::GPR16
			| RegisterFlags::GPR32
			| RegisterFlags::GPR64
			| RegisterFlags::XMM
			| RegisterFlags::YMM
			| RegisterFlags::ZMM
			| RegisterFlags::VECTOR_REGISTER
			| RegisterFlags::IP
			| RegisterFlags::K
			| RegisterFlags::BND
			| RegisterFlags::CR
			| RegisterFlags::DR
			| RegisterFlags::TR
			| RegisterFlags::ST
			| RegisterFlags::MM
			| RegisterFlags::TMM;
		// If it fails, update the flags above and the code below, eg. add a is_tmm() test
		assert_eq!(tc.flags & ALL_FLAGS, tc.flags);

		assert_eq!(tc.register.is_segment_register(), (tc.flags & RegisterFlags::SEGMENT_REGISTER) != 0);
		assert_eq!(tc.register.is_gpr(), (tc.flags & RegisterFlags::GPR) != 0);
		assert_eq!(tc.register.is_gpr8(), (tc.flags & RegisterFlags::GPR8) != 0);
		assert_eq!(tc.register.is_gpr16(), (tc.flags & RegisterFlags::GPR16) != 0);
		assert_eq!(tc.register.is_gpr32(), (tc.flags & RegisterFlags::GPR32) != 0);
		assert_eq!(tc.register.is_gpr64(), (tc.flags & RegisterFlags::GPR64) != 0);
		assert_eq!(tc.register.is_xmm(), (tc.flags & RegisterFlags::XMM) != 0);
		assert_eq!(tc.register.is_ymm(), (tc.flags & RegisterFlags::YMM) != 0);
		assert_eq!(tc.register.is_zmm(), (tc.flags & RegisterFlags::ZMM) != 0);
		assert_eq!(tc.register.is_vector_register(), (tc.flags & RegisterFlags::VECTOR_REGISTER) != 0);
		assert_eq!(tc.register.is_ip(), (tc.flags & RegisterFlags::IP) != 0);
		assert_eq!(tc.register.is_k(), (tc.flags & RegisterFlags::K) != 0);
		assert_eq!(tc.register.is_bnd(), (tc.flags & RegisterFlags::BND) != 0);
		assert_eq!(tc.register.is_cr(), (tc.flags & RegisterFlags::CR) != 0);
		assert_eq!(tc.register.is_dr(), (tc.flags & RegisterFlags::DR) != 0);
		assert_eq!(tc.register.is_tr(), (tc.flags & RegisterFlags::TR) != 0);
		assert_eq!(tc.register.is_st(), (tc.flags & RegisterFlags::ST) != 0);
		assert_eq!(tc.register.is_mm(), (tc.flags & RegisterFlags::MM) != 0);
		assert_eq!(tc.register.is_tmm(), (tc.flags & RegisterFlags::TMM) != 0);
	}
}

#[test]
fn is_branch_call() {
	let data = &*MISC_TESTS_DATA;
	let jcc_short = &data.jcc_short;
	let jcx_short = &data.jrcxz;
	let jmp_near = &data.jmp_near;
	let jmp_far = &data.jmp_far;
	let jmp_short = &data.jmp_short;
	let jmp_near_indirect = &data.jmp_near_indirect;
	let jmp_far_indirect = &data.jmp_far_indirect;
	let jcc_near = &data.jcc_near;
	let call_far = &data.call_far;
	let call_near = &data.call_near;
	let call_near_indirect = &data.call_near_indirect;
	let call_far_indirect = &data.call_far_indirect;
	#[cfg(feature = "mvex")]
	let jkcc_short = &data.jkcc_short;
	#[cfg(feature = "mvex")]
	let jkcc_near = &data.jkcc_near;
	let loop_ = &data.loop_;

	for code in Code::values() {
		let mut instr = Instruction::default();
		instr.set_code(code);

		assert_eq!(code.is_jcc_short_or_near(), jcc_short.contains(&code) || jcc_near.contains(&code));
		assert_eq!(instr.is_jcc_short_or_near(), code.is_jcc_short_or_near());

		assert_eq!(code.is_jcc_near(), jcc_near.contains(&code));
		assert_eq!(instr.is_jcc_near(), code.is_jcc_near());

		assert_eq!(code.is_jcc_short(), jcc_short.contains(&code));
		assert_eq!(instr.is_jcc_short(), code.is_jcc_short());

		assert_eq!(code.is_jcx_short(), jcx_short.contains(&code));
		assert_eq!(instr.is_jcx_short(), code.is_jcx_short());

		assert_eq!(code.is_jmp_short(), jmp_short.contains(&code));
		assert_eq!(instr.is_jmp_short(), code.is_jmp_short());

		assert_eq!(code.is_jmp_near(), jmp_near.contains(&code));
		assert_eq!(instr.is_jmp_near(), code.is_jmp_near());

		assert_eq!(code.is_jmp_short_or_near(), jmp_short.contains(&code) || jmp_near.contains(&code));
		assert_eq!(instr.is_jmp_short_or_near(), code.is_jmp_short_or_near());

		assert_eq!(code.is_jmp_far(), jmp_far.contains(&code));
		assert_eq!(instr.is_jmp_far(), code.is_jmp_far());

		assert_eq!(code.is_call_near(), call_near.contains(&code));
		assert_eq!(instr.is_call_near(), code.is_call_near());

		assert_eq!(code.is_call_far(), call_far.contains(&code));
		assert_eq!(instr.is_call_far(), code.is_call_far());

		assert_eq!(code.is_jmp_near_indirect(), jmp_near_indirect.contains(&code));
		assert_eq!(instr.is_jmp_near_indirect(), code.is_jmp_near_indirect());

		assert_eq!(code.is_jmp_far_indirect(), jmp_far_indirect.contains(&code));
		assert_eq!(instr.is_jmp_far_indirect(), code.is_jmp_far_indirect());

		assert_eq!(code.is_call_near_indirect(), call_near_indirect.contains(&code));
		assert_eq!(instr.is_call_near_indirect(), code.is_call_near_indirect());

		assert_eq!(code.is_call_far_indirect(), call_far_indirect.contains(&code));
		assert_eq!(instr.is_call_far_indirect(), code.is_call_far_indirect());

		#[cfg(feature = "mvex")]
		{
			assert_eq!(code.is_jkcc_short_or_near(), jkcc_short.contains(&code) || jkcc_near.contains(&code));
			assert_eq!(instr.is_jkcc_short_or_near(), code.is_jkcc_short_or_near());

			assert_eq!(code.is_jkcc_near(), jkcc_near.contains(&code));
			assert_eq!(instr.is_jkcc_near(), code.is_jkcc_near());

			assert_eq!(code.is_jkcc_short(), jkcc_short.contains(&code));
			assert_eq!(instr.is_jkcc_short(), code.is_jkcc_short());
		}

		assert_eq!(loop_.contains(&code), code.is_loop() || code.is_loopcc());
		assert_eq!(code.is_loop(), instr.is_loop());
		assert_eq!(code.is_loopcc(), instr.is_loopcc());
	}
}

#[test]
fn verify_negate_condition_code() {
	let data = &*MISC_TESTS_DATA;

	let mut to_negated_code_value: HashMap<Code, Code> = HashMap::new();
	to_negated_code_value.extend(data.jcc_short_infos.iter().map(|a| (a.0, a.1)));
	to_negated_code_value.extend(data.jcc_near_infos.iter().map(|a| (a.0, a.1)));
	to_negated_code_value.extend(data.setcc_infos.iter().map(|a| (a.0, a.1)));
	to_negated_code_value.extend(data.cmovcc_infos.iter().map(|a| (a.0, a.1)));
	to_negated_code_value.extend(data.cmpccxadd_infos.iter().map(|a| (a.0, a.1)));
	to_negated_code_value.extend(data.loopcc_infos.iter().map(|a| (a.0, a.1)));
	to_negated_code_value.extend(data.jkcc_short_infos.iter().map(|a| (a.0, a.1)));
	to_negated_code_value.extend(data.jkcc_near_infos.iter().map(|a| (a.0, a.1)));

	for code in Code::values() {
		let mut instr = Instruction::default();
		instr.set_code(code);

		let negated = *to_negated_code_value.get(&code).unwrap_or(&code);

		assert_eq!(code.negate_condition_code(), negated);
		instr.negate_condition_code();
		assert_eq!(instr.code(), negated);
	}
}

#[test]
fn verify_to_short_branch() {
	let data = &*MISC_TESTS_DATA;

	let mut as_short_branch: HashMap<Code, Code> = HashMap::new();
	as_short_branch.extend(data.jcc_near_infos.iter().map(|a| (a.0, a.2)));
	as_short_branch.extend(data.jmp_infos.iter().map(|a| (a.1, a.0)));
	as_short_branch.extend(data.jkcc_near_infos.iter().map(|a| (a.0, a.2)));

	for code in Code::values() {
		let mut instr = Instruction::default();
		instr.set_code(code);

		let short = *as_short_branch.get(&code).unwrap_or(&code);

		assert_eq!(code.as_short_branch(), short);
		instr.as_short_branch();
		assert_eq!(instr.code(), short);
	}
}

#[test]
fn verify_to_near_branch() {
	let data = &*MISC_TESTS_DATA;

	let mut as_near_branch: HashMap<Code, Code> = HashMap::new();
	as_near_branch.extend(data.jcc_short_infos.iter().map(|a| (a.0, a.2)));
	as_near_branch.extend(data.jmp_infos.iter().map(|a| (a.0, a.1)));
	as_near_branch.extend(data.jkcc_short_infos.iter().map(|a| (a.0, a.2)));

	for code in Code::values() {
		let mut instr = Instruction::default();
		instr.set_code(code);

		let near = *as_near_branch.get(&code).unwrap_or(&code);

		assert_eq!(code.as_near_branch(), near);
		instr.as_near_branch();
		assert_eq!(instr.code(), near);
	}
}

#[test]
fn verify_condition_code() {
	let data = &*MISC_TESTS_DATA;

	let mut to_condition_code: HashMap<Code, ConditionCode> = HashMap::new();
	to_condition_code.extend(data.jcc_short_infos.iter().map(|a| (a.0, a.3)));
	to_condition_code.extend(data.jcc_near_infos.iter().map(|a| (a.0, a.3)));
	to_condition_code.extend(data.setcc_infos.iter().map(|a| (a.0, a.2)));
	to_condition_code.extend(data.cmovcc_infos.iter().map(|a| (a.0, a.2)));
	to_condition_code.extend(data.cmpccxadd_infos.iter().map(|a| (a.0, a.2)));
	to_condition_code.extend(data.loopcc_infos.iter().map(|a| (a.0, a.2)));
	to_condition_code.extend(data.jkcc_short_infos.iter().map(|a| (a.0, a.3)));
	to_condition_code.extend(data.jkcc_near_infos.iter().map(|a| (a.0, a.3)));

	for code in Code::values() {
		let mut instr = Instruction::default();
		instr.set_code(code);

		let cc = *to_condition_code.get(&code).unwrap_or(&ConditionCode::None);

		assert_eq!(code.condition_code(), cc);
		assert_eq!(instr.condition_code(), cc);
	}
}

#[test]
fn verify_string_instr() {
	let data = &*MISC_TESTS_DATA;
	let string = &data.string;

	for code in Code::values() {
		let mut instr = Instruction::default();
		instr.set_code(code);

		assert_eq!(code.is_string_instruction(), string.contains(&code));
		assert_eq!(instr.is_string_instruction(), code.is_string_instruction());
	}
}

#[test]
fn verify_condition_code_values_are_in_correct_order() {
	const _: () = assert!(ConditionCode::None as u32 == 0);
	const _: () = assert!(ConditionCode::o as u32 == 1);
	const _: () = assert!(ConditionCode::no as u32 == 2);
	const _: () = assert!(ConditionCode::b as u32 == 3);
	const _: () = assert!(ConditionCode::ae as u32 == 4);
	const _: () = assert!(ConditionCode::e as u32 == 5);
	const _: () = assert!(ConditionCode::ne as u32 == 6);
	const _: () = assert!(ConditionCode::be as u32 == 7);
	const _: () = assert!(ConditionCode::a as u32 == 8);
	const _: () = assert!(ConditionCode::s as u32 == 9);
	const _: () = assert!(ConditionCode::ns as u32 == 10);
	const _: () = assert!(ConditionCode::p as u32 == 11);
	const _: () = assert!(ConditionCode::np as u32 == 12);
	const _: () = assert!(ConditionCode::l as u32 == 13);
	const _: () = assert!(ConditionCode::ge as u32 == 14);
	const _: () = assert!(ConditionCode::le as u32 == 15);
	const _: () = assert!(ConditionCode::g as u32 == 16);
}

#[test]
fn make_sure_all_code_values_are_tested() {
	let mut tested = [false; IcedConstants::CODE_ENUM_COUNT];
	for bitness in &[16u32, 32, 64] {
		for tc in get_instr_info_test_cases(*bitness) {
			tested[tc.code as usize] = true;
		}
	}

	let mut s = String::new();
	let mut missing = 0;
	let code_names = code_names();
	for i in tested.iter().enumerate() {
		if !*i.1 && !is_ignored_code(code_names[i.0]) {
			write!(s, "{} ", code_names[i.0]).unwrap();
			missing += 1;
		}
	}
	assert_eq!(format!("{} ins ", missing) + &s, "0 ins ".to_string());
}

#[test]
fn verify_used_memory_size() {
	const _: () = assert!(mem::size_of::<UsedMemory>() == 16);
}
