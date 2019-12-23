/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

#![cfg_attr(feature = "cargo-clippy", allow(clippy::useless_let_if_seq))]

use super::super::*;
use super::handlers::*;
use super::*;

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_VectorLength_EVEX {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) handlers: [&'static OpCodeHandler; 4],
}

impl OpCodeHandler_VectorLength_EVEX {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		// Safe, array has 4 elements and vector_length is 0..3
		let handler = unsafe { *this.handlers.get_unchecked(decoder.state.vector_length as usize) };
		(handler.decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_VectorLength_EVEX_er {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) handlers: [&'static OpCodeHandler; 4],
}

impl OpCodeHandler_VectorLength_EVEX_er {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		let mut index = decoder.state.vector_length as usize;
		if decoder.state.mod_ == 3 && (decoder.state.flags & StateFlags::B) != 0 {
			index = VectorLength::L512 as usize;
		}
		// Safe, array has 4 elements and index is 0..3
		let handler = unsafe { *this.handlers.get_unchecked(index) };
		(handler.decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_EVEX {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) handler_mem: &'static OpCodeHandler,
}

impl OpCodeHandler_EVEX {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		if decoder.state.mod_ == 3 || decoder.is64_mode {
			decoder.evex_mvex(instruction);
		} else {
			let handler = this.handler_mem;
			(handler.decode)(handler, decoder, instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_EVEX_V_H_Ev_er {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code_w0: u32,
	pub(crate) code_w1: u32,
	pub(crate) base_reg: Register,
	pub(crate) tuple_type_w0: TupleType,
	pub(crate) tuple_type_w1: TupleType,
}

impl OpCodeHandler_EVEX_V_H_Ev_er {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		if (((decoder.state.flags & StateFlags::Z) | decoder.state.aaa) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		let gpr;
		let tuple_type;
		if (decoder.state.flags & decoder.is64_mode_and_w) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code_w1);
			tuple_type = this.tuple_type_w1;
			gpr = Register::RAX as u32;
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code_w0);
			tuple_type = this.tuple_type_w0;
			gpr = Register::EAX as u32;
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32,
		);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op2_register_u32(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
			if (decoder.state.flags & StateFlags::B) != 0 {
				const_assert_eq!(0, RoundingControl::None as u32);
				const_assert_eq!(1, RoundingControl::RoundToNearest as u32);
				const_assert_eq!(2, RoundingControl::RoundDown as u32);
				const_assert_eq!(3, RoundingControl::RoundUp as u32);
				const_assert_eq!(4, RoundingControl::RoundTowardZero as u32);
				super::instruction_internal::internal_set_rounding_control(instruction, decoder.state.vector_length + 1);
			}
		} else {
			super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Memory);
			decoder.read_op_mem_tuple_type(instruction, tuple_type);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_EVEX_V_H_Ev_Ib {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code_w0: u32,
	pub(crate) code_w1: u32,
	pub(crate) base_reg: Register,
	pub(crate) tuple_type_w0: TupleType,
	pub(crate) tuple_type_w1: TupleType,
}

impl OpCodeHandler_EVEX_V_H_Ev_Ib {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		if (((decoder.state.flags & (StateFlags::B | StateFlags::Z)) | decoder.state.aaa) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		let gpr;
		if (decoder.state.flags & decoder.is64_mode_and_w) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code_w1);
			gpr = Register::RAX as u32;
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code_w0);
			gpr = Register::EAX as u32;
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32,
		);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op2_register_u32(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Memory);
			if (decoder.state.flags & decoder.is64_mode_and_w) != 0 {
				decoder.read_op_mem_tuple_type(instruction, this.tuple_type_w1);
			} else {
				decoder.read_op_mem_tuple_type(instruction, this.tuple_type_w0);
			}
		}
		super::instruction_internal::internal_set_op3_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_EVEX_Ed_V_Ib {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
	pub(crate) base_reg: Register,
	pub(crate) tuple_type32: TupleType,
	pub(crate) tuple_type64: TupleType,
}

impl OpCodeHandler_EVEX_Ed_V_Ib {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		if (((decoder.state.flags & (StateFlags::B | StateFlags::Z)) | decoder.state.vvvv | decoder.state.aaa) & decoder.invalid_check_mask) != 0 {
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
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			if (decoder.state.flags & decoder.is64_mode_and_w) != 0 {
				decoder.read_op_mem_tuple_type(instruction, this.tuple_type64);
			} else {
				decoder.read_op_mem_tuple_type(instruction, this.tuple_type32);
			}
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32,
		);
		super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_EVEX_VkHW_er {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg: Register,
	pub(crate) tuple_type: TupleType,
	pub(crate) only_sae: bool,
	pub(crate) can_broadcast: bool,
}

impl OpCodeHandler_EVEX_VkHW_er {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32,
		);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op2_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + decoder.state.extra_base_register_base_evex + this.base_reg as u32,
			);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.only_sae {
					super::instruction_internal::internal_set_suppress_all_exceptions(instruction);
				} else {
					const_assert_eq!(0, RoundingControl::None as u32);
					const_assert_eq!(1, RoundingControl::RoundToNearest as u32);
					const_assert_eq!(2, RoundingControl::RoundDown as u32);
					const_assert_eq!(3, RoundingControl::RoundUp as u32);
					const_assert_eq!(4, RoundingControl::RoundTowardZero as u32);
					super::instruction_internal::internal_set_rounding_control(instruction, decoder.state.vector_length + 1);
				}
			}
		} else {
			super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.can_broadcast {
					super::instruction_internal::internal_set_is_broadcast(instruction);
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
pub(crate) struct OpCodeHandler_EVEX_VkW_er {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg1: Register,
	pub(crate) base_reg2: Register,
	pub(crate) tuple_type: TupleType,
	pub(crate) only_sae: bool,
	pub(crate) can_broadcast: bool,
}

impl OpCodeHandler_EVEX_VkW_er {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		if (decoder.state.vvvv & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32,
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + decoder.state.extra_base_register_base_evex + this.base_reg2 as u32,
			);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.only_sae {
					super::instruction_internal::internal_set_suppress_all_exceptions(instruction);
				} else {
					const_assert_eq!(0, RoundingControl::None as u32);
					const_assert_eq!(1, RoundingControl::RoundToNearest as u32);
					const_assert_eq!(2, RoundingControl::RoundDown as u32);
					const_assert_eq!(3, RoundingControl::RoundUp as u32);
					const_assert_eq!(4, RoundingControl::RoundTowardZero as u32);
					super::instruction_internal::internal_set_rounding_control(instruction, decoder.state.vector_length + 1);
				}
			}
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.can_broadcast {
					super::instruction_internal::internal_set_is_broadcast(instruction);
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
pub(crate) struct OpCodeHandler_EVEX_VkWIb_er {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg1: Register,
	pub(crate) base_reg2: Register,
	pub(crate) tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_VkWIb_er {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		if (decoder.state.vvvv & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32,
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + decoder.state.extra_base_register_base_evex + this.base_reg2 as u32,
			);
			if (decoder.state.flags & StateFlags::B) != 0 {
				super::instruction_internal::internal_set_suppress_all_exceptions(instruction);
			}
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				super::instruction_internal::internal_set_is_broadcast(instruction);
			}
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
		super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_EVEX_VkW {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg1: Register,
	pub(crate) base_reg2: Register,
	pub(crate) tuple_type: TupleType,
	pub(crate) can_broadcast: bool,
}

impl OpCodeHandler_EVEX_VkW {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		if (decoder.state.vvvv & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32,
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + decoder.state.extra_base_register_base_evex + this.base_reg2 as u32,
			);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.can_broadcast {
					super::instruction_internal::internal_set_is_broadcast(instruction);
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
pub(crate) struct OpCodeHandler_EVEX_WkV {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) disallow_zeroing_masking: u32,
	pub(crate) code: u32,
	pub(crate) base_reg1: Register,
	pub(crate) base_reg2: Register,
	pub(crate) tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_WkV {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		if (((decoder.state.flags & StateFlags::B) | decoder.state.vvvv) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);

		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + decoder.state.extra_base_register_base_evex + this.base_reg1 as u32,
			);
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg2 as u32,
		);
		if ((decoder.state.flags & StateFlags::Z) & this.disallow_zeroing_masking & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_EVEX_VkM {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg: Register,
	pub(crate) tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_VkM {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		if (((decoder.state.flags & StateFlags::B) | decoder.state.vvvv) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32,
		);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_EVEX_VkWIb {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg1: Register,
	pub(crate) base_reg2: Register,
	pub(crate) tuple_type: TupleType,
	pub(crate) can_broadcast: bool,
}

impl OpCodeHandler_EVEX_VkWIb {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		if (decoder.state.vvvv & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32,
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + decoder.state.extra_base_register_base_evex + this.base_reg2 as u32,
			);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.can_broadcast {
					super::instruction_internal::internal_set_is_broadcast(instruction);
				} else if decoder.invalid_check_mask != 0 {
					decoder.set_invalid_instruction();
				}
			}
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
		super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_EVEX_WkVIb {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg1: Register,
	pub(crate) base_reg2: Register,
	pub(crate) tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_WkVIb {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		if (((decoder.state.flags & StateFlags::B) | decoder.state.vvvv) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);

		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + decoder.state.extra_base_register_base_evex + this.base_reg1 as u32,
			);
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg2 as u32,
		);
		super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_EVEX_HkWIb {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg1: Register,
	pub(crate) base_reg2: Register,
	pub(crate) tuple_type: TupleType,
	pub(crate) can_broadcast: bool,
}

impl OpCodeHandler_EVEX_HkWIb {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.vvvv + this.base_reg1 as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + decoder.state.extra_base_register_base_evex + this.base_reg2 as u32,
			);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.can_broadcast {
					super::instruction_internal::internal_set_is_broadcast(instruction);
				} else if decoder.invalid_check_mask != 0 {
					decoder.set_invalid_instruction();
				}
			}
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
		super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_EVEX_HWIb {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg1: Register,
	pub(crate) base_reg2: Register,
	pub(crate) tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_HWIb {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		if (((decoder.state.flags & (StateFlags::Z | StateFlags::B)) | decoder.state.aaa) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}

		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.vvvv + this.base_reg1 as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + decoder.state.extra_base_register_base_evex + this.base_reg2 as u32,
			);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
		super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_EVEX_WkVIb_er {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg1: Register,
	pub(crate) base_reg2: Register,
	pub(crate) tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_WkVIb_er {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		if (decoder.state.vvvv & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);

		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + decoder.state.extra_base_register_base_evex + this.base_reg1 as u32,
			);
			if (decoder.state.flags & StateFlags::B) != 0 {
				super::instruction_internal::internal_set_suppress_all_exceptions(instruction);
			}
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg2 as u32,
		);
		super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_EVEX_VW_er {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg1: Register,
	pub(crate) base_reg2: Register,
	pub(crate) tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_VW_er {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32,
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + decoder.state.extra_base_register_base_evex + this.base_reg2 as u32,
			);
			if (decoder.state.flags & StateFlags::B) != 0 {
				super::instruction_internal::internal_set_suppress_all_exceptions(instruction);
			}
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
		if (((decoder.state.flags & StateFlags::Z) | decoder.state.vvvv | decoder.state.aaa) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_EVEX_VW {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg1: Register,
	pub(crate) base_reg2: Register,
	pub(crate) tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_VW {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		if (((decoder.state.flags & (StateFlags::Z | StateFlags::B)) | decoder.state.vvvv | decoder.state.aaa) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32,
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + decoder.state.extra_base_register_base_evex + this.base_reg2 as u32,
			);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_EVEX_WV {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg1: Register,
	pub(crate) base_reg2: Register,
	pub(crate) tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_WV {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		if (((decoder.state.flags & (StateFlags::Z | StateFlags::B)) | decoder.state.vvvv | decoder.state.aaa) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);

		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + decoder.state.extra_base_register_base_evex + this.base_reg2 as u32,
			);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32,
		);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_EVEX_VM {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg: Register,
	pub(crate) tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_VM {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		if (((decoder.state.flags & (StateFlags::Z | StateFlags::B)) | decoder.state.vvvv | decoder.state.aaa) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32,
		);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_EVEX_VK {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg: Register,
}

impl OpCodeHandler_EVEX_VK {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		if (((decoder.state.flags & (StateFlags::B | StateFlags::Z)) | decoder.state.vvvv | decoder.state.aaa) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32,
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.rm + Register::K0 as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_EVEX_KR {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg: Register,
}

impl OpCodeHandler_EVEX_KR {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		if (((decoder.state.flags & (StateFlags::B | StateFlags::Z))
			| decoder.state.vvvv
			| decoder.state.aaa
			| decoder.state.extra_register_base
			| decoder.state.extra_register_base_evex)
			& decoder.invalid_check_mask)
			!= 0
		{
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.reg + Register::K0 as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + decoder.state.extra_base_register_base_evex + this.base_reg as u32,
			);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_EVEX_KkHWIb_sae {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg: Register,
	pub(crate) tuple_type: TupleType,
	pub(crate) can_broadcast: bool,
}

impl OpCodeHandler_EVEX_KkHWIb_sae {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		if (((decoder.state.flags & StateFlags::Z) | decoder.state.extra_register_base | decoder.state.extra_register_base_evex)
			& decoder.invalid_check_mask)
			!= 0
		{
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.reg + Register::K0 as u32);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op2_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + decoder.state.extra_base_register_base_evex + this.base_reg as u32,
			);
			if (decoder.state.flags & StateFlags::B) != 0 {
				super::instruction_internal::internal_set_suppress_all_exceptions(instruction);
			}
		} else {
			super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.can_broadcast {
					super::instruction_internal::internal_set_is_broadcast(instruction);
				} else if decoder.invalid_check_mask != 0 {
					decoder.set_invalid_instruction();
				}
			}
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
		super::instruction_internal::internal_set_op3_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_EVEX_VkHW {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg1: Register,
	pub(crate) base_reg2: Register,
	pub(crate) base_reg3: Register,
	pub(crate) tuple_type: TupleType,
	pub(crate) can_broadcast: bool,
}

impl OpCodeHandler_EVEX_VkHW {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32,
		);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg2 as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op2_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + decoder.state.extra_base_register_base_evex + this.base_reg3 as u32,
			);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.can_broadcast {
					super::instruction_internal::internal_set_is_broadcast(instruction);
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
pub(crate) struct OpCodeHandler_EVEX_VkHM {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg1: Register,
	pub(crate) base_reg2: Register,
	pub(crate) tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_VkHM {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32,
		);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg2 as u32);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Memory);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_EVEX_VkHWIb {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg1: Register,
	pub(crate) base_reg2: Register,
	pub(crate) base_reg3: Register,
	pub(crate) tuple_type: TupleType,
	pub(crate) can_broadcast: bool,
}

impl OpCodeHandler_EVEX_VkHWIb {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32,
		);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg2 as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op2_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + decoder.state.extra_base_register_base_evex + this.base_reg3 as u32,
			);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.can_broadcast {
					super::instruction_internal::internal_set_is_broadcast(instruction);
				} else if decoder.invalid_check_mask != 0 {
					decoder.set_invalid_instruction();
				}
			}
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
		super::instruction_internal::internal_set_op3_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_EVEX_VkHWIb_er {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg1: Register,
	pub(crate) base_reg2: Register,
	pub(crate) base_reg3: Register,
	pub(crate) tuple_type: TupleType,
	pub(crate) can_broadcast: bool,
}

impl OpCodeHandler_EVEX_VkHWIb_er {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32,
		);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg2 as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op2_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + decoder.state.extra_base_register_base_evex + this.base_reg3 as u32,
			);
			if (decoder.state.flags & StateFlags::B) != 0 {
				super::instruction_internal::internal_set_suppress_all_exceptions(instruction);
			}
		} else {
			super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.can_broadcast {
					super::instruction_internal::internal_set_is_broadcast(instruction);
				} else if decoder.invalid_check_mask != 0 {
					decoder.set_invalid_instruction();
				}
			}
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
		super::instruction_internal::internal_set_op3_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_EVEX_KkHW {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg: Register,
	pub(crate) tuple_type: TupleType,
	pub(crate) can_broadcast: bool,
}

impl OpCodeHandler_EVEX_KkHW {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.reg + Register::K0 as u32);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op2_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + decoder.state.extra_base_register_base_evex + this.base_reg as u32,
			);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.can_broadcast {
					super::instruction_internal::internal_set_is_broadcast(instruction);
				} else if decoder.invalid_check_mask != 0 {
					decoder.set_invalid_instruction();
				}
			}
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
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
pub(crate) struct OpCodeHandler_EVEX_KP1HW {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg: Register,
	pub(crate) tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_KP1HW {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.reg + Register::K0 as u32);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op2_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + decoder.state.extra_base_register_base_evex + this.base_reg as u32,
			);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				super::instruction_internal::internal_set_is_broadcast(instruction);
			}
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
		if (((decoder.state.flags & StateFlags::Z) | decoder.state.aaa | decoder.state.extra_register_base | decoder.state.extra_register_base_evex)
			& decoder.invalid_check_mask)
			!= 0
		{
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_EVEX_KkHWIb {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg: Register,
	pub(crate) tuple_type: TupleType,
	pub(crate) can_broadcast: bool,
}

impl OpCodeHandler_EVEX_KkHWIb {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.reg + Register::K0 as u32);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op2_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + decoder.state.extra_base_register_base_evex + this.base_reg as u32,
			);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.can_broadcast {
					super::instruction_internal::internal_set_is_broadcast(instruction);
				} else if decoder.invalid_check_mask != 0 {
					decoder.set_invalid_instruction();
				}
			}
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
		super::instruction_internal::internal_set_op3_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
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
pub(crate) struct OpCodeHandler_EVEX_WkHV {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg: Register,
	pub(crate) tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_WkHV {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);

		debug_assert_eq!(3, decoder.state.mod_);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.rm + decoder.state.extra_base_register_base + decoder.state.extra_base_register_base_evex + this.base_reg as u32,
		);
		if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg as u32);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op2_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32,
		);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_EVEX_VHWIb {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg: Register,
	pub(crate) tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_VHWIb {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		if (((decoder.state.flags & (StateFlags::B | StateFlags::Z)) | decoder.state.aaa) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32,
		);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op2_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + decoder.state.extra_base_register_base_evex + this.base_reg as u32,
			);
		} else {
			super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Memory);
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
		super::instruction_internal::internal_set_op3_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_EVEX_VHW {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code_r: u32,
	pub(crate) code_m: u32,
	pub(crate) base_reg1: Register,
	pub(crate) base_reg2: Register,
	pub(crate) base_reg3: Register,
	pub(crate) tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_VHW {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		if (((decoder.state.flags & (StateFlags::B | StateFlags::Z)) | decoder.state.aaa) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}

		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg1 as u32,
		);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg2 as u32);
		if decoder.state.mod_ == 3 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code_r);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op2_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + decoder.state.extra_base_register_base_evex + this.base_reg3 as u32,
			);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code_m);
			super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Memory);
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_EVEX_VHM {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg: Register,
	pub(crate) tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_VHM {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		if (((decoder.state.flags & (StateFlags::B | StateFlags::Z)) | decoder.state.aaa) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32,
		);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.vvvv + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Memory);
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_EVEX_Gv_W_er {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code_w0: u32,
	pub(crate) code_w1: u32,
	pub(crate) base_reg: Register,
	pub(crate) tuple_type: TupleType,
	pub(crate) only_sae: bool,
}

impl OpCodeHandler_EVEX_Gv_W_er {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		if (((decoder.state.flags & StateFlags::Z) | decoder.state.vvvv | decoder.state.aaa | decoder.state.extra_register_base_evex)
			& decoder.invalid_check_mask)
			!= 0
		{
			decoder.set_invalid_instruction();
		}
		if (decoder.state.flags & decoder.is64_mode_and_w) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code_w1);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32,
			);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code_w0);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32,
			);
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + decoder.state.extra_base_register_base_evex + this.base_reg as u32,
			);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.only_sae {
					super::instruction_internal::internal_set_suppress_all_exceptions(instruction);
				} else {
					const_assert_eq!(0, RoundingControl::None as u32);
					const_assert_eq!(1, RoundingControl::RoundToNearest as u32);
					const_assert_eq!(2, RoundingControl::RoundDown as u32);
					const_assert_eq!(3, RoundingControl::RoundUp as u32);
					const_assert_eq!(4, RoundingControl::RoundTowardZero as u32);
					super::instruction_internal::internal_set_rounding_control(instruction, decoder.state.vector_length + 1);
				}
			}
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_EVEX_VX_Ev {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
	pub(crate) tuple_type_w0: TupleType,
	pub(crate) tuple_type_w1: TupleType,
}

impl OpCodeHandler_EVEX_VX_Ev {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		if (((decoder.state.flags & (StateFlags::B | StateFlags::Z)) | decoder.state.vvvv | decoder.state.aaa) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		let gpr;
		let tuple_type;
		if (decoder.state.flags & decoder.is64_mode_and_w) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			tuple_type = this.tuple_type_w1;
			gpr = Register::RAX as u32;
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			tuple_type = this.tuple_type_w0;
			gpr = Register::EAX as u32;
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + Register::XMM0 as u32,
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem_tuple_type(instruction, tuple_type);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_EVEX_Ev_VX {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
	pub(crate) tuple_type_w0: TupleType,
	pub(crate) tuple_type_w1: TupleType,
}

impl OpCodeHandler_EVEX_Ev_VX {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		if (((decoder.state.flags & (StateFlags::B | StateFlags::Z)) | decoder.state.vvvv | decoder.state.aaa) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		let gpr;
		let tuple_type;
		if (decoder.state.flags & decoder.is64_mode_and_w) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			tuple_type = this.tuple_type_w1;
			gpr = Register::RAX as u32;
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			tuple_type = this.tuple_type_w0;
			gpr = Register::EAX as u32;
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem_tuple_type(instruction, tuple_type);
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + Register::XMM0 as u32,
		);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_EVEX_Ev_VX_Ib {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
	pub(crate) base_reg: Register,
}

impl OpCodeHandler_EVEX_Ev_VX_Ib {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		if (((decoder.state.flags & (StateFlags::B | StateFlags::Z))
			| decoder.state.vvvv
			| decoder.state.aaa
			| decoder.state.extra_register_base_evex)
			& decoder.invalid_check_mask)
			!= 0
		{
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
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.reg + decoder.state.extra_register_base + gpr);
		debug_assert_eq!(3, decoder.state.mod_);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(
			instruction,
			decoder.state.rm + decoder.state.extra_base_register_base + decoder.state.extra_base_register_base_evex + this.base_reg as u32,
		);
		super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_EVEX_MV {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg: Register,
	pub(crate) tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_MV {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		if (((decoder.state.flags & (StateFlags::B | StateFlags::Z)) | decoder.state.vvvv | decoder.state.aaa) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);

		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32,
		);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_EVEX_VkEv_REXW {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
	pub(crate) base_reg: Register,
}

impl OpCodeHandler_EVEX_VkEv_REXW {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		if (((decoder.state.flags & StateFlags::B) | decoder.state.vvvv) & decoder.invalid_check_mask) != 0 {
			decoder.set_invalid_instruction();
		}
		let gpr;
		if (decoder.state.flags & decoder.is64_mode_and_w) != 0 {
			debug_assert_ne!(Code::INVALID as u32, this.code64);
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			gpr = Register::RAX as u32;
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			gpr = Register::EAX as u32;
		}

		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32,
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_EVEX_Vk_VSIB {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg: Register,
	pub(crate) vsib_base: Register,
	pub(crate) tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_Vk_VSIB {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		if decoder.invalid_check_mask != 0
			&& (((decoder.state.flags & (StateFlags::Z | StateFlags::B)) | (decoder.state.vvvv & 0xF)) != 0 || decoder.state.aaa == 0)
		{
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);

		let reg_num = decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex;
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, reg_num + this.base_reg as u32);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
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
pub(crate) struct OpCodeHandler_EVEX_VSIB_k1_VX {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) vsib_index: Register,
	pub(crate) base_reg: Register,
	pub(crate) tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_VSIB_k1_VX {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		if decoder.invalid_check_mask != 0
			&& (((decoder.state.flags & (StateFlags::B | StateFlags::Z)) | (decoder.state.vvvv & 0xF)) != 0 || decoder.state.aaa == 0)
		{
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32,
		);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem_vsib(instruction, this.vsib_index, this.tuple_type);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_EVEX_VSIB_k1 {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) vsib_index: Register,
	pub(crate) tuple_type: TupleType,
}

impl OpCodeHandler_EVEX_VSIB_k1 {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		if decoder.invalid_check_mask != 0
			&& (((decoder.state.flags & (StateFlags::B | StateFlags::Z)) | (decoder.state.vvvv & 0xF)) != 0 || decoder.state.aaa == 0)
		{
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);

		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem_vsib(instruction, this.vsib_index, this.tuple_type);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_EVEX_GvM_VX_Ib {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
	pub(crate) base_reg: Register,
	pub(crate) tuple_type32: TupleType,
	pub(crate) tuple_type64: TupleType,
}

impl OpCodeHandler_EVEX_GvM_VX_Ib {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		if (((decoder.state.flags & (StateFlags::B | StateFlags::Z)) | decoder.state.vvvv | decoder.state.aaa) & decoder.invalid_check_mask) != 0 {
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
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.rm + decoder.state.extra_base_register_base + gpr);
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			if (decoder.state.flags & decoder.is64_mode_and_w) != 0 {
				decoder.read_op_mem_tuple_type(instruction, this.tuple_type64);
			} else {
				decoder.read_op_mem_tuple_type(instruction, this.tuple_type32);
			}
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + decoder.state.extra_register_base_evex + this.base_reg as u32,
		);
		super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_EVEX_KkWIb {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg: Register,
	pub(crate) tuple_type: TupleType,
	pub(crate) can_broadcast: bool,
}

impl OpCodeHandler_EVEX_KkWIb {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::EVEX, decoder.state.encoding());
		if (((decoder.state.flags & StateFlags::Z) | decoder.state.vvvv | decoder.state.extra_register_base | decoder.state.extra_register_base_evex)
			& decoder.invalid_check_mask)
			!= 0
		{
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);

		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.reg + Register::K0 as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + decoder.state.extra_base_register_base_evex + this.base_reg as u32,
			);
			if ((decoder.state.flags & StateFlags::B) & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			if (decoder.state.flags & StateFlags::B) != 0 {
				if this.can_broadcast {
					super::instruction_internal::internal_set_is_broadcast(instruction);
				} else if decoder.invalid_check_mask != 0 {
					decoder.set_invalid_instruction();
				}
			}
			decoder.read_op_mem_tuple_type(instruction, this.tuple_type);
		}
		super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}
