// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::ud::UserDataIds;
use loona::lua_api::*;
use loona::prelude::*;

lua_struct_module! { luaopen_iced_x86_Instruction : Instruction }
lua_impl_userdata! { Instruction }

/// A 16/32/64-bit x86 instruction. Created by `Decoder` or by `Instruction:create*()` methods
/// @class Instruction
pub(crate) struct Instruction {
	pub(crate) inner: iced_x86::Instruction,
}

impl Instruction {
	pub(crate) unsafe fn new<'lua>(lua: &Lua<'lua>) -> &'lua mut Instruction {
		unsafe {
			let instr = Instruction { inner: iced_x86::Instruction::new() };
			let instr = lua.push_user_data(instr);

			lua.get_registry_metatable(Instruction::METATABLE_KEY);
			let _ = lua.set_metatable(-2);
			instr
		}
	}

	unsafe fn init_metatable(lua: &Lua<'_>) {
		unsafe {
			lua.push_literal("__index");
			lua.new_table();

			for &(name, method) in INSTRUCTION_EXPORTS {
				lua.push_literal(name);
				lua.push_c_function(method);
				lua.raw_set(-3);
			}

			// Write to __index
			lua.raw_set(-3);
		}
	}
}

lua_pub_methods! { static INSTRUCTION_EXPORTS =>
	/// Creates a new empty instruction
	/// @return Instruction
	unsafe fn new(lua) -> 1 {
		unsafe {
			let _ = Instruction::new(&lua);
		}
	}

	/// Gets the instruction code (a `Code` enum value), see also `Instruction:mnemonic()`
	/// @return Code
	unsafe fn code(lua) -> 1 {
		unsafe {
			let instr: &Instruction = lua.get_user_data(1);
			lua.push_integer(instr.inner.code() as lua_Integer);
		}
	}

	//TODO:
}
