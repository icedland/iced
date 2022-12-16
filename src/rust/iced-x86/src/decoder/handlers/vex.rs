// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#![allow(clippy::useless_let_if_seq)]

use crate::decoder::handlers::*;
use crate::decoder::*;
use crate::iced_constants::IcedConstants;
use crate::instruction_internal;
use crate::*;

// SAFETY:
//	code: let this = unsafe { &*(self_ptr as *const Self) };
// The first arg (`self_ptr`) to decode() is always the handler itself, cast to a `*const OpCodeHandler`.
// All handlers are `#[repr(C)]` structs so the OpCodeHandler fields are always at the same offsets.

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VectorLength_VEX {
	has_modrm: bool,
	handlers: [(OpCodeHandlerDecodeFn, &'static OpCodeHandler); 4],
}

impl OpCodeHandler_VectorLength_VEX {
	#[allow(trivial_casts)]
	#[inline]
	pub(in crate::decoder) fn new(
		has_modrm: bool, handler128: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), handler256: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	) -> (OpCodeHandlerDecodeFn, Self) {
		const _: () = assert!(VectorLength::L128 as u32 == 0);
		const _: () = assert!(VectorLength::L256 as u32 == 1);
		const _: () = assert!(VectorLength::L512 as u32 == 2);
		const _: () = assert!(VectorLength::Unknown as u32 == 3);
		debug_assert!(!is_null_instance_handler(handler128.1));
		debug_assert!(!is_null_instance_handler(handler256.1));
		let handlers = [handler128, handler256, get_invalid_handler(), get_invalid_handler()];
		debug_assert_eq!(handlers[0].1.has_modrm, has_modrm);
		debug_assert_eq!(handlers[1].1.has_modrm, has_modrm);
		(OpCodeHandler_VectorLength_VEX::decode, Self { has_modrm, handlers })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		let (decode, handler) = this.handlers[decoder.state.vector_length as usize];
		(decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_Simple {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_VEX_Simple {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_Simple::decode, Self { has_modrm: false, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		instruction.set_code(this.code);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_VHEv {
	has_modrm: bool,
	base_reg: Register,
	code_w0: Code,
	code_w1: Code,
}

impl OpCodeHandler_VEX_VHEv {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code_w0: Code, code_w1: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_VHEv::decode, Self { has_modrm: true, base_reg, code_w0, code_w1 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		let gpr;
		if (decoder.state.flags & decoder.is64b_mode_and_w) != 0 {
			instruction.set_code(this.code_w1);
			gpr = Register::RAX as u32;
		} else {
			instruction.set_code(this.code_w0);
			gpr = Register::EAX as u32;
		}
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			write_op2_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op2_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_VHEvIb {
	has_modrm: bool,
	base_reg: Register,
	code_w0: Code,
	code_w1: Code,
}

impl OpCodeHandler_VEX_VHEvIb {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code_w0: Code, code_w1: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_VHEvIb::decode, Self { has_modrm: true, base_reg, code_w0, code_w1 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		let gpr;
		if (decoder.state.flags & decoder.is64b_mode_and_w) != 0 {
			instruction.set_code(this.code_w1);
			gpr = Register::RAX as u32;
		} else {
			instruction.set_code(this.code_w0);
			gpr = Register::EAX as u32;
		}
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg as u32);
		instruction.set_op3_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			write_op2_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op2_kind(OpKind::Memory);
			});
		}
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_VW {
	has_modrm: bool,
	code: Code,
	base_reg1: Register,
	base_reg2: Register,
}

impl OpCodeHandler_VEX_VW {
	#[inline]
	pub(in crate::decoder) fn new(base_reg1: Register, base_reg2: Register, code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_VW::decode, Self { has_modrm: true, base_reg1, base_reg2, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + this.base_reg1 as u32);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg2 as u32);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op1_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_VX_Ev {
	has_modrm: bool,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_VEX_VX_Ev {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_VX_Ev::decode, Self { has_modrm: true, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		let gpr;
		if (decoder.state.flags & decoder.is64b_mode_and_w) != 0 {
			instruction.set_code(this.code64);
			gpr = Register::RAX as u32;
		} else {
			instruction.set_code(this.code32);
			gpr = Register::EAX as u32;
		}
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::XMM0 as u32);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op1_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_Ev_VX {
	has_modrm: bool,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_VEX_Ev_VX {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_Ev_VX::decode, Self { has_modrm: true, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		let gpr;
		if (decoder.state.flags & decoder.is64b_mode_and_w) != 0 {
			instruction.set_code(this.code64);
			gpr = Register::RAX as u32;
		} else {
			instruction.set_code(this.code32);
			gpr = Register::EAX as u32;
		}
		write_op1_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::XMM0 as u32);
		if decoder.state.mod_ == 3 {
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op0_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_WV {
	has_modrm: bool,
	code: Code,
	base_reg1: Register,
	base_reg2: Register,
}

impl OpCodeHandler_VEX_WV {
	#[inline]
	pub(in crate::decoder) fn new(reg: Register, code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_WV::decode, Self { has_modrm: true, base_reg1: reg, base_reg2: reg, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);
		write_op1_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + this.base_reg1 as u32);
		if decoder.state.mod_ == 3 {
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg2 as u32);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op0_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_VM {
	has_modrm: bool,
	base_reg: Register,
	code: Code,
}

impl OpCodeHandler_VEX_VM {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_VM::decode, Self { has_modrm: true, base_reg, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op1_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_MV {
	has_modrm: bool,
	base_reg: Register,
	code: Code,
}

impl OpCodeHandler_VEX_MV {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_MV::decode, Self { has_modrm: true, base_reg, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);
		write_op1_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op0_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_M {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_VEX_M {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_M::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op0_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_RdRq {
	has_modrm: bool,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_VEX_RdRq {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_RdRq::decode, Self { has_modrm: true, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		if (decoder.state.flags & decoder.is64b_mode_and_w) != 0 {
			instruction.set_code(this.code64);
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::RAX as u32);
		} else {
			instruction.set_code(this.code32);
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::EAX as u32);
		}
		if decoder.state.mod_ != 3 {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_rDI_VX_RX {
	has_modrm: bool,
	base_reg: Register,
	code: Code,
}

impl OpCodeHandler_VEX_rDI_VX_RX {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_rDI_VX_RX::decode, Self { has_modrm: true, base_reg, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);
		if decoder.state.address_size == OpSize::Size64 {
			instruction.set_op0_kind(OpKind::MemorySegRDI);
		} else if decoder.state.address_size == OpSize::Size32 {
			instruction.set_op0_kind(OpKind::MemorySegEDI);
		} else {
			instruction.set_op0_kind(OpKind::MemorySegDI);
		}
		write_op1_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			write_op2_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_VWIb {
	has_modrm: bool,
	code_w0: Code,
	code_w1: Code,
	base_reg1: Register,
	base_reg2: Register,
}

impl OpCodeHandler_VEX_VWIb {
	#[inline]
	pub(in crate::decoder) fn new(base_reg1: Register, base_reg2: Register, code_w0: Code, code_w1: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_VWIb::decode, Self { has_modrm: true, base_reg1, base_reg2, code_w0, code_w1 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		if (decoder.state.flags & decoder.is64b_mode_and_w) != 0 {
			instruction.set_code(this.code_w1);
		} else {
			instruction.set_code(this.code_w0);
		}
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + this.base_reg1 as u32);
		instruction.set_op2_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg2 as u32);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op1_kind(OpKind::Memory);
			});
		}
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_WVIb {
	has_modrm: bool,
	code: Code,
	base_reg1: Register,
	base_reg2: Register,
}

impl OpCodeHandler_VEX_WVIb {
	#[inline]
	pub(in crate::decoder) fn new(base_reg1: Register, base_reg2: Register, code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_WVIb::decode, Self { has_modrm: true, base_reg1, base_reg2, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);
		write_op1_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + this.base_reg2 as u32);
		instruction.set_op2_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg1 as u32);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op0_kind(OpKind::Memory);
			});
		}
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_Ed_V_Ib {
	has_modrm: bool,
	base_reg: Register,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_VEX_Ed_V_Ib {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_Ed_V_Ib::decode, Self { has_modrm: true, base_reg, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		write_op1_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32);
		instruction.set_op2_kind(OpKind::Immediate8);
		let gpr;
		if (decoder.state.flags & decoder.is64b_mode_and_w) != 0 {
			instruction.set_code(this.code64);
			gpr = Register::RAX as u32;
		} else {
			instruction.set_code(this.code32);
			gpr = Register::EAX as u32;
		}
		if decoder.state.mod_ == 3 {
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op0_kind(OpKind::Memory);
			});
		}
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_VHW {
	has_modrm: bool,
	base_reg1: Register,
	base_reg2: Register,
	base_reg3: Register,
	code_r: Code,
	code_m: Code,
}

impl OpCodeHandler_VEX_VHW {
	#[inline]
	pub(in crate::decoder) fn new(
		base_reg1: Register, base_reg2: Register, base_reg3: Register, code_r: Code, code_m: Code,
	) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_VHW::decode, Self { has_modrm: true, base_reg1, base_reg2, base_reg3, code_r, code_m })
	}

	#[inline]
	pub(in crate::decoder) fn new1(base_reg1: Register, base_reg2: Register, base_reg3: Register, code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_VHW::decode, Self { has_modrm: true, base_reg1, base_reg2, base_reg3, code_r: code, code_m: code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + this.base_reg1 as u32);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg2 as u32);
		if decoder.state.mod_ == 3 {
			instruction.set_code(this.code_r);
			write_op2_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg3 as u32);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_code(this.code_m);
				instruction.set_op2_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_VWH {
	has_modrm: bool,
	base_reg: Register,
	code: Code,
}

impl OpCodeHandler_VEX_VWH {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_VWH::decode, Self { has_modrm: true, base_reg, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		instruction.set_code(this.code);
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32);
		write_op2_reg!(instruction, decoder.state.vvvv + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op1_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_WHV {
	has_modrm: bool,
	base_reg: Register,
	code_r: Code,
}

impl OpCodeHandler_VEX_WHV {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_WHV::decode, Self { has_modrm: true, base_reg, code_r: code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		debug_assert_eq!(decoder.state.mod_, 3);
		instruction.set_code(this.code_r);
		write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg as u32);
		write_op2_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_VHM {
	has_modrm: bool,
	base_reg: Register,
	code: Code,
}

impl OpCodeHandler_VEX_VHM {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_VHM::decode, Self { has_modrm: true, base_reg, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		instruction.set_code(this.code);
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op2_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_MHV {
	has_modrm: bool,
	base_reg: Register,
	code: Code,
}

impl OpCodeHandler_VEX_MHV {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_MHV::decode, Self { has_modrm: true, base_reg, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		instruction.set_code(this.code);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg as u32);
		write_op2_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op0_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_VHWIb {
	has_modrm: bool,
	base_reg1: Register,
	base_reg2: Register,
	base_reg3: Register,
	code: Code,
}

impl OpCodeHandler_VEX_VHWIb {
	#[inline]
	pub(in crate::decoder) fn new(base_reg1: Register, base_reg2: Register, base_reg3: Register, code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_VHWIb::decode, Self { has_modrm: true, base_reg1, base_reg2, base_reg3, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		instruction.set_code(this.code);
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + this.base_reg1 as u32);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg2 as u32);
		instruction.set_op3_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			write_op2_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg3 as u32);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op2_kind(OpKind::Memory);
			});
		}
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_HRIb {
	has_modrm: bool,
	base_reg: Register,
	code: Code,
}

impl OpCodeHandler_VEX_HRIb {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_HRIb::decode, Self { has_modrm: true, base_reg, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		instruction.set_code(this.code);
		write_op0_reg!(instruction, decoder.state.vvvv + this.base_reg as u32);
		instruction.set_op2_kind(OpKind::Immediate8);
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_VHWIs4 {
	has_modrm: bool,
	base_reg: Register,
	code: Code,
}

impl OpCodeHandler_VEX_VHWIs4 {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_VHWIs4::decode, Self { has_modrm: true, base_reg, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		instruction.set_code(this.code);
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			write_op2_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op2_kind(OpKind::Memory);
			});
		}
		let b = decoder.read_u8();
		write_op3_reg!(instruction, (((b as u32) >> 4) & decoder.reg15_mask) + this.base_reg as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_VHIs4W {
	has_modrm: bool,
	base_reg: Register,
	code: Code,
}

impl OpCodeHandler_VEX_VHIs4W {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_VHIs4W::decode, Self { has_modrm: true, base_reg, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		instruction.set_code(this.code);
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			write_op3_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op3_kind(OpKind::Memory);
			});
		}
		let b = decoder.read_u8();
		write_op2_reg!(instruction, (((b as u32) >> 4) & decoder.reg15_mask) + this.base_reg as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_VHWIs5 {
	has_modrm: bool,
	base_reg: Register,
	code: Code,
}

impl OpCodeHandler_VEX_VHWIs5 {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_VHWIs5::decode, Self { has_modrm: true, base_reg, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		instruction.set_code(this.code);
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			write_op2_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op2_kind(OpKind::Memory);
			});
		}
		let ib = decoder.read_u8() as u32;
		write_op3_reg!(instruction, ((ib >> 4) & decoder.reg15_mask) + this.base_reg as u32);
		debug_assert_eq!(instruction.op4_kind(), OpKind::Immediate8); // It's hard coded
		instruction_internal::internal_set_immediate8(instruction, ib & 0xF);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_VHIs5W {
	has_modrm: bool,
	base_reg: Register,
	code: Code,
}

impl OpCodeHandler_VEX_VHIs5W {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_VHIs5W::decode, Self { has_modrm: true, base_reg, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		instruction.set_code(this.code);
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			write_op3_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op3_kind(OpKind::Memory);
			});
		}
		let ib = decoder.read_u8() as u32;
		write_op2_reg!(instruction, ((ib >> 4) & decoder.reg15_mask) + this.base_reg as u32);
		debug_assert_eq!(instruction.op4_kind(), OpKind::Immediate8); // It's hard coded
		instruction_internal::internal_set_immediate8(instruction, ib & 0xF);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_VK_HK_RK {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_VEX_VK_HK_RK {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_VK_HK_RK::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		if decoder.invalid_check_mask != 0 && (decoder.state.vvvv > 7 || decoder.state.extra_register_base != 0) {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);
		write_op0_reg!(instruction, decoder.state.reg + Register::K0 as u32);
		write_op1_reg!(instruction, (decoder.state.vvvv & 7) + Register::K0 as u32);
		if decoder.state.mod_ == 3 {
			write_op2_reg!(instruction, decoder.state.rm + Register::K0 as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_VK_RK {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_VEX_VK_RK {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_VK_RK::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		if ((decoder.state.vvvv_invalid_check | decoder.state.extra_register_base) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);
		write_op0_reg!(instruction, decoder.state.reg + Register::K0 as u32);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + Register::K0 as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_VK_RK_Ib {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_VEX_VK_RK_Ib {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_VK_RK_Ib::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		if ((decoder.state.vvvv_invalid_check | decoder.state.extra_register_base) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);
		write_op0_reg!(instruction, decoder.state.reg + Register::K0 as u32);
		instruction.set_op2_kind(OpKind::Immediate8);
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + Register::K0 as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_VK_WK {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_VEX_VK_WK {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_VK_WK::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		if ((decoder.state.vvvv_invalid_check | decoder.state.extra_register_base) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);
		write_op0_reg!(instruction, decoder.state.reg + Register::K0 as u32);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + Register::K0 as u32);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op1_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_M_VK {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_VEX_M_VK {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_M_VK::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		if ((decoder.state.vvvv_invalid_check | decoder.state.extra_register_base) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);
		write_op1_reg!(instruction, decoder.state.reg + Register::K0 as u32);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op0_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_VK_R {
	has_modrm: bool,
	gpr: Register,
	code: Code,
}

impl OpCodeHandler_VEX_VK_R {
	#[inline]
	pub(in crate::decoder) fn new(code: Code, gpr: Register) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_VK_R::decode, Self { has_modrm: true, code, gpr })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		if ((decoder.state.vvvv_invalid_check | decoder.state.extra_register_base) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);
		write_op0_reg!(instruction, decoder.state.reg + Register::K0 as u32);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + this.gpr as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_G_VK {
	has_modrm: bool,
	gpr: Register,
	code: Code,
}

impl OpCodeHandler_VEX_G_VK {
	#[inline]
	pub(in crate::decoder) fn new(code: Code, gpr: Register) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_G_VK::decode, Self { has_modrm: true, code, gpr })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + this.gpr as u32);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + Register::K0 as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_Gv_W {
	has_modrm: bool,
	base_reg: Register,
	code_w0: Code,
	code_w1: Code,
}

impl OpCodeHandler_VEX_Gv_W {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code_w0: Code, code_w1: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_Gv_W::decode, Self { has_modrm: true, base_reg, code_w0, code_w1 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		if (decoder.state.flags & decoder.is64b_mode_and_w) != 0 {
			instruction.set_code(this.code_w1);
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32);
		} else {
			instruction.set_code(this.code_w0);
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32);
		}
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op1_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_Gv_RX {
	has_modrm: bool,
	base_reg: Register,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_VEX_Gv_RX {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_Gv_RX::decode, Self { has_modrm: true, base_reg, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		let gpr;
		if (decoder.state.flags & decoder.is64b_mode_and_w) != 0 {
			instruction.set_code(this.code64);
			gpr = Register::RAX as u32;
		} else {
			instruction.set_code(this.code32);
			gpr = Register::EAX as u32;
		}
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + gpr);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_Gv_GPR_Ib {
	has_modrm: bool,
	base_reg: Register,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_VEX_Gv_GPR_Ib {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_Gv_GPR_Ib::decode, Self { has_modrm: true, base_reg, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		let gpr;
		if (decoder.state.flags & decoder.is64b_mode_and_w) != 0 {
			instruction.set_code(this.code64);
			gpr = Register::RAX as u32;
		} else {
			instruction.set_code(this.code32);
			gpr = Register::EAX as u32;
		}
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + gpr);
		instruction.set_op2_kind(OpKind::Immediate8);
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_VX_VSIB_HX {
	has_modrm: bool,
	base_reg1: Register,
	vsib_index: Register,
	base_reg3: Register,
	code: Code,
}

impl OpCodeHandler_VEX_VX_VSIB_HX {
	#[inline]
	pub(in crate::decoder) fn new(base_reg1: Register, vsib_index: Register, base_reg3: Register, code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_VX_VSIB_HX::decode, Self { has_modrm: true, base_reg1, vsib_index, base_reg3, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		instruction.set_code(this.code);
		let reg_num = decoder.state.reg + decoder.state.extra_register_base;
		write_op0_reg!(instruction, reg_num + this.base_reg1 as u32);
		write_op2_reg!(instruction, decoder.state.vvvv + this.base_reg3 as u32);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			instruction.set_op1_kind(OpKind::Memory);
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
pub(in crate::decoder) struct OpCodeHandler_VEX_Gv_Gv_Ev {
	has_modrm: bool,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_VEX_Gv_Gv_Ev {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_Gv_Gv_Ev::decode, Self { has_modrm: true, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		let gpr;
		if (decoder.state.flags & decoder.is64b_mode_and_w) != 0 {
			instruction.set_code(this.code64);
			gpr = Register::RAX as u32;
		} else {
			instruction.set_code(this.code32);
			gpr = Register::EAX as u32;
		}
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + gpr);
		write_op1_reg!(instruction, decoder.state.vvvv + gpr);
		if decoder.state.mod_ == 3 {
			write_op2_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op2_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_Gv_Ev_Gv {
	has_modrm: bool,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_VEX_Gv_Ev_Gv {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_Gv_Ev_Gv::decode, Self { has_modrm: true, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		let gpr;
		if (decoder.state.flags & decoder.is64b_mode_and_w) != 0 {
			instruction.set_code(this.code64);
			gpr = Register::RAX as u32;
		} else {
			instruction.set_code(this.code32);
			gpr = Register::EAX as u32;
		}
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + gpr);
		write_op2_reg!(instruction, decoder.state.vvvv + gpr);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op1_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_Ev_Gv_Gv {
	has_modrm: bool,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_VEX_Ev_Gv_Gv {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_Ev_Gv_Gv::decode, Self { has_modrm: true, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		let gpr;
		if (decoder.state.flags & decoder.is64b_mode_and_w) != 0 {
			instruction.set_code(this.code64);
			gpr = Register::RAX as u32;
		} else {
			instruction.set_code(this.code32);
			gpr = Register::EAX as u32;
		}
		write_op1_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + gpr);
		write_op2_reg!(instruction, decoder.state.vvvv + gpr);
		if decoder.state.mod_ == 3 {
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op0_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_Hv_Ev {
	has_modrm: bool,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_VEX_Hv_Ev {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_Hv_Ev::decode, Self { has_modrm: true, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		let gpr;
		if (decoder.state.flags & decoder.is64b_mode_and_w) != 0 {
			instruction.set_code(this.code64);
			gpr = Register::RAX as u32;
		} else {
			instruction.set_code(this.code32);
			gpr = Register::EAX as u32;
		}
		write_op0_reg!(instruction, decoder.state.vvvv + gpr);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op1_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_Hv_Ed_Id {
	has_modrm: bool,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_VEX_Hv_Ed_Id {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_Hv_Ed_Id::decode, Self { has_modrm: true, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		instruction.set_op2_kind(OpKind::Immediate32);
		if (decoder.state.flags & decoder.is64b_mode_and_w) != 0 {
			instruction.set_code(this.code64);
			write_op0_reg!(instruction, decoder.state.vvvv + Register::RAX as u32);
		} else {
			instruction.set_code(this.code32);
			write_op0_reg!(instruction, decoder.state.vvvv + Register::EAX as u32);
		}
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::EAX as u32);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op1_kind(OpKind::Memory);
			});
		}
		instruction.set_immediate32(decoder.read_u32() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_GvM_VX_Ib {
	has_modrm: bool,
	base_reg: Register,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_VEX_GvM_VX_Ib {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_GvM_VX_Ib::decode, Self { has_modrm: true, base_reg, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		write_op1_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32);
		instruction.set_op2_kind(OpKind::Immediate8);
		let gpr;
		if (decoder.state.flags & decoder.is64b_mode_and_w) != 0 {
			instruction.set_code(this.code64);
			gpr = Register::RAX as u32;
		} else {
			instruction.set_code(this.code32);
			gpr = Register::EAX as u32;
		}
		if decoder.state.mod_ == 3 {
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op0_kind(OpKind::Memory);
			});
		}
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_Gv_Ev_Ib {
	has_modrm: bool,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_VEX_Gv_Ev_Ib {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_Gv_Ev_Ib::decode, Self { has_modrm: true, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_op2_kind(OpKind::Immediate8);
		let gpr;
		if (decoder.state.flags & decoder.is64b_mode_and_w) != 0 {
			instruction.set_code(this.code64);
			gpr = Register::RAX as u32;
		} else {
			instruction.set_code(this.code32);
			gpr = Register::EAX as u32;
		}
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + gpr);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op1_kind(OpKind::Memory);
			});
		}
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_Gv_Ev_Id {
	has_modrm: bool,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_VEX_Gv_Ev_Id {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_Gv_Ev_Id::decode, Self { has_modrm: true, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_op2_kind(OpKind::Immediate32);
		let gpr;
		if (decoder.state.flags & decoder.is64b_mode_and_w) != 0 {
			instruction.set_code(this.code64);
			gpr = Register::RAX as u32;
		} else {
			instruction.set_code(this.code32);
			gpr = Register::EAX as u32;
		}
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + gpr);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op1_kind(OpKind::Memory);
			});
		}
		instruction.set_immediate32(decoder.read_u32() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_VT_SIBMEM {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_VEX_VT_SIBMEM {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_VT_SIBMEM::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		if ((decoder.state.vvvv_invalid_check | decoder.state.extra_register_base) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);
		write_op0_reg!(instruction, decoder.state.reg + Register::TMM0 as u32);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem_sib(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_SIBMEM_VT {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_VEX_SIBMEM_VT {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_SIBMEM_VT::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		if ((decoder.state.vvvv_invalid_check | decoder.state.extra_register_base) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);
		write_op1_reg!(instruction, decoder.state.reg + Register::TMM0 as u32);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem_sib(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_VT {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_VEX_VT {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_VT::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		if ((decoder.state.vvvv_invalid_check | decoder.state.extra_register_base) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);
		write_op0_reg!(instruction, decoder.state.reg + Register::TMM0 as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_VT_RT_HT {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_VEX_VT_RT_HT {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_VT_RT_HT::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		if decoder.invalid_check_mask != 0 && (decoder.state.vvvv > 7 || decoder.state.extra_register_base != 0) {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);
		write_op0_reg!(instruction, decoder.state.reg + Register::TMM0 as u32);
		write_op2_reg!(instruction, (decoder.state.vvvv & 7) + Register::TMM0 as u32);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + Register::TMM0 as u32);
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

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_Gq_HK_RK {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_VEX_Gq_HK_RK {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_Gq_HK_RK::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		if decoder.invalid_check_mask != 0 && decoder.state.vvvv > 7 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32);
		write_op1_reg!(instruction, (decoder.state.vvvv & 7) + Register::K0 as u32);
		if decoder.state.mod_ == 3 {
			write_op2_reg!(instruction, decoder.state.rm + Register::K0 as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_VK_R_Ib {
	has_modrm: bool,
	gpr: Register,
	code: Code,
}

impl OpCodeHandler_VEX_VK_R_Ib {
	#[inline]
	pub(in crate::decoder) fn new(code: Code, gpr: Register) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_VK_R_Ib::decode, Self { has_modrm: true, code, gpr })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		if ((decoder.state.vvvv_invalid_check | decoder.state.extra_register_base) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);
		write_op0_reg!(instruction, decoder.state.reg + Register::K0 as u32);
		instruction.set_op2_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + this.gpr as u32);
		} else {
			decoder.set_invalid_instruction();
		}
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_K_Jb {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_VEX_K_Jb {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_K_Jb::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		decoder.state.flags |= StateFlags::BRANCH_IMM8;
		if decoder.invalid_check_mask != 0 && decoder.state.vvvv > 7 {
			decoder.set_invalid_instruction();
		}
		write_op0_reg!(instruction, (decoder.state.vvvv & 7) + Register::K0 as u32);
		debug_assert!(decoder.is64b_mode);
		// The modrm byte has the imm8 value
		instruction.set_near_branch64((decoder.state.modrm as i8 as u64).wrapping_add(decoder.current_ip64()));
		instruction.set_code(this.code);
		instruction.set_op1_kind(OpKind::NearBranch64);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_K_Jz {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_VEX_K_Jz {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_K_Jz::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		if decoder.invalid_check_mask != 0 && decoder.state.vvvv > 7 {
			decoder.set_invalid_instruction();
		}
		write_op0_reg!(instruction, (decoder.state.vvvv & 7) + Register::K0 as u32);
		debug_assert!(decoder.is64b_mode);
		instruction.set_code(this.code);
		instruction.set_op1_kind(OpKind::NearBranch64);
		// The modrm byte has the low 8 bits of imm32
		let imm = decoder.state.modrm | ((decoder.read_u8() as u32) << 8) | ((decoder.read_u16() as u32) << 16);
		instruction.set_near_branch64((imm as i32 as u64).wrapping_add(decoder.current_ip64()));
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_Gv_Ev {
	has_modrm: bool,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_VEX_Gv_Ev {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_Gv_Ev::decode, Self { has_modrm: true, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		let gpr;
		if (decoder.state.flags & decoder.is64b_mode_and_w) != 0 {
			instruction.set_code(this.code64);
			gpr = Register::RAX as u32;
		} else {
			instruction.set_code(this.code32);
			gpr = Register::EAX as u32;
		}
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + gpr);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op1_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX_Ev {
	has_modrm: bool,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_VEX_Ev {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VEX_Ev::decode, Self { has_modrm: true, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.encoding() == EncodingKind::VEX as u32 || decoder.state.encoding() == EncodingKind::XOP as u32);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		let gpr;
		if (decoder.state.flags & decoder.is64b_mode_and_w) != 0 {
			instruction.set_code(this.code64);
			gpr = Register::RAX as u32;
		} else {
			instruction.set_code(this.code32);
			gpr = Register::EAX as u32;
		}
		if decoder.state.mod_ == 3 {
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op0_kind(OpKind::Memory);
			});
		}
	}
}
