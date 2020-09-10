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

// These funcs should be in Instruction but since racer (used by RLS) shows
// all functions even if they're private, they've been moved here.
//	https://github.com/racer-rust/racer/issues/165
// If this 5 year old issue is ever fixed, move these funcs back and remove
// pub(crate) from Instruction's fields.

use super::*;
#[cfg(feature = "encoder")]
use core::{i16, i32, i8, u8};
use core::{u16, u32};

#[cfg(feature = "decoder")]
#[inline]
pub(crate) fn internal_set_code_size(this: &mut Instruction, new_value: CodeSize) {
	this.op_kind_flags |= (new_value as u32) << OpKindFlags::CODE_SIZE_SHIFT;
}

#[cfg(any(feature = "decoder", feature = "encoder"))]
#[inline]
pub(crate) fn internal_set_code(this: &mut Instruction, new_value: Code) {
	this.code_flags |= new_value as u32;
}

#[cfg(feature = "decoder")]
#[inline]
pub(crate) fn internal_set_code_u32(this: &mut Instruction, new_value: u32) {
	this.code_flags |= new_value;
}

#[cfg(feature = "decoder")]
#[inline]
pub(crate) fn internal_set_len(this: &mut Instruction, new_value: u32) {
	this.code_flags |= new_value << CodeFlags::INSTR_LENGTH_SHIFT;
}

#[cfg(feature = "encoder")]
#[cfg_attr(has_must_use, must_use)]
#[inline]
pub(crate) fn internal_has_repe_prefix_has_xrelease_prefix(this: &Instruction) -> bool {
	(this.code_flags & (CodeFlags::REPE_PREFIX | CodeFlags::XRELEASE_PREFIX)) != 0
}

#[cfg(feature = "encoder")]
#[cfg_attr(has_must_use, must_use)]
#[inline]
pub(crate) fn internal_has_repne_prefix_has_xacquire_prefix(this: &Instruction) -> bool {
	(this.code_flags & (CodeFlags::REPNE_PREFIX | CodeFlags::XACQUIRE_PREFIX)) != 0
}

#[cfg(feature = "instr_info")]
#[cfg_attr(has_must_use, must_use)]
#[inline]
pub(crate) fn internal_has_repe_or_repne_prefix(this: &Instruction) -> bool {
	(this.code_flags & (CodeFlags::REPE_PREFIX | CodeFlags::REPNE_PREFIX)) != 0
}

#[cfg(feature = "decoder")]
#[inline]
pub(crate) fn internal_set_has_xacquire_prefix(this: &mut Instruction) {
	this.code_flags |= CodeFlags::XACQUIRE_PREFIX
}

#[cfg(feature = "decoder")]
#[inline]
pub(crate) fn internal_set_has_xrelease_prefix(this: &mut Instruction) {
	this.code_flags |= CodeFlags::XRELEASE_PREFIX
}

#[cfg(any(feature = "decoder", feature = "encoder"))]
#[inline]
pub(crate) fn internal_set_has_repe_prefix(this: &mut Instruction) {
	this.code_flags = (this.code_flags & !CodeFlags::REPNE_PREFIX) | CodeFlags::REPE_PREFIX
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm", feature = "fast_fmt"))]
#[inline]
pub(crate) fn internal_has_any_of_xacquire_xrelease_lock_rep_repne_prefix(this: &Instruction) -> u32 {
	this.code_flags
		& (CodeFlags::XACQUIRE_PREFIX | CodeFlags::XRELEASE_PREFIX | CodeFlags::LOCK_PREFIX | CodeFlags::REPE_PREFIX | CodeFlags::REPNE_PREFIX)
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm", feature = "fast_fmt"))]
#[inline]
pub(crate) fn internal_has_op_mask_or_zeroing_masking(this: &Instruction) -> bool {
	(this.code_flags & ((CodeFlags::OP_MASK_MASK << CodeFlags::OP_MASK_SHIFT) | CodeFlags::ZEROING_MASKING)) != 0
}

#[cfg(feature = "decoder")]
#[inline]
pub(crate) fn internal_clear_has_repe_prefix(this: &mut Instruction) {
	this.code_flags &= !CodeFlags::REPE_PREFIX
}

#[cfg(any(feature = "decoder", feature = "encoder"))]
#[inline]
pub(crate) fn internal_set_has_repne_prefix(this: &mut Instruction) {
	this.code_flags = (this.code_flags & !CodeFlags::REPE_PREFIX) | CodeFlags::REPNE_PREFIX
}

#[cfg(feature = "decoder")]
#[inline]
pub(crate) fn internal_clear_has_repne_prefix(this: &mut Instruction) {
	this.code_flags &= !CodeFlags::REPNE_PREFIX
}

#[cfg(feature = "decoder")]
#[inline]
pub(crate) fn internal_set_has_lock_prefix(this: &mut Instruction) {
	this.code_flags |= CodeFlags::LOCK_PREFIX
}

#[cfg(feature = "decoder")]
#[inline]
pub(crate) fn internal_clear_has_lock_prefix(this: &mut Instruction) {
	this.code_flags &= !CodeFlags::LOCK_PREFIX
}

#[cfg(any(feature = "decoder", feature = "encoder"))]
#[inline]
pub(crate) fn internal_set_op0_kind(this: &mut Instruction, new_value: OpKind) {
	this.op_kind_flags |= new_value as u32;
}

#[cfg(feature = "instr_info")]
#[cfg_attr(has_must_use, must_use)]
#[inline]
pub(crate) fn internal_op0_is_not_reg_or_op1_is_not_reg(this: &Instruction) -> bool {
	(this.op_kind_flags & (OpKindFlags::OP_KIND_MASK | (OpKindFlags::OP_KIND_MASK << OpKindFlags::OP1_KIND_SHIFT))) != 0
}

#[cfg(any(feature = "decoder", feature = "encoder"))]
#[inline]
pub(crate) fn internal_set_op1_kind(this: &mut Instruction, new_value: OpKind) {
	this.op_kind_flags |= (new_value as u32) << OpKindFlags::OP1_KIND_SHIFT;
}

#[cfg(any(feature = "decoder", feature = "encoder"))]
#[inline]
pub(crate) fn internal_set_op2_kind(this: &mut Instruction, new_value: OpKind) {
	this.op_kind_flags |= (new_value as u32) << OpKindFlags::OP2_KIND_SHIFT;
}

#[cfg(any(feature = "decoder", feature = "encoder"))]
#[inline]
pub(crate) fn internal_set_op3_kind(this: &mut Instruction, new_value: OpKind) {
	this.op_kind_flags |= (new_value as u32) << OpKindFlags::OP3_KIND_SHIFT;
}

#[cfg(any(feature = "decoder", feature = "encoder"))]
#[inline]
pub(crate) fn internal_set_memory_displ_size(this: &mut Instruction, new_value: u32) {
	debug_assert!(new_value <= 4);
	this.memory_flags |= (new_value << MemoryFlags::DISPL_SIZE_SHIFT) as u16;
}

#[cfg(feature = "decoder")]
#[cfg(not(feature = "no_evex"))]
#[inline]
pub(crate) fn internal_set_is_broadcast(this: &mut Instruction) {
	this.memory_flags |= MemoryFlags::BROADCAST as u16;
}

#[cfg_attr(has_must_use, must_use)]
#[inline]
pub(crate) fn internal_get_memory_index_scale(this: &Instruction) -> u32 {
	(this.memory_flags & (MemoryFlags::SCALE_MASK as u16)) as u32
}

#[cfg(feature = "decoder")]
#[inline]
pub(crate) fn internal_set_memory_index_scale(this: &mut Instruction, new_value: u32) {
	this.memory_flags |= new_value as u16;
}

#[cfg(any(feature = "decoder", feature = "encoder"))]
#[inline]
pub(crate) fn internal_set_immediate8(this: &mut Instruction, new_value: u32) {
	this.immediate = new_value;
}

#[cfg(any(feature = "decoder", feature = "encoder"))]
#[inline]
pub(crate) fn internal_set_immediate8_2nd(this: &mut Instruction, new_value: u32) {
	this.mem_displ = new_value;
}

#[cfg(any(feature = "decoder", feature = "encoder"))]
#[inline]
pub(crate) fn internal_set_immediate16(this: &mut Instruction, new_value: u32) {
	this.immediate = new_value;
}

#[cfg(feature = "decoder")]
#[inline]
pub(crate) fn internal_set_immediate64_lo(this: &mut Instruction, new_value: u32) {
	this.immediate = new_value;
}

#[cfg(feature = "decoder")]
#[inline]
pub(crate) fn internal_set_immediate64_hi(this: &mut Instruction, new_value: u32) {
	this.mem_displ = new_value;
}

#[cfg(feature = "decoder")]
#[inline]
pub(crate) fn internal_set_memory_address64_lo(this: &mut Instruction, new_value: u32) {
	this.immediate = new_value;
}

#[cfg(feature = "decoder")]
#[inline]
pub(crate) fn internal_set_memory_address64_hi(this: &mut Instruction, new_value: u32) {
	this.mem_displ = new_value;
}

#[cfg(feature = "decoder")]
#[inline]
pub(crate) fn internal_set_near_branch16(this: &mut Instruction, new_value: u32) {
	this.immediate = new_value;
}

#[cfg(feature = "decoder")]
#[inline]
pub(crate) fn internal_set_far_branch16(this: &mut Instruction, new_value: u32) {
	this.immediate = new_value;
}

#[cfg(feature = "decoder")]
#[inline]
pub(crate) fn internal_set_far_branch_selector(this: &mut Instruction, new_value: u32) {
	this.mem_displ = new_value;
}

#[cfg(any(feature = "decoder", feature = "encoder"))]
#[inline]
pub(crate) fn internal_set_memory_base(this: &mut Instruction, new_value: Register) {
	this.mem_base_reg = new_value as u8;
}

#[cfg(feature = "decoder")]
#[inline]
pub(crate) fn internal_set_memory_base_u32(this: &mut Instruction, new_value: u32) {
	this.mem_base_reg = new_value as u8;
}

#[cfg(any(feature = "decoder", feature = "encoder"))]
#[inline]
pub(crate) fn internal_set_memory_index(this: &mut Instruction, new_value: Register) {
	this.mem_index_reg = new_value as u8;
}

#[cfg(feature = "decoder")]
#[inline]
pub(crate) fn internal_set_memory_index_u32(this: &mut Instruction, new_value: u32) {
	this.mem_index_reg = new_value as u8;
}

#[cfg(any(feature = "decoder", feature = "encoder"))]
#[inline]
pub(crate) fn internal_set_op0_register(this: &mut Instruction, new_value: Register) {
	this.reg0 = new_value as u8;
}

#[cfg(feature = "decoder")]
#[inline]
pub(crate) fn internal_set_op0_register_u32(this: &mut Instruction, new_value: u32) {
	this.reg0 = new_value as u8;
}

#[cfg(any(feature = "decoder", feature = "encoder"))]
#[inline]
pub(crate) fn internal_set_op1_register(this: &mut Instruction, new_value: Register) {
	this.reg1 = new_value as u8;
}

#[cfg(feature = "decoder")]
#[inline]
pub(crate) fn internal_set_op1_register_u32(this: &mut Instruction, new_value: u32) {
	this.reg1 = new_value as u8;
}

#[cfg(any(feature = "decoder", feature = "encoder"))]
#[inline]
pub(crate) fn internal_set_op2_register(this: &mut Instruction, new_value: Register) {
	this.reg2 = new_value as u8;
}

#[cfg(feature = "decoder")]
#[inline]
pub(crate) fn internal_set_op2_register_u32(this: &mut Instruction, new_value: u32) {
	this.reg2 = new_value as u8;
}

#[allow(dead_code)]
#[cfg(feature = "encoder")]
#[inline]
pub(crate) fn internal_set_op3_register(this: &mut Instruction, new_value: Register) {
	this.reg3 = new_value as u8;
}

#[cfg(feature = "decoder")]
#[cfg(any(not(feature = "no_vex"), not(feature = "no_xop")))]
#[inline]
pub(crate) fn internal_set_op3_register_u32(this: &mut Instruction, new_value: u32) {
	this.reg3 = new_value as u8;
}

#[cfg(feature = "encoder")]
#[cfg(not(feature = "no_evex"))]
#[cfg_attr(has_must_use, must_use)]
#[inline]
pub(crate) fn internal_op_mask(this: &Instruction) -> u32 {
	(this.code_flags >> CodeFlags::OP_MASK_SHIFT) & CodeFlags::OP_MASK_MASK
}

#[cfg(feature = "decoder")]
#[cfg(not(feature = "no_evex"))]
#[inline]
pub(crate) fn internal_set_op_mask(this: &mut Instruction, new_value: u32) {
	this.code_flags |= new_value << CodeFlags::OP_MASK_SHIFT
}

#[cfg(feature = "decoder")]
#[cfg(not(feature = "no_evex"))]
#[inline]
pub(crate) fn internal_set_zeroing_masking(this: &mut Instruction) {
	this.code_flags |= CodeFlags::ZEROING_MASKING;
}

#[cfg(feature = "decoder")]
#[cfg(not(feature = "no_evex"))]
#[inline]
pub(crate) fn internal_set_rounding_control(this: &mut Instruction, new_value: u32) {
	this.code_flags |= new_value << CodeFlags::ROUNDING_CONTROL_SHIFT;
}

#[cfg(any(feature = "intel", feature = "masm", feature = "fast_fmt"))]
#[inline]
pub(crate) fn internal_has_rounding_control_or_sae(this: &Instruction) -> bool {
	(this.code_flags & ((CodeFlags::ROUNDING_CONTROL_MASK << CodeFlags::ROUNDING_CONTROL_SHIFT) | CodeFlags::SUPPRESS_ALL_EXCEPTIONS)) != 0
}

#[cfg(feature = "encoder")]
#[inline]
pub(crate) fn internal_set_declare_data_len(this: &mut Instruction, new_value: u32) {
	this.op_kind_flags |= (new_value - 1) << OpKindFlags::DATA_LENGTH_SHIFT;
}

#[cfg(feature = "decoder")]
#[cfg(not(feature = "no_evex"))]
#[inline]
pub(crate) fn internal_set_suppress_all_exceptions(this: &mut Instruction) {
	this.code_flags |= CodeFlags::SUPPRESS_ALL_EXCEPTIONS;
}

#[cfg_attr(has_must_use, must_use)]
pub(crate) fn get_address_size_in_bytes(base_reg: Register, index_reg: Register, displ_size: u32, code_size: CodeSize) -> u32 {
	if (Register::RAX <= base_reg && base_reg <= Register::R15)
		|| (Register::RAX <= index_reg && index_reg <= Register::R15)
		|| base_reg == Register::RIP
	{
		return 8;
	}
	if (Register::EAX <= base_reg && base_reg <= Register::R15D)
		|| (Register::EAX <= index_reg && index_reg <= Register::R15D)
		|| base_reg == Register::EIP
	{
		return 4;
	}
	if (Register::AX <= base_reg && base_reg <= Register::DI) || (Register::AX <= index_reg && index_reg <= Register::DI) {
		return 2;
	}
	if displ_size == 2 || displ_size == 4 || displ_size == 8 {
		return displ_size;
	}

	match code_size {
		CodeSize::Code64 => 8,
		CodeSize::Code32 => 4,
		CodeSize::Code16 => 2,
		_ => 8,
	}
}

#[cfg(feature = "encoder")]
pub(crate) fn initialize_signed_immediate(instruction: &mut Instruction, operand: usize, immediate: i64) {
	let op_kind = get_immediate_op_kind(instruction.code(), operand);
	instruction.set_op_kind(operand as u32, op_kind);

	match op_kind {
		OpKind::Immediate8 => {
			// All i8 and all u8 values can be used
			if !(i8::MIN as i64 <= immediate && immediate <= u8::MAX as i64) {
				panic!();
			}
			internal_set_immediate8(instruction, immediate as u8 as u32);
		}

		OpKind::Immediate8_2nd => {
			// All i8 and all u8 values can be used
			if !(i8::MIN as i64 <= immediate && immediate <= u8::MAX as i64) {
				panic!();
			}
			internal_set_immediate8_2nd(instruction, immediate as u8 as u32);
		}

		OpKind::Immediate8to16 => {
			if !(i8::MIN as i64 <= immediate && immediate <= i8::MAX as i64) {
				panic!();
			}
			internal_set_immediate8(instruction, immediate as u8 as u32);
		}

		OpKind::Immediate8to32 => {
			if !(i8::MIN as i64 <= immediate && immediate <= i8::MAX as i64) {
				panic!();
			}
			internal_set_immediate8(instruction, immediate as u8 as u32);
		}

		OpKind::Immediate8to64 => {
			if !(i8::MIN as i64 <= immediate && immediate <= i8::MAX as i64) {
				panic!();
			}
			internal_set_immediate8(instruction, immediate as u8 as u32);
		}

		OpKind::Immediate16 => {
			// All short and all ushort values can be used
			if !(i16::MIN as i64 <= immediate && immediate <= u16::MAX as i64) {
				panic!();
			}
			internal_set_immediate16(instruction, immediate as u16 as u32);
		}

		OpKind::Immediate32 => {
			// All int and all uint values can be used
			if !(i32::MIN as i64 <= immediate && immediate <= u32::MAX as i64) {
				panic!();
			}
			instruction.set_immediate32(immediate as u32);
		}

		OpKind::Immediate32to64 => {
			if !(i32::MIN as i64 <= immediate && immediate <= i32::MAX as i64) {
				panic!();
			}
			instruction.set_immediate32(immediate as u32);
		}

		OpKind::Immediate64 => instruction.set_immediate64(immediate as u64),

		_ => panic!(),
	}
}

#[cfg(feature = "encoder")]
pub(crate) fn initialize_unsigned_immediate(instruction: &mut Instruction, operand: usize, immediate: u64) {
	let op_kind = get_immediate_op_kind(instruction.code(), operand);
	instruction.set_op_kind(operand as u32, op_kind);

	match op_kind {
		OpKind::Immediate8 => {
			if immediate > u8::MAX as u64 {
				panic!();
			}
			internal_set_immediate8(instruction, immediate as u8 as u32);
		}

		OpKind::Immediate8_2nd => {
			if immediate > u8::MAX as u64 {
				panic!();
			}
			internal_set_immediate8_2nd(instruction, immediate as u8 as u32);
		}

		OpKind::Immediate8to16 => {
			if !(immediate <= i8::MAX as u64 || (0xFF80 <= immediate && immediate <= 0xFFFF)) {
				panic!();
			}
			internal_set_immediate8(instruction, immediate as u8 as u32);
		}

		OpKind::Immediate8to32 => {
			if !(immediate <= i8::MAX as u64 || (0xFFFF_FF80 <= immediate && immediate <= 0xFFFF_FFFF)) {
				panic!();
			}
			internal_set_immediate8(instruction, immediate as u8 as u32);
		}

		OpKind::Immediate8to64 => {
			// Allow 00..7F and FFFF_FFFF_FFFF_FF80..FFFF_FFFF_FFFF_FFFF
			if immediate.wrapping_add(0x80) > u8::MAX as u64 {
				panic!();
			}
			internal_set_immediate8(instruction, immediate as u8 as u32);
		}

		OpKind::Immediate16 => {
			if immediate > u16::MAX as u64 {
				panic!();
			}
			internal_set_immediate16(instruction, immediate as u16 as u32);
		}

		OpKind::Immediate32 => {
			if immediate > u32::MAX as u64 {
				panic!();
			}
			instruction.set_immediate32(immediate as u32);
		}

		OpKind::Immediate32to64 => {
			// Allow 0..7FFF_FFFF and FFFF_FFFF_8000_0000..FFFF_FFFF_FFFF_FFFF
			if immediate.wrapping_add(0x8000_0000) > u32::MAX as u64 {
				panic!();
			}
			instruction.set_immediate32(immediate as u32);
		}

		OpKind::Immediate64 => instruction.set_immediate64(immediate),

		_ => panic!(),
	}
}

#[cfg(feature = "encoder")]
pub(crate) fn get_immediate_op_kind(code: Code, operand: usize) -> OpKind {
	let operands = &super::encoder::handlers_table::HANDLERS_TABLE[code as usize].operands;
	if operand >= operands.len() {
		if cfg!(debug_assertions) {
			panic!("{:?} doesn't have at least {} operands", code, operand + 1);
		} else {
			panic!("Code value {} doesn't have at least {} operands", code as u32, operand + 1);
		}
	}
	match operands[operand].immediate_op_kind() {
		Some(op_kind) => op_kind,
		None => {
			if cfg!(debug_assertions) {
				panic!("{:?}'s op{} isn't an immediate operand", code, operand);
			} else {
				panic!("Code value {}'s op{} isn't an immediate operand", code as u32, operand);
			}
		}
	}
}

#[cfg(feature = "encoder")]
pub(crate) fn get_near_branch_op_kind(code: Code, operand: usize) -> OpKind {
	let operands = &super::encoder::handlers_table::HANDLERS_TABLE[code as usize].operands;
	if operand >= operands.len() {
		if cfg!(debug_assertions) {
			panic!("{:?} doesn't have at least {} operands", code, operand + 1);
		} else {
			panic!("Code value {} doesn't have at least {} operands", code as u32, operand + 1);
		}
	}
	match operands[operand].near_branch_op_kind() {
		Some(op_kind) => op_kind,
		None => {
			if cfg!(debug_assertions) {
				panic!("{:?}'s op{} isn't a near branch operand", code, operand);
			} else {
				panic!("Code value {}'s op{} isn't a near branch operand", code as u32, operand);
			}
		}
	}
}

#[cfg(feature = "encoder")]
pub(crate) fn get_far_branch_op_kind(code: Code, operand: usize) -> OpKind {
	let operands = &super::encoder::handlers_table::HANDLERS_TABLE[code as usize].operands;
	if operand >= operands.len() {
		if cfg!(debug_assertions) {
			panic!("{:?} doesn't have at least {} operands", code, operand + 1);
		} else {
			panic!("Code value {} doesn't have at least {} operands", code as u32, operand + 1);
		}
	}
	match operands[operand].far_branch_op_kind() {
		Some(op_kind) => op_kind,
		None => {
			if cfg!(debug_assertions) {
				panic!("{:?}'s op{} isn't a far branch operand", code, operand);
			} else {
				panic!("Code value {}'s op{} isn't a far branch operand", code as u32, operand);
			}
		}
	}
}

#[cfg(feature = "encoder")]
pub(crate) fn with_string_reg_segrsi(
	code: Code, address_size: u32, register: Register, segment_prefix: Register, rep_prefix: RepPrefixKind,
) -> Instruction {
	let mut instruction = Instruction::default();
	internal_set_code(&mut instruction, code);

	match rep_prefix {
		RepPrefixKind::None => {}
		RepPrefixKind::Repe => internal_set_has_repe_prefix(&mut instruction),
		RepPrefixKind::Repne => internal_set_has_repne_prefix(&mut instruction),
	}

	const_assert_eq!(0, OpKind::Register as u32);
	//internal_set_op0_kind(&mut instruction, OpKind::Register);
	internal_set_op0_register(&mut instruction, register);

	match address_size {
		64 => internal_set_op1_kind(&mut instruction, OpKind::MemorySegRSI),
		32 => internal_set_op1_kind(&mut instruction, OpKind::MemorySegESI),
		16 => internal_set_op1_kind(&mut instruction, OpKind::MemorySegSI),
		_ => panic!(),
	}

	instruction.set_segment_prefix(segment_prefix);

	debug_assert_eq!(2, instruction.op_count());
	instruction
}

#[cfg(feature = "encoder")]
pub(crate) fn with_string_reg_esrdi(code: Code, address_size: u32, register: Register, rep_prefix: RepPrefixKind) -> Instruction {
	let mut instruction = Instruction::default();
	internal_set_code(&mut instruction, code);

	match rep_prefix {
		RepPrefixKind::None => {}
		RepPrefixKind::Repe => internal_set_has_repe_prefix(&mut instruction),
		RepPrefixKind::Repne => internal_set_has_repne_prefix(&mut instruction),
	}

	const_assert_eq!(0, OpKind::Register as u32);
	//internal_set_op0_kind(&mut instruction, OpKind::Register);
	internal_set_op0_register(&mut instruction, register);

	match address_size {
		64 => internal_set_op1_kind(&mut instruction, OpKind::MemoryESRDI),
		32 => internal_set_op1_kind(&mut instruction, OpKind::MemoryESEDI),
		16 => internal_set_op1_kind(&mut instruction, OpKind::MemoryESDI),
		_ => panic!(),
	}

	debug_assert_eq!(2, instruction.op_count());
	instruction
}

#[cfg(feature = "encoder")]
pub(crate) fn with_string_esrdi_reg(code: Code, address_size: u32, register: Register, rep_prefix: RepPrefixKind) -> Instruction {
	let mut instruction = Instruction::default();
	internal_set_code(&mut instruction, code);

	match rep_prefix {
		RepPrefixKind::None => {}
		RepPrefixKind::Repe => internal_set_has_repe_prefix(&mut instruction),
		RepPrefixKind::Repne => internal_set_has_repne_prefix(&mut instruction),
	}

	match address_size {
		64 => internal_set_op0_kind(&mut instruction, OpKind::MemoryESRDI),
		32 => internal_set_op0_kind(&mut instruction, OpKind::MemoryESEDI),
		16 => internal_set_op0_kind(&mut instruction, OpKind::MemoryESDI),
		_ => panic!(),
	}

	const_assert_eq!(0, OpKind::Register as u32);
	//internal_set_op1_kind(&mut instruction, OpKind::Register);
	internal_set_op1_register(&mut instruction, register);

	debug_assert_eq!(2, instruction.op_count());
	instruction
}

#[cfg(feature = "encoder")]
pub(crate) fn with_string_segrsi_esrdi(code: Code, address_size: u32, segment_prefix: Register, rep_prefix: RepPrefixKind) -> Instruction {
	let mut instruction = Instruction::default();
	internal_set_code(&mut instruction, code);

	match rep_prefix {
		RepPrefixKind::None => {}
		RepPrefixKind::Repe => internal_set_has_repe_prefix(&mut instruction),
		RepPrefixKind::Repne => internal_set_has_repne_prefix(&mut instruction),
	}

	match address_size {
		64 => {
			internal_set_op0_kind(&mut instruction, OpKind::MemorySegRSI);
			internal_set_op1_kind(&mut instruction, OpKind::MemoryESRDI);
		}
		32 => {
			internal_set_op0_kind(&mut instruction, OpKind::MemorySegESI);
			internal_set_op1_kind(&mut instruction, OpKind::MemoryESEDI);
		}
		16 => {
			internal_set_op0_kind(&mut instruction, OpKind::MemorySegSI);
			internal_set_op1_kind(&mut instruction, OpKind::MemoryESDI);
		}
		_ => panic!(),
	}

	instruction.set_segment_prefix(segment_prefix);

	debug_assert_eq!(2, instruction.op_count());
	instruction
}

#[cfg(feature = "encoder")]
pub(crate) fn with_string_esrdi_segrsi(code: Code, address_size: u32, segment_prefix: Register, rep_prefix: RepPrefixKind) -> Instruction {
	let mut instruction = Instruction::default();
	internal_set_code(&mut instruction, code);

	match rep_prefix {
		RepPrefixKind::None => {}
		RepPrefixKind::Repe => internal_set_has_repe_prefix(&mut instruction),
		RepPrefixKind::Repne => internal_set_has_repne_prefix(&mut instruction),
	}

	match address_size {
		64 => {
			internal_set_op0_kind(&mut instruction, OpKind::MemoryESRDI);
			internal_set_op1_kind(&mut instruction, OpKind::MemorySegRSI);
		}
		32 => {
			internal_set_op0_kind(&mut instruction, OpKind::MemoryESEDI);
			internal_set_op1_kind(&mut instruction, OpKind::MemorySegESI);
		}
		16 => {
			internal_set_op0_kind(&mut instruction, OpKind::MemoryESDI);
			internal_set_op1_kind(&mut instruction, OpKind::MemorySegSI);
		}
		_ => panic!(),
	}

	instruction.set_segment_prefix(segment_prefix);

	debug_assert_eq!(2, instruction.op_count());
	instruction
}

#[cfg(feature = "encoder")]
pub(crate) fn with_maskmov(code: Code, address_size: u32, register1: Register, register2: Register, segment_prefix: Register) -> Instruction {
	let mut instruction = Instruction::default();
	internal_set_code(&mut instruction, code);

	match address_size {
		64 => internal_set_op0_kind(&mut instruction, OpKind::MemorySegRDI),
		32 => internal_set_op0_kind(&mut instruction, OpKind::MemorySegEDI),
		16 => internal_set_op0_kind(&mut instruction, OpKind::MemorySegDI),
		_ => panic!(),
	}

	const_assert_eq!(0, OpKind::Register as u32);
	//internal_set_op1_kind(&mut instruction, OpKind::Register);
	internal_set_op1_register(&mut instruction, register1);

	const_assert_eq!(0, OpKind::Register as u32);
	//internal_set_op2_kind(&mut instruction, OpKind::Register);
	internal_set_op2_register(&mut instruction, register2);

	instruction.set_segment_prefix(segment_prefix);

	debug_assert_eq!(3, instruction.op_count());
	instruction
}
