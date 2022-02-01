// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::register::{register_to_iced, Register};
use wasm_bindgen::prelude::*;

/// Memory operand passed to one of [`Instruction`]'s `create*()` constructor methods
///
/// [`Instruction`]: struct.Instruction.html
#[wasm_bindgen]
pub struct MemoryOperand(pub(crate) iced_x86_rust::MemoryOperand);

#[wasm_bindgen]
impl MemoryOperand {
	/// Constructor
	///
	/// # Arguments
	///
	/// * `base`: Base register or [`Register.None`] (a [`Register`] value)
	/// * `index`: Index register or [`Register.None`] (a [`Register`] value)
	/// * `scale`: Index register scale (1, 2, 4, or 8)
	/// * `displacement`: Memory displacement
	/// * `displSize`: 0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a `i8`), 2 (16-bit), 4 (32-bit) or 8 (64-bit)
	/// * `isBroadcast`: `true` if it's broadcast memory (EVEX instructions)
	/// * `segmentPrefix`: Segment override or [`Register.None`] (a [`Register`] value)
	///
	/// [`Register`]: enum.Register.html
	/// [`Register.None`]: enum.Register.html#variant.None
	#[wasm_bindgen(constructor)]
	pub fn new(
		base: Register, index: Register, scale: u32, displacement: i64, #[allow(non_snake_case)] displSize: u32,
		#[allow(non_snake_case)] isBroadcast: bool, #[allow(non_snake_case)] segmentPrefix: Register,
	) -> Self {
		Self(iced_x86_rust::MemoryOperand::new(
			register_to_iced(base),
			register_to_iced(index),
			scale,
			displacement,
			displSize,
			isBroadcast,
			register_to_iced(segmentPrefix),
		))
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// * `base`: Base register or [`Register.None`] (a [`Register`] value)
	/// * `index`: Index register or [`Register.None`] (a [`Register`] value)
	/// * `scale`: Index register scale (1, 2, 4, or 8)
	/// * `displacement`: Memory displacement
	/// * `displSize`: 0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a `i8`), 2 (16-bit), 4 (32-bit) or 8 (64-bit)
	/// * `isBroadcast`: `true` if it's broadcast memory (EVEX instructions)
	/// * `segmentPrefix`: Segment override or [`Register.None`] (a [`Register`] value)
	///
	/// [`Register`]: enum.Register.html
	/// [`Register.None`]: enum.Register.html#variant.None
	#[wasm_bindgen(js_name = "new64")]
	pub fn new64(
		base: Register, index: Register, scale: u32, displacement: u64, #[allow(non_snake_case)] displSize: u32,
		#[allow(non_snake_case)] isBroadcast: bool, #[allow(non_snake_case)] segmentPrefix: Register,
	) -> Self {
		Self(iced_x86_rust::MemoryOperand::new(
			register_to_iced(base),
			register_to_iced(index),
			scale,
			displacement as i64,
			displSize,
			isBroadcast,
			register_to_iced(segmentPrefix),
		))
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// * `base`: Base register or [`Register.None`] (a [`Register`] value)
	/// * `index`: Index register or [`Register.None`] (a [`Register`] value)
	/// * `scale`: Index register scale (1, 2, 4, or 8)
	/// * `isBroadcast`: `true` if it's broadcast memory (EVEX instructions)
	/// * `segmentPrefix`: Segment override or [`Register.None`] (a [`Register`] value)
	///
	/// [`Register`]: enum.Register.html
	/// [`Register.None`]: enum.Register.html#variant.None
	#[wasm_bindgen(js_name = "createBaseIndexScaleBcstSeg")]
	pub fn with_base_index_scale_bcst_seg(
		base: Register, index: Register, scale: u32, #[allow(non_snake_case)] isBroadcast: bool, #[allow(non_snake_case)] segmentPrefix: Register,
	) -> Self {
		Self(iced_x86_rust::MemoryOperand::with_base_index_scale_bcst_seg(
			register_to_iced(base),
			register_to_iced(index),
			scale,
			isBroadcast,
			register_to_iced(segmentPrefix),
		))
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// * `base`: Base register or [`Register.None`] (a [`Register`] value)
	/// * `displacement`: Memory displacement
	/// * `displSize`: 0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a `i8`), 2 (16-bit), 4 (32-bit) or 8 (64-bit)
	/// * `isBroadcast`: `true` if it's broadcast memory (EVEX instructions)
	/// * `segmentPrefix`: Segment override or [`Register.None`] (a [`Register`] value)
	///
	/// [`Register`]: enum.Register.html
	/// [`Register.None`]: enum.Register.html#variant.None
	#[wasm_bindgen(js_name = "createBaseDisplSizeBcstSeg")]
	pub fn with_base_displ_size_bcst_seg(
		base: Register, displacement: i64, #[allow(non_snake_case)] displSize: u32, #[allow(non_snake_case)] isBroadcast: bool,
		#[allow(non_snake_case)] segmentPrefix: Register,
	) -> Self {
		Self(iced_x86_rust::MemoryOperand::with_base_displ_size_bcst_seg(
			register_to_iced(base),
			displacement,
			displSize,
			isBroadcast,
			register_to_iced(segmentPrefix),
		))
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// * `index`: Index register or [`Register.None`] (a [`Register`] value)
	/// * `scale`: Index register scale (1, 2, 4, or 8)
	/// * `displacement`: Memory displacement
	/// * `displSize`: 0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a `i8`), 2 (16-bit), 4 (32-bit) or 8 (64-bit)
	/// * `isBroadcast`: `true` if it's broadcast memory (EVEX instructions)
	/// * `segmentPrefix`: Segment override or [`Register.None`] (a [`Register`] value)
	///
	/// [`Register`]: enum.Register.html
	/// [`Register.None`]: enum.Register.html#variant.None
	#[wasm_bindgen(js_name = "createIndexScaleDisplSizeBcstSeg")]
	pub fn with_index_scale_displ_size_bcst_seg(
		index: Register, scale: u32, displacement: i64, #[allow(non_snake_case)] displSize: u32, #[allow(non_snake_case)] isBroadcast: bool,
		#[allow(non_snake_case)] segmentPrefix: Register,
	) -> Self {
		Self(iced_x86_rust::MemoryOperand::with_index_scale_displ_size_bcst_seg(
			register_to_iced(index),
			scale,
			displacement,
			displSize,
			isBroadcast,
			register_to_iced(segmentPrefix),
		))
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// * `base`: Base register or [`Register.None`] (a [`Register`] value)
	/// * `displacement`: Memory displacement
	/// * `isBroadcast`: `true` if it's broadcast memory (EVEX instructions)
	/// * `segmentPrefix`: Segment override or [`Register.None`] (a [`Register`] value)
	///
	/// [`Register`]: enum.Register.html
	/// [`Register.None`]: enum.Register.html#variant.None
	#[wasm_bindgen(js_name = "createBaseDisplBcstSeg")]
	pub fn with_base_displ_bcst_seg(
		base: Register, displacement: i64, #[allow(non_snake_case)] isBroadcast: bool, #[allow(non_snake_case)] segmentPrefix: Register,
	) -> Self {
		Self(iced_x86_rust::MemoryOperand::with_base_displ_bcst_seg(
			register_to_iced(base),
			displacement,
			isBroadcast,
			register_to_iced(segmentPrefix),
		))
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// * `base`: Base register or [`Register.None`] (a [`Register`] value)
	/// * `index`: Index register or [`Register.None`] (a [`Register`] value)
	/// * `scale`: Index register scale (1, 2, 4, or 8)
	/// * `displacement`: Memory displacement
	/// * `displSize`: 0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a `i8`), 2 (16-bit), 4 (32-bit) or 8 (64-bit)
	///
	/// [`Register`]: enum.Register.html
	/// [`Register.None`]: enum.Register.html#variant.None
	#[wasm_bindgen(js_name = "createBaseIndexScaleDisplSize")]
	pub fn with_base_index_scale_displ_size(
		base: Register, index: Register, scale: u32, displacement: i64, #[allow(non_snake_case)] displSize: u32,
	) -> Self {
		Self(iced_x86_rust::MemoryOperand::with_base_index_scale_displ_size(
			register_to_iced(base),
			register_to_iced(index),
			scale,
			displacement,
			displSize,
		))
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// * `base`: Base register or [`Register.None`] (a [`Register`] value)
	/// * `index`: Index register or [`Register.None`] (a [`Register`] value)
	/// * `scale`: Index register scale (1, 2, 4, or 8)
	///
	/// [`Register`]: enum.Register.html
	/// [`Register.None`]: enum.Register.html#variant.None
	#[wasm_bindgen(js_name = "createBaseIndexScale")]
	pub fn with_base_index_scale(base: Register, index: Register, scale: u32) -> Self {
		Self(iced_x86_rust::MemoryOperand::with_base_index_scale(register_to_iced(base), register_to_iced(index), scale))
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// * `base`: Base register or [`Register.None`] (a [`Register`] value)
	/// * `index`: Index register or [`Register.None`] (a [`Register`] value)
	///
	/// [`Register`]: enum.Register.html
	/// [`Register.None`]: enum.Register.html#variant.None
	#[wasm_bindgen(js_name = "createBaseIndex")]
	pub fn with_base_index(base: Register, index: Register) -> Self {
		Self(iced_x86_rust::MemoryOperand::with_base_index(register_to_iced(base), register_to_iced(index)))
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// * `base`: Base register or [`Register.None`] (a [`Register`] value)
	/// * `displacement`: Memory displacement
	/// * `displSize`: 0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a `i8`), 2 (16-bit), 4 (32-bit) or 8 (64-bit)
	///
	/// [`Register`]: enum.Register.html
	/// [`Register.None`]: enum.Register.html#variant.None
	#[wasm_bindgen(js_name = "createBaseDisplSize")]
	pub fn with_base_displ_size(base: Register, displacement: i64, #[allow(non_snake_case)] displSize: u32) -> Self {
		Self(iced_x86_rust::MemoryOperand::with_base_displ_size(register_to_iced(base), displacement, displSize))
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// * `index`: Index register or [`Register.None`] (a [`Register`] value)
	/// * `scale`: Index register scale (1, 2, 4, or 8)
	/// * `displacement`: Memory displacement
	/// * `displSize`: 0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a `i8`), 2 (16-bit), 4 (32-bit) or 8 (64-bit)
	///
	/// [`Register`]: enum.Register.html
	/// [`Register.None`]: enum.Register.html#variant.None
	#[wasm_bindgen(js_name = "createIndexScaleDisplSize")]
	pub fn with_index_scale_displ_size(index: Register, scale: u32, displacement: i64, #[allow(non_snake_case)] displSize: u32) -> Self {
		Self(iced_x86_rust::MemoryOperand::with_index_scale_displ_size(register_to_iced(index), scale, displacement, displSize))
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// * `base`: Base register or [`Register.None`] (a [`Register`] value)
	/// * `displacement`: Memory displacement
	///
	/// [`Register`]: enum.Register.html
	/// [`Register.None`]: enum.Register.html#variant.None
	#[wasm_bindgen(js_name = "createBaseDispl")]
	pub fn with_base_displ(base: Register, displacement: i64) -> Self {
		Self(iced_x86_rust::MemoryOperand::with_base_displ(register_to_iced(base), displacement))
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// * `base`: Base register or [`Register.None`] (a [`Register`] value)
	///
	/// [`Register`]: enum.Register.html
	/// [`Register.None`]: enum.Register.html#variant.None
	#[wasm_bindgen(js_name = "createBase")]
	pub fn with_base(base: Register) -> Self {
		Self(iced_x86_rust::MemoryOperand::with_base(register_to_iced(base)))
	}
}
