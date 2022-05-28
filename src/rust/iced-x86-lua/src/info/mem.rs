// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use loona::lua_api::lua_CFunction;
use loona::prelude::*;

lua_struct_module! { luaopen_iced_x86_UsedMemory : UsedMemory }
lua_impl_userdata! { UsedMemory }

/// A memory location used by an instruction
/// @class UsedMemory
#[allow(clippy::doc_markdown)]
#[derive(Clone, Copy)]
pub(crate) struct UsedMemory {
	pub(crate) inner: iced_x86::UsedMemory,
}

impl UsedMemory {
	pub(crate) unsafe fn init_and_push<'lua>(lua: &Lua<'lua>, used_mem: &UsedMemory) -> &'lua mut UsedMemory {
		unsafe {
			let used_mem = lua.push_user_data_copy(used_mem);

			lua_get_or_init_metatable!(UsedMemory: lua);
			let _ = lua.set_metatable(-2);
			used_mem
		}
	}

	unsafe fn init_metatable(lua: &Lua<'_>) {
		unsafe {
			lua.push("__index");
			lua.new_table();

			for &(name, method) in USED_MEMORY_EXPORTS {
				lua.push(name);
				lua.push(method);
				lua.raw_set(-3);
			}

			// Write to __index
			lua.raw_set(-3);

			#[rustfmt::skip]
			let special_methods: &[(&str, lua_CFunction)] = &[
				("__eq", used_memory_eq),
			];
			for &(name, method) in special_methods {
				lua.push(name);
				lua.push(method);
				lua.raw_set(-3);
			}
		}
	}
}

lua_pub_methods! { static USED_MEMORY_EXPORTS =>
	/// Effective segment register or `Register.None` if the segment register is ignored
	/// @return integer # A `Register` enum value
	unsafe fn segment(lua, this: &UsedMemory) -> 1 {
		unsafe { lua.push(this.inner.segment() as u32) }
	}

	/// Base register or `Register.None` if none
	/// @return integer # A `Register` enum value
	unsafe fn base(lua, this: &UsedMemory) -> 1 {
		unsafe { lua.push(this.inner.base() as u32) }
	}

	/// Index register or `Register.None` if none
	/// @return integer # A `Register` enum value
	unsafe fn index(lua, this: &UsedMemory) -> 1 {
		unsafe { lua.push(this.inner.index() as u32) }
	}

	/// Index scale (1, 2, 4 or 8)
	/// @return integer
	unsafe fn scale(lua, this: &UsedMemory) -> 1 {
		unsafe { lua.push(this.inner.scale()) }
	}

	/// Displacement
	/// @return integer
	unsafe fn displacement(lua, this: &UsedMemory) -> 1 {
		unsafe { lua.push(this.inner.displacement()) }
	}

	/// Size of location (a `MemorySize` enum value)
	/// @return integer # A `MemorySize` enum value
	unsafe fn memory_size(lua, this: &UsedMemory) -> 1 {
		unsafe { lua.push(this.inner.memory_size() as u32) }
	}

	/// Memory access (an `OpAccess` enum value)
	/// @return integer # An `OpAccess` enum value
	unsafe fn access(lua, this: &UsedMemory) -> 1 {
		unsafe { lua.push(this.inner.access() as u32) }
	}

	/// Address size (a `CodeSize` enum value)
	/// @return integer # A `CodeSize` enum value
	unsafe fn address_size(lua, this: &UsedMemory) -> 1 {
		unsafe { lua.push(this.inner.address_size() as u32) }
	}

	/// VSIB size (`0`, `4` or `8`)
	/// @return integer
	unsafe fn vsib_size(lua, this: &UsedMemory) -> 1 {
		unsafe { lua.push(this.inner.vsib_size()) }
	}

	/// Returns a copy of this instance.
	/// @return UsedMemory # A copy of this instance
	unsafe fn copy(lua, this: &UsedMemory) -> 1 {
		unsafe { let _ = UsedMemory::init_and_push(lua, this); }
	}
}

lua_methods! {
	unsafe fn used_memory_eq(lua, used_mem: &UsedMemory, used_mem2: &UsedMemory) -> 1 {
		unsafe { lua.push(used_mem.inner == used_mem2.inner) }
	}
}
