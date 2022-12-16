// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::decoder::handlers::vex::*;
use crate::decoder::handlers::{
	get_invalid_handler, get_invalid_no_modrm_handler, get_null_handler, OpCodeHandler, OpCodeHandlerDecodeFn, OpCodeHandler_Bitness,
	OpCodeHandler_Bitness_DontReadModRM, OpCodeHandler_Group, OpCodeHandler_Group8x64, OpCodeHandler_MandatoryPrefix2,
	OpCodeHandler_Options_DontReadModRM, OpCodeHandler_RM, OpCodeHandler_W,
};
use crate::decoder::table_de::enums::*;
use crate::decoder::table_de::{box_opcode_handler, TableDeserializer};
use alloc::vec::Vec;

#[allow(trivial_casts)]
pub(super) fn read_handlers(deserializer: &mut TableDeserializer<'_>, result: &mut Vec<(OpCodeHandlerDecodeFn, &'static OpCodeHandler)>) {
	let reg;
	let (decode, handler_ptr): (OpCodeHandlerDecodeFn, *const OpCodeHandler) = match deserializer.read_vex_op_code_handler_kind() {
		VexOpCodeHandlerKind::Invalid => {
			result.push(get_invalid_handler());
			return;
		}

		VexOpCodeHandlerKind::Invalid2 => {
			result.push(get_invalid_handler());
			result.push(get_invalid_handler());
			return;
		}

		VexOpCodeHandlerKind::Dup => {
			let count = deserializer.read_u32();
			let handler = deserializer.read_handler_or_null_instance();
			for _ in 0..count {
				result.push(handler);
			}
			return;
		}

		VexOpCodeHandlerKind::Null => {
			result.push(get_null_handler());
			return;
		}

		VexOpCodeHandlerKind::Invalid_NoModRM => {
			result.push(get_invalid_no_modrm_handler());
			return;
		}

		VexOpCodeHandlerKind::Bitness => box_opcode_handler(OpCodeHandler_Bitness::new(deserializer.read_handler(), deserializer.read_handler())),

		VexOpCodeHandlerKind::Bitness_DontReadModRM => {
			box_opcode_handler(OpCodeHandler_Bitness_DontReadModRM::new(deserializer.read_handler(), deserializer.read_handler()))
		}

		VexOpCodeHandlerKind::HandlerReference => {
			result.push(deserializer.read_handler_reference());
			return;
		}

		VexOpCodeHandlerKind::ArrayReference => unreachable!(),
		VexOpCodeHandlerKind::RM => box_opcode_handler(OpCodeHandler_RM::new(deserializer.read_handler(), deserializer.read_handler())),

		VexOpCodeHandlerKind::Group => {
			box_opcode_handler(OpCodeHandler_Group::new(deserializer.read_array_reference(VexOpCodeHandlerKind::ArrayReference as u32)))
		}

		VexOpCodeHandlerKind::Group8x64 => box_opcode_handler(OpCodeHandler_Group8x64::new(
			deserializer.read_array_reference(VexOpCodeHandlerKind::ArrayReference as u32),
			deserializer.read_array_reference(VexOpCodeHandlerKind::ArrayReference as u32),
		)),

		VexOpCodeHandlerKind::W => box_opcode_handler(OpCodeHandler_W::new(deserializer.read_handler(), deserializer.read_handler())),

		VexOpCodeHandlerKind::MandatoryPrefix2_1 => box_opcode_handler(OpCodeHandler_MandatoryPrefix2::new(
			true,
			deserializer.read_handler(),
			get_invalid_handler(),
			get_invalid_handler(),
			get_invalid_handler(),
		)),

		VexOpCodeHandlerKind::MandatoryPrefix2_4 => box_opcode_handler(OpCodeHandler_MandatoryPrefix2::new(
			true,
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
		)),

		VexOpCodeHandlerKind::MandatoryPrefix2_NoModRM => box_opcode_handler(OpCodeHandler_MandatoryPrefix2::new(
			false,
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
		)),

		VexOpCodeHandlerKind::VectorLength_NoModRM => {
			box_opcode_handler(OpCodeHandler_VectorLength_VEX::new(false, deserializer.read_handler(), deserializer.read_handler()))
		}

		VexOpCodeHandlerKind::VectorLength => {
			box_opcode_handler(OpCodeHandler_VectorLength_VEX::new(true, deserializer.read_handler(), deserializer.read_handler()))
		}

		VexOpCodeHandlerKind::Ed_V_Ib => {
			reg = deserializer.read_register();
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_VEX_Ed_V_Ib::new(reg, code1, code2))
		}

		VexOpCodeHandlerKind::Ev_VX => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_VEX_Ev_VX::new(code1, code2))
		}

		VexOpCodeHandlerKind::G_VK => box_opcode_handler(OpCodeHandler_VEX_G_VK::new(deserializer.read_code(), deserializer.read_register())),

		VexOpCodeHandlerKind::Gv_Ev_Gv => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_VEX_Gv_Ev_Gv::new(code1, code2))
		}

		VexOpCodeHandlerKind::Ev_Gv_Gv => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_VEX_Ev_Gv_Gv::new(code1, code2))
		}

		VexOpCodeHandlerKind::Gv_Ev_Ib => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_VEX_Gv_Ev_Ib::new(code1, code2))
		}

		VexOpCodeHandlerKind::Gv_Ev_Id => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_VEX_Gv_Ev_Id::new(code1, code2))
		}

		VexOpCodeHandlerKind::Gv_GPR_Ib => {
			reg = deserializer.read_register();
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_VEX_Gv_GPR_Ib::new(reg, code1, code2))
		}

		VexOpCodeHandlerKind::Gv_Gv_Ev => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_VEX_Gv_Gv_Ev::new(code1, code2))
		}

		VexOpCodeHandlerKind::Gv_RX => {
			reg = deserializer.read_register();
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_VEX_Gv_RX::new(reg, code1, code2))
		}

		VexOpCodeHandlerKind::Gv_W => {
			reg = deserializer.read_register();
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_VEX_Gv_W::new(reg, code1, code2))
		}

		VexOpCodeHandlerKind::GvM_VX_Ib => {
			reg = deserializer.read_register();
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_VEX_GvM_VX_Ib::new(reg, code1, code2))
		}

		VexOpCodeHandlerKind::HRIb => box_opcode_handler(OpCodeHandler_VEX_HRIb::new(deserializer.read_register(), deserializer.read_code())),

		VexOpCodeHandlerKind::Hv_Ed_Id => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_VEX_Hv_Ed_Id::new(code1, code2))
		}

		VexOpCodeHandlerKind::Hv_Ev => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_VEX_Hv_Ev::new(code1, code2))
		}

		VexOpCodeHandlerKind::M => box_opcode_handler(OpCodeHandler_VEX_M::new(deserializer.read_code())),
		VexOpCodeHandlerKind::MHV => box_opcode_handler(OpCodeHandler_VEX_MHV::new(deserializer.read_register(), deserializer.read_code())),
		VexOpCodeHandlerKind::M_VK => box_opcode_handler(OpCodeHandler_VEX_M_VK::new(deserializer.read_code())),
		VexOpCodeHandlerKind::MV => box_opcode_handler(OpCodeHandler_VEX_MV::new(deserializer.read_register(), deserializer.read_code())),

		VexOpCodeHandlerKind::rDI_VX_RX => {
			box_opcode_handler(OpCodeHandler_VEX_rDI_VX_RX::new(deserializer.read_register(), deserializer.read_code()))
		}

		VexOpCodeHandlerKind::RdRq => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_VEX_RdRq::new(code1, code2))
		}

		VexOpCodeHandlerKind::Simple => box_opcode_handler(OpCodeHandler_VEX_Simple::new(deserializer.read_code())),

		VexOpCodeHandlerKind::VHEv => {
			reg = deserializer.read_register();
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_VEX_VHEv::new(reg, code1, code2))
		}

		VexOpCodeHandlerKind::VHEvIb => {
			reg = deserializer.read_register();
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_VEX_VHEvIb::new(reg, code1, code2))
		}

		VexOpCodeHandlerKind::VHIs4W => box_opcode_handler(OpCodeHandler_VEX_VHIs4W::new(deserializer.read_register(), deserializer.read_code())),
		VexOpCodeHandlerKind::VHIs5W => box_opcode_handler(OpCodeHandler_VEX_VHIs5W::new(deserializer.read_register(), deserializer.read_code())),
		VexOpCodeHandlerKind::VHM => box_opcode_handler(OpCodeHandler_VEX_VHM::new(deserializer.read_register(), deserializer.read_code())),

		VexOpCodeHandlerKind::VHW_2 => {
			reg = deserializer.read_register();
			let code = deserializer.read_code();
			box_opcode_handler(OpCodeHandler_VEX_VHW::new(reg, reg, reg, code, code))
		}

		VexOpCodeHandlerKind::VHW_3 => {
			reg = deserializer.read_register();
			box_opcode_handler(OpCodeHandler_VEX_VHW::new(reg, reg, reg, deserializer.read_code(), deserializer.read_code()))
		}

		VexOpCodeHandlerKind::VHW_4 => box_opcode_handler(OpCodeHandler_VEX_VHW::new1(
			deserializer.read_register(),
			deserializer.read_register(),
			deserializer.read_register(),
			deserializer.read_code(),
		)),

		VexOpCodeHandlerKind::VHWIb_2 => {
			reg = deserializer.read_register();
			box_opcode_handler(OpCodeHandler_VEX_VHWIb::new(reg, reg, reg, deserializer.read_code()))
		}

		VexOpCodeHandlerKind::VHWIb_4 => box_opcode_handler(OpCodeHandler_VEX_VHWIb::new(
			deserializer.read_register(),
			deserializer.read_register(),
			deserializer.read_register(),
			deserializer.read_code(),
		)),

		VexOpCodeHandlerKind::VHWIs4 => box_opcode_handler(OpCodeHandler_VEX_VHWIs4::new(deserializer.read_register(), deserializer.read_code())),
		VexOpCodeHandlerKind::VHWIs5 => box_opcode_handler(OpCodeHandler_VEX_VHWIs5::new(deserializer.read_register(), deserializer.read_code())),
		VexOpCodeHandlerKind::VK_HK_RK => box_opcode_handler(OpCodeHandler_VEX_VK_HK_RK::new(deserializer.read_code())),
		VexOpCodeHandlerKind::VK_R => box_opcode_handler(OpCodeHandler_VEX_VK_R::new(deserializer.read_code(), deserializer.read_register())),
		VexOpCodeHandlerKind::VK_RK => box_opcode_handler(OpCodeHandler_VEX_VK_RK::new(deserializer.read_code())),
		VexOpCodeHandlerKind::VK_RK_Ib => box_opcode_handler(OpCodeHandler_VEX_VK_RK_Ib::new(deserializer.read_code())),
		VexOpCodeHandlerKind::VK_WK => box_opcode_handler(OpCodeHandler_VEX_VK_WK::new(deserializer.read_code())),

		VexOpCodeHandlerKind::VM => box_opcode_handler(OpCodeHandler_VEX_VM::new(deserializer.read_register(), deserializer.read_code())),

		VexOpCodeHandlerKind::VW_2 => {
			reg = deserializer.read_register();
			box_opcode_handler(OpCodeHandler_VEX_VW::new(reg, reg, deserializer.read_code()))
		}

		VexOpCodeHandlerKind::VW_3 => {
			box_opcode_handler(OpCodeHandler_VEX_VW::new(deserializer.read_register(), deserializer.read_register(), deserializer.read_code()))
		}

		VexOpCodeHandlerKind::VWH => box_opcode_handler(OpCodeHandler_VEX_VWH::new(deserializer.read_register(), deserializer.read_code())),

		VexOpCodeHandlerKind::VWIb_2 => {
			reg = deserializer.read_register();
			let code = deserializer.read_code();
			box_opcode_handler(OpCodeHandler_VEX_VWIb::new(reg, reg, code, code))
		}

		VexOpCodeHandlerKind::VWIb_3 => {
			reg = deserializer.read_register();
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_VEX_VWIb::new(reg, reg, code1, code2))
		}

		VexOpCodeHandlerKind::VX_Ev => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_VEX_VX_Ev::new(code1, code2))
		}

		VexOpCodeHandlerKind::VX_VSIB_HX => box_opcode_handler(OpCodeHandler_VEX_VX_VSIB_HX::new(
			deserializer.read_register(),
			deserializer.read_register(),
			deserializer.read_register(),
			deserializer.read_code(),
		)),

		VexOpCodeHandlerKind::WHV => box_opcode_handler(OpCodeHandler_VEX_WHV::new(deserializer.read_register(), deserializer.read_code())),
		VexOpCodeHandlerKind::WV => box_opcode_handler(OpCodeHandler_VEX_WV::new(deserializer.read_register(), deserializer.read_code())),

		VexOpCodeHandlerKind::WVIb => {
			box_opcode_handler(OpCodeHandler_VEX_WVIb::new(deserializer.read_register(), deserializer.read_register(), deserializer.read_code()))
		}

		VexOpCodeHandlerKind::VT_SIBMEM => box_opcode_handler(OpCodeHandler_VEX_VT_SIBMEM::new(deserializer.read_code())),
		VexOpCodeHandlerKind::SIBMEM_VT => box_opcode_handler(OpCodeHandler_VEX_SIBMEM_VT::new(deserializer.read_code())),
		VexOpCodeHandlerKind::VT => box_opcode_handler(OpCodeHandler_VEX_VT::new(deserializer.read_code())),
		VexOpCodeHandlerKind::VT_RT_HT => box_opcode_handler(OpCodeHandler_VEX_VT_RT_HT::new(deserializer.read_code())),
		VexOpCodeHandlerKind::Options_DontReadModRM => box_opcode_handler(OpCodeHandler_Options_DontReadModRM::new(
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_decoder_options(),
		)),
		VexOpCodeHandlerKind::Gq_HK_RK => box_opcode_handler(OpCodeHandler_VEX_Gq_HK_RK::new(deserializer.read_code())),
		VexOpCodeHandlerKind::VK_R_Ib => box_opcode_handler(OpCodeHandler_VEX_VK_R_Ib::new(deserializer.read_code(), deserializer.read_register())),
		VexOpCodeHandlerKind::Gv_Ev => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_VEX_Gv_Ev::new(code1, code2))
		}
		VexOpCodeHandlerKind::Ev => {
			let (code1, code2) = deserializer.read_code2();
			box_opcode_handler(OpCodeHandler_VEX_Ev::new(code1, code2))
		}
		VexOpCodeHandlerKind::K_Jb => box_opcode_handler(OpCodeHandler_VEX_K_Jb::new(deserializer.read_code())),
		VexOpCodeHandlerKind::K_Jz => box_opcode_handler(OpCodeHandler_VEX_K_Jz::new(deserializer.read_code())),
	};
	let handler = unsafe { &*handler_ptr };
	result.push((decode, handler));
}
