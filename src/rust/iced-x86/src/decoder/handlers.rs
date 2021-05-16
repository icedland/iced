// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::decoder::*;
use crate::instruction_internal;
use crate::*;
use alloc::boxed::Box;
use alloc::vec::Vec;

// SAFETY:
//	code: let this = unsafe { &*(self_ptr as *const Self) };
// The first arg (`self_ptr`) to decode() is always the handler itself, cast to a `*const OpCodeHandler`.
// All handlers are `#[repr(C)]` structs so the OpCodeHandler fields are always at the same offsets.

pub(super) type OpCodeHandlerDecodeFn = fn(*const OpCodeHandler, &mut Decoder<'_>, &mut Instruction);

#[allow(trivial_casts)]
#[must_use]
#[inline]
pub(super) fn is_null_instance_handler(handler: *const OpCodeHandler) -> bool {
	handler as *const u8 == &NULL_HANDLER as *const _ as *const u8
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
	pub(super) decode: OpCodeHandlerDecodeFn,
	pub(super) has_modrm: bool,
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Invalid {
	pub(super) decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
}

impl OpCodeHandler_Invalid {
	fn decode(_self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, _instruction: &mut Instruction) {
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

	fn decode(self_ptr: *const OpCodeHandler, _decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		instruction_internal::internal_set_code_u32(instruction, this.code);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Group8x8 {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	table_low: Vec<(&'static OpCodeHandler, OpCodeHandlerDecodeFn)>,
	table_high: Vec<(&'static OpCodeHandler, OpCodeHandlerDecodeFn)>,
}

impl OpCodeHandler_Group8x8 {
	pub(super) fn new(
		table_low: Vec<(&'static OpCodeHandler, OpCodeHandlerDecodeFn)>, table_high: Vec<(&'static OpCodeHandler, OpCodeHandlerDecodeFn)>,
	) -> Self {
		debug_assert_eq!(table_low.len(), 8);
		debug_assert_eq!(table_high.len(), 8);
		Self { decode: OpCodeHandler_Group8x8::decode, has_modrm: true, table_low, table_high }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		let (handler, decode) = if decoder.state.mod_ == 3 {
			unsafe { *this.table_high.get_unchecked(decoder.state.reg as usize) }
		} else {
			unsafe { *this.table_low.get_unchecked(decoder.state.reg as usize) }
		};
		(decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Group8x64 {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	table_low: Vec<(&'static OpCodeHandler, OpCodeHandlerDecodeFn)>,
	table_high: Vec<(&'static OpCodeHandler, OpCodeHandlerDecodeFn)>,
}

impl OpCodeHandler_Group8x64 {
	pub(super) fn new(
		table_low: Vec<(&'static OpCodeHandler, OpCodeHandlerDecodeFn)>, table_high: Vec<(&'static OpCodeHandler, OpCodeHandlerDecodeFn)>,
	) -> Self {
		debug_assert_eq!(table_low.len(), 8);
		debug_assert_eq!(table_high.len(), 0x40);
		Self { decode: OpCodeHandler_Group8x64::decode, has_modrm: true, table_low, table_high }
	}

	#[allow(trivial_casts)]
	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		let (handler, decode) = if decoder.state.mod_ == 3 {
			let (handler, decode) = unsafe { *this.table_high.get_unchecked((decoder.state.modrm & 0x3F) as usize) };
			if handler as *const _ as *const u8 == &NULL_HANDLER as *const _ as *const u8 {
				unsafe { *this.table_low.get_unchecked(decoder.state.reg as usize) }
			} else {
				(handler, decode)
			}
		} else {
			unsafe { *this.table_low.get_unchecked(decoder.state.reg as usize) }
		};
		(decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Group {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	group_handlers: Vec<(&'static OpCodeHandler, OpCodeHandlerDecodeFn)>,
}

impl OpCodeHandler_Group {
	pub(super) fn new(group_handlers: Vec<(&'static OpCodeHandler, OpCodeHandlerDecodeFn)>) -> Self {
		debug_assert_eq!(group_handlers.len(), 8);
		Self { decode: OpCodeHandler_Group::decode, has_modrm: true, group_handlers }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		let (handler, decode) = unsafe { *this.group_handlers.get_unchecked(decoder.state.reg as usize) };
		(decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_AnotherTable {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	handlers: Box<[(&'static OpCodeHandler, OpCodeHandlerDecodeFn); 0x100]>,
}

impl OpCodeHandler_AnotherTable {
	#[allow(clippy::unwrap_used)]
	pub(super) fn new(handlers: Vec<(&'static OpCodeHandler, OpCodeHandlerDecodeFn)>) -> Self {
		let handlers = handlers.into_boxed_slice();
		debug_assert_eq!(handlers.len(), 0x100);

		// SAFETY: handlers size is verified to be 0x100
		let handlers = unsafe { Box::from_raw(Box::into_raw(handlers) as *mut [_; 0x100]) };

		Self { decode: OpCodeHandler_AnotherTable::decode, has_modrm: false, handlers }
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
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	handlers: [(&'static OpCodeHandler, OpCodeHandlerDecodeFn); 4],
}

#[cfg(any(not(feature = "no_vex"), not(feature = "no_xop"), not(feature = "no_evex")))]
impl OpCodeHandler_MandatoryPrefix2 {
	pub(super) fn new(
		has_modrm: bool, handler: (*const OpCodeHandler, OpCodeHandlerDecodeFn), handler_66: (*const OpCodeHandler, OpCodeHandlerDecodeFn),
		handler_f3: (*const OpCodeHandler, OpCodeHandlerDecodeFn), handler_f2: (*const OpCodeHandler, OpCodeHandlerDecodeFn),
	) -> Self {
		const_assert_eq!(MandatoryPrefixByte::None as u32, 0);
		const_assert_eq!(MandatoryPrefixByte::P66 as u32, 1);
		const_assert_eq!(MandatoryPrefixByte::PF3 as u32, 2);
		const_assert_eq!(MandatoryPrefixByte::PF2 as u32, 3);
		debug_assert!(!is_null_instance_handler(handler.0));
		debug_assert!(!is_null_instance_handler(handler_66.0));
		debug_assert!(!is_null_instance_handler(handler_f3.0));
		debug_assert!(!is_null_instance_handler(handler_f2.0));
		let handlers =
			unsafe { [(&*handler.0, handler.1), (&*handler_66.0, handler_66.1), (&*handler_f3.0, handler_f3.1), (&*handler_f2.0, handler_f2.1)] };
		debug_assert_eq!(handlers[0].0.has_modrm, has_modrm);
		debug_assert_eq!(handlers[1].0.has_modrm, has_modrm);
		debug_assert_eq!(handlers[2].0.has_modrm, has_modrm);
		debug_assert_eq!(handlers[3].0.has_modrm, has_modrm);
		Self { decode: OpCodeHandler_MandatoryPrefix2::decode, has_modrm, handlers }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(
			decoder.state.encoding() == EncodingKind::VEX
				|| decoder.state.encoding() == EncodingKind::EVEX
				|| decoder.state.encoding() == EncodingKind::XOP
		);
		let (handler, decode) = unsafe { *this.handlers.get_unchecked(decoder.state.mandatory_prefix as usize) };
		(decode)(handler, decoder, instruction);
	}
}

#[cfg(any(not(feature = "no_vex"), not(feature = "no_xop"), not(feature = "no_evex")))]
#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_W {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	handlers: [(&'static OpCodeHandler, OpCodeHandlerDecodeFn); 2],
}

#[cfg(any(not(feature = "no_vex"), not(feature = "no_xop"), not(feature = "no_evex")))]
impl OpCodeHandler_W {
	pub(super) fn new(handler_w0: (*const OpCodeHandler, OpCodeHandlerDecodeFn), handler_w1: (*const OpCodeHandler, OpCodeHandlerDecodeFn)) -> Self {
		debug_assert!(!is_null_instance_handler(handler_w0.0));
		debug_assert!(!is_null_instance_handler(handler_w1.0));
		Self {
			decode: OpCodeHandler_W::decode,
			has_modrm: true,
			handlers: unsafe { [(&*handler_w0.0, handler_w0.1), (&*handler_w1.0, handler_w1.1)] },
		}
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
		let (handler, decode) = unsafe { *this.handlers.get_unchecked(index as usize) };
		(decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Bitness {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	handler1632: (&'static OpCodeHandler, OpCodeHandlerDecodeFn),
	handler64: (&'static OpCodeHandler, OpCodeHandlerDecodeFn),
}

impl OpCodeHandler_Bitness {
	pub(super) fn new(handler1632: (*const OpCodeHandler, OpCodeHandlerDecodeFn), handler64: (*const OpCodeHandler, OpCodeHandlerDecodeFn)) -> Self {
		debug_assert!(!is_null_instance_handler(handler1632.0));
		debug_assert!(!is_null_instance_handler(handler64.0));
		Self {
			decode: OpCodeHandler_Bitness::decode,
			has_modrm: false,
			handler1632: (unsafe { &*handler1632.0 }, handler1632.1),
			handler64: (unsafe { &*handler64.0 }, handler64.1),
		}
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		let (handler, decode) = if decoder.is64b_mode { this.handler64 } else { this.handler1632 };
		if handler.has_modrm {
			decoder.read_modrm();
		}
		(decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Bitness_DontReadModRM {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	handler1632: (&'static OpCodeHandler, OpCodeHandlerDecodeFn),
	handler64: (&'static OpCodeHandler, OpCodeHandlerDecodeFn),
}

impl OpCodeHandler_Bitness_DontReadModRM {
	pub(super) fn new(handler1632: (*const OpCodeHandler, OpCodeHandlerDecodeFn), handler64: (*const OpCodeHandler, OpCodeHandlerDecodeFn)) -> Self {
		debug_assert!(!is_null_instance_handler(handler1632.0));
		debug_assert!(!is_null_instance_handler(handler64.0));
		Self {
			decode: OpCodeHandler_Bitness_DontReadModRM::decode,
			has_modrm: true,
			handler1632: (unsafe { &*handler1632.0 }, handler1632.1),
			handler64: (unsafe { &*handler64.0 }, handler64.1),
		}
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		let (handler, decode) = if decoder.is64b_mode { this.handler64 } else { this.handler1632 };
		(decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_RM {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	reg: (&'static OpCodeHandler, OpCodeHandlerDecodeFn),
	mem: (&'static OpCodeHandler, OpCodeHandlerDecodeFn),
}

impl OpCodeHandler_RM {
	pub(super) fn new(reg: (*const OpCodeHandler, OpCodeHandlerDecodeFn), mem: (*const OpCodeHandler, OpCodeHandlerDecodeFn)) -> Self {
		debug_assert!(!is_null_instance_handler(reg.0));
		debug_assert!(!is_null_instance_handler(mem.0));
		Self { decode: OpCodeHandler_RM::decode, has_modrm: true, reg: (unsafe { &*reg.0 }, reg.1), mem: (unsafe { &*mem.0 }, mem.1) }
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		let (handler, decode) = if decoder.state.mod_ == 3 { this.reg } else { this.mem };
		(decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Options1632 {
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	default_handler: (&'static OpCodeHandler, OpCodeHandlerDecodeFn),
	infos: [(&'static OpCodeHandler, OpCodeHandlerDecodeFn, u32); 2],
	info_options: u32,
}

impl OpCodeHandler_Options1632 {
	#[allow(trivial_casts)]
	pub(super) fn new(
		default_handler: (*const OpCodeHandler, OpCodeHandlerDecodeFn), handler1: (*const OpCodeHandler, OpCodeHandlerDecodeFn), options1: u32,
	) -> Self {
		debug_assert!(!is_null_instance_handler(default_handler.0));
		debug_assert!(!is_null_instance_handler(handler1.0));
		Self {
			decode: OpCodeHandler_Options1632::decode,
			has_modrm: false,
			default_handler: (unsafe { &*default_handler.0 }, default_handler.1),
			infos: [
				(unsafe { &*handler1.0 }, handler1.1, options1),
				(unsafe { &*(&INVALID_NO_MODRM_HANDLER as *const _ as *const OpCodeHandler) }, INVALID_NO_MODRM_HANDLER.decode, 0),
			],
			info_options: options1,
		}
	}

	pub(super) fn new2(
		default_handler: (*const OpCodeHandler, OpCodeHandlerDecodeFn), handler1: (*const OpCodeHandler, OpCodeHandlerDecodeFn), options1: u32,
		handler2: (*const OpCodeHandler, OpCodeHandlerDecodeFn), options2: u32,
	) -> Self {
		debug_assert!(!is_null_instance_handler(default_handler.0));
		debug_assert!(!is_null_instance_handler(handler1.0));
		debug_assert!(!is_null_instance_handler(handler2.0));
		Self {
			decode: OpCodeHandler_Options1632::decode,
			has_modrm: false,
			default_handler: (unsafe { &*default_handler.0 }, default_handler.1),
			infos: [(unsafe { &*handler1.0 }, handler1.1, options1), (unsafe { &*handler2.0 }, handler2.1, options2)],
			info_options: options1 | options2,
		}
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		let (mut handler, mut decode) = this.default_handler;
		let options = decoder.options;
		if !decoder.is64b_mode && (decoder.options & this.info_options) != 0 {
			for info in &this.infos {
				if (options & info.2) != 0 {
					handler = info.0;
					decode = info.1;
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
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	default_handler: (&'static OpCodeHandler, OpCodeHandlerDecodeFn),
	infos: [(&'static OpCodeHandler, OpCodeHandlerDecodeFn, u32); 2],
	info_options: u32,
}

impl OpCodeHandler_Options {
	#[allow(trivial_casts)]
	pub(super) fn new(
		default_handler: (*const OpCodeHandler, OpCodeHandlerDecodeFn), handler1: (*const OpCodeHandler, OpCodeHandlerDecodeFn), options1: u32,
	) -> Self {
		debug_assert!(!is_null_instance_handler(default_handler.0));
		debug_assert!(!is_null_instance_handler(handler1.0));
		Self {
			decode: OpCodeHandler_Options::decode,
			has_modrm: false,
			default_handler: (unsafe { &*default_handler.0 }, default_handler.1),
			infos: [
				(unsafe { &*handler1.0 }, handler1.1, options1),
				(unsafe { &*(&INVALID_NO_MODRM_HANDLER as *const _ as *const OpCodeHandler) }, INVALID_NO_MODRM_HANDLER.decode, 0),
			],
			info_options: options1,
		}
	}

	pub(super) fn new2(
		default_handler: (*const OpCodeHandler, OpCodeHandlerDecodeFn), handler1: (*const OpCodeHandler, OpCodeHandlerDecodeFn), options1: u32,
		handler2: (*const OpCodeHandler, OpCodeHandlerDecodeFn), options2: u32,
	) -> Self {
		debug_assert!(!is_null_instance_handler(default_handler.0));
		debug_assert!(!is_null_instance_handler(handler1.0));
		debug_assert!(!is_null_instance_handler(handler2.0));
		Self {
			decode: OpCodeHandler_Options::decode,
			has_modrm: false,
			default_handler: (unsafe { &*default_handler.0 }, default_handler.1),
			infos: [(unsafe { &*handler1.0 }, handler1.1, options1), (unsafe { &*handler2.0 }, handler2.1, options2)],
			info_options: options1 | options2,
		}
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		let (mut handler, mut decode) = this.default_handler;
		let options = decoder.options;
		if (decoder.options & this.info_options) != 0 {
			for info in &this.infos {
				if (options & info.2) != 0 {
					handler = info.0;
					decode = info.1;
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
	decode: OpCodeHandlerDecodeFn,
	has_modrm: bool,
	default_handler: (&'static OpCodeHandler, OpCodeHandlerDecodeFn),
	opt_handler: (&'static OpCodeHandler, OpCodeHandlerDecodeFn),
	flags: u32,
}

impl OpCodeHandler_Options_DontReadModRM {
	pub(super) fn new(
		default_handler: (*const OpCodeHandler, OpCodeHandlerDecodeFn), opt_handler: (*const OpCodeHandler, OpCodeHandlerDecodeFn), flags: u32,
	) -> Self {
		debug_assert!(!is_null_instance_handler(default_handler.0));
		debug_assert!(!is_null_instance_handler(opt_handler.0));
		Self {
			decode: OpCodeHandler_Options_DontReadModRM::decode,
			has_modrm: true,
			default_handler: (unsafe { &*default_handler.0 }, default_handler.1),
			opt_handler: (unsafe { &*opt_handler.0 }, opt_handler.1),
			flags,
		}
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		let (mut handler, mut decode) = this.default_handler;
		let options = decoder.options;
		if (options & this.flags) != 0 {
			decode = this.opt_handler.1;
			handler = this.opt_handler.0;
		}
		(decode)(handler, decoder, instruction);
	}
}
