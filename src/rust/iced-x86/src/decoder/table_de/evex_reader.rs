// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::decoder::handlers::evex::*;
use crate::decoder::handlers::{
	get_invalid_handler, OpCodeHandler, OpCodeHandlerDecodeFn, OpCodeHandler_Group, OpCodeHandler_MandatoryPrefix2, OpCodeHandler_RM, OpCodeHandler_W,
};
use crate::decoder::table_de::enums::*;
use crate::decoder::table_de::{box_opcode_handler, TableDeserializer};
use crate::decoder::Code;
use alloc::vec::Vec;

#[allow(trivial_casts)]
pub(super) fn read_handlers(deserializer: &mut TableDeserializer<'_>, result: &mut Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>) {
	let reg;
	let (decode, handler_ptr): (OpCodeHandlerDecodeFn, *const OpCodeHandler) = match deserializer.read_evex_op_code_handler_kind() {
		EvexOpCodeHandlerKind::Invalid => {
			result.push(get_invalid_handler());
			return;
		}

		EvexOpCodeHandlerKind::Invalid2 => {
			result.push(get_invalid_handler());
			result.push(get_invalid_handler());
			return;
		}

		EvexOpCodeHandlerKind::Dup => {
			let count = deserializer.read_u32();
			let handler = deserializer.read_handler();
			for _ in 0..count {
				result.push(handler);
			}
			return;
		}

		EvexOpCodeHandlerKind::HandlerReference => {
			result.push(deserializer.read_handler_reference());
			return;
		}

		EvexOpCodeHandlerKind::ArrayReference => unreachable!(),
		EvexOpCodeHandlerKind::RM => box_opcode_handler(OpCodeHandler_RM::new(deserializer.read_handler(), deserializer.read_handler())),

		EvexOpCodeHandlerKind::Group => {
			box_opcode_handler(OpCodeHandler_Group::new(deserializer.read_array_reference(EvexOpCodeHandlerKind::ArrayReference as u32)))
		}

		EvexOpCodeHandlerKind::W => box_opcode_handler(OpCodeHandler_W::new(deserializer.read_handler(), deserializer.read_handler())),

		EvexOpCodeHandlerKind::MandatoryPrefix2 => box_opcode_handler(OpCodeHandler_MandatoryPrefix2::new(
			true,
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
		)),

		EvexOpCodeHandlerKind::VectorLength => box_opcode_handler(OpCodeHandler_VectorLength_EVEX::new(
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
		)),

		EvexOpCodeHandlerKind::VectorLength_er => box_opcode_handler(OpCodeHandler_VectorLength_EVEX_er::new(
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
		)),

		EvexOpCodeHandlerKind::Ed_V_Ib => {
			reg = deserializer.read_register();
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_EVEX_Ed_V_Ib::new(reg, code1, code2, deserializer.read_tuple_type(), deserializer.read_tuple_type()))
		}

		EvexOpCodeHandlerKind::Ev_VX => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_EVEX_Ev_VX::new(code1, code2, deserializer.read_tuple_type(), deserializer.read_tuple_type()))
		}

		EvexOpCodeHandlerKind::Ev_VX_Ib => {
			reg = deserializer.read_register();
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_EVEX_Ev_VX_Ib::new(reg, code1, code2))
		}

		EvexOpCodeHandlerKind::Gv_W_er => {
			reg = deserializer.read_register();
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_EVEX_Gv_W_er::new(reg, code1, code2, deserializer.read_tuple_type(), deserializer.read_boolean()))
		}

		EvexOpCodeHandlerKind::GvM_VX_Ib => {
			reg = deserializer.read_register();
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_EVEX_GvM_VX_Ib::new(reg, code1, code2, deserializer.read_tuple_type(), deserializer.read_tuple_type()))
		}

		EvexOpCodeHandlerKind::HkWIb_3 => box_opcode_handler(OpCodeHandler_EVEX_HkWIb::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			false,
		)),

		EvexOpCodeHandlerKind::HkWIb_3b => box_opcode_handler(OpCodeHandler_EVEX_HkWIb::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			true,
		)),

		EvexOpCodeHandlerKind::HWIb => {
			box_opcode_handler(OpCodeHandler_EVEX_HWIb::new(deserializer.read_register(), deserializer.read_code(), deserializer.read_tuple_type()))
		}

		EvexOpCodeHandlerKind::KkHW_3 => box_opcode_handler(OpCodeHandler_EVEX_KkHW::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			false,
		)),

		EvexOpCodeHandlerKind::KkHW_3b => box_opcode_handler(OpCodeHandler_EVEX_KkHW::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			true,
		)),

		EvexOpCodeHandlerKind::KkHWIb_sae_3 => box_opcode_handler(OpCodeHandler_EVEX_KkHWIb_sae::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			false,
		)),

		EvexOpCodeHandlerKind::KkHWIb_sae_3b => box_opcode_handler(OpCodeHandler_EVEX_KkHWIb_sae::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			true,
		)),

		EvexOpCodeHandlerKind::KkHWIb_3 => box_opcode_handler(OpCodeHandler_EVEX_KkHWIb::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			false,
		)),

		EvexOpCodeHandlerKind::KkHWIb_3b => box_opcode_handler(OpCodeHandler_EVEX_KkHWIb::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			true,
		)),

		EvexOpCodeHandlerKind::KkWIb_3 => box_opcode_handler(OpCodeHandler_EVEX_KkWIb::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			false,
		)),

		EvexOpCodeHandlerKind::KkWIb_3b => box_opcode_handler(OpCodeHandler_EVEX_KkWIb::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			true,
		)),

		EvexOpCodeHandlerKind::KP1HW => {
			box_opcode_handler(OpCodeHandler_EVEX_KP1HW::new(deserializer.read_register(), deserializer.read_code(), deserializer.read_tuple_type()))
		}

		EvexOpCodeHandlerKind::KR => box_opcode_handler(OpCodeHandler_EVEX_KR::new(deserializer.read_register(), deserializer.read_code())),

		EvexOpCodeHandlerKind::MV => {
			box_opcode_handler(OpCodeHandler_EVEX_MV::new(deserializer.read_register(), deserializer.read_code(), deserializer.read_tuple_type()))
		}

		EvexOpCodeHandlerKind::V_H_Ev_er => {
			reg = deserializer.read_register();
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_EVEX_V_H_Ev_er::new(reg, code1, code2, deserializer.read_tuple_type(), deserializer.read_tuple_type()))
		}

		EvexOpCodeHandlerKind::V_H_Ev_Ib => {
			reg = deserializer.read_register();
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_EVEX_V_H_Ev_Ib::new(reg, code1, code2, deserializer.read_tuple_type(), deserializer.read_tuple_type()))
		}

		EvexOpCodeHandlerKind::VHM => {
			box_opcode_handler(OpCodeHandler_EVEX_VHM::new(deserializer.read_register(), deserializer.read_code(), deserializer.read_tuple_type()))
		}

		EvexOpCodeHandlerKind::VHW_3 => {
			box_opcode_handler(OpCodeHandler_EVEX_VHW::new2(deserializer.read_register(), deserializer.read_code(), deserializer.read_tuple_type()))
		}

		EvexOpCodeHandlerKind::VHW_4 => box_opcode_handler(OpCodeHandler_EVEX_VHW::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
		)),

		EvexOpCodeHandlerKind::VHWIb => {
			box_opcode_handler(OpCodeHandler_EVEX_VHWIb::new(deserializer.read_register(), deserializer.read_code(), deserializer.read_tuple_type()))
		}

		EvexOpCodeHandlerKind::VK => box_opcode_handler(OpCodeHandler_EVEX_VK::new(deserializer.read_register(), deserializer.read_code())),

		EvexOpCodeHandlerKind::Vk_VSIB => box_opcode_handler(OpCodeHandler_EVEX_Vk_VSIB::new(
			deserializer.read_register(),
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
		)),

		EvexOpCodeHandlerKind::VkEv_REXW_2 => {
			box_opcode_handler(OpCodeHandler_EVEX_VkEv_REXW::new(deserializer.read_register(), deserializer.read_code(), Code::INVALID))
		}

		EvexOpCodeHandlerKind::VkEv_REXW_3 => {
			box_opcode_handler(OpCodeHandler_EVEX_VkEv_REXW::new(deserializer.read_register(), deserializer.read_code(), deserializer.read_code()))
		}

		EvexOpCodeHandlerKind::VkHM => {
			box_opcode_handler(OpCodeHandler_EVEX_VkHM::new(deserializer.read_register(), deserializer.read_code(), deserializer.read_tuple_type()))
		}

		EvexOpCodeHandlerKind::VkHW_3 => box_opcode_handler(OpCodeHandler_EVEX_VkHW::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			false,
		)),

		EvexOpCodeHandlerKind::VkHW_3b => box_opcode_handler(OpCodeHandler_EVEX_VkHW::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			true,
		)),

		EvexOpCodeHandlerKind::VkHW_5 => box_opcode_handler(OpCodeHandler_EVEX_VkHW::new1(
			deserializer.read_register(),
			deserializer.read_register(),
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			false,
		)),

		EvexOpCodeHandlerKind::VkHW_er_4 => box_opcode_handler(OpCodeHandler_EVEX_VkHW_er::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			deserializer.read_boolean(),
			false,
		)),

		EvexOpCodeHandlerKind::VkHW_er_4b => box_opcode_handler(OpCodeHandler_EVEX_VkHW_er::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			deserializer.read_boolean(),
			true,
		)),

		EvexOpCodeHandlerKind::VkHW_er_ur_3 => box_opcode_handler(OpCodeHandler_EVEX_VkHW_er_ur::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			false,
		)),

		EvexOpCodeHandlerKind::VkHW_er_ur_3b => box_opcode_handler(OpCodeHandler_EVEX_VkHW_er_ur::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			true,
		)),

		EvexOpCodeHandlerKind::VkHWIb_3 => box_opcode_handler(OpCodeHandler_EVEX_VkHWIb::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			false,
		)),

		EvexOpCodeHandlerKind::VkHWIb_3b => box_opcode_handler(OpCodeHandler_EVEX_VkHWIb::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			true,
		)),

		EvexOpCodeHandlerKind::VkHWIb_5 => box_opcode_handler(OpCodeHandler_EVEX_VkHWIb::new1(
			deserializer.read_register(),
			deserializer.read_register(),
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			false,
		)),

		EvexOpCodeHandlerKind::VkHWIb_er_4 => box_opcode_handler(OpCodeHandler_EVEX_VkHWIb_er::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			false,
		)),

		EvexOpCodeHandlerKind::VkHWIb_er_4b => box_opcode_handler(OpCodeHandler_EVEX_VkHWIb_er::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			true,
		)),

		EvexOpCodeHandlerKind::VkM => {
			box_opcode_handler(OpCodeHandler_EVEX_VkM::new(deserializer.read_register(), deserializer.read_code(), deserializer.read_tuple_type()))
		}

		EvexOpCodeHandlerKind::VkW_3 => box_opcode_handler(OpCodeHandler_EVEX_VkW::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			false,
		)),

		EvexOpCodeHandlerKind::VkW_3b => box_opcode_handler(OpCodeHandler_EVEX_VkW::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			true,
		)),

		EvexOpCodeHandlerKind::VkW_4 => box_opcode_handler(OpCodeHandler_EVEX_VkW::new1(
			deserializer.read_register(),
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			false,
		)),

		EvexOpCodeHandlerKind::VkW_4b => box_opcode_handler(OpCodeHandler_EVEX_VkW::new1(
			deserializer.read_register(),
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			true,
		)),

		EvexOpCodeHandlerKind::VkW_er_4 => box_opcode_handler(OpCodeHandler_EVEX_VkW_er::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			deserializer.read_boolean(),
		)),

		EvexOpCodeHandlerKind::VkW_er_5 => box_opcode_handler(OpCodeHandler_EVEX_VkW_er::new1(
			deserializer.read_register(),
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			deserializer.read_boolean(),
		)),

		EvexOpCodeHandlerKind::VkW_er_6 => box_opcode_handler(OpCodeHandler_EVEX_VkW_er::new2(
			deserializer.read_register(),
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			deserializer.read_boolean(),
			deserializer.read_boolean(),
		)),

		EvexOpCodeHandlerKind::VkWIb_3 => box_opcode_handler(OpCodeHandler_EVEX_VkWIb::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			false,
		)),

		EvexOpCodeHandlerKind::VkWIb_3b => box_opcode_handler(OpCodeHandler_EVEX_VkWIb::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			true,
		)),

		EvexOpCodeHandlerKind::VkWIb_er => box_opcode_handler(OpCodeHandler_EVEX_VkWIb_er::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
		)),

		EvexOpCodeHandlerKind::VM => {
			box_opcode_handler(OpCodeHandler_EVEX_VM::new(deserializer.read_register(), deserializer.read_code(), deserializer.read_tuple_type()))
		}

		EvexOpCodeHandlerKind::VSIB_k1 => box_opcode_handler(OpCodeHandler_EVEX_VSIB_k1::new(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
		)),

		EvexOpCodeHandlerKind::VSIB_k1_VX => box_opcode_handler(OpCodeHandler_EVEX_VSIB_k1_VX::new(
			deserializer.read_register(),
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
		)),

		EvexOpCodeHandlerKind::VW => {
			box_opcode_handler(OpCodeHandler_EVEX_VW::new(deserializer.read_register(), deserializer.read_code(), deserializer.read_tuple_type()))
		}

		EvexOpCodeHandlerKind::VW_er => {
			box_opcode_handler(OpCodeHandler_EVEX_VW_er::new(deserializer.read_register(), deserializer.read_code(), deserializer.read_tuple_type()))
		}

		EvexOpCodeHandlerKind::VX_Ev => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_EVEX_VX_Ev::new(code1, code2, deserializer.read_tuple_type(), deserializer.read_tuple_type()))
		}

		EvexOpCodeHandlerKind::WkHV => box_opcode_handler(OpCodeHandler_EVEX_WkHV::new(deserializer.read_register(), deserializer.read_code())),

		EvexOpCodeHandlerKind::WkV_3 => {
			box_opcode_handler(OpCodeHandler_EVEX_WkV::new(deserializer.read_register(), deserializer.read_code(), deserializer.read_tuple_type()))
		}

		EvexOpCodeHandlerKind::WkV_4a => box_opcode_handler(OpCodeHandler_EVEX_WkV::new2(
			deserializer.read_register(),
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
		)),

		EvexOpCodeHandlerKind::WkV_4b => box_opcode_handler(OpCodeHandler_EVEX_WkV::new1(
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
			deserializer.read_boolean(),
		)),

		EvexOpCodeHandlerKind::WkVIb => box_opcode_handler(OpCodeHandler_EVEX_WkVIb::new(
			deserializer.read_register(),
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
		)),

		EvexOpCodeHandlerKind::WkVIb_er => box_opcode_handler(OpCodeHandler_EVEX_WkVIb_er::new(
			deserializer.read_register(),
			deserializer.read_register(),
			deserializer.read_code(),
			deserializer.read_tuple_type(),
		)),

		EvexOpCodeHandlerKind::WV => {
			box_opcode_handler(OpCodeHandler_EVEX_WV::new(deserializer.read_register(), deserializer.read_code(), deserializer.read_tuple_type()))
		}
	};
	let handler = unsafe { &*handler_ptr };
	result.push((decode, handler));
}
