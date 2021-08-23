// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::decoder::*;
use crate::*;
use alloc::boxed::Box;
use alloc::vec::Vec;
use core::convert::TryInto;

// SAFETY:
//	code: let this = unsafe { &*(self_ptr as *const Self) };
// The first arg (`self_ptr`) to decode() is always the handler itself, cast to a `*const OpCodeHandler`.
// All handlers are `#[repr(C)]` structs so the OpCodeHandler fields are always at the same offsets.

pub(super) type OpCodeHandlerDecodeFn = fn(*const OpCodeHandler, &mut Decoder<'_>, &mut Instruction);

#[allow(trivial_casts)]
#[must_use]
#[inline]
pub(super) fn is_null_instance_handler(handler: &OpCodeHandler) -> bool {
	handler as *const _ as *const u8 == &NULL_HANDLER as *const _ as *const u8
}

#[allow(trivial_casts)]
#[must_use]
#[inline]
pub(super) fn get_null_handler() -> (OpCodeHandlerDecodeFn, &'static OpCodeHandler) {
	// SAFETY: it's #[repr(C)] and the first part is an `OpCodeHandler`
	(NULL_HANDLER.decode, unsafe { &*(&NULL_HANDLER as *const _ as *const OpCodeHandler) })
}

#[allow(trivial_casts)]
#[must_use]
#[inline]
pub(super) fn get_invalid_handler() -> (OpCodeHandlerDecodeFn, &'static OpCodeHandler) {
	// SAFETY: it's #[repr(C)] and the first part is an `OpCodeHandler`
	(INVALID_HANDLER.decode, unsafe { &*(&INVALID_HANDLER as *const _ as *const OpCodeHandler) })
}

#[allow(trivial_casts)]
#[must_use]
#[inline]
pub(super) fn get_invalid_no_modrm_handler() -> (OpCodeHandlerDecodeFn, &'static OpCodeHandler) {
	// SAFETY: it's #[repr(C)] and the first part is an `OpCodeHandler`
	(INVALID_NO_MODRM_HANDLER.decode, unsafe { &*(&INVALID_NO_MODRM_HANDLER as *const _ as *const OpCodeHandler) })
}

#[rustfmt::skip]
pub(super) static NULL_HANDLER: OpCodeHandler_Invalid = OpCodeHandler_Invalid {
	decode: OpCodeHandler_Invalid::decode,
	has_modrm: true,
};
#[rustfmt::skip]
pub(super) static INVALID_HANDLER: OpCodeHandler_Invalid = OpCodeHandler_Invalid {
	decode: OpCodeHandler_Invalid::decode,
	has_modrm: true,
};
#[rustfmt::skip]
pub(super) static INVALID_NO_MODRM_HANDLER: OpCodeHandler_Invalid = OpCodeHandler_Invalid {
	decode: OpCodeHandler_Invalid::decode,
	has_modrm: false,
};

#[repr(C)]
pub(super) struct OpCodeHandler {
	pub(super) has_modrm: bool,
	pub(super) decode: OpCodeHandlerDecodeFn,
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Invalid {
	has_modrm: bool,
	pub(super) decode: OpCodeHandlerDecodeFn,
}

impl OpCodeHandler_Invalid {
	fn decode(_self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, _instruction: &mut Instruction) {
		decoder.set_invalid_instruction();
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Simple {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	code: Code,
}

impl OpCodeHandler_Simple {
	pub(super) fn new(code: Code) -> Self {
		Self { has_modrm: false, decode: OpCodeHandler_Simple::decode, code }
	}

	pub(super) fn new_modrm(code: Code) -> Self {
		Self { has_modrm: true, decode: OpCodeHandler_Simple::decode, code }
	}

	fn decode(self_ptr: *const OpCodeHandler, _decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		instruction.set_code(this.code);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Group8x8 {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	table_low: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	table_high: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
}

impl OpCodeHandler_Group8x8 {
	pub(super) fn new(
		table_low: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>, table_high: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	) -> Self {
		debug_assert_eq!(table_low.len(), 8);
		debug_assert_eq!(table_high.len(), 8);
		Self { has_modrm: true, decode: OpCodeHandler_Group8x8::decode, table_low, table_high }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.reg <= 7);
		let (decode, handler) = if decoder.state.mod_ == 3 {
			// SAFETY: reg <= 7 and table_high.len() == 8 (see ctor)
			unsafe { *this.table_high.get_unchecked(decoder.state.reg as usize) }
		} else {
			// SAFETY: reg <= 7 and table_low.len() == 8 (see ctor)
			unsafe { *this.table_low.get_unchecked(decoder.state.reg as usize) }
		};
		(decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Group8x64 {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	table_low: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	table_high: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
}

impl OpCodeHandler_Group8x64 {
	pub(super) fn new(
		table_low: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>, table_high: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	) -> Self {
		debug_assert_eq!(table_low.len(), 8);
		debug_assert_eq!(table_high.len(), 0x40);
		Self { has_modrm: true, decode: OpCodeHandler_Group8x64::decode, table_low, table_high }
	}

	#[allow(trivial_casts)]
	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		let (decode, handler) = if decoder.state.mod_ == 3 {
			// SAFETY: table_high.len() == 0x40 (see ctor) and index <= 0x3F due to masking `modrm`
			let (decode, handler) = unsafe { *this.table_high.get_unchecked((decoder.state.modrm & 0x3F) as usize) };
			if handler as *const _ as *const u8 == &NULL_HANDLER as *const _ as *const u8 {
				debug_assert!(decoder.state.reg <= 7);
				// SAFETY: reg <= 7 and table_low.len() == 8 (see ctor)
				unsafe { *this.table_low.get_unchecked(decoder.state.reg as usize) }
			} else {
				(decode, handler)
			}
		} else {
			debug_assert!(decoder.state.reg <= 7);
			// SAFETY: reg <= 7 and table_low.len() == 8 (see ctor)
			unsafe { *this.table_low.get_unchecked(decoder.state.reg as usize) }
		};
		(decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Group {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	group_handlers: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
}

impl OpCodeHandler_Group {
	pub(super) fn new(group_handlers: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>) -> Self {
		debug_assert_eq!(group_handlers.len(), 8);
		Self { has_modrm: true, decode: OpCodeHandler_Group::decode, group_handlers }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.reg <= 7);
		// SAFETY: group_handlers.len() == 8 (see ctor) and reg <= 7
		let (decode, handler) = unsafe { *this.group_handlers.get_unchecked(decoder.state.reg as usize) };
		(decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_AnotherTable {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	handlers: Box<[(OpCodeHandlerDecodeFn, &'static OpCodeHandler); 0x100]>,
}

impl OpCodeHandler_AnotherTable {
	#[allow(clippy::unwrap_used)]
	pub(super) fn new(handlers: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>) -> Self {
		let handlers = handlers.into_boxed_slice().try_into().ok().unwrap();
		Self { has_modrm: false, decode: OpCodeHandler_AnotherTable::decode, handlers }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		decoder.decode_table(&this.handlers, instruction);
	}
}

#[cfg(any(not(feature = "no_vex"), not(feature = "no_xop"), not(feature = "no_evex")))]
#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_MandatoryPrefix2 {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	handlers: [(OpCodeHandlerDecodeFn, &'static OpCodeHandler); 4],
}

#[cfg(any(not(feature = "no_vex"), not(feature = "no_xop"), not(feature = "no_evex")))]
impl OpCodeHandler_MandatoryPrefix2 {
	pub(super) fn new(
		has_modrm: bool, handler: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), handler_66: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
		handler_f3: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), handler_f2: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	) -> Self {
		const_assert_eq!(DecoderMandatoryPrefix::PNP as u32, 0);
		const_assert_eq!(DecoderMandatoryPrefix::P66 as u32, 1);
		const_assert_eq!(DecoderMandatoryPrefix::PF3 as u32, 2);
		const_assert_eq!(DecoderMandatoryPrefix::PF2 as u32, 3);
		debug_assert!(!is_null_instance_handler(handler.1));
		debug_assert!(!is_null_instance_handler(handler_66.1));
		debug_assert!(!is_null_instance_handler(handler_f3.1));
		debug_assert!(!is_null_instance_handler(handler_f2.1));
		let handlers = [handler, handler_66, handler_f3, handler_f2];
		debug_assert_eq!(handlers[0].1.has_modrm, has_modrm);
		debug_assert_eq!(handlers[1].1.has_modrm, has_modrm);
		debug_assert_eq!(handlers[2].1.has_modrm, has_modrm);
		debug_assert_eq!(handlers[3].1.has_modrm, has_modrm);
		Self { decode: OpCodeHandler_MandatoryPrefix2::decode, has_modrm, handlers }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(
			decoder.state.encoding() == EncodingKind::VEX
				|| decoder.state.encoding() == EncodingKind::EVEX
				|| decoder.state.encoding() == EncodingKind::XOP
		);
		let (decode, handler) = unsafe { *this.handlers.get_unchecked(decoder.state.mandatory_prefix as usize) };
		(decode)(handler, decoder, instruction);
	}
}

#[cfg(any(not(feature = "no_vex"), not(feature = "no_xop"), not(feature = "no_evex")))]
#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_W {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	handlers: [(OpCodeHandlerDecodeFn, &'static OpCodeHandler); 2],
}

#[cfg(any(not(feature = "no_vex"), not(feature = "no_xop"), not(feature = "no_evex")))]
impl OpCodeHandler_W {
	pub(super) fn new(
		handler_w0: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), handler_w1: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	) -> Self {
		debug_assert!(!is_null_instance_handler(handler_w0.1));
		debug_assert!(!is_null_instance_handler(handler_w1.1));
		Self { decode: OpCodeHandler_W::decode, has_modrm: true, handlers: [handler_w0, handler_w1] }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(
			decoder.state.encoding() == EncodingKind::VEX
				|| decoder.state.encoding() == EncodingKind::EVEX
				|| decoder.state.encoding() == EncodingKind::XOP
		);
		const_assert_eq!(StateFlags::W, 0x80);
		let index = (decoder.state.flags >> 7) & 1;
		let (decode, handler) = unsafe { *this.handlers.get_unchecked(index as usize) };
		(decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Bitness {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	handler1632: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	handler64: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
}

impl OpCodeHandler_Bitness {
	pub(super) fn new(
		handler1632: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), handler64: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	) -> Self {
		debug_assert!(!is_null_instance_handler(handler1632.1));
		debug_assert!(!is_null_instance_handler(handler64.1));
		Self { decode: OpCodeHandler_Bitness::decode, has_modrm: false, handler1632, handler64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		let (decode, handler) = if decoder.is64b_mode { this.handler64 } else { this.handler1632 };
		if handler.has_modrm {
			decoder.read_modrm();
		}
		(decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Bitness_DontReadModRM {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	handler1632: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	handler64: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
}

impl OpCodeHandler_Bitness_DontReadModRM {
	pub(super) fn new(
		handler1632: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), handler64: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	) -> Self {
		debug_assert!(!is_null_instance_handler(handler1632.1));
		debug_assert!(!is_null_instance_handler(handler64.1));
		Self { decode: OpCodeHandler_Bitness_DontReadModRM::decode, has_modrm: true, handler1632, handler64 }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		let (decode, handler) = if decoder.is64b_mode { this.handler64 } else { this.handler1632 };
		(decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_RM {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	reg: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	mem: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
}

impl OpCodeHandler_RM {
	pub(super) fn new(reg: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), mem: (OpCodeHandlerDecodeFn, &'static OpCodeHandler)) -> Self {
		debug_assert!(!is_null_instance_handler(reg.1));
		debug_assert!(!is_null_instance_handler(mem.1));
		Self { has_modrm: true, decode: OpCodeHandler_RM::decode, reg, mem }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		let (decode, handler) = if decoder.state.mod_ == 3 { this.reg } else { this.mem };
		(decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Options1632 {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	default_handler: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	infos: [(OpCodeHandlerDecodeFn, &'static OpCodeHandler, u32); 2],
	info_options: u32,
}

impl OpCodeHandler_Options1632 {
	#[allow(trivial_casts)]
	pub(super) fn new(
		default_handler: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), handler1: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), options1: u32,
	) -> Self {
		debug_assert!(!is_null_instance_handler(default_handler.1));
		debug_assert!(!is_null_instance_handler(handler1.1));
		Self {
			decode: OpCodeHandler_Options1632::decode,
			has_modrm: false,
			default_handler,
			infos: [(handler1.0, handler1.1, options1), (get_invalid_no_modrm_handler().0, get_invalid_no_modrm_handler().1, 0)],
			info_options: options1,
		}
	}

	pub(super) fn new2(
		default_handler: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), handler1: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), options1: u32,
		handler2: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), options2: u32,
	) -> Self {
		debug_assert!(!is_null_instance_handler(default_handler.1));
		debug_assert!(!is_null_instance_handler(handler1.1));
		debug_assert!(!is_null_instance_handler(handler2.1));
		Self {
			decode: OpCodeHandler_Options1632::decode,
			has_modrm: false,
			default_handler,
			infos: [(handler1.0, handler1.1, options1), (handler2.0, handler2.1, options2)],
			info_options: options1 | options2,
		}
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		let (mut decode, mut handler) = this.default_handler;
		let options = decoder.options;
		if !decoder.is64b_mode && (decoder.options & this.info_options) != 0 {
			for info in &this.infos {
				if (options & info.2) != 0 {
					decode = info.0;
					handler = info.1;
					break;
				}
			}
		}
		if handler.has_modrm {
			decoder.read_modrm();
		}
		(decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Options {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	default_handler: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	infos: [(OpCodeHandlerDecodeFn, &'static OpCodeHandler, u32); 2],
	info_options: u32,
}

impl OpCodeHandler_Options {
	#[allow(trivial_casts)]
	pub(super) fn new(
		default_handler: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), handler1: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), options1: u32,
	) -> Self {
		debug_assert!(!is_null_instance_handler(default_handler.1));
		debug_assert!(!is_null_instance_handler(handler1.1));
		Self {
			decode: OpCodeHandler_Options::decode,
			has_modrm: false,
			default_handler,
			infos: [(handler1.0, handler1.1, options1), (get_invalid_no_modrm_handler().0, get_invalid_no_modrm_handler().1, 0)],
			info_options: options1,
		}
	}

	pub(super) fn new2(
		default_handler: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), handler1: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), options1: u32,
		handler2: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), options2: u32,
	) -> Self {
		debug_assert!(!is_null_instance_handler(default_handler.1));
		debug_assert!(!is_null_instance_handler(handler1.1));
		debug_assert!(!is_null_instance_handler(handler2.1));
		Self {
			decode: OpCodeHandler_Options::decode,
			has_modrm: false,
			default_handler,
			infos: [(handler1.0, handler1.1, options1), (handler2.0, handler2.1, options2)],
			info_options: options1 | options2,
		}
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		let (mut decode, mut handler) = this.default_handler;
		let options = decoder.options;
		if (decoder.options & this.info_options) != 0 {
			for info in &this.infos {
				if (options & info.2) != 0 {
					decode = info.0;
					handler = info.1;
					break;
				}
			}
		}
		if handler.has_modrm {
			decoder.read_modrm();
		}
		(decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Options_DontReadModRM {
	has_modrm: bool,
	decode: OpCodeHandlerDecodeFn,
	default_handler: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	opt_handler: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	flags: u32,
}

impl OpCodeHandler_Options_DontReadModRM {
	pub(super) fn new(
		default_handler: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), opt_handler: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), flags: u32,
	) -> Self {
		debug_assert!(!is_null_instance_handler(default_handler.1));
		debug_assert!(!is_null_instance_handler(opt_handler.1));
		Self { decode: OpCodeHandler_Options_DontReadModRM::decode, has_modrm: true, default_handler, opt_handler, flags }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		let (mut decode, mut handler) = this.default_handler;
		let options = decoder.options;
		if (options & this.flags) != 0 {
			decode = this.opt_handler.0;
			handler = this.opt_handler.1;
		}
		(decode)(handler, decoder, instruction);
	}
}
