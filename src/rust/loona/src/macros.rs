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
		unsafe extern "C" fn $modname(L: $crate::lua_api::lua_State) -> ::libc::c_int {
			const RET_VALS: ::libc::c_int = 1;
			let $lua = unsafe { $crate::lua::Lua::new(&L) };

			#[cfg(debug_assertions)]
			let _orig_top = unsafe { $lua.get_top() };

			$block

			#[cfg(debug_assertions)]
			unsafe {
				let _new_top = $lua.get_top();
				let _stack_usage = $lua.get_top().wrapping_sub(_orig_top);
				assert_eq!(RET_VALS, _stack_usage);
			}

			RET_VALS
		}
	};
}

#[macro_export]
macro_rules! lua_methods {
	($($(#[$attr:meta])* unsafe fn $method_name:ident($lua:ident) -> $ret_vals:literal $block:block)*) => {
		$(
			$(#[$attr])*
			#[allow(non_snake_case)]
			unsafe extern "C" fn $method_name(L: $crate::lua_api::lua_State) -> libc::c_int {
				let $lua = unsafe { $crate::lua::Lua::new(&L) };

				#[cfg(debug_assertions)]
				let _orig_top = unsafe { $lua.get_top() };

				$block

				#[cfg(debug_assertions)]
				unsafe {
					let _new_top = $lua.get_top();
					let _stack_usage = $lua.get_top().wrapping_sub(_orig_top);
					assert_eq!($ret_vals, _stack_usage);
				}

				$ret_vals
			}
		)*
	};
}

#[macro_export]
macro_rules! lua_pub_methods {
	(static $export_name:ident => $($(#[$attr:meta])* unsafe fn $method_name:ident($lua:ident) -> $ret_vals:literal $block:block)*) => {
		$crate::lua_methods! { $($(#[$attr])* unsafe fn $method_name($lua) -> $ret_vals $block)* }
		static $export_name: &[(&str, ::loona::lua_api::lua_CFunction)] = &[
			$(
				(stringify!($method_name), $method_name),
			)*
		];
	};
}
