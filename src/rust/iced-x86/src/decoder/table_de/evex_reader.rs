// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::decoder::handlers::OpCodeHandler;
use crate::decoder::handlers::*;
use crate::decoder::handlers_evex::*;
use crate::decoder::table_de::enums::*;
use crate::decoder::table_de::TableDeserializer;
use crate::decoder::Code;
use alloc::boxed::Box;
use alloc::vec::Vec;

#[allow(trivial_casts)]
pub(super) fn read_handlers(deserializer: &mut TableDeserializer<'_>, result: &mut Vec<(&'static OpCodeHandler, OpCodeHandlerDecodeFn)>) {
	let code;
	let reg;
	let elem: *const OpCodeHandler = match deserializer.read_evex_op_code_handler_kind() {
		EvexOpCodeHandlerKind::Invalid => &INVALID_HANDLER as *const _ as *const OpCodeHandler,

		EvexOpCodeHandlerKind::Invalid2 => {
			result.push((unsafe { &*(&INVALID_HANDLER as *const _ as *const OpCodeHandler) }, INVALID_HANDLER.decode));
			&INVALID_HANDLER as *const _ as *const OpCodeHandler
		}

		EvexOpCodeHandlerKind::Dup => {
			let count = deserializer.read_u32();
			let handler = deserializer.read_handler();
			for _ in 0..count {
				let handler = unsafe { &*handler.0 };
				result.push((handler, handler.decode));
			}
			return;
		}

		EvexOpCodeHandlerKind::HandlerReference => deserializer.read_handler_reference().0,

		EvexOpCodeHandlerKind::ArrayReference => unreachable!(),

		EvexOpCodeHandlerKind::RM => {
			Box::into_raw(Box::new(OpCodeHandler_RM::new(deserializer.read_handler(), deserializer.read_handler()))) as *const OpCodeHandler
		}

		EvexOpCodeHandlerKind::Group => {
			Box::into_raw(Box::new(OpCodeHandler_Group::new(deserializer.read_array_reference(EvexOpCodeHandlerKind::ArrayReference as u32))))
				as *const OpCodeHandler
		}

		EvexOpCodeHandlerKind::W => {
			Box::into_raw(Box::new(OpCodeHandler_W::new(deserializer.read_handler(), deserializer.read_handler()))) as *const OpCodeHandler
		}

		EvexOpCodeHandlerKind::MandatoryPrefix2 => Box::into_raw(Box::new(OpCodeHandler_MandatoryPrefix2::new(
			true,
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::VectorLength => Box::into_raw(Box::new(OpCodeHandler_VectorLength_EVEX::new(
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::VectorLength_er => Box::into_raw(Box::new(OpCodeHandler_VectorLength_EVEX_er::new(
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::Ed_V_Ib => {
			reg = deserializer.read_register();
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_EVEX_Ed_V_Ib::new(
				reg,
				code,
				code + 1,
				deserializer.read_tuple_type(),
				deserializer.read_tuple_type(),
			))) as *const OpCodeHandler
		}

		EvexOpCodeHandlerKind::Ev_VX => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_EVEX_Ev_VX::new(code, code + 1, deserializer.read_tuple_type(), deserializer.read_tuple_type())))
				as *const OpCodeHandler
		}

		EvexOpCodeHandlerKind::Ev_VX_Ib => {
			reg = deserializer.read_register();
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_EVEX_Ev_VX_Ib::new(reg, code, code + 1))) as *const OpCodeHandler
		}

		EvexOpCodeHandlerKind::Gv_W_er => {
			reg = deserializer.read_register();
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_EVEX_Gv_W_er::new(reg, code, code + 1, deserializer.read_tuple_type(), deserializer.read_boolean())))
				as *const OpCodeHandler
		}

		EvexOpCodeHandlerKind::GvM_VX_Ib => {
			reg = deserializer.read_register();
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_EVEX_GvM_VX_Ib::new(
				reg,
				code,
				code + 1,
				deserializer.read_tuple_type(),
				deserializer.read_tuple_type(),
			))) as *const OpCodeHandler
		}

		EvexOpCodeHandlerKind::HkWIb_3 => Box::into_raw(Box::new(OpCodeHandler_EVEX_HkWIb::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			false,
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::HkWIb_3b => Box::into_raw(Box::new(OpCodeHandler_EVEX_HkWIb::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			true,
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::HWIb => Box::into_raw(Box::new(OpCodeHandler_EVEX_HWIb::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::KkHW_3 => Box::into_raw(Box::new(OpCodeHandler_EVEX_KkHW::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			false,
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::KkHW_3b => Box::into_raw(Box::new(OpCodeHandler_EVEX_KkHW::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			true,
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::KkHWIb_sae_3 => Box::into_raw(Box::new(OpCodeHandler_EVEX_KkHWIb_sae::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			false,
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::KkHWIb_sae_3b => Box::into_raw(Box::new(OpCodeHandler_EVEX_KkHWIb_sae::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			true,
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::KkHWIb_3 => Box::into_raw(Box::new(OpCodeHandler_EVEX_KkHWIb::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			false,
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::KkHWIb_3b => Box::into_raw(Box::new(OpCodeHandler_EVEX_KkHWIb::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			true,
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::KkWIb_3 => Box::into_raw(Box::new(OpCodeHandler_EVEX_KkWIb::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			false,
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::KkWIb_3b => Box::into_raw(Box::new(OpCodeHandler_EVEX_KkWIb::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			true,
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::KP1HW => Box::into_raw(Box::new(OpCodeHandler_EVEX_KP1HW::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::KR => {
			Box::into_raw(Box::new(OpCodeHandler_EVEX_KR::new(deserializer.read_register(), deserializer.read_code()))) as *const OpCodeHandler
		}

		EvexOpCodeHandlerKind::MV => Box::into_raw(Box::new(OpCodeHandler_EVEX_MV::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::V_H_Ev_er => {
			reg = deserializer.read_register();
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_EVEX_V_H_Ev_er::new(
				reg,
				code,
				code + 1,
				deserializer.read_tuple_type(),
				deserializer.read_tuple_type(),
			))) as *const OpCodeHandler
		}

		EvexOpCodeHandlerKind::V_H_Ev_Ib => {
			reg = deserializer.read_register();
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_EVEX_V_H_Ev_Ib::new(
				reg,
				code,
				code + 1,
				deserializer.read_tuple_type(),
				deserializer.read_tuple_type(),
			))) as *const OpCodeHandler
		}

		EvexOpCodeHandlerKind::VHM => Box::into_raw(Box::new(OpCodeHandler_EVEX_VHM::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::VHW_3 => Box::into_raw(Box::new(OpCodeHandler_EVEX_VHW::new2(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::VHW_4 => Box::into_raw(Box::new(OpCodeHandler_EVEX_VHW::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::VHWIb => Box::into_raw(Box::new(OpCodeHandler_EVEX_VHWIb::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::VK => {
			Box::into_raw(Box::new(OpCodeHandler_EVEX_VK::new(deserializer.read_register(), deserializer.read_code()))) as *const OpCodeHandler
		}

		EvexOpCodeHandlerKind::Vk_VSIB => Box::into_raw(Box::new(OpCodeHandler_EVEX_Vk_VSIB::new(
			deserializer.read_register(),
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::VkEv_REXW_2 => {
			Box::into_raw(Box::new(OpCodeHandler_EVEX_VkEv_REXW::new(deserializer.read_register(), deserializer.read_code(), Code::INVALID as u32)))
				as *const OpCodeHandler
		}

		EvexOpCodeHandlerKind::VkEv_REXW_3 => Box::into_raw(Box::new(OpCodeHandler_EVEX_VkEv_REXW::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_code(),
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::VkHM => Box::into_raw(Box::new(OpCodeHandler_EVEX_VkHM::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::VkHW_3 => Box::into_raw(Box::new(OpCodeHandler_EVEX_VkHW::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			false,
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::VkHW_3b => Box::into_raw(Box::new(OpCodeHandler_EVEX_VkHW::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			true,
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::VkHW_5 => Box::into_raw(Box::new(OpCodeHandler_EVEX_VkHW::new1(
			deserializer.read_register(),
			deserializer.read_register(),
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			false,
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::VkHW_er_4 => Box::into_raw(Box::new(OpCodeHandler_EVEX_VkHW_er::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			deserializer.read_boolean(),
			false,
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::VkHW_er_4b => Box::into_raw(Box::new(OpCodeHandler_EVEX_VkHW_er::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			deserializer.read_boolean(),
			true,
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::VkHWIb_3 => Box::into_raw(Box::new(OpCodeHandler_EVEX_VkHWIb::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			false,
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::VkHWIb_3b => Box::into_raw(Box::new(OpCodeHandler_EVEX_VkHWIb::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			true,
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::VkHWIb_5 => Box::into_raw(Box::new(OpCodeHandler_EVEX_VkHWIb::new1(
			deserializer.read_register(),
			deserializer.read_register(),
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			false,
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::VkHWIb_er_4 => Box::into_raw(Box::new(OpCodeHandler_EVEX_VkHWIb_er::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			false,
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::VkHWIb_er_4b => Box::into_raw(Box::new(OpCodeHandler_EVEX_VkHWIb_er::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			true,
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::VkM => Box::into_raw(Box::new(OpCodeHandler_EVEX_VkM::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::VkW_3 => Box::into_raw(Box::new(OpCodeHandler_EVEX_VkW::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			false,
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::VkW_3b => Box::into_raw(Box::new(OpCodeHandler_EVEX_VkW::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			true,
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::VkW_4 => Box::into_raw(Box::new(OpCodeHandler_EVEX_VkW::new1(
			deserializer.read_register(),
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			false,
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::VkW_4b => Box::into_raw(Box::new(OpCodeHandler_EVEX_VkW::new1(
			deserializer.read_register(),
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			true,
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::VkW_er_4 => Box::into_raw(Box::new(OpCodeHandler_EVEX_VkW_er::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			deserializer.read_boolean(),
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::VkW_er_5 => Box::into_raw(Box::new(OpCodeHandler_EVEX_VkW_er::new1(
			deserializer.read_register(),
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			deserializer.read_boolean(),
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::VkW_er_6 => Box::into_raw(Box::new(OpCodeHandler_EVEX_VkW_er::new2(
			deserializer.read_register(),
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			deserializer.read_boolean(),
			deserializer.read_boolean(),
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::VkWIb_3 => Box::into_raw(Box::new(OpCodeHandler_EVEX_VkWIb::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			false,
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::VkWIb_3b => Box::into_raw(Box::new(OpCodeHandler_EVEX_VkWIb::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			true,
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::VkWIb_er => Box::into_raw(Box::new(OpCodeHandler_EVEX_VkWIb_er::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::VM => Box::into_raw(Box::new(OpCodeHandler_EVEX_VM::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::VSIB_k1 => Box::into_raw(Box::new(OpCodeHandler_EVEX_VSIB_k1::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::VSIB_k1_VX => Box::into_raw(Box::new(OpCodeHandler_EVEX_VSIB_k1_VX::new(
			deserializer.read_register(),
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::VW => Box::into_raw(Box::new(OpCodeHandler_EVEX_VW::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::VW_er => Box::into_raw(Box::new(OpCodeHandler_EVEX_VW_er::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::VX_Ev => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_EVEX_VX_Ev::new(code, code + 1, deserializer.read_tuple_type(), deserializer.read_tuple_type())))
				as *const OpCodeHandler
		}

		EvexOpCodeHandlerKind::WkHV => {
			Box::into_raw(Box::new(OpCodeHandler_EVEX_WkHV::new(deserializer.read_register(), deserializer.read_code()))) as *const OpCodeHandler
		}

		EvexOpCodeHandlerKind::WkV_3 => Box::into_raw(Box::new(OpCodeHandler_EVEX_WkV::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::WkV_4a => Box::into_raw(Box::new(OpCodeHandler_EVEX_WkV::new2(
			deserializer.read_register(),
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::WkV_4b => Box::into_raw(Box::new(OpCodeHandler_EVEX_WkV::new1(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			deserializer.read_boolean(),
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::WkVIb => Box::into_raw(Box::new(OpCodeHandler_EVEX_WkVIb::new(
			deserializer.read_register(),
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::WkVIb_er => Box::into_raw(Box::new(OpCodeHandler_EVEX_WkVIb_er::new(
			deserializer.read_register(),
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
		))) as *const OpCodeHandler,

		EvexOpCodeHandlerKind::WV => Box::into_raw(Box::new(OpCodeHandler_EVEX_WV::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
		))) as *const OpCodeHandler,
	};
	let handler = unsafe { &*elem };
	result.push((handler, handler.decode));
}
