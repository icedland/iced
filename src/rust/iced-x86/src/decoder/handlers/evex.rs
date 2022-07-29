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
pub(in crate::decoder) struct OpCodeHandler_VectorLength_EVEX {
	has_modrm: bool,
	handlers: [(OpCodeHandlerDecodeFn, &'static OpCodeHandler); 4],
}

impl OpCodeHandler_VectorLength_EVEX {
	#[allow(trivial_casts)]
	#[inline]
	pub(in crate::decoder) fn new(
		handler128: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), handler256: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
		handler512: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	) -> (OpCodeHandlerDecodeFn, Self) {
		const _: () = assert!(VectorLength::L128 as u32 == 0);
		const _: () = assert!(VectorLength::L256 as u32 == 1);
		const _: () = assert!(VectorLength::L512 as u32 == 2);
		const _: () = assert!(VectorLength::Unknown as u32 == 3);
		debug_assert!(!is_null_instance_handler(handler128.1));
		debug_assert!(!is_null_instance_handler(handler256.1));
		debug_assert!(!is_null_instance_handler(handler512.1));
		let handlers = [handler128, handler256, handler512, get_invalid_handler()];
		debug_assert!(handlers[0].1.has_modrm);
		debug_assert!(handlers[1].1.has_modrm);
		debug_assert!(handlers[2].1.has_modrm);
		(OpCodeHandler_VectorLength_EVEX::decode, Self { has_modrm: true, handlers })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		let (decode, handler) = this.handlers[decoder.state.vector_length as usize];
		(decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_VectorLength_EVEX_er {
	has_modrm: bool,
	handlers: [(OpCodeHandlerDecodeFn, &'static OpCodeHandler); 4],
}

impl OpCodeHandler_VectorLength_EVEX_er {
	#[allow(trivial_casts)]
	#[inline]
	pub(in crate::decoder) fn new(
		handler128: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), handler256: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
		handler512: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	) -> (OpCodeHandlerDecodeFn, Self) {
		const _: () = assert!(VectorLength::L128 as u32 == 0);
		const _: () = assert!(VectorLength::L256 as u32 == 1);
		const _: () = assert!(VectorLength::L512 as u32 == 2);
		const _: () = assert!(VectorLength::Unknown as u32 == 3);
		debug_assert!(!is_null_instance_handler(handler128.1));
		debug_assert!(!is_null_instance_handler(handler256.1));
		debug_assert!(!is_null_instance_handler(handler512.1));
		let handlers = [handler128, handler256, handler512, get_invalid_handler()];
		debug_assert!(handlers[0].1.has_modrm);
		debug_assert!(handlers[1].1.has_modrm);
		debug_assert!(handlers[2].1.has_modrm);
		(OpCodeHandler_VectorLength_EVEX_er::decode, Self { has_modrm: true, handlers })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		let mut index = decoder.state.vector_length;
		const _: () = assert!(StateFlags::B > 3);
		debug_assert!(decoder.state.mod_ <= 3);
		if ((decoder.state.flags & StateFlags::B) | decoder.state.mod_) == (StateFlags::B | 3) {
			index = VectorLength::L512;
		}
		let (decode, handler) = this.handlers[index as usize];
		(decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_V_H_Ev_er {
	has_modrm: bool,
	base_reg: Register,
	code_w0: Code,
	code_w1: Code,
	tuple_type_w0: TupleType,
	tuple_type_w1: TupleType,
}

impl OpCodeHandler_EVEX_V_H_Ev_er {
	#[inline]
	pub(in crate::decoder) fn new(
		base_reg: Register, code_w0: Code, code_w1: Code, tuple_type_w0: TupleType, tuple_type_w1: TupleType,
	) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_V_H_Ev_er::decode, Self { has_modrm: true, base_reg, code_w0, code_w1, tuple_type_w0, tuple_type_w1 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if (((decoder.state.flags & StateFlags::Z) | decoder.state.aaa) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		let gpr;
		let tuple_type;
		if (decoder.state.flags & decoder.is64b_mode_and_w) != 0 {
			instruction.set_code(this.code_w1);
			tuple_type = this.tuple_type_w1;
			gpr = Register::RAX as u32;
		} else {
			instruction.set_code(this.code_w0);
			tuple_type = this.tuple_type_w0;
			gpr = Register::EAX as u32;
		}
		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32
		);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			write_op2_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
			if (decoder.state.flags & StateFlags::B) != 0 {
				const _: () = assert!(RoundingControl::None as u32 == 0);
				const _: () = assert!(RoundingControl::RoundToNearest as u32 == 1);
				const _: () = assert!(RoundingControl::RoundDown as u32 == 2);
				const _: () = assert!(RoundingControl::RoundUp as u32 == 3);
				const _: () = assert!(RoundingControl::RoundTowardZero as u32 == 4);
				instruction_internal::internal_set_rounding_control(
					instruction,
					(decoder.state.vector_length as u32) + RoundingControl::RoundToNearest as u32,
				);
			}
		} else {
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
			instruction.set_op2_kind(OpKind::Memory);
			decoder.read_op_mem_tuple_type(instruction, tuple_type);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_V_H_Ev_Ib {
	has_modrm: bool,
	base_reg: Register,
	code_w0: Code,
	code_w1: Code,
	tuple_type_w0: TupleType,
	tuple_type_w1: TupleType,
}

impl OpCodeHandler_EVEX_V_H_Ev_Ib {
	#[inline]
	pub(in crate::decoder) fn new(
		base_reg: Register, code_w0: Code, code_w1: Code, tuple_type_w0: TupleType, tuple_type_w1: TupleType,
	) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_V_H_Ev_Ib::decode, Self { has_modrm: true, base_reg, code_w0, code_w1, tuple_type_w0, tuple_type_w1 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if (((decoder.state.flags & (StateFlags::B | StateFlags::Z)) | decoder.state.aaa) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		let gpr;
		if (decoder.state.flags & decoder.is64b_mode_and_w) != 0 {
			instruction.set_code(this.code_w1);
			gpr = Register::RAX as u32;
		} else {
			instruction.set_code(this.code_w0);
			gpr = Register::EAX as u32;
		}
		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32
		);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg as u32);
		instruction.set_op3_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			write_op2_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			instruction.set_op2_kind(OpKind::Memory);
			if (decoder.state.flags & decoder.is64b_mode_and_w) != 0 {
				decoder.read_op_mem_tuple_type(instruction, this.tuple_type_w1);
			} else {
				decoder.read_op_mem_tuple_type(instruction, this.tuple_type_w0);
			}
		}
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_Ed_V_Ib {
	has_modrm: bool,
	base_reg: Register,
	code32: Code,
	code64: Code,
	tuple_type32: TupleType,
	tuple_type64: TupleType,
}

impl OpCodeHandler_EVEX_Ed_V_Ib {
	#[inline]
	pub(in crate::decoder) fn new(
		base_reg: Register, code32: Code, code64: Code, tuple_type32: TupleType, tuple_type64: TupleType,
	) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_Ed_V_Ib::decode, Self { has_modrm: true, base_reg, code32, code64, tuple_type32, tuple_type64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if (((decoder.state.flags & (StateFlags::B | StateFlags::Z)) | decoder.state.vvvv_invalid_check | decoder.state.aaa)
			& decoder.invalid_check_mask)
			!= 0
		{
			decoder.set_invalid_instruction();
		}
		write_op1_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32
		);
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
			instruction.set_op0_kind(OpKind::Memory);
			if (decoder.state.flags & decoder.is64b_mode_and_w) != 0 {
				decoder.read_op_mem_tuple_type(instruction, this.tuple_type64);
			} else {
				decoder.read_op_mem_tuple_type(instruction, this.tuple_type32);
			}
		}
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_VkHW_er {
	has_modrm: bool,
	tuple_type: TupleType,
	code: Code,
	base_reg: Register,
	only_sae: bool,
	can_broadcast: bool,
}

impl OpCodeHandler_EVEX_VkHW_er {
	#[inline]
	pub(in crate::decoder) fn new(
		base_reg: Register, code: Code, tuple_type: TupleType, only_sae: bool, can_broadcast: bool,
	) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_VkHW_er::decode, Self { has_modrm: true, base_reg, code, tuple_type, only_sae, can_broadcast })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		instruction.set_code(this.code);

		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32
		);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			write_op2_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg as u32);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.only_sae {
					instruction.set_suppress_all_exceptions(true);
				} else {
					const _: () = assert!(RoundingControl::None as u32 == 0);
					const _: () = assert!(RoundingControl::RoundToNearest as u32 == 1);
					const _: () = assert!(RoundingControl::RoundDown as u32 == 2);
					const _: () = assert!(RoundingControl::RoundUp as u32 == 3);
					const _: () = assert!(RoundingControl::RoundTowardZero as u32 == 4);
					instruction_internal::internal_set_rounding_control(
						instruction,
						(decoder.state.vector_length as u32) + RoundingControl::RoundToNearest as u32,
					);
				}
			}
		} else {
			instruction.set_op2_kind(OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.can_broadcast {
					instruction.set_is_broadcast(true);
				} else if decoder.invalid_check_mask != 0 {
					decoder.set_invalid_instruction();
				}
			}
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_VkHW_er_ur {
	has_modrm: bool,
	tuple_type: TupleType,
	code: Code,
	base_reg: Register,
	can_broadcast: bool,
}

impl OpCodeHandler_EVEX_VkHW_er_ur {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code, tuple_type: TupleType, can_broadcast: bool) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_VkHW_er_ur::decode, Self { has_modrm: true, base_reg, code, tuple_type, can_broadcast })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		instruction.set_code(this.code);

		let reg_num0 = decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex;
		write_op0_reg!(instruction, reg_num0 + this.base_reg as u32);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			let reg_num2 = decoder.state.rm + decoder.state.extra_base_register_base_evex;
			write_op2_reg!(instruction, reg_num2 + this.base_reg as u32);
			if decoder.invalid_check_mask != 0 && (reg_num0 == decoder.state.vvvv || reg_num0 == reg_num2) {
				decoder.set_invalid_instruction();
			}
			if (decoder.state.flags & StateFlags::B) != 0 {
				const _: () = assert!(RoundingControl::None as u32 == 0);
				const _: () = assert!(RoundingControl::RoundToNearest as u32 == 1);
				const _: () = assert!(RoundingControl::RoundDown as u32 == 2);
				const _: () = assert!(RoundingControl::RoundUp as u32 == 3);
				const _: () = assert!(RoundingControl::RoundTowardZero as u32 == 4);
				instruction_internal::internal_set_rounding_control(
					instruction,
					(decoder.state.vector_length as u32) + RoundingControl::RoundToNearest as u32,
				);
			}
		} else {
			if decoder.invalid_check_mask != 0 && reg_num0 == decoder.state.vvvv {
				decoder.set_invalid_instruction();
			}
			instruction.set_op2_kind(OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.can_broadcast {
					instruction.set_is_broadcast(true);
				} else if decoder.invalid_check_mask != 0 {
					decoder.set_invalid_instruction();
				}
			}
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_VkW_er {
	has_modrm: bool,
	tuple_type: TupleType,
	code: Code,
	base_reg1: Register,
	base_reg2: Register,
	only_sae: bool,
	can_broadcast: bool,
}

impl OpCodeHandler_EVEX_VkW_er {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code, tuple_type: TupleType, only_sae: bool) -> (OpCodeHandlerDecodeFn, Self) {
		(
			OpCodeHandler_EVEX_VkW_er::decode,
			Self { has_modrm: true, base_reg1: base_reg, base_reg2: base_reg, code, tuple_type, only_sae, can_broadcast: true },
		)
	}

	#[inline]
	pub(in crate::decoder) fn new1(
		base_reg1: Register, base_reg2: Register, code: Code, tuple_type: TupleType, only_sae: bool,
	) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_VkW_er::decode, Self { has_modrm: true, base_reg1, base_reg2, code, tuple_type, only_sae, can_broadcast: true })
	}

	#[inline]
	pub(in crate::decoder) fn new2(
		base_reg1: Register, base_reg2: Register, code: Code, tuple_type: TupleType, only_sae: bool, can_broadcast: bool,
	) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_VkW_er::decode, Self { has_modrm: true, base_reg1, base_reg2, code, tuple_type, only_sae, can_broadcast })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);

		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32
		);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg2 as u32);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.only_sae {
					instruction.set_suppress_all_exceptions(true);
				} else {
					const _: () = assert!(RoundingControl::None as u32 == 0);
					const _: () = assert!(RoundingControl::RoundToNearest as u32 == 1);
					const _: () = assert!(RoundingControl::RoundDown as u32 == 2);
					const _: () = assert!(RoundingControl::RoundUp as u32 == 3);
					const _: () = assert!(RoundingControl::RoundTowardZero as u32 == 4);
					instruction_internal::internal_set_rounding_control(
						instruction,
						(decoder.state.vector_length as u32) + RoundingControl::RoundToNearest as u32,
					);
				}
			}
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.can_broadcast {
					instruction.set_is_broadcast(true);
				} else if decoder.invalid_check_mask != 0 {
					decoder.set_invalid_instruction();
				}
			}
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_VkWIb_er {
	has_modrm: bool,
	tuple_type: TupleType,
	code: Code,
	base_reg1: Register,
	base_reg2: Register,
}

impl OpCodeHandler_EVEX_VkWIb_er {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code, tuple_type: TupleType) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_VkWIb_er::decode, Self { has_modrm: true, base_reg1: base_reg, base_reg2: base_reg, code, tuple_type })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);
		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32
		);
		instruction.set_op2_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg2 as u32);
			if (decoder.state.flags & StateFlags::B) != 0 {
				instruction.set_suppress_all_exceptions(true);
			}
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				instruction.set_is_broadcast(true);
			}
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_VkW {
	has_modrm: bool,
	tuple_type: TupleType,
	code: Code,
	base_reg1: Register,
	base_reg2: Register,
	can_broadcast: bool,
}

impl OpCodeHandler_EVEX_VkW {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code, tuple_type: TupleType, can_broadcast: bool) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_VkW::decode, Self { has_modrm: true, base_reg1: base_reg, base_reg2: base_reg, code, tuple_type, can_broadcast })
	}

	#[inline]
	pub(in crate::decoder) fn new1(
		base_reg1: Register, base_reg2: Register, code: Code, tuple_type: TupleType, can_broadcast: bool,
	) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_VkW::decode, Self { has_modrm: true, base_reg1, base_reg2, code, tuple_type, can_broadcast })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);

		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32
		);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg2 as u32);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.can_broadcast {
					instruction.set_is_broadcast(true);
				} else if decoder.invalid_check_mask != 0 {
					decoder.set_invalid_instruction();
				}
			}
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_WkV {
	has_modrm: bool,
	tuple_type: TupleType,
	code: Code,
	disallow_zeroing_masking: u32,
	base_reg1: Register,
	base_reg2: Register,
}

impl OpCodeHandler_EVEX_WkV {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code, tuple_type: TupleType) -> (OpCodeHandlerDecodeFn, Self) {
		(
			OpCodeHandler_EVEX_WkV::decode,
			Self { has_modrm: true, base_reg1: base_reg, base_reg2: base_reg, code, tuple_type, disallow_zeroing_masking: 0 },
		)
	}

	#[inline]
	pub(in crate::decoder) fn new1(
		base_reg: Register, code: Code, tuple_type: TupleType, allow_zeroing_masking: bool,
	) -> (OpCodeHandlerDecodeFn, Self) {
		(
			OpCodeHandler_EVEX_WkV::decode,
			Self {
				has_modrm: true,
				base_reg1: base_reg,
				base_reg2: base_reg,
				code,
				tuple_type,
				disallow_zeroing_masking: if allow_zeroing_masking { 0 } else { u32::MAX },
			},
		)
	}

	#[inline]
	pub(in crate::decoder) fn new2(base_reg1: Register, base_reg2: Register, code: Code, tuple_type: TupleType) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_WkV::decode, Self { has_modrm: true, base_reg1, base_reg2, code, tuple_type, disallow_zeroing_masking: 0 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if (((decoder.state.flags & StateFlags::B) | decoder.state.vvvv_invalid_check) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);

		write_op1_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg2 as u32
		);
		if ((decoder.state.flags & StateFlags::Z) & this.disallow_zeroing_masking & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		if decoder.state.mod_ == 3 {
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg1 as u32);
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			if ((decoder.state.flags & StateFlags::Z) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_VkM {
	has_modrm: bool,
	tuple_type: TupleType,
	code: Code,
	base_reg: Register,
}

impl OpCodeHandler_EVEX_VkM {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code, tuple_type: TupleType) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_VkM::decode, Self { has_modrm: true, base_reg, code, tuple_type })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if (((decoder.state.flags & StateFlags::B) | decoder.state.vvvv_invalid_check) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);

		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32
		);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_VkWIb {
	has_modrm: bool,
	tuple_type: TupleType,
	code: Code,
	base_reg1: Register,
	base_reg2: Register,
	can_broadcast: bool,
}

impl OpCodeHandler_EVEX_VkWIb {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code, tuple_type: TupleType, can_broadcast: bool) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_VkWIb::decode, Self { has_modrm: true, base_reg1: base_reg, base_reg2: base_reg, code, tuple_type, can_broadcast })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);

		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32
		);
		instruction.set_op2_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg2 as u32);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.can_broadcast {
					instruction.set_is_broadcast(true);
				} else if decoder.invalid_check_mask != 0 {
					decoder.set_invalid_instruction();
				}
			}
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_WkVIb {
	has_modrm: bool,
	tuple_type: TupleType,
	code: Code,
	base_reg1: Register,
	base_reg2: Register,
}

impl OpCodeHandler_EVEX_WkVIb {
	#[inline]
	pub(in crate::decoder) fn new(base_reg1: Register, base_reg2: Register, code: Code, tuple_type: TupleType) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_WkVIb::decode, Self { has_modrm: true, base_reg1, base_reg2, code, tuple_type })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if (((decoder.state.flags & StateFlags::B) | decoder.state.vvvv_invalid_check) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);
		write_op1_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg2 as u32
		);
		instruction.set_op2_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg1 as u32);
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			if ((decoder.state.flags & StateFlags::Z) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_HkWIb {
	has_modrm: bool,
	can_broadcast: bool,
	code: Code,
	base_reg1: Register,
	base_reg2: Register,
	tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_HkWIb {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code, tuple_type: TupleType, can_broadcast: bool) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_HkWIb::decode, Self { has_modrm: true, base_reg1: base_reg, base_reg2: base_reg, code, tuple_type, can_broadcast })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		instruction.set_code(this.code);

		write_op0_reg!(instruction, decoder.state.vvvv + this.base_reg1 as u32);
		instruction.set_op2_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg2 as u32);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.can_broadcast {
					instruction.set_is_broadcast(true);
				} else if decoder.invalid_check_mask != 0 {
					decoder.set_invalid_instruction();
				}
			}
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_HWIb {
	has_modrm: bool,
	tuple_type: TupleType,
	code: Code,
	base_reg1: Register,
	base_reg2: Register,
}

impl OpCodeHandler_EVEX_HWIb {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code, tuple_type: TupleType) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_HWIb::decode, Self { has_modrm: true, base_reg1: base_reg, base_reg2: base_reg, code, tuple_type })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		instruction.set_code(this.code);
		if (((decoder.state.flags & (StateFlags::Z | StateFlags::B)) | decoder.state.aaa) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}

		write_op0_reg!(instruction, decoder.state.vvvv + this.base_reg1 as u32);
		instruction.set_op2_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg2 as u32);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_WkVIb_er {
	has_modrm: bool,
	tuple_type: TupleType,
	code: Code,
	base_reg1: Register,
	base_reg2: Register,
}

impl OpCodeHandler_EVEX_WkVIb_er {
	#[inline]
	pub(in crate::decoder) fn new(base_reg1: Register, base_reg2: Register, code: Code, tuple_type: TupleType) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_WkVIb_er::decode, Self { has_modrm: true, base_reg1, base_reg2, code, tuple_type })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);

		write_op1_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg2 as u32
		);
		instruction.set_op2_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg1 as u32);
			if (decoder.state.flags & StateFlags::B) != 0 {
				instruction.set_suppress_all_exceptions(true);
			}
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			if ((decoder.state.flags & (StateFlags::B | StateFlags::Z)) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_VW_er {
	has_modrm: bool,
	tuple_type: TupleType,
	code: Code,
	base_reg1: Register,
	base_reg2: Register,
}

impl OpCodeHandler_EVEX_VW_er {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code, tuple_type: TupleType) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_VW_er::decode, Self { has_modrm: true, base_reg1: base_reg, base_reg2: base_reg, code, tuple_type })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		instruction.set_code(this.code);

		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32
		);
		if (((decoder.state.flags & StateFlags::Z) | decoder.state.vvvv_invalid_check | decoder.state.aaa) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg2 as u32);
			if (decoder.state.flags & StateFlags::B) != 0 {
				instruction.set_suppress_all_exceptions(true);
			}
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_VW {
	has_modrm: bool,
	tuple_type: TupleType,
	code: Code,
	base_reg1: Register,
	base_reg2: Register,
}

impl OpCodeHandler_EVEX_VW {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code, tuple_type: TupleType) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_VW::decode, Self { has_modrm: true, base_reg1: base_reg, base_reg2: base_reg, code, tuple_type })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if (((decoder.state.flags & (StateFlags::Z | StateFlags::B)) | decoder.state.vvvv_invalid_check | decoder.state.aaa)
			& decoder.invalid_check_mask)
			!= 0
		{
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);

		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32
		);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg2 as u32);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_WV {
	has_modrm: bool,
	tuple_type: TupleType,
	code: Code,
	base_reg1: Register,
	base_reg2: Register,
}

impl OpCodeHandler_EVEX_WV {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code, tuple_type: TupleType) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_WV::decode, Self { has_modrm: true, base_reg1: base_reg, base_reg2: base_reg, code, tuple_type })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if (((decoder.state.flags & (StateFlags::Z | StateFlags::B)) | decoder.state.vvvv_invalid_check | decoder.state.aaa)
			& decoder.invalid_check_mask)
			!= 0
		{
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);

		write_op1_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32
		);
		if decoder.state.mod_ == 3 {
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg2 as u32);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_VM {
	has_modrm: bool,
	tuple_type: TupleType,
	code: Code,
	base_reg: Register,
}

impl OpCodeHandler_EVEX_VM {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code, tuple_type: TupleType) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_VM::decode, Self { has_modrm: true, base_reg, code, tuple_type })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if (((decoder.state.flags & (StateFlags::Z | StateFlags::B)) | decoder.state.vvvv_invalid_check | decoder.state.aaa)
			& decoder.invalid_check_mask)
			!= 0
		{
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);

		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32
		);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_VK {
	has_modrm: bool,
	base_reg: Register,
	code: Code,
}

impl OpCodeHandler_EVEX_VK {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_VK::decode, Self { has_modrm: true, base_reg, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if (((decoder.state.flags & (StateFlags::B | StateFlags::Z)) | decoder.state.vvvv_invalid_check | decoder.state.aaa)
			& decoder.invalid_check_mask)
			!= 0
		{
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);

		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32
		);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + Register::K0 as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_KR {
	has_modrm: bool,
	base_reg: Register,
	code: Code,
}

impl OpCodeHandler_EVEX_KR {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_KR::decode, Self { has_modrm: true, base_reg, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if (((decoder.state.flags & (StateFlags::B | StateFlags::Z))
			| decoder.state.vvvv_invalid_check
			| decoder.state.aaa
			| decoder.state.extra_register_base
			| decoder.state.extra_register_base_evex)
			& decoder.invalid_check_mask)
			!= 0
		{
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);

		write_op0_reg!(instruction, decoder.state.reg + Register::K0 as u32);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_KkHWIb_sae {
	has_modrm: bool,
	tuple_type: TupleType,
	code: Code,
	base_reg: Register,
	can_broadcast: bool,
}

impl OpCodeHandler_EVEX_KkHWIb_sae {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code, tuple_type: TupleType, can_broadcast: bool) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_KkHWIb_sae::decode, Self { has_modrm: true, base_reg, code, tuple_type, can_broadcast })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if (((decoder.state.flags & StateFlags::Z) | decoder.state.extra_register_base | decoder.state.extra_register_base_evex)
			& decoder.invalid_check_mask)
			!= 0
		{
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);

		write_op0_reg!(instruction, decoder.state.reg + Register::K0 as u32);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg as u32);
		instruction.set_op3_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			write_op2_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg as u32);
			if (decoder.state.flags & StateFlags::B) != 0 {
				instruction.set_suppress_all_exceptions(true);
			}
		} else {
			instruction.set_op2_kind(OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.can_broadcast {
					instruction.set_is_broadcast(true);
				} else if decoder.invalid_check_mask != 0 {
					decoder.set_invalid_instruction();
				}
			}
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_VkHW {
	has_modrm: bool,
	tuple_type: TupleType,
	code: Code,
	base_reg1: Register,
	base_reg2: Register,
	base_reg3: Register,
	can_broadcast: bool,
}

impl OpCodeHandler_EVEX_VkHW {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code, tuple_type: TupleType, can_broadcast: bool) -> (OpCodeHandlerDecodeFn, Self) {
		(
			OpCodeHandler_EVEX_VkHW::decode,
			Self { has_modrm: true, base_reg1: base_reg, base_reg2: base_reg, base_reg3: base_reg, code, tuple_type, can_broadcast },
		)
	}

	#[inline]
	pub(in crate::decoder) fn new1(
		base_reg1: Register, base_reg2: Register, base_reg3: Register, code: Code, tuple_type: TupleType, can_broadcast: bool,
	) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_VkHW::decode, Self { has_modrm: true, base_reg1, base_reg2, base_reg3, code, tuple_type, can_broadcast })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		instruction.set_code(this.code);

		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32
		);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg2 as u32);
		if decoder.state.mod_ == 3 {
			write_op2_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg3 as u32);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			instruction.set_op2_kind(OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.can_broadcast {
					instruction.set_is_broadcast(true);
				} else if decoder.invalid_check_mask != 0 {
					decoder.set_invalid_instruction();
				}
			}
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_VkHM {
	has_modrm: bool,
	tuple_type: TupleType,
	code: Code,
	base_reg1: Register,
	base_reg2: Register,
}

impl OpCodeHandler_EVEX_VkHM {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code, tuple_type: TupleType) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_VkHM::decode, Self { has_modrm: true, base_reg1: base_reg, base_reg2: base_reg, code, tuple_type })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		instruction.set_code(this.code);

		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32
		);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg2 as u32);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			instruction.set_op2_kind(OpKind::Memory);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_VkHWIb {
	has_modrm: bool,
	tuple_type: TupleType,
	code: Code,
	base_reg1: Register,
	base_reg2: Register,
	base_reg3: Register,
	can_broadcast: bool,
}

impl OpCodeHandler_EVEX_VkHWIb {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code, tuple_type: TupleType, can_broadcast: bool) -> (OpCodeHandlerDecodeFn, Self) {
		(
			OpCodeHandler_EVEX_VkHWIb::decode,
			Self { has_modrm: true, base_reg1: base_reg, base_reg2: base_reg, base_reg3: base_reg, code, tuple_type, can_broadcast },
		)
	}

	#[inline]
	pub(in crate::decoder) fn new1(
		base_reg1: Register, base_reg2: Register, base_reg3: Register, code: Code, tuple_type: TupleType, can_broadcast: bool,
	) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_VkHWIb::decode, Self { has_modrm: true, base_reg1, base_reg2, base_reg3, code, tuple_type, can_broadcast })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		instruction.set_code(this.code);

		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32
		);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg2 as u32);
		instruction.set_op3_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			write_op2_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg3 as u32);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			instruction.set_op2_kind(OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.can_broadcast {
					instruction.set_is_broadcast(true);
				} else if decoder.invalid_check_mask != 0 {
					decoder.set_invalid_instruction();
				}
			}
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_VkHWIb_er {
	has_modrm: bool,
	tuple_type: TupleType,
	code: Code,
	base_reg1: Register,
	base_reg2: Register,
	base_reg3: Register,
	can_broadcast: bool,
}

impl OpCodeHandler_EVEX_VkHWIb_er {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code, tuple_type: TupleType, can_broadcast: bool) -> (OpCodeHandlerDecodeFn, Self) {
		(
			OpCodeHandler_EVEX_VkHWIb_er::decode,
			Self { has_modrm: true, base_reg1: base_reg, base_reg2: base_reg, base_reg3: base_reg, code, tuple_type, can_broadcast },
		)
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		instruction.set_code(this.code);

		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32
		);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg2 as u32);
		instruction.set_op3_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			write_op2_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg3 as u32);
			if (decoder.state.flags & StateFlags::B) != 0 {
				instruction.set_suppress_all_exceptions(true);
			}
		} else {
			instruction.set_op2_kind(OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.can_broadcast {
					instruction.set_is_broadcast(true);
				} else if decoder.invalid_check_mask != 0 {
					decoder.set_invalid_instruction();
				}
			}
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_KkHW {
	has_modrm: bool,
	tuple_type: TupleType,
	code: Code,
	base_reg: Register,
	can_broadcast: bool,
}

impl OpCodeHandler_EVEX_KkHW {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code, tuple_type: TupleType, can_broadcast: bool) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_KkHW::decode, Self { has_modrm: true, base_reg, code, tuple_type, can_broadcast })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		instruction.set_code(this.code);

		write_op0_reg!(instruction, decoder.state.reg + Register::K0 as u32);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg as u32);
		if (((decoder.state.flags & StateFlags::Z) | decoder.state.extra_register_base | decoder.state.extra_register_base_evex)
			& decoder.invalid_check_mask)
			!= 0
		{
			decoder.set_invalid_instruction();
		}
		if decoder.state.mod_ == 3 {
			write_op2_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg as u32);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			instruction.set_op2_kind(OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.can_broadcast {
					instruction.set_is_broadcast(true);
				} else if decoder.invalid_check_mask != 0 {
					decoder.set_invalid_instruction();
				}
			}
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_KP1HW {
	has_modrm: bool,
	tuple_type: TupleType,
	code: Code,
	base_reg: Register,
}

impl OpCodeHandler_EVEX_KP1HW {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code, tuple_type: TupleType) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_KP1HW::decode, Self { has_modrm: true, base_reg, code, tuple_type })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		instruction.set_code(this.code);

		write_op0_reg!(instruction, decoder.state.reg + Register::K0 as u32);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg as u32);
		if (((decoder.state.flags & StateFlags::Z) | decoder.state.aaa | decoder.state.extra_register_base | decoder.state.extra_register_base_evex)
			& decoder.invalid_check_mask)
			!= 0
		{
			decoder.set_invalid_instruction();
		}
		if decoder.state.mod_ == 3 {
			write_op2_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg as u32);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			instruction.set_op2_kind(OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				instruction.set_is_broadcast(true);
			}
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_KkHWIb {
	has_modrm: bool,
	tuple_type: TupleType,
	code: Code,
	base_reg: Register,
	can_broadcast: bool,
}

impl OpCodeHandler_EVEX_KkHWIb {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code, tuple_type: TupleType, can_broadcast: bool) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_KkHWIb::decode, Self { has_modrm: true, base_reg, code, tuple_type, can_broadcast })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		instruction.set_code(this.code);

		write_op0_reg!(instruction, decoder.state.reg + Register::K0 as u32);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			write_op2_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg as u32);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			instruction.set_op2_kind(OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.can_broadcast {
					instruction.set_is_broadcast(true);
				} else if decoder.invalid_check_mask != 0 {
					decoder.set_invalid_instruction();
				}
			}
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
		instruction.set_op3_kind(OpKind::Immediate8);
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
		if (((decoder.state.flags & StateFlags::Z) | decoder.state.extra_register_base | decoder.state.extra_register_base_evex)
			& decoder.invalid_check_mask)
			!= 0
		{
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_WkHV {
	has_modrm: bool,
	base_reg: Register,
	code: Code,
}

impl OpCodeHandler_EVEX_WkHV {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_WkHV::decode, Self { has_modrm: true, base_reg, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		instruction.set_code(this.code);

		debug_assert_eq!(decoder.state.mod_, 3);
		write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg as u32);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg as u32);
		write_op2_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32
		);
		if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_VHWIb {
	has_modrm: bool,
	tuple_type: TupleType,
	code: Code,
	base_reg: Register,
}

impl OpCodeHandler_EVEX_VHWIb {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code, tuple_type: TupleType) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_VHWIb::decode, Self { has_modrm: true, base_reg, code, tuple_type })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if (((decoder.state.flags & (StateFlags::B | StateFlags::Z)) | decoder.state.aaa) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);

		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32
		);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg as u32);
		instruction.set_op3_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			write_op2_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg as u32);
		} else {
			instruction.set_op2_kind(OpKind::Memory);
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_VHW {
	has_modrm: bool,
	tuple_type: TupleType,
	code_r: Code,
	code_m: Code,
	base_reg1: Register,
	base_reg2: Register,
	base_reg3: Register,
}

impl OpCodeHandler_EVEX_VHW {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code_r: Code, code_m: Code, tuple_type: TupleType) -> (OpCodeHandlerDecodeFn, Self) {
		(
			OpCodeHandler_EVEX_VHW::decode,
			Self { has_modrm: true, base_reg1: base_reg, base_reg2: base_reg, base_reg3: base_reg, code_r, code_m, tuple_type },
		)
	}

	#[inline]
	pub(in crate::decoder) fn new2(base_reg: Register, code: Code, tuple_type: TupleType) -> (OpCodeHandlerDecodeFn, Self) {
		(
			OpCodeHandler_EVEX_VHW::decode,
			Self { has_modrm: true, base_reg1: base_reg, base_reg2: base_reg, base_reg3: base_reg, code_r: code, code_m: code, tuple_type },
		)
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if (((decoder.state.flags & (StateFlags::B | StateFlags::Z)) | decoder.state.aaa) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}

		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32
		);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg2 as u32);
		if decoder.state.mod_ == 3 {
			instruction.set_code(this.code_r);
			write_op2_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg3 as u32);
		} else {
			instruction.set_code(this.code_m);
			instruction.set_op2_kind(OpKind::Memory);
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_VHM {
	has_modrm: bool,
	tuple_type: TupleType,
	code: Code,
	base_reg: Register,
}

impl OpCodeHandler_EVEX_VHM {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code, tuple_type: TupleType) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_VHM::decode, Self { has_modrm: true, base_reg, code, tuple_type })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if (((decoder.state.flags & (StateFlags::B | StateFlags::Z)) | decoder.state.aaa) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);

		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32
		);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			instruction.set_op2_kind(OpKind::Memory);
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_Gv_W_er {
	has_modrm: bool,
	tuple_type: TupleType,
	code_w0: Code,
	code_w1: Code,
	base_reg: Register,
	only_sae: bool,
}

impl OpCodeHandler_EVEX_Gv_W_er {
	#[inline]
	pub(in crate::decoder) fn new(
		base_reg: Register, code_w0: Code, code_w1: Code, tuple_type: TupleType, only_sae: bool,
	) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_Gv_W_er::decode, Self { has_modrm: true, base_reg, code_w0, code_w1, tuple_type, only_sae })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if (((decoder.state.flags & StateFlags::Z) | decoder.state.vvvv_invalid_check | decoder.state.aaa | decoder.state.extra_register_base_evex)
			& decoder.invalid_check_mask)
			!= 0
		{
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
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg as u32);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.only_sae {
					instruction.set_suppress_all_exceptions(true);
				} else {
					const _: () = assert!(RoundingControl::None as u32 == 0);
					const _: () = assert!(RoundingControl::RoundToNearest as u32 == 1);
					const _: () = assert!(RoundingControl::RoundDown as u32 == 2);
					const _: () = assert!(RoundingControl::RoundUp as u32 == 3);
					const _: () = assert!(RoundingControl::RoundTowardZero as u32 == 4);
					instruction_internal::internal_set_rounding_control(
						instruction,
						(decoder.state.vector_length as u32) + RoundingControl::RoundToNearest as u32,
					);
				}
			}
		} else {
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_VX_Ev {
	has_modrm: bool,
	code32: Code,
	code64: Code,
	tuple_type_w0: TupleType,
	tuple_type_w1: TupleType,
}

impl OpCodeHandler_EVEX_VX_Ev {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code, tuple_type_w0: TupleType, tuple_type_w1: TupleType) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_VX_Ev::decode, Self { has_modrm: true, code32, code64, tuple_type_w0, tuple_type_w1 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if (((decoder.state.flags & (StateFlags::B | StateFlags::Z)) | decoder.state.vvvv_invalid_check | decoder.state.aaa)
			& decoder.invalid_check_mask)
			!= 0
		{
			decoder.set_invalid_instruction();
		}
		let gpr;
		let tuple_type;
		if (decoder.state.flags & decoder.is64b_mode_and_w) != 0 {
			instruction.set_code(this.code64);
			tuple_type = this.tuple_type_w1;
			gpr = Register::RAX as u32;
		} else {
			instruction.set_code(this.code32);
			tuple_type = this.tuple_type_w0;
			gpr = Register::EAX as u32;
		}
		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + Register::XMM0 as u32
		);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem_tuple_type(instruction, tuple_type);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_Ev_VX {
	has_modrm: bool,
	code32: Code,
	code64: Code,
	tuple_type_w0: TupleType,
	tuple_type_w1: TupleType,
}

impl OpCodeHandler_EVEX_Ev_VX {
	#[inline]
	pub(in crate::decoder) fn new(code32: Code, code64: Code, tuple_type_w0: TupleType, tuple_type_w1: TupleType) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_Ev_VX::decode, Self { has_modrm: true, code32, code64, tuple_type_w0, tuple_type_w1 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if (((decoder.state.flags & (StateFlags::B | StateFlags::Z)) | decoder.state.vvvv_invalid_check | decoder.state.aaa)
			& decoder.invalid_check_mask)
			!= 0
		{
			decoder.set_invalid_instruction();
		}
		let gpr;
		let tuple_type;
		if (decoder.state.flags & decoder.is64b_mode_and_w) != 0 {
			instruction.set_code(this.code64);
			tuple_type = this.tuple_type_w1;
			gpr = Register::RAX as u32;
		} else {
			instruction.set_code(this.code32);
			tuple_type = this.tuple_type_w0;
			gpr = Register::EAX as u32;
		}
		write_op1_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + Register::XMM0 as u32
		);
		if decoder.state.mod_ == 3 {
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem_tuple_type(instruction, tuple_type);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_Ev_VX_Ib {
	has_modrm: bool,
	base_reg: Register,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_EVEX_Ev_VX_Ib {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_Ev_VX_Ib::decode, Self { has_modrm: true, base_reg, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if (((decoder.state.flags & (StateFlags::B | StateFlags::Z))
			| decoder.state.vvvv_invalid_check
			| decoder.state.aaa
			| decoder.state.extra_register_base_evex)
			& decoder.invalid_check_mask)
			!= 0
		{
			decoder.set_invalid_instruction();
		}
		debug_assert_eq!(decoder.state.mod_, 3);
		write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg as u32);
		instruction.set_op2_kind(OpKind::Immediate8);
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
		if (decoder.state.flags & decoder.is64b_mode_and_w) != 0 {
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
pub(in crate::decoder) struct OpCodeHandler_EVEX_MV {
	has_modrm: bool,
	tuple_type: TupleType,
	code: Code,
	base_reg: Register,
}

impl OpCodeHandler_EVEX_MV {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code, tuple_type: TupleType) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_MV::decode, Self { has_modrm: true, base_reg, code, tuple_type })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if (((decoder.state.flags & (StateFlags::B | StateFlags::Z)) | decoder.state.vvvv_invalid_check | decoder.state.aaa)
			& decoder.invalid_check_mask)
			!= 0
		{
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);
		write_op1_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32
		);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_VkEv_REXW {
	has_modrm: bool,
	base_reg: Register,
	code32: Code,
	code64: Code,
}

impl OpCodeHandler_EVEX_VkEv_REXW {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code32: Code, code64: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_VkEv_REXW::decode, Self { has_modrm: true, base_reg, code32, code64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if (((decoder.state.flags & StateFlags::B) | decoder.state.vvvv_invalid_check) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		let gpr;
		if (decoder.state.flags & decoder.is64b_mode_and_w) != 0 {
			debug_assert_ne!(this.code64, Code::INVALID);
			instruction.set_code(this.code64);
			gpr = Register::RAX as u32;
		} else {
			instruction.set_code(this.code32);
			gpr = Register::EAX as u32;
		}

		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32
		);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_Vk_VSIB {
	has_modrm: bool,
	tuple_type: TupleType,
	code: Code,
	base_reg: Register,
	vsib_base: Register,
}

impl OpCodeHandler_EVEX_Vk_VSIB {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, vsib_base: Register, code: Code, tuple_type: TupleType) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_Vk_VSIB::decode, Self { has_modrm: true, base_reg, vsib_base, code, tuple_type })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if decoder.invalid_check_mask != 0
			&& (((decoder.state.flags & (StateFlags::Z | StateFlags::B)) | (decoder.state.vvvv_invalid_check & 0xF)) != 0 || decoder.state.aaa == 0)
		{
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);

		let reg_num = decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex;
		write_op0_reg!(instruction, reg_num + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem_vsib(instruction, this.vsib_base, this.tuple_type);
			if decoder.invalid_check_mask != 0 {
				if reg_num == ((instruction.memory_index() as u32).wrapping_sub(Register::XMM0 as u32) % IcedConstants::VMM_COUNT) {
					decoder.set_invalid_instruction();
				}
			}
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_VSIB_k1_VX {
	has_modrm: bool,
	tuple_type: TupleType,
	code: Code,
	vsib_index: Register,
	base_reg: Register,
}

impl OpCodeHandler_EVEX_VSIB_k1_VX {
	#[inline]
	pub(in crate::decoder) fn new(vsib_index: Register, base_reg: Register, code: Code, tuple_type: TupleType) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_VSIB_k1_VX::decode, Self { has_modrm: true, vsib_index, base_reg, code, tuple_type })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if decoder.invalid_check_mask != 0
			&& (((decoder.state.flags & (StateFlags::B | StateFlags::Z)) | (decoder.state.vvvv_invalid_check & 0xF)) != 0 || decoder.state.aaa == 0)
		{
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);

		write_op1_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32
		);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem_vsib(instruction, this.vsib_index, this.tuple_type);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_VSIB_k1 {
	has_modrm: bool,
	tuple_type: TupleType,
	code: Code,
	vsib_index: Register,
}

impl OpCodeHandler_EVEX_VSIB_k1 {
	#[inline]
	pub(in crate::decoder) fn new(vsib_index: Register, code: Code, tuple_type: TupleType) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_VSIB_k1::decode, Self { has_modrm: true, vsib_index, code, tuple_type })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if decoder.invalid_check_mask != 0
			&& (((decoder.state.flags & (StateFlags::B | StateFlags::Z)) | (decoder.state.vvvv_invalid_check & 0xF)) != 0 || decoder.state.aaa == 0)
		{
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);

		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem_vsib(instruction, this.vsib_index, this.tuple_type);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_GvM_VX_Ib {
	has_modrm: bool,
	base_reg: Register,
	code32: Code,
	code64: Code,
	tuple_type32: TupleType,
	tuple_type64: TupleType,
}

impl OpCodeHandler_EVEX_GvM_VX_Ib {
	#[inline]
	pub(in crate::decoder) fn new(
		base_reg: Register, code32: Code, code64: Code, tuple_type32: TupleType, tuple_type64: TupleType,
	) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_GvM_VX_Ib::decode, Self { has_modrm: true, base_reg, code32, code64, tuple_type32, tuple_type64 })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if (((decoder.state.flags & (StateFlags::B | StateFlags::Z)) | decoder.state.vvvv_invalid_check | decoder.state.aaa)
			& decoder.invalid_check_mask)
			!= 0
		{
			decoder.set_invalid_instruction();
		}
		write_op1_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32
		);
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
			instruction.set_op0_kind(OpKind::Memory);
			if (decoder.state.flags & decoder.is64b_mode_and_w) != 0 {
				decoder.read_op_mem_tuple_type(instruction, this.tuple_type64);
			} else {
				decoder.read_op_mem_tuple_type(instruction, this.tuple_type32);
			}
		}
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(in crate::decoder) struct OpCodeHandler_EVEX_KkWIb {
	has_modrm: bool,
	tuple_type: TupleType,
	code: Code,
	base_reg: Register,
	can_broadcast: bool,
}

impl OpCodeHandler_EVEX_KkWIb {
	#[inline]
	pub(in crate::decoder) fn new(base_reg: Register, code: Code, tuple_type: TupleType, can_broadcast: bool) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_EVEX_KkWIb::decode, Self { has_modrm: true, base_reg, code, tuple_type, can_broadcast })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if (((decoder.state.flags & StateFlags::Z)
			| decoder.state.vvvv_invalid_check
			| decoder.state.extra_register_base
			| decoder.state.extra_register_base_evex)
			& decoder.invalid_check_mask)
			!= 0
		{
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);

		write_op0_reg!(instruction, decoder.state.reg + Register::K0 as u32);
		instruction.set_op2_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg as u32);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.can_broadcast {
					instruction.set_is_broadcast(true);
				} else if decoder.invalid_check_mask != 0 {
					decoder.set_invalid_instruction();
				}
			}
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}
