// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use std::env;
use std::path::PathBuf;

fn main() {
	if let Some(lua_lib_dir) = env::var_os("LUA_LIB_DIR") {
		let lua_lib_dir: PathBuf = lua_lib_dir.into();
		println!("cargo:rustc-link-search={}", lua_lib_dir.display());
	}

	let lua_lib_name: PathBuf = if let Some(lua_lib_name) = env::var_os("LUA_LIB_NAME") {
		lua_lib_name.into()
	} else {
		#[cfg(feature = "lua5_1")]
		{
			"lua5.1".into()
		}
		#[cfg(feature = "lua5_2")]
		{
			"lua5.2".into()
		}
		#[cfg(feature = "lua5_3")]
		{
			"lua5.3".into()
		}
		#[cfg(feature = "lua5_4")]
		{
			"lua5.4".into()
		}
	};
	println!("cargo:rustc-link-lib={}", lua_lib_name.display());
}
