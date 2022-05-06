// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

macro_rules! lua_impl_userdata {
	($class:ident) => {
		unsafe impl ::loona::lua::LuaUserData for $class {
			const UNIQUE_ID: u32 = UserDataIds::$class as u32;
			const METATABLE_KEY: ::loona::lua::LuaCStr<'static> = ::loona::cstr!(concat!(stringify!($class), "iced"));
		}
	};
}

macro_rules! lua_struct_module {
	($mod_name:ident : $class:ident) => {
		::loona::lua_module! {unsafe fn $mod_name(lua) {
			unsafe {
				let mt_res = lua.new_registry_metatable($class::METATABLE_KEY);
				debug_assert!(mt_res);
				$class::init_metatable(&lua);
				lua.push_literal("__index");
				lua.raw_get(-2);
				lua.replace(-2); // replace metatable with metatable.__index
			}
		}}
	};
}
