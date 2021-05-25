// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::decoder::handlers::OpCodeHandler;
use crate::decoder::handlers::*;
use crate::decoder::handlers_3dnow::*;
use crate::decoder::handlers_fpu::*;
use crate::decoder::handlers_legacy::*;
use crate::decoder::table_de::enums::*;
use crate::decoder::table_de::TableDeserializer;
use crate::decoder::Code;
use alloc::boxed::Box;
use alloc::vec::Vec;

#[allow(trivial_casts)]
pub(super) fn read_handlers(deserializer: &mut TableDeserializer<'_>, result: &mut Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>) {
	let reg;
	let index;
	let elem: *const OpCodeHandler = match deserializer.read_op_code_handler_kind() {
		OpCodeHandlerKind::Bitness => {
			Box::into_raw(Box::new(OpCodeHandler_Bitness::new(deserializer.read_handler(), deserializer.read_handler()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Bitness_DontReadModRM => {
			Box::into_raw(Box::new(OpCodeHandler_Bitness_DontReadModRM::new(deserializer.read_handler(), deserializer.read_handler())))
				as *const OpCodeHandler
		}

		OpCodeHandlerKind::Invalid => &INVALID_HANDLER as *const _ as *const OpCodeHandler,
		OpCodeHandlerKind::Invalid_NoModRM => &INVALID_NO_MODRM_HANDLER as *const _ as *const OpCodeHandler,

		OpCodeHandlerKind::Invalid2 => {
			result.push((INVALID_HANDLER.decode, unsafe { &*(&INVALID_HANDLER as *const _ as *const OpCodeHandler) }));
			&INVALID_HANDLER as *const _ as *const OpCodeHandler
		}

		OpCodeHandlerKind::Dup => {
			let count = deserializer.read_u32();
			let handler = deserializer.read_handler_or_null_instance();
			for _ in 0..count {
				let handler = unsafe { &*handler.1 };
				result.push((handler.decode, handler));
			}
			return;
		}

		OpCodeHandlerKind::Null => &NULL_HANDLER as *const _ as *const OpCodeHandler,
		OpCodeHandlerKind::HandlerReference => deserializer.read_handler_reference().1,
		OpCodeHandlerKind::ArrayReference => unreachable!(),

		OpCodeHandlerKind::RM => {
			Box::into_raw(Box::new(OpCodeHandler_RM::new(deserializer.read_handler(), deserializer.read_handler()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Options1632_1 => Box::into_raw(Box::new(OpCodeHandler_Options1632::new(
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_decoder_options(),
		))) as *const OpCodeHandler,

		OpCodeHandlerKind::Options1632_2 => Box::into_raw(Box::new(OpCodeHandler_Options1632::new2(
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_decoder_options(),
			deserializer.read_handler(),
			deserializer.read_decoder_options(),
		))) as *const OpCodeHandler,

		OpCodeHandlerKind::Options3 => Box::into_raw(Box::new(OpCodeHandler_Options::new(
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_decoder_options(),
		))) as *const OpCodeHandler,

		OpCodeHandlerKind::Options5 => Box::into_raw(Box::new(OpCodeHandler_Options::new2(
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_decoder_options(),
			deserializer.read_handler(),
			deserializer.read_decoder_options(),
		))) as *const OpCodeHandler,

		OpCodeHandlerKind::Options_DontReadModRM => Box::into_raw(Box::new(OpCodeHandler_Options_DontReadModRM::new(
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_decoder_options(),
		))) as *const OpCodeHandler,

		OpCodeHandlerKind::AnotherTable => Box::into_raw(Box::new(OpCodeHandler_AnotherTable::new(
			deserializer.read_array_reference_no_clone(OpCodeHandlerKind::ArrayReference as u32),
		))) as *const OpCodeHandler,

		OpCodeHandlerKind::Group => {
			Box::into_raw(Box::new(OpCodeHandler_Group::new(deserializer.read_array_reference(OpCodeHandlerKind::ArrayReference as u32))))
				as *const OpCodeHandler
		}

		OpCodeHandlerKind::Group8x64 => Box::into_raw(Box::new(OpCodeHandler_Group8x64::new(
			deserializer.read_array_reference(OpCodeHandlerKind::ArrayReference as u32),
			deserializer.read_array_reference(OpCodeHandlerKind::ArrayReference as u32),
		))) as *const OpCodeHandler,

		OpCodeHandlerKind::Group8x8 => Box::into_raw(Box::new(OpCodeHandler_Group8x8::new(
			deserializer.read_array_reference(OpCodeHandlerKind::ArrayReference as u32),
			deserializer.read_array_reference(OpCodeHandlerKind::ArrayReference as u32),
		))) as *const OpCodeHandler,

		OpCodeHandlerKind::MandatoryPrefix => Box::into_raw(Box::new(OpCodeHandler_MandatoryPrefix::new(
			true,
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
		))) as *const OpCodeHandler,

		OpCodeHandlerKind::MandatoryPrefix4 => Box::into_raw(Box::new(OpCodeHandler_MandatoryPrefix4::new(
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_u32(),
		))) as *const OpCodeHandler,

		OpCodeHandlerKind::MandatoryPrefix_NoModRM => Box::into_raw(Box::new(OpCodeHandler_MandatoryPrefix::new(
			false,
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
		))) as *const OpCodeHandler,

		OpCodeHandlerKind::MandatoryPrefix3 => Box::into_raw(Box::new(OpCodeHandler_MandatoryPrefix3::new(
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_legacy_handler_flags(),
		))) as *const OpCodeHandler,

		OpCodeHandlerKind::D3NOW => Box::into_raw(Box::new(OpCodeHandler_D3NOW::new())) as *const OpCodeHandler,
		OpCodeHandlerKind::EVEX => Box::into_raw(Box::new(OpCodeHandler_EVEX::new(deserializer.read_handler()))) as *const OpCodeHandler,
		OpCodeHandlerKind::VEX2 => Box::into_raw(Box::new(OpCodeHandler_VEX2::new(deserializer.read_handler()))) as *const OpCodeHandler,
		OpCodeHandlerKind::VEX3 => Box::into_raw(Box::new(OpCodeHandler_VEX3::new(deserializer.read_handler()))) as *const OpCodeHandler,
		OpCodeHandlerKind::XOP => Box::into_raw(Box::new(OpCodeHandler_XOP::new(deserializer.read_handler()))) as *const OpCodeHandler,
		OpCodeHandlerKind::AL_DX => Box::into_raw(Box::new(OpCodeHandler_AL_DX::new(deserializer.read_code()))) as *const OpCodeHandler,

		OpCodeHandlerKind::Ap => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_Ap::new(code1, code2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::B_BM => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_B_BM::new(code1, code2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::B_Ev => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_B_Ev::new(code1, code2, deserializer.read_boolean()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::B_MIB => Box::into_raw(Box::new(OpCodeHandler_B_MIB::new(deserializer.read_code()))) as *const OpCodeHandler,

		OpCodeHandlerKind::BM_B => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_BM_B::new(code1, code2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::BranchIw => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_BranchIw::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::BranchSimple => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_BranchSimple::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::C_R_3a => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_C_R::new(code1, code2, deserializer.read_register()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::C_R_3b => {
			Box::into_raw(Box::new(OpCodeHandler_C_R::new(deserializer.read_code(), Code::INVALID, deserializer.read_register())))
				as *const OpCodeHandler
		}

		OpCodeHandlerKind::DX_AL => Box::into_raw(Box::new(OpCodeHandler_DX_AL::new(deserializer.read_code()))) as *const OpCodeHandler,

		OpCodeHandlerKind::DX_eAX => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_DX_eAX::new(code1, code2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::eAX_DX => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_eAX_DX::new(code1, code2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Eb_1 => Box::into_raw(Box::new(OpCodeHandler_Eb::new(deserializer.read_code(), 0))) as *const OpCodeHandler,

		OpCodeHandlerKind::Eb_2 => {
			Box::into_raw(Box::new(OpCodeHandler_Eb::new(deserializer.read_code(), deserializer.read_handler_flags()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Eb_CL => Box::into_raw(Box::new(OpCodeHandler_Eb_CL::new(deserializer.read_code()))) as *const OpCodeHandler,
		OpCodeHandlerKind::Eb_Gb_1 => Box::into_raw(Box::new(OpCodeHandler_Eb_Gb::new(deserializer.read_code(), 0))) as *const OpCodeHandler,

		OpCodeHandlerKind::Eb_Gb_2 => {
			Box::into_raw(Box::new(OpCodeHandler_Eb_Gb::new(deserializer.read_code(), deserializer.read_handler_flags()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Eb_Ib_1 => Box::into_raw(Box::new(OpCodeHandler_Eb_Ib::new(deserializer.read_code(), 0))) as *const OpCodeHandler,

		OpCodeHandlerKind::Eb_Ib_2 => {
			Box::into_raw(Box::new(OpCodeHandler_Eb_Ib::new(deserializer.read_code(), deserializer.read_handler_flags()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Eb1 => Box::into_raw(Box::new(OpCodeHandler_Eb_1::new(deserializer.read_code()))) as *const OpCodeHandler,

		OpCodeHandlerKind::Ed_V_Ib => {
			reg = deserializer.read_register();
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_Ed_V_Ib::new(reg, code1, code2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ep => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Ep::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_3a => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Ev::new(code1, code2, code3, 0))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_3b => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_Ev::new(code1, code2, Code::INVALID, 0))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_4 => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Ev::new(code1, code2, code3, deserializer.read_handler_flags()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_CL => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Ev_CL::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_Gv_32_64 => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_Ev_Gv_32_64::new(code1, code2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_Gv_3a => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Ev_Gv::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_Gv_3b => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_Ev_Gv::new(code1, code2, Code::INVALID))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_Gv_4 => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Ev_Gv_flags::new(code1, code2, code3, deserializer.read_handler_flags()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_Gv_CL => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Ev_Gv_CL::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_Gv_Ib => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Ev_Gv_Ib::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_Gv_REX => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_Ev_Gv_REX::new(code1, code2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_Ib_3 => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Ev_Ib::new(code1, code2, code3, 0))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_Ib_4 => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Ev_Ib::new(code1, code2, code3, deserializer.read_handler_flags()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_Ib2_3 => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Ev_Ib2::new(code1, code2, code3, 0))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_Ib2_4 => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Ev_Ib2::new(code1, code2, code3, deserializer.read_handler_flags()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_Iz_3 => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Ev_Iz::new(code1, code2, code3, 0))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_Iz_4 => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Ev_Iz::new(code1, code2, code3, deserializer.read_handler_flags()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_P => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_Ev_P::new(code1, code2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_REXW_1a => {
			let code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Ev_REXW::new(code, Code::INVALID, deserializer.read_u32()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_REXW => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_Ev_REXW::new(code1, code2, deserializer.read_u32()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_Sw => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Ev_Sw::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_VX => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_Ev_VX::new(code1, code2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev1 => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Ev_1::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Evj => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Evj::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Evw => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Evw::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ew => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Ew::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gb_Eb => Box::into_raw(Box::new(OpCodeHandler_Gb_Eb::new(deserializer.read_code()))) as *const OpCodeHandler,

		OpCodeHandlerKind::Gdq_Ev => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Gdq_Ev::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_Eb => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Gv_Eb::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_Eb_REX => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_Gv_Eb_REX::new(code1, code2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_Ev_32_64 => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_Gv_Ev_32_64::new(code1, code2, deserializer.read_boolean(), deserializer.read_boolean())))
				as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_Ev_3a => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Gv_Ev::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_Ev_3b => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_Gv_Ev::new(code1, code2, Code::INVALID))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_Ev_Ib => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Gv_Ev_Ib::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_Ev_Ib_REX => {
			reg = deserializer.read_register();
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_Gv_Ev_Ib_REX::new(reg, code1, code2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_Ev_Iz => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Gv_Ev_Iz::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_Ev_REX => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_Gv_Ev_REX::new(code1, code2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_Ev2 => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Gv_Ev2::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_Ev3 => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Gv_Ev3::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_Ew => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Gv_Ew::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_M => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Gv_M::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_M_as => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Gv_M_as::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_Ma => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_Gv_Ma::new(code1, code2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_Mp_2 => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_Gv_Mp::new(code1, code2, Code::INVALID))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_Mp_3 => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Gv_Mp::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_Mv => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Gv_Mv::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_N => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_Gv_N::new(code1, code2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_N_Ib_REX => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_Gv_N_Ib_REX::new(code1, code2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_RX => {
			reg = deserializer.read_register();
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_Gv_RX::new(reg, code1, code2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_W => {
			reg = deserializer.read_register();
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_Gv_W::new(reg, code1, code2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::GvM_VX_Ib => {
			reg = deserializer.read_register();
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_GvM_VX_Ib::new(reg, code1, code2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ib => Box::into_raw(Box::new(OpCodeHandler_Ib::new(deserializer.read_code()))) as *const OpCodeHandler,
		OpCodeHandlerKind::Ib3 => Box::into_raw(Box::new(OpCodeHandler_Ib3::new(deserializer.read_code()))) as *const OpCodeHandler,

		OpCodeHandlerKind::IbReg => {
			Box::into_raw(Box::new(OpCodeHandler_IbReg::new(deserializer.read_code(), deserializer.read_register()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::IbReg2 => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_IbReg2::new(code1, code2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Iw_Ib => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Iw_Ib::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Jb => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Jb::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Jb2 => Box::into_raw(Box::new(OpCodeHandler_Jb2::new(
			deserializer.read_code(),
			deserializer.read_code(),
			deserializer.read_code(),
			deserializer.read_code(),
			deserializer.read_code(),
			deserializer.read_code(),
			deserializer.read_code(),
		))) as *const OpCodeHandler,

		OpCodeHandlerKind::Jdisp => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_Jdisp::new(code1, code2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Jx => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_Jx::new(code1, code2, deserializer.read_code()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Jz => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Jz::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::M_1 => Box::into_raw(Box::new(OpCodeHandler_M::new(deserializer.read_code()))) as *const OpCodeHandler,

		OpCodeHandlerKind::M_2 => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_M::new1(code1, code2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::M_REXW_2 => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_M_REXW::new(code1, code2, 0, 0))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::M_REXW_4 => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_M_REXW::new(code1, code2, deserializer.read_handler_flags(), deserializer.read_handler_flags())))
				as *const OpCodeHandler
		}

		OpCodeHandlerKind::MemBx => Box::into_raw(Box::new(OpCodeHandler_MemBx::new(deserializer.read_code()))) as *const OpCodeHandler,
		OpCodeHandlerKind::Mf_1 => Box::into_raw(Box::new(OpCodeHandler_Mf::new(deserializer.read_code()))) as *const OpCodeHandler,

		OpCodeHandlerKind::Mf_2a => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_Mf::new1(code1, code2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Mf_2b => {
			Box::into_raw(Box::new(OpCodeHandler_Mf::new1(deserializer.read_code(), deserializer.read_code()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::MIB_B => Box::into_raw(Box::new(OpCodeHandler_MIB_B::new(deserializer.read_code()))) as *const OpCodeHandler,
		OpCodeHandlerKind::MP => Box::into_raw(Box::new(OpCodeHandler_MP::new(deserializer.read_code()))) as *const OpCodeHandler,

		OpCodeHandlerKind::Ms => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Ms::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::MV => {
			Box::into_raw(Box::new(OpCodeHandler_MV::new(deserializer.read_register(), deserializer.read_code()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Mv_Gv => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Mv_Gv::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Mv_Gv_REXW => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_Mv_Gv_REXW::new(code1, code2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::NIb => Box::into_raw(Box::new(OpCodeHandler_NIb::new(deserializer.read_code()))) as *const OpCodeHandler,

		OpCodeHandlerKind::Ob_Reg => {
			Box::into_raw(Box::new(OpCodeHandler_Ob_Reg::new(deserializer.read_code(), deserializer.read_register()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ov_Reg => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Ov_Reg::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::P_Ev => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_P_Ev::new(code1, code2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::P_Ev_Ib => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_P_Ev_Ib::new(code1, code2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::P_Q => Box::into_raw(Box::new(OpCodeHandler_P_Q::new(deserializer.read_code()))) as *const OpCodeHandler,
		OpCodeHandlerKind::P_Q_Ib => Box::into_raw(Box::new(OpCodeHandler_P_Q_Ib::new(deserializer.read_code()))) as *const OpCodeHandler,

		OpCodeHandlerKind::P_R => {
			Box::into_raw(Box::new(OpCodeHandler_P_R::new(deserializer.read_register(), deserializer.read_code()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::P_W => {
			Box::into_raw(Box::new(OpCodeHandler_P_W::new(deserializer.read_register(), deserializer.read_code()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::PushEv => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_PushEv::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::PushIb2 => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_PushIb2::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::PushIz => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_PushIz::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::PushOpSizeReg_4a => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_PushOpSizeReg::new(code1, code2, code3, deserializer.read_register()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::PushOpSizeReg_4b => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_PushOpSizeReg::new(code1, code2, Code::INVALID, deserializer.read_register())))
				as *const OpCodeHandler
		}

		OpCodeHandlerKind::PushSimple2 => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_PushSimple2::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::PushSimpleReg => {
			index = deserializer.read_u32();
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_PushSimpleReg::new(index, code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Q_P => Box::into_raw(Box::new(OpCodeHandler_Q_P::new(deserializer.read_code()))) as *const OpCodeHandler,

		OpCodeHandlerKind::R_C_3a => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_R_C::new(code1, code2, deserializer.read_register()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::R_C_3b => {
			Box::into_raw(Box::new(OpCodeHandler_R_C::new(deserializer.read_code(), Code::INVALID, deserializer.read_register())))
				as *const OpCodeHandler
		}

		OpCodeHandlerKind::rDI_P_N => Box::into_raw(Box::new(OpCodeHandler_rDI_P_N::new(deserializer.read_code()))) as *const OpCodeHandler,

		OpCodeHandlerKind::rDI_VX_RX => {
			Box::into_raw(Box::new(OpCodeHandler_rDI_VX_RX::new(deserializer.read_register(), deserializer.read_code()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Reg => {
			Box::into_raw(Box::new(OpCodeHandler_Reg::new(deserializer.read_code(), deserializer.read_register()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Reg_Ib2 => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_Reg_Ib2::new(code1, code2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Reg_Iz => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Reg_Iz::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Reg_Ob => {
			Box::into_raw(Box::new(OpCodeHandler_Reg_Ob::new(deserializer.read_code(), deserializer.read_register()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Reg_Ov => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Reg_Ov::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Reg_Xb => {
			Box::into_raw(Box::new(OpCodeHandler_Reg_Xb::new(deserializer.read_code(), deserializer.read_register()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Reg_Xv => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Reg_Xv::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Reg_Xv2 => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_Reg_Xv2::new(code1, code2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Reg_Yb => {
			Box::into_raw(Box::new(OpCodeHandler_Reg_Yb::new(deserializer.read_code(), deserializer.read_register()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Reg_Yv => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Reg_Yv::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::RegIb => {
			Box::into_raw(Box::new(OpCodeHandler_RegIb::new(deserializer.read_code(), deserializer.read_register()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::RegIb3 => Box::into_raw(Box::new(OpCodeHandler_RegIb3::new(deserializer.read_u32()))) as *const OpCodeHandler,
		OpCodeHandlerKind::RegIz2 => Box::into_raw(Box::new(OpCodeHandler_RegIz2::new(deserializer.read_u32()))) as *const OpCodeHandler,

		OpCodeHandlerKind::Reservednop => {
			Box::into_raw(Box::new(OpCodeHandler_Reservednop::new(deserializer.read_handler(), deserializer.read_handler()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::RIb => {
			Box::into_raw(Box::new(OpCodeHandler_RIb::new(deserializer.read_register(), deserializer.read_code()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::RIbIb => {
			Box::into_raw(Box::new(OpCodeHandler_RIbIb::new(deserializer.read_register(), deserializer.read_code()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Rv => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Rv::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Rv_32_64 => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_Rv_32_64::new(code1, code2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::RvMw_Gw => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_RvMw_Gw::new(code1, code2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Simple => Box::into_raw(Box::new(OpCodeHandler_Simple::new(deserializer.read_code()))) as *const OpCodeHandler,
		OpCodeHandlerKind::Simple_ModRM => Box::into_raw(Box::new(OpCodeHandler_Simple::new_modrm(deserializer.read_code()))) as *const OpCodeHandler,

		OpCodeHandlerKind::Simple2_3a => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Simple2::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Simple2_3b => {
			Box::into_raw(Box::new(OpCodeHandler_Simple2::new(deserializer.read_code(), deserializer.read_code(), deserializer.read_code())))
				as *const OpCodeHandler
		}

		OpCodeHandlerKind::Simple2Iw => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Simple2Iw::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Simple3 => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Simple3::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Simple4 => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_Simple4::new(code1, code2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Simple4b => {
			let code1 = deserializer.read_code();
			let code2 = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Simple4::new(code1, code2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Simple5 => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Simple5::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Simple5_ModRM_as => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Simple5_ModRM_as::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::SimpleReg => {
			Box::into_raw(Box::new(OpCodeHandler_SimpleReg::new(deserializer.read_code(), deserializer.read_u32()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::ST_STi => Box::into_raw(Box::new(OpCodeHandler_ST_STi::new(deserializer.read_code()))) as *const OpCodeHandler,
		OpCodeHandlerKind::STi => Box::into_raw(Box::new(OpCodeHandler_STi::new(deserializer.read_code()))) as *const OpCodeHandler,
		OpCodeHandlerKind::STi_ST => Box::into_raw(Box::new(OpCodeHandler_STi_ST::new(deserializer.read_code()))) as *const OpCodeHandler,

		OpCodeHandlerKind::Sw_Ev => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Sw_Ev::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::V_Ev => {
			reg = deserializer.read_register();
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_V_Ev::new(reg, code1, code2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::VM => {
			Box::into_raw(Box::new(OpCodeHandler_VM::new(deserializer.read_register(), deserializer.read_code()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::VN => {
			Box::into_raw(Box::new(OpCodeHandler_VN::new(deserializer.read_register(), deserializer.read_code()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::VQ => {
			Box::into_raw(Box::new(OpCodeHandler_VQ::new(deserializer.read_register(), deserializer.read_code()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::VRIbIb => {
			Box::into_raw(Box::new(OpCodeHandler_VRIbIb::new(deserializer.read_register(), deserializer.read_code()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::VW_2 => {
			Box::into_raw(Box::new(OpCodeHandler_VW::new(deserializer.read_register(), deserializer.read_code()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::VW_3 => {
			Box::into_raw(Box::new(OpCodeHandler_VW::new1(deserializer.read_register(), deserializer.read_code(), deserializer.read_code())))
				as *const OpCodeHandler
		}

		OpCodeHandlerKind::VWIb_2 => {
			Box::into_raw(Box::new(OpCodeHandler_VWIb::new(deserializer.read_register(), deserializer.read_code()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::VWIb_3 => {
			reg = deserializer.read_register();
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_VWIb::new1(reg, code1, code2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::VX_E_Ib => {
			reg = deserializer.read_register();
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_VX_E_Ib::new(reg, code1, code2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::VX_Ev => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_VX_Ev::new(code1, code2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Wbinvd => Box::into_raw(Box::new(OpCodeHandler_Wbinvd::new())) as *const OpCodeHandler,

		OpCodeHandlerKind::WV => {
			Box::into_raw(Box::new(OpCodeHandler_WV::new(deserializer.read_register(), deserializer.read_code()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Xb_Yb => Box::into_raw(Box::new(OpCodeHandler_Xb_Yb::new(deserializer.read_code()))) as *const OpCodeHandler,
		OpCodeHandlerKind::Xchg_Reg_rAX => Box::into_raw(Box::new(OpCodeHandler_Xchg_Reg_rAX::new(deserializer.read_u32()))) as *const OpCodeHandler,

		OpCodeHandlerKind::Xv_Yv => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Xv_Yv::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Yb_Reg => {
			Box::into_raw(Box::new(OpCodeHandler_Yb_Reg::new(deserializer.read_code(), deserializer.read_register()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Yb_Xb => Box::into_raw(Box::new(OpCodeHandler_Yb_Xb::new(deserializer.read_code()))) as *const OpCodeHandler,

		OpCodeHandlerKind::Yv_Reg => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Yv_Reg::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Yv_Reg2 => {
			let (code1, code2) = deserializer.read_code2();
			Box::into_raw(Box::new(OpCodeHandler_Yv_Reg2::new(code1, code2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Yv_Xv => {
			let (code1, code2, code3) = deserializer.read_code3();
			Box::into_raw(Box::new(OpCodeHandler_Yv_Xv::new(code1, code2, code3))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::M_Sw => {
			let code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_M_Sw::new(code))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Sw_M => {
			let code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Sw_M::new(code))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Rq => Box::into_raw(Box::new(OpCodeHandler_Rq::new(deserializer.read_code()))) as *const OpCodeHandler,
		OpCodeHandlerKind::Gd_Rd => Box::into_raw(Box::new(OpCodeHandler_Gd_Rd::new(deserializer.read_code()))) as *const OpCodeHandler,
		OpCodeHandlerKind::PrefixEsCsSsDs => {
			Box::into_raw(Box::new(OpCodeHandler_PrefixEsCsSsDs::new(deserializer.read_register()))) as *const OpCodeHandler
		}
		OpCodeHandlerKind::PrefixFsGs => Box::into_raw(Box::new(OpCodeHandler_PrefixFsGs::new(deserializer.read_register()))) as *const OpCodeHandler,
		OpCodeHandlerKind::Prefix66 => Box::into_raw(Box::new(OpCodeHandler_Prefix66::new())) as *const OpCodeHandler,
		OpCodeHandlerKind::Prefix67 => Box::into_raw(Box::new(OpCodeHandler_Prefix67::new())) as *const OpCodeHandler,
		OpCodeHandlerKind::PrefixF0 => Box::into_raw(Box::new(OpCodeHandler_PrefixF0::new())) as *const OpCodeHandler,
		OpCodeHandlerKind::PrefixF2 => Box::into_raw(Box::new(OpCodeHandler_PrefixF2::new())) as *const OpCodeHandler,
		OpCodeHandlerKind::PrefixF3 => Box::into_raw(Box::new(OpCodeHandler_PrefixF3::new())) as *const OpCodeHandler,
		OpCodeHandlerKind::PrefixREX => {
			Box::into_raw(Box::new(OpCodeHandler_PrefixREX::new(deserializer.read_handler(), deserializer.read_u32()))) as *const OpCodeHandler
		}
	};
	let handler = unsafe { &*elem };
	result.push((handler.decode, handler));
}
