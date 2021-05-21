// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#![allow(clippy::useless_let_if_seq)]

use crate::decoder::handlers::*;
use crate::decoder::*;
use crate::instruction_internal;
use crate::*;

// SAFETY:
//	code: let this = unsafe { &*(self_ptr as *const Self) };
// The first arg (`self_ptr`) to decode() is always the handler itself, cast to a `*const OpCodeHandler`.
// All handlers are `#[repr(C)]` structs so the OpCodeHandler fields are always at the same offsets.

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VectorLength_EVEX {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	handlers: [(&'static OpCodeHandler, OpCodeHandlerDecodeFn); 4],
}

impl OpCodeHandler_VectorLength_EVEX {
	#[allow(trivial_casts)]
	pub(super) fn new(
		handler128: (*const OpCodeHandler, OpCodeHandlerDecodeFn), handler256: (*const OpCodeHandler, OpCodeHandlerDecodeFn),
		handler512: (*const OpCodeHandler, OpCodeHandlerDecodeFn),
	) -> Self {
		const_assert_eq!(VectorLength::L128 as u32, 0);
		const_assert_eq!(VectorLength::L256 as u32, 1);
		const_assert_eq!(VectorLength::L512 as u32, 2);
		const_assert_eq!(VectorLength::Unknown as u32, 3);
		debug_assert!(!is_null_instance_handler(handler128.0));
		debug_assert!(!is_null_instance_handler(handler256.0));
		debug_assert!(!is_null_instance_handler(handler512.0));
		let handlers = unsafe {
			[
				(&*handler128.0, handler128.1),
				(&*handler256.0, handler256.1),
				(&*handler512.0, handler512.1),
				(&*(&INVALID_HANDLER as *const _ as *const OpCodeHandler), INVALID_HANDLER.decode),
			]
		};
		debug_assert!(handlers[0].0.has_modrm);
		debug_assert!(handlers[1].0.has_modrm);
		debug_assert!(handlers[2].0.has_modrm);
		Self { decode: OpCodeHandler_VectorLength_EVEX::decode, has_modrm: true, handlers }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		let (handler, decode) = this.handlers[decoder.state.vector_length as usize];
		(decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_VectorLength_EVEX_er {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	handlers: [(&'static OpCodeHandler, OpCodeHandlerDecodeFn); 4],
}

impl OpCodeHandler_VectorLength_EVEX_er {
	#[allow(trivial_casts)]
	pub(super) fn new(
		handler128: (*const OpCodeHandler, OpCodeHandlerDecodeFn), handler256: (*const OpCodeHandler, OpCodeHandlerDecodeFn),
		handler512: (*const OpCodeHandler, OpCodeHandlerDecodeFn),
	) -> Self {
		const_assert_eq!(VectorLength::L128 as u32, 0);
		const_assert_eq!(VectorLength::L256 as u32, 1);
		const_assert_eq!(VectorLength::L512 as u32, 2);
		const_assert_eq!(VectorLength::Unknown as u32, 3);
		debug_assert!(!is_null_instance_handler(handler128.0));
		debug_assert!(!is_null_instance_handler(handler256.0));
		debug_assert!(!is_null_instance_handler(handler512.0));
		let handlers = unsafe {
			[
				(&*handler128.0, handler128.1),
				(&*handler256.0, handler256.1),
				(&*handler512.0, handler512.1),
				(&*(&INVALID_HANDLER as *const _ as *const OpCodeHandler), INVALID_HANDLER.decode),
			]
		};
		debug_assert!(handlers[0].0.has_modrm);
		debug_assert!(handlers[1].0.has_modrm);
		debug_assert!(handlers[2].0.has_modrm);
		Self { decode: OpCodeHandler_VectorLength_EVEX_er::decode, has_modrm: true, handlers }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		let mut index = decoder.state.vector_length;
		if decoder.state.mod_ == 3 && (decoder.state.flags & StateFlags::B) != 0 {
			index = VectorLength::L512;
		}
		let (handler, decode) = this.handlers[index as usize];
		(decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_EVEX_V_H_Ev_er {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code_w0: u32,
	code_w1: u32,
	base_reg: Register,
	tuple_type_w0: TupleType,
	tuple_type_w1: TupleType,
}

impl OpCodeHandler_EVEX_V_H_Ev_er {
	pub(super) fn new(base_reg: Register, code_w0: u32, code_w1: u32, tuple_type_w0: TupleType, tuple_type_w1: TupleType) -> Self {
		Self { decode: OpCodeHandler_EVEX_V_H_Ev_er::decode, has_modrm: true, base_reg, code_w0, code_w1, tuple_type_w0, tuple_type_w1 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		if (((decoder.state.flags & StateFlags::Z) | decoder.state.aaa) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		let gpr;
		let tuple_type;
		if (decoder.state.flags & decoder.is64b_mode_and_w) != 0 {
			instruction_internal::internal_set_code_u32(instruction, this.code_w1);
			tuple_type = this.tuple_type_w1;
			gpr = Register::RAX as u32;
		} else {
			instruction_internal::internal_set_code_u32(instruction, this.code_w0);
			tuple_type = this.tuple_type_w0;
			gpr = Register::EAX as u32;
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32,
		);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op2_kind(OpKind::Register);
			instruction_internal::internal_set_op2_register_u32(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
			if (decoder.state.flags & StateFlags::B) != 0 {
				const_assert_eq!(RoundingControl::None as u32, 0);
				const_assert_eq!(RoundingControl::RoundToNearest as u32, 1);
				const_assert_eq!(RoundingControl::RoundDown as u32, 2);
				const_assert_eq!(RoundingControl::RoundUp as u32, 3);
				const_assert_eq!(RoundingControl::RoundTowardZero as u32, 4);
				instruction_internal::internal_set_rounding_control(instruction, (decoder.state.vector_length as u32) + 1);
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
pub(super) struct OpCodeHandler_EVEX_V_H_Ev_Ib {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code_w0: u32,
	code_w1: u32,
	base_reg: Register,
	tuple_type_w0: TupleType,
	tuple_type_w1: TupleType,
}

impl OpCodeHandler_EVEX_V_H_Ev_Ib {
	pub(super) fn new(base_reg: Register, code_w0: u32, code_w1: u32, tuple_type_w0: TupleType, tuple_type_w1: TupleType) -> Self {
		Self { decode: OpCodeHandler_EVEX_V_H_Ev_Ib::decode, has_modrm: true, base_reg, code_w0, code_w1, tuple_type_w0, tuple_type_w1 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		if (((decoder.state.flags & (StateFlags::B | StateFlags::Z)) | decoder.state.aaa) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		let gpr;
		if (decoder.state.flags & decoder.is64b_mode_and_w) != 0 {
			instruction_internal::internal_set_code_u32(instruction, this.code_w1);
			gpr = Register::RAX as u32;
		} else {
			instruction_internal::internal_set_code_u32(instruction, this.code_w0);
			gpr = Register::EAX as u32;
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32,
		);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg as u32);
		instruction.set_op3_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op2_kind(OpKind::Register);
			instruction_internal::internal_set_op2_register_u32(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
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
pub(super) struct OpCodeHandler_EVEX_Ed_V_Ib {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code32: u32,
	code64: u32,
	base_reg: Register,
	tuple_type32: TupleType,
	tuple_type64: TupleType,
}

impl OpCodeHandler_EVEX_Ed_V_Ib {
	pub(super) fn new(base_reg: Register, code32: u32, code64: u32, tuple_type32: TupleType, tuple_type64: TupleType) -> Self {
		Self { decode: OpCodeHandler_EVEX_Ed_V_Ib::decode, has_modrm: true, base_reg, code32, code64, tuple_type32, tuple_type64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		if (((decoder.state.flags & (StateFlags::B | StateFlags::Z)) | decoder.state.vvvv_invalid_check | decoder.state.aaa)
			& decoder.invalid_check_mask)
			!= 0
		{
			decoder.set_invalid_instruction();
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction_internal::internal_set_op1_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32,
		);
		instruction.set_op2_kind(OpKind::Immediate8);
		let gpr;
		if (decoder.state.flags & decoder.is64b_mode_and_w) != 0 {
			instruction_internal::internal_set_code_u32(instruction, this.code64);
			gpr = Register::RAX as u32;
		} else {
			instruction_internal::internal_set_code_u32(instruction, this.code32);
			gpr = Register::EAX as u32;
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
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
pub(super) struct OpCodeHandler_EVEX_VkHW_er {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg: Register,
	tuple_type: TupleType,
	only_sae: bool,
	can_broadcast: bool,
}

impl OpCodeHandler_EVEX_VkHW_er {
	pub(super) fn new(base_reg: Register, code: u32, tuple_type: TupleType, only_sae: bool, can_broadcast: bool) -> Self {
		Self { decode: OpCodeHandler_EVEX_VkHW_er::decode, has_modrm: true, base_reg, code, tuple_type, only_sae, can_broadcast }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32,
		);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op2_kind(OpKind::Register);
			instruction_internal::internal_set_op2_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg as u32,
			);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.only_sae {
					instruction_internal::internal_set_suppress_all_exceptions(instruction);
				} else {
					const_assert_eq!(RoundingControl::None as u32, 0);
					const_assert_eq!(RoundingControl::RoundToNearest as u32, 1);
					const_assert_eq!(RoundingControl::RoundDown as u32, 2);
					const_assert_eq!(RoundingControl::RoundUp as u32, 3);
					const_assert_eq!(RoundingControl::RoundTowardZero as u32, 4);
					instruction_internal::internal_set_rounding_control(instruction, (decoder.state.vector_length as u32) + 1);
				}
			}
		} else {
			instruction.set_op2_kind(OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.can_broadcast {
					instruction_internal::internal_set_is_broadcast(instruction);
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
pub(super) struct OpCodeHandler_EVEX_VkW_er {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg1: Register,
	base_reg2: Register,
	tuple_type: TupleType,
	only_sae: bool,
	can_broadcast: bool,
}

impl OpCodeHandler_EVEX_VkW_er {
	pub(super) fn new(base_reg: Register, code: u32, tuple_type: TupleType, only_sae: bool) -> Self {
		Self {
			decode: OpCodeHandler_EVEX_VkW_er::decode,
			has_modrm: true,
			base_reg1: base_reg,
			base_reg2: base_reg,
			code,
			tuple_type,
			only_sae,
			can_broadcast: true,
		}
	}

	pub(super) fn new1(base_reg1: Register, base_reg2: Register, code: u32, tuple_type: TupleType, only_sae: bool) -> Self {
		Self { decode: OpCodeHandler_EVEX_VkW_er::decode, has_modrm: true, base_reg1, base_reg2, code, tuple_type, only_sae, can_broadcast: true }
	}

	pub(super) fn new2(base_reg1: Register, base_reg2: Register, code: u32, tuple_type: TupleType, only_sae: bool, can_broadcast: bool) -> Self {
		Self { decode: OpCodeHandler_EVEX_VkW_er::decode, has_modrm: true, base_reg1, base_reg2, code, tuple_type, only_sae, can_broadcast }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32,
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg2 as u32,
			);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.only_sae {
					instruction_internal::internal_set_suppress_all_exceptions(instruction);
				} else {
					const_assert_eq!(RoundingControl::None as u32, 0);
					const_assert_eq!(RoundingControl::RoundToNearest as u32, 1);
					const_assert_eq!(RoundingControl::RoundDown as u32, 2);
					const_assert_eq!(RoundingControl::RoundUp as u32, 3);
					const_assert_eq!(RoundingControl::RoundTowardZero as u32, 4);
					instruction_internal::internal_set_rounding_control(instruction, (decoder.state.vector_length as u32) + 1);
				}
			}
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.can_broadcast {
					instruction_internal::internal_set_is_broadcast(instruction);
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
pub(super) struct OpCodeHandler_EVEX_VkWIb_er {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg1: Register,
	base_reg2: Register,
	tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_VkWIb_er {
	pub(super) fn new(base_reg: Register, code: u32, tuple_type: TupleType) -> Self {
		Self { decode: OpCodeHandler_EVEX_VkWIb_er::decode, has_modrm: true, base_reg1: base_reg, base_reg2: base_reg, code, tuple_type }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32,
		);
		instruction.set_op2_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg2 as u32,
			);
			if (decoder.state.flags & StateFlags::B) != 0 {
				instruction_internal::internal_set_suppress_all_exceptions(instruction);
			}
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				instruction_internal::internal_set_is_broadcast(instruction);
			}
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_EVEX_VkW {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg1: Register,
	base_reg2: Register,
	tuple_type: TupleType,
	can_broadcast: bool,
}

impl OpCodeHandler_EVEX_VkW {
	pub(super) fn new(base_reg: Register, code: u32, tuple_type: TupleType, can_broadcast: bool) -> Self {
		Self { decode: OpCodeHandler_EVEX_VkW::decode, has_modrm: true, base_reg1: base_reg, base_reg2: base_reg, code, tuple_type, can_broadcast }
	}

	pub(super) fn new1(base_reg1: Register, base_reg2: Register, code: u32, tuple_type: TupleType, can_broadcast: bool) -> Self {
		Self { decode: OpCodeHandler_EVEX_VkW::decode, has_modrm: true, base_reg1, base_reg2, code, tuple_type, can_broadcast }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32,
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg2 as u32,
			);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.can_broadcast {
					instruction_internal::internal_set_is_broadcast(instruction);
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
pub(super) struct OpCodeHandler_EVEX_WkV {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	disallow_zeroing_masking: u32,
	code: u32,
	base_reg1: Register,
	base_reg2: Register,
	tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_WkV {
	pub(super) fn new(base_reg: Register, code: u32, tuple_type: TupleType) -> Self {
		Self {
			decode: OpCodeHandler_EVEX_WkV::decode,
			has_modrm: true,
			base_reg1: base_reg,
			base_reg2: base_reg,
			code,
			tuple_type,
			disallow_zeroing_masking: 0,
		}
	}

	pub(super) fn new1(base_reg: Register, code: u32, tuple_type: TupleType, allow_zeroing_masking: bool) -> Self {
		Self {
			decode: OpCodeHandler_EVEX_WkV::decode,
			has_modrm: true,
			base_reg1: base_reg,
			base_reg2: base_reg,
			code,
			tuple_type,
			disallow_zeroing_masking: if allow_zeroing_masking { 0 } else { u32::MAX },
		}
	}

	pub(super) fn new2(base_reg1: Register, base_reg2: Register, code: u32, tuple_type: TupleType) -> Self {
		Self { decode: OpCodeHandler_EVEX_WkV::decode, has_modrm: true, base_reg1, base_reg2, code, tuple_type, disallow_zeroing_masking: 0 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		if (((decoder.state.flags & StateFlags::B) | decoder.state.vvvv_invalid_check) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction_internal::internal_set_op1_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg2 as u32,
		);
		if ((decoder.state.flags & StateFlags::Z) & this.disallow_zeroing_masking & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg1 as u32,
			);
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
pub(super) struct OpCodeHandler_EVEX_VkM {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg: Register,
	tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_VkM {
	pub(super) fn new(base_reg: Register, code: u32, tuple_type: TupleType) -> Self {
		Self { decode: OpCodeHandler_EVEX_VkM::decode, has_modrm: true, base_reg, code, tuple_type }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		if (((decoder.state.flags & StateFlags::B) | decoder.state.vvvv_invalid_check) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32,
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
pub(super) struct OpCodeHandler_EVEX_VkWIb {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg1: Register,
	base_reg2: Register,
	tuple_type: TupleType,
	can_broadcast: bool,
}

impl OpCodeHandler_EVEX_VkWIb {
	pub(super) fn new(base_reg: Register, code: u32, tuple_type: TupleType, can_broadcast: bool) -> Self {
		Self { decode: OpCodeHandler_EVEX_VkWIb::decode, has_modrm: true, base_reg1: base_reg, base_reg2: base_reg, code, tuple_type, can_broadcast }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32,
		);
		instruction.set_op2_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg2 as u32,
			);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.can_broadcast {
					instruction_internal::internal_set_is_broadcast(instruction);
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
pub(super) struct OpCodeHandler_EVEX_WkVIb {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg1: Register,
	base_reg2: Register,
	tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_WkVIb {
	pub(super) fn new(base_reg1: Register, base_reg2: Register, code: u32, tuple_type: TupleType) -> Self {
		Self { decode: OpCodeHandler_EVEX_WkVIb::decode, has_modrm: true, base_reg1, base_reg2, code, tuple_type }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		if (((decoder.state.flags & StateFlags::B) | decoder.state.vvvv_invalid_check) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction_internal::internal_set_op1_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg2 as u32,
		);
		instruction.set_op2_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg1 as u32,
			);
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
pub(super) struct OpCodeHandler_EVEX_HkWIb {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg1: Register,
	base_reg2: Register,
	tuple_type: TupleType,
	can_broadcast: bool,
}

impl OpCodeHandler_EVEX_HkWIb {
	pub(super) fn new(base_reg: Register, code: u32, tuple_type: TupleType, can_broadcast: bool) -> Self {
		Self { decode: OpCodeHandler_EVEX_HkWIb::decode, has_modrm: true, base_reg1: base_reg, base_reg2: base_reg, code, tuple_type, can_broadcast }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.vvvv + this.base_reg1 as u32);
		instruction.set_op2_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg2 as u32,
			);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.can_broadcast {
					instruction_internal::internal_set_is_broadcast(instruction);
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
pub(super) struct OpCodeHandler_EVEX_HWIb {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg1: Register,
	base_reg2: Register,
	tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_HWIb {
	pub(super) fn new(base_reg: Register, code: u32, tuple_type: TupleType) -> Self {
		Self { decode: OpCodeHandler_EVEX_HWIb::decode, has_modrm: true, base_reg1: base_reg, base_reg2: base_reg, code, tuple_type }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		instruction_internal::internal_set_code_u32(instruction, this.code);
		if (((decoder.state.flags & (StateFlags::Z | StateFlags::B)) | decoder.state.aaa) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.vvvv + this.base_reg1 as u32);
		instruction.set_op2_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg2 as u32,
			);
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
pub(super) struct OpCodeHandler_EVEX_WkVIb_er {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg1: Register,
	base_reg2: Register,
	tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_WkVIb_er {
	pub(super) fn new(base_reg1: Register, base_reg2: Register, code: u32, tuple_type: TupleType) -> Self {
		Self { decode: OpCodeHandler_EVEX_WkVIb_er::decode, has_modrm: true, base_reg1, base_reg2, code, tuple_type }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction_internal::internal_set_op1_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg2 as u32,
		);
		instruction.set_op2_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg1 as u32,
			);
			if (decoder.state.flags & StateFlags::B) != 0 {
				instruction_internal::internal_set_suppress_all_exceptions(instruction);
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
pub(super) struct OpCodeHandler_EVEX_VW_er {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg1: Register,
	base_reg2: Register,
	tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_VW_er {
	pub(super) fn new(base_reg: Register, code: u32, tuple_type: TupleType) -> Self {
		Self { decode: OpCodeHandler_EVEX_VW_er::decode, has_modrm: true, base_reg1: base_reg, base_reg2: base_reg, code, tuple_type }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32,
		);
		if (((decoder.state.flags & StateFlags::Z) | decoder.state.vvvv_invalid_check | decoder.state.aaa) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg2 as u32,
			);
			if (decoder.state.flags & StateFlags::B) != 0 {
				instruction_internal::internal_set_suppress_all_exceptions(instruction);
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
pub(super) struct OpCodeHandler_EVEX_VW {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg1: Register,
	base_reg2: Register,
	tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_VW {
	pub(super) fn new(base_reg: Register, code: u32, tuple_type: TupleType) -> Self {
		Self { decode: OpCodeHandler_EVEX_VW::decode, has_modrm: true, base_reg1: base_reg, base_reg2: base_reg, code, tuple_type }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		if (((decoder.state.flags & (StateFlags::Z | StateFlags::B)) | decoder.state.vvvv_invalid_check | decoder.state.aaa)
			& decoder.invalid_check_mask)
			!= 0
		{
			decoder.set_invalid_instruction();
		}
		instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32,
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg2 as u32,
			);
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
pub(super) struct OpCodeHandler_EVEX_WV {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg1: Register,
	base_reg2: Register,
	tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_WV {
	pub(super) fn new(base_reg: Register, code: u32, tuple_type: TupleType) -> Self {
		Self { decode: OpCodeHandler_EVEX_WV::decode, has_modrm: true, base_reg1: base_reg, base_reg2: base_reg, code, tuple_type }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		if (((decoder.state.flags & (StateFlags::Z | StateFlags::B)) | decoder.state.vvvv_invalid_check | decoder.state.aaa)
			& decoder.invalid_check_mask)
			!= 0
		{
			decoder.set_invalid_instruction();
		}
		instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction_internal::internal_set_op1_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32,
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg2 as u32,
			);
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
pub(super) struct OpCodeHandler_EVEX_VM {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg: Register,
	tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_VM {
	pub(super) fn new(base_reg: Register, code: u32, tuple_type: TupleType) -> Self {
		Self { decode: OpCodeHandler_EVEX_VM::decode, has_modrm: true, base_reg, code, tuple_type }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		if (((decoder.state.flags & (StateFlags::Z | StateFlags::B)) | decoder.state.vvvv_invalid_check | decoder.state.aaa)
			& decoder.invalid_check_mask)
			!= 0
		{
			decoder.set_invalid_instruction();
		}
		instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32,
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
pub(super) struct OpCodeHandler_EVEX_VK {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg: Register,
}

impl OpCodeHandler_EVEX_VK {
	pub(super) fn new(base_reg: Register, code: u32) -> Self {
		Self { decode: OpCodeHandler_EVEX_VK::decode, has_modrm: true, base_reg, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		if (((decoder.state.flags & (StateFlags::B | StateFlags::Z)) | decoder.state.vvvv_invalid_check | decoder.state.aaa)
			& decoder.invalid_check_mask)
			!= 0
		{
			decoder.set_invalid_instruction();
		}
		instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32,
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.rm + Register::K0 as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_EVEX_KR {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg: Register,
}

impl OpCodeHandler_EVEX_KR {
	pub(super) fn new(base_reg: Register, code: u32) -> Self {
		Self { decode: OpCodeHandler_EVEX_KR::decode, has_modrm: true, base_reg, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
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
		instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.reg + Register::K0 as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg as u32,
			);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_EVEX_KkHWIb_sae {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg: Register,
	tuple_type: TupleType,
	can_broadcast: bool,
}

impl OpCodeHandler_EVEX_KkHWIb_sae {
	pub(super) fn new(base_reg: Register, code: u32, tuple_type: TupleType, can_broadcast: bool) -> Self {
		Self { decode: OpCodeHandler_EVEX_KkHWIb_sae::decode, has_modrm: true, base_reg, code, tuple_type, can_broadcast }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		if (((decoder.state.flags & StateFlags::Z) | decoder.state.extra_register_base | decoder.state.extra_register_base_evex)
			& decoder.invalid_check_mask)
			!= 0
		{
			decoder.set_invalid_instruction();
		}
		instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.reg + Register::K0 as u32);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg as u32);
		instruction.set_op3_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op2_kind(OpKind::Register);
			instruction_internal::internal_set_op2_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg as u32,
			);
			if (decoder.state.flags & StateFlags::B) != 0 {
				instruction_internal::internal_set_suppress_all_exceptions(instruction);
			}
		} else {
			instruction.set_op2_kind(OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.can_broadcast {
					instruction_internal::internal_set_is_broadcast(instruction);
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
pub(super) struct OpCodeHandler_EVEX_VkHW {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg1: Register,
	base_reg2: Register,
	base_reg3: Register,
	tuple_type: TupleType,
	can_broadcast: bool,
}

impl OpCodeHandler_EVEX_VkHW {
	pub(super) fn new(base_reg: Register, code: u32, tuple_type: TupleType, can_broadcast: bool) -> Self {
		Self {
			decode: OpCodeHandler_EVEX_VkHW::decode,
			has_modrm: true,
			base_reg1: base_reg,
			base_reg2: base_reg,
			base_reg3: base_reg,
			code,
			tuple_type,
			can_broadcast,
		}
	}

	pub(super) fn new1(base_reg1: Register, base_reg2: Register, base_reg3: Register, code: u32, tuple_type: TupleType, can_broadcast: bool) -> Self {
		Self { decode: OpCodeHandler_EVEX_VkHW::decode, has_modrm: true, base_reg1, base_reg2, base_reg3, code, tuple_type, can_broadcast }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32,
		);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg2 as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op2_kind(OpKind::Register);
			instruction_internal::internal_set_op2_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg3 as u32,
			);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			instruction.set_op2_kind(OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.can_broadcast {
					instruction_internal::internal_set_is_broadcast(instruction);
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
pub(super) struct OpCodeHandler_EVEX_VkHM {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg1: Register,
	base_reg2: Register,
	tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_VkHM {
	pub(super) fn new(base_reg: Register, code: u32, tuple_type: TupleType) -> Self {
		Self { decode: OpCodeHandler_EVEX_VkHM::decode, has_modrm: true, base_reg1: base_reg, base_reg2: base_reg, code, tuple_type }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32,
		);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg2 as u32);
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
pub(super) struct OpCodeHandler_EVEX_VkHWIb {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg1: Register,
	base_reg2: Register,
	base_reg3: Register,
	tuple_type: TupleType,
	can_broadcast: bool,
}

impl OpCodeHandler_EVEX_VkHWIb {
	pub(super) fn new(base_reg: Register, code: u32, tuple_type: TupleType, can_broadcast: bool) -> Self {
		Self {
			decode: OpCodeHandler_EVEX_VkHWIb::decode,
			has_modrm: true,
			base_reg1: base_reg,
			base_reg2: base_reg,
			base_reg3: base_reg,
			code,
			tuple_type,
			can_broadcast,
		}
	}

	pub(super) fn new1(base_reg1: Register, base_reg2: Register, base_reg3: Register, code: u32, tuple_type: TupleType, can_broadcast: bool) -> Self {
		Self { decode: OpCodeHandler_EVEX_VkHWIb::decode, has_modrm: true, base_reg1, base_reg2, base_reg3, code, tuple_type, can_broadcast }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32,
		);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg2 as u32);
		instruction.set_op3_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op2_kind(OpKind::Register);
			instruction_internal::internal_set_op2_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg3 as u32,
			);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			instruction.set_op2_kind(OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.can_broadcast {
					instruction_internal::internal_set_is_broadcast(instruction);
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
pub(super) struct OpCodeHandler_EVEX_VkHWIb_er {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg1: Register,
	base_reg2: Register,
	base_reg3: Register,
	tuple_type: TupleType,
	can_broadcast: bool,
}

impl OpCodeHandler_EVEX_VkHWIb_er {
	pub(super) fn new(base_reg: Register, code: u32, tuple_type: TupleType, can_broadcast: bool) -> Self {
		Self {
			decode: OpCodeHandler_EVEX_VkHWIb_er::decode,
			has_modrm: true,
			base_reg1: base_reg,
			base_reg2: base_reg,
			base_reg3: base_reg,
			code,
			tuple_type,
			can_broadcast,
		}
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32,
		);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg2 as u32);
		instruction.set_op3_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op2_kind(OpKind::Register);
			instruction_internal::internal_set_op2_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg3 as u32,
			);
			if (decoder.state.flags & StateFlags::B) != 0 {
				instruction_internal::internal_set_suppress_all_exceptions(instruction);
			}
		} else {
			instruction.set_op2_kind(OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.can_broadcast {
					instruction_internal::internal_set_is_broadcast(instruction);
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
pub(super) struct OpCodeHandler_EVEX_KkHW {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg: Register,
	tuple_type: TupleType,
	can_broadcast: bool,
}

impl OpCodeHandler_EVEX_KkHW {
	pub(super) fn new(base_reg: Register, code: u32, tuple_type: TupleType, can_broadcast: bool) -> Self {
		Self { decode: OpCodeHandler_EVEX_KkHW::decode, has_modrm: true, base_reg, code, tuple_type, can_broadcast }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.reg + Register::K0 as u32);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg as u32);
		if (((decoder.state.flags & StateFlags::Z) | decoder.state.extra_register_base | decoder.state.extra_register_base_evex)
			& decoder.invalid_check_mask)
			!= 0
		{
			decoder.set_invalid_instruction();
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op2_kind(OpKind::Register);
			instruction_internal::internal_set_op2_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg as u32,
			);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			instruction.set_op2_kind(OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.can_broadcast {
					instruction_internal::internal_set_is_broadcast(instruction);
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
pub(super) struct OpCodeHandler_EVEX_KP1HW {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg: Register,
	tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_KP1HW {
	pub(super) fn new(base_reg: Register, code: u32, tuple_type: TupleType) -> Self {
		Self { decode: OpCodeHandler_EVEX_KP1HW::decode, has_modrm: true, base_reg, code, tuple_type }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.reg + Register::K0 as u32);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg as u32);
		if (((decoder.state.flags & StateFlags::Z) | decoder.state.aaa | decoder.state.extra_register_base | decoder.state.extra_register_base_evex)
			& decoder.invalid_check_mask)
			!= 0
		{
			decoder.set_invalid_instruction();
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op2_kind(OpKind::Register);
			instruction_internal::internal_set_op2_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg as u32,
			);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			instruction.set_op2_kind(OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				instruction_internal::internal_set_is_broadcast(instruction);
			}
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_EVEX_KkHWIb {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg: Register,
	tuple_type: TupleType,
	can_broadcast: bool,
}

impl OpCodeHandler_EVEX_KkHWIb {
	pub(super) fn new(base_reg: Register, code: u32, tuple_type: TupleType, can_broadcast: bool) -> Self {
		Self { decode: OpCodeHandler_EVEX_KkHWIb::decode, has_modrm: true, base_reg, code, tuple_type, can_broadcast }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.reg + Register::K0 as u32);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op2_kind(OpKind::Register);
			instruction_internal::internal_set_op2_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg as u32,
			);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			instruction.set_op2_kind(OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.can_broadcast {
					instruction_internal::internal_set_is_broadcast(instruction);
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
pub(super) struct OpCodeHandler_EVEX_WkHV {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg: Register,
}

impl OpCodeHandler_EVEX_WkHV {
	pub(super) fn new(base_reg: Register, code: u32) -> Self {
		Self { decode: OpCodeHandler_EVEX_WkHV::decode, has_modrm: true, base_reg, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		instruction_internal::internal_set_code_u32(instruction, this.code);

		debug_assert_eq!(decoder.state.mod_, 3);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg as u32,
		);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg as u32);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op2_kind(OpKind::Register);
		instruction_internal::internal_set_op2_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32,
		);
		if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_EVEX_VHWIb {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg: Register,
	tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_VHWIb {
	pub(super) fn new(base_reg: Register, code: u32, tuple_type: TupleType) -> Self {
		Self { decode: OpCodeHandler_EVEX_VHWIb::decode, has_modrm: true, base_reg, code, tuple_type }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		if (((decoder.state.flags & (StateFlags::B | StateFlags::Z)) | decoder.state.aaa) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32,
		);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg as u32);
		instruction.set_op3_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op2_kind(OpKind::Register);
			instruction_internal::internal_set_op2_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg as u32,
			);
		} else {
			instruction.set_op2_kind(OpKind::Memory);
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_EVEX_VHW {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code_r: u32,
	code_m: u32,
	base_reg1: Register,
	base_reg2: Register,
	base_reg3: Register,
	tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_VHW {
	pub(super) fn new(base_reg: Register, code_r: u32, code_m: u32, tuple_type: TupleType) -> Self {
		Self {
			decode: OpCodeHandler_EVEX_VHW::decode,
			has_modrm: true,
			base_reg1: base_reg,
			base_reg2: base_reg,
			base_reg3: base_reg,
			code_r,
			code_m,
			tuple_type,
		}
	}

	pub(super) fn new2(base_reg: Register, code: u32, tuple_type: TupleType) -> Self {
		Self {
			decode: OpCodeHandler_EVEX_VHW::decode,
			has_modrm: true,
			base_reg1: base_reg,
			base_reg2: base_reg,
			base_reg3: base_reg,
			code_r: code,
			code_m: code,
			tuple_type,
		}
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		if (((decoder.state.flags & (StateFlags::B | StateFlags::Z)) | decoder.state.aaa) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32,
		);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg2 as u32);
		if decoder.state.mod_ == 3 {
			instruction_internal::internal_set_code_u32(instruction, this.code_r);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op2_kind(OpKind::Register);
			instruction_internal::internal_set_op2_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg3 as u32,
			);
		} else {
			instruction_internal::internal_set_code_u32(instruction, this.code_m);
			instruction.set_op2_kind(OpKind::Memory);
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_EVEX_VHM {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg: Register,
	tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_VHM {
	pub(super) fn new(base_reg: Register, code: u32, tuple_type: TupleType) -> Self {
		Self { decode: OpCodeHandler_EVEX_VHM::decode, has_modrm: true, base_reg, code, tuple_type }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		if (((decoder.state.flags & (StateFlags::B | StateFlags::Z)) | decoder.state.aaa) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32,
		);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg as u32);
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
pub(super) struct OpCodeHandler_EVEX_Gv_W_er {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code_w0: u32,
	code_w1: u32,
	base_reg: Register,
	tuple_type: TupleType,
	only_sae: bool,
}

impl OpCodeHandler_EVEX_Gv_W_er {
	pub(super) fn new(base_reg: Register, code_w0: u32, code_w1: u32, tuple_type: TupleType, only_sae: bool) -> Self {
		Self { decode: OpCodeHandler_EVEX_Gv_W_er::decode, has_modrm: true, base_reg, code_w0, code_w1, tuple_type, only_sae }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		if (((decoder.state.flags & StateFlags::Z) | decoder.state.vvvv_invalid_check | decoder.state.aaa | decoder.state.extra_register_base_evex)
			& decoder.invalid_check_mask)
			!= 0
		{
			decoder.set_invalid_instruction();
		}
		if (decoder.state.flags & decoder.is64b_mode_and_w) != 0 {
			instruction_internal::internal_set_code_u32(instruction, this.code_w1);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32,
			);
		} else {
			instruction_internal::internal_set_code_u32(instruction, this.code_w0);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32,
			);
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg as u32,
			);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.only_sae {
					instruction_internal::internal_set_suppress_all_exceptions(instruction);
				} else {
					const_assert_eq!(RoundingControl::None as u32, 0);
					const_assert_eq!(RoundingControl::RoundToNearest as u32, 1);
					const_assert_eq!(RoundingControl::RoundDown as u32, 2);
					const_assert_eq!(RoundingControl::RoundUp as u32, 3);
					const_assert_eq!(RoundingControl::RoundTowardZero as u32, 4);
					instruction_internal::internal_set_rounding_control(instruction, (decoder.state.vector_length as u32) + 1);
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
pub(super) struct OpCodeHandler_EVEX_VX_Ev {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code32: u32,
	code64: u32,
	tuple_type_w0: TupleType,
	tuple_type_w1: TupleType,
}

impl OpCodeHandler_EVEX_VX_Ev {
	pub(super) fn new(code32: u32, code64: u32, tuple_type_w0: TupleType, tuple_type_w1: TupleType) -> Self {
		Self { decode: OpCodeHandler_EVEX_VX_Ev::decode, has_modrm: true, code32, code64, tuple_type_w0, tuple_type_w1 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		if (((decoder.state.flags & (StateFlags::B | StateFlags::Z)) | decoder.state.vvvv_invalid_check | decoder.state.aaa)
			& decoder.invalid_check_mask)
			!= 0
		{
			decoder.set_invalid_instruction();
		}
		let gpr;
		let tuple_type;
		if (decoder.state.flags & decoder.is64b_mode_and_w) != 0 {
			instruction_internal::internal_set_code_u32(instruction, this.code64);
			tuple_type = this.tuple_type_w1;
			gpr = Register::RAX as u32;
		} else {
			instruction_internal::internal_set_code_u32(instruction, this.code32);
			tuple_type = this.tuple_type_w0;
			gpr = Register::EAX as u32;
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + Register::XMM0 as u32,
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem_tuple_type(instruction, tuple_type);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_EVEX_Ev_VX {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code32: u32,
	code64: u32,
	tuple_type_w0: TupleType,
	tuple_type_w1: TupleType,
}

impl OpCodeHandler_EVEX_Ev_VX {
	pub(super) fn new(code32: u32, code64: u32, tuple_type_w0: TupleType, tuple_type_w1: TupleType) -> Self {
		Self { decode: OpCodeHandler_EVEX_Ev_VX::decode, has_modrm: true, code32, code64, tuple_type_w0, tuple_type_w1 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		if (((decoder.state.flags & (StateFlags::B | StateFlags::Z)) | decoder.state.vvvv_invalid_check | decoder.state.aaa)
			& decoder.invalid_check_mask)
			!= 0
		{
			decoder.set_invalid_instruction();
		}
		let gpr;
		let tuple_type;
		if (decoder.state.flags & decoder.is64b_mode_and_w) != 0 {
			instruction_internal::internal_set_code_u32(instruction, this.code64);
			tuple_type = this.tuple_type_w1;
			gpr = Register::RAX as u32;
		} else {
			instruction_internal::internal_set_code_u32(instruction, this.code32);
			tuple_type = this.tuple_type_w0;
			gpr = Register::EAX as u32;
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction_internal::internal_set_op1_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + Register::XMM0 as u32,
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem_tuple_type(instruction, tuple_type);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_EVEX_Ev_VX_Ib {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code32: u32,
	code64: u32,
	base_reg: Register,
}

impl OpCodeHandler_EVEX_Ev_VX_Ib {
	pub(super) fn new(base_reg: Register, code32: u32, code64: u32) -> Self {
		Self { decode: OpCodeHandler_EVEX_Ev_VX_Ib::decode, has_modrm: true, base_reg, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
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
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction_internal::internal_set_op1_register_u32(
			instruction,
			decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg as u32,
		);
		instruction.set_op2_kind(OpKind::Immediate8);
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
		if (decoder.state.flags & decoder.is64b_mode_and_w) != 0 {
			instruction_internal::internal_set_code_u32(instruction, this.code64);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32,
			);
		} else {
			instruction_internal::internal_set_code_u32(instruction, this.code32);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32,
			);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_EVEX_MV {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg: Register,
	tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_MV {
	pub(super) fn new(base_reg: Register, code: u32, tuple_type: TupleType) -> Self {
		Self { decode: OpCodeHandler_EVEX_MV::decode, has_modrm: true, base_reg, code, tuple_type }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		if (((decoder.state.flags & (StateFlags::B | StateFlags::Z)) | decoder.state.vvvv_invalid_check | decoder.state.aaa)
			& decoder.invalid_check_mask)
			!= 0
		{
			decoder.set_invalid_instruction();
		}
		instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction_internal::internal_set_op1_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32,
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
pub(super) struct OpCodeHandler_EVEX_VkEv_REXW {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code32: u32,
	code64: u32,
	base_reg: Register,
}

impl OpCodeHandler_EVEX_VkEv_REXW {
	pub(super) fn new(base_reg: Register, code32: u32, code64: u32) -> Self {
		Self { decode: OpCodeHandler_EVEX_VkEv_REXW::decode, has_modrm: true, base_reg, code32, code64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		if (((decoder.state.flags & StateFlags::B) | decoder.state.vvvv_invalid_check) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		let gpr;
		if (decoder.state.flags & decoder.is64b_mode_and_w) != 0 {
			debug_assert_ne!(this.code64, Code::INVALID as u32);
			instruction_internal::internal_set_code_u32(instruction, this.code64);
			gpr = Register::RAX as u32;
		} else {
			instruction_internal::internal_set_code_u32(instruction, this.code32);
			gpr = Register::EAX as u32;
		}

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32,
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_EVEX_Vk_VSIB {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg: Register,
	vsib_base: Register,
	tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_Vk_VSIB {
	pub(super) fn new(base_reg: Register, vsib_base: Register, code: u32, tuple_type: TupleType) -> Self {
		Self { decode: OpCodeHandler_EVEX_Vk_VSIB::decode, has_modrm: true, base_reg, vsib_base, code, tuple_type }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		if decoder.invalid_check_mask != 0
			&& (((decoder.state.flags & (StateFlags::Z | StateFlags::B)) | (decoder.state.vvvv_invalid_check & 0xF)) != 0 || decoder.state.aaa == 0)
		{
			decoder.set_invalid_instruction();
		}
		instruction_internal::internal_set_code_u32(instruction, this.code);

		let reg_num = decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex;
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction_internal::internal_set_op0_register_u32(instruction, reg_num + this.base_reg as u32);
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
pub(super) struct OpCodeHandler_EVEX_VSIB_k1_VX {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	vsib_index: Register,
	base_reg: Register,
	tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_VSIB_k1_VX {
	pub(super) fn new(vsib_index: Register, base_reg: Register, code: u32, tuple_type: TupleType) -> Self {
		Self { decode: OpCodeHandler_EVEX_VSIB_k1_VX::decode, has_modrm: true, vsib_index, base_reg, code, tuple_type }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		if decoder.invalid_check_mask != 0
			&& (((decoder.state.flags & (StateFlags::B | StateFlags::Z)) | (decoder.state.vvvv_invalid_check & 0xF)) != 0 || decoder.state.aaa == 0)
		{
			decoder.set_invalid_instruction();
		}
		instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction_internal::internal_set_op1_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32,
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
pub(super) struct OpCodeHandler_EVEX_VSIB_k1 {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	vsib_index: Register,
	tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_VSIB_k1 {
	pub(super) fn new(vsib_index: Register, code: u32, tuple_type: TupleType) -> Self {
		Self { decode: OpCodeHandler_EVEX_VSIB_k1::decode, has_modrm: true, vsib_index, code, tuple_type }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		if decoder.invalid_check_mask != 0
			&& (((decoder.state.flags & (StateFlags::B | StateFlags::Z)) | (decoder.state.vvvv_invalid_check & 0xF)) != 0 || decoder.state.aaa == 0)
		{
			decoder.set_invalid_instruction();
		}
		instruction_internal::internal_set_code_u32(instruction, this.code);

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
pub(super) struct OpCodeHandler_EVEX_GvM_VX_Ib {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code32: u32,
	code64: u32,
	base_reg: Register,
	tuple_type32: TupleType,
	tuple_type64: TupleType,
}

impl OpCodeHandler_EVEX_GvM_VX_Ib {
	pub(super) fn new(base_reg: Register, code32: u32, code64: u32, tuple_type32: TupleType, tuple_type64: TupleType) -> Self {
		Self { decode: OpCodeHandler_EVEX_GvM_VX_Ib::decode, has_modrm: true, base_reg, code32, code64, tuple_type32, tuple_type64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		if (((decoder.state.flags & (StateFlags::B | StateFlags::Z)) | decoder.state.vvvv_invalid_check | decoder.state.aaa)
			& decoder.invalid_check_mask)
			!= 0
		{
			decoder.set_invalid_instruction();
		}
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		instruction_internal::internal_set_op1_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32,
		);
		instruction.set_op2_kind(OpKind::Immediate8);
		let gpr;
		if (decoder.state.flags & decoder.is64b_mode_and_w) != 0 {
			instruction_internal::internal_set_code_u32(instruction, this.code64);
			gpr = Register::RAX as u32;
		} else {
			instruction_internal::internal_set_code_u32(instruction, this.code32);
			gpr = Register::EAX as u32;
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
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
pub(super) struct OpCodeHandler_EVEX_KkWIb {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
	base_reg: Register,
	tuple_type: TupleType,
	can_broadcast: bool,
}

impl OpCodeHandler_EVEX_KkWIb {
	pub(super) fn new(base_reg: Register, code: u32, tuple_type: TupleType, can_broadcast: bool) -> Self {
		Self { decode: OpCodeHandler_EVEX_KkWIb::decode, has_modrm: true, base_reg, code, tuple_type, can_broadcast }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX);
		if (((decoder.state.flags & StateFlags::Z)
			| decoder.state.vvvv_invalid_check
			| decoder.state.extra_register_base
			| decoder.state.extra_register_base_evex)
			& decoder.invalid_check_mask)
			!= 0
		{
			decoder.set_invalid_instruction();
		}
		instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.reg + Register::K0 as u32);
		instruction.set_op2_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg as u32,
			);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.can_broadcast {
					instruction_internal::internal_set_is_broadcast(instruction);
				} else if decoder.invalid_check_mask != 0 {
					decoder.set_invalid_instruction();
				}
			}
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}
