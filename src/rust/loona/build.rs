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
		#[cfg(feature = "lua51")]
		{
			"lua5.1".into()
		}
	};
	println!("cargo:rustc-link-lib={}", lua_lib_name.display());
}
