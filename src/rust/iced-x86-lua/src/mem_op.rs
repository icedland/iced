// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::enum_utils::to_register;
use loona::lua_api::lua_CFunction;
use loona::prelude::*;

lua_struct_module! { luaopen_iced_x86_MemoryOperand : MemoryOperand }
lua_impl_userdata! { MemoryOperand }

/// Memory operand passed to one of `Instruction`'s `create*()` constructor methods
/// @class MemoryOperand
#[allow(clippy::doc_markdown)]
#[derive(Clone, Copy)]
pub(crate) struct MemoryOperand {
	pub(crate) inner: iced_x86::MemoryOperand,
}

impl MemoryOperand {
	pub(crate) unsafe fn init_and_push<'lua>(lua: &Lua<'lua>, mem_op: &MemoryOperand) -> &'lua mut MemoryOperand {
		unsafe {
			let mem_op = lua.push_user_data_copy(mem_op);

			lua_get_or_init_metatable!(MemoryOperand: lua);
			let _ = lua.set_metatable(-2);
			mem_op
		}
	}

	unsafe fn init_metatable(lua: &Lua<'_>) {
		unsafe {
			lua.push("__index");
			lua.new_table();

			for &(name, method) in MEMORY_OPERAND_EXPORTS {
				lua.push(name);
				lua.push(method);
				lua.raw_set(-3);
			}

			// Write to __index
			lua.raw_set(-3);

			#[rustfmt::skip]
			let special_methods: &[(&str, lua_CFunction)] = &[
				("__eq", memory_operand_eq),
			];
			for &(name, method) in special_methods {
				lua.push(name);
				lua.push(method);
				lua.raw_set(-3);
			}
		}
	}
}

lua_pub_methods! { static MEMORY_OPERAND_EXPORTS =>
	/// Memory operand passed to one of `Instruction`'s `create*()` constructor methods
	///
	/// @param base? integer #(A `Register` enum variant) (default = `None`) Base register or `Register.None`
	/// @param index? integer #(A `Register` enum variant) (default = `None`) Index register or `Register.None`
	/// @param scale? integer #(default = `1`) Index register scale (1, 2, 4, or 8)
	/// @param displ? integer #(default = `0`) Memory displacement
	/// @param displ_size? integer #(default = `0`) 0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a `i8`), 2 (16-bit), 4 (32-bit) or 8 (64-bit)
	/// @param is_broadcast? boolean #(default = `false`) `true` if it's broadcast memory (EVEX instructions)
	/// @param segment? integer #(A `Register` enum variant) (default = `None`) Segment override or `Register.None`
	/// @return MemoryOperand
	unsafe fn new(lua, base: LuaDefaultU32<{iced_x86::Register::None as u32}>, index: LuaDefaultU32<{iced_x86::Register::None as u32}>, scale: LuaDefaultU32<1>, displ: LuaDefaultI64<0>, displ_size: LuaDefaultU32<0>, is_broadcast: LuaDefaultBool<false>, segment: LuaDefaultU32<{iced_x86::Register::None as u32}>) -> 1 {
		let base = unsafe { to_register(lua, base) };
		let index = unsafe { to_register(lua, index) };
		let segment = unsafe { to_register(lua, segment) };

		let displ_size = if displ != 0 && displ_size == 0 {
			1
		} else {
			displ_size
		};

		let mem_op = MemoryOperand { inner: iced_x86::MemoryOperand::new(base, index, scale, displ, displ_size, is_broadcast, segment) };
		let _ = unsafe { MemoryOperand::init_and_push(lua, &mem_op) };
	}

	/// Memory operand passed to one of `Instruction`'s `create*()` constructor methods
	///
	/// @param base integer #(A `Register` enum variant) Base register or `Register.None`
	/// @param index integer #(A `Register` enum variant) Index register or `Register.None`
	/// @param scale integer #Index register scale (1, 2, 4, or 8)
	/// @param displ integer #Memory displacement
	/// @param displ_size integer #0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a `i8`), 2 (16-bit), 4 (32-bit) or 8 (64-bit)
	/// @return MemoryOperand
	unsafe fn with_base_index_scale_displ_size(lua, base: u32, index: u32, scale: u32, displ: i64, displ_size: u32) -> 1 {
		let base = unsafe { to_register(lua, base) };
		let index = unsafe { to_register(lua, index) };
		let mem_op = MemoryOperand { inner: iced_x86::MemoryOperand::with_base_index_scale_displ_size(base, index, scale, displ, displ_size) };
		let _ = unsafe { MemoryOperand::init_and_push(lua, &mem_op) };
	}

	/// Memory operand passed to one of `Instruction`'s `create*()` constructor methods
	///
	/// @param base integer #(A `Register` enum variant) Base register or `Register.None`
	/// @param index integer #(A `Register` enum variant) Index register or `Register.None`
	/// @param scale integer #Index register scale (1, 2, 4, or 8)
	/// @return MemoryOperand
	unsafe fn with_base_index_scale(lua, base: u32, index: u32, scale: u32) -> 1 {
		let base = unsafe { to_register(lua, base) };
		let index = unsafe { to_register(lua, index) };
		let mem_op = MemoryOperand { inner: iced_x86::MemoryOperand::with_base_index_scale(base, index, scale) };
		let _ = unsafe { MemoryOperand::init_and_push(lua, &mem_op) };
	}

	/// Memory operand passed to one of `Instruction`'s `create*()` constructor methods
	///
	/// @param base integer #(A `Register` enum variant) Base register or `Register.None`
	/// @param index integer #(A `Register` enum variant) Index register or `Register.None`
	/// @return MemoryOperand
	unsafe fn with_base_index(lua, base: u32, index: u32) -> 1 {
		let base = unsafe { to_register(lua, base) };
		let index = unsafe { to_register(lua, index) };
		let mem_op = MemoryOperand { inner: iced_x86::MemoryOperand::with_base_index(base, index) };
		let _ = unsafe { MemoryOperand::init_and_push(lua, &mem_op) };
	}

	/// Memory operand passed to one of `Instruction`'s `create*()` constructor methods
	///
	/// @param base integer #(A `Register` enum variant) Base register or `Register.None`
	/// @param displ integer #Memory displacement
	/// @param displ_size integer #0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a `i8`), 2 (16-bit), 4 (32-bit) or 8 (64-bit)
	/// @return MemoryOperand
	unsafe fn with_base_displ_size(lua, base: u32, displ: i64, displ_size: u32) -> 1 {
		let base = unsafe { to_register(lua, base) };
		let mem_op = MemoryOperand { inner: iced_x86::MemoryOperand::with_base_displ_size(base, displ, displ_size) };
		let _ = unsafe { MemoryOperand::init_and_push(lua, &mem_op) };
	}

	/// Memory operand passed to one of `Instruction`'s `create*()` constructor methods
	///
	/// @param index integer #(A `Register` enum variant) Index register or `Register.None`
	/// @param scale integer #Index register scale (1, 2, 4, or 8)
	/// @param displ integer #Memory displacement
	/// @param displ_size integer #0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a `i8`), 2 (16-bit), 4 (32-bit) or 8 (64-bit)
	/// @return MemoryOperand
	unsafe fn with_index_scale_displ_size(lua, index: u32, scale: u32, displ: i64, displ_size: u32) -> 1 {
		let index = unsafe { to_register(lua, index) };
		let mem_op = MemoryOperand { inner: iced_x86::MemoryOperand::with_index_scale_displ_size(index, scale, displ, displ_size) };
		let _ = unsafe { MemoryOperand::init_and_push(lua, &mem_op) };
	}

	/// Memory operand passed to one of `Instruction`'s `create*()` constructor methods
	///
	/// @param base integer #(A `Register` enum variant) Base register or `Register.None`
	/// @param displ integer #Memory displacement
	/// @return MemoryOperand
	unsafe fn with_base_displ(lua, base: u32, displ: i64) -> 1 {
		let base = unsafe { to_register(lua, base) };
		let mem_op = MemoryOperand { inner: iced_x86::MemoryOperand::with_base_displ(base, displ) };
		let _ = unsafe { MemoryOperand::init_and_push(lua, &mem_op) };
	}

	/// Memory operand passed to one of `Instruction`'s `create*()` constructor methods
	///
	/// @param base integer #(A `Register` enum variant) Base register or `Register.None`
	/// @return MemoryOperand
	unsafe fn with_base(lua, base: u32) -> 1 {
		let base = unsafe { to_register(lua, base) };
		let mem_op = MemoryOperand { inner: iced_x86::MemoryOperand::with_base(base) };
		let _ = unsafe { MemoryOperand::init_and_push(lua, &mem_op) };
	}

	/// Memory operand passed to one of `Instruction`'s `create*()` constructor methods
	///
	/// @param displ integer #Memory displacement
	/// @param displ_size? integer #(default = `1`) 0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a `i8`), 2 (16-bit), 4 (32-bit) or 8 (64-bit)
	/// @return MemoryOperand
	unsafe fn with_displ(lua, displ: u64, displ_size: LuaDefaultU32<1>) -> 1 {
		let mem_op = MemoryOperand { inner: iced_x86::MemoryOperand::with_displ(displ, displ_size) };
		let _ = unsafe { MemoryOperand::init_and_push(lua, &mem_op) };
	}

	/// Returns a copy of this instance.
	/// @return MemoryOperand
	unsafe fn copy(lua, this: &MemoryOperand) -> 1 {
		let _ = unsafe { MemoryOperand::init_and_push(lua, this) };
	}
}

lua_methods! {
	unsafe fn memory_operand_eq(lua, mem_op: &MemoryOperand, mem_op2: &MemoryOperand) -> 1 {
		unsafe { lua.push(mem_op.inner == mem_op2.inner) }
	}
}
