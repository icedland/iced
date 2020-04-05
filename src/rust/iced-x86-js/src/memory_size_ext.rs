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

use super::memory_size::{iced_to_memory_size, memory_size_to_iced, MemorySize};
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
	/// ```
	/// use iced_x86::*;
	/// assert_eq!(4, MemorySize::UInt32.size());
	/// assert_eq!(32, MemorySize::Packed256_UInt16.size());
	/// assert_eq!(8, MemorySize::Broadcast512_UInt64.size());
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
	/// ```
	/// use iced_x86::*;
	/// assert_eq!(4, MemorySize::UInt32.element_size());
	/// assert_eq!(2, MemorySize::Packed256_UInt16.element_size());
	/// assert_eq!(8, MemorySize::Broadcast512_UInt64.element_size());
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
	/// ```
	/// use iced_x86::*;
	/// assert_eq!(MemorySize::UInt32, MemorySize::UInt32.element_type());
	/// assert_eq!(MemorySize::UInt16, MemorySize::Packed256_UInt16.element_type());
	/// assert_eq!(MemorySize::UInt64, MemorySize::Broadcast512_UInt64.element_type());
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
	/// ```
	/// use iced_x86::*;
	/// assert!(!MemorySize::UInt32.is_signed());
	/// assert!(MemorySize::Int32.is_signed());
	/// assert!(MemorySize::Float64.is_signed());
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
	/// ```
	/// use iced_x86::*;
	/// assert!(!MemorySize::UInt32.is_packed());
	/// assert!(MemorySize::Packed256_UInt16.is_packed());
	/// assert!(!MemorySize::Broadcast512_UInt64.is_packed());
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
	/// ```
	/// use iced_x86::*;
	/// assert_eq!(1, MemorySize::UInt32.element_count());
	/// assert_eq!(16, MemorySize::Packed256_UInt16.element_count());
	/// assert_eq!(1, MemorySize::Broadcast512_UInt64.element_count());
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
	/// ```
	/// use iced_x86::*;
	/// assert!(!MemorySize::Packed64_Float16.is_broadcast());
	/// assert!(MemorySize::Broadcast512_UInt64.is_broadcast());
	/// ```
	#[wasm_bindgen(js_name = "isBroadcast")]
	pub fn is_broadcast(value: MemorySize) -> bool {
		memory_size_to_iced(value).is_broadcast()
	}
}
