// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

use wasm_bindgen::prelude::*;

#[inline(never)]
pub(crate) fn to_js_error(error: iced_x86_rust::IcedError) -> JsValue {
	js_sys::Error::new(&format!("{}", error)).into()
}
