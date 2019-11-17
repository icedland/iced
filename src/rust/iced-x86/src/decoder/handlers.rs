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

pub(crate) type OpCodeHandlerDecodeFn = fn(*const OpCodeHandler, &mut Decoder, &mut Instruction);

#[cfg_attr(feature = "cargo-fmt", rustfmt::skip)]
pub(crate) static GEN_NULL_HANDLER: OpCodeHandler_Invalid = OpCodeHandler_Invalid {
	decode: OpCodeHandler_Invalid::decode,
	has_modrm: true,
};

#[repr(C)]
pub(crate) struct OpCodeHandler {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Invalid {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
}

impl OpCodeHandler_Invalid {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		decoder.set_invalid_instruction();
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Simple {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) code: u32,
}

impl OpCodeHandler_Simple {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		super::instruction_internal::internal_set_code_u32(instruction, this.code);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Group8x8 {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) table_low: &'static [&'static OpCodeHandler],
	pub(crate) table_high: &'static [&'static OpCodeHandler],
}

impl OpCodeHandler_Group8x8 {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		let handler = if decoder.state.mod_ == 3 {
			// Safe, table size is 8 and reg is 0..7
			unsafe { *this.table_high.as_ptr().offset(decoder.state.reg as isize) }
		} else {
			// Safe, table size is 8 and reg is 0..7
			unsafe { *this.table_low.as_ptr().offset(decoder.state.reg as isize) }
		};
		(handler.decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Group8x64 {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) table_low: &'static [&'static OpCodeHandler],
	pub(crate) table_high: &'static [&'static OpCodeHandler],
}

impl OpCodeHandler_Group8x64 {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		let mut handler;
		if decoder.state.mod_ == 3 {
			// Safe, table size is 64 and reg is 0..7
			handler = unsafe { *this.table_high.as_ptr().offset((decoder.state.modrm & 0x3F) as isize) };
			if handler as *const OpCodeHandler as *const u8 == &GEN_NULL_HANDLER as *const OpCodeHandler_Invalid as *const u8 {
				// Safe, table size is 8 and reg is 0..7
				handler = unsafe { *this.table_low.as_ptr().offset(decoder.state.reg as isize) };
			}
		} else {
			// Safe, table size is 8 and reg is 0..7
			handler = unsafe { *this.table_low.as_ptr().offset(decoder.state.reg as isize) };
		}
		(handler.decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Group {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) group_handlers: &'static [&'static OpCodeHandler],
}

impl OpCodeHandler_Group {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		// Safe, table size is 8 and reg is 0..7
		let handler = unsafe { *this.group_handlers.as_ptr().offset(decoder.state.reg as isize) };
		(handler.decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_AnotherTable {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) group_handlers: &'static [&'static OpCodeHandler],
}

impl OpCodeHandler_AnotherTable {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		decoder.decode_table(this.group_handlers, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_MandatoryPrefix2 {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) handlers: [&'static OpCodeHandler; 4],
}

impl OpCodeHandler_MandatoryPrefix2 {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(
			decoder.state.encoding() == EncodingKind::VEX
				|| decoder.state.encoding() == EncodingKind::EVEX
				|| decoder.state.encoding() == EncodingKind::XOP
		);
		// Safe, table size is 4 and mandatory_prefix is 0..3
		let handler = unsafe { *this.handlers.as_ptr().offset(decoder.state.mandatory_prefix as isize) };
		(handler.decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_W {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) handlers: [&'static OpCodeHandler; 2],
}

impl OpCodeHandler_W {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(
			decoder.state.encoding() == EncodingKind::VEX
				|| decoder.state.encoding() == EncodingKind::EVEX
				|| decoder.state.encoding() == EncodingKind::XOP
		);
		const_assert_eq!(0x80, StateFlags::W);
		let index = (decoder.state.flags >> 7) & 1;
		// Safe, table size is 2 and index is 0..1
		let handler = unsafe { *this.handlers.as_ptr().offset(index as isize) };
		(handler.decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Bitness {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) handler1632: &'static OpCodeHandler,
	pub(crate) handler64: &'static OpCodeHandler,
}

impl OpCodeHandler_Bitness {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
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
pub(crate) struct OpCodeHandler_Bitness_DontReadModRM {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) handler1632: &'static OpCodeHandler,
	pub(crate) handler64: &'static OpCodeHandler,
}

impl OpCodeHandler_Bitness_DontReadModRM {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		let handler = if decoder.is64_mode { this.handler64 } else { this.handler1632 };
		(handler.decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_RM {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) reg: &'static OpCodeHandler,
	pub(crate) mem: &'static OpCodeHandler,
}

impl OpCodeHandler_RM {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		let handler = if decoder.state.mod_ == 3 { this.reg } else { this.mem };
		(handler.decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(crate) struct OpCodeHandler_Options {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) default_handler: &'static OpCodeHandler,
	pub(crate) infos: [(&'static OpCodeHandler, u32); 2],
}

impl OpCodeHandler_Options {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
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
pub(crate) struct OpCodeHandler_Options_DontReadModRM {
	pub(crate) decode: OpCodeHandlerDecodeFn,
	pub(crate) has_modrm: bool,
	pub(crate) default_handler: &'static OpCodeHandler,
	pub(crate) opt_handler: &'static OpCodeHandler,
	pub(crate) flags: u32,
}

impl OpCodeHandler_Options_DontReadModRM {
	pub(crate) fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		let mut handler = this.default_handler;
		let options = decoder.options;
		if (options & this.flags) != 0 {
			handler = this.opt_handler;
		}
		(handler.decode)(handler, decoder, instruction);
	}
}
