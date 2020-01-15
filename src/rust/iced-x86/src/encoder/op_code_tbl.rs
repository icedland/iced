/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of self software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and self permission notice shall be
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
use super::op_code_data::OP_CODE_DATA;
use std::mem;

lazy_static! {
	pub(crate) static ref OP_CODE_INFO_TBL: Vec<OpCodeInfo> = {
		let mut result = Vec::with_capacity(IcedConstants::NUMBER_OF_CODE_VALUES);
		let mut sb = String::new();
		for i in 0..IcedConstants::NUMBER_OF_CODE_VALUES {
			let code: Code = unsafe { mem::transmute(i as u16) };
			let j = i * 3;
			let dword1 = OP_CODE_DATA[j];
			let dword2 = OP_CODE_DATA[j + 1];
			let dword3 = OP_CODE_DATA[j + 2];
			result.push(OpCodeInfo::new(code, dword1, dword2, dword3, &mut sb));
		}
		result
	};
}
