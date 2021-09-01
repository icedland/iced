// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::decoder::handlers::OpCodeHandler;
use crate::decoder::handlers::*;
use crate::decoder::handlers_3dnow::*;
use crate::decoder::handlers_fpu::*;
use crate::decoder::handlers_legacy::*;
use crate::decoder::table_de::enums::*;
use crate::decoder::table_de::{box_opcode_handler, TableDeserializer};
use crate::decoder::Code;
use alloc::vec::Vec;

#[allow(trivial_casts)]
pub(super) fn read_handlers(deserializer: &mut TableDeserializer<'_>, result: &mut Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>) {
	let reg;
	let index;
	let (decode, handler_ptr): (OpCodeHandlerDecodeFn, *const OpCodeHandler) = match deserializer.read_op_code_handler_kind() {
		OpCodeHandlerKind::Bitness => box_opcode_handler(OpCodeHandler_Bitness::new(deserializer.read_handler(), deserializer.read_handler())),

		OpCodeHandlerKind::Bitness_DontReadModRM => {
			box_opcode_handler(OpCodeHandler_Bitness_DontReadModRM::new(deserializer.read_handler(), deserializer.read_handler()))
		}

		OpCodeHandlerKind::Invalid => {
			result.push(get_invalid_handler());
			return;
		}

		OpCodeHandlerKind::Invalid_NoModRM => {
			result.push(get_invalid_no_modrm_handler());
			return;
		}

		OpCodeHandlerKind::Invalid2 => {
			result.push(get_invalid_handler());
			result.push(get_invalid_handler());
			return;
		}

		OpCodeHandlerKind::Dup => {
			let count = deserializer.read_u32();
			let handler = deserializer.read_handler_or_null_instance();
			for _ in 0..count {
				result.push(handler);
			}
			return;
		}

		OpCodeHandlerKind::Null => {
			result.push(get_null_handler());
			return;
		}

		OpCodeHandlerKind::HandlerReference => {
			result.push(deserializer.read_handler_reference());
			return;
		}

		OpCodeHandlerKind::ArrayReference => unreachable!(),
		OpCodeHandlerKind::RM => box_opcode_handler(OpCodeHandler_RM::new(deserializer.read_handler(), deserializer.read_handler())),

		OpCodeHandlerKind::Options1632_1 => box_opcode_handler(OpCodeHandler_Options1632::new(
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_decoder_options(),
		)),

		OpCodeHandlerKind::Options1632_2 => box_opcode_handler(OpCodeHandler_Options1632::new2(
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_decoder_options(),
			deserializer.read_handler(),
			deserializer.read_decoder_options(),
		)),

		OpCodeHandlerKind::Options3 => box_opcode_handler(OpCodeHandler_Options::new(
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_decoder_options(),
		)),

		OpCodeHandlerKind::Options5 => box_opcode_handler(OpCodeHandler_Options::new2(
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_decoder_options(),
			deserializer.read_handler(),
			deserializer.read_decoder_options(),
		)),

		OpCodeHandlerKind::Options_DontReadModRM => box_opcode_handler(OpCodeHandler_Options_DontReadModRM::new(
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_decoder_options(),
		)),

		OpCodeHandlerKind::AnotherTable => {
			box_opcode_handler(OpCodeHandler_AnotherTable::new(deserializer.read_array_reference_no_clone(OpCodeHandlerKind::ArrayReference as u32)))
		}

		OpCodeHandlerKind::Group => {
			box_opcode_handler(OpCodeHandler_Group::new(deserializer.read_array_reference(OpCodeHandlerKind::ArrayReference as u32)))
		}

		OpCodeHandlerKind::Group8x64 => box_opcode_handler(OpCodeHandler_Group8x64::new(
			deserializer.read_array_reference(OpCodeHandlerKind::ArrayReference as u32),
			deserializer.read_array_reference(OpCodeHandlerKind::ArrayReference as u32),
		)),

		OpCodeHandlerKind::Group8x8 => box_opcode_handler(OpCodeHandler_Group8x8::new(
			deserializer.read_array_reference(OpCodeHandlerKind::ArrayReference as u32),
			deserializer.read_array_reference(OpCodeHandlerKind::ArrayReference as u32),
		)),

		OpCodeHandlerKind::MandatoryPrefix => box_opcode_handler(OpCodeHandler_MandatoryPrefix::new(
			true,
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
		)),

		OpCodeHandlerKind::MandatoryPrefix4 => box_opcode_handler(OpCodeHandler_MandatoryPrefix4::new(
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_u32(),
		)),

		OpCodeHandlerKind::MandatoryPrefix_NoModRM => box_opcode_handler(OpCodeHandler_MandatoryPrefix::new(
			false,
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
		)),

		OpCodeHandlerKind::MandatoryPrefix3 => box_opcode_handler(OpCodeHandler_MandatoryPrefix3::new(
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_legacy_handler_flags(),
		)),

		OpCodeHandlerKind::D3NOW => box_opcode_handler(OpCodeHandler_D3NOW::new()),
		OpCodeHandlerKind::EVEX => box_opcode_handler(OpCodeHandler_EVEX::new(deserializer.read_handler())),
		OpCodeHandlerKind::VEX2 => box_opcode_handler(OpCodeHandler_VEX2::new(deserializer.read_handler())),
		OpCodeHandlerKind::VEX3 => box_opcode_handler(OpCodeHandler_VEX3::new(deserializer.read_handler())),
		OpCodeHandlerKind::XOP => box_opcode_handler(OpCodeHandler_XOP::new(deserializer.read_handler())),
		OpCodeHandlerKind::AL_DX => box_opcode_handler(OpCodeHandler_AL_DX::new(deserializer.read_code())),

		OpCodeHandlerKind::Ap => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Ap::new(code1, code2))
		}

		OpCodeHandlerKind::B_BM => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_B_BM::new(code1, code2))
		}

		OpCodeHandlerKind::B_Ev => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_B_Ev::new(code1, code2, deserializer.read_boolean()))
		}

		OpCodeHandlerKind::B_MIB => box_opcode_handler(OpCodeHandler_B_MIB::new(deserializer.read_code())),

		OpCodeHandlerKind::BM_B => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_BM_B::new(code1, code2))
		}

		OpCodeHandlerKind::BranchIw => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_BranchIw::new(code1, code2, code3))
		}

		OpCodeHandlerKind::BranchSimple => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_BranchSimple::new(code1, code2, code3))
		}

		OpCodeHandlerKind::C_R_3a => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_C_R::new(code1, code2, deserializer.read_register()))
		}

		OpCodeHandlerKind::C_R_3b => {
			box_opcode_handler(OpCodeHandler_C_R::new(deserializer.read_code(), Code::INVALID, deserializer.read_register()))
		}

		OpCodeHandlerKind::DX_AL => box_opcode_handler(OpCodeHandler_DX_AL::new(deserializer.read_code())),

		OpCodeHandlerKind::DX_eAX => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_DX_eAX::new(code1, code2))
		}

		OpCodeHandlerKind::eAX_DX => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_eAX_DX::new(code1, code2))
		}

		OpCodeHandlerKind::Eb_1 => box_opcode_handler(OpCodeHandler_Eb::new(deserializer.read_code(), 0)),
		OpCodeHandlerKind::Eb_2 => box_opcode_handler(OpCodeHandler_Eb::new(deserializer.read_code(), deserializer.read_handler_flags())),
		OpCodeHandlerKind::Eb_CL => box_opcode_handler(OpCodeHandler_Eb_CL::new(deserializer.read_code())),
		OpCodeHandlerKind::Eb_Gb_1 => box_opcode_handler(OpCodeHandler_Eb_Gb::new(deserializer.read_code(), 0)),
		OpCodeHandlerKind::Eb_Gb_2 => box_opcode_handler(OpCodeHandler_Eb_Gb::new(deserializer.read_code(), deserializer.read_handler_flags())),
		OpCodeHandlerKind::Eb_Ib_1 => box_opcode_handler(OpCodeHandler_Eb_Ib::new(deserializer.read_code(), 0)),
		OpCodeHandlerKind::Eb_Ib_2 => box_opcode_handler(OpCodeHandler_Eb_Ib::new(deserializer.read_code(), deserializer.read_handler_flags())),
		OpCodeHandlerKind::Eb1 => box_opcode_handler(OpCodeHandler_Eb_1::new(deserializer.read_code())),

		OpCodeHandlerKind::Ed_V_Ib => {
			reg = deserializer.read_register();
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Ed_V_Ib::new(reg, code1, code2))
		}

		OpCodeHandlerKind::Ep => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Ep::new(code1, code2, code3))
		}

		OpCodeHandlerKind::Ev_3a => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Ev::new(code1, code2, code3, 0))
		}

		OpCodeHandlerKind::Ev_3b => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Ev::new(code1, code2, Code::INVALID, 0))
		}

		OpCodeHandlerKind::Ev_4 => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Ev::new(code1, code2, code3, deserializer.read_handler_flags()))
		}

		OpCodeHandlerKind::Ev_CL => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Ev_CL::new(code1, code2, code3))
		}

		OpCodeHandlerKind::Ev_Gv_32_64 => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Ev_Gv_32_64::new(code1, code2))
		}

		OpCodeHandlerKind::Ev_Gv_3a => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Ev_Gv::new(code1, code2, code3))
		}

		OpCodeHandlerKind::Ev_Gv_3b => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Ev_Gv::new(code1, code2, Code::INVALID))
		}

		OpCodeHandlerKind::Ev_Gv_4 => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Ev_Gv_flags::new(code1, code2, code3, deserializer.read_handler_flags()))
		}

		OpCodeHandlerKind::Ev_Gv_CL => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Ev_Gv_CL::new(code1, code2, code3))
		}

		OpCodeHandlerKind::Ev_Gv_Ib => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Ev_Gv_Ib::new(code1, code2, code3))
		}

		OpCodeHandlerKind::Ev_Gv_REX => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Ev_Gv_REX::new(code1, code2))
		}

		OpCodeHandlerKind::Ev_Ib_3 => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Ev_Ib::new(code1, code2, code3, 0))
		}

		OpCodeHandlerKind::Ev_Ib_4 => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Ev_Ib::new(code1, code2, code3, deserializer.read_handler_flags()))
		}

		OpCodeHandlerKind::Ev_Ib2_3 => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Ev_Ib2::new(code1, code2, code3, 0))
		}

		OpCodeHandlerKind::Ev_Ib2_4 => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Ev_Ib2::new(code1, code2, code3, deserializer.read_handler_flags()))
		}

		OpCodeHandlerKind::Ev_Iz_3 => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Ev_Iz::new(code1, code2, code3, 0))
		}

		OpCodeHandlerKind::Ev_Iz_4 => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Ev_Iz::new(code1, code2, code3, deserializer.read_handler_flags()))
		}

		OpCodeHandlerKind::Ev_P => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Ev_P::new(code1, code2))
		}

		OpCodeHandlerKind::Ev_REXW_1a => {
			let code = deserializer.read_code();
			box_opcode_handler(OpCodeHandler_Ev_REXW::new(code, Code::INVALID, deserializer.read_u32()))
		}

		OpCodeHandlerKind::Ev_REXW => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Ev_REXW::new(code1, code2, deserializer.read_u32()))
		}

		OpCodeHandlerKind::Ev_Sw => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Ev_Sw::new(code1, code2, code3))
		}

		OpCodeHandlerKind::Ev_VX => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Ev_VX::new(code1, code2))
		}

		OpCodeHandlerKind::Ev1 => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Ev_1::new(code1, code2, code3))
		}

		OpCodeHandlerKind::Evj => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Evj::new(code1, code2, code3))
		}

		OpCodeHandlerKind::Evw => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Evw::new(code1, code2, code3))
		}

		OpCodeHandlerKind::Ew => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Ew::new(code1, code2, code3))
		}

		OpCodeHandlerKind::Gb_Eb => box_opcode_handler(OpCodeHandler_Gb_Eb::new(deserializer.read_code())),

		OpCodeHandlerKind::Gdq_Ev => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Gdq_Ev::new(code1, code2, code3))
		}

		OpCodeHandlerKind::Gv_Eb => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Gv_Eb::new(code1, code2, code3))
		}

		OpCodeHandlerKind::Gv_Eb_REX => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Gv_Eb_REX::new(code1, code2))
		}

		OpCodeHandlerKind::Gv_Ev_32_64 => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Gv_Ev_32_64::new(code1, code2, deserializer.read_boolean(), deserializer.read_boolean()))
		}

		OpCodeHandlerKind::Gv_Ev_3a => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Gv_Ev::new(code1, code2, code3))
		}

		OpCodeHandlerKind::Gv_Ev_3b => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Gv_Ev::new(code1, code2, Code::INVALID))
		}

		OpCodeHandlerKind::Gv_Ev_Ib => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Gv_Ev_Ib::new(code1, code2, code3))
		}

		OpCodeHandlerKind::Gv_Ev_Ib_REX => {
			reg = deserializer.read_register();
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Gv_Ev_Ib_REX::new(reg, code1, code2))
		}

		OpCodeHandlerKind::Gv_Ev_Iz => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Gv_Ev_Iz::new(code1, code2, code3))
		}

		OpCodeHandlerKind::Gv_Ev_REX => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Gv_Ev_REX::new(code1, code2))
		}

		OpCodeHandlerKind::Gv_Ev2 => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Gv_Ev2::new(code1, code2, code3))
		}

		OpCodeHandlerKind::Gv_Ev3 => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Gv_Ev3::new(code1, code2, code3))
		}

		OpCodeHandlerKind::Gv_Ew => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Gv_Ew::new(code1, code2, code3))
		}

		OpCodeHandlerKind::Gv_M => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Gv_M::new(code1, code2, code3))
		}

		OpCodeHandlerKind::Gv_M_as => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Gv_M_as::new(code1, code2, code3))
		}

		OpCodeHandlerKind::Gv_Ma => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Gv_Ma::new(code1, code2))
		}

		OpCodeHandlerKind::Gv_Mp_2 => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Gv_Mp::new(code1, code2, Code::INVALID))
		}

		OpCodeHandlerKind::Gv_Mp_3 => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Gv_Mp::new(code1, code2, code3))
		}

		OpCodeHandlerKind::Gv_Mv => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Gv_Mv::new(code1, code2, code3))
		}

		OpCodeHandlerKind::Gv_N => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Gv_N::new(code1, code2))
		}

		OpCodeHandlerKind::Gv_N_Ib_REX => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Gv_N_Ib_REX::new(code1, code2))
		}

		OpCodeHandlerKind::Gv_RX => {
			reg = deserializer.read_register();
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Gv_RX::new(reg, code1, code2))
		}

		OpCodeHandlerKind::Gv_W => {
			reg = deserializer.read_register();
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Gv_W::new(reg, code1, code2))
		}

		OpCodeHandlerKind::GvM_VX_Ib => {
			reg = deserializer.read_register();
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_GvM_VX_Ib::new(reg, code1, code2))
		}

		OpCodeHandlerKind::Ib => box_opcode_handler(OpCodeHandler_Ib::new(deserializer.read_code())),
		OpCodeHandlerKind::Ib3 => box_opcode_handler(OpCodeHandler_Ib3::new(deserializer.read_code())),
		OpCodeHandlerKind::IbReg => box_opcode_handler(OpCodeHandler_IbReg::new(deserializer.read_code(), deserializer.read_register())),

		OpCodeHandlerKind::IbReg2 => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_IbReg2::new(code1, code2))
		}

		OpCodeHandlerKind::Iw_Ib => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Iw_Ib::new(code1, code2, code3))
		}

		OpCodeHandlerKind::Jb => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Jb::new(code1, code2, code3))
		}

		OpCodeHandlerKind::Jb2 => box_opcode_handler(OpCodeHandler_Jb2::new(
			deserializer.read_code(),
			deserializer.read_code(),
			deserializer.read_code(),
			deserializer.read_code(),
			deserializer.read_code(),
			deserializer.read_code(),
			deserializer.read_code(),
		)),

		OpCodeHandlerKind::Jdisp => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Jdisp::new(code1, code2))
		}

		OpCodeHandlerKind::Jx => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Jx::new(code1, code2, deserializer.read_code()))
		}

		OpCodeHandlerKind::Jz => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Jz::new(code1, code2, code3))
		}

		OpCodeHandlerKind::M_1 => box_opcode_handler(OpCodeHandler_M::new(deserializer.read_code())),

		OpCodeHandlerKind::M_2 => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_M::new1(code1, code2))
		}

		OpCodeHandlerKind::M_REXW_2 => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_M_REXW::new(code1, code2, 0, 0))
		}

		OpCodeHandlerKind::M_REXW_4 => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_M_REXW::new(code1, code2, deserializer.read_handler_flags(), deserializer.read_handler_flags()))
		}

		OpCodeHandlerKind::MemBx => box_opcode_handler(OpCodeHandler_MemBx::new(deserializer.read_code())),
		OpCodeHandlerKind::Mf_1 => box_opcode_handler(OpCodeHandler_Mf::new(deserializer.read_code())),

		OpCodeHandlerKind::Mf_2a => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Mf::new1(code1, code2))
		}

		OpCodeHandlerKind::Mf_2b => box_opcode_handler(OpCodeHandler_Mf::new1(deserializer.read_code(), deserializer.read_code())),
		OpCodeHandlerKind::MIB_B => box_opcode_handler(OpCodeHandler_MIB_B::new(deserializer.read_code())),
		OpCodeHandlerKind::MP => box_opcode_handler(OpCodeHandler_MP::new(deserializer.read_code())),

		OpCodeHandlerKind::Ms => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Ms::new(code1, code2, code3))
		}

		OpCodeHandlerKind::MV => box_opcode_handler(OpCodeHandler_MV::new(deserializer.read_register(), deserializer.read_code())),

		OpCodeHandlerKind::Mv_Gv => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Mv_Gv::new(code1, code2, code3))
		}

		OpCodeHandlerKind::Mv_Gv_REXW => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Mv_Gv_REXW::new(code1, code2))
		}

		OpCodeHandlerKind::NIb => box_opcode_handler(OpCodeHandler_NIb::new(deserializer.read_code())),
		OpCodeHandlerKind::Ob_Reg => box_opcode_handler(OpCodeHandler_Ob_Reg::new(deserializer.read_code(), deserializer.read_register())),

		OpCodeHandlerKind::Ov_Reg => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Ov_Reg::new(code1, code2, code3))
		}

		OpCodeHandlerKind::P_Ev => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_P_Ev::new(code1, code2))
		}

		OpCodeHandlerKind::P_Ev_Ib => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_P_Ev_Ib::new(code1, code2))
		}

		OpCodeHandlerKind::P_Q => box_opcode_handler(OpCodeHandler_P_Q::new(deserializer.read_code())),
		OpCodeHandlerKind::P_Q_Ib => box_opcode_handler(OpCodeHandler_P_Q_Ib::new(deserializer.read_code())),
		OpCodeHandlerKind::P_R => box_opcode_handler(OpCodeHandler_P_R::new(deserializer.read_register(), deserializer.read_code())),
		OpCodeHandlerKind::P_W => box_opcode_handler(OpCodeHandler_P_W::new(deserializer.read_register(), deserializer.read_code())),

		OpCodeHandlerKind::PushEv => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_PushEv::new(code1, code2, code3))
		}

		OpCodeHandlerKind::PushIb2 => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_PushIb2::new(code1, code2, code3))
		}

		OpCodeHandlerKind::PushIz => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_PushIz::new(code1, code2, code3))
		}

		OpCodeHandlerKind::PushOpSizeReg_4a => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_PushOpSizeReg::new(code1, code2, code3, deserializer.read_register()))
		}

		OpCodeHandlerKind::PushOpSizeReg_4b => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_PushOpSizeReg::new(code1, code2, Code::INVALID, deserializer.read_register()))
		}

		OpCodeHandlerKind::PushSimple2 => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_PushSimple2::new(code1, code2, code3))
		}

		OpCodeHandlerKind::PushSimpleReg => {
			index = deserializer.read_u32();
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_PushSimpleReg::new(index, code1, code2, code3))
		}

		OpCodeHandlerKind::Q_P => box_opcode_handler(OpCodeHandler_Q_P::new(deserializer.read_code())),

		OpCodeHandlerKind::R_C_3a => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_R_C::new(code1, code2, deserializer.read_register()))
		}

		OpCodeHandlerKind::R_C_3b => {
			box_opcode_handler(OpCodeHandler_R_C::new(deserializer.read_code(), Code::INVALID, deserializer.read_register()))
		}

		OpCodeHandlerKind::rDI_P_N => box_opcode_handler(OpCodeHandler_rDI_P_N::new(deserializer.read_code())),
		OpCodeHandlerKind::rDI_VX_RX => box_opcode_handler(OpCodeHandler_rDI_VX_RX::new(deserializer.read_register(), deserializer.read_code())),
		OpCodeHandlerKind::Reg => box_opcode_handler(OpCodeHandler_Reg::new(deserializer.read_code(), deserializer.read_register())),

		OpCodeHandlerKind::Reg_Ib2 => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Reg_Ib2::new(code1, code2))
		}

		OpCodeHandlerKind::Reg_Iz => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Reg_Iz::new(code1, code2, code3))
		}

		OpCodeHandlerKind::Reg_Ob => box_opcode_handler(OpCodeHandler_Reg_Ob::new(deserializer.read_code(), deserializer.read_register())),

		OpCodeHandlerKind::Reg_Ov => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Reg_Ov::new(code1, code2, code3))
		}

		OpCodeHandlerKind::Reg_Xb => box_opcode_handler(OpCodeHandler_Reg_Xb::new(deserializer.read_code(), deserializer.read_register())),

		OpCodeHandlerKind::Reg_Xv => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Reg_Xv::new(code1, code2, code3))
		}

		OpCodeHandlerKind::Reg_Xv2 => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Reg_Xv2::new(code1, code2))
		}

		OpCodeHandlerKind::Reg_Yb => box_opcode_handler(OpCodeHandler_Reg_Yb::new(deserializer.read_code(), deserializer.read_register())),

		OpCodeHandlerKind::Reg_Yv => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Reg_Yv::new(code1, code2, code3))
		}

		OpCodeHandlerKind::RegIb => box_opcode_handler(OpCodeHandler_RegIb::new(deserializer.read_code(), deserializer.read_register())),
		OpCodeHandlerKind::RegIb3 => box_opcode_handler(OpCodeHandler_RegIb3::new(deserializer.read_u32())),
		OpCodeHandlerKind::RegIz2 => box_opcode_handler(OpCodeHandler_RegIz2::new(deserializer.read_u32())),

		OpCodeHandlerKind::Reservednop => {
			box_opcode_handler(OpCodeHandler_Reservednop::new(deserializer.read_handler(), deserializer.read_handler()))
		}

		OpCodeHandlerKind::RIb => box_opcode_handler(OpCodeHandler_RIb::new(deserializer.read_register(), deserializer.read_code())),
		OpCodeHandlerKind::RIbIb => box_opcode_handler(OpCodeHandler_RIbIb::new(deserializer.read_register(), deserializer.read_code())),

		OpCodeHandlerKind::Rv => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Rv::new(code1, code2, code3))
		}

		OpCodeHandlerKind::Rv_32_64 => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Rv_32_64::new(code1, code2))
		}

		OpCodeHandlerKind::RvMw_Gw => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_RvMw_Gw::new(code1, code2))
		}

		OpCodeHandlerKind::Simple => box_opcode_handler(OpCodeHandler_Simple::new(deserializer.read_code())),
		OpCodeHandlerKind::Simple_ModRM => box_opcode_handler(OpCodeHandler_Simple::new_modrm(deserializer.read_code())),

		OpCodeHandlerKind::Simple2_3a => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Simple2::new(code1, code2, code3))
		}

		OpCodeHandlerKind::Simple2_3b => {
			box_opcode_handler(OpCodeHandler_Simple2::new(deserializer.read_code(), deserializer.read_code(), deserializer.read_code()))
		}

		OpCodeHandlerKind::Simple2Iw => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Simple2Iw::new(code1, code2, code3))
		}

		OpCodeHandlerKind::Simple3 => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Simple3::new(code1, code2, code3))
		}

		OpCodeHandlerKind::Simple4 => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Simple4::new(code1, code2))
		}

		OpCodeHandlerKind::Simple4b => {
			let code1 = deserializer.read_code();
			let code2 = deserializer.read_code();
			box_opcode_handler(OpCodeHandler_Simple4::new(code1, code2))
		}

		OpCodeHandlerKind::Simple5 => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Simple5::new(code1, code2, code3))
		}

		OpCodeHandlerKind::Simple5_ModRM_as => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Simple5_ModRM_as::new(code1, code2, code3))
		}

		OpCodeHandlerKind::SimpleReg => box_opcode_handler(OpCodeHandler_SimpleReg::new(deserializer.read_code(), deserializer.read_u32())),
		OpCodeHandlerKind::ST_STi => box_opcode_handler(OpCodeHandler_ST_STi::new(deserializer.read_code())),
		OpCodeHandlerKind::STi => box_opcode_handler(OpCodeHandler_STi::new(deserializer.read_code())),
		OpCodeHandlerKind::STi_ST => box_opcode_handler(OpCodeHandler_STi_ST::new(deserializer.read_code())),

		OpCodeHandlerKind::Sw_Ev => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Sw_Ev::new(code1, code2, code3))
		}

		OpCodeHandlerKind::V_Ev => {
			reg = deserializer.read_register();
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_V_Ev::new(reg, code1, code2))
		}

		OpCodeHandlerKind::VM => box_opcode_handler(OpCodeHandler_VM::new(deserializer.read_register(), deserializer.read_code())),
		OpCodeHandlerKind::VN => box_opcode_handler(OpCodeHandler_VN::new(deserializer.read_register(), deserializer.read_code())),
		OpCodeHandlerKind::VQ => box_opcode_handler(OpCodeHandler_VQ::new(deserializer.read_register(), deserializer.read_code())),
		OpCodeHandlerKind::VRIbIb => box_opcode_handler(OpCodeHandler_VRIbIb::new(deserializer.read_register(), deserializer.read_code())),
		OpCodeHandlerKind::VW_2 => box_opcode_handler(OpCodeHandler_VW::new(deserializer.read_register(), deserializer.read_code())),

		OpCodeHandlerKind::VW_3 => {
			box_opcode_handler(OpCodeHandler_VW::new1(deserializer.read_register(), deserializer.read_code(), deserializer.read_code()))
		}

		OpCodeHandlerKind::VWIb_2 => box_opcode_handler(OpCodeHandler_VWIb::new(deserializer.read_register(), deserializer.read_code())),

		OpCodeHandlerKind::VWIb_3 => {
			reg = deserializer.read_register();
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_VWIb::new1(reg, code1, code2))
		}

		OpCodeHandlerKind::VX_E_Ib => {
			reg = deserializer.read_register();
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_VX_E_Ib::new(reg, code1, code2))
		}

		OpCodeHandlerKind::VX_Ev => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_VX_Ev::new(code1, code2))
		}

		OpCodeHandlerKind::Wbinvd => box_opcode_handler(OpCodeHandler_Wbinvd::new()),
		OpCodeHandlerKind::WV => box_opcode_handler(OpCodeHandler_WV::new(deserializer.read_register(), deserializer.read_code())),
		OpCodeHandlerKind::Xb_Yb => box_opcode_handler(OpCodeHandler_Xb_Yb::new(deserializer.read_code())),
		OpCodeHandlerKind::Xchg_Reg_rAX => box_opcode_handler(OpCodeHandler_Xchg_Reg_rAX::new(deserializer.read_u32())),

		OpCodeHandlerKind::Xv_Yv => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Xv_Yv::new(code1, code2, code3))
		}

		OpCodeHandlerKind::Yb_Reg => box_opcode_handler(OpCodeHandler_Yb_Reg::new(deserializer.read_code(), deserializer.read_register())),
		OpCodeHandlerKind::Yb_Xb => box_opcode_handler(OpCodeHandler_Yb_Xb::new(deserializer.read_code())),

		OpCodeHandlerKind::Yv_Reg => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Yv_Reg::new(code1, code2, code3))
		}

		OpCodeHandlerKind::Yv_Reg2 => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Yv_Reg2::new(code1, code2))
		}

		OpCodeHandlerKind::Yv_Xv => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Yv_Xv::new(code1, code2, code3))
		}

		OpCodeHandlerKind::M_Sw => {
			let code = deserializer.read_code();
			box_opcode_handler(OpCodeHandler_M_Sw::new(code))
		}

		OpCodeHandlerKind::Sw_M => {
			let code = deserializer.read_code();
			box_opcode_handler(OpCodeHandler_Sw_M::new(code))
		}

		OpCodeHandlerKind::Rq => box_opcode_handler(OpCodeHandler_Rq::new(deserializer.read_code())),
		OpCodeHandlerKind::Gd_Rd => box_opcode_handler(OpCodeHandler_Gd_Rd::new(deserializer.read_code())),
		OpCodeHandlerKind::PrefixEsCsSsDs => box_opcode_handler(OpCodeHandler_PrefixEsCsSsDs::new(deserializer.read_register())),
		OpCodeHandlerKind::PrefixFsGs => box_opcode_handler(OpCodeHandler_PrefixFsGs::new(deserializer.read_register())),
		OpCodeHandlerKind::Prefix66 => box_opcode_handler(OpCodeHandler_Prefix66::new()),
		OpCodeHandlerKind::Prefix67 => box_opcode_handler(OpCodeHandler_Prefix67::new()),
		OpCodeHandlerKind::PrefixF0 => box_opcode_handler(OpCodeHandler_PrefixF0::new()),
		OpCodeHandlerKind::PrefixF2 => box_opcode_handler(OpCodeHandler_PrefixF2::new()),
		OpCodeHandlerKind::PrefixF3 => box_opcode_handler(OpCodeHandler_PrefixF3::new()),
		OpCodeHandlerKind::PrefixREX => box_opcode_handler(OpCodeHandler_PrefixREX::new(deserializer.read_handler(), deserializer.read_u32())),
	};
	let handler = unsafe { &*handler_ptr };
	result.push((decode, handler));
}
