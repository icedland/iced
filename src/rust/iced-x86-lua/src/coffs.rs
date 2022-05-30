// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use loona::lua_api::lua_CFunction;
use loona::prelude::*;

lua_struct_module! { luaopen_iced_x86_ConstantOffsets : ConstantOffsets }
lua_impl_userdata! { ConstantOffsets }

/// Contains the offsets of the displacement and immediate.
///
/// Call `Decoder:get_constant_offsets()` or `Encoder:get_constant_offsets()` to get the
/// offsets of the constants after the instruction has been decoded/encoded.
///
/// @class ConstantOffsets
#[allow(clippy::doc_markdown)]
#[derive(Clone, Copy)]
pub(crate) struct ConstantOffsets {
	pub(crate) inner: iced_x86::ConstantOffsets,
}

impl ConstantOffsets {
	pub(crate) unsafe fn init_and_push<'lua>(lua: &Lua<'lua>, co: &ConstantOffsets) -> &'lua mut ConstantOffsets {
		unsafe {
			let co = lua.push_user_data_copy(co);

			lua_get_or_init_metatable!(ConstantOffsets: lua);
			let _ = lua.set_metatable(-2);
			co
		}
	}

	unsafe fn init_metatable(lua: &Lua<'_>) {
		unsafe {
			lua.push("__index");
			lua.new_table();

			for &(name, method) in CONSTANT_OFFSETS_EXPORTS {
				lua.push(name);
				lua.push(method);
				lua.raw_set(-3);
			}

			// Write to __index
			lua.raw_set(-3);

			#[rustfmt::skip]
			let special_methods: &[(&str, lua_CFunction)] = &[
				("__eq", constant_offsets_eq),
			];
			for &(name, method) in special_methods {
				lua.push(name);
				lua.push(method);
				lua.raw_set(-3);
			}
		}
	}
}

lua_pub_methods! { static CONSTANT_OFFSETS_EXPORTS =>
	/// The offset of the displacement, if any
	/// @return integer
	unsafe fn displacement_offset(lua, this: &ConstantOffsets) -> 1 {
		unsafe { lua.push(this.inner.displacement_offset() as u32); }
	}

	/// Size in bytes of the displacement, or 0 if there's no displacement
	/// @return integer
	unsafe fn displacement_size(lua, this: &ConstantOffsets) -> 1 {
		unsafe { lua.push(this.inner.displacement_size() as u32); }
	}

	/// The offset of the first immediate, if any.
	///
	/// This field can be invalid even if the operand has an immediate if it's an immediate that isn't part
	/// of the instruction stream, eg. `SHL AL,1`.
	///
	/// @return integer
	unsafe fn immediate_offset(lua, this: &ConstantOffsets) -> 1 {
		unsafe { lua.push(this.inner.immediate_offset() as u32); }
	}

	/// Size in bytes of the first immediate, or 0 if there's no immediate
	/// @return integer
	unsafe fn immediate_size(lua, this: &ConstantOffsets) -> 1 {
		unsafe { lua.push(this.inner.immediate_size() as u32); }
	}

	/// The offset of the second immediate, if any.
	/// @return integer
	unsafe fn immediate_offset2(lua, this: &ConstantOffsets) -> 1 {
		unsafe { lua.push(this.inner.immediate_offset2() as u32); }
	}

	/// Size in bytes of the second immediate, or 0 if there's no second immediate
	/// @return integer
	unsafe fn immediate_size2(lua, this: &ConstantOffsets) -> 1 {
		unsafe { lua.push(this.inner.immediate_size2() as u32); }
	}

	/// `true` if `ConstantOffsets:displacement_offset()` and `ConstantOffsets:displacement_size()` are valid
	/// @return boolean
	unsafe fn has_displacement(lua, this: &ConstantOffsets) -> 1 {
		unsafe { lua.push(this.inner.has_displacement()); }
	}

	/// `true` if `ConstantOffsets:immediate_offset()` and `ConstantOffsets:immediate_size()` are valid
	/// @return boolean
	unsafe fn has_immediate(lua, this: &ConstantOffsets) -> 1 {
		unsafe { lua.push(this.inner.has_immediate()); }
	}

	/// `true` if `ConstantOffsets:immediate_offset2()` and `ConstantOffsets:immediate_size2()` are valid
	/// @return boolean
	unsafe fn has_immediate2(lua, this: &ConstantOffsets) -> 1 {
		unsafe { lua.push(this.inner.has_immediate2()); }
	}

	/// Returns a copy of this instance.
	/// @return ConstantOffsets # A copy of this instance
	unsafe fn copy(lua, this: &ConstantOffsets) -> 1 {
		let _ = unsafe { ConstantOffsets::init_and_push(lua, this) };
	}
}

lua_methods! {
	unsafe fn constant_offsets_eq(lua, instr: &ConstantOffsets, instr2: &ConstantOffsets) -> 1 {
		unsafe { lua.push(instr.inner == instr2.inner) }
	}
}
