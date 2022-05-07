// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

//! Low level access to the Lua C API

#[cfg(feature = "lua51")]
mod lua51;
#[cfg(feature = "lua52")]
mod lua52;
#[cfg(feature = "lua53")]
mod lua53;
#[cfg(feature = "lua54")]
mod lua54;

#[cfg(feature = "lua51")]
pub use crate::lua_api::lua51::*;
#[cfg(feature = "lua52")]
pub use crate::lua_api::lua52::*;
#[cfg(feature = "lua53")]
pub use crate::lua_api::lua53::*;
#[cfg(feature = "lua54")]
pub use crate::lua_api::lua54::*;
