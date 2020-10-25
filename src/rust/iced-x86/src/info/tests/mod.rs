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

mod constants;
mod info_test_case;
mod mem_size_test_case;
mod mem_size_test_parser;
mod misc_test_data;
mod reg_info_test_case;
mod reg_test_parser;
mod test_parser;
mod va;

use self::constants::*;
use self::info_test_case::*;
use self::mem_size_test_parser::*;
use self::misc_test_data::*;
use self::reg_test_parser::*;
use self::test_parser::*;
use super::super::iced_constants::IcedConstants;
use super::super::test_utils::from_str_conv::*;
use super::super::test_utils::*;
use super::super::*;
use super::factory::*;
#[cfg(not(feature = "std"))]
use alloc::string::String;
#[cfg(not(feature = "std"))]
use alloc::vec::Vec;
use core::cmp::Ordering;
use core::fmt::Write;
use core::mem;
#[cfg(not(feature = "std"))]
use hashbrown::{HashMap, HashSet};
#[cfg(feature = "std")]
use std::collections::{HashMap, HashSet};

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
		16 => &*INSTR_INFO_16,
		32 => &*INSTR_INFO_32,
		64 => &*INSTR_INFO_64,
		_ => panic!(),
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
			assert_eq!(64, tc.bitness);
			instr.set_code_size(CodeSize::Code64);
			match tc.code {
				Code::DeclareByte => {
					assert_eq!("66", tc.hex_bytes);
					instr.set_declare_byte_value(0, 0x66);
				}
				Code::DeclareWord => {
					assert_eq!("6644", tc.hex_bytes);
					instr.set_declare_word_value(0, 0x4466);
				}
				Code::DeclareDword => {
					assert_eq!("664422EE", tc.hex_bytes);
					instr.set_declare_dword_value(0, 0xEE22_4466);
				}
				Code::DeclareQword => {
					assert_eq!("664422EE12345678", tc.hex_bytes);
					instr.set_declare_qword_value(0, 0x7856_3412_EE22_4466);
				}
				_ => unreachable!(),
			}
		} else {
			let mut decoder = create_decoder(tc.bitness, &code_bytes, tc.decoder_options).0;
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
		let mut decoder = create_decoder(tc.bitness, &code_bytes, tc.decoder_options).0;
		instr = decoder.decode();
	}
	assert_eq!(tc.code, instr.code());

	assert_eq!(tc.stack_pointer_increment, instr.stack_pointer_increment());

	let mut factory1 = InstructionInfoFactory::new();
	let info = factory1.info(&instr);
	assert_eq!(tc.encoding, info.encoding());
	assert_eq!(tc.cpuid_features, info.cpuid_features());
	assert_eq!(tc.rflags_read, info.rflags_read());
	assert_eq!(tc.rflags_undefined, info.rflags_undefined());
	assert_eq!(tc.rflags_written, info.rflags_written());
	assert_eq!(tc.rflags_cleared, info.rflags_cleared());
	assert_eq!(tc.rflags_set, info.rflags_set());
	assert_eq!(tc.is_privileged, info.is_privileged());
	assert_eq!(tc.is_stack_instruction, info.is_stack_instruction());
	assert_eq!(tc.is_save_restore_instruction, info.is_save_restore_instruction());
	assert_eq!(tc.flow_control, info.flow_control());
	assert_eq!(tc.op0_access, info.op0_access());
	assert_eq!(tc.op1_access, info.op1_access());
	assert_eq!(tc.op2_access, info.op2_access());
	assert_eq!(tc.op3_access, info.op3_access());
	assert_eq!(tc.op4_access, info.op4_access());
	let fpu = instr.fpu_stack_increment_info();
	assert_eq!(tc.fpu_top_increment, fpu.increment());
	assert_eq!(tc.fpu_conditional_top, fpu.conditional());
	assert_eq!(tc.fpu_writes_top, fpu.writes_top());
	assert!(tc.used_memory.iter().collect::<HashSet<_>>() == info.used_memory().iter().collect::<HashSet<_>>());
	assert_eq!(get_used_registers(tc.used_registers.iter()), get_used_registers(info.used_registers().iter()));

	const_assert_eq!(5, IcedConstants::MAX_OP_COUNT);
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
	}
	for i in instr.op_count()..IcedConstants::MAX_OP_COUNT as u32 {
		assert_eq!(OpAccess::None, info.op_access(i));
	}

	assert_eq!(RflagsBits::NONE, info.rflags_written() & (info.rflags_cleared() | info.rflags_set() | info.rflags_undefined()));
	assert_eq!(RflagsBits::NONE, info.rflags_cleared() & (info.rflags_written() | info.rflags_set() | info.rflags_undefined()));
	assert_eq!(RflagsBits::NONE, info.rflags_set() & (info.rflags_written() | info.rflags_cleared() | info.rflags_undefined()));
	assert_eq!(RflagsBits::NONE, info.rflags_undefined() & (info.rflags_written() | info.rflags_cleared() | info.rflags_set()));
	assert_eq!(info.rflags_written() | info.rflags_cleared() | info.rflags_set() | info.rflags_undefined(), info.rflags_modified());

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

	assert_eq!(info.encoding(), instr.code().encoding());
	#[cfg(feature = "encoder")]
	{
		assert_eq!(tc.code.op_code().encoding(), instr.code().encoding());
	}
	assert_eq!(info.cpuid_features(), instr.code().cpuid_features());
	assert_eq!(info.flow_control(), instr.code().flow_control());
	assert_eq!(info.is_privileged(), instr.code().is_privileged());
	assert_eq!(info.is_stack_instruction(), instr.code().is_stack_instruction());
	assert_eq!(info.is_save_restore_instruction(), instr.code().is_save_restore_instruction());

	assert_eq!(info.encoding(), instr.encoding());
	assert_eq!(info.cpuid_features(), instr.cpuid_features());
	assert_eq!(info.flow_control(), instr.flow_control());
	assert_eq!(info.is_privileged(), instr.is_privileged());
	assert_eq!(info.is_stack_instruction(), instr.is_stack_instruction());
	assert_eq!(info.is_save_restore_instruction(), instr.is_save_restore_instruction());
	assert_eq!(info.rflags_read(), instr.rflags_read());
	assert_eq!(info.rflags_written(), instr.rflags_written());
	assert_eq!(info.rflags_cleared(), instr.rflags_cleared());
	assert_eq!(info.rflags_set(), instr.rflags_set());
	assert_eq!(info.rflags_undefined(), instr.rflags_undefined());
	assert_eq!(info.rflags_modified(), instr.rflags_modified());
}

fn check_equal(info1: &InstructionInfo, info2: &InstructionInfo, has_regs2: bool, has_mem2: bool) {
	if has_regs2 {
		assert_eq!(info1.used_registers(), info2.used_registers());
	} else {
		assert!(info2.used_registers().is_empty());
	}
	if has_mem2 {
		assert_eq!(info1.used_memory(), info2.used_memory());
	} else {
		assert!(info2.used_memory().is_empty());
	}
	assert_eq!(info1.is_privileged(), info2.is_privileged());
	assert_eq!(info1.is_stack_instruction(), info2.is_stack_instruction());
	assert_eq!(info1.is_save_restore_instruction(), info2.is_save_restore_instruction());
	assert_eq!(info1.encoding(), info2.encoding());
	assert_eq!(info1.cpuid_features(), info2.cpuid_features());
	assert_eq!(info1.flow_control(), info2.flow_control());
	assert_eq!(info1.op0_access(), info2.op0_access());
	assert_eq!(info1.op1_access(), info2.op1_access());
	assert_eq!(info1.op2_access(), info2.op2_access());
	assert_eq!(info1.op3_access(), info2.op3_access());
	assert_eq!(info1.op4_access(), info2.op4_access());
	assert_eq!(info1.rflags_read(), info2.rflags_read());
	assert_eq!(info1.rflags_written(), info2.rflags_written());
	assert_eq!(info1.rflags_cleared(), info2.rflags_cleared());
	assert_eq!(info1.rflags_set(), info2.rflags_set());
	assert_eq!(info1.rflags_undefined(), info2.rflags_undefined());
	assert_eq!(info1.rflags_modified(), info2.rflags_modified());
}

#[cfg_attr(has_must_use, must_use)]
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
		let c = (*x).register.cmp(&(*y).register);
		if c != Ordering::Equal {
			c
		} else {
			(*x).access.cmp(&(*y).access)
		}
	});
	vec
}

#[cfg_attr(has_must_use, must_use)]
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
			if hash.contains(&unsafe { mem::transmute((Register::RAX as u32 + index) as u8) }) {
				continue;
			}
		} else if Register::AX <= reg && reg <= Register::R15W {
			index = reg as u32 - Register::AX as u32;
			if hash.contains(&unsafe { mem::transmute((Register::RAX as u32 + index) as u8) }) {
				continue;
			}
			if hash.contains(&unsafe { mem::transmute((Register::EAX as u32 + index) as u8) }) {
				continue;
			}
		} else if Register::AL <= reg && reg <= Register::R15L {
			index = reg as u32 - Register::AL as u32;
			if Register::AH <= reg && reg <= Register::BH {
				index -= 4;
			}
			if hash.contains(&unsafe { mem::transmute((Register::RAX as u32 + index) as u8) }) {
				continue;
			}
			if hash.contains(&unsafe { mem::transmute((Register::EAX as u32 + index) as u8) }) {
				continue;
			}
			if hash.contains(&unsafe { mem::transmute((Register::AX as u32 + index) as u8) }) {
				continue;
			}
		} else if Register::YMM0 <= reg && reg <= IcedConstants::YMM_LAST {
			index = reg as u32 - Register::YMM0 as u32;
			if hash.contains(&unsafe { mem::transmute((Register::ZMM0 as u32 + index) as u8) }) {
				continue;
			}
		} else if Register::XMM0 <= reg && reg <= IcedConstants::XMM_LAST {
			index = reg as u32 - Register::XMM0 as u32;
			if hash.contains(&unsafe { mem::transmute((Register::ZMM0 as u32 + index) as u8) }) {
				continue;
			}
			if hash.contains(&unsafe { mem::transmute((Register::YMM0 as u32 + index) as u8) }) {
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

#[cfg_attr(has_must_use, must_use)]
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
	assert_eq!(IcedConstants::NUMBER_OF_MEMORY_SIZES, h.len());
	// Make sure there are no dupes
	assert_eq!(IcedConstants::NUMBER_OF_MEMORY_SIZES, test_cases.len());
	for tc in &test_cases {
		let info = tc.memory_size.info();
		assert_eq!(tc.memory_size, info.memory_size());
		assert_eq!(tc.size, info.size());
		assert_eq!(tc.element_size, info.element_size());
		assert_eq!(tc.element_type, info.element_type());
		assert_eq!((tc.flags & MemorySizeFlags::SIGNED) != 0, info.is_signed());
		assert_eq!((tc.flags & MemorySizeFlags::BROADCAST) != 0, info.is_broadcast());
		assert_eq!((tc.flags & MemorySizeFlags::PACKED) != 0, info.is_packed());
		assert_eq!(tc.element_count, info.element_count());

		assert_eq!(tc.size, tc.memory_size.size());
		assert_eq!(tc.element_size, tc.memory_size.element_size());
		assert_eq!(tc.element_type, tc.memory_size.element_type());
		assert_eq!(tc.element_type, tc.memory_size.element_type_info().memory_size());
		assert_eq!((tc.flags & MemorySizeFlags::SIGNED) != 0, tc.memory_size.is_signed());
		assert_eq!((tc.flags & MemorySizeFlags::PACKED) != 0, tc.memory_size.is_packed());
		assert_eq!((tc.flags & MemorySizeFlags::BROADCAST) != 0, tc.memory_size.is_broadcast());
		assert_eq!(tc.element_count, tc.memory_size.element_count());
	}
}

#[test]
fn register_info() {
	let mut path = get_instr_info_unit_tests_dir();
	path.push("RegisterInfo.txt");
	let test_cases: Vec<_> = RegisterInfoTestParser::new(&path).into_iter().collect();
	let h: HashSet<Register> = test_cases.iter().map(|a| a.register).collect();
	// Make sure every value is tested
	assert_eq!(IcedConstants::NUMBER_OF_REGISTERS, h.len());
	// Make sure there are no dupes
	assert_eq!(IcedConstants::NUMBER_OF_REGISTERS, test_cases.len());
	for tc in &test_cases {
		let info = tc.register.info();
		assert_eq!(tc.register, info.register());
		assert_eq!(tc.base, info.base());
		assert_eq!(tc.number, info.number());
		assert_eq!(tc.full_register, info.full_register());
		assert_eq!(tc.full_register32, info.full_register32());
		assert_eq!(tc.size, info.size());

		assert_eq!(tc.base, tc.register.base());
		assert_eq!(tc.number, tc.register.number());
		assert_eq!(tc.full_register, tc.register.full_register());
		assert_eq!(tc.full_register32, tc.register.full_register32());
		assert_eq!(tc.size, tc.register.size());

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
		assert_eq!(tc.flags, tc.flags & ALL_FLAGS);

		assert_eq!((tc.flags & RegisterFlags::SEGMENT_REGISTER) != 0, tc.register.is_segment_register());
		assert_eq!((tc.flags & RegisterFlags::GPR) != 0, tc.register.is_gpr());
		assert_eq!((tc.flags & RegisterFlags::GPR8) != 0, tc.register.is_gpr8());
		assert_eq!((tc.flags & RegisterFlags::GPR16) != 0, tc.register.is_gpr16());
		assert_eq!((tc.flags & RegisterFlags::GPR32) != 0, tc.register.is_gpr32());
		assert_eq!((tc.flags & RegisterFlags::GPR64) != 0, tc.register.is_gpr64());
		assert_eq!((tc.flags & RegisterFlags::XMM) != 0, tc.register.is_xmm());
		assert_eq!((tc.flags & RegisterFlags::YMM) != 0, tc.register.is_ymm());
		assert_eq!((tc.flags & RegisterFlags::ZMM) != 0, tc.register.is_zmm());
		assert_eq!((tc.flags & RegisterFlags::VECTOR_REGISTER) != 0, tc.register.is_vector_register());
		assert_eq!((tc.flags & RegisterFlags::IP) != 0, tc.register.is_ip());
		assert_eq!((tc.flags & RegisterFlags::K) != 0, tc.register.is_k());
		assert_eq!((tc.flags & RegisterFlags::BND) != 0, tc.register.is_bnd());
		assert_eq!((tc.flags & RegisterFlags::CR) != 0, tc.register.is_cr());
		assert_eq!((tc.flags & RegisterFlags::DR) != 0, tc.register.is_dr());
		assert_eq!((tc.flags & RegisterFlags::TR) != 0, tc.register.is_tr());
		assert_eq!((tc.flags & RegisterFlags::ST) != 0, tc.register.is_st());
		assert_eq!((tc.flags & RegisterFlags::MM) != 0, tc.register.is_mm());
		assert_eq!((tc.flags & RegisterFlags::TMM) != 0, tc.register.is_tmm());
	}
}

#[test]
fn is_branch_call() {
	let data = &*MISC_TESTS_DATA;
	let jcc_short = &data.jcc_short;
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

	for i in 0..IcedConstants::NUMBER_OF_CODE_VALUES {
		let code: Code = unsafe { mem::transmute(i as u16) };
		let mut instr = Instruction::default();
		instr.set_code(code);

		assert_eq!(jcc_short.contains(&code) || jcc_near.contains(&code), code.is_jcc_short_or_near());
		assert_eq!(code.is_jcc_short_or_near(), instr.is_jcc_short_or_near());

		assert_eq!(jcc_near.contains(&code), code.is_jcc_near());
		assert_eq!(code.is_jcc_near(), instr.is_jcc_near());

		assert_eq!(jcc_short.contains(&code), code.is_jcc_short());
		assert_eq!(code.is_jcc_short(), instr.is_jcc_short());

		assert_eq!(jmp_short.contains(&code), code.is_jmp_short());
		assert_eq!(code.is_jmp_short(), instr.is_jmp_short());

		assert_eq!(jmp_near.contains(&code), code.is_jmp_near());
		assert_eq!(code.is_jmp_near(), instr.is_jmp_near());

		assert_eq!(jmp_short.contains(&code) || jmp_near.contains(&code), code.is_jmp_short_or_near());
		assert_eq!(code.is_jmp_short_or_near(), instr.is_jmp_short_or_near());

		assert_eq!(jmp_far.contains(&code), code.is_jmp_far());
		assert_eq!(code.is_jmp_far(), instr.is_jmp_far());

		assert_eq!(call_near.contains(&code), code.is_call_near());
		assert_eq!(code.is_call_near(), instr.is_call_near());

		assert_eq!(call_far.contains(&code), code.is_call_far());
		assert_eq!(code.is_call_far(), instr.is_call_far());

		assert_eq!(jmp_near_indirect.contains(&code), code.is_jmp_near_indirect());
		assert_eq!(code.is_jmp_near_indirect(), instr.is_jmp_near_indirect());

		assert_eq!(jmp_far_indirect.contains(&code), code.is_jmp_far_indirect());
		assert_eq!(code.is_jmp_far_indirect(), instr.is_jmp_far_indirect());

		assert_eq!(call_near_indirect.contains(&code), code.is_call_near_indirect());
		assert_eq!(code.is_call_near_indirect(), instr.is_call_near_indirect());

		assert_eq!(call_far_indirect.contains(&code), code.is_call_far_indirect());
		assert_eq!(code.is_call_far_indirect(), instr.is_call_far_indirect());
	}
}

#[test]
fn verify_negate_condition_code() {
	let data = &*MISC_TESTS_DATA;

	let mut to_negated_code_value: HashMap<Code, Code> = HashMap::new();
	to_negated_code_value.extend(data.jcc_short_infos.iter().map(|a| ((*a).0, (*a).1)));
	to_negated_code_value.extend(data.jcc_near_infos.iter().map(|a| ((*a).0, (*a).1)));
	to_negated_code_value.extend(data.setcc_infos.iter().map(|a| ((*a).0, (*a).1)));
	to_negated_code_value.extend(data.cmovcc_infos.iter().map(|a| ((*a).0, (*a).1)));
	to_negated_code_value.extend(data.loopcc_infos.iter().map(|a| ((*a).0, (*a).1)));

	for i in 0..IcedConstants::NUMBER_OF_CODE_VALUES {
		let code: Code = unsafe { mem::transmute(i as u16) };
		let mut instr = Instruction::default();
		instr.set_code(code);

		let negated = *to_negated_code_value.get(&code).unwrap_or(&code);

		assert_eq!(negated, code.negate_condition_code());
		instr.negate_condition_code();
		assert_eq!(negated, instr.code());
	}
}

#[test]
fn verify_to_short_branch() {
	let data = &*MISC_TESTS_DATA;

	let mut as_short_branch: HashMap<Code, Code> = HashMap::new();
	as_short_branch.extend(data.jcc_near_infos.iter().map(|a| ((*a).0, (*a).2)));
	as_short_branch.extend(data.jmp_infos.iter().map(|a| ((*a).1, (*a).0)));

	for i in 0..IcedConstants::NUMBER_OF_CODE_VALUES {
		let code: Code = unsafe { mem::transmute(i as u16) };
		let mut instr = Instruction::default();
		instr.set_code(code);

		let short = *as_short_branch.get(&code).unwrap_or(&code);

		assert_eq!(short, code.as_short_branch());
		instr.as_short_branch();
		assert_eq!(short, instr.code());
	}
}

#[test]
fn verify_to_near_branch() {
	let data = &*MISC_TESTS_DATA;

	let mut as_near_branch: HashMap<Code, Code> = HashMap::new();
	as_near_branch.extend(data.jcc_short_infos.iter().map(|a| ((*a).0, (*a).2)));
	as_near_branch.extend(data.jmp_infos.iter().map(|a| ((*a).0, (*a).1)));

	for i in 0..IcedConstants::NUMBER_OF_CODE_VALUES {
		let code: Code = unsafe { mem::transmute(i as u16) };
		let mut instr = Instruction::default();
		instr.set_code(code);

		let near = *as_near_branch.get(&code).unwrap_or(&code);

		assert_eq!(near, code.as_near_branch());
		instr.as_near_branch();
		assert_eq!(near, instr.code());
	}
}

#[test]
fn verify_condition_code() {
	let data = &*MISC_TESTS_DATA;

	let mut to_condition_code: HashMap<Code, ConditionCode> = HashMap::new();
	to_condition_code.extend(data.jcc_short_infos.iter().map(|a| ((*a).0, (*a).3)));
	to_condition_code.extend(data.jcc_near_infos.iter().map(|a| ((*a).0, (*a).3)));
	to_condition_code.extend(data.setcc_infos.iter().map(|a| ((*a).0, (*a).2)));
	to_condition_code.extend(data.cmovcc_infos.iter().map(|a| ((*a).0, (*a).2)));
	to_condition_code.extend(data.loopcc_infos.iter().map(|a| ((*a).0, (*a).2)));

	for i in 0..IcedConstants::NUMBER_OF_CODE_VALUES {
		let code: Code = unsafe { mem::transmute(i as u16) };
		let mut instr = Instruction::default();
		instr.set_code(code);

		let cc = *to_condition_code.get(&code).unwrap_or(&ConditionCode::None);

		assert_eq!(cc, code.condition_code());
		assert_eq!(cc, instr.condition_code());
	}
}

#[test]
fn verify_condition_code_values_are_in_correct_order() {
	const_assert_eq!(0, ConditionCode::None as u32);
	const_assert_eq!(1, ConditionCode::o as u32);
	const_assert_eq!(2, ConditionCode::no as u32);
	const_assert_eq!(3, ConditionCode::b as u32);
	const_assert_eq!(4, ConditionCode::ae as u32);
	const_assert_eq!(5, ConditionCode::e as u32);
	const_assert_eq!(6, ConditionCode::ne as u32);
	const_assert_eq!(7, ConditionCode::be as u32);
	const_assert_eq!(8, ConditionCode::a as u32);
	const_assert_eq!(9, ConditionCode::s as u32);
	const_assert_eq!(10, ConditionCode::ns as u32);
	const_assert_eq!(11, ConditionCode::p as u32);
	const_assert_eq!(12, ConditionCode::np as u32);
	const_assert_eq!(13, ConditionCode::l as u32);
	const_assert_eq!(14, ConditionCode::ge as u32);
	const_assert_eq!(15, ConditionCode::le as u32);
	const_assert_eq!(16, ConditionCode::g as u32);
}

#[test]
fn make_sure_all_code_values_are_tested() {
	let mut tested = [false; IcedConstants::NUMBER_OF_CODE_VALUES];
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
	assert_eq!("0 ins ".to_string(), format!("{} ins ", missing) + &s);
}

#[test]
fn verify_used_memory_size() {
	// core::mem::size_of() is a const func since rustc 1.24.0
	assert_eq!(16, mem::size_of::<UsedMemory>());
}
