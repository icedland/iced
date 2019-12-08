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

mod info_test_case;
mod test_parser;

use self::info_test_case::*;
use self::test_parser::*;
use super::super::iced_constants::IcedConstants;
use super::super::test_utils::from_str_conv::to_vec_u8;
use super::super::test_utils::*;
use super::super::*;
use super::factory::*;
use std::cmp::Ordering;
use std::collections::HashSet;
use std::mem;

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
	// std::mem::size_of() is a const func since rustc 1.24.0
	debug_assert_eq!(16, mem::size_of::<UsedMemory>());
	let mut path = get_instr_info_unit_tests_dir();
	path.push(format!("InstructionInfoTest_{}.txt", bitness));
	let mut factory = InstructionInfoFactory::new();
	for tc in InstrInfoTestParser::new(bitness, &path) {
		test_info_core(&tc, &mut factory);
	}
}

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
		} else if tc.code >= Code::DeclareByte {
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
				_ => panic!(),
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
					_ => panic!(),
				}
			} else {
				panic!();
			}
		}
	} else {
		let mut decoder = create_decoder(tc.bitness, &code_bytes, tc.decoder_options).0;
		instr = decoder.decode();
	}
	assert_eq!(tc.code, instr.code());

	assert_eq!(tc.stack_pointer_increment, instr.stack_pointer_increment());

	let info = instr.info();
	assert_eq!(tc.encoding, info.encoding());
	assert_eq!(tc.cpuid_features, info.cpuid_features());
	assert_eq!(tc.rflags_read, info.rflags_read());
	assert_eq!(tc.rflags_undefined, info.rflags_undefined());
	assert_eq!(tc.rflags_written, info.rflags_written());
	assert_eq!(tc.rflags_cleared, info.rflags_cleared());
	assert_eq!(tc.rflags_set, info.rflags_set());
	assert_eq!(tc.is_privileged, info.is_privileged());
	assert_eq!(tc.is_protected_mode, info.is_protected_mode());
	assert_eq!(tc.is_stack_instruction, info.is_stack_instruction());
	assert_eq!(tc.is_save_restore_instruction, info.is_save_restore_instruction());
	assert_eq!(tc.flow_control, info.flow_control());
	assert_eq!(tc.op0_access, info.op0_access());
	assert_eq!(tc.op1_access, info.op1_access());
	assert_eq!(tc.op2_access, info.op2_access());
	assert_eq!(tc.op3_access, info.op3_access());
	assert_eq!(tc.op4_access, info.op4_access());
	assert!(tc.used_memory.iter().collect::<HashSet<_>>() == info.used_memory().iter().collect::<HashSet<_>>());
	assert_eq!(
		get_used_registers(tc.used_registers.iter()),
		get_used_registers(info.used_registers().iter())
	);
	assert_eq!(info.used_memory(), &instr.used_memory());
	assert_eq!(info.used_registers(), &instr.used_registers());

	const_assert_eq!(5, IcedConstants::MAX_OP_COUNT);
	debug_assert!(instr.op_count() <= IcedConstants::MAX_OP_COUNT);
	for i in 0..instr.op_count() {
		match i {
			0 => assert_eq!(tc.op0_access, info.op_access(i)),
			1 => assert_eq!(tc.op1_access, info.op_access(i)),
			2 => assert_eq!(tc.op2_access, info.op_access(i)),
			3 => assert_eq!(tc.op3_access, info.op_access(i)),
			4 => assert_eq!(tc.op4_access, info.op_access(i)),
			_ => panic!(),
		}
	}
	for i in instr.op_count()..IcedConstants::MAX_OP_COUNT {
		assert_eq!(OpAccess::None, info.op_access(i));
	}

	assert_eq!(
		RflagsBits::NONE,
		info.rflags_written() & (info.rflags_cleared() | info.rflags_set() | info.rflags_undefined())
	);
	assert_eq!(
		RflagsBits::NONE,
		info.rflags_cleared() & (info.rflags_written() | info.rflags_set() | info.rflags_undefined())
	);
	assert_eq!(
		RflagsBits::NONE,
		info.rflags_set() & (info.rflags_written() | info.rflags_cleared() | info.rflags_undefined())
	);
	assert_eq!(
		RflagsBits::NONE,
		info.rflags_undefined() & (info.rflags_written() | info.rflags_cleared() | info.rflags_set())
	);
	assert_eq!(
		info.rflags_written() | info.rflags_cleared() | info.rflags_set() | info.rflags_undefined(),
		info.rflags_modified()
	);

	let mut factory2 = InstructionInfoFactory::new();
	let info2 = factory2.info(&instr);
	check_equal(&info, &info2, true, true);
	let mut factory2 = InstructionInfoFactory::new();
	let info2 = factory2.info_options(&instr, InstructionInfoOptions::NONE);
	check_equal(&info, &info2, true, true);
	let mut factory2 = InstructionInfoFactory::new();
	let info2 = factory2.info_options(&instr, InstructionInfoOptions::NO_MEMORY_USAGE);
	check_equal(&info, &info2, true, false);
	let mut factory2 = InstructionInfoFactory::new();
	let info2 = factory2.info_options(&instr, InstructionInfoOptions::NO_REGISTER_USAGE);
	check_equal(&info, &info2, false, true);
	let mut factory2 = InstructionInfoFactory::new();
	let info2 = factory2.info_options(
		&instr,
		InstructionInfoOptions::NO_REGISTER_USAGE | InstructionInfoOptions::NO_MEMORY_USAGE,
	);
	check_equal(&info, &info2, false, false);

	{
		let info2 = factory.info(&instr);
		check_equal(&info, &info2, true, true);
	}
	{
		let info2 = factory.info_options(&instr, InstructionInfoOptions::NONE);
		check_equal(&info, &info2, true, true);
	}
	{
		let info2 = factory.info_options(&instr, InstructionInfoOptions::NO_MEMORY_USAGE);
		check_equal(&info, &info2, true, false);
	}
	{
		let info2 = factory.info_options(&instr, InstructionInfoOptions::NO_REGISTER_USAGE);
		check_equal(&info, &info2, false, true);
	}
	{
		let info2 = factory.info_options(
			&instr,
			InstructionInfoOptions::NO_REGISTER_USAGE | InstructionInfoOptions::NO_MEMORY_USAGE,
		);
		check_equal(&info, &info2, false, false);
	}

	let info2 = instr.info_options(InstructionInfoOptions::NONE);
	check_equal(&info, &info2, true, true);
	let info2 = instr.info_options(InstructionInfoOptions::NO_MEMORY_USAGE);
	check_equal(&info, &info2, true, false);
	let info2 = instr.info_options(InstructionInfoOptions::NO_REGISTER_USAGE);
	check_equal(&info, &info2, false, true);
	let info2 = instr.info_options(InstructionInfoOptions::NO_REGISTER_USAGE | InstructionInfoOptions::NO_MEMORY_USAGE);
	check_equal(&info, &info2, false, false);

	assert_eq!(info.encoding(), instr.code().encoding());
	#[cfg(feature = "ENCODER")]
	{
		//TODO: enable this when the OpCodeInfo struct exists
		//assert_eq!(tc.code.to_op_code().encoding(), instr.code().encoding());
	}
	let mut cf = instr.code().cpuid_features();
	if cf.len() == 1
		&& cf[0] == CpuidFeature::AVX
		&& instr.op1_kind() == OpKind::Register
		&& (tc.code == Code::VEX_Vbroadcastss_xmm_xmmm32
			|| tc.code == Code::VEX_Vbroadcastss_ymm_xmmm32
			|| tc.code == Code::VEX_Vbroadcastsd_ymm_xmmm64)
	{
		cf = &CPUID_FEATURE_AVX2;
	}
	assert_eq!(info.cpuid_features(), cf);
	assert_eq!(info.flow_control(), instr.code().flow_control());
	assert_eq!(info.is_protected_mode(), instr.code().is_protected_mode());
	assert_eq!(info.is_privileged(), instr.code().is_privileged());
	assert_eq!(info.is_stack_instruction(), instr.code().is_stack_instruction());
	assert_eq!(info.is_save_restore_instruction(), instr.code().is_save_restore_instruction());

	assert_eq!(info.encoding(), instr.encoding());
	assert_eq!(info.cpuid_features(), instr.cpuid_features());
	assert_eq!(info.flow_control(), instr.flow_control());
	assert_eq!(info.is_protected_mode(), instr.is_protected_mode());
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
static CPUID_FEATURE_AVX2: [CpuidFeature; 1] = [CpuidFeature::AVX2];

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
	assert_eq!(info1.is_protected_mode(), info2.is_protected_mode());
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
			OpAccess::None | OpAccess::NoMemAccess => panic!(),
		}
	}

	let mut h: HashSet<UsedRegister> = HashSet::new();
	h.extend(get_registers(read).into_iter().map(|reg| UsedRegister {
		register: reg,
		access: OpAccess::Read,
	}));
	h.extend(get_registers(write).into_iter().map(|reg| UsedRegister {
		register: reg,
		access: OpAccess::Write,
	}));
	h.extend(get_registers(cond_read).into_iter().map(|reg| UsedRegister {
		register: reg,
		access: OpAccess::CondRead,
	}));
	h.extend(get_registers(cond_write).into_iter().map(|reg| UsedRegister {
		register: reg,
		access: OpAccess::CondWrite,
	}));
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
		let mut c = get_register_group_order(*x) - get_register_group_order(*y);
		if c < 0 {
			Ordering::Less
		} else if c > 0 {
			Ordering::Greater
		} else {
			c = *x as i32 - *y as i32;
			if c < 0 {
				Ordering::Less
			} else if c > 0 {
				Ordering::Greater
			} else {
				Ordering::Equal
			}
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
