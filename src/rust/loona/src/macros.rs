// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#[macro_export]
macro_rules! cstr {
	($s:expr) => {{
		const STR: &'static str = $s;
		$crate::lua::LuaCStr(concat!($s, "\0").as_bytes())
	}};
}
