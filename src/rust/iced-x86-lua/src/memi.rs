// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::enum_utils::to_memory_size;
use loona::prelude::*;

lua_struct_module! { luaopen_iced_x86_MemorySizeInfo : MemorySizeInfo }
lua_impl_userdata! { MemorySizeInfo }

/// `MemorySize` enum info, see also `MemorySizeExt`
/// @class MemorySizeInfo
#[allow(clippy::doc_markdown)]
#[derive(Clone, Copy)]
pub(crate) struct MemorySizeInfo {
	pub(crate) inner: iced_x86::MemorySizeInfo,
}

impl MemorySizeInfo {
	pub(crate) unsafe fn init_and_push<'lua>(lua: &Lua<'lua>, info: &MemorySizeInfo) -> &'lua mut MemorySizeInfo {
		unsafe {
			let info = lua.push_user_data_copy(info);

			lua_get_or_init_metatable!(MemorySizeInfo: lua);
			let _ = lua.set_metatable(-2);
			info
		}
	}

	unsafe fn init_metatable(lua: &Lua<'_>) {
		unsafe {
			lua.push("__index");
			lua.new_table();

			for &(name, method) in MEMORY_SIZE_INFO_EXPORTS {
				lua.push(name);
				lua.push(method);
				lua.raw_set(-3);
			}

			// Write to __index
			lua.raw_set(-3);
		}
	}
}

lua_pub_methods! { static MEMORY_SIZE_INFO_EXPORTS =>
	/// `MemorySize` enum info, see also `MemorySizeExt`
	///
	/// @param memory_size integer #(A `MemorySize` enum variant) Memory size value
	/// @return MemorySizeInfo
	///
	/// # Examples
	///
	/// ```lua
	/// local MemorySize = require("iced_x86.MemorySize")
	/// local MemorySizeInfo = require("iced_x86.MemorySizeInfo")
	///
	/// local info = MemorySizeInfo.new(MemorySize.Packed256_UInt16)
	/// assert(info:size() == 32)
	/// ```
	unsafe fn new(lua, memory_size: u32) -> 1 {
		let _ = unsafe { MemorySizeInfo::init_and_push(lua, &MemorySizeInfo { inner: *to_memory_size(lua, memory_size).info() }) };
	}

	/// Gets the `MemorySize` value
	/// @return integer #A `MemorySize` enum variant
	///
	/// # Examples
	///
	/// ```lua
	/// local MemorySize = require("iced_x86.MemorySize")
	/// local MemorySizeInfo = require("iced_x86.MemorySizeInfo")
	///
	/// local info = MemorySizeInfo.new(MemorySize.Packed256_UInt16)
	/// assert(info:memory_size() == MemorySize.Packed256_UInt16)
	/// ```
	unsafe fn memory_size(lua, this: &MemorySizeInfo) -> 1 {
		unsafe { lua.push(this.inner.memory_size() as u32) }
	}

	/// Gets the size in bytes of the memory location or 0 if it's not accessed or unknown
	/// @return integer
	///
	/// # Examples
	///
	/// ```lua
	/// local MemorySize = require("iced_x86.MemorySize")
	/// local MemorySizeInfo = require("iced_x86.MemorySizeInfo")
	///
	/// local info = MemorySizeInfo.new(MemorySize.UInt32)
	/// assert(info:size() == 4)
	/// info = MemorySizeInfo.new(MemorySize.Packed256_UInt16)
	/// assert(info:size() == 32)
	/// info = MemorySizeInfo.new(MemorySize.Broadcast512_UInt64)
	/// assert(info:size() == 8)
	/// ```
	unsafe fn size(lua, this: &MemorySizeInfo) -> 1 {
		unsafe { lua.push(this.inner.size() as u32) }
	}

	/// Gets the size in bytes of the packed element. If it's not a packed data type, it's equal to `MemorySizeInfo.size()`.
	/// @return integer
	///
	/// # Examples
	///
	/// ```lua
	/// local MemorySize = require("iced_x86.MemorySize")
	/// local MemorySizeInfo = require("iced_x86.MemorySizeInfo")
	///
	/// local info = MemorySizeInfo.new(MemorySize.UInt32)
	/// assert(info:element_size() == 4)
	/// info = MemorySizeInfo.new(MemorySize.Packed256_UInt16)
	/// assert(info:element_size() == 2)
	/// info = MemorySizeInfo.new(MemorySize.Broadcast512_UInt64)
	/// assert(info:element_size() == 8)
	/// ```
	unsafe fn element_size(lua, this: &MemorySizeInfo) -> 1 {
		unsafe { lua.push(this.inner.element_size() as u32) }
	}

	/// Gets the element type if it's packed data or the type itself if it's not packed data
	/// @return integer #A `MemorySize` enum variant
	///
	/// # Examples
	///
	/// ```lua
	/// local MemorySize = require("iced_x86.MemorySize")
	/// local MemorySizeInfo = require("iced_x86.MemorySizeInfo")
	///
	/// local info = MemorySizeInfo.new(MemorySize.UInt32)
	/// assert(info:element_type() == MemorySize.UInt32)
	/// info = MemorySizeInfo.new(MemorySize.Packed256_UInt16)
	/// assert(info:element_type() == MemorySize.UInt16)
	/// info = MemorySizeInfo.new(MemorySize.Broadcast512_UInt64)
	/// assert(info:element_type() == MemorySize.UInt64)
	/// ```
	unsafe fn element_type(lua, this: &MemorySizeInfo) -> 1 {
		unsafe { lua.push(this.inner.element_type() as u32) }
	}

	/// Gets the element type if it's packed data or the type itself if it's not packed data
	/// @return MemorySizeInfo
	///
	/// # Examples
	///
	/// ```lua
	/// local MemorySize = require("iced_x86.MemorySize")
	/// local MemorySizeInfo = require("iced_x86.MemorySizeInfo")
	///
	/// local info = MemorySizeInfo.new(MemorySize.UInt32):element_type_info()
	/// assert(info:memory_size() == MemorySize.UInt32)
	/// info = MemorySizeInfo.new(MemorySize.Packed256_UInt16):element_type_info()
	/// assert(info:memory_size() == MemorySize.UInt16)
	/// info = MemorySizeInfo.new(MemorySize.Broadcast512_UInt64):element_type_info()
	/// assert(info:memory_size() == MemorySize.UInt64)
	/// ```
	unsafe fn element_type_info(lua, this: &MemorySizeInfo) -> 1 {
		let _ = unsafe { MemorySizeInfo::init_and_push(lua, &MemorySizeInfo { inner: *this.inner.element_type().info() }) };
	}

	/// `true` if it's signed data (signed integer or a floating point value)
	/// @return boolean
	///
	/// # Examples
	///
	/// ```lua
	/// local MemorySize = require("iced_x86.MemorySize")
	/// local MemorySizeInfo = require("iced_x86.MemorySizeInfo")
	///
	/// local info = MemorySizeInfo.new(MemorySize.UInt32)
	/// assert(not info:is_signed())
	/// info = MemorySizeInfo.new(MemorySize.Int32)
	/// assert(info:is_signed())
	/// info = MemorySizeInfo.new(MemorySize.Float64)
	/// assert(info:is_signed())
	/// ```
	unsafe fn is_signed(lua, this: &MemorySizeInfo) -> 1 {
		unsafe { lua.push(this.inner.is_signed()) }
	}

	/// `true` if it's a broadcast memory type
	/// @return boolean
	///
	/// # Examples
	///
	/// ```lua
	/// local MemorySize = require("iced_x86.MemorySize")
	/// local MemorySizeInfo = require("iced_x86.MemorySizeInfo")
	///
	/// local info = MemorySizeInfo.new(MemorySize.UInt32)
	/// assert(not info:is_broadcast())
	/// info = MemorySizeInfo.new(MemorySize.Packed256_UInt16)
	/// assert(not info:is_broadcast())
	/// info = MemorySizeInfo.new(MemorySize.Broadcast512_UInt64)
	/// assert(info:is_broadcast())
	/// ```
	unsafe fn is_broadcast(lua, this: &MemorySizeInfo) -> 1 {
		unsafe { lua.push(this.inner.is_broadcast()) }
	}

	/// `true` if this is a packed data type, eg. `MemorySize.Packed128_Float32`. See also `MemorySizeInfo.element_count()`
	/// @return boolean
	///
	/// # Examples
	///
	/// ```lua
	/// local MemorySize = require("iced_x86.MemorySize")
	/// local MemorySizeInfo = require("iced_x86.MemorySizeInfo")
	///
	/// local info = MemorySizeInfo.new(MemorySize.UInt32)
	/// assert(not info:is_packed())
	/// info = MemorySizeInfo.new(MemorySize.Packed256_UInt16)
	/// assert(info:is_packed())
	/// info = MemorySizeInfo.new(MemorySize.Broadcast512_UInt64)
	/// assert(not info:is_packed())
	/// ```
	unsafe fn is_packed(lua, this: &MemorySizeInfo) -> 1 {
		unsafe { lua.push(this.inner.is_packed()) }
	}

	/// Gets the number of elements in the packed data type or `1` if it's not packed data (`MemorySizeInfo.is_packed()`)
	/// @return integer
	///
	/// # Examples
	///
	/// ```lua
	/// local MemorySize = require("iced_x86.MemorySize")
	/// local MemorySizeInfo = require("iced_x86.MemorySizeInfo")
	///
	/// local info = MemorySizeInfo.new(MemorySize.UInt32)
	/// assert(info:element_count() == 1)
	/// info = MemorySizeInfo.new(MemorySize.Packed256_UInt16)
	/// assert(info:element_count() == 16)
	/// info = MemorySizeInfo.new(MemorySize.Broadcast512_UInt64)
	/// assert(info:element_count() == 1)
	/// ```
	unsafe fn element_count(lua, this: &MemorySizeInfo) -> 1 {
		unsafe { lua.push(this.inner.element_count() as u32) }
	}
}
