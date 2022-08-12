// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#![allow(clippy::never_loop)]
#![allow(clippy::useless_let_if_seq)]

use crate::decoder::enums::*;
use crate::decoder::handlers::*;
use crate::decoder::*;
use crate::iced_constants::IcedConstants;
use crate::instruction_internal;

// SAFETY:
//	code: let this = unsafe { &*(self_ptr as *const Self) };
// The first arg (`self_ptr`) to decode() is always the handler itself, cast to a `*const OpCodeHandler`.
// All handlers are `#[repr(C)]` structs so the OpCodeHandler fields are always at the same offsets.

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VEX2 {
	has_modrm: bool,
	handler_mem: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
}

impl OpCodeHandler_VEX2 {
	#[inline]
	pub(in crate::decoder) fn new(handler_mem: (OpCodeHandlerDecodeFn, &'static OpCodeHandler)) -> (OpCodeHandlerDecodeFn, Self) {
		debug_assert!(!is_null_instance_handler(handler_mem.1));
		(OpCodeHandler_VEX2::decode, Self { has_modrm: true, handler_mem })
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
pub(in crate::decoder) struct OpCodeHandler_VEX3 {
	has_modrm: bool,
	handler_mem: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
}

impl OpCodeHandler_VEX3 {
	#[inline]
	pub(in crate::decoder) fn new(handler_mem: (OpCodeHandlerDecodeFn, &'static OpCodeHandler)) -> (OpCodeHandlerDecodeFn, Self) {
		debug_assert!(!is_null_instance_handler(handler_mem.1));
		(OpCodeHandler_VEX3::decode, Self { has_modrm: true, handler_mem })
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
pub(in crate::decoder) struct OpCodeHandler_XOP {
	has_modrm: bool,
	handler_reg0: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
}

impl OpCodeHandler_XOP {
	#[inline]
	pub(in crate::decoder) fn new(handler_reg0: (OpCodeHandlerDecodeFn, &'static OpCodeHandler)) -> (OpCodeHandlerDecodeFn, Self) {
		debug_assert!(!is_null_instance_handler(handler_reg0.1));
		(OpCodeHandler_XOP::decode, Self { has_modrm: true, handler_reg0 })
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
pub(in crate::decoder) struct OpCodeHandler_EVEX {
	has_modrm: bool,
	handler_mem: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
}

impl OpCodeHandler_EVEX {
	#[inline]
	pub(in crate::decoder) fn new(handler_mem: (OpCodeHandlerDecodeFn, &'static OpCodeHandler)) -> (OpCodeHandlerDecodeFn, Self) {
		debug_assert!(!is_null_instance_handler(handler_mem.1));
		(OpCodeHandler_EVEX::decode, Self { has_modrm: true, handler_mem })
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
pub(in crate::decoder) struct OpCodeHandler_PrefixEsCsSsDs {
	has_modrm: bool,
	seg: Register,
}

impl OpCodeHandler_PrefixEsCsSsDs {
	#[inline]
	pub(in crate::decoder) fn new(seg: Register) -> (OpCodeHandlerDecodeFn, Self) {
		debug_assert!(seg == Register::ES || seg == Register::CS || seg == Register::SS || seg == Register::DS);
		(OpCodeHandler_PrefixEsCsSsDs::decode, Self { has_modrm: false, seg })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);

		if !decoder.is64b_mode || decoder.state.segment_prio == 0 {
			instruction.set_segment_prefix(this.seg);
		}

		decoder.reset_rex_prefix_state();
		decoder.call_opcode_handlers_map0_table(instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_PrefixFsGs {
	has_modrm: bool,
	seg: Register,
}

impl OpCodeHandler_PrefixFsGs {
	#[inline]
	pub(in crate::decoder) fn new(seg: Register) -> (OpCodeHandlerDecodeFn, Self) {
		debug_assert!(seg == Register::FS || seg == Register::GS);
		(OpCodeHandler_PrefixFsGs::decode, Self { has_modrm: false, seg })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);

		instruction.set_segment_prefix(this.seg);
		decoder.state.segment_prio = 1;

		decoder.reset_rex_prefix_state();
		decoder.call_opcode_handlers_map0_table(instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Prefix66 {
	has_modrm: bool,
}

impl OpCodeHandler_Prefix66 {
	#[inline]
	pub(in crate::decoder) fn new() -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Prefix66::decode, Self { has_modrm: false })
	}

	fn decode(_self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);

		decoder.state.flags |= StateFlags::HAS66;
		decoder.state.operand_size = decoder.default_inverted_operand_size;
		if decoder.state.mandatory_prefix == DecoderMandatoryPrefix::PNP {
			decoder.state.mandatory_prefix = DecoderMandatoryPrefix::P66;
		}

		decoder.reset_rex_prefix_state();
		decoder.call_opcode_handlers_map0_table(instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Prefix67 {
	has_modrm: bool,
}

impl OpCodeHandler_Prefix67 {
	#[inline]
	pub(in crate::decoder) fn new() -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Prefix67::decode, Self { has_modrm: false })
	}

	fn decode(_self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);

		decoder.state.address_size = decoder.default_inverted_address_size;

		decoder.reset_rex_prefix_state();
		decoder.call_opcode_handlers_map0_table(instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_PrefixF0 {
	has_modrm: bool,
}

impl OpCodeHandler_PrefixF0 {
	#[inline]
	pub(in crate::decoder) fn new() -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_PrefixF0::decode, Self { has_modrm: false })
	}

	fn decode(_self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);

		instruction.set_has_lock_prefix(true);
		decoder.state.flags |= StateFlags::LOCK;

		decoder.reset_rex_prefix_state();
		decoder.call_opcode_handlers_map0_table(instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_PrefixF2 {
	has_modrm: bool,
}

impl OpCodeHandler_PrefixF2 {
	#[inline]
	pub(in crate::decoder) fn new() -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_PrefixF2::decode, Self { has_modrm: false })
	}

	fn decode(_self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);

		instruction_internal::internal_set_has_repne_prefix(instruction);
		decoder.state.mandatory_prefix = DecoderMandatoryPrefix::PF2;

		decoder.reset_rex_prefix_state();
		decoder.call_opcode_handlers_map0_table(instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_PrefixF3 {
	has_modrm: bool,
}

impl OpCodeHandler_PrefixF3 {
	#[inline]
	pub(in crate::decoder) fn new() -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_PrefixF3::decode, Self { has_modrm: false })
	}

	fn decode(_self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);

		instruction_internal::internal_set_has_repe_prefix(instruction);
		decoder.state.mandatory_prefix = DecoderMandatoryPrefix::PF3;

		decoder.reset_rex_prefix_state();
		decoder.call_opcode_handlers_map0_table(instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_PrefixREX {
	has_modrm: bool,
	rex: u32,
	handler: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
}

impl OpCodeHandler_PrefixREX {
	#[inline]
	pub(in crate::decoder) fn new(handler: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), rex: u32) -> (OpCodeHandlerDecodeFn, Self) {
		debug_assert!(rex <= 0x0F);
		(OpCodeHandler_PrefixREX::decode, Self { has_modrm: false, handler, rex })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);

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

			decoder.call_opcode_handlers_map0_table(instruction);
		} else {
			let (decode, handler) = this.handler;
			(decode)(handler, decoder, instruction)
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Reg {
	has_modrm: bool,
	reg: Register,
	code: Code,
}

impl OpCodeHandler_Reg {
	#[inline]
	pub(in crate::decoder) fn new(code: Code, reg: Register) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Reg::decode, Self { has_modrm: false, code, reg })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		instruction.set_op0_register(this.reg);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_RegIb {
	has_modrm: bool,
	reg: Register,
	code: Code,
}

impl OpCodeHandler_RegIb {
	#[inline]
	pub(in crate::decoder) fn new(code: Code, reg: Register) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_RegIb::decode, Self { has_modrm: false, code, reg })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		instruction.set_op0_register(this.reg);
		instruction.set_op1_kind(OpKind::Immediate8);
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_IbReg {
	has_modrm: bool,
	reg: Register,
	code: Code,
}

impl OpCodeHandler_IbReg {
	#[inline]
	pub(in crate::decoder) fn new(code: Code, reg: Register) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_IbReg::decode, Self { has_modrm: false, code, reg })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		instruction.set_op1_register(this.reg);
		instruction.set_op0_kind(OpKind::Immediate8);
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_AL_DX {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_AL_DX {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_AL_DX::decode, Self { has_modrm: false, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		instruction.set_op0_register(Register::AL);
		instruction.set_op1_register(Register::DX);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_DX_AL {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_DX_AL {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_DX_AL::decode, Self { has_modrm: false, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		instruction.set_op0_register(Register::DX);
		instruction.set_op1_register(Register::AL);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Ib {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_Ib {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Ib::decode, Self { has_modrm: false, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		instruction.set_op0_kind(OpKind::Immediate8);
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Ib3 {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_Ib3 {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Ib3::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		instruction.set_op0_kind(OpKind::Immediate8);
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_MandatoryPrefix {
	has_modrm: bool,
	handlers: [(OpCodeHandlerDecodeFn, &'static OpCodeHandler); 4],
}

impl OpCodeHandler_MandatoryPrefix {
	#[inline]
	pub(in crate::decoder) fn new(
		has_modrm: bool, handler: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), handler_66: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
		handler_f3: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), handler_f2: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	) -> (OpCodeHandlerDecodeFn, Self) {
		const _: () = assert!(DecoderMandatoryPrefix::PNP as u32 == 0);
		const _: () = assert!(DecoderMandatoryPrefix::P66 as u32 == 1);
		const _: () = assert!(DecoderMandatoryPrefix::PF3 as u32 == 2);
		const _: () = assert!(DecoderMandatoryPrefix::PF2 as u32 == 3);
		debug_assert!(!is_null_instance_handler(handler.1));
		debug_assert!(!is_null_instance_handler(handler_66.1));
		debug_assert!(!is_null_instance_handler(handler_f3.1));
		debug_assert!(!is_null_instance_handler(handler_f2.1));
		let handlers = [handler, handler_66, handler_f3, handler_f2];
		debug_assert_eq!(handlers[0].1.has_modrm, has_modrm);
		debug_assert_eq!(handlers[1].1.has_modrm, has_modrm);
		debug_assert_eq!(handlers[2].1.has_modrm, has_modrm);
		debug_assert_eq!(handlers[3].1.has_modrm, has_modrm);
		(OpCodeHandler_MandatoryPrefix::decode, Self { has_modrm, handlers })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		decoder.clear_mandatory_prefix(instruction);
		let (decode, handler) = this.handlers[decoder.state.mandatory_prefix as usize];
		(decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_MandatoryPrefix3 {
	has_modrm: bool,
	handlers_reg: [(OpCodeHandlerDecodeFn, &'static OpCodeHandler, bool); 4],
	handlers_mem: [(OpCodeHandlerDecodeFn, &'static OpCodeHandler, bool); 4],
}

impl OpCodeHandler_MandatoryPrefix3 {
	#[allow(clippy::too_many_arguments)]
	#[inline]
	pub(in crate::decoder) fn new(
		handler_reg: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), handler_mem: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
		handler66_reg: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), handler66_mem: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
		handlerf3_reg: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), handlerf3_mem: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
		handlerf2_reg: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), handlerf2_mem: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), flags: u32,
	) -> (OpCodeHandlerDecodeFn, Self) {
		debug_assert!(!is_null_instance_handler(handler_reg.1));
		debug_assert!(!is_null_instance_handler(handler_mem.1));
		debug_assert!(!is_null_instance_handler(handler66_reg.1));
		debug_assert!(!is_null_instance_handler(handler66_mem.1));
		debug_assert!(!is_null_instance_handler(handlerf3_reg.1));
		debug_assert!(!is_null_instance_handler(handlerf3_mem.1));
		debug_assert!(!is_null_instance_handler(handlerf2_reg.1));
		debug_assert!(!is_null_instance_handler(handlerf2_mem.1));
		let handlers_reg = [
			(handler_reg.0, handler_reg.1, (flags & LegacyHandlerFlags::HANDLER_REG) == 0),
			(handler66_reg.0, handler66_reg.1, (flags & LegacyHandlerFlags::HANDLER_66_REG) == 0),
			(handlerf3_reg.0, handlerf3_reg.1, (flags & LegacyHandlerFlags::HANDLER_F3_REG) == 0),
			(handlerf2_reg.0, handlerf2_reg.1, (flags & LegacyHandlerFlags::HANDLER_F2_REG) == 0),
		];
		let handlers_mem = [
			(handler_mem.0, handler_mem.1, (flags & LegacyHandlerFlags::HANDLER_MEM) == 0),
			(handler66_mem.0, handler66_mem.1, (flags & LegacyHandlerFlags::HANDLER_66_MEM) == 0),
			(handlerf3_mem.0, handlerf3_mem.1, (flags & LegacyHandlerFlags::HANDLER_F3_MEM) == 0),
			(handlerf2_mem.0, handlerf2_mem.1, (flags & LegacyHandlerFlags::HANDLER_F2_MEM) == 0),
		];
		debug_assert!(handlers_reg[0].1.has_modrm);
		debug_assert!(handlers_reg[1].1.has_modrm);
		debug_assert!(handlers_reg[2].1.has_modrm);
		debug_assert!(handlers_reg[3].1.has_modrm);
		debug_assert!(handlers_mem[0].1.has_modrm);
		debug_assert!(handlers_mem[1].1.has_modrm);
		debug_assert!(handlers_mem[2].1.has_modrm);
		debug_assert!(handlers_mem[3].1.has_modrm);
		(OpCodeHandler_MandatoryPrefix3::decode, Self { has_modrm: true, handlers_reg, handlers_mem })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		let (decode, handler, mandatory_prefix) = if decoder.state.mod_ == 3 {
			this.handlers_reg[decoder.state.mandatory_prefix as usize]
		} else {
			this.handlers_mem[decoder.state.mandatory_prefix as usize]
		};
		if mandatory_prefix {
			decoder.clear_mandatory_prefix(instruction);
		}
		(decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_MandatoryPrefix4 {
	has_modrm: bool,
	flags: u32,
	handler_np: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	handler_66: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	handler_f3: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	handler_f2: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
}

impl OpCodeHandler_MandatoryPrefix4 {
	#[inline]
	pub(in crate::decoder) fn new(
		handler_np: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), handler_66: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
		handler_f3: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), handler_f2: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), flags: u32,
	) -> (OpCodeHandlerDecodeFn, Self) {
		debug_assert!(!is_null_instance_handler(handler_np.1));
		debug_assert!(!is_null_instance_handler(handler_66.1));
		debug_assert!(!is_null_instance_handler(handler_f3.1));
		debug_assert!(!is_null_instance_handler(handler_f2.1));
		(OpCodeHandler_MandatoryPrefix4::decode, Self { has_modrm: false, handler_np, handler_66, handler_f3, handler_f2, flags })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		const _: () = assert!(DecoderMandatoryPrefix::PNP as u32 == 0);
		const _: () = assert!(DecoderMandatoryPrefix::P66 as u32 == 1);
		const _: () = assert!(DecoderMandatoryPrefix::PF3 as u32 == 2);
		const _: () = assert!(DecoderMandatoryPrefix::PF2 as u32 == 3);
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
pub(in crate::decoder) struct OpCodeHandler_NIb {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_NIb {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_NIb::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		instruction.set_op1_kind(OpKind::Immediate8);
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
		if decoder.state.mod_ == 3 {
			write_op0_reg!(instruction, decoder.state.rm + Register::MM0 as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Reservednop {
	has_modrm: bool,
	reserved_nop_handler: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	other_handler: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
}

impl OpCodeHandler_Reservednop {
	#[inline]
	pub(in crate::decoder) fn new(
		reserved_nop_handler: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), other_handler: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	) -> (OpCodeHandlerDecodeFn, Self) {
		debug_assert!(!is_null_instance_handler(reserved_nop_handler.1));
		debug_assert!(!is_null_instance_handler(other_handler.1));
		(OpCodeHandler_Reservednop::decode, Self { has_modrm: true, reserved_nop_handler, other_handler })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		let (decode, handler) =
			if (decoder.options & DecoderOptions::FORCE_RESERVED_NOP) != 0 { this.reserved_nop_handler } else { this.other_handler };
		(decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Ev_Iz {
	has_modrm: bool,
	code: [Code; 3],
	reg_base: [u32; 3],
	state_flags_or_value: u32,
}

impl OpCodeHandler_Ev_Iz {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code, flags: u32) -> (OpCodeHandlerDecodeFn, Self) {
		const _: () = assert!(HandlerFlags::LOCK == 1 << 3);
		const _: () = assert!(StateFlags::ALLOW_LOCK == 1 << 13);
		let state_flags_or_value = (flags & HandlerFlags::LOCK) << (13 - 3);

		(
			OpCodeHandler_Ev_Iz::decode,
			Self {
				has_modrm: true,
				reg_base: [Register::AX as u32, Register::EAX as u32, Register::RAX as u32],
				code: [code16, code32, code64],
				state_flags_or_value,
			},
		)
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		let operand_size = decoder.state.operand_size;
		instruction.set_code(this.code[operand_size as usize]);
		if decoder.state.mod_ < 3 {
			read_op_mem_stmt!(decoder, instruction, {
				decoder.state.flags |= this.state_flags_or_value;
				instruction.set_op0_kind(OpKind::Memory);
			});
		} else {
			let reg_base = this.reg_base[operand_size as usize];
			write_op0_reg!(instruction, reg_base + decoder.state.rm + decoder.state.extra_base_register_base);
		}
		if operand_size == OpSize::Size32 {
			instruction.set_op1_kind(OpKind::Immediate32);
			instruction.set_immediate32(decoder.read_u32() as u32);
		} else if operand_size == OpSize::Size64 {
			instruction.set_op1_kind(OpKind::Immediate32to64);
			instruction.set_immediate32(decoder.read_u32() as u32);
		} else {
			instruction.set_op1_kind(OpKind::Immediate16);
			instruction_internal::internal_set_immediate16(instruction, decoder.read_u16() as u32);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Ev_Ib {
	has_modrm: bool,
	op_kinds: [OpKind; 3],
	reg_base: [u32; 3],
	state_flags_or_value: u32,
	code: [Code; 3],
}

impl OpCodeHandler_Ev_Ib {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code, flags: u32) -> (OpCodeHandlerDecodeFn, Self) {
		const _: () = assert!(HandlerFlags::LOCK == 1 << 3);
		const _: () = assert!(StateFlags::ALLOW_LOCK == 1 << 13);
		let state_flags_or_value = (flags & HandlerFlags::LOCK) << (13 - 3);

		(
			OpCodeHandler_Ev_Ib::decode,
			Self {
				has_modrm: true,
				reg_base: [Register::AX as u32, Register::EAX as u32, Register::RAX as u32],
				code: [code16, code32, code64],
				state_flags_or_value,
				op_kinds: [OpKind::Immediate8to16, OpKind::Immediate8to32, OpKind::Immediate8to64],
			},
		)
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		let operand_size = decoder.state.operand_size;
		instruction.set_code(this.code[operand_size as usize]);
		instruction.set_op1_kind(this.op_kinds[operand_size as usize]);
		if decoder.state.mod_ == 3 {
			let reg_base = this.reg_base[operand_size as usize];
			write_op0_reg!(instruction, reg_base + decoder.state.rm + decoder.state.extra_base_register_base);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				decoder.state.flags |= this.state_flags_or_value;
				instruction.set_op0_kind(OpKind::Memory);
			});
		}
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Ev_Ib2 {
	has_modrm: bool,
	code: [Code; 3],
	reg_base: [u32; 3],
	state_flags_or_value: u32,
}

impl OpCodeHandler_Ev_Ib2 {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code, flags: u32) -> (OpCodeHandlerDecodeFn, Self) {
		const _: () = assert!(HandlerFlags::LOCK == 1 << 3);
		const _: () = assert!(StateFlags::ALLOW_LOCK == 1 << 13);
		let state_flags_or_value = (flags & HandlerFlags::LOCK) << (13 - 3);

		(
			OpCodeHandler_Ev_Ib2::decode,
			Self {
				has_modrm: true,
				reg_base: [Register::AX as u32, Register::EAX as u32, Register::RAX as u32],
				code: [code16, code32, code64],
				state_flags_or_value,
			},
		)
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_op1_kind(OpKind::Immediate8);
		let operand_size = decoder.state.operand_size;
		instruction.set_code(this.code[operand_size as usize]);
		if decoder.state.mod_ == 3 {
			let reg_base = this.reg_base[operand_size as usize];
			write_op0_reg!(instruction, reg_base + decoder.state.rm + decoder.state.extra_base_register_base);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				decoder.state.flags |= this.state_flags_or_value;
				instruction.set_op0_kind(OpKind::Memory);
			});
		}
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Ev_1 {
	has_modrm: bool,
	code: [Code; 3],
	reg_base: [u32; 3],
}

impl OpCodeHandler_Ev_1 {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(
			OpCodeHandler_Ev_1::decode,
			Self { has_modrm: true, reg_base: [Register::AX as u32, Register::EAX as u32, Register::RAX as u32], code: [code16, code32, code64] },
		)
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		let operand_size = decoder.state.operand_size;
		instruction.set_code(this.code[operand_size as usize]);
		instruction.set_op1_kind(OpKind::Immediate8);
		instruction_internal::internal_set_immediate8(instruction, 1);
		decoder.state.flags |= StateFlags::NO_IMM;
		if decoder.state.mod_ == 3 {
			let reg_base = this.reg_base[operand_size as usize];
			write_op0_reg!(instruction, reg_base + decoder.state.rm + decoder.state.extra_base_register_base);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op0_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Ev_CL {
	has_modrm: bool,
	code: [Code; 3],
	reg_base: [u32; 3],
}

impl OpCodeHandler_Ev_CL {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(
			OpCodeHandler_Ev_CL::decode,
			Self { has_modrm: true, reg_base: [Register::AX as u32, Register::EAX as u32, Register::RAX as u32], code: [code16, code32, code64] },
		)
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		let operand_size = decoder.state.operand_size;
		instruction.set_code(this.code[operand_size as usize]);
		instruction.set_op1_register(Register::CL);
		if decoder.state.mod_ == 3 {
			let reg_base = this.reg_base[operand_size as usize];
			write_op0_reg!(instruction, reg_base + decoder.state.rm + decoder.state.extra_base_register_base);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op0_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Ev {
	has_modrm: bool,
	code: [Code; 3],
	reg_base: [u32; 3],
	state_flags_or_value: u32,
}

impl OpCodeHandler_Ev {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code, flags: u32) -> (OpCodeHandlerDecodeFn, Self) {
		const _: () = assert!(HandlerFlags::LOCK == 1 << 3);
		const _: () = assert!(StateFlags::ALLOW_LOCK == 1 << 13);
		let state_flags_or_value = (flags & HandlerFlags::LOCK) << (13 - 3);

		(
			OpCodeHandler_Ev::decode,
			Self {
				has_modrm: true,
				reg_base: [Register::AX as u32, Register::EAX as u32, Register::RAX as u32],
				code: [code16, code32, code64],
				state_flags_or_value,
			},
		)
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		let operand_size = decoder.state.operand_size;
		instruction.set_code(this.code[operand_size as usize]);
		if decoder.state.mod_ == 3 {
			let reg_base = this.reg_base[operand_size as usize];
			write_op0_reg!(instruction, reg_base + decoder.state.rm + decoder.state.extra_base_register_base);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				decoder.state.flags |= this.state_flags_or_value;
				instruction.set_op0_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Rv {
	has_modrm: bool,
	code: [Code; 3],
	reg_base: [u32; 3],
}

impl OpCodeHandler_Rv {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(
			OpCodeHandler_Rv::decode,
			Self { has_modrm: true, reg_base: [Register::AX as u32, Register::EAX as u32, Register::RAX as u32], code: [code16, code32, code64] },
		)
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		debug_assert_eq!(decoder.state.mod_, 3);
		let operand_size = decoder.state.operand_size;
		instruction.set_code(this.code[operand_size as usize]);
		let reg_base = this.reg_base[operand_size as usize];
		write_op0_reg!(instruction, reg_base + decoder.state.rm + decoder.state.extra_base_register_base);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Rv_32_64 {
	has_modrm: bool,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Rv_32_64 {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Rv_32_64::decode, Self { has_modrm: true, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		if decoder.is64b_mode {
			instruction.set_code(this.code64);
			debug_assert_eq!(decoder.state.mod_, 3);
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::RAX as u32);
		} else {
			instruction.set_code(this.code32);
			debug_assert_eq!(decoder.state.mod_, 3);
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::EAX as u32);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Rq {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_Rq {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Rq::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		debug_assert_eq!(decoder.state.mod_, 3);
		write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::RAX as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Ev_REXW {
	has_modrm: bool,
	flags: u32,
	disallow_reg: u32,
	disallow_mem: u32,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Ev_REXW {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code, flags: u32) -> (OpCodeHandlerDecodeFn, Self) {
		(
			OpCodeHandler_Ev_REXW::decode,
			Self {
				has_modrm: true,
				code32,
				code64,
				flags,
				disallow_reg: if (flags & 1) != 0 { 0 } else { u32::MAX },
				disallow_mem: if (flags & 2) != 0 { 0 } else { u32::MAX },
			},
		)
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code64);
		} else {
			instruction.set_code(this.code32);
		}
		const _: () = assert!(StateFlags::HAS66 != 4);
		if (((this.flags & 4) | (decoder.state.flags & StateFlags::HAS66)) & decoder.invalid_check_mask) == (4 | StateFlags::HAS66) {
			decoder.set_invalid_instruction();
		}
		if decoder.state.mod_ == 3 {
			if (decoder.state.flags & StateFlags::W) != 0 {
				write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::RAX as u32);
			} else {
				write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::EAX as u32);
			}
			if (this.disallow_reg & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				if (this.disallow_mem & decoder.invalid_check_mask) != 0 {
					decoder.set_invalid_instruction();
				}
				instruction.set_op0_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Evj {
	has_modrm: bool,
	code16: Code,
	code32: Code,
	code64: Code,
	reg_base: [u32; 3],
}

impl OpCodeHandler_Evj {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(
			OpCodeHandler_Evj::decode,
			Self { has_modrm: true, reg_base: [Register::AX as u32, Register::EAX as u32, Register::RAX as u32], code16, code32, code64 },
		)
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		if decoder.is64b_mode {
			if (((decoder.options ^ DecoderOptions::AMD) & DecoderOptions::AMD) | (decoder.state.operand_size as u32 - OpSize::Size16 as u32)) != 0 {
				instruction.set_code(this.code64);
			} else {
				instruction.set_code(this.code16);
			}
			if decoder.state.mod_ < 3 {
				read_op_mem_stmt!(decoder, instruction, {
					instruction.set_op0_kind(OpKind::Memory);
				});
			} else {
				if (((decoder.options ^ DecoderOptions::AMD) & DecoderOptions::AMD) | (decoder.state.operand_size as u32 - OpSize::Size16 as u32))
					!= 0
				{
					write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::RAX as u32);
				} else {
					write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::AX as u32);
				}
			}
		} else {
			let operand_size = decoder.state.operand_size;
			if operand_size == OpSize::Size32 {
				instruction.set_code(this.code32);
			} else {
				instruction.set_code(this.code16);
			}
			if decoder.state.mod_ < 3 {
				read_op_mem_stmt!(decoder, instruction, {
					instruction.set_op0_kind(OpKind::Memory);
				});
			} else {
				let reg_base = this.reg_base[operand_size as usize];
				write_op0_reg!(instruction, reg_base + decoder.state.rm + decoder.state.extra_base_register_base);
			}
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Ep {
	has_modrm: bool,
	code16: Code,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Ep {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Ep::decode, Self { has_modrm: true, code16, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
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
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op0_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Evw {
	has_modrm: bool,
	code: [Code; 3],
	reg_base: [u32; 3],
}

impl OpCodeHandler_Evw {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(
			OpCodeHandler_Evw::decode,
			Self { has_modrm: true, reg_base: [Register::AX as u32, Register::EAX as u32, Register::RAX as u32], code: [code16, code32, code64] },
		)
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		let operand_size = decoder.state.operand_size;
		instruction.set_code(this.code[operand_size as usize]);
		if decoder.state.mod_ == 3 {
			let reg_base = this.reg_base[operand_size as usize];
			write_op0_reg!(instruction, reg_base + decoder.state.rm + decoder.state.extra_base_register_base);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op0_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Ew {
	has_modrm: bool,
	code: [Code; 3],
	reg_base: [u32; 3],
}

impl OpCodeHandler_Ew {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(
			OpCodeHandler_Ew::decode,
			Self { has_modrm: true, reg_base: [Register::AX as u32, Register::EAX as u32, Register::RAX as u32], code: [code16, code32, code64] },
		)
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		let operand_size = decoder.state.operand_size;
		instruction.set_code(this.code[operand_size as usize]);
		if decoder.state.mod_ == 3 {
			let reg_base = this.reg_base[operand_size as usize];
			write_op0_reg!(instruction, reg_base + decoder.state.rm + decoder.state.extra_base_register_base);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op0_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Ms {
	has_modrm: bool,
	code16: Code,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Ms {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Ms::decode, Self { has_modrm: true, code16, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		if decoder.state.mod_ < 3 {
			read_op_mem_stmt!(decoder, instruction, {
				if decoder.is64b_mode {
					instruction.set_code(this.code64);
				} else if decoder.state.operand_size == OpSize::Size32 {
					instruction.set_code(this.code32);
				} else {
					instruction.set_code(this.code16);
				}
				instruction.set_op0_kind(OpKind::Memory);
			});
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Gv_Ev {
	has_modrm: bool,
	code: [Code; 3],
	reg_base: [u32; 3],
}

impl OpCodeHandler_Gv_Ev {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(
			OpCodeHandler_Gv_Ev::decode,
			Self { has_modrm: true, reg_base: [Register::AX as u32, Register::EAX as u32, Register::RAX as u32], code: [code16, code32, code64] },
		)
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		let operand_size = decoder.state.operand_size;
		instruction.set_code(this.code[operand_size as usize]);
		let reg_base = this.reg_base[operand_size as usize];
		write_op0_reg!(instruction, reg_base + decoder.state.reg + decoder.state.extra_register_base);
		if decoder.state.mod_ < 3 {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op1_kind(OpKind::Memory);
			});
		} else {
			write_op1_reg!(instruction, reg_base + decoder.state.rm + decoder.state.extra_base_register_base);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Gd_Rd {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_Gd_Rd {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Gd_Rd::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::EAX as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Gv_M_as {
	has_modrm: bool,
	code: [Code; 3],
	reg_base: [u32; 3],
}

impl OpCodeHandler_Gv_M_as {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(
			OpCodeHandler_Gv_M_as::decode,
			Self { has_modrm: true, reg_base: [Register::AX as u32, Register::EAX as u32, Register::RAX as u32], code: [code16, code32, code64] },
		)
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		let address_size = decoder.state.address_size;
		instruction.set_code(this.code[address_size as usize]);
		let reg_base = this.reg_base[address_size as usize];
		write_op0_reg!(instruction, reg_base + decoder.state.reg + decoder.state.extra_register_base);
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
pub(in crate::decoder) struct OpCodeHandler_Gdq_Ev {
	has_modrm: bool,
	code: [Code; 3],
	reg_base: [u32; 3],
}

impl OpCodeHandler_Gdq_Ev {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(
			OpCodeHandler_Gdq_Ev::decode,
			Self { has_modrm: true, reg_base: [Register::AX as u32, Register::EAX as u32, Register::RAX as u32], code: [code16, code32, code64] },
		)
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		let operand_size = decoder.state.operand_size;
		instruction.set_code(this.code[operand_size as usize]);
		if operand_size != OpSize::Size64 {
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32);
		} else {
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32);
		}
		if decoder.state.mod_ == 3 {
			let reg_base = this.reg_base[operand_size as usize];
			write_op1_reg!(instruction, reg_base + decoder.state.rm + decoder.state.extra_base_register_base);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op1_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Gv_Ev3 {
	has_modrm: bool,
	code: [Code; 3],
	reg_base: [u32; 3],
}

impl OpCodeHandler_Gv_Ev3 {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(
			OpCodeHandler_Gv_Ev3::decode,
			Self { has_modrm: true, reg_base: [Register::AX as u32, Register::EAX as u32, Register::RAX as u32], code: [code16, code32, code64] },
		)
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		let operand_size = decoder.state.operand_size;
		instruction.set_code(this.code[operand_size as usize]);
		let reg_base = this.reg_base[operand_size as usize];
		write_op0_reg!(instruction, reg_base + decoder.state.reg + decoder.state.extra_register_base);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, reg_base + decoder.state.rm + decoder.state.extra_base_register_base);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op1_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Gv_Ev2 {
	has_modrm: bool,
	code: [Code; 3],
	reg_base: [u32; 3],
}

impl OpCodeHandler_Gv_Ev2 {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(
			OpCodeHandler_Gv_Ev2::decode,
			Self { has_modrm: true, reg_base: [Register::AX as u32, Register::EAX as u32, Register::RAX as u32], code: [code16, code32, code64] },
		)
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		let operand_size = decoder.state.operand_size;
		instruction.set_code(this.code[operand_size as usize]);
		let reg_base = this.reg_base[operand_size as usize];
		write_op0_reg!(instruction, reg_base + decoder.state.reg + decoder.state.extra_register_base);
		if decoder.state.mod_ == 3 {
			let index = decoder.state.rm + decoder.state.extra_base_register_base;
			if operand_size != OpSize::Size16 {
				write_op1_reg!(instruction, index + Register::EAX as u32);
			} else {
				write_op1_reg!(instruction, index + Register::AX as u32);
			}
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op1_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_R_C {
	has_modrm: bool,
	base_reg: Register,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_R_C {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code, base_reg: Register) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_R_C::decode, Self { has_modrm: true, code32, code64, base_reg })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		if decoder.is64b_mode {
			instruction.set_code(this.code64);
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::RAX as u32);
		} else {
			instruction.set_code(this.code32);
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::EAX as u32);
		}
		let mut extra_register_base = decoder.state.extra_register_base;
		// LOCK MOV CR0 is supported by some AMD CPUs
		if this.base_reg == Register::CR0 && instruction.has_lock_prefix() && (decoder.options & DecoderOptions::AMD) != 0 {
			if (extra_register_base & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
			extra_register_base = 8;
			instruction.set_has_lock_prefix(false);
			decoder.state.flags &= !StateFlags::LOCK;
		}
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
				debug_assert!(!decoder.is64b_mode);
				debug_assert_eq!(this.base_reg, Register::TR0);
			}
		}
		write_op1_reg!(instruction, reg + this.base_reg as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_C_R {
	has_modrm: bool,
	base_reg: Register,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_C_R {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code, base_reg: Register) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_C_R::decode, Self { has_modrm: true, code32, code64, base_reg })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		if decoder.is64b_mode {
			instruction.set_code(this.code64);
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::RAX as u32);
		} else {
			instruction.set_code(this.code32);
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::EAX as u32);
		}
		let mut extra_register_base = decoder.state.extra_register_base;
		// LOCK MOV CR0 is supported by some AMD CPUs
		if this.base_reg == Register::CR0 && instruction.has_lock_prefix() && (decoder.options & DecoderOptions::AMD) != 0 {
			if (extra_register_base & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
			extra_register_base = 8;
			instruction.set_has_lock_prefix(false);
			decoder.state.flags &= !StateFlags::LOCK;
		}
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
				debug_assert!(!decoder.is64b_mode);
				debug_assert_eq!(this.base_reg, Register::TR0);
			}
		}
		write_op0_reg!(instruction, reg + this.base_reg as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Jb {
	has_modrm: bool,
	code16: Code,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Jb {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Jb::decode, Self { has_modrm: false, code16, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		decoder.state.flags |= StateFlags::BRANCH_IMM8;
		let b = decoder.read_u8() as i8 as u64;
		if decoder.is64b_mode {
			if (((decoder.options ^ DecoderOptions::AMD) & DecoderOptions::AMD) | (decoder.state.operand_size as u32 - OpSize::Size16 as u32)) != 0 {
				instruction.set_near_branch64(b.wrapping_add(decoder.current_ip64()));
				instruction.set_code(this.code64);
				instruction.set_op0_kind(OpKind::NearBranch64);
			} else {
				instruction_internal::internal_set_near_branch16(instruction, (b as u32).wrapping_add(decoder.current_ip32()) as u16 as u32);
				instruction.set_code(this.code16);
				instruction.set_op0_kind(OpKind::NearBranch16);
			}
		} else {
			if decoder.state.operand_size != OpSize::Size16 {
				instruction.set_near_branch32((b as u32).wrapping_add(decoder.current_ip32()));
				instruction.set_code(this.code32);
				instruction.set_op0_kind(OpKind::NearBranch32);
			} else {
				instruction_internal::internal_set_near_branch16(instruction, (b as u32).wrapping_add(decoder.current_ip32()) as u16 as u32);
				instruction.set_code(this.code16);
				instruction.set_op0_kind(OpKind::NearBranch16);
			}
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Jx {
	has_modrm: bool,
	code16: Code,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Jx {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Jx::decode, Self { has_modrm: false, code16, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
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
pub(in crate::decoder) struct OpCodeHandler_Jz {
	has_modrm: bool,
	code16: Code,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Jz {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Jz::decode, Self { has_modrm: false, code16, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		if decoder.is64b_mode {
			if (((decoder.options ^ DecoderOptions::AMD) & DecoderOptions::AMD) | (decoder.state.operand_size as u32 - OpSize::Size16 as u32)) != 0 {
				loop {
					instruction.set_code(this.code64);
					instruction.set_op0_kind(OpKind::NearBranch64);
					instruction.set_near_branch64((read_u32_break!(decoder) as i32 as u64).wrapping_add(decoder.current_ip64()));
					return;
				}
				decoder.state.flags |= StateFlags::IS_INVALID | StateFlags::NO_MORE_BYTES;
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
pub(in crate::decoder) struct OpCodeHandler_Jb2 {
	has_modrm: bool,
	code16_16: Code,
	code16_32: Code,
	code16_64: Code,
	code32_16: Code,
	code32_32: Code,
	code64_32: Code,
	code64_64: Code,
}

impl OpCodeHandler_Jb2 {
	#[inline]
	pub(in crate::decoder) fn new(
		code16_16: Code, code16_32: Code, code16_64: Code, code32_16: Code, code32_32: Code, code64_32: Code, code64_64: Code,
	) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Jb2::decode, Self { has_modrm: false, code16_16, code16_32, code16_64, code32_16, code32_32, code64_32, code64_64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		decoder.state.flags |= StateFlags::BRANCH_IMM8;
		let b = decoder.read_u8();
		if decoder.is64b_mode {
			if (((decoder.options ^ DecoderOptions::AMD) & DecoderOptions::AMD) | (decoder.state.operand_size as u32 - OpSize::Size16 as u32)) != 0 {
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
pub(in crate::decoder) struct OpCodeHandler_Jdisp {
	has_modrm: bool,
	code16: Code,
	code32: Code,
}

impl OpCodeHandler_Jdisp {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Jdisp::decode, Self { has_modrm: false, code16, code32 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
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
pub(in crate::decoder) struct OpCodeHandler_PushOpSizeReg {
	has_modrm: bool,
	reg: Register,
	code16: Code,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_PushOpSizeReg {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code, reg: Register) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_PushOpSizeReg::decode, Self { has_modrm: false, code16, code32, code64, reg })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
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
pub(in crate::decoder) struct OpCodeHandler_PushEv {
	has_modrm: bool,
	code16: Code,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_PushEv {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_PushEv::decode, Self { has_modrm: true, code16, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
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
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op0_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Ev_Gv {
	has_modrm: bool,
	code: [Code; 3],
	reg_base: [u32; 3],
}

impl OpCodeHandler_Ev_Gv {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(
			OpCodeHandler_Ev_Gv::decode,
			Self { has_modrm: true, reg_base: [Register::AX as u32, Register::EAX as u32, Register::RAX as u32], code: [code16, code32, code64] },
		)
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		let operand_size = decoder.state.operand_size;
		instruction.set_code(this.code[operand_size as usize]);
		let reg_base = this.reg_base[operand_size as usize];
		write_op1_reg!(instruction, reg_base + decoder.state.reg + decoder.state.extra_register_base);
		if decoder.state.mod_ == 3 {
			write_op0_reg!(instruction, reg_base + decoder.state.rm + decoder.state.extra_base_register_base);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op0_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Ev_Gv_flags {
	has_modrm: bool,
	code: [Code; 3],
	reg_base: [u32; 3],
	state_flags_or_value: u32,
}

impl OpCodeHandler_Ev_Gv_flags {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code, flags: u32) -> (OpCodeHandlerDecodeFn, Self) {
		debug_assert!((flags & (HandlerFlags::XACQUIRE | HandlerFlags::XRELEASE)) != 0);

		const _: () = assert!(HandlerFlags::LOCK == 1 << 3);
		const _: () = assert!(StateFlags::ALLOW_LOCK == 1 << 13);
		let state_flags_or_value = (flags & HandlerFlags::LOCK) << (13 - 3);

		(
			OpCodeHandler_Ev_Gv_flags::decode,
			Self {
				has_modrm: true,
				reg_base: [Register::AX as u32, Register::EAX as u32, Register::RAX as u32],
				code: [code16, code32, code64],
				state_flags_or_value,
			},
		)
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		let operand_size = decoder.state.operand_size;
		instruction.set_code(this.code[operand_size as usize]);
		let reg_base = this.reg_base[operand_size as usize];
		write_op1_reg!(instruction, reg_base + decoder.state.reg + decoder.state.extra_register_base);
		if decoder.state.mod_ == 3 {
			write_op0_reg!(instruction, reg_base + decoder.state.rm + decoder.state.extra_base_register_base);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				decoder.state.flags |= this.state_flags_or_value;
				instruction.set_op0_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Ev_Gv_32_64 {
	has_modrm: bool,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Ev_Gv_32_64 {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Ev_Gv_32_64::decode, Self { has_modrm: true, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		let base_reg;
		if decoder.is64b_mode {
			instruction.set_code(this.code64);
			base_reg = Register::RAX;
		} else {
			instruction.set_code(this.code32);
			base_reg = Register::EAX;
		}
		write_op1_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + base_reg as u32);
		if decoder.state.mod_ == 3 {
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + base_reg as u32);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op0_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Ev_Gv_Ib {
	has_modrm: bool,
	code: [Code; 3],
	reg_base: [u32; 3],
}

impl OpCodeHandler_Ev_Gv_Ib {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(
			OpCodeHandler_Ev_Gv_Ib::decode,
			Self { has_modrm: true, reg_base: [Register::AX as u32, Register::EAX as u32, Register::RAX as u32], code: [code16, code32, code64] },
		)
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		let operand_size = decoder.state.operand_size;
		instruction.set_code(this.code[operand_size as usize]);
		let reg_base = this.reg_base[operand_size as usize];
		write_op1_reg!(instruction, reg_base + decoder.state.reg + decoder.state.extra_register_base);
		instruction.set_op2_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			write_op0_reg!(instruction, reg_base + decoder.state.rm + decoder.state.extra_base_register_base);
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
pub(in crate::decoder) struct OpCodeHandler_Ev_Gv_CL {
	has_modrm: bool,
	code: [Code; 3],
	reg_base: [u32; 3],
}

impl OpCodeHandler_Ev_Gv_CL {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(
			OpCodeHandler_Ev_Gv_CL::decode,
			Self { has_modrm: true, reg_base: [Register::AX as u32, Register::EAX as u32, Register::RAX as u32], code: [code16, code32, code64] },
		)
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		let operand_size = decoder.state.operand_size;
		instruction.set_code(this.code[operand_size as usize]);
		let reg_base = this.reg_base[operand_size as usize];
		write_op1_reg!(instruction, reg_base + decoder.state.reg + decoder.state.extra_register_base);
		instruction.set_op2_register(Register::CL);
		if decoder.state.mod_ == 3 {
			write_op0_reg!(instruction, reg_base + decoder.state.rm + decoder.state.extra_base_register_base);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op0_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Gv_Mp {
	has_modrm: bool,
	code16: Code,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Gv_Mp {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Gv_Mp::decode, Self { has_modrm: true, code16, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		if decoder.state.operand_size == OpSize::Size64 && (decoder.options & DecoderOptions::AMD) == 0 {
			instruction.set_code(this.code64);
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32);
		} else if decoder.state.operand_size == OpSize::Size16 {
			instruction.set_code(this.code16);
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::AX as u32);
		} else {
			instruction.set_code(this.code32);
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32);
		}
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
pub(in crate::decoder) struct OpCodeHandler_Gv_Eb {
	has_modrm: bool,
	code: [Code; 3],
	reg_base: [u32; 3],
}

impl OpCodeHandler_Gv_Eb {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(
			OpCodeHandler_Gv_Eb::decode,
			Self { has_modrm: true, reg_base: [Register::AX as u32, Register::EAX as u32, Register::RAX as u32], code: [code16, code32, code64] },
		)
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		let operand_size = decoder.state.operand_size;
		instruction.set_code(this.code[operand_size as usize]);
		let reg_base = this.reg_base[operand_size as usize];
		write_op0_reg!(instruction, reg_base + decoder.state.reg + decoder.state.extra_register_base);
		if decoder.state.mod_ < 3 {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op1_kind(OpKind::Memory);
			});
		} else {
			let mut index = decoder.state.rm + decoder.state.extra_base_register_base;
			if (decoder.state.flags & StateFlags::HAS_REX) != 0 && index >= 4 {
				index += 4;
			}
			write_op1_reg!(instruction, index + Register::AL as u32);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Gv_Ew {
	has_modrm: bool,
	code: [Code; 3],
	reg_base: [u32; 3],
}

impl OpCodeHandler_Gv_Ew {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(
			OpCodeHandler_Gv_Ew::decode,
			Self { has_modrm: true, reg_base: [Register::AX as u32, Register::EAX as u32, Register::RAX as u32], code: [code16, code32, code64] },
		)
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		let operand_size = decoder.state.operand_size;
		instruction.set_code(this.code[operand_size as usize]);
		let reg_base = this.reg_base[operand_size as usize];
		write_op0_reg!(instruction, reg_base + decoder.state.reg + decoder.state.extra_register_base);
		if decoder.state.mod_ < 3 {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op1_kind(OpKind::Memory);
			});
		} else {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::AX as u32);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_PushSimple2 {
	has_modrm: bool,
	code16: Code,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_PushSimple2 {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_PushSimple2::decode, Self { has_modrm: false, code16, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
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
pub(in crate::decoder) struct OpCodeHandler_Simple2 {
	has_modrm: bool,
	code: [Code; 3],
}

impl OpCodeHandler_Simple2 {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Simple2::decode, Self { has_modrm: false, code: [code16, code32, code64] })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code[decoder.state.operand_size as usize]);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Simple2Iw {
	has_modrm: bool,
	code: [Code; 3],
}

impl OpCodeHandler_Simple2Iw {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Simple2Iw::decode, Self { has_modrm: false, code: [code16, code32, code64] })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_op0_kind(OpKind::Immediate16);
		instruction_internal::internal_set_immediate16(instruction, decoder.read_u16() as u32);
		instruction.set_code(this.code[decoder.state.operand_size as usize]);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Simple3 {
	has_modrm: bool,
	code16: Code,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Simple3 {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Simple3::decode, Self { has_modrm: false, code16, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
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
pub(in crate::decoder) struct OpCodeHandler_Simple5 {
	has_modrm: bool,
	code: [Code; 3],
}

impl OpCodeHandler_Simple5 {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Simple5::decode, Self { has_modrm: false, code: [code16, code32, code64] })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code[decoder.state.address_size as usize]);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Simple5_a32 {
	has_modrm: bool,
	code: [Code; 3],
}

impl OpCodeHandler_Simple5_a32 {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Simple5_a32::decode, Self { has_modrm: false, code: [code16, code32, code64] })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		if decoder.state.address_size != OpSize::Size32 && decoder.invalid_check_mask != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code[decoder.state.address_size as usize]);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Simple5_ModRM_as {
	has_modrm: bool,
	code: [Code; 3],
	reg_base: [u32; 3],
}

impl OpCodeHandler_Simple5_ModRM_as {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(
			OpCodeHandler_Simple5_ModRM_as::decode,
			Self { has_modrm: true, reg_base: [Register::AX as u32, Register::EAX as u32, Register::RAX as u32], code: [code16, code32, code64] },
		)
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		let address_size = decoder.state.address_size;
		instruction.set_code(this.code[address_size as usize]);
		let reg_base = this.reg_base[address_size as usize];
		write_op0_reg!(instruction, reg_base + decoder.state.rm + decoder.state.extra_base_register_base);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Simple4 {
	has_modrm: bool,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Simple4 {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Simple4::decode, Self { has_modrm: false, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code64);
		} else {
			instruction.set_code(this.code32);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_PushSimpleReg {
	has_modrm: bool,
	code16: Code,
	code32: Code,
	code64: Code,
	index: u32,
}

impl OpCodeHandler_PushSimpleReg {
	#[inline]
	pub(in crate::decoder) fn new(index: u32, code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_PushSimpleReg::decode, Self { has_modrm: false, index, code16, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		if decoder.is64b_mode {
			if decoder.state.operand_size != OpSize::Size16 {
				instruction.set_code(this.code64);
				write_op0_reg!(instruction, this.index + decoder.state.extra_base_register_base + Register::RAX as u32);
			} else {
				instruction.set_code(this.code16);
				write_op0_reg!(instruction, this.index + decoder.state.extra_base_register_base + Register::AX as u32);
			}
		} else {
			if decoder.state.operand_size == OpSize::Size32 {
				instruction.set_code(this.code32);
				write_op0_reg!(instruction, this.index + decoder.state.extra_base_register_base + Register::EAX as u32);
			} else {
				instruction.set_code(this.code16);
				write_op0_reg!(instruction, this.index + decoder.state.extra_base_register_base + Register::AX as u32);
			}
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_SimpleReg {
	has_modrm: bool,
	code: Code,
	index: u32,
}

impl OpCodeHandler_SimpleReg {
	#[inline]
	pub(in crate::decoder) fn new(code: Code, index: u32) -> (OpCodeHandlerDecodeFn, Self) {
		const _: () = assert!(OpSize::Size16 as u32 == 0);
		const _: () = assert!(OpSize::Size32 as u32 == 1);
		const _: () = assert!(OpSize::Size64 as u32 == 2);
		debug_assert!(code as u32 + 2 < IcedConstants::CODE_ENUM_COUNT as u32);

		(OpCodeHandler_SimpleReg::decode, Self { has_modrm: false, code, index })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		const _: () = assert!(OpSize::Size16 as u32 == 0);
		const _: () = assert!(OpSize::Size32 as u32 == 1);
		const _: () = assert!(OpSize::Size64 as u32 == 2);
		let size_index = decoder.state.operand_size as u32;

		// SAFETY: this.code + {0,1,2} is a valid Code value, see ctor
		instruction.set_code(unsafe { mem::transmute((size_index + this.code as u32) as CodeUnderlyingType) });
		const _: () = assert!(Register::AX as u32 + 16 == Register::EAX as u32);
		const _: () = assert!(Register::AX as u32 + 32 == Register::RAX as u32);
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
pub(in crate::decoder) struct OpCodeHandler_Xchg_Reg_rAX {
	has_modrm: bool,
	index: u32,
}

impl OpCodeHandler_Xchg_Reg_rAX {
	#[inline]
	pub(in crate::decoder) fn new(index: u32) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Xchg_Reg_rAX::decode, Self { has_modrm: false, index })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);

		if this.index == 0 && decoder.state.mandatory_prefix == DecoderMandatoryPrefix::PF3 && (decoder.options & DecoderOptions::NO_PAUSE) == 0 {
			decoder.clear_mandatory_prefix_f3(instruction);
			instruction.set_code(Code::Pause);
		} else {
			const _: () = assert!(OpSize::Size16 as u32 == 0);
			const _: () = assert!(OpSize::Size32 as u32 == 1);
			const _: () = assert!(OpSize::Size64 as u32 == 2);
			let size_index = decoder.state.operand_size as u32;
			let code_index = this.index + decoder.state.extra_base_register_base;

			instruction.set_code(unsafe { *XCHG_REG_RAX_CODES.get_unchecked((size_index * 16 + code_index) as usize) });
			if code_index != 0 {
				const _: () = assert!(Register::AX as u32 + 16 == Register::EAX as u32);
				const _: () = assert!(Register::AX as u32 + 32 == Register::RAX as u32);
				let reg = size_index * 16 + code_index + Register::AX as u32;
				write_op0_reg!(instruction, reg);
				write_op1_reg!(instruction, size_index * 16 + Register::AX as u32);
			}
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Reg_Iz {
	has_modrm: bool,
	code: [Code; 3],
}

impl OpCodeHandler_Reg_Iz {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Reg_Iz::decode, Self { has_modrm: false, code: [code16, code32, code64] })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		let operand_size = decoder.state.operand_size;
		instruction.set_code(this.code[operand_size as usize]);
		if operand_size == OpSize::Size32 {
			instruction.set_op0_register(Register::EAX);
			instruction.set_op1_kind(OpKind::Immediate32);
			instruction.set_immediate32(decoder.read_u32() as u32);
		} else if operand_size == OpSize::Size64 {
			instruction.set_op0_register(Register::RAX);
			instruction.set_op1_kind(OpKind::Immediate32to64);
			instruction.set_immediate32(decoder.read_u32() as u32);
		} else {
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
pub(in crate::decoder) struct OpCodeHandler_RegIb3 {
	has_modrm: bool,
	index: u32,
}

impl OpCodeHandler_RegIb3 {
	#[inline]
	pub(in crate::decoder) fn new(index: u32) -> (OpCodeHandlerDecodeFn, Self) {
		debug_assert!(index <= 7);
		(OpCodeHandler_RegIb3::decode, Self { has_modrm: false, index })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(Code::Mov_r8_imm8);
		instruction.set_op1_kind(OpKind::Immediate8);
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
		if (decoder.state.flags & StateFlags::HAS_REX) != 0 {
			// SAFETY: index <= 7 (see ctor) and extra_base_register_base == 0 or 8 so index = 0-15
			let register =
				unsafe { *WITH_REX_PREFIX_MOV_REGISTERS.get_unchecked((this.index + decoder.state.extra_base_register_base) as usize) } as u32;
			write_op0_reg!(instruction, register);
		} else {
			write_op0_reg!(instruction, this.index + Register::AL as u32);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_RegIz2 {
	has_modrm: bool,
	index: u32,
}

impl OpCodeHandler_RegIz2 {
	#[inline]
	pub(in crate::decoder) fn new(index: u32) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_RegIz2::decode, Self { has_modrm: false, index })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		if decoder.state.operand_size == OpSize::Size32 {
			instruction.set_code(Code::Mov_r32_imm32);
			write_op0_reg!(instruction, this.index + decoder.state.extra_base_register_base + Register::EAX as u32);
			instruction.set_op1_kind(OpKind::Immediate32);
			instruction.set_immediate32(decoder.read_u32() as u32);
		} else if decoder.state.operand_size == OpSize::Size64 {
			instruction.set_code(Code::Mov_r64_imm64);
			write_op0_reg!(instruction, this.index + decoder.state.extra_base_register_base + Register::RAX as u32);
			instruction.set_op1_kind(OpKind::Immediate64);
			let q = decoder.read_u64();
			instruction_internal::internal_set_immediate64_lo(instruction, q as u32);
			instruction_internal::internal_set_immediate64_hi(instruction, (q >> 32) as u32);
		} else {
			instruction.set_code(Code::Mov_r16_imm16);
			write_op0_reg!(instruction, this.index + decoder.state.extra_base_register_base + Register::AX as u32);
			instruction.set_op1_kind(OpKind::Immediate16);
			instruction_internal::internal_set_immediate16(instruction, decoder.read_u16() as u32);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_PushIb2 {
	has_modrm: bool,
	code16: Code,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_PushIb2 {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_PushIb2::decode, Self { has_modrm: false, code16, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
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
pub(in crate::decoder) struct OpCodeHandler_PushIz {
	has_modrm: bool,
	code16: Code,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_PushIz {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_PushIz::decode, Self { has_modrm: false, code16, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
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
pub(in crate::decoder) struct OpCodeHandler_Gv_Ma {
	has_modrm: bool,
	code16: Code,
	code32: Code,
}

impl OpCodeHandler_Gv_Ma {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Gv_Ma::decode, Self { has_modrm: true, code16, code32 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		if decoder.state.operand_size != OpSize::Size16 {
			instruction.set_code(this.code32);
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32);
		} else {
			instruction.set_code(this.code16);
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::AX as u32);
		}
		if decoder.state.mod_ < 3 {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op1_kind(OpKind::Memory);
			});
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_RvMw_Gw {
	has_modrm: bool,
	code16: Code,
	code32: Code,
}

impl OpCodeHandler_RvMw_Gw {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_RvMw_Gw::decode, Self { has_modrm: true, code16, code32 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		let base_reg;
		if decoder.state.operand_size != OpSize::Size16 {
			instruction.set_code(this.code32);
			write_op1_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32);
			base_reg = Register::EAX;
		} else {
			instruction.set_code(this.code16);
			write_op1_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::AX as u32);
			base_reg = Register::AX;
		}
		if decoder.state.mod_ == 3 {
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + base_reg as u32);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op0_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Gv_Ev_Ib {
	has_modrm: bool,
	code: [Code; 3],
	reg_base: [u32; 3],
}

impl OpCodeHandler_Gv_Ev_Ib {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(
			OpCodeHandler_Gv_Ev_Ib::decode,
			Self { has_modrm: true, reg_base: [Register::AX as u32, Register::EAX as u32, Register::RAX as u32], code: [code16, code32, code64] },
		)
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		let operand_size = decoder.state.operand_size;
		instruction.set_code(this.code[operand_size as usize]);
		let reg_base = this.reg_base[operand_size as usize];
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, reg_base + decoder.state.rm + decoder.state.extra_base_register_base);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op1_kind(OpKind::Memory);
			});
		}
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
		write_op0_reg!(instruction, reg_base + decoder.state.reg + decoder.state.extra_register_base);
		if operand_size == OpSize::Size32 {
			instruction.set_op2_kind(OpKind::Immediate8to32);
		} else if operand_size == OpSize::Size64 {
			instruction.set_op2_kind(OpKind::Immediate8to64);
		} else {
			instruction.set_op2_kind(OpKind::Immediate8to16);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Gv_Ev_Ib_REX {
	has_modrm: bool,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Gv_Ev_Ib_REX {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Gv_Ev_Ib_REX::decode, Self { has_modrm: true, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		debug_assert_eq!(decoder.state.mod_, 3);
		write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::XMM0 as u32);
		instruction.set_op2_kind(OpKind::Immediate8);
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code64);
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32);
		} else {
			instruction.set_code(this.code32);
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Gv_Ev_32_64 {
	has_modrm: bool,
	disallow_reg: u32,
	disallow_mem: u32,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Gv_Ev_32_64 {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code, allow_reg: bool, allow_mem: bool) -> (OpCodeHandlerDecodeFn, Self) {
		(
			OpCodeHandler_Gv_Ev_32_64::decode,
			Self {
				has_modrm: true,
				code32,
				code64,
				disallow_mem: if allow_mem { 0 } else { u32::MAX },
				disallow_reg: if allow_reg { 0 } else { u32::MAX },
			},
		)
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		let base_reg;
		if decoder.is64b_mode {
			instruction.set_code(this.code64);
			base_reg = Register::RAX;
		} else {
			instruction.set_code(this.code32);
			base_reg = Register::EAX;
		}
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + base_reg as u32);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + base_reg as u32);
			if (this.disallow_reg & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				if (this.disallow_mem & decoder.invalid_check_mask) != 0 {
					decoder.set_invalid_instruction();
				}
				instruction.set_op1_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Gv_Ev_Iz {
	has_modrm: bool,
	code: [Code; 3],
	reg_base: [u32; 3],
}

impl OpCodeHandler_Gv_Ev_Iz {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(
			OpCodeHandler_Gv_Ev_Iz::decode,
			Self { has_modrm: true, reg_base: [Register::AX as u32, Register::EAX as u32, Register::RAX as u32], code: [code16, code32, code64] },
		)
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		let operand_size = decoder.state.operand_size;
		instruction.set_code(this.code[operand_size as usize]);
		let reg_base = this.reg_base[operand_size as usize];
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, reg_base + decoder.state.rm + decoder.state.extra_base_register_base);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op1_kind(OpKind::Memory);
			});
		}
		write_op0_reg!(instruction, reg_base + decoder.state.reg + decoder.state.extra_register_base);
		if operand_size == OpSize::Size32 {
			instruction.set_op2_kind(OpKind::Immediate32);
			instruction.set_immediate32(decoder.read_u32() as u32);
		} else if operand_size == OpSize::Size64 {
			instruction.set_op2_kind(OpKind::Immediate32to64);
			instruction.set_immediate32(decoder.read_u32() as u32);
		} else {
			instruction.set_op2_kind(OpKind::Immediate16);
			instruction_internal::internal_set_immediate16(instruction, decoder.read_u16() as u32);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Yb_Reg {
	has_modrm: bool,
	reg: Register,
	code: Code,
}

impl OpCodeHandler_Yb_Reg {
	#[inline]
	pub(in crate::decoder) fn new(code: Code, reg: Register) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Yb_Reg::decode, Self { has_modrm: false, code, reg })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
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
pub(in crate::decoder) struct OpCodeHandler_Yv_Reg {
	has_modrm: bool,
	code: [Code; 3],
}

impl OpCodeHandler_Yv_Reg {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Yv_Reg::decode, Self { has_modrm: false, code: [code16, code32, code64] })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		if decoder.state.address_size == OpSize::Size64 {
			instruction.set_op0_kind(OpKind::MemoryESRDI);
		} else if decoder.state.address_size == OpSize::Size32 {
			instruction.set_op0_kind(OpKind::MemoryESEDI);
		} else {
			instruction.set_op0_kind(OpKind::MemoryESDI);
		}
		instruction.set_code(this.code[decoder.state.operand_size as usize]);
		if decoder.state.operand_size == OpSize::Size32 {
			instruction.set_op1_register(Register::EAX);
		} else if decoder.state.operand_size == OpSize::Size64 {
			instruction.set_op1_register(Register::RAX);
		} else {
			instruction.set_op1_register(Register::AX);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Yv_Reg2 {
	has_modrm: bool,
	code16: Code,
	code32: Code,
}

impl OpCodeHandler_Yv_Reg2 {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Yv_Reg2::decode, Self { has_modrm: false, code16, code32 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_op1_register(Register::DX);
		if decoder.state.address_size == OpSize::Size64 {
			instruction.set_op0_kind(OpKind::MemoryESRDI);
		} else if decoder.state.address_size == OpSize::Size32 {
			instruction.set_op0_kind(OpKind::MemoryESEDI);
		} else {
			instruction.set_op0_kind(OpKind::MemoryESDI);
		}
		if decoder.state.operand_size != OpSize::Size16 {
			instruction.set_code(this.code32);
		} else {
			instruction.set_code(this.code16);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Reg_Xb {
	has_modrm: bool,
	reg: Register,
	code: Code,
}

impl OpCodeHandler_Reg_Xb {
	#[inline]
	pub(in crate::decoder) fn new(code: Code, reg: Register) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Reg_Xb::decode, Self { has_modrm: false, code, reg })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
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
pub(in crate::decoder) struct OpCodeHandler_Reg_Xv {
	has_modrm: bool,
	code: [Code; 3],
}

impl OpCodeHandler_Reg_Xv {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Reg_Xv::decode, Self { has_modrm: false, code: [code16, code32, code64] })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		if decoder.state.address_size == OpSize::Size64 {
			instruction.set_op1_kind(OpKind::MemorySegRSI);
		} else if decoder.state.address_size == OpSize::Size32 {
			instruction.set_op1_kind(OpKind::MemorySegESI);
		} else {
			instruction.set_op1_kind(OpKind::MemorySegSI);
		}
		instruction.set_code(this.code[decoder.state.operand_size as usize]);
		if decoder.state.operand_size == OpSize::Size32 {
			instruction.set_op0_register(Register::EAX);
		} else if decoder.state.operand_size == OpSize::Size64 {
			instruction.set_op0_register(Register::RAX);
		} else {
			instruction.set_op0_register(Register::AX);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Reg_Xv2 {
	has_modrm: bool,
	code16: Code,
	code32: Code,
}

impl OpCodeHandler_Reg_Xv2 {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Reg_Xv2::decode, Self { has_modrm: false, code16, code32 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_op0_register(Register::DX);
		if decoder.state.address_size == OpSize::Size64 {
			instruction.set_op1_kind(OpKind::MemorySegRSI);
		} else if decoder.state.address_size == OpSize::Size32 {
			instruction.set_op1_kind(OpKind::MemorySegESI);
		} else {
			instruction.set_op1_kind(OpKind::MemorySegSI);
		}
		if decoder.state.operand_size != OpSize::Size16 {
			instruction.set_code(this.code32);
		} else {
			instruction.set_code(this.code16);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Reg_Yb {
	has_modrm: bool,
	reg: Register,
	code: Code,
}

impl OpCodeHandler_Reg_Yb {
	#[inline]
	pub(in crate::decoder) fn new(code: Code, reg: Register) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Reg_Yb::decode, Self { has_modrm: false, code, reg })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
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
pub(in crate::decoder) struct OpCodeHandler_Reg_Yv {
	has_modrm: bool,
	code: [Code; 3],
}

impl OpCodeHandler_Reg_Yv {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Reg_Yv::decode, Self { has_modrm: false, code: [code16, code32, code64] })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		if decoder.state.address_size == OpSize::Size64 {
			instruction.set_op1_kind(OpKind::MemoryESRDI);
		} else if decoder.state.address_size == OpSize::Size32 {
			instruction.set_op1_kind(OpKind::MemoryESEDI);
		} else {
			instruction.set_op1_kind(OpKind::MemoryESDI);
		}
		instruction.set_code(this.code[decoder.state.operand_size as usize]);
		if decoder.state.operand_size == OpSize::Size32 {
			instruction.set_op0_register(Register::EAX);
		} else if decoder.state.operand_size == OpSize::Size64 {
			instruction.set_op0_register(Register::RAX);
		} else {
			instruction.set_op0_register(Register::AX);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Yb_Xb {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_Yb_Xb {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Yb_Xb::decode, Self { has_modrm: false, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
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
pub(in crate::decoder) struct OpCodeHandler_Yv_Xv {
	has_modrm: bool,
	code: [Code; 3],
}

impl OpCodeHandler_Yv_Xv {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Yv_Xv::decode, Self { has_modrm: false, code: [code16, code32, code64] })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
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
pub(in crate::decoder) struct OpCodeHandler_Xb_Yb {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_Xb_Yb {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Xb_Yb::decode, Self { has_modrm: false, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
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
pub(in crate::decoder) struct OpCodeHandler_Xv_Yv {
	has_modrm: bool,
	code: [Code; 3],
}

impl OpCodeHandler_Xv_Yv {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Xv_Yv::decode, Self { has_modrm: false, code: [code16, code32, code64] })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
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
pub(in crate::decoder) struct OpCodeHandler_Ev_Sw {
	has_modrm: bool,
	code: [Code; 3],
	reg_base: [u32; 3],
}

impl OpCodeHandler_Ev_Sw {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(
			OpCodeHandler_Ev_Sw::decode,
			Self { has_modrm: true, reg_base: [Register::AX as u32, Register::EAX as u32, Register::RAX as u32], code: [code16, code32, code64] },
		)
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		let operand_size = decoder.state.operand_size;
		instruction.set_code(this.code[operand_size as usize]);
		let seg = decoder.read_op_seg_reg();
		write_op1_reg!(instruction, seg);
		if decoder.state.mod_ == 3 {
			let reg_base = this.reg_base[operand_size as usize];
			write_op0_reg!(instruction, reg_base + decoder.state.rm + decoder.state.extra_base_register_base);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op0_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_M_Sw {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_M_Sw {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_M_Sw::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		let seg = decoder.read_op_seg_reg();
		write_op1_reg!(instruction, seg);
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
pub(in crate::decoder) struct OpCodeHandler_Gv_M {
	has_modrm: bool,
	code: [Code; 3],
	reg_base: [u32; 3],
}

impl OpCodeHandler_Gv_M {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(
			OpCodeHandler_Gv_M::decode,
			Self { has_modrm: true, reg_base: [Register::AX as u32, Register::EAX as u32, Register::RAX as u32], code: [code16, code32, code64] },
		)
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		let operand_size = decoder.state.operand_size;
		instruction.set_code(this.code[operand_size as usize]);
		let reg_base = this.reg_base[operand_size as usize];
		write_op0_reg!(instruction, reg_base + decoder.state.reg + decoder.state.extra_register_base);
		if decoder.state.mod_ < 3 {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op1_kind(OpKind::Memory);
			});
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Sw_Ev {
	has_modrm: bool,
	code: [Code; 3],
	reg_base: [u32; 3],
}

impl OpCodeHandler_Sw_Ev {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(
			OpCodeHandler_Sw_Ev::decode,
			Self { has_modrm: true, reg_base: [Register::AX as u32, Register::EAX as u32, Register::RAX as u32], code: [code16, code32, code64] },
		)
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		let operand_size = decoder.state.operand_size;
		instruction.set_code(this.code[operand_size as usize]);
		let sreg = decoder.read_op_seg_reg();
		if decoder.invalid_check_mask != 0 && sreg == Register::CS as u32 {
			decoder.set_invalid_instruction();
		}
		write_op0_reg!(instruction, sreg);
		if decoder.state.mod_ == 3 {
			let reg_base = this.reg_base[operand_size as usize];
			write_op1_reg!(instruction, reg_base + decoder.state.rm + decoder.state.extra_base_register_base);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op1_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Sw_M {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_Sw_M {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Sw_M::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		let seg = decoder.read_op_seg_reg();
		write_op0_reg!(instruction, seg);
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
pub(in crate::decoder) struct OpCodeHandler_Ap {
	has_modrm: bool,
	code16: Code,
	code32: Code,
}

impl OpCodeHandler_Ap {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Ap::decode, Self { has_modrm: false, code16, code32 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
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
pub(in crate::decoder) struct OpCodeHandler_Reg_Ob {
	has_modrm: bool,
	reg: Register,
	code: Code,
}

impl OpCodeHandler_Reg_Ob {
	#[inline]
	pub(in crate::decoder) fn new(code: Code, reg: Register) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Reg_Ob::decode, Self { has_modrm: false, code, reg })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		instruction.set_op0_register(this.reg);
		decoder.displ_index = decoder.data_ptr as u8;
		//instruction_internal::internal_set_memory_index_scale(instruction, 0);
		//instruction.set_memory_base(Register::None);
		//instruction.set_memory_index(Register::None);
		instruction.set_op1_kind(OpKind::Memory);
		if decoder.state.address_size == OpSize::Size64 {
			instruction_internal::internal_set_memory_displ_size(instruction, 4);
			decoder.state.flags |= StateFlags::ADDR64;
			instruction.set_memory_displacement64(decoder.read_u64());
		} else if decoder.state.address_size == OpSize::Size32 {
			instruction_internal::internal_set_memory_displ_size(instruction, 3);
			instruction.set_memory_displacement64(decoder.read_u32() as u64);
		} else {
			instruction_internal::internal_set_memory_displ_size(instruction, 2);
			instruction.set_memory_displacement64(decoder.read_u16() as u64);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Ob_Reg {
	has_modrm: bool,
	reg: Register,
	code: Code,
}

impl OpCodeHandler_Ob_Reg {
	#[inline]
	pub(in crate::decoder) fn new(code: Code, reg: Register) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Ob_Reg::decode, Self { has_modrm: false, code, reg })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		instruction.set_op1_register(this.reg);
		decoder.displ_index = decoder.data_ptr as u8;
		//instruction_internal::internal_set_memory_index_scale(instruction, 0);
		//instruction.set_memory_base(Register::None);
		//instruction.set_memory_index(Register::None);
		instruction.set_op0_kind(OpKind::Memory);
		if decoder.state.address_size == OpSize::Size64 {
			instruction_internal::internal_set_memory_displ_size(instruction, 4);
			decoder.state.flags |= StateFlags::ADDR64;
			instruction.set_memory_displacement64(decoder.read_u64());
		} else if decoder.state.address_size == OpSize::Size32 {
			instruction_internal::internal_set_memory_displ_size(instruction, 3);
			instruction.set_memory_displacement64(decoder.read_u32() as u64);
		} else {
			instruction_internal::internal_set_memory_displ_size(instruction, 2);
			instruction.set_memory_displacement64(decoder.read_u16() as u64);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Reg_Ov {
	has_modrm: bool,
	code: [Code; 3],
}

impl OpCodeHandler_Reg_Ov {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Reg_Ov::decode, Self { has_modrm: false, code: [code16, code32, code64] })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		decoder.displ_index = decoder.data_ptr as u8;
		instruction.set_code(this.code[decoder.state.operand_size as usize]);
		if decoder.state.operand_size == OpSize::Size32 {
			instruction.set_op0_register(Register::EAX);
		} else if decoder.state.operand_size == OpSize::Size64 {
			instruction.set_op0_register(Register::RAX);
		} else {
			instruction.set_op0_register(Register::AX);
		}
		//instruction_internal::internal_set_memory_index_scale(instruction, 0);
		//instruction.set_memory_base(Register::None);
		//instruction.set_memory_index(Register::None);
		instruction.set_op1_kind(OpKind::Memory);
		if decoder.state.address_size == OpSize::Size64 {
			instruction_internal::internal_set_memory_displ_size(instruction, 4);
			decoder.state.flags |= StateFlags::ADDR64;
			instruction.set_memory_displacement64(decoder.read_u64());
		} else if decoder.state.address_size == OpSize::Size32 {
			instruction_internal::internal_set_memory_displ_size(instruction, 3);
			instruction.set_memory_displacement64(decoder.read_u32() as u64);
		} else {
			instruction_internal::internal_set_memory_displ_size(instruction, 2);
			instruction.set_memory_displacement64(decoder.read_u16() as u64);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Ov_Reg {
	has_modrm: bool,
	code: [Code; 3],
}

impl OpCodeHandler_Ov_Reg {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Ov_Reg::decode, Self { has_modrm: false, code: [code16, code32, code64] })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		decoder.displ_index = decoder.data_ptr as u8;
		instruction.set_code(this.code[decoder.state.operand_size as usize]);
		if decoder.state.operand_size == OpSize::Size32 {
			instruction.set_op1_register(Register::EAX);
		} else if decoder.state.operand_size == OpSize::Size64 {
			instruction.set_op1_register(Register::RAX);
		} else {
			instruction.set_op1_register(Register::AX);
		}
		//instruction_internal::internal_set_memory_index_scale(instruction, 0);
		//instruction.set_memory_base(Register::None);
		//instruction.set_memory_index(Register::None);
		instruction.set_op0_kind(OpKind::Memory);
		if decoder.state.address_size == OpSize::Size64 {
			instruction_internal::internal_set_memory_displ_size(instruction, 4);
			decoder.state.flags |= StateFlags::ADDR64;
			instruction.set_memory_displacement64(decoder.read_u64());
		} else if decoder.state.address_size == OpSize::Size32 {
			instruction_internal::internal_set_memory_displ_size(instruction, 3);
			instruction.set_memory_displacement64(decoder.read_u32() as u64);
		} else {
			instruction_internal::internal_set_memory_displ_size(instruction, 2);
			instruction.set_memory_displacement64(decoder.read_u16() as u64);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_BranchIw {
	has_modrm: bool,
	code16: Code,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_BranchIw {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_BranchIw::decode, Self { has_modrm: false, code16, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_op0_kind(OpKind::Immediate16);
		instruction_internal::internal_set_immediate16(instruction, decoder.read_u16() as u32);
		if decoder.is64b_mode {
			if (((decoder.options ^ DecoderOptions::AMD) & DecoderOptions::AMD) | (decoder.state.operand_size as u32 - OpSize::Size16 as u32)) != 0 {
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
pub(in crate::decoder) struct OpCodeHandler_BranchSimple {
	has_modrm: bool,
	code16: Code,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_BranchSimple {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_BranchSimple::decode, Self { has_modrm: false, code16, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		if decoder.is64b_mode {
			if (((decoder.options ^ DecoderOptions::AMD) & DecoderOptions::AMD) | (decoder.state.operand_size as u32 - OpSize::Size16 as u32)) != 0 {
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
pub(in crate::decoder) struct OpCodeHandler_Iw_Ib {
	has_modrm: bool,
	code16: Code,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Iw_Ib {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Iw_Ib::decode, Self { has_modrm: false, code16, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
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
pub(in crate::decoder) struct OpCodeHandler_Reg_Ib2 {
	has_modrm: bool,
	code16: Code,
	code32: Code,
}

impl OpCodeHandler_Reg_Ib2 {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Reg_Ib2::decode, Self { has_modrm: false, code16, code32 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
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
pub(in crate::decoder) struct OpCodeHandler_IbReg2 {
	has_modrm: bool,
	code16: Code,
	code32: Code,
}

impl OpCodeHandler_IbReg2 {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_IbReg2::decode, Self { has_modrm: false, code16, code32 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_op0_kind(OpKind::Immediate8);
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
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
pub(in crate::decoder) struct OpCodeHandler_eAX_DX {
	has_modrm: bool,
	code16: Code,
	code32: Code,
}

impl OpCodeHandler_eAX_DX {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_eAX_DX::decode, Self { has_modrm: false, code16, code32 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_op1_register(Register::DX);
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
pub(in crate::decoder) struct OpCodeHandler_DX_eAX {
	has_modrm: bool,
	code16: Code,
	code32: Code,
}

impl OpCodeHandler_DX_eAX {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_DX_eAX::decode, Self { has_modrm: false, code16, code32 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_op0_register(Register::DX);
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
pub(in crate::decoder) struct OpCodeHandler_Eb_Ib {
	has_modrm: bool,
	code: Code,
	state_flags_or_value: u32,
}

impl OpCodeHandler_Eb_Ib {
	#[inline]
	pub(in crate::decoder) fn new(code: Code, flags: u32) -> (OpCodeHandlerDecodeFn, Self) {
		const _: () = assert!(HandlerFlags::LOCK == 1 << 3);
		const _: () = assert!(StateFlags::ALLOW_LOCK == 1 << 13);
		let state_flags_or_value = (flags & HandlerFlags::LOCK) << (13 - 3);

		(OpCodeHandler_Eb_Ib::decode, Self { has_modrm: true, code, state_flags_or_value })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		if decoder.state.mod_ < 3 {
			read_op_mem_stmt!(decoder, instruction, {
				decoder.state.flags |= this.state_flags_or_value;
				instruction.set_op0_kind(OpKind::Memory);
			});
		} else {
			let mut index = decoder.state.rm + decoder.state.extra_base_register_base;
			if (decoder.state.flags & StateFlags::HAS_REX) != 0 && index >= 4 {
				index += 4;
			}
			write_op0_reg!(instruction, index + Register::AL as u32);
		}
		instruction.set_op1_kind(OpKind::Immediate8);
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Eb_1 {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_Eb_1 {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Eb_1::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		instruction.set_op1_kind(OpKind::Immediate8);
		instruction_internal::internal_set_immediate8(instruction, 1);
		decoder.state.flags |= StateFlags::NO_IMM;
		if decoder.state.mod_ == 3 {
			let mut index = decoder.state.rm + decoder.state.extra_base_register_base;
			if (decoder.state.flags & StateFlags::HAS_REX) != 0 && index >= 4 {
				index += 4;
			}
			write_op0_reg!(instruction, index + Register::AL as u32);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op0_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Eb_CL {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_Eb_CL {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Eb_CL::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		instruction.set_op1_register(Register::CL);
		if decoder.state.mod_ == 3 {
			let mut index = decoder.state.rm + decoder.state.extra_base_register_base;
			if (decoder.state.flags & StateFlags::HAS_REX) != 0 && index >= 4 {
				index += 4;
			}
			write_op0_reg!(instruction, index + Register::AL as u32);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op0_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Eb {
	has_modrm: bool,
	code: Code,
	state_flags_or_value: u32,
}

impl OpCodeHandler_Eb {
	#[inline]
	pub(in crate::decoder) fn new(code: Code, flags: u32) -> (OpCodeHandlerDecodeFn, Self) {
		const _: () = assert!(HandlerFlags::LOCK == 1 << 3);
		const _: () = assert!(StateFlags::ALLOW_LOCK == 1 << 13);
		let state_flags_or_value = (flags & HandlerFlags::LOCK) << (13 - 3);

		(OpCodeHandler_Eb::decode, Self { has_modrm: true, code, state_flags_or_value })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		if decoder.state.mod_ == 3 {
			let mut index = decoder.state.rm + decoder.state.extra_base_register_base;
			if (decoder.state.flags & StateFlags::HAS_REX) != 0 && index >= 4 {
				index += 4;
			}
			write_op0_reg!(instruction, index + Register::AL as u32);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				decoder.state.flags |= this.state_flags_or_value;
				instruction.set_op0_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Eb_Gb {
	has_modrm: bool,
	code: Code,
	state_flags_or_value: u32,
}

impl OpCodeHandler_Eb_Gb {
	#[inline]
	pub(in crate::decoder) fn new(code: Code, flags: u32) -> (OpCodeHandlerDecodeFn, Self) {
		const _: () = assert!(HandlerFlags::LOCK == 1 << 3);
		const _: () = assert!(StateFlags::ALLOW_LOCK == 1 << 13);
		let state_flags_or_value = (flags & HandlerFlags::LOCK) << (13 - 3);

		(OpCodeHandler_Eb_Gb::decode, Self { has_modrm: true, code, state_flags_or_value })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		let mut index = decoder.state.reg + decoder.state.extra_register_base;
		if (decoder.state.flags & StateFlags::HAS_REX) != 0 && index >= 4 {
			index += 4;
		}
		write_op1_reg!(instruction, index + Register::AL as u32);
		if decoder.state.mod_ == 3 {
			index = decoder.state.rm + decoder.state.extra_base_register_base;
			if (decoder.state.flags & StateFlags::HAS_REX) != 0 && index >= 4 {
				index += 4;
			}
			write_op0_reg!(instruction, index + Register::AL as u32);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				decoder.state.flags |= this.state_flags_or_value;
				instruction.set_op0_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Gb_Eb {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_Gb_Eb {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Gb_Eb::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		let mut index = decoder.state.reg + decoder.state.extra_register_base;
		if (decoder.state.flags & StateFlags::HAS_REX) != 0 && index >= 4 {
			index += 4;
		}
		write_op0_reg!(instruction, index + Register::AL as u32);

		if decoder.state.mod_ < 3 {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op1_kind(OpKind::Memory);
			});
		} else {
			index = decoder.state.rm + decoder.state.extra_base_register_base;
			if (decoder.state.flags & StateFlags::HAS_REX) != 0 && index >= 4 {
				index += 4;
			}
			write_op1_reg!(instruction, index + Register::AL as u32);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_M {
	has_modrm: bool,
	code_w0: Code,
	code_w1: Code,
}

impl OpCodeHandler_M {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_M::decode, Self { has_modrm: true, code_w0: code, code_w1: code })
	}

	#[inline]
	pub(in crate::decoder) fn new1(code_w0: Code, code_w1: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_M::decode, Self { has_modrm: true, code_w0, code_w1 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code_w1);
		} else {
			instruction.set_code(this.code_w0);
		}
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
pub(in crate::decoder) struct OpCodeHandler_M_REXW {
	has_modrm: bool,
	flags32: u32,
	flags64: u32,
	code32: Code,
	code64: Code,
	state_flags_or_value: u32,
}

impl OpCodeHandler_M_REXW {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code, flags32: u32, flags64: u32) -> (OpCodeHandlerDecodeFn, Self) {
		debug_assert_eq!(flags32 & HandlerFlags::LOCK, flags64 & HandlerFlags::LOCK);

		const _: () = assert!(HandlerFlags::LOCK == 1 << 3);
		const _: () = assert!(StateFlags::ALLOW_LOCK == 1 << 13);
		let state_flags_or_value = (flags32 & HandlerFlags::LOCK) << (13 - 3);

		(OpCodeHandler_M_REXW::decode, Self { has_modrm: true, flags32, flags64, code32, code64, state_flags_or_value })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code64);
		} else {
			instruction.set_code(this.code32);
		}
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				let flags = if (decoder.state.flags & StateFlags::W) != 0 { this.flags64 } else { this.flags32 };
				if (flags & (HandlerFlags::XACQUIRE | HandlerFlags::XRELEASE)) != 0 {
					decoder.set_xacquire_xrelease(instruction, flags);
				}
				decoder.state.flags |= this.state_flags_or_value;
				instruction.set_op0_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_MemBx {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_MemBx {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_MemBx::decode, Self { has_modrm: false, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		instruction.set_memory_index(Register::AL);
		instruction.set_op0_kind(OpKind::Memory);
		//instruction.set_memory_displacement64(0);
		//instruction_internal::internal_set_memory_index_scale(instruction, 0);
		//instruction_internal::internal_set_memory_displ_size(instruction, 0);
		const _: () = assert!(Register::BX as u32 + 16 == Register::EBX as u32);
		const _: () = assert!(Register::BX as u32 + 32 == Register::RBX as u32);
		instruction.set_memory_base((decoder.state.address_size as u32) * 16 + Register::BX);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VW {
	has_modrm: bool,
	code_r: Code,
	code_m: Code,
}

impl OpCodeHandler_VW {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VW::decode, Self { has_modrm: true, code_r: code, code_m: code })
	}

	#[inline]
	pub(in crate::decoder) fn new1(code_r: Code, code_m: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VW::decode, Self { has_modrm: true, code_r, code_m })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::XMM0 as u32);
		if decoder.state.mod_ == 3 {
			instruction.set_code(this.code_r);
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::XMM0 as u32);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_code(this.code_m);
				instruction.set_op1_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_WV {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_WV {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_WV::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		write_op1_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::XMM0 as u32);
		if decoder.state.mod_ < 3 {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op0_kind(OpKind::Memory);
			});
		} else {
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::XMM0 as u32);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_rDI_VX_RX {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_rDI_VX_RX {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_rDI_VX_RX::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		if decoder.state.address_size == OpSize::Size64 {
			instruction.set_op0_kind(OpKind::MemorySegRDI);
		} else if decoder.state.address_size == OpSize::Size32 {
			instruction.set_op0_kind(OpKind::MemorySegEDI);
		} else {
			instruction.set_op0_kind(OpKind::MemorySegDI);
		}
		write_op1_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::XMM0 as u32);
		if decoder.state.mod_ == 3 {
			write_op2_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::XMM0 as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_rDI_P_N {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_rDI_P_N {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_rDI_P_N::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		if decoder.state.address_size == OpSize::Size64 {
			instruction.set_op0_kind(OpKind::MemorySegRDI);
		} else if decoder.state.address_size == OpSize::Size32 {
			instruction.set_op0_kind(OpKind::MemorySegEDI);
		} else {
			instruction.set_op0_kind(OpKind::MemorySegDI);
		}
		write_op1_reg!(instruction, decoder.state.reg + Register::MM0 as u32);
		if decoder.state.mod_ == 3 {
			write_op2_reg!(instruction, decoder.state.rm + Register::MM0 as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VM {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_VM {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VM::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::XMM0 as u32);
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
pub(in crate::decoder) struct OpCodeHandler_MV {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_MV {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_MV::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		write_op1_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::XMM0 as u32);
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
pub(in crate::decoder) struct OpCodeHandler_VQ {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_VQ {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VQ::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::XMM0 as u32);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + Register::MM0 as u32);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op1_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_P_Q {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_P_Q {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_P_Q::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		write_op0_reg!(instruction, decoder.state.reg + Register::MM0 as u32);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + Register::MM0 as u32);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op1_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Q_P {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_Q_P {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Q_P::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		write_op1_reg!(instruction, decoder.state.reg + Register::MM0 as u32);
		if decoder.state.mod_ == 3 {
			write_op0_reg!(instruction, decoder.state.rm + Register::MM0 as u32);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op0_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_MP {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_MP {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_MP::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		write_op1_reg!(instruction, decoder.state.reg + Register::MM0 as u32);
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
pub(in crate::decoder) struct OpCodeHandler_P_Q_Ib {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_P_Q_Ib {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_P_Q_Ib::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		write_op0_reg!(instruction, decoder.state.reg + Register::MM0 as u32);
		instruction.set_op2_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + Register::MM0 as u32);
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
pub(in crate::decoder) struct OpCodeHandler_P_W {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_P_W {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_P_W::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		write_op0_reg!(instruction, decoder.state.reg + Register::MM0 as u32);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::XMM0 as u32);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op1_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_P_R {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_P_R {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_P_R::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		write_op0_reg!(instruction, decoder.state.reg + Register::MM0 as u32);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::XMM0 as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_P_Ev {
	has_modrm: bool,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_P_Ev {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_P_Ev::decode, Self { has_modrm: true, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		let gpr;
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code64);
			gpr = Register::RAX;
		} else {
			instruction.set_code(this.code32);
			gpr = Register::EAX;
		}
		write_op0_reg!(instruction, decoder.state.reg + Register::MM0 as u32);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr as u32);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op1_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_P_Ev_Ib {
	has_modrm: bool,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_P_Ev_Ib {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_P_Ev_Ib::decode, Self { has_modrm: true, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		let gpr;
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code64);
			gpr = Register::RAX;
		} else {
			instruction.set_code(this.code32);
			gpr = Register::EAX;
		}
		write_op0_reg!(instruction, decoder.state.reg + Register::MM0 as u32);
		instruction.set_op2_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr as u32);
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
pub(in crate::decoder) struct OpCodeHandler_Ev_P {
	has_modrm: bool,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Ev_P {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Ev_P::decode, Self { has_modrm: true, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		let gpr;
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code64);
			gpr = Register::RAX;
		} else {
			instruction.set_code(this.code32);
			gpr = Register::EAX;
		}
		write_op1_reg!(instruction, decoder.state.reg + Register::MM0 as u32);
		if decoder.state.mod_ == 3 {
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr as u32);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op0_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Gv_W {
	has_modrm: bool,
	code_w0: Code,
	code_w1: Code,
}

impl OpCodeHandler_Gv_W {
	#[inline]
	pub(in crate::decoder) fn new(code_w0: Code, code_w1: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Gv_W::decode, Self { has_modrm: true, code_w0, code_w1 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code_w1);
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32);
		} else {
			instruction.set_code(this.code_w0);
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32);
		}
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::XMM0 as u32);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op1_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_V_Ev {
	has_modrm: bool,
	code_w0: Code,
	code_w1: Code,
}

impl OpCodeHandler_V_Ev {
	#[inline]
	pub(in crate::decoder) fn new(code_w0: Code, code_w1: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_V_Ev::decode, Self { has_modrm: true, code_w0, code_w1 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		let gpr;
		if decoder.state.operand_size != OpSize::Size64 {
			instruction.set_code(this.code_w0);
			gpr = Register::EAX;
		} else {
			instruction.set_code(this.code_w1);
			gpr = Register::RAX;
		}
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::XMM0 as u32);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr as u32);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op1_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VWIb {
	has_modrm: bool,
	code_w0: Code,
	code_w1: Code,
}

impl OpCodeHandler_VWIb {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VWIb::decode, Self { has_modrm: true, code_w0: code, code_w1: code })
	}

	#[inline]
	pub(in crate::decoder) fn new1(code_w0: Code, code_w1: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VWIb::decode, Self { has_modrm: true, code_w0, code_w1 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code_w1);
		} else {
			instruction.set_code(this.code_w0);
		}
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::XMM0 as u32);
		instruction.set_op2_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::XMM0 as u32);
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
pub(in crate::decoder) struct OpCodeHandler_VRIbIb {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_VRIbIb {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VRIbIb::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::XMM0 as u32);
		instruction.set_op2_kind(OpKind::Immediate8);
		instruction.set_op3_kind(OpKind::Immediate8_2nd);
		let w = decoder.read_u16() as u32;
		instruction_internal::internal_set_immediate8(instruction, w as u8 as u32);
		instruction_internal::internal_set_immediate8_2nd(instruction, w >> 8);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::XMM0 as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_RIbIb {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_RIbIb {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_RIbIb::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		instruction.set_op1_kind(OpKind::Immediate8);
		instruction.set_op2_kind(OpKind::Immediate8_2nd);
		let w = decoder.read_u16() as u32;
		instruction_internal::internal_set_immediate8(instruction, w as u8 as u32);
		instruction_internal::internal_set_immediate8_2nd(instruction, w >> 8);
		if decoder.state.mod_ == 3 {
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::XMM0 as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_RIb {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_RIb {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_RIb::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		instruction.set_op1_kind(OpKind::Immediate8);
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
		if decoder.state.mod_ == 3 {
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::XMM0 as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Ed_V_Ib {
	has_modrm: bool,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Ed_V_Ib {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Ed_V_Ib::decode, Self { has_modrm: true, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		write_op1_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::XMM0 as u32);
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
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr as u32);
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
pub(in crate::decoder) struct OpCodeHandler_VX_Ev {
	has_modrm: bool,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_VX_Ev {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VX_Ev::decode, Self { has_modrm: true, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		let gpr;
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code64);
			gpr = Register::RAX;
		} else {
			instruction.set_code(this.code32);
			gpr = Register::EAX;
		}
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::XMM0 as u32);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr as u32);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op1_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Ev_VX {
	has_modrm: bool,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Ev_VX {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Ev_VX::decode, Self { has_modrm: true, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		let gpr;
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code64);
			gpr = Register::RAX;
		} else {
			instruction.set_code(this.code32);
			gpr = Register::EAX;
		}
		write_op1_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::XMM0 as u32);
		if decoder.state.mod_ == 3 {
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr as u32);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op0_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VX_E_Ib {
	has_modrm: bool,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_VX_E_Ib {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VX_E_Ib::decode, Self { has_modrm: true, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		let gpr;
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code64);
			gpr = Register::RAX;
		} else {
			instruction.set_code(this.code32);
			gpr = Register::EAX;
		}
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::XMM0 as u32);
		instruction.set_op2_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr as u32);
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
pub(in crate::decoder) struct OpCodeHandler_Gv_RX {
	has_modrm: bool,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Gv_RX {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Gv_RX::decode, Self { has_modrm: true, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code64);
		} else {
			instruction.set_code(this.code32);
		}
		if (decoder.state.flags & StateFlags::W) != 0 {
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32);
		} else {
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32);
		}
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::XMM0 as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_B_MIB {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_B_MIB {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_B_MIB::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		if decoder.state.reg > 3 || (decoder.state.extra_register_base & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);
		write_op0_reg!(instruction, (decoder.state.reg & 3) + Register::BND0 as u32);
		if decoder.state.mod_ < 3 {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem_mpx(instruction);
			// It can't be EIP since if it's MPX + 64-bit mode, the address size is always 64-bit
			if decoder.invalid_check_mask != 0 && instruction.memory_base() == Register::RIP {
				decoder.set_invalid_instruction();
			}
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_MIB_B {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_MIB_B {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_MIB_B::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		if decoder.state.reg > 3 || (decoder.state.extra_register_base & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);
		write_op1_reg!(instruction, (decoder.state.reg & 3) + Register::BND0 as u32);
		if decoder.state.mod_ < 3 {
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem_mpx(instruction);
			// It can't be EIP since if it's MPX + 64-bit mode, the address size is always 64-bit
			if decoder.invalid_check_mask != 0 && instruction.memory_base() == Register::RIP {
				decoder.set_invalid_instruction();
			}
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_B_BM {
	has_modrm: bool,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_B_BM {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_B_BM::decode, Self { has_modrm: true, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		if decoder.state.reg > 3 || (decoder.state.extra_register_base & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		if decoder.is64b_mode {
			instruction.set_code(this.code64);
		} else {
			instruction.set_code(this.code32);
		}
		write_op0_reg!(instruction, (decoder.state.reg & 3) + Register::BND0 as u32);
		if decoder.state.mod_ == 3 {
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
pub(in crate::decoder) struct OpCodeHandler_BM_B {
	has_modrm: bool,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_BM_B {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_BM_B::decode, Self { has_modrm: true, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		if decoder.state.reg > 3 || (decoder.state.extra_register_base & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		if decoder.is64b_mode {
			instruction.set_code(this.code64);
		} else {
			instruction.set_code(this.code32);
		}
		write_op1_reg!(instruction, (decoder.state.reg & 3) + Register::BND0 as u32);
		if decoder.state.mod_ == 3 {
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
pub(in crate::decoder) struct OpCodeHandler_B_Ev {
	has_modrm: bool,
	code32: Code,
	code64: Code,
	rip_rel_mask: u32,
}

impl OpCodeHandler_B_Ev {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code, supports_rip_rel: bool) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_B_Ev::decode, Self { has_modrm: true, code32, code64, rip_rel_mask: if supports_rip_rel { 0 } else { u32::MAX } })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
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
		write_op0_reg!(instruction, (decoder.state.reg & 3) + Register::BND0 as u32);
		if decoder.state.mod_ == 3 {
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
pub(in crate::decoder) struct OpCodeHandler_Mv_Gv_REXW {
	has_modrm: bool,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Mv_Gv_REXW {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Mv_Gv_REXW::decode, Self { has_modrm: true, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code64);
			write_op1_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32);
		} else {
			instruction.set_code(this.code32);
			write_op1_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32);
		}
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
pub(in crate::decoder) struct OpCodeHandler_Gv_N_Ib_REX {
	has_modrm: bool,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Gv_N_Ib_REX {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Gv_N_Ib_REX::decode, Self { has_modrm: true, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code64);
		} else {
			instruction.set_code(this.code32);
		}
		if (decoder.state.flags & StateFlags::W) != 0 {
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32);
		} else {
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32);
		}
		instruction.set_op2_kind(OpKind::Immediate8);
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + Register::MM0 as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Gv_N {
	has_modrm: bool,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Gv_N {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Gv_N::decode, Self { has_modrm: true, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code64);
		} else {
			instruction.set_code(this.code32);
		}
		if (decoder.state.flags & StateFlags::W) != 0 {
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32);
		} else {
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32);
		}
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + Register::MM0 as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VN {
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_VN {
	#[inline]
	pub(in crate::decoder) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_VN::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		instruction.set_code(this.code);
		write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::XMM0 as u32);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + Register::MM0 as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Gv_Mv {
	has_modrm: bool,
	code: [Code; 3],
	reg_base: [u32; 3],
}

impl OpCodeHandler_Gv_Mv {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(
			OpCodeHandler_Gv_Mv::decode,
			Self { has_modrm: true, reg_base: [Register::AX as u32, Register::EAX as u32, Register::RAX as u32], code: [code16, code32, code64] },
		)
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		let operand_size = decoder.state.operand_size;
		instruction.set_code(this.code[operand_size as usize]);
		let reg_base = this.reg_base[operand_size as usize];
		write_op0_reg!(instruction, reg_base + decoder.state.reg + decoder.state.extra_register_base);
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
pub(in crate::decoder) struct OpCodeHandler_Mv_Gv {
	has_modrm: bool,
	code: [Code; 3],
	reg_base: [u32; 3],
}

impl OpCodeHandler_Mv_Gv {
	#[inline]
	pub(in crate::decoder) fn new(code16: Code, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(
			OpCodeHandler_Mv_Gv::decode,
			Self { has_modrm: true, reg_base: [Register::AX as u32, Register::EAX as u32, Register::RAX as u32], code: [code16, code32, code64] },
		)
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		let operand_size = decoder.state.operand_size;
		instruction.set_code(this.code[operand_size as usize]);
		let reg_base = this.reg_base[operand_size as usize];
		write_op1_reg!(instruction, reg_base + decoder.state.reg + decoder.state.extra_register_base);
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
pub(in crate::decoder) struct OpCodeHandler_Gv_Eb_REX {
	has_modrm: bool,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Gv_Eb_REX {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Gv_Eb_REX::decode, Self { has_modrm: true, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code64);
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32);
		} else {
			instruction.set_code(this.code32);
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32);
		}
		if decoder.state.mod_ == 3 {
			let mut index = decoder.state.rm + decoder.state.extra_base_register_base;
			if (decoder.state.flags & StateFlags::HAS_REX) != 0 && index >= 4 {
				index += 4;
			}
			write_op1_reg!(instruction, index + Register::AL as u32);
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op1_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Gv_Ev_REX {
	has_modrm: bool,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Gv_Ev_REX {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Gv_Ev_REX::decode, Self { has_modrm: true, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code64);
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32);
		} else {
			instruction.set_code(this.code32);
			write_op0_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32);
		}
		if decoder.state.mod_ == 3 {
			if (decoder.state.flags & StateFlags::W) != 0 {
				write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::RAX as u32);
			} else {
				write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + Register::EAX as u32);
			}
		} else {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op1_kind(OpKind::Memory);
			});
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_Ev_Gv_REX {
	has_modrm: bool,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_Ev_Gv_REX {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Ev_Gv_REX::decode, Self { has_modrm: true, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		if (decoder.state.flags & StateFlags::W) != 0 {
			instruction.set_code(this.code64);
			write_op1_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32);
		} else {
			instruction.set_code(this.code32);
			write_op1_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32);
		}
		if decoder.state.mod_ < 3 {
			read_op_mem_stmt!(decoder, instruction, {
				instruction.set_op0_kind(OpKind::Memory);
			});
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_GvM_VX_Ib {
	has_modrm: bool,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_GvM_VX_Ib {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_GvM_VX_Ib::decode, Self { has_modrm: true, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		write_op1_reg!(instruction, decoder.state.reg + decoder.state.extra_register_base + Register::XMM0 as u32);
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
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr as u32);
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
pub(in crate::decoder) struct OpCodeHandler_Wbinvd {
	has_modrm: bool,
}

impl OpCodeHandler_Wbinvd {
	#[inline]
	pub(in crate::decoder) fn new() -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Wbinvd::decode, Self { has_modrm: false })
	}

	fn decode(_self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::Legacy as u32);
		if (decoder.options & DecoderOptions::NO_WBNOINVD) != 0 || decoder.state.mandatory_prefix != DecoderMandatoryPrefix::PF3 {
			instruction.set_code(Code::Wbinvd);
		} else {
			decoder.clear_mandatory_prefix_f3(instruction);
			instruction.set_code(Code::Wbnoinvd);
		}
	}
}
