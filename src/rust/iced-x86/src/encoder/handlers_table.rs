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
use super::enums::*;
use super::op_code_data::OP_CODE_DATA;
use super::op_code_handler::*;
use std::mem;

lazy_static! {
	pub(crate) static ref HANDLERS_TABLE: Vec<&'static OpCodeHandler> = {
		let mut v = Vec::with_capacity(IcedConstants::NUMBER_OF_CODE_VALUES);
		debug_assert_eq!(IcedConstants::NUMBER_OF_CODE_VALUES * 3, OP_CODE_DATA.len());
		for i in 0..IcedConstants::NUMBER_OF_CODE_VALUES {
			let j = i * 3;
			let dword1 = OP_CODE_DATA[j];
			let dword2 = OP_CODE_DATA[j + 1];
			let dword3 = OP_CODE_DATA[j + 2];
			let encoding: EncodingKind = unsafe { mem::transmute(((dword1 >> EncFlags1::ENCODING_SHIFT) & EncFlags1::ENCODING_MASK) as u8) };
			let handler = match encoding {
				EncodingKind::Legacy => {
					let code: Code = unsafe { mem::transmute(i as u16) };
					if code == Code::INVALID {
						Box::into_raw(Box::new(InvalidHandler::new())) as *const OpCodeHandler
					} else if code >= Code::DeclareByte {
						Box::into_raw(Box::new(DeclareDataHandler::new(code))) as *const OpCodeHandler
					} else {
						Box::into_raw(Box::new(LegacyHandler::new(dword1, dword2, dword3))) as *const OpCodeHandler
					}
				}
				EncodingKind::VEX => Box::into_raw(Box::new(VexHandler::new(dword1, dword2, dword3))) as *const OpCodeHandler,
				EncodingKind::EVEX => Box::into_raw(Box::new(EvexHandler::new(dword1, dword2, dword3))) as *const OpCodeHandler,
				EncodingKind::XOP => Box::into_raw(Box::new(XopHandler::new(dword1, dword2, dword3))) as *const OpCodeHandler,
				EncodingKind::D3NOW => Box::into_raw(Box::new(D3nowHandler::new(dword1, dword2, dword3))) as *const OpCodeHandler,
			};
			v.push(unsafe { &*handler });
		}
		v
	};
}
