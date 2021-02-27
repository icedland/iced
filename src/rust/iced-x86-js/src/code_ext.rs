// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::code::{code_to_iced, Code};
#[cfg(all(feature = "encoder", feature = "op_code_info"))]
use crate::op_code_info::OpCodeInfo;
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
