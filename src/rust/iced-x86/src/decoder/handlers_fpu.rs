// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::decoder::handlers::*;
use crate::decoder::*;
use crate::instruction_internal;

// SAFETY:
//	code: let this = unsafe { &*(self_ptr as *const Self) };
// The first arg (`self_ptr`) to decode() is always the handler itself, cast to a `*const OpCodeHandler`.
// All handlers are `#[repr(C)]` structs so the OpCodeHandler fields are always at the same offsets.

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

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction_internal::internal_set_op0_register_u32(instruction, Register::ST0 as u32);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction_internal::internal_set_op1_register_u32(instruction, Register::ST0 as u32 + decoder.state.rm);
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

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction_internal::internal_set_op0_register_u32(instruction, Register::ST0 as u32 + decoder.state.rm);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction_internal::internal_set_op1_register_u32(instruction, Register::ST0 as u32);
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

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction_internal::internal_set_op0_register_u32(instruction, Register::ST0 as u32 + decoder.state.rm);
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

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if decoder.state.operand_size != OpSize::Size16 {
			instruction_internal::internal_set_code_u32(instruction, this.code32);
		} else {
			instruction_internal::internal_set_code_u32(instruction, this.code16);
		}
		debug_assert_ne!(decoder.state.mod_, 3);
		instruction.set_op0_kind(OpKind::Memory);
		decoder.read_op_mem(instruction);
	}
}
