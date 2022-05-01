// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use mlua::prelude::*;

pub(crate) struct Instruction {
	pub(crate) instr: iced_x86::Instruction,
}

impl Instruction {
	pub(crate) fn new() -> Self {
		Self { instr: iced_x86::Instruction::default() }
	}
}

impl LuaUserData for Instruction {}

#[mlua::lua_module]
fn iced_x86_priv_instr(lua: &Lua) -> LuaResult<LuaTable<'_>> {
	let exports = lua.create_table()?;
	lua_ctor!(lua, exports, Instruction = fn instruction_new() {
		Ok(Instruction::new())
	});
	Ok(exports)
}
