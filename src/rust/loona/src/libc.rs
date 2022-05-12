// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

// The exported macros reference libc and we can't require the users to have
// libc as a dependency. The macros will reference the types from this module.

pub use ::libc::*;
