// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use std::error::Error;
use std::fmt;

#[derive(Debug, Clone)]
#[allow(missing_copy_implementations)]
pub enum LuaError {
	MessageStr(&'static str),
}

impl Error for LuaError {}

impl fmt::Display for LuaError {
	#[inline]
	fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {
		match self {
			LuaError::MessageStr(s) => fmt::Display::fmt(s, f),
		}
	}
}
