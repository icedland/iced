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

#[cfg(not(feature = "no_evex"))]
mod data_evex;
mod data_legacy;
#[cfg(not(feature = "no_vex"))]
mod data_vex;
#[cfg(not(feature = "no_xop"))]
mod data_xop;
mod enums;
#[cfg(not(feature = "no_evex"))]
mod evex_reader;
mod legacy_reader;
#[cfg(any(not(feature = "no_vex"), not(feature = "no_xop")))]
mod vex_reader;

use self::enums::*;
use super::super::data_reader::DataReader;
use super::super::Register;
#[cfg(not(feature = "no_evex"))]
use super::super::TupleType;
use super::handlers::is_null_instance_handler;
use super::handlers::OpCodeHandler;
#[cfg(not(feature = "std"))]
use alloc::vec::Vec;
use core::mem;

enum HandlerInfo {
	Handler(*const OpCodeHandler),
	Handlers(Vec<&'static OpCodeHandler>),
}

struct TableDeserializer<'a> {
	reader: DataReader<'a>,
	handler_reader: fn(deserializer: &mut TableDeserializer, result: &mut Vec<&'static OpCodeHandler>),
	id_to_handler: Vec<HandlerInfo>,
	temp_vecs: Vec<Vec<&'static OpCodeHandler>>,
}

impl<'a> TableDeserializer<'a> {
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub(self) fn new(
		data: &'a [u8], max_ids: usize, handler_reader: fn(deserializer: &mut TableDeserializer, result: &mut Vec<&'static OpCodeHandler>),
	) -> Self {
		Self { reader: DataReader::new(data), handler_reader, id_to_handler: Vec::with_capacity(max_ids), temp_vecs: Vec::new() }
	}

	pub(self) fn deserialize(&mut self) {
		while self.reader.can_read() {
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
		assert!(!self.reader.can_read());
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub(self) fn read_op_code_handler_kind(&mut self) -> OpCodeHandlerKind {
		unsafe { mem::transmute(self.reader.read_u8() as u8) }
	}

	#[cfg(any(not(feature = "no_vex"), not(feature = "no_xop")))]
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub(self) fn read_vex_op_code_handler_kind(&mut self) -> VexOpCodeHandlerKind {
		unsafe { mem::transmute(self.reader.read_u8() as u8) }
	}

	#[cfg(not(feature = "no_evex"))]
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub(self) fn read_evex_op_code_handler_kind(&mut self) -> EvexOpCodeHandlerKind {
		unsafe { mem::transmute(self.reader.read_u8() as u8) }
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub(self) fn read_code(&mut self) -> u32 {
		self.reader.read_compressed_u32()
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub(self) fn read_register(&mut self) -> Register {
		unsafe { mem::transmute(self.reader.read_u8() as u8) }
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub(self) fn read_decoder_options(&mut self) -> u32 {
		self.reader.read_compressed_u32()
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub(self) fn read_handler_flags(&mut self) -> u32 {
		self.reader.read_compressed_u32()
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub(self) fn read_legacy_handler_flags(&mut self) -> u32 {
		self.reader.read_compressed_u32()
	}

	#[cfg(not(feature = "no_evex"))]
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub(self) fn read_tuple_type(&mut self) -> TupleType {
		unsafe { mem::transmute(self.reader.read_u8() as u8) }
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub(self) fn read_boolean(&mut self) -> bool {
		self.reader.read_u8() != 0
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub(self) fn read_u32(&mut self) -> u32 {
		self.reader.read_compressed_u32()
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub(self) fn read_handler(&mut self) -> *const OpCodeHandler {
		let result = self.read_handler_or_null_instance();
		assert!(!is_null_instance_handler(result));
		result
	}

	#[cfg_attr(has_must_use, must_use)]
	pub(self) fn read_handler_or_null_instance(&mut self) -> *const OpCodeHandler {
		let mut tmp_vec = self.temp_vecs.pop().unwrap_or_else(|| Vec::with_capacity(1));
		debug_assert!(tmp_vec.is_empty());
		(self.handler_reader)(self, &mut tmp_vec);
		assert_eq!(1, tmp_vec.len());
		let result = tmp_vec.pop().unwrap();
		debug_assert!(tmp_vec.is_empty());
		self.temp_vecs.push(tmp_vec);
		result
	}

	#[cfg_attr(has_must_use, must_use)]
	pub(self) fn read_handlers(&mut self, count: usize) -> Vec<&'static OpCodeHandler> {
		let mut handlers: Vec<&'static OpCodeHandler> = Vec::with_capacity(count);
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
			debug_assert_eq!(i, handlers.len());
		}
		assert_eq!(count, handlers.len());
		debug_assert_eq!(i, count);
		handlers
	}

	#[cfg_attr(has_must_use, must_use)]
	pub(self) fn read_handler_reference(&mut self) -> *const OpCodeHandler {
		let index = self.reader.read_u8();
		if let &HandlerInfo::Handler(handler) = self.id_to_handler.get(index).unwrap() {
			handler
		} else {
			unreachable!();
		}
	}

	#[cfg_attr(has_must_use, must_use)]
	pub(self) fn read_array_reference(&mut self, kind: u32) -> Vec<&'static OpCodeHandler> {
		assert_eq!(kind, self.reader.read_u8() as u32);
		let index = self.reader.read_u8();
		if let &HandlerInfo::Handlers(ref handlers) = self.id_to_handler.get(index).unwrap() {
			// There are a few dupe refs, clone the whole thing
			handlers.to_vec()
		} else {
			unreachable!();
		}
	}

	#[cfg_attr(has_must_use, must_use)]
	pub(self) fn read_array_reference_no_clone(&mut self, kind: u32) -> Vec<&'static OpCodeHandler> {
		assert_eq!(kind, self.reader.read_u8() as u32);
		let index = self.reader.read_u8();
		self.table(index)
	}

	#[cfg_attr(has_must_use, must_use)]
	pub(self) fn table(&mut self, index: usize) -> Vec<&'static OpCodeHandler> {
		if let &mut HandlerInfo::Handlers(ref mut tmp) = self.id_to_handler.get_mut(index).unwrap() {
			let handlers = mem::replace(tmp, Vec::new());
			assert!(!handlers.is_empty());
			handlers
		} else {
			unreachable!();
		}
	}
}

#[cfg_attr(has_must_use, must_use)]
pub(super) fn read_legacy() -> Vec<&'static OpCodeHandler> {
	let handler_reader = self::legacy_reader::read_handlers;
	let mut deserializer = TableDeserializer::new(data_legacy::TBL_DATA, data_legacy::MAX_ID_NAMES, handler_reader);
	deserializer.deserialize();
	deserializer.table(data_legacy::ONE_BYTE_HANDLERS_INDEX)
}

#[cfg(not(feature = "no_evex"))]
#[cfg_attr(has_must_use, must_use)]
pub(super) fn read_evex() -> (Vec<&'static OpCodeHandler>, Vec<&'static OpCodeHandler>, Vec<&'static OpCodeHandler>) {
	let handler_reader = self::evex_reader::read_handlers;
	let mut deserializer = TableDeserializer::new(data_evex::TBL_DATA, data_evex::MAX_ID_NAMES, handler_reader);
	deserializer.deserialize();
	(
		deserializer.table(data_evex::TWO_BYTE_HANDLERS_0FXX_INDEX),
		deserializer.table(data_evex::THREE_BYTE_HANDLERS_0F38XX_INDEX),
		deserializer.table(data_evex::THREE_BYTE_HANDLERS_0F3AXX_INDEX),
	)
}

#[cfg(not(feature = "no_vex"))]
#[cfg_attr(has_must_use, must_use)]
pub(super) fn read_vex() -> (Vec<&'static OpCodeHandler>, Vec<&'static OpCodeHandler>, Vec<&'static OpCodeHandler>) {
	let handler_reader = self::vex_reader::read_handlers;
	let mut deserializer = TableDeserializer::new(data_vex::TBL_DATA, data_vex::MAX_ID_NAMES, handler_reader);
	deserializer.deserialize();
	(
		deserializer.table(data_vex::TWO_BYTE_HANDLERS_0FXX_INDEX),
		deserializer.table(data_vex::THREE_BYTE_HANDLERS_0F38XX_INDEX),
		deserializer.table(data_vex::THREE_BYTE_HANDLERS_0F3AXX_INDEX),
	)
}

#[cfg(not(feature = "no_xop"))]
#[cfg_attr(has_must_use, must_use)]
pub(super) fn read_xop() -> (Vec<&'static OpCodeHandler>, Vec<&'static OpCodeHandler>, Vec<&'static OpCodeHandler>) {
	let handler_reader = self::vex_reader::read_handlers;
	let mut deserializer = TableDeserializer::new(data_xop::TBL_DATA, data_xop::MAX_ID_NAMES, handler_reader);
	deserializer.deserialize();
	(deserializer.table(data_xop::XOP8_INDEX), deserializer.table(data_xop::XOP9_INDEX), deserializer.table(data_xop::XOPA_INDEX))
}
