// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

//! Low level access to the Lua C API

#[cfg(feature = "lua5_1")]
mod lua5_1;
#[cfg(feature = "lua5_2")]
mod lua5_2;
#[cfg(feature = "lua5_3")]
mod lua5_3;
#[cfg(feature = "lua5_4")]
mod lua5_4;

#[cfg(feature = "lua5_1")]
pub use crate::lua_api::lua5_1::*;
#[cfg(feature = "lua5_2")]
pub use crate::lua_api::lua5_2::*;
#[cfg(feature = "lua5_3")]
pub use crate::lua_api::lua5_3::*;
#[cfg(feature = "lua5_4")]
pub use crate::lua_api::lua5_4::*;
