// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

macro_rules! lua_impl_userdata {
	($class:ident) => {
		unsafe impl ::loona::lua::LuaUserData for $class {
			const UNIQUE_ID: u32 = $crate::ud::UserDataIds::$class as u32;
			const METATABLE_KEY: ::loona::lua::LuaCStr<'static> = ::loona::cstr!(concat!(stringify!($class), "iced"));
		}
	};
}
