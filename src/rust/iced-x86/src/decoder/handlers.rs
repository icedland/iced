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

use super::super::*;
use super::*;
#[cfg(not(feature = "std"))]
use alloc::vec::Vec;

pub(super) type OpCodeHandlerDecodeFn = fn(*const OpCodeHandler, &mut Decoder, &mut Instruction);

#[allow(trivial_casts)]
#[cfg_attr(has_must_use, must_use)]
#[cfg_attr(not(feature = "javascript"), inline)]
pub(super) fn is_null_instance_handler(handler: *const OpCodeHandler) -> bool {
	handler as *const u8 == &NULL_HANDLER as *const _ as *const u8
}

#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
pub(super) static NULL_HANDLER: OpCodeHandler_Invalid = OpCodeHandler_Invalid {
	decode: OpCodeHandler_Invalid::decode,
	has_modrm: true,
};
#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
pub(super) static INVALID_HANDLER: OpCodeHandler_Invalid = OpCodeHandler_Invalid {
	decode: OpCodeHandler_Invalid::decode,
	has_modrm: true,
};
#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
pub(super) static INVALID_NO_MODRM_HANDLER: OpCodeHandler_Invalid = OpCodeHandler_Invalid {
	decode: OpCodeHandler_Invalid::decode,
	has_modrm: false,
};

#[repr(C)]
pub(super) struct OpCodeHandler {
	pub(super) decode: OpCodeHandlerDecodeFn,
	pub(super) has_modrm: bool,
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Invalid {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
}

impl OpCodeHandler_Invalid {
	fn decode(_self_ptr: *const OpCodeHandler, decoder: &mut Decoder, _instruction: &mut Instruction) {
		decoder.set_invalid_instruction();
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Simple {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	code: u32,
}

impl OpCodeHandler_Simple {
	pub(super) fn new(code: u32) -> Self {
		Self { decode: OpCodeHandler_Simple::decode, has_modrm: false, code }
	}

	pub(super) fn new_modrm(code: u32) -> Self {
		Self { decode: OpCodeHandler_Simple::decode, has_modrm: true, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, _decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Group8x8 {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	table_low: Vec<&'static OpCodeHandler>,
	table_high: Vec<&'static OpCodeHandler>,
}

impl OpCodeHandler_Group8x8 {
	pub(super) fn new(table_low: Vec<&'static OpCodeHandler>, table_high: Vec<&'static OpCodeHandler>) -> Self {
		assert_eq!(8, table_low.len());
		assert_eq!(8, table_high.len());
		Self { decode: OpCodeHandler_Group8x8::decode, has_modrm: true, table_low, table_high }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		let handler = if decoder.state.mod_ == 3 {
			unsafe { *this.table_high.get_unchecked(decoder.state.reg as usize) }
		} else {
			unsafe { *this.table_low.get_unchecked(decoder.state.reg as usize) }
		};
		(handler.decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Group8x64 {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	table_low: Vec<&'static OpCodeHandler>,
	table_high: Vec<&'static OpCodeHandler>,
}

impl OpCodeHandler_Group8x64 {
	pub(super) fn new(table_low: Vec<&'static OpCodeHandler>, table_high: Vec<&'static OpCodeHandler>) -> Self {
		assert_eq!(8, table_low.len());
		assert_eq!(0x40, table_high.len());
		Self { decode: OpCodeHandler_Group8x64::decode, has_modrm: true, table_low, table_high }
	}

	#[allow(trivial_casts)]
	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		let mut handler;
		if decoder.state.mod_ == 3 {
			handler = unsafe { *this.table_high.get_unchecked((decoder.state.modrm & 0x3F) as usize) };
			if handler as *const _ as *const u8 == &NULL_HANDLER as *const _ as *const u8 {
				handler = unsafe { *this.table_low.get_unchecked(decoder.state.reg as usize) };
			}
		} else {
			handler = unsafe { *this.table_low.get_unchecked(decoder.state.reg as usize) };
		}
		(handler.decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Group {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	group_handlers: Vec<&'static OpCodeHandler>,
}

impl OpCodeHandler_Group {
	pub(super) fn new(group_handlers: Vec<&'static OpCodeHandler>) -> Self {
		assert_eq!(8, group_handlers.len());
		Self { decode: OpCodeHandler_Group::decode, has_modrm: true, group_handlers }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		let handler = unsafe { *this.group_handlers.get_unchecked(decoder.state.reg as usize) };
		(handler.decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_AnotherTable {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	handlers: Vec<&'static OpCodeHandler>,
}

impl OpCodeHandler_AnotherTable {
	pub(super) fn new(handlers: Vec<&'static OpCodeHandler>) -> Self {
		assert_eq!(0x100, handlers.len());
		Self { decode: OpCodeHandler_AnotherTable::decode, has_modrm: false, handlers }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		decoder.decode_table(this.handlers.as_ptr(), instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_MandatoryPrefix2 {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	handlers: [&'static OpCodeHandler; 4],
}

impl OpCodeHandler_MandatoryPrefix2 {
	pub(super) fn new(
		has_modrm: bool, handler: *const OpCodeHandler, handler_66: *const OpCodeHandler, handler_f3: *const OpCodeHandler,
		handler_f2: *const OpCodeHandler,
	) -> Self {
		const_assert_eq!(0, MandatoryPrefixByte::None as u32);
		const_assert_eq!(1, MandatoryPrefixByte::P66 as u32);
		const_assert_eq!(2, MandatoryPrefixByte::PF3 as u32);
		const_assert_eq!(3, MandatoryPrefixByte::PF2 as u32);
		assert!(!is_null_instance_handler(handler));
		assert!(!is_null_instance_handler(handler_66));
		assert!(!is_null_instance_handler(handler_f3));
		assert!(!is_null_instance_handler(handler_f2));
		let handlers = unsafe { [&*handler, &*handler_66, &*handler_f3, &*handler_f2] };
		debug_assert_eq!(has_modrm, handlers[0].has_modrm);
		debug_assert_eq!(has_modrm, handlers[1].has_modrm);
		debug_assert_eq!(has_modrm, handlers[2].has_modrm);
		debug_assert_eq!(has_modrm, handlers[3].has_modrm);
		Self { decode: OpCodeHandler_MandatoryPrefix2::decode, has_modrm, handlers: unsafe { [&*handler, &*handler_66, &*handler_f3, &*handler_f2] } }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(
			decoder.state.encoding() == EncodingKind::VEX
				|| decoder.state.encoding() == EncodingKind::EVEX
				|| decoder.state.encoding() == EncodingKind::XOP
		);
		let handler = unsafe { *this.handlers.get_unchecked(decoder.state.mandatory_prefix as usize) };
		(handler.decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_W {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	handlers: [&'static OpCodeHandler; 2],
}

impl OpCodeHandler_W {
	pub(super) fn new(handler_w0: *const OpCodeHandler, handler_w1: *const OpCodeHandler) -> Self {
		assert!(!is_null_instance_handler(handler_w0));
		assert!(!is_null_instance_handler(handler_w1));
		Self { decode: OpCodeHandler_W::decode, has_modrm: true, handlers: unsafe { [&*handler_w0, &*handler_w1] } }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(
			decoder.state.encoding() == EncodingKind::VEX
				|| decoder.state.encoding() == EncodingKind::EVEX
				|| decoder.state.encoding() == EncodingKind::XOP
		);
		const_assert_eq!(0x80, StateFlags::W);
		let index = (decoder.state.flags >> 7) & 1;
		let handler = unsafe { *this.handlers.get_unchecked(index as usize) };
		(handler.decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Bitness {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	handler1632: &'static OpCodeHandler,
	handler64: &'static OpCodeHandler,
}

impl OpCodeHandler_Bitness {
	pub(super) fn new(handler1632: *const OpCodeHandler, handler64: *const OpCodeHandler) -> Self {
		assert!(!is_null_instance_handler(handler1632));
		assert!(!is_null_instance_handler(handler64));
		Self { decode: OpCodeHandler_Bitness::decode, has_modrm: false, handler1632: unsafe { &*handler1632 }, handler64: unsafe { &*handler64 } }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		let handler = if decoder.is64_mode { this.handler64 } else { this.handler1632 };
		if handler.has_modrm {
			decoder.read_modrm();
		}
		(handler.decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Bitness_DontReadModRM {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	handler1632: &'static OpCodeHandler,
	handler64: &'static OpCodeHandler,
}

impl OpCodeHandler_Bitness_DontReadModRM {
	pub(super) fn new(handler1632: *const OpCodeHandler, handler64: *const OpCodeHandler) -> Self {
		assert!(!is_null_instance_handler(handler1632));
		assert!(!is_null_instance_handler(handler64));
		Self {
			decode: OpCodeHandler_Bitness_DontReadModRM::decode,
			has_modrm: true,
			handler1632: unsafe { &*handler1632 },
			handler64: unsafe { &*handler64 },
		}
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		let handler = if decoder.is64_mode { this.handler64 } else { this.handler1632 };
		(handler.decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_RM {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	reg: &'static OpCodeHandler,
	mem: &'static OpCodeHandler,
}

impl OpCodeHandler_RM {
	pub(super) fn new(reg: *const OpCodeHandler, mem: *const OpCodeHandler) -> Self {
		assert!(!is_null_instance_handler(reg));
		assert!(!is_null_instance_handler(mem));
		Self { decode: OpCodeHandler_RM::decode, has_modrm: true, reg: unsafe { &*reg }, mem: unsafe { &*mem } }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		let handler = if decoder.state.mod_ == 3 { this.reg } else { this.mem };
		(handler.decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Options {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	default_handler: &'static OpCodeHandler,
	infos: [(&'static OpCodeHandler, u32); 2],
}

impl OpCodeHandler_Options {
	#[allow(trivial_casts)]
	pub(super) fn new(default_handler: *const OpCodeHandler, handler1: *const OpCodeHandler, options1: u32) -> Self {
		assert!(!is_null_instance_handler(default_handler));
		assert!(!is_null_instance_handler(handler1));
		Self {
			decode: OpCodeHandler_Options::decode,
			has_modrm: false,
			default_handler: unsafe { &*default_handler },
			infos: [(unsafe { &*handler1 }, options1), (unsafe { &*(&INVALID_NO_MODRM_HANDLER as *const _ as *const OpCodeHandler) }, 0)],
		}
	}

	pub(super) fn new2(
		default_handler: *const OpCodeHandler, handler1: *const OpCodeHandler, options1: u32, handler2: *const OpCodeHandler, options2: u32,
	) -> Self {
		assert!(!is_null_instance_handler(default_handler));
		assert!(!is_null_instance_handler(handler1));
		assert!(!is_null_instance_handler(handler2));
		Self {
			decode: OpCodeHandler_Options::decode,
			has_modrm: false,
			default_handler: unsafe { &*default_handler },
			infos: [(unsafe { &*handler1 }, options1), (unsafe { &*handler2 }, options2)],
		}
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		let mut handler = this.default_handler;
		let options = decoder.options;
		for info in &this.infos {
			if (options & info.1) != 0 {
				handler = info.0;
				break;
			}
		}
		if handler.has_modrm {
			decoder.read_modrm();
		}
		(handler.decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Options_DontReadModRM {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	default_handler: &'static OpCodeHandler,
	opt_handler: &'static OpCodeHandler,
	flags: u32,
}

impl OpCodeHandler_Options_DontReadModRM {
	pub(super) fn new(default_handler: *const OpCodeHandler, opt_handler: *const OpCodeHandler, flags: u32) -> Self {
		assert!(!is_null_instance_handler(default_handler));
		assert!(!is_null_instance_handler(opt_handler));
		Self {
			decode: OpCodeHandler_Options_DontReadModRM::decode,
			has_modrm: true,
			default_handler: unsafe { &*default_handler },
			opt_handler: unsafe { &*opt_handler },
			flags,
		}
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		let mut handler = this.default_handler;
		let options = decoder.options;
		if (options & this.flags) != 0 {
			handler = this.opt_handler;
		}
		(handler.decode)(handler, decoder, instruction);
	}
}
