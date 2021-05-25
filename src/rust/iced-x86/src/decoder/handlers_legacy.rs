// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#![allow(clippy::useless_let_if_seq)]

use crate::decoder::enums::*;
use crate::decoder::handlers::*;
use crate::decoder::*;
use crate::iced_constants::IcedConstants;
use crate::instruction_internal;
use core::u32;
use static_assertions::const_assert_ne;

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

macro_rules! write_op2_reg {
	($instruction:ident, $expr:expr) => {
		debug_assert!($expr < IcedConstants::REGISTER_ENUM_COUNT as u32);
		$instruction.set_op2_register(unsafe { mem::transmute($expr as RegisterUnderlyingType) });
	};
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX2 {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	handler_mem: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
}

impl OpCodeHandler_VEX2 {
	pub(super) fn new(handler_mem: (OpCodeHandlerDecodeFn, *const OpCodeHandler)) -> Self {
		debug_assert!(!is_null_instance_handler(handler_mem.1));
		Self { has_modrm: true, decode: OpCodeHandler_VEX2::decode, handler_mem: (handler_mem.0, unsafe { &*handler_mem.1 }) }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		if decoder.state.mod_ == 3 || decoder.is64b_mode {
			decoder.vex2(instruction);
		} else {
			let (decode, handler) = this.handler_mem;
			(decode)(handler, decoder, instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VEX3 {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	handler_mem: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
}

impl OpCodeHandler_VEX3 {
	pub(super) fn new(handler_mem: (OpCodeHandlerDecodeFn, *const OpCodeHandler)) -> Self {
		debug_assert!(!is_null_instance_handler(handler_mem.1));
		Self { has_modrm: true, decode: OpCodeHandler_VEX3::decode, handler_mem: (handler_mem.0, unsafe { &*handler_mem.1 }) }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		if decoder.state.mod_ == 3 || decoder.is64b_mode {
			decoder.vex3(instruction);
		} else {
			let (decode, handler) = this.handler_mem;
			(decode)(handler, decoder, instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_XOP {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	handler_reg0: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
}

impl OpCodeHandler_XOP {
	pub(super) fn new(handler_reg0: (OpCodeHandlerDecodeFn, *const OpCodeHandler)) -> Self {
		debug_assert!(!is_null_instance_handler(handler_reg0.1));
		Self { has_modrm: true, decode: OpCodeHandler_XOP::decode, handler_reg0: (handler_reg0.0, unsafe { &*handler_reg0.1 }) }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		if (decoder.state.modrm & 0x1F) < 8 {
			let (decode, handler) = this.handler_reg0;
			(decode)(handler, decoder, instruction);
		} else {
			decoder.xop(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_EVEX {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	handler_mem: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
}

impl OpCodeHandler_EVEX {
	pub(super) fn new(handler_mem: (OpCodeHandlerDecodeFn, *const OpCodeHandler)) -> Self {
		debug_assert!(!is_null_instance_handler(handler_mem.1));
		Self { has_modrm: true, decode: OpCodeHandler_EVEX::decode, handler_mem: (handler_mem.0, unsafe { &*handler_mem.1 }) }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		if decoder.state.mod_ == 3 || decoder.is64b_mode {
			decoder.evex_mvex(instruction);
		} else {
			let (decode, handler) = this.handler_mem;
			(decode)(handler, decoder, instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_PrefixEsCsSsDs {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	seg: Register,
}

impl OpCodeHandler_PrefixEsCsSsDs {
	pub(super) fn new(seg: Register) -> Self {
		debug_assert!(seg == Register::ES || seg == Register::CS || seg == Register::SS || seg == Register::DS);
		Self { has_modrm: false, decode: OpCodeHandler_PrefixEsCsSsDs::decode, seg }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);

		if !decoder.is64b_mode || decoder.state.segment_prio == 0 {
			instruction.set_segment_prefix(this.seg);
		}

		decoder.reset_rex_prefix_state();
		decoder.call_opcode_handler_xx_table(instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_PrefixFsGs {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	seg: Register,
}

impl OpCodeHandler_PrefixFsGs {
	pub(super) fn new(seg: Register) -> Self {
		debug_assert!(seg == Register::FS || seg == Register::GS);
		Self { has_modrm: false, decode: OpCodeHandler_PrefixFsGs::decode, seg }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);

		instruction.set_segment_prefix(this.seg);
		decoder.state.segment_prio = 1;

		decoder.reset_rex_prefix_state();
		decoder.call_opcode_handler_xx_table(instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Prefix66 {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
}

impl OpCodeHandler_Prefix66 {
	pub(super) fn new() -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_Prefix66::decode }
	}

	fn decode(_self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);

		decoder.state.flags |= StateFlags::HAS66;
		decoder.state.operand_size = decoder.default_inverted_operand_size;
		if decoder.state.mandatory_prefix == DecoderMandatoryPrefix::PNP {
			decoder.state.mandatory_prefix = DecoderMandatoryPrefix::P66;
		}

		decoder.reset_rex_prefix_state();
		decoder.call_opcode_handler_xx_table(instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Prefix67 {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
}

impl OpCodeHandler_Prefix67 {
	pub(super) fn new() -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_Prefix67::decode }
	}

	fn decode(_self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);

		decoder.state.address_size = decoder.default_inverted_address_size;

		decoder.reset_rex_prefix_state();
		decoder.call_opcode_handler_xx_table(instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_PrefixF0 {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
}

impl OpCodeHandler_PrefixF0 {
	pub(super) fn new() -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_PrefixF0::decode }
	}

	fn decode(_self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);

		instruction_internal::internal_set_has_lock_prefix(instruction);
		decoder.state.flags |= StateFlags::LOCK;

		decoder.reset_rex_prefix_state();
		decoder.call_opcode_handler_xx_table(instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_PrefixF2 {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
}

impl OpCodeHandler_PrefixF2 {
	pub(super) fn new() -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_PrefixF2::decode }
	}

	fn decode(_self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);

		instruction_internal::internal_set_has_repne_prefix(instruction);
		decoder.state.mandatory_prefix = DecoderMandatoryPrefix::PF2;

		decoder.reset_rex_prefix_state();
		decoder.call_opcode_handler_xx_table(instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_PrefixF3 {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
}

impl OpCodeHandler_PrefixF3 {
	pub(super) fn new() -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_PrefixF3::decode }
	}

	fn decode(_self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);

		instruction_internal::internal_set_has_repe_prefix(instruction);
		decoder.state.mandatory_prefix = DecoderMandatoryPrefix::PF3;

		decoder.reset_rex_prefix_state();
		decoder.call_opcode_handler_xx_table(instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_PrefixREX {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	handler: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	rex: u32,
}

impl OpCodeHandler_PrefixREX {
	pub(super) fn new(handler: (OpCodeHandlerDecodeFn, *const OpCodeHandler), rex: u32) -> Self {
		debug_assert!(rex <= 0x0F);
		let handler = (handler.0, unsafe { &*handler.1 });
		Self { has_modrm: false, decode: OpCodeHandler_PrefixREX::decode, handler, rex }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);

		if decoder.is64b_mode {
			decoder.state.flags |= StateFlags::HAS_REX;
			let b = this.rex;
			if (b & 8) != 0 {
				decoder.state.flags |= StateFlags::W;
				decoder.state.operand_size = OpSize::Size64;
			} else {
				decoder.state.flags &= !StateFlags::W;
				if (decoder.state.flags & StateFlags::HAS66) == 0 {
					decoder.state.operand_size = OpSize::Size32;
				} else {
					decoder.state.operand_size = OpSize::Size16;
				}
			}
			decoder.state.extra_register_base = (b & 4) << 1;
			decoder.state.extra_index_register_base = (b & 2) << 2;
			decoder.state.extra_base_register_base = (b & 1) << 3;

			decoder.call_opcode_handler_xx_table(instruction);
		} else {
			let (decode, handler) = this.handler;
			(decode)(handler, decoder, instruction)
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Reg {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	reg: Register,
}

impl OpCodeHandler_Reg {
	pub(super) fn new(code: Code, reg: Register) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_Reg::decode, code, reg }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(this.reg);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_RegIb {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	reg: Register,
}

impl OpCodeHandler_RegIb {
	pub(super) fn new(code: Code, reg: Register) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_RegIb::decode, code, reg }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(this.reg);
		instruction.set_op1_kind(OpKind::Immediate8);
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_IbReg {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	reg: Register,
}

impl OpCodeHandler_IbReg {
	pub(super) fn new(code: Code, reg: Register) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_IbReg::decode, code, reg }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		instruction.set_op0_kind(OpKind::Immediate8);
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction.set_op1_register(this.reg);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_AL_DX {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
}

impl OpCodeHandler_AL_DX {
	pub(super) fn new(code: Code) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_AL_DX::decode, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(Register::AL);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction.set_op1_register(Register::DX);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_DX_AL {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
}

impl OpCodeHandler_DX_AL {
	pub(super) fn new(code: Code) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_DX_AL::decode, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(Register::DX);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction.set_op1_register(Register::AL);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Ib {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
}

impl OpCodeHandler_Ib {
	pub(super) fn new(code: Code) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_Ib::decode, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		instruction.set_op0_kind(OpKind::Immediate8);
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Ib3 {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
}

impl OpCodeHandler_Ib3 {
	pub(super) fn new(code: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Ib3::decode, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		instruction.set_op0_kind(OpKind::Immediate8);
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_MandatoryPrefix {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	handlers: [(OpCodeHandlerDecodeFn, &'static OpCodeHandler); 4],
}

impl OpCodeHandler_MandatoryPrefix {
	pub(super) fn new(
		has_modrm: bool, handler: (OpCodeHandlerDecodeFn, *const OpCodeHandler), handler_66: (OpCodeHandlerDecodeFn, *const OpCodeHandler),
		handler_f3: (OpCodeHandlerDecodeFn, *const OpCodeHandler), handler_f2: (OpCodeHandlerDecodeFn, *const OpCodeHandler),
	) -> Self {
		const_assert_eq!(DecoderMandatoryPrefix::PNP as u32, 0);
		const_assert_eq!(DecoderMandatoryPrefix::P66 as u32, 1);
		const_assert_eq!(DecoderMandatoryPrefix::PF3 as u32, 2);
		const_assert_eq!(DecoderMandatoryPrefix::PF2 as u32, 3);
		debug_assert!(!is_null_instance_handler(handler.1));
		debug_assert!(!is_null_instance_handler(handler_66.1));
		debug_assert!(!is_null_instance_handler(handler_f3.1));
		debug_assert!(!is_null_instance_handler(handler_f2.1));
		let handlers =
			unsafe { [(handler.0, &*handler.1), (handler_66.0, &*handler_66.1), (handler_f3.0, &*handler_f3.1), (handler_f2.0, &*handler_f2.1)] };
		debug_assert_eq!(handlers[0].1.has_modrm, has_modrm);
		debug_assert_eq!(handlers[1].1.has_modrm, has_modrm);
		debug_assert_eq!(handlers[2].1.has_modrm, has_modrm);
		debug_assert_eq!(handlers[3].1.has_modrm, has_modrm);
		Self { decode: OpCodeHandler_MandatoryPrefix::decode, has_modrm, handlers }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		decoder.clear_mandatory_prefix(instruction);
		let (decode, handler) = this.handlers[decoder.state.mandatory_prefix as usize];
		(decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_MandatoryPrefix3 {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	handlers_reg: [(OpCodeHandlerDecodeFn, &'static OpCodeHandler, bool); 4],
	handlers_mem: [(OpCodeHandlerDecodeFn, &'static OpCodeHandler, bool); 4],
}

impl OpCodeHandler_MandatoryPrefix3 {
	#[allow(clippy::too_many_arguments)]
	pub(super) fn new(
		handler_reg: (OpCodeHandlerDecodeFn, *const OpCodeHandler), handler_mem: (OpCodeHandlerDecodeFn, *const OpCodeHandler),
		handler66_reg: (OpCodeHandlerDecodeFn, *const OpCodeHandler), handler66_mem: (OpCodeHandlerDecodeFn, *const OpCodeHandler),
		handlerf3_reg: (OpCodeHandlerDecodeFn, *const OpCodeHandler), handlerf3_mem: (OpCodeHandlerDecodeFn, *const OpCodeHandler),
		handlerf2_reg: (OpCodeHandlerDecodeFn, *const OpCodeHandler), handlerf2_mem: (OpCodeHandlerDecodeFn, *const OpCodeHandler), flags: u32,
	) -> Self {
		debug_assert!(!is_null_instance_handler(handler_reg.1));
		debug_assert!(!is_null_instance_handler(handler_mem.1));
		debug_assert!(!is_null_instance_handler(handler66_reg.1));
		debug_assert!(!is_null_instance_handler(handler66_mem.1));
		debug_assert!(!is_null_instance_handler(handlerf3_reg.1));
		debug_assert!(!is_null_instance_handler(handlerf3_mem.1));
		debug_assert!(!is_null_instance_handler(handlerf2_reg.1));
		debug_assert!(!is_null_instance_handler(handlerf2_mem.1));
		let handlers_reg = unsafe {
			[
				(handler_reg.0, &*handler_reg.1, (flags & LegacyHandlerFlags::HANDLER_REG) == 0),
				(handler66_reg.0, &*handler66_reg.1, (flags & LegacyHandlerFlags::HANDLER_66_REG) == 0),
				(handlerf3_reg.0, &*handlerf3_reg.1, (flags & LegacyHandlerFlags::HANDLER_F3_REG) == 0),
				(handlerf2_reg.0, &*handlerf2_reg.1, (flags & LegacyHandlerFlags::HANDLER_F2_REG) == 0),
			]
		};
		let handlers_mem = unsafe {
			[
				(handler_mem.0, &*handler_mem.1, (flags & LegacyHandlerFlags::HANDLER_MEM) == 0),
				(handler66_mem.0, &*handler66_mem.1, (flags & LegacyHandlerFlags::HANDLER_66_MEM) == 0),
				(handlerf3_mem.0, &*handlerf3_mem.1, (flags & LegacyHandlerFlags::HANDLER_F3_MEM) == 0),
				(handlerf2_mem.0, &*handlerf2_mem.1, (flags & LegacyHandlerFlags::HANDLER_F2_MEM) == 0),
			]
		};
		debug_assert!(handlers_reg[0].1.has_modrm);
		debug_assert!(handlers_reg[1].1.has_modrm);
		debug_assert!(handlers_reg[2].1.has_modrm);
		debug_assert!(handlers_reg[3].1.has_modrm);
		debug_assert!(handlers_mem[0].1.has_modrm);
		debug_assert!(handlers_mem[1].1.has_modrm);
		debug_assert!(handlers_mem[2].1.has_modrm);
		debug_assert!(handlers_mem[3].1.has_modrm);
		Self { has_modrm: true, decode: OpCodeHandler_MandatoryPrefix3::decode, handlers_reg, handlers_mem }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		let (decode, handler, mandatory_prefix) = {
			if decoder.state.mod_ == 3 {
				this.handlers_reg[decoder.state.mandatory_prefix as usize]
			} else {
				this.handlers_mem[decoder.state.mandatory_prefix as usize]
			}
		};
		if mandatory_prefix {
			decoder.clear_mandatory_prefix(instruction);
		}
		(decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_MandatoryPrefix4 {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	handler_np: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	handler_66: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	handler_f3: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	handler_f2: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	flags: u32,
}

impl OpCodeHandler_MandatoryPrefix4 {
	pub(super) fn new(
		handler_np: (OpCodeHandlerDecodeFn, *const OpCodeHandler), handler_66: (OpCodeHandlerDecodeFn, *const OpCodeHandler),
		handler_f3: (OpCodeHandlerDecodeFn, *const OpCodeHandler), handler_f2: (OpCodeHandlerDecodeFn, *const OpCodeHandler), flags: u32,
	) -> Self {
		debug_assert!(!is_null_instance_handler(handler_np.1));
		debug_assert!(!is_null_instance_handler(handler_66.1));
		debug_assert!(!is_null_instance_handler(handler_f3.1));
		debug_assert!(!is_null_instance_handler(handler_f2.1));
		Self {
			decode: OpCodeHandler_MandatoryPrefix4::decode,
			has_modrm: false,
			handler_np: (handler_np.0, unsafe { &*handler_np.1 }),
			handler_66: (handler_66.0, unsafe { &*handler_66.1 }),
			handler_f3: (handler_f3.0, unsafe { &*handler_f3.1 }),
			handler_f2: (handler_f2.0, unsafe { &*handler_f2.1 }),
			flags,
		}
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		const_assert_eq!(DecoderMandatoryPrefix::PNP as u32, 0);
		const_assert_eq!(DecoderMandatoryPrefix::P66 as u32, 1);
		const_assert_eq!(DecoderMandatoryPrefix::PF3 as u32, 2);
		const_assert_eq!(DecoderMandatoryPrefix::PF2 as u32, 3);
		let (decode, handler) = match decoder.state.mandatory_prefix {
			DecoderMandatoryPrefix::PNP => this.handler_np,
			DecoderMandatoryPrefix::P66 => this.handler_66,
			DecoderMandatoryPrefix::PF3 => {
				if (this.flags & 4) != 0 {
					decoder.clear_mandatory_prefix_f3(instruction);
				}
				this.handler_f3
			}
			// The compiler generates worse code (indirect branch) unless I use `_` here
			_ => {
				debug_assert_eq!(decoder.state.mandatory_prefix, DecoderMandatoryPrefix::PF2);
				if (this.flags & 8) != 0 {
					decoder.clear_mandatory_prefix_f2(instruction);
				}
				this.handler_f2
			}
		};
		if handler.has_modrm && (this.flags & 0x10) != 0 {
			decoder.read_modrm();
		}
		(decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_NIb {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
}

impl OpCodeHandler_NIb {
	pub(super) fn new(code: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_NIb::decode, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		instruction.set_op1_kind(OpKind::Immediate8);
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, decoder.state.rm + Register::MM0 as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Reservednop {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	reserved_nop_handler: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	other_handler: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
}

impl OpCodeHandler_Reservednop {
	pub(super) fn new(
		reserved_nop_handler: (OpCodeHandlerDecodeFn, *const OpCodeHandler), other_handler: (OpCodeHandlerDecodeFn, *const OpCodeHandler),
	) -> Self {
		debug_assert!(!is_null_instance_handler(reserved_nop_handler.1));
		debug_assert!(!is_null_instance_handler(other_handler.1));
		Self {
			decode: OpCodeHandler_Reservednop::decode,
			has_modrm: true,
			reserved_nop_handler: (reserved_nop_handler.0, unsafe { &*reserved_nop_handler.1 }),
			other_handler: (other_handler.0, unsafe { &*other_handler.1 }),
		}
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		let (decode, handler) =
			if (decoder.options & DecoderOptions::FORCE_RESERVED_NOP) != 0 { this.reserved_nop_handler } else { this.other_handler };
		(decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Ev_Iz {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	flags: u32,
	code16: Code,
	code32: Code,
	code64: Code,
	state_flags_or_value: u32,
}

impl OpCodeHandler_Ev_Iz {
	pub(super) fn new(code16: Code, code32: Code, code64: Code, flags: u32) -> Self {
		const_assert_eq!(HandlerFlags::LOCK, 1 << 3);
		const_assert_eq!(StateFlags::ALLOW_LOCK, 1 << 13);
		let state_flags_or_value = (flags & HandlerFlags::LOCK) << (13 - 3);

		Self { has_modrm: true, decode: OpCodeHandler_Ev_Iz::decode, flags, code16, code32, code64, state_flags_or_value }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			const_assert_eq!(Register::AX as u32 + 16, Register::EAX as u32);
			const_assert_eq!(Register::AX as u32 + 32, Register::RAX as u32);
			write_op0_reg!(
				instruction,
				(decoder.state.operand_size as u32) * 16 + decoder.state.rm + decoder.state.extra_base_register_base + Register::AX as u32
			);
		} else {
			if (this.flags & (HandlerFlags::XACQUIRE | HandlerFlags::XRELEASE)) != 0 {
				decoder.set_xacquire_xrelease(instruction, this.flags);
			}
			decoder.state.flags |= this.state_flags_or_value;
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		if decoder.state.operand_size == OpSize::Size32 {
			instruction.set_code(this.code32);
			instruction.set_op1_kind(OpKind::Immediate32);
			instruction.set_immediate32(decoder.read_u32() as u32);
		} else if decoder.state.operand_size == OpSize::Size64 {
			instruction.set_code(this.code64);
			instruction.set_op1_kind(OpKind::Immediate32to64);
			instruction.set_immediate32(decoder.read_u32() as u32);
		} else {
			instruction.set_code(this.code16);
			instruction.set_op1_kind(OpKind::Immediate16);
			instruction_internal::internal_set_immediate16(instruction, decoder.read_u16() as u32);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Ev_Ib {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	flags: u32,
	code16: Code,
	code32: Code,
	code64: Code,
	state_flags_or_value: u32,
}

impl OpCodeHandler_Ev_Ib {
	pub(super) fn new(code16: Code, code32: Code, code64: Code, flags: u32) -> Self {
		const_assert_eq!(HandlerFlags::LOCK, 1 << 3);
		const_assert_eq!(StateFlags::ALLOW_LOCK, 1 << 13);
		let state_flags_or_value = (flags & HandlerFlags::LOCK) << (13 - 3);

		Self { has_modrm: true, decode: OpCodeHandler_Ev_Ib::decode, flags, code16, code32, code64, state_flags_or_value }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			const_assert_eq!(Register::AX as u32 + 16, Register::EAX as u32);
			const_assert_eq!(Register::AX as u32 + 32, Register::RAX as u32);
			write_op0_reg!(
				instruction,
				(decoder.state.operand_size as u32) * 16 + decoder.state.rm + decoder.state.extra_base_register_base + Register::AX as u32
			);
		} else {
			if (this.flags & (HandlerFlags::XACQUIRE | HandlerFlags::XRELEASE)) != 0 {
				decoder.set_xacquire_xrelease(instruction, this.flags);
			}
			decoder.state.flags |= this.state_flags_or_value;
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
		if decoder.state.operand_size == OpSize::Size32 {
			instruction.set_code(this.code32);
			instruction.set_op1_kind(OpKind::Immediate8to32);
		} else if decoder.state.operand_size == OpSize::Size64 {
			instruction.set_code(this.code64);
			instruction.set_op1_kind(OpKind::Immediate8to64);
		} else {
			instruction.set_code(this.code16);
			instruction.set_op1_kind(OpKind::Immediate8to16);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Ev_Ib2 {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	flags: u32,
	code: [Code; 3],
	state_flags_or_value: u32,
}

impl OpCodeHandler_Ev_Ib2 {
	pub(super) fn new(code16: Code, code32: Code, code64: Code, flags: u32) -> Self {
		const_assert_eq!(HandlerFlags::LOCK, 1 << 3);
		const_assert_eq!(StateFlags::ALLOW_LOCK, 1 << 13);
		let state_flags_or_value = (flags & HandlerFlags::LOCK) << (13 - 3);

		Self { has_modrm: true, decode: OpCodeHandler_Ev_Ib2::decode, flags, code: [code16, code32, code64], state_flags_or_value }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_op1_kind(OpKind::Immediate8);
		instruction.set_code(this.code[decoder.state.operand_size as usize]);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			const_assert_eq!(Register::AX as u32 + 16, Register::EAX as u32);
			const_assert_eq!(Register::AX as u32 + 32, Register::RAX as u32);
			write_op0_reg!(
				instruction,
				(decoder.state.operand_size as u32) * 16 + decoder.state.rm + decoder.state.extra_base_register_base + Register::AX as u32
			);
		} else {
			if (this.flags & (HandlerFlags::XACQUIRE | HandlerFlags::XRELEASE)) != 0 {
				decoder.set_xacquire_xrelease(instruction, this.flags);
			}
			decoder.state.flags |= this.state_flags_or_value;
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Ev_1 {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: [Code; 3],
}

impl OpCodeHandler_Ev_1 {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Ev_1::decode, code: [code16, code32, code64] }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code[decoder.state.operand_size as usize]);
		instruction.set_op1_kind(OpKind::Immediate8);
		instruction_internal::internal_set_immediate8(instruction, 1);
		decoder.state.flags |= StateFlags::NO_IMM;
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			const_assert_eq!(Register::AX as u32 + 16, Register::EAX as u32);
			const_assert_eq!(Register::AX as u32 + 32, Register::RAX as u32);
			write_op0_reg!(
				instruction,
				(decoder.state.operand_size as u32) * 16 + decoder.state.rm + decoder.state.extra_base_register_base + Register::AX as u32
			);
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Ev_CL {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: [Code; 3],
}

impl OpCodeHandler_Ev_CL {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Ev_CL::decode, code: [code16, code32, code64] }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code[decoder.state.operand_size as usize]);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction.set_op1_register(Register::CL);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			const_assert_eq!(Register::AX as u32 + 16, Register::EAX as u32);
			const_assert_eq!(Register::AX as u32 + 32, Register::RAX as u32);
			write_op0_reg!(
				instruction,
				(decoder.state.operand_size as u32) * 16 + decoder.state.rm + decoder.state.extra_base_register_base + Register::AX as u32
			);
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Ev {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	flags: u32,
	code: [Code; 3],
	state_flags_or_value: u32,
}

impl OpCodeHandler_Ev {
	pub(super) fn new(code16: Code, code32: Code, code64: Code, flags: u32) -> Self {
		const_assert_eq!(HandlerFlags::LOCK, 1 << 3);
		const_assert_eq!(StateFlags::ALLOW_LOCK, 1 << 13);
		let state_flags_or_value = (flags & HandlerFlags::LOCK) << (13 - 3);

		Self { has_modrm: true, decode: OpCodeHandler_Ev::decode, flags, code: [code16, code32, code64], state_flags_or_value }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code[decoder.state.operand_size as usize]);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(
				instruction,
				(decoder.state.operand_size as u32) * 16 + decoder.state.rm + decoder.state.extra_base_register_base + Register::AX as u32
			);
		} else {
			if (this.flags & (HandlerFlags::XACQUIRE | HandlerFlags::XRELEASE)) != 0 {
				decoder.set_xacquire_xrelease(instruction, this.flags);
			}
			decoder.state.flags |= this.state_flags_or_value;
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Rv {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: [Code; 3],
}

impl OpCodeHandler_Rv {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Rv::decode, code: [code16, code32, code64] }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		debug_assert_eq!(decoder.state.mod_, 3);
		instruction.set_code(this.code[decoder.state.operand_size as usize]);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		const_assert_eq!(Register::AX as u32 + 16, Register::EAX as u32);
		const_assert_eq!(Register::AX as u32 + 32, Register::RAX as u32);
		write_op0_reg!(
			instruction,
			(decoder.state.operand_size as u32) * 16 + decoder.state.rm + decoder.state.extra_base_register_base + Register::AX as u32
		);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Rv_32_64 {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Rv_32_64 {
	pub(super) fn new(code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Rv_32_64::decode, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if decoder.is64b_mode {
			instruction.set_code(this.code64);
			debug_assert_eq!(decoder.state.mod_, 3);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::RAX as u32);
		} else {
			instruction.set_code(this.code32);
			debug_assert_eq!(decoder.state.mod_, 3);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::EAX as u32);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Rq {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
}

impl OpCodeHandler_Rq {
	pub(super) fn new(code: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Rq::decode, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		debug_assert_eq!(decoder.state.mod_, 3);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::RAX as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Ev_REXW {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	flags: u32,
	disallow_reg: u32,
	disallow_mem: u32,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Ev_REXW {
	pub(super) fn new(code32: Code, code64: Code, flags: u32) -> Self {
		Self {
			decode: OpCodeHandler_Ev_REXW::decode,
			has_modrm: true,
			code32,
			code64,
			flags,
			disallow_reg: if (flags & 1) != 0 { 0 } else { u32::MAX },
			disallow_mem: if (flags & 2) != 0 { 0 } else { u32::MAX },
		}
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code64);
		} else {
			instruction.set_code(this.code32);
		}
		const_assert_ne!(StateFlags::HAS66, 4);
		if (((this.flags & 4) | (decoder.state.flags & StateFlags::HAS66)) & decoder.invalid_check_mask) == (4 | StateFlags::HAS66) {
			decoder.set_invalid_instruction();
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			if (decoder.state.flags & StateFlags::W) != 0 {
				write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::RAX as u32);
			} else {
				write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::EAX as u32);
			}
			if (this.disallow_reg & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			if (this.disallow_mem & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Evj {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code16: Code,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Evj {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Evj::decode, code16, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if decoder.is64b_mode {
			if (decoder.options & DecoderOptions::AMD) == 0 || decoder.state.operand_size != OpSize::Size16 {
				instruction.set_code(this.code64);
			} else {
				instruction.set_code(this.code16);
			}
			if decoder.state.mod_ == 3 {
				const_assert_eq!(OpKind::Register as u32, 0);
				//instruction.set_op0_kind(OpKind::Register);
				if (decoder.options & DecoderOptions::AMD) == 0 || decoder.state.operand_size != OpSize::Size16 {
					write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::RAX as u32);
				} else {
					write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::AX as u32);
				}
			} else {
				instruction.set_op0_kind(OpKind::Memory);
				decoder.read_op_mem(instruction);
			}
		} else {
			if decoder.state.operand_size == OpSize::Size32 {
				instruction.set_code(this.code32);
			} else {
				instruction.set_code(this.code16);
			}
			if decoder.state.mod_ == 3 {
				const_assert_eq!(OpKind::Register as u32, 0);
				//instruction.set_op0_kind(OpKind::Register);
				const_assert_eq!(Register::AX as u32 + 16, Register::EAX as u32);
				const_assert_eq!(Register::AX as u32 + 32, Register::RAX as u32);
				write_op0_reg!(
					instruction,
					(decoder.state.operand_size as u32) * 16 + decoder.state.rm + decoder.state.extra_base_register_base + Register::AX as u32
				);
			} else {
				instruction.set_op0_kind(OpKind::Memory);
				decoder.read_op_mem(instruction);
			}
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Ep {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code16: Code,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Ep {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Ep::decode, code16, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if decoder.state.operand_size == OpSize::Size64 && (decoder.options & DecoderOptions::AMD) == 0 {
			instruction.set_code(this.code64);
		} else if decoder.state.operand_size == OpSize::Size16 {
			instruction.set_code(this.code16);
		} else {
			instruction.set_code(this.code32);
		}
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Evw {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: [Code; 3],
}

impl OpCodeHandler_Evw {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Evw::decode, code: [code16, code32, code64] }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code[decoder.state.operand_size as usize]);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			const_assert_eq!(Register::AX as u32 + 16, Register::EAX as u32);
			const_assert_eq!(Register::AX as u32 + 32, Register::RAX as u32);
			write_op0_reg!(
				instruction,
				(decoder.state.operand_size as u32) * 16 + decoder.state.rm + decoder.state.extra_base_register_base + Register::AX as u32
			);
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Ew {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: [Code; 3],
}

impl OpCodeHandler_Ew {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Ew::decode, code: [code16, code32, code64] }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code[decoder.state.operand_size as usize]);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			const_assert_eq!(Register::AX as u32 + 16, Register::EAX as u32);
			const_assert_eq!(Register::AX as u32 + 32, Register::RAX as u32);
			write_op0_reg!(
				instruction,
				(decoder.state.operand_size as u32) * 16 + decoder.state.rm + decoder.state.extra_base_register_base + Register::AX as u32
			);
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Ms {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code16: Code,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Ms {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Ms::decode, code16, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if decoder.is64b_mode {
			instruction.set_code(this.code64);
		} else if decoder.state.operand_size == OpSize::Size32 {
			instruction.set_code(this.code32);
		} else {
			instruction.set_code(this.code16);
		}
		debug_assert_ne!(decoder.state.mod_, 3);
		instruction.set_op0_kind(OpKind::Memory);
		decoder.read_op_mem(instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Gv_Ev {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: [Code; 3],
}

impl OpCodeHandler_Gv_Ev {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Gv_Ev::decode, code: [code16, code32, code64] }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		let operand_size = decoder.state.operand_size;
		instruction.set_code(this.code[operand_size as usize]);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(instruction, (operand_size as u32) * 16 + decoder.state.reg + decoder.state.extra_register_base + Register::AX as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, (operand_size as u32) * 16 + decoder.state.rm + decoder.state.extra_base_register_base + Register::AX as u32);
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Gd_Rd {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
}

impl OpCodeHandler_Gd_Rd {
	pub(super) fn new(code: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Gd_Rd::decode, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::EAX as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Gv_M_as {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: [Code; 3],
}

impl OpCodeHandler_Gv_M_as {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Gv_M_as::decode, code: [code16, code32, code64] }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code[decoder.state.address_size as usize]);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		const_assert_eq!(Register::AX as u32 + 16, Register::EAX as u32);
		const_assert_eq!(Register::AX as u32 + 32, Register::RAX as u32);
		write_op0_reg!(
			instruction,
			(decoder.state.address_size as u32) * 16 + decoder.state.reg + decoder.state.extra_register_base + Register::AX as u32
		);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Gdq_Ev {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: [Code; 3],
}

impl OpCodeHandler_Gdq_Ev {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Gdq_Ev::decode, code: [code16, code32, code64] }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code[decoder.state.operand_size as usize]);
		if decoder.state.operand_size != OpSize::Size64 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32);
		} else {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32);
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			const_assert_eq!(Register::AX as u32 + 16, Register::EAX as u32);
			const_assert_eq!(Register::AX as u32 + 32, Register::RAX as u32);
			write_op1_reg!(
				instruction,
				(decoder.state.operand_size as u32) * 16 + decoder.state.rm + decoder.state.extra_base_register_base + Register::AX as u32
			);
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Gv_Ev3 {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: [Code; 3],
}

impl OpCodeHandler_Gv_Ev3 {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Gv_Ev3::decode, code: [code16, code32, code64] }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code[decoder.state.operand_size as usize]);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		const_assert_eq!(Register::AX as u32 + 16, Register::EAX as u32);
		const_assert_eq!(Register::AX as u32 + 32, Register::RAX as u32);
		write_op0_reg!(
			instruction,
			(decoder.state.operand_size as u32) * 16 + decoder.state.reg + decoder.state.extra_register_base + Register::AX as u32
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			const_assert_eq!(Register::AX as u32 + 16, Register::EAX as u32);
			const_assert_eq!(Register::AX as u32 + 32, Register::RAX as u32);
			write_op1_reg!(
				instruction,
				(decoder.state.operand_size as u32) * 16 + decoder.state.rm + decoder.state.extra_base_register_base + Register::AX as u32
			);
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Gv_Ev2 {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: [Code; 3],
}

impl OpCodeHandler_Gv_Ev2 {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Gv_Ev2::decode, code: [code16, code32, code64] }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code[decoder.state.operand_size as usize]);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		const_assert_eq!(Register::AX as u32 + 16, Register::EAX as u32);
		const_assert_eq!(Register::AX as u32 + 32, Register::RAX as u32);
		write_op0_reg!(
			instruction,
			(decoder.state.operand_size as u32) * 16 + decoder.state.reg + decoder.state.extra_register_base + Register::AX as u32
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			let index = decoder.state.rm + decoder.state.extra_base_register_base;
			if decoder.state.operand_size != OpSize::Size16 {
				write_op1_reg!(instruction, index + Register::EAX as u32);
			} else {
				write_op1_reg!(instruction, index + Register::AX as u32);
			}
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_R_C {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code32: Code,
	code64: Code,
	base_reg: Register,
}

impl OpCodeHandler_R_C {
	pub(super) fn new(code32: Code, code64: Code, base_reg: Register) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_R_C::decode, code32, code64, base_reg }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if decoder.is64b_mode {
			instruction.set_code(this.code64);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::RAX as u32);
		} else {
			instruction.set_code(this.code32);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::EAX as u32);
		}
		let mut extra_register_base = decoder.state.extra_register_base;
		// LOCK MOV CR0 is supported by some AMD CPUs
		if this.base_reg == Register::CR0 && extra_register_base == 0 && instruction.has_lock_prefix() && (decoder.options & DecoderOptions::AMD) != 0
		{
			extra_register_base = 8;
			instruction_internal::internal_clear_has_lock_prefix(instruction);
			decoder.state.flags &= !StateFlags::LOCK;
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		let reg = decoder.state.reg + extra_register_base;
		if decoder.invalid_check_mask != 0 {
			if this.base_reg == Register::CR0 {
				if reg == 1 || (reg != 8 && reg >= 5) {
					decoder.set_invalid_instruction();
				}
			} else if this.base_reg == Register::DR0 {
				if reg > 7 {
					decoder.set_invalid_instruction();
				}
			} else {
				debug_assert_eq!(this.base_reg, Register::TR0);
			}
		}
		write_op1_reg!(instruction, reg + this.base_reg as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_C_R {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code32: Code,
	code64: Code,
	base_reg: Register,
}

impl OpCodeHandler_C_R {
	pub(super) fn new(code32: Code, code64: Code, base_reg: Register) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_C_R::decode, code32, code64, base_reg }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if decoder.is64b_mode {
			instruction.set_code(this.code64);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::RAX as u32);
		} else {
			instruction.set_code(this.code32);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::EAX as u32);
		}
		let mut extra_register_base = decoder.state.extra_register_base;
		// LOCK MOV CR0 is supported by some AMD CPUs
		if this.base_reg == Register::CR0 && extra_register_base == 0 && instruction.has_lock_prefix() && (decoder.options & DecoderOptions::AMD) != 0
		{
			extra_register_base = 8;
			instruction_internal::internal_clear_has_lock_prefix(instruction);
			decoder.state.flags &= !StateFlags::LOCK;
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		let reg = decoder.state.reg + extra_register_base;
		if decoder.invalid_check_mask != 0 {
			if this.base_reg == Register::CR0 {
				if reg == 1 || (reg != 8 && reg >= 5) {
					decoder.set_invalid_instruction();
				}
			} else if this.base_reg == Register::DR0 {
				if reg > 7 {
					decoder.set_invalid_instruction();
				}
			} else {
				debug_assert_eq!(this.base_reg, Register::TR0);
			}
		}
		write_op0_reg!(instruction, reg + this.base_reg as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Jb {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code16: Code,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Jb {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_Jb::decode, code16, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		decoder.state.flags |= StateFlags::BRANCH_IMM8;
		let b = decoder.read_u8();
		if decoder.is64b_mode {
			if (decoder.options & DecoderOptions::AMD) == 0 || decoder.state.operand_size != OpSize::Size16 {
				instruction.set_near_branch64((b as i8 as u64).wrapping_add(decoder.current_ip64()));
				instruction.set_code(this.code64);
				instruction.set_op0_kind(OpKind::NearBranch64);
			} else {
				instruction_internal::internal_set_near_branch16(instruction, (b as i8 as u32).wrapping_add(decoder.current_ip32()) as u16 as u32);
				instruction.set_code(this.code16);
				instruction.set_op0_kind(OpKind::NearBranch16);
			}
		} else {
			if decoder.state.operand_size != OpSize::Size16 {
				instruction.set_near_branch32((b as i8 as u32).wrapping_add(decoder.current_ip32()));
				instruction.set_code(this.code32);
				instruction.set_op0_kind(OpKind::NearBranch32);
			} else {
				instruction_internal::internal_set_near_branch16(instruction, (b as i8 as u32).wrapping_add(decoder.current_ip32()) as u16 as u32);
				instruction.set_code(this.code16);
				instruction.set_op0_kind(OpKind::NearBranch16);
			}
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Jx {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code16: Code,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Jx {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_Jx::decode, code16, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		decoder.state.flags |= StateFlags::XBEGIN;
		if decoder.is64b_mode {
			if decoder.state.operand_size == OpSize::Size32 {
				instruction.set_code(this.code32);
				instruction.set_op0_kind(OpKind::NearBranch64);
				instruction.set_near_branch64((decoder.read_u32() as i32 as u64).wrapping_add(decoder.current_ip64()));
			} else if decoder.state.operand_size == OpSize::Size64 {
				instruction.set_code(this.code64);
				instruction.set_op0_kind(OpKind::NearBranch64);
				instruction.set_near_branch64((decoder.read_u32() as i32 as u64).wrapping_add(decoder.current_ip64()));
			} else {
				instruction.set_code(this.code16);
				instruction.set_op0_kind(OpKind::NearBranch64);
				instruction.set_near_branch64((decoder.read_u16() as i16 as u64).wrapping_add(decoder.current_ip64()));
			}
		} else {
			debug_assert!(decoder.default_code_size == CodeSize::Code16 || decoder.default_code_size == CodeSize::Code32);
			if decoder.state.operand_size == OpSize::Size32 {
				instruction.set_code(this.code32);
				instruction.set_op0_kind(OpKind::NearBranch32);
				instruction.set_near_branch32((decoder.read_u32() as u32).wrapping_add(decoder.current_ip32()));
			} else {
				debug_assert!(decoder.state.operand_size == OpSize::Size16);
				instruction.set_code(this.code16);
				instruction.set_op0_kind(OpKind::NearBranch32);
				instruction.set_near_branch32((decoder.read_u16() as i16 as u32).wrapping_add(decoder.current_ip32()));
			}
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Jz {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code16: Code,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Jz {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_Jz::decode, code16, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if decoder.is64b_mode {
			if (decoder.options & DecoderOptions::AMD) == 0 || decoder.state.operand_size != OpSize::Size16 {
				instruction.set_code(this.code64);
				instruction.set_op0_kind(OpKind::NearBranch64);
				instruction.set_near_branch64((decoder.read_u32() as i32 as u64).wrapping_add(decoder.current_ip64()));
			} else {
				instruction.set_code(this.code16);
				instruction.set_op0_kind(OpKind::NearBranch16);
				instruction_internal::internal_set_near_branch16(
					instruction,
					(decoder.read_u16() as u32).wrapping_add(decoder.current_ip32()) as u16 as u32,
				);
			}
		} else {
			if decoder.state.operand_size != OpSize::Size16 {
				instruction.set_code(this.code32);
				instruction.set_op0_kind(OpKind::NearBranch32);
				instruction.set_near_branch32((decoder.read_u32() as u32).wrapping_add(decoder.current_ip32()));
			} else {
				instruction.set_code(this.code16);
				instruction.set_op0_kind(OpKind::NearBranch16);
				instruction_internal::internal_set_near_branch16(
					instruction,
					(decoder.read_u16() as u32).wrapping_add(decoder.current_ip32()) as u16 as u32,
				);
			}
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Jb2 {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code16_16: Code,
	code16_32: Code,
	code16_64: Code,
	code32_16: Code,
	code32_32: Code,
	code64_32: Code,
	code64_64: Code,
}

impl OpCodeHandler_Jb2 {
	pub(super) fn new(code16_16: Code, code16_32: Code, code16_64: Code, code32_16: Code, code32_32: Code, code64_32: Code, code64_64: Code) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_Jb2::decode, code16_16, code16_32, code16_64, code32_16, code32_32, code64_32, code64_64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		decoder.state.flags |= StateFlags::BRANCH_IMM8;
		let b = decoder.read_u8();
		if decoder.is64b_mode {
			if (decoder.options & DecoderOptions::AMD) == 0 || decoder.state.operand_size != OpSize::Size16 {
				instruction.set_near_branch64((b as i8 as u64).wrapping_add(decoder.current_ip64()));
				instruction.set_op0_kind(OpKind::NearBranch64);
				if decoder.state.address_size == OpSize::Size64 {
					instruction.set_code(this.code64_64);
				} else {
					instruction.set_code(this.code64_32);
				}
			} else {
				instruction_internal::internal_set_near_branch16(instruction, (b as i8 as u32).wrapping_add(decoder.current_ip32()) as u16 as u32);
				instruction.set_op0_kind(OpKind::NearBranch16);
				if decoder.state.address_size == OpSize::Size64 {
					instruction.set_code(this.code16_64);
				} else {
					instruction.set_code(this.code16_32);
				}
			}
		} else {
			if decoder.state.operand_size == OpSize::Size32 {
				instruction.set_near_branch32((b as i8 as u32).wrapping_add(decoder.current_ip32()));
				instruction.set_op0_kind(OpKind::NearBranch32);
				if decoder.state.address_size == OpSize::Size32 {
					instruction.set_code(this.code32_32);
				} else {
					instruction.set_code(this.code32_16);
				}
			} else {
				instruction_internal::internal_set_near_branch16(instruction, (b as i8 as u32).wrapping_add(decoder.current_ip32()) as u16 as u32);
				instruction.set_op0_kind(OpKind::NearBranch16);
				if decoder.state.address_size == OpSize::Size32 {
					instruction.set_code(this.code16_32);
				} else {
					instruction.set_code(this.code16_16);
				}
			}
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Jdisp {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code16: Code,
	code32: Code,
}

impl OpCodeHandler_Jdisp {
	pub(super) fn new(code16: Code, code32: Code) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_Jdisp::decode, code16, code32 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		debug_assert!(!decoder.is64b_mode);
		if decoder.state.operand_size != OpSize::Size16 {
			instruction.set_code(this.code32);
			instruction.set_op0_kind(OpKind::NearBranch32);
			instruction.set_near_branch32(decoder.read_u32() as u32);
		} else {
			instruction.set_code(this.code16);
			instruction.set_op0_kind(OpKind::NearBranch16);
			instruction_internal::internal_set_near_branch16(instruction, decoder.read_u16() as u32);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_PushOpSizeReg {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code16: Code,
	code32: Code,
	code64: Code,
	reg: Register,
}

impl OpCodeHandler_PushOpSizeReg {
	pub(super) fn new(code16: Code, code32: Code, code64: Code, reg: Register) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_PushOpSizeReg::decode, code16, code32, code64, reg }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(this.reg);
		if decoder.is64b_mode {
			if decoder.state.operand_size != OpSize::Size16 {
				instruction.set_code(this.code64);
			} else {
				instruction.set_code(this.code16);
			}
		} else {
			if decoder.state.operand_size == OpSize::Size32 {
				instruction.set_code(this.code32);
			} else {
				instruction.set_code(this.code16);
			}
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_PushEv {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code16: Code,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_PushEv {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_PushEv::decode, code16, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if decoder.is64b_mode {
			if decoder.state.operand_size != OpSize::Size16 {
				instruction.set_code(this.code64);
			} else {
				instruction.set_code(this.code16);
			}
		} else {
			if decoder.state.operand_size == OpSize::Size32 {
				instruction.set_code(this.code32);
			} else {
				instruction.set_code(this.code16);
			}
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			let index = decoder.state.rm + decoder.state.extra_base_register_base;
			if decoder.is64b_mode {
				if decoder.state.operand_size != OpSize::Size16 {
					write_op0_reg!(instruction, index + Register::RAX as u32);
				} else {
					write_op0_reg!(instruction, index + Register::AX as u32);
				}
			} else {
				if decoder.state.operand_size == OpSize::Size32 {
					write_op0_reg!(instruction, index + Register::EAX as u32);
				} else {
					write_op0_reg!(instruction, index + Register::AX as u32);
				}
			}
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Ev_Gv {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: [Code; 3],
}

impl OpCodeHandler_Ev_Gv {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Ev_Gv::decode, code: [code16, code32, code64] }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		let operand_size = decoder.state.operand_size;
		instruction.set_code(this.code[operand_size as usize]);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		write_op1_reg!(instruction, (operand_size as u32) * 16 + decoder.state.reg + decoder.state.extra_register_base + Register::AX as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, (operand_size as u32) * 16 + decoder.state.rm + decoder.state.extra_base_register_base + Register::AX as u32);
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Ev_Gv_flags {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	flags: u32,
	code: [Code; 3],
	state_flags_or_value: u32,
}

impl OpCodeHandler_Ev_Gv_flags {
	pub(super) fn new(code16: Code, code32: Code, code64: Code, flags: u32) -> Self {
		debug_assert!((flags & (HandlerFlags::XACQUIRE | HandlerFlags::XRELEASE)) != 0);

		const_assert_eq!(HandlerFlags::LOCK, 1 << 3);
		const_assert_eq!(StateFlags::ALLOW_LOCK, 1 << 13);
		let state_flags_or_value = (flags & HandlerFlags::LOCK) << (13 - 3);

		Self { has_modrm: true, decode: OpCodeHandler_Ev_Gv_flags::decode, flags, code: [code16, code32, code64], state_flags_or_value }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		let operand_size = decoder.state.operand_size;
		instruction.set_code(this.code[operand_size as usize]);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		write_op1_reg!(instruction, (operand_size as u32) * 16 + decoder.state.reg + decoder.state.extra_register_base + Register::AX as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, (operand_size as u32) * 16 + decoder.state.rm + decoder.state.extra_base_register_base + Register::AX as u32);
		} else {
			decoder.state.flags |= this.state_flags_or_value;
			decoder.set_xacquire_xrelease(instruction, this.flags);
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Ev_Gv_32_64 {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Ev_Gv_32_64 {
	pub(super) fn new(code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Ev_Gv_32_64::decode, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		let base_reg;
		if decoder.is64b_mode {
			instruction.set_code(this.code64);
			base_reg = Register::RAX;
		} else {
			instruction.set_code(this.code32);
			base_reg = Register::EAX;
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		write_op1_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + base_reg as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + base_reg as u32);
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Ev_Gv_Ib {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: [Code; 3],
}

impl OpCodeHandler_Ev_Gv_Ib {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Ev_Gv_Ib::decode, code: [code16, code32, code64] }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			const_assert_eq!(Register::AX as u32 + 16, Register::EAX as u32);
			const_assert_eq!(Register::AX as u32 + 32, Register::RAX as u32);
			write_op0_reg!(
				instruction,
				(decoder.state.operand_size as u32) * 16 + decoder.state.rm + decoder.state.extra_base_register_base + Register::AX as u32
			);
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		instruction.set_op2_kind(OpKind::Immediate8);
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
		instruction.set_code(this.code[decoder.state.operand_size as usize]);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		const_assert_eq!(Register::AX as u32 + 16, Register::EAX as u32);
		const_assert_eq!(Register::AX as u32 + 32, Register::RAX as u32);
		write_op1_reg!(
			instruction,
			(decoder.state.operand_size as u32) * 16 + decoder.state.reg + decoder.state.extra_register_base + Register::AX as u32
		);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Ev_Gv_CL {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: [Code; 3],
}

impl OpCodeHandler_Ev_Gv_CL {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Ev_Gv_CL::decode, code: [code16, code32, code64] }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code[decoder.state.operand_size as usize]);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		const_assert_eq!(Register::AX as u32 + 16, Register::EAX as u32);
		const_assert_eq!(Register::AX as u32 + 32, Register::RAX as u32);
		write_op1_reg!(
			instruction,
			(decoder.state.operand_size as u32) * 16 + decoder.state.reg + decoder.state.extra_register_base + Register::AX as u32
		);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op2_kind(OpKind::Register);
		instruction.set_op2_register(Register::CL);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			const_assert_eq!(Register::AX as u32 + 16, Register::EAX as u32);
			const_assert_eq!(Register::AX as u32 + 32, Register::RAX as u32);
			write_op0_reg!(
				instruction,
				(decoder.state.operand_size as u32) * 16 + decoder.state.rm + decoder.state.extra_base_register_base + Register::AX as u32
			);
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Gv_Mp {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code16: Code,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Gv_Mp {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Gv_Mp::decode, code16, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if decoder.state.operand_size == OpSize::Size64 && (decoder.options & DecoderOptions::AMD) == 0 {
			instruction.set_code(this.code64);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32);
		} else if decoder.state.operand_size == OpSize::Size16 {
			instruction.set_code(this.code16);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::AX as u32);
		} else {
			instruction.set_code(this.code32);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32);
		}
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Gv_Eb {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: [Code; 3],
}

impl OpCodeHandler_Gv_Eb {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Gv_Eb::decode, code: [code16, code32, code64] }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code[decoder.state.operand_size as usize]);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		const_assert_eq!(Register::AX as u32 + 16, Register::EAX as u32);
		const_assert_eq!(Register::AX as u32 + 32, Register::RAX as u32);
		write_op0_reg!(
			instruction,
			(decoder.state.operand_size as u32) * 16 + decoder.state.reg + decoder.state.extra_register_base + Register::AX as u32
		);
		if decoder.state.mod_ == 3 {
			let mut index = decoder.state.rm + decoder.state.extra_base_register_base;
			if (decoder.state.flags & StateFlags::HAS_REX) != 0 && index >= 4 {
				index += 4;
			}
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, index + Register::AL as u32);
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Gv_Ew {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: [Code; 3],
}

impl OpCodeHandler_Gv_Ew {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Gv_Ew::decode, code: [code16, code32, code64] }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code[decoder.state.operand_size as usize]);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		const_assert_eq!(Register::AX as u32 + 16, Register::EAX as u32);
		const_assert_eq!(Register::AX as u32 + 32, Register::RAX as u32);
		write_op0_reg!(
			instruction,
			(decoder.state.operand_size as u32) * 16 + decoder.state.reg + decoder.state.extra_register_base + Register::AX as u32
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::AX as u32);
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_PushSimple2 {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code16: Code,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_PushSimple2 {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_PushSimple2::decode, code16, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if decoder.is64b_mode {
			if decoder.state.operand_size != OpSize::Size16 {
				instruction.set_code(this.code64);
			} else {
				instruction.set_code(this.code16);
			}
		} else {
			if decoder.state.operand_size == OpSize::Size32 {
				instruction.set_code(this.code32);
			} else {
				instruction.set_code(this.code16);
			}
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Simple2 {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: [Code; 3],
}

impl OpCodeHandler_Simple2 {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_Simple2::decode, code: [code16, code32, code64] }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code[decoder.state.operand_size as usize]);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Simple2Iw {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: [Code; 3],
}

impl OpCodeHandler_Simple2Iw {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_Simple2Iw::decode, code: [code16, code32, code64] }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_op0_kind(OpKind::Immediate16);
		instruction_internal::internal_set_immediate16(instruction, decoder.read_u16() as u32);
		instruction.set_code(this.code[decoder.state.operand_size as usize]);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Simple3 {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code16: Code,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Simple3 {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_Simple3::decode, code16, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if decoder.is64b_mode {
			if decoder.state.operand_size != OpSize::Size16 {
				instruction.set_code(this.code64);
			} else {
				instruction.set_code(this.code16);
			}
		} else {
			if decoder.state.operand_size == OpSize::Size32 {
				instruction.set_code(this.code32);
			} else {
				instruction.set_code(this.code16);
			}
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Simple5 {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: [Code; 3],
}

impl OpCodeHandler_Simple5 {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_Simple5::decode, code: [code16, code32, code64] }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code[decoder.state.address_size as usize]);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Simple5_ModRM_as {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: [Code; 3],
}

impl OpCodeHandler_Simple5_ModRM_as {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Simple5_ModRM_as::decode, code: [code16, code32, code64] }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code[decoder.state.address_size as usize]);
		const_assert_eq!(Register::AX as u32 + 16, Register::EAX as u32);
		const_assert_eq!(Register::AX as u32 + 32, Register::RAX as u32);
		write_op0_reg!(
			instruction,
			(decoder.state.address_size as u32) * 16 + decoder.state.rm + decoder.state.extra_base_register_base + Register::AX as u32
		);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Simple4 {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Simple4 {
	pub(super) fn new(code32: Code, code64: Code) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_Simple4::decode, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code64);
		} else {
			instruction.set_code(this.code32);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_PushSimpleReg {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	index: u32,
	code16: Code,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_PushSimpleReg {
	pub(super) fn new(index: u32, code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_PushSimpleReg::decode, index, code16, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if decoder.is64b_mode {
			if decoder.state.operand_size != OpSize::Size16 {
				instruction.set_code(this.code64);
				const_assert_eq!(OpKind::Register as u32, 0);
				//instruction.set_op0_kind(OpKind::Register);
				write_op0_reg!(instruction, this.index + decoder.state.extra_base_register_base + Register::RAX as u32);
			} else {
				instruction.set_code(this.code16);
				const_assert_eq!(OpKind::Register as u32, 0);
				//instruction.set_op0_kind(OpKind::Register);
				write_op0_reg!(instruction, this.index + decoder.state.extra_base_register_base + Register::AX as u32);
			}
		} else {
			if decoder.state.operand_size == OpSize::Size32 {
				instruction.set_code(this.code32);
				const_assert_eq!(OpKind::Register as u32, 0);
				//instruction.set_op0_kind(OpKind::Register);
				write_op0_reg!(instruction, this.index + decoder.state.extra_base_register_base + Register::EAX as u32);
			} else {
				instruction.set_code(this.code16);
				const_assert_eq!(OpKind::Register as u32, 0);
				//instruction.set_op0_kind(OpKind::Register);
				write_op0_reg!(instruction, this.index + decoder.state.extra_base_register_base + Register::AX as u32);
			}
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_SimpleReg {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	index: u32,
	code: Code,
}

impl OpCodeHandler_SimpleReg {
	pub(super) fn new(code: Code, index: u32) -> Self {
		const_assert_eq!(OpSize::Size16 as u32, 0);
		const_assert_eq!(OpSize::Size32 as u32, 1);
		const_assert_eq!(OpSize::Size64 as u32, 2);
		debug_assert!(code as u32 + 2 < IcedConstants::CODE_ENUM_COUNT as u32);

		Self { has_modrm: false, decode: OpCodeHandler_SimpleReg::decode, code, index }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		const_assert_eq!(OpSize::Size16 as u32, 0);
		const_assert_eq!(OpSize::Size32 as u32, 1);
		const_assert_eq!(OpSize::Size64 as u32, 2);
		let size_index = decoder.state.operand_size as u32;

		// SAFETY: this.code + {0,1,2} is a valid Code value, see ctor
		instruction.set_code(unsafe { mem::transmute((size_index + this.code as u32) as u16) });
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		const_assert!(Register::AX as u32 + 16 == Register::EAX as u32);
		const_assert!(Register::AX as u32 + 32 == Register::RAX as u32);
		write_op0_reg!(instruction, size_index * 16 + this.index + decoder.state.extra_base_register_base + Register::AX as u32);
	}
}

static XCHG_REG_RAX_CODES: [Code; 3 * 16] = [
	Code::Nopw,
	Code::Xchg_r16_AX,
	Code::Xchg_r16_AX,
	Code::Xchg_r16_AX,
	Code::Xchg_r16_AX,
	Code::Xchg_r16_AX,
	Code::Xchg_r16_AX,
	Code::Xchg_r16_AX,
	Code::Xchg_r16_AX,
	Code::Xchg_r16_AX,
	Code::Xchg_r16_AX,
	Code::Xchg_r16_AX,
	Code::Xchg_r16_AX,
	Code::Xchg_r16_AX,
	Code::Xchg_r16_AX,
	Code::Xchg_r16_AX,
	Code::Nopd,
	Code::Xchg_r32_EAX,
	Code::Xchg_r32_EAX,
	Code::Xchg_r32_EAX,
	Code::Xchg_r32_EAX,
	Code::Xchg_r32_EAX,
	Code::Xchg_r32_EAX,
	Code::Xchg_r32_EAX,
	Code::Xchg_r32_EAX,
	Code::Xchg_r32_EAX,
	Code::Xchg_r32_EAX,
	Code::Xchg_r32_EAX,
	Code::Xchg_r32_EAX,
	Code::Xchg_r32_EAX,
	Code::Xchg_r32_EAX,
	Code::Xchg_r32_EAX,
	Code::Nopq,
	Code::Xchg_r64_RAX,
	Code::Xchg_r64_RAX,
	Code::Xchg_r64_RAX,
	Code::Xchg_r64_RAX,
	Code::Xchg_r64_RAX,
	Code::Xchg_r64_RAX,
	Code::Xchg_r64_RAX,
	Code::Xchg_r64_RAX,
	Code::Xchg_r64_RAX,
	Code::Xchg_r64_RAX,
	Code::Xchg_r64_RAX,
	Code::Xchg_r64_RAX,
	Code::Xchg_r64_RAX,
	Code::Xchg_r64_RAX,
	Code::Xchg_r64_RAX,
];

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Xchg_Reg_rAX {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	index: u32,
}

impl OpCodeHandler_Xchg_Reg_rAX {
	pub(super) fn new(index: u32) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_Xchg_Reg_rAX::decode, index }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);

		if this.index == 0 && decoder.state.mandatory_prefix == DecoderMandatoryPrefix::PF3 && (decoder.options & DecoderOptions::NO_PAUSE) == 0 {
			decoder.clear_mandatory_prefix_f3(instruction);
			instruction.set_code(Code::Pause);
		} else {
			const_assert_eq!(OpSize::Size16 as u32, 0);
			const_assert_eq!(OpSize::Size32 as u32, 1);
			const_assert_eq!(OpSize::Size64 as u32, 2);
			let size_index = decoder.state.operand_size as u32;
			let code_index = this.index + decoder.state.extra_base_register_base;

			instruction.set_code(unsafe { *XCHG_REG_RAX_CODES.get_unchecked((size_index * 16 + code_index) as usize) });
			if code_index != 0 {
				const_assert!(Register::AX as u32 + 16 == Register::EAX as u32);
				const_assert!(Register::AX as u32 + 32 == Register::RAX as u32);
				let reg = size_index * 16 + code_index + Register::AX as u32;
				const_assert_eq!(OpKind::Register as u32, 0);
				//instruction.set_op0_kind(OpKind::Register);
				write_op0_reg!(instruction, reg);
				const_assert_eq!(OpKind::Register as u32, 0);
				//instruction.set_op1_kind(OpKind::Register);
				write_op1_reg!(instruction, size_index * 16 + Register::AX as u32);
			}
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Reg_Iz {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code16: Code,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Reg_Iz {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_Reg_Iz::decode, code16, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if decoder.state.operand_size == OpSize::Size32 {
			instruction.set_code(this.code32);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			instruction.set_op0_register(Register::EAX);
			instruction.set_op1_kind(OpKind::Immediate32);
			instruction.set_immediate32(decoder.read_u32() as u32);
		} else if decoder.state.operand_size == OpSize::Size64 {
			instruction.set_code(this.code64);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			instruction.set_op0_register(Register::RAX);
			instruction.set_op1_kind(OpKind::Immediate32to64);
			instruction.set_immediate32(decoder.read_u32() as u32);
		} else {
			instruction.set_code(this.code16);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			instruction.set_op0_register(Register::AX);
			instruction.set_op1_kind(OpKind::Immediate16);
			instruction_internal::internal_set_immediate16(instruction, decoder.read_u16() as u32);
		}
	}
}

static WITH_REX_PREFIX_MOV_REGISTERS: [Register; 16] = [
	Register::AL,
	Register::CL,
	Register::DL,
	Register::BL,
	Register::SPL,
	Register::BPL,
	Register::SIL,
	Register::DIL,
	Register::R8L,
	Register::R9L,
	Register::R10L,
	Register::R11L,
	Register::R12L,
	Register::R13L,
	Register::R14L,
	Register::R15L,
];

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_RegIb3 {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	index: u32,
}

impl OpCodeHandler_RegIb3 {
	pub(super) fn new(index: u32) -> Self {
		debug_assert!(index <= 7);
		Self { has_modrm: false, decode: OpCodeHandler_RegIb3::decode, index }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(Code::Mov_r8_imm8);
		instruction.set_op1_kind(OpKind::Immediate8);
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
		if (decoder.state.flags & StateFlags::HAS_REX) != 0 {
			// SAFETY: index <= 7 (see ctor) and extra_base_register_base == 0 or 8 so index = 0-15
			let register =
				unsafe { *WITH_REX_PREFIX_MOV_REGISTERS.get_unchecked((this.index + decoder.state.extra_base_register_base) as usize) } as u32;
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, register);
		} else {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, this.index + Register::AL as u32);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_RegIz2 {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	index: u32,
}

impl OpCodeHandler_RegIz2 {
	pub(super) fn new(index: u32) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_RegIz2::decode, index }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if decoder.state.operand_size == OpSize::Size32 {
			instruction.set_code(Code::Mov_r32_imm32);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, this.index + decoder.state.extra_base_register_base + Register::EAX as u32);
			instruction.set_op1_kind(OpKind::Immediate32);
			instruction.set_immediate32(decoder.read_u32() as u32);
		} else if decoder.state.operand_size == OpSize::Size64 {
			instruction.set_code(Code::Mov_r64_imm64);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, this.index + decoder.state.extra_base_register_base + Register::RAX as u32);
			instruction.set_op1_kind(OpKind::Immediate64);
			let q = decoder.read_u64();
			instruction_internal::internal_set_immediate64_lo(instruction, q as u32);
			instruction_internal::internal_set_immediate64_hi(instruction, (q >> 32) as u32);
		} else {
			instruction.set_code(Code::Mov_r16_imm16);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, this.index + decoder.state.extra_base_register_base + Register::AX as u32);
			instruction.set_op1_kind(OpKind::Immediate16);
			instruction_internal::internal_set_immediate16(instruction, decoder.read_u16() as u32);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_PushIb2 {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code16: Code,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_PushIb2 {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_PushIb2::decode, code16, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
		if decoder.is64b_mode {
			if decoder.state.operand_size != OpSize::Size16 {
				instruction.set_code(this.code64);
				instruction.set_op0_kind(OpKind::Immediate8to64);
			} else {
				instruction.set_code(this.code16);
				instruction.set_op0_kind(OpKind::Immediate8to16);
			}
		} else {
			if decoder.state.operand_size == OpSize::Size32 {
				instruction.set_code(this.code32);
				instruction.set_op0_kind(OpKind::Immediate8to32);
			} else {
				instruction.set_code(this.code16);
				instruction.set_op0_kind(OpKind::Immediate8to16);
			}
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_PushIz {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code16: Code,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_PushIz {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_PushIz::decode, code16, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if decoder.is64b_mode {
			if decoder.state.operand_size != OpSize::Size16 {
				instruction.set_code(this.code64);
				instruction.set_op0_kind(OpKind::Immediate32to64);
				instruction.set_immediate32(decoder.read_u32() as u32);
			} else {
				instruction.set_code(this.code16);
				instruction.set_op0_kind(OpKind::Immediate16);
				instruction_internal::internal_set_immediate16(instruction, decoder.read_u16() as u32);
			}
		} else {
			if decoder.state.operand_size == OpSize::Size32 {
				instruction.set_code(this.code32);
				instruction.set_op0_kind(OpKind::Immediate32);
				instruction.set_immediate32(decoder.read_u32() as u32);
			} else {
				instruction.set_code(this.code16);
				instruction.set_op0_kind(OpKind::Immediate16);
				instruction_internal::internal_set_immediate16(instruction, decoder.read_u16() as u32);
			}
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Gv_Ma {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code16: Code,
	code32: Code,
}

impl OpCodeHandler_Gv_Ma {
	pub(super) fn new(code16: Code, code32: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Gv_Ma::decode, code16, code32 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if decoder.state.operand_size != OpSize::Size16 {
			instruction.set_code(this.code32);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32);
		} else {
			instruction.set_code(this.code16);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::AX as u32);
		}
		debug_assert_ne!(decoder.state.mod_, 3);
		instruction.set_op1_kind(OpKind::Memory);
		decoder.read_op_mem(instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_RvMw_Gw {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code16: Code,
	code32: Code,
}

impl OpCodeHandler_RvMw_Gw {
	pub(super) fn new(code16: Code, code32: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_RvMw_Gw::decode, code16, code32 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		let base_reg;
		if decoder.state.operand_size != OpSize::Size16 {
			instruction.set_code(this.code32);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32);
			base_reg = Register::EAX;
		} else {
			instruction.set_code(this.code16);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::AX as u32);
			base_reg = Register::AX;
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + base_reg as u32);
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Gv_Ev_Ib {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code16: Code,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Gv_Ev_Ib {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Gv_Ev_Ib::decode, code16, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			const_assert_eq!(Register::AX as u32 + 16, Register::EAX as u32);
			const_assert_eq!(Register::AX as u32 + 32, Register::RAX as u32);
			write_op1_reg!(
				instruction,
				(decoder.state.operand_size as u32) * 16 + decoder.state.rm + decoder.state.extra_base_register_base + Register::AX as u32
			);
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
		if decoder.state.operand_size == OpSize::Size32 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32);
			instruction.set_code(this.code32);
			instruction.set_op2_kind(OpKind::Immediate8to32);
		} else if decoder.state.operand_size == OpSize::Size64 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32);
			instruction.set_code(this.code64);
			instruction.set_op2_kind(OpKind::Immediate8to64);
		} else {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::AX as u32);
			instruction.set_code(this.code16);
			instruction.set_op2_kind(OpKind::Immediate8to16);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Gv_Ev_Ib_REX {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code32: Code,
	code64: Code,
	base_reg: Register,
}

impl OpCodeHandler_Gv_Ev_Ib_REX {
	pub(super) fn new(base_reg: Register, code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Gv_Ev_Ib_REX::decode, base_reg, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		debug_assert_eq!(decoder.state.mod_, 3);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32);
		instruction.set_op2_kind(OpKind::Immediate8);
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code64);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32);
		} else {
			instruction.set_code(this.code32);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Gv_Ev_32_64 {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	disallow_reg: u32,
	disallow_mem: u32,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Gv_Ev_32_64 {
	pub(super) fn new(code32: Code, code64: Code, allow_reg: bool, allow_mem: bool) -> Self {
		Self {
			decode: OpCodeHandler_Gv_Ev_32_64::decode,
			has_modrm: true,
			code32,
			code64,
			disallow_mem: if allow_mem { 0 } else { u32::MAX },
			disallow_reg: if allow_reg { 0 } else { u32::MAX },
		}
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		let base_reg;
		if decoder.is64b_mode {
			instruction.set_code(this.code64);
			base_reg = Register::RAX;
		} else {
			instruction.set_code(this.code32);
			base_reg = Register::EAX;
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + base_reg as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + base_reg as u32);
			if (this.disallow_reg & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			if (this.disallow_mem & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Gv_Ev_Iz {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code16: Code,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Gv_Ev_Iz {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Gv_Ev_Iz::decode, code16, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			const_assert_eq!(Register::AX as u32 + 16, Register::EAX as u32);
			const_assert_eq!(Register::AX as u32 + 32, Register::RAX as u32);
			write_op1_reg!(
				instruction,
				(decoder.state.operand_size as u32) * 16 + decoder.state.rm + decoder.state.extra_base_register_base + Register::AX as u32
			);
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		if decoder.state.operand_size == OpSize::Size32 {
			instruction.set_code(this.code32);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32);
			instruction.set_op2_kind(OpKind::Immediate32);
			instruction.set_immediate32(decoder.read_u32() as u32);
		} else if decoder.state.operand_size == OpSize::Size64 {
			instruction.set_code(this.code64);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32);
			instruction.set_op2_kind(OpKind::Immediate32to64);
			instruction.set_immediate32(decoder.read_u32() as u32);
		} else {
			instruction.set_code(this.code16);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::AX as u32);
			instruction.set_op2_kind(OpKind::Immediate16);
			instruction_internal::internal_set_immediate16(instruction, decoder.read_u16() as u32);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Yb_Reg {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	reg: Register,
}

impl OpCodeHandler_Yb_Reg {
	pub(super) fn new(code: Code, reg: Register) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_Yb_Reg::decode, code, reg }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction.set_op1_register(this.reg);
		if decoder.state.address_size == OpSize::Size64 {
			instruction.set_op0_kind(OpKind::MemoryESRDI);
		} else if decoder.state.address_size == OpSize::Size32 {
			instruction.set_op0_kind(OpKind::MemoryESEDI);
		} else {
			instruction.set_op0_kind(OpKind::MemoryESDI);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Yv_Reg {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: [Code; 3],
}

impl OpCodeHandler_Yv_Reg {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_Yv_Reg::decode, code: [code16, code32, code64] }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if decoder.state.address_size == OpSize::Size64 {
			instruction.set_op0_kind(OpKind::MemoryESRDI);
		} else if decoder.state.address_size == OpSize::Size32 {
			instruction.set_op0_kind(OpKind::MemoryESEDI);
		} else {
			instruction.set_op0_kind(OpKind::MemoryESDI);
		}
		instruction.set_code(this.code[decoder.state.operand_size as usize]);
		if decoder.state.operand_size == OpSize::Size32 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			instruction.set_op1_register(Register::EAX);
		} else if decoder.state.operand_size == OpSize::Size64 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			instruction.set_op1_register(Register::RAX);
		} else {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			instruction.set_op1_register(Register::AX);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Yv_Reg2 {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code16: Code,
	code32: Code,
}

impl OpCodeHandler_Yv_Reg2 {
	pub(super) fn new(code16: Code, code32: Code) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_Yv_Reg2::decode, code16, code32 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if decoder.state.address_size == OpSize::Size64 {
			instruction.set_op0_kind(OpKind::MemoryESRDI);
		} else if decoder.state.address_size == OpSize::Size32 {
			instruction.set_op0_kind(OpKind::MemoryESEDI);
		} else {
			instruction.set_op0_kind(OpKind::MemoryESDI);
		}
		if decoder.state.operand_size != OpSize::Size16 {
			instruction.set_code(this.code32);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			instruction.set_op1_register(Register::DX);
		} else {
			instruction.set_code(this.code16);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			instruction.set_op1_register(Register::DX);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Reg_Xb {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	reg: Register,
}

impl OpCodeHandler_Reg_Xb {
	pub(super) fn new(code: Code, reg: Register) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_Reg_Xb::decode, code, reg }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(this.reg);
		if decoder.state.address_size == OpSize::Size64 {
			instruction.set_op1_kind(OpKind::MemorySegRSI);
		} else if decoder.state.address_size == OpSize::Size32 {
			instruction.set_op1_kind(OpKind::MemorySegESI);
		} else {
			instruction.set_op1_kind(OpKind::MemorySegSI);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Reg_Xv {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: [Code; 3],
}

impl OpCodeHandler_Reg_Xv {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_Reg_Xv::decode, code: [code16, code32, code64] }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if decoder.state.address_size == OpSize::Size64 {
			instruction.set_op1_kind(OpKind::MemorySegRSI);
		} else if decoder.state.address_size == OpSize::Size32 {
			instruction.set_op1_kind(OpKind::MemorySegESI);
		} else {
			instruction.set_op1_kind(OpKind::MemorySegSI);
		}
		instruction.set_code(this.code[decoder.state.operand_size as usize]);
		if decoder.state.operand_size == OpSize::Size32 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			instruction.set_op0_register(Register::EAX);
		} else if decoder.state.operand_size == OpSize::Size64 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			instruction.set_op0_register(Register::RAX);
		} else {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			instruction.set_op0_register(Register::AX);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Reg_Xv2 {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code16: Code,
	code32: Code,
}

impl OpCodeHandler_Reg_Xv2 {
	pub(super) fn new(code16: Code, code32: Code) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_Reg_Xv2::decode, code16, code32 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if decoder.state.address_size == OpSize::Size64 {
			instruction.set_op1_kind(OpKind::MemorySegRSI);
		} else if decoder.state.address_size == OpSize::Size32 {
			instruction.set_op1_kind(OpKind::MemorySegESI);
		} else {
			instruction.set_op1_kind(OpKind::MemorySegSI);
		}
		if decoder.state.operand_size != OpSize::Size16 {
			instruction.set_code(this.code32);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			instruction.set_op0_register(Register::DX);
		} else {
			instruction.set_code(this.code16);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			instruction.set_op0_register(Register::DX);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Reg_Yb {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	reg: Register,
}

impl OpCodeHandler_Reg_Yb {
	pub(super) fn new(code: Code, reg: Register) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_Reg_Yb::decode, code, reg }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(this.reg);
		if decoder.state.address_size == OpSize::Size64 {
			instruction.set_op1_kind(OpKind::MemoryESRDI);
		} else if decoder.state.address_size == OpSize::Size32 {
			instruction.set_op1_kind(OpKind::MemoryESEDI);
		} else {
			instruction.set_op1_kind(OpKind::MemoryESDI);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Reg_Yv {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: [Code; 3],
}

impl OpCodeHandler_Reg_Yv {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_Reg_Yv::decode, code: [code16, code32, code64] }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if decoder.state.address_size == OpSize::Size64 {
			instruction.set_op1_kind(OpKind::MemoryESRDI);
		} else if decoder.state.address_size == OpSize::Size32 {
			instruction.set_op1_kind(OpKind::MemoryESEDI);
		} else {
			instruction.set_op1_kind(OpKind::MemoryESDI);
		}
		instruction.set_code(this.code[decoder.state.operand_size as usize]);
		if decoder.state.operand_size == OpSize::Size32 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			instruction.set_op0_register(Register::EAX);
		} else if decoder.state.operand_size == OpSize::Size64 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			instruction.set_op0_register(Register::RAX);
		} else {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			instruction.set_op0_register(Register::AX);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Yb_Xb {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
}

impl OpCodeHandler_Yb_Xb {
	pub(super) fn new(code: Code) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_Yb_Xb::decode, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		if decoder.state.address_size == OpSize::Size64 {
			instruction.set_op0_kind(OpKind::MemoryESRDI);
			instruction.set_op1_kind(OpKind::MemorySegRSI);
		} else if decoder.state.address_size == OpSize::Size32 {
			instruction.set_op0_kind(OpKind::MemoryESEDI);
			instruction.set_op1_kind(OpKind::MemorySegESI);
		} else {
			instruction.set_op0_kind(OpKind::MemoryESDI);
			instruction.set_op1_kind(OpKind::MemorySegSI);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Yv_Xv {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: [Code; 3],
}

impl OpCodeHandler_Yv_Xv {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_Yv_Xv::decode, code: [code16, code32, code64] }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code[decoder.state.operand_size as usize]);
		if decoder.state.address_size == OpSize::Size64 {
			instruction.set_op0_kind(OpKind::MemoryESRDI);
			instruction.set_op1_kind(OpKind::MemorySegRSI);
		} else if decoder.state.address_size == OpSize::Size32 {
			instruction.set_op0_kind(OpKind::MemoryESEDI);
			instruction.set_op1_kind(OpKind::MemorySegESI);
		} else {
			instruction.set_op0_kind(OpKind::MemoryESDI);
			instruction.set_op1_kind(OpKind::MemorySegSI);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Xb_Yb {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
}

impl OpCodeHandler_Xb_Yb {
	pub(super) fn new(code: Code) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_Xb_Yb::decode, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		if decoder.state.address_size == OpSize::Size64 {
			instruction.set_op0_kind(OpKind::MemorySegRSI);
			instruction.set_op1_kind(OpKind::MemoryESRDI);
		} else if decoder.state.address_size == OpSize::Size32 {
			instruction.set_op0_kind(OpKind::MemorySegESI);
			instruction.set_op1_kind(OpKind::MemoryESEDI);
		} else {
			instruction.set_op0_kind(OpKind::MemorySegSI);
			instruction.set_op1_kind(OpKind::MemoryESDI);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Xv_Yv {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: [Code; 3],
}

impl OpCodeHandler_Xv_Yv {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_Xv_Yv::decode, code: [code16, code32, code64] }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code[decoder.state.operand_size as usize]);
		if decoder.state.address_size == OpSize::Size64 {
			instruction.set_op0_kind(OpKind::MemorySegRSI);
			instruction.set_op1_kind(OpKind::MemoryESRDI);
		} else if decoder.state.address_size == OpSize::Size32 {
			instruction.set_op0_kind(OpKind::MemorySegESI);
			instruction.set_op1_kind(OpKind::MemoryESEDI);
		} else {
			instruction.set_op0_kind(OpKind::MemorySegSI);
			instruction.set_op1_kind(OpKind::MemoryESDI);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Ev_Sw {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: [Code; 3],
}

impl OpCodeHandler_Ev_Sw {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Ev_Sw::decode, code: [code16, code32, code64] }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code[decoder.state.operand_size as usize]);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		let seg = decoder.read_op_seg_reg();
		write_op1_reg!(instruction, seg);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			const_assert_eq!(Register::AX as u32 + 16, Register::EAX as u32);
			const_assert_eq!(Register::AX as u32 + 32, Register::RAX as u32);
			write_op0_reg!(
				instruction,
				(decoder.state.operand_size as u32) * 16 + decoder.state.rm + decoder.state.extra_base_register_base + Register::AX as u32
			);
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_M_Sw {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
}

impl OpCodeHandler_M_Sw {
	pub(super) fn new(code: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_M_Sw::decode, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		let seg = decoder.read_op_seg_reg();
		write_op1_reg!(instruction, seg);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Gv_M {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: [Code; 3],
}

impl OpCodeHandler_Gv_M {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Gv_M::decode, code: [code16, code32, code64] }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code[decoder.state.operand_size as usize]);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		const_assert_eq!(Register::AX as u32 + 16, Register::EAX as u32);
		const_assert_eq!(Register::AX as u32 + 32, Register::RAX as u32);
		write_op0_reg!(
			instruction,
			(decoder.state.operand_size as u32) * 16 + decoder.state.reg + decoder.state.extra_register_base + Register::AX as u32
		);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Sw_Ev {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: [Code; 3],
}

impl OpCodeHandler_Sw_Ev {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Sw_Ev::decode, code: [code16, code32, code64] }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code[decoder.state.operand_size as usize]);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		let sreg = decoder.read_op_seg_reg();
		if decoder.invalid_check_mask != 0 && sreg == Register::CS as u32 {
			decoder.set_invalid_instruction();
		}
		write_op0_reg!(instruction, sreg);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			const_assert_eq!(Register::AX as u32 + 16, Register::EAX as u32);
			const_assert_eq!(Register::AX as u32 + 32, Register::RAX as u32);
			write_op1_reg!(
				instruction,
				(decoder.state.operand_size as u32) * 16 + decoder.state.rm + decoder.state.extra_base_register_base + Register::AX as u32
			);
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Sw_M {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
}

impl OpCodeHandler_Sw_M {
	pub(super) fn new(code: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Sw_M::decode, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		let seg = decoder.read_op_seg_reg();
		write_op0_reg!(instruction, seg);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Ap {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code16: Code,
	code32: Code,
}

impl OpCodeHandler_Ap {
	pub(super) fn new(code16: Code, code32: Code) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_Ap::decode, code16, code32 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if decoder.state.operand_size != OpSize::Size16 {
			instruction.set_code(this.code32);
			instruction.set_op0_kind(OpKind::FarBranch32);
			instruction.set_far_branch32(decoder.read_u32() as u32);
			instruction_internal::internal_set_far_branch_selector(instruction, decoder.read_u16() as u32);
		} else {
			instruction.set_code(this.code16);
			instruction.set_op0_kind(OpKind::FarBranch16);
			let d = decoder.read_u32() as u32;
			instruction_internal::internal_set_far_branch16(instruction, d as u16 as u32);
			instruction_internal::internal_set_far_branch_selector(instruction, d >> 16);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Reg_Ob {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	reg: Register,
}

impl OpCodeHandler_Reg_Ob {
	pub(super) fn new(code: Code, reg: Register) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_Reg_Ob::decode, code, reg }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(this.reg);
		decoder.displ_index = decoder.data_ptr as u8;
		if decoder.state.address_size == OpSize::Size64 {
			instruction_internal::internal_set_memory_displ_size(instruction, 4);
			decoder.state.flags |= StateFlags::ADDR64;
			let q = decoder.read_u64();
			instruction_internal::internal_set_memory_displacement64_lo(instruction, q as u32);
			instruction_internal::internal_set_memory_displacement64_hi(instruction, (q >> 32) as u32);
			//instruction_internal::internal_set_memory_index_scale(instruction, 0);
			//instruction.set_memory_base(Register::None);
			//instruction.set_memory_index(Register::None);
			instruction.set_op1_kind(OpKind::Memory);
		} else if decoder.state.address_size == OpSize::Size32 {
			instruction_internal::internal_set_memory_displ_size(instruction, 3);
			instruction_internal::internal_set_memory_displacement64_lo(instruction, decoder.read_u32() as u32);
			//instruction_internal::internal_set_memory_index_scale(instruction, 0);
			//instruction.set_memory_base(Register::None);
			//instruction.set_memory_index(Register::None);
			instruction.set_op1_kind(OpKind::Memory);
		} else {
			instruction_internal::internal_set_memory_displ_size(instruction, 2);
			instruction_internal::internal_set_memory_displacement64_lo(instruction, decoder.read_u16() as u32);
			//instruction_internal::internal_set_memory_index_scale(instruction, 0);
			//instruction.set_memory_base(Register::None);
			//instruction.set_memory_index(Register::None);
			instruction.set_op1_kind(OpKind::Memory);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Ob_Reg {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	reg: Register,
}

impl OpCodeHandler_Ob_Reg {
	pub(super) fn new(code: Code, reg: Register) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_Ob_Reg::decode, code, reg }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction.set_op1_register(this.reg);
		decoder.displ_index = decoder.data_ptr as u8;
		if decoder.state.address_size == OpSize::Size64 {
			instruction_internal::internal_set_memory_displ_size(instruction, 4);
			decoder.state.flags |= StateFlags::ADDR64;
			let q = decoder.read_u64();
			instruction_internal::internal_set_memory_displacement64_lo(instruction, q as u32);
			instruction_internal::internal_set_memory_displacement64_hi(instruction, (q >> 32) as u32);
			//instruction_internal::internal_set_memory_index_scale(instruction, 0);
			//instruction.set_memory_base(Register::None);
			//instruction.set_memory_index(Register::None);
			instruction.set_op0_kind(OpKind::Memory);
		} else if decoder.state.address_size == OpSize::Size32 {
			instruction_internal::internal_set_memory_displ_size(instruction, 3);
			instruction_internal::internal_set_memory_displacement64_lo(instruction, decoder.read_u32() as u32);
			//instruction_internal::internal_set_memory_index_scale(instruction, 0);
			//instruction.set_memory_base(Register::None);
			//instruction.set_memory_index(Register::None);
			instruction.set_op0_kind(OpKind::Memory);
		} else {
			instruction_internal::internal_set_memory_displ_size(instruction, 2);
			instruction_internal::internal_set_memory_displacement64_lo(instruction, decoder.read_u16() as u32);
			//instruction_internal::internal_set_memory_index_scale(instruction, 0);
			//instruction.set_memory_base(Register::None);
			//instruction.set_memory_index(Register::None);
			instruction.set_op0_kind(OpKind::Memory);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Reg_Ov {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: [Code; 3],
}

impl OpCodeHandler_Reg_Ov {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_Reg_Ov::decode, code: [code16, code32, code64] }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		decoder.displ_index = decoder.data_ptr as u8;
		if decoder.state.address_size == OpSize::Size64 {
			instruction_internal::internal_set_memory_displ_size(instruction, 4);
			decoder.state.flags |= StateFlags::ADDR64;
			let q = decoder.read_u64();
			instruction_internal::internal_set_memory_displacement64_lo(instruction, q as u32);
			instruction_internal::internal_set_memory_displacement64_hi(instruction, (q >> 32) as u32);
			//instruction_internal::internal_set_memory_index_scale(instruction, 0);
			//instruction.set_memory_base(Register::None);
			//instruction.set_memory_index(Register::None);
			instruction.set_op1_kind(OpKind::Memory);
		} else if decoder.state.address_size == OpSize::Size32 {
			instruction_internal::internal_set_memory_displ_size(instruction, 3);
			instruction_internal::internal_set_memory_displacement64_lo(instruction, decoder.read_u32() as u32);
			//instruction_internal::internal_set_memory_index_scale(instruction, 0);
			//instruction.set_memory_base(Register::None);
			//instruction.set_memory_index(Register::None);
			instruction.set_op1_kind(OpKind::Memory);
		} else {
			instruction_internal::internal_set_memory_displ_size(instruction, 2);
			instruction_internal::internal_set_memory_displacement64_lo(instruction, decoder.read_u16() as u32);
			//instruction_internal::internal_set_memory_index_scale(instruction, 0);
			//instruction.set_memory_base(Register::None);
			//instruction.set_memory_index(Register::None);
			instruction.set_op1_kind(OpKind::Memory);
		}
		instruction.set_code(this.code[decoder.state.operand_size as usize]);
		if decoder.state.operand_size == OpSize::Size32 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			instruction.set_op0_register(Register::EAX);
		} else if decoder.state.operand_size == OpSize::Size64 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			instruction.set_op0_register(Register::RAX);
		} else {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			instruction.set_op0_register(Register::AX);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Ov_Reg {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: [Code; 3],
}

impl OpCodeHandler_Ov_Reg {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_Ov_Reg::decode, code: [code16, code32, code64] }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		decoder.displ_index = decoder.data_ptr as u8;
		if decoder.state.address_size == OpSize::Size64 {
			instruction_internal::internal_set_memory_displ_size(instruction, 4);
			decoder.state.flags |= StateFlags::ADDR64;
			let q = decoder.read_u64();
			instruction_internal::internal_set_memory_displacement64_lo(instruction, q as u32);
			instruction_internal::internal_set_memory_displacement64_hi(instruction, (q >> 32) as u32);
			//instruction_internal::internal_set_memory_index_scale(instruction, 0);
			//instruction.set_memory_base(Register::None);
			//instruction.set_memory_index(Register::None);
			instruction.set_op0_kind(OpKind::Memory);
		} else if decoder.state.address_size == OpSize::Size32 {
			instruction_internal::internal_set_memory_displ_size(instruction, 3);
			instruction_internal::internal_set_memory_displacement64_lo(instruction, decoder.read_u32() as u32);
			//instruction_internal::internal_set_memory_index_scale(instruction, 0);
			//instruction.set_memory_base(Register::None);
			//instruction.set_memory_index(Register::None);
			instruction.set_op0_kind(OpKind::Memory);
		} else {
			instruction_internal::internal_set_memory_displ_size(instruction, 2);
			instruction_internal::internal_set_memory_displacement64_lo(instruction, decoder.read_u16() as u32);
			//instruction_internal::internal_set_memory_index_scale(instruction, 0);
			//instruction.set_memory_base(Register::None);
			//instruction.set_memory_index(Register::None);
			instruction.set_op0_kind(OpKind::Memory);
		}
		instruction.set_code(this.code[decoder.state.operand_size as usize]);
		if decoder.state.operand_size == OpSize::Size32 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			instruction.set_op1_register(Register::EAX);
		} else if decoder.state.operand_size == OpSize::Size64 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			instruction.set_op1_register(Register::RAX);
		} else {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			instruction.set_op1_register(Register::AX);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_BranchIw {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code16: Code,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_BranchIw {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_BranchIw::decode, code16, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_op0_kind(OpKind::Immediate16);
		instruction_internal::internal_set_immediate16(instruction, decoder.read_u16() as u32);
		if decoder.is64b_mode {
			if (decoder.options & DecoderOptions::AMD) == 0 || decoder.state.operand_size != OpSize::Size16 {
				instruction.set_code(this.code64);
			} else {
				instruction.set_code(this.code16);
			}
		} else {
			if decoder.state.operand_size == OpSize::Size32 {
				instruction.set_code(this.code32);
			} else {
				instruction.set_code(this.code16);
			}
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_BranchSimple {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code16: Code,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_BranchSimple {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_BranchSimple::decode, code16, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if decoder.is64b_mode {
			if (decoder.options & DecoderOptions::AMD) == 0 || decoder.state.operand_size != OpSize::Size16 {
				instruction.set_code(this.code64);
			} else {
				instruction.set_code(this.code16);
			}
		} else {
			if decoder.state.operand_size == OpSize::Size32 {
				instruction.set_code(this.code32);
			} else {
				instruction.set_code(this.code16);
			}
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Iw_Ib {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code16: Code,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Iw_Ib {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_Iw_Ib::decode, code16, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_op0_kind(OpKind::Immediate16);
		instruction.set_op1_kind(OpKind::Immediate8_2nd);
		instruction_internal::internal_set_immediate16(instruction, decoder.read_u16() as u32);
		instruction_internal::internal_set_immediate8_2nd(instruction, decoder.read_u8() as u32);
		if decoder.is64b_mode {
			if decoder.state.operand_size != OpSize::Size16 {
				instruction.set_code(this.code64);
			} else {
				instruction.set_code(this.code16);
			}
		} else {
			if decoder.state.operand_size == OpSize::Size32 {
				instruction.set_code(this.code32);
			} else {
				instruction.set_code(this.code16);
			}
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Reg_Ib2 {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code16: Code,
	code32: Code,
}

impl OpCodeHandler_Reg_Ib2 {
	pub(super) fn new(code16: Code, code32: Code) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_Reg_Ib2::decode, code16, code32 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op1_kind(OpKind::Immediate8);
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
		if decoder.state.operand_size != OpSize::Size16 {
			instruction.set_code(this.code32);
			instruction.set_op0_register(Register::EAX);
		} else {
			instruction.set_code(this.code16);
			instruction.set_op0_register(Register::AX);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_IbReg2 {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code16: Code,
	code32: Code,
}

impl OpCodeHandler_IbReg2 {
	pub(super) fn new(code16: Code, code32: Code) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_IbReg2::decode, code16, code32 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_op0_kind(OpKind::Immediate8);
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		if decoder.state.operand_size != OpSize::Size16 {
			instruction.set_code(this.code32);
			instruction.set_op1_register(Register::EAX);
		} else {
			instruction.set_code(this.code16);
			instruction.set_op1_register(Register::AX);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_eAX_DX {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code16: Code,
	code32: Code,
}

impl OpCodeHandler_eAX_DX {
	pub(super) fn new(code16: Code, code32: Code) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_eAX_DX::decode, code16, code32 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction.set_op1_register(Register::DX);
		if decoder.state.operand_size != OpSize::Size16 {
			instruction.set_code(this.code32);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			instruction.set_op0_register(Register::EAX);
		} else {
			instruction.set_code(this.code16);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			instruction.set_op0_register(Register::AX);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_DX_eAX {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code16: Code,
	code32: Code,
}

impl OpCodeHandler_DX_eAX {
	pub(super) fn new(code16: Code, code32: Code) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_DX_eAX::decode, code16, code32 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction.set_op0_register(Register::DX);
		if decoder.state.operand_size != OpSize::Size16 {
			instruction.set_code(this.code32);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			instruction.set_op1_register(Register::EAX);
		} else {
			instruction.set_code(this.code16);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			instruction.set_op1_register(Register::AX);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Eb_Ib {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	flags: u32,
	code: Code,
	state_flags_or_value: u32,
}

impl OpCodeHandler_Eb_Ib {
	pub(super) fn new(code: Code, flags: u32) -> Self {
		const_assert_eq!(HandlerFlags::LOCK, 1 << 3);
		const_assert_eq!(StateFlags::ALLOW_LOCK, 1 << 13);
		let state_flags_or_value = (flags & HandlerFlags::LOCK) << (13 - 3);

		Self { has_modrm: true, decode: OpCodeHandler_Eb_Ib::decode, flags, code, state_flags_or_value }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		if decoder.state.mod_ == 3 {
			let mut index = decoder.state.rm + decoder.state.extra_base_register_base;
			if (decoder.state.flags & StateFlags::HAS_REX) != 0 && index >= 4 {
				index += 4;
			}
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, index + Register::AL as u32);
		} else {
			if (this.flags & (HandlerFlags::XACQUIRE | HandlerFlags::XRELEASE)) != 0 {
				decoder.set_xacquire_xrelease(instruction, this.flags);
			}
			decoder.state.flags |= this.state_flags_or_value;
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		instruction.set_op1_kind(OpKind::Immediate8);
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Eb_1 {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
}

impl OpCodeHandler_Eb_1 {
	pub(super) fn new(code: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Eb_1::decode, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		instruction.set_op1_kind(OpKind::Immediate8);
		instruction_internal::internal_set_immediate8(instruction, 1);
		decoder.state.flags |= StateFlags::NO_IMM;
		if decoder.state.mod_ == 3 {
			let mut index = decoder.state.rm + decoder.state.extra_base_register_base;
			if (decoder.state.flags & StateFlags::HAS_REX) != 0 && index >= 4 {
				index += 4;
			}
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, index + Register::AL as u32);
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Eb_CL {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
}

impl OpCodeHandler_Eb_CL {
	pub(super) fn new(code: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Eb_CL::decode, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction.set_op1_register(Register::CL);
		if decoder.state.mod_ == 3 {
			let mut index = decoder.state.rm + decoder.state.extra_base_register_base;
			if (decoder.state.flags & StateFlags::HAS_REX) != 0 && index >= 4 {
				index += 4;
			}
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, index + Register::AL as u32);
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Eb {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	flags: u32,
	code: Code,
	state_flags_or_value: u32,
}

impl OpCodeHandler_Eb {
	pub(super) fn new(code: Code, flags: u32) -> Self {
		const_assert_eq!(HandlerFlags::LOCK, 1 << 3);
		const_assert_eq!(StateFlags::ALLOW_LOCK, 1 << 13);
		let state_flags_or_value = (flags & HandlerFlags::LOCK) << (13 - 3);

		Self { has_modrm: true, decode: OpCodeHandler_Eb::decode, flags, code, state_flags_or_value }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		if decoder.state.mod_ == 3 {
			let mut index = decoder.state.rm + decoder.state.extra_base_register_base;
			if (decoder.state.flags & StateFlags::HAS_REX) != 0 && index >= 4 {
				index += 4;
			}
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, index + Register::AL as u32);
		} else {
			if (this.flags & (HandlerFlags::XACQUIRE | HandlerFlags::XRELEASE)) != 0 {
				decoder.set_xacquire_xrelease(instruction, this.flags);
			}
			decoder.state.flags |= this.state_flags_or_value;
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Eb_Gb {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	flags: u32,
	code: Code,
	state_flags_or_value: u32,
}

impl OpCodeHandler_Eb_Gb {
	pub(super) fn new(code: Code, flags: u32) -> Self {
		const_assert_eq!(HandlerFlags::LOCK, 1 << 3);
		const_assert_eq!(StateFlags::ALLOW_LOCK, 1 << 13);
		let state_flags_or_value = (flags & HandlerFlags::LOCK) << (13 - 3);

		Self { has_modrm: true, decode: OpCodeHandler_Eb_Gb::decode, flags, code, state_flags_or_value }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		let mut index = decoder.state.reg + decoder.state.extra_register_base;
		if (decoder.state.flags & StateFlags::HAS_REX) != 0 && index >= 4 {
			index += 4;
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		write_op1_reg!(instruction, index + Register::AL as u32);
		if decoder.state.mod_ == 3 {
			index = decoder.state.rm + decoder.state.extra_base_register_base;
			if (decoder.state.flags & StateFlags::HAS_REX) != 0 && index >= 4 {
				index += 4;
			}
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, index + Register::AL as u32);
		} else {
			if (this.flags & (HandlerFlags::XACQUIRE | HandlerFlags::XRELEASE)) != 0 {
				decoder.set_xacquire_xrelease(instruction, this.flags);
			}
			decoder.state.flags |= this.state_flags_or_value;
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Gb_Eb {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
}

impl OpCodeHandler_Gb_Eb {
	pub(super) fn new(code: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Gb_Eb::decode, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		let mut index = decoder.state.reg + decoder.state.extra_register_base;
		if (decoder.state.flags & StateFlags::HAS_REX) != 0 && index >= 4 {
			index += 4;
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(instruction, index + Register::AL as u32);

		if decoder.state.mod_ == 3 {
			index = decoder.state.rm + decoder.state.extra_base_register_base;
			if (decoder.state.flags & StateFlags::HAS_REX) != 0 && index >= 4 {
				index += 4;
			}
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, index + Register::AL as u32);
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_M {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code_w0: Code,
	code_w1: Code,
}

impl OpCodeHandler_M {
	pub(super) fn new(code: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_M::decode, code_w0: code, code_w1: code }
	}

	pub(super) fn new1(code_w0: Code, code_w1: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_M::decode, code_w0, code_w1 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code_w1);
		} else {
			instruction.set_code(this.code_w0);
		}
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_M_REXW {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	flags32: u32,
	flags64: u32,
	code32: Code,
	code64: Code,
	state_flags_or_value: u32,
}

impl OpCodeHandler_M_REXW {
	pub(super) fn new(code32: Code, code64: Code, flags32: u32, flags64: u32) -> Self {
		debug_assert_eq!(flags32 & HandlerFlags::LOCK, flags64 & HandlerFlags::LOCK);

		const_assert_eq!(HandlerFlags::LOCK, 1 << 3);
		const_assert_eq!(StateFlags::ALLOW_LOCK, 1 << 13);
		let state_flags_or_value = (flags32 & HandlerFlags::LOCK) << (13 - 3);

		Self { has_modrm: true, decode: OpCodeHandler_M_REXW::decode, flags32, flags64, code32, code64, state_flags_or_value }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code64);
		} else {
			instruction.set_code(this.code32);
		}
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			let flags;
			if (decoder.state.flags & StateFlags::W) != 0 {
				flags = this.flags64;
			} else {
				flags = this.flags32;
			}
			if (flags & (HandlerFlags::XACQUIRE | HandlerFlags::XRELEASE)) != 0 {
				decoder.set_xacquire_xrelease(instruction, flags);
			}
			decoder.state.flags |= this.state_flags_or_value;
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_MemBx {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
}

impl OpCodeHandler_MemBx {
	pub(super) fn new(code: Code) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_MemBx::decode, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		instruction.set_memory_index(Register::AL);
		instruction.set_op0_kind(OpKind::Memory);
		//instruction.set_memory_displacement64(0);
		//instruction_internal::internal_set_memory_index_scale(instruction, 0);
		//instruction_internal::internal_set_memory_displ_size(instruction, 0);
		const_assert_eq!(Register::BX as u32 + 16, Register::EBX as u32);
		const_assert_eq!(Register::BX as u32 + 32, Register::RBX as u32);
		instruction.set_memory_base((decoder.state.address_size as u32) * 16 + Register::BX);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VW {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code_r: Code,
	code_m: Code,
	base_reg: Register,
}

impl OpCodeHandler_VW {
	pub(super) fn new(base_reg: Register, code: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_VW::decode, base_reg, code_r: code, code_m: code }
	}

	pub(super) fn new1(base_reg: Register, code_r: Code, code_m: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_VW::decode, base_reg, code_r, code_m }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			instruction.set_code(this.code_r);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32);
		} else {
			if this.code_m == Code::INVALID {
				decoder.set_invalid_instruction();
			}
			instruction.set_code(this.code_m);
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_WV {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg: Register,
}

impl OpCodeHandler_WV {
	pub(super) fn new(base_reg: Register, code: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_WV::decode, base_reg, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		write_op1_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32);
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_rDI_VX_RX {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg: Register,
}

impl OpCodeHandler_rDI_VX_RX {
	pub(super) fn new(base_reg: Register, code: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_rDI_VX_RX::decode, base_reg, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		if decoder.state.address_size == OpSize::Size64 {
			instruction.set_op0_kind(OpKind::MemorySegRDI);
		} else if decoder.state.address_size == OpSize::Size32 {
			instruction.set_op0_kind(OpKind::MemorySegEDI);
		} else {
			instruction.set_op0_kind(OpKind::MemorySegDI);
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		write_op1_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op2_kind(OpKind::Register);
			write_op2_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_rDI_P_N {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
}

impl OpCodeHandler_rDI_P_N {
	pub(super) fn new(code: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_rDI_P_N::decode, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		if decoder.state.address_size == OpSize::Size64 {
			instruction.set_op0_kind(OpKind::MemorySegRDI);
		} else if decoder.state.address_size == OpSize::Size32 {
			instruction.set_op0_kind(OpKind::MemorySegEDI);
		} else {
			instruction.set_op0_kind(OpKind::MemorySegDI);
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		write_op1_reg!(instruction, decoder.state.reg + Register::MM0 as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op2_kind(OpKind::Register);
			write_op2_reg!(instruction, decoder.state.rm + Register::MM0 as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VM {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg: Register,
}

impl OpCodeHandler_VM {
	pub(super) fn new(base_reg: Register, code: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_VM::decode, base_reg, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_MV {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg: Register,
}

impl OpCodeHandler_MV {
	pub(super) fn new(base_reg: Register, code: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_MV::decode, base_reg, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		write_op1_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VQ {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg: Register,
}

impl OpCodeHandler_VQ {
	pub(super) fn new(base_reg: Register, code: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_VQ::decode, base_reg, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, decoder.state.rm + Register::MM0 as u32);
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_P_Q {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
}

impl OpCodeHandler_P_Q {
	pub(super) fn new(code: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_P_Q::decode, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(instruction, decoder.state.reg + Register::MM0 as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, decoder.state.rm + Register::MM0 as u32);
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Q_P {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
}

impl OpCodeHandler_Q_P {
	pub(super) fn new(code: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Q_P::decode, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		write_op1_reg!(instruction, decoder.state.reg + Register::MM0 as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, decoder.state.rm + Register::MM0 as u32);
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_MP {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
}

impl OpCodeHandler_MP {
	pub(super) fn new(code: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_MP::decode, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		write_op1_reg!(instruction, decoder.state.reg + Register::MM0 as u32);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_P_Q_Ib {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
}

impl OpCodeHandler_P_Q_Ib {
	pub(super) fn new(code: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_P_Q_Ib::decode, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(instruction, decoder.state.reg + Register::MM0 as u32);
		instruction.set_op2_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, decoder.state.rm + Register::MM0 as u32);
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_P_W {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg: Register,
}

impl OpCodeHandler_P_W {
	pub(super) fn new(base_reg: Register, code: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_P_W::decode, base_reg, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(instruction, decoder.state.reg + Register::MM0 as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32);
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_P_R {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg: Register,
}

impl OpCodeHandler_P_R {
	pub(super) fn new(base_reg: Register, code: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_P_R::decode, base_reg, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(instruction, decoder.state.reg + Register::MM0 as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_P_Ev {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_P_Ev {
	pub(super) fn new(code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_P_Ev::decode, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		let gpr;
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code64);
			gpr = Register::RAX;
		} else {
			instruction.set_code(this.code32);
			gpr = Register::EAX;
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(instruction, decoder.state.reg + Register::MM0 as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr as u32);
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_P_Ev_Ib {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_P_Ev_Ib {
	pub(super) fn new(code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_P_Ev_Ib::decode, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		let gpr;
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code64);
			gpr = Register::RAX;
		} else {
			instruction.set_code(this.code32);
			gpr = Register::EAX;
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(instruction, decoder.state.reg + Register::MM0 as u32);
		instruction.set_op2_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr as u32);
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Ev_P {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Ev_P {
	pub(super) fn new(code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Ev_P::decode, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		let gpr;
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code64);
			gpr = Register::RAX;
		} else {
			instruction.set_code(this.code32);
			gpr = Register::EAX;
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		write_op1_reg!(instruction, decoder.state.reg + Register::MM0 as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr as u32);
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Gv_W {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code_w0: Code,
	code_w1: Code,
	base_reg: Register,
}

impl OpCodeHandler_Gv_W {
	pub(super) fn new(base_reg: Register, code_w0: Code, code_w1: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Gv_W::decode, base_reg, code_w0, code_w1 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code_w1);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32);
		} else {
			instruction.set_code(this.code_w0);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32);
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32);
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_V_Ev {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code_w0: Code,
	code_w1: Code,
	base_reg: Register,
}

impl OpCodeHandler_V_Ev {
	pub(super) fn new(base_reg: Register, code_w0: Code, code_w1: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_V_Ev::decode, base_reg, code_w0, code_w1 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		let gpr;
		if decoder.state.operand_size != OpSize::Size64 {
			instruction.set_code(this.code_w0);
			gpr = Register::EAX;
		} else {
			instruction.set_code(this.code_w1);
			gpr = Register::RAX;
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr as u32);
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VWIb {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code_w0: Code,
	code_w1: Code,
	base_reg: Register,
}

impl OpCodeHandler_VWIb {
	pub(super) fn new(base_reg: Register, code: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_VWIb::decode, base_reg, code_w0: code, code_w1: code }
	}

	pub(super) fn new1(base_reg: Register, code_w0: Code, code_w1: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_VWIb::decode, base_reg, code_w0, code_w1 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code_w1);
		} else {
			instruction.set_code(this.code_w0);
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32);
		instruction.set_op2_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32);
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VRIbIb {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg: Register,
}

impl OpCodeHandler_VRIbIb {
	pub(super) fn new(base_reg: Register, code: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_VRIbIb::decode, base_reg, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32);
		instruction.set_op2_kind(OpKind::Immediate8);
		instruction.set_op3_kind(OpKind::Immediate8_2nd);
		let w = decoder.read_u16() as u32;
		instruction_internal::internal_set_immediate8(instruction, w as u8 as u32);
		instruction_internal::internal_set_immediate8_2nd(instruction, w >> 8);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_RIbIb {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg: Register,
}

impl OpCodeHandler_RIbIb {
	pub(super) fn new(base_reg: Register, code: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_RIbIb::decode, base_reg, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		instruction.set_op1_kind(OpKind::Immediate8);
		instruction.set_op2_kind(OpKind::Immediate8_2nd);
		let w = decoder.read_u16() as u32;
		instruction_internal::internal_set_immediate8(instruction, w as u8 as u32);
		instruction_internal::internal_set_immediate8_2nd(instruction, w >> 8);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_RIb {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg: Register,
}

impl OpCodeHandler_RIb {
	pub(super) fn new(base_reg: Register, code: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_RIb::decode, base_reg, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		instruction.set_op1_kind(OpKind::Immediate8);
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Ed_V_Ib {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code32: Code,
	code64: Code,
	base_reg: Register,
}

impl OpCodeHandler_Ed_V_Ib {
	pub(super) fn new(base_reg: Register, code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Ed_V_Ib::decode, base_reg, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		write_op1_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32);
		instruction.set_op2_kind(OpKind::Immediate8);
		let gpr;
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code64);
			gpr = Register::RAX;
		} else {
			instruction.set_code(this.code32);
			gpr = Register::EAX;
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr as u32);
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VX_Ev {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_VX_Ev {
	pub(super) fn new(code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_VX_Ev::decode, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		let gpr;
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code64);
			gpr = Register::RAX;
		} else {
			instruction.set_code(this.code32);
			gpr = Register::EAX;
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::XMM0 as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr as u32);
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Ev_VX {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Ev_VX {
	pub(super) fn new(code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Ev_VX::decode, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		let gpr;
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code64);
			gpr = Register::RAX;
		} else {
			instruction.set_code(this.code32);
			gpr = Register::EAX;
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		write_op1_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::XMM0 as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr as u32);
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VX_E_Ib {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code32: Code,
	code64: Code,
	base_reg: Register,
}

impl OpCodeHandler_VX_E_Ib {
	pub(super) fn new(base_reg: Register, code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_VX_E_Ib::decode, base_reg, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		let gpr;
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code64);
			gpr = Register::RAX;
		} else {
			instruction.set_code(this.code32);
			gpr = Register::EAX;
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32);
		instruction.set_op2_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr as u32);
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Gv_RX {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code32: Code,
	code64: Code,
	base_reg: Register,
}

impl OpCodeHandler_Gv_RX {
	pub(super) fn new(base_reg: Register, code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Gv_RX::decode, base_reg, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code64);
		} else {
			instruction.set_code(this.code32);
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		if (decoder.state.flags & StateFlags::W) != 0 {
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32);
		} else {
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32);
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_B_MIB {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
}

impl OpCodeHandler_B_MIB {
	pub(super) fn new(code: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_B_MIB::decode, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if decoder.state.reg > 3 || (decoder.state.extra_register_base & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(instruction, (decoder.state.reg & 3) + Register::BND0 as u32);
		debug_assert_ne!(decoder.state.mod_, 3);
		instruction.set_op1_kind(OpKind::Memory);
		decoder.read_op_mem_mpx(instruction);
		// It can't be EIP since if it's MPX + 64-bit mode, the address size is always 64-bit
		if decoder.invalid_check_mask != 0 && instruction.memory_base() == Register::RIP {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_MIB_B {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
}

impl OpCodeHandler_MIB_B {
	pub(super) fn new(code: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_MIB_B::decode, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if decoder.state.reg > 3 || (decoder.state.extra_register_base & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		write_op1_reg!(instruction, (decoder.state.reg & 3) + Register::BND0 as u32);
		debug_assert_ne!(decoder.state.mod_, 3);
		instruction.set_op0_kind(OpKind::Memory);
		decoder.read_op_mem_mpx(instruction);
		// It can't be EIP since if it's MPX + 64-bit mode, the address size is always 64-bit
		if decoder.invalid_check_mask != 0 && instruction.memory_base() == Register::RIP {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_B_BM {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_B_BM {
	pub(super) fn new(code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_B_BM::decode, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if decoder.state.reg > 3 || (decoder.state.extra_register_base & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		if decoder.is64b_mode {
			instruction.set_code(this.code64);
		} else {
			instruction.set_code(this.code32);
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(instruction, (decoder.state.reg & 3) + Register::BND0 as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, (decoder.state.rm & 3) + Register::BND0 as u32);
			if decoder.state.rm > 3 || (decoder.state.extra_base_register_base & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem_mpx(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_BM_B {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_BM_B {
	pub(super) fn new(code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_BM_B::decode, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if decoder.state.reg > 3 || (decoder.state.extra_register_base & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		if decoder.is64b_mode {
			instruction.set_code(this.code64);
		} else {
			instruction.set_code(this.code32);
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		write_op1_reg!(instruction, (decoder.state.reg & 3) + Register::BND0 as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, (decoder.state.rm & 3) + Register::BND0 as u32);
			if decoder.state.rm > 3 || (decoder.state.extra_base_register_base & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem_mpx(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_B_Ev {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code32: Code,
	code64: Code,
	rip_rel_mask: u32,
}

impl OpCodeHandler_B_Ev {
	pub(super) fn new(code32: Code, code64: Code, supports_rip_rel: bool) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_B_Ev::decode, code32, code64, rip_rel_mask: if supports_rip_rel { 0 } else { u32::MAX } }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if decoder.state.reg > 3 || (decoder.state.extra_register_base & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		let base_reg;
		if decoder.is64b_mode {
			instruction.set_code(this.code64);
			base_reg = Register::RAX;
		} else {
			instruction.set_code(this.code32);
			base_reg = Register::EAX;
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(instruction, (decoder.state.reg & 3) + Register::BND0 as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + base_reg as u32);
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem_mpx(instruction);
			// It can't be EIP since if it's MPX + 64-bit mode, the address size is always 64-bit
			if (this.rip_rel_mask & decoder.invalid_check_mask) != 0 && instruction.memory_base() == Register::RIP {
				decoder.set_invalid_instruction();
			}
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Mv_Gv_REXW {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Mv_Gv_REXW {
	pub(super) fn new(code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Mv_Gv_REXW::decode, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code64);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32);
		} else {
			instruction.set_code(this.code32);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32);
		}
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Gv_N_Ib_REX {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Gv_N_Ib_REX {
	pub(super) fn new(code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Gv_N_Ib_REX::decode, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code64);
		} else {
			instruction.set_code(this.code32);
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		if (decoder.state.flags & StateFlags::W) != 0 {
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32);
		} else {
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32);
		}
		instruction.set_op2_kind(OpKind::Immediate8);
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, decoder.state.rm + Register::MM0 as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Gv_N {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Gv_N {
	pub(super) fn new(code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Gv_N::decode, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code64);
		} else {
			instruction.set_code(this.code32);
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		if (decoder.state.flags & StateFlags::W) != 0 {
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32);
		} else {
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32);
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, decoder.state.rm + Register::MM0 as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VN {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg: Register,
}

impl OpCodeHandler_VN {
	pub(super) fn new(base_reg: Register, code: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_VN::decode, base_reg, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, decoder.state.rm + Register::MM0 as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Gv_Mv {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: [Code; 3],
}

impl OpCodeHandler_Gv_Mv {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Gv_Mv::decode, code: [code16, code32, code64] }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code[decoder.state.operand_size as usize]);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		const_assert_eq!(Register::AX as u32 + 16, Register::EAX as u32);
		const_assert_eq!(Register::AX as u32 + 32, Register::RAX as u32);
		write_op0_reg!(
			instruction,
			(decoder.state.operand_size as u32) * 16 + decoder.state.reg + decoder.state.extra_register_base + Register::AX as u32
		);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Mv_Gv {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: [Code; 3],
}

impl OpCodeHandler_Mv_Gv {
	pub(super) fn new(code16: Code, code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Mv_Gv::decode, code: [code16, code32, code64] }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		instruction.set_code(this.code[decoder.state.operand_size as usize]);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		const_assert_eq!(Register::AX as u32 + 16, Register::EAX as u32);
		const_assert_eq!(Register::AX as u32 + 32, Register::RAX as u32);
		write_op1_reg!(
			instruction,
			(decoder.state.operand_size as u32) * 16 + decoder.state.reg + decoder.state.extra_register_base + Register::AX as u32
		);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Gv_Eb_REX {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Gv_Eb_REX {
	pub(super) fn new(code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Gv_Eb_REX::decode, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code64);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32);
		} else {
			instruction.set_code(this.code32);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32);
		}
		if decoder.state.mod_ == 3 {
			let mut index = decoder.state.rm + decoder.state.extra_base_register_base;
			if (decoder.state.flags & StateFlags::HAS_REX) != 0 && index >= 4 {
				index += 4;
			}
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, index + Register::AL as u32);
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Gv_Ev_REX {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Gv_Ev_REX {
	pub(super) fn new(code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Gv_Ev_REX::decode, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code64);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32);
		} else {
			instruction.set_code(this.code32);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32);
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			if (decoder.state.flags & StateFlags::W) != 0 {
				write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::RAX as u32);
			} else {
				write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::EAX as u32);
			}
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Ev_Gv_REX {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Ev_Gv_REX {
	pub(super) fn new(code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Ev_Gv_REX::decode, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		debug_assert_ne!(decoder.state.mod_, 3);
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code64);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32);
		} else {
			instruction.set_code(this.code32);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32);
		}
		instruction.set_op0_kind(OpKind::Memory);
		decoder.read_op_mem(instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_GvM_VX_Ib {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code32: Code,
	code64: Code,
	base_reg: Register,
}

impl OpCodeHandler_GvM_VX_Ib {
	pub(super) fn new(base_reg: Register, code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_GvM_VX_Ib::decode, base_reg, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		write_op1_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32);
		instruction.set_op2_kind(OpKind::Immediate8);
		let gpr;
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code64);
			gpr = Register::RAX;
		} else {
			instruction.set_code(this.code32);
			gpr = Register::EAX;
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr as u32);
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Wbinvd {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
}

impl OpCodeHandler_Wbinvd {
	pub(super) fn new() -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_Wbinvd::decode }
	}

	fn decode(_self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy);
		if (decoder.options & DecoderOptions::NO_WBNOINVD) != 0 || decoder.state.mandatory_prefix != DecoderMandatoryPrefix::PF3 {
			instruction.set_code(Code::Wbinvd);
		} else {
			decoder.clear_mandatory_prefix_f3(instruction);
			instruction.set_code(Code::Wbnoinvd);
		}
	}
}
