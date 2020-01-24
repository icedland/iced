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
use super::super::handlers_vex::*;
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
	let elem: *const OpCodeHandler = match deserializer.read_vex_op_code_handler_kind() {
		VexOpCodeHandlerKind::Invalid => &INVALID_HANDLER as *const _ as *const OpCodeHandler,

		VexOpCodeHandlerKind::Invalid2 => {
			result.push(unsafe { &*(&INVALID_HANDLER as *const _ as *const OpCodeHandler) });
			&INVALID_HANDLER as *const _ as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::Dup => {
			let count = deserializer.read_u32();
			let handler = deserializer.read_handler();
			for _ in 0..count {
				result.push(unsafe { &*handler });
			}
			return;
		}

		VexOpCodeHandlerKind::Invalid_NoModRM => &INVALID_NO_MODRM_HANDLER as *const _ as *const OpCodeHandler,

		VexOpCodeHandlerKind::Bitness_DontReadModRM => {
			Box::into_raw(Box::new(OpCodeHandler_Bitness_DontReadModRM::new(deserializer.read_handler(), deserializer.read_handler())))
				as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::HandlerReference => deserializer.read_handler_reference(),
		VexOpCodeHandlerKind::ArrayReference => unreachable!(),

		VexOpCodeHandlerKind::RM => {
			Box::into_raw(Box::new(OpCodeHandler_RM::new(deserializer.read_handler(), deserializer.read_handler()))) as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::Group => {
			Box::into_raw(Box::new(OpCodeHandler_Group::new(deserializer.read_array_reference(VexOpCodeHandlerKind::ArrayReference as u32))))
				as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::W => {
			Box::into_raw(Box::new(OpCodeHandler_W::new(deserializer.read_handler(), deserializer.read_handler()))) as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::MandatoryPrefix2_1 => Box::into_raw(Box::new(OpCodeHandler_MandatoryPrefix2::new(
			true,
			deserializer.read_handler(),
			&INVALID_HANDLER as *const _ as *const OpCodeHandler,
			&INVALID_HANDLER as *const _ as *const OpCodeHandler,
			&INVALID_HANDLER as *const _ as *const OpCodeHandler,
		))) as *const OpCodeHandler,

		VexOpCodeHandlerKind::MandatoryPrefix2_4 => Box::into_raw(Box::new(OpCodeHandler_MandatoryPrefix2::new(
			true,
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
		))) as *const OpCodeHandler,

		VexOpCodeHandlerKind::MandatoryPrefix2_NoModRM => Box::into_raw(Box::new(OpCodeHandler_MandatoryPrefix2::new(
			false,
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
			deserializer.read_handler(),
		))) as *const OpCodeHandler,

		VexOpCodeHandlerKind::VectorLength_NoModRM => {
			Box::into_raw(Box::new(OpCodeHandler_VectorLength_VEX::new(false, deserializer.read_handler(), deserializer.read_handler())))
				as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::VectorLength => {
			Box::into_raw(Box::new(OpCodeHandler_VectorLength_VEX::new(true, deserializer.read_handler(), deserializer.read_handler())))
				as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::Ed_V_Ib => {
			reg = deserializer.read_register();
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_VEX_Ed_V_Ib::new(reg, code, code + 1))) as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::Ev_VX => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_VEX_Ev_VX::new(code, code + 1))) as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::G_VK => {
			Box::into_raw(Box::new(OpCodeHandler_VEX_G_VK::new(deserializer.read_code(), deserializer.read_register()))) as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::Gv_Ev_Gv => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_VEX_Gv_Ev_Gv::new(code, code + 1))) as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::Gv_Ev_Ib => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_VEX_Gv_Ev_Ib::new(code, code + 1))) as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::Gv_Ev_Id => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_VEX_Gv_Ev_Id::new(code, code + 1))) as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::Gv_GPR_Ib => {
			reg = deserializer.read_register();
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_VEX_Gv_GPR_Ib::new(reg, code, code + 1))) as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::Gv_Gv_Ev => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_VEX_Gv_Gv_Ev::new(code, code + 1))) as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::Gv_RX => {
			reg = deserializer.read_register();
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_VEX_Gv_RX::new(reg, code, code + 1))) as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::Gv_W => {
			reg = deserializer.read_register();
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_VEX_Gv_W::new(reg, code, code + 1))) as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::GvM_VX_Ib => {
			reg = deserializer.read_register();
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_VEX_GvM_VX_Ib::new(reg, code, code + 1))) as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::HRIb => {
			Box::into_raw(Box::new(OpCodeHandler_VEX_HRIb::new(deserializer.read_register(), deserializer.read_code()))) as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::Hv_Ed_Id => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_VEX_Hv_Ed_Id::new(code, code + 1))) as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::Hv_Ev => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_VEX_Hv_Ev::new(code, code + 1))) as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::M => Box::into_raw(Box::new(OpCodeHandler_VEX_M::new(deserializer.read_code()))) as *const OpCodeHandler,

		VexOpCodeHandlerKind::MHV => {
			Box::into_raw(Box::new(OpCodeHandler_VEX_MHV::new(deserializer.read_register(), deserializer.read_code()))) as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::M_VK => Box::into_raw(Box::new(OpCodeHandler_VEX_M_VK::new(deserializer.read_code()))) as *const OpCodeHandler,

		VexOpCodeHandlerKind::MV => {
			Box::into_raw(Box::new(OpCodeHandler_VEX_MV::new(deserializer.read_register(), deserializer.read_code()))) as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::rDI_VX_RX => {
			Box::into_raw(Box::new(OpCodeHandler_VEX_rDI_VX_RX::new(deserializer.read_register(), deserializer.read_code()))) as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::RdRq => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_VEX_RdRq::new(code, code + 1))) as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::Simple => Box::into_raw(Box::new(OpCodeHandler_VEX_Simple::new(deserializer.read_code()))) as *const OpCodeHandler,

		VexOpCodeHandlerKind::VHEv => {
			reg = deserializer.read_register();
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_VEX_VHEv::new(reg, code, code + 1))) as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::VHEvIb => {
			reg = deserializer.read_register();
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_VEX_VHEvIb::new(reg, code, code + 1))) as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::VHIs4W => {
			Box::into_raw(Box::new(OpCodeHandler_VEX_VHIs4W::new(deserializer.read_register(), deserializer.read_code()))) as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::VHIs5W => {
			Box::into_raw(Box::new(OpCodeHandler_VEX_VHIs5W::new(deserializer.read_register(), deserializer.read_code()))) as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::VHM => {
			Box::into_raw(Box::new(OpCodeHandler_VEX_VHM::new(deserializer.read_register(), deserializer.read_code()))) as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::VHW_2 => {
			reg = deserializer.read_register();
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_VEX_VHW::new(reg, reg, reg, code, code))) as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::VHW_3 => {
			reg = deserializer.read_register();
			Box::into_raw(Box::new(OpCodeHandler_VEX_VHW::new(reg, reg, reg, deserializer.read_code(), deserializer.read_code())))
				as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::VHW_4 => Box::into_raw(Box::new(OpCodeHandler_VEX_VHW::new1(
			deserializer.read_register(),
			deserializer.read_register(),
			deserializer.read_register(),
			deserializer.read_code(),
		))) as *const OpCodeHandler,

		VexOpCodeHandlerKind::VHWIb_2 => {
			reg = deserializer.read_register();
			Box::into_raw(Box::new(OpCodeHandler_VEX_VHWIb::new(reg, reg, reg, deserializer.read_code()))) as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::VHWIb_4 => Box::into_raw(Box::new(OpCodeHandler_VEX_VHWIb::new(
			deserializer.read_register(),
			deserializer.read_register(),
			deserializer.read_register(),
			deserializer.read_code(),
		))) as *const OpCodeHandler,

		VexOpCodeHandlerKind::VHWIs4 => {
			Box::into_raw(Box::new(OpCodeHandler_VEX_VHWIs4::new(deserializer.read_register(), deserializer.read_code()))) as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::VHWIs5 => {
			Box::into_raw(Box::new(OpCodeHandler_VEX_VHWIs5::new(deserializer.read_register(), deserializer.read_code()))) as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::VK_HK_RK => Box::into_raw(Box::new(OpCodeHandler_VEX_VK_HK_RK::new(deserializer.read_code()))) as *const OpCodeHandler,

		VexOpCodeHandlerKind::VK_R => {
			Box::into_raw(Box::new(OpCodeHandler_VEX_VK_R::new(deserializer.read_code(), deserializer.read_register()))) as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::VK_RK => Box::into_raw(Box::new(OpCodeHandler_VEX_VK_RK::new(deserializer.read_code()))) as *const OpCodeHandler,
		VexOpCodeHandlerKind::VK_RK_Ib => Box::into_raw(Box::new(OpCodeHandler_VEX_VK_RK_Ib::new(deserializer.read_code()))) as *const OpCodeHandler,
		VexOpCodeHandlerKind::VK_WK => Box::into_raw(Box::new(OpCodeHandler_VEX_VK_WK::new(deserializer.read_code()))) as *const OpCodeHandler,

		VexOpCodeHandlerKind::VM => {
			Box::into_raw(Box::new(OpCodeHandler_VEX_VM::new(deserializer.read_register(), deserializer.read_code()))) as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::VW_2 => {
			reg = deserializer.read_register();
			Box::into_raw(Box::new(OpCodeHandler_VEX_VW::new(reg, reg, deserializer.read_code()))) as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::VW_3 => {
			Box::into_raw(Box::new(OpCodeHandler_VEX_VW::new(deserializer.read_register(), deserializer.read_register(), deserializer.read_code())))
				as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::VWH => {
			Box::into_raw(Box::new(OpCodeHandler_VEX_VWH::new(deserializer.read_register(), deserializer.read_code()))) as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::VWIb_2 => {
			reg = deserializer.read_register();
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_VEX_VWIb::new(reg, reg, code, code))) as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::VWIb_3 => {
			reg = deserializer.read_register();
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_VEX_VWIb::new(reg, reg, code, code + 1))) as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::VX_Ev => {
			code = deserializer.read_code();
			Box::into_raw(Box::new(OpCodeHandler_VEX_VX_Ev::new(code, code + 1))) as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::VX_VSIB_HX => Box::into_raw(Box::new(OpCodeHandler_VEX_VX_VSIB_HX::new(
			deserializer.read_register(),
			deserializer.read_register(),
			deserializer.read_register(),
			deserializer.read_code(),
		))) as *const OpCodeHandler,

		VexOpCodeHandlerKind::WHV => {
			Box::into_raw(Box::new(OpCodeHandler_VEX_WHV::new(deserializer.read_register(), deserializer.read_code()))) as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::WV => {
			Box::into_raw(Box::new(OpCodeHandler_VEX_WV::new(deserializer.read_register(), deserializer.read_code()))) as *const OpCodeHandler
		}

		VexOpCodeHandlerKind::WVIb => {
			Box::into_raw(Box::new(OpCodeHandler_VEX_WVIb::new(deserializer.read_register(), deserializer.read_register(), deserializer.read_code())))
				as *const OpCodeHandler
		}
	};
	result.push(unsafe { &*elem });
}
