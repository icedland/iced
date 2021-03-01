// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#![allow(clippy::useless_let_if_seq)]

use crate::decoder::handlers::*;
use crate::decoder::*;
use crate::*;

// SAFETY:
//	code: let this = unsafe { &*(self_ptr as *const Self) };
// The first arg (`self_ptr`) to decode() is always the handler itself, cast to a `*const OpCodeHandler`.
// All handlers are `#[repr(C)]` structs so the OpCodeHandler fields are always at the same offsets.

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VectorLength_VEX {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	handlers: [&'static OpCodeHandler; 4],
}

impl OpCodeHandler_VectorLength_VEX {
	#[allow(trivial_casts)]
	pub(super) fn new(has_modrm: bool, handler128: *const OpCodeHandler, handler256: *const OpCodeHandler) -> Self {
		const_assert_eq!(VectorLength::L128 as u32, 0);
		const_assert_eq!(VectorLength::L256 as u32, 1);
		const_assert_eq!(VectorLength::L512 as u32, 2);
		const_assert_eq!(VectorLength::Unknown as u32, 3);
		debug_assert!(!is_null_instance_handler(handler128));
		debug_assert!(!is_null_instance_handler(handler256));
		let handlers = unsafe {
			[
				&*handler128,
				&*handler256,
				&*(&INVALID_HANDLER as *const _ as *const OpCodeHandler),
				&*(&INVALID_HANDLER as *const _ as *const OpCodeHandler),
			]
		};
		debug_assert_eq!(handlers[0].has_modrm, has_modrm);
		debug_assert_eq!(handlers[1].has_modrm, has_modrm);
		Self { decode: OpCodeHandler_VectorLength_VEX::decode, has_modrm, handlers }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		let handler = unsafe { *this.handlers.get_unchecked(decoder.state.vector_length as usize) };
		(handler.decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_Simple {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
}

impl OpCodeHandler_VEX_Simple {
	pub(super) fn new(code: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_Simple::decode, has_modrm: false, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_VHEv {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code_w0: u32,
	code_w1: u32,
	base_reg: Register,
}

impl OpCodeHandler_VEX_VHEv {
	pub(super) fn new(base_reg: Register, code_w0: u32, code_w1: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_VHEv::decode, has_modrm: true, base_reg, code_w0, code_w1 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		let gpr;
		if (decoder.state.flags & decoder.is64_mode_and_w) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code_w1);
			gpr = Register::RAX as u32;
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code_w0);
			gpr = Register::EAX as u32;
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32,
		);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op2_register_u32(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_VHEvIb {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code_w0: u32,
	code_w1: u32,
	base_reg: Register,
}

impl OpCodeHandler_VEX_VHEvIb {
	pub(super) fn new(base_reg: Register, code_w0: u32, code_w1: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_VHEvIb::decode, has_modrm: true, base_reg, code_w0, code_w1 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		let gpr;
		if (decoder.state.flags & decoder.is64_mode_and_w) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code_w1);
			gpr = Register::RAX as u32;
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code_w0);
			gpr = Register::EAX as u32;
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32,
		);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op2_register_u32(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		super::instruction_internal::internal_set_op3_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_VW {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg1: Register,
	base_reg2: Register,
}

impl OpCodeHandler_VEX_VW {
	pub(super) fn new(base_reg1: Register, base_reg2: Register, code: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_VW::decode, has_modrm: true, base_reg1, base_reg2, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + this.base_reg1 as u32,
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg2 as u32,
			);
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_VX_Ev {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code32: u32,
	code64: u32,
}

impl OpCodeHandler_VEX_VX_Ev {
	pub(super) fn new(code32: u32, code64: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_VX_Ev::decode, has_modrm: true, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		let gpr;
		if (decoder.state.flags & decoder.is64_mode_and_w) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			gpr = Register::RAX as u32;
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			gpr = Register::EAX as u32;
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + Register::XMM0 as u32,
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_Ev_VX {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code32: u32,
	code64: u32,
}

impl OpCodeHandler_VEX_Ev_VX {
	pub(super) fn new(code32: u32, code64: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_Ev_VX::decode, has_modrm: true, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		let gpr;
		if (decoder.state.flags & decoder.is64_mode_and_w) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			gpr = Register::RAX as u32;
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			gpr = Register::EAX as u32;
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + Register::XMM0 as u32,
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_WV {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg1: Register,
	base_reg2: Register,
}

impl OpCodeHandler_VEX_WV {
	pub(super) fn new(reg: Register, code: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_WV::decode, has_modrm: true, base_reg1: reg, base_reg2: reg, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + this.base_reg1 as u32,
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg2 as u32,
			);
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_VM {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg: Register,
}

impl OpCodeHandler_VEX_VM {
	pub(super) fn new(base_reg: Register, code: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_VM::decode, has_modrm: true, base_reg, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32,
		);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_MV {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg: Register,
}

impl OpCodeHandler_VEX_MV {
	pub(super) fn new(base_reg: Register, code: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_MV::decode, has_modrm: true, base_reg, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32,
		);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_M {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
}

impl OpCodeHandler_VEX_M {
	pub(super) fn new(code: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_M::decode, has_modrm: true, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_RdRq {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code32: u32,
	code64: u32,
}

impl OpCodeHandler_VEX_RdRq {
	pub(super) fn new(code32: u32, code64: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_RdRq::decode, has_modrm: true, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		if (decoder.state.flags & decoder.is64_mode_and_w) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + Register::RAX as u32,
			);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + Register::EAX as u32,
			);
		}
		if decoder.state.mod_ != 3 {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_rDI_VX_RX {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg: Register,
}

impl OpCodeHandler_VEX_rDI_VX_RX {
	pub(super) fn new(base_reg: Register, code: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_rDI_VX_RX::decode, has_modrm: true, base_reg, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		if decoder.state.address_size == OpSize::Size64 {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::MemorySegRDI);
		} else if decoder.state.address_size == OpSize::Size32 {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::MemorySegEDI);
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::MemorySegDI);
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32,
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op2_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32,
			);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_VWIb {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code_w0: u32,
	code_w1: u32,
	base_reg1: Register,
	base_reg2: Register,
}

impl OpCodeHandler_VEX_VWIb {
	pub(super) fn new(base_reg1: Register, base_reg2: Register, code_w0: u32, code_w1: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_VWIb::decode, has_modrm: true, base_reg1, base_reg2, code_w0, code_w1 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		if (decoder.state.flags & decoder.is64_mode_and_w) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code_w1);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code_w0);
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + this.base_reg1 as u32,
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg2 as u32,
			);
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_WVIb {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg1: Register,
	base_reg2: Register,
}

impl OpCodeHandler_VEX_WVIb {
	pub(super) fn new(base_reg1: Register, base_reg2: Register, code: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_WVIb::decode, has_modrm: true, base_reg1, base_reg2, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg1 as u32,
			);
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + this.base_reg2 as u32,
		);
		super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_Ed_V_Ib {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code32: u32,
	code64: u32,
	base_reg: Register,
}

impl OpCodeHandler_VEX_Ed_V_Ib {
	pub(super) fn new(base_reg: Register, code32: u32, code64: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_Ed_V_Ib::decode, has_modrm: true, base_reg, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		let gpr;
		if (decoder.state.flags & decoder.is64_mode_and_w) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			gpr = Register::RAX as u32;
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			gpr = Register::EAX as u32;
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32,
		);
		super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_VHW {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code_r: u32,
	code_m: u32,
	base_reg1: Register,
	base_reg2: Register,
	base_reg3: Register,
}

impl OpCodeHandler_VEX_VHW {
	pub(super) fn new(base_reg1: Register, base_reg2: Register, base_reg3: Register, code_r: u32, code_m: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_VHW::decode, has_modrm: true, base_reg1, base_reg2, base_reg3, code_r, code_m }
	}

	pub(super) fn new1(base_reg1: Register, base_reg2: Register, base_reg3: Register, code: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_VHW::decode, has_modrm: true, base_reg1, base_reg2, base_reg3, code_r: code, code_m: code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + this.base_reg1 as u32,
		);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg2 as u32);
		if decoder.state.mod_ == 3 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code_r);
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op2_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg3 as u32,
			);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code_m);
			super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_VWH {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg: Register,
}

impl OpCodeHandler_VEX_VWH {
	pub(super) fn new(base_reg: Register, code: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_VWH::decode, has_modrm: true, base_reg, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32,
		);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op2_register_u32(instruction, decoder.state.vvvv + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32,
			);
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_WHV {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code_r: u32,
	base_reg: Register,
}

impl OpCodeHandler_VEX_WHV {
	pub(super) fn new(base_reg: Register, code: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_WHV::decode, has_modrm: true, base_reg, code_r: code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		debug_assert_eq!(decoder.state.mod_, 3);
		super::instruction_internal::internal_set_code_u32(instruction, this.code_r);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32,
		);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg as u32);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op2_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32,
		);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_VHM {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg: Register,
}

impl OpCodeHandler_VEX_VHM {
	pub(super) fn new(base_reg: Register, code: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_VHM::decode, has_modrm: true, base_reg, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32,
		);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_MHV {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg: Register,
}

impl OpCodeHandler_VEX_MHV {
	pub(super) fn new(base_reg: Register, code: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_MHV::decode, has_modrm: true, base_reg, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg as u32);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op2_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32,
		);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_VHWIb {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg1: Register,
	base_reg2: Register,
	base_reg3: Register,
}

impl OpCodeHandler_VEX_VHWIb {
	pub(super) fn new(base_reg1: Register, base_reg2: Register, base_reg3: Register, code: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_VHWIb::decode, has_modrm: true, base_reg1, base_reg2, base_reg3, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + this.base_reg1 as u32,
		);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg2 as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op2_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg3 as u32,
			);
		} else {
			super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		super::instruction_internal::internal_set_op3_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_HRIb {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg: Register,
}

impl OpCodeHandler_VEX_HRIb {
	pub(super) fn new(base_reg: Register, code: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_HRIb::decode, has_modrm: true, base_reg, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.vvvv + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32,
			);
		} else {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_VHWIs4 {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg: Register,
}

impl OpCodeHandler_VEX_VHWIs4 {
	pub(super) fn new(base_reg: Register, code: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_VHWIs4::decode, has_modrm: true, base_reg, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32,
		);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op2_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32,
			);
		} else {
			super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op3_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op3_register_u32(
			instruction,
			(((decoder.read_u8() as u32) >> 4) & decoder.reg15_mask) + this.base_reg as u32,
		);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_VHIs4W {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg: Register,
}

impl OpCodeHandler_VEX_VHIs4W {
	pub(super) fn new(base_reg: Register, code: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_VHIs4W::decode, has_modrm: true, base_reg, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32,
		);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op3_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op3_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32,
			);
		} else {
			super::instruction_internal::internal_set_op3_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op2_register_u32(
			instruction,
			(((decoder.read_u8() as u32) >> 4) & decoder.reg15_mask) + this.base_reg as u32,
		);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_VHWIs5 {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg: Register,
}

impl OpCodeHandler_VEX_VHWIs5 {
	pub(super) fn new(base_reg: Register, code: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_VHWIs5::decode, has_modrm: true, base_reg, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32,
		);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op2_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32,
			);
		} else {
			super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		let ib = decoder.read_u8() as u32;
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op3_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op3_register_u32(instruction, ((ib >> 4) & decoder.reg15_mask) + this.base_reg as u32);
		debug_assert_eq!(instruction.op4_kind(), OpKind::Immediate8); // It's hard coded
		super::instruction_internal::internal_set_immediate8(instruction, ib & 0xF);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_VHIs5W {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg: Register,
}

impl OpCodeHandler_VEX_VHIs5W {
	pub(super) fn new(base_reg: Register, code: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_VHIs5W::decode, has_modrm: true, base_reg, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32,
		);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op3_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op3_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32,
			);
		} else {
			super::instruction_internal::internal_set_op3_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		let ib = decoder.read_u8() as u32;
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op2_register_u32(instruction, ((ib >> 4) & decoder.reg15_mask) + this.base_reg as u32);
		debug_assert_eq!(instruction.op4_kind(), OpKind::Immediate8); // It's hard coded
		super::instruction_internal::internal_set_immediate8(instruction, ib & 0xF);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_VK_HK_RK {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
}

impl OpCodeHandler_VEX_VK_HK_RK {
	pub(super) fn new(code: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_VK_HK_RK::decode, has_modrm: true, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		if decoder.invalid_check_mask != 0 && (decoder.state.vvvv > 7 || decoder.state.extra_register_base != 0) {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.reg + Register::K0 as u32);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, (decoder.state.vvvv & 7) + Register::K0 as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op2_register_u32(instruction, decoder.state.rm + Register::K0 as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_VK_RK {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
}

impl OpCodeHandler_VEX_VK_RK {
	pub(super) fn new(code: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_VK_RK::decode, has_modrm: true, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		if ((decoder.state.vvvv_invalid_check | decoder.state.extra_register_base) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.reg + Register::K0 as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.rm + Register::K0 as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_VK_RK_Ib {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
}

impl OpCodeHandler_VEX_VK_RK_Ib {
	pub(super) fn new(code: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_VK_RK_Ib::decode, has_modrm: true, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		if ((decoder.state.vvvv_invalid_check | decoder.state.extra_register_base) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.reg + Register::K0 as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.rm + Register::K0 as u32);
		} else {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_VK_WK {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
}

impl OpCodeHandler_VEX_VK_WK {
	pub(super) fn new(code: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_VK_WK::decode, has_modrm: true, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		if ((decoder.state.vvvv_invalid_check | decoder.state.extra_register_base) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.reg + Register::K0 as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.rm + Register::K0 as u32);
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_M_VK {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
}

impl OpCodeHandler_VEX_M_VK {
	pub(super) fn new(code: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_M_VK::decode, has_modrm: true, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		if ((decoder.state.vvvv_invalid_check | decoder.state.extra_register_base) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.reg + Register::K0 as u32);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_VK_R {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	gpr: Register,
}

impl OpCodeHandler_VEX_VK_R {
	pub(super) fn new(code: u32, gpr: Register) -> Self {
		Self { decode: OpCodeHandler_VEX_VK_R::decode, has_modrm: true, code, gpr }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		if ((decoder.state.vvvv_invalid_check | decoder.state.extra_register_base) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.reg + Register::K0 as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + this.gpr as u32,
			);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_G_VK {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	gpr: Register,
}

impl OpCodeHandler_VEX_G_VK {
	pub(super) fn new(code: u32, gpr: Register) -> Self {
		Self { decode: OpCodeHandler_VEX_G_VK::decode, has_modrm: true, code, gpr }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + this.gpr as u32,
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.rm + Register::K0 as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_Gv_W {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code_w0: u32,
	code_w1: u32,
	base_reg: Register,
}

impl OpCodeHandler_VEX_Gv_W {
	pub(super) fn new(base_reg: Register, code_w0: u32, code_w1: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_Gv_W::decode, has_modrm: true, base_reg, code_w0, code_w1 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		if (decoder.state.flags & decoder.is64_mode_and_w) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code_w1);
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32,
			);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code_w0);
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32,
			);
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32,
			);
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_Gv_RX {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code32: u32,
	code64: u32,
	base_reg: Register,
}

impl OpCodeHandler_VEX_Gv_RX {
	pub(super) fn new(base_reg: Register, code32: u32, code64: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_Gv_RX::decode, has_modrm: true, base_reg, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		let gpr;
		if (decoder.state.flags & decoder.is64_mode_and_w) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			gpr = Register::RAX as u32;
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			gpr = Register::EAX as u32;
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.reg + decoder.state.extra_register_base + gpr);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32,
			);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_Gv_GPR_Ib {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code32: u32,
	code64: u32,
	base_reg: Register,
}

impl OpCodeHandler_VEX_Gv_GPR_Ib {
	pub(super) fn new(base_reg: Register, code32: u32, code64: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_Gv_GPR_Ib::decode, has_modrm: true, base_reg, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		let gpr;
		if (decoder.state.flags & decoder.is64_mode_and_w) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			gpr = Register::RAX as u32;
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			gpr = Register::EAX as u32;
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.reg + decoder.state.extra_register_base + gpr);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32,
			);
		} else {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_VX_VSIB_HX {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg1: Register,
	vsib_index: Register,
	base_reg3: Register,
}

impl OpCodeHandler_VEX_VX_VSIB_HX {
	pub(super) fn new(base_reg1: Register, vsib_index: Register, base_reg3: Register, code: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_VX_VSIB_HX::decode, has_modrm: true, base_reg1, vsib_index, base_reg3, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		let reg_num = decoder.state.reg + decoder.state.extra_register_base;
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, reg_num + this.base_reg1 as u32);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op2_register_u32(instruction, decoder.state.vvvv + this.base_reg3 as u32);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem_vsib(instruction, this.vsib_index, TupleType::N1);
			if decoder.invalid_check_mask != 0 {
				let index_num = (instruction.memory_index() as u32).wrapping_sub(Register::XMM0 as u32) % IcedConstants::VMM_COUNT;
				if reg_num == index_num || decoder.state.vvvv == index_num || reg_num == decoder.state.vvvv {
					decoder.set_invalid_instruction();
				}
			}
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_Gv_Gv_Ev {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code32: u32,
	code64: u32,
}

impl OpCodeHandler_VEX_Gv_Gv_Ev {
	pub(super) fn new(code32: u32, code64: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_Gv_Gv_Ev::decode, has_modrm: true, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		let gpr;
		if (decoder.state.flags & decoder.is64_mode_and_w) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			gpr = Register::RAX as u32;
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			gpr = Register::EAX as u32;
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.reg + decoder.state.extra_register_base + gpr);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + gpr);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op2_register_u32(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_Gv_Ev_Gv {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code32: u32,
	code64: u32,
}

impl OpCodeHandler_VEX_Gv_Ev_Gv {
	pub(super) fn new(code32: u32, code64: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_Gv_Ev_Gv::decode, has_modrm: true, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		let gpr;
		if (decoder.state.flags & decoder.is64_mode_and_w) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			gpr = Register::RAX as u32;
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			gpr = Register::EAX as u32;
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.reg + decoder.state.extra_register_base + gpr);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op2_register_u32(instruction, decoder.state.vvvv + gpr);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_Hv_Ev {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code32: u32,
	code64: u32,
}

impl OpCodeHandler_VEX_Hv_Ev {
	pub(super) fn new(code32: u32, code64: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_Hv_Ev::decode, has_modrm: true, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		let gpr;
		if (decoder.state.flags & decoder.is64_mode_and_w) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			gpr = Register::RAX as u32;
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			gpr = Register::EAX as u32;
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.vvvv + gpr);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_Hv_Ed_Id {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code32: u32,
	code64: u32,
}

impl OpCodeHandler_VEX_Hv_Ed_Id {
	pub(super) fn new(code32: u32, code64: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_Hv_Ed_Id::decode, has_modrm: true, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		if (decoder.state.flags & decoder.is64_mode_and_w) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.vvvv + Register::RAX as u32);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.vvvv + Register::EAX as u32);
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + Register::EAX as u32,
			);
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Immediate32);
		instruction.set_immediate32(decoder.read_u32() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_GvM_VX_Ib {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code32: u32,
	code64: u32,
	base_reg: Register,
}

impl OpCodeHandler_VEX_GvM_VX_Ib {
	pub(super) fn new(base_reg: Register, code32: u32, code64: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_GvM_VX_Ib::decode, has_modrm: true, base_reg, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		let gpr;
		if (decoder.state.flags & decoder.is64_mode_and_w) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			gpr = Register::RAX as u32;
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			gpr = Register::EAX as u32;
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32,
		);
		super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_Gv_Ev_Ib {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code32: u32,
	code64: u32,
}

impl OpCodeHandler_VEX_Gv_Ev_Ib {
	pub(super) fn new(code32: u32, code64: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_Gv_Ev_Ib::decode, has_modrm: true, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		let gpr;
		if (decoder.state.flags & decoder.is64_mode_and_w) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			gpr = Register::RAX as u32;
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			gpr = Register::EAX as u32;
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.reg + decoder.state.extra_register_base + gpr);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_Gv_Ev_Id {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code32: u32,
	code64: u32,
}

impl OpCodeHandler_VEX_Gv_Ev_Id {
	pub(super) fn new(code32: u32, code64: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_Gv_Ev_Id::decode, has_modrm: true, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		let gpr;
		if (decoder.state.flags & decoder.is64_mode_and_w) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			gpr = Register::RAX as u32;
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			gpr = Register::EAX as u32;
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.reg + decoder.state.extra_register_base + gpr);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Immediate32);
		instruction.set_immediate32(decoder.read_u32() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_VT_SIBMEM {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
}

impl OpCodeHandler_VEX_VT_SIBMEM {
	pub(super) fn new(code: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_VT_SIBMEM::decode, has_modrm: true, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		if ((decoder.state.vvvv_invalid_check | decoder.state.extra_register_base) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.reg + Register::TMM0 as u32);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem_sib(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_SIBMEM_VT {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
}

impl OpCodeHandler_VEX_SIBMEM_VT {
	pub(super) fn new(code: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_SIBMEM_VT::decode, has_modrm: true, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		if ((decoder.state.vvvv_invalid_check | decoder.state.extra_register_base) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.reg + Register::TMM0 as u32);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem_sib(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_VT {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
}

impl OpCodeHandler_VEX_VT {
	pub(super) fn new(code: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_VT::decode, has_modrm: true, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		if ((decoder.state.vvvv_invalid_check | decoder.state.extra_register_base) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.reg + Register::TMM0 as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX_VT_RT_HT {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
}

impl OpCodeHandler_VEX_VT_RT_HT {
	pub(super) fn new(code: u32) -> Self {
		Self { decode: OpCodeHandler_VEX_VT_RT_HT::decode, has_modrm: true, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX || decoder.state.encoding() == EncodingKind::XOP);
		if decoder.invalid_check_mask != 0 && (decoder.state.vvvv > 7 || decoder.state.extra_register_base != 0) {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.reg + Register::TMM0 as u32);
		const_assert_eq!(OpKind::Register as u32, 0);
		//super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op2_register_u32(instruction, (decoder.state.vvvv & 7) + Register::TMM0 as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.rm + Register::TMM0 as u32);
			if decoder.invalid_check_mask != 0 {
				if decoder.state.extra_base_register_base != 0
					|| decoder.state.reg == decoder.state.vvvv
					|| decoder.state.reg == decoder.state.rm
					|| decoder.state.rm == decoder.state.vvvv
				{
					decoder.set_invalid_instruction();
				}
			}
		} else {
			decoder.set_invalid_instruction();
		}
	}
}
