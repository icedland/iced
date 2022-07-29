// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::iced_constants::IcedConstants;
use crate::*;
use core::mem;
use std::panic;

#[test]
fn invalid_code_value_is_zero() {
	// A 'default' Instruction should be an invalid instruction
	const _: () = assert!(Code::INVALID as u32 == 0);
	let instr1 = Instruction::default();
	assert_eq!(instr1.code(), Code::INVALID);
	let instr2 = Instruction::new();
	assert_eq!(instr2.code(), Code::INVALID);
	assert!(instr1.eq_all_bits(&instr2));
}

#[test]
#[cfg(feature = "encoder")]
#[cfg(not(feature = "no_vex"))]
fn eq_and_hash_ignore_some_fields() {
	use core::hash::{Hash, Hasher};
	use std::collections::hash_map::DefaultHasher;
	let mut instr1 = Instruction::with5(
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
	assert_eq!(instr.ip16(), u16::MIN);
	assert_eq!(instr.ip32(), u16::MIN as u32);
	assert_eq!(instr.ip(), u16::MIN as u64);
	instr.set_ip(0x8A6B_D04A_9B68_3A92);
	instr.set_ip16(u16::MAX);
	assert_eq!(instr.ip16(), u16::MAX);
	assert_eq!(instr.ip32(), u16::MAX as u32);
	assert_eq!(instr.ip(), u16::MAX as u64);

	instr.set_ip(0x8A6B_D04A_9B68_3A92);
	instr.set_ip32(u32::MIN);
	assert_eq!(instr.ip16(), u16::MIN);
	assert_eq!(instr.ip32(), u32::MIN);
	assert_eq!(instr.ip(), u32::MIN as u64);
	instr.set_ip(0x8A6B_D04A_9B68_3A92);
	instr.set_ip32(u32::MAX);
	assert_eq!(instr.ip16(), u16::MAX);
	assert_eq!(instr.ip32(), u32::MAX);
	assert_eq!(instr.ip(), u32::MAX as u64);

	instr.set_ip(u64::MIN);
	assert_eq!(instr.ip16(), u16::MIN);
	assert_eq!(instr.ip32(), u32::MIN);
	assert_eq!(instr.ip(), u64::MIN);
	instr.set_ip(u64::MAX);
	assert_eq!(instr.ip16(), u16::MAX);
	assert_eq!(instr.ip32(), u32::MAX);
	assert_eq!(instr.ip(), u64::MAX);

	instr.set_next_ip(0x8A6B_D04A_9B68_3A92);
	instr.set_next_ip16(u16::MIN);
	assert_eq!(instr.next_ip16(), u16::MIN);
	assert_eq!(instr.next_ip32(), u16::MIN as u32);
	assert_eq!(instr.next_ip(), u16::MIN as u64);
	instr.set_next_ip(0x8A6B_D04A_9B68_3A92);
	instr.set_next_ip16(u16::MAX);
	assert_eq!(instr.next_ip16(), u16::MAX);
	assert_eq!(instr.next_ip32(), u16::MAX as u32);
	assert_eq!(instr.next_ip(), u16::MAX as u64);

	instr.set_next_ip(0x8A6B_D04A_9B68_3A92);
	instr.set_next_ip32(u32::MIN);
	assert_eq!(instr.next_ip16(), u16::MIN);
	assert_eq!(instr.next_ip32(), u32::MIN);
	assert_eq!(instr.next_ip(), u32::MIN as u64);
	instr.set_next_ip(0x8A6B_D04A_9B68_3A92);
	instr.set_next_ip32(u32::MAX);
	assert_eq!(instr.next_ip16(), u16::MAX);
	assert_eq!(instr.next_ip32(), u32::MAX);
	assert_eq!(instr.next_ip(), u32::MAX as u64);

	instr.set_next_ip(u64::MIN);
	assert_eq!(instr.next_ip16(), u16::MIN);
	assert_eq!(instr.next_ip32(), u32::MIN);
	assert_eq!(instr.next_ip(), u64::MIN);
	instr.set_next_ip(u64::MAX);
	assert_eq!(instr.next_ip16(), u16::MAX);
	assert_eq!(instr.next_ip32(), u32::MAX);
	assert_eq!(instr.next_ip(), u64::MAX);

	instr.set_memory_displacement32(u32::MIN);
	assert_eq!(instr.memory_displacement32(), u32::MIN);
	assert_eq!(instr.memory_displacement64(), u32::MIN as u64);
	instr.set_memory_displacement32(u32::MAX);
	assert_eq!(instr.memory_displacement32(), u32::MAX);
	assert_eq!(instr.memory_displacement64(), u32::MAX as u64);

	instr.set_memory_displacement64(u64::MIN);
	assert_eq!(instr.memory_displacement32(), u32::MIN);
	assert_eq!(instr.memory_displacement64(), u64::MIN);
	instr.set_memory_displacement64(u64::MAX);
	assert_eq!(instr.memory_displacement32(), u32::MAX);
	assert_eq!(instr.memory_displacement64(), u64::MAX);

	instr.set_memory_displacement64(0x1234_5678_9ABC_DEF1);
	instr.set_memory_displacement32(0x5AA5_4321);
	assert_eq!(instr.memory_displacement32(), 0x5AA5_4321);
	assert_eq!(instr.memory_displacement64(), 0x5AA5_4321);

	instr.set_immediate8(u8::MIN);
	assert_eq!(instr.immediate8(), u8::MIN);
	instr.set_immediate8(u8::MAX);
	assert_eq!(instr.immediate8(), u8::MAX);

	instr.set_immediate8_2nd(u8::MIN);
	assert_eq!(instr.immediate8_2nd(), u8::MIN);
	instr.set_immediate8_2nd(u8::MAX);
	assert_eq!(instr.immediate8_2nd(), u8::MAX);

	instr.set_immediate16(u16::MIN);
	assert_eq!(instr.immediate16(), u16::MIN);
	instr.set_immediate16(u16::MAX);
	assert_eq!(instr.immediate16(), u16::MAX);

	instr.set_immediate32(u32::MIN);
	assert_eq!(instr.immediate32(), u32::MIN);
	instr.set_immediate32(u32::MAX);
	assert_eq!(instr.immediate32(), u32::MAX);

	instr.set_immediate64(u64::MIN);
	assert_eq!(instr.immediate64(), u64::MIN);
	instr.set_immediate64(u64::MAX);
	assert_eq!(instr.immediate64(), u64::MAX);

	instr.set_immediate8to16(i8::MIN as i16);
	assert_eq!(instr.immediate8to16(), i8::MIN as i16);
	instr.set_immediate8to16(i8::MAX as i16);
	assert_eq!(instr.immediate8to16(), i8::MAX as i16);

	instr.set_immediate8to32(i8::MIN as i32);
	assert_eq!(instr.immediate8to32(), i8::MIN as i32);
	instr.set_immediate8to32(i8::MAX as i32);
	assert_eq!(instr.immediate8to32(), i8::MAX as i32);

	instr.set_immediate8to64(i8::MIN as i64);
	assert_eq!(instr.immediate8to64(), i8::MIN as i64);
	instr.set_immediate8to64(i8::MAX as i64);
	assert_eq!(instr.immediate8to64(), i8::MAX as i64);

	instr.set_immediate32to64(i32::MIN as i64);
	assert_eq!(instr.immediate32to64(), i32::MIN as i64);
	instr.set_immediate32to64(i32::MAX as i64);
	assert_eq!(instr.immediate32to64(), i32::MAX as i64);

	instr.set_op0_kind(OpKind::NearBranch16);
	instr.set_near_branch16(u16::MIN);
	assert_eq!(instr.near_branch16(), u16::MIN);
	assert_eq!(instr.near_branch_target(), u16::MIN as u64);
	instr.set_near_branch16(u16::MAX);
	assert_eq!(instr.near_branch16(), u16::MAX);
	assert_eq!(instr.near_branch_target(), u16::MAX as u64);

	instr.set_op0_kind(OpKind::NearBranch32);
	instr.set_near_branch32(u32::MIN);
	assert_eq!(instr.near_branch32(), u32::MIN);
	assert_eq!(instr.near_branch_target(), u32::MIN as u64);
	instr.set_near_branch32(u32::MAX);
	assert_eq!(instr.near_branch32(), u32::MAX);
	assert_eq!(instr.near_branch_target(), u32::MAX as u64);

	instr.set_op0_kind(OpKind::NearBranch64);
	instr.set_near_branch64(u64::MIN);
	assert_eq!(instr.near_branch64(), u64::MIN);
	assert_eq!(instr.near_branch_target(), u64::MIN);
	instr.set_near_branch64(u64::MAX);
	assert_eq!(instr.near_branch64(), u64::MAX);
	assert_eq!(instr.near_branch_target(), u64::MAX);

	instr.set_far_branch16(u16::MIN);
	assert_eq!(instr.far_branch16(), u16::MIN);
	instr.set_far_branch16(u16::MAX);
	assert_eq!(instr.far_branch16(), u16::MAX);

	instr.set_far_branch32(u32::MIN);
	assert_eq!(instr.far_branch32(), u32::MIN);
	instr.set_far_branch32(u32::MAX);
	assert_eq!(instr.far_branch32(), u32::MAX);

	instr.set_far_branch_selector(u16::MIN);
	assert_eq!(instr.far_branch_selector(), u16::MIN);
	instr.set_far_branch_selector(u16::MAX);
	assert_eq!(instr.far_branch_selector(), u16::MAX);

	{
		let mut instr = instr;
		instr.set_code(Code::Cmpxchg8b_m64);
		instr.set_op0_kind(OpKind::Memory);
		instr.set_has_lock_prefix(true);

		instr.set_has_xacquire_prefix(false);
		assert!(!instr.has_xacquire_prefix());
		instr.set_has_xacquire_prefix(true);
		assert!(instr.has_xacquire_prefix());

		instr.set_has_xrelease_prefix(false);
		assert!(!instr.has_xrelease_prefix());
		instr.set_has_xrelease_prefix(true);
		assert!(instr.has_xrelease_prefix());
	}

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
		assert_eq!(instr.len(), i);
	}

	for code_size in CodeSize::values() {
		instr.set_code_size(code_size);
		assert_eq!(instr.code_size(), code_size);
	}

	for code in Code::values() {
		instr.set_code(code);
		assert_eq!(instr.code(), code);
	}

	const _: () = assert!(IcedConstants::MAX_OP_COUNT == 5);
	for op_kind in OpKind::values() {
		instr.set_op0_kind(op_kind);
		assert_eq!(instr.op0_kind(), op_kind);
	}

	for op_kind in OpKind::values() {
		instr.set_op1_kind(op_kind);
		assert_eq!(instr.op1_kind(), op_kind);
	}

	for op_kind in OpKind::values() {
		instr.set_op2_kind(op_kind);
		assert_eq!(instr.op2_kind(), op_kind);
	}

	for op_kind in OpKind::values() {
		instr.set_op3_kind(op_kind);
		assert_eq!(instr.op3_kind(), op_kind);
	}

	for op_kind in OpKind::values() {
		if op_kind == OpKind::Immediate8 {
			instr.set_op4_kind(op_kind);
			instr.try_set_op4_kind(op_kind).unwrap();
			assert_eq!(instr.op4_kind(), op_kind);
		} else {
			let mut instr = instr;
			if cfg!(debug_assertions) {
				assert!(panic::catch_unwind(move || instr.set_op4_kind(op_kind)).is_err());
			} else {
				instr.set_op4_kind(op_kind)
			}
			assert!(instr.try_set_op4_kind(op_kind).is_err());
		}
	}

	for op_kind in OpKind::values() {
		instr.set_op_kind(0, op_kind);
		instr.try_set_op_kind(0, op_kind).unwrap();
		assert_eq!(instr.op0_kind(), op_kind);
		assert_eq!(instr.op_kind(0), op_kind);
		assert_eq!(instr.try_op_kind(0).unwrap(), op_kind);
	}

	for op_kind in OpKind::values() {
		instr.set_op_kind(1, op_kind);
		instr.try_set_op_kind(1, op_kind).unwrap();
		assert_eq!(instr.op1_kind(), op_kind);
		assert_eq!(instr.op_kind(1), op_kind);
		assert_eq!(instr.try_op_kind(1).unwrap(), op_kind);
	}

	for op_kind in OpKind::values() {
		instr.set_op_kind(2, op_kind);
		instr.try_set_op_kind(2, op_kind).unwrap();
		assert_eq!(instr.op2_kind(), op_kind);
		assert_eq!(instr.op_kind(2), op_kind);
		assert_eq!(instr.try_op_kind(2).unwrap(), op_kind);
	}

	for op_kind in OpKind::values() {
		instr.set_op_kind(3, op_kind);
		instr.try_set_op_kind(3, op_kind).unwrap();
		assert_eq!(instr.op3_kind(), op_kind);
		assert_eq!(instr.op_kind(3), op_kind);
		assert_eq!(instr.try_op_kind(3).unwrap(), op_kind);
	}

	for op_kind in OpKind::values() {
		if op_kind == OpKind::Immediate8 {
			instr.set_op_kind(4, op_kind);
			instr.try_set_op_kind(4, op_kind).unwrap();
			assert_eq!(instr.op4_kind(), op_kind);
			assert_eq!(instr.op_kind(4), op_kind);
			assert_eq!(instr.try_op_kind(4).unwrap(), op_kind);
		} else {
			let mut instr = instr;
			assert!(instr.try_set_op_kind(4, op_kind).is_err());
			if cfg!(debug_assertions) {
				assert!(panic::catch_unwind(move || instr.set_op_kind(4, op_kind)).is_err());
			} else {
				instr.set_op_kind(4, op_kind);
			}
		}
	}

	let seg_values = [Register::ES, Register::CS, Register::SS, Register::DS, Register::FS, Register::GS, Register::None];
	for &seg in &seg_values {
		instr.set_segment_prefix(seg);
		assert_eq!(instr.segment_prefix(), seg);
		if instr.segment_prefix() == Register::None {
			assert!(!instr.has_segment_prefix());
		} else {
			assert!(instr.has_segment_prefix());
		}
	}

	let displ_sizes = [8, 4, 2, 1, 0];
	for &displ_size in &displ_sizes {
		instr.set_memory_displ_size(displ_size);
		assert_eq!(instr.memory_displ_size(), displ_size);
	}

	let scale_values = [8, 4, 2, 1];
	for &scale_value in &scale_values {
		instr.set_memory_index_scale(scale_value);
		assert_eq!(instr.memory_index_scale(), scale_value);
	}

	for reg in Register::values() {
		instr.set_memory_base(reg);
		assert_eq!(instr.memory_base(), reg);
	}

	for reg in Register::values() {
		instr.set_memory_index(reg);
		assert_eq!(instr.memory_index(), reg);
	}

	for reg in Register::values() {
		instr.set_op0_register(reg);
		assert_eq!(instr.op0_register(), reg);
	}

	for reg in Register::values() {
		instr.set_op1_register(reg);
		assert_eq!(instr.op1_register(), reg);
	}

	for reg in Register::values() {
		instr.set_op2_register(reg);
		assert_eq!(instr.op2_register(), reg);
	}

	for reg in Register::values() {
		instr.set_op3_register(reg);
		assert_eq!(instr.op3_register(), reg);
	}

	for reg in Register::values() {
		if reg == Register::None {
			instr.set_op4_register(reg);
			instr.try_set_op4_register(reg).unwrap();
			assert_eq!(instr.op4_register(), reg);
		} else {
			assert!(instr.try_set_op4_register(reg).is_err());
			if cfg!(debug_assertions) {
				let mut instr = instr;
				assert!(panic::catch_unwind(move || instr.set_op4_register(reg)).is_err());
			} else {
				instr.set_op4_register(reg);
			}
		}
	}

	for reg in Register::values() {
		instr.set_op_register(0, reg);
		instr.try_set_op_register(0, reg).unwrap();
		assert_eq!(instr.op0_register(), reg);
		assert_eq!(instr.op_register(0), reg);
		assert_eq!(instr.try_op_register(0).unwrap(), reg);
	}

	for reg in Register::values() {
		instr.set_op_register(1, reg);
		instr.try_set_op_register(1, reg).unwrap();
		assert_eq!(instr.op1_register(), reg);
		assert_eq!(instr.op_register(1), reg);
		assert_eq!(instr.try_op_register(1).unwrap(), reg);
	}

	for reg in Register::values() {
		instr.set_op_register(2, reg);
		instr.try_set_op_register(2, reg).unwrap();
		assert_eq!(instr.op2_register(), reg);
		assert_eq!(instr.op_register(2), reg);
		assert_eq!(instr.try_op_register(2).unwrap(), reg);
	}

	for reg in Register::values() {
		instr.set_op_register(3, reg);
		instr.try_set_op_register(3, reg).unwrap();
		assert_eq!(instr.op3_register(), reg);
		assert_eq!(instr.op_register(3), reg);
		assert_eq!(instr.try_op_register(3).unwrap(), reg);
	}

	for reg in Register::values() {
		if reg == Register::None {
			instr.set_op_register(4, reg);
			instr.try_set_op_register(4, reg).unwrap();
			assert_eq!(instr.op4_register(), reg);
			assert_eq!(instr.op_register(4), reg);
			assert_eq!(instr.try_op_register(4).unwrap(), reg);
		} else {
			assert!(instr.try_set_op_register(4, reg).is_err());
			if cfg!(debug_assertions) {
				let mut instr = instr;
				assert!(panic::catch_unwind(move || instr.set_op_register(4, reg)).is_err());
			} else {
				instr.set_op_register(4, reg);
			}
		}
	}

	let op_masks = [Register::K1, Register::K2, Register::K3, Register::K4, Register::K5, Register::K6, Register::K7, Register::None];
	for &op_mask in &op_masks {
		instr.set_op_mask(op_mask);
		assert_eq!(instr.op_mask(), op_mask);
		assert_eq!(instr.has_op_mask(), op_mask != Register::None);
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

	for rc in RoundingControl::values() {
		instr.set_rounding_control(rc);
		assert_eq!(instr.rounding_control(), rc);
	}

	for reg in Register::values() {
		instr.set_memory_base(reg);
		assert_eq!(instr.is_ip_rel_memory_operand(), reg == Register::RIP || reg == Register::EIP);
	}

	instr.set_memory_base(Register::EIP);
	instr.set_next_ip(0x1234_5670_9EDC_BA98);
	instr.set_memory_displacement64(0x8765_4321_9ABC_DEF5);
	assert!(instr.is_ip_rel_memory_operand());
	assert_eq!(instr.ip_rel_memory_address(), 0x9ABC_DEF5);

	instr.set_memory_base(Register::RIP);
	instr.set_next_ip(0x1234_5670_9EDC_BA98);
	instr.set_memory_displacement64(0x8765_4321_9ABC_DEF5);
	assert!(instr.is_ip_rel_memory_operand());
	assert_eq!(instr.ip_rel_memory_address(), 0x8765_4321_9ABC_DEF5);

	instr.set_declare_data_len(1);
	assert_eq!(instr.declare_data_len(), 1);
	instr.set_declare_data_len(15);
	assert_eq!(instr.declare_data_len(), 15);
	instr.set_declare_data_len(16);
	assert_eq!(instr.declare_data_len(), 16);
}

#[test]
fn verify_get_set_immediate() {
	let mut instr = Instruction::default();

	instr.set_code(Code::Add_AL_imm8);
	instr.set_op1_kind(OpKind::Immediate8);
	instr.set_immediate_i32(1, 0x5A);
	instr.try_set_immediate_i32(1, 0x5A).unwrap();
	assert_eq!(instr.immediate(1), 0x5A);
	assert_eq!(instr.try_immediate(1).unwrap(), 0x5A);
	instr.set_immediate_i32(1, 0xA5);
	instr.try_set_immediate_i32(1, 0xA5).unwrap();
	assert_eq!(instr.immediate(1), 0xA5);
	assert_eq!(instr.try_immediate(1).unwrap(), 0xA5);

	instr.set_code(Code::Add_AX_imm16);
	instr.set_op1_kind(OpKind::Immediate16);
	instr.set_immediate_i32(1, 0x5AA5);
	instr.try_set_immediate_i32(1, 0x5AA5).unwrap();
	assert_eq!(instr.immediate(1), 0x5AA5);
	assert_eq!(instr.try_immediate(1).unwrap(), 0x5AA5);
	instr.set_immediate_i32(1, 0xA55A);
	instr.try_set_immediate_i32(1, 0xA55A).unwrap();
	assert_eq!(instr.immediate(1), 0xA55A);
	assert_eq!(instr.try_immediate(1).unwrap(), 0xA55A);

	instr.set_code(Code::Add_EAX_imm32);
	instr.set_op1_kind(OpKind::Immediate32);
	instr.set_immediate_i32(1, 0x5AA5_1234);
	instr.try_set_immediate_i32(1, 0x5AA5_1234).unwrap();
	assert_eq!(instr.immediate(1), 0x5AA5_1234);
	assert_eq!(instr.try_immediate(1).unwrap(), 0x5AA5_1234);
	instr.set_immediate_u32(1, 0xA54A_1234);
	instr.try_set_immediate_u32(1, 0xA54A_1234).unwrap();
	assert_eq!(instr.immediate(1), 0xA54A_1234);
	assert_eq!(instr.try_immediate(1).unwrap(), 0xA54A_1234);

	instr.set_code(Code::Add_RAX_imm32);
	instr.set_op1_kind(OpKind::Immediate32to64);
	instr.set_immediate_i32(1, 0x5AA5_1234);
	instr.try_set_immediate_i32(1, 0x5AA5_1234).unwrap();
	assert_eq!(instr.immediate(1), 0x5AA5_1234);
	assert_eq!(instr.try_immediate(1).unwrap(), 0x5AA5_1234);
	instr.set_immediate_u32(1, 0xA54A_1234);
	instr.try_set_immediate_u32(1, 0xA54A_1234).unwrap();
	assert_eq!(instr.immediate(1), 0xFFFF_FFFF_A54A_1234);
	assert_eq!(instr.try_immediate(1).unwrap(), 0xFFFF_FFFF_A54A_1234);

	instr.set_code(Code::Enterq_imm16_imm8);
	instr.set_op1_kind(OpKind::Immediate8_2nd);
	instr.set_immediate_i32(1, 0x5A);
	instr.try_set_immediate_i32(1, 0x5A).unwrap();
	assert_eq!(instr.immediate(1), 0x5A);
	assert_eq!(instr.try_immediate(1).unwrap(), 0x5A);
	instr.set_immediate_i32(1, 0xA5);
	instr.try_set_immediate_i32(1, 0xA5).unwrap();
	assert_eq!(instr.immediate(1), 0xA5);
	assert_eq!(instr.try_immediate(1).unwrap(), 0xA5);

	instr.set_code(Code::Adc_rm16_imm8);
	instr.set_op1_kind(OpKind::Immediate8to16);
	instr.set_immediate_i32(1, 0x5A);
	instr.try_set_immediate_i32(1, 0x5A).unwrap();
	assert_eq!(instr.immediate(1), 0x5A);
	assert_eq!(instr.try_immediate(1).unwrap(), 0x5A);
	instr.set_immediate_i32(1, 0xA5);
	instr.try_set_immediate_i32(1, 0xA5).unwrap();
	assert_eq!(instr.immediate(1), 0xFFFF_FFFF_FFFF_FFA5);
	assert_eq!(instr.try_immediate(1).unwrap(), 0xFFFF_FFFF_FFFF_FFA5);

	instr.set_code(Code::Adc_rm32_imm8);
	instr.set_op1_kind(OpKind::Immediate8to32);
	instr.set_immediate_i32(1, 0x5A);
	instr.try_set_immediate_i32(1, 0x5A).unwrap();
	assert_eq!(instr.immediate(1), 0x5A);
	assert_eq!(instr.try_immediate(1).unwrap(), 0x5A);
	instr.set_immediate_i32(1, 0xA5);
	instr.try_set_immediate_i32(1, 0xA5).unwrap();
	assert_eq!(instr.immediate(1), 0xFFFF_FFFF_FFFF_FFA5);
	assert_eq!(instr.try_immediate(1).unwrap(), 0xFFFF_FFFF_FFFF_FFA5);

	instr.set_code(Code::Adc_rm64_imm8);
	instr.set_op1_kind(OpKind::Immediate8to64);
	instr.set_immediate_i32(1, 0x5A);
	instr.try_set_immediate_i32(1, 0x5A).unwrap();
	assert_eq!(instr.immediate(1), 0x5A);
	assert_eq!(instr.try_immediate(1).unwrap(), 0x5A);
	instr.set_immediate_i32(1, 0xA5);
	instr.try_set_immediate_i32(1, 0xA5).unwrap();
	assert_eq!(instr.immediate(1), 0xFFFF_FFFF_FFFF_FFA5);
	assert_eq!(instr.try_immediate(1).unwrap(), 0xFFFF_FFFF_FFFF_FFA5);

	instr.set_code(Code::Mov_r64_imm64);
	instr.set_op1_kind(OpKind::Immediate64);
	instr.set_immediate_i64(1, 0x5AA5_1234_5678_9ABC);
	instr.try_set_immediate_i64(1, 0x5AA5_1234_5678_9ABC).unwrap();
	assert_eq!(instr.immediate(1), 0x5AA5_1234_5678_9ABC);
	assert_eq!(instr.try_immediate(1).unwrap(), 0x5AA5_1234_5678_9ABC);
	instr.set_immediate_u64(1, 0xA54A_1234_5678_9ABC);
	instr.try_set_immediate_u64(1, 0xA54A_1234_5678_9ABC).unwrap();
	assert_eq!(instr.immediate(1), 0xA54A_1234_5678_9ABC);
	assert_eq!(instr.try_immediate(1).unwrap(), 0xA54A_1234_5678_9ABC);
	instr.set_immediate_i64(1, -0x5AB5_EDCB_A987_6544);
	instr.try_set_immediate_i64(1, -0x5AB5_EDCB_A987_6544).unwrap();
	assert_eq!(instr.immediate(1), 0xA54A_1234_5678_9ABC);
	assert_eq!(instr.try_immediate(1).unwrap(), 0xA54A_1234_5678_9ABC);

	{
		let instr = instr;
		assert!(instr.try_immediate(0).is_err());
		if cfg!(debug_assertions) {
			assert!(panic::catch_unwind(move || { instr.immediate(0) }).is_err());
		} else {
			let _ = instr.immediate(0);
		}
	}
	{
		assert!(instr.try_set_immediate_i32(0, 0).is_err());
		if cfg!(debug_assertions) {
			let mut instr = instr;
			assert!(panic::catch_unwind(move || instr.set_immediate_i32(0, 0)).is_err());
		} else {
			instr.set_immediate_i32(0, 0);
		}
	}
	{
		assert!(instr.try_set_immediate_u32(0, 0).is_err());
		if cfg!(debug_assertions) {
			let mut instr = instr;
			assert!(panic::catch_unwind(move || instr.set_immediate_u32(0, 0)).is_err());
		} else {
			instr.set_immediate_u32(0, 0);
		}
	}
	{
		assert!(instr.try_set_immediate_i64(0, 0).is_err());
		if cfg!(debug_assertions) {
			let mut instr = instr;
			assert!(panic::catch_unwind(move || instr.set_immediate_i64(0, 0)).is_err());
		} else {
			instr.set_immediate_i64(0, 0);
		}
	}
	{
		assert!(instr.try_set_immediate_u64(0, 0).is_err());
		if cfg!(debug_assertions) {
			let mut instr = instr;
			assert!(panic::catch_unwind(move || instr.set_immediate_u64(0, 0)).is_err());
		} else {
			instr.set_immediate_u64(0, 0);
		}
	}
}

#[test]
fn verify_instruction_size() {
	const _: () = assert!(mem::size_of::<Instruction>() == INSTRUCTION_TOTAL_SIZE);
}
