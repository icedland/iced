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

/// Contains the offsets of the displacement and immediate. Call `Decoder::get_constant_offsets()` or
/// `Encoder::get_constant_offsets()` to get the offsets of the constants after the instruction has been
/// decoded/encoded.
#[derive(Copy, Clone, Default)]
#[allow(dead_code)]
pub struct ConstantOffsets {
	pub(crate) displacement_offset: u8,
	pub(crate) displacement_size: u8,
	pub(crate) immediate_offset: u8,
	pub(crate) immediate_size: u8,
	pub(crate) immediate_offset2: u8,
	pub(crate) immediate_size2: u8,
	pad1: u8,
	pad2: u8,
}

#[cfg_attr(feature = "cargo-clippy", allow(clippy::trivially_copy_pass_by_ref))]
impl ConstantOffsets {
	/// The offset of the displacement, if any
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn displacement_offset(&self) -> u32 {
		self.displacement_offset as u32
	}

	/// Size in bytes of the displacement, or 0 if there's no displacement
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn displacement_size(&self) -> u32 {
		self.displacement_size as u32
	}

	/// The offset of the first immediate, if any.
	///
	/// This field can be invalid even if the operand has an immediate if it's an immediate that isn't part
	/// of the instruction stream, eg. `SHL AL,1`.
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn immediate_offset(&self) -> u32 {
		self.immediate_offset as u32
	}

	/// Size in bytes of the first immediate, or 0 if there's no immediate
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn immediate_size(&self) -> u32 {
		self.immediate_size as u32
	}

	/// The offset of the second immediate, if any.
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn immediate_offset2(&self) -> u32 {
		self.immediate_offset2 as u32
	}

	/// Size in bytes of the second immediate, or 0 if there's no second immediate
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn immediate_size2(&self) -> u32 {
		self.immediate_size2 as u32
	}

	/// true if `displacement_offset()` is valid
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn has_displacement(&self) -> bool {
		self.displacement_size != 0
	}

	/// true if `immediate_offset()` is valid
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn has_immediate(&self) -> bool {
		self.immediate_size != 0
	}

	/// true if `immediate_offset2()` is valid
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn has_immediate2(&self) -> bool {
		self.immediate_size2 != 0
	}
}
