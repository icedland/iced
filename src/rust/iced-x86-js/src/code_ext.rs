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

use super::code::{code_to_iced, Code};
#[cfg(all(feature = "encoder", feature = "op_code_info"))]
use super::op_code_info::OpCodeInfo;
use wasm_bindgen::prelude::*;

/// [`Code`] enum extension methods
///
/// [`Code`]: enum.Code.html
#[wasm_bindgen]
pub struct CodeExt;

#[wasm_bindgen]
#[cfg(all(feature = "encoder", feature = "op_code_info"))]
impl CodeExt {
	/// Gets a [`OpCodeInfo`]
	///
	/// [`OpCodeInfo`]: struct.OpCodeInfo.html
	#[wasm_bindgen(js_name = "opCode")]
	pub fn op_code(code: Code) -> OpCodeInfo {
		OpCodeInfo(code_to_iced(code).op_code())
	}
}
