// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::memory_size::{iced_to_memory_size, memory_size_to_iced, MemorySize};
use wasm_bindgen::prelude::*;

/// [`MemorySize`] enum extension methods
///
/// [`MemorySize`]: enum.MemorySize.html
#[wasm_bindgen]
pub struct MemorySizeExt;

#[wasm_bindgen]
impl MemorySizeExt {
	/// Gets the size in bytes of the memory location or 0 if it's not accessed by the instruction or unknown or variable sized
	///
	/// # Arguments
	///
	/// - `value`: A [`MemorySize`] enum value
	///
	/// [`MemorySize`]: enum.MemorySize.html
	///
	/// # Examples
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { MemorySize, MemorySizeExt } = require("iced-x86");
	///
	/// assert.equal(MemorySizeExt.size(MemorySize.UInt32), 4);
	/// assert.equal(MemorySizeExt.size(MemorySize.Packed256_UInt16), 32);
	/// assert.equal(MemorySizeExt.size(MemorySize.Broadcast512_UInt64), 8);
	/// ```
	pub fn size(value: MemorySize) -> u32 {
		memory_size_to_iced(value).size() as u32
	}

	/// Gets the size in bytes of the packed element. If it's not a packed data type, it's equal to [`MemorySizeExt.size()`].
	///
	/// # Arguments
	///
	/// - `value`: A [`MemorySize`] enum value
	///
	/// [`MemorySize`]: enum.MemorySize.html
	/// [`MemorySizeExt.size()`]: #method.size
	///
	/// # Examples
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { MemorySize, MemorySizeExt } = require("iced-x86");
	///
	/// assert.equal(MemorySizeExt.elementSize(MemorySize.UInt32), 4);
	/// assert.equal(MemorySizeExt.elementSize(MemorySize.Packed256_UInt16), 2);
	/// assert.equal(MemorySizeExt.elementSize(MemorySize.Broadcast512_UInt64), 8);
	/// ```
	#[wasm_bindgen(js_name = "elementSize")]
	pub fn element_size(value: MemorySize) -> u32 {
		memory_size_to_iced(value).element_size() as u32
	}

	/// Gets the element type if it's packed data or `value` if it's not packed data
	///
	/// # Arguments
	///
	/// - `value`: A [`MemorySize`] enum value
	///
	/// [`MemorySize`]: enum.MemorySize.html
	///
	/// # Examples
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { MemorySize, MemorySizeExt } = require("iced-x86");
	///
	/// assert.equal(MemorySizeExt.elementType(MemorySize.UInt32), MemorySize.UInt32);
	/// assert.equal(MemorySizeExt.elementType(MemorySize.Packed256_UInt16), MemorySize.UInt16);
	/// assert.equal(MemorySizeExt.elementType(MemorySize.Broadcast512_UInt64), MemorySize.UInt64);
	/// ```
	#[wasm_bindgen(js_name = "elementType")]
	pub fn element_type(value: MemorySize) -> MemorySize {
		iced_to_memory_size(memory_size_to_iced(value).element_type())
	}

	/// `true` if it's signed data (signed integer or a floating point value)
	///
	/// # Arguments
	///
	/// - `value`: A [`MemorySize`] enum value
	///
	/// [`MemorySize`]: enum.MemorySize.html
	///
	/// # Examples
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { MemorySize, MemorySizeExt } = require("iced-x86");
	///
	/// assert.ok(!MemorySizeExt.isSigned(MemorySize.UInt32));
	/// assert.ok(MemorySizeExt.isSigned(MemorySize.Int32));
	/// assert.ok(MemorySizeExt.isSigned(MemorySize.Float64));
	/// ```
	#[wasm_bindgen(js_name = "isSigned")]
	pub fn is_signed(value: MemorySize) -> bool {
		memory_size_to_iced(value).is_signed()
	}

	/// `true` if this is a packed data type, eg. [`MemorySize.Packed128_Float32`]
	///
	/// # Arguments
	///
	/// - `value`: A [`MemorySize`] enum value
	///
	/// [`MemorySize`]: enum.MemorySize.html
	/// [`MemorySize.Packed128_Float32`]: #variant.Packed128_Float32
	///
	/// # Examples
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { MemorySize, MemorySizeExt } = require("iced-x86");
	///
	/// assert.ok(!MemorySizeExt.isPacked(MemorySize.UInt32));
	/// assert.ok(MemorySizeExt.isPacked(MemorySize.Packed256_UInt16));
	/// assert.ok(!MemorySizeExt.isPacked(MemorySize.Broadcast512_UInt64));
	/// ```
	#[wasm_bindgen(js_name = "isPacked")]
	pub fn is_packed(value: MemorySize) -> bool {
		memory_size_to_iced(value).is_packed()
	}

	/// Gets the number of elements in the packed data type or `1` if it's not packed data ([`MemorySizeExt.isPacked()`])
	///
	/// # Arguments
	///
	/// - `value`: A [`MemorySize`] enum value
	///
	/// [`MemorySize`]: enum.MemorySize.html
	/// [`MemorySizeExt.isPacked()`]: #method.is_packed
	///
	/// # Examples
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { MemorySize, MemorySizeExt } = require("iced-x86");
	///
	/// assert.equal(MemorySizeExt.elementCount(MemorySize.UInt32), 1);
	/// assert.equal(MemorySizeExt.elementCount(MemorySize.Packed256_UInt16), 16);
	/// assert.equal(MemorySizeExt.elementCount(MemorySize.Broadcast512_UInt64), 1);
	/// ```
	#[wasm_bindgen(js_name = "elementCount")]
	pub fn element_count(value: MemorySize) -> u32 {
		memory_size_to_iced(value).element_count() as u32
	}

	/// Checks if it is a broadcast memory type
	///
	/// # Arguments
	///
	/// - `value`: A [`MemorySize`] enum value
	///
	/// [`MemorySize`]: enum.MemorySize.html
	///
	/// # Examples
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { MemorySize, MemorySizeExt } = require("iced-x86");
	///
	/// assert.ok(!MemorySizeExt.isBroadcast(MemorySize.Packed64_Float16));
	/// assert.ok(MemorySizeExt.isBroadcast(MemorySize.Broadcast512_UInt64));
	/// ```
	#[wasm_bindgen(js_name = "isBroadcast")]
	pub fn is_broadcast(value: MemorySize) -> bool {
		memory_size_to_iced(value).is_broadcast()
	}
}
