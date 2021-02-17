// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use wasm_bindgen::prelude::*;

/// Contains the offsets of the displacement and immediate. Call [`Decoder.getConstantOffsets()`] or
/// [`Encoder.getConstantOffsets()`] to get the offsets of the constants after the instruction has been
/// decoded/encoded.
///
/// [`Decoder.getConstantOffsets()`]: struct.Decoder.html#method.get_constant_offsets
/// [`Encoder.getConstantOffsets()`]: struct.Encoder.html#method.get_constant_offsets
#[wasm_bindgen]
pub struct ConstantOffsets(pub(crate) iced_x86_rust::ConstantOffsets);

#[wasm_bindgen]
impl ConstantOffsets {
	/// The offset of the displacement, if any
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "displacementOffset")]
	pub fn displacement_offset(&self) -> u32 {
		self.0.displacement_offset() as u32
	}

	/// Size in bytes of the displacement, or 0 if there's no displacement
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "displacementSize")]
	pub fn displacement_size(&self) -> u32 {
		self.0.displacement_size() as u32
	}

	/// The offset of the first immediate, if any.
	///
	/// This field can be invalid even if the operand has an immediate if it's an immediate that isn't part
	/// of the instruction stream, eg. `SHL AL,1`.
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "immediateOffset")]
	pub fn immediate_offset(&self) -> u32 {
		self.0.immediate_offset() as u32
	}

	/// Size in bytes of the first immediate, or 0 if there's no immediate
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "immediateSize")]
	pub fn immediate_size(&self) -> u32 {
		self.0.immediate_size() as u32
	}

	/// The offset of the second immediate, if any.
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "immediateOffset2")]
	pub fn immediate_offset2(&self) -> u32 {
		self.0.immediate_offset2() as u32
	}

	/// Size in bytes of the second immediate, or 0 if there's no second immediate
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "immediateSize2")]
	pub fn immediate_size2(&self) -> u32 {
		self.0.immediate_size2() as u32
	}

	/// `true` if [`displacementOffset`] and [`displacementSize`] are valid
	///
	/// [`displacementOffset`]: #method.displacement_offset
	/// [`displacementSize`]: #method.displacement_size
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "hasDisplacement")]
	pub fn has_displacement(&self) -> bool {
		self.0.has_displacement()
	}

	/// `true` if [`immediateOffset`] and [`immediateSize`] are valid
	///
	/// [`immediateOffset`]: #method.immediate_offset
	/// [`immediateSize`]: #method.immediate_size
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "hasImmediate")]
	pub fn has_immediate(&self) -> bool {
		self.0.has_immediate()
	}

	/// `true` if [`immediateOffset2`] and [`immediateSize2`] are valid
	///
	/// [`immediateOffset2`]: #method.immediate_offset2
	/// [`immediateSize2`]: #method.immediate_size2
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "hasImmediate2")]
	pub fn has_immediate2(&self) -> bool {
		self.0.has_immediate2()
	}
}
