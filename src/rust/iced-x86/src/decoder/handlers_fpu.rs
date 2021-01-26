// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

use super::handlers::*;
use super::*;

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_ST_STi {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
}

impl OpCodeHandler_ST_STi {
	pub(super) fn new(code: u32) -> Self {
		Self { decode: OpCodeHandler_ST_STi::decode, has_modrm: true, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, Register::ST0 as u32);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, Register::ST0 as u32 + decoder.state.rm);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_STi_ST {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
}

impl OpCodeHandler_STi_ST {
	pub(super) fn new(code: u32) -> Self {
		Self { decode: OpCodeHandler_STi_ST::decode, has_modrm: true, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, Register::ST0 as u32 + decoder.state.rm);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, Register::ST0 as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_STi {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
}

impl OpCodeHandler_STi {
	pub(super) fn new(code: u32) -> Self {
		Self { decode: OpCodeHandler_STi::decode, has_modrm: true, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, Register::ST0 as u32 + decoder.state.rm);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Mf {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code16: u32,
	code32: u32,
}

impl OpCodeHandler_Mf {
	pub(super) fn new(code: u32) -> Self {
		Self { decode: OpCodeHandler_Mf::decode, has_modrm: true, code16: code, code32: code }
	}

	pub(super) fn new1(code16: u32, code32: u32) -> Self {
		Self { decode: OpCodeHandler_Mf::decode, has_modrm: true, code16, code32 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.operand_size != OpSize::Size16 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
		}
		debug_assert_ne!(3, decoder.state.mod_);
		super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
		decoder.read_op_mem(instruction);
	}
}
