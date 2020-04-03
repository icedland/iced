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

use wasm_bindgen::prelude::*;

/// Contains the offsets of the displacement and immediate. Call [`Decoder.getConstantOffsets()`] or
/// [`Encoder.getConstantOffsets()`] to get the offsets of the constants after the instruction has been
/// decoded/encoded.
///
/// [`Decoder.getConstantOffsets()`]: struct.Decoder.html#method.get_constant_offsets
/// [`Encoder.getConstantOffsets()`]: struct.Encoder.html#method.get_constant_offsets
#[wasm_bindgen]
pub struct ConstantOffsets(pub(crate) iced_x86::ConstantOffsets);

#[wasm_bindgen]
impl ConstantOffsets {
	/// The offset of the displacement, if any
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "displacementOffset")]
	pub fn displacement_offset(&self) -> usize {
		self.0.displacement_offset()
	}

	/// Size in bytes of the displacement, or 0 if there's no displacement
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "displacementSize")]
	pub fn displacement_size(&self) -> usize {
		self.0.displacement_size()
	}

	/// The offset of the first immediate, if any.
	///
	/// This field can be invalid even if the operand has an immediate if it's an immediate that isn't part
	/// of the instruction stream, eg. `SHL AL,1`.
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "immediateOffset")]
	pub fn immediate_offset(&self) -> usize {
		self.0.immediate_offset()
	}

	/// Size in bytes of the first immediate, or 0 if there's no immediate
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "immediateSize")]
	pub fn immediate_size(&self) -> usize {
		self.0.immediate_size()
	}

	/// The offset of the second immediate, if any.
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "immediateOffset2")]
	pub fn immediate_offset2(&self) -> usize {
		self.0.immediate_offset2()
	}

	/// Size in bytes of the second immediate, or 0 if there's no second immediate
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "immediateSize2")]
	pub fn immediate_size2(&self) -> usize {
		self.0.immediate_size2()
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
