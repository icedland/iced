// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::encoder::encoder_data::{ENC_FLAGS1, ENC_FLAGS2, ENC_FLAGS3};
use crate::encoder::enums::*;
use crate::encoder::op_code_handler::*;
use crate::enums::EncodingKind;
use crate::iced_constants::IcedConstants;
use crate::*;
use alloc::boxed::Box;
use alloc::vec::Vec;
use core::convert::TryInto;
use core::mem;
use lazy_static::lazy_static;

lazy_static! {
	pub(crate) static ref HANDLERS_TABLE: Box<[&'static OpCodeHandler; IcedConstants::CODE_ENUM_COUNT]> = {
		let mut v = Vec::with_capacity(IcedConstants::CODE_ENUM_COUNT);
		let invalid_handler = Box::into_raw(Box::new(InvalidHandler::new())) as *const OpCodeHandler;
		for code in Code::values() {
			let enc_flags1 = ENC_FLAGS1[code as usize];
			let enc_flags2 = ENC_FLAGS2[code as usize];
			let enc_flags3 = ENC_FLAGS3[code as usize];
			// SAFETY: The table is generated and only contains valid enum variants
			let encoding: EncodingKind = unsafe { mem::transmute(((enc_flags3 >> EncFlags3::ENCODING_SHIFT) & EncFlags3::ENCODING_MASK) as EncodingKindUnderlyingType) };
			let handler = match encoding {
				EncodingKind::Legacy => {
					if code == Code::INVALID {
						invalid_handler
					} else if code <= Code::DeclareQword {
						Box::into_raw(Box::new(DeclareDataHandler::new(code))) as *const OpCodeHandler
					} else if code == Code::Zero_bytes {
						Box::into_raw(Box::new(ZeroBytesHandler::new(code))) as *const OpCodeHandler
					} else {
						Box::into_raw(Box::new(LegacyHandler::new(enc_flags1, enc_flags2, enc_flags3))) as *const OpCodeHandler
					}
				}
				#[cfg(not(feature = "no_vex"))]
				EncodingKind::VEX => Box::into_raw(Box::new(VexHandler::new(enc_flags1, enc_flags2, enc_flags3))) as *const OpCodeHandler,
				#[cfg(feature = "no_vex")]
				EncodingKind::VEX => invalid_handler,
				#[cfg(not(feature = "no_evex"))]
				EncodingKind::EVEX => Box::into_raw(Box::new(EvexHandler::new(enc_flags1, enc_flags2, enc_flags3))) as *const OpCodeHandler,
				#[cfg(feature = "no_evex")]
				EncodingKind::EVEX => invalid_handler,
				#[cfg(not(feature = "no_xop"))]
				EncodingKind::XOP => Box::into_raw(Box::new(XopHandler::new(enc_flags1, enc_flags2, enc_flags3))) as *const OpCodeHandler,
				#[cfg(feature = "no_xop")]
				EncodingKind::XOP => invalid_handler,
				#[cfg(not(feature = "no_d3now"))]
				EncodingKind::D3NOW => Box::into_raw(Box::new(D3nowHandler::new(enc_flags2, enc_flags3))) as *const OpCodeHandler,
				#[cfg(feature = "no_d3now")]
				EncodingKind::D3NOW => invalid_handler,
				#[cfg(feature = "mvex")]
				EncodingKind::MVEX => Box::into_raw(Box::new(MvexHandler::new(enc_flags1, enc_flags2, enc_flags3))) as *const OpCodeHandler,
				#[cfg(not(feature = "mvex"))]
				EncodingKind::MVEX => invalid_handler,
			};
			v.push(unsafe { &*handler });
		}
		#[allow(clippy::unwrap_used)]
		v.into_boxed_slice().try_into().ok().unwrap()
	};
}
