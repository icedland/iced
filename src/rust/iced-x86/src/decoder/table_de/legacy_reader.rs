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

use super::super::handlers::OpCodeHandler;
use super::super::handlers::*;
use super::super::handlers_3dnow::*;
use super::super::handlers_fpu::*;
use super::super::handlers_legacy::*;
use super::super::Code;
use super::enums::*;
use super::TableDeserializer;
#[cfg(not(feature = "std"))]
use alloc::boxed::Box;
#[cfg(not(feature = "std"))]
use alloc::vec::Vec;

#[allow(trivial_casts)]
pub(super) fn read_handlers(deserializer: &mut TableDeserializer, result: &mut Vec<&'static OpCodeHandler>) {
	let code;
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
			result.push(unsafe { &*(&INVALID_HANDLER as *const _ as *const OpCodeHandler) });
			&INVALID_HANDLER as *const _ as *const OpCodeHandler
		}

		OpCodeHandlerKind::Dup => {
			let count = deserializer.read_u32();
			let handler = deserializer.read_handler_or_null_instance();
			for _ in 0..count {
				result.push(unsafe { &*handler });
			}
			return;
		}

		OpCodeHandlerKind::Null => &NULL_HANDLER as *const _ as *const OpCodeHandler,
		OpCodeHandlerKind::HandlerReference => deserializer.read_handler_reference(),
		OpCodeHandlerKind::ArrayReference => unreachable!(),

		OpCodeHandlerKind::RM => {
			Box::into_raw(Box::new(OpCodeHandler_RM::new(deserializer.read_handler(), deserializer.read_handler()))) as *const OpCodeHandler
		}

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

		OpCodeHandlerKind::MandatoryPrefix_F3_F2 => Box::into_raw(Box::new(OpCodeHandler_MandatoryPrefix_F3_F2::new(
			deserializer.read_handler(),
			deserializer.read_handler(),
			true,
			deserializer.read_handler(),
			true,
		))) as *const OpCodeHandler,

		OpCodeHandlerKind::LegacyMandatoryPrefix_F3_F2 => Box::into_raw(Box::new(OpCodeHandler_MandatoryPrefix_F3_F2::new(
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_boolean(),
			deserializer.read_handler(),
			deserializer.read_boolean(),
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
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Ap::new(code, code + 1))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::B_BM => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_B_BM::new(code, code + 1))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::B_Ev => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_B_Ev::new(code, code + 1, deserializer.read_boolean()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::B_MIB => Box::into_raw(Box::new(OpCodeHandler_B_MIB::new(deserializer.read_code()))) as *const OpCodeHandler,

		OpCodeHandlerKind::BM_B => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_BM_B::new(code, code + 1))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::BranchIw => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_BranchIw::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::BranchSimple => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_BranchSimple::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::C_R_3a => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_C_R::new(code, code + 1, deserializer.read_register()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::C_R_3b => {
			Box::into_raw(Box::new(OpCodeHandler_C_R::new(deserializer.read_code(), Code::INVALID as u32, deserializer.read_register())))
				as *const OpCodeHandler
		}

		OpCodeHandlerKind::DX_AL => Box::into_raw(Box::new(OpCodeHandler_DX_AL::new(deserializer.read_code()))) as *const OpCodeHandler,

		OpCodeHandlerKind::DX_eAX => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_DX_eAX::new(code, code + 1))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::eAX_DX => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_eAX_DX::new(code, code + 1))) as *const OpCodeHandler
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
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Ed_V_Ib::new(reg, code, code + 1))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ep => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Ep::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_3a => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Ev::new(code, code + 1, code + 2, 0))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_3b => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Ev::new(code, code + 1, Code::INVALID as u32, 0))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_4 => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Ev::new(code, code + 1, code + 2, deserializer.read_handler_flags()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_CL => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Ev_CL::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_Gv_32_64 => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Ev_Gv_32_64::new(code, code + 1))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_Gv_3a => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Ev_Gv::new(code, code + 1, code + 2, 0))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_Gv_3b => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Ev_Gv::new(code, code + 1, Code::INVALID as u32, 0))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_Gv_4 => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Ev_Gv::new(code, code + 1, code + 2, deserializer.read_handler_flags()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_Gv_CL => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Ev_Gv_CL::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_Gv_Ib => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Ev_Gv_Ib::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_Gv_REX => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Ev_Gv_REX::new(code, code + 1))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_Ib_3 => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Ev_Ib::new(code, code + 1, code + 2, 0))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_Ib_4 => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Ev_Ib::new(code, code + 1, code + 2, deserializer.read_handler_flags()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_Ib2_3 => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Ev_Ib2::new(code, code + 1, code + 2, 0))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_Ib2_4 => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Ev_Ib2::new(code, code + 1, code + 2, deserializer.read_handler_flags()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_Iz_3 => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Ev_Iz::new(code, code + 1, code + 2, 0))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_Iz_4 => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Ev_Iz::new(code, code + 1, code + 2, deserializer.read_handler_flags()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_P => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Ev_P::new(code, code + 1))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_REXW => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Ev_REXW::new(code, code + 1, deserializer.read_boolean(), deserializer.read_boolean())))
				as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_Sw => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Ev_Sw::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev_VX => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Ev_VX::new(code, code + 1))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ev1 => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Ev_1::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Evj => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Evj::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Evw => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Evw::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ew => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Ew::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gb_Eb => Box::into_raw(Box::new(OpCodeHandler_Gb_Eb::new(deserializer.read_code()))) as *const OpCodeHandler,

		OpCodeHandlerKind::Gdq_Ev => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Gdq_Ev::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_Eb => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Gv_Eb::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_Eb_REX => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Gv_Eb_REX::new(code, code + 1))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_Ev_32_64 => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Gv_Ev_32_64::new(code, code + 1, deserializer.read_boolean(), deserializer.read_boolean())))
				as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_Ev_3a => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Gv_Ev::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_Ev_3b => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Gv_Ev::new(code, code + 1, Code::INVALID as u32))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_Ev_Ib => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Gv_Ev_Ib::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_Ev_Ib_REX => {
			reg = deserializer.read_register();
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Gv_Ev_Ib_REX::new(reg, code, code + 1))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_Ev_Iz => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Gv_Ev_Iz::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_Ev_REX => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Gv_Ev_REX::new(code, code + 1))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_Ev2 => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Gv_Ev2::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_Ev3 => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Gv_Ev3::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_Ew => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Gv_Ew::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_M => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Gv_M::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_M_as => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Gv_M_as::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_Ma => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Gv_Ma::new(code, code + 1))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_Mp_2 => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Gv_Mp::new(code, code + 1, Code::INVALID as u32))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_Mp_3 => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Gv_Mp::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_Mv => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Gv_Mv::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_N => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Gv_N::new(code, code + 1))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_N_Ib_REX => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Gv_N_Ib_REX::new(code, code + 1))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_RX => {
			reg = deserializer.read_register();
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Gv_RX::new(reg, code, code + 1))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Gv_W => {
			reg = deserializer.read_register();
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Gv_W::new(reg, code, code + 1))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::GvM_VX_Ib => {
			reg = deserializer.read_register();
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_GvM_VX_Ib::new(reg, code, code + 1))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ib => Box::into_raw(Box::new(OpCodeHandler_Ib::new(deserializer.read_code()))) as *const OpCodeHandler,
		OpCodeHandlerKind::Ib3 => Box::into_raw(Box::new(OpCodeHandler_Ib3::new(deserializer.read_code()))) as *const OpCodeHandler,

		OpCodeHandlerKind::IbReg => {
			Box::into_raw(Box::new(OpCodeHandler_IbReg::new(deserializer.read_code(), deserializer.read_register()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::IbReg2 => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_IbReg2::new(code, code + 1))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Iw_Ib => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Iw_Ib::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Jb => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Jb::new(code, code + 1, code + 2))) as *const OpCodeHandler
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
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Jdisp::new(code, code + 1))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Jx => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Jx::new(code, code + 1, deserializer.read_code()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Jz => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Jz::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::M_1 => Box::into_raw(Box::new(OpCodeHandler_M::new(deserializer.read_code()))) as *const OpCodeHandler,

		OpCodeHandlerKind::M_2 => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_M::new1(code, code + 1))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::M_REXW_2 => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_M_REXW::new(code, code + 1, 0, 0))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::M_REXW_4 => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_M_REXW::new(code, code + 1, deserializer.read_handler_flags(), deserializer.read_handler_flags())))
				as *const OpCodeHandler
		}

		OpCodeHandlerKind::MemBx => Box::into_raw(Box::new(OpCodeHandler_MemBx::new(deserializer.read_code()))) as *const OpCodeHandler,
		OpCodeHandlerKind::Mf_1 => Box::into_raw(Box::new(OpCodeHandler_Mf::new(deserializer.read_code()))) as *const OpCodeHandler,

		OpCodeHandlerKind::Mf_2a => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Mf::new1(code, code + 1))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Mf_2b => {
			Box::into_raw(Box::new(OpCodeHandler_Mf::new1(deserializer.read_code(), deserializer.read_code()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::MIB_B => Box::into_raw(Box::new(OpCodeHandler_MIB_B::new(deserializer.read_code()))) as *const OpCodeHandler,
		OpCodeHandlerKind::MP => Box::into_raw(Box::new(OpCodeHandler_MP::new(deserializer.read_code()))) as *const OpCodeHandler,

		OpCodeHandlerKind::Ms => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Ms::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::MV => {
			Box::into_raw(Box::new(OpCodeHandler_MV::new(deserializer.read_register(), deserializer.read_code()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Mv_Gv => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Mv_Gv::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Mv_Gv_REXW => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Mv_Gv_REXW::new(code, code + 1))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::NIb => Box::into_raw(Box::new(OpCodeHandler_NIb::new(deserializer.read_code()))) as *const OpCodeHandler,

		OpCodeHandlerKind::Ob_Reg => {
			Box::into_raw(Box::new(OpCodeHandler_Ob_Reg::new(deserializer.read_code(), deserializer.read_register()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Ov_Reg => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Ov_Reg::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::P_Ev => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_P_Ev::new(code, code + 1))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::P_Ev_Ib => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_P_Ev_Ib::new(code, code + 1))) as *const OpCodeHandler
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
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_PushEv::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::PushIb2 => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_PushIb2::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::PushIz => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_PushIz::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::PushOpSizeReg_4a => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_PushOpSizeReg::new(code, code + 1, code + 2, deserializer.read_register()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::PushOpSizeReg_4b => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_PushOpSizeReg::new(code, code + 1, Code::INVALID as u32, deserializer.read_register())))
				as *const OpCodeHandler
		}

		OpCodeHandlerKind::PushSimple2 => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_PushSimple2::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::PushSimpleReg => {
			index = deserializer.read_u32();
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_PushSimpleReg::new(index, code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Q_P => Box::into_raw(Box::new(OpCodeHandler_Q_P::new(deserializer.read_code()))) as *const OpCodeHandler,

		OpCodeHandlerKind::R_C_3a => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_R_C::new(code, code + 1, deserializer.read_register()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::R_C_3b => {
			Box::into_raw(Box::new(OpCodeHandler_R_C::new(deserializer.read_code(), Code::INVALID as u32, deserializer.read_register())))
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
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Reg_Ib2::new(code, code + 1))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Reg_Iz => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Reg_Iz::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Reg_Ob => {
			Box::into_raw(Box::new(OpCodeHandler_Reg_Ob::new(deserializer.read_code(), deserializer.read_register()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Reg_Ov => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Reg_Ov::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Reg_Xb => {
			Box::into_raw(Box::new(OpCodeHandler_Reg_Xb::new(deserializer.read_code(), deserializer.read_register()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Reg_Xv => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Reg_Xv::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Reg_Xv2 => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Reg_Xv2::new(code, code + 1))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Reg_Yb => {
			Box::into_raw(Box::new(OpCodeHandler_Reg_Yb::new(deserializer.read_code(), deserializer.read_register()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Reg_Yv => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Reg_Yv::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::RegIb => {
			Box::into_raw(Box::new(OpCodeHandler_RegIb::new(deserializer.read_code(), deserializer.read_register()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::RegIb3 => Box::into_raw(Box::new(OpCodeHandler_RegIb3::new(deserializer.read_u32()))) as *const OpCodeHandler,
		OpCodeHandlerKind::RegIz2 => Box::into_raw(Box::new(OpCodeHandler_RegIz2::new(deserializer.read_u32()))) as *const OpCodeHandler,

		OpCodeHandlerKind::ReservedNop => {
			Box::into_raw(Box::new(OpCodeHandler_ReservedNop::new(deserializer.read_handler(), deserializer.read_handler()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::RIb => {
			Box::into_raw(Box::new(OpCodeHandler_RIb::new(deserializer.read_register(), deserializer.read_code()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::RIbIb => {
			Box::into_raw(Box::new(OpCodeHandler_RIbIb::new(deserializer.read_register(), deserializer.read_code()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Rv => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Rv::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Rv_32_64 => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Rv_32_64::new(code, code + 1))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::RvMw_Gw => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_RvMw_Gw::new(code, code + 1))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Simple => Box::into_raw(Box::new(OpCodeHandler_Simple::new(deserializer.read_code()))) as *const OpCodeHandler,
		OpCodeHandlerKind::Simple_ModRM => Box::into_raw(Box::new(OpCodeHandler_Simple::new_modrm(deserializer.read_code()))) as *const OpCodeHandler,

		OpCodeHandlerKind::Simple2_3a => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Simple2::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Simple2_3b => {
			Box::into_raw(Box::new(OpCodeHandler_Simple2::new(deserializer.read_code(), deserializer.read_code(), deserializer.read_code())))
				as *const OpCodeHandler
		}

		OpCodeHandlerKind::Simple2Iw => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Simple2Iw::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Simple3 => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Simple3::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Simple4 => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Simple4::new(code, code + 1))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Simple4b => {
			code = deserializer.read_code();
			let code2 = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Simple4::new(code, code2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Simple5 => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Simple5::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Simple5_ModRM_as => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Simple5_ModRM_as::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::SimpleReg => {
			Box::into_raw(Box::new(OpCodeHandler_SimpleReg::new(deserializer.read_code(), deserializer.read_u32()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::ST_STi => Box::into_raw(Box::new(OpCodeHandler_ST_STi::new(deserializer.read_code()))) as *const OpCodeHandler,
		OpCodeHandlerKind::STi => Box::into_raw(Box::new(OpCodeHandler_STi::new(deserializer.read_code()))) as *const OpCodeHandler,
		OpCodeHandlerKind::STi_ST => Box::into_raw(Box::new(OpCodeHandler_STi_ST::new(deserializer.read_code()))) as *const OpCodeHandler,

		OpCodeHandlerKind::Sw_Ev => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Sw_Ev::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::V_Ev => {
			reg = deserializer.read_register();
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_V_Ev::new(reg, code, code + 1))) as *const OpCodeHandler
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
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_VWIb::new1(reg, code, code + 1))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::VX_E_Ib => {
			reg = deserializer.read_register();
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_VX_E_Ib::new(reg, code, code + 1))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::VX_Ev => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_VX_Ev::new(code, code + 1))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Wbinvd => Box::into_raw(Box::new(OpCodeHandler_Wbinvd::new())) as *const OpCodeHandler,

		OpCodeHandlerKind::WV => {
			Box::into_raw(Box::new(OpCodeHandler_WV::new(deserializer.read_register(), deserializer.read_code()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Xb_Yb => Box::into_raw(Box::new(OpCodeHandler_Xb_Yb::new(deserializer.read_code()))) as *const OpCodeHandler,
		OpCodeHandlerKind::Xchg_Reg_rAX => Box::into_raw(Box::new(OpCodeHandler_Xchg_Reg_rAX::new(deserializer.read_u32()))) as *const OpCodeHandler,

		OpCodeHandlerKind::Xv_Yv => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Xv_Yv::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Yb_Reg => {
			Box::into_raw(Box::new(OpCodeHandler_Yb_Reg::new(deserializer.read_code(), deserializer.read_register()))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Yb_Xb => Box::into_raw(Box::new(OpCodeHandler_Yb_Xb::new(deserializer.read_code()))) as *const OpCodeHandler,

		OpCodeHandlerKind::Yv_Reg => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Yv_Reg::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Yv_Reg2 => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Yv_Reg2::new(code, code + 1))) as *const OpCodeHandler
		}

		OpCodeHandlerKind::Yv_Xv => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_Yv_Xv::new(code, code + 1, code + 2))) as *const OpCodeHandler
		}
	};
	result.push(unsafe { &*elem });
}
