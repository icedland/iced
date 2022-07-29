// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

// Lua 5.2 `lua.h` converted to Rust

#![allow(non_camel_case_types)]
#![allow(non_snake_case)]
#![allow(clippy::missing_safety_doc)]
#![allow(clippy::unreadable_literal)]

use libc::{c_char, c_double, c_int, c_uchar, c_uint, c_void, ptrdiff_t, size_t};
use std::{mem, ptr};

// Needed to make the API method sigs identical without #[cfg]s
pub type lua_GetIType = c_int;

pub const LUA_SIGNATURE: &[u8] = b"\x1BLua";

pub const LUA_MULTRET: i32 = -1;

pub const LUAI_MAXSTACK: i32 = 1000000;
pub const LUAI_FIRSTPSEUDOIDX: i32 = -LUAI_MAXSTACK - 1000;
pub const LUA_REGISTRYINDEX: i32 = LUAI_FIRSTPSEUDOIDX;

#[inline]
pub unsafe fn lua_upvalueindex(i: i32) -> i32 {
	LUA_REGISTRYINDEX - i
}

pub const LUA_OK: i32 = 0;
pub const LUA_YIELD: i32 = 1;
pub const LUA_ERRRUN: i32 = 2;
pub const LUA_ERRSYNTAX: i32 = 3;
pub const LUA_ERRMEM: i32 = 4;
pub const LUA_ERRGCMM: i32 = 5;
pub const LUA_ERRERR: i32 = 6;

// It's probably not safe for `struct Lua` to impl Send or Sync so this type must not be Send + Sync.
pub type lua_State = *mut c_void;

pub type lua_CFunction = unsafe extern "C" fn(L: lua_State) -> c_int;

pub type lua_Reader = unsafe extern "C" fn(L: lua_State, ud: *mut c_void, sz: *mut size_t) -> *const c_char;

pub type lua_Writer = unsafe extern "C" fn(L: lua_State, p: *const c_void, sz: size_t, ud: *mut c_void) -> c_int;

pub type lua_Alloc = unsafe extern "C" fn(ud: *mut c_void, ptr: *mut c_void, osize: size_t, nsize: size_t) -> *mut c_void;

pub const LUA_TNONE: i32 = -1;

pub const LUA_TNIL: i32 = 0;
pub const LUA_TBOOLEAN: i32 = 1;
pub const LUA_TLIGHTUSERDATA: i32 = 2;
pub const LUA_TNUMBER: i32 = 3;
pub const LUA_TSTRING: i32 = 4;
pub const LUA_TTABLE: i32 = 5;
pub const LUA_TFUNCTION: i32 = 6;
pub const LUA_TUSERDATA: i32 = 7;
pub const LUA_TTHREAD: i32 = 8;

pub const LUA_NUMTAGS: i32 = 9;

pub const LUA_MINSTACK: i32 = 20;

pub const LUA_RIDX_MAINTHREAD: i32 = 1;
pub const LUA_RIDX_GLOBALS: i32 = 2;
pub const LUA_RIDX_LAST: i32 = LUA_RIDX_GLOBALS;

pub type lua_Number = c_double;
pub type lua_Integer = ptrdiff_t;
pub type lua_Unsigned = c_uint;

extern "C" {
	#[cfg(feature = "lua5_2_2")]
	pub static lua_ident: *const c_char;
}

extern "C" {
	pub fn lua_newstate(f: lua_Alloc, ud: *mut c_void) -> lua_State;
	pub fn lua_close(L: lua_State);
	pub fn lua_newthread(L: lua_State) -> lua_State;

	pub fn lua_atpanic(L: lua_State, panicf: lua_CFunction) -> lua_CFunction;

	pub fn lua_version(L: lua_State) -> *const lua_Number;

	pub fn lua_absindex(L: lua_State, idx: c_int) -> c_int;
	pub fn lua_gettop(L: lua_State) -> c_int;
	pub fn lua_settop(L: lua_State, idx: c_int);
	pub fn lua_pushvalue(L: lua_State, idx: c_int);
	pub fn lua_remove(L: lua_State, idx: c_int);
	pub fn lua_insert(L: lua_State, idx: c_int);
	pub fn lua_replace(L: lua_State, idx: c_int);
	pub fn lua_copy(L: lua_State, fromidx: c_int, toidx: c_int);
	pub fn lua_checkstack(L: lua_State, sz: c_int) -> c_int;

	pub fn lua_xmove(from: lua_State, to: lua_State, n: c_int);

	pub fn lua_isnumber(L: lua_State, idx: c_int) -> c_int;
	pub fn lua_isstring(L: lua_State, idx: c_int) -> c_int;
	pub fn lua_iscfunction(L: lua_State, idx: c_int) -> c_int;
	pub fn lua_isuserdata(L: lua_State, idx: c_int) -> c_int;
	pub fn lua_type(L: lua_State, idx: c_int) -> c_int;
	pub fn lua_typename(L: lua_State, tp: c_int) -> *const c_char;

	pub fn lua_tonumberx(L: lua_State, idx: c_int, isnum: *mut c_int) -> lua_Number;
	pub fn lua_tointegerx(L: lua_State, idx: c_int, isnum: *mut c_int) -> lua_Integer;
	pub fn lua_tounsignedx(L: lua_State, idx: c_int, isnum: *mut c_int) -> lua_Unsigned;
	pub fn lua_toboolean(L: lua_State, idx: c_int) -> c_int;
	pub fn lua_tolstring(L: lua_State, idx: c_int, len: *mut size_t) -> *const c_char;
	pub fn lua_rawlen(L: lua_State, idx: c_int) -> size_t;
	pub fn lua_tocfunction(L: lua_State, idx: c_int) -> lua_CFunction;
	pub fn lua_touserdata(L: lua_State, idx: c_int) -> *mut c_void;
	pub fn lua_tothread(L: lua_State, idx: c_int) -> lua_State;
	pub fn lua_topointer(L: lua_State, idx: c_int) -> *const c_void;
}

pub const LUA_OPADD: i32 = 0;
pub const LUA_OPSUB: i32 = 1;
pub const LUA_OPMUL: i32 = 2;
pub const LUA_OPDIV: i32 = 3;
pub const LUA_OPMOD: i32 = 4;
pub const LUA_OPPOW: i32 = 5;
pub const LUA_OPUNM: i32 = 6;

extern "C" {
	pub fn lua_arith(L: lua_State, op: c_int);
}

pub const LUA_OPEQ: i32 = 0;
pub const LUA_OPLT: i32 = 1;
pub const LUA_OPLE: i32 = 2;

extern "C" {
	pub fn lua_rawequal(L: lua_State, idx1: c_int, idx2: c_int) -> c_int;
	pub fn lua_compare(L: lua_State, idx1: c_int, idx2: c_int, op: c_int) -> c_int;

	pub fn lua_pushnil(L: lua_State);
	pub fn lua_pushnumber(L: lua_State, n: lua_Number);
	pub fn lua_pushinteger(L: lua_State, n: lua_Integer);
	pub fn lua_pushunsigned(L: lua_State, n: lua_Unsigned);
	pub fn lua_pushlstring(L: lua_State, s: *const c_char, l: size_t) -> *const c_char;
	pub fn lua_pushstring(L: lua_State, s: *const c_char) -> *const c_char;
	// pub fn lua_pushvfstring(L: lua_State, fmt: *const c_char, argp: va_list) -> *const c_char;
	pub fn lua_pushfstring(L: lua_State, fmt: *const c_char, ...) -> *const c_char;
	pub fn lua_pushcclosure(L: lua_State, f: lua_CFunction, n: c_int);
	pub fn lua_pushboolean(L: lua_State, b: c_int);
	pub fn lua_pushlightuserdata(L: lua_State, p: *const c_void);
	pub fn lua_pushthread(L: lua_State) -> c_int;

	pub fn lua_getglobal(L: lua_State, var: *const c_char);
	pub fn lua_gettable(L: lua_State, idx: c_int);
	pub fn lua_getfield(L: lua_State, idx: c_int, k: *const c_char);
	pub fn lua_rawget(L: lua_State, idx: c_int);
	pub fn lua_rawgeti(L: lua_State, idx: c_int, n: lua_GetIType);
	pub fn lua_rawgetp(L: lua_State, idx: c_int, p: *const c_void);
	pub fn lua_createtable(L: lua_State, narr: c_int, nrec: c_int);
	pub fn lua_newuserdata(L: lua_State, sz: size_t) -> *mut c_void;
	pub fn lua_getmetatable(L: lua_State, objindex: c_int) -> c_int;
	pub fn lua_getuservalue(L: lua_State, idx: c_int);

	pub fn lua_setglobal(L: lua_State, var: *const c_char);
	pub fn lua_settable(L: lua_State, idx: c_int);
	pub fn lua_setfield(L: lua_State, idx: c_int, k: *const c_char);
	pub fn lua_rawset(L: lua_State, idx: c_int);
	pub fn lua_rawseti(L: lua_State, idx: c_int, n: lua_GetIType);
	pub fn lua_rawsetp(L: lua_State, idx: c_int, p: *const c_void);
	pub fn lua_setmetatable(L: lua_State, objindex: c_int) -> c_int;
	pub fn lua_setuservalue(L: lua_State, idx: c_int);

	pub fn lua_callk(L: lua_State, nargs: c_int, nresults: c_int, ctx: c_int, k: Option<lua_CFunction>);

	pub fn lua_getctx(L: lua_State, ctx: *mut c_int) -> c_int;

	pub fn lua_pcallk(L: lua_State, nargs: c_int, nresults: c_int, errfunc: c_int, ctx: c_int, k: Option<lua_CFunction>) -> c_int;

	pub fn lua_load(L: lua_State, reader: lua_Reader, dt: *mut c_void, chunkname: *const c_char, mode: *const c_char) -> c_int;

	pub fn lua_dump(L: lua_State, writer: lua_Writer, data: *mut c_void) -> c_int;

	pub fn lua_yieldk(L: lua_State, nresults: c_int, ctx: c_int, k: Option<lua_CFunction>) -> c_int;
	pub fn lua_resume(L: lua_State, from: lua_State, narg: c_int) -> c_int;
	pub fn lua_status(L: lua_State) -> c_int;
}

const _: () = assert!(mem::size_of::<Option<lua_CFunction>>() == mem::size_of::<lua_CFunction>());

#[inline]
pub unsafe fn lua_call(L: lua_State, n: c_int, r: c_int) {
	unsafe {
		lua_callk(L, n, r, 0, None);
	}
}

#[inline]
pub unsafe fn lua_pcall(L: lua_State, n: c_int, r: c_int, f: c_int) -> c_int {
	unsafe { lua_pcallk(L, n, r, f, 0, None) }
}

#[inline]
pub unsafe fn lua_yield(L: lua_State, n: c_int) -> c_int {
	unsafe { lua_yieldk(L, n, 0, None) }
}

#[inline]
pub unsafe fn lua_strlen(L: lua_State, i: c_int) -> size_t {
	unsafe { lua_rawlen(L, i) }
}

#[inline]
pub unsafe fn lua_objlen(L: lua_State, i: c_int) -> size_t {
	unsafe { lua_rawlen(L, i) }
}

#[inline]
pub unsafe fn lua_equal(L: lua_State, idx1: c_int, idx2: c_int) -> c_int {
	unsafe { lua_compare(L, idx1, idx2, LUA_OPEQ) }
}

#[inline]
pub unsafe fn lua_lessthan(L: lua_State, idx1: c_int, idx2: c_int) -> c_int {
	unsafe { lua_compare(L, idx1, idx2, LUA_OPLT) }
}

pub const LUA_GCSTOP: i32 = 0;
pub const LUA_GCRESTART: i32 = 1;
pub const LUA_GCCOLLECT: i32 = 2;
pub const LUA_GCCOUNT: i32 = 3;
pub const LUA_GCCOUNTB: i32 = 4;
pub const LUA_GCSTEP: i32 = 5;
pub const LUA_GCSETPAUSE: i32 = 6;
pub const LUA_GCSETSTEPMUL: i32 = 7;
pub const LUA_GCSETMAJORINC: i32 = 8;
pub const LUA_GCISRUNNING: i32 = 9;
pub const LUA_GCGEN: i32 = 10;
pub const LUA_GCINC: i32 = 11;

extern "C" {
	pub fn lua_gc(L: lua_State, what: c_int, data: c_int) -> c_int;

	pub fn lua_error(L: lua_State) -> !;

	pub fn lua_next(L: lua_State, idx: c_int) -> c_int;

	pub fn lua_concat(L: lua_State, n: c_int);
	pub fn lua_len(L: lua_State, idx: c_int);

	pub fn lua_getallocf(L: lua_State, ud: *mut *mut c_void) -> lua_Alloc;
	pub fn lua_setallocf(L: lua_State, f: lua_Alloc, ud: *mut c_void);
}

#[inline]
pub unsafe fn lua_tonumber(L: lua_State, i: c_int) -> lua_Number {
	unsafe { lua_tonumberx(L, i, ptr::null_mut()) }
}

#[inline]
pub unsafe fn lua_tointeger(L: lua_State, i: c_int) -> lua_Integer {
	unsafe { lua_tointegerx(L, i, ptr::null_mut()) }
}

#[inline]
pub unsafe fn lua_tounsigned(L: lua_State, i: c_int) -> lua_Unsigned {
	unsafe { lua_tounsignedx(L, i, ptr::null_mut()) }
}

#[inline]
pub unsafe fn lua_pop(L: lua_State, n: c_int) {
	unsafe {
		lua_settop(L, -(n) - 1);
	}
}

#[inline]
pub unsafe fn lua_newtable(L: lua_State) {
	unsafe {
		lua_createtable(L, 0, 0);
	}
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
	unsafe {
		lua_pushcclosure(L, f, 0);
	}
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
	unsafe {
		let _ = lua_pushlstring(L, s.as_ptr().cast(), s.len());
	}
}

#[inline]
pub unsafe fn lua_pushglobaltable(L: lua_State) {
	unsafe {
		lua_rawgeti(L, LUA_REGISTRYINDEX, LUA_RIDX_GLOBALS);
	}
}

#[inline]
pub unsafe fn lua_tostring(L: lua_State, i: c_int) -> *const c_char {
	unsafe { lua_tolstring(L, i, ptr::null_mut()) }
}

pub const LUA_HOOKCALL: i32 = 0;
pub const LUA_HOOKRET: i32 = 1;
pub const LUA_HOOKLINE: i32 = 2;
pub const LUA_HOOKCOUNT: i32 = 3;
pub const LUA_HOOKTAILCALL: i32 = 4;

pub const LUA_MASKCALL: i32 = 1 << LUA_HOOKCALL;
pub const LUA_MASKRET: i32 = 1 << LUA_HOOKRET;
pub const LUA_MASKLINE: i32 = 1 << LUA_HOOKLINE;
pub const LUA_MASKCOUNT: i32 = 1 << LUA_HOOKCOUNT;

pub type lua_Hook = unsafe extern "C" fn(L: lua_State, ar: *mut lua_Debug);

extern "C" {
	pub fn lua_getstack(L: lua_State, level: c_int, ar: *mut lua_Debug) -> c_int;
	pub fn lua_getinfo(L: lua_State, what: *const c_char, ar: *mut lua_Debug) -> c_int;
	pub fn lua_getlocal(L: lua_State, ar: *const lua_Debug, n: c_int) -> *const c_char;
	pub fn lua_setlocal(L: lua_State, ar: *const lua_Debug, n: c_int) -> *const c_char;
	pub fn lua_getupvalue(L: lua_State, funcindex: c_int, n: c_int) -> *const c_char;
	pub fn lua_setupvalue(L: lua_State, funcindex: c_int, n: c_int) -> *const c_char;

	pub fn lua_upvalueid(L: lua_State, fidx: c_int, n: c_int) -> *mut c_void;
	pub fn lua_upvaluejoin(L: lua_State, fidx1: c_int, n1: c_int, fidx2: c_int, n2: c_int);

	pub fn lua_sethook(L: lua_State, func: lua_Hook, mask: c_int, count: c_int) -> c_int;
	pub fn lua_gethook(L: lua_State) -> lua_Hook;
	pub fn lua_gethookmask(L: lua_State) -> c_int;
	pub fn lua_gethookcount(L: lua_State) -> c_int;
}

pub const LUA_IDSIZE: usize = 60;

pub type CallInfo = c_void;

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
	pub linedefined: c_int,
	pub lastlinedefined: c_int,
	pub nups: c_uchar,
	pub nparams: c_uchar,
	pub isvararg: c_char,
	pub istailcall: c_char,
	pub short_src: [c_char; LUA_IDSIZE],
	pub i_ci: *mut CallInfo,
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
