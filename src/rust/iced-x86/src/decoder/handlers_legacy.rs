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

use super::handlers::*;
use super::*;

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Reg {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) reg: Register,
}

impl OpCodeHandler_Reg {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register(instruction, this.reg);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_RegIb {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) reg: Register,
}

impl OpCodeHandler_RegIb {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register(instruction, this.reg);
		super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_IbReg {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) reg: Register,
}

impl OpCodeHandler_IbReg {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register(instruction, this.reg);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_AL_DX {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
}

impl OpCodeHandler_AL_DX {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register(instruction, Register::AL);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register(instruction, Register::DX);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_DX_AL {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
}

impl OpCodeHandler_DX_AL {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register(instruction, Register::DX);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register(instruction, Register::AL);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Ib {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
}

impl OpCodeHandler_Ib {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Ib3 {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
}

impl OpCodeHandler_Ib3 {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_MandatoryPrefix {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) handlers: [&'static OpCodeHandler; 4],
}

impl OpCodeHandler_MandatoryPrefix {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		decoder.clear_mandatory_prefix(instruction);
		// Safe, array has 4 elements and mandatory_prefix is 0..3
		let handler = unsafe { *this.handlers.get_unchecked(decoder.state.mandatory_prefix as usize) };
		(handler.decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_MandatoryPrefix3 {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) handlers_reg: [(&'static OpCodeHandler, bool); 4],
	pub(crate) handlers_mem: [(&'static OpCodeHandler, bool); 4],
}

impl OpCodeHandler_MandatoryPrefix3 {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		// Safe, array has 4 elements and mandatory_prefix is 0..3
		let (handler, mandatory_prefix) = unsafe {
			if decoder.state.mod_ == 3 {
				*this.handlers_reg.get_unchecked(decoder.state.mandatory_prefix as usize)
			} else {
				*this.handlers_mem.get_unchecked(decoder.state.mandatory_prefix as usize)
			}
		};
		if mandatory_prefix {
			decoder.clear_mandatory_prefix(instruction);
		}
		(handler.decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_MandatoryPrefix_F3_F2 {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) handler_normal: &'static OpCodeHandler,
	pub(crate) handler_f3: &'static OpCodeHandler,
	pub(crate) handler_f2: &'static OpCodeHandler,
	pub(crate) clear_f3: bool,
	pub(crate) clear_f2: bool,
}

impl OpCodeHandler_MandatoryPrefix_F3_F2 {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		let handler;
		let prefix = decoder.state.mandatory_prefix;
		if prefix == MandatoryPrefixByte::PF3 as u32 {
			if this.clear_f3 {
				decoder.clear_mandatory_prefix_f3(instruction);
			}
			handler = this.handler_f3;
		} else if prefix == MandatoryPrefixByte::PF2 as u32 {
			if this.clear_f2 {
				decoder.clear_mandatory_prefix_f2(instruction);
			}
			handler = this.handler_f2;
		} else {
			debug_assert!(prefix == MandatoryPrefixByte::None as u32 || prefix == MandatoryPrefixByte::P66 as u32);
			handler = this.handler_normal;
		}
		if handler.has_modrm {
			decoder.read_modrm();
		}
		(handler.decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_NIb {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
}

impl OpCodeHandler_NIb {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.rm + Register::MM0 as u32);
		} else {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_ReservedNop {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) reserved_nop_handler: &'static OpCodeHandler,
	pub(crate) other_handler: &'static OpCodeHandler,
}

impl OpCodeHandler_ReservedNop {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		let handler = if (decoder.options & DecoderOptions::FORCE_RESERVED_NOP) != 0 { this.reserved_nop_handler } else { this.other_handler };
		(handler.decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Ev_Iz {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) flags: u32,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Ev_Iz {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			let index = decoder.state.rm + decoder.state.extra_base_register_base;
			if decoder.state.operand_size == OpSize::Size32 {
				super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::EAX as u32);
			} else if decoder.state.operand_size == OpSize::Size64 {
				super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::RAX as u32);
			} else {
				super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::AX as u32);
			}
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
			if (this.flags & (HandlerFlags::XACQUIRE | HandlerFlags::XRELEASE)) != 0 {
				decoder.set_xacquire_xrelease(instruction, this.flags);
			}
			const_assert_eq!(8, HandlerFlags::LOCK);
			const_assert_eq!(0x0000_2000, StateFlags::ALLOW_LOCK);
			decoder.state.flags |= (this.flags & HandlerFlags::LOCK) << (13 - 3);
		}
		if decoder.state.operand_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Immediate32);
			instruction.set_immediate32(decoder.read_u32() as u32);
		} else if decoder.state.operand_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Immediate32to64);
			instruction.set_immediate32(decoder.read_u32() as u32);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Immediate16);
			super::instruction_internal::internal_set_immediate16(instruction, decoder.read_u16() as u32);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Ev_Ib {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) flags: u32,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Ev_Ib {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			let index = decoder.state.rm + decoder.state.extra_base_register_base;
			if decoder.state.operand_size == OpSize::Size32 {
				super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::EAX as u32);
			} else if decoder.state.operand_size == OpSize::Size64 {
				super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::RAX as u32);
			} else {
				super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::AX as u32);
			}
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
			if (this.flags & (HandlerFlags::XACQUIRE | HandlerFlags::XRELEASE)) != 0 {
				decoder.set_xacquire_xrelease(instruction, this.flags);
			}
			const_assert_eq!(8, HandlerFlags::LOCK);
			const_assert_eq!(0x0000_2000, StateFlags::ALLOW_LOCK);
			decoder.state.flags |= (this.flags & HandlerFlags::LOCK) << (13 - 3);
		}
		if decoder.state.operand_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Immediate8to32);
			super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
		} else if decoder.state.operand_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Immediate8to64);
			super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Immediate8to16);
			super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Ev_Ib2 {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) flags: u32,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Ev_Ib2 {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.operand_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
		} else if decoder.state.operand_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			let index = decoder.state.rm + decoder.state.extra_base_register_base;
			if decoder.state.operand_size == OpSize::Size32 {
				super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::EAX as u32);
			} else if decoder.state.operand_size == OpSize::Size64 {
				super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::RAX as u32);
			} else {
				super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::AX as u32);
			}
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
			if (this.flags & (HandlerFlags::XACQUIRE | HandlerFlags::XRELEASE)) != 0 {
				decoder.set_xacquire_xrelease(instruction, this.flags);
			}
			const_assert_eq!(8, HandlerFlags::LOCK);
			const_assert_eq!(0x0000_2000, StateFlags::ALLOW_LOCK);
			decoder.state.flags |= (this.flags & HandlerFlags::LOCK) << (13 - 3);
		}
		super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Ev_1 {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Ev_1 {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.operand_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
		} else if decoder.state.operand_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			let index = decoder.state.rm + decoder.state.extra_base_register_base;
			if decoder.state.operand_size == OpSize::Size32 {
				super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::EAX as u32);
			} else if decoder.state.operand_size == OpSize::Size64 {
				super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::RAX as u32);
			} else {
				super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::AX as u32);
			}
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, 1);
		decoder.state.flags |= StateFlags::NO_IMM;
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Ev_CL {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Ev_CL {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.operand_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
		} else if decoder.state.operand_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			let index = decoder.state.rm + decoder.state.extra_base_register_base;
			if decoder.state.operand_size == OpSize::Size32 {
				super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::EAX as u32);
			} else if decoder.state.operand_size == OpSize::Size64 {
				super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::RAX as u32);
			} else {
				super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::AX as u32);
			}
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, Register::CL as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Ev {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) flags: u32,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Ev {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.operand_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
		} else if decoder.state.operand_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			let index = decoder.state.rm + decoder.state.extra_base_register_base;
			if decoder.state.operand_size == OpSize::Size32 {
				super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::EAX as u32);
			} else if decoder.state.operand_size == OpSize::Size64 {
				super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::RAX as u32);
			} else {
				super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::AX as u32);
			}
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
			if (this.flags & (HandlerFlags::XACQUIRE | HandlerFlags::XRELEASE)) != 0 {
				decoder.set_xacquire_xrelease(instruction, this.flags);
			}
			const_assert_eq!(8, HandlerFlags::LOCK);
			const_assert_eq!(0x0000_2000, StateFlags::ALLOW_LOCK);
			decoder.state.flags |= (this.flags & HandlerFlags::LOCK) << (13 - 3);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Rv {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Rv {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.operand_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + Register::EAX as u32,
			);
		} else if decoder.state.operand_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + Register::RAX as u32,
			);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + Register::AX as u32,
			);
		}
		debug_assert_eq!(3, decoder.state.mod_);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Rv_32_64 {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Rv_32_64 {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		let base_reg;
		if decoder.is64_mode {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			base_reg = Register::RAX;
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			base_reg = Register::EAX;
		}
		debug_assert_eq!(3, decoder.state.mod_);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.rm + decoder.state.extra_base_register_base + base_reg as u32,
		);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Ev_REXW {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) disallow_reg: u32,
	pub(crate) disallow_mem: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Ev_REXW {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if (decoder.state.flags & StateFlags::W) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			if (decoder.state.flags & StateFlags::W) != 0 {
				super::instruction_internal::internal_set_op0_register_u32(
					instruction,
					decoder.state.rm + decoder.state.extra_base_register_base + Register::RAX as u32,
				);
			} else {
				super::instruction_internal::internal_set_op0_register_u32(
					instruction,
					decoder.state.rm + decoder.state.extra_base_register_base + Register::EAX as u32,
				);
			}
			if (this.disallow_reg & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
			if (this.disallow_mem & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Evj {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Evj {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.is64_mode {
			if (decoder.options & DecoderOptions::AMD_BRANCHES) == 0 || decoder.state.operand_size == OpSize::Size32 {
				super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			} else {
				super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			}
			if decoder.state.mod_ == 3 {
				const_assert_eq!(0, OpKind::Register as u32);
				//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
				if (decoder.options & DecoderOptions::AMD_BRANCHES) == 0 || decoder.state.operand_size == OpSize::Size32 {
					super::instruction_internal::internal_set_op0_register_u32(
						instruction,
						decoder.state.rm + decoder.state.extra_base_register_base + Register::RAX as u32,
					);
				} else {
					super::instruction_internal::internal_set_op0_register_u32(
						instruction,
						decoder.state.rm + decoder.state.extra_base_register_base + Register::AX as u32,
					);
				}
			} else {
				super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
				decoder.read_op_mem(instruction);
			}
		} else {
			if decoder.state.operand_size == OpSize::Size32 {
				super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			} else {
				super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			}
			if decoder.state.mod_ == 3 {
				const_assert_eq!(0, OpKind::Register as u32);
				//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
				if decoder.state.operand_size == OpSize::Size32 {
					super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.rm + Register::EAX as u32);
				} else {
					super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.rm + Register::AX as u32);
				}
			} else {
				super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
				decoder.read_op_mem(instruction);
			}
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Ep {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Ep {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.operand_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
		} else if decoder.state.operand_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
		}
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
pub(crate) struct OpCodeHandler_Evw {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Evw {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.operand_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
		} else if decoder.state.operand_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			let index = decoder.state.rm + decoder.state.extra_base_register_base;
			if decoder.state.operand_size == OpSize::Size32 {
				super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::EAX as u32);
			} else if decoder.state.operand_size == OpSize::Size64 {
				super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::RAX as u32);
			} else {
				super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::AX as u32);
			}
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Ew {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Ew {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.operand_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
		} else if decoder.state.operand_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			let index = decoder.state.rm + decoder.state.extra_base_register_base;
			if decoder.state.operand_size == OpSize::Size32 {
				super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::EAX as u32);
			} else if decoder.state.operand_size == OpSize::Size64 {
				super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::RAX as u32);
			} else {
				super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::AX as u32);
			}
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Ms {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Ms {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.is64_mode {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
		} else if decoder.state.operand_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
		}
		debug_assert_ne!(3, decoder.state.mod_);
		super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
		decoder.read_op_mem(instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Gv_Ev {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Gv_Ev {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.operand_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32,
			);
		} else if decoder.state.operand_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32,
			);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::AX as u32,
			);
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			let index = decoder.state.rm + decoder.state.extra_base_register_base;
			if decoder.state.operand_size == OpSize::Size32 {
				super::instruction_internal::internal_set_op1_register_u32(instruction, index + Register::EAX as u32);
			} else if decoder.state.operand_size == OpSize::Size64 {
				super::instruction_internal::internal_set_op1_register_u32(instruction, index + Register::RAX as u32);
			} else {
				super::instruction_internal::internal_set_op1_register_u32(instruction, index + Register::AX as u32);
			}
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Gv_M_as {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Gv_M_as {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.address_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32,
			);
		} else if decoder.state.address_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32,
			);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::AX as u32,
			);
		}
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
pub(crate) struct OpCodeHandler_Gdq_Ev {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Gdq_Ev {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.operand_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32,
			);
		} else if decoder.state.operand_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32,
			);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
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
			let index = decoder.state.rm + decoder.state.extra_base_register_base;
			if decoder.state.operand_size == OpSize::Size32 {
				super::instruction_internal::internal_set_op1_register_u32(instruction, index + Register::EAX as u32);
			} else if decoder.state.operand_size == OpSize::Size64 {
				super::instruction_internal::internal_set_op1_register_u32(instruction, index + Register::RAX as u32);
			} else {
				super::instruction_internal::internal_set_op1_register_u32(instruction, index + Register::AX as u32);
			}
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Gv_Ev3 {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Gv_Ev3 {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.operand_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32,
			);
		} else if decoder.state.operand_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32,
			);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::AX as u32,
			);
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			let index = decoder.state.rm + decoder.state.extra_base_register_base;
			if decoder.state.operand_size == OpSize::Size32 {
				super::instruction_internal::internal_set_op1_register_u32(instruction, index + Register::EAX as u32);
			} else if decoder.state.operand_size == OpSize::Size64 {
				super::instruction_internal::internal_set_op1_register_u32(instruction, index + Register::RAX as u32);
			} else {
				super::instruction_internal::internal_set_op1_register_u32(instruction, index + Register::AX as u32);
			}
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Gv_Ev2 {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Gv_Ev2 {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.operand_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32,
			);
		} else if decoder.state.operand_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32,
			);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::AX as u32,
			);
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			let index = decoder.state.rm + decoder.state.extra_base_register_base;
			if decoder.state.operand_size != OpSize::Size16 {
				super::instruction_internal::internal_set_op1_register_u32(instruction, index + Register::EAX as u32);
			} else {
				super::instruction_internal::internal_set_op1_register_u32(instruction, index + Register::AX as u32);
			}
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_R_C {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
	pub(crate) base_reg: Register,
}

impl OpCodeHandler_R_C {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.is64_mode {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + Register::RAX as u32,
			);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + Register::EAX as u32,
			);
		}
		// LOCK MOV CR0 is supported by some AMD CPUs
		if this.base_reg == Register::CR0
			&& decoder.state.reg == 0
			&& decoder.state.extra_register_base == 0
			&& instruction.has_lock_prefix()
			&& (decoder.options & DecoderOptions::NO_LOCK_MOV_CR0) == 0
		{
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(instruction, Register::CR8 as u32);
			super::instruction_internal::internal_clear_has_lock_prefix(instruction);
			decoder.state.flags &= !StateFlags::LOCK;
		} else {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32,
			);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_C_R {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
	pub(crate) base_reg: Register,
}

impl OpCodeHandler_C_R {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.is64_mode {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + Register::RAX as u32,
			);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + Register::EAX as u32,
			);
		}
		// LOCK MOV CR0 is supported by some AMD CPUs
		if this.base_reg == Register::CR0
			&& decoder.state.reg == 0
			&& decoder.state.extra_register_base == 0
			&& instruction.has_lock_prefix()
			&& (decoder.options & DecoderOptions::NO_LOCK_MOV_CR0) == 0
		{
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register(instruction, Register::CR8);
			super::instruction_internal::internal_clear_has_lock_prefix(instruction);
			decoder.state.flags &= !StateFlags::LOCK;
		} else {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32,
			);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Jb {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Jb {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		decoder.state.flags |= StateFlags::BRANCH_IMM8;
		if decoder.is64_mode {
			if (decoder.options & DecoderOptions::AMD_BRANCHES) == 0 || decoder.state.operand_size == OpSize::Size32 {
				super::instruction_internal::internal_set_code_u32(instruction, this.code64);
				super::instruction_internal::internal_set_op0_kind(instruction, OpKind::NearBranch64);
				instruction.set_near_branch64((decoder.read_u8() as i8 as u64).wrapping_add(decoder.current_ip64()));
			} else {
				super::instruction_internal::internal_set_code_u32(instruction, this.code16);
				super::instruction_internal::internal_set_op0_kind(instruction, OpKind::NearBranch16);
				super::instruction_internal::internal_set_near_branch16(
					instruction,
					(decoder.read_u8() as i8 as u32).wrapping_add(decoder.current_ip32()),
				);
			}
		} else {
			if decoder.state.operand_size != OpSize::Size16 {
				super::instruction_internal::internal_set_code_u32(instruction, this.code32);
				super::instruction_internal::internal_set_op0_kind(instruction, OpKind::NearBranch32);
				instruction.set_near_branch32((decoder.read_u8() as i8 as u32).wrapping_add(decoder.current_ip32()));
			} else {
				super::instruction_internal::internal_set_code_u32(instruction, this.code16);
				super::instruction_internal::internal_set_op0_kind(instruction, OpKind::NearBranch16);
				super::instruction_internal::internal_set_near_branch16(
					instruction,
					(decoder.read_u8() as i8 as u32).wrapping_add(decoder.current_ip32()),
				);
			}
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Jx {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Jx {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		decoder.state.flags |= StateFlags::XBEGIN;
		if decoder.is64_mode {
			if decoder.state.operand_size == OpSize::Size32 {
				super::instruction_internal::internal_set_code_u32(instruction, this.code32);
				super::instruction_internal::internal_set_op0_kind(instruction, OpKind::NearBranch64);
				instruction.set_near_branch64((decoder.read_u32() as i32 as u64).wrapping_add(decoder.current_ip64()));
			} else if decoder.state.operand_size == OpSize::Size64 {
				super::instruction_internal::internal_set_code_u32(instruction, this.code64);
				super::instruction_internal::internal_set_op0_kind(instruction, OpKind::NearBranch64);
				instruction.set_near_branch64((decoder.read_u32() as i32 as u64).wrapping_add(decoder.current_ip64()));
			} else {
				super::instruction_internal::internal_set_code_u32(instruction, this.code16);
				super::instruction_internal::internal_set_op0_kind(instruction, OpKind::NearBranch64);
				instruction.set_near_branch64((decoder.read_u16() as i16 as u64).wrapping_add(decoder.current_ip64()));
			}
		} else if decoder.default_code_size == CodeSize::Code32 {
			if decoder.state.operand_size == OpSize::Size32 {
				super::instruction_internal::internal_set_code_u32(instruction, this.code32);
				super::instruction_internal::internal_set_op0_kind(instruction, OpKind::NearBranch32);
				instruction.set_near_branch32((decoder.read_u32() as u32).wrapping_add(decoder.current_ip32()));
			} else {
				debug_assert!(decoder.state.operand_size == OpSize::Size16);
				super::instruction_internal::internal_set_code_u32(instruction, this.code16);
				super::instruction_internal::internal_set_op0_kind(instruction, OpKind::NearBranch32);
				instruction.set_near_branch32((decoder.read_u16() as i16 as u32).wrapping_add(decoder.current_ip32()));
			}
		} else {
			debug_assert!(decoder.default_code_size == CodeSize::Code16);
			if decoder.state.operand_size == OpSize::Size16 {
				super::instruction_internal::internal_set_code_u32(instruction, this.code16);
				super::instruction_internal::internal_set_op0_kind(instruction, OpKind::NearBranch16);
				super::instruction_internal::internal_set_near_branch16(
					instruction,
					(decoder.read_u16() as u32).wrapping_add(decoder.current_ip32()) as u16 as u32,
				);
			} else {
				debug_assert!(decoder.state.operand_size == OpSize::Size32);
				super::instruction_internal::internal_set_code_u32(instruction, this.code32);
				super::instruction_internal::internal_set_op0_kind(instruction, OpKind::NearBranch16);
				super::instruction_internal::internal_set_near_branch16(
					instruction,
					(decoder.read_u32() as u32).wrapping_add(decoder.current_ip32()) as u16 as u32,
				);
			}
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Jz {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Jz {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.is64_mode {
			if (decoder.options & DecoderOptions::AMD_BRANCHES) == 0 || decoder.state.operand_size == OpSize::Size32 {
				super::instruction_internal::internal_set_code_u32(instruction, this.code64);
				super::instruction_internal::internal_set_op0_kind(instruction, OpKind::NearBranch64);
				instruction.set_near_branch64((decoder.read_u32() as i32 as u64).wrapping_add(decoder.current_ip64()));
			} else {
				super::instruction_internal::internal_set_code_u32(instruction, this.code16);
				super::instruction_internal::internal_set_op0_kind(instruction, OpKind::NearBranch16);
				super::instruction_internal::internal_set_near_branch16(
					instruction,
					(decoder.read_u16() as u32).wrapping_add(decoder.current_ip32()),
				);
			}
		} else {
			if decoder.state.operand_size != OpSize::Size16 {
				super::instruction_internal::internal_set_code_u32(instruction, this.code32);
				super::instruction_internal::internal_set_op0_kind(instruction, OpKind::NearBranch32);
				instruction.set_near_branch32((decoder.read_u32() as u32).wrapping_add(decoder.current_ip32()));
			} else {
				super::instruction_internal::internal_set_code_u32(instruction, this.code16);
				super::instruction_internal::internal_set_op0_kind(instruction, OpKind::NearBranch16);
				super::instruction_internal::internal_set_near_branch16(
					instruction,
					(decoder.read_u16() as u32).wrapping_add(decoder.current_ip32()),
				);
			}
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Jb2 {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16_16: u32,
	pub(crate) code16_32: u32,
	pub(crate) code16_64: u32,
	pub(crate) code32_16: u32,
	pub(crate) code32_32: u32,
	pub(crate) code64_32: u32,
	pub(crate) code64_64: u32,
}

impl OpCodeHandler_Jb2 {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		decoder.state.flags |= StateFlags::BRANCH_IMM8;
		if decoder.is64_mode {
			if (decoder.options & DecoderOptions::AMD_BRANCHES) == 0 || decoder.state.operand_size == OpSize::Size32 {
				if decoder.state.address_size == OpSize::Size64 {
					super::instruction_internal::internal_set_code_u32(instruction, this.code64_64);
				} else {
					super::instruction_internal::internal_set_code_u32(instruction, this.code64_32);
				}
				super::instruction_internal::internal_set_op0_kind(instruction, OpKind::NearBranch64);
				instruction.set_near_branch64((decoder.read_u8() as i8 as u64).wrapping_add(decoder.current_ip64()));
			} else {
				if decoder.state.address_size == OpSize::Size64 {
					super::instruction_internal::internal_set_code_u32(instruction, this.code16_64);
				} else {
					super::instruction_internal::internal_set_code_u32(instruction, this.code16_32);
				}
				super::instruction_internal::internal_set_op0_kind(instruction, OpKind::NearBranch16);
				super::instruction_internal::internal_set_near_branch16(
					instruction,
					(decoder.read_u8() as i8 as u32).wrapping_add(decoder.current_ip32()),
				);
			}
		} else {
			if decoder.state.operand_size == OpSize::Size32 {
				if decoder.state.address_size == OpSize::Size32 {
					super::instruction_internal::internal_set_code_u32(instruction, this.code32_32);
				} else {
					super::instruction_internal::internal_set_code_u32(instruction, this.code32_16);
				}
				super::instruction_internal::internal_set_op0_kind(instruction, OpKind::NearBranch32);
				instruction.set_near_branch32((decoder.read_u8() as i8 as u32).wrapping_add(decoder.current_ip32()));
			} else {
				if decoder.state.address_size == OpSize::Size32 {
					super::instruction_internal::internal_set_code_u32(instruction, this.code16_32);
				} else {
					super::instruction_internal::internal_set_code_u32(instruction, this.code16_16);
				}
				super::instruction_internal::internal_set_op0_kind(instruction, OpKind::NearBranch16);
				super::instruction_internal::internal_set_near_branch16(
					instruction,
					(decoder.read_u8() as i8 as u32).wrapping_add(decoder.current_ip32()),
				);
			}
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Jdisp {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
}

impl OpCodeHandler_Jdisp {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		debug_assert!(!decoder.is64_mode);
		if decoder.state.operand_size != OpSize::Size16 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::NearBranch32);
			instruction.set_near_branch32(decoder.read_u32() as u32);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::NearBranch16);
			super::instruction_internal::internal_set_near_branch16(instruction, decoder.read_u16() as u32);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_PushOpSizeReg {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
	pub(crate) reg: Register,
}

impl OpCodeHandler_PushOpSizeReg {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.is64_mode {
			if decoder.state.operand_size != OpSize::Size16 {
				super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			} else {
				super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			}
		} else {
			if decoder.state.operand_size == OpSize::Size32 {
				super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			} else {
				super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			}
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register(instruction, this.reg);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_PushEv {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_PushEv {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.is64_mode {
			if decoder.state.operand_size != OpSize::Size16 {
				super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			} else {
				super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			}
		} else {
			if decoder.state.operand_size == OpSize::Size32 {
				super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			} else {
				super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			}
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			let index = decoder.state.rm + decoder.state.extra_base_register_base;
			if decoder.is64_mode {
				if decoder.state.operand_size != OpSize::Size16 {
					super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::RAX as u32);
				} else {
					super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::AX as u32);
				}
			} else {
				if decoder.state.operand_size == OpSize::Size32 {
					super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::EAX as u32);
				} else {
					super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::AX as u32);
				}
			}
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Ev_Gv {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) flags: u32,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Ev_Gv {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			let index = decoder.state.rm + decoder.state.extra_base_register_base;
			if decoder.state.operand_size == OpSize::Size32 {
				super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::EAX as u32);
			} else if decoder.state.operand_size == OpSize::Size64 {
				super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::RAX as u32);
			} else {
				super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::AX as u32);
			}
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
			if (this.flags & (HandlerFlags::XACQUIRE | HandlerFlags::XRELEASE)) != 0 {
				decoder.set_xacquire_xrelease(instruction, this.flags);
			}
			const_assert_eq!(8, HandlerFlags::LOCK);
			const_assert_eq!(0x0000_2000, StateFlags::ALLOW_LOCK);
			decoder.state.flags |= (this.flags & HandlerFlags::LOCK) << (13 - 3);
		}
		if decoder.state.operand_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32,
			);
		} else if decoder.state.operand_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32,
			);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::AX as u32,
			);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Ev_Gv_32_64 {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Ev_Gv_32_64 {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		let base_reg;
		if decoder.is64_mode {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			base_reg = Register::RAX;
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			base_reg = Register::EAX;
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + base_reg as u32,
			);
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + base_reg as u32,
		);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Ev_Gv_Ib {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Ev_Gv_Ib {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			let index = decoder.state.rm + decoder.state.extra_base_register_base;
			if decoder.state.operand_size == OpSize::Size32 {
				super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::EAX as u32);
			} else if decoder.state.operand_size == OpSize::Size64 {
				super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::RAX as u32);
			} else {
				super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::AX as u32);
			}
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		if decoder.state.operand_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32,
			);
		} else if decoder.state.operand_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32,
			);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::AX as u32,
			);
		}
		super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Ev_Gv_CL {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Ev_Gv_CL {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			let index = decoder.state.rm + decoder.state.extra_base_register_base;
			if decoder.state.operand_size == OpSize::Size32 {
				super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::EAX as u32);
			} else if decoder.state.operand_size == OpSize::Size64 {
				super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::RAX as u32);
			} else {
				super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::AX as u32);
			}
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		if decoder.state.operand_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32,
			);
		} else if decoder.state.operand_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32,
			);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::AX as u32,
			);
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op2_register(instruction, Register::CL);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Gv_Mp {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Gv_Mp {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.operand_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32,
			);
		} else if decoder.state.operand_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32,
			);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::AX as u32,
			);
		}
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
pub(crate) struct OpCodeHandler_Gv_Eb {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Gv_Eb {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.operand_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32,
			);
		} else if decoder.state.operand_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32,
			);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::AX as u32,
			);
		}
		if decoder.state.mod_ == 3 {
			let mut index = decoder.state.rm + decoder.state.extra_base_register_base;
			if (decoder.state.flags & StateFlags::HAS_REX) != 0 && index >= 4 {
				index += 4;
			}
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(instruction, index + Register::AL as u32);
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Gv_Ew {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Gv_Ew {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.operand_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32,
			);
		} else if decoder.state.operand_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32,
			);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::AX as u32,
			);
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + Register::AX as u32,
			);
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_PushSimple2 {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_PushSimple2 {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.is64_mode {
			if decoder.state.operand_size != OpSize::Size16 {
				super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			} else {
				super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			}
		} else {
			if decoder.state.operand_size == OpSize::Size32 {
				super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			} else {
				super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			}
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Simple2 {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Simple2 {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.operand_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
		} else if decoder.state.operand_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Simple2Iw {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Simple2Iw {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.operand_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
		} else if decoder.state.operand_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
		}
		super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Immediate16);
		super::instruction_internal::internal_set_immediate16(instruction, decoder.read_u16() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Simple3 {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Simple3 {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.is64_mode {
			if decoder.state.operand_size != OpSize::Size16 {
				super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			} else {
				super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			}
		} else {
			if decoder.state.operand_size == OpSize::Size32 {
				super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			} else {
				super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			}
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Simple5 {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Simple5 {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.address_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
		} else if decoder.state.address_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Simple5_ModRM_as {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Simple5_ModRM_as {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.address_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + Register::RAX as u32,
			);
		} else if decoder.state.address_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + Register::EAX as u32,
			);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + Register::AX as u32,
			);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Simple4 {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Simple4 {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if (decoder.state.flags & StateFlags::W) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_PushSimpleReg {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) index: u32,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_PushSimpleReg {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.is64_mode {
			if decoder.state.operand_size != OpSize::Size16 {
				super::instruction_internal::internal_set_code_u32(instruction, this.code64);
				const_assert_eq!(0, OpKind::Register as u32);
				//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
				super::instruction_internal::internal_set_op0_register_u32(
					instruction,
					this.index + decoder.state.extra_base_register_base + Register::RAX as u32,
				);
			} else {
				super::instruction_internal::internal_set_code_u32(instruction, this.code16);
				const_assert_eq!(0, OpKind::Register as u32);
				//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
				super::instruction_internal::internal_set_op0_register_u32(
					instruction,
					this.index + decoder.state.extra_base_register_base + Register::AX as u32,
				);
			}
		} else {
			if decoder.state.operand_size == OpSize::Size32 {
				super::instruction_internal::internal_set_code_u32(instruction, this.code32);
				const_assert_eq!(0, OpKind::Register as u32);
				//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
				super::instruction_internal::internal_set_op0_register_u32(
					instruction,
					this.index + decoder.state.extra_base_register_base + Register::EAX as u32,
				);
			} else {
				super::instruction_internal::internal_set_code_u32(instruction, this.code16);
				const_assert_eq!(0, OpKind::Register as u32);
				//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
				super::instruction_internal::internal_set_op0_register_u32(
					instruction,
					this.index + decoder.state.extra_base_register_base + Register::AX as u32,
				);
			}
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_SimpleReg {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) index: u32,
	pub(crate) code: u32,
}

impl OpCodeHandler_SimpleReg {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		const_assert_eq!(0, OpSize::Size16 as u32);
		const_assert_eq!(1, OpSize::Size32 as u32);
		const_assert_eq!(2, OpSize::Size64 as u32);
		let size_index = decoder.state.operand_size as u32;

		super::instruction_internal::internal_set_code_u32(instruction, size_index + this.code);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		const_assert!(Register::AX as u32 + 16 == Register::EAX as u32);
		const_assert!(Register::AX as u32 + 32 == Register::RAX as u32);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			size_index * 16 + this.index + decoder.state.extra_base_register_base + Register::AX as u32,
		);
	}
}

static XCHG_REG_RAX_CODES: [u16; 3 * 16] = [
	Code::Nopw as u16,
	Code::Xchg_r16_AX as u16,
	Code::Xchg_r16_AX as u16,
	Code::Xchg_r16_AX as u16,
	Code::Xchg_r16_AX as u16,
	Code::Xchg_r16_AX as u16,
	Code::Xchg_r16_AX as u16,
	Code::Xchg_r16_AX as u16,
	Code::Xchg_r16_AX as u16,
	Code::Xchg_r16_AX as u16,
	Code::Xchg_r16_AX as u16,
	Code::Xchg_r16_AX as u16,
	Code::Xchg_r16_AX as u16,
	Code::Xchg_r16_AX as u16,
	Code::Xchg_r16_AX as u16,
	Code::Xchg_r16_AX as u16,
	Code::Nopd as u16,
	Code::Xchg_r32_EAX as u16,
	Code::Xchg_r32_EAX as u16,
	Code::Xchg_r32_EAX as u16,
	Code::Xchg_r32_EAX as u16,
	Code::Xchg_r32_EAX as u16,
	Code::Xchg_r32_EAX as u16,
	Code::Xchg_r32_EAX as u16,
	Code::Xchg_r32_EAX as u16,
	Code::Xchg_r32_EAX as u16,
	Code::Xchg_r32_EAX as u16,
	Code::Xchg_r32_EAX as u16,
	Code::Xchg_r32_EAX as u16,
	Code::Xchg_r32_EAX as u16,
	Code::Xchg_r32_EAX as u16,
	Code::Xchg_r32_EAX as u16,
	Code::Nopq as u16,
	Code::Xchg_r64_RAX as u16,
	Code::Xchg_r64_RAX as u16,
	Code::Xchg_r64_RAX as u16,
	Code::Xchg_r64_RAX as u16,
	Code::Xchg_r64_RAX as u16,
	Code::Xchg_r64_RAX as u16,
	Code::Xchg_r64_RAX as u16,
	Code::Xchg_r64_RAX as u16,
	Code::Xchg_r64_RAX as u16,
	Code::Xchg_r64_RAX as u16,
	Code::Xchg_r64_RAX as u16,
	Code::Xchg_r64_RAX as u16,
	Code::Xchg_r64_RAX as u16,
	Code::Xchg_r64_RAX as u16,
	Code::Xchg_r64_RAX as u16,
];

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Xchg_Reg_rAX {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) index: u32,
}

impl OpCodeHandler_Xchg_Reg_rAX {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());

		if this.index == 0 && decoder.state.mandatory_prefix == MandatoryPrefixByte::PF3 as u32 && (decoder.options & DecoderOptions::NO_PAUSE) == 0 {
			decoder.clear_mandatory_prefix_f3(instruction);
			super::instruction_internal::internal_set_code(instruction, Code::Pause);
		} else {
			const_assert_eq!(0, OpSize::Size16 as u32);
			const_assert_eq!(1, OpSize::Size32 as u32);
			const_assert_eq!(2, OpSize::Size64 as u32);
			let size_index = decoder.state.operand_size as u32;
			let code_index = this.index + decoder.state.extra_base_register_base;

			// Safe, size_index is 0-2 (a valid OpSize value) and code_index is 0-15
			super::instruction_internal::internal_set_code_u32(instruction, unsafe {
				*XCHG_REG_RAX_CODES.get_unchecked((size_index * 16 + code_index) as usize) as u32
			});
			if code_index != 0 {
				const_assert!(Register::AX as u32 + 16 == Register::EAX as u32);
				const_assert!(Register::AX as u32 + 32 == Register::RAX as u32);
				let reg = size_index * 16 + code_index + Register::AX as u32;
				const_assert_eq!(0, OpKind::Register as u32);
				//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
				super::instruction_internal::internal_set_op0_register_u32(instruction, reg);
				const_assert_eq!(0, OpKind::Register as u32);
				//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
				super::instruction_internal::internal_set_op1_register_u32(instruction, size_index * 16 + Register::AX as u32);
			}
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Reg_Iz {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Reg_Iz {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.operand_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register(instruction, Register::EAX);
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Immediate32);
			instruction.set_immediate32(decoder.read_u32() as u32);
		} else if decoder.state.operand_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register(instruction, Register::RAX);
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Immediate32to64);
			instruction.set_immediate32(decoder.read_u32() as u32);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register(instruction, Register::AX);
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Immediate16);
			super::instruction_internal::internal_set_immediate16(instruction, decoder.read_u16() as u32);
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
pub(crate) struct OpCodeHandler_RegIb3 {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) index: u32,
}

impl OpCodeHandler_RegIb3 {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		let register;
		if (decoder.state.flags & StateFlags::HAS_REX) != 0 {
			// Safe, index = 0..7 and extra_base_register_base is 0 or 8
			register = unsafe { *WITH_REX_PREFIX_MOV_REGISTERS.get_unchecked((this.index + decoder.state.extra_base_register_base) as usize) };
		} else {
			// Safe, index = 0..7
			register = unsafe { std::mem::transmute((this.index + Register::AL as u32) as u8) };
		}
		super::instruction_internal::internal_set_code(instruction, Code::Mov_r8_imm8);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register(instruction, register);
		super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_RegIz2 {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) index: u32,
}

impl OpCodeHandler_RegIz2 {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.operand_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code(instruction, Code::Mov_r32_imm32);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				this.index + decoder.state.extra_base_register_base + Register::EAX as u32,
			);
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Immediate32);
			instruction.set_immediate32(decoder.read_u32() as u32);
		} else if decoder.state.operand_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code(instruction, Code::Mov_r64_imm64);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				this.index + decoder.state.extra_base_register_base + Register::RAX as u32,
			);
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Immediate64);
			super::instruction_internal::internal_set_immediate64_lo(instruction, decoder.read_u32() as u32);
			super::instruction_internal::internal_set_immediate64_hi(instruction, decoder.read_u32() as u32);
		} else {
			super::instruction_internal::internal_set_code(instruction, Code::Mov_r16_imm16);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				this.index + decoder.state.extra_base_register_base + Register::AX as u32,
			);
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Immediate16);
			super::instruction_internal::internal_set_immediate16(instruction, decoder.read_u16() as u32);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_PushIb2 {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_PushIb2 {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.is64_mode {
			if decoder.state.operand_size != OpSize::Size16 {
				super::instruction_internal::internal_set_code_u32(instruction, this.code64);
				super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Immediate8to64);
				super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
			} else {
				super::instruction_internal::internal_set_code_u32(instruction, this.code16);
				super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Immediate8to16);
				super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
			}
		} else {
			if decoder.state.operand_size == OpSize::Size32 {
				super::instruction_internal::internal_set_code_u32(instruction, this.code32);
				super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Immediate8to32);
				super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
			} else {
				super::instruction_internal::internal_set_code_u32(instruction, this.code16);
				super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Immediate8to16);
				super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
			}
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_PushIz {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_PushIz {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.is64_mode {
			if decoder.state.operand_size != OpSize::Size16 {
				super::instruction_internal::internal_set_code_u32(instruction, this.code64);
				super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Immediate32to64);
				instruction.set_immediate32(decoder.read_u32() as u32);
			} else {
				super::instruction_internal::internal_set_code_u32(instruction, this.code16);
				super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Immediate16);
				super::instruction_internal::internal_set_immediate16(instruction, decoder.read_u16() as u32);
			}
		} else {
			if decoder.state.operand_size == OpSize::Size32 {
				super::instruction_internal::internal_set_code_u32(instruction, this.code32);
				super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Immediate32);
				instruction.set_immediate32(decoder.read_u32() as u32);
			} else {
				super::instruction_internal::internal_set_code_u32(instruction, this.code16);
				super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Immediate16);
				super::instruction_internal::internal_set_immediate16(instruction, decoder.read_u16() as u32);
			}
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Gv_Ma {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
}

impl OpCodeHandler_Gv_Ma {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.operand_size != OpSize::Size16 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32,
			);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::AX as u32,
			);
		}
		debug_assert_ne!(3, decoder.state.mod_);
		super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
		decoder.read_op_mem(instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_RvMw_Gw {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
}

impl OpCodeHandler_RvMw_Gw {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		let base_reg;
		if decoder.state.operand_size != OpSize::Size16 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32,
			);
			base_reg = Register::EAX;
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::AX as u32,
			);
			base_reg = Register::AX;
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + base_reg as u32,
			);
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Gv_Ev_Ib {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Gv_Ev_Ib {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			let index = decoder.state.rm + decoder.state.extra_base_register_base;
			if decoder.state.operand_size == OpSize::Size32 {
				super::instruction_internal::internal_set_op1_register_u32(instruction, index + Register::EAX as u32);
			} else if decoder.state.operand_size == OpSize::Size64 {
				super::instruction_internal::internal_set_op1_register_u32(instruction, index + Register::RAX as u32);
			} else {
				super::instruction_internal::internal_set_op1_register_u32(instruction, index + Register::AX as u32);
			}
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		if decoder.state.operand_size == OpSize::Size32 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32,
			);
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Immediate8to32);
			super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
		} else if decoder.state.operand_size == OpSize::Size64 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32,
			);
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Immediate8to64);
			super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
		} else {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::AX as u32,
			);
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Immediate8to16);
			super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Gv_Ev_Ib_REX {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
	pub(crate) base_reg: Register,
}

impl OpCodeHandler_Gv_Ev_Ib_REX {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if (decoder.state.flags & StateFlags::W) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32,
			);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32,
			);
		}
		debug_assert_eq!(3, decoder.state.mod_);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(
			instruction,
			decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32,
		);
		super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Gv_Ev_32_64 {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) disallow_reg: u32,
	pub(crate) disallow_mem: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Gv_Ev_32_64 {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		let base_reg;
		if decoder.is64_mode {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			base_reg = Register::RAX;
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			base_reg = Register::EAX;
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + base_reg as u32,
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + base_reg as u32,
			);
			if (this.disallow_reg & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
			if (this.disallow_mem & decoder.invalid_check_mask) != 0 {
				decoder.set_invalid_instruction();
			}
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Gv_Ev_Iz {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Gv_Ev_Iz {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			let index = decoder.state.rm + decoder.state.extra_base_register_base;
			if decoder.state.operand_size == OpSize::Size32 {
				super::instruction_internal::internal_set_op1_register_u32(instruction, index + Register::EAX as u32);
			} else if decoder.state.operand_size == OpSize::Size64 {
				super::instruction_internal::internal_set_op1_register_u32(instruction, index + Register::RAX as u32);
			} else {
				super::instruction_internal::internal_set_op1_register_u32(instruction, index + Register::AX as u32);
			}
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		if decoder.state.operand_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32,
			);
			super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Immediate32);
			instruction.set_immediate32(decoder.read_u32() as u32);
		} else if decoder.state.operand_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32,
			);
			super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Immediate32to64);
			instruction.set_immediate32(decoder.read_u32() as u32);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::AX as u32,
			);
			super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Immediate16);
			super::instruction_internal::internal_set_immediate16(instruction, decoder.read_u16() as u32);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Yb_Reg {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) reg: Register,
}

impl OpCodeHandler_Yb_Reg {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		if decoder.state.address_size == OpSize::Size64 {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::MemoryESRDI);
		} else if decoder.state.address_size == OpSize::Size32 {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::MemoryESEDI);
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::MemoryESDI);
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register(instruction, this.reg);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Yv_Reg {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Yv_Reg {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.address_size == OpSize::Size64 {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::MemoryESRDI);
		} else if decoder.state.address_size == OpSize::Size32 {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::MemoryESEDI);
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::MemoryESDI);
		}
		if decoder.state.operand_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register(instruction, Register::EAX);
		} else if decoder.state.operand_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register(instruction, Register::RAX);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register(instruction, Register::AX);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Yv_Reg2 {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
}

impl OpCodeHandler_Yv_Reg2 {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.address_size == OpSize::Size64 {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::MemoryESRDI);
		} else if decoder.state.address_size == OpSize::Size32 {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::MemoryESEDI);
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::MemoryESDI);
		}
		if decoder.state.operand_size != OpSize::Size16 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register(instruction, Register::DX);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register(instruction, Register::DX);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Reg_Xb {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) reg: Register,
}

impl OpCodeHandler_Reg_Xb {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register(instruction, this.reg);
		if decoder.state.address_size == OpSize::Size64 {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::MemorySegRSI);
		} else if decoder.state.address_size == OpSize::Size32 {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::MemorySegESI);
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::MemorySegSI);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Reg_Xv {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Reg_Xv {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.address_size == OpSize::Size64 {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::MemorySegRSI);
		} else if decoder.state.address_size == OpSize::Size32 {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::MemorySegESI);
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::MemorySegSI);
		}
		if decoder.state.operand_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register(instruction, Register::EAX);
		} else if decoder.state.operand_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register(instruction, Register::RAX);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register(instruction, Register::AX);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Reg_Xv2 {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
}

impl OpCodeHandler_Reg_Xv2 {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.address_size == OpSize::Size64 {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::MemorySegRSI);
		} else if decoder.state.address_size == OpSize::Size32 {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::MemorySegESI);
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::MemorySegSI);
		}
		if decoder.state.operand_size != OpSize::Size16 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register(instruction, Register::DX);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register(instruction, Register::DX);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Reg_Yb {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) reg: Register,
}

impl OpCodeHandler_Reg_Yb {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register(instruction, this.reg);
		if decoder.state.address_size == OpSize::Size64 {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::MemoryESRDI);
		} else if decoder.state.address_size == OpSize::Size32 {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::MemoryESEDI);
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::MemoryESDI);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Reg_Yv {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Reg_Yv {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.address_size == OpSize::Size64 {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::MemoryESRDI);
		} else if decoder.state.address_size == OpSize::Size32 {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::MemoryESEDI);
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::MemoryESDI);
		}
		if decoder.state.operand_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register(instruction, Register::EAX);
		} else if decoder.state.operand_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register(instruction, Register::RAX);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register(instruction, Register::AX);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Yb_Xb {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
}

impl OpCodeHandler_Yb_Xb {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		if decoder.state.address_size == OpSize::Size64 {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::MemoryESRDI);
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::MemorySegRSI);
		} else if decoder.state.address_size == OpSize::Size32 {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::MemoryESEDI);
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::MemorySegESI);
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::MemoryESDI);
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::MemorySegSI);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Yv_Xv {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Yv_Xv {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.address_size == OpSize::Size64 {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::MemoryESRDI);
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::MemorySegRSI);
		} else if decoder.state.address_size == OpSize::Size32 {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::MemoryESEDI);
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::MemorySegESI);
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::MemoryESDI);
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::MemorySegSI);
		}
		if decoder.state.operand_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
		} else if decoder.state.operand_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Xb_Yb {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
}

impl OpCodeHandler_Xb_Yb {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		if decoder.state.address_size == OpSize::Size64 {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::MemorySegRSI);
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::MemoryESRDI);
		} else if decoder.state.address_size == OpSize::Size32 {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::MemorySegESI);
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::MemoryESEDI);
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::MemorySegSI);
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::MemoryESDI);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Xv_Yv {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Xv_Yv {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.address_size == OpSize::Size64 {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::MemorySegRSI);
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::MemoryESRDI);
		} else if decoder.state.address_size == OpSize::Size32 {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::MemorySegESI);
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::MemoryESEDI);
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::MemorySegSI);
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::MemoryESDI);
		}
		if decoder.state.operand_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
		} else if decoder.state.operand_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Ev_Sw {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Ev_Sw {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.operand_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
		} else if decoder.state.operand_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			let index = decoder.state.rm + decoder.state.extra_base_register_base;
			if decoder.state.operand_size == OpSize::Size32 {
				super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::EAX as u32);
			} else if decoder.state.operand_size == OpSize::Size64 {
				super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::RAX as u32);
			} else {
				super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::AX as u32);
			}
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.read_op_seg_reg());
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Gv_M {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Gv_M {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.operand_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32,
			);
		} else if decoder.state.operand_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32,
			);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::AX as u32,
			);
		}
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
pub(crate) struct OpCodeHandler_Sw_Ev {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Sw_Ev {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.operand_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
		} else if decoder.state.operand_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		let sreg = decoder.read_op_seg_reg();
		if decoder.invalid_check_mask != 0 && sreg == Register::CS as u32 {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_op0_register_u32(instruction, sreg);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			let index = decoder.state.rm + decoder.state.extra_base_register_base;
			if decoder.state.operand_size == OpSize::Size32 {
				super::instruction_internal::internal_set_op1_register_u32(instruction, index + Register::EAX as u32);
			} else if decoder.state.operand_size == OpSize::Size64 {
				super::instruction_internal::internal_set_op1_register_u32(instruction, index + Register::RAX as u32);
			} else {
				super::instruction_internal::internal_set_op1_register_u32(instruction, index + Register::AX as u32);
			}
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Ap {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
}

impl OpCodeHandler_Ap {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.operand_size != OpSize::Size16 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
		}
		if decoder.state.operand_size != OpSize::Size16 {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::FarBranch32);
			instruction.set_far_branch32(decoder.read_u32() as u32);
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::FarBranch16);
			super::instruction_internal::internal_set_far_branch16(instruction, decoder.read_u16() as u32);
		}
		super::instruction_internal::internal_set_far_branch_selector(instruction, decoder.read_u16() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Reg_Ob {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) reg: Register,
}

impl OpCodeHandler_Reg_Ob {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register(instruction, this.reg);
		decoder.displ_index = decoder.data_ptr as usize;
		if decoder.state.address_size == OpSize::Size64 {
			super::instruction_internal::internal_set_memory_displ_size(instruction, 4);
			decoder.state.flags |= StateFlags::ADDR64;
			super::instruction_internal::internal_set_memory_address64_lo(instruction, decoder.read_u32() as u32);
			super::instruction_internal::internal_set_memory_address64_hi(instruction, decoder.read_u32() as u32);
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory64);
		} else if decoder.state.address_size == OpSize::Size32 {
			super::instruction_internal::internal_set_memory_displ_size(instruction, 3);
			instruction.set_memory_displacement(decoder.read_u32() as u32);
			//super::instruction_internal::internal_set_memory_index_scale(instruction, 0);
			//super::instruction_internal::internal_set_memory_base(instruction, Register::None);
			//super::instruction_internal::internal_set_memory_index(instruction, Register::None);
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
		} else {
			super::instruction_internal::internal_set_memory_displ_size(instruction, 2);
			instruction.set_memory_displacement(decoder.read_u16() as u32);
			//super::instruction_internal::internal_set_memory_index_scale(instruction, 0);
			//super::instruction_internal::internal_set_memory_base(instruction, Register::None);
			//super::instruction_internal::internal_set_memory_index(instruction, Register::None);
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Ob_Reg {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) reg: Register,
}

impl OpCodeHandler_Ob_Reg {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		decoder.displ_index = decoder.data_ptr as usize;
		if decoder.state.address_size == OpSize::Size64 {
			super::instruction_internal::internal_set_memory_displ_size(instruction, 4);
			decoder.state.flags |= StateFlags::ADDR64;
			super::instruction_internal::internal_set_memory_address64_lo(instruction, decoder.read_u32() as u32);
			super::instruction_internal::internal_set_memory_address64_hi(instruction, decoder.read_u32() as u32);
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory64);
		} else if decoder.state.address_size == OpSize::Size32 {
			super::instruction_internal::internal_set_memory_displ_size(instruction, 3);
			instruction.set_memory_displacement(decoder.read_u32() as u32);
			//super::instruction_internal::internal_set_memory_index_scale(instruction, 0);
			//super::instruction_internal::internal_set_memory_base(instruction, Register::None);
			//super::instruction_internal::internal_set_memory_index(instruction, Register::None);
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
		} else {
			super::instruction_internal::internal_set_memory_displ_size(instruction, 2);
			instruction.set_memory_displacement(decoder.read_u16() as u32);
			//super::instruction_internal::internal_set_memory_index_scale(instruction, 0);
			//super::instruction_internal::internal_set_memory_base(instruction, Register::None);
			//super::instruction_internal::internal_set_memory_index(instruction, Register::None);
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register(instruction, this.reg);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Reg_Ov {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Reg_Ov {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		decoder.displ_index = decoder.data_ptr as usize;
		if decoder.state.address_size == OpSize::Size64 {
			super::instruction_internal::internal_set_memory_displ_size(instruction, 4);
			decoder.state.flags |= StateFlags::ADDR64;
			super::instruction_internal::internal_set_memory_address64_lo(instruction, decoder.read_u32() as u32);
			super::instruction_internal::internal_set_memory_address64_hi(instruction, decoder.read_u32() as u32);
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory64);
		} else if decoder.state.address_size == OpSize::Size32 {
			super::instruction_internal::internal_set_memory_displ_size(instruction, 3);
			instruction.set_memory_displacement(decoder.read_u32() as u32);
			//super::instruction_internal::internal_set_memory_index_scale(instruction, 0);
			//super::instruction_internal::internal_set_memory_base(instruction, Register::None);
			//super::instruction_internal::internal_set_memory_index(instruction, Register::None);
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
		} else {
			super::instruction_internal::internal_set_memory_displ_size(instruction, 2);
			instruction.set_memory_displacement(decoder.read_u16() as u32);
			//super::instruction_internal::internal_set_memory_index_scale(instruction, 0);
			//super::instruction_internal::internal_set_memory_base(instruction, Register::None);
			//super::instruction_internal::internal_set_memory_index(instruction, Register::None);
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
		}
		if decoder.state.operand_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register(instruction, Register::EAX);
		} else if decoder.state.operand_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register(instruction, Register::RAX);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register(instruction, Register::AX);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Ov_Reg {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Ov_Reg {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		decoder.displ_index = decoder.data_ptr as usize;
		if decoder.state.address_size == OpSize::Size64 {
			super::instruction_internal::internal_set_memory_displ_size(instruction, 4);
			decoder.state.flags |= StateFlags::ADDR64;
			super::instruction_internal::internal_set_memory_address64_lo(instruction, decoder.read_u32() as u32);
			super::instruction_internal::internal_set_memory_address64_hi(instruction, decoder.read_u32() as u32);
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory64);
		} else if decoder.state.address_size == OpSize::Size32 {
			super::instruction_internal::internal_set_memory_displ_size(instruction, 3);
			instruction.set_memory_displacement(decoder.read_u32() as u32);
			//super::instruction_internal::internal_set_memory_index_scale(instruction, 0);
			//super::instruction_internal::internal_set_memory_base(instruction, Register::None);
			//super::instruction_internal::internal_set_memory_index(instruction, Register::None);
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
		} else {
			super::instruction_internal::internal_set_memory_displ_size(instruction, 2);
			instruction.set_memory_displacement(decoder.read_u16() as u32);
			//super::instruction_internal::internal_set_memory_index_scale(instruction, 0);
			//super::instruction_internal::internal_set_memory_base(instruction, Register::None);
			//super::instruction_internal::internal_set_memory_index(instruction, Register::None);
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
		}
		if decoder.state.operand_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register(instruction, Register::EAX);
		} else if decoder.state.operand_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register(instruction, Register::RAX);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register(instruction, Register::AX);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_BranchIw {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_BranchIw {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.is64_mode {
			if (decoder.options & DecoderOptions::AMD_BRANCHES) == 0 || decoder.state.operand_size == OpSize::Size32 {
				super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			} else {
				super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			}
		} else {
			if decoder.state.operand_size == OpSize::Size32 {
				super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			} else {
				super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			}
		}
		super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Immediate16);
		super::instruction_internal::internal_set_immediate16(instruction, decoder.read_u16() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_BranchSimple {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_BranchSimple {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.is64_mode {
			if (decoder.options & DecoderOptions::AMD_BRANCHES) == 0 || decoder.state.operand_size == OpSize::Size32 {
				super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			} else {
				super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			}
		} else {
			if decoder.state.operand_size == OpSize::Size32 {
				super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			} else {
				super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			}
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Iw_Ib {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Iw_Ib {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.is64_mode {
			if decoder.state.operand_size != OpSize::Size16 {
				super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			} else {
				super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			}
		} else {
			if decoder.state.operand_size == OpSize::Size32 {
				super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			} else {
				super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			}
		}
		super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Immediate16);
		super::instruction_internal::internal_set_immediate16(instruction, decoder.read_u16() as u32);
		super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Immediate8_2nd);
		super::instruction_internal::internal_set_immediate8_2nd(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Reg_Ib2 {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
}

impl OpCodeHandler_Reg_Ib2 {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		if decoder.state.operand_size != OpSize::Size16 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			super::instruction_internal::internal_set_op0_register(instruction, Register::EAX);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			super::instruction_internal::internal_set_op0_register(instruction, Register::AX);
		}
		super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_IbReg2 {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
}

impl OpCodeHandler_IbReg2 {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		if decoder.state.operand_size != OpSize::Size16 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			super::instruction_internal::internal_set_op1_register(instruction, Register::EAX);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			super::instruction_internal::internal_set_op1_register(instruction, Register::AX);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_eAX_DX {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
}

impl OpCodeHandler_eAX_DX {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.operand_size != OpSize::Size16 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register(instruction, Register::EAX);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register(instruction, Register::AX);
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register(instruction, Register::DX);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_DX_eAX {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
}

impl OpCodeHandler_DX_eAX {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register(instruction, Register::DX);
		if decoder.state.operand_size != OpSize::Size16 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register(instruction, Register::EAX);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register(instruction, Register::AX);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Eb_Ib {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) flags: u32,
	pub(crate) code: u32,
}

impl OpCodeHandler_Eb_Ib {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		if decoder.state.mod_ == 3 {
			let mut index = decoder.state.rm + decoder.state.extra_base_register_base;
			if (decoder.state.flags & StateFlags::HAS_REX) != 0 && index >= 4 {
				index += 4;
			}
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::AL as u32);
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
			if (this.flags & (HandlerFlags::XACQUIRE | HandlerFlags::XRELEASE)) != 0 {
				decoder.set_xacquire_xrelease(instruction, this.flags);
			}
			const_assert_eq!(8, HandlerFlags::LOCK);
			const_assert_eq!(0x0000_2000, StateFlags::ALLOW_LOCK);
			decoder.state.flags |= (this.flags & HandlerFlags::LOCK) << (13 - 3);
		}
		super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Eb_1 {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
}

impl OpCodeHandler_Eb_1 {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		if decoder.state.mod_ == 3 {
			let mut index = decoder.state.rm + decoder.state.extra_base_register_base;
			if (decoder.state.flags & StateFlags::HAS_REX) != 0 && index >= 4 {
				index += 4;
			}
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::AL as u32);
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, 1);
		decoder.state.flags |= StateFlags::NO_IMM;
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Eb_CL {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
}

impl OpCodeHandler_Eb_CL {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		if decoder.state.mod_ == 3 {
			let mut index = decoder.state.rm + decoder.state.extra_base_register_base;
			if (decoder.state.flags & StateFlags::HAS_REX) != 0 && index >= 4 {
				index += 4;
			}
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::AL as u32);
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, Register::CL as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Eb {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) flags: u32,
	pub(crate) code: u32,
}

impl OpCodeHandler_Eb {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		if decoder.state.mod_ == 3 {
			let mut index = decoder.state.rm + decoder.state.extra_base_register_base;
			if (decoder.state.flags & StateFlags::HAS_REX) != 0 && index >= 4 {
				index += 4;
			}
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::AL as u32);
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
			if (this.flags & (HandlerFlags::XACQUIRE | HandlerFlags::XRELEASE)) != 0 {
				decoder.set_xacquire_xrelease(instruction, this.flags);
			}
			const_assert_eq!(8, HandlerFlags::LOCK);
			const_assert_eq!(0x0000_2000, StateFlags::ALLOW_LOCK);
			decoder.state.flags |= (this.flags & HandlerFlags::LOCK) << (13 - 3);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Eb_Gb {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) flags: u32,
	pub(crate) code: u32,
}

impl OpCodeHandler_Eb_Gb {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		let mut index;
		if decoder.state.mod_ == 3 {
			index = decoder.state.rm + decoder.state.extra_base_register_base;
			if (decoder.state.flags & StateFlags::HAS_REX) != 0 && index >= 4 {
				index += 4;
			}
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::AL as u32);
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
			if (this.flags & (HandlerFlags::XACQUIRE | HandlerFlags::XRELEASE)) != 0 {
				decoder.set_xacquire_xrelease(instruction, this.flags);
			}
			const_assert_eq!(8, HandlerFlags::LOCK);
			const_assert_eq!(0x0000_2000, StateFlags::ALLOW_LOCK);
			decoder.state.flags |= (this.flags & HandlerFlags::LOCK) << (13 - 3);
		}
		index = decoder.state.reg + decoder.state.extra_register_base;
		if (decoder.state.flags & StateFlags::HAS_REX) != 0 && index >= 4 {
			index += 4;
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, index + Register::AL as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Gb_Eb {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
}

impl OpCodeHandler_Gb_Eb {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		let mut index = decoder.state.reg + decoder.state.extra_register_base;
		if (decoder.state.flags & StateFlags::HAS_REX) != 0 && index >= 4 {
			index += 4;
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, index + Register::AL as u32);

		if decoder.state.mod_ == 3 {
			index = decoder.state.rm + decoder.state.extra_base_register_base;
			if (decoder.state.flags & StateFlags::HAS_REX) != 0 && index >= 4 {
				index += 4;
			}
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(instruction, index + Register::AL as u32);
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_M {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code_w0: u32,
	pub(crate) code_w1: u32,
}

impl OpCodeHandler_M {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if (decoder.state.flags & StateFlags::W) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code_w1);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code_w0);
		}
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
pub(crate) struct OpCodeHandler_M_REXW {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) flags32: u32,
	pub(crate) flags64: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_M_REXW {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if (decoder.state.flags & StateFlags::W) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
		}
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			let flags;
			if (decoder.state.flags & StateFlags::W) != 0 {
				flags = this.flags64;
			} else {
				flags = this.flags32;
			}
			decoder.read_op_mem(instruction);
			if (flags & (HandlerFlags::XACQUIRE | HandlerFlags::XRELEASE)) != 0 {
				decoder.set_xacquire_xrelease(instruction, flags);
			}
			const_assert_eq!(8, HandlerFlags::LOCK);
			const_assert_eq!(0x0000_2000, StateFlags::ALLOW_LOCK);
			decoder.state.flags |= (flags & HandlerFlags::LOCK) << (13 - 3);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_MemBx {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
}

impl OpCodeHandler_MemBx {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		//instruction.set_memory_displacement(0);
		//super::instruction_internal::internal_set_memory_index_scale(instruction, 0);
		//super::instruction_internal::internal_set_memory_displ_size(instruction, 0);
		if decoder.state.address_size == OpSize::Size64 {
			super::instruction_internal::internal_set_memory_base(instruction, Register::RBX);
		} else if decoder.state.address_size == OpSize::Size32 {
			super::instruction_internal::internal_set_memory_base(instruction, Register::EBX);
		} else {
			super::instruction_internal::internal_set_memory_base(instruction, Register::BX);
		}
		super::instruction_internal::internal_set_memory_index(instruction, Register::AL);
		super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_VW {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code_r: u32,
	pub(crate) code_m: u32,
	pub(crate) base_reg: Register,
}

impl OpCodeHandler_VW {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32,
		);
		if decoder.state.mod_ == 3 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code_r);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32,
			);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code_m);
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
			if this.code_m == Code::INVALID as u32 {
				decoder.set_invalid_instruction();
			}
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_WV {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg: Register,
}

impl OpCodeHandler_WV {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32,
			);
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32,
		);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_rDI_VX_RX {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg: Register,
}

impl OpCodeHandler_rDI_VX_RX {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		if decoder.state.address_size == OpSize::Size64 {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::MemorySegRDI);
		} else if decoder.state.address_size == OpSize::Size32 {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::MemorySegEDI);
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::MemorySegDI);
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32,
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
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
pub(crate) struct OpCodeHandler_rDI_P_N {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
}

impl OpCodeHandler_rDI_P_N {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		if decoder.state.address_size == OpSize::Size64 {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::MemorySegRDI);
		} else if decoder.state.address_size == OpSize::Size32 {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::MemorySegEDI);
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::MemorySegDI);
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.reg + Register::MM0 as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op2_register_u32(instruction, decoder.state.rm + Register::MM0 as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_VM {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg: Register,
}

impl OpCodeHandler_VM {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(0, OpKind::Register as u32);
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
pub(crate) struct OpCodeHandler_MV {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg: Register,
}

impl OpCodeHandler_MV {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32,
		);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_VQ {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg: Register,
}

impl OpCodeHandler_VQ {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32,
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.rm + Register::MM0 as u32);
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_P_Q {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
}

impl OpCodeHandler_P_Q {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.reg + Register::MM0 as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.rm + Register::MM0 as u32);
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Q_P {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
}

impl OpCodeHandler_Q_P {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.rm + Register::MM0 as u32);
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.reg + Register::MM0 as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_MP {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
}

impl OpCodeHandler_MP {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.reg + Register::MM0 as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_P_Q_Ib {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
}

impl OpCodeHandler_P_Q_Ib {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.reg + Register::MM0 as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.rm + Register::MM0 as u32);
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
pub(crate) struct OpCodeHandler_P_W {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg: Register,
}

impl OpCodeHandler_P_W {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.reg + Register::MM0 as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
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
pub(crate) struct OpCodeHandler_P_R {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg: Register,
}

impl OpCodeHandler_P_R {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.reg + Register::MM0 as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
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
pub(crate) struct OpCodeHandler_P_Ev {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_P_Ev {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		let gpr;
		if (decoder.state.flags & StateFlags::W) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			gpr = Register::RAX;
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			gpr = Register::EAX;
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.reg + Register::MM0 as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + gpr as u32,
			);
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_P_Ev_Ib {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_P_Ev_Ib {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		let gpr;
		if (decoder.state.flags & StateFlags::W) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			gpr = Register::RAX;
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			gpr = Register::EAX;
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.reg + Register::MM0 as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + gpr as u32,
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
pub(crate) struct OpCodeHandler_Ev_P {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Ev_P {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		let gpr;
		if (decoder.state.flags & StateFlags::W) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			gpr = Register::RAX;
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			gpr = Register::EAX;
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + gpr as u32,
			);
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.reg + Register::MM0 as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Gv_W {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code_w0: u32,
	pub(crate) code_w1: u32,
	pub(crate) base_reg: Register,
}

impl OpCodeHandler_Gv_W {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if (decoder.state.flags & StateFlags::W) != 0 {
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
pub(crate) struct OpCodeHandler_V_Ev {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code_w0: u32,
	pub(crate) code_w1: u32,
	pub(crate) base_reg: Register,
}

impl OpCodeHandler_V_Ev {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		let gpr;
		if decoder.state.operand_size != OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code_w0);
			gpr = Register::EAX;
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code_w1);
			gpr = Register::RAX;
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32,
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + gpr as u32,
			);
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_VWIb {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code_w0: u32,
	pub(crate) code_w1: u32,
	pub(crate) base_reg: Register,
}

impl OpCodeHandler_VWIb {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if (decoder.state.flags & StateFlags::W) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code_w1);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code_w0);
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32,
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32,
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
pub(crate) struct OpCodeHandler_VRIbIb {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg: Register,
}

impl OpCodeHandler_VRIbIb {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32,
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
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
		super::instruction_internal::internal_set_op3_kind(instruction, OpKind::Immediate8_2nd);
		super::instruction_internal::internal_set_immediate8_2nd(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_RIbIb {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg: Register,
}

impl OpCodeHandler_RIbIb {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32,
			);
		} else {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
		super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Immediate8_2nd);
		super::instruction_internal::internal_set_immediate8_2nd(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_RIb {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg: Register,
}

impl OpCodeHandler_RIb {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32,
			);
		} else {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Ed_V_Ib {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
	pub(crate) base_reg: Register,
}

impl OpCodeHandler_Ed_V_Ib {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		let gpr;
		if (decoder.state.flags & StateFlags::W) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			gpr = Register::RAX;
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			gpr = Register::EAX;
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + gpr as u32,
			);
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		const_assert_eq!(0, OpKind::Register as u32);
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
pub(crate) struct OpCodeHandler_VX_Ev {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_VX_Ev {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		let gpr;
		if (decoder.state.flags & StateFlags::W) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			gpr = Register::RAX;
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			gpr = Register::EAX;
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + Register::XMM0 as u32,
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + gpr as u32,
			);
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Ev_VX {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Ev_VX {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		let gpr;
		if (decoder.state.flags & StateFlags::W) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			gpr = Register::RAX;
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			gpr = Register::EAX;
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + gpr as u32,
			);
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + Register::XMM0 as u32,
		);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_VX_E_Ib {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
	pub(crate) base_reg: Register,
}

impl OpCodeHandler_VX_E_Ib {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		let gpr;
		if (decoder.state.flags & StateFlags::W) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			gpr = Register::RAX;
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			gpr = Register::EAX;
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32,
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + gpr as u32,
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
pub(crate) struct OpCodeHandler_Gv_RX {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
	pub(crate) base_reg: Register,
}

impl OpCodeHandler_Gv_RX {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if (decoder.state.flags & StateFlags::W) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		if (decoder.state.flags & StateFlags::W) != 0 {
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32,
			);
		} else {
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
				decoder.state.rm + decoder.state.extra_base_register_base + this.base_reg as u32,
			);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_B_MIB {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
}

impl OpCodeHandler_B_MIB {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.reg > 3 || ((decoder.state.extra_register_base & decoder.invalid_check_mask) != 0) {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.reg + Register::BND0 as u32);
		debug_assert_ne!(3, decoder.state.mod_);
		super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
		decoder.read_op_mem_mpx(instruction);
		// It can't be EIP since if it's MPX + 64-bit, the address size is always 64-bit
		if decoder.invalid_check_mask != 0 && instruction.memory_base() == Register::RIP {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_MIB_B {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
}

impl OpCodeHandler_MIB_B {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.reg > 3 || ((decoder.state.extra_register_base & decoder.invalid_check_mask) != 0) {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		debug_assert_ne!(3, decoder.state.mod_);
		super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
		decoder.read_op_mem_mpx(instruction);
		// It can't be EIP since if it's MPX + 64-bit, the address size is always 64-bit
		if decoder.invalid_check_mask != 0 && instruction.memory_base() == Register::RIP {
			decoder.set_invalid_instruction();
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.reg + Register::BND0 as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_B_BM {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_B_BM {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if (decoder.state.reg | decoder.state.rm) > 3
			|| (((decoder.state.extra_register_base | decoder.state.extra_base_register_base) & decoder.invalid_check_mask) != 0)
		{
			decoder.set_invalid_instruction();
		}
		if decoder.is64_mode {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.reg + Register::BND0 as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.rm + Register::BND0 as u32);
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem_mpx(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_BM_B {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_BM_B {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if (decoder.state.reg | decoder.state.rm) > 3
			|| (((decoder.state.extra_register_base | decoder.state.extra_base_register_base) & decoder.invalid_check_mask) != 0)
		{
			decoder.set_invalid_instruction();
		}
		if decoder.is64_mode {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.rm + Register::BND0 as u32);
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem_mpx(instruction);
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.reg + Register::BND0 as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_B_Ev {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_B_Ev {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.reg > 3 || ((decoder.state.extra_register_base & decoder.invalid_check_mask) != 0) {
			decoder.set_invalid_instruction();
		}
		let base_reg;
		if decoder.is64_mode {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			base_reg = Register::RAX;
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			base_reg = Register::EAX;
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(instruction, decoder.state.reg + Register::BND0 as u32);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + base_reg as u32,
			);
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem_mpx(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Mv_Gv_REXW {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Mv_Gv_REXW {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		if (decoder.state.flags & StateFlags::W) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32,
			);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32,
			);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Gv_N_Ib_REX {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Gv_N_Ib_REX {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if (decoder.state.flags & StateFlags::W) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		if (decoder.state.flags & StateFlags::W) != 0 {
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32,
			);
		} else {
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32,
			);
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.rm + Register::MM0 as u32);
		} else {
			decoder.set_invalid_instruction();
		}
		super::instruction_internal::internal_set_op2_kind(instruction, OpKind::Immediate8);
		super::instruction_internal::internal_set_immediate8(instruction, decoder.read_u8() as u32);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Gv_N {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Gv_N {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if (decoder.state.flags & StateFlags::W) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
		}
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		if (decoder.state.flags & StateFlags::W) != 0 {
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32,
			);
		} else {
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32,
			);
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.rm + Register::MM0 as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_VN {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
	pub(crate) base_reg: Register,
}

impl OpCodeHandler_VN {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
		const_assert_eq!(0, OpKind::Register as u32);
		//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
		super::instruction_internal::internal_set_op0_register_u32(
			instruction,
			decoder.state.reg + decoder.state.extra_register_base + this.base_reg as u32,
		);
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(instruction, decoder.state.rm + Register::MM0 as u32);
		} else {
			decoder.set_invalid_instruction();
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Gv_Mv {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Gv_Mv {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.operand_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32,
			);
		} else if decoder.state.operand_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32,
			);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::AX as u32,
			);
		}
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
pub(crate) struct OpCodeHandler_Mv_Gv {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code16: u32,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Mv_Gv {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if decoder.state.mod_ == 3 {
			decoder.set_invalid_instruction();
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		if decoder.state.operand_size == OpSize::Size32 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32,
			);
		} else if decoder.state.operand_size == OpSize::Size64 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32,
			);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code16);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::AX as u32,
			);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Gv_Eb_REX {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Gv_Eb_REX {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if (decoder.state.flags & StateFlags::W) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32,
			);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32,
			);
		}
		if decoder.state.mod_ == 3 {
			let mut index = decoder.state.rm + decoder.state.extra_base_register_base;
			if (decoder.state.flags & StateFlags::HAS_REX) != 0 && index >= 4 {
				index += 4;
			}
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(instruction, index + Register::AL as u32);
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Gv_Ev_REX {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Gv_Ev_REX {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if (decoder.state.flags & StateFlags::W) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32,
			);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
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
			if (decoder.state.flags & StateFlags::W) != 0 {
				super::instruction_internal::internal_set_op1_register_u32(
					instruction,
					decoder.state.rm + decoder.state.extra_base_register_base + Register::RAX as u32,
				);
			} else {
				super::instruction_internal::internal_set_op1_register_u32(
					instruction,
					decoder.state.rm + decoder.state.extra_base_register_base + Register::EAX as u32,
				);
			}
		} else {
			super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Ev_Gv_REX {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
}

impl OpCodeHandler_Ev_Gv_REX {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		debug_assert_ne!(3, decoder.state.mod_);
		super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
		decoder.read_op_mem(instruction);
		if (decoder.state.flags & StateFlags::W) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::RAX as u32,
			);
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op1_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op1_register_u32(
				instruction,
				decoder.state.reg + decoder.state.extra_register_base + Register::EAX as u32,
			);
		}
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_GvM_VX_Ib {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code32: u32,
	pub(crate) code64: u32,
	pub(crate) base_reg: Register,
}

impl OpCodeHandler_GvM_VX_Ib {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		let gpr;
		if (decoder.state.flags & StateFlags::W) != 0 {
			super::instruction_internal::internal_set_code_u32(instruction, this.code64);
			gpr = Register::RAX;
		} else {
			super::instruction_internal::internal_set_code_u32(instruction, this.code32);
			gpr = Register::EAX;
		}
		if decoder.state.mod_ == 3 {
			const_assert_eq!(0, OpKind::Register as u32);
			//super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Register);
			super::instruction_internal::internal_set_op0_register_u32(
				instruction,
				decoder.state.rm + decoder.state.extra_base_register_base + gpr as u32,
			);
		} else {
			super::instruction_internal::internal_set_op0_kind(instruction, OpKind::Memory);
			decoder.read_op_mem(instruction);
		}
		const_assert_eq!(0, OpKind::Register as u32);
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
pub(crate) struct OpCodeHandler_Wbinvd {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
}

impl OpCodeHandler_Wbinvd {
	pub(crate) fn decode(_self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		debug_assert_eq!(EncodingKind::Legacy, decoder.state.encoding());
		if (decoder.options & DecoderOptions::NO_WBNOINVD) != 0 || decoder.state.mandatory_prefix != MandatoryPrefixByte::PF3 as u32 {
			super::instruction_internal::internal_set_code(instruction, Code::Wbinvd);
		} else {
			decoder.clear_mandatory_prefix_f3(instruction);
			super::instruction_internal::internal_set_code(instruction, Code::Wbnoinvd);
		}
	}
}
