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

#![allow(dead_code)] //TODO: REMOVE

use super::*;

#[inline]
pub(crate) fn internal_set_code_size(this: &mut Instruction, new_value: CodeSize) {
	this.op_kind_flags |= (new_value as u32) << OpKindFlags::CODE_SIZE_SHIFT;
}

#[inline]
pub(crate) fn internal_set_code(this: &mut Instruction, new_value: Code) {
	this.code_flags |= new_value as u32;
}

#[inline]
pub(crate) fn internal_set_code_u32(this: &mut Instruction, new_value: u32) {
	this.code_flags |= new_value;
}

#[inline]
pub(crate) fn internal_set_code_no_check(this: &mut Instruction, new_value: Code) {
	this.code_flags = (this.code_flags & !CodeFlags::CODE_MASK) | new_value as u32;
}

#[inline]
pub(crate) fn internal_set_len(this: &mut Instruction, new_value: u32) {
	this.code_flags |= new_value << CodeFlags::INSTR_LENGTH_SHIFT;
}

#[cfg_attr(has_must_use, must_use)]
#[inline]
pub(crate) fn internal_has_repe_prefix_has_xrelease_prefix(this: &mut Instruction) -> bool {
	(this.code_flags & (CodeFlags::REPE_PREFIX | CodeFlags::XRELEASE_PREFIX)) != 0
}

#[cfg_attr(has_must_use, must_use)]
#[inline]
pub(crate) fn internal_has_repne_prefix_has_xacquire_prefix(this: &mut Instruction) -> bool {
	(this.code_flags & (CodeFlags::REPNE_PREFIX | CodeFlags::XACQUIRE_PREFIX)) != 0
}

#[cfg_attr(has_must_use, must_use)]
#[inline]
pub(crate) fn internal_has_repe_or_repne_prefix(this: &mut Instruction) -> bool {
	(this.code_flags & (CodeFlags::REPE_PREFIX | CodeFlags::REPNE_PREFIX)) != 0
}

#[inline]
pub(crate) fn internal_set_has_xacquire_prefix(this: &mut Instruction) {
	this.code_flags |= CodeFlags::XACQUIRE_PREFIX
}

#[inline]
pub(crate) fn internal_set_has_xrelease_prefix(this: &mut Instruction) {
	this.code_flags |= CodeFlags::XRELEASE_PREFIX
}

#[inline]
pub(crate) fn internal_set_has_repe_prefix(this: &mut Instruction) {
	this.code_flags |= CodeFlags::REPE_PREFIX
}

#[inline]
pub(crate) fn internal_clear_has_repe_prefix(this: &mut Instruction) {
	this.code_flags &= !CodeFlags::REPE_PREFIX
}

#[inline]
pub(crate) fn internal_set_has_repne_prefix(this: &mut Instruction) {
	this.code_flags |= CodeFlags::REPNE_PREFIX
}

#[inline]
pub(crate) fn internal_clear_has_repne_prefix(this: &mut Instruction) {
	this.code_flags &= !CodeFlags::REPNE_PREFIX
}

#[inline]
pub(crate) fn internal_set_has_lock_prefix(this: &mut Instruction) {
	this.code_flags |= CodeFlags::LOCK_PREFIX
}

#[inline]
pub(crate) fn internal_clear_has_lock_prefix(this: &mut Instruction) {
	this.code_flags &= !CodeFlags::LOCK_PREFIX
}

#[inline]
pub(crate) fn internal_set_op0_kind(this: &mut Instruction, new_value: OpKind) {
	this.op_kind_flags |= new_value as u32;
}

#[cfg_attr(has_must_use, must_use)]
#[inline]
pub(crate) fn internal_op0_is_not_reg_or_op0_is_not_reg(this: &mut Instruction) -> bool {
	(this.op_kind_flags & (OpKindFlags::OP_KIND_MASK | (OpKindFlags::OP_KIND_MASK << OpKindFlags::OP1_KIND_SHIFT))) != 0
}

#[inline]
pub(crate) fn internal_set_op1_kind(this: &mut Instruction, new_value: OpKind) {
	this.op_kind_flags |= (new_value as u32) << OpKindFlags::OP1_KIND_SHIFT;
}

#[inline]
pub(crate) fn internal_set_op2_kind(this: &mut Instruction, new_value: OpKind) {
	this.op_kind_flags |= (new_value as u32) << OpKindFlags::OP2_KIND_SHIFT;
}

#[inline]
pub(crate) fn internal_set_op3_kind(this: &mut Instruction, new_value: OpKind) {
	this.op_kind_flags |= (new_value as u32) << OpKindFlags::OP3_KIND_SHIFT;
}

#[inline]
pub(crate) fn internal_set_memory_displ_size(this: &mut Instruction, new_value: u32) {
	debug_assert!(new_value <= 4);
	this.memory_flags |= (new_value << MemoryFlags::DISPL_SIZE_SHIFT) as u16;
}

#[inline]
pub(crate) fn internal_set_is_broadcast(this: &mut Instruction) {
	this.memory_flags |= MemoryFlags::BROADCAST as u16;
}

#[cfg_attr(has_must_use, must_use)]
#[inline]
pub(crate) fn internal_get_memory_index_scale(this: &Instruction) -> i32 {
	(this.memory_flags & (MemoryFlags::SCALE_MASK as u16)) as i32
}

#[inline]
pub(crate) fn internal_set_memory_index_scale(this: &mut Instruction, new_value: i32) {
	this.memory_flags |= new_value as u16;
}

#[inline]
pub(crate) fn internal_set_immediate8(this: &mut Instruction, new_value: u32) {
	this.immediate = new_value;
}

#[inline]
pub(crate) fn internal_set_immediate8_2nd(this: &mut Instruction, new_value: u32) {
	this.mem_displ = new_value;
}

#[inline]
pub(crate) fn internal_set_immediate16(this: &mut Instruction, new_value: u32) {
	this.immediate = new_value;
}

#[inline]
pub(crate) fn internal_set_immediate64_lo(this: &mut Instruction, new_value: u32) {
	this.immediate = new_value;
}

#[inline]
pub(crate) fn internal_set_immediate64_hi(this: &mut Instruction, new_value: u32) {
	this.mem_displ = new_value;
}

#[inline]
pub(crate) fn internal_set_memory_address64_lo(this: &mut Instruction, new_value: u32) {
	this.immediate = new_value;
}

#[inline]
pub(crate) fn internal_set_memory_address64_hi(this: &mut Instruction, new_value: u32) {
	this.mem_displ = new_value;
}

#[inline]
pub(crate) fn internal_set_near_branch16(this: &mut Instruction, new_value: u32) {
	this.immediate = new_value;
}

#[inline]
pub(crate) fn internal_set_far_branch16(this: &mut Instruction, new_value: u32) {
	this.immediate = new_value;
}

#[inline]
pub(crate) fn internal_set_far_branch_selector(this: &mut Instruction, new_value: u32) {
	this.mem_displ = new_value;
}

#[inline]
pub(crate) fn internal_set_memory_base(this: &mut Instruction, new_value: Register) {
	this.mem_base_reg = new_value as u8;
}

#[inline]
pub(crate) fn internal_set_memory_base_u32(this: &mut Instruction, new_value: u32) {
	this.mem_base_reg = new_value as u8;
}

#[inline]
pub(crate) fn internal_set_memory_index(this: &mut Instruction, new_value: Register) {
	this.mem_index_reg = new_value as u8;
}

#[inline]
pub(crate) fn internal_set_memory_index_u32(this: &mut Instruction, new_value: u32) {
	this.mem_index_reg = new_value as u8;
}

#[inline]
pub(crate) fn internal_set_op0_register(this: &mut Instruction, new_value: Register) {
	this.reg0 = new_value as u8;
}

#[inline]
pub(crate) fn internal_set_op0_register_u32(this: &mut Instruction, new_value: u32) {
	this.reg0 = new_value as u8;
}

#[inline]
pub(crate) fn internal_set_op1_register(this: &mut Instruction, new_value: Register) {
	this.reg1 = new_value as u8;
}

#[inline]
pub(crate) fn internal_set_op1_register_u32(this: &mut Instruction, new_value: u32) {
	this.reg1 = new_value as u8;
}

#[inline]
pub(crate) fn internal_set_op2_register(this: &mut Instruction, new_value: Register) {
	this.reg2 = new_value as u8;
}

#[inline]
pub(crate) fn internal_set_op2_register_u32(this: &mut Instruction, new_value: u32) {
	this.reg2 = new_value as u8;
}

#[inline]
pub(crate) fn internal_set_op3_register_u32(this: &mut Instruction, new_value: u32) {
	this.reg3 = new_value as u8;
}

#[cfg_attr(has_must_use, must_use)]
#[inline]
pub(crate) fn internal_op_mask(this: &mut Instruction) -> u32 {
	(this.code_flags >> CodeFlags::OP_MASK_SHIFT) & CodeFlags::OP_MASK_MASK
}

#[inline]
pub(crate) fn internal_set_op_mask(this: &mut Instruction, new_value: u32) {
	this.code_flags |= new_value << CodeFlags::OP_MASK_SHIFT
}

#[inline]
pub(crate) fn internal_set_zeroing_masking(this: &mut Instruction) {
	this.code_flags |= CodeFlags::ZEROING_MASKING;
}

#[inline]
pub(crate) fn internal_set_rounding_control(this: &mut Instruction, new_value: u32) {
	this.code_flags |= new_value << CodeFlags::ROUNDING_CONTROL_SHIFT;
}

#[inline]
pub(crate) fn internal_set_declare_data_len(this: &mut Instruction, new_value: u32) {
	this.op_kind_flags |= (new_value - 1) << OpKindFlags::DATA_LENGTH_SHIFT;
}

#[inline]
pub(crate) fn internal_set_suppress_all_exceptions(this: &mut Instruction) {
	this.code_flags |= CodeFlags::SUPPRESS_ALL_EXCEPTIONS;
}

#[cfg_attr(has_must_use, must_use)]
pub(crate) fn get_address_size_in_bytes(base_reg: Register, index_reg: Register, displ_size: i32, code_size: CodeSize) -> i32 {
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
	if base_reg == Register::BX
		|| base_reg == Register::BP
		|| base_reg == Register::SI
		|| base_reg == Register::DI
		|| index_reg == Register::SI
		|| index_reg == Register::DI
	{
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
