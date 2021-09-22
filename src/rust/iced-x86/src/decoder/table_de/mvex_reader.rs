// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::decoder::handlers::mvex::*;
use crate::decoder::handlers::{
	get_invalid_handler, OpCodeHandler, OpCodeHandlerDecodeFn, OpCodeHandler_Group, OpCodeHandler_MandatoryPrefix2, OpCodeHandler_RM, OpCodeHandler_W,
};
use crate::decoder::table_de::enums::*;
use crate::decoder::table_de::{box_opcode_handler, TableDeserializer};
use alloc::vec::Vec;

#[allow(trivial_casts)]
pub(super) fn read_handlers(deserializer: &mut TableDeserializer<'_>, result: &mut Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>) {
	let (decode, handler_ptr): (OpCodeHandlerDecodeFn, *const OpCodeHandler) = match deserializer.read_mvex_op_code_handler_kind() {
		MvexOpCodeHandlerKind::Invalid => {
			result.push(get_invalid_handler());
			return;
		}

		MvexOpCodeHandlerKind::Invalid2 => {
			result.push(get_invalid_handler());
			result.push(get_invalid_handler());
			return;
		}

		MvexOpCodeHandlerKind::Dup => {
			let count = deserializer.read_u32();
			let handler = deserializer.read_handler();
			for _ in 0..count {
				result.push(handler);
			}
			return;
		}

		MvexOpCodeHandlerKind::HandlerReference => {
			result.push(deserializer.read_handler_reference());
			return;
		}

		MvexOpCodeHandlerKind::ArrayReference => unreachable!(),
		MvexOpCodeHandlerKind::RM => box_opcode_handler(OpCodeHandler_RM::new(deserializer.read_handler(), deserializer.read_handler())),

		MvexOpCodeHandlerKind::Group => {
			box_opcode_handler(OpCodeHandler_Group::new(deserializer.read_array_reference(MvexOpCodeHandlerKind::ArrayReference as u32)))
		}

		MvexOpCodeHandlerKind::W => box_opcode_handler(OpCodeHandler_W::new(deserializer.read_handler(), deserializer.read_handler())),

		MvexOpCodeHandlerKind::MandatoryPrefix2 => box_opcode_handler(OpCodeHandler_MandatoryPrefix2::new(
			true,
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
		)),

		MvexOpCodeHandlerKind::EH => box_opcode_handler(OpCodeHandler_EH::new(deserializer.read_handler(), deserializer.read_handler())),
		MvexOpCodeHandlerKind::M => box_opcode_handler(OpCodeHandler_MVEX_M::new(deserializer.read_code())),
		MvexOpCodeHandlerKind::MV => box_opcode_handler(OpCodeHandler_MVEX_MV::new(deserializer.read_code())),
		MvexOpCodeHandlerKind::VW => box_opcode_handler(OpCodeHandler_MVEX_VW::new(deserializer.read_code())),
		MvexOpCodeHandlerKind::HWIb => box_opcode_handler(OpCodeHandler_MVEX_HWIb::new(deserializer.read_code())),
		MvexOpCodeHandlerKind::VWIb => box_opcode_handler(OpCodeHandler_MVEX_VWIb::new(deserializer.read_code())),
		MvexOpCodeHandlerKind::VHW => box_opcode_handler(OpCodeHandler_MVEX_VHW::new(deserializer.read_code())),
		MvexOpCodeHandlerKind::VHWIb => box_opcode_handler(OpCodeHandler_MVEX_VHWIb::new(deserializer.read_code())),
		MvexOpCodeHandlerKind::VKW => box_opcode_handler(OpCodeHandler_MVEX_VKW::new(deserializer.read_code())),
		MvexOpCodeHandlerKind::KHW => box_opcode_handler(OpCodeHandler_MVEX_KHW::new(deserializer.read_code())),
		MvexOpCodeHandlerKind::KHWIb => box_opcode_handler(OpCodeHandler_MVEX_KHWIb::new(deserializer.read_code())),
		MvexOpCodeHandlerKind::VSIB => box_opcode_handler(OpCodeHandler_MVEX_VSIB::new(deserializer.read_code())),
		MvexOpCodeHandlerKind::VSIB_V => box_opcode_handler(OpCodeHandler_MVEX_VSIB_V::new(deserializer.read_code())),
		MvexOpCodeHandlerKind::V_VSIB => box_opcode_handler(OpCodeHandler_MVEX_V_VSIB::new(deserializer.read_code())),
	};
	let handler = unsafe { &*handler_ptr };
	result.push((decode, handler));
}
