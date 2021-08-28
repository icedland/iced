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
pub(super) struct OpCodeHandler_VectorLength_EVEX {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	handlers: [(OpCodeHandlerDecodeFn, &'static OpCodeHandler); 4],
}

impl OpCodeHandler_VectorLength_EVEX {
	#[allow(trivial_casts)]
	#[cold]
	pub(super) fn new(
		handler128: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), handler256: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
		handler512: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	) -> Self {
		const_assert_eq!(VectorLength::L128 as u32, 0);
		const_assert_eq!(VectorLength::L256 as u32, 1);
		const_assert_eq!(VectorLength::L512 as u32, 2);
		const_assert_eq!(VectorLength::Unknown as u32, 3);
		debug_assert!(!is_null_instance_handler(handler128.1));
		debug_assert!(!is_null_instance_handler(handler256.1));
		debug_assert!(!is_null_instance_handler(handler512.1));
		let handlers = [handler128, handler256, handler512, get_invalid_handler()];
		debug_assert!(handlers[0].1.has_modrm);
		debug_assert!(handlers[1].1.has_modrm);
		debug_assert!(handlers[2].1.has_modrm);
		Self { has_modrm: true, decode: OpCodeHandler_VectorLength_EVEX::decode, handlers }
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
pub(super) struct OpCodeHandler_VectorLength_EVEX_er {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	handlers: [(OpCodeHandlerDecodeFn, &'static OpCodeHandler); 4],
}

impl OpCodeHandler_VectorLength_EVEX_er {
	#[allow(trivial_casts)]
	#[cold]
	pub(super) fn new(
		handler128: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), handler256: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
		handler512: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	) -> Self {
		const_assert_eq!(VectorLength::L128 as u32, 0);
		const_assert_eq!(VectorLength::L256 as u32, 1);
		const_assert_eq!(VectorLength::L512 as u32, 2);
		const_assert_eq!(VectorLength::Unknown as u32, 3);
		debug_assert!(!is_null_instance_handler(handler128.1));
		debug_assert!(!is_null_instance_handler(handler256.1));
		debug_assert!(!is_null_instance_handler(handler512.1));
		let handlers = [handler128, handler256, handler512, get_invalid_handler()];
		debug_assert!(handlers[0].1.has_modrm);
		debug_assert!(handlers[1].1.has_modrm);
		debug_assert!(handlers[2].1.has_modrm);
		Self { has_modrm: true, decode: OpCodeHandler_VectorLength_EVEX_er::decode, handlers }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		let mut index = decoder.state.vector_length;
		const_assert!(StateFlags::B > 3);
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
pub(super) struct OpCodeHandler_EVEX_V_H_Ev_er {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code_w0: Code,
	code_w1: Code,
	base_reg: Register,
	tuple_type_w0: TupleType,
	tuple_type_w1: TupleType,
}

impl OpCodeHandler_EVEX_V_H_Ev_er {
	#[cold]
	pub(super) fn new(base_reg: Register, code_w0: Code, code_w1: Code, tuple_type_w0: TupleType, tuple_type_w1: TupleType) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_V_H_Ev_er::decode, base_reg, code_w0, code_w1, tuple_type_w0, tuple_type_w1 }
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
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32
		);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op2_kind(OpKind::Register);
			write_op2_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
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
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code_w0: Code,
	code_w1: Code,
	base_reg: Register,
	tuple_type_w0: TupleType,
	tuple_type_w1: TupleType,
}

impl OpCodeHandler_EVEX_V_H_Ev_Ib {
	#[cold]
	pub(super) fn new(base_reg: Register, code_w0: Code, code_w1: Code, tuple_type_w0: TupleType, tuple_type_w1: TupleType) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_V_H_Ev_Ib::decode, base_reg, code_w0, code_w1, tuple_type_w0, tuple_type_w1 }
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
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32
		);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg as u32);
		instruction.set_op3_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op2_kind(OpKind::Register);
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
pub(super) struct OpCodeHandler_EVEX_Ed_V_Ib {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code32: Code,
	code64: Code,
	base_reg: Register,
	tuple_type32: TupleType,
	tuple_type64: TupleType,
}

impl OpCodeHandler_EVEX_Ed_V_Ib {
	#[cold]
	pub(super) fn new(base_reg: Register, code32: Code, code64: Code, tuple_type32: TupleType, tuple_type64: TupleType) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_Ed_V_Ib::decode, base_reg, code32, code64, tuple_type32, tuple_type64 }
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
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
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
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
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
pub(super) struct OpCodeHandler_EVEX_VkHW_er {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg: Register,
	tuple_type: TupleType,
	only_sae: bool,
	can_broadcast: bool,
}

impl OpCodeHandler_EVEX_VkHW_er {
	#[cold]
	pub(super) fn new(base_reg: Register, code: Code, tuple_type: TupleType, only_sae: bool, can_broadcast: bool) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_VkHW_er::decode, base_reg, code, tuple_type, only_sae, can_broadcast }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		instruction.set_code(this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32
		);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op2_kind(OpKind::Register);
			write_op2_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg as u32);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.only_sae {
					instruction.set_suppress_all_exceptions(true);
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
pub(super) struct OpCodeHandler_EVEX_VkHW_er_ur {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg: Register,
	tuple_type: TupleType,
	can_broadcast: bool,
}

impl OpCodeHandler_EVEX_VkHW_er_ur {
	#[cold]
	pub(super) fn new(base_reg: Register, code: Code, tuple_type: TupleType, can_broadcast: bool) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_VkHW_er_ur::decode, base_reg, code, tuple_type, can_broadcast }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		instruction.set_code(this.code);

		let reg_num0 = decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex;
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(instruction, reg_num0 + this.base_reg as u32);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			let reg_num2 = decoder.state.rm + decoder.state.extra_base_register_base_evex;
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op2_kind(OpKind::Register);
			write_op2_reg!(instruction, reg_num2 + this.base_reg as u32);
			if decoder.invalid_check_mask != 0 && (reg_num0 == decoder.state.vvvv || reg_num0 == reg_num2) {
				decoder.set_invalid_instruction();
			}
			if (decoder.state.flags & StateFlags::B) != 0 {
				const_assert_eq!(RoundingControl::None as u32, 0);
				const_assert_eq!(RoundingControl::RoundToNearest as u32, 1);
				const_assert_eq!(RoundingControl::RoundDown as u32, 2);
				const_assert_eq!(RoundingControl::RoundUp as u32, 3);
				const_assert_eq!(RoundingControl::RoundTowardZero as u32, 4);
				instruction_internal::internal_set_rounding_control(instruction, (decoder.state.vector_length as u32) + 1);
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
pub(super) struct OpCodeHandler_EVEX_VkW_er {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg1: Register,
	base_reg2: Register,
	tuple_type: TupleType,
	only_sae: bool,
	can_broadcast: bool,
}

impl OpCodeHandler_EVEX_VkW_er {
	#[cold]
	pub(super) fn new(base_reg: Register, code: Code, tuple_type: TupleType, only_sae: bool) -> Self {
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

	#[cold]
	pub(super) fn new1(base_reg1: Register, base_reg2: Register, code: Code, tuple_type: TupleType, only_sae: bool) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_VkW_er::decode, base_reg1, base_reg2, code, tuple_type, only_sae, can_broadcast: true }
	}

	#[cold]
	pub(super) fn new2(base_reg1: Register, base_reg2: Register, code: Code, tuple_type: TupleType, only_sae: bool, can_broadcast: bool) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_VkW_er::decode, base_reg1, base_reg2, code, tuple_type, only_sae, can_broadcast }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg2 as u32);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.only_sae {
					instruction.set_suppress_all_exceptions(true);
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
pub(super) struct OpCodeHandler_EVEX_VkWIb_er {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg1: Register,
	base_reg2: Register,
	tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_VkWIb_er {
	#[cold]
	pub(super) fn new(base_reg: Register, code: Code, tuple_type: TupleType) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_VkWIb_er::decode, base_reg1: base_reg, base_reg2: base_reg, code, tuple_type }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32
		);
		instruction.set_op2_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
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
pub(super) struct OpCodeHandler_EVEX_VkW {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg1: Register,
	base_reg2: Register,
	tuple_type: TupleType,
	can_broadcast: bool,
}

impl OpCodeHandler_EVEX_VkW {
	#[cold]
	pub(super) fn new(base_reg: Register, code: Code, tuple_type: TupleType, can_broadcast: bool) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_VkW::decode, base_reg1: base_reg, base_reg2: base_reg, code, tuple_type, can_broadcast }
	}

	#[cold]
	pub(super) fn new1(base_reg1: Register, base_reg2: Register, code: Code, tuple_type: TupleType, can_broadcast: bool) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_VkW::decode, base_reg1, base_reg2, code, tuple_type, can_broadcast }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
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
pub(super) struct OpCodeHandler_EVEX_WkV {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	disallow_zeroing_masking: u32,
	code: Code,
	base_reg1: Register,
	base_reg2: Register,
	tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_WkV {
	#[cold]
	pub(super) fn new(base_reg: Register, code: Code, tuple_type: TupleType) -> Self {
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

	#[cold]
	pub(super) fn new1(base_reg: Register, code: Code, tuple_type: TupleType, allow_zeroing_masking: bool) -> Self {
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

	#[cold]
	pub(super) fn new2(base_reg1: Register, base_reg2: Register, code: Code, tuple_type: TupleType) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_WkV::decode, base_reg1, base_reg2, code, tuple_type, disallow_zeroing_masking: 0 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if (((decoder.state.flags & StateFlags::B) | decoder.state.vvvv_invalid_check) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		write_op1_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg2 as u32
		);
		if ((decoder.state.flags & StateFlags::Z) & this.disallow_zeroing_masking & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
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
pub(super) struct OpCodeHandler_EVEX_VkM {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg: Register,
	tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_VkM {
	#[cold]
	pub(super) fn new(base_reg: Register, code: Code, tuple_type: TupleType) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_VkM::decode, base_reg, code, tuple_type }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if (((decoder.state.flags & StateFlags::B) | decoder.state.vvvv_invalid_check) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
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
pub(super) struct OpCodeHandler_EVEX_VkWIb {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg1: Register,
	base_reg2: Register,
	tuple_type: TupleType,
	can_broadcast: bool,
}

impl OpCodeHandler_EVEX_VkWIb {
	#[cold]
	pub(super) fn new(base_reg: Register, code: Code, tuple_type: TupleType, can_broadcast: bool) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_VkWIb::decode, base_reg1: base_reg, base_reg2: base_reg, code, tuple_type, can_broadcast }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32
		);
		instruction.set_op2_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
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
pub(super) struct OpCodeHandler_EVEX_WkVIb {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg1: Register,
	base_reg2: Register,
	tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_WkVIb {
	#[cold]
	pub(super) fn new(base_reg1: Register, base_reg2: Register, code: Code, tuple_type: TupleType) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_WkVIb::decode, base_reg1, base_reg2, code, tuple_type }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if (((decoder.state.flags & StateFlags::B) | decoder.state.vvvv_invalid_check) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		write_op1_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg2 as u32
		);
		instruction.set_op2_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
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
pub(super) struct OpCodeHandler_EVEX_HkWIb {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg1: Register,
	base_reg2: Register,
	tuple_type: TupleType,
	can_broadcast: bool,
}

impl OpCodeHandler_EVEX_HkWIb {
	#[cold]
	pub(super) fn new(base_reg: Register, code: Code, tuple_type: TupleType, can_broadcast: bool) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_HkWIb::decode, base_reg1: base_reg, base_reg2: base_reg, code, tuple_type, can_broadcast }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		instruction.set_code(this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(instruction, decoder.state.vvvv + this.base_reg1 as u32);
		instruction.set_op2_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
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
pub(super) struct OpCodeHandler_EVEX_HWIb {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg1: Register,
	base_reg2: Register,
	tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_HWIb {
	#[cold]
	pub(super) fn new(base_reg: Register, code: Code, tuple_type: TupleType) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_HWIb::decode, base_reg1: base_reg, base_reg2: base_reg, code, tuple_type }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		instruction.set_code(this.code);
		if (((decoder.state.flags & (StateFlags::Z | StateFlags::B)) | decoder.state.aaa) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(instruction, decoder.state.vvvv + this.base_reg1 as u32);
		instruction.set_op2_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
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
pub(super) struct OpCodeHandler_EVEX_WkVIb_er {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg1: Register,
	base_reg2: Register,
	tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_WkVIb_er {
	#[cold]
	pub(super) fn new(base_reg1: Register, base_reg2: Register, code: Code, tuple_type: TupleType) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_WkVIb_er::decode, base_reg1, base_reg2, code, tuple_type }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if (decoder.state.vvvv_invalid_check & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		write_op1_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg2 as u32
		);
		instruction.set_op2_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
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
pub(super) struct OpCodeHandler_EVEX_VW_er {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg1: Register,
	base_reg2: Register,
	tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_VW_er {
	#[cold]
	pub(super) fn new(base_reg: Register, code: Code, tuple_type: TupleType) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_VW_er::decode, base_reg1: base_reg, base_reg2: base_reg, code, tuple_type }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		instruction.set_code(this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32
		);
		if (((decoder.state.flags & StateFlags::Z) | decoder.state.vvvv_invalid_check | decoder.state.aaa) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
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
pub(super) struct OpCodeHandler_EVEX_VW {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg1: Register,
	base_reg2: Register,
	tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_VW {
	#[cold]
	pub(super) fn new(base_reg: Register, code: Code, tuple_type: TupleType) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_VW::decode, base_reg1: base_reg, base_reg2: base_reg, code, tuple_type }
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

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
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
pub(super) struct OpCodeHandler_EVEX_WV {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg1: Register,
	base_reg2: Register,
	tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_WV {
	#[cold]
	pub(super) fn new(base_reg: Register, code: Code, tuple_type: TupleType) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_WV::decode, base_reg1: base_reg, base_reg2: base_reg, code, tuple_type }
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

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		write_op1_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
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
pub(super) struct OpCodeHandler_EVEX_VM {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg: Register,
	tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_VM {
	#[cold]
	pub(super) fn new(base_reg: Register, code: Code, tuple_type: TupleType) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_VM::decode, base_reg, code, tuple_type }
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

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
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
pub(super) struct OpCodeHandler_EVEX_VK {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg: Register,
}

impl OpCodeHandler_EVEX_VK {
	#[cold]
	pub(super) fn new(base_reg: Register, code: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_VK::decode, base_reg, code }
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

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, decoder.state.rm + Register::K0 as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_EVEX_KR {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg: Register,
}

impl OpCodeHandler_EVEX_KR {
	#[cold]
	pub(super) fn new(base_reg: Register, code: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_KR::decode, base_reg, code }
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

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(instruction, decoder.state.reg + Register::K0 as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_EVEX_KkHWIb_sae {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg: Register,
	tuple_type: TupleType,
	can_broadcast: bool,
}

impl OpCodeHandler_EVEX_KkHWIb_sae {
	#[cold]
	pub(super) fn new(base_reg: Register, code: Code, tuple_type: TupleType, can_broadcast: bool) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_KkHWIb_sae::decode, base_reg, code, tuple_type, can_broadcast }
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

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(instruction, decoder.state.reg + Register::K0 as u32);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg as u32);
		instruction.set_op3_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op2_kind(OpKind::Register);
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
pub(super) struct OpCodeHandler_EVEX_VkHW {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg1: Register,
	base_reg2: Register,
	base_reg3: Register,
	tuple_type: TupleType,
	can_broadcast: bool,
}

impl OpCodeHandler_EVEX_VkHW {
	#[cold]
	pub(super) fn new(base_reg: Register, code: Code, tuple_type: TupleType, can_broadcast: bool) -> Self {
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

	#[cold]
	pub(super) fn new1(
		base_reg1: Register, base_reg2: Register, base_reg3: Register, code: Code, tuple_type: TupleType, can_broadcast: bool,
	) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_VkHW::decode, base_reg1, base_reg2, base_reg3, code, tuple_type, can_broadcast }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		instruction.set_code(this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32
		);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg2 as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op2_kind(OpKind::Register);
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
pub(super) struct OpCodeHandler_EVEX_VkHM {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg1: Register,
	base_reg2: Register,
	tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_VkHM {
	#[cold]
	pub(super) fn new(base_reg: Register, code: Code, tuple_type: TupleType) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_VkHM::decode, base_reg1: base_reg, base_reg2: base_reg, code, tuple_type }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		instruction.set_code(this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32
		);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
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
pub(super) struct OpCodeHandler_EVEX_VkHWIb {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg1: Register,
	base_reg2: Register,
	base_reg3: Register,
	tuple_type: TupleType,
	can_broadcast: bool,
}

impl OpCodeHandler_EVEX_VkHWIb {
	#[cold]
	pub(super) fn new(base_reg: Register, code: Code, tuple_type: TupleType, can_broadcast: bool) -> Self {
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

	#[cold]
	pub(super) fn new1(
		base_reg1: Register, base_reg2: Register, base_reg3: Register, code: Code, tuple_type: TupleType, can_broadcast: bool,
	) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_VkHWIb::decode, base_reg1, base_reg2, base_reg3, code, tuple_type, can_broadcast }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		instruction.set_code(this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32
		);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg2 as u32);
		instruction.set_op3_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op2_kind(OpKind::Register);
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
pub(super) struct OpCodeHandler_EVEX_VkHWIb_er {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg1: Register,
	base_reg2: Register,
	base_reg3: Register,
	tuple_type: TupleType,
	can_broadcast: bool,
}

impl OpCodeHandler_EVEX_VkHWIb_er {
	#[cold]
	pub(super) fn new(base_reg: Register, code: Code, tuple_type: TupleType, can_broadcast: bool) -> Self {
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
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		instruction.set_code(this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32
		);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg2 as u32);
		instruction.set_op3_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op2_kind(OpKind::Register);
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
pub(super) struct OpCodeHandler_EVEX_KkHW {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg: Register,
	tuple_type: TupleType,
	can_broadcast: bool,
}

impl OpCodeHandler_EVEX_KkHW {
	#[cold]
	pub(super) fn new(base_reg: Register, code: Code, tuple_type: TupleType, can_broadcast: bool) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_KkHW::decode, base_reg, code, tuple_type, can_broadcast }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		instruction.set_code(this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(instruction, decoder.state.reg + Register::K0 as u32);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg as u32);
		if (((decoder.state.flags & StateFlags::Z) | decoder.state.extra_register_base | decoder.state.extra_register_base_evex)
			& decoder.invalid_check_mask)
			!= 0
		{
			decoder.set_invalid_instruction();
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op2_kind(OpKind::Register);
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
pub(super) struct OpCodeHandler_EVEX_KP1HW {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg: Register,
	tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_KP1HW {
	#[cold]
	pub(super) fn new(base_reg: Register, code: Code, tuple_type: TupleType) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_KP1HW::decode, base_reg, code, tuple_type }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		instruction.set_code(this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(instruction, decoder.state.reg + Register::K0 as u32);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg as u32);
		if (((decoder.state.flags & StateFlags::Z) | decoder.state.aaa | decoder.state.extra_register_base | decoder.state.extra_register_base_evex)
			& decoder.invalid_check_mask)
			!= 0
		{
			decoder.set_invalid_instruction();
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op2_kind(OpKind::Register);
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
pub(super) struct OpCodeHandler_EVEX_KkHWIb {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg: Register,
	tuple_type: TupleType,
	can_broadcast: bool,
}

impl OpCodeHandler_EVEX_KkHWIb {
	#[cold]
	pub(super) fn new(base_reg: Register, code: Code, tuple_type: TupleType, can_broadcast: bool) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_KkHWIb::decode, base_reg, code, tuple_type, can_broadcast }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		instruction.set_code(this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(instruction, decoder.state.reg + Register::K0 as u32);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op2_kind(OpKind::Register);
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
pub(super) struct OpCodeHandler_EVEX_WkHV {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg: Register,
}

impl OpCodeHandler_EVEX_WkHV {
	#[cold]
	pub(super) fn new(base_reg: Register, code: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_WkHV::decode, base_reg, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		instruction.set_code(this.code);

		debug_assert_eq!(decoder.state.mod_, 3);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg as u32);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg as u32);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op2_kind(OpKind::Register);
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
pub(super) struct OpCodeHandler_EVEX_VHWIb {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg: Register,
	tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_VHWIb {
	#[cold]
	pub(super) fn new(base_reg: Register, code: Code, tuple_type: TupleType) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_VHWIb::decode, base_reg, code, tuple_type }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if (((decoder.state.flags & (StateFlags::B | StateFlags::Z)) | decoder.state.aaa) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32
		);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg as u32);
		instruction.set_op3_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op2_kind(OpKind::Register);
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
pub(super) struct OpCodeHandler_EVEX_VHW {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code_r: Code,
	code_m: Code,
	base_reg1: Register,
	base_reg2: Register,
	base_reg3: Register,
	tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_VHW {
	#[cold]
	pub(super) fn new(base_reg: Register, code_r: Code, code_m: Code, tuple_type: TupleType) -> Self {
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

	#[cold]
	pub(super) fn new2(base_reg: Register, code: Code, tuple_type: TupleType) -> Self {
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
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if (((decoder.state.flags & (StateFlags::B | StateFlags::Z)) | decoder.state.aaa) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32
		);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		write_op1_reg!(instruction, decoder.state.vvvv + this.base_reg2 as u32);
		if decoder.state.mod_ == 3 {
			instruction.set_code(this.code_r);
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op2_kind(OpKind::Register);
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
pub(super) struct OpCodeHandler_EVEX_VHM {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg: Register,
	tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_VHM {
	#[cold]
	pub(super) fn new(base_reg: Register, code: Code, tuple_type: TupleType) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_VHM::decode, base_reg, code, tuple_type }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(decoder.state.encoding(), EncodingKind::EVEX as u32);
		if (((decoder.state.flags & (StateFlags::B | StateFlags::Z)) | decoder.state.aaa) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		instruction.set_code(this.code);

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32
		);
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
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
pub(super) struct OpCodeHandler_EVEX_Gv_W_er {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code_w0: Code,
	code_w1: Code,
	base_reg: Register,
	tuple_type: TupleType,
	only_sae: bool,
}

impl OpCodeHandler_EVEX_Gv_W_er {
	#[cold]
	pub(super) fn new(base_reg: Register, code_w0: Code, code_w1: Code, tuple_type: TupleType, only_sae: bool) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_Gv_W_er::decode, base_reg, code_w0, code_w1, tuple_type, only_sae }
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
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg as u32);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.only_sae {
					instruction.set_suppress_all_exceptions(true);
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
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code32: Code,
	code64: Code,
	tuple_type_w0: TupleType,
	tuple_type_w1: TupleType,
}

impl OpCodeHandler_EVEX_VX_Ev {
	#[cold]
	pub(super) fn new(code32: Code, code64: Code, tuple_type_w0: TupleType, tuple_type_w1: TupleType) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_VX_Ev::decode, code32, code64, tuple_type_w0, tuple_type_w1 }
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
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + Register::XMM0 as u32
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			instruction.set_op1_kind(OpKind::Memory);
			decoder.read_op_mem_tuple_type(instruction, tuple_type);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_EVEX_Ev_VX {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code32: Code,
	code64: Code,
	tuple_type_w0: TupleType,
	tuple_type_w1: TupleType,
}

impl OpCodeHandler_EVEX_Ev_VX {
	#[cold]
	pub(super) fn new(code32: Code, code64: Code, tuple_type_w0: TupleType, tuple_type_w1: TupleType) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_Ev_VX::decode, code32, code64, tuple_type_w0, tuple_type_w1 }
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
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		write_op1_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + Register::XMM0 as u32
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
			write_op0_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			instruction.set_op0_kind(OpKind::Memory);
			decoder.read_op_mem_tuple_type(instruction, tuple_type);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_EVEX_Ev_VX_Ib {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code32: Code,
	code64: Code,
	base_reg: Register,
}

impl OpCodeHandler_EVEX_Ev_VX_Ib {
	#[cold]
	pub(super) fn new(base_reg: Register, code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_Ev_VX_Ib::decode, base_reg, code32, code64 }
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
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
		write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base_evex + this.base_reg as u32);
		instruction.set_op2_kind(OpKind::Immediate8);
		instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
		if (decoder.state.flags & decoder.is64b_mode_and_w) != 0 {
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
pub(super) struct OpCodeHandler_EVEX_MV {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg: Register,
	tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_MV {
	#[cold]
	pub(super) fn new(base_reg: Register, code: Code, tuple_type: TupleType) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_MV::decode, base_reg, code, tuple_type }
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
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
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
pub(super) struct OpCodeHandler_EVEX_VkEv_REXW {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code32: Code,
	code64: Code,
	base_reg: Register,
}

impl OpCodeHandler_EVEX_VkEv_REXW {
	#[cold]
	pub(super) fn new(base_reg: Register, code32: Code, code64: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_VkEv_REXW::decode, base_reg, code32, code64 }
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

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
			write_op1_reg!(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_EVEX_Vk_VSIB {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg: Register,
	vsib_base: Register,
	tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_Vk_VSIB {
	#[cold]
	pub(super) fn new(base_reg: Register, vsib_base: Register, code: Code, tuple_type: TupleType) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_Vk_VSIB::decode, base_reg, vsib_base, code, tuple_type }
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
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
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
pub(super) struct OpCodeHandler_EVEX_VSIB_k1_VX {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	vsib_index: Register,
	base_reg: Register,
	tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_VSIB_k1_VX {
	#[cold]
	pub(super) fn new(vsib_index: Register, base_reg: Register, code: Code, tuple_type: TupleType) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_VSIB_k1_VX::decode, vsib_index, base_reg, code, tuple_type }
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

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
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
pub(super) struct OpCodeHandler_EVEX_VSIB_k1 {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	vsib_index: Register,
	tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_VSIB_k1 {
	#[cold]
	pub(super) fn new(vsib_index: Register, code: Code, tuple_type: TupleType) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_VSIB_k1::decode, vsib_index, code, tuple_type }
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
pub(super) struct OpCodeHandler_EVEX_GvM_VX_Ib {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code32: Code,
	code64: Code,
	base_reg: Register,
	tuple_type32: TupleType,
	tuple_type64: TupleType,
}

impl OpCodeHandler_EVEX_GvM_VX_Ib {
	#[cold]
	pub(super) fn new(base_reg: Register, code32: Code, code64: Code, tuple_type32: TupleType, tuple_type64: TupleType) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_GvM_VX_Ib::decode, base_reg, code32, code64, tuple_type32, tuple_type64 }
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
		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op1_kind(OpKind::Register);
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
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op0_kind(OpKind::Register);
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
pub(super) struct OpCodeHandler_EVEX_KkWIb {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
	base_reg: Register,
	tuple_type: TupleType,
	can_broadcast: bool,
}

impl OpCodeHandler_EVEX_KkWIb {
	#[cold]
	pub(super) fn new(base_reg: Register, code: Code, tuple_type: TupleType, can_broadcast: bool) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_EVEX_KkWIb::decode, base_reg, code, tuple_type, can_broadcast }
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

		const_assert_eq!(OpKind::Register as u32, 0);
		//instruction.set_op0_kind(OpKind::Register);
		write_op0_reg!(instruction, decoder.state.reg + Register::K0 as u32);
		instruction.set_op2_kind(OpKind::Immediate8);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(OpKind::Register as u32, 0);
			//instruction.set_op1_kind(OpKind::Register);
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
