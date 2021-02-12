// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

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
