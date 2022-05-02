// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

// The reason we don't use methods attached to userdata is that calling functions stored in the
// module table is faster than methods attached to userdata. ~170ms vs ~220ms (1,000,000 calls).
// (mlua 0.8.0-beta.4)

macro_rules! lua_ctor {
	($lua:ident, $exports:ident = fn $name:ident($($arg:ident : $arg_ty:ty,)*) $code:block) => {
		$exports.raw_set(
			stringify!($name),
			$lua.create_function(|_, ($($arg,)*): ($($arg_ty,)*)| {
				$code
			})?,
		)?;
	};
}

macro_rules! lua_method {
	($lua:ident, $exports:ident = fn $name:ident($this:ident : $this_ty:ty, $($arg:ident : $arg_ty:ty,)*) $code:block) => {
		$exports.raw_set(
			stringify!($name),
			$lua.create_function(|_, ($this, $($arg,)*): (LuaAnyUserData<'_>, $($arg_ty,)*)| {
				let $this = &*$this.borrow::<$this_ty>()?;
				$code
			})?,
		)?;
	};
}

macro_rules! lua_method_mut {
	($lua:ident, $exports:ident = fn $name:ident($this:ident : $this_ty:ty, $($arg:ident : $arg_ty:ty,)*) $code:block) => {
		$exports.raw_set(
			stringify!($name),
			$lua.create_function(|_, ($this, $($arg,)*): (LuaAnyUserData<'_>, $($arg_ty,)*)| {
				let $this = &mut *$this.borrow_mut::<$this_ty>()?;
				$code
			})?,
		)?;
	};
}

macro_rules! lua_getter {
	($lua:ident, $exports:ident = fn $name:ident($this:ident : $this_ty:ty) $code:block) => {
		lua_method!($lua, $exports = fn $name($this: $this_ty,) $code)
	};
}

macro_rules! lua_setter {
	($lua:ident, $exports:ident = fn $name:ident($this:ident : $this_ty:ty, $arg:ident : $arg_ty:ty) $code:block) => {
		lua_method_mut!($lua, $exports = fn $name($this: $this_ty, $arg: $arg_ty,) $code)
	};
}
