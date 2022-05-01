// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use std::sync::Arc;

pub(crate) fn to_mlua_error(err: iced_x86::IcedError) -> mlua::Error {
	mlua::Error::ExternalError(Arc::new(err))
}
