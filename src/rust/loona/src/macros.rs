// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#[macro_export]
macro_rules! cstr {
	($s:expr) => {{
		const STR: &'static str = $s;
		$crate::lua::LuaCStr(concat!($s, "\0").as_bytes())
	}};
}

#[macro_export]
macro_rules! lua_module {
	($(#[$attr:meta])* unsafe fn $modname:ident($lua:ident) $block:block) => {
		$(#[$attr])*
		#[no_mangle]
		unsafe extern "C" fn $modname(L: $crate::lua_api::lua_State) -> $crate::libc::c_int {
			const RET_VALS: $crate::libc::c_int = 1;
			let $lua = unsafe { $crate::lua::Lua::new(&L) };
			let $lua = &$lua;

			#[cfg(any(debug_assertions, feature = "extra_checks"))]
			let _orig_top = unsafe { $lua.get_top() };

			$block

			#[cfg(any(debug_assertions, feature = "extra_checks"))]
			unsafe { assert_eq!(RET_VALS, $lua.get_top().wrapping_sub(_orig_top)); }

			RET_VALS
		}
	};
}

#[macro_export]
macro_rules! lua_struct_module {
	($mod_name:ident : $class:ident) => {
		$crate::lua_module! {unsafe fn $mod_name(lua) {
			unsafe {
				$crate::lua_get_or_init_metatable!($class : lua);
				lua.push("__index");
				lua.raw_get(-2);
				lua.replace(-2); // replace metatable with metatable.__index
			}
		}}
	};
}

#[macro_export]
macro_rules! lua_get_or_init_metatable {
	($class:ty : $lua:ident) => {
		if $lua.new_registry_metatable(<$class>::METATABLE_KEY) {
			<$class>::init_metatable($lua);
		}
	};
}

#[macro_export]
macro_rules! lua_methods {
	($($(#[$attr:meta])* unsafe fn $method_name:ident($lua:ident $(, $arg:ident:$arg_ty:ty)*) -> $ret_vals:literal $block:block)*) => {
		$(
			$(#[$attr])*
			#[allow(non_snake_case)]
			unsafe extern "C" fn $method_name(L: $crate::lua_api::lua_State) -> $crate::libc::c_int {
				let $lua = unsafe { $crate::lua::Lua::new(&L) };
				let $lua = &$lua;

				#[cfg(any(debug_assertions, feature = "extra_checks"))]
				let _orig_top = unsafe { $lua.get_top() };

				// Unfortunately consts don't work, eg. const A: c_int = 1; const A: c_int = A + 1; etc.
				let _lua_index: $crate::libc::c_int = 1;
				$(
					let $arg: <$arg_ty as $crate::tofrom::FromLua<'_>>::RetType = <$arg_ty as $crate::tofrom::FromLua<'_>>::from_lua($lua, _lua_index);
					let _lua_index: $crate::libc::c_int = _lua_index + 1;
				)*

				const _: () = assert!($ret_vals >= 0);
				if $ret_vals > $crate::lua_api::LUA_MINSTACK {
					if !$lua.check_stack($ret_vals as $crate::libc::c_int) {
						$lua.throw_error_msg("Couldn't grow the stack");
					}
				}
				$block

				#[cfg(any(debug_assertions, feature = "extra_checks"))]
				unsafe { assert_eq!($ret_vals, $lua.get_top().wrapping_sub(_orig_top)); }

				$ret_vals
			}
		)*
	};
}

#[macro_export]
macro_rules! lua_pub_methods {
	(static $export_name:ident => $($(#[$attr:meta])* unsafe fn $method_name:ident($lua:ident $(, $arg:ident:$arg_ty:ty)*) -> $ret_vals:literal $block:block)*) => {
		$crate::lua_methods! { $($(#[$attr])* unsafe fn $method_name($lua $(, $arg:$arg_ty)*) -> $ret_vals $block)* }
		static $export_name: &[(&str, $crate::lua_api::lua_CFunction)] = &[
			$(
				(stringify!($method_name), $method_name),
			)*
		];
	};
}
