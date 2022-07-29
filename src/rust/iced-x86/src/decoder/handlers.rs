// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

macro_rules! write_op0_reg {
	($instruction:ident, $expr:expr) => {
		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op0_kind(OpKind::Register);
		debug_assert!($expr < IcedConstants::REGISTER_ENUM_COUNT as u32);
		$instruction.set_op0_register(unsafe { mem::transmute($expr as RegisterUnderlyingType) });
	};
}

macro_rules! write_op1_reg {
	($instruction:ident, $expr:expr) => {
		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op1_kind(OpKind::Register);
		debug_assert!($expr < IcedConstants::REGISTER_ENUM_COUNT as u32);
		$instruction.set_op1_register(unsafe { mem::transmute($expr as RegisterUnderlyingType) });
	};
}

macro_rules! write_op2_reg {
	($instruction:ident, $expr:expr) => {
		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op2_kind(OpKind::Register);
		debug_assert!($expr < IcedConstants::REGISTER_ENUM_COUNT as u32);
		$instruction.set_op2_register(unsafe { mem::transmute($expr as RegisterUnderlyingType) });
	};
}

#[cfg(any(not(feature = "no_vex"), not(feature = "no_xop")))]
macro_rules! write_op3_reg {
	($instruction:ident, $expr:expr) => {
		const _: () = assert!(OpKind::Register as u32 == 0);
		//instruction.set_op3_kind(OpKind::Register);
		debug_assert!($expr < IcedConstants::REGISTER_ENUM_COUNT as u32);
		$instruction.set_op3_register(unsafe { mem::transmute($expr as RegisterUnderlyingType) });
	};
}

pub(super) mod d3now;
#[cfg(not(feature = "no_evex"))]
pub(super) mod evex;
pub(super) mod fpu;
pub(super) mod legacy;
#[cfg(feature = "mvex")]
pub(super) mod mvex;
pub(super) mod tables;
#[cfg(any(not(feature = "no_vex"), not(feature = "no_xop")))]
pub(super) mod vex;

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
	(OpCodeHandler_Invalid::decode, unsafe { &*(&NULL_HANDLER as *const _ as *const OpCodeHandler) })
}

#[allow(trivial_casts)]
#[must_use]
#[inline]
pub(super) fn get_invalid_handler() -> (OpCodeHandlerDecodeFn, &'static OpCodeHandler) {
	// SAFETY: it's #[repr(C)] and the first part is an `OpCodeHandler`
	(OpCodeHandler_Invalid::decode, unsafe { &*(&INVALID_HANDLER as *const _ as *const OpCodeHandler) })
}

#[allow(trivial_casts)]
#[must_use]
#[inline]
pub(super) fn get_invalid_no_modrm_handler() -> (OpCodeHandlerDecodeFn, &'static OpCodeHandler) {
	// SAFETY: it's #[repr(C)] and the first part is an `OpCodeHandler`
	(OpCodeHandler_Invalid::decode, unsafe { &*(&INVALID_NO_MODRM_HANDLER as *const _ as *const OpCodeHandler) })
}

#[rustfmt::skip]
pub(super) static NULL_HANDLER: OpCodeHandler_Invalid = OpCodeHandler_Invalid {
	has_modrm: true,
};
#[rustfmt::skip]
pub(super) static INVALID_HANDLER: OpCodeHandler_Invalid = OpCodeHandler_Invalid {
	has_modrm: true,
};
#[rustfmt::skip]
pub(super) static INVALID_NO_MODRM_HANDLER: OpCodeHandler_Invalid = OpCodeHandler_Invalid {
	has_modrm: false,
};

#[repr(C)]
pub(super) struct OpCodeHandler {
	pub(super) has_modrm: bool,
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Invalid {
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
	has_modrm: bool,
	code: Code,
}

impl OpCodeHandler_Simple {
	#[inline]
	pub(super) fn new(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Simple::decode, Self { has_modrm: false, code })
	}

	#[inline]
	pub(super) fn new_modrm(code: Code) -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Simple::decode, Self { has_modrm: true, code })
	}

	fn decode(self_ptr: *const OpCodeHandler, _decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		instruction.set_code(this.code);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Int3 {
	has_modrm: bool,
}

impl OpCodeHandler_Int3 {
	#[inline]
	pub(super) fn new() -> (OpCodeHandlerDecodeFn, Self) {
		(OpCodeHandler_Int3::decode, Self { has_modrm: false })
	}

	fn decode(_self_ptr: *const OpCodeHandler, _decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		instruction.set_code(Code::Int3);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Group8x8 {
	has_modrm: bool,
	table_low: Box<[(OpCodeHandlerDecodeFn, &'static OpCodeHandler); 8]>,
	table_high: Box<[(OpCodeHandlerDecodeFn, &'static OpCodeHandler); 8]>,
}

impl OpCodeHandler_Group8x8 {
	#[inline]
	#[allow(clippy::unwrap_used)]
	pub(super) fn new(
		table_low: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>, table_high: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	) -> (OpCodeHandlerDecodeFn, Self) {
		let table_low = table_low.into_boxed_slice().try_into().ok().unwrap();
		let table_high = table_high.into_boxed_slice().try_into().ok().unwrap();
		(OpCodeHandler_Group8x8::decode, Self { has_modrm: true, table_low, table_high })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.reg <= 7);
		let (decode, handler) = if decoder.state.mod_ == 3 {
			// SAFETY: reg <= 7 and table_high.len() == 8 (see field type)
			unsafe { *this.table_high.get_unchecked(decoder.state.reg as usize) }
		} else {
			// SAFETY: reg <= 7 and table_low.len() == 8 (see field type)
			unsafe { *this.table_low.get_unchecked(decoder.state.reg as usize) }
		};
		(decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Group8x64 {
	has_modrm: bool,
	table_low: Box<[(OpCodeHandlerDecodeFn, &'static OpCodeHandler); 8]>,
	table_high: Box<[(OpCodeHandlerDecodeFn, &'static OpCodeHandler); 0x40]>,
}

impl OpCodeHandler_Group8x64 {
	#[inline]
	#[allow(clippy::unwrap_used)]
	pub(super) fn new(
		table_low: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>, table_high: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	) -> (OpCodeHandlerDecodeFn, Self) {
		let table_low = table_low.into_boxed_slice().try_into().ok().unwrap();
		let table_high = table_high.into_boxed_slice().try_into().ok().unwrap();
		(OpCodeHandler_Group8x64::decode, Self { has_modrm: true, table_low, table_high })
	}

	#[allow(trivial_casts)]
	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		let (decode, handler) = if decoder.state.mod_ == 3 {
			// SAFETY: table_high.len() == 0x40 (see field type) and index <= 0x3F due to masking `modrm`
			let (decode, handler) = unsafe { *this.table_high.get_unchecked((decoder.state.modrm & 0x3F) as usize) };
			if handler as *const _ as *const u8 == &NULL_HANDLER as *const _ as *const u8 {
				debug_assert!(decoder.state.reg <= 7);
				// SAFETY: reg <= 7 and table_low.len() == 8 (see field type)
				unsafe { *this.table_low.get_unchecked(decoder.state.reg as usize) }
			} else {
				(decode, handler)
			}
		} else {
			debug_assert!(decoder.state.reg <= 7);
			// SAFETY: reg <= 7 and table_low.len() == 8 (see field type)
			unsafe { *this.table_low.get_unchecked(decoder.state.reg as usize) }
		};
		(decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Group {
	has_modrm: bool,
	group_handlers: Box<[(OpCodeHandlerDecodeFn, &'static OpCodeHandler); 8]>,
}

impl OpCodeHandler_Group {
	#[inline]
	#[allow(clippy::unwrap_used)]
	pub(super) fn new(group_handlers: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>) -> (OpCodeHandlerDecodeFn, Self) {
		let group_handlers = group_handlers.into_boxed_slice().try_into().ok().unwrap();
		(OpCodeHandler_Group::decode, Self { has_modrm: true, group_handlers })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(decoder.state.reg <= 7);
		// SAFETY: group_handlers.len() == 8 (see field type) and reg <= 7
		let (decode, handler) = unsafe { *this.group_handlers.get_unchecked(decoder.state.reg as usize) };
		(decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_AnotherTable {
	has_modrm: bool,
	handlers: Box<[(OpCodeHandlerDecodeFn, &'static OpCodeHandler); 0x100]>,
}

impl OpCodeHandler_AnotherTable {
	#[allow(clippy::unwrap_used)]
	#[inline]
	pub(super) fn new(handlers: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>) -> (OpCodeHandlerDecodeFn, Self) {
		let handlers = handlers.into_boxed_slice().try_into().ok().unwrap();
		(OpCodeHandler_AnotherTable::decode, Self { has_modrm: false, handlers })
	}

	#[allow(clippy::never_loop)]
	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		loop {
			let b = read_u8_break!(decoder);
			let (decode, handler) = this.handlers[b];
			if handler.has_modrm {
				let m = read_u8_break!(decoder) as u32;
				decoder.state.modrm = m;
				decoder.state.reg = (m >> 3) & 7;
				decoder.state.mod_ = m >> 6;
				decoder.state.rm = m & 7;
				decoder.state.mem_index = (decoder.state.mod_ << 3) | decoder.state.rm;
			}
			(decode)(handler, decoder, instruction);
			return;
		}
		decoder.state.flags |= StateFlags::IS_INVALID | StateFlags::NO_MORE_BYTES;
	}
}

#[cfg(any(not(feature = "no_vex"), not(feature = "no_xop"), not(feature = "no_evex"), feature = "mvex"))]
#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_MandatoryPrefix2 {
	has_modrm: bool,
	handlers: [(OpCodeHandlerDecodeFn, &'static OpCodeHandler); 4],
}

#[cfg(any(not(feature = "no_vex"), not(feature = "no_xop"), not(feature = "no_evex"), feature = "mvex"))]
impl OpCodeHandler_MandatoryPrefix2 {
	#[inline]
	pub(super) fn new(
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
		(OpCodeHandler_MandatoryPrefix2::decode, Self { has_modrm, handlers })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(
			decoder.state.encoding() == EncodingKind::VEX as u32
				|| decoder.state.encoding() == EncodingKind::EVEX as u32
				|| decoder.state.encoding() == EncodingKind::XOP as u32
				|| decoder.state.encoding() == EncodingKind::MVEX as u32
		);
		let (decode, handler) = this.handlers[decoder.state.mandatory_prefix as usize];
		(decode)(handler, decoder, instruction);
	}
}

#[cfg(any(not(feature = "no_vex"), not(feature = "no_xop"), not(feature = "no_evex"), feature = "mvex"))]
#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_W {
	has_modrm: bool,
	handlers: [(OpCodeHandlerDecodeFn, &'static OpCodeHandler); 2],
}

#[cfg(any(not(feature = "no_vex"), not(feature = "no_xop"), not(feature = "no_evex"), feature = "mvex"))]
impl OpCodeHandler_W {
	#[inline]
	pub(super) fn new(
		handler_w0: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), handler_w1: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	) -> (OpCodeHandlerDecodeFn, Self) {
		debug_assert!(!is_null_instance_handler(handler_w0.1));
		debug_assert!(!is_null_instance_handler(handler_w1.1));
		(OpCodeHandler_W::decode, Self { has_modrm: true, handlers: [handler_w0, handler_w1] })
	}

	fn decode(self_ptr: *const OpCodeHandler, decoder: &mut Decoder<'_>, instruction: &mut Instruction) {
		let this = unsafe { &*(self_ptr as *const Self) };
		debug_assert!(
			decoder.state.encoding() == EncodingKind::VEX as u32
				|| decoder.state.encoding() == EncodingKind::EVEX as u32
				|| decoder.state.encoding() == EncodingKind::XOP as u32
				|| decoder.state.encoding() == EncodingKind::MVEX as u32
		);
		let (decode, handler) = this.handlers[((decoder.state.flags & StateFlags::W) != 0) as usize];
		(decode)(handler, decoder, instruction);
	}
}

#[allow(non_camel_case_types)]
#[repr(C)]
pub(super) struct OpCodeHandler_Bitness {
	has_modrm: bool,
	handler1632: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	handler64: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
}

impl OpCodeHandler_Bitness {
	#[inline]
	pub(super) fn new(
		handler1632: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), handler64: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	) -> (OpCodeHandlerDecodeFn, Self) {
		debug_assert!(!is_null_instance_handler(handler1632.1));
		debug_assert!(!is_null_instance_handler(handler64.1));
		(OpCodeHandler_Bitness::decode, Self { has_modrm: false, handler1632, handler64 })
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
	handler1632: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	handler64: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
}

impl OpCodeHandler_Bitness_DontReadModRM {
	#[inline]
	pub(super) fn new(
		handler1632: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), handler64: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	) -> (OpCodeHandlerDecodeFn, Self) {
		debug_assert!(!is_null_instance_handler(handler1632.1));
		debug_assert!(!is_null_instance_handler(handler64.1));
		(OpCodeHandler_Bitness_DontReadModRM::decode, Self { has_modrm: true, handler1632, handler64 })
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
	reg: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	mem: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
}

impl OpCodeHandler_RM {
	#[inline]
	pub(super) fn new(
		reg: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), mem: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	) -> (OpCodeHandlerDecodeFn, Self) {
		debug_assert!(!is_null_instance_handler(reg.1));
		debug_assert!(!is_null_instance_handler(mem.1));
		(OpCodeHandler_RM::decode, Self { has_modrm: true, reg, mem })
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
	default_handler: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	infos: [(OpCodeHandlerDecodeFn, &'static OpCodeHandler, u32); 2],
	info_options: u32,
}

impl OpCodeHandler_Options1632 {
	#[allow(trivial_casts)]
	#[inline]
	pub(super) fn new(
		default_handler: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), handler1: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), options1: u32,
	) -> (OpCodeHandlerDecodeFn, Self) {
		debug_assert!(!is_null_instance_handler(default_handler.1));
		debug_assert!(!is_null_instance_handler(handler1.1));
		(
			OpCodeHandler_Options1632::decode,
			Self {
				has_modrm: false,
				default_handler,
				infos: [(handler1.0, handler1.1, options1), (get_invalid_no_modrm_handler().0, get_invalid_no_modrm_handler().1, 0)],
				info_options: options1,
			},
		)
	}

	#[inline]
	pub(super) fn new2(
		default_handler: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), handler1: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), options1: u32,
		handler2: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), options2: u32,
	) -> (OpCodeHandlerDecodeFn, Self) {
		debug_assert!(!is_null_instance_handler(default_handler.1));
		debug_assert!(!is_null_instance_handler(handler1.1));
		debug_assert!(!is_null_instance_handler(handler2.1));
		(
			OpCodeHandler_Options1632::decode,
			Self {
				has_modrm: false,
				default_handler,
				infos: [(handler1.0, handler1.1, options1), (handler2.0, handler2.1, options2)],
				info_options: options1 | options2,
			},
		)
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
	default_handler: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	infos: [(OpCodeHandlerDecodeFn, &'static OpCodeHandler, u32); 2],
	info_options: u32,
}

impl OpCodeHandler_Options {
	#[allow(trivial_casts)]
	#[inline]
	pub(super) fn new(
		default_handler: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), handler1: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), options1: u32,
	) -> (OpCodeHandlerDecodeFn, Self) {
		debug_assert!(!is_null_instance_handler(default_handler.1));
		debug_assert!(!is_null_instance_handler(handler1.1));
		(
			OpCodeHandler_Options::decode,
			Self {
				has_modrm: false,
				default_handler,
				infos: [(handler1.0, handler1.1, options1), (get_invalid_no_modrm_handler().0, get_invalid_no_modrm_handler().1, 0)],
				info_options: options1,
			},
		)
	}

	#[inline]
	pub(super) fn new2(
		default_handler: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), handler1: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), options1: u32,
		handler2: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), options2: u32,
	) -> (OpCodeHandlerDecodeFn, Self) {
		debug_assert!(!is_null_instance_handler(default_handler.1));
		debug_assert!(!is_null_instance_handler(handler1.1));
		debug_assert!(!is_null_instance_handler(handler2.1));
		(
			OpCodeHandler_Options::decode,
			Self {
				has_modrm: false,
				default_handler,
				infos: [(handler1.0, handler1.1, options1), (handler2.0, handler2.1, options2)],
				info_options: options1 | options2,
			},
		)
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
	default_handler: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	opt_handler: (OpCodeHandlerDecodeFn, &'static OpCodeHandler),
	flags: u32,
}

impl OpCodeHandler_Options_DontReadModRM {
	#[inline]
	pub(super) fn new(
		default_handler: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), opt_handler: (OpCodeHandlerDecodeFn, &'static OpCodeHandler), flags: u32,
	) -> (OpCodeHandlerDecodeFn, Self) {
		debug_assert!(!is_null_instance_handler(default_handler.1));
		debug_assert!(!is_null_instance_handler(opt_handler.1));
		(OpCodeHandler_Options_DontReadModRM::decode, Self { has_modrm: true, default_handler, opt_handler, flags })
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
