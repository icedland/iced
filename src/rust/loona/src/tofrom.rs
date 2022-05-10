// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::lua::Lua;
use crate::prelude::LuaUserData;
use libc::c_int;

pub trait FromLua<'lua> {
	type RetType;

	/// # Safety
	///
	/// `idx` must be a valid index that can be passed to Lua API functions.
	unsafe fn from_lua(lua: &Lua<'lua>, idx: c_int) -> Self::RetType;
}

/// A value that is ignored
#[derive(Debug, Clone, Copy)]
pub struct LuaIgnore;

impl<'lua> FromLua<'lua> for LuaIgnore {
	type RetType = Self;

	#[inline]
	unsafe fn from_lua(_lua: &Lua<'lua>, _idx: c_int) -> Self::RetType {
		Self
	}
}

macro_rules! create_lua_option {
    ($lua:ident, $idx:ident { $($struct_name:ident : $ty:ty => $expr:expr,)+ }) => {
		$(
			#[derive(Debug, Clone, Copy)]
			pub struct $struct_name<const DEFAULT: $ty>;

			impl<'lua, const DEFAULT: $ty> $crate::tofrom::FromLua<'lua> for $struct_name<DEFAULT> {
				type RetType = $ty;

				#[inline]
				unsafe fn from_lua($lua: &Lua<'lua>, $idx: c_int) -> Self::RetType {
					// SAFETY: caller guarantees `$idx` is valid to pass to this function
					if unsafe { $lua.is_none_or_nil($idx) } {
						DEFAULT
					} else {
						$expr
					}
				}
			}
		)+
    };
}

// SAFETY: caller guarantees `idx` is valid to pass to these functions
create_lua_option! { lua, idx {
	LuaDefaultBool : bool => unsafe { lua.get_bool(idx) },
	LuaDefaultChar : char => unsafe { lua.get_char(idx) },
	LuaDefaultI8 : i8 => unsafe { lua.get_i8(idx) },
	LuaDefaultI16 : i16 => unsafe { lua.get_i16(idx) },
	LuaDefaultI32 : i32 => unsafe { lua.get_i32(idx) },
	LuaDefaultI64 : i64 => unsafe { lua.get_i64(idx) },
	LuaDefaultIsize : isize => unsafe { lua.get_isize(idx) },
	LuaDefaultU8 : u8 => unsafe { lua.get_u8(idx) },
	LuaDefaultU16 : u16 => unsafe { lua.get_u16(idx) },
	LuaDefaultU32 : u32 => unsafe { lua.get_u32(idx) },
	LuaDefaultU64 : u64 => unsafe { lua.get_u64(idx) },
	LuaDefaultUsize : usize => unsafe { lua.get_usize(idx) },
}}

macro_rules! impl_from_lua {
	($lua_lt:lifetime, $lua:ident, $idx:ident { $($ty:ty => $cvt:expr,)+ }) => {
		$(
			impl<$lua_lt> $crate::tofrom::FromLua<$lua_lt> for $ty {
				type RetType = $ty;

				#[inline]
				unsafe fn from_lua($lua: &Lua<$lua_lt>, $idx: c_int) -> Self {
					$cvt
				}
			}

			impl<$lua_lt> $crate::tofrom::FromLua<$lua_lt> for Option<$ty> {
				type RetType = Option<$ty>;

				#[inline]
				unsafe fn from_lua($lua: &Lua<$lua_lt>, $idx: c_int) -> Self::RetType {
					// SAFETY: caller guarantees `$idx` is valid to pass to this function
					if unsafe { $lua.is_none_or_nil($idx) } {
						None
					} else {
						Some($cvt)
					}
				}
			}
		)+
	};
}

// SAFETY: caller guarantees `idx` is valid to pass to these functions
impl_from_lua! { 'lua, lua, idx {
	bool => unsafe { lua.get_bool(idx) },
	char => unsafe { lua.get_char(idx) },
	i8 => unsafe { lua.get_i8(idx) },
	i16 => unsafe { lua.get_i16(idx) },
	i32 => unsafe { lua.get_i32(idx) },
	i64 => unsafe { lua.get_i64(idx) },
	isize => unsafe { lua.get_isize(idx) },
	u8 => unsafe { lua.get_u8(idx) },
	u16 => unsafe { lua.get_u16(idx) },
	u32 => unsafe { lua.get_u32(idx) },
	u64 => unsafe { lua.get_u64(idx) },
	usize => unsafe { lua.get_usize(idx) },
	&'lua [u8] => unsafe { lua.get_byte_slice(idx) },
	Vec<u8> => unsafe { lua.get_bytes(idx) },
}}

impl<'lua, T: LuaUserData + 'lua> FromLua<'lua> for &T {
	type RetType = &'lua T;

	#[inline]
	unsafe fn from_lua(lua: &Lua<'lua>, idx: c_int) -> Self::RetType {
		// SAFETY: caller guarantees `idx` is valid to pass to this function
		unsafe { lua.get_user_data(idx) }
	}
}

impl<'lua, T: LuaUserData + 'lua> FromLua<'lua> for Option<&T> {
	type RetType = Option<&'lua T>;

	#[inline]
	unsafe fn from_lua(lua: &Lua<'lua>, idx: c_int) -> Self::RetType {
		// SAFETY: caller guarantees `idx` is valid to pass to these functions
		unsafe {
			if lua.is_none_or_nil(idx) {
				None
			} else {
				Some(lua.get_user_data(idx))
			}
		}
	}
}

impl<'lua, T: LuaUserData + 'lua> FromLua<'lua> for &mut T {
	type RetType = &'lua mut T;

	#[inline]
	unsafe fn from_lua(lua: &Lua<'lua>, idx: c_int) -> Self::RetType {
		// SAFETY: caller guarantees `idx` is valid to pass to this function
		unsafe { lua.get_user_data_mut(idx) }
	}
}

impl<'lua, T: LuaUserData + 'lua> FromLua<'lua> for Option<&mut T> {
	type RetType = Option<&'lua mut T>;

	#[inline]
	unsafe fn from_lua(lua: &Lua<'lua>, idx: c_int) -> Self::RetType {
		// SAFETY: caller guarantees `idx` is valid to pass to these functions
		unsafe {
			if lua.is_none_or_nil(idx) {
				None
			} else {
				Some(lua.get_user_data_mut(idx))
			}
		}
	}
}
