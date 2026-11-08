// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

// The parts of Lua 5.4 `lua.h` that are used by this crate, converted to Rust

#![allow(non_camel_case_types)]
#![allow(non_snake_case)]
#![allow(clippy::missing_safety_doc)]

use libc::{c_char, c_int, c_void, size_t};

// Needed to make the API method sigs identical without #[cfg]s
pub type lua_GetIType = lua_Integer;

// It's probably not safe for `struct Lua` to impl Send or Sync so this type must not be Send + Sync.
pub type lua_State = *mut c_void;

pub const LUA_TNIL: i32 = 0;
pub const LUA_TBOOLEAN: i32 = 1;
pub const LUA_TNUMBER: i32 = 3;
pub const LUA_TSTRING: i32 = 4;
pub const LUA_TTABLE: i32 = 5;
pub const LUA_TUSERDATA: i32 = 7;

pub const LUA_MINSTACK: i32 = 20;

// The following types depend on `luaconf.h` and assume a Lua lib built with the default config:
// `lua_Integer`, `lua_Unsigned`. Eg. a Lua lib built with `LUA_32BITS` isn't supported.
pub type lua_Integer = i64;
pub type lua_Unsigned = u64;

pub type lua_CFunction = unsafe extern "C" fn(L: lua_State) -> c_int;

extern "C" {
	pub fn lua_gettop(L: lua_State) -> c_int;
	pub fn lua_settop(L: lua_State, idx: c_int);
	pub fn lua_pushvalue(L: lua_State, idx: c_int);
	pub fn lua_copy(L: lua_State, fromidx: c_int, toidx: c_int);
	pub fn lua_checkstack(L: lua_State, n: c_int) -> c_int;

	pub fn lua_type(L: lua_State, idx: c_int) -> c_int;

	pub fn lua_tointegerx(L: lua_State, idx: c_int, isnum: *mut c_int) -> lua_Integer;
	pub fn lua_toboolean(L: lua_State, idx: c_int) -> c_int;
	pub fn lua_tolstring(L: lua_State, idx: c_int, len: *mut size_t) -> *const c_char;
	pub fn lua_rawlen(L: lua_State, idx: c_int) -> lua_Unsigned;
	pub fn lua_touserdata(L: lua_State, idx: c_int) -> *mut c_void;

	pub fn lua_pushnil(L: lua_State);
	pub fn lua_pushinteger(L: lua_State, n: lua_Integer);
	pub fn lua_pushlstring(L: lua_State, s: *const c_char, len: size_t) -> *const c_char;
	pub fn lua_pushcclosure(L: lua_State, f: lua_CFunction, n: c_int);
	pub fn lua_pushboolean(L: lua_State, b: c_int);

	pub fn lua_rawget(L: lua_State, idx: c_int) -> c_int;
	pub fn lua_rawgeti(L: lua_State, idx: c_int, n: lua_GetIType) -> c_int;

	pub fn lua_createtable(L: lua_State, narr: c_int, nrec: c_int);
	pub fn lua_newuserdatauv(L: lua_State, sz: size_t, nuvalue: c_int) -> *mut c_void;

	pub fn lua_rawset(L: lua_State, idx: c_int);
	pub fn lua_setmetatable(L: lua_State, objindex: c_int) -> c_int;

	pub fn lua_error(L: lua_State) -> !;
}

#[inline]
pub unsafe fn lua_pop(L: lua_State, n: c_int) {
	unsafe {
		lua_settop(L, -n - 1);
	}
}

#[inline]
pub unsafe fn lua_newtable(L: lua_State) {
	unsafe {
		lua_createtable(L, 0, 0);
	}
}

#[inline]
pub unsafe fn lua_pushcfunction(L: lua_State, f: lua_CFunction) {
	unsafe {
		lua_pushcclosure(L, f, 0);
	}
}

#[inline]
pub unsafe fn lua_istable(L: lua_State, n: c_int) -> bool {
	unsafe { lua_type(L, n) == LUA_TTABLE }
}

#[inline]
pub unsafe fn lua_isnil(L: lua_State, n: c_int) -> bool {
	unsafe { lua_type(L, n) == LUA_TNIL }
}

#[inline]
pub unsafe fn lua_isnoneornil(L: lua_State, n: c_int) -> bool {
	unsafe { lua_type(L, n) <= 0 }
}

#[inline]
pub unsafe fn lua_pushliteral(L: lua_State, s: &str) {
	unsafe {
		let _ = lua_pushlstring(L, s.as_ptr().cast(), s.len());
	}
}

#[inline]
pub unsafe fn lua_replace(L: lua_State, idx: c_int) {
	unsafe {
		lua_copy(L, -1, idx);
		lua_pop(L, 1);
	}
}

extern "C" {
	pub fn luaL_newmetatable(L: lua_State, tname: *const c_char) -> c_int;
}
