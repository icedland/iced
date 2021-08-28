// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::decoder::handlers::*;
use crate::decoder::*;
use crate::iced_constants::IcedConstants;

// SAFETY:
//	code: let this = unsafe { &*(self_ptr as *const Self) };
// The first arg (`self_ptr`) to decode() is always the handler itself, cast to a `*const OpCodeHandler`.
// All handlers are `#[repr(C)]` structs so the OpCodeHandler fields are always at the same offsets.

macro_rules! write_op0_reg {
	($instruction:ident, $expr:expr) => {
		debug_assert!($expr < IcedConstants::REGISTER_ENUM_COUNT as u32);
		$instruction.set_op0_register(unsafe { mem::transmute($expr as RegisterUnderlyingType) });
	};
}

macro_rules! write_op1_reg {
	($instruction:ident, $expr:expr) => {
		debug_assert!($expr < IcedConstants::REGISTER_ENUM_COUNT as u32);
		$instruction.set_op1_register(unsafe { mem::transmute($expr as RegisterUnderlyingType) });
	};
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_ST_STi {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
}

impl OpCodeHandler_ST_STi {
	#[cold]
	pub(super) fn new(code: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_ST_STi::decode, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(Register::ST0);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		write_op1_reg!(instruction, Register::ST0 as u32 + decoder.state.rm);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_STi_ST {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
}

impl OpCodeHandler_STi_ST {
	#[cold]
	pub(super) fn new(code: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_STi_ST::decode, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(instruction, Register::ST0 as u32 + decoder.state.rm);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction.set_op1_register(Register::ST0);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_STi {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
}

impl OpCodeHandler_STi {
	#[cold]
	pub(super) fn new(code: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_STi::decode, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(instruction, Register::ST0 as u32 + decoder.state.rm);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Mf {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code16: Code,
	code32: Code,
}

impl OpCodeHandler_Mf {
	#[cold]
	pub(super) fn new(code: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Mf::decode, code16: code, code32: code }
	}

	#[cold]
	pub(super) fn new1(code16: Code, code32: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Mf::decode, code16, code32 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		if decoder.state.operand_size != OpSize::Size16 {
			instruction.set_code(this.code32);
		} else {
			instruction.set_code(this.code16);
		}
		debug_assert_ne!(decoder.state.mod_, 3);
		instruction.set_op0_kind(OpKind::Memory);
		decoder.read_op_mem(instruction);
	}
}
