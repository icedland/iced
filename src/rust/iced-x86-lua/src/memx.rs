// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::enum_utils::to_memory_size;
use crate::memi::MemorySizeInfo;
use loona::prelude::*;

lua_module! {
	/// `MemorySize` enum extension methods, see also `MemorySizeInfo`
	unsafe fn luaopen_iced_x86_MemorySizeExt(lua) {
		unsafe {
			lua.new_table();

			for &(name, method) in MEMORY_SIZE_EXT_EXPORTS {
				lua.push(name);
				lua.push(method);
				lua.raw_set(-3);
			}
		}
	}
}

lua_pub_methods! { static MEMORY_SIZE_EXT_EXPORTS =>
	/// Gets the memory size info
	///
	/// @param memory_size integer #(A `MemorySize` enum variant) Enum value
	/// @return MemorySizeInfo
	///
	/// # Examples
	///
	/// ```lua
	/// local MemorySize = require("iced_x86.MemorySize")
	/// local MemorySizeExt = require("iced_x86.MemorySizeExt")
	///
	/// local info = MemorySizeExt.info(MemorySize.Packed256_UInt16)
	/// assert(info:size() == 32)
	/// ```
	unsafe fn info(lua, memory_size: u32) -> 1 {
		let _ = unsafe { MemorySizeInfo::init_and_push(lua, &MemorySizeInfo { inner: *to_memory_size(lua, memory_size).info() }) };
	}

	/// Gets the size in bytes of the memory location or 0 if it's not accessed by the instruction or unknown or variable sized
	///
	/// @param memory_size integer #(A `MemorySize` enum variant) Enum value
	/// @return integer #(`u32`) Size in bytes of the memory location or
	///
	/// # Examples
	///
	/// ```lua
	/// local MemorySize = require("iced_x86.MemorySize")
	/// local MemorySizeExt = require("iced_x86.MemorySizeExt")
	///
	/// assert(MemorySizeExt.size(MemorySize.UInt32) == 4)
	/// assert(MemorySizeExt.size(MemorySize.Packed256_UInt16) == 32)
	/// assert(MemorySizeExt.size(MemorySize.Broadcast512_UInt64) == 8)
	/// ```
	unsafe fn size(lua, memory_size: u32) -> 1 {
		unsafe { lua.push(to_memory_size(lua, memory_size).size() as u32); }
	}

	/// Gets the size in bytes of the packed element. If it's not a packed data type, it's equal to `MemorySizeExt.size()`.
	///
	/// @param memory_size integer #(A `MemorySize` enum variant) Enum value
	/// @return integer #(`u32`) Size in bytes of the packed element
	///
	/// # Examples
	///
	/// ```lua
	/// local MemorySize = require("iced_x86.MemorySize")
	/// local MemorySizeExt = require("iced_x86.MemorySizeExt")
	///
	/// assert(MemorySizeExt.element_size(MemorySize.UInt32) == 4)
	/// assert(MemorySizeExt.element_size(MemorySize.Packed256_UInt16) == 2)
	/// assert(MemorySizeExt.element_size(MemorySize.Broadcast512_UInt64) == 8)
	/// ```
	unsafe fn element_size(lua, memory_size: u32) -> 1 {
		unsafe { lua.push(to_memory_size(lua, memory_size).element_size() as u32); }
	}

	/// Gets the element type if it's packed data or the input value if it's not packed data
	///
	/// @param memory_size integer #(A `MemorySize` enum variant) Enum value
	/// @return integer #(A `MemorySize` enum variant) Element type
	///
	/// # Examples
	///
	/// ```lua
	/// local MemorySize = require("iced_x86.MemorySize")
	/// local MemorySizeExt = require("iced_x86.MemorySizeExt")
	///
	/// assert(MemorySizeExt.element_type(MemorySize.UInt32) == MemorySize.UInt32)
	/// assert(MemorySizeExt.element_type(MemorySize.Packed256_UInt16) == MemorySize.UInt16)
	/// assert(MemorySizeExt.element_type(MemorySize.Broadcast512_UInt64) == MemorySize.UInt64)
	/// ```
	unsafe fn element_type(lua, memory_size: u32) -> 1 {
		unsafe { lua.push(to_memory_size(lua, memory_size).element_type() as u32); }
	}

	/// Gets the element type info if it's packed data or the input value if it's not packed data
	///
	/// @param memory_size integer #(A `MemorySize` enum variant) Enum value
	/// @return integer #(A `MemorySizeInfo` enum variant) Element type info
	///
	/// # Examples
	///
	/// ```lua
	/// local MemorySize = require("iced_x86.MemorySize")
	/// local MemorySizeExt = require("iced_x86.MemorySizeExt")
	///
	/// assert(MemorySizeExt.element_type_info(MemorySize.UInt32):memory_size() == MemorySize.UInt32)
	/// assert(MemorySizeExt.element_type_info(MemorySize.Packed256_UInt16):memory_size() == MemorySize.UInt16)
	/// assert(MemorySizeExt.element_type_info(MemorySize.Broadcast512_UInt64):memory_size() == MemorySize.UInt64)
	/// ```
	unsafe fn element_type_info(lua, memory_size: u32) -> 1 {
		let _ = unsafe { MemorySizeInfo::init_and_push(lua, &MemorySizeInfo { inner: *to_memory_size(lua, memory_size).element_type_info() }) };
	}

	/// `true` if it's signed data (signed integer or a floating point value)
	///
	/// @param memory_size integer #(A `MemorySize` enum variant) Enum value
	/// @return boolean #`true` if it's signed data
	///
	/// # Examples
	///
	/// ```lua
	/// local MemorySize = require("iced_x86.MemorySize")
	/// local MemorySizeExt = require("iced_x86.MemorySizeExt")
	///
	/// assert(not MemorySizeExt.is_signed(MemorySize.UInt32))
	/// assert(MemorySizeExt.is_signed(MemorySize.Int32))
	/// assert(MemorySizeExt.is_signed(MemorySize.Float64))
	/// ```
	unsafe fn is_signed(lua, memory_size: u32) -> 1 {
		unsafe { lua.push(to_memory_size(lua, memory_size).is_signed()); }
	}

	/// `true` if this is a packed data type, eg. `MemorySize.Packed128_Float32`
	///
	/// @param memory_size integer #(A `MemorySize` enum variant) Enum value
	/// @return boolean #`true` if this is a packed data type
	///
	/// # Examples
	///
	/// ```lua
	/// local MemorySize = require("iced_x86.MemorySize")
	/// local MemorySizeExt = require("iced_x86.MemorySizeExt")
	///
	/// assert(not MemorySizeExt.is_packed(MemorySize.UInt32))
	/// assert(MemorySizeExt.is_packed(MemorySize.Packed256_UInt16))
	/// assert(not MemorySizeExt.is_packed(MemorySize.Broadcast512_UInt64))
	/// ```
	unsafe fn is_packed(lua, memory_size: u32) -> 1 {
		unsafe { lua.push(to_memory_size(lua, memory_size).is_packed()); }
	}

	/// Gets the number of elements in the packed data type or `1` if it's not packed data (`MemorySizeExt.is_packed()`)
	///
	/// @param memory_size integer #(A `MemorySize` enum variant) Enum value
	/// @return integer #(`u32`) Number of elements in the packed data type
	///
	/// # Examples
	///
	/// ```lua
	/// local MemorySize = require("iced_x86.MemorySize")
	/// local MemorySizeExt = require("iced_x86.MemorySizeExt")
	///
	/// assert(MemorySizeExt.element_count(MemorySize.UInt32) == 1)
	/// assert(MemorySizeExt.element_count(MemorySize.Packed256_UInt16) == 16)
	/// assert(MemorySizeExt.element_count(MemorySize.Broadcast512_UInt64) == 1)
	/// ```
	unsafe fn element_count(lua, memory_size: u32) -> 1 {
		unsafe { lua.push(to_memory_size(lua, memory_size).element_count() as u32); }
	}

	/// `true` if it is a broadcast memory type
	///
	/// @param memory_size integer #(A `MemorySize` enum variant) Enum value
	/// @return boolean #`true` if it is a broadcast memory type
	///
	/// # Examples
	///
	/// ```lua
	/// local MemorySize = require("iced_x86.MemorySize")
	/// local MemorySizeExt = require("iced_x86.MemorySizeExt")
	///
	/// assert(not MemorySizeExt.is_broadcast(MemorySize.Packed64_Float16))
	/// assert(MemorySizeExt.is_broadcast(MemorySize.Broadcast512_UInt64))
	/// ```
	unsafe fn is_broadcast(lua, memory_size: u32) -> 1 {
		unsafe { lua.push(to_memory_size(lua, memory_size).is_broadcast()); }
	}
}
