// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use loona::lua_api::lua_CFunction;
use loona::prelude::*;

lua_struct_module! { luaopen_iced_x86_FpuStackIncrementInfo : FpuStackIncrementInfo }
lua_impl_userdata! { FpuStackIncrementInfo }

/// Contains the FPU `TOP` increment, whether it's conditional and whether the instruction writes to `TOP`
/// @class FpuStackIncrementInfo
#[allow(clippy::doc_markdown)]
#[derive(Clone, Copy)]
pub(crate) struct FpuStackIncrementInfo {
	inner: iced_x86::FpuStackIncrementInfo,
}

impl FpuStackIncrementInfo {
	unsafe fn push_new<'lua>(lua: &Lua<'lua>, increment: i32, conditional: bool, writes_top: bool) -> &'lua mut FpuStackIncrementInfo {
		unsafe {
			let fpui = FpuStackIncrementInfo { inner: iced_x86::FpuStackIncrementInfo::new(increment, conditional, writes_top) };
			Self::init_and_push(lua, &fpui)
		}
	}

	pub(crate) unsafe fn init_and_push_iced<'lua>(lua: &Lua<'lua>, fpui: &iced_x86::FpuStackIncrementInfo) -> &'lua mut FpuStackIncrementInfo {
		let fpui = FpuStackIncrementInfo { inner: *fpui };
		unsafe { Self::init_and_push(lua, &fpui) }
	}

	unsafe fn init_and_push<'lua>(lua: &Lua<'lua>, fpui: &FpuStackIncrementInfo) -> &'lua mut FpuStackIncrementInfo {
		unsafe {
			let fpui = lua.push_user_data_copy(fpui);

			lua_get_or_init_metatable!(FpuStackIncrementInfo: lua);
			let _ = lua.set_metatable(-2);
			fpui
		}
	}

	unsafe fn init_metatable(lua: &Lua<'_>) {
		unsafe {
			lua.push("__index");
			lua.new_table();

			for &(name, method) in FPU_INFO_EXPORTS {
				lua.push(name);
				lua.push(method);
				lua.raw_set(-3);
			}

			// Write to __index
			lua.raw_set(-3);

			#[rustfmt::skip]
			let special_methods: &[(&str, lua_CFunction)] = &[
				("__eq", fpuinfo_eq),
			];
			for &(name, method) in special_methods {
				lua.push(name);
				lua.push(method);
				lua.raw_set(-3);
			}
		}
	}
}

lua_pub_methods! { static FPU_INFO_EXPORTS =>
	/// Creates a new instance
	/// @param increment integer # (`i32`) Used if `writes_top` is `true`. Value added to `TOP`.
	/// @param conditional boolean # `true` if it's a conditional push/pop (eg. `FPTAN` or `FSINCOS`)
	/// @param writes_top boolean # `true` if `TOP` is written (it's a conditional/unconditional push/pop, `FNSAVE`, `FLDENV`, etc)
	/// @return FpuStackIncrementInfo
	unsafe fn new(lua, increment: i32, conditional: bool, writes_top: bool) -> 1 {
		unsafe { let _ = FpuStackIncrementInfo::push_new(lua, increment, conditional, writes_top); }
	}

	/// Used if `FpuStackIncrementInfo:writes_top()` is `true`. Value added to `TOP`.
	///
	/// This is negative if it pushes one or more values and positive if it pops one or more values
	/// and `0` if it writes to `TOP` (eg. `FLDENV`, etc) without pushing/popping anything.
	///
	/// @return integer
	unsafe fn increment(lua, this: &FpuStackIncrementInfo) -> 1 {
		unsafe { lua.push(this.inner.increment()); }
	}

	/// `true` if it's a conditional push/pop (eg. `FPTAN` or `FSINCOS`)
	/// @return boolean
	unsafe fn conditional(lua, this: &FpuStackIncrementInfo) -> 1 {
		unsafe { lua.push(this.inner.conditional()); }
	}

	/// `true` if `TOP` is written (it's a conditional/unconditional push/pop, `FNSAVE`, `FLDENV`, etc)
	/// @return boolean
	unsafe fn writes_top(lua, this: &FpuStackIncrementInfo) -> 1 {
		unsafe { lua.push(this.inner.writes_top()); }
	}
}

lua_methods! {
	unsafe fn fpuinfo_eq(lua, fpui: &FpuStackIncrementInfo, fpui2: &FpuStackIncrementInfo) -> 1 {
		unsafe { lua.push(fpui.inner == fpui2.inner) }
	}
}
