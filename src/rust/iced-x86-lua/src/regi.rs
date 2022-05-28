// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::enum_utils::to_register;
use loona::prelude::*;

lua_struct_module! { luaopen_iced_x86_RegisterInfo : RegisterInfo }
lua_impl_userdata! { RegisterInfo }

/// `Register` enum info, see also `RegisterExt`
/// @class RegisterInfo
#[allow(clippy::doc_markdown)]
#[derive(Clone, Copy)]
pub(crate) struct RegisterInfo {
	pub(crate) inner: iced_x86::RegisterInfo,
}

impl RegisterInfo {
	pub(crate) unsafe fn init_and_push<'lua>(lua: &Lua<'lua>, info: &RegisterInfo) -> &'lua mut RegisterInfo {
		unsafe {
			let info = lua.push_user_data_copy(info);

			lua_get_or_init_metatable!(RegisterInfo: lua);
			let _ = lua.set_metatable(-2);
			info
		}
	}

	unsafe fn init_metatable(lua: &Lua<'_>) {
		unsafe {
			lua.push("__index");
			lua.new_table();

			for &(name, method) in REGISTER_INFO_EXPORTS {
				lua.push(name);
				lua.push(method);
				lua.raw_set(-3);
			}

			// Write to __index
			lua.raw_set(-3);
		}
	}
}

lua_pub_methods! { static REGISTER_INFO_EXPORTS =>
	/// `Register` enum info, see also `RegisterExt`
	///
	/// @param register integer #(A `Register` enum variant) Enum value
	/// @return RegisterInfo
	///
	/// # Examples
	///
	/// ```lua
	/// local Register = require("iced_x86.Register")
	/// local RegisterInfo = require("iced_x86.RegisterInfo")
	///
	/// local info = RegisterInfo.new(Register.GS)
	/// assert(info:number() == 5)
	/// ```
	unsafe fn new(lua, register: u32) -> 1 {
		let _ = unsafe { RegisterInfo::init_and_push(lua, &RegisterInfo { inner: *to_register(lua, register).info() }) };
	}

	/// Gets the register value passed into the constructor
	/// @return integer #A `Register` enum variant
	///
	/// # Examples
	///
	/// ```lua
	/// local Register = require("iced_x86.Register")
	/// local RegisterInfo = require("iced_x86.RegisterInfo")
	///
	/// local info = RegisterInfo.new(Register.EAX)
	/// assert(info:register() == Register.EAX)
	/// ```
	unsafe fn register(lua, this: &RegisterInfo) -> 1 {
		unsafe { lua.push(this.inner.register() as u32) }
	}

	/// Gets the base register, eg. `AL`, `AX`, `EAX`, `RAX`, `MM0`, `XMM0`, `YMM0`, `ZMM0`, `ES`
	/// @return integer #A `Register` enum variant
	///
	/// # Examples
	///
	/// ```lua
	/// local Register = require("iced_x86.Register")
	/// local RegisterInfo = require("iced_x86.RegisterInfo")
	///
	/// local info = RegisterInfo.new(Register.GS)
	/// assert(info:base() == Register.ES)
	/// info = RegisterInfo.new(Register.RDX)
	/// assert(info:base() == Register.RAX)
	/// info = RegisterInfo.new(Register.XMM13)
	/// assert(info:base() == Register.XMM0)
	/// info = RegisterInfo.new(Register.YMM13)
	/// assert(info:base() == Register.YMM0)
	/// info = RegisterInfo.new(Register.ZMM13)
	/// assert(info:base() == Register.ZMM0)
	/// ```
	unsafe fn base(lua, this: &RegisterInfo) -> 1 {
		unsafe { lua.push(this.inner.base() as u32) }
	}

	/// int: The register number (index) relative to `RegisterInfo.base()`, eg. 0-15, or 0-31, or if 8-bit GPR, 0-19
	///
	/// # Examples
	///
	/// ```lua
	/// local Register = require("iced_x86.Register")
	/// local RegisterInfo = require("iced_x86.RegisterInfo")
	///
	/// local info = RegisterInfo.new(Register.GS)
	/// assert(info:number() == 5)
	/// info = RegisterInfo.new(Register.RDX)
	/// assert(info:number() == 2)
	/// info = RegisterInfo.new(Register.XMM13)
	/// assert(info:number() == 13)
	/// info = RegisterInfo.new(Register.YMM13)
	/// assert(info:number() == 13)
	/// info = RegisterInfo.new(Register.ZMM13)
	/// assert(info:number() == 13)
	/// ```
	unsafe fn number(lua, this: &RegisterInfo) -> 1 {
		unsafe { lua.push(this.inner.number() as u32) }
	}

	/// The full register that this one is a part of, eg. `CL`/`CH`/`CX`/`ECX`/`RCX` -> `RCX`, `XMM11`/`YMM11`/`ZMM11` -> `ZMM11`
	/// @return integer #A `Register` enum variant
	///
	/// # Examples
	///
	/// ```lua
	/// local Register = require("iced_x86.Register")
	/// local RegisterInfo = require("iced_x86.RegisterInfo")
	///
	/// local info = RegisterInfo.new(Register.GS)
	/// assert(info:full_register() == Register.GS)
	/// info = RegisterInfo.new(Register.BH)
	/// assert(info:full_register() == Register.RBX)
	/// info = RegisterInfo.new(Register.DX)
	/// assert(info:full_register() == Register.RDX)
	/// info = RegisterInfo.new(Register.ESP)
	/// assert(info:full_register() == Register.RSP)
	/// info = RegisterInfo.new(Register.RCX)
	/// assert(info:full_register() == Register.RCX)
	/// info = RegisterInfo.new(Register.XMM3)
	/// assert(info:full_register() == Register.ZMM3)
	/// info = RegisterInfo.new(Register.YMM3)
	/// assert(info:full_register() == Register.ZMM3)
	/// info = RegisterInfo.new(Register.ZMM3)
	/// assert(info:full_register() == Register.ZMM3)
	/// ```
	unsafe fn full_register(lua, this: &RegisterInfo) -> 1 {
		unsafe { lua.push(this.inner.full_register() as u32) }
	}

	/// Gets the full register that this one is a part of, except if it's a GPR in which case the 32-bit register is returned,
	/// eg. `CL`/`CH`/`CX`/`ECX`/`RCX` -> `ECX`, `XMM11`/`YMM11`/`ZMM11` -> `ZMM11`
	/// @return integer #A `Register` enum variant
	///
	/// # Examples
	///
	/// ```lua
	/// local Register = require("iced_x86.Register")
	/// local RegisterInfo = require("iced_x86.RegisterInfo")
	///
	/// local info = RegisterInfo.new(Register.GS)
	/// assert(info:full_register32() == Register.GS)
	/// info = RegisterInfo.new(Register.BH)
	/// assert(info:full_register32() == Register.EBX)
	/// info = RegisterInfo.new(Register.DX)
	/// assert(info:full_register32() == Register.EDX)
	/// info = RegisterInfo.new(Register.ESP)
	/// assert(info:full_register32() == Register.ESP)
	/// info = RegisterInfo.new(Register.RCX)
	/// assert(info:full_register32() == Register.ECX)
	/// info = RegisterInfo.new(Register.XMM3)
	/// assert(info:full_register32() == Register.ZMM3)
	/// info = RegisterInfo.new(Register.YMM3)
	/// assert(info:full_register32() == Register.ZMM3)
	/// info = RegisterInfo.new(Register.ZMM3)
	/// assert(info:full_register32() == Register.ZMM3)
	/// ```
	unsafe fn full_register32(lua, this: &RegisterInfo) -> 1 {
		unsafe { lua.push(this.inner.full_register32() as u32) }
	}

	/// int: Size of the register in bytes
	///
	/// # Examples
	///
	/// ```lua
	/// local Register = require("iced_x86.Register")
	/// local RegisterInfo = require("iced_x86.RegisterInfo")
	///
	/// local info = RegisterInfo.new(Register.GS)
	/// assert(info:size() == 2)
	/// info = RegisterInfo.new(Register.BH)
	/// assert(info:size() == 1)
	/// info = RegisterInfo.new(Register.DX)
	/// assert(info:size() == 2)
	/// info = RegisterInfo.new(Register.ESP)
	/// assert(info:size() == 4)
	/// info = RegisterInfo.new(Register.RCX)
	/// assert(info:size() == 8)
	/// info = RegisterInfo.new(Register.XMM3)
	/// assert(info:size() == 16)
	/// info = RegisterInfo.new(Register.YMM3)
	/// assert(info:size() == 32)
	/// info = RegisterInfo.new(Register.ZMM3)
	/// assert(info:size() == 64)
	/// ```
	unsafe fn size(lua, this: &RegisterInfo) -> 1 {
		unsafe { lua.push(this.inner.size() as u32) }
	}
}
