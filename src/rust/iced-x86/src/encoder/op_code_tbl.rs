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

use super::super::iced_constants::IcedConstants;
use super::super::{Code, OpCodeInfo};
use super::encoder_data::{ENC_FLAGS1, ENC_FLAGS2, ENC_FLAGS3};
use super::op_code_data::{OPC_FLAGS1, OPC_FLAGS2};
#[cfg(not(feature = "std"))]
use alloc::string::String;
#[cfg(not(feature = "std"))]
use alloc::vec::Vec;
use core::mem;

lazy_static! {
	pub(crate) static ref OP_CODE_INFO_TBL: Vec<OpCodeInfo> = {
		let mut result = Vec::with_capacity(IcedConstants::CODE_ENUM_COUNT);
		let mut sb = String::new();
		for i in 0..IcedConstants::CODE_ENUM_COUNT {
			let code: Code = unsafe { mem::transmute(i as u16) };
			let enc_flags1 = ENC_FLAGS1[i];
			let enc_flags2 = ENC_FLAGS2[i];
			let enc_flags3 = ENC_FLAGS3[i];
			let opc_flags1 = OPC_FLAGS1[i];
			let opc_flags2 = OPC_FLAGS2[i];
			result.push(OpCodeInfo::new(code, enc_flags1, enc_flags2, enc_flags3, opc_flags1, opc_flags2, &mut sb));
		}
		result
	};
}
