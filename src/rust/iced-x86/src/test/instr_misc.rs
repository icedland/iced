// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

use super::super::iced_constants::IcedConstants;
use super::super::*;
use alloc::vec::Vec;
use core::{i32, i8, mem, u16, u32, u64, u8};
use std::panic;

#[test]
fn invalid_code_value_is_zero() {
	// A 'default' Instruction should be an invalid instruction
	const_assert_eq!(0, Code::INVALID as u32);
	let instr1 = Instruction::default();
	assert_eq!(Code::INVALID, instr1.code());
	let instr2 = Instruction::new();
	assert_eq!(Code::INVALID, instr2.code());
	assert!(instr1.eq_all_bits(&instr2));
}

#[test]
#[cfg(feature = "encoder")]
#[cfg(not(feature = "no_vex"))]
fn eq_and_hash_ignore_some_fields() {
	use core::hash::{Hash, Hasher};
	use std::collections::hash_map::DefaultHasher;
	let mut instr1 = Instruction::try_with_reg_reg_mem_reg_u32(
		Code::VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm4,
		Register::XMM1,
		Register::XMM2,
		MemoryOperand::new(Register::RCX, Register::R14, 8, 0x1234_5678, 8, false, Register::FS),
		Register::XMM10,
		0xA5,
	)
	.unwrap();
	let mut instr2 = instr1;
	assert!(instr1.eq_all_bits(&instr2));
	instr1.set_code_size(CodeSize::Code32);
	instr2.set_code_size(CodeSize::Code64);
	assert!(!instr1.eq_all_bits(&instr2));
	instr1.set_len(10);
	instr2.set_len(5);
	instr1.set_ip(0x9733_3795_FA7C_EAAB);
	instr2.set_ip(0x9BE5_A3A0_7A66_FC05);
	assert_eq!(instr1, instr2);
	let mut hasher1 = DefaultHasher::new();
	let mut hasher2 = DefaultHasher::new();
	instr1.hash(&mut hasher1);
	instr2.hash(&mut hasher2);
	assert_eq!(hasher1.finish(), hasher2.finish());
}

#[test]
fn write_all_properties() {
	let mut instr = Instruction::default();

	instr.set_ip(0x8A6B_D04A_9B68_3A92);
	instr.set_ip16(u16::MIN);
	assert_eq!(u16::MIN, instr.ip16());
	assert_eq!(u16::MIN as u32, instr.ip32());
	assert_eq!(u16::MIN as u64, instr.ip());
	instr.set_ip(0x8A6B_D04A_9B68_3A92);
	instr.set_ip16(u16::MAX);
	assert_eq!(u16::MAX, instr.ip16());
	assert_eq!(u16::MAX as u32, instr.ip32());
	assert_eq!(u16::MAX as u64, instr.ip());

	instr.set_ip(0x8A6B_D04A_9B68_3A92);
	instr.set_ip32(u32::MIN);
	assert_eq!(u16::MIN, instr.ip16());
	assert_eq!(u32::MIN, instr.ip32());
	assert_eq!(u32::MIN as u64, instr.ip());
	instr.set_ip(0x8A6B_D04A_9B68_3A92);
	instr.set_ip32(u32::MAX);
	assert_eq!(u16::MAX, instr.ip16());
	assert_eq!(u32::MAX, instr.ip32());
	assert_eq!(u32::MAX as u64, instr.ip());

	instr.set_ip(u64::MIN);
	assert_eq!(u16::MIN, instr.ip16());
	assert_eq!(u32::MIN, instr.ip32());
	assert_eq!(u64::MIN, instr.ip());
	instr.set_ip(u64::MAX);
	assert_eq!(u16::MAX, instr.ip16());
	assert_eq!(u32::MAX, instr.ip32());
	assert_eq!(u64::MAX, instr.ip());

	instr.set_next_ip(0x8A6B_D04A_9B68_3A92);
	instr.set_next_ip16(u16::MIN);
	assert_eq!(u16::MIN, instr.next_ip16());
	assert_eq!(u16::MIN as u32, instr.next_ip32());
	assert_eq!(u16::MIN as u64, instr.next_ip());
	instr.set_next_ip(0x8A6B_D04A_9B68_3A92);
	instr.set_next_ip16(u16::MAX);
	assert_eq!(u16::MAX, instr.next_ip16());
	assert_eq!(u16::MAX as u32, instr.next_ip32());
	assert_eq!(u16::MAX as u64, instr.next_ip());

	instr.set_next_ip(0x8A6B_D04A_9B68_3A92);
	instr.set_next_ip32(u32::MIN);
	assert_eq!(u16::MIN, instr.next_ip16());
	assert_eq!(u32::MIN, instr.next_ip32());
	assert_eq!(u32::MIN as u64, instr.next_ip());
	instr.set_next_ip(0x8A6B_D04A_9B68_3A92);
	instr.set_next_ip32(u32::MAX);
	assert_eq!(u16::MAX, instr.next_ip16());
	assert_eq!(u32::MAX, instr.next_ip32());
	assert_eq!(u32::MAX as u64, instr.next_ip());

	instr.set_next_ip(u64::MIN);
	assert_eq!(u16::MIN, instr.next_ip16());
	assert_eq!(u32::MIN, instr.next_ip32());
	assert_eq!(u64::MIN, instr.next_ip());
	instr.set_next_ip(u64::MAX);
	assert_eq!(u16::MAX, instr.next_ip16());
	assert_eq!(u32::MAX, instr.next_ip32());
	assert_eq!(u64::MAX, instr.next_ip());

	instr.set_memory_displacement32(u32::MIN);
	assert_eq!(u32::MIN, instr.memory_displacement32());
	assert_eq!(u32::MIN as u64, instr.memory_displacement64());
	instr.set_memory_displacement32(u32::MAX);
	assert_eq!(u32::MAX, instr.memory_displacement32());
	assert_eq!(u32::MAX as u64, instr.memory_displacement64());

	instr.set_memory_displacement64(u64::MIN);
	assert_eq!(u32::MIN, instr.memory_displacement32());
	assert_eq!(u64::MIN, instr.memory_displacement64());
	instr.set_memory_displacement64(u64::MAX);
	assert_eq!(u32::MAX, instr.memory_displacement32());
	assert_eq!(u64::MAX, instr.memory_displacement64());

	instr.set_memory_displacement64(0x1234_5678_9ABC_DEF1);
	instr.set_memory_displacement32(0x5AA5_4321);
	assert_eq!(0x5AA5_4321, instr.memory_displacement32());
	assert_eq!(0x5AA5_4321, instr.memory_displacement64());

	instr.set_immediate8(u8::MIN);
	assert_eq!(u8::MIN, instr.immediate8());
	instr.set_immediate8(u8::MAX);
	assert_eq!(u8::MAX, instr.immediate8());

	instr.set_immediate8_2nd(u8::MIN);
	assert_eq!(u8::MIN, instr.immediate8_2nd());
	instr.set_immediate8_2nd(u8::MAX);
	assert_eq!(u8::MAX, instr.immediate8_2nd());

	instr.set_immediate16(u16::MIN);
	assert_eq!(u16::MIN, instr.immediate16());
	instr.set_immediate16(u16::MAX);
	assert_eq!(u16::MAX, instr.immediate16());

	instr.set_immediate32(u32::MIN);
	assert_eq!(u32::MIN, instr.immediate32());
	instr.set_immediate32(u32::MAX);
	assert_eq!(u32::MAX, instr.immediate32());

	instr.set_immediate64(u64::MIN);
	assert_eq!(u64::MIN, instr.immediate64());
	instr.set_immediate64(u64::MAX);
	assert_eq!(u64::MAX, instr.immediate64());

	instr.set_immediate8to16(i8::MIN as i16);
	assert_eq!(i8::MIN as i16, instr.immediate8to16());
	instr.set_immediate8to16(i8::MAX as i16);
	assert_eq!(i8::MAX as i16, instr.immediate8to16());

	instr.set_immediate8to32(i8::MIN as i32);
	assert_eq!(i8::MIN as i32, instr.immediate8to32());
	instr.set_immediate8to32(i8::MAX as i32);
	assert_eq!(i8::MAX as i32, instr.immediate8to32());

	instr.set_immediate8to64(i8::MIN as i64);
	assert_eq!(i8::MIN as i64, instr.immediate8to64());
	instr.set_immediate8to64(i8::MAX as i64);
	assert_eq!(i8::MAX as i64, instr.immediate8to64());

	instr.set_immediate32to64(i32::MIN as i64);
	assert_eq!(i32::MIN as i64, instr.immediate32to64());
	instr.set_immediate32to64(i32::MAX as i64);
	assert_eq!(i32::MAX as i64, instr.immediate32to64());

	instr.set_op0_kind(OpKind::NearBranch16);
	instr.set_near_branch16(u16::MIN);
	assert_eq!(u16::MIN, instr.near_branch16());
	assert_eq!(u16::MIN as u64, instr.near_branch_target());
	instr.set_near_branch16(u16::MAX);
	assert_eq!(u16::MAX, instr.near_branch16());
	assert_eq!(u16::MAX as u64, instr.near_branch_target());

	instr.set_op0_kind(OpKind::NearBranch32);
	instr.set_near_branch32(u32::MIN);
	assert_eq!(u32::MIN, instr.near_branch32());
	assert_eq!(u32::MIN as u64, instr.near_branch_target());
	instr.set_near_branch32(u32::MAX);
	assert_eq!(u32::MAX, instr.near_branch32());
	assert_eq!(u32::MAX as u64, instr.near_branch_target());

	instr.set_op0_kind(OpKind::NearBranch64);
	instr.set_near_branch64(u64::MIN);
	assert_eq!(u64::MIN, instr.near_branch64());
	assert_eq!(u64::MIN, instr.near_branch_target());
	instr.set_near_branch64(u64::MAX);
	assert_eq!(u64::MAX, instr.near_branch64());
	assert_eq!(u64::MAX, instr.near_branch_target());

	instr.set_far_branch16(u16::MIN);
	assert_eq!(u16::MIN, instr.far_branch16());
	instr.set_far_branch16(u16::MAX);
	assert_eq!(u16::MAX, instr.far_branch16());

	instr.set_far_branch32(u32::MIN);
	assert_eq!(u32::MIN, instr.far_branch32());
	instr.set_far_branch32(u32::MAX);
	assert_eq!(u32::MAX, instr.far_branch32());

	instr.set_far_branch_selector(u16::MIN);
	assert_eq!(u16::MIN, instr.far_branch_selector());
	instr.set_far_branch_selector(u16::MAX);
	assert_eq!(u16::MAX, instr.far_branch_selector());

	instr.set_has_xacquire_prefix(false);
	assert!(!instr.has_xacquire_prefix());
	instr.set_has_xacquire_prefix(true);
	assert!(instr.has_xacquire_prefix());

	instr.set_has_xrelease_prefix(false);
	assert!(!instr.has_xrelease_prefix());
	instr.set_has_xrelease_prefix(true);
	assert!(instr.has_xrelease_prefix());

	instr.set_has_rep_prefix(false);
	assert!(!instr.has_rep_prefix());
	assert!(!instr.has_repe_prefix());
	instr.set_has_rep_prefix(true);
	assert!(instr.has_rep_prefix());
	assert!(instr.has_repe_prefix());

	instr.set_has_repe_prefix(false);
	assert!(!instr.has_rep_prefix());
	assert!(!instr.has_repe_prefix());
	instr.set_has_repe_prefix(true);
	assert!(instr.has_rep_prefix());
	assert!(instr.has_repe_prefix());

	instr.set_has_repne_prefix(false);
	assert!(!instr.has_repne_prefix());
	instr.set_has_repne_prefix(true);
	assert!(instr.has_repne_prefix());

	instr.set_has_lock_prefix(false);
	assert!(!instr.has_lock_prefix());
	instr.set_has_lock_prefix(true);
	assert!(instr.has_lock_prefix());

	instr.set_is_broadcast(false);
	assert!(!instr.is_broadcast());
	instr.set_is_broadcast(true);
	assert!(instr.is_broadcast());

	instr.set_suppress_all_exceptions(false);
	assert!(!instr.suppress_all_exceptions());
	instr.set_suppress_all_exceptions(true);
	assert!(instr.suppress_all_exceptions());

	for i in 0..IcedConstants::MAX_INSTRUCTION_LENGTH + 1 {
		instr.set_len(i);
		assert_eq!(i, instr.len());
	}

	for code_size in get_code_size_values() {
		instr.set_code_size(code_size);
		assert_eq!(code_size, instr.code_size());
	}

	for code in get_code_values() {
		instr.set_code(code);
		assert_eq!(code, instr.code());
	}

	const_assert_eq!(5, IcedConstants::MAX_OP_COUNT);
	let op_kinds = get_op_kind_values();
	for &op_kind in &op_kinds {
		instr.set_op0_kind(op_kind);
		assert_eq!(op_kind, instr.op0_kind());
	}

	for &op_kind in &op_kinds {
		instr.set_op1_kind(op_kind);
		assert_eq!(op_kind, instr.op1_kind());
	}

	for &op_kind in &op_kinds {
		instr.set_op2_kind(op_kind);
		assert_eq!(op_kind, instr.op2_kind());
	}

	for &op_kind in &op_kinds {
		instr.set_op3_kind(op_kind);
		assert_eq!(op_kind, instr.op3_kind());
	}

	for &op_kind in &op_kinds {
		if op_kind == OpKind::Immediate8 {
			#[allow(deprecated)]
			{
				instr.set_op4_kind(op_kind);
			}
			instr.try_set_op4_kind(op_kind).unwrap();
			assert_eq!(op_kind, instr.op4_kind());
		} else {
			let mut instr = instr;
			assert!(instr.try_set_op4_kind(op_kind).is_err());
			#[allow(deprecated)]
			{
				assert!(panic::catch_unwind(move || instr.set_op4_kind(op_kind)).is_err());
			}
		}
	}

	for &op_kind in &op_kinds {
		#[allow(deprecated)]
		{
			instr.set_op_kind(0, op_kind);
		}
		instr.try_set_op_kind(0, op_kind).unwrap();
		assert_eq!(op_kind, instr.op0_kind());
		#[allow(deprecated)]
		{
			assert_eq!(op_kind, instr.op_kind(0));
		}
		assert_eq!(op_kind, instr.try_op_kind(0).unwrap());
	}

	for &op_kind in &op_kinds {
		#[allow(deprecated)]
		{
			instr.set_op_kind(1, op_kind);
		}
		instr.try_set_op_kind(1, op_kind).unwrap();
		assert_eq!(op_kind, instr.op1_kind());
		#[allow(deprecated)]
		{
			assert_eq!(op_kind, instr.op_kind(1));
		}
		assert_eq!(op_kind, instr.try_op_kind(1).unwrap());
	}

	for &op_kind in &op_kinds {
		#[allow(deprecated)]
		{
			instr.set_op_kind(2, op_kind);
		}
		instr.try_set_op_kind(2, op_kind).unwrap();
		assert_eq!(op_kind, instr.op2_kind());
		#[allow(deprecated)]
		{
			assert_eq!(op_kind, instr.op_kind(2));
		}
		assert_eq!(op_kind, instr.try_op_kind(2).unwrap());
	}

	for &op_kind in &op_kinds {
		#[allow(deprecated)]
		{
			instr.set_op_kind(3, op_kind);
		}
		instr.try_set_op_kind(3, op_kind).unwrap();
		assert_eq!(op_kind, instr.op3_kind());
		#[allow(deprecated)]
		{
			assert_eq!(op_kind, instr.op_kind(3));
		}
		assert_eq!(op_kind, instr.try_op_kind(3).unwrap());
	}

	for &op_kind in &op_kinds {
		if op_kind == OpKind::Immediate8 {
			#[allow(deprecated)]
			{
				instr.set_op_kind(4, op_kind);
			}
			instr.try_set_op_kind(4, op_kind).unwrap();
			assert_eq!(op_kind, instr.op4_kind());
			#[allow(deprecated)]
			{
				assert_eq!(op_kind, instr.op_kind(4));
			}
			assert_eq!(op_kind, instr.try_op_kind(4).unwrap());
		} else {
			let mut instr = instr;
			assert!(instr.try_set_op_kind(4, op_kind).is_err());
			#[allow(deprecated)]
			{
				assert!(panic::catch_unwind(move || instr.set_op_kind(4, op_kind)).is_err());
			}
		}
	}

	let seg_values = [Register::ES, Register::CS, Register::SS, Register::DS, Register::FS, Register::GS, Register::None];
	for &seg in &seg_values {
		instr.set_segment_prefix(seg);
		assert_eq!(seg, instr.segment_prefix());
		if instr.segment_prefix() == Register::None {
			assert!(!instr.has_segment_prefix());
		} else {
			assert!(instr.has_segment_prefix());
		}
	}

	let displ_sizes = [8, 4, 2, 1, 0];
	for &displ_size in &displ_sizes {
		instr.set_memory_displ_size(displ_size);
		assert_eq!(displ_size, instr.memory_displ_size());
	}

	let scale_values = [8, 4, 2, 1];
	for &scale_value in &scale_values {
		instr.set_memory_index_scale(scale_value);
		assert_eq!(scale_value, instr.memory_index_scale());
	}

	let register_values = get_register_values();
	for &reg in &register_values {
		instr.set_memory_base(reg);
		assert_eq!(reg, instr.memory_base());
	}

	for &reg in &register_values {
		instr.set_memory_index(reg);
		assert_eq!(reg, instr.memory_index());
	}

	for &reg in &register_values {
		instr.set_op0_register(reg);
		assert_eq!(reg, instr.op0_register());
	}

	for &reg in &register_values {
		instr.set_op1_register(reg);
		assert_eq!(reg, instr.op1_register());
	}

	for &reg in &register_values {
		instr.set_op2_register(reg);
		assert_eq!(reg, instr.op2_register());
	}

	for &reg in &register_values {
		instr.set_op3_register(reg);
		assert_eq!(reg, instr.op3_register());
	}

	for &reg in &register_values {
		if reg == Register::None {
			#[allow(deprecated)]
			{
				instr.set_op4_register(reg);
			}
			instr.try_set_op4_register(reg).unwrap();
			assert_eq!(reg, instr.op4_register());
		} else {
			let mut instr = instr;
			assert!(instr.try_set_op4_register(reg).is_err());
			#[allow(deprecated)]
			{
				assert!(panic::catch_unwind(move || instr.set_op4_register(reg)).is_err());
			}
		}
	}

	for &reg in &register_values {
		#[allow(deprecated)]
		{
			instr.set_op_register(0, reg);
		}
		instr.try_set_op_register(0, reg).unwrap();
		assert_eq!(reg, instr.op0_register());
		#[allow(deprecated)]
		{
			assert_eq!(reg, instr.op_register(0));
		}
		assert_eq!(reg, instr.try_op_register(0).unwrap());
	}

	for &reg in &register_values {
		#[allow(deprecated)]
		{
			instr.set_op_register(1, reg);
		}
		instr.try_set_op_register(1, reg).unwrap();
		assert_eq!(reg, instr.op1_register());
		#[allow(deprecated)]
		{
			assert_eq!(reg, instr.op_register(1));
		}
		assert_eq!(reg, instr.try_op_register(1).unwrap());
	}

	for &reg in &register_values {
		#[allow(deprecated)]
		{
			instr.set_op_register(2, reg);
		}
		instr.try_set_op_register(2, reg).unwrap();
		assert_eq!(reg, instr.op2_register());
		#[allow(deprecated)]
		{
			assert_eq!(reg, instr.op_register(2));
		}
		assert_eq!(reg, instr.try_op_register(2).unwrap());
	}

	for &reg in &register_values {
		#[allow(deprecated)]
		{
			instr.set_op_register(3, reg);
		}
		instr.try_set_op_register(3, reg).unwrap();
		assert_eq!(reg, instr.op3_register());
		#[allow(deprecated)]
		{
			assert_eq!(reg, instr.op_register(3));
		}
		assert_eq!(reg, instr.try_op_register(3).unwrap());
	}

	for &reg in &register_values {
		if reg == Register::None {
			#[allow(deprecated)]
			{
				instr.set_op_register(4, reg);
			}
			instr.try_set_op_register(4, reg).unwrap();
			assert_eq!(reg, instr.op4_register());
			#[allow(deprecated)]
			{
				assert_eq!(reg, instr.op_register(4));
			}
			assert_eq!(reg, instr.try_op_register(4).unwrap());
		} else {
			let mut instr = instr;
			assert!(instr.try_set_op_register(4, reg).is_err());
			#[allow(deprecated)]
			{
				assert!(panic::catch_unwind(move || instr.set_op_register(4, reg)).is_err());
			}
		}
	}

	let op_masks = [Register::K1, Register::K2, Register::K3, Register::K4, Register::K5, Register::K6, Register::K7, Register::None];
	for &op_mask in &op_masks {
		instr.set_op_mask(op_mask);
		assert_eq!(op_mask, instr.op_mask());
		assert_eq!(op_mask != Register::None, instr.has_op_mask());
	}

	instr.set_zeroing_masking(false);
	assert!(!instr.zeroing_masking());
	assert!(instr.merging_masking());
	instr.set_zeroing_masking(true);
	assert!(instr.zeroing_masking());
	assert!(!instr.merging_masking());
	instr.set_merging_masking(false);
	assert!(!instr.merging_masking());
	assert!(instr.zeroing_masking());
	instr.set_merging_masking(true);
	assert!(instr.merging_masking());
	assert!(!instr.zeroing_masking());

	for rc in get_rounding_control_values() {
		instr.set_rounding_control(rc);
		assert_eq!(rc, instr.rounding_control());
	}

	for &reg in &register_values {
		instr.set_memory_base(reg);
		assert_eq!(reg == Register::RIP || reg == Register::EIP, instr.is_ip_rel_memory_operand());
	}

	instr.set_memory_base(Register::EIP);
	instr.set_next_ip(0x1234_5670_9EDC_BA98);
	instr.set_memory_displacement64(0x8765_4321_9ABC_DEF5);
	assert!(instr.is_ip_rel_memory_operand());
	assert_eq!(0x9ABC_DEF5, instr.ip_rel_memory_address());

	instr.set_memory_base(Register::RIP);
	instr.set_next_ip(0x1234_5670_9EDC_BA98);
	instr.set_memory_displacement64(0x8765_4321_9ABC_DEF5);
	assert!(instr.is_ip_rel_memory_operand());
	assert_eq!(0x8765_4321_9ABC_DEF5, instr.ip_rel_memory_address());

	instr.set_declare_data_len(1);
	assert_eq!(1, instr.declare_data_len());
	instr.set_declare_data_len(15);
	assert_eq!(15, instr.declare_data_len());
	instr.set_declare_data_len(16);
	assert_eq!(16, instr.declare_data_len());

	fn get_code_size_values() -> Vec<CodeSize> {
		(0..IcedConstants::CODE_SIZE_ENUM_COUNT).map(|x| unsafe { mem::transmute(x as u8) }).collect()
	}

	fn get_code_values() -> Vec<Code> {
		(0..IcedConstants::CODE_ENUM_COUNT).map(|x| unsafe { mem::transmute(x as u16) }).collect()
	}

	fn get_op_kind_values() -> Vec<OpKind> {
		(0..IcedConstants::OP_KIND_ENUM_COUNT).map(|x| unsafe { mem::transmute(x as u8) }).collect()
	}

	fn get_register_values() -> Vec<Register> {
		(0..IcedConstants::REGISTER_ENUM_COUNT).map(|x| unsafe { mem::transmute(x as u8) }).collect()
	}

	fn get_rounding_control_values() -> Vec<RoundingControl> {
		(0..IcedConstants::ROUNDING_CONTROL_ENUM_COUNT).map(|x| unsafe { mem::transmute(x as u8) }).collect()
	}
}

#[test]
fn verify_get_set_immediate() {
	let mut instr = Instruction::default();

	instr.set_code(Code::Add_AL_imm8);
	instr.set_op1_kind(OpKind::Immediate8);
	#[allow(deprecated)]
	{
		instr.set_immediate_i32(1, 0x5A);
	}
	instr.try_set_immediate_i32(1, 0x5A).unwrap();
	assert_eq!(0x5A, instr.immediate(1));
	assert_eq!(0x5A, instr.try_immediate(1).unwrap());
	#[allow(deprecated)]
	{
		instr.set_immediate_i32(1, 0xA5);
	}
	instr.try_set_immediate_i32(1, 0xA5).unwrap();
	assert_eq!(0xA5, instr.immediate(1));
	assert_eq!(0xA5, instr.try_immediate(1).unwrap());

	instr.set_code(Code::Add_AX_imm16);
	instr.set_op1_kind(OpKind::Immediate16);
	#[allow(deprecated)]
	{
		instr.set_immediate_i32(1, 0x5AA5);
	}
	instr.try_set_immediate_i32(1, 0x5AA5).unwrap();
	assert_eq!(0x5AA5, instr.immediate(1));
	assert_eq!(0x5AA5, instr.try_immediate(1).unwrap());
	#[allow(deprecated)]
	{
		instr.set_immediate_i32(1, 0xA55A);
	}
	instr.try_set_immediate_i32(1, 0xA55A).unwrap();
	assert_eq!(0xA55A, instr.immediate(1));
	assert_eq!(0xA55A, instr.try_immediate(1).unwrap());

	instr.set_code(Code::Add_EAX_imm32);
	instr.set_op1_kind(OpKind::Immediate32);
	#[allow(deprecated)]
	{
		instr.set_immediate_i32(1, 0x5AA5_1234);
	}
	instr.try_set_immediate_i32(1, 0x5AA5_1234).unwrap();
	assert_eq!(0x5AA5_1234, instr.immediate(1));
	assert_eq!(0x5AA5_1234, instr.try_immediate(1).unwrap());
	#[allow(deprecated)]
	{
		instr.set_immediate_u32(1, 0xA54A_1234);
	}
	instr.try_set_immediate_u32(1, 0xA54A_1234).unwrap();
	assert_eq!(0xA54A_1234, instr.immediate(1));
	assert_eq!(0xA54A_1234, instr.try_immediate(1).unwrap());

	instr.set_code(Code::Add_RAX_imm32);
	instr.set_op1_kind(OpKind::Immediate32to64);
	#[allow(deprecated)]
	{
		instr.set_immediate_i32(1, 0x5AA5_1234);
	}
	instr.try_set_immediate_i32(1, 0x5AA5_1234).unwrap();
	assert_eq!(0x5AA5_1234, instr.immediate(1));
	assert_eq!(0x5AA5_1234, instr.try_immediate(1).unwrap());
	#[allow(deprecated)]
	{
		instr.set_immediate_u32(1, 0xA54A_1234);
	}
	instr.try_set_immediate_u32(1, 0xA54A_1234).unwrap();
	assert_eq!(0xFFFF_FFFF_A54A_1234, instr.immediate(1));
	assert_eq!(0xFFFF_FFFF_A54A_1234, instr.try_immediate(1).unwrap());

	instr.set_code(Code::Enterq_imm16_imm8);
	instr.set_op1_kind(OpKind::Immediate8_2nd);
	#[allow(deprecated)]
	{
		instr.set_immediate_i32(1, 0x5A);
	}
	instr.try_set_immediate_i32(1, 0x5A).unwrap();
	assert_eq!(0x5A, instr.immediate(1));
	assert_eq!(0x5A, instr.try_immediate(1).unwrap());
	#[allow(deprecated)]
	{
		instr.set_immediate_i32(1, 0xA5);
	}
	instr.try_set_immediate_i32(1, 0xA5).unwrap();
	assert_eq!(0xA5, instr.immediate(1));
	assert_eq!(0xA5, instr.try_immediate(1).unwrap());

	instr.set_code(Code::Adc_rm16_imm8);
	instr.set_op1_kind(OpKind::Immediate8to16);
	#[allow(deprecated)]
	{
		instr.set_immediate_i32(1, 0x5A);
	}
	instr.try_set_immediate_i32(1, 0x5A).unwrap();
	assert_eq!(0x5A, instr.immediate(1));
	assert_eq!(0x5A, instr.try_immediate(1).unwrap());
	#[allow(deprecated)]
	{
		instr.set_immediate_i32(1, 0xA5);
	}
	instr.try_set_immediate_i32(1, 0xA5).unwrap();
	assert_eq!(0xFFFF_FFFF_FFFF_FFA5, instr.immediate(1));
	assert_eq!(0xFFFF_FFFF_FFFF_FFA5, instr.try_immediate(1).unwrap());

	instr.set_code(Code::Adc_rm32_imm8);
	instr.set_op1_kind(OpKind::Immediate8to32);
	#[allow(deprecated)]
	{
		instr.set_immediate_i32(1, 0x5A);
	}
	instr.try_set_immediate_i32(1, 0x5A).unwrap();
	assert_eq!(0x5A, instr.immediate(1));
	assert_eq!(0x5A, instr.try_immediate(1).unwrap());
	#[allow(deprecated)]
	{
		instr.set_immediate_i32(1, 0xA5);
	}
	instr.try_set_immediate_i32(1, 0xA5).unwrap();
	assert_eq!(0xFFFF_FFFF_FFFF_FFA5, instr.immediate(1));
	assert_eq!(0xFFFF_FFFF_FFFF_FFA5, instr.try_immediate(1).unwrap());

	instr.set_code(Code::Adc_rm64_imm8);
	instr.set_op1_kind(OpKind::Immediate8to64);
	#[allow(deprecated)]
	{
		instr.set_immediate_i32(1, 0x5A);
	}
	instr.try_set_immediate_i32(1, 0x5A).unwrap();
	assert_eq!(0x5A, instr.immediate(1));
	assert_eq!(0x5A, instr.try_immediate(1).unwrap());
	#[allow(deprecated)]
	{
		instr.set_immediate_i32(1, 0xA5);
	}
	instr.try_set_immediate_i32(1, 0xA5).unwrap();
	assert_eq!(0xFFFF_FFFF_FFFF_FFA5, instr.immediate(1));
	assert_eq!(0xFFFF_FFFF_FFFF_FFA5, instr.try_immediate(1).unwrap());

	instr.set_code(Code::Mov_r64_imm64);
	instr.set_op1_kind(OpKind::Immediate64);
	#[allow(deprecated)]
	{
		instr.set_immediate_i64(1, 0x5AA5_1234_5678_9ABC);
	}
	instr.try_set_immediate_i64(1, 0x5AA5_1234_5678_9ABC).unwrap();
	assert_eq!(0x5AA5_1234_5678_9ABC, instr.immediate(1));
	assert_eq!(0x5AA5_1234_5678_9ABC, instr.try_immediate(1).unwrap());
	#[allow(deprecated)]
	{
		instr.set_immediate_u64(1, 0xA54A_1234_5678_9ABC);
	}
	instr.try_set_immediate_u64(1, 0xA54A_1234_5678_9ABC).unwrap();
	assert_eq!(0xA54A_1234_5678_9ABC, instr.immediate(1));
	assert_eq!(0xA54A_1234_5678_9ABC, instr.try_immediate(1).unwrap());
	#[allow(deprecated)]
	{
		instr.set_immediate_i64(1, -0x5AB5_EDCB_A987_6544);
	}
	instr.try_set_immediate_i64(1, -0x5AB5_EDCB_A987_6544).unwrap();
	assert_eq!(0xA54A_1234_5678_9ABC, instr.immediate(1));
	assert_eq!(0xA54A_1234_5678_9ABC, instr.try_immediate(1).unwrap());

	{
		let instr = instr;
		assert!(instr.try_immediate(0).is_err());
		assert!(panic::catch_unwind(move || instr.immediate(0)).is_err());
	}
	{
		let mut instr = instr;
		assert!(instr.try_set_immediate_i32(0, 0).is_err());
		#[allow(deprecated)]
		{
			assert!(panic::catch_unwind(move || instr.set_immediate_i32(0, 0)).is_err());
		}
	}
	{
		let mut instr = instr;
		assert!(instr.try_set_immediate_u32(0, 0).is_err());
		#[allow(deprecated)]
		{
			assert!(panic::catch_unwind(move || instr.set_immediate_u32(0, 0)).is_err());
		}
	}
	{
		let mut instr = instr;
		assert!(instr.try_set_immediate_i64(0, 0).is_err());
		#[allow(deprecated)]
		{
			assert!(panic::catch_unwind(move || instr.set_immediate_i64(0, 0)).is_err());
		}
	}
	{
		let mut instr = instr;
		assert!(instr.try_set_immediate_u64(0, 0).is_err());
		#[allow(deprecated)]
		{
			assert!(panic::catch_unwind(move || instr.set_immediate_u64(0, 0)).is_err());
		}
	}
}

#[test]
fn verify_instruction_size() {
	const_assert_eq!(INSTRUCTION_TOTAL_SIZE, mem::size_of::<Instruction>());
}
