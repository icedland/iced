// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use loona::lua_api::lua_CFunction;
use loona::prelude::*;

lua_struct_module! { luaopen_iced_x86_UsedRegister : UsedRegister }
lua_impl_userdata! { UsedRegister }

/// A register used by an instruction
/// @class UsedRegister
#[allow(clippy::doc_markdown)]
#[derive(Clone, Copy)]
pub(crate) struct UsedRegister {
	pub(crate) inner: iced_x86::UsedRegister,
}

impl UsedRegister {
	pub(crate) unsafe fn init_and_push<'lua>(lua: &Lua<'lua>, used_reg: &UsedRegister) -> &'lua mut UsedRegister {
		unsafe {
			let used_reg = lua.push_user_data_copy(used_reg);

			lua_get_or_init_metatable!(UsedRegister: lua);
			let _ = lua.set_metatable(-2);
			used_reg
		}
	}

	unsafe fn init_metatable(lua: &Lua<'_>) {
		unsafe {
			lua.push("__index");
			lua.new_table();

			for &(name, method) in USED_REGISTER_EXPORTS {
				lua.push(name);
				lua.push(method);
				lua.raw_set(-3);
			}

			// Write to __index
			lua.raw_set(-3);

			#[rustfmt::skip]
			let special_methods: &[(&str, lua_CFunction)] = &[
				("__eq", used_register_eq),
			];
			for &(name, method) in special_methods {
				lua.push(name);
				lua.push(method);
				lua.raw_set(-3);
			}
		}
	}
}

lua_pub_methods! { static USED_REGISTER_EXPORTS =>
	/// Gets the register (a `Register` enum value)
	/// @return integer # A `Register` enum value
	unsafe fn register(lua, this: &UsedRegister) -> 1 {
		unsafe { lua.push(this.inner.register() as u32) }
	}

	/// Gets the register access (an `OpAccess` enum value)
	/// @return integer # An `OpAccess` enum value
	unsafe fn access(lua, this: &UsedRegister) -> 1 {
		unsafe { lua.push(this.inner.access() as u32) }
	}

	/// Returns a copy of this instance.
	/// @return UsedRegister # A copy of this instance
	unsafe fn copy(lua, this: &UsedRegister) -> 1 {
		unsafe { let _ = UsedRegister::init_and_push(lua, this); }
	}
}

lua_methods! {
	unsafe fn used_register_eq(lua, used_reg: &UsedRegister, used_reg2: &UsedRegister) -> 1 {
		unsafe { lua.push(used_reg.inner == used_reg2.inner) }
	}
}
