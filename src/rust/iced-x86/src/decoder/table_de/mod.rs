// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#[cfg(not(feature = "no_evex"))]
mod data_evex;
mod data_legacy;
#[cfg(feature = "mvex")]
mod data_mvex;
#[cfg(not(feature = "no_vex"))]
mod data_vex;
#[cfg(not(feature = "no_xop"))]
mod data_xop;
mod enums;
#[cfg(not(feature = "no_evex"))]
mod evex_reader;
mod legacy_reader;
#[cfg(feature = "mvex")]
mod mvex_reader;
#[cfg(any(not(feature = "no_vex"), not(feature = "no_xop")))]
mod vex_reader;

use crate::data_reader::DataReader;
use crate::decoder::handlers::OpCodeHandler;
use crate::decoder::handlers::{is_null_instance_handler, OpCodeHandlerDecodeFn};
use crate::decoder::table_de::enums::*;
use crate::iced_constants::IcedConstants;
use crate::{Code, CodeUnderlyingType, Register, RegisterUnderlyingType};
#[cfg(not(feature = "no_evex"))]
use crate::{TupleType, TupleTypeUnderlyingType};
use alloc::boxed::Box;
use alloc::vec::Vec;
use core::mem;

#[inline]
fn box_opcode_handler<T>((decode, handler): (OpCodeHandlerDecodeFn, T)) -> (OpCodeHandlerDecodeFn, *const OpCodeHandler) {
	// All handlers are #[repr(C)] and the first fields are the same as OpCodeHandler
	(decode, Box::into_raw(Box::new(handler)) as *const OpCodeHandler)
}

enum HandlerInfo {
	Handler((OpCodeHandlerDecodeFn, &'static OpCodeHandler)),
	Handlers(Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>),
}

struct TableDeserializer<'a> {
	reader: DataReader<'a>,
	handler_reader: fn(deserializer: &mut TableDeserializer<'_>, result: &mut Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>),
	id_to_handler: Vec<HandlerInfo>,
	temp_vecs: Vec<Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>>,
}

impl<'a> TableDeserializer<'a> {
	#[must_use]
	#[inline]
	fn new(
		data: &'a [u8], max_ids: usize,
		handler_reader: fn(deserializer: &mut TableDeserializer<'_>, result: &mut Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>),
	) -> Self {
		Self { reader: DataReader::new(data), handler_reader, id_to_handler: Vec::with_capacity(max_ids), temp_vecs: Vec::new() }
	}

	fn deserialize(&mut self) {
		while self.reader.can_read() {
			// SAFETY: generated (and also immutable) data is valid
			let kind: SerializedDataKind = unsafe { mem::transmute(self.reader.read_u8() as u8) };
			match kind {
				SerializedDataKind::HandlerReference => {
					let tmp = self.read_handler();
					self.id_to_handler.push(HandlerInfo::Handler(tmp));
				}

				SerializedDataKind::ArrayReference => {
					let size = self.reader.read_compressed_u32() as usize;
					let tmp = self.read_handlers(size);
					self.id_to_handler.push(HandlerInfo::Handlers(tmp));
				}
			}
		}
		debug_assert!(!self.reader.can_read());
	}

	#[must_use]
	#[inline]
	fn read_legacy_op_code_handler_kind(&mut self) -> LegacyOpCodeHandlerKind {
		// SAFETY: generated (and also immutable) data is valid
		unsafe { mem::transmute(self.reader.read_u8() as u8) }
	}

	#[cfg(any(not(feature = "no_vex"), not(feature = "no_xop")))]
	#[must_use]
	#[inline]
	fn read_vex_op_code_handler_kind(&mut self) -> VexOpCodeHandlerKind {
		// SAFETY: generated (and also immutable) data is valid
		unsafe { mem::transmute(self.reader.read_u8() as u8) }
	}

	#[cfg(not(feature = "no_evex"))]
	#[must_use]
	#[inline]
	fn read_evex_op_code_handler_kind(&mut self) -> EvexOpCodeHandlerKind {
		// SAFETY: generated (and also immutable) data is valid
		unsafe { mem::transmute(self.reader.read_u8() as u8) }
	}

	#[cfg(feature = "mvex")]
	#[must_use]
	#[inline]
	fn read_mvex_op_code_handler_kind(&mut self) -> MvexOpCodeHandlerKind {
		// SAFETY: generated (and also immutable) data is valid
		unsafe { mem::transmute(self.reader.read_u8() as u8) }
	}

	#[must_use]
	#[inline]
	fn read_code(&mut self) -> Code {
		let v = self.reader.read_compressed_u32();
		debug_assert!(v < IcedConstants::CODE_ENUM_COUNT as u32);
		// SAFETY: generated (and also immutable) data is valid
		unsafe { mem::transmute(v as CodeUnderlyingType) }
	}

	#[must_use]
	#[inline]
	fn read_code2(&mut self) -> (Code, Code) {
		let v = self.reader.read_compressed_u32();
		debug_assert!(v < IcedConstants::CODE_ENUM_COUNT as u32);
		debug_assert!(v + 1 < IcedConstants::CODE_ENUM_COUNT as u32);
		// SAFETY: generated (and also immutable) data is valid
		(unsafe { mem::transmute(v as CodeUnderlyingType) }, unsafe { mem::transmute((v + 1) as CodeUnderlyingType) })
	}

	#[must_use]
	#[inline]
	fn read_code3(&mut self) -> (Code, Code, Code) {
		let v = self.reader.read_compressed_u32();
		debug_assert!(v < IcedConstants::CODE_ENUM_COUNT as u32);
		debug_assert!(v + 2 < IcedConstants::CODE_ENUM_COUNT as u32);
		// SAFETY: generated (and also immutable) data is valid
		(unsafe { mem::transmute(v as CodeUnderlyingType) }, unsafe { mem::transmute((v + 1) as CodeUnderlyingType) }, unsafe {
			mem::transmute((v + 2) as CodeUnderlyingType)
		})
	}

	#[must_use]
	#[inline]
	fn read_register(&mut self) -> Register {
		let v = self.reader.read_u8();
		debug_assert!(v < IcedConstants::REGISTER_ENUM_COUNT);
		// SAFETY: generated (and also immutable) data is valid
		unsafe { mem::transmute(v as RegisterUnderlyingType) }
	}

	#[must_use]
	#[inline]
	fn read_decoder_options(&mut self) -> u32 {
		self.reader.read_compressed_u32()
	}

	#[must_use]
	#[inline]
	fn read_handler_flags(&mut self) -> u32 {
		self.reader.read_compressed_u32()
	}

	#[must_use]
	#[inline]
	fn read_legacy_handler_flags(&mut self) -> u32 {
		self.reader.read_compressed_u32()
	}

	#[cfg(not(feature = "no_evex"))]
	#[must_use]
	#[inline]
	fn read_tuple_type(&mut self) -> TupleType {
		let v = self.reader.read_u8();
		debug_assert!(v < IcedConstants::TUPLE_TYPE_ENUM_COUNT);
		// SAFETY: generated (and also immutable) data is valid
		unsafe { mem::transmute(v as TupleTypeUnderlyingType) }
	}

	#[must_use]
	#[inline]
	fn read_boolean(&mut self) -> bool {
		self.reader.read_u8() != 0
	}

	#[must_use]
	#[inline]
	fn read_u32(&mut self) -> u32 {
		self.reader.read_compressed_u32()
	}

	#[must_use]
	#[inline]
	fn read_handler(&mut self) -> (OpCodeHandlerDecodeFn, &'static OpCodeHandler) {
		let result = self.read_handler_or_null_instance();
		debug_assert!(!is_null_instance_handler(result.1));
		result
	}

	#[must_use]
	#[allow(clippy::unwrap_used)]
	fn read_handler_or_null_instance(&mut self) -> (OpCodeHandlerDecodeFn, &'static OpCodeHandler) {
		let mut tmp_vec = self.temp_vecs.pop().unwrap_or_else(|| Vec::with_capacity(1));
		debug_assert!(tmp_vec.is_empty());
		(self.handler_reader)(self, &mut tmp_vec);
		debug_assert_eq!(tmp_vec.len(), 1);
		let result = tmp_vec.pop().unwrap();
		debug_assert!(tmp_vec.is_empty());
		self.temp_vecs.push(tmp_vec);
		result
	}

	#[must_use]
	fn read_handlers(&mut self, count: usize) -> Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)> {
		let mut handlers: Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)> = Vec::with_capacity(count);
		let mut i = 0;
		while handlers.len() < count {
			let len = handlers.len();
			(self.handler_reader)(self, &mut handlers);
			debug_assert!(handlers.len() >= len);
			let size = handlers.len() - len;
			if size == 0 {
				break; // will panic
			}
			i += size;
			debug_assert_eq!(handlers.len(), i);
		}
		debug_assert_eq!(handlers.len(), count);
		debug_assert_eq!(count, i);
		handlers
	}

	#[must_use]
	#[allow(clippy::get_unwrap)]
	#[allow(clippy::unwrap_used)]
	fn read_handler_reference(&mut self) -> (OpCodeHandlerDecodeFn, &'static OpCodeHandler) {
		let index = self.reader.read_u8();
		if let &HandlerInfo::Handler(handler) = self.id_to_handler.get(index).unwrap() {
			handler
		} else {
			unreachable!();
		}
	}

	#[must_use]
	#[allow(clippy::get_unwrap)]
	#[allow(clippy::unwrap_used)]
	fn read_array_reference(&mut self, kind: u32) -> Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)> {
		let read_kind = self.reader.read_u8() as u32;
		debug_assert_eq!(read_kind, kind);
		let index = self.reader.read_u8();
		if let HandlerInfo::Handlers(handlers) = self.id_to_handler.get(index).unwrap() {
			// There are a few dupe refs, clone the whole thing
			handlers.to_vec()
		} else {
			unreachable!();
		}
	}

	#[must_use]
	fn read_array_reference_no_clone(&mut self, kind: u32) -> Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)> {
		let read_kind = self.reader.read_u8() as u32;
		debug_assert_eq!(read_kind, kind);
		let index = self.reader.read_u8();
		self.table(index)
	}

	#[must_use]
	#[allow(clippy::get_unwrap)]
	#[allow(clippy::unwrap_used)]
	fn table(&mut self, index: usize) -> Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)> {
		if let &mut HandlerInfo::Handlers(ref mut tmp) = self.id_to_handler.get_mut(index).unwrap() {
			let handlers = mem::take(tmp);
			debug_assert!(!handlers.is_empty());
			handlers
		} else {
			unreachable!();
		}
	}
}

#[must_use]
pub(super) fn read_legacy() -> Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)> {
	let handler_reader = self::legacy_reader::read_handlers;
	let mut deserializer = TableDeserializer::new(data_legacy::TBL_DATA, data_legacy::MAX_ID_NAMES, handler_reader);
	deserializer.deserialize();
	deserializer.table(data_legacy::HANDLERS_MAP0_INDEX)
}

#[cfg(not(feature = "no_evex"))]
#[must_use]
pub(super) fn read_evex() -> (
	Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
) {
	let handler_reader = self::evex_reader::read_handlers;
	let mut deserializer = TableDeserializer::new(data_evex::TBL_DATA, data_evex::MAX_ID_NAMES, handler_reader);
	deserializer.deserialize();
	(
		deserializer.table(data_evex::HANDLERS_0F_INDEX),
		deserializer.table(data_evex::HANDLERS_0F38_INDEX),
		deserializer.table(data_evex::HANDLERS_0F3A_INDEX),
		deserializer.table(data_evex::HANDLERS_MAP5_INDEX),
		deserializer.table(data_evex::HANDLERS_MAP6_INDEX),
	)
}

#[cfg(not(feature = "no_vex"))]
#[must_use]
#[allow(clippy::let_unit_value)]
pub(super) fn read_vex() -> (
	Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
) {
	let handler_reader = self::vex_reader::read_handlers;
	let mut deserializer = TableDeserializer::new(data_vex::TBL_DATA, data_vex::MAX_ID_NAMES, handler_reader);
	deserializer.deserialize();

	(
		deserializer.table(data_vex::HANDLERS_MAP0_INDEX),
		deserializer.table(data_vex::HANDLERS_0F_INDEX),
		deserializer.table(data_vex::HANDLERS_0F38_INDEX),
		deserializer.table(data_vex::HANDLERS_0F3A_INDEX),
	)
}

#[cfg(not(feature = "no_xop"))]
#[must_use]
pub(super) fn read_xop() -> (
	Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
) {
	let handler_reader = self::vex_reader::read_handlers;
	let mut deserializer = TableDeserializer::new(data_xop::TBL_DATA, data_xop::MAX_ID_NAMES, handler_reader);
	deserializer.deserialize();
	(
		deserializer.table(data_xop::HANDLERS_MAP8_INDEX),
		deserializer.table(data_xop::HANDLERS_MAP9_INDEX),
		deserializer.table(data_xop::HANDLERS_MAP10_INDEX),
	)
}

#[cfg(feature = "mvex")]
#[must_use]
pub(super) fn read_mvex() -> (
	Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
	Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>,
) {
	let handler_reader = self::mvex_reader::read_handlers;
	let mut deserializer = TableDeserializer::new(data_mvex::TBL_DATA, data_mvex::MAX_ID_NAMES, handler_reader);
	deserializer.deserialize();
	(
		deserializer.table(data_mvex::HANDLERS_0F_INDEX),
		deserializer.table(data_mvex::HANDLERS_0F38_INDEX),
		deserializer.table(data_mvex::HANDLERS_0F3A_INDEX),
	)
}
