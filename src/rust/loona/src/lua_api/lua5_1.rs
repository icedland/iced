// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

// Lua 5.1 `lua.h` converted to Rust

#![allow(non_camel_case_types)]
#![allow(non_snake_case)]
#![allow(clippy::missing_safety_doc)]

use libc::{c_char, c_double, c_int, c_void, ptrdiff_t, size_t};
use std::ptr;

// Needed to make the API method sigs identical without #[cfg]s
pub type lua_GetIType = c_int;

pub const LUA_SIGNATURE: &[u8] = b"\x1BLua";

pub const LUA_MULTRET: c_int = -1;

pub const LUA_REGISTRYINDEX: c_int = -10000;
pub const LUA_ENVIRONINDEX: c_int = -10001;
pub const LUA_GLOBALSINDEX: c_int = -10002;

#[inline]
pub unsafe fn lua_upvalueindex(i: c_int) -> c_int {
	LUA_GLOBALSINDEX - i
}

pub const LUA_YIELD: c_int = 1;
pub const LUA_ERRRUN: c_int = 2;
pub const LUA_ERRSYNTAX: c_int = 3;
pub const LUA_ERRMEM: c_int = 4;
pub const LUA_ERRERR: c_int = 5;

// It's probably not safe for `struct Lua` to impl Send or Sync so this type must not be Send + Sync.
pub type lua_State = *mut c_void;
pub type lua_CFunction = unsafe extern "C" fn(L: lua_State) -> c_int;
pub type lua_Reader = unsafe extern "C" fn(L: lua_State, ud: *mut c_void, sz: *mut size_t) -> *const c_char;
pub type lua_Writer = unsafe extern "C" fn(L: lua_State, p: *const c_void, sz: size_t, ud: *mut c_void) -> c_int;
pub type lua_Alloc = unsafe extern "C" fn(ud: *mut c_void, ptr: *mut c_void, osize: size_t, nsize: size_t) -> *mut c_void;

pub const LUA_TNONE: c_int = -1;

pub const LUA_TNIL: c_int = 0;
pub const LUA_TBOOLEAN: c_int = 1;
pub const LUA_TLIGHTUSERDATA: c_int = 2;
pub const LUA_TNUMBER: c_int = 3;
pub const LUA_TSTRING: c_int = 4;
pub const LUA_TTABLE: c_int = 5;
pub const LUA_TFUNCTION: c_int = 6;
pub const LUA_TUSERDATA: c_int = 7;
pub const LUA_TTHREAD: c_int = 8;

pub const LUA_MINSTACK: c_int = 20;

pub type lua_Number = c_double;
pub type lua_Integer = ptrdiff_t;

extern "C" {
	pub fn lua_newstate(f: lua_Alloc, ud: *mut c_void) -> lua_State;
	pub fn lua_close(L: lua_State);
	pub fn lua_newthread(L: lua_State) -> lua_State;

	pub fn lua_atpanic(L: lua_State, panicf: lua_CFunction) -> lua_CFunction;

	pub fn lua_gettop(L: lua_State) -> c_int;
	pub fn lua_settop(L: lua_State, idx: c_int);
	pub fn lua_pushvalue(L: lua_State, idx: c_int);
	pub fn lua_remove(L: lua_State, idx: c_int);
	pub fn lua_insert(L: lua_State, idx: c_int);
	pub fn lua_replace(L: lua_State, idx: c_int);
	pub fn lua_checkstack(L: lua_State, sz: c_int) -> c_int;

	pub fn lua_xmove(from: lua_State, to: lua_State, n: c_int);

	pub fn lua_isnumber(L: lua_State, idx: c_int) -> c_int;
	pub fn lua_isstring(L: lua_State, idx: c_int) -> c_int;
	pub fn lua_iscfunction(L: lua_State, idx: c_int) -> c_int;
	pub fn lua_isuserdata(L: lua_State, idx: c_int) -> c_int;
	pub fn lua_type(L: lua_State, idx: c_int) -> c_int;
	pub fn lua_typename(L: lua_State, tp: c_int) -> *const c_char;

	pub fn lua_equal(L: lua_State, idx1: c_int, idx2: c_int) -> c_int;
	pub fn lua_rawequal(L: lua_State, idx1: c_int, idx2: c_int) -> c_int;
	pub fn lua_lessthan(L: lua_State, idx1: c_int, idx2: c_int) -> c_int;

	pub fn lua_tonumber(L: lua_State, idx: c_int) -> lua_Number;
	pub fn lua_tointeger(L: lua_State, idx: c_int) -> lua_Integer;
	pub fn lua_toboolean(L: lua_State, idx: c_int) -> c_int;
	pub fn lua_tolstring(L: lua_State, idx: c_int, len: *mut size_t) -> *const c_char;
	pub fn lua_objlen(L: lua_State, idx: c_int) -> size_t;
	pub fn lua_tocfunction(L: lua_State, idx: c_int) -> lua_CFunction;
	pub fn lua_touserdata(L: lua_State, idx: c_int) -> *mut c_void;
	pub fn lua_tothread(L: lua_State, idx: c_int) -> lua_State;
	pub fn lua_topointer(L: lua_State, idx: c_int) -> *const c_void;

	pub fn lua_pushnil(L: lua_State);
	pub fn lua_pushnumber(L: lua_State, n: lua_Number);
	pub fn lua_pushinteger(L: lua_State, n: lua_Integer);
	pub fn lua_pushlstring(L: lua_State, s: *const c_char, l: size_t);
	pub fn lua_pushstring(L: lua_State, s: *const c_char);
	// pub fn lua_pushvfstring(L: lua_State, fmt: *const c_char, argp: va_list) -> *const c_char;
	pub fn lua_pushfstring(L: lua_State, fmt: *const c_char, ...) -> *const c_char;
	pub fn lua_pushcclosure(L: lua_State, f: lua_CFunction, n: c_int);
	pub fn lua_pushboolean(L: lua_State, b: c_int);
	pub fn lua_pushlightuserdata(L: lua_State, p: *const c_void);
	pub fn lua_pushthread(L: lua_State) -> c_int;

	pub fn lua_gettable(L: lua_State, idx: c_int);
	pub fn lua_getfield(L: lua_State, idx: c_int, k: *const c_char);
	pub fn lua_rawget(L: lua_State, idx: c_int);
	pub fn lua_rawgeti(L: lua_State, idx: c_int, n: lua_GetIType);
	pub fn lua_createtable(L: lua_State, narr: c_int, nrec: c_int);
	pub fn lua_newuserdata(L: lua_State, sz: size_t) -> *mut c_void;
	pub fn lua_getmetatable(L: lua_State, objindex: c_int) -> c_int;
	pub fn lua_getfenv(L: lua_State, idx: c_int);

	pub fn lua_settable(L: lua_State, idx: c_int);
	pub fn lua_setfield(L: lua_State, idx: c_int, k: *const c_char);
	pub fn lua_rawset(L: lua_State, idx: c_int);
	pub fn lua_rawseti(L: lua_State, idx: c_int, n: lua_GetIType);
	pub fn lua_setmetatable(L: lua_State, objindex: c_int) -> c_int;
	pub fn lua_setfenv(L: lua_State, idx: c_int) -> c_int;

	pub fn lua_call(L: lua_State, nargs: c_int, nresults: c_int);
	pub fn lua_pcall(L: lua_State, nargs: c_int, nresults: c_int, errfunc: c_int) -> c_int;
	pub fn lua_cpcall(L: lua_State, func: lua_CFunction, ud: *mut c_void) -> c_int;
	pub fn lua_load(L: lua_State, reader: lua_Reader, dt: *mut c_void, chunkname: *const c_char) -> c_int;

	pub fn lua_dump(L: lua_State, writer: lua_Writer, data: *mut c_void) -> c_int;

	pub fn lua_yield(L: lua_State, nresults: c_int) -> c_int;
	pub fn lua_resume(L: lua_State, narg: c_int) -> c_int;
	pub fn lua_status(L: lua_State) -> c_int;
}

pub const LUA_GCSTOP: c_int = 0;
pub const LUA_GCRESTART: c_int = 1;
pub const LUA_GCCOLLECT: c_int = 2;
pub const LUA_GCCOUNT: c_int = 3;
pub const LUA_GCCOUNTB: c_int = 4;
pub const LUA_GCSTEP: c_int = 5;
pub const LUA_GCSETPAUSE: c_int = 6;
pub const LUA_GCSETSTEPMUL: c_int = 7;

extern "C" {
	pub fn lua_gc(L: lua_State, what: c_int, data: c_int) -> c_int;

	pub fn lua_error(L: lua_State) -> !;

	pub fn lua_next(L: lua_State, idx: c_int) -> c_int;

	pub fn lua_concat(L: lua_State, n: c_int);

	pub fn lua_getallocf(L: lua_State, ud: *mut *mut c_void) -> lua_Alloc;
	pub fn lua_setallocf(L: lua_State, f: lua_Alloc, ud: *mut c_void);
}

#[inline]
pub unsafe fn lua_pop(L: lua_State, n: c_int) {
	unsafe { lua_settop(L, -n - 1) };
}

#[inline]
pub unsafe fn lua_newtable(L: lua_State) {
	unsafe { lua_createtable(L, 0, 0) };
}

#[inline]
pub unsafe fn lua_register(L: lua_State, n: *const c_char, f: lua_CFunction) {
	unsafe {
		lua_pushcfunction(L, f);
		lua_setglobal(L, n);
	}
}

#[inline]
pub unsafe fn lua_pushcfunction(L: lua_State, f: lua_CFunction) {
	unsafe { lua_pushcclosure(L, f, 0) };
}

#[inline]
pub unsafe fn lua_strlen(L: lua_State, i: c_int) -> size_t {
	unsafe { lua_objlen(L, i) }
}

#[inline]
pub unsafe fn lua_isfunction(L: lua_State, n: c_int) -> bool {
	unsafe { lua_type(L, n) == LUA_TFUNCTION }
}

#[inline]
pub unsafe fn lua_istable(L: lua_State, n: c_int) -> bool {
	unsafe { lua_type(L, n) == LUA_TTABLE }
}

#[inline]
pub unsafe fn lua_islightuserdata(L: lua_State, n: c_int) -> bool {
	unsafe { lua_type(L, n) == LUA_TLIGHTUSERDATA }
}

#[inline]
pub unsafe fn lua_isnil(L: lua_State, n: c_int) -> bool {
	unsafe { lua_type(L, n) == LUA_TNIL }
}

#[inline]
pub unsafe fn lua_isboolean(L: lua_State, n: c_int) -> bool {
	unsafe { lua_type(L, n) == LUA_TBOOLEAN }
}

#[inline]
pub unsafe fn lua_isthread(L: lua_State, n: c_int) -> bool {
	unsafe { lua_type(L, n) == LUA_TTHREAD }
}

#[inline]
pub unsafe fn lua_isnone(L: lua_State, n: c_int) -> bool {
	unsafe { lua_type(L, n) == LUA_TNONE }
}

#[inline]
pub unsafe fn lua_isnoneornil(L: lua_State, n: c_int) -> bool {
	unsafe { lua_type(L, n) <= 0 }
}

#[inline]
pub unsafe fn lua_pushliteral(L: lua_State, s: &str) {
	unsafe { lua_pushlstring(L, s.as_ptr().cast(), s.len()) };
}

#[inline]
pub unsafe fn lua_setglobal(L: lua_State, s: *const c_char) {
	unsafe { lua_setfield(L, LUA_GLOBALSINDEX, s) };
}

#[inline]
pub unsafe fn lua_getglobal(L: lua_State, s: *const c_char) {
	unsafe { lua_getfield(L, LUA_GLOBALSINDEX, s) };
}

#[inline]
pub unsafe fn lua_tostring(L: lua_State, i: c_int) -> *const c_char {
	unsafe { lua_tolstring(L, i, ptr::null_mut()) }
}

#[inline]
pub unsafe fn lua_getregistry(L: lua_State) {
	unsafe { lua_pushvalue(L, LUA_REGISTRYINDEX) };
}

#[inline]
pub unsafe fn lua_getgccount(L: lua_State) -> c_int {
	unsafe { lua_gc(L, LUA_GCCOUNT, 0) }
}

pub type lua_Chunkreader = lua_Reader;
pub type lua_Chunkwriter = lua_Writer;

extern "C" {
	#[cfg(feature = "lua5_1_3")]
	pub fn lua_setlevel(from: lua_State, to: lua_State);
}

pub const LUA_HOOKCALL: c_int = 0;
pub const LUA_HOOKRET: c_int = 1;
pub const LUA_HOOKLINE: c_int = 2;
pub const LUA_HOOKCOUNT: c_int = 3;
pub const LUA_HOOKTAILRET: c_int = 4;

pub const LUA_MASKCALL: c_int = 1 << LUA_HOOKCALL;
pub const LUA_MASKRET: c_int = 1 << LUA_HOOKRET;
pub const LUA_MASKLINE: c_int = 1 << LUA_HOOKLINE;
pub const LUA_MASKCOUNT: c_int = 1 << LUA_HOOKCOUNT;

pub type lua_Hook = unsafe extern "C" fn(L: lua_State, ar: *mut lua_Debug);

extern "C" {
	pub fn lua_getstack(L: lua_State, level: c_int, ar: *mut lua_Debug) -> c_int;
	pub fn lua_getinfo(L: lua_State, what: *const c_char, ar: *mut lua_Debug) -> c_int;
	pub fn lua_getlocal(L: lua_State, ar: *const lua_Debug, n: c_int) -> *const c_char;
	pub fn lua_setlocal(L: lua_State, ar: *const lua_Debug, n: c_int) -> *const c_char;
	pub fn lua_getupvalue(L: lua_State, funcindex: c_int, n: c_int) -> *const c_char;
	pub fn lua_setupvalue(L: lua_State, funcindex: c_int, n: c_int) -> *const c_char;

	pub fn lua_sethook(L: lua_State, func: lua_Hook, mask: c_int, count: c_int) -> c_int;
	pub fn lua_gethook(L: lua_State) -> lua_Hook;
	pub fn lua_gethookmask(L: lua_State) -> c_int;
	pub fn lua_gethookcount(L: lua_State) -> c_int;
}

pub const LUA_IDSIZE: usize = 60;

#[allow(missing_copy_implementations)]
#[allow(missing_debug_implementations)]
#[repr(C)]
pub struct lua_Debug {
	pub event: c_int,
	pub name: *const c_char,
	pub namewhat: *const c_char,
	pub what: *const c_char,
	pub source: *const c_char,
	pub currentline: c_int,
	pub nups: c_int,
	pub linedefined: c_int,
	pub lastlinedefined: c_int,
	pub short_src: [c_char; LUA_IDSIZE],
	pub i_ci: c_int,
}

extern "C" {
	pub fn luaL_newmetatable(L: lua_State, tname: *const c_char) -> c_int;
}

#[inline]
pub unsafe fn luaL_getmetatable(L: lua_State, n: *const c_char) {
	unsafe {
		lua_getfield(L, LUA_REGISTRYINDEX, n);
	}
}
