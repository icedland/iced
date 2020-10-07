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

use super::super::enums::EncodingKind;
use super::super::iced_constants::IcedConstants;
use super::super::*;
use super::encoder_data::{ENC_FLAGS1, ENC_FLAGS2, ENC_FLAGS3};
use super::enums::*;
use super::op_code_handler::*;
#[cfg(not(feature = "std"))]
use alloc::boxed::Box;
#[cfg(not(feature = "std"))]
use alloc::vec::Vec;
use core::mem;

lazy_static! {
	pub(crate) static ref HANDLERS_TABLE: Vec<&'static OpCodeHandler> = {
		let mut v = Vec::with_capacity(IcedConstants::NUMBER_OF_CODE_VALUES);
		let invalid_handler = Box::into_raw(Box::new(InvalidHandler::new())) as *const OpCodeHandler;
		for i in 0..IcedConstants::NUMBER_OF_CODE_VALUES {
			let enc_flags1 = ENC_FLAGS1[i];
			let enc_flags2 = ENC_FLAGS2[i];
			let enc_flags3 = ENC_FLAGS3[i];
			let encoding: EncodingKind = unsafe { mem::transmute(((enc_flags3 >> EncFlags3::ENCODING_SHIFT) & EncFlags3::ENCODING_MASK) as u8) };
			let handler = match encoding {
				EncodingKind::Legacy => {
					let code: Code = unsafe { mem::transmute(i as u16) };
					if code == Code::INVALID {
						invalid_handler
					} else if code <= Code::DeclareQword {
						Box::into_raw(Box::new(DeclareDataHandler::new(code))) as *const OpCodeHandler
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
			};
			v.push(unsafe { &*handler });
		}
		v
	};
}
