// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#![allow(clippy::missing_safety_doc)]
#![allow(clippy::missing_errors_doc)]

use crate::lua_api::*;
use libc::{c_char, c_int, c_void, size_t};
use static_assertions::const_assert;
use std::error::Error;
use std::marker::PhantomData;
use std::{mem, ptr, slice};

pub unsafe trait LuaUserData {
	/// This must be a unique ID. No other `LuaUserData` must have the same value. This should also
	/// be a 'random' number, i.e., not common numbers such as 0, 1, etc.
	const UNIQUE_ID: u32;
	/// This must be a unique metatable key passed to `luaL_newmetatable()` and `luaL_getmetatable()`
	const METATABLE_KEY: LuaCStr<'static>;
}

#[allow(missing_debug_implementations)]
#[allow(missing_copy_implementations)]
pub struct LuaCStr<'a>(pub &'a [u8]);

#[derive(Debug, Clone, Copy, PartialEq, Eq)]
pub enum ConvErr {
	/// Doesn't fit in target type (eg. an i32)
	OutOfRange,
	/// It's not a number (or number as a string), eg. it's a table or something
	NotANumber,
}

macro_rules! gen_get_signed_int {
	($try_get_i:ident, $get_i:ident, $get_i_default:ident, $ty_i:ty, $ty_u:ty) => {
		#[inline]
		pub unsafe fn $try_get_i(&self, idx: c_int) -> Result<$ty_i, ConvErr> {
			unsafe {
				match self.try_get_i64(idx) {
					Ok(value) => {
						// We accept all valid signed and unsigned values
						if <$ty_i>::MIN as i64 <= value && value <= <$ty_u>::MAX as i64 {
							Ok(value as $ty_i)
						} else {
							Err(ConvErr::OutOfRange)
						}
					}
					Err(e) => Err(e),
				}
			}
		}

		#[inline]
		pub unsafe fn $get_i(&self, idx: c_int) -> $ty_i {
			unsafe {
				match self.$try_get_i(idx) {
					Ok(value) => value,
					Err(e) => {
						let msg = match e {
							ConvErr::OutOfRange => "Integer is too big/small",
							ConvErr::NotANumber => "Expected an integer",
						};
						self.push_literal(msg);
						self.error();
					}
				}
			}
		}

		#[inline]
		pub unsafe fn $get_i_default(&self, idx: c_int, default: $ty_i) -> $ty_i {
			unsafe {
				if self.is_none_or_nil(idx) {
					default
				} else {
					match self.$try_get_i(idx) {
						Ok(value) => value,
						Err(e) => {
							let msg = match e {
								ConvErr::OutOfRange => "Integer is too big/small",
								ConvErr::NotANumber => "Expected an optional integer",
							};
							self.push_literal(msg);
							self.error();
						}
					}
				}
			}
		}
	};
}

macro_rules! gen_get_unsigned_int {
	($try_get_u:ident, $get_u:ident, $get_u_default:ident, $ty_i:ty, $ty_u:ty) => {
		#[inline]
		pub unsafe fn $try_get_u(&self, idx: c_int) -> Result<$ty_u, ConvErr> {
			unsafe {
				match self.try_get_i64(idx) {
					Ok(value) => {
						// We accept all valid signed and unsigned values
						if <$ty_i>::MIN as i64 <= value && value <= <$ty_u>::MAX as i64 {
							Ok(value as $ty_u)
						} else {
							Err(ConvErr::OutOfRange)
						}
					}
					Err(e) => Err(e),
				}
			}
		}

		#[inline]
		pub unsafe fn $get_u(&self, idx: c_int) -> $ty_u {
			unsafe {
				match self.$try_get_u(idx) {
					Ok(value) => value,
					Err(e) => {
						let msg = match e {
							ConvErr::OutOfRange => "Integer is too big/small",
							ConvErr::NotANumber => "Expected an integer",
						};
						self.push_literal(msg);
						self.error();
					}
				}
			}
		}

		#[inline]
		pub unsafe fn $get_u_default(&self, idx: c_int, default: $ty_u) -> $ty_u {
			unsafe {
				if self.is_none_or_nil(idx) {
					default
				} else {
					match self.$try_get_u(idx) {
						Ok(value) => value,
						Err(e) => {
							let msg = match e {
								ConvErr::OutOfRange => "Integer is too big/small",
								ConvErr::NotANumber => "Expected an optional integer",
							};
							self.push_literal(msg);
							self.error();
						}
					}
				}
			}
		}
	};
}

macro_rules! get_user_data_body {
	($slf:ident, $idx:ident, $ptr:ident: $ptr_ty:ty, $code:block) => {{
		// lua_touserdata() returns a valid pointer if it's a userdata or a lightuserdata.
		// lua_objlen()/lua_rawlen() return 0 if it's a lightuserdata and the size of the
		// userdata if it's a userdata.
		// The userdata is a WrappedUserData<T> and its size is never 0 since it has an `id`
		// field of type `u32`.
		// This means we don't have to check if the type is userdata, saving one call and
		// speeding up this code a little bit.
		let $ptr: $ptr_ty = $slf.to_user_data($idx);
		if !$ptr.is_null() && $slf.raw_len($idx) == mem::size_of::<WrappedUserData<T>>() {
			// Make sure it's our userdata. We can only check this after we've verified
			// the size (see above).
			// We assume that the `id` field is at offset 0.
			if *$ptr.cast::<u32>() == T::UNIQUE_ID {
				// Now that we know it's our data, we can create a ref to it
				$code
			}
		}
		$slf.push_literal("Expected a userdata");
		$slf.error();
	}};
}

/// Wraps a `lua_State`. Any references returned by it have the same lifetime as arg `L: lua_State`
#[derive(Debug, Clone, Copy)]
#[repr(transparent)]
pub struct Lua<'lua> {
	state: lua_State,
	_phantom: PhantomData<&'lua lua_State>,
}

#[repr(C)]
struct WrappedUserData<T: LuaUserData> {
	// LuaUserData::UNIQUE_ID is stored in this field so we can check if it's our userdata.
	// This field must be the first field, see get_user_data()
	id: u32,
	ud: T,
}

impl<'lua> Lua<'lua> {
	/// Creates a new instance
	#[inline]
	pub unsafe fn new(state: &'lua lua_State) -> Self {
		debug_assert!(!state.is_null());
		Self { state: *state, _phantom: PhantomData }
	}

	#[inline]
	pub unsafe fn push_user_data<T: LuaUserData>(&self, ud: T) -> &'lua mut T {
		unsafe {
			// Make sure it's not a common number, it should be 'random'
			debug_assert!(T::UNIQUE_ID > 0x1_0000);
			let ptr_ud = self.new_user_data_no_uv(mem::size_of::<WrappedUserData<T>>()).cast::<WrappedUserData<T>>();
			ptr::write(ptr_ud, WrappedUserData { id: T::UNIQUE_ID, ud });
			&mut (*ptr_ud).ud
		}
	}

	#[inline]
	pub unsafe fn get_user_data<T: LuaUserData>(&self, idx: c_int) -> &'lua T {
		unsafe {
			get_user_data_body!(self, idx, ptr: *const c_void, {
				let wrapped = &*ptr.cast::<WrappedUserData<T>>();
				return &wrapped.ud;
			})
		}
	}

	#[inline]
	pub unsafe fn get_user_data_mut<T: LuaUserData>(&self, idx: c_int) -> &'lua mut T {
		unsafe {
			get_user_data_body!(self, idx, ptr: *mut c_void, {
				let wrapped = &mut *ptr.cast::<WrappedUserData<T>>();
				return &mut wrapped.ud;
			})
		}
	}

	#[inline]
	unsafe fn new_user_data_no_uv(&self, sz: size_t) -> *mut c_void {
		#[cfg(any(feature = "lua51", feature = "lua52", feature = "lua53"))]
		unsafe {
			self.new_user_data(sz)
		}
		#[cfg(feature = "lua54")]
		unsafe {
			self.new_user_data_uv(sz, 0)
		}
	}

	unsafe fn from_c_str(p: *const c_char) -> Option<&'lua [u8]> {
		if !p.is_null() {
			unsafe {
				let len = Self::c_str_len_not_null(p);
				Some(slice::from_raw_parts(p.cast::<u8>(), len))
			}
		} else {
			None
		}
	}

	unsafe fn c_str_len_not_null(mut s: *const c_char) -> usize {
		let mut len = 0;
		loop {
			let c = unsafe { ptr::read(s) };
			if c == 0 {
				break;
			}
			len += 1;
			s = unsafe { s.add(1) };
		}
		len
	}

	#[inline]
	pub unsafe fn try_get_i64(&self, idx: c_int) -> Result<i64, ConvErr> {
		unsafe {
			let mut is_int = 0;
			let value = self.to_integer_x(idx, &mut is_int);
			if is_int != 0 {
				// into() doesn't work, so make sure `as i64` won't truncate.
				// try_into() would also work but I want a compilation error.
				const_assert!(mem::size_of::<lua_Integer>() <= mem::size_of::<i64>());
				#[allow(trivial_numeric_casts)]
				Ok(value as i64)
			} else {
				Err(ConvErr::NotANumber)
			}
		}
	}

	#[inline]
	pub unsafe fn try_get_u64(&self, idx: c_int) -> Result<u64, ConvErr> {
		unsafe { self.try_get_i64(idx).map(|v| v as u64) }
	}

	#[inline]
	pub unsafe fn try_get_f64(&self, idx: c_int) -> Result<f64, ConvErr> {
		unsafe {
			let mut is_num = 0;
			let value = self.to_number_x(idx, &mut is_num);
			if is_num != 0 {
				#[allow(dead_code)]
				type ExpectedType = f64;
				const_assert!(mem::size_of::<ExpectedType>() <= mem::size_of::<f64>());
				#[allow(clippy::useless_conversion)]
				Ok(value.into())
			} else {
				Err(ConvErr::NotANumber)
			}
		}
	}

	#[inline]
	pub unsafe fn get_i64(&self, idx: c_int) -> i64 {
		unsafe {
			match self.try_get_i64(idx) {
				Ok(value) => value,
				Err(e) => {
					let msg = match e {
						ConvErr::OutOfRange => "Integer is too big/small",
						ConvErr::NotANumber => "Expected an integer",
					};
					self.push_literal(msg);
					self.error();
				}
			}
		}
	}

	#[inline]
	pub unsafe fn get_i64_default(&self, idx: c_int, default: i64) -> i64 {
		unsafe {
			if self.is_none_or_nil(idx) {
				default
			} else {
				match self.try_get_i64(idx) {
					Ok(value) => value,
					Err(e) => {
						let msg = match e {
							ConvErr::OutOfRange => "Integer is too big/small",
							ConvErr::NotANumber => "Expected an optional integer",
						};
						self.push_literal(msg);
						self.error();
					}
				}
			}
		}
	}

	#[inline]
	pub unsafe fn get_u64(&self, idx: c_int) -> u64 {
		unsafe {
			match self.try_get_u64(idx) {
				Ok(value) => value,
				Err(e) => {
					let msg = match e {
						ConvErr::OutOfRange => "Integer is too big/small",
						ConvErr::NotANumber => "Expected an integer",
					};
					self.push_literal(msg);
					self.error();
				}
			}
		}
	}

	#[inline]
	pub unsafe fn get_u64_default(&self, idx: c_int, default: u64) -> u64 {
		unsafe {
			if self.is_none_or_nil(idx) {
				default
			} else {
				match self.try_get_u64(idx) {
					Ok(value) => value,
					Err(e) => {
						let msg = match e {
							ConvErr::OutOfRange => "Integer is too big/small",
							ConvErr::NotANumber => "Expected an optional integer",
						};
						self.push_literal(msg);
						self.error();
					}
				}
			}
		}
	}

	#[inline]
	pub unsafe fn try_get_usize(&self, idx: c_int) -> Result<usize, ConvErr> {
		#[cfg(target_pointer_width = "32")]
		unsafe {
			const_assert!(mem::size_of::<usize>() == 4);
			self.try_get_u32(idx).map(|v| v as usize)
		}
		#[cfg(target_pointer_width = "64")]
		unsafe {
			const_assert!(mem::size_of::<usize>() == 8);
			self.try_get_u64(idx).map(|v| v as usize)
		}
	}

	#[inline]
	pub unsafe fn get_usize(&self, idx: c_int) -> usize {
		#[cfg(target_pointer_width = "32")]
		unsafe {
			const_assert!(mem::size_of::<usize>() == 4);
			self.get_u32(idx) as usize
		}
		#[cfg(target_pointer_width = "64")]
		unsafe {
			const_assert!(mem::size_of::<usize>() == 8);
			self.get_u64(idx) as usize
		}
	}

	#[inline]
	pub unsafe fn get_usize_default(&self, idx: c_int, default: usize) -> usize {
		#[cfg(target_pointer_width = "32")]
		unsafe {
			const_assert!(mem::size_of::<usize>() == 4);
			self.get_u32_default(idx, default as u32) as usize
		}
		#[cfg(target_pointer_width = "64")]
		unsafe {
			const_assert!(mem::size_of::<usize>() == 8);
			self.get_u64_default(idx, default as u64) as usize
		}
	}

	#[inline]
	pub unsafe fn try_get_isize(&self, idx: c_int) -> Result<isize, ConvErr> {
		#[cfg(target_pointer_width = "32")]
		unsafe {
			const_assert!(mem::size_of::<isize>() == 4);
			self.try_get_i32(idx).map(|v| v as isize)
		}
		#[cfg(target_pointer_width = "64")]
		unsafe {
			const_assert!(mem::size_of::<isize>() == 8);
			self.try_get_i64(idx).map(|v| v as isize)
		}
	}

	#[inline]
	pub unsafe fn get_isize(&self, idx: c_int) -> isize {
		#[cfg(target_pointer_width = "32")]
		unsafe {
			const_assert!(mem::size_of::<isize>() == 4);
			self.get_i32(idx) as isize
		}
		#[cfg(target_pointer_width = "64")]
		unsafe {
			const_assert!(mem::size_of::<isize>() == 8);
			self.get_i64(idx) as isize
		}
	}

	#[inline]
	pub unsafe fn get_isize_default(&self, idx: c_int, default: isize) -> isize {
		#[cfg(target_pointer_width = "32")]
		unsafe {
			const_assert!(mem::size_of::<isize>() == 4);
			self.get_i32_default(idx, default as i32) as isize
		}
		#[cfg(target_pointer_width = "64")]
		unsafe {
			const_assert!(mem::size_of::<isize>() == 8);
			self.get_i64_default(idx, default as i64) as isize
		}
	}

	gen_get_signed_int! {try_get_i32, get_i32, get_i32_default, i32, u32}
	gen_get_signed_int! {try_get_i16, get_i16, get_i16_default, i16, u16}
	gen_get_signed_int! {try_get_i8, get_i8, get_i8_default, i8, u8}

	gen_get_unsigned_int! {try_get_u32, get_u32, get_u32_default, i32, u32}
	gen_get_unsigned_int! {try_get_u16, get_u16, get_u16_default, i16, u16}
	gen_get_unsigned_int! {try_get_u8, get_u8, get_u8_default, i8, u8}

	#[inline]
	pub unsafe fn get_f64(&self, idx: c_int) -> f64 {
		unsafe {
			match self.try_get_f64(idx) {
				Ok(value) => value,
				Err(_) => {
					self.push_literal("Expected a number");
					self.error();
				}
			}
		}
	}

	#[inline]
	pub unsafe fn get_f64_default(&self, idx: c_int, default: f64) -> f64 {
		unsafe {
			if self.is_none_or_nil(idx) {
				default
			} else {
				match self.try_get_f64(idx) {
					Ok(value) => value,
					Err(_) => {
						self.push_literal("Expected an optional number");
						self.error();
					}
				}
			}
		}
	}

	#[inline]
	pub unsafe fn get_f32(&self, idx: c_int) -> f32 {
		unsafe { self.get_f64(idx) as f32 }
	}

	#[inline]
	pub unsafe fn get_f32_default(&self, idx: c_int, default: f32) -> f32 {
		unsafe { self.get_f64_default(idx, default as f64) as f32 }
	}

	#[inline]
	pub unsafe fn try_get_bool(&self, idx: c_int) -> Option<bool> {
		unsafe {
			// We only allow booleans. self.to_boolean() can be called if you want to convert any
			// value to a boolean.
			if self.type_(idx) == LUA_TBOOLEAN {
				Some(self.to_boolean(idx))
			} else {
				None
			}
		}
	}

	#[inline]
	pub unsafe fn get_bool(&self, idx: c_int) -> bool {
		unsafe {
			if let Some(value) = self.try_get_bool(idx) {
				value
			} else {
				self.push_literal("Expected a boolean");
				self.error();
			}
		}
	}

	#[inline]
	pub unsafe fn get_bool_default(&self, idx: c_int, default: bool) -> bool {
		unsafe { self.try_get_bool(idx).unwrap_or(default) }
	}

	#[inline]
	pub unsafe fn try_get_string(&self, idx: c_int) -> Option<&'lua [u8]> {
		unsafe { self.to_l_string(idx) }
	}

	#[inline]
	pub unsafe fn get_string(&self, idx: c_int) -> &'lua [u8] {
		unsafe {
			if let Some(value) = self.try_get_string(idx) {
				value
			} else {
				self.push_literal("Expected a string");
				self.error();
			}
		}
	}

	#[inline]
	pub unsafe fn throw_error<E: Error>(&self, e: E) -> ! {
		unsafe {
			let msg = e.to_string();
			self.push_literal(&msg);
			self.error();
		}
	}
}

// Simple wrappers calling the Lua C API
impl<'lua> Lua<'lua> {
	#[inline]
	pub unsafe fn up_value_index(i: c_int) -> c_int {
		unsafe { lua_upvalueindex(i) }
	}

	#[inline]
	#[cfg(feature = "lua54")]
	pub unsafe fn reset_thread(&self) -> c_int {
		unsafe { lua_resetthread(self.state) }
	}

	#[inline]
	pub unsafe fn new_thread(&self) -> lua_State {
		unsafe { lua_newthread(self.state) }
	}

	#[inline]
	pub unsafe fn at_panic(&self, panicf: lua_CFunction) -> lua_CFunction {
		unsafe { lua_atpanic(self.state, panicf) }
	}

	#[inline]
	#[cfg(any(feature = "lua52", feature = "lua53", feature = "lua54"))]
	pub unsafe fn abs_index(&self, idx: c_int) -> c_int {
		unsafe { lua_absindex(self.state, idx) }
	}

	#[inline]
	pub unsafe fn get_top(&self) -> c_int {
		unsafe { lua_gettop(self.state) }
	}

	#[inline]
	pub unsafe fn set_top(&self, idx: c_int) {
		unsafe {
			lua_settop(self.state, idx);
		}
	}

	#[inline]
	pub unsafe fn push_value(&self, idx: c_int) {
		unsafe {
			lua_pushvalue(self.state, idx);
		}
	}

	#[inline]
	pub unsafe fn remove(&self, idx: c_int) {
		unsafe {
			lua_remove(self.state, idx);
		}
	}

	#[inline]
	pub unsafe fn insert(&self, idx: c_int) {
		unsafe {
			lua_insert(self.state, idx);
		}
	}

	#[inline]
	pub unsafe fn replace(&self, idx: c_int) {
		unsafe {
			lua_replace(self.state, idx);
		}
	}

	#[inline]
	#[cfg(any(feature = "lua53", feature = "lua54"))]
	pub unsafe fn rotate(&self, idx: c_int, n: c_int) {
		unsafe {
			lua_rotate(self.state, idx, n);
		}
	}

	#[inline]
	#[cfg(any(feature = "lua52", feature = "lua53", feature = "lua54"))]
	pub unsafe fn copy(&self, fromidx: c_int, toidx: c_int) {
		unsafe {
			lua_copy(self.state, fromidx, toidx);
		}
	}

	#[inline]
	pub unsafe fn check_stack(&self, sz: c_int) -> c_int {
		unsafe { lua_checkstack(self.state, sz) }
	}

	#[inline]
	pub unsafe fn is_number(&self, idx: c_int) -> bool {
		unsafe { lua_isnumber(self.state, idx) != 0 }
	}

	#[inline]
	pub unsafe fn is_string(&self, idx: c_int) -> bool {
		unsafe { lua_isstring(self.state, idx) != 0 }
	}

	#[inline]
	pub unsafe fn is_c_function(&self, idx: c_int) -> bool {
		unsafe { lua_iscfunction(self.state, idx) != 0 }
	}

	#[inline]
	#[cfg(any(feature = "lua53", feature = "lua54"))]
	pub unsafe fn is_integer(&self, idx: c_int) -> bool {
		unsafe { lua_isinteger(self.state, idx) != 0 }
	}

	#[inline]
	pub unsafe fn is_user_data(&self, idx: c_int) -> bool {
		unsafe { lua_isuserdata(self.state, idx) != 0 }
	}

	#[inline]
	pub unsafe fn type_(&self, idx: c_int) -> c_int {
		unsafe { lua_type(self.state, idx) }
	}

	#[inline]
	pub unsafe fn type_name(&self, tp: c_int) -> &'lua [u8] {
		unsafe { Self::from_c_str(lua_typename(self.state, tp)).unwrap_or_default() }
	}

	#[inline]
	pub unsafe fn equal(&self, idx1: c_int, idx2: c_int) -> c_int {
		unsafe { lua_equal(self.state, idx1, idx2) }
	}

	#[inline]
	pub unsafe fn raw_equal(&self, idx1: c_int, idx2: c_int) -> c_int {
		unsafe { lua_rawequal(self.state, idx1, idx2) }
	}

	#[inline]
	pub unsafe fn less_than(&self, idx1: c_int, idx2: c_int) -> c_int {
		unsafe { lua_lessthan(self.state, idx1, idx2) }
	}

	#[inline]
	#[cfg(any(feature = "lua52", feature = "lua53", feature = "lua54"))]
	pub unsafe fn compare(&self, idx1: c_int, idx2: c_int, op: c_int) -> c_int {
		unsafe { lua_compare(self.state, idx1, idx2, op) }
	}

	#[inline]
	pub unsafe fn to_number(&self, idx: c_int) -> lua_Number {
		unsafe { lua_tonumber(self.state, idx) }
	}

	#[inline]
	pub unsafe fn to_integer(&self, idx: c_int) -> lua_Integer {
		unsafe { lua_tointeger(self.state, idx) }
	}

	#[inline]
	#[cfg(any(feature = "lua52", feature = "lua53", feature = "lua54"))]
	pub unsafe fn to_unsigned(&self, idx: c_int) -> lua_Unsigned {
		unsafe { lua_tounsigned(self.state, idx) }
	}

	#[inline]
	pub unsafe fn to_number_x(&self, idx: c_int, isnum: *mut c_int) -> lua_Number {
		#[cfg(feature = "lua51")]
		unsafe {
			let value = self.to_number(idx);
			if !isnum.is_null() {
				*isnum = (value != lua_Number::default() || self.is_number(idx)) as c_int;
			}
			value
		}
		#[cfg(any(feature = "lua52", feature = "lua53", feature = "lua54"))]
		unsafe {
			lua_tonumberx(self.state, idx, isnum)
		}
	}

	#[inline]
	pub unsafe fn to_integer_x(&self, idx: c_int, isnum: *mut c_int) -> lua_Integer {
		#[cfg(feature = "lua51")]
		unsafe {
			let value = self.to_integer(idx);
			if !isnum.is_null() {
				*isnum = (value != lua_Integer::default() || self.is_number(idx)) as c_int;
			}
			value
		}
		#[cfg(any(feature = "lua52", feature = "lua53", feature = "lua54"))]
		unsafe {
			lua_tointegerx(self.state, idx, isnum)
		}
	}

	#[inline]
	#[cfg(any(feature = "lua52", feature = "lua53", feature = "lua54"))]
	pub unsafe fn to_unsigned_x(&self, idx: c_int, isnum: *mut c_int) -> lua_Unsigned {
		unsafe { lua_tounsignedx(self.state, idx, isnum) }
	}

	#[inline]
	pub unsafe fn to_boolean(&self, idx: c_int) -> bool {
		unsafe { lua_toboolean(self.state, idx) != 0 }
	}

	#[inline]
	pub unsafe fn to_l_string(&self, idx: c_int) -> Option<&'lua [u8]> {
		unsafe {
			let mut len = 0;
			let data = lua_tolstring(self.state, idx, &mut len);
			if !data.is_null() {
				Some(slice::from_raw_parts(data.cast(), len))
			} else {
				None
			}
		}
	}

	#[inline]
	pub unsafe fn obj_len(&self, idx: c_int) -> size_t {
		unsafe { lua_objlen(self.state, idx) }
	}

	#[inline]
	pub unsafe fn raw_len(&self, idx: c_int) -> size_t {
		#[cfg(feature = "lua51")]
		unsafe {
			lua_objlen(self.state, idx)
		}
		#[cfg(any(feature = "lua52", feature = "lua53", feature = "lua54"))]
		#[allow(trivial_numeric_casts)]
		unsafe {
			lua_rawlen(self.state, idx) as size_t
		}
	}

	#[inline]
	pub unsafe fn to_c_function(&self, idx: c_int) -> lua_CFunction {
		unsafe { lua_tocfunction(self.state, idx) }
	}

	#[inline]
	pub unsafe fn to_user_data(&self, idx: c_int) -> *mut c_void {
		unsafe { lua_touserdata(self.state, idx) }
	}

	#[inline]
	pub unsafe fn to_thread(&self, idx: c_int) -> lua_State {
		unsafe { lua_tothread(self.state, idx) }
	}

	#[inline]
	pub unsafe fn to_pointer(&self, idx: c_int) -> *const c_void {
		unsafe { lua_topointer(self.state, idx) }
	}

	#[inline]
	#[cfg(any(feature = "lua52", feature = "lua53", feature = "lua54"))]
	pub unsafe fn arith(&self, op: c_int) {
		unsafe {
			lua_arith(self.state, op);
		}
	}

	#[inline]
	pub unsafe fn push_nil(&self) {
		unsafe {
			lua_pushnil(self.state);
		}
	}

	#[inline]
	pub unsafe fn push_number(&self, n: lua_Number) {
		unsafe {
			lua_pushnumber(self.state, n);
		}
	}

	#[inline]
	pub unsafe fn push_integer(&self, n: lua_Integer) {
		unsafe {
			lua_pushinteger(self.state, n);
		}
	}

	#[inline]
	#[cfg(any(feature = "lua52", feature = "lua53", feature = "lua54"))]
	pub unsafe fn push_unsigned(&self, n: lua_Unsigned) {
		unsafe {
			lua_pushunsigned(self.state, n);
		}
	}

	#[inline]
	pub unsafe fn push_bytes(&self, s: &'_ [u8]) {
		unsafe {
			let _ = lua_pushlstring(self.state, s.as_ptr().cast(), s.len());
		}
	}

	#[inline]
	pub unsafe fn push_string(&self, s: &'_ str) {
		unsafe {
			let _ = lua_pushlstring(self.state, s.as_ptr().cast(), s.len());
		}
	}

	#[inline]
	pub unsafe fn push_c_closure(&self, f: lua_CFunction, n: c_int) {
		unsafe {
			lua_pushcclosure(self.state, f, n);
		}
	}

	#[inline]
	pub unsafe fn push_boolean(&self, b: c_int) {
		unsafe {
			lua_pushboolean(self.state, b);
		}
	}

	#[inline]
	pub unsafe fn push_light_user_data(&self, p: *const c_void) {
		unsafe {
			lua_pushlightuserdata(self.state, p);
		}
	}

	#[inline]
	pub unsafe fn push_global_table(&self) {
		#[cfg(feature = "lua51")]
		unsafe {
			self.push_value(LUA_GLOBALSINDEX)
		}
		#[cfg(any(feature = "lua52", feature = "lua53", feature = "lua54"))]
		unsafe {
			lua_pushglobaltable(self.state);
		}
	}

	#[inline]
	pub unsafe fn push_thread(&self) -> c_int {
		unsafe { lua_pushthread(self.state) }
	}

	#[inline]
	pub unsafe fn get_table(&self, idx: c_int) {
		unsafe {
			let _ = lua_gettable(self.state, idx);
		}
	}

	#[inline]
	pub unsafe fn get_field(&self, idx: c_int, k: LuaCStr<'_>) {
		unsafe {
			let _ = lua_getfield(self.state, idx, k.0.as_ptr().cast());
		}
	}

	#[inline]
	pub unsafe fn raw_get(&self, idx: c_int) {
		unsafe {
			let _ = lua_rawget(self.state, idx);
		}
	}

	#[inline]
	pub unsafe fn raw_get_i(&self, idx: c_int, n: lua_GetIType) {
		unsafe {
			let _ = lua_rawgeti(self.state, idx, n);
		}
	}

	#[inline]
	#[cfg(any(feature = "lua52", feature = "lua53", feature = "lua54"))]
	pub unsafe fn raw_get_p(&self, idx: c_int, p: *const c_void) {
		unsafe {
			let _ = lua_rawgetp(self.state, idx, p);
		}
	}

	#[inline]
	pub unsafe fn create_table(&self, narr: c_int, nrec: c_int) {
		unsafe {
			lua_createtable(self.state, narr, nrec);
		}
	}

	#[inline]
	pub unsafe fn new_user_data(&self, sz: size_t) -> *mut c_void {
		unsafe { lua_newuserdata(self.state, sz) }
	}

	#[inline]
	#[cfg(feature = "lua54")]
	pub unsafe fn new_user_data_uv(&self, sz: size_t, nuvalue: c_int) -> *mut c_void {
		unsafe { lua_newuserdatauv(self.state, sz, nuvalue) }
	}

	#[inline]
	pub unsafe fn get_metatable(&self, objindex: c_int) -> c_int {
		unsafe { lua_getmetatable(self.state, objindex) }
	}

	#[inline]
	#[cfg(feature = "lua51")]
	pub unsafe fn get_f_env(&self, idx: c_int) {
		unsafe {
			lua_getfenv(self.state, idx);
		}
	}

	#[inline]
	#[cfg(any(feature = "lua52", feature = "lua53", feature = "lua54"))]
	pub unsafe fn get_user_value(&self, idx: c_int) {
		unsafe {
			let _ = lua_getuservalue(self.state, idx);
		}
	}

	#[inline]
	#[cfg(feature = "lua54")]
	pub unsafe fn get_i_user_value(&self, idx: c_int, n: c_int) -> c_int {
		unsafe { lua_getiuservalue(self.state, idx, n) }
	}

	#[inline]
	pub unsafe fn set_table(&self, idx: c_int) {
		unsafe {
			lua_settable(self.state, idx);
		}
	}

	#[inline]
	pub unsafe fn set_field(&self, idx: c_int, k: LuaCStr<'_>) {
		unsafe {
			lua_setfield(self.state, idx, k.0.as_ptr().cast());
		}
	}

	#[inline]
	#[cfg(any(feature = "lua53", feature = "lua54"))]
	pub unsafe fn set_i(&self, idx: c_int, n: lua_GetIType) {
		unsafe {
			lua_seti(self.state, idx, n);
		}
	}

	#[inline]
	pub unsafe fn raw_set(&self, idx: c_int) {
		unsafe {
			lua_rawset(self.state, idx);
		}
	}

	#[inline]
	pub unsafe fn raw_set_i(&self, idx: c_int, n: lua_GetIType) {
		unsafe {
			lua_rawseti(self.state, idx, n);
		}
	}

	#[inline]
	#[cfg(any(feature = "lua52", feature = "lua53", feature = "lua54"))]
	pub unsafe fn raw_set_p(&self, idx: c_int, p: *const c_void) {
		unsafe {
			lua_rawsetp(self.state, idx, p);
		}
	}

	#[inline]
	pub unsafe fn set_metatable(&self, objindex: c_int) -> c_int {
		unsafe { lua_setmetatable(self.state, objindex) }
	}

	#[inline]
	#[cfg(feature = "lua51")]
	pub unsafe fn set_f_env(&self, idx: c_int) -> c_int {
		unsafe { lua_setfenv(self.state, idx) }
	}

	#[inline]
	#[cfg(any(feature = "lua52", feature = "lua53", feature = "lua54"))]
	pub unsafe fn set_user_value(&self, idx: c_int) {
		unsafe {
			lua_setuservalue(self.state, idx);
		}
	}

	#[inline]
	#[cfg(feature = "lua54")]
	pub unsafe fn set_i_user_value(&self, idx: c_int, n: c_int) -> c_int {
		unsafe { lua_setiuservalue(self.state, idx, n) }
	}

	#[inline]
	pub unsafe fn call(&self, nargs: c_int, nresults: c_int) {
		unsafe {
			lua_call(self.state, nargs, nresults);
		}
	}

	#[inline]
	pub unsafe fn pcall(&self, nargs: c_int, nresults: c_int, errfunc: c_int) -> c_int {
		unsafe { lua_pcall(self.state, nargs, nresults, errfunc) }
	}

	#[inline]
	pub unsafe fn cpcall(&self, func: lua_CFunction, ud: *mut c_void) -> c_int {
		#[cfg(feature = "lua51")]
		unsafe {
			lua_cpcall(self.state, func, ud)
		}
		#[cfg(any(feature = "lua52", feature = "lua53", feature = "lua54"))]
		unsafe {
			self.push_c_function(func);
			self.push_light_user_data(ud);
			self.pcall(1, 0, 0)
		}
	}

	#[inline]
	#[cfg(feature = "lua52")]
	pub unsafe fn callk(&self, nargs: c_int, nresults: c_int, ctx: c_int, k: lua_CFunction) {
		unsafe {
			lua_callk(self.state, nargs, nresults, ctx, Some(k));
		}
	}

	#[inline]
	#[cfg(any(feature = "lua53", feature = "lua54"))]
	pub unsafe fn callk(&self, nargs: c_int, nresults: c_int, ctx: lua_KContext, k: lua_KFunction) {
		unsafe {
			lua_callk(self.state, nargs, nresults, ctx, Some(k));
		}
	}

	#[inline]
	#[cfg(feature = "lua52")]
	pub unsafe fn pcallk(&self, nargs: c_int, nresults: c_int, errfunc: c_int, ctx: c_int, k: lua_CFunction) -> c_int {
		unsafe { lua_pcallk(self.state, nargs, nresults, errfunc, ctx, Some(k)) }
	}

	#[inline]
	#[cfg(any(feature = "lua53", feature = "lua54"))]
	pub unsafe fn pcallk(&self, nargs: c_int, nresults: c_int, errfunc: c_int, ctx: lua_KContext, k: lua_KFunction) -> c_int {
		unsafe { lua_pcallk(self.state, nargs, nresults, errfunc, ctx, Some(k)) }
	}

	#[inline]
	#[cfg(feature = "lua52")]
	pub unsafe fn get_ctx(&self, ctx: &mut c_int) -> c_int {
		unsafe { lua_getctx(self.state, ctx) }
	}

	#[inline]
	pub unsafe fn load(&self, reader: lua_Reader, dt: *mut c_void, chunkname: LuaCStr<'_>) -> c_int {
		#[cfg(feature = "lua51")]
		unsafe {
			lua_load(self.state, reader, dt, chunkname.0.as_ptr().cast())
		}
		#[cfg(any(feature = "lua52", feature = "lua53", feature = "lua54"))]
		unsafe {
			lua_load(self.state, reader, dt, chunkname.0.as_ptr().cast(), ptr::null())
		}
	}

	#[inline]
	#[cfg(any(feature = "lua52", feature = "lua53", feature = "lua54"))]
	pub unsafe fn load4(&self, reader: lua_Reader, dt: *mut c_void, chunkname: LuaCStr<'_>, mode: LuaCStr<'_>) -> c_int {
		unsafe { lua_load(self.state, reader, dt, chunkname.0.as_ptr().cast(), mode.0.as_ptr().cast()) }
	}

	#[inline]
	pub unsafe fn dump(&self, writer: lua_Writer, data: *mut c_void) -> c_int {
		#[cfg(any(feature = "lua51", feature = "lua52"))]
		unsafe {
			lua_dump(self.state, writer, data)
		}
		#[cfg(any(feature = "lua53", feature = "lua54"))]
		unsafe {
			lua_dump(self.state, writer, data, 0)
		}
	}

	#[inline]
	#[cfg(any(feature = "lua53", feature = "lua54"))]
	pub unsafe fn dump3(&self, writer: lua_Writer, data: *mut c_void, strip: c_int) -> c_int {
		unsafe { lua_dump(self.state, writer, data, strip) }
	}

	#[inline]
	pub unsafe fn yield_(&self, nresults: c_int) -> c_int {
		unsafe { lua_yield(self.state, nresults) }
	}

	#[inline]
	#[cfg(feature = "lua52")]
	pub unsafe fn yieldk(&self, nresults: c_int, ctx: c_int, k: lua_CFunction) -> c_int {
		unsafe { lua_yieldk(self.state, nresults, ctx, Some(k)) }
	}

	#[inline]
	#[cfg(any(feature = "lua53", feature = "lua54"))]
	pub unsafe fn yieldk(&self, nresults: c_int, ctx: lua_KContext, k: lua_KFunction) -> c_int {
		unsafe { lua_yieldk(self.state, nresults, ctx, Some(k)) }
	}

	#[inline]
	pub unsafe fn resume(&self, narg: c_int) -> c_int {
		#[cfg(feature = "lua51")]
		unsafe {
			lua_resume(self.state, narg)
		}
		#[cfg(any(feature = "lua52", feature = "lua53"))]
		unsafe {
			lua_resume(self.state, ptr::null_mut(), narg)
		}
		#[cfg(feature = "lua54")]
		unsafe {
			let mut nres = 0;
			lua_resume(self.state, ptr::null_mut(), narg, &mut nres)
		}
	}

	#[inline]
	#[cfg(any(feature = "lua52", feature = "lua53", feature = "lua54"))]
	pub unsafe fn resume2(&self, from: lua_State, narg: c_int) -> c_int {
		#[cfg(any(feature = "lua52", feature = "lua53"))]
		unsafe {
			lua_resume(self.state, from, narg)
		}
		#[cfg(feature = "lua54")]
		unsafe {
			let mut nres = 0;
			lua_resume(self.state, from, narg, &mut nres)
		}
	}

	#[inline]
	#[cfg(feature = "lua54")]
	pub unsafe fn resume3(&self, from: lua_State, narg: c_int, nres: &mut c_int) -> c_int {
		unsafe { lua_resume(self.state, from, narg, nres) }
	}

	#[inline]
	pub unsafe fn status(&self) -> c_int {
		unsafe { lua_status(self.state) }
	}

	#[inline]
	#[cfg(any(feature = "lua53", feature = "lua54"))]
	pub unsafe fn is_yieldable(&self) -> bool {
		unsafe { lua_isyieldable(self.state) != 0 }
	}

	#[inline]
	#[cfg(feature = "lua54")]
	pub unsafe fn set_warn_f(&self, f: lua_WarnFunction, ud: *mut c_void) {
		unsafe {
			lua_setwarnf(self.state, f, ud);
		}
	}

	#[inline]
	#[cfg(feature = "lua54")]
	pub unsafe fn warning(&self, msg: LuaCStr<'_>, tocont: c_int) {
		unsafe {
			lua_warning(self.state, msg.0.as_ptr().cast(), tocont);
		}
	}

	#[inline]
	pub unsafe fn gc(&self, what: c_int, data: c_int) -> c_int {
		unsafe { lua_gc(self.state, what, data) }
	}

	#[inline]
	pub unsafe fn error(&self) -> ! {
		unsafe { lua_error(self.state) }
	}

	#[inline]
	pub unsafe fn next(&self, idx: c_int) -> c_int {
		unsafe { lua_next(self.state, idx) }
	}

	#[inline]
	pub unsafe fn concat(&self, n: c_int) {
		unsafe {
			lua_concat(self.state, n);
		}
	}

	#[inline]
	#[cfg(any(feature = "lua52", feature = "lua53", feature = "lua54"))]
	pub unsafe fn len(&self, idx: c_int) {
		unsafe {
			lua_len(self.state, idx);
		}
	}

	#[inline]
	#[cfg(any(feature = "lua53", feature = "lua54"))]
	pub unsafe fn string_to_number(&self, s: LuaCStr<'_>) -> bool {
		unsafe { lua_stringtonumber(self.state, s.0.as_ptr().cast()) != 0 }
	}

	#[inline]
	#[cfg(feature = "lua54")]
	pub unsafe fn to_close(&self, idx: c_int) {
		unsafe {
			lua_toclose(self.state, idx);
		}
	}

	#[inline]
	#[cfg(feature = "lua54")]
	pub unsafe fn close_slot(&self, idx: c_int) {
		unsafe {
			lua_closeslot(self.state, idx);
		}
	}

	#[inline]
	#[cfg(any(feature = "lua53", feature = "lua54"))]
	pub unsafe fn get_extra_space(&self) -> *mut c_void {
		unsafe { lua_getextraspace(self.state) }
	}

	#[inline]
	pub unsafe fn pop(&self, n: c_int) {
		unsafe {
			lua_pop(self.state, n);
		}
	}

	#[inline]
	pub unsafe fn new_table(&self) {
		unsafe {
			lua_newtable(self.state);
		}
	}

	#[inline]
	pub unsafe fn register(&self, n: LuaCStr<'_>, f: lua_CFunction) {
		unsafe {
			lua_register(self.state, n.0.as_ptr().cast(), f);
		}
	}

	#[inline]
	pub unsafe fn push_c_function(&self, f: lua_CFunction) {
		unsafe {
			lua_pushcfunction(self.state, f);
		}
	}

	#[inline]
	pub unsafe fn strlen(&self, i: c_int) -> size_t {
		unsafe { lua_strlen(self.state, i) }
	}

	#[inline]
	pub unsafe fn is_function(&self, n: c_int) -> bool {
		unsafe { lua_isfunction(self.state, n) }
	}

	#[inline]
	pub unsafe fn is_table(&self, n: c_int) -> bool {
		unsafe { lua_istable(self.state, n) }
	}

	#[inline]
	pub unsafe fn is_light_user_data(&self, n: c_int) -> bool {
		unsafe { lua_islightuserdata(self.state, n) }
	}

	#[inline]
	pub unsafe fn is_nil(&self, n: c_int) -> bool {
		unsafe { lua_isnil(self.state, n) }
	}

	#[inline]
	pub unsafe fn is_boolean(&self, n: c_int) -> bool {
		unsafe { lua_isboolean(self.state, n) }
	}

	#[inline]
	pub unsafe fn is_thread(&self, n: c_int) -> bool {
		unsafe { lua_isthread(self.state, n) }
	}

	#[inline]
	pub unsafe fn is_none(&self, n: c_int) -> bool {
		unsafe { lua_isnone(self.state, n) }
	}

	#[inline]
	pub unsafe fn is_none_or_nil(&self, n: c_int) -> bool {
		unsafe { lua_isnoneornil(self.state, n) }
	}

	#[inline]
	pub unsafe fn push_literal(&self, s: &str) {
		unsafe {
			lua_pushliteral(self.state, s);
		}
	}

	#[inline]
	pub unsafe fn set_global(&self, s: LuaCStr<'_>) {
		unsafe {
			lua_setglobal(self.state, s.0.as_ptr().cast());
		}
	}

	#[inline]
	pub unsafe fn get_global(&self, s: LuaCStr<'_>) {
		unsafe {
			let _ = lua_getglobal(self.state, s.0.as_ptr().cast());
		}
	}

	#[inline]
	pub unsafe fn to_string(&self, i: c_int) -> Option<&'lua [u8]> {
		unsafe { Self::from_c_str(lua_tostring(self.state, i)) }
	}

	#[inline]
	#[cfg(feature = "lua51")]
	pub unsafe fn get_registry(&self) {
		unsafe {
			lua_getregistry(self.state);
		}
	}

	#[inline]
	#[cfg(feature = "lua51")]
	pub unsafe fn get_gc_count(&self) -> c_int {
		unsafe { lua_getgccount(self.state) }
	}

	#[inline]
	pub unsafe fn get_registry_metatable(&self, name: LuaCStr<'_>) {
		unsafe {
			luaL_getmetatable(self.state, name.0.as_ptr().cast());
		}
	}

	#[inline]
	pub unsafe fn new_registry_metatable(&self, name: LuaCStr<'_>) -> bool {
		unsafe { luaL_newmetatable(self.state, name.0.as_ptr().cast()) != 0 }
	}
}
