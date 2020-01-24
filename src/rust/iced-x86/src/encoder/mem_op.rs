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

use super::super::*;

/// Memory operand passed to one of [`Instruction`]'s `with_*()` constructor methods
///
/// [`Instruction`]: struct.Instruction.html
#[derive(Debug, Default, Copy, Clone, Eq, PartialEq, Hash)]
pub struct MemoryOperand {
	/// Segment override or [`Register::None`]
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	pub segment_prefix: Register,

	/// Base register or [`Register::None`]
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	pub base: Register,

	/// Index register or [`Register::None`]
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	pub index: Register,

	/// Index register scale (1, 2, 4, or 8)
	pub scale: u32,

	/// Memory displacement
	pub displacement: i32,

	/// 0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a `i8`), 2 (16-bit), 4 (32-bit) or 8 (64-bit)
	pub displ_size: u32,

	/// `true` if it's broadcasted memory (EVEX instructions)
	pub is_broadcast: bool,
}

impl MemoryOperand {
	/// Constructor
	///
	/// # Arguments
	///
	/// * `base`: Base register or [`Register::None`]
	/// * `index`: Index register or [`Register::None`]
	/// * `scale`: Index register scale (1, 2, 4, or 8)
	/// * `displacement`: Memory displacement
	/// * `displ_size`: 0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a `i8`), 2 (16-bit), 4 (32-bit) or 8 (64-bit)
	/// * `is_broadcast`: `true` if it's broadcasted memory (EVEX instructions)
	/// * `segment_prefix`: Segment override or [`Register::None`]
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn new(
		base: Register, index: Register, scale: u32, displacement: i32, displ_size: u32, is_broadcast: bool, segment_prefix: Register,
	) -> Self {
		Self { segment_prefix, base, index, scale, displacement, displ_size, is_broadcast }
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// * `base`: Base register or [`Register::None`]
	/// * `index`: Index register or [`Register::None`]
	/// * `scale`: Index register scale (1, 2, 4, or 8)
	/// * `is_broadcast`: `true` if it's broadcasted memory (EVEX instructions)
	/// * `segment_prefix`: Segment override or [`Register::None`]
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_base_index_scale_bcst_seg(base: Register, index: Register, scale: u32, is_broadcast: bool, segment_prefix: Register) -> Self {
		Self { segment_prefix, base, index, scale, displacement: 0, displ_size: 0, is_broadcast }
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// * `base`: Base register or [`Register::None`]
	/// * `displacement`: Memory displacement
	/// * `displ_size`: 0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a `i8`), 2 (16-bit), 4 (32-bit) or 8 (64-bit)
	/// * `is_broadcast`: `true` if it's broadcasted memory (EVEX instructions)
	/// * `segment_prefix`: Segment override or [`Register::None`]
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_base_displ_size_bcst_seg(base: Register, displacement: i32, displ_size: u32, is_broadcast: bool, segment_prefix: Register) -> Self {
		Self { segment_prefix, base, index: Register::None, scale: 1, displacement, displ_size, is_broadcast }
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// * `index`: Index register or [`Register::None`]
	/// * `scale`: Index register scale (1, 2, 4, or 8)
	/// * `displacement`: Memory displacement
	/// * `displ_size`: 0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a `i8`), 2 (16-bit), 4 (32-bit) or 8 (64-bit)
	/// * `is_broadcast`: `true` if it's broadcasted memory (EVEX instructions)
	/// * `segment_prefix`: Segment override or [`Register::None`]
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_index_scale_displ_size_bcst_seg(
		index: Register, scale: u32, displacement: i32, displ_size: u32, is_broadcast: bool, segment_prefix: Register,
	) -> Self {
		Self { segment_prefix, base: Register::None, index, scale, displacement, displ_size, is_broadcast }
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// * `base`: Base register or [`Register::None`]
	/// * `displacement`: Memory displacement
	/// * `is_broadcast`: `true` if it's broadcasted memory (EVEX instructions)
	/// * `segment_prefix`: Segment override or [`Register::None`]
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_base_displ_bcst_seg(base: Register, displacement: i32, is_broadcast: bool, segment_prefix: Register) -> Self {
		Self { segment_prefix, base, index: Register::None, scale: 1, displacement, displ_size: 1, is_broadcast }
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// * `base`: Base register or [`Register::None`]
	/// * `index`: Index register or [`Register::None`]
	/// * `scale`: Index register scale (1, 2, 4, or 8)
	/// * `displacement`: Memory displacement
	/// * `displ_size`: 0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a `i8`), 2 (16-bit), 4 (32-bit) or 8 (64-bit)
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_base_index_scale_displ_size(base: Register, index: Register, scale: u32, displacement: i32, displ_size: u32) -> Self {
		Self { segment_prefix: Register::None, base, index, scale, displacement, displ_size, is_broadcast: false }
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// * `base`: Base register or [`Register::None`]
	/// * `index`: Index register or [`Register::None`]
	/// * `scale`: Index register scale (1, 2, 4, or 8)
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_base_index_scale(base: Register, index: Register, scale: u32) -> Self {
		Self { segment_prefix: Register::None, base, index, scale, displacement: 0, displ_size: 0, is_broadcast: false }
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// * `base`: Base register or [`Register::None`]
	/// * `index`: Index register or [`Register::None`]
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_base_index(base: Register, index: Register) -> Self {
		Self { segment_prefix: Register::None, base, index, scale: 1, displacement: 0, displ_size: 0, is_broadcast: false }
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// * `base`: Base register or [`Register::None`]
	/// * `displacement`: Memory displacement
	/// * `displ_size`: 0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a `i8`), 2 (16-bit), 4 (32-bit) or 8 (64-bit)
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_base_displ_size(base: Register, displacement: i32, displ_size: u32) -> Self {
		Self { segment_prefix: Register::None, base, index: Register::None, scale: 1, displacement, displ_size, is_broadcast: false }
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// * `index`: Index register or [`Register::None`]
	/// * `scale`: Index register scale (1, 2, 4, or 8)
	/// * `displacement`: Memory displacement
	/// * `displ_size`: 0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a `i8`), 2 (16-bit), 4 (32-bit) or 8 (64-bit)
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_index_scale_displ_size(index: Register, scale: u32, displacement: i32, displ_size: u32) -> Self {
		Self { segment_prefix: Register::None, base: Register::None, index, scale, displacement, displ_size, is_broadcast: false }
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// * `base`: Base register or [`Register::None`]
	/// * `displacement`: Memory displacement
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_base_displ(base: Register, displacement: i32) -> Self {
		Self { segment_prefix: Register::None, base, index: Register::None, scale: 1, displacement, displ_size: 1, is_broadcast: false }
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// * `base`: Base register or [`Register::None`]
	///
	/// [`Register::None`]: enum.Register.html#variant.None
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_base(base: Register) -> Self {
		Self { segment_prefix: Register::None, base, index: Register::None, scale: 1, displacement: 0, displ_size: 0, is_broadcast: false }
	}
}
