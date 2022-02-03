// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::decoder::handlers::d3now::*;
use crate::decoder::handlers::fpu::*;
use crate::decoder::handlers::legacy::*;
use crate::decoder::handlers::{
	get_invalid_handler, get_invalid_no_modrm_handler, get_null_handler, OpCodeHandler, OpCodeHandlerDecodeFn, OpCodeHandler_AnotherTable,
	OpCodeHandler_Bitness, OpCodeHandler_Bitness_DontReadModRM, OpCodeHandler_Group, OpCodeHandler_Group8x64, OpCodeHandler_Group8x8,
	OpCodeHandler_Int3, OpCodeHandler_Options, OpCodeHandler_Options1632, OpCodeHandler_Options_DontReadModRM, OpCodeHandler_RM,
	OpCodeHandler_Simple,
};
use crate::decoder::table_de::enums::*;
use crate::decoder::table_de::{box_opcode_handler, TableDeserializer};
use crate::decoder::Code;
use alloc::vec::Vec;

#[allow(trivial_casts)]
pub(super) fn read_handlers(deserializer: &mut TableDeserializer<'_>, result: &mut Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>) {
	let index;
	let (decode, handler_ptr): (OpCodeHandlerDecodeFn, *const OpCodeHandler) = match deserializer.read_legacy_op_code_handler_kind() {
		LegacyOpCodeHandlerKind::Bitness => box_opcode_handler(OpCodeHandler_Bitness::new(deserializer.read_handler(), deserializer.read_handler())),

		LegacyOpCodeHandlerKind::Bitness_DontReadModRM => {
			box_opcode_handler(OpCodeHandler_Bitness_DontReadModRM::new(deserializer.read_handler(), deserializer.read_handler()))
		}

		LegacyOpCodeHandlerKind::Invalid => {
			result.push(get_invalid_handler());
			return;
		}

		LegacyOpCodeHandlerKind::Invalid_NoModRM => {
			result.push(get_invalid_no_modrm_handler());
			return;
		}

		LegacyOpCodeHandlerKind::Invalid2 => {
			result.push(get_invalid_handler());
			result.push(get_invalid_handler());
			return;
		}

		LegacyOpCodeHandlerKind::Dup => {
			let count = deserializer.read_u32();
			let handler = deserializer.read_handler_or_null_instance();
			for _ in 0..count {
				result.push(handler);
			}
			return;
		}

		LegacyOpCodeHandlerKind::Null => {
			result.push(get_null_handler());
			return;
		}

		LegacyOpCodeHandlerKind::HandlerReference => {
			result.push(deserializer.read_handler_reference());
			return;
		}

		LegacyOpCodeHandlerKind::ArrayReference => unreachable!(),
		LegacyOpCodeHandlerKind::RM => box_opcode_handler(OpCodeHandler_RM::new(deserializer.read_handler(), deserializer.read_handler())),

		LegacyOpCodeHandlerKind::Options1632_1 => box_opcode_handler(OpCodeHandler_Options1632::new(
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_decoder_options(),
		)),

		LegacyOpCodeHandlerKind::Options1632_2 => box_opcode_handler(OpCodeHandler_Options1632::new2(
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_decoder_options(),
			deserializer.read_handler(),
			deserializer.read_decoder_options(),
		)),

		LegacyOpCodeHandlerKind::Options3 => box_opcode_handler(OpCodeHandler_Options::new(
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_decoder_options(),
		)),

		LegacyOpCodeHandlerKind::Options5 => box_opcode_handler(OpCodeHandler_Options::new2(
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_decoder_options(),
			deserializer.read_handler(),
			deserializer.read_decoder_options(),
		)),

		LegacyOpCodeHandlerKind::Options_DontReadModRM => box_opcode_handler(OpCodeHandler_Options_DontReadModRM::new(
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_decoder_options(),
		)),

		LegacyOpCodeHandlerKind::AnotherTable => box_opcode_handler(OpCodeHandler_AnotherTable::new(
			deserializer.read_array_reference_no_clone(LegacyOpCodeHandlerKind::ArrayReference as u32),
		)),

		LegacyOpCodeHandlerKind::Group => {
			box_opcode_handler(OpCodeHandler_Group::new(deserializer.read_array_reference(LegacyOpCodeHandlerKind::ArrayReference as u32)))
		}

		LegacyOpCodeHandlerKind::Group8x64 => box_opcode_handler(OpCodeHandler_Group8x64::new(
			deserializer.read_array_reference(LegacyOpCodeHandlerKind::ArrayReference as u32),
			deserializer.read_array_reference(LegacyOpCodeHandlerKind::ArrayReference as u32),
		)),

		LegacyOpCodeHandlerKind::Group8x8 => box_opcode_handler(OpCodeHandler_Group8x8::new(
			deserializer.read_array_reference(LegacyOpCodeHandlerKind::ArrayReference as u32),
			deserializer.read_array_reference(LegacyOpCodeHandlerKind::ArrayReference as u32),
		)),

		LegacyOpCodeHandlerKind::MandatoryPrefix => box_opcode_handler(OpCodeHandler_MandatoryPrefix::new(
			true,
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
		)),

		LegacyOpCodeHandlerKind::MandatoryPrefix4 => box_opcode_handler(OpCodeHandler_MandatoryPrefix4::new(
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_u32(),
		)),

		LegacyOpCodeHandlerKind::MandatoryPrefix_NoModRM => box_opcode_handler(OpCodeHandler_MandatoryPrefix::new(
			false,
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
		)),

		LegacyOpCodeHandlerKind::MandatoryPrefix3 => box_opcode_handler(OpCodeHandler_MandatoryPrefix3::new(
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

		LegacyOpCodeHandlerKind::D3NOW => box_opcode_handler(OpCodeHandler_D3NOW::new()),
		LegacyOpCodeHandlerKind::EVEX => box_opcode_handler(OpCodeHandler_EVEX::new(deserializer.read_handler())),
		LegacyOpCodeHandlerKind::VEX2 => box_opcode_handler(OpCodeHandler_VEX2::new(deserializer.read_handler())),
		LegacyOpCodeHandlerKind::VEX3 => box_opcode_handler(OpCodeHandler_VEX3::new(deserializer.read_handler())),
		LegacyOpCodeHandlerKind::XOP => box_opcode_handler(OpCodeHandler_XOP::new(deserializer.read_handler())),
		LegacyOpCodeHandlerKind::AL_DX => box_opcode_handler(OpCodeHandler_AL_DX::new(deserializer.read_code())),

		LegacyOpCodeHandlerKind::Ap => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Ap::new(code1, code2))
		}

		LegacyOpCodeHandlerKind::B_BM => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_B_BM::new(code1, code2))
		}

		LegacyOpCodeHandlerKind::B_Ev => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_B_Ev::new(code1, code2, deserializer.read_boolean()))
		}

		LegacyOpCodeHandlerKind::B_MIB => box_opcode_handler(OpCodeHandler_B_MIB::new(deserializer.read_code())),

		LegacyOpCodeHandlerKind::BM_B => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_BM_B::new(code1, code2))
		}

		LegacyOpCodeHandlerKind::BranchIw => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_BranchIw::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::BranchSimple => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_BranchSimple::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::C_R_3a => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_C_R::new(code1, code2, deserializer.read_register()))
		}

		LegacyOpCodeHandlerKind::C_R_3b => {
			box_opcode_handler(OpCodeHandler_C_R::new(deserializer.read_code(), Code::INVALID, deserializer.read_register()))
		}

		LegacyOpCodeHandlerKind::DX_AL => box_opcode_handler(OpCodeHandler_DX_AL::new(deserializer.read_code())),

		LegacyOpCodeHandlerKind::DX_eAX => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_DX_eAX::new(code1, code2))
		}

		LegacyOpCodeHandlerKind::eAX_DX => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_eAX_DX::new(code1, code2))
		}

		LegacyOpCodeHandlerKind::Eb_1 => box_opcode_handler(OpCodeHandler_Eb::new(deserializer.read_code(), 0)),
		LegacyOpCodeHandlerKind::Eb_2 => box_opcode_handler(OpCodeHandler_Eb::new(deserializer.read_code(), deserializer.read_handler_flags())),
		LegacyOpCodeHandlerKind::Eb_CL => box_opcode_handler(OpCodeHandler_Eb_CL::new(deserializer.read_code())),
		LegacyOpCodeHandlerKind::Eb_Gb_1 => box_opcode_handler(OpCodeHandler_Eb_Gb::new(deserializer.read_code(), 0)),
		LegacyOpCodeHandlerKind::Eb_Gb_2 => box_opcode_handler(OpCodeHandler_Eb_Gb::new(deserializer.read_code(), deserializer.read_handler_flags())),
		LegacyOpCodeHandlerKind::Eb_Ib_1 => box_opcode_handler(OpCodeHandler_Eb_Ib::new(deserializer.read_code(), 0)),
		LegacyOpCodeHandlerKind::Eb_Ib_2 => box_opcode_handler(OpCodeHandler_Eb_Ib::new(deserializer.read_code(), deserializer.read_handler_flags())),
		LegacyOpCodeHandlerKind::Eb1 => box_opcode_handler(OpCodeHandler_Eb_1::new(deserializer.read_code())),

		LegacyOpCodeHandlerKind::Ed_V_Ib => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Ed_V_Ib::new(code1, code2))
		}

		LegacyOpCodeHandlerKind::Ep => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Ep::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::Ev_3a => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Ev::new(code1, code2, code3, 0))
		}

		LegacyOpCodeHandlerKind::Ev_3b => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Ev::new(code1, code2, Code::INVALID, 0))
		}

		LegacyOpCodeHandlerKind::Ev_4 => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Ev::new(code1, code2, code3, deserializer.read_handler_flags()))
		}

		LegacyOpCodeHandlerKind::Ev_CL => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Ev_CL::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::Ev_Gv_32_64 => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Ev_Gv_32_64::new(code1, code2))
		}

		LegacyOpCodeHandlerKind::Ev_Gv_3a => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Ev_Gv::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::Ev_Gv_3b => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Ev_Gv::new(code1, code2, Code::INVALID))
		}

		LegacyOpCodeHandlerKind::Ev_Gv_4 => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Ev_Gv_flags::new(code1, code2, code3, deserializer.read_handler_flags()))
		}

		LegacyOpCodeHandlerKind::Ev_Gv_CL => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Ev_Gv_CL::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::Ev_Gv_Ib => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Ev_Gv_Ib::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::Ev_Gv_REX => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Ev_Gv_REX::new(code1, code2))
		}

		LegacyOpCodeHandlerKind::Ev_Ib_3 => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Ev_Ib::new(code1, code2, code3, 0))
		}

		LegacyOpCodeHandlerKind::Ev_Ib_4 => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Ev_Ib::new(code1, code2, code3, deserializer.read_handler_flags()))
		}

		LegacyOpCodeHandlerKind::Ev_Ib2_3 => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Ev_Ib2::new(code1, code2, code3, 0))
		}

		LegacyOpCodeHandlerKind::Ev_Ib2_4 => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Ev_Ib2::new(code1, code2, code3, deserializer.read_handler_flags()))
		}

		LegacyOpCodeHandlerKind::Ev_Iz_3 => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Ev_Iz::new(code1, code2, code3, 0))
		}

		LegacyOpCodeHandlerKind::Ev_Iz_4 => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Ev_Iz::new(code1, code2, code3, deserializer.read_handler_flags()))
		}

		LegacyOpCodeHandlerKind::Ev_P => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Ev_P::new(code1, code2))
		}

		LegacyOpCodeHandlerKind::Ev_REXW_1a => {
			let code = deserializer.read_code();
			box_opcode_handler(OpCodeHandler_Ev_REXW::new(code, Code::INVALID, deserializer.read_u32()))
		}

		LegacyOpCodeHandlerKind::Ev_REXW => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Ev_REXW::new(code1, code2, deserializer.read_u32()))
		}

		LegacyOpCodeHandlerKind::Ev_Sw => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Ev_Sw::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::Ev_VX => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Ev_VX::new(code1, code2))
		}

		LegacyOpCodeHandlerKind::Ev1 => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Ev_1::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::Evj => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Evj::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::Evw => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Evw::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::Ew => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Ew::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::Gb_Eb => box_opcode_handler(OpCodeHandler_Gb_Eb::new(deserializer.read_code())),

		LegacyOpCodeHandlerKind::Gdq_Ev => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Gdq_Ev::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::Gv_Eb => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Gv_Eb::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::Gv_Eb_REX => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Gv_Eb_REX::new(code1, code2))
		}

		LegacyOpCodeHandlerKind::Gv_Ev_32_64 => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Gv_Ev_32_64::new(code1, code2, deserializer.read_boolean(), deserializer.read_boolean()))
		}

		LegacyOpCodeHandlerKind::Gv_Ev_3a => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Gv_Ev::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::Gv_Ev_3b => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Gv_Ev::new(code1, code2, Code::INVALID))
		}

		LegacyOpCodeHandlerKind::Gv_Ev_Ib => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Gv_Ev_Ib::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::Gv_Ev_Ib_REX => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Gv_Ev_Ib_REX::new(code1, code2))
		}

		LegacyOpCodeHandlerKind::Gv_Ev_Iz => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Gv_Ev_Iz::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::Gv_Ev_REX => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Gv_Ev_REX::new(code1, code2))
		}

		LegacyOpCodeHandlerKind::Gv_Ev2 => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Gv_Ev2::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::Gv_Ev3 => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Gv_Ev3::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::Gv_Ew => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Gv_Ew::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::Gv_M => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Gv_M::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::Gv_M_as => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Gv_M_as::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::Gv_Ma => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Gv_Ma::new(code1, code2))
		}

		LegacyOpCodeHandlerKind::Gv_Mp_2 => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Gv_Mp::new(code1, code2, Code::INVALID))
		}

		LegacyOpCodeHandlerKind::Gv_Mp_3 => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Gv_Mp::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::Gv_Mv => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Gv_Mv::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::Gv_N => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Gv_N::new(code1, code2))
		}

		LegacyOpCodeHandlerKind::Gv_N_Ib_REX => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Gv_N_Ib_REX::new(code1, code2))
		}

		LegacyOpCodeHandlerKind::Gv_RX => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Gv_RX::new(code1, code2))
		}

		LegacyOpCodeHandlerKind::Gv_W => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Gv_W::new(code1, code2))
		}

		LegacyOpCodeHandlerKind::GvM_VX_Ib => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_GvM_VX_Ib::new(code1, code2))
		}

		LegacyOpCodeHandlerKind::Ib => box_opcode_handler(OpCodeHandler_Ib::new(deserializer.read_code())),
		LegacyOpCodeHandlerKind::Ib3 => box_opcode_handler(OpCodeHandler_Ib3::new(deserializer.read_code())),
		LegacyOpCodeHandlerKind::IbReg => box_opcode_handler(OpCodeHandler_IbReg::new(deserializer.read_code(), deserializer.read_register())),

		LegacyOpCodeHandlerKind::IbReg2 => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_IbReg2::new(code1, code2))
		}

		LegacyOpCodeHandlerKind::Iw_Ib => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Iw_Ib::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::Jb => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Jb::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::Jb2 => box_opcode_handler(OpCodeHandler_Jb2::new(
			deserializer.read_code(),
			deserializer.read_code(),
			deserializer.read_code(),
			deserializer.read_code(),
			deserializer.read_code(),
			deserializer.read_code(),
			deserializer.read_code(),
		)),

		LegacyOpCodeHandlerKind::Jdisp => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Jdisp::new(code1, code2))
		}

		LegacyOpCodeHandlerKind::Jx => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Jx::new(code1, code2, deserializer.read_code()))
		}

		LegacyOpCodeHandlerKind::Jz => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Jz::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::M_1 => box_opcode_handler(OpCodeHandler_M::new(deserializer.read_code())),

		LegacyOpCodeHandlerKind::M_2 => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_M::new1(code1, code2))
		}

		LegacyOpCodeHandlerKind::M_REXW_2 => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_M_REXW::new(code1, code2, 0, 0))
		}

		LegacyOpCodeHandlerKind::M_REXW_4 => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_M_REXW::new(code1, code2, deserializer.read_handler_flags(), deserializer.read_handler_flags()))
		}

		LegacyOpCodeHandlerKind::MemBx => box_opcode_handler(OpCodeHandler_MemBx::new(deserializer.read_code())),
		LegacyOpCodeHandlerKind::Mf_1 => box_opcode_handler(OpCodeHandler_Mf::new(deserializer.read_code())),

		LegacyOpCodeHandlerKind::Mf_2a => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Mf::new1(code1, code2))
		}

		LegacyOpCodeHandlerKind::Mf_2b => box_opcode_handler(OpCodeHandler_Mf::new1(deserializer.read_code(), deserializer.read_code())),
		LegacyOpCodeHandlerKind::MIB_B => box_opcode_handler(OpCodeHandler_MIB_B::new(deserializer.read_code())),
		LegacyOpCodeHandlerKind::MP => box_opcode_handler(OpCodeHandler_MP::new(deserializer.read_code())),

		LegacyOpCodeHandlerKind::Ms => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Ms::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::MV => box_opcode_handler(OpCodeHandler_MV::new(deserializer.read_code())),

		LegacyOpCodeHandlerKind::Mv_Gv => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Mv_Gv::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::Mv_Gv_REXW => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Mv_Gv_REXW::new(code1, code2))
		}

		LegacyOpCodeHandlerKind::NIb => box_opcode_handler(OpCodeHandler_NIb::new(deserializer.read_code())),
		LegacyOpCodeHandlerKind::Ob_Reg => box_opcode_handler(OpCodeHandler_Ob_Reg::new(deserializer.read_code(), deserializer.read_register())),

		LegacyOpCodeHandlerKind::Ov_Reg => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Ov_Reg::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::P_Ev => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_P_Ev::new(code1, code2))
		}

		LegacyOpCodeHandlerKind::P_Ev_Ib => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_P_Ev_Ib::new(code1, code2))
		}

		LegacyOpCodeHandlerKind::P_Q => box_opcode_handler(OpCodeHandler_P_Q::new(deserializer.read_code())),
		LegacyOpCodeHandlerKind::P_Q_Ib => box_opcode_handler(OpCodeHandler_P_Q_Ib::new(deserializer.read_code())),
		LegacyOpCodeHandlerKind::P_R => box_opcode_handler(OpCodeHandler_P_R::new(deserializer.read_code())),
		LegacyOpCodeHandlerKind::P_W => box_opcode_handler(OpCodeHandler_P_W::new(deserializer.read_code())),

		LegacyOpCodeHandlerKind::PushEv => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_PushEv::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::PushIb2 => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_PushIb2::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::PushIz => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_PushIz::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::PushOpSizeReg_4a => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_PushOpSizeReg::new(code1, code2, code3, deserializer.read_register()))
		}

		LegacyOpCodeHandlerKind::PushOpSizeReg_4b => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_PushOpSizeReg::new(code1, code2, Code::INVALID, deserializer.read_register()))
		}

		LegacyOpCodeHandlerKind::PushSimple2 => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_PushSimple2::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::PushSimpleReg => {
			index = deserializer.read_u32();
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_PushSimpleReg::new(index, code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::Q_P => box_opcode_handler(OpCodeHandler_Q_P::new(deserializer.read_code())),

		LegacyOpCodeHandlerKind::R_C_3a => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_R_C::new(code1, code2, deserializer.read_register()))
		}

		LegacyOpCodeHandlerKind::R_C_3b => {
			box_opcode_handler(OpCodeHandler_R_C::new(deserializer.read_code(), Code::INVALID, deserializer.read_register()))
		}

		LegacyOpCodeHandlerKind::rDI_P_N => box_opcode_handler(OpCodeHandler_rDI_P_N::new(deserializer.read_code())),
		LegacyOpCodeHandlerKind::rDI_VX_RX => box_opcode_handler(OpCodeHandler_rDI_VX_RX::new(deserializer.read_code())),
		LegacyOpCodeHandlerKind::Reg => box_opcode_handler(OpCodeHandler_Reg::new(deserializer.read_code(), deserializer.read_register())),

		LegacyOpCodeHandlerKind::Reg_Ib2 => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Reg_Ib2::new(code1, code2))
		}

		LegacyOpCodeHandlerKind::Reg_Iz => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Reg_Iz::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::Reg_Ob => box_opcode_handler(OpCodeHandler_Reg_Ob::new(deserializer.read_code(), deserializer.read_register())),

		LegacyOpCodeHandlerKind::Reg_Ov => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Reg_Ov::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::Reg_Xb => box_opcode_handler(OpCodeHandler_Reg_Xb::new(deserializer.read_code(), deserializer.read_register())),

		LegacyOpCodeHandlerKind::Reg_Xv => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Reg_Xv::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::Reg_Xv2 => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Reg_Xv2::new(code1, code2))
		}

		LegacyOpCodeHandlerKind::Reg_Yb => box_opcode_handler(OpCodeHandler_Reg_Yb::new(deserializer.read_code(), deserializer.read_register())),

		LegacyOpCodeHandlerKind::Reg_Yv => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Reg_Yv::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::RegIb => box_opcode_handler(OpCodeHandler_RegIb::new(deserializer.read_code(), deserializer.read_register())),
		LegacyOpCodeHandlerKind::RegIb3 => box_opcode_handler(OpCodeHandler_RegIb3::new(deserializer.read_u32())),
		LegacyOpCodeHandlerKind::RegIz2 => box_opcode_handler(OpCodeHandler_RegIz2::new(deserializer.read_u32())),

		LegacyOpCodeHandlerKind::Reservednop => {
			box_opcode_handler(OpCodeHandler_Reservednop::new(deserializer.read_handler(), deserializer.read_handler()))
		}

		LegacyOpCodeHandlerKind::RIb => box_opcode_handler(OpCodeHandler_RIb::new(deserializer.read_code())),
		LegacyOpCodeHandlerKind::RIbIb => box_opcode_handler(OpCodeHandler_RIbIb::new(deserializer.read_code())),

		LegacyOpCodeHandlerKind::Rv => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Rv::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::Rv_32_64 => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Rv_32_64::new(code1, code2))
		}

		LegacyOpCodeHandlerKind::RvMw_Gw => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_RvMw_Gw::new(code1, code2))
		}

		LegacyOpCodeHandlerKind::Simple => {
			let code = deserializer.read_code();
			if code == Code::Int3 {
				box_opcode_handler(OpCodeHandler_Int3::new())
			} else {
				box_opcode_handler(OpCodeHandler_Simple::new(code))
			}
		}
		LegacyOpCodeHandlerKind::Simple_ModRM => box_opcode_handler(OpCodeHandler_Simple::new_modrm(deserializer.read_code())),

		LegacyOpCodeHandlerKind::Simple2_3a => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Simple2::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::Simple2_3b => {
			box_opcode_handler(OpCodeHandler_Simple2::new(deserializer.read_code(), deserializer.read_code(), deserializer.read_code()))
		}

		LegacyOpCodeHandlerKind::Simple2Iw => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Simple2Iw::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::Simple3 => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Simple3::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::Simple4 => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Simple4::new(code1, code2))
		}

		LegacyOpCodeHandlerKind::Simple4b => {
			let code1 = deserializer.read_code();
			let code2 = deserializer.read_code();
			box_opcode_handler(OpCodeHandler_Simple4::new(code1, code2))
		}

		LegacyOpCodeHandlerKind::Simple5 => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Simple5::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::Simple5_a32 => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Simple5_a32::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::Simple5_ModRM_as => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Simple5_ModRM_as::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::SimpleReg => box_opcode_handler(OpCodeHandler_SimpleReg::new(deserializer.read_code(), deserializer.read_u32())),
		LegacyOpCodeHandlerKind::ST_STi => box_opcode_handler(OpCodeHandler_ST_STi::new(deserializer.read_code())),
		LegacyOpCodeHandlerKind::STi => box_opcode_handler(OpCodeHandler_STi::new(deserializer.read_code())),
		LegacyOpCodeHandlerKind::STi_ST => box_opcode_handler(OpCodeHandler_STi_ST::new(deserializer.read_code())),

		LegacyOpCodeHandlerKind::Sw_Ev => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Sw_Ev::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::V_Ev => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_V_Ev::new(code1, code2))
		}

		LegacyOpCodeHandlerKind::VM => box_opcode_handler(OpCodeHandler_VM::new(deserializer.read_code())),
		LegacyOpCodeHandlerKind::VN => box_opcode_handler(OpCodeHandler_VN::new(deserializer.read_code())),
		LegacyOpCodeHandlerKind::VQ => box_opcode_handler(OpCodeHandler_VQ::new(deserializer.read_code())),
		LegacyOpCodeHandlerKind::VRIbIb => box_opcode_handler(OpCodeHandler_VRIbIb::new(deserializer.read_code())),
		LegacyOpCodeHandlerKind::VW_2 => box_opcode_handler(OpCodeHandler_VW::new(deserializer.read_code())),
		LegacyOpCodeHandlerKind::VW_3 => box_opcode_handler(OpCodeHandler_VW::new1(deserializer.read_code(), deserializer.read_code())),
		LegacyOpCodeHandlerKind::VWIb_2 => box_opcode_handler(OpCodeHandler_VWIb::new(deserializer.read_code())),

		LegacyOpCodeHandlerKind::VWIb_3 => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_VWIb::new1(code1, code2))
		}

		LegacyOpCodeHandlerKind::VX_E_Ib => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_VX_E_Ib::new(code1, code2))
		}

		LegacyOpCodeHandlerKind::VX_Ev => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_VX_Ev::new(code1, code2))
		}

		LegacyOpCodeHandlerKind::Wbinvd => box_opcode_handler(OpCodeHandler_Wbinvd::new()),
		LegacyOpCodeHandlerKind::WV => box_opcode_handler(OpCodeHandler_WV::new(deserializer.read_code())),
		LegacyOpCodeHandlerKind::Xb_Yb => box_opcode_handler(OpCodeHandler_Xb_Yb::new(deserializer.read_code())),
		LegacyOpCodeHandlerKind::Xchg_Reg_rAX => box_opcode_handler(OpCodeHandler_Xchg_Reg_rAX::new(deserializer.read_u32())),

		LegacyOpCodeHandlerKind::Xv_Yv => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Xv_Yv::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::Yb_Reg => box_opcode_handler(OpCodeHandler_Yb_Reg::new(deserializer.read_code(), deserializer.read_register())),
		LegacyOpCodeHandlerKind::Yb_Xb => box_opcode_handler(OpCodeHandler_Yb_Xb::new(deserializer.read_code())),

		LegacyOpCodeHandlerKind::Yv_Reg => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Yv_Reg::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::Yv_Reg2 => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_Yv_Reg2::new(code1, code2))
		}

		LegacyOpCodeHandlerKind::Yv_Xv => {
			let (code1, code2, code3) = deserializer.read_code3();
			box_opcode_handler(OpCodeHandler_Yv_Xv::new(code1, code2, code3))
		}

		LegacyOpCodeHandlerKind::M_Sw => {
			let code = deserializer.read_code();
			box_opcode_handler(OpCodeHandler_M_Sw::new(code))
		}

		LegacyOpCodeHandlerKind::Sw_M => {
			let code = deserializer.read_code();
			box_opcode_handler(OpCodeHandler_Sw_M::new(code))
		}

		LegacyOpCodeHandlerKind::Rq => box_opcode_handler(OpCodeHandler_Rq::new(deserializer.read_code())),
		LegacyOpCodeHandlerKind::Gd_Rd => box_opcode_handler(OpCodeHandler_Gd_Rd::new(deserializer.read_code())),
		LegacyOpCodeHandlerKind::PrefixEsCsSsDs => box_opcode_handler(OpCodeHandler_PrefixEsCsSsDs::new(deserializer.read_register())),
		LegacyOpCodeHandlerKind::PrefixFsGs => box_opcode_handler(OpCodeHandler_PrefixFsGs::new(deserializer.read_register())),
		LegacyOpCodeHandlerKind::Prefix66 => box_opcode_handler(OpCodeHandler_Prefix66::new()),
		LegacyOpCodeHandlerKind::Prefix67 => box_opcode_handler(OpCodeHandler_Prefix67::new()),
		LegacyOpCodeHandlerKind::PrefixF0 => box_opcode_handler(OpCodeHandler_PrefixF0::new()),
		LegacyOpCodeHandlerKind::PrefixF2 => box_opcode_handler(OpCodeHandler_PrefixF2::new()),
		LegacyOpCodeHandlerKind::PrefixF3 => box_opcode_handler(OpCodeHandler_PrefixF3::new()),
		LegacyOpCodeHandlerKind::PrefixREX => box_opcode_handler(OpCodeHandler_PrefixREX::new(deserializer.read_handler(), deserializer.read_u32())),
	};
	let handler = unsafe { &*handler_ptr };
	result.push((decode, handler));
}
