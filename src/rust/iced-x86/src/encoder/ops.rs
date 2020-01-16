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

use super::super::*;
use super::enums::*;
use super::Encoder;
use core::mem;

pub(crate) trait Op {
	fn encode(&self, encoder: &mut Encoder, instruction: &Instruction, operand: u32);

	/// If this is an immediate operand, it returns the `OpKind` value, else it returns `None`
	fn immediate_op_kind(&self) -> Option<OpKind> {
		None
	}

	/// If this is a near branch operand, it returns the `OpKind` value, else it returns `None`
	fn near_branch_op_kind(&self) -> Option<OpKind> {
		None
	}

	/// If this is a far branch operand, it returns the `OpKind` value, else it returns `None`
	fn far_branch_op_kind(&self) -> Option<OpKind> {
		None
	}
}

pub(crate) struct InvalidOpHandler;
impl Op for InvalidOpHandler {
	fn encode(&self, _encoder: &mut Encoder, _instruction: &Instruction, _operand: u32) {
		unreachable!()
	}
}

#[allow(non_camel_case_types)]
pub(crate) struct OpModRM_rm_mem_only;
impl Op for OpModRM_rm_mem_only {
	fn encode(&self, encoder: &mut Encoder, instruction: &Instruction, operand: u32) {
		encoder.add_reg_or_mem(instruction, operand, Register::None, Register::None, true, false);
	}
}

#[allow(non_camel_case_types)]
pub(crate) struct OpModRM_rm {
	pub(crate) reg_lo: Register,
	pub(crate) reg_hi: Register,
}
impl Op for OpModRM_rm {
	fn encode(&self, encoder: &mut Encoder, instruction: &Instruction, operand: u32) {
		encoder.add_reg_or_mem(instruction, operand, self.reg_lo, self.reg_hi, true, true);
	}
}

#[allow(non_camel_case_types)]
pub(crate) struct OpRegEmbed8 {
	pub(crate) reg_lo: Register,
	pub(crate) reg_hi: Register,
}
impl Op for OpRegEmbed8 {
	fn encode(&self, encoder: &mut Encoder, instruction: &Instruction, operand: u32) {
		encoder.add_reg(instruction, operand, self.reg_lo, self.reg_hi);
	}
}

#[allow(non_camel_case_types)]
pub(crate) struct OpModRM_rm_reg_only {
	pub(crate) reg_lo: Register,
	pub(crate) reg_hi: Register,
}
impl Op for OpModRM_rm_reg_only {
	fn encode(&self, encoder: &mut Encoder, instruction: &Instruction, operand: u32) {
		encoder.add_reg_or_mem(instruction, operand, self.reg_lo, self.reg_hi, false, true);
	}
}

#[allow(non_camel_case_types)]
pub(crate) struct OpModRM_reg {
	pub(crate) reg_lo: Register,
	pub(crate) reg_hi: Register,
}
impl Op for OpModRM_reg {
	fn encode(&self, encoder: &mut Encoder, instruction: &Instruction, operand: u32) {
		encoder.add_mod_rm_register(instruction, operand, self.reg_lo, self.reg_hi);
	}
}

#[allow(non_camel_case_types)]
pub(crate) struct OpModRM_reg_mem {
	pub(crate) reg_lo: Register,
	pub(crate) reg_hi: Register,
}
impl Op for OpModRM_reg_mem {
	fn encode(&self, encoder: &mut Encoder, instruction: &Instruction, operand: u32) {
		encoder.add_mod_rm_register(instruction, operand, self.reg_lo, self.reg_hi);
		encoder.encoder_flags |= EncoderFlags::REG_IS_MEMORY;
	}
}

#[allow(non_camel_case_types)]
pub(crate) struct OpModRM_regF0 {
	pub(crate) reg_lo: Register,
	pub(crate) reg_hi: Register,
}
impl Op for OpModRM_regF0 {
	fn encode(&self, encoder: &mut Encoder, instruction: &Instruction, operand: u32) {
		if encoder.bitness() != 64
			&& instruction.op_kind(operand) == OpKind::Register
			&& instruction.op_register(operand) as u32 == self.reg_lo as u32 + 8
		{
			encoder.encoder_flags |= EncoderFlags::PF0;
			let reg = unsafe { mem::transmute(self.reg_lo as u8 + 8) };
			encoder.add_mod_rm_register(instruction, operand, reg, reg);
		} else {
			encoder.add_mod_rm_register(instruction, operand, self.reg_lo, self.reg_hi);
		}
	}
}

#[allow(non_camel_case_types)]
pub(crate) struct OpReg {
	pub(crate) register: Register,
}
impl Op for OpReg {
	fn encode(&self, encoder: &mut Encoder, instruction: &Instruction, operand: u32) {
		let _ = encoder.verify_op_kind(operand, OpKind::Register, instruction.op_kind(operand));
		let _ = encoder.verify_register(operand, self.register, instruction.op_register(operand));
	}
}

#[allow(non_camel_case_types)]
pub(crate) struct OpRegSTi;
impl Op for OpRegSTi {
	fn encode(&self, encoder: &mut Encoder, instruction: &Instruction, operand: u32) {
		if !encoder.verify_op_kind(operand, OpKind::Register, instruction.op_kind(operand)) {
			return;
		}
		let reg = instruction.op_register(operand);
		if !encoder.verify_register_range(operand, reg, Register::ST0, Register::ST7) {
			return;
		}
		debug_assert!((encoder.op_code & 7) == 0);
		encoder.op_code |= reg as u32 - Register::ST0 as u32;
	}
}

#[allow(non_camel_case_types)]
pub(crate) struct OprDI;
impl OprDI {
	pub fn get_reg_size(op_kind: OpKind) -> u32 {
		match op_kind {
			OpKind::MemorySegRDI => 8,
			OpKind::MemorySegEDI => 4,
			OpKind::MemorySegDI => 2,
			_ => 0,
		}
	}
}
impl Op for OprDI {
	fn encode(&self, encoder: &mut Encoder, instruction: &Instruction, operand: u32) {
		let reg_size = Self::get_reg_size(instruction.op_kind(operand));
		if reg_size == 0 {
			encoder.set_error_message(format!(
				"Operand {}: expected OpKind = OpKind::MemorySegDI, OpKind::MemorySegEDI or OpKind::MemorySegRDI",
				operand
			));
			return;
		}
		encoder.set_addr_size(reg_size);
	}
}

#[allow(non_camel_case_types)]
pub(crate) struct OpIb {
	pub(crate) op_kind: OpKind,
}
impl Op for OpIb {
	fn encode(&self, encoder: &mut Encoder, instruction: &Instruction, operand: u32) {
		let op_imm_kind = instruction.op_kind(operand);
		if !encoder.verify_op_kind(operand, self.op_kind, op_imm_kind) {
			return;
		}
		encoder.imm_size = ImmSize::Size1;
		encoder.immediate = instruction.immediate8() as u32;
	}

	fn immediate_op_kind(&self) -> Option<OpKind> {
		Some(self.op_kind)
	}
}

#[allow(non_camel_case_types)]
pub(crate) struct OpIw;
impl Op for OpIw {
	fn encode(&self, encoder: &mut Encoder, instruction: &Instruction, operand: u32) {
		if !encoder.verify_op_kind(operand, OpKind::Immediate16, instruction.op_kind(operand)) {
			return;
		}
		encoder.imm_size = ImmSize::Size2;
		encoder.immediate = instruction.immediate16() as u32;
	}

	fn immediate_op_kind(&self) -> Option<OpKind> {
		Some(OpKind::Immediate16)
	}
}

#[allow(non_camel_case_types)]
pub(crate) struct OpId {
	pub(crate) op_kind: OpKind,
}
impl Op for OpId {
	fn encode(&self, encoder: &mut Encoder, instruction: &Instruction, operand: u32) {
		let op_imm_kind = instruction.op_kind(operand);
		if !encoder.verify_op_kind(operand, self.op_kind, op_imm_kind) {
			return;
		}
		encoder.imm_size = ImmSize::Size4;
		encoder.immediate = instruction.immediate32();
	}

	fn immediate_op_kind(&self) -> Option<OpKind> {
		Some(self.op_kind)
	}
}

#[allow(non_camel_case_types)]
pub(crate) struct OpIq;
impl Op for OpIq {
	fn encode(&self, encoder: &mut Encoder, instruction: &Instruction, operand: u32) {
		if !encoder.verify_op_kind(operand, OpKind::Immediate64, instruction.op_kind(operand)) {
			return;
		}
		encoder.imm_size = ImmSize::Size8;
		let imm = instruction.immediate64();
		encoder.immediate = imm as u32;
		encoder.immediate_hi = (imm >> 32) as u32;
	}

	fn immediate_op_kind(&self) -> Option<OpKind> {
		Some(OpKind::Immediate64)
	}
}

#[allow(non_camel_case_types)]
pub(crate) struct OpIb21;
impl Op for OpIb21 {
	fn encode(&self, encoder: &mut Encoder, instruction: &Instruction, operand: u32) {
		if !encoder.verify_op_kind(operand, OpKind::Immediate8_2nd, instruction.op_kind(operand)) {
			return;
		}
		debug_assert_eq!(ImmSize::Size2, encoder.imm_size);
		encoder.imm_size = ImmSize::Size2_1;
		encoder.immediate_hi = instruction.immediate8_2nd() as u32;
	}

	fn immediate_op_kind(&self) -> Option<OpKind> {
		Some(OpKind::Immediate8_2nd)
	}
}

#[allow(non_camel_case_types)]
pub(crate) struct OpIb11;
impl Op for OpIb11 {
	fn encode(&self, encoder: &mut Encoder, instruction: &Instruction, operand: u32) {
		if !encoder.verify_op_kind(operand, OpKind::Immediate8_2nd, instruction.op_kind(operand)) {
			return;
		}
		debug_assert_eq!(ImmSize::Size1, encoder.imm_size);
		encoder.imm_size = ImmSize::Size1_1;
		encoder.immediate_hi = instruction.immediate8_2nd() as u32;
	}

	fn immediate_op_kind(&self) -> Option<OpKind> {
		Some(OpKind::Immediate8_2nd)
	}
}

#[allow(non_camel_case_types)]
pub(crate) struct OpI2;
impl Op for OpI2 {
	fn encode(&self, encoder: &mut Encoder, instruction: &Instruction, operand: u32) {
		let op_imm_kind = instruction.op_kind(operand);
		if !encoder.verify_op_kind(operand, OpKind::Immediate8, op_imm_kind) {
			return;
		}
		debug_assert_eq!(ImmSize::SizeIbReg, encoder.imm_size);
		debug_assert_eq!(0, (encoder.immediate & 3));
		if instruction.immediate8() > 3 {
			encoder.set_error_message(format!("Operand {}: Immediate value must be 0-3, but value is 0x{:02X}", operand, instruction.immediate8()));
			return;
		}
		encoder.imm_size = ImmSize::Size1;
		encoder.immediate |= instruction.immediate8() as u32;
	}

	fn immediate_op_kind(&self) -> Option<OpKind> {
		Some(OpKind::Immediate8)
	}
}

#[allow(non_camel_case_types)]
pub(crate) struct OpX;
impl OpX {
	pub fn get_xreg_size(op_kind: OpKind) -> u32 {
		match op_kind {
			OpKind::MemorySegRSI => 8,
			OpKind::MemorySegESI => 4,
			OpKind::MemorySegSI => 2,
			_ => 0,
		}
	}

	pub fn get_yreg_size(op_kind: OpKind) -> u32 {
		match op_kind {
			OpKind::MemoryESRDI => 8,
			OpKind::MemoryESEDI => 4,
			OpKind::MemoryESDI => 2,
			_ => 0,
		}
	}
}
impl Op for OpX {
	fn encode(&self, encoder: &mut Encoder, instruction: &Instruction, operand: u32) {
		let regx_size = Self::get_xreg_size(instruction.op_kind(operand));
		if regx_size == 0 {
			encoder.set_error_message(format!(
				"Operand {}: expected OpKind = OpKind::MemorySegSI, OpKind::MemorySegESI or OpKind::MemorySegRSI",
				operand
			));
			return;
		}
		match instruction.code() {
			Code::Movsb_m8_m8 | Code::Movsw_m16_m16 | Code::Movsd_m32_m32 | Code::Movsq_m64_m64 => {
				let regy_size = Self::get_yreg_size(instruction.op0_kind());
				if regx_size != regy_size {
					encoder.set_error_message(format!(
						"Same sized register must be used: reg #1 size = {}, reg #2 size = {}",
						regy_size * 8,
						regx_size * 8
					));
					return;
				}
			}
			_ => {}
		}
		encoder.set_addr_size(regx_size);
	}
}

#[allow(non_camel_case_types)]
pub(crate) struct OpY;
impl Op for OpY {
	fn encode(&self, encoder: &mut Encoder, instruction: &Instruction, operand: u32) {
		let regy_size = OpX::get_yreg_size(instruction.op_kind(operand));
		if regy_size == 0 {
			encoder
				.set_error_message(format!("Operand {}: expected OpKind = OpKind::MemoryESDI, OpKind::MemoryESEDI or OpKind::MemoryESRDI", operand));
			return;
		}
		match instruction.code() {
			Code::Cmpsb_m8_m8 | Code::Cmpsw_m16_m16 | Code::Cmpsd_m32_m32 | Code::Cmpsq_m64_m64 => {
				let regx_size = OpX::get_xreg_size(instruction.op0_kind());
				if regx_size != regy_size {
					encoder.set_error_message(format!(
						"Same sized register must be used: reg #1 size = {}, reg #2 size = {}",
						regx_size * 8,
						regy_size * 8
					));
					return;
				}
			}
			_ => {}
		}
		encoder.set_addr_size(regy_size);
	}
}

#[allow(non_camel_case_types)]
pub(crate) struct OpMRBX;
impl Op for OpMRBX {
	fn encode(&self, encoder: &mut Encoder, instruction: &Instruction, operand: u32) {
		if !encoder.verify_op_kind(operand, OpKind::Memory, instruction.op_kind(operand)) {
			return;
		}
		let base = instruction.memory_base();
		if instruction.memory_displ_size() != 0
			|| instruction.memory_index() != Register::AL
			|| (base != Register::BX && base != Register::EBX && base != Register::RBX)
		{
			encoder.set_error_message(format!("Operand {}: Operand must be [bx+al], [ebx+al], or [rbx+al]", operand));
			return;
		}
		let reg_size = if base == Register::RBX {
			8
		} else if base == Register::EBX {
			4
		} else {
			debug_assert_eq!(Register::BX, base);
			2
		};
		encoder.set_addr_size(reg_size);
	}
}

#[allow(non_camel_case_types)]
pub(crate) struct OpJ {
	pub(crate) op_kind: OpKind,
	pub(crate) imm_size: u32,
}
impl Op for OpJ {
	fn encode(&self, encoder: &mut Encoder, instruction: &Instruction, operand: u32) {
		encoder.add_branch(self.op_kind, self.imm_size, instruction, operand);
	}

	fn near_branch_op_kind(&self) -> Option<OpKind> {
		Some(self.op_kind)
	}
}

#[allow(non_camel_case_types)]
pub(crate) struct OpJx {
	pub(crate) imm_size: u32,
}
impl Op for OpJx {
	fn encode(&self, encoder: &mut Encoder, instruction: &Instruction, operand: u32) {
		encoder.add_branch_x(self.imm_size, instruction, operand);
	}

	fn near_branch_op_kind(&self) -> Option<OpKind> {
		// xbegin is special and doesn't mask the target IP. We need to know the code size to return the correct value.
		// Instruction::with_xbegin() should be used to create the instruction and this method should never be called.
		debug_assert!(false, "Call Instruction::with_xbegin()");
		None
	}
}

#[allow(non_camel_case_types)]
pub(crate) struct OpJdisp {
	pub(crate) displ_size: u32,
}
impl Op for OpJdisp {
	fn encode(&self, encoder: &mut Encoder, instruction: &Instruction, operand: u32) {
		encoder.add_branch_disp(self.displ_size, instruction, operand);
	}

	fn near_branch_op_kind(&self) -> Option<OpKind> {
		Some(if self.displ_size == 2 { OpKind::NearBranch16 } else { OpKind::NearBranch32 })
	}
}

#[allow(non_camel_case_types)]
pub(crate) struct OpA {
	pub(crate) size: u32,
}
impl Op for OpA {
	fn encode(&self, encoder: &mut Encoder, instruction: &Instruction, operand: u32) {
		encoder.add_far_branch(instruction, operand, self.size);
	}

	fn far_branch_op_kind(&self) -> Option<OpKind> {
		Some(if self.size == 2 { OpKind::FarBranch16 } else { OpKind::FarBranch32 })
	}
}

#[allow(non_camel_case_types)]
pub(crate) struct OpO;
impl Op for OpO {
	fn encode(&self, encoder: &mut Encoder, instruction: &Instruction, operand: u32) {
		encoder.add_abs_mem(instruction, operand);
	}
}

#[allow(non_camel_case_types)]
pub(crate) struct OpImm {
	pub(crate) value: u8,
}
impl Op for OpImm {
	fn encode(&self, encoder: &mut Encoder, instruction: &Instruction, operand: u32) {
		if !encoder.verify_op_kind(operand, OpKind::Immediate8, instruction.op_kind(operand)) {
			return;
		}
		if instruction.immediate8() != self.value {
			encoder.set_error_message(format!("Operand {}: Expected 0x{:02X}, actual: 0x{:02X}", operand, self.value, instruction.immediate8()));
			return;
		}
	}

	fn immediate_op_kind(&self) -> Option<OpKind> {
		Some(OpKind::Immediate8)
	}
}

#[allow(non_camel_case_types)]
pub(crate) struct OpHx {
	pub(crate) reg_lo: Register,
	pub(crate) reg_hi: Register,
}
impl Op for OpHx {
	fn encode(&self, encoder: &mut Encoder, instruction: &Instruction, operand: u32) {
		if !encoder.verify_op_kind(operand, OpKind::Register, instruction.op_kind(operand)) {
			return;
		}
		let reg = instruction.op_register(operand);
		if !encoder.verify_register_range(operand, reg, self.reg_lo, self.reg_hi) {
			return;
		}
		encoder.encoder_flags |= (reg as u32 - self.reg_lo as u32) << EncoderFlags::VVVVV_SHIFT;
	}
}

#[allow(non_camel_case_types)]
pub(crate) struct OpVMx {
	pub(crate) vsib_index_reg_lo: Register,
	pub(crate) vsib_index_reg_hi: Register,
}
impl Op for OpVMx {
	fn encode(&self, encoder: &mut Encoder, instruction: &Instruction, operand: u32) {
		encoder.add_reg_or_mem_full(
			instruction,
			operand,
			Register::None,
			Register::None,
			self.vsib_index_reg_lo,
			self.vsib_index_reg_hi,
			true,
			false,
		);
	}
}

#[allow(non_camel_case_types)]
pub(crate) struct OpIs4x {
	pub(crate) reg_lo: Register,
	pub(crate) reg_hi: Register,
}
impl Op for OpIs4x {
	fn encode(&self, encoder: &mut Encoder, instruction: &Instruction, operand: u32) {
		if !encoder.verify_op_kind(operand, OpKind::Register, instruction.op_kind(operand)) {
			return;
		}
		let reg = instruction.op_register(operand);
		if !encoder.verify_register_range(operand, reg, self.reg_lo, self.reg_hi) {
			return;
		}
		encoder.imm_size = ImmSize::SizeIbReg;
		encoder.immediate = (reg as u32 - self.reg_lo as u32) << 4;
	}
}
